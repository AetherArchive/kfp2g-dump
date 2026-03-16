using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class AccountGPGConnectCmd : Command
	{
		private AccountGPGConnectCmd()
		{
			this.request = new AccountGPGConnectRequest();
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "common/AccountGPGConnect.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.DefaultEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static AccountGPGConnectCmd Create()
		{
			return new AccountGPGConnectCmd();
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<AccountGPGConnectResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/AccountGPGConnect";
		}

		public enum RESULT_CODE
		{
			INVALID,
			SUCCESS
		}
	}
}
