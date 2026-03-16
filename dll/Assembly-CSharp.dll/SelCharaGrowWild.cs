using System;
using System.Collections.Generic;
using AEAuth3;
using UnityEngine;

public class SelCharaGrowWild
{
	public SelCharaGrowWild(Transform baseTr)
	{
		this.GrowWildGUI = new SelCharaGrowWild.CharaGrowWildGUI();
		GameObject gameObject = (GameObject)Resources.Load("SceneCharaEdit/GUI/Prefab/GUI_CharaEdit_Window_Yasei");
		this.GrowWildGUI.wildGrowWindow = new SelCharaGrowWild.WindowWildGrow(Object.Instantiate<Transform>(gameObject.transform.Find("Window_YaseiAll"), baseTr).transform);
		this.GrowWildGUI.wildResultWindow = new SelCharaGrowWild.WindowWildResult(Object.Instantiate<Transform>(gameObject.transform.Find("Window_Yasei_After"), baseTr).transform);
		this.GrowWildGUI.tokuseiInfoWindow = new SelCharaGrowWild.WindowTokuseiInfo(Object.Instantiate<Transform>(gameObject.transform.Find("Window_TokuseiInfo"), baseTr).transform);
		this.GrowWildGUI.iconOpenWindow = new SelCharaGrowWild.WindowIconOpen(Object.Instantiate<Transform>(gameObject.transform.Find("Window_IconOpen"), baseTr).transform);
		this.GrowWildGUI.wildReleaseTab = new SelCharaGrowWild.WildReleaseTab(baseTr.Find("CharaGrow_Main").Find("Main/Right/Yasei"));
	}

