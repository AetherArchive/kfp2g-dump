using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000550 RID: 1360
	public class TrainingRankingRequest : Request
	{
		// Token: 0x04002850 RID: 10320
		public int season_id;

		// Token: 0x04002851 RID: 10321
		public int dayofweek;

		// Token: 0x04002852 RID: 10322
		public long last_update_time;
	}
}
