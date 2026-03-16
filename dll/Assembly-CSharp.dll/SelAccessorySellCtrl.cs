using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelAccessorySellCtrl : MonoBehaviour
{
	private List<DataManagerCharaAccessory.Accessory> sellAccessoryList { get; } = new List<DataManagerCharaAccessory.Accessory>();

	public void Initialize()
	{
		GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneAccessory/GUI/Prefab/GUI_AccessorySell"), base.transform);
		this.guiData = new SelAccessorySellCtrl.GUI(gameObject.transform);
		this.guiData.guiTop.SortFilterBar = new SortFilterBtnsAllCtrl(SortFilterDefine.RegisterType.ACCESSORY_SELL, gameObject.transform.Find("All/WindowAll/SortFilterBtnsAll").gameObject, new UnityAction(this.OnChangeSortFilter));
		this.guiData.guiTop.SortFilterBar.SelectAccessoryList = this.sellAccessoryList;
		AccessoryUtil.SizeChangeBtnGUI.DataPack[] dataPacks = AccessoryUtil.SizeChangeBtnGUI.GetDataPacks(AccessoryUtil.SizeChangeBtnGUI.DataPackType.All);
		AccessoryUtil.SizeChangeBtnGUI sizeChangeBtnGUI = this.guiData.guiTop.sizeChangeBtnGUI;
		AccessoryUtil.SizeChangeBtnGUI.SetupParam setupParam = new AccessoryUtil.SizeChangeBtnGUI.SetupParam();
		setupParam.funcResult = delegate(AccessoryUtil.SizeChangeBtnGUI.ResultParam result)
		{
			DataManager.DmGameStatus.RequestActionUpdateIconsideIndex(SortFilterDefine.IconPlace.AccessoryAll, result.sizeIndex);
		};
		setupParam.iconAccessoryParamList = new List<AccessoryUtil.SizeChangeBtnGUI.IconAccessoryParam>
		{
			new AccessoryUtil.SizeChangeBtnGUI.IconAccessoryParam
			{
				scale = new Vector3(0.6f, 0.6f, 1f),
				scaleCurrent = new Vector3(1f, 1f, 1f),
				scaleCount = new Vector3(1f, 1f, 1f),
				num = 10,
				prefab = Object.Instantiate<GameObject>((GameObject)Resources.Load(dataPacks[0].prefabName), base.transform)
			},
			new AccessoryUtil.SizeChangeBtnGUI.IconAccessoryParam
			{
				scale = new Vector3(0.75f, 0.75f, 1f),
				scaleCurrent = new Vector3(1f, 1f, 1f),
				scaleCount = new Vector3(1f, 1f, 1f),
				num = 8,
				prefab = Object.Instantiate<GameObject>((GameObject)Resources.Load(dataPacks[1].prefabName), base.transform)
			},
			new AccessoryUtil.SizeChangeBtnGUI.IconAccessoryParam
			{
				scale = new Vector3(0.85f, 0.85f, 1f),
				scaleCurrent = new Vector3(1f, 1f, 1f),
				scaleCount = new Vector3(1f, 1f, 1f),
				num = 7,
				prefab = Object.Instantiate<GameObject>((GameObject)Resources.Load(dataPacks[2].prefabName), base.transform)
			},
			new AccessoryUtil.SizeChangeBtnGUI.IconAccessoryParam
			{
				scale = new Vector3(1f, 1f, 1f),
				scaleCurrent = new Vector3(1f, 1f, 1f),
				scaleCount = new Vector3(1f, 1f, 1f),
				num = 6,
				prefab = Object.Instantiate<GameObject>((GameObject)Resources.Load(dataPacks[3].prefabName), base.transform)
			}
		};
		setupParam.onStartItem = new Action<int, GameObject>(this.OnStartItemAccessorySelect);
		setupParam.onUpdateItem = new Action<int, GameObject>(this.OnUpdateItemAccessorySelect);
		setupParam.refScrollView = this.guiData.guiTop.ScrollView;
		setupParam.sizeIndex = this.GetIconSizeIndex();
		setupParam.resetCallback = delegate
		{
			this.guiData.guiTop.iconAccessorySetList.Clear();
		};
		setupParam.dispIconAccessoryCountCallback = () => this.dispAccessoryList.Count;
		sizeChangeBtnGUI.Setup(setupParam);
		this.ResizeScrollView();
		this.guiData.guiTop.Btn_CancelAll.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickCancelAllButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.guiTop.Btn_SelectAll.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickSellButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.guiTop.Btn_buy.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickSelectAllButton), PguiButtonCtrl.SoundType.DEFAULT);
		GameObject gameObject2 = (GameObject)Resources.Load("SceneAccessory/GUI/Prefab/GUI_AccessorySell_SelectWindow");
		this.windowSelectAll = new SelAccessorySellCtrl.SelectAllWindow(Object.Instantiate<Transform>(gameObject2.transform.Find("Window_AccessorySelect"), Singleton<CanvasManager>.Instance.SystemMiddleArea).transform);
		foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl in this.windowSelectAll.RarityBtnList)
		{
			pguiToggleButtonCtrl.AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickRarityButton));
		}
		foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl2 in this.windowSelectAll.LevelBtnList)
		{
			pguiToggleButtonCtrl2.AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickLevelButton));
		}
		foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl3 in this.windowSelectAll.ContractBtnList)
		{
			pguiToggleButtonCtrl3.AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickContractButton));
		}
	}

	private int GetIconSizeIndex()
	{
		return DataManager.DmGameStatus.MakeUserFlagData().GetIconSizeData(SortFilterDefine.IconPlace.AccessorySell).SizeIndex;
	}

	private void Update()
	{
		if (this.serverRequestSell != null && !this.serverRequestSell.MoveNext())
		{
			this.serverRequestSell = null;
		}
	}

	public void Deestroy()
	{
		if (this.windowSelectAll != null)
		{
			Object.Destroy(this.windowSelectAll.baseObj);
			this.windowSelectAll = null;
		}
	}

	public void Setup()
	{
		this.sellAccessoryList.Clear();
		this.ReloadDataManager();
		this.UpdateUserInfo();
		this.UpdateSellAccessoryInfo();
		this.ResizeScrollView();
	}

	public void ReloadDataManager()
	{
		this.dispAccessoryList.Clear();
		this.dispAccessoryList.AddRange(this.guiData.guiTop.SortFilterBar.GetSortFilteredAccessoryList());
		this.equippedAccessoryUniqIdList.Clear();
		List<CharaPackData> list = new List<CharaPackData>(DataManager.DmChara.GetUserCharaMap().Values);
		this.equippedAccessoryUniqIdList.AddRange(list.FindAll((CharaPackData item) => !AccessoryUtil.IsInvalid(item.dynamicData.accessory)).ConvertAll<long>((CharaPackData item) => item.dynamicData.accessory.UniqId));
	}

	private void UpdateUserInfo()
	{
		this.guiData.guiTop.Num_Own.ReplaceTextByDefault("Param01", DataManager.DmChAccessory.GetUserAccessoryList().Count.ToString() + "/" + DataManager.DmChAccessory.AccessoryStockLimit.ToString());
		this.guiData.guiTop.PossessionGold.text = DataManager.DmItem.GetUserItemData(30101).num.ToString();
	}

	private void UpdateSellAccessoryInfo()
	{
		this.guiData.guiTop.GetCoin_Num.text = this.CalcSellCoin().ToString();
		bool flag = 0 < this.sellAccessoryList.Count;
		this.guiData.guiTop.Btn_CancelAll.SetActEnable(flag, false, false);
		this.guiData.guiTop.Btn_SelectAll.SetActEnable(flag, false, false);
	}

	private int CalcSellCoin()
	{
		int num = 0;
		foreach (DataManagerCharaAccessory.Accessory accessory in this.sellAccessoryList)
		{
			num += accessory.AccessoryData.Rarity.SellPrice;
		}
		return num;
	}

	private bool AddToSellAccessoryList(DataManagerCharaAccessory.Accessory accessory)
	{
		int count = this.sellAccessoryList.Count;
		if (!AccessoryUtil.IsInvalid(accessory) && !accessory.IsLock)
		{
			this.sellAccessoryList.Add(accessory);
		}
		int count2 = this.sellAccessoryList.Count;
		return count < count2;
	}

	private bool RemoveFromSellAccessoryList(DataManagerCharaAccessory.Accessory accessory)
	{
		return this.sellAccessoryList.Remove(accessory);
	}

	private void UpdateSelectStatus(AccessoryUtil.IconAccessorySet iconSet)
	{
		int num = this.sellAccessoryList.IndexOf(iconSet.iconAccessoryCtrl.accessory);
		if (0 <= num)
		{
			iconSet.currentFrame.SetActive(true);
			iconSet.DispCount(true, (num + 1).ToString());
			return;
		}
		iconSet.currentFrame.SetActive(false);
		iconSet.DispCount(false, null);
	}

	private void UpdateSelectStatusAll()
	{
		foreach (AccessoryUtil.IconAccessorySet iconAccessorySet in this.guiData.guiTop.iconAccessorySetList)
		{
			this.UpdateSelectStatus(iconAccessorySet);
		}
	}

	private bool HasFlag(SelAccessorySellCtrl.CategoryMask target, SelAccessorySellCtrl.CategoryMask flag)
	{
		return (target & flag) > SelAccessorySellCtrl.CategoryMask.None;
	}

	private void AddFlag(ref SelAccessorySellCtrl.CategoryMask target, SelAccessorySellCtrl.CategoryMask flag)
	{
		target |= flag;
	}

	private void RemoveFlag(ref SelAccessorySellCtrl.CategoryMask target, SelAccessorySellCtrl.CategoryMask flag)
	{
		target &= ~flag;
	}

	private IEnumerator ServerRequestSell()
	{
		DataManager.DmChAccessory.RequestActionRemoveCharaAccessory(this.sellAccessoryList);
		yield return null;
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		bool isGoldStock = DataManagerItem.IsExpectedItemStock(30101, (long)this.CalcSellCoin());
		DataManager.DmChAccessory.RequestActionSell(this.sellAccessoryList);
		yield return null;
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		yield return null;
		this.Setup();
		string text = PrjUtil.MakeMessage("売却しました");
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
		CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("売却完了"), text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, null, null, false);
		CanvasManager.HdlOpenWindowBasic.Open();
		yield break;
	}

	private void OnClickCancelAllButton(PguiButtonCtrl button)
	{
		this.sellAccessoryList.Clear();
		this.UpdateSellAccessoryInfo();
		this.guiData.guiTop.ScrollView.Refresh();
	}

	private void OnClickSelectAllButton(PguiButtonCtrl button)
	{
		this.windowSelectAll.owCtrl.Setup("まとめて選択", "", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, new PguiOpenWindowCtrl.Callback(this.SelectAllWindowButtonCallback), null, false);
		this.windowSelectAll.owCtrl.Open();
	}

	private void OnClickSellButton(PguiButtonCtrl button)
	{
		CanvasManager.HdlAccessoryCheckWindowCtrl.OpenSale(this.sellAccessoryList, new UnityAction(this.AccessoryCheckWindowCallback));
	}

	private void AccessoryCheckWindowCallback()
	{
		this.serverRequestSell = this.ServerRequestSell();
	}

	public bool OnClickRarityButton(PguiToggleButtonCtrl button, int index)
	{
		SelAccessorySellCtrl.CategoryType categoryType = (SelAccessorySellCtrl.CategoryType)this.windowSelectAll.RarityBtnList.FindIndex((PguiToggleButtonCtrl item) => item == button);
		if (index == 0)
		{
			switch (categoryType)
			{
			case SelAccessorySellCtrl.CategoryType.S1:
				this.AddFlag(ref this.categoryMask, SelAccessorySellCtrl.CategoryMask.S1);
				break;
			case SelAccessorySellCtrl.CategoryType.S2:
				this.AddFlag(ref this.categoryMask, SelAccessorySellCtrl.CategoryMask.S2);
				break;
			case SelAccessorySellCtrl.CategoryType.S3:
				this.AddFlag(ref this.categoryMask, SelAccessorySellCtrl.CategoryMask.S3);
				break;
			case SelAccessorySellCtrl.CategoryType.S4:
				this.AddFlag(ref this.categoryMask, SelAccessorySellCtrl.CategoryMask.S4);
				break;
			}
		}
		else
		{
			switch (categoryType)
			{
			case SelAccessorySellCtrl.CategoryType.S1:
				this.RemoveFlag(ref this.categoryMask, SelAccessorySellCtrl.CategoryMask.S1);
				break;
			case SelAccessorySellCtrl.CategoryType.S2:
				this.RemoveFlag(ref this.categoryMask, SelAccessorySellCtrl.CategoryMask.S2);
				break;
			case SelAccessorySellCtrl.CategoryType.S3:
				this.RemoveFlag(ref this.categoryMask, SelAccessorySellCtrl.CategoryMask.S3);
				break;
			case SelAccessorySellCtrl.CategoryType.S4:
				this.RemoveFlag(ref this.categoryMask, SelAccessorySellCtrl.CategoryMask.S4);
				break;
			}
		}
		return true;
	}

	public bool OnClickLevelButton(PguiToggleButtonCtrl button, int index)
	{
		if (index == 0)
		{
			foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl in this.windowSelectAll.LevelBtnList)
			{
				if (pguiToggleButtonCtrl != button)
				{
					pguiToggleButtonCtrl.SetToggleIndex(0);
				}
			}
			int num = this.windowSelectAll.LevelBtnList.FindIndex((PguiToggleButtonCtrl item) => item == button);
			this.lvType = (SelAccessorySellCtrl.LvType)num;
			return true;
		}
		foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl2 in this.windowSelectAll.LevelBtnList)
		{
			if (pguiToggleButtonCtrl2 == button)
			{
				pguiToggleButtonCtrl2.SetToggleIndex(0);
			}
		}
		this.lvType = SelAccessorySellCtrl.LvType.Invaid;
		return false;
	}

	public bool OnClickContractButton(PguiToggleButtonCtrl button, int index)
	{
		int num = this.windowSelectAll.ContractBtnList.FindIndex((PguiToggleButtonCtrl item) => item == button);
		SelAccessorySellCtrl.CategoryType categoryType = SelAccessorySellCtrl.CategoryType.NotOwned + num;
		if (index == 0)
		{
			if (categoryType != SelAccessorySellCtrl.CategoryType.NotOwned)
			{
				if (categoryType == SelAccessorySellCtrl.CategoryType.Owned)
				{
					this.AddFlag(ref this.categoryMask, SelAccessorySellCtrl.CategoryMask.Owned);
				}
			}
			else
			{
				this.AddFlag(ref this.categoryMask, SelAccessorySellCtrl.CategoryMask.NotOwned);
			}
		}
		else if (categoryType != SelAccessorySellCtrl.CategoryType.NotOwned)
		{
			if (categoryType == SelAccessorySellCtrl.CategoryType.Owned)
			{
				this.RemoveFlag(ref this.categoryMask, SelAccessorySellCtrl.CategoryMask.Owned);
			}
		}
		else
		{
			this.RemoveFlag(ref this.categoryMask, SelAccessorySellCtrl.CategoryMask.NotOwned);
		}
		return true;
	}

	private bool SelectAllWindowButtonCallback(int index)
	{
		if (SelAccessorySellCtrl.LvType.Invaid == this.lvType && this.categoryMask == SelAccessorySellCtrl.CategoryMask.None)
		{
			return true;
		}
		if (1 == index)
		{
			this.sellAccessoryList.Clear();
			SelAccessorySellCtrl.CategoryMask adjustMask = this.categoryMask;
			if (!this.HasFlag(this.categoryMask, SelAccessorySellCtrl.CategoryMask.Rarity))
			{
				this.AddFlag(ref adjustMask, SelAccessorySellCtrl.CategoryMask.Rarity);
			}
			if (!this.HasFlag(this.categoryMask, SelAccessorySellCtrl.CategoryMask.Owner))
			{
				this.AddFlag(ref adjustMask, SelAccessorySellCtrl.CategoryMask.Owner);
			}
			this.sellAccessoryList.Clear();
			this.sellAccessoryList.AddRange(this.dispAccessoryList.FindAll((DataManagerCharaAccessory.Accessory item) => !item.IsLock && ((this.HasFlag(adjustMask, SelAccessorySellCtrl.CategoryMask.S4) && 4 == item.AccessoryData.Rarity.Rarity) || (this.HasFlag(adjustMask, SelAccessorySellCtrl.CategoryMask.S3) && 3 == item.AccessoryData.Rarity.Rarity) || (this.HasFlag(adjustMask, SelAccessorySellCtrl.CategoryMask.S2) && 2 == item.AccessoryData.Rarity.Rarity) || (this.HasFlag(adjustMask, SelAccessorySellCtrl.CategoryMask.S1) && 1 == item.AccessoryData.Rarity.Rarity)) && ((this.HasFlag(adjustMask, SelAccessorySellCtrl.CategoryMask.NotOwned) && item.CharaId == 0) || (this.HasFlag(adjustMask, SelAccessorySellCtrl.CategoryMask.Owned) && item.CharaId != 0)) && ((this.lvType == SelAccessorySellCtrl.LvType.Lv1Only && item.Level <= 1) || ((SelAccessorySellCtrl.LvType.Invaid == this.lvType || SelAccessorySellCtrl.LvType.Unspecified == this.lvType) && 1 <= item.Level))));
			this.UpdateSellAccessoryInfo();
			this.guiData.guiTop.ScrollView.Refresh();
		}
		return true;
	}

	private void ResizeScrollView()
	{
		this.guiData.guiTop.sizeChangeBtnGUI.ResetScrollView();
		List<DataManagerCharaAccessory.Accessory> userAccessoryList = DataManager.DmChAccessory.GetUserAccessoryList();
		this.guiData.guiTop.Txt_None_Noitem.SetActive(userAccessoryList.Count <= 0);
		this.guiData.guiTop.ResizeScrollView(this.dispAccessoryList.Count, this.dispAccessoryList.Count / this.guiData.guiTop.sizeChangeBtnGUI.IconAccessoryParamList[this.guiData.guiTop.sizeChangeBtnGUI.SizeIndex].num + 1);
	}

	public void OnClickMenuReturn()
	{
		SceneCharaEdit.Args args = new SceneCharaEdit.Args
		{
			requestMode = SceneCharaEdit.Mode.ACCESSORY_GROW
		};
		Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneCharaEdit, args);
	}

	private void OnStartItemAccessorySelect(int index, GameObject go)
	{
		for (int i = 0; i < this.guiData.guiTop.sizeChangeBtnGUI.IconAccessoryParamList[this.guiData.guiTop.sizeChangeBtnGUI.SizeIndex].num; i++)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_AccessorySet, go.transform);
			gameObject.name = i.ToString();
			AccessoryUtil.IconAccessorySet iconAccessorySet = new AccessoryUtil.IconAccessorySet(gameObject.transform);
			iconAccessorySet.iconAccessoryCtrl.AddOnClickListener(new IconAccessoryCtrl.OnClick(this.OnTouchPhotoIcon));
			iconAccessorySet.iconAccessoryCtrl.AddOnUpdateStatus(new IconAccessoryCtrl.OnUpdateLockFlag(this.OnUpdateAccessoryStatus));
			iconAccessorySet.baseObj.transform.Find("Icon_Accessory").transform.localScale = this.guiData.guiTop.sizeChangeBtnGUI.IconAccessoryParamList[this.guiData.guiTop.sizeChangeBtnGUI.SizeIndex].scale;
			iconAccessorySet.SetScale(this.guiData.guiTop.sizeChangeBtnGUI.IconAccessoryParamList[this.guiData.guiTop.sizeChangeBtnGUI.SizeIndex].scaleCurrent, this.guiData.guiTop.sizeChangeBtnGUI.IconAccessoryParamList[this.guiData.guiTop.sizeChangeBtnGUI.SizeIndex].scaleCount);
			this.guiData.guiTop.iconAccessorySetList.Add(iconAccessorySet);
			go.GetComponent<GridLayoutGroup>().SetLayoutHorizontal();
		}
	}

	private void OnUpdateItemAccessorySelect(int index, GameObject go)
	{
		for (int i = 0; i < this.guiData.guiTop.sizeChangeBtnGUI.IconAccessoryParamList[this.guiData.guiTop.sizeChangeBtnGUI.SizeIndex].num; i++)
		{
			int num = index * this.guiData.guiTop.sizeChangeBtnGUI.IconAccessoryParamList[this.guiData.guiTop.sizeChangeBtnGUI.SizeIndex].num + i;
			DataManagerCharaAccessory.Accessory accessory = ((num < this.dispAccessoryList.Count) ? this.dispAccessoryList[num] : null);
			GameObject iconObj = go.transform.Find(i.ToString()).gameObject;
			AccessoryUtil.IconAccessorySet iconAccessorySet = this.guiData.guiTop.iconAccessorySetList.Find((AccessoryUtil.IconAccessorySet item) => item.baseObj == iconObj);
			iconAccessorySet.iconAccessoryCtrl.Setup(new IconAccessoryCtrl.SetupParam
			{
				acce = accessory,
				sortType = this.guiData.guiTop.SortFilterBar.SortType
			});
			iconAccessorySet.iconAccessoryCtrl.onReturnAccessoryList = () => this.dispAccessoryList;
			if (accessory != null)
			{
				if (AccessoryUtil.IsDecidedOwner(accessory))
				{
					CharaStaticData charaStaticData = DataManager.DmChara.GetCharaStaticData(accessory.CharaId);
					iconAccessorySet.iconAccessoryCtrl.DispIconCharaMini(charaStaticData != null, charaStaticData);
				}
				if (accessory.IsLock)
				{
					iconAccessorySet.iconAccessoryCtrl.DispImgDisable(true);
				}
				if (this.equippedAccessoryUniqIdList.Contains(accessory.UniqId))
				{
					iconAccessorySet.iconAccessoryCtrl.DispParty(true, true);
				}
			}
			this.UpdateSelectStatus(iconAccessorySet);
		}
	}

	private void OnTouchPhotoIcon(IconAccessoryCtrl iconAcce)
	{
		SoundManager.Play("prd_se_click", false, false);
		if (null != iconAcce && !AccessoryUtil.IsInvalid(iconAcce.accessory))
		{
			bool flag;
			if (this.sellAccessoryList.Contains(iconAcce.accessory))
			{
				flag = this.RemoveFromSellAccessoryList(iconAcce.accessory);
			}
			else
			{
				flag = this.AddToSellAccessoryList(iconAcce.accessory);
			}
			if (flag)
			{
				this.UpdateSelectStatusAll();
				this.UpdateSellAccessoryInfo();
			}
		}
	}

	private void OnChangeSortFilter()
	{
		this.ReloadDataManager();
		this.UpdateSellAccessoryInfo();
		this.ResizeScrollView();
	}

	private void OnUpdateAccessoryStatus(IconAccessoryCtrl iac)
	{
		DataManagerCharaAccessory.Accessory accessory = this.sellAccessoryList.Find((DataManagerCharaAccessory.Accessory x) => x.IsLock);
		this.RemoveFromSellAccessoryList(accessory);
		this.guiData.guiTop.ScrollView.Refresh();
	}

	private SelAccessorySellCtrl.GUI guiData;

	private SelAccessorySellCtrl.SelectAllWindow windowSelectAll;

	private List<DataManagerCharaAccessory.Accessory> dispAccessoryList = new List<DataManagerCharaAccessory.Accessory>();

	private List<long> equippedAccessoryUniqIdList = new List<long>();

	private SelAccessorySellCtrl.CategoryMask categoryMask;

	private SelAccessorySellCtrl.LvType lvType = SelAccessorySellCtrl.LvType.Invaid;

	private IEnumerator serverRequestSell;

	private enum CategoryType
	{
		S1,
		S2,
		S3,
		S4,
		NotOwned,
		Owned
	}

	[Flags]
	private enum CategoryMask
	{
		None = 0,
		S1 = 1,
		S2 = 2,
		S3 = 4,
		S4 = 8,
		Rarity = 15,
		NotOwned = 16,
		Owned = 32,
		Owner = 48
	}

	private enum LvType
	{
		Invaid = -1,
		Lv1Only,
		Unspecified
	}

	public class Top
	{
		public Top(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_FilterOnOff = baseTr.Find("All/WindowAll/SortFilterBtnsAll/Btn_FilterOnOff").GetComponent<PguiButtonCtrl>();
			this.Btn_Sort = baseTr.Find("All/WindowAll/SortFilterBtnsAll/Btn_Sort").GetComponent<PguiButtonCtrl>();
			this.Btn_SortUpDown = baseTr.Find("All/WindowAll/SortFilterBtnsAll/Btn_SortUpDown").GetComponent<PguiButtonCtrl>();
			this.Num_Own = baseTr.Find("All/WindowAll/SortFilterBtnsAll/Num_Own").GetComponent<PguiTextCtrl>();
			this.Btn_CancelAll = baseTr.Find("All/SellInfo/ButtonL").GetComponent<PguiButtonCtrl>();
			this.Btn_SelectAll = baseTr.Find("All/SellInfo/ButtonR").GetComponent<PguiButtonCtrl>();
			this.Btn_buy = baseTr.Find("All/SellInfo/ButtonC").GetComponent<PguiButtonCtrl>();
			this.ScrollView = baseTr.Find("All/WindowAll/ScrollView").GetComponent<ReuseScroll>();
			this.PossessionGold = baseTr.Find("All/SellInfo/SelectPhoto/Num").GetComponent<PguiTextCtrl>();
			this.GetCoin_Num = baseTr.Find("All/SellInfo/GetCoin/Num").GetComponent<PguiTextCtrl>();
			this.sizeChangeBtnGUI = new AccessoryUtil.SizeChangeBtnGUI(baseTr.Find("All/WindowAll/SortFilterBtnsAll/Btn_SizeChange"));
			this.iconAccessorySetList = new List<AccessoryUtil.IconAccessorySet>();
			this.Txt_None_Nofilter = baseTr.Find("All/WindowAll/Txt_None_Nofilter").gameObject;
			this.Txt_None_Nofilter.SetActive(false);
			this.Txt_None_Noitem = baseTr.Find("All/WindowAll/Txt_None_Noitem").gameObject;
			this.Txt_None_Noitem.SetActive(false);
		}

		public void ResizeScrollView(int count, int resize)
		{
			if (this.Txt_None_Noitem.activeSelf)
			{
				this.Txt_None_Nofilter.SetActive(false);
			}
			else
			{
				this.Txt_None_Nofilter.SetActive(count <= 0);
			}
			this.ScrollView.Resize(resize, 0);
		}

		public GameObject baseObj;

		public PguiButtonCtrl Btn_FilterOnOff;

		public PguiButtonCtrl Btn_Sort;

		public PguiButtonCtrl Btn_SortUpDown;

		public PguiTextCtrl Num_Own;

		public PguiButtonCtrl Btn_CancelAll;

		public PguiButtonCtrl Btn_SelectAll;

		public PguiButtonCtrl Btn_buy;

		public ReuseScroll ScrollView;

		public PguiTextCtrl PossessionGold;

		public PguiTextCtrl GetCoin_Num;

		public AccessoryUtil.SizeChangeBtnGUI sizeChangeBtnGUI;

		public SortFilterBtnsAllCtrl SortFilterBar;

		public List<AccessoryUtil.IconAccessorySet> iconAccessorySetList;

		public GameObject Txt_None_Nofilter;

		public GameObject Txt_None_Noitem;
	}

	public class SelectAllWindow
	{
		public SelectAllWindow(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.RarityBtnList = new List<PguiToggleButtonCtrl>
			{
				baseTr.Find("Base/Window/Box01/Btn01").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Base/Window/Box01/Btn02").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Base/Window/Box01/Btn03").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Base/Window/Box01/Btn04").GetComponent<PguiToggleButtonCtrl>()
			};
			this.LevelBtnList = new List<PguiToggleButtonCtrl>
			{
				baseTr.Find("Base/Window/Box02/Btn01").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Base/Window/Box02/Btn02").GetComponent<PguiToggleButtonCtrl>()
			};
			this.ContractBtnList = new List<PguiToggleButtonCtrl>
			{
				baseTr.Find("Base/Window/Box03/Btn01").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Base/Window/Box03/Btn02").GetComponent<PguiToggleButtonCtrl>()
			};
		}

		public GameObject baseObj;

		public PguiOpenWindowCtrl owCtrl;

		public PguiTextCtrl Txt_Massage;

		public List<PguiToggleButtonCtrl> RarityBtnList;

		public List<PguiToggleButtonCtrl> LevelBtnList;

		public List<PguiToggleButtonCtrl> ContractBtnList;
	}

	public class GUI
	{
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.guiTop = new SelAccessorySellCtrl.Top(this.baseObj.transform);
		}

		public GameObject baseObj;

		public SelAccessorySellCtrl.Top guiTop;
	}
}
