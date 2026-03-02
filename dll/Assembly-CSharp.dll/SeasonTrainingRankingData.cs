using System;
using System.Collections.Generic;
using SGNFW.HttpRequest.Protocol;

// Token: 0x020000AD RID: 173
public class SeasonTrainingRankingData
{
	// Token: 0x1700016C RID: 364
	// (get) Token: 0x06000796 RID: 1942 RVA: 0x0003411B File Offset: 0x0003231B
	public DateTime lastUpdateTime
	{
		get
		{
			return new DateTime(PrjUtil.ConvertTimeToTicks(this.lastUpdateServerTime));
		}
	}

	// Token: 0x06000797 RID: 1943 RVA: 0x0003412D File Offset: 0x0003232D
	public SeasonTrainingRankingData()
	{
	}

	// Token: 0x06000798 RID: 1944 RVA: 0x00034138 File Offset: 0x00032338
	public SeasonTrainingRankingData(TrainingPointRankingResponse serverData)
	{
		this.myRankingNo = serverData.myrank;
		this.isTallyFinish = serverData.confirm_flag == 1;
		this.lastUpdateServerTime = serverData.last_update_time;
		this.rankingList = serverData.training_point_ranking.ConvertAll<SeasonTrainingRankingData.RankingOne>((TrainingPointRankingData item) => new SeasonTrainingRankingData.RankingOne(item));
	}

	// Token: 0x04000699 RID: 1689
	public int seasonId;

	// Token: 0x0400069A RID: 1690
	private long lastUpdateServerTime;

	// Token: 0x0400069B RID: 1691
	public List<SeasonTrainingRankingData.RankingOne> rankingList;

	// Token: 0x0400069C RID: 1692
	public int myRankingNo;

	// Token: 0x0400069D RID: 1693
	public bool isTallyFinish;

	// Token: 0x0200079A RID: 1946
	public class RankingOne
	{
		// Token: 0x060036C3 RID: 14019 RVA: 0x001C6D1F File Offset: 0x001C4F1F
		public RankingOne()
		{
		}

		// Token: 0x060036C4 RID: 14020 RVA: 0x001C6D28 File Offset: 0x001C4F28
		public RankingOne(TrainingPointRankingData serverData)
		{
			this.number = serverData.rank;
			this.userName = serverData.user_name;
			this.userLevel = serverData.user_level;
			this.rankingPoint = serverData.rankingpoint;
			this.totalGoodScore = serverData.total_score;
			this.favoriteCharaId = serverData.favorite_chara_id;
			this.favoriteCharaFaceId = serverData.favorite_chara_face_id;
			this.achievementId = serverData.achievement_id;
		}

		// Token: 0x040033BE RID: 13246
		public int number;

		// Token: 0x040033BF RID: 13247
		public string userName;

		// Token: 0x040033C0 RID: 13248
		public int userLevel;

		// Token: 0x040033C1 RID: 13249
		public long rankingPoint;

		// Token: 0x040033C2 RID: 13250
		public long totalGoodScore;

		// Token: 0x040033C3 RID: 13251
		public int favoriteCharaId;

		// Token: 0x040033C4 RID: 13252
		public int favoriteCharaFaceId;

		// Token: 0x040033C5 RID: 13253
		public int achievementId;
	}
}
