using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class CharaLimitLevelUpRequest : Request
	{
		public int chara_id;

		public int target_level_limit_id;
	}
}
