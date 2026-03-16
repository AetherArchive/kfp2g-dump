using System;

namespace SGNFW.Mst
{
	[Serializable]
	public class MstEventCoopConditionData
	{
		public int eventId;

		public int questMapId;

		public int level;

		public string levelName;

		public long achieveCondition;

		public int type;

		public int itemId;

		public int itemNum;

		public string texturePath;
	}
}
