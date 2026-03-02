using System;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004DE RID: 1246
	[Serializable]
	public class Quest
	{
		// Token: 0x04002745 RID: 10053
		public int quest_id;

		// Token: 0x04002746 RID: 10054
		public int eval;

		// Token: 0x04002747 RID: 10055
		public int clear_num;

		// Token: 0x04002748 RID: 10056
		public int play_num;

		// Token: 0x04002749 RID: 10057
		public int today_clear_num;

		// Token: 0x0400274A RID: 10058
		public long first_clear_time;

		// Token: 0x0400274B RID: 10059
		public long last_clear_time;

		// Token: 0x0400274C RID: 10060
		public int today_recovery_num;

		// Token: 0x0400274D RID: 10061
		public int open_key_flag;

		// Token: 0x0400274E RID: 10062
		public int skip_count;

		// Token: 0x0400274F RID: 10063
		public int skip_recovery_count;
	}
}
