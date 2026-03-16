using System;
using System.Collections.Generic;

public class KemoStatusRankingData
{
	public DateTime lastUpdateTime
	{
		get
		{
			return new DateTime(PrjUtil.ConvertTimeToTicks(this.lastUpdateServerTime));
		}
	}

	public KemoStatusRankingData(long lastUpdateTime)
	{
		this.lastUpdateServerTime = lastUpdateTime;
		this.rankingList = new List<KemoStatusRankingData.RankingOne>();
	}

	public KemoStatusRankingData()
	{
	}

	private long lastUpdateServerTime;

	public List<KemoStatusRankingData.RankingOne> rankingList;

	public int myRank;

	public class RankingOne
	{
		public int number;

		public string userName;

		public int userLevel;

		public long kemoStatus;

		public int favoriteCharaId;

		public int favoriteCharaFaceId;

		public int achievementId;
	}
}
