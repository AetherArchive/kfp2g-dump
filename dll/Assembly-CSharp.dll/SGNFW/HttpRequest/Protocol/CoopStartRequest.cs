using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003A5 RID: 933
	public class CoopStartRequest : Request
	{
		// Token: 0x04002446 RID: 9286
		public int quest_id;

		// Token: 0x04002447 RID: 9287
		public int deck_id;

		// Token: 0x04002448 RID: 9288
		public int friend_id;

		// Token: 0x04002449 RID: 9289
		public int helper_chara_id;

		// Token: 0x0400244A RID: 9290
		public int kemostatus;

		// Token: 0x0400244B RID: 9291
		public List<long> photo_id_List;

		// Token: 0x0400244C RID: 9292
		public long get_info_time;

		// Token: 0x0400244D RID: 9293
		public int event_id;

		// Token: 0x0400244E RID: 9294
		public long coop_last_update_point;
	}
}
