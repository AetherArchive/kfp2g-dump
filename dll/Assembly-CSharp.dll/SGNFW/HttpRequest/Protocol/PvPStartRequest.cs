using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PvPStartRequest : Request
	{
		public int type_id;

		public int season_id;

		public int pvp_id;

		public int opp_friend_id;

		public int pvp_use_stamina;
	}
}
