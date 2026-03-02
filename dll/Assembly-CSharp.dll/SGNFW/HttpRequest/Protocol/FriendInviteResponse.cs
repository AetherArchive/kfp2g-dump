using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003C6 RID: 966
	public class FriendInviteResponse : Response
	{
		// Token: 0x04002495 RID: 9365
		public string invite_url;

		// Token: 0x04002496 RID: 9366
		public int response_code;

		// Token: 0x04002497 RID: 9367
		public string error_msg;
	}
}
