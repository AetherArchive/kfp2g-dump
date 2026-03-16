using System;
using System.Collections;
using SGNFW.Common;

public class ScenePvpDeck : BaseScene
{
	public override void OnCreateScene()
	{
	}

	public override void OnEnableScene(object args)
	{
		this.currentArgs = args as ScenePvpDeck.Args;
		PvpPackData pvpPackDataBySeasonID = DataManager.DmPvp.GetPvpPackDataBySeasonID(this.currentArgs.pvpSeasonId);
		bool flag = pvpPackDataBySeasonID != null && PvpStaticData.Type.SPECIAL == pvpPackDataBySeasonID.staticData.type;
		CanvasManager.HdlCmnMenu.SetupMenu(true, flag ? PrjUtil.SPECIAL_PvP_FORMATION : PrjUtil.PvP_FORMATION, new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickButtonMenuRetrun), "", new PguiCmnMenuCtrl.OnClickMoveSequenceButton(this.OnClickMoveSequenceButton), null);
		CanvasManager.SetBgObj("PanelBg_PvP");
		SoundManager.PlayBGM("prd_bgm0007");
		int num = -1;
		if (flag)
		{
			DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventData(pvpPackDataBySeasonID.staticData.spEventId);
			if (eventData != null)
			{
				num = DataManager.DmQuest.QuestStaticData.chapterDataMap[eventData.eventChapterId].mapDataList[0].questGroupList[0].questOneList[0].questId;
			}
		}
		CanvasManager.HdlPhotoWindowCtrl.SetPvpSeasonId(this.currentArgs.pvpSeasonId);
		CanvasManager.HdlAccessoryWindowCtrl.SetPvpSeasonId(this.currentArgs.pvpSeasonId);
		CanvasManager.HdlSelCharaDeck.SetActive(true, false);
		CanvasManager.HdlSelCharaDeck.Setup(new SelCharaDeckCtrl.SetupParam
		{
			deckCategory = UserDeckData.Category.PVP,
			pvpSeasonId = this.currentArgs.pvpSeasonId,
			callScene = SceneManager.SceneName.ScenePvpDeck,
			callbackGotoBattle = new SelCharaDeckCtrl.OnClickGotoBattle(this.OnClickBattleButton),
			attrIndex = 0,
			helperPackData = null
		}, num);
		if (this.currentArgs.openCharaWindow)
		{
			CanvasManager.HdlCharaWindowCtrl.OpenPrev();
		}
		else if (this.currentArgs.openPhotoWindow)
		{
			CanvasManager.HdlPhotoWindowCtrl.OpenPrev();
		}
		else if (this.currentArgs.openAccessoryWindow)
		{
			CanvasManager.HdlAccessoryWindowCtrl.OpenPrev();
		}
		this.requestNextScene = SceneManager.SceneName.None;
	}

	private bool OnClickMoveSequenceButton(SceneManager.SceneName sceneName, object sceneArgs)
	{
		return CanvasManager.HdlSelCharaDeck.OnClickMoveSequenceButton(sceneName, sceneArgs);
	}

	private void OnClickButtonMenuRetrun()
	{
		if (CanvasManager.HdlSelCharaDeck.gameObject.activeSelf)
		{
			CanvasManager.HdlSelCharaDeck.OnClickMenuReturn(delegate
			{
				this.requestNextScene = SceneManager.SceneName.ScenePvp;
				this.nextSceneArgs = new ScenePvp.Args
				{
					fastPvpSeasonId = this.currentArgs.pvpSeasonId,
					isReturnFromPvpDeck = true
				};
			});
		}
	}

	private bool OnClickBattleButton()
	{
		UserOptionData userOptionData = DataManager.DmUserInfo.optionData.Clone();
		PvpPackData pvpPackDataBySeasonID = DataManager.DmPvp.GetPvpPackDataBySeasonID(this.currentArgs.pvpSeasonId);
		if (pvpPackDataBySeasonID != null && PvpStaticData.Type.SPECIAL == pvpPackDataBySeasonID.staticData.type)
		{
			userOptionData.CurrentSpPvpParty = CanvasManager.HdlSelCharaDeck.GetDeckId();
		}
		else
		{
			userOptionData.CurrentPvpParty = CanvasManager.HdlSelCharaDeck.GetDeckId();
		}
		DataManager.DmUserInfo.RequestActionUpdateUserOption(userOptionData);
		Singleton<SceneManager>.Instance.StartCoroutine(this.RequestPvPDeckSelect());
		return true;
	}

	private IEnumerator RequestPvPDeckSelect()
	{
		SelCharaDeckCtrl.EditResultData editResultData = CanvasManager.HdlSelCharaDeck.GetEditResultData();
		DataManager.DmPvp.RequestActionPvPDeckSelect(this.currentArgs.pvpSeasonId, editResultData.currentDeckId);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		this.requestNextScene = SceneManager.SceneName.ScenePvp;
		this.nextSceneArgs = new ScenePvp.Args
		{
			fastPvpSeasonId = this.currentArgs.pvpSeasonId,
			isReturnFromPvpDeck = true
		};
		yield break;
	}

	public override void Update()
	{
		bool flag = true;
		if (CanvasManager.HdlSelCharaDeck.gameObject.activeSelf && CanvasManager.HdlSelCharaDeck.ForceReturnTop())
		{
			this.requestNextScene = SceneManager.SceneName.ScenePvp;
			this.nextSceneArgs = new ScenePvp.Args
			{
				fastPvpSeasonId = this.currentArgs.pvpSeasonId,
				isReturnFromPvpDeck = true
			};
		}
		if (this.requestNextScene != SceneManager.SceneName.None)
		{
			Singleton<SceneManager>.Instance.SetNextScene(this.requestNextScene, this.nextSceneArgs);
			flag = false;
		}
		CanvasManager.HdlCmnMenu.UpdateMenu(flag, true);
	}

	public override void OnDisableScene()
	{
		CanvasManager.HdlSelCharaDeck.SetActive(false, false);
	}

	public override void OnDestroyScene()
	{
	}

	private SceneManager.SceneName requestNextScene;

	private object nextSceneArgs;

	private ScenePvpDeck.Args currentArgs;

	public class Args
	{
		public int pvpSeasonId;

		public bool openCharaWindow;

		public bool openPhotoWindow;

		public bool openAccessoryWindow;
	}
}
