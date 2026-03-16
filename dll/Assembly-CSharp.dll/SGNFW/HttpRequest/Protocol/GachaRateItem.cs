using System;

namespace SGNFW.HttpRequest.Protocol
{
	[Serializable]
	public class GachaRateItem
	{
		public int item_id;

		public int item_num;

		public int remain_num;

		public double normal;

		public double decided;

		public double decided_3;

		public double decided_4;

		public double decided_ceiling;
	}
}
