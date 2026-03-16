using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PvPEndRequest : Request
	{
		public int type_id;

		public int season_id;

		public int pvp_id;

		public long hash_id;

		public int turn_num;

		public int battle_result;

		public int okawari_cnt;

		public int maxChain_num;

		public int chain_cnt;

		public int chain_sum_cnt;

		public int max_damage;

		public int kemostatus;

		public int arts_cnt;

		public int player_skill_cnt;

		public int tickle_success_cnt;
	}
}
