using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class QuestStartRequest : Request
	{
		public int quest_id;

		public int deck_id;

		public int friend_id;

		public int helper_chara_id;

		public int kemostatus;

		public List<long> photo_id_List;
	}
}
