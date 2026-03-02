using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004C1 RID: 1217
	public class PresentGetResponse : Response
	{
		// Token: 0x040026E3 RID: 9955
		public List<UserPresent> userPresentList;

		// Token: 0x040026E4 RID: 9956
		public List<long> receiveIdList;

		// Token: 0x040026E5 RID: 9957
		public Assets assets;

		// Token: 0x040026E6 RID: 9958
		public List<UserReceiveHistory> userReceiveHistoryList;

		// Token: 0x040026E7 RID: 9959
		public List<GachaResult> gacha_result;
	}
}
