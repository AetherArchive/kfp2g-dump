using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.HttpRequest.Protocol;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000173 RID: 371
public class SelEventCoopCtrl : MonoBehaviour
{
	// Token: 0x1700039F RID: 927
	// (get) Token: 0x0600174B RID: 5963 RVA: 0x001234DE File Offset: 0x001216DE
	// (set) Token: 0x0600174C RID: 5964 RVA: 0x001234E6 File Offset: 0x001216E6
	public SelEventCoopCtrl.GUI GuiData { get; private set; }

	// Token: 0x170003A0 RID: 928
	// (get) Token: 0x0600174D RID: 5965 RVA: 0x001234EF File Offset: 0x001216EF
	// (set) Token: 0x0600174E RID: 5966 RVA: 0x001234F7 File Offset: 0x001216F7
	private string LoadAssetPath { get; set; }

	// Token: 0x170003A1 RID: 929
	// (get) Token: 0x06001750 RID: 5968 RVA: 0x00123508 File Offset: 0x00121708
	// (set) Token: 0x0600174F RID: 5967 RVA: 0x00123500 File Offset: 0x00121700
	private static Dictionary<SelEventCoopCtrl.AttributeType, int> MapIdPairs { get; set; }

	// Token: 0x170003A2 RID: 930
	// (get) Token: 0x06001751 RID: 5969 RVA: 0x0012350F File Offset: 0x0012170F
	// (set) Token: 0x06001752 RID: 5970 RVA: 0x00123517 File Offset: 0x00121717
	public QuestUtil.SelectData SelectData { private get; set; }

	// Token: 0x170003A3 RID: 931
	// (get) Token: 0x06001754 RID: 5972 RVA: 0x00123529 File Offset: 0x00121729
	// (set) Token: 0x06001753 RID: 5971 RVA: 0x00123520 File Offset: 0x00121720
	private int SelectRankingIndex { get; set; }

	// Token: 0x06001755 RID: 5973 RVA: 0x00123531 File Offset: 0x00121731
	public bool FinishedRequestGetCoopInfo()
	{
		return this.requestGetCoopInfo == null;
	}

	// Token: 0x06001756 RID: 5974 RVA: 0x0012353C File Offset: 0x0012173C
	public float GetOffsetPosY()
	{
		if (this.GuiData == null)
		{
			return 0f;
		}
		return this.GuiData.questSelect.GetOffsetPosY();
	}

	// Token: 0x06001757 RID: 5975 RVA: 0x0012355C File Offset: 0x0012175C
	public void SetupQuestSelect()
	{
		if (this.GuiData != null)
		{
			this.GuiData.questSelect.SetActive(false);
			this.requestGetCoopInfo = this.RequestGetCoopInfo(this.SelectData.mapId);
		}
	}

	// Token: 0x06001758 RID: 5976 RVA: 0x0012358E File Offset: 0x0012178E
	public bool IsBonus(int mapId)
	{
		return SelEventCoopCtrl.MapIdPairs[SelEventCoopCtrl.AttributeType.BONUS] == mapId;
	}

	// Token: 0x06001759 RID: 5977 RVA: 0x001235A0 File Offset: 0x001217A0
	public static bool OpenedBonus()
	{
		DataManagerEvent.CoopData lastCoopInfo = DataManager.DmEvent.LastCoopInfo;
		bool flag = true;
		foreach (KeyValuePair<SelEventCoopCtrl.AttributeType, int> keyValuePair in SelEventCoopCtrl.MapIdPairs)
		{
			if (lastCoopInfo.MapInfoMap.ContainsKey(keyValuePair.Value))
			{
				DataManagerEvent.CoopData.MapInfo mapInfo = lastCoopInfo.MapInfoMap[keyValuePair.Value];
				if (mapInfo != null)
				{
					flag &= mapInfo.IsClear;
				}
			}
		}
		return flag;
	}

	// Token: 0x0600175A RID: 5978 RVA: 0x00123630 File Offset: 0x00121830
	public void Init(SelEventCoopCtrl.InitParam _initParam, SelEventCoopCtrl.SetupParam _setupParam)
	{
		this.createGui = null;
		this.initParam = _initParam;
		List<int> playableMapIdList = DataManager.DmQuest.GetPlayableMapIdList(_setupParam.eventData.eventChapterId);
		SelEventCoopCtrl.MapIdPairs = new Dictionary<SelEventCoopCtrl.AttributeType, int>
		{
			{
				SelEventCoopCtrl.AttributeType.RED,
				playableMapIdList[0]
			},
			{
				SelEventCoopCtrl.AttributeType.GREEN,
				playableMapIdList[1]
			},
			{
				SelEventCoopCtrl.AttributeType.BLUE,
				playableMapIdList[2]
			},
			{
				SelEventCoopCtrl.AttributeType.PINK,
				playableMapIdList[3]
			},
			{
				SelEventCoopCtrl.AttributeType.LIME,
				playableMapIdList[4]
			},
			{
				SelEventCoopCtrl.AttributeType.AQUA,
				playableMapIdList[5]
			},
			{
				SelEventCoopCtrl.AttributeType.BONUS,
				playableMapIdList[6]
			}
		};
		this.Setup(_setupParam);
	}

	// Token: 0x0600175B RID: 5979 RVA: 0x001236D4 File Offset: 0x001218D4
	public void Setup(SelEventCoopCtrl.SetupParam _setupParam)
	{
		this.setupParam = _setupParam;
		this.SelectRankingIndex = 0;
		if (this.createGui != null)
		{
			return;
		}
		this.createGui = Singleton<SceneManager>.Instance.StartCoroutine(this.CreateGUI());
	}

	// Token: 0x0600175C RID: 5980 RVA: 0x00123704 File Offset: 0x00121904
	public void UpdateDecoration()
	{
		CanvasManager.HdlHelpWindowCtrl.SetCurrentOpenHelpByCoop(true, this.setupParam.eventData.eventId);
		DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(this.setupParam.eventData.eventId);
		if (eventData == null)
		{
			return;
		}
		this.requestGetCoopInfo = this.RequestGetCoopInfo(0);
		HomeBannerData homeBannerData = DataManager.DmHome.GetHomeBannerData(this.setupParam.eventData.eventBannerId);
		if (homeBannerData != null)
		{
			this.GuiData.mapSelect.EventBanner.banner = homeBannerData.bannerImagePathEvent;
		}
		this.GuiData.mapSelect.Txt_Term.gameObject.GetParent().SetActive(!eventData.raidFlg);
		this.GuiData.mapSelect.Txt_RaidTerm.gameObject.GetParent().SetActive(eventData.raidFlg);
		if (eventData.raidFlg)
		{
			DateTime now = TimeManager.Now;
			DataManagerEvent.CoopRaidTermData nowTermData = DataManager.DmEvent.GetNowTermData(eventData.eventId);
			if (nowTermData == null)
			{
				return;
			}
			DateTime dateTime = new DateTime(now.Year, now.Month, now.Day, nowTermData.startTime.Hours, nowTermData.startTime.Minutes, nowTermData.startTime.Seconds);
			DateTime dateTime2 = new DateTime(now.Year, now.Month, now.Day, nowTermData.endTime.Hours, nowTermData.endTime.Minutes, nowTermData.endTime.Seconds);
			this.GuiData.mapSelect.Txt_RaidTerm.text = "今回のターム：" + dateTime.ToString("HH:mm") + " ～ " + dateTime2.ToString("HH:mm まで");
		}
		else
		{
			DateTime startTime = DataManager.DmQuest.QuestStaticData.chapterDataMap[eventData.eventChapterId].mapDataList[0].questGroupList[0].startTime;
			DateTime endTime = DataManager.DmQuest.QuestStaticData.chapterDataMap[eventData.eventChapterId].mapDataList[0].questGroupList[0].endTime;
			this.GuiData.mapSelect.Txt_Term.text = startTime.ToString("M/d") + " ～ " + endTime.ToString("M/d HH:mm まで");
		}
		if (this.setupParam.eventData.eventCoinIdList.Count > 0)
		{
			this.GuiData.mapSelect.eventInfo.ItemOwnBase_Num_Txt.text = string.Format("{0}", DataManager.DmItem.GetUserItemData(this.setupParam.eventData.eventCoinIdList[0]).num);
			this.GuiData.mapSelect.eventInfo.Icon_Stone.SetRawImage(DataManager.DmItem.GetItemStaticBase(this.setupParam.eventData.eventCoinIdList[0]).GetIconName(), true, false, null);
		}
		int userClearEventMissionNum = DataManager.DmMission.GetUserClearEventMissionNum(eventData.eventId);
		this.GuiData.mapSelect.Txt_Mission_Num.transform.parent.transform.gameObject.SetActive(userClearEventMissionNum > 0);
		this.GuiData.mapSelect.Txt_Mission_Num.text = string.Format("{0}", userClearEventMissionNum);
		CanvasManager.SetBgTexture(SelEventCoopCtrl.BG_NAME);
	}

	// Token: 0x0600175D RID: 5981 RVA: 0x00123A84 File Offset: 0x00121C84
	public void Dest()
	{
		SelEventCoopCtrl.GUI guiData = this.GuiData;
	}

	// Token: 0x0600175E RID: 5982 RVA: 0x00123A8D File Offset: 0x00121C8D
	public void Destroy()
	{
	}

	// Token: 0x0600175F RID: 5983 RVA: 0x00123A8F File Offset: 0x00121C8F
	public static void PlayBGM()
	{
		SoundManager.PlayBGM("prd_bgm0068");
	}

	// Token: 0x06001760 RID: 5984 RVA: 0x00123A9B File Offset: 0x00121C9B
	public static void OpenHelpWindow(int eventId, bool isPriorityPickup)
	{
		CanvasManager.HdlCmnFeedPageWindowCtrl.Open(CmnFeedPageWindowCtrl.Type.PAGE_FEED, "イベントの遊びかた", SelEventCoopCtrl.GetTipsFilePath(eventId, isPriorityPickup), null);
	}

