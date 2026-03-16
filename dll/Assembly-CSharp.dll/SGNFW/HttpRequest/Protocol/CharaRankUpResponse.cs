using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class CharaRankUpResponse : Response
	{
		public Assets assets;

		public RankUpResult rankup_result;
	}
}
