using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000527 RID: 1319
	public class ShopBulkBuyCmd : Command
	{
		// Token: 0x06002D58 RID: 11608 RVA: 0x001AFE7B File Offset: 0x001AE07B
		private ShopBulkBuyCmd()
		{
		}

		// Token: 0x06002D59 RID: 11609 RVA: 0x001AFE83 File Offset: 0x001AE083
		private ShopBulkBuyCmd(List<ShopData.ItemOne> goodsDataList)
		{
			this.request = new ShopBulkBuyRequest();
			((ShopBulkBuyRequest)this.request).goodsDataList = goodsDataList;
			this.Setting();
		}

		// Token: 0x06002D5A RID: 11610 RVA: 0x001AFEB0 File Offset: 0x001AE0B0
		private void Setting()
		{
			base.Url = "ShopBulkBuy.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002D5B RID: 11611 RVA: 0x001AFF1C File Offset: 0x001AE11C
		public static ShopBulkBuyCmd Create(List<ShopData.ItemOne> goodsDataList)
		{
			return new ShopBulkBuyCmd(goodsDataList);
		}

		// Token: 0x06002D5C RID: 11612 RVA: 0x001AFF24 File Offset: 0x001AE124
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<ShopBulkBuyResponse>(__text);
		}

		// Token: 0x06002D5D RID: 11613 RVA: 0x001AFF2C File Offset: 0x001AE12C
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/ShopBulkBuy";
		}
	}
}
