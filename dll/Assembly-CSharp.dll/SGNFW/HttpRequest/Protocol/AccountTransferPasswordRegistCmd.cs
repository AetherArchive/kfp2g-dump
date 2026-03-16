using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class AccountTransferPasswordRegistCmd : Command
	{
		private AccountTransferPasswordRegistCmd()
		{
		}

		private AccountTransferPasswordRegistCmd(string transfer_id, string password, string uuid, string secure_id)
		{
			this.request = new AccountTransferPasswordRegistRequest();
			AccountTransferPasswordRegistRequest accountTransferPasswordRegistRequest = (AccountTransferPasswordRegistRequest)this.request;
			accountTransferPasswordRegistRequest.transfer_id = transfer_id;
			accountTransferPasswordRegistRequest.password = password;
			accountTransferPasswordRegistRequest.uuid = uuid;
			accountTransferPasswordRegistRequest.secure_id = secure_id;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "common/AccountTransferPasswordRegist.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static AccountTransferPasswordRegistCmd Create(string transfer_id, string password, string uuid, string secure_id)
		{
			return new AccountTransferPasswordRegistCmd(transfer_id, password, uuid, secure_id);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<AccountTransferPasswordRegistResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/AccountTransferPasswordRegist";
		}
	}
}
