using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class QuestEndRequest : Request
	{
		public int quest_id;

		public int end_type;

		public int eval;

		public int turn_num;

		public long hash_id;

		public int arrival_wave;

		public int continue_cnt;

		public int okawari_cnt;

		public int maxChain_num;

		public int chain_cnt;

		public int chain_sum_cnt;

		public int max_damage;

		public int arts_cnt;

		public int player_skill_cnt;

		public int tickle_success_cnt;

		public List<int> banned_chara_list;
	}
}
