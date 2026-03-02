using System;
using System.Collections.Generic;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003C0 RID: 960
	[Serializable]
	public class FollowsUser
	{
		// Token: 0x04002489 RID: 9353
		public int friend_id;

		// Token: 0x0400248A RID: 9354
		public string user_name;

		// Token: 0x0400248B RID: 9355
		public int level;

		// Token: 0x0400248C RID: 9356
		public long last_login_time;

		// Token: 0x0400248D RID: 9357
		public int is_receive_follow;

		// Token: 0x0400248E RID: 9358
		public long is_receive_follow_datetime;

		// Token: 0x0400248F RID: 9359
		public int is_send_follw;

		// Token: 0x04002490 RID: 9360
		public long is_send_follw_datetime;

		// Token: 0x04002491 RID: 9361
		public string comment;

		// Token: 0x04002492 RID: 9362
		public List<FollowsChara> help_charas;

		// Token: 0x04002493 RID: 9363
		public int achievement_id;
	}
}
