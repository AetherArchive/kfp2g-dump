using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004E2 RID: 1250
	public class QuestCmd : Command
	{
		// Token: 0x06002CB5 RID: 11445 RVA: 0x001AEDFB File Offset: 0x001ACFFB
		private QuestCmd()
		{
		}

		// Token: 0x06002CB6 RID: 11446 RVA: 0x001AEE03 File Offset: 0x001AD003
		private QuestCmd(int quest_type)
		{
			this.request = new QuestRequest();
			((QuestRequest)this.request).quest_type = quest_type;
			this.Setting();
		}

		// Token: 0x06002CB7 RID: 11447 RVA: 0x001AEE30 File Offset: 0x001AD030
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

		// Token: 0x06002CB8 RID: 11448 RVA: 0x001AEE9C File Offset: 0x001AD09C
		public static QuestCmd Create(int quest_type)
		{
			return new QuestCmd(quest_type);
		}

		// Token: 0x06002CB9 RID: 11449 RVA: 0x001AEEA4 File Offset: 0x001AD0A4
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<QuestResponse>(__text);
		}

		// Token: 0x06002CBA RID: 11450 RVA: 0x001AEEAC File Offset: 0x001AD0AC
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/Quest";
		}
	}
}
