using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Mst;

public class TrainingStaticData
{
	public int SeasonId
	{
		get
		{
			return this.mstTrainingSeasonData.seasonId;
		}
	}

	public DateTime StartTime
	{
		get
		{
			return new DateTime(PrjUtil.ConvertTimeToTicks(this.mstTrainingSeasonData.seasonStartDatetime));
		}
	}

	public DateTime EndTime
	{
		get
		{
			return new DateTime(PrjUtil.ConvertTimeToTicks(this.mstTrainingSeasonData.seasonEndDatetime));
		}
	}

	public int RecoveryMax
	{
		get
		{
			return this.mstTrainingSeasonData.recoveryMax;
		}
	}

	public int RecoveryStoneNum
	{
		get
		{
			return this.mstTrainingSeasonData.recoveryItemNum;
		}
	}

	public TrainingStaticData(int seasonId, List<MstTrainingSeasonData> mstTrainingSeasonDatas, List<MstTrainingDayofweekData> mstTrainingDayofweekDatas, List<MstTrainingRewardData> mstTrainingRewardDatas)
	{
		this.mstTrainingSeasonData = mstTrainingSeasonDatas.Find((MstTrainingSeasonData item) => item.seasonId == seasonId);
		this.dayOfWeekDataList = new Dictionary<DayOfWeek, TrainingStaticData.DayOfWeekData>();
		using (IEnumerator enumerator = Enum.GetValues(typeof(DayOfWeek)).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				DayOfWeek dow = (DayOfWeek)enumerator.Current;
				this.dayOfWeekDataList.Add(dow, new TrainingStaticData.DayOfWeekData(mstTrainingDayofweekDatas.Find((MstTrainingDayofweekData item) => item.seasonId == seasonId && dow == TimeManager.ConvertDayOfWeekByServer2Client(item.dayOfWeek))));
			}
		}
		this.rewardList = new List<TrainingStaticData.RewardData>();
		foreach (MstTrainingRewardData mstTrainingRewardData in mstTrainingRewardDatas.FindAll((MstTrainingRewardData item) => item.seasonId == seasonId))
		{
			this.rewardList.Add(new TrainingStaticData.RewardData(mstTrainingRewardData));
		}
		this.rewardList.Sort((TrainingStaticData.RewardData a, TrainingStaticData.RewardData b) => b.PointRangeUnder.CompareTo(a.PointRangeUnder));
	}

	private MstTrainingSeasonData mstTrainingSeasonData;

	public Dictionary<DayOfWeek, TrainingStaticData.DayOfWeekData> dayOfWeekDataList;

	public List<TrainingStaticData.RewardData> rewardList;

	public class DayOfWeekData
	{
		public DayOfWeek dayOfWeek
		{
			get
			{
				return TimeManager.ConvertDayOfWeekByServer2Client(this.mstTrainingDayofweekData.dayOfWeek);
			}
		}

		public int seasonId
		{
			get
			{
				return this.mstTrainingDayofweekData.seasonId;
			}
		}

		public int questOneId
		{
			get
			{
				return this.mstTrainingDayofweekData.questOneId;
			}
		}

		public int turnLimit
		{
			get
			{
				return this.mstTrainingDayofweekData.turnLimit;
			}
		}

		public int missionConditions
		{
			get
			{
				return this.mstTrainingDayofweekData.missionConditions;
			}
		}

		public string captureInfoText
		{
			get
			{
				return this.mstTrainingDayofweekData.captureInfoText;
			}
		}

		public string charaText01
		{
			get
			{
				return this.mstTrainingDayofweekData.text01;
			}
		}

		public string charaText02
		{
			get
			{
				return this.mstTrainingDayofweekData.text02;
			}
		}

		public string enemyTexturePath
		{
			get
			{
				return this.mstTrainingDayofweekData.enemyTexturePath;
			}
		}

		public int enemyRevivalHpratio
		{
			get
			{
				return this.mstTrainingDayofweekData.enemyRevivalHpratio;
			}
		}

		public int enemyRevivalAtkratio
		{
			get
			{
				return this.mstTrainingDayofweekData.enemyRevivalAtkratio;
			}
		}

		public int enemyRevivalDefratio
		{
			get
			{
				return this.mstTrainingDayofweekData.enemyRevivalDefratio;
			}
		}

		public DayOfWeekData(MstTrainingDayofweekData mstTrainingDayofweekData)
		{
			this.mstTrainingDayofweekData = mstTrainingDayofweekData;
			this.missionBonusList = new List<TrainingStaticData.DayOfWeekData.MissionBonus>();
			if (mstTrainingDayofweekData == null)
			{
				return;
			}
			if (mstTrainingDayofweekData.missionType01 != 0)
			{
				this.missionBonusList.Add(new TrainingStaticData.DayOfWeekData.MissionBonus
				{
					type = (TrainingStaticData.DayOfWeekData.MissionBonus.Type)mstTrainingDayofweekData.missionType01,
					val = mstTrainingDayofweekData.missionValue01
				});
			}
			if (mstTrainingDayofweekData.missionType02 != 0)
			{
				this.missionBonusList.Add(new TrainingStaticData.DayOfWeekData.MissionBonus
				{
					type = (TrainingStaticData.DayOfWeekData.MissionBonus.Type)mstTrainingDayofweekData.missionType02,
					val = mstTrainingDayofweekData.missionValue02
				});
			}
		}

		private MstTrainingDayofweekData mstTrainingDayofweekData;

		public List<TrainingStaticData.DayOfWeekData.MissionBonus> missionBonusList;

		public class MissionBonus
		{
			public TrainingStaticData.DayOfWeekData.MissionBonus.Type type;

			public int val;

			public enum Type
			{
				INVALID,
				MASTER_SKILL,
				WAIT_SKILL,
				OKAWARI,
				HP,
				MP
			}
		}
	}

	public class RewardData
	{
		public long PointRangeUnder
		{
			get
			{
				return this.mstTrainingRewardData.pvpPointMin;
			}
		}

		public int RewardId
		{
			get
			{
				return this.mstTrainingRewardData.id;
			}
		}

		public RewardData(MstTrainingRewardData mstTrainingRewardData)
		{
			this.mstTrainingRewardData = mstTrainingRewardData;
			this.rewardItemList = new List<ItemInput>();
			if (mstTrainingRewardData.itemId01 != 0)
			{
				this.rewardItemList.Add(new ItemInput(mstTrainingRewardData.itemId01, mstTrainingRewardData.itemNum01));
			}
			if (mstTrainingRewardData.itemId02 != 0)
			{
				this.rewardItemList.Add(new ItemInput(mstTrainingRewardData.itemId02, mstTrainingRewardData.itemNum02));
			}
			if (mstTrainingRewardData.itemId03 != 0)
			{
				this.rewardItemList.Add(new ItemInput(mstTrainingRewardData.itemId03, mstTrainingRewardData.itemNum03));
			}
			if (mstTrainingRewardData.itemId04 != 0)
			{
				this.rewardItemList.Add(new ItemInput(mstTrainingRewardData.itemId04, mstTrainingRewardData.itemNum04));
			}
			if (mstTrainingRewardData.itemId05 != 0)
			{
				this.rewardItemList.Add(new ItemInput(mstTrainingRewardData.itemId05, mstTrainingRewardData.itemNum05));
			}
		}

		private MstTrainingRewardData mstTrainingRewardData;

		public List<ItemInput> rewardItemList;
	}
}
