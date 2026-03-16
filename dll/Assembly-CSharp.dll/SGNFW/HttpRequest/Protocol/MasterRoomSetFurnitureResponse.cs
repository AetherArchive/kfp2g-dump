using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class MasterRoomSetFurnitureResponse : Response
	{
		public Assets assets;

		public List<MasterRoomMachineDataModel> master_room_machine_list;
	}
}
