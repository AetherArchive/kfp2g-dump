using System;
using System.Collections.Generic;
using SGNFW.Mst;

// Token: 0x020000E4 RID: 228
public class QuestStaticQuestGroup
{
	// Token: 0x17000236 RID: 566
	// (get) Token: 0x060009FE RID: 2558 RVA: 0x0003BA13 File Offset: 0x00039C13
	public int questGroupId
	{
		get
		{
			return this.mstData.questGroupId;
		}
	}

	// Token: 0x17000237 RID: 567
	// (get) Token: 0x060009FF RID: 2559 RVA: 0x0003BA20 File Offset: 0x00039C20
	public int mapId
	{
		get
		{
			return this.mstData.mapId;
		}
	}

	// Token: 0x17000238 RID: 568
	// (get) Token: 0x06000A00 RID: 2560 RVA: 0x0003BA2D File Offset: 0x00039C2D
	public string titleName
	{
		get
		{
			return this.mstData.titleName;
		}
	}

	// Token: 0x17000239 RID: 569
	// (get) Token: 0x06000A01 RID: 2561 RVA: 0x0003BA3A File Offset: 0x00039C3A
	public string storyName
	{
		get
		{
			return this.mstData.storyName;
		}
	}

	// Token: 0x1700023A RID: 570
	// (get) Token: 0x06000A02 RID: 2562 RVA: 0x0003BA47 File Offset: 0x00039C47
	public int titleCategory
	{
		get
		{
			return this.mstData.titleCategory;
		}
	}

	// Token: 0x1700023B RID: 571
	// (get) Token: 0x06000A03 RID: 2563 RVA: 0x0003BA54 File Offset: 0x00039C54
	public int dispPriority
	{
		get
		{
			return this.mstData.dispPriority;
		}
	}

	// Token: 0x1700023C RID: 572
	// (get) Token: 0x06000A04 RID: 2564 RVA: 0x0003BA61 File Offset: 0x00039C61
	public DataManagerEvent.CoopType CoopType
	{
		get
		{
			return (DataManagerEvent.CoopType)this.mstData.questGroupCategory;
		}
	}

	// Token: 0x1700023D RID: 573
	// (get) Token: 0x06000A05 RID: 2565 RVA: 0x0003BA6E File Offset: 0x00039C6E
	public QuestStaticQuestGroup.GroupCategory QuestGroupCategory
	{
		get
		{
			return (QuestStaticQuestGroup.GroupCategory)this.mstData.questGroupCategory;
		}
	}

	// Token: 0x1700023E RID: 574
	// (get) Token: 0x06000A06 RID: 2566 RVA: 0x0003BA7B File Offset: 0x00039C7B
	// (set) Token: 0x06000A07 RID: 2567 RVA: 0x0003BA83 File Offset: 0x00039C83
	public List<QuestStaticQuestGroup.Chara> CharaList { get; private set; }

	// Token: 0x1700023F RID: 575
	// (get) Token: 0x06000A08 RID: 2568 RVA: 0x0003BA8C File Offset: 0x00039C8C
	public string charaComment
	{
		get
		{
			return this.mstData.dispCharaComment;
		}
	}

	// Token: 0x17000240 RID: 576
	// (get) Token: 0x06000A09 RID: 2569 RVA: 0x0003BA99 File Offset: 0x00039C99
	public int charaId
	{
		get
		{
			return this.mstData.dispCharaId;
		}
	}

	// Token: 0x17000241 RID: 577
	// (get) Token: 0x06000A0A RID: 2570 RVA: 0x0003BAA6 File Offset: 0x00039CA6
	public string charaBodyMotionId
	{
		get
		{
			return this.mstData.dispCharaBodyMotion;
		}
	}

	// Token: 0x17000242 RID: 578
	// (get) Token: 0x06000A0B RID: 2571 RVA: 0x0003BAB3 File Offset: 0x00039CB3
	public string charaFaceMotionId
	{
		get
		{
			return this.mstData.dispCharaFaceMotion;
		}
	}

