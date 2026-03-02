using System;
using System.Collections.Generic;
using SGNFW.Mst;

// Token: 0x020000E2 RID: 226
public class QuestStaticChapter
{
	// Token: 0x17000211 RID: 529
	// (get) Token: 0x060009D1 RID: 2513 RVA: 0x0003B42E File Offset: 0x0003962E
	public int chapterId
	{
		get
		{
			return this.mstData.chapterId;
		}
	}

	// Token: 0x17000212 RID: 530
	// (get) Token: 0x060009D2 RID: 2514 RVA: 0x0003B43B File Offset: 0x0003963B
	public string chapterName
	{
		get
		{
			return this.mstData.chapterName;
		}
	}

	// Token: 0x17000213 RID: 531
	// (get) Token: 0x060009D3 RID: 2515 RVA: 0x0003B448 File Offset: 0x00039648
	public string chapterTitle
	{
		get
		{
			return this.mstData.chapterTitle;
		}
	}

	// Token: 0x17000214 RID: 532
	// (get) Token: 0x060009D4 RID: 2516 RVA: 0x0003B455 File Offset: 0x00039655
	public string mapPath
	{
		get
		{
			return this.mstData.mapPath;
		}
	}

	// Token: 0x17000215 RID: 533
	// (get) Token: 0x060009D5 RID: 2517 RVA: 0x0003B462 File Offset: 0x00039662
	public QuestStaticChapter.Category category
	{
		get
		{
			return (QuestStaticChapter.Category)this.mstData.category;
		}
	}

	// Token: 0x17000216 RID: 534
	// (get) Token: 0x060009D6 RID: 2518 RVA: 0x0003B46F File Offset: 0x0003966F
	public int chapterNumber
	{
		get
		{
			return this.mstData.chapterId;
		}
	}

	// Token: 0x17000217 RID: 535
	// (get) Token: 0x060009D7 RID: 2519 RVA: 0x0003B47C File Offset: 0x0003967C
	public string topCharaComment
	{
		get
		{
			return this.mstData.topCharaComment;
		}
	}

	// Token: 0x17000218 RID: 536
	// (get) Token: 0x060009D8 RID: 2520 RVA: 0x0003B48C File Offset: 0x0003968C
	public string topBoardImagePath
	{
		get
		{
			return "Texture2D/QuestTopPhoto/questtop_photo_" + this.chapterId.ToString("D4");
		}
	}

	// Token: 0x17000219 RID: 537
	// (get) Token: 0x060009D9 RID: 2521 RVA: 0x0003B4B6 File Offset: 0x000396B6
	public int hardChapterId
	{
		get
		{
			return this.mstData.hardChapterId;
		}
	}

	// Token: 0x1700021A RID: 538
	// (get) Token: 0x060009DA RID: 2522 RVA: 0x0003B4C3 File Offset: 0x000396C3
	public int MarkerType
	{
		get
		{
			return this.mstData.markerType;
		}
	}

	// Token: 0x1700021B RID: 539
	// (get) Token: 0x060009DB RID: 2523 RVA: 0x0003B4D0 File Offset: 0x000396D0
	public QuestStaticChapter.ChapterEndType EndType
	{
		get
		{
			return (QuestStaticChapter.ChapterEndType)this.mstData.endType;
		}
	}

	// Token: 0x1700021C RID: 540
	// (get) Token: 0x060009DC RID: 2524 RVA: 0x0003B4DD File Offset: 0x000396DD
	public QuestWeather WeatherType
	{
		get
		{
			return (QuestWeather)this.mstData.weatherType;
		}
	}

	// Token: 0x1700021D RID: 541
	// (get) Token: 0x060009DD RID: 2525 RVA: 0x0003B4EA File Offset: 0x000396EA
	// (set) Token: 0x060009DE RID: 2526 RVA: 0x0003B4F2 File Offset: 0x000396F2
	public List<QuestStaticMap> mapDataList { get; set; }

