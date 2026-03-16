using System;
using System.Collections.Generic;
using SGNFW.Mst;

public class QuestStaticQuestGroup
{
	public int questGroupId
	{
		get
		{
			return this.mstData.questGroupId;
		}
	}

	public int mapId
	{
		get
		{
			return this.mstData.mapId;
		}
	}

	public string titleName
	{
		get
		{
			return this.mstData.titleName;
		}
	}

	public string storyName
	{
		get
		{
			return this.mstData.storyName;
		}
	}

	public int titleCategory
	{
		get
		{
			return this.mstData.titleCategory;
		}
	}

	public int dispPriority
	{
		get
		{
			return this.mstData.dispPriority;
		}
	}

	public DataManagerEvent.CoopType CoopType
	{
		get
		{
			return (DataManagerEvent.CoopType)this.mstData.questGroupCategory;
		}
	}

	public QuestStaticQuestGroup.GroupCategory QuestGroupCategory
	{
		get
		{
			return (QuestStaticQuestGroup.GroupCategory)this.mstData.questGroupCategory;
		}
	}

	public List<QuestStaticQuestGroup.Chara> CharaList { get; private set; }

	public string charaComment
	{
		get
		{
			return this.mstData.dispCharaComment;
		}
	}

	public int charaId
	{
		get
		{
			return this.mstData.dispCharaId;
		}
	}

	public string charaBodyMotionId
	{
		get
		{
			return this.mstData.dispCharaBodyMotion;
		}
	}

	public string charaFaceMotionId
	{
		get
		{
			return this.mstData.dispCharaFaceMotion;
		}
	}

	public int targetCharaId
	{
		get
		{
			return this.mstData.relCharaId;
		}
	}

	public DateTime startTime
	{
		get
		{
			return new DateTime(PrjUtil.ConvertTimeToTicks(this.mstData.startDatetime));
		}
	}

	public DateTime endTime
	{
		get
		{
			return new DateTime(PrjUtil.ConvertTimeToTicks(this.mstData.endDatetime));
		}
	}

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

	public DateTime startDatetime
	{
		get
		{
			return new DateTime(PrjUtil.ConvertTimeToTicks(this.mstData.startDatetime));
		}
	}

	public DateTime endDatetime
	{
		get
		{
			return new DateTime(PrjUtil.ConvertTimeToTicks(this.mstData.endDatetime));
		}
	}

	public bool dispLimitTime
	{
		get
		{
			return this.mstData.timeDispFlg != 0;
		}
	}

	public DateTime limitTime
	{
		get
		{
			return new DateTime(PrjUtil.ConvertTimeToTicks(this.mstData.endDatetime));
		}
	}

	public List<QuestStaticQuestOne> questOneList { get; set; }

	public int limitClearNum
	{
		get
		{
			return this.mstData.limitClearNum;
		}
	}

	public int autoModeEnable
	{
		get
		{
			return this.mstData.autoModeEnable;
		}
	}

	public int growthEventId
	{
		get
		{
			return this.mstData.growthEventId;
		}
	}

	public List<DataManagerQuest.DrawItemTermData> drawItemTermDataList { get; set; }

	private List<DataManagerQuest.DrawItemData> _drawItemIdList { get; set; }

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

	public bool HideUnreleasedBelt
	{
		get
		{
			return this.mstData.hideFlag == 1;
		}
	}

	public bool limitGroupFlag
	{
		get
		{
			return this.mstData.limitGroupFlag == 1;
		}
	}

	public QuestUtil.SkipType SkippableFlag
	{
		get
		{
			return (QuestUtil.SkipType)this.mstData.skippableFlag;
		}
	}

	public int LimitSkipNum
	{
		get
		{
			return this.mstData.limitSkipNum;
		}
	}

	public int LimitSkipRecoveryNum
	{
		get
		{
			return this.mstData.limitSkipRecoveryNum;
		}
	}

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

	public QuestStaticQuestGroup(MstQuestQuestgroupData m)
		: this(m, new List<DataManagerQuest.DrawItemData>(), new List<DataManagerPhoto.PhotoDropItemData>())
	{
	}

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

	private MstQuestQuestgroupData mstData;

	private List<DataManagerPhoto.PhotoDropItemData> _photoDropItemList;

	public class Chara
	{
		public int DispNum { get; private set; }

		public int Id { get; private set; }

		public string BodyMotion { get; private set; }

		public string FaceMotion { get; private set; }

		public Chara(int num, int id, string body, string face)
		{
			this.DispNum = num;
			this.Id = id;
			this.BodyMotion = body;
			this.FaceMotion = face;
		}
	}

	public enum GroupCategory
	{
		Default,
		CoopNormal = 101,
		CoopDifficult,
		CoopBonus
	}
}
