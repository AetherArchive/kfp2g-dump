using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200018A RID: 394
public class AccessoryFilterWindowCtrl : MonoBehaviour
{
	// Token: 0x06001A60 RID: 6752 RVA: 0x00156760 File Offset: 0x00154960
	public void Initialize(GameObject go)
	{
		this.winGUI = new AccessoryFilterWindowCtrl.WindowGUI(go);
	}

	// Token: 0x06001A61 RID: 6753 RVA: 0x0015676E File Offset: 0x0015496E
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

	// Token: 0x0400142E RID: 5166
	private AccessoryFilterWindowCtrl.WindowGUI winGUI;

	// Token: 0x02000E70 RID: 3696
	public class WindowGUI
	{
		// Token: 0x17000B23 RID: 2851
		// (get) Token: 0x06004C7D RID: 19581 RVA: 0x0022D530 File Offset: 0x0022B730
		// (set) Token: 0x06004C7E RID: 19582 RVA: 0x0022D538 File Offset: 0x0022B738
		private PguiOpenWindowCtrl baseWindow { get; set; }

		// Token: 0x17000B24 RID: 2852
		// (get) Token: 0x06004C7F RID: 19583 RVA: 0x0022D541 File Offset: 0x0022B741
		// (set) Token: 0x06004C80 RID: 19584 RVA: 0x0022D549 File Offset: 0x0022B749
		private PguiButtonCtrl ResetButton { get; set; }

		// Token: 0x17000B25 RID: 2853
		// (get) Token: 0x06004C81 RID: 19585 RVA: 0x0022D552 File Offset: 0x0022B752
		// (set) Token: 0x06004C82 RID: 19586 RVA: 0x0022D55A File Offset: 0x0022B75A
		private List<AccessoryFilterWindowCtrl.WindowGUI.RarityButton> RarityButtonList { get; set; }

		// Token: 0x17000B26 RID: 2854
		// (get) Token: 0x06004C83 RID: 19587 RVA: 0x0022D563 File Offset: 0x0022B763
		// (set) Token: 0x06004C84 RID: 19588 RVA: 0x0022D56B File Offset: 0x0022B76B
		private List<AccessoryFilterWindowCtrl.WindowGUI.DispTypeButton> DispTypeButtonList { get; set; }

		// Token: 0x17000B27 RID: 2855
		// (get) Token: 0x06004C85 RID: 19589 RVA: 0x0022D574 File Offset: 0x0022B774
		// (set) Token: 0x06004C86 RID: 19590 RVA: 0x0022D57C File Offset: 0x0022B77C
		private List<AccessoryFilterWindowCtrl.WindowGUI.OwnerStatusButton> OwnerStatusButtonList { get; set; }

		// Token: 0x17000B28 RID: 2856
		// (get) Token: 0x06004C87 RID: 19591 RVA: 0x0022D585 File Offset: 0x0022B785
		// (set) Token: 0x06004C88 RID: 19592 RVA: 0x0022D58D File Offset: 0x0022B78D
		private Dictionary<ItemDef.Rarity, bool> RarityMap { get; set; }

		// Token: 0x17000B29 RID: 2857
		// (get) Token: 0x06004C89 RID: 19593 RVA: 0x0022D596 File Offset: 0x0022B796
		// (set) Token: 0x06004C8A RID: 19594 RVA: 0x0022D59E File Offset: 0x0022B79E
		private Dictionary<DataManagerCharaAccessory.DispType, bool> DispTypeMap { get; set; }

		// Token: 0x17000B2A RID: 2858
		// (get) Token: 0x06004C8B RID: 19595 RVA: 0x0022D5A7 File Offset: 0x0022B7A7
		// (set) Token: 0x06004C8C RID: 19596 RVA: 0x0022D5AF File Offset: 0x0022B7AF
		private Dictionary<bool, bool> OwnerStatusMap { get; set; }

		// Token: 0x17000B2B RID: 2859
		// (get) Token: 0x06004C8D RID: 19597 RVA: 0x0022D5B8 File Offset: 0x0022B7B8
		// (set) Token: 0x06004C8E RID: 19598 RVA: 0x0022D5C0 File Offset: 0x0022B7C0
		private UnityAction<bool, AccessorySortFilter.FilterStatus> CloseCallBack { get; set; }

