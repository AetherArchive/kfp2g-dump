using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class CharaGrowMultiRequest : Request
	{
		public CharaLevelUpRequest chara_level_up_request;

		public CharaWildRelRequest chara_wild_rel_request;

		public CharaRankUpRequest chara_rank_up_request;

		public CharaArtsUpRequest chara_arts_up_request;

		public CharaNanairoAbilityReleaseRequest chara_nanairo_ability_release_request;

		public CharaLimitLevelUpRequest chara_limit_level_up_request;

		public CharaKizunaLimitLevelUpRequest chara_kizuna_limit_level_up_request;
	}
}
