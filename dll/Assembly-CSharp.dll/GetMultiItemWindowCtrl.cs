using System;
using System.Collections.Generic;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.UI;

public class GetMultiItemWindowCtrl : MonoBehaviour
{
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

	public void OnUpdateItem(int index, GameObject go)
	{
		this.ItemSetup(index, new GetMultiItemWindowCtrl.GridGUI(go));
	}

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

	public void Open()
	{
		this.windowGuiData.openWindowCtrl.Open();
	}

	private void OnClickButton(int index)
	{
	}

	public GetMultiItemWindowCtrl.WindowGUI windowGuiData;

	public PguiOpenWindowCtrl.Callback windowClosedCallback;

	private List<ItemData> dispItemList;

	private List<int> dispNewindexList = new List<int>();

	private int columnCount;

	private Dictionary<int, GetMultiItemWindowCtrl.GridGUI> gridGUIMap;

	private int fontSize;

	public class SetupParam
	{
		public string titleText;

		public string messageText;

		public string innerTitleText;

		public PguiOpenWindowCtrl.Callback callBack;

		public HashSet<int> dispNewItemIdSet = new HashSet<int>();

		public string buyBeforeMoney;

		public string buyAfterMoney;

		public string iconName;
	}

	public class WindowGUI
	{
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

		public void WindowSettings(GetMultiItemWindowCtrl.SetupParam setupParam)
		{
			this.InnerTitleText.text = setupParam.innerTitleText;
		}

		public PguiOpenWindowCtrl openWindowCtrl;

		public GameObject baseObj;

		public PguiTextCtrl InnerTitleText;

		public GameObject contentObject;

		public ReuseScroll reuseScroll;

		public GameObject ItemUseCoin;

		public PguiTextCtrl Txt_BuyBeforeMoney;

		public PguiTextCtrl Txt_BuyAfterMoney;

		public PguiRawImageCtrl UseMoneyImage;
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
