using System;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200054D RID: 1357
	[Serializable]
	public class TrainingPointRankingData
	{
		// Token: 0x0400283F RID: 10303
		public int rank;

		// Token: 0x04002840 RID: 10304
		public string user_name;

		// Token: 0x04002841 RID: 10305
		public int favorite_chara_id;

		// Token: 0x04002842 RID: 10306
		public int favorite_chara_face_id;

		// Token: 0x04002843 RID: 10307
		public int achievement_id;

		// Token: 0x04002844 RID: 10308
		public int user_level;

		// Token: 0x04002845 RID: 10309
		public long rankingpoint;

		// Token: 0x04002846 RID: 10310
		public long total_score;
	}
}
