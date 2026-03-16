using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class AtomRewardCmd : Command
	{
		private AtomRewardCmd()
		{
		}

		private AtomRewardCmd(string url_scheme, string noah_id)
		{
			this.request = new AtomRewardRequest();
			AtomRewardRequest atomRewardRequest = (AtomRewardRequest)this.request;
			atomRewardRequest.url_scheme = url_scheme;
			atomRewardRequest.noah_id = noah_id;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "atom/AtomReward.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static AtomRewardCmd Create(string url_scheme, string noah_id)
		{
			return new AtomRewardCmd(url_scheme, noah_id);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<AtomRewardResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/AtomReward";
		}
	}
}
