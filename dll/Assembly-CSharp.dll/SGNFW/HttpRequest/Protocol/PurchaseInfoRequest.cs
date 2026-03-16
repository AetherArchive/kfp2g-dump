using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PurchaseInfoRequest : Request
	{
		public List<string> notFinishTransactionList;

		public bool isStartGetProduct;
	}
}
