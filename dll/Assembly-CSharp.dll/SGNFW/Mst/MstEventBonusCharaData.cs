using System;

namespace SGNFW.Mst
{
	[Serializable]
	public class MstEventBonusCharaData
	{
		public int eventId;

		public int charaId;

		public long startTime;

		public long endTime;

		public int hpBonusRatio;

		public int strBonusRatio;

		public int defBonusRatio;

		public int kizunaBonusRatio;

		public int pickupFlag;

		public int increaseItemId01;

		public int dropBonusRatio01;

		public int increaseItemId02;

		public int dropBonusRatio02;
	}
}
