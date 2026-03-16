using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class QuestStartResponse : Response
	{
		public Assets assets;

		public long hash_id;

		public List<DrewItem> drew_items;

		public long start_time;

		public List<int> enemies;
	}
}
