using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003E4 RID: 996
	public class GetGrowthEventCharaIdResponse : Response
	{
		// Token: 0x040024E9 RID: 9449
		public int chara_id;

		// Token: 0x040024EA RID: 9450
		public int event_id;

		// Token: 0x040024EB RID: 9451
		public long select_chara_datetime;

		// Token: 0x040024EC RID: 9452
		public long quest_clear_datetime;
	}
}
