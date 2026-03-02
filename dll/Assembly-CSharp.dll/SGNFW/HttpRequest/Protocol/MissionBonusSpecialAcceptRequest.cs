using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000458 RID: 1112
	public class MissionBonusSpecialAcceptRequest : Request
	{
		// Token: 0x0400263E RID: 9790
		public List<int> mission_id_list;

		// Token: 0x0400263F RID: 9791
		public List<AcceptMission> accept_mission_list;
	}
}
