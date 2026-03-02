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
	// Token: 0x02000249 RID: 585
	public class Manager : Singleton<Manager>
	{
		// Token: 0x0600248D RID: 9357 RVA: 0x0019CC70 File Offset: 0x0019AE70
		public void UpdateProductList(List<PurchaseInfo> list)
		{
			this.m_productBaseList = list;
		}

		// Token: 0x17000561 RID: 1377
		// (get) Token: 0x0600248E RID: 9358 RVA: 0x0019CC79 File Offset: 0x0019AE79
		public List<PurchaseInfo> ProductBaseList
		{
			get
			{
				return this.m_productBaseList;
			}
		}

		// Token: 0x17000562 RID: 1378
		// (get) Token: 0x0600248F RID: 9359 RVA: 0x0019CC81 File Offset: 0x0019AE81
		// (set) Token: 0x06002490 RID: 9360 RVA: 0x0019CC89 File Offset: 0x0019AE89
		public int residuePurchaseNum { get; private set; }

		// Token: 0x17000563 RID: 1379
		// (get) Token: 0x06002491 RID: 9361 RVA: 0x0019CC92 File Offset: 0x0019AE92
		public Manager.PurchaseConfig PurchaseConfigInfo
		{
			get
			{
				return this.m_purchaseConfig;
			}
		}

		// Token: 0x17000564 RID: 1380
		// (get) Token: 0x06002492 RID: 9362 RVA: 0x0019CC9A File Offset: 0x0019AE9A
		// (set) Token: 0x06002493 RID: 9363 RVA: 0x0019CCA2 File Offset: 0x0019AEA2
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

		// Token: 0x17000565 RID: 1381
		// (get) Token: 0x06002494 RID: 9364 RVA: 0x0019CCAB File Offset: 0x0019AEAB
		// (set) Token: 0x06002495 RID: 9365 RVA: 0x0019CCB3 File Offset: 0x0019AEB3
		public Manager.SuccessDelegate PurchaseSuccessDelegate { get; set; }

		// Token: 0x17000566 RID: 1382
		// (get) Token: 0x06002496 RID: 9366 RVA: 0x0019CCBC File Offset: 0x0019AEBC
		// (set) Token: 0x06002497 RID: 9367 RVA: 0x0019CCC4 File Offset: 0x0019AEC4
		public Manager.ErrorDelegate PurchaseErrorDelegate { get; set; }

		// Token: 0x17000567 RID: 1383
		// (get) Token: 0x06002498 RID: 9368 RVA: 0x0019CCCD File Offset: 0x0019AECD
		// (set) Token: 0x06002499 RID: 9369 RVA: 0x0019CCD5 File Offset: 0x0019AED5
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

		// Token: 0x17000568 RID: 1384
		// (get) Token: 0x0600249A RID: 9370 RVA: 0x0019CCDE File Offset: 0x0019AEDE
		// (set) Token: 0x0600249B RID: 9371 RVA: 0x0019CCE6 File Offset: 0x0019AEE6
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

		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x0600249C RID: 9372 RVA: 0x0019CCEF File Offset: 0x0019AEEF
		// (set) Token: 0x0600249D RID: 9373 RVA: 0x0019CCF7 File Offset: 0x0019AEF7
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

		// Token: 0x1700056A RID: 1386
		// (get) Token: 0x0600249E RID: 9374 RVA: 0x0019CD00 File Offset: 0x0019AF00
		// (set) Token: 0x0600249F RID: 9375 RVA: 0x0019CD08 File Offset: 0x0019AF08
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

		// Token: 0x1700056B RID: 1387
		// (get) Token: 0x060024A0 RID: 9376 RVA: 0x0019CD11 File Offset: 0x0019AF11
		public Manager.State state
		{
			get
			{
				return this.m_state;
			}
		}

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x060024A1 RID: 9377 RVA: 0x0019CD19 File Offset: 0x0019AF19
		public int purchaseQueueCount
		{
			get
			{
				return this.m_purchasedQueue.Count;
			}
		}

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x060024A2 RID: 9378 RVA: 0x0019CD26 File Offset: 0x0019AF26
		public List<string> NotFinishTransactionList
		{
			get
			{
				return this.m_notFinishTransactionList;
			}
		}

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x060024A3 RID: 9379 RVA: 0x0019CD2E File Offset: 0x0019AF2E
		// (set) Token: 0x060024A4 RID: 9380 RVA: 0x0019CD36 File Offset: 0x0019AF36
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

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x060024A5 RID: 9381 RVA: 0x0019CD3F File Offset: 0x0019AF3F
		// (set) Token: 0x060024A6 RID: 9382 RVA: 0x0019CD47 File Offset: 0x0019AF47
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

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x060024A7 RID: 9383 RVA: 0x0019CD56 File Offset: 0x0019AF56
		// (set) Token: 0x060024A8 RID: 9384 RVA: 0x0019CD5E File Offset: 0x0019AF5E
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

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x060024A9 RID: 9385 RVA: 0x0019CD67 File Offset: 0x0019AF67
		// (set) Token: 0x060024AA RID: 9386 RVA: 0x0019CD6F File Offset: 0x0019AF6F
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

		// Token: 0x060024AB RID: 9387 RVA: 0x0019CD78 File Offset: 0x0019AF78
		private string MakeProductId(int mstId)
		{
			if (!this.serverMstId2ProductIdMap.ContainsKey(mstId))
			{
				return "";
			}
			return this.serverMstId2ProductIdMap[mstId];
		}

		// Token: 0x060024AC RID: 9388 RVA: 0x0019CD9A File Offset: 0x0019AF9A
		private void Start()
		{
			this.changeState(Manager.State.WAIT);
		}

		// Token: 0x060024AD RID: 9389 RVA: 0x0019CDA4 File Offset: 0x0019AFA4
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

		// Token: 0x060024AE RID: 9390 RVA: 0x0019CEA5 File Offset: 0x0019B0A5
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

		// Token: 0x060024AF RID: 9391 RVA: 0x0019CEE0 File Offset: 0x0019B0E0
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

		// Token: 0x060024B0 RID: 9392 RVA: 0x0019CF36 File Offset: 0x0019B136
		public bool IsPrepared()
		{
			return this.m_prepared;
		}

		// Token: 0x060024B1 RID: 9393 RVA: 0x0019CF3E File Offset: 0x0019B13E
		public bool IsSetupFinish()
		{
			return this.m_setupFinish;
		}

		// Token: 0x060024B2 RID: 9394 RVA: 0x0019CF46 File Offset: 0x0019B146
		public bool IsSetupFailed()
		{
			return this.m_setupFailed;
		}

		// Token: 0x060024B3 RID: 9395 RVA: 0x0019CF4E File Offset: 0x0019B14E
		public bool IsWaitingForPurchase()
		{
			return false;
		}

		// Token: 0x060024B4 RID: 9396 RVA: 0x0019CF51 File Offset: 0x0019B151
		public bool CanMakePayments()
		{
			return NativePlugin.CanMakePayments();
		}

		// Token: 0x060024B5 RID: 9397 RVA: 0x0019CF58 File Offset: 0x0019B158
		public uint GetTransactionNum()
		{
			return NativePlugin.GetTransactionNum();
		}

		// Token: 0x060024B6 RID: 9398 RVA: 0x0019CF5F File Offset: 0x0019B15F
		public uint GetLeftTransactionNum()
		{
			return (uint)this.m_leftTransactionList.Count;
		}

		// Token: 0x060024B7 RID: 9399 RVA: 0x0019CF6C File Offset: 0x0019B16C
		public void RequestProductsDataExternal()
		{
			this.InitInAppPurchase_Core();
		}

		// Token: 0x060024B8 RID: 9400 RVA: 0x0019CF74 File Offset: 0x0019B174
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

		// Token: 0x060024B9 RID: 9401 RVA: 0x0019D003 File Offset: 0x0019B203
		public void GetProductInfo(ref List<PurchaseInfo> info)
		{
			this._getProductInfo_core(ref info, (PurchaseInfo server_product_data, NativePlugin.ProductInfo store_product_data) => true);
		}

		// Token: 0x060024BA RID: 9402 RVA: 0x0019D02C File Offset: 0x0019B22C
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

		// Token: 0x060024BB RID: 9403 RVA: 0x0019D07F File Offset: 0x0019B27F
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

		// Token: 0x060024BC RID: 9404 RVA: 0x0019D0BB File Offset: 0x0019B2BB
		public bool SelectProductByIndex(int index)
		{
			return this.SelectProduct(this.m_productInfo[index].productID);
		}

		// Token: 0x060024BD RID: 9405 RVA: 0x0019D0D4 File Offset: 0x0019B2D4
		protected void SetupFinishedMessage(string dummy)
		{
			this.m_setupFinish = true;
			foreach (PurchaseInfo purchaseInfo in this.m_productBaseList)
			{
				NativePlugin.AddProductID(this.MakeProductId(purchaseInfo.productIdCommon));
			}
			NativePlugin.RequestProductsData();
		}

		// Token: 0x060024BE RID: 9406 RVA: 0x0019D140 File Offset: 0x0019B340
		protected void SetupFailedMessage(string message)
		{
			this.m_setupFailed = true;
			if (this.SetupFailedCallback != null)
			{
				this.SetupFailedCallback(message);
			}
		}

		// Token: 0x060024BF RID: 9407 RVA: 0x0019D15D File Offset: 0x0019B35D
		protected void UpdateProductListMessage(string res)
		{
			NativePlugin.UpdateProductList(WWW.UnEscapeURL(res));
		}

		// Token: 0x060024C0 RID: 9408 RVA: 0x0019D16C File Offset: 0x0019B36C
		protected void PurchasedMessage(string jsonStr)
		{
			Manager.QueueItem queueItem = Data.ToObject<Manager.QueueItem>(jsonStr);
			this.queuePurchasedReceipt(queueItem);
		}

		// Token: 0x060024C1 RID: 9409 RVA: 0x0019D188 File Offset: 0x0019B388
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

		// Token: 0x060024C2 RID: 9410 RVA: 0x0019D1E0 File Offset: 0x0019B3E0
		protected void RestoreMessage(string jsonStr)
		{
			Manager.QueueItem queueItem = Data.ToObject<Manager.QueueItem>(jsonStr);
			this.queuePurchasedReceipt(queueItem);
		}

		// Token: 0x060024C3 RID: 9411 RVA: 0x0019D1FC File Offset: 0x0019B3FC
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

		// Token: 0x060024C4 RID: 9412 RVA: 0x0019D254 File Offset: 0x0019B454
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

		// Token: 0x060024C5 RID: 9413 RVA: 0x0019D304 File Offset: 0x0019B504
		public bool FetchProductList(Action<List<PurchaseInfo>, int> successCallback, Action failureCallback)
		{
			this.FetchProductListSuccessCallback = successCallback;
			this.FetchProductListFailureCallback = failureCallback;
			this.PurchaseHttpRequestFunc(PurchaseInfoCmd.Create(null, false), new Action<Command, int, string>(this.productList_dlgt));
			return true;
		}

		// Token: 0x060024C6 RID: 9414 RVA: 0x0019D334 File Offset: 0x0019B534
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

		// Token: 0x060024C7 RID: 9415 RVA: 0x0019D38C File Offset: 0x0019B58C
		public bool CompletePurchase(string productID, string transactionId, string receiptEncoded, List<string> notFinishTransactionList, bool isGooglePromoProduct, Action successCallback, Action<Manager.PURCHASE_COMPLETE_RESULT> failureCallback)
		{
			this.CompletePurchaseSuccessCallback = successCallback;
			this.CompletePurchaseFailureCallback = failureCallback;
			this.PurchaseHttpRequestFunc(PurchaseCmd.Create(productID, transactionId, receiptEncoded, "", Singleton<DMMHelpManager>.Instance.VewerID, Singleton<DMMHelpManager>.Instance.OnetimeToken, notFinishTransactionList), new Action<Command, int, string>(this.completePurchase_dlgt));
			return true;
		}

		// Token: 0x060024C8 RID: 9416 RVA: 0x0019D3E4 File Offset: 0x0019B5E4
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

		// Token: 0x060024C9 RID: 9417 RVA: 0x0019D424 File Offset: 0x0019B624
		private void startPurchaseCommon(bool need_parental_confirm, string item_id, string uniq_id, string productIdString)
		{
			if (!need_parental_confirm && !this.StartPurchaseSuccessCallback(item_id, uniq_id) && this.PurchaseErrorDelegate != null)
			{
				this.PurchaseErrorDelegate(null, -1, productIdString, new Manager.InAppPurchaseException(1, ""));
			}
		}

		// Token: 0x060024CA RID: 9418 RVA: 0x0019D470 File Offset: 0x0019B670
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

		// Token: 0x060024CB RID: 9419 RVA: 0x0019D4DA File Offset: 0x0019B6DA
		private IEnumerator WaitAction(UnityAction action)
		{
			yield return null;
			action();
			yield break;
		}

		// Token: 0x060024CC RID: 9420 RVA: 0x0019D4EC File Offset: 0x0019B6EC
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

		// Token: 0x060024CD RID: 9421 RVA: 0x0019D568 File Offset: 0x0019B768
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

		// Token: 0x060024CE RID: 9422 RVA: 0x0019D5BA File Offset: 0x0019B7BA
		protected void changeState(Manager.State st)
		{
			this.m_state = st;
		}

		// Token: 0x060024CF RID: 9423 RVA: 0x0019D5C4 File Offset: 0x0019B7C4
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

		// Token: 0x060024D0 RID: 9424 RVA: 0x0019D620 File Offset: 0x0019B820
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

		// Token: 0x060024D1 RID: 9425 RVA: 0x0019D640 File Offset: 0x0019B840
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

		// Token: 0x060024D2 RID: 9426 RVA: 0x0019D700 File Offset: 0x0019B900
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

		// Token: 0x060024D3 RID: 9427 RVA: 0x0019D7A5 File Offset: 0x0019B9A5
		protected void finishTransaction(string id)
		{
			NativePlugin.FinishTransaction(id);
			this.removeTransaction(id);
		}

		// Token: 0x060024D4 RID: 9428 RVA: 0x0019D7B5 File Offset: 0x0019B9B5
		protected void removeTransaction(string id)
		{
			this.m_leftTransactionList.Remove(id);
		}

		// Token: 0x060024D5 RID: 9429 RVA: 0x0019D7C4 File Offset: 0x0019B9C4
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

		// Token: 0x060024D6 RID: 9430 RVA: 0x0019D87C File Offset: 0x0019BA7C
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

		// Token: 0x060024D7 RID: 9431 RVA: 0x0019D8E2 File Offset: 0x0019BAE2
		private void UpdateWaitPurchaseState()
		{
			this.m_waitPurchase = this.m_purchasedQueue.Count > 0 && !this.BlockReceipt;
		}

		// Token: 0x060024D8 RID: 9432 RVA: 0x0019D904 File Offset: 0x0019BB04
		protected override void OnSingletonAwake()
		{
			Verbose<SGNFW.InAppPurchase.Verbose>.Enabled = true;
			this.m_purchasedQueue = new Queue<Manager.QueueItem>();
			this.m_leftTransactionList = new List<string>();
			this.m_notFinishTransactionList = new List<string>();
		}

		// Token: 0x060024D9 RID: 9433 RVA: 0x0019D92D File Offset: 0x0019BB2D
		protected override void OnSingletonDestroy()
		{
			NativePlugin.Destroy();
		}

		// Token: 0x04001B41 RID: 6977
		protected bool m_prepared;

		// Token: 0x04001B42 RID: 6978
		protected bool m_setupFinish;

		// Token: 0x04001B43 RID: 6979
		protected bool m_setupFailed;

		// Token: 0x04001B44 RID: 6980
		protected bool m_waitPurchase;

		// Token: 0x04001B45 RID: 6981
		protected bool m_isPresentBoxSequence;

		// Token: 0x04001B46 RID: 6982
		protected bool m_queryPurchaseOnResume;

		// Token: 0x04001B47 RID: 6983
		protected Manager.State m_state;

		// Token: 0x04001B48 RID: 6984
		protected List<PurchaseInfo> m_productBaseList;

		// Token: 0x04001B4A RID: 6986
		protected NativePlugin.ProductInfo[] m_productInfo;

		// Token: 0x04001B4B RID: 6987
		protected NativePlugin.TransactionInfo m_transactionInfo;

		// Token: 0x04001B4C RID: 6988
		protected Manager.QueueItem m_cancelInfo;

		// Token: 0x04001B4D RID: 6989
		protected Queue<Manager.QueueItem> m_purchasedQueue;

		// Token: 0x04001B4E RID: 6990
		protected List<string> m_leftTransactionList;

		// Token: 0x04001B4F RID: 6991
		protected List<string> m_notFinishTransactionList;

		// Token: 0x04001B50 RID: 6992
		protected Action<string> m_setupFailedCallback;

		// Token: 0x04001B51 RID: 6993
		protected Action<NativePlugin.TransactionInfo?, Manager.PURCHASE_START_RESULT> m_purchaseAbortedCallback;

		// Token: 0x04001B52 RID: 6994
		protected Action<string> m_purchaseDetectedCallback;

		// Token: 0x04001B53 RID: 6995
		protected Manager.HttpRequestDelegate m_purchaseHttpRequestFunc;

		// Token: 0x04001B54 RID: 6996
		protected Dictionary<int, string> serverMstId2ProductIdMap;

		// Token: 0x04001B55 RID: 6997
		private Action<List<PurchaseInfo>, int> FetchProductListSuccessCallback;

		// Token: 0x04001B56 RID: 6998
		private Action FetchProductListFailureCallback;

		// Token: 0x04001B57 RID: 6999
		private Func<string, string, bool> StartPurchaseSuccessCallback;

		// Token: 0x04001B58 RID: 7000
		private Action<Manager.PURCHASE_START_RESULT> StartPurchaseFailureCallback;

		// Token: 0x04001B59 RID: 7001
		private Action CompletePurchaseSuccessCallback;

		// Token: 0x04001B5A RID: 7002
		private Action<Manager.PURCHASE_COMPLETE_RESULT> CompletePurchaseFailureCallback;

		// Token: 0x04001B5B RID: 7003
		protected Manager.PurchaseConfig m_purchaseConfig;

		// Token: 0x04001B5C RID: 7004
		protected Manager.ConfirmDelegate m_confirmDlgt;

		// Token: 0x04001B5F RID: 7007
		private bool _blockReceipt;

		// Token: 0x04001B60 RID: 7008
		private bool isNewReceiptQueued_;

		// Token: 0x0200107F RID: 4223
		public enum PURCHASE_START_RESULT
		{
			// Token: 0x04005BE4 RID: 23524
			FAILURE,
			// Token: 0x04005BE5 RID: 23525
			REJECTED,
			// Token: 0x04005BE6 RID: 23526
			ABORTED,
			// Token: 0x04005BE7 RID: 23527
			DEFERRED,
			// Token: 0x04005BE8 RID: 23528
			BLOCKED,
			// Token: 0x04005BE9 RID: 23529
			SUCCESS
		}

		// Token: 0x02001080 RID: 4224
		public enum PURCHASE_COMPLETE_RESULT
		{
			// Token: 0x04005BEB RID: 23531
			FAILURE,
			// Token: 0x04005BEC RID: 23532
			INVALID_RECEIPT,
			// Token: 0x04005BED RID: 23533
			RECEIPT_FOR_OTHER_ACCOUNT,
			// Token: 0x04005BEE RID: 23534
			SUCCESS
		}

		// Token: 0x02001081 RID: 4225
		public enum State
		{
			// Token: 0x04005BF0 RID: 23536
			WAIT,
			// Token: 0x04005BF1 RID: 23537
			INIT,
			// Token: 0x04005BF2 RID: 23538
			GET_PRODUCT,
			// Token: 0x04005BF3 RID: 23539
			SETUP,
			// Token: 0x04005BF4 RID: 23540
			MAIN,
			// Token: 0x04005BF5 RID: 23541
			CHECK_RECEIPT,
			// Token: 0x04005BF6 RID: 23542
			ERROR
		}

		// Token: 0x02001082 RID: 4226
		public class QueueItem : IComparable
		{
			// Token: 0x0600531E RID: 21278 RVA: 0x00249C13 File Offset: 0x00247E13
			public QueueItem()
			{
			}

			// Token: 0x0600531F RID: 21279 RVA: 0x00249C1B File Offset: 0x00247E1B
			public QueueItem(string product_id, string transaction_id)
			{
				this.product_id = product_id;
				this.transaction_id = transaction_id;
			}

			// Token: 0x06005320 RID: 21280 RVA: 0x00249C34 File Offset: 0x00247E34
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

			// Token: 0x06005321 RID: 21281 RVA: 0x00249C8A File Offset: 0x00247E8A
			public override string ToString()
			{
				return this.product_id + "," + this.transaction_id;
			}

			// Token: 0x04005BF7 RID: 23543
			public string product_id;

			// Token: 0x04005BF8 RID: 23544
			public string transaction_id;
		}

		// Token: 0x02001083 RID: 4227
		// (Invoke) Token: 0x06005323 RID: 21283
		public delegate void HttpRequestDelegate(Command cmd, Action<Command, int, string> finished);

		// Token: 0x02001084 RID: 4228
		// (Invoke) Token: 0x06005327 RID: 21287
		public delegate void ConfirmAcceptDelegate(string uniq_id);

		// Token: 0x02001085 RID: 4229
		// (Invoke) Token: 0x0600532B RID: 21291
		public delegate void ConfirmDenyDelegate(string uniq_id);

		// Token: 0x02001086 RID: 4230
		// (Invoke) Token: 0x0600532F RID: 21295
		public delegate void ConfirmDelegate(NativePlugin.ProductInfo pinfo, string uniq_id, Manager.ConfirmAcceptDelegate acceptDlgt, Manager.ConfirmDenyDelegate denyDlgt);

		// Token: 0x02001087 RID: 4231
		// (Invoke) Token: 0x06005333 RID: 21299
		public delegate void SuccessDelegate(PurchaseResponse response, PurchaseRequest request);

		// Token: 0x02001088 RID: 4232
		// (Invoke) Token: 0x06005337 RID: 21303
		public delegate void ErrorDelegate(NativePlugin.TransactionInfo? tinfo, int error_code, string reqProductIdString, Exception exception);

		// Token: 0x02001089 RID: 4233
		public class PurchaseConfig
		{
			// Token: 0x04005BF9 RID: 23545
			public int language;

			// Token: 0x04005BFA RID: 23546
			public int platform_id;

			// Token: 0x04005BFB RID: 23547
			public int store_id;

			// Token: 0x04005BFC RID: 23548
			public string bundle_id;
		}

		// Token: 0x0200108A RID: 4234
		public class InAppPurchaseException : Exception
		{
			// Token: 0x0600533B RID: 21307 RVA: 0x00249CAA File Offset: 0x00247EAA
			public InAppPurchaseException(int errorCode, string msg)
				: base(msg)
			{
				this.ErrorCode = errorCode;
			}

			// Token: 0x04005BFD RID: 23549
			public const int ERRCODE_IAP_SELECTPRODUCT = 1;

			// Token: 0x04005BFE RID: 23550
			public const int ERRCODE_IAP_STARTPURCHASE = 2;

			// Token: 0x04005BFF RID: 23551
			public const int ERRCODE_IAP_FAILEDMESSAGE = 3;

			// Token: 0x04005C00 RID: 23552
			public const int ERRCODE_IAP_DEFERREDMESSAGE = 4;

			// Token: 0x04005C01 RID: 23553
			public const int ERRCODE_IAP_SETUPFAILED = 5;

			// Token: 0x04005C02 RID: 23554
			public const int ERRCODE_IAP_NEEDCHECKAGE = 6;

			// Token: 0x04005C03 RID: 23555
			public const int ERRCODE_IAP_LIMITEDPURCHASE = 7;

			// Token: 0x04005C04 RID: 23556
			public int ErrorCode;
		}
	}
}
