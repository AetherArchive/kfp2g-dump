using System;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class MasterRoomStampPoint
	{
		public int stamp_id;

		public long total_point;

		public long weekly_point;

		public long daily_point;
	}
}
