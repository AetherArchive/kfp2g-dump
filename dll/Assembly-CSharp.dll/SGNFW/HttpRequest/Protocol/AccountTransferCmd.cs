using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000339 RID: 825
	public class AccountTransferCmd : Command
	{
		// Token: 0x060028DB RID: 10459 RVA: 0x001A8CF4 File Offset: 0x001A6EF4
		private AccountTransferCmd()
		{
		}

		// Token: 0x060028DC RID: 10460 RVA: 0x001A8CFC File Offset: 0x001A6EFC
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

		// Token: 0x060028DD RID: 10461 RVA: 0x001A8D3C File Offset: 0x001A6F3C
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

		// Token: 0x060028DE RID: 10462 RVA: 0x001A8DA8 File Offset: 0x001A6FA8
		public static AccountTransferCmd Create(string transfer_id, string password, int dmm_viewer_id, string device)
		{
			return new AccountTransferCmd(transfer_id, password, dmm_viewer_id, device);
		}

		// Token: 0x060028DF RID: 10463 RVA: 0x001A8DB3 File Offset: 0x001A6FB3
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<AccountTransferResponse>(__text);
		}

		// Token: 0x060028E0 RID: 10464 RVA: 0x001A8DBB File Offset: 0x001A6FBB
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/AccountTransfer";
		}
	}
}
