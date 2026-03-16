using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PvPDeckSelectCmd : Command
	{
		private PvPDeckSelectCmd()
		{
		}

		private PvPDeckSelectCmd(int type_id, int season_id, int deck_id)
		{
			this.request = new PvPDeckSelectRequest();
			PvPDeckSelectRequest pvPDeckSelectRequest = (PvPDeckSelectRequest)this.request;
			pvPDeckSelectRequest.type_id = type_id;
			pvPDeckSelectRequest.season_id = season_id;
			pvPDeckSelectRequest.deck_id = deck_id;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "PvPDeckSelect.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static PvPDeckSelectCmd Create(int type_id, int season_id, int deck_id)
		{
			return new PvPDeckSelectCmd(type_id, season_id, deck_id);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PvPDeckSelectResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PvPDeckSelect";
		}
	}
}
