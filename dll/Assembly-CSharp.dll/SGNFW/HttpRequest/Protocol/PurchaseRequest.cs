using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004C3 RID: 1219
	public class PurchaseRequest : Request
	{
		// Token: 0x040026E8 RID: 9960
		public string productId;

		// Token: 0x040026E9 RID: 9961
		public string transactionId;

		// Token: 0x040026EA RID: 9962
		public string receipt;

		// Token: 0x040026EB RID: 9963
		public string signature;

		// Token: 0x040026EC RID: 9964
		public int dmm_viewer_id;

		// Token: 0x040026ED RID: 9965
		public string onetime_token;

		// Token: 0x040026EE RID: 9966
		public List<string> notFinishTransactionList;
	}
}
