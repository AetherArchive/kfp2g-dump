using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200050F RID: 1295
	public class RegistAccountRequest : Request
	{
		// Token: 0x040027AC RID: 10156
		public string device;

		// Token: 0x040027AD RID: 10157
		public string signature;

		// Token: 0x040027AE RID: 10158
		public int dmm_viewer_id;
	}
}
