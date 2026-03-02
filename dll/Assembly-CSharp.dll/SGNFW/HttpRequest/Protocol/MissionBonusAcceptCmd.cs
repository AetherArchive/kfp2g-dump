using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000454 RID: 1108
	public class MissionBonusAcceptCmd : Command
	{
		// Token: 0x06002B62 RID: 11106 RVA: 0x001ACC14 File Offset: 0x001AAE14
		private MissionBonusAcceptCmd()
		{
		}

		// Token: 0x06002B63 RID: 11107 RVA: 0x001ACC1C File Offset: 0x001AAE1C
		private MissionBonusAcceptCmd(List<int> mission_id_list, List<AcceptMission> accept_mission_list)
		{
			this.request = new MissionBonusAcceptRequest();
			MissionBonusAcceptRequest missionBonusAcceptRequest = (MissionBonusAcceptRequest)this.request;
			missionBonusAcceptRequest.mission_id_list = mission_id_list;
			missionBonusAcceptRequest.accept_mission_list = accept_mission_list;
			this.Setting();
		}

		// Token: 0x06002B64 RID: 11108 RVA: 0x001ACC50 File Offset: 0x001AAE50
		private void Setting()
		{
			base.Url = "MissionBonusAccept.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002B65 RID: 11109 RVA: 0x001ACCBC File Offset: 0x001AAEBC
		public static MissionBonusAcceptCmd Create(List<int> mission_id_list, List<AcceptMission> accept_mission_list)
		{
			return new MissionBonusAcceptCmd(mission_id_list, accept_mission_list);
		}

		// Token: 0x06002B66 RID: 11110 RVA: 0x001ACCC5 File Offset: 0x001AAEC5
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<MissionBonusAcceptResponse>(__text);
		}

		// Token: 0x06002B67 RID: 11111 RVA: 0x001ACCCD File Offset: 0x001AAECD
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/MissionBonusAccept";
		}
	}
}
