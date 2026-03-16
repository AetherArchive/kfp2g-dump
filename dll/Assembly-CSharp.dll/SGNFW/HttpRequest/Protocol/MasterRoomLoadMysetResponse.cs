using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class MasterRoomLoadMysetResponse : Response
	{
		public Assets assets;

		public string room_name;

		public List<MasterRoomFurniture> furniture_list;

		public List<MasterRoomMachineDataModel> master_room_machine_list;
	}
}
