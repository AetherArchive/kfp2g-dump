using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class CharaKizunaLevelUpResponse : Response
	{
		public Assets assets;

		public KizunaLevelResult level_result;
	}
}
