using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DMMHelper;
using SGNFW.Common;
using SGNFW.Common.Json;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using UnityEngine;
using UnityEngine.Events;

namespace SGNFW.InAppPurchase
{
	public class Manager : Singleton<Manager>
	{
		public void UpdateProductList(List<PurchaseInfo> list)
		{
			this.m_productBaseList = list;
		}

		public List<PurchaseInfo> ProductBaseList
		{
			get
			{
				return this.m_productBaseList;
			}
		}

		public int residuePurchaseNum { get; private set; }

		public Manager.PurchaseConfig PurchaseConfigInfo
		{
			get
			{
				return this.m_purchaseConfig;
			}
		}

		public Manager.ConfirmDelegate PurchaseConfirmDelegate
		{
			get
			{
				return this.m_confirmDlgt;
			}
			set
			{
				this.m_confirmDlgt = value;
			}
		}

		public Manager.SuccessDelegate PurchaseSuccessDelegate { get; set; }

		public Manager.ErrorDelegate PurchaseErrorDelegate { get; set; }

		public NativePlugin.TransactionInfo CurrentTransactionInfo
		{
			get
			{
				return this.m_transactionInfo;
			}
			set
			{
				this.m_transactionInfo = value;
			}
		}

		public Action<string> SetupFailedCallback
		{
			get
			{
				return this.m_setupFailedCallback;
			}
			set
			{
				this.m_setupFailedCallback = value;
			}
		}

		public Action<NativePlugin.TransactionInfo?, Manager.PURCHASE_START_RESULT> PurchaseAbortedCallback
		{
			get
			{
				return this.m_purchaseAbortedCallback;
			}
			set
			{
				this.m_purchaseAbortedCallback = value;
			}
		}

		public Action<string> PurchaseDetectedCallback
		{
			get
			{
				return this.m_purchaseDetectedCallback;
			}
			set
			{
				this.m_purchaseDetectedCallback = value;
			}
		}

		public Manager.State state
		{
			get
			{
				return this.m_state;
			}
		}

		public int purchaseQueueCount
		{
			get
			{
				return this.m_purchasedQueue.Count;
			}
		}

		public List<string> NotFinishTransactionList
		{
			get
			{
				return this.m_notFinishTransactionList;
			}
		}

		public Manager.HttpRequestDelegate PurchaseHttpRequestFunc
		{
			get
			{
				return this.m_purchaseHttpRequestFunc;
			}
			set
			{
				this.m_purchaseHttpRequestFunc = value;
			}
		}

		public bool BlockReceipt
		{
			get
			{
				return this._blockReceipt;
			}
			set
			{
				this._blockReceipt = value;
				this.UpdateWaitPurchaseState();
			}
		}

		public bool IsNewReceiptQueued
		{
			get
			{
				return this.isNewReceiptQueued_;
			}
			private set
			{
				this.isNewReceiptQueued_ = value;
			}
		}

		public bool QueryPurchaseOnResume
		{
			get
			{
				return this.m_queryPurchaseOnResume;
			}
			set
			{
				this.m_queryPurchaseOnResume = value;
			}
		}

		private string MakeProductId(int mstId)
		{
			if (!this.serverMstId2ProductIdMap.ContainsKey(mstId))
			{
				return "";
			}
			return this.serverMstId2ProductIdMap[mstId];
		}

		private void Start()
		{
			this.changeState(Manager.State.WAIT);
		}

