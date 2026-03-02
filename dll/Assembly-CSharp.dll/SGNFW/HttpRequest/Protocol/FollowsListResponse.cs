using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003BE RID: 958
	public class FollowsListResponse : Response
	{
		// Token: 0x04002484 RID: 9348
		public List<FollowsUser> followsList;

		// Token: 0x04002485 RID: 9349
		public Assets assets;
	}
}
