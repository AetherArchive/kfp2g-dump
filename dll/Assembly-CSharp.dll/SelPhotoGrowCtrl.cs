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

// Token: 0x0200013A RID: 314
public class SelPhotoGrowCtrl : MonoBehaviour
{
	// Token: 0x1700034E RID: 846
	// (get) Token: 0x060010C9 RID: 4297 RVA: 0x000CC115 File Offset: 0x000CA315
	// (set) Token: 0x060010CA RID: 4298 RVA: 0x000CC11D File Offset: 0x000CA31D
	public SelPhotoGrowCtrl.Mode CurrentMode { get; private set; }

	// Token: 0x1700034F RID: 847
	// (get) Token: 0x060010CB RID: 4299 RVA: 0x000CC126 File Offset: 0x000CA326
	// (set) Token: 0x060010CC RID: 4300 RVA: 0x000CC12E File Offset: 0x000CA32E
	private SelPhotoGrowCtrl.PhotoGrowEffectPahse GrowEffectPahse { get; set; }

	// Token: 0x060010CD RID: 4301 RVA: 0x000CC138 File Offset: 0x000CA338
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

	// Token: 0x060010CE RID: 4302 RVA: 0x000CCA98 File Offset: 0x000CAC98
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

	// Token: 0x060010CF RID: 4303 RVA: 0x000CCAE8 File Offset: 0x000CACE8
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

	// Token: 0x060010D0 RID: 4304 RVA: 0x000CCBA8 File Offset: 0x000CADA8
	private void SetupBasePhotoPackData(PhotoPackData ppd)
	{
		this.basePhotoPackData = ppd;
		this.requestMode = SelPhotoGrowCtrl.Mode.GROW_MAIN;
	}

	// Token: 0x060010D1 RID: 4305 RVA: 0x000CCBB8 File Offset: 0x000CADB8
	public void SetupBySceneForce(long photoId)
	{
		this.SetupBasePhotoPackData(DataManager.DmPhoto.GetUserPhotoData(photoId));
		this.CurrentMode = SelPhotoGrowCtrl.Mode.PHOTO_SELECT;
	}

	// Token: 0x060010D2 RID: 4306 RVA: 0x000CCBD2 File Offset: 0x000CADD2
	public void Dest()
	{
		this.ClearFeedPhotoPackList();
	}

	// Token: 0x060010D3 RID: 4307 RVA: 0x000CCBDA File Offset: 0x000CADDA
	public void ReloadDataManager()
	{
		this.<ReloadDataManager>g__SetupPhotoList|48_0();
		this.ReloadDeckPhotoIdList();
	}

	// Token: 0x060010D4 RID: 4308 RVA: 0x000CCBE8 File Offset: 0x000CADE8
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

	// Token: 0x060010D5 RID: 4309 RVA: 0x000CCF1C File Offset: 0x000CB11C
	private void ExecSort()
	{
		CanvasManager.HdlOpenWindowSortFilter.SolutionList(SortFilterDefine.RegisterType.PHOTO_GROW_TOP, null);
		CanvasManager.HdlOpenWindowSortFilter.SolutionList(SortFilterDefine.RegisterType.PHOTO_GROW_MAIN, null);
	}

	// Token: 0x060010D6 RID: 4310 RVA: 0x000CCF38 File Offset: 0x000CB138
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

	// Token: 0x060010D7 RID: 4311 RVA: 0x000CCF70 File Offset: 0x000CB170
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

	// Token: 0x060010D8 RID: 4312 RVA: 0x000CD150 File Offset: 0x000CB350
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

	// Token: 0x060010D9 RID: 4313 RVA: 0x000CD51C File Offset: 0x000CB71C
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

	// Token: 0x060010DA RID: 4314 RVA: 0x000CD58C File Offset: 0x000CB78C
	private void OnUpdatePhotoLock(IconPhotoCtrl iconPhoto)
	{
		if (iconPhoto.photoPackData.dynamicData.lockFlag && this.feedPhotoPackList.Contains(iconPhoto.photoPackData))
		{
			this.OnTouchSelectMaterialPhotoIcon(iconPhoto);
		}
		this.guiData.photoGrowMain.ScrollView.Refresh();
	}

