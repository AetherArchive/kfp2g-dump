using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200032D RID: 813
	public class AccountDeleteCmd : Command
	{
		// Token: 0x060028BC RID: 10428 RVA: 0x001A89F0 File Offset: 0x001A6BF0
		private AccountDeleteCmd()
		{
		}

		// Token: 0x060028BD RID: 10429 RVA: 0x001A89F8 File Offset: 0x001A6BF8
		private AccountDeleteCmd(int friend_code)
		{
			this.request = new AccountDeleteRequest();
			((AccountDeleteRequest)this.request).friend_code = friend_code;
			this.Setting();
		}

		// Token: 0x060028BE RID: 10430 RVA: 0x001A8A24 File Offset: 0x001A6C24
		private void Setting()
		{
			base.Url = "common/AccountDelete.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.DefaultEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x060028BF RID: 10431 RVA: 0x001A8A90 File Offset: 0x001A6C90
		public static AccountDeleteCmd Create(int friend_code)
		{
			return new AccountDeleteCmd(friend_code);
		}

		// Token: 0x060028C0 RID: 10432 RVA: 0x001A8A98 File Offset: 0x001A6C98
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<AccountDeleteResponse>(__text);
		}

		// Token: 0x060028C1 RID: 10433 RVA: 0x001A8AA0 File Offset: 0x001A6CA0
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/AccountDelete";
		}
	}
}
