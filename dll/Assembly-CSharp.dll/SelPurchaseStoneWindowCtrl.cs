using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.HttpRequest.Protocol;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x020001BE RID: 446
public class SelPurchaseStoneWindowCtrl : MonoBehaviour
{
	// Token: 0x06001EBF RID: 7871 RVA: 0x0017EA24 File Offset: 0x0017CC24
	public void Init()
	{
		GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("Cmn/GUI/Prefab/GUI_Cmn_BillingWindow"), base.transform);
		this.window = new SelPurchaseStoneWindowCtrl.Window(gameObject.transform);
		this.window.ScrollView.InitForce();
		ReuseScroll scrollView = this.window.ScrollView;
		scrollView.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollView.onStartItem, new Action<int, GameObject>(this.OnStartWindow));
		ReuseScroll scrollView2 = this.window.ScrollView;
		scrollView2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollView2.onUpdateItem, new Action<int, GameObject>(this.OnUpdateWindow));
		this.window.owCtrl.SetupByPurchaseStone(new PguiOpenWindowCtrl.Callback(this.OnClickOwButton));
		this.window.Btn_Law.AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			if (this.isNotifyNewProduct)
			{
				return;
			}
			CanvasManager.HdlWebViewWindowCtrl.Open("torihiki/index.html");
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.window.Btn_Offer.AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			if (this.isNotifyNewProduct)
			{
				return;
			}
			PrjUtil.OpenOfferWallWebview();
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.window.banner.GetComponent<PguiRawImageCtrl>().bannerOnly = SelPurchaseStoneWindowCtrl.Window.BANNER_TEXTURE;
		this.window.banner.AddComponent<PguiTouchTrigger>().AddListener(delegate
		{
			CanvasManager.HdlCmnFeedPageWindowCtrl.transform.SetAsLastSibling();
			CanvasManager.HdlHelpWindowCtrl.Open(true);
		}, null, null, null, null);
		GameObject gameObject2 = Object.Instantiate<GameObject>((GameObject)Resources.Load("prefab/CmnOpenWindow_GetItem"), base.transform);
		this.resultWindow = new GetItemWindowCtrl.GUI(gameObject2.transform);
		this.resultWindow.owCtrl.WindowRectTransform.sizeDelta = new Vector2(780f, 580f);
		this.resultWindow.Icon_item02.SetActive(false);
		Text component = gameObject2.transform.Find("Base/Window/Txt01").GetComponent<Text>();
		component.transform.localPosition = new Vector3(0f, -30f, 0f);
		component.alignment = TextAnchor.MiddleCenter;
		this.resultWindow.Img_item.transform.localPosition = new Vector3(0f, -30f, 0f);
		this.requestStatus = SelPurchaseStoneWindowCtrl.Status.NONE;
		this.currentStatus = SelPurchaseStoneWindowCtrl.Status.NONE;
		this.requestTabIndex = 0;
		this.isNotifyNewProduct = false;
		this.releaseProductIdList = new List<int>();
		this.clearReleaseProductIdList = true;
	}

	// Token: 0x06001EC0 RID: 7872 RVA: 0x0017EC60 File Offset: 0x0017CE60
	public void Setup(PurchaseProductOne.TabType requestOpenTabType = PurchaseProductOne.TabType.Invalid)
	{
		this.isActiveWindow = true;
		this.requestStatus = SelPurchaseStoneWindowCtrl.Status.SETUP;
		this.requestTabIndex = this.GetTabIndex(requestOpenTabType);
		if (this.clearReleaseProductIdList)
		{
			this.releaseProductIdList.Clear();
		}
		else
		{
			this.clearReleaseProductIdList = true;
		}
		this.window.Btn_Offer.gameObject.SetActive(false);
		this.window.Txt_None.SetActive(false);
		if (DataManager.DmServerMst.IsEnableNoahWeb())
		{
			this.window.Btn_Offer.gameObject.SetActive(true);
		}
	}

	// Token: 0x06001EC1 RID: 7873 RVA: 0x0017ECF0 File Offset: 0x0017CEF0
	private int GetTabIndex(PurchaseProductOne.TabType tabType)
	{
		int num = 0;
		switch (tabType)
		{
		case PurchaseProductOne.TabType.Limited:
			num = 1;
			break;
		case PurchaseProductOne.TabType.MonthlyPack:
			num = 2;
			break;
		case PurchaseProductOne.TabType.Kirakira:
			num = 3;
			break;
		case PurchaseProductOne.TabType.Pack:
			num = 4;
			break;
		}
		return num;
	}

	// Token: 0x06001EC2 RID: 7874 RVA: 0x0017ED28 File Offset: 0x0017CF28
	public bool IsActiveWindow()
	{
		return this.isActiveWindow;
	}

	// Token: 0x06001EC3 RID: 7875 RVA: 0x0017ED30 File Offset: 0x0017CF30
	public void AddOnSuccessPurchaseListener(UnityAction cb)
	{
		this.onSuccessPurchaseList.Add(cb);
	}

	// Token: 0x06001EC4 RID: 7876 RVA: 0x0017ED3E File Offset: 0x0017CF3E
	public void RemoveOnSuccessPurchaseListener(UnityAction cb)
	{
		this.onSuccessPurchaseList.Remove(cb);
	}

	// Token: 0x06001EC5 RID: 7877 RVA: 0x0017ED50 File Offset: 0x0017CF50
	private void Update()
	{
		if (this.requestStatus != this.currentStatus)
		{
			this.currentStatus = this.requestStatus;
			if (this.currentStatus == SelPurchaseStoneWindowCtrl.Status.SETUP)
			{
				this.setupSequence = this.SetupSequence(true);
			}
			else if (this.currentStatus == SelPurchaseStoneWindowCtrl.Status.AGE_AUTHENTICATION)
			{
				this.ageAuthenticationSequence = this.AgeAuthenticationSequence();
			}
			else if (this.currentStatus == SelPurchaseStoneWindowCtrl.Status.PURCHASE)
			{
				this.purchaseSequence = this.PurchaseSequence();
			}
			else if (this.currentStatus == SelPurchaseStoneWindowCtrl.Status.ERROR)
			{
				this.requestStatus = SelPurchaseStoneWindowCtrl.Status.NONE;
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
		if (this.purchaseSequence != null && !this.purchaseSequence.MoveNext())
		{
			this.purchaseSequence = null;
		}
		if (this.currentStatus != SelPurchaseStoneWindowCtrl.Status.NONE && DataManager.IsFinishCreateByMst())
		{
			this.window.Num_Own.text = DataManager.DmItem.GetUserItemData(30100).num.ToString();
		}
	}

	// Token: 0x06001EC6 RID: 7878 RVA: 0x0017EE5C File Offset: 0x0017D05C
	private IEnumerator SetupSequence(bool isOpen)
	{
		this.window.ScrollView.Clear();
		if (isOpen)
		{
			this.window.owCtrl.Open();
		}
		DataManager.DmPurchase.SetupProduct();
		while (!DataManager.DmPurchase.IsFinishSetupProduct)
		{
			yield return null;
		}
		this.tabPurchaseProductOneListList = DataManager.DmPurchase.CreateTabPurchaseProductOneListList();
		this.SortReleaseProduct();
		this.isEnableMonthlyPack = false;
		HomeCheckResult homeCheckResult = DataManager.DmHome.GetHomeCheckResult();
		Sealed @sealed = ((homeCheckResult != null) ? homeCheckResult.sealedData : null);
		if (@sealed != null && 1 == @sealed.stone_buy)
		{
			this.isEnableMonthlyPack = false;
			string text = @sealed.stone_buy_text;
			text = text.Replace("¥n", "\n");
			string url = @sealed.stone_buy_url;
			List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>> list = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>();
			list.Clear();
			list.Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, "閉じる"));
			if (!string.IsNullOrEmpty(url))
			{
				list.Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, "お知らせ"));
			}
			bool isFinish = false;
			CanvasManager.HdlOpenWindowBasic.Setup("", string.IsNullOrEmpty(text) ? "現在ご利用いただけません" : text, list, true, delegate(int idx)
			{
				if (idx == 1)
				{
					CanvasManager.HdlWebViewWindowCtrl.Open(url);
					return false;
				}
				isFinish = true;
				this.window.owCtrl.ForceClose();
				this.requestStatus = SelPurchaseStoneWindowCtrl.Status.NONE;
				return true;
			}, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			while (!isFinish)
			{
				yield return null;
			}
		}
		else
		{
			this.isEnableMonthlyPack = DataManager.DmMonthlyPack.IsEnableMonthlyPack(TimeManager.Now);
		}
		if (this.tabPurchaseProductOneListList.Count < this.requestTabIndex || this.tabPurchaseProductOneListList[this.requestTabIndex].Count <= 0)
		{
			this.requestTabIndex = 0;
		}
		this.currentPurchaseProductOneList = this.tabPurchaseProductOneListList[this.requestTabIndex];
		int num = this.currentPurchaseProductOneList.Count;
		if (this.requestTabIndex == 0 && this.isEnableMonthlyPack)
		{
			num++;
		}
		this.window.ScrollView.Setup(num, 0);
		this.window.ScrollView.Refresh();
		this.window.tabGroupCtrl.Setup(this.requestTabIndex, new PguiTabGroupCtrl.OnSelectTab(this.OnSelectTab));
		this.isAllTab = this.requestTabIndex == 0;
		for (int i = 1; i < this.tabPurchaseProductOneListList.Count; i++)
		{
			this.window.tabGroupCtrl.m_PguiTabList[i].SetActEnable(0 < this.tabPurchaseProductOneListList[i].Count);
		}
		yield return null;
		PrjUtil.SendAppsFlyerLtvId("af_purchase_view", null);
		yield break;
	}

	// Token: 0x06001EC7 RID: 7879 RVA: 0x0017EE74 File Offset: 0x0017D074
	private void SortReleaseProduct()
	{
		if (0 < this.releaseProductIdList.Count)
		{
			foreach (int num in this.releaseProductIdList)
			{
				PurchaseProductOne.TabType tabType = PurchaseProductOne.TabType.Invalid;
				List<PurchaseProductOne> list = new List<PurchaseProductOne>(this.tabPurchaseProductOneListList[0]);
				foreach (PurchaseProductOne purchaseProductOne in this.tabPurchaseProductOneListList[0])
				{
					if (num == purchaseProductOne.productId)
					{
						tabType = purchaseProductOne.tabType;
						list.Remove(purchaseProductOne);
						list.Insert(0, purchaseProductOne);
						break;
					}
				}
				this.tabPurchaseProductOneListList[0] = list;
				int tabIndex = this.GetTabIndex(tabType);
				if (tabIndex != 0)
				{
					List<PurchaseProductOne> list2 = new List<PurchaseProductOne>(this.tabPurchaseProductOneListList[tabIndex]);
					foreach (PurchaseProductOne purchaseProductOne2 in this.tabPurchaseProductOneListList[tabIndex])
					{
						if (num == purchaseProductOne2.productId)
						{
							list2.Remove(purchaseProductOne2);
							list2.Insert(0, purchaseProductOne2);
							break;
						}
					}
					this.tabPurchaseProductOneListList[tabIndex] = list2;
				}
			}
		}
	}

	// Token: 0x06001EC8 RID: 7880 RVA: 0x0017F01C File Offset: 0x0017D21C
	private IEnumerator PurchaseSequence()
	{
		DataManager.DmPurchase.SolutionPurchase(this.purchaseProductOne.productId, this.purchaseProductOne.productIdString);
		while (!DataManager.DmPurchase.IsFinishPurchase)
		{
			yield return null;
		}
		DataManagerPurchase.PurchaseResult result = DataManager.DmPurchase.GetPurchaseResult();
		while (result.productIdString != this.purchaseProductOne.productIdString && result.status != DataManagerPurchase.PurchaseResult.Status.FAILURE_COMMON)
		{
			yield return null;
		}
		result.isTargetProduct = true;
		if (result.status == DataManagerPurchase.PurchaseResult.Status.SUCCESS)
		{
			SelPurchaseStoneWindowCtrl.<>c__DisplayClass34_0 CS$<>8__locals1 = new SelPurchaseStoneWindowCtrl.<>c__DisplayClass34_0();
			this.requestStatus = SelPurchaseStoneWindowCtrl.Status.NONE;
			if (this.resultAECtrl != null)
			{
				Object.Destroy(this.resultAECtrl.gameObject);
			}
			if (this.resultIconItemCtrl != null)
			{
				this.resultIconItemCtrl.gameObject.SetActive(false);
			}
			this.resultWindow.Txt_ItemNum.transform.parent.gameObject.SetActive(false);
			this.resultWindow.Img_item.gameObject.SetActive(false);
			bool isItem = this.ChkDispItemSetting(this.purchaseProductOne);
			if (isItem)
			{
				if (ItemDef.Kind.PRESET != ItemDef.Id2Kind(this.purchaseProductOne.MainItem.id) && this.ChkUseDefaultItemIconSetting(this.purchaseProductOne))
				{
					if (this.resultIconItemCtrl == null)
					{
						this.resultIconItemCtrl = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, this.resultWindow.Icon_item01.transform).GetComponent<IconItemCtrl>();
						this.resultIconItemCtrl.transform.localPosition = new Vector3(0f, -30f, 0f);
					}
					this.resultIconItemCtrl.gameObject.SetActive(true);
					this.resultIconItemCtrl.Setup(DataManager.DmItem.GetItemStaticBase(this.purchaseProductOne.MainItem.id));
					this.resultWindow.Txt_ItemNum.transform.parent.gameObject.SetActive(true);
					this.resultWindow.Txt_ItemNum.text = "×" + this.purchaseProductOne.MainItem.num.ToString();
				}
				else
				{
					this.resultWindow.Img_item.gameObject.SetActive(true);
					this.resultWindow.Img_item.SetRawImage(this.purchaseProductOne.MainItemIconOptionPath, true, false, null);
				}
			}
			else
			{
				GameObject gameObject = Resources.Load(this.purchaseProductOne.resultIconPath) as GameObject;
				this.resultAECtrl = Object.Instantiate<GameObject>(gameObject, this.resultWindow.Icon_item01.transform).GetComponent<PguiAECtrl>();
				this.resultAECtrl.transform.localPosition = new Vector3(0f, 0f);
				this.resultAECtrl.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
			}
			yield return null;
			string text = string.Empty;
			if (isItem)
			{
				text = "アイテムを獲得しました\n\nアイテムをプレゼントに送りました";
			}
			else
			{
				text = string.Format("キラキラ {0}個\n", this.purchaseProductOne.chargeNum) + ((this.purchaseProductOne.freeNum > 0) ? string.Format("＋おまけ {0}個\n", this.purchaseProductOne.freeNum) : "") + "を獲得しました" + ((this.purchaseProductOne.bonusItem != null) ? "\n\n残りのアイテムはプレゼントに送りました" : "");
			}
			CS$<>8__locals1.isResultWindowFinish = false;
			this.resultWindow.owCtrl.Setup(PrjUtil.MakeMessage("購入確認ウィンドウ"), text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int index)
			{
				CS$<>8__locals1.isResultWindowFinish = true;
				return true;
			}, null, false);
			this.resultWindow.owCtrl.Open();
			this.purchaseProductOne = null;
			while (!CS$<>8__locals1.isResultWindowFinish)
			{
				yield return null;
			}
			bool addedNewProduct = 0 < result.releasePurchaseProductList.Count;
			if (addedNewProduct)
			{
				IEnumerator enumrator = this.NotifyNewProduct(result.releasePurchaseProductList);
				do
				{
					yield return null;
				}
				while (enumrator.MoveNext());
				enumrator = null;
			}
			this.window.owCtrl.ForceClose();
			this.isActiveWindow = false;
			foreach (UnityAction unityAction in this.onSuccessPurchaseList)
			{
				unityAction();
			}
			if (addedNewProduct)
			{
				do
				{
					yield return null;
				}
				while (!this.window.owCtrl.FinishedClose());
				PurchaseProductOne.TabType tabType;
				if (1 == result.releasePurchaseProductList.Count)
				{
					tabType = (PurchaseProductOne.TabType)result.releasePurchaseProductList[0].tab;
				}
				else
				{
					tabType = PurchaseProductOne.TabType.Invalid;
				}
				this.Setup(tabType);
			}
			CS$<>8__locals1 = null;
		}
		else if (result.status == DataManagerPurchase.PurchaseResult.Status.FAILURE_LIMIT_AGE)
		{
			this.requestStatus = SelPurchaseStoneWindowCtrl.Status.NONE;
			CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("年齢認証エラー"), PrjUtil.MakeMessage("お客様の年齢における当月のご利用金額が\n限度額に達している為、ご利用いただけません\n1か月あたりの限度額は\n13歳未満が5,000円(pt)\n20歳未満が20,000円(pt)です"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, null, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			this.purchaseProductOne = null;
		}
		else if (result.status == DataManagerPurchase.PurchaseResult.Status.FAILURE_PENDING_COUNT_LIMIT)
		{
			this.requestStatus = SelPurchaseStoneWindowCtrl.Status.NONE;
			CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("エラー"), PrjUtil.MakeMessage("同じアカウントが紐づけられた別の端末で\nお支払い待ちの商品のため、現在購入できません"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, null, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			this.purchaseProductOne = null;
		}
		else if (result.status != DataManagerPurchase.PurchaseResult.Status.FAILURE_APP_SERVER)
		{
			if (result.status == DataManagerPurchase.PurchaseResult.Status.FAILURE_INTERRUPT || result.status == DataManagerPurchase.PurchaseResult.Status.FAILURE_OS_SERVER)
			{
				SelPurchaseStoneWindowCtrl.<>c__DisplayClass34_1 CS$<>8__locals2 = new SelPurchaseStoneWindowCtrl.<>c__DisplayClass34_1();
				CS$<>8__locals2.isResultWindowFinish = false;
				CanvasManager.HdlOpenWindowServerError.Setup(PrjUtil.MakeMessage(""), PrjUtil.MakeMessage(string.Format("購入に失敗しました\nCode：{0}", (int)result.status)), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int index)
				{
					CS$<>8__locals2.isResultWindowFinish = true;
					return true;
				}, null, false);
				CanvasManager.HdlOpenWindowServerError.Open();
				this.requestStatus = SelPurchaseStoneWindowCtrl.Status.ERROR;
				while (!CS$<>8__locals2.isResultWindowFinish)
				{
					yield return null;
				}
				this.window.owCtrl.ForceClose();
				this.isActiveWindow = false;
				CS$<>8__locals2 = null;
			}
			else if (result.status == DataManagerPurchase.PurchaseResult.Status.FAILURE_DMM_POINT)
			{
				CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage(""), PrjUtil.MakeMessage("通信エラーが発⽣しました。\n電波状況をご確認いただき、再度お試しください。"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, null, null, false);
				CanvasManager.HdlOpenWindowBasic.Open();
				this.requestStatus = SelPurchaseStoneWindowCtrl.Status.ERROR;
			}
			else if (result.status == DataManagerPurchase.PurchaseResult.Status.FAILURE_PENDING_PURCHASE)
			{
				SelPurchaseStoneWindowCtrl.<>c__DisplayClass34_2 CS$<>8__locals3 = new SelPurchaseStoneWindowCtrl.<>c__DisplayClass34_2();
				CS$<>8__locals3.isResultWindowFinish = false;
				CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("エラー"), PrjUtil.MakeMessage("お支払いを確認できませんでした。\nお支払い完了後、キラキラ&パック購入画面へ\n再度アクセスしてください。"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int index)
				{
					CS$<>8__locals3.isResultWindowFinish = true;
					return true;
				}, null, false);
				CanvasManager.HdlOpenWindowBasic.Open();
				this.requestStatus = SelPurchaseStoneWindowCtrl.Status.ERROR;
				this.purchaseProductOne = null;
				while (!CS$<>8__locals3.isResultWindowFinish)
				{
					yield return null;
				}
				this.window.owCtrl.ForceClose();
				this.isActiveWindow = false;
				CS$<>8__locals3 = null;
			}
			else
			{
				CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage(""), PrjUtil.MakeMessage("購入がキャンセルされました"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, null, null, false);
				CanvasManager.HdlOpenWindowBasic.Open();
				this.requestStatus = SelPurchaseStoneWindowCtrl.Status.ERROR;
			}
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001EC9 RID: 7881 RVA: 0x0017F02B File Offset: 0x0017D22B
	private IEnumerator NotifyNewProduct(List<PurchaseProductStatic> productList)
	{
		this.isNotifyNewProduct = true;
		int num;
		for (int index = 0; index < productList.Count; index = num + 1)
		{
			string text = productList[index].productName.Replace(" ", "\n");
			string text2 = "キラキラ&パック購入に\n\n" + text + "\n\nが追加されました！";
			if (productList.Count - 1 != index)
			{
				CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("新規追加！"), text2, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, null, null, false);
				CanvasManager.HdlOpenWindowBasic.Open();
				do
				{
					yield return null;
				}
				while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
			}
			else
			{
				SelPurchaseStoneWindowCtrl.<>c__DisplayClass35_0 CS$<>8__locals1 = new SelPurchaseStoneWindowCtrl.<>c__DisplayClass35_0();
				CS$<>8__locals1.isWindowFinish = false;
				CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("新規追加！"), text2, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int buttonIndex)
				{
					CS$<>8__locals1.isWindowFinish = true;
					return true;
				}, null, false);
				CanvasManager.HdlOpenWindowBasic.Open();
				while (!CS$<>8__locals1.isWindowFinish)
				{
					yield return null;
				}
				CS$<>8__locals1 = null;
			}
			num = index;
		}
		PrjUtil.InsertionSort<PurchaseProductStatic>(ref productList, (PurchaseProductStatic a, PurchaseProductStatic b) => b.sortIndex - a.sortIndex);
		this.releaseProductIdList.Clear();
		foreach (PurchaseProductStatic purchaseProductStatic in productList)
		{
			this.releaseProductIdList.Add(purchaseProductStatic.productIdCommon);
		}
		this.clearReleaseProductIdList = false;
		this.isNotifyNewProduct = false;
		yield break;
	}

	// Token: 0x06001ECA RID: 7882 RVA: 0x0017F041 File Offset: 0x0017D241
	private IEnumerator AgeAuthenticationSequence()
	{
		IEnumerator func = DataManager.DmPurchase.RequestSolutionAgeAuthentic();
		while (func.MoveNext())
		{
			yield return null;
		}
		this.requestStatus = SelPurchaseStoneWindowCtrl.Status.NONE;
		yield break;
	}

	// Token: 0x06001ECB RID: 7883 RVA: 0x0017F050 File Offset: 0x0017D250
	private void OnStartWindow(int index, GameObject go)
	{
		GameObject gameObject = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, go.transform.Find("BaseImage/Info_Pack/Buy_Img_PackItem_Single"));
		gameObject.transform.SetAsFirstSibling();
		gameObject.GetComponent<IconItemCtrl>().SetRaycastTargetIconItem(false);
		SelPurchaseStoneWindowCtrl.BuyStonePlate buyStonePlate = new SelPurchaseStoneWindowCtrl.BuyStonePlate(go.transform);
		buyStonePlate.CmnShop_BuyStone.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickProductButton), PguiButtonCtrl.SoundType.DEFAULT);
		buyStonePlate.CmnShop_BuyStone.gameObject.AddComponent<PguiDataHolder>();
		buyStonePlate.Info_Btn.gameObject.AddComponent<PguiDataHolder>();
		buyStonePlate.Info_Btn.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickPresetInfoButton), PguiButtonCtrl.SoundType.DEFAULT);
	}

	// Token: 0x06001ECC RID: 7884 RVA: 0x0017F0F0 File Offset: 0x0017D2F0
	private void OnUpdateWindow(int index, GameObject go)
	{
		SelPurchaseStoneWindowCtrl.BuyStonePlate buyStonePlate = new SelPurchaseStoneWindowCtrl.BuyStonePlate(go.transform);
		if (index < this.currentPurchaseProductOneList.Count)
		{
			PurchaseProductOne purchaseProductOne = this.currentPurchaseProductOneList[index];
			buyStonePlate.baseObj.SetActive(true);
			buyStonePlate.Txt_Times.text = ((purchaseProductOne.limitText != null) ? purchaseProductOne.limitText : "");
			buyStonePlate.Num_Txt.transform.parent.gameObject.SetActive(true);
			buyStonePlate.Num_Txt.text = purchaseProductOne.price.ToString() + PrjUtil.MakeMessage("pt");
			foreach (SelPurchaseStoneWindowCtrl.BuyStonePlate.BaseInfo baseInfo in buyStonePlate.infoList)
			{
				baseInfo.baseObj.SetActive(false);
			}
			if (purchaseProductOne.MonthlyPackId != 0)
			{
				buyStonePlate.infoList[2].baseObj.SetActive(true);
				buyStonePlate.Num_Txt.transform.parent.gameObject.SetActive(false);
				buyStonePlate.Mark_Own.gameObject.SetActive(false);
				buyStonePlate.Base_Rainbow.gameObject.SetActive(false);
				buyStonePlate.Info_Btn.gameObject.SetActive(false);
				buyStonePlate.baseImage.ReplaceColorByPguiColorCtrl("NORMAL");
				buyStonePlate.Base_Pattern.ReplaceColorByPguiColorCtrl("NORMAL");
				buyStonePlate.TitleBase.ReplaceColorByPguiColorCtrl("NORMAL");
				buyStonePlate.iconImage.gameObject.SetActive(false);
				buyStonePlate.CmnShop_BuyStone.SetActEnable(true, false, true);
				buyStonePlate.Txt_Times.text = "";
			}
			else
			{
				string text = string.Empty;
				bool flag = false;
				bool flag2 = false;
				switch (purchaseProductOne.bgType)
				{
				case PurchaseProductOne.BGType.Rainbow:
					text = "PACK_RAINBOW";
					flag = true;
					flag2 = true;
					break;
				case PurchaseProductOne.BGType.Orange:
					text = "PACK";
					break;
				case PurchaseProductOne.BGType.Red:
					text = "PACK_RED";
					flag2 = true;
					break;
				case PurchaseProductOne.BGType.Yellow:
					text = "PACK_YELLOW";
					flag2 = true;
					break;
				case PurchaseProductOne.BGType.Pink:
					text = "LIMITED";
					break;
				case PurchaseProductOne.BGType.LightPink:
					text = "PACK_LIGHTPINK";
					flag2 = true;
					break;
				case PurchaseProductOne.BGType.Green:
					text = "PACK_GREEN";
					flag2 = true;
					break;
				case PurchaseProductOne.BGType.Blue:
					text = "PACK_BLUE";
					flag2 = true;
					break;
				case PurchaseProductOne.BGType.Normal:
					text = "NORMAL";
					break;
				default:
					text = "NORMAL";
					break;
				}
				buyStonePlate.Base_Rainbow.gameObject.SetActive(flag);
				buyStonePlate.baseImage.ReplaceColorByPguiColorCtrl(flag ? "NORMAL" : text);
				if (this.releaseProductIdList.Contains(purchaseProductOne.productId))
				{
					buyStonePlate.iconImage.gameObject.SetActive(true);
					buyStonePlate.iconImage.Replace(-1);
				}
				else
				{
					bool flag3 = purchaseProductOne.commentType > PurchaseProductOne.CommentType.Invalid;
					buyStonePlate.iconImage.gameObject.SetActive(flag3);
					if (flag3)
					{
						buyStonePlate.iconImage.Replace((int)purchaseProductOne.commentType);
					}
				}
				bool flag4 = this.ChkDispItemSetting(purchaseProductOne);
				if (purchaseProductOne.isDispInfo || flag4)
				{
					buyStonePlate.infoList[1].baseObj.SetActive(true);
					buyStonePlate.Txt_PackName.text = purchaseProductOne.bonusItemTitle;
					buyStonePlate.Base_Pattern.ReplaceColorByPguiColorCtrl(text);
					buyStonePlate.TitleBase.ReplaceColorByPguiColorCtrl(text);
					buyStonePlate.Txt_PackNameGradationOutline.enabled = flag2;
					buyStonePlate.Txt_PackNameGradationText.enabled = flag2;
					if (flag2)
					{
						buyStonePlate.Txt_PackNameGradient.SetGameObjectById(text);
					}
					buyStonePlate.infoList[1].Txt_Stone.gameObject.SetActive(!flag4);
					buyStonePlate.infoList[1].Buy_Img_Stone.gameObject.SetActive(!flag4);
					buyStonePlate.infoList[1].Buy_Img_PackItem.gameObject.SetActive(!flag4);
					buyStonePlate.infoList[1].Img_Plus.gameObject.SetActive(!flag4);
					buyStonePlate.infoList[1].Txt_Option.gameObject.SetActive(!flag4);
					buyStonePlate.infoList[1].MainItemIcon.transform.parent.gameObject.SetActive(flag4);
					buyStonePlate.infoList[1].Txt_MainItem.gameObject.SetActive(flag4);
					if (flag4)
					{
						buyStonePlate.infoList[1].Txt_MainItem.text = purchaseProductOne.infoText;
						bool flag5 = ItemDef.Kind.PRESET != ItemDef.Id2Kind(purchaseProductOne.MainItem.id) && this.ChkUseDefaultItemIconSetting(purchaseProductOne);
						buyStonePlate.infoList[1].MainItemOptionIcon.gameObject.SetActive(!flag5);
						buyStonePlate.infoList[1].MainItemIcon.gameObject.SetActive(flag5);
						buyStonePlate.infoList[1].Txt_MainItem_Num.transform.parent.gameObject.SetActive(flag5);
						if (flag5)
						{
							buyStonePlate.infoList[1].MainItemIcon.Setup(DataManager.DmItem.GetItemStaticBase(purchaseProductOne.MainItem.id));
							buyStonePlate.infoList[1].Txt_MainItem_Num.text = "×" + purchaseProductOne.MainItem.num.ToString();
						}
						else
						{
							buyStonePlate.infoList[1].MainItemOptionIcon.SetRawImage(purchaseProductOne.MainItemIconOptionPath, true, false, null);
						}
					}
					else if (purchaseProductOne.isDispInfo)
					{
						buyStonePlate.infoList[1].Txt_Option.text = purchaseProductOne.infoText;
						buyStonePlate.infoList[1].Txt_Stone.ReplaceTextByDefault("Param01", purchaseProductOne.chargeNum.ToString());
						buyStonePlate.infoList[1].Buy_Img_Stone.SetRawImage(purchaseProductOne.iconPath, true, false, null);
						buyStonePlate.infoList[1].Buy_Img_PackItem.SetRawImage(purchaseProductOne.iconOptionPath, true, false, null);
					}
				}
				else
				{
					buyStonePlate.infoList[0].baseObj.SetActive(true);
					buyStonePlate.infoList[0].Txt_Stone.ReplaceTextByDefault("Param01", purchaseProductOne.chargeNum.ToString());
					buyStonePlate.infoList[0].Txt_Option.text = ((purchaseProductOne.freeNum > 0) ? string.Format("＋おまけ {0}個", purchaseProductOne.freeNum) : "");
					buyStonePlate.infoList[0].Buy_Img_Stone.SetRawImage(purchaseProductOne.iconPath, true, false, null);
					buyStonePlate.Info_Btn.gameObject.SetActive(false);
				}
			}
			if (purchaseProductOne.MonthlyPackId == 0)
			{
				if (DataManager.DmPurchase.SoldOutIdList.Contains(purchaseProductOne.productId))
				{
					buyStonePlate.CmnShop_BuyStone.SetActEnable(false, false, true);
					buyStonePlate.Mark_Own.gameObject.SetActive(true);
					buyStonePlate.iconImage.gameObject.SetActive(false);
				}
				else
				{
					buyStonePlate.CmnShop_BuyStone.SetActEnable(true, false, true);
					buyStonePlate.Mark_Own.gameObject.SetActive(false);
				}
			}
			buyStonePlate.CmnShop_BuyStone.GetComponent<PguiDataHolder>().id = purchaseProductOne.productId;
			buyStonePlate.Info_Btn.gameObject.GetComponent<PguiDataHolder>().id = purchaseProductOne.productId;
			return;
		}
		if (this.isEnableMonthlyPack && this.isAllTab && index == this.currentPurchaseProductOneList.Count)
		{
			buyStonePlate.baseObj.SetActive(true);
			buyStonePlate.infoList[0].baseObj.SetActive(false);
			buyStonePlate.infoList[1].baseObj.SetActive(false);
			buyStonePlate.infoList[2].baseObj.SetActive(true);
			buyStonePlate.Num_Txt.transform.parent.gameObject.SetActive(false);
			buyStonePlate.Mark_Own.gameObject.SetActive(true);
			buyStonePlate.Base_Rainbow.gameObject.SetActive(false);
			buyStonePlate.Info_Btn.gameObject.SetActive(false);
			buyStonePlate.baseImage.ReplaceColorByPguiColorCtrl("NORMAL");
			buyStonePlate.Base_Pattern.ReplaceColorByPguiColorCtrl("NORMAL");
			buyStonePlate.TitleBase.ReplaceColorByPguiColorCtrl("NORMAL");
			buyStonePlate.iconImage.gameObject.SetActive(false);
			buyStonePlate.CmnShop_BuyStone.SetActEnable(true, false, true);
			buyStonePlate.Txt_Times.text = "";
			buyStonePlate.CmnShop_BuyStone.GetComponent<PguiDataHolder>().id = -1;
			buyStonePlate.Info_Btn.gameObject.GetComponent<PguiDataHolder>().id = -1;
			return;
		}
		buyStonePlate.baseObj.SetActive(false);
	}

	// Token: 0x06001ECD RID: 7885 RVA: 0x0017F9C4 File Offset: 0x0017DBC4
	private bool ChkDispItemSetting(PurchaseProductOne ppo)
	{
		return ppo.chargeNum == 0 && ppo.freeNum == 0 && ppo.bonusItem == null && ppo.MainItem != null && 0 < ppo.MainItem.num && "Texture2D/Shop_BuyImg_Pack/" != ppo.MainItemIconOptionPath;
	}

	// Token: 0x06001ECE RID: 7886 RVA: 0x0017FA11 File Offset: 0x0017DC11
	private bool ChkUseDefaultItemIconSetting(PurchaseProductOne ppo)
	{
		return "Texture2D/Shop_BuyImg_Pack/0" == ppo.MainItemIconOptionPath;
	}

	// Token: 0x06001ECF RID: 7887 RVA: 0x0017FA24 File Offset: 0x0017DC24
	private void OnClickPresetInfoButton(PguiButtonCtrl button)
	{
		if (this.isNotifyNewProduct)
		{
			return;
		}
		this.purchaseProductOne = this.currentPurchaseProductOneList.Find((PurchaseProductOne item) => item.productId == button.gameObject.GetComponent<PguiDataHolder>().id);
	}

	// Token: 0x06001ED0 RID: 7888 RVA: 0x0017FA64 File Offset: 0x0017DC64
	private void OnClickPresetInfoButton(PurchaseProductOne purchaseProductOne, Action ok_callback = null, Action cancel_callback = null, bool isApppayExists = false)
	{
		if (this.isNotifyNewProduct)
		{
			return;
		}
		CanvasManager.HdlItemPresetWindowCtrl.OpenByPurchase(purchaseProductOne, ok_callback, cancel_callback, isApppayExists);
	}

	// Token: 0x06001ED1 RID: 7889 RVA: 0x0017FA80 File Offset: 0x0017DC80
	private void OnClickProductButton(PguiButtonCtrl button)
	{
		if (this.isNotifyNewProduct)
		{
			return;
		}
		this.purchaseProductOne = this.currentPurchaseProductOneList.Find((PurchaseProductOne item) => item.productId == button.gameObject.GetComponent<PguiDataHolder>().id);
		bool flag = DataManager.DmPurchase.PurchaseProductStaticList.FindAll((PurchaseProductStatic item) => item.groupId == this.purchaseProductOne.groupId).Exists((PurchaseProductStatic item) => item.storeType == 5);
		this.OnClickProductButton(this.purchaseProductOne, flag);
	}

	// Token: 0x06001ED2 RID: 7890 RVA: 0x0017FB14 File Offset: 0x0017DD14
	private void OnClickProductButton(PurchaseProductOne purchaseProductOne, bool isApppayExists)
	{
		if (this.isNotifyNewProduct)
		{
			return;
		}
		PurchaseProductOne tmp = purchaseProductOne;
		if (tmp != null)
		{
			if (tmp.MonthlyPackId != 0)
			{
				this.requestStatus = SelPurchaseStoneWindowCtrl.Status.NONE;
				this.isActiveWindow = false;
				this.window.owCtrl.ForceClose();
				CanvasManager.HdlSelMonthlyPackWindowCtrl.Setup();
			}
			else if (DataManager.DmPurchase.IsEnableBirthday)
			{
				this.OnClickPresetInfoButton(purchaseProductOne, delegate
				{
					purchaseProductOne = tmp;
					this.requestStatus = SelPurchaseStoneWindowCtrl.Status.PURCHASE;
				}, delegate
				{
				}, isApppayExists);
			}
			else
			{
				this.requestStatus = SelPurchaseStoneWindowCtrl.Status.AGE_AUTHENTICATION;
			}
			if (-1 == tmp.productId)
			{
				this.requestStatus = SelPurchaseStoneWindowCtrl.Status.NONE;
				this.isActiveWindow = false;
				this.window.owCtrl.ForceClose();
				CanvasManager.HdlSelMonthlyPackWindowCtrl.Setup();
			}
		}
	}

	// Token: 0x06001ED3 RID: 7891 RVA: 0x0017FC0F File Offset: 0x0017DE0F
	private bool OnClickOwButton(int index)
	{
		if (this.isNotifyNewProduct)
		{
			return false;
		}
		this.requestStatus = SelPurchaseStoneWindowCtrl.Status.NONE;
		this.isActiveWindow = false;
		return true;
	}

	// Token: 0x06001ED4 RID: 7892 RVA: 0x0017FC2C File Offset: 0x0017DE2C
	private bool OnSelectTab(int index)
	{
		if (this.isNotifyNewProduct)
		{
			return false;
		}
		this.currentPurchaseProductOneList = this.tabPurchaseProductOneListList[index];
		int num = this.currentPurchaseProductOneList.Count;
		this.isAllTab = index == 0;
		if (this.isEnableMonthlyPack && this.isAllTab)
		{
			num++;
		}
		this.window.Txt_None.SetActive(num <= 0);
		this.window.ScrollView.Resize(num, 0);
		return true;
	}

	// Token: 0x0400166A RID: 5738
	private const int DUMMY_MONTHLYPACK_PRODUCT_ID = -1;

	// Token: 0x0400166B RID: 5739
	private const int ALL_TAB_INDEX = 0;

	// Token: 0x0400166C RID: 5740
	private SelPurchaseStoneWindowCtrl.Window window;

	// Token: 0x0400166D RID: 5741
	private GetItemWindowCtrl.GUI resultWindow;

	// Token: 0x0400166E RID: 5742
	private PguiAECtrl resultAECtrl;

	// Token: 0x0400166F RID: 5743
	private IconItemCtrl resultIconItemCtrl;

	// Token: 0x04001670 RID: 5744
	private IEnumerator setupSequence;

	// Token: 0x04001671 RID: 5745
	private IEnumerator purchaseSequence;

	// Token: 0x04001672 RID: 5746
	private IEnumerator ageAuthenticationSequence;

	// Token: 0x04001673 RID: 5747
	private List<PurchaseProductOne> currentPurchaseProductOneList = new List<PurchaseProductOne>();

	// Token: 0x04001674 RID: 5748
	private List<List<PurchaseProductOne>> tabPurchaseProductOneListList;

	// Token: 0x04001675 RID: 5749
	private PurchaseProductOne purchaseProductOne;

	// Token: 0x04001676 RID: 5750
	private List<int> releaseProductIdList;

	// Token: 0x04001677 RID: 5751
	private bool clearReleaseProductIdList;

	// Token: 0x04001678 RID: 5752
	private List<UnityAction> onSuccessPurchaseList = new List<UnityAction>();

	// Token: 0x04001679 RID: 5753
	private SelPurchaseStoneWindowCtrl.Status requestStatus;

	// Token: 0x0400167A RID: 5754
	private SelPurchaseStoneWindowCtrl.Status currentStatus;

	// Token: 0x0400167B RID: 5755
	private int requestTabIndex;

	// Token: 0x0400167C RID: 5756
	private bool isActiveWindow;

	// Token: 0x0400167D RID: 5757
	private bool isEnableMonthlyPack;

	// Token: 0x0400167E RID: 5758
	private bool isAllTab;

	// Token: 0x0400167F RID: 5759
	private bool isNotifyNewProduct;

	// Token: 0x02000FD6 RID: 4054
	public enum Status
	{
		// Token: 0x04005908 RID: 22792
		NONE,
		// Token: 0x04005909 RID: 22793
		ERROR,
		// Token: 0x0400590A RID: 22794
		SETUP,
		// Token: 0x0400590B RID: 22795
		AGE_AUTHENTICATION,
		// Token: 0x0400590C RID: 22796
		PURCHASE
	}

	// Token: 0x02000FD7 RID: 4055
	public class Window
	{
		// Token: 0x0600512C RID: 20780 RVA: 0x002443B8 File Offset: 0x002425B8
		public Window(Transform baseTr)
		{
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.Btn_Law = baseTr.Find("Base/Window/Btn_Law").GetComponent<PguiButtonCtrl>();
			this.Btn_Offer = baseTr.Find("Base/Window/Btn_Offer").GetComponent<PguiButtonCtrl>();
			this.banner = baseTr.Find("Base/Window/Banner_Official").gameObject;
			this.Txt = baseTr.Find("Base/Window/Title/Txt").GetComponent<PguiTextCtrl>();
			this.ScrollView = baseTr.Find("Base/Window/InBase/ScrollView").GetComponent<ReuseScroll>();
			this.Num_Own = baseTr.Find("Base/Window/ItemOwnBase/Num_Own").GetComponent<PguiTextCtrl>();
			this.Txt_None = baseTr.Find("Base/Window/InBase/ScrollView/Txt_None").gameObject;
			this.tabGroupCtrl = baseTr.Find("Base/Window/InBase/TabGroup").GetComponent<PguiTabGroupCtrl>();
		}

		// Token: 0x0400590D RID: 22797
		public PguiOpenWindowCtrl owCtrl;

		// Token: 0x0400590E RID: 22798
		public PguiButtonCtrl Btn_Law;

		// Token: 0x0400590F RID: 22799
		public PguiButtonCtrl Btn_Offer;

		// Token: 0x04005910 RID: 22800
		public PguiTextCtrl Txt;

		// Token: 0x04005911 RID: 22801
		public ReuseScroll ScrollView;

		// Token: 0x04005912 RID: 22802
		public PguiTextCtrl Num_Own;

		// Token: 0x04005913 RID: 22803
		public GameObject Txt_None;

		// Token: 0x04005914 RID: 22804
		public PguiTabGroupCtrl tabGroupCtrl;

		// Token: 0x04005915 RID: 22805
		public GameObject banner;

		// Token: 0x04005916 RID: 22806
		public static readonly string BANNER_TEXTURE = "Texture2D/AdvertiseBanner/XXXZeWoMxaxlIQ";
	}

	// Token: 0x02000FD8 RID: 4056
	public class BuyStonePlate
	{
		// Token: 0x0600512E RID: 20782 RVA: 0x00244494 File Offset: 0x00242694
		public BuyStonePlate(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.baseImage = baseTr.Find("BaseImage").GetComponent<PguiImageCtrl>();
			this.Base_Rainbow = baseTr.Find("BaseImage/Base_Rainbow").GetComponent<PguiImageCtrl>();
			this.Base_Pattern = baseTr.Find("BaseImage/Info_Pack/Base_Pattern").GetComponent<PguiImageCtrl>();
			this.TitleBase = baseTr.Find("BaseImage/Info_Pack/TitleBase").GetComponent<PguiImageCtrl>();
			this.iconImage = baseTr.Find("BaseImage/Mark_Reasonable").GetComponent<PguiReplaceSpriteCtrl>();
			this.CmnShop_BuyStone = baseTr.GetComponent<PguiButtonCtrl>();
			this.Info_Btn = baseTr.Find("BaseImage/Info_Btn").GetComponent<PguiButtonCtrl>();
			this.Txt_PackName = baseTr.Find("BaseImage/Info_Pack/Txt_PackName").GetComponent<PguiTextCtrl>();
			this.Txt_PackNameGradient = baseTr.Find("BaseImage/Info_Pack/Txt_PackName").GetComponent<PguiGradientCtrl>();
			this.Txt_PackNameGradationText = baseTr.Find("BaseImage/Info_Pack/Txt_PackName").GetComponent<GradationText>();
			this.Txt_PackNameGradationOutline = baseTr.Find("BaseImage/Info_Pack/Txt_PackName").GetComponent<PguiOutline>();
			this.Num_Txt = baseTr.Find("BaseImage/ItemOwnBase/Num_Txt").GetComponent<PguiTextCtrl>();
			this.Txt_Times = baseTr.Find("BaseImage/Txt_Times").GetComponent<PguiTextCtrl>();
			this.Mark_Own = baseTr.Find("BaseImage/Mark_Own").GetComponent<PguiImageCtrl>();
			this.Mark_Reasonable = baseTr.Find("BaseImage/Mark_Reasonable").GetComponent<PguiImageCtrl>();
			this.infoList = new List<SelPurchaseStoneWindowCtrl.BuyStonePlate.BaseInfo>
			{
				new SelPurchaseStoneWindowCtrl.BuyStonePlate.BaseInfo(baseTr.Find("BaseImage/Info_Normal")),
				new SelPurchaseStoneWindowCtrl.BuyStonePlate.BaseInfo(baseTr.Find("BaseImage/Info_Pack")),
				new SelPurchaseStoneWindowCtrl.BuyStonePlate.BaseInfo(baseTr.Find("BaseImage/Info_Pass"))
			};
		}

		// Token: 0x04005917 RID: 22807
		public GameObject baseObj;

		// Token: 0x04005918 RID: 22808
		public PguiButtonCtrl CmnShop_BuyStone;

		// Token: 0x04005919 RID: 22809
		public PguiReplaceSpriteCtrl iconImage;

		// Token: 0x0400591A RID: 22810
		public PguiTextCtrl Txt_PackName;

		// Token: 0x0400591B RID: 22811
		public PguiButtonCtrl Info_Btn;

		// Token: 0x0400591C RID: 22812
		public PguiTextCtrl Num_Txt;

		// Token: 0x0400591D RID: 22813
		public PguiTextCtrl Txt_Times;

		// Token: 0x0400591E RID: 22814
		public PguiImageCtrl Mark_Own;

		// Token: 0x0400591F RID: 22815
		public PguiImageCtrl Mark_Reasonable;

		// Token: 0x04005920 RID: 22816
		public List<SelPurchaseStoneWindowCtrl.BuyStonePlate.BaseInfo> infoList = new List<SelPurchaseStoneWindowCtrl.BuyStonePlate.BaseInfo>();

		// Token: 0x04005921 RID: 22817
		public PguiImageCtrl baseImage;

		// Token: 0x04005922 RID: 22818
		public PguiImageCtrl Base_Rainbow;

		// Token: 0x04005923 RID: 22819
		public PguiImageCtrl Base_Pattern;

		// Token: 0x04005924 RID: 22820
		public PguiImageCtrl TitleBase;

		// Token: 0x04005925 RID: 22821
		public PguiGradientCtrl Txt_PackNameGradient;

		// Token: 0x04005926 RID: 22822
		public GradationText Txt_PackNameGradationText;

		// Token: 0x04005927 RID: 22823
		public PguiOutline Txt_PackNameGradationOutline;

		// Token: 0x02001224 RID: 4644
		public class BaseInfo
		{
			// Token: 0x060057ED RID: 22509 RVA: 0x00259D14 File Offset: 0x00257F14
			public BaseInfo(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				Transform transform = baseTr.Find("Base_Pattern");
				this.Base_Pattern = ((transform != null) ? transform.GetComponent<PguiImageCtrl>() : null);
				Transform transform2 = baseTr.Find("TitleBase");
				this.TitleBase = ((transform2 != null) ? transform2.GetComponent<PguiImageCtrl>() : null);
				Transform transform3 = baseTr.Find("ItemInfo");
				this.ItemInfo = ((transform3 != null) ? transform3.GetComponent<PguiImageCtrl>() : null);
				Transform transform4 = baseTr.Find("Img_Plus");
				this.Img_Plus = ((transform4 != null) ? transform4.GetComponent<PguiImageCtrl>() : null);
				Transform transform5 = baseTr.Find("Buy_Img_Stone");
				this.Buy_Img_Stone = ((transform5 != null) ? transform5.GetComponent<PguiRawImageCtrl>() : null);
				Transform transform6 = baseTr.Find("Buy_Img_PackItem");
				this.Buy_Img_PackItem = ((transform6 != null) ? transform6.GetComponent<PguiRawImageCtrl>() : null);
				Transform transform7 = baseTr.Find("Buy_Img_PackItem_Single/Icon_Item(Clone)");
				this.MainItemIcon = ((transform7 != null) ? transform7.GetComponent<IconItemCtrl>() : null);
				Transform transform8 = baseTr.Find("Buy_Img_PackItem_Single/Icon_Item");
				this.MainItemOptionIcon = ((transform8 != null) ? transform8.GetComponent<PguiRawImageCtrl>() : null);
				Transform transform9 = baseTr.Find("Buy_Img_PackItem_Single/Txt_Window/Num_Txt");
				this.Txt_MainItem_Num = ((transform9 != null) ? transform9.GetComponent<PguiTextCtrl>() : null);
				Transform transform10 = baseTr.Find("Txt_PackName");
				this.Txt_PackName = ((transform10 != null) ? transform10.GetComponent<PguiTextCtrl>() : null);
				Transform transform11 = baseTr.Find("ItemInfo/Txt_Stone");
				this.Txt_Stone = ((transform11 != null) ? transform11.GetComponent<PguiTextCtrl>() : null);
				Transform transform12 = baseTr.Find("ItemInfo/Txt_Option");
				this.Txt_Option = ((transform12 != null) ? transform12.GetComponent<PguiTextCtrl>() : null);
				Transform transform13 = baseTr.Find("ItemInfo/Txt_Option02");
				this.Txt_MainItem = ((transform13 != null) ? transform13.GetComponent<PguiTextCtrl>() : null);
			}

			// Token: 0x0400635A RID: 25434
			public GameObject baseObj;

			// Token: 0x0400635B RID: 25435
			public PguiImageCtrl Base_Pattern;

			// Token: 0x0400635C RID: 25436
			public PguiImageCtrl TitleBase;

			// Token: 0x0400635D RID: 25437
			public PguiImageCtrl ItemInfo;

			// Token: 0x0400635E RID: 25438
			public PguiImageCtrl Img_Plus;

			// Token: 0x0400635F RID: 25439
			public PguiRawImageCtrl Buy_Img_Stone;

			// Token: 0x04006360 RID: 25440
			public PguiRawImageCtrl Buy_Img_PackItem;

			// Token: 0x04006361 RID: 25441
			public IconItemCtrl MainItemIcon;

			// Token: 0x04006362 RID: 25442
			public PguiRawImageCtrl MainItemOptionIcon;

			// Token: 0x04006363 RID: 25443
			public PguiTextCtrl Txt_PackName;

			// Token: 0x04006364 RID: 25444
			public PguiTextCtrl Txt_Stone;

			// Token: 0x04006365 RID: 25445
			public PguiTextCtrl Txt_Option;

			// Token: 0x04006366 RID: 25446
			public PguiTextCtrl Txt_MainItem;

			// Token: 0x04006367 RID: 25447
			public PguiTextCtrl Txt_MainItem_Num;
		}

		// Token: 0x02001225 RID: 4645
		public enum InfoListType
		{
			// Token: 0x04006369 RID: 25449
			Normal,
			// Token: 0x0400636A RID: 25450
			Pack,
			// Token: 0x0400636B RID: 25451
			Pass
		}
	}
}
