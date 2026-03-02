using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004E8 RID: 1256
	public class QuestLimitRecoveryCmd : Command
	{
		// Token: 0x06002CC5 RID: 11461 RVA: 0x001AF031 File Offset: 0x001AD231
		private QuestLimitRecoveryCmd()
		{
		}

		// Token: 0x06002CC6 RID: 11462 RVA: 0x001AF039 File Offset: 0x001AD239
		private QuestLimitRecoveryCmd(int quest_id, bool is_raid = false)
		{
			this.request = new QuestLimitRecoveryRequest();
			QuestLimitRecoveryRequest questLimitRecoveryRequest = (QuestLimitRecoveryRequest)this.request;
			questLimitRecoveryRequest.quest_id = quest_id;
			questLimitRecoveryRequest.is_raid = is_raid;
			this.Setting();
		}

		// Token: 0x06002CC7 RID: 11463 RVA: 0x001AF06C File Offset: 0x001AD26C
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

		// Token: 0x06002CC8 RID: 11464 RVA: 0x001AF0D8 File Offset: 0x001AD2D8
		public static QuestLimitRecoveryCmd Create(int quest_id, bool is_raid = false)
		{
			return new QuestLimitRecoveryCmd(quest_id, is_raid);
		}

		// Token: 0x06002CC9 RID: 11465 RVA: 0x001AF0E1 File Offset: 0x001AD2E1
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<QuestLimitRecoveryResponse>(__text);
		}

		// Token: 0x06002CCA RID: 11466 RVA: 0x001AF0E9 File Offset: 0x001AD2E9
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/QuestLimitRecovery";
		}
	}
}
