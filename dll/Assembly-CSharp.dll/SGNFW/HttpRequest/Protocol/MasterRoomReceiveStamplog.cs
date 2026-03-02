using System;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200043C RID: 1084
	[Serializable]
	public class MasterRoomReceiveStamplog
	{
		// Token: 0x040025FD RID: 9725
		public int stamp_id;

		// Token: 0x040025FE RID: 9726
		public string user_name;

		// Token: 0x040025FF RID: 9727
		public int user_rank;

		// Token: 0x04002600 RID: 9728
		public long receive_time;

		// Token: 0x04002601 RID: 9729
		public int follow_status;

		// Token: 0x04002602 RID: 9730
		public long stamp_point;
	}
}
