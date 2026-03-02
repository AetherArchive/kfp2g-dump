using System;
using System.Collections.Generic;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000392 RID: 914
	[Serializable]
	public class CharaViewResult
	{
		// Token: 0x04002402 RID: 9218
		public List<GachaRateItem> pickup;

		// Token: 0x04002403 RID: 9219
		public List<GachaRateItem> other;
	}
}
