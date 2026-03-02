using System;
using System.Collections.Generic;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003D6 RID: 982
	[Serializable]
	public class GachaItemOmakeResult
	{
		// Token: 0x040024BA RID: 9402
		public int item_id;

		// Token: 0x040024BB RID: 9403
		public int item_num;

		// Token: 0x040024BC RID: 9404
		public List<RepItem> rep_item_list;

		// Token: 0x040024BD RID: 9405
		public int is_new;

		// Token: 0x040024BE RID: 9406
		public int is_present_box;
	}
}
