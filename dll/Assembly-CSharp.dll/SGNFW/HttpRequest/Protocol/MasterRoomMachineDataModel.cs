using System;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class MasterRoomMachineDataModel
	{
		public MasterRoomMachineDataModel()
		{
		}

		public MasterRoomMachineDataModel(MasterRoomMachineDataModel model)
		{
			this.machineId = model.machineId;
			this.indexId = model.indexId;
			this.nextsecond = model.nextsecond;
			this.lastGettime = model.lastGettime;
			this.furnitureId = model.furnitureId;
		}

		public int machineId;

		public int indexId;

		public int nextsecond;

		public long lastGettime;

		public int furnitureId;
	}
}
