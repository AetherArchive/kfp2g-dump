using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000428 RID: 1064
	public class MasterRoomGetDataRequest : Request
	{
		// Token: 0x040025AE RID: 9646
		public int get_furniture_flg;

		// Token: 0x040025AF RID: 9647
		public int get_chara_flg;

		// Token: 0x040025B0 RID: 9648
		public int get_receive_stamp_log_flg;

		// Token: 0x040025B1 RID: 9649
		public int get_myset_flg;

		// Token: 0x040025B2 RID: 9650
		public int get_follow_flg;

		// Token: 0x040025B3 RID: 9651
		public int get_passing_flg;

		// Token: 0x040025B4 RID: 9652
		public int get_ranking_flg;

		// Token: 0x040025B5 RID: 9653
		public int get_stamp_log_flg;

		// Token: 0x040025B6 RID: 9654
		public int get_public_info_flg;
	}
}
