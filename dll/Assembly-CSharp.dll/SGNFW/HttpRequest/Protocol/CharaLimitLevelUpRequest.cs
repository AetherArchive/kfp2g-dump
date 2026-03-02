using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200037B RID: 891
	public class CharaLimitLevelUpRequest : Request
	{
		// Token: 0x040023E7 RID: 9191
		public int chara_id;

		// Token: 0x040023E8 RID: 9192
		public int target_level_limit_id;
	}
}
