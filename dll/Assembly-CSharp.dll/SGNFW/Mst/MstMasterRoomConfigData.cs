using System;

namespace SGNFW.Mst
{
	[Serializable]
	public class MstMasterRoomConfigData
	{
		public int id;

		public int shopId;

		public int gachaId;

		public int followDispNum;

		public int passingDispNum;

		public int passingBorderPoint;

		public int rankingDispNum;

		public int rankingBorderRank;

		public int stampLogDispNum;

		public int stampPointRate;

		public int stampPointRateFollow;

		public int stampPointRateFollower;

		public int stampPointRateFollowFollower;
	}
}
