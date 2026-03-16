using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.HttpRequest.Protocol;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CmnItemWindowCtrl : MonoBehaviour
{
	public void Init()
	{
		this.guiSellItemInfoWindow = new CmnItemWindowCtrl.GUI_SellItemInfoWindow(CanvasManager.HdlSellItemInfoWindowCtrl.transform);
		this.guiSellItemInfoWindow.OpenWindowCtrl.Setup("", null, null, true, null, null, false);
		this.guiItemBankContentWindow = new CmnItemWindowCtrl.GUI_ItemBankContentWindow(CanvasManager.HdlBankContentWindowCtrl.transform);
		this.guiItemBankContentWindow.OpenWindowCtrl.Setup(null, null, null, true, null, null, false);
	}

	private void Update()
	{
		if (this.enumSellItem != null && !this.enumSellItem.MoveNext())
		{
			this.enumSellItem = null;
		}
	}

	public void OpenBuyInfoWindow(ItemData itemData)
	{
		this.guiSellItemInfoWindow.Initialize();
		List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>> list = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>
		{
			new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.NEGATIVE, "キャンセル"),
			new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, "売却する")
		};
		this.guiSellItemInfoWindow.OpenWindowCtrl.Setup("売却確認", null, list, true, delegate(int idx)
		{
			if (1 == idx)
			{
				this.enumSellItem = this.SellItemExecute();
			}
			return true;
		}, null, false);
		this.guiSellItemInfoWindow.baseObj.transform.SetAsLastSibling();
		this.guiSellItemInfoWindow.BuyInfo.Parts_Exchange.SetActive(true);
		this.guiSellItemInfoWindow.BuyInfo.ItemNeedInfo.SetActive(true);
		this.guiSellItemInfoWindow.BuyInfo.IconItemId = itemData.id;
		this.guiSellItemInfoWindow.BuyInfo.IconItem.gameObject.SetActive(true);
		this.guiSellItemInfoWindow.BuyInfo.IconItem.Setup(itemData.staticData);
		this.guiSellItemInfoWindow.BuyInfo.IconItemNumTextWindow.SetActive(true);
		this.guiSellItemInfoWindow.BuyInfo.IconItemNumText.text = itemData.num.ToString();
		this.guiSellItemInfoWindow.BuyInfo.ButtonInfo.SetActive(false);
		this.guiSellItemInfoWindow.BuyInfo.SetIconImage();
		this.guiSellItemInfoWindow.BuyInfo.ItemNameText.text = itemData.staticData.GetName();
		this.guiSellItemInfoWindow.BuyInfo.ItemInfoText.m_Text.fontSize = 18;
		this.guiSellItemInfoWindow.BuyInfo.ItemInfoText.text = itemData.staticData.GetInfo();
		this.guiSellItemInfoWindow.BuyInfo.ItemNeedInfoTitle.text = "価格";
		this.guiSellItemInfoWindow.BuyInfo.ItemNeedNum.text = itemData.staticData.GetSalePrice().ToString();
		ItemData userItemData = DataManager.DmItem.GetUserItemData(30101);
		this.guiSellItemInfoWindow.BuyInfo.ItemInfoBefore.text = itemData.num.ToString();
		this.guiSellItemInfoWindow.BuyInfo.GoldInfoBefore.text = userItemData.num.ToString();
		long num = Math.Min(2147483647L, DataManagerItem.GetUserHaveMaxNum(30101, 0) - (long)userItemData.num) / (long)itemData.staticData.GetSalePrice();
		this.guiSellItemInfoWindow.BuyInfo.inputNum = (int)Math.Min(1L, num);
		bool flag = this.guiSellItemInfoWindow.BuyInfo.ItemData.num > this.guiSellItemInfoWindow.BuyInfo.inputNum && num > (long)this.guiSellItemInfoWindow.BuyInfo.inputNum;
		this.guiSellItemInfoWindow.BuyInfo.BtnPlus.SetActEnable(flag, false, false);
		this.guiSellItemInfoWindow.BuyInfo.BtnMinus.SetActEnable(false, false, false);
		this.guiSellItemInfoWindow.BuyInfo.BtnPlus.AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			this.OnClickPlusButton();
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.guiSellItemInfoWindow.BuyInfo.BtnMinus.AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			this.OnClickMinusButton();
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.guiSellItemInfoWindow.BuyInfo.SliderBar.onValueChanged.RemoveAllListeners();
		this.guiSellItemInfoWindow.BuyInfo.SliderBar.minValue = (float)Math.Min(1L, num);
		this.guiSellItemInfoWindow.BuyInfo.SliderBar.maxValue = Mathf.Min((float)num, (float)itemData.num);
		this.guiSellItemInfoWindow.BuyInfo.SliderBar.onValueChanged.AddListener(new UnityAction<float>(this.OnSliderValueChanged));
		this.UpdateSellInfo();
		this.guiSellItemInfoWindow.OpenWindowCtrl.Open();
	}

	private void OnClickPlusButton()
	{
		this.guiSellItemInfoWindow.BuyInfo.inputNum++;
		if (this.guiSellItemInfoWindow.BuyInfo.ItemData.num <= this.guiSellItemInfoWindow.BuyInfo.inputNum)
		{
			this.guiSellItemInfoWindow.BuyInfo.BtnPlus.SetActEnable(false, false, false);
		}
		this.guiSellItemInfoWindow.BuyInfo.BtnMinus.SetActEnable(true, false, false);
		this.UpdateSellInfo();
	}

	private void OnClickMinusButton()
	{
		this.guiSellItemInfoWindow.BuyInfo.inputNum--;
		if (1 >= this.guiSellItemInfoWindow.BuyInfo.inputNum)
		{
			this.guiSellItemInfoWindow.BuyInfo.BtnMinus.SetActEnable(false, false, false);
		}
		this.guiSellItemInfoWindow.BuyInfo.BtnPlus.SetActEnable(true, false, false);
		this.UpdateSellInfo();
	}

	private void OnSliderValueChanged(float value)
	{
		this.guiSellItemInfoWindow.BuyInfo.inputNum = int.Parse(value.ToString());
		this.UpdateSellInfo();
	}

	private void UpdateSellInfo()
	{
		ItemData userItemData = DataManager.DmItem.GetUserItemData(this.guiSellItemInfoWindow.BuyInfo.IconItemId);
		this.guiSellItemInfoWindow.BuyInfo.NumText.text = this.guiSellItemInfoWindow.BuyInfo.inputNum.ToString();
		this.guiSellItemInfoWindow.BuyInfo.SliderBar.value = (float)this.guiSellItemInfoWindow.BuyInfo.inputNum;
		this.guiSellItemInfoWindow.BuyInfo.ItemInfoAfter.text = (userItemData.num - this.guiSellItemInfoWindow.BuyInfo.inputNum).ToString();
		ItemData userItemData2 = DataManager.DmItem.GetUserItemData(30101);
		int num = this.guiSellItemInfoWindow.BuyInfo.inputNum * userItemData.staticData.GetSalePrice();
		this.guiSellItemInfoWindow.BuyInfo.addGoldNum = num;
		this.guiSellItemInfoWindow.BuyInfo.GoldInfoAfter.text = ((long)userItemData2.num + (long)num).ToString();
		int num2 = int.Parse(this.guiSellItemInfoWindow.BuyInfo.SliderBar.minValue.ToString());
		int num3 = int.Parse(this.guiSellItemInfoWindow.BuyInfo.SliderBar.maxValue.ToString());
		this.guiSellItemInfoWindow.BuyInfo.BtnMinus.SetActEnable((float)num2 != this.guiSellItemInfoWindow.BuyInfo.SliderBar.value, false, false);
		this.guiSellItemInfoWindow.BuyInfo.BtnPlus.SetActEnable((float)num3 != this.guiSellItemInfoWindow.BuyInfo.SliderBar.value, false, false);
		this.guiSellItemInfoWindow.BtnOK.SetActEnable(this.guiSellItemInfoWindow.BuyInfo.inputNum > 0, false, false);
		this.guiSellItemInfoWindow.BtnOkDisableText.text = ((this.guiSellItemInfoWindow.BuyInfo.inputNum > 0) ? "" : "売却できません");
	}

	private void OpenSellReportWindow(bool isGoldStock)
	{
		this.guiSellItemInfoWindow.Initialize();
		List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>> list = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>
		{
			new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, "閉じる")
		};
		this.guiSellItemInfoWindow.OpenWindowCtrl.Setup("売却確認", null, list, true, null, null, false);
		this.guiSellItemInfoWindow.baseObj.transform.SetAsLastSibling();
		this.guiSellItemInfoWindow.BuyInfo.Parts_Exchange.SetActive(false);
		this.guiSellItemInfoWindow.BuyInfo.ItemNeedInfo.SetActive(false);
		DataManager.DmItem.GetUserItemData(30101);
		this.guiSellItemInfoWindow.BuyInfo.IconItem.gameObject.SetActive(false);
		this.guiSellItemInfoWindow.BuyInfo.IconItemNumTextWindow.SetActive(false);
		this.guiSellItemInfoWindow.BuyInfo.ItemInfoText.m_Text.fontSize = 22;
		string text = "を" + this.guiSellItemInfoWindow.BuyInfo.inputNum.ToString() + "個売却しました。";
		if (isGoldStock)
		{
			text = string.Concat(new string[]
			{
				text,
				"\n\n\n所持数上限を超える",
				DataManager.DmItem.GetItemStaticBase(30101).GetName(),
				"は",
				DataManager.DmItem.GetItemStaticBase(30090).GetName(),
				"に補充されました"
			});
		}
		this.guiSellItemInfoWindow.BuyInfo.ItemInfoText.text = text;
		this.guiSellItemInfoWindow.BuyInfo.SetIconImage();
		this.guiSellItemInfoWindow.OpenWindowCtrl.Open();
		this.selItemViewCtrl.RefreshScrollItem();
	}

	private IEnumerator SellItemExecute()
	{
		List<ItemInput> list = new List<ItemInput>();
		list.Add(new ItemInput(this.guiSellItemInfoWindow.BuyInfo.IconItemId, this.guiSellItemInfoWindow.BuyInfo.inputNum));
		bool isGoldStock = DataManagerItem.IsExpectedItemStock(30101, (long)this.guiSellItemInfoWindow.BuyInfo.addGoldNum);
		DataManager.DmItem.RequestActionItemSell(list);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		while (!this.guiSellItemInfoWindow.OpenWindowCtrl.FinishedClose())
		{
			yield return null;
		}
		this.OpenSellReportWindow(isGoldStock);
		yield break;
	}

	public void OpenBankContentWindow(ItemData itemData)
	{
		this.guiItemBankContentWindow.baseObj.transform.SetAsLastSibling();
		ItemBank itemBank = DataManager.DmItem.UserItemBankMap.TryGetValueEx(itemData.id, null);
		if (itemBank == null)
		{
			itemBank = new ItemBank
			{
				item_id = itemData.id,
				content_id = ((itemData.id == 30090) ? 30101 : 0),
				content_num = 0L
			};
		}
		ItemStaticBase itemStaticBase = DataManager.DmItem.GetItemStaticBase(itemBank.content_id);
		this.guiItemBankContentWindow.Txt01.text = this.guiItemBankContentWindow.Txt01.m_OriginalText.Replace("{itemName}", itemData.staticData.GetName());
		string text = this.guiItemBankContentWindow.Txt02.m_OriginalText.Replace("{itemName}", itemData.staticData.GetName()).Replace("{contentName}", itemStaticBase.GetName()).Replace("{stock}", itemStaticBase.GetStackMax().ToString());
		this.guiItemBankContentWindow.Txt02.text = text;
		this.guiItemBankContentWindow.ItemRow.transform.Find("BaseImage/Txt").GetComponent<PguiTextCtrl>().text = itemStaticBase.GetName();
		this.guiItemBankContentWindow.ItemRow.transform.Find("BaseImage/Cmn_Parts_Info/Num_01").GetComponent<PguiTextCtrl>().text = string.Format("<size=24>所持数\u3000</size>{0}", itemBank.content_num);
		this.guiItemBankContentWindow.ItemRow.transform.Find("BaseImage/Icon_Item").GetComponent<IconItemCtrl>().Setup(itemStaticBase);
		this.guiItemBankContentWindow.OpenWindowCtrl.Open();
	}

	private IEnumerator enumSellItem;

	private CmnItemWindowCtrl.GUI_SellItemInfoWindow guiSellItemInfoWindow;

	private CmnItemWindowCtrl.GUI_ItemBankContentWindow guiItemBankContentWindow;

	public SelItemViewCtrl selItemViewCtrl;

	private enum WindowMode
	{
		BuyInfo,
		SetInfo,
		BuyReport,
		ImgInfo
	}

	public class GUI_SellItemInfoWindow
	{
		public GUI_SellItemInfoWindow(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.OpenWindowCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			Transform transform = baseTr.Find("Base/Window");
			transform.Find("Base_BuyInfo/Buy_Img_Exchange").gameObject.SetActive(false);
			transform.Find("Base_BuyInfo/Parts_NotPhotoBuy").gameObject.SetActive(false);
			this.WindowBtnClose = transform.Find("Btn_WindowClose").GetComponent<PguiButtonCtrl>();
			this.BtnOK = transform.Find("BtnOk").GetComponent<PguiButtonCtrl>();
			this.BtnOkDisableText = transform.Find("BtnOk/DisableText").GetComponent<PguiTextCtrl>();
			this.BtnOkDisableText.text = "";
			this.BuyInfo = new CmnItemWindowCtrl.GUI_SellItemInfoWindow.BuyInfoData(transform.Find("Base_BuyInfo").gameObject);
		}

		public void Initialize()
		{
			this.WindowBtnClose.gameObject.SetActive(true);
			this.WindowBtnClose.SetActEnable(true, false, false);
			this.BtnOkDisableText.text = "";
		}

		public GameObject baseObj;

		public PguiOpenWindowCtrl OpenWindowCtrl;

		public PguiButtonCtrl WindowBtnClose;

		public PguiButtonCtrl BtnOK;

		public PguiTextCtrl BtnOkDisableText;

		public CmnItemWindowCtrl.GUI_SellItemInfoWindow.BuyInfoData BuyInfo;

		public CmnItemWindowCtrl.GUI_SellItemInfoWindow.BuyReportData BuyReport;

		public CmnItemWindowCtrl.GUI_SellItemInfoWindow.SetInfoData SetInfo;

		public IconItemCtrl presetIconItem;

		public PguiTextCtrl presetItemNum;

		public PguiTextCtrl presetItemName;

		public PguiTextCtrl presetItemInfo;

		public ReuseScroll ScrollView;

		public class BuyInfoData
		{
			public ItemData ItemData
			{
				get
				{
					return DataManager.DmItem.GetUserItemData(this.IconItemId);
				}
			}

			public BuyInfoData(GameObject obj)
			{
				this.baseObj = obj;
				Transform transform = this.baseObj.transform;
				this.IconItem = transform.Find("Buy_Img/Icon_Item").GetComponent<IconItemCtrl>();
				this.IconItem.transform.SetAsFirstSibling();
				this.IconItemNumTextWindow = transform.Find("Buy_Img/Txt_Window").gameObject;
				this.IconItemNumText = transform.Find("Buy_Img/Txt_Window/Num_Txt").GetComponent<PguiTextCtrl>();
				this.ButtonInfo = transform.Find("Buy_Img/Cmn_Btn_Info").gameObject;
				this.ItemNameText = transform.Find("Txt01").GetComponent<PguiTextCtrl>();
				this.ItemInfoText = transform.Find("Txt02").GetComponent<PguiTextCtrl>();
				this.Parts_Exchange = transform.Find("Parts_Exchange/Exchange").gameObject;
				this.BtnPlus = transform.Find("Parts_Exchange/Exchange/Btn_Plus").GetComponent<PguiButtonCtrl>();
				this.BtnMinus = transform.Find("Parts_Exchange/Exchange/Btn_Minus").GetComponent<PguiButtonCtrl>();
				this.NumText = transform.Find("Parts_Exchange/Exchange/Num_Txt").GetComponent<PguiTextCtrl>();
				this.SliderBar = transform.Find("Parts_Exchange/Exchange/SliderBar").GetComponent<Slider>();
				this.ItemNeedInfo = transform.Find("Parts_Exchange/Parts_ItemNeedInfo").gameObject;
				this.ItemNeedInfoTitle = transform.Find("Parts_Exchange/Parts_ItemNeedInfo/Txt").GetComponent<PguiTextCtrl>();
				this.ItemNeedNum = transform.Find("Parts_Exchange/Parts_ItemNeedInfo/Num_Txt").GetComponent<PguiTextCtrl>();
				this.NeedInfoImage = transform.Find("Parts_Exchange/Parts_ItemNeedInfo/Icon_Tex").GetComponent<PguiRawImageCtrl>();
				this.ItemInfo = transform.Find("Parts_Exchange/Parts_ItemUseInfo").gameObject;
				this.ItemInfoTitle = transform.Find("Parts_Exchange/Parts_ItemUseInfo/Txt01").GetComponent<PguiTextCtrl>();
				this.ItemInfoBefore = transform.Find("Parts_Exchange/Parts_ItemUseInfo/Num_BeforeTxt").GetComponent<PguiTextCtrl>();
				this.ItemInfoAfter = transform.Find("Parts_Exchange/Parts_ItemUseInfo/Num_AfterTxt").GetComponent<PguiTextCtrl>();
				this.ItemInfoImage = transform.Find("Parts_Exchange/Parts_ItemUseInfo/Icon_Tex").GetComponent<PguiRawImageCtrl>();
				this.GoldInfo = transform.Find("Parts_Exchange/Parts_ItemUseCoin").gameObject;
				this.GoldInfoTitle = transform.Find("Parts_Exchange/Parts_ItemUseCoin/Txt01").GetComponent<PguiTextCtrl>();
				this.GoldInfoBefore = transform.Find("Parts_Exchange/Parts_ItemUseCoin/Num_BeforeTxt").GetComponent<PguiTextCtrl>();
				this.GoldInfoAfter = transform.Find("Parts_Exchange/Parts_ItemUseCoin/Num_AfterTxt").GetComponent<PguiTextCtrl>();
				this.GoldImage = transform.Find("Parts_Exchange/Parts_ItemUseCoin/Icon_Tex").GetComponent<PguiRawImageCtrl>();
			}

			public void SetIconImage()
			{
				this.NeedInfoImage.SetRawImage(DataManager.DmItem.GetItemStaticBase(30101).GetIconName(), true, false, null);
				this.ItemInfoImage.gameObject.SetActive(false);
				this.GoldImage.SetRawImage(DataManager.DmItem.GetItemStaticBase(30101).GetIconName(), true, false, null);
				this.NeedInfoImage.GetComponent<RectTransform>().sizeDelta = new Vector2(40f, 40f);
				this.ItemInfoImage.GetComponent<RectTransform>().sizeDelta = new Vector2(40f, 40f);
				this.GoldImage.GetComponent<RectTransform>().sizeDelta = new Vector2(40f, 40f);
			}

			public GameObject baseObj;

			public IconItemCtrl IconItem;

			public GameObject IconItemNumTextWindow;

			public PguiTextCtrl IconItemNumText;

			public PguiTextCtrl ItemNameText;

			public PguiTextCtrl ItemInfoText;

			public GameObject ButtonInfo;

			public GameObject Parts_Exchange;

			public PguiButtonCtrl BtnPlus;

			public PguiButtonCtrl BtnMinus;

			public PguiTextCtrl NumText;

			public GameObject ItemNeedInfo;

			public PguiTextCtrl ItemNeedInfoTitle;

			public PguiTextCtrl ItemNeedNum;

			public GameObject ItemInfo;

			public PguiTextCtrl ItemInfoTitle;

			public PguiTextCtrl ItemInfoBefore;

			public PguiTextCtrl ItemInfoAfter;

			public GameObject GoldInfo;

			public PguiTextCtrl GoldInfoTitle;

			public PguiTextCtrl GoldInfoBefore;

			public PguiTextCtrl GoldInfoAfter;

			public Slider SliderBar;

			public PguiRawImageCtrl NeedInfoImage;

			public PguiRawImageCtrl ItemInfoImage;

			public PguiRawImageCtrl GoldImage;

			public int IconItemId;

			public int inputNum;

			public int addGoldNum;
		}

		public class BuyReportData
		{
			public BuyReportData(GameObject obj)
			{
				this.baseObj = obj;
				Transform transform = this.baseObj.transform;
				this.ItemNameText = transform.Find("Txt01").GetComponent<PguiTextCtrl>();
				this.ItemInfoText = transform.Find("Txt02").GetComponent<PguiTextCtrl>();
				this.ItemInfo = transform.Find("Parts_ItemUseInfo").gameObject;
				this.ItemInfoTitle = transform.Find("Parts_ItemUseInfo/Txt01").GetComponent<PguiTextCtrl>();
				this.ItemInfoBefore = transform.Find("Parts_ItemUseInfo/Num_BeforeTxt").GetComponent<PguiTextCtrl>();
				this.ItemInfoAfter = transform.Find("Parts_ItemUseInfo/Num_AfterTxt").GetComponent<PguiTextCtrl>();
				this.GoldInfo = transform.Find("Parts_ItemUseCoin").gameObject;
				this.GoldInfoTitle = transform.Find("Parts_ItemUseCoin/Txt01").GetComponent<PguiTextCtrl>();
				this.GoldInfoBefore = transform.Find("Parts_ItemUseCoin/Num_BeforeTxt").GetComponent<PguiTextCtrl>();
				this.GoldInfoAfter = transform.Find("Parts_ItemUseCoin/Num_AfterTxt").GetComponent<PguiTextCtrl>();
				this.ItemInfoImage = transform.Find("Parts_ItemUseInfo/Icon_Tex").GetComponent<PguiRawImageCtrl>();
				this.GoldImage = transform.Find("Parts_ItemUseCoin/Icon_Tex").GetComponent<PguiRawImageCtrl>();
			}

			public void SetIconImage(int goldIconId)
			{
				this.ItemInfoImage.gameObject.SetActive(false);
				this.GoldImage.SetRawImage(DataManager.DmItem.GetItemStaticBase(goldIconId).GetIconName(), true, false, null);
				this.ItemInfoImage.GetComponent<RectTransform>().sizeDelta = new Vector2(40f, 40f);
				this.GoldImage.GetComponent<RectTransform>().sizeDelta = new Vector2(40f, 40f);
			}

			public GameObject baseObj;

			public PguiTextCtrl ItemNameText;

			public PguiTextCtrl ItemInfoText;

			public GameObject ItemInfo;

			public PguiTextCtrl ItemInfoTitle;

			public PguiTextCtrl ItemInfoBefore;

			public PguiTextCtrl ItemInfoAfter;

			public GameObject GoldInfo;

			public PguiTextCtrl GoldInfoTitle;

			public PguiTextCtrl GoldInfoBefore;

			public PguiTextCtrl GoldInfoAfter;

			public PguiRawImageCtrl ItemInfoImage;

			public PguiRawImageCtrl GoldImage;
		}

		public class SetInfoData
		{
			public SetInfoData(GameObject obj)
			{
				this.baseObj = obj;
				Transform transform = this.baseObj.transform;
			}

			public GameObject baseObj;
		}

		public class ImgInfoData
		{
			public ImgInfoData(GameObject obj)
			{
				this.baseObj = obj;
				Transform transform = this.baseObj.transform;
			}

			public GameObject baseObj;
		}
	}

	public class GUI_ItemBankContentWindow
	{
		public GUI_ItemBankContentWindow(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.OpenWindowCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			Transform transform = baseTr.Find("Base/Window");
			this.Txt01 = transform.Find("Txt01").GetComponent<PguiTextCtrl>();
			this.Txt02 = transform.Find("Txt02").GetComponent<PguiTextCtrl>();
			this.ItemRow = transform.Find("ItemRow").gameObject;
		}

		public GameObject baseObj;

		public PguiOpenWindowCtrl OpenWindowCtrl;

		public PguiTextCtrl Txt01;

		public PguiTextCtrl Txt02;

		public GameObject ItemRow;
	}
}
