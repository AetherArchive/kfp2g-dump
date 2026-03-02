using System;
using System.Collections.Generic;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003EB RID: 1003
	[Serializable]
	public class Helper
	{
		// Token: 0x040024FE RID: 9470
		public int friend_id;

		// Token: 0x040024FF RID: 9471
		public string user_name;

		// Token: 0x04002500 RID: 9472
		public int level;

		// Token: 0x04002501 RID: 9473
		public int add_point;

		// Token: 0x04002502 RID: 9474
		public long last_login_time;

		// Token: 0x04002503 RID: 9475
		public int is_receive_follow;

		// Token: 0x04002504 RID: 9476
		public long is_receive_follow_datetime;

		// Token: 0x04002505 RID: 9477
		public int is_send_follw;

		// Token: 0x04002506 RID: 9478
		public long is_send_follw_datetime;

		// Token: 0x04002507 RID: 9479
		public Chara favorite_chara;

		// Token: 0x04002508 RID: 9480
		public string comment;

		// Token: 0x04002509 RID: 9481
		public List<Chara> help_charas;

		// Token: 0x0400250A RID: 9482
		public int achievement_id;
	}
}
