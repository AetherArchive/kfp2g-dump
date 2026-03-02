using System;
using System.Collections.Generic;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004D6 RID: 1238
	[Serializable]
	public class PvPInfo
	{
		// Token: 0x04002722 RID: 10018
		public int pvp_type;

		// Token: 0x04002723 RID: 10019
		public int season_id;

		// Token: 0x04002724 RID: 10020
		public int pvp_id;

		// Token: 0x04002725 RID: 10021
		public int before_season_id;

		// Token: 0x04002726 RID: 10022
		public int pvp_point_before;

		// Token: 0x04002727 RID: 10023
		public int pvp_rank_before;

		// Token: 0x04002728 RID: 10024
		public int pvp_point_now;

		// Token: 0x04002729 RID: 10025
		public int pvp_rank_now;

		// Token: 0x0400272A RID: 10026
		public int set_deck_id;

		// Token: 0x0400272B RID: 10027
		public int limit_chal_count;

		// Token: 0x0400272C RID: 10028
		public long last_chal_datetime;

		// Token: 0x0400272D RID: 10029
		public List<OppUser> opp_user_list;

		// Token: 0x0400272E RID: 10030
		public List<PvPDefenseResult> pvp_defense_result;
	}
}
