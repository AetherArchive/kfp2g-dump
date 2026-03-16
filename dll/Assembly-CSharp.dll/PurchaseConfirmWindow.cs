using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PurchaseConfirmWindow : MonoBehaviour
{
	private void Update()
	{
		if (this.IEWindowMove != null && !this.IEWindowMove.MoveNext())
		{
			this.IEWindowMove = null;
		}
	}

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

	public void Initialize(PurchaseProductOne productOne)
	{
		string text = "";
		if (productOne.sellStartTime != null && productOne.sellEndTime != null && (productOne.sellEndTime.Value - TimeManager.Now).Days < DataManagerPurchase.LimitItemJudgeDays)
		{
			text = productOne.sellStartTime.ToString() + " 〜 " + productOne.sellEndTime.ToString();
		}
		this.Initialize(productOne.bonusItemTitle.Replace("\r", "").Replace("\n", ""), "", productOne.price, text, PurchaseConfirmWindow.TEMP_IMMEDIATE_DELIVERY, false);
	}

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

	public void Open()
	{
		this.OpenWindow(null);
	}

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

	private void RefreshInfo()
	{
	}

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

	public static readonly string TEMP_IMMEDIATE_DELIVERY = "お支払い後直ちに提供 (通信状況により数分程度の遅延が生じることがあります)";

	public static readonly string TEMP_MONTHLY_PROVIDE = "お支払い後直ちに提供、ただし現在有効な月間パスポートがある場合は期間終了直後に提供\n(通信状況により数分程度の遅延が生じることがあります)";

	public static readonly string TEMP_NO_CANCEL = "商品の性質上、申込みの撤回、キャンセル、解除等はできません。";

	public static readonly string TEMP_WINDOW_TITLE = "特定商取引法第12条の6に関する事項";

	public static readonly string TEMP_NA = "該当ありません。";

	public PurchaseConfirmWindow.GUI guiData;

	private IEnumerator IEWindowMove;

	private UnityAction windowCloseEndCb;

	public class GUI
	{
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

		public GameObject baseObj;

		public PguiOpenWindowCtrl owCtrl;

		public PguiTextCtrl Text_Service;

		public PguiTextCtrl Text_Cost;

		public PguiTextCtrl Text_EffectiveDate;

		public PguiTextCtrl Text_ProvidePeriod;

		public PguiTextCtrl Text_IsCancelable;

		public PguiTextCtrl Text_PaymentTiming;
	}

	public class SetupParam
	{
		public SetupParam()
		{
			this.openEndCb = null;
			this.closeEndCb = null;
		}

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

		public UnityAction openEndCb;

		public UnityAction closeEndCb;

		public string serviceName;

		public string serviceAmount;

		public string costName;

		public string costAmount;

		public string effectiveDate;

		public string providePeriod;

		public bool isCancelable;
	}
}
