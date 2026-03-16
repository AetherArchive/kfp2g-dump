using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class TrainingEndCmd : Command
	{
		private TrainingEndCmd()
		{
		}

		private TrainingEndCmd(int season_id, int dayofweek, long hash_id, long score, int finish_turn_num, int okawari_cnt, int maxChain_num, int chain_cnt, int chain_sum_cnt, int max_damage, int kill_num, int end_type, int arts_cnt, int boss_id, List<int> kill_mob_enemy_list, int player_skill_cnt)
		{
			this.request = new TrainingEndRequest();
			TrainingEndRequest trainingEndRequest = (TrainingEndRequest)this.request;
			trainingEndRequest.season_id = season_id;
			trainingEndRequest.dayofweek = dayofweek;
			trainingEndRequest.hash_id = hash_id;
			trainingEndRequest.score = score;
			trainingEndRequest.finish_turn_num = finish_turn_num;
			trainingEndRequest.okawari_cnt = okawari_cnt;
			trainingEndRequest.maxChain_num = maxChain_num;
			trainingEndRequest.chain_cnt = chain_cnt;
			trainingEndRequest.chain_sum_cnt = chain_sum_cnt;
			trainingEndRequest.max_damage = max_damage;
			trainingEndRequest.kill_num = kill_num;
			trainingEndRequest.end_type = end_type;
			trainingEndRequest.arts_cnt = arts_cnt;
			trainingEndRequest.boss_id = boss_id;
			trainingEndRequest.kill_mob_enemy_list = kill_mob_enemy_list;
			trainingEndRequest.player_skill_cnt = player_skill_cnt;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "TrainingEnd.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static TrainingEndCmd Create(int season_id, int dayofweek, long hash_id, long score, int finish_turn_num, int okawari_cnt, int maxChain_num, int chain_cnt, int chain_sum_cnt, int max_damage, int kill_num, int end_type, int arts_cnt, int boss_id, List<int> kill_mob_enemy_list, int player_skill_cnt)
		{
			return new TrainingEndCmd(season_id, dayofweek, hash_id, score, finish_turn_num, okawari_cnt, maxChain_num, chain_cnt, chain_sum_cnt, max_damage, kill_num, end_type, arts_cnt, boss_id, kill_mob_enemy_list, player_skill_cnt);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<TrainingEndResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/TrainingEnd";
		}
	}
}
