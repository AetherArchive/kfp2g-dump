using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200045D RID: 1117
	public class MstDataCmd : Command
	{
		// Token: 0x06002B7A RID: 11130 RVA: 0x001ACE7B File Offset: 0x001AB07B
		private MstDataCmd()
		{
		}

		// Token: 0x06002B7B RID: 11131 RVA: 0x001ACE83 File Offset: 0x001AB083
		private MstDataCmd(string type, int dmm_viewer_id)
		{
			this.request = new MstDataRequest();
			MstDataRequest mstDataRequest = (MstDataRequest)this.request;
			mstDataRequest.type = type;
			mstDataRequest.dmm_viewer_id = dmm_viewer_id;
			this.Setting();
		}

		// Token: 0x06002B7C RID: 11132 RVA: 0x001ACEB4 File Offset: 0x001AB0B4
		private void Setting()
		{
			base.Url = "common/MstData.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.DefaultEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = false;
			Manager.Add(this);
		}

		// Token: 0x06002B7D RID: 11133 RVA: 0x001ACF20 File Offset: 0x001AB120
		public static MstDataCmd Create(string type, int dmm_viewer_id)
		{
			return new MstDataCmd(type, dmm_viewer_id);
		}

		// Token: 0x06002B7E RID: 11134 RVA: 0x001ACF29 File Offset: 0x001AB129
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<MstDataResponse>(__text);
		}

		// Token: 0x06002B7F RID: 11135 RVA: 0x001ACF31 File Offset: 0x001AB131
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/MstData";
		}
	}
}
