using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class ItemListCmd : Command
	{
		private ItemListCmd()
		{
			this.request = new ItemListRequest();
			ItemListRequest itemListRequest = (ItemListRequest)this.request;
			this.Setting();
		}

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

		public static ItemListCmd Create()
		{
			return new ItemListCmd();
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<ItemListResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/ItemList";
		}
	}
}
