using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class CoopStartRequest : Request
	{
		public int quest_id;

		public int deck_id;

		public int friend_id;

		public int helper_chara_id;

		public int kemostatus;

		public List<long> photo_id_List;

		public long get_info_time;

		public int event_id;

		public long coop_last_update_point;
	}
}
