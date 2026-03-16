using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class AchievementSelectFlagCmd : Command
	{
		private AchievementSelectFlagCmd()
		{
		}

		private AchievementSelectFlagCmd(int achievement_id)
		{
			this.request = new AchievementSelectFlagRequest();
			((AchievementSelectFlagRequest)this.request).achievement_id = achievement_id;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "AchievementSelectFlag.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static AchievementSelectFlagCmd Create(int achievement_id)
		{
			return new AchievementSelectFlagCmd(achievement_id);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<AchievementSelectFlagResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/AchievementSelectFlag";
		}
	}
}
