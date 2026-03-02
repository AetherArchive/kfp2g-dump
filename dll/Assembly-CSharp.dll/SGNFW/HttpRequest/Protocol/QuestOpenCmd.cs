using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004EB RID: 1259
	public class QuestOpenCmd : Command
	{
		// Token: 0x06002CCD RID: 11469 RVA: 0x001AF100 File Offset: 0x001AD300
		private QuestOpenCmd()
		{
		}

		// Token: 0x06002CCE RID: 11470 RVA: 0x001AF108 File Offset: 0x001AD308
		private QuestOpenCmd(int quest_id)
		{
			this.request = new QuestOpenRequest();
			((QuestOpenRequest)this.request).quest_id = quest_id;
			this.Setting();
		}

		// Token: 0x06002CCF RID: 11471 RVA: 0x001AF134 File Offset: 0x001AD334
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

		// Token: 0x06002CD0 RID: 11472 RVA: 0x001AF1A0 File Offset: 0x001AD3A0
		public static QuestOpenCmd Create(int quest_id)
		{
			return new QuestOpenCmd(quest_id);
		}

		// Token: 0x06002CD1 RID: 11473 RVA: 0x001AF1A8 File Offset: 0x001AD3A8
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<QuestOpenResponse>(__text);
		}

		// Token: 0x06002CD2 RID: 11474 RVA: 0x001AF1B0 File Offset: 0x001AD3B0
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/QuestOpen";
		}
	}
}
