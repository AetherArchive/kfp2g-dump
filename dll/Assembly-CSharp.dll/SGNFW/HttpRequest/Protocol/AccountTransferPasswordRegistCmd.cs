using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200033C RID: 828
	public class AccountTransferPasswordRegistCmd : Command
	{
		// Token: 0x060028E3 RID: 10467 RVA: 0x001A8DD2 File Offset: 0x001A6FD2
		private AccountTransferPasswordRegistCmd()
		{
		}

		// Token: 0x060028E4 RID: 10468 RVA: 0x001A8DDA File Offset: 0x001A6FDA
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

		// Token: 0x060028E5 RID: 10469 RVA: 0x001A8E1C File Offset: 0x001A701C
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

		// Token: 0x060028E6 RID: 10470 RVA: 0x001A8E88 File Offset: 0x001A7088
		public static AccountTransferPasswordRegistCmd Create(string transfer_id, string password, string uuid, string secure_id)
		{
			return new AccountTransferPasswordRegistCmd(transfer_id, password, uuid, secure_id);
		}

		// Token: 0x060028E7 RID: 10471 RVA: 0x001A8E93 File Offset: 0x001A7093
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<AccountTransferPasswordRegistResponse>(__text);
		}

		// Token: 0x060028E8 RID: 10472 RVA: 0x001A8E9B File Offset: 0x001A709B
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/AccountTransferPasswordRegist";
		}
	}
}