	// Token: 0x060010DB RID: 4315 RVA: 0x000CD5DC File Offset: 0x000CB7DC
	private void OnUpdatePhotoFavorite(IconPhotoCtrl iconPhoto)
	{
		if (iconPhoto.photoPackData.dynamicData.favoriteFlag && this.feedPhotoPackList.Contains(iconPhoto.photoPackData))
		{
			this.OnTouchSelectMaterialPhotoIcon(iconPhoto);
		}
		this.guiData.photoGrowMain.ScrollView.Refresh();
	}

	// Token: 0x060010DC RID: 4316 RVA: 0x000CD62C File Offset: 0x000CB82C
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

	// Token: 0x060010DD RID: 4317 RVA: 0x000CD67C File Offset: 0x000CB87C
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

	// Token: 0x060010DE RID: 4318 RVA: 0x000CD8FC File Offset: 0x000CBAFC
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

	// Token: 0x060010DF RID: 4319 RVA: 0x000CD914 File Offset: 0x000CBB14
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

	// Token: 0x060010E0 RID: 4320 RVA: 0x000CE040 File Offset: 0x000CC240
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

	// Token: 0x060010E1 RID: 4321 RVA: 0x000CE22C File Offset: 0x000CC42C
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

	// Token: 0x060010E2 RID: 4322 RVA: 0x000CE3F0 File Offset: 0x000CC5F0
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

	// Token: 0x060010E3 RID: 4323 RVA: 0x000CE614 File Offset: 0x000CC814
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

	// Token: 0x060010E4 RID: 4324 RVA: 0x000CEAF8 File Offset: 0x000CCCF8
	private bool CheckBasePhoto()
	{
		return this.guiData.photoGrowMain.baseIconPhotoCtrl.photoPackData != null && this.guiData.photoGrowMain.baseIconPhotoCtrl.photoPackData.staticData != null && !this.guiData.photoGrowMain.baseIconPhotoCtrl.photoPackData.IsInvalid();
	}

	// Token: 0x060010E5 RID: 4325 RVA: 0x000CEB58 File Offset: 0x000CCD58
	private bool CanLevelLimitOver(PhotoPackData ppd)
	{
		return ppd != null && this.CheckBasePhoto() && (this.guiData.photoGrowMain.baseIconPhotoCtrl.photoPackData.staticData.GetId() == ppd.staticData.GetId() || (PhotoUtil.IsLevelLimitOverPhoto(ppd) && ppd.staticData.GetRarity() >= this.guiData.photoGrowMain.baseIconPhotoCtrl.photoPackData.staticData.GetRarity())) && this.guiData.photoGrowMain.baseIconPhotoCtrl.photoPackData.dynamicData.levelRank < 4;
	}

	// Token: 0x060010E6 RID: 4326 RVA: 0x000CEBFC File Offset: 0x000CCDFC
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

	// Token: 0x060010E7 RID: 4327 RVA: 0x000CEC50 File Offset: 0x000CCE50
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

	// Token: 0x060010E8 RID: 4328 RVA: 0x000CECC4 File Offset: 0x000CCEC4
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

	// Token: 0x060010E9 RID: 4329 RVA: 0x000CED68 File Offset: 0x000CCF68
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

	// Token: 0x060010EA RID: 4330 RVA: 0x000CEE2C File Offset: 0x000CD02C
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

	// Token: 0x060010EB RID: 4331 RVA: 0x000CEE9C File Offset: 0x000CD09C
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

	// Token: 0x060010EC RID: 4332 RVA: 0x000CFAEC File Offset: 0x000CDCEC
	private void OpenItemWindow(List<ItemData> getItem, string message, PguiOpenWindowCtrl.Callback closeCallback)
	{
		CanvasManager.HdlGetItemWindowCtrl.Setup(getItem, new GetItemWindowCtrl.SetupParam
		{
			strItemCb = (GetItemWindowCtrl.WordingCallbackParam param) => message,
			windowFinishedCallback = closeCallback
		});
		CanvasManager.HdlGetItemWindowCtrl.Open();
	}

	// Token: 0x060010ED RID: 4333 RVA: 0x000CFB39 File Offset: 0x000CDD39
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

	// Token: 0x060010EE RID: 4334 RVA: 0x000CFB50 File Offset: 0x000CDD50
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

	// Token: 0x060010EF RID: 4335 RVA: 0x000CFDB6 File Offset: 0x000CDFB6
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

