using System;
using System.Collections.Generic;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001A2 RID: 418
public class GetMultiItemWindowCtrl : MonoBehaviour
{
	// Token: 0x06001BDB RID: 7131 RVA: 0x00162530 File Offset: 0x00160730
	public void Init(Transform baseTr)
	{
		this.windowGuiData = new GetMultiItemWindowCtrl.WindowGUI(baseTr);
		this.windowGuiData.reuseScroll.InitForce();
		ReuseScroll reuseScroll = this.windowGuiData.reuseScroll;
		reuseScroll.onStartItem = (Action<int, GameObject>)Delegate.Combine(reuseScroll.onStartItem, new Action<int, GameObject>(this.OnStartItem));
		ReuseScroll reuseScroll2 = this.windowGuiData.reuseScroll;
		reuseScroll2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(reuseScroll2.onUpdateItem, new Action<int, GameObject>(this.OnUpdateItem));
		this.gridGUIMap = new Dictionary<int, GetMultiItemWindowCtrl.GridGUI>();
		this.fontSize = this.windowGuiData.openWindowCtrl.MassageText.m_Text.fontSize;
	}

	// Token: 0x06001BDC RID: 7132 RVA: 0x001625DC File Offset: 0x001607DC
	public void Setup(List<ItemData> itemList, GetMultiItemWindowCtrl.SetupParam paramIn, bool showMoney = false, int fsiz = 0)
	{
		this.windowClosedCallback = paramIn.callBack;
		this.windowGuiData.WindowSettings(paramIn);
		this.windowGuiData.openWindowCtrl.MassageText.m_Text.fontSize = ((fsiz > 0) ? fsiz : this.fontSize);
		this.windowGuiData.ItemUseCoin.SetActive(showMoney);
		if (showMoney)
		{
			this.windowGuiData.Txt_BuyBeforeMoney.text = paramIn.buyBeforeMoney.ToString();
			this.windowGuiData.Txt_BuyAfterMoney.text = paramIn.buyAfterMoney.ToString();
			this.windowGuiData.UseMoneyImage.SetRawImage(paramIn.iconName, true, false, null);
		}
		this.dispItemList = itemList;
		this.dispNewindexList.Clear();
		if (paramIn != null && paramIn.dispNewItemIdSet != null && 0 < paramIn.dispNewItemIdSet.Count)
		{
			using (HashSet<int>.Enumerator enumerator = paramIn.dispNewItemIdSet.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int id = enumerator.Current;
					int num = this.dispItemList.FindIndex((ItemData x) => x.id == id);
					if (0 <= num)
					{
						this.dispNewindexList.Add(num);
					}
				}
			}
		}
		int num2 = this.dispItemList.Count / 5 + ((this.dispItemList.Count % 5 != 0) ? 1 : 0);
		foreach (GetMultiItemWindowCtrl.GridGUI gridGUI in this.gridGUIMap.Values)
		{
			foreach (IconItemCtrl iconItemCtrl in gridGUI.iconItemList)
			{
				iconItemCtrl.gameObject.SetActive(false);
			}
		}
		if (this.gridGUIMap.Count == 0)
		{
			this.windowGuiData.reuseScroll.Setup(num2, 0);
		}
		else
		{
			this.windowGuiData.reuseScroll.Resize(num2, 0);
		}
		this.windowGuiData.openWindowCtrl.Setup(paramIn.titleText, paramIn.messageText, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int index)
		{
			this.OnClickButton(index);
			return true;
		}, delegate
		{
			PguiOpenWindowCtrl.Callback callBack = paramIn.callBack;
			if (callBack == null)
			{
				return;
			}
			callBack(0);
		}, false);
	}

	// Token: 0x06001BDD RID: 7133 RVA: 0x00162894 File Offset: 0x00160A94
	public void OnStartItem(int index, GameObject go)
	{
		GetMultiItemWindowCtrl.GridGUI gridGUI;
		if (!this.gridGUIMap.ContainsKey(index))
		{
			gridGUI = new GetMultiItemWindowCtrl.GridGUI(go);
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

	// Token: 0x06001BDE RID: 7134 RVA: 0x0016297C File Offset: 0x00160B7C
	public void OnUpdateItem(int index, GameObject go)
	{
		this.ItemSetup(index, new GetMultiItemWindowCtrl.GridGUI(go));
	}

	// Token: 0x06001BDF RID: 7135 RVA: 0x0016298C File Offset: 0x00160B8C
	private void ItemSetup(int index, GetMultiItemWindowCtrl.GridGUI gridGUI)
	{
		int num = 0;
		foreach (IconItemCtrl iconItemCtrl in gridGUI.iconItemList)
		{
			int num2 = this.columnCount * index + num;
			if (this.dispItemList.Count <= num2)
			{
				iconItemCtrl.gameObject.SetActive(false);
			}
			else
			{
				iconItemCtrl.gameObject.SetActive(true);
				iconItemCtrl.Setup(DataManager.DmItem.GetItemStaticBase(this.dispItemList[num2].id), this.dispItemList[num2].num, new IconItemCtrl.SetupParam
				{
					useInfo = true,
					viewItemCount = true
				});
				if (this.dispNewindexList.Contains(num2))
				{
					iconItemCtrl.DispNew(true);
				}
				num++;
			}
		}
	}

	// Token: 0x06001BE0 RID: 7136 RVA: 0x00162A74 File Offset: 0x00160C74
	public void Open()
	{
		this.windowGuiData.openWindowCtrl.Open();
	}

	// Token: 0x06001BE1 RID: 7137 RVA: 0x00162A86 File Offset: 0x00160C86
	private void OnClickButton(int index)
	{
	}

	// Token: 0x040014BE RID: 5310
	public GetMultiItemWindowCtrl.WindowGUI windowGuiData;

	// Token: 0x040014BF RID: 5311
	public PguiOpenWindowCtrl.Callback windowClosedCallback;

	// Token: 0x040014C0 RID: 5312
	private List<ItemData> dispItemList;

	// Token: 0x040014C1 RID: 5313
	private List<int> dispNewindexList = new List<int>();

	// Token: 0x040014C2 RID: 5314
	private int columnCount;

	// Token: 0x040014C3 RID: 5315
	private Dictionary<int, GetMultiItemWindowCtrl.GridGUI> gridGUIMap;

	// Token: 0x040014C4 RID: 5316
	private int fontSize;

	// Token: 0x02000EE6 RID: 3814
	public class SetupParam
	{
		// Token: 0x04005520 RID: 21792
		public string titleText;

		// Token: 0x04005521 RID: 21793
		public string messageText;

		// Token: 0x04005522 RID: 21794
		public string innerTitleText;

		// Token: 0x04005523 RID: 21795
		public PguiOpenWindowCtrl.Callback callBack;

		// Token: 0x04005524 RID: 21796
		public HashSet<int> dispNewItemIdSet = new HashSet<int>();

		// Token: 0x04005525 RID: 21797
		public string buyBeforeMoney;

		// Token: 0x04005526 RID: 21798
		public string buyAfterMoney;

		// Token: 0x04005527 RID: 21799
		public string iconName;
	}

	// Token: 0x02000EE7 RID: 3815
	public class WindowGUI
	{
		// Token: 0x06004E10 RID: 19984 RVA: 0x002350DC File Offset: 0x002332DC
		public WindowGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.InnerTitleText = baseTr.Find("Base/Window/GetItemInfo/TitleBase/Text").GetComponent<PguiTextCtrl>();
			this.contentObject = baseTr.Find("Base/Window/GetItemInfo/ScrollView/Viewport/Content").gameObject;
			this.openWindowCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.reuseScroll = baseTr.Find("Base/Window/GetItemInfo/ScrollView/").GetComponent<ReuseScroll>();
			this.ItemUseCoin = baseTr.Find("Base/Window/Parts_ItemUseCoin").gameObject;
			this.Txt_BuyBeforeMoney = baseTr.Find("Base/Window/Parts_ItemUseCoin/Num_BeforeTxt").GetComponent<PguiTextCtrl>();
			this.Txt_BuyAfterMoney = baseTr.Find("Base/Window/Parts_ItemUseCoin/Num_AfterTxt").GetComponent<PguiTextCtrl>();
			this.UseMoneyImage = baseTr.Find("Base/Window/Parts_ItemUseCoin/Icon_Tex").GetComponent<PguiRawImageCtrl>();
			null == this.reuseScroll;
		}

		// Token: 0x06004E11 RID: 19985 RVA: 0x002351AE File Offset: 0x002333AE
		public void WindowSettings(GetMultiItemWindowCtrl.SetupParam setupParam)
		{
			this.InnerTitleText.text = setupParam.innerTitleText;
		}

		// Token: 0x04005528 RID: 21800
		public PguiOpenWindowCtrl openWindowCtrl;

		// Token: 0x04005529 RID: 21801
		public GameObject baseObj;

		// Token: 0x0400552A RID: 21802
		public PguiTextCtrl InnerTitleText;

		// Token: 0x0400552B RID: 21803
		public GameObject contentObject;

		// Token: 0x0400552C RID: 21804
		public ReuseScroll reuseScroll;

		// Token: 0x0400552D RID: 21805
		public GameObject ItemUseCoin;

		// Token: 0x0400552E RID: 21806
		public PguiTextCtrl Txt_BuyBeforeMoney;

		// Token: 0x0400552F RID: 21807
		public PguiTextCtrl Txt_BuyAfterMoney;

		// Token: 0x04005530 RID: 21808
		public PguiRawImageCtrl UseMoneyImage;
	}

	// Token: 0x02000EE8 RID: 3816
	public class GridGUI
	{
		// Token: 0x06004E12 RID: 19986 RVA: 0x002351C4 File Offset: 0x002333C4
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

		// Token: 0x04005531 RID: 21809
		public GameObject baseObj;

		// Token: 0x04005532 RID: 21810
		public GridLayoutGroup gridLayoutGroup;

		// Token: 0x04005533 RID: 21811
		public List<IconItemCtrl> iconItemList;
	}
}
