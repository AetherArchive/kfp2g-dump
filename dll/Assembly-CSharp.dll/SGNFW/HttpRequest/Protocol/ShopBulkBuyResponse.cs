using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000529 RID: 1321
	public class ShopBulkBuyResponse : Response
	{
		// Token: 0x040027FB RID: 10235
		public Assets assets;

		// Token: 0x040027FC RID: 10236
		public int send_presentbox;

		// Token: 0x040027FD RID: 10237
		public List<GachaResult> gacha_result;
	}
}
