using System;

namespace SGNFW.Mst
{
	[Serializable]
	public class MstAppConfig
	{
		public int id;

		public int photoStockLimit;

		public int accessoryStocKLimit;

		public long startTime;

		public long endTime;

		public int followLimit;

		public int followerLimit;

		public int presentLimit;

		public int historyLimit;

		public int staminaRecoveryTime;

		public int staminaLimit;

		public int pvpRecoveryTime;

		public int pvpLimit;

		public int pvpspecialRecoveryTime;

		public int pvpspecialLimit;

		public int staminaStone;

		public int pvpStone;

		public int continueStone;

		public int skipStone;

		public string stoneUnitPrice;

		public int enableNoahweb;
	}
}
