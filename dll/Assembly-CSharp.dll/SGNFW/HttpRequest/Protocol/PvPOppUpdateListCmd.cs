using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004D7 RID: 1239
	public class PvPOppUpdateListCmd : Command
	{
		// Token: 0x06002C9B RID: 11419 RVA: 0x001AEB57 File Offset: 0x001ACD57
		private PvPOppUpdateListCmd()
		{
		}

		// Token: 0x06002C9C RID: 11420 RVA: 0x001AEB5F File Offset: 0x001ACD5F
		private PvPOppUpdateListCmd(int type_id, int season_id, int pvp_id)
		{
			this.request = new PvPOppUpdateListRequest();
			PvPOppUpdateListRequest pvPOppUpdateListRequest = (PvPOppUpdateListRequest)this.request;
			pvPOppUpdateListRequest.type_id = type_id;
			pvPOppUpdateListRequest.season_id = season_id;
			pvPOppUpdateListRequest.pvp_id = pvp_id;
			this.Setting();
		}

		// Token: 0x06002C9D RID: 11421 RVA: 0x001AEB98 File Offset: 0x001ACD98
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

		// Token: 0x06002C9E RID: 11422 RVA: 0x001AEC04 File Offset: 0x001ACE04
		public static PvPOppUpdateListCmd Create(int type_id, int season_id, int pvp_id)
		{
			return new PvPOppUpdateListCmd(type_id, season_id, pvp_id);
		}

		// Token: 0x06002C9F RID: 11423 RVA: 0x001AEC0E File Offset: 0x001ACE0E
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PvPOppUpdateListResponse>(__text);
		}

		// Token: 0x06002CA0 RID: 11424 RVA: 0x001AEC16 File Offset: 0x001ACE16
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PvPOppUpdateList";
		}
	}
}
