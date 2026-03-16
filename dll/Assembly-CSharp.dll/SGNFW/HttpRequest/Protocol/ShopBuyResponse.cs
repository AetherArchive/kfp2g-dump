using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class ShopBuyResponse : Response
	{
		public Assets assets;

		public int send_presentbox;

		public List<GachaResult> gacha_result;
	}
}
