using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class HelperSearchResponse : Response
	{
		public int result_status;

		public Helper helper;
	}
}
