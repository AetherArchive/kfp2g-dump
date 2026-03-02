using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004D5 RID: 1237
	public class PvPEndResponse : Response
	{
		// Token: 0x0400271E RID: 10014
		public Assets assets;

		// Token: 0x0400271F RID: 10015
		public PvPResult pvp_result;

		// Token: 0x04002720 RID: 10016
		public List<int> pvpspecialReleaseIdList;

		// Token: 0x04002721 RID: 10017
		public List<KizunaBonus> kizuna_bonuspoint;
	}
}
