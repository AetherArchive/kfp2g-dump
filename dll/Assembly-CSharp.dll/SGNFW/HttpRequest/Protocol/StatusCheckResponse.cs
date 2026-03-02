using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000534 RID: 1332
	public class StatusCheckResponse : Response
	{
		// Token: 0x0400280D RID: 10253
		public int result;

		// Token: 0x0400280E RID: 10254
		public int maintenance_login;

		// Token: 0x0400280F RID: 10255
		public int dif_version;

		// Token: 0x04002810 RID: 10256
		public int friend_code;

		// Token: 0x04002811 RID: 10257
		public int not_regist_flg;

		// Token: 0x04002812 RID: 10258
		public int dmm_data_linked_flg;

		// Token: 0x04002813 RID: 10259
		public int reaccept_flg;

		// Token: 0x04002814 RID: 10260
		public Maintenance maintenance;
	}
}
