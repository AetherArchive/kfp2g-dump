using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000132 RID: 306
public class SelCharaGrowMiracle
{
	// Token: 0x0600106B RID: 4203 RVA: 0x000C772C File Offset: 0x000C592C
	public SelCharaGrowMiracle(Transform baseTr)
	{
		this.GrowMiracleGUI = new SelCharaGrowMiracle.CharaGrowMiracleGUI();
		GameObject gameObject = (GameObject)Resources.Load("SceneCharaEdit/GUI/Prefab/GUI_CharaEdit_Window_Arts");
		this.GrowMiracleGUI.miracleWindow = new SelCharaGrowMiracle.WindowMiracle(Object.Instantiate<Transform>(gameObject.transform.Find("Window_ArtsLvUp"), baseTr).transform);
		this.GrowMiracleGUI.miracleResultWindow = new SelCharaGrowMiracle.WindowArtsResult(Object.Instantiate<Transform>(gameObject.transform.Find("Window_ArtsLvUp_After"), baseTr).transform);
		this.GrowMiracleGUI.miracleUpTab = new SelCharaGrowMiracle.MiracleUpTab(baseTr.Find("CharaGrow_Main").Find("Main/Right/ArtsLv"));
	}

	// Token: 0x0600106C RID: 4204 RVA: 0x000C77D8 File Offset: 0x000C59D8
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

	// Token: 0x0600106D RID: 4205 RVA: 0x000C7928 File Offset: 0x000C5B28
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

	// Token: 0x04000E4E RID: 3662
	public SelCharaGrowMiracle.CharaGrowMiracleGUI GrowMiracleGUI;

	// Token: 0x02000A09 RID: 2569
	public class MiracleItem
	{
		// Token: 0x04004027 RID: 16423
		public IconItemCtrl iconItemCtrl;

		// Token: 0x04004028 RID: 16424
		public PguiTextCtrl itemNum;
	}

	// Token: 0x02000A0A RID: 2570
	public class MiracleUpTab
	{
		// Token: 0x06003E32 RID: 15922 RVA: 0x001E5A02 File Offset: 0x001E3C02
		public MiracleUpTab(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.iconItemList = new List<SelCharaGrowMiracle.MiracleItem>();
			this.iconBaseList = new List<RectTransform>();
			this.Txt_LvInfo = baseTr.Find("Txt_LvInfo").GetComponent<PguiTextCtrl>();
		}

		// Token: 0x04004029 RID: 16425
		public GameObject baseObj;

		// Token: 0x0400402A RID: 16426
		public static readonly int COUNT = 8;

		// Token: 0x0400402B RID: 16427
		public List<SelCharaGrowMiracle.MiracleItem> iconItemList;

		// Token: 0x0400402C RID: 16428
		public List<RectTransform> iconBaseList;

		// Token: 0x0400402D RID: 16429
		public PguiTextCtrl Txt_LvInfo;
	}

	// Token: 0x02000A0B RID: 2571
	public class WindowMiracle
	{
		// Token: 0x06003E34 RID: 15924 RVA: 0x001E5A4C File Offset: 0x001E3C4C
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

		// Token: 0x0400402E RID: 16430
		private static readonly int COUNT = 8;

		// Token: 0x0400402F RID: 16431
		public PguiOpenWindowCtrl owCtrl;

		// Token: 0x04004030 RID: 16432
		public PguiTextCtrl Txt_ArtsName;

		// Token: 0x04004031 RID: 16433
		public PguiTextCtrl Num_Lv_Before;

		// Token: 0x04004032 RID: 16434
		public PguiTextCtrl Num_Lv_After;

		// Token: 0x04004033 RID: 16435
		public List<IconItemCtrl> iconItemList = new List<IconItemCtrl>();

		// Token: 0x04004034 RID: 16436
		public PguiTextCtrl ItemUse_Num;

		// Token: 0x04004035 RID: 16437
		public PguiTextCtrl ItemOwn_Num;

		// Token: 0x04004036 RID: 16438
		public CharaUtil.GUISkillInfo skillInfoBefore;

		// Token: 0x04004037 RID: 16439
		public CharaUtil.GUISkillInfo skillInfoAfter;
	}

	// Token: 0x02000A0C RID: 2572
	public class WindowArtsResult
	{
		// Token: 0x06003E36 RID: 15926 RVA: 0x001E5B58 File Offset: 0x001E3D58
		public WindowArtsResult(Transform baseTr)
		{
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.Txt_ArtsName = baseTr.Find("Base/Window/Txt_ArtsName").GetComponent<PguiTextCtrl>();
			this.Num_Lv_Before = baseTr.Find("Base/Window/Txt_ArtsName/Img_Yaji/Num_Lv_Before").GetComponent<PguiTextCtrl>();
			this.Num_Lv_After = baseTr.Find("Base/Window/Txt_ArtsName/Img_Yaji/Num_Lv_After").GetComponent<PguiTextCtrl>();
		}

		// Token: 0x04004038 RID: 16440
		public PguiOpenWindowCtrl owCtrl;

		// Token: 0x04004039 RID: 16441
		public PguiTextCtrl Txt_ArtsName;

		// Token: 0x0400403A RID: 16442
		public PguiTextCtrl Num_Lv_Before;

		// Token: 0x0400403B RID: 16443
		public PguiTextCtrl Num_Lv_After;
	}

	// Token: 0x02000A0D RID: 2573
	public class CharaGrowMiracleGUI
	{
		// Token: 0x0400403C RID: 16444
		public SelCharaGrowMiracle.WindowMiracle miracleWindow;

		// Token: 0x0400403D RID: 16445
		public SelCharaGrowMiracle.WindowArtsResult miracleResultWindow;

		// Token: 0x0400403E RID: 16446
		public SelCharaGrowMiracle.MiracleUpTab miracleUpTab;
	}
}
