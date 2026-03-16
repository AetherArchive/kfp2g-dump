using System;

public class TrainingPackData
{
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

	public bool IsEnableRecovery()
	{
		return this.staticData.RecoveryMax > 0 && this.staticData.RecoveryMax - this.dynamicData.todayRecoveryNum > 0;
	}

	public bool IsEnablePlay()
	{
		return this.dynamicData.todayPlayNum <= this.dynamicData.todayRecoveryNum;
	}

	public TrainingDynamicData dynamicData;

	public TrainingStaticData staticData;
}
