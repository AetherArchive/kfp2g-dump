using System;
using SGNFW.Mst;

public class HomeBannerData
{
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

	public int bannerId;

	public string bannerImagePath;

	public string bannerImagePathByQuestTop;

	public string bannerImagePathEvent;

	public string bannerText;

	public HomeBannerData.ActionType actionType;

	public string actionParamURL;

	public SceneManager.SceneName actionParamScene;

	public int actionParamID;

	public int priority;

	public DateTime startTime;

	public DateTime endTime;

	public enum ActionType
	{
		INVALID,
		MOVE_GAME_SCENE,
		OPEN_WEBVIEW,
		OPEN_BROWSER,
		NOTICE,
		NOAH_OFFER,
		ATOM_INVITE,
		MOVE_MOVIE,
		MOVE_PVP_SPECIAL,
		FRIEND_INVITE,
		EXCHANGE_PLAYER
	}
}
