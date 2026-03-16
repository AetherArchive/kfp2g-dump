using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class MasterSkillGrowRequest : Request
	{
		public int master_skill_id;

		public List<UseItem> use_items;
	}
}
