using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class ShopAssistantUpdateCmd : Command
	{
		private ShopAssistantUpdateCmd()
		{
		}

		private ShopAssistantUpdateCmd(int shopAssistantCharaId)
		{
			this.request = new ShopAssistantUpdateRequest();
			((ShopAssistantUpdateRequest)this.request).shop_assistant_chara_id = shopAssistantCharaId;
			this.Setting();
		}

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

		public static ShopAssistantUpdateCmd Create(int shopAssistantCharaId)
		{
			return new ShopAssistantUpdateCmd(shopAssistantCharaId);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<ShopAssistantUpdateResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/ShopAssistantUpdate";
		}
	}
}
