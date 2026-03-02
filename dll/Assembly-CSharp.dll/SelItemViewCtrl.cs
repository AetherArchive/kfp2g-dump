using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000160 RID: 352
public class SelItemViewCtrl : MonoBehaviour
{
	// Token: 0x17000384 RID: 900
	// (get) Token: 0x06001447 RID: 5191 RVA: 0x000F8588 File Offset: 0x000F6788
	// (set) Token: 0x06001448 RID: 5192 RVA: 0x000F8590 File Offset: 0x000F6790
	public bool BackAnimPlaying { get; private set; }

	// Token: 0x17000385 RID: 901
	// (get) Token: 0x06001449 RID: 5193 RVA: 0x000F8599 File Offset: 0x000F6799
	// (set) Token: 0x0600144A RID: 5194 RVA: 0x000F85A1 File Offset: 0x000F67A1
	private int CurrentItemId { get; set; }

	// Token: 0x17000386 RID: 902
	// (get) Token: 0x0600144B RID: 5195 RVA: 0x000F85AA File Offset: 0x000F67AA
	// (set) Token: 0x0600144C RID: 5196 RVA: 0x000F85B2 File Offset: 0x000F67B2
	private int CurrentTabIndex { get; set; }

	// Token: 0x0600144D RID: 5197 RVA: 0x000F85BC File Offset: 0x000F67BC
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

	// Token: 0x0600144E RID: 5198 RVA: 0x000F8708 File Offset: 0x000F6908
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

	// Token: 0x0600144F RID: 5199 RVA: 0x000F8830 File Offset: 0x000F6A30
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

	// Token: 0x06001450 RID: 5200 RVA: 0x000F88B7 File Offset: 0x000F6AB7
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

	// Token: 0x06001451 RID: 5201 RVA: 0x000F88C8 File Offset: 0x000F6AC8
	private List<ItemData> GetItemDataList(DataManagerItem.DispType type)
	{
		return this.TabItemDataList.Find((SelItemViewCtrl.TabItemList x) => x.dispType == type).ItemDataList;
	}

	// Token: 0x06001452 RID: 5202 RVA: 0x000F8900 File Offset: 0x000F6B00
	private void UpdateTabItemData()
	{
		this.TabItemDataList = new List<SelItemViewCtrl.TabItemList>();
		this.TabItemDataList.Add(new SelItemViewCtrl.TabItemList(DataManagerItem.DispType.Common));
		this.TabItemDataList.Add(new SelItemViewCtrl.TabItemList(DataManagerItem.DispType.Growth));
		this.TabItemDataList.Add(new SelItemViewCtrl.TabItemList(DataManagerItem.DispType.Decoration));
		this.TabItemDataList.Add(new SelItemViewCtrl.TabItemList(DataManagerItem.DispType.PlayItem));
	}

	// Token: 0x06001453 RID: 5203 RVA: 0x000F895C File Offset: 0x000F6B5C
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

	// Token: 0x06001454 RID: 5204 RVA: 0x000F8B44 File Offset: 0x000F6D44
	public void InitializeItemInfo()
	{
		if (0 < this.GetItemDataList(DataManagerItem.DispType.Common).Count)
		{
			ItemStaticBase staticData = this.GetItemDataList(DataManagerItem.DispType.Common)[0].staticData;
			this.UpdateItemInfo(staticData);
		}
	}

	// Token: 0x06001455 RID: 5205 RVA: 0x000F8B7C File Offset: 0x000F6D7C
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

	// Token: 0x06001456 RID: 5206 RVA: 0x000F8CB4 File Offset: 0x000F6EB4
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

	// Token: 0x06001457 RID: 5207 RVA: 0x000F8D28 File Offset: 0x000F6F28
	private void OnStartItemView(int rowIdx, GameObject go)
	{
		GameObject gameObject = (GameObject)Resources.Load("SceneMenu/GUI/Prefab/ItemList_ItemSet");
		DataManagerItem.DispType iconItemTabType = this.GetIconItemTabType(go);
		this.SetItemView(rowIdx, go, iconItemTabType, gameObject);
		this.InitializeItemInfo();
	}

	// Token: 0x06001458 RID: 5208 RVA: 0x000F8D60 File Offset: 0x000F6F60
	private void OnUpdateItemView(int rowIdx, GameObject go)
	{
		DataManagerItem.DispType iconItemTabType = this.GetIconItemTabType(go);
		this.SetItemView(rowIdx, go, iconItemTabType, null);
	}

	// Token: 0x06001459 RID: 5209 RVA: 0x000F8D80 File Offset: 0x000F6F80
	private void OnClickButton(DataManagerItem.DispType type, int itemIdx)
	{
		ItemStaticBase staticData = this.GetItemDataList(type)[itemIdx].staticData;
		this.UpdateItemInfo(staticData);
	}

