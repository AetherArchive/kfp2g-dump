using System;
using System.Collections.Generic;
using System.Linq;
using AEAuth3;
using UnityEngine;

// Token: 0x02000134 RID: 308
public class SelCharaGrowNanairo
{
	// Token: 0x06001072 RID: 4210 RVA: 0x000C7D94 File Offset: 0x000C5F94
	public SelCharaGrowNanairo(Transform baseTr)
	{
		this.GrowNanairoGUI = new SelCharaGrowNanairo.CharaGrowNanairoGUI();
		GameObject gameObject = (GameObject)Resources.Load("SceneCharaEdit/GUI/Prefab/GUI_CharaEdit_Window_Nanairo");
		this.GrowNanairoGUI.nanairoWindow = new SelCharaGrowNanairo.WindowNanairo(Object.Instantiate<Transform>(gameObject.transform.Find("Window_NanairoRelease"), baseTr).transform);
		this.GrowNanairoGUI.nanairoResultWindow = new SelCharaGrowNanairo.WindowNanairoResult(Object.Instantiate<Transform>(gameObject.transform.Find("Window_TokuseiInfo"), baseTr).transform);
		this.GrowNanairoGUI.nanairoUpTab = new SelCharaGrowNanairo.NanairoReleaseTab(baseTr.Find("CharaGrow_Main").Find("Main/Right/NanairoRelease"));
	}

