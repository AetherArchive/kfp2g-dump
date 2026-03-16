using System;
using System.Collections.Generic;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class ItemViewResult
	{
		public List<GachaRateItem> pickup;

		public List<GachaRateItem> other;
	}
}
