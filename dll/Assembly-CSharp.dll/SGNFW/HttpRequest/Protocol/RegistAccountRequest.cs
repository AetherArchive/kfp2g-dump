using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class RegistAccountRequest : Request
	{
		public string device;

		public string signature;

		public int dmm_viewer_id;
	}
}
