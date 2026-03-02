using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000340 RID: 832
	public class AchievementNewAcquisitionCmd : Command
	{
		// Token: 0x060028EC RID: 10476 RVA: 0x001A8EBA File Offset: 0x001A70BA
		private AchievementNewAcquisitionCmd()
		{
			this.request = new AchievementNewAcquisitionRequest();
			AchievementNewAcquisitionRequest achievementNewAcquisitionRequest = (AchievementNewAcquisitionRequest)this.request;
			this.Setting();
		}

		// Token: 0x060028ED RID: 10477 RVA: 0x001A8EE0 File Offset: 0x001A70E0
		private void Setting()
		{
			base.Url = "AchievementNewAcquisition.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x060028EE RID: 10478 RVA: 0x001A8F4C File Offset: 0x001A714C
		public static AchievementNewAcquisitionCmd Create()
		{
			return new AchievementNewAcquisitionCmd();
		}

		// Token: 0x060028EF RID: 10479 RVA: 0x001A8F53 File Offset: 0x001A7153
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<AchievementNewAcquisitionResponse>(__text);
		}

		// Token: 0x060028F0 RID: 10480 RVA: 0x001A8F5B File Offset: 0x001A715B
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/AchievementNewAcquisition";
		}
	}
}
