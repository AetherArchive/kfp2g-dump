using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class HelperMineExcecRequest : Request
	{
		public int action_type;

		public List<int> target_friend_id_list;
	}
}
