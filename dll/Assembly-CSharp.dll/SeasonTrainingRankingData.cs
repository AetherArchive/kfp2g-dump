using System;
using System.Collections.Generic;
using SGNFW.HttpRequest.Protocol;

public class SeasonTrainingRankingData
{
	public DateTime lastUpdateTime
	{
		get
		{
			return new DateTime(PrjUtil.ConvertTimeToTicks(this.lastUpdateServerTime));
		}
	}

	public SeasonTrainingRankingData()
	{
	}

	public SeasonTrainingRankingData(TrainingPointRankingResponse serverData)
	{
		this.myRankingNo = serverData.myrank;
		this.isTallyFinish = serverData.confirm_flag == 1;
		this.lastUpdateServerTime = serverData.last_update_time;
		this.rankingList = serverData.training_point_ranking.ConvertAll<SeasonTrainingRankingData.RankingOne>((TrainingPointRankingData item) => new SeasonTrainingRankingData.RankingOne(item));
	}

	public int seasonId;

	private long lastUpdateServerTime;

	public List<SeasonTrainingRankingData.RankingOne> rankingList;

	public int myRankingNo;

	public bool isTallyFinish;

	public class RankingOne
	{
		public RankingOne()
		{
		}

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

		public int number;

		public string userName;

		public int userLevel;

		public long rankingPoint;

		public long totalGoodScore;

		public int favoriteCharaId;

		public int favoriteCharaFaceId;

		public int achievementId;
	}
}
