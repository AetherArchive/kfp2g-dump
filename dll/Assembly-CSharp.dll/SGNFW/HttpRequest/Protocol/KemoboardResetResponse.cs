using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class KemoboardResetResponse : Response
	{
		public Assets assets;

		public List<int> kemoboard_panels;
	}
}
