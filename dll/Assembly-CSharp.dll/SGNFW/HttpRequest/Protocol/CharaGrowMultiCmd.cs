using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class CharaGrowMultiCmd : Command
	{
		private CharaGrowMultiCmd()
		{
		}

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

		public static CharaGrowMultiCmd Create(CharaGrowMultiRequest request)
		{
			return new CharaGrowMultiCmd(request.chara_level_up_request, request.chara_wild_rel_request, request.chara_rank_up_request, request.chara_arts_up_request, request.chara_nanairo_ability_release_request, request.chara_limit_level_up_request, request.chara_kizuna_limit_level_up_request);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CharaGrowMultiResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CharaGrowMulti";
		}
	}
}
