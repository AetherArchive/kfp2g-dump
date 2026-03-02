using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.HttpRequest.Protocol;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020001BC RID: 444
public class SelMonthlyPackWindowCtrl : MonoBehaviour
{
	// Token: 0x06001E77 RID: 7799 RVA: 0x0017AF34 File Offset: 0x00179134
	public void Init()
	{
		GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("SelCmn/GUI/Prefab/GUI_CmnShop_Pass_Window"), base.transform);
		this.window = new SelMonthlyPackWindowCtrl.Window(gameObject.transform);
		this.window.owCtrl.SetupByMonthlyPack(new PguiOpenWindowCtrl.Callback(this.OnClickOwButton));
		this.window.Btn_Law.AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			CanvasManager.HdlWebViewWindowCtrl.Open("torihiki/index.html");
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.window.Btn_Offer.AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			PrjUtil.OpenOfferWallWebview();
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.window.Btn_Gorgeous.AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			this.OnClickBuy(this.gorgeousPack, this.gorgeousPurchase);
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.window.Btn_Great.AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			this.OnClickBuy(this.greatPack, this.greatPurchase);
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.window.Btn_Small.AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			this.OnClickBuy(this.smallPack, this.smallPurchase);
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.window.Btn_First.AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			this.OnClickBuy(this.firstPack, this.firstPurchase);
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.window.Btn_NewGorgeous.AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			this.OnClickBuy(this.newGorgeousPack, this.newGorgeousPurchase);
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.window.Btn_NewGreat.AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			this.OnClickBuy(this.newGreatPack, this.newGreatPurchase);
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.window.Btn_NewSmall.AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			this.OnClickBuy(this.newSmallPack, this.newSmallPurchase);
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.window.Btn_NewFirst.AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			this.OnClickBuy(this.newFirstPack, this.newFirstPurchase);
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.window.Btn_GorgeousInfo.AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			this.OnClickInfo(this.gorgeousPack);
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.window.Btn_GreatInfo.AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			this.OnClickInfo(this.greatPack);
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.window.Btn_SmallInfo.AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			this.OnClickInfo(this.smallPack);
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.window.Btn_FirstInfo.AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			this.OnClickInfo(this.firstPack);
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.window.Btn_NewGorgeousInfo.AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			this.OnClickInfo(this.newGorgeousPack);
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.window.Btn_NewGreatInfo.AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			this.OnClickInfo(this.newGreatPack);
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.window.Btn_NewSmallInfo.AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			this.OnClickInfo(this.newSmallPack);
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.window.Btn_NewFirstInfo.AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			this.OnClickInfo(this.newFirstPack);
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.window.Num_Own.text = "";
		this.requestStatus = SelMonthlyPackWindowCtrl.Status.NONE;
		this.currentStatus = SelMonthlyPackWindowCtrl.Status.NONE;
		this.isActiveWindow = false;
		this.nowPack = null;
		this.gorgeousPack = null;
		this.greatPack = null;
		this.smallPack = null;
		this.firstPack = null;
		this.buyPack = null;
		this.gorgeousPurchase = null;
		this.greatPurchase = null;
		this.buyPurchase = null;
	}

	// Token: 0x06001E78 RID: 7800 RVA: 0x0017B224 File Offset: 0x00179424
	public void Setup()
	{
		this.isActiveWindow = true;
		this.requestStatus = SelMonthlyPackWindowCtrl.Status.SETUP;
		this.nowPack = null;
		this.gorgeousPack = null;
		this.greatPack = null;
		this.smallPack = null;
		this.firstPack = null;
		this.buyPack = null;
		this.gorgeousPurchase = null;
		this.greatPurchase = null;
		this.buyPurchase = null;
		this.window.Btn_Offer.gameObject.SetActive(false);
		if (DataManager.DmServerMst.IsEnableNoahWeb())
		{
			this.window.Btn_Offer.gameObject.SetActive(true);
		}
	}

	// Token: 0x06001E79 RID: 7801 RVA: 0x0017B2B6 File Offset: 0x001794B6
	public bool IsActiveWindow()
	{
		return this.isActiveWindow;
	}

	// Token: 0x06001E7A RID: 7802 RVA: 0x0017B2C0 File Offset: 0x001794C0
	private int countValidPack()
	{
		int num = 0;
		if (this.gorgeousPack != null)
		{
			num++;
		}
		if (this.greatPack != null)
		{
			num++;
		}
		if (this.smallPack != null)
		{
			num++;
		}
		if (this.firstPack != null)
		{
			num++;
		}
		if (this.newGorgeousPack != null)
		{
			num++;
		}
		if (this.newGreatPack != null)
		{
			num++;
		}
		if (this.newSmallPack != null)
		{
			num++;
		}
		if (this.newFirstPack != null)
		{
			num++;
		}
		return num;
	}

	// Token: 0x06001E7B RID: 7803 RVA: 0x0017B330 File Offset: 0x00179530
	private void Update()
	{
		if (this.requestStatus != this.currentStatus)
		{
			this.currentStatus = this.requestStatus;
			if (this.currentStatus == SelMonthlyPackWindowCtrl.Status.SETUP)
			{
				this.setupSequence = this.SetupSequence();
			}
			else if (this.currentStatus == SelMonthlyPackWindowCtrl.Status.AGE_AUTHENTICATION)
			{
				this.ageAuthenticationSequence = this.AgeAuthenticationSequence();
			}
			else if (this.currentStatus == SelMonthlyPackWindowCtrl.Status.BUY)
			{
				this.monthlyPackSequence = this.MonthlyPackSequence();
			}
			else if (this.currentStatus == SelMonthlyPackWindowCtrl.Status.CLOSE)
			{
				this.window.owCtrl.ForceClose();
				this.isActiveWindow = false;
			}
		}
		if (this.setupSequence != null && !this.setupSequence.MoveNext())
		{
			this.setupSequence = null;
		}
		if (this.ageAuthenticationSequence != null && !this.ageAuthenticationSequence.MoveNext())
		{
			this.ageAuthenticationSequence = null;
		}
		if (this.monthlyPackSequence != null && !this.monthlyPackSequence.MoveNext())
		{
			this.monthlyPackSequence = null;
		}
		if (DataManager.IsFinishCreateByMst())
		{
			this.window.Num_Own.text = DataManager.DmItem.GetUserItemData(30100).num.ToString();
		}
	}

	// Token: 0x06001E7C RID: 7804 RVA: 0x0017B444 File Offset: 0x00179644
	private static int IsAlreadyBuy()
	{
		DateTime dateTime = new DateTime(DataManager.DmMonthlyPack.nowPackData.EndDatetime.Year, DataManager.DmMonthlyPack.nowPackData.EndDatetime.Month, DataManager.DmMonthlyPack.nowPackData.EndDatetime.Day);
		DateTime dateTime2 = new DateTime(DataManager.DmMonthlyPack.nextPackData.EndDatetime.Year, DataManager.DmMonthlyPack.nextPackData.EndDatetime.Month, DataManager.DmMonthlyPack.nextPackData.EndDatetime.Day);
		DateTime dateTime3 = new DateTime(TimeManager.Now.Year, TimeManager.Now.Month, TimeManager.Now.Day);
		TimeSpan timeSpan = dateTime - dateTime3;
		int num = ((DataManager.DmMonthlyPack.purchaseMonthlypackMessageDataList.Count > 0) ? DataManager.DmMonthlyPack.purchaseMonthlypackMessageDataList[0].ReminderDay : 5);
		if ((DataManager.DmMonthlyPack.nowPackData.MonthlypackData != null && timeSpan.Days > num) || (DataManager.DmMonthlyPack.nextPackData.MonthlypackData != null && dateTime2 >= dateTime3))
		{
			return num;
		}
		return 0;
	}

	// Token: 0x06001E7D RID: 7805 RVA: 0x0017B588 File Offset: 0x00179788
	private IEnumerator SetupSequence()
	{
		this.nowPack = DataManager.DmMonthlyPack.nowPackData.MonthlypackData;
		int num = SelMonthlyPackWindowCtrl.IsAlreadyBuy();
		if (num > 0)
		{
			CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("月間パスポート"), PrjUtil.MakeMessage("すでに月間パスポートを購入されています。\n期限が切れる" + num.ToString() + "日前までは\n追加購入できません。\n有効期限についてはホームの\nアイコンからご確認ください。"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, null, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			do
			{
				yield return null;
			}
			while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
			this.requestStatus = SelMonthlyPackWindowCtrl.Status.NONE;
			this.isActiveWindow = false;
			yield break;
		}
		this.gorgeousPack = DataManager.DmMonthlyPack.purchaseMonthlypackDataList.Find((DataManagerMonthlyPack.PurchaseMonthlypackData itm) => itm.PackId == 1);
		this.greatPack = DataManager.DmMonthlyPack.purchaseMonthlypackDataList.Find((DataManagerMonthlyPack.PurchaseMonthlypackData itm) => itm.PackId == 2);
		this.smallPack = DataManager.DmMonthlyPack.purchaseMonthlypackDataList.Find((DataManagerMonthlyPack.PurchaseMonthlypackData itm) => itm.PackId == 3);
		this.firstPack = DataManager.DmMonthlyPack.purchaseMonthlypackDataList.Find((DataManagerMonthlyPack.PurchaseMonthlypackData itm) => itm.PackId == 4);
		this.newGorgeousPack = DataManager.DmMonthlyPack.purchaseMonthlypackDataList.Find((DataManagerMonthlyPack.PurchaseMonthlypackData itm) => itm.PackId == 5);
		this.newGreatPack = DataManager.DmMonthlyPack.purchaseMonthlypackDataList.Find((DataManagerMonthlyPack.PurchaseMonthlypackData itm) => itm.PackId == 6);
		this.newSmallPack = DataManager.DmMonthlyPack.purchaseMonthlypackDataList.Find((DataManagerMonthlyPack.PurchaseMonthlypackData itm) => itm.PackId == 7);
		this.newFirstPack = DataManager.DmMonthlyPack.purchaseMonthlypackDataList.Find((DataManagerMonthlyPack.PurchaseMonthlypackData itm) => itm.PackId == 8);
		HomeCheckResult homeCheckResult = DataManager.DmHome.GetHomeCheckResult();
		Sealed sealData = ((homeCheckResult != null) ? homeCheckResult.sealedData : null) ?? new Sealed();
		if (sealData.stone_buy == 1)
		{
			this.gorgeousPack = null;
			this.greatPack = null;
			this.smallPack = null;
			this.firstPack = null;
			this.newGorgeousPack = null;
			this.newGreatPack = null;
			this.newSmallPack = null;
			this.newFirstPack = null;
		}
		if (this.nowPack != null)
		{
			this.firstPack = null;
			this.newFirstPack = null;
		}
		if (this.gorgeousPack != null || this.greatPack != null || this.smallPack != null || this.firstPack != null || this.newGorgeousPack != null || this.newGreatPack != null || this.newSmallPack != null || this.newFirstPack != null)
		{
			DataManager.DmPurchase.SetupProduct();
			while (!DataManager.DmPurchase.IsFinishSetupProduct)
			{
				yield return null;
			}
			this.gorgeousPurchase = ((this.gorgeousPack == null) ? null : DataManager.DmPurchase.GetPurchaseProductList().Find((PurchaseProductOne itm) => itm.MonthlyPackId == this.gorgeousPack.PackId));
			this.greatPurchase = ((this.greatPack == null) ? null : DataManager.DmPurchase.GetPurchaseProductList().Find((PurchaseProductOne itm) => itm.MonthlyPackId == this.greatPack.PackId));
			this.smallPurchase = ((this.smallPack == null) ? null : DataManager.DmPurchase.GetPurchaseProductList().Find((PurchaseProductOne itm) => itm.MonthlyPackId == this.smallPack.PackId));
			this.firstPurchase = ((this.firstPack == null) ? null : DataManager.DmPurchase.GetPurchaseProductList().Find((PurchaseProductOne itm) => itm.MonthlyPackId == this.firstPack.PackId));
			this.newGorgeousPurchase = ((this.newGorgeousPack == null) ? null : DataManager.DmPurchase.GetPurchaseProductList().Find((PurchaseProductOne itm) => itm.MonthlyPackId == this.newGorgeousPack.PackId));
			this.newGreatPurchase = ((this.newGreatPack == null) ? null : DataManager.DmPurchase.GetPurchaseProductList().Find((PurchaseProductOne itm) => itm.MonthlyPackId == this.newGreatPack.PackId));
			this.newSmallPurchase = ((this.newSmallPack == null) ? null : DataManager.DmPurchase.GetPurchaseProductList().Find((PurchaseProductOne itm) => itm.MonthlyPackId == this.newSmallPack.PackId));
			this.newFirstPurchase = ((this.newFirstPack == null) ? null : DataManager.DmPurchase.GetPurchaseProductList().Find((PurchaseProductOne itm) => itm.MonthlyPackId == this.newFirstPack.PackId));
		}
		if (this.gorgeousPurchase == null && this.greatPurchase == null && this.smallPurchase == null && this.firstPurchase == null && this.newGorgeousPurchase == null && this.newGreatPurchase == null && this.newSmallPurchase == null && this.newFirstPurchase == null)
		{
			string text = sealData.stone_buy_text;
			text = text.Replace("¥n", "\n");
			string url = sealData.stone_buy_url;
			List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>> list = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>();
			list.Clear();
			list.Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, "閉じる"));
			if (!string.IsNullOrEmpty(url))
			{
				list.Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, "お知らせ"));
			}
			string text2 = "";
			if (DataManager.DmPurchase.IsPendingMonthlyPack)
			{
				text2 = "お支払い待ちの月間パスポートがあるため、\n";
			}
			CanvasManager.HdlOpenWindowBasic.Setup("エラー", string.IsNullOrEmpty(text) ? (text2 + "現在月間パスポートは購入できません") : text, list, true, delegate(int idx)
			{
				if (idx == 1)
				{
					CanvasManager.HdlWebViewWindowCtrl.Open(url);
					return false;
				}
				return true;
			}, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			do
			{
				yield return null;
			}
			while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
			this.requestStatus = SelMonthlyPackWindowCtrl.Status.NONE;
			this.isActiveWindow = false;
			yield break;
		}
		this.SetupInfo(this.window.Btn_Gorgeous, this.gorgeousPack, this.gorgeousPurchase);
		this.SetupInfo(this.window.Btn_Great, this.greatPack, this.greatPurchase);
		this.SetupInfo(this.window.Btn_Small, this.smallPack, this.smallPurchase);
		this.SetupInfo(this.window.Btn_First, this.firstPack, this.firstPurchase);
		this.SetupInfo(this.window.Btn_NewGorgeous, this.newGorgeousPack, this.newGorgeousPurchase);
		this.SetupInfo(this.window.Btn_NewGreat, this.newGreatPack, this.newGreatPurchase);
		this.SetupInfo(this.window.Btn_NewSmall, this.newSmallPack, this.newSmallPurchase);
		this.SetupInfo(this.window.Btn_NewFirst, this.newFirstPack, this.newFirstPurchase);
		this.window.scroll.enabled = false;
		this.window.owCtrl.Open();
		do
		{
			this.window.scroll.content.anchoredPosition = Vector2.zero;
			yield return null;
		}
		while (!this.window.owCtrl.FinishedOpen());
		this.window.scroll.enabled = this.countValidPack() >= SelMonthlyPackWindowCtrl.COUNT_TO_SCROLL;
		this.window.scroll.horizontalScrollbar.gameObject.SetActive(this.window.scroll.enabled);
		PrjUtil.SendAppsFlyerLtvId("af_purchase_pass_view", null);
		yield break;
	}

	// Token: 0x06001E7E RID: 7806 RVA: 0x0017B598 File Offset: 0x00179798
	private void SetupInfo(PguiButtonCtrl btn, DataManagerMonthlyPack.PurchaseMonthlypackData pmpd, PurchaseProductOne ppo)
	{
		btn.gameObject.SetActive(ppo != null);
		Transform transform = btn.transform.Find("BaseImage");
		Transform transform2 = transform.Find("Mark_BuyContinue");
		if (transform2 != null)
		{
			int num = 0;
			if (this.nowPack != null && pmpd != null)
			{
				DateTime dateTime = new DateTime(TimeManager.Now.Year, TimeManager.Now.Month, TimeManager.Now.Day);
				int days = (new DateTime(DataManager.DmMonthlyPack.nowPackData.EndDatetime.Year, DataManager.DmMonthlyPack.nowPackData.EndDatetime.Month, DataManager.DmMonthlyPack.nowPackData.EndDatetime.Day) - dateTime).Days;
				int num2 = ((DataManager.DmMonthlyPack.purchaseMonthlypackMessageDataList.Count > 0) ? DataManager.DmMonthlyPack.purchaseMonthlypackMessageDataList[0].ContinueLimitDay : 10);
				if (days >= -num2)
				{
					DataManagerMonthlyPack.MonthlypackContinueData monthlypackContinueData = DataManager.DmMonthlyPack.monthlypackContinueDataList.Find((DataManagerMonthlyPack.MonthlypackContinueData mst) => mst.PrevMonthlyPackId == this.nowPack.PackId && mst.NextMonthlyPackId == pmpd.PackId);
					if (monthlypackContinueData != null)
					{
						num = monthlypackContinueData.AddItemNum;
					}
				}
			}
			transform2.gameObject.SetActive(num > 0);
			transform2.Find("Txt_Buff").GetComponent<PguiTextCtrl>().text = num.ToString() + "<size=16>つ</size>\n<size=20>獲得</size>";
		}
		transform.Find("Txt_Kira").GetComponent<PguiTextCtrl>().text = ((ppo == null) ? "" : ("<size=20>有償キラキラ</size> " + ppo.chargeNum.ToString() + "個"));
		transform.Find("Txt_Money").GetComponent<PguiTextCtrl>().text = ((ppo == null) ? "" : (ppo.price.ToString() + PrjUtil.MakeMessage("pt")));
	}

	// Token: 0x06001E7F RID: 7807 RVA: 0x0017B796 File Offset: 0x00179996
	private IEnumerator AgeAuthenticationSequence()
	{
		IEnumerator func = DataManager.DmPurchase.RequestSolutionAgeAuthentic();
		while (func.MoveNext())
		{
			yield return null;
		}
		this.requestStatus = SelMonthlyPackWindowCtrl.Status.NONE;
		yield break;
	}

	// Token: 0x06001E80 RID: 7808 RVA: 0x0017B7A5 File Offset: 0x001799A5
	private IEnumerator MonthlyPackSequence()
	{
		int num = SelMonthlyPackWindowCtrl.IsAlreadyBuy();
		if (num > 0)
		{
			CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("月間パスポート"), PrjUtil.MakeMessage("すでに月間パスポートを購入されています。\n期限が切れる" + num.ToString() + "日前までは\n追加購入できません。\n有効期限についてはホームの\nアイコンからご確認ください。"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, null, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			do
			{
				yield return null;
			}
			while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
			this.requestStatus = SelMonthlyPackWindowCtrl.Status.CLOSE;
			yield break;
		}
		string text = "";
		string text2 = "\n※『" + this.buyPack.PackName + "』を購入した場合、\nほかのパスポートを重複購入することはできません。";
		if (this.firstPack != null)
		{
			text = text + "『" + this.firstPack.PackName + "』";
		}
		if (this.newFirstPack != null)
		{
			text = text + "『" + this.newFirstPack.PackName + "』";
		}
		if (!string.IsNullOrEmpty(text))
		{
			text2 = text2 + "\nまた、" + text + "は購入できなくなります。";
		}
		int idx = -1;
		CanvasManager.HdlMonthlyConfirmWindowCtrl.Setup(PrjUtil.MakeMessage("確認"), PrjUtil.MakeMessage("『" + this.buyPack.PackName + "』を購入しますか？" + text2), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, delegate(int index)
		{
			idx = index;
			return true;
		}, null, false);
		Transform transform = CanvasManager.HdlMonthlyConfirmWindowCtrl.transform.Find("Base/Window/PurchaseConfirmButton");
		if (transform != null)
		{
			PurchaseProductOne purchaseProductMst = DataManager.DmPurchase.GetPurchaseProductList().Find((PurchaseProductOne itm) => itm.MonthlyPackId == this.buyPack.PackId);
			PguiButtonCtrl component = transform.GetComponent<PguiButtonCtrl>();
			if (component != null)
			{
				component.AddOnClickListener(delegate(PguiButtonCtrl btn)
				{
					CanvasManager.HdlPurchaseConfirmWindow.Initialize(this.buyPack.PackName ?? "", "", purchaseProductMst.price, null, PurchaseConfirmWindow.TEMP_MONTHLY_PROVIDE, false);
				}, PguiButtonCtrl.SoundType.DEFAULT);
			}
		}
		CanvasManager.HdlMonthlyConfirmWindowCtrl.Open();
		do
		{
			yield return null;
		}
		while (!CanvasManager.HdlMonthlyConfirmWindowCtrl.FinishedClose());
		if (idx != 1)
		{
			this.requestStatus = SelMonthlyPackWindowCtrl.Status.NONE;
			yield break;
		}
		DataManager.DmPurchase.SolutionPurchase(this.buyPurchase.productId, this.buyPurchase.productIdString);
		while (!DataManager.DmPurchase.IsFinishPurchase)
		{
			yield return null;
		}
		DataManagerPurchase.PurchaseResult result = DataManager.DmPurchase.GetPurchaseResult();
		while (result.productIdString != this.buyPurchase.productIdString && result.status != DataManagerPurchase.PurchaseResult.Status.FAILURE_COMMON)
		{
			yield return null;
		}
		result.isTargetProduct = true;
		if (result.status == DataManagerPurchase.PurchaseResult.Status.SUCCESS)
		{
			CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("購入確認"), PrjUtil.MakeMessage("購入しました"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, null, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			foreach (UnityAction unityAction in this.onSuccessPurchaseCbList)
			{
				unityAction();
			}
			this.requestStatus = SelMonthlyPackWindowCtrl.Status.CLOSE;
		}
		else if (result.status == DataManagerPurchase.PurchaseResult.Status.FAILURE_LIMIT_AGE)
		{
			CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("年齢認証エラー"), PrjUtil.MakeMessage("お客様の年齢における当月のご利用金額が\n限度額に達している為、ご利用いただけません\n1か月あたりの限度額は\n13歳未満が5,000円(pt)\n20歳未満が20,000円(pt)です"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, null, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			this.requestStatus = SelMonthlyPackWindowCtrl.Status.CLOSE;
		}
		else if (result.status == DataManagerPurchase.PurchaseResult.Status.FAILURE_PENDING_PURCHASE)
		{
			SelMonthlyPackWindowCtrl.<>c__DisplayClass39_2 CS$<>8__locals3 = new SelMonthlyPackWindowCtrl.<>c__DisplayClass39_2();
			CS$<>8__locals3.isResultWindowFinish = false;
			CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("エラー"), PrjUtil.MakeMessage("お支払いを確認できませんでした。\nお支払い完了後、キラキラ&パック購入画面へ\n再度アクセスしてください。"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int index)
			{
				CS$<>8__locals3.isResultWindowFinish = true;
				return true;
			}, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			this.requestStatus = SelMonthlyPackWindowCtrl.Status.CLOSE;
			while (!CS$<>8__locals3.isResultWindowFinish)
			{
				yield return null;
			}
			CS$<>8__locals3 = null;
		}
		else
		{
			CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("購入確認"), PrjUtil.MakeMessage("購入がキャンセルされました"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, null, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			this.requestStatus = SelMonthlyPackWindowCtrl.Status.CLOSE;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001E81 RID: 7809 RVA: 0x0017B7B4 File Offset: 0x001799B4
	private bool OnClickOwButton(int index)
	{
		this.requestStatus = SelMonthlyPackWindowCtrl.Status.NONE;
		this.isActiveWindow = false;
		return true;
	}

	// Token: 0x06001E82 RID: 7810 RVA: 0x0017B7C8 File Offset: 0x001799C8
	private void OnClickBuy(DataManagerMonthlyPack.PurchaseMonthlypackData pack, PurchaseProductOne purchase)
	{
		if (this.setupSequence != null || this.ageAuthenticationSequence != null || this.monthlyPackSequence != null)
		{
			return;
		}
		if (pack == null || purchase == null)
		{
			this.requestStatus = SelMonthlyPackWindowCtrl.Status.CLOSE;
			return;
		}
		if (purchase.price > 0 && !DataManager.DmPurchase.IsEnableBirthday)
		{
			this.requestStatus = SelMonthlyPackWindowCtrl.Status.AGE_AUTHENTICATION;
			return;
		}
		this.buyPack = pack;
		this.buyPurchase = purchase;
		this.requestStatus = SelMonthlyPackWindowCtrl.Status.BUY;
	}

	// Token: 0x06001E83 RID: 7811 RVA: 0x0017B82E File Offset: 0x00179A2E
	private void OnClickInfo(DataManagerMonthlyPack.PurchaseMonthlypackData pack)
	{
		if (this.setupSequence != null || this.ageAuthenticationSequence != null || this.monthlyPackSequence != null)
		{
			return;
		}
		if (this.isActiveWindow && pack != null)
		{
			CanvasManager.HdlWebViewWindowCtrl.Open(pack.WebviewLink);
		}
	}

	// Token: 0x06001E84 RID: 7812 RVA: 0x0017B864 File Offset: 0x00179A64
	public void AddOnSuccessPurchaseListener(UnityAction cb)
	{
		this.onSuccessPurchaseCbList.Add(cb);
	}

	// Token: 0x06001E85 RID: 7813 RVA: 0x0017B872 File Offset: 0x00179A72
	public void RemoveOnSuccessPurchaseListener(UnityAction cb)
	{
		this.onSuccessPurchaseCbList.Remove(cb);
	}

	// Token: 0x04001641 RID: 5697
	private SelMonthlyPackWindowCtrl.Window window;

	// Token: 0x04001642 RID: 5698
	private static readonly int COUNT_TO_SCROLL = 4;

	// Token: 0x04001643 RID: 5699
	private SelMonthlyPackWindowCtrl.Status requestStatus;

	// Token: 0x04001644 RID: 5700
	private SelMonthlyPackWindowCtrl.Status currentStatus;

	// Token: 0x04001645 RID: 5701
	private bool isActiveWindow;

	// Token: 0x04001646 RID: 5702
	private DataManagerMonthlyPack.PurchaseMonthlypackData nowPack;

	// Token: 0x04001647 RID: 5703
	private DataManagerMonthlyPack.PurchaseMonthlypackData gorgeousPack;

	// Token: 0x04001648 RID: 5704
	private DataManagerMonthlyPack.PurchaseMonthlypackData greatPack;

	// Token: 0x04001649 RID: 5705
	private DataManagerMonthlyPack.PurchaseMonthlypackData smallPack;

	// Token: 0x0400164A RID: 5706
	private DataManagerMonthlyPack.PurchaseMonthlypackData firstPack;

	// Token: 0x0400164B RID: 5707
	private DataManagerMonthlyPack.PurchaseMonthlypackData buyPack;

	// Token: 0x0400164C RID: 5708
	private PurchaseProductOne gorgeousPurchase;

	// Token: 0x0400164D RID: 5709
	private PurchaseProductOne greatPurchase;

	// Token: 0x0400164E RID: 5710
	private PurchaseProductOne smallPurchase;

	// Token: 0x0400164F RID: 5711
	private PurchaseProductOne firstPurchase;

	// Token: 0x04001650 RID: 5712
	private PurchaseProductOne buyPurchase;

	// Token: 0x04001651 RID: 5713
	private DataManagerMonthlyPack.PurchaseMonthlypackData newGorgeousPack;

	// Token: 0x04001652 RID: 5714
	private DataManagerMonthlyPack.PurchaseMonthlypackData newGreatPack;

	// Token: 0x04001653 RID: 5715
	private DataManagerMonthlyPack.PurchaseMonthlypackData newSmallPack;

	// Token: 0x04001654 RID: 5716
	private DataManagerMonthlyPack.PurchaseMonthlypackData newFirstPack;

	// Token: 0x04001655 RID: 5717
	private PurchaseProductOne newGorgeousPurchase;

	// Token: 0x04001656 RID: 5718
	private PurchaseProductOne newGreatPurchase;

	// Token: 0x04001657 RID: 5719
	private PurchaseProductOne newSmallPurchase;

	// Token: 0x04001658 RID: 5720
	private PurchaseProductOne newFirstPurchase;

	// Token: 0x04001659 RID: 5721
	private List<UnityAction> onSuccessPurchaseCbList = new List<UnityAction>();

	// Token: 0x0400165A RID: 5722
	private IEnumerator setupSequence;

	// Token: 0x0400165B RID: 5723
	private IEnumerator ageAuthenticationSequence;

	// Token: 0x0400165C RID: 5724
	private IEnumerator monthlyPackSequence;

	// Token: 0x02000FB1 RID: 4017
	public class Window
	{
		// Token: 0x0600509C RID: 20636 RVA: 0x0024265C File Offset: 0x0024085C
		public Window(Transform baseTr)
		{
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.Btn_Law = baseTr.Find("Base/Window/Btn_Law").GetComponent<PguiButtonCtrl>();
			this.Num_Own = baseTr.Find("Base/Window/ItemOwnBase/Num_Own").GetComponent<PguiTextCtrl>();
			this.Btn_Gorgeous = baseTr.Find("Base/Window/InBase/ScrollView/Viewport/Content/Btn_Shop01").GetComponent<PguiButtonCtrl>();
			this.Btn_Great = baseTr.Find("Base/Window/InBase/ScrollView/Viewport/Content/Btn_Shop02").GetComponent<PguiButtonCtrl>();
			this.Btn_Small = baseTr.Find("Base/Window/InBase/ScrollView/Viewport/Content/Btn_Shop03").GetComponent<PguiButtonCtrl>();
			this.Btn_First = baseTr.Find("Base/Window/InBase/ScrollView/Viewport/Content/Btn_Shop04").GetComponent<PguiButtonCtrl>();
			this.Btn_NewGorgeous = baseTr.Find("Base/Window/InBase/ScrollView/Viewport/Content/Btn_Shop05").GetComponent<PguiButtonCtrl>();
			this.Btn_NewGreat = baseTr.Find("Base/Window/InBase/ScrollView/Viewport/Content/Btn_Shop06").GetComponent<PguiButtonCtrl>();
			this.Btn_NewSmall = baseTr.Find("Base/Window/InBase/ScrollView/Viewport/Content/Btn_Shop07").GetComponent<PguiButtonCtrl>();
			this.Btn_NewFirst = baseTr.Find("Base/Window/InBase/ScrollView/Viewport/Content/Btn_Shop08").GetComponent<PguiButtonCtrl>();
			this.Btn_GorgeousInfo = this.Btn_Gorgeous.transform.Find("BaseImage/Cmn_Btn_Info_Circle").GetComponent<PguiButtonCtrl>();
			this.Btn_GreatInfo = this.Btn_Great.transform.Find("BaseImage/Cmn_Btn_Info_Circle").GetComponent<PguiButtonCtrl>();
			this.Btn_SmallInfo = this.Btn_Small.transform.Find("BaseImage/Cmn_Btn_Info_Circle").GetComponent<PguiButtonCtrl>();
			this.Btn_FirstInfo = this.Btn_First.transform.Find("BaseImage/Cmn_Btn_Info_Circle").GetComponent<PguiButtonCtrl>();
			this.Btn_NewGorgeousInfo = this.Btn_NewGorgeous.transform.Find("BaseImage/Cmn_Btn_Info_Circle").GetComponent<PguiButtonCtrl>();
			this.Btn_NewGreatInfo = this.Btn_NewGreat.transform.Find("BaseImage/Cmn_Btn_Info_Circle").GetComponent<PguiButtonCtrl>();
			this.Btn_NewSmallInfo = this.Btn_NewSmall.transform.Find("BaseImage/Cmn_Btn_Info_Circle").GetComponent<PguiButtonCtrl>();
			this.Btn_NewFirstInfo = this.Btn_NewFirst.transform.Find("BaseImage/Cmn_Btn_Info_Circle").GetComponent<PguiButtonCtrl>();
			this.Btn_Offer = baseTr.Find("Base/Window/Btn_Offer").GetComponent<PguiButtonCtrl>();
			this.scroll = baseTr.Find("Base/Window/InBase/ScrollView").GetComponent<ScrollRect>();
			this.scroll.scrollSensitivity = ScrollParamDefine.MonthlyPackWindow;
		}

		// Token: 0x04005883 RID: 22659
		public PguiOpenWindowCtrl owCtrl;

		// Token: 0x04005884 RID: 22660
		public PguiButtonCtrl Btn_Law;

		// Token: 0x04005885 RID: 22661
		public PguiButtonCtrl Btn_Offer;

		// Token: 0x04005886 RID: 22662
		public PguiTextCtrl Num_Own;

		// Token: 0x04005887 RID: 22663
		public PguiButtonCtrl Btn_Gorgeous;

		// Token: 0x04005888 RID: 22664
		public PguiButtonCtrl Btn_Great;

		// Token: 0x04005889 RID: 22665
		public PguiButtonCtrl Btn_Small;

		// Token: 0x0400588A RID: 22666
		public PguiButtonCtrl Btn_First;

		// Token: 0x0400588B RID: 22667
		public PguiButtonCtrl Btn_GorgeousInfo;

		// Token: 0x0400588C RID: 22668
		public PguiButtonCtrl Btn_GreatInfo;

		// Token: 0x0400588D RID: 22669
		public PguiButtonCtrl Btn_SmallInfo;

		// Token: 0x0400588E RID: 22670
		public PguiButtonCtrl Btn_FirstInfo;

		// Token: 0x0400588F RID: 22671
		public ScrollRect scroll;

		// Token: 0x04005890 RID: 22672
		public PguiButtonCtrl Btn_NewGorgeous;

		// Token: 0x04005891 RID: 22673
		public PguiButtonCtrl Btn_NewGreat;

		// Token: 0x04005892 RID: 22674
		public PguiButtonCtrl Btn_NewSmall;

		// Token: 0x04005893 RID: 22675
		public PguiButtonCtrl Btn_NewFirst;

		// Token: 0x04005894 RID: 22676
		public PguiButtonCtrl Btn_NewGorgeousInfo;

		// Token: 0x04005895 RID: 22677
		public PguiButtonCtrl Btn_NewGreatInfo;

		// Token: 0x04005896 RID: 22678
		public PguiButtonCtrl Btn_NewSmallInfo;

		// Token: 0x04005897 RID: 22679
		public PguiButtonCtrl Btn_NewFirstInfo;
	}

	// Token: 0x02000FB2 RID: 4018
	public enum Status
	{
		// Token: 0x04005899 RID: 22681
		NONE,
		// Token: 0x0400589A RID: 22682
		CLOSE,
		// Token: 0x0400589B RID: 22683
		SETUP,
		// Token: 0x0400589C RID: 22684
		AGE_AUTHENTICATION,
		// Token: 0x0400589D RID: 22685
		BUY
	}
}