	// Token: 0x1700021E RID: 542
	// (get) Token: 0x060009DF RID: 2527 RVA: 0x0003B4FB File Offset: 0x000396FB
	public List<DataManagerQuest.DrawItemData> DrawItemIdList
	{
		get
		{
			return this._drawItemIdList.FindAll((DataManagerQuest.DrawItemData x) => x.StartDateTime <= TimeManager.Now && TimeManager.Now < x.EndDateTime);
		}
	}

	// Token: 0x1700021F RID: 543
	// (get) Token: 0x060009E0 RID: 2528 RVA: 0x0003B527 File Offset: 0x00039727
	public List<DataManagerPhoto.PhotoDropItemData> PhotoDropItemList
	{
		get
		{
			return this._photoDropItemList.FindAll((DataManagerPhoto.PhotoDropItemData x) => x.StartDateTime <= TimeManager.Now && TimeManager.Now < x.EndDateTime);
		}
	}

	// Token: 0x060009E1 RID: 2529 RVA: 0x0003B553 File Offset: 0x00039753
	public QuestStaticChapter(MstQuestChapterData m)
		: this(m, new List<DataManagerQuest.DrawItemData>(), new List<DataManagerPhoto.PhotoDropItemData>())
	{
	}

	// Token: 0x060009E2 RID: 2530 RVA: 0x0003B568 File Offset: 0x00039768
	public QuestStaticChapter(MstQuestChapterData m, List<DataManagerQuest.DrawItemData> drawItemDataList, List<DataManagerPhoto.PhotoDropItemData> mstPhotoDropItemDataList)
	{
		this.mstData = m;
		this._drawItemIdList = drawItemDataList;
		this._photoDropItemList = mstPhotoDropItemDataList.FindAll((DataManagerPhoto.PhotoDropItemData x) => x.TargetId == m.chapterId);
	}

	// Token: 0x04000815 RID: 2069
	public List<DataManagerQuest.DrawItemTermData> drawItemTermDataList;

	// Token: 0x04000816 RID: 2070
	private List<DataManagerQuest.DrawItemData> _drawItemIdList;

	// Token: 0x04000817 RID: 2071
	private List<DataManagerPhoto.PhotoDropItemData> _photoDropItemList;

	// Token: 0x04000818 RID: 2072
	private MstQuestChapterData mstData;

	// Token: 0x020007D0 RID: 2000
	public enum ChapterEndType
	{
		// Token: 0x040034CE RID: 13518
		Default,
		// Token: 0x040034CF RID: 13519
		araiDiaryEnd,
		// Token: 0x040034D0 RID: 13520
		mainStoryEnd,
		// Token: 0x040034D1 RID: 13521
		cellvalEnd,
		// Token: 0x040034D2 RID: 13522
		mainStory2End,
		// Token: 0x040034D3 RID: 13523
		mainStory3End
	}

	// Token: 0x020007D1 RID: 2001
	public enum Category
	{
		// Token: 0x040034D5 RID: 13525
		INVALID,
		// Token: 0x040034D6 RID: 13526
		STORY,
		// Token: 0x040034D7 RID: 13527
		GROW,
		// Token: 0x040034D8 RID: 13528
		CHARA,
		// Token: 0x040034D9 RID: 13529
		PVP,
		// Token: 0x040034DA RID: 13530
		EVENT,
		// Token: 0x040034DB RID: 13531
		SIDE_STORY,
		// Token: 0x040034DC RID: 13532
		TUTORIAL,
		// Token: 0x040034DD RID: 13533
		TRAINING,
		// Token: 0x040034DE RID: 13534
		CELLVAL,
		// Token: 0x040034DF RID: 13535
		SCENARIO_SP_PVP,
		// Token: 0x040034E0 RID: 13536
		ETCETERA,
		// Token: 0x040034E1 RID: 13537
		STORY2,
		// Token: 0x040034E2 RID: 13538
		STORY3,
		// Token: 0x040034E3 RID: 13539
		ASSISTANT
	}
}
