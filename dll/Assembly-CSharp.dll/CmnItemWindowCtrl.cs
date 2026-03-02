using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.HttpRequest.Protocol;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000197 RID: 407
public class CmnItemWindowCtrl : MonoBehaviour
{
	// Token: 0x06001B17 RID: 6935 RVA: 0x0015CE2C File Offset: 0x0015B02C
	public void Init()
	{
		this.guiSellItemInfoWindow = new CmnItemWindowCtrl.GUI_SellItemInfoWindow(CanvasManager.HdlSellItemInfoWindowCtrl.transform);
		this.guiSellItemInfoWindow.OpenWindowCtrl.Setup("", null, null, true, null, null, false);
		this.guiItemBankContentWindow = new CmnItemWindowCtrl.GUI_ItemBankContentWindow(CanvasManager.HdlBankContentWindowCtrl.transform);
		this.guiItemBankContentWindow.OpenWindowCtrl.Setup(null, null, null, true, null, null, false);
	}

	// Token: 0x06001B18 RID: 6936 RVA: 0x0015CE95 File Offset: 0x0015B095
	private void Update()
	{
		if (this.enumSellItem != null && !this.enumSellItem.MoveNext())
		{
			this.enumSellItem = null;
		}
	}

	// Token: 0x06001B19 RID: 6937 RVA: 0x0015CEB4 File Offset: 0x0015B0B4
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

	// Token: 0x06001B1A RID: 6938 RVA: 0x0015D28C File Offset: 0x0015B48C
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

	// Token: 0x06001B1B RID: 6939 RVA: 0x0015D310 File Offset: 0x0015B510
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

	// Token: 0x06001B1C RID: 6940 RVA: 0x0015D37E File Offset: 0x0015B57E
	private void OnSliderValueChanged(float value)
	{
		this.guiSellItemInfoWindow.BuyInfo.inputNum = int.Parse(value.ToString());
		this.UpdateSellInfo();
	}

	// Token: 0x06001B1D RID: 6941 RVA: 0x0015D3A4 File Offset: 0x0015B5A4
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

	// Token: 0x06001B1E RID: 6942 RVA: 0x0015D5BC File Offset: 0x0015B7BC
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

	// Token: 0x06001B1F RID: 6943 RVA: 0x0015D763 File Offset: 0x0015B963
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

	// Token: 0x06001B20 RID: 6944 RVA: 0x0015D774 File Offset: 0x0015B974
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

	// Token: 0x0400147F RID: 5247
	private IEnumerator enumSellItem;

	// Token: 0x04001480 RID: 5248
	private CmnItemWindowCtrl.GUI_SellItemInfoWindow guiSellItemInfoWindow;

	// Token: 0x04001481 RID: 5249
	private CmnItemWindowCtrl.GUI_ItemBankContentWindow guiItemBankContentWindow;

	// Token: 0x04001482 RID: 5250
	public SelItemViewCtrl selItemViewCtrl;

	// Token: 0x02000EA7 RID: 3751
	private enum WindowMode
	{
		// Token: 0x04005408 RID: 21512
		BuyInfo,
		// Token: 0x04005409 RID: 21513
		SetInfo,
		// Token: 0x0400540A RID: 21514
		BuyReport,
		// Token: 0x0400540B RID: 21515
		ImgInfo
	}

	// Token: 0x02000EA8 RID: 3752
	public class GUI_SellItemInfoWindow
	{
		// Token: 0x06004D48 RID: 19784 RVA: 0x00231AB0 File Offset: 0x0022FCB0
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

		// Token: 0x06004D49 RID: 19785 RVA: 0x00231B80 File Offset: 0x0022FD80
		public void Initialize()
		{
			this.WindowBtnClose.gameObject.SetActive(true);
			this.WindowBtnClose.SetActEnable(true, false, false);
			this.BtnOkDisableText.text = "";
		}

		// Token: 0x0400540C RID: 21516
		public GameObject baseObj;

		// Token: 0x0400540D RID: 21517
		public PguiOpenWindowCtrl OpenWindowCtrl;

		// Token: 0x0400540E RID: 21518
		public PguiButtonCtrl WindowBtnClose;

		// Token: 0x0400540F RID: 21519
		public PguiButtonCtrl BtnOK;

		// Token: 0x04005410 RID: 21520
		public PguiTextCtrl BtnOkDisableText;

		// Token: 0x04005411 RID: 21521
		public CmnItemWindowCtrl.GUI_SellItemInfoWindow.BuyInfoData BuyInfo;

