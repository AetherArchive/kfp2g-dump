using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200036E RID: 878
	public class CharaGrowMultiRequest : Request
	{
		// Token: 0x040023D0 RID: 9168
		public CharaLevelUpRequest chara_level_up_request;

		// Token: 0x040023D1 RID: 9169
		public CharaWildRelRequest chara_wild_rel_request;

		// Token: 0x040023D2 RID: 9170
		public CharaRankUpRequest chara_rank_up_request;

		// Token: 0x040023D3 RID: 9171
		public CharaArtsUpRequest chara_arts_up_request;

		// Token: 0x040023D4 RID: 9172
		public CharaNanairoAbilityReleaseRequest chara_nanairo_ability_release_request;

		// Token: 0x040023D5 RID: 9173
		public CharaLimitLevelUpRequest chara_limit_level_up_request;

		// Token: 0x040023D6 RID: 9174
		public CharaKizunaLimitLevelUpRequest chara_kizuna_limit_level_up_request;
	}
}
