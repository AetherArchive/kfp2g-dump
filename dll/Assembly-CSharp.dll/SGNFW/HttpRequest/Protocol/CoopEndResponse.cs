using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200039B RID: 923
	public class CoopEndResponse : Response
	{
		// Token: 0x0400241D RID: 9245
		public Assets assets;

		// Token: 0x0400241E RID: 9246
		public int result_quest;

		// Token: 0x0400241F RID: 9247
		public List<Quest> quests;

		// Token: 0x04002420 RID: 9248
		public int item_presentbox;

		// Token: 0x04002421 RID: 9249
		public List<DrewItem> drew_items;

		// Token: 0x04002422 RID: 9250
		public List<KizunaBonus> kizuna_bonuspoint;
	}
}
