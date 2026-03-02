using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200052C RID: 1324
	public class ShopBuyResponse : Response
	{
		// Token: 0x04002800 RID: 10240
		public Assets assets;

		// Token: 0x04002801 RID: 10241
		public int send_presentbox;

		// Token: 0x04002802 RID: 10242
		public List<GachaResult> gacha_result;
	}
}
