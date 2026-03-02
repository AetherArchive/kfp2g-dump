using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000394 RID: 916
	public class CharaWildRelRequest : Request
	{
		// Token: 0x04002404 RID: 9220
		public int chara_id;

		// Token: 0x04002405 RID: 9221
		public List<WildResult> promote_request;

		// Token: 0x04002406 RID: 9222
		public int is_promoteup_action;
	}
}
