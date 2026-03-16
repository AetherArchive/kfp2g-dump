using System;

namespace SGNFW.Mst
{
	[Serializable]
	public class MstTrainingDayofweekData
	{
		public int seasonId;

		public int dayOfWeek;

		public int questOneId;

		public int turnLimit;

		public int missionConditions;

		public int missionType01;

		public int missionType02;

		public int missionValue01;

		public int missionValue02;

		public string captureInfoText;

		public string text01;

		public string text02;

		public string enemyTexturePath;

		public int enemyRevivalHpratio;

		public int enemyRevivalAtkratio;

		public int enemyRevivalDefratio;
	}
}
