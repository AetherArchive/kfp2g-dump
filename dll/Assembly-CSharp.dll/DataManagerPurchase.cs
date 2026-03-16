using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using DMMHelper;
using SGNFW.Common;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.InAppPurchase;
using SGNFW.Login;
using SGNFW.Mst;
using UnityEngine;

public class DataManagerPurchase
{
	public List<PurchaseProductStatic> PurchaseProductStaticList
	{
		get
		{
			return this.PurchaseProductStaticDataMap.Values.ToList<PurchaseProductStatic>();
		}
	}

	public List<int> SoldOutIdList { get; private set; }

	public List<int> PendingIdList { get; private set; }

	public List<int> InfoHiddenList { get; private set; }

	public DataManagerPurchase(DataManager p)
	{
		this.parentData = p;
	}

	public bool IsFinishSetupProduct { get; private set; }

	public bool IsFinishPurchase { get; private set; }

	public DateTime? userBirthDay
	{
		get
		{
			return this.birthday;
		}
	}

	public bool IsEnableBirthday
	{
		get
		{
			return this.birthday != null;
		}
	}

	public bool IsPendingMonthlyPack
	{
		get
		{
			return this.isPendingMonthlyPack;
		}
	}

	public DateTime? BadgeDispLimitedTime { get; private set; }

	public void SetupProduct()
	{
		this.currentEnumerator = this.SetupProductInternal();
		this.currentEnumerator.MoveNext();
	}

	public List<PurchaseProductOne> GetPurchaseProductList()
	{
		return this.purchaseProductList;
	}

