using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class ShopBulkBuyRequest : Request
	{
		public List<ShopData.ItemOne> goodsDataList;
	}
}
