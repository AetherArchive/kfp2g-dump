using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004FB RID: 1275
	public class QuestSkipResponse : Response
	{
		// Token: 0x04002786 RID: 10118
		public Assets assets;

		// Token: 0x04002787 RID: 10119
		public List<Quest> quests;

		// Token: 0x04002788 RID: 10120
		public long hash_id;

		// Token: 0x04002789 RID: 10121
		public List<DrewItem> drew_items;

		// Token: 0x0400278A RID: 10122
		public long start_time;
	}
}
