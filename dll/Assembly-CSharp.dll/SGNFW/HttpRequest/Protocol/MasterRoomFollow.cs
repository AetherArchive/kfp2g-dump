using System;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000424 RID: 1060
	[Serializable]
	public class MasterRoomFollow
	{
		// Token: 0x04002599 RID: 9625
		public int friend_id;

		// Token: 0x0400259A RID: 9626
		public string name;

		// Token: 0x0400259B RID: 9627
		public int user_rank;

		// Token: 0x0400259C RID: 9628
		public MasterRoomPublicInfo public_info;

		// Token: 0x0400259D RID: 9629
		public long send_stamp_time;

		// Token: 0x0400259E RID: 9630
		public int favorite_chara_id;

		// Token: 0x0400259F RID: 9631
		public int favorite_chara_face_id;

		// Token: 0x040025A0 RID: 9632
		public int achievement_id;

		// Token: 0x040025A1 RID: 9633
		public long update_time;

		// Token: 0x040025A2 RID: 9634
		public int visit_flg;

		// Token: 0x040025A3 RID: 9635
		public int follow_status;

		// Token: 0x040025A4 RID: 9636
		public long last_visit_time;
	}
}
