using System;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000449 RID: 1097
	[Serializable]
	public class MasterRoomStampLog
	{
		// Token: 0x0400260F RID: 9743
		public int friend_id;

		// Token: 0x04002610 RID: 9744
		public int stamp_id;

		// Token: 0x04002611 RID: 9745
		public string name;

		// Token: 0x04002612 RID: 9746
		public int user_rank;

		// Token: 0x04002613 RID: 9747
		public MasterRoomPublicInfo public_info;

		// Token: 0x04002614 RID: 9748
		public long send_stamp_time;

		// Token: 0x04002615 RID: 9749
		public int favorite_chara_id;

		// Token: 0x04002616 RID: 9750
		public int favorite_chara_face_id;

		// Token: 0x04002617 RID: 9751
		public int achievement_id;

		// Token: 0x04002618 RID: 9752
		public long time;

		// Token: 0x04002619 RID: 9753
		public int send_flg;

		// Token: 0x0400261A RID: 9754
		public int visit_flg;

		// Token: 0x0400261B RID: 9755
		public int follow_status;
	}
}
