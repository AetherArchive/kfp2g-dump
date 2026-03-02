using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004C6 RID: 1222
	public class PurchaseInfoCmd : Command
	{
		// Token: 0x06002C72 RID: 11378 RVA: 0x001AE6A4 File Offset: 0x001AC8A4
		private PurchaseInfoCmd(List<string> notFinishTransactionList, bool isStartGetProduct)
		{
			this.request = new PurchaseInfoRequest();
			PurchaseInfoRequest purchaseInfoRequest = (PurchaseInfoRequest)this.request;
			purchaseInfoRequest.notFinishTransactionList = notFinishTransactionList;
			purchaseInfoRequest.isStartGetProduct = isStartGetProduct;
			this.Setting();
		}

		// Token: 0x06002C73 RID: 11379 RVA: 0x001AE6D8 File Offset: 0x001AC8D8
		private void Setting()
		{
			base.Url = "PurchaseInfo.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002C74 RID: 11380 RVA: 0x001AE744 File Offset: 0x001AC944
		public static PurchaseInfoCmd Create(List<string> notFinishTransactionList, bool isStartGetProduct)
		{
			return new PurchaseInfoCmd(notFinishTransactionList, isStartGetProduct);
		}

		// Token: 0x06002C75 RID: 11381 RVA: 0x001AE74D File Offset: 0x001AC94D
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PurchaseInfoResponse>(__text);
		}

		// Token: 0x06002C76 RID: 11382 RVA: 0x001AE755 File Offset: 0x001AC955
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PurchaseInfo";
		}
	}
}
