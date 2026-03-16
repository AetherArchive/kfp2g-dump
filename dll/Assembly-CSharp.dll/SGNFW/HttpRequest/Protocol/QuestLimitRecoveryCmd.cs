using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class QuestLimitRecoveryCmd : Command
	{
		private QuestLimitRecoveryCmd()
		{
		}

		private QuestLimitRecoveryCmd(int quest_id, bool is_raid = false)
		{
			this.request = new QuestLimitRecoveryRequest();
			QuestLimitRecoveryRequest questLimitRecoveryRequest = (QuestLimitRecoveryRequest)this.request;
			questLimitRecoveryRequest.quest_id = quest_id;
			questLimitRecoveryRequest.is_raid = is_raid;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "QuestLimitRecovery.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static QuestLimitRecoveryCmd Create(int quest_id, bool is_raid = false)
		{
			return new QuestLimitRecoveryCmd(quest_id, is_raid);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<QuestLimitRecoveryResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/QuestLimitRecovery";
		}
	}
}
