using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class CoopEndResponse : Response
	{
		public Assets assets;

		public int result_quest;

		public List<Quest> quests;

		public int item_presentbox;

		public List<DrewItem> drew_items;

		public List<KizunaBonus> kizuna_bonuspoint;
	}
}