	// Token: 0x17000243 RID: 579
	// (get) Token: 0x06000A0C RID: 2572 RVA: 0x0003BAC0 File Offset: 0x00039CC0
	public int targetCharaId
	{
		get
		{
			return this.mstData.relCharaId;
		}
	}

	// Token: 0x17000244 RID: 580
	// (get) Token: 0x06000A0D RID: 2573 RVA: 0x0003BACD File Offset: 0x00039CCD
	public DateTime startTime
	{
		get
		{
			return new DateTime(PrjUtil.ConvertTimeToTicks(this.mstData.startDatetime));
		}
	}

	// Token: 0x17000245 RID: 581
	// (get) Token: 0x06000A0E RID: 2574 RVA: 0x0003BAE4 File Offset: 0x00039CE4
	public DateTime endTime
	{
		get
		{
			return new DateTime(PrjUtil.ConvertTimeToTicks(this.mstData.endDatetime));
		}
	}

	// Token: 0x17000246 RID: 582
	// (get) Token: 0x06000A0F RID: 2575 RVA: 0x0003BAFC File Offset: 0x00039CFC
	public int targetCharaKizunaLevel
	{
		get
		{
			int num = this.mstData.questGroupId % 10;
			if (this.targetCharaId <= 0 || num == 4)
			{
				return 0;
			}
			return num;
		}
	}

	// Token: 0x17000247 RID: 583
	// (get) Token: 0x06000A10 RID: 2576 RVA: 0x0003BB28 File Offset: 0x00039D28
	public int targetCharaArtsLevel
	{
		get
		{
			int num = this.mstData.questGroupId % 10;
			if (this.targetCharaId <= 0 || num != 4)
			{
				return 0;
			}
			return 5;
		}
	}

	// Token: 0x17000248 RID: 584
	// (get) Token: 0x06000A11 RID: 2577 RVA: 0x0003BB54 File Offset: 0x00039D54
	public DateTime startDatetime
	{
		get
		{
			return new DateTime(PrjUtil.ConvertTimeToTicks(this.mstData.startDatetime));
		}
	}

	// Token: 0x17000249 RID: 585
	// (get) Token: 0x06000A12 RID: 2578 RVA: 0x0003BB6B File Offset: 0x00039D6B
	public DateTime endDatetime
	{
		get
		{
			return new DateTime(PrjUtil.ConvertTimeToTicks(this.mstData.endDatetime));
		}
	}

	// Token: 0x1700024A RID: 586
	// (get) Token: 0x06000A13 RID: 2579 RVA: 0x0003BB82 File Offset: 0x00039D82
	public bool dispLimitTime
	{
		get
		{
			return this.mstData.timeDispFlg != 0;
		}
	}

	// Token: 0x1700024B RID: 587
	// (get) Token: 0x06000A14 RID: 2580 RVA: 0x0003BB92 File Offset: 0x00039D92
	public DateTime limitTime
	{
		get
		{
			return new DateTime(PrjUtil.ConvertTimeToTicks(this.mstData.endDatetime));
		}
	}

	// Token: 0x1700024C RID: 588
	// (get) Token: 0x06000A15 RID: 2581 RVA: 0x0003BBA9 File Offset: 0x00039DA9
	// (set) Token: 0x06000A16 RID: 2582 RVA: 0x0003BBB1 File Offset: 0x00039DB1
	public List<QuestStaticQuestOne> questOneList { get; set; }

	// Token: 0x1700024D RID: 589
	// (get) Token: 0x06000A17 RID: 2583 RVA: 0x0003BBBA File Offset: 0x00039DBA
	public int limitClearNum
	{
		get
		{
			return this.mstData.limitClearNum;
		}
	}

	// Token: 0x1700024E RID: 590
	// (get) Token: 0x06000A18 RID: 2584 RVA: 0x0003BBC7 File Offset: 0x00039DC7
	public int autoModeEnable
	{
		get
		{
			return this.mstData.autoModeEnable;
		}
	}

