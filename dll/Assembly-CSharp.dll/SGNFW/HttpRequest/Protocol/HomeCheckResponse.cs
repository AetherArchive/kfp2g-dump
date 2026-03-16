using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class HomeCheckResponse : Response
	{
		public Assets assets;

		public int new_present_num;

		public List<Bonus> bonuses;

		public int friend_point;

		public int friend_use_num;

		public Sealed sealed_line;

		public int master_room_stamp_flg;

		public List<MasterRoomMachineDataModel> master_room_machine_list;

		public RouletteData roulette_data;
	}
}
