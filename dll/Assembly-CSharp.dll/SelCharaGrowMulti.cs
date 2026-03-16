using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SGNFW.HttpRequest.Protocol;
using UnityEngine;
using UnityEngine.UI;

public class SelCharaGrowMulti
{
	public SelCharaGrowMulti(Transform baseTr)
	{
		this.GrowMultiGUI = new SelCharaGrowMulti.CharaGrowMultiGUI();
		GameObject gameObject = (GameObject)Resources.Load("SceneCharaEdit/GUI/Prefab/GUI_CharaEdit_Window_Multi");
		this.GrowMultiGUI.growMultiSelectWindow = new SelCharaGrowMulti.WindowGrowMultiSelect(Object.Instantiate<Transform>(gameObject.transform.Find("Window_GrowMulti_Select"), baseTr).transform);
		this.GrowMultiGUI.growMultiCheckWindow = new SelCharaGrowMulti.WindowGrowMultiCheck(Object.Instantiate<Transform>(gameObject.transform.Find("Window_GrowMulti_Check"), baseTr).transform, this);
	}

	public bool IsSelected(SelCharaGrowMulti.GrowCategory key)
	{
		return this.GrowMultiGUI.growMultiSelectWindow.toggleButtonMap[key].GetToggleIndex() == 1;
	}

	public void UpdateGrowMulti(SelCharaGrowCtrl selCharaGrowCtrl)
	{
		bool flag = this.GrowMultiGUI.growMultiCheckWindow.owCtrl.FinishedOpen();
		this.GrowMultiGUI.growMultiCheckWindow.infoScrollRect.inertia = flag;
		if (this.GrowMultiGUI.growMultiSelectWindow.owCtrl.FinishedOpen())
		{
			Rect rect = SafeAreaScaler.GetSafeArea();
			if (this.safeArea != rect)
			{
				selCharaGrowCtrl.StartCoroutine(this.GrowMultiGUI.growMultiSelectWindow.DispMessageMark());
			}
			this.safeArea = rect;
		}
	}

	public static bool UpdateItemInputList(List<ItemInput> itemList, int itemId, int num)
	{
		if (itemId == 0)
		{
			return true;
		}
		if (num <= 0)
		{
			return true;
		}
		if (DataManagerItem.GetUserHaveNum(itemId) - num < 0)
		{
			return false;
		}
		ItemInput itemInput = itemList.Find((ItemInput x) => x.itemId == itemId);
		if (itemInput == null)
		{
			itemInput = new ItemInput
			{
				itemId = itemId
			};
			itemList.Add(itemInput);
		}
		else if (DataManagerItem.GetUserHaveNum(itemId) - itemInput.num - num < 0)
		{
			return false;
		}
		itemInput.num += num;
		return true;
	}

	private const int TOGGLE_OFF = 0;

	public const int TOGGLE_ON = 1;

	public SelCharaGrowMulti.CharaGrowMultiGUI GrowMultiGUI;

	private Rect safeArea;

	public enum GrowCategory
	{
		Level,
		Wild,
		Rank,
		Arts,
		Nanairo,
		LevelLimit = 10,
		KizunaLimit
	}

	public class CharaGrowMultiGUI
	{
		public SelCharaGrowMulti.WindowGrowMultiSelect growMultiSelectWindow;

		public SelCharaGrowMulti.WindowGrowMultiCheck growMultiCheckWindow;
	}