	public void SetupItemInfoWild(int charaId)
	{
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(charaId);
		CharaPromoteOne charaPromoteOne = userCharaData.staticData.promoteList[this.GetPromoteNum(charaId)].promoteOneList[this.currentIndexWild];
		if (userCharaData.dynamicData.promoteNum >= userCharaData.staticData.maxPromoteNum)
		{
			this.GrowWildGUI.wildReleaseTab.ItemIconCtrl.gameObject.SetActive(false);
			this.GrowWildGUI.wildReleaseTab.Num_Own.gameObject.SetActive(false);
			this.GrowWildGUI.wildReleaseTab.Txt_ItemName.gameObject.SetActive(false);
			this.GrowWildGUI.wildReleaseTab.Mark_YaseiUse.gameObject.SetActive(false);
			this.GrowWildGUI.wildReleaseTab.Num_After01.gameObject.SetActive(false);
			this.GrowWildGUI.wildReleaseTab.Num_After02.gameObject.SetActive(false);
			this.GrowWildGUI.wildReleaseTab.Num_After03.gameObject.SetActive(false);
			this.GrowWildGUI.wildReleaseTab.Img_Line.gameObject.SetActive(false);
			this.GrowWildGUI.wildReleaseTab.Contents.gameObject.SetActive(false);
			return;
		}
		this.GrowWildGUI.wildReleaseTab.ItemIconCtrl.gameObject.SetActive(true);
		this.GrowWildGUI.wildReleaseTab.Num_Own.gameObject.SetActive(true);
		this.GrowWildGUI.wildReleaseTab.Txt_ItemName.gameObject.SetActive(true);
		this.GrowWildGUI.wildReleaseTab.Img_Line.gameObject.SetActive(true);
		this.GrowWildGUI.wildReleaseTab.Contents.gameObject.SetActive(true);
		this.GrowWildGUI.wildReleaseTab.ItemIconCtrl.Setup(DataManager.DmItem.GetItemStaticBase(charaPromoteOne.promoteUseItemId));
		ItemData userItemData = DataManager.DmItem.GetUserItemData(charaPromoteOne.promoteUseItemId);
		this.GrowWildGUI.wildReleaseTab.Num_Own.text = ((userItemData.num >= charaPromoteOne.promoteUseItemNum) ? string.Format("{0}/{1}", userItemData.num, charaPromoteOne.promoteUseItemNum) : string.Format("{0}{1}{2}/{3}", new object[]
		{
			PrjUtil.ColorRedStartTag,
			userItemData.num,
			PrjUtil.ColorEndTag,
			charaPromoteOne.promoteUseItemNum
		}));
		string name = userItemData.staticData.GetName();
		this.GrowWildGUI.wildReleaseTab.Txt_ItemName.text = this.GetItemNameFix(name);
		CharaDynamicData dynamicData = userCharaData.dynamicData;
		List<bool> list = new List<bool>(dynamicData.promoteFlag);
		list[this.currentIndexWild] = false;
		PrjUtil.ParamPreset paramPreset = PrjUtil.CalcParamByChara(dynamicData, dynamicData.level, dynamicData.rank, dynamicData.promoteNum, list);
		List<bool> list2 = new List<bool>(dynamicData.promoteFlag);
		list2[this.currentIndexWild] = true;
		PrjUtil.ParamPreset paramPreset2 = PrjUtil.CalcParamByChara(dynamicData, dynamicData.level, dynamicData.rank, dynamicData.promoteNum, list2);
		PrjUtil.ParamPreset paramPreset3 = new PrjUtil.ParamPreset
		{
			hp = paramPreset2.hp - paramPreset.hp,
			atk = paramPreset2.atk - paramPreset.atk,
			def = paramPreset2.def - paramPreset.def,
			avoid = paramPreset2.avoid - paramPreset.avoid,
			beatDamageRatio = paramPreset2.beatDamageRatio - paramPreset.beatDamageRatio,
			actionDamageRatio = paramPreset2.actionDamageRatio - paramPreset.actionDamageRatio,
			tryDamageRatio = paramPreset2.tryDamageRatio - paramPreset.tryDamageRatio
		};
		List<string> list3 = new List<string>();
		if (paramPreset3.hp > 0)
		{
			list3.Add("たいりょく  +" + paramPreset3.hp.ToString());
		}
		if (paramPreset3.atk > 0)
		{
			list3.Add("こうげき  +" + paramPreset3.atk.ToString());
		}
		if (paramPreset3.def > 0)
		{
			list3.Add("まもり  +" + paramPreset3.def.ToString());
		}
		if (paramPreset3.avoid > 0)
		{
			list3.Add("かいひ  +" + ((float)paramPreset3.avoid / 10f).ToString("F1") + "%");
		}
		if (paramPreset3.beatDamageRatio > 0)
		{
			list3.Add("Beat!!!ダメージアップ\u3000+" + ((float)paramPreset3.beatDamageRatio / 10f).ToString("F1") + "%");
		}
		if (paramPreset3.actionDamageRatio > 0)
		{
			list3.Add("Action!ダメージアップ\u3000+" + ((float)paramPreset3.actionDamageRatio / 10f).ToString("F1") + "%");
		}
		if (paramPreset3.tryDamageRatio > 0)
		{
			list3.Add("Try!!ダメージアップ\u3000+" + ((float)paramPreset3.tryDamageRatio / 10f).ToString("F1") + "%");
		}
		this.GrowWildGUI.wildReleaseTab.Mark_YaseiUse.gameObject.SetActive(userCharaData.dynamicData.promoteFlag[this.currentIndexWild]);
		if (list3.Count > 0)
		{
			this.GrowWildGUI.wildReleaseTab.Num_After01.gameObject.SetActive(true);
			this.GrowWildGUI.wildReleaseTab.Num_After01.text = PrjUtil.MakeMessage(list3[0]);
		}
		else
		{
			this.GrowWildGUI.wildReleaseTab.Num_After01.gameObject.SetActive(false);
		}
		if (list3.Count > 1)
		{
			this.GrowWildGUI.wildReleaseTab.Num_After02.gameObject.SetActive(true);
			this.GrowWildGUI.wildReleaseTab.Num_After02.text = PrjUtil.MakeMessage(list3[1]);
		}
		else
		{
			this.GrowWildGUI.wildReleaseTab.Num_After02.gameObject.SetActive(false);
		}
		if (list3.Count > 2)
		{
			this.GrowWildGUI.wildReleaseTab.Num_After03.gameObject.SetActive(true);
			this.GrowWildGUI.wildReleaseTab.Num_After03.text = PrjUtil.MakeMessage(list3[2]);
			return;
		}
		this.GrowWildGUI.wildReleaseTab.Num_After03.gameObject.SetActive(false);
	}