	private List<PurchaseProductOne> MakeSoldoutLimitedItem()
	{
		List<PurchaseProductOne> list = new List<PurchaseProductOne>();
		if (Singleton<LoginManager>.Instance.serviceCloseData != null && Singleton<LoginManager>.Instance.serviceCloseData.IsNoticeServiceClose)
		{
			return list;
		}
		long num = PrjUtil.ConvertTicksToTime(TimeManager.Now.Ticks);
		using (Dictionary<int, PurchaseProductStatic>.ValueCollection.Enumerator enumerator = this.PurchaseProductStaticDataMap.Values.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PurchaseProductStatic staticData = enumerator.Current;
				if (this.purchaseProductList != null && staticData.storeType == LoginManager.Platform && staticData.startTime <= num && staticData.endTime >= num && staticData.purchaseLimitType != 0 && !this.purchaseProductList.Exists((PurchaseProductOne item) => item.productId == staticData.productIdCommon))
				{
					list.Add(new PurchaseProductOne(staticData, null));
				}
			}
		}
		return list;
	}

	public List<List<PurchaseProductOne>> CreateTabPurchaseProductOneListList()
	{
		HomeCheckResult homeCheckResult = DataManager.DmHome.GetHomeCheckResult();
		Sealed @sealed = ((homeCheckResult != null) ? homeCheckResult.sealedData : null);
		List<List<PurchaseProductOne>> list = new List<List<PurchaseProductOne>>();
		if (@sealed != null && 1 == @sealed.stone_buy)
		{
			list = DataManagerPurchase.<CreateTabPurchaseProductOneListList>g__CreatePurchseProductOneList|46_0(new List<PurchaseProductOne>());
		}
		else
		{
			List<PurchaseProductOne> list2 = DataManager.DmPurchase.GetPurchaseProductList();
			List<PurchaseProductOne> list3 = new List<PurchaseProductOne>(list2.FindAll((PurchaseProductOne item) => item.MonthlyPackId == 0));
			foreach (PurchaseProductOne purchaseProductOne in DataManager.DmPurchase.MakeSoldoutLimitedItem())
			{
				if (DataManager.DmPurchase.SoldOutIdList.Contains(purchaseProductOne.productId))
				{
					list3.Add(purchaseProductOne);
				}
			}
			PurchaseProductOne purchaseProductOne2 = list2.Find((PurchaseProductOne item) => item.MonthlyPackId != 0);
			if (purchaseProductOne2 != null)
			{
				list3.Insert(0, purchaseProductOne2);
			}
			else if (this.isPendingMonthlyPack)
			{
				purchaseProductOne2 = new PurchaseProductOne(new PurchaseProductStatic(new MstPurchaseProduct())
				{
					monthlyPackId = -1
				}, null);
				list3.Insert(0, purchaseProductOne2);
			}
			list = DataManagerPurchase.<CreateTabPurchaseProductOneListList>g__CreatePurchseProductOneList|46_0(list3);
		}
		return list;
	}

	public void SolutionPurchase(int productId, string productIdString)
	{
		this.currentEnumerator = this.SolutionPurchaseInternal(productId, productIdString);
		this.currentEnumerator.MoveNext();
	}

	public DataManagerPurchase.PurchaseResult GetPurchaseResult()
	{
		return this.lastPurchaseResult;
	}

	public void UpdateByDataManager()
	{
		if (this.currentEnumerator != null && !this.currentEnumerator.MoveNext())
		{
			this.currentEnumerator = null;
		}
	}

	private IEnumerator currentEnumerator
	{
		get
		{
			return this._currentEnumerator;
		}
		set
		{
			if (value == null || this._currentEnumerator == null)
			{
				this._currentEnumerator = value;
			}
		}
	}

	private SGNFW.InAppPurchase.Manager.PurchaseConfig GetPurchaseConfig()
	{
		return new SGNFW.InAppPurchase.Manager.PurchaseConfig
		{
			language = 0,
			platform_id = 4,
			store_id = 4
		};
	}

	private void PurchaseSuccessCallback(PurchaseResponse response, PurchaseRequest request)
	{
		if (this.lastPurchaseResult == null || !this.lastPurchaseResult.isTargetProduct)
		{
			this.lastPurchaseResult = new DataManagerPurchase.PurchaseResult
			{
				status = DataManagerPurchase.PurchaseResult.Status.SUCCESS,
				productIdString = request.productId
			};
		}
		this.parentData.UpdateUserAssetByAssets(response.assets);
		if (response.new_release_idList != null)
		{
			foreach (int num in response.new_release_idList)
			{
				PurchaseProductStatic purchaseProductStatic = this.PurchaseProductStaticDataMap.TryGetValueEx(num, null);
				if (purchaseProductStatic != null && purchaseProductStatic.storeType == LoginManager.Platform)
				{
					this.lastPurchaseResult.releasePurchaseProductList.Add(purchaseProductStatic);
				}
			}
		}
	}

	private void PurchaseFailedCallback(NativePlugin.TransactionInfo? tinfo, int error_code, string reqProductIdString, Exception exception)
	{
		if (this.lastPurchaseResult != null && this.lastPurchaseResult.isTargetProduct)
		{
			return;
		}
		string text = "";
		SGNFW.InAppPurchase.Manager.InAppPurchaseException ex = exception as SGNFW.InAppPurchase.Manager.InAppPurchaseException;
		if (ex != null)
		{
			switch (ex.ErrorCode)
			{
			case 1:
				text = "商品購入開始時にエラーが発生しました";
				break;
			case 2:
				text = "商品購入開始登録でエラーが発生しました";
				break;
			case 3:
				text = "商品購入が中断されました";
				break;
			case 4:
				text = "商品購入が親権者によって保留されています";
				break;
			case 5:
				text = "商品購入システムの準備でエラーが発生しました";
				break;
			}
			text = text + ":" + exception.Message;
		}
		if (error_code == 302)
		{
			text = text ?? "";
		}
		if (error_code == 1112)
		{
			this.lastPurchaseResult = new DataManagerPurchase.PurchaseResult
			{
				status = DataManagerPurchase.PurchaseResult.Status.FAILURE_PENDING_PURCHASE,
				errorMassage = text,
				productIdString = reqProductIdString
			};
			return;
		}
		this.lastPurchaseResult = new DataManagerPurchase.PurchaseResult
		{
			status = (Command.IsStaticErrorCode(error_code) ? DataManagerPurchase.PurchaseResult.Status.FAILURE_OS_SERVER : DataManagerPurchase.PurchaseResult.Status.FAILURE_APP_SERVER),
			errorMassage = text,
			productIdString = reqProductIdString
		};
	}

	private void PurchaseAbortedCallback(NativePlugin.TransactionInfo? info, SGNFW.InAppPurchase.Manager.PURCHASE_START_RESULT res)
	{
		string text = "";
		switch (res)
		{
		case SGNFW.InAppPurchase.Manager.PURCHASE_START_RESULT.FAILURE:
			text = "FAILURE";
			break;
		case SGNFW.InAppPurchase.Manager.PURCHASE_START_RESULT.REJECTED:
			text = "ゲーム側システムによる拒否（年齢認証など）";
			break;
		case SGNFW.InAppPurchase.Manager.PURCHASE_START_RESULT.ABORTED:
			text = "購入が中断された（ユーザ操作等）";
			break;
		case SGNFW.InAppPurchase.Manager.PURCHASE_START_RESULT.DEFERRED:
			text = "購入が保留された（ファミリー共有など）";
			break;
		case SGNFW.InAppPurchase.Manager.PURCHASE_START_RESULT.BLOCKED:
			text = "レシートがブロックされた（デバッグ機能）";
			break;
		}
		this.lastPurchaseResult = new DataManagerPurchase.PurchaseResult
		{
			status = DataManagerPurchase.PurchaseResult.Status.FAILURE_COMMON,
			errorMassage = text
		};
	}

	public IEnumerator RequestSolutionAgeAuthentic()
	{
		DataManagerPurchase.<>c__DisplayClass60_0 CS$<>8__locals1 = new DataManagerPurchase.<>c__DisplayClass60_0();
		CS$<>8__locals1.isWindowFinish = false;
		CS$<>8__locals1.owAnswer = 0;
		CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("年齢認証が必要です"), PrjUtil.MakeMessage("有料アイテムを購入するには年齢認証が必要です\n1か月あたりの限度額は\n13歳未満が5,000円(pt)\n20歳未満が20,000円(pt)です\n未成年の方は保護者の同意を得てください\n登録情報は年齢認証以外に使用しません"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, delegate(int index)
		{
			CS$<>8__locals1.owAnswer = index;
			CS$<>8__locals1.isWindowFinish = true;
			return true;
		}, null, false);
		CanvasManager.HdlOpenWindowBasic.Open();
		while (!CS$<>8__locals1.isWindowFinish)
		{
			yield return null;
		}
		if (CS$<>8__locals1.owAnswer != 1)
		{
			yield break;
		}
		CS$<>8__locals1 = null;
		DataManagerPurchase.<>c__DisplayClass60_1 CS$<>8__locals2 = new DataManagerPurchase.<>c__DisplayClass60_1();
		CS$<>8__locals2.isWindowFinish = false;
		CS$<>8__locals2.owAnswer = 0;
		BirthdayWindowCtrl.GUI guiData = new BirthdayWindowCtrl.GUI(AssetManager.InstantiateAssetData("Cmn/GUI/Prefab/GUI_Cmn_Window_Age", Singleton<CanvasManager>.Instance.SystemMiddleArea).transform);
		guiData.openWindow.Setup(null, null, null, true, delegate(int index)
		{
			CS$<>8__locals2.owAnswer = index;
			CS$<>8__locals2.isWindowFinish = true;
			return true;
		}, null, false);
		if (this.birthday != null)
		{
			guiData.inputFiledYear.text = this.birthday.Value.Year.ToString();
			guiData.inputFiledMonth.text = this.birthday.Value.Month.ToString();
		}
		guiData.openWindow.Open();
		while (!CS$<>8__locals2.isWindowFinish)
		{
			yield return null;
		}
		int num = int.Parse(guiData.inputFiledYear.text);
		int num2 = int.Parse(guiData.inputFiledMonth.text);
		Object.Destroy(guiData.baseObj);
		if (CS$<>8__locals2.owAnswer != 0)
		{
			yield break;
		}
		CS$<>8__locals2 = null;
		guiData = null;
		bool flag = true;
		if (num < 1900 || num > TimeManager.Now.Year)
		{
			flag = false;
		}
		else if (num == TimeManager.Now.Year && num2 > TimeManager.Now.Month)
		{
			flag = false;
		}
		else if (num2 <= 0 || num2 > 12)
		{
			flag = false;
		}
		if (!flag)
		{
			bool isWindowFinish = false;
			CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("生年月確認"), PrjUtil.MakeMessage("無効な生年月が入力されています\n正しい生年月を入力してください"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int index)
			{
				isWindowFinish = true;
				return true;
			}, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			while (!isWindowFinish)
			{
				yield return null;
			}
			yield break;
		}
		DataManager.DmPurchase.RequestActionUpdateBirthday(num, num2);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		this.parentData.ServerRequest(PurchaseInfoCmd.Create(null, false), new Action<Command>(this.CbPurchaseInfoCmdInternal));
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("認証完了"), PrjUtil.MakeMessage("年齢認証が完了しました"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int index) => true, null, false);
		CanvasManager.HdlOpenWindowBasic.Open();
		while (!CanvasManager.HdlOpenWindowBasic.FinishedClose())
		{
			yield return null;
		}
		yield break;
	}

	public List<PurchaseProductStatic> CreateMstPurchaseProductByQuest(int clearQuestOneId)
	{
		List<PurchaseProductStatic> list = new List<PurchaseProductStatic>();
		long num = PrjUtil.ConvertTicksToTime(TimeManager.Now.Ticks);
		if (Singleton<LoginManager>.Instance.serviceCloseData != null && Singleton<LoginManager>.Instance.serviceCloseData.IsNoticeServiceClose)
		{
			return list;
		}
		foreach (PurchaseProductStatic purchaseProductStatic in this.PurchaseProductStaticDataMap.Values)
		{
			if (purchaseProductStatic.releaseQuestId != 0 && clearQuestOneId == purchaseProductStatic.releaseQuestId && purchaseProductStatic.storeType == LoginManager.Platform && purchaseProductStatic.startTime <= num && purchaseProductStatic.endTime >= num)
			{
				list.Add(purchaseProductStatic);
			}
		}
		return list;
	}

	public void RequestActionUpdateBirthday(int year, int month)
	{
		DateTime dateTime = new DateTime(year, month, 1);
		PlayerInfo playerInfo = this.parentData.MakeProtocolByPlayerInfo();
		playerInfo.birthday = PrjUtil.ConvertTicksToTime(dateTime.Ticks);
		this.parentData.ServerRequest(PlayerInfoChangeCmd.Create(playerInfo), new Action<Command>(this.CbPlayerInfoChangeCmd));
	}

	public void RequestUpdateAddHiddenInfo(int productId)
	{
		if (this.InfoHiddenList.Contains(productId))
		{
			return;
		}
		if (!this.PurchaseProductStaticDataMap.ContainsKey(productId))
		{
			return;
		}
		List<int> list = new List<int>();
		int count = this.InfoHiddenList.Count;
		foreach (int num in this.InfoHiddenList)
		{
			PurchaseProductStatic purchaseProductStatic = this.PurchaseProductStaticDataMap[num];
			DateTime dateTime = new DateTime(PrjUtil.ConvertTimeToTicks(purchaseProductStatic.startTime));
			DateTime dateTime2 = new DateTime(PrjUtil.ConvertTimeToTicks(purchaseProductStatic.endTime));
			if (dateTime <= TimeManager.Now && TimeManager.Now < dateTime2)
			{
				list.Add(num);
			}
		}
		list.Add(productId);
		if (0 < count - list.Count)
		{
			int num2 = count - list.Count;
			for (int i = 0; i < num2; i++)
			{
				list.Add(0);
			}
		}
		List<NewFlg> list2 = new List<NewFlg>();
		int num3 = 0;
		foreach (int num4 in list)
		{
			list2.Add(new NewFlg
			{
				any_id = num3,
				category = 12,
				new_mgmt_flg = num4
			});
			num3++;
		}
		this.parentData.ServerRequest(NewFlgUpdateCmd.Create(list2), new Action<Command>(this.CbNewFlgUpdateCmd));
	}

	public void UpdateBadgeDispLimitedTime(List<Quest> serverQuestList = null, int nowClearQuestOneId = 0)
	{
		this.BadgeDispLimitedTime = null;
		if (serverQuestList != null)
		{
			this.lastUseServerQuestList = serverQuestList;
		}
		if (this.lastUseServerQuestList == null)
		{
			return;
		}
		DateTime now = TimeManager.Now;
		long num = PrjUtil.ConvertTicksToTime(now.Ticks);
		using (Dictionary<int, PurchaseProductStatic>.ValueCollection.Enumerator enumerator = this.PurchaseProductStaticDataMap.Values.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PurchaseProductStatic mst = enumerator.Current;
				if (this.productIdCommonListInternal != null && mst.releaseQuestId != 0 && mst.storeType == LoginManager.Platform && mst.startTime <= num && mst.endTime >= num)
				{
					Quest quest = this.lastUseServerQuestList.Find((Quest item) => item.quest_id == mst.releaseQuestId);
					if (quest != null && quest.first_clear_time != 0L)
					{
						if (!this.productIdCommonListInternal.Contains(mst.productIdCommon))
						{
							if (quest.quest_id != nowClearQuestOneId || quest.clear_num != 1)
							{
								continue;
							}
							this.productIdCommonListInternal.Add(mst.productIdCommon);
						}
						DateTime dateTime = new DateTime(PrjUtil.ConvertTimeToTicks(quest.first_clear_time)).AddHours((double)mst.releaseTime);
						if (this.BadgeDispLimitedTime == null || this.BadgeDispLimitedTime < dateTime)
						{
							this.BadgeDispLimitedTime = new DateTime?(dateTime);
						}
					}
				}
			}
		}
		if (this.productIdCommonListInternal != null)
		{
			foreach (int num2 in this.productIdCommonListInternal)
			{
				PurchaseProductStatic purchaseProductStatic = this.PurchaseProductStaticDataMap.TryGetValueEx(num2, null);
				if (purchaseProductStatic != null)
				{
					DateTime dateTime2 = new DateTime(PrjUtil.ConvertTimeToTicks(purchaseProductStatic.endTime));
					if ((dateTime2 - now).Days < DataManagerPurchase.LimitItemJudgeDays && (this.BadgeDispLimitedTime == null || this.BadgeDispLimitedTime < dateTime2))
					{
						this.BadgeDispLimitedTime = new DateTime?(dateTime2);
					}
				}
			}
		}
	}

	public void InitializeMstData(MstManager mstManager)
	{
		List<MstPurchaseProduct> mst = mstManager.GetMst<List<MstPurchaseProduct>>(MstType.PURCHASE_PRODUCT);
		this.PurchaseProductStaticDataMap = new Dictionary<int, PurchaseProductStatic>();
		this.PurchaseProductStaticDataMap = mst.ToDictionary<MstPurchaseProduct, int, PurchaseProductStatic>((MstPurchaseProduct item) => item.productIdCommon, (MstPurchaseProduct item) => new PurchaseProductStatic(item));
	}

	private IEnumerator SetupProductInternal()
	{
		this.purchaseProductList = null;
		this.IsFinishSetupProduct = false;
		this.parentData.ServerRequest(PurchaseInfoCmd.Create(null, false), new Action<Command>(this.CbPurchaseInfoCmdInternal));
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		this.parentData.lockByPurchase = true;
		bool isSetupFailed = false;
		Singleton<SGNFW.InAppPurchase.Manager>.Instance.PurchaseSuccessDelegate = new SGNFW.InAppPurchase.Manager.SuccessDelegate(this.PurchaseSuccessCallback);
		Singleton<SGNFW.InAppPurchase.Manager>.Instance.PurchaseErrorDelegate = new SGNFW.InAppPurchase.Manager.ErrorDelegate(this.PurchaseFailedCallback);
		Singleton<SGNFW.InAppPurchase.Manager>.Instance.PurchaseAbortedCallback = new Action<NativePlugin.TransactionInfo?, SGNFW.InAppPurchase.Manager.PURCHASE_START_RESULT>(this.PurchaseAbortedCallback);
		Singleton<SGNFW.InAppPurchase.Manager>.Instance.SetupFailedCallback = delegate(string msg)
		{
			isSetupFailed = true;
		};
		if (Singleton<SGNFW.InAppPurchase.Manager>.Instance.IsPrepared())
		{
			Singleton<SGNFW.InAppPurchase.Manager>.Instance.Restart();
		}
		else
		{
			Dictionary<int, string> dictionary = new Dictionary<int, string>();
			foreach (PurchaseProductStatic purchaseProductStatic in this.PurchaseProductStaticDataMap.Values)
			{
				dictionary.Add(purchaseProductStatic.productIdCommon, purchaseProductStatic.productId);
			}
			Singleton<SGNFW.InAppPurchase.Manager>.Instance.StartInitialize(this.GetPurchaseConfig(), new SGNFW.InAppPurchase.Manager.HttpRequestDelegate(this.ServerRequest), dictionary);
		}
		while (!Singleton<SGNFW.InAppPurchase.Manager>.Instance.IsPrepared() && !isSetupFailed)
		{
			yield return null;
		}
		if (!isSetupFailed)
		{
			while (Singleton<SGNFW.InAppPurchase.Manager>.Instance.GetLeftTransactionNum() > 0U)
			{
				yield return null;
			}
		}
		List<PurchaseInfo> list = new List<PurchaseInfo>();
		Singleton<SGNFW.InAppPurchase.Manager>.Instance.GetProductInfo(ref list);
		this.purchaseProductList = new List<PurchaseProductOne>();
		foreach (PurchaseInfo purchaseInfo in list)
		{
			PurchaseProductStatic purchaseProductStatic2 = this.PurchaseProductStaticDataMap.TryGetValueEx(purchaseInfo.productIdCommon, null);
			if (purchaseProductStatic2 != null && !purchaseProductStatic2.IsFreeMonthlyPack)
			{
				this.purchaseProductList.Add(new PurchaseProductOne(purchaseProductStatic2, purchaseInfo));
			}
		}
		foreach (PurchaseInfo purchaseInfo2 in Singleton<SGNFW.InAppPurchase.Manager>.Instance.ProductBaseList)
		{
			PurchaseProductStatic purchaseProductStatic3 = this.PurchaseProductStaticDataMap.TryGetValueEx(purchaseInfo2.productIdCommon, null);
			if (purchaseProductStatic3 != null && purchaseProductStatic3.IsFreeMonthlyPack)
			{
				this.purchaseProductList.Add(new PurchaseProductOne(purchaseProductStatic3, purchaseInfo2));
			}
		}
		this.residuePurchaseNum = Singleton<SGNFW.InAppPurchase.Manager>.Instance.residuePurchaseNum;
		this.IsFinishSetupProduct = true;
		this.parentData.lockByPurchase = false;
		yield break;
	}

	private IEnumerator SolutionPurchaseDMMInternal(int productId, string productIdString)
	{
		DataManagerPurchase.<>c__DisplayClass67_0 CS$<>8__locals1 = new DataManagerPurchase.<>c__DisplayClass67_0();
		CS$<>8__locals1.tgtProductMst = this.PurchaseProductStaticDataMap[productId];
		if (this.residuePurchaseNum != -1 && this.residuePurchaseNum < CS$<>8__locals1.tgtProductMst.priceInTax)
		{
			this.lastPurchaseResult = new DataManagerPurchase.PurchaseResult
			{
				status = DataManagerPurchase.PurchaseResult.Status.FAILURE_LIMIT_AGE,
				productIdString = productIdString
			};
			this.IsFinishPurchase = true;
			TimeManager.EnableTimeCmd = true;
			yield break;
		}
		if (DataManager.DmPurchase.GetPurchaseProductList().Find((PurchaseProductOne item) => item.productId == CS$<>8__locals1.tgtProductMst.productIdCommon).purchasePossibleNum == 1 && DataManager.DmPurchase.PendingIdList.Contains(CS$<>8__locals1.tgtProductMst.productIdCommon))
		{
			this.lastPurchaseResult = new DataManagerPurchase.PurchaseResult
			{
				status = DataManagerPurchase.PurchaseResult.Status.FAILURE_PENDING_COUNT_LIMIT,
				productIdString = productIdString
			};
			this.IsFinishPurchase = true;
			this.parentData.lockByPurchase = false;
			TimeManager.EnableTimeCmd = true;
			yield break;
		}
		CS$<>8__locals1.dmmPoint = 0;
		DataManagerPurchase.<>c__DisplayClass67_1 CS$<>8__locals2 = new DataManagerPurchase.<>c__DisplayClass67_1();
		CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
		CS$<>8__locals2.isPointFinish = false;
		Singleton<DMMHelpManager>.Instance.GetDmmPoint(delegate(int point)
		{
			CS$<>8__locals2.isPointFinish = true;
			CS$<>8__locals2.CS$<>8__locals1.dmmPoint = point;
		});
		while (!CS$<>8__locals2.isPointFinish)
		{
			yield return null;
		}
		CS$<>8__locals2 = null;
		if (CS$<>8__locals1.tgtProductMst.priceInTax > CS$<>8__locals1.dmmPoint)
		{
			if (CS$<>8__locals1.dmmPoint >= 0)
			{
				DataManagerPurchase.<>c__DisplayClass67_2 CS$<>8__locals3 = new DataManagerPurchase.<>c__DisplayClass67_2();
				CS$<>8__locals3.isWindowFinish = false;
				CS$<>8__locals3.isOk = false;
				CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("ポイントチャージ"), PrjUtil.MakeMessage("購入に必要なDMMポイントが不足しています。\nチャージしますか？"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, delegate(int index)
				{
					CS$<>8__locals3.isWindowFinish = true;
					CS$<>8__locals3.isOk = index == 1;
					return true;
				}, null, false);
				CanvasManager.HdlOpenWindowBasic.Open();
				while (!CS$<>8__locals3.isWindowFinish || !CanvasManager.HdlOpenWindowBasic.FinishedClose())
				{
					yield return null;
				}
				if (CS$<>8__locals3.isOk)
				{
					Application.OpenURL("https://point.dmm.com/choice/pay");
				}
				CS$<>8__locals3 = null;
			}
			this.lastPurchaseResult = new DataManagerPurchase.PurchaseResult
			{
				status = ((CS$<>8__locals1.dmmPoint >= 0) ? DataManagerPurchase.PurchaseResult.Status.FAILURE_COMMON : DataManagerPurchase.PurchaseResult.Status.FAILURE_DMM_POINT),
				productIdString = productIdString
			};
			this.IsFinishPurchase = true;
			TimeManager.EnableTimeCmd = true;
			yield break;
		}
		if (!DataManager.DmGameStatus.MakeUserFlagData().TutorialFinishFlag.DMMPurchaseWarning && CS$<>8__locals1.tgtProductMst.priceInTax > 0)
		{
			DateTime now = TimeManager.Now;
			int num = now.Year - this.birthday.Value.Year - ((now.Month < this.birthday.Value.Month) ? 1 : 0);
			if (num < 20)
			{
				DataManagerPurchase.<>c__DisplayClass67_3 CS$<>8__locals4 = new DataManagerPurchase.<>c__DisplayClass67_3();
				CS$<>8__locals4.isWindowFinish = false;
				CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("月額制限額"), PrjUtil.MakeMessage("1か月あたりの購入限度額は、\nスマートフォン、DMMアカウント合わせて\n" + ((num < 13) ? "5,000" : "20,000") + "円(pt)に制限されています"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int index)
				{
					CS$<>8__locals4.isWindowFinish = true;
					return true;
				}, null, false);
				CanvasManager.HdlOpenWindowBasic.Open();
				while (!CS$<>8__locals4.isWindowFinish || !CanvasManager.HdlOpenWindowBasic.FinishedClose())
				{
					yield return null;
				}
				DataManagerGameStatus.UserFlagData userFlagData = DataManager.DmGameStatus.MakeUserFlagData();
				userFlagData.TutorialFinishFlag.DMMPurchaseWarning = true;
				DataManager.DmGameStatus.RequestActionUpdateUserFlag(userFlagData);
				while (DataManager.IsServerRequesting())
				{
					yield return null;
				}
				CS$<>8__locals4 = null;
			}
		}
		DataManagerPurchase.<>c__DisplayClass67_4 CS$<>8__locals5 = new DataManagerPurchase.<>c__DisplayClass67_4();
		CS$<>8__locals5.isFinish = false;
		CS$<>8__locals5.isOk = false;
		string text = string.Concat(new string[]
		{
			"【",
			CS$<>8__locals1.tgtProductMst.productName.Replace("\n", ""),
			"】\nを購入します。よろしいですか？\n\n",
			string.Format("所持DMMポイント：{0}pt\n", CS$<>8__locals1.dmmPoint),
			string.Format("必要DMMポイント：{0}pt\n", CS$<>8__locals1.tgtProductMst.priceInTax),
			"<color=",
			PrjUtil.WARNING_COLOR_CODE,
			">※DMM版で購入した有償キラキラは\nDMM版でのみ使用できます</color>"
		});
		CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage((CS$<>8__locals1.tgtProductMst.monthlyPackId == 0) ? "キラキラ＆パック購入" : "月間パスポート購入"), PrjUtil.MakeMessage(text), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, delegate(int index)
		{
			CS$<>8__locals5.isFinish = true;
			CS$<>8__locals5.isOk = index == 1;
			return true;
		}, null, false);
		CanvasManager.HdlOpenWindowBasic.Open();
		while (!CS$<>8__locals5.isFinish || !CanvasManager.HdlOpenWindowBasic.FinishedClose())
		{
			yield return null;
		}
		if (!CS$<>8__locals5.isOk)
		{
			this.lastPurchaseResult = new DataManagerPurchase.PurchaseResult
			{
				status = DataManagerPurchase.PurchaseResult.Status.FAILURE_COMMON,
				productIdString = productIdString
			};
			this.IsFinishPurchase = true;
			TimeManager.EnableTimeCmd = true;
			yield break;
		}
		CS$<>8__locals5 = null;
		yield return null;
		yield break;
	}

	private IEnumerator SolutionPurchaseInternal(int productId, string productIdString)
	{
		PurchaseProductStatic tgtProductMst = this.PurchaseProductStaticDataMap.TryGetValueEx(productId, null);
		TimeManager.EnableTimeCmd = false;
		this.lastPurchaseResult = null;
		this.IsFinishPurchase = false;
		IEnumerator ienum = this.SolutionPurchaseDMMInternal(productId, productIdString);
		while (ienum.MoveNext())
		{
			yield return null;
		}
		if (this.IsFinishPurchase)
		{
			yield break;
		}
		ienum = null;
		this.parentData.ServerRequest(PurchaseInfoCmd.Create(null, false), new Action<Command>(this.CbPurchaseInfoCmdInternal));
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		if (!this.productIdCommonListInternal.Contains(tgtProductMst.productIdCommon))
		{
			this.lastPurchaseResult = new DataManagerPurchase.PurchaseResult
			{
				status = DataManagerPurchase.PurchaseResult.Status.FAILURE_INTERRUPT,
				productIdString = productIdString
			};
			this.IsFinishPurchase = true;
			TimeManager.EnableTimeCmd = true;
			yield break;
		}
		if (tgtProductMst.IsFreeMonthlyPack)
		{
			this.parentData.ServerRequest(PurchaseCmd.Create(tgtProductMst.productId, "", "", "", 0, "", new List<string>()), delegate(Command cmd)
			{
				this.PurchaseSuccessCallback(cmd.response as PurchaseResponse, cmd.request as PurchaseRequest);
			});
			while (DataManager.IsServerRequesting())
			{
				yield return null;
			}
		}
		else
		{
			this.parentData.lockByPurchase = true;
			if (this.residuePurchaseNum != -1 && this.residuePurchaseNum < tgtProductMst.priceInTax)
			{
				this.lastPurchaseResult = new DataManagerPurchase.PurchaseResult
				{
					status = DataManagerPurchase.PurchaseResult.Status.FAILURE_LIMIT_AGE,
					productIdString = productIdString
				};
				this.IsFinishPurchase = true;
				this.parentData.lockByPurchase = false;
				TimeManager.EnableTimeCmd = true;
				yield break;
			}
			if (DataManager.DmPurchase.GetPurchaseProductList().Find((PurchaseProductOne item) => item.productId == tgtProductMst.productIdCommon).purchasePossibleNum == 1 && DataManager.DmPurchase.PendingIdList.Contains(tgtProductMst.productIdCommon))
			{
				this.lastPurchaseResult = new DataManagerPurchase.PurchaseResult
				{
					status = DataManagerPurchase.PurchaseResult.Status.FAILURE_PENDING_COUNT_LIMIT,
					productIdString = productIdString
				};
				this.IsFinishPurchase = true;
				this.parentData.lockByPurchase = false;
				TimeManager.EnableTimeCmd = true;
				yield break;
			}
			if (Singleton<SGNFW.InAppPurchase.Manager>.Instance.state != SGNFW.InAppPurchase.Manager.State.MAIN || !Singleton<SGNFW.InAppPurchase.Manager>.Instance.SelectProduct(tgtProductMst.productId))
			{
				this.lastPurchaseResult = new DataManagerPurchase.PurchaseResult
				{
					status = DataManagerPurchase.PurchaseResult.Status.FAILURE_INTERRUPT,
					productIdString = productIdString
				};
				this.IsFinishPurchase = true;
				this.parentData.lockByPurchase = false;
				TimeManager.EnableTimeCmd = true;
				yield break;
			}
		}
		while (this.lastPurchaseResult == null)
		{
			yield return null;
		}
		while (this.lastPurchaseResult.productIdString != productIdString && this.lastPurchaseResult.status != DataManagerPurchase.PurchaseResult.Status.FAILURE_COMMON)
		{
			yield return null;
		}
		this.lastPurchaseResult.isTargetProduct = true;
		if (this.lastPurchaseResult.status == DataManagerPurchase.PurchaseResult.Status.FAILURE_APP_SERVER)
		{
			CanvasManager.HdlOpenWindowServerError.Setup("エラー", "購入を失敗しました\nタイトル画面に戻ります", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), false, delegate(int index)
			{
				CanvasManager.HdlCmnMenu.SetupMenu(false, "", null, "", null, null);
				CanvasManager.SetEnableCmnTouchMask(false);
				CanvasManager.HdlMissionProgressCtrl.ClaerProgress();
				Singleton<SceneManager>.Instance.SetSceneReboot();
				return true;
			}, null, false);
			CanvasManager.HdlOpenWindowServerError.ForceOpen();
			yield break;
		}
		if (this.lastPurchaseResult.status == DataManagerPurchase.PurchaseResult.Status.SUCCESS)
		{
			PrjUtil.SendAppsFlyerPurcaseLtvId((double)tgtProductMst.priceInTax, "JPY", tgtProductMst.productId, 1);
		}
		this.parentData.lockByPurchase = false;
		TimeManager.EnableTimeCmd = true;
		if (this.lastPurchaseResult.status == DataManagerPurchase.PurchaseResult.Status.SUCCESS)
		{
			this.parentData.ServerRequest(PurchaseInfoCmd.Create(null, false), new Action<Command>(this.CbPurchaseInfoCmdInternal));
			while (DataManager.IsServerRequesting())
			{
				yield return null;
			}
			if (tgtProductMst.monthlyPackId != 0)
			{
				DataManagerGameStatus.UserFlagData userFlagData = DataManager.DmGameStatus.MakeUserFlagData();
				userFlagData.InformationsFlag.DisableMonthlyPackInfo1 = false;
				userFlagData.InformationsFlag.DisableMonthlyPackInfo2 = false;
				DataManager.DmGameStatus.RequestActionUpdateUserFlag(userFlagData);
				while (DataManager.IsServerRequesting())
				{
					yield return null;
				}
			}
		}
		this.IsFinishPurchase = true;
		yield break;
	}

	private void CbPurchaseInfoCmdInternal(Command cmd)
	{
		PurchaseInfoResponse purchaseInfoResponse = cmd.response as PurchaseInfoResponse;
		this.residuePurchaseNum = purchaseInfoResponse.residuePurchaseNum;
		this.productIdCommonListInternal = purchaseInfoResponse.purchaseInfoList.ConvertAll<int>((PurchaseInfo item) => item.productIdCommon);
		this.SoldOutIdList = purchaseInfoResponse.soldOutIdList;
		this.PendingIdList = purchaseInfoResponse.pendingIdList;
		Singleton<SGNFW.InAppPurchase.Manager>.Instance.UpdateProductList(new List<PurchaseInfo>(purchaseInfoResponse.purchaseInfoList));
		this.isPendingMonthlyPack = purchaseInfoResponse.isPendingMonthlyPack;
		this.UpdateBadgeDispLimitedTime(null, 0);
		this.parentData.UpdateUserAssetByAssets(purchaseInfoResponse.assets);
	}

	private void CbPlayerInfoChangeCmd(Command cmd)
	{
		PlayerInfoChangeResponse playerInfoChangeResponse = cmd.response as PlayerInfoChangeResponse;
		this.parentData.UpdateUserAssetByAssets(playerInfoChangeResponse.assets);
	}

	private void CbNewFlgUpdateCmd(Command cmd)
	{
		NewFlgUpdateRequest newFlgUpdateRequest = cmd.request as NewFlgUpdateRequest;
		this.UpdateUserFlagByServer(newFlgUpdateRequest.new_flg_list);
	}

	public void UpdateUserDataByServer(PlayerInfo playerInfo)
	{
		if (playerInfo.birthday == 0L)
		{
			this.birthday = null;
			return;
		}
		this.birthday = new DateTime?(new DateTime(PrjUtil.ConvertTimeToTicks(playerInfo.birthday)));
	}

	public void UpdateUserFlagByServer(List<NewFlg> newFlagList)
	{
		this.InfoHiddenList = this.InfoHiddenList ?? new List<int>();
		int count = this.InfoHiddenList.Count;
		List<int> list = new List<int>();
		foreach (NewFlg newFlg in newFlagList)
		{
			if (12 == newFlg.category)
			{
				int new_mgmt_flg = newFlg.new_mgmt_flg;
				if (this.PurchaseProductStaticDataMap.ContainsKey(new_mgmt_flg))
				{
					DateTime dateTime = new DateTime(PrjUtil.ConvertTimeToTicks(this.PurchaseProductStaticDataMap[new_mgmt_flg].startTime));
					DateTime dateTime2 = new DateTime(PrjUtil.ConvertTimeToTicks(this.PurchaseProductStaticDataMap[new_mgmt_flg].endTime));
					if (dateTime <= TimeManager.Now && TimeManager.Now < dateTime2)
					{
						list.Add(new_mgmt_flg);
					}
				}
			}
		}
		if (0 < count - list.Count)
		{
			int num = count - list.Count;
			for (int i = 0; i < num; i++)
			{
				list.Add(0);
			}
		}
		this.InfoHiddenList = list;
	}

	public void ServerRequest(Command cmd, Action<Command, int, string> cb)
	{
		Action<Command> action = delegate(Command c)
		{
			cb(cmd, 0, "");
			PurchaseInfoResponse purchaseInfoResponse = cmd.response as PurchaseInfoResponse;
			if (purchaseInfoResponse != null)
			{
				this.parentData.UpdateUserAssetByAssets(purchaseInfoResponse.assets);
			}
		};
		Action<Command> action2 = delegate(Command c)
		{
			cb(cmd, c.response.error_code.id, c.response.error_code.msg);
			cmd.Exit();
		};
		Action<Command, ErrorCode> action3 = delegate(Command c, ErrorCode e)
		{
			cb(cmd, e.id, e.msg);
			cmd.Exit();
		};
		Command.SetupRequest(cmd, action, action2, action3);
	}

	public void RequestGetPurchaseInternalList()
	{
		this.parentData.ServerRequest(PurchaseInfoCmd.Create(null, false), new Action<Command>(this.CbPurchaseInfoCmdInternal));
	}

	[CompilerGenerated]
	internal static List<List<PurchaseProductOne>> <CreateTabPurchaseProductOneListList>g__CreatePurchseProductOneList|46_0(List<PurchaseProductOne> baseProductOneList)
	{
		List<List<PurchaseProductOne>> list = new List<List<PurchaseProductOne>>();
		list.Add(new List<PurchaseProductOne>(baseProductOneList));
		list.Add(baseProductOneList.FindAll((PurchaseProductOne item) => PurchaseProductOne.TabType.Limited == item.tabType));
		list.Add(baseProductOneList.FindAll((PurchaseProductOne item) => PurchaseProductOne.TabType.MonthlyPack == item.tabType));
		list.Add(baseProductOneList.FindAll((PurchaseProductOne item) => PurchaseProductOne.TabType.Kirakira == item.tabType));
		list.Add(baseProductOneList.FindAll((PurchaseProductOne item) => PurchaseProductOne.TabType.Pack == item.tabType));
		return list;
	}

	private DataManager parentData;

	private Dictionary<int, PurchaseProductStatic> PurchaseProductStaticDataMap;

	private List<PurchaseProductOne> purchaseProductList;

	private List<int> productIdCommonListInternal;

	private List<Quest> lastUseServerQuestList;

	private DateTime? birthday;

	private int residuePurchaseNum;

	private bool isPendingMonthlyPack;

	private DataManagerPurchase.PurchaseResult lastPurchaseResult;

	public static readonly int LimitItemJudgeDays = 365;

	private IEnumerator _currentEnumerator;

	private DataManagerPurchase.PurchaseResult purchaseResult;

	public class PurchaseResult
	{
		public DataManagerPurchase.PurchaseResult.Status status;

		public string errorMassage;

		public string productIdString;

		public bool isTargetProduct;

		public List<PurchaseProductStatic> releasePurchaseProductList = new List<PurchaseProductStatic>();

		public enum Status
		{
			INVALID,
			SUCCESS,
			FAILURE_COMMON,
			FAILURE_APP_SERVER,
			FAILURE_OS_SERVER,
			FAILURE_LIMIT_AGE,
			FAILURE_INTERRUPT,
			FAILURE_DMM_POINT,
			FAILURE_PENDING_PURCHASE,
			FAILURE_PENDING_COUNT_LIMIT
		}
	}
}
