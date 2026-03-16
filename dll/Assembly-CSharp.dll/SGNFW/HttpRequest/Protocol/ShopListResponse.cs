using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class ShopListResponse : Response
	{
		public List<ShopItemInfo> infoList;
	}
}
