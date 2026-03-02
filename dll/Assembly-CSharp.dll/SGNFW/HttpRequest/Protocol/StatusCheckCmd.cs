using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000532 RID: 1330
	public class StatusCheckCmd : Command
	{
		// Token: 0x06002D71 RID: 11633 RVA: 0x001B00DA File Offset: 0x001AE2DA
		private StatusCheckCmd()
		{
		}

		// Token: 0x06002D72 RID: 11634 RVA: 0x001B00E2 File Offset: 0x001AE2E2
		private StatusCheckCmd(string uuid, string version, int dmm_viewer_id)
		{
			this.request = new StatusCheckRequest();
			StatusCheckRequest statusCheckRequest = (StatusCheckRequest)this.request;
			statusCheckRequest.uuid = uuid;
			statusCheckRequest.version = version;
			statusCheckRequest.dmm_viewer_id = dmm_viewer_id;
			this.Setting();
		}

		// Token: 0x06002D73 RID: 11635 RVA: 0x001B011C File Offset: 0x001AE31C
		private void Setting()
		{
			base.Url = "common/StatusCheck.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.DefaultEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = true;
			Manager.Add(this);
		}

		// Token: 0x06002D74 RID: 11636 RVA: 0x001B0188 File Offset: 0x001AE388
		public static StatusCheckCmd Create(string uuid, string version, int dmm_viewer_id)
		{
			return new StatusCheckCmd(uuid, version, dmm_viewer_id);
		}

		// Token: 0x06002D75 RID: 11637 RVA: 0x001B0192 File Offset: 0x001AE392
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<StatusCheckResponse>(__text);
		}

		// Token: 0x06002D76 RID: 11638 RVA: 0x001B019A File Offset: 0x001AE39A
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/StatusCheck";
		}
	}
}
