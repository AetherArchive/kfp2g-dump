using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AccessoryFilterWindowCtrl : MonoBehaviour
{
	public void Initialize(GameObject go)
	{
		this.winGUI = new AccessoryFilterWindowCtrl.WindowGUI(go);
	}

	public void Open(SortFilterDefine.RegisterType registerType, UnityAction<bool, AccessorySortFilter.FilterStatus> closeCallBack)
	{
		if (registerType != SortFilterDefine.RegisterType.ACCESSORY_ALL)
		{
			if (registerType - SortFilterDefine.RegisterType.ACCESSORY_GROW_BASE > 3)
			{
				return;
			}
		}
		else
		{
			this.winGUI.SetupIsAllWindow(registerType);
		}
		this.winGUI.Setup(registerType, closeCallBack);
		this.winGUI.Open();
	}

	private AccessoryFilterWindowCtrl.WindowGUI winGUI;

	public class WindowGUI
	{
		private PguiOpenWindowCtrl baseWindow { get; set; }

		private PguiButtonCtrl ResetButton { get; set; }

		private List<AccessoryFilterWindowCtrl.WindowGUI.RarityButton> RarityButtonList { get; set; }

		private List<AccessoryFilterWindowCtrl.WindowGUI.DispTypeButton> DispTypeButtonList { get; set; }

		private List<AccessoryFilterWindowCtrl.WindowGUI.OwnerStatusButton> OwnerStatusButtonList { get; set; }

		private Dictionary<ItemDef.Rarity, bool> RarityMap { get; set; }

		private Dictionary<DataManagerCharaAccessory.DispType, bool> DispTypeMap { get; set; }

		private Dictionary<bool, bool> OwnerStatusMap { get; set; }

		private UnityAction<bool, AccessorySortFilter.FilterStatus> CloseCallBack { get; set; }

		private string SearchText { get; set; }

		private string AllSearchText { get; set; }

		public WindowGUI(GameObject go)
		{
			this.baseObj = go;
			Transform transform = this.baseObj.transform;
			this.baseWindow = transform.Find("Window_AccessoryFilter").GetComponent<PguiOpenWindowCtrl>();
			this.ResetButton = transform.Find("Window_AccessoryFilter/Base/Window/Btn_reset").GetComponent<PguiButtonCtrl>();
			this.ResetButton.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickResetButton), PguiButtonCtrl.SoundType.DEFAULT);
			this.RarityMap = new Dictionary<ItemDef.Rarity, bool>();
			this.DispTypeMap = new Dictionary<DataManagerCharaAccessory.DispType, bool>();
			this.OwnerStatusMap = new Dictionary<bool, bool>();
			AccessoryFilterWindowCtrl.WindowGUI.RarityButton rarityButton = new AccessoryFilterWindowCtrl.WindowGUI.RarityButton(transform.Find("Window_AccessoryFilter/Base/Window/ScrollView/Viewport/Content/Box01/Btn01").gameObject, ItemDef.Rarity.STAR1);
			rarityButton.Button.AddOnClickListener((PguiToggleButtonCtrl pbc, int idx) => this.OnClickRarityButton(ItemDef.Rarity.STAR1, idx));
			AccessoryFilterWindowCtrl.WindowGUI.RarityButton rarityButton2 = new AccessoryFilterWindowCtrl.WindowGUI.RarityButton(transform.Find("Window_AccessoryFilter/Base/Window/ScrollView/Viewport/Content/Box01/Btn02").gameObject, ItemDef.Rarity.STAR2);
			rarityButton2.Button.AddOnClickListener((PguiToggleButtonCtrl pbc, int idx) => this.OnClickRarityButton(ItemDef.Rarity.STAR2, idx));
			AccessoryFilterWindowCtrl.WindowGUI.RarityButton rarityButton3 = new AccessoryFilterWindowCtrl.WindowGUI.RarityButton(transform.Find("Window_AccessoryFilter/Base/Window/ScrollView/Viewport/Content/Box01/Btn03").gameObject, ItemDef.Rarity.STAR3);
			rarityButton3.Button.AddOnClickListener((PguiToggleButtonCtrl pbc, int idx) => this.OnClickRarityButton(ItemDef.Rarity.STAR3, idx));
			AccessoryFilterWindowCtrl.WindowGUI.RarityButton rarityButton4 = new AccessoryFilterWindowCtrl.WindowGUI.RarityButton(transform.Find("Window_AccessoryFilter/Base/Window/ScrollView/Viewport/Content/Box01/Btn04").gameObject, ItemDef.Rarity.STAR4);
			rarityButton4.Button.AddOnClickListener((PguiToggleButtonCtrl pbc, int idx) => this.OnClickRarityButton(ItemDef.Rarity.STAR4, idx));
			this.RarityButtonList = new List<AccessoryFilterWindowCtrl.WindowGUI.RarityButton> { rarityButton, rarityButton2, rarityButton3, rarityButton4 };
			AccessoryFilterWindowCtrl.WindowGUI.DispTypeButton dispTypeButton = new AccessoryFilterWindowCtrl.WindowGUI.DispTypeButton(transform.Find("Window_AccessoryFilter/Base/Window/ScrollView/Viewport/Content/Horizon/Box02/Btn01").gameObject, DataManagerCharaAccessory.DispType.Always);
			dispTypeButton.Button.AddOnClickListener((PguiToggleButtonCtrl pbc, int idx) => this.OnClickDispTypeButton(DataManagerCharaAccessory.DispType.Always, idx));
			AccessoryFilterWindowCtrl.WindowGUI.DispTypeButton dispTypeButton2 = new AccessoryFilterWindowCtrl.WindowGUI.DispTypeButton(transform.Find("Window_AccessoryFilter/Base/Window/ScrollView/Viewport/Content/Horizon/Box02/Btn02").gameObject, DataManagerCharaAccessory.DispType.Battle);
			dispTypeButton2.Button.AddOnClickListener((PguiToggleButtonCtrl pbc, int idx) => this.OnClickDispTypeButton(DataManagerCharaAccessory.DispType.Battle, idx));
			this.DispTypeButtonList = new List<AccessoryFilterWindowCtrl.WindowGUI.DispTypeButton> { dispTypeButton, dispTypeButton2 };
			AccessoryFilterWindowCtrl.WindowGUI.OwnerStatusButton ownerStatusButton = new AccessoryFilterWindowCtrl.WindowGUI.OwnerStatusButton(transform.Find("Window_AccessoryFilter/Base/Window/ScrollView/Viewport/Content/Horizon/Box03/Btn01").gameObject, false);
			ownerStatusButton.Button.AddOnClickListener((PguiToggleButtonCtrl pbc, int idx) => this.OnClickDispTypeButton(false, idx));
			AccessoryFilterWindowCtrl.WindowGUI.OwnerStatusButton ownerStatusButton2 = new AccessoryFilterWindowCtrl.WindowGUI.OwnerStatusButton(transform.Find("Window_AccessoryFilter/Base/Window/ScrollView/Viewport/Content/Horizon/Box03/Btn02").gameObject, true);
			ownerStatusButton2.Button.AddOnClickListener((PguiToggleButtonCtrl pbc, int idx) => this.OnClickDispTypeButton(true, idx));
			this.OwnerStatusButtonList = new List<AccessoryFilterWindowCtrl.WindowGUI.OwnerStatusButton> { ownerStatusButton, ownerStatusButton2 };
			this.windowTextSearchChange = new AccessoryFilterWindowCtrl.WindowGUI.WindowTextSearchChange(transform);
			this.SearchText = "";
			this.AllSearchText = "";
			this.baseWindow.Setup("フィルター", "選択した項目で絞り込みが出来ます", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, new PguiOpenWindowCtrl.Callback(this.OnClickWindowButton), new UnityAction(this.WindowDestroy), false);
		}

		private bool OnClickRarityButton(ItemDef.Rarity rarity, int idx)
		{
			this.RarityMap[rarity] = idx == 0;
			return true;
		}

		private bool OnClickDispTypeButton(DataManagerCharaAccessory.DispType dispType, int idx)
		{
			this.DispTypeMap[dispType] = idx == 0;
			return true;
		}

		private bool OnClickDispTypeButton(bool ownered, int idx)
		{
			this.OwnerStatusMap[ownered] = idx == 0;
			return true;
		}

		private void OnClickResetButton(PguiButtonCtrl btn)
		{
			this.RarityMap = new Dictionary<ItemDef.Rarity, bool>();
			this.DispTypeMap = new Dictionary<DataManagerCharaAccessory.DispType, bool>();
			this.OwnerStatusMap = new Dictionary<bool, bool>();
			this.SearchText = "";
			this.windowTextSearchChange.InputField.text = "";
			foreach (AccessoryFilterWindowCtrl.WindowGUI.RarityButton rarityButton in this.RarityButtonList)
			{
				rarityButton.Button.SetToggleIndex(0);
			}
			foreach (AccessoryFilterWindowCtrl.WindowGUI.DispTypeButton dispTypeButton in this.DispTypeButtonList)
			{
				dispTypeButton.Button.SetToggleIndex(0);
			}
			foreach (AccessoryFilterWindowCtrl.WindowGUI.OwnerStatusButton ownerStatusButton in this.OwnerStatusButtonList)
			{
				ownerStatusButton.Button.SetToggleIndex(0);
			}
		}

		private bool OnClickWindowButton(int index)
		{
			bool flag = false;
			AccessorySortFilter.FilterStatus filterStatus = new AccessorySortFilter.FilterStatus();
			switch (index)
			{
			case -1:
				this.ResetSearchText();
				break;
			case 0:
				this.ResetSearchText();
				break;
			case 1:
			{
				HashSet<ItemDef.Rarity> hashSet = new HashSet<ItemDef.Rarity>();
				foreach (KeyValuePair<ItemDef.Rarity, bool> keyValuePair in this.RarityMap)
				{
					if (keyValuePair.Value)
					{
						hashSet.Add(keyValuePair.Key);
					}
				}
				filterStatus.SelectRarityList = hashSet;
				HashSet<DataManagerCharaAccessory.DispType> hashSet2 = new HashSet<DataManagerCharaAccessory.DispType>();
				foreach (KeyValuePair<DataManagerCharaAccessory.DispType, bool> keyValuePair2 in this.DispTypeMap)
				{
					if (keyValuePair2.Value)
					{
						hashSet2.Add(keyValuePair2.Key);
					}
				}
				filterStatus.SelectDispTypeList = hashSet2;
				HashSet<bool> hashSet3 = new HashSet<bool>();
				foreach (KeyValuePair<bool, bool> keyValuePair3 in this.OwnerStatusMap)
				{
					if (keyValuePair3.Value)
					{
						hashSet3.Add(keyValuePair3.Key);
					}
				}
				filterStatus.SelectOwnerStatusList = hashSet3;
				filterStatus.SearchText = this.windowTextSearchChange.InputField.text;
				flag = true;
				break;
			}
			}
			UnityAction<bool, AccessorySortFilter.FilterStatus> closeCallBack = this.CloseCallBack;
			if (closeCallBack != null)
			{
				closeCallBack(flag, filterStatus);
			}
			return true;
		}

		public void Setup(SortFilterDefine.RegisterType registerType, UnityAction<bool, AccessorySortFilter.FilterStatus> closeCallBack)
		{
			AccessorySortFilter.FilterStatus filterStatus = SortFilterManager.Accessory.GetFilterStatus(registerType);
			foreach (AccessoryFilterWindowCtrl.WindowGUI.RarityButton rarityButton in this.RarityButtonList)
			{
				if (filterStatus.SelectRarityList.Contains(rarityButton.Rarity))
				{
					rarityButton.Button.SetToggleIndex(1);
					this.RarityMap.Add(rarityButton.Rarity, true);
				}
				else
				{
					rarityButton.Button.SetToggleIndex(0);
				}
			}
			foreach (AccessoryFilterWindowCtrl.WindowGUI.DispTypeButton dispTypeButton in this.DispTypeButtonList)
			{
				if (filterStatus.SelectDispTypeList.Contains(dispTypeButton.DispType))
				{
					dispTypeButton.Button.SetToggleIndex(1);
					this.DispTypeMap.Add(dispTypeButton.DispType, true);
				}
				else
				{
					dispTypeButton.Button.SetToggleIndex(0);
				}
			}
			foreach (AccessoryFilterWindowCtrl.WindowGUI.OwnerStatusButton ownerStatusButton in this.OwnerStatusButtonList)
			{
				if (filterStatus.SelectOwnerStatusList.Contains(ownerStatusButton.OwnerStatus))
				{
					ownerStatusButton.Button.SetToggleIndex(1);
					this.OwnerStatusMap.Add(ownerStatusButton.OwnerStatus, true);
				}
				else
				{
					ownerStatusButton.Button.SetToggleIndex(0);
				}
			}
			if (filterStatus.SearchText.Length != 0)
			{
				if (this.isAllWindow)
				{
					this.AllSearchText = filterStatus.SearchText;
					this.windowTextSearchChange.InputField.text = this.AllSearchText;
				}
				else
				{
					this.SearchText = filterStatus.SearchText;
					this.windowTextSearchChange.InputField.text = this.SearchText;
				}
			}
			this.windowTextSearchChange.InputField.onEndEdit.AddListener(delegate(string str)
			{
				if (this.isAllWindow)
				{
					this.AllSearchText = PrjUtil.ModifiedComment(str);
					this.windowTextSearchChange.InputField.text = this.AllSearchText;
					return;
				}
				this.SearchText = PrjUtil.ModifiedComment(str);
				this.windowTextSearchChange.InputField.text = this.SearchText;
			});
			this.CloseCallBack = closeCallBack;
		}

		public void Open()
		{
			this.baseWindow.Open();
			this.SetupSearchTextActive();
		}

		public void WindowDestroy()
		{
			Object.Destroy(this.baseObj);
		}

		private void ResetSearchText()
		{
			if (this.windowTextSearchChange.InputField.text.Length == 0)
			{
				this.windowTextSearchChange.TextTransform.gameObject.SetActive(false);
			}
			this.windowTextSearchChange.InputField.text = (this.isAllWindow ? this.AllSearchText : this.SearchText);
		}

		private void SetupSearchTextActive()
		{
			if (this.windowTextSearchChange.TextTransform != null)
			{
				this.windowTextSearchChange.TextTransform.gameObject.SetActive(true);
			}
		}

		public void SetupIsAllWindow(SortFilterDefine.RegisterType type)
		{
			this.isAllWindow = SortFilterDefine.RegisterType.ACCESSORY_ALL == type;
		}

		private GameObject baseObj;

		private AccessoryFilterWindowCtrl.WindowGUI.WindowTextSearchChange windowTextSearchChange;

		private bool isAllWindow;

		private class RarityButton
		{
			public PguiToggleButtonCtrl Button { get; set; }

			public ItemDef.Rarity Rarity { get; set; }

			public RarityButton(GameObject go, ItemDef.Rarity rarity)
			{
				this.Button = go.GetComponent<PguiToggleButtonCtrl>();
				this.Rarity = rarity;
			}
		}

		private class DispTypeButton
		{
			public PguiToggleButtonCtrl Button { get; set; }

			public DataManagerCharaAccessory.DispType DispType { get; set; }

			public DispTypeButton(GameObject go, DataManagerCharaAccessory.DispType dispType)
			{
				this.Button = go.GetComponent<PguiToggleButtonCtrl>();
				this.DispType = dispType;
			}
		}

		private class OwnerStatusButton
		{
			public PguiToggleButtonCtrl Button { get; set; }

			public bool OwnerStatus { get; set; }

			public OwnerStatusButton(GameObject go, bool ownerStatus)
			{
				this.Button = go.GetComponent<PguiToggleButtonCtrl>();
				this.OwnerStatus = ownerStatus;
			}
		}

		public class WindowTextSearchChange
		{
			public WindowTextSearchChange(Transform baseTr)
			{
				this.InputField = baseTr.Find("Window_AccessoryFilter/Base/Window/ScrollView/Viewport/Content/Box00/InputField").GetComponent<InputField>();
				this.TextTransform = baseTr.Find("Window_AccessoryFilter/Base/Window/ScrollView/Viewport/Content/Box00/InputField/Txt").GetComponent<Transform>();
				this.InputField.lineType = InputField.LineType.SingleLine;
			}

			public InputField InputField;

			public Transform TextTransform;
		}
	}
}