	public int GetPromoteNum(int charaId)
	{
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(charaId);
		if (userCharaData.dynamicData.promoteNum < userCharaData.staticData.promoteList.Count)
		{
			return userCharaData.dynamicData.promoteNum;
		}
		return userCharaData.dynamicData.promoteNum - 1;
	}

	public int GetCostGoldWildRelease(int charaId)
	{
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(charaId);
		CharaPromotePreset charaPromotePreset = userCharaData.staticData.promoteList[this.GetPromoteNum(charaId)];
		int num = 0;
		for (int i = 0; i < charaPromotePreset.promoteOneList.Count; i++)
		{
			CharaPromoteOne charaPromoteOne = charaPromotePreset.promoteOneList[i];
			if (!userCharaData.dynamicData.promoteFlag[i])
			{
				num += charaPromoteOne.costGoldNum;
			}
		}
		return num;
	}

	public void UpdateItemWild(int charaId)
	{
		SelCharaGrowWild.WildReleaseTab wildReleaseTab = this.GrowWildGUI.wildReleaseTab;
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(charaId);
		CharaPromotePreset charaPromotePreset = userCharaData.staticData.promoteList[this.GetPromoteNum(charaId)];
		for (int i = 0; i < wildReleaseTab.iconItemList.Count; i++)
		{
			SelCharaGrowWild.WildItem wildItem = wildReleaseTab.iconItemList[i];
			CharaPromoteOne charaPromoteOne = charaPromotePreset.promoteOneList[i];
			ItemData userItemData = DataManager.DmItem.GetUserItemData(charaPromoteOne.promoteUseItemId);
			wildItem.iconItemCtrl.Setup(userItemData.staticData, -1);
			wildItem.iconItemCtrl.SetActEnable(userCharaData.dynamicData.promoteFlag[i]);
			bool flag = userCharaData.dynamicData.promoteNum >= userCharaData.staticData.maxPromoteNum;
			wildItem.markPlus.gameObject.SetActive(charaPromoteOne.promoteUseItemNum <= userItemData.num && !userCharaData.dynamicData.promoteFlag[i] && !flag);
		}
	}

