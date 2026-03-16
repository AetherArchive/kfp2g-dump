using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PvPCmd : Command
	{
		private PvPCmd()
		{
		}

		private PvPCmd(int re_flg, int defense_season_id)
		{
			this.request = new PvPRequest();
			PvPRequest pvPRequest = (PvPRequest)this.request;
			pvPRequest.re_flg = re_flg;
			pvPRequest.defense_season_id = defense_season_id;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "PvP.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static PvPCmd Create(int re_flg, int defense_season_id)
		{
			return new PvPCmd(re_flg, defense_season_id);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PvPResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PvP";
		}
	}
}
