using System;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class MasterRoomReceiveStamplog
	{
		public int stamp_id;

		public string user_name;

		public int user_rank;

		public long receive_time;

		public int follow_status;

		public long stamp_point;
	}
}
