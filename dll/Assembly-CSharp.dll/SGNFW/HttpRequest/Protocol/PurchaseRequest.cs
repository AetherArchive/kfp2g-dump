using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class PurchaseRequest : Request
	{
		public string productId;

		public string transactionId;

		public string receipt;

		public string signature;

		public int dmm_viewer_id;

		public string onetime_token;

		public List<string> notFinishTransactionList;
	}
}
