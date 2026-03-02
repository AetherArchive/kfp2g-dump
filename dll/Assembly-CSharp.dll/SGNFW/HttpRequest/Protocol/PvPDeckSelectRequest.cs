using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004D0 RID: 1232
	public class PvPDeckSelectRequest : Request
	{
		// Token: 0x04002708 RID: 9992
		public int type_id;

		// Token: 0x04002709 RID: 9993
		public int season_id;

		// Token: 0x0400270A RID: 9994
		public int deck_id;
	}
}
