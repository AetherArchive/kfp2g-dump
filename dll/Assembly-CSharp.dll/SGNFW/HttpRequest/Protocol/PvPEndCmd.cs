using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004D3 RID: 1235
	public class PvPEndCmd : Command
	{
		// Token: 0x06002C92 RID: 11410 RVA: 0x001AE9E9 File Offset: 0x001ACBE9
		private PvPEndCmd()
		{
		}

		// Token: 0x06002C93 RID: 11411 RVA: 0x001AE9F4 File Offset: 0x001ACBF4
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

		// Token: 0x06002C94 RID: 11412 RVA: 0x001AEA98 File Offset: 0x001ACC98
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

		// Token: 0x06002C95 RID: 11413 RVA: 0x001AEB04 File Offset: 0x001ACD04
		public static PvPEndCmd Create(int type_id, int season_id, int pvp_id, long hash_id, int turn_num, int battle_result, int okawari_cnt, int maxChain_num, int chain_cnt, int chain_sum_cnt, int max_damage, int kemostatus, int arts_cnt, int player_skill_cnt, int tickle_success_cnt)
		{
			return new PvPEndCmd(type_id, season_id, pvp_id, hash_id, turn_num, battle_result, okawari_cnt, maxChain_num, chain_cnt, chain_sum_cnt, max_damage, kemostatus, arts_cnt, player_skill_cnt, tickle_success_cnt);
		}

		// Token: 0x06002C96 RID: 11414 RVA: 0x001AEB30 File Offset: 0x001ACD30
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PvPEndResponse>(__text);
		}

		// Token: 0x06002C97 RID: 11415 RVA: 0x001AEB38 File Offset: 0x001ACD38
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PvPEnd";
		}
	}
}
