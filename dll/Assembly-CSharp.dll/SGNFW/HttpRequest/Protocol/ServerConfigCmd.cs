using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000521 RID: 1313
	public class ServerConfigCmd : Command
	{
		// Token: 0x06002D48 RID: 11592 RVA: 0x001AFCEC File Offset: 0x001ADEEC
		private ServerConfigCmd()
		{
		}

		// Token: 0x06002D49 RID: 11593 RVA: 0x001AFCF4 File Offset: 0x001ADEF4
		private ServerConfigCmd(int dmm_viewer_id)
		{
			this.request = new ServerConfigRequest();
			((ServerConfigRequest)this.request).dmm_viewer_id = dmm_viewer_id;
			this.Setting();
		}

		// Token: 0x06002D4A RID: 11594 RVA: 0x001AFD20 File Offset: 0x001ADF20
		private void Setting()
		{
			base.Url = "common/ServerConfig.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.DefaultEncryptKey;
			base.TimeoutTime = 5f;
			base.IsDummy = false;
			base.IsPostMethod = false;
			Manager.Add(this);
		}

		// Token: 0x06002D4B RID: 11595 RVA: 0x001AFD8C File Offset: 0x001ADF8C
		public static ServerConfigCmd Create(int dmm_viewer_id)
		{
			return new ServerConfigCmd(dmm_viewer_id);
		}

		// Token: 0x06002D4C RID: 11596 RVA: 0x001AFD94 File Offset: 0x001ADF94
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<ServerConfigResponse>(__text);
		}

		// Token: 0x06002D4D RID: 11597 RVA: 0x001AFD9C File Offset: 0x001ADF9C
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/ServerConfig";
		}
	}
}
