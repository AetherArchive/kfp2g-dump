using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PhotoGrowResponse : Response
	{
		public Assets assets;

		public PhotoGrowResult result;

		public List<RewardInfo> rewardinfoList;
	}
}
