using System;
using System.Collections.Generic;
using AEAuth3;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000131 RID: 305
public class SelCharaGrowLevel
{
	// Token: 0x06001064 RID: 4196 RVA: 0x000C7100 File Offset: 0x000C5300
	public SelCharaGrowLevel(Transform baseTr)
	{
		this.LevelUpGUI = new SelCharaGrowLevel.CharaLevelUpGUI();
		GameObject gameObject = (GameObject)Resources.Load("SceneCharaEdit/GUI/Prefab/GUI_CharaEdit_Window_Lv");
		this.LevelUpGUI.lvupAuth = new SelCharaGrowLevel.LvupAuth(Object.Instantiate<Transform>(gameObject.transform.Find("Auth_JapamanFeed"), baseTr).transform);
		this.LevelUpGUI.lvupAuth.baseObj.SetActive(false);
		this.LevelUpGUI.lvUpWindow = new SelCharaGrowLevel.WindowLvUp(Object.Instantiate<Transform>(gameObject.transform.Find("Window_LvUp"), baseTr).transform);
		this.LevelUpGUI.lvUpTab = new SelCharaGrowLevel.LvUpTab(baseTr.Find("CharaGrow_Main").Find("Main/Right/LevelUp"));
		this.LevelUpGUI.itemUseWindow = new SelCharaGrowLevel.WindowItemUse(Object.Instantiate<Transform>(gameObject.transform.Find("Window_ItemUse"), baseTr).transform);
		this.LevelUpGUI.levelLimitOverWindow = new SelCharaGrowLevel.WindowLevelLimitOver(Object.Instantiate<Transform>(gameObject.transform.Find("Window_LvLimitOpen"), baseTr).transform);
	}

	// Token: 0x06001065 RID: 4197 RVA: 0x000C7218 File Offset: 0x000C5418
	public void CreateLvUpItem(GameObject go, int i, int itemId, int attr)
	{
		SelCharaGrowLevel.LvUpItem lvUpItem = new SelCharaGrowLevel.LvUpItem();
		GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneCharaEdit/GUI/Prefab/CharaGrow_ItemIconSet"), go.transform);
		gameObject.name = i.ToString();
		GameObject gameObject2 = Object.Instantiate<GameObject>((GameObject)Resources.Load("CmnIconFrame/GUI/Prefab/Icon_Item"), gameObject.transform.Find("Icon_Item"));
		lvUpItem.iconItemCtrl = gameObject2.GetComponent<IconItemCtrl>();
		lvUpItem.expBonus = gameObject.transform.Find("Txt_ExpBonus").gameObject.GetComponent<PguiTextCtrl>();
		lvUpItem.itemNum = gameObject.transform.Find("Num_Own").gameObject.GetComponent<PguiTextCtrl>();
		lvUpItem.ColorBase = gameObject.transform.Find("ColorBase").GetComponent<PguiImageCtrl>();
		lvUpItem.imgCount = gameObject.transform.Find("Count").gameObject.GetComponent<PguiImageCtrl>();
		lvUpItem.itemCount = gameObject.transform.Find("Count/Num_Count").gameObject.GetComponent<PguiTextCtrl>();
		lvUpItem.iconItemCtrl.Clear();
		lvUpItem.expBonus.gameObject.SetActive(false);
		lvUpItem.itemNum.gameObject.SetActive(false);
		lvUpItem.ColorBase.gameObject.SetActive(false);
		lvUpItem.imgCount.gameObject.SetActive(false);
		lvUpItem.itemCount.gameObject.SetActive(false);
		lvUpItem.itemId = itemId;
		this.LevelUpGUI.lvUpTab.iconItemList.Add(lvUpItem);
		this.LevelUpGUI.lvUpTab.itemListBar[attr].IconItemList.Add(lvUpItem);
	}

	// Token: 0x06001066 RID: 4198 RVA: 0x000C73C0 File Offset: 0x000C55C0
	public string TabInfoText(int charaId)
	{
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(charaId);
		DataManagerChara.LevelLimitData levelLimitData = DataManager.DmChara.GetLevelLimitData(userCharaData.dynamicData.levelLimitId + 1);
		string text = ((levelLimitData != null) ? levelLimitData.NeedItemName01 : string.Empty);
		string text2 = string.Empty;
		if (levelLimitData != null && levelLimitData.needItemId02 != 0)
		{
			text2 = "と" + levelLimitData.NeedItemName02;
		}
		if (!this.CanLevelLimitOver(charaId))
		{
			return "ジャパまんを使ってフレンズを強化します<size=16>\u3000※アイコン長押しで一括選択ができます</size>";
		}
		return text + text2 + "を使ってレベル上限を解放します";
	}

