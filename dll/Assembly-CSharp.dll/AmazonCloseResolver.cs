using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.Http;
using SGNFW.Login;
using SGNFW.Mst;
using UnityEngine;

// Token: 0x02000123 RID: 291
public class AmazonCloseResolver
{
	// Token: 0x17000339 RID: 825
	// (get) Token: 0x06000EEA RID: 3818 RVA: 0x000B5228 File Offset: 0x000B3428
	// (set) Token: 0x06000EEB RID: 3819 RVA: 0x000B5230 File Offset: 0x000B3430
	public AmazonCloseResolver.Result result { get; private set; }

	// Token: 0x06000EEC RID: 3820 RVA: 0x000B5239 File Offset: 0x000B3439
	public bool ResolveAction(LoginManager.ServiceCloseData closeData, bool isFadeAction)
	{
		if (this.ienum == null)
		{
			this.ienum = this.ResolveActionInternal(closeData, isFadeAction);
		}
		return !this.ienum.MoveNext();
	}

	// Token: 0x06000EED RID: 3821 RVA: 0x000B525F File Offset: 0x000B345F
	private IEnumerator ResolveActionInternal(LoginManager.ServiceCloseData closeData, bool isFadeAction)
	{
		AmazonCloseResolver.<>c__DisplayClass7_0 CS$<>8__locals1 = new AmazonCloseResolver.<>c__DisplayClass7_0();
		CS$<>8__locals1.closeData = closeData;
		CS$<>8__locals1.isRefundFinish = CS$<>8__locals1.closeData.IsFreezeAccount;
		CS$<>8__locals1.nextAction = AmazonCloseResolver.ActionType.INVALID;
		DateTime now = TimeManager.Now;
		List<MstPlatformStatusData> mst = Singleton<MstManager>.Instance.GetMst<List<MstPlatformStatusData>>(MstType.PLATFORM_STATUS_DATA);
		AmazonCloseResolver.<>c__DisplayClass7_0 CS$<>8__locals2 = CS$<>8__locals1;
		MstPlatformStatusData mstPlatformStatusData;
		if (mst == null)
		{
			mstPlatformStatusData = null;
		}
		else
		{
			mstPlatformStatusData = mst.Find((MstPlatformStatusData item) => item.platform == LoginManager.Platform);
		}
		CS$<>8__locals2.mstPlatformStatus = mstPlatformStatusData;
		if (CS$<>8__locals1.mstPlatformStatus == null)
		{
			yield break;
		}
		switch (CS$<>8__locals1.closeData.ServiceClosePhase)
		{
		case LoginManager.ServiceCloseData.ClosePhase.OPEN_NOTICE:
			if (DataManager.DmUserInfo.tutorialSequence != TutorialUtil.Sequence.END)
			{
				CS$<>8__locals1.nextAction = AmazonCloseResolver.ActionType.INVALID;
			}
			else if (now.Year == CS$<>8__locals1.closeData.lastCheckDateTime.Year && now.Month == CS$<>8__locals1.closeData.lastCheckDateTime.Month && now.Day == CS$<>8__locals1.closeData.lastCheckDateTime.Day && DataManager.DmGameStatus.MakeUserFlagData().TutorialFinishFlag.FirstViewAmazonCloseInfo)
			{
				CS$<>8__locals1.nextAction = AmazonCloseResolver.ActionType.INVALID;
			}
			else if (DataManager.DmGameStatus.MakeUserFlagData().InformationsFlag.DisableAmazonCloseInfo)
			{
				CS$<>8__locals1.nextAction = AmazonCloseResolver.ActionType.INVALID;
			}
			else
			{
				CS$<>8__locals1.nextAction = AmazonCloseResolver.ActionType.BEFORE_CLOSE_NOTICE;
			}
			break;
		case LoginManager.ServiceCloseData.ClosePhase.FINISH_REFUND:
			CS$<>8__locals1.nextAction = AmazonCloseResolver.ActionType.AFTER_CLOSE_NOTICE_A;
			break;
		case LoginManager.ServiceCloseData.ClosePhase.FINISH:
			CS$<>8__locals1.nextAction = AmazonCloseResolver.ActionType.AFTER_CLOSE_NOTICE_B;
			break;
		}
		if (CS$<>8__locals1.nextAction == AmazonCloseResolver.ActionType.INVALID)
		{
			yield break;
		}
		if (isFadeAction)
		{
			CanvasManager.RestartFade();
			CanvasManager.SwitchLayerHdlLoadAndTipsCtrl(SceneManager.CanvasType.SYSTEM);
			CanvasManager.HdlLoadAndTipsCtrl.Close(true);
		}
		CS$<>8__locals1.guiData = new AmazonCloseResolver.GUI(AssetManager.InstantiateAssetData("SceneTitle/GUI/Prefab/Window_AmazonCloseInfo", Singleton<CanvasManager>.Instance.SystemMiddleArea).transform);
		for (;;)
		{
			AmazonCloseResolver.<>c__DisplayClass7_1 CS$<>8__locals3 = new AmazonCloseResolver.<>c__DisplayClass7_1();
			CS$<>8__locals3.CS$<>8__locals1 = CS$<>8__locals1;
			AmazonCloseResolver.ActionType nextAction = CS$<>8__locals3.CS$<>8__locals1.nextAction;
			if (nextAction == AmazonCloseResolver.ActionType.INVALID)
			{
				break;
			}
			CS$<>8__locals3.isOpenCustomerBrowser = false;
			switch (nextAction)
			{
			case AmazonCloseResolver.ActionType.BEFORE_CLOSE_NOTICE:
			{
				PguiOpenWindowCtrl owCtrl = CS$<>8__locals3.CS$<>8__locals1.guiData.owCtrl;
				string text = "データ連携・引き継ぎ";
				string text2 = CS$<>8__locals3.CS$<>8__locals1.mstPlatformStatus.text01;
				List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>> list = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>();
				list.Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.NEGATIVE, "パスワード設定"));
				list.Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, "ゲーム開始"));
				bool flag = false;
				PguiOpenWindowCtrl.Callback callback;
				if ((callback = CS$<>8__locals3.CS$<>8__locals1.<>9__1) == null)
				{
					callback = (CS$<>8__locals3.CS$<>8__locals1.<>9__1 = delegate(int index)
					{
						CS$<>8__locals3.CS$<>8__locals1.nextAction = ((index == 0) ? AmazonCloseResolver.ActionType.TRANSFER_SETTING : AmazonCloseResolver.ActionType.INVALID);
						return true;
					});
				}
				owCtrl.Setup(text, text2, list, flag, callback, null, false);
				CS$<>8__locals3.CS$<>8__locals1.guiData.info01.baseObj.gameObject.SetActive(true);
				CS$<>8__locals3.CS$<>8__locals1.guiData.info02.baseObj.gameObject.SetActive(false);
				CS$<>8__locals3.CS$<>8__locals1.guiData.info03.baseObj.gameObject.SetActive(false);
				PguiButtonCtrl btn_CheckBox = CS$<>8__locals3.CS$<>8__locals1.guiData.info01.Btn_CheckBox;
				PguiButtonCtrl.OnClick onClick;
				if ((onClick = CS$<>8__locals3.CS$<>8__locals1.<>9__2) == null)
				{
					onClick = (CS$<>8__locals3.CS$<>8__locals1.<>9__2 = delegate(PguiButtonCtrl btn)
					{
						DataManagerGameStatus.UserFlagData userFlagData2 = DataManager.DmGameStatus.MakeUserFlagData();
						userFlagData2.InformationsFlag.DisableAmazonCloseInfo = !CS$<>8__locals3.CS$<>8__locals1.guiData.info01.Img_Check.gameObject.activeSelf;
						DataManager.DmGameStatus.RequestActionUpdateUserFlag(userFlagData2);
						CS$<>8__locals3.CS$<>8__locals1.guiData.info01.Img_Check.gameObject.SetActive(!CS$<>8__locals3.CS$<>8__locals1.guiData.info01.Img_Check.gameObject.activeSelf);
					});
				}
				btn_CheckBox.AddOnClickListener(onClick, PguiButtonCtrl.SoundType.DEFAULT);
				DataManagerGameStatus.UserFlagData userFlagData = DataManager.DmGameStatus.MakeUserFlagData();
				userFlagData.TutorialFinishFlag.FirstViewAmazonCloseInfo = true;
				DataManager.DmGameStatus.RequestActionUpdateUserFlag(userFlagData);
				CS$<>8__locals3.CS$<>8__locals1.guiData.owCtrl.Open();
				break;
			}
			case AmazonCloseResolver.ActionType.TRANSFER_SETTING:
			{
				PguiOpenWindowCtrl owCtrl2 = CS$<>8__locals3.CS$<>8__locals1.guiData.owCtrl;
				string text3 = "データ連携・引き継ぎ";
				string text4 = "";
				List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>> list2 = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>();
				list2.Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, CS$<>8__locals3.CS$<>8__locals1.closeData.IsServiceClose ? "戻る" : "ゲーム開始"));
				bool flag2 = true;
				PguiOpenWindowCtrl.Callback callback2;
				if ((callback2 = CS$<>8__locals3.CS$<>8__locals1.<>9__3) == null)
				{
					callback2 = (CS$<>8__locals3.CS$<>8__locals1.<>9__3 = delegate(int index)
					{
						if (CS$<>8__locals3.CS$<>8__locals1.closeData.IsServiceClose)
						{
							CS$<>8__locals3.CS$<>8__locals1.nextAction = AmazonCloseResolver.ActionType.AFTER_CLOSE_NOTICE_A;
						}
						else
						{
							CS$<>8__locals3.CS$<>8__locals1.nextAction = ((index == -1) ? AmazonCloseResolver.ActionType.BEFORE_CLOSE_NOTICE : AmazonCloseResolver.ActionType.INVALID);
						}
						return true;
					});
				}
				owCtrl2.Setup(text3, text4, list2, flag2, callback2, null, false);
				CS$<>8__locals3.CS$<>8__locals1.guiData.info01.baseObj.gameObject.SetActive(false);
				CS$<>8__locals3.CS$<>8__locals1.guiData.info02.baseObj.gameObject.SetActive(true);
				CS$<>8__locals3.CS$<>8__locals1.guiData.info02.selTransferCtrl.Setup(false);
				if (CS$<>8__locals3.CS$<>8__locals1.closeData.IsServiceClose)
				{
					CS$<>8__locals3.CS$<>8__locals1.guiData.info02.selTransferCtrl.InfoMassageTextCtrl.text = "データ連携・引き継ぎ先にプレイ中のデータが存在する場合、当該データはプレイできない状態となります。";
				}
				CS$<>8__locals3.CS$<>8__locals1.guiData.info03.baseObj.gameObject.SetActive(false);
				CS$<>8__locals3.CS$<>8__locals1.guiData.owCtrl.Open();
				break;
			}
			case AmazonCloseResolver.ActionType.AFTER_CLOSE_NOTICE_A:
			{
				List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>> list3 = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>();
				list3.Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.NEGATIVE, CS$<>8__locals3.CS$<>8__locals1.isRefundFinish ? "払い戻し窓口" : "払い戻し"));
				list3.Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.NEGATIVE, "お知らせ"));
				if (!CS$<>8__locals3.CS$<>8__locals1.isRefundFinish)
				{
					list3.Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.NEGATIVE, "データ連携・引き継ぎ"));
				}
				PguiOpenWindowCtrl owCtrl3 = CS$<>8__locals3.CS$<>8__locals1.guiData.owCtrl;
				string text5 = (CS$<>8__locals3.CS$<>8__locals1.isRefundFinish ? "払い戻し受付" : "払い戻しについて");
				string text6 = "";
				List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>> list4 = list3;
				bool flag3 = false;
				PguiOpenWindowCtrl.Callback callback3;
				if ((callback3 = CS$<>8__locals3.CS$<>8__locals1.<>9__4) == null)
				{
					callback3 = (CS$<>8__locals3.CS$<>8__locals1.<>9__4 = delegate(int index)
					{
						bool flag5 = true;
						switch (index)
						{
						case 0:
							if (CS$<>8__locals3.CS$<>8__locals1.isRefundFinish)
							{
								flag5 = false;
								Application.OpenURL(CS$<>8__locals3.CS$<>8__locals1.mstPlatformStatus.phase2Url02);
							}
							else
							{
								CS$<>8__locals3.CS$<>8__locals1.nextAction = AmazonCloseResolver.ActionType.FREEZE_USER;
							}
							break;
						case 1:
							flag5 = false;
							CanvasManager.HdlWebViewWindowCtrl.Open(CS$<>8__locals3.CS$<>8__locals1.mstPlatformStatus.phase2Url);
							break;
						case 2:
							CS$<>8__locals3.CS$<>8__locals1.nextAction = AmazonCloseResolver.ActionType.TRANSFER_SETTING;
							break;
						}
						return flag5;
					});
				}
				owCtrl3.Setup(text5, text6, list4, flag3, callback3, null, false);
				CS$<>8__locals3.CS$<>8__locals1.guiData.info01.baseObj.gameObject.SetActive(false);
				CS$<>8__locals3.CS$<>8__locals1.guiData.info02.baseObj.gameObject.SetActive(false);
				CS$<>8__locals3.CS$<>8__locals1.guiData.info03.baseObj.gameObject.SetActive(true);
				CS$<>8__locals3.CS$<>8__locals1.guiData.info03.Txt_Label01.ReplaceTextByDefault("Param01", DataManager.DmUserInfo.userName);
				CS$<>8__locals3.CS$<>8__locals1.guiData.info03.Txt_Label02.ReplaceTextByDefault("Param01", CS$<>8__locals3.CS$<>8__locals1.closeData.StoneChargeNum.ToString());
				CS$<>8__locals3.CS$<>8__locals1.guiData.info03.Txt_UserName.text = CS$<>8__locals3.CS$<>8__locals1.closeData.RepaymentID;
				CS$<>8__locals3.CS$<>8__locals1.guiData.info03.Txt_Ruby.text = SelTransferCtrl.MakeTransferIdRuby(CS$<>8__locals3.CS$<>8__locals1.closeData.RepaymentID);
				PguiButtonCtrl btn_Copy = CS$<>8__locals3.CS$<>8__locals1.guiData.info03.Btn_Copy;
				PguiButtonCtrl.OnClick onClick2;
				if ((onClick2 = CS$<>8__locals3.CS$<>8__locals1.<>9__5) == null)
				{
					onClick2 = (CS$<>8__locals3.CS$<>8__locals1.<>9__5 = delegate(PguiButtonCtrl btn)
					{
						GUIUtility.systemCopyBuffer = CS$<>8__locals3.CS$<>8__locals1.closeData.RepaymentID;
						CanvasManager.HdlOpenWindowBasic.Setup("", PrjUtil.MakeMessage("コピーしました"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, null, null, false);
						CanvasManager.HdlOpenWindowBasic.Open();
					});
				}
				btn_Copy.AddOnClickListener(onClick2, PguiButtonCtrl.SoundType.DEFAULT);
				CS$<>8__locals3.CS$<>8__locals1.guiData.info03.Massage01.text = (CS$<>8__locals3.CS$<>8__locals1.isRefundFinish ? CS$<>8__locals3.CS$<>8__locals1.mstPlatformStatus.text06 : CS$<>8__locals3.CS$<>8__locals1.mstPlatformStatus.text02);
				CS$<>8__locals3.CS$<>8__locals1.guiData.info03.Massage02.text = (CS$<>8__locals3.CS$<>8__locals1.isRefundFinish ? "" : CS$<>8__locals3.CS$<>8__locals1.mstPlatformStatus.text03);
				CS$<>8__locals3.CS$<>8__locals1.guiData.owCtrl.Open();
				break;
			}
			case AmazonCloseResolver.ActionType.AFTER_CLOSE_NOTICE_B:
			{
				string text7 = CS$<>8__locals3.CS$<>8__locals1.mstPlatformStatus.text07 ?? "";
				PguiOpenWindowCtrl owCtrl4 = CS$<>8__locals3.CS$<>8__locals1.guiData.owCtrl;
				string text8 = "サービス終了";
				string text9 = text7;
				List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>> list5 = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>();
				list5.Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.NEGATIVE, "お知らせ"));
				bool flag4 = false;
				PguiOpenWindowCtrl.Callback callback4;
				if ((callback4 = CS$<>8__locals3.CS$<>8__locals1.<>9__8) == null)
				{
					callback4 = (CS$<>8__locals3.CS$<>8__locals1.<>9__8 = delegate(int index)
					{
						CanvasManager.HdlWebViewWindowCtrl.Open(CS$<>8__locals3.CS$<>8__locals1.mstPlatformStatus.phase3Url);
						return false;
					});
				}
				owCtrl4.Setup(text8, text9, list5, flag4, callback4, null, false);
				CS$<>8__locals3.CS$<>8__locals1.guiData.info01.baseObj.gameObject.SetActive(false);
				CS$<>8__locals3.CS$<>8__locals1.guiData.info02.baseObj.gameObject.SetActive(false);
				CS$<>8__locals3.CS$<>8__locals1.guiData.info03.baseObj.gameObject.SetActive(false);
				CS$<>8__locals3.CS$<>8__locals1.guiData.owCtrl.Open();
				break;
			}
			case AmazonCloseResolver.ActionType.FREEZE_USER:
			{
				string text10 = CS$<>8__locals3.CS$<>8__locals1.mstPlatformStatus.text04 + "\n\n" + CS$<>8__locals3.CS$<>8__locals1.mstPlatformStatus.text05;
				CS$<>8__locals3.CS$<>8__locals1.guiData.owCtrl.Setup("確認", text10, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), false, delegate(int index)
				{
					CS$<>8__locals3.CS$<>8__locals1.nextAction = AmazonCloseResolver.ActionType.AFTER_CLOSE_NOTICE_A;
					if (index == 1)
					{
						Singleton<LoginManager>.Instance.FreezeUserAmazonAccount(delegate(Command cmd)
						{
						});
						CS$<>8__locals3.CS$<>8__locals1.isRefundFinish = true;
						CS$<>8__locals3.isOpenCustomerBrowser = true;
					}
					return true;
				}, null, false);
				CS$<>8__locals3.CS$<>8__locals1.guiData.info01.baseObj.gameObject.SetActive(false);
				CS$<>8__locals3.CS$<>8__locals1.guiData.info02.baseObj.gameObject.SetActive(false);
				CS$<>8__locals3.CS$<>8__locals1.guiData.info03.baseObj.gameObject.SetActive(false);
				CS$<>8__locals3.CS$<>8__locals1.guiData.owCtrl.Open();
				break;
			}
			}
			while (!CS$<>8__locals3.CS$<>8__locals1.guiData.owCtrl.FinishedOpen())
			{
				yield return null;
			}
			while (!CS$<>8__locals3.CS$<>8__locals1.guiData.owCtrl.FinishedClose())
			{
				yield return null;
			}
			while (DataManager.IsServerRequesting())
			{
				yield return null;
			}
			if (CS$<>8__locals3.isOpenCustomerBrowser)
			{
				Application.OpenURL(CS$<>8__locals3.CS$<>8__locals1.mstPlatformStatus.phase2Url02);
			}
			CS$<>8__locals3 = null;
		}
		Object.Destroy(CS$<>8__locals1.guiData.owCtrl.gameObject);
		if (isFadeAction)
		{
			CanvasManager.RequestFade(CanvasManager.FadeType.NORMAL);
			do
			{
				yield return null;
			}
			while (!CanvasManager.IsFinishFadeAction);
		}
		yield return null;
		yield break;
	}

	// Token: 0x04000D8B RID: 3467
	private IEnumerator ienum;

	// Token: 0x0200093F RID: 2367
	private enum ActionType
	{
		// Token: 0x04003C46 RID: 15430
		INVALID,
		// Token: 0x04003C47 RID: 15431
		BEFORE_CLOSE_NOTICE,
		// Token: 0x04003C48 RID: 15432
		TRANSFER_SETTING,
		// Token: 0x04003C49 RID: 15433
		AFTER_CLOSE_NOTICE_A,
		// Token: 0x04003C4A RID: 15434
		AFTER_CLOSE_NOTICE_B,
		// Token: 0x04003C4B RID: 15435
		FREEZE_USER
	}

	// Token: 0x02000940 RID: 2368
	public class Result
	{
	}

	// Token: 0x02000941 RID: 2369
	private class GUI
	{
		// Token: 0x06003B38 RID: 15160 RVA: 0x001D431C File Offset: 0x001D251C
		public GUI(Transform baseTr)
		{
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.info01 = new AmazonCloseResolver.GUI.Info01(baseTr.Find("Base/Window/Info01"));
			this.info02 = new AmazonCloseResolver.GUI.Info02(baseTr.Find("Base/Window/Info02"));
			this.info03 = new AmazonCloseResolver.GUI.Info03(baseTr.Find("Base/Window/Info03"));
		}

		// Token: 0x04003C4C RID: 15436
		public PguiOpenWindowCtrl owCtrl;

		// Token: 0x04003C4D RID: 15437
		public AmazonCloseResolver.GUI.Info01 info01;

		// Token: 0x04003C4E RID: 15438
		public AmazonCloseResolver.GUI.Info02 info02;

		// Token: 0x04003C4F RID: 15439
		public AmazonCloseResolver.GUI.Info03 info03;

		// Token: 0x0200114C RID: 4428
		public class Info01
		{
			// Token: 0x06005591 RID: 21905 RVA: 0x0024F404 File Offset: 0x0024D604
			public Info01(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Btn_CheckBox = baseTr.Find("Btn_CheckBox").GetComponent<PguiButtonCtrl>();
				this.Img_Check = baseTr.Find("Btn_CheckBox/BaseImage/Img_Check").GetComponent<PguiImageCtrl>();
				this.Img_Check.gameObject.SetActive(false);
			}

			// Token: 0x04005F0B RID: 24331
			public GameObject baseObj;

			// Token: 0x04005F0C RID: 24332
			public PguiButtonCtrl Btn_CheckBox;

			// Token: 0x04005F0D RID: 24333
			public PguiImageCtrl Img_Check;
		}

		// Token: 0x0200114D RID: 4429
		public class Info02
		{
			// Token: 0x06005592 RID: 21906 RVA: 0x0024F460 File Offset: 0x0024D660
			public Info02(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.selTransferCtrl = this.baseObj.AddComponent<SelTransferCtrl>();
				this.selTransferCtrl.Init();
			}

			// Token: 0x04005F0E RID: 24334
			public GameObject baseObj;

			// Token: 0x04005F0F RID: 24335
			public SelTransferCtrl selTransferCtrl;
		}

		// Token: 0x0200114E RID: 4430
		public class Info03
		{
			// Token: 0x06005593 RID: 21907 RVA: 0x0024F490 File Offset: 0x0024D690
			public Info03(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Btn_Copy = baseTr.Find("panel03/Btn_Copy").GetComponent<PguiButtonCtrl>();
				this.Txt_Label01 = baseTr.Find("panel01/Txt_Label").GetComponent<PguiTextCtrl>();
				this.Txt_Label02 = baseTr.Find("panel02/Txt_Label").GetComponent<PguiTextCtrl>();
				this.Txt_UserName = baseTr.Find("panel03/Img_line_1/Txt_UserName").GetComponent<PguiTextCtrl>();
				this.Txt_Ruby = baseTr.Find("panel03/Img_line_1/Txt_Ruby").GetComponent<PguiTextCtrl>();
				this.Massage01 = baseTr.Find("panel04/Massage01").GetComponent<PguiTextCtrl>();
				this.Massage02 = baseTr.Find("panel04/Massage02").GetComponent<PguiTextCtrl>();
			}

			// Token: 0x04005F10 RID: 24336
			public GameObject baseObj;

			// Token: 0x04005F11 RID: 24337
			public PguiButtonCtrl Btn_Copy;

			// Token: 0x04005F12 RID: 24338
			public PguiTextCtrl Txt_Label01;

			// Token: 0x04005F13 RID: 24339
			public PguiTextCtrl Txt_Label02;

			// Token: 0x04005F14 RID: 24340
			public PguiTextCtrl Txt_UserName;

			// Token: 0x04005F15 RID: 24341
			public PguiTextCtrl Txt_Ruby;

			// Token: 0x04005F16 RID: 24342
			public PguiTextCtrl Massage01;

			// Token: 0x04005F17 RID: 24343
			public PguiTextCtrl Massage02;
		}
	}
}
