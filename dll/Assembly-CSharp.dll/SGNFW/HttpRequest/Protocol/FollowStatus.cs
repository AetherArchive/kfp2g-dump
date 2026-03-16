using System;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class FollowStatus
	{
		public int friend_id;

		public int status;

		public long is_send_follw_datetime;
	}
}
