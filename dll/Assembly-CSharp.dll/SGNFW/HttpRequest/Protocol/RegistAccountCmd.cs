using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200050E RID: 1294
	public class RegistAccountCmd : Command
	{
		// Token: 0x06002D1C RID: 11548 RVA: 0x001AF8D1 File Offset: 0x001ADAD1
		private RegistAccountCmd()
		{
		}

		// Token: 0x06002D1D RID: 11549 RVA: 0x001AF8D9 File Offset: 0x001ADAD9
		private RegistAccountCmd(string device, string signature, int dmm_viewer_id)
		{
			this.request = new RegistAccountRequest();
			RegistAccountRequest registAccountRequest = (RegistAccountRequest)this.request;
			registAccountRequest.device = device;
			registAccountRequest.signature = signature;
			registAccountRequest.dmm_viewer_id = dmm_viewer_id;
			this.Setting();
		}

		// Token: 0x06002D1E RID: 11550 RVA: 0x001AF914 File Offset: 0x001ADB14
		private void Setting()
		{
			base.Url = "RegistAccount.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.DefaultEncryptKey;
			base.TimeoutTime = 5f;
			base.IsDummy = false;
			base.IsPostMethod = false;
			Manager.Add(this);
		}

		// Token: 0x06002D1F RID: 11551 RVA: 0x001AF980 File Offset: 0x001ADB80
		public static RegistAccountCmd Create(string device, string signature, int dmm_viewer_id)
		{
			return new RegistAccountCmd(device, signature, dmm_viewer_id);
		}

		// Token: 0x06002D20 RID: 11552 RVA: 0x001AF98A File Offset: 0x001ADB8A
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<RegistAccountResponse>(__text);
		}

		// Token: 0x06002D21 RID: 11553 RVA: 0x001AF992 File Offset: 0x001ADB92
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/RegistAccount";
		}
	}
}
