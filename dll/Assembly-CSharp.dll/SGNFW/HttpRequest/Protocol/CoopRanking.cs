using System;
using System.Collections.Generic;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class CoopRanking
	{
		public long target_time;

		public int mine_point;

		public List<CoopPlayerInfo> ranked_user_list;
	}
}
