using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003FF RID: 1023
	public class ItemExchangeResponse : Response
	{
		// Token: 0x04002529 RID: 9513
		public Assets assets;

		// Token: 0x0400252A RID: 9514
		public List<ExchangeExecuteCountInfo> infoList;
	}
}
