using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003EA RID: 1002
	public class GPGLoginResponse : Response
	{
		// Token: 0x040024F9 RID: 9465
		public int result;

		// Token: 0x040024FA RID: 9466
		public int after_friend_code;

		// Token: 0x040024FB RID: 9467
		public string before_user_name;

		// Token: 0x040024FC RID: 9468
		public string after_user_name;

		// Token: 0x040024FD RID: 9469
		public string after_transfer_id;
	}
}
