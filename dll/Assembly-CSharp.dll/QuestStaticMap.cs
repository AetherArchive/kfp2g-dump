using System;
using System.Collections.Generic;
using SGNFW.Mst;

// Token: 0x020000E3 RID: 227
public class QuestStaticMap
{
	// Token: 0x17000220 RID: 544
	// (get) Token: 0x060009E3 RID: 2531 RVA: 0x0003B5B3 File Offset: 0x000397B3
	public int mapId
	{
		get
		{
			return this.mstData.mapId;
		}
	}

	// Token: 0x17000221 RID: 545
	// (get) Token: 0x060009E4 RID: 2532 RVA: 0x0003B5C0 File Offset: 0x000397C0
	public int chapterId
	{
		get
		{
			return this.mstData.chapterId;
		}
	}

	// Token: 0x17000222 RID: 546
	// (get) Token: 0x060009E5 RID: 2533 RVA: 0x0003B5CD File Offset: 0x000397CD
	public string mapName
	{
		get
		{
			return this.mstData.mapName;
		}
	}

	// Token: 0x17000223 RID: 547
	// (get) Token: 0x060009E6 RID: 2534 RVA: 0x0003B5DA File Offset: 0x000397DA
	public int mapPosX
	{
		get
		{
			return this.mstData.mapPosX;
		}
	}

	// Token: 0x17000224 RID: 548
	// (get) Token: 0x060009E7 RID: 2535 RVA: 0x0003B5E7 File Offset: 0x000397E7
	public int mapPosY
	{
		get
		{
			return this.mstData.mapPosY;
		}
	}

	// Token: 0x17000225 RID: 549
	// (get) Token: 0x060009E8 RID: 2536 RVA: 0x0003B5F4 File Offset: 0x000397F4
	public string mapObjName
	{
		get
		{
			return this.mstData.mapObjName;
		}
	}

	// Token: 0x17000226 RID: 550
	// (get) Token: 0x060009E9 RID: 2537 RVA: 0x0003B601 File Offset: 0x00039801
	public QuestWeather WeatherType
	{
		get
		{
			return (QuestWeather)this.mstData.weatherType;
		}
	}

	// Token: 0x17000227 RID: 551
	// (get) Token: 0x060009EA RID: 2538 RVA: 0x0003B60E File Offset: 0x0003980E
	public string ReleaseDateFreeText
	{
		get
		{
			return this.mstData.freeword;
		}
	}

	// Token: 0x17000228 RID: 552
	// (get) Token: 0x060009EB RID: 2539 RVA: 0x0003B61B File Offset: 0x0003981B
	public int RaidTargetCharaId
	{
		get
		{
			return this.mstData.raidTargetCharaId;
		}
	}

	// Token: 0x17000229 RID: 553
	// (get) Token: 0x060009EC RID: 2540 RVA: 0x0003B628 File Offset: 0x00039828
	// (set) Token: 0x060009ED RID: 2541 RVA: 0x0003B630 File Offset: 0x00039830
	public List<QuestStaticQuestGroup> questGroupList { get; set; }

	// Token: 0x1700022A RID: 554
	// (get) Token: 0x060009EE RID: 2542 RVA: 0x0003B639 File Offset: 0x00039839
	// (set) Token: 0x060009EF RID: 2543 RVA: 0x0003B641 File Offset: 0x00039841
	public DateTime HighEndTimeByGroup { get; set; }

	// Token: 0x1700022B RID: 555
	// (get) Token: 0x060009F0 RID: 2544 RVA: 0x0003B64A File Offset: 0x0003984A
	// (set) Token: 0x060009F1 RID: 2545 RVA: 0x0003B652 File Offset: 0x00039852
	public DateTime LowStartTimeByGroup { get; set; }

	// Token: 0x1700022C RID: 556
	// (get) Token: 0x060009F2 RID: 2546 RVA: 0x0003B65C File Offset: 0x0003985C
	public bool IsInfiniteEndTime
	{
		get
		{
			return this.HighEndTimeByGroup.Year > TimeManager.Now.Year + 2;
		}
	}

	// Token: 0x1700022D RID: 557
	// (get) Token: 0x060009F3 RID: 2547 RVA: 0x0003B688 File Offset: 0x00039888
	// (set) Token: 0x060009F4 RID: 2548 RVA: 0x0003B690 File Offset: 0x00039890
	public List<int> dispItemIconId { get; private set; }

	// Token: 0x1700022E RID: 558
	// (get) Token: 0x060009F5 RID: 2549 RVA: 0x0003B699 File Offset: 0x00039899
	public QuestStaticMap.MapCategory QuestMapCategory
	{
		get
		{
			return (QuestStaticMap.MapCategory)this.mstData.questMapCategory;
		}
	}

	// Token: 0x1700022F RID: 559
	// (get) Token: 0x060009F6 RID: 2550 RVA: 0x0003B6A6 File Offset: 0x000398A6
	public DateTime StartDateTime
	{
		get
		{
			return new DateTime(PrjUtil.ConvertTimeToTicks(this.mstData.startDatetime));
		}
	}

	// Token: 0x17000230 RID: 560
	// (get) Token: 0x060009F7 RID: 2551 RVA: 0x0003B6BD File Offset: 0x000398BD
	public bool StartHideFlag
	{
		get
		{
			return this.mstData.startHideFlag != 0;
		}
	}

