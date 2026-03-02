using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200054C RID: 1356
	public class TrainingPointRankingResponse : Response
	{
		// Token: 0x0400283B RID: 10299
		public List<TrainingPointRankingData> training_point_ranking;

		// Token: 0x0400283C RID: 10300
		public long last_update_time;

		// Token: 0x0400283D RID: 10301
		public int myrank;

		// Token: 0x0400283E RID: 10302
		public int confirm_flag;
	}
}
