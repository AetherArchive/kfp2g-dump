using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class GetUrlResponse : Response
	{
		public string asset_bundle_url;

		public string base_data_url;

		public string notice_url;

		public string asset_bundle_version;

		public int is_need_version_up;

		public string server_id;

		public string webview_url;

		public Maintenance maintenance;
	}
}
