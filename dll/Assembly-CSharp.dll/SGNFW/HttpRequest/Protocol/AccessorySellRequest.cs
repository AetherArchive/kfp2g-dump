using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class AccessorySellRequest : Request
	{
		public List<long> accessory_idList;
	}
}
