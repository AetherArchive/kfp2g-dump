using System;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004DA RID: 1242
	[Serializable]
	public class PvPResult
	{
		// Token: 0x04002736 RID: 10038
		public int pvp_rank;

		// Token: 0x04002737 RID: 10039
		public int pvp_point;

		// Token: 0x04002738 RID: 10040
		public int limit_chal_count;

		// Token: 0x04002739 RID: 10041
		public long last_chal_datetime;

		// Token: 0x0400273A RID: 10042
		public int winning_num;

		// Token: 0x0400273B RID: 10043
		public int c_incr_coin;

		// Token: 0x0400273C RID: 10044
		public int use_pvp_stamina;

		// Token: 0x0400273D RID: 10045
		public int photobonus_num;

		// Token: 0x0400273E RID: 10046
		public SpecialPvPBonusInfo bonus_info;
	}
}
