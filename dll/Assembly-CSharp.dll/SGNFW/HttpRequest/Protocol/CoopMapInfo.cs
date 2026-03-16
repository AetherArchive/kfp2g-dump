using System;
using System.Collections.Generic;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class CoopMapInfo
	{
		public int map_id;

		public int level;

		public long total_point;

		public int hard_quest_open_flg;

		public int hard_quest_clear_num;

		public long hard_quest_start_time;

		public CoopPlayerInfo hard_quest_clear_user;

		public List<CoopRanking> ranking_list;

		public int bonus_defeated_count;
	}
}
