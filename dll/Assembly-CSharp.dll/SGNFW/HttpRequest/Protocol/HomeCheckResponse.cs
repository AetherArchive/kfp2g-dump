using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x020003FA RID: 1018
	public class HomeCheckResponse : Response
	{
		// Token: 0x04002517 RID: 9495
		public Assets assets;

		// Token: 0x04002518 RID: 9496
		public int new_present_num;

		// Token: 0x04002519 RID: 9497
		public List<Bonus> bonuses;

		// Token: 0x0400251A RID: 9498
		public int friend_point;

		// Token: 0x0400251B RID: 9499
		public int friend_use_num;

		// Token: 0x0400251C RID: 9500
		public Sealed sealed_line;

		// Token: 0x0400251D RID: 9501
		public int master_room_stamp_flg;

		// Token: 0x0400251E RID: 9502
		public List<MasterRoomMachineDataModel> master_room_machine_list;

		// Token: 0x0400251F RID: 9503
		public RouletteData roulette_data;
	}
}
