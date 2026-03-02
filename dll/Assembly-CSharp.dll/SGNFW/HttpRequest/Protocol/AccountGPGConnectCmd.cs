using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000330 RID: 816
	public class AccountGPGConnectCmd : Command
	{
		// Token: 0x060028C4 RID: 10436 RVA: 0x001A8AB7 File Offset: 0x001A6CB7
		private AccountGPGConnectCmd()
		{
			this.request = new AccountGPGConnectRequest();
			this.Setting();
		}

		// Token: 0x060028C5 RID: 10437 RVA: 0x001A8AD0 File Offset: 0x001A6CD0
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

		// Token: 0x060028C6 RID: 10438 RVA: 0x001A8B3C File Offset: 0x001A6D3C
		public static AccountGPGConnectCmd Create()
		{
			return new AccountGPGConnectCmd();
		}

		// Token: 0x060028C7 RID: 10439 RVA: 0x001A8B43 File Offset: 0x001A6D43
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<AccountGPGConnectResponse>(__text);
		}

		// Token: 0x060028C8 RID: 10440 RVA: 0x001A8B4B File Offset: 0x001A6D4B
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/AccountGPGConnect";
		}

		// Token: 0x020010E0 RID: 4320
		public enum RESULT_CODE
		{
			// Token: 0x04005D48 RID: 23880
			INVALID,
			// Token: 0x04005D49 RID: 23881
			SUCCESS
		}
	}
}
