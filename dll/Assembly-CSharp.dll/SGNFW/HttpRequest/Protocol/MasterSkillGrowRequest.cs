using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000450 RID: 1104
	public class MasterSkillGrowRequest : Request
	{
		// Token: 0x0400262E RID: 9774
		public int master_skill_id;

		// Token: 0x0400262F RID: 9775
		public List<UseItem> use_items;
	}
}
