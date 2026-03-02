using System;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003CF RID: 975
	[Serializable]
	public class Gacha
	{
		// Token: 0x040024A7 RID: 9383
		public int gacha_id;

		// Token: 0x040024A8 RID: 9384
		public int gacha_type;

		// Token: 0x040024A9 RID: 9385
		public int ceiling_count;

		// Token: 0x040024AA RID: 9386
		public int total_sub_play_num;

		// Token: 0x040024AB RID: 9387
		public int continue_play_num;

		// Token: 0x040024AC RID: 9388
		public int disc_play_num;

		// Token: 0x040024AD RID: 9389
		public long last_play_time;

		// Token: 0x040024AE RID: 9390
		public long today_last_play_time;

		// Token: 0x040024AF RID: 9391
		public int reset_num;

		// Token: 0x040024B0 RID: 9392
		public int box_remain_num;
	}
}
