using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class MasterRoomVisitResponse : Response
	{
		public List<MasterRoomFurniture> furniture_list;

		public List<MasterRoomChara> chara_list;

		public long send_stamp_time;

		public int follow_flg;

		public string room_name;

		public int error_type;

		public string user_name;

		public string room_comment;

		public int achievement_id;
	}
}
