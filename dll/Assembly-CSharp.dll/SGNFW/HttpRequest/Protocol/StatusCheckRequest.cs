using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000533 RID: 1331
	public class StatusCheckRequest : Request
	{
		// Token: 0x0400280A RID: 10250
		public string uuid;

		// Token: 0x0400280B RID: 10251
		public string version;

		// Token: 0x0400280C RID: 10252
		public int dmm_viewer_id;
	}
}
