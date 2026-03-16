using System;

namespace SGNFW.Mst
{
	[Serializable]
	public class MstGachaTypeData
	{
		public int gachaId;

		public int gachaType;

		public int discountId;

		public int lotTime;

		public string balloonText;

		public int useItemId;

		public int useItemNum;

		public int subItemUseCondition;

		public int subItemId;

		public int subItemNum;

		public int bonusItemId;

		public int bonusItemNum;

		public string bonusItemDispMessage;

		public int bonusItemLimit;

		public int lastTimeBenefitFriends;

		public int lastTimeBenefitRarity;

		public int lastTimeBenefitRarity4;

		public int bonusItemPresetId;
	}
}
