using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class KemostatusRankingResponse : Response
	{
		public List<KemostatusRankData> kemostatus_ranking;

		public long last_update_time;

		public int myrank;
	}
}
