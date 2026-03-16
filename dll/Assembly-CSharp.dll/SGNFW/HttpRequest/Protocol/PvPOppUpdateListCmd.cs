using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PvPOppUpdateListCmd : Command
	{
		private PvPOppUpdateListCmd()
		{
		}

		private PvPOppUpdateListCmd(int type_id, int season_id, int pvp_id)
		{
			this.request = new PvPOppUpdateListRequest();
			PvPOppUpdateListRequest pvPOppUpdateListRequest = (PvPOppUpdateListRequest)this.request;
			pvPOppUpdateListRequest.type_id = type_id;
			pvPOppUpdateListRequest.season_id = season_id;
			pvPOppUpdateListRequest.pvp_id = pvp_id;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "PvPOppUpdateList.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static PvPOppUpdateListCmd Create(int type_id, int season_id, int pvp_id)
		{
			return new PvPOppUpdateListCmd(type_id, season_id, pvp_id);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PvPOppUpdateListResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PvPOppUpdateList";
		}
	}
}
