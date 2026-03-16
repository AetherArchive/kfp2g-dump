using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PurchaseCmd : Command
	{
		private PurchaseCmd()
		{
		}

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

		public static PurchaseCmd Create(string productId, string transactionId, string receipt, string signature, int dmm_viewer_id, string onetime_token, List<string> notFinishTransactionList)
		{
			return new PurchaseCmd(productId, transactionId, receipt, signature, dmm_viewer_id, onetime_token, notFinishTransactionList);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PurchaseResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/Purchase";
		}
	}
}
