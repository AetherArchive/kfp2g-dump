using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class AchievementNewFlagCmd : Command
	{
		private AchievementNewFlagCmd()
		{
		}

		private AchievementNewFlagCmd(int achievement_id)
		{
			this.request = new AchievementNewFlagRequest();
			((AchievementNewFlagRequest)this.request).achievement_id = achievement_id;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "AchievementNewFlag.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static AchievementNewFlagCmd Create(int achievement_id)
		{
			return new AchievementNewFlagCmd(achievement_id);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<AchievementNewFlagResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/AchievementNewFlag";
		}
	}
}
