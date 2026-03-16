using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PurchaseInfoCmd : Command
	{
		private PurchaseInfoCmd(List<string> notFinishTransactionList, bool isStartGetProduct)
		{
			this.request = new PurchaseInfoRequest();
			PurchaseInfoRequest purchaseInfoRequest = (PurchaseInfoRequest)this.request;
			purchaseInfoRequest.notFinishTransactionList = notFinishTransactionList;
			purchaseInfoRequest.isStartGetProduct = isStartGetProduct;
			this.Setting();
		}

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

		public static PurchaseInfoCmd Create(List<string> notFinishTransactionList, bool isStartGetProduct)
		{
			return new PurchaseInfoCmd(notFinishTransactionList, isStartGetProduct);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PurchaseInfoResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PurchaseInfo";
		}
	}
}
