using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004FC RID: 1276
	public class QuestSkipRecoveryCmd : Command
	{
		// Token: 0x06002CF6 RID: 11510 RVA: 0x001AF528 File Offset: 0x001AD728
		private QuestSkipRecoveryCmd()
		{
		}

		// Token: 0x06002CF7 RID: 11511 RVA: 0x001AF530 File Offset: 0x001AD730
		private QuestSkipRecoveryCmd(int quest_id, int use_item_id, int skip_recovery_num)
		{
			this.request = new QuestSkipRecoveryRequest();
			QuestSkipRecoveryRequest questSkipRecoveryRequest = (QuestSkipRecoveryRequest)this.request;
			questSkipRecoveryRequest.quest_id = quest_id;
			questSkipRecoveryRequest.use_item_id = use_item_id;
			questSkipRecoveryRequest.skip_recovery_num = skip_recovery_num;
			this.Setting();
		}

		// Token: 0x06002CF8 RID: 11512 RVA: 0x001AF568 File Offset: 0x001AD768
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

		// Token: 0x06002CF9 RID: 11513 RVA: 0x001AF5D4 File Offset: 0x001AD7D4
		public static QuestSkipRecoveryCmd Create(int quest_id, int use_item_id, int skip_recovery_num)
		{
			return new QuestSkipRecoveryCmd(quest_id, use_item_id, skip_recovery_num);
		}

		// Token: 0x06002CFA RID: 11514 RVA: 0x001AF5DE File Offset: 0x001AD7DE
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<QuestSkipRecoveryResponse>(__text);
		}

		// Token: 0x06002CFB RID: 11515 RVA: 0x001AF5E6 File Offset: 0x001AD7E6
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/QuestSkipRecovery";
		}
	}
}
