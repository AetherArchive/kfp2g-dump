using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class QuestSkipResponse : Response
	{
		public Assets assets;

		public List<Quest> quests;

		public long hash_id;

		public List<DrewItem> drew_items;

		public long start_time;
	}
}
