using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AEAuth3;
using SGNFW.Common;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelShopCtrl : MonoBehaviour
{
	private SelShopCtrl.State currentStatus { get; set; }

	private SelShopCtrl.State requestStatus { get; set; }

	private int currentTabIndex { get; set; }

	public bool IsEnableScene
	{
		get
		{
			return this.requestStatus > SelShopCtrl.State.INVALID;
		}
		set
		{
		}
	}

	public HashSet<int> DispNewGoodsId { get; set; } = new HashSet<int>();

	public SceneManager.SceneName RequestNextScene { get; private set; }

	public object RequestNextSceneArgs { get; private set; }

	private string SearchText { get; set; }

	public void Init()
	{
		this.requestStatus = SelShopCtrl.State.INVALID;
		this.shopBtnDataList = new List<List<SelShopCtrl.ShopBtn>>();
		this.shopDataList = new List<List<ShopData>>();
		for (int i = 0; i < 4; i++)
		{
			this.shopBtnDataList.Add(new List<SelShopCtrl.ShopBtn>());
			this.shopDataList.Add(new List<ShopData>());
		}
		this.renderTextureChara = null;
		this.mainObj = Object.Instantiate<GameObject>((GameObject)AssetManager.GetAssetData("SceneShop/GUI/Prefab/GUI_Shop"), base.transform);
		this.guiData = new SelShopCtrl.GUI(this.mainObj.transform);
		GameObject gameObject = AssetManager.GetAssetData("Cmn/GUI/Prefab/GUI_Cmn_ShopWindow_Normal") as GameObject;
		this.windowBuyConfirmNormal = new SelShopCtrl.WindowBuyConfirmNormal(Object.Instantiate<Transform>(gameObject.transform, Singleton<CanvasManager>.Instance.SystemMiddleArea).transform);
		this.windowBuyConfirmNormal.Btn_Plus.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickPlusButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.windowBuyConfirmNormal.Btn_Minus.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickMinusButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.windowBuyConfirmNormal.Btn_PhotoGrow.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickPhotoGrowButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.windowBuyConfirmNormal.Btn_PhotoSell.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickPhotoSellButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.windowBuyConfirmNormal.SliderBar.onValueChanged.AddListener(new UnityAction<float>(this.OnSliderValueChanged));
		gameObject = AssetManager.GetAssetData("Cmn/GUI/Prefab/GUI_Cmn_ShopWindow_KiraKira") as GameObject;
		this.windowBuyConfirmKiraKira = new SelShopCtrl.WindowBuyConfirmKiraKira(Object.Instantiate<Transform>(gameObject.transform, Singleton<CanvasManager>.Instance.SystemMiddleArea).transform);
		this.windowBuyConfirmKiraKira.Btn_Plus.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickPlusButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.windowBuyConfirmKiraKira.Btn_Minus.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickMinusButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.windowBuyConfirmKiraKira.Btn_PhotoGrow.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickPhotoGrowButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.windowBuyConfirmKiraKira.Btn_PhotoSell.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickPhotoSellButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.windowBuyConfirmKiraKira.SliderBar.onValueChanged.AddListener(new UnityAction<float>(this.OnSliderValueChanged));
		this.windowBuyConfirmKiraKira.Btn_PurchaseConfirm.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickPurchaseConfirmButton), PguiButtonCtrl.SoundType.DEFAULT);
		gameObject = AssetManager.GetAssetData("Cmn/GUI/Prefab/GUI_Cmn_ShopWindow_Result") as GameObject;
		this.windowBuyEnd = new SelShopCtrl.WindowBuyEnd(Object.Instantiate<Transform>(gameObject.transform, Singleton<CanvasManager>.Instance.SystemMiddleArea).transform);
		gameObject = AssetManager.GetAssetData("Cmn/GUI/Prefab/GUI_Cmn_ShopWindow_Friends") as GameObject;
		this.windowBuyConfirmGrow = new SelShopCtrl.WindowBuyConfirmGrow(Object.Instantiate<Transform>(gameObject.transform, Singleton<CanvasManager>.Instance.SystemMiddleArea).transform);
		this.windowBuyConfirmGrow.GrowCharaScroll.InitForce();
		this.windowBuyConfirmGrow.GrowCharaScroll.onUpdateItem = new Action<int, GameObject>(this.OnUpdateItem);
		this.windowBuyConfirmGrow.GrowCharaScroll.Setup(8, 0);
		this.windowBuyConfirmGrow.Btn_Plus.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickPlusButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.windowBuyConfirmGrow.Btn_Minus.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickMinusButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.windowBuyConfirmGrow.SliderBar.onValueChanged.AddListener(new UnityAction<float>(this.OnSliderValueChanged));
		gameObject = AssetManager.GetAssetData("Cmn/GUI/Prefab/GUI_Cmn_ShopWindow_Friends_KiraKira") as GameObject;
		this.windowBuyConfirmGrowKiraKira = new SelShopCtrl.WindowBuyConfirmGrowKiraKira(Object.Instantiate<Transform>(gameObject.transform, Singleton<CanvasManager>.Instance.SystemMiddleArea).transform);
		this.windowBuyConfirmGrowKiraKira.GrowCharaScroll.InitForce();
		this.windowBuyConfirmGrowKiraKira.GrowCharaScroll.onUpdateItem = new Action<int, GameObject>(this.OnUpdateItem);
		this.windowBuyConfirmGrowKiraKira.GrowCharaScroll.Setup(8, 0);
		this.windowBuyConfirmGrowKiraKira.Btn_Plus.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickPlusButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.windowBuyConfirmGrowKiraKira.Btn_Minus.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickMinusButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.windowBuyConfirmGrowKiraKira.SliderBar.onValueChanged.AddListener(new UnityAction<float>(this.OnSliderValueChanged));
		this.windowBuyConfirmGrowKiraKira.Btn_PurchaseConfirm.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickPurchaseConfirmButton), PguiButtonCtrl.SoundType.DEFAULT);
		gameObject = AssetManager.GetAssetData("Cmn/GUI/Prefab/GUI_Cmn_ShopWindow_Bulk") as GameObject;
		GameObject gameObject2 = AssetManager.GetAssetData("Cmn/GUI/Prefab/CmnShop_ListSet_BulkBuyItem") as GameObject;
		this.windowBuyConfirmBulk = new SelShopCtrl.WindowBuyConfirmBulk(Object.Instantiate<Transform>(gameObject.transform, Singleton<CanvasManager>.Instance.SystemMiddleArea).transform, gameObject2);
		this.guiData.BtnNext_Left.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickPrevButton), PguiButtonCtrl.SoundType.MENU_SLIDE);
		this.guiData.BtnNext_Right.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickNextButton), PguiButtonCtrl.SoundType.MENU_SLIDE);
		this.guiShopWindowList = new List<SelShopCtrl.GUIShopWindow>();
		for (int j = 0; j < 2; j++)
		{
			GameObject gameObject3 = this.mainObj.transform.Find("Window" + j.ToString()).gameObject;
			this.guiShopWindowList.Add(new SelShopCtrl.GUIShopWindow(gameObject3.transform));
			ReuseScroll component = gameObject3.transform.Find("InBase/ScrollView").GetComponent<ReuseScroll>();
			component.InitForce();
			component.onStartItem = (Action<int, GameObject>)Delegate.Combine(component.onStartItem, new Action<int, GameObject>(this.OnStartBuyItemList));
			component.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(component.onUpdateItem, new Action<int, GameObject>(this.OnUpdateBuyItemList));
			component.Setup(0, 0);
		}
		this.windowTextSearchChange = new SelShopCtrl.WindowTextSearchChange(this.mainObj.transform);
		this.SearchText = "";
		this.windowTextSearchChange.FirstInputField.onEndEdit.AddListener(delegate(string str)
		{
			this.windowTextSearchChange.FirstInputField.text = PrjUtil.ModifiedComment(str);
			this.SearchText = this.windowTextSearchChange.FirstInputField.text;
			this.windowTextSearchChange.SecondInputField.text = this.SearchText;
			this.SetFilteredItemList();
		});
		this.windowTextSearchChange.SecondInputField.onEndEdit.AddListener(delegate(string str)
		{
			this.windowTextSearchChange.SecondInputField.text = PrjUtil.ModifiedComment(str);
			this.SearchText = this.windowTextSearchChange.SecondInputField.text;
			this.windowTextSearchChange.FirstInputField.text = this.SearchText;
			this.SetFilteredItemList();
		});
		this.windowTextSearchChange.firstResetButton.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickResetSearch), PguiButtonCtrl.SoundType.DEFAULT);
		this.windowTextSearchChange.secondResetButton.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickResetSearch), PguiButtonCtrl.SoundType.DEFAULT);
		this.windowBuyConfirmBulk.ItemScroll.InitForce();
		this.windowBuyConfirmBulk.ItemScroll.onStartItem = new Action<int, GameObject>(this.OnStartBulkItem);
		this.windowBuyConfirmBulk.ItemScroll.onUpdateItem = new Action<int, GameObject>(this.OnUpdateBulkItem);
		this.gridGUIMap = new Dictionary<int, SelShopCtrl.GridGUI>();
		GameObject gameObject4 = new GameObject();
		gameObject4.AddComponent<RectTransform>();
		gameObject4.name = "SelShopAssistantCtrl";
		gameObject4.transform.SetParent(this.mainObj.transform, false);
		this.selAssistantCtrl = gameObject4.AddComponent<SelAssistantCtrl>();
		this.selAssistantCtrl.transform.SetAsLastSibling();
		this.selAssistantCtrl.Init(SelAssistantCtrl.Scene.SHOP);
	}

	public void Setup(SceneShopArgs args = null)
	{
		this.requestStatus = SelShopCtrl.State.INVALID;
		this.RequestNextScene = SceneManager.SceneName.None;
		this.RequestNextSceneArgs = null;
		this.ReleaseShopData();
		this.currentShopIndex = 0;
		this.currentTabIndex = 0;
		this.openShop = args;
		base.StartCoroutine(this.RequestSetup());
		this.guiData.markLockAssistantEdit.Setup(new MarkLockCtrl.SetupParam
		{
			releaseFlag = QuestUtil.IsDispDhole(),
			tagetObject = this.guiData.BtnAssistantEdit.gameObject,
			text = "メインストーリー\n9章11話クリアで再解放"
		}, false);
	}

	public void Teardown()
	{
		if (this.renderTextureChara != null)
		{
			DataManager.DmChara.RequestCharaTouchCount(this.renderTextureChara.DispCharaId, this.renderTextureChara.TapCount);
		}
		this.ReleaseShopData();
	}

	public void UpdateSel()
	{
		if (this.renderTextureChara != this.selAssistantCtrl.renderTextureChara)
		{
			this.renderTextureChara = this.selAssistantCtrl.renderTextureChara;
		}
		if (this.currentCharaId != this.selAssistantCtrl.CurrentCharaId)
		{
			this.currentCharaId = this.selAssistantCtrl.CurrentCharaId;
		}
		if (this.requestStatus != this.currentStatus)
		{
			this.currentStatus = this.requestStatus;
			this.ConvertBGImage(this.currentTabIndex, this.currentShopIndex);
			SelShopCtrl.State currentStatus = this.currentStatus;
			if (currentStatus != SelShopCtrl.State.SHOP_BUY_CHECK)
			{
				if (currentStatus != SelShopCtrl.State.SHOP_BUY_END)
				{
					for (int i = 0; i < 4; i++)
					{
						for (int j = 0; j < this.shopDataList[i].Count; j++)
						{
							if (this.shopDataList[i][j].priceItemId != 0)
							{
								ItemData userItemData = DataManager.DmItem.GetUserItemData(this.shopDataList[i][j].priceItemId);
								this.shopBtnDataList[i][j].PriceTxt.text = userItemData.num.ToString();
							}
						}
					}
				}
				else
				{
					base.StartCoroutine(this.RequestWindowBuyEnd());
				}
			}
			else
			{
				base.StartCoroutine(this.RequestWindowBuyCheck());
			}
		}
		this.guiData.BtnAssistantEdit.gameObject.SetActive(this.currentStatus == SelShopCtrl.State.TOP);
		for (int k = 0; k < 4; k++)
		{
			if (this.shopBtnDataList[k].Count > 0 && this.shopDataList[k] != null)
			{
				int num = 0;
				while (num < this.shopBtnDataList[k].Count && num < this.shopDataList[k].Count)
				{
					if (this.shopBtnDataList[k][num].Mark != null)
					{
						bool flag = this.shopDataList[k][num].category == ShopData.Category.MONTHLYPACK;
						if (flag)
						{
							flag = DataManager.DmMonthlyPack.IsEnableMonthlyPack(TimeManager.Now);
						}
						this.shopBtnDataList[k][num].Mark.SetActive(flag);
					}
					num++;
				}
			}
		}
		if (this.windowChangeAnime != null && !this.windowChangeAnime.MoveNext())
		{
			this.windowChangeAnime = null;
		}
	}

	public void Destroy()
	{
		this.windowBuyEnd = null;
		if (this.windowBuyConfirmNormal != null && this.windowBuyConfirmNormal.owCtrl != null)
		{
			Object.Destroy(this.windowBuyConfirmNormal.owCtrl.gameObject);
			this.windowBuyConfirmNormal = null;
		}
		if (this.windowBuyConfirmKiraKira != null && this.windowBuyConfirmKiraKira.owCtrl != null)
		{
			Object.Destroy(this.windowBuyConfirmKiraKira.owCtrl.gameObject);
			this.windowBuyConfirmKiraKira = null;
		}
		if (this.windowBuyConfirmGrow != null && this.windowBuyConfirmGrow.owCtrl != null)
		{
			Object.Destroy(this.windowBuyConfirmGrow.owCtrl.gameObject);
			this.windowBuyConfirmGrow = null;
		}
		if (this.windowBuyConfirmGrowKiraKira != null && this.windowBuyConfirmGrowKiraKira.owCtrl != null)
		{
			Object.Destroy(this.windowBuyConfirmGrowKiraKira.owCtrl.gameObject);
			this.windowBuyConfirmGrowKiraKira = null;
		}
		if (this.windowBuyConfirmBulk != null && this.windowBuyConfirmBulk.owCtrl != null)
		{
			Object.Destroy(this.windowBuyConfirmBulk.owCtrl.gameObject);
			this.windowBuyConfirmBulk = null;
		}
		if (this.guiData != null)
		{
			Object.Destroy(this.guiData.baseObj);
			this.guiData = null;
		}
	}

	private IEnumerator RequestSetup()
	{
		DataManager.DmShop.RequestGetShopList();
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		this.SetupShopList();
		if (this.openShop != null && this.requestStatus == SelShopCtrl.State.SHOP && this.openShop.shopItem > 0)
		{
			List<ShopData.ItemOne> lst = this.shopDataList[this.currentTabIndex][this.currentShopIndex].oneDataList.FindAll((ShopData.ItemOne itm) => itm.itemId == this.openShop.shopItem && itm.itemNum > 0);
			if (lst != null && lst.Count > 0)
			{
				lst.Sort((ShopData.ItemOne a, ShopData.ItemOne b) => a.priceItemNum / a.itemNum - b.priceItemNum / b.itemNum);
				while (this.currentStatus != SelShopCtrl.State.SHOP)
				{
					yield return null;
				}
				GameObject gameObject = new GameObject("btn");
				gameObject.AddComponent<PguiDataHolder>().id = lst[0].goodsId;
				this.OnClickItemButton(gameObject.AddComponent<PguiButtonCtrl>());
			}
			lst = null;
		}
		yield break;
	}

	private void SetupShopList()
	{
		this.SetupHiddenObject();
		for (ShopData.TabCategory tabCategory = ShopData.TabCategory.ALL; tabCategory < ShopData.TabCategory.MAX; tabCategory++)
		{
			this.SetupShopListCreate(tabCategory);
			this.SetupShopListGUI(tabCategory);
		}
		SelAssistantCtrl.Mode mode = ((!QuestUtil.IsDispDhole()) ? SelAssistantCtrl.Mode.DISP_DHOLE : SelAssistantCtrl.Mode.TOP);
		this.selAssistantCtrl.ChangeMode(mode);
		if (mode == SelAssistantCtrl.Mode.DISP_DHOLE)
		{
			this.selAssistantCtrl.currentMode = SelAssistantCtrl.Mode.DISP_DHOLE;
		}
		this.RequestSetupShopAssistant();
		this.SetupShop();
		CanvasManager.SetBgTexture("selbg_shop");
		this.SetupTab();
		this.ItemUpdate();
		this.allItemDataList = this.shopDataList[this.currentTabIndex][this.currentShopIndex].oneDataList;
		this.SearchText = "";
		this.windowTextSearchChange.FirstInputField.text = "";
		this.windowTextSearchChange.SecondInputField.text = "";
		foreach (SelShopCtrl.GUIShopWindow guishopWindow in this.guiShopWindowList)
		{
			guishopWindow.SwitchModeButton.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickCheckButton), PguiButtonCtrl.SoundType.DEFAULT);
			guishopWindow.ExchangeButton.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickExchangeButton), PguiButtonCtrl.SoundType.DEFAULT);
		}
	}

	private void SetupShop()
	{
		bool flag = false;
		if (this.openShop != null)
		{
			int num = this.shopDataList[0].FindIndex((ShopData item) => item.shopId == this.openShop.shopId);
			this.currentShopIndex = num;
			this.currentTabIndex = 0;
			if (this.currentShopIndex != -1)
			{
				flag = true;
				this.ChangeCurrentShopData(this.currentShopIndex, 0, true);
				this.StartShop();
			}
		}
		if (!flag)
		{
			this.requestStatus = SelShopCtrl.State.TOP;
			this.currentShopIndex = 0;
			this.guiData.TabGroup.gameObject.SetActive(true);
		}
	}

	private void SetupHiddenObject()
	{
		this.DispNewGoodsId.Clear();
		this.guiData.BtnNext_Left.gameObject.SetActive(false);
		this.guiData.BtnNext_Right.gameObject.SetActive(false);
		foreach (SelShopCtrl.GUIShopWindow guishopWindow in this.guiShopWindowList)
		{
			guishopWindow.BaseObj.SetActive(false);
		}
	}

	private void SetupShopListCreate(ShopData.TabCategory category)
	{
		List<ShopData> list = DataManager.DmShop.GetShopDataList(true, true, category);
		this.shopDataList[(int)category] = list;
	}

	private void SetupShopListGUI(ShopData.TabCategory tabCategory)
	{
		this.guiData.ScrollViewList[(int)tabCategory].movementType = ScrollRect.MovementType.Elastic;
		this.guiData.ScrollViewList[(int)tabCategory].content = this.guiData.ScrollContentList[(int)tabCategory];
		float num = 0f;
		int num2 = 0;
		foreach (ShopData shopData in this.shopDataList[(int)tabCategory])
		{
			GameObject gameObject = Manager.Create((shopData.category == ShopData.Category.EVENT || shopData.category == ShopData.Category.EVENT_NOITEM_HIDE) ? "SceneShop/GUI/Prefab/Shop_BtnEvent" : "SceneShop/GUI/Prefab/Shop_BtnNone", this.guiData.ScrollContentList[(int)tabCategory], null, null, SGNFW.uGUI.Layer.UI);
			gameObject.name = num2.ToString();
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.SetParent(this.guiData.ScrollContentList[(int)tabCategory]);
			this.OnStartShopList(num2, gameObject, (int)tabCategory);
			num -= this.SPACE_SIZE;
			component.anchoredPosition = new Vector2(0f, num);
			num -= component.rect.height;
			component.anchorMin = new Vector2(0.5f, 1f);
			component.anchorMax = new Vector2(0.5f, 1f);
			component.pivot = new Vector2(0.5f, 1f);
			num2++;
		}
		Vector2 sizeDelta = this.guiData.ScrollViewList[(int)tabCategory].content.sizeDelta;
		Vector2 vector = this.guiData.ScrollViewList[(int)tabCategory].content.anchorMin;
		vector.y = 1f;
		this.guiData.ScrollViewList[(int)tabCategory].content.anchorMin = vector;
		vector = this.guiData.ScrollViewList[(int)tabCategory].content.anchorMax;
		vector.y = 1f;
		this.guiData.ScrollViewList[(int)tabCategory].content.anchorMax = vector;
		this.guiData.ScrollViewList[(int)tabCategory].content.sizeDelta = sizeDelta;
		this.guiData.ScrollContentList[(int)tabCategory].sizeDelta = new Vector2(0f, -num);
		this.guiData.ScrollViewList[(int)tabCategory].gameObject.SetActive(true);
		this.guiData.ScrollBarList[(int)tabCategory].gameObject.SetActive(true);
	}

	private void SetupTab()
	{
		for (int i = 0; i < 4; i++)
		{
			Transform transform = this.guiData.TabGroup.gameObject.transform.Find(string.Format("Grid/Tab0{0}", i + 1));
			if (this.shopDataList[i].Count <= 0)
			{
				transform.GetComponent<PguiTabCtrl>().SetActEnable(false);
			}
			else if (i > 0)
			{
				PguiTextCtrl component = transform.transform.Find("BaseImage/Txt").gameObject.GetComponent<PguiTextCtrl>();
				component.text = (string.IsNullOrWhiteSpace(this.shopDataList[i][0].tabName) ? component.text : this.shopDataList[i][0].tabName);
			}
		}
		this.currentTabIndex = 0;
		this.guiData.TabGroup.Setup(this.currentTabIndex, new PguiTabGroupCtrl.OnSelectTab(this.OnSelectTab));
		this.OnSelectTab(this.currentTabIndex);
	}

	private bool OnSelectTab(int tabIdx)
	{
		int num = 0;
		foreach (GameObject gameObject in this.guiData.TabList)
		{
			if (null != gameObject)
			{
				gameObject.SetActive(tabIdx == num);
			}
			num++;
		}
		this.currentTabIndex = tabIdx;
		return true;
	}

	private void SortOneDataList(ShopData shopData)
	{
		ShopData.Sort sortType = shopData.SortType;
		if (sortType != ShopData.Sort.Invalid && sortType == ShopData.Sort.Name)
		{
			PrjUtil.InsertionSort<ShopData.ItemOne>(ref shopData.oneDataList, (ShopData.ItemOne a, ShopData.ItemOne b) => a.goodsId.CompareTo(b.goodsId));
			PrjUtil.InsertionSort<ShopData.ItemOne>(ref shopData.oneDataList, (ShopData.ItemOne a, ShopData.ItemOne b) => a.itemId.CompareTo(b.itemId));
			PrjUtil.InsertionSort<ShopData.ItemOne>(ref shopData.oneDataList, (ShopData.ItemOne a, ShopData.ItemOne b) => PrjUtil.CompareByName(a.itemId, b.itemId));
			PrjUtil.IsPriorityZeroSort(shopData);
			PrjUtil.InsertionSort<ShopData.ItemOne>(ref shopData.oneDataList, (ShopData.ItemOne a, ShopData.ItemOne b) => a.isSoldout.CompareTo(b.isSoldout));
		}
	}

	private IEnumerator BuyEndUpdate()
	{
		while (!this.windowBuyEnd.owCtrl.FinishedClose())
		{
			yield return null;
		}
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		this.ItemUpdate();
		yield break;
	}

	private void ItemUpdate()
	{
		if (this.shopDataList[this.currentTabIndex].Count == 0)
		{
			return;
		}
		if (this.shopDataList[this.currentTabIndex][this.currentShopIndex].category == ShopData.Category.PURCHASE)
		{
			return;
		}
		if (this.shopDataList[this.currentTabIndex][this.currentShopIndex].category == ShopData.Category.MONTHLYPACK)
		{
			return;
		}
		PguiTextCtrl numOwn = this.guiShopWindowList[this.currentShopWindowFlip].NumOwn;
		int priceItemId = this.shopDataList[this.currentTabIndex][this.currentShopIndex].priceItemId;
		if (priceItemId != 0)
		{
			numOwn.text = DataManagerItem.GetUserHaveNum(priceItemId).ToString();
		}
		this.guiShopWindowList[this.currentShopWindowFlip].ScrollView.Refresh();
		this.guiShopWindowList[this.currentShopWindowFlip].TxtNoneObj.SetActive(this.allItemDataList == null || this.allItemDataList.Count <= 0);
		this.guiShopWindowList[this.currentShopWindowFlip].TxtNoneFilteredObj.SetActive(this.shopDataList[this.currentTabIndex][this.currentShopIndex].oneDataList.Count <= 0);
	}

	private IEnumerator RequestWindowBuyCheck()
	{
		this.windowBuyConfirmCurrent.owCtrl.Setup(PrjUtil.MakeMessage(this.windowBuyConfirmCurrent.title), PrjUtil.MakeMessage(this.windowBuyConfirmCurrent.info), PguiOpenWindowCtrl.GetButtonPreset(this.windowBuyConfirmCurrent.notBuyPhoto ? PguiOpenWindowCtrl.PresetType.CLOSE_GREEN : PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, new PguiOpenWindowCtrl.Callback(this.OnClickOwButton), null, false);
		this.windowBuyConfirmCurrent.buyCount = Mathf.Min(1, this.windowBuyConfirmCurrent.buyMax);
		this.windowBuyConfirmCurrent.owCtrl.Open();
		this.BuyItemUpdate(this.windowBuyConfirmCurrent.isBuy);
		if (this.windowBuyConfirmCurrent.tryingToBuyNotComeCharaClothes)
		{
			while (!this.windowBuyConfirmCurrent.owCtrl.FinishedOpen())
			{
				yield return null;
			}
			CanvasManager.HdlOpenWindowBasic.Setup("確認", "<color=red>衣装の対象フレンズが未加入です</color>", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, null, delegate
			{
				this.isDispWarning = false;
			}, false);
			CanvasManager.HdlOpenWindowBasic.Open();
		}
		yield break;
	}

	private IEnumerator RequestWindowBuyEnd()
	{
		while (!this.windowBuyConfirmCurrent.owCtrl.FinishedClose())
		{
			yield return null;
		}
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		if (this.currentShopIndex < this.shopDataList[this.currentTabIndex].Count)
		{
			this.SortOneDataList(this.shopDataList[this.currentTabIndex][this.currentShopIndex]);
		}
		if (!this.isBulkExChangeMode)
		{
			this.windowBuyEnd.owCtrl.Setup("交換完了", "を交換しました。", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, new PguiOpenWindowCtrl.Callback(this.OnClickOwButton), null, false);
			if (this.windowBuyConfirmCurrent.replaceBeforeItemData != null && ItemDef.Kind.ACHIEVEMENT == ItemDef.Id2Kind(this.windowBuyConfirmCurrent.replaceBeforeItemData.itemId))
			{
				DataManagerAchievement.AchievementStaticData achievementData = DataManager.DmAchievement.GetAchievementData(this.windowBuyConfirmCurrent.replaceBeforeItemData.itemId);
				this.windowBuyConfirmCurrent.buyCount = achievementData.duplicateItemNum;
			}
			this.windowBuyEnd.Txt_ItemName.text = this.windowBuyConfirmCurrent.Txt_BuyItemName.text;
			this.windowBuyEnd.Txt_ItemCount.text = string.Format("を{0}", this.windowBuyConfirmCurrent.buyCount) + this.windowBuyConfirmCurrent.buyComment;
			this.windowBuyEnd.Txt_ItemReq.text = this.windowBuyConfirmCurrent.reqComment;
			this.windowBuyEnd.Txt_BuyBeforeMoney.text = this.windowBuyConfirmCurrent.Txt_BuyBeforeMoney.text;
			this.windowBuyEnd.Txt_BuyAfterMoney.text = this.windowBuyConfirmCurrent.Txt_BuyAfterMoney.text;
			this.windowBuyEnd.Txt_BuyBeforeCount.text = this.windowBuyConfirmCurrent.Txt_BuyBeforeCount.text;
			this.windowBuyEnd.Txt_BuyAfterCount.text = this.windowBuyConfirmCurrent.Txt_BuyAfterCount.text;
			bool flag = this.windowBuyConfirmCurrent.itemData.setItems != null && this.windowBuyConfirmCurrent.itemData.setItems.Count > 0;
			this.windowBuyEnd.Parts_ItemUseInfo.SetActive(!flag);
			this.SetMoneyImage(this.windowBuyEnd.UseInfoImage, 40f);
			this.SetMoneyImage(this.windowBuyEnd.UseMoneyImage, 40f);
			SoundManager.Play("prd_se_shop_payment", false, false);
			this.windowBuyEnd.owCtrl.Open();
		}
		else
		{
			this.ShowItemResultWindow(this.BuyitemData);
			SoundManager.Play("prd_se_shop_payment", false, false);
			this.SetBuyItemMode(false);
		}
		yield break;
	}

	private bool OnClickOwButton(int index)
	{
		SelShopCtrl.State currentStatus = this.currentStatus;
		if (currentStatus != SelShopCtrl.State.SHOP_BUY_CHECK)
		{
			if (currentStatus != SelShopCtrl.State.SHOP_BUY_END)
			{
				this.requestStatus = SelShopCtrl.State.SHOP;
			}
			else
			{
				this.requestStatus = SelShopCtrl.State.SHOP;
				this.renderTextureChara = this.selAssistantCtrl.renderTextureChara;
				this.renderTextureChara.SetupInterruptMotion(new RenderTextureChara.InterruptMotion
				{
					key = CharaMotionDefine.ActKey.SHOP_BUY
				}, true);
				base.StartCoroutine(this.BuyEndUpdate());
				this.allItemDataList = this.shopDataList[this.currentTabIndex][this.currentShopIndex].oneDataList;
				this.SearchText = "";
				this.windowTextSearchChange.FirstInputField.text = "";
				this.windowTextSearchChange.SecondInputField.text = "";
				ScrollRect component = this.guiShopWindowList[this.currentShopWindowFlip].ScrollView.GetComponent<ScrollRect>();
				float verticalNormalizedPosition = component.verticalNormalizedPosition;
				this.UpdateItemList();
				component.verticalNormalizedPosition = verticalNormalizedPosition;
				SoundManager.PlayVoice(RenderTextureChara.CharaId2CueName(this.currentCharaId), VOICE_TYPE.SHP02);
			}
		}
		else
		{
			if (this.isDispWarning)
			{
				return false;
			}
			if (index == 1)
			{
				if (!this.isBulkExChangeMode)
				{
					DataManager.DmShop.RequestActionBuyShopItem(this.windowBuyConfirmCurrent.itemData.goodsId, this.windowBuyConfirmCurrent.buyCount);
				}
				else
				{
					DataManager.DmShop.RequestActionBulkBuyShopItem(this.itemDataList);
				}
				this.requestStatus = SelShopCtrl.State.SHOP_BUY_END;
			}
			else
			{
				this.BuyitemData.Clear();
				this.requestStatus = SelShopCtrl.State.SHOP;
			}
		}
		return true;
	}

	private void OnStartShopList(int index, GameObject go, int tabCategoryId)
	{
		if (this.shopDataList[tabCategoryId].Count <= index)
		{
			return;
		}
		HashSet<int> oldGoodsList = DataManager.DmShop.GetOldGoodsIdList();
		SelShopCtrl.ShopBtn shopBtn = new SelShopCtrl.ShopBtn(go.transform);
		ShopData.Category category = this.shopDataList[tabCategoryId][index].category;
		if (category != ShopData.Category.PURCHASE)
		{
			if (category != ShopData.Category.MONTHLYPACK)
			{
				if (this.shopDataList[tabCategoryId][index].priceItemId != 0)
				{
					ItemData userItemData = DataManager.DmItem.GetUserItemData(this.shopDataList[tabCategoryId][index].priceItemId);
					if (this.shopDataList[tabCategoryId][index].category == ShopData.Category.EVENT || this.shopDataList[tabCategoryId][index].category == ShopData.Category.EVENT_NOITEM_HIDE)
					{
						shopBtn.ShopBanner.banner = "Texture2D/Shop/" + this.shopDataList[tabCategoryId][index].bannerImageName;
					}
					else
					{
						shopBtn.ShopBtn_Icon.SetRawImage(userItemData.staticData.GetIconName(), true, false, null);
						if (this.shopDataList[tabCategoryId][index].category == ShopData.Category.OTHER_NOITEM_HIDE)
						{
							shopBtn.baseRepSprite.Replace(1);
						}
					}
					shopBtn.ShopBtn_PriceIcon.transform.parent.gameObject.SetActive(true);
					shopBtn.ShopBtn_PriceIcon.SetRawImage(userItemData.staticData.GetIconName(), true, false, null);
					shopBtn.PriceTxt.text = userItemData.num.ToString();
				}
				else
				{
					shopBtn.ShopBtn_PriceIcon.transform.parent.gameObject.SetActive(false);
				}
			}
			else
			{
				shopBtn.ShopBtn_Icon.SetRawImage("Texture2D/Icon_Item/icon_item_passport", true, false, null);
				shopBtn.ShopBtn_PriceIcon.transform.parent.gameObject.SetActive(false);
			}
		}
		else
		{
			shopBtn.ShopBtn_Icon.SetRawImage("Texture2D/Shop_BuyImg/shopbtn_icon03", true, false, null);
			shopBtn.ShopBtn_PriceIcon.transform.parent.gameObject.SetActive(false);
		}
		if (shopBtn.Mark != null)
		{
			shopBtn.Mark.SetActive(false);
		}
		shopBtn.index = index;
		shopBtn.BtnNone.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickGetButton), PguiButtonCtrl.SoundType.DEFAULT);
		bool flag = this.shopDataList[tabCategoryId][index].oneDataList.Exists((ShopData.ItemOne item) => item.notOpenDispFlag && !this.ItemBuyCheck(item).IsLockNgReason && !oldGoodsList.Contains(item.goodsId));
		shopBtn.Cmn_Mark_New.SetActive(flag);
		this.guiData.back.SetActive(true);
		this.guiData.back.transform.SetSiblingIndex(0);
		this.guiData.ScrollBarList[0].gameObject.SetActive(this.shopDataList[0].Count >= 5);
		this.shopBtnDataList[tabCategoryId].Add(shopBtn);
		this.shopBtnDataList[tabCategoryId][index].itemData = this.shopDataList[tabCategoryId][index];
		this.shopBtnDataList[tabCategoryId][index].Txt.text = this.shopDataList[tabCategoryId][index].shopName;
	}

	private void OnStartBuyItemList(int index, GameObject go)
	{
		GameObject gameObject = (GameObject)AssetManager.GetAssetData("Cmn/GUI/Prefab/CmnShop_BuyItem");
		for (int i = 0; i < SelShopCtrl.Shop_ListSet_Btn.SCROLL_ITEM_NUN_H; i++)
		{
			GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject, go.transform);
			gameObject2.AddComponent<PguiDataHolder>();
			SelShopCtrl.Shop_ListSet_Btn shop_ListSet_Btn = new SelShopCtrl.Shop_ListSet_Btn(gameObject2.transform);
			shop_ListSet_Btn.CmnShop_BuyItem.name = i.ToString();
			shop_ListSet_Btn.CmnShop_BuyItem.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickItemButton), PguiButtonCtrl.SoundType.DEFAULT);
			shop_ListSet_Btn.Info_Btn.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickItemInfo), PguiButtonCtrl.SoundType.DEFAULT);
			shop_ListSet_Btn.Cmn_Btn_Lock.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickItemLock), PguiButtonCtrl.SoundType.DEFAULT);
		}
	}

	private void OnUpdateBuyItemList(int index, GameObject go)
	{
		HashSet<int> oldGoodsIdList = DataManager.DmShop.GetOldGoodsIdList();
		for (int i = 0; i < SelShopCtrl.Shop_ListSet_Btn.SCROLL_ITEM_NUN_H; i++)
		{
			GameObject gameObject = go.transform.Find(i.ToString()).gameObject;
			SelShopCtrl.Shop_ListSet_Btn shop_ListSet_Btn = new SelShopCtrl.Shop_ListSet_Btn(gameObject.transform);
			int num = i + SelShopCtrl.Shop_ListSet_Btn.SCROLL_ITEM_NUN_H * index;
			if (num < 0 || this.shopDataList[this.currentTabIndex][this.currentShopIndex].oneDataList.Count <= num)
			{
				gameObject.SetActive(false);
			}
			else
			{
				gameObject.SetActive(true);
				ShopData.ItemOne itemOne = this.shopDataList[this.currentTabIndex][this.currentShopIndex].oneDataList[num];
				PguiButtonCtrl component = gameObject.GetComponent<PguiButtonCtrl>();
				component.GetComponent<PguiDataHolder>().id = itemOne.goodsId;
				shop_ListSet_Btn.Price_Num_Txt.text = itemOne.priceItemNum.ToString();
				ItemData itemData = new ItemData(itemOne.itemId, itemOne.itemNum);
				string text = ((itemData.staticData == null) ? "" : itemData.staticData.GetName());
				if (ItemDef.Kind.CHARA == ItemDef.Id2Kind(itemOne.itemId))
				{
					CharaStaticData charaStaticData = DataManager.DmChara.GetCharaStaticData(DataManager.DmItem.ItemId2ChraId(itemOne.itemId));
					text = charaStaticData.baseData.NickName + charaStaticData.baseData.charaName;
				}
				shop_ListSet_Btn.Txt_BuyInfo01.text = text;
				PguiTextCtrl txt_BuyInfo = shop_ListSet_Btn.Txt_BuyInfo02;
				int num2 = itemOne.maxChangeNum - itemOne.nowChangeNum;
				bool flag = itemOne.maxChangeNum != 0 && num2 <= 0;
				txt_BuyInfo.text = "";
				shop_ListSet_Btn.Txt_ItemOwn.gameObject.SetActive(ItemDef.Kind.PRESET != ItemDef.Id2Kind(itemOne.itemId));
				shop_ListSet_Btn.Txt_ItemOwn.ReplaceTextByDefault("Param01", DataManagerItem.GetUserHaveNum(itemOne.itemId).ToString());
				shop_ListSet_Btn.Select_CheckBox.SetActive(this.isBulkExChangeMode);
				shop_ListSet_Btn.Select_Mark.SetActive(this.selectGoodsIds.Contains(itemOne.goodsId));
				if (flag)
				{
					txt_BuyInfo.text = ((ItemDef.Kind.CHARA == ItemDef.Id2Kind(itemOne.itemId)) ? "これ以上しょうたい\nできません。" : "売り切れです。");
					shop_ListSet_Btn.Mark_Sold.SetActive(itemOne.maxChangeNum != 0);
					shop_ListSet_Btn.Mark_Sold_Txt.text = ((ItemDef.Kind.CHARA == ItemDef.Id2Kind(itemOne.itemId)) ? "しょうたい済み" : "売り切れ");
				}
				else
				{
					if (DataManagerItem.isOverUserHaveMaxNum(itemOne.itemId, itemOne.itemNum) && itemOne.itemId == DataManagerPhoto.PHOTO_STOCK_RELEASEITEM_ID)
					{
						txt_BuyInfo.text = "解放の上限数を超える\nため交換できません";
						itemOne.resetInfoPrefix = "";
					}
					else if (0 < num2)
					{
						txt_BuyInfo.text = "あと" + num2.ToString() + "回";
					}
					DateTime? dateTime = null;
					bool flag2 = false;
					if (!itemOne.IsInfinitie)
					{
						dateTime = new DateTime?(itemOne.endTime);
					}
					if (!this.shopDataList[this.currentTabIndex][this.currentShopIndex].IsInfinitie && (dateTime == null || dateTime > this.shopDataList[this.currentTabIndex][this.currentShopIndex].endTime))
					{
						dateTime = new DateTime?(this.shopDataList[this.currentTabIndex][this.currentShopIndex].endTime);
					}
					if (itemOne.resetTime != null && (dateTime == null || dateTime > itemOne.resetTime))
					{
						flag2 = true;
						txt_BuyInfo.text = itemOne.resetInfoPrefix + txt_BuyInfo.text;
					}
					if (dateTime != null && !flag2)
					{
						PguiTextCtrl pguiTextCtrl = txt_BuyInfo;
						pguiTextCtrl.text = string.Concat(new string[]
						{
							pguiTextCtrl.text,
							"\u3000",
							dateTime.Value.Month.ToString(),
							"/",
							dateTime.Value.Day.ToString(),
							"まで"
						});
					}
					shop_ListSet_Btn.Mark_Sold.SetActive(false);
				}
				this.SetMoneyImage(shop_ListSet_Btn.Price_Item_Icon, 40f);
				shop_ListSet_Btn.Info_Btn.gameObject.SetActive(ItemDef.Kind.PRESET == ItemDef.Id2Kind(itemOne.itemId));
				SelShopCtrl.ItemBuyCheckResult itemBuyCheckResult = this.ItemBuyCheck(itemOne);
				shop_ListSet_Btn.Icon_Item.Setup(DataManager.DmItem.GetItemStaticBase(itemOne.itemId));
				shop_ListSet_Btn.Icon_Item.transform.SetAsFirstSibling();
				shop_ListSet_Btn.Item_Num_Txt.text = "×" + itemOne.itemNum.ToString();
				if (ItemDef.Kind.CHARA == ItemDef.Id2Kind(itemOne.itemId) && itemOne.charaStatusId > 0)
				{
					IconCharaCtrl iconCharaCtrl = shop_ListSet_Btn.Icon_Item.GetIconCharaCtrl();
					if (iconCharaCtrl != null)
					{
						CharaPackData charaPackData = CharaPackData.MakeShopCharaData(itemOne.itemId, itemOne.charaStatusId);
						iconCharaCtrl.Setup(charaPackData, SortFilterDefine.SortType.LEVEL, false, new CharaWindowCtrl.DetailParamSetting(CharaWindowCtrl.DetailParamSetting.Preset.SHOP_DETAIL, null), 0, -1, 0);
						iconCharaCtrl.DispPhotoPocketLevel(true);
					}
				}
				component.ReloadChildObject();
				if (!this.isBulkExChangeMode)
				{
					component.SetActEnable(itemBuyCheckResult.isBuy, true, false);
				}
				else
				{
					int num3 = DataManagerItem.GetUserHaveNum(itemOne.priceItemId) - this.selectItemMoney;
					bool flag3 = itemBuyCheckResult.isBuy && itemOne.maxChangeNum != 0 && num3 >= num2 * itemOne.priceItemNum && ItemDef.Kind.CHARA != ItemDef.Id2Kind(itemOne.itemId);
					bool flag4 = this.itemDataList.Contains(itemOne);
					component.SetActEnable(flag3 || flag4, false, false);
				}
				shop_ListSet_Btn.Cmn_Mark_New.gameObject.SetActive(false);
				if (itemOne.notOpenDispFlag && !itemBuyCheckResult.IsLockNgReason && !oldGoodsIdList.Contains(itemOne.goodsId))
				{
					AEImage component2 = shop_ListSet_Btn.Cmn_Mark_New.GetComponent<AEImage>();
					if (component2 != null)
					{
						component2.color = Color.white;
					}
					shop_ListSet_Btn.Cmn_Mark_New.gameObject.SetActive(true);
					this.DispNewGoodsId.Add(itemOne.goodsId);
				}
				shop_ListSet_Btn.Cmn_Btn_Lock.gameObject.SetActive(false);
				if (itemOne.notOpenDispFlag && !itemBuyCheckResult.isBuy && itemBuyCheckResult.IsLockNgReason)
				{
					shop_ListSet_Btn.Cmn_Btn_Lock.gameObject.SetActive(true);
				}
			}
		}
	}

	private void OnUpdateItem(int index, GameObject go)
	{
		if (this.charaGrowNeedInfoList != null && index < this.charaGrowNeedInfoList.Count)
		{
			new SelShopCtrl.GUIFriendsUsecaseLabel(go, this.charaGrowNeedInfoList[index]);
			go.SetActive(true);
			return;
		}
		go.SetActive(false);
	}

	public void OnStartBulkItem(int index, GameObject go)
	{
		SelShopCtrl.GridGUI gridGUI;
		if (!this.gridGUIMap.ContainsKey(index))
		{
			gridGUI = new SelShopCtrl.GridGUI(go);
			this.columnCount = gridGUI.gridLayoutGroup.constraintCount;
			gridGUI.iconItemList = new List<IconItemCtrl>();
			for (int i = 0; i < this.columnCount; i++)
			{
				GameObject gameObject = new GameObject();
				gameObject.name = "iconBase";
				gameObject.AddComponent<RectTransform>();
				gameObject.transform.SetParent(gridGUI.baseObj.transform, false);
				GameObject gameObject2 = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, gameObject.transform);
				gameObject2.name = "ItemIcon" + i.ToString("D2");
				IconItemCtrl component = gameObject2.GetComponent<IconItemCtrl>();
				gridGUI.iconItemList.Add(component);
			}
			this.gridGUIMap.Add(index, gridGUI);
		}
		else
		{
			gridGUI = this.gridGUIMap[index];
		}
		this.ItemSetup(index, gridGUI);
	}

	public void OnUpdateBulkItem(int index, GameObject go)
	{
		this.ItemSetup(index, new SelShopCtrl.GridGUI(go));
	}

	private void ItemSetup(int index, SelShopCtrl.GridGUI gridGUI)
	{
		int num = 0;
		foreach (IconItemCtrl iconItemCtrl in gridGUI.iconItemList)
		{
			int num2 = this.columnCount * index + num;
			if (this.BuyitemData.Count <= num2)
			{
				iconItemCtrl.gameObject.SetActive(false);
			}
			else
			{
				iconItemCtrl.gameObject.SetActive(true);
				iconItemCtrl.Setup(DataManager.DmItem.GetItemStaticBase(this.BuyitemData[num2].id), this.BuyitemData[num2].num, new IconItemCtrl.SetupParam
				{
					useInfo = true,
					viewItemCount = true
				});
				num++;
			}
		}
	}

	private void OnClickPhotoGrowButton(PguiButtonCtrl button)
	{
		this.windowBuyConfirmCurrent.owCtrl.ForceClose();
		this.RequestNextScene = SceneManager.SceneName.SceneCharaEdit;
		this.RequestNextSceneArgs = new SceneCharaEdit.Args
		{
			requestMode = SceneCharaEdit.Mode.PHOTO_GROW,
			requestSubMode = SelPhotoGrowCtrl.Mode.PHOTO_SELECT,
			menuBackSceneName = SceneManager.SceneName.SceneShop,
			menuBackSceneArgs = new SceneShopArgs
			{
				shopId = ((this.currentShopIndex < this.shopDataList[this.currentTabIndex].Count) ? this.shopDataList[this.currentTabIndex][this.currentShopIndex].shopId : 0),
				disableSceneBack = true
			}
		};
	}

	private void OnClickPhotoSellButton(PguiButtonCtrl button)
	{
		this.windowBuyConfirmCurrent.owCtrl.ForceClose();
		this.RequestNextScene = SceneManager.SceneName.SceneCharaEdit;
		this.RequestNextSceneArgs = new SceneCharaEdit.Args
		{
			requestMode = SceneCharaEdit.Mode.PHOTO_SELL,
			menuBackSceneName = SceneManager.SceneName.SceneShop,
			menuBackSceneArgs = new SceneShopArgs
			{
				shopId = ((this.currentShopIndex < this.shopDataList[this.currentTabIndex].Count) ? this.shopDataList[this.currentTabIndex][this.currentShopIndex].shopId : 0),
				disableSceneBack = true
			}
		};
	}

	private void OnSliderValueChanged(float val)
	{
		if (this.currentStatus != SelShopCtrl.State.SHOP_BUY_CHECK)
		{
			return;
		}
		this.windowBuyConfirmCurrent.buyCount = (int)Mathf.Max((float)this.windowBuyConfirmCurrent.buyMax * val / this.windowBuyConfirmCurrent.SliderBar.maxValue, 1f);
		this.BuyItemUpdate(true);
	}

	private void OnClickPlusButton(PguiButtonCtrl button)
	{
		if (this.currentStatus != SelShopCtrl.State.SHOP_BUY_CHECK)
		{
			return;
		}
		this.windowBuyConfirmCurrent.buyCount++;
		if (this.windowBuyConfirmCurrent.buyCount > this.windowBuyConfirmCurrent.buyMax)
		{
			this.windowBuyConfirmCurrent.buyCount = 1;
		}
		this.windowBuyConfirmCurrent.SliderBar.value = (float)this.windowBuyConfirmCurrent.buyCount;
		this.BuyItemUpdate(true);
	}

	private void OnClickMinusButton(PguiButtonCtrl button)
	{
		if (this.currentStatus != SelShopCtrl.State.SHOP_BUY_CHECK)
		{
			return;
		}
		this.windowBuyConfirmCurrent.buyCount--;
		if (this.windowBuyConfirmCurrent.buyCount <= 0)
		{
			this.windowBuyConfirmCurrent.buyCount = this.windowBuyConfirmCurrent.buyMax;
		}
		this.windowBuyConfirmCurrent.SliderBar.value = (float)this.windowBuyConfirmCurrent.buyCount;
		this.BuyItemUpdate(true);
	}

	private void OnClickPurchaseConfirmButton(PguiButtonCtrl button)
	{
		if (this.currentStatus != SelShopCtrl.State.SHOP_BUY_CHECK)
		{
			return;
		}
		CanvasManager.HdlPurchaseConfirmWindow.Initialize(this.windowBuyConfirmCurrent.itemData);
	}

	private void OnClickMaxButton(PguiButtonCtrl button)
	{
		if (this.currentStatus != SelShopCtrl.State.SHOP_BUY_CHECK)
		{
			return;
		}
		this.windowBuyConfirmCurrent.buyCount = this.windowBuyConfirmCurrent.buyMax;
		this.windowBuyConfirmCurrent.SliderBar.value = (float)this.windowBuyConfirmCurrent.buyCount;
		this.BuyItemUpdate(true);
	}

	private void OnClickMinButton(PguiButtonCtrl button)
	{
		if (this.currentStatus != SelShopCtrl.State.SHOP_BUY_CHECK)
		{
			return;
		}
		this.windowBuyConfirmCurrent.buyCount = 1;
		this.windowBuyConfirmCurrent.SliderBar.value = (float)this.windowBuyConfirmCurrent.buyCount;
		this.BuyItemUpdate(true);
	}

	private void OnClickGetButton(PguiButtonCtrl button)
	{
		SelShopCtrl.ShopBtn shopBtn = null;
		Predicate<SelShopCtrl.ShopBtn> <>9__0;
		foreach (List<SelShopCtrl.ShopBtn> list in this.shopBtnDataList)
		{
			Predicate<SelShopCtrl.ShopBtn> predicate;
			if ((predicate = <>9__0) == null)
			{
				predicate = (<>9__0 = (SelShopCtrl.ShopBtn item) => item.BtnNone == button);
			}
			shopBtn = list.Find(predicate);
			if (shopBtn != null)
			{
				break;
			}
		}
		if (shopBtn == null)
		{
			return;
		}
		int index = shopBtn.index;
		if (this.shopDataList[this.currentTabIndex][index].category == ShopData.Category.PURCHASE)
		{
			CanvasManager.HdlSelPurchaseStoneWindowCtrl.Setup(PurchaseProductOne.TabType.Invalid);
			this.guiShopWindowList[this.currentShopWindowFlip].SwitchModeButton.gameObject.SetActive(false);
			return;
		}
		if (this.shopDataList[this.currentTabIndex][index].category == ShopData.Category.MONTHLYPACK)
		{
			CanvasManager.HdlSelMonthlyPackWindowCtrl.Setup();
			this.guiShopWindowList[this.currentShopWindowFlip].SwitchModeButton.gameObject.SetActive(false);
			return;
		}
		this.ChangeCurrentShopData(shopBtn.index, 0, true);
		this.ConvertBGImage(this.currentTabIndex, this.currentShopIndex);
		this.guiShopWindowList[this.currentShopWindowFlip].SwitchModeButton.gameObject.SetActive(this.shopDataList[this.currentTabIndex][index].category == ShopData.Category.EVENT);
		this.StartShop();
	}

	private void StartShop()
	{
		this.guiData.back.SetActive(false);
		this.guiData.TabGroup.gameObject.SetActive(false);
		foreach (PguiScrollbar pguiScrollbar in this.guiData.ScrollBarList)
		{
			pguiScrollbar.gameObject.SetActive(false);
		}
		foreach (ScrollRect scrollRect in this.guiData.ScrollViewList)
		{
			scrollRect.gameObject.SetActive(false);
		}
		this.guiShopWindowList[this.currentShopWindowFlip].BaseObj.SetActive(true);
		bool flag = this.GetShopCount() > 1;
		this.guiData.BtnNext_Left.gameObject.SetActive(flag);
		this.guiData.BtnNext_Right.gameObject.SetActive(flag);
		this.SetBuyItemMode(false);
		this.UpdateItemList();
		this.ItemUpdate();
		this.EndTimeUpdate();
		this.requestStatus = SelShopCtrl.State.SHOP;
	}

	private void EndTimeUpdate()
	{
		PguiTextCtrl endTimeText = this.guiShopWindowList[this.currentShopWindowFlip].EndTimeText;
		endTimeText.gameObject.SetActive(!this.shopDataList[this.currentTabIndex][this.currentShopIndex].IsInfinitie);
		if (!this.shopDataList[this.currentTabIndex][this.currentShopIndex].IsInfinitie)
		{
			endTimeText.text = TimeManager.MakeTimeResidueText(TimeManager.Now, this.shopDataList[this.currentTabIndex][this.currentShopIndex].endTime, false, true);
			ShopData.Category category = this.shopDataList[this.currentTabIndex][this.currentShopIndex].category;
			if (category == ShopData.Category.EVENT || category == ShopData.Category.EVENT_NOITEM_HIDE)
			{
				endTimeText.GetComponent<PguiGradientCtrl>().SetGameObjectById("EVENT");
				return;
			}
			if (category == ShopData.Category.OTHER_NOITEM_HIDE)
			{
				endTimeText.GetComponent<PguiGradientCtrl>().SetGameObjectById("SPECIAL_A");
				return;
			}
			endTimeText.GetComponent<PguiGradientCtrl>().SetGameObjectById("NORMAL");
		}
	}

	private void ChangeCurrentShopData(int index, int next, bool positionReset)
	{
		this.currentShopIndex = index;
		if (index < 0)
		{
			this.currentShopIndex = this.shopDataList[this.currentTabIndex].Count - 1;
		}
		if (this.shopDataList[this.currentTabIndex].Count <= index)
		{
			this.currentShopIndex = 0;
		}
		if (this.guiShopWindowList != null && this.guiShopWindowList.Count > this.currentShopWindowFlip)
		{
			if (this.currentShopIndex < this.shopDataList[this.currentTabIndex].Count)
			{
				this.SortOneDataList(this.shopDataList[this.currentTabIndex][this.currentShopIndex]);
			}
			if (this.shopDataList[this.currentTabIndex][this.currentShopIndex].category == ShopData.Category.PURCHASE || this.shopDataList[this.currentTabIndex][this.currentShopIndex].category == ShopData.Category.MONTHLYPACK)
			{
				if (next != 0)
				{
					this.ChangeCurrentShopData(this.currentShopIndex + next, next, positionReset);
					return;
				}
			}
			else
			{
				this.currentShopWindowFlip = ((this.currentShopWindowFlip == 0) ? 1 : 0);
				PguiTextCtrl component = this.guiShopWindowList[this.currentShopWindowFlip].BaseObj.transform.Find("Title/Txt").GetComponent<PguiTextCtrl>();
				component.text = this.shopDataList[this.currentTabIndex][this.currentShopIndex].shopName;
				PguiReplaceSpriteCtrl component2 = this.guiShopWindowList[this.currentShopWindowFlip].BaseObj.transform.Find("Title/Tex").GetComponent<PguiReplaceSpriteCtrl>();
				ShopData.Category category = this.shopDataList[this.currentTabIndex][this.currentShopIndex].category;
				if (category != ShopData.Category.EVENT && category != ShopData.Category.EVENT_NOITEM_HIDE)
				{
					if (category != ShopData.Category.OTHER_NOITEM_HIDE)
					{
						component.GetComponent<PguiGradientCtrl>().SetGameObjectById("NORMAL");
						component2.Replace(0);
					}
					else
					{
						component.GetComponent<PguiGradientCtrl>().SetGameObjectById("SPECIAL_A");
						component2.Replace(2);
					}
				}
				else
				{
					component.GetComponent<PguiGradientCtrl>().SetGameObjectById("EVENT");
					component2.Replace(1);
				}
				PguiRawImageCtrl component3 = this.guiShopWindowList[this.currentShopWindowFlip].BaseObj.transform.Find("Title/Txt/Icon_Item").GetComponent<PguiRawImageCtrl>();
				component3.gameObject.SetActive(true);
				this.SetMoneyImage(component3, component3.GetComponent<RectTransform>().sizeDelta.x);
				PguiTextCtrl numOwn = this.guiShopWindowList[this.currentShopWindowFlip].NumOwn;
				PguiRawImageCtrl component4 = this.guiShopWindowList[this.currentShopWindowFlip].BaseObj.transform.Find("ItemOwnBase/Icon_Img").GetComponent<PguiRawImageCtrl>();
				numOwn.transform.parent.gameObject.SetActive(true);
				int priceItemId = this.shopDataList[this.currentTabIndex][this.currentShopIndex].priceItemId;
				if (priceItemId != 0)
				{
					numOwn.text = DataManagerItem.GetUserHaveNum(priceItemId).ToString();
				}
				else
				{
					numOwn.transform.parent.gameObject.SetActive(false);
				}
				this.SetMoneyImage(component4, 40f);
				this.allItemDataList = this.shopDataList[this.currentTabIndex][this.currentShopIndex].oneDataList;
				this.UpdateItemList();
				if (positionReset)
				{
					foreach (SelShopCtrl.GUIShopWindow guishopWindow in this.guiShopWindowList)
					{
						guishopWindow.BaseObj.SetActive(true);
					}
					this.guiShopWindowList[this.currentShopWindowFlip].BaseObj.GetComponent<SimpleAnimation>().ExPauseAnimationLastFrame(SimpleAnimation.ExPguiStatus.END_SUB);
					this.guiShopWindowList[(this.currentShopWindowFlip == 0) ? 1 : 0].BaseObj.GetComponent<SimpleAnimation>().ExPauseAnimationLastFrame(SimpleAnimation.ExPguiStatus.START_SUB);
				}
			}
		}
	}

	private void OnClickPageButtonInternal(int move)
	{
		if (this.windowChangeAnime != null)
		{
			return;
		}
		this.shopDataList[this.currentTabIndex][this.currentShopIndex].oneDataList = this.allItemDataList;
		SimpleAnimation component = this.guiShopWindowList[this.currentShopWindowFlip].BaseObj.GetComponent<SimpleAnimation>();
		this.guiShopWindowList[this.currentShopWindowFlip].BaseObj.SetActive(true);
		this.ChangeCurrentShopData(this.currentShopIndex + move, move, false);
		this.ConvertBGImage(this.currentTabIndex, this.currentShopIndex);
		this.guiShopWindowList[this.currentShopWindowFlip].BaseObj.SetActive(true);
		this.guiShopWindowList[this.currentShopWindowFlip].SwitchModeButton.gameObject.SetActive(this.shopDataList[this.currentTabIndex][this.currentShopIndex].category == ShopData.Category.EVENT);
		this.guiShopWindowList[this.currentShopWindowFlip].ExchangeButton.gameObject.SetActive(false);
		this.SetBuyItemMode(false);
		this.guiShopWindowList[this.currentShopWindowFlip].ScrollView.Refresh();
		SimpleAnimation component2 = this.guiShopWindowList[this.currentShopWindowFlip].BaseObj.GetComponent<SimpleAnimation>();
		this.EndTimeUpdate();
		if (this.SearchText.Length > 0)
		{
			this.SetFilteredItemList();
		}
		this.windowChangeAnime = SelShopCtrl.WindowChangeAnime(move, component, component2);
	}

	private static IEnumerator WindowChangeAnime(int move, SimpleAnimation animOut, SimpleAnimation animIn)
	{
		bool isOutFinish = false;
		animOut.ExPlayAnimation((move > 0) ? SimpleAnimation.ExPguiStatus.START_SUB : SimpleAnimation.ExPguiStatus.START, delegate
		{
			isOutFinish = true;
			animIn.ExPlayAnimation((move > 0) ? SimpleAnimation.ExPguiStatus.END_SUB : SimpleAnimation.ExPguiStatus.END, null);
		});
		while (!isOutFinish || animIn.ExGetTime() < 0.4f)
		{
			yield return null;
		}
		yield break;
	}

	private void OnClickCheckButton(PguiButtonCtrl button)
	{
		this.SetBuyItemMode(!this.isBulkExChangeMode);
		this.UpdateItemList();
	}

	private void OnClickPrevButton(PguiButtonCtrl button)
	{
		this.OnClickPageButtonInternal(-1);
	}

	private void OnClickNextButton(PguiButtonCtrl button)
	{
		this.OnClickPageButtonInternal(1);
	}

	private void SelectGoods(ShopData.ItemOne shopItemOne)
	{
		int num = shopItemOne.maxChangeNum - shopItemOne.nowChangeNum;
		if (!this.itemDataList.Contains(shopItemOne))
		{
			this.itemDataList.Add(shopItemOne);
			this.selectGoodsIds.Add(shopItemOne.goodsId);
			this.selectItemMoney += shopItemOne.priceItemNum * num;
		}
		else
		{
			this.itemDataList.Remove(shopItemOne);
			this.selectGoodsIds.Remove(shopItemOne.goodsId);
			this.selectItemMoney -= shopItemOne.priceItemNum * num;
		}
		this.guiShopWindowList[this.currentShopWindowFlip].ExchangeButton.SetActEnable(this.selectGoodsIds.Count > 0, false, false);
		this.guiShopWindowList[this.currentShopWindowFlip].ScrollView.Refresh();
	}

	private void OnClickItemButton(PguiButtonCtrl button)
	{
		ShopData.ItemOne itemOne = this.shopDataList[this.currentTabIndex][this.currentShopIndex].oneDataList.Find((ShopData.ItemOne item) => item.goodsId == button.GetComponent<PguiDataHolder>().id);
		if (itemOne == null)
		{
			return;
		}
		if (!this.isBulkExChangeMode)
		{
			bool flag = ItemDef.IsGrowKindByShop(ItemDef.Id2Kind(itemOne.itemId));
			if (flag)
			{
				SelShopCtrl.WindowBuyConfirmGrow windowBuyConfirmGrow;
				if (itemOne.priceItemId == 30100 || itemOne.priceItemId == 30002)
				{
					this.windowBuyConfirmCurrent = this.windowBuyConfirmGrowKiraKira;
					windowBuyConfirmGrow = this.windowBuyConfirmGrowKiraKira;
				}
				else
				{
					this.windowBuyConfirmCurrent = this.windowBuyConfirmGrow;
					windowBuyConfirmGrow = this.windowBuyConfirmGrow;
				}
				this.charaGrowNeedInfoList = CharaGrowItemInfo.MakeCharaGrowNeedInfoList(itemOne.itemId);
				windowBuyConfirmGrow.GrowCharaScroll.Resize(this.charaGrowNeedInfoList.Count, 0);
				windowBuyConfirmGrow.Message_NotFriends.SetActive(this.charaGrowNeedInfoList.Count == 0);
			}
			else if (itemOne.priceItemId == 30100 || itemOne.priceItemId == 30002)
			{
				this.windowBuyConfirmCurrent = this.windowBuyConfirmKiraKira;
			}
			else
			{
				this.windowBuyConfirmCurrent = this.windowBuyConfirmNormal;
			}
			this.windowBuyConfirmCurrent.itemData = itemOne;
			this.windowBuyConfirmCurrent.replaceBeforeItemData = null;
			if (ItemDef.Kind.CLOTHES == ItemDef.Id2Kind(itemOne.itemId) && 0 < DataManager.DmItem.GetUserItemData(itemOne.itemId).num)
			{
				this.windowBuyConfirmCurrent.replaceBeforeItemData = itemOne;
				CharaClothStatic charaClothesStaticData = DataManager.DmChara.GetCharaClothesStaticData(this.windowBuyConfirmCurrent.itemData.itemId);
				this.windowBuyConfirmCurrent.itemData = itemOne.Clone();
				this.windowBuyConfirmCurrent.itemData.itemId = 30130;
				this.windowBuyConfirmCurrent.itemData.itemNum = charaClothesStaticData.replaceItemNum;
			}
			if (ItemDef.Kind.ACHIEVEMENT == ItemDef.Id2Kind(itemOne.itemId) && DataManager.DmAchievement.GetHaveAchievementData(itemOne.itemId) != null)
			{
				this.windowBuyConfirmCurrent.replaceBeforeItemData = itemOne;
				this.windowBuyConfirmCurrent.itemData = itemOne.Clone();
				DataManagerAchievement.AchievementStaticData achievementData = DataManager.DmAchievement.GetAchievementData(itemOne.itemId);
				this.windowBuyConfirmCurrent.itemData.itemId = achievementData.duplicateItemId;
				this.windowBuyConfirmCurrent.itemData.itemNum = achievementData.duplicateItemNum;
			}
			SelShopCtrl.ItemBuyCheckResult itemBuyCheckResult = this.ItemBuyCheck(this.windowBuyConfirmCurrent.itemData);
			this.requestStatus = SelShopCtrl.State.SHOP_BUY_CHECK;
			ItemData itemData = new ItemData(this.windowBuyConfirmCurrent.itemData.itemId, this.windowBuyConfirmCurrent.itemData.itemNum);
			new ItemData(this.windowBuyConfirmCurrent.itemData.priceItemId, this.windowBuyConfirmCurrent.itemData.priceItemNum);
			int num = this.CalcBuyItemMax();
			this.windowBuyConfirmCurrent.isBuy = itemBuyCheckResult.isBuy;
			this.windowBuyConfirmCurrent.buyMax = num;
			this.windowBuyConfirmCurrent.info = itemData.staticData.GetInfo();
			bool flag2 = false;
			if (ItemDef.Kind.CLOTHES == itemData.staticData.GetKind())
			{
				CharaClothStatic charaClothesStaticData2 = DataManager.DmChara.GetCharaClothesStaticData(itemData.staticData.GetId());
				CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(charaClothesStaticData2.CharaId);
				flag2 = userCharaData == null;
			}
			if (flag2)
			{
				this.isDispWarning = true;
			}
			this.windowBuyConfirmCurrent.tryingToBuyNotComeCharaClothes = flag2;
			string text = ((itemData.staticData == null) ? "" : itemData.staticData.GetName());
			if (ItemDef.Kind.CHARA == itemData.staticData.GetKind())
			{
				CharaStaticData charaStaticData = DataManager.DmChara.GetCharaStaticData(DataManager.DmItem.ItemId2ChraId(itemData.id));
				text = charaStaticData.baseData.NickName + charaStaticData.baseData.charaName;
				if (itemOne.charaStatusId > 0)
				{
					PguiOpenWindowCtrl hdlOpenWindowBasic = CanvasManager.HdlOpenWindowBasic;
					string text2 = "確認";
					string text3 = "<color=red>強化状態で交換されるフレンズが選択されています\n※しょうたい後のステータスは「i」ボタンから確認可能です</color>";
					List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>> list = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>();
					list.Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, "閉じる"));
					hdlOpenWindowBasic.Setup(text2, text3, list, true, (int idx) => true, null, false);
					CanvasManager.HdlOpenWindowBasic.Open();
				}
			}
			this.windowBuyConfirmCurrent.Txt_BuyItemName.text = text;
			this.windowBuyConfirmCurrent.Txt_BuyItemInfo.text = ((ItemDef.Kind.PHOTO == itemData.staticData.GetKind() || ItemDef.Kind.CHARA == itemData.staticData.GetKind()) ? "" : itemData.staticData.GetInfo());
			if (ItemDef.Kind.CHARA == itemData.staticData.GetKind() && itemOne.charaStatusId > 0)
			{
				this.windowBuyConfirmCurrent.Txt_BuyItemInfo.text = "既にしょうたい済みの場合、\n強化状態やステータスは高いほうで上書きされます";
				this.windowBuyConfirmCurrent.Txt_BuyItemInfo.m_Text.color = Color.red;
			}
			else
			{
				this.windowBuyConfirmCurrent.Txt_BuyItemInfo.m_Text.color = new Color32(180, 99, 38, byte.MaxValue);
			}
			this.windowBuyConfirmCurrent.Txt_BuyItemCount.text = "×" + itemData.num.ToString();
			this.windowBuyConfirmCurrent.Txt_Price.text = this.windowBuyConfirmCurrent.itemData.priceItemNum.ToString();
			this.windowBuyConfirmCurrent.buyComment = ((ItemDef.Kind.CHARA == itemData.staticData.GetKind()) ? "回" : "個") + "交換しました" + ((ItemDef.Kind.CHARA == itemData.staticData.GetKind()) ? "\nプレゼントをご確認ください" : "");
			this.windowBuyConfirmCurrent.reqComment = "";
			this.windowBuyConfirmCurrent.title = "交換確認";
			this.windowBuyConfirmCurrent.Parts_ItemUseInfo.SetActive(itemData.staticData.GetKind() != ItemDef.Kind.PRESET);
			if (this.windowBuyConfirmCurrent.replaceBeforeItemData != null)
			{
				if (!flag)
				{
					this.windowBuyConfirmNormal.BuyObjectDouble.SetActive(true);
					this.windowBuyConfirmKiraKira.BuyObjectDouble.SetActive(true);
				}
				this.windowBuyConfirmCurrent.BuyObject.SetActive(false);
				this.windowBuyConfirmCurrent.IconItemDouble[1].Setup(DataManager.DmItem.GetItemStaticBase(itemData.id));
				this.windowBuyConfirmCurrent.IconItemNumDouble[1].text = "×" + itemData.num.ToString();
				this.windowBuyConfirmCurrent.IconItemDouble[0].Setup(DataManager.DmItem.GetItemStaticBase(this.windowBuyConfirmCurrent.replaceBeforeItemData.itemId));
				this.windowBuyConfirmCurrent.IconItemNumDouble[0].text = "×" + this.windowBuyConfirmCurrent.replaceBeforeItemData.itemNum.ToString();
				string text4 = "※既に持っている{0}のため、下記アイテムに変換されます※";
				string text5 = string.Empty;
				ItemDef.Kind kind = ItemDef.Id2Kind(this.windowBuyConfirmCurrent.replaceBeforeItemData.itemId);
				if (kind != ItemDef.Kind.CLOTHES)
				{
					if (kind == ItemDef.Kind.ACHIEVEMENT)
					{
						text5 = string.Format(text4, "称号");
					}
				}
				else
				{
					text5 = string.Format(text4, "衣装");
				}
				this.windowBuyConfirmCurrent.Txt_Warning.text = text5;
				ItemStaticBase itemStaticBase = DataManager.DmItem.GetItemStaticBase(this.windowBuyConfirmCurrent.replaceBeforeItemData.itemId);
				this.windowBuyConfirmCurrent.reqComment = "※「" + itemStaticBase.GetName() + "」が変換されました\n";
			}
			else
			{
				if (!flag)
				{
					this.windowBuyConfirmNormal.BuyObjectDouble.SetActive(false);
					this.windowBuyConfirmKiraKira.BuyObjectDouble.SetActive(false);
				}
				this.windowBuyConfirmCurrent.BuyObject.SetActive(true);
				this.windowBuyConfirmCurrent.Txt_Price.text = this.windowBuyConfirmCurrent.itemData.priceItemNum.ToString();
				ItemStaticBase itemStaticBase2 = DataManager.DmItem.GetItemStaticBase(itemData.id);
				this.windowBuyConfirmCurrent.IconItem.Setup(itemStaticBase2, new IconItemCtrl.SetupParam
				{
					useMaxDetail = true
				});
				if (itemStaticBase2.GetKind() == ItemDef.Kind.CHARA)
				{
					this.windowBuyConfirmCurrent.IconItem.transform.localPosition = new Vector3(0f, 7f, 0f);
				}
				else
				{
					this.windowBuyConfirmCurrent.IconItem.transform.localPosition = new Vector3(0f, 0f, 0f);
				}
				if (ItemDef.Kind.CHARA == itemData.staticData.GetKind() && itemOne.charaStatusId > 0)
				{
					this.windowBuyConfirmCurrent.InfoBtn.gameObject.SetActive(true);
					IconCharaCtrl iconCharaCtrl = this.windowBuyConfirmCurrent.IconItem.GetIconCharaCtrl();
					if (iconCharaCtrl != null)
					{
						this.windowBuyConfirmCurrent.InfoBtn.AddOnClickListener(delegate(PguiButtonCtrl x)
						{
							iconCharaCtrl.OnLongPress();
						}, PguiButtonCtrl.SoundType.DEFAULT);
						CharaPackData charaPackData = CharaPackData.MakeShopCharaData(itemOne.itemId, itemOne.charaStatusId);
						iconCharaCtrl.Setup(charaPackData, SortFilterDefine.SortType.LEVEL, false, new CharaWindowCtrl.DetailParamSetting(CharaWindowCtrl.DetailParamSetting.Preset.SHOP_DETAIL, null), 0, -1, 0);
						iconCharaCtrl.DispPhotoPocketLevel(true);
					}
				}
				else
				{
					this.windowBuyConfirmCurrent.InfoBtn.gameObject.SetActive(false);
				}
			}
			if (ItemDef.Kind.PHOTO == itemData.staticData.GetKind() && !itemBuyCheckResult.isBuy && SelShopCtrl.ItemBuyCheckResult.Reason.STACK_MAX == itemBuyCheckResult.ngReason)
			{
				if (!flag)
				{
					this.windowBuyConfirmNormal.Base_NotPhotoBuyInfo.SetActive(true);
					this.windowBuyConfirmKiraKira.Base_NotPhotoBuyInfo.SetActive(true);
				}
				this.windowBuyConfirmCurrent.Parts_Exchange.SetActive(false);
				this.windowBuyConfirmCurrent.notBuyPhoto = true;
			}
			else
			{
				if (!flag)
				{
					this.windowBuyConfirmNormal.Base_NotPhotoBuyInfo.SetActive(false);
					this.windowBuyConfirmKiraKira.Base_NotPhotoBuyInfo.SetActive(false);
				}
				this.windowBuyConfirmCurrent.Parts_Exchange.SetActive(true);
				this.windowBuyConfirmCurrent.notBuyPhoto = false;
			}
			if (ItemDef.Kind.PRESET == ItemDef.Id2Kind(itemOne.itemId))
			{
				List<ItemPresetData.Item> setItemList = (DataManager.DmItem.GetItemStaticBase(itemOne.itemId) as ItemPresetData).SetItemList;
				bool flag3 = false;
				foreach (ItemPresetData.Item item2 in setItemList)
				{
					if (DataManagerItem.isOverUserHaveMaxNum(item2.itemId, item2.num))
					{
						flag3 = true;
					}
				}
				if (flag3)
				{
					SelShopCtrl.WindowBuyConfirm windowBuyConfirm = this.windowBuyConfirmCurrent;
					windowBuyConfirm.reqComment += "\n※所持数上限を超えたアイテムはプレゼントボックスに移動しました";
				}
			}
			if (DataManagerItem.IsExpectedItemStock(itemData.id, (long)itemData.num))
			{
				if (this.windowBuyConfirmCurrent.reqComment.Length > 0)
				{
					SelShopCtrl.WindowBuyConfirm windowBuyConfirm2 = this.windowBuyConfirmCurrent;
					windowBuyConfirm2.reqComment += "\n";
				}
				int num2 = ((itemData.id == 30101) ? 30090 : 0);
				SelShopCtrl.WindowBuyConfirm windowBuyConfirm3 = this.windowBuyConfirmCurrent;
				windowBuyConfirm3.reqComment = string.Concat(new string[]
				{
					windowBuyConfirm3.reqComment,
					"※所持数上限を超えた",
					DataManager.DmItem.GetItemStaticBase(itemData.id).GetName(),
					"は",
					DataManager.DmItem.GetItemStaticBase(num2).GetName(),
					"に補充されました"
				});
			}
			this.windowBuyConfirmCurrent.owCtrl.choiceR.GetComponent<PguiButtonCtrl>().SetActEnable(itemBuyCheckResult.isBuy, false, false);
			if (!itemBuyCheckResult.isBuy)
			{
				switch (itemBuyCheckResult.ngReason)
				{
				case SelShopCtrl.ItemBuyCheckResult.Reason.SHOP_SOLDOUT:
					this.windowBuyConfirmCurrent.Txt_owErrorText.text = PrjUtil.MakeMessage((itemData.staticData.GetKind() == ItemDef.Kind.CHARA) ? "これ以上しょうたい\nできません" : "このアイテムは\n売り切れです");
					break;
				case SelShopCtrl.ItemBuyCheckResult.Reason.SHOP_MONEY_LACK:
					this.windowBuyConfirmCurrent.Txt_owErrorText.text = PrjUtil.MakeMessage("アイテムが足りません");
					break;
				case SelShopCtrl.ItemBuyCheckResult.Reason.STACK_MAX:
					this.windowBuyConfirmCurrent.Txt_owErrorText.text = PrjUtil.MakeMessage("これ以上\n所持できません");
					break;
				default:
					this.windowBuyConfirmCurrent.Txt_owErrorText.text = PrjUtil.MakeMessage("");
					break;
				}
			}
			this.windowBuyConfirmCurrent.SliderBar.maxValue = (float)this.windowBuyConfirmCurrent.buyMax;
			this.windowBuyConfirmCurrent.SliderBar.value = this.windowBuyConfirmCurrent.SliderBar.minValue;
			this.windowBuyConfirmCurrent.SliderBar.interactable = itemBuyCheckResult.isBuy && this.windowBuyConfirmCurrent.buyMax > 1;
			this.BuyItemUpdate(itemBuyCheckResult.isBuy);
			return;
		}
		this.windowBuyConfirmCurrent = this.windowBuyConfirmBulk;
		if (itemOne.isSoldout || itemOne.maxChangeNum == 0)
		{
			return;
		}
		this.SelectGoods(itemOne);
	}

	private void OnClickExchangeButton(PguiButtonCtrl button)
	{
		if (!this.isBulkExChangeMode)
		{
			return;
		}
		this.windowBuyConfirmCurrent = this.windowBuyConfirmBulk;
		this.requestStatus = SelShopCtrl.State.SHOP_BUY_CHECK;
		SelShopCtrl.ItemBuyCheckResult itemBuyCheckResult = null;
		foreach (ShopData.ItemOne itemOne in this.itemDataList)
		{
			this.BuyitemData.Add(new ItemData(itemOne.itemId, itemOne.itemNum * (itemOne.maxChangeNum - itemOne.nowChangeNum)));
		}
		int num = this.itemDataList.Count / 5 + ((this.itemDataList.Count % 5 != 0) ? 1 : 0);
		this.windowBuyConfirmBulk.ItemScroll.ReuseItemNum = 5;
		foreach (SelShopCtrl.GridGUI gridGUI in this.gridGUIMap.Values)
		{
			foreach (IconItemCtrl iconItemCtrl in gridGUI.iconItemList)
			{
				iconItemCtrl.gameObject.SetActive(false);
			}
		}
		if (this.gridGUIMap.Count == 0)
		{
			this.windowBuyConfirmBulk.ItemScroll.Setup(num, 0);
		}
		else
		{
			this.windowBuyConfirmBulk.ItemScroll.Resize(num, 0);
		}
		this.windowBuyConfirmCurrent.tryingToBuyNotComeCharaClothes = false;
		using (List<int>.Enumerator enumerator4 = this.selectGoodsIds.GetEnumerator())
		{
			while (enumerator4.MoveNext())
			{
				int goodsId = enumerator4.Current;
				ShopData.ItemOne itemOne2 = this.shopDataList[this.currentTabIndex][this.currentShopIndex].oneDataList.Find((ShopData.ItemOne item) => item.goodsId == goodsId);
				if (itemOne2 == null)
				{
					return;
				}
				this.windowBuyConfirmCurrent.itemData = itemOne2;
				SelShopCtrl.ItemBuyCheckResult itemBuyCheckResult2 = this.ItemBuyCheck(itemOne2);
				if (itemBuyCheckResult == null || itemBuyCheckResult.isBuy)
				{
					itemBuyCheckResult = itemBuyCheckResult2;
				}
				this.windowBuyConfirmCurrent.replaceBeforeItemData = null;
				ItemData itemData = new ItemData(this.windowBuyConfirmCurrent.itemData.itemId, this.windowBuyConfirmCurrent.itemData.itemNum);
				int num2 = this.CalcBuyItemMax();
				this.windowBuyConfirmCurrent.isBuy = itemBuyCheckResult2.isBuy;
				this.windowBuyConfirmCurrent.buyMax = num2;
				this.windowBuyConfirmCurrent.info = itemData.staticData.GetInfo();
				bool flag = false;
				if (ItemDef.Kind.CLOTHES == itemData.staticData.GetKind())
				{
					CharaClothStatic charaClothesStaticData = DataManager.DmChara.GetCharaClothesStaticData(itemData.staticData.GetId());
					CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(charaClothesStaticData.CharaId);
					flag = userCharaData == null;
				}
				if (flag)
				{
					this.isDispWarning = true;
				}
				if (!this.windowBuyConfirmCurrent.tryingToBuyNotComeCharaClothes)
				{
					this.windowBuyConfirmCurrent.tryingToBuyNotComeCharaClothes = flag;
				}
				string text = ((itemData.staticData == null) ? "" : itemData.staticData.GetName());
				this.windowBuyConfirmCurrent.Txt_BuyItemName.text = text;
				this.windowBuyConfirmCurrent.Txt_BuyItemInfo.text = ((ItemDef.Kind.PHOTO == itemData.staticData.GetKind() || ItemDef.Kind.CHARA == itemData.staticData.GetKind()) ? "" : itemData.staticData.GetInfo());
				this.windowBuyConfirmCurrent.Txt_BuyItemInfo.m_Text.color = new Color32(180, 99, 38, byte.MaxValue);
				this.windowBuyConfirmCurrent.Txt_BuyItemCount.text = "×" + (itemData.num * itemOne2.itemNum).ToString();
				this.windowBuyConfirmCurrent.title = "交換確認";
				this.windowBuyConfirmCurrent.Parts_ItemUseInfo.SetActive(false);
				if (ItemDef.Kind.PHOTO == itemData.staticData.GetKind() && !itemBuyCheckResult2.isBuy && SelShopCtrl.ItemBuyCheckResult.Reason.STACK_MAX == itemBuyCheckResult2.ngReason)
				{
					this.windowBuyConfirmCurrent.Parts_Exchange.SetActive(false);
					this.windowBuyConfirmCurrent.notBuyPhoto = true;
				}
				else
				{
					this.windowBuyConfirmCurrent.Parts_Exchange.SetActive(true);
					this.windowBuyConfirmCurrent.notBuyPhoto = false;
				}
				if (ItemDef.Kind.PRESET == ItemDef.Id2Kind(itemOne2.itemId))
				{
					List<ItemPresetData.Item> setItemList = (DataManager.DmItem.GetItemStaticBase(itemOne2.itemId) as ItemPresetData).SetItemList;
					bool flag2 = false;
					foreach (ItemPresetData.Item item2 in setItemList)
					{
						if (DataManagerItem.isOverUserHaveMaxNum(item2.itemId, item2.num))
						{
							flag2 = true;
						}
					}
					if (flag2)
					{
						SelShopCtrl.WindowBuyConfirm windowBuyConfirm = this.windowBuyConfirmCurrent;
						windowBuyConfirm.reqComment += "\n※所持数上限を超えたアイテムはプレゼントボックスに移動しました";
					}
				}
				if (DataManagerItem.IsExpectedItemStock(itemData.id, (long)itemData.num))
				{
					if (this.windowBuyConfirmCurrent.reqComment.Length > 0)
					{
						SelShopCtrl.WindowBuyConfirm windowBuyConfirm2 = this.windowBuyConfirmCurrent;
						windowBuyConfirm2.reqComment += "\n";
					}
					int num3 = ((itemData.id == 30101) ? 30090 : 0);
					SelShopCtrl.WindowBuyConfirm windowBuyConfirm3 = this.windowBuyConfirmCurrent;
					windowBuyConfirm3.reqComment = string.Concat(new string[]
					{
						windowBuyConfirm3.reqComment,
						"※所持数上限を超えた",
						DataManager.DmItem.GetItemStaticBase(itemData.id).GetName(),
						"は",
						DataManager.DmItem.GetItemStaticBase(num3).GetName(),
						"に補充されました"
					});
				}
			}
		}
		this.windowBuyConfirmCurrent.owCtrl.choiceR.GetComponent<PguiButtonCtrl>().SetActEnable(itemBuyCheckResult.isBuy, false, false);
		if (!itemBuyCheckResult.isBuy)
		{
			switch (itemBuyCheckResult.ngReason)
			{
			case SelShopCtrl.ItemBuyCheckResult.Reason.SHOP_SOLDOUT:
				this.windowBuyConfirmCurrent.Txt_owErrorText.text = PrjUtil.MakeMessage("このアイテムは\n売り切れです");
				break;
			case SelShopCtrl.ItemBuyCheckResult.Reason.SHOP_MONEY_LACK:
				this.windowBuyConfirmCurrent.Txt_owErrorText.text = PrjUtil.MakeMessage("アイテムが足りません");
				break;
			case SelShopCtrl.ItemBuyCheckResult.Reason.STACK_MAX:
				this.windowBuyConfirmCurrent.Txt_owErrorText.text = PrjUtil.MakeMessage("これ以上\n所持できません");
				break;
			default:
				this.windowBuyConfirmCurrent.Txt_owErrorText.text = PrjUtil.MakeMessage("");
				break;
			}
		}
		this.windowBuyConfirmCurrent.Txt_Price.text = this.selectItemMoney.ToString();
		int userHaveNum = DataManagerItem.GetUserHaveNum(this.windowBuyConfirmCurrent.itemData.priceItemId);
		this.windowBuyConfirmCurrent.Txt_BuyItemType.text = "所持数";
		this.windowBuyConfirmCurrent.Txt_BuyBeforeMoney.text = userHaveNum.ToString();
		this.windowBuyConfirmCurrent.Txt_BuyAfterMoney.text = (userHaveNum - this.selectItemMoney).ToString();
		this.SetMoneyImage(this.windowBuyConfirmCurrent.NeedInfoImage, 40f);
		this.SetMoneyImage(this.windowBuyConfirmCurrent.UseInfoImage, 40f);
		this.SetMoneyImage(this.windowBuyConfirmCurrent.UseMoneyImage, 40f);
	}

	private void SetMoneyImage(PguiRawImageCtrl rawImage, float size = 40f)
	{
		if (!rawImage)
		{
			return;
		}
		int priceItemId = this.shopDataList[this.currentTabIndex][this.currentShopIndex].priceItemId;
		if (priceItemId == 0)
		{
			rawImage.gameObject.SetActive(false);
			return;
		}
		string iconName = DataManager.DmItem.GetItemStaticBase(priceItemId).GetIconName();
		rawImage.SetRawImage(iconName, true, false, null);
		rawImage.GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
	}

	private void BuyItemUpdate(bool isBuy = true)
	{
		bool flag = this.windowBuyConfirmCurrent.buyCount > 1;
		this.windowBuyConfirmCurrent.Btn_Minus.SetActEnable(flag && isBuy, false, false);
		if (this.windowBuyConfirmCurrent.itemData == null)
		{
			return;
		}
		bool flag2 = this.windowBuyConfirmCurrent.buyCount < this.windowBuyConfirmCurrent.buyMax;
		this.windowBuyConfirmCurrent.Btn_Plus.SetActEnable(flag2 && isBuy, false, false);
		int userHaveNum = DataManagerItem.GetUserHaveNum(this.windowBuyConfirmCurrent.itemData.priceItemId);
		this.windowBuyConfirmCurrent.Txt_BuyItemType.text = "所持数";
		if (!this.isBulkExChangeMode)
		{
			this.windowBuyConfirmCurrent.Txt_BuyBeforeMoney.text = userHaveNum.ToString();
			this.windowBuyConfirmCurrent.Txt_BuyAfterMoney.text = (userHaveNum - this.windowBuyConfirmCurrent.itemData.priceItemNum * this.windowBuyConfirmCurrent.buyCount).ToString();
		}
		this.windowBuyConfirmCurrent.Txt_BuyCount.text = this.windowBuyConfirmCurrent.buyCount.ToString();
		if (this.windowBuyConfirmCurrent.buyMax <= 0)
		{
			this.windowBuyConfirmCurrent.Txt_BuyCount.text = 0.ToString();
		}
		this.windowBuyConfirmCurrent.Txt_BuyBeforeCount.text = DataManagerItem.GetUserHaveNum(this.windowBuyConfirmCurrent.itemData.itemId).ToString();
		this.windowBuyConfirmCurrent.Txt_BuyAfterCount.text = ((long)DataManagerItem.GetUserHaveNum(this.windowBuyConfirmCurrent.itemData.itemId) + (long)this.windowBuyConfirmCurrent.itemData.itemNum * (long)this.windowBuyConfirmCurrent.buyCount).ToString();
		this.SetMoneyImage(this.windowBuyConfirmCurrent.NeedInfoImage, 40f);
		this.SetMoneyImage(this.windowBuyConfirmCurrent.UseInfoImage, 40f);
		this.SetMoneyImage(this.windowBuyConfirmCurrent.UseMoneyImage, 40f);
	}

	private void OnClickItemInfo(PguiButtonCtrl button)
	{
		ShopData.ItemOne itemOne = this.shopDataList[this.currentTabIndex][this.currentShopIndex].oneDataList.Find((ShopData.ItemOne item) => item.goodsId == button.transform.parent.GetComponent<PguiDataHolder>().id);
		if (itemOne == null)
		{
			return;
		}
		ItemData itemData = new ItemData(itemOne.itemId, itemOne.itemNum);
		CanvasManager.HdlItemPresetWindowCtrl.OpenByItem(itemData);
	}

	private void OnClickItemLock(PguiButtonCtrl button)
	{
		ShopData.ItemOne shopItemOne = this.shopDataList[this.currentTabIndex][this.currentShopIndex].oneDataList.Find((ShopData.ItemOne item) => item.goodsId == button.transform.parent.GetComponent<PguiDataHolder>().id);
		if (shopItemOne == null)
		{
			return;
		}
		List<CmnReleaseConditionWindowCtrl.SetupParam> list = new List<CmnReleaseConditionWindowCtrl.SetupParam>();
		if (shopItemOne.isLockByTime)
		{
			list.Add(new CmnReleaseConditionWindowCtrl.SetupParam
			{
				text = shopItemOne.startTime.ToString("yyyy/M/d HH:mm") + " 以降"
			});
		}
		if (shopItemOne.isLockByQuest)
		{
			list.Add(new CmnReleaseConditionWindowCtrl.SetupParam
			{
				text = shopItemOne.openQuestOneIdText
			});
		}
		if (shopItemOne.isLockByMission)
		{
			DataManagerMission.StaticMissionData staticMissionData = DataManager.DmMission.StaticMissionDataList.Find((DataManagerMission.StaticMissionData item) => item.MissionId == shopItemOne.openMissionId);
			list.Add(new CmnReleaseConditionWindowCtrl.SetupParam
			{
				text = ((staticMissionData != null) ? staticMissionData.MissionContents : "")
			});
		}
		CanvasManager.HdlCmnReleaseConditionWindowCtrl.Open(PrjUtil.MakeMessage("解放条件"), list);
	}

	private int CalcBuyItemMax()
	{
		if (this.windowBuyConfirmCurrent.itemData == null)
		{
			return 0;
		}
		ShopData.ItemOne itemData = this.windowBuyConfirmCurrent.itemData;
		DataManager.DmItem.GetUserItemData(this.windowBuyConfirmCurrent.itemData.itemId);
		int num = 9999999;
		int num2 = ((this.windowBuyConfirmCurrent.replaceBeforeItemData != null) ? this.windowBuyConfirmCurrent.replaceBeforeItemData.itemId : 0);
		long num3 = (DataManagerItem.GetUserHaveMaxNum(this.windowBuyConfirmCurrent.itemData.itemId, num2) - (long)DataManagerItem.GetUserHaveNum(this.windowBuyConfirmCurrent.itemData.itemId)) / (long)itemData.itemNum;
		int num4 = ((0 < itemData.maxChangeNum) ? (itemData.maxChangeNum - itemData.nowChangeNum) : 9999999);
		int num5 = DataManagerItem.GetUserHaveNum(itemData.priceItemId) / itemData.priceItemNum;
		int num6 = Math.Min(num4, num5);
		num6 = (int)Math.Min(num3, (long)num6);
		num6 = Math.Min(num, num6);
		num6 = Math.Max(0, num6);
		return Math.Min(int.MaxValue / itemData.itemNum, num6);
	}

	private SelShopCtrl.ItemBuyCheckResult ItemBuyCheck(ShopData.ItemOne item)
	{
		SelShopCtrl.ItemBuyCheckResult itemBuyCheckResult = new SelShopCtrl.ItemBuyCheckResult();
		if (item == null)
		{
			return itemBuyCheckResult;
		}
		if (item.isLockByTime)
		{
			itemBuyCheckResult.isBuy = false;
			itemBuyCheckResult.ngReason = SelShopCtrl.ItemBuyCheckResult.Reason.BEFORE_START_TIME;
			return itemBuyCheckResult;
		}
		if (item.isLockByQuest)
		{
			itemBuyCheckResult.isBuy = false;
			itemBuyCheckResult.ngReason = SelShopCtrl.ItemBuyCheckResult.Reason.NO_CLAER_QUEST;
			return itemBuyCheckResult;
		}
		if (item.isLockByMission)
		{
			itemBuyCheckResult.isBuy = false;
			itemBuyCheckResult.ngReason = SelShopCtrl.ItemBuyCheckResult.Reason.NO_CLAER_MISSION;
			return itemBuyCheckResult;
		}
		if (item.isSoldout)
		{
			itemBuyCheckResult.isBuy = false;
			itemBuyCheckResult.ngReason = SelShopCtrl.ItemBuyCheckResult.Reason.SHOP_SOLDOUT;
			return itemBuyCheckResult;
		}
		if (DataManagerItem.GetUserHaveNum(item.priceItemId) < item.priceItemNum)
		{
			itemBuyCheckResult.isBuy = false;
			itemBuyCheckResult.ngReason = SelShopCtrl.ItemBuyCheckResult.Reason.SHOP_MONEY_LACK;
			return itemBuyCheckResult;
		}
		if (DataManagerItem.isOverUserHaveMaxNum(item.itemId, item.itemNum))
		{
			itemBuyCheckResult.isBuy = false;
			itemBuyCheckResult.ngReason = SelShopCtrl.ItemBuyCheckResult.Reason.STACK_MAX;
			return itemBuyCheckResult;
		}
		itemBuyCheckResult.isBuy = true;
		return itemBuyCheckResult;
	}

	private int GetShopCount()
	{
		return this.shopDataList[this.currentTabIndex].FindAll((ShopData x) => x.category != ShopData.Category.PURCHASE && x.category != ShopData.Category.MONTHLYPACK).Count;
	}

	public bool OnClickReturnButton()
	{
		if ((this.openShop != null && this.openShop.shopId != 0 && !this.openShop.disableSceneBack) || this.currentStatus != SelShopCtrl.State.SHOP)
		{
			return false;
		}
		int num = 0;
		this.requestStatus = SelShopCtrl.State.TOP;
		this.guiData.ScrollBarList[num].gameObject.SetActive(this.shopDataList[num].Count >= 5);
		this.guiData.back.SetActive(true);
		this.guiData.TabGroup.gameObject.SetActive(true);
		foreach (PguiScrollbar pguiScrollbar in this.guiData.ScrollBarList)
		{
			pguiScrollbar.gameObject.SetActive(true);
		}
		foreach (ScrollRect scrollRect in this.guiData.ScrollViewList)
		{
			scrollRect.gameObject.SetActive(true);
		}
		this.guiData.BtnNext_Left.gameObject.SetActive(false);
		this.guiData.BtnNext_Right.gameObject.SetActive(false);
		this.guiShopWindowList[this.currentShopWindowFlip].BaseObj.SetActive(false);
		this.guiData.ScrollViewList[num].gameObject.SetActive(true);
		CanvasManager.SetBgTexture("selbg_shop");
		this.OnClickResetSearch(null);
		return true;
	}

	public void ReleaseShopData()
	{
		foreach (List<SelShopCtrl.ShopBtn> list in this.shopBtnDataList)
		{
			foreach (SelShopCtrl.ShopBtn shopBtn in list)
			{
				Object.Destroy(shopBtn.baseObj);
			}
		}
		for (int i = 0; i < 4; i++)
		{
			this.shopBtnDataList[i].Clear();
			this.shopDataList[i].Clear();
		}
		int num = 0;
		foreach (object obj in this.guiData.ScrollContentList[num])
		{
			Object.Destroy(((Transform)obj).gameObject);
		}
		this.guiData.ScrollContentList[num].localPosition = new Vector3(this.guiData.ScrollContentList[num].localPosition.x, 0f, this.guiData.ScrollContentList[num].localPosition.z);
		this.requestStatus = SelShopCtrl.State.INVALID;
		if (this.renderTextureChara != null)
		{
			Object.Destroy(this.renderTextureChara.gameObject);
		}
		this.renderTextureChara = null;
	}

	private void ConvertBGImage(int tabIdx, int shopIdx)
	{
		if ((this.shopDataList[tabIdx][shopIdx].category == ShopData.Category.EVENT || this.shopDataList[tabIdx][shopIdx].category == ShopData.Category.EVENT_NOITEM_HIDE) && (this.currentStatus == SelShopCtrl.State.SHOP || this.currentStatus == SelShopCtrl.State.SHOP_BUY_CHECK || this.currentStatus == SelShopCtrl.State.SHOP_BUY_END))
		{
			CanvasManager.SetBgTexture("selbg_paper");
			return;
		}
		CanvasManager.SetBgTexture("selbg_shop");
	}

	private void OnClickResetSearch(PguiButtonCtrl clickBtn = null)
	{
		this.SearchText = "";
		this.windowTextSearchChange.FirstInputField.text = "";
		this.windowTextSearchChange.SecondInputField.text = "";
		if (this.allItemDataList == null || this.shopDataList[this.currentTabIndex][this.currentShopIndex] == null || this.allItemDataList == this.shopDataList[this.currentTabIndex][this.currentShopIndex].oneDataList)
		{
			return;
		}
		this.shopDataList[this.currentTabIndex][this.currentShopIndex].oneDataList = this.allItemDataList;
		this.UpdateItemList();
	}

	private void SetFilteredItemList()
	{
		List<ShopData.ItemOne> list = new List<ShopData.ItemOne>();
		if (this.allItemDataList == null)
		{
			list = this.shopDataList[this.currentTabIndex][this.currentShopIndex].oneDataList;
			this.allItemDataList = list;
		}
		else
		{
			list = this.allItemDataList;
		}
		list = list.Where<ShopData.ItemOne>(delegate(ShopData.ItemOne shopItem)
		{
			if (shopItem.itemName == "")
			{
				ItemData itemData = new ItemData(shopItem.itemId, shopItem.itemNum);
				string text = ((itemData.staticData == null) ? "" : itemData.staticData.GetName());
				shopItem.SetShopItemName(text);
				return text.Contains(this.SearchText);
			}
			return shopItem.itemName.Contains(this.SearchText);
		}).ToList<ShopData.ItemOne>();
		this.shopDataList[this.currentTabIndex][this.currentShopIndex].oneDataList = list;
		this.UpdateItemList();
	}

	private void UpdateItemList()
	{
		SelShopCtrl.GUIShopWindow guishopWindow = this.guiShopWindowList[this.currentShopWindowFlip];
		int count = this.shopDataList[this.currentTabIndex][this.currentShopIndex].oneDataList.Count;
		guishopWindow.TxtNoneObj.gameObject.SetActive(this.allItemDataList == null || this.allItemDataList.Count <= 0);
		guishopWindow.TxtNoneFilteredObj.gameObject.SetActive(count <= 0);
		if (!this.isBulkExChangeMode)
		{
			guishopWindow.ExchangeButton.gameObject.SetActive(false);
		}
		else
		{
			guishopWindow.ExchangeButton.gameObject.SetActive(true);
			guishopWindow.ExchangeButton.SetActEnable(this.selectGoodsIds.Count > 0, false, false);
		}
		guishopWindow.BaseObj.transform.Find("InBase/ScrollView").GetComponent<ReuseScroll>().Resize(count / SelShopCtrl.Shop_ListSet_Btn.SCROLL_ITEM_NUN_H + ((count % SelShopCtrl.Shop_ListSet_Btn.SCROLL_ITEM_NUN_H == 0) ? 0 : 1), 0);
	}

	private void SetBuyItemMode(bool isBulk)
	{
		this.isBulkExChangeMode = isBulk;
		this.guiShopWindowList[this.currentShopWindowFlip].SwitchModeCheck.SetActive(this.isBulkExChangeMode);
		this.selectGoodsIds.Clear();
		this.itemDataList.Clear();
		this.selectItemMoney = 0;
	}

	private void ShowItemResultWindow(List<ItemData> dispItemList)
	{
		int priceItemId = this.shopDataList[this.currentTabIndex][this.currentShopIndex].priceItemId;
		string iconName = DataManager.DmItem.GetItemStaticBase(priceItemId).GetIconName();
		GetMultiItemWindowCtrl.SetupParam setupParam = new GetMultiItemWindowCtrl.SetupParam
		{
			titleText = "確認",
			messageText = "一括交換しました",
			innerTitleText = "一括交換したアイテム",
			buyBeforeMoney = this.windowBuyConfirmCurrent.Txt_BuyBeforeMoney.text,
			buyAfterMoney = this.windowBuyConfirmCurrent.Txt_BuyAfterMoney.text,
			iconName = iconName,
			callBack = delegate(int x)
			{
				this.BuyitemData.Clear();
				this.OnClickOwButton(x);
				return true;
			}
		};
		CanvasManager.HdlGetItemSetWindowCtrl.Setup(dispItemList, setupParam, true, 0);
		CanvasManager.HdlGetItemSetWindowCtrl.Open();
	}

	public void RequestSetupShopAssistant()
	{
		this.currentCharaId = DataManager.DmAssistant.UserData.shopAssistantCharaId;
		this.selAssistantCtrl.SetupAssistant();
		this.renderTextureChara = this.selAssistantCtrl.renderTextureChara;
	}

	private float SPACE_SIZE = 10f;

	private const int BUY_COUNT_MAX = 9999999;

	public SelAssistantCtrl selAssistantCtrl;

	public SelShopCtrl.GUI guiData;

	private List<SelShopCtrl.GUIShopWindow> guiShopWindowList;

	private GameObject mainObj;

	public RenderTextureChara renderTextureChara;

	private int currentShopIndex;

	private int currentShopWindowFlip;

	private List<List<ShopData>> shopDataList;

	private List<List<SelShopCtrl.ShopBtn>> shopBtnDataList;

	private SelShopCtrl.WindowBuyConfirmNormal windowBuyConfirmNormal;

	private SelShopCtrl.WindowBuyConfirmKiraKira windowBuyConfirmKiraKira;

	private SelShopCtrl.WindowBuyConfirmGrow windowBuyConfirmGrow;

	private SelShopCtrl.WindowBuyConfirmGrowKiraKira windowBuyConfirmGrowKiraKira;

	private SelShopCtrl.WindowBuyConfirmBulk windowBuyConfirmBulk;

	private SelShopCtrl.WindowBuyConfirm windowBuyConfirmCurrent;

	private List<CharaGrowItemInfo.NeedInfo> charaGrowNeedInfoList;

	private SelShopCtrl.WindowBuyEnd windowBuyEnd;

	private SceneShopArgs openShop;

	private int currentCharaId;

	private IEnumerator windowChangeAnime;

	private bool isBulkExChangeMode;

	private bool isDispWarning;

	public List<int> selectGoodsIds = new List<int>();

	public List<ShopData.ItemOne> itemDataList = new List<ShopData.ItemOne>();

	private int selectItemMoney;

	private List<ItemData> BuyitemData = new List<ItemData>();

	private Dictionary<int, SelShopCtrl.GridGUI> gridGUIMap = new Dictionary<int, SelShopCtrl.GridGUI>();

	private int columnCount;

	private SelShopCtrl.WindowTextSearchChange windowTextSearchChange;

	private List<ShopData.ItemOne> allItemDataList;

	private enum State
	{
		INVALID,
		TOP,
		SHOP,
		SHOP_BUY_CHECK,
		SHOP_BUY_END,
		MAX
	}

	public class Shop_ListSet_Btn
	{
		public Shop_ListSet_Btn(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.CmnShop_BuyItem = baseTr.GetComponent<PguiButtonCtrl>();
			this.Cmn_Btn_Lock = baseTr.Find("Cmn_Btn_Lock").GetComponent<PguiButtonCtrl>();
			this.Info_Btn = baseTr.Find("Info_Btn").GetComponent<PguiButtonCtrl>();
			this.Icon_Item = baseTr.Find("BaseImage/Icon_Item").GetComponent<IconItemCtrl>();
			this.Icon_Item.SetRaycastTargetIconItem(false);
			this.Txt_BuyInfo01 = baseTr.Find("BaseImage/Txt_BuyInfo01").GetComponent<PguiTextCtrl>();
			this.Txt_BuyInfo02 = baseTr.Find("BaseImage/Txt_BuyInfo02").GetComponent<PguiTextCtrl>();
			this.Price_Item_Icon = baseTr.Find("BaseImage/ItemOwnBase/Icon_Img").GetComponent<PguiRawImageCtrl>();
			this.Item_Num_Txt = baseTr.Find("BaseImage/Txt_Window/Num_Txt").GetComponent<PguiTextCtrl>();
			this.Price_Num_Txt = baseTr.Find("BaseImage/ItemOwnBase/Num_Txt").GetComponent<PguiTextCtrl>();
			this.Txt_ItemOwn = baseTr.Find("BaseImage/Txt_ItemOwn").GetComponent<PguiTextCtrl>();
			this.Cmn_Mark_New = baseTr.Find("BaseImage/Cmn_Mark_New").GetComponent<PguiAECtrl>();
			this.Mark_Sold = baseTr.Find("BaseImage/Mark_Sold").gameObject;
			this.Mark_Sold_Txt = baseTr.Find("BaseImage/Mark_Sold/Txt").GetComponent<PguiTextCtrl>();
			this.Select_CheckBox = baseTr.Find("BaseImage/Btn_CheckBox").gameObject;
			this.Select_Mark = baseTr.Find("BaseImage/Btn_CheckBox/BaseImage/Img_Check").gameObject;
		}

		public static readonly int SCROLL_ITEM_NUN_H = 4;

		public GameObject baseObj;

		public PguiButtonCtrl CmnShop_BuyItem;

		public PguiButtonCtrl Cmn_Btn_Lock;

		public PguiButtonCtrl Info_Btn;

		public IconItemCtrl Icon_Item;

		public PguiTextCtrl Txt_BuyInfo01;

		public PguiTextCtrl Txt_BuyInfo02;

		public PguiRawImageCtrl Price_Item_Icon;

		public PguiTextCtrl Item_Num_Txt;

		public PguiTextCtrl Price_Num_Txt;

		public PguiTextCtrl Txt_ItemOwn;

		public PguiAECtrl Cmn_Mark_New;

		public GameObject Mark_Sold;

		public PguiTextCtrl Mark_Sold_Txt;

		public GameObject Select_CheckBox;

		public GameObject Select_Mark;
	}

	public class ShopBtn
	{
		public ShopBtn(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.BtnNone = baseTr.GetComponent<PguiButtonCtrl>();
			this.baseRepSprite = baseTr.Find("BaseImage").GetComponent<PguiReplaceSpriteCtrl>();
			this.ShopBtn_Icon = baseTr.Find("BaseImage/ShopBtn_Icon").GetComponent<PguiRawImageCtrl>();
			Transform transform = baseTr.Find("BaseImage/Tex");
			if (transform != null)
			{
				this.ShopBanner = transform.GetComponent<PguiRawImageCtrl>();
			}
			this.Txt = baseTr.Find("BaseImage/Txt").GetComponent<PguiTextCtrl>();
			transform = baseTr.Find("BaseImage/Mark_Passport_Buy");
			if (transform != null)
			{
				this.Mark = transform.gameObject;
			}
			this.index = -1;
			this.ShopBtn_PriceIcon = baseTr.Find("BaseImage/ItemOwnBase/Icon_Img").GetComponent<PguiRawImageCtrl>();
			this.PriceTxt = baseTr.Find("BaseImage/ItemOwnBase/Num_Txt").GetComponent<PguiTextCtrl>();
			this.Cmn_Mark_New = baseTr.Find("BaseImage/Cmn_Mark_New").gameObject;
		}

		public GameObject baseObj;

		public PguiButtonCtrl BtnNone;

		public PguiRawImageCtrl ShopBanner;

		public PguiRawImageCtrl ShopBtn_Icon;

		public PguiTextCtrl Txt;

		public GameObject Mark;

		public int index;

		public ShopData itemData;

		public PguiRawImageCtrl ShopBtn_PriceIcon;

		public PguiTextCtrl PriceTxt;

		public PguiReplaceSpriteCtrl baseRepSprite;

		public GameObject Cmn_Mark_New;
	}

	public class ShopWindow
	{
		public ShopWindow(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
		}

		public GameObject baseObj;
	}

	public class GUI
	{
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.back = baseTr.Find("Deco_All").gameObject;
			this.BtnNext_Left = baseTr.Find("BtnNext_Left").GetComponent<PguiButtonCtrl>();
			this.BtnNext_Right = baseTr.Find("BtnNext_Right").GetComponent<PguiButtonCtrl>();
			this.BtnAssistantEdit = baseTr.Find("BtnAssistant_Edit").GetComponent<PguiButtonCtrl>();
			this.BtnAssistantEdit.transform.SetSiblingIndex(2);
			this.markLockAssistantEdit = baseTr.Find("BtnAssistant_Edit/Mark_Lock_New").GetComponent<MarkLockCtrl>();
			this.TabGroup = baseTr.Find("ShopListTabCategoryAll/TabGroup").GetComponent<PguiTabGroupCtrl>();
			this.ScrollViewList = new List<ScrollRect>();
			this.ScrollContentList = new List<RectTransform>();
			this.ScrollBarList = new List<PguiScrollbar>();
			this.TabList = new List<GameObject>();
			for (ShopData.TabCategory tabCategory = ShopData.TabCategory.ALL; tabCategory < ShopData.TabCategory.MAX; tabCategory++)
			{
				string text = "ShopList_All";
				int num = 0;
				switch (tabCategory)
				{
				case ShopData.TabCategory.ALL:
					text = "ShopList_All";
					num = 0;
					break;
				case ShopData.TabCategory.EVENT:
					text = "ShopList_Event";
					num = 1;
					break;
				case ShopData.TabCategory.NORMAL:
					text = "ShopList_Normal";
					num = 2;
					break;
				case ShopData.TabCategory.TICKET:
					text = "ShopList_Ticket";
					num = 3;
					break;
				}
				this.ScrollViewList.Add(baseTr.Find("ShopListTabCategoryAll/" + text + "/ScrollView").GetComponent<ScrollRect>());
				this.ScrollViewList[num].scrollSensitivity = ScrollParamDefine.ShopTop;
				this.ScrollContentList.Add(baseTr.Find("ShopListTabCategoryAll/" + text + "/ScrollView/Viewport/Content").GetComponent<RectTransform>());
				this.ScrollBarList.Add(baseTr.Find("ShopListTabCategoryAll/" + text + "/Scrollbar_Vertical").GetComponent<PguiScrollbar>());
				this.TabList.Add(baseTr.Find("ShopListTabCategoryAll/" + text).gameObject);
			}
		}

		public GameObject baseObj;

		public GameObject back;

		public PguiButtonCtrl BtnNext_Left;

		public PguiButtonCtrl BtnNext_Right;

		public PguiButtonCtrl BtnAssistantEdit;

		public MarkLockCtrl markLockAssistantEdit;

		public List<ScrollRect> ScrollViewList;

		public List<RectTransform> ScrollContentList;

		public List<PguiScrollbar> ScrollBarList;

		public List<GameObject> TabList;

		public PguiTabGroupCtrl TabGroup;
	}

	public class GUIShopWindow
	{
		public GUIShopWindow(Transform baseTr)
		{
			this.BaseObj = baseTr.gameObject;
			this.NumOwn = baseTr.Find("ItemOwnBase/Num_Own").GetComponent<PguiTextCtrl>();
			this.ScrollView = baseTr.Find("InBase/ScrollView").GetComponent<ReuseScroll>();
			this.TxtNoneObj = baseTr.Find("InBase/Txt_None").gameObject;
			this.TxtNoneFilteredObj = baseTr.Find("InBase/Txt_NoneFiltered").gameObject;
			this.EndTimeText = baseTr.Find("Num_Txt").GetComponent<PguiTextCtrl>();
			this.SwitchModeButton = baseTr.Find("InBase/Btn_CheckBox").GetComponent<PguiButtonCtrl>();
			this.SwitchModeCheck = baseTr.Find("InBase/Btn_CheckBox/BaseImage/Img_Check").gameObject;
			this.ExchangeButton = baseTr.Find("InBase/Btn_BulkExchange").GetComponent<PguiButtonCtrl>();
		}

		public GameObject BaseObj;

		public PguiTextCtrl NumOwn;

		public ReuseScroll ScrollView;

		public GameObject TxtNoneObj;

		public GameObject TxtNoneFilteredObj;

		public PguiTextCtrl EndTimeText;

		public PguiButtonCtrl SwitchModeButton;

		public GameObject SwitchModeCheck;

		public PguiButtonCtrl ExchangeButton;
	}

	public class GUIFriendsUsecaseLabel
	{
		public GUIFriendsUsecaseLabel(GameObject go, CharaGrowItemInfo.NeedInfo info)
		{
			Dictionary<ItemDef.Kind, string> dictionary = new Dictionary<ItemDef.Kind, string>
			{
				{
					ItemDef.Kind.PROMOTE,
					"野生解放"
				},
				{
					ItemDef.Kind.RANK_UP,
					"けも級"
				},
				{
					ItemDef.Kind.ARTS_UP,
					"ミラクル強化"
				},
				{
					ItemDef.Kind.PHOTO_FRAME_UP,
					"フォトポケ"
				},
				{
					ItemDef.Kind.ABILITY_RELEASE,
					"なないろとくせい"
				}
			};
			this.baseObj = go;
			this.baseBtn = this.baseObj.GetComponent<PguiButtonCtrl>();
			this.charaIcon = this.baseObj.transform.Find("Icon_Chara/Icon_Chara").GetComponent<IconCharaCtrl>();
			this.titleText = this.baseObj.transform.Find("Info/Title").GetComponent<PguiTextCtrl>();
			this.numText = this.baseObj.transform.Find("Info/Num").GetComponent<PguiTextCtrl>();
			this.charaIcon.Setup(DataManager.DmChara.GetUserCharaData(info.charaId), SortFilterDefine.SortType.LEVEL, false, null, 0, -1, 0);
			this.charaIcon.DispPhotoPocketLevel(true);
			ItemDef.Kind needType = info.needType;
			if (needType != ItemDef.Kind.PROMOTE)
			{
				if (needType - ItemDef.Kind.RANK_UP <= 2 || needType == ItemDef.Kind.ABILITY_RELEASE)
				{
					this.baseBtn.AddOnClickListener(null, PguiButtonCtrl.SoundType.DEFAULT);
				}
			}
			else
			{
				this.baseBtn.AddOnClickListener(delegate(PguiButtonCtrl x)
				{
					CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(info.charaId);
					CharaStaticData staticData = userCharaData.staticData;
					CharaDynamicData dynamicData = userCharaData.dynamicData;
					CanvasManager.HdlShopWildReleaseWindow.owCtrl.Setup("強化後のステータス", "", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, null, null, false);
					CanvasManager.HdlShopWildReleaseWindow.iconChara.Setup(userCharaData, SortFilterDefine.SortType.LEVEL, false, null, 0, -1, 0);
					CanvasManager.HdlShopWildReleaseWindow.Txt_CharaName.text = staticData.GetName();
					CanvasManager.HdlShopWildReleaseWindow.Txt_Num.text = dynamicData.promoteNum.ToString() + "/" + staticData.maxPromoteNum.ToString();
					PrjUtil.ParamPreset paramPreset = PrjUtil.CalcParamByChara(dynamicData, dynamicData.level, dynamicData.rank);
					List<bool> list = new List<bool>(dynamicData.promoteFlag);
					list[info.promoteIndex] = true;
					PrjUtil.ParamPreset paramPreset2 = PrjUtil.CalcParamByChara(dynamicData, dynamicData.level, dynamicData.rank, dynamicData.promoteNum, list);
					List<int> list2 = new List<int>();
					list2.Add(paramPreset.totalParam);
					list2.Add(paramPreset.hp);
					list2.Add(paramPreset.atk);
					list2.Add(paramPreset.def);
					list2.Add(paramPreset.avoid);
					list2.Add(paramPreset.beatDamageRatio);
					list2.Add(paramPreset.actionDamageRatio);
					list2.Add(paramPreset.tryDamageRatio);
					List<int> list3 = new List<int> { paramPreset2.totalParam, paramPreset2.hp, paramPreset2.atk, paramPreset2.def, paramPreset2.avoid, paramPreset2.beatDamageRatio, paramPreset2.actionDamageRatio, paramPreset2.tryDamageRatio };
					SelCharaGrowWild.WindowWildResult.SetWindowParam(list2, list3, CanvasManager.HdlShopWildReleaseWindow.ParamAll);
					CanvasManager.HdlShopWildReleaseWindow.owCtrl.Open();
				}, PguiButtonCtrl.SoundType.DEFAULT);
			}
			this.titleText.text = dictionary[info.needType];
			this.numText.text = info.num.ToString();
		}

		public GameObject baseObj;

		public PguiButtonCtrl baseBtn;

		public IconCharaCtrl charaIcon;

		public PguiTextCtrl titleText;

		public PguiTextCtrl numText;
	}

	public class WindowTextSearchChange
	{
		public WindowTextSearchChange(Transform baseTr)
		{
			this.FirstInputField = baseTr.Find("Window0/InBase/InputField").GetComponent<InputField>();
			this.SecondInputField = baseTr.Find("Window1/InBase/InputField").GetComponent<InputField>();
			this.firstResetButton = baseTr.Find("Window0/InBase/InputField/Btn_Close").GetComponent<PguiButtonCtrl>();
			this.secondResetButton = baseTr.Find("Window1/InBase/InputField/Btn_Close").GetComponent<PguiButtonCtrl>();
			this.FirstInputField.lineType = InputField.LineType.SingleLine;
			this.SecondInputField.lineType = InputField.LineType.SingleLine;
		}

		public InputField FirstInputField;

		public InputField SecondInputField;

		public PguiButtonCtrl firstResetButton;

		public PguiButtonCtrl secondResetButton;
	}

	private class ItemBuyCheckResult
	{
		public bool IsLockNgReason
		{
			get
			{
				return this.ngReason == SelShopCtrl.ItemBuyCheckResult.Reason.BEFORE_START_TIME || this.ngReason == SelShopCtrl.ItemBuyCheckResult.Reason.NO_CLAER_QUEST || this.ngReason == SelShopCtrl.ItemBuyCheckResult.Reason.NO_CLAER_MISSION;
			}
		}

		public bool isBuy;

		public SelShopCtrl.ItemBuyCheckResult.Reason ngReason;

		public enum Reason
		{
			INVALID,
			SHOP_SOLDOUT,
			SHOP_MONEY_LACK,
			STACK_MAX,
			BEFORE_START_TIME,
			NO_CLAER_QUEST,
			NO_CLAER_MISSION
		}
	}

	public abstract class WindowBuyConfirm
	{
		public WindowBuyConfirm(Transform baseTr, Transform windowBase)
		{
			this.buyCount = 1;
			this.buyMax = 0;
			this.notBuyPhoto = false;
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.Base_BuyInfo = windowBase.Find("Base_BuyInfo").gameObject;
			this.Txt_owErrorText = windowBase.Find("BtnOk/DisableText").GetComponent<PguiTextCtrl>();
			this.Txt_BuyItemName = windowBase.Find("Base_BuyInfo/Txt01").GetComponent<PguiTextCtrl>();
			this.Txt_BuyItemInfo = windowBase.Find("Base_BuyInfo/Txt02").GetComponent<PguiTextCtrl>();
			this.Txt_BuyItemCount = windowBase.Find("Base_BuyInfo/Buy_Img/Txt_Window/Num_Txt").GetComponent<PguiTextCtrl>();
			this.Txt_BuyItemType = windowBase.Find("Base_BuyInfo/Parts_Exchange/Parts_ItemUseCoin/Txt01").GetComponent<PguiTextCtrl>();
			this.Txt_BuyBeforeMoney = windowBase.Find("Base_BuyInfo/Parts_Exchange/Parts_ItemUseCoin/Num_BeforeTxt").GetComponent<PguiTextCtrl>();
			this.Txt_BuyAfterMoney = windowBase.Find("Base_BuyInfo/Parts_Exchange/Parts_ItemUseCoin/Num_AfterTxt").GetComponent<PguiTextCtrl>();
			this.Txt_BuyBeforeCount = windowBase.Find("Base_BuyInfo/Parts_Exchange/Parts_ItemUseInfo/Num_BeforeTxt").GetComponent<PguiTextCtrl>();
			this.Txt_BuyAfterCount = windowBase.Find("Base_BuyInfo/Parts_Exchange/Parts_ItemUseInfo/Num_AfterTxt").GetComponent<PguiTextCtrl>();
			this.Parts_ItemUseInfo = windowBase.Find("Base_BuyInfo/Parts_Exchange/Parts_ItemUseInfo").gameObject;
			this.Parts_Exchange = windowBase.Find("Base_BuyInfo/Parts_Exchange").gameObject;
			this.Txt_BuyCount = windowBase.Find("Base_BuyInfo/Parts_Exchange/Exchange/Num_Txt").GetComponent<PguiTextCtrl>();
			this.Txt_Price = windowBase.Find("Base_BuyInfo/Parts_Exchange/Parts_ItemNeedInfo/Num_Txt").GetComponent<PguiTextCtrl>();
			this.BuyObject = windowBase.Find("Base_BuyInfo/Buy_Img").gameObject;
			this.NeedInfoImage = windowBase.Find("Base_BuyInfo/Parts_Exchange/Parts_ItemNeedInfo/Icon_Tex").GetComponent<PguiRawImageCtrl>();
			this.UseInfoImage = windowBase.Find("Base_BuyInfo/Parts_Exchange/Parts_ItemUseInfo/Icon_Tex").GetComponent<PguiRawImageCtrl>();
			this.UseMoneyImage = windowBase.Find("Base_BuyInfo/Parts_Exchange/Parts_ItemUseCoin/Icon_Tex").GetComponent<PguiRawImageCtrl>();
			this.UseInfoImage.gameObject.SetActive(false);
			this.Btn_Plus = this.Base_BuyInfo.transform.Find("Parts_Exchange/Exchange/Btn_Plus").GetComponent<PguiButtonCtrl>();
			this.Btn_Minus = this.Base_BuyInfo.transform.Find("Parts_Exchange/Exchange/Btn_Minus").GetComponent<PguiButtonCtrl>();
			this.IconItem = windowBase.Find("Base_BuyInfo/Buy_Img/Icon_Item").GetComponent<IconItemCtrl>();
			this.InfoBtn = windowBase.Find("Base_BuyInfo/Buy_Img/Cmn_Btn_Info").GetComponent<PguiButtonCtrl>();
			this.SliderBar = this.Base_BuyInfo.transform.Find("Parts_Exchange/Exchange/SliderBar").GetComponent<Slider>();
			this.SliderBar.minValue = 1f;
			this.SliderBar.maxValue = 100f;
		}

		public PguiOpenWindowCtrl owCtrl;

		public PguiTextCtrl Txt_owErrorText;

		public PguiTextCtrl Txt_BuyCount;

		public PguiTextCtrl Txt_BuyItemName;

		public PguiTextCtrl Txt_BuyItemInfo;

		public PguiTextCtrl Txt_BuyItemCount;

		public PguiTextCtrl Txt_BuyItemType;

		public PguiTextCtrl Txt_BuyBeforeMoney;

		public PguiTextCtrl Txt_BuyAfterMoney;

		public PguiTextCtrl Txt_BuyBeforeCount;

		public PguiTextCtrl Txt_BuyAfterCount;

		public PguiTextCtrl Txt_ExchangeCount;

		public PguiTextCtrl Txt_Price;

		public PguiTextCtrl Txt_Warning;

		public IconItemCtrl IconItem;

		public PguiButtonCtrl InfoBtn;

		public GameObject BuyObject;

		public List<IconItemCtrl> IconItemDouble;

		public List<PguiTextCtrl> IconItemNumDouble;

		public PguiRawImageCtrl NeedInfoImage;

		public PguiRawImageCtrl UseInfoImage;

		public PguiRawImageCtrl UseMoneyImage;

		public GameObject Base_BuyInfo;

		public GameObject Parts_ItemUseInfo;

		public GameObject Parts_Exchange;

		public ShopData.ItemOne itemData;

		public ShopData.ItemOne replaceBeforeItemData;

		public bool isBuy;

		public int buyCount;

		public int buyMax;

		public string title;

		public string info;

		public bool tryingToBuyNotComeCharaClothes;

		public PguiButtonCtrl Btn_Plus;

		public PguiButtonCtrl Btn_Minus;

		public bool notBuyPhoto;

		public string buyComment;

		public string reqComment;

		public Slider SliderBar;
	}

	public class WindowBuyConfirmNormal : SelShopCtrl.WindowBuyConfirm
	{
		public WindowBuyConfirmNormal(Transform baseTr)
			: base(baseTr, baseTr.Find("Base/Window"))
		{
			Transform transform = baseTr.Find("Base/Window");
			this.Base_NotPhotoBuyInfo = transform.Find("Base_BuyInfo/Parts_NotPhotoBuy").gameObject;
			this.BuyObjectDouble = transform.Find("Base_BuyInfo/Buy_Img_Exchange").gameObject;
			this.IconItemDouble = new List<IconItemCtrl>
			{
				this.BuyObjectDouble.transform.Find("Buy_Img_Before/Icon_Item").GetComponent<IconItemCtrl>(),
				this.BuyObjectDouble.transform.Find("Buy_Img_After/Icon_Item").GetComponent<IconItemCtrl>()
			};
			this.IconItemNumDouble = new List<PguiTextCtrl>
			{
				this.BuyObjectDouble.transform.Find("Buy_Img_Before/Txt_Window/Num_Txt").GetComponent<PguiTextCtrl>(),
				this.BuyObjectDouble.transform.Find("Buy_Img_After/Txt_Window/Num_Txt").GetComponent<PguiTextCtrl>()
			};
			this.Btn_PhotoGrow = this.Base_NotPhotoBuyInfo.transform.Find("Btn_PhotoGrow").GetComponent<PguiButtonCtrl>();
			this.Btn_PhotoSell = this.Base_NotPhotoBuyInfo.transform.Find("Btn_PhotoSell").GetComponent<PguiButtonCtrl>();
			this.Txt_Warning = this.BuyObjectDouble.transform.Find("Txt_Info").GetComponent<PguiTextCtrl>();
		}

		public GameObject Base_NotPhotoBuyInfo;

		public GameObject BuyObjectDouble;

		public PguiButtonCtrl Btn_PhotoGrow;

		public PguiButtonCtrl Btn_PhotoSell;
	}

	public class WindowBuyConfirmKiraKira : SelShopCtrl.WindowBuyConfirmNormal
	{
		public WindowBuyConfirmKiraKira(Transform baseTr)
			: base(baseTr)
		{
			this.Btn_PurchaseConfirm = baseTr.Find("Base/Window/PurchaseConfirmButton").GetComponent<PguiButtonCtrl>();
		}

		public PguiButtonCtrl Btn_PurchaseConfirm;
	}

	public class WindowBuyConfirmGrow : SelShopCtrl.WindowBuyConfirm
	{
		public WindowBuyConfirmGrow(Transform baseTr)
			: base(baseTr, baseTr.Find("Base/Window/Nul"))
		{
			this.Message_NotFriends = baseTr.Find("Base/Window/Message_NotFriends").gameObject;
			this.GrowCharaScroll = baseTr.Find("Base/Window/InBase/ScrollView").GetComponent<ReuseScroll>();
		}

		public GameObject Message_NotFriends;

		public ReuseScroll GrowCharaScroll;
	}

	public class WindowBuyConfirmGrowKiraKira : SelShopCtrl.WindowBuyConfirmGrow
	{
		public WindowBuyConfirmGrowKiraKira(Transform baseTr)
			: base(baseTr)
		{
			this.Btn_PurchaseConfirm = baseTr.Find("Base/Window/Nul/PurchaseConfirmButton").GetComponent<PguiButtonCtrl>();
		}

		public PguiButtonCtrl Btn_PurchaseConfirm;
	}

	public class WindowBuyConfirmBulk : SelShopCtrl.WindowBuyConfirm
	{
		public WindowBuyConfirmBulk(Transform baseTr, GameObject layout)
			: base(baseTr, baseTr.Find("Base/Window"))
		{
			Transform transform = baseTr.Find("Base/Window");
			this.Txt_BuyItemCount = transform.Find("Base_BuyInfo/Icon_Item/Num_Own").GetComponent<PguiTextCtrl>();
			this.IconItem = transform.Find("Base_BuyInfo/Icon_Item").GetComponent<IconItemCtrl>();
			this.BuyObject = transform.Find("Base_BuyInfo/Icon_Item").gameObject;
			this.InfoBtn = transform.Find("Base_BuyInfo/Buy_Img/Cmn_Btn_Info").GetComponent<PguiButtonCtrl>();
			this.LayoutObject = layout;
			this.scrollContent = transform.Find("InBase/ScrollView/Viewport/Content");
			this.ItemScroll = transform.Find("InBase/ScrollView").GetComponent<ReuseScroll>();
		}

		public GameObject LayoutObject;

		public Transform scrollContent;

		public ReuseScroll ItemScroll;
	}

	public class WindowBuyEnd
	{
		public WindowBuyEnd(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			Transform transform = baseTr.Find("Base/Window/Base_BuyInfo");
			this.Txt_ItemName = transform.Find("Txt01").GetComponent<PguiTextCtrl>();
			this.Txt_ItemCount = transform.Find("Txt02").GetComponent<PguiTextCtrl>();
			this.Txt_ItemReq = transform.Find("Txt03").GetComponent<PguiTextCtrl>();
			this.Txt_BuyBeforeMoney = transform.Find("Parts_Exchange/Parts_ItemUseCoin/Num_BeforeTxt").GetComponent<PguiTextCtrl>();
			this.Txt_BuyAfterMoney = transform.Find("Parts_Exchange/Parts_ItemUseCoin/Num_AfterTxt").GetComponent<PguiTextCtrl>();
			this.Txt_BuyBeforeCount = transform.Find("Parts_Exchange/Parts_ItemUseInfo/Num_BeforeTxt").GetComponent<PguiTextCtrl>();
			this.Txt_BuyAfterCount = transform.Find("Parts_Exchange/Parts_ItemUseInfo/Num_AfterTxt").GetComponent<PguiTextCtrl>();
			this.UseInfoImage = transform.Find("Parts_Exchange/Parts_ItemUseInfo/Icon_Tex").GetComponent<PguiRawImageCtrl>();
			this.UseMoneyImage = transform.Find("Parts_Exchange/Parts_ItemUseCoin/Icon_Tex").GetComponent<PguiRawImageCtrl>();
			this.Parts_ItemUseInfo = transform.Find("Parts_Exchange/Parts_ItemUseInfo").gameObject;
			this.UseInfoImage.gameObject.SetActive(false);
		}

		public GameObject baseObj;

		public PguiOpenWindowCtrl owCtrl;

		public PguiTextCtrl Txt_ItemName;

		public PguiTextCtrl Txt_ItemCount;

		public PguiTextCtrl Txt_ItemReq;

		public PguiTextCtrl Txt_BuyBeforeMoney;

		public PguiTextCtrl Txt_BuyAfterMoney;

		public PguiTextCtrl Txt_BuyBeforeCount;

		public PguiTextCtrl Txt_BuyAfterCount;

		public PguiRawImageCtrl UseInfoImage;

		public PguiRawImageCtrl UseMoneyImage;

		public GameObject Parts_ItemUseInfo;
	}

	public class GridGUI
	{
		public GridGUI(GameObject go)
		{
			this.baseObj = go;
			this.gridLayoutGroup = this.baseObj.GetComponent<GridLayoutGroup>();
			this.iconItemList = new List<IconItemCtrl>();
			for (int i = 0; i < this.gridLayoutGroup.constraintCount; i++)
			{
				Transform transform = go.transform.Find("iconBase/ItemIcon" + i.ToString("D2"));
				if (null == transform)
				{
					break;
				}
				this.iconItemList.Add(transform.GetComponent<IconItemCtrl>());
			}
		}

		public GameObject baseObj;

		public GridLayoutGroup gridLayoutGroup;

		public List<IconItemCtrl> iconItemList;
	}
}
