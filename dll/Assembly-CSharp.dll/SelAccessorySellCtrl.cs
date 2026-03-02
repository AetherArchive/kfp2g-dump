using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200012D RID: 301
public class SelAccessorySellCtrl : MonoBehaviour
{
	// Token: 0x17000343 RID: 835
	// (get) Token: 0x06000F82 RID: 3970 RVA: 0x000B9B00 File Offset: 0x000B7D00
	private List<DataManagerCharaAccessory.Accessory> sellAccessoryList { get; } = new List<DataManagerCharaAccessory.Accessory>();

	// Token: 0x06000F83 RID: 3971 RVA: 0x000B9B08 File Offset: 0x000B7D08
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

	// Token: 0x06000F84 RID: 3972 RVA: 0x000B9FFC File Offset: 0x000B81FC
	private int GetIconSizeIndex()
	{
		return DataManager.DmGameStatus.MakeUserFlagData().GetIconSizeData(SortFilterDefine.IconPlace.AccessorySell).SizeIndex;
	}

	// Token: 0x06000F85 RID: 3973 RVA: 0x000BA014 File Offset: 0x000B8214
	private void Update()
	{
		if (this.serverRequestSell != null && !this.serverRequestSell.MoveNext())
		{
			this.serverRequestSell = null;
		}
	}

	// Token: 0x06000F86 RID: 3974 RVA: 0x000BA032 File Offset: 0x000B8232
	public void Deestroy()
	{
		if (this.windowSelectAll != null)
		{
			Object.Destroy(this.windowSelectAll.baseObj);
			this.windowSelectAll = null;
		}
	}

	// Token: 0x06000F87 RID: 3975 RVA: 0x000BA053 File Offset: 0x000B8253
	public void Setup()
	{
		this.sellAccessoryList.Clear();
		this.ReloadDataManager();
		this.UpdateUserInfo();
		this.UpdateSellAccessoryInfo();
		this.ResizeScrollView();
	}

	// Token: 0x06000F88 RID: 3976 RVA: 0x000BA078 File Offset: 0x000B8278
	public void ReloadDataManager()
	{
		this.dispAccessoryList.Clear();
		this.dispAccessoryList.AddRange(this.guiData.guiTop.SortFilterBar.GetSortFilteredAccessoryList());
		this.equippedAccessoryUniqIdList.Clear();
		List<CharaPackData> list = new List<CharaPackData>(DataManager.DmChara.GetUserCharaMap().Values);
		this.equippedAccessoryUniqIdList.AddRange(list.FindAll((CharaPackData item) => !AccessoryUtil.IsInvalid(item.dynamicData.accessory)).ConvertAll<long>((CharaPackData item) => item.dynamicData.accessory.UniqId));
	}

	// Token: 0x06000F89 RID: 3977 RVA: 0x000BA124 File Offset: 0x000B8324
	private void UpdateUserInfo()
	{
		this.guiData.guiTop.Num_Own.ReplaceTextByDefault("Param01", DataManager.DmChAccessory.GetUserAccessoryList().Count.ToString() + "/" + DataManager.DmChAccessory.AccessoryStockLimit.ToString());
		this.guiData.guiTop.PossessionGold.text = DataManager.DmItem.GetUserItemData(30101).num.ToString();
	}

	// Token: 0x06000F8A RID: 3978 RVA: 0x000BA1B0 File Offset: 0x000B83B0
	private void UpdateSellAccessoryInfo()
	{
		this.guiData.guiTop.GetCoin_Num.text = this.CalcSellCoin().ToString();
		bool flag = 0 < this.sellAccessoryList.Count;
		this.guiData.guiTop.Btn_CancelAll.SetActEnable(flag, false, false);
		this.guiData.guiTop.Btn_SelectAll.SetActEnable(flag, false, false);
	}

	// Token: 0x06000F8B RID: 3979 RVA: 0x000BA220 File Offset: 0x000B8420
	private int CalcSellCoin()
	{
		int num = 0;
		foreach (DataManagerCharaAccessory.Accessory accessory in this.sellAccessoryList)
		{
			num += accessory.AccessoryData.Rarity.SellPrice;
		}
		return num;
	}

