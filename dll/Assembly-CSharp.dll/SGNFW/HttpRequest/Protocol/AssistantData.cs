using System;
using System.Collections.Generic;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200034E RID: 846
	[Serializable]
	public class AssistantData
	{
		// Token: 0x04002390 RID: 9104
		public List<int> purchaseListQuest;

		// Token: 0x04002391 RID: 9105
		public List<int> purchaseListShop;

		// Token: 0x04002392 RID: 9106
		public int questAssistantCharaId;

		// Token: 0x04002393 RID: 9107
		public int shopAssistantCharaId;
	}
}
