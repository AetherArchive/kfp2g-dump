using System;

namespace SGNFW.Mst
{
	[Serializable]
	public class MstItemBonusData
	{
		public int id;

		public int increaseItemId;

		public int bonusPhotoId;

		public int bonusRatio;

		public long startDatetime;

		public long endDatetime;
	}
}
