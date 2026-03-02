using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000333 RID: 819
	public class AccountGPGDisconnectCmd : Command
	{
		// Token: 0x060028CB RID: 10443 RVA: 0x001A8B62 File Offset: 0x001A6D62
		private AccountGPGDisconnectCmd()
		{
		}

		// Token: 0x060028CC RID: 10444 RVA: 0x001A8B6A File Offset: 0x001A6D6A
		private AccountGPGDisconnectCmd(string disconnect_transfer_id)
		{
			this.request = new AccountGPGDisconnectRequest();
			((AccountGPGDisconnectRequest)this.request).disconnect_transfer_id = disconnect_transfer_id;
			this.Setting();
		}

		// Token: 0x060028CD RID: 10445 RVA: 0x001A8B94 File Offset: 0x001A6D94
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

		// Token: 0x060028CE RID: 10446 RVA: 0x001A8C00 File Offset: 0x001A6E00
		public static AccountGPGDisconnectCmd Create(string disconnect_transfer_id)
		{
			return new AccountGPGDisconnectCmd(disconnect_transfer_id);
		}

		// Token: 0x060028CF RID: 10447 RVA: 0x001A8C08 File Offset: 0x001A6E08
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<AccountGPGDisconnectResponse>(__text);
		}

		// Token: 0x060028D0 RID: 10448 RVA: 0x001A8C10 File Offset: 0x001A6E10
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/AccountGPGDisconnect";
		}

		// Token: 0x020010E1 RID: 4321
		public enum RESULT_CODE
		{
			// Token: 0x04005D4B RID: 23883
			INVALID,
			// Token: 0x04005D4C RID: 23884
			SUCCESS
		}
	}
}
