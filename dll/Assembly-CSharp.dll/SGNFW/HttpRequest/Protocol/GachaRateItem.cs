using System;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003D7 RID: 983
	[Serializable]
	public class GachaRateItem
	{
		// Token: 0x040024BF RID: 9407
		public int item_id;

		// Token: 0x040024C0 RID: 9408
		public int item_num;

		// Token: 0x040024C1 RID: 9409
		public int remain_num;

		// Token: 0x040024C2 RID: 9410
		public double normal;

		// Token: 0x040024C3 RID: 9411
		public double decided;

		// Token: 0x040024C4 RID: 9412
		public double decided_3;

		// Token: 0x040024C5 RID: 9413
		public double decided_4;

		// Token: 0x040024C6 RID: 9414
		public double decided_ceiling;
	}
}
