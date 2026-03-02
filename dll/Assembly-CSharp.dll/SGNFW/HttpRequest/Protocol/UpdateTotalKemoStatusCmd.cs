using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200055E RID: 1374
	public class UpdateTotalKemoStatusCmd : Command
	{
		// Token: 0x06002DDA RID: 11738 RVA: 0x001B0BEA File Offset: 0x001AEDEA
		private UpdateTotalKemoStatusCmd()
		{
		}

		// Token: 0x06002DDB RID: 11739 RVA: 0x001B0BF2 File Offset: 0x001AEDF2
		private UpdateTotalKemoStatusCmd(int kemostatus)
		{
			this.request = new UpdateTotalKemoStatusRequest();
			((UpdateTotalKemoStatusRequest)this.request).kemostatus = kemostatus;
			this.Setting();
		}

		// Token: 0x06002DDC RID: 11740 RVA: 0x001B0C1C File Offset: 0x001AEE1C
		private void Setting()
		{
			base.Url = "UpdateTotalKemoStatus.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002DDD RID: 11741 RVA: 0x001B0C88 File Offset: 0x001AEE88
		public static UpdateTotalKemoStatusCmd Create(int kemostatus)
		{
			return new UpdateTotalKemoStatusCmd(kemostatus);
		}

		// Token: 0x06002DDE RID: 11742 RVA: 0x001B0C90 File Offset: 0x001AEE90
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<UpdateTotalKemoStatusResponse>(__text);
		}

		// Token: 0x06002DDF RID: 11743 RVA: 0x001B0C98 File Offset: 0x001AEE98
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/UpdateTotalKemoStatus";
		}
	}
}
