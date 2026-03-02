using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200041C RID: 1052
	public class LoginDmmRequest : Request
	{
		// Token: 0x04002570 RID: 9584
		public string version;

		// Token: 0x04002571 RID: 9585
		public int lang;

		// Token: 0x04002572 RID: 9586
		public string device;

		// Token: 0x04002573 RID: 9587
		public string signature;

		// Token: 0x04002574 RID: 9588
		public int dmm_viewer_id;

		// Token: 0x04002575 RID: 9589
		public string onetime_token;
	}
}
