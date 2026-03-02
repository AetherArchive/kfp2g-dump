using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000387 RID: 903
	public class CharaRankUpRequest : Request
	{
		// Token: 0x040023F4 RID: 9204
		public int chara_id;

		// Token: 0x040023F5 RID: 9205
		public int target_rank;
	}
}
