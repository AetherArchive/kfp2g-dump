using System;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class PlayerInfo
	{
		public string player_name;

		public int player_type;

		public int player_rank;

		public long player_exp;

		public int stamina;

		public long stamina_updated_at;

		public int tutorial_step;

		public int friend_id;

		public long last_login_time;

		public string comment;

		public int favorite_chara_id;

		public long birthday;

		public int monthlypack_id;

		public long monthlypack_endtime;

		public int monthlypack_id_next;

		public long monthlypack_endtime_next;

		public string played_login_scenario_list;

		public string played_introduction_list;
	}
}
