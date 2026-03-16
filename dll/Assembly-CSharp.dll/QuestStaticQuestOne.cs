using System;
using System.Collections.Generic;
using SGNFW.Mst;

public class QuestStaticQuestOne
{
	public int questId
	{
		get
		{
			return this.mstQuestOneData.questOneId;
		}
	}

	public int questGroupId
	{
		get
		{
			return this.mstQuestOneData.questGroupId;
		}
	}

	public int useItemId
	{
		get
		{
			return this.mstQuestOneData.useItemId;
		}
	}

	public int useItemNum
	{
		get
		{
			return this.mstQuestOneData.useItemNum;
		}
	}

	public List<QuestStaticQuestOne.RewardItem> RewardItemList { get; private set; }

	public string questName
	{
		get
		{
			return this.mstQuestOneData.questOneName;
		}
	}

	public QuestStaticQuestOne.QuestOneCategory QuestCategory
	{
		get
		{
			return (QuestStaticQuestOne.QuestOneCategory)this.mstQuestOneData.questOneCategory;
		}
	}

	public int dispPriority
	{
		get
		{
			return this.mstQuestOneData.dispPriority;
		}
	}

	public int ruleId
	{
		get
		{
			return this.mstQuestOneData.ruleId;
		}
	}

	public int ownHelperFlag
	{
		get
		{
			return this.mstQuestOneData.ownHelperFlag;
		}
	}

	public string scenarioBeforeId
	{
		get
		{
			return this.mstQuestOneData.scenarioBeforeId;
		}
	}

	public string scenarioAfterId
	{
		get
		{
			return this.mstQuestOneData.scenarioAfterId;
		}
	}

	public int relQuestId
	{
		get
		{
			return this.mstQuestOneData.relQuestId1;
		}
	}

	public List<QuestStaticQuestOne.ReleaseConditions> ReleaseConditionsList { get; set; }

	public int evalSetId
	{
		get
		{
			return this.mstQuestOneData.evalSetId;
		}
	}

	public int compItemId
	{
		get
		{
			return this.mstQuestOneData.compItemId;
		}
	}

	public int difficulty
	{
		get
		{
			return this.mstQuestOneData.difficulty;
		}
	}

	public int stamina
	{
		get
		{
			return this.mstQuestOneData.stamina;
		}
	}

	public bool ContinueImpossible
	{
		get
		{
			return 1 == this.mstQuestOneData.continueImpossible;
		}
	}

	public int goldNum
	{
		get
		{
			return this.mstQuestOneData.goldNum;
		}
	}

	public int userExp
	{
		get
		{
			return this.mstQuestOneData.userExp;
		}
	}

	public int charaExp
	{
		get
		{
			return this.mstQuestOneData.charaExp;
		}
	}

	public CharaDef.AttributeMask attrMask
	{
		get
		{
			return (CharaDef.AttributeMask)this.mstQuestOneData.attkMask;
		}
	}

	public CharaDef.AbilityTraits traitsType { get; private set; }

	public CharaDef.AbilityTraits2 traitsType2 { get; private set; }

	public bool isNightTraits { get; private set; }

	public int limitClearNum
	{
		get
		{
			return this.mstQuestOneData.limitClearNum;
		}
	}

	public string stagePresetId
	{
		get
		{
			return this.mstQuestOneData.stagePresetId;
		}
	}

	public string stageName
	{
		get
		{
			return this.mstQuestOneData.stageName;
		}
	}

	public int kizunaExp
	{
		get
		{
			return this.mstQuestOneData.kizunaExp;
		}
	}

	public int kizunabonusCharaId
	{
		get
		{
			return this.mstQuestOneData.kizunabonusCharaId;
		}
	}

	public int kizunabonusRatio
	{
		get
		{
			return this.mstQuestOneData.kizunabonusRatio;
		}
	}

	public int rewardPhoto
	{
		get
		{
			return this.mstQuestOneData.rewardPhoto;
		}
	}

