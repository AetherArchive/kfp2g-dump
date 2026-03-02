using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000459 RID: 1113
	public class MissionBonusSpecialAcceptResponse : Response
	{
		// Token: 0x04002640 RID: 9792
		public Assets assets;

		// Token: 0x04002641 RID: 9793
		public List<GachaResult> gacha_result;

		// Token: 0x04002642 RID: 9794
		public int accept_result;
	}
}
