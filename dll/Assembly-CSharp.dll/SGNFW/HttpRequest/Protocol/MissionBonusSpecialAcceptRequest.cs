using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class MissionBonusSpecialAcceptRequest : Request
	{
		public List<int> mission_id_list;

		public List<AcceptMission> accept_mission_list;
	}
}
