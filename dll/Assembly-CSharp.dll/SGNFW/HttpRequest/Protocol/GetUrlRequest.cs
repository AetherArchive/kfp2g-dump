using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class GetUrlRequest : Request
	{
		public string version;

		public int dmm_viewer_id;
	}
}
