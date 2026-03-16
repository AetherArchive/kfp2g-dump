using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class CharaKizunaLimitLevelUpCmd : Command
	{
		private CharaKizunaLimitLevelUpCmd()
		{
		}

		private CharaKizunaLimitLevelUpCmd(int chara_id)
		{
			this.request = new CharaKizunaLimitLevelUpRequest();
			((CharaKizunaLimitLevelUpRequest)this.request).chara_id = chara_id;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "CharaKizunaLimitLevelUp.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static CharaKizunaLimitLevelUpCmd Create(int chara_id)
		{
			return new CharaKizunaLimitLevelUpCmd(chara_id);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<CharaKizunaLimitLevelUpResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/CharaKizunaLimitLevelUp";
		}
	}
}
