using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PlayerLevelUpCmd : Command
	{
		private PlayerLevelUpCmd()
		{
		}

		private PlayerLevelUpCmd(long user_exp_overflow)
		{
			this.request = new PlayerLevelUpRequest();
			((PlayerLevelUpRequest)this.request).user_exp_overflow = user_exp_overflow;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "PlayerLevelUp.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static PlayerLevelUpCmd Create(long user_exp_overflow)
		{
			return new PlayerLevelUpCmd(user_exp_overflow);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PlayerLevelUpResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PlayerLevelUp";
		}
	}
}
