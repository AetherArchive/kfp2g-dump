using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200036D RID: 877
	public class CharaGrowMultiCmd : Command
	{
		// Token: 0x06002956 RID: 10582 RVA: 0x001A98E7 File Offset: 0x001A7AE7
		private CharaGrowMultiCmd()
		{
		}

		// Token: 0x06002957 RID: 10583 RVA: 0x001A98F0 File Offset: 0x001A7AF0
		private CharaGrowMultiCmd(CharaLevelUpRequest chara_level_up_request, CharaWildRelRequest chara_wild_rel_request, CharaRankUpRequest chara_rank_up_request, CharaArtsUpRequest chara_arts_up_request, CharaNanairoAbilityReleaseRequest chara_nanairo_ability_release_request, CharaLimitLevelUpRequest chara_limit_level_up_request, CharaKizunaLimitLevelUpRequest chara_kizuna_limit_level_up_request)
		{
			this.request = new CharaGrowMultiRequest();
			CharaGrowMultiRequest charaGrowMultiRequest = (CharaGrowMultiRequest)this.request;
			charaGrowMultiRequest.chara_level_up_request = chara_level_up_request;
			charaGrowMultiRequest.chara_wild_rel_request = chara_wild_rel_request;
			charaGrowMultiRequest.chara_rank_up_request = chara_rank_up_request;
			charaGrowMultiRequest.chara_arts_up_request = chara_arts_up_request;
			charaGrowMultiRequest.chara_nanairo_ability_release_request = chara_nanairo_ability_release_request;
			charaGrowMultiRequest.chara_limit_level_up_request = chara_limit_level_up_request;
			charaGrowMultiRequest.chara_kizuna_limit_level_up_request = chara_kizuna_limit_level_up_request;
			this.Setting();
		}

		// Token: 0x06002958 RID: 10584 RVA: 0x001A9954 File Offset: 0x001A7B54
		private void Setting()
		{
			base.Url = "CharaGrowMulti.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002959 RID: 10585 RVA: 0x001A99C0 File Offset: 0x001A7BC0
		public static CharaGrowMultiCmd Create(CharaGrowMultiRequest request)
		{
			return new CharaGrowMultiCmd(request.chara_level_up_request, request.chara_wild_rel_request, request.chara_rank_up_request, request.chara_arts_up_request, request.chara_nanairo_ability_release_request, request.chara_limit_level_up_request, request.chara_kizuna_limit_level_up_request);
		}

		// Token: 0x0600295A RID: 10586 RVA: 0x001A99F1 File Offset: 0x001A7BF1
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CharaGrowMultiResponse>(__text);
		}

		// Token: 0x0600295B RID: 10587 RVA: 0x001A99F9 File Offset: 0x001A7BF9
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CharaGrowMulti";
		}
	}
}
