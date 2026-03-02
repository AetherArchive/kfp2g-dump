using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020004AA RID: 1194
	public class PlatformStatusCheckResponse : Response
	{
		// Token: 0x040026B7 RID: 9911
		public int phase;

		// Token: 0x040026B8 RID: 9912
		public int stone_charge_num;

		// Token: 0x040026B9 RID: 9913
		public string repayment_id;

		// Token: 0x040026BA RID: 9914
		public int freeze_flg;

		// Token: 0x040026BB RID: 9915
		public long last_check_datetime;
	}
}