	// Token: 0x1700024F RID: 591
	// (get) Token: 0x06000A19 RID: 2585 RVA: 0x0003BBD4 File Offset: 0x00039DD4
	public int growthEventId
	{
		get
		{
			return this.mstData.growthEventId;
		}
	}

	// Token: 0x17000250 RID: 592
	// (get) Token: 0x06000A1A RID: 2586 RVA: 0x0003BBE1 File Offset: 0x00039DE1
	// (set) Token: 0x06000A1B RID: 2587 RVA: 0x0003BBE9 File Offset: 0x00039DE9
	public List<DataManagerQuest.DrawItemTermData> drawItemTermDataList { get; set; }

	// Token: 0x17000251 RID: 593
	// (get) Token: 0x06000A1C RID: 2588 RVA: 0x0003BBF2 File Offset: 0x00039DF2
	// (set) Token: 0x06000A1D RID: 2589 RVA: 0x0003BBFA File Offset: 0x00039DFA
	private List<DataManagerQuest.DrawItemData> _drawItemIdList { get; set; }

	// Token: 0x17000252 RID: 594
	// (get) Token: 0x06000A1E RID: 2590 RVA: 0x0003BC04 File Offset: 0x00039E04
	public List<DataManagerQuest.DrawItemData> DrawItemIdList
	{
		get
		{
			List<DataManagerQuest.DrawItemData> list = this._drawItemIdList.FindAll((DataManagerQuest.DrawItemData x) => x.StartDateTime <= TimeManager.Now && TimeManager.Now < x.EndDateTime);
			if (list.Count == 0 && DataManager.DmQuest.QuestStaticData.mapDataMap.ContainsKey(this.mapId) && 0 < DataManager.DmQuest.QuestStaticData.mapDataMap[this.mapId].DrawItemIdList.Count)
			{
				return DataManager.DmQuest.QuestStaticData.mapDataMap[this.mapId].DrawItemIdList;
			}
			return list;
		}
	}

	// Token: 0x17000253 RID: 595
	// (get) Token: 0x06000A1F RID: 2591 RVA: 0x0003BCA8 File Offset: 0x00039EA8
	public bool HideUnreleasedBelt
	{
		get
		{
			return this.mstData.hideFlag == 1;
		}
	}

	// Token: 0x17000254 RID: 596
	// (get) Token: 0x06000A20 RID: 2592 RVA: 0x0003BCB8 File Offset: 0x00039EB8
	public bool limitGroupFlag
	{
		get
		{
			return this.mstData.limitGroupFlag == 1;
		}
	}

	// Token: 0x17000255 RID: 597
	// (get) Token: 0x06000A21 RID: 2593 RVA: 0x0003BCC8 File Offset: 0x00039EC8
	public QuestUtil.SkipType SkippableFlag
	{
		get
		{
			return (QuestUtil.SkipType)this.mstData.skippableFlag;
		}
	}

	// Token: 0x17000256 RID: 598
	// (get) Token: 0x06000A22 RID: 2594 RVA: 0x0003BCD5 File Offset: 0x00039ED5
	public int LimitSkipNum
	{
		get
		{
			return this.mstData.limitSkipNum;
		}
	}

	// Token: 0x17000257 RID: 599
	// (get) Token: 0x06000A23 RID: 2595 RVA: 0x0003BCE2 File Offset: 0x00039EE2
	public int LimitSkipRecoveryNum
	{
		get
		{
			return this.mstData.limitSkipRecoveryNum;
		}
	}

