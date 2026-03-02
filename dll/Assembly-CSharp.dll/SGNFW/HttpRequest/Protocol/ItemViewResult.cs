using System;
using System.Collections.Generic;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000406 RID: 1030
	[Serializable]
	public class ItemViewResult
	{
		// Token: 0x0400252F RID: 9519
		public List<GachaRateItem> pickup;

		// Token: 0x04002530 RID: 9520
		public List<GachaRateItem> other;
	}
}
