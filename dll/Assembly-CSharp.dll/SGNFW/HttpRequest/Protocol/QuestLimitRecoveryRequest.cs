using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004E9 RID: 1257
	public class QuestLimitRecoveryRequest : Request
	{
		// Token: 0x0400276B RID: 10091
		public int quest_id;

		// Token: 0x0400276C RID: 10092
		public bool is_raid;
	}
}
