using System;
using System.Collections.Generic;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	// Token: 0x02000447 RID: 1095
	public class MasterRoomSetFurnitureRequest : Request
	{
		// Token: 0x0400260C RID: 9740
		public List<MasterRoomFurniture> furniture_list;
	}
}
