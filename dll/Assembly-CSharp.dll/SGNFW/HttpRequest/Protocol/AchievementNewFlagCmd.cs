using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000343 RID: 835
	public class AchievementNewFlagCmd : Command
	{
		// Token: 0x060028F3 RID: 10483 RVA: 0x001A8F72 File Offset: 0x001A7172
		private AchievementNewFlagCmd()
		{
		}

		// Token: 0x060028F4 RID: 10484 RVA: 0x001A8F7A File Offset: 0x001A717A
		private AchievementNewFlagCmd(int achievement_id)
		{
			this.request = new AchievementNewFlagRequest();
			((AchievementNewFlagRequest)this.request).achievement_id = achievement_id;
			this.Setting();
		}

		// Token: 0x060028F5 RID: 10485 RVA: 0x001A8FA4 File Offset: 0x001A71A4
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

		// Token: 0x060028F6 RID: 10486 RVA: 0x001A9010 File Offset: 0x001A7210
		public static AchievementNewFlagCmd Create(int achievement_id)
		{
			return new AchievementNewFlagCmd(achievement_id);
		}

		// Token: 0x060028F7 RID: 10487 RVA: 0x001A9018 File Offset: 0x001A7218
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<AchievementNewFlagResponse>(__text);
		}

		// Token: 0x060028F8 RID: 10488 RVA: 0x001A9020 File Offset: 0x001A7220
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/AchievementNewFlag";
		}
	}
}
