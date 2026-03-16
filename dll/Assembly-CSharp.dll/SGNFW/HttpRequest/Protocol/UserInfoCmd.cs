using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class UserInfoCmd : Command
	{
		private UserInfoCmd()
		{
		}

		private UserInfoCmd(string name)
		{
			this.request = new UserInfoRequest();
			((UserInfoRequest)this.request).name = name;
			this.Setting();
		}

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

		public static UserInfoCmd Create(string name)
		{
			return new UserInfoCmd(name);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<UserInfoResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/UserInfo";
		}
	}
}
