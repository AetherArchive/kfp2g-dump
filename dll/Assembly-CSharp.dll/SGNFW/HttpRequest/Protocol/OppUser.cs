using System;
using System.Collections.Generic;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000472 RID: 1138
	[Serializable]
	public class OppUser
	{
		// Token: 0x0400265D RID: 9821
		public int friend_id;

		// Token: 0x0400265E RID: 9822
		public string user_name;

		// Token: 0x0400265F RID: 9823
		public int player_rank;

		// Token: 0x04002660 RID: 9824
		public int achievement_id;

		// Token: 0x04002661 RID: 9825
		public int difficulty;

		// Token: 0x04002662 RID: 9826
		public List<Chara> opp_chara_list;

		// Token: 0x04002663 RID: 9827
		public List<int> kemoboard_panel_list;

		// Token: 0x04002664 RID: 9828
		public int tactics_param1;

		// Token: 0x04002665 RID: 9829
		public int tactics_param2;

		// Token: 0x04002666 RID: 9830
		public int tactics_param3;

		// Token: 0x04002667 RID: 9831
		public int npc_flag;

		// Token: 0x04002668 RID: 9832
		public int topRank;

		// Token: 0x04002669 RID: 9833
		public int kizuna_buff_qualified;
	}
}
