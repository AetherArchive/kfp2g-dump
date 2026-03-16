using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PvPEndCmd : Command
	{
		private PvPEndCmd()
		{
		}

		private PvPEndCmd(int type_id, int season_id, int pvp_id, long hash_id, int turn_num, int battle_result, int okawari_cnt, int maxChain_num, int chain_cnt, int chain_sum_cnt, int max_damage, int kemostatus, int arts_cnt, int player_skill_cnt, int tickle_success_cnt)
		{
			this.request = new PvPEndRequest();
			PvPEndRequest pvPEndRequest = (PvPEndRequest)this.request;
			pvPEndRequest.type_id = type_id;
			pvPEndRequest.season_id = season_id;
			pvPEndRequest.pvp_id = pvp_id;
			pvPEndRequest.hash_id = hash_id;
			pvPEndRequest.turn_num = turn_num;
			pvPEndRequest.battle_result = battle_result;
			pvPEndRequest.okawari_cnt = okawari_cnt;
			pvPEndRequest.maxChain_num = maxChain_num;
			pvPEndRequest.chain_cnt = chain_cnt;
			pvPEndRequest.chain_sum_cnt = chain_sum_cnt;
			pvPEndRequest.max_damage = max_damage;
			pvPEndRequest.kemostatus = kemostatus;
			pvPEndRequest.arts_cnt = arts_cnt;
			pvPEndRequest.player_skill_cnt = player_skill_cnt;
			pvPEndRequest.tickle_success_cnt = tickle_success_cnt;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "PvPEnd.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static PvPEndCmd Create(int type_id, int season_id, int pvp_id, long hash_id, int turn_num, int battle_result, int okawari_cnt, int maxChain_num, int chain_cnt, int chain_sum_cnt, int max_damage, int kemostatus, int arts_cnt, int player_skill_cnt, int tickle_success_cnt)
		{
			return new PvPEndCmd(type_id, season_id, pvp_id, hash_id, turn_num, battle_result, okawari_cnt, maxChain_num, chain_cnt, chain_sum_cnt, max_damage, kemostatus, arts_cnt, player_skill_cnt, tickle_success_cnt);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PvPEndResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PvPEnd";
		}
	}
}