	// Token: 0x06000F8C RID: 3980 RVA: 0x000BA284 File Offset: 0x000B8484
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

	// Token: 0x06000F8D RID: 3981 RVA: 0x000BA2C7 File Offset: 0x000B84C7
	private bool RemoveFromSellAccessoryList(DataManagerCharaAccessory.Accessory accessory)
	{
		return this.sellAccessoryList.Remove(accessory);
	}

	// Token: 0x06000F8E RID: 3982 RVA: 0x000BA2D8 File Offset: 0x000B84D8
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

	// Token: 0x06000F8F RID: 3983 RVA: 0x000BA334 File Offset: 0x000B8534
	private void UpdateSelectStatusAll()
	{
		foreach (AccessoryUtil.IconAccessorySet iconAccessorySet in this.guiData.guiTop.iconAccessorySetList)
		{
			this.UpdateSelectStatus(iconAccessorySet);
		}
	}

	// Token: 0x06000F90 RID: 3984 RVA: 0x000BA394 File Offset: 0x000B8594
	private bool HasFlag(SelAccessorySellCtrl.CategoryMask target, SelAccessorySellCtrl.CategoryMask flag)
	{
		return (target & flag) > SelAccessorySellCtrl.CategoryMask.None;
	}

	// Token: 0x06000F91 RID: 3985 RVA: 0x000BA39C File Offset: 0x000B859C
	private void AddFlag(ref SelAccessorySellCtrl.CategoryMask target, SelAccessorySellCtrl.CategoryMask flag)
	{
		target |= flag;
	}

	// Token: 0x06000F92 RID: 3986 RVA: 0x000BA3A4 File Offset: 0x000B85A4
	private void RemoveFlag(ref SelAccessorySellCtrl.CategoryMask target, SelAccessorySellCtrl.CategoryMask flag)
	{
		target &= ~flag;
	}

	// Token: 0x06000F93 RID: 3987 RVA: 0x000BA3AD File Offset: 0x000B85AD
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

	// Token: 0x06000F94 RID: 3988 RVA: 0x000BA3BC File Offset: 0x000B85BC
	private void OnClickCancelAllButton(PguiButtonCtrl button)
	{
		this.sellAccessoryList.Clear();
		this.UpdateSellAccessoryInfo();
		this.guiData.guiTop.ScrollView.Refresh();
	}

	// Token: 0x06000F95 RID: 3989 RVA: 0x000BA3E4 File Offset: 0x000B85E4
	private void OnClickSelectAllButton(PguiButtonCtrl button)
	{
		this.windowSelectAll.owCtrl.Setup("まとめて選択", "", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, new PguiOpenWindowCtrl.Callback(this.SelectAllWindowButtonCallback), null, false);
		this.windowSelectAll.owCtrl.Open();
	}

	// Token: 0x06000F96 RID: 3990 RVA: 0x000BA430 File Offset: 0x000B8630
	private void OnClickSellButton(PguiButtonCtrl button)
	{
		CanvasManager.HdlAccessoryCheckWindowCtrl.OpenSale(this.sellAccessoryList, new UnityAction(this.AccessoryCheckWindowCallback));
	}

	// Token: 0x06000F97 RID: 3991 RVA: 0x000BA44E File Offset: 0x000B864E
	private void AccessoryCheckWindowCallback()
	{
		this.serverRequestSell = this.ServerRequestSell();
	}

	// Token: 0x06000F98 RID: 3992 RVA: 0x000BA45C File Offset: 0x000B865C
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

	// Token: 0x06000F99 RID: 3993 RVA: 0x000BA540 File Offset: 0x000B8740
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

	// Token: 0x06000F9A RID: 3994 RVA: 0x000BA63C File Offset: 0x000B883C
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

	// Token: 0x06000F9B RID: 3995 RVA: 0x000BA6D0 File Offset: 0x000B88D0
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

