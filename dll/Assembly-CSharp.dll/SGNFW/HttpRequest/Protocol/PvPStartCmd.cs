using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PvPStartCmd : Command
	{
		private PvPStartCmd()
		{
		}

		private PvPStartCmd(int type_id, int season_id, int pvp_id, int opp_friend_id, int pvp_use_stamina)
		{
			this.request = new PvPStartRequest();
			PvPStartRequest pvPStartRequest = (PvPStartRequest)this.request;
			pvPStartRequest.type_id = type_id;
			pvPStartRequest.season_id = season_id;
			pvPStartRequest.pvp_id = pvp_id;
			pvPStartRequest.opp_friend_id = opp_friend_id;
			pvPStartRequest.pvp_use_stamina = pvp_use_stamina;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "PvPStart.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static PvPStartCmd Create(int type_id, int season_id, int pvp_id, int opp_friend_id, int pvp_use_stamina)
		{
			return new PvPStartCmd(type_id, season_id, pvp_id, opp_friend_id, pvp_use_stamina);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PvPStartResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PvPStart";
		}
	}
}