	// Token: 0x060010F0 RID: 4336 RVA: 0x000CFDCC File Offset: 0x000CDFCC
	private void ClearFeedPhotoPackList()
	{
		this.feedPhotoPackList.Clear();
	}

	// Token: 0x060010F1 RID: 4337 RVA: 0x000CFDDC File Offset: 0x000CDFDC
	private void SettingGrowMain()
	{
		this.ClearFeedPhotoPackList();
		this.guiData.photoGrowMain.ResizeScrollView(this.dispAllPhotoPackList.Count, this.dispAllPhotoPackList.Count / this.guiData.photoGrowMain.sizeChangeBtnGUI.IconPhotoParamList[this.guiData.photoGrowMain.sizeChangeBtnGUI.SizeIndex].num);
		this.guiData.photoGrowMain.baseIconPhotoCtrl.Setup(this.basePhotoPackData, SortFilterDefine.SortType.LEVEL, true, false, -1, false);
		this.guiData.photoGrowMain.Btn_Info.gameObject.SetActive(this.basePhotoPackData.IsEnableSwitchCharacteristic());
		this.SettingGrowMainBySelectInfo();
	}

	// Token: 0x060010F2 RID: 4338 RVA: 0x000CFE95 File Offset: 0x000CE095
	private IEnumerator Wait(float second)
	{
		float timeSinceStartup = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup - timeSinceStartup < second)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060010F3 RID: 4339 RVA: 0x000CFEA4 File Offset: 0x000CE0A4
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

	// Token: 0x060010F4 RID: 4340 RVA: 0x000CFED4 File Offset: 0x000CE0D4
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

	// Token: 0x060010F5 RID: 4341 RVA: 0x000CFF2C File Offset: 0x000CE12C
	private void WindowCloseStatusCallBack(List<ItemDef.Rarity> rarityList, List<SortFilterDefine.PhotoLevelType> photoLevelList, List<PhotoDef.Type> photoTypeList)
	{
		this.LatestRarityList = rarityList;
		this.LatestPhotoLevelList = photoLevelList;
		this.LatestPhotoTypeList = photoTypeList;
		this.AutoSelectGrowPhotoList();
	}

	// Token: 0x060010F6 RID: 4342 RVA: 0x000CFF4C File Offset: 0x000CE14C
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

	// Token: 0x060010F7 RID: 4343 RVA: 0x000D0074 File Offset: 0x000CE274
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

	// Token: 0x06001105 RID: 4357 RVA: 0x000D0808 File Offset: 0x000CEA08
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

	// Token: 0x0600110F RID: 4367 RVA: 0x000D0AA4 File Offset: 0x000CECA4
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

	// Token: 0x04000E6F RID: 3695
	private static readonly string UP_PARAM_COLOR_CODE = "#FF7C17FF";

	// Token: 0x04000E70 RID: 3696
	private static readonly string NORMAL_PARAM_COLOR_CODE = "#533C06FF";

	// Token: 0x04000E71 RID: 3697
	private static readonly Color UP_PARAM_COLOR = new Color32(byte.MaxValue, 124, 23, byte.MaxValue);

	// Token: 0x04000E72 RID: 3698
	private static readonly Color NORMAL_PARAM_COLOR = new Color32(83, 60, 6, byte.MaxValue);

	// Token: 0x04000E73 RID: 3699
	private SelPhotoGrowCtrl.Mode requestMode;

	// Token: 0x04000E75 RID: 3701
	private SelPhotoGrowCtrl.SetupParam setupParam = new SelPhotoGrowCtrl.SetupParam();

	// Token: 0x04000E76 RID: 3702
	private SelPhotoGrowCtrl.GUI guiData;

	// Token: 0x04000E77 RID: 3703
	private SortFilterDefine.SortType sortTypeTop = SortFilterDefine.SortType.LEVEL;

	// Token: 0x04000E78 RID: 3704
	private SortFilterDefine.SortType sortTypeMain = SortFilterDefine.SortType.LEVEL;

	// Token: 0x04000E79 RID: 3705
	private List<PhotoPackData> haveAllPhotoPackList = new List<PhotoPackData>();

	// Token: 0x04000E7A RID: 3706
	private List<PhotoPackData> havePhotoPackList = new List<PhotoPackData>();

	// Token: 0x04000E7B RID: 3707
	private List<PhotoPackData> dispAllPhotoPackList = new List<PhotoPackData>();

