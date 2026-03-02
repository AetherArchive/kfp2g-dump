using System;
using System.Collections.Generic;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003A3 RID: 931
	[Serializable]
	public class CoopRanking
	{
		// Token: 0x04002443 RID: 9283
		public long target_time;

		// Token: 0x04002444 RID: 9284
		public int mine_point;

		// Token: 0x04002445 RID: 9285
		public List<CoopPlayerInfo> ranked_user_list;
	}
}
