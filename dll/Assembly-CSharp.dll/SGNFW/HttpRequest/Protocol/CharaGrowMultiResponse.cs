using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class CharaGrowMultiResponse : Response
	{
		public Assets assets;

		public LevelResult level_result;

		public List<WildResult> wild_result;

		public RankUpResult rankup_result;
	}
}
