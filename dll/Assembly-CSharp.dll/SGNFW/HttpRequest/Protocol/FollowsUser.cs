using System;
using System.Collections.Generic;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class FollowsUser
	{
		public int friend_id;

		public string user_name;

		public int level;

		public long last_login_time;

		public int is_receive_follow;

		public long is_receive_follow_datetime;

		public int is_send_follw;

		public long is_send_follw_datetime;

		public string comment;

		public List<FollowsChara> help_charas;

		public int achievement_id;
	}
}