	// Token: 0x04000E7C RID: 3708
	private List<PhotoPackData> dispPhotoPackList = new List<PhotoPackData>();

	// Token: 0x04000E7D RID: 3709
	private PhotoPackData basePhotoPackData;

	// Token: 0x04000E7E RID: 3710
	private List<PhotoPackData> feedPhotoPackList = new List<PhotoPackData>();

	// Token: 0x04000E7F RID: 3711
	private List<SelPhotoGrowCtrl.FeedPhotoPack> _feedPhotoPackList;

	// Token: 0x04000E80 RID: 3712
	private List<long> deckPhotoDataIdList = new List<long>();

	// Token: 0x04000E81 RID: 3713
	private bool simulateGrowLvMax;

	// Token: 0x04000E82 RID: 3714
	private bool SimulateGrowBreakLimitMax;

	// Token: 0x04000E83 RID: 3715
	private UserOptionData cloneUserOptionData;

	// Token: 0x04000E84 RID: 3716
	private List<ItemDef.Rarity> LatestRarityList;

	// Token: 0x04000E85 RID: 3717
	private List<SortFilterDefine.PhotoLevelType> LatestPhotoLevelList;

	// Token: 0x04000E86 RID: 3718
	private List<PhotoDef.Type> LatestPhotoTypeList;

	// Token: 0x04000E88 RID: 3720
	private List<PhotoPackData> optionPhotoPacks = new List<PhotoPackData>();

	// Token: 0x04000E89 RID: 3721
	private bool effectChangeAction;

	// Token: 0x04000E8A RID: 3722
	private IEnumerator growGageEffect;

	// Token: 0x04000E8B RID: 3723
	private IEnumerator serverRequestGrow;

	// Token: 0x02000A46 RID: 2630
	public class SetupParam
	{
		// Token: 0x040041A0 RID: 16800
		public SceneCharaEdit.OnReturnSceneName onReturnSceneNameCB;
	}

	// Token: 0x02000A47 RID: 2631
	public enum FrameType
	{
		// Token: 0x040041A2 RID: 16802
		INVALID,
		// Token: 0x040041A3 RID: 16803
		PHOTO_SELECT,
		// Token: 0x040041A4 RID: 16804
		GROW_MAIN
	}

	// Token: 0x02000A48 RID: 2632
	public enum Mode
	{
		// Token: 0x040041A6 RID: 16806
		INVALID,
		// Token: 0x040041A7 RID: 16807
		PHOTO_SELECT,
		// Token: 0x040041A8 RID: 16808
		GROW_MAIN,
		// Token: 0x040041A9 RID: 16809
		OW_CONFIRM,
		// Token: 0x040041AA RID: 16810
		SERVER_REQUEST_GROW,
		// Token: 0x040041AB RID: 16811
		GROW_EFFECT
	}

	// Token: 0x02000A49 RID: 2633
	public enum PhotoGrowEffectPahse
	{
		// Token: 0x040041AD RID: 16813
		None,
		// Token: 0x040041AE RID: 16814
		Initialize,
		// Token: 0x040041AF RID: 16815
		GageUp,
		// Token: 0x040041B0 RID: 16816
		GrowResult,
		// Token: 0x040041B1 RID: 16817
		BreakthroughLimitMax,
		// Token: 0x040041B2 RID: 16818
		GrowthMax,
		// Token: 0x040041B3 RID: 16819
		ReturnPhoto,
		// Token: 0x040041B4 RID: 16820
		TouchWait,
		// Token: 0x040041B5 RID: 16821
		Refresh
	}

	// Token: 0x02000A4A RID: 2634
	public class PhotoGrowTop
	{
		// Token: 0x06003EB2 RID: 16050 RVA: 0x001EB92C File Offset: 0x001E9B2C
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

		// Token: 0x06003EB3 RID: 16051 RVA: 0x001EBA67 File Offset: 0x001E9C67
		public void ResizeScrollView(int count, int resize)
		{
			this.Txt_None.SetActive(count <= 0);
			this.ScrollView.ResizeFocesNoMove(1 + resize);
		}

		// Token: 0x06003EB4 RID: 16052 RVA: 0x001EBA89 File Offset: 0x001E9C89
		public void InitializeCampaignInfo()
		{
			this.campaignGrowData = DataManager.DmCampaign.PresentCampaignGrowPhotoData;
			this.UpdateCampaignInfo();
		}

