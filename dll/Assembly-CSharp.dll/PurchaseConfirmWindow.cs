using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020000FC RID: 252
public class PurchaseConfirmWindow : MonoBehaviour
{
	// Token: 0x06000C19 RID: 3097 RVA: 0x00047F6D File Offset: 0x0004616D
	private void Update()
	{
		if (this.IEWindowMove != null && !this.IEWindowMove.MoveNext())
		{
			this.IEWindowMove = null;
		}
	}

	// Token: 0x06000C1A RID: 3098 RVA: 0x00047F8C File Offset: 0x0004618C
	public void Initialize(ShopData.ItemOne itemOne)
	{
		string text = "";
		DateTime startTime = itemOne.startTime;
		DateTime endTime = itemOne.endTime;
		if ((itemOne.endTime - TimeManager.Now).Days < DataManagerPurchase.LimitItemJudgeDays)
		{
			text = itemOne.startTime.ToString() + " 〜 " + itemOne.endTime.ToString();
		}
		ItemData itemData = new ItemData(itemOne.itemId, itemOne.itemNum);
		string text2 = ((itemData.staticData == null) ? "" : itemData.staticData.GetName());
		ItemData itemData2 = new ItemData(itemOne.priceItemId, itemOne.priceItemNum);
		string text3 = ((itemData2.staticData == null) ? "" : itemData2.staticData.GetName());
		this.Initialize(text2.Replace("\r", "").Replace("\n", "") + " " + itemOne.itemNum.ToString() + "個", text3, itemOne.priceItemNum, text, PurchaseConfirmWindow.TEMP_IMMEDIATE_DELIVERY, false);
	}

	// Token: 0x06000C1B RID: 3099 RVA: 0x00048098 File Offset: 0x00046298
	public void Initialize(PurchaseProductOne productOne)
	{
		string text = "";
		if (productOne.sellStartTime != null && productOne.sellEndTime != null && (productOne.sellEndTime.Value - TimeManager.Now).Days < DataManagerPurchase.LimitItemJudgeDays)
		{
			text = productOne.sellStartTime.ToString() + " 〜 " + productOne.sellEndTime.ToString();
		}
		this.Initialize(productOne.bonusItemTitle.Replace("\r", "").Replace("\n", ""), "", productOne.price, text, PurchaseConfirmWindow.TEMP_IMMEDIATE_DELIVERY, false);
	}

