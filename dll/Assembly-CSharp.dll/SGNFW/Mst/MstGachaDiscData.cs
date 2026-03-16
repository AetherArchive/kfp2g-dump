using System;

namespace SGNFW.Mst
{
	[Serializable]
	public class MstGachaDiscData
	{
		public int discountId;

		public string discountName;

		public int discountType;

		public int discountNum;

		public long startDatetime;

		public long endDatetime;

		public int availableCount;
	}
}
