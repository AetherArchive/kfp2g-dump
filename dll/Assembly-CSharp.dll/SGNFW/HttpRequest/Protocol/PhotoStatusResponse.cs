using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PhotoStatusResponse : Response
	{
		public Assets assets;

		public int result;
	}
}
