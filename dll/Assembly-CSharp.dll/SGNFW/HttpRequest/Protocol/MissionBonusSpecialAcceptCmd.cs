using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000457 RID: 1111
	public class MissionBonusSpecialAcceptCmd : Command
	{
		// Token: 0x06002B6A RID: 11114 RVA: 0x001ACCE4 File Offset: 0x001AAEE4
		private MissionBonusSpecialAcceptCmd()
		{
		}

		// Token: 0x06002B6B RID: 11115 RVA: 0x001ACCEC File Offset: 0x001AAEEC
		private MissionBonusSpecialAcceptCmd(List<int> mission_id_list, List<AcceptMission> accept_mission_list)
		{
			this.request = new MissionBonusSpecialAcceptRequest();
			MissionBonusSpecialAcceptRequest missionBonusSpecialAcceptRequest = (MissionBonusSpecialAcceptRequest)this.request;
			missionBonusSpecialAcceptRequest.mission_id_list = mission_id_list;
			missionBonusSpecialAcceptRequest.accept_mission_list = accept_mission_list;
			this.Setting();
		}

		// Token: 0x06002B6C RID: 11116 RVA: 0x001ACD20 File Offset: 0x001AAF20
		private void Setting()
		{
			base.Url = "MissionBonusSpecialAccept.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002B6D RID: 11117 RVA: 0x001ACD8C File Offset: 0x001AAF8C
		public static MissionBonusSpecialAcceptCmd Create(List<int> mission_id_list, List<AcceptMission> accept_mission_list)
		{
			return new MissionBonusSpecialAcceptCmd(mission_id_list, accept_mission_list);
		}

		// Token: 0x06002B6E RID: 11118 RVA: 0x001ACD95 File Offset: 0x001AAF95
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<MissionBonusSpecialAcceptResponse>(__text);
		}

		// Token: 0x06002B6F RID: 11119 RVA: 0x001ACD9D File Offset: 0x001AAF9D
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/MissionBonusSpecialAccept";
		}
	}
}
