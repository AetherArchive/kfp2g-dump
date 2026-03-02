using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004DB RID: 1243
	public class PvPStartCmd : Command
	{
		// Token: 0x06002CA4 RID: 11428 RVA: 0x001AEC35 File Offset: 0x001ACE35
		private PvPStartCmd()
		{
		}

		// Token: 0x06002CA5 RID: 11429 RVA: 0x001AEC40 File Offset: 0x001ACE40
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

		// Token: 0x06002CA6 RID: 11430 RVA: 0x001AEC94 File Offset: 0x001ACE94
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

		// Token: 0x06002CA7 RID: 11431 RVA: 0x001AED00 File Offset: 0x001ACF00
		public static PvPStartCmd Create(int type_id, int season_id, int pvp_id, int opp_friend_id, int pvp_use_stamina)
		{
			return new PvPStartCmd(type_id, season_id, pvp_id, opp_friend_id, pvp_use_stamina);
		}

		// Token: 0x06002CA8 RID: 11432 RVA: 0x001AED0D File Offset: 0x001ACF0D
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PvPStartResponse>(__text);
		}

		// Token: 0x06002CA9 RID: 11433 RVA: 0x001AED15 File Offset: 0x001ACF15
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PvPStart";
		}
	}
}
