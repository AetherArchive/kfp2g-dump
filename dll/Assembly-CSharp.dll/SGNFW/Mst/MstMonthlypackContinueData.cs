using System;

namespace SGNFW.Mst
{
	[Serializable]
	public class MstMonthlypackContinueData
	{
		public int continueId;

		public int prevMonthlyPackId;

		public int nextMonthlyPackId;

		public int addItemId;

		public int addItemNum;
	}
}
