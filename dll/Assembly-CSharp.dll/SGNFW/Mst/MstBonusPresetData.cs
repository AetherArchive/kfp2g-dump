using System;

namespace SGNFW.Mst
{
	[Serializable]
	public class MstBonusPresetData
	{
		public MstBonusPresetData.DispCategory GetDispCategory()
		{
			if (this.bonusId == 1)
			{
				return MstBonusPresetData.DispCategory.DYAS;
			}
			if (this.bonusId == 2)
			{
				return MstBonusPresetData.DispCategory.TOTAL;
			}
			if (this.bonusId == 3)
			{
				return MstBonusPresetData.DispCategory.SPECIAL_TOTAL;
			}
			if (this.bonusId >= 4)
			{
				return MstBonusPresetData.DispCategory.CAMPAIGN;
			}
			return MstBonusPresetData.DispCategory.INVALID;
		}

		public int bonusId;

		public string bannerName;

		public int bonusLoopFlg;

		public string bonusName;

		public int getFriendPointByFriend;

		public int charaId;

		public string charaMessage;

		public int displayType;

		public int lastPlayDay;

		public int totalFlg;

		public long startDatetime;

		public long endDatetime;

		public int bonusType;

		public enum DispCategory
		{
			INVALID,
			DYAS,
			TOTAL,
			SPECIAL_TOTAL,
			CAMPAIGN
		}
	}
}
