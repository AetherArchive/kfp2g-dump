using System;
using System.Collections.Generic;
using SGNFW.Common;
using UnityEngine;

namespace SGNFW.InAppPurchase
{
	public class NativePlugin
	{
		public static bool CanMakePayments()
		{
			return false;
		}

		public static void Create(NativePlugin.InitInfo initInfo)
		{
			Verbose<NativePlugin.Verbose>.Enabled = true;
			NativePlugin.m_initInfo = initInfo;
			NativePlugin.productid_list.Clear();
			GameObject.Find(NativePlugin.m_initInfo.gameObjectName).SendMessage(NativePlugin.m_initInfo.cbSetupFinished, "");
		}

		public static void SetGooglePublicKey(string publicKey)
		{
		}

		public static void Destroy()
		{
		}

		public static void AddProductID(string pid)
		{
			NativePlugin.productid_list.Add(pid);
		}

		public static void RequestProductsData()
		{
			NativePlugin.m_isloaded = false;
			int count = NativePlugin.productid_list.Count;
			if (count > 0)
			{
				for (int i = 0; i < count; i++)
				{
					NativePlugin.ProductInfo productInfo = default(NativePlugin.ProductInfo);
					productInfo.name = (productInfo.productID = NativePlugin.productid_list[i]);
					productInfo.price = "\0";
					productInfo.amount = 0.0;
					productInfo.currencyCode = "JPY";
					productInfo.valid = true;
					NativePlugin.productinfo_list[productInfo.productID] = productInfo;
				}
				GameObject.Find(NativePlugin.m_initInfo.gameObjectName).SendMessage(NativePlugin.m_initInfo.cbUpdateProductList, "");
				return;
			}
			GameObject.Find(NativePlugin.m_initInfo.gameObjectName).SendMessage(NativePlugin.m_initInfo.cbSetupFailed, "No products");
		}

		public static void UpdateProductList(string res)
		{
			NativePlugin.m_isloaded = true;
		}

		public static bool IsProductsDownloaded()
		{
			return NativePlugin.m_isloaded;
		}

		public static bool GetProductInfo(string pid, ref NativePlugin.ProductInfo info)
		{
			if (NativePlugin.productinfo_list.ContainsKey(pid))
			{
				info = NativePlugin.productinfo_list[pid];
			}
			else
			{
				info.valid = false;
			}
			return true;
		}

		public static bool SelectProduct(string pid, string uniq_id)
		{
			NativePlugin.TransactionInfo transactionInfo = default(NativePlugin.TransactionInfo);
			transactionInfo.transactionID = uniq_id;
			transactionInfo.productID = pid;
			transactionInfo.receipt = "";
			transactionInfo.isBase64Encoded = 0;
			NativePlugin.transaction_list.Add(transactionInfo);
			string text = string.Format("{{\"product_id\":\"{0}\",\"transaction_id\":\"{1}\"}}", pid, uniq_id);
			GameObject.Find(NativePlugin.m_initInfo.gameObjectName).SendMessage(NativePlugin.m_initInfo.cbPurchaseSuccess, text);
			return true;
		}

		[CLSCompliant(false)]
		public static uint GetTransactionNum()
		{
			return (uint)NativePlugin.transaction_list.Count;
		}

		public static string GetTransactionID(int idx)
		{
			return NativePlugin.transaction_list[idx].transactionID;
		}

		public static bool GetTransactionInfo(string tid, ref NativePlugin.TransactionInfo info)
		{
			NativePlugin.TransactionInfo transactionInfo = NativePlugin.transaction_list.Find((NativePlugin.TransactionInfo ti) => ti.transactionID == tid);
			if (transactionInfo.transactionID == tid)
			{
				info = transactionInfo;
				return true;
			}
			return false;
		}

		public static bool FinishTransaction(string tid)
		{
			NativePlugin.transaction_list.RemoveAll((NativePlugin.TransactionInfo ti) => ti.transactionID == tid);
			return false;
		}

		public static void ForceFinishTransactionAll()
		{
			NativePlugin.transaction_list.Clear();
		}

		public static void purchaseUpdates(string str)
		{
		}

		public static void registPurchase(string msg)
		{
		}

		private static List<string> productid_list = new List<string>();

		private static Dictionary<string, NativePlugin.ProductInfo> productinfo_list = new Dictionary<string, NativePlugin.ProductInfo>();

		private static List<NativePlugin.TransactionInfo> transaction_list = new List<NativePlugin.TransactionInfo>();

		private static bool m_isloaded = false;

		private static NativePlugin.InitInfo m_initInfo;

		public struct InitInfo
		{
			public string gameObjectName;

			public string cbSetupFinished;

			public string cbSetupFailed;

			public string cbUpdateProductList;

			public string cbPurchaseSuccess;

			public string cbPurchaseFailure;

			public string cbPurchaseRestore;

			public string cbPurchaseDeferred;

			public string cbPurchaseDetected;

			public bool queryPurchaseOnResume;
		}

		public struct ProductInfo
		{
			public bool valid;

			public string productID;

			public string name;

			public string price;

			public double amount;

			public string currencyCode;
		}

		public struct TransactionInfo
		{
			public void clear()
			{
				this.transactionID = "";
				this.productID = "";
				this.receipt = "";
				this.isBase64Encoded = 0;
			}

			public bool isValid()
			{
				return !string.IsNullOrEmpty(this.transactionID);
			}

			public string transactionID;

			public string productID;

			public string receipt;

			public int isBase64Encoded;
		}

		public class Verbose : Verbose<NativePlugin.Verbose>
		{
		}
	}
}
