using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class MissionListRequest : Request
	{
		public List<int> mission_types;
	}
}
