using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Mst;

// Token: 0x020000AA RID: 170
public class TrainingStaticData
{
	// Token: 0x17000167 RID: 359
	// (get) Token: 0x0600078E RID: 1934 RVA: 0x00033F4D File Offset: 0x0003214D
	public int SeasonId
	{
		get
		{
			return this.mstTrainingSeasonData.seasonId;
		}
	}

	// Token: 0x17000168 RID: 360
	// (get) Token: 0x0600078F RID: 1935 RVA: 0x00033F5A File Offset: 0x0003215A
	public DateTime StartTime
	{
		get
		{
			return new DateTime(PrjUtil.ConvertTimeToTicks(this.mstTrainingSeasonData.seasonStartDatetime));
		}
	}

	// Token: 0x17000169 RID: 361
	// (get) Token: 0x06000790 RID: 1936 RVA: 0x00033F71 File Offset: 0x00032171
	public DateTime EndTime
	{
		get
		{
			return new DateTime(PrjUtil.ConvertTimeToTicks(this.mstTrainingSeasonData.seasonEndDatetime));
		}
	}

	// Token: 0x1700016A RID: 362
	// (get) Token: 0x06000791 RID: 1937 RVA: 0x00033F88 File Offset: 0x00032188
	public int RecoveryMax
	{
		get
		{
			return this.mstTrainingSeasonData.recoveryMax;
		}
	}

	// Token: 0x1700016B RID: 363
	// (get) Token: 0x06000792 RID: 1938 RVA: 0x00033F95 File Offset: 0x00032195
	public int RecoveryStoneNum
	{
		get
		{
			return this.mstTrainingSeasonData.recoveryItemNum;
		}
	}

	// Token: 0x06000793 RID: 1939 RVA: 0x00033FA4 File Offset: 0x000321A4
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

	// Token: 0x04000690 RID: 1680
	private MstTrainingSeasonData mstTrainingSeasonData;

	// Token: 0x04000691 RID: 1681
	public Dictionary<DayOfWeek, TrainingStaticData.DayOfWeekData> dayOfWeekDataList;

	// Token: 0x04000692 RID: 1682
	public List<TrainingStaticData.RewardData> rewardList;

	// Token: 0x02000794 RID: 1940
	public class DayOfWeekData
	{
		// Token: 0x1700080A RID: 2058
		// (get) Token: 0x060036AA RID: 13994 RVA: 0x001C6A64 File Offset: 0x001C4C64
		public DayOfWeek dayOfWeek
		{
			get
			{
				return TimeManager.ConvertDayOfWeekByServer2Client(this.mstTrainingDayofweekData.dayOfWeek);
			}
		}

		// Token: 0x1700080B RID: 2059
		// (get) Token: 0x060036AB RID: 13995 RVA: 0x001C6A76 File Offset: 0x001C4C76
		public int seasonId
		{
			get
			{
				return this.mstTrainingDayofweekData.seasonId;
			}
		}

		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x060036AC RID: 13996 RVA: 0x001C6A83 File Offset: 0x001C4C83
		public int questOneId
		{
			get
			{
				return this.mstTrainingDayofweekData.questOneId;
			}
		}

		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x060036AD RID: 13997 RVA: 0x001C6A90 File Offset: 0x001C4C90
		public int turnLimit
		{
			get
			{
				return this.mstTrainingDayofweekData.turnLimit;
			}
		}

		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x060036AE RID: 13998 RVA: 0x001C6A9D File Offset: 0x001C4C9D
		public int missionConditions
		{
			get
			{
				return this.mstTrainingDayofweekData.missionConditions;
			}
		}

		// Token: 0x1700080F RID: 2063
		// (get) Token: 0x060036AF RID: 13999 RVA: 0x001C6AAA File Offset: 0x001C4CAA
		public string captureInfoText
		{
			get
			{
				return this.mstTrainingDayofweekData.captureInfoText;
			}
		}