	public List<SelCharaGrowWild.PromoteItem> WildReleaseDispItemList(int charaId)
	{
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(charaId);
		CharaPromotePreset charaPromotePreset = userCharaData.staticData.promoteList[this.GetPromoteNum(charaId)];
		foreach (IconItemCtrl iconItemCtrl in this.GrowWildGUI.wildGrowWindow.iconItemList)
		{
			iconItemCtrl.gameObject.SetActive(false);
		}
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		for (int i = 0; i < charaPromotePreset.promoteOneList.Count; i++)
		{
			if (!userCharaData.dynamicData.promoteFlag[i])
			{
				if (dictionary.ContainsKey(charaPromotePreset.promoteOneList[i].promoteUseItemId))
				{
					Dictionary<int, int> dictionary2 = dictionary;
					int promoteUseItemId = charaPromotePreset.promoteOneList[i].promoteUseItemId;
					dictionary2[promoteUseItemId] += charaPromotePreset.promoteOneList[i].promoteUseItemNum;
				}
				else
				{
					dictionary.Add(charaPromotePreset.promoteOneList[i].promoteUseItemId, charaPromotePreset.promoteOneList[i].promoteUseItemNum);
				}
			}
		}
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, int> keyValuePair in dictionary)
		{
			if (keyValuePair.Value <= DataManager.DmItem.GetUserItemData(keyValuePair.Key).num)
			{
				list.Add(keyValuePair.Key);
			}
		}
		List<SelCharaGrowWild.PromoteItem> list2 = new List<SelCharaGrowWild.PromoteItem>();
		int num = 0;
		foreach (CharaPromoteOne charaPromoteOne in charaPromotePreset.promoteOneList)
		{
			if (!userCharaData.dynamicData.promoteFlag[num] && list.Contains(charaPromoteOne.promoteUseItemId))
			{
				SelCharaGrowWild.PromoteItem promoteItem = new SelCharaGrowWild.PromoteItem
				{
					id = charaPromoteOne.promoteUseItemId,
					num = charaPromoteOne.promoteUseItemNum,
					cost = charaPromoteOne.costGoldNum
				};
				list2.Add(promoteItem);
			}
			num++;
		}
		return list2;
	}

	public List<SelCharaGrowWild.PromoteItem> CreatePromoteItemList(int charaID)
	{
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(charaID);
		CharaPromotePreset charaPromotePreset = userCharaData.staticData.promoteList[this.GetPromoteNum(charaID)];
		List<SelCharaGrowWild.PromoteItem> list = new List<SelCharaGrowWild.PromoteItem>();
		for (int i = 0; i < charaPromotePreset.promoteOneList.Count; i++)
		{
			CharaPromoteOne wildPresetOne = charaPromotePreset.promoteOneList[i];
			SelCharaGrowWild.PromoteItem promoteItem = new SelCharaGrowWild.PromoteItem();
			int num = list.FindIndex((SelCharaGrowWild.PromoteItem item) => item.id == wildPresetOne.promoteUseItemId);
			if (num >= 0)
			{
				if (!userCharaData.dynamicData.promoteFlag[i])
				{
					list[num].num += wildPresetOne.promoteUseItemNum;
				}
			}
			else
			{
				promoteItem.id = wildPresetOne.promoteUseItemId;
				promoteItem.num = wildPresetOne.promoteUseItemNum;
				promoteItem.cost = wildPresetOne.costGoldNum;
				list.Add(promoteItem);
			}
		}
		return list;
	}

	public int GetCostGoldTogetherEquip(int charaId)
	{
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(charaId);
		CharaPromotePreset charaPromotePreset = userCharaData.staticData.promoteList[this.GetPromoteNum(charaId)];
		List<SelCharaGrowWild.PromoteItem> list = this.CreatePromoteItemList(charaId);
		int num = 0;
		for (int i = 0; i < charaPromotePreset.promoteOneList.Count; i++)
		{
			CharaPromoteOne wildPresetOne = charaPromotePreset.promoteOneList[i];
			ItemData userItemData = DataManager.DmItem.GetUserItemData(wildPresetOne.promoteUseItemId);
			SelCharaGrowWild.PromoteItem promoteItem = list.Find((SelCharaGrowWild.PromoteItem item) => item.id == wildPresetOne.promoteUseItemId);
			if (promoteItem != null && promoteItem.num <= userItemData.num && !userCharaData.dynamicData.promoteFlag[i])
			{
				num += wildPresetOne.costGoldNum;
			}
		}
		return num;
	}

	public void SetWindowWildItem(List<SelCharaGrowWild.PromoteItem> dispItemList)
	{
		foreach (IconItemCtrl iconItemCtrl in this.GrowWildGUI.wildGrowWindow.iconItemList)
		{
			iconItemCtrl.gameObject.SetActive(false);
		}
		if (0 < dispItemList.Count)
		{
			int num = 0;
			foreach (SelCharaGrowWild.PromoteItem promoteItem in dispItemList)
			{
				this.GrowWildGUI.wildGrowWindow.iconItemList[num].gameObject.SetActive(true);
				this.GrowWildGUI.wildGrowWindow.iconItemList[num].Setup(DataManager.DmItem.GetItemStaticBase(promoteItem.id));
				num++;
			}
		}
	}

	public void SetWindowParam(List<int> befParamList, List<int> aftParamList, List<PguiTextCtrl> ParamAll)
	{
		foreach (PguiTextCtrl pguiTextCtrl in ParamAll)
		{
			pguiTextCtrl.gameObject.SetActive(false);
		}
		for (int i = 0; i < befParamList.Count; i++)
		{
			if (i < this.GrowWildGUI.wildResultWindow.ParamAll.Count)
			{
				ParamAll[i].gameObject.SetActive(true);
				int num = aftParamList[i] - befParamList[i];
				if (i < 4)
				{
					ParamAll[i].text = string.Concat(new string[]
					{
						aftParamList[i].ToString(),
						"(",
						(0 <= num) ? "+" : string.Empty,
						num.ToString(),
						")"
					});
				}
				else
				{
					float num2 = (float)aftParamList[i] / 10f;
					float num3 = (float)num / 10f;
					ParamAll[i].text = string.Concat(new string[]
					{
						num2.ToString("F1"),
						"%(",
						(0f <= num3) ? "+" : string.Empty,
						num3.ToString("F1"),
						"%)"
					});
				}
			}
		}
	}

	public void SetActWildButton(int charaId)
	{
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(charaId);
		CharaPromotePreset charaPromotePreset = userCharaData.staticData.promoteList[this.GetPromoteNum(charaId)];
		int num = 0;
		using (List<bool>.Enumerator enumerator = userCharaData.dynamicData.promoteFlag.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current)
				{
					num++;
				}
			}
		}
		List<SelCharaGrowWild.PromoteItem> list = this.WildReleaseDispItemList(charaId);
		int num2 = 0;
		foreach (SelCharaGrowWild.PromoteItem promoteItem in list)
		{
			num2 += promoteItem.cost;
		}
		bool flag = num2 <= DataManager.DmItem.GetUserItemData(30101).num;
		flag = true;
		if (num == list.Count)
		{
			this.GrowWildGUI.wildReleaseTab.Btn_OpenComp.gameObject.SetActive(true);
			this.GrowWildGUI.wildReleaseTab.Btn_OpenComp.SetActEnable(flag, false, false);
			if (flag)
			{
				this.GrowWildGUI.wildReleaseTab.AEImage_OpenComp.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
			}
			else
			{
				this.GrowWildGUI.wildReleaseTab.AEImage_OpenComp.PauseAnimationLastFrame(PguiAECtrl.AmimeType.LOOP);
			}
			this.GrowWildGUI.wildReleaseTab.Btn_OpenAll.gameObject.SetActive(false);
		}
		else
		{
			this.GrowWildGUI.wildReleaseTab.Btn_OpenComp.gameObject.SetActive(false);
			this.GrowWildGUI.wildReleaseTab.Btn_OpenAll.gameObject.SetActive(true);
			this.GrowWildGUI.wildReleaseTab.Btn_OpenAll.SetActEnable(0 < num && 0 < list.Count && flag, false, false);
		}
		this.SetWindowWildItem(list);
	}

	private string GetItemNameFix(string itemName)
	{
		string text = "】";
		if (itemName.IndexOf(text) >= 0)
		{
			return itemName.Insert(itemName.IndexOf(text) + 1, "\n");
		}
		return itemName;
	}

	public SelCharaGrowWild.CharaGrowWildGUI GrowWildGUI;

	public int currentIndexWild;

	public class PromoteItem
	{
		public int id;

		public int num;

		public int cost;
	}

	public class WildItem
	{
		public IconItemCtrl iconItemCtrl;

		public GameObject current;

		public PguiImageCtrl markPlus;

		public PguiAECtrl AEImage_OpenEff;
	}

	public class WildReleaseTab
	{
		public WildReleaseTab(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.YaseiInfo = baseTr.Find("YaseiInfo").gameObject;
			this.Btn_OpenAll = baseTr.Find("YaseiInfo/Btn_OpenAll").GetComponent<PguiButtonCtrl>();
			this.Btn_OpenComp = baseTr.Find("YaseiInfo/Btn_OpenComp").GetComponent<PguiButtonCtrl>();
			this.AEImage_OpenComp = baseTr.Find("YaseiInfo/Btn_OpenComp/BaseImage/AEImage_OpenComp").GetComponent<PguiAECtrl>();
			this.iconItemList = new List<SelCharaGrowWild.WildItem>();
			this.iconBaseList = new List<RectTransform>();
			this.Txt_ItemName = baseTr.Find("Txt_ItemName").GetComponent<PguiTextCtrl>();
			this.Num_Own = baseTr.Find("Num_Own").GetComponent<PguiTextCtrl>();
			this.ItemIcon = baseTr.Find("ItemIcon").gameObject;
			this.Mark_YaseiUse = baseTr.Find("Mark_YaseiUse").GetComponent<PguiImageCtrl>();
			this.Num_After01 = baseTr.Find("Num_After01").GetComponent<PguiTextCtrl>();
			this.Num_After02 = baseTr.Find("Num_After02").GetComponent<PguiTextCtrl>();
			this.Num_After03 = baseTr.Find("Num_After03").GetComponent<PguiTextCtrl>();
			this.AEImage_result = baseTr.Find("AEImage_result").GetComponent<AEImage>();
			this.AEImage_result.gameObject.SetActive(false);
			this.Img_Line = baseTr.Find("Img_Line").GetComponent<PguiImageCtrl>();
			this.Contents = baseTr.Find("Contents").GetComponent<PguiImageCtrl>();
			this.Num_Result = baseTr.Find("Result_Up/Num_Result").GetComponent<PguiTextCtrl>();
			this.Result_Up = baseTr.Find("Result_Up").GetComponent<SimpleAnimation>();
			this.Result_Up.gameObject.SetActive(false);
		}

		public GameObject baseObj;

		public GameObject YaseiInfo;

		public PguiButtonCtrl Btn_OpenAll;

		public PguiButtonCtrl Btn_OpenComp;

		public PguiAECtrl AEImage_OpenComp;

		public List<SelCharaGrowWild.WildItem> iconItemList;

		public List<RectTransform> iconBaseList;

		public PguiTextCtrl Txt_ItemName;

		public PguiTextCtrl Num_Own;

		public GameObject ItemIcon;

		public IconItemCtrl ItemIconCtrl;

		public PguiImageCtrl Mark_YaseiUse;

		public PguiTextCtrl Num_After01;

		public PguiTextCtrl Num_After02;

		public PguiTextCtrl Num_After03;

		public AEImage AEImage_result;

		public PguiImageCtrl Img_Line;

		public PguiImageCtrl Contents;

		public PguiTextCtrl Num_Result;

		public SimpleAnimation Result_Up;
	}

	public class WindowWildGrow
	{
		public WindowWildGrow(Transform baseTr)
		{
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.iconItemList = new List<IconItemCtrl>();
			for (int i = 0; i < 6; i++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("CmnIconFrame/GUI/Prefab/Icon_Item"), baseTr.Find("Base/Window/ItemUseInfo/ItemIconAll"));
				this.iconItemList.Add(gameObject.GetComponent<IconItemCtrl>());
			}
			this.needGoldText = baseTr.Find("Base/Window/ItemUse/Num").GetComponent<PguiTextCtrl>();
			this.haveGoldText = baseTr.Find("Base/Window/ItemOwn/Num").GetComponent<PguiTextCtrl>();
			this.setReleaseIconSet = baseTr.Find("Base/Window/CharaInfo/Icon_Chara01").gameObject;
			GameObject gameObject2 = Object.Instantiate<GameObject>((GameObject)Resources.Load("CmnIconFrame/GUI/Prefab/Icon_Chara"), this.setReleaseIconSet.transform);
			this.iconCharaSetRelease = gameObject2.GetComponent<IconCharaCtrl>();
			this.setReleasePhaseNum = this.setReleaseIconSet.transform.Find("Num_Before").GetComponent<PguiTextCtrl>();
			this.ButtonRight = baseTr.Find("Base/Window/ButtonR").GetComponent<PguiButtonCtrl>();
			this.DisableMessageText = baseTr.Find("Base/Window/ButtonR/DisableText").GetComponent<PguiTextCtrl>();
			this.wildReleaseIconSet = baseTr.Find("Base/Window/CharaInfo/Icon_Chara02/").gameObject;
			GameObject gameObject3 = Object.Instantiate<GameObject>((GameObject)Resources.Load("CmnIconFrame/GUI/Prefab/Icon_Chara"), this.wildReleaseIconSet.transform.Find("Icon_Chara_Before"));
			this.iconCharaWildRelease_Before = gameObject3.GetComponent<IconCharaCtrl>();
			this.wildReleasePhaseNumBefore = this.wildReleaseIconSet.transform.Find("Icon_Chara_Before/Num_Before").GetComponent<PguiTextCtrl>();
			GameObject gameObject4 = Object.Instantiate<GameObject>((GameObject)Resources.Load("CmnIconFrame/GUI/Prefab/Icon_Chara"), this.wildReleaseIconSet.transform.Find("Icon_Chara_After"));
			this.iconCharaWildRelease_After = gameObject4.GetComponent<IconCharaCtrl>();
			this.wildReleasePhaseNumAfter = this.wildReleaseIconSet.transform.Find("Icon_Chara_After/Num_Before").GetComponent<PguiTextCtrl>();
			this.charaNameText = baseTr.Find("Base/Window/CharaInfo/Txt_CharaName").GetComponent<PguiTextCtrl>();
		}

		public PguiOpenWindowCtrl owCtrl;

		public PguiTextCtrl needGoldText;

		public PguiTextCtrl haveGoldText;

		public List<IconItemCtrl> iconItemList;

		public GameObject setReleaseIconSet;

		public GameObject wildReleaseIconSet;

		public IconCharaCtrl iconCharaSetRelease;

		public IconCharaCtrl iconCharaWildRelease_Before;

		public IconCharaCtrl iconCharaWildRelease_After;

		public PguiTextCtrl charaNameText;

		public PguiTextCtrl setReleasePhaseNum;

		public PguiTextCtrl wildReleasePhaseNumBefore;

		public PguiTextCtrl wildReleasePhaseNumAfter;

		public PguiTextCtrl DisableMessageText;

		public PguiButtonCtrl ButtonRight;
	}

	public class WindowWildResult
	{
		public WindowWildResult(Transform baseTr)
		{
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("CmnIconFrame/GUI/Prefab/Icon_Chara"), baseTr.Find("Base/Window/Icon_Chara"));
			this.iconChara = gameObject.GetComponent<IconCharaCtrl>();
			this.Txt_CharaName = baseTr.Find("Base/Window/Icon_Chara/Txt_CharaName").GetComponent<PguiTextCtrl>();
			this.Txt_Num = baseTr.Find("Base/Window/Icon_Chara/Num").GetComponent<PguiTextCtrl>();
			this.ParamAll = new List<PguiTextCtrl>();
			for (int i = 0; i < 5; i++)
			{
				this.ParamAll.Add(baseTr.Find("Base/Window/ParamAll/Info" + (i + 1).ToString("D2") + "/Num_After").GetComponent<PguiTextCtrl>());
			}
			this.ParamAll.Add(baseTr.Find("Base/Window/ParamAll/Info_Flag_Beat/Num_After").GetComponent<PguiTextCtrl>());
			this.ParamAll.Add(baseTr.Find("Base/Window/ParamAll/Info_Flag_Action/Num_After").GetComponent<PguiTextCtrl>());
			this.ParamAll.Add(baseTr.Find("Base/Window/ParamAll/Info_Flag_Try/Num_After").GetComponent<PguiTextCtrl>());
		}

		public static void SetWindowParam(List<int> befParamList, List<int> aftParamList, List<PguiTextCtrl> ParamAll)
		{
			foreach (PguiTextCtrl pguiTextCtrl in ParamAll)
			{
				pguiTextCtrl.gameObject.SetActive(false);
			}
			for (int i = 0; i < befParamList.Count; i++)
			{
				if (i < ParamAll.Count)
				{
					ParamAll[i].gameObject.SetActive(true);
					int num = aftParamList[i] - befParamList[i];
					if (i < 4)
					{
						ParamAll[i].text = string.Concat(new string[]
						{
							aftParamList[i].ToString(),
							"(",
							(0 <= num) ? "+" : string.Empty,
							num.ToString(),
							")"
						});
					}
					else
					{
						float num2 = (float)aftParamList[i] / 10f;
						float num3 = (float)num / 10f;
						ParamAll[i].text = string.Concat(new string[]
						{
							num2.ToString("F1"),
							"%(",
							(0f <= num3) ? "+" : string.Empty,
							num3.ToString("F1"),
							"%)"
						});
					}
				}
			}
		}

		public PguiOpenWindowCtrl owCtrl;

		public IconCharaCtrl iconChara;

		public PguiTextCtrl Txt_CharaName;

		public PguiTextCtrl Txt_Num;

		public List<PguiTextCtrl> ParamAll;
	}

	public class WindowTokuseiInfo
	{
		public WindowTokuseiInfo(Transform baseTr)
		{
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.Txt_Kind = baseTr.Find("Base/Window/CharaInfo_List_Skill/Txt_Kind").GetComponent<PguiTextCtrl>();
			this.Txt_Name = baseTr.Find("Base/Window/CharaInfo_List_Skill/Txt_Name").GetComponent<PguiTextCtrl>();
			this.Txt_Info = baseTr.Find("Base/Window/CharaInfo_List_Skill/Txt_Info").GetComponent<PguiTextCtrl>();
			this.Num_Lv = baseTr.Find("Base/Window/CharaInfo_List_Skill/Num_Lv").GetComponent<PguiTextCtrl>();
			this.Num_Lv.gameObject.SetActive(false);
			this.Icon_OrderCard = baseTr.Find("Base/Window/CharaInfo_List_Skill/Icon_OrderCard").GetComponent<PguiImageCtrl>();
			this.Icon_OrderCard.gameObject.SetActive(false);
			this.Txt_KindKiseki = baseTr.Find("Base/Window/CharaInfo_List_Skill_05_kiseki/Txt_Kind").GetComponent<PguiTextCtrl>();
			this.Txt_NameKiseki = baseTr.Find("Base/Window/CharaInfo_List_Skill_05_kiseki/Txt_Name").GetComponent<PguiTextCtrl>();
			this.Txt_InfoKiseki = baseTr.Find("Base/Window/CharaInfo_List_Skill_05_kiseki/Txt_Info").GetComponent<PguiTextCtrl>();
			this.Num_LvKiseki = baseTr.Find("Base/Window/CharaInfo_List_Skill_05_kiseki/Num_Lv").GetComponent<PguiTextCtrl>();
			this.Num_LvKiseki.gameObject.SetActive(false);
			this.disableKiseki = baseTr.Find("Base/Window/CharaInfo_List_Skill_05_kiseki/Disable").gameObject;
			this.disableKiseki.SetActive(false);
			this.skill = baseTr.Find("Base/Window/CharaInfo_List_Skill").gameObject;
			this.skill.SetActive(false);
			this.skillKiseki = baseTr.Find("Base/Window/CharaInfo_List_Skill_05_kiseki").gameObject;
			this.skillKiseki.SetActive(false);
			this.Num_Mp = baseTr.Find("Base/Window/CharaInfo_List_Skill/Num_Mp").GetComponent<PguiTextCtrl>();
			this.Num_Mp.gameObject.SetActive(false);
		}

		public PguiOpenWindowCtrl owCtrl;

		public PguiTextCtrl Txt_Kind;

		public PguiTextCtrl Txt_Name;

		public PguiTextCtrl Txt_Info;

		public PguiTextCtrl Num_Lv;

		public PguiImageCtrl Icon_OrderCard;

		public PguiTextCtrl Txt_KindKiseki;

		public PguiTextCtrl Txt_NameKiseki;

		public PguiTextCtrl Txt_InfoKiseki;

		public PguiTextCtrl Num_LvKiseki;

		public GameObject disableKiseki;

		public GameObject skill;

		public GameObject skillKiseki;

		public PguiTextCtrl Num_Mp;
	}

	public class WindowIconOpen
	{
		public WindowIconOpen(Transform baseTr)
		{
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.Title = baseTr.Find("Base/Window/Title/Text").GetComponent<PguiTextCtrl>();
			this.Icon_Chara = baseTr.Find("Base/Window/Icon_Chara").GetComponent<IconCharaCtrl>();
		}

		public PguiOpenWindowCtrl owCtrl;

		public PguiTextCtrl Title;

		public IconCharaCtrl Icon_Chara;
	}

	public class CharaGrowWildGUI
	{
		public SelCharaGrowWild.WindowWildGrow wildGrowWindow;

		public SelCharaGrowWild.WindowWildResult wildResultWindow;

		public SelCharaGrowWild.WindowTokuseiInfo tokuseiInfoWindow;

		public SelCharaGrowWild.WindowIconOpen iconOpenWindow;

		public SelCharaGrowWild.WildReleaseTab wildReleaseTab;
	}
}
