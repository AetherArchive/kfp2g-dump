using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003D5 RID: 981
	public class GachaExecResponse : Response
	{
		// Token: 0x040024B5 RID: 9397
		public Assets assets;

		// Token: 0x040024B6 RID: 9398
		public Gacha gacha;

		// Token: 0x040024B7 RID: 9399
		public List<GachaResult> gacha_result;

		// Token: 0x040024B8 RID: 9400
		public GachaResult gachatype_omake;

		// Token: 0x040024B9 RID: 9401
		public List<GachaResult> gachatype_omake_preset;
	}
}
