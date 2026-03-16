using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class CharaWildRelResponse : Response
	{
		public Assets assets;

		public List<WildResult> wild_result;
	}
}
