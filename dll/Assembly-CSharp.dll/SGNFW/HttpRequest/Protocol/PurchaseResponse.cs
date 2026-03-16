using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PurchaseResponse : Response
	{
		public int resultStatus;

		public int buyProductId;

		public Assets assets;

		public List<int> new_release_idList;
	}
}
