using System;
using System.Collections.Generic;
using SGNFW.Common;
using UnityEngine;

namespace SGNFW.InAppPurchase
{
	// Token: 0x0200024A RID: 586
	public class NativePlugin
	{
		// Token: 0x060024E1 RID: 9441 RVA: 0x0019D9F6 File Offset: 0x0019BBF6
		public static bool CanMakePayments()
		{
			return false;
		}

		// Token: 0x060024E2 RID: 9442 RVA: 0x0019D9F9 File Offset: 0x0019BBF9
		public static void Create(NativePlugin.InitInfo initInfo)
		{
			Verbose<NativePlugin.Verbose>.Enabled = true;
			NativePlugin.m_initInfo = initInfo;
			NativePlugin.productid_list.Clear();
			GameObject.Find(NativePlugin.m_initInfo.gameObjectName).SendMessage(NativePlugin.m_initInfo.cbSetupFinished, "");
		}

		// Token: 0x060024E3 RID: 9443 RVA: 0x0019DA34 File Offset: 0x0019BC34
		public static void SetGooglePublicKey(string publicKey)
		{
		}

		// Token: 0x060024E4 RID: 9444 RVA: 0x0019DA36 File Offset: 0x0019BC36
		public static void Destroy()
		{
		}

		// Token: 0x060024E5 RID: 9445 RVA: 0x0019DA38 File Offset: 0x0019BC38
		public static void AddProductID(string pid)
		{
			NativePlugin.productid_list.Add(pid);
		}

		// Token: 0x060024E6 RID: 9446 RVA: 0x0019DA48 File Offset: 0x0019BC48
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

		// Token: 0x060024E7 RID: 9447 RVA: 0x0019DB25 File Offset: 0x0019BD25
		public static void UpdateProductList(string res)
		{
			NativePlugin.m_isloaded = true;
		}

		// Token: 0x060024E8 RID: 9448 RVA: 0x0019DB2D File Offset: 0x0019BD2D
		public static bool IsProductsDownloaded()
		{
			return NativePlugin.m_isloaded;
		}

		// Token: 0x060024E9 RID: 9449 RVA: 0x0019DB34 File Offset: 0x0019BD34
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

		// Token: 0x060024EA RID: 9450 RVA: 0x0019DB60 File Offset: 0x0019BD60
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

		// Token: 0x060024EB RID: 9451 RVA: 0x0019DBD1 File Offset: 0x0019BDD1
		[CLSCompliant(false)]
		public static uint GetTransactionNum()
		{
			return (uint)NativePlugin.transaction_list.Count;
		}

		// Token: 0x060024EC RID: 9452 RVA: 0x0019DBDD File Offset: 0x0019BDDD
		public static string GetTransactionID(int idx)
		{
			return NativePlugin.transaction_list[idx].transactionID;
		}

		// Token: 0x060024ED RID: 9453 RVA: 0x0019DBF0 File Offset: 0x0019BDF0
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

		// Token: 0x060024EE RID: 9454 RVA: 0x0019DC40 File Offset: 0x0019BE40
		public static bool FinishTransaction(string tid)
		{
			NativePlugin.transaction_list.RemoveAll((NativePlugin.TransactionInfo ti) => ti.transactionID == tid);
			return false;
		}

		// Token: 0x060024EF RID: 9455 RVA: 0x0019DC72 File Offset: 0x0019BE72
		public static void ForceFinishTransactionAll()
		{
			NativePlugin.transaction_list.Clear();
		}

		// Token: 0x060024F0 RID: 9456 RVA: 0x0019DC7E File Offset: 0x0019BE7E
		public static void purchaseUpdates(string str)
		{
		}

		// Token: 0x060024F1 RID: 9457 RVA: 0x0019DC80 File Offset: 0x0019BE80
		public static void registPurchase(string msg)
		{
		}

		// Token: 0x04001B61 RID: 7009
		private static List<string> productid_list = new List<string>();

		// Token: 0x04001B62 RID: 7010
		private static Dictionary<string, NativePlugin.ProductInfo> productinfo_list = new Dictionary<string, NativePlugin.ProductInfo>();

		// Token: 0x04001B63 RID: 7011
		private static List<NativePlugin.TransactionInfo> transaction_list = new List<NativePlugin.TransactionInfo>();

		// Token: 0x04001B64 RID: 7012
		private static bool m_isloaded = false;

		// Token: 0x04001B65 RID: 7013
		private static NativePlugin.InitInfo m_initInfo;

		// Token: 0x0200108F RID: 4239
		public struct InitInfo
		{
			// Token: 0x04005C0F RID: 23567
			public string gameObjectName;

			// Token: 0x04005C10 RID: 23568
			public string cbSetupFinished;

			// Token: 0x04005C11 RID: 23569
			public string cbSetupFailed;

			// Token: 0x04005C12 RID: 23570
			public string cbUpdateProductList;

			// Token: 0x04005C13 RID: 23571
			public string cbPurchaseSuccess;

			// Token: 0x04005C14 RID: 23572
			public string cbPurchaseFailure;

			// Token: 0x04005C15 RID: 23573
			public string cbPurchaseRestore;

			// Token: 0x04005C16 RID: 23574
			public string cbPurchaseDeferred;

			// Token: 0x04005C17 RID: 23575
			public string cbPurchaseDetected;

			// Token: 0x04005C18 RID: 23576
			public bool queryPurchaseOnResume;
		}

		// Token: 0x02001090 RID: 4240
		public struct ProductInfo
		{
			// Token: 0x04005C19 RID: 23577
			public bool valid;

			// Token: 0x04005C1A RID: 23578
			public string productID;

			// Token: 0x04005C1B RID: 23579
			public string name;

			// Token: 0x04005C1C RID: 23580
			public string price;

			// Token: 0x04005C1D RID: 23581
			public double amount;

			// Token: 0x04005C1E RID: 23582
			public string currencyCode;
		}

		// Token: 0x02001091 RID: 4241
		public struct TransactionInfo
		{
			// Token: 0x06005349 RID: 21321 RVA: 0x00249D92 File Offset: 0x00247F92
			public void clear()
			{
				this.transactionID = "";
				this.productID = "";
				this.receipt = "";
				this.isBase64Encoded = 0;
			}

			// Token: 0x0600534A RID: 21322 RVA: 0x00249DBC File Offset: 0x00247FBC
			public bool isValid()
			{
				return !string.IsNullOrEmpty(this.transactionID);
			}

			// Token: 0x04005C1F RID: 23583
			public string transactionID;

			// Token: 0x04005C20 RID: 23584
			public string productID;

			// Token: 0x04005C21 RID: 23585
			public string receipt;

			// Token: 0x04005C22 RID: 23586
			public int isBase64Encoded;
		}

		// Token: 0x02001092 RID: 4242
		public class Verbose : Verbose<NativePlugin.Verbose>
		{
		}
	}
}
