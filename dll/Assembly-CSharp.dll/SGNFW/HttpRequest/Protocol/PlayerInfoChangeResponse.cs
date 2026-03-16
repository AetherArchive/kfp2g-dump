using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PlayerInfoChangeResponse : Response
	{
		public Assets assets;

		public int result_name;

		public int result_comment;
	}
}
