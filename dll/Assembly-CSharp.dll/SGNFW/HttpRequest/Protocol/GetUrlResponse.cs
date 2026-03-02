using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003E7 RID: 999
	public class GetUrlResponse : Response
	{
		// Token: 0x040024EF RID: 9455
		public string asset_bundle_url;

		// Token: 0x040024F0 RID: 9456
		public string base_data_url;

		// Token: 0x040024F1 RID: 9457
		public string notice_url;

		// Token: 0x040024F2 RID: 9458
		public string asset_bundle_version;

		// Token: 0x040024F3 RID: 9459
		public int is_need_version_up;

		// Token: 0x040024F4 RID: 9460
		public string server_id;

		// Token: 0x040024F5 RID: 9461
		public string webview_url;

		// Token: 0x040024F6 RID: 9462
		public Maintenance maintenance;
	}
}
