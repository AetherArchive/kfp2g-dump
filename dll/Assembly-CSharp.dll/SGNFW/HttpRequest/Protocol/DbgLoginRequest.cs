using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class DbgLoginRequest : Request
	{
		public string secure_id;

		public string version;

		public int lang;
	}
}
