using System;

namespace SGNFW.Mst
{
	// Token: 0x020002EE RID: 750
	[Serializable]
	public class MstTrainingSeasonData
	{
		// Token: 0x0400219D RID: 8605
		public int seasonId;

		// Token: 0x0400219E RID: 8606
		public long seasonStartDatetime;

		// Token: 0x0400219F RID: 8607
		public long seasonEndDatetime;

		// Token: 0x040021A0 RID: 8608
		public int recoveryMax;

		// Token: 0x040021A1 RID: 8609
		public int recoveryItemNum;
	}
}
