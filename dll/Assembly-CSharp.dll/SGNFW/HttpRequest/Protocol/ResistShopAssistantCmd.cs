using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000518 RID: 1304
	public class ResistShopAssistantCmd : Command
	{
		// Token: 0x06002D35 RID: 11573 RVA: 0x001AFB3F File Offset: 0x001ADD3F
		private ResistShopAssistantCmd()
		{
		}

		// Token: 0x06002D36 RID: 11574 RVA: 0x001AFB47 File Offset: 0x001ADD47
		private ResistShopAssistantCmd(int resist)
		{
			this.request = new ResistShopAssistantRequest();
			((ResistShopAssistantRequest)this.request).resist = resist;
			this.Setting();
		}

		// Token: 0x06002D37 RID: 11575 RVA: 0x001AFB74 File Offset: 0x001ADD74
		private void Setting()
		{
			base.Url = "ResistShopAssistant.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002D38 RID: 11576 RVA: 0x001AFBE0 File Offset: 0x001ADDE0
		public static ResistShopAssistantCmd Create(int shopAssistantCharaId)
		{
			return new ResistShopAssistantCmd(shopAssistantCharaId);
		}

		// Token: 0x06002D39 RID: 11577 RVA: 0x001AFBE8 File Offset: 0x001ADDE8
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<ResistShopAssistantResponse>(__text);
		}

		// Token: 0x06002D3A RID: 11578 RVA: 0x001AFBF0 File Offset: 0x001ADDF0
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/ResistShopAssistant";
		}
	}
}
