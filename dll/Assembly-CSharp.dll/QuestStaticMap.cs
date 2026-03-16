using System;
using System.Collections.Generic;
using SGNFW.Mst;

public class QuestStaticMap
{
	public int mapId
	{
		get
		{
			return this.mstData.mapId;
		}
	}

	public int chapterId
	{
		get
		{
			return this.mstData.chapterId;
		}
	}

	public string mapName
	{
		get
		{
			return this.mstData.mapName;
		}
	}

	public int mapPosX
	{
		get
		{
			return this.mstData.mapPosX;
		}
	}

	public int mapPosY
	{
		get
		{
			return this.mstData.mapPosY;
		}
	}

	public string mapObjName
	{
		get
		{
			return this.mstData.mapObjName;
		}
	}

	public QuestWeather WeatherType
	{
		get
		{
			return (QuestWeather)this.mstData.weatherType;
		}
	}

	public string ReleaseDateFreeText
	{
		get
		{
			return this.mstData.freeword;
		}
	}

	public int RaidTargetCharaId
	{
		get
		{
			return this.mstData.raidTargetCharaId;
		}
	}

	public List<QuestStaticQuestGroup> questGroupList { get; set; }

	public DateTime HighEndTimeByGroup { get; set; }

	public DateTime LowStartTimeByGroup { get; set; }

	public bool IsInfiniteEndTime
	{
		get
		{
			return this.HighEndTimeByGroup.Year > TimeManager.Now.Year + 2;
		}
	}

	public List<int> dispItemIconId { get; private set; }

	public QuestStaticMap.MapCategory QuestMapCategory
	{
		get
		{
			return (QuestStaticMap.MapCategory)this.mstData.questMapCategory;
		}
	}

	public DateTime StartDateTime
	{
		get
		{
			return new DateTime(PrjUtil.ConvertTimeToTicks(this.mstData.startDatetime));
		}
	}

	public bool StartHideFlag
	{
		get
		{
			return this.mstData.startHideFlag != 0;
		}
	}

	public bool isWeekQuest
	{
		get
		{
			return this.mstData.questRelWeek != 0;
		}
	}

	public int questCharaId
	{
		get
		{
			return this.questGroupList[0].targetCharaId;
		}
	}

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

	public QuestStaticMap(MstQuestMapData m, List<DataManagerQuest.DrawItemData> drawItemDataList, List<DataManagerPhoto.PhotoDropItemData> mstPhotoDropItemDataList)
	{
		this.mstData = m;
		this._drawItemIdList = drawItemDataList;
		this._photoDropItemList = mstPhotoDropItemDataList.FindAll((DataManagerPhoto.PhotoDropItemData x) => x.TargetId == m.mapId);
		this.dispItemIconId = new List<int> { m.dispItemIconId00, m.dispItemIconId01, m.dispItemIconId02, m.dispItemIconId03, m.dispItemIconId04, m.dispItemIconId05 };
		this.dispItemIconId.RemoveAll((int item) => item == 0);
	}

	private MstQuestMapData mstData;

	public List<DataManagerQuest.DrawItemTermData> drawItemTermDataList;

	private List<DataManagerQuest.DrawItemData> _drawItemIdList;

	private List<DataManagerPhoto.PhotoDropItemData> _photoDropItemList;

	private QuestStaticMap.LargeEventUIData _largeEventUIData;

	public class LargeEventUIData
	{
		public QuestStaticQuestOne openItemInfoOneData;

		public QuestStaticQuestOne pickupRewardOneData;
	}

	public enum MapCategory
	{
		Default,
		CoopPoint = 101,
		CoopBonus
	}
}