	// Token: 0x06001067 RID: 4199 RVA: 0x000C7440 File Offset: 0x000C5640
	public bool CanLevelLimitOver(int charaId)
	{
		CharaDynamicData dynamicData = DataManager.DmChara.GetUserCharaData(charaId).dynamicData;
		int num = CharaPackData.CalcLimitLevel(dynamicData.id, dynamicData.rank, dynamicData.levelLimitId);
		int levelLimitDataListCount = DataManager.DmChara.GetLevelLimitDataListCount();
		bool flag = num <= dynamicData.level;
		bool flag2 = dynamicData.rank == 6;
		bool flag3 = dynamicData.levelLimitId < levelLimitDataListCount;
		int num2 = (flag3 ? (dynamicData.levelLimitId + 1) : levelLimitDataListCount);
		bool flag4 = DataManager.DmChara.GetLevelLimitData(num2) != null;
		return flag && flag2 && flag3 && flag4;
	}

	// Token: 0x06001068 RID: 4200 RVA: 0x000C74C8 File Offset: 0x000C56C8
	public List<ItemStaticBase> GetExpAddItemList(int attr)
	{
		List<ItemStaticBase> list = new List<ItemStaticBase>();
		foreach (DataManagerServerMst.CharaLevelItem charaLevelItem in DataManager.DmServerMst.charaLevelItemDataList)
		{
			if (charaLevelItem.attribute == (CharaDef.AttributeType)attr && charaLevelItem.isKizuna == 0)
			{
				list.Add(DataManager.DmItem.GetItemStaticMap()[charaLevelItem.itemId]);
			}
		}
		list.Sort((ItemStaticBase x, ItemStaticBase y) => x.GetRarity() - y.GetRarity());
		return list;
	}

