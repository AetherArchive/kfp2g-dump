using System;

namespace SGNFW.Mst
{
	// Token: 0x020002C0 RID: 704
	[Serializable]
	public class MstPvpSeasonData
	{
		// Token: 0x04002008 RID: 8200
		public int typeId;

		// Token: 0x04002009 RID: 8201
		public int seasonId;

		// Token: 0x0400200A RID: 8202
		public int pvpId;

		// Token: 0x0400200B RID: 8203
		public long seasonStartDatetime;

		// Token: 0x0400200C RID: 8204
		public long seasonEndDatetime;

		// Token: 0x0400200D RID: 8205
		public int rankResetNum;

		// Token: 0x0400200E RID: 8206
		public int rankResetRatio;

		// Token: 0x0400200F RID: 8207
		public int eventId;
	}
}
