using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004A8 RID: 1192
	public class PlatformStatusCheckCmd : Command
	{
		// Token: 0x06002C29 RID: 11305 RVA: 0x001ADF3B File Offset: 0x001AC13B
		private PlatformStatusCheckCmd()
		{
			this.request = new PlatformStatusCheckRequest();
			PlatformStatusCheckRequest platformStatusCheckRequest = (PlatformStatusCheckRequest)this.request;
			this.Setting();
		}

		// Token: 0x06002C2A RID: 11306 RVA: 0x001ADF60 File Offset: 0x001AC160
		private void Setting()
		{
			base.Url = "PlatformStatusCheck.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.AccountEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002C2B RID: 11307 RVA: 0x001ADFCC File Offset: 0x001AC1CC
		public static PlatformStatusCheckCmd Create()
		{
			return new PlatformStatusCheckCmd();
		}

		// Token: 0x06002C2C RID: 11308 RVA: 0x001ADFD3 File Offset: 0x001AC1D3
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<PlatformStatusCheckResponse>(__text);
		}

		// Token: 0x06002C2D RID: 11309 RVA: 0x001ADFDB File Offset: 0x001AC1DB
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/PlatformStatusCheck";
		}
	}
}
