using System;
using System.Collections.Generic;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class OppUser
	{
		public int friend_id;

		public string user_name;

		public int player_rank;

		public int achievement_id;

		public int difficulty;

		public List<Chara> opp_chara_list;

		public List<int> kemoboard_panel_list;

		public int tactics_param1;

		public int tactics_param2;

		public int tactics_param3;

		public int npc_flag;

		public int topRank;

		public int kizuna_buff_qualified;
	}
}
