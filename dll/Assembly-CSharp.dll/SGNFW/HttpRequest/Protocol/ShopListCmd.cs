using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200052E RID: 1326
	public class ShopListCmd : Command
	{
		// Token: 0x06002D69 RID: 11625 RVA: 0x001B0018 File Offset: 0x001AE218
		private ShopListCmd()
		{
			this.request = new ShopListRequest();
			ShopListRequest shopListRequest = (ShopListRequest)this.request;
			this.Setting();
		}

		// Token: 0x06002D6A RID: 11626 RVA: 0x001B0040 File Offset: 0x001AE240
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

		// Token: 0x06002D6B RID: 11627 RVA: 0x001B00AC File Offset: 0x001AE2AC
		public static ShopListCmd Create()
		{
			return new ShopListCmd();
		}

		// Token: 0x06002D6C RID: 11628 RVA: 0x001B00B3 File Offset: 0x001AE2B3
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<ShopListResponse>(__text);
		}

		// Token: 0x06002D6D RID: 11629 RVA: 0x001B00BB File Offset: 0x001AE2BB
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/ShopList";
		}
	}
}
