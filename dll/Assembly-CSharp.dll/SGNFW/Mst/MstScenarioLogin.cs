using System;

namespace SGNFW.Mst
{
	// Token: 0x02000313 RID: 787
	[Serializable]
	public class MstScenarioLogin
	{
		// Token: 0x040022CA RID: 8906
		public int id;

		// Token: 0x040022CB RID: 8907
		public string scenarioFileName;

		// Token: 0x040022CC RID: 8908
		public int orderId;

		// Token: 0x040022CD RID: 8909
		public long startTime;

		// Token: 0x040022CE RID: 8910
		public long endTime;

		// Token: 0x040022CF RID: 8911
		public string memoryGroupName;

		// Token: 0x040022D0 RID: 8912
		public string memoryTitleName;

		// Token: 0x040022D1 RID: 8913
		public int memoryCharaId01;

		// Token: 0x040022D2 RID: 8914
		public int memoryCharaId02;

		// Token: 0x040022D3 RID: 8915
		public string memoryText01;

		// Token: 0x040022D4 RID: 8916
		public string memoryText02;

		// Token: 0x040022D5 RID: 8917
		public int randomId;
	}
}
