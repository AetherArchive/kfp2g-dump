using System;
using System.Collections.Generic;
using AEAuth3;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.UI;

public class SelCharaGrowLevel
{
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

	public SelCharaGrowLevel.CharaLevelUpGUI LevelUpGUI;

	public class LvUpItem
	{
		public void SetUp(bool isMax, int itemDataNum, int count)
		{
			this.iconItemCtrl.SetActEnable(!isMax);
			this.itemNum.gameObject.SetActive(true);
			this.itemNum.text = itemDataNum.ToString();
			this.imgCount.gameObject.SetActive(0 < count);
			this.itemCount.gameObject.SetActive(0 < count);
			this.itemCount.text = count.ToString();
		}

		public int itemId;

		public IconItemCtrl iconItemCtrl;

		public PguiTextCtrl expBonus;

		public PguiTextCtrl itemNum;

		public PguiImageCtrl imgCount;

		public PguiTextCtrl itemCount;

		public PguiImageCtrl ColorBase;
	}

	public class LvUpTab
	{
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

		public void SetAEImageLevelUP()
		{
			this.AEImage_LevelUP.gameObject.SetActive(true);
			this.AEImage_LevelUP.playTime = 0f;
			this.AEImage_LevelUP.autoPlay = true;
			this.AEImage_LevelUP.playLoop = false;
		}

		public void SetActiveGage(bool isActive)
		{
			this.Gage.gameObject.SetActive(isActive);
		}

		public GameObject baseObj;

		public PguiTextCtrl Num_Lv_L;

		public PguiTextCtrl Num_Lv_R;

		public PguiTextCtrl Num_Result;

		public SimpleAnimation Result_Lvup;

		public ReuseScroll ScrollView;

		public List<SelCharaGrowCtrl.CommonGUI.ItemListBar> itemListBar;

		public List<SelCharaGrowLevel.LvUpItem> iconItemList;

		public Dictionary<int, SelCharaGrowLevel.LvUpItem> iconItemMap;

		public List<RectTransform> iconBaseList;

		public PguiTextCtrl Num_Exp_Next;

		public PguiImageCtrl Gage_Up;

		public PguiImageCtrl Gage;

		public PguiImageCtrl Img_Yaji;

		public AEImage AEImage_result;

		public AEImage AEImage_LevelUP;
	}

	public class LvLimitOpen
	{
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

		public GameObject baseObj;

		public PguiButtonCtrl ButtonC;

		public PguiTextCtrl Txt_ItemName01;

		public PguiTextCtrl Txt_ItemName02;

		public PguiTextCtrl Num_Own01;

		public PguiTextCtrl Num_Own02;

		public PguiTextCtrl Num_After01;

		public IconItemCtrl iconItemCtrl01;

		public IconItemCtrl iconItemCtrl02;

		public AEImage AEImage_result;

		public class SetupParam
		{
			public DataManagerChara.LevelLimitData levelLimitData;

			public int diffLevel;
		}
	}

	public class WindowLvUp
	{
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

		public void SetActiveGage(bool isActive)
		{
			this.Gage.gameObject.SetActive(isActive);
		}

		public const int SCROLL_ITEM_NUN_H = 5;

		public PguiOpenWindowCtrl owCtrl;

		public PguiTextCtrl Num_CoinUse;

		public PguiTextCtrl Num_CoinOwn;

		public List<SelCharaGrowLevel.LvUpItem> iconItemList;

		public List<ItemInput> itemList;

		public IconCharaCtrl iconChara;

		public GameObject iconCharaObject;

		public PguiTextCtrl Txt_CharaName;

		public PguiTextCtrl Num_Lv_Before;

		public PguiTextCtrl Num_Lv_After;

		public PguiImageCtrl Gage_Up;

		public PguiImageCtrl Gage;

		public ReuseScroll ScrollView;

		public PguiTextCtrl Num_Exp_Next;
	}

	public class LvupAuth
	{
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

		public GameObject baseObj;

		public PguiAECtrl AEImage_JapamanFeed;

		public List<PguiReplaceAECtrl> AEImage_AList;

		public List<PguiReplaceAECtrl> AEImage_BList;

		public List<PguiReplaceAECtrl> AEImage_CList;
	}

	public class WindowLevelLimitOver
	{
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

		public GameObject baseObj;

		public PguiTextCtrl Num_Lv_Before;

		public PguiTextCtrl Num_Lv_After;

		public PguiTextCtrl Txt01;

		public PguiTextCtrl Num_BeforeTxt01;

		public PguiTextCtrl Num_AfterTxt01;

		public PguiRawImageCtrl Icon_Tex01;

		public PguiTextCtrl Txt02;

		public PguiTextCtrl Num_BeforeTxt02;

		public PguiTextCtrl Num_AfterTxt02;

		public PguiRawImageCtrl Icon_Tex02;

		public PguiTextCtrl Num_Coin_BeforeTxt;

		public PguiTextCtrl Num_Coin_AfterTxt;

		public PguiOpenWindowCtrl owCtrl;

		public class SetupParam
		{
			public CharaPackData charaData;
		}
	}

	public class WindowItemUse
	{
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

		public PguiOpenWindowCtrl owCtrl;

		public PguiButtonCtrl Btn_Plus;

		public PguiButtonCtrl Btn_Minus;

		public PguiButtonCtrl BtnClose;

		public PguiButtonCtrl ButtonC;

		public PguiTextCtrl Num_Exp_Next;

		public PguiImageCtrl Gage_Up;

		public PguiImageCtrl Gage;

		public PguiImageCtrl Img_Yaji;

		public PguiTextCtrl Num_Lv_Before;

		public PguiTextCtrl Num_Lv_After;

		public PguiTextCtrl Text;

		public PguiTextCtrl Num_BeforeCoin;

		public PguiTextCtrl Num_AfterCoin;

		public SimpleAnimation Base;

		public SelCharaGrowLevel.LvUpItem lvUpItem;

		public Slider SliderBar;
	}

	public class CharaLevelUpGUI
	{
		public SelCharaGrowLevel.LvupAuth lvupAuth;

		public SelCharaGrowLevel.WindowLvUp lvUpWindow;

		public SelCharaGrowLevel.WindowItemUse itemUseWindow;

		public SelCharaGrowLevel.WindowLevelLimitOver levelLimitOverWindow;

		public SelCharaGrowLevel.LvUpTab lvUpTab;
	}
}
