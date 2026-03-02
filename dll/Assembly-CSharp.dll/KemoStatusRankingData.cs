using System;
using System.Collections.Generic;

// Token: 0x0200007C RID: 124
public class KemoStatusRankingData
{
	// Token: 0x170000E6 RID: 230
	// (get) Token: 0x06000499 RID: 1177 RVA: 0x00021A58 File Offset: 0x0001FC58
	public DateTime lastUpdateTime
	{
		get
		{
			return new DateTime(PrjUtil.ConvertTimeToTicks(this.lastUpdateServerTime));
		}
	}

	// Token: 0x0600049A RID: 1178 RVA: 0x00021A6A File Offset: 0x0001FC6A
	public KemoStatusRankingData(long lastUpdateTime)
	{
		this.lastUpdateServerTime = lastUpdateTime;
		this.rankingList = new List<KemoStatusRankingData.RankingOne>();
	}

	// Token: 0x0600049B RID: 1179 RVA: 0x00021A84 File Offset: 0x0001FC84
	public KemoStatusRankingData()
	{
	}

	// Token: 0x04000516 RID: 1302
	private long lastUpdateServerTime;

	// Token: 0x04000517 RID: 1303
	public List<KemoStatusRankingData.RankingOne> rankingList;

	// Token: 0x04000518 RID: 1304
	public int myRank;

	// Token: 0x020006B0 RID: 1712
	public class RankingOne
	{
		// Token: 0x04003015 RID: 12309
		public int number;

		// Token: 0x04003016 RID: 12310
		public string userName;

		// Token: 0x04003017 RID: 12311
		public int userLevel;

		// Token: 0x04003018 RID: 12312
		public long kemoStatus;

		// Token: 0x04003019 RID: 12313
		public int favoriteCharaId;

		// Token: 0x0400301A RID: 12314
		public int favoriteCharaFaceId;

		// Token: 0x0400301B RID: 12315
		public int achievementId;
	}
}
