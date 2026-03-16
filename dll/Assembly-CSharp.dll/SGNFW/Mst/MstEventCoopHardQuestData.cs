using System;

namespace SGNFW.Mst
{
	[Serializable]
	public class MstEventCoopHardQuestData
	{
		public int eventId;

		public int questMapId;

		public long startDatetime;

		public long endDatetime;

		public int achieveCondition;
	}
}
