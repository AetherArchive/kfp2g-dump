using System;

namespace SGNFW.Mst
{
	[Serializable]
	public class MstQuestPhotoDropItemData
	{
		public int category;

		public int targetId;

		public int photoId;

		public int photoLimitOverNum;

		public long startDatetime;

		public long endDatetime;

		public int bonusDrawId;

		public int bonusDrawNum;

		public int targetHelperFlg;
	}
}
