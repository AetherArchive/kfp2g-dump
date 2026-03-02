using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000562 RID: 1378
	public class UserInfoCmd : Command
	{
		// Token: 0x06002DE3 RID: 11747 RVA: 0x001B0CB7 File Offset: 0x001AEEB7
		private UserInfoCmd()
		{
		}

		// Token: 0x06002DE4 RID: 11748 RVA: 0x001B0CBF File Offset: 0x001AEEBF
		private UserInfoCmd(string name)
		{
			this.request = new UserInfoRequest();
			((UserInfoRequest)this.request).name = name;
			this.Setting();
		}

		// Token: 0x06002DE5 RID: 11749 RVA: 0x001B0CEC File Offset: 0x001AEEEC
		private void Setting()
		{
			base.Url = "UserInfo.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = false;
			Manager.Add(this);
		}

		// Token: 0x06002DE6 RID: 11750 RVA: 0x001B0D58 File Offset: 0x001AEF58
		public static UserInfoCmd Create(string name)
		{
			return new UserInfoCmd(name);
		}

		// Token: 0x06002DE7 RID: 11751 RVA: 0x001B0D60 File Offset: 0x001AEF60
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<UserInfoResponse>(__text);
		}

		// Token: 0x06002DE8 RID: 11752 RVA: 0x001B0D68 File Offset: 0x001AEF68
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/UserInfo";
		}
	}
}
