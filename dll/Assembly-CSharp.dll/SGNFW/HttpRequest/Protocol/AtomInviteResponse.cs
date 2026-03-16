using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class AtomInviteResponse : Response
	{
		public string invite_url;

		public int response_code;

		public string error_msg;
	}
}
