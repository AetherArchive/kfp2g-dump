using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000325 RID: 805
	public class AccessoryGrowRequest : Request
	{
		// Token: 0x04002352 RID: 9042
		public long accessory_id;

		// Token: 0x04002353 RID: 9043
		public List<long> materials;
	}
}
