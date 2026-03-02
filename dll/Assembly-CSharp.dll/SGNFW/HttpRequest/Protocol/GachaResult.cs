using System;
using System.Collections.Generic;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003DF RID: 991
	[Serializable]
	public class GachaResult
	{
		// Token: 0x040024D4 RID: 9428
		public int item_id;

		// Token: 0x040024D5 RID: 9429
		public int item_num;

		// Token: 0x040024D6 RID: 9430
		public int rep_flg;

		// Token: 0x040024D7 RID: 9431
		public List<RepItem> rep_item_list;

		// Token: 0x040024D8 RID: 9432
		public long photo_id;

		// Token: 0x040024D9 RID: 9433
		public List<GachaItemOmakeResult> omake_item_list;

		// Token: 0x040024DA RID: 9434
		public int is_new;

		// Token: 0x040024DB RID: 9435
		public int is_present_box;
	}
}
