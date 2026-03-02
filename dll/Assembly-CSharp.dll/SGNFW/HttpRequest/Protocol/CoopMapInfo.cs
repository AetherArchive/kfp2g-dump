using System;
using System.Collections.Generic;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003A1 RID: 929
	[Serializable]
	public class CoopMapInfo
	{
		// Token: 0x04002432 RID: 9266
		public int map_id;

		// Token: 0x04002433 RID: 9267
		public int level;

		// Token: 0x04002434 RID: 9268
		public long total_point;

		// Token: 0x04002435 RID: 9269
		public int hard_quest_open_flg;

		// Token: 0x04002436 RID: 9270
		public int hard_quest_clear_num;

		// Token: 0x04002437 RID: 9271
		public long hard_quest_start_time;

		// Token: 0x04002438 RID: 9272
		public CoopPlayerInfo hard_quest_clear_user;

		// Token: 0x04002439 RID: 9273
		public List<CoopRanking> ranking_list;

		// Token: 0x0400243A RID: 9274
		public int bonus_defeated_count;
	}
}
