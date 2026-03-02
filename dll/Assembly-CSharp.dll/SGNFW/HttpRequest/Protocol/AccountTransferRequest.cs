using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200033A RID: 826
	public class AccountTransferRequest : Request
	{
		// Token: 0x04002365 RID: 9061
		public string transfer_id;

		// Token: 0x04002366 RID: 9062
		public string password;

		// Token: 0x04002367 RID: 9063
		public int dmm_viewer_id;

		// Token: 0x04002368 RID: 9064
		public string device;
	}
}
