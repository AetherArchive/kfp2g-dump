using System;
using System.Collections.Generic;
using SGNFW.Mst;

// Token: 0x020000E5 RID: 229
public class QuestStaticQuestOne
{
	// Token: 0x17000259 RID: 601
	// (get) Token: 0x06000A27 RID: 2599 RVA: 0x0003BE7C File Offset: 0x0003A07C
	public int questId
	{
		get
		{
			return this.mstQuestOneData.questOneId;
		}
	}

	// Token: 0x1700025A RID: 602
	// (get) Token: 0x06000A28 RID: 2600 RVA: 0x0003BE89 File Offset: 0x0003A089
	public int questGroupId
	{
		get
		{
			return this.mstQuestOneData.questGroupId;
		}
	}

	// Token: 0x1700025B RID: 603
	// (get) Token: 0x06000A29 RID: 2601 RVA: 0x0003BE96 File Offset: 0x0003A096
	public int useItemId
	{
		get
		{
			return this.mstQuestOneData.useItemId;
		}
	}

	// Token: 0x1700025C RID: 604
	// (get) Token: 0x06000A2A RID: 2602 RVA: 0x0003BEA3 File Offset: 0x0003A0A3
	public int useItemNum
	{
		get
		{
			return this.mstQuestOneData.useItemNum;
		}
	}

	// Token: 0x1700025D RID: 605
	// (get) Token: 0x06000A2B RID: 2603 RVA: 0x0003BEB0 File Offset: 0x0003A0B0
	// (set) Token: 0x06000A2C RID: 2604 RVA: 0x0003BEB8 File Offset: 0x0003A0B8
	public List<QuestStaticQuestOne.RewardItem> RewardItemList { get; private set; }

	// Token: 0x1700025E RID: 606
	// (get) Token: 0x06000A2D RID: 2605 RVA: 0x0003BEC1 File Offset: 0x0003A0C1
	public string questName
	{
		get
		{
			return this.mstQuestOneData.questOneName;
		}
	}

	// Token: 0x1700025F RID: 607
	// (get) Token: 0x06000A2E RID: 2606 RVA: 0x0003BECE File Offset: 0x0003A0CE
	public QuestStaticQuestOne.QuestOneCategory QuestCategory
	{
		get
		{
			return (QuestStaticQuestOne.QuestOneCategory)this.mstQuestOneData.questOneCategory;
		}
	}

	// Token: 0x17000260 RID: 608
	// (get) Token: 0x06000A2F RID: 2607 RVA: 0x0003BEDB File Offset: 0x0003A0DB
	public int dispPriority
	{
		get
		{
			return this.mstQuestOneData.dispPriority;
		}
	}

	// Token: 0x17000261 RID: 609
	// (get) Token: 0x06000A30 RID: 2608 RVA: 0x0003BEE8 File Offset: 0x0003A0E8
	public int ruleId
	{
		get
		{
			return this.mstQuestOneData.ruleId;
		}
	}

	// Token: 0x17000262 RID: 610
	// (get) Token: 0x06000A31 RID: 2609 RVA: 0x0003BEF5 File Offset: 0x0003A0F5
	public int ownHelperFlag
	{
		get
		{
			return this.mstQuestOneData.ownHelperFlag;
		}
	}

	// Token: 0x17000263 RID: 611
	// (get) Token: 0x06000A32 RID: 2610 RVA: 0x0003BF02 File Offset: 0x0003A102
	public string scenarioBeforeId
	{
		get
		{
			return this.mstQuestOneData.scenarioBeforeId;
		}
	}

	// Token: 0x17000264 RID: 612
	// (get) Token: 0x06000A33 RID: 2611 RVA: 0x0003BF0F File Offset: 0x0003A10F
	public string scenarioAfterId
	{
		get
		{
			return this.mstQuestOneData.scenarioAfterId;
		}
	}