		private void Update()
		{
			switch (this.m_state)
			{
			case Manager.State.WAIT:
			case Manager.State.GET_PRODUCT:
			case Manager.State.CHECK_RECEIPT:
				break;
			case Manager.State.INIT:
				if (this.startGetProduct())
				{
					this.changeState(Manager.State.GET_PRODUCT);
					return;
				}
				break;
			case Manager.State.SETUP:
				if (this.IsSetupFinish() && NativePlugin.IsProductsDownloaded())
				{
					this.m_productInfo = new NativePlugin.ProductInfo[this.m_productBaseList.Count];
					for (int i = 0; i < this.m_productBaseList.Count; i++)
					{
						NativePlugin.GetProductInfo(this.MakeProductId(this.m_productBaseList[i].productIdCommon), ref this.m_productInfo[i]);
					}
					this.InsertIncompleteTransaction();
					this.m_prepared = true;
					this.changeState(Manager.State.MAIN);
					return;
				}
				if (this.IsSetupFailed())
				{
					this.changeState(Manager.State.ERROR);
					return;
				}
				break;
			case Manager.State.MAIN:
				this.checkPurchasedQueue();
				return;
			case Manager.State.ERROR:
				if (!this.IsSetupFailed())
				{
					if (!this.IsPrepared())
					{
						this.changeState(Manager.State.INIT);
					}
					else
					{
						this.changeState(Manager.State.MAIN);
					}
				}
				this.m_isPresentBoxSequence = true;
				break;
			default:
				return;
			}
		}

		public void StartInitialize(Manager.PurchaseConfig config, Manager.HttpRequestDelegate httpRequestFunc, Dictionary<int, string> mstId2ProductIdMap)
		{
			this.m_prepared = false;
			this.m_setupFinish = false;
			this.m_setupFailed = false;
			this.m_isPresentBoxSequence = true;
			this.m_purchaseConfig = config;
			this.PurchaseHttpRequestFunc = httpRequestFunc;
			this.serverMstId2ProductIdMap = mstId2ProductIdMap;
			this.changeState(Manager.State.INIT);
		}

		public void Restart()
		{
			this.m_isPresentBoxSequence = true;
			this.UpdateWaitPurchaseState();
			if (this.IsSetupFailed())
			{
				this.changeState(Manager.State.ERROR);
				return;
			}
			if (!this.IsPrepared())
			{
				this.changeState(Manager.State.INIT);
				return;
			}
			this.m_prepared = false;
			this.m_setupFinish = false;
			this.m_setupFailed = false;
			this.changeState(Manager.State.INIT);
		}

		public bool IsPrepared()
		{
			return this.m_prepared;
		}

		public bool IsSetupFinish()
		{
			return this.m_setupFinish;
		}

		public bool IsSetupFailed()
		{
			return this.m_setupFailed;
		}

		public bool IsWaitingForPurchase()
		{
			return false;
		}

		public bool CanMakePayments()
		{
			return NativePlugin.CanMakePayments();
		}

		public uint GetTransactionNum()
		{
			return NativePlugin.GetTransactionNum();
		}

		public uint GetLeftTransactionNum()
		{
			return (uint)this.m_leftTransactionList.Count;
		}

		public void RequestProductsDataExternal()
		{
			this.InitInAppPurchase_Core();
		}

		private void _getProductInfo_core(ref List<PurchaseInfo> info, Func<PurchaseInfo, NativePlugin.ProductInfo, bool> predicate)
		{
			info.Clear();
			if (this.m_productInfo != null)
			{
				NativePlugin.ProductInfo[] productInfo = this.m_productInfo;
				for (int i = 0; i < productInfo.Length; i++)
				{
					NativePlugin.ProductInfo store_product_data = productInfo[i];
					if (store_product_data.valid)
					{
						PurchaseInfo purchaseInfo = Array.Find<PurchaseInfo>(this.m_productBaseList.ToArray(), (PurchaseInfo t) => this.MakeProductId(t.productIdCommon) == store_product_data.productID);
						if (purchaseInfo != null && predicate(purchaseInfo, store_product_data))
						{
							info.Add(purchaseInfo);
						}
					}
				}
			}
		}

		public void GetProductInfo(ref List<PurchaseInfo> info)
		{
			this._getProductInfo_core(ref info, (PurchaseInfo server_product_data, NativePlugin.ProductInfo store_product_data) => true);
		}

		public void GetProductInfo(ref NativePlugin.ProductInfo info, string productID)
		{
			info.valid = false;
			for (int i = 0; i < this.m_productInfo.Length; i++)
			{
				if (this.m_productInfo[i].productID == productID)
				{
					info = this.m_productInfo[i];
					return;
				}
			}
		}

