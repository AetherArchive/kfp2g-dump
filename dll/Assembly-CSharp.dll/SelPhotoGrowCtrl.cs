using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using AEAuth3;
using CriWare;
using SGNFW.Common;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelPhotoGrowCtrl : MonoBehaviour
{
	public SelPhotoGrowCtrl.Mode CurrentMode { get; private set; }

	private SelPhotoGrowCtrl.PhotoGrowEffectPahse GrowEffectPahse { get; set; }

	public void Init()
	{
		this.cloneUserOptionData = DataManager.DmUserInfo.optionData.Clone();
		this.LatestRarityList = new List<ItemDef.Rarity>();
		this.LatestPhotoLevelList = new List<SortFilterDefine.PhotoLevelType>();
		this.LatestPhotoTypeList = new List<PhotoDef.Type>();
		this.guiData = new SelPhotoGrowCtrl.GUI(base.transform);
		this.guiData.photoGrowSelectWindow.SetCloseStatusCallback(new UnityAction<List<ItemDef.Rarity>, List<SortFilterDefine.PhotoLevelType>, List<PhotoDef.Type>>(this.WindowCloseStatusCallBack));
		this.guiData.photoGrowTop.sizeChangeBtnGUI.Setup(new PhotoUtil.SizeChangeBtnGUI.SetupParam
		{
			funcResult = delegate(PhotoUtil.SizeChangeBtnGUI.ResultParam result)
			{
				this.cloneUserOptionData.photoIconSizeGrow = result.sizeIndex;
				DataManager.DmUserInfo.RequestActionUpdateUserOption(this.cloneUserOptionData);
			},
			iconPhotoParamList = new List<PhotoUtil.SizeChangeBtnGUI.IconPhotoParam>
			{
				new PhotoUtil.SizeChangeBtnGUI.IconPhotoParam
				{
					scale = new Vector3(0.6f, 0.6f, 1f),
					scaleCurrent = new Vector3(0.6f, 0.6f, 1f),
					scaleCount = new Vector3(0.75f, 0.75f, 1f),
					num = 10,
					prefab = Object.Instantiate<GameObject>((GameObject)Resources.Load("SelCmn/GUI/Prefab/CharaPhotoAll_Icon_Photo_List_S"), base.transform)
				},
				new PhotoUtil.SizeChangeBtnGUI.IconPhotoParam
				{
					scale = new Vector3(0.75f, 0.75f, 1f),
					scaleCurrent = new Vector3(0.75f, 0.75f, 1f),
					scaleCount = new Vector3(0.85f, 0.85f, 1f),
					num = 8,
					prefab = Object.Instantiate<GameObject>((GameObject)Resources.Load("SelCmn/GUI/Prefab/CharaPhotoAll_Icon_Photo_List_M"), base.transform)
				},
				new PhotoUtil.SizeChangeBtnGUI.IconPhotoParam
				{
					scale = new Vector3(0.85f, 0.85f, 1f),
					scaleCurrent = new Vector3(0.85f, 0.85f, 1f),
					scaleCount = new Vector3(0.85f, 0.85f, 1f),
					num = 7,
					prefab = Object.Instantiate<GameObject>((GameObject)Resources.Load("SelCmn/GUI/Prefab/CharaPhotoAll_Icon_Photo_List_L"), base.transform)
				},
				new PhotoUtil.SizeChangeBtnGUI.IconPhotoParam
				{
					scale = new Vector3(1f, 1f, 1f),
					scaleCurrent = new Vector3(1f, 1f, 1f),
					scaleCount = new Vector3(1f, 1f, 1f),
					num = 6,
					prefab = Object.Instantiate<GameObject>((GameObject)Resources.Load("SelCmn/GUI/Prefab/CharaPhotoAll_Icon_Photo_List_XL"), base.transform)
				}
			},
			onStartItem = new Action<int, GameObject>(this.OnStartItemPhotoSelect),
			onUpdateItem = new Action<int, GameObject>(this.OnUpdateItemPhotoSelect),
			refScrollView = this.guiData.photoGrowTop.ScrollView,
			sizeIndex = this.cloneUserOptionData.photoIconSizeGrow,
			dispIconPhotoCountCallback = () => this.dispPhotoPackList.Count,
			resetCallback = delegate
			{
				this.guiData.topSelectPhotoIcon.Clear();
			}
		});
		this.guiData.photoGrowMain.sizeChangeBtnGUI.Setup(new PhotoUtil.SizeChangeBtnGUI.SetupParam
		{
			funcResult = delegate(PhotoUtil.SizeChangeBtnGUI.ResultParam result)
			{
				this.cloneUserOptionData.photoIconSizeSelectMaterial = result.sizeIndex;
				DataManager.DmUserInfo.RequestActionUpdateUserOption(this.cloneUserOptionData);
			},
			iconPhotoParamList = new List<PhotoUtil.SizeChangeBtnGUI.IconPhotoParam>
			{
				new PhotoUtil.SizeChangeBtnGUI.IconPhotoParam
				{
					scale = new Vector3(0.52f, 0.52f, 1f),
					scaleCurrent = new Vector3(0.85f, 0.85f, 1f),
					scaleCount = new Vector3(0.85f, 0.85f, 1f),
					num = 8,
					prefab = Object.Instantiate<GameObject>((GameObject)Resources.Load("SelCmn/GUI/Prefab/PhotoGrow_Icon_Photo_List_S"), base.transform)
				},
				new PhotoUtil.SizeChangeBtnGUI.IconPhotoParam
				{
					scale = new Vector3(0.6f, 0.6f, 1f),
					scaleCurrent = new Vector3(1f, 1f, 1f),
					scaleCount = new Vector3(1f, 1f, 1f),
					num = 7,
					prefab = Object.Instantiate<GameObject>((GameObject)Resources.Load("SelCmn/GUI/Prefab/PhotoGrow_Icon_Photo_List_M"), base.transform)
				},
				new PhotoUtil.SizeChangeBtnGUI.IconPhotoParam
				{
					scale = new Vector3(0.7f, 0.7f, 1f),
					scaleCurrent = new Vector3(1f, 1f, 1f),
					scaleCount = new Vector3(1f, 1f, 1f),
					num = 6,
					prefab = Object.Instantiate<GameObject>((GameObject)Resources.Load("SelCmn/GUI/Prefab/PhotoGrow_Icon_Photo_List_L"), base.transform)
				},
				new PhotoUtil.SizeChangeBtnGUI.IconPhotoParam
				{
					scale = new Vector3(1f, 1f, 1f),
					scaleCurrent = new Vector3(1f, 1f, 1f),
					scaleCount = new Vector3(1f, 1f, 1f),
					num = 4,
					prefab = Object.Instantiate<GameObject>((GameObject)Resources.Load("SelCmn/GUI/Prefab/PhotoGrow_Icon_Photo_List_XL"), base.transform)
				}
			},
			onStartItem = new Action<int, GameObject>(this.OnStartItemMainFeed),
			onUpdateItem = new Action<int, GameObject>(this.OnUpdateItemMainFeed),
			refScrollView = this.guiData.photoGrowMain.ScrollView,
			sizeIndex = this.cloneUserOptionData.photoIconSizeSelectMaterial,
			resetCallback = delegate
			{
				this.guiData.reservePhotoIcon.Clear();
			},
			dispIconPhotoCountCallback = () => this.dispAllPhotoPackList.Count
		});
		this.guiData.photoGrowMain.baseIconPhotoCtrl.AddOnClickListener(delegate(IconPhotoCtrl x)
		{
			SoundManager.Play("prd_se_click", false, false);
			this.ReturnPhotoSelect();
		});
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this.guiData.photoGrowConfirmWindow.owCtrl.transform, true);
		this.guiData.photoGrowConfirmWindow.owCtrl.transform.SetParent(Singleton<CanvasManager>.Instance.SystemPanel.transform);
		this.guiData.photoGrowConfirmWindow.owCtrl.transform.SetSiblingIndex(Singleton<CanvasManager>.Instance.cmnTouchMask.transform.GetSiblingIndex());
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this.guiData.photoGrowSelectWindow.windowGUI.baseWindow.transform, true);
		this.guiData.photoGrowSelectWindow.windowGUI.baseWindow.transform.SetParent(Singleton<CanvasManager>.Instance.SystemPanel.transform);
		this.guiData.photoGrowSelectWindow.windowGUI.baseWindow.transform.SetSiblingIndex(Singleton<CanvasManager>.Instance.cmnTouchMask.transform.GetSiblingIndex());
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this.guiData.photoCharacteristicWindow.owCtrl.transform, true);
		this.guiData.photoCharacteristicWindow.owCtrl.transform.SetParent(Singleton<CanvasManager>.Instance.SystemPanel.transform);
		this.guiData.photoCharacteristicWindow.owCtrl.transform.SetSiblingIndex(Singleton<CanvasManager>.Instance.cmnTouchMask.transform.GetSiblingIndex());
		SortWindowCtrl.RegisterData registerData = new SortWindowCtrl.RegisterData
		{
			register = SortFilterDefine.RegisterType.PHOTO_GROW_TOP,
			filterButton = this.guiData.photoGrowTop.Btn_FilterOnOff,
			sortButton = this.guiData.photoGrowTop.Btn_Sort,
			sortUdButton = this.guiData.photoGrowTop.Btn_SortUpDown,
			funcGetTargetBaseList = delegate
			{
				List<PhotoPackData> list = new List<PhotoPackData>();
				List<PhotoPackData> list2 = new List<PhotoPackData>();
				foreach (PhotoPackData photoPackData in this.haveAllPhotoPackList)
				{
					if (photoPackData.staticData.baseData.isForbiddenGrowBase)
					{
						list.Add(photoPackData);
						list2.Remove(photoPackData);
					}
					else
					{
						list2.Add(photoPackData);
					}
				}
				return new SortWindowCtrl.SortTarget
				{
					photoList = list2,
					lowerDisableSortPhotoList = list
				};
			},
			funcDisideTarget = delegate(SortWindowCtrl.SortTarget item)
			{
				this.dispPhotoPackList = item.photoList;
				this.sortTypeTop = item.sortType;
				this.guiData.photoGrowTop.ResizeScrollView(this.dispPhotoPackList.Count, this.dispPhotoPackList.Count / this.guiData.photoGrowTop.sizeChangeBtnGUI.IconPhotoParamList[this.guiData.photoGrowTop.sizeChangeBtnGUI.SizeIndex].num);
			}
		};
		CanvasManager.HdlOpenWindowSortFilter.Register(registerData, true, null);
		this.guiData.photoGrowMain.ButtonL.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.photoGrowMain.ButtonC.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.photoGrowMain.Btn_Info.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.photoGrowMain.ButtonR.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickPhotoGrowExecuteButton), PguiButtonCtrl.SoundType.DEFAULT);
		SortWindowCtrl.RegisterData registerData2 = new SortWindowCtrl.RegisterData
		{
			register = SortFilterDefine.RegisterType.PHOTO_GROW_MAIN,
			filterButton = this.guiData.photoGrowMain.Btn_FilterOnOff,
			sortButton = this.guiData.photoGrowMain.Btn_Sort,
			sortUdButton = this.guiData.photoGrowMain.Btn_SortUpDown,
			funcGetTargetBaseList = () => this.SortTargetPhotoMaterial(),
			funcDisideTarget = delegate(SortWindowCtrl.SortTarget item)
			{
				this.dispAllPhotoPackList = item.photoList;
				this.dispAllPhotoPackList.Remove(this.basePhotoPackData);
				this.sortTypeMain = item.sortType;
				this.guiData.photoGrowMain.ResizeScrollView(this.dispAllPhotoPackList.Count, this.dispAllPhotoPackList.Count / this.guiData.photoGrowMain.sizeChangeBtnGUI.IconPhotoParamList[this.guiData.photoGrowMain.sizeChangeBtnGUI.SizeIndex].num);
			}
		};
		CanvasManager.HdlOpenWindowSortFilter.Register(registerData2, true, null);
		this.guiData.photoGrowConfirmWindow.ScrollView.InitForce();
		ReuseScroll scrollView = this.guiData.photoGrowConfirmWindow.ScrollView;
		scrollView.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollView.onStartItem, new Action<int, GameObject>(this.OnStartItemWindowPhotoGrow));
		ReuseScroll scrollView2 = this.guiData.photoGrowConfirmWindow.ScrollView;
		scrollView2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollView2.onUpdateItem, new Action<int, GameObject>(this.OnUpdateItemWindowPhotoGrow));
		this.guiData.photoGrowConfirmWindow.ScrollView.Setup(10, 0);
	}

	private SortWindowCtrl.SortTarget SortTargetPhotoMaterial()
	{
		List<PhotoPackData> list = new List<PhotoPackData>();
		List<PhotoPackData> list2 = this.UnSelectableMaterialPhotoList(this.haveAllPhotoPackList, ref list);
		return new SortWindowCtrl.SortTarget
		{
			photoList = list,
			disableFilterPhotoList = this.feedPhotoPackList,
			lowerDisableSortPhotoList = list2,
			basePhotoPackData = this.basePhotoPackData
		};
	}

	public void Setup(SelPhotoGrowCtrl.SetupParam param)
	{
		this.setupParam = param;
		this.GrowEffectPahse = SelPhotoGrowCtrl.PhotoGrowEffectPahse.None;
		this.requestMode = SelPhotoGrowCtrl.Mode.PHOTO_SELECT;
		this.CurrentMode = SelPhotoGrowCtrl.Mode.INVALID;
		this.cloneUserOptionData = DataManager.DmUserInfo.optionData.Clone();
		this.guiData.photoGrowMain.baseObj.SetActive(false);
		this.guiData.photoGrowTop.baseObj.SetActive(false);
		this.ReloadDataManager();
		this.ExecSort();
		this.guiData.photoGrowMain.InitializeCampaignInfo();
		this.guiData.photoGrowTop.InitializeCampaignInfo();
		this.guiData.photoGrowTop.sizeChangeBtnGUI.ResetScrollView();
		this.guiData.photoGrowMain.sizeChangeBtnGUI.ResetScrollView();
	}

	private void SetupBasePhotoPackData(PhotoPackData ppd)
	{
		this.basePhotoPackData = ppd;
		this.requestMode = SelPhotoGrowCtrl.Mode.GROW_MAIN;
	}

	public void SetupBySceneForce(long photoId)
	{
		this.SetupBasePhotoPackData(DataManager.DmPhoto.GetUserPhotoData(photoId));
		this.CurrentMode = SelPhotoGrowCtrl.Mode.PHOTO_SELECT;
	}

	public void Dest()
	{
		this.ClearFeedPhotoPackList();
	}

	public void ReloadDataManager()
	{
		this.<ReloadDataManager>g__SetupPhotoList|48_0();
		this.ReloadDeckPhotoIdList();
	}

	private void ReloadDeckPhotoIdList()
	{
		this.deckPhotoDataIdList.Clear();
		foreach (UserDeckData userDeckData in DataManager.DmDeck.GetUserDeckList(UserDeckData.Category.NORMAL))
		{
			foreach (List<long> list in userDeckData.equipPhotoList)
			{
				foreach (long num in list)
				{
					if (0L < num && !this.deckPhotoDataIdList.Contains(num))
					{
						this.deckPhotoDataIdList.Add(num);
					}
				}
			}
		}
		foreach (UserDeckData userDeckData2 in DataManager.DmDeck.GetUserDeckList(UserDeckData.Category.PVP))
		{
			foreach (List<long> list2 in userDeckData2.equipPhotoList)
			{
				foreach (long num2 in list2)
				{
					if (0L < num2 && !this.deckPhotoDataIdList.Contains(num2))
					{
						this.deckPhotoDataIdList.Add(num2);
					}
				}
			}
		}
		foreach (UserDeckData userDeckData3 in DataManager.DmDeck.GetUserDeckList(UserDeckData.Category.TRAINING))
		{
			foreach (List<long> list3 in userDeckData3.equipPhotoList)
			{
				foreach (long num3 in list3)
				{
					if (0L < num3 && !this.deckPhotoDataIdList.Contains(num3))
					{
						this.deckPhotoDataIdList.Add(num3);
					}
				}
			}
		}
		foreach (LoanPackData loanPackData in DataManager.DmUserInfo.loanPackList)
		{
			foreach (long num4 in loanPackData.photoDataIdList)
			{
				if (0L < num4 && !this.deckPhotoDataIdList.Contains(num4))
				{
					this.deckPhotoDataIdList.Add(num4);
				}
			}
		}
	}

	private void ExecSort()
	{
		CanvasManager.HdlOpenWindowSortFilter.SolutionList(SortFilterDefine.RegisterType.PHOTO_GROW_TOP, null);
		CanvasManager.HdlOpenWindowSortFilter.SolutionList(SortFilterDefine.RegisterType.PHOTO_GROW_MAIN, null);
	}

	private void OnClickPanel()
	{
		if (this.CurrentMode == SelPhotoGrowCtrl.Mode.GROW_EFFECT)
		{
			SelPhotoGrowCtrl.PhotoGrowEffectPahse growEffectPahse = this.GrowEffectPahse;
			if (growEffectPahse == SelPhotoGrowCtrl.PhotoGrowEffectPahse.GageUp)
			{
				this.GrowEffectPahse = SelPhotoGrowCtrl.PhotoGrowEffectPahse.GrowResult;
				return;
			}
			if (growEffectPahse != SelPhotoGrowCtrl.PhotoGrowEffectPahse.TouchWait)
			{
				return;
			}
			this.GrowEffectPahse = SelPhotoGrowCtrl.PhotoGrowEffectPahse.Refresh;
		}
	}

	private void OnClickButton(PguiButtonCtrl button)
	{
		if (this.CurrentMode != SelPhotoGrowCtrl.Mode.GROW_MAIN)
		{
			return;
		}
		if (button == this.guiData.photoGrowMain.ButtonL)
		{
			this.ClearFeedPhotoPackList();
			this.SettingGrowMainBySelectInfo();
			this.guiData.photoGrowMain.ScrollView.Refresh();
			return;
		}
		if (button == this.guiData.photoGrowMain.ButtonC)
		{
			this.guiData.photoGrowSelectWindow.windowGUI.SelectSetup(this.LatestRarityList, this.LatestPhotoLevelList, this.LatestPhotoTypeList);
			this.guiData.photoGrowSelectWindow.windowGUI.baseWindow.Open();
			return;
		}
		if (button == this.guiData.photoGrowMain.Btn_Info)
		{
			this.guiData.photoCharacteristicWindow.owCtrl.Setup(null, null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int index) => true, null, false);
			this.guiData.photoCharacteristicWindow.owCtrl.Open();
			this.guiData.photoCharacteristicWindow.Txt_PhotoName_After.text = (this.guiData.photoCharacteristicWindow.Txt_PhotoName_Before.text = this.basePhotoPackData.staticData.baseData.photoName);
			this.guiData.photoCharacteristicWindow.Txt_Info_After.text = ((this.basePhotoPackData.GetSwitchAbility(true) != null) ? this.basePhotoPackData.GetSwitchAbility(true).abilityEffect : "");
			this.guiData.photoCharacteristicWindow.Txt_Info_Before.text = ((this.basePhotoPackData.GetSwitchAbility(false) != null) ? this.basePhotoPackData.GetSwitchAbility(false).abilityEffect : "");
		}
	}

	private void OnClickPhotoGrowExecuteButton(PguiButtonCtrl button)
	{
		if (this.CurrentMode != SelPhotoGrowCtrl.Mode.GROW_MAIN)
		{
			return;
		}
		this.guiData.photoGrowConfirmWindow.owCtrl.Setup("フォト強化", "フォトの強化を行いますか？", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, new PguiOpenWindowCtrl.Callback(this.OnSelectOpenWindowButtonCallback), null, false);
		string text = string.Empty;
		bool flag = false;
		foreach (PhotoPackData photoPackData in this.feedPhotoPackList)
		{
			if (ItemDef.Rarity.STAR3 <= photoPackData.staticData.GetRarity())
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			if (text != string.Empty)
			{
				text += "\n";
			}
			text += "※★3以上のフォトが含まれています※";
		}
		bool flag2 = false;
		foreach (PhotoPackData photoPackData2 in this.feedPhotoPackList)
		{
			if (photoPackData2.dynamicData.level != 1 || photoPackData2.dynamicData.exp != 0L)
			{
				flag2 = true;
				break;
			}
		}
		if (flag2)
		{
			if (text != string.Empty)
			{
				text += "\n";
			}
			text += "※成長済のフォトが含まれています※";
		}
		this.guiData.photoGrowConfirmWindow.MassageCautionText.text = text;
		this._feedPhotoPackList = new List<SelPhotoGrowCtrl.FeedPhotoPack>();
		foreach (PhotoPackData photoPackData3 in this.feedPhotoPackList)
		{
			if (this.CanLevelLimitOver(photoPackData3))
			{
				int count = this._feedPhotoPackList.Count;
				int num = 4 - this.guiData.photoGrowMain.baseIconPhotoCtrl.photoPackData.dynamicData.levelRank;
				this._feedPhotoPackList.Add(new SelPhotoGrowCtrl.FeedPhotoPack
				{
					data = photoPackData3,
					IsDispOverlimits = (count < num)
				});
			}
		}
		DataManagerPhoto.PhotoLevelupResult photoLevelupResult = PhotoUtil.CalcPhotoGrow(this.basePhotoPackData, this.feedPhotoPackList);
		bool flag3 = 0 < photoLevelupResult.BonusRrityList.Count;
		this.guiData.photoGrowConfirmWindow.RightFukidasiBase.SetActive(flag3);
		bool flag4 = this.feedPhotoPackList.Count % SelPhotoGrowCtrl.PhotoGrowConfirmWindow.SCROLL_ITEM_NUN_H == 0;
		if (flag3)
		{
			string text2 = string.Empty;
			foreach (ItemDef.Rarity rarity in photoLevelupResult.BonusRrityList)
			{
				text2 += string.Format("☆{0}", (int)rarity);
			}
			this.guiData.photoGrowConfirmWindow.RightFukidasiText.ReplaceTextByDefault("Param01", text2);
		}
		bool flag5 = false;
		this.guiData.photoGrowConfirmWindow.LeftFukidasiBase.SetActive(flag5);
		int num2 = this.feedPhotoPackList.Count / SelPhotoGrowCtrl.PhotoGrowConfirmWindow.SCROLL_ITEM_NUN_H;
		int num3 = (flag4 ? num2 : (num2 + 1));
		this.guiData.photoGrowConfirmWindow.ScrollView.Resize(num3, 0);
		this.guiData.photoGrowConfirmWindow.Num_CoinOwn.text = DataManager.DmItem.GetUserItemData(30101).num.ToString();
		this.guiData.photoGrowConfirmWindow.Num_CoinUse.text = this.feedPhotoPackList.Sum<PhotoPackData>((PhotoPackData item) => item.staticData.rarityData.strengConsCoin).ToString();
		this.guiData.photoGrowConfirmWindow.owCtrl.Open();
		this.requestMode = SelPhotoGrowCtrl.Mode.OW_CONFIRM;
	}

	private bool OnSelectOpenWindowButtonCallback(int index)
	{
		if (this.CurrentMode == SelPhotoGrowCtrl.Mode.OW_CONFIRM)
		{
			if (index == 1)
			{
				CanvasManager.HdlMissionProgressCtrl.IsPhotoGrow = true;
				this.serverRequestGrow = this.ServerRequestGrow(this.basePhotoPackData, this.feedPhotoPackList);
				this.guiData.photoGrowMain.SelCmn_AllInOut.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
				{
					this.guiData.photoGrowMain.baseObj.SetActive(false);
				});
				this.requestMode = SelPhotoGrowCtrl.Mode.SERVER_REQUEST_GROW;
			}
			else
			{
				this.requestMode = SelPhotoGrowCtrl.Mode.GROW_MAIN;
			}
		}
		return true;
	}

	private void OnUpdatePhotoLock(IconPhotoCtrl iconPhoto)
	{
		if (iconPhoto.photoPackData.dynamicData.lockFlag && this.feedPhotoPackList.Contains(iconPhoto.photoPackData))
		{
			this.OnTouchSelectMaterialPhotoIcon(iconPhoto);
		}
		this.guiData.photoGrowMain.ScrollView.Refresh();
	}

	private void OnUpdatePhotoFavorite(IconPhotoCtrl iconPhoto)
	{
		if (iconPhoto.photoPackData.dynamicData.favoriteFlag && this.feedPhotoPackList.Contains(iconPhoto.photoPackData))
		{
			this.OnTouchSelectMaterialPhotoIcon(iconPhoto);
		}
		this.guiData.photoGrowMain.ScrollView.Refresh();
	}

	private void OnTouchSelectBasePhotoIcon(IconPhotoCtrl iconPhoto)
	{
		SoundManager.Play("prd_se_click", false, false);
		if (this.CurrentMode == SelPhotoGrowCtrl.Mode.PHOTO_SELECT && iconPhoto.photoPackData != null && !iconPhoto.photoPackData.IsInvalid())
		{
			if (iconPhoto.CheckImgDisable())
			{
				return;
			}
			this.SetupBasePhotoPackData(iconPhoto.photoPackData);
		}
	}

	private void OnTouchSelectMaterialPhotoIcon(IconPhotoCtrl iconPhoto)
	{
		SoundManager.Play("prd_se_click", false, false);
		if (this.CurrentMode == SelPhotoGrowCtrl.Mode.GROW_MAIN && iconPhoto.photoPackData != null && !iconPhoto.photoPackData.IsInvalid())
		{
			bool flag = false;
			if (this.basePhotoPackData != iconPhoto.photoPackData)
			{
				if (this.deckPhotoDataIdList.Contains(iconPhoto.photoPackData.dataId))
				{
					if (!iconPhoto.CheckImgDisable())
					{
						PhotoUtil.OpenWindowConfirmReleasePhotoDeck(iconPhoto, this.RequestUpdatePhoto(iconPhoto.photoPackData));
					}
				}
				else if (!iconPhoto.photoPackData.staticData.baseData.forbiddenDiscardFlg || this.basePhotoPackData.staticData.GetId() == iconPhoto.photoPackData.staticData.GetId())
				{
					if (iconPhoto.photoPackData.dynamicData.lockFlag)
					{
						if (this.feedPhotoPackList.Contains(iconPhoto.photoPackData))
						{
							this.feedPhotoPackList.Remove(iconPhoto.photoPackData);
							flag = true;
						}
					}
					else if (iconPhoto.photoPackData.dynamicData.favoriteFlag)
					{
						if (this.feedPhotoPackList.Contains(iconPhoto.photoPackData))
						{
							this.feedPhotoPackList.Remove(iconPhoto.photoPackData);
							flag = true;
						}
					}
					else if (!iconPhoto.CheckImgDisable())
					{
						if (this.feedPhotoPackList.Contains(iconPhoto.photoPackData))
						{
							this.feedPhotoPackList.Remove(iconPhoto.photoPackData);
							flag = true;
						}
						else
						{
							this.feedPhotoPackList.Add(iconPhoto.photoPackData);
							flag = true;
						}
					}
				}
			}
			if (flag)
			{
				foreach (KeyValuePair<GameObject, SelPhotoEditCtrl.GUI.IconPhotoSet> keyValuePair in this.guiData.reservePhotoIcon)
				{
					int num = this.feedPhotoPackList.IndexOf(keyValuePair.Value.iconPhotoCtrl.photoPackData);
					if (0 <= num)
					{
						keyValuePair.Value.currentFrame.SetActive(true);
						keyValuePair.Value.DispCount(true, (num + 1).ToString());
					}
					else
					{
						keyValuePair.Value.currentFrame.SetActive(false);
						keyValuePair.Value.DispCount(false, null);
					}
				}
				bool flag2 = this.simulateGrowLvMax;
				bool simulateGrowBreakLimitMax = this.SimulateGrowBreakLimitMax;
				this.SettingGrowMainBySelectInfo();
				if (flag2 != this.simulateGrowLvMax || simulateGrowBreakLimitMax != this.SimulateGrowBreakLimitMax)
				{
					this.guiData.photoGrowMain.ScrollView.Refresh();
				}
			}
		}
	}

	private IEnumerator RequestUpdatePhoto(PhotoPackData photoPackData)
	{
		DataManager.DmPhoto.RequestActionPhotoRelease(photoPackData.dataId);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		if ((!photoPackData.staticData.baseData.forbiddenDiscardFlg || this.basePhotoPackData.staticData.GetId() == photoPackData.staticData.GetId()) && !photoPackData.dynamicData.lockFlag && !photoPackData.dynamicData.favoriteFlag)
		{
			this.feedPhotoPackList.Add(photoPackData);
		}
		this.ReloadDataManager();
		this.ExecSort();
		this.SettingGrowMainBySelectInfo();
		this.guiData.photoGrowMain.ScrollView.Refresh();
		yield break;
	}

	private void SettingGrowMainBySelectInfo()
	{
		DataManagerPhoto.PhotoLevelupResult photoLevelupResult = PhotoUtil.CalcPhotoGrow(this.basePhotoPackData, this.feedPhotoPackList);
		int id = DataManager.DmPhoto.GetUserPhotoData(photoLevelupResult.photoDataId).staticData.GetId();
		PrjUtil.ParamPreset paramPreset = PrjUtil.CalcParamByPhoto(DataManager.DmPhoto.GetPhotoStaticData(id), photoLevelupResult.befLevel);
		PrjUtil.ParamPreset paramPreset2 = PrjUtil.CalcParamByPhoto(DataManager.DmPhoto.GetPhotoStaticData(id), photoLevelupResult.level);
		List<int> list = new List<int> { paramPreset.hp, paramPreset.atk, paramPreset.def };
		List<int> list2 = new List<int> { paramPreset2.hp, paramPreset2.atk, paramPreset2.def };
		this.simulateGrowLvMax = photoLevelupResult.level >= photoLevelupResult.limitLevel;
		this.SimulateGrowBreakLimitMax = photoLevelupResult.levelRank >= 4;
		this.guiData.photoGrowMain.Num_Lv_Before.ReplaceTextByDefault("Param01", this.basePhotoPackData.dynamicData.level.ToString() + PrjUtil.MakeMessage("/") + this.basePhotoPackData.limitLevel.ToString());
		this.guiData.photoGrowMain.Num_Lv_After.ReplaceTextByDefault(new string[] { "Param01", "Param02", "Param03", "Param04" }, new string[]
		{
			photoLevelupResult.level.ToString(),
			photoLevelupResult.limitLevel.ToString(),
			(photoLevelupResult.level > this.basePhotoPackData.dynamicData.level) ? SelPhotoGrowCtrl.UP_PARAM_COLOR_CODE : SelPhotoGrowCtrl.NORMAL_PARAM_COLOR_CODE,
			(photoLevelupResult.limitLevel > this.basePhotoPackData.limitLevel) ? SelPhotoGrowCtrl.UP_PARAM_COLOR_CODE : SelPhotoGrowCtrl.NORMAL_PARAM_COLOR_CODE
		});
		int num = this.feedPhotoPackList.Sum<PhotoPackData>((PhotoPackData item) => item.staticData.rarityData.strengConsCoin);
		this.guiData.photoGrowMain.Num_Coin.text = num.ToString();
		for (int i = 0; i < 3; i++)
		{
			this.guiData.photoGrowMain.Num_Before[i].text = list[i].ToString();
			this.guiData.photoGrowMain.Num_After[i].text = list2[i].ToString();
			this.guiData.photoGrowMain.Num_After[i].m_Text.color = ((photoLevelupResult.level > this.basePhotoPackData.dynamicData.level) ? SelPhotoGrowCtrl.UP_PARAM_COLOR : SelPhotoGrowCtrl.NORMAL_PARAM_COLOR);
		}
		if (photoLevelupResult.level == this.basePhotoPackData.dynamicData.level)
		{
			this.guiData.photoGrowMain.Gage.gameObject.SetActive(true);
			this.guiData.photoGrowMain.Gage.m_Image.fillAmount = (float)this.basePhotoPackData.dynamicData.exp / (float)DataManager.DmPhoto.GetExpByNextLevel(this.basePhotoPackData);
		}
		else
		{
			this.guiData.photoGrowMain.Gage.gameObject.SetActive(false);
		}
		if (this.feedPhotoPackList.Count > 0)
		{
			this.guiData.photoGrowMain.Gage_Up.gameObject.SetActive(true);
			this.guiData.photoGrowMain.Gage_Up.m_Image.fillAmount = (float)photoLevelupResult.exp / (float)DataManager.DmPhoto.GetExpByNextLevel(photoLevelupResult, false);
		}
		else
		{
			this.guiData.photoGrowMain.Gage_Up.gameObject.SetActive(false);
		}
		long num2 = DataManager.DmPhoto.GetExpByNextLevel(photoLevelupResult, false) - photoLevelupResult.exp;
		this.guiData.photoGrowMain.Num_Exp_Next.text = "次のLvまで" + num2.ToString();
		for (int j = 0; j < this.guiData.photoGrowMain.RebirthIcon_Before.Count; j++)
		{
			this.guiData.photoGrowMain.RebirthIcon_Before[j].SetActive(j < this.basePhotoPackData.dynamicData.levelRank);
			this.guiData.photoGrowMain.RebirthIcon_After[j].SetActive(j < photoLevelupResult.levelRank);
		}
		bool flag = this.feedPhotoPackList.Count > 0;
		this.guiData.photoGrowMain.ButtonL.SetActEnable(flag, false, false);
		bool flag2 = (photoLevelupResult.befExp != photoLevelupResult.exp || photoLevelupResult.befLevel != photoLevelupResult.level || photoLevelupResult.befLevelRank != photoLevelupResult.levelRank || photoLevelupResult.befLimitLevel != photoLevelupResult.limitLevel) && 0 < this.feedPhotoPackList.Count && num <= DataManager.DmItem.GetUserItemData(30101).num;
		this.guiData.photoGrowMain.ButtonR.SetActEnable(flag2, false, false);
		this.guiData.photoGrowMain.Num_SelPhoto.text = string.Format("{0}", this.feedPhotoPackList.Count);
		this.guiData.photoGrowMain.OwnCoin.text = DataManager.DmItem.GetUserItemData(30101).num.ToString();
		this.guiData.photoGrowMain.OwnCoin.m_Text.color = ((num > DataManager.DmItem.GetUserItemData(30101).num) ? Color.red : Color.white);
		this.optionPhotoPacks.Clear();
		foreach (PhotoPackData photoPackData in this.feedPhotoPackList)
		{
			if (this.CanLevelLimitOver(photoPackData) && 4 - this.basePhotoPackData.dynamicData.levelRank > this.optionPhotoPacks.Count)
			{
				this.optionPhotoPacks.Add(photoPackData);
			}
		}
		if (0 < this.optionPhotoPacks.Count)
		{
			this.optionPhotoPacks.Add(this.basePhotoPackData);
		}
		InfoPhotoItemEffectCtrl infoPhotoItemEffectCtrl = this.guiData.photoGrowMain.infoPhotoItemEffectCtrl;
		InfoPhotoItemEffectCtrl.SetupParam setupParam = new InfoPhotoItemEffectCtrl.SetupParam();
		setupParam.photoPackDatas = new List<PhotoPackData> { this.basePhotoPackData };
		setupParam.optionGameObjects = this.guiData.photoGrowMain.Num_Percents.ConvertAll<GameObject>((PguiTextCtrl item) => item.gameObject);
		setupParam.optionPhotoPackDatas = this.optionPhotoPacks;
		setupParam.optionFloatValueCB = delegate(GameObject go, float val)
		{
			PguiTextCtrl component = go.GetComponent<PguiTextCtrl>();
			if (component != null)
			{
				float num3 = val;
				component.text = "+" + num3.ToString("#.#") + "%";
			}
		};
		infoPhotoItemEffectCtrl.Setup(setupParam);
	}

	private void OnStartItemPhotoSelect(int index, GameObject go)
	{
		for (int i = 0; i < this.guiData.photoGrowTop.sizeChangeBtnGUI.IconPhotoParamList[this.guiData.photoGrowTop.sizeChangeBtnGUI.SizeIndex].num; i++)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_PhotoSet, go.transform);
			gameObject.name = i.ToString();
			SelPhotoEditCtrl.GUI.IconPhotoSet iconPhoto = new SelPhotoEditCtrl.GUI.IconPhotoSet(gameObject.transform);
			iconPhoto.iconPhotoCtrl.AddOnClickListener(delegate(IconPhotoCtrl x)
			{
				this.OnTouchSelectBasePhotoIcon(iconPhoto.iconPhotoCtrl);
			});
			iconPhoto.iconPhotoCtrl.AddOnUpdateStatus(delegate(IconPhotoCtrl x)
			{
				this.guiData.photoGrowTop.ScrollView.Refresh();
			});
			iconPhoto.iconPhotoCtrl.AddOnCloseWindow(delegate(IconPhotoCtrl x)
			{
				CanvasManager.HdlOpenWindowSortFilter.SolutionList(SortFilterDefine.RegisterType.PHOTO_GROW_TOP, null);
			});
			iconPhoto.baseObj.transform.Find("Icon_Photo").localScale = this.guiData.photoGrowTop.sizeChangeBtnGUI.IconPhotoParamList[this.guiData.photoGrowTop.sizeChangeBtnGUI.SizeIndex].scale;
			iconPhoto.SetScale(this.guiData.photoGrowTop.sizeChangeBtnGUI.IconPhotoParamList[this.guiData.photoGrowTop.sizeChangeBtnGUI.SizeIndex].scaleCurrent, this.guiData.photoGrowTop.sizeChangeBtnGUI.IconPhotoParamList[this.guiData.photoGrowTop.sizeChangeBtnGUI.SizeIndex].scaleCount);
			this.guiData.topSelectPhotoIcon.Add(iconPhoto.baseObj, iconPhoto);
		}
		go.GetComponent<GridLayoutGroup>().SetLayoutHorizontal();
	}

	private void OnUpdateItemPhotoSelect(int index, GameObject go)
	{
		for (int i = 0; i < this.guiData.photoGrowTop.sizeChangeBtnGUI.IconPhotoParamList[this.guiData.photoGrowTop.sizeChangeBtnGUI.SizeIndex].num; i++)
		{
			int num = index * this.guiData.photoGrowTop.sizeChangeBtnGUI.IconPhotoParamList[this.guiData.photoGrowTop.sizeChangeBtnGUI.SizeIndex].num + i;
			PhotoPackData photoPackData = null;
			if (num < this.dispPhotoPackList.Count)
			{
				photoPackData = this.dispPhotoPackList[num];
			}
			GameObject gameObject = go.transform.Find(i.ToString()).gameObject;
			SelPhotoEditCtrl.GUI.IconPhotoSet iconPhotoSet = this.guiData.topSelectPhotoIcon[gameObject];
			if (photoPackData != null)
			{
				iconPhotoSet.iconPhotoCtrl.Setup(photoPackData, this.sortTypeTop, true, false, -1, false);
				iconPhotoSet.iconPhotoCtrl.onReturnPhotoPackDataList = () => this.dispPhotoPackList;
				bool flag = true;
				if (photoPackData.dynamicData.level >= photoPackData.limitLevel && photoPackData.dynamicData.levelRank >= 4)
				{
					iconPhotoSet.iconPhotoCtrl.DispImgDisable(true);
					iconPhotoSet.iconPhotoCtrl.DispTextDisable(true, PrjUtil.MakeMessage("LvMAX"), null);
					flag = false;
				}
				else if (photoPackData.staticData.baseData.isForbiddenGrowBase)
				{
					iconPhotoSet.iconPhotoCtrl.DispImgDisable(true);
					iconPhotoSet.iconPhotoCtrl.DispTextDisable(true, PhotoUtil.NoSelectedText, null);
					flag = false;
				}
				if (this.deckPhotoDataIdList.Contains(photoPackData.dataId) && flag)
				{
					iconPhotoSet.iconPhotoCtrl.DispParty(true, true);
				}
			}
			else
			{
				iconPhotoSet.iconPhotoCtrl.Setup(null, SortFilterDefine.SortType.LEVEL, true, false, -1, false);
			}
		}
	}

	private void OnStartItemMainFeed(int index, GameObject go)
	{
		for (int i = 0; i < this.guiData.photoGrowMain.sizeChangeBtnGUI.IconPhotoParamList[this.guiData.photoGrowMain.sizeChangeBtnGUI.SizeIndex].num; i++)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_PhotoSet, go.transform);
			gameObject.name = i.ToString();
			SelPhotoEditCtrl.GUI.IconPhotoSet iconPhoto = new SelPhotoEditCtrl.GUI.IconPhotoSet(gameObject.transform);
			iconPhoto.iconPhotoCtrl.AddOnClickListener(delegate(IconPhotoCtrl x)
			{
				this.OnTouchSelectMaterialPhotoIcon(iconPhoto.iconPhotoCtrl);
			});
			iconPhoto.iconPhotoCtrl.AddOnUpdateLockListener(delegate(IconPhotoCtrl x)
			{
				this.OnUpdatePhotoLock(iconPhoto.iconPhotoCtrl);
			});
			iconPhoto.iconPhotoCtrl.AddOnUpdateFavoriteListener(delegate(IconPhotoCtrl x)
			{
				this.OnUpdatePhotoFavorite(iconPhoto.iconPhotoCtrl);
			});
			iconPhoto.iconPhotoCtrl.AddOnUpdateStatus(delegate(IconPhotoCtrl x)
			{
				this.guiData.photoGrowMain.ScrollView.Refresh();
			});
			iconPhoto.iconPhotoCtrl.AddOnCloseWindow(delegate(IconPhotoCtrl x)
			{
				CanvasManager.HdlOpenWindowSortFilter.SolutionList(SortFilterDefine.RegisterType.PHOTO_GROW_MAIN, null);
			});
			iconPhoto.baseObj.transform.Find("Icon_Photo").localScale = this.guiData.photoGrowMain.sizeChangeBtnGUI.IconPhotoParamList[this.guiData.photoGrowMain.sizeChangeBtnGUI.SizeIndex].scale;
			iconPhoto.SetScale(this.guiData.photoGrowMain.sizeChangeBtnGUI.IconPhotoParamList[this.guiData.photoGrowMain.sizeChangeBtnGUI.SizeIndex].scaleCurrent, this.guiData.photoGrowMain.sizeChangeBtnGUI.IconPhotoParamList[this.guiData.photoGrowMain.sizeChangeBtnGUI.SizeIndex].scaleCount);
			this.guiData.reservePhotoIcon.Add(iconPhoto.baseObj, iconPhoto);
		}
		go.GetComponent<GridLayoutGroup>().SetLayoutHorizontal();
	}

	private void OnUpdateItemMainFeed(int index, GameObject go)
	{
		for (int i = 0; i < this.guiData.photoGrowMain.sizeChangeBtnGUI.IconPhotoParamList[this.guiData.photoGrowMain.sizeChangeBtnGUI.SizeIndex].num; i++)
		{
			int num = index * this.guiData.photoGrowMain.sizeChangeBtnGUI.IconPhotoParamList[this.guiData.photoGrowMain.sizeChangeBtnGUI.SizeIndex].num + i;
			PhotoPackData ppd = null;
			if (num < this.dispAllPhotoPackList.Count)
			{
				ppd = this.dispAllPhotoPackList[num];
			}
			GameObject gameObject = go.transform.Find(i.ToString()).gameObject;
			SelPhotoEditCtrl.GUI.IconPhotoSet iconPhotoSet = this.guiData.reservePhotoIcon[gameObject];
			iconPhotoSet.iconPhotoCtrl.Setup(ppd, this.sortTypeMain, true, false, -1, false);
			bool flag = ppd != null && this.basePhotoPackData != null && ppd.staticData.baseData.forbiddenDiscardFlg && ppd.staticData.GetId() != this.basePhotoPackData.staticData.GetId();
			if (ppd != null && ppd == this.basePhotoPackData)
			{
				iconPhotoSet.iconPhotoCtrl.DispImgDisable(true);
				if (!ppd.dynamicData.lockFlag && !ppd.dynamicData.favoriteFlag)
				{
					iconPhotoSet.iconPhotoCtrl.DispParty(true, true);
				}
			}
			else if (ppd != null && this.deckPhotoDataIdList.Contains(ppd.dataId))
			{
				if (ppd == this.basePhotoPackData || ppd.dynamicData.lockFlag || ppd.dynamicData.favoriteFlag || flag)
				{
					iconPhotoSet.iconPhotoCtrl.DispImgDisable(true);
				}
				if (flag)
				{
					iconPhotoSet.iconPhotoCtrl.DispTextDisable(true, SceneCharaEdit.IsStoryPhoto(ppd.staticData) ? PhotoUtil.StoryPhotoText : PhotoUtil.NoSelectedText, null);
				}
				else if (!ppd.dynamicData.lockFlag && !ppd.dynamicData.favoriteFlag)
				{
					iconPhotoSet.iconPhotoCtrl.DispParty(true, true);
				}
			}
			else if (ppd != null && (ppd.dynamicData.lockFlag || ppd.dynamicData.favoriteFlag))
			{
				iconPhotoSet.iconPhotoCtrl.DispImgDisable(true);
				if (flag)
				{
					iconPhotoSet.iconPhotoCtrl.DispTextDisable(true, SceneCharaEdit.IsStoryPhoto(ppd.staticData) ? PhotoUtil.StoryPhotoText : PhotoUtil.NoSelectedText, null);
				}
			}
			else if (flag)
			{
				iconPhotoSet.iconPhotoCtrl.DispImgDisable(true);
				iconPhotoSet.iconPhotoCtrl.DispTextDisable(true, SceneCharaEdit.IsStoryPhoto(ppd.staticData) ? PhotoUtil.StoryPhotoText : PhotoUtil.NoSelectedText, null);
			}
			else if (this.UnuseLevelLimitOverPhoto(ppd))
			{
				iconPhotoSet.iconPhotoCtrl.DispImgDisable(true);
				iconPhotoSet.iconPhotoCtrl.DispTextDisable(true, PhotoUtil.NoStrengthenText, null);
			}
			else if (this.CheckPhotoForbiddenDiscard(ppd))
			{
				iconPhotoSet.iconPhotoCtrl.DispImgDisable(true);
				iconPhotoSet.iconPhotoCtrl.DispTextDisable(true, PhotoUtil.NoStrengthenText, null);
			}
			else if (this.CanLevelLimitOver(ppd))
			{
				iconPhotoSet.iconPhotoCtrl.DispInfoPop(true, null);
			}
			else
			{
				iconPhotoSet.iconPhotoCtrl.DispImgDisable(false);
				iconPhotoSet.iconPhotoCtrl.DispTextDisable(false, null, null);
				iconPhotoSet.iconPhotoCtrl.DispParty(false, true);
			}
			iconPhotoSet.iconPhotoCtrl.CheckTextDisable(false, null);
			int num2 = this.feedPhotoPackList.IndexOf(ppd);
			if (num2 >= 0)
			{
				iconPhotoSet.currentFrame.SetActive(true);
				iconPhotoSet.DispCount(true, (num2 + 1).ToString());
			}
			else
			{
				iconPhotoSet.currentFrame.SetActive(false);
				iconPhotoSet.DispCount(false, null);
			}
			bool flag2 = false;
			if (this.simulateGrowLvMax && !this.CanLevelLimitOver(ppd))
			{
				if (this.feedPhotoPackList.Find((PhotoPackData item) => item == ppd) == null)
				{
					flag2 = true;
				}
			}
			else if (this.SimulateGrowBreakLimitMax && PhotoUtil.IsLevelLimitOverPhoto(ppd) && this.feedPhotoPackList.Find((PhotoPackData item) => item == ppd) == null)
			{
				flag2 = true;
				iconPhotoSet.iconPhotoCtrl.DispInfoPop(false, null);
				iconPhotoSet.iconPhotoCtrl.DispTextDisable(true, PhotoUtil.NoSelectedText, null);
			}
			if (flag2)
			{
				iconPhotoSet.iconPhotoCtrl.DispImgDisable(true);
			}
		}
	}

	private bool CheckBasePhoto()
	{
		return this.guiData.photoGrowMain.baseIconPhotoCtrl.photoPackData != null && this.guiData.photoGrowMain.baseIconPhotoCtrl.photoPackData.staticData != null && !this.guiData.photoGrowMain.baseIconPhotoCtrl.photoPackData.IsInvalid();
	}

	private bool CanLevelLimitOver(PhotoPackData ppd)
	{
		return ppd != null && this.CheckBasePhoto() && (this.guiData.photoGrowMain.baseIconPhotoCtrl.photoPackData.staticData.GetId() == ppd.staticData.GetId() || (PhotoUtil.IsLevelLimitOverPhoto(ppd) && ppd.staticData.GetRarity() >= this.guiData.photoGrowMain.baseIconPhotoCtrl.photoPackData.staticData.GetRarity())) && this.guiData.photoGrowMain.baseIconPhotoCtrl.photoPackData.dynamicData.levelRank < 4;
	}

	private bool UnuseLevelLimitOverPhoto(PhotoPackData targetPPD)
	{
		if (!this.CheckBasePhoto())
		{
			return false;
		}
		if (targetPPD == null)
		{
			return false;
		}
		ItemDef.Rarity rarity = this.guiData.photoGrowMain.baseIconPhotoCtrl.photoPackData.staticData.GetRarity();
		return PhotoUtil.IsLevelLimitOverPhoto(targetPPD) && targetPPD.staticData.GetRarity() < rarity;
	}

	private bool CheckPhotoForbiddenDiscard(PhotoPackData targetPPD)
	{
		if (!this.CheckBasePhoto())
		{
			return false;
		}
		if (targetPPD == null)
		{
			return false;
		}
		bool forbiddenDiscardFlg = this.guiData.photoGrowMain.baseIconPhotoCtrl.photoPackData.staticData.baseData.forbiddenDiscardFlg;
		bool isForbiddenUseLimitOverPhoto = this.guiData.photoGrowMain.baseIconPhotoCtrl.photoPackData.staticData.baseData.isForbiddenUseLimitOverPhoto;
		return (forbiddenDiscardFlg || isForbiddenUseLimitOverPhoto) && PhotoUtil.IsLevelLimitOverPhoto(targetPPD);
	}

	private void OnStartItemWindowPhotoGrow(int index, GameObject go)
	{
		for (int i = 0; i < SelPhotoGrowCtrl.PhotoGrowConfirmWindow.SCROLL_ITEM_NUN_H; i++)
		{
			IconPhotoCtrl component = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Photo, go.transform).GetComponent<IconPhotoCtrl>();
			component.transform.Find("AEImage_Eff_Change").gameObject.SetActive(false);
			component.transform.Find("All").gameObject.GetComponent<AELayerConstraint>().enabled = false;
			component.name = i.ToString();
			component.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
		}
	}

	private void OnUpdateItemWindowPhotoGrow(int index, GameObject go)
	{
		for (int i = 0; i < SelPhotoGrowCtrl.PhotoGrowConfirmWindow.SCROLL_ITEM_NUN_H; i++)
		{
			int num = index * SelPhotoGrowCtrl.PhotoGrowConfirmWindow.SCROLL_ITEM_NUN_H + i;
			PhotoPackData ppd = null;
			if (num < this.feedPhotoPackList.Count)
			{
				ppd = this.feedPhotoPackList[num];
			}
			IconPhotoCtrl component = go.transform.Find(string.Format("{0}", i)).GetComponent<IconPhotoCtrl>();
			if (component != null)
			{
				component.Setup(ppd, SortFilterDefine.SortType.LEVEL, false, false, -1, false);
				SelPhotoGrowCtrl.FeedPhotoPack feedPhotoPack = this._feedPhotoPackList.Find((SelPhotoGrowCtrl.FeedPhotoPack item) => item.data == ppd);
				if (feedPhotoPack != null && feedPhotoPack.IsDispOverlimits)
				{
					component.DispInfoPop(true, null);
				}
			}
		}
	}

	private void OnDestroy()
	{
		if (this.guiData.photoGrowConfirmWindow.owCtrl != null)
		{
			Object.Destroy(this.guiData.photoGrowConfirmWindow.owCtrl.gameObject);
			this.guiData.photoGrowConfirmWindow.owCtrl = null;
		}
		if (this.guiData != null)
		{
			Object.Destroy(this.guiData.baseObj);
			this.guiData = null;
		}
	}

	private void Update()
	{
		if (this.CurrentMode == SelPhotoGrowCtrl.Mode.SERVER_REQUEST_GROW && !this.serverRequestGrow.MoveNext())
		{
			this.requestMode = SelPhotoGrowCtrl.Mode.GROW_EFFECT;
		}
		switch (this.CurrentMode)
		{
		case SelPhotoGrowCtrl.Mode.GROW_EFFECT:
			switch (this.GrowEffectPahse)
			{
			case SelPhotoGrowCtrl.PhotoGrowEffectPahse.Initialize:
				if (this.guiData.photoGrowAuth.AEImage_Front.m_AEImage.playTime >= 0.5f && this.guiData.photoGrowMain.baseObj.activeSelf)
				{
					this.SettingGrowMain();
				}
				if (!this.guiData.photoGrowAuth.AEImage_Front.IsPlaying())
				{
					DataManagerPhoto.PhotoLevelupResult photoGrowResult = DataManager.DmPhoto.GetPhotoGrowResult();
					this.GrowEffectPahse = SelPhotoGrowCtrl.PhotoGrowEffectPahse.GageUp;
					this.guiData.photoGrowAuth.AEImage_Back.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
					{
						this.guiData.photoGrowAuth.AEImage_Back.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
					});
					this.growGageEffect = null;
					if (this.effectChangeAction)
					{
						this.guiData.photoGrowAuth.AEImage_Result.m_AEImage.color = Color.white;
						this.guiData.photoGrowAuth.AEImage_Result.PlayAnimation(PguiAECtrl.AmimeType.START, null);
						if (!this.guiData.photoGrowAuth.AEImage_Front.IsPlaying())
						{
							this.growGageEffect = this.GrowGageEffect(this.basePhotoPackData, photoGrowResult);
						}
					}
					else
					{
						this.guiData.photoGrowAuth.AEImage_Front.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
						this.growGageEffect = this.GrowGageEffect(this.basePhotoPackData, photoGrowResult);
					}
				}
				break;
			case SelPhotoGrowCtrl.PhotoGrowEffectPahse.GageUp:
				if (this.growGageEffect != null && !this.growGageEffect.MoveNext())
				{
					this.GrowEffectPahse = SelPhotoGrowCtrl.PhotoGrowEffectPahse.GrowResult;
				}
				break;
			case SelPhotoGrowCtrl.PhotoGrowEffectPahse.GrowResult:
			{
				SoundManager.Stop("se_common");
				DataManagerPhoto.PhotoLevelupResult photoGrowResult2 = DataManager.DmPhoto.GetPhotoGrowResult();
				this.guiData.photoGrowAuth.Gage.m_Image.fillAmount = (float)photoGrowResult2.exp / (float)DataManager.DmPhoto.GetExpByNextLevel(photoGrowResult2, false);
				this.SettingGrowGageEffectParam(photoGrowResult2.photoDataId, photoGrowResult2.befLevel, photoGrowResult2.befLevelRank, photoGrowResult2.befLimitLevel, photoGrowResult2.level, photoGrowResult2.levelRank, photoGrowResult2.limitLevel);
				CanvasManager.HdlMissionProgressCtrl.IsPhotoGrow = false;
				if (photoGrowResult2.UnusedPhotos)
				{
					this.GrowEffectPahse = SelPhotoGrowCtrl.PhotoGrowEffectPahse.ReturnPhoto;
				}
				else if (0 < photoGrowResult2.GrowRewardInfoList.FindAll((DataManagerPhoto.PhotoLevelupResult.GrowRewardInfo x) => x.GetType == DataManagerPhoto.PhotoLevelupResult.GrowRewardInfo.GET_TYPE.RANK_MAX).Count)
				{
					this.GrowEffectPahse = SelPhotoGrowCtrl.PhotoGrowEffectPahse.BreakthroughLimitMax;
				}
				else if (0 < photoGrowResult2.GrowRewardInfoList.FindAll((DataManagerPhoto.PhotoLevelupResult.GrowRewardInfo x) => x.GetType == DataManagerPhoto.PhotoLevelupResult.GrowRewardInfo.GET_TYPE.LEVEL_MAX).Count)
				{
					this.GrowEffectPahse = SelPhotoGrowCtrl.PhotoGrowEffectPahse.GrowthMax;
				}
				else
				{
					this.GrowEffectPahse = SelPhotoGrowCtrl.PhotoGrowEffectPahse.TouchWait;
				}
				break;
			}
			case SelPhotoGrowCtrl.PhotoGrowEffectPahse.BreakthroughLimitMax:
			{
				DataManagerPhoto.PhotoLevelupResult photoGrowResult3 = DataManager.DmPhoto.GetPhotoGrowResult();
				SelPhotoGrowCtrl.PhotoGrowEffectPahse nexPhase2 = SelPhotoGrowCtrl.PhotoGrowEffectPahse.None;
				if (0 < photoGrowResult3.GrowRewardInfoList.FindAll((DataManagerPhoto.PhotoLevelupResult.GrowRewardInfo x) => x.GetType == DataManagerPhoto.PhotoLevelupResult.GrowRewardInfo.GET_TYPE.LEVEL_MAX).Count)
				{
					nexPhase2 = SelPhotoGrowCtrl.PhotoGrowEffectPahse.GrowthMax;
				}
				else
				{
					nexPhase2 = SelPhotoGrowCtrl.PhotoGrowEffectPahse.TouchWait;
				}
				List<ItemData> list = photoGrowResult3.GrowRewardInfoList.FindAll((DataManagerPhoto.PhotoLevelupResult.GrowRewardInfo x) => x.GetType == DataManagerPhoto.PhotoLevelupResult.GrowRewardInfo.GET_TYPE.RANK_MAX).ConvertAll<ItemData>((DataManagerPhoto.PhotoLevelupResult.GrowRewardInfo x) => new ItemData(x.ItemId, 1));
				ItemData itemData = list[0];
				DataManager.DmPhoto.GetPhotoStaticData(photoGrowResult3.itemId);
				this.OpenItemWindow(list, "限界突破最大を初めて達成したため\n" + itemData.staticData.GetName() + "\nを獲得しました！", delegate(int x)
				{
					this.GrowEffectPahse = nexPhase2;
					return true;
				});
				this.GrowEffectPahse = SelPhotoGrowCtrl.PhotoGrowEffectPahse.None;
				break;
			}
			case SelPhotoGrowCtrl.PhotoGrowEffectPahse.GrowthMax:
			{
				DataManagerPhoto.PhotoLevelupResult photoGrowResult4 = DataManager.DmPhoto.GetPhotoGrowResult();
				SelPhotoGrowCtrl.PhotoGrowEffectPahse nexPhase3 = SelPhotoGrowCtrl.PhotoGrowEffectPahse.TouchWait;
				List<ItemData> list2 = photoGrowResult4.GrowRewardInfoList.FindAll((DataManagerPhoto.PhotoLevelupResult.GrowRewardInfo x) => x.GetType == DataManagerPhoto.PhotoLevelupResult.GrowRewardInfo.GET_TYPE.LEVEL_MAX).ConvertAll<ItemData>((DataManagerPhoto.PhotoLevelupResult.GrowRewardInfo x) => new ItemData(x.ItemId, 1));
				ItemData itemData2 = list2[0];
				DataManager.DmPhoto.GetPhotoStaticData(photoGrowResult4.itemId);
				this.OpenItemWindow(list2, "強化最大を初めて達成したため\n" + itemData2.staticData.GetName() + "\nを獲得しました！", delegate(int x)
				{
					this.GrowEffectPahse = nexPhase3;
					return true;
				});
				this.GrowEffectPahse = SelPhotoGrowCtrl.PhotoGrowEffectPahse.None;
				break;
			}
			case SelPhotoGrowCtrl.PhotoGrowEffectPahse.ReturnPhoto:
			{
				DataManagerPhoto.PhotoLevelupResult photoGrowResult5 = DataManager.DmPhoto.GetPhotoGrowResult();
				SelPhotoGrowCtrl.PhotoGrowEffectPahse nexPhase = SelPhotoGrowCtrl.PhotoGrowEffectPahse.None;
				if (0 < photoGrowResult5.GrowRewardInfoList.FindAll((DataManagerPhoto.PhotoLevelupResult.GrowRewardInfo x) => x.GetType == DataManagerPhoto.PhotoLevelupResult.GrowRewardInfo.GET_TYPE.RANK_MAX).Count)
				{
					nexPhase = SelPhotoGrowCtrl.PhotoGrowEffectPahse.BreakthroughLimitMax;
				}
				else if (0 < photoGrowResult5.GrowRewardInfoList.FindAll((DataManagerPhoto.PhotoLevelupResult.GrowRewardInfo x) => x.GetType == DataManagerPhoto.PhotoLevelupResult.GrowRewardInfo.GET_TYPE.LEVEL_MAX).Count)
				{
					nexPhase = SelPhotoGrowCtrl.PhotoGrowEffectPahse.GrowthMax;
				}
				else
				{
					nexPhase = SelPhotoGrowCtrl.PhotoGrowEffectPahse.TouchWait;
				}
				CanvasManager.HdlOpenWindowBasic.Setup("確認", "EXPの上限を超えた分の素材フォトとゴールドは\n使用を取り消しました！", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int x)
				{
					this.GrowEffectPahse = nexPhase;
					return true;
				}, null, false);
				CanvasManager.HdlOpenWindowBasic.Open();
				this.GrowEffectPahse = SelPhotoGrowCtrl.PhotoGrowEffectPahse.None;
				break;
			}
			case SelPhotoGrowCtrl.PhotoGrowEffectPahse.Refresh:
				this.ReloadDataManager();
				this.GrowEffectPahse = SelPhotoGrowCtrl.PhotoGrowEffectPahse.None;
				if (this.basePhotoPackData.dynamicData.level >= this.basePhotoPackData.staticData.getLimitLevel(this.basePhotoPackData.dynamicData.levelRank) && this.basePhotoPackData.dynamicData.levelRank >= 4)
				{
					CanvasManager.HdlPhotoWindowCtrl.ResetPrevData();
					this.requestMode = SelPhotoGrowCtrl.Mode.PHOTO_SELECT;
				}
				else
				{
					this.requestMode = SelPhotoGrowCtrl.Mode.GROW_MAIN;
					this.guiData.photoGrowMain.baseObj.SetActive(true);
					this.guiData.photoGrowMain.SelCmn_AllInOut.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
					this.SettingGrowMain();
				}
				this.ExecSort();
				CanvasManager.HdlCmnMenu.gameObject.SetActive(true);
				break;
			}
			break;
		}
		if (this.requestMode != this.CurrentMode)
		{
			if (this.CurrentMode != SelPhotoGrowCtrl.Mode.PHOTO_SELECT)
			{
				if (this.CurrentMode == SelPhotoGrowCtrl.Mode.GROW_MAIN)
				{
					if (this.requestMode == SelPhotoGrowCtrl.Mode.PHOTO_SELECT)
					{
					}
				}
				else if (this.CurrentMode == SelPhotoGrowCtrl.Mode.OW_CONFIRM)
				{
					if (this.requestMode == SelPhotoGrowCtrl.Mode.GROW_EFFECT)
					{
					}
				}
				else if (this.CurrentMode == SelPhotoGrowCtrl.Mode.GROW_EFFECT && this.guiData.photoGrowAuth != null)
				{
					Object.Destroy(this.guiData.photoGrowAuth.baseObj);
					this.guiData.photoGrowAuth = null;
				}
			}
			if (this.requestMode == SelPhotoGrowCtrl.Mode.PHOTO_SELECT)
			{
				if (this.CurrentMode == SelPhotoGrowCtrl.Mode.GROW_MAIN)
				{
					this.guiData.photoGrowMain.SelCmn_AllInOut.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
					{
						this.guiData.photoGrowMain.baseObj.SetActive(false);
						this.guiData.photoGrowTop.baseObj.SetActive(true);
						this.guiData.photoGrowTop.ResizeScrollView(this.dispPhotoPackList.Count, this.dispPhotoPackList.Count / this.guiData.photoGrowTop.sizeChangeBtnGUI.IconPhotoParamList[this.guiData.photoGrowTop.sizeChangeBtnGUI.SizeIndex].num);
						this.guiData.photoGrowTop.Num_Own.ReplaceTextByDefault("Param01", this.haveAllPhotoPackList.Count.ToString() + "/" + DataManager.DmPhoto.PhotoStockLimit.ToString());
						this.guiData.photoGrowTop.SelCmn_AllInOut.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
					});
				}
				else
				{
					this.guiData.photoGrowTop.baseObj.SetActive(true);
					this.guiData.photoGrowTop.ResizeScrollView(this.dispPhotoPackList.Count, this.dispPhotoPackList.Count / this.guiData.photoGrowTop.sizeChangeBtnGUI.IconPhotoParamList[this.guiData.photoGrowTop.sizeChangeBtnGUI.SizeIndex].num);
					this.guiData.photoGrowTop.Num_Own.ReplaceTextByDefault("Param01", this.haveAllPhotoPackList.Count.ToString() + "/" + DataManager.DmPhoto.PhotoStockLimit.ToString());
					this.guiData.photoGrowTop.SelCmn_AllInOut.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
				}
			}
			else if (this.requestMode == SelPhotoGrowCtrl.Mode.GROW_MAIN)
			{
				if (this.CurrentMode != SelPhotoGrowCtrl.Mode.GROW_EFFECT && this.CurrentMode == SelPhotoGrowCtrl.Mode.PHOTO_SELECT)
				{
					this.SettingGrowMain();
					if (this.guiData.photoGrowTop.baseObj.activeSelf)
					{
						this.guiData.photoGrowTop.SelCmn_AllInOut.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
						{
							this.guiData.photoGrowTop.baseObj.SetActive(false);
							this.guiData.photoGrowMain.baseObj.SetActive(true);
							CanvasManager.HdlOpenWindowSortFilter.SolutionList(SortFilterDefine.RegisterType.PHOTO_GROW_MAIN, null);
							this.guiData.photoGrowMain.SelCmn_AllInOut.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
						});
					}
					else
					{
						this.guiData.photoGrowMain.baseObj.SetActive(true);
						CanvasManager.HdlOpenWindowSortFilter.SolutionList(SortFilterDefine.RegisterType.PHOTO_GROW_MAIN, null);
						this.guiData.photoGrowMain.SelCmn_AllInOut.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
					}
				}
			}
			else if (this.requestMode == SelPhotoGrowCtrl.Mode.GROW_EFFECT)
			{
				DataManagerPhoto.PhotoLevelupResult photoGrowResult6 = DataManager.DmPhoto.GetPhotoGrowResult();
				this.effectChangeAction = DataManager.DmPhoto.GetPhotoStaticData(photoGrowResult6.itemId).baseData.imageChangeFlg && photoGrowResult6.levelRank != photoGrowResult6.befLevelRank && this.basePhotoPackData.IsImageChange();
				this.guiData.photoGrowAuth = new SelPhotoGrowCtrl.PhotoGrowAuth(AssetManager.InstantiateAssetData((!this.effectChangeAction) ? "SceneCharaEdit/GUI/Prefab/PhotoGrowAuth_Normal" : "SceneCharaEdit/GUI/Prefab/PhotoGrowAuth_Special", base.transform).transform);
				PrjUtil.AddTouchEventTrigger(this.guiData.photoGrowAuth.touchPanel.gameObject, delegate(Transform x)
				{
					this.OnClickPanel();
				});
				this.guiData.photoGrowAuth.iconPhoto.Setup(this.basePhotoPackData, SortFilterDefine.SortType.LEVEL, true, false, -1, false);
				this.SettingGrowGageEffectParam(photoGrowResult6.photoDataId, photoGrowResult6.befLevel, photoGrowResult6.befLevelRank, photoGrowResult6.befLimitLevel, photoGrowResult6.befLevel, photoGrowResult6.befLevelRank, photoGrowResult6.befLimitLevel);
				this.guiData.photoGrowAuth.Gage.m_Image.fillAmount = (float)photoGrowResult6.befExp / (float)DataManager.DmPhoto.GetExpByNextLevel(photoGrowResult6, true);
				this.guiData.photoGrowAuth.AEImage_LevelUp.PauseAnimation(PguiAECtrl.AmimeType.START, null);
				this.guiData.photoGrowAuth.AEImage_Back.PauseAnimation(PguiAECtrl.AmimeType.START, null);
				this.optionPhotoPacks.Remove(this.basePhotoPackData);
				InfoPhotoItemEffectCtrl infoPhotoItemEffectCtrl = this.guiData.photoGrowAuth.infoPhotoItemEffectCtrl;
				InfoPhotoItemEffectCtrl.SetupParam setupParam = new InfoPhotoItemEffectCtrl.SetupParam();
				setupParam.photoPackDatas = new List<PhotoPackData> { this.basePhotoPackData };
				setupParam.optionGameObjects = this.guiData.photoGrowAuth.Num_Percents.ConvertAll<GameObject>((PguiTextCtrl item) => item.gameObject);
				setupParam.optionPhotoPackDatas = this.optionPhotoPacks;
				setupParam.optionPhotoBonusResultCB = delegate(GameObject go, DataManagerPhoto.CalcDropBonusResult val)
				{
					PguiTextCtrl component = go.GetComponent<PguiTextCtrl>();
					if (component != null)
					{
						float num = (float)val.ratio;
						if (num > (float)(4 * val.targetItemBonusRatio))
						{
							num = (float)(4 * val.targetItemBonusRatio);
						}
						component.text = "(+" + (num / 100f).ToString("#.#") + "%)";
					}
				};
				infoPhotoItemEffectCtrl.Setup(setupParam);
				if (this.effectChangeAction)
				{
					this.guiData.photoGrowAuth.iconPhoto.DispPhotoChange(false);
				}
				this.guiData.photoGrowAuth.AEImage_Front.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
				{
					if (this.effectChangeAction)
					{
						this.guiData.photoGrowAuth.iconPhoto.DispPhotoChange(true);
					}
				});
				this.GrowEffectPahse = SelPhotoGrowCtrl.PhotoGrowEffectPahse.Initialize;
				string text = string.Empty;
				switch (photoGrowResult6.successStatus)
				{
				case DataManagerPhoto.PhotoLevelupResult.Status.NORMAL:
					text = "NORMAL";
					SoundManager.Play(this.effectChangeAction ? "prd_se_photo_limit_break_max" : "prd_se_photo_levelup_auth", false, false);
					break;
				case DataManagerPhoto.PhotoLevelupResult.Status.SPECIAL_S:
					text = "SPECIAL_S";
					SoundManager.Play(this.effectChangeAction ? "prd_se_photo_limit_break_max_good" : "prd_se_photo_levelup_good_auth", false, false);
					break;
				case DataManagerPhoto.PhotoLevelupResult.Status.SPECIAL_L:
					text = "SPECIAL_L";
					SoundManager.Play(this.effectChangeAction ? "prd_se_photo_limit_break_max_great" : "prd_se_photo_levelup_great_auth", false, false);
					break;
				default:
					text = "NORMAL";
					break;
				}
				this.guiData.photoGrowAuth.AEImage_Result.gameObject.SetActive(true);
				this.guiData.photoGrowAuth.AEImage_Result.GetComponent<PguiReplaceAECtrl>().Replace(text);
				if (this.effectChangeAction)
				{
					this.guiData.photoGrowAuth.AEImage_Result.m_AEImage.color = Color.clear;
				}
				else
				{
					this.guiData.photoGrowAuth.AEImage_Result.PlayAnimation(PguiAECtrl.AmimeType.START, null);
				}
				CanvasManager.HdlCmnMenu.gameObject.SetActive(false);
			}
			CanvasManager.HdlCmnMenu.UpdateSubInfo((this.requestMode != SelPhotoGrowCtrl.Mode.PHOTO_SELECT) ? "素材のフォトを選んでください" : "強化するベースとなるフォトを選んでください");
			this.CurrentMode = this.requestMode;
		}
	}

	private void OpenItemWindow(List<ItemData> getItem, string message, PguiOpenWindowCtrl.Callback closeCallback)
	{
		CanvasManager.HdlGetItemWindowCtrl.Setup(getItem, new GetItemWindowCtrl.SetupParam
		{
			strItemCb = (GetItemWindowCtrl.WordingCallbackParam param) => message,
			windowFinishedCallback = closeCallback
		});
		CanvasManager.HdlGetItemWindowCtrl.Open();
	}

	private IEnumerator GrowGageEffect(PhotoPackData affterPhoto, DataManagerPhoto.PhotoLevelupResult result)
	{
		this.SettingGrowGageEffectParam(result.photoDataId, result.befLevel, result.befLevelRank, result.befLimitLevel, result.befLevel, result.befLevelRank, result.befLimitLevel);
		CriAtomExPlayback caep = SoundManager.Play("prd_se_photo_levelup_gauge", true, false);
		yield return null;
		int effectLevel = result.befLevel;
		float afterFill = (float)result.exp / (float)DataManager.DmPhoto.GetExpByNextLevel(result, false);
		this.guiData.photoGrowAuth.Gage.m_Image.fillAmount = (float)result.befExp / (float)DataManager.DmPhoto.GetExpByNextLevel(result.itemId, result.befLevel, result.levelRank);
		while (effectLevel <= result.level)
		{
			this.guiData.photoGrowAuth.Gage.m_Image.fillAmount += 0.03f;
			if (effectLevel < result.level)
			{
				if (this.guiData.photoGrowAuth.Gage.m_Image.fillAmount >= 1f)
				{
					this.guiData.photoGrowAuth.Gage.m_Image.fillAmount = 1f;
					yield return null;
					int num = effectLevel;
					effectLevel = num + 1;
					this.guiData.photoGrowAuth.AEImage_LevelUp.PlayAnimation(PguiAECtrl.AmimeType.START, null);
					this.guiData.photoGrowAuth.Gage.m_Image.fillAmount = 0f;
					this.SettingGrowGageEffectParam(result.photoDataId, result.befLevel, result.befLevelRank, result.befLimitLevel, effectLevel, result.befLevelRank, result.befLimitLevel);
					SoundManager.Play("prd_se_photo_levelup_font", false, false);
				}
			}
			else if (this.guiData.photoGrowAuth.Gage.m_Image.fillAmount >= afterFill)
			{
				int num = effectLevel;
				effectLevel = num + 1;
				this.guiData.photoGrowAuth.Gage.m_Image.fillAmount = afterFill;
			}
			yield return null;
		}
		caep.Stop();
		if (result.befLevelRank != result.levelRank)
		{
			IEnumerator wait = this.Wait(0.5f);
			while (wait.MoveNext())
			{
				yield return null;
			}
			wait = null;
		}
		this.SettingGrowGageEffectParam(result.photoDataId, result.befLevel, result.befLevelRank, result.befLimitLevel, result.level, result.levelRank, result.limitLevel);
		yield break;
	}

	private void SettingGrowGageEffectParam(long photoDataId, int befLevel, int befLevelRank, int befLimitLevel, int aftLevel, int aftLevelRank, int aftLimitLevel)
	{
		string text = ((aftLevel > befLevel) ? SelPhotoGrowCtrl.UP_PARAM_COLOR_CODE : SelPhotoGrowCtrl.NORMAL_PARAM_COLOR_CODE);
		string text2 = ((aftLimitLevel > befLimitLevel) ? SelPhotoGrowCtrl.UP_PARAM_COLOR_CODE : SelPhotoGrowCtrl.NORMAL_PARAM_COLOR_CODE);
		this.guiData.photoGrowAuth.Num_Lv_After.ReplaceTextByDefault(new string[] { "Param01", "Param02", "Param03", "Param04", "Param05" }, new string[]
		{
			aftLevel.ToString(),
			aftLimitLevel.ToString(),
			(aftLevel - befLevel).ToString(),
			text,
			text2
		});
		int id = DataManager.DmPhoto.GetUserPhotoData(photoDataId).staticData.GetId();
		PrjUtil.ParamPreset paramPreset = PrjUtil.CalcParamByPhoto(DataManager.DmPhoto.GetPhotoStaticData(id), befLevel);
		PrjUtil.ParamPreset paramPreset2 = PrjUtil.CalcParamByPhoto(DataManager.DmPhoto.GetPhotoStaticData(id), aftLevel);
		List<int> list = new List<int> { paramPreset.hp, paramPreset.atk, paramPreset.def };
		List<int> list2 = new List<int> { paramPreset2.hp, paramPreset2.atk, paramPreset2.def };
		for (int i = 0; i < this.guiData.photoGrowAuth.Num_After.Count; i++)
		{
			this.guiData.photoGrowAuth.Num_After[i].ReplaceTextByDefault(new string[] { "Param01", "Param02", "Param03" }, new string[]
			{
				list2[i].ToString(),
				(list2[i] - list[i]).ToString(),
				(aftLevel > befLevel) ? SelPhotoGrowCtrl.UP_PARAM_COLOR_CODE : SelPhotoGrowCtrl.NORMAL_PARAM_COLOR_CODE
			});
		}
		for (int j = 0; j < this.guiData.photoGrowAuth.RebirthIcon_After.Count; j++)
		{
			this.guiData.photoGrowAuth.RebirthIcon_After[j].SetActive(j < aftLevelRank);
			this.guiData.photoGrowAuth.RebirthIcon_After[j].GetComponent<uGUITweenColor>().enabled = j < aftLevelRank && j >= befLevelRank;
		}
	}

	private IEnumerator ServerRequestGrow(PhotoPackData basePhoto, List<PhotoPackData> feedPhotoList)
	{
		DataManager.DmPhoto.RequestActionPhotoGrow(basePhoto, this.feedPhotoPackList);
		yield return null;
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		yield return null;
		yield break;
	}

	private void ClearFeedPhotoPackList()
	{
		this.feedPhotoPackList.Clear();
	}

	private void SettingGrowMain()
	{
		this.ClearFeedPhotoPackList();
		this.guiData.photoGrowMain.ResizeScrollView(this.dispAllPhotoPackList.Count, this.dispAllPhotoPackList.Count / this.guiData.photoGrowMain.sizeChangeBtnGUI.IconPhotoParamList[this.guiData.photoGrowMain.sizeChangeBtnGUI.SizeIndex].num);
		this.guiData.photoGrowMain.baseIconPhotoCtrl.Setup(this.basePhotoPackData, SortFilterDefine.SortType.LEVEL, true, false, -1, false);
		this.guiData.photoGrowMain.Btn_Info.gameObject.SetActive(this.basePhotoPackData.IsEnableSwitchCharacteristic());
		this.SettingGrowMainBySelectInfo();
	}

	private IEnumerator Wait(float second)
	{
		float timeSinceStartup = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup - timeSinceStartup < second)
		{
			yield return null;
		}
		yield break;
	}

	private bool ReturnPhotoSelect()
	{
		if (this.setupParam.onReturnSceneNameCB() == SceneManager.SceneName.None && this.CurrentMode == SelPhotoGrowCtrl.Mode.GROW_MAIN)
		{
			this.requestMode = SelPhotoGrowCtrl.Mode.PHOTO_SELECT;
			this.ClearFeedPhotoPackList();
			return true;
		}
		return false;
	}

	public void OnClickMenuReturn(UnityAction callback)
	{
		if (this.ReturnPhotoSelect())
		{
			CanvasManager.HdlOpenWindowSortFilter.SolutionList(SortFilterDefine.RegisterType.PHOTO_GROW_TOP, null);
			return;
		}
		this.guiData.photoGrowTop.SelCmn_AllInOut.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
		{
			this.ClearFeedPhotoPackList();
			UnityAction callback2 = callback;
			if (callback2 == null)
			{
				return;
			}
			callback2();
		});
	}

	private void WindowCloseStatusCallBack(List<ItemDef.Rarity> rarityList, List<SortFilterDefine.PhotoLevelType> photoLevelList, List<PhotoDef.Type> photoTypeList)
	{
		this.LatestRarityList = rarityList;
		this.LatestPhotoLevelList = photoLevelList;
		this.LatestPhotoTypeList = photoTypeList;
		this.AutoSelectGrowPhotoList();
	}

	private List<PhotoPackData> UnSelectableMaterialPhotoList(List<PhotoPackData> targetPhotoList, ref List<PhotoPackData> withoutDisableList)
	{
		List<PhotoPackData> list = new List<PhotoPackData>();
		foreach (PhotoPackData photoPackData in targetPhotoList)
		{
			if (photoPackData.dynamicData.lockFlag || photoPackData.dynamicData.favoriteFlag)
			{
				list.Add(photoPackData);
			}
			else if (this.basePhotoPackData != null && photoPackData.staticData != null && photoPackData.staticData.baseData.forbiddenDiscardFlg && this.basePhotoPackData.staticData.GetId() != photoPackData.staticData.GetId())
			{
				list.Add(photoPackData);
			}
			else if (this.UnuseLevelLimitOverPhoto(photoPackData))
			{
				list.Add(photoPackData);
			}
			else if (this.CheckPhotoForbiddenDiscard(photoPackData))
			{
				list.Add(photoPackData);
			}
			else if (this.basePhotoPackData != null && 4 == this.basePhotoPackData.dynamicData.levelRank && photoPackData.staticData.baseData.expPhotoType == PhotoDef.ExpPhotoType.LevelLimitOver)
			{
				list.Add(photoPackData);
			}
			else
			{
				withoutDisableList.Add(photoPackData);
			}
		}
		return list;
	}

	private void AutoSelectGrowPhotoList()
	{
		List<PhotoPackData> list = new List<PhotoPackData>();
		this.UnSelectableMaterialPhotoList(this.dispAllPhotoPackList, ref list);
		List<PhotoPackData> list2 = new List<PhotoPackData>();
		list.RemoveAll((PhotoPackData x) => x.dynamicData.dataId == this.basePhotoPackData.dataId);
		list.RemoveAll((PhotoPackData x) => x.staticData.baseData.expPhotoType == PhotoDef.ExpPhotoType.LevelLimitOver);
		list.RemoveAll((PhotoPackData x) => x.dynamicData.lockFlag);
		list.RemoveAll((PhotoPackData x) => x.dynamicData.favoriteFlag);
		list.RemoveAll((PhotoPackData x) => this.deckPhotoDataIdList.Contains(x.dynamicData.dataId));
		if (this.LatestRarityList.Count == 0 && this.LatestPhotoLevelList.Count == 0 && this.LatestPhotoTypeList.Count == 0)
		{
			list = new List<PhotoPackData>();
		}
		if (0 < this.LatestRarityList.Count)
		{
			if (!this.LatestRarityList.Contains(ItemDef.Rarity.STAR1))
			{
				list.RemoveAll((PhotoPackData x) => x.staticData.baseData.rarity == ItemDef.Rarity.STAR1);
			}
			if (!this.LatestRarityList.Contains(ItemDef.Rarity.STAR2))
			{
				list.RemoveAll((PhotoPackData x) => x.staticData.baseData.rarity == ItemDef.Rarity.STAR2);
			}
			if (!this.LatestRarityList.Contains(ItemDef.Rarity.STAR3))
			{
				list.RemoveAll((PhotoPackData x) => x.staticData.baseData.rarity == ItemDef.Rarity.STAR3);
			}
			if (!this.LatestRarityList.Contains(ItemDef.Rarity.STAR4))
			{
				list.RemoveAll((PhotoPackData x) => x.staticData.baseData.rarity == ItemDef.Rarity.STAR4);
			}
		}
		if (0 < this.LatestPhotoLevelList.Count && this.LatestPhotoLevelList.Contains(SortFilterDefine.PhotoLevelType.One))
		{
			list.RemoveAll((PhotoPackData x) => 1 != x.dynamicData.level);
		}
		if (0 < this.LatestPhotoTypeList.Count && this.LatestPhotoTypeList.Contains(PhotoDef.Type.OTHER))
		{
			list.RemoveAll((PhotoPackData x) => x.staticData.baseData.expPhotoType != PhotoDef.ExpPhotoType.Experience);
		}
		List<PhotoPackData> list3 = list.FindAll((PhotoPackData x) => x.staticData.GetId() == this.basePhotoPackData.staticData.GetId());
		list3.Sort((PhotoPackData a, PhotoPackData b) => a.dynamicData.level - b.dynamicData.level);
		list.RemoveAll((PhotoPackData x) => x.staticData.GetId() == this.basePhotoPackData.staticData.GetId());
		int num = 0;
		foreach (PhotoPackData photoPackData in list3)
		{
			if (4 <= this.basePhotoPackData.dynamicData.levelRank + num)
			{
				list.Add(photoPackData);
			}
			else
			{
				list2.Add(photoPackData);
				num += photoPackData.dynamicData.levelRank + 1;
			}
		}
		List<PhotoPackData> list4 = list.FindAll((PhotoPackData x) => x.staticData.GetRarity() == ItemDef.Rarity.STAR1);
		SelPhotoGrowCtrl.<AutoSelectGrowPhotoList>g__SortPhotoTypeLevel|88_0(ref list4);
		List<PhotoPackData> list5 = list.FindAll((PhotoPackData x) => x.staticData.GetRarity() == ItemDef.Rarity.STAR2);
		SelPhotoGrowCtrl.<AutoSelectGrowPhotoList>g__SortPhotoTypeLevel|88_0(ref list5);
		List<PhotoPackData> list6 = list.FindAll((PhotoPackData x) => x.staticData.GetRarity() == ItemDef.Rarity.STAR3);
		SelPhotoGrowCtrl.<AutoSelectGrowPhotoList>g__SortPhotoTypeLevel|88_0(ref list6);
		List<PhotoPackData> list7 = list.FindAll((PhotoPackData x) => x.staticData.GetRarity() == ItemDef.Rarity.STAR4);
		SelPhotoGrowCtrl.<AutoSelectGrowPhotoList>g__SortPhotoTypeLevel|88_0(ref list7);
		list = new List<PhotoPackData>();
		list.AddRange(list4);
		list.AddRange(list5);
		list.AddRange(list6);
		list.AddRange(list7);
		foreach (PhotoPackData photoPackData2 in list)
		{
			DataManagerPhoto.PhotoLevelupResult photoLevelupResult = PhotoUtil.CalcPhotoGrow(this.basePhotoPackData, list2);
			if (photoLevelupResult.limitLevel == photoLevelupResult.level)
			{
				break;
			}
			list2.Add(photoPackData2);
		}
		this.feedPhotoPackList = list2;
		this.SettingGrowMainBySelectInfo();
		this.guiData.photoGrowMain.ScrollView.Refresh();
	}

	[CompilerGenerated]
	private void <ReloadDataManager>g__SetupPhotoList|48_0()
	{
		this.haveAllPhotoPackList = new List<PhotoPackData>(DataManager.DmPhoto.GetUserPhotoMap().Values);
		this.havePhotoPackList = new List<PhotoPackData>();
		foreach (PhotoPackData photoPackData in DataManager.DmPhoto.GetUserPhotoMap().Values)
		{
			if (!photoPackData.staticData.baseData.isForbiddenGrowBase)
			{
				this.havePhotoPackList.Add(photoPackData);
			}
		}
		this.dispAllPhotoPackList = new List<PhotoPackData>(this.haveAllPhotoPackList);
		this.dispPhotoPackList = new List<PhotoPackData>(this.havePhotoPackList);
	}

	[CompilerGenerated]
	internal static void <AutoSelectGrowPhotoList>g__SortPhotoTypeLevel|88_0(ref List<PhotoPackData> targetPhotoList)
	{
		targetPhotoList.Sort((PhotoPackData a, PhotoPackData b) => a.dynamicData.dataId.CompareTo(b.dynamicData.dataId));
		PrjUtil.InsertionSort<PhotoPackData>(ref targetPhotoList, (PhotoPackData a, PhotoPackData b) => a.dynamicData.level - b.dynamicData.level);
		List<PhotoPackData> list = new List<PhotoPackData>();
		List<PhotoPackData> list2 = new List<PhotoPackData>();
		foreach (PhotoPackData photoPackData in targetPhotoList)
		{
			if (photoPackData.staticData.baseData.expPhotoType == PhotoDef.ExpPhotoType.Experience)
			{
				list.Add(photoPackData);
			}
			else
			{
				list2.Add(photoPackData);
			}
		}
		targetPhotoList = new List<PhotoPackData>();
		targetPhotoList.AddRange(list);
		targetPhotoList.AddRange(list2);
	}

	private static readonly string UP_PARAM_COLOR_CODE = "#FF7C17FF";

	private static readonly string NORMAL_PARAM_COLOR_CODE = "#533C06FF";

	private static readonly Color UP_PARAM_COLOR = new Color32(byte.MaxValue, 124, 23, byte.MaxValue);

	private static readonly Color NORMAL_PARAM_COLOR = new Color32(83, 60, 6, byte.MaxValue);

	private SelPhotoGrowCtrl.Mode requestMode;

	private SelPhotoGrowCtrl.SetupParam setupParam = new SelPhotoGrowCtrl.SetupParam();

	private SelPhotoGrowCtrl.GUI guiData;

	private SortFilterDefine.SortType sortTypeTop = SortFilterDefine.SortType.LEVEL;

	private SortFilterDefine.SortType sortTypeMain = SortFilterDefine.SortType.LEVEL;

	private List<PhotoPackData> haveAllPhotoPackList = new List<PhotoPackData>();

	private List<PhotoPackData> havePhotoPackList = new List<PhotoPackData>();

	private List<PhotoPackData> dispAllPhotoPackList = new List<PhotoPackData>();

	private List<PhotoPackData> dispPhotoPackList = new List<PhotoPackData>();

	private PhotoPackData basePhotoPackData;

	private List<PhotoPackData> feedPhotoPackList = new List<PhotoPackData>();

	private List<SelPhotoGrowCtrl.FeedPhotoPack> _feedPhotoPackList;

	private List<long> deckPhotoDataIdList = new List<long>();

	private bool simulateGrowLvMax;

	private bool SimulateGrowBreakLimitMax;

	private UserOptionData cloneUserOptionData;

	private List<ItemDef.Rarity> LatestRarityList;

	private List<SortFilterDefine.PhotoLevelType> LatestPhotoLevelList;

	private List<PhotoDef.Type> LatestPhotoTypeList;

	private List<PhotoPackData> optionPhotoPacks = new List<PhotoPackData>();

	private bool effectChangeAction;

	private IEnumerator growGageEffect;

	private IEnumerator serverRequestGrow;

	public class SetupParam
	{
		public SceneCharaEdit.OnReturnSceneName onReturnSceneNameCB;
	}

	public enum FrameType
	{
		INVALID,
		PHOTO_SELECT,
		GROW_MAIN
	}

	public enum Mode
	{
		INVALID,
		PHOTO_SELECT,
		GROW_MAIN,
		OW_CONFIRM,
		SERVER_REQUEST_GROW,
		GROW_EFFECT
	}

	public enum PhotoGrowEffectPahse
	{
		None,
		Initialize,
		GageUp,
		GrowResult,
		BreakthroughLimitMax,
		GrowthMax,
		ReturnPhoto,
		TouchWait,
		Refresh
	}

	public class PhotoGrowTop
	{
		public PhotoGrowTop(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_FilterOnOff = baseTr.Find("All/WindowAll/SortFilterBtnsAll/Btn_FilterOnOff").GetComponent<PguiButtonCtrl>();
			this.Btn_Sort = baseTr.Find("All/WindowAll/SortFilterBtnsAll/Btn_Sort").GetComponent<PguiButtonCtrl>();
			this.Btn_SortUpDown = baseTr.Find("All/WindowAll/SortFilterBtnsAll/Btn_SortUpDown").GetComponent<PguiButtonCtrl>();
			this.ScrollView = baseTr.Find("All/WindowAll/ScrollView").GetComponent<ReuseScroll>();
			this.Num_Own = baseTr.Find("All/WindowAll/SortFilterBtnsAll/Num_Own").GetComponent<PguiTextCtrl>();
			this.SelCmn_AllInOut = baseTr.GetComponent<SimpleAnimation>();
			this.Campaign = baseTr.Find("All/WindowAll/Campaign/SelCmn_CampaignInfo").gameObject;
			PguiTextCtrl component = this.Campaign.transform.Find("Txt_Campaign").GetComponent<PguiTextCtrl>();
			this.CampaignTimeText = this.Campaign.transform.Find("TimeInfo/Num_Time").GetComponent<PguiTextCtrl>();
			component.text = component.text.Replace("フレンズ成長", "フォト強化");
			this.sizeChangeBtnGUI = new PhotoUtil.SizeChangeBtnGUI(baseTr.Find("All/WindowAll/SortFilterBtnsAll/Btn_SizeChange"));
			this.Txt_None = baseTr.Find("All/WindowAll/Txt_None").gameObject;
			this.Txt_None.SetActive(false);
		}

		public void ResizeScrollView(int count, int resize)
		{
			this.Txt_None.SetActive(count <= 0);
			this.ScrollView.ResizeFocesNoMove(1 + resize);
		}

		public void InitializeCampaignInfo()
		{
			this.campaignGrowData = DataManager.DmCampaign.PresentCampaignGrowPhotoData;
			this.UpdateCampaignInfo();
		}

		public void UpdateCampaignInfo()
		{
			if (this.campaignGrowData == null)
			{
				this.Campaign.SetActive(false);
				return;
			}
			if (DataManager.DmCampaign.PresentCampaignGrowPhotoData == null || this.campaignGrowData.campaignId != DataManager.DmCampaign.PresentCampaignGrowPhotoData.campaignId)
			{
				this.campaignGrowData = null;
				return;
			}
			bool isDisplayCampaign = this.campaignGrowData.IsDisplayCampaign;
			this.Campaign.SetActive(isDisplayCampaign);
			if (isDisplayCampaign)
			{
				this.CampaignTimeText.text = TimeManager.MakeTimeResidueText(TimeManager.Now, this.campaignGrowData.endDatetime, true, true);
			}
		}

		public static readonly int SCROLL_ITEM_NUN_H = 6;

		public GameObject baseObj;

		public PguiButtonCtrl Btn_FilterOnOff;

		public PguiButtonCtrl Btn_Sort;

		public PguiButtonCtrl Btn_SortUpDown;

		public ReuseScroll ScrollView;

		public PguiTextCtrl Num_Own;

		public SimpleAnimation SelCmn_AllInOut;

		public GameObject Campaign;

		private PguiTextCtrl CampaignTimeText;

		private DataManagerCampaign.CampaignGrowData campaignGrowData;

		public PhotoUtil.SizeChangeBtnGUI sizeChangeBtnGUI;

		public GameObject Txt_None;
	}

	public class PhotoGrowMain
	{
		public PhotoGrowMain(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_FilterOnOff = baseTr.Find("All/PhotoAll/SortFilterBtnsAll/Btn_FilterOnOff").GetComponent<PguiButtonCtrl>();
			this.Btn_Sort = baseTr.Find("All/PhotoAll/SortFilterBtnsAll/Btn_Sort").GetComponent<PguiButtonCtrl>();
			this.Btn_SortUpDown = baseTr.Find("All/PhotoAll/SortFilterBtnsAll/Btn_SortUpDown").GetComponent<PguiButtonCtrl>();
			this.ButtonL = baseTr.Find("All/UseInfo/ButtonL").GetComponent<PguiButtonCtrl>();
			this.ButtonC = baseTr.Find("All/UseInfo/ButtonC").GetComponent<PguiButtonCtrl>();
			this.ButtonR = baseTr.Find("All/UseInfo/ButtonR").GetComponent<PguiButtonCtrl>();
			this.Num_Lv_Before = baseTr.Find("All/BasePhoto/StatusInfo/Num_Lv_Before").GetComponent<PguiTextCtrl>();
			this.Num_Lv_After = baseTr.Find("All/BasePhoto/StatusInfo/Num_Lv_After").GetComponent<PguiTextCtrl>();
			this.Num_Before = new List<PguiTextCtrl>
			{
				baseTr.Find("All/BasePhoto/StatusInfo/Contents01/Num_Before").GetComponent<PguiTextCtrl>(),
				baseTr.Find("All/BasePhoto/StatusInfo/Contents02/Num_Before").GetComponent<PguiTextCtrl>(),
				baseTr.Find("All/BasePhoto/StatusInfo/Contents03/Num_Before").GetComponent<PguiTextCtrl>()
			};
			this.Num_After = new List<PguiTextCtrl>
			{
				baseTr.Find("All/BasePhoto/StatusInfo/Contents01/Num_After").GetComponent<PguiTextCtrl>(),
				baseTr.Find("All/BasePhoto/StatusInfo/Contents02/Num_After").GetComponent<PguiTextCtrl>(),
				baseTr.Find("All/BasePhoto/StatusInfo/Contents03/Num_After").GetComponent<PguiTextCtrl>()
			};
			this.ScrollView = baseTr.Find("All/PhotoAll/ScrollView").GetComponent<ReuseScroll>();
			this.OwnCoin = baseTr.Find("All/UseInfo/OwnCoin/Num").GetComponent<PguiTextCtrl>();
			this.Num_Coin = baseTr.Find("All/UseInfo/UseCoin/Num").GetComponent<PguiTextCtrl>();
			this.baseIconPhotoCtrl = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Photo, baseTr.Find("All/BasePhoto/StatusInfo/Icon_Photo").transform).GetComponent<IconPhotoCtrl>();
			this.Num_Exp_Next = baseTr.Find("All/BasePhoto/StatusInfo/ExpGage/Num_Exp_Next").GetComponent<PguiTextCtrl>();
			this.Gage_Up = baseTr.Find("All/BasePhoto/StatusInfo/ExpGage/Gage_Up").GetComponent<PguiImageCtrl>();
			this.Gage = baseTr.Find("All/BasePhoto/StatusInfo/ExpGage/Gage").GetComponent<PguiImageCtrl>();
			GameObject gameObject = (GameObject)Resources.Load("SelCmn/GUI/Prefab/Icon_PhotoRebirth");
			Transform transform = baseTr.Find("All/BasePhoto/StatusInfo/RebirthIcon_Before");
			Transform transform2 = baseTr.Find("All/BasePhoto/StatusInfo/RebirthIcon_After");
			for (int i = 0; i < 4; i++)
			{
				GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject, transform.transform);
				this.RebirthIcon_Before.Add(gameObject2.transform.Find("Icon_PhotoRebirth_Act").gameObject);
				GameObject gameObject3 = Object.Instantiate<GameObject>(gameObject, transform2.transform);
				this.RebirthIcon_After.Add(gameObject3.transform.Find("Icon_PhotoRebirth_Act").gameObject);
			}
			this.Num_SelPhoto = baseTr.Find("All/PhotoAll/Num_SelPhoto").GetComponent<PguiTextCtrl>();
			this.SelCmn_AllInOut = baseTr.GetComponent<SimpleAnimation>();
			this.Campaign = baseTr.Find("All/PhotoAll/Campaign/SelCmn_CampaignInfo").gameObject;
			PguiTextCtrl component = this.Campaign.transform.Find("Txt_Campaign").GetComponent<PguiTextCtrl>();
			this.CampaignTimeText = this.Campaign.transform.Find("TimeInfo/Num_Time").GetComponent<PguiTextCtrl>();
			component.text = component.text.Replace("フレンズ成長", "フォト強化");
			this.sizeChangeBtnGUI = new PhotoUtil.SizeChangeBtnGUI(baseTr.Find("All/PhotoAll/SortFilterBtnsAll/Btn_SizeChange"));
			this.Btn_Info = baseTr.Find("All/BasePhoto/StatusInfo/Btn_Info").GetComponent<PguiButtonCtrl>();
			this.infoPhotoItemEffectCtrl = baseTr.Find("All/BasePhoto/StatusInfo/ItemEffect/Info_PhotoItemEffect").GetComponent<InfoPhotoItemEffectCtrl>();
			this.Num_Percents = new List<PguiTextCtrl>
			{
				baseTr.Find("All/BasePhoto/StatusInfo/ItemEffect/UpInfo/Num_Percent01").GetComponent<PguiTextCtrl>(),
				baseTr.Find("All/BasePhoto/StatusInfo/ItemEffect/UpInfo/Num_Percent02").GetComponent<PguiTextCtrl>()
			};
			this.Txt_None = baseTr.Find("All/PhotoAll/Txt_None").gameObject;
			this.Txt_None.SetActive(false);
		}

		public void ResizeScrollView(int count, int resize)
		{
			this.Txt_None.SetActive(count <= 0);
			this.ScrollView.ResizeFocesNoMove(1 + resize);
		}

		public void InitializeCampaignInfo()
		{
			this.campaignGrowData = DataManager.DmCampaign.PresentCampaignGrowPhotoData;
			this.UpdateCampaignInfo();
		}

		public void UpdateCampaignInfo()
		{
			if (this.campaignGrowData == null)
			{
				this.Campaign.SetActive(false);
				return;
			}
			if (DataManager.DmCampaign.PresentCampaignGrowPhotoData == null || this.campaignGrowData.campaignId != DataManager.DmCampaign.PresentCampaignGrowPhotoData.campaignId)
			{
				this.campaignGrowData = null;
				return;
			}
			bool isDisplayCampaign = this.campaignGrowData.IsDisplayCampaign;
			this.Campaign.SetActive(isDisplayCampaign);
			if (isDisplayCampaign)
			{
				this.CampaignTimeText.text = TimeManager.MakeTimeResidueText(TimeManager.Now, this.campaignGrowData.endDatetime, true, true);
			}
		}

		public static readonly int SCROLL_ITEM_NUN_H = 6;

		public GameObject baseObj;

		public PguiButtonCtrl Btn_FilterOnOff;

		public PguiButtonCtrl Btn_Sort;

		public PguiButtonCtrl Btn_SortUpDown;

		public PguiButtonCtrl ButtonL;

		public PguiButtonCtrl ButtonC;

		public PguiButtonCtrl ButtonR;

		public PguiTextCtrl Num_Lv_Before;

		public PguiTextCtrl Num_Lv_After;

		public PguiTextCtrl Num_Exp_Next;

		public List<PguiTextCtrl> Num_Before;

		public List<PguiTextCtrl> Num_After;

		public PguiTextCtrl OwnCoin;

		public PguiTextCtrl Num_Coin;

		public ReuseScroll ScrollView;

		public IconPhotoCtrl baseIconPhotoCtrl;

		public PguiImageCtrl Gage_Up;

		public PguiImageCtrl Gage;

		public PguiTextCtrl Num_SelPhoto;

		public SimpleAnimation SelCmn_AllInOut;

		public List<GameObject> RebirthIcon_Before = new List<GameObject>();

		public List<GameObject> RebirthIcon_After = new List<GameObject>();

		public GameObject Campaign;

		private PguiTextCtrl CampaignTimeText;

		private DataManagerCampaign.CampaignGrowData campaignGrowData;

		public PhotoUtil.SizeChangeBtnGUI sizeChangeBtnGUI;

		public PguiButtonCtrl Btn_Info;

		public InfoPhotoItemEffectCtrl infoPhotoItemEffectCtrl;

		public List<PguiTextCtrl> Num_Percents;

		public GameObject Txt_None;
	}

	public class PhotoGrowConfirmWindow
	{
		public PhotoGrowConfirmWindow(Transform baseTr)
		{
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.MassageText = baseTr.Find("Base/Window/Massage").GetComponent<PguiTextCtrl>();
			this.MassageCautionText = baseTr.Find("Base/Window/Massage_Caution").GetComponent<PguiTextCtrl>();
			this.Num_CoinUse = baseTr.Find("Base/Window/ItemUse/Num").GetComponent<PguiTextCtrl>();
			this.Num_CoinOwn = baseTr.Find("Base/Window/ItemOwn/Num").GetComponent<PguiTextCtrl>();
			this.ScrollView = baseTr.Find("Base/Window/PhotoUseInfo/ScrollView_PhotoIconAll").GetComponent<ReuseScroll>();
			this.RightFukidasiBase = baseTr.Find("Base/Window/tex_Fukidashi_Right").gameObject;
			this.RightFukidasiText = baseTr.Find("Base/Window/tex_Fukidashi_Right/Txt_Info").GetComponent<PguiTextCtrl>();
			this.LeftFukidasiBase = baseTr.Find("Base/Window/tex_Fukidashi_Left").gameObject;
			this.LeftFukidasiText = baseTr.Find("Base/Window/tex_Fukidashi_Left/Txt_Info").GetComponent<PguiTextCtrl>();
		}

		public static readonly int SCROLL_ITEM_NUN_H = 5;

		public PguiOpenWindowCtrl owCtrl;

		public PguiTextCtrl MassageText;

		public PguiTextCtrl MassageCautionText;

		public PguiTextCtrl Num_CoinUse;

		public PguiTextCtrl Num_CoinOwn;

		public ReuseScroll ScrollView;

		public GameObject RightFukidasiBase;

		public PguiTextCtrl RightFukidasiText;

		public GameObject LeftFukidasiBase;

		public PguiTextCtrl LeftFukidasiText;
	}

	public class PhotoGrowAuth
	{
		public PhotoGrowAuth(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.touchPanel = baseTr.Find("TouchPanel").GetComponent<PguiPanel>();
			this.Num_Lv_After = baseTr.Find("ResultWindow/Base/Num_Lv_After").GetComponent<PguiTextCtrl>();
			this.Num_After = new List<PguiTextCtrl>
			{
				baseTr.Find("ResultWindow/Base/Contents01/Num_After").GetComponent<PguiTextCtrl>(),
				baseTr.Find("ResultWindow/Base/Contents02/Num_After").GetComponent<PguiTextCtrl>(),
				baseTr.Find("ResultWindow/Base/Contents03/Num_After").GetComponent<PguiTextCtrl>()
			};
			this.AEImage_Front = baseTr.Find("AEImage_Front").GetComponent<PguiAECtrl>();
			this.AEImage_Back = baseTr.Find("AEImage_Back").GetComponent<PguiAECtrl>();
			this.AEImage_LevelUp = baseTr.Find("ResultWindow/Base/ExpGage/AEImage_LevelUp").GetComponent<PguiAECtrl>();
			this.AEImage_Result = baseTr.Find("Result/AEImage_Result").GetComponent<PguiAECtrl>();
			this.iconPhoto = Object.Instantiate<GameObject>(CanvasManager.RefResource.Card_Photo, baseTr.Find("Card_Photo")).GetComponent<IconPhotoCtrl>();
			GameObject gameObject = (GameObject)Resources.Load("SelCmn/GUI/Prefab/Icon_PhotoRebirth");
			Transform transform = baseTr.Find("ResultWindow/Base/RebirthIcon");
			for (int i = 0; i < 4; i++)
			{
				GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject, transform.transform);
				this.RebirthIcon_After.Add(gameObject2.transform.Find("Icon_PhotoRebirth_Act").gameObject);
			}
			this.Gage = baseTr.Find("ResultWindow/Base/ExpGage/Gage").GetComponent<PguiImageCtrl>();
			this.infoPhotoItemEffectCtrl = baseTr.Find("ResultWindow/Base/ItemEffect/Info_PhotoItemEffect").GetComponent<InfoPhotoItemEffectCtrl>();
			this.Num_Percents = new List<PguiTextCtrl>
			{
				baseTr.Find("ResultWindow/Base/ItemEffect/UpInfo/Num_Percent01").GetComponent<PguiTextCtrl>(),
				baseTr.Find("ResultWindow/Base/ItemEffect/UpInfo/Num_Percent02").GetComponent<PguiTextCtrl>()
			};
		}

		public GameObject baseObj;

		public PguiTextCtrl Num_Lv_After;

		public List<PguiTextCtrl> Num_After;

		public PguiPanel touchPanel;

		public PguiAECtrl AEImage_Front;

		public PguiAECtrl AEImage_Back;

		public PguiAECtrl AEImage_LevelUp;

		public PguiAECtrl AEImage_Result;

		public IconPhotoCtrl iconPhoto;

		public PguiImageCtrl Gage;

		public List<GameObject> RebirthIcon_After = new List<GameObject>();

		public InfoPhotoItemEffectCtrl infoPhotoItemEffectCtrl;

		public List<PguiTextCtrl> Num_Percents;

		public class AuthIcon
		{
			public GameObject Icon_Card;

			public IconPhotoCtrl IconCardCtrl;

			public PguiAECtrl AEImage;
		}
	}

	public class WindowPhotoCharacteristic
	{
		public WindowPhotoCharacteristic(Transform baseTr)
		{
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.baseObj = baseTr.gameObject;
			this.Txt_PhotoName_Before = baseTr.Find("Base/Window/PhotoInfo_Before/TitleBase/Txt_PhotoName").GetComponent<PguiTextCtrl>();
			this.Txt_Info_Before = baseTr.Find("Base/Window/PhotoInfo_Before/Txt_Info").GetComponent<PguiTextCtrl>();
			this.Txt_PhotoName_After = baseTr.Find("Base/Window/PhotoInfo_After/TitleBase/Txt_PhotoName").GetComponent<PguiTextCtrl>();
			this.Txt_Info_After = baseTr.Find("Base/Window/PhotoInfo_After/Txt_Info").GetComponent<PguiTextCtrl>();
		}

		public GameObject baseObj;

		public PguiOpenWindowCtrl owCtrl;

		public PguiTextCtrl Txt_PhotoName_Before;

		public PguiTextCtrl Txt_Info_Before;

		public PguiTextCtrl Txt_PhotoName_After;

		public PguiTextCtrl Txt_Info_After;
	}

	private class GUI
	{
		public GUI(Transform baseTr)
		{
			GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneCharaEdit/GUI/Prefab/GUI_PhotoGrow"), baseTr);
			this.photoGrowTop = new SelPhotoGrowCtrl.PhotoGrowTop(gameObject.transform.Find("PhotoGrow_Top"));
			this.photoGrowMain = new SelPhotoGrowCtrl.PhotoGrowMain(gameObject.transform.Find("PhotoGrow_Main"));
			GameObject gameObject2 = (GameObject)Resources.Load("SceneCharaEdit/GUI/Prefab/GUI_PhotoGrow_Window");
			this.photoGrowConfirmWindow = new SelPhotoGrowCtrl.PhotoGrowConfirmWindow(Object.Instantiate<Transform>(gameObject2.transform.Find("Window_PhotoGrow"), gameObject.transform).transform);
			this.photoGrowSelectWindow = new PhotoGrowSelectWindowCtrl(Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneCharaEdit/GUI/Prefab/GUI_FilterWindow_PhotoGrow"), gameObject.transform).transform);
			this.photoCharacteristicWindow = new SelPhotoGrowCtrl.WindowPhotoCharacteristic(Object.Instantiate<Transform>(gameObject2.transform.Find("Window_PhotoTokusei"), gameObject.transform).transform);
			this.photoGrowMain.baseObj.SetActive(false);
			this.photoGrowTop.baseObj.SetActive(false);
		}

		public SelPhotoGrowCtrl.PhotoGrowTop photoGrowTop;

		public SelPhotoGrowCtrl.PhotoGrowMain photoGrowMain;

		public GameObject baseObj;

		public Dictionary<GameObject, SelPhotoEditCtrl.GUI.IconPhotoSet> topSelectPhotoIcon = new Dictionary<GameObject, SelPhotoEditCtrl.GUI.IconPhotoSet>();

		public Dictionary<GameObject, SelPhotoEditCtrl.GUI.IconPhotoSet> reservePhotoIcon = new Dictionary<GameObject, SelPhotoEditCtrl.GUI.IconPhotoSet>();

		public SelPhotoGrowCtrl.PhotoGrowConfirmWindow photoGrowConfirmWindow;

		public PhotoGrowSelectWindowCtrl photoGrowSelectWindow;

		public SelPhotoGrowCtrl.PhotoGrowAuth photoGrowAuth;

		public SelPhotoGrowCtrl.WindowPhotoCharacteristic photoCharacteristicWindow;
	}

	private class FeedPhotoPack
	{
		public bool IsDispOverlimits { get; set; }

		public PhotoPackData data;
	}
}
