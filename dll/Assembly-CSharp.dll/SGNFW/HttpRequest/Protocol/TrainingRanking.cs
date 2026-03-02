using System;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200054E RID: 1358
	[Serializable]
	public class TrainingRanking
	{
		// Token: 0x04002847 RID: 10311
		public int rank;

		// Token: 0x04002848 RID: 10312
		public int user_id;

		// Token: 0x04002849 RID: 10313
		public int friend_id;

		// Token: 0x0400284A RID: 10314
		public string user_name;

		// Token: 0x0400284B RID: 10315
		public int favorite_chara_id;

		// Token: 0x0400284C RID: 10316
		public int favorite_chara_face_id;

		// Token: 0x0400284D RID: 10317
		public int achievement_id;

		// Token: 0x0400284E RID: 10318
		public TrainingScore trainingscore;

		// Token: 0x0400284F RID: 10319
		public int user_level;
	}
}
