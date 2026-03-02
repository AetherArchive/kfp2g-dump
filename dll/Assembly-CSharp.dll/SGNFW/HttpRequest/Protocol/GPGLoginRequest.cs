using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003E9 RID: 1001
	public class GPGLoginRequest : Request
	{
		// Token: 0x040024F7 RID: 9463
		public string gpg_player_id;

		// Token: 0x040024F8 RID: 9464
		public string auth_code;
	}
}
