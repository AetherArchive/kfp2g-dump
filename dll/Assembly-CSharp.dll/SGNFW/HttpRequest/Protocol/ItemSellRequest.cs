using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class ItemSellRequest : Request
	{
		public List<Item> sell_item_list;
	}
}
