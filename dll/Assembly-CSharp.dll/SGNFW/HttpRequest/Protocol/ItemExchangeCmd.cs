using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003FD RID: 1021
	public class ItemExchangeCmd : Command
	{
		// Token: 0x06002AA3 RID: 10915 RVA: 0x001AB9C7 File Offset: 0x001A9BC7
		private ItemExchangeCmd()
		{
		}

		// Token: 0x06002AA4 RID: 10916 RVA: 0x001AB9CF File Offset: 0x001A9BCF
		private ItemExchangeCmd(int executeCount, int targetItemId)
		{
			this.request = new ItemExchangeRequest();
			ItemExchangeRequest itemExchangeRequest = (ItemExchangeRequest)this.request;
			itemExchangeRequest.executeCount = executeCount;
			itemExchangeRequest.targetItemId = targetItemId;
			this.Setting();
		}

		// Token: 0x06002AA5 RID: 10917 RVA: 0x001ABA00 File Offset: 0x001A9C00
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

		// Token: 0x06002AA6 RID: 10918 RVA: 0x001ABA6C File Offset: 0x001A9C6C
		public static ItemExchangeCmd Create(int executeCount, int targetItemId)
		{
			return new ItemExchangeCmd(executeCount, targetItemId);
		}

		// Token: 0x06002AA7 RID: 10919 RVA: 0x001ABA75 File Offset: 0x001A9C75
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<ItemExchangeResponse>(__text);
		}

		// Token: 0x06002AA8 RID: 10920 RVA: 0x001ABA7D File Offset: 0x001A9C7D
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/ItemExchange";
		}
	}
}
