using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class KemostatusRankingCmd : Command
	{
		private KemostatusRankingCmd()
		{
		}

		private KemostatusRankingCmd(long last_update_time)
		{
			this.request = new KemostatusRankingRequest();
			((KemostatusRankingRequest)this.request).last_update_time = last_update_time;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "KemostatusRanking.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static KemostatusRankingCmd Create(long last_update_time)
		{
			return new KemostatusRankingCmd(last_update_time);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<KemostatusRankingResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/KemostatusRanking";
		}
	}
}