		// Token: 0x04005412 RID: 21522
		public CmnItemWindowCtrl.GUI_SellItemInfoWindow.BuyReportData BuyReport;

		// Token: 0x04005413 RID: 21523
		public CmnItemWindowCtrl.GUI_SellItemInfoWindow.SetInfoData SetInfo;

		// Token: 0x04005414 RID: 21524
		public IconItemCtrl presetIconItem;

		// Token: 0x04005415 RID: 21525
		public PguiTextCtrl presetItemNum;

		// Token: 0x04005416 RID: 21526
		public PguiTextCtrl presetItemName;

		// Token: 0x04005417 RID: 21527
		public PguiTextCtrl presetItemInfo;

		// Token: 0x04005418 RID: 21528
		public ReuseScroll ScrollView;

		// Token: 0x020011EB RID: 4587
		public class BuyInfoData
		{
			// Token: 0x17000D08 RID: 3336
			// (get) Token: 0x06005756 RID: 22358 RVA: 0x00256CD1 File Offset: 0x00254ED1
			public ItemData ItemData
			{
				get
				{
					return DataManager.DmItem.GetUserItemData(this.IconItemId);
				}
			}

			// Token: 0x06005757 RID: 22359 RVA: 0x00256CE4 File Offset: 0x00254EE4
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

			// Token: 0x06005758 RID: 22360 RVA: 0x00256F40 File Offset: 0x00255140
			public void SetIconImage()
			{
				this.NeedInfoImage.SetRawImage(DataManager.DmItem.GetItemStaticBase(30101).GetIconName(), true, false, null);
				this.ItemInfoImage.gameObject.SetActive(false);
				this.GoldImage.SetRawImage(DataManager.DmItem.GetItemStaticBase(30101).GetIconName(), true, false, null);
				this.NeedInfoImage.GetComponent<RectTransform>().sizeDelta = new Vector2(40f, 40f);
				this.ItemInfoImage.GetComponent<RectTransform>().sizeDelta = new Vector2(40f, 40f);
				this.GoldImage.GetComponent<RectTransform>().sizeDelta = new Vector2(40f, 40f);
			}

			// Token: 0x04006236 RID: 25142
			public GameObject baseObj;

			// Token: 0x04006237 RID: 25143
			public IconItemCtrl IconItem;

			// Token: 0x04006238 RID: 25144
			public GameObject IconItemNumTextWindow;

			// Token: 0x04006239 RID: 25145
			public PguiTextCtrl IconItemNumText;

			// Token: 0x0400623A RID: 25146
			public PguiTextCtrl ItemNameText;

			// Token: 0x0400623B RID: 25147
			public PguiTextCtrl ItemInfoText;

			// Token: 0x0400623C RID: 25148
			public GameObject ButtonInfo;

			// Token: 0x0400623D RID: 25149
			public GameObject Parts_Exchange;

			// Token: 0x0400623E RID: 25150
			public PguiButtonCtrl BtnPlus;

			// Token: 0x0400623F RID: 25151
			public PguiButtonCtrl BtnMinus;

			// Token: 0x04006240 RID: 25152
			public PguiTextCtrl NumText;

			// Token: 0x04006241 RID: 25153
			public GameObject ItemNeedInfo;

			// Token: 0x04006242 RID: 25154
			public PguiTextCtrl ItemNeedInfoTitle;

			// Token: 0x04006243 RID: 25155
			public PguiTextCtrl ItemNeedNum;

			// Token: 0x04006244 RID: 25156
			public GameObject ItemInfo;

			// Token: 0x04006245 RID: 25157
			public PguiTextCtrl ItemInfoTitle;

			// Token: 0x04006246 RID: 25158
			public PguiTextCtrl ItemInfoBefore;

			// Token: 0x04006247 RID: 25159
			public PguiTextCtrl ItemInfoAfter;

			// Token: 0x04006248 RID: 25160
			public GameObject GoldInfo;

			// Token: 0x04006249 RID: 25161
			public PguiTextCtrl GoldInfoTitle;

			// Token: 0x0400624A RID: 25162
			public PguiTextCtrl GoldInfoBefore;

			// Token: 0x0400624B RID: 25163
			public PguiTextCtrl GoldInfoAfter;

			// Token: 0x0400624C RID: 25164
			public Slider SliderBar;

			// Token: 0x0400624D RID: 25165
			public PguiRawImageCtrl NeedInfoImage;

