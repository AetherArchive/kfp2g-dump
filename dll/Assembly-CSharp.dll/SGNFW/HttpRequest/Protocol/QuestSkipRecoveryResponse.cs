using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004FE RID: 1278
	public class QuestSkipRecoveryResponse : Response
	{
		// Token: 0x0400278E RID: 10126
		public Assets assets;

		// Token: 0x0400278F RID: 10127
		public List<Quest> quests;
	}
}
