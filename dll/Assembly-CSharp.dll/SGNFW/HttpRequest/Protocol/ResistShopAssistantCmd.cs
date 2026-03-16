using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class ResistShopAssistantCmd : Command
	{
		private ResistShopAssistantCmd()
		{
		}

		private ResistShopAssistantCmd(int resist)
		{
			this.request = new ResistShopAssistantRequest();
			((ResistShopAssistantRequest)this.request).resist = resist;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "ResistShopAssistant.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static ResistShopAssistantCmd Create(int shopAssistantCharaId)
		{
			return new ResistShopAssistantCmd(shopAssistantCharaId);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<ResistShopAssistantResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/ResistShopAssistant";
		}
	}
}
