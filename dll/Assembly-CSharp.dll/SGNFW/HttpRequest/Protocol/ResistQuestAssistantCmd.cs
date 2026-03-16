using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class ResistQuestAssistantCmd : Command
	{
		private ResistQuestAssistantCmd()
		{
		}

		private ResistQuestAssistantCmd(int resist)
		{
			this.request = new ResistQuestAssistantRequest();
			((ResistQuestAssistantRequest)this.request).resist = resist;
			this.Setting();
		}

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

		public static ResistQuestAssistantCmd Create(int shopAssistantCharaId)
		{
			return new ResistQuestAssistantCmd(shopAssistantCharaId);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<ResistQuestAssistantResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/ResistQuestAssistant";
		}
	}
}