	// Token: 0x17000265 RID: 613
	// (get) Token: 0x06000A34 RID: 2612 RVA: 0x0003BF1C File Offset: 0x0003A11C
	public int relQuestId
	{
		get
		{
			return this.mstQuestOneData.relQuestId1;
		}
	}

	// Token: 0x17000266 RID: 614
	// (get) Token: 0x06000A35 RID: 2613 RVA: 0x0003BF29 File Offset: 0x0003A129
	// (set) Token: 0x06000A36 RID: 2614 RVA: 0x0003BF31 File Offset: 0x0003A131
	public List<QuestStaticQuestOne.ReleaseConditions> ReleaseConditionsList { get; set; }

	// Token: 0x17000267 RID: 615
	// (get) Token: 0x06000A37 RID: 2615 RVA: 0x0003BF3A File Offset: 0x0003A13A
	public int evalSetId
	{
		get
		{
			return this.mstQuestOneData.evalSetId;
		}
	}

	// Token: 0x17000268 RID: 616
	// (get) Token: 0x06000A38 RID: 2616 RVA: 0x0003BF47 File Offset: 0x0003A147
	public int compItemId
	{
		get
		{
			return this.mstQuestOneData.compItemId;
		}
	}

	// Token: 0x17000269 RID: 617
	// (get) Token: 0x06000A39 RID: 2617 RVA: 0x0003BF54 File Offset: 0x0003A154
	public int difficulty
	{
		get
		{
			return this.mstQuestOneData.difficulty;
		}
	}

	// Token: 0x1700026A RID: 618
	// (get) Token: 0x06000A3A RID: 2618 RVA: 0x0003BF61 File Offset: 0x0003A161
	public int stamina
	{
		get
		{
			return this.mstQuestOneData.stamina;
		}
	}

	// Token: 0x1700026B RID: 619
	// (get) Token: 0x06000A3B RID: 2619 RVA: 0x0003BF6E File Offset: 0x0003A16E
	public bool ContinueImpossible
	{
		get
		{
			return 1 == this.mstQuestOneData.continueImpossible;
		}
	}

	// Token: 0x1700026C RID: 620
	// (get) Token: 0x06000A3C RID: 2620 RVA: 0x0003BF7E File Offset: 0x0003A17E
	public int goldNum
	{
		get
		{
			return this.mstQuestOneData.goldNum;
		}
	}

	// Token: 0x1700026D RID: 621
	// (get) Token: 0x06000A3D RID: 2621 RVA: 0x0003BF8B File Offset: 0x0003A18B
	public int userExp
	{
		get
		{
			return this.mstQuestOneData.userExp;
		}
	}

	// Token: 0x1700026E RID: 622
	// (get) Token: 0x06000A3E RID: 2622 RVA: 0x0003BF98 File Offset: 0x0003A198
	public int charaExp
	{
		get
		{
			return this.mstQuestOneData.charaExp;
		}
	}

	// Token: 0x1700026F RID: 623
	// (get) Token: 0x06000A3F RID: 2623 RVA: 0x0003BFA5 File Offset: 0x0003A1A5
	public CharaDef.AttributeMask attrMask
	{
		get
		{
			return (CharaDef.AttributeMask)this.mstQuestOneData.attkMask;
		}
	}

	// Token: 0x17000270 RID: 624
	// (get) Token: 0x06000A40 RID: 2624 RVA: 0x0003BFB2 File Offset: 0x0003A1B2
	// (set) Token: 0x06000A41 RID: 2625 RVA: 0x0003BFBA File Offset: 0x0003A1BA
	public CharaDef.AbilityTraits traitsType { get; private set; }

	// Token: 0x17000271 RID: 625
	// (get) Token: 0x06000A42 RID: 2626 RVA: 0x0003BFC3 File Offset: 0x0003A1C3
	// (set) Token: 0x06000A43 RID: 2627 RVA: 0x0003BFCB File Offset: 0x0003A1CB
	public CharaDef.AbilityTraits2 traitsType2 { get; private set; }

