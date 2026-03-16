using System;
using System.Collections.Generic;
using Battle;

public class TrainingRankingData
{
	public DateTime lastUpdateTime
	{
		get
		{
			return new DateTime(PrjUtil.ConvertTimeToTicks(this.lastUpdateServerTime));
		}
	}

	public TrainingRankingData(long lastUpdateTime)
	{
		this.lastUpdateServerTime = lastUpdateTime;
		this.dayOfWeekRankingList = new Dictionary<DayOfWeek, List<TrainingRankingData.RankingOne>>();
		foreach (object obj in Enum.GetValues(typeof(DayOfWeek)))
		{
			DayOfWeek dayOfWeek = (DayOfWeek)obj;
			this.dayOfWeekRankingList[dayOfWeek] = new List<TrainingRankingData.RankingOne>();
		}
	}

	public TrainingRankingData()
	{
	}

	public int seasonId;

	private long lastUpdateServerTime;

	public Dictionary<DayOfWeek, List<TrainingRankingData.RankingOne>> dayOfWeekRankingList;

	public class RankingOne
	{
		public static TrainingRankingData.RankingOne MakeDummy(int id)
		{
			return new TrainingRankingData.RankingOne
			{
				number = 1,
				userName = string.Format("ユーザー{0}", id),
				point = (long)(id * 1000),
				updateTime = TimeManager.Now,
				deckInfo = new SceneBattle_DeckInfo(2, null, 0),
				favoriteCharaId = 1,
				favoriteCharaFaceId = 1,
				achievementId = 0
			};
		}

		public int number;

		public string userName;

		public int userLevel;

		public long point;

		public DateTime updateTime;

		public SceneBattle_DeckInfo deckInfo;

		public int favoriteCharaId;

		public int favoriteCharaFaceId;

		public int achievementId;
	}
}