		// Token: 0x06003EB5 RID: 16053 RVA: 0x001EBAA4 File Offset: 0x001E9CA4
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

		// Token: 0x040041B6 RID: 16822
		public static readonly int SCROLL_ITEM_NUN_H = 6;

		// Token: 0x040041B7 RID: 16823
		public GameObject baseObj;

		// Token: 0x040041B8 RID: 16824
		public PguiButtonCtrl Btn_FilterOnOff;

		// Token: 0x040041B9 RID: 16825
		public PguiButtonCtrl Btn_Sort;

		// Token: 0x040041BA RID: 16826
		public PguiButtonCtrl Btn_SortUpDown;

		// Token: 0x040041BB RID: 16827
		public ReuseScroll ScrollView;

		// Token: 0x040041BC RID: 16828
		public PguiTextCtrl Num_Own;

		// Token: 0x040041BD RID: 16829
		public SimpleAnimation SelCmn_AllInOut;

		// Token: 0x040041BE RID: 16830
		public GameObject Campaign;

		// Token: 0x040041BF RID: 16831
		private PguiTextCtrl CampaignTimeText;

		// Token: 0x040041C0 RID: 16832
		private DataManagerCampaign.CampaignGrowData campaignGrowData;

		// Token: 0x040041C1 RID: 16833
		public PhotoUtil.SizeChangeBtnGUI sizeChangeBtnGUI;

		// Token: 0x040041C2 RID: 16834
		public GameObject Txt_None;
	}

	// Token: 0x02000A4B RID: 2635
	public class PhotoGrowMain
	{
		// Token: 0x06003EB7 RID: 16055 RVA: 0x001EBB3C File Offset: 0x001E9D3C
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

		// Token: 0x06003EB8 RID: 16056 RVA: 0x001EBF22 File Offset: 0x001EA122
		public void ResizeScrollView(int count, int resize)
		{
			this.Txt_None.SetActive(count <= 0);
			this.ScrollView.ResizeFocesNoMove(1 + resize);
		}

		// Token: 0x06003EB9 RID: 16057 RVA: 0x001EBF44 File Offset: 0x001EA144
		public void InitializeCampaignInfo()
		{
			this.campaignGrowData = DataManager.DmCampaign.PresentCampaignGrowPhotoData;
			this.UpdateCampaignInfo();
		}

		// Token: 0x06003EBA RID: 16058 RVA: 0x001EBF5C File Offset: 0x001EA15C
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

		// Token: 0x040041C3 RID: 16835
		public static readonly int SCROLL_ITEM_NUN_H = 6;

		// Token: 0x040041C4 RID: 16836
		public GameObject baseObj;

		// Token: 0x040041C5 RID: 16837
		public PguiButtonCtrl Btn_FilterOnOff;

		// Token: 0x040041C6 RID: 16838
		public PguiButtonCtrl Btn_Sort;

		// Token: 0x040041C7 RID: 16839
		public PguiButtonCtrl Btn_SortUpDown;

		// Token: 0x040041C8 RID: 16840
		public PguiButtonCtrl ButtonL;

		// Token: 0x040041C9 RID: 16841
		public PguiButtonCtrl ButtonC;

		// Token: 0x040041CA RID: 16842
		public PguiButtonCtrl ButtonR;

		// Token: 0x040041CB RID: 16843
		public PguiTextCtrl Num_Lv_Before;

		// Token: 0x040041CC RID: 16844
		public PguiTextCtrl Num_Lv_After;

		// Token: 0x040041CD RID: 16845
		public PguiTextCtrl Num_Exp_Next;

		// Token: 0x040041CE RID: 16846
		public List<PguiTextCtrl> Num_Before;

		// Token: 0x040041CF RID: 16847
		public List<PguiTextCtrl> Num_After;

		// Token: 0x040041D0 RID: 16848
		public PguiTextCtrl OwnCoin;

		// Token: 0x040041D1 RID: 16849
		public PguiTextCtrl Num_Coin;

		// Token: 0x040041D2 RID: 16850
		public ReuseScroll ScrollView;

		// Token: 0x040041D3 RID: 16851
		public IconPhotoCtrl baseIconPhotoCtrl;

