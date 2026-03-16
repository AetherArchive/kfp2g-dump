using System;
using System.Collections.Generic;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class MasterRoomFurnitureViewResult
	{
		public List<GachaRateItem> pickup;

		public List<GachaRateItem> other;
	}
}