	// Token: 0x17000272 RID: 626
	// (get) Token: 0x06000A44 RID: 2628 RVA: 0x0003BFD4 File Offset: 0x0003A1D4
	// (set) Token: 0x06000A45 RID: 2629 RVA: 0x0003BFDC File Offset: 0x0003A1DC
	public bool isNightTraits { get; private set; }

	// Token: 0x17000273 RID: 627
	// (get) Token: 0x06000A46 RID: 2630 RVA: 0x0003BFE5 File Offset: 0x0003A1E5
	public int limitClearNum
	{
		get
		{
			return this.mstQuestOneData.limitClearNum;
		}
	}

	// Token: 0x17000274 RID: 628
	// (get) Token: 0x06000A47 RID: 2631 RVA: 0x0003BFF2 File Offset: 0x0003A1F2
	public string stagePresetId
	{
		get
		{
			return this.mstQuestOneData.stagePresetId;
		}
	}

	// Token: 0x17000275 RID: 629
	// (get) Token: 0x06000A48 RID: 2632 RVA: 0x0003BFFF File Offset: 0x0003A1FF
	public string stageName
	{
		get
		{
			return this.mstQuestOneData.stageName;
		}
	}

	// Token: 0x17000276 RID: 630
	// (get) Token: 0x06000A49 RID: 2633 RVA: 0x0003C00C File Offset: 0x0003A20C
	public int kizunaExp
	{
		get
		{
			return this.mstQuestOneData.kizunaExp;
		}
	}

	// Token: 0x17000277 RID: 631
	// (get) Token: 0x06000A4A RID: 2634 RVA: 0x0003C019 File Offset: 0x0003A219
	public int kizunabonusCharaId
	{
		get
		{
			return this.mstQuestOneData.kizunabonusCharaId;
		}
	}

	// Token: 0x17000278 RID: 632
	// (get) Token: 0x06000A4B RID: 2635 RVA: 0x0003C026 File Offset: 0x0003A226
	public int kizunabonusRatio
	{
		get
		{
			return this.mstQuestOneData.kizunabonusRatio;
		}
	}

	// Token: 0x17000279 RID: 633
	// (get) Token: 0x06000A4C RID: 2636 RVA: 0x0003C033 File Offset: 0x0003A233
	public int rewardPhoto
	{
		get
		{
			return this.mstQuestOneData.rewardPhoto;
		}
	}

	// Token: 0x1700027A RID: 634
	// (get) Token: 0x06000A4D RID: 2637 RVA: 0x0003C040 File Offset: 0x0003A240
	public bool clearPerformance
	{
		get
		{
			return this.mstQuestOneData.clearPerformance != 0;
		}
	}

	// Token: 0x1700027B RID: 635
	// (get) Token: 0x06000A4E RID: 2638 RVA: 0x0003C050 File Offset: 0x0003A250
	public int rewardGroupId
	{
		get
		{
			return this.mstQuestOneData.rewardGroupId;
		}
	}

	// Token: 0x1700027C RID: 636
	// (get) Token: 0x06000A4F RID: 2639 RVA: 0x0003C05D File Offset: 0x0003A25D
	public int memoryCharaId01
	{
		get
		{
			return this.mstQuestOneData.memoryCharaId01;
		}
	}

	// Token: 0x1700027D RID: 637
	// (get) Token: 0x06000A50 RID: 2640 RVA: 0x0003C06A File Offset: 0x0003A26A
	public int memoryCharaId02
	{
		get
		{
			return this.mstQuestOneData.memoryCharaId02;
		}
	}

	// Token: 0x1700027E RID: 638
	// (get) Token: 0x06000A51 RID: 2641 RVA: 0x0003C077 File Offset: 0x0003A277
	public QuestStaticQuestOne.MemoryTextType memoryTextType
	{
		get
		{
			return (QuestStaticQuestOne.MemoryTextType)this.mstQuestOneData.memoryTextType;
		}
	}

