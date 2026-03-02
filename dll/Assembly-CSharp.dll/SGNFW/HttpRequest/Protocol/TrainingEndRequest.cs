using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200053C RID: 1340
	public class TrainingEndRequest : Request
	{
		// Token: 0x04002818 RID: 10264
		public int season_id;

		// Token: 0x04002819 RID: 10265
		public int dayofweek;

		// Token: 0x0400281A RID: 10266
		public long hash_id;

		// Token: 0x0400281B RID: 10267
		public long score;

		// Token: 0x0400281C RID: 10268
		public int finish_turn_num;

		// Token: 0x0400281D RID: 10269
		public int okawari_cnt;

		// Token: 0x0400281E RID: 10270
		public int maxChain_num;

		// Token: 0x0400281F RID: 10271
		public int chain_cnt;

		// Token: 0x04002820 RID: 10272
		public int chain_sum_cnt;

		// Token: 0x04002821 RID: 10273
		public int max_damage;

		// Token: 0x04002822 RID: 10274
		public int kill_num;

		// Token: 0x04002823 RID: 10275
		public int end_type;

		// Token: 0x04002824 RID: 10276
		public int arts_cnt;

		// Token: 0x04002825 RID: 10277
		public int boss_id;

		// Token: 0x04002826 RID: 10278
		public List<int> kill_mob_enemy_list;

		// Token: 0x04002827 RID: 10279
		public int player_skill_cnt;
	}
}
