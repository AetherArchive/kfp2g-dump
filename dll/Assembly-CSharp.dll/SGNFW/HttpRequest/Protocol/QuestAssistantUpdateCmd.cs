using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class QuestAssistantUpdateCmd : Command
	{
		private QuestAssistantUpdateCmd()
		{
		}

		private QuestAssistantUpdateCmd(int questAssistantCharaId)
		{
			this.request = new QuestAssistantUpdateRequest();
			((QuestAssistantUpdateRequest)this.request).quest_assistant_chara_id = questAssistantCharaId;
			this.Setting();
		}

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

		public static QuestAssistantUpdateCmd Create(int questAssistantCharaId)
		{
			return new QuestAssistantUpdateCmd(questAssistantCharaId);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<QuestAssistantUpdateResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/QuestAssistantUpdate";
		}
	}
}
