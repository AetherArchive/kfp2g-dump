using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000483 RID: 1155
	public class PhotoLockRequest : Request
	{
		// Token: 0x04002685 RID: 9861
		public List<long> lock_photo_id;

		// Token: 0x04002686 RID: 9862
		public List<long> lock_clear_photo_id;
	}
}
