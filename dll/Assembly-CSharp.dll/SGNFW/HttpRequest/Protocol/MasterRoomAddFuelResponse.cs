using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000422 RID: 1058
	public class MasterRoomAddFuelResponse : Response
	{
		// Token: 0x04002593 RID: 9619
		public Assets assets;

		// Token: 0x04002594 RID: 9620
		public List<MasterRoomMachineDataModel> master_room_machine_list;
	}
}
