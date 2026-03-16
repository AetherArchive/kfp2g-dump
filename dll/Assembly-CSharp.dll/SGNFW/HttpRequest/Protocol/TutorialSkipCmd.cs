using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class TutorialSkipCmd : Command
	{
		private TutorialSkipCmd()
		{
			this.request = new TutorialSkipRequest();
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "TutorialSkip.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static TutorialSkipCmd Create()
		{
			return new TutorialSkipCmd();
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<TutorialSkipResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/TutorialSkip";
		}
	}
}
