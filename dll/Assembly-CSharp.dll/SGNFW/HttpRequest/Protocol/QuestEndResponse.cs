using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004E7 RID: 1255
	public class QuestEndResponse : Response
	{
		// Token: 0x04002764 RID: 10084
		public Assets assets;

		// Token: 0x04002765 RID: 10085
		public int result_quest;

		// Token: 0x04002766 RID: 10086
		public List<Quest> quests;

		// Token: 0x04002767 RID: 10087
		public int chara_scenario;

		// Token: 0x04002768 RID: 10088
		public int item_presentbox;

		// Token: 0x04002769 RID: 10089
		public List<DrewItem> drew_items;

		// Token: 0x0400276A RID: 10090
		public List<KizunaBonus> kizuna_bonuspoint;
	}
}
