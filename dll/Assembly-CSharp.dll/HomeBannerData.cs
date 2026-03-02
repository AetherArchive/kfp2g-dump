using System;
using SGNFW.Mst;

// Token: 0x02000086 RID: 134
public class HomeBannerData
{
	// Token: 0x17000100 RID: 256
	// (get) Token: 0x0600051C RID: 1308 RVA: 0x000236F8 File Offset: 0x000218F8
	public int eventId
	{
		get
		{
			int num = 0;
			DataManagerEvent.EventData ev = DataManager.DmEvent.GetEventDataList().Find((DataManagerEvent.EventData x) => x.eventBannerId == this.bannerId);
			if (ev != null && DataManager.DmQuest.QuestStaticData != null)
			{
				QuestStaticChapter questStaticChapter = DataManager.DmQuest.QuestStaticData.chapterDataList.Find((QuestStaticChapter x) => x.chapterId == ev.eventChapterId);
				int questGroupId = questStaticChapter.mapDataList[0].questGroupList[0].questGroupId;
				QuestStaticQuestGroup questStaticQuestGroup = DataManager.DmQuest.QuestStaticData.groupDataList.Find((QuestStaticQuestGroup x) => x.questGroupId == questGroupId);
				if (questStaticQuestGroup.startDatetime <= TimeManager.Now && TimeManager.Now <= questStaticQuestGroup.endDatetime)
				{
					num = ev.eventId;
				}
			}
			return num;
		}
	}

	// Token: 0x0600051D RID: 1309 RVA: 0x000237F0 File Offset: 0x000219F0
	public HomeBannerData(MstBannerData mst)
	{
		this.bannerId = mst.id;
		this.bannerImagePath = "Texture2D/HomeBanner/home_banner_" + mst.bannerName;
		this.bannerImagePathByQuestTop = "Texture2D/QuestTop/questtop_banner_" + mst.bannerName;
		this.bannerImagePathEvent = "Texture2D/EventTop/eventtop_banner_" + mst.bannerName;
		this.bannerText = mst.bannerText;
		this.actionType = (HomeBannerData.ActionType)mst.linkType;
		this.actionParamURL = mst.linkAdress;
		this.actionParamID = mst.linkValue;
		this.priority = mst.priority;
		PrjUtil.EnumTryParse<SceneManager.SceneName>(mst.linkAdress, true, out this.actionParamScene);
		this.startTime = new DateTime(PrjUtil.ConvertTimeToTicks(mst.startTime));
		this.endTime = new DateTime(PrjUtil.ConvertTimeToTicks(mst.endTime));
		if (this.actionType == HomeBannerData.ActionType.MOVE_MOVIE)
		{
			this.bannerImagePath = mst.bannerName;
		}
	}

	// Token: 0x04000563 RID: 1379
	public int bannerId;

	// Token: 0x04000564 RID: 1380
	public string bannerImagePath;

	// Token: 0x04000565 RID: 1381
	public string bannerImagePathByQuestTop;

	// Token: 0x04000566 RID: 1382
	public string bannerImagePathEvent;

	// Token: 0x04000567 RID: 1383
	public string bannerText;

	// Token: 0x04000568 RID: 1384
	public HomeBannerData.ActionType actionType;

	// Token: 0x04000569 RID: 1385
	public string actionParamURL;

	// Token: 0x0400056A RID: 1386
	public SceneManager.SceneName actionParamScene;

	// Token: 0x0400056B RID: 1387
	public int actionParamID;

	// Token: 0x0400056C RID: 1388
	public int priority;

	// Token: 0x0400056D RID: 1389
	public DateTime startTime;

	// Token: 0x0400056E RID: 1390
	public DateTime endTime;

	// Token: 0x020006C9 RID: 1737
	public enum ActionType
	{
		// Token: 0x04003088 RID: 12424
		INVALID,
		// Token: 0x04003089 RID: 12425
		MOVE_GAME_SCENE,
		// Token: 0x0400308A RID: 12426
		OPEN_WEBVIEW,
		// Token: 0x0400308B RID: 12427
		OPEN_BROWSER,
		// Token: 0x0400308C RID: 12428
		NOTICE,
		// Token: 0x0400308D RID: 12429
		NOAH_OFFER,
		// Token: 0x0400308E RID: 12430
		ATOM_INVITE,
		// Token: 0x0400308F RID: 12431
		MOVE_MOVIE,
		// Token: 0x04003090 RID: 12432
		MOVE_PVP_SPECIAL,
		// Token: 0x04003091 RID: 12433
		FRIEND_INVITE,
		// Token: 0x04003092 RID: 12434
		EXCHANGE_PLAYER
	}
}
