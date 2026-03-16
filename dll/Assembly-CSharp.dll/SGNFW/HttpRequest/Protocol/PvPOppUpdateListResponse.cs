using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PvPOppUpdateListResponse : Response
	{
		public Assets assets;

		public List<OppUser> opp_user_list;

		public int limit_chal_count;

		public long last_chal_datetime;
	}
}