		public bool SelectProduct(string product_id)
		{
			if (this.IsWaitingForPurchase())
			{
				return false;
			}
			bool flag = this.StartPurchase(product_id, delegate(string productID, string devPayload)
			{
				if (!NativePlugin.SelectProduct(productID, devPayload))
				{
					Verbose<SGNFW.InAppPurchase.Verbose>.LogError("start purchase failed", null);
					this.UpdateWaitPurchaseState();
					return false;
				}
				return true;
			}, delegate(Manager.PURCHASE_START_RESULT result_status)
			{
				this.m_isPresentBoxSequence = true;
				this.UpdateWaitPurchaseState();
			});
			if (flag)
			{
				this.m_waitPurchase = true;
				this.m_isPresentBoxSequence = false;
			}
			return flag;
		}

		public bool SelectProductByIndex(int index)
		{
			return this.SelectProduct(this.m_productInfo[index].productID);
		}

		protected void SetupFinishedMessage(string dummy)
		{
			this.m_setupFinish = true;
			foreach (PurchaseInfo purchaseInfo in this.m_productBaseList)
			{
				NativePlugin.AddProductID(this.MakeProductId(purchaseInfo.productIdCommon));
			}
			NativePlugin.RequestProductsData();
		}

		protected void SetupFailedMessage(string message)
		{
			this.m_setupFailed = true;
			if (this.SetupFailedCallback != null)
			{
				this.SetupFailedCallback(message);
			}
		}

		protected void UpdateProductListMessage(string res)
		{
			NativePlugin.UpdateProductList(WWW.UnEscapeURL(res));
		}

		protected void PurchasedMessage(string jsonStr)
		{
			Manager.QueueItem queueItem = Data.ToObject<Manager.QueueItem>(jsonStr);
			this.queuePurchasedReceipt(queueItem);
		}

		protected void FailedMessage(string jsonStr)
		{
			Manager.QueueItem queueItem = Data.ToObject<Manager.QueueItem>(jsonStr);
			this.finishTransaction(queueItem.transaction_id);
			if (this.PurchaseAbortedCallback != null)
			{
				NativePlugin.TransactionInfo transactionInfo = default(NativePlugin.TransactionInfo);
				NativePlugin.GetTransactionInfo(queueItem.transaction_id, ref transactionInfo);
				this.PurchaseAbortedCallback(new NativePlugin.TransactionInfo?(transactionInfo), Manager.PURCHASE_START_RESULT.ABORTED);
			}
			this.UpdateWaitPurchaseState();
		}

		protected void RestoreMessage(string jsonStr)
		{
			Manager.QueueItem queueItem = Data.ToObject<Manager.QueueItem>(jsonStr);
			this.queuePurchasedReceipt(queueItem);
		}

		protected void DeferredMessage(string jsonStr)
		{
			Manager.QueueItem queueItem = Data.ToObject<Manager.QueueItem>(jsonStr);
			if (this.PurchaseAbortedCallback != null)
			{
				NativePlugin.TransactionInfo transactionInfo = default(NativePlugin.TransactionInfo);
				NativePlugin.GetTransactionInfo(queueItem.transaction_id, ref transactionInfo);
				this.PurchaseAbortedCallback(new NativePlugin.TransactionInfo?(transactionInfo), Manager.PURCHASE_START_RESULT.DEFERRED);
			}
			this.removeTransaction(queueItem.transaction_id);
			this.UpdateWaitPurchaseState();
		}

		protected void PurchaseDetectedMessage(string jsonStr)
		{
			Manager.QueueItem queueItem = Data.ToObject<Manager.QueueItem>(jsonStr);
			foreach (Manager.QueueItem queueItem2 in this.m_purchasedQueue)
			{
				if (queueItem.transaction_id == queueItem2.transaction_id)
				{
					return;
				}
			}
			if (this.m_transactionInfo.isValid() && this.m_transactionInfo.transactionID == queueItem.transaction_id)
			{
				return;
			}
			this.queuePurchasedReceipt(queueItem);
			if (this.PurchaseDetectedCallback != null)
			{
				this.PurchaseDetectedCallback(queueItem.product_id);
			}
		}

