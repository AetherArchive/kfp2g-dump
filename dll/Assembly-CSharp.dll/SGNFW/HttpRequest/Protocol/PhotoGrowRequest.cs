using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200047F RID: 1151
	public class PhotoGrowRequest : Request
	{
		// Token: 0x04002679 RID: 9849
		public long photo_id;

		// Token: 0x0400267A RID: 9850
		public List<long> materials;
	}
}
