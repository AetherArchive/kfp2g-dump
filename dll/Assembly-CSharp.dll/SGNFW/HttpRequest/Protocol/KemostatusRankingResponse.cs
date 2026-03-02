using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000410 RID: 1040
	public class KemostatusRankingResponse : Response
	{
		// Token: 0x04002541 RID: 9537
		public List<KemostatusRankData> kemostatus_ranking;

		// Token: 0x04002542 RID: 9538
		public long last_update_time;

		// Token: 0x04002543 RID: 9539
		public int myrank;
	}
}
