using System;

// Token: 0x020000A9 RID: 169
public class TrainingPackData
{
	// Token: 0x0600078A RID: 1930 RVA: 0x00033E98 File Offset: 0x00032098
	public TrainingStaticData.RewardData GetRewardDataByScore(long score)
	{
		foreach (TrainingStaticData.RewardData rewardData in this.staticData.rewardList)
		{
			if (score >= rewardData.PointRangeUnder)
			{
				return rewardData;
			}
		}
		return null;
	}

	// Token: 0x0600078B RID: 1931 RVA: 0x00033EFC File Offset: 0x000320FC
	public bool IsEnableRecovery()
	{
		return this.staticData.RecoveryMax > 0 && this.staticData.RecoveryMax - this.dynamicData.todayRecoveryNum > 0;
	}

	// Token: 0x0600078C RID: 1932 RVA: 0x00033F28 File Offset: 0x00032128
	public bool IsEnablePlay()
	{
		return this.dynamicData.todayPlayNum <= this.dynamicData.todayRecoveryNum;
	}

	// Token: 0x0400068E RID: 1678
	public TrainingDynamicData dynamicData;

	// Token: 0x0400068F RID: 1679
	public TrainingStaticData staticData;
}