	// Token: 0x06000F9C RID: 3996 RVA: 0x000BA7A0 File Offset: 0x000B89A0
	private void ResizeScrollView()
	{
		this.guiData.guiTop.sizeChangeBtnGUI.ResetScrollView();
		List<DataManagerCharaAccessory.Accessory> userAccessoryList = DataManager.DmChAccessory.GetUserAccessoryList();
		this.guiData.guiTop.Txt_None_Noitem.SetActive(userAccessoryList.Count <= 0);
		this.guiData.guiTop.ResizeScrollView(this.dispAccessoryList.Count, this.dispAccessoryList.Count / this.guiData.guiTop.sizeChangeBtnGUI.IconAccessoryParamList[this.guiData.guiTop.sizeChangeBtnGUI.SizeIndex].num + 1);
	}

	// Token: 0x06000F9D RID: 3997 RVA: 0x000BA84C File Offset: 0x000B8A4C
	public void OnClickMenuReturn()
	{
		SceneCharaEdit.Args args = new SceneCharaEdit.Args
		{
			requestMode = SceneCharaEdit.Mode.ACCESSORY_GROW
		};
		Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneCharaEdit, args);
	}

	// Token: 0x06000F9E RID: 3998 RVA: 0x000BA874 File Offset: 0x000B8A74
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

	// Token: 0x06000F9F RID: 3999 RVA: 0x000BAA04 File Offset: 0x000B8C04
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

	// Token: 0x06000FA0 RID: 4000 RVA: 0x000BABAC File Offset: 0x000B8DAC
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

	// Token: 0x06000FA1 RID: 4001 RVA: 0x000BAC1C File Offset: 0x000B8E1C
	private void OnChangeSortFilter()
	{
		this.ReloadDataManager();
		this.UpdateSellAccessoryInfo();
		this.ResizeScrollView();
	}

	// Token: 0x06000FA2 RID: 4002 RVA: 0x000BAC30 File Offset: 0x000B8E30
	private void OnUpdateAccessoryStatus(IconAccessoryCtrl iac)
	{
		DataManagerCharaAccessory.Accessory accessory = this.sellAccessoryList.Find((DataManagerCharaAccessory.Accessory x) => x.IsLock);
		this.RemoveFromSellAccessoryList(accessory);
		this.guiData.guiTop.ScrollView.Refresh();
	}

	// Token: 0x04000DCB RID: 3531
	private SelAccessorySellCtrl.GUI guiData;

	// Token: 0x04000DCC RID: 3532
	private SelAccessorySellCtrl.SelectAllWindow windowSelectAll;

	// Token: 0x04000DCD RID: 3533
	private List<DataManagerCharaAccessory.Accessory> dispAccessoryList = new List<DataManagerCharaAccessory.Accessory>();

	// Token: 0x04000DCE RID: 3534
	private List<long> equippedAccessoryUniqIdList = new List<long>();

	// Token: 0x04000DD0 RID: 3536
	private SelAccessorySellCtrl.CategoryMask categoryMask;

	// Token: 0x04000DD1 RID: 3537
	private SelAccessorySellCtrl.LvType lvType = SelAccessorySellCtrl.LvType.Invaid;

	// Token: 0x04000DD2 RID: 3538
	private IEnumerator serverRequestSell;

	// Token: 0x02000988 RID: 2440
	private enum CategoryType
	{
		// Token: 0x04003D7A RID: 15738
		S1,
		// Token: 0x04003D7B RID: 15739
		S2,
		// Token: 0x04003D7C RID: 15740
		S3,
		// Token: 0x04003D7D RID: 15741
		S4,
		// Token: 0x04003D7E RID: 15742
		NotOwned,
		// Token: 0x04003D7F RID: 15743
		Owned
	}

	// Token: 0x02000989 RID: 2441
	[Flags]
	private enum CategoryMask
	{
		// Token: 0x04003D81 RID: 15745
		None = 0,
		// Token: 0x04003D82 RID: 15746
		S1 = 1,
		// Token: 0x04003D83 RID: 15747
		S2 = 2,
		// Token: 0x04003D84 RID: 15748
		S3 = 4,
		// Token: 0x04003D85 RID: 15749
		S4 = 8,
		// Token: 0x04003D86 RID: 15750
		Rarity = 15,
		// Token: 0x04003D87 RID: 15751
		NotOwned = 16,
		// Token: 0x04003D88 RID: 15752
		Owned = 32,
		// Token: 0x04003D89 RID: 15753
		Owner = 48
	}

	// Token: 0x0200098A RID: 2442
	private enum LvType
	{
		// Token: 0x04003D8B RID: 15755
		Invaid = -1,
		// Token: 0x04003D8C RID: 15756
		Lv1Only,
		// Token: 0x04003D8D RID: 15757
		Unspecified
	}

	// Token: 0x0200098B RID: 2443
	public class Top
	{
		// Token: 0x06003C24 RID: 15396 RVA: 0x001D9DCC File Offset: 0x001D7FCC
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

		// Token: 0x06003C25 RID: 15397 RVA: 0x001D9F2C File Offset: 0x001D812C
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

		// Token: 0x04003D8E RID: 15758
		public GameObject baseObj;

		// Token: 0x04003D8F RID: 15759
		public PguiButtonCtrl Btn_FilterOnOff;

		// Token: 0x04003D90 RID: 15760
		public PguiButtonCtrl Btn_Sort;

		// Token: 0x04003D91 RID: 15761
		public PguiButtonCtrl Btn_SortUpDown;

		// Token: 0x04003D92 RID: 15762
		public PguiTextCtrl Num_Own;

		// Token: 0x04003D93 RID: 15763
		public PguiButtonCtrl Btn_CancelAll;

		// Token: 0x04003D94 RID: 15764
		public PguiButtonCtrl Btn_SelectAll;

		// Token: 0x04003D95 RID: 15765
		public PguiButtonCtrl Btn_buy;

		// Token: 0x04003D96 RID: 15766
		public ReuseScroll ScrollView;

		// Token: 0x04003D97 RID: 15767
		public PguiTextCtrl PossessionGold;

		// Token: 0x04003D98 RID: 15768
		public PguiTextCtrl GetCoin_Num;

		// Token: 0x04003D99 RID: 15769
		public AccessoryUtil.SizeChangeBtnGUI sizeChangeBtnGUI;

		// Token: 0x04003D9A RID: 15770
		public SortFilterBtnsAllCtrl SortFilterBar;

		// Token: 0x04003D9B RID: 15771
		public List<AccessoryUtil.IconAccessorySet> iconAccessorySetList;

		// Token: 0x04003D9C RID: 15772
		public GameObject Txt_None_Nofilter;

		// Token: 0x04003D9D RID: 15773
		public GameObject Txt_None_Noitem;
	}

	// Token: 0x0200098C RID: 2444
	public class SelectAllWindow
	{
		// Token: 0x06003C26 RID: 15398 RVA: 0x001D9F68 File Offset: 0x001D8168
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

		// Token: 0x04003D9E RID: 15774
		public GameObject baseObj;

		// Token: 0x04003D9F RID: 15775
		public PguiOpenWindowCtrl owCtrl;

		// Token: 0x04003DA0 RID: 15776
		public PguiTextCtrl Txt_Massage;

		// Token: 0x04003DA1 RID: 15777
		public List<PguiToggleButtonCtrl> RarityBtnList;

		// Token: 0x04003DA2 RID: 15778
		public List<PguiToggleButtonCtrl> LevelBtnList;

		// Token: 0x04003DA3 RID: 15779
		public List<PguiToggleButtonCtrl> ContractBtnList;
	}

	// Token: 0x0200098D RID: 2445
	public class GUI
	{
		// Token: 0x06003C27 RID: 15399 RVA: 0x001DA064 File Offset: 0x001D8264
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.guiTop = new SelAccessorySellCtrl.Top(this.baseObj.transform);
		}

		// Token: 0x04003DA4 RID: 15780
		public GameObject baseObj;

		// Token: 0x04003DA5 RID: 15781
		public SelAccessorySellCtrl.Top guiTop;
	}
}
