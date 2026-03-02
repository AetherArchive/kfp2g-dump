using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200033B RID: 827
	public class AccountTransferResponse : Response
	{
		// Token: 0x04002369 RID: 9065
		public int result;

		// Token: 0x0400236A RID: 9066
		public string account_id;

		// Token: 0x0400236B RID: 9067
		public string uuid;

		// Token: 0x0400236C RID: 9068
		public int dmm_data_linked_flg;
	}
}
