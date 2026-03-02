using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000400 RID: 1024
	public class ItemListCmd : Command
	{
		// Token: 0x06002AAB RID: 10923 RVA: 0x001ABA94 File Offset: 0x001A9C94
		private ItemListCmd()
		{
			this.request = new ItemListRequest();
			ItemListRequest itemListRequest = (ItemListRequest)this.request;
			this.Setting();
		}

		// Token: 0x06002AAC RID: 10924 RVA: 0x001ABABC File Offset: 0x001A9CBC
		private void Setting()
		{
			base.Url = "ItemList.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002AAD RID: 10925 RVA: 0x001ABB28 File Offset: 0x001A9D28
		public static ItemListCmd Create()
		{
			return new ItemListCmd();
		}

		// Token: 0x06002AAE RID: 10926 RVA: 0x001ABB2F File Offset: 0x001A9D2F
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<ItemListResponse>(__text);
		}

		// Token: 0x06002AAF RID: 10927 RVA: 0x001ABB37 File Offset: 0x001A9D37
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/ItemList";
		}
	}
}
