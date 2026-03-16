using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class TrainingEndRequest : Request
	{
		public int season_id;

		public int dayofweek;

		public long hash_id;

		public long score;

		public int finish_turn_num;

		public int okawari_cnt;

		public int maxChain_num;

		public int chain_cnt;

		public int chain_sum_cnt;

		public int max_damage;

		public int kill_num;

		public int end_type;

		public int arts_cnt;

		public int boss_id;

		public List<int> kill_mob_enemy_list;

		public int player_skill_cnt;
	}
}
