using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class MasterRoomAddFuelRequest : Request
	{
		public List<UseItem> use_items;
	}
}
