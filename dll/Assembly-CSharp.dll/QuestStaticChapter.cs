using System;
using System.Collections.Generic;
using SGNFW.Mst;

public class QuestStaticChapter
{
	public int chapterId
	{
		get
		{
			return this.mstData.chapterId;
		}
	}

	public string chapterName
	{
		get
		{
			return this.mstData.chapterName;
		}
	}

	public string chapterTitle
	{
		get
		{
			return this.mstData.chapterTitle;
		}
	}

	public string mapPath
	{
		get
		{
			return this.mstData.mapPath;
		}
	}

	public QuestStaticChapter.Category category
	{
		get
		{
			return (QuestStaticChapter.Category)this.mstData.category;
		}
	}

	public int chapterNumber
	{
		get
		{
			return this.mstData.chapterId;
		}
	}

	public string topCharaComment
	{
		get
		{
			return this.mstData.topCharaComment;
		}
	}

	public string topBoardImagePath
	{
		get
		{
			return "Texture2D/QuestTopPhoto/questtop_photo_" + this.chapterId.ToString("D4");
		}
	}

	public int hardChapterId
	{
		get
		{
			return this.mstData.hardChapterId;
		}
	}

	public int MarkerType
	{
		get
		{
			return this.mstData.markerType;
		}
	}

	public QuestStaticChapter.ChapterEndType EndType
	{
		get
		{
			return (QuestStaticChapter.ChapterEndType)this.mstData.endType;
		}
	}

	public QuestWeather WeatherType
	{
		get
		{
			return (QuestWeather)this.mstData.weatherType;
		}
	}

	public List<QuestStaticMap> mapDataList { get; set; }

	public List<DataManagerQuest.DrawItemData> DrawItemIdList
	{
		get
		{
			return this._drawItemIdList.FindAll((DataManagerQuest.DrawItemData x) => x.StartDateTime <= TimeManager.Now && TimeManager.Now < x.EndDateTime);
		}
	}

	public List<DataManagerPhoto.PhotoDropItemData> PhotoDropItemList
	{
		get
		{
			return this._photoDropItemList.FindAll((DataManagerPhoto.PhotoDropItemData x) => x.StartDateTime <= TimeManager.Now && TimeManager.Now < x.EndDateTime);
		}
	}

	public QuestStaticChapter(MstQuestChapterData m)
		: this(m, new List<DataManagerQuest.DrawItemData>(), new List<DataManagerPhoto.PhotoDropItemData>())
	{
	}

	public QuestStaticChapter(MstQuestChapterData m, List<DataManagerQuest.DrawItemData> drawItemDataList, List<DataManagerPhoto.PhotoDropItemData> mstPhotoDropItemDataList)
	{
		this.mstData = m;
		this._drawItemIdList = drawItemDataList;
		this._photoDropItemList = mstPhotoDropItemDataList.FindAll((DataManagerPhoto.PhotoDropItemData x) => x.TargetId == m.chapterId);
	}

	public List<DataManagerQuest.DrawItemTermData> drawItemTermDataList;

	private List<DataManagerQuest.DrawItemData> _drawItemIdList;

	private List<DataManagerPhoto.PhotoDropItemData> _photoDropItemList;

	private MstQuestChapterData mstData;

	public enum ChapterEndType
	{
		Default,
		araiDiaryEnd,
		mainStoryEnd,
		cellvalEnd,
		mainStory2End,
		mainStory3End
	}

	public enum Category
	{
		INVALID,
		STORY,
		GROW,
		CHARA,
		PVP,
		EVENT,
		SIDE_STORY,
		TUTORIAL,
		TRAINING,
		CELLVAL,
		SCENARIO_SP_PVP,
		ETCETERA,
		STORY2,
		STORY3,
		ASSISTANT
	}
}
