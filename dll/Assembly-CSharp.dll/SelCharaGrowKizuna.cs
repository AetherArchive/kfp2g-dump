using System;
using System.Collections.Generic;
using AEAuth3;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000130 RID: 304
public class SelCharaGrowKizuna
{
	// Token: 0x17000348 RID: 840
	// (get) Token: 0x06001056 RID: 4182 RVA: 0x000C682B File Offset: 0x000C4A2B
	public SelCharaGrowKizuna.CharaGrowKizunaGUI GrowKizunaGUI
	{
		get
		{
			return this._growKizunaGUI;
		}
	}

	// Token: 0x17000349 RID: 841
	// (get) Token: 0x06001057 RID: 4183 RVA: 0x000C6833 File Offset: 0x000C4A33
	public SelCharaGrowKizuna.KizunaLevelUpGUI LevelUpGUI
	{
		get
		{
			return this._levelUpGUI;
		}
	}

	// Token: 0x06001058 RID: 4184 RVA: 0x000C683C File Offset: 0x000C4A3C
	public SelCharaGrowKizuna(Transform baseTr)
	{
		this._growKizunaGUI = new SelCharaGrowKizuna.CharaGrowKizunaGUI();
		this._growKizunaGUI.Setup(baseTr);
		this._levelUpGUI = new SelCharaGrowKizuna.KizunaLevelUpGUI();
		this._levelUpGUI.Setup(baseTr);
	}

