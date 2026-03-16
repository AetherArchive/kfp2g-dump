using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class QuestCmd : Command
	{
		private QuestCmd()
		{
		}

		private QuestCmd(int quest_type)
		{
			this.request = new QuestRequest();
			((QuestRequest)this.request).quest_type = quest_type;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "Quest.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static QuestCmd Create(int quest_type)
		{
			return new QuestCmd(quest_type);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<QuestResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/Quest";
		}
	}
}
