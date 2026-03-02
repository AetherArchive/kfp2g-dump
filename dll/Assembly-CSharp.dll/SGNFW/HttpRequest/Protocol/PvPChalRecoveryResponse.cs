using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004CB RID: 1227
	public class PvPChalRecoveryResponse : Response
	{
		// Token: 0x04002701 RID: 9985
		public Assets assets;

		// Token: 0x04002702 RID: 9986
		public int limit_chal_count;

		// Token: 0x04002703 RID: 9987
		public long last_chal_datetime;
	}
}
