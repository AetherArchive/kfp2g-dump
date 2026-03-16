using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class AccessoryGrowRequest : Request
	{
		public long accessory_id;

		public List<long> materials;
	}
}
