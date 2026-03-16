using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class QuestSkipRecoveryCmd : Command
	{
		private QuestSkipRecoveryCmd()
		{
		}

		private QuestSkipRecoveryCmd(int quest_id, int use_item_id, int skip_recovery_num)
		{
			this.request = new QuestSkipRecoveryRequest();
			QuestSkipRecoveryRequest questSkipRecoveryRequest = (QuestSkipRecoveryRequest)this.request;
			questSkipRecoveryRequest.quest_id = quest_id;
			questSkipRecoveryRequest.use_item_id = use_item_id;
			questSkipRecoveryRequest.skip_recovery_num = skip_recovery_num;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "QuestSkipRecovery.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static QuestSkipRecoveryCmd Create(int quest_id, int use_item_id, int skip_recovery_num)
		{
			return new QuestSkipRecoveryCmd(quest_id, use_item_id, skip_recovery_num);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<QuestSkipRecoveryResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/QuestSkipRecovery";
		}
	}
}
