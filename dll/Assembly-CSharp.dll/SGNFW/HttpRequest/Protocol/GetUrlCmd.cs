using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003E5 RID: 997
	public class GetUrlCmd : Command
	{
		// Token: 0x06002A69 RID: 10857 RVA: 0x001AB42F File Offset: 0x001A962F
		private GetUrlCmd()
		{
		}

		// Token: 0x06002A6A RID: 10858 RVA: 0x001AB437 File Offset: 0x001A9637
		private GetUrlCmd(string version, int dmm_viewer_id)
		{
			this.request = new GetUrlRequest();
			GetUrlRequest getUrlRequest = (GetUrlRequest)this.request;
			getUrlRequest.version = version;
			getUrlRequest.dmm_viewer_id = dmm_viewer_id;
			this.Setting();
		}

		// Token: 0x06002A6B RID: 10859 RVA: 0x001AB468 File Offset: 0x001A9668
		private void Setting()
		{
			base.Url = "common/GetUrl.do";
			base.Server = Manager.ServerRoot["root"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.DefaultEncryptKey;
			base.TimeoutTime = 30f;
			base.IsDummy = false;
			base.IsPostMethod = false;
			Manager.Add(this);
		}

		// Token: 0x06002A6C RID: 10860 RVA: 0x001AB4D4 File Offset: 0x001A96D4
		public static GetUrlCmd Create(string version, int dmm_viewer_id)
		{
			return new GetUrlCmd(version, dmm_viewer_id);
		}

		// Token: 0x06002A6D RID: 10861 RVA: 0x001AB4DD File Offset: 0x001A96DD
		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<GetUrlResponse>(__text);
		}

		// Token: 0x06002A6E RID: 10862 RVA: 0x001AB4E5 File Offset: 0x001A96E5
		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/GetUrl";
		}
	}
}