		public bool FetchProductList(Action<List<PurchaseInfo>, int> successCallback, Action failureCallback)
		{
			this.FetchProductListSuccessCallback = successCallback;
			this.FetchProductListFailureCallback = failureCallback;
			this.PurchaseHttpRequestFunc(PurchaseInfoCmd.Create(null, false), new Action<Command, int, string>(this.productList_dlgt));
			return true;
		}

		public bool StartPurchase(string productID, Func<string, string, bool> successCallback, Action<Manager.PURCHASE_START_RESULT> failureCallback)
		{
			this.StartPurchaseSuccessCallback = successCallback;
			this.StartPurchaseFailureCallback = failureCallback;
			string tmp = productID;
			base.StartCoroutine(this.WaitAction(delegate
			{
				this.startPurchaseCommon(false, tmp, "abcd", productID);
			}));
			return true;
		}

		public bool CompletePurchase(string productID, string transactionId, string receiptEncoded, List<string> notFinishTransactionList, bool isGooglePromoProduct, Action successCallback, Action<Manager.PURCHASE_COMPLETE_RESULT> failureCallback)
		{
			this.CompletePurchaseSuccessCallback = successCallback;
			this.CompletePurchaseFailureCallback = failureCallback;
			this.PurchaseHttpRequestFunc(PurchaseCmd.Create(productID, transactionId, receiptEncoded, "", Singleton<DMMHelpManager>.Instance.VewerID, Singleton<DMMHelpManager>.Instance.OnetimeToken, notFinishTransactionList), new Action<Command, int, string>(this.completePurchase_dlgt));
			return true;
		}

		private void productList_dlgt(Command cmd, int res_code, string error_msg)
		{
			if (res_code == 0)
			{
				PurchaseInfoResponse purchaseInfoResponse = cmd.response as PurchaseInfoResponse;
				this.FetchProductListSuccessCallback(purchaseInfoResponse.purchaseInfoList, purchaseInfoResponse.residuePurchaseNum);
				return;
			}
			this.FetchProductListFailureCallback();
		}

		private void startPurchaseCommon(bool need_parental_confirm, string item_id, string uniq_id, string productIdString)
		{
			if (!need_parental_confirm && !this.StartPurchaseSuccessCallback(item_id, uniq_id) && this.PurchaseErrorDelegate != null)
			{
				this.PurchaseErrorDelegate(null, -1, productIdString, new Manager.InAppPurchaseException(1, ""));
			}
		}

		private void completePurchase_dlgt(Command cmd, int res_code, string error_msg)
		{
			if (res_code == 0)
			{
				PurchaseResponse purchaseResponse = cmd.response as PurchaseResponse;
				PurchaseRequest purchaseRequest = cmd.request as PurchaseRequest;
				NativePlugin.TransactionInfo currentTransactionInfo = this.CurrentTransactionInfo;
				this.CompletePurchaseSuccessCallback();
				if (this.PurchaseSuccessDelegate != null)
				{
					this.PurchaseSuccessDelegate(purchaseResponse, purchaseRequest);
					return;
				}
			}
			else
			{
				PurchaseRequest purchaseRequest2 = cmd.request as PurchaseRequest;
				this.completePurchase_ErrHandler(res_code, error_msg, purchaseRequest2.productId);
			}
		}

		private IEnumerator WaitAction(UnityAction action)
		{
			yield return null;
			action();
			yield break;
		}

