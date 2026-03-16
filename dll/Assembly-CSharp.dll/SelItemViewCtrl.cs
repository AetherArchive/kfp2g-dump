using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.UI;

public class SelItemViewCtrl : MonoBehaviour
{
	public bool BackAnimPlaying { get; private set; }

	private int CurrentItemId { get; set; }

	private int CurrentTabIndex { get; set; }

	public void Init()
	{
		this.UpdateTabItemData();
		this.guiData = new SelItemViewCtrl.GUI(AssetManager.InstantiateAssetData("SceneMenu/GUI/Prefab/GUI_ItemList", base.transform).transform);
		SelItemViewCtrl.GUI gui = this.guiData;
		foreach (SelItemViewCtrl.GUI.TabScrollView tabScrollView in this.guiData.TabScrollViewList)
		{
			ReuseScroll scroll = tabScrollView.scroll;
			scroll.onStartItem = (Action<int, GameObject>)Delegate.Combine(scroll.onStartItem, new Action<int, GameObject>(this.OnStartItemView));
			ReuseScroll scroll2 = tabScrollView.scroll;
			scroll2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scroll2.onUpdateItem, new Action<int, GameObject>(this.OnUpdateItemView));
			List<ItemData> itemDataList = this.GetItemDataList(tabScrollView.dispType);
			int num = itemDataList.Count / 3 + ((itemDataList.Count % 3 == 0) ? 0 : 1);
			tabScrollView.scroll.Setup(num, 0);
		}
		this.guiData.SellButton.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickSellItemButton), PguiButtonCtrl.SoundType.DECIDE);
		this.guiData.ContentButton.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickContentButton), PguiButtonCtrl.SoundType.DECIDE);
		CanvasManager.HdlCmnItemWindowCtrl.selItemViewCtrl = this;
	}

	public void Setup()
	{
		this.BackAnimPlaying = false;
		this.UpdateTabItemData();
		this.updateTargetItemDataList = new List<ItemData>();
		this.updateTargetItemDataList.Add(DataManager.DmItem.GetUserItemData(30001));
		this.updateTargetItemDataList.Add(DataManager.DmItem.GetUserItemData(30002));
		foreach (SelItemViewCtrl.GUI.TabScrollView tabScrollView in this.guiData.TabScrollViewList)
		{
			List<ItemData> itemDataList = this.GetItemDataList(tabScrollView.dispType);
			int num = itemDataList.Count / 3 + ((itemDataList.Count % 3 == 0) ? 0 : 1);
			tabScrollView.scroll.Resize(num, 0);
		}
		this.OnClickMoveSequenceName = SceneManager.SceneName.None;
		this.OnClickMoveSequenceArgs = null;
		this.CurrentTabIndex = 0;
		this.guiData.TabGroup.Setup(this.CurrentTabIndex, new PguiTabGroupCtrl.OnSelectTab(this.OnSelectTab));
		this.OnSelectTab(this.CurrentTabIndex);
		this.guiData.inOutAnimation.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
	}

	private void Update()
	{
		if (this.currentEnumerator != null && !this.currentEnumerator.MoveNext())
		{
			this.currentEnumerator = null;
		}
		if (this.updateTargetItemDataList != null)
		{
			for (int i = 0; i < this.updateTargetItemDataList.Count; i++)
			{
				ItemData itemData = this.updateTargetItemDataList[i];
				ItemData userItemData = DataManager.DmItem.GetUserItemData(itemData.id);
				if (userItemData.num != itemData.num)
				{
					this.updateTargetItemDataList[i] = userItemData;
					this.RefreshScrollItem();
					return;
				}
			}
		}
	}

	private IEnumerator RequestUpdateOption()
	{
		if (this.OnClickMoveSequenceName == SceneManager.SceneName.None)
		{
			CanvasManager.HdlCmnMenu.MoveSceneByMenu(SceneManager.SceneName.SceneOtherMenuTop, null);
		}
		else
		{
			CanvasManager.HdlCmnMenu.MoveSceneByMenu(this.OnClickMoveSequenceName, this.OnClickMoveSequenceArgs);
		}
		yield return null;
		yield break;
	}

	private List<ItemData> GetItemDataList(DataManagerItem.DispType type)
	{
		return this.TabItemDataList.Find((SelItemViewCtrl.TabItemList x) => x.dispType == type).ItemDataList;
	}

	private void UpdateTabItemData()
	{
		this.TabItemDataList = new List<SelItemViewCtrl.TabItemList>();
		this.TabItemDataList.Add(new SelItemViewCtrl.TabItemList(DataManagerItem.DispType.Common));
		this.TabItemDataList.Add(new SelItemViewCtrl.TabItemList(DataManagerItem.DispType.Growth));
		this.TabItemDataList.Add(new SelItemViewCtrl.TabItemList(DataManagerItem.DispType.Decoration));
		this.TabItemDataList.Add(new SelItemViewCtrl.TabItemList(DataManagerItem.DispType.PlayItem));
	}

	private void SetItemView(int rowIdx, GameObject go, DataManagerItem.DispType type, GameObject parent = null)
	{
		for (int i = 0; i < 3; i++)
		{
			int itemIdx = rowIdx * 3 + i;
			GameObject gameObject;
			if (null != parent)
			{
				gameObject = Object.Instantiate<GameObject>(parent, go.transform);
				Transform transform = gameObject.transform.Find("BaseImage/Icon_Item");
				if (transform != null)
				{
					PguiNestPrefab component = transform.GetComponent<PguiNestPrefab>();
					if (component != null)
					{
						component.InitForce();
					}
				}
				gameObject.name = i.ToString();
			}
			else
			{
				gameObject = go.transform.Find(i.ToString()).gameObject;
			}
			PguiButtonCtrl component2 = gameObject.GetComponent<PguiButtonCtrl>();
			List<ItemData> itemDataList = this.GetItemDataList(type);
			if (itemDataList.Count <= itemIdx)
			{
				component2.gameObject.SetActive(false);
			}
			else
			{
				component2.gameObject.SetActive(true);
				Transform transform2 = gameObject.transform.Find("BaseImage/Icon_Item/Icon_Item");
				GameObject gameObject2 = ((transform2 != null) ? transform2.gameObject : null);
				if (null != gameObject2)
				{
					IconItemCtrl component3 = gameObject2.GetComponent<IconItemCtrl>();
					if (null != component3)
					{
						component3.Setup(itemDataList[itemIdx].staticData);
						if (null != parent)
						{
							component3.SetRaycastTargetIconItem(false);
						}
					}
				}
				gameObject.transform.Find("BaseImage/Title").gameObject.GetComponent<Text>().text = itemDataList[itemIdx].staticData.GetName();
				gameObject.transform.Find("BaseImage/Num").gameObject.GetComponent<PguiTextCtrl>().text = itemDataList[itemIdx].num.ToString();
				gameObject.GetComponent<PguiButtonCtrl>().AddOnClickListener(delegate(PguiButtonCtrl button)
				{
					this.OnClickButton(type, itemIdx);
				}, PguiButtonCtrl.SoundType.DEFAULT);
			}
		}
	}

	public void InitializeItemInfo()
	{
		if (0 < this.GetItemDataList(DataManagerItem.DispType.Common).Count)
		{
			ItemStaticBase staticData = this.GetItemDataList(DataManagerItem.DispType.Common)[0].staticData;
			this.UpdateItemInfo(staticData);
		}
	}

	private void UpdateItemInfo(ItemStaticBase itemBase)
	{
		this.guiData.infoTitle.text = itemBase.GetName();
		this.guiData.infoText.text = itemBase.GetInfo();
		this.guiData.infoIcon.Setup(itemBase);
		this.CurrentItemId = itemBase.GetId();
		ItemCommonData itemCommonData = DataManager.DmItem.GetItemStaticMap()[itemBase.GetId()] as ItemCommonData;
		if (itemCommonData != null)
		{
			this.guiData.SellButton.gameObject.SetActive(!itemCommonData.notForSale);
			this.guiData.ContentButton.gameObject.SetActive(this.CurrentItemId == 30090);
		}
		else
		{
			this.guiData.SellButton.gameObject.SetActive(false);
			this.guiData.ContentButton.gameObject.SetActive(false);
		}
		DateTime? endTime = itemBase.endTime;
		if (endTime == null)
		{
			this.guiData.infoTextNote.text = string.Empty;
			return;
		}
		DateTime value = endTime.Value;
		this.guiData.infoTextNote.text = "※このアイテムは " + TimeManager.FormattedTime(value, TimeManager.Format.yyyyMMdd_hhmm) + " に消失します。";
	}

	private DataManagerItem.DispType GetIconItemTabType(GameObject go)
	{
		string name = go.transform.parent.parent.parent.parent.name;
		DataManagerItem.DispType dispType;
		if ("Window_Common" == name)
		{
			dispType = DataManagerItem.DispType.Common;
		}
		else if ("Window_Growth" == name)
		{
			dispType = DataManagerItem.DispType.Growth;
		}
		else if ("Window_Ivent" == name)
		{
			dispType = DataManagerItem.DispType.Decoration;
		}
		else if ("Window_PlayItem" == name)
		{
			dispType = DataManagerItem.DispType.PlayItem;
		}
		else
		{
			dispType = DataManagerItem.DispType.Undefined;
		}
		return dispType;
	}

	private void OnStartItemView(int rowIdx, GameObject go)
	{
		GameObject gameObject = (GameObject)Resources.Load("SceneMenu/GUI/Prefab/ItemList_ItemSet");
		DataManagerItem.DispType iconItemTabType = this.GetIconItemTabType(go);
		this.SetItemView(rowIdx, go, iconItemTabType, gameObject);
		this.InitializeItemInfo();
	}

	private void OnUpdateItemView(int rowIdx, GameObject go)
	{
		DataManagerItem.DispType iconItemTabType = this.GetIconItemTabType(go);
		this.SetItemView(rowIdx, go, iconItemTabType, null);
	}

	private void OnClickButton(DataManagerItem.DispType type, int itemIdx)
	{
		ItemStaticBase staticData = this.GetItemDataList(type)[itemIdx].staticData;
		this.UpdateItemInfo(staticData);
	}

	private bool OnSelectTab(int tabIdx)
	{
		int num = 0;
		foreach (GameObject gameObject in this.guiData.TabList)
		{
			if (null != gameObject)
			{
				gameObject.SetActive(tabIdx == num);
				this.guiData.NoItemText.gameObject.SetActive(this.GetItemDataList(this.guiData.TabScrollViewList[tabIdx].dispType).Count == 0);
			}
			num++;
		}
		this.CurrentTabIndex = tabIdx;
		return true;
	}

	public bool OnClickReturnButton()
	{
		if (this.BackAnimPlaying)
		{
			return true;
		}
		this.BackAnimPlaying = true;
		this.guiData.inOutAnimation.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
		{
			this.currentEnumerator = this.RequestUpdateOption();
		});
		return true;
	}

	public void OnClickSellItemButton(PguiButtonCtrl button)
	{
		ItemData userItemData = DataManager.DmItem.GetUserItemData(this.CurrentItemId);
		CanvasManager.HdlCmnItemWindowCtrl.OpenBuyInfoWindow(userItemData);
	}

	public void OnClickContentButton(PguiButtonCtrl button)
	{
		ItemData userItemData = DataManager.DmItem.GetUserItemData(this.CurrentItemId);
		CanvasManager.HdlCmnItemWindowCtrl.OpenBankContentWindow(userItemData);
	}

	public void RefreshScrollItem()
	{
		this.UpdateTabItemData();
		foreach (SelItemViewCtrl.GUI.TabScrollView tabScrollView in this.guiData.TabScrollViewList)
		{
			tabScrollView.scroll.Refresh();
		}
		ItemData userItemData = DataManager.DmItem.GetUserItemData(this.CurrentItemId);
		if (userItemData.num == 0)
		{
			this.InitializeItemInfo();
		}
	}

	public bool OnClickMoveSequenceButton(SceneManager.SceneName sceneName, object sceneArgs)
	{
		this.currentEnumerator = this.RequestUpdateOption();
		this.OnClickMoveSequenceName = sceneName;
		this.OnClickMoveSequenceArgs = sceneArgs;
		return true;
	}

	private SelItemViewCtrl.GUI guiData;

	private IEnumerator currentEnumerator;

	private List<SelItemViewCtrl.TabItemList> TabItemDataList;

	private List<ItemData> updateTargetItemDataList;

	private SceneManager.SceneName OnClickMoveSequenceName;

	private object OnClickMoveSequenceArgs;

	private class TabItemList
	{
		public TabItemList(DataManagerItem.DispType type)
		{
			this.dispType = type;
			this.ItemDataList = DataManager.DmItem.GetUserTabItemList(DataManager.DmItem.GetUserItemMap(), this.dispType);
		}

		public DataManagerItem.DispType dispType;

		public List<ItemData> ItemDataList;
	}

	public class GUI
	{
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.infoTitle = baseTr.Find("All/Info/Set/Title").GetComponent<PguiTextCtrl>();
			this.infoIcon = baseTr.Find("All/Info/Set/Icon_Item/Icon_Item").GetComponent<IconItemCtrl>();
			this.infoText = baseTr.Find("All/Info/Set/Txt").GetComponent<Text>();
			this.infoTextNote = baseTr.Find("All/Info/Set/Txt_Note").GetComponent<PguiTextCtrl>();
			this.NoItemText = baseTr.Find("All/Info/Txt_NoItem").GetComponent<PguiTextCtrl>();
			this.NoItemText.gameObject.SetActive(false);
			this.TabGroup = baseTr.Find("All/WindowAll/TabGroup").GetComponent<PguiTabGroupCtrl>();
			this.TabList = new List<GameObject>();
			this.TabList.Add(baseTr.Find("All/WindowAll/Window_Common").gameObject);
			this.TabList.Add(baseTr.Find("All/WindowAll/Window_Growth").gameObject);
			this.TabList.Add(baseTr.Find("All/WindowAll/Window_Ivent").gameObject);
			this.TabList.Add(baseTr.Find("All/WindowAll/Window_PlayItem").gameObject);
			this.TabScrollViewList = new List<SelItemViewCtrl.GUI.TabScrollView>();
			this.TabScrollViewList.Add(new SelItemViewCtrl.GUI.TabScrollView(DataManagerItem.DispType.Common, baseTr.Find("All/WindowAll/Window_Common/ScrollView").GetComponent<ReuseScroll>()));
			this.TabScrollViewList.Add(new SelItemViewCtrl.GUI.TabScrollView(DataManagerItem.DispType.Growth, baseTr.Find("All/WindowAll/Window_Growth/ScrollView").GetComponent<ReuseScroll>()));
			this.TabScrollViewList.Add(new SelItemViewCtrl.GUI.TabScrollView(DataManagerItem.DispType.Decoration, baseTr.Find("All/WindowAll/Window_Ivent/ScrollView").GetComponent<ReuseScroll>()));
			this.TabScrollViewList.Add(new SelItemViewCtrl.GUI.TabScrollView(DataManagerItem.DispType.PlayItem, baseTr.Find("All/WindowAll/Window_PlayItem/ScrollView").GetComponent<ReuseScroll>()));
			this.SellButton = baseTr.Find("All/Info/Button_sell").GetComponent<PguiButtonCtrl>();
			this.ContentButton = baseTr.Find("All/Info/Button_content").GetComponent<PguiButtonCtrl>();
			this.inOutAnimation = baseTr.GetComponent<SimpleAnimation>();
		}

		public GameObject baseObj;

		public PguiTextCtrl infoTitle;

		public IconItemCtrl infoIcon;

		public Text infoText;

		public PguiTextCtrl infoTextNote;

		public PguiTextCtrl NoItemText;

		public PguiTabGroupCtrl TabGroup;

		public PguiButtonCtrl SellButton;

		public PguiButtonCtrl ContentButton;

		public List<SelItemViewCtrl.GUI.TabScrollView> TabScrollViewList;

		public List<GameObject> TabList;

		public SimpleAnimation inOutAnimation;

		public class TabScrollView
		{
			public TabScrollView(DataManagerItem.DispType type, ReuseScroll scrl)
			{
				this.dispType = type;
				this.scroll = scrl;
			}

			public DataManagerItem.DispType dispType;

			public ReuseScroll scroll;
		}
	}
}
