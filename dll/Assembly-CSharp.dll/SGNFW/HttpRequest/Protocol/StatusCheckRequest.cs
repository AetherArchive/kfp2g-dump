using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class StatusCheckRequest : Request
	{
		public string uuid;

		public string version;

		public int dmm_viewer_id;
	}
}