		// Token: 0x17000B2C RID: 2860
		// (get) Token: 0x06004C8F RID: 19599 RVA: 0x0022D5C9 File Offset: 0x0022B7C9
		// (set) Token: 0x06004C90 RID: 19600 RVA: 0x0022D5D1 File Offset: 0x0022B7D1
		private string SearchText { get; set; }

		// Token: 0x17000B2D RID: 2861
		// (get) Token: 0x06004C91 RID: 19601 RVA: 0x0022D5DA File Offset: 0x0022B7DA
		// (set) Token: 0x06004C92 RID: 19602 RVA: 0x0022D5E2 File Offset: 0x0022B7E2
		private string AllSearchText { get; set; }

		// Token: 0x06004C93 RID: 19603 RVA: 0x0022D5EC File Offset: 0x0022B7EC
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

		// Token: 0x06004C94 RID: 19604 RVA: 0x0022D8A6 File Offset: 0x0022BAA6
		private bool OnClickRarityButton(ItemDef.Rarity rarity, int idx)
		{
			this.RarityMap[rarity] = idx == 0;
			return true;
		}

		// Token: 0x06004C95 RID: 19605 RVA: 0x0022D8B9 File Offset: 0x0022BAB9
		private bool OnClickDispTypeButton(DataManagerCharaAccessory.DispType dispType, int idx)
		{
			this.DispTypeMap[dispType] = idx == 0;
			return true;
		}

		// Token: 0x06004C96 RID: 19606 RVA: 0x0022D8CC File Offset: 0x0022BACC
		private bool OnClickDispTypeButton(bool ownered, int idx)
		{
			this.OwnerStatusMap[ownered] = idx == 0;
			return true;
		}

		// Token: 0x06004C97 RID: 19607 RVA: 0x0022D8E0 File Offset: 0x0022BAE0
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

		// Token: 0x06004C98 RID: 19608 RVA: 0x0022DA04 File Offset: 0x0022BC04
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

		// Token: 0x06004C99 RID: 19609 RVA: 0x0022DB98 File Offset: 0x0022BD98
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

		// Token: 0x06004C9A RID: 19610 RVA: 0x0022DDB4 File Offset: 0x0022BFB4
		public void Open()
		{
			this.baseWindow.Open();
			this.SetupSearchTextActive();
		}

		// Token: 0x06004C9B RID: 19611 RVA: 0x0022DDC7 File Offset: 0x0022BFC7
		public void WindowDestroy()
		{
			Object.Destroy(this.baseObj);
		}

		// Token: 0x06004C9C RID: 19612 RVA: 0x0022DDD4 File Offset: 0x0022BFD4
		private void ResetSearchText()
		{
			if (this.windowTextSearchChange.InputField.text.Length == 0)
			{
				this.windowTextSearchChange.TextTransform.gameObject.SetActive(false);
			}
			this.windowTextSearchChange.InputField.text = (this.isAllWindow ? this.AllSearchText : this.SearchText);
		}

		// Token: 0x06004C9D RID: 19613 RVA: 0x0022DE34 File Offset: 0x0022C034
		private void SetupSearchTextActive()
		{
			if (this.windowTextSearchChange.TextTransform != null)
			{
				this.windowTextSearchChange.TextTransform.gameObject.SetActive(true);
			}
		}

		// Token: 0x06004C9E RID: 19614 RVA: 0x0022DE5F File Offset: 0x0022C05F
		public void SetupIsAllWindow(SortFilterDefine.RegisterType type)
		{
			this.isAllWindow = SortFilterDefine.RegisterType.ACCESSORY_ALL == type;
		}

		// Token: 0x04005306 RID: 21254
		private GameObject baseObj;

		// Token: 0x04005310 RID: 21264
		private AccessoryFilterWindowCtrl.WindowGUI.WindowTextSearchChange windowTextSearchChange;

		// Token: 0x04005313 RID: 21267
		private bool isAllWindow;

