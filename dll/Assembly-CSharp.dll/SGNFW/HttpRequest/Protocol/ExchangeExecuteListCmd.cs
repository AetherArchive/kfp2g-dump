using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class ExchangeExecuteListCmd : Command
	{
		private ExchangeExecuteListCmd()
		{
			this.request = new ExchangeExecuteListRequest();
			ExchangeExecuteListRequest exchangeExecuteListRequest = (ExchangeExecuteListRequest)this.request;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "ExchangeExecuteList.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static ExchangeExecuteListCmd Create()
		{
			return new ExchangeExecuteListCmd();
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<ExchangeExecuteListResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/ExchangeExecuteList";
		}
	}
}