	// Token: 0x06001073 RID: 4211 RVA: 0x000C7E40 File Offset: 0x000C6040
	public void SetupItemNanairo(int charaId)
	{
		GrowItemList releaseItemByNanairoAbilityRelease = DataManager.DmChara.GetUserCharaData(charaId).GetReleaseItemByNanairoAbilityRelease();
		for (int i = 0; i < SelCharaGrowNanairo.NanairoReleaseTab.COUNT; i++)
		{
			SelCharaGrowNanairo.NanairoItem nanairoItem = new SelCharaGrowNanairo.NanairoItem();
			GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("CmnIconFrame/GUI/Prefab/Icon_Item"), this.GrowNanairoGUI.nanairoUpTab.baseObj.transform.Find("Item" + (i + 1).ToString("D2") + "/Icon_Item"));
			nanairoItem.iconItemCtrl = gameObject.GetComponent<IconItemCtrl>();
			nanairoItem.iconItemCtrl.Setup((releaseItemByNanairoAbilityRelease != null && i < releaseItemByNanairoAbilityRelease.itemList.Count) ? DataManager.DmItem.GetItemStaticBase(releaseItemByNanairoAbilityRelease.itemList[i].id) : null);
			nanairoItem.itemNum = this.GrowNanairoGUI.nanairoUpTab.baseObj.transform.Find("Item" + (i + 1).ToString("D2") + "/Num_Item").GetComponent<PguiTextCtrl>();
			this.GrowNanairoGUI.nanairoUpTab.iconItemList.Add(nanairoItem);
			RectTransform component = nanairoItem.iconItemCtrl.GetComponent<RectTransform>();
			this.GrowNanairoGUI.nanairoUpTab.iconBaseList.Add(component);
		}
	}

	// Token: 0x06001074 RID: 4212 RVA: 0x000C7F90 File Offset: 0x000C6190
	public void UpdateNanairo(int charaId)
	{
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(charaId);
		CharaDynamicData dynamicData = userCharaData.dynamicData;
		GrowItemList releaseItemByNanairoAbilityRelease = userCharaData.GetReleaseItemByNanairoAbilityRelease();
		if (releaseItemByNanairoAbilityRelease != null)
		{
			for (int i = 0; i < SelCharaGrowNanairo.NanairoReleaseTab.COUNT; i++)
			{
				SelCharaGrowNanairo.NanairoItem nanairoItem = this.GrowNanairoGUI.nanairoUpTab.iconItemList[i];
				if (i < releaseItemByNanairoAbilityRelease.itemList.Count)
				{
					ItemData userItemData = DataManager.DmItem.GetUserItemData(releaseItemByNanairoAbilityRelease.itemList[i].id);
					nanairoItem.iconItemCtrl.Setup(userItemData.staticData);
					nanairoItem.itemNum.gameObject.SetActive(true);
					nanairoItem.itemNum.ReplaceTextByDefault(new string[] { "Param01", "Param02", "Param03" }, new string[]
					{
						userItemData.num.ToString(),
						releaseItemByNanairoAbilityRelease.itemList[i].num.ToString(),
						(userItemData.num < releaseItemByNanairoAbilityRelease.itemList[i].num) ? PrjUtil.WARNING_COLOR_CODE : ("#" + ColorUtility.ToHtmlStringRGBA(nanairoItem.itemNum.m_Text.color))
					});
				}
				else
				{
					nanairoItem.iconItemCtrl.Clear();
					nanairoItem.itemNum.gameObject.SetActive(false);
				}
			}
		}
		else
		{
			foreach (SelCharaGrowNanairo.NanairoItem nanairoItem2 in this.GrowNanairoGUI.nanairoUpTab.iconItemList)
			{
				nanairoItem2.iconItemCtrl.Clear();
				nanairoItem2.itemNum.gameObject.SetActive(false);
			}
		}
		bool flag = !userCharaData.IsNanairoAbilityReleaseAvailable;
		if (flag)
		{
			this.GrowNanairoGUI.nanairoUpTab.Txt_LvInfo.text = string.Format("{0} / {1}", dynamicData.level, 90);
			this.GrowNanairoGUI.nanairoUpTab.Txt_PromoteInfo.text = string.Format("{0} / {1}", dynamicData.promoteNum, 4);
			this.GrowNanairoGUI.nanairoUpTab.Txt_PhotoPocketInfo.text = string.Format("{0} / {1}", dynamicData.PhotoPocket.Count<CharaDynamicData.PPParam>((CharaDynamicData.PPParam v) => v.Flag), 2);
			this.GrowNanairoGUI.nanairoUpTab.Txt_ArtsInfo.text = string.Format("{0} / {1}", dynamicData.artsLevel, 3);
			PguiColorCtrl pguiColorCtrl = this.GrowNanairoGUI.nanairoUpTab.Txt_LvInfo.GetComponent<PguiColorCtrl>();
			pguiColorCtrl.InitForce();
			this.GrowNanairoGUI.nanairoUpTab.Txt_LvInfo.m_Text.color = ((dynamicData.level >= 90) ? pguiColorCtrl.GetGameObjectById("NORMAL") : pguiColorCtrl.GetGameObjectById("CAUTION"));
			pguiColorCtrl = this.GrowNanairoGUI.nanairoUpTab.Txt_PromoteInfo.GetComponent<PguiColorCtrl>();
			pguiColorCtrl.InitForce();
			this.GrowNanairoGUI.nanairoUpTab.Txt_PromoteInfo.m_Text.color = ((dynamicData.promoteNum >= 4) ? pguiColorCtrl.GetGameObjectById("NORMAL") : pguiColorCtrl.GetGameObjectById("CAUTION"));
			pguiColorCtrl = this.GrowNanairoGUI.nanairoUpTab.Txt_PhotoPocketInfo.GetComponent<PguiColorCtrl>();
			pguiColorCtrl.InitForce();
			this.GrowNanairoGUI.nanairoUpTab.Txt_PhotoPocketInfo.m_Text.color = ((dynamicData.PhotoPocket.Count<CharaDynamicData.PPParam>((CharaDynamicData.PPParam v) => v.Flag) >= 2) ? pguiColorCtrl.GetGameObjectById("NORMAL") : pguiColorCtrl.GetGameObjectById("CAUTION"));
			pguiColorCtrl = this.GrowNanairoGUI.nanairoUpTab.Txt_ArtsInfo.GetComponent<PguiColorCtrl>();
			pguiColorCtrl.InitForce();
			this.GrowNanairoGUI.nanairoUpTab.Txt_ArtsInfo.m_Text.color = ((dynamicData.artsLevel >= 3) ? pguiColorCtrl.GetGameObjectById("NORMAL") : pguiColorCtrl.GetGameObjectById("CAUTION"));
		}
		bool isHaveNanairoAbility = userCharaData.IsHaveNanairoAbility;
		this.GrowNanairoGUI.nanairoUpTab.maskBaseObj.SetActive(!isHaveNanairoAbility);
		this.GrowNanairoGUI.nanairoUpTab.paramBaseObj.SetActive(isHaveNanairoAbility && flag);
		this.GrowNanairoGUI.nanairoUpTab.titleBaseObj.SetActive(isHaveNanairoAbility && !flag);
		if (!isHaveNanairoAbility && releaseItemByNanairoAbilityRelease.itemList != null && releaseItemByNanairoAbilityRelease.itemList.Count > 0)
		{
			for (int j = 0; j < releaseItemByNanairoAbilityRelease.itemList.Count; j++)
			{
				this.GrowNanairoGUI.nanairoUpTab.iconItemList[j].iconItemCtrl.gameObject.SetActive(false);
				this.GrowNanairoGUI.nanairoUpTab.iconItemList[j].itemNum.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x04000E53 RID: 3667
	public SelCharaGrowNanairo.CharaGrowNanairoGUI GrowNanairoGUI;

	// Token: 0x02000A13 RID: 2579
	public class NanairoItem
	{
		// Token: 0x04004077 RID: 16503
		public IconItemCtrl iconItemCtrl;

		// Token: 0x04004078 RID: 16504
		public PguiTextCtrl itemNum;
	}

	// Token: 0x02000A14 RID: 2580
	public class NanairoReleaseTab
	{
		// Token: 0x06003E54 RID: 15956 RVA: 0x001E897C File Offset: 0x001E6B7C
		public NanairoReleaseTab(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.iconItemList = new List<SelCharaGrowNanairo.NanairoItem>();
			this.iconBaseList = new List<RectTransform>();
			this.AEImage_result = baseTr.Find("AEImage_result").GetComponent<AEImage>();
			this.AEImage_result.gameObject.SetActive(false);
			this.paramBaseObj = baseTr.Find("ParamBase").gameObject;
			this.paramBaseObj.gameObject.SetActive(false);
			this.Txt_LvInfo = baseTr.Find("ParamBase/Num_Lv").GetComponent<PguiTextCtrl>();
			this.Txt_PromoteInfo = baseTr.Find("ParamBase/Num_Promote").GetComponent<PguiTextCtrl>();
			this.Txt_PhotoPocketInfo = baseTr.Find("ParamBase/Num_PhotoPocket").GetComponent<PguiTextCtrl>();
			this.Txt_ArtsInfo = baseTr.Find("ParamBase/Num_Arts").GetComponent<PguiTextCtrl>();
			this.titleBaseObj = baseTr.Find("TitleBase").gameObject;
			this.maskBaseObj = baseTr.Find("MaskBase").gameObject;
		}

		// Token: 0x04004079 RID: 16505
		public GameObject baseObj;

		// Token: 0x0400407A RID: 16506
		public GameObject titleBaseObj;

		// Token: 0x0400407B RID: 16507
		public GameObject paramBaseObj;

		// Token: 0x0400407C RID: 16508
		public GameObject maskBaseObj;

		// Token: 0x0400407D RID: 16509
		public static readonly int COUNT = 8;

		// Token: 0x0400407E RID: 16510
		public List<SelCharaGrowNanairo.NanairoItem> iconItemList;

		// Token: 0x0400407F RID: 16511
		public List<RectTransform> iconBaseList;

		// Token: 0x04004080 RID: 16512
		public PguiTextCtrl Txt_LvInfo;

		// Token: 0x04004081 RID: 16513
		public PguiTextCtrl Txt_PromoteInfo;

		// Token: 0x04004082 RID: 16514
		public PguiTextCtrl Txt_PhotoPocketInfo;

		// Token: 0x04004083 RID: 16515
		public PguiTextCtrl Txt_ArtsInfo;

		// Token: 0x04004084 RID: 16516
		public AEImage AEImage_result;
	}

	// Token: 0x02000A15 RID: 2581
	public class WindowNanairo
	{
		// Token: 0x06003E56 RID: 15958 RVA: 0x001E8A8C File Offset: 0x001E6C8C
		public WindowNanairo(Transform baseTr)
		{
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.Txt_AbilityName = baseTr.Find("Base/Window/Txt_AbilityName").GetComponent<PguiTextCtrl>();
			for (int i = 0; i < SelCharaGrowNanairo.WindowNanairo.COUNT; i++)
			{
				this.iconItemList.Add(baseTr.Find("Base/Window/UseItemInfo/Grid/Icon_Item" + (i + 1).ToString("D2")).GetComponent<IconItemCtrl>());
			}
			this.AbilityInfo = new CharaUtil.GUISkillInfo(baseTr.Find("Base/Window/CharaInfo_List_Skill_06_nanairo"));
		}

		// Token: 0x04004085 RID: 16517
		private static readonly int COUNT = 8;

		// Token: 0x04004086 RID: 16518
		public PguiOpenWindowCtrl owCtrl;

		// Token: 0x04004087 RID: 16519
		public PguiTextCtrl Txt_AbilityName;

		// Token: 0x04004088 RID: 16520
		public List<IconItemCtrl> iconItemList = new List<IconItemCtrl>();

		// Token: 0x04004089 RID: 16521
		public PguiTextCtrl ItemUse_Num;

		// Token: 0x0400408A RID: 16522
		public PguiTextCtrl ItemOwn_Num;

		// Token: 0x0400408B RID: 16523
		public CharaUtil.GUISkillInfo AbilityInfo;
	}

	// Token: 0x02000A16 RID: 2582
	public class WindowNanairoResult
	{
		// Token: 0x06003E58 RID: 15960 RVA: 0x001E8B2C File Offset: 0x001E6D2C
		public WindowNanairoResult(Transform baseTr)
		{
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.Txt_KindNanairo = baseTr.Find("Base/Window/CharaInfo_List_Skill_06_nanairo/Txt_Kind").GetComponent<PguiTextCtrl>();
			this.Txt_NameNanairo = baseTr.Find("Base/Window/CharaInfo_List_Skill_06_nanairo/Txt_Name").GetComponent<PguiTextCtrl>();
			this.Txt_InfoNanairo = baseTr.Find("Base/Window/CharaInfo_List_Skill_06_nanairo/Txt_Info").GetComponent<PguiTextCtrl>();
			this.Num_LvNanairo = baseTr.Find("Base/Window/CharaInfo_List_Skill_06_nanairo/Num_Lv").GetComponent<PguiTextCtrl>();
			this.Num_LvNanairo.gameObject.SetActive(false);
			this.disableNanairo = baseTr.Find("Base/Window/CharaInfo_List_Skill_06_nanairo/Disable").gameObject;
			this.disableNanairo.SetActive(false);
			this.skillNanairo = baseTr.Find("Base/Window/CharaInfo_List_Skill_06_nanairo").gameObject;
			this.skillNanairo.SetActive(false);
		}

		// Token: 0x0400408C RID: 16524
		public PguiOpenWindowCtrl owCtrl;

		// Token: 0x0400408D RID: 16525
		public PguiTextCtrl Txt_KindNanairo;

		// Token: 0x0400408E RID: 16526
		public PguiTextCtrl Txt_NameNanairo;

		// Token: 0x0400408F RID: 16527
		public PguiTextCtrl Txt_InfoNanairo;

		// Token: 0x04004090 RID: 16528
		public PguiTextCtrl Num_LvNanairo;

		// Token: 0x04004091 RID: 16529
		public GameObject disableNanairo;

		// Token: 0x04004092 RID: 16530
		public GameObject skillNanairo;
	}

	// Token: 0x02000A17 RID: 2583
	public class CharaGrowNanairoGUI
	{
		// Token: 0x04004093 RID: 16531
		public SelCharaGrowNanairo.WindowNanairo nanairoWindow;

		// Token: 0x04004094 RID: 16532
		public SelCharaGrowNanairo.WindowNanairoResult nanairoResultWindow;

		// Token: 0x04004095 RID: 16533
		public SelCharaGrowNanairo.NanairoReleaseTab nanairoUpTab;
	}
}
