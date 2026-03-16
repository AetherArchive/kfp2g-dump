using System;
using System.Collections.Generic;
using AEAuth3;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.UI;

public class SelCharaGrowKizuna
{
	public SelCharaGrowKizuna.CharaGrowKizunaGUI GrowKizunaGUI
	{
		get
		{
			return this._growKizunaGUI;
		}
	}

	public SelCharaGrowKizuna.KizunaLevelUpGUI LevelUpGUI
	{
		get
		{
			return this._levelUpGUI;
		}
	}

	public SelCharaGrowKizuna(Transform baseTr)
	{
		this._growKizunaGUI = new SelCharaGrowKizuna.CharaGrowKizunaGUI();
		this._growKizunaGUI.Setup(baseTr);
		this._levelUpGUI = new SelCharaGrowKizuna.KizunaLevelUpGUI();
		this._levelUpGUI.Setup(baseTr);
	}

	public void Initialize(PguiOpenWindowCtrl.Callback windowCallBack)
	{
		List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>> list = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>
		{
			new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.NEGATIVE, "キャンセル"),
			new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, "決定")
		};
		this._growKizunaGUI.KizunaWindow.OpenWindowCtrl.Setup("なかよしレベル上限解放確認", null, list, true, windowCallBack, null, false);
	}

	public void CreateLvUpItem(GameObject go, int i, int itemId, int attr)
	{
		GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneCharaEdit/GUI/Prefab/CharaGrow_ItemIconSet"), go.transform);
		gameObject.name = i.ToString();
		SelCharaGrowKizuna.KizunaLvUpItem kizunaLvUpItem = new SelCharaGrowKizuna.KizunaLvUpItem(Object.Instantiate<GameObject>((GameObject)Resources.Load("CmnIconFrame/GUI/Prefab/Icon_Item"), gameObject.transform.Find("Icon_Item")), gameObject, itemId);
		this._growKizunaGUI.KizunaLvUpTab.IconItemList.Add(kizunaLvUpItem);
		this._growKizunaGUI.KizunaLvUpTab.ItemListBar[attr].IconItemListKizuna.Add(kizunaLvUpItem);
	}

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

	public void UpdateLimitLvItemActivation(int charaId)
	{
		this.SetupLimitLvItemActivation(charaId, false);
	}

	public void AdjustExpInfoActivation()
	{
		this._growKizunaGUI.KizunaLvUpTab.ExpInfoObject.SetActive(true);
	}

	public bool CheckIsPossibleLevelUp(int charaId)
	{
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(charaId);
		return userCharaData.dynamicData.KizunaLimitLevel > userCharaData.dynamicData.kizunaLevel;
	}

	public void SetActiveTab(bool islvMax)
	{
		this._growKizunaGUI.KizunaTab.ParentObject.SetActive(islvMax);
		this._growKizunaGUI.KizunaLvUpTab.BaseObj.SetActive(!islvMax);
	}

	private void OnClickReleaseButton(PguiButtonCtrl btn)
	{
		this._growKizunaGUI.KizunaWindow.OpenWindowCtrl.Open();
	}

	private readonly string KIZUNA_LEVEL_UP_TAB_MESSAGE = "マジカルキャンディを使ってなかよしポイントを獲得します";

	private SelCharaGrowKizuna.CharaGrowKizunaGUI _growKizunaGUI;

	private SelCharaGrowKizuna.KizunaLevelUpGUI _levelUpGUI;

	public class KizunaTab
	{
		public GameObject ParentObject
		{
			get
			{
				return this._parentObject;
			}
		}

		public GameObject BaseObject
		{
			get
			{
				return this._baseObject;
			}
		}

		public PguiTextCtrl TxtItemName
		{
			get
			{
				return this._txtItemName;
			}
		}

		public PguiTextCtrl NumItem
		{
			get
			{
				return this._numItem;
			}
		}

		public IconItemCtrl IconItem
		{
			get
			{
				return this._iconItem;
			}
		}

		public PguiButtonCtrl ReleaseButton
		{
			get
			{
				return this._releaseButton;
			}
		}

		public PguiTextCtrl NowLimitLevelText
		{
			get
			{
				return this._nowLimitLevelText;
			}
		}

		public PguiTextCtrl HeartInfoText
		{
			get
			{
				return this._heartInfoText;
			}
		}

		public AEImage ImageResult
		{
			get
			{
				return this._imageResult;
			}
		}

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

		private void SetUpActivation()
		{
			this._baseObject.SetActive(true);
			this._imageResult.gameObject.SetActive(false);
		}

		private GameObject _expInfoObject;

		private GameObject _parentObject;

		private GameObject _baseObject;

		private PguiTextCtrl _txtItemName;

		private PguiTextCtrl _numItem;

		private IconItemCtrl _iconItem;

		private PguiButtonCtrl _releaseButton;

		private PguiTextCtrl _nowLimitLevelText;

		private PguiTextCtrl _heartInfoText;

		private AEImage _imageResult;
	}

	public class KizunaLvUpItem
	{
		public int ItemId
		{
			get
			{
				return this._itemId;
			}
		}

		public IconItemCtrl IconItemCtrl
		{
			get
			{
				return this._iconItemCtrl;
			}
		}

		public PguiTextCtrl ExpBonus
		{
			get
			{
				return this._expBonus;
			}
		}

		public PguiTextCtrl ItemNum
		{
			get
			{
				return this._itemNum;
			}
		}

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

		public void SetActiveImageCountGameObject(bool isActive)
		{
			this._imgCount.gameObject.SetActive(isActive);
		}

		public void SetTextItemCount(string text)
		{
			this._itemCount.text = text;
		}

		private int _itemId;

		private IconItemCtrl _iconItemCtrl;

		private PguiTextCtrl _expBonus;

		private PguiTextCtrl _itemNum;

		private PguiImageCtrl _imgCount;

		private PguiTextCtrl _itemCount;

		private PguiImageCtrl _colorBase;
	}

	public class KizunaLvUpTab
	{
		public GameObject BaseObj
		{
			get
			{
				return this._baseObj;
			}
		}

		public PguiTextCtrl NumLvLeft
		{
			get
			{
				return this._numLvLeft;
			}
		}

		public PguiTextCtrl NumLvRight
		{
			get
			{
				return this._numLvRight;
			}
		}

		public PguiTextCtrl NumResult
		{
			get
			{
				return this._numResult;
			}
		}

		public SimpleAnimation ResultLvup
		{
			get
			{
				return this._resultLvup;
			}
		}

		public ReuseScroll ScrollView
		{
			get
			{
				return this._scrollView;
			}
		}

		public List<SelCharaGrowCtrl.CommonGUI.ItemListBar> ItemListBar
		{
			get
			{
				return this._itemListBar;
			}
		}

		public List<SelCharaGrowKizuna.KizunaLvUpItem> IconItemList
		{
			get
			{
				return this._iconItemList;
			}
		}

		public PguiImageCtrl Gage
		{
			get
			{
				return this._gage;
			}
		}

		public PguiImageCtrl ImgYaji
		{
			get
			{
				return this._imgYaji;
			}
		}

		public AEImage ImageResult
		{
			get
			{
				return this._imageResult;
			}
		}

		public AEImage ImageLevelUP
		{
			get
			{
				return this._imageLevelUP;
			}
		}

		public GameObject ExpInfoObject
		{
			get
			{
				return this._expInfoObject;
			}
		}

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

		public void SetAction(Action<int, GameObject> setupAction, Action<int, GameObject> updateAction)
		{
			ReuseScroll scrollView = this._scrollView;
			scrollView.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollView.onStartItem, setupAction);
			ReuseScroll scrollView2 = this._scrollView;
			scrollView2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollView2.onUpdateItem, updateAction);
			this._scrollView.Setup(1, 0);
		}

		public void SetActiveGage(bool isActive)
		{
			this._gage.gameObject.SetActive(isActive);
		}

		public void SetText(string leftText, string rightText, string expNextText)
		{
			this._numLvLeft.text = leftText;
			this._numLvRight.text = rightText;
			this._numExpNext.text = expNextText;
		}

		public void SetGageUpImageFillAmount(float fillAmount)
		{
			this._gageUp.m_Image.fillAmount = fillAmount;
		}

		public void SetGageImageFillAmount(float fillAmount)
		{
			this._gage.m_Image.fillAmount = fillAmount;
		}

		public void AddGageImageFillAmount(float fillAmount)
		{
			this._gage.m_Image.fillAmount += fillAmount;
		}

		public void SetAEImageLevelUP()
		{
			this._imageLevelUP.gameObject.SetActive(true);
			this._imageLevelUP.playTime = 0f;
			this._imageLevelUP.autoPlay = true;
			this._imageLevelUP.playLoop = false;
		}

		public void SetActiveCtrl(bool doDisp, bool effectDisp)
		{
			this._numLvLeft.gameObject.SetActive(doDisp || effectDisp);
			this._numLvRight.gameObject.SetActive(true);
			this._imgYaji.gameObject.SetActive(doDisp || effectDisp);
			this._gageUp.gameObject.SetActive(doDisp);
		}

		private GameObject _baseObj;

		private PguiTextCtrl _numLvLeft;

		private PguiTextCtrl _numLvRight;

		private PguiTextCtrl _numResult;

		private SimpleAnimation _resultLvup;

		private ReuseScroll _scrollView;

		private List<SelCharaGrowCtrl.CommonGUI.ItemListBar> _itemListBar;

		private List<SelCharaGrowKizuna.KizunaLvUpItem> _iconItemList;

		private PguiTextCtrl _numExpNext;

		private PguiImageCtrl _gageUp;

		private PguiImageCtrl _gage;

		private PguiImageCtrl _imgYaji;

		private AEImage _imageResult;

		private AEImage _imageLevelUP;

		private GameObject _expInfoObject;
	}

	public class WindowKizunaLvUp
	{
		public PguiOpenWindowCtrl OpenWindowCtrl
		{
			get
			{
				return this._openWindowCtrl;
			}
		}

		public PguiTextCtrl NumCoinUse
		{
			get
			{
				return this._numCoinUse;
			}
		}

		public PguiTextCtrl NumCoinOwn
		{
			get
			{
				return this._numCoinOwn;
			}
		}

		public List<ItemInput> ItemList
		{
			get
			{
				return this._itemList;
			}
		}

		public IconCharaCtrl IconChara
		{
			get
			{
				return this._iconChara;
			}
		}

		public PguiTextCtrl TxtCharaName
		{
			get
			{
				return this._txtCharaName;
			}
		}

		public ReuseScroll ScrollView
		{
			get
			{
				return this._scrollView;
			}
		}

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

		public void SetAction(Action<int, GameObject> setupAction, Action<int, GameObject> updateAction)
		{
			this._scrollView.InitForce();
			ReuseScroll scrollView = this._scrollView;
			scrollView.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollView.onStartItem, setupAction);
			ReuseScroll scrollView2 = this._scrollView;
			scrollView2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollView2.onUpdateItem, updateAction);
			this._scrollView.Setup(1, 0);
		}

		public void SetActiveGage(bool isActive)
		{
			this._gage.gameObject.SetActive(isActive);
		}

		public void SetText(string beforeText, string afterText, string expNextText)
		{
			this._numLvBefore.text = beforeText;
			this._numLvAfter.text = afterText;
			this._numExpNext.text = expNextText;
		}

		public void SetGageUpImageFillAmount(float fillAmount)
		{
			this._gageUp.m_Image.fillAmount = fillAmount;
		}

		public void SetGageImageFillAmount(float fillAmount)
		{
			this._gage.m_Image.fillAmount = fillAmount;
		}

		public void SetItemImputList(List<ItemInput> itemList)
		{
			this._itemList = itemList;
		}

		private PguiTextCtrl _numLvBefore;

		private PguiTextCtrl _numLvAfter;

		private PguiImageCtrl _gageUp;

		private PguiImageCtrl _gage;

		private PguiTextCtrl _numExpNext;

		private PguiOpenWindowCtrl _openWindowCtrl;

		private PguiTextCtrl _numCoinUse;

		private PguiTextCtrl _numCoinOwn;

		private List<ItemInput> _itemList;

		private IconCharaCtrl _iconChara;

		private PguiTextCtrl _txtCharaName;

		private ReuseScroll _scrollView;

		public const int SCROLL_ITEM_NUN_H = 5;
	}

	public class KizunaLvupAuth
	{
		public GameObject BaseObj
		{
			get
			{
				return this._baseObj;
			}
		}

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

		private List<PguiReplaceAECtrl> _imageAList;

		private List<PguiReplaceAECtrl> _imageBList;

		private List<PguiReplaceAECtrl> _imageCList;

		private GameObject _baseObj;
	}

	public class WindowKizunaLimit
	{
		public PguiOpenWindowCtrl OpenWindowCtrl
		{
			get
			{
				return this._openWindowCtrl;
			}
		}

		public PguiTextCtrl BeforeLevelText
		{
			get
			{
				return this._beforeLevelText;
			}
		}

		public PguiTextCtrl AfterLevelText
		{
			get
			{
				return this._afterLevelText;
			}
		}

		public PguiTextCtrl ItemNameText
		{
			get
			{
				return this._itemNameText;
			}
		}

		public PguiTextCtrl ItemBeforeNumText
		{
			get
			{
				return this._itemBeforeNumText;
			}
		}

		public PguiTextCtrl ItemAfterNumText
		{
			get
			{
				return this._itemAfterNumText;
			}
		}

		public PguiRawImageCtrl IconTex
		{
			get
			{
				return this._iconTex;
			}
		}

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

		private PguiOpenWindowCtrl _openWindowCtrl;

		private PguiTextCtrl _beforeLevelText;

		private PguiTextCtrl _afterLevelText;

		private PguiTextCtrl _itemNameText;

		private PguiTextCtrl _itemBeforeNumText;

		private PguiTextCtrl _itemAfterNumText;

		private PguiRawImageCtrl _iconTex;
	}

	public class KizunaLevelUpEffectWindow
	{
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

		public void SetCurrentCharaPackData(CharaPackData charaPackData)
		{
			this._currentCharaPackData = charaPackData;
		}

		public bool CheckKizunaWinCharaIsActive()
		{
			return this._kizunaWinChara != null && this._kizunaWinChara.gameObject.activeSelf;
		}

		public bool CheckTouchIsActive()
		{
			return this._touch.gameObject.activeSelf;
		}

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

		private void CallbackKizunaUpChara()
		{
			this._kizunaWinChrY = this._kizunaWinChara.GetNodePos("j_neck").y + 0.005f;
			this._kizunaWinChara.SetAnimation(CharaMotionDefine.ActKey.GACHA_LP, true);
			if (!this._kizunaWinInfo.gameObject.activeSelf)
			{
				this.SkipKizunaUp();
			}
		}

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

		private readonly int RENDER_TEXTURE_WIDTH = 1654;

		private readonly int RENDER_TEXTURE_HEIGHT = 1024;

		private readonly int MAX_STRING_COUNT = 2;

		private readonly string KEMONO_MIRACLE_LEVEL_LIMIT_RELEASE_TEXT = "けものミラクルレベルの上限が開放されました";

		private readonly string CHARA_STORY_RELEASE_TEXT = "新たなキャラストーリーが開放されました";

		private GameObject _windowPanel;

		private GameObject _kizunaWindow;

		private PguiAECtrl _kizunaWinWhite;

		private PguiAECtrl _kizunaWinBack;

		private PguiAECtrl _kizunaWinFront;

		private PguiAECtrl _kizunaWinInfo;

		private int _kizunaWinId;

		private int _kizunaWinCloth;

		private bool _kizunaWinLongSkirt;

		private RenderTextureChara _kizunaWinChara;

		private bool _kizunaWinCharaVoice;

		private float _kizunaWinTime;

		private float _kizunaWinChrY;

		private List<ItemData> _kizunaWinItem;

		private Dictionary<int, int> _afterItemIdToSourceItemId;

		private Transform _touch;

		private bool _isTouch;

		private CharaPackData _currentCharaPackData;
	}

	public class CharaGrowKizunaGUI
	{
		public SelCharaGrowKizuna.WindowKizunaLimit KizunaWindow
		{
			get
			{
				return this._kizunaWindow;
			}
		}

		public SelCharaGrowKizuna.KizunaTab KizunaTab
		{
			get
			{
				return this._kizunaTab;
			}
		}

		public SelCharaGrowKizuna.KizunaLvUpTab KizunaLvUpTab
		{
			get
			{
				return this._kizunaLvUpTab;
			}
		}

		public SelCharaGrowKizuna.WindowKizunaLvUp KizunaLvUpWindow
		{
			get
			{
				return this._kizunaLvUpWindow;
			}
		}

		public SelCharaGrowKizuna.KizunaLevelUpEffectWindow KizunaLevelUpEffectWindow
		{
			get
			{
				return this._kizunaLevelUpEffectWindow;
			}
		}

		public void SetActiveCtrl(bool doDisp, bool effectDisp)
		{
			this.KizunaLvUpTab.SetActiveCtrl(doDisp, effectDisp);
		}

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

		private SelCharaGrowKizuna.KizunaLvupAuth _kizunaLevelUpAuth;

		private SelCharaGrowKizuna.WindowKizunaLimit _kizunaWindow;

		private SelCharaGrowKizuna.KizunaTab _kizunaTab;

		private SelCharaGrowKizuna.KizunaLvUpTab _kizunaLvUpTab;

		private SelCharaGrowKizuna.WindowKizunaLvUp _kizunaLvUpWindow;

		private SelCharaGrowKizuna.KizunaLevelUpEffectWindow _kizunaLevelUpEffectWindow;
	}

	public class LvupAuth
	{
		public GameObject BaseObj
		{
			get
			{
				return this._baseObj;
			}
		}

		public PguiAECtrl ImageJapamanFeed
		{
			get
			{
				return this._imageJapamanFeed;
			}
		}

		public List<PguiReplaceAECtrl> ImageAList
		{
			get
			{
				return this._imageAList;
			}
		}

		public List<PguiReplaceAECtrl> ImageBList
		{
			get
			{
				return this._imageBList;
			}
		}

		public List<PguiReplaceAECtrl> ImageCList
		{
			get
			{
				return this._imageCList;
			}
		}

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

		private GameObject _baseObj;

		private PguiAECtrl _imageJapamanFeed;

		private List<PguiReplaceAECtrl> _imageAList;

		private List<PguiReplaceAECtrl> _imageBList;

		private List<PguiReplaceAECtrl> _imageCList;
	}

	public class WindowItemUse
	{
		public PguiOpenWindowCtrl OpenWindowCtrl
		{
			get
			{
				return this._openWindowCtrl;
			}
		}

		public PguiButtonCtrl BtnPlus
		{
			get
			{
				return this._btnPlus;
			}
		}

		public PguiButtonCtrl BtnMinus
		{
			get
			{
				return this._btnMinus;
			}
		}

		public PguiTextCtrl NumExpNext
		{
			get
			{
				return this._numExpNext;
			}
		}

		public PguiImageCtrl GageUp
		{
			get
			{
				return this._gageUp;
			}
		}

		public PguiImageCtrl Gage
		{
			get
			{
				return this._gage;
			}
		}

		public PguiTextCtrl NumLvBefore
		{
			get
			{
				return this._numLvBefore;
			}
		}

		public PguiTextCtrl NumLvAfter
		{
			get
			{
				return this._numLvAfter;
			}
		}

		public PguiTextCtrl NumBeforeCoin
		{
			get
			{
				return this._numBeforeCoin;
			}
		}

		public PguiTextCtrl NumAfterCoin
		{
			get
			{
				return this._numAfterCoin;
			}
		}

		public SelCharaGrowLevel.LvUpItem LvUpItem
		{
			get
			{
				return this._lvUpItem;
			}
		}

		public Slider SliderBar
		{
			get
			{
				return this._sliderBar;
			}
		}

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

		public void SetActiveCtrl(bool doDisp)
		{
			this._numLvBefore.gameObject.SetActive(doDisp);
			this._numLvAfter.gameObject.SetActive(true);
			this._imgYaji.gameObject.SetActive(doDisp);
			this._gageUp.gameObject.SetActive(doDisp);
		}

		public void SetUpLvUpItem(bool isMax, int itemDataNum, int count)
		{
			this._lvUpItem.SetUp(isMax, itemDataNum, count);
		}

		private PguiButtonCtrl _btnClose;

		private PguiImageCtrl _imgYaji;

		private PguiOpenWindowCtrl _openWindowCtrl;

		private PguiButtonCtrl _btnPlus;

		private PguiButtonCtrl _btnMinus;

		private PguiTextCtrl _numExpNext;

		private PguiImageCtrl _gageUp;

		private PguiImageCtrl _gage;

		private PguiTextCtrl _numLvBefore;

		private PguiTextCtrl _numLvAfter;

		private PguiTextCtrl _numBeforeCoin;

		private PguiTextCtrl _numAfterCoin;

		private SelCharaGrowLevel.LvUpItem _lvUpItem;

		private Slider _sliderBar;
	}

	public class WindowLevelLimitOver
	{
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

		private GameObject _baseObj;

		private PguiTextCtrl _num_Lv_Before;

		private PguiTextCtrl _num_Lv_After;

		private PguiTextCtrl _txt01;

		private PguiTextCtrl _num_BeforeTxt01;

		private PguiTextCtrl _num_AfterTxt01;

		private PguiRawImageCtrl _icon_Tex01;

		private PguiTextCtrl _txt02;

		private PguiTextCtrl _num_BeforeTxt02;

		private PguiTextCtrl _num_AfterTxt02;

		private PguiRawImageCtrl _icon_Tex02;

		private PguiTextCtrl _num_Coin_BeforeTxt;

		private PguiTextCtrl _num_Coin_AfterTxt;

		private PguiOpenWindowCtrl _owCtrl;
	}

	public class KizunaLevelUpGUI
	{
		public SelCharaGrowKizuna.LvupAuth LvupAuth
		{
			get
			{
				return this._lvupAuth;
			}
		}

		public SelCharaGrowKizuna.WindowKizunaLvUp LvUpWindow
		{
			get
			{
				return this._lvUpWindow;
			}
		}

		public SelCharaGrowKizuna.WindowItemUse ItemUseWindow
		{
			get
			{
				return this._itemUseWindow;
			}
		}

		public void Setup(Transform baseTr)
		{
			GameObject gameObject = (GameObject)Resources.Load("SceneCharaEdit/GUI/Prefab/GUI_CharaEdit_Window_Lv");
			this._lvupAuth = new SelCharaGrowKizuna.LvupAuth(Object.Instantiate<Transform>(gameObject.transform.Find("Auth_JapamanFeed"), baseTr).transform);
			this._lvupAuth.BaseObj.SetActive(false);
			this._lvUpWindow = new SelCharaGrowKizuna.WindowKizunaLvUp(Object.Instantiate<Transform>(gameObject.transform.Find("Window_LvUp"), baseTr).transform);
			this._itemUseWindow = new SelCharaGrowKizuna.WindowItemUse(Object.Instantiate<Transform>(gameObject.transform.Find("Window_ItemUse"), baseTr).transform);
			this._levelLimitOverWindow = new SelCharaGrowKizuna.WindowLevelLimitOver(Object.Instantiate<Transform>(gameObject.transform.Find("Window_LvLimitOpen"), baseTr).transform);
		}

		private SelCharaGrowKizuna.LvupAuth _lvupAuth;

		private SelCharaGrowKizuna.WindowKizunaLvUp _lvUpWindow;

		private SelCharaGrowKizuna.WindowItemUse _itemUseWindow;

		private SelCharaGrowKizuna.WindowLevelLimitOver _levelLimitOverWindow;
	}
}
