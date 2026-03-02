using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020001BD RID: 445
public class SelPhotoEditCtrl : MonoBehaviour
{
	// Token: 0x17000427 RID: 1063
	// (get) Token: 0x06001EA0 RID: 7840 RVA: 0x0017BA54 File Offset: 0x00179C54
	// (set) Token: 0x06001EA1 RID: 7841 RVA: 0x0017BA5C File Offset: 0x00179C5C
	public bool isChangeClone { get; set; }

	// Token: 0x17000428 RID: 1064
	// (get) Token: 0x06001EA2 RID: 7842 RVA: 0x0017BA65 File Offset: 0x00179C65
	// (set) Token: 0x06001EA3 RID: 7843 RVA: 0x0017BA74 File Offset: 0x00179C74
	public bool TouchRect
	{
		get
		{
			bool flag = this.touchRect;
			this.touchRect = false;
			return flag;
		}
		private set
		{
			this.touchRect = value;
		}
	}

	// Token: 0x06001EA4 RID: 7844 RVA: 0x0017BA80 File Offset: 0x00179C80
	public void ChangePhotoInfo(int selPhoto)
	{
		SelPhotoEditCtrl.<>c__DisplayClass23_0 CS$<>8__locals1 = new SelPhotoEditCtrl.<>c__DisplayClass23_0();
		CS$<>8__locals1.<>4__this = this;
		CharaPackData charaPackData = ((this.setupParam.cbGetCharaPackData != null) ? this.setupParam.cbGetCharaPackData() : null);
		if (charaPackData == null)
		{
			charaPackData = CharaPackData.MakeInvalid();
		}
		this.guiData.photoDeck.Txt_Party.text = ((this.setupParam.cbSetPartyName != null) ? this.setupParam.cbSetPartyName() : "パーティー名をセットし忘れないように");
		IconCharaCtrl mainIconCharaCtrl = this.guiData.photoDeck.mainIconCharaCtrl;
		CharaPackData charaPackData2 = charaPackData;
		SortFilterDefine.SortType sortType = SortFilterDefine.SortType.LEVEL;
		bool flag = false;
		CharaWindowCtrl.DetailParamSetting detailParamSetting2;
		if (!this.setupParam.isTutorial)
		{
			CharaWindowCtrl.DetailParamSetting detailParamSetting = new CharaWindowCtrl.DetailParamSetting(CharaWindowCtrl.DetailParamSetting.Preset.MINE_EASY, null);
			detailParamSetting.pvpSeasonId = this.setupParam.pvpSeasonId;
			detailParamSetting.selectEventId = this.setupParam.EventId;
			detailParamSetting.selectQuestOneId = this.setupParam.PlayQuestOneId;
			detailParamSetting2 = detailParamSetting;
			detailParamSetting.deckCategory = this.setupParam.deckCategory;
		}
		else
		{
			detailParamSetting2 = null;
		}
		mainIconCharaCtrl.Setup(charaPackData2, sortType, flag, detailParamSetting2, this.setupParam.EventId, -1, this.setupParam.PlayQuestOneId);
		CS$<>8__locals1.equipPhotoList = ((this.setupParam.cbGetEquipPhoto != null) ? this.setupParam.cbGetEquipPhoto() : new List<long>());
		CS$<>8__locals1.equipPhotoPackList = this.havePhotoPackList.FindAll((PhotoPackData item) => item != CS$<>8__locals1.<>4__this.removeButttonPhotoData && CS$<>8__locals1.equipPhotoList.Exists((long item2) => item2 == item.dataId));
		int j;
		int i;
		for (j = 0; j < CS$<>8__locals1.equipPhotoList.Count; j = i + 1)
		{
			PhotoPackData photoPackData = this.havePhotoPackList.Find((PhotoPackData item) => item != CS$<>8__locals1.<>4__this.removeButttonPhotoData && item.dataId == CS$<>8__locals1.equipPhotoList[j]);
			if (photoPackData == null)
			{
				photoPackData = PhotoPackData.MakeInvalid();
			}
			this.guiData.photoDeck.mainIconPhotoCtrl[j].iconPhotoSet.iconPhotoCtrl.Setup(photoPackData, SortFilterDefine.SortType.LEVEL, !this.setupParam.isTutorial, false, this.setupParam.PlayQuestOneId, this.setupParam.isHelper);
			if (DataManagerDeck.CheckDisableDropIcon(this.setupParam.deckCategory, this.setupParam.pvpSeasonId))
			{
				this.guiData.photoDeck.mainIconPhotoCtrl[j].iconPhotoSet.iconPhotoCtrl.DispDrop(false, 0);
			}
			IconPhotoCtrl iconPhotoCtrl = this.guiData.photoDeck.mainIconPhotoCtrl[j].iconPhotoSet.iconPhotoCtrl;
			IconPhotoCtrl.OnUpdateLockFlag onUpdateLockFlag;
			if ((onUpdateLockFlag = CS$<>8__locals1.<>9__3) == null)
			{
				onUpdateLockFlag = (CS$<>8__locals1.<>9__3 = delegate(IconPhotoCtrl item)
				{
					CS$<>8__locals1.<>4__this.guiData.photoDeck.ScrollView.Refresh();
				});
			}
			iconPhotoCtrl.AddOnUpdateLockListener(onUpdateLockFlag);
			IconPhotoCtrl iconPhotoCtrl2 = this.guiData.photoDeck.mainIconPhotoCtrl[j].iconPhotoSet.iconPhotoCtrl;
			IconPhotoCtrl.OnUpdateFavoriteFlag onUpdateFavoriteFlag;
			if ((onUpdateFavoriteFlag = CS$<>8__locals1.<>9__4) == null)
			{
				onUpdateFavoriteFlag = (CS$<>8__locals1.<>9__4 = delegate(IconPhotoCtrl item)
				{
					CS$<>8__locals1.<>4__this.guiData.photoDeck.ScrollView.Refresh();
				});
			}
			iconPhotoCtrl2.AddOnUpdateFavoriteListener(onUpdateFavoriteFlag);
			IconPhotoCtrl iconPhotoCtrl3 = this.guiData.photoDeck.mainIconPhotoCtrl[j].iconPhotoSet.iconPhotoCtrl;
			IconPhotoCtrl.OnUpdateLockFlag onUpdateLockFlag2;
			if ((onUpdateLockFlag2 = CS$<>8__locals1.<>9__5) == null)
			{
				onUpdateLockFlag2 = (CS$<>8__locals1.<>9__5 = delegate(IconPhotoCtrl item)
				{
					CS$<>8__locals1.<>4__this.guiData.photoDeck.ScrollView.Refresh();
					List<SelPhotoEditCtrl.GUI.PhotoEdit> mainIconPhotoCtrl = CS$<>8__locals1.<>4__this.guiData.photoDeck.mainIconPhotoCtrl;
					Predicate<SelPhotoEditCtrl.GUI.PhotoEdit> predicate;
					if ((predicate = CS$<>8__locals1.<>9__8) == null)
					{
						predicate = (CS$<>8__locals1.<>9__8 = (SelPhotoEditCtrl.GUI.PhotoEdit item) => item.iconPhotoSet.iconPhotoCtrl.photoPackData == CS$<>8__locals1.<>4__this.selectPhotoData.photo);
					}
					if (mainIconPhotoCtrl.Find(predicate) != null)
					{
						CS$<>8__locals1.<>4__this.guiData.photoDeck.infoIconPhotoCtrl.Setup(CS$<>8__locals1.<>4__this.selectPhotoData.photo, SortFilterDefine.SortType.LEVEL, true, false, -1, false);
					}
				});
			}
			iconPhotoCtrl3.AddOnUpdateStatus(onUpdateLockFlag2);
			IconPhotoCtrl iconPhotoCtrl4 = this.guiData.photoDeck.mainIconPhotoCtrl[j].iconPhotoSet.iconPhotoCtrl;
			IconPhotoCtrl.OnUpdateLockFlag onUpdateLockFlag3;
			if ((onUpdateLockFlag3 = CS$<>8__locals1.<>9__6) == null)
			{
				onUpdateLockFlag3 = (CS$<>8__locals1.<>9__6 = delegate(IconPhotoCtrl item)
				{
					UserDeckData.Category deckCategory = CS$<>8__locals1.<>4__this.setupParam.deckCategory;
					SortFilterDefine.RegisterType registerType;
					if (deckCategory != UserDeckData.Category.PVP)
					{
						if (deckCategory != UserDeckData.Category.TRAINING)
						{
							registerType = SortFilterDefine.RegisterType.PHOTO_DECK;
						}
						else
						{
							registerType = SortFilterDefine.RegisterType.PHOTO_DECK_TRAINING;
						}
					}
					else
					{
						registerType = SortFilterDefine.RegisterType.PHOTO_DECK_PVP;
					}
					SortFilterDefine.RegisterType registerType2 = registerType;
					CanvasManager.HdlOpenWindowSortFilter.SolutionList(registerType2, null);
				});
			}
			iconPhotoCtrl4.AddOnCloseWindow(onUpdateLockFlag3);
			IconPhotoCtrl iconPhotoCtrl5 = this.guiData.photoDeck.mainIconPhotoCtrl[j].iconPhotoSet.iconPhotoCtrl;
			IconPhotoCtrl.OnReturnPhotoPackDataList onReturnPhotoPackDataList;
			if ((onReturnPhotoPackDataList = CS$<>8__locals1.<>9__7) == null)
			{
				onReturnPhotoPackDataList = (CS$<>8__locals1.<>9__7 = () => CS$<>8__locals1.equipPhotoPackList);
			}
			iconPhotoCtrl5.onReturnPhotoPackDataList = onReturnPhotoPackDataList;
			if (charaPackData != null && !charaPackData.IsInvalid() && charaPackData.dynamicData.PhotoPocket[j].Flag)
			{
				this.guiData.photoDeck.mainIconPhotoCtrl[j].BaseBlank_Lock.SetActive(false);
				this.guiData.photoDeck.mainIconPhotoCtrl[j].BaseBlank_Plus.SetActive(true);
				this.guiData.photoDeck.mainIconPhotoCtrl[j].iconPhotoSet.iconPhotoCtrl.DispImgDisable(false);
				this.guiData.photoDeck.mainIconPhotoCtrl[j].iconPhotoSet.iconPhotoCtrl.DispTextDisable(false, null, null);
				this.guiData.photoDeck.mainIconPhotoCtrl[j].Num_Lv.gameObject.SetActive(charaPackData.dynamicData.PhotoPocket[j].Step > 0);
				this.guiData.photoDeck.mainIconPhotoCtrl[j].Num_Lv.text = string.Format("{0}", charaPackData.dynamicData.PhotoPocket[j].Step);
			}
			else
			{
				this.guiData.photoDeck.mainIconPhotoCtrl[j].BaseBlank_Lock.SetActive(true);
				this.guiData.photoDeck.mainIconPhotoCtrl[j].BaseBlank_Plus.SetActive(false);
				this.guiData.photoDeck.mainIconPhotoCtrl[j].Num_Lv.gameObject.SetActive(false);
				if (!photoPackData.IsInvalid())
				{
					this.guiData.photoDeck.mainIconPhotoCtrl[j].iconPhotoSet.iconPhotoCtrl.DispImgDisable(true);
					this.guiData.photoDeck.mainIconPhotoCtrl[j].iconPhotoSet.iconPhotoCtrl.DispMarkNotYetReleased(true);
				}
			}
			if (!photoPackData.IsInvalid() && photoPackData.staticData.baseData.kizunaPhotoFlg)
			{
				this.guiData.photoDeck.mainIconPhotoCtrl[j].iconPhotoSet.iconPhotoCtrl.DispImgDisable(charaPackData == null || charaPackData.IsInvalid() || charaPackData.staticData.baseData.kizunaPhotoId != photoPackData.staticData.GetId() || !charaPackData.dynamicData.PhotoPocket[j].Flag);
				this.guiData.photoDeck.mainIconPhotoCtrl[j].iconPhotoSet.iconPhotoCtrl.DispTextDisable(charaPackData == null || charaPackData.IsInvalid() || charaPackData.staticData.baseData.kizunaPhotoId != photoPackData.staticData.GetId(), PhotoUtil.FriendsText, PhotoUtil.NoFormationText);
			}
			this.guiData.photoDeck.mainIconPhotoCtrl[j].Active.gameObject.SetActive(false);
			if (!this.guiData.photoDeck.mainIconPhotoCtrl[j].iconPhotoSet.iconPhotoCtrl.CheckImgDisable())
			{
				this.guiData.photoDeck.mainIconPhotoCtrl[j].iconPhotoSet.iconPhotoCtrl.DispTextDisable(false, null, null);
			}
			i = j;
		}
		if (selPhoto >= 0 && this.guiData.photoDeck.mainIconPhotoCtrl.Count > selPhoto)
		{
			this.OnTouchPhotoIcon(SelCharaDeckCtrl.FrameType.DECK, this.guiData.photoDeck.mainIconPhotoCtrl[selPhoto].iconPhotoSet);
		}
	}