			// Token: 0x0400624E RID: 25166
			public PguiRawImageCtrl ItemInfoImage;

			// Token: 0x0400624F RID: 25167
			public PguiRawImageCtrl GoldImage;

			// Token: 0x04006250 RID: 25168
			public int IconItemId;

			// Token: 0x04006251 RID: 25169
			public int inputNum;

			// Token: 0x04006252 RID: 25170
			public int addGoldNum;
		}

		// Token: 0x020011EC RID: 4588
		public class BuyReportData
		{
			// Token: 0x06005759 RID: 22361 RVA: 0x00257000 File Offset: 0x00255200
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

			// Token: 0x0600575A RID: 22362 RVA: 0x00257130 File Offset: 0x00255330
			public void SetIconImage(int goldIconId)
			{
				this.ItemInfoImage.gameObject.SetActive(false);
				this.GoldImage.SetRawImage(DataManager.DmItem.GetItemStaticBase(goldIconId).GetIconName(), true, false, null);
				this.ItemInfoImage.GetComponent<RectTransform>().sizeDelta = new Vector2(40f, 40f);
				this.GoldImage.GetComponent<RectTransform>().sizeDelta = new Vector2(40f, 40f);
			}

			// Token: 0x04006253 RID: 25171
			public GameObject baseObj;

			// Token: 0x04006254 RID: 25172
			public PguiTextCtrl ItemNameText;

			// Token: 0x04006255 RID: 25173
			public PguiTextCtrl ItemInfoText;

			// Token: 0x04006256 RID: 25174
			public GameObject ItemInfo;

			// Token: 0x04006257 RID: 25175
			public PguiTextCtrl ItemInfoTitle;

			// Token: 0x04006258 RID: 25176
			public PguiTextCtrl ItemInfoBefore;

			// Token: 0x04006259 RID: 25177
			public PguiTextCtrl ItemInfoAfter;

			// Token: 0x0400625A RID: 25178
			public GameObject GoldInfo;

			// Token: 0x0400625B RID: 25179
			public PguiTextCtrl GoldInfoTitle;

			// Token: 0x0400625C RID: 25180
			public PguiTextCtrl GoldInfoBefore;

			// Token: 0x0400625D RID: 25181
			public PguiTextCtrl GoldInfoAfter;

			// Token: 0x0400625E RID: 25182
			public PguiRawImageCtrl ItemInfoImage;

			// Token: 0x0400625F RID: 25183
			public PguiRawImageCtrl GoldImage;
		}

		// Token: 0x020011ED RID: 4589
		public class SetInfoData
		{
			// Token: 0x0600575B RID: 22363 RVA: 0x002571AA File Offset: 0x002553AA
			public SetInfoData(GameObject obj)
			{
				this.baseObj = obj;
				Transform transform = this.baseObj.transform;
			}

			// Token: 0x04006260 RID: 25184
			public GameObject baseObj;
		}

		// Token: 0x020011EE RID: 4590
		public class ImgInfoData
		{
			// Token: 0x0600575C RID: 22364 RVA: 0x002571C5 File Offset: 0x002553C5
			public ImgInfoData(GameObject obj)
			{
				this.baseObj = obj;
				Transform transform = this.baseObj.transform;
			}

			// Token: 0x04006261 RID: 25185
			public GameObject baseObj;
		}
	}

	// Token: 0x02000EA9 RID: 3753
	public class GUI_ItemBankContentWindow
	{
		// Token: 0x06004D4A RID: 19786 RVA: 0x00231BB4 File Offset: 0x0022FDB4
		public GUI_ItemBankContentWindow(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.OpenWindowCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			Transform transform = baseTr.Find("Base/Window");
			this.Txt01 = transform.Find("Txt01").GetComponent<PguiTextCtrl>();
			this.Txt02 = transform.Find("Txt02").GetComponent<PguiTextCtrl>();
			this.ItemRow = transform.Find("ItemRow").gameObject;
		}

		// Token: 0x04005419 RID: 21529
		public GameObject baseObj;

		// Token: 0x0400541A RID: 21530
		public PguiOpenWindowCtrl OpenWindowCtrl;

		// Token: 0x0400541B RID: 21531
		public PguiTextCtrl Txt01;

		// Token: 0x0400541C RID: 21532
		public PguiTextCtrl Txt02;

		// Token: 0x0400541D RID: 21533
		public GameObject ItemRow;
	}
}