		// Token: 0x020011D4 RID: 4564
		private class RarityButton
		{
			// Token: 0x17000CFF RID: 3327
			// (get) Token: 0x0600572C RID: 22316 RVA: 0x00256603 File Offset: 0x00254803
			// (set) Token: 0x0600572D RID: 22317 RVA: 0x0025660B File Offset: 0x0025480B
			public PguiToggleButtonCtrl Button { get; set; }

			// Token: 0x17000D00 RID: 3328
			// (get) Token: 0x0600572E RID: 22318 RVA: 0x00256614 File Offset: 0x00254814
			// (set) Token: 0x0600572F RID: 22319 RVA: 0x0025661C File Offset: 0x0025481C
			public ItemDef.Rarity Rarity { get; set; }

			// Token: 0x06005730 RID: 22320 RVA: 0x00256625 File Offset: 0x00254825
			public RarityButton(GameObject go, ItemDef.Rarity rarity)
			{
				this.Button = go.GetComponent<PguiToggleButtonCtrl>();
				this.Rarity = rarity;
			}
		}

		// Token: 0x020011D5 RID: 4565
		private class DispTypeButton
		{
			// Token: 0x17000D01 RID: 3329
			// (get) Token: 0x06005731 RID: 22321 RVA: 0x00256640 File Offset: 0x00254840
			// (set) Token: 0x06005732 RID: 22322 RVA: 0x00256648 File Offset: 0x00254848
			public PguiToggleButtonCtrl Button { get; set; }

			// Token: 0x17000D02 RID: 3330
			// (get) Token: 0x06005733 RID: 22323 RVA: 0x00256651 File Offset: 0x00254851
			// (set) Token: 0x06005734 RID: 22324 RVA: 0x00256659 File Offset: 0x00254859
			public DataManagerCharaAccessory.DispType DispType { get; set; }

			// Token: 0x06005735 RID: 22325 RVA: 0x00256662 File Offset: 0x00254862
			public DispTypeButton(GameObject go, DataManagerCharaAccessory.DispType dispType)
			{
				this.Button = go.GetComponent<PguiToggleButtonCtrl>();
				this.DispType = dispType;
			}
		}

		// Token: 0x020011D6 RID: 4566
		private class OwnerStatusButton
		{
			// Token: 0x17000D03 RID: 3331
			// (get) Token: 0x06005736 RID: 22326 RVA: 0x0025667D File Offset: 0x0025487D
			// (set) Token: 0x06005737 RID: 22327 RVA: 0x00256685 File Offset: 0x00254885
			public PguiToggleButtonCtrl Button { get; set; }

			// Token: 0x17000D04 RID: 3332
			// (get) Token: 0x06005738 RID: 22328 RVA: 0x0025668E File Offset: 0x0025488E
			// (set) Token: 0x06005739 RID: 22329 RVA: 0x00256696 File Offset: 0x00254896
			public bool OwnerStatus { get; set; }

			// Token: 0x0600573A RID: 22330 RVA: 0x0025669F File Offset: 0x0025489F
			public OwnerStatusButton(GameObject go, bool ownerStatus)
			{
				this.Button = go.GetComponent<PguiToggleButtonCtrl>();
				this.OwnerStatus = ownerStatus;
			}
		}

		// Token: 0x020011D7 RID: 4567
		public class WindowTextSearchChange
		{
			// Token: 0x0600573B RID: 22331 RVA: 0x002566BA File Offset: 0x002548BA
			public WindowTextSearchChange(Transform baseTr)
			{
				this.InputField = baseTr.Find("Window_AccessoryFilter/Base/Window/ScrollView/Viewport/Content/Box00/InputField").GetComponent<InputField>();
				this.TextTransform = baseTr.Find("Window_AccessoryFilter/Base/Window/ScrollView/Viewport/Content/Box00/InputField/Txt").GetComponent<Transform>();
				this.InputField.lineType = InputField.LineType.SingleLine;
			}

			// Token: 0x040061DE RID: 25054
			public InputField InputField;

			// Token: 0x040061DF RID: 25055
			public Transform TextTransform;
		}
	}
}
