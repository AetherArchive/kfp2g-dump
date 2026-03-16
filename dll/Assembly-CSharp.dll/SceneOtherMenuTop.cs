using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SGNFW.Common;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Login;
using UnityEngine;

public class SceneOtherMenuTop : BaseScene
{
	public override void OnCreateScene()
	{
		this.basePanel = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneMenu/GUI/Prefab/GUI_Menu"));
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.FRONT, this.basePanel.transform, true);
		this.guiData = new SceneOtherMenuTop.GUI(this.basePanel.transform);
		this.guiGoogleAccountWindow = new SceneOtherMenuTop.GuiGoogleAccountWindow(AssetManager.InstantiateAssetData("SceneTitle/GUI/Prefab/GUI_Title_Google_Account_Window", Singleton<CanvasManager>.Instance.SystemPanel.transform).transform);
		this.guiData.Profile_Btn.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Item_Btn.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Story_Btn.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Option_Btn.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Help_Btn.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Download_Btn.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Dataid_Btn.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Birthday_Btn.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Mail_Btn.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Termsofuse_Btn.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Ranking_Btn.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickRankingButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Achievement_Btn.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiGoogleAccountWindow.Btn_accountLink.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiGoogleAccountWindow.Btn_idPassLink.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiGoogleAccountWindow.Btn_release.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiSubData = new SceneOtherMenuTop.GUISub(Object.Instantiate<GameObject>(Resources.Load("SceneMenu/GUI/Prefab/GUI_Menu_Terms_Window") as GameObject, Singleton<CanvasManager>.Instance.SystemMiddleArea).transform);
		this.guiSubData.Btn_Credit.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonSub), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiSubData.Btn_Terms.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonSub), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiSubData.Btn_Law01.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonSub), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiSubData.Btn_Law02.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonSub), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiSubData.Btn_CopyLight.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonSub), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiSubData.Btn_Privacy.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonSub), PguiButtonCtrl.SoundType.DEFAULT);
	}

	public override void OnEnableScene(object args)
	{
		this.basePanel.gameObject.SetActive(true);
		this.setting = SceneOtherMenuTop.PHASE.NONE;
		this.enumerator = null;
		this.isEnableMenu = true;
		CanvasManager.HdlCmnMenu.SetupMenu(true, "その他", null, "", null, null);
		CanvasManager.SetBgTexture("selbg_home_in");
		SoundManager.PlayBGM("prd_bgm0013");
		this.guiData.Download_Btn.SetActEnable(true, false, false);
		this.guiData.baseAnim.ExPauseAnimation(SimpleAnimation.ExPguiStatus.START, null);
		this.requestNextScene = SceneManager.SceneName.None;
	}

	public override void OnStartControl()
	{
		this.guiData.baseAnim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
	}

	private void OnClickButton(PguiButtonCtrl button)
	{
		if (this.requestNextScene != SceneManager.SceneName.None)
		{
			return;
		}
		if (button == this.guiData.Profile_Btn)
		{
			this.requestNextScene = SceneManager.SceneName.SceneProfile;
			return;
		}
		if (button == this.guiData.Item_Btn)
		{
			this.requestNextScene = SceneManager.SceneName.SceneItemView;
			return;
		}
		if (button == this.guiData.Story_Btn)
		{
			this.requestNextScene = SceneManager.SceneName.SceneStoryView;
			return;
		}
		if (button == this.guiData.Option_Btn)
		{
			this.requestNextScene = SceneManager.SceneName.SceneOption;
			return;
		}
		if (button == this.guiData.Help_Btn)
		{
			CanvasManager.HdlHelpWindowCtrl.Open(false);
			return;
		}
		if (button == this.guiData.Download_Btn)
		{
			this.enumerator = AssetDownloadResolver.ResolveActionFull(true);
			return;
		}
		if (button == this.guiData.Dataid_Btn)
		{
			this.enumerator = this.AccountTransferMove();
			return;
		}
		if (button == this.guiGoogleAccountWindow.Btn_idPassLink)
		{
			this.setting = SceneOtherMenuTop.PHASE.IDPASS;
			return;
		}
		if (button == this.guiGoogleAccountWindow.Btn_accountLink)
		{
			this.setting = SceneOtherMenuTop.PHASE.GPCONNECT;
			return;
		}
		if (button == this.guiGoogleAccountWindow.Btn_release)
		{
			this.setting = SceneOtherMenuTop.PHASE.GPDISCONNECT;
			return;
		}
		if (button == this.guiData.Birthday_Btn)
		{
			Singleton<SceneManager>.Instance.StartCoroutine(DataManager.DmPurchase.RequestSolutionAgeAuthentic());
			return;
		}
		if (button == this.guiData.Mail_Btn)
		{
			CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("お問い合わせ"), PrjUtil.MakeMessage("お客様の疑問や問題が解決する場合がございますので、\nお問い合わせをいただく前に\nよくある質問をご利用ください。\n\nよくある質問・サポート窓口へ移動します。\n(ブラウザを起動します)"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_MOVE), true, delegate(int index)
			{
				if (index == 1)
				{
					this.requestOpenUrl = "https://support.kemono-friends-3.jp/hc/ja";
				}
				return true;
			}, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			return;
		}
		if (button == this.guiData.Termsofuse_Btn)
		{
			this.guiSubData.Window.Setup(null, null, null, true, null, null, false);
			this.guiSubData.Window.Open();
			return;
		}
		if (button == this.guiData.Achievement_Btn)
		{
			this.requestNextScene = SceneManager.SceneName.SceneAchievement;
			return;
		}
		CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage(""), PrjUtil.MakeMessage("準備中"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, null, null, false);
		CanvasManager.HdlOpenWindowBasic.Open();
	}

	private void OnClickRankingButton(PguiButtonCtrl buttuon)
	{
		if (this.requestNextScene != SceneManager.SceneName.None)
		{
			return;
		}
		this.requestNextScene = SceneManager.SceneName.SceneRanking;
		this.requestNextSceneArgs = new SceneRanking.OpenParam
		{
			resultNextSceneName = SceneManager.SceneName.SceneOtherMenuTop
		};
	}

	private IEnumerator AccountTransferMove()
	{
		if (!LoginManager.IsDmmLink)
		{
			string text = "DMMアカウント同士での\n引き継ぎを行うことはできません。\n\nまた、１つのDMMアカウントは\n１度のみデータ連携することができます。";
			CanvasManager.HdlOpenWindowBasic.Setup("データ連携", text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), false, (int index) => true, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			do
			{
				yield return null;
			}
			while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
		}
		yield return null;
		this.setting = SceneOtherMenuTop.PHASE.TEMP;
		this.requestNextScene = SceneManager.SceneName.SceneAccountTransfer;
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
		this.guiGoogleAccountWindow.linkedText.text = (flag ? "連携済み" : "未連携");
		this.guiGoogleAccountWindow.setting.Setup(null, null, null, true, null, null, false);
		this.guiGoogleAccountWindow.setting.Open();
		this.setting = SceneOtherMenuTop.PHASE.NONE;
		yield return null;
		while (this.setting < SceneOtherMenuTop.PHASE.SETTING && !this.guiGoogleAccountWindow.setting.FinishedClose())
		{
			yield return null;
		}
		this.guiGoogleAccountWindow.setting.ForceClose();
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
			this.setting = SceneOtherMenuTop.PHASE.SETTING;
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
		if (response.result != 3)
		{
			int transferForking = -1;
			if (response.result == 2 || response.result == 4)
			{
				SceneOtherMenuTop.<>c__DisplayClass21_2 CS$<>8__locals2 = new SceneOtherMenuTop.<>c__DisplayClass21_2();
				List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>> list = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>();
				list.Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.NEGATIVE, "いいえ"));
				list.Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, "連携上書き"));
				string text2 = string.Format("フレンドID:{0}\nユーザー名:{1}\n{2}\nログインしたGoogle Play ゲームアカウントは\n既に上記のアカウントと連携されています\n連携を上書きしますか？\n\n<color=#ff0000>データ引き継ぎはタイトル画面で行うことができます</color>", response.after_friend_code, response.after_user_name, this.GetHyphen(response.after_friend_code.ToString().IsNullOrEmpty() ? "" : response.after_friend_code.ToString(), response.after_user_name.IsNullOrEmpty() ? "" : response.after_user_name.ToString()));
				CS$<>8__locals2.sel = -2;
				CanvasManager.HdlOpenWindowBasic.Setup("データ連携", text2, list, true, delegate(int index)
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
				if (CS$<>8__locals2.sel == -1 || CS$<>8__locals2.sel == 0)
				{
					this.setting = SceneOtherMenuTop.PHASE.SETTING;
					yield break;
				}
				if (CS$<>8__locals2.sel == 1)
				{
					transferForking = 1;
				}
				CS$<>8__locals2 = null;
			}
			if (transferForking == 1)
			{
				SceneOtherMenuTop.<>c__DisplayClass21_3 CS$<>8__locals3 = new SceneOtherMenuTop.<>c__DisplayClass21_3();
				if (LoginManager.FriendCode == response.after_friend_code)
				{
					CanvasManager.HdlOpenWindowBasic.Setup("データ連携", "連携されているアカウントと今のアカウントの情報が同じです\n連携を中断します", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int index) => true, null, false);
					CanvasManager.HdlOpenWindowBasic.Open();
					do
					{
						yield return null;
					}
					while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
					this.setting = SceneOtherMenuTop.PHASE.SETTING;
					yield break;
				}
				string text3 = string.Format("フレンドID:{0}\nユーザー名:{1}\n{2}\n上記のアカウントは現在ログインしている\nGoogle Play ゲームアカウントの連携から解除されます\nよろしいですか？", response.after_friend_code, response.after_user_name, this.GetHyphen(response.after_friend_code.ToString().IsNullOrEmpty() ? "" : response.after_friend_code.ToString(), response.after_user_name.IsNullOrEmpty() ? "" : response.after_user_name.ToString()));
				CS$<>8__locals3.sel = -2;
				CanvasManager.HdlOpenWindowBasic.Setup("データ連携", text3, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, delegate(int index)
				{
					CS$<>8__locals3.sel = index;
					return true;
				}, null, false);
				CanvasManager.HdlOpenWindowBasic.Open();
				do
				{
					yield return null;
				}
				while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
				if (CS$<>8__locals3.sel != 1)
				{
					this.setting = SceneOtherMenuTop.PHASE.SETTING;
					yield break;
				}
				CS$<>8__locals3.isFailed = false;
				Singleton<DataManager>.Instance.ServerRequest(AccountGPGConnectCmd.Create(), delegate(Command cmd)
				{
					AccountGPGConnectResponse accountGPGConnectResponse = cmd.response as AccountGPGConnectResponse;
					CS$<>8__locals3.isFailed = accountGPGConnectResponse.result == 0;
				});
				while (DataManager.IsServerRequesting())
				{
					yield return null;
				}
				if (CS$<>8__locals3.isFailed)
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
				CS$<>8__locals3 = null;
			}
			this.setting = SceneOtherMenuTop.PHASE.SETTING;
			yield break;
		}
		string text4 = string.Format("フレンドID:{0}\nユーザー名:{1}\n{2}\n上記のアカウントを連携しますか？", LoginManager.FriendCode, response.before_user_name, this.GetHyphen(LoginManager.FriendCode.ToString(), response.before_user_name.IsNullOrEmpty() ? "" : response.before_user_name.ToString()));
		int sel = -2;
		CanvasManager.HdlOpenWindowBasic.Setup("データ連携", text4, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, delegate(int index)
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
			this.setting = SceneOtherMenuTop.PHASE.SETTING;
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
		text4 = string.Format("フレンドID:{0}\nユーザー名:{1}\n{2}\n上記のアカウントをログインした\nGoogle Play ゲームアカウントに連携しました", LoginManager.FriendCode, response.before_user_name, this.GetHyphen(LoginManager.FriendCode.ToString(), response.before_user_name.IsNullOrEmpty() ? "" : response.before_user_name.ToString()));
		CanvasManager.HdlOpenWindowBasic.Setup("データ連携", text4, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int index) => true, null, false);
		CanvasManager.HdlOpenWindowBasic.Open();
		do
		{
			yield return null;
		}
		while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
		this.setting = SceneOtherMenuTop.PHASE.SETTING;
		yield break;
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
			this.setting = SceneOtherMenuTop.PHASE.SETTING;
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
			this.setting = SceneOtherMenuTop.PHASE.SETTING;
			yield break;
		}
		if (response.result == 2 || response.result == 4)
		{
			SceneOtherMenuTop.<>c__DisplayClass22_1 CS$<>8__locals2 = new SceneOtherMenuTop.<>c__DisplayClass22_1();
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
				this.setting = SceneOtherMenuTop.PHASE.SETTING;
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
		this.setting = SceneOtherMenuTop.PHASE.SETTING;
		yield break;
	}

	private void OnClickButtonSub(PguiButtonCtrl buttuon)
	{
		if (buttuon == this.guiSubData.Btn_Credit)
		{
			CanvasManager.HdlWebViewWindowCtrl.Open("credit/index.html");
			return;
		}
		if (buttuon == this.guiSubData.Btn_Terms)
		{
			CanvasManager.HdlWebViewWindowCtrl.Open("kiyaku/index.html");
			return;
		}
		if (buttuon == this.guiSubData.Btn_Law01)
		{
			CanvasManager.HdlWebViewWindowCtrl.Open("torihiki/index.html");
			return;
		}
		if (buttuon == this.guiSubData.Btn_Law02)
		{
			CanvasManager.HdlWebViewWindowCtrl.Open("kessai/index.html");
			return;
		}
		if (buttuon == this.guiSubData.Btn_CopyLight)
		{
			CanvasManager.HdlWebViewWindowCtrl.Open("kenri/index.html");
			return;
		}
		if (buttuon == this.guiSubData.Btn_Privacy)
		{
			CanvasManager.HdlWebViewWindowCtrl.Open("privacy/index.html");
		}
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

	public override void Update()
	{
		if (this.enumerator != null && !this.enumerator.MoveNext())
		{
			this.enumerator = null;
			switch (this.setting)
			{
			case SceneOtherMenuTop.PHASE.SETTING:
				this.enumerator = this.GoogleSetting();
				break;
			case SceneOtherMenuTop.PHASE.IDPASS:
				this.enumerator = this.AccountTransferMove();
				break;
			case SceneOtherMenuTop.PHASE.GPCONNECT:
				this.enumerator = this.GoogleAccountLink();
				break;
			case SceneOtherMenuTop.PHASE.GPDISCONNECT:
				this.enumerator = this.GoogleAccountRelease();
				break;
			}
		}
		if (this.isEnableMenu && this.requestNextScene != SceneManager.SceneName.None)
		{
			this.isEnableMenu = false;
			this.guiData.baseAnim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
			{
				Singleton<SceneManager>.Instance.SetNextScene(this.requestNextScene, this.requestNextSceneArgs);
				if (this.setting == SceneOtherMenuTop.PHASE.TEMP)
				{
					this.enumerator = null;
					this.enumerator = this.GoogleSetting();
					this.setting = SceneOtherMenuTop.PHASE.SETTING;
				}
			});
		}
		CanvasManager.HdlCmnMenu.UpdateMenu(this.isEnableMenu, true);
		if (!string.IsNullOrEmpty(this.requestOpenUrl))
		{
			Application.OpenURL(this.requestOpenUrl);
			this.requestOpenUrl = "";
		}
	}

	public override void OnDisableScene()
	{
		this.basePanel.gameObject.SetActive(false);
	}

	public override void OnDestroyScene()
	{
		Object.Destroy(this.basePanel);
		Object.Destroy(this.guiSubData.baseObj);
		this.basePanel = null;
		this.guiSubData = null;
	}

	private IEnumerator enumerator;

	private GameObject basePanel;

	private SceneOtherMenuTop.GUI guiData;

	private SceneOtherMenuTop.GUISub guiSubData;

	private SceneOtherMenuTop.GuiGoogleAccountWindow guiGoogleAccountWindow;

	private SceneManager.SceneName requestNextScene;

	private object requestNextSceneArgs;

	private bool isEnableMenu = true;

	private SceneOtherMenuTop.PHASE setting;

	private string requestOpenUrl = "";

	public class GUI
	{
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.baseAnim = baseTr.GetComponent<SimpleAnimation>();
			this.Profile_Btn = baseTr.Find("All/Profile_Btn").GetComponent<PguiButtonCtrl>();
			this.Item_Btn = baseTr.Find("All/Item_Btn").GetComponent<PguiButtonCtrl>();
			this.Story_Btn = baseTr.Find("All/Story_Btn").GetComponent<PguiButtonCtrl>();
			this.Option_Btn = baseTr.Find("All/Option_Btn").GetComponent<PguiButtonCtrl>();
			this.Help_Btn = baseTr.Find("All/Help_Btn").GetComponent<PguiButtonCtrl>();
			this.Download_Btn = baseTr.Find("All/Download_Btn").GetComponent<PguiButtonCtrl>();
			this.Dataid_Btn = baseTr.Find("All/Dataid_Btn").GetComponent<PguiButtonCtrl>();
			this.Birthday_Btn = baseTr.Find("All/Birthday_Btn").GetComponent<PguiButtonCtrl>();
			this.Mail_Btn = baseTr.Find("All/Mail_Btn").GetComponent<PguiButtonCtrl>();
			this.Termsofuse_Btn = baseTr.Find("All/Termsofuse_Btn").GetComponent<PguiButtonCtrl>();
			this.Ranking_Btn = baseTr.Find("All/Ranking_Btn").GetComponent<PguiButtonCtrl>();
			this.Achievement_Btn = baseTr.Find("All/Achievement_Btn").GetComponent<PguiButtonCtrl>();
		}

		public GameObject baseObj;

		public SimpleAnimation baseAnim;

		public PguiButtonCtrl Profile_Btn;

		public PguiButtonCtrl Item_Btn;

		public PguiButtonCtrl Story_Btn;

		public PguiButtonCtrl Option_Btn;

		public PguiButtonCtrl Help_Btn;

		public PguiButtonCtrl Download_Btn;

		public PguiButtonCtrl Dataid_Btn;

		public PguiButtonCtrl Birthday_Btn;

		public PguiButtonCtrl Mail_Btn;

		public PguiButtonCtrl Termsofuse_Btn;

		public PguiButtonCtrl Ranking_Btn;

		public PguiButtonCtrl Achievement_Btn;
	}

	public class GUISub
	{
		public GUISub(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_Credit = baseTr.Find("Window/Base/Window/Btn_Credit").GetComponent<PguiButtonCtrl>();
			this.Btn_Terms = baseTr.Find("Window/Base/Window/Btn_Terms").GetComponent<PguiButtonCtrl>();
			this.Btn_Law01 = baseTr.Find("Window/Base/Window/Btn_Law01").GetComponent<PguiButtonCtrl>();
			this.Btn_Law02 = baseTr.Find("Window/Base/Window/Btn_Law02").GetComponent<PguiButtonCtrl>();
			this.Btn_CopyLight = baseTr.Find("Window/Base/Window/Btn_CopyLight").GetComponent<PguiButtonCtrl>();
			this.Btn_Privacy = baseTr.Find("Window/Base/Window/Btn_Privacy").GetComponent<PguiButtonCtrl>();
			this.Window = baseTr.Find("Window").GetComponent<PguiOpenWindowCtrl>();
		}

		public GameObject baseObj;

		public PguiButtonCtrl Btn_Credit;

		public PguiButtonCtrl Btn_Terms;

		public PguiButtonCtrl Btn_Law01;

		public PguiButtonCtrl Btn_Law02;

		public PguiButtonCtrl Btn_CopyLight;

		public PguiButtonCtrl Btn_Privacy;

		public PguiOpenWindowCtrl Window;
	}

	public class GuiGoogleAccountWindow
	{
		public GuiGoogleAccountWindow(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.setting = baseTr.Find("Window_SettingMenu").GetComponent<PguiOpenWindowCtrl>();
			this.idPassLink = baseTr.Find("Window_DataMigration/NameWindow").GetComponent<PguiOpenWindowCtrl>();
			this.Btn_idPassLink = this.setting.transform.Find("Base/Window/Layout/Btn_Id").GetComponent<PguiButtonCtrl>();
			this.Btn_accountLink = this.setting.transform.Find("Base/Window/Layout/Btn_Link").GetComponent<PguiButtonCtrl>();
			this.Btn_release = this.setting.transform.Find("Base/Window/Layout/Btn_Release").GetComponent<PguiButtonCtrl>();
			this.linkedText = this.setting.transform.Find("Base/Window/Layout/Btn_Link/Linked").GetComponent<PguiTextCtrl>();
		}

		public GameObject baseObj;

		public PguiOpenWindowCtrl setting;

		public PguiOpenWindowCtrl idPassLink;

		public PguiButtonCtrl Btn_idPassLink;

		public PguiButtonCtrl Btn_accountLink;

		public PguiButtonCtrl Btn_release;

		public PguiTextCtrl linkedText;
	}

	private enum PHASE
	{
		NONE,
		SETTING,
		IDPASS,
		GPCONNECT,
		GPDISCONNECT,
		TEMP
	}
}
