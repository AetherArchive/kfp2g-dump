using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PresentBoxCmd : Command
	{
		private PresentBoxCmd()
		{
		}

		private PresentBoxCmd(int rangeLow, int rangeHigh)
		{
			this.request = new PresentBoxRequest();
			PresentBoxRequest presentBoxRequest = (PresentBoxRequest)this.request;
			presentBoxRequest.rangeLow = rangeLow;
			presentBoxRequest.rangeHigh = rangeHigh;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "PresentBox.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static PresentBoxCmd Create(int rangeLow, int rangeHigh)
		{
			return new PresentBoxCmd(rangeLow, rangeHigh);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PresentBoxResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PresentBox";
		}
	}
}
