using System;

namespace SGNFW.Mst
{
	// Token: 0x020002EB RID: 747
	[Serializable]
	public class MstEventBonusCharaData
	{
		// Token: 0x04002182 RID: 8578
		public int eventId;

		// Token: 0x04002183 RID: 8579
		public int charaId;

		// Token: 0x04002184 RID: 8580
		public long startTime;

		// Token: 0x04002185 RID: 8581
		public long endTime;

		// Token: 0x04002186 RID: 8582
		public int hpBonusRatio;

		// Token: 0x04002187 RID: 8583
		public int strBonusRatio;

		// Token: 0x04002188 RID: 8584
		public int defBonusRatio;

		// Token: 0x04002189 RID: 8585
		public int kizunaBonusRatio;

		// Token: 0x0400218A RID: 8586
		public int pickupFlag;

		// Token: 0x0400218B RID: 8587
		public int increaseItemId01;

		// Token: 0x0400218C RID: 8588
		public int dropBonusRatio01;

		// Token: 0x0400218D RID: 8589
		public int increaseItemId02;

		// Token: 0x0400218E RID: 8590
		public int dropBonusRatio02;
	}
}
