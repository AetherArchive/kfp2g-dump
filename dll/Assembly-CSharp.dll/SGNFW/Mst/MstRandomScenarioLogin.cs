using System;

namespace SGNFW.Mst
{
	[Serializable]
	public class MstRandomScenarioLogin
	{
		public int id;

		public string scenarioFileName;

		public int scenarioId;

		public int randomWeight;

		public string memoryTitleName;

		public int memoryCharaId01;

		public int memoryCharaId02;

		public string memoryText01;

		public string memoryText02;

		public long endTime;
	}
}
