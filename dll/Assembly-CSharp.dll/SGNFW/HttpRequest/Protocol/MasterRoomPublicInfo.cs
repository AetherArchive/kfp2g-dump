using System;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class MasterRoomPublicInfo
	{
		public string room_name;

		public string comment;

		public int public_type;
	}
}