	// Token: 0x0600145A RID: 5210 RVA: 0x000F8DA8 File Offset: 0x000F6FA8
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

	// Token: 0x0600145B RID: 5211 RVA: 0x000F8E54 File Offset: 0x000F7054
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

	// Token: 0x0600145C RID: 5212 RVA: 0x000F8E88 File Offset: 0x000F7088
	public void OnClickSellItemButton(PguiButtonCtrl button)
	{
		ItemData userItemData = DataManager.DmItem.GetUserItemData(this.CurrentItemId);
		CanvasManager.HdlCmnItemWindowCtrl.OpenBuyInfoWindow(userItemData);
	}

	// Token: 0x0600145D RID: 5213 RVA: 0x000F8EB4 File Offset: 0x000F70B4
	public void OnClickContentButton(PguiButtonCtrl button)
	{
		ItemData userItemData = DataManager.DmItem.GetUserItemData(this.CurrentItemId);
		CanvasManager.HdlCmnItemWindowCtrl.OpenBankContentWindow(userItemData);
	}

	// Token: 0x0600145E RID: 5214 RVA: 0x000F8EE0 File Offset: 0x000F70E0
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

	// Token: 0x0600145F RID: 5215 RVA: 0x000F8F60 File Offset: 0x000F7160
	public bool OnClickMoveSequenceButton(SceneManager.SceneName sceneName, object sceneArgs)
	{
		this.currentEnumerator = this.RequestUpdateOption();
		this.OnClickMoveSequenceName = sceneName;
		this.OnClickMoveSequenceArgs = sceneArgs;
		return true;
	}

	// Token: 0x040010BC RID: 4284
	private SelItemViewCtrl.GUI guiData;

	// Token: 0x040010BD RID: 4285
	private IEnumerator currentEnumerator;

	// Token: 0x040010BE RID: 4286
	private List<SelItemViewCtrl.TabItemList> TabItemDataList;

	// Token: 0x040010BF RID: 4287
	private List<ItemData> updateTargetItemDataList;

	// Token: 0x040010C2 RID: 4290
	private SceneManager.SceneName OnClickMoveSequenceName;

	// Token: 0x040010C3 RID: 4291
	private object OnClickMoveSequenceArgs;

	// Token: 0x02000B71 RID: 2929
	private class TabItemList
	{
		// Token: 0x060042D4 RID: 17108 RVA: 0x00201377 File Offset: 0x001FF577
		public TabItemList(DataManagerItem.DispType type)
		{
			this.dispType = type;
			this.ItemDataList = DataManager.DmItem.GetUserTabItemList(DataManager.DmItem.GetUserItemMap(), this.dispType);
		}

		// Token: 0x04004782 RID: 18306
		public DataManagerItem.DispType dispType;

		// Token: 0x04004783 RID: 18307
		public List<ItemData> ItemDataList;
	}

	// Token: 0x02000B72 RID: 2930
	public class GUI
	{
		// Token: 0x060042D5 RID: 17109 RVA: 0x002013A8 File Offset: 0x001FF5A8
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

		// Token: 0x04004784 RID: 18308
		public GameObject baseObj;

		// Token: 0x04004785 RID: 18309
		public PguiTextCtrl infoTitle;

		// Token: 0x04004786 RID: 18310
		public IconItemCtrl infoIcon;

		// Token: 0x04004787 RID: 18311
		public Text infoText;

		// Token: 0x04004788 RID: 18312
		public PguiTextCtrl infoTextNote;

		// Token: 0x04004789 RID: 18313
		public PguiTextCtrl NoItemText;

		// Token: 0x0400478A RID: 18314
		public PguiTabGroupCtrl TabGroup;

		// Token: 0x0400478B RID: 18315
		public PguiButtonCtrl SellButton;

		// Token: 0x0400478C RID: 18316
		public PguiButtonCtrl ContentButton;

		// Token: 0x0400478D RID: 18317
		public List<SelItemViewCtrl.GUI.TabScrollView> TabScrollViewList;

		// Token: 0x0400478E RID: 18318
		public List<GameObject> TabList;

		// Token: 0x0400478F RID: 18319
		public SimpleAnimation inOutAnimation;

		// Token: 0x02001193 RID: 4499
		public class TabScrollView
		{
			// Token: 0x0600569C RID: 22172 RVA: 0x00252D95 File Offset: 0x00250F95
			public TabScrollView(DataManagerItem.DispType type, ReuseScroll scrl)
			{
				this.dispType = type;
				this.scroll = scrl;
			}

			// Token: 0x0400603D RID: 24637
			public DataManagerItem.DispType dispType;

			// Token: 0x0400603E RID: 24638
			public ReuseScroll scroll;
		}
	}
}
