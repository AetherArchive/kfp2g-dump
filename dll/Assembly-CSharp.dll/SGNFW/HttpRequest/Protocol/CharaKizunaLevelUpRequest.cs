using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000371 RID: 881
	public class CharaKizunaLevelUpRequest : Request
	{
		// Token: 0x040023DB RID: 9179
		public int chara_id;

		// Token: 0x040023DC RID: 9180
		public List<UseItem> use_items;
	}
}
