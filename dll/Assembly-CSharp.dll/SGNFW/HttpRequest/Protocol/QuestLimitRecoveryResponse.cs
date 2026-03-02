using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004EA RID: 1258
	public class QuestLimitRecoveryResponse : Response
	{
		// Token: 0x0400276D RID: 10093
		public Assets assets;

		// Token: 0x0400276E RID: 10094
		public int today_recovery_num;
	}
}
