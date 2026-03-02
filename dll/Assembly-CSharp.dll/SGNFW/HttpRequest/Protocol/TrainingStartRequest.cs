using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000554 RID: 1364
	public class TrainingStartRequest : Request
	{
		// Token: 0x0400285A RID: 10330
		public int season_id;

		// Token: 0x0400285B RID: 10331
		public int dayofweek;

		// Token: 0x0400285C RID: 10332
		public int quest_id;

		// Token: 0x0400285D RID: 10333
		public int deck_id;

		// Token: 0x0400285E RID: 10334
		public int kemostatus;
	}
}
