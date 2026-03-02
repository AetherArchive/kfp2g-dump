using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004C2 RID: 1218
	public class PurchaseCmd : Command
	{
		// Token: 0x06002C69 RID: 11369 RVA: 0x001AE594 File Offset: 0x001AC794
		private PurchaseCmd()
		{
		}

		// Token: 0x06002C6A RID: 11370 RVA: 0x001AE59C File Offset: 0x001AC79C
		private PurchaseCmd(string productId, string transactionId, string receipt, string signature, int dmm_viewer_id, string onetime_token, List<string> notFinishTransactionList)
		{
			this.request = new PurchaseRequest();
			PurchaseRequest purchaseRequest = (PurchaseRequest)this.request;
			purchaseRequest.productId = productId;
			purchaseRequest.transactionId = transactionId;
			purchaseRequest.receipt = receipt;
			purchaseRequest.signature = signature;
			purchaseRequest.dmm_viewer_id = dmm_viewer_id;
			purchaseRequest.onetime_token = onetime_token;
			purchaseRequest.notFinishTransactionList = notFinishTransactionList;
			this.Setting();
		}

		// Token: 0x06002C6B RID: 11371 RVA: 0x001AE600 File Offset: 0x001AC800
		private void Setting()
		{
			base.Url = "Purchase.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002C6C RID: 11372 RVA: 0x001AE66C File Offset: 0x001AC86C
		public static PurchaseCmd Create(string productId, string transactionId, string receipt, string signature, int dmm_viewer_id, string onetime_token, List<string> notFinishTransactionList)
		{
			return new PurchaseCmd(productId, transactionId, receipt, signature, dmm_viewer_id, onetime_token, notFinishTransactionList);
		}

		// Token: 0x06002C6D RID: 11373 RVA: 0x001AE67D File Offset: 0x001AC87D
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PurchaseResponse>(__text);
		}

		// Token: 0x06002C6E RID: 11374 RVA: 0x001AE685 File Offset: 0x001AC885
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/Purchase";
		}
	}
}
