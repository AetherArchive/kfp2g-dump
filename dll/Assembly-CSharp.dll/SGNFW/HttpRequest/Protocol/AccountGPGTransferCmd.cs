using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class AccountGPGTransferCmd : Command
	{
		private AccountGPGTransferCmd()
		{
		}

		private AccountGPGTransferCmd(string after_transfer_id, string device)
		{
			this.request = new AccountGPGTransferRequest();
			AccountGPGTransferRequest accountGPGTransferRequest = (AccountGPGTransferRequest)this.request;
			accountGPGTransferRequest.after_transfer_id = after_transfer_id;
			accountGPGTransferRequest.device = device;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "common/AccountGPGTransfer.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.DefaultEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		public static AccountGPGTransferCmd Create(string after_transfer_id, string device)
		{
			return new AccountGPGTransferCmd(after_transfer_id, device);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<AccountGPGTransferResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/AccountGPGTransfer";
		}

		public enum RESULT_CODE
		{
			INVALID,
			SUCCESS,
			TRANSFER_ID_NO_EXIST = 101,
			TRANSFER_NOT_EXIST_PASSWORD,
			TRANSFER_PASSWORD_DIFFER,
			TRANSFER_JUST_ONE_DMM,
			TRANSFER_WITH_DMM,
			FREEZED_ACCOUNT,
			REPAYMENT_END_PLATFORM,
			DELETED_ACCOUNT
		}
	}
}
