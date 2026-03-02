using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000551 RID: 1361
	public class TrainingRankingResponse : Response
	{
		// Token: 0x04002853 RID: 10323
		public List<TrainingRanking> training_ranking;

		// Token: 0x04002854 RID: 10324
		public long last_update_time;
	}
}
