using System;
using System.Collections.Generic;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001A0 RID: 416
public class GetAchievementWindowCtrl : MonoBehaviour
{
	// Token: 0x06001BCA RID: 7114 RVA: 0x00161CFC File Offset: 0x0015FEFC
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

	// Token: 0x06001BCB RID: 7115 RVA: 0x00161DA8 File Offset: 0x0015FFA8
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

	// Token: 0x06001BCC RID: 7116 RVA: 0x00161F3C File Offset: 0x0016013C
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

	// Token: 0x06001BCD RID: 7117 RVA: 0x0016202C File Offset: 0x0016022C
	public void OnUpdateItem(int index, GameObject go)
	{
		this.ItemSetup(index, new GetAchievementWindowCtrl.GridGUI(go));
	}

	// Token: 0x06001BCE RID: 7118 RVA: 0x0016203C File Offset: 0x0016023C
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

	// Token: 0x06001BCF RID: 7119 RVA: 0x001620F4 File Offset: 0x001602F4
	public void Open()
	{
		this.windowGuiData.openWindowCtrl.Open();
	}

	// Token: 0x06001BD0 RID: 7120 RVA: 0x00162106 File Offset: 0x00160306
	private void OnClickButton(int index)
	{
	}

	// Token: 0x040014B2 RID: 5298
	public GetAchievementWindowCtrl.WindowGUI windowGuiData;

	// Token: 0x040014B3 RID: 5299
	public PguiOpenWindowCtrl.Callback windowClosedCallback;

	// Token: 0x040014B4 RID: 5300
	private List<DataManagerAchievement.AchievementStaticData> dispItemList;

	// Token: 0x040014B5 RID: 5301
	private int columnCount;

	// Token: 0x040014B6 RID: 5302
	private Dictionary<int, GetAchievementWindowCtrl.GridGUI> gridGUIMap;

	// Token: 0x040014B7 RID: 5303
	private int fontSize;

	// Token: 0x02000EDC RID: 3804
	public class SetupParam
	{
		// Token: 0x040054FB RID: 21755
		public string titleText;

		// Token: 0x040054FC RID: 21756
		public string messageText;

		// Token: 0x040054FD RID: 21757
		public string innerTitleText;

		// Token: 0x040054FE RID: 21758
		public PguiOpenWindowCtrl.Callback callBack;

		// Token: 0x040054FF RID: 21759
		public HashSet<int> dispNewItemIdSet = new HashSet<int>();
	}

	// Token: 0x02000EDD RID: 3805
	public class WindowGUI
	{
		// Token: 0x06004DF8 RID: 19960 RVA: 0x00234DB0 File Offset: 0x00232FB0
		public WindowGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.InnerTitleText = baseTr.Find("Base/Window/GetItemInfo/TitleBase/Text").GetComponent<PguiTextCtrl>();
			this.contentObject = baseTr.Find("Base/Window/GetItemInfo/ScrollView/Viewport/Content").gameObject;
			this.openWindowCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.reuseScroll = baseTr.Find("Base/Window/GetItemInfo/ScrollView/").GetComponent<ReuseScroll>();
			null == this.reuseScroll;
		}

		// Token: 0x06004DF9 RID: 19961 RVA: 0x00234E2A File Offset: 0x0023302A
		public void WindowSettings(GetAchievementWindowCtrl.SetupParam setupParam)
		{
			this.InnerTitleText.text = setupParam.innerTitleText;
		}

		// Token: 0x04005500 RID: 21760
		public PguiOpenWindowCtrl openWindowCtrl;

		// Token: 0x04005501 RID: 21761
		public GameObject baseObj;

		// Token: 0x04005502 RID: 21762
		public PguiTextCtrl InnerTitleText;

		// Token: 0x04005503 RID: 21763
		public GameObject contentObject;

		// Token: 0x04005504 RID: 21764
		public ReuseScroll reuseScroll;
	}

	// Token: 0x02000EDE RID: 3806
	public class GridGUI
	{
		// Token: 0x06004DFA RID: 19962 RVA: 0x00234E40 File Offset: 0x00233040
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

		// Token: 0x04005505 RID: 21765
		public GameObject baseObj;

		// Token: 0x04005506 RID: 21766
		public GridLayoutGroup gridLayoutGroup;

		// Token: 0x04005507 RID: 21767
		public List<AchievementCtrl> iconItemList;
	}
}
