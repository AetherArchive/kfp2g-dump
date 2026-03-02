using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000524 RID: 1316
	public class ShopAssistantUpdateCmd : Command
	{
		// Token: 0x06002D50 RID: 11600 RVA: 0x001AFDB3 File Offset: 0x001ADFB3
		private ShopAssistantUpdateCmd()
		{
		}

		// Token: 0x06002D51 RID: 11601 RVA: 0x001AFDBB File Offset: 0x001ADFBB
		private ShopAssistantUpdateCmd(int shopAssistantCharaId)
		{
			this.request = new ShopAssistantUpdateRequest();
			((ShopAssistantUpdateRequest)this.request).shop_assistant_chara_id = shopAssistantCharaId;
			this.Setting();
		}

		// Token: 0x06002D52 RID: 11602 RVA: 0x001AFDE8 File Offset: 0x001ADFE8
		private void Setting()
		{
			base.Url = "ShopAssistantUpdate.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002D53 RID: 11603 RVA: 0x001AFE54 File Offset: 0x001AE054
		public static ShopAssistantUpdateCmd Create(int shopAssistantCharaId)
		{
			return new ShopAssistantUpdateCmd(shopAssistantCharaId);
		}

		// Token: 0x06002D54 RID: 11604 RVA: 0x001AFE5C File Offset: 0x001AE05C
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<ShopAssistantUpdateResponse>(__text);
		}

		// Token: 0x06002D55 RID: 11605 RVA: 0x001AFE64 File Offset: 0x001AE064
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/ShopAssistantUpdate";
		}
	}
}
