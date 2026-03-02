using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000338 RID: 824
	public class AccountGPGTransferResponse : Response
	{
		// Token: 0x04002360 RID: 9056
		public int result;

		// Token: 0x04002361 RID: 9057
		public int friend_code;

		// Token: 0x04002362 RID: 9058
		public string account_id;

		// Token: 0x04002363 RID: 9059
		public string uuid;

		// Token: 0x04002364 RID: 9060
		public string user_name;
	}
}
