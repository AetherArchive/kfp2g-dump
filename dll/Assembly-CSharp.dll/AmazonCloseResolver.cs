using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.Http;
using SGNFW.Login;
using SGNFW.Mst;
using UnityEngine;

public class AmazonCloseResolver
{
	public AmazonCloseResolver.Result result { get; private set; }

	public bool ResolveAction(LoginManager.ServiceCloseData closeData, bool isFadeAction)
	{
		if (this.ienum == null)
		{
			this.ienum = this.ResolveActionInternal(closeData, isFadeAction);
		}
		return !this.ienum.MoveNext();
	}

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

	private IEnumerator ienum;

	private enum ActionType
	{
		INVALID,
		BEFORE_CLOSE_NOTICE,
		TRANSFER_SETTING,
		AFTER_CLOSE_NOTICE_A,
		AFTER_CLOSE_NOTICE_B,
		FREEZE_USER
	}

	public class Result
	{
	}

	private class GUI
	{
		public GUI(Transform baseTr)
		{
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.info01 = new AmazonCloseResolver.GUI.Info01(baseTr.Find("Base/Window/Info01"));
			this.info02 = new AmazonCloseResolver.GUI.Info02(baseTr.Find("Base/Window/Info02"));
			this.info03 = new AmazonCloseResolver.GUI.Info03(baseTr.Find("Base/Window/Info03"));
		}

		public PguiOpenWindowCtrl owCtrl;

		public AmazonCloseResolver.GUI.Info01 info01;

		public AmazonCloseResolver.GUI.Info02 info02;

		public AmazonCloseResolver.GUI.Info03 info03;

		public class Info01
		{
			public Info01(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Btn_CheckBox = baseTr.Find("Btn_CheckBox").GetComponent<PguiButtonCtrl>();
				this.Img_Check = baseTr.Find("Btn_CheckBox/BaseImage/Img_Check").GetComponent<PguiImageCtrl>();
				this.Img_Check.gameObject.SetActive(false);
			}

			public GameObject baseObj;

			public PguiButtonCtrl Btn_CheckBox;

			public PguiImageCtrl Img_Check;
		}

		public class Info02
		{
			public Info02(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.selTransferCtrl = this.baseObj.AddComponent<SelTransferCtrl>();
				this.selTransferCtrl.Init();
			}

			public GameObject baseObj;

			public SelTransferCtrl selTransferCtrl;
		}

		public class Info03
		{
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

			public GameObject baseObj;

			public PguiButtonCtrl Btn_Copy;

			public PguiTextCtrl Txt_Label01;

			public PguiTextCtrl Txt_Label02;

			public PguiTextCtrl Txt_UserName;

			public PguiTextCtrl Txt_Ruby;

			public PguiTextCtrl Massage01;

			public PguiTextCtrl Massage02;
		}
	}
}