	public bool clearPerformance
	{
		get
		{
			return this.mstQuestOneData.clearPerformance != 0;
		}
	}

	public int rewardGroupId
	{
		get
		{
			return this.mstQuestOneData.rewardGroupId;
		}
	}

	public int memoryCharaId01
	{
		get
		{
			return this.mstQuestOneData.memoryCharaId01;
		}
	}

	public int memoryCharaId02
	{
		get
		{
			return this.mstQuestOneData.memoryCharaId02;
		}
	}

	public QuestStaticQuestOne.MemoryTextType memoryTextType
	{
		get
		{
			return (QuestStaticQuestOne.MemoryTextType)this.mstQuestOneData.memoryTextType;
		}
	}

	public string memoryText01
	{
		get
		{
			return this.mstQuestOneData.memoryText01;
		}
	}

	public string memoryText02
	{
		get
		{
			return this.mstQuestOneData.memoryText02;
		}
	}

	public QuestUtil.SkipType skippableFlag
	{
		get
		{
			return (QuestUtil.SkipType)this.mstQuestOneData.skippableFlag;
		}
	}

	public int limitSkipNum
	{
		get
		{
			return this.mstQuestOneData.limitSkipNum;
		}
	}

	public int limitSkipRecoveryNum
	{
		get
		{
			return this.mstQuestOneData.limitSkipRecoveryNum;
		}
	}

	public QuestStaticWave waveData { get; set; }

	public HashSet<int> DropItemList
	{
		get
		{
			HashSet<int> hashSet = new HashSet<int>();
			foreach (int num in this.EnemyDropItemIdList)
			{
				hashSet.Add(num);
			}
			foreach (int num2 in this.QuestDropItemList)
			{
				hashSet.Add(num2);
			}
			return hashSet;
		}
	}

	public List<DataManagerQuest.DrawItemTermData> drawItemTermDataList { get; set; }

	private List<DataManagerQuest.DrawItemData> _drawItemIdList { get; set; }

	public void AddDrawItemIdListExternal(DataManagerQuest.DrawItemData item)
	{
		this._drawItemIdList.Add(item);
	}

	public List<DataManagerQuest.DrawItemData> DrawItemIdList
	{
		get
		{
			List<DataManagerQuest.DrawItemData> list = this._drawItemIdList.FindAll((DataManagerQuest.DrawItemData x) => x.StartDateTime <= TimeManager.Now && TimeManager.Now < x.EndDateTime);
			if (list.Count == 0 && DataManager.DmQuest.QuestStaticData.groupDataMap.ContainsKey(this.questGroupId) && 0 < DataManager.DmQuest.QuestStaticData.groupDataMap[this.questGroupId].DrawItemIdList.Count)
			{
				return DataManager.DmQuest.QuestStaticData.groupDataMap[this.questGroupId].DrawItemIdList;
			}
			return list;
		}
	}

	public HashSet<int> QuestDropItemList
	{
		get
		{
			HashSet<int> hashSet = new HashSet<int>();
			DateTime now = TimeManager.Now;
			foreach (DataManagerQuest.DrawItemData drawItemData in this.DrawItemIdList)
			{
				foreach (ItemInput itemInput in drawItemData.DropItemList)
				{
					hashSet.Add(itemInput.itemId);
				}
			}
			return hashSet;
		}
	}

	public List<DataManagerPhoto.PhotoDropItemData> PhotoDropItemList
	{
		get
		{
			List<DataManagerPhoto.PhotoDropItemData> list = this._photoDropItemList.FindAll((DataManagerPhoto.PhotoDropItemData x) => x.StartDateTime <= TimeManager.Now && TimeManager.Now < x.EndDateTime);
			if (list.Count == 0 && DataManager.DmQuest.QuestStaticData.groupDataMap.ContainsKey(this.questGroupId) && 0 < DataManager.DmQuest.QuestStaticData.groupDataMap[this.questGroupId].PhotoDropItemList.Count)
			{
				return DataManager.DmQuest.QuestStaticData.groupDataMap[this.questGroupId].PhotoDropItemList;
			}
			return list;
		}
	}

