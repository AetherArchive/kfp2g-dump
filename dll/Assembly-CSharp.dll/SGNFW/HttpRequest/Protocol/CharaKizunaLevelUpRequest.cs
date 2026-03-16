using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class CharaKizunaLevelUpRequest : Request
	{
		public int chara_id;

		public List<UseItem> use_items;
	}
}
