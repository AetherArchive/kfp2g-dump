using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class CoopEndCmd : Command
	{
		private CoopEndCmd()
		{
		}

		private CoopEndCmd(int quest_id, int end_type, int eval, int turn_num, long hash_id, int arrival_wave, int continue_cnt, int okawari_cnt, int maxChain_num, int chain_cnt, int chain_sum_cnt, int max_damage, int arts_cnt, int player_skill_cnt, int tickle_success_cnt, int raid_total_daage, int bonus_defeated_count)
		{
			this.request = new CoopEndRequest();
			CoopEndRequest coopEndRequest = (CoopEndRequest)this.request;
			coopEndRequest.quest_id = quest_id;
			coopEndRequest.end_type = end_type;
			coopEndRequest.eval = eval;
			coopEndRequest.turn_num = turn_num;
			coopEndRequest.hash_id = hash_id;
			coopEndRequest.arrival_wave = arrival_wave;
			coopEndRequest.continue_cnt = continue_cnt;
			coopEndRequest.okawari_cnt = okawari_cnt;
			coopEndRequest.maxChain_num = maxChain_num;
			coopEndRequest.chain_cnt = chain_cnt;
			coopEndRequest.chain_sum_cnt = chain_sum_cnt;
			coopEndRequest.max_damage = max_damage;
			coopEndRequest.arts_cnt = arts_cnt;
			coopEndRequest.player_skill_cnt = player_skill_cnt;
			coopEndRequest.tickle_success_cnt = tickle_success_cnt;
			coopEndRequest.raid_total_damage = raid_total_daage;
			coopEndRequest.bonus_defeated_count = bonus_defeated_count;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "CoopEnd.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static CoopEndCmd Create(int quest_id, int end_type, int eval, int turn_num, long hash_id, int arrival_wave, int continue_cnt, int okawari_cnt, int maxChain_num, int chain_cnt, int chain_sum_cnt, int max_damage, int arts_cnt, int player_skill_cnt, int tickle_success_cnt, int raid_total_damage, int bonus_defeated_count)
		{
			return new CoopEndCmd(quest_id, end_type, eval, turn_num, hash_id, arrival_wave, continue_cnt, okawari_cnt, maxChain_num, chain_cnt, chain_sum_cnt, max_damage, arts_cnt, player_skill_cnt, tickle_success_cnt, raid_total_damage, bonus_defeated_count);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CoopEndResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CoopEnd";
		}
	}
}