	// Token: 0x06001761 RID: 5985 RVA: 0x00123AB5 File Offset: 0x00121CB5
	public IEnumerator RequestBounusClearReset(int mapId)
	{
		QuestStaticMap mapData = DataManager.DmQuest.QuestStaticData.mapDataMap[mapId];
		QuestStaticQuestGroup questStaticQuestGroup = mapData.questGroupList.Find((QuestStaticQuestGroup item) => item.mapId == mapData.mapId);
		int questOneId = 0;
		Predicate<QuestDynamicQuestOne> <>9__1;
		for (int i = 0; i < questStaticQuestGroup.questOneList.Count; i++)
		{
			QuestStaticQuestOne questStaticQuestOne = questStaticQuestGroup.questOneList[i];
			questOneId = questStaticQuestOne.questId;
			List<QuestDynamicQuestOne> oneDataList = DataManager.DmQuest.QuestDynamicData.oneDataList;
			Predicate<QuestDynamicQuestOne> predicate;
			if ((predicate = <>9__1) == null)
			{
				predicate = (<>9__1 = (QuestDynamicQuestOne data) => data.questOneId == questOneId);
			}
			QuestDynamicQuestOne questDynamicQuestOne = oneDataList.Find(predicate);
			DataManagerEvent.CoopRaidTermData nowTermData = DataManager.DmEvent.GetNowTermData(DataManager.DmEvent.LastCoopInfo.EventId);
			if (nowTermData == null || questDynamicQuestOne.lastClearTime == null || nowTermData.IsOverStartTime(questDynamicQuestOne.lastClearTime.Value))
			{
				yield break;
			}
		}
		if (questOneId == 0)
		{
			yield break;
		}
		DataManager.DmQuest.RequestActionQuestLimitRecoveryByQuestOne(questOneId, true);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001762 RID: 5986 RVA: 0x00123AC4 File Offset: 0x00121CC4
	public IEnumerator RequestBounusClearResetFromGroup(int mapId)
	{
		QuestStaticMap mapData = DataManager.DmQuest.QuestStaticData.mapDataMap[mapId];
		QuestStaticQuestGroup questStaticQuestGroup = mapData.questGroupList.Find((QuestStaticQuestGroup item) => item.mapId == mapData.mapId);
		int num = 0;
		if (questStaticQuestGroup.limitClearNum <= 0)
		{
			yield break;
		}
		DateTime? lastClearTime = new DateTime?(default(DateTime));
		for (int i = 0; i < questStaticQuestGroup.questOneList.Count; i++)
		{
			QuestStaticQuestOne one = questStaticQuestGroup.questOneList[i];
			QuestDynamicQuestOne questDynamicQuestOne = DataManager.DmQuest.QuestDynamicData.oneDataList.Find((QuestDynamicQuestOne data) => data.questOneId == one.questId);
			if (lastClearTime == null || (questDynamicQuestOne.lastClearTime != null && lastClearTime.Value.CompareTo(questDynamicQuestOne.lastClearTime) <= 0))
			{
				lastClearTime = questDynamicQuestOne.lastClearTime;
				num = questDynamicQuestOne.questOneId;
			}
		}
		if (DataManager.DmEvent.GetNowTermData(DataManager.DmEvent.LastCoopInfo.EventId).IsOverStartTime(lastClearTime.Value) && lastClearTime.Value.Date == TimeManager.Now.Date)
		{
			yield break;
		}
		DataManager.DmQuest.RequestActionQuestLimitRecoveryByQuestOne(num, true);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001763 RID: 5987 RVA: 0x00123AD4 File Offset: 0x00121CD4
	private static List<string> GetTipsFilePath(int eventId, bool isPriorityPickup)
	{
		DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(eventId);
		if (eventData != null)
		{
			List<DataManagerEvent.EventImageData> list = new List<DataManagerEvent.EventImageData>(eventData.eventImageDataList);
			PrjUtil.InsertionSort<DataManagerEvent.EventImageData>(ref list, (DataManagerEvent.EventImageData a, DataManagerEvent.EventImageData b) => a.Sort - b.Sort);
			if (isPriorityPickup)
			{
				PrjUtil.InsertionSort<DataManagerEvent.EventImageData>(ref list, (DataManagerEvent.EventImageData a, DataManagerEvent.EventImageData b) => b.Type - a.Type);
			}
			else
			{
				PrjUtil.InsertionSort<DataManagerEvent.EventImageData>(ref list, (DataManagerEvent.EventImageData a, DataManagerEvent.EventImageData b) => a.Type - b.Type);
			}
			List<string> list2 = new List<string>();
			foreach (DataManagerEvent.EventImageData eventImageData in list)
			{
				list2.Add(eventImageData.ImagePath);
			}
			return list2;
		}
		return new List<string> { "Texture2D/Tutorial_Window/Event_Multi/tutorial_multievent_01", "Texture2D/Tutorial_Window/Event_Multi/tutorial_multievent_02", "Texture2D/Tutorial_Window/Event_Multi/tutorial_multievent_03", "Texture2D/Tutorial_Window/Event_Multi/tutorial_multievent_04" };
	}

	// Token: 0x06001764 RID: 5988 RVA: 0x00123BF8 File Offset: 0x00121DF8
	private IEnumerator LoadAssetObject(string path)
	{
		AssetManager.LoadAssetData(path, AssetManager.OWNER.QuestSelector, 0, null);
		while (!AssetManager.IsLoadFinishAssetData(path))
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001765 RID: 5989 RVA: 0x00123C07 File Offset: 0x00121E07
	private IEnumerator CreateGUI()
	{
		if (this.GuiData == null)
		{
			yield return null;
			this.GuiData = new SelEventCoopCtrl.GUI(Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneEvent/GUI/Prefab/GUI_Event_Multi_PointSelect"), base.transform).transform, Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneEvent/GUI/Prefab/Quest_ChapterLeft_EventMulti"), this.initParam.chapterLeftObject.transform).transform, Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneEvent/GUI/Prefab/Quest_ChapterRight_EventMulti"), this.initParam.chapterRightObject.transform).transform);
			this.GuiData.mapSelect.Setup(this.setupParam.pointTouchCB);
			PrjUtil.AddTouchEventTrigger(this.GuiData.mapSelect.EventBanner.gameObject, delegate(Transform tr)
			{
				QuestUtil.OpenBannerWebViewWindow(this.setupParam.eventData.eventId);
			});
			this.GuiData.mapSelect.eventInfo.Btn_ShopEvent.AddOnClickListener(delegate(PguiButtonCtrl button)
			{
				this.initParam.reqShopSequenceCB();
			}, PguiButtonCtrl.SoundType.DEFAULT);
			this.GuiData.mapSelect.eventInfo.Btn_Gacha.AddOnClickListener(delegate(PguiButtonCtrl button)
			{
				SceneGacha.OpenParam openParam = new SceneGacha.OpenParam
				{
					gachaId = this.setupParam.eventData.eventGachaId,
					resultNextSceneName = SceneManager.SceneName.SceneQuest,
					resultNextSceneArgs = new SceneQuest.Args
					{
						selectEventId = this.setupParam.eventData.eventId,
						category = QuestStaticChapter.Category.EVENT,
						backSequenceGameObject = this.GuiData.mapSelect.baseObj
					}
				};
				Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneGacha, openParam);
			}, PguiButtonCtrl.SoundType.DEFAULT);
			this.GuiData.mapSelect.Btn_Mission.AddOnClickListener(delegate(PguiButtonCtrl button)
			{
				SceneMission.MissionOpenParam missionOpenParam = new SceneMission.MissionOpenParam(MissionType.EVENTTOTAL, this.setupParam.eventData.eventId)
				{
					returnSceneName = SceneManager.SceneName.SceneQuest,
					resultNextSceneArgs = new SceneQuest.Args
					{
						selectEventId = this.setupParam.eventData.eventId,
						category = QuestStaticChapter.Category.EVENT,
						backSequenceGameObject = this.GuiData.mapSelect.baseObj
					}
				};
				Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneMission, missionOpenParam);
			}, PguiButtonCtrl.SoundType.DEFAULT);
			this.GuiData.questSelect.right.Btn_GetItem.AddOnClickListener(delegate(PguiButtonCtrl button)
			{
				CanvasManager.HdlEventCoopWindowCtrl.Open(this.SelectData.mapId);
			}, PguiButtonCtrl.SoundType.DEFAULT);
			this.GuiData.questSelect.right.ConvertPrefabToRaid(DataManager.DmEvent.GetEventData(this.setupParam.eventData.eventId).raidFlg);
			this.GuiData.questSelect.left.ConvertRankingInRaid(DataManager.DmEvent.GetEventData(this.setupParam.eventData.eventId).raidFlg);
			this.GuiData.questSelect.left.ScrollView.InitForce();
			ReuseScroll scrollView = this.GuiData.questSelect.left.ScrollView;
			scrollView.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollView.onStartItem, new Action<int, GameObject>(delegate(int index, GameObject go)
			{
				new SelEventCoopCtrl.GUI.QuestSelect.Left.ListBarPlayer(go.transform);
			}));
			ReuseScroll scrollView2 = this.GuiData.questSelect.left.ScrollView;
			scrollView2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollView2.onUpdateItem, new Action<int, GameObject>(delegate(int index, GameObject go)
			{
				DataManagerEvent.CoopData.MapInfo mapInfo = DataManager.DmEvent.LastCoopInfo.MapInfoMap[this.SelectData.mapId];
				List<DataManagerEvent.CoopData.MapInfo.RankingInfo> rankingInfoList = mapInfo.RankingInfoList;
				if (DataManager.DmEvent.GetEventData(mapInfo.EventId).raidFlg)
				{
					List<CoopPlayerInfo> userRankingList = rankingInfoList[this.SelectRankingIndex].UserRankingList;
					go.SetActive(true);
					CoopPlayerInfo coopPlayerInfo = userRankingList[index];
					SelEventCoopCtrl.GUI.QuestSelect.Left.ListBarPlayer listBarPlayer = new SelEventCoopCtrl.GUI.QuestSelect.Left.ListBarPlayer(go.transform);
					listBarPlayer.IconChara.SetupPrm(new IconCharaCtrl.SetupParam
					{
						cpd = CharaPackData.MakeInitial(coopPlayerInfo.favorite_chara_id),
						iconId = coopPlayerInfo.favorite_chara_face_id
					});
					listBarPlayer.IconChara.DispRanking();
					listBarPlayer.Icon_Stone.gameObject.SetActive(false);
					listBarPlayer.Num_Rank.ReplaceTextByDefault("Param01", string.Format("{0}", coopPlayerInfo.user_level));
					listBarPlayer.Num_Txt.text = string.Format("{0}", coopPlayerInfo.point);
					listBarPlayer.Txt_FriendName.text = coopPlayerInfo.user_name;
					listBarPlayer.SetupRanking(coopPlayerInfo.rank);
					listBarPlayer.baseObj.gameObject.SetActive(true);
					listBarPlayer.Achievement.Setup(coopPlayerInfo.achievement_id, true, false);
					return;
				}
				if (index < rankingInfoList.Count && rankingInfoList.Count > 0)
				{
					DataManagerEvent.CoopData.MapInfo.RankingInfo rankingInfo = rankingInfoList[this.SelectRankingIndex];
					for (int i = 0; i < 1; i++)
					{
						int num = index + i;
						go.SetActive(num < rankingInfo.UserRankingList.Count);
						if (num < rankingInfo.UserRankingList.Count)
						{
							CoopPlayerInfo coopPlayerInfo2 = rankingInfo.UserRankingList[num];
							SelEventCoopCtrl.GUI.QuestSelect.Left.ListBarPlayer listBarPlayer2 = new SelEventCoopCtrl.GUI.QuestSelect.Left.ListBarPlayer(go.transform);
							listBarPlayer2.IconChara.SetupPrm(new IconCharaCtrl.SetupParam
							{
								cpd = CharaPackData.MakeInitial(coopPlayerInfo2.favorite_chara_id),
								iconId = coopPlayerInfo2.favorite_chara_face_id
							});
							listBarPlayer2.IconChara.DispRanking();
							listBarPlayer2.Icon_Stone.SetRawImage(DataManager.DmItem.GetItemStaticBase(this.setupParam.eventData.eventCoinIdList[0]).GetIconName(), true, false, null);
							listBarPlayer2.Num_Rank.ReplaceTextByDefault("Param01", string.Format("{0}", coopPlayerInfo2.user_level));
							listBarPlayer2.Num_Txt.text = string.Format("{0}", coopPlayerInfo2.point);
							listBarPlayer2.Txt_FriendName.text = coopPlayerInfo2.user_name;
							listBarPlayer2.SetupRanking(coopPlayerInfo2.rank);
							listBarPlayer2.Achievement.Setup(coopPlayerInfo2.achievement_id, true, false);
						}
					}
				}
			}));
			this.GuiData.questSelect.left.ScrollView.Setup(10, 0);
			this.GuiData.questSelect.left.Btn_Yaji_Left.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickLR), PguiButtonCtrl.SoundType.DEFAULT);
			this.GuiData.questSelect.left.Btn_Yaji_Right.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickLR), PguiButtonCtrl.SoundType.DEFAULT);
			UnityAction selectObjsCB = this.initParam.selectObjsCB;
			if (selectObjsCB != null)
			{
				selectObjsCB();
			}
		}
		UnityAction reqNextSequenceCB = this.setupParam.reqNextSequenceCB;
		if (reqNextSequenceCB != null)
		{
			reqNextSequenceCB();
		}
		this.createGui = null;
		yield break;
	}

	// Token: 0x06001766 RID: 5990 RVA: 0x00123C16 File Offset: 0x00121E16
	private IEnumerator RequestGetCoopInfo(int mapId)
	{
		int oldTermId = 0;
		int eventId = this.setupParam.eventData.eventId;
		if (DataManager.DmEvent.GetNowTermData(eventId) != null)
		{
			DataManager.DmEvent.RequestGetCoopInfo(eventId, mapId);
			if (DataManager.DmEvent.LastCoopInfo != null)
			{
				oldTermId = DataManager.DmEvent.GetTermData(eventId, DataManager.DmEvent.LastCoopInfo.InfoGetDateTime).termId;
			}
		}
		if (mapId != 0)
		{
			this.GuiData.questSelect.SetActive(false);
		}
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		if (oldTermId != 0 && oldTermId != DataManager.DmEvent.GetTermData(eventId, DataManager.DmEvent.LastCoopInfo.InfoGetDateTime).termId)
		{
			CanvasManager.HdlOpenWindowBasic.Setup("確認", "更新データが見つかりました\nデータ更新を行います", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int index) => true, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			do
			{
				yield return null;
			}
			while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
			Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneQuest, null);
			yield break;
		}
		List<DataManagerEvent.ReleaseEffects> releaseEffectsList = DataManager.DmEvent.GetReleaseEffectsList();
		DataManagerEvent.ReleaseEffects releaseEffects = null;
		QuestUtil.GetEnableEventReleaseEffects(ref releaseEffectsList, ref releaseEffects, this.setupParam.eventData);
		if (releaseEffects.TutorialPhase == 0)
		{
			SelEventCoopCtrl.OpenHelpWindow(this.setupParam.eventData.eventId, true);
			yield return null;
			while (!CanvasManager.HdlCmnFeedPageWindowCtrl.FinishedClose())
			{
				yield return null;
			}
			releaseEffects.TutorialPhase = 1;
			DataManager.DmEvent.RequestUpdateReleaseEffects(releaseEffectsList);
		}
		if (mapId == 0)
		{
			this.GuiData.mapSelect.baseObj.SetActive(true);
			this.GuiData.mapSelect.UpdateObject(this.setupParam.eventData.eventId);
			this.GuiData.mapSelect.eventInfo.UpdateObject();
		}
		else
		{
			this.GuiData.questSelect.SetActive(true);
			this.GuiData.questSelect.Setup(mapId, this.SelectRankingIndex, this.setupParam.eventData.raidFlg, this.IsBonus(mapId));
			this.SetActiveLRButton();
		}
		yield break;
	}

	// Token: 0x06001767 RID: 5991 RVA: 0x00123C2C File Offset: 0x00121E2C
	private void Start()
	{
	}

	// Token: 0x06001768 RID: 5992 RVA: 0x00123C2E File Offset: 0x00121E2E
	private void Update()
	{
		if (this.requestGetCoopInfo != null && !this.requestGetCoopInfo.MoveNext())
		{
			this.requestGetCoopInfo = null;
		}
	}

	// Token: 0x06001769 RID: 5993 RVA: 0x00123C4C File Offset: 0x00121E4C
	private void SetActiveLRButton()
	{
		DataManagerEvent.CoopData lastCoopInfo = DataManager.DmEvent.LastCoopInfo;
		if (lastCoopInfo.MapInfoMap.ContainsKey(this.SelectData.mapId))
		{
			DataManagerEvent.CoopData.MapInfo mapInfo = lastCoopInfo.MapInfoMap[this.SelectData.mapId];
			this.GuiData.questSelect.left.Btn_Yaji_Left.gameObject.SetActive(this.SelectRankingIndex < mapInfo.RankingInfoList.Count - 1);
			this.GuiData.questSelect.left.Btn_Yaji_Right.gameObject.SetActive(this.SelectRankingIndex > 0);
		}
	}

	// Token: 0x0600176A RID: 5994 RVA: 0x00123CF0 File Offset: 0x00121EF0
	private void OnClickLR(PguiButtonCtrl button)
	{
		DataManagerEvent.CoopData.MapInfo mapInfo = DataManager.DmEvent.LastCoopInfo.MapInfoMap[this.SelectData.mapId];
		if (this.GuiData.questSelect.left.Btn_Yaji_Left == button || this.GuiData.questSelect.left.Btn_Yaji_Right == button)
		{
			this.SelectRankingIndex += ((button == this.GuiData.questSelect.left.Btn_Yaji_Left) ? (-1) : 1);
			this.SelectRankingIndex = (this.SelectRankingIndex + mapInfo.RankingInfoList.Count) % mapInfo.RankingInfoList.Count;
		}
		this.GuiData.questSelect.left.SetupRanking(this.SelectData.mapId, this.SelectRankingIndex);
		this.SetActiveLRButton();
	}

	// Token: 0x04001260 RID: 4704
	public static readonly string BG_NAME = "selbg_event_multi";

	// Token: 0x04001262 RID: 4706
	private SelEventCoopCtrl.InitParam initParam = new SelEventCoopCtrl.InitParam();

	// Token: 0x04001263 RID: 4707
	private SelEventCoopCtrl.SetupParam setupParam = new SelEventCoopCtrl.SetupParam();

	// Token: 0x04001265 RID: 4709
	private Coroutine createGui;

	// Token: 0x04001269 RID: 4713
	private IEnumerator requestGetCoopInfo;

	// Token: 0x02000CE6 RID: 3302
	private enum AttributeType
	{
		// Token: 0x04004CD0 RID: 19664
		RED,
		// Token: 0x04004CD1 RID: 19665
		GREEN,
		// Token: 0x04004CD2 RID: 19666
		BLUE,
		// Token: 0x04004CD3 RID: 19667
		PINK,
		// Token: 0x04004CD4 RID: 19668
		LIME,
		// Token: 0x04004CD5 RID: 19669
		AQUA,
		// Token: 0x04004CD6 RID: 19670
		BONUS
	}

	// Token: 0x02000CE7 RID: 3303
	public class InitParam
	{
		// Token: 0x04004CD7 RID: 19671
		public UnityAction reqShopSequenceCB;

		// Token: 0x04004CD8 RID: 19672
		public UnityAction selectObjsCB;

		// Token: 0x04004CD9 RID: 19673
		public GameObject chapterLeftObject;

		// Token: 0x04004CDA RID: 19674
		public GameObject chapterRightObject;
	}

	// Token: 0x02000CE8 RID: 3304
	public class SetupParam
	{
		// Token: 0x04004CDB RID: 19675
		public DataManagerEvent.EventData eventData;

		// Token: 0x04004CDC RID: 19676
		public UnityAction reqNextSequenceCB;

		// Token: 0x04004CDD RID: 19677
		public UnityAction<Transform> pointTouchCB;
	}

	// Token: 0x02000CE9 RID: 3305
	public class GUI
	{
		// Token: 0x06004791 RID: 18321 RVA: 0x00218C7E File Offset: 0x00216E7E
		public GUI(Transform mapBaseTr, Transform questLeftBaseTr, Transform questRightBaseTr)
		{
			this.mapSelect = new SelEventCoopCtrl.GUI.MapSelect(mapBaseTr);
			this.mapSelect.baseObj.SetActive(false);
			this.questSelect = new SelEventCoopCtrl.GUI.QuestSelect(questLeftBaseTr, questRightBaseTr);
		}

		// Token: 0x04004CDE RID: 19678
		public SelEventCoopCtrl.GUI.MapSelect mapSelect;

		// Token: 0x04004CDF RID: 19679
		public SelEventCoopCtrl.GUI.QuestSelect questSelect;

		// Token: 0x020011BA RID: 4538
		public class PlayerInfo
		{
			// Token: 0x060056F6 RID: 22262 RVA: 0x00254ECB File Offset: 0x002530CB
			public PlayerInfo(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Txt_Info = baseTr.Find("Txt_Info").GetComponent<PguiTextCtrl>();
				this.anim = baseTr.GetComponent<SimpleAnimation>();
			}

			// Token: 0x060056F7 RID: 22263 RVA: 0x00254F04 File Offset: 0x00253104
			public void Setup(List<DataManagerEvent.CoopData.DispLog> logs, int index, bool isRaid)
			{
				this.baseObj.SetActive(true);
				DataManagerEvent.CoopData.DispLog dispLog = logs[index];
				QuestStaticMap questStaticMap = DataManager.DmQuest.QuestStaticData.mapDataMap[dispLog.MapId];
				this.Txt_Info.text = (isRaid ? string.Format("{0} が {1} でＨＰを{2}減らした！", dispLog.UserName, questStaticMap.mapName, dispLog.Point) : string.Format("{0} が {1} で{2}ポイント獲得！", dispLog.UserName, questStaticMap.mapName, dispLog.Point));
				SimpleAnimation.ExFinishCallback <>9__1;
				this.anim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, delegate
				{
					SimpleAnimation simpleAnimation = this.anim;
					SimpleAnimation.ExPguiStatus exPguiStatus = SimpleAnimation.ExPguiStatus.END;
					SimpleAnimation.ExFinishCallback exFinishCallback;
					if ((exFinishCallback = <>9__1) == null)
					{
						exFinishCallback = (<>9__1 = delegate
						{
							this.baseObj.SetActive(false);
							int num = index + 1;
							index = num;
							if (logs.Count > index)
							{
								this.Setup(logs, index, isRaid);
							}
						});
					}
					simpleAnimation.ExPlayAnimation(exPguiStatus, exFinishCallback);
				});
			}

			// Token: 0x0400613F RID: 24895
			public GameObject baseObj;

			// Token: 0x04006140 RID: 24896
			public PguiTextCtrl Txt_Info;

			// Token: 0x04006141 RID: 24897
			public SimpleAnimation anim;
		}

		// Token: 0x020011BB RID: 4539
		public class MapSelect
		{
			// Token: 0x060056F8 RID: 22264 RVA: 0x00254FDC File Offset: 0x002531DC
			public MapSelect(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.mapPointPairs = new Dictionary<SelEventCoopCtrl.AttributeType, SelEventCoopCtrl.GUI.MapSelect.IMapPoint>
				{
					{
						SelEventCoopCtrl.AttributeType.RED,
						new SelEventCoopCtrl.GUI.MapSelect.MapPoint(baseTr.Find("EventMulti_MapPoint01"))
					},
					{
						SelEventCoopCtrl.AttributeType.GREEN,
						new SelEventCoopCtrl.GUI.MapSelect.MapPoint(baseTr.Find("EventMulti_MapPoint02"))
					},
					{
						SelEventCoopCtrl.AttributeType.BLUE,
						new SelEventCoopCtrl.GUI.MapSelect.MapPoint(baseTr.Find("EventMulti_MapPoint03"))
					},
					{
						SelEventCoopCtrl.AttributeType.PINK,
						new SelEventCoopCtrl.GUI.MapSelect.MapPoint(baseTr.Find("EventMulti_MapPoint04"))
					},
					{
						SelEventCoopCtrl.AttributeType.LIME,
						new SelEventCoopCtrl.GUI.MapSelect.MapPoint(baseTr.Find("EventMulti_MapPoint05"))
					},
					{
						SelEventCoopCtrl.AttributeType.AQUA,
						new SelEventCoopCtrl.GUI.MapSelect.MapPoint(baseTr.Find("EventMulti_MapPoint06"))
					},
					{
						SelEventCoopCtrl.AttributeType.BONUS,
						new SelEventCoopCtrl.GUI.MapSelect.MapPointBonus(baseTr.Find("EventMulti_MapPoint_Bonus"))
					}
				};
				this.eventInfo = new SelEventCoopCtrl.GUI.MapSelect.EventInfo(baseTr.Find("Right/EventInfo"));
				this.Btn_Mission = baseTr.Find("Right/Btn_Mission").GetComponent<PguiButtonCtrl>();
				this.Btn_Mission.transform.Find("BaseImage/Mark_New").gameObject.SetActive(false);
				this.Txt_Mission_Num = this.Btn_Mission.transform.Find("BaseImage/Badge/Cmn_Badge/Num").GetComponent<PguiTextCtrl>();
				this.EventBanner = baseTr.Find("Left/EventBanner").GetComponent<PguiRawImageCtrl>();
				this.Txt_Term = baseTr.Find("Left/EventTerm/Txt_Term").GetComponent<PguiTextCtrl>();
				this.Txt_Term.text = "";
				this.Txt_RaidTerm = baseTr.Find("Right/RaidEventTerm/Txt_Term").GetComponent<PguiTextCtrl>();
				this.Txt_RaidTerm.text = "";
				this.playerInfo = new SelEventCoopCtrl.GUI.PlayerInfo(baseTr.Find("EventMulti_PlayerInfo"));
				this.Go_Left = baseTr.Find("Left").gameObject;
				this.playerInfo.baseObj.gameObject.SetActive(false);
			}

			// Token: 0x060056F9 RID: 22265 RVA: 0x002551B8 File Offset: 0x002533B8
			public void Setup(UnityAction<Transform> pointTouchCB)
			{
				foreach (KeyValuePair<SelEventCoopCtrl.AttributeType, int> keyValuePair in SelEventCoopCtrl.MapIdPairs)
				{
					this.mapPointPairs[keyValuePair.Key].baseObj.name = string.Format("{0}", keyValuePair.Value);
				}
				foreach (SelEventCoopCtrl.GUI.MapSelect.IMapPoint mapPoint in this.mapPointPairs.Values)
				{
					mapPoint.Setup(pointTouchCB);
					mapPoint.baseObj.SetActive(false);
				}
			}

			// Token: 0x060056FA RID: 22266 RVA: 0x00255288 File Offset: 0x00253488
			public void UpdateObject(int eventId)
			{
				foreach (SelEventCoopCtrl.GUI.MapSelect.IMapPoint mapPoint in this.mapPointPairs.Values)
				{
					SelEventCoopCtrl.GUI.MapSelect.MapPoint mapPoint2 = mapPoint as SelEventCoopCtrl.GUI.MapSelect.MapPoint;
					if (mapPoint2 != null)
					{
						mapPoint2.Setup();
						mapPoint2.baseObj.SetActive(true);
					}
					else
					{
						SelEventCoopCtrl.GUI.MapSelect.MapPointBonus mapPointBonus = mapPoint as SelEventCoopCtrl.GUI.MapSelect.MapPointBonus;
						mapPointBonus.Setup(eventId);
						mapPointBonus.baseObj.SetActive(true);
					}
				}
				DataManagerEvent.CoopData lastCoopInfo = DataManager.DmEvent.LastCoopInfo;
				int num = 0;
				if (lastCoopInfo.DispLogList.Count > 0)
				{
					this.playerInfo.Setup(lastCoopInfo.DispLogList, num, DataManager.DmEvent.GetEventData(lastCoopInfo.EventId).raidFlg);
				}
			}

			// Token: 0x04006142 RID: 24898
			public GameObject baseObj;

			// Token: 0x04006143 RID: 24899
			private Dictionary<SelEventCoopCtrl.AttributeType, SelEventCoopCtrl.GUI.MapSelect.IMapPoint> mapPointPairs;

			// Token: 0x04006144 RID: 24900
			public SelEventCoopCtrl.GUI.MapSelect.EventInfo eventInfo;

			// Token: 0x04006145 RID: 24901
			public PguiButtonCtrl Btn_Mission;

			// Token: 0x04006146 RID: 24902
			public PguiTextCtrl Txt_Mission_Num;

			// Token: 0x04006147 RID: 24903
			public PguiRawImageCtrl EventBanner;

			// Token: 0x04006148 RID: 24904
			public PguiTextCtrl Txt_Term;

			// Token: 0x04006149 RID: 24905
			public PguiTextCtrl Txt_RaidTerm;

			// Token: 0x0400614A RID: 24906
			public SelEventCoopCtrl.GUI.PlayerInfo playerInfo;

			// Token: 0x0400614B RID: 24907
			public GameObject Go_Left;

			// Token: 0x02001242 RID: 4674
			public class IMapPoint
			{
				// Token: 0x06005844 RID: 22596 RVA: 0x0025B53A File Offset: 0x0025973A
				public IMapPoint(Transform baseTr)
				{
					this.baseObj = baseTr.gameObject;
					this.baseRtf = baseTr as RectTransform;
				}

				// Token: 0x06005845 RID: 22597 RVA: 0x0025B55A File Offset: 0x0025975A
				public void Setup(UnityAction<Transform> pointTouchCB)
				{
					PrjUtil.AddTouchEventTrigger(this.baseRtf.gameObject, pointTouchCB);
				}

				// Token: 0x040063ED RID: 25581
				public GameObject baseObj;

				// Token: 0x040063EE RID: 25582
				public RectTransform baseRtf;
			}

			// Token: 0x02001243 RID: 4675
			public class MapPoint : SelEventCoopCtrl.GUI.MapSelect.IMapPoint
			{
				// Token: 0x17000D1F RID: 3359
				// (get) Token: 0x06005846 RID: 22598 RVA: 0x0025B56D File Offset: 0x0025976D
				// (set) Token: 0x06005847 RID: 22599 RVA: 0x0025B575 File Offset: 0x00259775
				private bool FinishedHardQuest { get; set; }

				// Token: 0x17000D20 RID: 3360
				// (get) Token: 0x06005848 RID: 22600 RVA: 0x0025B57E File Offset: 0x0025977E
				// (set) Token: 0x06005849 RID: 22601 RVA: 0x0025B586 File Offset: 0x00259786
				private bool PlayedHardQuestAE { get; set; }

				// Token: 0x17000D21 RID: 3361
				// (get) Token: 0x0600584A RID: 22602 RVA: 0x0025B58F File Offset: 0x0025978F
				// (set) Token: 0x0600584B RID: 22603 RVA: 0x0025B597 File Offset: 0x00259797
				private bool PlayedPointClearAE { get; set; }

				// Token: 0x0600584C RID: 22604 RVA: 0x0025B5A0 File Offset: 0x002597A0
				public MapPoint(Transform baseTr)
					: base(baseTr)
				{
					this.ClearGage_Gage = baseTr.Find("ClearGage/ClearGage_Gage").GetComponent<PguiImageCtrl>();
					this.Mark_Complete = baseTr.Find("Mark_Complete").GetComponent<PguiImageCtrl>();
					this.Txt_Clear = baseTr.Find("ClearGage/Txt_Clear").GetComponent<PguiTextCtrl>();
					this.AEimage_HardQuest = baseTr.Find("AEimage_HardQuest").GetComponent<PguiAECtrl>();
					this.AEimage_HardQuest.gameObject.SetActive(false);
					this.AEimage_PointClear = baseTr.Find("AEimage_PointClear").GetComponent<PguiAECtrl>();
					this.AEimage_PointClear.gameObject.SetActive(false);
					this.AEimage_Aura = baseTr.Find("AEimage_Aura").GetComponent<PguiAECtrl>();
					this.AEimage_Aura.gameObject.SetActive(false);
					this.AnchorObj = this.ClearGage_Gage.transform.Find("AnchorObj").gameObject;
					this.Icon_Boss = baseTr.Find("Tex").GetComponent<PguiRawImageCtrl>();
					this.bus = this.ClearGage_Gage.transform.Find("AnchorObj/Mark_car").GetComponent<RectTransform>();
					this.Campaign = baseTr.Find("Campaign").gameObject;
					this.FinishedHardQuest = false;
					this.PlayedHardQuestAE = false;
					this.PlayedPointClearAE = false;
				}

				// Token: 0x0600584D RID: 22605 RVA: 0x0025B6EC File Offset: 0x002598EC
				public void Setup()
				{
					DataManagerEvent.CoopData coopInfo = DataManager.DmEvent.LastCoopInfo;
					Action HardQuestEndInfoFunc = delegate
					{
						DataManagerEvent.CoopData.HardQuestEndInfo hardQuestEndInfoData = coopInfo.HardQuestEndInfoData;
						if (hardQuestEndInfoData.MapId != 0)
						{
							switch (hardQuestEndInfoData.type)
							{
							case DataManagerEvent.CoopData.HardQuestEndInfo.InfoType.Timeout:
								CanvasManager.HdlEventCoopWindowCtrl.OpenTimeout();
								return;
							case DataManagerEvent.CoopData.HardQuestEndInfo.InfoType.Achievement:
								CanvasManager.HdlEventCoopWindowCtrl.OpenAchievement();
								return;
							case DataManagerEvent.CoopData.HardQuestEndInfo.InfoType.AchievementAndClear:
								CanvasManager.HdlEventCoopWindowCtrl.OpenAchievementAndClear();
								break;
							default:
								return;
							}
						}
					};
					int mapId = int.Parse(this.baseObj.name);
					DataManagerEvent.CoopData.MapInfo mapInfo = coopInfo.MapInfoMap[mapId];
					DataManagerEvent.CoopConditionData coopConditionData = DataManager.DmEvent.GetCoopConditionDataList().Find((DataManagerEvent.CoopConditionData x) => x.MapId == mapId);
					if (!coopConditionData.TexturePath.IsNullOrEmpty())
					{
						this.Icon_Boss.SetRawImage("Texture2D/" + coopConditionData.TexturePath, true, false, null);
					}
					DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(coopConditionData.EventId);
					float num = mapInfo.ProgressRate;
					if (eventData.raidFlg)
					{
						num = 1f - mapInfo.ProgressRate;
					}
					if (num > 1f)
					{
						num = 1f;
					}
					if (num < 0f)
					{
						num = 0f;
					}
					this.Campaign.SetActive(eventData.raidFlg && num != 0f);
					if (this.Campaign.activeSelf)
					{
						this.Campaign.transform.Find("Popup_Campaign_Cmn").GetComponent<SimpleAnimation>().ExPlayAnimation(SimpleAnimation.ExPguiStatus.LOOP, null);
					}
					this.ClearGage_Gage.m_Image.fillAmount = num;
					bool flag = (eventData.raidFlg ? (this.ClearGage_Gage.m_Image.fillAmount <= 0f) : (this.ClearGage_Gage.m_Image.fillAmount >= 1f));
					this.Txt_Clear.gameObject.SetActive(flag);
					this.AnchorObj.SetActive(!flag);
					if (eventData.raidFlg)
					{
						Quaternion localRotation = this.bus.localRotation;
						Vector3 localPosition = this.bus.localPosition;
						this.bus.localRotation = new Quaternion(localRotation.x, -180f, localRotation.z, localRotation.w);
					}
					if (flag)
					{
						this.AEimage_Aura.gameObject.SetActive(false);
					}
					else
					{
						this.AEimage_Aura.gameObject.SetActive(eventData.raidFlg ? (this.ClearGage_Gage.m_Image.fillAmount <= 0.5f) : (this.ClearGage_Gage.m_Image.fillAmount >= 0.5f));
					}
					string text = (eventData.raidFlg ? ((this.ClearGage_Gage.m_Image.fillAmount <= 0.25f) ? "2" : "1") : ((this.ClearGage_Gage.m_Image.fillAmount >= 0.75f) ? "2" : "1"));
					this.AEimage_Aura.GetComponent<PguiReplaceAECtrl>().Replace(text);
					this.AEimage_Aura.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
					if (this.FinishedHardQuest && !mapInfo.IsHardQuestOpen)
					{
						this.AEimage_HardQuest.gameObject.SetActive(true);
						this.AEimage_HardQuest.PlayAnimation(PguiAECtrl.AmimeType.END, delegate
						{
							this.AEimage_HardQuest.gameObject.SetActive(false);
							HardQuestEndInfoFunc();
						});
						this.FinishedHardQuest = false;
						this.PlayedHardQuestAE = false;
						SoundManager.Play("prd_se_multievent_blockquest_end", false, false);
					}
					else
					{
						this.AEimage_HardQuest.gameObject.SetActive(mapInfo.IsHardQuestOpen);
						if (mapInfo.IsHardQuestOpen)
						{
							if (!this.PlayedHardQuestAE)
							{
								this.AEimage_HardQuest.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
								{
									this.AEimage_HardQuest.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
								});
								SoundManager.Play("prd_se_multievent_blockquest_appearance", false, false);
								this.FinishedHardQuest = true;
								this.PlayedHardQuestAE = true;
							}
							else
							{
								this.AEimage_HardQuest.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
							}
						}
						else
						{
							HardQuestEndInfoFunc();
						}
					}
					this.AEimage_PointClear.gameObject.SetActive(flag);
					if (flag)
					{
						if (!this.PlayedPointClearAE)
						{
							this.AEimage_PointClear.PlayAnimation(PguiAECtrl.AmimeType.START, null);
							SoundManager.Play("prd_se_multievent_mission_complete", false, false);
							this.PlayedPointClearAE = true;
						}
					}
					else
					{
						this.PlayedPointClearAE = false;
					}
					QuestStaticMap mapData = DataManager.DmQuest.QuestStaticData.mapDataMap[mapId];
					QuestStaticQuestGroup questStaticQuestGroup = mapData.questGroupList.Find((QuestStaticQuestGroup item) => item.mapId == mapData.mapId);
					int num2 = 0;
					for (int i = 0; i < questStaticQuestGroup.questOneList.Count; i++)
					{
						QuestStaticQuestOne questStaticQuestOne = questStaticQuestGroup.questOneList[i];
						QuestDynamicQuestOne questDynamicQuestOne = null;
						if (DataManager.DmQuest.QuestDynamicData.oneDataMap.ContainsKey(questStaticQuestOne.questId))
						{
							questDynamicQuestOne = DataManager.DmQuest.QuestDynamicData.oneDataMap[questStaticQuestOne.questId];
						}
						if (((questDynamicQuestOne != null) ? questDynamicQuestOne.status : QuestOneStatus.INVALID) == QuestOneStatus.COMPLETE)
						{
							num2++;
						}
					}
					this.Mark_Complete.gameObject.SetActive(num2 >= questStaticQuestGroup.questOneList.Count);
				}

				// Token: 0x040063EF RID: 25583
				public PguiImageCtrl ClearGage_Gage;

				// Token: 0x040063F0 RID: 25584
				public PguiImageCtrl Mark_Complete;

				// Token: 0x040063F1 RID: 25585
				public PguiTextCtrl Txt_Clear;

				// Token: 0x040063F2 RID: 25586
				public PguiAECtrl AEimage_HardQuest;

				// Token: 0x040063F3 RID: 25587
				public PguiAECtrl AEimage_PointClear;

				// Token: 0x040063F4 RID: 25588
				public PguiAECtrl AEimage_Aura;

				// Token: 0x040063F5 RID: 25589
				public GameObject AnchorObj;

				// Token: 0x040063F6 RID: 25590
				public PguiRawImageCtrl Icon_Boss;

				// Token: 0x040063F7 RID: 25591
				public RectTransform bus;

				// Token: 0x040063F8 RID: 25592
				public GameObject Campaign;
			}

			// Token: 0x02001244 RID: 4676
			public class MapPointBonus : SelEventCoopCtrl.GUI.MapSelect.IMapPoint
			{
				// Token: 0x17000D22 RID: 3362
				// (get) Token: 0x0600584E RID: 22606 RVA: 0x0025BBD0 File Offset: 0x00259DD0
				// (set) Token: 0x0600584F RID: 22607 RVA: 0x0025BBD8 File Offset: 0x00259DD8
				private bool PlayedBonusOpenAE { get; set; }

				// Token: 0x06005850 RID: 22608 RVA: 0x0025BBE4 File Offset: 0x00259DE4
				public MapPointBonus(Transform baseTr)
					: base(baseTr)
				{
					this.attrPairs = new Dictionary<SelEventCoopCtrl.AttributeType, PguiImageCtrl>
					{
						{
							SelEventCoopCtrl.AttributeType.RED,
							baseTr.Find("Atr01").GetComponent<PguiImageCtrl>()
						},
						{
							SelEventCoopCtrl.AttributeType.GREEN,
							baseTr.Find("Atr02").GetComponent<PguiImageCtrl>()
						},
						{
							SelEventCoopCtrl.AttributeType.BLUE,
							baseTr.Find("Atr03").GetComponent<PguiImageCtrl>()
						},
						{
							SelEventCoopCtrl.AttributeType.PINK,
							baseTr.Find("Atr04").GetComponent<PguiImageCtrl>()
						},
						{
							SelEventCoopCtrl.AttributeType.LIME,
							baseTr.Find("Atr05").GetComponent<PguiImageCtrl>()
						},
						{
							SelEventCoopCtrl.AttributeType.AQUA,
							baseTr.Find("Atr06").GetComponent<PguiImageCtrl>()
						}
					};
					this.Tex = baseTr.Find("Tex").GetComponent<PguiRawImageCtrl>();
					this.Tex.m_RawImage.material = new Material(Shader.Find("UI/Grayscale"));
					this.Num = baseTr.Find("TimeLimit/Num").GetComponent<PguiTextCtrl>();
					this.AEimage_BonusOpen = baseTr.Find("AEimage_BonusOpen").GetComponent<PguiAECtrl>();
					this.AEimage_BonusOpen.gameObject.SetActive(false);
					this.PlayedBonusOpenAE = false;
				}

				// Token: 0x06005851 RID: 22609 RVA: 0x0025BD08 File Offset: 0x00259F08
				public void Setup(int eventId)
				{
					DataManagerEvent.CoopData lastCoopInfo = DataManager.DmEvent.LastCoopInfo;
					bool flag = SelEventCoopCtrl.OpenedBonus();
					Material material = new Material(Shader.Find("UI/Grayscale"));
					foreach (KeyValuePair<SelEventCoopCtrl.AttributeType, int> keyValuePair in SelEventCoopCtrl.MapIdPairs)
					{
						if (lastCoopInfo.MapInfoMap.ContainsKey(keyValuePair.Value))
						{
							DataManagerEvent.CoopData.MapInfo mapInfo = lastCoopInfo.MapInfoMap[keyValuePair.Value];
							if (mapInfo != null && keyValuePair.Key != SelEventCoopCtrl.AttributeType.BONUS)
							{
								this.attrPairs[keyValuePair.Key].m_Image.material = (mapInfo.IsClear ? null : material);
							}
						}
					}
					int mapId = int.Parse(this.baseObj.name);
					DataManagerEvent.CoopConditionData coopConditionData = DataManager.DmEvent.GetCoopConditionDataList().Find((DataManagerEvent.CoopConditionData x) => x.MapId == mapId);
					if (!coopConditionData.TexturePath.IsNullOrEmpty())
					{
						this.Tex.SetRawImage("Texture2D/" + coopConditionData.TexturePath, true, false, null);
					}
					this.Tex.m_RawImage.material = (flag ? null : material);
					this.AEimage_BonusOpen.gameObject.SetActive(flag);
					this.Num.transform.parent.gameObject.SetActive(flag);
					if (flag)
					{
						DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(eventId);
						DateTime dateTime = new DateTime(lastCoopInfo.InfoGetDateTime.Year, lastCoopInfo.InfoGetDateTime.Month, lastCoopInfo.InfoGetDateTime.Day, eventData.ResetTime.Hour, eventData.ResetTime.Minute, eventData.ResetTime.Second);
						if (eventData.raidFlg)
						{
							DataManagerEvent.CoopRaidTermData nowTermData = DataManager.DmEvent.GetNowTermData(eventData.eventId);
							dateTime = new DateTime(lastCoopInfo.InfoGetDateTime.Year, lastCoopInfo.InfoGetDateTime.Month, lastCoopInfo.InfoGetDateTime.Day, nowTermData.endTime.Hours, nowTermData.endTime.Minutes, nowTermData.endTime.Seconds);
						}
						if (dateTime < lastCoopInfo.InfoGetDateTime)
						{
							dateTime = dateTime.AddDays(1.0);
						}
						this.Num.text = TimeManager.MakeTimeResidueText(TimeManager.Now, dateTime, false, true);
						if (!this.PlayedBonusOpenAE)
						{
							this.PlayedBonusOpenAE = true;
							this.AEimage_BonusOpen.PlayAnimation(PguiAECtrl.AmimeType.START, null);
							SoundManager.Play("prd_se_multievent_bonusquest_appearance", false, false);
						}
					}
				}

				// Token: 0x040063FC RID: 25596
				private Dictionary<SelEventCoopCtrl.AttributeType, PguiImageCtrl> attrPairs;

				// Token: 0x040063FD RID: 25597
				public PguiRawImageCtrl Tex;

				// Token: 0x040063FE RID: 25598
				public PguiTextCtrl Num;

				// Token: 0x040063FF RID: 25599
				public PguiAECtrl AEimage_BonusOpen;
			}

			// Token: 0x02001245 RID: 4677
			public class EventInfo
			{
				// Token: 0x06005852 RID: 22610 RVA: 0x0025BFC0 File Offset: 0x0025A1C0
				public EventInfo(Transform baseTr)
				{
					this.baseObj = baseTr.gameObject;
					this.Btn_Gacha = baseTr.Find("Btn_Gacha").GetComponent<PguiButtonCtrl>();
					this.Btn_ShopEvent = baseTr.Find("Btn_ShopEvent").GetComponent<PguiButtonCtrl>();
					this.ItemOwnBase = baseTr.Find("ItemOwnBase").GetComponent<PguiImageCtrl>();
					this.Icon_Stone = baseTr.Find("ItemOwnBase/Icon_Stone").GetComponent<PguiRawImageCtrl>();
					this.TotalItem_Num_Txt = baseTr.Find("TotalItem/Num_Txt").GetComponent<PguiTextCtrl>();
					this.TotalItem_Num_Txt.text = string.Format("{0}", 0);
					this.ItemOwnBase_Num_Txt = baseTr.Find("ItemOwnBase/Num_Txt").GetComponent<PguiTextCtrl>();
					this.TotalItem_Type_Txt = baseTr.Find("TotalItem/Txt").GetComponent<PguiTextCtrl>();
				}

				// Token: 0x06005853 RID: 22611 RVA: 0x0025C094 File Offset: 0x0025A294
				public void UpdateObject()
				{
					DataManager.DmEvent.GetCoopConditionDataList();
					DataManagerEvent.CoopData lastCoopInfo = DataManager.DmEvent.LastCoopInfo;
					long num = (long)lastCoopInfo.EventItemNum;
					if (DataManager.DmEvent.GetEventData(lastCoopInfo.EventId).raidFlg)
					{
						num = 0L;
						foreach (DataManagerEvent.CoopData.MapInfo mapInfo in lastCoopInfo.MapInfoMap.Values)
						{
							List<DataManagerEvent.CoopData.MapInfo.RankingInfo> rankingInfoList = mapInfo.RankingInfoList;
							rankingInfoList.Sort((DataManagerEvent.CoopData.MapInfo.RankingInfo a, DataManagerEvent.CoopData.MapInfo.RankingInfo b) => b.RankedTime.CompareTo(a.RankedTime));
							DataManagerEvent.CoopData.MapInfo.RankingInfo rankingInfo = ((rankingInfoList.Count == 0) ? null : rankingInfoList[0]);
							num += (long)((rankingInfo == null) ? 0 : rankingInfo.MyPoint);
						}
						this.TotalItem_Type_Txt.text = "ターム中のスコア";
					}
					this.TotalItem_Num_Txt.text = string.Format("{0}", num);
				}

				// Token: 0x04006401 RID: 25601
				public GameObject baseObj;

				// Token: 0x04006402 RID: 25602
				public PguiButtonCtrl Btn_Gacha;

				// Token: 0x04006403 RID: 25603
				public PguiButtonCtrl Btn_ShopEvent;

				// Token: 0x04006404 RID: 25604
				public PguiImageCtrl ItemOwnBase;

				// Token: 0x04006405 RID: 25605
				public PguiRawImageCtrl Icon_Stone;

				// Token: 0x04006406 RID: 25606
				public PguiTextCtrl TotalItem_Num_Txt;

				// Token: 0x04006407 RID: 25607
				public PguiTextCtrl ItemOwnBase_Num_Txt;

				// Token: 0x04006408 RID: 25608
				public PguiTextCtrl TotalItem_Type_Txt;
			}
		}

		// Token: 0x020011BC RID: 4540
		public class QuestSelect
		{
			// Token: 0x060056FB RID: 22267 RVA: 0x00255358 File Offset: 0x00253558
			public QuestSelect(Transform leftBaseTr, Transform rightBaseTr)
			{
				this.left = new SelEventCoopCtrl.GUI.QuestSelect.Left(leftBaseTr);
				this.right = new SelEventCoopCtrl.GUI.QuestSelect.Right(rightBaseTr);
			}

			// Token: 0x060056FC RID: 22268 RVA: 0x00255378 File Offset: 0x00253578
			public void SetActive(bool sw)
			{
				this.left.baseObj.SetActive(sw);
				this.right.baseObj.SetActive(sw);
			}

			// Token: 0x060056FD RID: 22269 RVA: 0x0025539C File Offset: 0x0025359C
			public float GetOffsetPosY()
			{
				return this.right.GetOffsetPosY();
			}

			// Token: 0x060056FE RID: 22270 RVA: 0x002553AC File Offset: 0x002535AC
			public void Setup(int mapId, int index, bool isRaid, bool isBonus = false)
			{
				DataManagerEvent.CoopData lastCoopInfo = DataManager.DmEvent.LastCoopInfo;
				if (lastCoopInfo.MapInfoMap.ContainsKey(mapId) && !isBonus)
				{
					DataManagerEvent.CoopData.MapInfo mapInfo = lastCoopInfo.MapInfoMap[mapId];
					bool isHardQuestOpen = mapInfo.IsHardQuestOpen;
					this.right.Normal.SetActive(!isHardQuestOpen);
					this.right.Hard.SetActive(isHardQuestOpen);
					if (isHardQuestOpen)
					{
						this.right.SetupHard(mapInfo.StaticCoopHardQuestData, mapId);
					}
					else
					{
						this.right.SetupNormal(mapInfo.StaticNextCoopConditionData, mapId);
					}
					this.left.Setup(mapId, index);
					this.right.SetupGauge(mapId, isRaid);
					this.right.SetupCharaIcon(mapId, isRaid);
					return;
				}
				this.SetActive(false);
			}

			// Token: 0x0400614C RID: 24908
			public SelEventCoopCtrl.GUI.QuestSelect.Left left;

			// Token: 0x0400614D RID: 24909
			public SelEventCoopCtrl.GUI.QuestSelect.Right right;

			// Token: 0x02001246 RID: 4678
			public class Left
			{
				// Token: 0x06005854 RID: 22612 RVA: 0x0025C19C File Offset: 0x0025A39C
				public Left(Transform baseTr)
				{
					this.baseObj = baseTr.gameObject;
					this.playerInfo = new SelEventCoopCtrl.GUI.PlayerInfo(baseTr.Find("AdjustmentGameObject/EventMulti_PlayerInfo"));
					this.playerInfo.baseObj.SetActive(false);
					this.HighScorePlayer = baseTr.Find("WindowBase/HighScorePlayer").gameObject;
					this.Ranking = baseTr.Find("WindowBase/Ranking").gameObject;
					this.Friend_ListBar_Friend = new SelFollowCtrl.GuiFriendBar(baseTr.Find("WindowBase/HighScorePlayer/Friend_ListBar_Friend"));
					this.Friend_ListBar_Friend.baseButton.GetComponent<Button>().enabled = false;
					this.Friend_ListBar_Friend.Txt_LastLogin.gameObject.SetActive(false);
					this.Friend_ListBar_Friend.Btn_Follow_L.gameObject.SetActive(false);
					this.Friend_ListBar_Friend.Btn_Follow_R.gameObject.SetActive(false);
					this.Friend_ListBar_Friend.Mark_Friend.gameObject.SetActive(false);
					this.Btn_Yaji_Left = baseTr.Find("WindowBase/Ranking/Btn_Yaji_Left").GetComponent<PguiButtonCtrl>();
					this.Btn_Yaji_Right = baseTr.Find("WindowBase/Ranking/Btn_Yaji_Right").GetComponent<PguiButtonCtrl>();
					this.Complete = baseTr.Find("WindowBase/Ranking/Complete").GetComponent<PguiImageCtrl>();
					this.Title = baseTr.Find("WindowBase/Ranking/Title").GetComponent<PguiTextCtrl>();
					this.MyScore_Txt = baseTr.Find("WindowBase/Ranking/MyScore/Txt").GetComponent<PguiTextCtrl>();
					this.ScrollView = baseTr.Find("WindowBase/Ranking/RankAll/ScrollView").GetComponent<ReuseScroll>();
					this.Blank = baseTr.Find("WindowBase/HighScorePlayer/Blank").gameObject;
					this.BlankRanking = baseTr.Find("WindowBase/Ranking/Blank").gameObject;
					this.ScrollBar = baseTr.Find("WindowBase/Ranking/RankAll/Scrollbar_Vertical").gameObject;
					this.Raid_Ranking = baseTr.Find("WindowBase/Raid_Ranking").gameObject;
					this.Raid_Btn_Yaji_Left = baseTr.Find("WindowBase/Raid_Ranking/Btn_Yaji_Left").GetComponent<PguiButtonCtrl>();
					this.Raid_Btn_Yaji_Right = baseTr.Find("WindowBase/Raid_Ranking/Btn_Yaji_Right").GetComponent<PguiButtonCtrl>();
					this.Raid_Complete = baseTr.Find("WindowBase/Raid_Ranking/Complete").GetComponent<PguiImageCtrl>();
					this.Raid_Title = baseTr.Find("WindowBase/Raid_Ranking/Title").GetComponent<PguiTextCtrl>();
					this.Raid_MyScore_Txt = baseTr.Find("WindowBase/Raid_Ranking/MyScore/Txt").GetComponent<PguiTextCtrl>();
					this.Raid_ScrollView = baseTr.Find("WindowBase/Raid_Ranking/RankAll/ScrollView").GetComponent<ReuseScroll>();
					this.Raid_ScrollBar = baseTr.Find("WindowBase/Raid_Ranking/RankAll/Scrollbar_Vertical").gameObject;
					this.Raid_BlankRanking = baseTr.Find("WindowBase/Raid_Ranking/Blank").gameObject;
				}

				// Token: 0x06005855 RID: 22613 RVA: 0x0025C420 File Offset: 0x0025A620
				public void Setup(int mapId, int index)
				{
					DataManagerEvent.CoopData lastCoopInfo = DataManager.DmEvent.LastCoopInfo;
					CoopPlayerInfo hardClearUser = lastCoopInfo.MapInfoMap[mapId].HardClearUser;
					this.Friend_ListBar_Friend.baseObj.SetActive(hardClearUser != null);
					this.Blank.SetActive(hardClearUser == null);
					if (hardClearUser != null)
					{
						CharaPackData charaPackData = CharaPackData.MakeInitial(hardClearUser.favorite_chara_id);
						this.Friend_ListBar_Friend.iconChara.SetupPrm(new IconCharaCtrl.SetupParam
						{
							cpd = charaPackData,
							iconId = hardClearUser.favorite_chara_face_id
						});
						this.Friend_ListBar_Friend.iconChara.DispRanking();
						this.Friend_ListBar_Friend.Txt_Comment.text = hardClearUser.comment;
						this.Friend_ListBar_Friend.Txt_FriendName.text = hardClearUser.user_name;
						this.Friend_ListBar_Friend.Num_Rank.ReplaceTextByDefault("Param01", string.Format("{0}", hardClearUser.user_level));
						this.Friend_ListBar_Friend.SetAttrIcon(0, false);
					}
					this.SetupRanking(mapId, index);
					int num = 0;
					if (lastCoopInfo.DispLogList.Count > 0)
					{
						this.playerInfo.Setup(lastCoopInfo.DispLogList, num, DataManager.DmEvent.GetEventData(lastCoopInfo.EventId).raidFlg);
					}
				}

				// Token: 0x06005856 RID: 22614 RVA: 0x0025C55C File Offset: 0x0025A75C
				public void ConvertRankingInRaid(bool isRaid)
				{
					this.Ranking.SetActive(!isRaid);
					this.HighScorePlayer.SetActive(!isRaid);
					this.Raid_Ranking.SetActive(isRaid);
					if (isRaid)
					{
						this.Btn_Yaji_Left = this.Raid_Btn_Yaji_Left;
						this.Btn_Yaji_Right = this.Raid_Btn_Yaji_Right;
						this.Complete = this.Raid_Complete;
						this.Title = this.Raid_Title;
						this.MyScore_Txt = this.Raid_MyScore_Txt;
						this.ScrollView = this.Raid_ScrollView;
						this.BlankRanking = this.Raid_BlankRanking;
					}
				}

				// Token: 0x06005857 RID: 22615 RVA: 0x0025C5EC File Offset: 0x0025A7EC
				public void SetupRanking(int mapId, int rankingListIndex)
				{
					DataManagerEvent.CoopData lastCoopInfo = DataManager.DmEvent.LastCoopInfo;
					DataManagerEvent.CoopData.MapInfo mapInfo = lastCoopInfo.MapInfoMap[mapId];
					List<DataManagerEvent.CoopData.MapInfo.RankingInfo> rankingInfoList = mapInfo.RankingInfoList;
					rankingInfoList.Sort((DataManagerEvent.CoopData.MapInfo.RankingInfo a, DataManagerEvent.CoopData.MapInfo.RankingInfo b) => b.RankedTime.CompareTo(a.RankedTime));
					if (rankingInfoList.Count >= 0)
					{
						DataManagerEvent.CoopData.MapInfo.RankingInfo rankingInfo = rankingInfoList[rankingListIndex];
						this.ScrollView.Resize(rankingInfo.UserRankingList.Count, 0);
						this.MyScore_Txt.ReplaceTextByDefault("Param01", string.Format("{0}", rankingInfo.MyPoint));
						this.Complete.gameObject.SetActive(mapInfo.IsClear && rankingInfo.UserRankingList.Count == 0 && rankingListIndex == 0);
						if (DataManager.DmEvent.GetEventData(lastCoopInfo.EventId).raidFlg)
						{
							this.Title.text = ((rankingListIndex == 0) ? "今回" : "前回") + "のランキング";
						}
						else
						{
							this.Title.text = rankingInfo.RankedTime.ToString("yyyy/MM/dd HH時") + " ランキング";
						}
						this.BlankRanking.gameObject.SetActive(!this.Complete.gameObject.activeSelf && rankingInfo.UserRankingList.Count == 0);
						this.ScrollBar.gameObject.SetActive(!this.Complete.gameObject.activeSelf && rankingInfo.UserRankingList.Count == 0);
						return;
					}
					this.ScrollView.Resize(0, 0);
					this.MyScore_Txt.text = string.Format("{0}", 0);
					this.Complete.gameObject.SetActive(false);
					this.Title.text = "";
					this.BlankRanking.gameObject.SetActive(true);
					this.ScrollBar.gameObject.SetActive(false);
				}

				// Token: 0x04006409 RID: 25609
				public GameObject baseObj;

				// Token: 0x0400640A RID: 25610
				public SelEventCoopCtrl.GUI.PlayerInfo playerInfo;

				// Token: 0x0400640B RID: 25611
				public GameObject Ranking;

				// Token: 0x0400640C RID: 25612
				public GameObject HighScorePlayer;

				// Token: 0x0400640D RID: 25613
				public SelFollowCtrl.GuiFriendBar Friend_ListBar_Friend;

				// Token: 0x0400640E RID: 25614
				public PguiButtonCtrl Btn_Yaji_Left;

				// Token: 0x0400640F RID: 25615
				public PguiButtonCtrl Btn_Yaji_Right;

				// Token: 0x04006410 RID: 25616
				public PguiImageCtrl Complete;

				// Token: 0x04006411 RID: 25617
				public PguiTextCtrl Title;

				// Token: 0x04006412 RID: 25618
				public PguiTextCtrl MyScore_Txt;

				// Token: 0x04006413 RID: 25619
				public ReuseScroll ScrollView;

				// Token: 0x04006414 RID: 25620
				public GameObject Blank;

				// Token: 0x04006415 RID: 25621
				public GameObject BlankRanking;

				// Token: 0x04006416 RID: 25622
				public GameObject ScrollBar;

				// Token: 0x04006417 RID: 25623
				public GameObject Raid_Ranking;

				// Token: 0x04006418 RID: 25624
				public PguiButtonCtrl Raid_Btn_Yaji_Left;

				// Token: 0x04006419 RID: 25625
				public PguiButtonCtrl Raid_Btn_Yaji_Right;

				// Token: 0x0400641A RID: 25626
				public PguiImageCtrl Raid_Complete;

				// Token: 0x0400641B RID: 25627
				public PguiTextCtrl Raid_Title;

				// Token: 0x0400641C RID: 25628
				public PguiTextCtrl Raid_MyScore_Txt;

				// Token: 0x0400641D RID: 25629
				public ReuseScroll Raid_ScrollView;

				// Token: 0x0400641E RID: 25630
				public GameObject Raid_ScrollBar;

				// Token: 0x0400641F RID: 25631
				public SelEventCoopCtrl.GUI.PlayerInfo Raid_playerInfo;

				// Token: 0x04006420 RID: 25632
				public GameObject Raid_BlankRanking;

				// Token: 0x02001259 RID: 4697
				public class ListBarPlayer
				{
					// Token: 0x0600588C RID: 22668 RVA: 0x0025D32C File Offset: 0x0025B52C
					public ListBarPlayer(Transform baseTr)
					{
						this.baseObj = baseTr.gameObject;
						this.Mark_Friend = baseTr.Find("BaseImage/Mark_Friend").GetComponent<PguiImageCtrl>();
						this.Mark_Friend.gameObject.SetActive(false);
						this.Rank_1 = baseTr.Find("Num_Rank/Rank_1").GetComponent<PguiImageCtrl>();
						this.Rank_2 = baseTr.Find("Num_Rank/Rank_2").GetComponent<PguiImageCtrl>();
						this.Rank_3 = baseTr.Find("Num_Rank/Rank_3").GetComponent<PguiImageCtrl>();
						this.Rank_4_10 = baseTr.Find("Num_Rank/Rank_4_10").GetComponent<PguiImageCtrl>();
						this.Rank_11_20 = baseTr.Find("Num_Rank/Rank_11_20").GetComponent<PguiImageCtrl>();
						this.Icon_Stone = baseTr.Find("BaseImage/ItemOwnBase/Icon_Stone").GetComponent<PguiRawImageCtrl>();
						this.Txt_FriendName = baseTr.Find("BaseImage/Txt_FriendName").GetComponent<PguiTextCtrl>();
						this.Num_Rank = baseTr.Find("BaseImage/Num_Rank").GetComponent<PguiTextCtrl>();
						this.Num_Txt = baseTr.Find("BaseImage/ItemOwnBase/Num_Txt").GetComponent<PguiTextCtrl>();
						this.Num_Rank_4_10 = baseTr.Find("Num_Rank/Rank_4_10/Num").GetComponent<PguiTextCtrl>();
						this.Num_Rank_11_20 = baseTr.Find("Num_Rank/Rank_11_20/Num").GetComponent<PguiTextCtrl>();
						this.IconChara = baseTr.Find("BaseImage/Icon_Chara/Icon_Chara").GetComponent<IconCharaCtrl>();
						this.Achievement = baseTr.Find("BaseImage/Achievement").GetComponent<AchievementCtrl>();
					}

					// Token: 0x0600588D RID: 22669 RVA: 0x0025D490 File Offset: 0x0025B690
					public void SetupRanking(int ranking)
					{
						this.Rank_1.gameObject.SetActive(ranking == 1);
						this.Rank_2.gameObject.SetActive(ranking == 2);
						this.Rank_3.gameObject.SetActive(ranking == 3);
						this.Rank_4_10.gameObject.SetActive(4 <= ranking && ranking <= 10);
						this.Num_Rank_4_10.text = string.Format("{0}", ranking);
						this.Rank_11_20.gameObject.SetActive(ranking >= 11);
						this.Num_Rank_11_20.text = string.Format("{0}", ranking);
					}

					// Token: 0x04006466 RID: 25702
					public GameObject baseObj;

					// Token: 0x04006467 RID: 25703
					public PguiImageCtrl Mark_Friend;

					// Token: 0x04006468 RID: 25704
					public PguiImageCtrl Rank_1;

					// Token: 0x04006469 RID: 25705
					public PguiImageCtrl Rank_2;

					// Token: 0x0400646A RID: 25706
					public PguiImageCtrl Rank_3;

					// Token: 0x0400646B RID: 25707
					public PguiImageCtrl Rank_4_10;

					// Token: 0x0400646C RID: 25708
					public PguiImageCtrl Rank_11_20;

					// Token: 0x0400646D RID: 25709
					public PguiRawImageCtrl Icon_Stone;

					// Token: 0x0400646E RID: 25710
					public PguiTextCtrl Txt_FriendName;

					// Token: 0x0400646F RID: 25711
					public PguiTextCtrl Num_Rank;

					// Token: 0x04006470 RID: 25712
					public PguiTextCtrl Num_Txt;

					// Token: 0x04006471 RID: 25713
					public PguiTextCtrl Num_Rank_4_10;

					// Token: 0x04006472 RID: 25714
					public PguiTextCtrl Num_Rank_11_20;

					// Token: 0x04006473 RID: 25715
					public IconCharaCtrl IconChara;

					// Token: 0x04006474 RID: 25716
					public AchievementCtrl Achievement;
				}
			}

			// Token: 0x02001247 RID: 4679
			public class Right
			{
				// Token: 0x06005858 RID: 22616 RVA: 0x0025C7F4 File Offset: 0x0025A9F4
				public Right(Transform baseTr)
				{
					this.baseObj = baseTr.gameObject;
					this.All = baseTr.Find("All").gameObject;
					this.Normal = this.All.transform.Find("Normal").gameObject;
					this.NormalContents = this.Normal.transform.Find("Contents").gameObject;
					this.Num_Txt = this.NormalContents.transform.Find("Next/Num_Txt").GetComponent<PguiTextCtrl>();
					this.Btn_GetItem = this.NormalContents.transform.Find("Btn_GetItem").GetComponent<PguiButtonCtrl>();
					this.Icon_Stone = this.NormalContents.transform.Find("Next/Icon_Stone").GetComponent<PguiRawImageCtrl>();
					this.Hard = this.All.transform.Find("Hard").gameObject;
					this.Contents = this.Hard.transform.Find("Contents_Base/Contents").GetComponent<HorizontalLayoutGroup>();
					this.Contents_Txt01 = this.NormalContents.transform.Find("Next/Base/Txt_01").GetComponent<PguiTextCtrl>();
					this.Contents_Txt02 = this.Contents.transform.Find("Txt02").GetComponent<PguiTextCtrl>();
					this.Contents_Txt04 = this.Contents.transform.Find("Txt04").GetComponent<PguiTextCtrl>();
					this.Gauge = this.All.transform.Find("GageAll/Gage").GetComponent<PguiImageCtrl>();
					this.Txt_Complete = this.All.transform.Find("GageAll/Txt_Complete").gameObject;
					this.Txt_Difficulty = this.All.transform.Find("GageAll/Lv").GetComponent<PguiTextCtrl>();
					this.LvBase = this.All.transform.Find("GageAll/LvBase").gameObject;
					this.Title = this.Normal.transform.Find("Base01/Title").GetComponent<PguiTextCtrl>();
					this.CharaIcon = this.All.transform.Find("CharaIcon/Texture").GetComponent<PguiRawImageCtrl>();
				}

				// Token: 0x06005859 RID: 22617 RVA: 0x0025CA2C File Offset: 0x0025AC2C
				public DataManagerEvent.CoopConditionData GetNextCoopConditionData(DataManagerEvent.CoopData.MapInfo mapInfo)
				{
					DataManagerEvent.CoopConditionData coopConditionData = null;
					foreach (DataManagerEvent.CoopConditionData coopConditionData2 in mapInfo.MapRewardConditionalDataList)
					{
						if (coopConditionData2.AchievementCondition > mapInfo.TotalPoint)
						{
							coopConditionData = coopConditionData2;
							break;
						}
					}
					return coopConditionData;
				}

				// Token: 0x0600585A RID: 22618 RVA: 0x0025CA90 File Offset: 0x0025AC90
				public void SetupNormal(DataManagerEvent.CoopConditionData coopConditionData, int mapId)
				{
					DataManagerEvent.CoopData.MapInfo mapInfo = DataManager.DmEvent.LastCoopInfo.MapInfoMap[mapId];
					DataManagerEvent.CoopConditionData nextCoopConditionData = this.GetNextCoopConditionData(mapInfo);
					DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(mapInfo.EventId);
					string text = "";
					if (nextCoopConditionData != null)
					{
						text = (eventData.raidFlg ? string.Format("{0}", nextCoopConditionData.AchievementCondition - mapInfo.TotalPoint) : string.Format("{0}", Mathf.Abs((float)(nextCoopConditionData.AchievementCondition - mapInfo.TotalPoint))));
					}
					this.Num_Txt.text = ((nextCoopConditionData != null) ? text : (eventData.raidFlg ? "0" : "-"));
					this.Icon_Stone.gameObject.SetActive(nextCoopConditionData != null && !eventData.raidFlg);
					if (nextCoopConditionData != null && !eventData.raidFlg)
					{
						this.Icon_Stone.SetRawImage(DataManager.DmItem.GetItemStaticBase(nextCoopConditionData.AchievementItem.itemId).GetIconName(), true, false, null);
					}
				}

				// Token: 0x0600585B RID: 22619 RVA: 0x0025CB98 File Offset: 0x0025AD98
				public void SetupHard(DataManagerEvent.CoopHardQuestData coopHardData, int mapId)
				{
					DataManagerEvent.CoopData.MapInfo mapInfo = DataManager.DmEvent.LastCoopInfo.MapInfoMap[mapId];
					this.Contents_Txt02.ReplaceTextByDefault("Param01", string.Format("{0}/{1}", mapInfo.HardQuestClearNum, coopHardData.AchievementCondition));
					string text = TimeManager.MakeTimeResidueText(TimeManager.Now, coopHardData.EndDatetime, false, true);
					text = text.Replace("あと", "");
					this.Contents_Txt04.ReplaceTextByDefault("Param01", text ?? "");
					DataManagerEvent.CoopConditionData nextCoopConditionData = this.GetNextCoopConditionData(mapInfo);
					this.Icon_Stone.gameObject.SetActive(nextCoopConditionData != null && !DataManager.DmEvent.GetEventData(nextCoopConditionData.EventId).raidFlg);
					if (nextCoopConditionData != null && !DataManager.DmEvent.GetEventData(nextCoopConditionData.EventId).raidFlg)
					{
						this.Icon_Stone.SetRawImage(DataManager.DmItem.GetItemStaticBase(nextCoopConditionData.AchievementItem.itemId).GetIconName(), true, false, null);
					}
					Singleton<SceneManager>.Instance.StartCoroutine(this.StimulateLayoutGroupObject());
				}

				// Token: 0x0600585C RID: 22620 RVA: 0x0025CCB2 File Offset: 0x0025AEB2
				private IEnumerator StimulateLayoutGroupObject()
				{
					yield return null;
					this.Contents.enabled = false;
					yield return null;
					this.Contents.enabled = true;
					yield break;
				}

				// Token: 0x0600585D RID: 22621 RVA: 0x0025CCC4 File Offset: 0x0025AEC4
				public void SetupGauge(int mapId, bool isRaid)
				{
					DataManagerEvent.CoopData.MapInfo mapInfo = DataManager.DmEvent.LastCoopInfo.MapInfoMap[mapId];
					float num = mapInfo.ProgressRate;
					if (isRaid)
					{
						num = 1f - mapInfo.ProgressRate;
					}
					if (num > 1f)
					{
						num = 1f;
					}
					if (num < 0f)
					{
						num = 0f;
					}
					this.Gauge.m_Image.fillAmount = num;
					this.Txt_Complete.gameObject.SetActive(mapInfo.IsClear);
					this.Txt_Difficulty.text = mapInfo.StaticNextCoopConditionData.LevelName;
				}

				// Token: 0x0600585E RID: 22622 RVA: 0x0025CD58 File Offset: 0x0025AF58
				public void SetupCharaIcon(int mapId, bool isRaid)
				{
					this.CharaIcon.gameObject.SetActive(isRaid);
					string texturePath = DataManager.DmEvent.GetCoopConditionDataList().Find((DataManagerEvent.CoopConditionData item) => item.MapId == mapId).TexturePath;
					this.CharaIcon.SetRawImage("Texture2D/" + texturePath, true, false, null);
				}

				// Token: 0x0600585F RID: 22623 RVA: 0x0025CDBD File Offset: 0x0025AFBD
				public float GetOffsetPosY()
				{
					return (this.All.transform as RectTransform).sizeDelta.y;
				}

				// Token: 0x06005860 RID: 22624 RVA: 0x0025CDDC File Offset: 0x0025AFDC
				public void ConvertPrefabToRaid(bool isRaid)
				{
					this.Icon_Stone.gameObject.SetActive(!isRaid);
					this.Btn_GetItem.gameObject.SetActive(!isRaid);
					this.LvBase.SetActive(!isRaid);
					this.Txt_Difficulty.gameObject.SetActive(!isRaid);
					this.CharaIcon.gameObject.SetActive(isRaid);
					this.Title.text = (isRaid ? "拠点ボスのHP" : "今日の拠点もくひょう");
					this.Contents_Txt01.text = (isRaid ? "残りHP" : "次の全体報酬まで");
				}

				// Token: 0x04006421 RID: 25633
				public GameObject baseObj;

				// Token: 0x04006422 RID: 25634
				public GameObject All;

				// Token: 0x04006423 RID: 25635
				public GameObject Normal;

				// Token: 0x04006424 RID: 25636
				public PguiTextCtrl Num_Txt;

				// Token: 0x04006425 RID: 25637
				public PguiButtonCtrl Btn_GetItem;

				// Token: 0x04006426 RID: 25638
				public GameObject Hard;

				// Token: 0x04006427 RID: 25639
				public PguiTextCtrl Contents_Txt01;

				// Token: 0x04006428 RID: 25640
				public PguiTextCtrl Contents_Txt02;

				// Token: 0x04006429 RID: 25641
				public PguiTextCtrl Contents_Txt04;

				// Token: 0x0400642A RID: 25642
				public PguiImageCtrl Gauge;

				// Token: 0x0400642B RID: 25643
				public GameObject Txt_Complete;

				// Token: 0x0400642C RID: 25644
				public PguiRawImageCtrl Icon_Stone;

				// Token: 0x0400642D RID: 25645
				public PguiTextCtrl Txt_Difficulty;

				// Token: 0x0400642E RID: 25646
				public HorizontalLayoutGroup Contents;

				// Token: 0x0400642F RID: 25647
				public PguiTextCtrl Title;

				// Token: 0x04006430 RID: 25648
				public GameObject NormalContents;

				// Token: 0x04006431 RID: 25649
				public GameObject LvBase;

				// Token: 0x04006432 RID: 25650
				public PguiRawImageCtrl CharaIcon;
			}
		}
	}
}
