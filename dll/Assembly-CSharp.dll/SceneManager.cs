using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000E9 RID: 233
public class SceneManager : Singleton<SceneManager>
{
	// Token: 0x06000A83 RID: 2691 RVA: 0x0003C999 File Offset: 0x0003AB99
	public static void CanvasSetActive(SceneManager.CanvasType type, bool isActive)
	{
		if (Singleton<SceneManager>.Instance.baseCanvasMap.ContainsKey(type))
		{
			Singleton<SceneManager>.Instance.baseCanvasMap[type].gameObject.SetActive(isActive);
		}
	}

	// Token: 0x17000294 RID: 660
	// (get) Token: 0x06000A84 RID: 2692 RVA: 0x0003C9C8 File Offset: 0x0003ABC8
	public SceneManager.SceneName CurrentSceneName
	{
		get
		{
			if (this.currentScene == null)
			{
				return SceneManager.SceneName.None;
			}
			return this.currentScene.sceneName;
		}
	}

	// Token: 0x17000295 RID: 661
	// (get) Token: 0x06000A85 RID: 2693 RVA: 0x0003C9DF File Offset: 0x0003ABDF
	public SceneManager.SceneName RequestNextScene
	{
		get
		{
			return this.requestNextScene;
		}
	}

	// Token: 0x17000296 RID: 662
	// (get) Token: 0x06000A86 RID: 2694 RVA: 0x0003C9E7 File Offset: 0x0003ABE7
	public BaseScene CurrentScene
	{
		get
		{
			if (this.currentScene == null)
			{
				return null;
			}
			return this.currentScene.baseScene;
		}
	}

	// Token: 0x17000297 RID: 663
	// (get) Token: 0x06000A87 RID: 2695 RVA: 0x0003C9FE File Offset: 0x0003ABFE
	public bool IsSceneChange
	{
		get
		{
			return this.sceneStatus > SceneManager.SceneStatus.POLLING_REQUEST;
		}
	}

	// Token: 0x17000298 RID: 664
	// (get) Token: 0x06000A88 RID: 2696 RVA: 0x0003CA09 File Offset: 0x0003AC09
	// (set) Token: 0x06000A89 RID: 2697 RVA: 0x0003CA11 File Offset: 0x0003AC11
	public bool LockByEnableSceneWaitFromEnableScene { get; set; }

	// Token: 0x06000A8A RID: 2698 RVA: 0x0003CA1C File Offset: 0x0003AC1C
	public static int[] GetOption()
	{
		string[] array = PlayerPrefs.GetString(SceneManager.optionKey).Split(',', StringSplitOptions.None);
		int[] array2 = new int[] { 0, 2, 6, 6, 6 };
		int num = 0;
		while (num < array.Length && num < array2.Length)
		{
			int num2 = 0;
			if (int.TryParse(array[num], out num2))
			{
				array2[num] = num2;
			}
			num++;
		}
		return array2;
	}

	// Token: 0x06000A8B RID: 2699 RVA: 0x0003CA78 File Offset: 0x0003AC78
	public static void SetOption(int[] opt)
	{
		string text = "";
		for (int i = 0; i < opt.Length; i++)
		{
			if (i > 0)
			{
				text += ",";
			}
			text += opt[i].ToString();
		}
		PlayerPrefs.SetString(SceneManager.optionKey, text);
	}

	// Token: 0x06000A8C RID: 2700 RVA: 0x0003CAC8 File Offset: 0x0003ACC8
	public static void InitializeOption()
	{
		int[] option = SceneManager.GetOption();
		SceneManager.screenSize = new Resolution
		{
			width = Screen.width,
			height = Screen.height
		};
		SceneManager.safeArea = SafeAreaScaler.GetSafeArea();
		Singleton<CanvasManager>.Instance.SetDisplayDirection(option[0], ScreenOrientation.LandscapeLeft);
		UserOptionData.SetDisplayQuality(option[1], new Vector2Int(SceneManager.screenSize.width, SceneManager.screenSize.height));
		SoundManager.SetCategoryVolume(UserOptionData.GetVolumeList(option[2], option[3], option[4]));
	}

