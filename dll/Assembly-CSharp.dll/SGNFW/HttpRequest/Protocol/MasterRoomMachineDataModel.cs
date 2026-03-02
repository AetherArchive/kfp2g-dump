using System;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000430 RID: 1072
	[Serializable]
	public class MasterRoomMachineDataModel
	{
		// Token: 0x06002B15 RID: 11029 RVA: 0x001AC507 File Offset: 0x001AA707
		public MasterRoomMachineDataModel()
		{
		}

		// Token: 0x06002B16 RID: 11030 RVA: 0x001AC510 File Offset: 0x001AA710
		public MasterRoomMachineDataModel(MasterRoomMachineDataModel model)
		{
			this.machineId = model.machineId;
			this.indexId = model.indexId;
			this.nextsecond = model.nextsecond;
			this.lastGettime = model.lastGettime;
			this.furnitureId = model.furnitureId;
		}

		// Token: 0x040025CB RID: 9675
		public int machineId;

		// Token: 0x040025CC RID: 9676
		public int indexId;

		// Token: 0x040025CD RID: 9677
		public int nextsecond;

		// Token: 0x040025CE RID: 9678
		public long lastGettime;

		// Token: 0x040025CF RID: 9679
		public int furnitureId;
	}
}
