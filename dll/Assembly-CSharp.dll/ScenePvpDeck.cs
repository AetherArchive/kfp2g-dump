using System;
using System.Collections;
using SGNFW.Common;

// Token: 0x0200016B RID: 363
public class ScenePvpDeck : BaseScene
{
	// Token: 0x06001580 RID: 5504 RVA: 0x0010D8CC File Offset: 0x0010BACC
	public override void OnCreateScene()
	{
	}

	// Token: 0x06001581 RID: 5505 RVA: 0x0010D8D0 File Offset: 0x0010BAD0
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

	// Token: 0x06001582 RID: 5506 RVA: 0x0010DA95 File Offset: 0x0010BC95
	private bool OnClickMoveSequenceButton(SceneManager.SceneName sceneName, object sceneArgs)
	{
		return CanvasManager.HdlSelCharaDeck.OnClickMoveSequenceButton(sceneName, sceneArgs);
	}

	// Token: 0x06001583 RID: 5507 RVA: 0x0010DAA3 File Offset: 0x0010BCA3
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

	// Token: 0x06001584 RID: 5508 RVA: 0x0010DAD0 File Offset: 0x0010BCD0
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

	// Token: 0x06001585 RID: 5509 RVA: 0x0010DB53 File Offset: 0x0010BD53
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

	// Token: 0x06001586 RID: 5510 RVA: 0x0010DB64 File Offset: 0x0010BD64
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

	// Token: 0x06001587 RID: 5511 RVA: 0x0010DBE7 File Offset: 0x0010BDE7
	public override void OnDisableScene()
	{
		CanvasManager.HdlSelCharaDeck.SetActive(false, false);
	}

	// Token: 0x06001588 RID: 5512 RVA: 0x0010DBF5 File Offset: 0x0010BDF5
	public override void OnDestroyScene()
	{
	}

	// Token: 0x040011C6 RID: 4550
	private SceneManager.SceneName requestNextScene;

	// Token: 0x040011C7 RID: 4551
	private object nextSceneArgs;

	// Token: 0x040011C8 RID: 4552
	private ScenePvpDeck.Args currentArgs;

	// Token: 0x02000C03 RID: 3075
	public class Args
	{
		// Token: 0x0400495E RID: 18782
		public int pvpSeasonId;

		// Token: 0x0400495F RID: 18783
		public bool openCharaWindow;

		// Token: 0x04004960 RID: 18784
		public bool openPhotoWindow;

		// Token: 0x04004961 RID: 18785
		public bool openAccessoryWindow;
	}
}