	// Token: 0x06001069 RID: 4201 RVA: 0x000C7570 File Offset: 0x000C5770
	public void UpdateItemLvUp()
	{
		foreach (SelCharaGrowLevel.LvUpItem lvUpItem in this.LevelUpGUI.lvUpTab.iconItemList)
		{
			Dictionary<int, ItemData> userItemMap = DataManager.DmItem.GetUserItemMap();
			ItemData itemData = (userItemMap.ContainsKey(lvUpItem.itemId) ? userItemMap[lvUpItem.itemId] : null);
			if (itemData != null)
			{
				lvUpItem.itemNum.text = itemData.num.ToString();
				lvUpItem.expBonus.gameObject.SetActive(false);
			}
			else
			{
				lvUpItem.itemNum.text = 0.ToString();
				lvUpItem.expBonus.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x0600106A RID: 4202 RVA: 0x000C764C File Offset: 0x000C584C
	public string GetItemId2AttributeId(int itemId)
	{
		string text = "BROWN";
		switch (DataManager.DmServerMst.charaLevelItemDataList.Find((DataManagerServerMst.CharaLevelItem x) => x.itemId == itemId).attribute)
		{
		case CharaDef.AttributeType.ALL:
			switch (DataManager.DmItem.GetUserItemData(itemId).staticData.GetRarity())
			{
			case ItemDef.Rarity.STAR1:
				text = "BROWN";
				break;
			case ItemDef.Rarity.STAR2:
				text = "YELLOW";
				break;
			case ItemDef.Rarity.STAR3:
				text = "RAINBOW";
				break;
			}
			break;
		case CharaDef.AttributeType.RED:
			text = "FUNNY";
			break;
		case CharaDef.AttributeType.GREEN:
			text = "FRIENDLY";
			break;
		case CharaDef.AttributeType.BLUE:
			text = "RELAX";
			break;
		case CharaDef.AttributeType.PINK:
			text = "LOVELY";
			break;
		case CharaDef.AttributeType.LIME:
			text = "ACTOVE";
			break;
		case CharaDef.AttributeType.AQUA:
			text = "MYPACE";
			break;
		}
		return text;
	}

	// Token: 0x04000E4D RID: 3661
	public SelCharaGrowLevel.CharaLevelUpGUI LevelUpGUI;

	// Token: 0x020009FF RID: 2559
	public class LvUpItem
	{
		// Token: 0x06003E1E RID: 15902 RVA: 0x001E498C File Offset: 0x001E2B8C
		public void SetUp(bool isMax, int itemDataNum, int count)
		{
			this.iconItemCtrl.SetActEnable(!isMax);
			this.itemNum.gameObject.SetActive(true);
			this.itemNum.text = itemDataNum.ToString();
			this.imgCount.gameObject.SetActive(0 < count);
			this.itemCount.gameObject.SetActive(0 < count);
			this.itemCount.text = count.ToString();
		}

		// Token: 0x04003FCB RID: 16331
		public int itemId;

		// Token: 0x04003FCC RID: 16332
		public IconItemCtrl iconItemCtrl;

		// Token: 0x04003FCD RID: 16333
		public PguiTextCtrl expBonus;

		// Token: 0x04003FCE RID: 16334
		public PguiTextCtrl itemNum;

		// Token: 0x04003FCF RID: 16335
		public PguiImageCtrl imgCount;

		// Token: 0x04003FD0 RID: 16336
		public PguiTextCtrl itemCount;

		// Token: 0x04003FD1 RID: 16337
		public PguiImageCtrl ColorBase;
	}

	// Token: 0x02000A00 RID: 2560
	public class LvUpTab
	{
		// Token: 0x06003E20 RID: 15904 RVA: 0x001E4A10 File Offset: 0x001E2C10
		public LvUpTab(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Num_Lv_L = baseTr.Find("ExpInfo/Num_Lv_L").GetComponent<PguiTextCtrl>();
			this.Num_Lv_R = baseTr.Find("ExpInfo/Num_Lv_R").GetComponent<PguiTextCtrl>();
			this.Num_Result = baseTr.Find("ExpInfo/Result_Lvup/Num_Result").GetComponent<PguiTextCtrl>();
			this.Result_Lvup = baseTr.Find("ExpInfo/Result_Lvup").GetComponent<SimpleAnimation>();
			this.Result_Lvup.gameObject.SetActive(false);
			this.ScrollView = baseTr.Find("Base/ScrollView").GetComponent<ReuseScroll>();
			this.itemListBar = new List<SelCharaGrowCtrl.CommonGUI.ItemListBar>();
			this.iconItemList = new List<SelCharaGrowLevel.LvUpItem>();
			this.iconItemMap = new Dictionary<int, SelCharaGrowLevel.LvUpItem>();
			this.iconBaseList = new List<RectTransform>();
			this.Num_Exp_Next = baseTr.Find("ExpInfo/Num_Exp_Next").GetComponent<PguiTextCtrl>();
			this.Gage_Up = baseTr.Find("ExpInfo/ExpGage/Gage_Up").GetComponent<PguiImageCtrl>();
			this.Gage_Up.gameObject.SetActive(false);
			this.Gage = baseTr.Find("ExpInfo/ExpGage/Gage").GetComponent<PguiImageCtrl>();
			this.Img_Yaji = baseTr.Find("ExpInfo/Num_Lv_L/Img_Yaji").GetComponent<PguiImageCtrl>();
			this.AEImage_result = baseTr.Find("ExpInfo/AEImage_result").GetComponent<AEImage>();
			this.AEImage_result.gameObject.SetActive(false);
			this.AEImage_LevelUP = baseTr.Find("ExpInfo/AEImage_LevelUP").GetComponent<AEImage>();
			this.AEImage_LevelUP.gameObject.SetActive(false);
		}

		// Token: 0x06003E21 RID: 15905 RVA: 0x001E4B91 File Offset: 0x001E2D91
		public void SetAEImageLevelUP()
		{
			this.AEImage_LevelUP.gameObject.SetActive(true);
			this.AEImage_LevelUP.playTime = 0f;
			this.AEImage_LevelUP.autoPlay = true;
			this.AEImage_LevelUP.playLoop = false;
		}

		// Token: 0x06003E22 RID: 15906 RVA: 0x001E4BCC File Offset: 0x001E2DCC
		public void SetActiveGage(bool isActive)
		{
			this.Gage.gameObject.SetActive(isActive);
		}

		// Token: 0x04003FD2 RID: 16338
		public GameObject baseObj;

		// Token: 0x04003FD3 RID: 16339
		public PguiTextCtrl Num_Lv_L;

		// Token: 0x04003FD4 RID: 16340
		public PguiTextCtrl Num_Lv_R;

		// Token: 0x04003FD5 RID: 16341
		public PguiTextCtrl Num_Result;

		// Token: 0x04003FD6 RID: 16342
		public SimpleAnimation Result_Lvup;

		// Token: 0x04003FD7 RID: 16343
		public ReuseScroll ScrollView;

		// Token: 0x04003FD8 RID: 16344
		public List<SelCharaGrowCtrl.CommonGUI.ItemListBar> itemListBar;

		// Token: 0x04003FD9 RID: 16345
		public List<SelCharaGrowLevel.LvUpItem> iconItemList;

		// Token: 0x04003FDA RID: 16346
		public Dictionary<int, SelCharaGrowLevel.LvUpItem> iconItemMap;

		// Token: 0x04003FDB RID: 16347
		public List<RectTransform> iconBaseList;

		// Token: 0x04003FDC RID: 16348
		public PguiTextCtrl Num_Exp_Next;

		// Token: 0x04003FDD RID: 16349
		public PguiImageCtrl Gage_Up;

		// Token: 0x04003FDE RID: 16350
		public PguiImageCtrl Gage;

		// Token: 0x04003FDF RID: 16351
		public PguiImageCtrl Img_Yaji;

		// Token: 0x04003FE0 RID: 16352
		public AEImage AEImage_result;

		// Token: 0x04003FE1 RID: 16353
		public AEImage AEImage_LevelUP;
	}

	// Token: 0x02000A01 RID: 2561
	public class LvLimitOpen
	{
		// Token: 0x06003E23 RID: 15907 RVA: 0x001E4BE0 File Offset: 0x001E2DE0
		public LvLimitOpen(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.ButtonC = baseTr.Find("ButtonC").GetComponent<PguiButtonCtrl>();
			this.Txt_ItemName01 = baseTr.Find("LayoutGroup/ItemInfo/Txt_ItemName").GetComponent<PguiTextCtrl>();
			this.Txt_ItemName02 = baseTr.Find("LayoutGroup/ItemInfo2/Txt_ItemName").GetComponent<PguiTextCtrl>();
			this.Num_Own01 = baseTr.Find("LayoutGroup/ItemInfo/Num_Own").GetComponent<PguiTextCtrl>();
			this.Num_Own02 = baseTr.Find("LayoutGroup/ItemInfo2/Num_Own").GetComponent<PguiTextCtrl>();
			this.Num_After01 = baseTr.Find("LayoutGroup/EffectInfo/Num_After01").GetComponent<PguiTextCtrl>();
			this.iconItemCtrl01 = baseTr.Find("LayoutGroup/ItemInfo/ItemIcon/Icon_Item").GetComponent<IconItemCtrl>();
			this.iconItemCtrl02 = baseTr.Find("LayoutGroup/ItemInfo2/ItemIcon/Icon_Item").GetComponent<IconItemCtrl>();
			this.AEImage_result = baseTr.Find("AEImage_result").GetComponent<AEImage>();
			this.iconItemCtrl02.transform.parent.parent.gameObject.SetActive(false);
			this.AEImage_result.gameObject.SetActive(false);
		}

		// Token: 0x06003E24 RID: 15908 RVA: 0x001E4CF8 File Offset: 0x001E2EF8
		public void Setup(SelCharaGrowLevel.LvLimitOpen.SetupParam param)
		{
			ItemStaticBase itemStaticBase = DataManager.DmItem.GetItemStaticBase(param.levelLimitData.needItemId01);
			this.iconItemCtrl01.Setup(itemStaticBase);
			this.Txt_ItemName01.text = itemStaticBase.GetName() ?? "";
			this.Num_After01.text = string.Format("レベル上限＋{0}", param.diffLevel);
			ItemData userItemData = DataManager.DmItem.GetUserItemData(param.levelLimitData.needItemId01);
			int needItemNum = param.levelLimitData.needItemNum01;
			int num = DataManager.DmItem.GetUserItemData(30101).num;
			this.Num_Own01.text = ((userItemData.num >= needItemNum) ? string.Format("{0}/{1}", userItemData.num, needItemNum) : string.Format("{0}{1}{2}/{3}", new object[]
			{
				PrjUtil.ColorRedStartTag,
				userItemData.num,
				PrjUtil.ColorEndTag,
				needItemNum
			}));
			int num2 = 0;
			int num3 = 0;
			this.iconItemCtrl02.transform.parent.parent.gameObject.SetActive(param.levelLimitData.needItemId02 != 0);
			if (param.levelLimitData.needItemId02 != 0)
			{
				ItemStaticBase itemStaticBase2 = DataManager.DmItem.GetItemStaticBase(param.levelLimitData.needItemId02);
				this.iconItemCtrl02.Setup(itemStaticBase2);
				this.Txt_ItemName02.text = itemStaticBase2.GetName() ?? "";
				ItemData userItemData2 = DataManager.DmItem.GetUserItemData(param.levelLimitData.needItemId02);
				num2 = param.levelLimitData.needItemNum02;
				num3 = userItemData2.num;
				this.Num_Own02.text = ((userItemData2.num >= num2) ? string.Format("{0}/{1}", userItemData2.num, num2) : string.Format("{0}{1}{2}/{3}", new object[]
				{
					PrjUtil.ColorRedStartTag,
					userItemData2.num,
					PrjUtil.ColorEndTag,
					num2
				}));
			}
			this.ButtonC.SetActEnable(userItemData.num >= needItemNum && (param.levelLimitData.needItemId02 == 0 || (param.levelLimitData.needItemId02 != 0 && num3 >= num2)) && num >= param.levelLimitData.needGoldNum, false, false);
		}

		// Token: 0x04003FE2 RID: 16354
		public GameObject baseObj;

		// Token: 0x04003FE3 RID: 16355
		public PguiButtonCtrl ButtonC;

		// Token: 0x04003FE4 RID: 16356
		public PguiTextCtrl Txt_ItemName01;

		// Token: 0x04003FE5 RID: 16357
		public PguiTextCtrl Txt_ItemName02;

		// Token: 0x04003FE6 RID: 16358
		public PguiTextCtrl Num_Own01;

		// Token: 0x04003FE7 RID: 16359
		public PguiTextCtrl Num_Own02;

		// Token: 0x04003FE8 RID: 16360
		public PguiTextCtrl Num_After01;

		// Token: 0x04003FE9 RID: 16361
		public IconItemCtrl iconItemCtrl01;

		// Token: 0x04003FEA RID: 16362
		public IconItemCtrl iconItemCtrl02;

		// Token: 0x04003FEB RID: 16363
		public AEImage AEImage_result;

		// Token: 0x0200115F RID: 4447
		public class SetupParam
		{
			// Token: 0x04005F75 RID: 24437
			public DataManagerChara.LevelLimitData levelLimitData;

			// Token: 0x04005F76 RID: 24438
			public int diffLevel;
		}
	}

	// Token: 0x02000A02 RID: 2562
	public class WindowLvUp
	{
		// Token: 0x06003E25 RID: 15909 RVA: 0x001E4F68 File Offset: 0x001E3168
		public WindowLvUp(Transform baseTr)
		{
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.iconItemList = new List<SelCharaGrowLevel.LvUpItem>();
			this.Num_CoinUse = baseTr.Find("Base/Window/ItemUse/Num").GetComponent<PguiTextCtrl>();
			this.Num_CoinOwn = baseTr.Find("Base/Window/ItemOwn/Num").GetComponent<PguiTextCtrl>();
			GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("CmnIconFrame/GUI/Prefab/Icon_Chara"), baseTr.Find("Base/Window/Icon_Chara"));
			this.iconChara = gameObject.GetComponent<IconCharaCtrl>();
			this.iconCharaObject = baseTr.Find("Base/Window/Icon_Chara").gameObject;
			this.Txt_CharaName = baseTr.Find("Base/Window/Txt_CharaName").GetComponent<PguiTextCtrl>();
			this.Num_Lv_Before = baseTr.Find("Base/Window/ExpInfo/Num_Lv_Before").GetComponent<PguiTextCtrl>();
			this.Num_Lv_After = baseTr.Find("Base/Window/ExpInfo/Num_Lv_After").GetComponent<PguiTextCtrl>();
			this.Gage_Up = baseTr.Find("Base/Window/ExpInfo/ExpGage/Gage_Up").GetComponent<PguiImageCtrl>();
			this.Gage = baseTr.Find("Base/Window/ExpInfo/ExpGage/Gage").GetComponent<PguiImageCtrl>();
			this.ScrollView = baseTr.Find("Base/Window/ItemUseInfo/ScrollView").GetComponent<ReuseScroll>();
			this.Num_Exp_Next = baseTr.Find("Base/Window/ExpInfo/Num_Exp_Next").GetComponent<PguiTextCtrl>();
		}

		// Token: 0x06003E26 RID: 15910 RVA: 0x001E509A File Offset: 0x001E329A
		public void SetActiveGage(bool isActive)
		{
			this.Gage.gameObject.SetActive(isActive);
		}

		// Token: 0x04003FEC RID: 16364
		public const int SCROLL_ITEM_NUN_H = 5;

		// Token: 0x04003FED RID: 16365
		public PguiOpenWindowCtrl owCtrl;

		// Token: 0x04003FEE RID: 16366
		public PguiTextCtrl Num_CoinUse;

		// Token: 0x04003FEF RID: 16367
		public PguiTextCtrl Num_CoinOwn;

		// Token: 0x04003FF0 RID: 16368
		public List<SelCharaGrowLevel.LvUpItem> iconItemList;

		// Token: 0x04003FF1 RID: 16369
		public List<ItemInput> itemList;

		// Token: 0x04003FF2 RID: 16370
		public IconCharaCtrl iconChara;

		// Token: 0x04003FF3 RID: 16371
		public GameObject iconCharaObject;

		// Token: 0x04003FF4 RID: 16372
		public PguiTextCtrl Txt_CharaName;

		// Token: 0x04003FF5 RID: 16373
		public PguiTextCtrl Num_Lv_Before;

		// Token: 0x04003FF6 RID: 16374
		public PguiTextCtrl Num_Lv_After;

		// Token: 0x04003FF7 RID: 16375
		public PguiImageCtrl Gage_Up;

		// Token: 0x04003FF8 RID: 16376
		public PguiImageCtrl Gage;

		// Token: 0x04003FF9 RID: 16377
		public ReuseScroll ScrollView;

		// Token: 0x04003FFA RID: 16378
		public PguiTextCtrl Num_Exp_Next;
	}

	// Token: 0x02000A03 RID: 2563
	public class LvupAuth
	{
		// Token: 0x06003E27 RID: 15911 RVA: 0x001E50B0 File Offset: 0x001E32B0
		public LvupAuth(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.AEImage_JapamanFeed = baseTr.Find("AEImage_JapamanFeed").GetComponent<PguiAECtrl>();
			this.AEImage_AList = new List<PguiReplaceAECtrl>();
			this.AEImage_BList = new List<PguiReplaceAECtrl>();
			this.AEImage_CList = new List<PguiReplaceAECtrl>();
			this.AEImage_AList.Add(baseTr.Find("Null_A_01_R/AEImage_Japaman").GetComponent<PguiReplaceAECtrl>());
			this.AEImage_AList.Add(baseTr.Find("Null_A_02_R/AEImage_Japaman").GetComponent<PguiReplaceAECtrl>());
			this.AEImage_AList.Add(baseTr.Find("Null_A_03_L/AEImage_Japaman").GetComponent<PguiReplaceAECtrl>());
			this.AEImage_AList.Add(baseTr.Find("Null_A_04_R/AEImage_Japaman").GetComponent<PguiReplaceAECtrl>());
			this.AEImage_AList.Add(baseTr.Find("Null_A_05_R/AEImage_Japaman").GetComponent<PguiReplaceAECtrl>());
			this.AEImage_AList.Add(baseTr.Find("Null_A_06_L/AEImage_Japaman").GetComponent<PguiReplaceAECtrl>());
			this.AEImage_BList.Add(baseTr.Find("Null_B_01_L/AEImage_Japaman").GetComponent<PguiReplaceAECtrl>());
			this.AEImage_BList.Add(baseTr.Find("Null_B_02_L/AEImage_Japaman").GetComponent<PguiReplaceAECtrl>());
			this.AEImage_BList.Add(baseTr.Find("Null_B_03_L/AEImage_Japaman").GetComponent<PguiReplaceAECtrl>());
			this.AEImage_BList.Add(baseTr.Find("Null_B_04_R/AEImage_Japaman").GetComponent<PguiReplaceAECtrl>());
			this.AEImage_CList.Add(baseTr.Find("Null_C_01_R/AEImage_Japaman").GetComponent<PguiReplaceAECtrl>());
			this.AEImage_CList.Add(baseTr.Find("Null_C_02_L/AEImage_Japaman").GetComponent<PguiReplaceAECtrl>());
			this.AEImage_CList.Add(baseTr.Find("Null_C_03_R/AEImage_Japaman").GetComponent<PguiReplaceAECtrl>());
			foreach (PguiReplaceAECtrl pguiReplaceAECtrl in this.AEImage_AList)
			{
				pguiReplaceAECtrl.InitForce();
			}
			foreach (PguiReplaceAECtrl pguiReplaceAECtrl2 in this.AEImage_BList)
			{
				pguiReplaceAECtrl2.InitForce();
			}
			foreach (PguiReplaceAECtrl pguiReplaceAECtrl3 in this.AEImage_CList)
			{
				pguiReplaceAECtrl3.InitForce();
			}
		}

		// Token: 0x04003FFB RID: 16379
		public GameObject baseObj;

		// Token: 0x04003FFC RID: 16380
		public PguiAECtrl AEImage_JapamanFeed;

		// Token: 0x04003FFD RID: 16381
		public List<PguiReplaceAECtrl> AEImage_AList;

		// Token: 0x04003FFE RID: 16382
		public List<PguiReplaceAECtrl> AEImage_BList;

		// Token: 0x04003FFF RID: 16383
		public List<PguiReplaceAECtrl> AEImage_CList;
	}

	// Token: 0x02000A04 RID: 2564
	public class WindowLevelLimitOver
	{
		// Token: 0x06003E28 RID: 15912 RVA: 0x001E5328 File Offset: 0x001E3528
		public WindowLevelLimitOver(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Num_Lv_Before = baseTr.Find("Base/Window/Txt/Img_Yaji/Num_Lv_Before").GetComponent<PguiTextCtrl>();
			this.Num_Lv_After = baseTr.Find("Base/Window/Txt/Img_Yaji/Num_Lv_After").GetComponent<PguiTextCtrl>();
			Transform transform = baseTr.Find("Base/Window/LayoutGroup/ItemInfo");
			this.Txt01 = transform.Find("Txt01").GetComponent<PguiTextCtrl>();
			this.Num_BeforeTxt01 = transform.Find("Img_Yaji/Num_BeforeTxt").GetComponent<PguiTextCtrl>();
			this.Num_AfterTxt01 = transform.Find("Img_Yaji/Num_AfterTxt").GetComponent<PguiTextCtrl>();
			this.Icon_Tex01 = transform.Find("Icon_Tex").GetComponent<PguiRawImageCtrl>();
			Transform transform2 = baseTr.Find("Base/Window/LayoutGroup/ItemInfo2");
			this.Txt02 = transform2.Find("Txt01").GetComponent<PguiTextCtrl>();
			this.Num_BeforeTxt02 = transform2.Find("Img_Yaji/Num_BeforeTxt").GetComponent<PguiTextCtrl>();
			this.Num_AfterTxt02 = transform2.Find("Img_Yaji/Num_AfterTxt").GetComponent<PguiTextCtrl>();
			this.Icon_Tex02 = transform2.Find("Icon_Tex").GetComponent<PguiRawImageCtrl>();
			transform2.gameObject.SetActive(false);
			Transform transform3 = baseTr.Find("Base/Window/LayoutGroup/UseCoin");
			this.Num_Coin_BeforeTxt = transform3.Find("Img_Yaji/Num_BeforeTxt").GetComponent<PguiTextCtrl>();
			this.Num_Coin_AfterTxt = transform3.Find("Img_Yaji/Num_AfterTxt").GetComponent<PguiTextCtrl>();
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
		}

		// Token: 0x06003E29 RID: 15913 RVA: 0x001E548C File Offset: 0x001E368C
		public void Setup(SelCharaGrowLevel.WindowLevelLimitOver.SetupParam param)
		{
			CharaPackData charaData = param.charaData;
			DataManagerChara.LevelLimitData levelLimitData = DataManager.DmChara.GetLevelLimitData(charaData.dynamicData.levelLimitId + 1);
			if (levelLimitData != null)
			{
				ItemData userItemData = DataManager.DmItem.GetUserItemData(levelLimitData.needItemId01);
				this.Num_Lv_Before.text = string.Format("{0}", charaData.dynamicData.level);
				this.Num_Lv_After.text = string.Format("{0}", charaData.dynamicData.limitLevelRankMax + levelLimitData.IncreaseMaxLevel);
				this.Txt01.text = userItemData.staticData.GetName();
				this.Num_BeforeTxt01.text = string.Format("{0}", userItemData.num);
				this.Num_AfterTxt01.text = string.Format("{0}", userItemData.num - levelLimitData.needItemNum01);
				this.Icon_Tex01.SetRawImage(userItemData.staticData.GetIconName(), true, false, null);
				int num = DataManager.DmItem.GetUserItemData(30101).num;
				this.Num_Coin_BeforeTxt.text = string.Format("{0}", num);
				this.Num_Coin_AfterTxt.text = string.Format("{0}", num - levelLimitData.needGoldNum);
				this.Icon_Tex02.transform.parent.gameObject.SetActive(levelLimitData.needItemId02 != 0);
				if (levelLimitData.needItemId02 == 0)
				{
					return;
				}
				ItemData userItemData2 = DataManager.DmItem.GetUserItemData(levelLimitData.needItemId02);
				this.Txt02.text = userItemData2.staticData.GetName();
				this.Num_BeforeTxt02.text = string.Format("{0}", userItemData2.num);
				this.Num_AfterTxt02.text = string.Format("{0}", userItemData2.num - levelLimitData.needItemNum02);
				this.Icon_Tex02.SetRawImage(userItemData2.staticData.GetIconName(), true, false, null);
			}
		}

		// Token: 0x04004000 RID: 16384
		public GameObject baseObj;

		// Token: 0x04004001 RID: 16385
		public PguiTextCtrl Num_Lv_Before;

		// Token: 0x04004002 RID: 16386
		public PguiTextCtrl Num_Lv_After;

		// Token: 0x04004003 RID: 16387
		public PguiTextCtrl Txt01;

		// Token: 0x04004004 RID: 16388
		public PguiTextCtrl Num_BeforeTxt01;

		// Token: 0x04004005 RID: 16389
		public PguiTextCtrl Num_AfterTxt01;

		// Token: 0x04004006 RID: 16390
		public PguiRawImageCtrl Icon_Tex01;

		// Token: 0x04004007 RID: 16391
		public PguiTextCtrl Txt02;

		// Token: 0x04004008 RID: 16392
		public PguiTextCtrl Num_BeforeTxt02;

		// Token: 0x04004009 RID: 16393
		public PguiTextCtrl Num_AfterTxt02;

		// Token: 0x0400400A RID: 16394
		public PguiRawImageCtrl Icon_Tex02;

		// Token: 0x0400400B RID: 16395
		public PguiTextCtrl Num_Coin_BeforeTxt;

		// Token: 0x0400400C RID: 16396
		public PguiTextCtrl Num_Coin_AfterTxt;

		// Token: 0x0400400D RID: 16397
		public PguiOpenWindowCtrl owCtrl;

		// Token: 0x02001160 RID: 4448
		public class SetupParam
		{
			// Token: 0x04005F77 RID: 24439
			public CharaPackData charaData;
		}
	}

	// Token: 0x02000A05 RID: 2565
	public class WindowItemUse
	{
		// Token: 0x06003E2A RID: 15914 RVA: 0x001E56A0 File Offset: 0x001E38A0
		public WindowItemUse(Transform baseTr)
		{
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.Btn_Plus = baseTr.Find("Base/Window/Btn_Plus").GetComponent<PguiButtonCtrl>();
			this.Btn_Minus = baseTr.Find("Base/Window/Btn_Minus").GetComponent<PguiButtonCtrl>();
			this.BtnClose = baseTr.Find("Base/Window/BtnClose").GetComponent<PguiButtonCtrl>();
			this.BtnClose.androidBackKeyTarget = true;
			this.ButtonC = baseTr.Find("Base/Window/ButtonC").GetComponent<PguiButtonCtrl>();
			this.Num_Lv_Before = baseTr.Find("Base/Window/ExpInfo/Num_Lv_Before").GetComponent<PguiTextCtrl>();
			this.Num_Lv_After = baseTr.Find("Base/Window/ExpInfo/Num_Lv_After").GetComponent<PguiTextCtrl>();
			this.Num_Exp_Next = baseTr.Find("Base/Window/ExpInfo/Num_Exp_Next").GetComponent<PguiTextCtrl>();
			this.Gage_Up = baseTr.Find("Base/Window/ExpInfo/ExpGage/Gage_Up").GetComponent<PguiImageCtrl>();
			this.Gage_Up.gameObject.SetActive(false);
			this.Gage = baseTr.Find("Base/Window/ExpInfo/ExpGage/Gage").GetComponent<PguiImageCtrl>();
			this.Img_Yaji = baseTr.Find("Base/Window/ExpInfo/Img_Yaji").GetComponent<PguiImageCtrl>();
			this.Num_BeforeCoin = baseTr.Find("Base/Window/Parts_ItemUseCoin/Num_BeforeTxt").GetComponent<PguiTextCtrl>();
			this.Num_AfterCoin = baseTr.Find("Base/Window/Parts_ItemUseCoin/Num_AfterTxt").GetComponent<PguiTextCtrl>();
			this.Text = baseTr.Find("Base/Window/Title/Text").GetComponent<PguiTextCtrl>();
			this.Base = baseTr.Find("Base").GetComponent<SimpleAnimation>();
			this.SliderBar = baseTr.Find("Base/Window/SliderBar").GetComponent<Slider>();
			Object.Destroy(baseTr.Find("Base/Window/Icon_Item").GetComponent<PguiNestPrefab>());
			Transform transform = baseTr.Find("Base/Window/Icon_Item");
			GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneCharaEdit/GUI/Prefab/CharaGrow_ItemIconSet"), transform);
			gameObject.name = "icon";
			this.lvUpItem = new SelCharaGrowLevel.LvUpItem();
			GameObject gameObject2 = Object.Instantiate<GameObject>((GameObject)Resources.Load("CmnIconFrame/GUI/Prefab/Icon_Item"), gameObject.transform.Find("Icon_Item"));
			this.lvUpItem.iconItemCtrl = gameObject2.GetComponent<IconItemCtrl>();
			this.lvUpItem.expBonus = gameObject.transform.Find("Txt_ExpBonus").GetComponent<PguiTextCtrl>();
			this.lvUpItem.itemNum = gameObject.transform.Find("Num_Own").GetComponent<PguiTextCtrl>();
			this.lvUpItem.ColorBase = gameObject.transform.Find("ColorBase").GetComponent<PguiImageCtrl>();
			this.lvUpItem.imgCount = gameObject.transform.Find("Count").GetComponent<PguiImageCtrl>();
			this.lvUpItem.itemCount = gameObject.transform.Find("Count/Num_Count").GetComponent<PguiTextCtrl>();
			this.lvUpItem.expBonus.gameObject.SetActive(false);
			this.lvUpItem.itemNum.gameObject.SetActive(false);
			this.lvUpItem.ColorBase.gameObject.SetActive(false);
			this.lvUpItem.imgCount.gameObject.SetActive(false);
			this.lvUpItem.itemCount.gameObject.SetActive(false);
		}

		// Token: 0x0400400E RID: 16398
		public PguiOpenWindowCtrl owCtrl;

		// Token: 0x0400400F RID: 16399
		public PguiButtonCtrl Btn_Plus;

		// Token: 0x04004010 RID: 16400
		public PguiButtonCtrl Btn_Minus;

		// Token: 0x04004011 RID: 16401
		public PguiButtonCtrl BtnClose;

		// Token: 0x04004012 RID: 16402
		public PguiButtonCtrl ButtonC;

		// Token: 0x04004013 RID: 16403
		public PguiTextCtrl Num_Exp_Next;

		// Token: 0x04004014 RID: 16404
		public PguiImageCtrl Gage_Up;

		// Token: 0x04004015 RID: 16405
		public PguiImageCtrl Gage;

		// Token: 0x04004016 RID: 16406
		public PguiImageCtrl Img_Yaji;

		// Token: 0x04004017 RID: 16407
		public PguiTextCtrl Num_Lv_Before;

		// Token: 0x04004018 RID: 16408
		public PguiTextCtrl Num_Lv_After;

		// Token: 0x04004019 RID: 16409
		public PguiTextCtrl Text;

		// Token: 0x0400401A RID: 16410
		public PguiTextCtrl Num_BeforeCoin;

		// Token: 0x0400401B RID: 16411
		public PguiTextCtrl Num_AfterCoin;

		// Token: 0x0400401C RID: 16412
		public SimpleAnimation Base;

		// Token: 0x0400401D RID: 16413
		public SelCharaGrowLevel.LvUpItem lvUpItem;

		// Token: 0x0400401E RID: 16414
		public Slider SliderBar;
	}

	// Token: 0x02000A06 RID: 2566
	public class CharaLevelUpGUI
	{
		// Token: 0x0400401F RID: 16415
		public SelCharaGrowLevel.LvupAuth lvupAuth;

		// Token: 0x04004020 RID: 16416
		public SelCharaGrowLevel.WindowLvUp lvUpWindow;

		// Token: 0x04004021 RID: 16417
		public SelCharaGrowLevel.WindowItemUse itemUseWindow;

		// Token: 0x04004022 RID: 16418
		public SelCharaGrowLevel.WindowLevelLimitOver levelLimitOverWindow;

		// Token: 0x04004023 RID: 16419
		public SelCharaGrowLevel.LvUpTab lvUpTab;
	}
}
