using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003BC RID: 956
	public class FollowsListCmd : Command
	{
		// Token: 0x06002A11 RID: 10769 RVA: 0x001AAC2E File Offset: 0x001A8E2E
		private FollowsListCmd()
		{
			this.request = new FollowsListRequest();
			FollowsListRequest followsListRequest = (FollowsListRequest)this.request;
			this.Setting();
		}

		// Token: 0x06002A12 RID: 10770 RVA: 0x001AAC54 File Offset: 0x001A8E54
		private void Setting()
		{
			base.Url = "FollowsList.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002A13 RID: 10771 RVA: 0x001AACC0 File Offset: 0x001A8EC0
		public static FollowsListCmd Create()
		{
			return new FollowsListCmd();
		}

		// Token: 0x06002A14 RID: 10772 RVA: 0x001AACC7 File Offset: 0x001A8EC7
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<FollowsListResponse>(__text);
		}

		// Token: 0x06002A15 RID: 10773 RVA: 0x001AACCF File Offset: 0x001A8ECF
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/FollowsList";
		}
	}
}
