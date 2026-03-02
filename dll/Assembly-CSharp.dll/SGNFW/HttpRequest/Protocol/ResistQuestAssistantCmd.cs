using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000515 RID: 1301
	public class ResistQuestAssistantCmd : Command
	{
		// Token: 0x06002D2D RID: 11565 RVA: 0x001AFA77 File Offset: 0x001ADC77
		private ResistQuestAssistantCmd()
		{
		}

		// Token: 0x06002D2E RID: 11566 RVA: 0x001AFA7F File Offset: 0x001ADC7F
		private ResistQuestAssistantCmd(int resist)
		{
			this.request = new ResistQuestAssistantRequest();
			((ResistQuestAssistantRequest)this.request).resist = resist;
			this.Setting();
		}

		// Token: 0x06002D2F RID: 11567 RVA: 0x001AFAAC File Offset: 0x001ADCAC
		private void Setting()
		{
			base.Url = "ResistQuestAssistant.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002D30 RID: 11568 RVA: 0x001AFB18 File Offset: 0x001ADD18
		public static ResistQuestAssistantCmd Create(int shopAssistantCharaId)
		{
			return new ResistQuestAssistantCmd(shopAssistantCharaId);
		}

		// Token: 0x06002D31 RID: 11569 RVA: 0x001AFB20 File Offset: 0x001ADD20
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<ResistQuestAssistantResponse>(__text);
		}

		// Token: 0x06002D32 RID: 11570 RVA: 0x001AFB28 File Offset: 0x001ADD28
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/ResistQuestAssistant";
		}
	}
}
