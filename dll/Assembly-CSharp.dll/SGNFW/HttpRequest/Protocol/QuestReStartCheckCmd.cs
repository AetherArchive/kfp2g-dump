using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004EE RID: 1262
	public class QuestReStartCheckCmd : Command
	{
		// Token: 0x06002CD5 RID: 11477 RVA: 0x001AF1C7 File Offset: 0x001AD3C7
		private QuestReStartCheckCmd()
		{
		}

		// Token: 0x06002CD6 RID: 11478 RVA: 0x001AF1CF File Offset: 0x001AD3CF
		private QuestReStartCheckCmd(long hash_id)
		{
			this.request = new QuestReStartCheckRequest();
			((QuestReStartCheckRequest)this.request).hash_id = hash_id;
			this.Setting();
		}

		// Token: 0x06002CD7 RID: 11479 RVA: 0x001AF1FC File Offset: 0x001AD3FC
		private void Setting()
		{
			base.Url = "QuestReStartCheck.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002CD8 RID: 11480 RVA: 0x001AF268 File Offset: 0x001AD468
		public static QuestReStartCheckCmd Create(long hash_id)
		{
			return new QuestReStartCheckCmd(hash_id);
		}

		// Token: 0x06002CD9 RID: 11481 RVA: 0x001AF270 File Offset: 0x001AD470
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<QuestReStartCheckResponse>(__text);
		}

		// Token: 0x06002CDA RID: 11482 RVA: 0x001AF278 File Offset: 0x001AD478
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/QuestReStartCheck";
		}
	}
}
