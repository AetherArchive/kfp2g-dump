using System;
using System.Collections.Generic;
using System.Linq;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001AB RID: 427
public class ItemPresetWindowCtrl : MonoBehaviour
{
	// Token: 0x06001CD1 RID: 7377 RVA: 0x00169760 File Offset: 0x00167960
	public void Init(Transform baseTr)
	{
		this.guiData = new ItemPresetWindowCtrl.GUI(baseTr);
		UnityAction unityAction = delegate
		{
			using (Dictionary<GameObject, List<ItemPresetWindowCtrl.GUIParts>>.ValueCollection.Enumerator enumerator = this.guiData.scrollViewParts.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.ForEach(delegate(ItemPresetWindowCtrl.GUIParts part)
					{
						part.iconItem.Destroy();
					});
				}
			}
		};
		this.guiData.owCtrl.Setup("", null, null, true, null, unityAction, false);
		this.guiData.ScrollView.InitForce();
		ReuseScroll scrollView = this.guiData.ScrollView;
		scrollView.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollView.onStartItem, new Action<int, GameObject>(this.OnStartItem));
		ReuseScroll scrollView2 = this.guiData.ScrollView;
		scrollView2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollView2.onUpdateItem, new Action<int, GameObject>(this.OnUpdateItem));
		this.guiData.ScrollView.Setup(10, 0);
	}

	// Token: 0x06001CD2 RID: 7378 RVA: 0x0016981C File Offset: 0x00167A1C
	private void OnStartItem(int index, GameObject go)
	{
		GameObject gameObject = AssetManager.GetAssetData("Cmn/GUI/Prefab/CmnShop_SetItem") as GameObject;
		List<ItemPresetWindowCtrl.GUIParts> list = new List<ItemPresetWindowCtrl.GUIParts>();
		for (int i = 0; i < 4; i++)
		{
			ItemPresetWindowCtrl.GUIParts guiparts = new ItemPresetWindowCtrl.GUIParts(Object.Instantiate<GameObject>(gameObject, go.transform).transform);
			list.Add(guiparts);
		}
		this.guiData.scrollViewParts.Add(go, list);
	}

	// Token: 0x06001CD3 RID: 7379 RVA: 0x0016987C File Offset: 0x00167A7C
	private void OnUpdateItem(int index, GameObject go)
	{
		List<ItemPresetWindowCtrl.GUIParts> list = this.guiData.scrollViewParts[go];
		int num = this.currentPartsDataList.Count / 4 + 1;
		if (index > num)
		{
			go.SetActive(false);
			return;
		}
		go.SetActive(true);
		for (int i = 0; i < 4; i++)
		{
			int num2 = index * 4 + i;
			if (num2 < this.currentPartsDataList.Count && this.currentPartsDataList[num2].itemData.staticData != null)
			{
				list[i].baseObj.SetActive(true);
				list[i].iconItem.Setup(this.currentPartsDataList[num2].itemData.staticData, new IconItemCtrl.SetupParam
				{
					useInfo = true,
					useMaxDetail = this.isOpenByPurchase,
					viewItemCount = false
				});
				list[i].Num_Txt.text = this.currentPartsDataList[num2].itemData.num.ToString();
				list[i].Info_Txt.text = this.currentPartsDataList[num2].itemData.staticData.GetName();
				if (this.currentPartsDataList[num2].isOverMax)
				{
					list[i].Info_Txt_2.gameObject.SetActive(true);
				}
				else
				{
					list[i].Info_Txt_2.gameObject.SetActive(false);
				}
				list[i].Mark_Omake.SetActive(this.isOpenByPurchase && ItemPresetData.DispType.Hidden != this.currentPartsDataList[num2].dispType);
			}
			else
			{
				list[i].baseObj.SetActive(false);
			}
		}
	}

	// Token: 0x06001CD4 RID: 7380 RVA: 0x00169A40 File Offset: 0x00167C40
	private void SetupInternal()
	{
		this.guiData.fukidashiBase.SetActive(this.isOpenByPurchase);
		this.guiData.itemIconBase.SetActive(!this.isOpenByPurchase);
		this.guiData.purchaseIconBase.SetActive(this.isOpenByPurchase);
	}

	// Token: 0x06001CD5 RID: 7381 RVA: 0x00169A94 File Offset: 0x00167C94
	public void OpenByItem(ItemData itemData)
	{
		if (itemData.staticData.GetKind() != ItemDef.Kind.PRESET)
		{
			return;
		}
		this.guiData.baseObj.transform.Find("Base/Window/BtnOk").gameObject.SetActive(false);
		this.guiData.baseObj.transform.Find("Base/Window/BtnCancel").gameObject.SetActive(false);
		this.guiData.baseObj.transform.Find("Base/Window/PurchaseConfirmButton").gameObject.SetActive(false);
		this.guiData.baseObj.transform.Find("Base/Window/BtnClose").gameObject.SetActive(true);
		this.isOpenByPurchase = false;
		this.SetupInternal();
		this.guiData.presetIconItem.Setup(itemData.staticData);
		this.guiData.presetItemNum.text = itemData.num.ToString();
		this.guiData.presetItemName.text = itemData.staticData.GetName().Replace("\r", "").Replace("\n", "");
		this.guiData.presetItemInfo.text = itemData.staticData.GetInfo();
		this.guiData.owCtrl.Open();
		this.currentPartsDataList = (itemData.staticData as ItemPresetData).SetItemList.ConvertAll<ItemPresetWindowCtrl.PartsData>(delegate(ItemPresetData.Item item)
		{
			bool flag = false;
			if (item.itemId == DataManagerPhoto.PHOTO_STOCK_RELEASEITEM_ID && DataManagerItem.isOverUserHaveMaxNum(item.itemId, item.num))
			{
				flag = true;
			}
			return new ItemPresetWindowCtrl.PartsData(new ItemData(item.itemId, item.num), item.dispType, flag);
		});
		this.guiData.ScrollView.Resize(this.currentPartsDataList.Count / 4 + ((this.currentPartsDataList.Count % 4 != 0) ? 1 : 0), 0);
	}

	// Token: 0x06001CD6 RID: 7382 RVA: 0x00169C58 File Offset: 0x00167E58
	public void OpenByPurchase(PurchaseProductOne purchaseProductOne, Action onClickOKButton, Action onClickCancelButton, bool isApppayExists)
	{
		this.isOpenByPurchase = true;
		this.guiData.baseObj.transform.Find("Base/Window/BtnOk").gameObject.SetActive(true);
		this.guiData.baseObj.transform.Find("Base/Window/BtnCancel").gameObject.SetActive(true);
		this.guiData.baseObj.transform.Find("Base/Window/PurchaseConfirmButton").gameObject.SetActive(true);
		this.guiData.baseObj.transform.Find("Base/Window/BtnClose").gameObject.SetActive(false);
		bool flag = this.ChkDispItemSetting(purchaseProductOne);
		ItemData itemData = (flag ? purchaseProductOne.MainItem : purchaseProductOne.bonusItem);
		string text = (flag ? purchaseProductOne.MainItemIconOptionPath : purchaseProductOne.iconPath);
		this.SetupInternal();
		this.guiData.purchaseIcon.SetRawImage(text, true, false, null);
		this.guiData.apppayIconBase.SetActive(isApppayExists);
		this.guiData.presetItemName.text = purchaseProductOne.bonusItemTitle.Replace("\r", "").Replace("\n", "");
		this.guiData.fukidashiBase.SetActive(!string.IsNullOrEmpty(purchaseProductOne.limitText));
		this.guiData.fukidashiInfo.text = purchaseProductOne.limitText.Replace("\u3000", "\n");
		this.guiData.fukidashiBase.SetActive(!string.IsNullOrEmpty(purchaseProductOne.limitText));
		this.guiData.buyButton.AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			onClickOKButton();
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.purchaseConfirmButton.AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			CanvasManager.HdlPurchaseConfirmWindow.Initialize(purchaseProductOne);
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.owCtrl.Open();
		if (itemData != null)
		{
			this.guiData.presetItemInfo.text = itemData.staticData.GetInfo();
			if (itemData.staticData.GetKind() == ItemDef.Kind.PRESET)
			{
				this.currentPartsDataList = (itemData.staticData as ItemPresetData).SetItemList.ConvertAll<ItemPresetWindowCtrl.PartsData>((ItemPresetData.Item item) => new ItemPresetWindowCtrl.PartsData(new ItemData(item.itemId, item.num), item.dispType, false));
			}
			else
			{
				this.currentPartsDataList = new List<ItemPresetWindowCtrl.PartsData>
				{
					new ItemPresetWindowCtrl.PartsData(new ItemData(itemData.id, itemData.num), ItemPresetData.DispType.Invalid, false)
				};
			}
			if (this.currentPartsDataList.Count<ItemPresetWindowCtrl.PartsData>((ItemPresetWindowCtrl.PartsData parts) => parts.itemData.staticData.GetKind() == ItemDef.Kind.ACHIEVEMENT && DataManager.DmAchievement.GetHaveAchievementData(parts.itemData.id) != null) != 0)
			{
				PguiTextCtrl presetItemInfo = this.guiData.presetItemInfo;
				presetItemInfo.text += "\n<color=#ff0000>※所持している称号を含みます</color>";
			}
			string overMaxText = "";
			bool flag2 = false;
			foreach (ItemPresetWindowCtrl.PartsData partsData in this.currentPartsDataList)
			{
				if (partsData.itemData.id == DataManagerPhoto.PHOTO_STOCK_RELEASEITEM_ID && DataManagerItem.isOverUserHaveMaxNum(partsData.itemData.id, partsData.itemData.num))
				{
					flag2 = true;
					overMaxText = overMaxText + partsData.itemData.staticData.GetName() + "は所持枠解放の上限に達するため、\nこれ以上獲得できません。\n";
					partsData.isOverMax = true;
				}
			}
			if (flag2)
			{
				List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>> btnList = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>
				{
					new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.NEGATIVE, "キャンセル"),
					new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, "購入する")
				};
				PguiOpenWindowCtrl.Callback <>9__5;
				this.guiData.buyButton.AddOnClickListener(delegate(PguiButtonCtrl btn)
				{
					PguiOpenWindowCtrl hdlOpenWindowBasic = CanvasManager.HdlOpenWindowBasic;
					string text2 = "ご注意";
					string text3 = overMaxText + "\n\n本当によろしいですか？";
					List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>> btnList2 = btnList;
					bool flag3 = true;
					PguiOpenWindowCtrl.Callback callback;
					if ((callback = <>9__5) == null)
					{
						callback = (<>9__5 = delegate(int idx)
						{
							this.guiData.owCtrl.RegistCallback((int i) => true);
							if (idx == 1)
							{
								onClickOKButton();
								this.guiData.owCtrl.ForceClose();
							}
							return true;
						});
					}
					hdlOpenWindowBasic.Setup(text2, text3, btnList2, flag3, callback, null, false);
					CanvasManager.HdlOpenWindowBasic.Open();
					this.guiData.owCtrl.RegistCallback((int i) => false);
				}, PguiButtonCtrl.SoundType.DEFAULT);
			}
		}
		else
		{
			this.guiData.presetItemInfo.text = "";
			this.currentPartsDataList = new List<ItemPresetWindowCtrl.PartsData>();
		}
		if (!flag)
		{
			if (purchaseProductOne.freeNum > 0)
			{
				this.currentPartsDataList.Insert(0, new ItemPresetWindowCtrl.PartsData(new ItemData(30001, purchaseProductOne.freeNum), ItemPresetData.DispType.Invalid, false));
			}
			this.currentPartsDataList.Insert(0, new ItemPresetWindowCtrl.PartsData(new ItemData(30002, purchaseProductOne.chargeNum), ItemPresetData.DispType.Hidden, false));
		}
		this.guiData.ScrollView.Resize(this.currentPartsDataList.Count / 4 + ((this.currentPartsDataList.Count % 4 != 0) ? 1 : 0), 0);
	}

	// Token: 0x06001CD7 RID: 7383 RVA: 0x0016A130 File Offset: 0x00168330
	private bool ChkDispItemSetting(PurchaseProductOne purchaseProductOne)
	{
		return purchaseProductOne.chargeNum == 0 && purchaseProductOne.freeNum == 0 && purchaseProductOne.bonusItem == null && purchaseProductOne.MainItem != null && 0 < purchaseProductOne.MainItem.num && "Texture2D/Shop_BuyImg_Pack/" != purchaseProductOne.MainItemIconOptionPath;
	}

	// Token: 0x0400156C RID: 5484
	private ItemPresetWindowCtrl.GUI guiData;

	// Token: 0x0400156D RID: 5485
	private List<ItemPresetWindowCtrl.PartsData> currentPartsDataList;

	// Token: 0x0400156E RID: 5486
	private bool isOpenByPurchase;

	// Token: 0x02000F20 RID: 3872
	public class GUI
	{
		// Token: 0x06004EB4 RID: 20148 RVA: 0x002370CC File Offset: 0x002352CC
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.itemIconBase = baseTr.Find("Base/Window/Base_SetInfo/Buy_Img").gameObject;
			this.presetIconItem = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, this.itemIconBase.transform).GetComponent<IconItemCtrl>();
			this.presetIconItem.transform.SetAsFirstSibling();
			this.presetItemNumObj = baseTr.Find("Base/Window/Base_SetInfo/Buy_Img/Txt_Window/").gameObject;
			this.presetItemNum = baseTr.Find("Base/Window/Base_SetInfo/Buy_Img/Txt_Window/Num_Txt").GetComponent<PguiTextCtrl>();
			this.presetItemNumObj.SetActive(false);
			this.presetItemName = baseTr.Find("Base/Window/Base_SetInfo/Txt01").GetComponent<PguiTextCtrl>();
			this.presetItemInfo = baseTr.Find("Base/Window/Base_SetInfo/Txt02").GetComponent<PguiTextCtrl>();
			this.fukidashiBase = baseTr.Find("Base/Window/Base_SetInfo/tex_Fukidashi").gameObject;
			this.fukidashiInfo = this.fukidashiBase.transform.Find("Txt_BuyInfo").GetComponent<PguiTextCtrl>();
			this.purchaseIconBase = baseTr.Find("Base/Window/Base_SetInfo/Buy_Img_kira").gameObject;
			this.purchaseIcon = this.purchaseIconBase.GetComponent<PguiRawImageCtrl>();
			this.apppayIconBase = baseTr.Find("Base/Window/Base_SetInfo/ApppayBanner").gameObject;
			this.apppayIconBase.GetComponent<PguiTouchTrigger>().AddListener(delegate
			{
				CanvasManager.HdlCmnFeedPageWindowCtrl.transform.SetAsLastSibling();
				CanvasManager.HdlHelpWindowCtrl.Open(true);
			}, null, null, null, null);
			this.buyButton = baseTr.Find("Base/Window/BtnOk").GetComponent<PguiButtonCtrl>();
			this.purchaseConfirmButton = baseTr.Find("Base/Window/PurchaseConfirmButton").GetComponent<PguiButtonCtrl>();
			this.ScrollView = baseTr.Find("Base/Window/Base_SetInfo/InBase/ScrollView").GetComponent<ReuseScroll>();
		}

		// Token: 0x040055E3 RID: 21987
		public GameObject baseObj;

		// Token: 0x040055E4 RID: 21988
		public PguiOpenWindowCtrl owCtrl;

		// Token: 0x040055E5 RID: 21989
		public IconItemCtrl presetIconItem;

		// Token: 0x040055E6 RID: 21990
		public GameObject presetItemNumObj;

		// Token: 0x040055E7 RID: 21991
		public PguiTextCtrl presetItemNum;

		// Token: 0x040055E8 RID: 21992
		public PguiTextCtrl presetItemName;

		// Token: 0x040055E9 RID: 21993
		public PguiTextCtrl presetItemInfo;

		// Token: 0x040055EA RID: 21994
		public PguiTextCtrl fukidashiInfo;

		// Token: 0x040055EB RID: 21995
		public PguiRawImageCtrl purchaseIcon;

		// Token: 0x040055EC RID: 21996
		public GameObject itemIconBase;

		// Token: 0x040055ED RID: 21997
		public GameObject purchaseIconBase;

		// Token: 0x040055EE RID: 21998
		public GameObject apppayIconBase;

		// Token: 0x040055EF RID: 21999
		public GameObject fukidashiBase;

		// Token: 0x040055F0 RID: 22000
		public ReuseScroll ScrollView;

		// Token: 0x040055F1 RID: 22001
		public Dictionary<GameObject, List<ItemPresetWindowCtrl.GUIParts>> scrollViewParts = new Dictionary<GameObject, List<ItemPresetWindowCtrl.GUIParts>>();

		// Token: 0x040055F2 RID: 22002
		public PguiButtonCtrl buyButton;

		// Token: 0x040055F3 RID: 22003
		public PguiButtonCtrl purchaseConfirmButton;

		// Token: 0x040055F4 RID: 22004
		public PurchaseConfirmWindow purchaseConfirmWindow;
	}

	// Token: 0x02000F21 RID: 3873
	public class GUIParts
	{
		// Token: 0x06004EB5 RID: 20149 RVA: 0x0023729C File Offset: 0x0023549C
		public GUIParts(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.CmnShop_SetItem = baseTr.GetComponent<PguiButtonCtrl>();
			this.CmnShop_SetItem.m_Collider.enabled = false;
			this.Buy_Img_Stone = baseTr.Find("BaseImage/Buy_Img_Stone").GetComponent<PguiRawImageCtrl>();
			this.Buy_Img_Stone.m_RawImage.enabled = false;
			this.Num_Txt = baseTr.Find("BaseImage/Buy_Img_Stone/Txt_Window/Num_Txt").GetComponent<PguiTextCtrl>();
			this.Info_Txt = baseTr.Find("BaseImage/Txt_BuyInfo01").GetComponent<PguiTextCtrl>();
			this.Info_Txt_2 = baseTr.Find("BaseImage/Txt_BuyInfo02").GetComponent<PguiTextCtrl>();
			this.iconItem = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, baseTr.Find("BaseImage/Buy_Img_Stone").transform).GetComponent<IconItemCtrl>();
			this.iconItem.transform.SetAsFirstSibling();
			this.Mark_Omake = baseTr.Find("BaseImage/Mark_Omake").gameObject;
		}

		// Token: 0x040055F5 RID: 22005
		public const int NUM = 4;

		// Token: 0x040055F6 RID: 22006
		public GameObject baseObj;

		// Token: 0x040055F7 RID: 22007
		public PguiButtonCtrl CmnShop_SetItem;

		// Token: 0x040055F8 RID: 22008
		public PguiRawImageCtrl Buy_Img_Stone;

		// Token: 0x040055F9 RID: 22009
		public PguiTextCtrl Num_Txt;

		// Token: 0x040055FA RID: 22010
		public PguiTextCtrl Info_Txt;

		// Token: 0x040055FB RID: 22011
		public PguiTextCtrl Info_Txt_2;

		// Token: 0x040055FC RID: 22012
		public IconItemCtrl iconItem;

		// Token: 0x040055FD RID: 22013
		public GameObject Mark_Omake;
	}

	// Token: 0x02000F22 RID: 3874
	private class PartsData
	{
		// Token: 0x06004EB6 RID: 20150 RVA: 0x00237391 File Offset: 0x00235591
		public PartsData(ItemData data, ItemPresetData.DispType type, bool overMax = false)
		{
			this.itemData = data;
			this.dispType = type;
			this.isOverMax = overMax;
		}

		// Token: 0x040055FE RID: 22014
		public ItemData itemData;

		// Token: 0x040055FF RID: 22015
		public ItemPresetData.DispType dispType;

		// Token: 0x04005600 RID: 22016
		public bool isOverMax;
	}
}
