using System;
using System.Collections.Generic;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.UI;

public class GetAchievementWindowCtrl : MonoBehaviour
{
	public void Init(Transform baseTr)
	{
		this.windowGuiData = new GetAchievementWindowCtrl.WindowGUI(baseTr);
		this.windowGuiData.reuseScroll.InitForce();
		ReuseScroll reuseScroll = this.windowGuiData.reuseScroll;
		reuseScroll.onStartItem = (Action<int, GameObject>)Delegate.Combine(reuseScroll.onStartItem, new Action<int, GameObject>(this.OnStartItem));
		ReuseScroll reuseScroll2 = this.windowGuiData.reuseScroll;
		reuseScroll2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(reuseScroll2.onUpdateItem, new Action<int, GameObject>(this.OnUpdateItem));
		this.gridGUIMap = new Dictionary<int, GetAchievementWindowCtrl.GridGUI>();
		this.fontSize = this.windowGuiData.openWindowCtrl.MassageText.m_Text.fontSize;
	}

	public void Setup(List<DataManagerAchievement.AchievementStaticData> itemList, GetAchievementWindowCtrl.SetupParam paramIn, int fsiz = 0)
	{
		this.windowClosedCallback = paramIn.callBack;
		this.windowGuiData.WindowSettings(paramIn);
		this.windowGuiData.openWindowCtrl.MassageText.m_Text.fontSize = ((fsiz > 0) ? fsiz : this.fontSize);
		this.dispItemList = itemList;
		int num = this.dispItemList.Count / 2 + ((this.dispItemList.Count % 2 != 0) ? 1 : 0);
		foreach (GetAchievementWindowCtrl.GridGUI gridGUI in this.gridGUIMap.Values)
		{
			foreach (AchievementCtrl achievementCtrl in gridGUI.iconItemList)
			{
				achievementCtrl.gameObject.SetActive(false);
			}
		}
		if (this.gridGUIMap.Count == 0)
		{
			this.windowGuiData.reuseScroll.Setup(num, 0);
		}
		else
		{
			this.windowGuiData.reuseScroll.Resize(num, 0);
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
		GetAchievementWindowCtrl.GridGUI gridGUI;
		if (!this.gridGUIMap.ContainsKey(index))
		{
			gridGUI = new GetAchievementWindowCtrl.GridGUI(go);
			this.columnCount = gridGUI.gridLayoutGroup.constraintCount;
			gridGUI.iconItemList = new List<AchievementCtrl>();
			for (int i = 0; i < this.columnCount; i++)
			{
				GameObject gameObject = new GameObject();
				gameObject.name = "iconBase";
				gameObject.AddComponent<RectTransform>();
				gameObject.transform.SetParent(gridGUI.baseObj.transform, false);
				GameObject gameObject2 = Object.Instantiate<GameObject>((GameObject)Resources.Load("CmnIconFrame/GUI/Prefab/Achievement"), gameObject.transform);
				gameObject2.name = "ItemIcon" + i.ToString("D2");
				AchievementCtrl component = gameObject2.GetComponent<AchievementCtrl>();
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
		this.ItemSetup(index, new GetAchievementWindowCtrl.GridGUI(go));
	}

	private void ItemSetup(int index, GetAchievementWindowCtrl.GridGUI gridGUI)
	{
		int num = 0;
		foreach (AchievementCtrl achievementCtrl in gridGUI.iconItemList)
		{
			int num2 = this.columnCount * index + num;
			if (this.dispItemList.Count <= num2)
			{
				achievementCtrl.gameObject.SetActive(false);
			}
			else
			{
				achievementCtrl.gameObject.SetActive(true);
				achievementCtrl.Setup(this.dispItemList[num2].id, false, false);
				achievementCtrl.HideBadge(this.dispItemList[num2].id);
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

	public GetAchievementWindowCtrl.WindowGUI windowGuiData;

	public PguiOpenWindowCtrl.Callback windowClosedCallback;

	private List<DataManagerAchievement.AchievementStaticData> dispItemList;

	private int columnCount;

	private Dictionary<int, GetAchievementWindowCtrl.GridGUI> gridGUIMap;

	private int fontSize;

	public class SetupParam
	{
		public string titleText;

		public string messageText;

		public string innerTitleText;

		public PguiOpenWindowCtrl.Callback callBack;

		public HashSet<int> dispNewItemIdSet = new HashSet<int>();
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
			null == this.reuseScroll;
		}

		public void WindowSettings(GetAchievementWindowCtrl.SetupParam setupParam)
		{
			this.InnerTitleText.text = setupParam.innerTitleText;
		}

		public PguiOpenWindowCtrl openWindowCtrl;

		public GameObject baseObj;

		public PguiTextCtrl InnerTitleText;

		public GameObject contentObject;

		public ReuseScroll reuseScroll;
	}

	public class GridGUI
	{
		public GridGUI(GameObject go)
		{
			this.baseObj = go;
			this.gridLayoutGroup = this.baseObj.GetComponent<GridLayoutGroup>();
			this.iconItemList = new List<AchievementCtrl>();
			for (int i = 0; i < this.gridLayoutGroup.constraintCount; i++)
			{
				Transform transform = go.transform.Find("iconBase/ItemIcon" + i.ToString("D2"));
				if (null == transform)
				{
					break;
				}
				this.iconItemList.Add(transform.GetComponent<AchievementCtrl>());
			}
		}

		public GameObject baseObj;

		public GridLayoutGroup gridLayoutGroup;

		public List<AchievementCtrl> iconItemList;
	}
}
