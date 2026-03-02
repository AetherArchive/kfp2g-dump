using System;
using System.Collections.Generic;
using Battle;

// Token: 0x020000AE RID: 174
public class TrainingRankingData
{
	// Token: 0x1700016D RID: 365
	// (get) Token: 0x06000799 RID: 1945 RVA: 0x000341A2 File Offset: 0x000323A2
	public DateTime lastUpdateTime
	{
		get
		{
			return new DateTime(PrjUtil.ConvertTimeToTicks(this.lastUpdateServerTime));
		}
	}

	// Token: 0x0600079A RID: 1946 RVA: 0x000341B4 File Offset: 0x000323B4
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

	// Token: 0x0600079B RID: 1947 RVA: 0x00034238 File Offset: 0x00032438
	public TrainingRankingData()
	{
	}

	// Token: 0x0400069E RID: 1694
	public int seasonId;

	// Token: 0x0400069F RID: 1695
	private long lastUpdateServerTime;

	// Token: 0x040006A0 RID: 1696
	public Dictionary<DayOfWeek, List<TrainingRankingData.RankingOne>> dayOfWeekRankingList;

	// Token: 0x0200079C RID: 1948
	public class RankingOne
	{
		// Token: 0x060036C8 RID: 14024 RVA: 0x001C6DB8 File Offset: 0x001C4FB8
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

		// Token: 0x040033C8 RID: 13256
		public int number;

		// Token: 0x040033C9 RID: 13257
		public string userName;

		// Token: 0x040033CA RID: 13258
		public int userLevel;

		// Token: 0x040033CB RID: 13259
		public long point;

		// Token: 0x040033CC RID: 13260
		public DateTime updateTime;

		// Token: 0x040033CD RID: 13261
		public SceneBattle_DeckInfo deckInfo;

		// Token: 0x040033CE RID: 13262
		public int favoriteCharaId;

		// Token: 0x040033CF RID: 13263
		public int favoriteCharaFaceId;

		// Token: 0x040033D0 RID: 13264
		public int achievementId;
	}
}
