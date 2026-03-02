using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000346 RID: 838
	public class AchievementSelectFlagCmd : Command
	{
		// Token: 0x060028FB RID: 10491 RVA: 0x001A9037 File Offset: 0x001A7237
		private AchievementSelectFlagCmd()
		{
		}

		// Token: 0x060028FC RID: 10492 RVA: 0x001A903F File Offset: 0x001A723F
		private AchievementSelectFlagCmd(int achievement_id)
		{
			this.request = new AchievementSelectFlagRequest();
			((AchievementSelectFlagRequest)this.request).achievement_id = achievement_id;
			this.Setting();
		}

		// Token: 0x060028FD RID: 10493 RVA: 0x001A906C File Offset: 0x001A726C
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

		// Token: 0x060028FE RID: 10494 RVA: 0x001A90D8 File Offset: 0x001A72D8
		public static AchievementSelectFlagCmd Create(int achievement_id)
		{
			return new AchievementSelectFlagCmd(achievement_id);
		}

		// Token: 0x060028FF RID: 10495 RVA: 0x001A90E0 File Offset: 0x001A72E0
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<AchievementSelectFlagResponse>(__text);
		}

		// Token: 0x06002900 RID: 10496 RVA: 0x001A90E8 File Offset: 0x001A72E8
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/AchievementSelectFlag";
		}
	}
}
