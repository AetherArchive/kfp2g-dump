using System;
using System.Collections.Generic;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003C8 RID: 968
	public class FriendsDataCmd : Command
	{
		// Token: 0x06002A2A RID: 10794 RVA: 0x001AAE7B File Offset: 0x001A907B
		private FriendsDataCmd()
		{
		}

		// Token: 0x06002A2B RID: 10795 RVA: 0x001AAE83 File Offset: 0x001A9083
		private FriendsDataCmd(List<FriendsData> friends_data)
		{
			this.request = new FriendsDataRequest();
			((FriendsDataRequest)this.request).friends_data = friends_data;
			this.Setting();
		}

		// Token: 0x06002A2C RID: 10796 RVA: 0x001AAEB0 File Offset: 0x001A90B0
		private void Setting()
		{
			base.Url = "FriendsData.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002A2D RID: 10797 RVA: 0x001AAF1C File Offset: 0x001A911C
		public static FriendsDataCmd Create(List<FriendsData> friends_data)
		{
			return new FriendsDataCmd(friends_data);
		}

		// Token: 0x06002A2E RID: 10798 RVA: 0x001AAF24 File Offset: 0x001A9124
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<FriendsDataResponse>(__text);
		}

		// Token: 0x06002A2F RID: 10799 RVA: 0x001AAF2C File Offset: 0x001A912C
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/FriendsData";
		}
	}
}
