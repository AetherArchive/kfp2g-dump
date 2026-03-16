using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class GachaExecRequest : Request
	{
		public int gacha_id;

		public int gacha_type;

		public int my_use_item_id;
	}
}
