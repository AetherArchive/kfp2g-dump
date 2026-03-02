using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004D9 RID: 1241
	public class PvPOppUpdateListResponse : Response
	{
		// Token: 0x04002732 RID: 10034
		public Assets assets;

		// Token: 0x04002733 RID: 10035
		public List<OppUser> opp_user_list;

		// Token: 0x04002734 RID: 10036
		public int limit_chal_count;

		// Token: 0x04002735 RID: 10037
		public long last_chal_datetime;
	}
}
