using System;
using System.Collections;
using System.Collections.Generic;
using AEAuth3;
using SGNFW.Common;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelPhotoSellCtrl : MonoBehaviour
{
	public void Init()
	{
		GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneCharaEdit/GUI/Prefab/GUI_PhotoSell"), base.transform);
		this.cloneUserOptionData = DataManager.DmUserInfo.optionData.Clone();
		this.guiData = new SelPhotoSellCtrl.GUI(gameObject.transform);
		this.guiData.photoSellTopGUI.sizeChangeBtnGUI.Setup(new PhotoUtil.SizeChangeBtnGUI.SetupParam
		{
			funcResult = delegate(PhotoUtil.SizeChangeBtnGUI.ResultParam result)
			{
				this.cloneUserOptionData.photoIconSizeSell = result.sizeIndex;
				DataManager.DmUserInfo.RequestActionUpdateUserOption(this.cloneUserOptionData);
			},
			iconPhotoParamList = new List<PhotoUtil.SizeChangeBtnGUI.IconPhotoParam>
			{
				new PhotoUtil.SizeChangeBtnGUI.IconPhotoParam
				{
					scale = new Vector3(0.6f, 0.6f, 1f),
					scaleCurrent = new Vector3(1f, 1f, 1f),
					scaleCount = new Vector3(1f, 1f, 1f),
					num = 10,
					prefab = Object.Instantiate<GameObject>((GameObject)Resources.Load("SelCmn/GUI/Prefab/CharaPhotoAll_Icon_Photo_List_S"), base.transform)
				},
				new PhotoUtil.SizeChangeBtnGUI.IconPhotoParam
				{
					scale = new Vector3(0.75f, 0.75f, 1f),
					scaleCurrent = new Vector3(1f, 1f, 1f),
					scaleCount = new Vector3(1f, 1f, 1f),
					num = 8,
					prefab = Object.Instantiate<GameObject>((GameObject)Resources.Load("SelCmn/GUI/Prefab/CharaPhotoAll_Icon_Photo_List_M"), base.transform)
				},
				new PhotoUtil.SizeChangeBtnGUI.IconPhotoParam
				{
					scale = new Vector3(0.85f, 0.85f, 1f),
					scaleCurrent = new Vector3(1f, 1f, 1f),
					scaleCount = new Vector3(1f, 1f, 1f),
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
			refScrollView = this.guiData.photoSellTopGUI.ScrollView,
			sizeIndex = this.cloneUserOptionData.photoIconSizeSell,
			resetCallback = delegate
			{
				this.guiData.reservePhotoIcon.Clear();
			},
			dispIconPhotoCountCallback = () => this.dispPhotoPackList.Count
		});
		if (!this.isDebug)
		{
			SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this.guiData.windowPhotoSell.owCtrl.transform, true);
			this.guiData.windowPhotoSell.owCtrl.transform.SetParent(Singleton<CanvasManager>.Instance.SystemPanel.transform);
			this.guiData.windowPhotoSell.owCtrl.transform.SetSiblingIndex(Singleton<CanvasManager>.Instance.cmnTouchMask.transform.GetSiblingIndex());
			SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this.guiData.windowSelectAll.baseWindow.transform, true);
			this.guiData.windowSelectAll.baseWindow.transform.SetParent(Singleton<CanvasManager>.Instance.SystemPanel.transform);
			this.guiData.windowSelectAll.baseWindow.transform.SetSiblingIndex(Singleton<CanvasManager>.Instance.cmnTouchMask.transform.GetSiblingIndex());
			this.guiData.windowSelectAll.SetActiveSellButton();
			this.guiData.windowSelectAll.SetCategoryText(SortFilterDefine.PhotoFilterType.SellPhoto);
			this.guiData.windowSelectAll.AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickToggleButton));
		}
		this.guiData.photoSellTopGUI.sizeChangeBtnGUI.ResetScrollView();
		this.guiData.photoSellTopGUI.ButtonL.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.photoSellTopGUI.ButtonR.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.photoSellTopGUI.ButtonC.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		SortWindowCtrl.RegisterData registerData = new SortWindowCtrl.RegisterData
		{
			register = SortFilterDefine.RegisterType.PHOTO_SELL_MAIN,
			filterButton = this.guiData.photoSellTopGUI.Btn_FilterOnOff,
			sortButton = this.guiData.photoSellTopGUI.Btn_Sort,
			sortUdButton = this.guiData.photoSellTopGUI.Btn_SortUpDown,
			funcGetTargetBaseList = delegate
			{
				List<PhotoPackData> list = new List<PhotoPackData>();
				List<PhotoPackData> list2 = new List<PhotoPackData>();
				foreach (PhotoPackData photoPackData in this.havePhotoPackList)
				{
					if (photoPackData.dynamicData.lockFlag)
					{
						list.Add(photoPackData);
					}
					else if (photoPackData.dynamicData.favoriteFlag)
					{
						list.Add(photoPackData);
					}
					else if (photoPackData.staticData.baseData.forbiddenDiscardFlg)
					{
						list.Add(photoPackData);
					}
					else
					{
						list2.Add(photoPackData);
					}
				}
				return new SortWindowCtrl.SortTarget
				{
					photoList = list2,
					disableFilterPhotoList = this.sellPhotoPackList,
					lowerDisableSortPhotoList = list
				};
			},
			funcDisideTarget = delegate(SortWindowCtrl.SortTarget item)
			{
				this.dispPhotoPackList = item.photoList;
				this.sortType = item.sortType;
				this.guiData.photoSellTopGUI.ResizeScrollView(this.dispPhotoPackList.Count, 1 + this.dispPhotoPackList.Count / this.guiData.photoSellTopGUI.sizeChangeBtnGUI.IconPhotoParamList[this.guiData.photoSellTopGUI.sizeChangeBtnGUI.SizeIndex].num);
			}
		};
		CanvasManager.HdlOpenWindowSortFilter.Register(registerData, false, null);
	}

	public void Setup()
	{
		this.ReloadDataManager(-1L);
		this.sellPhotoPackList.Clear();
		this.cloneUserOptionData = DataManager.DmUserInfo.optionData.Clone();
		CanvasManager.HdlOpenWindowSortFilter.SolutionList(SortFilterDefine.RegisterType.PHOTO_SELL_MAIN, null);
		this.guiData.photoSellTopGUI.sizeChangeBtnGUI.ResetScrollView();
		this.guiData.photoSellTopGUI.Num_Own.ReplaceTextByDefault("Param01", this.havePhotoPackList.Count.ToString() + "/" + DataManager.DmPhoto.PhotoStockLimit.ToString());
		this.UpdateInfo();
		this.guiData.SelCmn_AllInOut.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
	}

	public void Dest()
	{
		this.Reset();
	}

	public void ReloadDataManager(long baseDataId = -1L)
	{
		this.havePhotoPackList = new List<PhotoPackData>(DataManager.DmPhoto.GetUserPhotoMap().Values);
		this.dispPhotoPackList = new List<PhotoPackData>(this.havePhotoPackList);
		this.deckPhotoDataId.Clear();
		foreach (UserDeckData userDeckData in DataManager.DmDeck.GetUserDeckList(UserDeckData.Category.NORMAL))
		{
			for (int i = 0; i < userDeckData.equipPhotoList.Count; i++)
			{
				for (int j = 0; j < userDeckData.equipPhotoList[j].Count; j++)
				{
					long num = userDeckData.equipPhotoList[i][j];
					if (num > 0L && !this.deckPhotoDataId.Contains(num))
					{
						this.deckPhotoDataId.Add(num);
					}
				}
			}
		}
		foreach (UserDeckData userDeckData2 in DataManager.DmDeck.GetUserDeckList(UserDeckData.Category.PVP))
		{
			for (int k = 0; k < userDeckData2.equipPhotoList.Count; k++)
			{
				for (int l = 0; l < userDeckData2.equipPhotoList[l].Count; l++)
				{
					long num2 = userDeckData2.equipPhotoList[k][l];
					if (num2 > 0L && !this.deckPhotoDataId.Contains(num2))
					{
						this.deckPhotoDataId.Add(num2);
					}
				}
			}
		}
		foreach (UserDeckData userDeckData3 in DataManager.DmDeck.GetUserDeckList(UserDeckData.Category.TRAINING))
		{
			for (int m = 0; m < userDeckData3.equipPhotoList.Count; m++)
			{
				for (int n = 0; n < userDeckData3.equipPhotoList[n].Count; n++)
				{
					long num3 = userDeckData3.equipPhotoList[m][n];
					if (num3 > 0L && !this.deckPhotoDataId.Contains(num3))
					{
						this.deckPhotoDataId.Add(num3);
					}
				}
			}
		}
		foreach (LoanPackData loanPackData in DataManager.DmUserInfo.loanPackList)
		{
			foreach (long num4 in loanPackData.photoDataIdList)
			{
				if (num4 > 0L && !this.deckPhotoDataId.Contains(num4))
				{
					this.deckPhotoDataId.Add(num4);
				}
			}
		}
	}

	private void Start()
	{
	}

	private void OnDestroy()
	{
		if (this.guiData.windowPhotoSell != null)
		{
			Object.Destroy(this.guiData.windowPhotoSell.baseObj);
			this.guiData.windowPhotoSell = null;
		}
		if (this.guiData.windowSelectAll != null)
		{
			Object.Destroy(this.guiData.windowSelectAll.baseObj);
			this.guiData.windowSelectAll = null;
		}
		if (this.guiData != null)
		{
			Object.Destroy(this.guiData.baseObj);
			this.guiData = null;
		}
	}

	private void Update()
	{
		if (this.serverRequestSell != null && !this.serverRequestSell.MoveNext())
		{
			this.serverRequestSell = null;
		}
		if (this.requestTapButton != null && !this.requestTapButton.MoveNext())
		{
			this.requestTapButton = null;
		}
	}

	private void Reset()
	{
		this.sellPhotoPackList.Clear();
		if (this.requestTapButton != null)
		{
			this.requestTapButton = null;
		}
	}

	private void UpdateInfo()
	{
		this.guiData.photoSellTopGUI.PossessionGold.text = DataManager.DmItem.GetUserItemData(30101).num.ToString();
		this.getCoinNum = 0;
		foreach (PhotoPackData photoPackData in this.sellPhotoPackList)
		{
			this.getCoinNum += photoPackData.staticData.GetSalePrice();
		}
		this.guiData.photoSellTopGUI.GetCoin_Num.text = this.getCoinNum.ToString();
		bool flag = this.sellPhotoPackList.Count > 0;
		this.guiData.photoSellTopGUI.ButtonL.SetActEnable(flag, false, false);
		this.guiData.photoSellTopGUI.ButtonR.SetActEnable(this.sellPhotoPackList.Count > 0, false, false);
	}

	private IEnumerator ServerRequestSell()
	{
		bool isGoldStock = DataManagerItem.IsExpectedItemStock(30101, (long)this.getCoinNum);
		DataManager.DmPhoto.RequestActionPhotoSale(this.sellPhotoPackList);
		yield return null;
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		yield return null;
		this.ReloadDataManager(-1L);
		this.sellPhotoPackList.Clear();
		CanvasManager.HdlOpenWindowSortFilter.SolutionList(SortFilterDefine.RegisterType.PHOTO_SELL_MAIN, null);
		this.guiData.photoSellTopGUI.Num_Own.ReplaceTextByDefault("Param01", this.havePhotoPackList.Count.ToString() + "/" + DataManager.DmPhoto.PhotoStockLimit.ToString());
		this.UpdateInfo();
		string text = PrjUtil.MakeMessage("整理（売却）しました");
		if (isGoldStock)
		{
			text = string.Concat(new string[]
			{
				text,
				"\n\n所持数上限を超える",
				DataManager.DmItem.GetItemStaticBase(30101).GetName(),
				"は",
				DataManager.DmItem.GetItemStaticBase(30090).GetName(),
				"に補充されました"
			});
		}
		CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("整理（売却）完了"), text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, new PguiOpenWindowCtrl.Callback(this.OnResultWindowButtonCallback), null, false);
		CanvasManager.HdlOpenWindowBasic.Open();
		yield break;
	}

	private IEnumerator RequestTapButton(PguiButtonCtrl button)
	{
		if (button == this.guiData.photoSellTopGUI.ButtonL)
		{
			this.sellPhotoPackList.Clear();
			this.guiData.photoSellTopGUI.ScrollView.Refresh();
			this.UpdateInfo();
		}
		else if (button == this.guiData.photoSellTopGUI.ButtonR)
		{
			bool flag = false;
			using (List<PhotoPackData>.Enumerator enumerator = this.sellPhotoPackList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.staticData.GetRarity() >= ItemDef.Rarity.STAR3)
					{
						flag = true;
						break;
					}
				}
			}
			bool flag2 = false;
			foreach (PhotoPackData photoPackData in this.sellPhotoPackList)
			{
				if (photoPackData.dynamicData.exp != 0L || photoPackData.dynamicData.level != 1)
				{
					flag2 = true;
					break;
				}
			}
			string text = PrjUtil.MakeMessage("整理（売却）しますか？");
			if (flag)
			{
				text = text + PrjUtil.MakeMessage("\n") + PrjUtil.MakeMessage("<size=22><color=#FF0000FF>※★3以上のフォトが含まれています※</color></size>");
			}
			if (flag2)
			{
				text = text + PrjUtil.MakeMessage("\n") + PrjUtil.MakeMessage("<size=22><color=#FF0000FF>※成長済のフォトが含まれています※</color></size>");
			}
			this.guiData.windowPhotoSell.owCtrl.Setup(null, text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, new PguiOpenWindowCtrl.Callback(this.OnSelectOpenWindowButtonCallback), null, false);
			this.guiData.windowPhotoSell.owCtrl.Open();
			bool flag3 = false;
			if (this.guiData.windowPhotoSell.ScrollView_PhotoIconAll.onStartItem == null)
			{
				ReuseScroll scrollView_PhotoIconAll = this.guiData.windowPhotoSell.ScrollView_PhotoIconAll;
				scrollView_PhotoIconAll.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollView_PhotoIconAll.onStartItem, new Action<int, GameObject>(this.OnStartItemWindow));
				if (this.guiData.windowPhotoSell.ScrollView_PhotoIconAll.onUpdateItem == null)
				{
					ReuseScroll scrollView_PhotoIconAll2 = this.guiData.windowPhotoSell.ScrollView_PhotoIconAll;
					scrollView_PhotoIconAll2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollView_PhotoIconAll2.onUpdateItem, new Action<int, GameObject>(this.OnUpdateItemWindow));
					flag3 = true;
				}
			}
			if (flag3)
			{
				this.guiData.windowPhotoSell.ScrollView_PhotoIconAll.InitForce();
				this.guiData.windowPhotoSell.ScrollView_PhotoIconAll.Setup(10, 0);
			}
			this.guiData.windowPhotoSell.ScrollView_PhotoIconAll.Resize(1 + this.sellPhotoPackList.Count / SelPhotoSellCtrl.PhotoSellConfirmWindow.SCROLL_ITEM_NUN_H, 0);
			int num = 0;
			foreach (PhotoPackData photoPackData2 in this.sellPhotoPackList)
			{
				num += photoPackData2.staticData.GetSalePrice();
			}
			this.guiData.windowPhotoSell.Num.text = num.ToString();
			while (!this.guiData.windowPhotoSell.owCtrl.FinishedOpen())
			{
				yield return null;
			}
		}
		else if (button == this.guiData.photoSellTopGUI.ButtonC)
		{
			this.guiData.windowSelectAll.baseWindow.Setup("まとめて選択", "", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, new PguiOpenWindowCtrl.Callback(this.OnSelectAllOpenWindowButtonCallback), null, false);
			this.guiData.windowSelectAll.baseWindow.Open();
			while (!this.guiData.windowSelectAll.baseWindow.FinishedOpen())
			{
				yield return null;
			}
		}
		yield break;
	}

	private void OnClickButton(PguiButtonCtrl button)
	{
		if (button == this.guiData.photoSellTopGUI.ButtonL)
		{
			this.sellPhotoPackList.Clear();
			this.guiData.photoSellTopGUI.ScrollView.Refresh();
			this.UpdateInfo();
			return;
		}
		if (button == this.guiData.photoSellTopGUI.ButtonR)
		{
			bool flag = false;
			using (List<PhotoPackData>.Enumerator enumerator = this.sellPhotoPackList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.staticData.GetRarity() >= ItemDef.Rarity.STAR3)
					{
						flag = true;
						break;
					}
				}
			}
			bool flag2 = false;
			foreach (PhotoPackData photoPackData in this.sellPhotoPackList)
			{
				if (photoPackData.dynamicData.exp != 0L || photoPackData.dynamicData.level != 1)
				{
					flag2 = true;
					break;
				}
			}
			string text = PrjUtil.MakeMessage("整理（売却）しますか？");
			if (flag)
			{
				text = text + PrjUtil.MakeMessage("\n") + PrjUtil.MakeMessage("<size=22><color=#FF0000FF>※★3以上のフォトが含まれています※</color></size>");
			}
			if (flag2)
			{
				text = text + PrjUtil.MakeMessage("\n") + PrjUtil.MakeMessage("<size=22><color=#FF0000FF>※成長済のフォトが含まれています※</color></size>");
			}
			this.guiData.windowPhotoSell.owCtrl.Setup(null, text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, new PguiOpenWindowCtrl.Callback(this.OnSelectOpenWindowButtonCallback), null, false);
			this.guiData.windowPhotoSell.owCtrl.Open();
			bool flag3 = false;
			if (this.guiData.windowPhotoSell.ScrollView_PhotoIconAll.onStartItem == null)
			{
				ReuseScroll scrollView_PhotoIconAll = this.guiData.windowPhotoSell.ScrollView_PhotoIconAll;
				scrollView_PhotoIconAll.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollView_PhotoIconAll.onStartItem, new Action<int, GameObject>(this.OnStartItemWindow));
				if (this.guiData.windowPhotoSell.ScrollView_PhotoIconAll.onUpdateItem == null)
				{
					ReuseScroll scrollView_PhotoIconAll2 = this.guiData.windowPhotoSell.ScrollView_PhotoIconAll;
					scrollView_PhotoIconAll2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollView_PhotoIconAll2.onUpdateItem, new Action<int, GameObject>(this.OnUpdateItemWindow));
					flag3 = true;
				}
			}
			if (flag3)
			{
				this.guiData.windowPhotoSell.ScrollView_PhotoIconAll.InitForce();
				this.guiData.windowPhotoSell.ScrollView_PhotoIconAll.Setup(10, 0);
			}
			this.guiData.windowPhotoSell.ScrollView_PhotoIconAll.Resize(1 + this.sellPhotoPackList.Count / SelPhotoSellCtrl.PhotoSellConfirmWindow.SCROLL_ITEM_NUN_H, 0);
			int num = 0;
			foreach (PhotoPackData photoPackData2 in this.sellPhotoPackList)
			{
				num += photoPackData2.staticData.GetSalePrice();
			}
			this.guiData.windowPhotoSell.Num.text = num.ToString();
			return;
		}
		if (button == this.guiData.photoSellTopGUI.ButtonC)
		{
			this.guiData.windowSelectAll.baseWindow.Setup("まとめて選択", "", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, new PguiOpenWindowCtrl.Callback(this.OnSelectAllOpenWindowButtonCallback), null, false);
			this.guiData.windowSelectAll.baseWindow.Open();
		}
	}

	private bool OnClickToggleButton(PguiToggleButtonCtrl button, int index)
	{
		if (this.guiData.windowSelectAll.RarityBtnList.Exists((PguiToggleButtonCtrl item) => item == button))
		{
			this.RegistRarityButton(button);
			return true;
		}
		if (this.guiData.windowSelectAll.TypeBtnList.Exists((PguiToggleButtonCtrl item) => item == button))
		{
			this.RegistPhotoTypeButton(button);
			return true;
		}
		if (this.guiData.windowSelectAll.TempBtnList.Exists((PguiToggleButtonCtrl item) => item == button))
		{
			if (index == 0)
			{
				foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl in this.guiData.windowSelectAll.TempBtnList)
				{
					if (pguiToggleButtonCtrl != button)
					{
						pguiToggleButtonCtrl.SetToggleIndex(0);
					}
				}
				int num = this.guiData.windowSelectAll.TempBtnList.FindIndex((PguiToggleButtonCtrl item) => item == button);
				this.lvType = (SelPhotoSellCtrl.PhotoSellSelectAllWindow.LvType)num;
				return true;
			}
			foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl2 in this.guiData.windowSelectAll.TempBtnList)
			{
				if (pguiToggleButtonCtrl2 == button)
				{
					pguiToggleButtonCtrl2.SetToggleIndex(0);
				}
			}
			this.lvType = SelPhotoSellCtrl.PhotoSellSelectAllWindow.LvType.Invaid;
		}
		return false;
	}

	public void RegistRarityButton(PguiToggleButtonCtrl button)
	{
		int num = this.guiData.windowSelectAll.RarityBtnList.FindIndex((PguiToggleButtonCtrl item) => item == button);
		if (this.guiData.windowSelectAll.RarityBtnList[num].GetToggleIndex() == 0)
		{
			switch (num)
			{
			case 0:
				this.category |= SelPhotoSellCtrl.PhotoSellSelectAllWindow.CategoryMask.S1;
				return;
			case 1:
				this.category |= SelPhotoSellCtrl.PhotoSellSelectAllWindow.CategoryMask.S2;
				return;
			case 2:
				this.category |= SelPhotoSellCtrl.PhotoSellSelectAllWindow.CategoryMask.S3;
				return;
			case 3:
				this.category |= SelPhotoSellCtrl.PhotoSellSelectAllWindow.CategoryMask.S4;
				return;
			default:
				return;
			}
		}
		else
		{
			switch (num)
			{
			case 0:
				this.category &= (SelPhotoSellCtrl.PhotoSellSelectAllWindow.CategoryMask)(-2);
				return;
			case 1:
				this.category &= (SelPhotoSellCtrl.PhotoSellSelectAllWindow.CategoryMask)(-3);
				return;
			case 2:
				this.category &= (SelPhotoSellCtrl.PhotoSellSelectAllWindow.CategoryMask)(-5);
				return;
			case 3:
				this.category &= (SelPhotoSellCtrl.PhotoSellSelectAllWindow.CategoryMask)(-9);
				return;
			default:
				return;
			}
		}
	}

	public void RegistPhotoTypeButton(PguiToggleButtonCtrl button)
	{
		int num = this.guiData.windowSelectAll.TypeBtnList.FindIndex((PguiToggleButtonCtrl item) => item == button);
		int num2 = this.guiData.windowSelectAll.RarityBtnList.Count + num;
		if (this.guiData.windowSelectAll.TypeBtnList[num].GetToggleIndex() == 0)
		{
			switch (num2)
			{
			case 4:
				this.category |= SelPhotoSellCtrl.PhotoSellSelectAllWindow.CategoryMask.Status;
				return;
			case 5:
				this.category |= SelPhotoSellCtrl.PhotoSellSelectAllWindow.CategoryMask.Ability;
				return;
			case 6:
				this.category |= SelPhotoSellCtrl.PhotoSellSelectAllWindow.CategoryMask.Other;
				return;
			default:
				return;
			}
		}
		else
		{
			switch (num2)
			{
			case 4:
				this.category &= (SelPhotoSellCtrl.PhotoSellSelectAllWindow.CategoryMask)(-17);
				return;
			case 5:
				this.category &= (SelPhotoSellCtrl.PhotoSellSelectAllWindow.CategoryMask)(-33);
				return;
			case 6:
				this.category &= (SelPhotoSellCtrl.PhotoSellSelectAllWindow.CategoryMask)(-65);
				return;
			default:
				return;
			}
		}
	}

	public void OnClickMenuReturn(UnityAction callback = null)
	{
		this.guiData.SelCmn_AllInOut.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
		{
			this.Reset();
			UnityAction callback2 = callback;
			if (callback2 == null)
			{
				return;
			}
			callback2();
		});
	}

	private void OnStartItemPhotoSelect(int index, GameObject go)
	{
		for (int i = 0; i < this.guiData.photoSellTopGUI.sizeChangeBtnGUI.IconPhotoParamList[this.guiData.photoSellTopGUI.sizeChangeBtnGUI.SizeIndex].num; i++)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_PhotoSet, go.transform);
			gameObject.name = i.ToString();
			SelPhotoEditCtrl.GUI.IconPhotoSet iconPhoto = new SelPhotoEditCtrl.GUI.IconPhotoSet(gameObject.transform);
			iconPhoto.iconPhotoCtrl.AddOnClickListener(delegate(IconPhotoCtrl x)
			{
				this.OnTouchPhotoIcon(iconPhoto.iconPhotoCtrl);
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
				this.guiData.photoSellTopGUI.ScrollView.Refresh();
			});
			iconPhoto.iconPhotoCtrl.AddOnCloseWindow(delegate(IconPhotoCtrl x)
			{
				CanvasManager.HdlOpenWindowSortFilter.SolutionList(SortFilterDefine.RegisterType.PHOTO_SELL_MAIN, null);
			});
			iconPhoto.baseObj.transform.Find("Icon_Photo").transform.localScale = this.guiData.photoSellTopGUI.sizeChangeBtnGUI.IconPhotoParamList[this.guiData.photoSellTopGUI.sizeChangeBtnGUI.SizeIndex].scale;
			iconPhoto.SetScale(this.guiData.photoSellTopGUI.sizeChangeBtnGUI.IconPhotoParamList[this.guiData.photoSellTopGUI.sizeChangeBtnGUI.SizeIndex].scaleCurrent, this.guiData.photoSellTopGUI.sizeChangeBtnGUI.IconPhotoParamList[this.guiData.photoSellTopGUI.sizeChangeBtnGUI.SizeIndex].scaleCount);
			this.guiData.reservePhotoIcon.Add(iconPhoto);
			go.GetComponent<GridLayoutGroup>().SetLayoutHorizontal();
		}
	}

	private void OnUpdateItemPhotoSelect(int index, GameObject go)
	{
		for (int i = 0; i < this.guiData.photoSellTopGUI.sizeChangeBtnGUI.IconPhotoParamList[this.guiData.photoSellTopGUI.sizeChangeBtnGUI.SizeIndex].num; i++)
		{
			int num = index * this.guiData.photoSellTopGUI.sizeChangeBtnGUI.IconPhotoParamList[this.guiData.photoSellTopGUI.sizeChangeBtnGUI.SizeIndex].num + i;
			PhotoPackData photoPackData = null;
			if (num < this.dispPhotoPackList.Count)
			{
				photoPackData = this.dispPhotoPackList[num];
			}
			GameObject iconObj = go.transform.Find(i.ToString()).gameObject;
			SelPhotoEditCtrl.GUI.IconPhotoSet iconPhotoSet = this.guiData.reservePhotoIcon.Find((SelPhotoEditCtrl.GUI.IconPhotoSet item) => item.baseObj == iconObj);
			iconPhotoSet.iconPhotoCtrl.Setup(photoPackData, this.sortType, true, false, -1, false);
			iconPhotoSet.iconPhotoCtrl.onReturnPhotoPackDataList = () => this.dispPhotoPackList;
			int num2 = this.sellPhotoPackList.IndexOf(photoPackData);
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
			if (photoPackData != null && this.deckPhotoDataId.Contains(photoPackData.dataId))
			{
				if (photoPackData.dynamicData.lockFlag || photoPackData.dynamicData.favoriteFlag || photoPackData.staticData.baseData.forbiddenDiscardFlg)
				{
					iconPhotoSet.iconPhotoCtrl.DispImgDisable(true);
				}
				if (photoPackData.staticData.baseData.forbiddenDiscardFlg)
				{
					iconPhotoSet.iconPhotoCtrl.DispTextDisable(true, PrjUtil.MakeMessage(SceneCharaEdit.IsStoryPhoto(photoPackData.staticData) ? "ストーリーフォト" : "選択不可"), null);
				}
				else
				{
					iconPhotoSet.iconPhotoCtrl.DispParty(true, true);
				}
			}
			else if (photoPackData != null && (photoPackData.dynamicData.lockFlag || photoPackData.dynamicData.favoriteFlag))
			{
				iconPhotoSet.iconPhotoCtrl.DispImgDisable(true);
			}
			else if (photoPackData != null && photoPackData.staticData.baseData.forbiddenDiscardFlg)
			{
				iconPhotoSet.iconPhotoCtrl.DispImgDisable(true);
				iconPhotoSet.iconPhotoCtrl.DispTextDisable(true, PrjUtil.MakeMessage(SceneCharaEdit.IsStoryPhoto(photoPackData.staticData) ? "ストーリーフォト" : "売却不可"), null);
			}
			else
			{
				iconPhotoSet.iconPhotoCtrl.DispTextDisable(false, null, null);
				iconPhotoSet.iconPhotoCtrl.DispImgDisable(false);
				iconPhotoSet.iconPhotoCtrl.DispParty(false, true);
			}
		}
	}

	private void OnStartItemWindow(int index, GameObject go)
	{
		for (int i = 0; i < SelPhotoSellCtrl.PhotoSellConfirmWindow.SCROLL_ITEM_NUN_H; i++)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Photo, go.transform);
			gameObject.name = i.ToString();
			gameObject.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
			gameObject.transform.Find("AEImage_Eff_Change").gameObject.SetActive(false);
			gameObject.transform.Find("All").gameObject.GetComponent<AELayerConstraint>().enabled = false;
		}
	}

	private void OnUpdateItemWindow(int index, GameObject go)
	{
		for (int i = 0; i < SelPhotoSellCtrl.PhotoSellConfirmWindow.SCROLL_ITEM_NUN_H; i++)
		{
			int num = index * SelPhotoSellCtrl.PhotoSellConfirmWindow.SCROLL_ITEM_NUN_H + i;
			PhotoPackData photoPackData = null;
			if (num < this.sellPhotoPackList.Count)
			{
				photoPackData = this.sellPhotoPackList[num];
			}
			GameObject gameObject = go.transform.Find(i.ToString()).gameObject;
			gameObject.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
			gameObject.GetComponent<IconPhotoCtrl>().Setup(photoPackData, SortFilterDefine.SortType.LEVEL, false, false, -1, false);
			int num2 = this.sellPhotoPackList.IndexOf(photoPackData);
			gameObject.SetActive(num2 >= 0);
		}
	}

	private void OnUpdatePhotoLock(IconPhotoCtrl iconPhoto)
	{
		if (iconPhoto.photoPackData.dynamicData.lockFlag && this.sellPhotoPackList.Contains(iconPhoto.photoPackData))
		{
			this.OnTouchPhotoIcon(iconPhoto);
		}
		this.guiData.photoSellTopGUI.ScrollView.Refresh();
	}

	private void OnUpdatePhotoFavorite(IconPhotoCtrl iconPhoto)
	{
		if (iconPhoto.photoPackData.dynamicData.favoriteFlag && this.sellPhotoPackList.Contains(iconPhoto.photoPackData))
		{
			this.OnTouchPhotoIcon(iconPhoto);
		}
		this.guiData.photoSellTopGUI.ScrollView.Refresh();
	}

	private void OnTouchPhotoIcon(IconPhotoCtrl iconPhoto)
	{
		SoundManager.Play("prd_se_click", false, false);
		if (iconPhoto.photoPackData != null && !iconPhoto.photoPackData.IsInvalid())
		{
			bool flag = false;
			if (!iconPhoto.photoPackData.staticData.baseData.forbiddenDiscardFlg)
			{
				if (this.deckPhotoDataId.Contains(iconPhoto.photoPackData.dataId))
				{
					if (!iconPhoto.CheckImgDisable())
					{
						PhotoUtil.OpenWindowConfirmReleasePhotoDeck(iconPhoto, this.RequestUpdatePhoto(iconPhoto.photoPackData));
					}
				}
				else if ((iconPhoto.photoPackData.dynamicData.lockFlag || iconPhoto.photoPackData.dynamicData.favoriteFlag) && this.sellPhotoPackList.Contains(iconPhoto.photoPackData))
				{
					this.sellPhotoPackList.Remove(iconPhoto.photoPackData);
					flag = true;
				}
				else if (!iconPhoto.photoPackData.dynamicData.lockFlag && !iconPhoto.photoPackData.dynamicData.favoriteFlag)
				{
					if (this.sellPhotoPackList.Contains(iconPhoto.photoPackData))
					{
						this.sellPhotoPackList.Remove(iconPhoto.photoPackData);
						flag = true;
					}
					else if (this.sellPhotoPackList.Count >= this.MAX_SELL_PHOTO_LIST)
					{
						CanvasManager.HdlOpenWindowBasic.Setup("選択上限", "これ以上選択できません。", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, null, null, false);
						CanvasManager.HdlOpenWindowBasic.Open();
					}
					else
					{
						this.sellPhotoPackList.Add(iconPhoto.photoPackData);
						flag = true;
					}
				}
			}
			if (flag)
			{
				for (int i = 0; i < this.guiData.reservePhotoIcon.Count; i++)
				{
					int num = this.sellPhotoPackList.IndexOf(this.guiData.reservePhotoIcon[i].iconPhotoCtrl.photoPackData);
					if (num >= 0)
					{
						this.guiData.reservePhotoIcon[i].currentFrame.SetActive(true);
						this.guiData.reservePhotoIcon[i].DispCount(true, (num + 1).ToString());
					}
					else
					{
						this.guiData.reservePhotoIcon[i].currentFrame.SetActive(false);
						this.guiData.reservePhotoIcon[i].DispCount(false, null);
					}
				}
			}
			this.UpdateInfo();
		}
	}

	private IEnumerator RequestUpdatePhoto(PhotoPackData photoPackData)
	{
		DataManager.DmPhoto.RequestActionPhotoRelease(photoPackData.dataId);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		if (!photoPackData.staticData.baseData.forbiddenDiscardFlg && !photoPackData.dynamicData.lockFlag && !photoPackData.dynamicData.favoriteFlag)
		{
			this.sellPhotoPackList.Add(photoPackData);
		}
		this.ReloadDataManager(-1L);
		CanvasManager.HdlOpenWindowSortFilter.SolutionList(SortFilterDefine.RegisterType.PHOTO_SELL_MAIN, null);
		this.UpdateInfo();
		this.guiData.photoSellTopGUI.ScrollView.Refresh();
		yield break;
	}

	private bool OnSelectOpenWindowButtonCallback(int index)
	{
		if (index == 1)
		{
			this.serverRequestSell = this.ServerRequestSell();
		}
		return true;
	}

	private bool OnResultWindowButtonCallback(int index)
	{
		return true;
	}

	private bool OnSelectAllOpenWindowButtonCallback(int index)
	{
		if (this.lvType == SelPhotoSellCtrl.PhotoSellSelectAllWindow.LvType.Invaid && this.category == (SelPhotoSellCtrl.PhotoSellSelectAllWindow.CategoryMask)0)
		{
			return true;
		}
		if (index == 1)
		{
			this.sellPhotoPackList.Clear();
			SelPhotoSellCtrl.PhotoSellSelectAllWindow.CategoryMask workFlag = this.category;
			if ((SelPhotoSellCtrl.PhotoSellSelectAllWindow.CategoryMask.Rarity & this.category) == (SelPhotoSellCtrl.PhotoSellSelectAllWindow.CategoryMask)0)
			{
				workFlag |= SelPhotoSellCtrl.PhotoSellSelectAllWindow.CategoryMask.Rarity;
			}
			if ((SelPhotoSellCtrl.PhotoSellSelectAllWindow.CategoryMask.Type & this.category) == (SelPhotoSellCtrl.PhotoSellSelectAllWindow.CategoryMask)0)
			{
				workFlag |= SelPhotoSellCtrl.PhotoSellSelectAllWindow.CategoryMask.Type;
			}
			this.sellPhotoPackList = this.dispPhotoPackList.FindAll((PhotoPackData item) => !this.deckPhotoDataId.Contains(item.dataId) && !item.staticData.baseData.forbiddenDiscardFlg && !item.dynamicData.lockFlag && !item.dynamicData.favoriteFlag && (((workFlag & SelPhotoSellCtrl.PhotoSellSelectAllWindow.CategoryMask.S4) != (SelPhotoSellCtrl.PhotoSellSelectAllWindow.CategoryMask)0 && item.staticData.baseData.rarity == ItemDef.Rarity.STAR4) || ((workFlag & SelPhotoSellCtrl.PhotoSellSelectAllWindow.CategoryMask.S3) != (SelPhotoSellCtrl.PhotoSellSelectAllWindow.CategoryMask)0 && item.staticData.baseData.rarity == ItemDef.Rarity.STAR3) || ((workFlag & SelPhotoSellCtrl.PhotoSellSelectAllWindow.CategoryMask.S2) != (SelPhotoSellCtrl.PhotoSellSelectAllWindow.CategoryMask)0 && item.staticData.baseData.rarity == ItemDef.Rarity.STAR2) || ((workFlag & SelPhotoSellCtrl.PhotoSellSelectAllWindow.CategoryMask.S1) != (SelPhotoSellCtrl.PhotoSellSelectAllWindow.CategoryMask)0 && item.staticData.baseData.rarity == ItemDef.Rarity.STAR1)) && (((workFlag & SelPhotoSellCtrl.PhotoSellSelectAllWindow.CategoryMask.Status) != (SelPhotoSellCtrl.PhotoSellSelectAllWindow.CategoryMask)0 && item.staticData.baseData.type == PhotoDef.Type.PARAMETER) || ((workFlag & SelPhotoSellCtrl.PhotoSellSelectAllWindow.CategoryMask.Ability) != (SelPhotoSellCtrl.PhotoSellSelectAllWindow.CategoryMask)0 && item.staticData.baseData.type == PhotoDef.Type.ABILITY) || ((workFlag & SelPhotoSellCtrl.PhotoSellSelectAllWindow.CategoryMask.Other) != (SelPhotoSellCtrl.PhotoSellSelectAllWindow.CategoryMask)0 && item.staticData.baseData.type == PhotoDef.Type.OTHER) || (workFlag & SelPhotoSellCtrl.PhotoSellSelectAllWindow.CategoryMask.Type) == SelPhotoSellCtrl.PhotoSellSelectAllWindow.CategoryMask.Type) && ((this.lvType == SelPhotoSellCtrl.PhotoSellSelectAllWindow.LvType.Lv1Only && item.dynamicData.level <= 1) || ((this.lvType == SelPhotoSellCtrl.PhotoSellSelectAllWindow.LvType.Invaid || this.lvType == SelPhotoSellCtrl.PhotoSellSelectAllWindow.LvType.Unspecified) && item.dynamicData.level >= 1)));
			if (this.sellPhotoPackList.Count > this.MAX_SELL_PHOTO_LIST)
			{
				this.sellPhotoPackList.RemoveRange(this.MAX_SELL_PHOTO_LIST, this.sellPhotoPackList.Count - this.MAX_SELL_PHOTO_LIST);
			}
			this.guiData.photoSellTopGUI.ScrollView.Refresh();
			this.UpdateInfo();
		}
		return true;
	}

	private SelPhotoSellCtrl.GUI guiData;

	public bool isDebug;

	private List<PhotoPackData> havePhotoPackList = new List<PhotoPackData>();

	private List<PhotoPackData> dispPhotoPackList = new List<PhotoPackData>();

	private List<long> deckPhotoDataId = new List<long>();

	private List<PhotoPackData> sellPhotoPackList = new List<PhotoPackData>();

	private SortFilterDefine.SortType sortType = SortFilterDefine.SortType.LEVEL;

	private SelPhotoSellCtrl.PhotoSellSelectAllWindow.CategoryMask category;

	private SelPhotoSellCtrl.PhotoSellSelectAllWindow.LvType lvType = SelPhotoSellCtrl.PhotoSellSelectAllWindow.LvType.Invaid;

	private UserOptionData cloneUserOptionData;

	private int getCoinNum;

	private readonly int MAX_SELL_PHOTO_LIST = 1000;

	private IEnumerator serverRequestSell;

	private IEnumerator requestTapButton;

	public class GUIPhotoSellFilter
	{
		public GUIPhotoSellFilter(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.baseWindow = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.PartsList = new List<SelPhotoSellCtrl.GUIPhotoSellFilter.Parts>
			{
				new SelPhotoSellCtrl.GUIPhotoSellFilter.Parts(baseTr.Find("Base/Window/Sort/Btn01")),
				new SelPhotoSellCtrl.GUIPhotoSellFilter.Parts(baseTr.Find("Base/Window/Sort/Btn02")),
				new SelPhotoSellCtrl.GUIPhotoSellFilter.Parts(baseTr.Find("Base/Window/Sort/Btn03")),
				new SelPhotoSellCtrl.GUIPhotoSellFilter.Parts(baseTr.Find("Base/Window/Sort/Btn04")),
				new SelPhotoSellCtrl.GUIPhotoSellFilter.Parts(baseTr.Find("Base/Window/Sort/Btn05")),
				new SelPhotoSellCtrl.GUIPhotoSellFilter.Parts(baseTr.Find("Base/Window/Sort/Btn06")),
				new SelPhotoSellCtrl.GUIPhotoSellFilter.Parts(baseTr.Find("Base/Window/Sort/Btn07")),
				new SelPhotoSellCtrl.GUIPhotoSellFilter.Parts(baseTr.Find("Base/Window/Sort/Btn08")),
				new SelPhotoSellCtrl.GUIPhotoSellFilter.Parts(baseTr.Find("Base/Window/Sort/Btn09")),
				new SelPhotoSellCtrl.GUIPhotoSellFilter.Parts(baseTr.Find("Base/Window/Sort/Btn10")),
				new SelPhotoSellCtrl.GUIPhotoSellFilter.Parts(baseTr.Find("Base/Window/Sort/Btn11")),
				new SelPhotoSellCtrl.GUIPhotoSellFilter.Parts(baseTr.Find("Base/Window/Sort/Btn12"))
			};
			this.RarityBtnList = new List<PguiToggleButtonCtrl>
			{
				baseTr.Find("Base/Window/Sort/Btn01").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Base/Window/Sort/Btn02").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Base/Window/Sort/Btn03").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Base/Window/Sort/Btn04").GetComponent<PguiToggleButtonCtrl>()
			};
			this.TypeBtnList = new List<PguiToggleButtonCtrl>
			{
				baseTr.Find("Base/Window/Sort/Btn05").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Base/Window/Sort/Btn06").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Base/Window/Sort/Btn07").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Base/Window/Sort/Btn08").GetComponent<PguiToggleButtonCtrl>()
			};
			this.TempBtnList = new List<PguiToggleButtonCtrl>
			{
				baseTr.Find("Base/Window/Sort/Btn09").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Base/Window/Sort/Btn10").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Base/Window/Sort/Btn11").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Base/Window/Sort/Btn12").GetComponent<PguiToggleButtonCtrl>()
			};
			this.Btn_EventList = new List<PguiToggleButtonCtrl> { baseTr.Find("Base/Window/Sort/Btn_Event01").GetComponent<PguiToggleButtonCtrl>() };
			this.Txt01 = baseTr.Find("Base/Window/Txt01").GetComponent<PguiTextCtrl>();
			this.Txt02 = baseTr.Find("Base/Window/Txt02").GetComponent<PguiTextCtrl>();
			this.Txt03 = baseTr.Find("Base/Window/Txt03").GetComponent<PguiTextCtrl>();
		}

		public void SetActiveSellButton()
		{
			List<string> sellPhotoFilterPhotoNameList = SortFilterDefine.sellPhotoFilterPhotoNameList;
			for (int i = 0; i < this.PartsList.Count; i++)
			{
				if (this.PartsList[i].text != null)
				{
					if (i < sellPhotoFilterPhotoNameList.Count)
					{
						this.PartsList[i].text.text = sellPhotoFilterPhotoNameList[i];
					}
					else
					{
						this.PartsList[i].text.text = "";
					}
				}
			}
			int num = 0;
			foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl in this.RarityBtnList)
			{
				pguiToggleButtonCtrl.gameObject.SetActive(num < sellPhotoFilterPhotoNameList.Count);
				if (num < sellPhotoFilterPhotoNameList.Count)
				{
					pguiToggleButtonCtrl.gameObject.SetActive(!sellPhotoFilterPhotoNameList[num].Contains(SortFilterDefine.BTN_DISABLE_STR_NAME));
				}
				num++;
			}
			foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl2 in this.TypeBtnList)
			{
				pguiToggleButtonCtrl2.gameObject.SetActive(num < sellPhotoFilterPhotoNameList.Count);
				if (num < sellPhotoFilterPhotoNameList.Count)
				{
					pguiToggleButtonCtrl2.gameObject.SetActive(!sellPhotoFilterPhotoNameList[num].Contains(SortFilterDefine.BTN_DISABLE_STR_NAME));
				}
				num++;
			}
			foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl3 in this.TempBtnList)
			{
				pguiToggleButtonCtrl3.gameObject.SetActive(num < sellPhotoFilterPhotoNameList.Count);
				if (num < sellPhotoFilterPhotoNameList.Count)
				{
					pguiToggleButtonCtrl3.gameObject.SetActive(!sellPhotoFilterPhotoNameList[num].Contains(SortFilterDefine.BTN_DISABLE_STR_NAME));
				}
				num++;
			}
			foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl4 in this.Btn_EventList)
			{
				pguiToggleButtonCtrl4.gameObject.SetActive(false);
			}
		}

		public void AddOnClickListener(PguiToggleButtonCtrl.OnClick onClick)
		{
			foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl in this.RarityBtnList)
			{
				pguiToggleButtonCtrl.AddOnClickListener(onClick);
			}
			foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl2 in this.TypeBtnList)
			{
				pguiToggleButtonCtrl2.AddOnClickListener(onClick);
			}
			foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl3 in this.TempBtnList)
			{
				pguiToggleButtonCtrl3.AddOnClickListener(onClick);
			}
			foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl4 in this.Btn_EventList)
			{
				pguiToggleButtonCtrl4.AddOnClickListener(onClick);
			}
		}

		public void SetCategoryText(SortFilterDefine.PhotoFilterType filterType)
		{
			switch (filterType)
			{
			case SortFilterDefine.PhotoFilterType.SortFilter:
				this.Txt03.text = "特殊効果";
				return;
			case SortFilterDefine.PhotoFilterType.SellPhoto:
				this.Txt03.text = "レベル";
				return;
			case SortFilterDefine.PhotoFilterType.PhotoAlbum:
				this.Txt03.text = "登録状態";
				return;
			default:
				this.Txt03.text = "特殊効果";
				return;
			}
		}

		public GameObject baseObj;

		public List<SelPhotoSellCtrl.GUIPhotoSellFilter.Parts> PartsList;

		public List<PguiToggleButtonCtrl> RarityBtnList;

		public List<PguiToggleButtonCtrl> TypeBtnList;

		public List<PguiToggleButtonCtrl> TempBtnList;

		public List<PguiToggleButtonCtrl> Btn_EventList;

		public PguiTextCtrl Txt01;

		public PguiTextCtrl Txt02;

		public PguiTextCtrl Txt03;

		public PguiOpenWindowCtrl baseWindow;

		public class Parts
		{
			public Parts(Transform baseTr)
			{
				Transform transform = baseTr.Find("Num_Txt");
				this.text = ((transform != null) ? transform.GetComponent<PguiTextCtrl>() : null);
				Transform transform2 = baseTr.Find("Img");
				this.img = ((transform2 != null) ? transform2.GetComponent<PguiImageCtrl>() : null);
			}

			public PguiTextCtrl text;

			public PguiImageCtrl img;
		}
	}

	public class GUI
	{
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.basePanel = baseTr.GetComponent<PguiPanel>();
			this.photoSellTopGUI = new SelPhotoSellCtrl.PhotoSellTopGUI(this.baseObj.transform);
			GameObject gameObject = (GameObject)Resources.Load("SceneCharaEdit/GUI/Prefab/GUI_PhotoSell_Window");
			this.windowPhotoSell = new SelPhotoSellCtrl.PhotoSellConfirmWindow(Object.Instantiate<Transform>(gameObject.transform.Find("Window_PhotoSell"), baseTr).transform);
			GameObject gameObject2 = AssetManager.InstantiateAssetData("SelCmn/GUI/Prefab/GUI_FilterWindow_PhotoSell", null);
			gameObject2.AddComponent<SafeAreaScaler>();
			this.windowSelectAll = new SelPhotoSellCtrl.GUIPhotoSellFilter(gameObject2.transform);
			this.SelCmn_AllInOut = baseTr.GetComponent<SimpleAnimation>();
		}

		public SelPhotoSellCtrl.PhotoSellTopGUI photoSellTopGUI;

		public SelPhotoSellCtrl.PhotoSellConfirmWindow windowPhotoSell;

		public SelPhotoSellCtrl.GUIPhotoSellFilter windowSelectAll;

		public GameObject baseObj;

		public PguiPanel basePanel;

		public SimpleAnimation SelCmn_AllInOut;

		public List<SelPhotoEditCtrl.GUI.IconPhotoSet> reservePhotoIcon = new List<SelPhotoEditCtrl.GUI.IconPhotoSet>();
	}

	public class PhotoSellTopGUI
	{
		public PhotoSellTopGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_FilterOnOff = baseTr.Find("All/WindowAll/SortFilterBtnsAll/Btn_FilterOnOff").GetComponent<PguiButtonCtrl>();
			this.Btn_Sort = baseTr.Find("All/WindowAll/SortFilterBtnsAll/Btn_Sort").GetComponent<PguiButtonCtrl>();
			this.Btn_SortUpDown = baseTr.Find("All/WindowAll/SortFilterBtnsAll/Btn_SortUpDown").GetComponent<PguiButtonCtrl>();
			this.Num_Own = baseTr.Find("All/WindowAll/SortFilterBtnsAll/Num_Own").GetComponent<PguiTextCtrl>();
			this.ButtonL = baseTr.Find("All/SellInfo/ButtonL").GetComponent<PguiButtonCtrl>();
			this.ButtonR = baseTr.Find("All/SellInfo/ButtonR").GetComponent<PguiButtonCtrl>();
			this.ButtonC = baseTr.Find("All/SellInfo/ButtonC").GetComponent<PguiButtonCtrl>();
			this.ScrollView = baseTr.Find("All/WindowAll/ScrollView").GetComponent<ReuseScroll>();
			this.PossessionGold = baseTr.Find("All/SellInfo/SelectPhoto/Num").GetComponent<PguiTextCtrl>();
			this.GetCoin_Num = baseTr.Find("All/SellInfo/GetCoin/Num").GetComponent<PguiTextCtrl>();
			this.sizeChangeBtnGUI = new PhotoUtil.SizeChangeBtnGUI(baseTr.Find("All/WindowAll/SortFilterBtnsAll/Btn_SizeChange"));
			this.Txt_None = baseTr.Find("All/WindowAll/Txt_None").gameObject;
			this.Txt_None.SetActive(false);
		}

		public void ResizeScrollView(int count, int resize)
		{
			this.Txt_None.SetActive(count <= 0);
			this.ScrollView.ResizeFocesNoMove(resize);
		}

		public static readonly int SCROLL_ITEM_NUN_H = 8;

		public GameObject baseObj;

		public PguiButtonCtrl Btn_FilterOnOff;

		public PguiButtonCtrl Btn_Sort;

		public PguiButtonCtrl Btn_SortUpDown;

		public PguiTextCtrl Num_Own;

		public PguiButtonCtrl ButtonL;

		public PguiButtonCtrl ButtonR;

		public PguiButtonCtrl ButtonC;

		public ReuseScroll ScrollView;

		public PguiTextCtrl PossessionGold;

		public PguiTextCtrl GetCoin_Num;

		public PhotoUtil.SizeChangeBtnGUI sizeChangeBtnGUI;

		public GameObject Txt_None;
	}

	public class PhotoSellConfirmWindow
	{
		public PhotoSellConfirmWindow(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Massage = baseTr.Find("Base/Window/Massage").GetComponent<PguiTextCtrl>();
			this.Text = baseTr.Find("Base/Window/Title/Text").GetComponent<PguiTextCtrl>();
			this.Num = baseTr.Find("Base/Window/GetCoin/Num").GetComponent<PguiTextCtrl>();
			this.Base = baseTr.Find("Base").GetComponent<SimpleAnimation>();
			this.ScrollView_PhotoIconAll = baseTr.Find("Base/Window/PhotoSellInfo/ScrollView_PhotoIconAll").GetComponent<ReuseScroll>();
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
		}

		public static readonly int SCROLL_ITEM_NUN_H = 5;

		public GameObject baseObj;

		public PguiTextCtrl Massage;

		public PguiTextCtrl Text;

		public PguiTextCtrl Num;

		public SimpleAnimation Base;

		public ReuseScroll ScrollView_PhotoIconAll;

		public PguiOpenWindowCtrl owCtrl;
	}

	public class PhotoSellSelectAllWindow
	{
		public enum CategoryType
		{
			S1,
			S2,
			S3,
			S4,
			Status,
			Ability,
			Other
		}

		public enum CategoryMask
		{
			S1 = 1,
			S2,
			S3 = 4,
			S4 = 8,
			Status = 16,
			Ability = 32,
			Other = 64,
			Rarity = 15,
			Type = 112
		}

		public enum LvType
		{
			Invaid = -1,
			Lv1Only,
			Unspecified
		}
	}
}
