using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class CharaLevelUpResponse : Response
	{
		public Assets assets;

		public LevelResult level_result;
	}
}