	public ItemInput OpenKeyItem { get; private set; }

	public ItemInput RecoveryKeyItem { get; private set; }

	public int RecoveryMaxNum
	{
		get
		{
			return this.mstQuestOneData.recoveryMaxNum;
		}
	}

	public bool StoryOnly
	{
		get
		{
			return this.mstQuestOneData.storyOnly != 0;
		}
	}

	public QuestStaticQuestOne()
	{
	}

	public QuestStaticQuestOne(MstQuestQuestoneData questOneData, List<MstQuestRewardgroupData> rewardGroupDataList, List<DataManagerQuest.DrawItemData> drawItemDataList, List<DataManagerPhoto.PhotoDropItemData> mstQuestPhotoDropItemDataList)
	{
		QuestStaticQuestOne <>4__this = this;
		this.mstQuestOneData = questOneData;
		this.DummyHelperList = new List<int>();
		if (this.mstQuestOneData.dummyHelperId01 != 0)
		{
			this.DummyHelperList.Add(this.mstQuestOneData.dummyHelperId01);
		}
		if (this.mstQuestOneData.dummyHelperId02 != 0)
		{
			this.DummyHelperList.Add(this.mstQuestOneData.dummyHelperId02);
		}
		if (this.mstQuestOneData.dummyHelperId03 != 0)
		{
			this.DummyHelperList.Add(this.mstQuestOneData.dummyHelperId03);
		}
		List<MstQuestRewardgroupData> list = rewardGroupDataList.FindAll((MstQuestRewardgroupData x) => <>4__this.mstQuestOneData.rewardGroupId == x.id.groupId);
		this.RewardItemList = new List<QuestStaticQuestOne.RewardItem>();
		foreach (MstQuestRewardgroupData mstQuestRewardgroupData in list)
		{
			QuestStaticQuestOne.RewardItem rewardItem = new QuestStaticQuestOne.RewardItem
			{
				itemId = mstQuestRewardgroupData.itemId,
				num = mstQuestRewardgroupData.num
			};
			this.RewardItemList.Add(rewardItem);
		}
		this.ReleaseConditionsList = new List<QuestStaticQuestOne.ReleaseConditions>
		{
			new QuestStaticQuestOne.ReleaseConditions(this.mstQuestOneData.relQuestId1, this.mstQuestOneData.relQuestType1),
			new QuestStaticQuestOne.ReleaseConditions(this.mstQuestOneData.relQuestId2, this.mstQuestOneData.relQuestType2),
			new QuestStaticQuestOne.ReleaseConditions(this.mstQuestOneData.relQuestId3, this.mstQuestOneData.relQuestType3),
			new QuestStaticQuestOne.ReleaseConditions(this.mstQuestOneData.relQuestId4, this.mstQuestOneData.relQuestType4),
			new QuestStaticQuestOne.ReleaseConditions(this.mstQuestOneData.relQuestId5, this.mstQuestOneData.relQuestType5)
		};
		this.SolutionStageTraits(this.mstQuestOneData.stagePresetId);
		this._drawItemIdList = drawItemDataList;
		List<DataManagerPhoto.PhotoDropItemData> list2 = mstQuestPhotoDropItemDataList.FindAll((DataManagerPhoto.PhotoDropItemData x) => x.TargetId == questOneData.questOneId);
		this._photoDropItemList = list2;
		if (this.mstQuestOneData.openKeyItemId != 0)
		{
			this.OpenKeyItem = new ItemInput
			{
				itemId = this.mstQuestOneData.openKeyItemId,
				num = this.mstQuestOneData.openKeyItemNum
			};
		}
		if (this.mstQuestOneData.recoveryKeyItemId != 0)
		{
			this.RecoveryKeyItem = new ItemInput
			{
				itemId = this.mstQuestOneData.recoveryKeyItemId,
				num = this.mstQuestOneData.recoveryKeyItemNum
			};
		}
	}

