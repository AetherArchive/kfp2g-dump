using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x0200042F RID: 1071
	public class MasterRoomLoadMysetResponse : Response
	{
		// Token: 0x040025C7 RID: 9671
		public Assets assets;

		// Token: 0x040025C8 RID: 9672
		public string room_name;

		// Token: 0x040025C9 RID: 9673
		public List<MasterRoomFurniture> furniture_list;

		// Token: 0x040025CA RID: 9674
		public List<MasterRoomMachineDataModel> master_room_machine_list;
	}
}
