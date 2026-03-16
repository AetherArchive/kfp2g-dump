using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class HelperMineExcecResponse : Response
	{
		public List<FollowStatus> result_status;

		public Assets assets;
	}
}
