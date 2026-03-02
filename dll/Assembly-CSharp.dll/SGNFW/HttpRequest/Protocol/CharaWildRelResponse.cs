using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000395 RID: 917
	public class CharaWildRelResponse : Response
	{
		// Token: 0x04002407 RID: 9223
		public Assets assets;

		// Token: 0x04002408 RID: 9224
		public List<WildResult> wild_result;
	}
}
