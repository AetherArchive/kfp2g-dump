using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class FollowsListResponse : Response
	{
		public List<FollowsUser> followsList;

		public Assets assets;
	}
}
