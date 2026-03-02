using System;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003E0 RID: 992
	[Serializable]
	public class Gachas
	{
		// Token: 0x040024DC RID: 9436
		public int gacha_id;

		// Token: 0x040024DD RID: 9437
		public int gacha_type;

		// Token: 0x040024DE RID: 9438
		public int ceiling_count;

		// Token: 0x040024DF RID: 9439
		public int total_sub_play_num;

		// Token: 0x040024E0 RID: 9440
		public int continue_play_num;

		// Token: 0x040024E1 RID: 9441
		public long last_play_time;

		// Token: 0x040024E2 RID: 9442
		public long today_last_play_time;
	}
}
