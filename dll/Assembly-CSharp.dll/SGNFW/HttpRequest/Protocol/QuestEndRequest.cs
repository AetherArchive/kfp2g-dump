using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004E6 RID: 1254
	public class QuestEndRequest : Request
	{
		// Token: 0x04002754 RID: 10068
		public int quest_id;

		// Token: 0x04002755 RID: 10069
		public int end_type;

		// Token: 0x04002756 RID: 10070
		public int eval;

		// Token: 0x04002757 RID: 10071
		public int turn_num;

		// Token: 0x04002758 RID: 10072
		public long hash_id;

		// Token: 0x04002759 RID: 10073
		public int arrival_wave;

		// Token: 0x0400275A RID: 10074
		public int continue_cnt;

		// Token: 0x0400275B RID: 10075
		public int okawari_cnt;

		// Token: 0x0400275C RID: 10076
		public int maxChain_num;

		// Token: 0x0400275D RID: 10077
		public int chain_cnt;

		// Token: 0x0400275E RID: 10078
		public int chain_sum_cnt;

		// Token: 0x0400275F RID: 10079
		public int max_damage;

		// Token: 0x04002760 RID: 10080
		public int arts_cnt;

		// Token: 0x04002761 RID: 10081
		public int player_skill_cnt;

		// Token: 0x04002762 RID: 10082
		public int tickle_success_cnt;

		// Token: 0x04002763 RID: 10083
		public List<int> banned_chara_list;
	}
}
