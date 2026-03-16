using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class MissionListResponse : Response
	{
		public List<Mission> missions;

		public Assets assets;
	}
}
