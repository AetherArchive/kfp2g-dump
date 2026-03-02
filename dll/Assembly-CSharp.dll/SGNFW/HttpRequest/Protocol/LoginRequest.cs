using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000419 RID: 1049
	public class LoginRequest : Request
	{
		// Token: 0x04002556 RID: 9558
		public string uuid;

		// Token: 0x04002557 RID: 9559
		public string secure_id;

		// Token: 0x04002558 RID: 9560
		public string version;

		// Token: 0x04002559 RID: 9561
		public int lang;

		// Token: 0x0400255A RID: 9562
		public string device;

		// Token: 0x0400255B RID: 9563
		public string device_id;

		// Token: 0x0400255C RID: 9564
		public string signature;
	}
}
