using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class AccountGPGDisconnectCmd : Command
	{
		private AccountGPGDisconnectCmd()
		{
		}

		private AccountGPGDisconnectCmd(string disconnect_transfer_id)
		{
			this.request = new AccountGPGDisconnectRequest();
			((AccountGPGDisconnectRequest)this.request).disconnect_transfer_id = disconnect_transfer_id;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "common/AccountGPGDisconnect.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.DefaultEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static AccountGPGDisconnectCmd Create(string disconnect_transfer_id)
		{
			return new AccountGPGDisconnectCmd(disconnect_transfer_id);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<AccountGPGDisconnectResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/AccountGPGDisconnect";
		}

		public enum RESULT_CODE
		{
			INVALID,
			SUCCESS
		}
	}
}
