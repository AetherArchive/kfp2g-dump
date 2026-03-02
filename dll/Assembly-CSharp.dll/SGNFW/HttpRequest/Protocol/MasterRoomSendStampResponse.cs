using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000442 RID: 1090
	public class MasterRoomSendStampResponse : Response
	{
		// Token: 0x04002609 RID: 9737
		public Assets assets;

		// Token: 0x0400260A RID: 9738
		public long send_stamp_time;
	}
}
