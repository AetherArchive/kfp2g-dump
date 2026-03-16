using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class ShopBuyCmd : Command
	{
		private ShopBuyCmd()
		{
		}

		private ShopBuyCmd(int goodsId, int goodsNum)
		{
			this.request = new ShopBuyRequest();
			ShopBuyRequest shopBuyRequest = (ShopBuyRequest)this.request;
			shopBuyRequest.goodsId = goodsId;
			shopBuyRequest.goodsNum = goodsNum;
			this.Setting();
		}

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

		public static ShopBuyCmd Create(int goodsId, int goodsNum)
		{
			return new ShopBuyCmd(goodsId, goodsNum);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<ShopBuyResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/ShopBuy";
		}
	}
}
