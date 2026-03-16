using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class CharaRankUpRequest : Request
	{
		public int chara_id;

		public int target_rank;
	}
}
