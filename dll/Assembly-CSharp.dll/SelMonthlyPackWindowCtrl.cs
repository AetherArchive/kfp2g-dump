using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.HttpRequest.Protocol;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelMonthlyPackWindowCtrl : MonoBehaviour
{
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

	public bool IsActiveWindow()
	{
		return this.isActiveWindow;
	}

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

	private bool OnClickOwButton(int index)
	{
		this.requestStatus = SelMonthlyPackWindowCtrl.Status.NONE;
		this.isActiveWindow = false;
		return true;
	}

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

	public void AddOnSuccessPurchaseListener(UnityAction cb)
	{
		this.onSuccessPurchaseCbList.Add(cb);
	}

	public void RemoveOnSuccessPurchaseListener(UnityAction cb)
	{
		this.onSuccessPurchaseCbList.Remove(cb);
	}

	private SelMonthlyPackWindowCtrl.Window window;

	private static readonly int COUNT_TO_SCROLL = 4;

	private SelMonthlyPackWindowCtrl.Status requestStatus;

	private SelMonthlyPackWindowCtrl.Status currentStatus;

	private bool isActiveWindow;

	private DataManagerMonthlyPack.PurchaseMonthlypackData nowPack;

	private DataManagerMonthlyPack.PurchaseMonthlypackData gorgeousPack;

	private DataManagerMonthlyPack.PurchaseMonthlypackData greatPack;

	private DataManagerMonthlyPack.PurchaseMonthlypackData smallPack;

	private DataManagerMonthlyPack.PurchaseMonthlypackData firstPack;

	private DataManagerMonthlyPack.PurchaseMonthlypackData buyPack;

	private PurchaseProductOne gorgeousPurchase;

	private PurchaseProductOne greatPurchase;

	private PurchaseProductOne smallPurchase;

	private PurchaseProductOne firstPurchase;

	private PurchaseProductOne buyPurchase;

	private DataManagerMonthlyPack.PurchaseMonthlypackData newGorgeousPack;

	private DataManagerMonthlyPack.PurchaseMonthlypackData newGreatPack;

	private DataManagerMonthlyPack.PurchaseMonthlypackData newSmallPack;

	private DataManagerMonthlyPack.PurchaseMonthlypackData newFirstPack;

	private PurchaseProductOne newGorgeousPurchase;

	private PurchaseProductOne newGreatPurchase;

	private PurchaseProductOne newSmallPurchase;

	private PurchaseProductOne newFirstPurchase;

	private List<UnityAction> onSuccessPurchaseCbList = new List<UnityAction>();

	private IEnumerator setupSequence;

	private IEnumerator ageAuthenticationSequence;

	private IEnumerator monthlyPackSequence;

	public class Window
	{
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

		public PguiOpenWindowCtrl owCtrl;

		public PguiButtonCtrl Btn_Law;

		public PguiButtonCtrl Btn_Offer;

		public PguiTextCtrl Num_Own;

		public PguiButtonCtrl Btn_Gorgeous;

		public PguiButtonCtrl Btn_Great;

		public PguiButtonCtrl Btn_Small;

		public PguiButtonCtrl Btn_First;

		public PguiButtonCtrl Btn_GorgeousInfo;

		public PguiButtonCtrl Btn_GreatInfo;

		public PguiButtonCtrl Btn_SmallInfo;

		public PguiButtonCtrl Btn_FirstInfo;

		public ScrollRect scroll;

		public PguiButtonCtrl Btn_NewGorgeous;

		public PguiButtonCtrl Btn_NewGreat;

		public PguiButtonCtrl Btn_NewSmall;

		public PguiButtonCtrl Btn_NewFirst;

		public PguiButtonCtrl Btn_NewGorgeousInfo;

		public PguiButtonCtrl Btn_NewGreatInfo;

		public PguiButtonCtrl Btn_NewSmallInfo;

		public PguiButtonCtrl Btn_NewFirstInfo;
	}

	public enum Status
	{
		NONE,
		CLOSE,
		SETUP,
		AGE_AUTHENTICATION,
		BUY
	}
}
