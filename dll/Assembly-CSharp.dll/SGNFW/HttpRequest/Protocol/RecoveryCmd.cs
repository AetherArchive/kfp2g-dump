using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200050B RID: 1291
	public class RecoveryCmd : Command
	{
		// Token: 0x06002D14 RID: 11540 RVA: 0x001AF7FC File Offset: 0x001AD9FC
		private RecoveryCmd()
		{
		}

		// Token: 0x06002D15 RID: 11541 RVA: 0x001AF804 File Offset: 0x001ADA04
		private RecoveryCmd(int itemId, int itemNum, int category)
		{
			this.request = new RecoveryRequest();
			RecoveryRequest recoveryRequest = (RecoveryRequest)this.request;
			recoveryRequest.itemId = itemId;
			recoveryRequest.itemNum = itemNum;
			recoveryRequest.category = category;
			this.Setting();
		}

		// Token: 0x06002D16 RID: 11542 RVA: 0x001AF83C File Offset: 0x001ADA3C
		private void Setting()
		{
			base.Url = "Recovery.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002D17 RID: 11543 RVA: 0x001AF8A8 File Offset: 0x001ADAA8
		public static RecoveryCmd Create(int itemId, int itemNum, int category)
		{
			return new RecoveryCmd(itemId, itemNum, category);
		}

		// Token: 0x06002D18 RID: 11544 RVA: 0x001AF8B2 File Offset: 0x001ADAB2
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<RecoveryResponse>(__text);
		}

		// Token: 0x06002D19 RID: 11545 RVA: 0x001AF8BA File Offset: 0x001ADABA
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/Recovery";
		}
	}
}
