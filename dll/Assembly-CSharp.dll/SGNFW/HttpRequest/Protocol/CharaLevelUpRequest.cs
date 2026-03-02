using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000378 RID: 888
	public class CharaLevelUpRequest : Request
	{
		// Token: 0x040023E3 RID: 9187
		public int chara_id;

		// Token: 0x040023E4 RID: 9188
		public List<UseItem> use_items;
	}
}
