using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200045C RID: 1116
	public class MissionListResponse : Response
	{
		// Token: 0x04002644 RID: 9796
		public List<Mission> missions;

		// Token: 0x04002645 RID: 9797
		public Assets assets;
	}
}
