using System;
using System.Collections.Generic;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class PvPInfo
	{
		public int pvp_type;

		public int season_id;

		public int pvp_id;

		public int before_season_id;

		public int pvp_point_before;

		public int pvp_rank_before;

		public int pvp_point_now;

		public int pvp_rank_now;

		public int set_deck_id;

		public int limit_chal_count;

		public long last_chal_datetime;

		public List<OppUser> opp_user_list;

		public List<PvPDefenseResult> pvp_defense_result;
	}
}
