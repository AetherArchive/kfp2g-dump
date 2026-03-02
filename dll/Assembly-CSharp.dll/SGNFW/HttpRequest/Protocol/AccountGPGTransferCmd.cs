using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000336 RID: 822
	public class AccountGPGTransferCmd : Command
	{
		// Token: 0x060028D3 RID: 10451 RVA: 0x001A8C27 File Offset: 0x001A6E27
		private AccountGPGTransferCmd()
		{
		}

		// Token: 0x060028D4 RID: 10452 RVA: 0x001A8C2F File Offset: 0x001A6E2F
		private AccountGPGTransferCmd(string after_transfer_id, string device)
		{
			this.request = new AccountGPGTransferRequest();
			AccountGPGTransferRequest accountGPGTransferRequest = (AccountGPGTransferRequest)this.request;
			accountGPGTransferRequest.after_transfer_id = after_transfer_id;
			accountGPGTransferRequest.device = device;
			this.Setting();
		}

		// Token: 0x060028D5 RID: 10453 RVA: 0x001A8C60 File Offset: 0x001A6E60
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

		// Token: 0x060028D6 RID: 10454 RVA: 0x001A8CCC File Offset: 0x001A6ECC
		public static AccountGPGTransferCmd Create(string after_transfer_id, string device)
		{
			return new AccountGPGTransferCmd(after_transfer_id, device);
		}

		// Token: 0x060028D7 RID: 10455 RVA: 0x001A8CD5 File Offset: 0x001A6ED5
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<AccountGPGTransferResponse>(__text);
		}

		// Token: 0x060028D8 RID: 10456 RVA: 0x001A8CDD File Offset: 0x001A6EDD
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/AccountGPGTransfer";
		}

		// Token: 0x020010E2 RID: 4322
		public enum RESULT_CODE
		{
			// Token: 0x04005D4E RID: 23886
			INVALID,
			// Token: 0x04005D4F RID: 23887
			SUCCESS,
			// Token: 0x04005D50 RID: 23888
			TRANSFER_ID_NO_EXIST = 101,
			// Token: 0x04005D51 RID: 23889
			TRANSFER_NOT_EXIST_PASSWORD,
			// Token: 0x04005D52 RID: 23890
			TRANSFER_PASSWORD_DIFFER,
			// Token: 0x04005D53 RID: 23891
			TRANSFER_JUST_ONE_DMM,
			// Token: 0x04005D54 RID: 23892
			TRANSFER_WITH_DMM,
			// Token: 0x04005D55 RID: 23893
			FREEZED_ACCOUNT,
			// Token: 0x04005D56 RID: 23894
			REPAYMENT_END_PLATFORM,
			// Token: 0x04005D57 RID: 23895
			DELETED_ACCOUNT
		}
	}
}
