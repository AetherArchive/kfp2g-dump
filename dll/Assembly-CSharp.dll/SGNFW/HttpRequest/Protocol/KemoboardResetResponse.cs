using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200040C RID: 1036
	public class KemoboardResetResponse : Response
	{
		// Token: 0x04002535 RID: 9525
		public Assets assets;

		// Token: 0x04002536 RID: 9526
		public List<int> kemoboard_panels;
	}
}
