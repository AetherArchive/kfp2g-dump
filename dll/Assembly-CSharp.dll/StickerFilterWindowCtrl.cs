using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SGNFW.Common;
using UnityEngine;
using UnityEngine.UI;

public class StickerFilterWindowCtrl
{
	private string SearchText { get; set; }

	public void Initialize(GameObject go)
	{
		this.winGUI = new StickerFilterWindowCtrl.WindowGUI(go);
		this.winGUI.resetButton.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickReset), PguiButtonCtrl.SoundType.DEFAULT);
		this.winGUI.baseWindow.RegistCallback(new PguiOpenWindowCtrl.Callback(this.OnClickWindowButton));
		this.winGUI.frameRarity = this.CreateButtonFrame("レアリティ", SortFilterDefine.stickerFilterRarityList);
		this.winGUI.frameType = this.CreateButtonFrame("シールタイプ", SortFilterDefine.stickerFilterTypeList);
		this.windowTextSearchChange = new StickerFilterWindowCtrl.WindowTextSearchChange(go.transform);
		this.SearchText = "";
		this.windowTextSearchChange.InputField.onEndEdit.AddListener(delegate(string str)
		{
			this.windowTextSearchChange.InputField.text = PrjUtil.ModifiedComment(str);
			this.SearchText = this.windowTextSearchChange.InputField.text;
		});
	}

	public void Open()
	{
		this.winGUI.baseWindow.Setup(PrjUtil.MakeMessage("フィルターの設定"), PrjUtil.MakeMessage("選択したカテゴリで絞り込みができます"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, new PguiOpenWindowCtrl.Callback(this.OnClickWindowButton), null, false);
		this.winGUI.baseWindow.Open();
	}

	public StickerFilterWindowCtrl.FrameGUI CreateButtonFrame(string frameName, List<string> buttonNameList)
	{
		List<PguiToggleButtonCtrl> list = new List<PguiToggleButtonCtrl>();
		GameObject gameObject = AssetManager.InstantiateAssetData("SelCmn/GUI/Prefab/FilterPhoto_Parts01", Singleton<CanvasManager>.Instance.SystemPanel.transform);
		gameObject.transform.SetParent(this.winGUI.scrollContentbasePanel.transform, false);
		gameObject.transform.Find("Base/Title").GetComponent<PguiTextCtrl>().text = frameName;
		gameObject.transform.Find("Base/Title").GetComponent<PguiTextCtrl>();
		gameObject.transform.Find("Base/Title/RadioBtns").gameObject.SetActive(false);
		int num = buttonNameList.Count / 4 + ((buttonNameList.Count % 4 == 0) ? 0 : 1);
		for (int i = 0; i < num; i++)
		{
			GameObject assetObj = this.GetAssetObj(frameName);
			assetObj.transform.SetParent(gameObject.transform.Find("Base"), false);
			List<PguiToggleButtonCtrl> filterButtonList = this.GetFilterButtonList(assetObj, frameName);
			for (int j = 0; j < filterButtonList.Count; j++)
			{
				int num2 = i * 4 + j;
				if (buttonNameList.Count <= num2)
				{
					filterButtonList[j].gameObject.SetActive(false);
				}
				else if (buttonNameList[num2] == SortFilterDefine.BTN_DISABLE_STR_NAME)
				{
					filterButtonList[j].gameObject.SetActive(false);
				}
				else
				{
					if (buttonNameList[num2] != string.Empty)
					{
						Transform transform = filterButtonList[j].transform.Find("Img");
						if (transform != null)
						{
							transform.gameObject.SetActive(false);
						}
					}
					list.Add(filterButtonList[j]);
					filterButtonList[j].transform.Find("Num_Txt").GetComponent<PguiTextCtrl>().text = buttonNameList[num2];
				}
			}
		}
		return new StickerFilterWindowCtrl.FrameGUI(gameObject, list);
	}

	public GameObject GetAssetObj(string frameName)
	{
		GameObject gameObject;
		if (frameName == "お気に入り")
		{
			gameObject = AssetManager.InstantiateAssetData("SelCmn/GUI/Prefab/FilterFriends_PartsFavorite", Singleton<CanvasManager>.Instance.SystemPanel.transform);
		}
		else
		{
			gameObject = AssetManager.InstantiateAssetData("SelCmn/GUI/Prefab/FilterPhoto_Parts02", Singleton<CanvasManager>.Instance.SystemPanel.transform);
		}
		return gameObject;
	}

	public List<PguiToggleButtonCtrl> GetFilterButtonList(GameObject buttonObj, string frameName = "")
	{
		return new List<PguiToggleButtonCtrl>
		{
			buttonObj.transform.Find("Btn01").GetComponent<PguiToggleButtonCtrl>(),
			buttonObj.transform.Find("Btn02").GetComponent<PguiToggleButtonCtrl>(),
			buttonObj.transform.Find("Btn03").GetComponent<PguiToggleButtonCtrl>(),
			buttonObj.transform.Find("Btn04").GetComponent<PguiToggleButtonCtrl>()
		};
	}

	public List<ItemDef.Rarity> GetRarityButtonstatus()
	{
		List<PguiToggleButtonCtrl> buttonList = this.winGUI.frameRarity.buttonList;
		List<ItemDef.Rarity> list = new List<ItemDef.Rarity>();
		for (int i = 0; i < buttonList.Count; i++)
		{
			if (buttonList[i].GetToggleIndex() == 1)
			{
				list.Add(i + ItemDef.Rarity.STAR3);
			}
		}
		return list;
	}

	public List<DataManagerSticker.StickerType> GetTypeButtonstatus()
	{
		List<PguiToggleButtonCtrl> buttonList = this.winGUI.frameType.buttonList;
		List<DataManagerSticker.StickerType> list = new List<DataManagerSticker.StickerType>();
		for (int i = 0; i < buttonList.Count; i++)
		{
			if (buttonList[i].GetToggleIndex() == 1)
			{
				list.Add((DataManagerSticker.StickerType)i);
			}
		}
		return list;
	}

	public string GetSearchText()
	{
		return this.windowTextSearchChange.InputField.text;
	}

	public void SetupSearchText(string searchText)
	{
		this.SearchText = searchText;
		this.windowTextSearchChange.InputField.text = this.SearchText;
		this.SetupSearchTextActive();
	}

	public bool OnClickWindowButton(int index)
	{
		switch (index)
		{
		case -1:
			this.ResetSearchText();
			break;
		case 0:
			this.ResetSearchText();
			break;
		case 1:
			CanvasManager.HdlOpenWindowSortFilter.RegistStickerFilter();
			break;
		}
		return true;
	}

	private void ResetSearchText()
	{
		if (this.windowTextSearchChange.InputField.text.Length == 0)
		{
			this.windowTextSearchChange.TextTransform.gameObject.SetActive(false);
		}
		this.windowTextSearchChange.InputField.text = this.SearchText;
	}

	public void SetupSearchTextActive()
	{
		if (this.windowTextSearchChange.TextTransform != null)
		{
			this.windowTextSearchChange.TextTransform.gameObject.SetActive(true);
		}
	}

	public void SetupRarity(List<ItemDef.Rarity> rarityList)
	{
		for (int i = 0; i < this.winGUI.frameRarity.buttonList.Count; i++)
		{
			this.winGUI.frameRarity.buttonList[i].SetToggleIndex(rarityList.Contains(i + ItemDef.Rarity.STAR3) ? 1 : 0);
			int btnIdx = i;
			this.winGUI.frameRarity.buttonList[i].AllRemoveOnClickListener();
			this.winGUI.frameRarity.buttonList[i].AddOnClickListener((PguiToggleButtonCtrl pbc, int toggleIdx) => this.OnClickRarityButton(btnIdx, toggleIdx));
		}
	}

	public void SetupType(List<DataManagerSticker.StickerType> typeList)
	{
		for (int i = 0; i < this.winGUI.frameType.buttonList.Count; i++)
		{
			this.winGUI.frameType.buttonList[i].SetToggleIndex(typeList.Contains((DataManagerSticker.StickerType)i) ? 1 : 0);
			int btnIdx = i;
			this.winGUI.frameType.buttonList[i].AllRemoveOnClickListener();
			this.winGUI.frameType.buttonList[i].AddOnClickListener((PguiToggleButtonCtrl pbc, int toggleIdx) => this.OnClickTypeButton(btnIdx, toggleIdx));
		}
	}

	private void OnClickReset(PguiButtonCtrl clickBtn)
	{
		StickerFilterWindowCtrl.<OnClickReset>g__ResetButton|23_0(this.winGUI.frameRarity.buttonList);
		StickerFilterWindowCtrl.<OnClickReset>g__ResetButton|23_0(this.winGUI.frameType.buttonList);
		this.windowTextSearchChange.InputField.text = "";
	}

	public bool OnClickRarityButton(int btnIdx, int toggleIdx)
	{
		this.winGUI.frameRarity.buttonList[btnIdx].SetToggleIndex(toggleIdx);
		return true;
	}

	public bool OnClickTypeButton(int btnIdx, int toggleIdx)
	{
		this.winGUI.frameType.buttonList[btnIdx].SetToggleIndex(toggleIdx);
		return true;
	}

	[CompilerGenerated]
	internal static void <OnClickReset>g__ResetButton|23_0(List<PguiToggleButtonCtrl> btnList)
	{
		foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl in btnList)
		{
			pguiToggleButtonCtrl.SetToggleIndex(0);
		}
	}

	private StickerFilterWindowCtrl.WindowGUI winGUI;

	private StickerFilterWindowCtrl.WindowTextSearchChange windowTextSearchChange;

	public class WindowGUI
	{
		public WindowGUI(GameObject go)
		{
			this.baseObj = go;
			this.baseWindow = this.baseObj.GetComponent<PguiOpenWindowCtrl>();
			this.baseWindow.Setup(string.Empty, string.Empty, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, null, null, false);
			this.scrollRect = this.baseObj.transform.Find("Base/Window/ScrollView").GetComponent<ScrollRect>();
			this.scrollContent = this.baseObj.transform.Find("Base/Window/ScrollView/Viewport/Content").gameObject;
			this.scrollContentSizeFilter = this.scrollContent.GetComponent<ContentSizeFitter>();
			this.resetButton = this.baseObj.transform.Find("Base/Window/Btn_reset").GetComponent<PguiButtonCtrl>();
			this.scrollContentbasePanel = new GameObject();
			this.scrollContentbasePanel.name = "BasePanel";
			this.scrollContentbasePanel.AddComponent<RectTransform>();
			this.scrollContentbasePanel.transform.SetParent(this.scrollContent.transform);
			(this.scrollContentbasePanel.transform as RectTransform).anchoredPosition = Vector3.zero;
			this.scrollContentbasePanel.transform.localScale = Vector3.one;
			VerticalLayoutGroup verticalLayoutGroup = this.scrollContentbasePanel.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.childAlignment = TextAnchor.UpperCenter;
			verticalLayoutGroup.childControlWidth = false;
			verticalLayoutGroup.childControlHeight = true;
			verticalLayoutGroup.childForceExpandWidth = false;
			verticalLayoutGroup.childForceExpandHeight = true;
		}

		public GameObject baseObj;

		public PguiOpenWindowCtrl baseWindow;

		public GameObject radioBtns;

		public PguiButtonCtrl resetButton;

		public ScrollRect scrollRect;

		public GameObject scrollContent;

		public ContentSizeFitter scrollContentSizeFilter;

		public GameObject scrollContentbasePanel;

		public StickerFilterWindowCtrl.FrameGUI frameRarity;

		public StickerFilterWindowCtrl.FrameGUI frameType;
	}

	public class FrameGUI
	{
		public FrameGUI(GameObject go, List<PguiToggleButtonCtrl> btnList)
		{
			this.baseObj = go;
			this.buttonList = btnList;
		}

		public GameObject baseObj;

		public List<PguiToggleButtonCtrl> buttonList;
	}

	public class WindowTextSearchChange
	{
		public WindowTextSearchChange(Transform baseTr)
		{
			this.InputField = baseTr.Find("Base/Window/ScrollView/Viewport/Content/Box00/InputField").GetComponent<InputField>();
			this.TextTransform = baseTr.Find("Base/Window/ScrollView/Viewport/Content/Box00/InputField/Txt").GetComponent<Transform>();
			this.InputField.lineType = InputField.LineType.SingleLine;
		}

		public InputField InputField;

		public Transform TextTransform;
	}
}