	// Token: 0x06000C1C RID: 3100 RVA: 0x00048154 File Offset: 0x00046354
	public void Initialize(string serviceName, string costName, int costAmount, string providePeriod, string effectiveDate, bool isCancelable = false)
	{
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, base.transform, true);
		base.transform.SetAsLastSibling();
		this.guiData = new PurchaseConfirmWindow.GUI(base.transform.Find("Window_PurchaseConfirm").transform);
		this.guiData.Text_Service.text = serviceName;
		if (!string.IsNullOrEmpty(costName))
		{
			this.guiData.Text_Cost.text = costName + " " + costAmount.ToString() + "個";
		}
		else
		{
			this.guiData.Text_Cost.text = "￥ " + costAmount.ToString();
		}
		this.guiData.Text_ProvidePeriod.text = (string.IsNullOrEmpty(providePeriod) ? PurchaseConfirmWindow.TEMP_NA : providePeriod);
		this.guiData.Text_EffectiveDate.text = effectiveDate;
		this.guiData.Text_IsCancelable.text = PurchaseConfirmWindow.TEMP_NO_CANCEL;
		this.Open();
	}

	// Token: 0x06000C1D RID: 3101 RVA: 0x0004824B File Offset: 0x0004644B
	public void Open()
	{
		this.OpenWindow(null);
	}

	// Token: 0x06000C1E RID: 3102 RVA: 0x00048254 File Offset: 0x00046454
	private void OpenWindow(UnityAction openEndCb)
	{
		this.RefreshInfo();
		this.guiData.owCtrl.Setup(PurchaseConfirmWindow.TEMP_WINDOW_TITLE, null, null, true, new PguiOpenWindowCtrl.Callback(this.OnClickOwButton), null, false);
		this.guiData.owCtrl.Open();
		if (openEndCb != null)
		{
			openEndCb();
		}
	}

	// Token: 0x06000C1F RID: 3103 RVA: 0x000482A6 File Offset: 0x000464A6
	private void RefreshInfo()
	{
	}

	// Token: 0x06000C20 RID: 3104 RVA: 0x000482A8 File Offset: 0x000464A8
	private bool OnClickOwButton(int index)
	{
		if (-1 == index)
		{
			UnityAction unityAction = this.windowCloseEndCb;
			if (unityAction != null)
			{
				unityAction();
			}
			this.windowCloseEndCb = null;
			return true;
		}
		return false;
	}

	// Token: 0x0400094A RID: 2378
	public static readonly string TEMP_IMMEDIATE_DELIVERY = "お支払い後直ちに提供 (通信状況により数分程度の遅延が生じることがあります)";

	// Token: 0x0400094B RID: 2379
	public static readonly string TEMP_MONTHLY_PROVIDE = "お支払い後直ちに提供、ただし現在有効な月間パスポートがある場合は期間終了直後に提供\n(通信状況により数分程度の遅延が生じることがあります)";

	// Token: 0x0400094C RID: 2380
	public static readonly string TEMP_NO_CANCEL = "商品の性質上、申込みの撤回、キャンセル、解除等はできません。";

	// Token: 0x0400094D RID: 2381
	public static readonly string TEMP_WINDOW_TITLE = "特定商取引法第12条の6に関する事項";

	// Token: 0x0400094E RID: 2382
	public static readonly string TEMP_NA = "該当ありません。";

	// Token: 0x0400094F RID: 2383
	public PurchaseConfirmWindow.GUI guiData;

	// Token: 0x04000950 RID: 2384
	private IEnumerator IEWindowMove;

	// Token: 0x04000951 RID: 2385
	private UnityAction windowCloseEndCb;

	// Token: 0x02000819 RID: 2073
	public class GUI
	{
		// Token: 0x060037FA RID: 14330 RVA: 0x001CA024 File Offset: 0x001C8224
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.Find("Base").gameObject;
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.Text_Service = baseTr.Find("Base/Window/LayoutGroup/Service/Text").GetComponent<PguiTextCtrl>();
			this.Text_Cost = baseTr.Find("Base/Window/LayoutGroup/Cost/Text").GetComponent<PguiTextCtrl>();
			this.Text_EffectiveDate = baseTr.Find("Base/Window/LayoutGroup/EffectiveDate/Text").GetComponent<PguiTextCtrl>();
			this.Text_ProvidePeriod = baseTr.Find("Base/Window/LayoutGroup/ProvidePeriod/Text").GetComponent<PguiTextCtrl>();
			this.Text_IsCancelable = baseTr.Find("Base/Window/LayoutGroup/isCancelable/Text").GetComponent<PguiTextCtrl>();
			this.Text_PaymentTiming = baseTr.Find("Base/Window/LayoutGroup/PaymentTiming/Text").GetComponent<PguiTextCtrl>();
		}

		// Token: 0x0400363D RID: 13885
		public GameObject baseObj;

		// Token: 0x0400363E RID: 13886
		public PguiOpenWindowCtrl owCtrl;

		// Token: 0x0400363F RID: 13887
		public PguiTextCtrl Text_Service;

		// Token: 0x04003640 RID: 13888
		public PguiTextCtrl Text_Cost;

		// Token: 0x04003641 RID: 13889
		public PguiTextCtrl Text_EffectiveDate;

		// Token: 0x04003642 RID: 13890
		public PguiTextCtrl Text_ProvidePeriod;

		// Token: 0x04003643 RID: 13891
		public PguiTextCtrl Text_IsCancelable;

		// Token: 0x04003644 RID: 13892
		public PguiTextCtrl Text_PaymentTiming;
	}

	// Token: 0x0200081A RID: 2074
	public class SetupParam
	{
		// Token: 0x060037FB RID: 14331 RVA: 0x001CA0DD File Offset: 0x001C82DD
		public SetupParam()
		{
			this.openEndCb = null;
			this.closeEndCb = null;
		}

		// Token: 0x060037FC RID: 14332 RVA: 0x001CA0F3 File Offset: 0x001C82F3
		public void Initialize(string serviceName, string serviceAmount, string costName, string costAmount, string effectiveDate, string providePeriod, bool isCancelable)
		{
			this.serviceName = serviceName;
			this.serviceAmount = serviceAmount;
			this.costName = costName;
			this.costAmount = costAmount;
			this.effectiveDate = effectiveDate;
			this.providePeriod = providePeriod;
			this.isCancelable = isCancelable;
		}

		// Token: 0x04003645 RID: 13893
		public UnityAction openEndCb;

		// Token: 0x04003646 RID: 13894
		public UnityAction closeEndCb;

		// Token: 0x04003647 RID: 13895
		public string serviceName;

		// Token: 0x04003648 RID: 13896
		public string serviceAmount;

		// Token: 0x04003649 RID: 13897
		public string costName;

		// Token: 0x0400364A RID: 13898
		public string costAmount;

		// Token: 0x0400364B RID: 13899
		public string effectiveDate;

		// Token: 0x0400364C RID: 13900
		public string providePeriod;

		// Token: 0x0400364D RID: 13901
		public bool isCancelable;
	}
}
