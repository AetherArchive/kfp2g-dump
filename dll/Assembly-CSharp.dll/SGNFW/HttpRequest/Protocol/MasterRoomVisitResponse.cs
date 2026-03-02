using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200044D RID: 1101
	public class MasterRoomVisitResponse : Response
	{
		// Token: 0x04002622 RID: 9762
		public List<MasterRoomFurniture> furniture_list;

		// Token: 0x04002623 RID: 9763
		public List<MasterRoomChara> chara_list;

		// Token: 0x04002624 RID: 9764
		public long send_stamp_time;

		// Token: 0x04002625 RID: 9765
		public int follow_flg;

		// Token: 0x04002626 RID: 9766
		public string room_name;

		// Token: 0x04002627 RID: 9767
		public int error_type;

		// Token: 0x04002628 RID: 9768
		public string user_name;

		// Token: 0x04002629 RID: 9769
		public string room_comment;

		// Token: 0x0400262A RID: 9770
		public int achievement_id;
	}
}
