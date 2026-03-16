using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class MasterRoomSetFurnitureRequest : Request
	{
		public List<MasterRoomFurniture> furniture_list;
	}
}
