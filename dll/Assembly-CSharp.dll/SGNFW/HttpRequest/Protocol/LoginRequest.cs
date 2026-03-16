using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class LoginRequest : Request
	{
		public string uuid;

		public string secure_id;

		public string version;

		public int lang;

		public string device;

		public string device_id;

		public string signature;
	}
}
