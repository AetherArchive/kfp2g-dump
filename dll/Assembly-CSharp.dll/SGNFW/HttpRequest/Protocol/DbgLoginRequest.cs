using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003A8 RID: 936
	public class DbgLoginRequest : Request
	{
		// Token: 0x04002454 RID: 9300
		public string secure_id;

		// Token: 0x04002455 RID: 9301
		public string version;

		// Token: 0x04002456 RID: 9302
		public int lang;
	}
}
