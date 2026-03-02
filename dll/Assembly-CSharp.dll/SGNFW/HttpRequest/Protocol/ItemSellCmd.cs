using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000403 RID: 1027
	public class ItemSellCmd : Command
	{
		// Token: 0x06002AB2 RID: 10930 RVA: 0x001ABB4E File Offset: 0x001A9D4E
		private ItemSellCmd()
		{
		}

		// Token: 0x06002AB3 RID: 10931 RVA: 0x001ABB56 File Offset: 0x001A9D56
		private ItemSellCmd(List<Item> sell_item_list)
		{
			this.request = new ItemSellRequest();
			((ItemSellRequest)this.request).sell_item_list = sell_item_list;
			this.Setting();
		}

		// Token: 0x06002AB4 RID: 10932 RVA: 0x001ABB80 File Offset: 0x001A9D80
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

		// Token: 0x06002AB5 RID: 10933 RVA: 0x001ABBEC File Offset: 0x001A9DEC
		public static ItemSellCmd Create(List<Item> sell_item_list)
		{
			return new ItemSellCmd(sell_item_list);
		}

		// Token: 0x06002AB6 RID: 10934 RVA: 0x001ABBF4 File Offset: 0x001A9DF4
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<ItemSellResponse>(__text);
		}

		// Token: 0x06002AB7 RID: 10935 RVA: 0x001ABBFC File Offset: 0x001A9DFC
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/ItemSell";
		}
	}
}
