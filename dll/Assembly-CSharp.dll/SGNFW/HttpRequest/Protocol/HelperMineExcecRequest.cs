using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003F3 RID: 1011
	public class HelperMineExcecRequest : Request
	{
		// Token: 0x0400250F RID: 9487
		public int action_type;

		// Token: 0x04002510 RID: 9488
		public List<int> target_friend_id_list;
	}
}
