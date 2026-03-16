using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class QuestSkipRequest : Request
	{
		public int quest_id;

		public int deck_id;

		public int friend_id;

		public int helper_chara_id;

		public int skip_num;

		public int kemostatus;

		public List<long> helper_photo_id_list;
	}
}