		// Token: 0x040041D4 RID: 16852
		public PguiImageCtrl Gage_Up;

		// Token: 0x040041D5 RID: 16853
		public PguiImageCtrl Gage;

		// Token: 0x040041D6 RID: 16854
		public PguiTextCtrl Num_SelPhoto;

		// Token: 0x040041D7 RID: 16855
		public SimpleAnimation SelCmn_AllInOut;

		// Token: 0x040041D8 RID: 16856
		public List<GameObject> RebirthIcon_Before = new List<GameObject>();

		// Token: 0x040041D9 RID: 16857
		public List<GameObject> RebirthIcon_After = new List<GameObject>();

		// Token: 0x040041DA RID: 16858
		public GameObject Campaign;

		// Token: 0x040041DB RID: 16859
		private PguiTextCtrl CampaignTimeText;

		// Token: 0x040041DC RID: 16860
		private DataManagerCampaign.CampaignGrowData campaignGrowData;

		// Token: 0x040041DD RID: 16861
		public PhotoUtil.SizeChangeBtnGUI sizeChangeBtnGUI;

		// Token: 0x040041DE RID: 16862
		public PguiButtonCtrl Btn_Info;

		// Token: 0x040041DF RID: 16863
		public InfoPhotoItemEffectCtrl infoPhotoItemEffectCtrl;

		// Token: 0x040041E0 RID: 16864
		public List<PguiTextCtrl> Num_Percents;

		// Token: 0x040041E1 RID: 16865
		public GameObject Txt_None;
	}

	// Token: 0x02000A4C RID: 2636
	public class PhotoGrowConfirmWindow
	{
		// Token: 0x06003EBC RID: 16060 RVA: 0x001EBFF4 File Offset: 0x001EA1F4
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

		// Token: 0x040041E2 RID: 16866
		public static readonly int SCROLL_ITEM_NUN_H = 5;

		// Token: 0x040041E3 RID: 16867
		public PguiOpenWindowCtrl owCtrl;

		// Token: 0x040041E4 RID: 16868
		public PguiTextCtrl MassageText;

		// Token: 0x040041E5 RID: 16869
		public PguiTextCtrl MassageCautionText;

		// Token: 0x040041E6 RID: 16870
		public PguiTextCtrl Num_CoinUse;

		// Token: 0x040041E7 RID: 16871
		public PguiTextCtrl Num_CoinOwn;

		// Token: 0x040041E8 RID: 16872
		public ReuseScroll ScrollView;

		// Token: 0x040041E9 RID: 16873
		public GameObject RightFukidasiBase;

		// Token: 0x040041EA RID: 16874
		public PguiTextCtrl RightFukidasiText;

		// Token: 0x040041EB RID: 16875
		public GameObject LeftFukidasiBase;

		// Token: 0x040041EC RID: 16876
		public PguiTextCtrl LeftFukidasiText;
	}

	// Token: 0x02000A4D RID: 2637
	public class PhotoGrowAuth
	{
		// Token: 0x06003EBE RID: 16062 RVA: 0x001EC0E4 File Offset: 0x001EA2E4
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

		// Token: 0x040041ED RID: 16877
		public GameObject baseObj;

		// Token: 0x040041EE RID: 16878
		public PguiTextCtrl Num_Lv_After;

		// Token: 0x040041EF RID: 16879
		public List<PguiTextCtrl> Num_After;

		// Token: 0x040041F0 RID: 16880
		public PguiPanel touchPanel;

		// Token: 0x040041F1 RID: 16881
		public PguiAECtrl AEImage_Front;

		// Token: 0x040041F2 RID: 16882
		public PguiAECtrl AEImage_Back;

		// Token: 0x040041F3 RID: 16883
		public PguiAECtrl AEImage_LevelUp;

		// Token: 0x040041F4 RID: 16884
		public PguiAECtrl AEImage_Result;

		// Token: 0x040041F5 RID: 16885
		public IconPhotoCtrl iconPhoto;

		// Token: 0x040041F6 RID: 16886
		public PguiImageCtrl Gage;

		// Token: 0x040041F7 RID: 16887
		public List<GameObject> RebirthIcon_After = new List<GameObject>();

		// Token: 0x040041F8 RID: 16888
		public InfoPhotoItemEffectCtrl infoPhotoItemEffectCtrl;

