using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200039A RID: 922
	public class CoopEndRequest : Request
	{
		// Token: 0x0400240C RID: 9228
		public int quest_id;

		// Token: 0x0400240D RID: 9229
		public int end_type;

		// Token: 0x0400240E RID: 9230
		public int eval;

		// Token: 0x0400240F RID: 9231
		public int turn_num;

		// Token: 0x04002410 RID: 9232
		public long hash_id;

		// Token: 0x04002411 RID: 9233
		public int arrival_wave;

		// Token: 0x04002412 RID: 9234
		public int continue_cnt;

		// Token: 0x04002413 RID: 9235
		public int okawari_cnt;

		// Token: 0x04002414 RID: 9236
		public int maxChain_num;

		// Token: 0x04002415 RID: 9237
		public int chain_cnt;

		// Token: 0x04002416 RID: 9238
		public int chain_sum_cnt;

		// Token: 0x04002417 RID: 9239
		public int max_damage;

		// Token: 0x04002418 RID: 9240
		public int arts_cnt;

		// Token: 0x04002419 RID: 9241
		public int player_skill_cnt;

		// Token: 0x0400241A RID: 9242
		public int tickle_success_cnt;

		// Token: 0x0400241B RID: 9243
		public int raid_total_damage;

		// Token: 0x0400241C RID: 9244
		public int bonus_defeated_count;
	}
}
