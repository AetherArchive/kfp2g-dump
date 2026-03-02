using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200018B RID: 395
public class AccessorySortWindowCtrl : MonoBehaviour
{
	// Token: 0x06001A63 RID: 6755 RVA: 0x001567A9 File Offset: 0x001549A9
	public void Initialize(GameObject go)
	{
		this.winGUI = new AccessorySortWindowCtrl.WindowGUI(go);
	}

	// Token: 0x06001A64 RID: 6756 RVA: 0x001567B7 File Offset: 0x001549B7
	public void Open(SortFilterDefine.RegisterType registerType, UnityAction<bool, SortFilterDefine.SortType> closeCallBack)
	{
		if (registerType - SortFilterDefine.RegisterType.ACCESSORY_ALL > 4)
		{
			return;
		}
		this.winGUI.Setup(registerType, closeCallBack);
		this.winGUI.Open();
	}

	// Token: 0x0400142F RID: 5167
	private AccessorySortWindowCtrl.WindowGUI winGUI;

	// Token: 0x02000E71 RID: 3697
	public class WindowGUI
	{
		// Token: 0x17000B2E RID: 2862
		// (get) Token: 0x06004CA8 RID: 19624 RVA: 0x0022DF1A File Offset: 0x0022C11A
		// (set) Token: 0x06004CA9 RID: 19625 RVA: 0x0022DF22 File Offset: 0x0022C122
		private List<AccessorySortWindowCtrl.WindowGUI.SortButton> ButtonList { get; set; }

		// Token: 0x17000B2F RID: 2863
		// (get) Token: 0x06004CAA RID: 19626 RVA: 0x0022DF2B File Offset: 0x0022C12B
		// (set) Token: 0x06004CAB RID: 19627 RVA: 0x0022DF33 File Offset: 0x0022C133
		private List<PguiToggleButtonCtrl> Btn_EventList { get; set; }

		// Token: 0x17000B30 RID: 2864
		// (get) Token: 0x06004CAC RID: 19628 RVA: 0x0022DF3C File Offset: 0x0022C13C
		// (set) Token: 0x06004CAD RID: 19629 RVA: 0x0022DF44 File Offset: 0x0022C144
		private UnityAction<bool, SortFilterDefine.SortType> CloseCallBack { get; set; }

		// Token: 0x06004CAE RID: 19630 RVA: 0x0022DF50 File Offset: 0x0022C150
		public WindowGUI(GameObject go)
		{
			this.baseObj = go;
			Transform transform = this.baseObj.transform;
			this.baseWindow = transform.GetComponent<PguiOpenWindowCtrl>();
			this.ButtonList = new List<AccessorySortWindowCtrl.WindowGUI.SortButton>
			{
				new AccessorySortWindowCtrl.WindowGUI.SortButton(transform.Find("Base/Window/Sort/Btn01").gameObject),
				new AccessorySortWindowCtrl.WindowGUI.SortButton(transform.Find("Base/Window/Sort/Btn02").gameObject),
				new AccessorySortWindowCtrl.WindowGUI.SortButton(transform.Find("Base/Window/Sort/Btn03").gameObject),
				new AccessorySortWindowCtrl.WindowGUI.SortButton(transform.Find("Base/Window/Sort/Btn04").gameObject),
				new AccessorySortWindowCtrl.WindowGUI.SortButton(transform.Find("Base/Window/Sort/Btn05").gameObject),
				new AccessorySortWindowCtrl.WindowGUI.SortButton(transform.Find("Base/Window/Sort/Btn06").gameObject),
				new AccessorySortWindowCtrl.WindowGUI.SortButton(transform.Find("Base/Window/Sort/Btn07").gameObject),
				new AccessorySortWindowCtrl.WindowGUI.SortButton(transform.Find("Base/Window/Sort/Btn08").gameObject),
				new AccessorySortWindowCtrl.WindowGUI.SortButton(transform.Find("Base/Window/Sort/Btn09").gameObject),
				new AccessorySortWindowCtrl.WindowGUI.SortButton(transform.Find("Base/Window/Sort/Btn10").gameObject),
				new AccessorySortWindowCtrl.WindowGUI.SortButton(transform.Find("Base/Window/Sort/Btn11").gameObject),
				new AccessorySortWindowCtrl.WindowGUI.SortButton(transform.Find("Base/Window/Sort/Btn12").gameObject),
				new AccessorySortWindowCtrl.WindowGUI.SortButton(transform.Find("Base/Window/Sort/Btn13").gameObject),
				new AccessorySortWindowCtrl.WindowGUI.SortButton(transform.Find("Base/Window/Sort/Btn14").gameObject),
				new AccessorySortWindowCtrl.WindowGUI.SortButton(transform.Find("Base/Window/Sort/Btn15").gameObject),
				new AccessorySortWindowCtrl.WindowGUI.SortButton(transform.Find("Base/Window/Sort/Btn16").gameObject)
			};
			foreach (AccessorySortWindowCtrl.WindowGUI.SortButton sortButton in this.ButtonList)
			{
				sortButton.Button.AddOnClickListener((PguiToggleButtonCtrl ptb, int idx) => this.OnClickToggleButton(ptb));
			}
			this.Btn_EventList = new List<PguiToggleButtonCtrl> { transform.Find("Base/Window/Sort/Btn_Event01").GetComponent<PguiToggleButtonCtrl>() };
			foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl in this.Btn_EventList)
			{
				pguiToggleButtonCtrl.gameObject.SetActive(false);
			}
			this.baseWindow.Setup("ソート順の設定", "並び順の基準とする条件を設定できます", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, new PguiOpenWindowCtrl.Callback(this.OnClickWindowButton), new UnityAction(this.WindowDestroy), false);
		}

