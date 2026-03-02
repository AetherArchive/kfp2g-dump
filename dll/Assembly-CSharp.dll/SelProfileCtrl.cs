using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000162 RID: 354
public class SelProfileCtrl : MonoBehaviour
{
	// Token: 0x17000387 RID: 903
	// (get) Token: 0x06001470 RID: 5232 RVA: 0x000F9C2E File Offset: 0x000F7E2E
	// (set) Token: 0x06001471 RID: 5233 RVA: 0x000F9C36 File Offset: 0x000F7E36
	private SelProfileCtrl.Mode currentMode
	{
		get
		{
			return this._currentMode;
		}
		set
		{
			this._preMode = this._currentMode;
			this._currentMode = value;
		}
	}

	// Token: 0x17000388 RID: 904
	// (get) Token: 0x06001472 RID: 5234 RVA: 0x000F9C4B File Offset: 0x000F7E4B
	private SelProfileCtrl.Mode preMode
	{
		get
		{
			return this._preMode;
		}
	}

	// Token: 0x17000389 RID: 905
	// (get) Token: 0x06001473 RID: 5235 RVA: 0x000F9C53 File Offset: 0x000F7E53
	public bool GetStart
	{
		get
		{
			return this.getStart;
		}
	}

	// Token: 0x06001474 RID: 5236 RVA: 0x000F9C5C File Offset: 0x000F7E5C
	public void Init()
	{
		GameObject gameObject = Object.Instantiate<GameObject>((GameObject)AssetManager.GetAssetData("SceneMenu/GUI/Prefab/GUI_Menu_Profile"), base.transform);
		GameObject gameObject2 = AssetManager.GetAssetData("SceneMenu/GUI/Prefab/GUI_Menu_ProfileWindow") as GameObject;
		this.windowCommentChange = new SelProfileCtrl.WindowCommentChange(Object.Instantiate<Transform>(gameObject2.transform.Find("CommentWindow"), base.transform).transform);
		this.windowNameChange = new SelProfileCtrl.WindowNameChange(Object.Instantiate<Transform>(gameObject2.transform.Find("NameWindow"), base.transform).transform);
		this.renderTextureChara = gameObject.transform.Find("ProfileAll/All/RenderChara").GetComponent<PguiRenderTextureCharaCtrl>();
		this.guiData = new SelProfileCtrl.GUI(gameObject.transform);
		this.guiData.guiBtnFavorite.Btn_Favorite.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.guiBtnLoginBonus.Btn_LoginBonus.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.guiBtnLoginBonus.favorite.SetActive(false);
		this.guiData.guiBtnLoginBonus.random.SetActive(false);
		this.guiData.guiProfile.Btn_Name.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.guiProfile.Btn_Comment.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.guiProfile.Btn_Id.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.guiProfile.Btn_Gender.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.guiHelper.Btn_Helper.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.guiFavorite.ScrollView.InitForce();
		ReuseScroll scrollView = this.guiData.guiFavorite.ScrollView;
		scrollView.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollView.onStartItem, new Action<int, GameObject>(this.OnStartCharaTop));
		ReuseScroll scrollView2 = this.guiData.guiFavorite.ScrollView;
		scrollView2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollView2.onUpdateItem, new Action<int, GameObject>(this.OnUpdateCharaTop));
		this.guiData.guiFavorite.ScrollView.Setup(10, 0);
		SortWindowCtrl.RegisterData registerData = new SortWindowCtrl.RegisterData();
		registerData.register = SortFilterDefine.RegisterType.CHARA_FAVORITE;
		registerData.filterButton = this.guiData.guiFavorite.Btn_FilterOnOff;
		registerData.sortButton = this.guiData.guiFavorite.Btn_Sort;
		registerData.sortUdButton = this.guiData.guiFavorite.Btn_SortUpDown;
		registerData.funcGetTargetBaseList = () => new SortWindowCtrl.SortTarget
		{
			charaList = new List<CharaPackData>(DataManager.DmChara.GetUserCharaMap().Values)
		};
		registerData.funcDisideTarget = delegate(SortWindowCtrl.SortTarget item)
		{
			this.dispFavoriteCharaList = item.charaList;
			this.sortType = item.sortType;
			this.guiData.guiFavorite.ResizeScrollView(this.dispFavoriteCharaList.Count, 1 + this.dispFavoriteCharaList.Count / 3);
		};
		SortWindowCtrl.RegisterData registerData2 = registerData;
		CanvasManager.HdlOpenWindowSortFilter.Register(registerData2, false, null);
		this.InitByHelperDeck();
		this.windowNameChange.InputField.onEndEdit.AddListener(delegate(string str)
		{
			this.windowNameChange.InputField.text = PrjUtil.ModifiedName(str);
			this.windowNameChange.Txt_ErrorMassage.gameObject.SetActive(false);
		});
		this.windowCommentChange.InputField.onEndEdit.AddListener(delegate(string str)
		{
			this.windowCommentChange.InputField.text = PrjUtil.ModifiedComment(str);
			this.windowCommentChange.Txt_ErrorMassage.gameObject.SetActive(false);
		});
		if (!this.isDebug)
		{
			SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this.windowCommentChange.owCtrl.transform, true);
			SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this.windowNameChange.owCtrl.transform, true);
		}
		this.currentMode = SelProfileCtrl.Mode.INVALID;
	}

	// Token: 0x06001475 RID: 5237 RVA: 0x000F9FD8 File Offset: 0x000F81D8
	public void Setup(bool helperStart, bool selectPhoto, bool selectAccessory, bool fromBattleSelector, UnityAction<bool> cb = null)
	{
		this.isHelperSettingStartFromCharaEdit = helperStart;
		this.isFromBattleSelecter = fromBattleSelector;
		this.setActiveQuestMapDataCB = cb;
		this.currentFavoriteCharaId = 0;
		this.requestFavoriteCharaId = DataManager.DmUserInfo.favoriteCharaId;
		this.renderTextureChara.Create();
		Dictionary<int, CharaPackData> userCharaMap = DataManager.DmChara.GetUserCharaMap();
		this.dispFavoriteCharaList = new List<CharaPackData>(userCharaMap.Values);
		CanvasManager.HdlOpenWindowSortFilter.SolutionList(SortFilterDefine.RegisterType.CHARA_FAVORITE, null);
		this.avaterType = DataManager.DmUserInfo.avatarType - DataManagerUserInfo.AvatarType.TYPE_A;
		this.guiData.guiHelperChange.charaDeck.SwitchHelperIcon(-1, false);
		if (this.isHelperSettingStartFromCharaEdit || this.isFromBattleSelecter)
		{
			this.getStart = true;
			this.guiData.guiFavorite.baseObj.SetActive(false);
			this.guiData.helperAnimation.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
			this.requestMode = SelProfileCtrl.Mode.HELPER_CHANGE;
			this.guiData.profileAnimation.ExPauseAnimation(SimpleAnimation.ExPguiStatus.START, null);
			this.guiData.guiProfile.baseObj.SetActive(false);
			this.guiData.guiHelper.baseObj.SetActive(false);
			this.guiData.guiBtnFavorite.baseObj.SetActive(false);
			this.guiData.guiBtnLoginBonus.baseObj.SetActive(false);
			this.SetupDeckTop(selectPhoto, selectAccessory);
		}
		else
		{
			this.getStart = true;
			this.requestMode = SelProfileCtrl.Mode.TOP;
			this.guiData.guiFavorite.baseObj.SetActive(false);
			this.guiData.guiHelperChange.baseObj.SetActive(false);
			this.guiData.guiProfile.baseObj.SetActive(true);
			this.guiData.guiBtnFavorite.baseObj.SetActive(true);
			this.guiData.guiBtnLoginBonus.baseObj.SetActive(true);
			this.DispLoginBonus();
			this.guiData.guiHelper.baseObj.SetActive(true);
			this.RefreshProfile();
			this.guiData.profileAnimation.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
			for (int i = 0; i < this.guiData.guiHelperChange.charaDeck.iconCharaPacks.Count; i++)
			{
				this.guiData.guiHelperChange.charaDeck.iconCharaPacks[i].iconChara.anime.ExPauseAnimation(SimpleAnimation.ExPguiStatus.START, null);
			}
		}
		this.OnClickMoveSequenceName = SceneManager.SceneName.None;
		this.OnClickMoveSequenceArgs = null;
		DataManagerAchievement.AchievementData selectData = DataManager.DmAchievement.GetSelectData();
		int num = ((selectData == null) ? 0 : selectData.staticData.id);
		this.guiData.guiProfile.Achievement.Setup(num, false, false);
		this.guiData.guiProfile.Btn_Achievement.AddOnClickListener(delegate(PguiButtonCtrl button)
		{
			this.OnClickButtionChangeAchievement();
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.guiProfile.Btn_KizunaBuff.AddOnClickListener(delegate(PguiButtonCtrl button)
		{
			this.currentEnumerator = this.OpenBuffInfoWindow();
		}, PguiButtonCtrl.SoundType.DEFAULT);
	}

	// Token: 0x06001476 RID: 5238 RVA: 0x000FA2B8 File Offset: 0x000F84B8
	public bool IsProcessing()
	{
		return this.currentEnumerator != null;
	}

	// Token: 0x06001477 RID: 5239 RVA: 0x000FA2C3 File Offset: 0x000F84C3
	private void Start()
	{
	}

	// Token: 0x06001478 RID: 5240 RVA: 0x000FA2C8 File Offset: 0x000F84C8
	private void OnDestroy()
	{
		if (this.guiData != null)
		{
			Object.Destroy(this.guiData.baseObj);
			this.guiData = null;
		}
		if (this.windowCommentChange != null && null != this.windowCommentChange.owCtrl)
		{
			Object.Destroy(this.windowCommentChange.owCtrl.gameObject);
			this.windowCommentChange = null;
		}
		if (this.windowNameChange != null && null != this.windowNameChange.owCtrl)
		{
			Object.Destroy(this.windowNameChange.owCtrl.gameObject);
			this.windowNameChange = null;
		}
	}

	// Token: 0x06001479 RID: 5241 RVA: 0x000FA364 File Offset: 0x000F8564
	private void Update()
	{
		if (this.currentEnumerator != null && !this.currentEnumerator.MoveNext())
		{
			this.currentEnumerator = null;
		}
		if (this.requestActionUpdateLoanPackList != null && !this.requestActionUpdateLoanPackList.MoveNext())
		{
			this.requestActionUpdateLoanPackList = null;
		}
		if (this.requestMode != this.currentMode)
		{
			if (this.requestMode == SelProfileCtrl.Mode.NAME_CHANGE)
			{
				this.windowNameChange.Txt_ErrorMassage.gameObject.SetActive(false);
				this.windowNameChange.owCtrl.Setup(PrjUtil.MakeMessage("名前変更"), PrjUtil.MakeMessage("名前を入力してください"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, new PguiOpenWindowCtrl.Callback(this.OnClickOwButton), null, false);
				this.windowNameChange.owCtrl.Open();
			}
			else if (this.requestMode == SelProfileCtrl.Mode.COMMENT_CHANGE)
			{
				this.windowCommentChange.Txt_ErrorMassage.gameObject.SetActive(false);
				this.windowCommentChange.owCtrl.Setup(PrjUtil.MakeMessage("コメント変更"), PrjUtil.MakeMessage("コメントを入力してください"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, new PguiOpenWindowCtrl.Callback(this.OnClickOwButton), null, false);
				this.windowCommentChange.owCtrl.Open();
			}
			else
			{
				if (this.currentMode == SelProfileCtrl.Mode.TOP || this.requestMode == SelProfileCtrl.Mode.TOP)
				{
					this.guiData.guiBtnFavorite.baseObj.SetActive(this.requestMode == SelProfileCtrl.Mode.TOP);
					this.guiData.guiBtnLoginBonus.baseObj.SetActive(this.requestMode == SelProfileCtrl.Mode.TOP);
					this.guiData.guiHelper.baseObj.SetActive(this.requestMode == SelProfileCtrl.Mode.TOP);
					this.guiData.guiProfile.baseObj.SetActive(this.requestMode == SelProfileCtrl.Mode.TOP);
					this.renderTextureChara.renderTextureChara.gameObject.SetActive(this.requestMode == SelProfileCtrl.Mode.TOP);
					base.StartCoroutine(this.CharaMotionChange());
				}
				if (this.currentMode == SelProfileCtrl.Mode.FAVORITE_CHANGE || this.requestMode == SelProfileCtrl.Mode.FAVORITE_CHANGE)
				{
					this.requestFavoriteCharaId = this.currentFavoriteCharaId;
					this.guiData.guiFavorite.baseObj.SetActive(this.requestMode == SelProfileCtrl.Mode.FAVORITE_CHANGE);
					this.guiData.guiFavorite.ScrollView.Refresh();
				}
				if (this.currentMode == SelProfileCtrl.Mode.HELPER_CHANGE || this.requestMode == SelProfileCtrl.Mode.HELPER_CHANGE)
				{
					this.guiData.guiHelperChange.baseObj.SetActive(this.requestMode == SelProfileCtrl.Mode.HELPER_CHANGE);
					if (this.requestMode == SelProfileCtrl.Mode.HELPER_CHANGE)
					{
						this.SetupDeckTop(false, false);
					}
				}
			}
			this.currentMode = this.requestMode;
		}
		SimpleAnimation sa = this.guiData.guiHelperChange.charaDeck.Btn_PhotoRemove.transform.parent.GetComponent<SimpleAnimation>();
		if (this.currentHelperMode == SelProfileCtrl.HelperMode.PHOTO_TOP)
		{
			if (!sa.gameObject.activeSelf || !sa.ExIsCurrent(SimpleAnimation.ExPguiStatus.START))
			{
				sa.gameObject.SetActive(true);
				sa.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, delegate
				{
					sa.ExPauseAnimationLastFrame(SimpleAnimation.ExPguiStatus.START);
				});
			}
		}
		else if (sa.gameObject.activeSelf)
		{
			if (sa.ExIsCurrent(SimpleAnimation.ExPguiStatus.END))
			{
				if (!sa.ExIsPlaying())
				{
					sa.gameObject.SetActive(false);
				}
			}
			else
			{
				sa.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
			}
		}
		if (this.isDebug)
		{
			CanvasManager.HdlCmnMenu.UpdateMenu(true, true);
		}
	}

	// Token: 0x0600147A RID: 5242 RVA: 0x000FA6CF File Offset: 0x000F88CF
	public void RequestUpdateAvater()
	{
		DataManager.DmUserInfo.RequestActionUpdateUserAvatar(this.avaterType + DataManagerUserInfo.AvatarType.TYPE_A);
		this.renderTextureChara.Destroy();
	}

	// Token: 0x0600147B RID: 5243 RVA: 0x000FA6EE File Offset: 0x000F88EE
	public void SetActive(bool sw)
	{
		if (this.selPhotoEditCtrl != null)
		{
			if (sw)
			{
				this.selPhotoEditCtrl.SetActive(sw, false);
				return;
			}
			this.selPhotoEditCtrl.NotActive(null);
		}
	}

	// Token: 0x0600147C RID: 5244 RVA: 0x000FA71B File Offset: 0x000F891B
	private IEnumerator RequestUpdateName(string name)
	{
		DataManager.DmUserInfo.RequestActionUpdateUserName(name);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		if (DataManager.DmUserInfo.GetUpdateNameResult().isSuccess)
		{
			this.windowNameChange.owCtrl.ForceClose();
			this.RefreshProfile();
			this.requestMode = this.preMode;
		}
		else
		{
			this.windowNameChange.Txt_ErrorMassage.gameObject.SetActive(true);
			this.windowNameChange.Txt_ErrorMassage.text = "この名前は利用できません";
		}
		yield break;
	}

	// Token: 0x0600147D RID: 5245 RVA: 0x000FA731 File Offset: 0x000F8931
	private IEnumerator RequestUpdateFavoriteChara(int charaId)
	{
		bool isWindowFinish = false;
		int owAnswer = 0;
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(charaId);
		string text = (string.IsNullOrEmpty(userCharaData.staticData.baseData.NickName) ? userCharaData.staticData.baseData.NickName : (userCharaData.staticData.baseData.NickName + "\n")) + "「" + userCharaData.staticData.GetName() + "」をアシスタントに設定します\nよろしいですか？";
		CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("確認"), text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, delegate(int index)
		{
			owAnswer = index;
			isWindowFinish = true;
			return true;
		}, null, false);
		CanvasManager.HdlOpenWindowBasic.Open();
		while (!isWindowFinish)
		{
			yield return null;
		}
		if (owAnswer != 1)
		{
			yield break;
		}
		DataManager.DmUserInfo.RequestActionUpdateFavoriteChara(charaId);
		this.requestFavoriteCharaId = charaId;
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		this.isSetup = false;
		if (this.OnClickMoveSequenceName == SceneManager.SceneName.None)
		{
			this.RefreshProfile();
			this.guiData.favoriteAnimation.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
			{
				this.requestMode = SelProfileCtrl.Mode.TOP;
				this.guiData.profileAnimation.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
				this.isSetup = true;
			});
		}
		else
		{
			CanvasManager.HdlCmnMenu.MoveSceneByMenu(this.OnClickMoveSequenceName, this.OnClickMoveSequenceArgs);
		}
		while (!this.isSetup)
		{
			yield return null;
		}
		while (!this.renderTextureChara.renderTextureChara.FinishedSetup)
		{
			yield return null;
		}
		SoundManager.PlayVoice(DataManager.DmChara.GetCharaStaticData(this.currentFavoriteCharaId).cueSheetName, VOICE_TYPE.AST01);
		yield break;
	}

	// Token: 0x0600147E RID: 5246 RVA: 0x000FA747 File Offset: 0x000F8947
	private IEnumerator RequestUpdateComment(string comment)
	{
		DataManager.DmUserInfo.RequestActionUpdateUserComment(comment);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		if (DataManager.DmUserInfo.GetUpdateCommentResult().isSuccess)
		{
			this.windowCommentChange.owCtrl.ForceClose();
			this.RefreshProfile();
			this.requestMode = this.preMode;
		}
		else
		{
			this.windowCommentChange.Txt_ErrorMassage.gameObject.SetActive(true);
		}
		yield break;
	}

	// Token: 0x0600147F RID: 5247 RVA: 0x000FA75D File Offset: 0x000F895D
	private IEnumerator RequestActionUpdateLoanPackList()
	{
		DataManager.DmUserInfo.RequestActionUpdateLoanPackList(this.disideLoanPackList);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001480 RID: 5248 RVA: 0x000FA76C File Offset: 0x000F896C
	private IEnumerator RequestUpdateLoanPack()
	{
		bool requestServer = false;
		if (this.IsChangeHelpers())
		{
			SelProfileCtrl.<>c__DisplayClass50_0 CS$<>8__locals1 = new SelProfileCtrl.<>c__DisplayClass50_0();
			CS$<>8__locals1.isWindowFinish = false;
			CS$<>8__locals1.owAnswer = 0;
			CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("確認"), PrjUtil.MakeMessage("編成が変更されています"), SelCharaDeckCtrl.questionButtonSet, true, delegate(int index)
			{
				CS$<>8__locals1.owAnswer = index;
				CS$<>8__locals1.isWindowFinish = true;
				return true;
			}, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			while (!CS$<>8__locals1.isWindowFinish)
			{
				yield return null;
			}
			if (CS$<>8__locals1.owAnswer == -1)
			{
				yield break;
			}
			if (CS$<>8__locals1.owAnswer == 0)
			{
				SelProfileCtrl.DeepCopyLoanPackList(this.disideLoanPackList, this.cloneLoanPackList);
			}
			else
			{
				SelProfileCtrl.DeepCopyLoanPackList(this.cloneLoanPackList, this.disideLoanPackList);
				requestServer = true;
			}
			this.isChangeHelpers = false;
			if (this.selPhotoEditCtrl != null)
			{
				this.selPhotoEditCtrl.isChangeClone = false;
			}
			CS$<>8__locals1 = null;
		}
		if (this.OnClickMoveSequenceName != SceneManager.SceneName.None)
		{
			if (this.disideLoanPackList.Count > 0 && requestServer)
			{
				this.requestActionUpdateLoanPackList = this.RequestActionUpdateLoanPackList();
				while (this.requestActionUpdateLoanPackList != null)
				{
					yield return null;
				}
				requestServer = false;
			}
			CanvasManager.HdlCmnMenu.MoveSceneByMenu(this.OnClickMoveSequenceName, this.OnClickMoveSequenceArgs);
		}
		else if (this.currentHelperMode == SelProfileCtrl.HelperMode.DECK_TOP || this.currentHelperMode == SelProfileCtrl.HelperMode.PHOTO_TOP || this.currentHelperMode == SelProfileCtrl.HelperMode.ACCESSORY_TOP)
		{
			if (this.disideLoanPackList.Count > 0 && requestServer)
			{
				this.requestActionUpdateLoanPackList = this.RequestActionUpdateLoanPackList();
				while (this.requestActionUpdateLoanPackList != null)
				{
					yield return null;
				}
				requestServer = false;
			}
			if (this.isHelperSettingStartFromCharaEdit)
			{
				CanvasManager.HdlCmnMenu.MoveSceneByMenu(SceneManager.SceneName.SceneCharaEdit, null);
			}
			else if (this.isFromBattleSelecter)
			{
				SceneBattleSelector.Args args = new SceneBattleSelector.Args();
				args.recordCameSceneName = SceneManager.SceneName.SceneProfile;
				args.setActiveQuestMapDataCB = this.setActiveQuestMapDataCB;
				CanvasManager.HdlCmnMenu.MoveSceneByMenu(SceneManager.SceneName.SceneBattleSelector, args);
			}
			else
			{
				this.guiData.helperAnimation.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
				{
					this.requestMode = SelProfileCtrl.Mode.TOP;
					this.guiData.profileAnimation.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
					this.RefreshProfile();
				});
			}
		}
		else if (this.currentHelperMode == SelProfileCtrl.HelperMode.DECK_EDIT)
		{
			this.OnClickButtonHelper(this.guiData.guiHelperChange.charaDeck.Btn_EditOk);
		}
		else if (this.currentHelperMode == SelProfileCtrl.HelperMode.PHOTO_EDIT)
		{
			this.selPhotoEditCtrl.setupParam.cbButtonEditOk(this.selPhotoEditCtrl.guiData.photoDeck.Btn_EditOk);
		}
		if (this.disideLoanPackList.Count > 0 && requestServer)
		{
			this.requestActionUpdateLoanPackList = this.RequestActionUpdateLoanPackList();
			while (this.requestActionUpdateLoanPackList != null)
			{
				yield return null;
			}
			requestServer = false;
		}
		yield break;
	}

	// Token: 0x06001481 RID: 5249 RVA: 0x000FA77B File Offset: 0x000F897B
	private IEnumerator CharaMotionChange()
	{
		while (!this.renderTextureChara.renderTextureChara.FinishedSetup)
		{
			yield return null;
		}
		this.renderTextureChara.renderTextureChara.SetAnimation(CharaMotionDefine.ActKey.GACHA_LP, true);
		this.renderTextureChara.renderTextureChara.postion = new Vector2(-460f, -130f);
		this.renderTextureChara.renderTextureChara.fieldOfView = 20f;
		this.renderTextureChara.renderTextureChara.OnValidate();
		this.getStart = true;
		yield break;
	}

	// Token: 0x06001482 RID: 5250 RVA: 0x000FA78C File Offset: 0x000F898C
	private void RefreshProfile()
	{
		this.guiData.guiProfile.Txt_UserName.text = DataManager.DmUserInfo.userName;
		this.windowNameChange.InputField.text = DataManager.DmUserInfo.userName;
		this.windowNameChange.InputField.textComponent.text = DataManager.DmUserInfo.userName;
		this.guiData.guiProfile.Txt_Comment.text = DataManager.DmUserInfo.userComment;
		this.windowCommentChange.InputField.text = DataManager.DmUserInfo.userComment;
		this.windowCommentChange.InputField.textComponent.text = DataManager.DmUserInfo.userComment;
		this.guiData.guiProfile.Relpace_Photo.Replace(this.avaterType);
		this.guiData.guiProfile.Txt_Id.text = DataManager.DmUserInfo.friendId.ToString();
		this.guiData.guiProfile.TotalKemoStatusText.text = DataManager.DmChara.UserAllCharaKemoStatus.ToString();
		if (DataManager.DmUserInfo.favoriteCharaId != this.currentFavoriteCharaId)
		{
			this.renderTextureChara.renderTextureChara.Setup(DataManager.DmChara.GetUserCharaData(DataManager.DmUserInfo.favoriteCharaId), 0, CharaMotionDefine.ActKey.GACHA_LP, true, null, false, null, 0f, null, false);
			this.currentFavoriteCharaId = DataManager.DmUserInfo.favoriteCharaId;
		}
		base.StartCoroutine(this.CharaMotionChange());
		for (int i = 0; i < this.guiData.guiHelper.iconChara.Count; i++)
		{
			this.guiData.guiHelper.iconChara[i].Setup(DataManager.DmChara.GetUserCharaData(DataManager.DmUserInfo.loanPackList[i].charaId), SortFilterDefine.SortType.LEVEL, false, new CharaWindowCtrl.DetailParamSetting(CharaWindowCtrl.DetailParamSetting.Preset.MINE_EASY, null), 0, -1, 0);
		}
	}

	// Token: 0x06001483 RID: 5251 RVA: 0x000FA979 File Offset: 0x000F8B79
	private IEnumerator ChangeLoginBonus()
	{
		UserOptionData userOptionData = DataManager.DmUserInfo.optionData.Clone();
		userOptionData.LoginBonusFriends = !DataManager.DmUserInfo.optionData.LoginBonusFriends;
		DataManager.DmUserInfo.RequestActionUpdateUserOption(userOptionData);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		this.DispLoginBonus();
		yield break;
	}

	// Token: 0x06001484 RID: 5252 RVA: 0x000FA988 File Offset: 0x000F8B88
	private void DispLoginBonus()
	{
		this.guiData.guiBtnLoginBonus.favorite.SetActive(!DataManager.DmUserInfo.optionData.LoginBonusFriends);
		this.guiData.guiBtnLoginBonus.random.SetActive(DataManager.DmUserInfo.optionData.LoginBonusFriends);
	}

	// Token: 0x06001485 RID: 5253 RVA: 0x000FA9E0 File Offset: 0x000F8BE0
	private void OnClickButton(PguiButtonCtrl button)
	{
		if (button == this.guiData.guiBtnFavorite.Btn_Favorite && this.currentMode == SelProfileCtrl.Mode.TOP)
		{
			this.guiData.profileAnimation.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
			{
				this.guiData.favoriteAnimation.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
				this.requestMode = SelProfileCtrl.Mode.FAVORITE_CHANGE;
			});
			return;
		}
		if (button == this.guiData.guiBtnLoginBonus.Btn_LoginBonus && this.currentMode == SelProfileCtrl.Mode.TOP)
		{
			if (this.currentEnumerator == null)
			{
				this.currentEnumerator = this.ChangeLoginBonus();
				return;
			}
		}
		else
		{
			if (button == this.guiData.guiProfile.Btn_Name && this.currentMode == SelProfileCtrl.Mode.TOP)
			{
				this.requestMode = SelProfileCtrl.Mode.NAME_CHANGE;
				return;
			}
			if (button == this.guiData.guiProfile.Btn_Comment && this.currentMode == SelProfileCtrl.Mode.TOP)
			{
				this.requestMode = SelProfileCtrl.Mode.COMMENT_CHANGE;
				return;
			}
			if (button == this.guiData.guiProfile.Btn_Id)
			{
				GUIUtility.systemCopyBuffer = DataManager.DmUserInfo.friendId.ToString();
				CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("コピーしました"), PrjUtil.MakeMessage("そのままペーストしてください"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, new PguiOpenWindowCtrl.Callback(this.OnClickOwButton), null, false);
				CanvasManager.HdlOpenWindowBasic.Open();
				return;
			}
			if (button == this.guiData.guiProfile.Btn_Gender)
			{
				this.avaterType ^= 1;
				this.guiData.guiProfile.Relpace_Photo.Replace(this.avaterType);
				return;
			}
			if (button == this.guiData.guiHelper.Btn_Helper)
			{
				this.guiData.profileAnimation.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
				{
					this.guiData.helperAnimation.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
					this.requestMode = SelProfileCtrl.Mode.HELPER_CHANGE;
				});
			}
		}
	}

	// Token: 0x06001486 RID: 5254 RVA: 0x000FAB9B File Offset: 0x000F8D9B
	private void OnClickCharaButton(IconCharaCtrl icc)
	{
		if (icc.charaPackData != null && this.currentFavoriteCharaId != icc.charaPackData.id)
		{
			this.currentEnumerator = this.RequestUpdateFavoriteChara(icc.charaPackData.id);
		}
	}

	// Token: 0x06001487 RID: 5255 RVA: 0x000FABD0 File Offset: 0x000F8DD0
	private bool OnClickOwButton(int index)
	{
		if (this.currentMode == SelProfileCtrl.Mode.NAME_CHANGE)
		{
			if (index == 1)
			{
				if (string.IsNullOrEmpty(this.windowNameChange.InputField.text))
				{
					this.windowNameChange.InputField.textComponent.text = "";
					this.windowNameChange.Txt_ErrorMassage.gameObject.SetActive(true);
					this.windowNameChange.Txt_ErrorMassage.text = "名前を入力してください";
				}
				else
				{
					this.currentEnumerator = this.RequestUpdateName(this.windowNameChange.InputField.text);
				}
				return false;
			}
			this.RefreshProfile();
			this.requestMode = this.preMode;
			return true;
		}
		else
		{
			if (this.currentMode != SelProfileCtrl.Mode.COMMENT_CHANGE)
			{
				return true;
			}
			if (index == 1)
			{
				this.currentEnumerator = this.RequestUpdateComment(this.windowCommentChange.InputField.text);
				return false;
			}
			this.RefreshProfile();
			this.requestMode = this.preMode;
			return true;
		}
	}

	// Token: 0x06001488 RID: 5256 RVA: 0x000FACBD File Offset: 0x000F8EBD
	private bool OnClickButtionChangeAchievement()
	{
		Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneAchievement, null);
		return true;
	}

	// Token: 0x06001489 RID: 5257 RVA: 0x000FACD0 File Offset: 0x000F8ED0
	private void OnStartCharaTop(int index, GameObject go)
	{
		GameObject gameObject = (GameObject)Resources.Load("SceneCharaEdit/GUI/Prefab/CharaGrow_Btn_CharaSelect");
		for (int i = 0; i < 3; i++)
		{
			GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject, go.transform);
			IconCharaCtrl icon = gameObject2.GetComponent<IconCharaCtrl>();
			gameObject2.name = i.ToString();
			gameObject2.GetComponent<PguiButtonCtrl>().AddOnClickListener(delegate(PguiButtonCtrl button)
			{
				this.OnClickCharaButton(icon);
			}, PguiButtonCtrl.SoundType.DEFAULT);
		}
	}

	// Token: 0x0600148A RID: 5258 RVA: 0x000FAD44 File Offset: 0x000F8F44
	private void OnUpdateCharaTop(int index, GameObject go)
	{
		for (int i = 0; i < 3; i++)
		{
			int num = index * 3 + i;
			Transform transform = go.transform.Find(i.ToString());
			IconCharaCtrl component = transform.GetComponent<IconCharaCtrl>();
			if (num < this.dispFavoriteCharaList.Count)
			{
				component.Setup(this.dispFavoriteCharaList[num], this.sortType, false, new CharaWindowCtrl.DetailParamSetting(CharaWindowCtrl.DetailParamSetting.Preset.MINE_EASY, null), 0, -1, 0);
				component.DispCurrentFrame(this.dispFavoriteCharaList[num].id == this.requestFavoriteCharaId);
			}
			else
			{
				transform.GetComponent<IconCharaCtrl>().Setup(null, this.sortType, false, new CharaWindowCtrl.DetailParamSetting(CharaWindowCtrl.DetailParamSetting.Preset.MINE_EASY, null), 0, -1, 0);
			}
		}
	}

	// Token: 0x0600148B RID: 5259 RVA: 0x000FADF4 File Offset: 0x000F8FF4
	public bool OnClickReturnButton()
	{
		if (this.currentMode == SelProfileCtrl.Mode.FAVORITE_CHANGE)
		{
			this.guiData.favoriteAnimation.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
			{
				this.guiData.profileAnimation.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
				this.requestMode = SelProfileCtrl.Mode.TOP;
			});
			return true;
		}
		if (this.currentMode == SelProfileCtrl.Mode.HELPER_CHANGE)
		{
			this.currentEnumerator = this.RequestUpdateLoanPack();
			return true;
		}
		return this.currentMode != SelProfileCtrl.Mode.TOP;
	}

	// Token: 0x0600148C RID: 5260 RVA: 0x000FAE4C File Offset: 0x000F904C
	private IEnumerator RequestEndAnimation()
	{
		this.guiData.profileAnimation.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
		{
			CanvasManager.HdlCmnMenu.MoveSceneByMenu(SceneManager.SceneName.SceneOtherMenuTop, null);
		});
		yield return null;
		yield break;
	}

	// Token: 0x0600148D RID: 5261 RVA: 0x000FAE5B File Offset: 0x000F905B
	public bool OnClickMoveSequenceButton(SceneManager.SceneName sceneName, object sceneArgs)
	{
		if (this.currentMode == SelProfileCtrl.Mode.HELPER_CHANGE)
		{
			this.currentEnumerator = this.RequestUpdateLoanPack();
			this.OnClickMoveSequenceName = sceneName;
			this.OnClickMoveSequenceArgs = sceneArgs;
			return true;
		}
		return false;
	}

	// Token: 0x0600148E RID: 5262 RVA: 0x000FAE83 File Offset: 0x000F9083
	private IEnumerator OpenBuffInfoWindow()
	{
		CanvasManager.HdlKizunaKizunaBuffWindowCtrl.SetupBuffInfo();
		CanvasManager.HdlKizunaKizunaBuffWindowCtrl.buffInfoData.owCtrl.Open();
		do
		{
			yield return null;
		}
		while (!CanvasManager.HdlKizunaKizunaBuffWindowCtrl.buffInfoData.owCtrl.FinishedClose());
		CanvasManager.HdlKizunaKizunaBuffWindowCtrl.buffInfoData.owCtrl.ForceClose();
		yield break;
	}

	// Token: 0x1700038A RID: 906
	// (get) Token: 0x0600148F RID: 5263 RVA: 0x000FAE8C File Offset: 0x000F908C
	private List<CharaPackData> DispLoanCharaList
	{
		get
		{
			List<CharaPackData> list = new List<CharaPackData>(DataManager.DmChara.GetUserCharaMap().Values);
			if (this.currentLoanAttribute != CharaDef.AttributeType.ALL)
			{
				list.RemoveAll((CharaPackData item) => item.staticData.baseData.attribute != this.currentLoanAttribute);
			}
			return list;
		}
	}

	// Token: 0x06001490 RID: 5264 RVA: 0x000FAECA File Offset: 0x000F90CA
	private bool IsChangeHelpers()
	{
		return this.isChangeHelpers || (this.selPhotoEditCtrl != null && this.selPhotoEditCtrl.isChangeClone);
	}

	// Token: 0x06001491 RID: 5265 RVA: 0x000FAEF1 File Offset: 0x000F90F1
	private void SelectPhoto()
	{
		this.OnClickPhotoButton(this.guiData.guiHelperChange.charaDeck.Btn_Photo, 0);
		this.guiData.guiHelperChange.charaDeck.Btn_Photo.SetToggleIndex(1);
	}

	// Token: 0x06001492 RID: 5266 RVA: 0x000FAF2B File Offset: 0x000F912B
	private void SelectAccessory()
	{
		this.OnClickAccessoryButton(this.guiData.guiHelperChange.charaDeck.Btn_Accessory, 0);
		this.guiData.guiHelperChange.charaDeck.Btn_Accessory.SetToggleIndex(1);
	}

	// Token: 0x06001493 RID: 5267 RVA: 0x000FAF68 File Offset: 0x000F9168
	private void InitByHelperDeck()
	{
		this.guiData.guiHelperChange.charaDeck.Btn_Chara.AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickCharaButton));
		this.guiData.guiHelperChange.charaDeck.Btn_Photo.AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickPhotoButton));
		this.guiData.guiHelperChange.charaDeck.Btn_Accessory.AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickAccessoryButton));
		this.guiData.guiHelperChange.charaDeck.Btn_Chara.SetToggleIndex(1);
		this.guiData.guiHelperChange.charaDeck.Btn_PhotoRemove.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonHelper), PguiButtonCtrl.SoundType.DECIDE);
		this.guiData.guiHelperChange.charaDeck.ScrollView.InitForce();
		ReuseScroll scrollView = this.guiData.guiHelperChange.charaDeck.ScrollView;
		scrollView.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollView.onStartItem, new Action<int, GameObject>(this.OnStartItemChara));
		ReuseScroll scrollView2 = this.guiData.guiHelperChange.charaDeck.ScrollView;
		scrollView2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollView2.onUpdateItem, new Action<int, GameObject>(this.OnUpdateItemChara));
		this.guiData.guiHelperChange.charaDeck.ScrollView.Setup(10, 0);
		for (int i = 0; i < this.guiData.guiHelperChange.charaDeck.iconCharaPacks.Count; i++)
		{
			this.guiData.guiHelperChange.charaDeck.iconCharaPacks[i].iconChara.Setup(new SelCharaDeckCtrl.GUI.IconChara.SetupParam
			{
				cbTouchCharaIcon = delegate(SelCharaDeckCtrl.FrameType type, SelCharaDeckCtrl.GUI.IconChara icon, int photo)
				{
					this.OnTouchCharaIcon(type, icon, photo);
					return true;
				},
				cbClickIconPhotoKind = delegate
				{
					this.SelectPhoto();
					return true;
				},
				isDeck = false,
				index = i
			});
		}
		this.guiData.guiHelperChange.charaDeck.DeckSelect.ExPauseAnimation(SimpleAnimation.ExPguiStatus.START, null);
		SortWindowCtrl.RegisterData registerData = new SortWindowCtrl.RegisterData
		{
			register = SortFilterDefine.RegisterType.CHARA_HELPER_LOAN,
			filterButton = this.guiData.guiHelperChange.charaDeck.Btn_FilterOnOff,
			sortButton = this.guiData.guiHelperChange.charaDeck.Btn_Sort,
			sortUdButton = this.guiData.guiHelperChange.charaDeck.Btn_SortUpDown,
			funcGetTargetBaseList = delegate
			{
				List<CharaPackData> list = new List<CharaPackData>(this.DispLoanCharaList);
				if (this.removeButtonCharaData != null)
				{
					list.Remove(this.removeButtonCharaData);
				}
				return new SortWindowCtrl.SortTarget
				{
					charaList = list
				};
			},
			funcDisideTarget = delegate(SortWindowCtrl.SortTarget item)
			{
				this.dispLoanCharaList = new List<CharaPackData>(item.charaList);
				int num = this.dispLoanCharaList.Count;
				if (this.currentLoanAttribute != CharaDef.AttributeType.ALL)
				{
					this.dispLoanCharaList.Insert(0, this.removeButtonCharaData);
					num = this.dispLoanCharaList.Count;
					num--;
				}
				this.sortTypeLoan = item.sortType;
				this.guiData.guiHelperChange.charaDeck.ResizeScrollView(num, this.dispLoanCharaList.Count / 2 + 1);
				this.RefreshDeckFrame();
			}
		};
		CanvasManager.HdlOpenWindowSortFilter.Register(registerData, false, null);
	}

	// Token: 0x06001494 RID: 5268 RVA: 0x000FB1FC File Offset: 0x000F93FC
	public static void DeepCopyLoanPackList(List<LoanPackData> from, List<LoanPackData> to)
	{
		to.Clear();
		for (int i = 0; i < from.Count; i++)
		{
			to.Add(new LoanPackData
			{
				charaId = from[i].charaId,
				photoDataIdList = new List<long>(from[i].photoDataIdList)
			});
		}
	}

	// Token: 0x06001495 RID: 5269 RVA: 0x000FB254 File Offset: 0x000F9454
	private void SetupDeckTop(bool selectPhoto, bool selectAccessory)
	{
		SelProfileCtrl.DeepCopyLoanPackList(DataManager.DmUserInfo.loanPackList, this.cloneLoanPackList);
		SelProfileCtrl.DeepCopyLoanPackList(DataManager.DmUserInfo.loanPackList, this.disideLoanPackList);
		this.currentHelperMode = SelProfileCtrl.HelperMode.DECK_TOP;
		this.guiData.guiHelperChange.charaDeck.baseObj.SetActive(true);
		this.guiData.guiHelperChange.charaDeck.Btn_Chara.SetToggleIndex(1);
		this.guiData.guiHelperChange.charaDeck.Btn_Photo.SetToggleIndex(0);
		this.guiData.guiHelperChange.charaDeck.Btn_Accessory.SetToggleIndex(0);
		this.guiData.guiHelperChange.charaDeck.DeckSelect.ExPauseAnimation(SimpleAnimation.ExPguiStatus.START, null);
		foreach (SelCharaDeckCtrl.GUI.IconCharaPack iconCharaPack in this.guiData.guiHelperChange.charaDeck.iconCharaPacks)
		{
			iconCharaPack.iconChara.anime.ExPauseAnimationLastFrame(SimpleAnimation.ExPguiStatus.END);
		}
		this.guiData.guiHelperChange.charaDeck.Btn_EditOk.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonHelper), PguiButtonCtrl.SoundType.DECIDE);
		this.RefreshDeckFrame();
		this.ResetCurrentIcon();
		this.isChangeHelpers = false;
		if (this.selPhotoEditCtrl != null)
		{
			this.selPhotoEditCtrl.isChangeClone = false;
		}
		this.guiData.guiHelperChange.charaDeck.Btn_PhotoRemove.transform.parent.GetComponent<SimpleAnimation>().ExPauseAnimationLastFrame(SimpleAnimation.ExPguiStatus.END);
		this.guiData.guiHelperChange.charaDeck.Btn_PhotoRemove.transform.parent.gameObject.SetActive(false);
		if (selectPhoto)
		{
			this.SelectPhoto();
			return;
		}
		if (selectAccessory)
		{
			this.SelectAccessory();
		}
	}

	// Token: 0x06001496 RID: 5270 RVA: 0x000FB434 File Offset: 0x000F9634
	private void RefreshDeckFrame()
	{
		for (int i = 0; i < this.guiData.guiHelperChange.charaDeck.iconCharaPacks.Count; i++)
		{
			CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(this.cloneLoanPackList[i].charaId);
			SelCharaDeckCtrl.GUI.IconChara icon = this.guiData.guiHelperChange.charaDeck.iconCharaPacks[i].iconChara;
			icon.iconCharaSet.iconCharaCtrl.Setup((userCharaData != null) ? userCharaData : CharaPackData.MakeInvalid(), SortFilterDefine.SortType.LEVEL, false, new CharaWindowCtrl.DetailParamSetting(CharaWindowCtrl.DetailParamSetting.Preset.MINE_EASY_NO_KEMOBOARD, null), 0, -1, 0);
			icon.iconCharaSet.iconCharaCtrl.DispPhotoPocketLevel(true);
			List<PhotoPackData> equipPhotoList = this.cloneLoanPackList[i].photoDataIdList.FindAll((long item) => DataManager.DmPhoto.GetUserPhotoData(item) != null).ConvertAll<PhotoPackData>((long item) => DataManager.DmPhoto.GetUserPhotoData(item));
			IconPhotoCtrl.OnReturnPhotoPackDataList <>9__2;
			IconPhotoCtrl.OnUpdateLockFlag <>9__3;
			for (int j = 0; j < this.cloneLoanPackList[i].photoDataIdList.Count; j++)
			{
				PhotoPackData userPhotoData = DataManager.DmPhoto.GetUserPhotoData(this.cloneLoanPackList[i].photoDataIdList[j]);
				icon.iconPhotoCtrl[j].Setup(userPhotoData, SortFilterDefine.SortType.LEVEL, true, false, -1, true);
				if (userPhotoData != null && !userPhotoData.IsInvalid())
				{
					icon.iconPhotoKind[j].Replace(userPhotoData.staticData.baseData.type);
				}
				else
				{
					icon.iconPhotoKind[j].Replace(-1);
				}
				if (icon.iconBlankFrame != null)
				{
					bool flag = userCharaData != null && !userCharaData.IsInvalid() && userCharaData.dynamicData.PhotoPocket[j].Flag;
					bool flag2 = userPhotoData != null && !userPhotoData.IsInvalid();
					bool flag3 = userPhotoData != null && !userPhotoData.IsInvalid() && userPhotoData.staticData.baseData.kizunaPhotoFlg;
					bool flag4 = flag3 && (userCharaData == null || userCharaData.IsInvalid() || userPhotoData.staticData.GetId() != userCharaData.staticData.baseData.kizunaPhotoId);
					int num = ((userCharaData == null || userCharaData.IsInvalid()) ? 0 : userCharaData.dynamicData.PhotoPocket[j].Step);
					if (icon != null)
					{
						SelCharaDeckCtrl.DecorationPhotoFrame(icon, j, flag2, flag, flag3, flag4, num, false);
					}
				}
				IconPhotoCtrl iconPhotoCtrl = icon.iconPhotoCtrl[j];
				IconPhotoCtrl.OnReturnPhotoPackDataList onReturnPhotoPackDataList;
				if ((onReturnPhotoPackDataList = <>9__2) == null)
				{
					onReturnPhotoPackDataList = (<>9__2 = () => equipPhotoList);
				}
				iconPhotoCtrl.onReturnPhotoPackDataList = onReturnPhotoPackDataList;
				IconPhotoCtrl iconPhotoCtrl2 = icon.iconPhotoCtrl[j];
				IconPhotoCtrl.OnUpdateLockFlag onUpdateLockFlag;
				if ((onUpdateLockFlag = <>9__3) == null)
				{
					onUpdateLockFlag = (<>9__3 = delegate(IconPhotoCtrl item)
					{
						foreach (IconPhotoCtrl iconPhotoCtrl3 in icon.iconPhotoCtrl)
						{
							iconPhotoCtrl3.Setup(iconPhotoCtrl3.photoPackData, SortFilterDefine.SortType.LEVEL, true, false, -1, true);
						}
					});
				}
				iconPhotoCtrl2.AddOnUpdateStatus(onUpdateLockFlag);
			}
			DataManagerCharaAccessory.Accessory accessory = ((userCharaData != null) ? userCharaData.dynamicData.accessory : null);
			icon.iconAccessoryCtrl.Setup(new IconAccessoryCtrl.SetupParam
			{
				acce = accessory
			});
			SelCharaDeckCtrl.DecorationAccessoryFrame(icon, userCharaData != null, false);
		}
		bool flag5 = false;
		int num2 = 0;
		while (!flag5 && num2 < this.cloneLoanPackList.Count)
		{
			int num3 = 0;
			while (!flag5 && num3 < this.cloneLoanPackList[num2].photoDataIdList.Count)
			{
				if (this.cloneLoanPackList[num2].photoDataIdList[num3] > 0L)
				{
					flag5 = true;
				}
				num3++;
			}
			num2++;
		}
		this.guiData.guiHelperChange.charaDeck.Btn_PhotoRemove.SetActEnable(flag5, false, false);
	}

	// Token: 0x06001497 RID: 5271 RVA: 0x000FB835 File Offset: 0x000F9A35
	private void RefreshReserveFrame(SelCharaDeckCtrl.GUI.IconChara iconChara)
	{
		this.currentLoanAttribute = (CharaDef.AttributeType)this.GetIconCharaIndex(iconChara);
		this.dispLoanCharaList = this.DispLoanCharaList;
		CanvasManager.HdlOpenWindowSortFilter.SolutionList(SortFilterDefine.RegisterType.CHARA_HELPER_LOAN, null);
	}

	// Token: 0x06001498 RID: 5272 RVA: 0x000FB860 File Offset: 0x000F9A60
	private int GetIconCharaIndex(SelCharaDeckCtrl.GUI.IconChara iconChara)
	{
		return this.guiData.guiHelperChange.charaDeck.iconCharaPacks.FindIndex((SelCharaDeckCtrl.GUI.IconCharaPack item) => item.iconChara == iconChara);
	}

	// Token: 0x06001499 RID: 5273 RVA: 0x000FB8A0 File Offset: 0x000F9AA0
	private void SelectCharaIcon(SelProfileCtrl.SelectCharaData newSelectChara, SelProfileCtrl.SelectCharaData oldSelectChara)
	{
		List<PguiAECtrl> list = new List<PguiAECtrl>();
		bool flag = false;
		if (oldSelectChara == null)
		{
			newSelectChara.chara.iconCharaSet.currentFrame.SetActive(true);
			this.selectCharaData = newSelectChara;
			this.RefreshReserveFrame(newSelectChara.chara);
		}
		else
		{
			SelCharaDeckCtrl.GUI.IconChara chara = newSelectChara.chara;
			SelCharaDeckCtrl.GUI.IconChara chara2 = oldSelectChara.chara;
			if (newSelectChara.type == SelCharaDeckCtrl.FrameType.RESERVE && oldSelectChara.type == SelCharaDeckCtrl.FrameType.DECK)
			{
				SelProfileCtrl.SelectCharaData resvChara = ((newSelectChara.type == SelCharaDeckCtrl.FrameType.RESERVE) ? newSelectChara : oldSelectChara);
				SelProfileCtrl.SelectCharaData selectCharaData = ((newSelectChara.type == SelCharaDeckCtrl.FrameType.DECK) ? newSelectChara : oldSelectChara);
				int iconCharaIndex = this.GetIconCharaIndex(selectCharaData.chara);
				if (!resvChara.chara.iconCharaSet.selected.activeSelf && !resvChara.chara.iconCharaSet.disable.activeSelf && resvChara.chara.iconCharaSet.iconCharaCtrl.charaPackData.id != selectCharaData.chara.iconCharaSet.iconCharaCtrl.charaPackData.id)
				{
					LoanPackData loanPackData;
					if ((loanPackData = this.cloneLoanPackList.Find((LoanPackData item) => item.charaId == resvChara.chara.iconCharaSet.iconCharaCtrl.charaPackData.id)) != null && loanPackData.charaId != 0)
					{
						int num = this.cloneLoanPackList.IndexOf(loanPackData);
						int charaId = this.cloneLoanPackList[num].charaId;
						this.cloneLoanPackList[num].charaId = this.cloneLoanPackList[iconCharaIndex].charaId;
						this.cloneLoanPackList[iconCharaIndex].charaId = charaId;
						list.Add(chara.iconCharaSet.aeEffChange);
						list.Add(chara2.iconCharaSet.aeEffChange);
						list.Add(this.guiData.guiHelperChange.charaDeck.iconCharaPacks[num].iconChara.iconCharaSet.aeEffChange);
						list.Add(this.guiData.guiHelperChange.charaDeck.iconCharaPacks[iconCharaIndex].iconChara.iconCharaSet.aeEffChange);
						this.isChangeHelpers = true;
						this.RefreshDeckFrame();
						this.RefreshReserveFrame(selectCharaData.chara);
					}
					else
					{
						list.Add(chara.iconCharaSet.aeEffChange);
						list.Add(chara2.iconCharaSet.aeEffChange);
						this.cloneLoanPackList[iconCharaIndex].charaId = resvChara.chara.iconCharaSet.iconCharaCtrl.charaPackData.id;
						flag = this.cloneLoanPackList[iconCharaIndex].charaId == 0;
						this.isChangeHelpers = true;
						this.RefreshDeckFrame();
						this.RefreshReserveFrame(selectCharaData.chara);
					}
				}
			}
			else if (newSelectChara.type == SelCharaDeckCtrl.FrameType.DECK && oldSelectChara.type == SelCharaDeckCtrl.FrameType.DECK && chara != chara2)
			{
				chara.iconCharaSet.currentFrame.SetActive(true);
				chara2.iconCharaSet.currentFrame.SetActive(false);
				this.selectCharaData = newSelectChara;
				this.RefreshReserveFrame(chara);
			}
		}
		if (list.Count > 0)
		{
			SoundManager.Play(flag ? "prd_se_cancel" : "prd_se_click", false, false);
			for (int i = 0; i < list.Count; i++)
			{
				list[i].PlayAnimation(PguiAECtrl.AmimeType.START, null);
			}
			return;
		}
		SoundManager.Play("prd_se_click", false, false);
	}

	// Token: 0x0600149A RID: 5274 RVA: 0x000FBC10 File Offset: 0x000F9E10
	private void ResetCurrentIcon()
	{
		if (this.selectCharaData != null)
		{
			this.selectCharaData.chara.iconCharaSet.currentFrame.SetActive(false);
			this.selectCharaData = null;
		}
	}

	// Token: 0x0600149B RID: 5275 RVA: 0x000FBC3C File Offset: 0x000F9E3C
	private IEnumerator RequestCharaDeck()
	{
		this.currentHelperMode = SelProfileCtrl.HelperMode.DECK_TOP;
		if (this.isChangeHelpers)
		{
			SelProfileCtrl.DeepCopyLoanPackList(this.cloneLoanPackList, this.disideLoanPackList);
			this.requestActionUpdateLoanPackList = this.RequestActionUpdateLoanPackList();
			while (this.requestActionUpdateLoanPackList != null)
			{
				yield return null;
			}
			this.isChangeHelpers = false;
		}
		this.ResetCurrentIcon();
		this.RefreshDeckFrame();
		yield break;
	}

	// Token: 0x0600149C RID: 5276 RVA: 0x000FBC4B File Offset: 0x000F9E4B
	private IEnumerator RequestPhotoDeck()
	{
		this.currentHelperMode = SelProfileCtrl.HelperMode.PHOTO_TOP;
		if (this.selPhotoEditCtrl.isChangeClone)
		{
			SelProfileCtrl.DeepCopyLoanPackList(this.cloneLoanPackList, this.disideLoanPackList);
			this.requestActionUpdateLoanPackList = this.RequestActionUpdateLoanPackList();
			while (this.requestActionUpdateLoanPackList != null)
			{
				yield return null;
			}
			this.selPhotoEditCtrl.isChangeClone = false;
		}
		this.selPhotoEditCtrl.NotActive(delegate
		{
			this.guiData.guiHelperChange.charaDeck.baseObj.SetActive(true);
		});
		this.ResetCurrentIcon();
		this.RefreshDeckFrame();
		yield break;
	}

	// Token: 0x0600149D RID: 5277 RVA: 0x000FBC5A File Offset: 0x000F9E5A
	private void RequestAccessoryDeck()
	{
		this.ResetCurrentIcon();
		this.RefreshDeckFrame();
	}

	// Token: 0x0600149E RID: 5278 RVA: 0x000FBC68 File Offset: 0x000F9E68
	private void OnClickButtonHelper(PguiButtonCtrl button)
	{
		if (this.currentHelperMode == SelProfileCtrl.HelperMode.DECK_EDIT && button == this.guiData.guiHelperChange.charaDeck.Btn_EditOk)
		{
			this.guiData.guiHelperChange.charaDeck.DeckSelect.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
			{
			});
			Singleton<SceneManager>.Instance.StartCoroutine(this.RequestCharaDeck());
			return;
		}
		if (this.currentHelperMode == SelProfileCtrl.HelperMode.PHOTO_TOP && button == this.guiData.guiHelperChange.charaDeck.Btn_PhotoRemove)
		{
			if (!CanvasManager.HdlOpenWindowBasic.FinishedClose())
			{
				return;
			}
			bool flag = false;
			int num = 0;
			while (!flag && num < this.cloneLoanPackList.Count)
			{
				int num2 = 0;
				while (!flag && num2 < this.cloneLoanPackList[num].photoDataIdList.Count)
				{
					if (this.cloneLoanPackList[num].photoDataIdList[num2] > 0L)
					{
						flag = true;
					}
					num2++;
				}
				num++;
			}
			if (flag)
			{
				CanvasManager.HdlOpenWindowBasic.Setup("フォト一括解除確認", "装着されている全てのフォトを外します。\n\nよろしいですか？", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, delegate(int idx)
				{
					if (idx == 1)
					{
						this.RemovePhoto();
					}
					return true;
				}, null, false);
				CanvasManager.HdlOpenWindowBasic.Open();
			}
		}
	}

	// Token: 0x0600149F RID: 5279 RVA: 0x000FBDB4 File Offset: 0x000F9FB4
	private void RemovePhoto()
	{
		for (int i = 0; i < this.cloneLoanPackList.Count; i++)
		{
			for (int j = 0; j < this.cloneLoanPackList[i].photoDataIdList.Count; j++)
			{
				if (this.cloneLoanPackList[i].photoDataIdList[j] > 0L)
				{
					this.cloneLoanPackList[i].photoDataIdList[j] = 0L;
				}
			}
		}
		this.isChangeHelpers = true;
		this.RefreshDeckFrame();
	}

	// Token: 0x060014A0 RID: 5280 RVA: 0x000FBE3C File Offset: 0x000FA03C
	private bool OnClickCharaButton(PguiToggleButtonCtrl pbc, int toggleIndex)
	{
		if (this.currentHelperMode != SelProfileCtrl.HelperMode.DECK_TOP)
		{
			string text = ((this.currentHelperMode == SelProfileCtrl.HelperMode.PHOTO_TOP) ? "END" : "END2");
			this.currentHelperMode = SelProfileCtrl.HelperMode.DECK_TOP;
			for (int i = 0; i < this.guiData.guiHelperChange.charaDeck.iconCharaPacks.Count; i++)
			{
				SelCharaDeckCtrl.GUI.IconChara iconChara = this.guiData.guiHelperChange.charaDeck.iconCharaPacks[i].iconChara;
				if (!iconChara.PhotoIconKind.gameObject.activeSelf)
				{
					iconChara.PhotoIconKind.gameObject.SetActive(true);
				}
				iconChara.anime.ExPlayAnimation(text, null);
			}
			this.guiData.guiHelperChange.charaDeck.Btn_Photo.SetToggleIndex(0);
			this.guiData.guiHelperChange.charaDeck.Btn_Accessory.SetToggleIndex(0);
			return true;
		}
		return false;
	}

	// Token: 0x060014A1 RID: 5281 RVA: 0x000FBF24 File Offset: 0x000FA124
	private bool OnClickPhotoButton(PguiToggleButtonCtrl pbc, int toggleIndex)
	{
		if (this.currentHelperMode != SelProfileCtrl.HelperMode.PHOTO_TOP)
		{
			string text = ((this.currentHelperMode == SelProfileCtrl.HelperMode.DECK_TOP) ? "START" : "END3");
			this.currentHelperMode = SelProfileCtrl.HelperMode.PHOTO_TOP;
			for (int i = 0; i < this.guiData.guiHelperChange.charaDeck.iconCharaPacks.Count; i++)
			{
				this.guiData.guiHelperChange.charaDeck.iconCharaPacks[i].iconChara.anime.ExPlayAnimation(text, null);
			}
			this.guiData.guiHelperChange.charaDeck.Btn_Chara.SetToggleIndex(0);
			this.guiData.guiHelperChange.charaDeck.Btn_Accessory.SetToggleIndex(0);
			return true;
		}
		return false;
	}

	// Token: 0x060014A2 RID: 5282 RVA: 0x000FBFE4 File Offset: 0x000FA1E4
	private bool OnClickAccessoryButton(PguiToggleButtonCtrl pbc, int toggleIndex)
	{
		if (this.currentHelperMode != SelProfileCtrl.HelperMode.ACCESSORY_TOP)
		{
			string text = ((this.currentHelperMode == SelProfileCtrl.HelperMode.DECK_TOP) ? "START2" : "START3");
			this.currentHelperMode = SelProfileCtrl.HelperMode.ACCESSORY_TOP;
			for (int i = 0; i < this.guiData.guiHelperChange.charaDeck.iconCharaPacks.Count; i++)
			{
				SelCharaDeckCtrl.GUI.IconChara et = this.guiData.guiHelperChange.charaDeck.iconCharaPacks[i].iconChara;
				et.anime.ExPlayAnimation(text, delegate
				{
					et.PhotoIconKind.gameObject.SetActive(false);
				});
			}
			this.guiData.guiHelperChange.charaDeck.Btn_Chara.SetToggleIndex(0);
			this.guiData.guiHelperChange.charaDeck.Btn_Photo.SetToggleIndex(0);
			return true;
		}
		return false;
	}

	// Token: 0x060014A3 RID: 5283 RVA: 0x000FC0C4 File Offset: 0x000FA2C4
	private void OnStartItemChara(int index, GameObject go)
	{
		for (int i = 0; i < 2; i++)
		{
			SelCharaDeckCtrl.GUI.IconChara et = new SelCharaDeckCtrl.GUI.IconChara(go.transform.Find("Icon_Chara0" + (i + 1).ToString()), go.transform.Find("AEImage_Mark_Ban0" + (i + 1).ToString()), -1);
			et.iconCharaSet.iconCharaCtrl.AddOnClickListener(delegate(IconCharaCtrl x)
			{
				this.OnTouchCharaIcon(SelCharaDeckCtrl.FrameType.RESERVE, et, -1);
			});
			Transform transform = et.baseObj.transform.Find("Icon_CharaSet(Clone)/Remove");
			if (transform != null)
			{
				PrjUtil.AddTouchEventTrigger(transform.gameObject, delegate(Transform x)
				{
					this.OnTouchCharaIcon(SelCharaDeckCtrl.FrameType.RESERVE, et, -1);
				});
			}
			this.guiData.guiHelperChange.reserveCharaIcon.Add(et);
		}
	}

	// Token: 0x060014A4 RID: 5284 RVA: 0x000FC1B4 File Offset: 0x000FA3B4
	private void OnUpdateItemChara(int index, GameObject go)
	{
		for (int i = 0; i < 2; i++)
		{
			GameObject iconObj = go.transform.Find("Icon_Chara0" + (i + 1).ToString()).gameObject;
			SelCharaDeckCtrl.GUI.IconChara iconChara = this.guiData.guiHelperChange.reserveCharaIcon.Find((SelCharaDeckCtrl.GUI.IconChara item) => item.baseObj == iconObj);
			int num = index * 2 + i;
			if (this.dispLoanCharaList.Count > num)
			{
				iconChara.baseObj.SetActive(true);
				CharaPackData cpd = this.dispLoanCharaList[num];
				iconChara.iconCharaSet.iconCharaCtrl.Setup(cpd, this.sortTypeLoan, false, new CharaWindowCtrl.DetailParamSetting(CharaWindowCtrl.DetailParamSetting.Preset.MINE_EASY_NO_KEMOBOARD, null), 0, -1, 0);
				iconChara.iconCharaSet.iconCharaCtrl.DispPhotoPocketLevel(true);
				iconChara.iconCharaSet.removeFrame.SetActive(cpd == this.removeButtonCharaData);
				bool flag = this.cloneLoanPackList.Find((LoanPackData item) => item.charaId == cpd.id) != null;
				CharaStaticBase csb = ((cpd.id <= 0 || cpd.staticData == null) ? null : cpd.staticData.baseData);
				bool flag2 = this.cloneLoanPackList.Find((LoanPackData item) => item.charaId > 0 && (this.selectCharaData == null || this.selectCharaData.chara.iconCharaSet.iconCharaCtrl.charaPackData.id != item.charaId) && csb != null && DataManager.DmChara.CheckSameChara(item.charaId, cpd.id)) != null;
				iconChara.iconCharaSet.selected.SetActive(flag && cpd != this.removeButtonCharaData);
				iconChara.iconCharaSet.disable.SetActive(!flag && flag2 && cpd != this.removeButtonCharaData);
				iconChara.iconCharaSet.iconCharaCtrl.IsEnableMask(iconChara.iconCharaSet.selected.activeSelf || iconChara.iconCharaSet.disable.activeSelf);
			}
			else
			{
				iconChara.baseObj.SetActive(false);
			}
		}
	}

	// Token: 0x060014A5 RID: 5285 RVA: 0x000FC3DC File Offset: 0x000FA5DC
	private void OnTouchCharaIcon(SelCharaDeckCtrl.FrameType type, SelCharaDeckCtrl.GUI.IconChara iconChara, int selPhoto)
	{
		if (this.currentHelperMode == SelProfileCtrl.HelperMode.DECK_TOP)
		{
			this.guiData.guiHelperChange.charaDeck.DeckSelect.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
			this.currentHelperMode = SelProfileCtrl.HelperMode.DECK_EDIT;
			this.RefreshReserveFrame(iconChara);
			this.SelectCharaIcon(new SelProfileCtrl.SelectCharaData(type, iconChara), this.selectCharaData);
			return;
		}
		if (this.currentHelperMode == SelProfileCtrl.HelperMode.DECK_EDIT)
		{
			this.SelectCharaIcon(new SelProfileCtrl.SelectCharaData(type, iconChara), this.selectCharaData);
			return;
		}
		if (this.currentHelperMode == SelProfileCtrl.HelperMode.PHOTO_TOP)
		{
			this.currentHelperMode = SelProfileCtrl.HelperMode.PHOTO_EDIT;
			this.guiData.guiHelperChange.charaDeck.baseObj.SetActive(false);
			if (this.selPhotoEditCtrl == null)
			{
				GameObject gameObject = new GameObject("PhotoEdit");
				RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
				rectTransform.anchorMin = new Vector2(0f, 0f);
				rectTransform.anchorMax = new Vector2(1f, 1f);
				rectTransform.offsetMin = new Vector2(0f, 0f);
				rectTransform.offsetMax = new Vector2(0f, 0f);
				this.selPhotoEditCtrl = gameObject.AddComponent<SelPhotoEditCtrl>();
				this.selPhotoEditCtrl.Init(SelPhotoEditCtrl.Type.Asistant);
				this.selPhotoEditCtrl.transform.SetParent(base.transform);
				Object.Destroy(this.selPhotoEditCtrl.GetComponent<SafeAreaScaler>());
			}
			this.selPhotoEditCtrl.Setup(new SelPhotoEditCtrl.SetupParam
			{
				cbButtonEditOk = delegate(PguiButtonCtrl button)
				{
					Singleton<SceneManager>.Instance.StartCoroutine(this.RequestPhotoDeck());
					return true;
				},
				cbButtonArrow = delegate(PguiButtonCtrl button)
				{
					this.selPhotoEditCtrl.ResetCurrentIcon();
					this.selPhotoEditCtrl.setupParam.indexByDeckChara += ((button == this.selPhotoEditCtrl.guiData.photoDeck.Btn_Yaji_Left) ? (-1) : 1);
					this.selPhotoEditCtrl.setupParam.indexByDeckChara = (this.selPhotoEditCtrl.setupParam.indexByDeckChara + 7) % 7;
					this.selPhotoEditCtrl.ChangePhotoInfo(-1);
					this.selPhotoEditCtrl.guiData.photoDeck.ScrollView.Refresh();
					return true;
				},
				indexByDeckChara = this.GetIconCharaIndex(iconChara),
				cbGetEquipPhoto = () => this.cloneLoanPackList[this.selPhotoEditCtrl.setupParam.indexByDeckChara].photoDataIdList,
				cbGetCharaPackData = () => DataManager.DmChara.GetUserCharaData(this.cloneLoanPackList[this.selPhotoEditCtrl.setupParam.indexByDeckChara].charaId),
				CurrentUserPartyId = () => this.cloneLoanPackList.ConvertAll<int>((LoanPackData x) => x.charaId),
				cbGetAllEquipPhoto = delegate
				{
					List<long> list = new List<long>();
					for (int i = 0; i < this.cloneLoanPackList.Count; i++)
					{
						for (int j = 0; j < this.cloneLoanPackList[i].photoDataIdList.Count; j++)
						{
							long num = this.cloneLoanPackList[i].photoDataIdList[j];
							if (num > 0L && !list.Contains(num))
							{
								list.Add(num);
							}
						}
					}
					return list;
				},
				cbIsEquipPhoto = delegate(long photoDataId)
				{
					int num2 = 0;
					Predicate<long> <>9__10;
					for (int k = 0; k < this.cloneLoanPackList.Count; k++)
					{
						List<long> photoDataIdList = this.cloneLoanPackList[k].photoDataIdList;
						Predicate<long> predicate;
						if ((predicate = <>9__10) == null)
						{
							predicate = (<>9__10 = (long item) => item == photoDataId);
						}
						if (photoDataIdList.Find(predicate) > 0L)
						{
							return this.cloneLoanPackList[k].charaId;
						}
					}
					return num2;
				},
				cbSetPartyName = () => CharaDef.GetAttributeName((CharaDef.AttributeType)this.selPhotoEditCtrl.setupParam.indexByDeckChara),
				cbResignEquipPhotoByDataId = delegate(long photoDataId)
				{
					for (int l = 0; l < this.cloneLoanPackList.Count; l++)
					{
						for (int m = 0; m < this.cloneLoanPackList[l].photoDataIdList.Count; m++)
						{
							if (this.cloneLoanPackList[l].photoDataIdList[m] == photoDataId)
							{
								this.cloneLoanPackList[l].photoDataIdList[m] = 0L;
							}
						}
					}
					return true;
				},
				deckCategory = UserDeckData.Category.NORMAL,
				isHelper = true
			});
			this.ResetCurrentIcon();
			this.selPhotoEditCtrl.ChangePhotoInfo(selPhoto);
			return;
		}
		if (this.currentHelperMode == SelProfileCtrl.HelperMode.ACCESSORY_TOP)
		{
			int iconCharaIndex = this.GetIconCharaIndex(iconChara);
			CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(this.cloneLoanPackList[iconCharaIndex].charaId);
			if (userCharaData != null)
			{
				CanvasManager.HdlDetachableAccessoryWindowCtrl.Open(userCharaData, delegate
				{
					this.RequestAccessoryDeck();
				});
			}
			SoundManager.Play("prd_se_click", false, false);
		}
	}

	// Token: 0x040010CA RID: 4298
	public bool isDebug;

	// Token: 0x040010CB RID: 4299
	private SelProfileCtrl.GUI guiData;

	// Token: 0x040010CC RID: 4300
	private SelPhotoEditCtrl selPhotoEditCtrl;

	// Token: 0x040010CD RID: 4301
	private SortFilterDefine.SortType sortType = SortFilterDefine.SortType.LEVEL;

	// Token: 0x040010CE RID: 4302
	private SelProfileCtrl.WindowCommentChange windowCommentChange;

	// Token: 0x040010CF RID: 4303
	private SelProfileCtrl.WindowNameChange windowNameChange;

	// Token: 0x040010D0 RID: 4304
	private SelProfileCtrl.Mode requestMode;

	// Token: 0x040010D1 RID: 4305
	private SelProfileCtrl.Mode _currentMode;

	// Token: 0x040010D2 RID: 4306
	private SelProfileCtrl.Mode _preMode;

	// Token: 0x040010D3 RID: 4307
	private bool getStart;

	// Token: 0x040010D4 RID: 4308
	private SelProfileCtrl.HelperMode currentHelperMode;

	// Token: 0x040010D5 RID: 4309
	private IEnumerator currentEnumerator;

	// Token: 0x040010D6 RID: 4310
	private IEnumerator requestActionUpdateLoanPackList;

	// Token: 0x040010D7 RID: 4311
	private PguiRenderTextureCharaCtrl renderTextureChara;

	// Token: 0x040010D8 RID: 4312
	private List<CharaPackData> dispFavoriteCharaList;

	// Token: 0x040010D9 RID: 4313
	private int currentFavoriteCharaId;

	// Token: 0x040010DA RID: 4314
	private int requestFavoriteCharaId;

	// Token: 0x040010DB RID: 4315
	private int avaterType;

	// Token: 0x040010DC RID: 4316
	private bool isHelperSettingStartFromCharaEdit;

	// Token: 0x040010DD RID: 4317
	private bool isFromBattleSelecter;

	// Token: 0x040010DE RID: 4318
	public UnityAction<bool> setActiveQuestMapDataCB;

	// Token: 0x040010DF RID: 4319
	private bool isSetup;

	// Token: 0x040010E0 RID: 4320
	private SceneManager.SceneName OnClickMoveSequenceName;

	// Token: 0x040010E1 RID: 4321
	private object OnClickMoveSequenceArgs;

	// Token: 0x040010E2 RID: 4322
	private bool isChangeHelpers;

	// Token: 0x040010E3 RID: 4323
	private List<LoanPackData> cloneLoanPackList = new List<LoanPackData>();

	// Token: 0x040010E4 RID: 4324
	private List<LoanPackData> disideLoanPackList = new List<LoanPackData>();

	// Token: 0x040010E5 RID: 4325
	private List<CharaPackData> dispLoanCharaList = new List<CharaPackData>();

	// Token: 0x040010E6 RID: 4326
	private List<PhotoPackData> dispLoanPhotoList = new List<PhotoPackData>();

	// Token: 0x040010E7 RID: 4327
	private CharaDef.AttributeType currentLoanAttribute;

	// Token: 0x040010E8 RID: 4328
	private SelProfileCtrl.SelectCharaData selectCharaData;

	// Token: 0x040010E9 RID: 4329
	private CharaPackData removeButtonCharaData = CharaPackData.MakeInvalid();

	// Token: 0x040010EA RID: 4330
	private PhotoPackData removeButtonPhotoData = PhotoPackData.MakeInvalid();

	// Token: 0x040010EB RID: 4331
	private int currentHelperDeckIndexByPhotoEdit;

	// Token: 0x040010EC RID: 4332
	private SortFilterDefine.SortType sortTypeLoan = SortFilterDefine.SortType.LEVEL;

	// Token: 0x02000B7B RID: 2939
	public enum Mode
	{
		// Token: 0x040047BA RID: 18362
		INVALID,
		// Token: 0x040047BB RID: 18363
		TOP,
		// Token: 0x040047BC RID: 18364
		FAVORITE_CHANGE,
		// Token: 0x040047BD RID: 18365
		NAME_CHANGE,
		// Token: 0x040047BE RID: 18366
		COMMENT_CHANGE,
		// Token: 0x040047BF RID: 18367
		HELPER_CHANGE
	}

	// Token: 0x02000B7C RID: 2940
	public class GuiBtnFavorite
	{
		// Token: 0x06004311 RID: 17169 RVA: 0x00201DD7 File Offset: 0x001FFFD7
		public GuiBtnFavorite(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_Favorite = baseTr.GetComponent<PguiButtonCtrl>();
		}

		// Token: 0x040047C0 RID: 18368
		public GameObject baseObj;

		// Token: 0x040047C1 RID: 18369
		public PguiButtonCtrl Btn_Favorite;
	}

	// Token: 0x02000B7D RID: 2941
	public class GuiBtnLoginBonus
	{
		// Token: 0x06004312 RID: 17170 RVA: 0x00201DF8 File Offset: 0x001FFFF8
		public GuiBtnLoginBonus(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_LoginBonus = baseTr.GetComponent<PguiButtonCtrl>();
			this.favorite = baseTr.Find("BaseImage/Chara_Favorite").gameObject;
			this.random = baseTr.Find("BaseImage/Chara_Random").gameObject;
		}

		// Token: 0x040047C2 RID: 18370
		public GameObject baseObj;

		// Token: 0x040047C3 RID: 18371
		public PguiButtonCtrl Btn_LoginBonus;

		// Token: 0x040047C4 RID: 18372
		public GameObject favorite;

		// Token: 0x040047C5 RID: 18373
		public GameObject random;
	}

	// Token: 0x02000B7E RID: 2942
	public class GuiProfile
	{
		// Token: 0x06004313 RID: 17171 RVA: 0x00201E50 File Offset: 0x00200050
		public GuiProfile(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_Name = baseTr.Find("Btn_Name").GetComponent<PguiButtonCtrl>();
			this.Btn_Comment = baseTr.Find("Btn_Comment").GetComponent<PguiButtonCtrl>();
			this.Btn_Id = baseTr.Find("Btn_Id").GetComponent<PguiButtonCtrl>();
			this.Img_Photo = baseTr.Find("Btn_Gender/BaseImage/Img_Photo").GetComponent<PguiImageCtrl>();
			this.Relpace_Photo = baseTr.Find("Btn_Gender/BaseImage/Img_Photo").GetComponent<PguiReplaceSpriteCtrl>();
			this.Paper01 = baseTr.Find("Paper01").GetComponent<PguiAECtrl>();
			this.Txt_UserName = baseTr.Find("Txt_UserName").GetComponent<PguiTextCtrl>();
			this.Txt_Comment = baseTr.Find("Txt_Comment").GetComponent<PguiTextCtrl>();
			this.Txt_Id = baseTr.Find("Txt_Id").GetComponent<PguiTextCtrl>();
			this.Btn_Gender = baseTr.Find("Btn_Gender").GetComponent<PguiButtonCtrl>();
			this.TotalKemoStatusText = baseTr.Find("Txt_TotalStatus").GetComponent<PguiTextCtrl>();
			this.Achievement = baseTr.Find("Achievement").GetComponent<AchievementCtrl>();
			this.Btn_Achievement = baseTr.Find("Btn_Achievement").GetComponent<PguiButtonCtrl>();
			this.Btn_KizunaBuff = baseTr.Find("Btn_Kizuna").GetComponent<PguiButtonCtrl>();
		}

		// Token: 0x040047C6 RID: 18374
		public GameObject baseObj;

		// Token: 0x040047C7 RID: 18375
		public PguiButtonCtrl Btn_Name;

		// Token: 0x040047C8 RID: 18376
		public PguiButtonCtrl Btn_Comment;

		// Token: 0x040047C9 RID: 18377
		public PguiButtonCtrl Btn_Id;

		// Token: 0x040047CA RID: 18378
		public PguiImageCtrl Img_Photo;

		// Token: 0x040047CB RID: 18379
		public PguiReplaceSpriteCtrl Relpace_Photo;

		// Token: 0x040047CC RID: 18380
		public PguiAECtrl Paper01;

		// Token: 0x040047CD RID: 18381
		public PguiTextCtrl Txt_UserName;

		// Token: 0x040047CE RID: 18382
		public PguiTextCtrl Txt_Comment;

		// Token: 0x040047CF RID: 18383
		public PguiTextCtrl Txt_Id;

		// Token: 0x040047D0 RID: 18384
		public PguiButtonCtrl Btn_Gender;

		// Token: 0x040047D1 RID: 18385
		public PguiTextCtrl TotalKemoStatusText;

		// Token: 0x040047D2 RID: 18386
		public AchievementCtrl Achievement;

		// Token: 0x040047D3 RID: 18387
		public PguiButtonCtrl Btn_Achievement;

		// Token: 0x040047D4 RID: 18388
		public PguiButtonCtrl Btn_KizunaBuff;
	}

	// Token: 0x02000B7F RID: 2943
	public class GuiHelper
	{
		// Token: 0x06004314 RID: 17172 RVA: 0x00201FA4 File Offset: 0x002001A4
		public GuiHelper(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_Helper = baseTr.Find("Btn_Helper").GetComponent<PguiButtonCtrl>();
			for (int i = 0; i < 7; i++)
			{
				this.iconChara.Add(Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Chara, baseTr.Find("Chara_All/Icon_0" + (i + 1).ToString() + "/Icon_Chara_01")).GetComponent<IconCharaCtrl>());
			}
		}

		// Token: 0x040047D5 RID: 18389
		public GameObject baseObj;

		// Token: 0x040047D6 RID: 18390
		public PguiButtonCtrl Btn_Helper;

		// Token: 0x040047D7 RID: 18391
		public List<IconCharaCtrl> iconChara = new List<IconCharaCtrl>();
	}

	// Token: 0x02000B80 RID: 2944
	public class GuiFavorite
	{
		// Token: 0x06004315 RID: 17173 RVA: 0x00202030 File Offset: 0x00200230
		public GuiFavorite(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_FilterOnOff = baseTr.Find("All/WindowAll/SortFilterBtnsAll/Btn_FilterOnOff").GetComponent<PguiButtonCtrl>();
			this.Btn_Sort = baseTr.Find("All/WindowAll/SortFilterBtnsAll/Btn_Sort").GetComponent<PguiButtonCtrl>();
			this.Btn_SortUpDown = baseTr.Find("All/WindowAll/SortFilterBtnsAll/Btn_SortUpDown").GetComponent<PguiButtonCtrl>();
			this.ScrollView = baseTr.Find("All/WindowAll/ScrollView").GetComponent<ReuseScroll>();
			this.Txt_None = baseTr.Find("All/WindowAll/Txt_None").gameObject;
			this.Txt_None.SetActive(false);
		}

		// Token: 0x06004316 RID: 17174 RVA: 0x002020C9 File Offset: 0x002002C9
		public void ResizeScrollView(int count, int resize)
		{
			this.Txt_None.SetActive(count <= 0);
			this.ScrollView.Resize(resize, 0);
		}

		// Token: 0x040047D8 RID: 18392
		public GameObject baseObj;

		// Token: 0x040047D9 RID: 18393
		public PguiButtonCtrl Btn_FilterOnOff;

		// Token: 0x040047DA RID: 18394
		public PguiButtonCtrl Btn_Sort;

		// Token: 0x040047DB RID: 18395
		public PguiButtonCtrl Btn_SortUpDown;

		// Token: 0x040047DC RID: 18396
		public ReuseScroll ScrollView;

		// Token: 0x040047DD RID: 18397
		public GameObject Txt_None;
	}

	// Token: 0x02000B81 RID: 2945
	public class GUI
	{
		// Token: 0x06004317 RID: 17175 RVA: 0x002020EC File Offset: 0x002002EC
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.guiBtnFavorite = new SelProfileCtrl.GuiBtnFavorite(baseTr.Find("ProfileAll/All/Btn_Favorite").transform);
			this.guiBtnLoginBonus = new SelProfileCtrl.GuiBtnLoginBonus(baseTr.Find("ProfileAll/All/Btn_LoginBonusChara").transform);
			this.guiProfile = new SelProfileCtrl.GuiProfile(baseTr.Find("ProfileAll/All/Profile").transform);
			this.guiHelper = new SelProfileCtrl.GuiHelper(baseTr.Find("ProfileAll/All/Helper").transform);
			this.guiFavorite = new SelProfileCtrl.GuiFavorite(baseTr.Find("Favorite").transform);
			this.guiHelperChange = new SelProfileCtrl.GuiHelperChange(baseTr.Find("HelperChange/All").transform, 7);
			this.profileAnimation = baseTr.Find("ProfileAll").GetComponent<SimpleAnimation>();
			this.favoriteAnimation = baseTr.Find("Favorite").GetComponent<SimpleAnimation>();
			this.helperAnimation = baseTr.Find("HelperChange").GetComponent<SimpleAnimation>();
		}

		// Token: 0x040047DE RID: 18398
		public GameObject baseObj;

		// Token: 0x040047DF RID: 18399
		public SelProfileCtrl.GuiBtnFavorite guiBtnFavorite;

		// Token: 0x040047E0 RID: 18400
		public SelProfileCtrl.GuiBtnLoginBonus guiBtnLoginBonus;

		// Token: 0x040047E1 RID: 18401
		public SelProfileCtrl.GuiProfile guiProfile;

		// Token: 0x040047E2 RID: 18402
		public SelProfileCtrl.GuiHelper guiHelper;

		// Token: 0x040047E3 RID: 18403
		public SelProfileCtrl.GuiFavorite guiFavorite;

		// Token: 0x040047E4 RID: 18404
		public SelProfileCtrl.GuiHelperChange guiHelperChange;

		// Token: 0x040047E5 RID: 18405
		public SimpleAnimation profileAnimation;

		// Token: 0x040047E6 RID: 18406
		public SimpleAnimation favoriteAnimation;

		// Token: 0x040047E7 RID: 18407
		public SimpleAnimation helperAnimation;
	}

	// Token: 0x02000B82 RID: 2946
	public class WindowCommentChange
	{
		// Token: 0x06004318 RID: 17176 RVA: 0x002021F0 File Offset: 0x002003F0
		public WindowCommentChange(Transform baseTr)
		{
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.InputField = baseTr.Find("Base/Window/InputField").GetComponent<InputField>();
			this.Txt_ErrorMassage = baseTr.Find("Base/Window/Massage_03").GetComponent<PguiTextCtrl>();
			this.Txt_ErrorMassage.gameObject.SetActive(false);
		}

		// Token: 0x040047E8 RID: 18408
		public PguiOpenWindowCtrl owCtrl;

		// Token: 0x040047E9 RID: 18409
		public PguiTextCtrl Txt_ErrorMassage;

		// Token: 0x040047EA RID: 18410
		public InputField InputField;
	}

	// Token: 0x02000B83 RID: 2947
	public class WindowNameChange
	{
		// Token: 0x06004319 RID: 17177 RVA: 0x0020224C File Offset: 0x0020044C
		public WindowNameChange(Transform baseTr)
		{
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.InputField = baseTr.Find("Base/Window/InputField").GetComponent<InputField>();
			this.Txt_ErrorMassage = baseTr.Find("Base/Window/Massage_03").GetComponent<PguiTextCtrl>();
			this.Txt_ErrorMassage.gameObject.SetActive(false);
		}

		// Token: 0x040047EB RID: 18411
		public PguiOpenWindowCtrl owCtrl;

		// Token: 0x040047EC RID: 18412
		public PguiTextCtrl Txt_ErrorMassage;

		// Token: 0x040047ED RID: 18413
		public InputField InputField;
	}

	// Token: 0x02000B84 RID: 2948
	public enum HelperMode
	{
		// Token: 0x040047EF RID: 18415
		INVALID,
		// Token: 0x040047F0 RID: 18416
		DECK_TOP,
		// Token: 0x040047F1 RID: 18417
		DECK_EDIT,
		// Token: 0x040047F2 RID: 18418
		PHOTO_TOP,
		// Token: 0x040047F3 RID: 18419
		PHOTO_EDIT,
		// Token: 0x040047F4 RID: 18420
		ACCESSORY_TOP
	}

	// Token: 0x02000B85 RID: 2949
	public class SelectCharaData
	{
		// Token: 0x0600431A RID: 17178 RVA: 0x002022A8 File Offset: 0x002004A8
		public SelectCharaData(SelCharaDeckCtrl.FrameType t, SelCharaDeckCtrl.GUI.IconChara c)
		{
			this.type = t;
			this.chara = c;
		}

		// Token: 0x040047F5 RID: 18421
		public SelCharaDeckCtrl.FrameType type;

		// Token: 0x040047F6 RID: 18422
		public SelCharaDeckCtrl.GUI.IconChara chara;
	}

	// Token: 0x02000B86 RID: 2950
	public class GuiHelperChange
	{
		// Token: 0x0600431B RID: 17179 RVA: 0x002022C0 File Offset: 0x002004C0
		public GuiHelperChange(Transform baseTr, int num)
		{
			this.baseObj = baseTr.gameObject;
			this.charaDeck = new SelCharaDeckCtrl.GUI.CharaDeck(baseTr.Find("DeckSelect"), num);
		}

		// Token: 0x040047F7 RID: 18423
		public GameObject baseObj;

		// Token: 0x040047F8 RID: 18424
		public SelCharaDeckCtrl.GUI.CharaDeck charaDeck;

		// Token: 0x040047F9 RID: 18425
		public List<SelCharaDeckCtrl.GUI.IconChara> reserveCharaIcon = new List<SelCharaDeckCtrl.GUI.IconChara>();

		// Token: 0x040047FA RID: 18426
		public List<SelPhotoEditCtrl.GUI.IconPhotoSet> reservePhotoIcon = new List<SelPhotoEditCtrl.GUI.IconPhotoSet>();
	}
}
