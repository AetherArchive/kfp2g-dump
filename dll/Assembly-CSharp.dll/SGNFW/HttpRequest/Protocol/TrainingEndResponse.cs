using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200053D RID: 1341
	public class TrainingEndResponse : Response
	{
		// Token: 0x04002828 RID: 10280
		public Assets assets;

		// Token: 0x04002829 RID: 10281
		public int reward_id;

		// Token: 0x0400282A RID: 10282
		public List<KizunaBonus> kizuna_bonuspoint;
	}
}
