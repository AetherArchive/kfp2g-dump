using System;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000441 RID: 1089
	public class MasterRoomSendStampRequest : Request
	{
		// Token: 0x04002606 RID: 9734
		public int friend_id;

		// Token: 0x04002607 RID: 9735
		public int stamp_id;

		// Token: 0x04002608 RID: 9736
		public int root;
	}
}