	// Token: 0x06001059 RID: 4185 RVA: 0x000C6888 File Offset: 0x000C4A88
	public void Initialize(PguiOpenWindowCtrl.Callback windowCallBack)
	{
		List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>> list = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>
		{
			new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.NEGATIVE, "キャンセル"),
			new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, "決定")
		};
		this._growKizunaGUI.KizunaWindow.OpenWindowCtrl.Setup("なかよしレベル上限解放確認", null, list, true, windowCallBack, null, false);
	}

	// Token: 0x0600105A RID: 4186 RVA: 0x000C68E0 File Offset: 0x000C4AE0
	public void CreateLvUpItem(GameObject go, int i, int itemId, int attr)
	{
		GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneCharaEdit/GUI/Prefab/CharaGrow_ItemIconSet"), go.transform);
		gameObject.name = i.ToString();
		SelCharaGrowKizuna.KizunaLvUpItem kizunaLvUpItem = new SelCharaGrowKizuna.KizunaLvUpItem(Object.Instantiate<GameObject>((GameObject)Resources.Load("CmnIconFrame/GUI/Prefab/Icon_Item"), gameObject.transform.Find("Icon_Item")), gameObject, itemId);
		this._growKizunaGUI.KizunaLvUpTab.IconItemList.Add(kizunaLvUpItem);
		this._growKizunaGUI.KizunaLvUpTab.ItemListBar[attr].IconItemListKizuna.Add(kizunaLvUpItem);
	}

	// Token: 0x0600105B RID: 4187 RVA: 0x000C697C File Offset: 0x000C4B7C
	public string TabInfoText(int charaId)
	{
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(charaId);
		int nowLimitLevel = userCharaData.dynamicData.KizunaLimitLevel;
		GameLevelInfo gameLevelInfo = DataManager.DmServerMst.gameLevelInfoList.Find((GameLevelInfo x) => x.level == nowLimitLevel + 1);
		int kizunaLevelId = userCharaData.staticData.baseData.kizunaLevelId;
		int num = (gameLevelInfo.kizunaLevelExp.ContainsKey(kizunaLevelId) ? gameLevelInfo.kizunaLevelExp[kizunaLevelId].releaseItemId : 0);
		ItemStaticBase itemStaticBase = ((num == 0) ? null : DataManager.DmItem.GetItemStaticBase(num));
		string text = ((itemStaticBase != null) ? itemStaticBase.GetName() : "上限解放アイテム");
		if (nowLimitLevel != userCharaData.dynamicData.kizunaLevel)
		{
			return this.KIZUNA_LEVEL_UP_TAB_MESSAGE;
		}
		return text + "を使ってなかよしレベル上限を解放します";
	}

	// Token: 0x0600105C RID: 4188 RVA: 0x000C6A50 File Offset: 0x000C4C50
	public List<ItemStaticBase> GetExpAddItemList(int attr)
	{
		List<ItemStaticBase> list = new List<ItemStaticBase>();
		foreach (DataManagerServerMst.CharaLevelItem charaLevelItem in DataManager.DmServerMst.charaLevelItemDataList)
		{
			if (charaLevelItem.attribute == (CharaDef.AttributeType)attr && charaLevelItem.isKizuna == 1)
			{
				list.Add(DataManager.DmItem.GetItemStaticMap()[charaLevelItem.itemId]);
			}
		}
		list.Sort((ItemStaticBase x, ItemStaticBase y) => x.GetRarity() - y.GetRarity());
		return list;
	}

	// Token: 0x0600105D RID: 4189 RVA: 0x000C6AFC File Offset: 0x000C4CFC
	public void UpdateItemLvUp()
	{
		foreach (SelCharaGrowKizuna.KizunaLvUpItem kizunaLvUpItem in this._growKizunaGUI.KizunaLvUpTab.IconItemList)
		{
			Dictionary<int, ItemData> userItemMap = DataManager.DmItem.GetUserItemMap();
			ItemData itemData = (userItemMap.ContainsKey(kizunaLvUpItem.ItemId) ? userItemMap[kizunaLvUpItem.ItemId] : null);
			if (itemData != null)
			{
				kizunaLvUpItem.ItemNum.text = itemData.num.ToString();
				kizunaLvUpItem.ExpBonus.gameObject.SetActive(false);
			}
			else
			{
				kizunaLvUpItem.ItemNum.text = 0.ToString();
				kizunaLvUpItem.ExpBonus.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x0600105E RID: 4190 RVA: 0x000C6BD8 File Offset: 0x000C4DD8
	public void SetupLimitLvItemActivation(int charaId, bool isGrowMulti = false)
	{
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(charaId);
		CharaDynamicData dynamicData = userCharaData.dynamicData;
		int nowLimitLevel = (isGrowMulti ? (dynamicData.KizunaLimitLevel - 1) : dynamicData.KizunaLimitLevel);
		int kizunaLevel = dynamicData.kizunaLevel;
		GameLevelInfo gameLevelInfo = DataManager.DmServerMst.gameLevelInfoList.Find((GameLevelInfo x) => x.level == nowLimitLevel + 1);
		int kizunaLevelId = userCharaData.staticData.baseData.kizunaLevelId;
		bool flag = gameLevelInfo.kizunaLevelExp.ContainsKey(kizunaLevelId);
		bool flag2 = nowLimitLevel != kizunaLevel;
		if (flag)
		{
			this._growKizunaGUI.KizunaTab.BaseObject.SetActive(true);
			GameLevelInfo.KizunaLevelData kizunaLevelData = gameLevelInfo.kizunaLevelExp[kizunaLevelId];
			ItemStaticBase itemStaticBase = DataManager.DmItem.GetItemStaticBase(kizunaLevelData.releaseItemId);
			ItemData userItemData = DataManager.DmItem.GetUserItemData(kizunaLevelData.releaseItemId);
			int num = (isGrowMulti ? (userItemData.num + 1) : userItemData.num);
			this._growKizunaGUI.KizunaTab.IconItem.Setup(itemStaticBase);
			this._growKizunaGUI.KizunaWindow.IconTex.SetRawImage(itemStaticBase.GetIconName(), true, false, null);
			this._growKizunaGUI.KizunaTab.ImageResult.gameObject.SetActive(false);
			this._growKizunaGUI.KizunaTab.ReleaseButton.SetActEnable(true, false, false);
			this._growKizunaGUI.KizunaTab.TxtItemName.text = itemStaticBase.GetName();
			string text = ((num < kizunaLevelData.releaseItemNum) ? string.Format("<color=red>{0}</color>", num) : string.Format("{0}", num));
			this._growKizunaGUI.KizunaTab.NumItem.ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[]
			{
				text,
				string.Format("{0}", kizunaLevelData.releaseItemNum)
			});
			this._growKizunaGUI.KizunaTab.NowLimitLevelText.ReplaceTextByDefault(new string[] { "Param01" }, new string[] { string.Format("{0}", nowLimitLevel) });
			this._growKizunaGUI.KizunaTab.HeartInfoText.ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[]
			{
				string.Format("{0}", nowLimitLevel),
				string.Format("{0}", kizunaLevel)
			});
			PguiColorCtrl component = this._growKizunaGUI.KizunaTab.HeartInfoText.GetComponent<PguiColorCtrl>();
			component.InitForce();
			bool flag3 = nowLimitLevel == kizunaLevel && kizunaLevelData.releaseItemNum <= num;
			this._growKizunaGUI.KizunaTab.HeartInfoText.m_Text.color = (flag3 ? component.GetGameObjectById("NORMAL") : component.GetGameObjectById("CAUTION"));
			this._growKizunaGUI.KizunaTab.ReleaseButton.SetActEnable(flag3, false, false);
			this._growKizunaGUI.KizunaWindow.BeforeLevelText.text = string.Format("{0}", nowLimitLevel);
			this._growKizunaGUI.KizunaWindow.AfterLevelText.text = string.Format("{0}", nowLimitLevel + 1);
			string text2 = ((itemStaticBase != null) ? itemStaticBase.GetName() : "上限解放アイテム");
			this._growKizunaGUI.KizunaWindow.ItemNameText.text = text2;
			this._growKizunaGUI.KizunaWindow.ItemBeforeNumText.text = string.Format("{0}", num);
			this._growKizunaGUI.KizunaWindow.ItemAfterNumText.text = string.Format("{0}", num - kizunaLevelData.releaseItemNum);
		}
		else if (flag2)
		{
			this._growKizunaGUI.KizunaTab.BaseObject.SetActive(true);
		}
		else
		{
			this._growKizunaGUI.KizunaTab.BaseObject.SetActive(false);
			this._growKizunaGUI.KizunaTab.ReleaseButton.gameObject.SetActive(true);
			this._growKizunaGUI.KizunaTab.ReleaseButton.SetActEnable(false, false, false);
		}
		this._growKizunaGUI.KizunaTab.ReleaseButton.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickReleaseButton), PguiButtonCtrl.SoundType.DEFAULT);
	}

	// Token: 0x0600105F RID: 4191 RVA: 0x000C705C File Offset: 0x000C525C
	public void UpdateLimitLvItemActivation(int charaId)
	{
		this.SetupLimitLvItemActivation(charaId, false);
	}

	// Token: 0x06001060 RID: 4192 RVA: 0x000C7066 File Offset: 0x000C5266
	public void AdjustExpInfoActivation()
	{
		this._growKizunaGUI.KizunaLvUpTab.ExpInfoObject.SetActive(true);
	}

	// Token: 0x06001061 RID: 4193 RVA: 0x000C7080 File Offset: 0x000C5280
	public bool CheckIsPossibleLevelUp(int charaId)
	{
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(charaId);
		return userCharaData.dynamicData.KizunaLimitLevel > userCharaData.dynamicData.kizunaLevel;
	}

	// Token: 0x06001062 RID: 4194 RVA: 0x000C70B7 File Offset: 0x000C52B7
	public void SetActiveTab(bool islvMax)
	{
		this._growKizunaGUI.KizunaTab.ParentObject.SetActive(islvMax);
		this._growKizunaGUI.KizunaLvUpTab.BaseObj.SetActive(!islvMax);
	}

	// Token: 0x06001063 RID: 4195 RVA: 0x000C70E8 File Offset: 0x000C52E8
	private void OnClickReleaseButton(PguiButtonCtrl btn)
	{
		this._growKizunaGUI.KizunaWindow.OpenWindowCtrl.Open();
	}

	// Token: 0x04000E4A RID: 3658
	private readonly string KIZUNA_LEVEL_UP_TAB_MESSAGE = "マジカルキャンディを使ってなかよしポイントを獲得します";

	// Token: 0x04000E4B RID: 3659
	private SelCharaGrowKizuna.CharaGrowKizunaGUI _growKizunaGUI;

	// Token: 0x04000E4C RID: 3660
	private SelCharaGrowKizuna.KizunaLevelUpGUI _levelUpGUI;

	// Token: 0x020009F0 RID: 2544
	public class KizunaTab
	{
		// Token: 0x170008EC RID: 2284
		// (get) Token: 0x06003DA6 RID: 15782 RVA: 0x001E27C2 File Offset: 0x001E09C2
		public GameObject ParentObject
		{
			get
			{
				return this._parentObject;
			}
		}

		// Token: 0x170008ED RID: 2285
		// (get) Token: 0x06003DA7 RID: 15783 RVA: 0x001E27CA File Offset: 0x001E09CA
		public GameObject BaseObject
		{
			get
			{
				return this._baseObject;
			}
		}

		// Token: 0x170008EE RID: 2286
		// (get) Token: 0x06003DA8 RID: 15784 RVA: 0x001E27D2 File Offset: 0x001E09D2
		public PguiTextCtrl TxtItemName
		{
			get
			{
				return this._txtItemName;
			}
		}

		// Token: 0x170008EF RID: 2287
		// (get) Token: 0x06003DA9 RID: 15785 RVA: 0x001E27DA File Offset: 0x001E09DA
		public PguiTextCtrl NumItem
		{
			get
			{
				return this._numItem;
			}
		}

		// Token: 0x170008F0 RID: 2288
		// (get) Token: 0x06003DAA RID: 15786 RVA: 0x001E27E2 File Offset: 0x001E09E2
		public IconItemCtrl IconItem
		{
			get
			{
				return this._iconItem;
			}
		}

		// Token: 0x170008F1 RID: 2289
		// (get) Token: 0x06003DAB RID: 15787 RVA: 0x001E27EA File Offset: 0x001E09EA
		public PguiButtonCtrl ReleaseButton
		{
			get
			{
				return this._releaseButton;
			}
		}

		// Token: 0x170008F2 RID: 2290
		// (get) Token: 0x06003DAC RID: 15788 RVA: 0x001E27F2 File Offset: 0x001E09F2
		public PguiTextCtrl NowLimitLevelText
		{
			get
			{
				return this._nowLimitLevelText;
			}
		}

		// Token: 0x170008F3 RID: 2291
		// (get) Token: 0x06003DAD RID: 15789 RVA: 0x001E27FA File Offset: 0x001E09FA
		public PguiTextCtrl HeartInfoText
		{
			get
			{
				return this._heartInfoText;
			}
		}

		// Token: 0x170008F4 RID: 2292
		// (get) Token: 0x06003DAE RID: 15790 RVA: 0x001E2802 File Offset: 0x001E0A02
		public AEImage ImageResult
		{
			get
			{
				return this._imageResult;
			}
		}

		// Token: 0x06003DAF RID: 15791 RVA: 0x001E280C File Offset: 0x001E0A0C
		public KizunaTab(Transform baseTr)
		{
			this._parentObject = baseTr.gameObject;
			this._baseObject = baseTr.Find("Base").gameObject;
			this._txtItemName = this._baseObject.transform.Find("Txt_ItemName").GetComponent<PguiTextCtrl>();
			this._numItem = this._baseObject.transform.Find("Num_Own").GetComponent<PguiTextCtrl>();
			this._iconItem = this._baseObject.transform.Find("ItemIcon/Icon_Item").GetComponent<IconItemCtrl>();
			this._releaseButton = this._baseObject.transform.Find("ButtonC").GetComponent<PguiButtonCtrl>();
			this._nowLimitLevelText = this._baseObject.transform.Find("Num_Own_Level").GetComponent<PguiTextCtrl>();
			this._heartInfoText = this._baseObject.transform.Find("Txt_HeartInfo").GetComponent<PguiTextCtrl>();
			this._imageResult = this._baseObject.transform.Find("AEImage_result").GetComponent<AEImage>();
			this._expInfoObject = baseTr.Find("ExpInfo").gameObject;
			this._expInfoObject.SetActive(false);
			this.SetUpActivation();
		}

		// Token: 0x06003DB0 RID: 15792 RVA: 0x001E2949 File Offset: 0x001E0B49
		private void SetUpActivation()
		{
			this._baseObject.SetActive(true);
			this._imageResult.gameObject.SetActive(false);
		}

		// Token: 0x04003F4D RID: 16205
		private GameObject _expInfoObject;

		// Token: 0x04003F4E RID: 16206
		private GameObject _parentObject;

		// Token: 0x04003F4F RID: 16207
		private GameObject _baseObject;

		// Token: 0x04003F50 RID: 16208
		private PguiTextCtrl _txtItemName;

		// Token: 0x04003F51 RID: 16209
		private PguiTextCtrl _numItem;

		// Token: 0x04003F52 RID: 16210
		private IconItemCtrl _iconItem;

		// Token: 0x04003F53 RID: 16211
		private PguiButtonCtrl _releaseButton;

		// Token: 0x04003F54 RID: 16212
		private PguiTextCtrl _nowLimitLevelText;

		// Token: 0x04003F55 RID: 16213
		private PguiTextCtrl _heartInfoText;

		// Token: 0x04003F56 RID: 16214
		private AEImage _imageResult;
	}

	// Token: 0x020009F1 RID: 2545
	public class KizunaLvUpItem
	{
		// Token: 0x170008F5 RID: 2293
		// (get) Token: 0x06003DB1 RID: 15793 RVA: 0x001E2968 File Offset: 0x001E0B68
		public int ItemId
		{
			get
			{
				return this._itemId;
			}
		}

		// Token: 0x170008F6 RID: 2294
		// (get) Token: 0x06003DB2 RID: 15794 RVA: 0x001E2970 File Offset: 0x001E0B70
		public IconItemCtrl IconItemCtrl
		{
			get
			{
				return this._iconItemCtrl;
			}
		}

		// Token: 0x170008F7 RID: 2295
		// (get) Token: 0x06003DB3 RID: 15795 RVA: 0x001E2978 File Offset: 0x001E0B78
		public PguiTextCtrl ExpBonus
		{
			get
			{
				return this._expBonus;
			}
		}

		// Token: 0x170008F8 RID: 2296
		// (get) Token: 0x06003DB4 RID: 15796 RVA: 0x001E2980 File Offset: 0x001E0B80
		public PguiTextCtrl ItemNum
		{
			get
			{
				return this._itemNum;
			}
		}

		// Token: 0x06003DB5 RID: 15797 RVA: 0x001E2988 File Offset: 0x001E0B88
		public KizunaLvUpItem(GameObject item, GameObject itemIconSet, int itemId)
		{
			this._iconItemCtrl = item.GetComponent<IconItemCtrl>();
			this._expBonus = itemIconSet.transform.Find("Txt_ExpBonus").gameObject.GetComponent<PguiTextCtrl>();
			this._itemNum = itemIconSet.transform.Find("Num_Own").gameObject.GetComponent<PguiTextCtrl>();
			this._colorBase = itemIconSet.transform.Find("ColorBase").GetComponent<PguiImageCtrl>();
			this._imgCount = itemIconSet.transform.Find("Count").gameObject.GetComponent<PguiImageCtrl>();
			this._itemCount = itemIconSet.transform.Find("Count/Num_Count").gameObject.GetComponent<PguiTextCtrl>();
			this._iconItemCtrl.Clear();
			this._expBonus.gameObject.SetActive(false);
			this._itemNum.gameObject.SetActive(false);
			this._colorBase.gameObject.SetActive(false);
			this._imgCount.gameObject.SetActive(false);
			this._itemCount.gameObject.SetActive(false);
			this._itemId = itemId;
		}

		// Token: 0x06003DB6 RID: 15798 RVA: 0x001E2AAC File Offset: 0x001E0CAC
		public void SetUp(int id, ItemStaticBase expAddItem, int num, IconItemCtrl.OnClick touch, IconItemCtrl.OnLongClick longTouch)
		{
			this._iconItemCtrl.gameObject.SetActive(true);
			this._itemId = id;
			this._iconItemCtrl.Setup(expAddItem, -1);
			this._itemNum.gameObject.SetActive(true);
			this._itemCount.gameObject.SetActive(true);
			this._expBonus.gameObject.SetActive(false);
			this._colorBase.gameObject.SetActive(false);
			this._itemNum.text = num.ToString();
			this._iconItemCtrl.SetActEnable(0 < num);
			this._iconItemCtrl.GetComponent<RectTransform>();
			this._iconItemCtrl.AddOnClickListener(touch);
			this._iconItemCtrl.AddOnLongClickListener(longTouch);
		}

		// Token: 0x06003DB7 RID: 15799 RVA: 0x001E2B69 File Offset: 0x001E0D69
		public void SetActiveImageCountGameObject(bool isActive)
		{
			this._imgCount.gameObject.SetActive(isActive);
		}

		// Token: 0x06003DB8 RID: 15800 RVA: 0x001E2B7C File Offset: 0x001E0D7C
		public void SetTextItemCount(string text)
		{
			this._itemCount.text = text;
		}

		// Token: 0x04003F57 RID: 16215
		private int _itemId;

		// Token: 0x04003F58 RID: 16216
		private IconItemCtrl _iconItemCtrl;

		// Token: 0x04003F59 RID: 16217
		private PguiTextCtrl _expBonus;

		// Token: 0x04003F5A RID: 16218
		private PguiTextCtrl _itemNum;

		// Token: 0x04003F5B RID: 16219
		private PguiImageCtrl _imgCount;

		// Token: 0x04003F5C RID: 16220
		private PguiTextCtrl _itemCount;

		// Token: 0x04003F5D RID: 16221
		private PguiImageCtrl _colorBase;
	}

	// Token: 0x020009F2 RID: 2546
	public class KizunaLvUpTab
	{
		// Token: 0x170008F9 RID: 2297
		// (get) Token: 0x06003DB9 RID: 15801 RVA: 0x001E2B8A File Offset: 0x001E0D8A
		public GameObject BaseObj
		{
			get
			{
				return this._baseObj;
			}
		}

		// Token: 0x170008FA RID: 2298
		// (get) Token: 0x06003DBA RID: 15802 RVA: 0x001E2B92 File Offset: 0x001E0D92
		public PguiTextCtrl NumLvLeft
		{
			get
			{
				return this._numLvLeft;
			}
		}

		// Token: 0x170008FB RID: 2299
		// (get) Token: 0x06003DBB RID: 15803 RVA: 0x001E2B9A File Offset: 0x001E0D9A
		public PguiTextCtrl NumLvRight
		{
			get
			{
				return this._numLvRight;
			}
		}

		// Token: 0x170008FC RID: 2300
		// (get) Token: 0x06003DBC RID: 15804 RVA: 0x001E2BA2 File Offset: 0x001E0DA2
		public PguiTextCtrl NumResult
		{
			get
			{
				return this._numResult;
			}
		}

		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x06003DBD RID: 15805 RVA: 0x001E2BAA File Offset: 0x001E0DAA
		public SimpleAnimation ResultLvup
		{
			get
			{
				return this._resultLvup;
			}
		}

		// Token: 0x170008FE RID: 2302
		// (get) Token: 0x06003DBE RID: 15806 RVA: 0x001E2BB2 File Offset: 0x001E0DB2
		public ReuseScroll ScrollView
		{
			get
			{
				return this._scrollView;
			}
		}

		// Token: 0x170008FF RID: 2303
		// (get) Token: 0x06003DBF RID: 15807 RVA: 0x001E2BBA File Offset: 0x001E0DBA
		public List<SelCharaGrowCtrl.CommonGUI.ItemListBar> ItemListBar
		{
			get
			{
				return this._itemListBar;
			}
		}

		// Token: 0x17000900 RID: 2304
		// (get) Token: 0x06003DC0 RID: 15808 RVA: 0x001E2BC2 File Offset: 0x001E0DC2
		public List<SelCharaGrowKizuna.KizunaLvUpItem> IconItemList
		{
			get
			{
				return this._iconItemList;
			}
		}

		// Token: 0x17000901 RID: 2305
		// (get) Token: 0x06003DC1 RID: 15809 RVA: 0x001E2BCA File Offset: 0x001E0DCA
		public PguiImageCtrl Gage
		{
			get
			{
				return this._gage;
			}
		}

		// Token: 0x17000902 RID: 2306
		// (get) Token: 0x06003DC2 RID: 15810 RVA: 0x001E2BD2 File Offset: 0x001E0DD2
		public PguiImageCtrl ImgYaji
		{
			get
			{
				return this._imgYaji;
			}
		}

		// Token: 0x17000903 RID: 2307
		// (get) Token: 0x06003DC3 RID: 15811 RVA: 0x001E2BDA File Offset: 0x001E0DDA
		public AEImage ImageResult
		{
			get
			{
				return this._imageResult;
			}
		}

		// Token: 0x17000904 RID: 2308
		// (get) Token: 0x06003DC4 RID: 15812 RVA: 0x001E2BE2 File Offset: 0x001E0DE2
		public AEImage ImageLevelUP
		{
			get
			{
				return this._imageLevelUP;
			}
		}

		// Token: 0x17000905 RID: 2309
		// (get) Token: 0x06003DC5 RID: 15813 RVA: 0x001E2BEA File Offset: 0x001E0DEA
		public GameObject ExpInfoObject
		{
			get
			{
				return this._expInfoObject;
			}
		}

		// Token: 0x06003DC6 RID: 15814 RVA: 0x001E2BF4 File Offset: 0x001E0DF4
		public KizunaLvUpTab(Transform baseTr)
		{
			this._baseObj = baseTr.gameObject;
			this._expInfoObject = baseTr.Find("ExpInfo").gameObject;
			this._numLvLeft = baseTr.Find("ExpInfo/Num_Lv_L").GetComponent<PguiTextCtrl>();
			this._numLvRight = baseTr.Find("ExpInfo/Num_Lv_R").GetComponent<PguiTextCtrl>();
			this._numResult = baseTr.Find("ExpInfo/Result_Lvup/Num_Result").GetComponent<PguiTextCtrl>();
			this._resultLvup = baseTr.Find("ExpInfo/Result_Lvup").GetComponent<SimpleAnimation>();
			this._resultLvup.gameObject.SetActive(false);
			this._scrollView = baseTr.Find("Base/ScrollView").GetComponent<ReuseScroll>();
			this._itemListBar = new List<SelCharaGrowCtrl.CommonGUI.ItemListBar>();
			this._iconItemList = new List<SelCharaGrowKizuna.KizunaLvUpItem>();
			this._numExpNext = baseTr.Find("ExpInfo/Num_Exp_Next").GetComponent<PguiTextCtrl>();
			this._gageUp = baseTr.Find("ExpInfo/ExpGage/Gage_Up").GetComponent<PguiImageCtrl>();
			this._gageUp.gameObject.SetActive(false);
			this._gage = baseTr.Find("ExpInfo/ExpGage/Gage").GetComponent<PguiImageCtrl>();
			this._imgYaji = baseTr.Find("ExpInfo/Num_Lv_L/Img_Yaji").GetComponent<PguiImageCtrl>();
			this._imageResult = baseTr.Find("ExpInfo/AEImage_result").GetComponent<AEImage>();
			this._imageResult.gameObject.SetActive(false);
			this._imageLevelUP = baseTr.Find("ExpInfo/AEImage_LevelUP").GetComponent<AEImage>();
			this._imageLevelUP.gameObject.SetActive(false);
			if (this._scrollView.RefScrollRect == null)
			{
				this._scrollView.InitForce();
			}
		}

		// Token: 0x06003DC7 RID: 15815 RVA: 0x001E2D94 File Offset: 0x001E0F94
		public void SetAction(Action<int, GameObject> setupAction, Action<int, GameObject> updateAction)
		{
			ReuseScroll scrollView = this._scrollView;
			scrollView.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollView.onStartItem, setupAction);
			ReuseScroll scrollView2 = this._scrollView;
			scrollView2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollView2.onUpdateItem, updateAction);
			this._scrollView.Setup(1, 0);
		}

		// Token: 0x06003DC8 RID: 15816 RVA: 0x001E2DE6 File Offset: 0x001E0FE6
		public void SetActiveGage(bool isActive)
		{
			this._gage.gameObject.SetActive(isActive);
		}

		// Token: 0x06003DC9 RID: 15817 RVA: 0x001E2DF9 File Offset: 0x001E0FF9
		public void SetText(string leftText, string rightText, string expNextText)
		{
			this._numLvLeft.text = leftText;
			this._numLvRight.text = rightText;
			this._numExpNext.text = expNextText;
		}

		// Token: 0x06003DCA RID: 15818 RVA: 0x001E2E1F File Offset: 0x001E101F
		public void SetGageUpImageFillAmount(float fillAmount)
		{
			this._gageUp.m_Image.fillAmount = fillAmount;
		}

		// Token: 0x06003DCB RID: 15819 RVA: 0x001E2E32 File Offset: 0x001E1032
		public void SetGageImageFillAmount(float fillAmount)
		{
			this._gage.m_Image.fillAmount = fillAmount;
		}

		// Token: 0x06003DCC RID: 15820 RVA: 0x001E2E45 File Offset: 0x001E1045
		public void AddGageImageFillAmount(float fillAmount)
		{
			this._gage.m_Image.fillAmount += fillAmount;
		}

		// Token: 0x06003DCD RID: 15821 RVA: 0x001E2E5F File Offset: 0x001E105F
		public void SetAEImageLevelUP()
		{
			this._imageLevelUP.gameObject.SetActive(true);
			this._imageLevelUP.playTime = 0f;
			this._imageLevelUP.autoPlay = true;
			this._imageLevelUP.playLoop = false;
		}

		// Token: 0x06003DCE RID: 15822 RVA: 0x001E2E9C File Offset: 0x001E109C
		public void SetActiveCtrl(bool doDisp, bool effectDisp)
		{
			this._numLvLeft.gameObject.SetActive(doDisp || effectDisp);
			this._numLvRight.gameObject.SetActive(true);
			this._imgYaji.gameObject.SetActive(doDisp || effectDisp);
			this._gageUp.gameObject.SetActive(doDisp);
		}

		// Token: 0x04003F5E RID: 16222
		private GameObject _baseObj;

		// Token: 0x04003F5F RID: 16223
		private PguiTextCtrl _numLvLeft;

		// Token: 0x04003F60 RID: 16224
		private PguiTextCtrl _numLvRight;

		// Token: 0x04003F61 RID: 16225
		private PguiTextCtrl _numResult;

		// Token: 0x04003F62 RID: 16226
		private SimpleAnimation _resultLvup;

		// Token: 0x04003F63 RID: 16227
		private ReuseScroll _scrollView;

		// Token: 0x04003F64 RID: 16228
		private List<SelCharaGrowCtrl.CommonGUI.ItemListBar> _itemListBar;

		// Token: 0x04003F65 RID: 16229
		private List<SelCharaGrowKizuna.KizunaLvUpItem> _iconItemList;

		// Token: 0x04003F66 RID: 16230
		private PguiTextCtrl _numExpNext;

		// Token: 0x04003F67 RID: 16231
		private PguiImageCtrl _gageUp;

		// Token: 0x04003F68 RID: 16232
		private PguiImageCtrl _gage;

		// Token: 0x04003F69 RID: 16233
		private PguiImageCtrl _imgYaji;

		// Token: 0x04003F6A RID: 16234
		private AEImage _imageResult;

		// Token: 0x04003F6B RID: 16235
		private AEImage _imageLevelUP;

		// Token: 0x04003F6C RID: 16236
		private GameObject _expInfoObject;
	}

	// Token: 0x020009F3 RID: 2547
	public class WindowKizunaLvUp
	{
		// Token: 0x17000906 RID: 2310
		// (get) Token: 0x06003DCF RID: 15823 RVA: 0x001E2EF1 File Offset: 0x001E10F1
		public PguiOpenWindowCtrl OpenWindowCtrl
		{
			get
			{
				return this._openWindowCtrl;
			}
		}

		// Token: 0x17000907 RID: 2311
		// (get) Token: 0x06003DD0 RID: 15824 RVA: 0x001E2EF9 File Offset: 0x001E10F9
		public PguiTextCtrl NumCoinUse
		{
			get
			{
				return this._numCoinUse;
			}
		}

		// Token: 0x17000908 RID: 2312
		// (get) Token: 0x06003DD1 RID: 15825 RVA: 0x001E2F01 File Offset: 0x001E1101
		public PguiTextCtrl NumCoinOwn
		{
			get
			{
				return this._numCoinOwn;
			}
		}

		// Token: 0x17000909 RID: 2313
		// (get) Token: 0x06003DD2 RID: 15826 RVA: 0x001E2F09 File Offset: 0x001E1109
		public List<ItemInput> ItemList
		{
			get
			{
				return this._itemList;
			}
		}

		// Token: 0x1700090A RID: 2314
		// (get) Token: 0x06003DD3 RID: 15827 RVA: 0x001E2F11 File Offset: 0x001E1111
		public IconCharaCtrl IconChara
		{
			get
			{
				return this._iconChara;
			}
		}

		// Token: 0x1700090B RID: 2315
		// (get) Token: 0x06003DD4 RID: 15828 RVA: 0x001E2F19 File Offset: 0x001E1119
		public PguiTextCtrl TxtCharaName
		{
			get
			{
				return this._txtCharaName;
			}
		}

		// Token: 0x1700090C RID: 2316
		// (get) Token: 0x06003DD5 RID: 15829 RVA: 0x001E2F21 File Offset: 0x001E1121
		public ReuseScroll ScrollView
		{
			get
			{
				return this._scrollView;
			}
		}

		// Token: 0x06003DD6 RID: 15830 RVA: 0x001E2F2C File Offset: 0x001E112C
		public WindowKizunaLvUp(Transform baseTr)
		{
			this._openWindowCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this._numCoinUse = baseTr.Find("Base/Window/ItemUse/Num").GetComponent<PguiTextCtrl>();
			this._numCoinOwn = baseTr.Find("Base/Window/ItemOwn/Num").GetComponent<PguiTextCtrl>();
			GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("CmnIconFrame/GUI/Prefab/Icon_Chara"), baseTr.Find("Base/Window/Icon_Chara"));
			this._iconChara = gameObject.GetComponent<IconCharaCtrl>();
			this._txtCharaName = baseTr.Find("Base/Window/Txt_CharaName").GetComponent<PguiTextCtrl>();
			this._numLvBefore = baseTr.Find("Base/Window/ExpInfo/Num_Lv_Before").GetComponent<PguiTextCtrl>();
			this._numLvAfter = baseTr.Find("Base/Window/ExpInfo/Num_Lv_After").GetComponent<PguiTextCtrl>();
			this._gageUp = baseTr.Find("Base/Window/ExpInfo/ExpGage/Gage_Up").GetComponent<PguiImageCtrl>();
			this._gage = baseTr.Find("Base/Window/ExpInfo/ExpGage/Gage").GetComponent<PguiImageCtrl>();
			this._scrollView = baseTr.Find("Base/Window/ItemUseInfo/ScrollView").GetComponent<ReuseScroll>();
			this._numExpNext = baseTr.Find("Base/Window/ExpInfo/Num_Exp_Next").GetComponent<PguiTextCtrl>();
		}

		// Token: 0x06003DD7 RID: 15831 RVA: 0x001E3040 File Offset: 0x001E1240
		public void SetAction(Action<int, GameObject> setupAction, Action<int, GameObject> updateAction)
		{
			this._scrollView.InitForce();
			ReuseScroll scrollView = this._scrollView;
			scrollView.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollView.onStartItem, setupAction);
			ReuseScroll scrollView2 = this._scrollView;
			scrollView2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollView2.onUpdateItem, updateAction);
			this._scrollView.Setup(1, 0);
		}

		// Token: 0x06003DD8 RID: 15832 RVA: 0x001E309D File Offset: 0x001E129D
		public void SetActiveGage(bool isActive)
		{
			this._gage.gameObject.SetActive(isActive);
		}

		// Token: 0x06003DD9 RID: 15833 RVA: 0x001E30B0 File Offset: 0x001E12B0
		public void SetText(string beforeText, string afterText, string expNextText)
		{
			this._numLvBefore.text = beforeText;
			this._numLvAfter.text = afterText;
			this._numExpNext.text = expNextText;
		}

		// Token: 0x06003DDA RID: 15834 RVA: 0x001E30D6 File Offset: 0x001E12D6
		public void SetGageUpImageFillAmount(float fillAmount)
		{
			this._gageUp.m_Image.fillAmount = fillAmount;
		}

		// Token: 0x06003DDB RID: 15835 RVA: 0x001E30E9 File Offset: 0x001E12E9
		public void SetGageImageFillAmount(float fillAmount)
		{
			this._gage.m_Image.fillAmount = fillAmount;
		}

		// Token: 0x06003DDC RID: 15836 RVA: 0x001E30FC File Offset: 0x001E12FC
		public void SetItemImputList(List<ItemInput> itemList)
		{
			this._itemList = itemList;
		}

		// Token: 0x04003F6D RID: 16237
		private PguiTextCtrl _numLvBefore;

		// Token: 0x04003F6E RID: 16238
		private PguiTextCtrl _numLvAfter;

		// Token: 0x04003F6F RID: 16239
		private PguiImageCtrl _gageUp;

		// Token: 0x04003F70 RID: 16240
		private PguiImageCtrl _gage;

		// Token: 0x04003F71 RID: 16241
		private PguiTextCtrl _numExpNext;

		// Token: 0x04003F72 RID: 16242
		private PguiOpenWindowCtrl _openWindowCtrl;

		// Token: 0x04003F73 RID: 16243
		private PguiTextCtrl _numCoinUse;

		// Token: 0x04003F74 RID: 16244
		private PguiTextCtrl _numCoinOwn;

		// Token: 0x04003F75 RID: 16245
		private List<ItemInput> _itemList;

		// Token: 0x04003F76 RID: 16246
		private IconCharaCtrl _iconChara;

		// Token: 0x04003F77 RID: 16247
		private PguiTextCtrl _txtCharaName;

		// Token: 0x04003F78 RID: 16248
		private ReuseScroll _scrollView;

		// Token: 0x04003F79 RID: 16249
		public const int SCROLL_ITEM_NUN_H = 5;
	}

	// Token: 0x020009F4 RID: 2548
	public class KizunaLvupAuth
	{
		// Token: 0x1700090D RID: 2317
		// (get) Token: 0x06003DDD RID: 15837 RVA: 0x001E3105 File Offset: 0x001E1305
		public GameObject BaseObj
		{
			get
			{
				return this._baseObj;
			}
		}

		// Token: 0x06003DDE RID: 15838 RVA: 0x001E3110 File Offset: 0x001E1310
		public KizunaLvupAuth(Transform baseTr)
		{
			this._baseObj = baseTr.gameObject;
			this._imageAList = new List<PguiReplaceAECtrl>();
			this._imageBList = new List<PguiReplaceAECtrl>();
			this._imageCList = new List<PguiReplaceAECtrl>();
			this._imageAList.Add(baseTr.Find("Null_A_01_R/AEImage_Japaman").GetComponent<PguiReplaceAECtrl>());
			this._imageAList.Add(baseTr.Find("Null_A_02_R/AEImage_Japaman").GetComponent<PguiReplaceAECtrl>());
			this._imageAList.Add(baseTr.Find("Null_A_03_L/AEImage_Japaman").GetComponent<PguiReplaceAECtrl>());
			this._imageAList.Add(baseTr.Find("Null_A_04_R/AEImage_Japaman").GetComponent<PguiReplaceAECtrl>());
			this._imageAList.Add(baseTr.Find("Null_A_05_R/AEImage_Japaman").GetComponent<PguiReplaceAECtrl>());
			this._imageAList.Add(baseTr.Find("Null_A_06_L/AEImage_Japaman").GetComponent<PguiReplaceAECtrl>());
			this._imageBList.Add(baseTr.Find("Null_B_01_L/AEImage_Japaman").GetComponent<PguiReplaceAECtrl>());
			this._imageBList.Add(baseTr.Find("Null_B_02_L/AEImage_Japaman").GetComponent<PguiReplaceAECtrl>());
			this._imageBList.Add(baseTr.Find("Null_B_03_L/AEImage_Japaman").GetComponent<PguiReplaceAECtrl>());
			this._imageBList.Add(baseTr.Find("Null_B_04_R/AEImage_Japaman").GetComponent<PguiReplaceAECtrl>());
			this._imageCList.Add(baseTr.Find("Null_C_01_R/AEImage_Japaman").GetComponent<PguiReplaceAECtrl>());
			this._imageCList.Add(baseTr.Find("Null_C_02_L/AEImage_Japaman").GetComponent<PguiReplaceAECtrl>());
			this._imageCList.Add(baseTr.Find("Null_C_03_R/AEImage_Japaman").GetComponent<PguiReplaceAECtrl>());
			foreach (PguiReplaceAECtrl pguiReplaceAECtrl in this._imageAList)
			{
				pguiReplaceAECtrl.InitForce();
			}
			foreach (PguiReplaceAECtrl pguiReplaceAECtrl2 in this._imageBList)
			{
				pguiReplaceAECtrl2.InitForce();
			}
			foreach (PguiReplaceAECtrl pguiReplaceAECtrl3 in this._imageCList)
			{
				pguiReplaceAECtrl3.InitForce();
			}
		}

		// Token: 0x04003F7A RID: 16250
		private List<PguiReplaceAECtrl> _imageAList;

		// Token: 0x04003F7B RID: 16251
		private List<PguiReplaceAECtrl> _imageBList;

		// Token: 0x04003F7C RID: 16252
		private List<PguiReplaceAECtrl> _imageCList;

		// Token: 0x04003F7D RID: 16253
		private GameObject _baseObj;
	}

	// Token: 0x020009F5 RID: 2549
	public class WindowKizunaLimit
	{
		// Token: 0x1700090E RID: 2318
		// (get) Token: 0x06003DDF RID: 15839 RVA: 0x001E3370 File Offset: 0x001E1570
		public PguiOpenWindowCtrl OpenWindowCtrl
		{
			get
			{
				return this._openWindowCtrl;
			}
		}

		// Token: 0x1700090F RID: 2319
		// (get) Token: 0x06003DE0 RID: 15840 RVA: 0x001E3378 File Offset: 0x001E1578
		public PguiTextCtrl BeforeLevelText
		{
			get
			{
				return this._beforeLevelText;
			}
		}

		// Token: 0x17000910 RID: 2320
		// (get) Token: 0x06003DE1 RID: 15841 RVA: 0x001E3380 File Offset: 0x001E1580
		public PguiTextCtrl AfterLevelText
		{
			get
			{
				return this._afterLevelText;
			}
		}

		// Token: 0x17000911 RID: 2321
		// (get) Token: 0x06003DE2 RID: 15842 RVA: 0x001E3388 File Offset: 0x001E1588
		public PguiTextCtrl ItemNameText
		{
			get
			{
				return this._itemNameText;
			}
		}

		// Token: 0x17000912 RID: 2322
		// (get) Token: 0x06003DE3 RID: 15843 RVA: 0x001E3390 File Offset: 0x001E1590
		public PguiTextCtrl ItemBeforeNumText
		{
			get
			{
				return this._itemBeforeNumText;
			}
		}

		// Token: 0x17000913 RID: 2323
		// (get) Token: 0x06003DE4 RID: 15844 RVA: 0x001E3398 File Offset: 0x001E1598
		public PguiTextCtrl ItemAfterNumText
		{
			get
			{
				return this._itemAfterNumText;
			}
		}

		// Token: 0x17000914 RID: 2324
		// (get) Token: 0x06003DE5 RID: 15845 RVA: 0x001E33A0 File Offset: 0x001E15A0
		public PguiRawImageCtrl IconTex
		{
			get
			{
				return this._iconTex;
			}
		}

		// Token: 0x06003DE6 RID: 15846 RVA: 0x001E33A8 File Offset: 0x001E15A8
		public WindowKizunaLimit(Transform baseTr)
		{
			this._openWindowCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this._beforeLevelText = baseTr.Find("Base/Window/Txt/Num_Lv_Before").GetComponent<PguiTextCtrl>();
			this._afterLevelText = baseTr.Find("Base/Window/Txt/Num_Lv_After").GetComponent<PguiTextCtrl>();
			this._itemNameText = baseTr.Find("Base/Window/ItemInfo/Txt01").GetComponent<PguiTextCtrl>();
			this._itemBeforeNumText = baseTr.Find("Base/Window/ItemInfo/Num_BeforeTxt").GetComponent<PguiTextCtrl>();
			this._itemAfterNumText = baseTr.Find("Base/Window/ItemInfo/Num_AfterTxt").GetComponent<PguiTextCtrl>();
			this._iconTex = baseTr.Find("Base/Window/ItemInfo/Icon_Tex").GetComponent<PguiRawImageCtrl>();
		}

		// Token: 0x04003F7E RID: 16254
		private PguiOpenWindowCtrl _openWindowCtrl;

		// Token: 0x04003F7F RID: 16255
		private PguiTextCtrl _beforeLevelText;

		// Token: 0x04003F80 RID: 16256
		private PguiTextCtrl _afterLevelText;

		// Token: 0x04003F81 RID: 16257
		private PguiTextCtrl _itemNameText;

		// Token: 0x04003F82 RID: 16258
		private PguiTextCtrl _itemBeforeNumText;

		// Token: 0x04003F83 RID: 16259
		private PguiTextCtrl _itemAfterNumText;

		// Token: 0x04003F84 RID: 16260
		private PguiRawImageCtrl _iconTex;
	}

	// Token: 0x020009F6 RID: 2550
	public class KizunaLevelUpEffectWindow
	{
		// Token: 0x06003DE7 RID: 15847 RVA: 0x001E344C File Offset: 0x001E164C
		public KizunaLevelUpEffectWindow(Transform baseTransform)
		{
			this._touch = baseTransform.Find("TouchCollision");
			this._windowPanel = baseTransform.gameObject;
			SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, baseTransform, true);
			PguiPanel component = this._windowPanel.GetComponent<PguiPanel>();
			if (component != null)
			{
				component.raycastTarget = false;
			}
			this._kizunaWindow = baseTransform.Find("Auth_HeartLvUp").gameObject;
			this._kizunaWindow.SetActive(false);
			this._kizunaWinWhite = this._kizunaWindow.transform.Find("AEImage_White").GetComponent<PguiAECtrl>();
			this._kizunaWinBack = this._kizunaWindow.transform.Find("AEImage_Back").GetComponent<PguiAECtrl>();
			this._kizunaWinFront = this._kizunaWindow.transform.Find("AEImage_Front").GetComponent<PguiAECtrl>();
			this._kizunaWinInfo = this._kizunaWindow.transform.Find("AEImage_Info").GetComponent<PguiAECtrl>();
			this._kizunaWinId = 0;
			this._kizunaWinCloth = 0;
			this._kizunaWinLongSkirt = false;
			this._kizunaWinChara = null;
			this._kizunaWinCharaVoice = false;
			this._kizunaWinTime = 0f;
			this._kizunaWinChrY = 0f;
			this._kizunaWinItem = new List<ItemData>();
			this._isTouch = false;
			this._touch.gameObject.AddComponent<PguiTouchTrigger>().AddListener(delegate
			{
				this._isTouch = true;
			}, null, null, null, null);
		}

		// Token: 0x06003DE8 RID: 15848 RVA: 0x001E35E5 File Offset: 0x001E17E5
		public void SetCurrentCharaPackData(CharaPackData charaPackData)
		{
			this._currentCharaPackData = charaPackData;
		}

		// Token: 0x06003DE9 RID: 15849 RVA: 0x001E35EE File Offset: 0x001E17EE
		public bool CheckKizunaWinCharaIsActive()
		{
			return this._kizunaWinChara != null && this._kizunaWinChara.gameObject.activeSelf;
		}

		// Token: 0x06003DEA RID: 15850 RVA: 0x001E3613 File Offset: 0x001E1813
		public bool CheckTouchIsActive()
		{
			return this._touch.gameObject.activeSelf;
		}

		// Token: 0x06003DEB RID: 15851 RVA: 0x001E3628 File Offset: 0x001E1828
		public void UpdateKizunaLevelUpEffect(RenderTextureChara renderTextureChara)
		{
			bool flag = this._kizunaWindow != null && this._kizunaWindow.activeSelf;
			if (flag && this._kizunaWinWhite.GetAnimeType() == PguiAECtrl.AmimeType.END)
			{
				if (this._kizunaWinWhite.IsPlaying())
				{
					if (this._kizunaWinChara.gameObject.activeSelf)
					{
						renderTextureChara.gameObject.SetActive(false);
						if (!this._kizunaWinBack.IsPlaying())
						{
							this._kizunaWinChara.gameObject.SetActive(false);
						}
					}
				}
				else
				{
					this._kizunaWindow.SetActive(false);
					this.Reset();
				}
			}
			if (flag && this._isTouch)
			{
				this.SkipKizunaUp();
				SoundManager.Play("prd_se_click", false, false);
				return;
			}
			if (flag)
			{
				this.UpdateKizunaUp();
			}
		}

		// Token: 0x06003DEC RID: 15852 RVA: 0x001E36E8 File Offset: 0x001E18E8
		public void StartKizunaUp(int beforeKizunaLevel, int afterKizunaLevel)
		{
			if (this._currentCharaPackData == null)
			{
				return;
			}
			this._kizunaWinChara = AssetManager.InstantiateAssetData("RenderTextureChara/Prefab/RenderTextureCharaCtrl", this._kizunaWindow.transform.Find("RenderTexture")).GetComponent<RenderTextureChara>();
			this._kizunaWinChara.SetupRenderTexture(this.RENDER_TEXTURE_WIDTH, this.RENDER_TEXTURE_HEIGHT);
			this._kizunaWinChara.gameObject.SetActive(false);
			this._kizunaWinId = this._currentCharaPackData.id;
			this._kizunaWinCloth = this._currentCharaPackData.equipClothImageId;
			this._kizunaWinLongSkirt = this._kizunaWinCloth > 0 && this._currentCharaPackData.equipLongSkirt;
			this._kizunaWinInfo.transform.Find("Lv_Info01/Txt").GetComponent<PguiTextCtrl>().text = "Lv.<size=60>" + beforeKizunaLevel.ToString() + "</size>";
			this._kizunaWinInfo.transform.Find("Lv_Info02/Txt").GetComponent<PguiTextCtrl>().text = "Lv.<size=60><color=#fb556b>" + afterKizunaLevel.ToString() + "</color></size>";
			this._kizunaWinInfo.transform.Find("Serif_Info03/Txt").GetComponent<PguiTextCtrl>().text = this._currentCharaPackData.staticData.baseData.kizunaupText;
			bool flag = false;
			bool flag2 = false;
			List<string> list = new List<string>();
			this._kizunaWinItem = new List<ItemData>();
			this._afterItemIdToSourceItemId = new Dictionary<int, int>();
			DataManagerChara.KiznaRewardData kiznaRewardData = DataManager.DmChara.GetKizunaRewardData(beforeKizunaLevel, this._currentCharaPackData.id);
			int num = ((kiznaRewardData == null) ? 0 : kiznaRewardData.artsMax);
			List<int> latestAcquiredAchievementIdList = DataManager.DmAchievement.GetLatestAcquiredAchievementIdList();
			for (int i = beforeKizunaLevel; i < afterKizunaLevel; i++)
			{
				kiznaRewardData = DataManager.DmChara.GetKizunaRewardData(i + 1, this._currentCharaPackData.id);
				if (kiznaRewardData != null)
				{
					if (!flag && kiznaRewardData.artsMax > num)
					{
						flag = true;
						list.Add(this.KEMONO_MIRACLE_LEVEL_LIMIT_RELEASE_TEXT);
					}
					if (!flag2 && kiznaRewardData.charaquest > 0)
					{
						flag2 = true;
						list.Add(this.CHARA_STORY_RELEASE_TEXT);
					}
					int num2 = kiznaRewardData.itemId;
					int num3 = kiznaRewardData.itemNum;
					DataManagerAchievement.AchievementStaticData achievementData = DataManager.DmAchievement.GetAchievementData(num2);
					DataManagerAchievement.AchievementData haveAchievementData = DataManager.DmAchievement.GetHaveAchievementData(num2);
					if (achievementData != null && haveAchievementData != null && !latestAcquiredAchievementIdList.Contains(num2))
					{
						num2 = achievementData.duplicateItemId;
						num3 = achievementData.duplicateItemNum;
						this._afterItemIdToSourceItemId.Add(num2, kiznaRewardData.itemId);
					}
					else if (latestAcquiredAchievementIdList.Contains(num2))
					{
						latestAcquiredAchievementIdList.Remove(num2);
					}
					if (kiznaRewardData.itemId != 0 && num2 != 0 && kiznaRewardData.itemNum != 0 && num3 != 0)
					{
						this._kizunaWinItem.Add(new ItemData(num2, num3));
					}
				}
			}
			string text = "";
			int num4 = 0;
			while (num4 < list.Count && num4 <= this.MAX_STRING_COUNT)
			{
				if (num4 > 0)
				{
					text += "\n";
				}
				text += list[num4];
				num4++;
			}
			this._kizunaWinInfo.transform.Find("Item_Info04/Txt").GetComponent<PguiTextCtrl>().text = text;
			this._kizunaWindow.SetActive(true);
			this._kizunaWinWhite.PlayAnimation(PguiAECtrl.AmimeType.START, null);
			this._kizunaWinBack.PlayAnimation(PguiAECtrl.AmimeType.START, null);
			if (this._kizunaWindow.activeSelf)
			{
				this._kizunaWinWhite.m_AEImage.playTime = (this._kizunaWinBack.m_AEImage.playTime = 0.5f);
			}
			this._kizunaWinFront.gameObject.SetActive(false);
			this._kizunaWinInfo.gameObject.SetActive(false);
			this._kizunaWinChara.gameObject.SetActive(false);
			this._kizunaWinChara.StopVoice();
			this._kizunaWinChara.Setup(0, 0, CharaMotionDefine.ActKey.INVALID, 0, false, true, null, false, null, 0f, null, false, false, false);
			SoundManager.Play("prd_se_result_bond_levelup_window", false, false);
			this._touch.gameObject.SetActive(true);
			this._touch.SetAsLastSibling();
		}

		// Token: 0x06003DED RID: 15853 RVA: 0x001E3ADC File Offset: 0x001E1CDC
		private void UpdateKizunaUp()
		{
			if (this._kizunaWinWhite == null)
			{
				return;
			}
			if (this._kizunaWinWhite.GetAnimeType() == PguiAECtrl.AmimeType.START && !this._kizunaWinWhite.IsPlaying())
			{
				this._kizunaWinWhite.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
			}
			if (this._kizunaWinBack.GetAnimeType() == PguiAECtrl.AmimeType.START)
			{
				if (!this._kizunaWinBack.IsPlaying())
				{
					this._kizunaWinBack.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
					this._kizunaWinCharaVoice = false;
					this._kizunaWinChara.gameObject.SetActive(true);
					this._kizunaWinChara.Setup(this._kizunaWinId, 0, CharaMotionDefine.ActKey.GACHA_ST, this._kizunaWinCloth, this._kizunaWinLongSkirt, false, new RenderTextureChara.FinishCallback(this.CallbackKizunaUpChara), false, null, 1.8333334f, delegate
					{
						this._kizunaWinCharaVoice = true;
					}, false, false, false);
					this._kizunaWinChara.SetCameraPosition(new Vector3(0f, 1.07f, 5.4f));
					this._kizunaWinTime = 0f;
					this._kizunaWinChrY = 1.225f;
					return;
				}
			}
			else if (this._kizunaWinInfo.gameObject.activeSelf)
			{
				if (this._kizunaWinChara == null)
				{
					return;
				}
				if (this._kizunaWinChara.IsCurrentAnimation(CharaMotionDefine.ActKey.GACHA_ST))
				{
					this._kizunaWinChrY = this._kizunaWinChara.GetNodePos("j_neck").y + 0.005f;
				}
				this._kizunaWinTime = Mathf.Clamp01(this._kizunaWinTime + TimeManager.DeltaTime * 3f);
				this._kizunaWinChara.SetCameraPosition(Vector3.Lerp(new Vector3(0f, 1.07f, 5.4f), new Vector3(0f, this._kizunaWinChrY, 3.7f), this._kizunaWinTime));
				if (this._kizunaWinCharaVoice)
				{
					this._kizunaWinChara.PlayVoice(VOICE_TYPE.KUP01);
					this._kizunaWinCharaVoice = false;
					return;
				}
			}
			else
			{
				float num = this._kizunaWinChara.AnimationLength();
				if (num > 0f)
				{
					float num2 = this._kizunaWinChara.AnimationTime();
					if ((1f - num2) * num < 1f)
					{
						this.SkipKizunaUp();
					}
				}
			}
		}

		// Token: 0x06003DEE RID: 15854 RVA: 0x001E3CE0 File Offset: 0x001E1EE0
		private void SkipKizunaUp()
		{
			this._isTouch = false;
			if (!this._kizunaWinInfo.gameObject.activeSelf)
			{
				this._kizunaWinFront.gameObject.SetActive(true);
				this._kizunaWinFront.PlayAnimation(PguiAECtrl.AmimeType.START, null);
				this._kizunaWinInfo.gameObject.SetActive(true);
				this._kizunaWinInfo.PlayAnimation(PguiAECtrl.AmimeType.START, null);
				return;
			}
			if (this._kizunaWinInfo.GetAnimeType() == PguiAECtrl.AmimeType.START)
			{
				if (this._kizunaWinInfo.IsPlaying())
				{
					this._kizunaWinInfo.ForceEnd();
					return;
				}
				this._kizunaWinInfo.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
				return;
			}
			else
			{
				if (this._kizunaWinInfo.GetAnimeType() == PguiAECtrl.AmimeType.END)
				{
					return;
				}
				if (this._kizunaWinItem.Count > 0)
				{
					CanvasManager.HdlGetItemWindowCtrl.Setup(this._kizunaWinItem, new GetItemWindowCtrl.SetupParam
					{
						strItemCb = delegate(GetItemWindowCtrl.WordingCallbackParam param)
						{
							string text = string.Empty;
							int id = param.itemStaticBase.GetId();
							if (this._afterItemIdToSourceItemId.ContainsKey(id))
							{
								DataManagerAchievement.AchievementStaticData achievementData = DataManager.DmAchievement.GetAchievementData(this._afterItemIdToSourceItemId[id]);
								text = string.Format("{0}はすでに所持していたため\n{1}×{2}に変換されました", achievementData.GetName(), param.itemStaticBase.GetName(), achievementData.duplicateItemNum);
							}
							else
							{
								text = PrjUtil.MakeMessage(param.itemStaticBase.GetName() + "を獲得しました");
							}
							return text;
						}
					});
					CanvasManager.HdlGetItemWindowCtrl.Open();
					this._kizunaWinItem = new List<ItemData>();
					return;
				}
				if (this._kizunaWinInfo.IsPlaying())
				{
					this._kizunaWinInfo.ForceEnd();
					return;
				}
				this._kizunaWinWhite.PlayAnimation(PguiAECtrl.AmimeType.END, null);
				this._kizunaWinBack.PlayAnimation(PguiAECtrl.AmimeType.END, null);
				this._kizunaWinFront.PlayAnimation(PguiAECtrl.AmimeType.END, null);
				this._kizunaWinInfo.PlayAnimation(PguiAECtrl.AmimeType.END, null);
				return;
			}
		}

		// Token: 0x06003DEF RID: 15855 RVA: 0x001E3E24 File Offset: 0x001E2024
		private void CallbackKizunaUpChara()
		{
			this._kizunaWinChrY = this._kizunaWinChara.GetNodePos("j_neck").y + 0.005f;
			this._kizunaWinChara.SetAnimation(CharaMotionDefine.ActKey.GACHA_LP, true);
			if (!this._kizunaWinInfo.gameObject.activeSelf)
			{
				this.SkipKizunaUp();
			}
		}

		// Token: 0x06003DF0 RID: 15856 RVA: 0x001E3E78 File Offset: 0x001E2078
		public void Reset()
		{
			if (this._kizunaWinChara != null)
			{
				this._kizunaWinChara.gameObject.SetActive(false);
				Object.Destroy(this._kizunaWinChara.gameObject);
			}
			this._kizunaWinChara = null;
			this._touch.gameObject.SetActive(false);
		}

		// Token: 0x04003F85 RID: 16261
		private readonly int RENDER_TEXTURE_WIDTH = 1654;

		// Token: 0x04003F86 RID: 16262
		private readonly int RENDER_TEXTURE_HEIGHT = 1024;

		// Token: 0x04003F87 RID: 16263
		private readonly int MAX_STRING_COUNT = 2;

		// Token: 0x04003F88 RID: 16264
		private readonly string KEMONO_MIRACLE_LEVEL_LIMIT_RELEASE_TEXT = "けものミラクルレベルの上限が開放されました";

		// Token: 0x04003F89 RID: 16265
		private readonly string CHARA_STORY_RELEASE_TEXT = "新たなキャラストーリーが開放されました";

		// Token: 0x04003F8A RID: 16266
		private GameObject _windowPanel;

		// Token: 0x04003F8B RID: 16267
		private GameObject _kizunaWindow;

		// Token: 0x04003F8C RID: 16268
		private PguiAECtrl _kizunaWinWhite;

		// Token: 0x04003F8D RID: 16269
		private PguiAECtrl _kizunaWinBack;

		// Token: 0x04003F8E RID: 16270
		private PguiAECtrl _kizunaWinFront;

		// Token: 0x04003F8F RID: 16271
		private PguiAECtrl _kizunaWinInfo;

		// Token: 0x04003F90 RID: 16272
		private int _kizunaWinId;

		// Token: 0x04003F91 RID: 16273
		private int _kizunaWinCloth;

		// Token: 0x04003F92 RID: 16274
		private bool _kizunaWinLongSkirt;

		// Token: 0x04003F93 RID: 16275
		private RenderTextureChara _kizunaWinChara;

		// Token: 0x04003F94 RID: 16276
		private bool _kizunaWinCharaVoice;

		// Token: 0x04003F95 RID: 16277
		private float _kizunaWinTime;

		// Token: 0x04003F96 RID: 16278
		private float _kizunaWinChrY;

		// Token: 0x04003F97 RID: 16279
		private List<ItemData> _kizunaWinItem;

		// Token: 0x04003F98 RID: 16280
		private Dictionary<int, int> _afterItemIdToSourceItemId;

		// Token: 0x04003F99 RID: 16281
		private Transform _touch;

		// Token: 0x04003F9A RID: 16282
		private bool _isTouch;

		// Token: 0x04003F9B RID: 16283
		private CharaPackData _currentCharaPackData;
	}

	// Token: 0x020009F7 RID: 2551
	public class CharaGrowKizunaGUI
	{
		// Token: 0x17000915 RID: 2325
		// (get) Token: 0x06003DF4 RID: 15860 RVA: 0x001E3F69 File Offset: 0x001E2169
		public SelCharaGrowKizuna.WindowKizunaLimit KizunaWindow
		{
			get
			{
				return this._kizunaWindow;
			}
		}

		// Token: 0x17000916 RID: 2326
		// (get) Token: 0x06003DF5 RID: 15861 RVA: 0x001E3F71 File Offset: 0x001E2171
		public SelCharaGrowKizuna.KizunaTab KizunaTab
		{
			get
			{
				return this._kizunaTab;
			}
		}

		// Token: 0x17000917 RID: 2327
		// (get) Token: 0x06003DF6 RID: 15862 RVA: 0x001E3F79 File Offset: 0x001E2179
		public SelCharaGrowKizuna.KizunaLvUpTab KizunaLvUpTab
		{
			get
			{
				return this._kizunaLvUpTab;
			}
		}

		// Token: 0x17000918 RID: 2328
		// (get) Token: 0x06003DF7 RID: 15863 RVA: 0x001E3F81 File Offset: 0x001E2181
		public SelCharaGrowKizuna.WindowKizunaLvUp KizunaLvUpWindow
		{
			get
			{
				return this._kizunaLvUpWindow;
			}
		}

		// Token: 0x17000919 RID: 2329
		// (get) Token: 0x06003DF8 RID: 15864 RVA: 0x001E3F89 File Offset: 0x001E2189
		public SelCharaGrowKizuna.KizunaLevelUpEffectWindow KizunaLevelUpEffectWindow
		{
			get
			{
				return this._kizunaLevelUpEffectWindow;
			}
		}

		// Token: 0x06003DF9 RID: 15865 RVA: 0x001E3F91 File Offset: 0x001E2191
		public void SetActiveCtrl(bool doDisp, bool effectDisp)
		{
			this.KizunaLvUpTab.SetActiveCtrl(doDisp, effectDisp);
		}

		// Token: 0x06003DFA RID: 15866 RVA: 0x001E3FA0 File Offset: 0x001E21A0
		public void Setup(Transform baseTr)
		{
			GameObject gameObject = (GameObject)Resources.Load("SceneCharaEdit/GUI/Prefab/GUI_CharaEdit_Window_HeartLimitOpen");
			this._kizunaWindow = new SelCharaGrowKizuna.WindowKizunaLimit(Object.Instantiate<Transform>(gameObject.transform.Find("Window_HeartLimitOpen"), baseTr).transform);
			this._kizunaTab = new SelCharaGrowKizuna.KizunaTab(baseTr.Find("CharaGrow_Main").Find("Main/Right/HeartLimitOpen"));
			this._kizunaLvUpTab = new SelCharaGrowKizuna.KizunaLvUpTab(baseTr.Find("CharaGrow_Main").Find("Main/Right/KizunaLevelUp"));
			this._kizunaLvUpWindow = new SelCharaGrowKizuna.WindowKizunaLvUp(Object.Instantiate<Transform>(gameObject.transform.Find("Window_LvUp"), baseTr).transform);
			this._kizunaLevelUpAuth = new SelCharaGrowKizuna.KizunaLvupAuth(Object.Instantiate<Transform>(gameObject.transform.Find("Auth_JapamanFeed"), baseTr).transform);
			this._kizunaLevelUpEffectWindow = new SelCharaGrowKizuna.KizunaLevelUpEffectWindow(Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneBattleResult/GUI/Prefab/GUI_BattleResult_Window")).transform);
			this._kizunaLevelUpAuth.BaseObj.SetActive(false);
		}

		// Token: 0x04003F9C RID: 16284
		private SelCharaGrowKizuna.KizunaLvupAuth _kizunaLevelUpAuth;

		// Token: 0x04003F9D RID: 16285
		private SelCharaGrowKizuna.WindowKizunaLimit _kizunaWindow;

		// Token: 0x04003F9E RID: 16286
		private SelCharaGrowKizuna.KizunaTab _kizunaTab;

		// Token: 0x04003F9F RID: 16287
		private SelCharaGrowKizuna.KizunaLvUpTab _kizunaLvUpTab;

		// Token: 0x04003FA0 RID: 16288
		private SelCharaGrowKizuna.WindowKizunaLvUp _kizunaLvUpWindow;

		// Token: 0x04003FA1 RID: 16289
		private SelCharaGrowKizuna.KizunaLevelUpEffectWindow _kizunaLevelUpEffectWindow;
	}

	// Token: 0x020009F8 RID: 2552
	public class LvupAuth
	{
		// Token: 0x1700091A RID: 2330
		// (get) Token: 0x06003DFC RID: 15868 RVA: 0x001E40AC File Offset: 0x001E22AC
		public GameObject BaseObj
		{
			get
			{
				return this._baseObj;
			}
		}

		// Token: 0x1700091B RID: 2331
		// (get) Token: 0x06003DFD RID: 15869 RVA: 0x001E40B4 File Offset: 0x001E22B4
		public PguiAECtrl ImageJapamanFeed
		{
			get
			{
				return this._imageJapamanFeed;
			}
		}

		// Token: 0x1700091C RID: 2332
		// (get) Token: 0x06003DFE RID: 15870 RVA: 0x001E40BC File Offset: 0x001E22BC
		public List<PguiReplaceAECtrl> ImageAList
		{
			get
			{
				return this._imageAList;
			}
		}

		// Token: 0x1700091D RID: 2333
		// (get) Token: 0x06003DFF RID: 15871 RVA: 0x001E40C4 File Offset: 0x001E22C4
		public List<PguiReplaceAECtrl> ImageBList
		{
			get
			{
				return this._imageBList;
			}
		}

		// Token: 0x1700091E RID: 2334
		// (get) Token: 0x06003E00 RID: 15872 RVA: 0x001E40CC File Offset: 0x001E22CC
		public List<PguiReplaceAECtrl> ImageCList
		{
			get
			{
				return this._imageCList;
			}
		}

		// Token: 0x06003E01 RID: 15873 RVA: 0x001E40D4 File Offset: 0x001E22D4
		public LvupAuth(Transform baseTr)
		{
			this._baseObj = baseTr.gameObject;
			this._imageJapamanFeed = baseTr.Find("AEImage_JapamanFeed").GetComponent<PguiAECtrl>();
			this._imageAList = new List<PguiReplaceAECtrl>();
			this._imageBList = new List<PguiReplaceAECtrl>();
			this._imageCList = new List<PguiReplaceAECtrl>();
			this._imageAList.Add(baseTr.Find("Null_A_01_R/AEImage_Japaman").GetComponent<PguiReplaceAECtrl>());
			this._imageAList.Add(baseTr.Find("Null_A_02_R/AEImage_Japaman").GetComponent<PguiReplaceAECtrl>());
			this._imageAList.Add(baseTr.Find("Null_A_03_L/AEImage_Japaman").GetComponent<PguiReplaceAECtrl>());
			this._imageAList.Add(baseTr.Find("Null_A_04_R/AEImage_Japaman").GetComponent<PguiReplaceAECtrl>());
			this._imageAList.Add(baseTr.Find("Null_A_05_R/AEImage_Japaman").GetComponent<PguiReplaceAECtrl>());
			this._imageAList.Add(baseTr.Find("Null_A_06_L/AEImage_Japaman").GetComponent<PguiReplaceAECtrl>());
			this._imageBList.Add(baseTr.Find("Null_B_01_L/AEImage_Japaman").GetComponent<PguiReplaceAECtrl>());
			this._imageBList.Add(baseTr.Find("Null_B_02_L/AEImage_Japaman").GetComponent<PguiReplaceAECtrl>());
			this._imageBList.Add(baseTr.Find("Null_B_03_L/AEImage_Japaman").GetComponent<PguiReplaceAECtrl>());
			this._imageBList.Add(baseTr.Find("Null_B_04_R/AEImage_Japaman").GetComponent<PguiReplaceAECtrl>());
			this._imageCList.Add(baseTr.Find("Null_C_01_R/AEImage_Japaman").GetComponent<PguiReplaceAECtrl>());
			this._imageCList.Add(baseTr.Find("Null_C_02_L/AEImage_Japaman").GetComponent<PguiReplaceAECtrl>());
			this._imageCList.Add(baseTr.Find("Null_C_03_R/AEImage_Japaman").GetComponent<PguiReplaceAECtrl>());
			foreach (PguiReplaceAECtrl pguiReplaceAECtrl in this._imageAList)
			{
				pguiReplaceAECtrl.InitForce();
			}
			foreach (PguiReplaceAECtrl pguiReplaceAECtrl2 in this._imageBList)
			{
				pguiReplaceAECtrl2.InitForce();
			}
			foreach (PguiReplaceAECtrl pguiReplaceAECtrl3 in this._imageCList)
			{
				pguiReplaceAECtrl3.InitForce();
			}
		}

		// Token: 0x04003FA2 RID: 16290
		private GameObject _baseObj;

		// Token: 0x04003FA3 RID: 16291
		private PguiAECtrl _imageJapamanFeed;

		// Token: 0x04003FA4 RID: 16292
		private List<PguiReplaceAECtrl> _imageAList;

		// Token: 0x04003FA5 RID: 16293
		private List<PguiReplaceAECtrl> _imageBList;

		// Token: 0x04003FA6 RID: 16294
		private List<PguiReplaceAECtrl> _imageCList;
	}

	// Token: 0x020009F9 RID: 2553
	public class WindowItemUse
	{
		// Token: 0x1700091F RID: 2335
		// (get) Token: 0x06003E02 RID: 15874 RVA: 0x001E434C File Offset: 0x001E254C
		public PguiOpenWindowCtrl OpenWindowCtrl
		{
			get
			{
				return this._openWindowCtrl;
			}
		}

		// Token: 0x17000920 RID: 2336
		// (get) Token: 0x06003E03 RID: 15875 RVA: 0x001E4354 File Offset: 0x001E2554
		public PguiButtonCtrl BtnPlus
		{
			get
			{
				return this._btnPlus;
			}
		}

		// Token: 0x17000921 RID: 2337
		// (get) Token: 0x06003E04 RID: 15876 RVA: 0x001E435C File Offset: 0x001E255C
		public PguiButtonCtrl BtnMinus
		{
			get
			{
				return this._btnMinus;
			}
		}

		// Token: 0x17000922 RID: 2338
		// (get) Token: 0x06003E05 RID: 15877 RVA: 0x001E4364 File Offset: 0x001E2564
		public PguiTextCtrl NumExpNext
		{
			get
			{
				return this._numExpNext;
			}
		}

		// Token: 0x17000923 RID: 2339
		// (get) Token: 0x06003E06 RID: 15878 RVA: 0x001E436C File Offset: 0x001E256C
		public PguiImageCtrl GageUp
		{
			get
			{
				return this._gageUp;
			}
		}

		// Token: 0x17000924 RID: 2340
		// (get) Token: 0x06003E07 RID: 15879 RVA: 0x001E4374 File Offset: 0x001E2574
		public PguiImageCtrl Gage
		{
			get
			{
				return this._gage;
			}
		}

		// Token: 0x17000925 RID: 2341
		// (get) Token: 0x06003E08 RID: 15880 RVA: 0x001E437C File Offset: 0x001E257C
		public PguiTextCtrl NumLvBefore
		{
			get
			{
				return this._numLvBefore;
			}
		}

		// Token: 0x17000926 RID: 2342
		// (get) Token: 0x06003E09 RID: 15881 RVA: 0x001E4384 File Offset: 0x001E2584
		public PguiTextCtrl NumLvAfter
		{
			get
			{
				return this._numLvAfter;
			}
		}

		// Token: 0x17000927 RID: 2343
		// (get) Token: 0x06003E0A RID: 15882 RVA: 0x001E438C File Offset: 0x001E258C
		public PguiTextCtrl NumBeforeCoin
		{
			get
			{
				return this._numBeforeCoin;
			}
		}

		// Token: 0x17000928 RID: 2344
		// (get) Token: 0x06003E0B RID: 15883 RVA: 0x001E4394 File Offset: 0x001E2594
		public PguiTextCtrl NumAfterCoin
		{
			get
			{
				return this._numAfterCoin;
			}
		}

		// Token: 0x17000929 RID: 2345
		// (get) Token: 0x06003E0C RID: 15884 RVA: 0x001E439C File Offset: 0x001E259C
		public SelCharaGrowLevel.LvUpItem LvUpItem
		{
			get
			{
				return this._lvUpItem;
			}
		}

		// Token: 0x1700092A RID: 2346
		// (get) Token: 0x06003E0D RID: 15885 RVA: 0x001E43A4 File Offset: 0x001E25A4
		public Slider SliderBar
		{
			get
			{
				return this._sliderBar;
			}
		}

		// Token: 0x06003E0E RID: 15886 RVA: 0x001E43AC File Offset: 0x001E25AC
		public WindowItemUse(Transform baseTr)
		{
			this._openWindowCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this._btnPlus = baseTr.Find("Base/Window/Btn_Plus").GetComponent<PguiButtonCtrl>();
			this._btnMinus = baseTr.Find("Base/Window/Btn_Minus").GetComponent<PguiButtonCtrl>();
			this._btnClose = baseTr.Find("Base/Window/BtnClose").GetComponent<PguiButtonCtrl>();
			this._btnClose.androidBackKeyTarget = true;
			this._numLvBefore = baseTr.Find("Base/Window/ExpInfo/Num_Lv_Before").GetComponent<PguiTextCtrl>();
			this._numLvAfter = baseTr.Find("Base/Window/ExpInfo/Num_Lv_After").GetComponent<PguiTextCtrl>();
			this._numExpNext = baseTr.Find("Base/Window/ExpInfo/Num_Exp_Next").GetComponent<PguiTextCtrl>();
			this._gageUp = baseTr.Find("Base/Window/ExpInfo/ExpGage/Gage_Up").GetComponent<PguiImageCtrl>();
			this._gageUp.gameObject.SetActive(false);
			this._gage = baseTr.Find("Base/Window/ExpInfo/ExpGage/Gage").GetComponent<PguiImageCtrl>();
			this._imgYaji = baseTr.Find("Base/Window/ExpInfo/Img_Yaji").GetComponent<PguiImageCtrl>();
			this._numBeforeCoin = baseTr.Find("Base/Window/Parts_ItemUseCoin/Num_BeforeTxt").GetComponent<PguiTextCtrl>();
			this._numAfterCoin = baseTr.Find("Base/Window/Parts_ItemUseCoin/Num_AfterTxt").GetComponent<PguiTextCtrl>();
			this._sliderBar = baseTr.Find("Base/Window/SliderBar").GetComponent<Slider>();
			Object.Destroy(baseTr.Find("Base/Window/Icon_Item").GetComponent<PguiNestPrefab>());
			Transform transform = baseTr.Find("Base/Window/Icon_Item");
			GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneCharaEdit/GUI/Prefab/CharaGrow_ItemIconSet"), transform);
			gameObject.name = "icon";
			this._lvUpItem = new SelCharaGrowLevel.LvUpItem();
			GameObject gameObject2 = Object.Instantiate<GameObject>((GameObject)Resources.Load("CmnIconFrame/GUI/Prefab/Icon_Item"), gameObject.transform.Find("Icon_Item"));
			this._lvUpItem.iconItemCtrl = gameObject2.GetComponent<IconItemCtrl>();
			this._lvUpItem.expBonus = gameObject.transform.Find("Txt_ExpBonus").GetComponent<PguiTextCtrl>();
			this._lvUpItem.itemNum = gameObject.transform.Find("Num_Own").GetComponent<PguiTextCtrl>();
			this._lvUpItem.ColorBase = gameObject.transform.Find("ColorBase").GetComponent<PguiImageCtrl>();
			this._lvUpItem.imgCount = gameObject.transform.Find("Count").GetComponent<PguiImageCtrl>();
			this._lvUpItem.itemCount = gameObject.transform.Find("Count/Num_Count").GetComponent<PguiTextCtrl>();
			this._lvUpItem.expBonus.gameObject.SetActive(false);
			this._lvUpItem.itemNum.gameObject.SetActive(false);
			this._lvUpItem.ColorBase.gameObject.SetActive(false);
			this._lvUpItem.imgCount.gameObject.SetActive(false);
			this._lvUpItem.itemCount.gameObject.SetActive(false);
		}

		// Token: 0x06003E0F RID: 15887 RVA: 0x001E4684 File Offset: 0x001E2884
		public void SetActiveCtrl(bool doDisp)
		{
			this._numLvBefore.gameObject.SetActive(doDisp);
			this._numLvAfter.gameObject.SetActive(true);
			this._imgYaji.gameObject.SetActive(doDisp);
			this._gageUp.gameObject.SetActive(doDisp);
		}

		// Token: 0x06003E10 RID: 15888 RVA: 0x001E46D5 File Offset: 0x001E28D5
		public void SetUpLvUpItem(bool isMax, int itemDataNum, int count)
		{
			this._lvUpItem.SetUp(isMax, itemDataNum, count);
		}

		// Token: 0x04003FA7 RID: 16295
		private PguiButtonCtrl _btnClose;

		// Token: 0x04003FA8 RID: 16296
		private PguiImageCtrl _imgYaji;

		// Token: 0x04003FA9 RID: 16297
		private PguiOpenWindowCtrl _openWindowCtrl;

		// Token: 0x04003FAA RID: 16298
		private PguiButtonCtrl _btnPlus;

		// Token: 0x04003FAB RID: 16299
		private PguiButtonCtrl _btnMinus;

		// Token: 0x04003FAC RID: 16300
		private PguiTextCtrl _numExpNext;

		// Token: 0x04003FAD RID: 16301
		private PguiImageCtrl _gageUp;

		// Token: 0x04003FAE RID: 16302
		private PguiImageCtrl _gage;

		// Token: 0x04003FAF RID: 16303
		private PguiTextCtrl _numLvBefore;

		// Token: 0x04003FB0 RID: 16304
		private PguiTextCtrl _numLvAfter;

		// Token: 0x04003FB1 RID: 16305
		private PguiTextCtrl _numBeforeCoin;

		// Token: 0x04003FB2 RID: 16306
		private PguiTextCtrl _numAfterCoin;

		// Token: 0x04003FB3 RID: 16307
		private SelCharaGrowLevel.LvUpItem _lvUpItem;

		// Token: 0x04003FB4 RID: 16308
		private Slider _sliderBar;
	}

	// Token: 0x020009FA RID: 2554
	public class WindowLevelLimitOver
	{
		// Token: 0x06003E11 RID: 15889 RVA: 0x001E46E8 File Offset: 0x001E28E8
		public WindowLevelLimitOver(Transform baseTr)
		{
			this._baseObj = baseTr.gameObject;
			this._num_Lv_Before = baseTr.Find("Base/Window/Txt/Img_Yaji/Num_Lv_Before").GetComponent<PguiTextCtrl>();
			this._num_Lv_After = baseTr.Find("Base/Window/Txt/Img_Yaji/Num_Lv_After").GetComponent<PguiTextCtrl>();
			Transform transform = baseTr.Find("Base/Window/LayoutGroup/ItemInfo");
			this._txt01 = transform.Find("Txt01").GetComponent<PguiTextCtrl>();
			this._num_BeforeTxt01 = transform.Find("Img_Yaji/Num_BeforeTxt").GetComponent<PguiTextCtrl>();
			this._num_AfterTxt01 = transform.Find("Img_Yaji/Num_AfterTxt").GetComponent<PguiTextCtrl>();
			this._icon_Tex01 = transform.Find("Icon_Tex").GetComponent<PguiRawImageCtrl>();
			Transform transform2 = baseTr.Find("Base/Window/LayoutGroup/ItemInfo2");
			this._txt02 = transform2.Find("Txt01").GetComponent<PguiTextCtrl>();
			this._num_BeforeTxt02 = transform2.Find("Img_Yaji/Num_BeforeTxt").GetComponent<PguiTextCtrl>();
			this._num_AfterTxt02 = transform2.Find("Img_Yaji/Num_AfterTxt").GetComponent<PguiTextCtrl>();
			this._icon_Tex02 = transform2.Find("Icon_Tex").GetComponent<PguiRawImageCtrl>();
			transform2.gameObject.SetActive(false);
			Transform transform3 = baseTr.Find("Base/Window/LayoutGroup/UseCoin");
			this._num_Coin_BeforeTxt = transform3.Find("Img_Yaji/Num_BeforeTxt").GetComponent<PguiTextCtrl>();
			this._num_Coin_AfterTxt = transform3.Find("Img_Yaji/Num_AfterTxt").GetComponent<PguiTextCtrl>();
			this._owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
		}

		// Token: 0x04003FB5 RID: 16309
		private GameObject _baseObj;

		// Token: 0x04003FB6 RID: 16310
		private PguiTextCtrl _num_Lv_Before;

		// Token: 0x04003FB7 RID: 16311
		private PguiTextCtrl _num_Lv_After;

		// Token: 0x04003FB8 RID: 16312
		private PguiTextCtrl _txt01;

		// Token: 0x04003FB9 RID: 16313
		private PguiTextCtrl _num_BeforeTxt01;

		// Token: 0x04003FBA RID: 16314
		private PguiTextCtrl _num_AfterTxt01;

		// Token: 0x04003FBB RID: 16315
		private PguiRawImageCtrl _icon_Tex01;

		// Token: 0x04003FBC RID: 16316
		private PguiTextCtrl _txt02;

		// Token: 0x04003FBD RID: 16317
		private PguiTextCtrl _num_BeforeTxt02;

		// Token: 0x04003FBE RID: 16318
		private PguiTextCtrl _num_AfterTxt02;

		// Token: 0x04003FBF RID: 16319
		private PguiRawImageCtrl _icon_Tex02;

		// Token: 0x04003FC0 RID: 16320
		private PguiTextCtrl _num_Coin_BeforeTxt;

		// Token: 0x04003FC1 RID: 16321
		private PguiTextCtrl _num_Coin_AfterTxt;

		// Token: 0x04003FC2 RID: 16322
		private PguiOpenWindowCtrl _owCtrl;
	}

	// Token: 0x020009FB RID: 2555
	public class KizunaLevelUpGUI
	{
		// Token: 0x1700092B RID: 2347
		// (get) Token: 0x06003E12 RID: 15890 RVA: 0x001E484B File Offset: 0x001E2A4B
		public SelCharaGrowKizuna.LvupAuth LvupAuth
		{
			get
			{
				return this._lvupAuth;
			}
		}

		// Token: 0x1700092C RID: 2348
		// (get) Token: 0x06003E13 RID: 15891 RVA: 0x001E4853 File Offset: 0x001E2A53
		public SelCharaGrowKizuna.WindowKizunaLvUp LvUpWindow
		{
			get
			{
				return this._lvUpWindow;
			}
		}

		// Token: 0x1700092D RID: 2349
		// (get) Token: 0x06003E14 RID: 15892 RVA: 0x001E485B File Offset: 0x001E2A5B
		public SelCharaGrowKizuna.WindowItemUse ItemUseWindow
		{
			get
			{
				return this._itemUseWindow;
			}
		}

		// Token: 0x06003E15 RID: 15893 RVA: 0x001E4864 File Offset: 0x001E2A64
		public void Setup(Transform baseTr)
		{
			GameObject gameObject = (GameObject)Resources.Load("SceneCharaEdit/GUI/Prefab/GUI_CharaEdit_Window_Lv");
			this._lvupAuth = new SelCharaGrowKizuna.LvupAuth(Object.Instantiate<Transform>(gameObject.transform.Find("Auth_JapamanFeed"), baseTr).transform);
			this._lvupAuth.BaseObj.SetActive(false);
			this._lvUpWindow = new SelCharaGrowKizuna.WindowKizunaLvUp(Object.Instantiate<Transform>(gameObject.transform.Find("Window_LvUp"), baseTr).transform);
			this._itemUseWindow = new SelCharaGrowKizuna.WindowItemUse(Object.Instantiate<Transform>(gameObject.transform.Find("Window_ItemUse"), baseTr).transform);
			this._levelLimitOverWindow = new SelCharaGrowKizuna.WindowLevelLimitOver(Object.Instantiate<Transform>(gameObject.transform.Find("Window_LvLimitOpen"), baseTr).transform);
		}

		// Token: 0x04003FC3 RID: 16323
		private SelCharaGrowKizuna.LvupAuth _lvupAuth;

		// Token: 0x04003FC4 RID: 16324
		private SelCharaGrowKizuna.WindowKizunaLvUp _lvUpWindow;

		// Token: 0x04003FC5 RID: 16325
		private SelCharaGrowKizuna.WindowItemUse _itemUseWindow;

		// Token: 0x04003FC6 RID: 16326
		private SelCharaGrowKizuna.WindowLevelLimitOver _levelLimitOverWindow;
	}
}