	public class WindowGrowMultiSelect
	{
		public WindowGrowMultiSelect(Transform baseTr)
		{
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.noItemMessage = baseTr.Find("Base/Window/NoItemMessage").GetComponent<PguiTextCtrl>();
			this.mainContent = baseTr.Find("Base/Window/MainContent").gameObject;
			this.selectAllBtn = this.mainContent.transform.Find("Btn_SelectAll/Btn_Base").GetComponent<PguiButtonCtrl>();
			this.selectAllBtnTxt = this.selectAllBtn.transform.Find("BaseImage/Txt").GetComponent<PguiTextCtrl>();
			this.yellowBadgeList.Add(this.mainContent.transform.Find("Message2/Mark_YellowBadge").GetComponent<PguiAECtrl>());
			foreach (object obj in Enum.GetValues(typeof(SelCharaGrowMulti.GrowCategory)))
			{
				SelCharaGrowMulti.GrowCategory growCategory = (SelCharaGrowMulti.GrowCategory)obj;
				bool flag = growCategory < SelCharaGrowMulti.GrowCategory.LevelLimit;
				string text = string.Format("{0}/Btn{1:D2}", flag ? "PowerUp" : "LimitOver", (int)growCategory);
				PguiToggleButtonCtrl component = this.mainContent.transform.Find(text).GetComponent<PguiToggleButtonCtrl>();
				component.AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickToggleBtn));
				this.toggleButtonMap[growCategory] = component;
				this.yellowBadgeList.Add(component.transform.Find("Mark_YellowBadge").GetComponent<PguiAECtrl>());
			}
		}

		public void InitContent(List<bool> enhanceList)
		{
			this.owCtrl.choiceR.GetComponent<PguiButtonCtrl>().SetActEnable(false, false, false);
			bool flag = false;
			for (int i = 0; i < Enum.GetValues(typeof(SelCharaGrowCtrl.TabType)).Length; i++)
			{
				if (i != 5)
				{
					bool flag2 = (i != 2 || this.cpd.dynamicData.rank != this.cpd.staticData.baseData.rankHigh) && enhanceList[i];
					if (flag2)
					{
						flag = true;
						break;
					}
				}
			}
			this.noItemMessage.gameObject.SetActive(!flag);
			this.mainContent.SetActive(flag);
			if (!flag)
			{
				return;
			}
			this.selectAllBtn.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickSelectAllBtn), PguiButtonCtrl.SoundType.DEFAULT);
			this.selectAllBtnTxt.text = "すべて選択";
			foreach (object obj in Enum.GetValues(typeof(SelCharaGrowMulti.GrowCategory)))
			{
				SelCharaGrowMulti.GrowCategory growCategory = (SelCharaGrowMulti.GrowCategory)obj;
				PguiToggleButtonCtrl pguiToggleButtonCtrl = this.toggleButtonMap[growCategory];
				pguiToggleButtonCtrl.SetToggleIndex(0);
				Transform transform = pguiToggleButtonCtrl.transform.Find("Mark_YellowBadge");
				bool flag3 = false;
				switch (growCategory)
				{
				case SelCharaGrowMulti.GrowCategory.Level:
					flag3 = this.cpd.dynamicData.level < this.cpd.dynamicData.limitLevel;
					break;
				case SelCharaGrowMulti.GrowCategory.Wild:
					flag3 = this.cpd.dynamicData.promoteNum < this.cpd.staticData.maxPromoteNum;
					break;
				case SelCharaGrowMulti.GrowCategory.Rank:
					flag3 = this.cpd.dynamicData.rank < this.cpd.staticData.baseData.rankHigh;
					break;
				case SelCharaGrowMulti.GrowCategory.Arts:
					flag3 = this.cpd.dynamicData.artsLevel < this.cpd.dynamicData.limitMiracleLevel;
					break;
				case SelCharaGrowMulti.GrowCategory.Nanairo:
					flag3 = this.cpd.IsHaveNanairoAbility && !this.cpd.dynamicData.nanairoAbilityReleaseFlag && this.cpd.IsNanairoAbilityReleaseAvailable;
					break;
				case SelCharaGrowMulti.GrowCategory.LevelLimit:
					flag3 = this.cpd.dynamicData.limitLevel <= this.cpd.dynamicData.level && 6 <= this.cpd.dynamicData.rank && DataManager.DmChara.GetLevelLimitData(this.cpd.dynamicData.levelLimitId + 1) != null;
					break;
				case SelCharaGrowMulti.GrowCategory.KizunaLimit:
				{
					GameLevelInfo gameLevelInfo = DataManager.DmServerMst.gameLevelInfoList.Find((GameLevelInfo x) => x.level == this.cpd.dynamicData.KizunaLimitLevel + 1);
					flag3 = this.cpd.dynamicData.KizunaLimitLevel <= this.cpd.dynamicData.kizunaLevel && gameLevelInfo != null && gameLevelInfo.kizunaLevelExp.ContainsKey(this.cpd.staticData.baseData.kizunaLevelId);
					break;
				}
				}
				transform.gameObject.SetActive(flag3);
				pguiToggleButtonCtrl.SetActEnable(flag3);
			}
		}

		public IEnumerator DispMessageMark()
		{
			while (!this.owCtrl.FinishedOpen())
			{
				yield return null;
			}
			if (!this.mainContent.transform.Find("Message2").gameObject.activeInHierarchy)
			{
				yield break;
			}
			Canvas.ForceUpdateCanvases();
			Text text = this.mainContent.transform.Find("Message2").GetComponent<PguiTextCtrl>().m_Text;
			IList<UIVertex> verts = text.cachedTextGenerator.verts;
			int num = (text.text.IndexOf("※\u3000マーク") + 1) * 4;
			UIVertex uivertex = verts[num];
			UIVertex uivertex2 = verts[num + 7];
			uivertex.position /= text.pixelsPerUnit;
			uivertex2.position /= text.pixelsPerUnit;
			Vector3 vector = (uivertex.position + uivertex2.position) / 2f;
			vector = new Vector3(vector.x, vector.y * 2.4f, vector.z);
			this.yellowBadgeList[0].GetComponent<RectTransform>().localPosition = vector;
			this.yellowBadgeList[0].gameObject.SetActive(true);
			using (List<PguiAECtrl>.Enumerator enumerator = this.yellowBadgeList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PguiAECtrl pguiAECtrl = enumerator.Current;
					if (pguiAECtrl.gameObject.activeSelf)
					{
						pguiAECtrl.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
					}
				}
				yield break;
			}
			yield break;
		}

		public void OnClickSelectAllBtn(PguiButtonCtrl btn)
		{
			if (!this.toggleButtonMap.Values.Any<PguiToggleButtonCtrl>((PguiToggleButtonCtrl x) => x.ActEnable && x.GetToggleIndex() == 0))
			{
				foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl in this.toggleButtonMap.Values)
				{
					if (pguiToggleButtonCtrl.ActEnable)
					{
						pguiToggleButtonCtrl.SetToggleIndex(0);
					}
				}
				this.OnClickToggleBtn(null, 0);
				return;
			}
			for (;;)
			{
				if (!this.toggleButtonMap.Values.Any<PguiToggleButtonCtrl>((PguiToggleButtonCtrl x) => x.ActEnable && x.GetToggleIndex() == 0))
				{
					break;
				}
				foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl2 in this.toggleButtonMap.Values)
				{
					if (pguiToggleButtonCtrl2.ActEnable)
					{
						pguiToggleButtonCtrl2.SetToggleIndex(1);
					}
				}
				this.OnClickToggleBtn(null, 0);
			}
		}

		public bool OnClickToggleBtn(PguiToggleButtonCtrl btn, int index)
		{
			while (this.UpdateToggleButton(btn, index))
			{
			}
			this.owCtrl.choiceR.GetComponent<PguiButtonCtrl>().SetActEnable(this.toggleButtonMap.Values.Any<PguiToggleButtonCtrl>((PguiToggleButtonCtrl x) => this.IsToggleOn(x, btn, index)), false, false);
			foreach (PguiAECtrl pguiAECtrl in this.yellowBadgeList)
			{
				if (pguiAECtrl.gameObject.activeSelf)
				{
					pguiAECtrl.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
				}
			}
			if (this.toggleButtonMap.Values.Any<PguiToggleButtonCtrl>((PguiToggleButtonCtrl x) => x.ActEnable && !this.IsToggleOn(x, btn, index)))
			{
				this.selectAllBtnTxt.text = "すべて選択";
			}
			else
			{
				this.selectAllBtnTxt.text = "すべて解除";
			}
			return true;
		}

		private bool UpdateToggleButton(PguiToggleButtonCtrl btn, int index)
		{
			bool flag = false;
			bool flag2 = this.cpd.dynamicData.level < this.cpd.dynamicData.limitLevel || this.IsToggleOn(this.toggleButtonMap[SelCharaGrowMulti.GrowCategory.Rank], btn, index) || this.IsToggleOn(this.toggleButtonMap[SelCharaGrowMulti.GrowCategory.LevelLimit], btn, index);
			this.toggleButtonMap[SelCharaGrowMulti.GrowCategory.Level].SetActEnable(flag2);
			this.toggleButtonMap[SelCharaGrowMulti.GrowCategory.Level].transform.Find("Mark_YellowBadge").gameObject.SetActive(flag2);
			if (!flag2)
			{
				if (this.toggleButtonMap[SelCharaGrowMulti.GrowCategory.Level].GetToggleIndex() == 1)
				{
					flag = true;
				}
				this.toggleButtonMap[SelCharaGrowMulti.GrowCategory.Level].SetToggleIndex(0);
			}
			bool flag3 = this.cpd.IsHaveNanairoAbility && !this.cpd.dynamicData.nanairoAbilityReleaseFlag && (this.cpd.IsNanairoConditionPPOk() && (this.cpd.IsNanairoConditionArtsOk() || (3 <= this.cpd.dynamicData.limitMiracleLevel && this.IsToggleOn(this.toggleButtonMap[SelCharaGrowMulti.GrowCategory.Arts], btn, index))) && (this.cpd.IsNanairoConditionLevelOk() || (90 <= this.cpd.dynamicData.limitLevel && this.IsToggleOn(this.toggleButtonMap[SelCharaGrowMulti.GrowCategory.Level], btn, index)) || (this.IsToggleOn(this.toggleButtonMap[SelCharaGrowMulti.GrowCategory.Rank], btn, index) && this.IsToggleOn(this.toggleButtonMap[SelCharaGrowMulti.GrowCategory.Level], btn, index)))) && (this.cpd.IsNanairoConditionPromoteOk() || this.IsToggleOn(this.toggleButtonMap[SelCharaGrowMulti.GrowCategory.Wild], btn, index));
			this.toggleButtonMap[SelCharaGrowMulti.GrowCategory.Nanairo].SetActEnable(flag3);
			this.toggleButtonMap[SelCharaGrowMulti.GrowCategory.Nanairo].transform.Find("Mark_YellowBadge").gameObject.SetActive(flag3);
			if (!flag3)
			{
				if (this.toggleButtonMap[SelCharaGrowMulti.GrowCategory.Nanairo].GetToggleIndex() == 1)
				{
					flag = true;
				}
				this.toggleButtonMap[SelCharaGrowMulti.GrowCategory.Nanairo].SetToggleIndex(0);
			}
			bool flag4 = ((90 <= this.cpd.dynamicData.level && this.cpd.dynamicData.limitLevel <= this.cpd.dynamicData.level) || this.IsToggleOn(this.toggleButtonMap[SelCharaGrowMulti.GrowCategory.Level], btn, index)) && (6 <= this.cpd.dynamicData.rank || this.IsToggleOn(this.toggleButtonMap[SelCharaGrowMulti.GrowCategory.Rank], btn, index)) && DataManager.DmChara.GetLevelLimitData(this.cpd.dynamicData.levelLimitId + 1) != null;
			this.toggleButtonMap[SelCharaGrowMulti.GrowCategory.LevelLimit].SetActEnable(flag4);
			this.toggleButtonMap[SelCharaGrowMulti.GrowCategory.LevelLimit].transform.Find("Mark_YellowBadge").gameObject.SetActive(flag4);
			if (!flag4)
			{
				if (this.toggleButtonMap[SelCharaGrowMulti.GrowCategory.LevelLimit].GetToggleIndex() == 1)
				{
					flag = true;
				}
				this.toggleButtonMap[SelCharaGrowMulti.GrowCategory.LevelLimit].SetToggleIndex(0);
			}
			return flag;
		}

		private bool IsToggleOn(PguiToggleButtonCtrl targetBtn, PguiToggleButtonCtrl thisBtn, int thisIndex)
		{
			if (thisBtn == targetBtn)
			{
				return thisIndex == 0;
			}
			return targetBtn.GetToggleIndex() == 1;
		}

		public PguiOpenWindowCtrl owCtrl;

		public PguiTextCtrl noItemMessage;

		public GameObject mainContent;

		public PguiButtonCtrl selectAllBtn;

		public PguiTextCtrl selectAllBtnTxt;

		public Dictionary<SelCharaGrowMulti.GrowCategory, PguiToggleButtonCtrl> toggleButtonMap = new Dictionary<SelCharaGrowMulti.GrowCategory, PguiToggleButtonCtrl>();

		private readonly List<PguiAECtrl> yellowBadgeList = new List<PguiAECtrl>();

		public CharaPackData cpd;
	}

	public class WindowGrowMultiCheck
	{
		public WindowGrowMultiCheck(Transform baseTr, SelCharaGrowMulti selCharaGrowMulti)
		{
			this.selCharaGrowMulti = selCharaGrowMulti;
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.infoScrollRect = baseTr.Find("Base/Window/CheckInfo/ScrollView").GetComponent<ScrollRect>();
			this.scrollContent = this.infoScrollRect.transform.Find("Viewport/Content").gameObject;
			this.messageInfoObj = this.scrollContent.transform.Find("Panel/MessageInfo").gameObject;
			this.notExecMsg = this.messageInfoObj.transform.Find("Message").GetComponent<PguiTextCtrl>();
			this.iconChara = this.scrollContent.transform.Find("Panel/CharaInfo/Icon_Chara_Before/Icon_Chara").GetComponent<IconCharaCtrl>();
			foreach (object obj in Enum.GetValues(typeof(SelCharaGrowMulti.GrowCategory)))
			{
				SelCharaGrowMulti.GrowCategory growCategory = (SelCharaGrowMulti.GrowCategory)obj;
				this.execMap[growCategory] = false;
				this.useItemMap[growCategory] = new List<ItemInput>();
				if (growCategory != SelCharaGrowMulti.GrowCategory.LevelLimit)
				{
					string text = growCategory.ToString().Replace("Limit", "");
					Transform transform = this.scrollContent.transform.Find("Panel/CharaInfo/Txt_Area/" + text);
					this.categoryTxtMap[growCategory] = transform.Find("Category_Txt").GetComponent<PguiTextCtrl>();
					this.beforeTxtMap[growCategory] = transform.Find("Num_Before").GetComponent<PguiTextCtrl>();
					this.afterTxtMap[growCategory] = transform.Find("Num_After").GetComponent<PguiTextCtrl>();
					if (growCategory == SelCharaGrowMulti.GrowCategory.Rank)
					{
						for (int i = 1; i <= 6; i++)
						{
							this.beforeStarAll.Add(transform.Find(string.Format("Num_Before/Icon_StarAll/Icon_Star{0:D2}", i)).GetComponent<PguiImageCtrl>());
							this.afterStarAll.Add(transform.Find(string.Format("Num_After/Icon_StarAll/Icon_Star{0:D2}", i)).GetComponent<PguiImageCtrl>());
							this.addStarAll.Add(transform.Find(string.Format("Num_After/Icon_StarAll/Icon_Star{0:D2}/Icon_Add", i)).GetComponent<PguiImageCtrl>());
						}
					}
				}
			}
			this.itemUseInfoLayout = this.scrollContent.transform.Find("Panel/ItemUseInfo").GetComponent<LayoutElement>();
			this.itemUseInfoRowListObj = this.itemUseInfoLayout.transform.Find("RowList").gameObject;
			this.itemUseInfoRowOriginObj = this.itemUseInfoRowListObj.transform.Find("RowOrigin").gameObject;
			this.paramObj = this.scrollContent.transform.Find("Panel/ParamAll").gameObject;
			for (int j = 0; j < 5; j++)
			{
				this.paramAll.Add(this.paramObj.transform.Find("Info" + (j + 1).ToString("D2") + "/Num_After").GetComponent<PguiTextCtrl>());
			}
			this.paramAll.Add(this.paramObj.transform.Find("Info_Flag_Beat/Num_After").GetComponent<PguiTextCtrl>());
			this.paramAll.Add(this.paramObj.transform.Find("Info_Flag_Action/Num_After").GetComponent<PguiTextCtrl>());
			this.paramAll.Add(this.paramObj.transform.Find("Info_Flag_Try/Num_After").GetComponent<PguiTextCtrl>());
			this.artsInfoObj = this.scrollContent.transform.Find("Panel/ArtsInfo").gameObject;
			this.artsNameTxt = this.artsInfoObj.transform.Find("Txt_ArtsName").GetComponent<PguiTextCtrl>();
			this.artsInfoBefore = new CharaUtil.GUISkillInfo(this.artsInfoObj.transform.Find("CharaInfo_List_Skill_Short_Before"));
			this.artsInfoAfter = new CharaUtil.GUISkillInfo(this.artsInfoObj.transform.Find("CharaInfo_List_Skill_Short_After"));
			this.nanairoInfoObj = this.scrollContent.transform.Find("Panel/NanairoInfo").gameObject;
			this.nanairoNameTxt = this.nanairoInfoObj.transform.Find("Txt_AbilityName").GetComponent<PguiTextCtrl>();
			this.nanairoInfo = new CharaUtil.GUISkillInfo(this.nanairoInfoObj.transform.Find("CharaInfo_List_Skill_06_nanairo"));
		}

		public void SetUpGrowMultiContent(CharaPackData cpd)
		{
			this.iconChara.Setup(cpd, SortFilterDefine.SortType.LEVEL, false, null, 0, -1, 0);
			foreach (PguiTextCtrl pguiTextCtrl in this.categoryTxtMap.Values)
			{
				pguiTextCtrl.text = pguiTextCtrl.text.Replace("<color=#FF7B16>", "");
				pguiTextCtrl.text = pguiTextCtrl.text.Replace("</color>", "");
			}
			this.Reset();
			this.beforeTxtMap[SelCharaGrowMulti.GrowCategory.Level].text = (this.afterTxtMap[SelCharaGrowMulti.GrowCategory.Level].text = "<size=22>Lv.</size>" + cpd.dynamicData.level.ToString() + "/" + cpd.dynamicData.limitLevel.ToString());
			this.beforeTxtMap[SelCharaGrowMulti.GrowCategory.Wild].text = (this.afterTxtMap[SelCharaGrowMulti.GrowCategory.Wild].text = cpd.dynamicData.promoteNum.ToString() + "/" + cpd.staticData.maxPromoteNum.ToString());
			for (int i = 0; i < this.beforeStarAll.Count; i++)
			{
				PguiImageCtrl pguiImageCtrl = this.beforeStarAll[i];
				PguiReplaceSpriteCtrl component = pguiImageCtrl.GetComponent<PguiReplaceSpriteCtrl>();
				component.InitForce();
				component.Replace((i < cpd.dynamicData.rank) ? 1 : 2);
				pguiImageCtrl.gameObject.SetActive(i < cpd.staticData.baseData.rankHigh);
				PguiImageCtrl pguiImageCtrl2 = this.afterStarAll[i];
				PguiReplaceSpriteCtrl component2 = pguiImageCtrl2.GetComponent<PguiReplaceSpriteCtrl>();
				component2.InitForce();
				component2.Replace((i < cpd.dynamicData.rank) ? 1 : 2);
				pguiImageCtrl2.gameObject.SetActive(i < cpd.staticData.baseData.rankHigh);
				this.addStarAll[i].gameObject.SetActive(false);
			}
			this.beforeTxtMap[SelCharaGrowMulti.GrowCategory.Arts].text = (this.afterTxtMap[SelCharaGrowMulti.GrowCategory.Arts].text = "<size=22>Lv.</size>" + cpd.dynamicData.artsLevel.ToString());
			this.beforeTxtMap[SelCharaGrowMulti.GrowCategory.Nanairo].text = (this.afterTxtMap[SelCharaGrowMulti.GrowCategory.Nanairo].text = (cpd.dynamicData.nanairoAbilityReleaseFlag ? "解放" : "未解放"));
			this.beforeTxtMap[SelCharaGrowMulti.GrowCategory.KizunaLimit].text = (this.afterTxtMap[SelCharaGrowMulti.GrowCategory.KizunaLimit].text = "<size=22>Lv.</size>" + cpd.dynamicData.KizunaLimitLevel.ToString());
			this.afterLevel = cpd.dynamicData.level;
			this.afterPromoteNum = cpd.dynamicData.promoteNum;
			this.afterPromoteFlag = new List<bool>(cpd.dynamicData.promoteFlag);
			this.afterRank = cpd.dynamicData.rank;
			this.afterArtsLv = cpd.dynamicData.artsLevel;
			this.afterLevelLimitId = cpd.dynamicData.levelLimitId;
		}

		public void SetUpLevelInfo(CharaPackData cpd, ref List<int> selectLvUpItemIdList, ref int lvUpCostCoin)
		{
			lvUpCostCoin = 0;
			SelCharaGrowMulti.GrowCategory growCategory = SelCharaGrowMulti.GrowCategory.Level;
			int num = CharaPackData.CalcLimitLevel(cpd.id, this.afterRank, this.afterLevelLimitId);
			selectLvUpItemIdList = this.CalcLvUpItem(cpd, num);
			if (selectLvUpItemIdList != null)
			{
				List<ItemInput> list = new List<ItemInput>();
				List<ItemData> expAddItemList = DataManager.DmItem.GetUserItemListByKind(ItemDef.Kind.EXP_ADD);
				int j;
				int i;
				Predicate<int> <>9__0;
				for (i = 0; i < expAddItemList.Count; i = j + 1)
				{
					List<int> list2 = selectLvUpItemIdList;
					Predicate<int> predicate;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = (int n) => n == expAddItemList[i].id);
					}
					List<int> list3 = list2.FindAll(predicate);
					if (list3.Count != 0)
					{
						ItemInput itemInput = new ItemInput
						{
							itemId = expAddItemList[i].id,
							num = list3.Count
						};
						list.Add(itemInput);
						SelCharaGrowMulti.UpdateItemInputList(this.useItemList, itemInput.itemId, itemInput.num);
						SelCharaGrowMulti.UpdateItemInputList(this.useItemMap[growCategory], itemInput.itemId, itemInput.num);
					}
					j = i;
				}
				DataManagerChara.SimulateAddExpResult simulateAddExpResult = DataManager.DmChara.SimulateAddExp(cpd, list);
				SelCharaGrowMulti.UpdateItemInputList(this.useItemList, 30101, simulateAddExpResult.costGold);
				SelCharaGrowMulti.UpdateItemInputList(this.useItemMap[growCategory], 30101, simulateAddExpResult.costGold);
				lvUpCostCoin = simulateAddExpResult.costGold;
				if (this.useItemMap[growCategory].Count > 0)
				{
					string text = ((cpd.dynamicData.limitLevel != num) ? string.Format("<color=#FF7B16>{0}</color>", num) : num.ToString());
					this.categoryTxtMap[growCategory].text = this.categoryTxtMap[growCategory].text.Replace("レベル/", "<color=#FF7B16>レベル</color>/");
					this.afterTxtMap[growCategory].text = "<size=22>Lv.</size><color=#FF7B16>" + simulateAddExpResult.level.ToString() + "</color>/" + text;
					this.afterLevel = simulateAddExpResult.level;
				}
			}
			else
			{
				selectLvUpItemIdList = new List<int>();
			}
			this.execMap[growCategory] = this.useItemMap[growCategory].Count > 0;
		}

		private List<int> CalcLvUpItem(CharaPackData cpd, int limitLevel)
		{
			List<DataManagerServerMst.CharaLevelItem> list = cpd.LevelItemUseOrderList(false);
			long num = 0L;
			foreach (DataManagerServerMst.CharaLevelItem charaLevelItem in list)
			{
				ItemData userItemData = DataManager.DmItem.GetUserItemData(charaLevelItem.itemId);
				long num2 = ((cpd.staticData.baseData.attribute == charaLevelItem.attribute) ? charaLevelItem.attributeExp : charaLevelItem.exp);
				num += (long)userItemData.num * num2;
			}
			long num3 = 0L;
			long num4 = 0L;
			for (int i = cpd.dynamicData.level; i < limitLevel; i++)
			{
				num3 += DataManager.DmChara.GetExpByNextLevel(cpd.id, i);
				if (num3 > num + cpd.dynamicData.exp)
				{
					break;
				}
				num4 = (long)(i + 1);
			}
			List<int> list2 = null;
			ItemInput itemInput = this.useItemList.Find((ItemInput x) => x.itemId == 30101);
			int num5 = DataManager.DmItem.GetUserItemData(30101).num - ((itemInput != null) ? itemInput.num : 0);
			if (num5 <= 0)
			{
				return null;
			}
			while (num4 > (long)cpd.dynamicData.level)
			{
				list2 = cpd.CalcEatLevelItem((int)num4, num5, false);
				if (list2 != null && list2.Count > 0)
				{
					break;
				}
				num4 -= 1L;
			}
			return list2;
		}

		public void SetUpWildInfo(CharaPackData cpd)
		{
			SelCharaGrowMulti.GrowCategory growCategory = SelCharaGrowMulti.GrowCategory.Wild;
			this.promoteRequestList.Clear();
			int num = cpd.dynamicData.promoteNum;
			for (int i = cpd.dynamicData.promoteNum; i < cpd.staticData.maxPromoteNum; i++)
			{
				bool flag = false;
				CharaPromotePreset charaPromotePreset = cpd.staticData.promoteList[i];
				WildResult wildResult = new WildResult
				{
					chara_id = cpd.id,
					promote_num = i
				};
				int num2 = 0;
				for (int j = 0; j < charaPromotePreset.promoteOneList.Count; j++)
				{
					if (i == cpd.dynamicData.promoteNum && cpd.dynamicData.promoteFlag[j])
					{
						num2++;
					}
					else
					{
						CharaPromoteOne charaPromoteOne = charaPromotePreset.promoteOneList[j];
						if (!SelCharaGrowMulti.UpdateItemInputList(this.useItemList, 30101, charaPromoteOne.costGoldNum))
						{
							flag = true;
							break;
						}
						if (!SelCharaGrowMulti.UpdateItemInputList(this.useItemList, charaPromoteOne.promoteUseItemId, charaPromoteOne.promoteUseItemNum))
						{
							flag = true;
						}
						else
						{
							if (j == 0)
							{
								wildResult.promote_flag00 = 1;
							}
							else if (j == 1)
							{
								wildResult.promote_flag01 = 1;
							}
							else if (j == 2)
							{
								wildResult.promote_flag02 = 1;
							}
							else if (j == 3)
							{
								wildResult.promote_flag03 = 1;
							}
							else if (j == 4)
							{
								wildResult.promote_flag04 = 1;
							}
							else if (j == 5)
							{
								wildResult.promote_flag05 = 1;
							}
							SelCharaGrowMulti.UpdateItemInputList(this.useItemMap[growCategory], 30101, charaPromoteOne.costGoldNum);
							SelCharaGrowMulti.UpdateItemInputList(this.useItemMap[growCategory], charaPromoteOne.promoteUseItemId, charaPromoteOne.promoteUseItemNum);
							this.afterPromoteFlag[j] = true;
						}
					}
				}
				if (wildResult.promote_flag00 == 1 || wildResult.promote_flag01 == 1 || wildResult.promote_flag02 == 1 || wildResult.promote_flag03 == 1 || wildResult.promote_flag04 == 1 || wildResult.promote_flag05 == 1 || num2 == charaPromotePreset.promoteOneList.Count)
				{
					this.promoteRequestList.Add(wildResult);
				}
				if (flag)
				{
					break;
				}
				num = i + 1;
				for (int k = 0; k < this.afterPromoteFlag.Count; k++)
				{
					this.afterPromoteFlag[k] = false;
				}
			}
			if (this.useItemMap[growCategory].Count > 0 || num > cpd.dynamicData.promoteNum)
			{
				this.categoryTxtMap[growCategory].text = "<color=#FF7B16>野生解放</color>";
				this.afterTxtMap[growCategory].text = "<color=#FF7B16>" + num.ToString() + "</color>/" + cpd.staticData.maxPromoteNum.ToString();
				this.afterPromoteNum = num;
				this.paramObj.SetActive(true);
				this.execMap[growCategory] = true;
				return;
			}
			this.execMap[growCategory] = false;
		}

		public void SetUpRankInfo(CharaPackData cpd, ref List<int> selectLvUpItemIdList, ref int lvUpCostCoin)
		{
			SelCharaGrowMulti.GrowCategory growCategory = SelCharaGrowMulti.GrowCategory.Rank;
			for (int i = cpd.dynamicData.rank; i < cpd.staticData.baseData.rankHigh; i++)
			{
				GrowItemData nextItemByRankup = cpd.GetNextItemByRankup(i);
				if (nextItemByRankup == null || !SelCharaGrowMulti.UpdateItemInputList(this.useItemList, 30101, nextItemByRankup.needGold) || !SelCharaGrowMulti.UpdateItemInputList(this.useItemList, nextItemByRankup.item.id, nextItemByRankup.item.num))
				{
					break;
				}
				SelCharaGrowMulti.UpdateItemInputList(this.useItemMap[growCategory], 30101, nextItemByRankup.needGold);
				SelCharaGrowMulti.UpdateItemInputList(this.useItemMap[growCategory], nextItemByRankup.item.id, nextItemByRankup.item.num);
				this.afterRank = i + 1;
				if (this.selCharaGrowMulti.IsSelected(SelCharaGrowMulti.GrowCategory.Level))
				{
					this.useItemMap[SelCharaGrowMulti.GrowCategory.Level].Clear();
					this.UpdateUseItemList();
					this.SetUpLevelInfo(cpd, ref selectLvUpItemIdList, ref lvUpCostCoin);
				}
			}
			if (this.afterRank > cpd.dynamicData.rank)
			{
				this.categoryTxtMap[growCategory].text = "<color=#FF7B16>けも級</color>";
				for (int j = 0; j < this.afterStarAll.Count; j++)
				{
					PguiImageCtrl pguiImageCtrl = this.afterStarAll[j];
					PguiReplaceSpriteCtrl component = pguiImageCtrl.GetComponent<PguiReplaceSpriteCtrl>();
					component.InitForce();
					component.Replace((j < cpd.dynamicData.rank) ? 1 : 2);
					pguiImageCtrl.gameObject.SetActive(j < cpd.staticData.baseData.rankHigh);
					PguiImageCtrl pguiImageCtrl2 = this.addStarAll[j];
					pguiImageCtrl2.GetComponent<uGUITweenColor>().Reset();
					pguiImageCtrl2.gameObject.SetActive(cpd.dynamicData.rank <= j && j < this.afterRank);
				}
				this.paramObj.SetActive(true);
				if (!this.execMap[SelCharaGrowMulti.GrowCategory.Level] && !this.execMap[SelCharaGrowMulti.GrowCategory.LevelLimit])
				{
					int num = CharaPackData.CalcLimitLevel(cpd.id, this.afterRank, this.afterLevelLimitId);
					string text = ((cpd.dynamicData.limitLevel != num) ? string.Format("<color=#FF7B16>{0}</color>", num) : num.ToString());
					this.afterTxtMap[SelCharaGrowMulti.GrowCategory.Level].text = "<size=22>Lv.</size>" + this.afterLevel.ToString() + "/" + text;
				}
				this.execMap[growCategory] = true;
				return;
			}
			this.execMap[growCategory] = false;
		}

		public void SetUpArtsInfo(CharaPackData cpd)
		{
			SelCharaGrowMulti.GrowCategory growCategory = SelCharaGrowMulti.GrowCategory.Arts;
			for (int i = cpd.dynamicData.artsLevel; i < cpd.dynamicData.limitMiracleLevel; i++)
			{
				GrowItemList nextItemByArtsUp = cpd.GetNextItemByArtsUp(i);
				if (nextItemByArtsUp == null || !SelCharaGrowMulti.UpdateItemInputList(this.useItemList, 30101, nextItemByArtsUp.needGold))
				{
					break;
				}
				bool flag = false;
				foreach (ItemData itemData in nextItemByArtsUp.itemList)
				{
					if (!SelCharaGrowMulti.UpdateItemInputList(this.useItemList, itemData.id, itemData.num))
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					break;
				}
				SelCharaGrowMulti.UpdateItemInputList(this.useItemMap[growCategory], 30101, nextItemByArtsUp.needGold);
				foreach (ItemData itemData2 in nextItemByArtsUp.itemList)
				{
					SelCharaGrowMulti.UpdateItemInputList(this.useItemMap[growCategory], itemData2.id, itemData2.num);
				}
				this.afterArtsLv = i + 1;
			}
			if (this.afterArtsLv > cpd.dynamicData.artsLevel)
			{
				this.categoryTxtMap[growCategory].text = "<color=#FF7B16>けものミラクル</color>";
				this.afterTxtMap[growCategory].text = "<size=22>Lv.</size><color=#FF7B16>" + this.afterArtsLv.ToString() + "</color>";
				this.artsInfoObj.SetActive(true);
				this.artsNameTxt.text = cpd.staticData.artsData.actionName;
				this.artsInfoBefore.Setup(new CharaUtil.GUISkillInfo.SetupParam
				{
					charaPackData = cpd,
					maxDisp = false,
					type = CharaUtil.GUISkillInfo.Type.KemonoMiracle
				});
				this.artsInfoAfter.Setup(new CharaUtil.GUISkillInfo.SetupParam
				{
					charaPackData = cpd,
					maxDisp = false,
					type = CharaUtil.GUISkillInfo.Type.KemonoMiracle,
					offsetKemonoMiracleLv = this.afterArtsLv - cpd.dynamicData.artsLevel
				});
				int num = Math.Max(this.artsInfoBefore.Txt_Info.text.Split('\n', StringSplitOptions.None).Length, this.artsInfoAfter.Txt_Info.text.Split('\n', StringSplitOptions.None).Length);
				this.artsInfoBefore.baseObj.GetComponent<RectTransform>().sizeDelta = new Vector2(550f, (float)Math.Max(150, 150 + (num - 4) * 20));
				this.artsInfoAfter.baseObj.GetComponent<RectTransform>().sizeDelta = new Vector2(550f, (float)Math.Max(150, 150 + (num - 4) * 20));
				this.artsInfoObj.GetComponent<LayoutElement>().preferredHeight = (float)Math.Max(310, 310 + (num - 4) * 20);
				this.execMap[growCategory] = true;
				return;
			}
			this.execMap[growCategory] = false;
		}

		public void SetUpNanairoInfo(CharaPackData cpd)
		{
			SelCharaGrowMulti.GrowCategory growCategory = SelCharaGrowMulti.GrowCategory.Nanairo;
			bool flag = false;
			if (cpd.IsHaveNanairoAbility && !cpd.dynamicData.nanairoAbilityReleaseFlag && cpd.IsNanairoConditionPPOk() && (cpd.IsNanairoConditionLevelOk() || this.afterLevel >= 90) && (cpd.IsNanairoConditionPromoteOk() || this.afterPromoteNum >= 4) && (cpd.IsNanairoConditionArtsOk() || this.afterArtsLv >= 3))
			{
				GrowItemList releaseItemByNanairoAbilityRelease = cpd.GetReleaseItemByNanairoAbilityRelease();
				flag = SelCharaGrowMulti.UpdateItemInputList(this.useItemList, 30101, releaseItemByNanairoAbilityRelease.needGold);
				if (flag)
				{
					foreach (ItemData itemData in releaseItemByNanairoAbilityRelease.itemList)
					{
						if (!SelCharaGrowMulti.UpdateItemInputList(this.useItemList, itemData.id, itemData.num))
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					SelCharaGrowMulti.UpdateItemInputList(this.useItemMap[growCategory], 30101, releaseItemByNanairoAbilityRelease.needGold);
					foreach (ItemData itemData2 in releaseItemByNanairoAbilityRelease.itemList)
					{
						SelCharaGrowMulti.UpdateItemInputList(this.useItemMap[growCategory], itemData2.id, itemData2.num);
					}
				}
			}
			if (flag)
			{
				this.categoryTxtMap[growCategory].text = "<color=#FF7B16>なないろとくせい</color>";
				this.afterTxtMap[growCategory].text = "<color=#FF7B16>解放</color>";
				this.nanairoInfoObj.SetActive(true);
				this.nanairoNameTxt.text = cpd.staticData.nanairoAbilityData.abilityName;
				this.nanairoInfo.Setup(new CharaUtil.GUISkillInfo.SetupParam
				{
					charaPackData = cpd,
					maxDisp = false,
					type = CharaUtil.GUISkillInfo.Type.NanairoAbility
				});
				this.execMap[growCategory] = true;
				return;
			}
			this.execMap[growCategory] = false;
		}

		public void SetUpLevelLimitInfo(CharaPackData cpd, ref List<int> selectLvUpItemIdList, ref int lvUpCostCoin)
		{
			SelCharaGrowMulti.GrowCategory growCategory = SelCharaGrowMulti.GrowCategory.LevelLimit;
			if (6 <= this.afterRank)
			{
				int num = cpd.dynamicData.levelLimitId;
				while (DataManager.DmChara.GetLevelLimitData(num + 1) != null)
				{
					int num2 = CharaPackData.CalcLimitLevel(cpd.id, this.afterRank, num);
					if (this.afterLevel < num2)
					{
						break;
					}
					DataManagerChara.LevelLimitData levelLimitData = DataManager.DmChara.GetLevelLimitData(num + 1);
					if (!SelCharaGrowMulti.UpdateItemInputList(this.useItemList, 30101, levelLimitData.needGoldNum) || !SelCharaGrowMulti.UpdateItemInputList(this.useItemList, levelLimitData.needItemId01, levelLimitData.needItemNum01) || (levelLimitData.needItemId02 != 0 && !SelCharaGrowMulti.UpdateItemInputList(this.useItemList, levelLimitData.needItemId02, levelLimitData.needItemNum02)))
					{
						break;
					}
					SelCharaGrowMulti.UpdateItemInputList(this.useItemMap[growCategory], 30101, levelLimitData.needGoldNum);
					SelCharaGrowMulti.UpdateItemInputList(this.useItemMap[growCategory], levelLimitData.needItemId01, levelLimitData.needItemNum01);
					if (levelLimitData.needItemId02 != 0)
					{
						SelCharaGrowMulti.UpdateItemInputList(this.useItemMap[growCategory], levelLimitData.needItemId02, levelLimitData.needItemNum02);
					}
					this.afterLevelLimitId = num + 1;
					if (this.selCharaGrowMulti.IsSelected(SelCharaGrowMulti.GrowCategory.Level))
					{
						this.useItemMap[SelCharaGrowMulti.GrowCategory.Level].Clear();
						this.UpdateUseItemList();
						this.SetUpLevelInfo(cpd, ref selectLvUpItemIdList, ref lvUpCostCoin);
					}
					num++;
				}
			}
			if (this.afterLevelLimitId > cpd.dynamicData.levelLimitId)
			{
				SelCharaGrowMulti.GrowCategory growCategory2 = SelCharaGrowMulti.GrowCategory.Level;
				this.categoryTxtMap[growCategory2].text = this.categoryTxtMap[growCategory2].text.Replace("レベル上限", "<color=#FF7B16>レベル上限</color>");
				string text = (this.execMap[growCategory2] ? string.Format("<color=#FF7B16>{0}</color>", this.afterLevel) : this.afterLevel.ToString());
				int num3 = CharaPackData.CalcLimitLevel(cpd.id, this.afterRank, this.afterLevelLimitId);
				this.afterTxtMap[growCategory2].text = string.Concat(new string[]
				{
					"<size=22>Lv.</size>",
					text,
					"/<color=#FF7B16>",
					num3.ToString(),
					"</color>"
				});
				this.execMap[growCategory] = true;
				return;
			}
			this.execMap[growCategory] = false;
		}

		public void SetUpKizunaLevelLimitInfo(CharaPackData cpd)
		{
			SelCharaGrowMulti.GrowCategory growCategory = SelCharaGrowMulti.GrowCategory.KizunaLimit;
			bool flag = false;
			GameLevelInfo gameLevelInfo = DataManager.DmServerMst.gameLevelInfoList.Find((GameLevelInfo x) => x.level == cpd.dynamicData.KizunaLimitLevel + 1);
			if (cpd.dynamicData.KizunaLimitLevel <= cpd.dynamicData.kizunaLevel && gameLevelInfo != null && gameLevelInfo.kizunaLevelExp.ContainsKey(cpd.staticData.baseData.kizunaLevelId))
			{
				GameLevelInfo.KizunaLevelData kizunaLevelData = gameLevelInfo.kizunaLevelExp[cpd.staticData.baseData.kizunaLevelId];
				if (SelCharaGrowMulti.UpdateItemInputList(this.useItemList, kizunaLevelData.releaseItemId, kizunaLevelData.releaseItemNum))
				{
					SelCharaGrowMulti.UpdateItemInputList(this.useItemMap[growCategory], kizunaLevelData.releaseItemId, kizunaLevelData.releaseItemNum);
					flag = true;
				}
			}
			if (flag)
			{
				this.categoryTxtMap[growCategory].text = "<color=#FF7B16>なかよしレベル上限</color>";
				this.afterTxtMap[SelCharaGrowMulti.GrowCategory.KizunaLimit].text = "<size=22>Lv.</size><color=#FF7B16>" + (cpd.dynamicData.KizunaLimitLevel + 1).ToString() + "</color>";
				this.execMap[growCategory] = true;
				return;
			}
			this.execMap[growCategory] = false;
		}

		public void UpdateUseItemList()
		{
			this.useItemList.Clear();
			foreach (object obj in Enum.GetValues(typeof(SelCharaGrowMulti.GrowCategory)))
			{
				SelCharaGrowMulti.GrowCategory growCategory = (SelCharaGrowMulti.GrowCategory)obj;
				foreach (ItemInput itemInput in this.useItemMap[growCategory])
				{
					SelCharaGrowMulti.UpdateItemInputList(this.useItemList, itemInput.itemId, itemInput.num);
				}
			}
		}

		public void SortUseItemList()
		{
			this.useItemList.Clear();
			foreach (object obj in Enum.GetValues(typeof(SelCharaGrowMulti.GrowCategory)))
			{
				SelCharaGrowMulti.GrowCategory growCategory = (SelCharaGrowMulti.GrowCategory)obj;
				this.useItemMap[growCategory].Sort(delegate(ItemInput a, ItemInput b)
				{
					if (a.itemId == 30101)
					{
						return -1;
					}
					if (b.itemId == 30101)
					{
						return 1;
					}
					return a.itemId - b.itemId;
				});
				foreach (ItemInput itemInput in this.useItemMap[growCategory])
				{
					SelCharaGrowMulti.UpdateItemInputList(this.useItemList, itemInput.itemId, itemInput.num);
				}
			}
		}

		public void SetUseItemList(CharaPackData cpd)
		{
			this.SortUseItemList();
			int num = 0;
			foreach (ItemInput itemInput in this.useItemList)
			{
				int num2 = num / 8;
				if (num % 8 == 0 && this.itemUseInfoRowObjList.Count <= num2)
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.itemUseInfoRowOriginObj, this.itemUseInfoRowListObj.transform);
					gameObject.name = string.Format("Row_{0:D2}", num2);
					this.itemUseInfoRowObjList.Add(gameObject);
				}
				this.itemUseInfoRowObjList[num2].SetActive(true);
				Transform transform = this.itemUseInfoRowObjList[num2].transform.Find(string.Format("IconSet{0:D3}", num));
				if (transform == null)
				{
					transform = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneCharaEdit/GUI/Prefab/CharaGrow_ItemIconSet"), this.itemUseInfoRowObjList[num2].transform).transform;
					transform.name = string.Format("IconSet{0:D3}", num);
					GameObject gameObject2 = Object.Instantiate<GameObject>((GameObject)Resources.Load("CmnIconFrame/GUI/Prefab/Icon_Item"), transform.transform);
					gameObject2.name = "Icon";
					gameObject2.transform.SetSiblingIndex(1);
				}
				transform.Find("Icon_Item").gameObject.SetActive(false);
				transform.Find("Icon").GetComponent<IconItemCtrl>().Setup(DataManager.DmItem.GetItemStaticBase(itemInput.itemId), -1, new IconItemCtrl.SetupParam
				{
					useInfo = true
				});
				transform.Find("Num_Own").GetComponent<PguiTextCtrl>().text = DataManager.DmItem.GetUserItemData(itemInput.itemId).num.ToString();
				int addCharaLevelExpRatio = ItemDef.GetAddCharaLevelExpRatio(itemInput.itemId, cpd.staticData.baseData.attribute);
				transform.Find("Txt_ExpBonus").gameObject.SetActive(addCharaLevelExpRatio > 100);
				transform.Find("Count/Num_Count").gameObject.GetComponent<PguiTextCtrl>().text = itemInput.num.ToString();
				transform.Find("ColorBase").gameObject.SetActive(false);
				RectTransform component = transform.Find("Count").GetComponent<RectTransform>();
				Vector2 sizeDelta = component.sizeDelta;
				if (itemInput.num.ToString().Length > 5)
				{
					component.sizeDelta = new Vector2((float)(66 + (itemInput.num.ToString().Length - 5) * 10), sizeDelta.y);
				}
				else
				{
					component.sizeDelta = new Vector2(66f, sizeDelta.y);
				}
				transform.gameObject.SetActive(true);
				num++;
				if (num == this.useItemList.Count)
				{
					while (num % 8 != 0)
					{
						Transform transform2 = this.itemUseInfoRowObjList[num2].transform.Find(string.Format("IconSet{0:D3}", num));
						if (transform2 != null)
						{
							transform2.gameObject.SetActive(false);
						}
						num++;
					}
					for (int i = num2 + 1; i < this.itemUseInfoRowObjList.Count; i++)
					{
						this.itemUseInfoRowObjList[i].SetActive(false);
					}
				}
			}
			this.itemUseInfoLayout.preferredHeight = (float)Math.Ceiling((double)this.useItemList.Count / 8.0) * this.itemUseInfoRowOriginObj.GetComponent<RectTransform>().sizeDelta.y - this.itemUseInfoRowListObj.transform.localPosition.y;
		}

		public void SetParamInfo(CharaPackData cpd)
		{
			PrjUtil.ParamPreset paramPreset = PrjUtil.CalcParamByChara(cpd.dynamicData, cpd.dynamicData.level, cpd.dynamicData.rank);
			List<int> list = new List<int>();
			list.Add(paramPreset.totalParam);
			list.Add(paramPreset.hp);
			list.Add(paramPreset.atk);
			list.Add(paramPreset.def);
			list.Add(paramPreset.avoid);
			list.Add(paramPreset.beatDamageRatio);
			list.Add(paramPreset.actionDamageRatio);
			list.Add(paramPreset.tryDamageRatio);
			PrjUtil.ParamPreset paramPreset2 = PrjUtil.CalcParamByChara(cpd.dynamicData, this.afterLevel, this.afterRank, this.afterPromoteNum, this.afterPromoteFlag);
			List<int> list2 = new List<int> { paramPreset2.totalParam, paramPreset2.hp, paramPreset2.atk, paramPreset2.def, paramPreset2.avoid, paramPreset2.beatDamageRatio, paramPreset2.actionDamageRatio, paramPreset2.tryDamageRatio };
			SelCharaGrowWild.WindowWildResult.SetWindowParam(list, list2, this.paramAll);
		}

		public void SetNotExecMessage()
		{
			string text = "";
			foreach (KeyValuePair<SelCharaGrowMulti.GrowCategory, bool> keyValuePair in this.execMap)
			{
				if (!keyValuePair.Value && this.selCharaGrowMulti.IsSelected(keyValuePair.Key))
				{
					switch (keyValuePair.Key)
					{
					case SelCharaGrowMulti.GrowCategory.Level:
						text += "「レベル」";
						break;
					case SelCharaGrowMulti.GrowCategory.Wild:
						text += "「野生解放」";
						break;
					case SelCharaGrowMulti.GrowCategory.Rank:
						text += "「けも級」";
						break;
					case SelCharaGrowMulti.GrowCategory.Arts:
						text += "「けものミラクル」";
						break;
					case SelCharaGrowMulti.GrowCategory.Nanairo:
						text += "「なないろとくせい」";
						break;
					case SelCharaGrowMulti.GrowCategory.LevelLimit:
						text += "「レベル上限」";
						break;
					case SelCharaGrowMulti.GrowCategory.KizunaLimit:
						text += "「なかよしレベル上限」";
						break;
					}
				}
			}
			this.messageInfoObj.SetActive(text != "");
			this.notExecMsg.ReplaceTextByDefault("Param01", text);
		}

		public void Reset()
		{
			foreach (object obj in Enum.GetValues(typeof(SelCharaGrowMulti.GrowCategory)))
			{
				SelCharaGrowMulti.GrowCategory growCategory = (SelCharaGrowMulti.GrowCategory)obj;
				this.execMap[growCategory] = false;
				this.useItemMap[growCategory].Clear();
			}
			this.paramObj.SetActive(false);
			this.artsInfoObj.SetActive(false);
			this.nanairoInfoObj.SetActive(false);
			this.useItemList.Clear();
			this.promoteRequestList.Clear();
		}

		private readonly SelCharaGrowMulti selCharaGrowMulti;

		public PguiOpenWindowCtrl owCtrl;

		public ScrollRect infoScrollRect;

		public GameObject scrollContent;

		public GameObject messageInfoObj;

		public PguiTextCtrl notExecMsg;

		public Dictionary<SelCharaGrowMulti.GrowCategory, bool> execMap = new Dictionary<SelCharaGrowMulti.GrowCategory, bool>();

		public IconCharaCtrl iconChara;

		public Dictionary<SelCharaGrowMulti.GrowCategory, PguiTextCtrl> categoryTxtMap = new Dictionary<SelCharaGrowMulti.GrowCategory, PguiTextCtrl>();

		public Dictionary<SelCharaGrowMulti.GrowCategory, PguiTextCtrl> beforeTxtMap = new Dictionary<SelCharaGrowMulti.GrowCategory, PguiTextCtrl>();

		public Dictionary<SelCharaGrowMulti.GrowCategory, PguiTextCtrl> afterTxtMap = new Dictionary<SelCharaGrowMulti.GrowCategory, PguiTextCtrl>();

		public List<PguiImageCtrl> beforeStarAll = new List<PguiImageCtrl>();

		public List<PguiImageCtrl> afterStarAll = new List<PguiImageCtrl>();

		public List<PguiImageCtrl> addStarAll = new List<PguiImageCtrl>();

		public List<ItemInput> useItemList = new List<ItemInput>();

		public Dictionary<SelCharaGrowMulti.GrowCategory, List<ItemInput>> useItemMap = new Dictionary<SelCharaGrowMulti.GrowCategory, List<ItemInput>>();

		public LayoutElement itemUseInfoLayout;

		public GameObject itemUseInfoRowListObj;

		public GameObject itemUseInfoRowOriginObj;

		public List<GameObject> itemUseInfoRowObjList = new List<GameObject>();

		public const int SCROLL_ITEMS_PER_ROW = 8;

		public GameObject paramObj;

		public List<PguiTextCtrl> paramAll = new List<PguiTextCtrl>();

		public GameObject artsInfoObj;

		public PguiTextCtrl artsNameTxt;

		public CharaUtil.GUISkillInfo artsInfoBefore;

		public CharaUtil.GUISkillInfo artsInfoAfter;

		public GameObject nanairoInfoObj;

		public PguiTextCtrl nanairoNameTxt;

		public CharaUtil.GUISkillInfo nanairoInfo;

		public int afterLevel;

		public int afterPromoteNum;

		private List<bool> afterPromoteFlag;

		public int afterRank;

		public int afterArtsLv;

		public int afterLevelLimitId;

		public List<WildResult> promoteRequestList = new List<WildResult>();
	}
}
