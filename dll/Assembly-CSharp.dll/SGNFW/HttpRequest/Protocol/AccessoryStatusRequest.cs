using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200032B RID: 811
	public class AccessoryStatusRequest : Request
	{
		// Token: 0x04002357 RID: 9047
		public List<long> lock_accessory_id_list;

		// Token: 0x04002358 RID: 9048
		public List<long> lock_clear_accessory_id_list;
	}
}
