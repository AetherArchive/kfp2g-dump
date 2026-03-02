using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004DC RID: 1244
	public class PvPStartRequest : Request
	{
		// Token: 0x0400273F RID: 10047
		public int type_id;

		// Token: 0x04002740 RID: 10048
		public int season_id;

		// Token: 0x04002741 RID: 10049
		public int pvp_id;

		// Token: 0x04002742 RID: 10050
		public int opp_friend_id;

		// Token: 0x04002743 RID: 10051
		public int pvp_use_stamina;
	}
}
