using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004CE RID: 1230
	public class PvPResponse : Response
	{
		// Token: 0x04002706 RID: 9990
		public Assets assets;

		// Token: 0x04002707 RID: 9991
		public List<PvPInfo> pvp_info_list;
	}
}
