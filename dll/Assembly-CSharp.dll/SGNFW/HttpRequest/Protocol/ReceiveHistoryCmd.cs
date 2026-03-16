using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class ReceiveHistoryCmd : Command
	{
		private ReceiveHistoryCmd()
		{
		}

		private ReceiveHistoryCmd(int rangeLow, int rangeHigh)
		{
			this.request = new ReceiveHistoryRequest();
			ReceiveHistoryRequest receiveHistoryRequest = (ReceiveHistoryRequest)this.request;
			receiveHistoryRequest.rangeLow = rangeLow;
			receiveHistoryRequest.rangeHigh = rangeHigh;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "ReceiveHistory.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static ReceiveHistoryCmd Create(int rangeLow, int rangeHigh)
		{
			return new ReceiveHistoryCmd(rangeLow, rangeHigh);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<ReceiveHistoryResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/ReceiveHistory";
		}
	}
}
