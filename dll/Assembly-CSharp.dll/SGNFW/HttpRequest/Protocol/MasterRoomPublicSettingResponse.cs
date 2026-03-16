using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class MasterRoomPublicSettingResponse : Response
	{
		public int result_room_name;

		public int result_comment;

		public MasterRoomPublicInfo public_info;
	}
}
