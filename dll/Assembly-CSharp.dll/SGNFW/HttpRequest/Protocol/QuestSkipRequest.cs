using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004FA RID: 1274
	public class QuestSkipRequest : Request
	{
		// Token: 0x0400277F RID: 10111
		public int quest_id;

		// Token: 0x04002780 RID: 10112
		public int deck_id;

		// Token: 0x04002781 RID: 10113
		public int friend_id;

		// Token: 0x04002782 RID: 10114
		public int helper_chara_id;

		// Token: 0x04002783 RID: 10115
		public int skip_num;

		// Token: 0x04002784 RID: 10116
		public int kemostatus;

		// Token: 0x04002785 RID: 10117
		public List<long> helper_photo_id_list;
	}
}