	// Token: 0x1700027F RID: 639
	// (get) Token: 0x06000A52 RID: 2642 RVA: 0x0003C084 File Offset: 0x0003A284
	public string memoryText01
	{
		get
		{
			return this.mstQuestOneData.memoryText01;
		}
	}

	// Token: 0x17000280 RID: 640
	// (get) Token: 0x06000A53 RID: 2643 RVA: 0x0003C091 File Offset: 0x0003A291
	public string memoryText02
	{
		get
		{
			return this.mstQuestOneData.memoryText02;
		}
	}

	// Token: 0x17000281 RID: 641
	// (get) Token: 0x06000A54 RID: 2644 RVA: 0x0003C09E File Offset: 0x0003A29E
	public QuestUtil.SkipType skippableFlag
	{
		get
		{
			return (QuestUtil.SkipType)this.mstQuestOneData.skippableFlag;
		}
	}

	// Token: 0x17000282 RID: 642
	// (get) Token: 0x06000A55 RID: 2645 RVA: 0x0003C0AB File Offset: 0x0003A2AB
	public int limitSkipNum
	{
		get
		{
			return this.mstQuestOneData.limitSkipNum;
		}
	}

	// Token: 0x17000283 RID: 643
	// (get) Token: 0x06000A56 RID: 2646 RVA: 0x0003C0B8 File Offset: 0x0003A2B8
	public int limitSkipRecoveryNum
	{
		get
		{
			return this.mstQuestOneData.limitSkipRecoveryNum;
		}
	}

	// Token: 0x17000284 RID: 644
	// (get) Token: 0x06000A57 RID: 2647 RVA: 0x0003C0C5 File Offset: 0x0003A2C5
	// (set) Token: 0x06000A58 RID: 2648 RVA: 0x0003C0CD File Offset: 0x0003A2CD
	public QuestStaticWave waveData { get; set; }

	// Token: 0x17000285 RID: 645
	// (get) Token: 0x06000A59 RID: 2649 RVA: 0x0003C0D8 File Offset: 0x0003A2D8
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

	// Token: 0x17000286 RID: 646
	// (get) Token: 0x06000A5A RID: 2650 RVA: 0x0003C178 File Offset: 0x0003A378
	// (set) Token: 0x06000A5B RID: 2651 RVA: 0x0003C180 File Offset: 0x0003A380
	public List<DataManagerQuest.DrawItemTermData> drawItemTermDataList { get; set; }

	// Token: 0x17000287 RID: 647
	// (get) Token: 0x06000A5C RID: 2652 RVA: 0x0003C189 File Offset: 0x0003A389
	// (set) Token: 0x06000A5D RID: 2653 RVA: 0x0003C191 File Offset: 0x0003A391
	private List<DataManagerQuest.DrawItemData> _drawItemIdList { get; set; }

	// Token: 0x06000A5E RID: 2654 RVA: 0x0003C19A File Offset: 0x0003A39A
	public void AddDrawItemIdListExternal(DataManagerQuest.DrawItemData item)
	{
		this._drawItemIdList.Add(item);
	}

	// Token: 0x17000288 RID: 648
	// (get) Token: 0x06000A5F RID: 2655 RVA: 0x0003C1A8 File Offset: 0x0003A3A8
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

	// Token: 0x17000289 RID: 649
	// (get) Token: 0x06000A60 RID: 2656 RVA: 0x0003C24C File Offset: 0x0003A44C
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

	// Token: 0x1700028A RID: 650
	// (get) Token: 0x06000A61 RID: 2657 RVA: 0x0003C2EC File Offset: 0x0003A4EC
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