		private void completePurchase_ErrHandler(int res_code, string error_msg, string reqProductIdString)
		{
			NativePlugin.TransactionInfo currentTransactionInfo = this.CurrentTransactionInfo;
			Manager.PURCHASE_COMPLETE_RESULT purchase_COMPLETE_RESULT = Manager.PURCHASE_COMPLETE_RESULT.FAILURE;
			if (res_code == 1112)
			{
				purchase_COMPLETE_RESULT = Manager.PURCHASE_COMPLETE_RESULT.FAILURE;
			}
			if (res_code == 1100 || res_code == 1102 || res_code == 1111 || res_code == 1113)
			{
				purchase_COMPLETE_RESULT = Manager.PURCHASE_COMPLETE_RESULT.INVALID_RECEIPT;
			}
			else if (res_code == 302)
			{
				purchase_COMPLETE_RESULT = Manager.PURCHASE_COMPLETE_RESULT.RECEIPT_FOR_OTHER_ACCOUNT;
			}
			this.CompletePurchaseFailureCallback(purchase_COMPLETE_RESULT);
			if (this.PurchaseErrorDelegate != null)
			{
				this.PurchaseErrorDelegate(new NativePlugin.TransactionInfo?(currentTransactionInfo), res_code, reqProductIdString, new Manager.InAppPurchaseException(3, error_msg));
			}
		}

		private void queuePurchasedReceipt(Manager.QueueItem queueItem)
		{
			if (!this.m_purchasedQueue.Contains(queueItem))
			{
				this.m_purchasedQueue.Enqueue(queueItem);
				this.IsNewReceiptQueued = true;
			}
			if (!this.m_notFinishTransactionList.Contains(queueItem.transaction_id))
			{
				this.m_notFinishTransactionList.Add(queueItem.transaction_id);
			}
		}

		protected void changeState(Manager.State st)
		{
			this.m_state = st;
		}