	public QuestStaticQuestOne(PvpStaticData pvpStaticData)
	{
		this.mstQuestOneData = new MstQuestQuestoneData();
		this.mstQuestOneData.questOneName = "";
		this.mstQuestOneData.stagePresetId = pvpStaticData.baseData.stagePresetId;
		this.mstQuestOneData.goldNum = 0;
		this.mstQuestOneData.charaExp = pvpStaticData.baseData.winAcquireExp;
		this.mstQuestOneData.kizunaExp = pvpStaticData.baseData.winAcquireKizuna;
		this.ReleaseConditionsList = new List<QuestStaticQuestOne.ReleaseConditions>
		{
			new QuestStaticQuestOne.ReleaseConditions(this.mstQuestOneData.relQuestId1, this.mstQuestOneData.relQuestType1),
			new QuestStaticQuestOne.ReleaseConditions(this.mstQuestOneData.relQuestId2, this.mstQuestOneData.relQuestType2),
			new QuestStaticQuestOne.ReleaseConditions(this.mstQuestOneData.relQuestId3, this.mstQuestOneData.relQuestType3),
			new QuestStaticQuestOne.ReleaseConditions(this.mstQuestOneData.relQuestId4, this.mstQuestOneData.relQuestType4),
			new QuestStaticQuestOne.ReleaseConditions(this.mstQuestOneData.relQuestId5, this.mstQuestOneData.relQuestType5)
		};
		this.SolutionStageTraits(this.mstQuestOneData.stagePresetId);
	}

	public void SolutionStageTraits(string stagePresetId)
	{
		foreach (object obj in Enum.GetValues(typeof(CharaDef.AbilityTraits)))
		{
			CharaDef.AbilityTraits abilityTraits = (CharaDef.AbilityTraits)obj;
			if (abilityTraits != CharaDef.AbilityTraits.without && stagePresetId.Contains(abilityTraits.ToString()))
			{
				this.traitsType = abilityTraits;
				break;
			}
		}
		if (this.traitsType == (CharaDef.AbilityTraits)0)
		{
			this.traitsType = CharaDef.AbilityTraits.without;
		}
		foreach (object obj2 in Enum.GetValues(typeof(CharaDef.AbilityTraits2)))
		{
			CharaDef.AbilityTraits2 abilityTraits2 = (CharaDef.AbilityTraits2)obj2;
			if (abilityTraits2 != CharaDef.AbilityTraits2.without && stagePresetId.Contains(abilityTraits2.ToString()))
			{
				this.traitsType2 = abilityTraits2;
				break;
			}
		}
		if (this.traitsType2 == (CharaDef.AbilityTraits2)0)
		{
			this.traitsType2 = CharaDef.AbilityTraits2.without;
			return;
		}
		this.isNightTraits = true;
	}

	public List<int> DummyHelperList;

	public CharaDef.AttributeMask ennemyAttrMask;

	public HashSet<MstQuestDrawItemData> EnemyDropDrawDataList = new HashSet<MstQuestDrawItemData>();

	public HashSet<MstQuestQuestdropItemData> QuestDropDrawDataList = new HashSet<MstQuestQuestdropItemData>();

	public HashSet<int> EnemyDropItemIdList = new HashSet<int>();

	private List<DataManagerPhoto.PhotoDropItemData> _photoDropItemList;

	private MstQuestQuestoneData mstQuestOneData;

	public class RewardItem
	{
		public int itemId;

		public int num;
	}

	public class ReleaseConditions
	{
		public ReleaseConditions(int id, int type)
		{
			this.questId = id;
			this.questType = type;
		}

		public int questId;

		public int questType;
	}

	public enum MemoryTextType
	{
		Single,
		Double,
		TwoLine
	}

	public enum QuestOneCategory
	{
		Invalid,
		NoPlayer,
		NoDhole
	}
}
