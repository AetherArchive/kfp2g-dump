using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class QuestEndCmd : Command
	{
		private QuestEndCmd()
		{
		}

		private QuestEndCmd(int quest_id, int end_type, int eval, int turn_num, long hash_id, int arrival_wave, int continue_cnt, int okawari_cnt, int maxChain_num, int chain_cnt, int chain_sum_cnt, int max_damage, int arts_cnt, int player_skill_cnt, int tickle_success_cnt, List<int> banned_chara_list)
		{
			this.request = new QuestEndRequest();
			QuestEndRequest questEndRequest = (QuestEndRequest)this.request;
			questEndRequest.quest_id = quest_id;
			questEndRequest.end_type = end_type;
			questEndRequest.eval = eval;
			questEndRequest.turn_num = turn_num;
			questEndRequest.hash_id = hash_id;
			questEndRequest.arrival_wave = arrival_wave;
			questEndRequest.continue_cnt = continue_cnt;
			questEndRequest.okawari_cnt = okawari_cnt;
			questEndRequest.maxChain_num = maxChain_num;
			questEndRequest.chain_cnt = chain_cnt;
			questEndRequest.chain_sum_cnt = chain_sum_cnt;
			questEndRequest.max_damage = max_damage;
			questEndRequest.arts_cnt = arts_cnt;
			questEndRequest.player_skill_cnt = player_skill_cnt;
			questEndRequest.tickle_success_cnt = tickle_success_cnt;
			questEndRequest.banned_chara_list = banned_chara_list;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "QuestEnd.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static QuestEndCmd Create(int quest_id, int end_type, int eval, int turn_num, long hash_id, int arrival_wave, int continue_cnt, int okawari_cnt, int maxChain_num, int chain_cnt, int chain_sum_cnt, int max_damage, int arts_cnt, int player_skill_cnt, int tickle_success_cnt, List<int> banned_chara_list = null)
		{
			return new QuestEndCmd(quest_id, end_type, eval, turn_num, hash_id, arrival_wave, continue_cnt, okawari_cnt, maxChain_num, chain_cnt, chain_sum_cnt, max_damage, arts_cnt, player_skill_cnt, tickle_success_cnt, banned_chara_list);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<QuestEndResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/QuestEnd";
		}
	}
}
