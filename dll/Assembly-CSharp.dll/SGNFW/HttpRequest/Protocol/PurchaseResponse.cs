using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004C4 RID: 1220
	public class PurchaseResponse : Response
	{
		// Token: 0x040026EF RID: 9967
		public int resultStatus;

		// Token: 0x040026F0 RID: 9968
		public int buyProductId;

		// Token: 0x040026F1 RID: 9969
		public Assets assets;

		// Token: 0x040026F2 RID: 9970
		public List<int> new_release_idList;
	}
}
