using System;
using System.Collections.Generic;
using SGNFW.HttpRequest.Protocol;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200012F RID: 303
public class SelCharaGrowItemExchangeWindowCtrl : MonoBehaviour
{
	// Token: 0x06001050 RID: 4176 RVA: 0x000C61B8 File Offset: 0x000C43B8
	public void Initialize(Transform transform)
	{
		this.guiItemUseWindow = new SelCharaGrowItemExchangeWindowCtrl.ExchangeItemUseNumWindow(transform);
	}

	// Token: 0x06001051 RID: 4177 RVA: 0x000C61C8 File Offset: 0x000C43C8
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

	// Token: 0x06001052 RID: 4178 RVA: 0x000C6638 File Offset: 0x000C4838
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

	// Token: 0x06001053 RID: 4179 RVA: 0x000C675C File Offset: 0x000C495C
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

	// Token: 0x06001054 RID: 4180 RVA: 0x000C67C0 File Offset: 0x000C49C0
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

	// Token: 0x04000E46 RID: 3654
	public SelCharaGrowItemExchangeWindowCtrl.ExchangeItemUseNumWindow guiItemUseWindow;

	// Token: 0x04000E47 RID: 3655
	private DataManagerItem.ExchangeRatesData currentExchangeData;

	// Token: 0x04000E48 RID: 3656
	private Action<int> actionExecuteCount;

	// Token: 0x04000E49 RID: 3657
	private int currentMaxExecuteCount;

	// Token: 0x020009EE RID: 2542
	public class ExchangeItemUseNumWindow
	{
		// Token: 0x06003DA2 RID: 15778 RVA: 0x001E2424 File Offset: 0x001E0624
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

		// Token: 0x04003F34 RID: 16180
		public GameObject baseObj;

		// Token: 0x04003F35 RID: 16181
		public PguiOpenWindowCtrl window;

		// Token: 0x04003F36 RID: 16182
		public PguiButtonCtrl buttonClose;

		// Token: 0x04003F37 RID: 16183
		public PguiButtonCtrl buttonOk;

		// Token: 0x04003F38 RID: 16184
		public PguiButtonCtrl buttonInc;

		// Token: 0x04003F39 RID: 16185
		public PguiButtonCtrl buttonDec;

		// Token: 0x04003F3A RID: 16186
		public PguiTextCtrl numBeforeSourceItemText;

		// Token: 0x04003F3B RID: 16187
		public PguiTextCtrl numAfterSourceItemText;

		// Token: 0x04003F3C RID: 16188
		public PguiTextCtrl numBeforeTargetItemText;

		// Token: 0x04003F3D RID: 16189
		public PguiTextCtrl numAfterTargetItemText;

		// Token: 0x04003F3E RID: 16190
		public PguiTextCtrl sourceItemNameText;

		// Token: 0x04003F3F RID: 16191
		public PguiTextCtrl targetItemNameText;

		// Token: 0x04003F40 RID: 16192
		public PguiTextCtrl useItemNumText;

		// Token: 0x04003F41 RID: 16193
		public PguiTextCtrl gainItemNumText;

		// Token: 0x04003F42 RID: 16194
		public PguiTextCtrl executeCountText;

		// Token: 0x04003F43 RID: 16195
		public PguiTextCtrl excText;

		// Token: 0x04003F44 RID: 16196
		public Slider slider;

		// Token: 0x04003F45 RID: 16197
		public GameObject sliderObj;

		// Token: 0x04003F46 RID: 16198
		public SelCharaGrowItemExchangeWindowCtrl.ExchangeItemUseNumWindow.IconItemPack sourceIconItemPack = new SelCharaGrowItemExchangeWindowCtrl.ExchangeItemUseNumWindow.IconItemPack();

		// Token: 0x04003F47 RID: 16199
		public SelCharaGrowItemExchangeWindowCtrl.ExchangeItemUseNumWindow.IconItemPack targetIconItemPack = new SelCharaGrowItemExchangeWindowCtrl.ExchangeItemUseNumWindow.IconItemPack();

		// Token: 0x04003F48 RID: 16200
		public PguiTextCtrl exchangeRatioText;

		// Token: 0x04003F49 RID: 16201
		public PguiRawImageCtrl sourceItemTex;

		// Token: 0x04003F4A RID: 16202
		public PguiRawImageCtrl targetItemTex;

		// Token: 0x0200115E RID: 4446
		public class IconItemPack
		{
			// Token: 0x060055FB RID: 22011 RVA: 0x0025076D File Offset: 0x0024E96D
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

			// Token: 0x04005F73 RID: 24435
			public IconItemCtrl iconItemCtrl;

			// Token: 0x04005F74 RID: 24436
			public PguiTextCtrl textCtrl;
		}
	}
}
