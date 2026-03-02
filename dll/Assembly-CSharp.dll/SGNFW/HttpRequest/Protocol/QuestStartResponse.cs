using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000501 RID: 1281
	public class QuestStartResponse : Response
	{
		// Token: 0x04002796 RID: 10134
		public Assets assets;

		// Token: 0x04002797 RID: 10135
		public long hash_id;

		// Token: 0x04002798 RID: 10136
		public List<DrewItem> drew_items;

		// Token: 0x04002799 RID: 10137
		public long start_time;

		// Token: 0x0400279A RID: 10138
		public List<int> enemies;
	}
}
