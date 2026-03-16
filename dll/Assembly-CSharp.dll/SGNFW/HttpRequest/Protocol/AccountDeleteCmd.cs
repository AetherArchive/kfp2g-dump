using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class AccountDeleteCmd : Command
	{
		private AccountDeleteCmd()
		{
		}

		private AccountDeleteCmd(int friend_code)
		{
			this.request = new AccountDeleteRequest();
			((AccountDeleteRequest)this.request).friend_code = friend_code;
			this.Setting();
		}

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

		public static AccountDeleteCmd Create(int friend_code)
		{
			return new AccountDeleteCmd(friend_code);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<AccountDeleteResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/AccountDelete";
		}
	}
}
