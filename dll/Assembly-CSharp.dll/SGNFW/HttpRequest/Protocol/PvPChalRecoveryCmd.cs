using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PvPChalRecoveryCmd : Command
	{
		private PvPChalRecoveryCmd()
		{
		}

		private PvPChalRecoveryCmd(int type_id, int season_id)
		{
			this.request = new PvPChalRecoveryRequest();
			PvPChalRecoveryRequest pvPChalRecoveryRequest = (PvPChalRecoveryRequest)this.request;
			pvPChalRecoveryRequest.type_id = type_id;
			pvPChalRecoveryRequest.season_id = season_id;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "PvPChalRecovery.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static PvPChalRecoveryCmd Create(int type_id, int season_id)
		{
			return new PvPChalRecoveryCmd(type_id, season_id);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PvPChalRecoveryResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PvPChalRecovery";
		}
	}
}
