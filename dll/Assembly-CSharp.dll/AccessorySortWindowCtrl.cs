using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AccessorySortWindowCtrl : MonoBehaviour
{
	public void Initialize(GameObject go)
	{
		this.winGUI = new AccessorySortWindowCtrl.WindowGUI(go);
	}

	public void Open(SortFilterDefine.RegisterType registerType, UnityAction<bool, SortFilterDefine.SortType> closeCallBack)
	{
		if (registerType - SortFilterDefine.RegisterType.ACCESSORY_ALL > 4)
		{
			return;
		}
		this.winGUI.Setup(registerType, closeCallBack);
		this.winGUI.Open();
	}

	private AccessorySortWindowCtrl.WindowGUI winGUI;

	public class WindowGUI
	{
		private List<AccessorySortWindowCtrl.WindowGUI.SortButton> ButtonList { get; set; }

		private List<PguiToggleButtonCtrl> Btn_EventList { get; set; }

		private UnityAction<bool, SortFilterDefine.SortType> CloseCallBack { get; set; }

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

		public void Open()
		{
			this.baseWindow.Open();
		}

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

		public void WindowDestroy()
		{
			Object.Destroy(this.baseObj);
		}

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

		private GameObject baseObj;

		private PguiOpenWindowCtrl baseWindow;

		private bool LatestChanged;

		private class SortButton
		{
			public PguiToggleButtonCtrl Button { get; private set; }

			public PguiTextCtrl Name { get; private set; }

			public SortFilterDefine.SortType SortType { get; set; }

			public SortButton(GameObject go)
			{
				this.baseObj = go;
				this.Button = go.GetComponent<PguiToggleButtonCtrl>();
				this.Name = go.transform.Find("Num_Txt").GetComponent<PguiTextCtrl>();
				this.SortType = SortFilterDefine.SortType.INVALID;
			}

			public GameObject baseObj;
		}
	}
}
