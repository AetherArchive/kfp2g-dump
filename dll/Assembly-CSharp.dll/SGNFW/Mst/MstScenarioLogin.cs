using System;

namespace SGNFW.Mst
{
	[Serializable]
	public class MstScenarioLogin
	{
		public int id;

		public string scenarioFileName;

		public int orderId;

		public long startTime;

		public long endTime;

		public string memoryGroupName;

		public string memoryTitleName;

		public int memoryCharaId01;

		public int memoryCharaId02;

		public string memoryText01;

		public string memoryText02;

		public int randomId;
	}
}