	// Token: 0x06001EA5 RID: 7845 RVA: 0x0017C290 File Offset: 0x0017A490
	public void Init(SelPhotoEditCtrl.Type type)
	{
		if (this.isInit)
		{
			return;
		}
		GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("SelCmn/GUI/Prefab/GUI_CharaDeck_PhotoEdit"), base.transform);
		this.cloneUserOptionData = DataManager.DmUserInfo.optionData.Clone();
		this.guiData = new SelPhotoEditCtrl.GUI(gameObject.transform);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.FRONT, base.transform, true);
		this.guiData.photoDeck.sizeChangeBtnGUI.Setup(new PhotoUtil.SizeChangeBtnGUI.SetupParam
		{
			funcResult = delegate(PhotoUtil.SizeChangeBtnGUI.ResultParam result)
			{
				if (type == SelPhotoEditCtrl.Type.Asistant)
				{
					this.cloneUserOptionData.photoIconSizeAsistant = result.sizeIndex;
				}
				else
				{
					this.cloneUserOptionData.photoIconSizePartyEdit = result.sizeIndex;
				}
				DataManager.DmUserInfo.RequestActionUpdateUserOption(this.cloneUserOptionData);
			},
			iconPhotoParamList = new List<PhotoUtil.SizeChangeBtnGUI.IconPhotoParam>
			{
				new PhotoUtil.SizeChangeBtnGUI.IconPhotoParam
				{
					scale = new Vector3(0.52f, 0.52f, 1f),
					scaleCurrent = new Vector3(0.85f, 0.85f, 1f),
					scaleCount = new Vector3(0.85f, 0.85f, 1f),
					num = 7,
					prefab = Object.Instantiate<GameObject>((GameObject)Resources.Load("SelCmn/GUI/Prefab/CharaDeck_Icon_Photo_List_S"), base.transform)
				},
				new PhotoUtil.SizeChangeBtnGUI.IconPhotoParam
				{
					scale = new Vector3(0.6f, 0.6f, 1f),
					scaleCurrent = new Vector3(1f, 1f, 1f),
					scaleCount = new Vector3(1f, 1f, 1f),
					num = 6,
					prefab = Object.Instantiate<GameObject>((GameObject)Resources.Load("SelCmn/GUI/Prefab/CharaDeck_Icon_Photo_List_M"), base.transform)
				},
				new PhotoUtil.SizeChangeBtnGUI.IconPhotoParam
				{
					scale = new Vector3(0.7f, 0.7f, 1f),
					scaleCurrent = new Vector3(1f, 1f, 1f),
					scaleCount = new Vector3(1f, 1f, 1f),
					num = 5,
					prefab = Object.Instantiate<GameObject>((GameObject)Resources.Load("SelCmn/GUI/Prefab/CharaDeck_Icon_Photo_List_L"), base.transform)
				},
				new PhotoUtil.SizeChangeBtnGUI.IconPhotoParam
				{
					scale = new Vector3(1f, 1f, 1f),
					scaleCurrent = new Vector3(1f, 1f, 1f),
					scaleCount = new Vector3(1f, 1f, 1f),
					num = 4,
					prefab = Object.Instantiate<GameObject>((GameObject)Resources.Load("SelCmn/GUI/Prefab/CharaDeck_Icon_Photo_List_XL"), base.transform)
				}
			},
			onStartItem = new Action<int, GameObject>(this.OnStartItemPhoto),
			onUpdateItem = new Action<int, GameObject>(this.OnUpdateItemPhoto),
			refScrollView = this.guiData.photoDeck.ScrollView,
			sizeIndex = ((type == SelPhotoEditCtrl.Type.Asistant) ? this.cloneUserOptionData.photoIconSizeAsistant : this.cloneUserOptionData.photoIconSizePartyEdit),
			resetCallback = delegate
			{
				this.guiData.reservePhotoIcon.Clear();
			},
			dispIconPhotoCountCallback = () => this.dispPhotoPackList.Count
		});
		this.guiData.photoDeck.Btn_EditOk.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DECIDE);
		this.guiData.photoDeck.Btn_Yaji_Left.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.photoDeck.Btn_Yaji_Right.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		for (int i = 0; i < this.guiData.photoDeck.mainIconPhotoCtrl.Count; i++)
		{
			SelPhotoEditCtrl.GUI.IconPhotoSet et = this.guiData.photoDeck.mainIconPhotoCtrl[i].iconPhotoSet;
			et.iconPhotoCtrl.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
			RectTransform rectTransform = et.currentFrame.transform as RectTransform;
			rectTransform.offsetMin = new Vector2(22f, 22f);
			rectTransform.offsetMax = new Vector2(-22f, -22f);
			et.iconPhotoCtrl.AddOnClickListener(delegate(IconPhotoCtrl x)
			{
				this.OnTouchPhotoIcon(SelCharaDeckCtrl.FrameType.DECK, et);
			});
			PrjUtil.AddTouchEventTrigger(et.iconBase, delegate(Transform x)
			{
				this.OnTouchPhotoIcon(SelCharaDeckCtrl.FrameType.DECK, et);
			});
		}
		this.guiData.anim.ExInit();
		this.guiData.anim.ExPauseAnimation(SimpleAnimation.ExPguiStatus.START, null);
		this.isInit = true;
	}

	// Token: 0x06001EA6 RID: 7846 RVA: 0x0017C748 File Offset: 0x0017A948
	public void Setup(SelPhotoEditCtrl.SetupParam param)
	{
		this.isSetup = true;
		this.cloneUserOptionData = DataManager.DmUserInfo.optionData.Clone();
		this.setupParam = param;
		this.Reload();
		this.guiData.photoDeck.SetPhotoInfo(new SelPhotoEditCtrl.GUI.PhotoDeck.SetupPhotoInfoParam
		{
			sortType = this.sortType,
			isEnableDetail = !this.setupParam.isTutorial,
			playQuestOneId = this.setupParam.PlayQuestOneId,
			cpd = ((this.setupParam.cbGetCharaPackData != null) ? this.setupParam.cbGetCharaPackData() : null),
			selectIndex = -1,
			isHelper = this.setupParam.isHelper,
			deckCategory = this.setupParam.deckCategory,
			pvpSeasonId = this.setupParam.pvpSeasonId
		});
		this.guiData.photoDeck.sizeChangeBtnGUI.ResetScrollView();
		this.ResetCurrentIcon();
		foreach (SelPhotoEditCtrl.GUI.PhotoEdit photoEdit in this.guiData.photoDeck.mainIconPhotoCtrl)
		{
			photoEdit.iconPhotoSet.currentFrame.gameObject.SetActive(false);
		}
		SortWindowCtrl.RegisterData registerData = new SortWindowCtrl.RegisterData();
		SortWindowCtrl.RegisterData registerData2 = registerData;
		UserDeckData.Category deckCategory = this.setupParam.deckCategory;
		SortFilterDefine.RegisterType registerType;
		if (deckCategory != UserDeckData.Category.PVP)
		{
			if (deckCategory != UserDeckData.Category.TRAINING)
			{
				registerType = SortFilterDefine.RegisterType.PHOTO_DECK;
			}
			else
			{
				registerType = SortFilterDefine.RegisterType.PHOTO_DECK_TRAINING;
			}
		}
		else
		{
			registerType = SortFilterDefine.RegisterType.PHOTO_DECK_PVP;
		}
		registerData2.register = registerType;
		registerData.filterButton = this.guiData.photoDeck.Btn_FilterOnOff;
		registerData.sortButton = this.guiData.photoDeck.Btn_Sort;
		registerData.sortUdButton = this.guiData.photoDeck.Btn_SortUpDown;
		registerData.funcGetTargetBaseList = delegate
		{
			if (this.setupParam.cbGetAllEquipPhoto != null)
			{
				this.deckPhotoDataId = this.setupParam.cbGetAllEquipPhoto();
			}
			List<PhotoPackData> list = new List<PhotoPackData>();
			if (this.selectPhotoData != null && this.selectPhotoData.type == SelCharaDeckCtrl.FrameType.RESERVE)
			{
				list.Add(this.selectPhotoData.photo);
			}
			List<PhotoPackData> list2 = new List<PhotoPackData>();
			List<PhotoPackData> list3 = new List<PhotoPackData>();
			List<PhotoPackData> list4 = new List<PhotoPackData>();
			List<int> list5 = ((this.setupParam.CurrentUserPartyId != null) ? this.setupParam.CurrentUserPartyId() : new List<int>());
			List<int> list6 = new List<int>();
			foreach (int num in list5)
			{
				CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(num);
				if (userCharaData != null)
				{
					list6.Add(userCharaData.staticData.baseData.kizunaPhotoId);
				}
			}
			foreach (PhotoPackData photoPackData in this.havePhotoPackList)
			{
				if (photoPackData.staticData.baseData.isForbiddenEquip)
				{
					list2.Add(photoPackData);
				}
				else if (photoPackData.staticData.baseData.kizunaPhotoFlg && !list6.Contains(photoPackData.staticData.baseData.id))
				{
					list2.Add(photoPackData);
				}
				else if (this.deckPhotoDataId.Contains(photoPackData.dataId))
				{
					list3.Add(photoPackData);
				}
				else
				{
					list4.Add(photoPackData);
				}
			}
			return new SortWindowCtrl.SortTarget
			{
				photoList = list4,
				disableFilterPhotoList = list,
				upperDisableSortPhotoList = list3,
				lowerDisableSortPhotoList = list2
			};
		};
		registerData.funcDisideTarget = delegate(SortWindowCtrl.SortTarget item)
		{
			this.dispPhotoPackList = new List<PhotoPackData>(item.photoList);
			this.sortType = item.sortType;
			if (item.includePhotoBonus && this.setupParam.PlayQuestOneId > 0)
			{
				this.dispPhotoPackList.RemoveAll((PhotoPackData itm) => !DataManager.DmPhoto.IsEnablePhotoBonusByTime(itm.staticData.GetId(), TimeManager.Now, this.setupParam.PlayQuestOneId));
			}
			this.dispPhotoPackList.Insert(0, this.removeButttonPhotoData);
			this.guiData.photoDeck.ResizeScrollView(this.dispPhotoPackList.Count - 1, this.dispPhotoPackList.Count / this.guiData.photoDeck.sizeChangeBtnGUI.IconPhotoParamList[this.guiData.photoDeck.sizeChangeBtnGUI.SizeIndex].num + 1);
		};
		SortWindowCtrl.RegisterData registerData3 = registerData;
		CanvasManager.HdlOpenWindowSortFilter.Register(registerData3, true, null);
		this.SetActive(true, false);
	}

	// Token: 0x06001EA7 RID: 7847 RVA: 0x0017C958 File Offset: 0x0017AB58
	public void SetActive(bool val, bool quick = false)
	{
		if (!this.isSetup)
		{
			this.guiData.photoDeck.baseObj.SetActive(val);
			return;
		}
		if (quick)
		{
			this.guiData.photoDeck.baseObj.SetActive(val);
			return;
		}
		if (val)
		{
			Singleton<SceneManager>.Instance.StartCoroutine(this.StartAnimation());
			return;
		}
		this.NotActive(null);
	}

	// Token: 0x06001EA8 RID: 7848 RVA: 0x0017C9BC File Offset: 0x0017ABBC
	public void NotActive(UnityAction cb = null)
	{
		this.guiData.anim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
		{
			this.guiData.photoDeck.baseObj.SetActive(false);
			UnityAction cb2 = cb;
			if (cb2 == null)
			{
				return;
			}
			cb2();
		});
	}

	// Token: 0x06001EA9 RID: 7849 RVA: 0x0017C9FA File Offset: 0x0017ABFA
	public bool IsPlayingdAnim()
	{
		return this.guiData.anim.ExIsPlaying();
	}

	// Token: 0x06001EAA RID: 7850 RVA: 0x0017CA0C File Offset: 0x0017AC0C
	public void ResetCurrentIcon()
	{
		if (this.selectPhotoData != null)
		{
			SelPhotoEditCtrl.GUI.IconPhotoSet iconPhotoSet = this.guiData.SearchIconPhoto(this.selectPhotoData);
			if (iconPhotoSet != null && iconPhotoSet.currentFrame != null)
			{
				iconPhotoSet.currentFrame.SetActive(false);
			}
			this.selectPhotoData = null;
			this.guiData.photoDeck.SetPhotoInfo(new SelPhotoEditCtrl.GUI.PhotoDeck.SetupPhotoInfoParam
			{
				sortType = this.sortType,
				isEnableDetail = !this.setupParam.isTutorial,
				playQuestOneId = this.setupParam.PlayQuestOneId,
				cpd = ((this.setupParam.cbGetCharaPackData != null) ? this.setupParam.cbGetCharaPackData() : null),
				selectIndex = -1,
				isHelper = this.setupParam.isHelper,
				deckCategory = this.setupParam.deckCategory,
				pvpSeasonId = this.setupParam.pvpSeasonId
			});
		}
		this.guiData.photoDeck.infoPhotoItemEffectCtrl.Setup(new InfoPhotoItemEffectCtrl.SetupParam());
	}

	// Token: 0x06001EAB RID: 7851 RVA: 0x0017CB1A File Offset: 0x0017AD1A
	private void Start()
	{
	}

	// Token: 0x06001EAC RID: 7852 RVA: 0x0017CB1C File Offset: 0x0017AD1C
	private void Update()
	{
	}

	// Token: 0x06001EAD RID: 7853 RVA: 0x0017CB1E File Offset: 0x0017AD1E
	private IEnumerator StartAnimation()
	{
		this.guiData.anim.ExPauseAnimationLastFrame(SimpleAnimation.ExPguiStatus.END);
		yield return null;
		this.guiData.photoDeck.baseObj.SetActive(true);
		this.guiData.anim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
		yield break;
	}

	// Token: 0x06001EAE RID: 7854 RVA: 0x0017CB2D File Offset: 0x0017AD2D
	public void Reload()
	{
		this.isChangeClone = false;
		this.ReloadDeckData();
	}

	// Token: 0x06001EAF RID: 7855 RVA: 0x0017CB3C File Offset: 0x0017AD3C
	private void ReloadDeckData()
	{
		this.havePhotoPackList = new List<PhotoPackData>(DataManager.DmPhoto.GetUserPhotoMap().Values);
		this.dispPhotoPackList = new List<PhotoPackData>(this.havePhotoPackList);
		this.dispPhotoPackList.Insert(0, this.removeButttonPhotoData);
		UserDeckData.Category deckCategory = this.setupParam.deckCategory;
		SortFilterDefine.RegisterType registerType;
		if (deckCategory != UserDeckData.Category.PVP)
		{
			if (deckCategory != UserDeckData.Category.TRAINING)
			{
				registerType = SortFilterDefine.RegisterType.PHOTO_DECK;
			}
			else
			{
				registerType = SortFilterDefine.RegisterType.PHOTO_DECK_TRAINING;
			}
		}
		else
		{
			registerType = SortFilterDefine.RegisterType.PHOTO_DECK_PVP;
		}
		SortFilterDefine.RegisterType registerType2 = registerType;
		CanvasManager.HdlOpenWindowSortFilter.SolutionList(registerType2, null);
		if (this.setupParam.cbGetAllEquipPhoto != null)
		{
			this.deckPhotoDataId = this.setupParam.cbGetAllEquipPhoto();
		}
		if (this.guiData != null)
		{
			this.guiData.photoDeck.ScrollView.Refresh();
		}
	}

	// Token: 0x06001EB0 RID: 7856 RVA: 0x0017CBF8 File Offset: 0x0017ADF8
	private void GetEquipPhotoCount(ref int equipParamPhoto, ref int equipAbilityPhoto)
	{
		foreach (long num in ((this.setupParam.cbGetEquipPhoto != null) ? this.setupParam.cbGetEquipPhoto() : new List<long>()))
		{
			if (num != 0L)
			{
				PhotoPackData userPhotoData = DataManager.DmPhoto.GetUserPhotoData(num);
				if (userPhotoData != null)
				{
					if (userPhotoData.staticData.baseData.type == PhotoDef.Type.ABILITY)
					{
						equipAbilityPhoto++;
					}
					else if (userPhotoData.staticData.baseData.type == PhotoDef.Type.PARAMETER)
					{
						equipParamPhoto++;
					}
				}
			}
		}
	}

	// Token: 0x06001EB1 RID: 7857 RVA: 0x0017CCA8 File Offset: 0x0017AEA8
	private void SelectPhotoIcon(SelPhotoEditCtrl.SelectPhotoData newSelectPhoto, SelPhotoEditCtrl.SelectPhotoData oldSelectPhoto)
	{
		int num = -1;
		SelPhotoEditCtrl.GUI.IconPhotoSet iconPhotoSet = this.guiData.SearchIconPhoto(newSelectPhoto);
		SelPhotoEditCtrl.GUI.IconPhotoSet iconPhotoSet2 = this.guiData.SearchIconPhoto(oldSelectPhoto);
		List<PguiAECtrl> list = new List<PguiAECtrl>();
		List<long> list2 = ((this.setupParam.cbGetEquipPhoto != null) ? this.setupParam.cbGetEquipPhoto() : new List<long>());
		if (oldSelectPhoto == null)
		{
			iconPhotoSet.currentFrame.SetActive(true);
			this.selectPhotoData = newSelectPhoto;
			num = this.guiData.photoDeck.mainIconPhotoCtrl.FindIndex((SelPhotoEditCtrl.GUI.PhotoEdit item) => item.iconPhotoSet.iconPhotoCtrl.photoPackData == this.selectPhotoData.photo);
		}
		else if (newSelectPhoto.type == SelCharaDeckCtrl.FrameType.RESERVE && oldSelectPhoto.type == SelCharaDeckCtrl.FrameType.RESERVE)
		{
			if (newSelectPhoto.photo == oldSelectPhoto.photo)
			{
				iconPhotoSet.currentFrame.SetActive(false);
				if (iconPhotoSet2 != null)
				{
					iconPhotoSet2.currentFrame.SetActive(false);
				}
				this.selectPhotoData = null;
			}
			else
			{
				iconPhotoSet.currentFrame.SetActive(true);
				if (iconPhotoSet2 != null)
				{
					iconPhotoSet2.currentFrame.SetActive(false);
				}
				this.selectPhotoData = newSelectPhoto;
			}
		}
		else if ((newSelectPhoto.type == SelCharaDeckCtrl.FrameType.RESERVE && oldSelectPhoto.type == SelCharaDeckCtrl.FrameType.DECK) || (newSelectPhoto.type == SelCharaDeckCtrl.FrameType.DECK && oldSelectPhoto.type == SelCharaDeckCtrl.FrameType.RESERVE))
		{
			SelPhotoEditCtrl.SelectPhotoData selectPhotoData = ((newSelectPhoto.type == SelCharaDeckCtrl.FrameType.RESERVE) ? newSelectPhoto : oldSelectPhoto);
			SelPhotoEditCtrl.SelectPhotoData deckPhoto = ((newSelectPhoto.type == SelCharaDeckCtrl.FrameType.DECK) ? newSelectPhoto : oldSelectPhoto);
			int num2 = this.guiData.photoDeck.mainIconPhotoCtrl.FindIndex((SelPhotoEditCtrl.GUI.PhotoEdit item) => item.iconPhotoSet.iconPhotoCtrl.photoPackData == deckPhoto.photo);
			if (selectPhotoData.photo == this.removeButttonPhotoData && !deckPhoto.photo.IsInvalid())
			{
				list2[num2] = 0L;
				this.isChangeClone = true;
			}
			else if (selectPhotoData.photo != this.removeButttonPhotoData && selectPhotoData.photo.dataId != deckPhoto.photo.dataId)
			{
				if (list2.Contains(selectPhotoData.photo.dataId))
				{
					int num3 = list2.IndexOf(selectPhotoData.photo.dataId);
					long num4 = list2[num3];
					list2[num3] = list2[num2];
					list2[num2] = num4;
					list.Add(iconPhotoSet.iconPhotoCtrl.aeEffChange);
					if (iconPhotoSet2 != null)
					{
						list.Add(iconPhotoSet2.iconPhotoCtrl.aeEffChange);
					}
					this.isChangeClone = true;
				}
				else if ((newSelectPhoto.type == SelCharaDeckCtrl.FrameType.DECK || !selectPhotoData.textDisable) && (newSelectPhoto.type == SelCharaDeckCtrl.FrameType.RESERVE || !deckPhoto.textDisable))
				{
					if (this.setupParam.cbResignEquipPhotoByDataId != null)
					{
						this.setupParam.cbResignEquipPhotoByDataId(selectPhotoData.photo.dataId);
					}
					list2[num2] = selectPhotoData.photo.dataId;
					list.Add(iconPhotoSet.iconPhotoCtrl.aeEffChange);
					if (iconPhotoSet2 != null)
					{
						list.Add(iconPhotoSet2.iconPhotoCtrl.aeEffChange);
					}
					this.isChangeClone = true;
				}
			}
			num = num2;
			iconPhotoSet.currentFrame.SetActive(false);
			if (iconPhotoSet2 != null)
			{
				iconPhotoSet2.currentFrame.SetActive(false);
			}
			this.selectPhotoData = null;
			this.ChangePhotoInfo(-1);
		}
		else if (newSelectPhoto.type == SelCharaDeckCtrl.FrameType.DECK && oldSelectPhoto.type == SelCharaDeckCtrl.FrameType.DECK)
		{
			if (newSelectPhoto.photo != oldSelectPhoto.photo)
			{
				int num5 = this.guiData.photoDeck.mainIconPhotoCtrl.FindIndex((SelPhotoEditCtrl.GUI.PhotoEdit item) => item.iconPhotoSet.iconPhotoCtrl.photoPackData == newSelectPhoto.photo);
				int num6 = this.guiData.photoDeck.mainIconPhotoCtrl.FindIndex((SelPhotoEditCtrl.GUI.PhotoEdit item) => item.iconPhotoSet.iconPhotoCtrl.photoPackData == oldSelectPhoto.photo);
				long num7 = list2[num5];
				list2[num5] = list2[num6];
				list2[num6] = num7;
				list.Add(iconPhotoSet.iconPhotoCtrl.aeEffChange);
				if (iconPhotoSet2 != null)
				{
					list.Add(iconPhotoSet2.iconPhotoCtrl.aeEffChange);
				}
				this.isChangeClone = true;
				num = num5;
			}
			iconPhotoSet.currentFrame.SetActive(false);
			if (iconPhotoSet2 != null)
			{
				iconPhotoSet2.currentFrame.SetActive(false);
			}
			this.selectPhotoData = null;
			this.ChangePhotoInfo(-1);
		}
		if (this.isChangeClone)
		{
			for (int i = 0; i < list.Count; i++)
			{
				list[i].PlayAnimation(PguiAECtrl.AmimeType.START, null);
			}
		}
		this.guiData.photoDeck.ScrollView.Refresh();
		if (this.selectPhotoData == null)
		{
			for (int j = 0; j < list2.Count; j++)
			{
				this.guiData.photoDeck.mainIconPhotoCtrl[j].Active.gameObject.SetActive(false);
			}
		}
		this.guiData.photoDeck.SetPhotoInfo(new SelPhotoEditCtrl.GUI.PhotoDeck.SetupPhotoInfoParam
		{
			ppd = ((this.selectPhotoData == null) ? null : this.selectPhotoData.photo),
			sortType = this.sortType,
			isEnableDetail = !this.setupParam.isTutorial,
			playQuestOneId = this.setupParam.PlayQuestOneId,
			cpd = ((this.setupParam.cbGetCharaPackData != null) ? this.setupParam.cbGetCharaPackData() : null),
			selectIndex = num,
			isHelper = this.setupParam.isHelper,
			deckCategory = this.setupParam.deckCategory,
			pvpSeasonId = this.setupParam.pvpSeasonId
		});
		this.guiData.photoDeck.infoIconPhotoCtrl.AddOnUpdateLockListener(delegate(IconPhotoCtrl lockPhoto)
		{
			SelPhotoEditCtrl.GUI.PhotoEdit photoEdit2 = this.guiData.photoDeck.mainIconPhotoCtrl.Find((SelPhotoEditCtrl.GUI.PhotoEdit item) => item.iconPhotoSet.iconPhotoCtrl.photoPackData == lockPhoto.photoPackData);
			if (photoEdit2 != null)
			{
				photoEdit2.iconPhotoSet.iconPhotoCtrl.DispLockIcon(lockPhoto.photoPackData.dynamicData.lockFlag);
			}
			this.guiData.photoDeck.ScrollView.Refresh();
		});
		this.guiData.photoDeck.infoIconPhotoCtrl.AddOnUpdateFavoriteListener(delegate(IconPhotoCtrl favoritePhoto)
		{
			SelPhotoEditCtrl.GUI.PhotoEdit photoEdit3 = this.guiData.photoDeck.mainIconPhotoCtrl.Find((SelPhotoEditCtrl.GUI.PhotoEdit item) => item.iconPhotoSet.iconPhotoCtrl.photoPackData == favoritePhoto.photoPackData);
			if (photoEdit3 != null)
			{
				photoEdit3.iconPhotoSet.iconPhotoCtrl.DispFavoriteIcon(favoritePhoto.photoPackData.dynamicData.favoriteFlag);
			}
			this.guiData.photoDeck.ScrollView.Refresh();
		});
		this.guiData.photoDeck.infoIconPhotoCtrl.AddOnUpdateStatus(delegate(IconPhotoCtrl x)
		{
			this.guiData.photoDeck.ScrollView.Refresh();
		});
		this.guiData.photoDeck.infoIconPhotoCtrl.AddOnCloseWindow(delegate(IconPhotoCtrl item)
		{
			UserDeckData.Category deckCategory = this.setupParam.deckCategory;
			SortFilterDefine.RegisterType registerType;
			if (deckCategory != UserDeckData.Category.PVP)
			{
				if (deckCategory != UserDeckData.Category.TRAINING)
				{
					registerType = SortFilterDefine.RegisterType.PHOTO_DECK;
				}
				else
				{
					registerType = SortFilterDefine.RegisterType.PHOTO_DECK_TRAINING;
				}
			}
			else
			{
				registerType = SortFilterDefine.RegisterType.PHOTO_DECK_PVP;
			}
			SortFilterDefine.RegisterType registerType2 = registerType;
			CanvasManager.HdlOpenWindowSortFilter.SolutionList(registerType2, null);
		});
		if (newSelectPhoto.type == SelCharaDeckCtrl.FrameType.RESERVE)
		{
			CharaPackData charaPackData = ((this.setupParam.cbGetCharaPackData != null) ? this.setupParam.cbGetCharaPackData() : null);
			if (charaPackData == null)
			{
				charaPackData = CharaPackData.MakeInvalid();
			}
			for (int k = 0; k < list2.Count; k++)
			{
				long num8 = list2[k];
				IconPhotoCtrl iconPhotoCtrl = this.guiData.photoDeck.mainIconPhotoCtrl[k].iconPhotoSet.iconPhotoCtrl;
				SelPhotoEditCtrl.GUI.PhotoEdit photoEdit = this.guiData.photoDeck.mainIconPhotoCtrl[k];
				PhotoPackData userPhotoData = DataManager.DmPhoto.GetUserPhotoData(num8);
				int num9 = ((!charaPackData.IsInvalid()) ? charaPackData.staticData.baseData.kizunaPhotoId : 0);
				bool flag = userPhotoData != null && userPhotoData.staticData.baseData.kizunaPhotoFlg && userPhotoData.staticData.GetId() == num9;
				bool flag2 = userPhotoData != null && !userPhotoData.IsInvalid() && userPhotoData.staticData.baseData.kizunaPhotoFlg;
				if (this.selectPhotoData == null)
				{
					if ((!charaPackData.IsInvalid() && charaPackData.dynamicData.PhotoPocket[k].Flag) || num8 == 0L)
					{
						iconPhotoCtrl.DispImgDisable(flag2 && !flag);
						iconPhotoCtrl.DispTextDisable(flag2 && !flag, PhotoUtil.FriendsText, null);
					}
					if (!charaPackData.IsInvalid() && !charaPackData.dynamicData.PhotoPocket[k].Flag && num8 != 0L)
					{
						iconPhotoCtrl.DispTextDisable(flag2 && !flag, PhotoUtil.FriendsText, null);
					}
				}
			}
		}
	}

	// Token: 0x06001EB2 RID: 7858 RVA: 0x0017D4F0 File Offset: 0x0017B6F0
	private void OnTouchPhotoIcon(SelCharaDeckCtrl.FrameType type, SelPhotoEditCtrl.GUI.IconPhotoSet iconPhoto)
	{
		SelPhotoEditCtrl.<>c__DisplayClass37_0 CS$<>8__locals1 = new SelPhotoEditCtrl.<>c__DisplayClass37_0();
		CS$<>8__locals1.<>4__this = this;
		SoundManager.Play("prd_se_click", false, false);
		CS$<>8__locals1.ppd = iconPhoto.iconPhotoCtrl.photoPackData;
		CS$<>8__locals1.equipPhotoList = ((this.setupParam.cbGetEquipPhoto != null) ? this.setupParam.cbGetEquipPhoto() : new List<long>());
		if (type == SelCharaDeckCtrl.FrameType.RESERVE && CS$<>8__locals1.ppd != null && CS$<>8__locals1.ppd.staticData != null)
		{
			SelPhotoEditCtrl.<>c__DisplayClass37_1 CS$<>8__locals2 = new SelPhotoEditCtrl.<>c__DisplayClass37_1();
			CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
			CS$<>8__locals2.cpd = ((this.setupParam.cbGetCharaPackData != null) ? this.setupParam.cbGetCharaPackData() : null);
			if (CS$<>8__locals2.cpd == null)
			{
				CS$<>8__locals2.cpd = CharaPackData.MakeInvalid();
			}
			List<long> list3 = CS$<>8__locals2.CS$<>8__locals1.equipPhotoList.FindAll((long item) => item != 0L && item != CS$<>8__locals2.CS$<>8__locals1.ppd.dataId && DataManager.DmPhoto.GetUserPhotoData(item).staticData.baseData.type == CS$<>8__locals2.CS$<>8__locals1.ppd.staticData.baseData.type);
			List<long> list2 = list3.FindAll((long item) => DataManager.DmPhoto.GetUserPhotoData(item).staticData.GetId() == CS$<>8__locals2.CS$<>8__locals1.ppd.staticData.GetId());
			int num = ((CS$<>8__locals2.cpd != null && !CS$<>8__locals2.cpd.IsInvalid()) ? CS$<>8__locals2.cpd.staticData.baseData.kizunaPhotoId : 0);
			int num2;
			int k;
			for (k = 0; k < CS$<>8__locals2.CS$<>8__locals1.equipPhotoList.Count; k = num2)
			{
				SelPhotoEditCtrl.GUI.PhotoEdit photoEdit = this.guiData.photoDeck.mainIconPhotoCtrl[k];
				IconPhotoCtrl iconPhotoCtrl = this.guiData.photoDeck.mainIconPhotoCtrl[k].iconPhotoSet.iconPhotoCtrl;
				photoEdit.Active.gameObject.SetActive(false);
				iconPhotoCtrl.DispImgDisable(false);
				iconPhotoCtrl.DispTextDisable(false, null, null);
				PhotoPackData photoPackData = this.havePhotoPackList.Find((PhotoPackData item) => item != CS$<>8__locals2.CS$<>8__locals1.<>4__this.removeButttonPhotoData && item.dataId == CS$<>8__locals2.CS$<>8__locals1.equipPhotoList[k]);
				if (photoPackData == null)
				{
					photoPackData = PhotoPackData.MakeInvalid();
				}
				if (!CS$<>8__locals2.cpd.IsInvalid() && CS$<>8__locals2.cpd.dynamicData.PhotoPocket[k].Flag)
				{
					photoEdit.BaseBlank_Lock.SetActive(false);
					photoEdit.BaseBlank_Plus.SetActive(true);
					iconPhotoCtrl.DispImgDisable(false);
					iconPhotoCtrl.DispTextDisable(false, null, null);
				}
				else
				{
					photoEdit.BaseBlank_Lock.SetActive(true);
					photoEdit.BaseBlank_Plus.SetActive(false);
					if (!photoPackData.IsInvalid())
					{
						iconPhotoCtrl.DispImgDisable(true);
						iconPhotoCtrl.DispMarkNotYetReleased(true);
					}
				}
				if (!photoPackData.IsInvalid() && photoPackData.staticData.baseData.kizunaPhotoFlg)
				{
					iconPhotoCtrl.DispImgDisable(CS$<>8__locals2.cpd == null || CS$<>8__locals2.cpd.IsInvalid() || CS$<>8__locals2.cpd.staticData.baseData.kizunaPhotoId != photoPackData.staticData.GetId() || !CS$<>8__locals2.cpd.dynamicData.PhotoPocket[k].Flag);
					iconPhotoCtrl.DispTextDisable(CS$<>8__locals2.cpd == null || CS$<>8__locals2.cpd.IsInvalid() || CS$<>8__locals2.cpd.staticData.baseData.kizunaPhotoId != photoPackData.staticData.GetId(), PhotoUtil.FriendsText, PhotoUtil.NoFormationText);
				}
				num2 = k + 1;
			}
			int i;
			Predicate<long> <>9__4;
			for (i = 0; i < CS$<>8__locals2.CS$<>8__locals1.equipPhotoList.Count; i = num2)
			{
				IconPhotoCtrl mainIconPhotoCtrl = this.guiData.photoDeck.mainIconPhotoCtrl[i].iconPhotoSet.iconPhotoCtrl;
				SelPhotoEditCtrl.GUI.PhotoEdit mainIconPhoto = this.guiData.photoDeck.mainIconPhotoCtrl[i];
				int j = i;
				Action<List<long>, string> action = delegate(List<long> list, string str)
				{
					Predicate<long> predicate;
					if ((predicate = <>9__4) == null)
					{
						predicate = (<>9__4 = (long item) => item == CS$<>8__locals2.CS$<>8__locals1.equipPhotoList[i]);
					}
					if (list.Find(predicate) > 0L)
					{
						if (CS$<>8__locals2.cpd != null && !CS$<>8__locals2.cpd.IsInvalid() && CS$<>8__locals2.cpd.dynamicData.PhotoPocket[i].Flag)
						{
							mainIconPhoto.Active.gameObject.SetActive(true);
							mainIconPhoto.StartAnim();
							mainIconPhotoCtrl.DispTextDisable(false, null, null);
							return;
						}
					}
					else
					{
						PhotoPackData userPhotoData2 = DataManager.DmPhoto.GetUserPhotoData(CS$<>8__locals2.CS$<>8__locals1.equipPhotoList[i]);
						mainIconPhotoCtrl.SetupNoEquipIcon(userPhotoData2);
						mainIconPhotoCtrl.DispImgDisable(true);
						mainIconPhotoCtrl.DispTextDisable(true, PrjUtil.MakeMessage(str), null);
					}
				};
				if (CS$<>8__locals2.CS$<>8__locals1.ppd.staticData.baseData.isForbiddenEquip)
				{
					PhotoPackData userPhotoData = DataManager.DmPhoto.GetUserPhotoData(CS$<>8__locals2.CS$<>8__locals1.equipPhotoList[i]);
					mainIconPhotoCtrl.SetupNoEquipIcon(userPhotoData);
					mainIconPhotoCtrl.DispImgDisable(true);
					mainIconPhotoCtrl.DispTextDisable(true, PhotoUtil.NoSelectedText, null);
				}
				else if (list2.Count > 0)
				{
					action(list2, PhotoUtil.SamePhotoText);
				}
				else if (CS$<>8__locals2.CS$<>8__locals1.ppd.staticData.baseData.kizunaPhotoFlg)
				{
					if (CS$<>8__locals2.CS$<>8__locals1.ppd.staticData.GetId() != num)
					{
						mainIconPhotoCtrl.SetupNoEquipIcon(DataManager.DmPhoto.GetUserPhotoData(CS$<>8__locals2.CS$<>8__locals1.equipPhotoList[i]));
						mainIconPhotoCtrl.DispImgDisable(true);
						mainIconPhotoCtrl.DispTextDisable(true, PhotoUtil.FriendsText, null);
					}
					else if (list3.Count >= 2)
					{
						action(list3, PhotoUtil.PhotoTypeText);
					}
				}
				else if (list3.Count >= 2)
				{
					action(list3, PhotoUtil.PhotoTypeText);
				}
				else if (!CS$<>8__locals2.cpd.IsInvalid() && CS$<>8__locals2.cpd.dynamicData.PhotoPocket[i].Flag)
				{
					mainIconPhoto.Active.gameObject.SetActive(true);
					mainIconPhoto.StartAnim();
					mainIconPhotoCtrl.DispTextDisable(false, null, null);
				}
				num2 = i + 1;
			}
		}
		this.SelectPhotoIcon(new SelPhotoEditCtrl.SelectPhotoData(type, CS$<>8__locals1.ppd, iconPhoto.iconPhotoCtrl.CheckTextDisable(false, null)), this.selectPhotoData);
		if (this.setupParam.deckCategory == UserDeckData.Category.PVP || this.setupParam.deckCategory == UserDeckData.Category.TRAINING)
		{
			this.guiData.photoDeck.infoPhotoItemEffectCtrl.Setup(new InfoPhotoItemEffectCtrl.SetupParam());
		}
		else
		{
			this.guiData.photoDeck.infoPhotoItemEffectCtrl.Setup(new InfoPhotoItemEffectCtrl.SetupParam
			{
				photoPackDatas = new List<PhotoPackData> { (this.selectPhotoData != null) ? this.selectPhotoData.photo : null }
			});
		}
		this.TouchRect = true;
	}

	// Token: 0x06001EB3 RID: 7859 RVA: 0x0017DC2C File Offset: 0x0017BE2C
	private void OnStartItemPhoto(int index, GameObject go)
	{
		for (int i = 0; i < this.guiData.photoDeck.sizeChangeBtnGUI.IconPhotoParamList[this.guiData.photoDeck.sizeChangeBtnGUI.SizeIndex].num; i++)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_PhotoSet, go.transform);
			gameObject.name = i.ToString();
			SelPhotoEditCtrl.GUI.IconPhotoSet et = new SelPhotoEditCtrl.GUI.IconPhotoSet(gameObject.transform);
			et.iconPhotoCtrl.AddOnClickListener(delegate(IconPhotoCtrl x)
			{
				this.OnTouchPhotoIcon(SelCharaDeckCtrl.FrameType.RESERVE, et);
			});
			et.iconPhotoCtrl.AddOnUpdateLockListener(delegate(IconPhotoCtrl lockPhoto)
			{
				SelPhotoEditCtrl.GUI.PhotoEdit photoEdit = this.guiData.photoDeck.mainIconPhotoCtrl.Find((SelPhotoEditCtrl.GUI.PhotoEdit item) => item.iconPhotoSet.iconPhotoCtrl.photoPackData == lockPhoto.photoPackData);
				if (photoEdit != null)
				{
					photoEdit.iconPhotoSet.iconPhotoCtrl.DispLockIcon(lockPhoto.photoPackData.dynamicData.lockFlag);
				}
			});
			et.iconPhotoCtrl.AddOnUpdateFavoriteListener(delegate(IconPhotoCtrl favoritePhoto)
			{
				SelPhotoEditCtrl.GUI.PhotoEdit photoEdit2 = this.guiData.photoDeck.mainIconPhotoCtrl.Find((SelPhotoEditCtrl.GUI.PhotoEdit item) => item.iconPhotoSet.iconPhotoCtrl.photoPackData == favoritePhoto.photoPackData);
				if (photoEdit2 != null)
				{
					photoEdit2.iconPhotoSet.iconPhotoCtrl.DispFavoriteIcon(favoritePhoto.photoPackData.dynamicData.favoriteFlag);
				}
			});
			et.iconPhotoCtrl.AddOnUpdateStatus(delegate(IconPhotoCtrl photo)
			{
				this.guiData.photoDeck.ScrollView.Refresh();
				if (this.selectPhotoData == null)
				{
					return;
				}
				if (this.selectPhotoData.photo == photo.photoPackData)
				{
					this.guiData.photoDeck.infoIconPhotoCtrl.Setup(photo.photoPackData, SortFilterDefine.SortType.LEVEL, true, false, -1, false);
					return;
				}
				if (this.guiData.photoDeck.mainIconPhotoCtrl.Find((SelPhotoEditCtrl.GUI.PhotoEdit item) => item.iconPhotoSet.iconPhotoCtrl.photoPackData == photo.photoPackData) != null)
				{
					this.guiData.photoDeck.infoIconPhotoCtrl.Setup(this.selectPhotoData.photo, SortFilterDefine.SortType.LEVEL, true, false, -1, false);
				}
			});
			et.iconPhotoCtrl.AddOnCloseWindow(delegate(IconPhotoCtrl item)
			{
				UserDeckData.Category deckCategory = this.setupParam.deckCategory;
				SortFilterDefine.RegisterType registerType;
				if (deckCategory != UserDeckData.Category.PVP)
				{
					if (deckCategory != UserDeckData.Category.TRAINING)
					{
						registerType = SortFilterDefine.RegisterType.PHOTO_DECK;
					}
					else
					{
						registerType = SortFilterDefine.RegisterType.PHOTO_DECK_TRAINING;
					}
				}
				else
				{
					registerType = SortFilterDefine.RegisterType.PHOTO_DECK_PVP;
				}
				SortFilterDefine.RegisterType registerType2 = registerType;
				CanvasManager.HdlOpenWindowSortFilter.SolutionList(registerType2, null);
			});
			et.baseObj.transform.Find("Icon_Photo").localScale = this.guiData.photoDeck.sizeChangeBtnGUI.IconPhotoParamList[this.guiData.photoDeck.sizeChangeBtnGUI.SizeIndex].scale;
			et.SetScale(this.guiData.photoDeck.sizeChangeBtnGUI.IconPhotoParamList[this.guiData.photoDeck.sizeChangeBtnGUI.SizeIndex].scaleCurrent, this.guiData.photoDeck.sizeChangeBtnGUI.IconPhotoParamList[this.guiData.photoDeck.sizeChangeBtnGUI.SizeIndex].scaleCount);
			PrjUtil.AddTouchEventTrigger(et.iconBase, delegate(Transform x)
			{
				this.OnTouchPhotoIcon(SelCharaDeckCtrl.FrameType.RESERVE, et);
			});
			this.guiData.reservePhotoIcon.Add(et);
		}
	}

	// Token: 0x06001EB4 RID: 7860 RVA: 0x0017DE44 File Offset: 0x0017C044
	private void OnUpdateItemPhoto(int index, GameObject go)
	{
		List<long> list = ((this.setupParam.cbGetEquipPhoto != null) ? this.setupParam.cbGetEquipPhoto() : new List<long>());
		int num = 0;
		int num2 = 0;
		this.GetEquipPhotoCount(ref num, ref num2);
		CharaPackData charaPackData = ((this.setupParam.cbGetCharaPackData != null) ? this.setupParam.cbGetCharaPackData() : null);
		if (charaPackData == null)
		{
			charaPackData = CharaPackData.MakeInvalid();
		}
		CharaStaticData staticData = charaPackData.staticData;
		for (int i = 0; i < this.guiData.photoDeck.sizeChangeBtnGUI.IconPhotoParamList[this.guiData.photoDeck.sizeChangeBtnGUI.SizeIndex].num; i++)
		{
			GameObject iconObj = go.transform.Find(i.ToString()).gameObject;
			SelPhotoEditCtrl.GUI.IconPhotoSet iconPhotoSet = this.guiData.reservePhotoIcon.Find((SelPhotoEditCtrl.GUI.IconPhotoSet item) => item.baseObj == iconObj);
			int num3 = index * this.guiData.photoDeck.sizeChangeBtnGUI.IconPhotoParamList[this.guiData.photoDeck.sizeChangeBtnGUI.SizeIndex].num + i;
			if (this.dispPhotoPackList.Count > num3)
			{
				iconPhotoSet.baseObj.SetActive(true);
				PhotoPackData ppd = this.dispPhotoPackList[num3];
				iconPhotoSet.iconPhotoCtrl.Setup(ppd, this.sortType, !this.setupParam.isTutorial, false, this.setupParam.PlayQuestOneId, this.setupParam.isHelper);
				if (DataManagerDeck.CheckDisableDropIcon(this.setupParam.deckCategory, this.setupParam.pvpSeasonId))
				{
					iconPhotoSet.iconPhotoCtrl.DispDrop(false, 0);
				}
				iconPhotoSet.iconPhotoCtrl.onReturnPhotoPackDataList = () => this.dispPhotoPackList;
				iconPhotoSet.iconPhotoCtrl.DispRemove(this.removeButttonPhotoData == ppd);
				iconPhotoSet.currentFrame.SetActive(this.selectPhotoData != null && this.selectPhotoData.type == SelCharaDeckCtrl.FrameType.RESERVE && this.selectPhotoData.photo == ppd);
				int num4 = 0;
				bool flag = false;
				if (ppd != null && !ppd.IsInvalid())
				{
					num4 = ((this.setupParam.cbIsEquipPhoto != null) ? this.setupParam.cbIsEquipPhoto(ppd.dataId) : 0);
					flag = num4 != 0;
				}
				CharaStaticData charaStaticData = null;
				if (num4 != 0)
				{
					charaStaticData = DataManager.DmChara.GetCharaStaticData(num4);
				}
				iconPhotoSet.iconPhotoCtrl.DispImgDisable(false);
				iconPhotoSet.iconPhotoCtrl.DispIconCharaMini(flag, charaStaticData);
				if (ppd != null && !ppd.IsInvalid())
				{
					if (ppd.staticData.baseData.isForbiddenEquip)
					{
						iconPhotoSet.iconPhotoCtrl.DispImgDisable(true);
						iconPhotoSet.iconPhotoCtrl.DispTextDisable(true, PhotoUtil.NoSelectedText, null);
					}
					else if (ppd.staticData.baseData.kizunaPhotoFlg && staticData != null && staticData.baseData.kizunaPhotoId != ppd.staticData.GetId())
					{
						iconPhotoSet.iconPhotoCtrl.DispImgDisable(true);
						iconPhotoSet.iconPhotoCtrl.DispTextDisable(true, PhotoUtil.FriendsText, null);
					}
					if (this.selectPhotoData != null)
					{
						if (this.selectPhotoData.photo.IsInvalid())
						{
							bool flag2 = false;
							if (ppd.staticData.baseData.type == PhotoDef.Type.PARAMETER && num >= 2)
							{
								flag2 = true;
							}
							else if (ppd.staticData.baseData.type == PhotoDef.Type.ABILITY && num2 >= 2)
							{
								flag2 = true;
							}
							if (flag2 && !iconPhotoSet.iconPhotoCtrl.CheckTextDisable(true, PhotoUtil.FriendsText))
							{
								iconPhotoSet.iconPhotoCtrl.DispImgDisable(true);
								iconPhotoSet.iconPhotoCtrl.DispTextDisable(true, PhotoUtil.PhotoTypeText, null);
							}
						}
						else if (num + num2 >= 3)
						{
							if (num2 >= 2 && this.selectPhotoData.photo.staticData.baseData.type == PhotoDef.Type.PARAMETER)
							{
								if (ppd.staticData.baseData.type == PhotoDef.Type.ABILITY)
								{
									iconPhotoSet.iconPhotoCtrl.DispImgDisable(true);
									iconPhotoSet.iconPhotoCtrl.DispTextDisable(true, PhotoUtil.PhotoTypeText, null);
								}
							}
							else if (num >= 2 && this.selectPhotoData.photo.staticData.baseData.type == PhotoDef.Type.ABILITY && ppd.staticData.baseData.type == PhotoDef.Type.PARAMETER)
							{
								iconPhotoSet.iconPhotoCtrl.DispImgDisable(true);
								iconPhotoSet.iconPhotoCtrl.DispTextDisable(true, PhotoUtil.PhotoTypeText, null);
							}
						}
						foreach (long num5 in list.FindAll((long item) => item != ppd.dataId))
						{
							bool flag3 = false;
							PhotoPackData userPhotoData = DataManager.DmPhoto.GetUserPhotoData(num5);
							if (userPhotoData != null && !userPhotoData.IsInvalid() && this.selectPhotoData != null && !this.selectPhotoData.photo.IsInvalid())
							{
								if (userPhotoData.staticData.GetId() == ppd.staticData.GetId() && this.selectPhotoData.photo.staticData.GetId() != userPhotoData.staticData.GetId())
								{
									flag3 = true;
								}
							}
							else if (userPhotoData != null && !userPhotoData.IsInvalid() && this.selectPhotoData != null && this.selectPhotoData.photo.IsInvalid() && userPhotoData.staticData.GetId() == ppd.staticData.GetId())
							{
								flag3 = true;
							}
							if (flag3)
							{
								iconPhotoSet.iconPhotoCtrl.DispImgDisable(true);
								iconPhotoSet.iconPhotoCtrl.DispTextDisable(true, PhotoUtil.SamePhotoText, null);
							}
						}
					}
				}
				iconPhotoSet.iconPhotoCtrl.DispParty(flag, !iconPhotoSet.iconPhotoCtrl.CheckTextDisable(false, null));
			}
			else
			{
				iconPhotoSet.baseObj.SetActive(false);
			}
		}
		go.GetComponent<GridLayoutGroup>().enabled = false;
		go.GetComponent<GridLayoutGroup>().enabled = true;
	}

	// Token: 0x06001EB5 RID: 7861 RVA: 0x0017E4BC File Offset: 0x0017C6BC
	private void OnClickButton(PguiButtonCtrl button)
	{
		if (button == this.guiData.photoDeck.Btn_EditOk)
		{
			if (this.setupParam.cbButtonEditOk != null)
			{
				this.setupParam.cbButtonEditOk(button);
				return;
			}
		}
		else if ((button == this.guiData.photoDeck.Btn_Yaji_Left || button == this.guiData.photoDeck.Btn_Yaji_Right) && this.setupParam.cbButtonArrow != null)
		{
			this.setupParam.cbButtonArrow(button);
		}
	}

	// Token: 0x0400165D RID: 5725
	private bool isInit;

	// Token: 0x0400165E RID: 5726
	private bool isSetup;

	// Token: 0x0400165F RID: 5727
	public SelPhotoEditCtrl.GUI guiData;

	// Token: 0x04001660 RID: 5728
	private SortFilterDefine.SortType sortType = SortFilterDefine.SortType.LEVEL;

	// Token: 0x04001661 RID: 5729
	public SelPhotoEditCtrl.SetupParam setupParam = new SelPhotoEditCtrl.SetupParam();

	// Token: 0x04001662 RID: 5730
	private SelPhotoEditCtrl.SelectPhotoData selectPhotoData;

	// Token: 0x04001663 RID: 5731
	private List<PhotoPackData> havePhotoPackList;

	// Token: 0x04001664 RID: 5732
	private PhotoPackData removeButttonPhotoData = PhotoPackData.MakeInvalid();

	// Token: 0x04001665 RID: 5733
	private List<long> deckPhotoDataId = new List<long>();

	// Token: 0x04001666 RID: 5734
	private List<PhotoPackData> dispPhotoPackList;

	// Token: 0x04001667 RID: 5735
	private UserOptionData cloneUserOptionData;

	// Token: 0x04001669 RID: 5737
	private bool touchRect;

	// Token: 0x02000FBC RID: 4028
	public enum Type
	{
		// Token: 0x040058C0 RID: 22720
		Party,
		// Token: 0x040058C1 RID: 22721
		Asistant
	}

	// Token: 0x02000FBD RID: 4029
	public class SelectPhotoData
	{
		// Token: 0x060050C6 RID: 20678 RVA: 0x002437A9 File Offset: 0x002419A9
		public SelectPhotoData(SelCharaDeckCtrl.FrameType t, PhotoPackData c, bool b)
		{
			this.type = t;
			this.photo = c;
			this.textDisable = b;
		}

		// Token: 0x040058C2 RID: 22722
		public SelCharaDeckCtrl.FrameType type;

		// Token: 0x040058C3 RID: 22723
		public PhotoPackData photo;

		// Token: 0x040058C4 RID: 22724
		public bool textDisable;
	}

	// Token: 0x02000FBE RID: 4030
	public class SetupParam
	{
		// Token: 0x17000B95 RID: 2965
		// (get) Token: 0x060050C7 RID: 20679 RVA: 0x002437C6 File Offset: 0x002419C6
		// (set) Token: 0x060050C8 RID: 20680 RVA: 0x002437CE File Offset: 0x002419CE
		public SelPhotoEditCtrl.SetupParam.OnClickButton cbButtonEditOk { get; set; }

		// Token: 0x17000B96 RID: 2966
		// (get) Token: 0x060050C9 RID: 20681 RVA: 0x002437D7 File Offset: 0x002419D7
		// (set) Token: 0x060050CA RID: 20682 RVA: 0x002437DF File Offset: 0x002419DF
		public SelPhotoEditCtrl.SetupParam.OnClickButton cbButtonArrow { get; set; }

		// Token: 0x17000B97 RID: 2967
		// (get) Token: 0x060050CB RID: 20683 RVA: 0x002437E8 File Offset: 0x002419E8
		// (set) Token: 0x060050CC RID: 20684 RVA: 0x002437F0 File Offset: 0x002419F0
		public SelPhotoEditCtrl.SetupParam.OnGetEquipPhoto cbGetEquipPhoto { get; set; }

		// Token: 0x17000B98 RID: 2968
		// (get) Token: 0x060050CD RID: 20685 RVA: 0x002437F9 File Offset: 0x002419F9
		// (set) Token: 0x060050CE RID: 20686 RVA: 0x00243801 File Offset: 0x00241A01
		public SelPhotoEditCtrl.SetupParam.OnGetEquipPhoto cbGetAllEquipPhoto { get; set; }

		// Token: 0x17000B99 RID: 2969
		// (get) Token: 0x060050CF RID: 20687 RVA: 0x0024380A File Offset: 0x00241A0A
		// (set) Token: 0x060050D0 RID: 20688 RVA: 0x00243812 File Offset: 0x00241A12
		public int indexByDeckChara { get; set; }

		// Token: 0x17000B9A RID: 2970
		// (get) Token: 0x060050D1 RID: 20689 RVA: 0x0024381B File Offset: 0x00241A1B
		// (set) Token: 0x060050D2 RID: 20690 RVA: 0x00243823 File Offset: 0x00241A23
		public SelPhotoEditCtrl.SetupParam.OnGetCharaPackData cbGetCharaPackData { get; set; }

		// Token: 0x17000B9B RID: 2971
		// (get) Token: 0x060050D3 RID: 20691 RVA: 0x0024382C File Offset: 0x00241A2C
		// (set) Token: 0x060050D4 RID: 20692 RVA: 0x00243834 File Offset: 0x00241A34
		public SelPhotoEditCtrl.SetupParam.OnIsEquipPhoto cbIsEquipPhoto { get; set; }

		// Token: 0x17000B9C RID: 2972
		// (get) Token: 0x060050D5 RID: 20693 RVA: 0x0024383D File Offset: 0x00241A3D
		// (set) Token: 0x060050D6 RID: 20694 RVA: 0x00243845 File Offset: 0x00241A45
		public SelPhotoEditCtrl.SetupParam.OnSetString cbSetPartyName { get; set; }

		// Token: 0x17000B9D RID: 2973
		// (get) Token: 0x060050D7 RID: 20695 RVA: 0x0024384E File Offset: 0x00241A4E
		// (set) Token: 0x060050D8 RID: 20696 RVA: 0x00243856 File Offset: 0x00241A56
		public SelPhotoEditCtrl.SetupParam.OnResignEquipPhotoByDataId cbResignEquipPhotoByDataId { get; set; }

		// Token: 0x17000B9E RID: 2974
		// (get) Token: 0x060050D9 RID: 20697 RVA: 0x0024385F File Offset: 0x00241A5F
		// (set) Token: 0x060050DA RID: 20698 RVA: 0x00243867 File Offset: 0x00241A67
		public SelPhotoEditCtrl.SetupParam.CurrentUerPartyId CurrentUserPartyId { get; set; }

		// Token: 0x17000B9F RID: 2975
		// (get) Token: 0x060050DC RID: 20700 RVA: 0x00243879 File Offset: 0x00241A79
		// (set) Token: 0x060050DB RID: 20699 RVA: 0x00243870 File Offset: 0x00241A70
		public bool isTutorial { get; set; }

		// Token: 0x17000BA0 RID: 2976
		// (get) Token: 0x060050DD RID: 20701 RVA: 0x00243881 File Offset: 0x00241A81
		// (set) Token: 0x060050DE RID: 20702 RVA: 0x00243889 File Offset: 0x00241A89
		public UserDeckData.Category deckCategory { get; set; }

		// Token: 0x17000BA1 RID: 2977
		// (get) Token: 0x060050DF RID: 20703 RVA: 0x00243892 File Offset: 0x00241A92
		// (set) Token: 0x060050E0 RID: 20704 RVA: 0x0024389A File Offset: 0x00241A9A
		public int pvpSeasonId { get; set; }

		// Token: 0x17000BA2 RID: 2978
		// (get) Token: 0x060050E2 RID: 20706 RVA: 0x002438AC File Offset: 0x00241AAC
		// (set) Token: 0x060050E1 RID: 20705 RVA: 0x002438A3 File Offset: 0x00241AA3
		public int EventId { get; set; }

		// Token: 0x17000BA3 RID: 2979
		// (get) Token: 0x060050E4 RID: 20708 RVA: 0x002438BD File Offset: 0x00241ABD
		// (set) Token: 0x060050E3 RID: 20707 RVA: 0x002438B4 File Offset: 0x00241AB4
		public int PlayQuestOneId { get; set; }

		// Token: 0x17000BA4 RID: 2980
		// (get) Token: 0x060050E6 RID: 20710 RVA: 0x002438CE File Offset: 0x00241ACE
		// (set) Token: 0x060050E5 RID: 20709 RVA: 0x002438C5 File Offset: 0x00241AC5
		public bool isHelper { get; set; }

		// Token: 0x060050E7 RID: 20711 RVA: 0x002438D8 File Offset: 0x00241AD8
		public SetupParam()
		{
			this.cbButtonArrow = null;
			this.cbButtonEditOk = null;
			this.cbGetAllEquipPhoto = null;
			this.cbGetCharaPackData = null;
			this.cbGetEquipPhoto = null;
			this.indexByDeckChara = 0;
			this.isTutorial = false;
			this.deckCategory = UserDeckData.Category.NORMAL;
			this.pvpSeasonId = 0;
			this.EventId = -1;
			this.PlayQuestOneId = -1;
			this.isHelper = false;
		}

		// Token: 0x02001217 RID: 4631
		// (Invoke) Token: 0x060057BF RID: 22463
		public delegate bool OnClickButton(PguiButtonCtrl button);

		// Token: 0x02001218 RID: 4632
		// (Invoke) Token: 0x060057C3 RID: 22467
		public delegate List<long> OnGetEquipPhoto();

		// Token: 0x02001219 RID: 4633
		// (Invoke) Token: 0x060057C7 RID: 22471
		public delegate CharaPackData OnGetCharaPackData();

		// Token: 0x0200121A RID: 4634
		// (Invoke) Token: 0x060057CB RID: 22475
		public delegate int OnIsEquipPhoto(long photoDataId);

		// Token: 0x0200121B RID: 4635
		// (Invoke) Token: 0x060057CF RID: 22479
		public delegate string OnSetString();

		// Token: 0x0200121C RID: 4636
		// (Invoke) Token: 0x060057D3 RID: 22483
		public delegate bool OnResignEquipPhotoByDataId(long dataId);

		// Token: 0x0200121D RID: 4637
		// (Invoke) Token: 0x060057D7 RID: 22487
		public delegate List<int> CurrentUerPartyId();
	}

	// Token: 0x02000FBF RID: 4031
	public class GUI
	{
		// Token: 0x060050E8 RID: 20712 RVA: 0x0024393F File Offset: 0x00241B3F
		public GUI(Transform baseTr)
		{
			this.photoDeck = new SelPhotoEditCtrl.GUI.PhotoDeck(baseTr.Find("PhotoEdit"));
			this.anim = baseTr.GetComponent<SimpleAnimation>();
		}

		// Token: 0x060050E9 RID: 20713 RVA: 0x00243974 File Offset: 0x00241B74
		public SelPhotoEditCtrl.GUI.IconPhotoSet SearchIconPhoto(SelPhotoEditCtrl.SelectPhotoData scd)
		{
			if (scd != null)
			{
				SelPhotoEditCtrl.GUI.IconPhotoSet iconPhotoSet = null;
				SelCharaDeckCtrl.FrameType type = scd.type;
				if (type != SelCharaDeckCtrl.FrameType.DECK)
				{
					if (type == SelCharaDeckCtrl.FrameType.RESERVE)
					{
						iconPhotoSet = this.reservePhotoIcon.Find((SelPhotoEditCtrl.GUI.IconPhotoSet item) => item.iconPhotoCtrl.photoPackData == scd.photo);
					}
				}
				else
				{
					SelPhotoEditCtrl.GUI.PhotoEdit photoEdit = this.photoDeck.mainIconPhotoCtrl.Find((SelPhotoEditCtrl.GUI.PhotoEdit item) => item.iconPhotoSet.iconPhotoCtrl.photoPackData == scd.photo);
					if (photoEdit != null)
					{
						iconPhotoSet = photoEdit.iconPhotoSet;
					}
				}
				return iconPhotoSet;
			}
			return null;
		}

		// Token: 0x060050EA RID: 20714 RVA: 0x002439F4 File Offset: 0x00241BF4
		public SelPhotoEditCtrl.GUI.IconPhotoSet SearchIconPhoto(SelCharaDeckCtrl.FrameType type, long id)
		{
			if (id != 0L)
			{
				SelPhotoEditCtrl.GUI.IconPhotoSet iconPhotoSet = null;
				if (type != SelCharaDeckCtrl.FrameType.DECK)
				{
					if (type == SelCharaDeckCtrl.FrameType.RESERVE)
					{
						iconPhotoSet = this.reservePhotoIcon.Find((SelPhotoEditCtrl.GUI.IconPhotoSet item) => item != null && item.iconPhotoCtrl.photoPackData != null && item.iconPhotoCtrl.photoPackData.dataId == id);
					}
				}
				else
				{
					SelPhotoEditCtrl.GUI.PhotoEdit photoEdit = this.photoDeck.mainIconPhotoCtrl.Find((SelPhotoEditCtrl.GUI.PhotoEdit item) => item.iconPhotoSet.iconPhotoCtrl.photoPackData.dataId == id);
					if (photoEdit != null)
					{
						iconPhotoSet = photoEdit.iconPhotoSet;
					}
				}
				return iconPhotoSet;
			}
			return null;
		}

		// Token: 0x060050EB RID: 20715 RVA: 0x00243A68 File Offset: 0x00241C68
		public SelPhotoEditCtrl.GUI.IconPhotoSet SearchIconPhoto(SelCharaDeckCtrl.FrameType type, int id)
		{
			if (id != 0)
			{
				SelPhotoEditCtrl.GUI.IconPhotoSet iconPhotoSet = null;
				if (type != SelCharaDeckCtrl.FrameType.DECK)
				{
					if (type == SelCharaDeckCtrl.FrameType.RESERVE)
					{
						iconPhotoSet = this.reservePhotoIcon.Find((SelPhotoEditCtrl.GUI.IconPhotoSet item) => item != null && item.iconPhotoCtrl.photoPackData != null && item.iconPhotoCtrl.photoPackData.staticData != null && item.iconPhotoCtrl.photoPackData.staticData.GetId() == id);
					}
				}
				else
				{
					SelPhotoEditCtrl.GUI.PhotoEdit photoEdit = this.photoDeck.mainIconPhotoCtrl.Find((SelPhotoEditCtrl.GUI.PhotoEdit item) => item.iconPhotoSet.iconPhotoCtrl.photoPackData.staticData.GetId() == id);
					if (photoEdit != null)
					{
						iconPhotoSet = photoEdit.iconPhotoSet;
					}
				}
				return iconPhotoSet;
			}
			return null;
		}

		// Token: 0x040058D5 RID: 22741
		public SelPhotoEditCtrl.GUI.PhotoDeck photoDeck;

		// Token: 0x040058D6 RID: 22742
		public SimpleAnimation anim;

		// Token: 0x040058D7 RID: 22743
		public List<SelPhotoEditCtrl.GUI.IconPhotoSet> reservePhotoIcon = new List<SelPhotoEditCtrl.GUI.IconPhotoSet>();

		// Token: 0x0200121E RID: 4638
		public class IconPhotoSet
		{
			// Token: 0x060057DA RID: 22490 RVA: 0x0025931C File Offset: 0x0025751C
			public IconPhotoSet(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				GameObject gameObject = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Photo, baseTr.Find("Icon_Photo"));
				this.iconPhotoCtrl = gameObject.GetComponent<IconPhotoCtrl>();
				this.iconBase = baseTr.Find("Icon_Photo").GetComponent<RectTransform>();
				this.currentFrame = baseTr.Find("Current").gameObject;
				this.currentFrame.SetActive(false);
				this.textCount = baseTr.Find("Count/Num_Count").GetComponent<PguiTextCtrl>();
				this.textCount.transform.parent.gameObject.SetActive(false);
			}

			// Token: 0x060057DB RID: 22491 RVA: 0x002593CB File Offset: 0x002575CB
			public void DispCount(bool flag, string str = null)
			{
				if (this.textCount != null)
				{
					if (str != null)
					{
						this.textCount.text = str;
					}
					this.textCount.transform.parent.gameObject.SetActive(flag);
				}
			}

			// Token: 0x060057DC RID: 22492 RVA: 0x00259405 File Offset: 0x00257605
			public void SetScale(Vector3 scaleCurrent, Vector3 scaleCount)
			{
				this.currentFrame.transform.Find("Current").localScale = scaleCurrent;
				this.textCount.transform.parent.localScale = scaleCount;
			}

			// Token: 0x04006339 RID: 25401
			public GameObject baseObj;

			// Token: 0x0400633A RID: 25402
			public RectTransform iconBase;

			// Token: 0x0400633B RID: 25403
			public IconPhotoCtrl iconPhotoCtrl;

			// Token: 0x0400633C RID: 25404
			public GameObject currentFrame;

			// Token: 0x0400633D RID: 25405
			public PguiTextCtrl textCount;
		}

		// Token: 0x0200121F RID: 4639
		public class PhotoEdit
		{
			// Token: 0x060057DD RID: 22493 RVA: 0x00259438 File Offset: 0x00257638
			public PhotoEdit(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Active = baseTr.Find("Active").GetComponent<SimpleAnimation>();
				this.Active.gameObject.SetActive(false);
				this.BaseBlank_Plus = baseTr.Find("BaseBlank_Plus").gameObject;
				this.BaseBlank_Lock = baseTr.Find("BaseBlank_Lock").gameObject;
				this.Num_Lv = baseTr.Find("Num_Lv").GetComponent<PguiTextCtrl>();
				GameObject gameObject = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_PhotoSet, baseTr.Find("Icon_Photo"));
				this.iconPhotoSet = new SelPhotoEditCtrl.GUI.IconPhotoSet(gameObject.transform);
			}

			// Token: 0x060057DE RID: 22494 RVA: 0x002594EC File Offset: 0x002576EC
			public void StartAnim()
			{
				this.Active.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, delegate
				{
					this.Active.ExPlayAnimation(SimpleAnimation.ExPguiStatus.LOOP, null);
				});
			}

			// Token: 0x0400633E RID: 25406
			public GameObject baseObj;

			// Token: 0x0400633F RID: 25407
			public SelPhotoEditCtrl.GUI.IconPhotoSet iconPhotoSet;

			// Token: 0x04006340 RID: 25408
			public SimpleAnimation Active;

			// Token: 0x04006341 RID: 25409
			public GameObject BaseBlank_Plus;

			// Token: 0x04006342 RID: 25410
			public GameObject BaseBlank_Lock;

			// Token: 0x04006343 RID: 25411
			public PguiTextCtrl Num_Lv;
		}

		// Token: 0x02001220 RID: 4640
		public class PhotoDeck
		{
			// Token: 0x060057E0 RID: 22496 RVA: 0x00259518 File Offset: 0x00257718
			public PhotoDeck(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Btn_EditOk = baseTr.Find("Btn_EditOk").GetComponent<PguiButtonCtrl>();
				this.Btn_Yaji_Left = baseTr.Find("CurrentDeck/Btn_Yaji_Left").GetComponent<PguiButtonCtrl>();
				this.Btn_Yaji_Right = baseTr.Find("CurrentDeck/Btn_Yaji_Right").GetComponent<PguiButtonCtrl>();
				this.Btn_FilterOnOff = baseTr.Find("PhotoAll/SortFilterBtnsAll/Btn_FilterOnOff").GetComponent<PguiButtonCtrl>();
				this.Btn_Sort = baseTr.Find("PhotoAll/SortFilterBtnsAll/Btn_Sort").GetComponent<PguiButtonCtrl>();
				this.Btn_SortUpDown = baseTr.Find("PhotoAll/SortFilterBtnsAll/Btn_SortUpDown").GetComponent<PguiButtonCtrl>();
				this.Txt_Party = baseTr.Find("CurrentDeck/Txt_Party").GetComponent<PguiTextCtrl>();
				this.Txt_SkillInfo = baseTr.Find("PhotoInfo/Txt_SkillInfo").GetComponent<PguiTextCtrl>();
				this.ScrollView = baseTr.Find("PhotoAll/ScrollView").GetComponent<ReuseScroll>();
				this.Num_PhotoPocketLv = baseTr.Find("PhotoInfo/Num_PhotoPocketLv").GetComponent<PguiTextCtrl>();
				this.parameters = new List<SelPhotoEditCtrl.GUI.PhotoDeck.Parameter>
				{
					new SelPhotoEditCtrl.GUI.PhotoDeck.Parameter(baseTr.Find("PhotoInfo/Param01")),
					new SelPhotoEditCtrl.GUI.PhotoDeck.Parameter(baseTr.Find("PhotoInfo/Param02")),
					new SelPhotoEditCtrl.GUI.PhotoDeck.Parameter(baseTr.Find("PhotoInfo/Param03"))
				};
				this.mainIconCharaCtrl = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Chara, baseTr.Find("CurrentDeck/Icon_Chara")).GetComponent<IconCharaCtrl>();
				for (int i = 0; i < 4; i++)
				{
					this.mainIconPhotoCtrl.Add(new SelPhotoEditCtrl.GUI.PhotoEdit(baseTr.Find("CurrentDeck/Icon_Photo0" + (i + 1).ToString())));
				}
				this.infoIconPhotoCtrl = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Photo, baseTr.Find("PhotoInfo/Icon_Photo")).GetComponent<IconPhotoCtrl>();
				this.sizeChangeBtnGUI = new PhotoUtil.SizeChangeBtnGUI(baseTr.Find("PhotoAll/SortFilterBtnsAll/Btn_SizeChange"));
				this.infoPhotoItemEffectCtrl = baseTr.Find("PhotoInfo/Info_PhotoEffect").GetComponent<InfoPhotoItemEffectCtrl>();
				this.Txt_None = baseTr.Find("PhotoAll/Txt_None").gameObject;
				this.Txt_None.SetActive(false);
			}

			// Token: 0x060057E1 RID: 22497 RVA: 0x0025973A File Offset: 0x0025793A
			public void ResizeScrollView(int count, int resize)
			{
				this.Txt_None.SetActive(count <= 0);
				this.ScrollView.Resize(resize, 0);
			}

			// Token: 0x060057E2 RID: 22498 RVA: 0x0025975C File Offset: 0x0025795C
			public void SetPhotoInfo(SelPhotoEditCtrl.GUI.PhotoDeck.SetupPhotoInfoParam setupPhotoInfoParam)
			{
				bool flag = setupPhotoInfoParam.ppd != null && !setupPhotoInfoParam.ppd.IsInvalid();
				int selectIndex = setupPhotoInfoParam.selectIndex;
				bool flag2 = selectIndex >= 0 && setupPhotoInfoParam.cpd != null && !setupPhotoInfoParam.cpd.IsInvalid();
				foreach (SelPhotoEditCtrl.GUI.PhotoDeck.Parameter parameter in this.parameters)
				{
					parameter.SetActive(flag2);
				}
				this.Num_PhotoPocketLv.gameObject.SetActive(flag2);
				this.Txt_SkillInfo.gameObject.SetActive(flag);
				this.infoIconPhotoCtrl.gameObject.SetActive(flag);
				PrjUtil.ParamPreset paramPreset = (flag ? PrjUtil.CalcParamByPhoto(setupPhotoInfoParam.ppd) : null);
				PrjUtil.ParamPreset paramPreset2 = new PrjUtil.ParamPreset();
				if (flag2 && flag)
				{
					paramPreset2 = setupPhotoInfoParam.cpd.dynamicData.PhotoPocket[selectIndex].DiffCalcPhotoParam(setupPhotoInfoParam.ppd);
				}
				SelPhotoEditCtrl.GUI.PhotoDeck.Parameter parameter2 = this.parameters[0];
				if (flag)
				{
					parameter2.baseValue.text = string.Format("{0}", paramPreset.hp);
				}
				parameter2.baseValue.gameObject.SetActive(flag);
				parameter2.addValue.gameObject.SetActive(flag && flag2);
				parameter2.addValue2.gameObject.SetActive(!flag && flag2);
				if (flag2)
				{
					parameter2.addValue2.text = "x" + setupPhotoInfoParam.cpd.dynamicData.PhotoPocket[selectIndex].HpRatio2String;
					if (flag)
					{
						parameter2.addValue.text = string.Format("+{0}", paramPreset2.hp);
						parameter2.addValue.gameObject.SetActive(paramPreset2.hp != 0);
					}
				}
				SelPhotoEditCtrl.GUI.PhotoDeck.Parameter parameter3 = this.parameters[1];
				if (flag)
				{
					parameter3.baseValue.text = string.Format("{0}", paramPreset.atk);
				}
				parameter3.baseValue.gameObject.SetActive(flag);
				parameter3.addValue.gameObject.SetActive(flag && flag2);
				parameter3.addValue2.gameObject.SetActive(!flag && flag2);
				if (flag2)
				{
					parameter3.addValue2.text = "x" + setupPhotoInfoParam.cpd.dynamicData.PhotoPocket[selectIndex].AtkRatio2String;
					if (flag)
					{
						parameter3.addValue.text = string.Format("+{0}", paramPreset2.atk);
						parameter3.addValue.gameObject.SetActive(paramPreset2.atk != 0);
					}
				}
				SelPhotoEditCtrl.GUI.PhotoDeck.Parameter parameter4 = this.parameters[2];
				if (flag)
				{
					parameter4.baseValue.text = string.Format("{0}", paramPreset.def);
				}
				parameter4.baseValue.gameObject.SetActive(flag);
				parameter4.addValue.gameObject.SetActive(flag && flag2);
				parameter4.addValue2.gameObject.SetActive(!flag && flag2);
				if (flag2)
				{
					parameter4.addValue2.text = "x" + setupPhotoInfoParam.cpd.dynamicData.PhotoPocket[selectIndex].DefRatio2String;
					if (flag)
					{
						parameter4.addValue.text = string.Format("+{0}", paramPreset2.def);
						parameter4.addValue.gameObject.SetActive(paramPreset2.def != 0);
					}
				}
				this.Num_PhotoPocketLv.gameObject.SetActive(flag2);
				if (flag2)
				{
					this.Num_PhotoPocketLv.text = ((setupPhotoInfoParam.cpd.dynamicData.PhotoPocket[selectIndex].Step <= 0) ? PhotoUtil.PhotoPocketNotReleasedText : string.Format("Lv.{0}", setupPhotoInfoParam.cpd.dynamicData.PhotoPocket[selectIndex].Step));
				}
				if (flag)
				{
					this.Txt_SkillInfo.text = ((setupPhotoInfoParam.ppd.GetCurrentAbility() != null) ? setupPhotoInfoParam.ppd.GetCurrentAbility().abilityEffect : "");
				}
				this.infoIconPhotoCtrl.Setup(setupPhotoInfoParam.ppd, SortFilterDefine.SortType.LEVEL, setupPhotoInfoParam.isEnableDetail, false, setupPhotoInfoParam.playQuestOneId, setupPhotoInfoParam.isHelper);
				if (DataManagerDeck.CheckDisableDropIcon(setupPhotoInfoParam.deckCategory, setupPhotoInfoParam.pvpSeasonId))
				{
					this.infoIconPhotoCtrl.DispDrop(false, 0);
				}
			}

			// Token: 0x04006344 RID: 25412
			public static readonly int SCROLL_ITEM_NUN_H = 5;

			// Token: 0x04006345 RID: 25413
			public GameObject baseObj;

			// Token: 0x04006346 RID: 25414
			public PguiButtonCtrl Btn_EditOk;

			// Token: 0x04006347 RID: 25415
			public PguiButtonCtrl Btn_Yaji_Left;

			// Token: 0x04006348 RID: 25416
			public PguiButtonCtrl Btn_Yaji_Right;

			// Token: 0x04006349 RID: 25417
			public PguiButtonCtrl Btn_FilterOnOff;

			// Token: 0x0400634A RID: 25418
			public PguiButtonCtrl Btn_Sort;

			// Token: 0x0400634B RID: 25419
			public PguiButtonCtrl Btn_SortUpDown;

			// Token: 0x0400634C RID: 25420
			public PguiTextCtrl Txt_Party;

			// Token: 0x0400634D RID: 25421
			public ReuseScroll ScrollView;

			// Token: 0x0400634E RID: 25422
			public IconPhotoCtrl infoIconPhotoCtrl;

			// Token: 0x0400634F RID: 25423
			public PguiTextCtrl Txt_SkillInfo;

			// Token: 0x04006350 RID: 25424
			public PguiTextCtrl Num_PhotoPocketLv;

			// Token: 0x04006351 RID: 25425
			public List<SelPhotoEditCtrl.GUI.PhotoDeck.Parameter> parameters;

			// Token: 0x04006352 RID: 25426
			public IconCharaCtrl mainIconCharaCtrl;

			// Token: 0x04006353 RID: 25427
			public List<SelPhotoEditCtrl.GUI.PhotoEdit> mainIconPhotoCtrl = new List<SelPhotoEditCtrl.GUI.PhotoEdit>();

			// Token: 0x04006354 RID: 25428
			public PhotoUtil.SizeChangeBtnGUI sizeChangeBtnGUI;

			// Token: 0x04006355 RID: 25429
			public InfoPhotoItemEffectCtrl infoPhotoItemEffectCtrl;

			// Token: 0x04006356 RID: 25430
			public GameObject Txt_None;

			// Token: 0x0200124D RID: 4685
			public class SetupPhotoInfoParam
			{
				// Token: 0x04006440 RID: 25664
				public PhotoPackData ppd;

				// Token: 0x04006441 RID: 25665
				public CharaPackData cpd;

				// Token: 0x04006442 RID: 25666
				public SortFilterDefine.SortType sortType;

				// Token: 0x04006443 RID: 25667
				public bool isEnableDetail;

				// Token: 0x04006444 RID: 25668
				public int playQuestOneId;

				// Token: 0x04006445 RID: 25669
				public int selectIndex;

				// Token: 0x04006446 RID: 25670
				public bool isHelper;

				// Token: 0x04006447 RID: 25671
				public UserDeckData.Category deckCategory;

				// Token: 0x04006448 RID: 25672
				public int pvpSeasonId;
			}

			// Token: 0x0200124E RID: 4686
			public enum ParamKind
			{
				// Token: 0x0400644A RID: 25674
				Hp,
				// Token: 0x0400644B RID: 25675
				Atk,
				// Token: 0x0400644C RID: 25676
				Def
			}

			// Token: 0x0200124F RID: 4687
			public class Parameter
			{
				// Token: 0x0600586E RID: 22638 RVA: 0x0025CFEC File Offset: 0x0025B1EC
				public Parameter(Transform baseTr)
				{
					this.baseObj = baseTr.gameObject;
					this.baseValue = baseTr.Find("Num_Param").GetComponent<PguiTextCtrl>();
					this.addValue = baseTr.Find("Num_Up").GetComponent<PguiTextCtrl>();
					this.addValue2 = baseTr.Find("Txt_Up").GetComponent<PguiTextCtrl>();
				}

				// Token: 0x0600586F RID: 22639 RVA: 0x0025D04D File Offset: 0x0025B24D
				public void SetActive(bool sw)
				{
					this.baseValue.gameObject.SetActive(sw);
					this.addValue.gameObject.SetActive(sw);
					this.addValue2.gameObject.SetActive(sw);
				}

				// Token: 0x0400644D RID: 25677
				public GameObject baseObj;

				// Token: 0x0400644E RID: 25678
				public PguiTextCtrl baseValue;

				// Token: 0x0400644F RID: 25679
				public PguiTextCtrl addValue;

				// Token: 0x04006450 RID: 25680
				public PguiTextCtrl addValue2;
			}
		}
	}
}
