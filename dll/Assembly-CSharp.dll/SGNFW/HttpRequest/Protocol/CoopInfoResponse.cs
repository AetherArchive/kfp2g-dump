using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200039F RID: 927
	public class CoopInfoResponse : Response
	{
		// Token: 0x04002428 RID: 9256
		public List<CoopMapInfo> map_info_list;

		// Token: 0x04002429 RID: 9257
		public List<CoopLog> log_list;

		// Token: 0x0400242A RID: 9258
		public int event_item_num;

		// Token: 0x0400242B RID: 9259
		public List<Quest> quests;

		// Token: 0x0400242C RID: 9260
		public long get_info_time;

		// Token: 0x0400242D RID: 9261
		public CoopHardQuestEndInfo hard_quest_end_info;
	}
}
