using System;
using System.Collections.Generic;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class Helper
	{
		public int friend_id;

		public string user_name;

		public int level;

		public int add_point;

		public long last_login_time;

		public int is_receive_follow;

		public long is_receive_follow_datetime;

		public int is_send_follw;

		public long is_send_follw_datetime;

		public Chara favorite_chara;

		public string comment;

		public List<Chara> help_charas;

		public int achievement_id;
	}
}
