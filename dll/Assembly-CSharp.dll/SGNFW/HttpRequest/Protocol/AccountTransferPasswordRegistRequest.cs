using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200033D RID: 829
	public class AccountTransferPasswordRegistRequest : Request
	{
		// Token: 0x0400236D RID: 9069
		public string transfer_id;

		// Token: 0x0400236E RID: 9070
		public string password;

		// Token: 0x0400236F RID: 9071
		public string uuid;

		// Token: 0x04002370 RID: 9072
		public string secure_id;
	}
}