	// Token: 0x17000231 RID: 561
	// (get) Token: 0x060009F8 RID: 2552 RVA: 0x0003B6CD File Offset: 0x000398CD
	public bool isWeekQuest
	{
		get
		{
			return this.mstData.questRelWeek != 0;
		}
	}

	// Token: 0x17000232 RID: 562
	// (get) Token: 0x060009F9 RID: 2553 RVA: 0x0003B6DD File Offset: 0x000398DD
	public int questCharaId
	{
		get
		{
			return this.questGroupList[0].targetCharaId;
		}
	}

	// Token: 0x17000233 RID: 563
	// (get) Token: 0x060009FA RID: 2554 RVA: 0x0003B6F0 File Offset: 0x000398F0
	public List<DataManagerQuest.DrawItemData> DrawItemIdList
	{
		get
		{
			List<DataManagerQuest.DrawItemData> list = this._drawItemIdList.FindAll((DataManagerQuest.DrawItemData x) => x.StartDateTime <= TimeManager.Now && TimeManager.Now < x.EndDateTime);
			if (list.Count == 0 && DataManager.DmQuest.QuestStaticData.chapterDataMap.ContainsKey(this.chapterId) && 0 < DataManager.DmQuest.QuestStaticData.chapterDataMap[this.chapterId].DrawItemIdList.Count)
			{
				return DataManager.DmQuest.QuestStaticData.chapterDataMap[this.chapterId].DrawItemIdList;
			}
			return list;
		}
	}

	// Token: 0x17000234 RID: 564
	// (get) Token: 0x060009FB RID: 2555 RVA: 0x0003B794 File Offset: 0x00039994
	public List<DataManagerPhoto.PhotoDropItemData> PhotoDropItemList
	{
		get
		{
			List<DataManagerPhoto.PhotoDropItemData> list = this._photoDropItemList.FindAll((DataManagerPhoto.PhotoDropItemData x) => x.StartDateTime <= TimeManager.Now && TimeManager.Now < x.EndDateTime);
			if (list.Count == 0 && DataManager.DmQuest.QuestStaticData.chapterDataMap.ContainsKey(this.chapterId) && 0 < DataManager.DmQuest.QuestStaticData.chapterDataMap[this.chapterId].PhotoDropItemList.Count)
			{
				return DataManager.DmQuest.QuestStaticData.chapterDataMap[this.chapterId].PhotoDropItemList;
			}
			return list;
		}
	}

	// Token: 0x17000235 RID: 565
	// (get) Token: 0x060009FC RID: 2556 RVA: 0x0003B838 File Offset: 0x00039A38
	public QuestStaticMap.LargeEventUIData largeEventUIData
	{
		get
		{
			if (this._largeEventUIData == null)
			{
				this._largeEventUIData = new QuestStaticMap.LargeEventUIData();
				foreach (QuestStaticQuestGroup questStaticQuestGroup in this.questGroupList)
				{
					if (this._largeEventUIData.openItemInfoOneData == null)
					{
						this._largeEventUIData.openItemInfoOneData = questStaticQuestGroup.questOneList.Find((QuestStaticQuestOne item) => item.OpenKeyItem != null);
					}
					if (this._largeEventUIData.pickupRewardOneData == null)
					{
						this._largeEventUIData.pickupRewardOneData = questStaticQuestGroup.questOneList.Find((QuestStaticQuestOne item) => item.RewardItemList.Count > 0);
					}
				}
			}
			return this._largeEventUIData;
		}
	}

	// Token: 0x060009FD RID: 2557 RVA: 0x0003B92C File Offset: 0x00039B2C
	public QuestStaticMap(MstQuestMapData m, List<DataManagerQuest.DrawItemData> drawItemDataList, List<DataManagerPhoto.PhotoDropItemData> mstPhotoDropItemDataList)
	{
		this.mstData = m;
		this._drawItemIdList = drawItemDataList;
		this._photoDropItemList = mstPhotoDropItemDataList.FindAll((DataManagerPhoto.PhotoDropItemData x) => x.TargetId == m.mapId);
		this.dispItemIconId = new List<int> { m.dispItemIconId00, m.dispItemIconId01, m.dispItemIconId02, m.dispItemIconId03, m.dispItemIconId04, m.dispItemIconId05 };
		this.dispItemIconId.RemoveAll((int item) => item == 0);
	}

	// Token: 0x04000819 RID: 2073
	private MstQuestMapData mstData;

	// Token: 0x0400081E RID: 2078
	public List<DataManagerQuest.DrawItemTermData> drawItemTermDataList;

	// Token: 0x0400081F RID: 2079
	private List<DataManagerQuest.DrawItemData> _drawItemIdList;

	// Token: 0x04000820 RID: 2080
	private List<DataManagerPhoto.PhotoDropItemData> _photoDropItemList;

	// Token: 0x04000821 RID: 2081
	private QuestStaticMap.LargeEventUIData _largeEventUIData;

	// Token: 0x020007D4 RID: 2004
	public class LargeEventUIData
	{
		// Token: 0x040034E8 RID: 13544
		public QuestStaticQuestOne openItemInfoOneData;

		// Token: 0x040034E9 RID: 13545
		public QuestStaticQuestOne pickupRewardOneData;
	}

	// Token: 0x020007D5 RID: 2005
	public enum MapCategory
	{
		// Token: 0x040034EB RID: 13547
		Default,
		// Token: 0x040034EC RID: 13548
		CoopPoint = 101,
		// Token: 0x040034ED RID: 13549
		CoopBonus
	}
}