		// Token: 0x040041F9 RID: 16889
		public List<PguiTextCtrl> Num_Percents;

		// Token: 0x0200116E RID: 4462
		public class AuthIcon
		{
			// Token: 0x04005FB3 RID: 24499
			public GameObject Icon_Card;

			// Token: 0x04005FB4 RID: 24500
			public IconPhotoCtrl IconCardCtrl;

			// Token: 0x04005FB5 RID: 24501
			public PguiAECtrl AEImage;
		}
	}

	// Token: 0x02000A4E RID: 2638
	public class WindowPhotoCharacteristic
	{
		// Token: 0x06003EBF RID: 16063 RVA: 0x001EC2BC File Offset: 0x001EA4BC
		public WindowPhotoCharacteristic(Transform baseTr)
		{
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.baseObj = baseTr.gameObject;
			this.Txt_PhotoName_Before = baseTr.Find("Base/Window/PhotoInfo_Before/TitleBase/Txt_PhotoName").GetComponent<PguiTextCtrl>();
			this.Txt_Info_Before = baseTr.Find("Base/Window/PhotoInfo_Before/Txt_Info").GetComponent<PguiTextCtrl>();
			this.Txt_PhotoName_After = baseTr.Find("Base/Window/PhotoInfo_After/TitleBase/Txt_PhotoName").GetComponent<PguiTextCtrl>();
			this.Txt_Info_After = baseTr.Find("Base/Window/PhotoInfo_After/Txt_Info").GetComponent<PguiTextCtrl>();
		}

		// Token: 0x040041FA RID: 16890
		public GameObject baseObj;

		// Token: 0x040041FB RID: 16891
		public PguiOpenWindowCtrl owCtrl;

		// Token: 0x040041FC RID: 16892
		public PguiTextCtrl Txt_PhotoName_Before;

		// Token: 0x040041FD RID: 16893
		public PguiTextCtrl Txt_Info_Before;

		// Token: 0x040041FE RID: 16894
		public PguiTextCtrl Txt_PhotoName_After;

		// Token: 0x040041FF RID: 16895
		public PguiTextCtrl Txt_Info_After;
	}

	// Token: 0x02000A4F RID: 2639
	private class GUI
	{
		// Token: 0x06003EC0 RID: 16064 RVA: 0x001EC340 File Offset: 0x001EA540
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

		// Token: 0x04004200 RID: 16896
		public SelPhotoGrowCtrl.PhotoGrowTop photoGrowTop;

		// Token: 0x04004201 RID: 16897
		public SelPhotoGrowCtrl.PhotoGrowMain photoGrowMain;

		// Token: 0x04004202 RID: 16898
		public GameObject baseObj;

		// Token: 0x04004203 RID: 16899
		public Dictionary<GameObject, SelPhotoEditCtrl.GUI.IconPhotoSet> topSelectPhotoIcon = new Dictionary<GameObject, SelPhotoEditCtrl.GUI.IconPhotoSet>();

		// Token: 0x04004204 RID: 16900
		public Dictionary<GameObject, SelPhotoEditCtrl.GUI.IconPhotoSet> reservePhotoIcon = new Dictionary<GameObject, SelPhotoEditCtrl.GUI.IconPhotoSet>();

		// Token: 0x04004205 RID: 16901
		public SelPhotoGrowCtrl.PhotoGrowConfirmWindow photoGrowConfirmWindow;

		// Token: 0x04004206 RID: 16902
		public PhotoGrowSelectWindowCtrl photoGrowSelectWindow;

		// Token: 0x04004207 RID: 16903
		public SelPhotoGrowCtrl.PhotoGrowAuth photoGrowAuth;

		// Token: 0x04004208 RID: 16904
		public SelPhotoGrowCtrl.WindowPhotoCharacteristic photoCharacteristicWindow;
	}

	// Token: 0x02000A50 RID: 2640
	private class FeedPhotoPack
	{
		// Token: 0x17000931 RID: 2353
		// (get) Token: 0x06003EC1 RID: 16065 RVA: 0x001EC467 File Offset: 0x001EA667
		// (set) Token: 0x06003EC2 RID: 16066 RVA: 0x001EC46F File Offset: 0x001EA66F
		public bool IsDispOverlimits { get; set; }

		// Token: 0x04004209 RID: 16905
		public PhotoPackData data;
	}
}