	// Token: 0x1700028B RID: 651
	// (get) Token: 0x06000A62 RID: 2658 RVA: 0x0003C390 File Offset: 0x0003A590
	// (set) Token: 0x06000A63 RID: 2659 RVA: 0x0003C398 File Offset: 0x0003A598
	public ItemInput OpenKeyItem { get; private set; }

	// Token: 0x1700028C RID: 652
	// (get) Token: 0x06000A64 RID: 2660 RVA: 0x0003C3A1 File Offset: 0x0003A5A1
	// (set) Token: 0x06000A65 RID: 2661 RVA: 0x0003C3A9 File Offset: 0x0003A5A9
	public ItemInput RecoveryKeyItem { get; private set; }

	// Token: 0x1700028D RID: 653
	// (get) Token: 0x06000A66 RID: 2662 RVA: 0x0003C3B2 File Offset: 0x0003A5B2
	public int RecoveryMaxNum
	{
		get
		{
			return this.mstQuestOneData.recoveryMaxNum;
		}
	}

	// Token: 0x1700028E RID: 654
	// (get) Token: 0x06000A67 RID: 2663 RVA: 0x0003C3BF File Offset: 0x0003A5BF
	public bool StoryOnly
	{
		get
		{
			return this.mstQuestOneData.storyOnly != 0;
		}
	}

	// Token: 0x06000A68 RID: 2664 RVA: 0x0003C3CF File Offset: 0x0003A5CF
	public QuestStaticQuestOne()
	{
	}

	// Token: 0x06000A69 RID: 2665 RVA: 0x0003C3F8 File Offset: 0x0003A5F8
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

	// Token: 0x06000A6A RID: 2666 RVA: 0x0003C69C File Offset: 0x0003A89C
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

	// Token: 0x06000A6B RID: 2667 RVA: 0x0003C7FC File Offset: 0x0003A9FC
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

	// Token: 0x0400082D RID: 2093
	public List<int> DummyHelperList;

	// Token: 0x0400082F RID: 2095
	public CharaDef.AttributeMask ennemyAttrMask;

	// Token: 0x04000830 RID: 2096
	public HashSet<MstQuestDrawItemData> EnemyDropDrawDataList = new HashSet<MstQuestDrawItemData>();

	// Token: 0x04000831 RID: 2097
	public HashSet<MstQuestQuestdropItemData> QuestDropDrawDataList = new HashSet<MstQuestQuestdropItemData>();

	// Token: 0x04000832 RID: 2098
	public HashSet<int> EnemyDropItemIdList = new HashSet<int>();

	// Token: 0x04000835 RID: 2101
	private List<DataManagerPhoto.PhotoDropItemData> _photoDropItemList;

	// Token: 0x04000838 RID: 2104
	private MstQuestQuestoneData mstQuestOneData;

	// Token: 0x020007DC RID: 2012
	public class RewardItem
	{
		// Token: 0x04003502 RID: 13570
		public int itemId;

		// Token: 0x04003503 RID: 13571
		public int num;
	}

	// Token: 0x020007DD RID: 2013
	public class ReleaseConditions
	{
		// Token: 0x06003764 RID: 14180 RVA: 0x001C81D1 File Offset: 0x001C63D1
		public ReleaseConditions(int id, int type)
		{
			this.questId = id;
			this.questType = type;
		}

		// Token: 0x04003504 RID: 13572
		public int questId;

		// Token: 0x04003505 RID: 13573
		public int questType;
	}

	// Token: 0x020007DE RID: 2014
	public enum MemoryTextType
	{
		// Token: 0x04003507 RID: 13575
		Single,
		// Token: 0x04003508 RID: 13576
		Double,
		// Token: 0x04003509 RID: 13577
		TwoLine
	}

	// Token: 0x020007DF RID: 2015
	public enum QuestOneCategory
	{
		// Token: 0x0400350B RID: 13579
		Invalid,
		// Token: 0x0400350C RID: 13580
		NoPlayer,
		// Token: 0x0400350D RID: 13581
		NoDhole
	}
}
