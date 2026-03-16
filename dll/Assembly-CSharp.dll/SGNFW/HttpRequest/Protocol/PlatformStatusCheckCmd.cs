using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PlatformStatusCheckCmd : Command
	{
		private PlatformStatusCheckCmd()
		{
			this.request = new PlatformStatusCheckRequest();
			PlatformStatusCheckRequest platformStatusCheckRequest = (PlatformStatusCheckRequest)this.request;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "PlatformStatusCheck.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static PlatformStatusCheckCmd Create()
		{
			return new PlatformStatusCheckCmd();
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PlatformStatusCheckResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PlatformStatusCheck";
		}
	}
}
