using System;

namespace SGNFW.Mst
{
	[Serializable]
	public class MstQuestQuestgroupData
	{
		public int questGroupId;

		public int mapId;

		public int questGroupCategory;

		public string titleName;

		public string storyName;

		public int titleCategory;

		public int dispPriority;

		public int relCharaId;

		public int dispCharaId;

		public string dispCharaComment;

		public int timeDispFlg;

		public int limitClearNum;

		public long startDatetime;

		public long endDatetime;

		public string dispCharaBodyMotion;

		public string dispCharaFaceMotion;

		public int autoModeEnable;

		public int growthEventId;

		public string dispChara2BodyMotion;

		public string dispChara2FaceMotion;

		public int dispCharaId2;

		public int hideFlag;

		public int limitGroupFlag;

		public int skippableFlag;

		public int limitSkipNum;

		public int limitSkipRecoveryNum;
	}
}
