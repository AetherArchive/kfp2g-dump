using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000448 RID: 1096
	public class MasterRoomSetFurnitureResponse : Response
	{
		// Token: 0x0400260D RID: 9741
		public Assets assets;

		// Token: 0x0400260E RID: 9742
		public List<MasterRoomMachineDataModel> master_room_machine_list;
	}
}
