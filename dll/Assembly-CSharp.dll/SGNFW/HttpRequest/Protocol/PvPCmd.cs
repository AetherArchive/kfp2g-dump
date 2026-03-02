using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004CC RID: 1228
	public class PvPCmd : Command
	{
		// Token: 0x06002C81 RID: 11393 RVA: 0x001AE83C File Offset: 0x001ACA3C
		private PvPCmd()
		{
		}

		// Token: 0x06002C82 RID: 11394 RVA: 0x001AE844 File Offset: 0x001ACA44
		private PvPCmd(int re_flg, int defense_season_id)
		{
			this.request = new PvPRequest();
			PvPRequest pvPRequest = (PvPRequest)this.request;
			pvPRequest.re_flg = re_flg;
			pvPRequest.defense_season_id = defense_season_id;
			this.Setting();
		}

		// Token: 0x06002C83 RID: 11395 RVA: 0x001AE878 File Offset: 0x001ACA78
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

		// Token: 0x06002C84 RID: 11396 RVA: 0x001AE8E4 File Offset: 0x001ACAE4
		public static PvPCmd Create(int re_flg, int defense_season_id)
		{
			return new PvPCmd(re_flg, defense_season_id);
		}

		// Token: 0x06002C85 RID: 11397 RVA: 0x001AE8ED File Offset: 0x001ACAED
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PvPResponse>(__text);
		}

		// Token: 0x06002C86 RID: 11398 RVA: 0x001AE8F5 File Offset: 0x001ACAF5
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PvP";
		}
	}
}
