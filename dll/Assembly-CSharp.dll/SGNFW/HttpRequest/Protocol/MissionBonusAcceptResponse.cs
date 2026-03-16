using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class MissionBonusAcceptResponse : Response
	{
		public Assets assets;

		public int accept_result;
	}
}
