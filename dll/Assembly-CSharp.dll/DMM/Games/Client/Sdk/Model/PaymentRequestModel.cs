using System;

namespace DMM.Games.Client.Sdk.Model
{
	// Token: 0x02000579 RID: 1401
	[Serializable]
	public class PaymentRequestModel : RequestModel
	{
		// Token: 0x040028C3 RID: 10435
		public string itemId;

		// Token: 0x040028C4 RID: 10436
		public string itemName;

		// Token: 0x040028C5 RID: 10437
		public int unitPrice;

		// Token: 0x040028C6 RID: 10438
		public int quantity;

		// Token: 0x040028C7 RID: 10439
		public string callbackurl;

		// Token: 0x040028C8 RID: 10440
		public string finishurl;
	}
}
