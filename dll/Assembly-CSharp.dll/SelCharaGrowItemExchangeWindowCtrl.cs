using System;
using System.Collections.Generic;
using SGNFW.HttpRequest.Protocol;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelCharaGrowItemExchangeWindowCtrl : MonoBehaviour
{
	public void Initialize(Transform transform)
	{
		this.guiItemUseWindow = new SelCharaGrowItemExchangeWindowCtrl.ExchangeItemUseNumWindow(transform);
	}

	public void Setup(DataManagerItem.ExchangeRatesData exchange, Action<int> action)
	{
		List<ExchangeExecuteCountInfo> executeCountInfos = DataManager.DmItem.GetExecuteCountInfos();
		this.currentExchangeData = exchange;
		this.actionExecuteCount = action;
		ExchangeExecuteCountInfo exchangeExecuteCountInfo = executeCountInfos.Find((ExchangeExecuteCountInfo info) => info.targetItemId == exchange.targetItemId);
		ItemData userItemData = DataManager.DmItem.GetUserItemData(exchange.sourceItemId);
		ItemData userItemData2 = DataManager.DmItem.GetUserItemData(exchange.targetItemId);
		int num = userItemData.num / exchange.useNum;
		this.currentMaxExecuteCount = ((exchangeExecuteCountInfo == null) ? exchange.monthlyExchangeLimit : (exchange.monthlyExchangeLimit - exchangeExecuteCountInfo.executeCount));
		this.currentMaxExecuteCount = ((this.currentMaxExecuteCount <= num) ? this.currentMaxExecuteCount : num);
		this.guiItemUseWindow.buttonOk.AddOnClickListener(delegate(PguiButtonCtrl button)
		{
			int num2 = int.Parse(this.guiItemUseWindow.slider.value.ToString());
			Action<int> action2 = this.actionExecuteCount;
			if (action2 == null)
			{
				return;
			}
			action2(num2);
		}, PguiButtonCtrl.SoundType.DEFAULT);
		ItemStaticBase itemStaticBase = DataManager.DmItem.GetItemStaticBase(exchange.targetItemId);
		ItemStaticBase itemStaticBase2 = DataManager.DmItem.GetItemStaticBase(exchange.sourceItemId);
		this.guiItemUseWindow.buttonClose.androidBackKeyTarget = true;
		this.guiItemUseWindow.sourceIconItemPack.iconItemCtrl.Setup(itemStaticBase2);
		this.guiItemUseWindow.sourceIconItemPack.textCtrl.text = itemStaticBase2.GetName();
		this.guiItemUseWindow.targetIconItemPack.iconItemCtrl.Setup(itemStaticBase);
		this.guiItemUseWindow.targetIconItemPack.textCtrl.text = itemStaticBase.GetName();
		this.guiItemUseWindow.exchangeRatioText.ReplaceTextByDefault(new string[] { "Param01", "Param02", "Param03", "Param04" }, new string[]
		{
			itemStaticBase2.GetName(),
			exchange.useNum.ToString(),
			itemStaticBase.GetName(),
			exchange.gainNum.ToString()
		});
		this.guiItemUseWindow.sourceItemTex.SetRawImage(itemStaticBase2.GetIconName(), true, false, null);
		this.guiItemUseWindow.targetItemTex.SetRawImage(itemStaticBase.GetIconName(), true, false, null);
		this.guiItemUseWindow.sourceItemNameText.text = itemStaticBase2.GetName();
		this.guiItemUseWindow.targetItemNameText.text = itemStaticBase.GetName();
		this.guiItemUseWindow.useItemNumText.text = userItemData.num.ToString();
		this.guiItemUseWindow.gainItemNumText.text = userItemData2.num.ToString();
		this.guiItemUseWindow.numBeforeSourceItemText.text = userItemData.num.ToString();
		this.guiItemUseWindow.numAfterSourceItemText.text = (userItemData.num - exchange.useNum).ToString();
		this.guiItemUseWindow.numBeforeTargetItemText.text = userItemData2.num.ToString();
		this.guiItemUseWindow.numAfterTargetItemText.text = (userItemData2.num + exchange.gainNum).ToString();
		this.guiItemUseWindow.slider.onValueChanged.RemoveAllListeners();
		this.guiItemUseWindow.slider.value = 1f;
		this.guiItemUseWindow.slider.minValue = 1f;
		this.guiItemUseWindow.slider.maxValue = (float)this.currentMaxExecuteCount;
		this.guiItemUseWindow.slider.onValueChanged.AddListener(new UnityAction<float>(this.OnSliderValueChanged));
		this.guiItemUseWindow.executeCountText.text = int.Parse(this.guiItemUseWindow.slider.value.ToString()).ToString();
		this.guiItemUseWindow.buttonInc.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickPlusButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiItemUseWindow.buttonDec.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickMinusButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiItemUseWindow.buttonInc.SetActEnable(this.guiItemUseWindow.slider.value < this.guiItemUseWindow.slider.maxValue, false, false);
		this.guiItemUseWindow.buttonDec.SetActEnable(false, false, false);
	}

	private void OnSliderValueChanged(float value)
	{
		int num = int.Parse(value.ToString());
		this.guiItemUseWindow.executeCountText.text = num.ToString();
		int num2 = int.Parse(this.guiItemUseWindow.slider.maxValue.ToString());
		int num3 = int.Parse(this.guiItemUseWindow.slider.minValue.ToString());
		this.guiItemUseWindow.buttonInc.SetActEnable(num < num2, false, false);
		this.guiItemUseWindow.buttonDec.SetActEnable(num > num3, false, false);
		ItemData userItemData = DataManager.DmItem.GetUserItemData(this.currentExchangeData.sourceItemId);
		ItemData userItemData2 = DataManager.DmItem.GetUserItemData(this.currentExchangeData.targetItemId);
		this.guiItemUseWindow.numAfterSourceItemText.text = (userItemData.num - this.currentExchangeData.useNum * num).ToString();
		this.guiItemUseWindow.numAfterTargetItemText.text = (userItemData2.num + this.currentExchangeData.gainNum * num).ToString();
	}

	private void OnClickPlusButton(PguiButtonCtrl button)
	{
		int num = int.Parse(this.guiItemUseWindow.slider.value.ToString());
		int num2 = int.Parse(this.guiItemUseWindow.slider.maxValue.ToString());
		if (num < num2)
		{
			num++;
		}
		this.guiItemUseWindow.slider.value = (float)num;
	}

	private void OnClickMinusButton(PguiButtonCtrl button)
	{
		int num = int.Parse(this.guiItemUseWindow.slider.value.ToString());
		int num2 = int.Parse(this.guiItemUseWindow.slider.minValue.ToString());
		if (num > num2)
		{
			num--;
		}
		this.guiItemUseWindow.slider.value = (float)num;
	}

	public SelCharaGrowItemExchangeWindowCtrl.ExchangeItemUseNumWindow guiItemUseWindow;

	private DataManagerItem.ExchangeRatesData currentExchangeData;

	private Action<int> actionExecuteCount;

	private int currentMaxExecuteCount;

	public class ExchangeItemUseNumWindow
	{
		public ExchangeItemUseNumWindow(Transform baseTr)
		{
			this.window = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.baseObj = baseTr.gameObject;
			this.buttonClose = baseTr.Find("Base/Window/Window_BtnClose").GetComponent<PguiButtonCtrl>();
			this.buttonOk = baseTr.Find("Base/Window/BtnOk").GetComponent<PguiButtonCtrl>();
			string text = "Base/Window/Base_UseInfo/";
			this.buttonInc = baseTr.Find(text + "Exchange/Btn_Plus").gameObject.GetComponent<PguiButtonCtrl>();
			this.buttonDec = baseTr.Find(text + "Exchange/Btn_Minus").gameObject.GetComponent<PguiButtonCtrl>();
			this.numBeforeSourceItemText = baseTr.Find(text + "Parts_SourceItemInfo/Num_BeforeTxt").gameObject.GetComponent<PguiTextCtrl>();
			this.numAfterSourceItemText = baseTr.Find(text + "Parts_SourceItemInfo/Num_AfterTxt").gameObject.GetComponent<PguiTextCtrl>();
			this.numBeforeTargetItemText = baseTr.Find(text + "Parts_TargetItemInfo/Num_BeforeTxt").gameObject.GetComponent<PguiTextCtrl>();
			this.numAfterTargetItemText = baseTr.Find(text + "Parts_TargetItemInfo/Num_AfterTxt").gameObject.GetComponent<PguiTextCtrl>();
			this.sourceItemNameText = baseTr.Find(text + "Parts_SourceItemInfo/Txt01").gameObject.GetComponent<PguiTextCtrl>();
			this.targetItemNameText = baseTr.Find(text + "Parts_TargetItemInfo/Txt01").gameObject.GetComponent<PguiTextCtrl>();
			this.useItemNumText = baseTr.Find(text + "Source_Item/Parts_ItemUseInfo/Num_AfterTxt").gameObject.GetComponent<PguiTextCtrl>();
			this.gainItemNumText = baseTr.Find(text + "Target_Item/Parts_ItemUseInfo/Num_AfterTxt").gameObject.GetComponent<PguiTextCtrl>();
			this.excText = baseTr.Find(text + "Exchange/Num_Txt").gameObject.GetComponent<PguiTextCtrl>();
			this.slider = baseTr.Find(text + "Exchange/SliderBar").GetComponent<Slider>();
			this.sliderObj = baseTr.Find(text + "Exchange/SliderBar").gameObject;
			Transform transform = baseTr.Find(text + "Source_Item");
			GameObject gameObject = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, transform);
			this.sourceIconItemPack.iconItemCtrl = gameObject.GetComponent<IconItemCtrl>();
			this.sourceIconItemPack.textCtrl = transform.Find("Txt").GetComponent<PguiTextCtrl>();
			transform = baseTr.Find(text + "Target_Item");
			gameObject = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, transform);
			this.targetIconItemPack.iconItemCtrl = gameObject.GetComponent<IconItemCtrl>();
			this.targetIconItemPack.textCtrl = transform.Find("Txt").GetComponent<PguiTextCtrl>();
			this.exchangeRatioText = baseTr.Find(text + "Txt_RateInfo").gameObject.GetComponent<PguiTextCtrl>();
			this.sourceItemTex = baseTr.Find(text + "Parts_SourceItemInfo/Icon_Tex").gameObject.GetComponent<PguiRawImageCtrl>();
			this.targetItemTex = baseTr.Find(text + "Parts_TargetItemInfo/Icon_Tex").gameObject.GetComponent<PguiRawImageCtrl>();
			this.executeCountText = baseTr.Find(text + "Exchange/Num_Txt").gameObject.GetComponent<PguiTextCtrl>();
		}

		public GameObject baseObj;

		public PguiOpenWindowCtrl window;

		public PguiButtonCtrl buttonClose;

		public PguiButtonCtrl buttonOk;

		public PguiButtonCtrl buttonInc;

		public PguiButtonCtrl buttonDec;

		public PguiTextCtrl numBeforeSourceItemText;

		public PguiTextCtrl numAfterSourceItemText;

		public PguiTextCtrl numBeforeTargetItemText;

		public PguiTextCtrl numAfterTargetItemText;

		public PguiTextCtrl sourceItemNameText;

		public PguiTextCtrl targetItemNameText;

		public PguiTextCtrl useItemNumText;

		public PguiTextCtrl gainItemNumText;

		public PguiTextCtrl executeCountText;

		public PguiTextCtrl excText;

		public Slider slider;

		public GameObject sliderObj;

		public SelCharaGrowItemExchangeWindowCtrl.ExchangeItemUseNumWindow.IconItemPack sourceIconItemPack = new SelCharaGrowItemExchangeWindowCtrl.ExchangeItemUseNumWindow.IconItemPack();

		public SelCharaGrowItemExchangeWindowCtrl.ExchangeItemUseNumWindow.IconItemPack targetIconItemPack = new SelCharaGrowItemExchangeWindowCtrl.ExchangeItemUseNumWindow.IconItemPack();

		public PguiTextCtrl exchangeRatioText;

		public PguiRawImageCtrl sourceItemTex;

		public PguiRawImageCtrl targetItemTex;

		public class IconItemPack
		{
			public void Clear()
			{
				if (this.iconItemCtrl != null)
				{
					this.iconItemCtrl.Clear();
				}
				if (this.textCtrl != null)
				{
					this.textCtrl.text = "";
				}
			}

			public IconItemCtrl iconItemCtrl;

			public PguiTextCtrl textCtrl;
		}
	}
}
