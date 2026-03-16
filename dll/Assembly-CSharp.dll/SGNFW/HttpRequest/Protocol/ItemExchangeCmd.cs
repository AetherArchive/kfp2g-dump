using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class ItemExchangeCmd : Command
	{
		private ItemExchangeCmd()
		{
		}

		private ItemExchangeCmd(int executeCount, int targetItemId)
		{
			this.request = new ItemExchangeRequest();
			ItemExchangeRequest itemExchangeRequest = (ItemExchangeRequest)this.request;
			itemExchangeRequest.executeCount = executeCount;
			itemExchangeRequest.targetItemId = targetItemId;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "ItemExchange.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static ItemExchangeCmd Create(int executeCount, int targetItemId)
		{
			return new ItemExchangeCmd(executeCount, targetItemId);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<ItemExchangeResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/ItemExchange";
		}
	}
}