		// Token: 0x06004CAF RID: 19631 RVA: 0x0022E22C File Offset: 0x0022C42C
		public void Setup(SortFilterDefine.RegisterType registerType, UnityAction<bool, SortFilterDefine.SortType> closeCallBack)
		{
			foreach (AccessorySortWindowCtrl.WindowGUI.SortButton sortButton in this.ButtonList)
			{
				sortButton.baseObj.SetActive(false);
				sortButton.SortType = SortFilterDefine.SortType.INVALID;
			}
			SortFilterManager.Accessory.LatestSortWindowRegisterType = registerType;
			SortFilterDefine.SortType sortType = SortFilterManager.GetActiveSortTypeData(SortFilterManager.Accessory.LatestSortWindowRegisterType).SortType;
			List<SortFilterDefine.SortType> accessorySortTypeList = SortFilterDefine.AccessorySortTypeList;
			int num = 0;
			foreach (SortFilterDefine.SortType sortType2 in accessorySortTypeList)
			{
				this.ButtonList[num].baseObj.SetActive(true);
				this.ButtonList[num].Name.text = SortFilterDefine.SortTypeDispNameMap[sortType2];
				this.ButtonList[num].SortType = sortType2;
				if (sortType2 == sortType)
				{
					this.ButtonList[num].Button.SetToggleIndex(1);
				}
				else
				{
					this.ButtonList[num].Button.SetToggleIndex(0);
				}
				num++;
			}
			SortFilterManager.Accessory.LatestSortWindowSortType = sortType;
			this.CloseCallBack = closeCallBack;
		}

		// Token: 0x06004CB0 RID: 19632 RVA: 0x0022E384 File Offset: 0x0022C584
		public void Open()
		{
			this.baseWindow.Open();
		}

		// Token: 0x06004CB1 RID: 19633 RVA: 0x0022E394 File Offset: 0x0022C594
		private bool OnClickWindowButton(int index)
		{
			AccessorySortWindowCtrl.WindowGUI.SortButton sortButton = this.ButtonList.Find((AccessorySortWindowCtrl.WindowGUI.SortButton x) => 1 == x.Button.GetToggleIndex());
			this.LatestChanged = false;
			if (index == 1 && SortFilterManager.Accessory.LatestSortWindowSortType != sortButton.SortType)
			{
				this.LatestChanged = true;
				SortFilterManager.Accessory.LatestSortWindowSortType = sortButton.SortType;
			}
			UnityAction<bool, SortFilterDefine.SortType> closeCallBack = this.CloseCallBack;
			if (closeCallBack != null)
			{
				closeCallBack(this.LatestChanged, SortFilterManager.Accessory.LatestSortWindowSortType);
			}
			return true;
		}

		// Token: 0x06004CB2 RID: 19634 RVA: 0x0022E422 File Offset: 0x0022C622
		public void WindowDestroy()
		{
			Object.Destroy(this.baseObj);
		}

		// Token: 0x06004CB3 RID: 19635 RVA: 0x0022E430 File Offset: 0x0022C630
		private bool OnClickToggleButton(PguiToggleButtonCtrl tgbtn)
		{
			if (1 == tgbtn.GetToggleIndex())
			{
				return false;
			}
			foreach (AccessorySortWindowCtrl.WindowGUI.SortButton sortButton in this.ButtonList)
			{
				sortButton.Button.SetToggleIndex(0);
			}
			return true;
		}

		// Token: 0x04005314 RID: 21268
		private GameObject baseObj;

		// Token: 0x04005317 RID: 21271
		private PguiOpenWindowCtrl baseWindow;

		// Token: 0x04005319 RID: 21273
		private bool LatestChanged;

		// Token: 0x020011D8 RID: 4568
		private class SortButton
		{
			// Token: 0x17000D05 RID: 3333
			// (get) Token: 0x0600573C RID: 22332 RVA: 0x002566FA File Offset: 0x002548FA
			// (set) Token: 0x0600573D RID: 22333 RVA: 0x00256702 File Offset: 0x00254902
			public PguiToggleButtonCtrl Button { get; private set; }

			// Token: 0x17000D06 RID: 3334
			// (get) Token: 0x0600573E RID: 22334 RVA: 0x0025670B File Offset: 0x0025490B
			// (set) Token: 0x0600573F RID: 22335 RVA: 0x00256713 File Offset: 0x00254913
			public PguiTextCtrl Name { get; private set; }

			// Token: 0x17000D07 RID: 3335
			// (get) Token: 0x06005740 RID: 22336 RVA: 0x0025671C File Offset: 0x0025491C
			// (set) Token: 0x06005741 RID: 22337 RVA: 0x00256724 File Offset: 0x00254924
			public SortFilterDefine.SortType SortType { get; set; }

			// Token: 0x06005742 RID: 22338 RVA: 0x0025672D File Offset: 0x0025492D
			public SortButton(GameObject go)
			{
				this.baseObj = go;
				this.Button = go.GetComponent<PguiToggleButtonCtrl>();
				this.Name = go.transform.Find("Num_Txt").GetComponent<PguiTextCtrl>();
				this.SortType = SortFilterDefine.SortType.INVALID;
			}

			// Token: 0x040061E0 RID: 25056
			public GameObject baseObj;
		}
	}
}
