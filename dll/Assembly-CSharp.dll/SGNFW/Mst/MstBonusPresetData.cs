using System;

namespace SGNFW.Mst
{
	// Token: 0x0200028E RID: 654
	[Serializable]
	public class MstBonusPresetData
	{
		// Token: 0x060027B2 RID: 10162 RVA: 0x001A73C0 File Offset: 0x001A55C0
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

		// Token: 0x04001D83 RID: 7555
		public int bonusId;

		// Token: 0x04001D84 RID: 7556
		public string bannerName;

		// Token: 0x04001D85 RID: 7557
		public int bonusLoopFlg;

		// Token: 0x04001D86 RID: 7558
		public string bonusName;

		// Token: 0x04001D87 RID: 7559
		public int getFriendPointByFriend;

		// Token: 0x04001D88 RID: 7560
		public int charaId;

		// Token: 0x04001D89 RID: 7561
		public string charaMessage;

		// Token: 0x04001D8A RID: 7562
		public int displayType;

		// Token: 0x04001D8B RID: 7563
		public int lastPlayDay;

		// Token: 0x04001D8C RID: 7564
		public int totalFlg;

		// Token: 0x04001D8D RID: 7565
		public long startDatetime;

		// Token: 0x04001D8E RID: 7566
		public long endDatetime;

		// Token: 0x04001D8F RID: 7567
		public int bonusType;

		// Token: 0x020010C9 RID: 4297
		public enum DispCategory
		{
			// Token: 0x04005D01 RID: 23809
			INVALID,
			// Token: 0x04005D02 RID: 23810
			DYAS,
			// Token: 0x04005D03 RID: 23811
			TOTAL,
			// Token: 0x04005D04 RID: 23812
			SPECIAL_TOTAL,
			// Token: 0x04005D05 RID: 23813
			CAMPAIGN
		}
	}
}
