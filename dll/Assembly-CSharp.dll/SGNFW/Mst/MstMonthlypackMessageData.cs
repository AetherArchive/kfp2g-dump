using System;

namespace SGNFW.Mst
{
	[Serializable]
	public class MstMonthlypackMessageData
	{
		public int id;

		public string beforePeriodText;

		public int beforeCanbuyDay;

		public string afterPeriodText;

		public int continueReckonDay;
	}
}
