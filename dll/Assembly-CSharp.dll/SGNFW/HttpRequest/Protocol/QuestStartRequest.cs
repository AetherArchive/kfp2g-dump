using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000500 RID: 1280
	public class QuestStartRequest : Request
	{
		// Token: 0x04002790 RID: 10128
		public int quest_id;

		// Token: 0x04002791 RID: 10129
		public int deck_id;

		// Token: 0x04002792 RID: 10130
		public int friend_id;

		// Token: 0x04002793 RID: 10131
		public int helper_chara_id;

		// Token: 0x04002794 RID: 10132
		public int kemostatus;

		// Token: 0x04002795 RID: 10133
		public List<long> photo_id_List;
	}
}
