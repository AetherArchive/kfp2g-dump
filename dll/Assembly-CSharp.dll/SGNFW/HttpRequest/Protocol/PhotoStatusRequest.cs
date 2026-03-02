using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200048F RID: 1167
	public class PhotoStatusRequest : Request
	{
		// Token: 0x04002691 RID: 9873
		public List<long> lock_photo_id;

		// Token: 0x04002692 RID: 9874
		public List<long> lock_clear_photo_id;

		// Token: 0x04002693 RID: 9875
		public List<long> img_before_photo_id;

		// Token: 0x04002694 RID: 9876
		public List<long> img_after_photo_id;

		// Token: 0x04002695 RID: 9877
		public List<long> favorite_photo_id;

		// Token: 0x04002696 RID: 9878
		public List<long> favorite_clear_photo_id;
	}
}
