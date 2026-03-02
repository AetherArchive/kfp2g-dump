using System;
using UnityEngine;

// Token: 0x0200006F RID: 111
public class DataManagerBookmark
{
	// Token: 0x06000324 RID: 804 RVA: 0x00017D26 File Offset: 0x00015F26
	public DataManagerBookmark(DataManager p)
	{
		this.parentData = p;
	}

	// Token: 0x06000325 RID: 805 RVA: 0x00017D38 File Offset: 0x00015F38
	public void UpdateBookmark(int newBookmark, int currentCharaId, int currentChapterId, int currentCategoryId, SceneStoryView.MODE currentCategory, string chapterName)
	{
		string text = string.Empty;
		string text2 = string.Empty;
		string text3 = string.Empty;
		string text4 = string.Empty;
		if (currentCategory != SceneStoryView.MODE.ANY2CATEGORY)
		{
			switch (currentCategory)
			{
			case SceneStoryView.MODE.FRIENDS:
				text = "bookmarkCharaScenario";
				PlayerPrefs.SetInt("bookmarkCharaId", currentCharaId);
				PlayerPrefs.SetInt(text, newBookmark);
				PlayerPrefs.Save();
				break;
			case SceneStoryView.MODE.EVENT:
				text = "bookmarkEvent";
				text2 = "bookmarkEventChapter";
				text4 = "bookmarkEventChapterName";
				break;
			case SceneStoryView.MODE.ARAI:
				text = "bookmarkArai";
				text2 = "bookmarkAraiChapter";
				text4 = "bookmarkAraiChapterName";
				break;
			case SceneStoryView.MODE.PVP:
				text = "bookmarkOther";
				text2 = "bookmarkOtherChapter";
				text3 = "bookmarkOtherCategory";
				text4 = "bookmarkOtherCategoryName";
				break;
			}
		}
		else
		{
			text = "bookmarkMain";
			text2 = "bookmarkMainChapter";
			text3 = "bookmarkMainCategory";
			text4 = "bookmarkMainCategoryName";
		}
		if (currentCategory == SceneStoryView.MODE.FRIENDS)
		{
			return;
		}
		PlayerPrefs.SetInt(text, newBookmark);
		PlayerPrefs.SetInt(text2, currentChapterId);
		PlayerPrefs.SetString(text4, chapterName);
		if (currentCategory == SceneStoryView.MODE.ANY2CATEGORY || currentCategory == SceneStoryView.MODE.PVP)
		{
			PlayerPrefs.SetInt(text3, currentCategoryId);
		}
		PlayerPrefs.Save();
	}

	// Token: 0x06000326 RID: 806 RVA: 0x00017E2C File Offset: 0x0001602C
	public int GetCurrentBookmarkCharaid()
	{
		return PlayerPrefs.GetInt("bookmarkCharaId");
	}

	// Token: 0x06000327 RID: 807 RVA: 0x00017E38 File Offset: 0x00016038
	public int GetCurrentBookmark(SceneStoryView.MODE currentCategory)
	{
		string text = string.Empty;
		if (currentCategory != SceneStoryView.MODE.ANY2CATEGORY)
		{
			switch (currentCategory)
			{
			case SceneStoryView.MODE.FRIENDS:
				text = "bookmarkCharaScenario";
				break;
			case SceneStoryView.MODE.EVENT:
				text = "bookmarkEvent";
				break;
			case SceneStoryView.MODE.ARAI:
				text = "bookmarkArai";
				break;
			case SceneStoryView.MODE.PVP:
				text = "bookmarkOther";
				break;
			}
		}
		else
		{
			text = "bookmarkMain";
		}
		return PlayerPrefs.GetInt(text);
	}

	// Token: 0x06000328 RID: 808 RVA: 0x00017E98 File Offset: 0x00016098
	public int GetCurrentBookmarkChapter(SceneStoryView.MODE currentCategory)
	{
		string text = string.Empty;
		if (currentCategory != SceneStoryView.MODE.ANY2CATEGORY)
		{
			switch (currentCategory)
			{
			case SceneStoryView.MODE.EVENT:
				text = "bookmarkEventChapter";
				break;
			case SceneStoryView.MODE.ARAI:
				text = "bookmarkAraiChapter";
				break;
			case SceneStoryView.MODE.PVP:
				text = "bookmarkOtherChapter";
				break;
			}
		}
		else
		{
			text = "bookmarkMainChapter";
		}
		return PlayerPrefs.GetInt(text);
	}

	// Token: 0x06000329 RID: 809 RVA: 0x00017EEC File Offset: 0x000160EC
	public int GetCurrentBookmarkCategory(SceneStoryView.MODE currentCategory)
	{
		string text = string.Empty;
		if (currentCategory != SceneStoryView.MODE.ANY2CATEGORY)
		{
			if (currentCategory == SceneStoryView.MODE.PVP)
			{
				text = "bookmarkOtherCategory";
			}
		}
		else
		{
			text = "bookmarkMainCategory";
		}
		return PlayerPrefs.GetInt(text);
	}

	// Token: 0x0600032A RID: 810 RVA: 0x00017F20 File Offset: 0x00016120
	public string GetCurrentChapterName(SceneStoryView.MODE currentCategory)
	{
		string text = string.Empty;
		if (currentCategory != SceneStoryView.MODE.ANY2CATEGORY)
		{
			switch (currentCategory)
			{
			case SceneStoryView.MODE.EVENT:
				text = "bookmarkEventChapterName";
				break;
			case SceneStoryView.MODE.ARAI:
				text = "bookmarkAraiChapterName";
				break;
			case SceneStoryView.MODE.PVP:
				text = "bookmarkOtherCategoryName";
				break;
			}
		}
		else
		{
			text = "bookmarkMainCategoryName";
		}
		return PlayerPrefs.GetString(text);
	}

	// Token: 0x040004A1 RID: 1185
	private DataManager parentData;
}
