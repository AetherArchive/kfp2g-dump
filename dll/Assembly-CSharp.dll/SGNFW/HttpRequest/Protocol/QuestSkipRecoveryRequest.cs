using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004FD RID: 1277
	public class QuestSkipRecoveryRequest : Request
	{
		// Token: 0x0400278B RID: 10123
		public int quest_id;

		// Token: 0x0400278C RID: 10124
		public int use_item_id;

		// Token: 0x0400278D RID: 10125
		public int skip_recovery_num;
	}
}
