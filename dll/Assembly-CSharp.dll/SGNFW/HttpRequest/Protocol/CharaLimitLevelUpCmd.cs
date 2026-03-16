using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class CharaLimitLevelUpCmd : Command
	{
		private CharaLimitLevelUpCmd()
		{
		}

		private CharaLimitLevelUpCmd(int chara_id, int target_level_limit_id)
		{
			this.request = new CharaLimitLevelUpRequest();
			CharaLimitLevelUpRequest charaLimitLevelUpRequest = (CharaLimitLevelUpRequest)this.request;
			charaLimitLevelUpRequest.chara_id = chara_id;
			charaLimitLevelUpRequest.target_level_limit_id = target_level_limit_id;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "CharaLimitLevelUp.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static CharaLimitLevelUpCmd Create(int chara_id, int target_level_limit_id)
		{
			return new CharaLimitLevelUpCmd(chara_id, target_level_limit_id);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CharaLimitLevelUpResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CharaLimitLevelUp";
		}
	}
}
