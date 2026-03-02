using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000429 RID: 1065
	public class MasterRoomGetDataResponse : Response
	{
		// Token: 0x040025B7 RID: 9655
		public Assets assets;

		// Token: 0x040025B8 RID: 9656
		public List<MasterRoomFurniture> furniture_list;

		// Token: 0x040025B9 RID: 9657
		public List<MasterRoomChara> chara_list;

		// Token: 0x040025BA RID: 9658
		public List<MasterRoomReceiveStamplog> receive_stamp_log_list;

		// Token: 0x040025BB RID: 9659
		public long receive_stamp_num;

		// Token: 0x040025BC RID: 9660
		public List<MasterRoomMyset> myset_list;

		// Token: 0x040025BD RID: 9661
		public List<MasterRoomFollow> follow_list;

		// Token: 0x040025BE RID: 9662
		public List<MasterRoomPassing> passing_list;

		// Token: 0x040025BF RID: 9663
		public List<MasterRoomRanking> ranking_list;

		// Token: 0x040025C0 RID: 9664
		public List<MasterRoomStampLog> stamp_log_list;

		// Token: 0x040025C1 RID: 9665
		public MasterRoomPublicInfo public_info;

		// Token: 0x040025C2 RID: 9666
		public List<MasterRoomStampPoint> receive_stamp_list;

		// Token: 0x040025C3 RID: 9667
		public List<MasterRoomMachineDataModel> master_room_machine_list;
	}
}
