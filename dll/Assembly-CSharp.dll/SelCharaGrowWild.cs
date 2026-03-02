using System;
using System.Collections.Generic;
using AEAuth3;
using UnityEngine;

// Token: 0x02000137 RID: 311
public class SelCharaGrowWild
{
	// Token: 0x0600107B RID: 4219 RVA: 0x000C89BC File Offset: 0x000C6BBC
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

	// Token: 0x0600107C RID: 4220 RVA: 0x000C8ABC File Offset: 0x000C6CBC
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

	// Token: 0x0600107D RID: 4221 RVA: 0x000C9154 File Offset: 0x000C7354
	public int GetPromoteNum(int charaId)
	{
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(charaId);
		if (userCharaData.dynamicData.promoteNum < userCharaData.staticData.promoteList.Count)
		{
			return userCharaData.dynamicData.promoteNum;
		}
		return userCharaData.dynamicData.promoteNum - 1;
	}

	// Token: 0x0600107E RID: 4222 RVA: 0x000C91A4 File Offset: 0x000C73A4
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

	// Token: 0x0600107F RID: 4223 RVA: 0x000C921C File Offset: 0x000C741C
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

	// Token: 0x06001080 RID: 4224 RVA: 0x000C9328 File Offset: 0x000C7528
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

	// Token: 0x06001081 RID: 4225 RVA: 0x000C9580 File Offset: 0x000C7780
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

	// Token: 0x06001082 RID: 4226 RVA: 0x000C9688 File Offset: 0x000C7888
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

	// Token: 0x06001083 RID: 4227 RVA: 0x000C9764 File Offset: 0x000C7964
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

	// Token: 0x06001084 RID: 4228 RVA: 0x000C9858 File Offset: 0x000C7A58
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

	// Token: 0x06001085 RID: 4229 RVA: 0x000C99D0 File Offset: 0x000C7BD0
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

	// Token: 0x06001086 RID: 4230 RVA: 0x000C9BA8 File Offset: 0x000C7DA8
	private string GetItemNameFix(string itemName)
	{
		string text = "】";
		if (itemName.IndexOf(text) >= 0)
		{
			return itemName.Insert(itemName.IndexOf(text) + 1, "\n");
		}
		return itemName;
	}

	// Token: 0x04000E57 RID: 3671
	public SelCharaGrowWild.CharaGrowWildGUI GrowWildGUI;

	// Token: 0x04000E58 RID: 3672
	public int currentIndexWild;

	// Token: 0x02000A26 RID: 2598
	public class PromoteItem
	{
		// Token: 0x040040F8 RID: 16632
		public int id;

		// Token: 0x040040F9 RID: 16633
		public int num;

		// Token: 0x040040FA RID: 16634
		public int cost;
	}

	// Token: 0x02000A27 RID: 2599
	public class WildItem
	{
		// Token: 0x040040FB RID: 16635
		public IconItemCtrl iconItemCtrl;

		// Token: 0x040040FC RID: 16636
		public GameObject current;

		// Token: 0x040040FD RID: 16637
		public PguiImageCtrl markPlus;

		// Token: 0x040040FE RID: 16638
		public PguiAECtrl AEImage_OpenEff;
	}

	// Token: 0x02000A28 RID: 2600
	public class WildReleaseTab
	{
		// Token: 0x06003E7D RID: 15997 RVA: 0x001EA094 File Offset: 0x001E8294
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

		// Token: 0x040040FF RID: 16639
		public GameObject baseObj;

		// Token: 0x04004100 RID: 16640
		public GameObject YaseiInfo;

		// Token: 0x04004101 RID: 16641
		public PguiButtonCtrl Btn_OpenAll;

		// Token: 0x04004102 RID: 16642
		public PguiButtonCtrl Btn_OpenComp;

		// Token: 0x04004103 RID: 16643
		public PguiAECtrl AEImage_OpenComp;

		// Token: 0x04004104 RID: 16644
		public List<SelCharaGrowWild.WildItem> iconItemList;

		// Token: 0x04004105 RID: 16645
		public List<RectTransform> iconBaseList;

		// Token: 0x04004106 RID: 16646
		public PguiTextCtrl Txt_ItemName;

		// Token: 0x04004107 RID: 16647
		public PguiTextCtrl Num_Own;

		// Token: 0x04004108 RID: 16648
		public GameObject ItemIcon;

		// Token: 0x04004109 RID: 16649
		public IconItemCtrl ItemIconCtrl;

		// Token: 0x0400410A RID: 16650
		public PguiImageCtrl Mark_YaseiUse;

		// Token: 0x0400410B RID: 16651
		public PguiTextCtrl Num_After01;

		// Token: 0x0400410C RID: 16652
		public PguiTextCtrl Num_After02;

		// Token: 0x0400410D RID: 16653
		public PguiTextCtrl Num_After03;

		// Token: 0x0400410E RID: 16654
		public AEImage AEImage_result;

		// Token: 0x0400410F RID: 16655
		public PguiImageCtrl Img_Line;

		// Token: 0x04004110 RID: 16656
		public PguiImageCtrl Contents;

		// Token: 0x04004111 RID: 16657
		public PguiTextCtrl Num_Result;

		// Token: 0x04004112 RID: 16658
		public SimpleAnimation Result_Up;
	}

	// Token: 0x02000A29 RID: 2601
	public class WindowWildGrow
	{
		// Token: 0x06003E7E RID: 15998 RVA: 0x001EA24C File Offset: 0x001E844C
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

