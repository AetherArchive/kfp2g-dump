using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000433 RID: 1075
	public class MasterRoomMachineReceiveResponse : Response
	{
		// Token: 0x040025D2 RID: 9682
		public Assets assets;

		// Token: 0x040025D3 RID: 9683
		public List<MasterRoomMachineDataModel> master_room_machine_list;

		// Token: 0x040025D4 RID: 9684
		public List<MasterRoomMachineReceiveModel> machine_receive_list;
	}
}
