using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004DF RID: 1247
	public class QuestAssistantUpdateCmd : Command
	{
		// Token: 0x06002CAD RID: 11437 RVA: 0x001AED34 File Offset: 0x001ACF34
		private QuestAssistantUpdateCmd()
		{
		}

		// Token: 0x06002CAE RID: 11438 RVA: 0x001AED3C File Offset: 0x001ACF3C
		private QuestAssistantUpdateCmd(int questAssistantCharaId)
		{
			this.request = new QuestAssistantUpdateRequest();
			((QuestAssistantUpdateRequest)this.request).quest_assistant_chara_id = questAssistantCharaId;
			this.Setting();
		}

		// Token: 0x06002CAF RID: 11439 RVA: 0x001AED68 File Offset: 0x001ACF68
		private void Setting()
		{
			base.Url = "QuestAssistantUpdate.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002CB0 RID: 11440 RVA: 0x001AEDD4 File Offset: 0x001ACFD4
		public static QuestAssistantUpdateCmd Create(int questAssistantCharaId)
		{
			return new QuestAssistantUpdateCmd(questAssistantCharaId);
		}

		// Token: 0x06002CB1 RID: 11441 RVA: 0x001AEDDC File Offset: 0x001ACFDC
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<QuestAssistantUpdateResponse>(__text);
		}

		// Token: 0x06002CB2 RID: 11442 RVA: 0x001AEDE4 File Offset: 0x001ACFE4
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/QuestAssistantUpdate";
		}
	}
}
