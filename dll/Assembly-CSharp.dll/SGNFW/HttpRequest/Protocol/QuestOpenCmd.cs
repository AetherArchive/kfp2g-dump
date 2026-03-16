using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class QuestOpenCmd : Command
	{
		private QuestOpenCmd()
		{
		}

		private QuestOpenCmd(int quest_id)
		{
			this.request = new QuestOpenRequest();
			((QuestOpenRequest)this.request).quest_id = quest_id;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "QuestOpen.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static QuestOpenCmd Create(int quest_id)
		{
			return new QuestOpenCmd(quest_id);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<QuestOpenResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/QuestOpen";
		}
	}
}
