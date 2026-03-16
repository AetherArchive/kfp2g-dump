using System;
using System.Collections.Generic;
using System.Linq;
using AEAuth3;
using UnityEngine;

public class SelCharaGrowNanairo
{
	public SelCharaGrowNanairo(Transform baseTr)
	{
		this.GrowNanairoGUI = new SelCharaGrowNanairo.CharaGrowNanairoGUI();
		GameObject gameObject = (GameObject)Resources.Load("SceneCharaEdit/GUI/Prefab/GUI_CharaEdit_Window_Nanairo");
		this.GrowNanairoGUI.nanairoWindow = new SelCharaGrowNanairo.WindowNanairo(Object.Instantiate<Transform>(gameObject.transform.Find("Window_NanairoRelease"), baseTr).transform);
		this.GrowNanairoGUI.nanairoResultWindow = new SelCharaGrowNanairo.WindowNanairoResult(Object.Instantiate<Transform>(gameObject.transform.Find("Window_TokuseiInfo"), baseTr).transform);
		this.GrowNanairoGUI.nanairoUpTab = new SelCharaGrowNanairo.NanairoReleaseTab(baseTr.Find("CharaGrow_Main").Find("Main/Right/NanairoRelease"));
	}

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

	public SelCharaGrowNanairo.CharaGrowNanairoGUI GrowNanairoGUI;

	public class NanairoItem
	{
		public IconItemCtrl iconItemCtrl;

		public PguiTextCtrl itemNum;
	}

	public class NanairoReleaseTab
	{
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

		public GameObject baseObj;

		public GameObject titleBaseObj;

		public GameObject paramBaseObj;

		public GameObject maskBaseObj;

		public static readonly int COUNT = 8;

		public List<SelCharaGrowNanairo.NanairoItem> iconItemList;

		public List<RectTransform> iconBaseList;

		public PguiTextCtrl Txt_LvInfo;

		public PguiTextCtrl Txt_PromoteInfo;

		public PguiTextCtrl Txt_PhotoPocketInfo;

		public PguiTextCtrl Txt_ArtsInfo;

		public AEImage AEImage_result;
	}

	public class WindowNanairo
	{
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

		private static readonly int COUNT = 8;

		public PguiOpenWindowCtrl owCtrl;

		public PguiTextCtrl Txt_AbilityName;

		public List<IconItemCtrl> iconItemList = new List<IconItemCtrl>();

		public PguiTextCtrl ItemUse_Num;

		public PguiTextCtrl ItemOwn_Num;

		public CharaUtil.GUISkillInfo AbilityInfo;
	}

	public class WindowNanairoResult
	{
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

		public PguiOpenWindowCtrl owCtrl;

		public PguiTextCtrl Txt_KindNanairo;

		public PguiTextCtrl Txt_NameNanairo;

		public PguiTextCtrl Txt_InfoNanairo;

		public PguiTextCtrl Num_LvNanairo;

		public GameObject disableNanairo;

		public GameObject skillNanairo;
	}

	public class CharaGrowNanairoGUI
	{
		public SelCharaGrowNanairo.WindowNanairo nanairoWindow;

		public SelCharaGrowNanairo.WindowNanairoResult nanairoResultWindow;

		public SelCharaGrowNanairo.NanairoReleaseTab nanairoUpTab;
	}
}
