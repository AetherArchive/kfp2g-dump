using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PvPOppUpdateListRequest : Request
	{
		public int type_id;

		public int season_id;

		public int pvp_id;
	}
}
