using System;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class PvPResult
	{
		public int pvp_rank;

		public int pvp_point;

		public int limit_chal_count;

		public long last_chal_datetime;

		public int winning_num;

		public int c_incr_coin;

		public int use_pvp_stamina;

		public int photobonus_num;

		public SpecialPvPBonusInfo bonus_info;
	}
}
