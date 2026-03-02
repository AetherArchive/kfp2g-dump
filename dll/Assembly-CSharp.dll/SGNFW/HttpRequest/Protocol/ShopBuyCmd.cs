using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200052A RID: 1322
	public class ShopBuyCmd : Command
	{
		// Token: 0x06002D60 RID: 11616 RVA: 0x001AFF43 File Offset: 0x001AE143
		private ShopBuyCmd()
		{
		}

		// Token: 0x06002D61 RID: 11617 RVA: 0x001AFF4B File Offset: 0x001AE14B
		private ShopBuyCmd(int goodsId, int goodsNum)
		{
			this.request = new ShopBuyRequest();
			ShopBuyRequest shopBuyRequest = (ShopBuyRequest)this.request;
			shopBuyRequest.goodsId = goodsId;
			shopBuyRequest.goodsNum = goodsNum;
			this.Setting();
		}

		// Token: 0x06002D62 RID: 11618 RVA: 0x001AFF7C File Offset: 0x001AE17C
		private void Setting()
		{
			base.Url = "ShopBuy.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002D63 RID: 11619 RVA: 0x001AFFE8 File Offset: 0x001AE1E8
		public static ShopBuyCmd Create(int goodsId, int goodsNum)
		{
			return new ShopBuyCmd(goodsId, goodsNum);
		}

		// Token: 0x06002D64 RID: 11620 RVA: 0x001AFFF1 File Offset: 0x001AE1F1
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<ShopBuyResponse>(__text);
		}

		// Token: 0x06002D65 RID: 11621 RVA: 0x001AFFF9 File Offset: 0x001AE1F9
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/ShopBuy";
		}
	}
}
