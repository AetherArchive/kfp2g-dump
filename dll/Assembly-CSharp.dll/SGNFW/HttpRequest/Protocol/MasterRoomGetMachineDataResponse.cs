using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200042C RID: 1068
	public class MasterRoomGetMachineDataResponse : Response
	{
		// Token: 0x040025C4 RID: 9668
		public Assets assets;

		// Token: 0x040025C5 RID: 9669
		public List<MasterRoomMachineDataModel> master_room_machine_list;
	}
}
