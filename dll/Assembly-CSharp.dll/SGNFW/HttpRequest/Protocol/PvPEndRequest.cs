using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004D4 RID: 1236
	public class PvPEndRequest : Request
	{
		// Token: 0x0400270F RID: 9999
		public int type_id;

		// Token: 0x04002710 RID: 10000
		public int season_id;

		// Token: 0x04002711 RID: 10001
		public int pvp_id;

		// Token: 0x04002712 RID: 10002
		public long hash_id;

		// Token: 0x04002713 RID: 10003
		public int turn_num;

		// Token: 0x04002714 RID: 10004
		public int battle_result;

		// Token: 0x04002715 RID: 10005
		public int okawari_cnt;

		// Token: 0x04002716 RID: 10006
		public int maxChain_num;

		// Token: 0x04002717 RID: 10007
		public int chain_cnt;

		// Token: 0x04002718 RID: 10008
		public int chain_sum_cnt;

		// Token: 0x04002719 RID: 10009
		public int max_damage;

		// Token: 0x0400271A RID: 10010
		public int kemostatus;

		// Token: 0x0400271B RID: 10011
		public int arts_cnt;

		// Token: 0x0400271C RID: 10012
		public int player_skill_cnt;

		// Token: 0x0400271D RID: 10013
		public int tickle_success_cnt;
	}
}
