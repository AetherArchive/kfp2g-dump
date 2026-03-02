using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000461 RID: 1121
	public class MstVersionCmd : Command
	{
		// Token: 0x06002B83 RID: 11139 RVA: 0x001ACF50 File Offset: 0x001AB150
		private MstVersionCmd()
		{
		}

		// Token: 0x06002B84 RID: 11140 RVA: 0x001ACF58 File Offset: 0x001AB158
		private MstVersionCmd(int dmm_viewer_id)
		{
			this.request = new MstVersionRequest();
			((MstVersionRequest)this.request).dmm_viewer_id = dmm_viewer_id;
			this.Setting();
		}

		// Token: 0x06002B85 RID: 11141 RVA: 0x001ACF84 File Offset: 0x001AB184
		private void Setting()
		{
			base.Url = "common/MstVersion.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.DefaultEncryptKey;
			base.TimeoutTime = 5f;
			base.IsDummy = false;
			base.IsPostMethod = false;
			Manager.Add(this);
		}

		// Token: 0x06002B86 RID: 11142 RVA: 0x001ACFF0 File Offset: 0x001AB1F0
		public static MstVersionCmd Create(int dmm_viewer_id)
		{
			return new MstVersionCmd(dmm_viewer_id);
		}

		// Token: 0x06002B87 RID: 11143 RVA: 0x001ACFF8 File Offset: 0x001AB1F8
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<MstVersionResponse>(__text);
		}

		// Token: 0x06002B88 RID: 11144 RVA: 0x001AD000 File Offset: 0x001AB200
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/MstVersion";
		}
	}
}
