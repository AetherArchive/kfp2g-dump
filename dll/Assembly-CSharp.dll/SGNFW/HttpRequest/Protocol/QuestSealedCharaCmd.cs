using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004F5 RID: 1269
	public class QuestSealedCharaCmd : Command
	{
		// Token: 0x06002CE6 RID: 11494 RVA: 0x001AF35F File Offset: 0x001AD55F
		private QuestSealedCharaCmd()
		{
			this.request = new QuestSealedCharaRequest();
			QuestSealedCharaRequest questSealedCharaRequest = (QuestSealedCharaRequest)this.request;
			this.Setting();
		}

		// Token: 0x06002CE7 RID: 11495 RVA: 0x001AF384 File Offset: 0x001AD584
		private void Setting()
		{
			base.Url = "QuestSealedChara.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002CE8 RID: 11496 RVA: 0x001AF3F0 File Offset: 0x001AD5F0
		public static QuestSealedCharaCmd Create()
		{
			return new QuestSealedCharaCmd();
		}

		// Token: 0x06002CE9 RID: 11497 RVA: 0x001AF3F7 File Offset: 0x001AD5F7
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<QuestSealedCharaResponse>(__text);
		}

		// Token: 0x06002CEA RID: 11498 RVA: 0x001AF3FF File Offset: 0x001AD5FF
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/QuestSealedChara";
		}
	}
}
