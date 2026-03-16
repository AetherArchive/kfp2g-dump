using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class ItemSellCmd : Command
	{
		private ItemSellCmd()
		{
		}

		private ItemSellCmd(List<Item> sell_item_list)
		{
			this.request = new ItemSellRequest();
			((ItemSellRequest)this.request).sell_item_list = sell_item_list;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "ItemSell.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static ItemSellCmd Create(List<Item> sell_item_list)
		{
			return new ItemSellCmd(sell_item_list);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<ItemSellResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/ItemSell";
		}
	}
}
