using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004CF RID: 1231
	public class PvPDeckSelectCmd : Command
	{
		// Token: 0x06002C89 RID: 11401 RVA: 0x001AE90C File Offset: 0x001ACB0C
		private PvPDeckSelectCmd()
		{
		}

		// Token: 0x06002C8A RID: 11402 RVA: 0x001AE914 File Offset: 0x001ACB14
		private PvPDeckSelectCmd(int type_id, int season_id, int deck_id)
		{
			this.request = new PvPDeckSelectRequest();
			PvPDeckSelectRequest pvPDeckSelectRequest = (PvPDeckSelectRequest)this.request;
			pvPDeckSelectRequest.type_id = type_id;
			pvPDeckSelectRequest.season_id = season_id;
			pvPDeckSelectRequest.deck_id = deck_id;
			this.Setting();
		}

		// Token: 0x06002C8B RID: 11403 RVA: 0x001AE94C File Offset: 0x001ACB4C
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

		// Token: 0x06002C8C RID: 11404 RVA: 0x001AE9B8 File Offset: 0x001ACBB8
		public static PvPDeckSelectCmd Create(int type_id, int season_id, int deck_id)
		{
			return new PvPDeckSelectCmd(type_id, season_id, deck_id);
		}

		// Token: 0x06002C8D RID: 11405 RVA: 0x001AE9C2 File Offset: 0x001ACBC2
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PvPDeckSelectResponse>(__text);
		}

		// Token: 0x06002C8E RID: 11406 RVA: 0x001AE9CA File Offset: 0x001ACBCA
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PvPDeckSelect";
		}
	}
}
