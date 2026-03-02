using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000480 RID: 1152
	public class PhotoGrowResponse : Response
	{
		// Token: 0x0400267B RID: 9851
		public Assets assets;

		// Token: 0x0400267C RID: 9852
		public PhotoGrowResult result;

		// Token: 0x0400267D RID: 9853
		public List<RewardInfo> rewardinfoList;
	}
}