		protected bool findLeftTransactionID(string id)
		{
			using (List<string>.Enumerator enumerator = this.m_leftTransactionList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == id)
					{
						return true;
					}
				}
			}
			return false;
		}

		protected bool startGetProduct()
		{
			return this.FetchProductList(delegate(List<PurchaseInfo> list, int residuePurchaseNum)
			{
				this.m_productBaseList = list;
				int count = list.Count;
				this.residuePurchaseNum = residuePurchaseNum;
				this.InitInAppPurchase_Core();
			}, delegate
			{
				this.changeState(Manager.State.ERROR);
			});
		}

		private void InitInAppPurchase_Core()
		{
			if (!this.m_prepared && this.m_productBaseList != null)
			{
				this.m_setupFinish = false;
				this.m_setupFailed = false;
				NativePlugin.InitInfo initInfo;
				initInfo.gameObjectName = base.gameObject.name;
				initInfo.cbSetupFinished = "SetupFinishedMessage";
				initInfo.cbSetupFailed = "SetupFailedMessage";
				initInfo.cbUpdateProductList = "UpdateProductListMessage";
				initInfo.cbPurchaseSuccess = "PurchasedMessage";
				initInfo.cbPurchaseFailure = "FailedMessage";
				initInfo.cbPurchaseRestore = "RestoreMessage";
				initInfo.cbPurchaseDeferred = "DeferredMessage";
				initInfo.cbPurchaseDetected = "PurchaseDetectedMessage";
				initInfo.queryPurchaseOnResume = this.m_queryPurchaseOnResume;
				NativePlugin.Create(initInfo);
				this.changeState(Manager.State.SETUP);
			}
		}

		protected bool startCpBuy()
		{
			string @string = Encoding.ASCII.GetString(Encoding.UTF8.GetBytes(this.m_transactionInfo.receipt));
			string text;
			if (this.m_transactionInfo.isBase64Encoded != 0)
			{
				text = @string;
			}
			else
			{
				text = Convert.ToBase64String(Encoding.ASCII.GetBytes(@string));
			}
			if (!this.m_isPresentBoxSequence)
			{
				this.findLeftTransactionID(this.m_transactionInfo.transactionID);
			}
			return this.CompletePurchase(this.m_transactionInfo.productID, this.m_transactionInfo.transactionID, text, this.m_notFinishTransactionList, false, delegate
			{
				this.finishTransaction(this.m_transactionInfo.transactionID);
				this.m_transactionInfo.clear();
				this.changeState(Manager.State.MAIN);
				this.m_isPresentBoxSequence = true;
				this.UpdateWaitPurchaseState();
			}, delegate(Manager.PURCHASE_COMPLETE_RESULT result_status)
			{
				this.finishTransaction(this.m_transactionInfo.transactionID);
				this.m_transactionInfo.clear();
				this.changeState(Manager.State.MAIN);
				this.m_isPresentBoxSequence = true;
				this.UpdateWaitPurchaseState();
			});
		}

		protected void finishTransaction(string id)
		{
			NativePlugin.FinishTransaction(id);
			this.removeTransaction(id);
		}

		protected void removeTransaction(string id)
		{
			this.m_leftTransactionList.Remove(id);
		}

		protected void checkPurchasedQueue()
		{
			if (!this.BlockReceipt)
			{
				if (!this.m_transactionInfo.isValid())
				{
					while (this.m_purchasedQueue.Count > 0)
					{
						Manager.QueueItem queueItem = this.m_purchasedQueue.Dequeue();
						this.m_transactionInfo.clear();
						if (NativePlugin.GetTransactionInfo(queueItem.transaction_id, ref this.m_transactionInfo))
						{
							break;
						}
						this.m_transactionInfo.clear();
					}
				}
				if (this.m_transactionInfo.isValid() && this.startCpBuy())
				{
					this.changeState(Manager.State.CHECK_RECEIPT);
				}
			}
			else
			{
				this.UpdateWaitPurchaseState();
				if (this.IsNewReceiptQueued && this.PurchaseAbortedCallback != null)
				{
					this.PurchaseAbortedCallback(new NativePlugin.TransactionInfo?(this.m_transactionInfo), Manager.PURCHASE_START_RESULT.BLOCKED);
				}
			}
			this.IsNewReceiptQueued = false;
		}

		public void InsertIncompleteTransaction()
		{
			uint transactionNum = NativePlugin.GetTransactionNum();
			this.m_notFinishTransactionList = new List<string>();
			if (transactionNum > 0U)
			{
				int num = 0;
				while ((long)num < (long)((ulong)transactionNum))
				{
					string transactionID = NativePlugin.GetTransactionID(num);
					if (!string.IsNullOrEmpty(transactionID) && !this.findLeftTransactionID(transactionID))
					{
						this.m_leftTransactionList.Add(transactionID);
						this.m_notFinishTransactionList.Add(transactionID);
					}
					num++;
				}
			}
			this.UpdateWaitPurchaseState();
		}

		private void UpdateWaitPurchaseState()
		{
			this.m_waitPurchase = this.m_purchasedQueue.Count > 0 && !this.BlockReceipt;
		}

		protected override void OnSingletonAwake()
		{
			Verbose<SGNFW.InAppPurchase.Verbose>.Enabled = true;
			this.m_purchasedQueue = new Queue<Manager.QueueItem>();
			this.m_leftTransactionList = new List<string>();
			this.m_notFinishTransactionList = new List<string>();
		}

		protected override void OnSingletonDestroy()
		{
			NativePlugin.Destroy();
		}

		protected bool m_prepared;

		protected bool m_setupFinish;

		protected bool m_setupFailed;

		protected bool m_waitPurchase;

		protected bool m_isPresentBoxSequence;

		protected bool m_queryPurchaseOnResume;

		protected Manager.State m_state;

		protected List<PurchaseInfo> m_productBaseList;

		protected NativePlugin.ProductInfo[] m_productInfo;

		protected NativePlugin.TransactionInfo m_transactionInfo;

		protected Manager.QueueItem m_cancelInfo;

		protected Queue<Manager.QueueItem> m_purchasedQueue;

		protected List<string> m_leftTransactionList;

		protected List<string> m_notFinishTransactionList;

		protected Action<string> m_setupFailedCallback;

		protected Action<NativePlugin.TransactionInfo?, Manager.PURCHASE_START_RESULT> m_purchaseAbortedCallback;

		protected Action<string> m_purchaseDetectedCallback;

		protected Manager.HttpRequestDelegate m_purchaseHttpRequestFunc;

		protected Dictionary<int, string> serverMstId2ProductIdMap;

		private Action<List<PurchaseInfo>, int> FetchProductListSuccessCallback;

		private Action FetchProductListFailureCallback;

		private Func<string, string, bool> StartPurchaseSuccessCallback;

		private Action<Manager.PURCHASE_START_RESULT> StartPurchaseFailureCallback;

		private Action CompletePurchaseSuccessCallback;

		private Action<Manager.PURCHASE_COMPLETE_RESULT> CompletePurchaseFailureCallback;

		protected Manager.PurchaseConfig m_purchaseConfig;

		protected Manager.ConfirmDelegate m_confirmDlgt;

		private bool _blockReceipt;

		private bool isNewReceiptQueued_;

		public enum PURCHASE_START_RESULT
		{
			FAILURE,
			REJECTED,
			ABORTED,
			DEFERRED,
			BLOCKED,
			SUCCESS
		}

		public enum PURCHASE_COMPLETE_RESULT
		{
			FAILURE,
			INVALID_RECEIPT,
			RECEIPT_FOR_OTHER_ACCOUNT,
			SUCCESS
		}

		public enum State
		{
			WAIT,
			INIT,
			GET_PRODUCT,
			SETUP,
			MAIN,
			CHECK_RECEIPT,
			ERROR
		}

		public class QueueItem : IComparable
		{
			public QueueItem()
			{
			}

			public QueueItem(string product_id, string transaction_id)
			{
				this.product_id = product_id;
				this.transaction_id = transaction_id;
			}

			public int CompareTo(object obj)
			{
				if (obj == null)
				{
					return 1;
				}
				Manager.QueueItem queueItem = obj as Manager.QueueItem;
				if (queueItem == null)
				{
					throw new ArgumentException("Object is not QueueItem");
				}
				int num = this.product_id.CompareTo(queueItem.product_id);
				if (num != 0)
				{
					return num;
				}
				int num2 = this.transaction_id.CompareTo(queueItem.transaction_id);
				if (num2 != 0)
				{
					return num2;
				}
				return 0;
			}

			public override string ToString()
			{
				return this.product_id + "," + this.transaction_id;
			}

			public string product_id;

			public string transaction_id;
		}

		public delegate void HttpRequestDelegate(Command cmd, Action<Command, int, string> finished);

		public delegate void ConfirmAcceptDelegate(string uniq_id);

		public delegate void ConfirmDenyDelegate(string uniq_id);

		public delegate void ConfirmDelegate(NativePlugin.ProductInfo pinfo, string uniq_id, Manager.ConfirmAcceptDelegate acceptDlgt, Manager.ConfirmDenyDelegate denyDlgt);

		public delegate void SuccessDelegate(PurchaseResponse response, PurchaseRequest request);

		public delegate void ErrorDelegate(NativePlugin.TransactionInfo? tinfo, int error_code, string reqProductIdString, Exception exception);

		public class PurchaseConfig
		{
			public int language;

			public int platform_id;

			public int store_id;

			public string bundle_id;
		}

		public class InAppPurchaseException : Exception
		{
			public InAppPurchaseException(int errorCode, string msg)
				: base(msg)
			{
				this.ErrorCode = errorCode;
			}

			public const int ERRCODE_IAP_SELECTPRODUCT = 1;

			public const int ERRCODE_IAP_STARTPURCHASE = 2;

			public const int ERRCODE_IAP_FAILEDMESSAGE = 3;

			public const int ERRCODE_IAP_DEFERREDMESSAGE = 4;

			public const int ERRCODE_IAP_SETUPFAILED = 5;

			public const int ERRCODE_IAP_NEEDCHECKAGE = 6;

			public const int ERRCODE_IAP_LIMITEDPURCHASE = 7;

			public int ErrorCode;
		}
	}
}
