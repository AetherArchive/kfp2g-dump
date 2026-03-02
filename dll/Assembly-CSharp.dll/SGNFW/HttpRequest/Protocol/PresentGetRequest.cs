using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004C0 RID: 1216
	public class PresentGetRequest : Request
	{
		// Token: 0x040026DE RID: 9950
		public List<long> targetIdList;

		// Token: 0x040026DF RID: 9951
		public int rangeLow;

		// Token: 0x040026E0 RID: 9952
		public int rangeHigh;

		// Token: 0x040026E1 RID: 9953
		public int histRangeLow;

		// Token: 0x040026E2 RID: 9954
		public int histRangeHigh;
	}
}
