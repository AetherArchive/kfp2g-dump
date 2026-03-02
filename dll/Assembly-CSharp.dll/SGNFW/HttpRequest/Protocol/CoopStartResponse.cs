using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003A6 RID: 934
	public class CoopStartResponse : Response
	{
		// Token: 0x0400244F RID: 9295
		public Assets assets;

		// Token: 0x04002450 RID: 9296
		public long hash_id;

		// Token: 0x04002451 RID: 9297
		public List<DrewItem> drew_items;

		// Token: 0x04002452 RID: 9298
		public long start_time;

		// Token: 0x04002453 RID: 9299
		public List<int> enemies;
	}
}
