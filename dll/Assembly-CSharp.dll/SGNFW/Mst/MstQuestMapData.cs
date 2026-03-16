using System;

namespace SGNFW.Mst
{
	[Serializable]
	public class MstQuestMapData
	{
		public int mapId;

		public int chapterId;

		public string mapName;

		public int questMapCategory;

		public int questRelWeek;

		public int dispItemIconId00;

		public int dispItemIconId01;

		public int dispItemIconId02;

		public int dispItemIconId03;

		public int dispItemIconId04;

		public int dispItemIconId05;

		public int mapPosX;

		public int mapPosY;

		public string mapObjName;

		public long startDatetime;

		public int startHideFlag;

		public int weatherType;

		public string freeword;

		public int raidTargetCharaId;
	}
}
