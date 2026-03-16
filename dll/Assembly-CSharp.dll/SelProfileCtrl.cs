using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelProfileCtrl : MonoBehaviour
{
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

	private SelProfileCtrl.Mode preMode
	{
		get
		{
			return this._preMode;
		}
	}

	public bool GetStart
	{
		get
		{
			return this.getStart;
		}
	}

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

	public bool IsProcessing()
	{
		return this.currentEnumerator != null;
	}

	private void Start()
	{
	}

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

	public void RequestUpdateAvater()
	{
		DataManager.DmUserInfo.RequestActionUpdateUserAvatar(this.avaterType + DataManagerUserInfo.AvatarType.TYPE_A);
		this.renderTextureChara.Destroy();
	}

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

	private IEnumerator RequestActionUpdateLoanPackList()
	{
		DataManager.DmUserInfo.RequestActionUpdateLoanPackList(this.disideLoanPackList);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		yield break;
	}

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

	private void DispLoginBonus()
	{
		this.guiData.guiBtnLoginBonus.favorite.SetActive(!DataManager.DmUserInfo.optionData.LoginBonusFriends);
		this.guiData.guiBtnLoginBonus.random.SetActive(DataManager.DmUserInfo.optionData.LoginBonusFriends);
	}

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

	private void OnClickCharaButton(IconCharaCtrl icc)
	{
		if (icc.charaPackData != null && this.currentFavoriteCharaId != icc.charaPackData.id)
		{
			this.currentEnumerator = this.RequestUpdateFavoriteChara(icc.charaPackData.id);
		}
	}

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

	private bool OnClickButtionChangeAchievement()
	{
		Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneAchievement, null);
		return true;
	}

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

	private IEnumerator RequestEndAnimation()
	{
		this.guiData.profileAnimation.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
		{
			CanvasManager.HdlCmnMenu.MoveSceneByMenu(SceneManager.SceneName.SceneOtherMenuTop, null);
		});
		yield return null;
		yield break;
	}

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

	private bool IsChangeHelpers()
	{
		return this.isChangeHelpers || (this.selPhotoEditCtrl != null && this.selPhotoEditCtrl.isChangeClone);
	}

	private void SelectPhoto()
	{
		this.OnClickPhotoButton(this.guiData.guiHelperChange.charaDeck.Btn_Photo, 0);
		this.guiData.guiHelperChange.charaDeck.Btn_Photo.SetToggleIndex(1);
	}

	private void SelectAccessory()
	{
		this.OnClickAccessoryButton(this.guiData.guiHelperChange.charaDeck.Btn_Accessory, 0);
		this.guiData.guiHelperChange.charaDeck.Btn_Accessory.SetToggleIndex(1);
	}

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

	private void RefreshReserveFrame(SelCharaDeckCtrl.GUI.IconChara iconChara)
	{
		this.currentLoanAttribute = (CharaDef.AttributeType)this.GetIconCharaIndex(iconChara);
		this.dispLoanCharaList = this.DispLoanCharaList;
		CanvasManager.HdlOpenWindowSortFilter.SolutionList(SortFilterDefine.RegisterType.CHARA_HELPER_LOAN, null);
	}

	private int GetIconCharaIndex(SelCharaDeckCtrl.GUI.IconChara iconChara)
	{
		return this.guiData.guiHelperChange.charaDeck.iconCharaPacks.FindIndex((SelCharaDeckCtrl.GUI.IconCharaPack item) => item.iconChara == iconChara);
	}

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

	private void ResetCurrentIcon()
	{
		if (this.selectCharaData != null)
		{
			this.selectCharaData.chara.iconCharaSet.currentFrame.SetActive(false);
			this.selectCharaData = null;
		}
	}

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

	private void RequestAccessoryDeck()
	{
		this.ResetCurrentIcon();
		this.RefreshDeckFrame();
	}

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

	public bool isDebug;

	private SelProfileCtrl.GUI guiData;

	private SelPhotoEditCtrl selPhotoEditCtrl;

	private SortFilterDefine.SortType sortType = SortFilterDefine.SortType.LEVEL;

	private SelProfileCtrl.WindowCommentChange windowCommentChange;

	private SelProfileCtrl.WindowNameChange windowNameChange;

	private SelProfileCtrl.Mode requestMode;

	private SelProfileCtrl.Mode _currentMode;

	private SelProfileCtrl.Mode _preMode;

	private bool getStart;

	private SelProfileCtrl.HelperMode currentHelperMode;

	private IEnumerator currentEnumerator;

	private IEnumerator requestActionUpdateLoanPackList;

	private PguiRenderTextureCharaCtrl renderTextureChara;

	private List<CharaPackData> dispFavoriteCharaList;

	private int currentFavoriteCharaId;

	private int requestFavoriteCharaId;

	private int avaterType;

	private bool isHelperSettingStartFromCharaEdit;

	private bool isFromBattleSelecter;

	public UnityAction<bool> setActiveQuestMapDataCB;

	private bool isSetup;

	private SceneManager.SceneName OnClickMoveSequenceName;

	private object OnClickMoveSequenceArgs;

	private bool isChangeHelpers;

	private List<LoanPackData> cloneLoanPackList = new List<LoanPackData>();

	private List<LoanPackData> disideLoanPackList = new List<LoanPackData>();

	private List<CharaPackData> dispLoanCharaList = new List<CharaPackData>();

	private List<PhotoPackData> dispLoanPhotoList = new List<PhotoPackData>();

	private CharaDef.AttributeType currentLoanAttribute;

	private SelProfileCtrl.SelectCharaData selectCharaData;

	private CharaPackData removeButtonCharaData = CharaPackData.MakeInvalid();

	private PhotoPackData removeButtonPhotoData = PhotoPackData.MakeInvalid();

	private int currentHelperDeckIndexByPhotoEdit;

	private SortFilterDefine.SortType sortTypeLoan = SortFilterDefine.SortType.LEVEL;

	public enum Mode
	{
		INVALID,
		TOP,
		FAVORITE_CHANGE,
		NAME_CHANGE,
		COMMENT_CHANGE,
		HELPER_CHANGE
	}

	public class GuiBtnFavorite
	{
		public GuiBtnFavorite(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_Favorite = baseTr.GetComponent<PguiButtonCtrl>();
		}

		public GameObject baseObj;

		public PguiButtonCtrl Btn_Favorite;
	}

	public class GuiBtnLoginBonus
	{
		public GuiBtnLoginBonus(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_LoginBonus = baseTr.GetComponent<PguiButtonCtrl>();
			this.favorite = baseTr.Find("BaseImage/Chara_Favorite").gameObject;
			this.random = baseTr.Find("BaseImage/Chara_Random").gameObject;
		}

		public GameObject baseObj;

		public PguiButtonCtrl Btn_LoginBonus;

		public GameObject favorite;

		public GameObject random;
	}

	public class GuiProfile
	{
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

		public GameObject baseObj;

		public PguiButtonCtrl Btn_Name;

		public PguiButtonCtrl Btn_Comment;

		public PguiButtonCtrl Btn_Id;

		public PguiImageCtrl Img_Photo;

		public PguiReplaceSpriteCtrl Relpace_Photo;

		public PguiAECtrl Paper01;

		public PguiTextCtrl Txt_UserName;

		public PguiTextCtrl Txt_Comment;

		public PguiTextCtrl Txt_Id;

		public PguiButtonCtrl Btn_Gender;

		public PguiTextCtrl TotalKemoStatusText;

		public AchievementCtrl Achievement;

		public PguiButtonCtrl Btn_Achievement;

		public PguiButtonCtrl Btn_KizunaBuff;
	}

	public class GuiHelper
	{
		public GuiHelper(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_Helper = baseTr.Find("Btn_Helper").GetComponent<PguiButtonCtrl>();
			for (int i = 0; i < 7; i++)
			{
				this.iconChara.Add(Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Chara, baseTr.Find("Chara_All/Icon_0" + (i + 1).ToString() + "/Icon_Chara_01")).GetComponent<IconCharaCtrl>());
			}
		}

		public GameObject baseObj;

		public PguiButtonCtrl Btn_Helper;

		public List<IconCharaCtrl> iconChara = new List<IconCharaCtrl>();
	}

	public class GuiFavorite
	{
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

		public void ResizeScrollView(int count, int resize)
		{
			this.Txt_None.SetActive(count <= 0);
			this.ScrollView.Resize(resize, 0);
		}

		public GameObject baseObj;

		public PguiButtonCtrl Btn_FilterOnOff;

		public PguiButtonCtrl Btn_Sort;

		public PguiButtonCtrl Btn_SortUpDown;

		public ReuseScroll ScrollView;

		public GameObject Txt_None;
	}

	public class GUI
	{
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

		public GameObject baseObj;

		public SelProfileCtrl.GuiBtnFavorite guiBtnFavorite;

		public SelProfileCtrl.GuiBtnLoginBonus guiBtnLoginBonus;

		public SelProfileCtrl.GuiProfile guiProfile;

		public SelProfileCtrl.GuiHelper guiHelper;

		public SelProfileCtrl.GuiFavorite guiFavorite;

		public SelProfileCtrl.GuiHelperChange guiHelperChange;

		public SimpleAnimation profileAnimation;

		public SimpleAnimation favoriteAnimation;

		public SimpleAnimation helperAnimation;
	}

	public class WindowCommentChange
	{
		public WindowCommentChange(Transform baseTr)
		{
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.InputField = baseTr.Find("Base/Window/InputField").GetComponent<InputField>();
			this.Txt_ErrorMassage = baseTr.Find("Base/Window/Massage_03").GetComponent<PguiTextCtrl>();
			this.Txt_ErrorMassage.gameObject.SetActive(false);
		}

		public PguiOpenWindowCtrl owCtrl;

		public PguiTextCtrl Txt_ErrorMassage;

		public InputField InputField;
	}

	public class WindowNameChange
	{
		public WindowNameChange(Transform baseTr)
		{
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.InputField = baseTr.Find("Base/Window/InputField").GetComponent<InputField>();
			this.Txt_ErrorMassage = baseTr.Find("Base/Window/Massage_03").GetComponent<PguiTextCtrl>();
			this.Txt_ErrorMassage.gameObject.SetActive(false);
		}

		public PguiOpenWindowCtrl owCtrl;

		public PguiTextCtrl Txt_ErrorMassage;

		public InputField InputField;
	}

	public enum HelperMode
	{
		INVALID,
		DECK_TOP,
		DECK_EDIT,
		PHOTO_TOP,
		PHOTO_EDIT,
		ACCESSORY_TOP
	}

	public class SelectCharaData
	{
		public SelectCharaData(SelCharaDeckCtrl.FrameType t, SelCharaDeckCtrl.GUI.IconChara c)
		{
			this.type = t;
			this.chara = c;
		}

		public SelCharaDeckCtrl.FrameType type;

		public SelCharaDeckCtrl.GUI.IconChara chara;
	}

	public class GuiHelperChange
	{
		public GuiHelperChange(Transform baseTr, int num)
		{
			this.baseObj = baseTr.gameObject;
			this.charaDeck = new SelCharaDeckCtrl.GUI.CharaDeck(baseTr.Find("DeckSelect"), num);
		}

		public GameObject baseObj;

		public SelCharaDeckCtrl.GUI.CharaDeck charaDeck;

		public List<SelCharaDeckCtrl.GUI.IconChara> reserveCharaIcon = new List<SelCharaDeckCtrl.GUI.IconChara>();

		public List<SelPhotoEditCtrl.GUI.IconPhotoSet> reservePhotoIcon = new List<SelPhotoEditCtrl.GUI.IconPhotoSet>();
	}
}
