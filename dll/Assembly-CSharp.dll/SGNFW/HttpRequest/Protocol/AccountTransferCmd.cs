using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class AccountTransferCmd : Command
	{
		private AccountTransferCmd()
		{
		}

		private AccountTransferCmd(string transfer_id, string password, int dmm_viewer_id, string device)
		{
			this.request = new AccountTransferRequest();
			AccountTransferRequest accountTransferRequest = (AccountTransferRequest)this.request;
			accountTransferRequest.transfer_id = transfer_id;
			accountTransferRequest.password = password;
			accountTransferRequest.dmm_viewer_id = dmm_viewer_id;
			accountTransferRequest.device = device;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "common/AccountTransfer.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.DefaultEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static AccountTransferCmd Create(string transfer_id, string password, int dmm_viewer_id, string device)
		{
			return new AccountTransferCmd(transfer_id, password, dmm_viewer_id, device);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<AccountTransferResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/AccountTransfer";
		}
	}
}
