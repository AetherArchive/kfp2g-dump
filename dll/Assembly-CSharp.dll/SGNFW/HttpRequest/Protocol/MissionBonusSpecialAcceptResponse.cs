using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class MissionBonusSpecialAcceptResponse : Response
	{
		public Assets assets;

		public List<GachaResult> gacha_result;

		public int accept_result;
	}
}