		// Token: 0x17000810 RID: 2064
		// (get) Token: 0x060036B0 RID: 14000 RVA: 0x001C6AB7 File Offset: 0x001C4CB7
		public string charaText01
		{
			get
			{
				return this.mstTrainingDayofweekData.text01;
			}
		}

		// Token: 0x17000811 RID: 2065
		// (get) Token: 0x060036B1 RID: 14001 RVA: 0x001C6AC4 File Offset: 0x001C4CC4
		public string charaText02
		{
			get
			{
				return this.mstTrainingDayofweekData.text02;
			}
		}

		// Token: 0x17000812 RID: 2066
		// (get) Token: 0x060036B2 RID: 14002 RVA: 0x001C6AD1 File Offset: 0x001C4CD1
		public string enemyTexturePath
		{
			get
			{
				return this.mstTrainingDayofweekData.enemyTexturePath;
			}
		}

		// Token: 0x17000813 RID: 2067
		// (get) Token: 0x060036B3 RID: 14003 RVA: 0x001C6ADE File Offset: 0x001C4CDE
		public int enemyRevivalHpratio
		{
			get
			{
				return this.mstTrainingDayofweekData.enemyRevivalHpratio;
			}
		}

		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x060036B4 RID: 14004 RVA: 0x001C6AEB File Offset: 0x001C4CEB
		public int enemyRevivalAtkratio
		{
			get
			{
				return this.mstTrainingDayofweekData.enemyRevivalAtkratio;
			}
		}

		// Token: 0x17000815 RID: 2069
		// (get) Token: 0x060036B5 RID: 14005 RVA: 0x001C6AF8 File Offset: 0x001C4CF8
		public int enemyRevivalDefratio
		{
			get
			{
				return this.mstTrainingDayofweekData.enemyRevivalDefratio;
			}
		}

		// Token: 0x060036B6 RID: 14006 RVA: 0x001C6B08 File Offset: 0x001C4D08
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

		// Token: 0x040033B1 RID: 13233
		private MstTrainingDayofweekData mstTrainingDayofweekData;

		// Token: 0x040033B2 RID: 13234
		public List<TrainingStaticData.DayOfWeekData.MissionBonus> missionBonusList;

		// Token: 0x02001149 RID: 4425
		public class MissionBonus
		{
			// Token: 0x04005EFD RID: 24317
			public TrainingStaticData.DayOfWeekData.MissionBonus.Type type;

			// Token: 0x04005EFE RID: 24318
			public int val;

			// Token: 0x02001235 RID: 4661
			public enum Type
			{
				// Token: 0x040063AC RID: 25516
				INVALID,
				// Token: 0x040063AD RID: 25517
				MASTER_SKILL,
				// Token: 0x040063AE RID: 25518
				WAIT_SKILL,
				// Token: 0x040063AF RID: 25519
				OKAWARI,
				// Token: 0x040063B0 RID: 25520
				HP,
				// Token: 0x040063B1 RID: 25521
				MP
			}
		}
	}

	// Token: 0x02000795 RID: 1941
	public class RewardData
	{
		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x060036B7 RID: 14007 RVA: 0x001C6B91 File Offset: 0x001C4D91
		public long PointRangeUnder
		{
			get
			{
				return this.mstTrainingRewardData.pvpPointMin;
			}
		}

		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x060036B8 RID: 14008 RVA: 0x001C6B9E File Offset: 0x001C4D9E
		public int RewardId
		{
			get
			{
				return this.mstTrainingRewardData.id;
			}
		}

		// Token: 0x060036B9 RID: 14009 RVA: 0x001C6BAC File Offset: 0x001C4DAC
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

		// Token: 0x040033B3 RID: 13235
		private MstTrainingRewardData mstTrainingRewardData;

		// Token: 0x040033B4 RID: 13236
		public List<ItemInput> rewardItemList;
	}
}
