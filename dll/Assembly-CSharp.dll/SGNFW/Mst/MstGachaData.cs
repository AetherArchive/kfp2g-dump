using System;

namespace SGNFW.Mst
{
	[Serializable]
	public class MstGachaData
	{
		public int gachaId;

		public int gachaCategory;

		public string gachaName;

		public int gachaGroupId;

		public string labelTextureName;

		public int sortIndex;

		public string banner;

		public int highLimit;

		public int highLimitCountFlg;

		public int availableCount;

		public string detailDispText;

		public long startDatetime;

		public long endDatetime;

		public int recommendFlg;

		public int stepParentGachaId;

		public string resetStepTime;

		public string stepupBtnText;

		public int rateHiddenFlg;

		public int resultInfoType;

		public int replaceGroupId;

		public int mondayFlg;

		public int tuesdayFlg;

		public int wednesdayFlg;

		public int thursdayFlg;

		public int fridayFlg;

		public int saturdayFlg;

		public int sundayFlg;

		public int tabCategory;
	}
}
