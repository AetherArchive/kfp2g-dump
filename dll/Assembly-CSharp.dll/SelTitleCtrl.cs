using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SGNFW.Ab;
using SGNFW.Common;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Login;
using SGNFW.Mst;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelTitleCtrl : MonoBehaviour
{
	public void Init()
	{
		this.guiData = new SelTitleCtrl.GUI(AssetManager.InstantiateAssetData("SceneTitle/GUI/Prefab/GUI_Title", base.transform).transform);
		this.guiData.baseObj.transform.SetAsFirstSibling();
		this.winData = new SelTitleCtrl.WIN(AssetManager.InstantiateAssetData("SceneTitle/GUI/Prefab/GUI_Title_Window", base.transform).transform);
		this.googleWindowData = new SelTitleCtrl.GoogleAccountWindow(AssetManager.InstantiateAssetData("SceneTitle/GUI/Prefab/GUI_Title_Google_Account_Window", base.transform).transform);
		PguiPanel component = this.winData.baseObj.GetComponent<PguiPanel>();
		if (component != null)
		{
			component.raycastTarget = false;
		}
		this.guiData.Btn_Termsofuse.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Btn_Dataid.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.winData.Btn_Graphic.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.winData.Btn_Recovery.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.winData.Btn_Migration.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.winData.Btn_DeleteAccount.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.winData.Tgl_High.AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickToggle));
		this.winData.Tgl_Normal.AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickToggle));
		this.winData.Tgl_Light.AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickToggle));
		this.googleWindowData.Btn_idPassLink.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.googleWindowData.Btn_accountLink.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.googleWindowData.Btn_release.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.googleWindowData.Btn_setting_close.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.baseObj.transform.Find("Auth_Title/BG_All/Base_Img").gameObject.AddComponent<PguiTouchTrigger>().AddListener(delegate
		{
			if (this.openingEnumerator == null && this.settingEnumerator == null && this.graphicEnumerator == null && this.recoveryEnumerator == null && this.migrationEnumerator == null && this.deleteAccountEnumerator == null && CanvasManager.HdlWebViewWindowCtrl.FinishedClose())
			{
				this.gotoNextScene(false);
			}
		}, null, null, null, null);
		this.setting = SelTitleCtrl.PHASE.NONE;
	}

	public void Setup(UnityAction<bool> gotoNextScene)
	{
		this.guiData.Num_Txt_Id.text = LoginManager.FriendCode.ToString();
		string text = LoginManager.AssetBundleURL;
		text = text.Substring(text.LastIndexOf("/") + 1);
		this.guiData.Num_Txt_Ver.text = "ver" + DataInitializeResolver.ServerEnv.ver + "_" + text;
		this.isSetupBanner = false;
		this.gotoNextScene = gotoNextScene;
		this.guiData.AEImage_Title.gameObject.SetActive(true);
		this.guiData.logo.gameObject.SetActive(true);
		this.guiData.logo.SetTexture(AssetManager.GetAssetData(DataInitializeResolver.titleLogo) as Texture2D, false);
		this.openingEnumerator = this.Opening();
	}

	public void SetEnableTransferButton(bool e)
	{
		SelTitleCtrl.WIN win = this.winData;
		if (win == null)
		{
			return;
		}
		win.Btn_Migration.gameObject.SetActive(e);
	}

	public void SetupGraphic()
	{
		this.setting = SelTitleCtrl.PHASE.NONE;
		this.graphicEnumerator = this.Graphic();
	}

	public bool CheckGraphic()
	{
		return this.graphicEnumerator == null;
	}

	private IEnumerator Opening()
	{
		this.guiData.AEImage_Title.PauseAnimation(PguiAECtrl.AmimeType.START, null);
		yield return null;
		this.guiData.AEImage_Title.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
		{
			this.guiData.AEImage_Title.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
		});
		PguiAECtrl.AnimeParam animeParam = this.guiData.AEImage_Title.m_AnimeParam.Find((PguiAECtrl.AnimeParam itm) => itm.type == PguiAECtrl.AmimeType.START_SUB);
		float f = ((animeParam == null) ? 1f : animeParam.start);
		while (this.guiData.AEImage_Title.m_AEImage.playTime < f)
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator Setting()
	{
		this.winData.setting.Setup(null, null, null, true, null, null, false);
		this.winData.setting.Open();
		this.setting = SelTitleCtrl.PHASE.NONE;
		yield return null;
		while (this.setting == SelTitleCtrl.PHASE.NONE && !this.winData.setting.FinishedClose())
		{
			yield return null;
		}
		this.winData.setting.ForceClose();
		yield break;
	}

	private IEnumerator GoogleSetting()
	{
		GPGLoginResponse response = null;
		bool isAuthFailed = false;
		Singleton<LoginManager>.Instance.LoginGooglePlayGames(delegate(GPGLoginResponse action)
		{
			response = action;
		}, delegate(bool auth)
		{
			isAuthFailed = auth;
		});
		bool flag = response != null && response.result == 4 && LoginManager.FriendCode == response.after_friend_code;
		this.googleWindowData.linkedText.text = (flag ? "連携済み" : "未連携");
		this.googleWindowData.setting.Setup(null, null, null, true, null, null, false);
		this.googleWindowData.setting.Open();
		this.setting = SelTitleCtrl.PHASE.NONE;
		yield return null;
		while (this.setting == SelTitleCtrl.PHASE.NONE && !this.googleWindowData.setting.FinishedClose())
		{
			yield return null;
		}
		this.googleWindowData.setting.ForceClose();
		yield break;
	}

	private IEnumerator Graphic()
	{
		int[] option = SceneManager.GetOption();
		this.winData.Tgl_High.SetActEnable(true);
		this.winData.Tgl_Normal.SetActEnable(true);
		this.winData.Tgl_Light.SetActEnable(true);
		this.winData.Tgl_High.SetToggleIndex((option[1] == 2) ? 1 : 0);
		this.winData.Tgl_Normal.SetToggleIndex((option[1] == 1) ? 1 : 0);
		this.winData.Tgl_Light.SetToggleIndex((option[1] == 0) ? 1 : 0);
		this.winData.graphic.SetupTitleGraphic();
		this.winData.graphic.Open();
		yield return null;
		while (!this.winData.graphic.FinishedClose())
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator Recovery()
	{
		SelTitleCtrl.<>c__DisplayClass29_0 CS$<>8__locals1 = new SelTitleCtrl.<>c__DisplayClass29_0();
		CS$<>8__locals1.sel = 0;
		string text = "追加データが破損している場合、\n一度データを削除することで、\n現象が改善する場合があります。\n※プレイデータは消えません\n\n追加データを削除しますか？";
		CanvasManager.HdlOpenWindowBasic.Setup("データ復旧", text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, delegate(int index)
		{
			CS$<>8__locals1.sel = index;
			return true;
		}, null, false);
		CanvasManager.HdlOpenWindowBasic.Open();
		do
		{
			yield return null;
		}
		while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
		if (CS$<>8__locals1.sel != 1)
		{
			yield break;
		}
		CS$<>8__locals1 = null;
		this.guiData.AEImage_Title.gameObject.SetActive(false);
		this.guiData.logo.gameObject.SetActive(false);
		Singleton<AssetManager>.Instance.SetAbChkType(AssetManager.abCheckType.OFF);
		CanvasManager.HdlLoadAndTipsCtrl.SetDispLoading(true);
		SoundManager.StopBGM();
		yield return null;
		SoundManager.UnloadCueSheetAll();
		yield return null;
		SGNFW.Ab.Manager.UnloadAll(true);
		yield return null;
		List<Data> list = new List<Data>(SGNFW.Ab.Manager.DataList);
		while (list.Count > 0)
		{
			List<Data> list2 = new List<Data>((list.Count > 100) ? list.GetRange(0, 100) : list);
			list.RemoveRange(0, list2.Count);
			SGNFW.Ab.Manager.RemoveCacheFile(list2);
			yield return null;
		}
		SGNFW.Ab.Manager.RemoveCaches();
		yield return null;
		Singleton<MstManager>.Instance.RemoveMstDataCaches();
		yield return null;
		CanvasManager.HdlLoadAndTipsCtrl.SetDispLoading(false);
		string text2 = "追加データを削除しました\n※プレイデータは消えていません\n\nゲームを再起動します";
		CanvasManager.HdlOpenWindowBasic.Setup("データ復旧", text2, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int index) => true, null, false);
		CanvasManager.HdlOpenWindowBasic.Open();
		do
		{
			yield return null;
		}
		while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
		this.setting = SelTitleCtrl.PHASE.NONE;
		yield break;
	}

	private IEnumerator Migration()
	{
		if (LoginManager.IsDmmLink)
		{
			string text = "このDMMアカウントは連携済みです。\n\n１つのDMMアカウントは\n１度のみデータ連携することができます。";
			CanvasManager.HdlOpenWindowBasic.Setup("データ連携", text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), false, (int index) => true, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			do
			{
				yield return null;
			}
			while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
			yield break;
		}
		string text2 = "DMMアカウント同士での\n引き継ぎを行うことはできません。\n\nまた、１つのDMMアカウントは\n１度のみデータ連携することができます。";
		CanvasManager.HdlOpenWindowBasic.Setup("データ連携", text2, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), false, (int index) => true, null, false);
		CanvasManager.HdlOpenWindowBasic.Open();
		do
		{
			yield return null;
		}
		while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
		SelTitleCtrl.<>c__DisplayClass30_1 CS$<>8__locals2 = new SelTitleCtrl.<>c__DisplayClass30_1();
		CS$<>8__locals2.isFinish = false;
		CS$<>8__locals2.isRequest = false;
		PguiOpenWindowCtrl.Callback callback = delegate(int index)
		{
			if (index == 0 || index == PguiOpenWindowCtrl.CLOSE_BUTTON_INDEX)
			{
				CS$<>8__locals2.isFinish = true;
				return true;
			}
			int num = 1;
			if (num != 0)
			{
				CS$<>8__locals2.isRequest = true;
				CS$<>8__locals2.isFinish = true;
			}
			return num != 0;
		};
		this.winData.InputFieldPass.text = "";
		this.winData.InputFieldPass2.text = "";
		this.winData.migration.Setup(null, null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, callback, null, false);
		this.winData.migration.Open();
		while (!CS$<>8__locals2.isFinish)
		{
			yield return null;
		}
		if (!CS$<>8__locals2.isRequest)
		{
			yield break;
		}
		CS$<>8__locals2 = null;
		bool isTransferSuccess = false;
		bool isFinish = false;
		Singleton<LoginManager>.Instance.AccountTransfer(delegate(Command cmd)
		{
			AccountTransferResponse accountTransferResponse = cmd.response as AccountTransferResponse;
			isTransferSuccess = accountTransferResponse.result == 1;
			isFinish = true;
		}, this.winData.InputFieldPass.text, this.winData.InputFieldPass2.text);
		while (!isFinish)
		{
			yield return null;
		}
		SelTitleCtrl.<>c__DisplayClass30_2 CS$<>8__locals3 = new SelTitleCtrl.<>c__DisplayClass30_2();
		CS$<>8__locals3.isFinish = false;
		CanvasManager.HdlOpenWindowBasic.Setup("確認", isTransferSuccess ? "データ連携、引き継ぎに成功しました\nゲームを再起動します" : "データ連携、引き継ぎに失敗しました", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int index)
		{
			CS$<>8__locals3.isFinish = true;
			return true;
		}, null, false);
		CanvasManager.HdlOpenWindowBasic.Open();
		while (!CS$<>8__locals3.isFinish)
		{
			yield return null;
		}
		CS$<>8__locals3 = null;
		if (isTransferSuccess)
		{
			this.setting = SelTitleCtrl.PHASE.NONE;
		}
		yield break;
	}

	private IEnumerator DeleteAccount()
	{
		SelTitleCtrl.<>c__DisplayClass31_0 CS$<>8__locals1 = new SelTitleCtrl.<>c__DisplayClass31_0();
		CS$<>8__locals1.<>4__this = this;
		if (string.IsNullOrEmpty(LoginManager.Account))
		{
			string text = "削除対象のアカウントが存在しません";
			CanvasManager.HdlOpenWindowBasic.Setup("アカウント削除", text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), false, (int index) => true, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			do
			{
				yield return null;
			}
			while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
			yield break;
		}
		CS$<>8__locals1.isFinish = false;
		CS$<>8__locals1.isRequest = false;
		this.winData.deleteAccount.Setup(null, null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, new PguiOpenWindowCtrl.Callback(CS$<>8__locals1.<DeleteAccount>g__OnClickFirstWindowButton|1), null, false);
		this.winData.deleteAccount.Open();
		while (!CS$<>8__locals1.isFinish)
		{
			yield return null;
		}
		if (!CS$<>8__locals1.isRequest)
		{
			yield break;
		}
		SelTitleCtrl.<>c__DisplayClass31_1 CS$<>8__locals2 = new SelTitleCtrl.<>c__DisplayClass31_1();
		CS$<>8__locals2.isFinish = false;
		Singleton<LoginManager>.Instance.AccountDelete(delegate(Command cmd)
		{
			CS$<>8__locals2.isFinish = true;
		});
		while (!CS$<>8__locals2.isFinish)
		{
			yield return null;
		}
		CS$<>8__locals2.isFinish = true;
		CS$<>8__locals2 = null;
		SelTitleCtrl.<>c__DisplayClass31_2 CS$<>8__locals3 = new SelTitleCtrl.<>c__DisplayClass31_2();
		CS$<>8__locals3.isFinish = false;
		CanvasManager.HdlOpenWindowBasic.Setup("確認", "アカウントを削除しました\nゲームを再起動します", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int index)
		{
			CS$<>8__locals3.isFinish = true;
			return true;
		}, null, false);
		CanvasManager.HdlOpenWindowBasic.Open();
		while (!CS$<>8__locals3.isFinish)
		{
			yield return null;
		}
		CS$<>8__locals3 = null;
		this.setting = SelTitleCtrl.PHASE.NONE;
		yield break;
	}

	private IEnumerator GoogleAccountLink()
	{
		GPGLoginResponse response = null;
		bool isAuthFailed = false;
		Singleton<LoginManager>.Instance.LoginGooglePlayGames(delegate(GPGLoginResponse action)
		{
			response = action;
		}, delegate(bool auth)
		{
			isAuthFailed = auth;
		});
		if ((!Social.localUser.authenticated || response.result == 0) | isAuthFailed)
		{
			this.setting = SelTitleCtrl.PHASE.SETTING;
			string text = "Google Play ゲームアカウントにログインされていません";
			CanvasManager.HdlOpenWindowBasic.Setup("データ連携", text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), false, (int index) => true, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			do
			{
				yield return null;
			}
			while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
			yield break;
		}
		if (response.result == 1)
		{
			string text2 = "連携できるデータがありません\nデータ連携を中断します";
			CanvasManager.HdlOpenWindowBasic.Setup("データ連携", text2, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int index) => true, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			do
			{
				yield return null;
			}
			while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
			this.setting = SelTitleCtrl.PHASE.SETTING;
			yield break;
		}
		if (response.result == 3)
		{
			string text3 = string.Format("フレンドID:{0}\nユーザー名:{1}\n{2}\n上記のアカウントを連携しますか？", LoginManager.FriendCode, response.before_user_name, this.GetHyphen(LoginManager.FriendCode.ToString(), response.before_user_name.IsNullOrEmpty() ? "" : response.before_user_name));
			int sel2 = -2;
			CanvasManager.HdlOpenWindowBasic.Setup("データ連携", text3, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, delegate(int index)
			{
				sel2 = index;
				return true;
			}, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			do
			{
				yield return null;
			}
			while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
			if (sel2 != 1)
			{
				this.setting = SelTitleCtrl.PHASE.SETTING;
				yield break;
			}
			bool isFailed2 = false;
			Singleton<DataManager>.Instance.ServerRequest(AccountGPGConnectCmd.Create(), delegate(Command cmd)
			{
				AccountGPGConnectResponse accountGPGConnectResponse = cmd.response as AccountGPGConnectResponse;
				isFailed2 = accountGPGConnectResponse.result == 0;
			});
			while (DataManager.IsServerRequesting())
			{
				yield return null;
			}
			if (isFailed2)
			{
				yield break;
			}
			text3 = string.Format("フレンドID:{0}\nユーザー名:{1}\n{2}\n上記のアカウントをログインした\nGoogle Play ゲームアカウントに連携しました", LoginManager.FriendCode, response.before_user_name, this.GetHyphen(LoginManager.FriendCode.ToString(), response.before_user_name.IsNullOrEmpty() ? "" : response.before_user_name));
			CanvasManager.HdlOpenWindowBasic.Setup("データ連携", text3, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int index) => true, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			do
			{
				yield return null;
			}
			while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
			this.setting = SelTitleCtrl.PHASE.SETTING;
			yield break;
		}
		else
		{
			int transferForking = -1;
			if (response.result == 2 || response.result == 4)
			{
				SelTitleCtrl.<>c__DisplayClass32_2 CS$<>8__locals3 = new SelTitleCtrl.<>c__DisplayClass32_2();
				List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>> list = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>();
				list.Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.NEGATIVE, "いいえ"));
				list.Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, "引き継ぎ"));
				list.Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, "連携上書き"));
				string text4 = string.Format("フレンドID:{0}\nユーザー名:{1}\n{2}\nログインしたGoogle Play ゲームアカウントは\n既に上記のアカウントと連携されています\nアカウントを引き継ぐか、アカウントと連携を\n上書きするか選択してください", response.after_friend_code, response.after_user_name, this.GetHyphen(response.after_friend_code.ToString().IsNullOrEmpty() ? "" : response.after_friend_code.ToString(), response.after_user_name.IsNullOrEmpty() ? "" : response.after_user_name));
				CS$<>8__locals3.sel = -2;
				CanvasManager.HdlOpenWindowBasic.Setup("データ連携", text4, list, true, delegate(int index)
				{
					CS$<>8__locals3.sel = index;
					return true;
				}, null, true);
				CanvasManager.HdlOpenWindowBasic.Open();
				do
				{
					yield return null;
				}
				while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
				if (CS$<>8__locals3.sel == -1 || CS$<>8__locals3.sel == 0)
				{
					this.setting = SelTitleCtrl.PHASE.SETTING;
					yield break;
				}
				if (CS$<>8__locals3.sel == 1)
				{
					transferForking = 1;
				}
				if (CS$<>8__locals3.sel == 2)
				{
					transferForking = 2;
				}
				CS$<>8__locals3 = null;
			}
			if (transferForking == 1)
			{
				SelTitleCtrl.<>c__DisplayClass32_3 CS$<>8__locals4 = new SelTitleCtrl.<>c__DisplayClass32_3();
				if (LoginManager.FriendCode == response.after_friend_code)
				{
					CanvasManager.HdlOpenWindowBasic.Setup("データ連携", "連携されているアカウントと今のアカウントの情報が同じです\n引き継ぎを中断します", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int index) => true, null, false);
					CanvasManager.HdlOpenWindowBasic.Open();
					do
					{
						yield return null;
					}
					while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
					this.setting = SelTitleCtrl.PHASE.SETTING;
					yield break;
				}
				string text5 = string.Format("フレンドID:{0}\nユーザー名:{1}\n{2}\nアカウントを引き継ぐと\n上記アカウントは端末から削除されます\n本当にアカウントを引き継ぎますか？", LoginManager.FriendCode, response.before_user_name, this.GetHyphen(LoginManager.FriendCode.ToString(), response.before_user_name.IsNullOrEmpty() ? "" : response.before_user_name.ToString()));
				CS$<>8__locals4.sel = -2;
				if (response.result == 4)
				{
					CanvasManager.HdlOpenWindowBasic.Setup("データ連携", text5, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, delegate(int index)
					{
						CS$<>8__locals4.sel = index;
						return true;
					}, null, false);
					CanvasManager.HdlOpenWindowBasic.Open();
					do
					{
						yield return null;
					}
					while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
					if (CS$<>8__locals4.sel != 1)
					{
						this.setting = SelTitleCtrl.PHASE.SETTING;
						yield break;
					}
				}
				if (response.result != 2 && response.result != 4)
				{
					text5 = "連携できるデータがありません\nデータ連携を中断します";
					CanvasManager.HdlOpenWindowBasic.Setup("データ連携", text5, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int index)
					{
						CS$<>8__locals4.sel = index;
						return true;
					}, null, false);
					CanvasManager.HdlOpenWindowBasic.Open();
					do
					{
						yield return null;
					}
					while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
					yield break;
				}
				text5 = string.Format("ログインしたGoogle Play ゲームアカウントは\n下記のアカウントと連携されています\n{2}\nフレンドID:{0}\nユーザー名:{1}\n{2}\n上記のアカウントを引き継ぎますか？", response.after_friend_code, response.after_user_name, this.GetHyphen(response.after_friend_code.ToString().IsNullOrEmpty() ? "" : response.after_friend_code.ToString(), response.after_user_name.IsNullOrEmpty() ? "" : response.after_user_name.ToString()));
				CS$<>8__locals4.sel = -2;
				CanvasManager.HdlOpenWindowBasic.Setup("データ連携", text5, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, delegate(int index)
				{
					CS$<>8__locals4.sel = index;
					return true;
				}, null, false);
				CanvasManager.HdlOpenWindowBasic.Open();
				do
				{
					yield return null;
				}
				while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
				if (CS$<>8__locals4.sel != 1)
				{
					this.setting = SelTitleCtrl.PHASE.SETTING;
					yield break;
				}
				CS$<>8__locals4.isFailed = false;
				CS$<>8__locals4.transferResponse = new AccountGPGTransferResponse();
				Singleton<DataManager>.Instance.ServerRequest(AccountGPGTransferCmd.Create(response.after_transfer_id, SystemInfo.deviceModel), delegate(Command cmd)
				{
					CS$<>8__locals4.transferResponse = cmd.response as AccountGPGTransferResponse;
					CS$<>8__locals4.isFailed = CS$<>8__locals4.transferResponse.result != 1;
				});
				while (DataManager.IsServerRequesting())
				{
					yield return null;
				}
				if (CS$<>8__locals4.isFailed)
				{
					yield break;
				}
				Singleton<LoginManager>.Instance.AccountGPGTransfer(CS$<>8__locals4.transferResponse.account_id, CS$<>8__locals4.transferResponse.uuid, response.after_transfer_id);
				text5 = string.Format("フレンドID:{0}\nユーザー名:{1}\n{2}\n上記のアカウントを引き継ぎました\nゲームを再起動します", response.after_friend_code, response.after_user_name, this.GetHyphen(response.after_friend_code.ToString().IsNullOrEmpty() ? "" : response.after_friend_code.ToString(), response.after_user_name.IsNullOrEmpty() ? "" : response.after_user_name.ToString()));
				CanvasManager.HdlOpenWindowBasic.Setup("データ連携", text5, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int index) => true, null, false);
				CanvasManager.HdlOpenWindowBasic.Open();
				do
				{
					yield return null;
				}
				while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
				Singleton<SceneManager>.Instance.SetSceneReboot();
				CS$<>8__locals4 = null;
			}
			if (transferForking != 2)
			{
				yield break;
			}
			if (LoginManager.FriendCode == response.after_friend_code)
			{
				CanvasManager.HdlOpenWindowBasic.Setup("データ連携", "連携されているアカウントと今のアカウントの情報が同じです\n連携を中断します", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int index) => true, null, false);
				CanvasManager.HdlOpenWindowBasic.Open();
				do
				{
					yield return null;
				}
				while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
				this.setting = SelTitleCtrl.PHASE.SETTING;
				yield break;
			}
			if (LoginManager.FriendCode == 0)
			{
				CanvasManager.HdlOpenWindowBasic.Setup("データ連携", "連携できるデータがありません\nデータ連携を中断します", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int index) => true, null, false);
				CanvasManager.HdlOpenWindowBasic.Open();
				do
				{
					yield return null;
				}
				while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
				this.setting = SelTitleCtrl.PHASE.SETTING;
				yield break;
			}
			string text6 = string.Format("フレンドID:{0}\nユーザー名:{1}\n{2}\n上記のアカウントは現在ログインしている\nGoogle Play ゲームアカウントの連携から解除されます\nよろしいですか？", response.after_friend_code, response.after_user_name, this.GetHyphen(response.after_friend_code.ToString().IsNullOrEmpty() ? "" : response.after_friend_code.ToString(), response.after_user_name.IsNullOrEmpty() ? "" : response.after_user_name.ToString()));
			int sel = -2;
			CanvasManager.HdlOpenWindowBasic.Setup("データ連携", text6, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, delegate(int index)
			{
				sel = index;
				return true;
			}, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			do
			{
				yield return null;
			}
			while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
			if (sel != 1)
			{
				this.setting = SelTitleCtrl.PHASE.SETTING;
				yield break;
			}
			bool isFailed = false;
			Singleton<DataManager>.Instance.ServerRequest(AccountGPGConnectCmd.Create(), delegate(Command cmd)
			{
				AccountGPGConnectResponse accountGPGConnectResponse2 = cmd.response as AccountGPGConnectResponse;
				isFailed = accountGPGConnectResponse2.result == 0;
			});
			while (DataManager.IsServerRequesting())
			{
				yield return null;
			}
			if (isFailed)
			{
				yield break;
			}
			text6 = string.Format("フレンドID:{0}\nユーザー名:{1}\n{2}\n上記のアカウントをログインした\nGoogle Play ゲームアカウントに連携しました", LoginManager.FriendCode, response.before_user_name, this.GetHyphen(LoginManager.FriendCode.ToString(), response.before_user_name.IsNullOrEmpty() ? "" : response.before_user_name));
			CanvasManager.HdlOpenWindowBasic.Setup("データ連携", text6, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int index) => true, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			do
			{
				yield return null;
			}
			while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
			this.setting = SelTitleCtrl.PHASE.SETTING;
			yield break;
		}
	}

	private IEnumerator GoogleAccountRelease()
	{
		GPGLoginResponse response = null;
		bool isAuthFailed = false;
		Singleton<LoginManager>.Instance.LoginGooglePlayGames(delegate(GPGLoginResponse action)
		{
			response = action;
		}, delegate(bool auth)
		{
			isAuthFailed = auth;
		});
		if ((!Social.localUser.authenticated || response.result == 0) | isAuthFailed)
		{
			this.setting = SelTitleCtrl.PHASE.SETTING;
			string text = "Google Play ゲームアカウントにログインされていません";
			CanvasManager.HdlOpenWindowBasic.Setup("データ連携", text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), false, (int index) => true, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			do
			{
				yield return null;
			}
			while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
			yield break;
		}
		if (response.result == 1 || response.result == 3)
		{
			string text2 = "ログインしたGoogle Play ゲームアカウントは\nアカウント連携されていません";
			CanvasManager.HdlOpenWindowBasic.Setup("データ連携", text2, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int index) => true, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			do
			{
				yield return null;
			}
			while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
			this.setting = SelTitleCtrl.PHASE.SETTING;
			yield break;
		}
		if (response.result == 2 || response.result == 4)
		{
			SelTitleCtrl.<>c__DisplayClass33_1 CS$<>8__locals2 = new SelTitleCtrl.<>c__DisplayClass33_1();
			string text3 = string.Format("ログインしたGoogle Play ゲームアカウントは\n下記のアカウントと連携されています\n{2}\nフレンドID:{0}\nユーザー名:{1}\n{2}\n上記のアカウントの連携を解除しますか？", response.after_friend_code, response.after_user_name, this.GetHyphen(response.after_friend_code.ToString().IsNullOrEmpty() ? "" : response.after_friend_code.ToString(), response.after_user_name.IsNullOrEmpty() ? "" : response.after_user_name.ToString()));
			CS$<>8__locals2.sel = -2;
			CanvasManager.HdlOpenWindowBasic.Setup("データ連携", text3, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, delegate(int index)
			{
				CS$<>8__locals2.sel = index;
				return true;
			}, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			do
			{
				yield return null;
			}
			while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
			if (CS$<>8__locals2.sel != 1)
			{
				this.setting = SelTitleCtrl.PHASE.SETTING;
				yield break;
			}
			CS$<>8__locals2.isFailed = false;
			Singleton<DataManager>.Instance.ServerRequest(AccountGPGDisconnectCmd.Create(response.after_transfer_id), delegate(Command cmd)
			{
				AccountGPGDisconnectResponse accountGPGDisconnectResponse = cmd.response as AccountGPGDisconnectResponse;
				CS$<>8__locals2.isFailed = accountGPGDisconnectResponse.result != 1;
			});
			while (DataManager.IsServerRequesting())
			{
				yield return null;
			}
			if (CS$<>8__locals2.isFailed)
			{
				yield break;
			}
			text3 = string.Format("フレンドID:{0}\nユーザー名:{1}\n{2}\n上記のアカウントの連携を解除しました", response.after_friend_code, response.after_user_name, this.GetHyphen(response.after_friend_code.ToString().IsNullOrEmpty() ? "" : response.after_friend_code.ToString(), response.after_user_name.IsNullOrEmpty() ? "" : response.after_user_name.ToString()));
			CS$<>8__locals2.sel = -2;
			CanvasManager.HdlOpenWindowBasic.Setup("データ連携", text3, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int index)
			{
				CS$<>8__locals2.sel = index;
				return true;
			}, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			do
			{
				yield return null;
			}
			while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
			CS$<>8__locals2 = null;
		}
		this.setting = SelTitleCtrl.PHASE.SETTING;
		yield break;
	}

	private void OnClickButton(PguiButtonCtrl button)
	{
		if (this.graphicEnumerator != null || this.recoveryEnumerator != null || this.migrationEnumerator != null || this.deleteAccountEnumerator != null)
		{
			return;
		}
		if (!CanvasManager.HdlWebViewWindowCtrl.FinishedClose())
		{
			return;
		}
		if (button == this.winData.Btn_Graphic)
		{
			this.setting = SelTitleCtrl.PHASE.GRAPHIC;
			return;
		}
		if (button == this.winData.Btn_Recovery)
		{
			this.setting = SelTitleCtrl.PHASE.RECOVERY;
			return;
		}
		if (button == this.winData.Btn_Migration || button == this.googleWindowData.Btn_idPassLink)
		{
			if (button == this.winData.Btn_Migration)
			{
				this.setting = SelTitleCtrl.PHASE.MIGRATION;
				return;
			}
			this.setting = SelTitleCtrl.PHASE.MIGRATION;
			return;
		}
		else
		{
			if (button == this.winData.Btn_DeleteAccount)
			{
				this.setting = SelTitleCtrl.PHASE.DELETE;
				return;
			}
			if (button == this.googleWindowData.Btn_setting_close)
			{
				this.setting = SelTitleCtrl.PHASE.SETTING;
				return;
			}
			if (button == this.googleWindowData.Btn_accountLink)
			{
				this.setting = SelTitleCtrl.PHASE.GPG_LINK;
				return;
			}
			if (button == this.googleWindowData.Btn_release)
			{
				this.setting = SelTitleCtrl.PHASE.GPG_RELEASE;
				return;
			}
			if (this.settingEnumerator == null)
			{
				if (button == this.guiData.Btn_Dataid)
				{
					this.settingEnumerator = this.Setting();
					CanvasManager.HdlAdevertiseBannerCtrl.ClaerProgress();
					return;
				}
				if (button == this.guiData.Btn_Termsofuse)
				{
					CanvasManager.HdlWebViewWindowCtrl.Open("kiyaku/index.html");
					CanvasManager.HdlAdevertiseBannerCtrl.ClaerProgress();
				}
			}
			return;
		}
	}

	private bool OnClickToggle(PguiToggleButtonCtrl pbc, int toggleIndex)
	{
		int[] option = SceneManager.GetOption();
		int num = option[1];
		if (pbc == this.winData.Tgl_Light)
		{
			num = 0;
		}
		else if (pbc == this.winData.Tgl_Normal)
		{
			num = 1;
		}
		else if (pbc == this.winData.Tgl_High)
		{
			num = 2;
		}
		else
		{
			num = 1;
		}
		if (option[1] != num)
		{
			option[1] = num;
			SceneManager.SetOption(option);
			UserOptionData.SetDisplayQuality(option[1], new Vector2Int(SceneManager.screenSize.width, SceneManager.screenSize.height));
		}
		this.winData.Tgl_High.SetToggleIndex(0);
		this.winData.Tgl_Normal.SetToggleIndex(0);
		this.winData.Tgl_Light.SetToggleIndex(0);
		return true;
	}

	private string GetHyphen(string str1, string str2)
	{
		string text = string.Empty;
		int num = ((str1.Count<char>() < str2.Count<char>()) ? str2.Count<char>() : str1.Count<char>());
		int num2 = (6 + num) * 2;
		for (int i = 0; i < num2; i++)
		{
			text += "-";
		}
		return text;
	}

	public void Update()
	{
		if (this.openingEnumerator != null && !this.openingEnumerator.MoveNext())
		{
			this.openingEnumerator = null;
		}
		if (this.settingEnumerator != null && !this.settingEnumerator.MoveNext())
		{
			this.settingEnumerator = null;
			if (this.setting == SelTitleCtrl.PHASE.GRAPHIC)
			{
				this.graphicEnumerator = this.Graphic();
			}
			else if (this.setting == SelTitleCtrl.PHASE.RECOVERY)
			{
				this.recoveryEnumerator = this.Recovery();
			}
			else if (this.setting == SelTitleCtrl.PHASE.MIGRATION)
			{
				this.migrationEnumerator = this.Migration();
			}
			else if (this.setting == SelTitleCtrl.PHASE.DELETE)
			{
				this.deleteAccountEnumerator = this.DeleteAccount();
			}
			else if (this.setting == SelTitleCtrl.PHASE.SETTING)
			{
				this.googleSettingEnumerator = this.GoogleSetting();
			}
		}
		if (this.graphicEnumerator != null && !this.graphicEnumerator.MoveNext())
		{
			this.graphicEnumerator = null;
			if (this.setting == SelTitleCtrl.PHASE.GRAPHIC)
			{
				this.settingEnumerator = this.Setting();
			}
		}
		if (this.recoveryEnumerator != null && !this.recoveryEnumerator.MoveNext())
		{
			this.recoveryEnumerator = null;
			if (this.setting == SelTitleCtrl.PHASE.RECOVERY)
			{
				this.settingEnumerator = this.Setting();
			}
			else
			{
				this.gotoNextScene(true);
			}
		}
		if (this.googleSettingEnumerator != null && !this.googleSettingEnumerator.MoveNext())
		{
			this.googleSettingEnumerator = null;
			if (this.setting == SelTitleCtrl.PHASE.SETTING)
			{
				this.settingEnumerator = this.Setting();
			}
			else if (this.setting == SelTitleCtrl.PHASE.MIGRATION)
			{
				this.migrationEnumerator = this.Migration();
				this.setting = SelTitleCtrl.PHASE.SETTING;
			}
			else if (this.setting == SelTitleCtrl.PHASE.GPG_LINK)
			{
				this.googleAccountLinkEnumerator = this.GoogleAccountLink();
			}
			else if (this.setting == SelTitleCtrl.PHASE.GPG_RELEASE)
			{
				this.googleAccountReleaseEnumerator = this.GoogleAccountRelease();
			}
			else
			{
				this.gotoNextScene(true);
			}
		}
		if (this.googleAccountLinkEnumerator != null && !this.googleAccountLinkEnumerator.MoveNext())
		{
			this.googleAccountLinkEnumerator = null;
			if (this.setting == SelTitleCtrl.PHASE.SETTING)
			{
				this.googleSettingEnumerator = this.GoogleSetting();
			}
			else
			{
				this.gotoNextScene(true);
			}
		}
		if (this.googleAccountReleaseEnumerator != null && !this.googleAccountReleaseEnumerator.MoveNext())
		{
			this.googleAccountReleaseEnumerator = null;
			if (this.setting == SelTitleCtrl.PHASE.SETTING)
			{
				this.googleSettingEnumerator = this.GoogleSetting();
			}
			else
			{
				this.gotoNextScene(true);
			}
		}
		if (this.migrationEnumerator != null && !this.migrationEnumerator.MoveNext())
		{
			this.migrationEnumerator = null;
			if (this.setting == SelTitleCtrl.PHASE.MIGRATION)
			{
				this.settingEnumerator = this.Setting();
			}
			else if (this.setting == SelTitleCtrl.PHASE.SETTING)
			{
				this.googleSettingEnumerator = this.GoogleSetting();
			}
			else
			{
				this.gotoNextScene(true);
			}
		}
		if (this.deleteAccountEnumerator != null && !this.deleteAccountEnumerator.MoveNext())
		{
			this.deleteAccountEnumerator = null;
			if (this.setting == SelTitleCtrl.PHASE.DELETE)
			{
				this.settingEnumerator = this.Setting();
				return;
			}
			this.gotoNextScene(true);
		}
	}

	private IEnumerator openingEnumerator;

	private IEnumerator settingEnumerator;

	private IEnumerator graphicEnumerator;

	private IEnumerator recoveryEnumerator;

	private IEnumerator migrationEnumerator;

	private IEnumerator deleteAccountEnumerator;

	private IEnumerator googleSettingEnumerator;

	private IEnumerator googleAccountLinkEnumerator;

	private IEnumerator googleAccountReleaseEnumerator;

	private SelTitleCtrl.GUI guiData;

	private SelTitleCtrl.WIN winData;

	private SelTitleCtrl.GoogleAccountWindow googleWindowData;

	private bool pvp;

	private SelTitleCtrl.PHASE setting;

	private UnityAction<bool> gotoNextScene;

	private bool isSetupBanner;

	public class GUI
	{
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_Termsofuse = baseTr.Find("Auth_Title/Info_All/Btn_Termsofuse").GetComponent<PguiButtonCtrl>();
			this.Btn_Dataid = baseTr.Find("Auth_Title/Info_All/Btn_Dataid").GetComponent<PguiButtonCtrl>();
			this.Num_Txt_Id = baseTr.Find("Auth_Title/Info_All/Num_Txt_Id").GetComponent<PguiTextCtrl>();
			this.Num_Txt_Ver = baseTr.Find("Auth_Title/Info_All/Num_Txt_Ver").GetComponent<PguiTextCtrl>();
			this.AEImage_Title = baseTr.Find("Auth_Title/AEimg_Title_All").GetComponent<PguiAECtrl>();
			this.logo = baseTr.Find("Auth_Title/Texture_Logo").GetComponent<PguiRawImageCtrl>();
		}

		public GameObject baseObj;

		public PguiButtonCtrl Btn_Termsofuse;

		public PguiButtonCtrl Btn_Dataid;

		public PguiTextCtrl Num_Txt_Id;

		public PguiTextCtrl Num_Txt_Ver;

		public PguiAECtrl AEImage_Title;

		public PguiRawImageCtrl logo;
	}

	public class WIN
	{
		public WIN(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.setting = baseTr.Find("Window_SettingMenu").GetComponent<PguiOpenWindowCtrl>();
			this.migration = baseTr.Find("Window_DataMigration/NameWindow").GetComponent<PguiOpenWindowCtrl>();
			this.graphic = baseTr.Find("Window_GraphicSetting").GetComponent<PguiOpenWindowCtrl>();
			this.deleteAccount = baseTr.Find("Window_DeleteAccount/NameWindow").GetComponent<PguiOpenWindowCtrl>();
			this.deleteAccountFinal = baseTr.Find("Window_DeleteAccount/FinalCheckWindow").GetComponent<PguiOpenWindowCtrl>();
			this.Btn_Graphic = this.setting.transform.Find("Base/Window/Btn_Graphic").GetComponent<PguiButtonCtrl>();
			this.Btn_Recovery = this.setting.transform.Find("Base/Window/Btn_Recovery").GetComponent<PguiButtonCtrl>();
			this.Btn_Migration = this.setting.transform.Find("Base/Window/Btn_DataMigration").GetComponent<PguiButtonCtrl>();
			this.Btn_DeleteAccount = this.setting.transform.Find("Base/Window/Btn_DeleteAccount").GetComponent<PguiButtonCtrl>();
			this.Tgl_High = this.graphic.transform.Find("Base/Window/Btn_High").GetComponent<PguiToggleButtonCtrl>();
			this.Tgl_Normal = this.graphic.transform.Find("Base/Window/Btn_Normal").GetComponent<PguiToggleButtonCtrl>();
			this.Tgl_Light = this.graphic.transform.Find("Base/Window/Btn_Light").GetComponent<PguiToggleButtonCtrl>();
			this.InputFieldPass = this.migration.transform.Find("Base/Window/Input_01/InputField").GetComponent<InputField>();
			this.InputFieldPass2 = this.migration.transform.Find("Base/Window/Input_02/InputField").GetComponent<InputField>();
			this.Btn_DeleteAccount.gameObject.SetActive(false);
			this.Tgl_Light.gameObject.SetActive(false);
			(this.Tgl_Normal.transform as RectTransform).localPosition = new Vector3(-120f, -34f, 0f);
			(this.Tgl_High.transform as RectTransform).localPosition = new Vector3(120f, -34f, 0f);
			this.graphic.transform.Find("Base/Window/Txt_ Suishou").gameObject.SetActive(false);
		}

		public GameObject baseObj;

		public PguiOpenWindowCtrl setting;

		public PguiOpenWindowCtrl migration;

		public PguiOpenWindowCtrl graphic;

		public PguiOpenWindowCtrl deleteAccount;

		public PguiOpenWindowCtrl deleteAccountFinal;

		public PguiButtonCtrl Btn_Graphic;

		public PguiButtonCtrl Btn_Recovery;

		public PguiButtonCtrl Btn_Migration;

		public PguiButtonCtrl Btn_DeleteAccount;

		public PguiToggleButtonCtrl Tgl_High;

		public PguiToggleButtonCtrl Tgl_Normal;

		public PguiToggleButtonCtrl Tgl_Light;

		public InputField InputFieldPass;

		public InputField InputFieldPass2;
	}

	public class GoogleAccountWindow
	{
		public GoogleAccountWindow(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.setting = baseTr.Find("Window_SettingMenu").GetComponent<PguiOpenWindowCtrl>();
			this.idPassLink = baseTr.Find("Window_DataMigration/NameWindow").GetComponent<PguiOpenWindowCtrl>();
			this.Btn_idPassLink = this.setting.transform.Find("Base/Window/Layout/Btn_Id").GetComponent<PguiButtonCtrl>();
			this.Btn_accountLink = this.setting.transform.Find("Base/Window/Layout/Btn_Link").GetComponent<PguiButtonCtrl>();
			this.Btn_release = this.setting.transform.Find("Base/Window/Layout/Btn_Release").GetComponent<PguiButtonCtrl>();
			this.Btn_setting_close = this.setting.transform.Find("Base/Window/BtnClose").GetComponent<PguiButtonCtrl>();
			this.linkedText = this.setting.transform.Find("Base/Window/Layout/Btn_Link/Linked").GetComponent<PguiTextCtrl>();
		}

		public GameObject baseObj;

		public PguiOpenWindowCtrl setting;

		public PguiOpenWindowCtrl idPassLink;

		public PguiButtonCtrl Btn_idPassLink;

		public PguiButtonCtrl Btn_accountLink;

		public PguiButtonCtrl Btn_release;

		public PguiButtonCtrl Btn_setting_close;

		public PguiTextCtrl linkedText;
	}

	private enum PHASE
	{
		NONE,
		GRAPHIC,
		RECOVERY,
		MIGRATION,
		DELETE,
		SETTING,
		GPG_LINK,
		GPG_RELEASE
	}
}
