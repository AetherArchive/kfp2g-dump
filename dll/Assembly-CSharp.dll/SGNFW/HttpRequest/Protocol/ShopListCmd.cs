using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class ShopListCmd : Command
	{
		private ShopListCmd()
		{
			this.request = new ShopListRequest();
			ShopListRequest shopListRequest = (ShopListRequest)this.request;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "ShopList.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static ShopListCmd Create()
		{
			return new ShopListCmd();
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<ShopListResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/ShopList";
		}
	}
}
