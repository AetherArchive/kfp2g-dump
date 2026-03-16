using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class AccessoryStatusRequest : Request
	{
		public List<long> lock_accessory_id_list;

		public List<long> lock_clear_accessory_id_list;
	}
}