		// Token: 0x04004113 RID: 16659
		public PguiOpenWindowCtrl owCtrl;

		// Token: 0x04004114 RID: 16660
		public PguiTextCtrl needGoldText;

		// Token: 0x04004115 RID: 16661
		public PguiTextCtrl haveGoldText;

		// Token: 0x04004116 RID: 16662
		public List<IconItemCtrl> iconItemList;

		// Token: 0x04004117 RID: 16663
		public GameObject setReleaseIconSet;

		// Token: 0x04004118 RID: 16664
		public GameObject wildReleaseIconSet;

		// Token: 0x04004119 RID: 16665
		public IconCharaCtrl iconCharaSetRelease;

		// Token: 0x0400411A RID: 16666
		public IconCharaCtrl iconCharaWildRelease_Before;

		// Token: 0x0400411B RID: 16667
		public IconCharaCtrl iconCharaWildRelease_After;

		// Token: 0x0400411C RID: 16668
		public PguiTextCtrl charaNameText;

		// Token: 0x0400411D RID: 16669
		public PguiTextCtrl setReleasePhaseNum;

		// Token: 0x0400411E RID: 16670
		public PguiTextCtrl wildReleasePhaseNumBefore;

		// Token: 0x0400411F RID: 16671
		public PguiTextCtrl wildReleasePhaseNumAfter;

		// Token: 0x04004120 RID: 16672
		public PguiTextCtrl DisableMessageText;

		// Token: 0x04004121 RID: 16673
		public PguiButtonCtrl ButtonRight;
	}

	// Token: 0x02000A2A RID: 2602
	public class WindowWildResult
	{
		// Token: 0x06003E7F RID: 15999 RVA: 0x001EA448 File Offset: 0x001E8648
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

		// Token: 0x06003E80 RID: 16000 RVA: 0x001EA55C File Offset: 0x001E875C
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

		// Token: 0x04004122 RID: 16674
		public PguiOpenWindowCtrl owCtrl;

		// Token: 0x04004123 RID: 16675
		public IconCharaCtrl iconChara;

		// Token: 0x04004124 RID: 16676
		public PguiTextCtrl Txt_CharaName;

		// Token: 0x04004125 RID: 16677
		public PguiTextCtrl Txt_Num;

		// Token: 0x04004126 RID: 16678
		public List<PguiTextCtrl> ParamAll;
	}

	// Token: 0x02000A2B RID: 2603
	public class WindowTokuseiInfo
	{
		// Token: 0x06003E81 RID: 16001 RVA: 0x001EA6C8 File Offset: 0x001E88C8
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

		// Token: 0x04004127 RID: 16679
		public PguiOpenWindowCtrl owCtrl;

		// Token: 0x04004128 RID: 16680
		public PguiTextCtrl Txt_Kind;

		// Token: 0x04004129 RID: 16681
		public PguiTextCtrl Txt_Name;

		// Token: 0x0400412A RID: 16682
		public PguiTextCtrl Txt_Info;

		// Token: 0x0400412B RID: 16683
		public PguiTextCtrl Num_Lv;

		// Token: 0x0400412C RID: 16684
		public PguiImageCtrl Icon_OrderCard;

		// Token: 0x0400412D RID: 16685
		public PguiTextCtrl Txt_KindKiseki;

		// Token: 0x0400412E RID: 16686
		public PguiTextCtrl Txt_NameKiseki;

		// Token: 0x0400412F RID: 16687
		public PguiTextCtrl Txt_InfoKiseki;

		// Token: 0x04004130 RID: 16688
		public PguiTextCtrl Num_LvKiseki;

		// Token: 0x04004131 RID: 16689
		public GameObject disableKiseki;

		// Token: 0x04004132 RID: 16690
		public GameObject skill;

		// Token: 0x04004133 RID: 16691
		public GameObject skillKiseki;

		// Token: 0x04004134 RID: 16692
		public PguiTextCtrl Num_Mp;
	}

	// Token: 0x02000A2C RID: 2604
	public class WindowIconOpen
	{
		// Token: 0x06003E82 RID: 16002 RVA: 0x001EA86D File Offset: 0x001E8A6D
		public WindowIconOpen(Transform baseTr)
		{
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.Title = baseTr.Find("Base/Window/Title/Text").GetComponent<PguiTextCtrl>();
			this.Icon_Chara = baseTr.Find("Base/Window/Icon_Chara").GetComponent<IconCharaCtrl>();
		}

		// Token: 0x04004135 RID: 16693
		public PguiOpenWindowCtrl owCtrl;

		// Token: 0x04004136 RID: 16694
		public PguiTextCtrl Title;

		// Token: 0x04004137 RID: 16695
		public IconCharaCtrl Icon_Chara;
	}

	// Token: 0x02000A2D RID: 2605
	public class CharaGrowWildGUI
	{
		// Token: 0x04004138 RID: 16696
		public SelCharaGrowWild.WindowWildGrow wildGrowWindow;

		// Token: 0x04004139 RID: 16697
		public SelCharaGrowWild.WindowWildResult wildResultWindow;

		// Token: 0x0400413A RID: 16698
		public SelCharaGrowWild.WindowTokuseiInfo tokuseiInfoWindow;

		// Token: 0x0400413B RID: 16699
		public SelCharaGrowWild.WindowIconOpen iconOpenWindow;

		// Token: 0x0400413C RID: 16700
		public SelCharaGrowWild.WildReleaseTab wildReleaseTab;
	}
}
