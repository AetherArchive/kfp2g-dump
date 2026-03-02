using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003C1 RID: 961
	public class FreezeAccountCmd : Command
	{
		// Token: 0x06002A1A RID: 10778 RVA: 0x001AACF6 File Offset: 0x001A8EF6
		private FreezeAccountCmd()
		{
			this.request = new FreezeAccountRequest();
			FreezeAccountRequest freezeAccountRequest = (FreezeAccountRequest)this.request;
			this.Setting();
		}

		// Token: 0x06002A1B RID: 10779 RVA: 0x001AAD1C File Offset: 0x001A8F1C
		private void Setting()
		{
			base.Url = "FreezeAccount.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002A1C RID: 10780 RVA: 0x001AAD88 File Offset: 0x001A8F88
		public static FreezeAccountCmd Create()
		{
			return new FreezeAccountCmd();
		}

		// Token: 0x06002A1D RID: 10781 RVA: 0x001AAD8F File Offset: 0x001A8F8F
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<FreezeAccountResponse>(__text);
		}

		// Token: 0x06002A1E RID: 10782 RVA: 0x001AAD97 File Offset: 0x001A8F97
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/FreezeAccount";
		}
	}
}
