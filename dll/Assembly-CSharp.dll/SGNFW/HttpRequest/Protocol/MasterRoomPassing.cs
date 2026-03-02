using System;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000436 RID: 1078
	[Serializable]
	public class MasterRoomPassing
	{
		// Token: 0x040025DC RID: 9692
		public int friend_id;

		// Token: 0x040025DD RID: 9693
		public string name;

		// Token: 0x040025DE RID: 9694
		public int user_rank;

		// Token: 0x040025DF RID: 9695
		public MasterRoomPublicInfo public_info;

		// Token: 0x040025E0 RID: 9696
		public long send_stamp_time;

		// Token: 0x040025E1 RID: 9697
		public int favorite_chara_id;

		// Token: 0x040025E2 RID: 9698
		public int favorite_chara_face_id;

		// Token: 0x040025E3 RID: 9699
		public int achievement_id;

		// Token: 0x040025E4 RID: 9700
		public long stamp_point;

		// Token: 0x040025E5 RID: 9701
		public long update_time;

		// Token: 0x040025E6 RID: 9702
		public int visit_flg;

		// Token: 0x040025E7 RID: 9703
		public int follow_status;
	}
}