	// Token: 0x06000A8D RID: 2701 RVA: 0x0003CB4D File Offset: 0x0003AD4D
	private void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus)
		{
			return;
		}
		if (SceneManager.screenSize.width == 0)
		{
			return;
		}
		this.returnBackground = true;
	}

	// Token: 0x06000A8E RID: 2702 RVA: 0x0003CB67 File Offset: 0x0003AD67
	public void Initialize()
	{
		this.sceneRenderSetting = base.gameObject.AddComponent<RenderSettingParam>();
		this.sceneRenderSetting.Scene2Param();
		this.sceneStatus = SceneManager.SceneStatus.POLLING_REQUEST;
		this.SetNextScene(SceneManager.SceneName.SceneBoot, null);
		this.LockByEnableSceneWaitFromEnableScene = false;
	}

	// Token: 0x06000A8F RID: 2703 RVA: 0x0003CB9B File Offset: 0x0003AD9B
	public void SetNextScene(SceneManager.SceneName nextscene, object args)
	{
		if (this.sceneStatus == SceneManager.SceneStatus.POLLING_REQUEST)
		{
			this.requestNextScene = nextscene;
			this.requestNextSceneArgs = args;
			this.sceneStatus = SceneManager.SceneStatus.REQUEST_NEXT_SCENE;
		}
	}

	// Token: 0x06000A90 RID: 2704 RVA: 0x0003CBBA File Offset: 0x0003ADBA
	public void SetSceneReboot()
	{
		Manager.PauseCmdExit();
		if (this.changeSceneEnumerator != null)
		{
			this.changeSceneEnumerator = null;
		}
		this.currentScene = null;
		this.requestNextScene = SceneManager.SceneName.SceneDataInitialize;
		this.sceneStatus = SceneManager.SceneStatus.REQUEST_NEXT_SCENE;
	}

	// Token: 0x06000A91 RID: 2705 RVA: 0x0003CBE8 File Offset: 0x0003ADE8
	public void DestroyAliveScene()
	{
		foreach (SceneManager.SceneData sceneData in this.aliveSceneMap.Values)
		{
			if (sceneData != this.currentScene)
			{
				sceneData.baseScene.OnDestroyScene();
			}
		}
		this.aliveSceneMap.Clear();
		if (this.currentScene != null)
		{
			this.aliveSceneMap[this.currentScene.sceneName] = this.currentScene;
		}
	}

	// Token: 0x06000A92 RID: 2706 RVA: 0x0003CC7C File Offset: 0x0003AE7C
	public void Update()
	{
		if (this.changeSceneEnumerator != null && !this.changeSceneEnumerator.MoveNext())
		{
			this.changeSceneEnumerator = null;
		}
		SceneManager.SceneStatus sceneStatus = this.sceneStatus;
		if (sceneStatus != SceneManager.SceneStatus.POLLING_REQUEST)
		{
			if (sceneStatus == SceneManager.SceneStatus.REQUEST_NEXT_SCENE)
			{
				this.sceneStatus = SceneManager.SceneStatus.CHANGING_SCENE;
				this.changeSceneEnumerator = this.ChangeScene(this.requestNextScene, this.requestNextSceneArgs);
			}
		}
		else if (this.currentScene != null)
		{
			this.currentScene.baseScene.Update();
		}
		if (this.returnBackground)
		{
			this.returnBackground = false;
			int[] option = SceneManager.GetOption();
			Singleton<CanvasManager>.Instance.SetDisplayDirection(option[0], ScreenOrientation.LandscapeLeft);
			if (SceneHome.nowVertView)
			{
				UserOptionData.SetDisplayQuality(option[1], new Vector2Int(SceneManager.screenSize.height, SceneManager.screenSize.width));
			}
			else
			{
				UserOptionData.SetDisplayQuality(option[1], new Vector2Int(SceneManager.screenSize.width, SceneManager.screenSize.height));
			}
			SoundManager.SetCategoryVolume(UserOptionData.GetVolumeList(option[2], option[3], option[4]));
		}
	}

	// Token: 0x06000A93 RID: 2707 RVA: 0x0003CD6D File Offset: 0x0003AF6D
	public void LateUpdate()
	{
		if (this.sceneStatus == SceneManager.SceneStatus.POLLING_REQUEST && this.currentScene != null)
		{
			this.currentScene.baseScene.LateUpdate();
		}
	}

	// Token: 0x06000A94 RID: 2708 RVA: 0x0003CD8F File Offset: 0x0003AF8F
	private IEnumerator ChangeScene(SceneManager.SceneName nextScene, object args)
	{
		bool isFadeEnable = this.currentScene != null;
		CanvasManager.FadeType fadeType = CanvasManager.FadeType.TIPS;
		if (this.currentScene == null || this.currentScene.baseScene.GetType() != typeof(SceneTitle))
		{
			foreach (SceneManager.FadeDispPattern fadeDispPattern in SceneManager.fadeDispPatternList)
			{
				if ((fadeDispPattern.preScene == null || (this.currentScene != null && this.currentScene.baseScene.GetType() == fadeDispPattern.preScene)) && (fadeDispPattern.nextScene == null || Type.GetType(nextScene.ToString()) == fadeDispPattern.nextScene))
				{
					isFadeEnable = fadeDispPattern.isFade;
					fadeType = fadeDispPattern.fadeType;
					if (fadeType == CanvasManager.FadeType.BATTLE_LOSE)
					{
						if (DataManager.DmUserInfo.tutorialSequence > TutorialUtil.Sequence.INVALID && DataManager.DmUserInfo.tutorialSequence < TutorialUtil.Sequence.END)
						{
							fadeType = CanvasManager.FadeType.BATTLE_END;
							break;
						}
						if (SceneBattle.pvpBattle || SceneBattle.friendWild)
						{
							fadeType = CanvasManager.FadeType.TIPS;
							break;
						}
						break;
					}
					else if (fadeType == CanvasManager.FadeType.BATTLE_END)
					{
						if (nextScene != SceneManager.SceneName.SceneScenario)
						{
							break;
						}
						SceneScenario.Args args2 = args as SceneScenario.Args;
						if (args2 != null && args2.storyType == 1)
						{
							fadeType = CanvasManager.FadeType.SCENARIO;
							break;
						}
						break;
					}
					else if (fadeType == CanvasManager.FadeType.SCENARIO2)
					{
						if (nextScene != SceneManager.SceneName.SceneScenario || this.currentScene.sceneName != SceneManager.SceneName.SceneScenario)
						{
							break;
						}
						SceneScenario.Args args3 = args as SceneScenario.Args;
						if (args3 == null || args3.storyType != 1)
						{
							break;
						}
						SceneScenario sceneScenario = this.currentScene.baseScene as SceneScenario;
						if (sceneScenario != null && sceneScenario.GetStoryType() == 2 && args3.questId == sceneScenario.GetQuestId())
						{
							fadeType = CanvasManager.FadeType.SCENARIO;
							break;
						}
						break;
					}
					else
					{
						if (!isFadeEnable && fadeType == CanvasManager.FadeType.NORMAL && nextScene == SceneManager.SceneName.SceneQuest && args == null)
						{
							isFadeEnable = true;
							fadeType = CanvasManager.FadeType.TIPS;
							break;
						}
						break;
					}
				}
			}
		}
		if (this.currentScene != null)
		{
			this.currentScene.baseScene.OnStopControl();
			if (isFadeEnable && (this.currentScene.baseScene.GetType() != typeof(SceneTitle) || !CanvasManager.IsFadeOut))
			{
				yield return null;
				CanvasManager.RequestFade(fadeType);
			}
			while (!CanvasManager.IsFinishFadeAction)
			{
				this.currentScene.baseScene.OnStopControlFadeWait();
				yield return null;
			}
			if (Singleton<CanvasManager>.Instance != null && Singleton<CanvasManager>.Instance.IsInitialized)
			{
				if (isFadeEnable && fadeType == CanvasManager.FadeType.TIPS)
				{
					if (!CanvasManager.HdlLoadAndTipsCtrl.isActive())
					{
						SceneManager.<>c__DisplayClass48_1 CS$<>8__locals2 = new SceneManager.<>c__DisplayClass48_1();
						CS$<>8__locals2.dispTips = false;
						CanvasManager.SwitchLayerHdlLoadAndTipsCtrl(SceneManager.CanvasType.OVERLAY);
						CanvasManager.HdlLoadAndTipsCtrl.Setup(new LoadAndTipsCtrl.SetupParam
						{
							dispTipsId = 0,
							isDispTips = true,
							cbTipsDispFinish = delegate
							{
								CS$<>8__locals2.dispTips = true;
							},
							prevSceneName = this.currentScene.sceneName,
							nextSceneName = nextScene,
							nextSceneArgs = args
						});
						while (!CS$<>8__locals2.dispTips)
						{
							yield return null;
						}
						CS$<>8__locals2 = null;
					}
				}
				else if (CanvasManager.HdlLoadAndTipsCtrl.isActive())
				{
					CanvasManager.SwitchLayerHdlLoadAndTipsCtrl(SceneManager.CanvasType.SYSTEM);
					CanvasManager.HdlLoadAndTipsCtrl.Close(true);
				}
			}
			CanvasManager.HdlOpenWindowSortFilter.RequestActionUpdateSortType();
			SortFilterManager.RequestUpdateSortTypeData();
			DataManagerGameStatus.UserFlagData userFlagData = DataManager.DmGameStatus.MakeUserFlagData();
			if (((userFlagData != null) ? userFlagData.GachaNewInfoData : null) != null && DataManager.DmGacha.SelectedGachaIdHashSet != null && userFlagData.GachaNewInfoData.RegisterIDs(DataManager.DmGacha.SelectedGachaIdHashSet))
			{
				DataManager.DmGameStatus.RequestActionUpdateUserFlag(userFlagData);
				while (DataManager.IsServerRequesting())
				{
					yield return null;
				}
			}
			if (SceneManager.EnableUploadKemoStatusSceneList.Contains(nextScene))
			{
				DataManager.DmChara.RequestUpdateTotalKemoStatus();
				while (DataManager.IsServerRequesting())
				{
					yield return null;
				}
			}
			this.currentScene.baseScene.OnDisableScene();
			while (!this.currentScene.baseScene.OnDisableSceneWait())
			{
				yield return null;
			}
			if (SceneManager.DisableDestorySceneList.Contains(this.currentScene.sceneName))
			{
				this.currentScene.baseScene.OnDestroyScene();
				this.aliveSceneMap.Remove(this.currentScene.sceneName);
			}
			if (null != CanvasManager.HdlCmnMenu)
			{
				CanvasManager.HdlCmnMenu.IsSceneManagerMoving = false;
			}
			if (nextScene == SceneManager.SceneName.SceneTitle)
			{
				SceneManager.CanvasSetActive(SceneManager.CanvasType.PRESET, false);
			}
		}
		if (SceneManager.DestorySceneList.ContainsKey(nextScene))
		{
			foreach (SceneManager.SceneName sceneName in SceneManager.DestorySceneList[nextScene])
			{
				SceneManager.SceneData sceneData = (this.aliveSceneMap.ContainsKey(sceneName) ? this.aliveSceneMap[sceneName] : null);
				if (sceneData != null)
				{
					sceneData.baseScene.OnDestroyScene();
					this.aliveSceneMap.Remove(sceneData.sceneName);
				}
			}
		}
		PrjUtil.ReleaseMemory(isFadeEnable ? PrjUtil.Garbagecollection : PrjUtil.UnloadUnused);
		if (isFadeEnable)
		{
			yield return null;
			yield return null;
		}
		PlayerPrefs.Save();
		this.sceneRenderSetting.Param2Scene();
		if (Singleton<CanvasManager>.Instance != null && Singleton<CanvasManager>.Instance.IsInitialized && DataManager.DmServerMst.mstHelpDataList != null)
		{
			CanvasManager.HdlHelpWindowCtrl.SetCurrentOpenHelpId(-1);
		}
		bool isOpenInfo = false;
		string url = this.GetSealedUrl(nextScene);
		if (Singleton<DataManager>.Instance != null && SceneManager.CheckSeal(nextScene, this.requestNextSceneArgs))
		{
			if (Singleton<CanvasManager>.Instance != null && Singleton<CanvasManager>.Instance.IsInitialized)
			{
				bool isFinish = false;
				string sealedText = this.GetSealedText(nextScene);
				List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>> list = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>();
				list.Clear();
				list.Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, "閉じる"));
				if (!string.IsNullOrEmpty(url))
				{
					list.Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, "お知らせ"));
				}
				CanvasManager.HdlOpenWindowServerError.Setup("", string.IsNullOrEmpty(sealedText) ? "現在ご利用いただけません\nホームに戻ります" : sealedText, list, false, delegate(int idx)
				{
					if (idx == 1)
					{
						isOpenInfo = true;
					}
					isFinish = true;
					return true;
				}, null, false);
				CanvasManager.HdlOpenWindowServerError.Open();
				while (!isFinish)
				{
					yield return null;
				}
			}
			nextScene = SceneManager.SceneName.SceneHome;
		}
		Input.multiTouchEnabled = SceneManager.EnableMultiTouchSceneList.Contains(nextScene);
		while (CanvasManager.IsTouchWaitFadeAction)
		{
			yield return null;
		}
		DateTime dt = TimeManager.SystemNow;
		this.currentScene = null;
		if (this.aliveSceneMap.ContainsKey(nextScene))
		{
			this.currentScene = this.aliveSceneMap[nextScene];
		}
		if (this.currentScene == null)
		{
			BaseScene baseScene = (BaseScene)Activator.CreateInstance(Type.GetType(nextScene.ToString()));
			this.currentScene = new SceneManager.SceneData(nextScene, baseScene);
			this.aliveSceneMap[nextScene] = this.currentScene;
			List<string> list2 = this.currentScene.baseScene.FirstLoadResourcesList();
			if (list2 != null)
			{
				foreach (string text in list2)
				{
					ResourceRequest rr = Resources.LoadAsync(text);
					while (rr != null && !rr.isDone)
					{
						yield return null;
					}
					rr = null;
				}
				List<string>.Enumerator enumerator3 = default(List<string>.Enumerator);
			}
			this.currentScene.baseScene.OnCreateScene();
			while (!this.currentScene.baseScene.OnCreateSceneWait())
			{
				this.CheckLoadingDisp(dt);
				yield return null;
			}
		}
		this.LockByEnableSceneWaitFromEnableScene = false;
		this.currentScene.baseScene.OnEnableScene(this.requestNextSceneArgs);
		while (!this.currentScene.baseScene.OnEnableSceneWait())
		{
			this.CheckLoadingDisp(dt);
			yield return null;
		}
		while (this.LockByEnableSceneWaitFromEnableScene && isFadeEnable)
		{
			this.CheckLoadingDisp(dt);
			yield return null;
		}
		if (isFadeEnable)
		{
			yield return null;
			CanvasManager.RestartFade();
		}
		if (Singleton<CanvasManager>.Instance != null && Singleton<CanvasManager>.Instance.IsInitialized && CanvasManager.HdlLoadAndTipsCtrl.isActive())
		{
			CanvasManager.SwitchLayerHdlLoadAndTipsCtrl(SceneManager.CanvasType.SYSTEM);
			CanvasManager.HdlLoadAndTipsCtrl.Close(true);
		}
		this.currentScene.baseScene.OnStartSceneFade();
		while (!CanvasManager.IsFinishFadeAction)
		{
			this.currentScene.baseScene.OnStartSceneFadeWait();
			yield return null;
		}
		this.currentScene.baseScene.OnStartControl();
		if (isOpenInfo)
		{
			CanvasManager.HdlWebViewWindowCtrl.Open(url);
		}
		this.sceneStatus = SceneManager.SceneStatus.POLLING_REQUEST;
		yield break;
		yield break;
	}

	// Token: 0x06000A95 RID: 2709 RVA: 0x0003CDAC File Offset: 0x0003AFAC
	private void CheckLoadingDisp(DateTime dt)
	{
		if (Singleton<CanvasManager>.Instance != null && Singleton<CanvasManager>.Instance.IsInitialized && !CanvasManager.HdlLoadAndTipsCtrl.isActive() && (TimeManager.SystemNow - dt).TotalSeconds > 1.0)
		{
			CanvasManager.SwitchLayerHdlLoadAndTipsCtrl(SceneManager.CanvasType.OVERLAY);
			CanvasManager.HdlLoadAndTipsCtrl.Setup(new LoadAndTipsCtrl.SetupParam());
		}
	}

	// Token: 0x06000A96 RID: 2710 RVA: 0x0003CE14 File Offset: 0x0003B014
	private static bool CheckSeal(SceneManager.SceneName nextScene, object requestNextSceneArgs)
	{
		bool flag = false;
		HomeCheckResult homeCheckResult = DataManager.DmHome.GetHomeCheckResult();
		Sealed @sealed = ((homeCheckResult != null) ? homeCheckResult.sealedData : null) ?? new Sealed();
		if (nextScene <= SceneManager.SceneName.ScenePvp)
		{
			switch (nextScene)
			{
			case SceneManager.SceneName.SceneBattleSelector:
			{
				SceneBattleSelector.Args args = requestNextSceneArgs as SceneBattleSelector.Args;
				if (args != null && args.selectQuestOneId != 0)
				{
					QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(args.selectQuestOneId);
					if (questOnePackData != null)
					{
						flag = SceneManager.CheckSealByQuestCategory(questOnePackData.questChapter.category, @sealed);
					}
				}
				break;
			}
			case SceneManager.SceneName.SceneGacha:
				flag = @sealed.menu_gacha == 1;
				break;
			case SceneManager.SceneName.SceneHome:
				break;
			case SceneManager.SceneName.SceneQuest:
			{
				SceneQuest.Args args2 = requestNextSceneArgs as SceneQuest.Args;
				if (args2 != null && args2.selectQuestOneId != 0)
				{
					QuestOnePackData questOnePackData2 = DataManager.DmQuest.GetQuestOnePackData(args2.selectQuestOneId);
					if (questOnePackData2 != null)
					{
						flag = SceneManager.CheckSealByQuestCategory(questOnePackData2.questChapter.category, @sealed);
					}
				}
				else if (args2 != null && args2.selectEventId != 0)
				{
					flag = @sealed.quest_event == 1;
				}
				else if (args2 != null && args2.category != QuestStaticChapter.Category.INVALID)
				{
					flag = SceneManager.CheckSealByQuestCategory(args2.category, @sealed);
				}
				break;
			}
			default:
				if (nextScene == SceneManager.SceneName.ScenePvp)
				{
					flag = @sealed.menu_pvp == 1;
				}
				break;
			}
		}
		else if (nextScene != SceneManager.SceneName.SceneShop)
		{
			if (nextScene == SceneManager.SceneName.ScenePicnic)
			{
				flag = @sealed.menu_picnic == 1;
			}
		}
		else
		{
			flag = @sealed.menu_shop == 1;
		}
		return flag;
	}

	// Token: 0x06000A97 RID: 2711 RVA: 0x0003CF64 File Offset: 0x0003B164
	private static bool CheckSealByQuestCategory(QuestStaticChapter.Category category, Sealed sd)
	{
		switch (category)
		{
		case QuestStaticChapter.Category.STORY:
			return sd.quest_main == 1;
		case QuestStaticChapter.Category.GROW:
			return sd.quest_grow == 1;
		case QuestStaticChapter.Category.CHARA:
			return sd.quest_friends == 1;
		case QuestStaticChapter.Category.EVENT:
			return sd.quest_event == 1;
		case QuestStaticChapter.Category.SIDE_STORY:
			return sd.quest_side == 1;
		case QuestStaticChapter.Category.ETCETERA:
			return sd.quest_special == 1;
		}
		return false;
	}

	// Token: 0x06000A98 RID: 2712 RVA: 0x0003CFE4 File Offset: 0x0003B1E4
	public void SetBaseCanvas(SceneManager.CanvasType type, Transform canvas, Camera cam)
	{
		this.baseCanvasMap[type] = canvas;
		this.canvasCameraMap[type] = cam;
	}

	// Token: 0x06000A99 RID: 2713 RVA: 0x0003D000 File Offset: 0x0003B200
	public Transform GetBaseCanvas(SceneManager.CanvasType type)
	{
		return this.baseCanvasMap[type];
	}

	// Token: 0x06000A9A RID: 2714 RVA: 0x0003D00E File Offset: 0x0003B20E
	public Camera GetCanvasCamera(SceneManager.CanvasType type)
	{
		return this.canvasCameraMap[type];
	}

	// Token: 0x06000A9B RID: 2715 RVA: 0x0003D01C File Offset: 0x0003B21C
	public static void AddPanelByBaseCanvas(SceneManager.CanvasType targetCanvasType, Transform targetTransform, bool safeAreaScalerEnabled = true)
	{
		if (safeAreaScalerEnabled && targetTransform.gameObject.GetComponent<SafeAreaScaler>() == null)
		{
			targetTransform.gameObject.AddComponent<SafeAreaScaler>();
		}
		targetTransform.SetParent(Singleton<SceneManager>.Instance.baseCanvasMap[targetCanvasType], false);
	}

	// Token: 0x06000A9C RID: 2716 RVA: 0x0003D058 File Offset: 0x0003B258
	public static GameObject CreateEmptyPanelByBaseCanvas(SceneManager.CanvasType type, string objName, bool enableSafeAreaScaler = true)
	{
		GameObject gameObject = new GameObject();
		gameObject.name = objName;
		RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
		rectTransform.localScale = Vector3.one;
		rectTransform.anchorMin = Vector2.zero;
		rectTransform.anchorMax = Vector2.one;
		rectTransform.offsetMax = Vector2.zero;
		rectTransform.offsetMin = Vector2.zero;
		SceneManager.AddPanelByBaseCanvas(type, gameObject.transform, enableSafeAreaScaler);
		return gameObject;
	}

	// Token: 0x06000A9D RID: 2717 RVA: 0x0003D0BC File Offset: 0x0003B2BC
	public static void WorldToCanvas(SceneManager.CanvasType type, Transform obj, Vector3 worldPos, Camera worldCam)
	{
		Vector2 vector = RectTransformUtility.WorldToScreenPoint(worldCam, worldPos);
		RectTransformUtility.ScreenPointToLocalPointInRectangle(obj.parent.GetComponent<RectTransform>(), vector, Singleton<SceneManager>.Instance.baseCanvasMap[type].GetComponent<Canvas>().worldCamera, out vector);
		obj.GetComponent<RectTransform>().localPosition = vector;
	}

	// Token: 0x17000299 RID: 665
	// (get) Token: 0x06000A9E RID: 2718 RVA: 0x0003D110 File Offset: 0x0003B310
	// (set) Token: 0x06000A9F RID: 2719 RVA: 0x0003D118 File Offset: 0x0003B318
	public Transform BaseField { get; set; }

	// Token: 0x06000AA0 RID: 2720 RVA: 0x0003D121 File Offset: 0x0003B321
	public static void Add3DObjectByBaseField(Transform addObject)
	{
		addObject.SetParent(Singleton<SceneManager>.Instance.BaseField, false);
	}

	// Token: 0x06000AA1 RID: 2721 RVA: 0x0003D134 File Offset: 0x0003B334
	public static void Initialize3DField()
	{
		GameObject gameObject = new GameObject
		{
			name = "MainField3D"
		};
		Singleton<SceneManager>.Instance.BaseField = gameObject.transform;
	}

	// Token: 0x06000AA2 RID: 2722 RVA: 0x0003D162 File Offset: 0x0003B362
	public static void Crean3DField()
	{
		if (null != Singleton<SceneManager>.Instance.BaseField)
		{
			SceneManager.RecursiveDestroy(Singleton<SceneManager>.Instance.BaseField.gameObject);
			Object.Destroy(Singleton<SceneManager>.Instance.BaseField.gameObject);
		}
	}

	// Token: 0x06000AA3 RID: 2723 RVA: 0x0003D1A0 File Offset: 0x0003B3A0
	private static void RecursiveDestroy(GameObject go)
	{
		foreach (object obj in go.transform)
		{
			Transform transform = (Transform)obj;
			SceneManager.RecursiveDestroy(transform.gameObject);
			Object.Destroy(transform.gameObject);
		}
	}

	// Token: 0x06000AA4 RID: 2724 RVA: 0x0003D208 File Offset: 0x0003B408
	private string GetSealedUrl(SceneManager.SceneName nextScene)
	{
		HomeCheckResult homeCheckResult = DataManager.DmHome.GetHomeCheckResult();
		Sealed @sealed = ((homeCheckResult != null) ? homeCheckResult.sealedData : null) ?? new Sealed();
		if (nextScene <= SceneManager.SceneName.ScenePvp)
		{
			if (nextScene == SceneManager.SceneName.SceneGacha)
			{
				return @sealed.menu_gacha_url;
			}
			if (nextScene == SceneManager.SceneName.ScenePvp)
			{
				return @sealed.menu_pvp_url;
			}
		}
		else
		{
			if (nextScene == SceneManager.SceneName.SceneShop)
			{
				return @sealed.menu_shop_url;
			}
			if (nextScene == SceneManager.SceneName.ScenePicnic)
			{
				return @sealed.menu_picnic_url;
			}
		}
		return null;
	}

	// Token: 0x06000AA5 RID: 2725 RVA: 0x0003D270 File Offset: 0x0003B470
	private string GetSealedText(SceneManager.SceneName nextScene)
	{
		HomeCheckResult homeCheckResult = DataManager.DmHome.GetHomeCheckResult();
		Sealed @sealed = ((homeCheckResult != null) ? homeCheckResult.sealedData : null) ?? new Sealed();
		if (nextScene <= SceneManager.SceneName.ScenePvp)
		{
			if (nextScene == SceneManager.SceneName.SceneGacha)
			{
				return this.ConvertEnterText(@sealed.menu_gacha_text);
			}
			if (nextScene == SceneManager.SceneName.ScenePvp)
			{
				return this.ConvertEnterText(@sealed.menu_pvp_text);
			}
		}
		else
		{
			if (nextScene == SceneManager.SceneName.SceneShop)
			{
				return this.ConvertEnterText(@sealed.menu_shop_text);
			}
			if (nextScene == SceneManager.SceneName.ScenePicnic)
			{
				return this.ConvertEnterText(@sealed.menu_picnic_text);
			}
		}
		return null;
	}

	// Token: 0x06000AA6 RID: 2726 RVA: 0x0003D2EE File Offset: 0x0003B4EE
	private string ConvertEnterText(string text)
	{
		text = text.Replace("¥n", "\n");
		return text;
	}

	// Token: 0x06000AA7 RID: 2727 RVA: 0x0003D304 File Offset: 0x0003B504
	public object GetNextSceneArgs(SceneManager.SceneName sceneName, int id)
	{
		object obj = null;
		if (sceneName != SceneManager.SceneName.SceneGacha)
		{
			if (sceneName != SceneManager.SceneName.SceneQuest)
			{
				if (sceneName == SceneManager.SceneName.SceneShop)
				{
					obj = new SceneShopArgs
					{
						resultNextSceneName = SceneManager.SceneName.SceneMission,
						resultNextSceneArgs = null,
						shopId = id
					};
				}
			}
			else
			{
				QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(id);
				DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(id);
				SceneQuest.Args args = new SceneQuest.Args();
				args.selectQuestOneId = (((questOnePackData == null || questOnePackData.questChapter.category != QuestStaticChapter.Category.EVENT) && eventData == null) ? id : 0);
				args.selectEventId = (((questOnePackData != null && questOnePackData.questChapter.category == QuestStaticChapter.Category.EVENT) || eventData != null) ? id : 0);
				args.menuBackSceneName = SceneManager.SceneName.SceneMission;
				args.menuBackSceneArgs = null;
				args.category = ((questOnePackData == null && eventData != null) ? QuestStaticChapter.Category.EVENT : ((questOnePackData != null) ? questOnePackData.questChapter.category : QuestStaticChapter.Category.INVALID));
				if (args.category == QuestStaticChapter.Category.EVENT)
				{
					args.initialMap = eventData.eventCategory == DataManagerEvent.Category.Large;
				}
				args.recordCameSceneName = SceneManager.SceneName.SceneMission;
				obj = args;
			}
		}
		else
		{
			obj = new SceneGacha.OpenParam
			{
				resultNextSceneName = SceneManager.SceneName.SceneMission,
				resultNextSceneArgs = null,
				gachaId = id
			};
		}
		return obj;
	}

	// Token: 0x06000AA8 RID: 2728 RVA: 0x0003D41C File Offset: 0x0003B61C
	public void CheckReleaseDataByCategory(QuestStaticChapter.Category category, UnityAction<bool, QuestOnePackData> action)
	{
		List<DataManagerServerMst.ModeReleaseData> modeReleaseDataList = DataManager.DmServerMst.ModeReleaseDataList;
		DataManagerGameStatus.UserFlagData.ReleaseMode releaseModeFlag = DataManager.DmGameStatus.MakeUserFlagData().ReleaseModeFlag;
		QuestOnePackData questOnePackData = null;
		bool flag = true;
		switch (category)
		{
		case QuestStaticChapter.Category.STORY:
			flag = true;
			questOnePackData = null;
			break;
		case QuestStaticChapter.Category.GROW:
		{
			DataManagerServerMst.ModeReleaseData modeReleaseData = modeReleaseDataList.Find((DataManagerServerMst.ModeReleaseData item) => item.Category == DataManagerServerMst.ModeReleaseData.ModeCategory.GrowthQuest);
			questOnePackData = DataManager.DmQuest.GetQuestOnePackData(modeReleaseData.QuestId);
			flag = releaseModeFlag.GrowthQuest > DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Locked;
			break;
		}
		case QuestStaticChapter.Category.CHARA:
		{
			DataManagerServerMst.ModeReleaseData modeReleaseData2 = modeReleaseDataList.Find((DataManagerServerMst.ModeReleaseData item) => item.Category == DataManagerServerMst.ModeReleaseData.ModeCategory.FriendsStory);
			questOnePackData = DataManager.DmQuest.GetQuestOnePackData(modeReleaseData2.QuestId);
			flag = releaseModeFlag.FriendsStory > DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Locked;
			break;
		}
		case QuestStaticChapter.Category.SIDE_STORY:
		{
			DataManagerServerMst.ModeReleaseData modeReleaseData3 = modeReleaseDataList.Find((DataManagerServerMst.ModeReleaseData item) => item.Category == DataManagerServerMst.ModeReleaseData.ModeCategory.AraiDiary);
			questOnePackData = DataManager.DmQuest.GetQuestOnePackData(modeReleaseData3.QuestId);
			flag = releaseModeFlag.AraiDiary > DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Locked;
			break;
		}
		case QuestStaticChapter.Category.CELLVAL:
		{
			DataManagerServerMst.ModeReleaseData modeReleaseData4 = modeReleaseDataList.Find((DataManagerServerMst.ModeReleaseData item) => item.Category == DataManagerServerMst.ModeReleaseData.ModeCategory.Cellval);
			questOnePackData = DataManager.DmQuest.GetQuestOnePackData(modeReleaseData4.QuestId);
			flag = releaseModeFlag.CellvalQuest > DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Locked;
			break;
		}
		case QuestStaticChapter.Category.ETCETERA:
		{
			DataManagerServerMst.ModeReleaseData modeReleaseData5 = modeReleaseDataList.Find((DataManagerServerMst.ModeReleaseData item) => item.Category == DataManagerServerMst.ModeReleaseData.ModeCategory.EtceteraQuest);
			questOnePackData = DataManager.DmQuest.GetQuestOnePackData(modeReleaseData5.QuestId);
			flag = releaseModeFlag.EtceteraQuest > DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Locked;
			break;
		}
		case QuestStaticChapter.Category.STORY2:
		{
			DataManagerServerMst.ModeReleaseData modeReleaseData6 = modeReleaseDataList.Find((DataManagerServerMst.ModeReleaseData item) => item.Category == DataManagerServerMst.ModeReleaseData.ModeCategory.MainStory2);
			questOnePackData = DataManager.DmQuest.GetQuestOnePackData(modeReleaseData6.QuestId);
			flag = releaseModeFlag.MainStory2 > DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Locked;
			break;
		}
		case QuestStaticChapter.Category.STORY3:
		{
			DataManagerServerMst.ModeReleaseData modeReleaseData7 = modeReleaseDataList.Find((DataManagerServerMst.ModeReleaseData item) => item.Category == DataManagerServerMst.ModeReleaseData.ModeCategory.MainStory3);
			questOnePackData = DataManager.DmQuest.GetQuestOnePackData(modeReleaseData7.QuestId);
			flag = releaseModeFlag.MainStory3 > DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Locked;
			break;
		}
		}
		action(flag, questOnePackData);
	}

	// Token: 0x06000AA9 RID: 2729 RVA: 0x0003D68C File Offset: 0x0003B88C
	public void CheckReleaseDataBySceneName(SceneManager.SceneName sceneName, UnityAction<bool, QuestOnePackData> action)
	{
		List<DataManagerServerMst.ModeReleaseData> modeReleaseDataList = DataManager.DmServerMst.ModeReleaseDataList;
		DataManagerGameStatus.UserFlagData.ReleaseMode releaseModeFlag = DataManager.DmGameStatus.MakeUserFlagData().ReleaseModeFlag;
		QuestOnePackData questOnePackData = null;
		bool flag = true;
		if (sceneName != SceneManager.SceneName.ScenePvp)
		{
			if (sceneName != SceneManager.SceneName.ScenePicnic)
			{
				if (sceneName == SceneManager.SceneName.SceneTraining)
				{
					DataManagerServerMst.ModeReleaseData modeReleaseData = modeReleaseDataList.Find((DataManagerServerMst.ModeReleaseData item) => item.Category == DataManagerServerMst.ModeReleaseData.ModeCategory.TrainingMode);
					questOnePackData = DataManager.DmQuest.GetQuestOnePackData(modeReleaseData.QuestId);
					flag = releaseModeFlag.TrainingByQuestTop > DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Locked;
				}
			}
			else
			{
				DataManagerServerMst.ModeReleaseData modeReleaseData2 = modeReleaseDataList.Find((DataManagerServerMst.ModeReleaseData item) => item.Category == DataManagerServerMst.ModeReleaseData.ModeCategory.Picnic);
				questOnePackData = DataManager.DmQuest.GetQuestOnePackData(modeReleaseData2.QuestId);
				flag = releaseModeFlag.Picnic > DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Locked;
			}
		}
		else
		{
			DataManagerServerMst.ModeReleaseData modeReleaseData3 = modeReleaseDataList.Find((DataManagerServerMst.ModeReleaseData item) => item.Category == DataManagerServerMst.ModeReleaseData.ModeCategory.PvpMode);
			questOnePackData = DataManager.DmQuest.GetQuestOnePackData(modeReleaseData3.QuestId);
			flag = releaseModeFlag.PvpMode > DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Locked;
		}
		action(flag, questOnePackData);
	}

	// Token: 0x0400083B RID: 2107
	private SceneManager.SceneName requestNextScene;

	// Token: 0x0400083C RID: 2108
	private object requestNextSceneArgs;

	// Token: 0x0400083D RID: 2109
	private SceneManager.SceneStatus sceneStatus;

	// Token: 0x0400083E RID: 2110
	private IEnumerator changeSceneEnumerator;

	// Token: 0x0400083F RID: 2111
	public static readonly Dictionary<SceneManager.CanvasType, int> CameraDepth = new Dictionary<SceneManager.CanvasType, int>
	{
		{
			SceneManager.CanvasType.BACK,
			10
		},
		{
			SceneManager.CanvasType.PRESET,
			15
		},
		{
			SceneManager.CanvasType.FRONT,
			20
		},
		{
			SceneManager.CanvasType.SYSTEM,
			60
		},
		{
			SceneManager.CanvasType.OVERLAY,
			70
		}
	};

	// Token: 0x04000840 RID: 2112
	public static readonly int CAMERA_DEPTH_LOWER_LIMIT = -100;

	// Token: 0x04000841 RID: 2113
	private Dictionary<SceneManager.CanvasType, Transform> baseCanvasMap = new Dictionary<SceneManager.CanvasType, Transform>();

	// Token: 0x04000842 RID: 2114
	private Dictionary<SceneManager.CanvasType, Camera> canvasCameraMap = new Dictionary<SceneManager.CanvasType, Camera>();

	// Token: 0x04000843 RID: 2115
	private Dictionary<SceneManager.SceneName, SceneManager.SceneData> aliveSceneMap = new Dictionary<SceneManager.SceneName, SceneManager.SceneData>();

	// Token: 0x04000844 RID: 2116
	private SceneManager.SceneData currentScene;

	// Token: 0x04000845 RID: 2117
	private RenderSettingParam sceneRenderSetting;

	// Token: 0x04000847 RID: 2119
	private static readonly string optionKey = "option";

	// Token: 0x04000848 RID: 2120
	public static Resolution screenSize = new Resolution
	{
		width = 0,
		height = 0
	};

	// Token: 0x04000849 RID: 2121
	public static Rect safeArea = Rect.zero;

	// Token: 0x0400084A RID: 2122
	public bool returnBackground;

	// Token: 0x0400084B RID: 2123
	private static readonly List<SceneManager.FadeDispPattern> fadeDispPatternList = new List<SceneManager.FadeDispPattern>
	{
		new SceneManager.FadeDispPattern
		{
			preScene = typeof(SceneBoot),
			nextScene = null,
			isFade = false
		},
		new SceneManager.FadeDispPattern
		{
			preScene = typeof(SceneAdvertise),
			nextScene = null,
			isFade = false
		},
		new SceneManager.FadeDispPattern
		{
			preScene = null,
			nextScene = typeof(SceneDataInitialize),
			isFade = false
		},
		new SceneManager.FadeDispPattern
		{
			preScene = typeof(SceneDataInitialize),
			nextScene = null,
			isFade = false
		},
		new SceneManager.FadeDispPattern
		{
			preScene = null,
			nextScene = typeof(SceneTitle),
			isFade = false
		},
		new SceneManager.FadeDispPattern
		{
			preScene = typeof(SceneQuest),
			nextScene = typeof(SceneBattleSelector),
			isFade = false
		},
		new SceneManager.FadeDispPattern
		{
			preScene = typeof(SceneBattleSelector),
			nextScene = typeof(SceneQuest),
			isFade = false
		},
		new SceneManager.FadeDispPattern
		{
			preScene = typeof(SceneOtherMenuTop),
			nextScene = typeof(SceneProfile),
			isFade = false
		},
		new SceneManager.FadeDispPattern
		{
			preScene = typeof(SceneProfile),
			nextScene = typeof(SceneOtherMenuTop),
			isFade = false
		},
		new SceneManager.FadeDispPattern
		{
			preScene = typeof(SceneOtherMenuTop),
			nextScene = typeof(SceneOption),
			isFade = false
		},
		new SceneManager.FadeDispPattern
		{
			preScene = typeof(SceneOption),
			nextScene = typeof(SceneOtherMenuTop),
			isFade = false
		},
		new SceneManager.FadeDispPattern
		{
			preScene = typeof(SceneOtherMenuTop),
			nextScene = typeof(SceneItemView),
			isFade = false
		},
		new SceneManager.FadeDispPattern
		{
			preScene = typeof(SceneItemView),
			nextScene = typeof(SceneOtherMenuTop),
			isFade = false
		},
		new SceneManager.FadeDispPattern
		{
			preScene = typeof(SceneOtherMenuTop),
			nextScene = typeof(SceneStoryView),
			isFade = false
		},
		new SceneManager.FadeDispPattern
		{
			preScene = typeof(SceneStoryView),
			nextScene = typeof(SceneOtherMenuTop),
			isFade = false
		},
		new SceneManager.FadeDispPattern
		{
			preScene = typeof(SceneBattleSelector),
			nextScene = typeof(SceneScenario),
			isFade = true,
			fadeType = CanvasManager.FadeType.SCENARIO
		},
		new SceneManager.FadeDispPattern
		{
			preScene = typeof(SceneBattleResult),
			nextScene = typeof(SceneScenario),
			isFade = true,
			fadeType = CanvasManager.FadeType.BATTLE_END
		},
		new SceneManager.FadeDispPattern
		{
			preScene = typeof(SceneScenario),
			nextScene = typeof(SceneScenario),
			isFade = true,
			fadeType = CanvasManager.FadeType.SCENARIO2
		},
		new SceneManager.FadeDispPattern
		{
			preScene = typeof(SceneBattle),
			nextScene = typeof(SceneBattleResult),
			isFade = false
		},
		new SceneManager.FadeDispPattern
		{
			preScene = typeof(SceneBattle),
			nextScene = null,
			isFade = true,
			fadeType = CanvasManager.FadeType.BATTLE_LOSE
		},
		new SceneManager.FadeDispPattern
		{
			preScene = null,
			nextScene = typeof(SceneBattle),
			isFade = true,
			fadeType = CanvasManager.FadeType.TIPS
		},
		new SceneManager.FadeDispPattern
		{
			preScene = typeof(SceneOtherMenuTop),
			nextScene = typeof(SceneAccountTransfer),
			isFade = false
		},
		new SceneManager.FadeDispPattern
		{
			preScene = typeof(SceneAccountTransfer),
			nextScene = typeof(SceneOtherMenuTop),
			isFade = false
		},
		new SceneManager.FadeDispPattern
		{
			preScene = typeof(SceneOtherMenuTop),
			nextScene = typeof(SceneRanking),
			isFade = false
		},
		new SceneManager.FadeDispPattern
		{
			preScene = typeof(SceneRanking),
			nextScene = typeof(SceneOtherMenuTop),
			isFade = false
		},
		new SceneManager.FadeDispPattern
		{
			preScene = typeof(SceneOtherMenuTop),
			nextScene = typeof(SceneAchievement),
			isFade = false
		},
		new SceneManager.FadeDispPattern
		{
			preScene = typeof(SceneAchievement),
			nextScene = typeof(SceneOtherMenuTop),
			isFade = false
		}
	};

	// Token: 0x0400084C RID: 2124
	private static readonly HashSet<SceneManager.SceneName> DisableDestorySceneList = new HashSet<SceneManager.SceneName>
	{
		SceneManager.SceneName.SceneAdvertise,
		SceneManager.SceneName.SceneDataInitialize,
		SceneManager.SceneName.SceneTitle,
		SceneManager.SceneName.SceneTutorialFirst,
		SceneManager.SceneName.SceneTutorialEnd,
		SceneManager.SceneName.SceneOpening
	};

	// Token: 0x0400084D RID: 2125
	private static readonly Dictionary<SceneManager.SceneName, HashSet<SceneManager.SceneName>> DestorySceneList = new Dictionary<SceneManager.SceneName, HashSet<SceneManager.SceneName>>();

	// Token: 0x0400084E RID: 2126
	private static readonly HashSet<SceneManager.SceneName> EnableMultiTouchSceneList = new HashSet<SceneManager.SceneName>
	{
		SceneManager.SceneName.SceneHome,
		SceneManager.SceneName.SceneKemoBoard,
		SceneManager.SceneName.SceneTreeHouse
	};

	// Token: 0x0400084F RID: 2127
	private static readonly HashSet<SceneManager.SceneName> EnableUploadKemoStatusSceneList = new HashSet<SceneManager.SceneName>
	{
		SceneManager.SceneName.SceneMission,
		SceneManager.SceneName.SceneProfile,
		SceneManager.SceneName.SceneRanking
	};

	// Token: 0x020007E7 RID: 2023
	public enum SceneName
	{
		// Token: 0x04003531 RID: 13617
		None,
		// Token: 0x04003532 RID: 13618
		SceneBoot,
		// Token: 0x04003533 RID: 13619
		SceneAdvertise,
		// Token: 0x04003534 RID: 13620
		SceneDataInitialize,
		// Token: 0x04003535 RID: 13621
		SceneTitle,
		// Token: 0x04003536 RID: 13622
		SceneBattle,
		// Token: 0x04003537 RID: 13623
		SceneBattleResult,
		// Token: 0x04003538 RID: 13624
		SceneBattleSelector,
		// Token: 0x04003539 RID: 13625
		SceneGacha,
		// Token: 0x0400353A RID: 13626
		SceneHome,
		// Token: 0x0400353B RID: 13627
		SceneQuest,
		// Token: 0x0400353C RID: 13628
		SceneScenario,
		// Token: 0x0400353D RID: 13629
		SceneFriend,
		// Token: 0x0400353E RID: 13630
		SceneCharaEdit,
		// Token: 0x0400353F RID: 13631
		SceneMission,
		// Token: 0x04003540 RID: 13632
		SceneOption,
		// Token: 0x04003541 RID: 13633
		ScenePresent,
		// Token: 0x04003542 RID: 13634
		ScenePvp,
		// Token: 0x04003543 RID: 13635
		SceneShop,
		// Token: 0x04003544 RID: 13636
		SceneTutorialFirst,
		// Token: 0x04003545 RID: 13637
		SceneProfile,
		// Token: 0x04003546 RID: 13638
		SceneOtherMenuTop,
		// Token: 0x04003547 RID: 13639
		SceneTutorialEnd,
		// Token: 0x04003548 RID: 13640
		SceneItemView,
		// Token: 0x04003549 RID: 13641
		ScenePvpDeck,
		// Token: 0x0400354A RID: 13642
		SceneOpening,
		// Token: 0x0400354B RID: 13643
		SceneStoryView,
		// Token: 0x0400354C RID: 13644
		ScenePicnic,
		// Token: 0x0400354D RID: 13645
		SceneAccountTransfer,
		// Token: 0x0400354E RID: 13646
		SceneTraining,
		// Token: 0x0400354F RID: 13647
		SceneKemoBoard,
		// Token: 0x04003550 RID: 13648
		ScenePhotoAlbum,
		// Token: 0x04003551 RID: 13649
		SceneTreeHouse,
		// Token: 0x04003552 RID: 13650
		SceneRanking,
		// Token: 0x04003553 RID: 13651
		SceneAchievement
	}

	// Token: 0x020007E8 RID: 2024
	private enum SceneStatus
	{
		// Token: 0x04003555 RID: 13653
		POLLING_REQUEST,
		// Token: 0x04003556 RID: 13654
		REQUEST_NEXT_SCENE,
		// Token: 0x04003557 RID: 13655
		CHANGING_SCENE
	}

	// Token: 0x020007E9 RID: 2025
	public enum CanvasType
	{
		// Token: 0x04003559 RID: 13657
		INVALID,
		// Token: 0x0400355A RID: 13658
		PRESET,
		// Token: 0x0400355B RID: 13659
		FRONT,
		// Token: 0x0400355C RID: 13660
		BACK,
		// Token: 0x0400355D RID: 13661
		SYSTEM,
		// Token: 0x0400355E RID: 13662
		OVERLAY
	}

	// Token: 0x020007EA RID: 2026
	private class SceneData
	{
		// Token: 0x06003770 RID: 14192 RVA: 0x001C82A4 File Offset: 0x001C64A4
		public SceneData(SceneManager.SceneName sceneName, BaseScene baseScene)
		{
			this.sceneName = sceneName;
			this.baseScene = baseScene;
		}

		// Token: 0x0400355F RID: 13663
		public BaseScene baseScene;

		// Token: 0x04003560 RID: 13664
		public SceneManager.SceneName sceneName;
	}

	// Token: 0x020007EB RID: 2027
	private class FadeDispPattern
	{
		// Token: 0x04003561 RID: 13665
		public Type preScene;

		// Token: 0x04003562 RID: 13666
		public Type nextScene;

		// Token: 0x04003563 RID: 13667
		public bool isFade = true;

		// Token: 0x04003564 RID: 13668
		public CanvasManager.FadeType fadeType = CanvasManager.FadeType.NORMAL;
	}
}
