using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class LoginDmmRequest : Request
	{
		public string version;

		public int lang;

		public string device;

		public string signature;

		public int dmm_viewer_id;

		public string onetime_token;
	}
}
