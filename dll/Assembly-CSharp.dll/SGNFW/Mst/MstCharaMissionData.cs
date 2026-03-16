using System;

namespace SGNFW.Mst
{
	[Serializable]
	public class MstCharaMissionData
	{
		public int missionId;

		public int charaId;

		public int relType;

		public int denom;

		public string missionTitle;

		public string missionContents;

		public int groupStartFlg;

		public int alwaysDispFlg;

		public int needMissionId;

		public int sortNum;

		public int rewardItemId;

		public int rewardItemNum;

		public long startTime;

		public long endTime;
	}
}
