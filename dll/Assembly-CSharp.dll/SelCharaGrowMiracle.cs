using System;
using System.Collections.Generic;
using UnityEngine;

public class SelCharaGrowMiracle
{
	public SelCharaGrowMiracle(Transform baseTr)
	{
		this.GrowMiracleGUI = new SelCharaGrowMiracle.CharaGrowMiracleGUI();
		GameObject gameObject = (GameObject)Resources.Load("SceneCharaEdit/GUI/Prefab/GUI_CharaEdit_Window_Arts");
		this.GrowMiracleGUI.miracleWindow = new SelCharaGrowMiracle.WindowMiracle(Object.Instantiate<Transform>(gameObject.transform.Find("Window_ArtsLvUp"), baseTr).transform);
		this.GrowMiracleGUI.miracleResultWindow = new SelCharaGrowMiracle.WindowArtsResult(Object.Instantiate<Transform>(gameObject.transform.Find("Window_ArtsLvUp_After"), baseTr).transform);
		this.GrowMiracleGUI.miracleUpTab = new SelCharaGrowMiracle.MiracleUpTab(baseTr.Find("CharaGrow_Main").Find("Main/Right/ArtsLv"));
	}

	public void SetupItemMiracle(int charaId)
	{
		GrowItemList nextItemByArtsUp = DataManager.DmChara.GetUserCharaData(charaId).GetNextItemByArtsUp(0);
		for (int i = 0; i < SelCharaGrowMiracle.MiracleUpTab.COUNT; i++)
		{
			SelCharaGrowMiracle.MiracleItem miracleItem = new SelCharaGrowMiracle.MiracleItem();
			GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("CmnIconFrame/GUI/Prefab/Icon_Item"), this.GrowMiracleGUI.miracleUpTab.baseObj.transform.Find("Item" + (i + 1).ToString("D2") + "/Icon_Item"));
			miracleItem.iconItemCtrl = gameObject.GetComponent<IconItemCtrl>();
			miracleItem.iconItemCtrl.Setup((nextItemByArtsUp != null && i < nextItemByArtsUp.itemList.Count) ? DataManager.DmItem.GetItemStaticBase(nextItemByArtsUp.itemList[i].id) : null);
			miracleItem.itemNum = this.GrowMiracleGUI.miracleUpTab.baseObj.transform.Find("Item" + (i + 1).ToString("D2") + "/Num_Item").GetComponent<PguiTextCtrl>();
			this.GrowMiracleGUI.miracleUpTab.iconItemList.Add(miracleItem);
			RectTransform component = miracleItem.iconItemCtrl.GetComponent<RectTransform>();
			this.GrowMiracleGUI.miracleUpTab.iconBaseList.Add(component);
		}
	}

	public void UpdateMiracle(int charaId)
	{
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(charaId);
		CharaDynamicData dynamicData = userCharaData.dynamicData;
		GrowItemList nextItemByArtsUp = userCharaData.GetNextItemByArtsUp(0);
		if (nextItemByArtsUp != null)
		{
			for (int i = 0; i < SelCharaGrowMiracle.MiracleUpTab.COUNT; i++)
			{
				SelCharaGrowMiracle.MiracleItem miracleItem = this.GrowMiracleGUI.miracleUpTab.iconItemList[i];
				if (i < nextItemByArtsUp.itemList.Count)
				{
					ItemData userItemData = DataManager.DmItem.GetUserItemData(nextItemByArtsUp.itemList[i].id);
					miracleItem.iconItemCtrl.Setup(userItemData.staticData);
					miracleItem.itemNum.gameObject.SetActive(true);
					miracleItem.itemNum.ReplaceTextByDefault(new string[] { "Param01", "Param02", "Param03" }, new string[]
					{
						userItemData.num.ToString(),
						nextItemByArtsUp.itemList[i].num.ToString(),
						(userItemData.num < nextItemByArtsUp.itemList[i].num) ? PrjUtil.WARNING_COLOR_CODE : ("#" + ColorUtility.ToHtmlStringRGBA(miracleItem.itemNum.m_Text.color))
					});
					this.GrowMiracleGUI.miracleUpTab.Txt_LvInfo.gameObject.SetActive(dynamicData.artsLevel < 5);
					this.GrowMiracleGUI.miracleUpTab.Txt_LvInfo.text = string.Format("強化には「なかよしLv.{0}」以上が必要 (現在「なかよしLv.{1}」)", dynamicData.artsLevel + 1, dynamicData.kizunaLevel);
					PguiColorCtrl component = this.GrowMiracleGUI.miracleUpTab.Txt_LvInfo.GetComponent<PguiColorCtrl>();
					component.InitForce();
					this.GrowMiracleGUI.miracleUpTab.Txt_LvInfo.m_Text.color = ((dynamicData.kizunaLevel >= dynamicData.artsLevel + 1) ? component.GetGameObjectById("NORMAL") : component.GetGameObjectById("CAUTION"));
				}
				else
				{
					miracleItem.iconItemCtrl.Clear();
					miracleItem.itemNum.gameObject.SetActive(false);
				}
			}
			return;
		}
		foreach (SelCharaGrowMiracle.MiracleItem miracleItem2 in this.GrowMiracleGUI.miracleUpTab.iconItemList)
		{
			miracleItem2.iconItemCtrl.Clear();
			miracleItem2.itemNum.gameObject.SetActive(false);
		}
		this.GrowMiracleGUI.miracleUpTab.Txt_LvInfo.gameObject.SetActive(false);
	}

	public SelCharaGrowMiracle.CharaGrowMiracleGUI GrowMiracleGUI;

	public class MiracleItem
	{
		public IconItemCtrl iconItemCtrl;

		public PguiTextCtrl itemNum;
	}

	public class MiracleUpTab
	{
		public MiracleUpTab(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.iconItemList = new List<SelCharaGrowMiracle.MiracleItem>();
			this.iconBaseList = new List<RectTransform>();
			this.Txt_LvInfo = baseTr.Find("Txt_LvInfo").GetComponent<PguiTextCtrl>();
		}

		public GameObject baseObj;

		public static readonly int COUNT = 8;

		public List<SelCharaGrowMiracle.MiracleItem> iconItemList;

		public List<RectTransform> iconBaseList;

		public PguiTextCtrl Txt_LvInfo;
	}

	public class WindowMiracle
	{
		public WindowMiracle(Transform baseTr)
		{
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.Txt_ArtsName = baseTr.Find("Base/Window/Txt_ArtsName").GetComponent<PguiTextCtrl>();
			this.Num_Lv_Before = baseTr.Find("Base/Window/Txt_ArtsName/Img_Yaji/Num_Lv_Before").GetComponent<PguiTextCtrl>();
			this.Num_Lv_After = baseTr.Find("Base/Window/Txt_ArtsName/Img_Yaji/Num_Lv_After").GetComponent<PguiTextCtrl>();
			for (int i = 0; i < SelCharaGrowMiracle.WindowMiracle.COUNT; i++)
			{
				this.iconItemList.Add(baseTr.Find("Base/Window/UseItemInfo/Grid/Icon_Item" + (i + 1).ToString("D2")).GetComponent<IconItemCtrl>());
			}
			this.ItemUse_Num = baseTr.Find("Base/Window/ItemUse/Num").GetComponent<PguiTextCtrl>();
			this.ItemOwn_Num = baseTr.Find("Base/Window/ItemOwn/Num").GetComponent<PguiTextCtrl>();
			this.skillInfoBefore = new CharaUtil.GUISkillInfo(baseTr.Find("Base/Window/CharaInfo_List_Skill_Short_Before"));
			this.skillInfoAfter = new CharaUtil.GUISkillInfo(baseTr.Find("Base/Window/CharaInfo_List_Skill_Short_After"));
		}

		private static readonly int COUNT = 8;

		public PguiOpenWindowCtrl owCtrl;

		public PguiTextCtrl Txt_ArtsName;

		public PguiTextCtrl Num_Lv_Before;

		public PguiTextCtrl Num_Lv_After;

		public List<IconItemCtrl> iconItemList = new List<IconItemCtrl>();

		public PguiTextCtrl ItemUse_Num;

		public PguiTextCtrl ItemOwn_Num;

		public CharaUtil.GUISkillInfo skillInfoBefore;

		public CharaUtil.GUISkillInfo skillInfoAfter;
	}

	public class WindowArtsResult
	{
		public WindowArtsResult(Transform baseTr)
		{
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.Txt_ArtsName = baseTr.Find("Base/Window/Txt_ArtsName").GetComponent<PguiTextCtrl>();
			this.Num_Lv_Before = baseTr.Find("Base/Window/Txt_ArtsName/Img_Yaji/Num_Lv_Before").GetComponent<PguiTextCtrl>();
			this.Num_Lv_After = baseTr.Find("Base/Window/Txt_ArtsName/Img_Yaji/Num_Lv_After").GetComponent<PguiTextCtrl>();
		}

		public PguiOpenWindowCtrl owCtrl;

		public PguiTextCtrl Txt_ArtsName;

		public PguiTextCtrl Num_Lv_Before;

		public PguiTextCtrl Num_Lv_After;
	}

	public class CharaGrowMiracleGUI
	{
		public SelCharaGrowMiracle.WindowMiracle miracleWindow;

		public SelCharaGrowMiracle.WindowArtsResult miracleResultWindow;

		public SelCharaGrowMiracle.MiracleUpTab miracleUpTab;
	}
}
