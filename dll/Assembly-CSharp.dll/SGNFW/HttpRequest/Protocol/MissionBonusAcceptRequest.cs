using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000455 RID: 1109
	public class MissionBonusAcceptRequest : Request
	{
		// Token: 0x0400263A RID: 9786
		public List<int> mission_id_list;

		// Token: 0x0400263B RID: 9787
		public List<AcceptMission> accept_mission_list;
	}
}
