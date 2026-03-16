using System;

namespace SGNFW.Mst
{
	[Serializable]
	public class MstMissionData
	{
		public int missionId;

		public string missionTitle;

		public string missionContents;

		public int missionType;

		public int transitionScene;

		public int transitionId;

		public int relType;

		public int groupStartFlg;

		public int alwaysDispFlg;

		public int needMissionId;

		public int missionNumType00;

		public int missionNum00;

		public int missionNumType01;

		public int missionNum01;

		public int missionNumType02;

		public int missionNum02;

		public int missionNumType03;

		public int missionNum03;

		public int missionStrType00;

		public string missionStr00;

		public long missionDatetime00;

		public long missionDatetime01;

		public int denom;

		public int sortNum;

		public int rewardItemId;

		public int rewardItemNum;

		public long startTime;

		public long endTime;
	}
}
