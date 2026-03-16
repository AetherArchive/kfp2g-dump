using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class CharaWildRelRequest : Request
	{
		public int chara_id;

		public List<WildResult> promote_request;

		public int is_promoteup_action;
	}
}
