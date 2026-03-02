using System;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200043B RID: 1083
	[Serializable]
	public class MasterRoomRanking
	{
		// Token: 0x040025EF RID: 9711
		public int friend_id;

		// Token: 0x040025F0 RID: 9712
		public string name;

		// Token: 0x040025F1 RID: 9713
		public int user_rank;

		// Token: 0x040025F2 RID: 9714
		public MasterRoomPublicInfo public_info;

		// Token: 0x040025F3 RID: 9715
		public long send_stamp_time;

		// Token: 0x040025F4 RID: 9716
		public int favorite_chara_id;

		// Token: 0x040025F5 RID: 9717
		public int favorite_chara_face_id;

		// Token: 0x040025F6 RID: 9718
		public int achievement_id;

		// Token: 0x040025F7 RID: 9719
		public long stamp_point;

		// Token: 0x040025F8 RID: 9720
		public int rank;

		// Token: 0x040025F9 RID: 9721
		public int visit_flg;

		// Token: 0x040025FA RID: 9722
		public int follow_status;

		// Token: 0x040025FB RID: 9723
		public long update_time;

		// Token: 0x040025FC RID: 9724
		public long last_visit_time;
	}
}
