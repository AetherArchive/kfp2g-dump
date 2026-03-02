using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000540 RID: 1344
	public class TrainingInfoResponse : Response
	{
		// Token: 0x0400282B RID: 10283
		public int season_id;

		// Token: 0x0400282C RID: 10284
		public int dayofweek;

		// Token: 0x0400282D RID: 10285
		public long hiscore;

		// Token: 0x0400282E RID: 10286
		public int today_recovery_num;

		// Token: 0x0400282F RID: 10287
		public int today_play_num;

		// Token: 0x04002830 RID: 10288
		public int firsttme_flag;
	}
}
