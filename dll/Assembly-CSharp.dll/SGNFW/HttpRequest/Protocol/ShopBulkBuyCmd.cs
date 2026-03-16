using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class ShopBulkBuyCmd : Command
	{
		private ShopBulkBuyCmd()
		{
		}

		private ShopBulkBuyCmd(List<ShopData.ItemOne> goodsDataList)
		{
			this.request = new ShopBulkBuyRequest();
			((ShopBulkBuyRequest)this.request).goodsDataList = goodsDataList;
			this.Setting();
		}

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

		public static ShopBulkBuyCmd Create(List<ShopData.ItemOne> goodsDataList)
		{
			return new ShopBulkBuyCmd(goodsDataList);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<ShopBulkBuyResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/ShopBulkBuy";
		}
	}
}
