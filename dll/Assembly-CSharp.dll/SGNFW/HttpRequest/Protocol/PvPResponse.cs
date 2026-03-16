using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PvPResponse : Response
	{
		public Assets assets;

		public List<PvPInfo> pvp_info_list;
	}
}
