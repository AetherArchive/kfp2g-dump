using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class CoopInfoResponse : Response
	{
		public List<CoopMapInfo> map_info_list;

		public List<CoopLog> log_list;

		public int event_item_num;

		public List<Quest> quests;

		public long get_info_time;

		public CoopHardQuestEndInfo hard_quest_end_info;
	}
}