	// Token: 0x17000258 RID: 600
	// (get) Token: 0x06000A24 RID: 2596 RVA: 0x0003BCF0 File Offset: 0x00039EF0
	public List<DataManagerPhoto.PhotoDropItemData> PhotoDropItemList
	{
		get
		{
			List<DataManagerPhoto.PhotoDropItemData> list = this._photoDropItemList.FindAll((DataManagerPhoto.PhotoDropItemData x) => x.StartDateTime <= TimeManager.Now && TimeManager.Now < x.EndDateTime);
			if (list.Count == 0 && DataManager.DmQuest.QuestStaticData.mapDataMap.ContainsKey(this.mapId) && 0 < DataManager.DmQuest.QuestStaticData.mapDataMap[this.mapId].PhotoDropItemList.Count)
			{
				return DataManager.DmQuest.QuestStaticData.mapDataMap[this.mapId].PhotoDropItemList;
			}
			return list;
		}
	}

	// Token: 0x06000A25 RID: 2597 RVA: 0x0003BD94 File Offset: 0x00039F94
	public QuestStaticQuestGroup(MstQuestQuestgroupData m)
		: this(m, new List<DataManagerQuest.DrawItemData>(), new List<DataManagerPhoto.PhotoDropItemData>())
	{
	}

	// Token: 0x06000A26 RID: 2598 RVA: 0x0003BDA8 File Offset: 0x00039FA8
	public QuestStaticQuestGroup(MstQuestQuestgroupData m, List<DataManagerQuest.DrawItemData> drawItemDataList, List<DataManagerPhoto.PhotoDropItemData> mstPhotoDropItemDataList)
	{
		this.mstData = m;
		this._drawItemIdList = drawItemDataList;
		this._photoDropItemList = mstPhotoDropItemDataList.FindAll((DataManagerPhoto.PhotoDropItemData x) => x.TargetId == m.questGroupId);
		this.CharaList = new List<QuestStaticQuestGroup.Chara>();
		if (m.dispCharaId != 0)
		{
			this.CharaList.Add(new QuestStaticQuestGroup.Chara(1, m.dispCharaId, m.dispCharaBodyMotion, m.dispCharaFaceMotion));
		}
		if (m.dispCharaId2 != 0)
		{
			this.CharaList.Add(new QuestStaticQuestGroup.Chara(2, m.dispCharaId2, m.dispChara2BodyMotion, m.dispChara2FaceMotion));
		}
	}

	// Token: 0x04000822 RID: 2082
	private MstQuestQuestgroupData mstData;

	// Token: 0x04000827 RID: 2087
	private List<DataManagerPhoto.PhotoDropItemData> _photoDropItemList;

	// Token: 0x020007D8 RID: 2008
	public class Chara
	{
		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x06003754 RID: 14164 RVA: 0x001C80E3 File Offset: 0x001C62E3
		// (set) Token: 0x06003755 RID: 14165 RVA: 0x001C80EB File Offset: 0x001C62EB
		public int DispNum { get; private set; }

		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x06003756 RID: 14166 RVA: 0x001C80F4 File Offset: 0x001C62F4
		// (set) Token: 0x06003757 RID: 14167 RVA: 0x001C80FC File Offset: 0x001C62FC
		public int Id { get; private set; }

		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x06003758 RID: 14168 RVA: 0x001C8105 File Offset: 0x001C6305
		// (set) Token: 0x06003759 RID: 14169 RVA: 0x001C810D File Offset: 0x001C630D
		public string BodyMotion { get; private set; }

		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x0600375A RID: 14170 RVA: 0x001C8116 File Offset: 0x001C6316
		// (set) Token: 0x0600375B RID: 14171 RVA: 0x001C811E File Offset: 0x001C631E
		public string FaceMotion { get; private set; }

		// Token: 0x0600375C RID: 14172 RVA: 0x001C8127 File Offset: 0x001C6327
		public Chara(int num, int id, string body, string face)
		{
			this.DispNum = num;
			this.Id = id;
			this.BodyMotion = body;
			this.FaceMotion = face;
		}
	}

	// Token: 0x020007D9 RID: 2009
	public enum GroupCategory
	{
		// Token: 0x040034FA RID: 13562
		Default,
		// Token: 0x040034FB RID: 13563
		CoopNormal = 101,
		// Token: 0x040034FC RID: 13564
		CoopDifficult,
		// Token: 0x040034FD RID: 13565
		CoopBonus
	}
}
