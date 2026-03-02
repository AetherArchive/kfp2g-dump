using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200043A RID: 1082
	public class MasterRoomPublicSettingResponse : Response
	{
		// Token: 0x040025EC RID: 9708
		public int result_room_name;

		// Token: 0x040025ED RID: 9709
		public int result_comment;

		// Token: 0x040025EE RID: 9710
		public MasterRoomPublicInfo public_info;
	}
}
