using System;

namespace SGNFW.Mst
{
	[Serializable]
	public class MstPvpData
	{
		public int pvpId;

		public int pvpCategory;

		public string pvpName;

		public int dispPriority;

		public int dispCharaId;

		public string bannerTextureName;

		public int winAcquirePoint;

		public int rewardItemId;

		public int winAcquireCoin;

		public int loseAcquireCoin;

		public int winAcquireExp;

		public int winAcquireKizuna;

		public string stagePresetId;

		public string bgFile;

		public long startTime;

		public long endTime;

		public int specialMaxTurn;

		public int cheerBonusId;

		public int cheerBonusNum;

		public int consecutiveWinsBonusId;

		public int consecutiveWinsBonusNum;

		public int championLotteyRatio;

		public int rankConditions;

		public int kemostatusConditions;

		public int pickNum;

		public int addCoinNum;

		public int lotteryRankLow;
	}
}
