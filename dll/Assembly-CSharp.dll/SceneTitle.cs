using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SGNFW.Common;
using SGNFW.Http;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Login;
using SGNFW.Mst;
using UnityEngine;
using UnityEngine.Events;

public class SceneTitle : BaseScene
{
	public override void OnCreateScene()
	{
		this.basePanel = new GameObject();
		this.basePanel.name = "SceneTitle";
		this.basePanel.AddComponent<RectTransform>();
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.FRONT, this.basePanel.transform, true);
		this.appEndObj = new GameObject("AppEnd", new Type[] { typeof(RectTransform) });
		RectTransform component = this.appEndObj.GetComponent<RectTransform>();
		component.SetParent(this.basePanel.transform, false);
		component.sizeDelta = new Vector2(1280f, 720f);
		this.appEndObj.AddComponent<PguiCollider>().raycastTarget = true;
		this.selTitleCtrl = this.basePanel.AddComponent<SelTitleCtrl>();
		this.selTitleCtrl.Init();
	}

	public override void OnEnableScene(object args)
	{
		Singleton<AssetManager>.Instance.SetAbChkType(AssetManager.abCheckType.LOW);
		Singleton<DataManager>.Instance.DisableServerRequestByDebug = false;
		this.basePanel.gameObject.SetActive(true);
		CanvasManager.HdlCmnMenu.SetupMenu(false, "タイトル", null, "", null, null);
		SoundManager.PlayBGM(SceneTitle.bgm);
		CanvasManager.SetBgTexture(null);
		this.selTitleCtrl.Setup(new UnityAction<bool>(this.GotoNextScene));
		this.appEndObj.SetActive(false);
		this.appEnd = 0;
		this.gotoNextSceneInternal = null;
		PguiRawImageCtrl.ClearBanner();
		CanvasManager.HdlAdevertiseBannerCtrl.Init();
		CanvasManager.HdlAdevertiseBannerCtrl.StartProgress();
		this.isServiceCloseByMst = false;
		List<MstPlatformStatusData> mst = Singleton<MstManager>.Instance.GetMst<List<MstPlatformStatusData>>(MstType.PLATFORM_STATUS_DATA);
		MstPlatformStatusData mstPlatformStatusData;
		if (mst == null)
		{
			mstPlatformStatusData = null;
		}
		else
		{
			mstPlatformStatusData = mst.Find((MstPlatformStatusData item) => item.platform == LoginManager.Platform);
		}
		MstPlatformStatusData mstPlatformStatusData2 = mstPlatformStatusData;
		if (mstPlatformStatusData2 != null)
		{
			this.isServiceCloseByMst = TimeManager.Now.Ticks > PrjUtil.ConvertTimeToTicks(mstPlatformStatusData2.phase2StartDatetime);
			this.selTitleCtrl.SetEnableTransferButton(TimeManager.Now.Ticks <= PrjUtil.ConvertTimeToTicks(mstPlatformStatusData2.phase1StartDatetime));
		}
	}

	public override void OnStartControl()
	{
	}

	private bool AppEndWindow(int index)
	{
		if (this.appEnd == 1)
		{
			this.appEnd = ((index == 1) ? 2 : 3);
		}
		return true;
	}

	public override void Update()
	{
		if (this.gotoNextSceneInternal != null && !this.gotoNextSceneInternal.MoveNext())
		{
			this.gotoNextSceneInternal = null;
		}
	}

	private bool TermsWindow(int index)
	{
		if (!CanvasManager.HdlWebViewWindowCtrl.FinishedClose())
		{
			return false;
		}
		if (index < 2)
		{
			if (this.terms == 0)
			{
				this.terms = ((index == 1) ? 1 : (-1));
			}
			return true;
		}
		CanvasManager.HdlWebViewWindowCtrl.Open("kiyaku/index.html");
		return false;
	}

	private IEnumerator GotoNextSceneInternal()
	{
		SoundManager.Play("prd_se_title_tap", false, false);
		CanvasManager.HdlAdevertiseBannerCtrl.ClaerProgress();
		if (LoginManager.FriendCode == 0 || !LoginManager.IsCheckedTerms)
		{
			this.terms = 0;
			CanvasManager.HdlOpenWindowBasic.SetupTerms(new PguiOpenWindowCtrl.Callback(this.TermsWindow));
			CanvasManager.HdlOpenWindowBasic.Open();
			do
			{
				yield return null;
			}
			while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
			CanvasManager.HdlOpenWindowBasic.CloseTerms();
			if (this.terms <= 0)
			{
				yield break;
			}
			if (LoginManager.FriendCode == 0)
			{
				this.selTitleCtrl.SetupGraphic();
				while (!this.selTitleCtrl.CheckGraphic())
				{
					yield return null;
				}
			}
		}
		bool isGPGConnect = false;
		CanvasManager.RequestFade(CanvasManager.FadeType.TIPS);
		do
		{
			yield return null;
		}
		while (!CanvasManager.IsFinishFadeAction);
		SceneTitle.<>c__DisplayClass15_0 CS$<>8__locals1 = new SceneTitle.<>c__DisplayClass15_0();
		CS$<>8__locals1.dispTips = false;
		CanvasManager.SwitchLayerHdlLoadAndTipsCtrl(SceneManager.CanvasType.OVERLAY);
		CanvasManager.HdlLoadAndTipsCtrl.Setup(new LoadAndTipsCtrl.SetupParam
		{
			dispTipsId = 0,
			isDispTips = true,
			cbTipsDispFinish = delegate
			{
				CS$<>8__locals1.dispTips = true;
			}
		});
		while (!CS$<>8__locals1.dispTips)
		{
			yield return null;
		}
		CS$<>8__locals1 = null;
		IEnumerator ienum = DataInitializeResolver.InitializeActionAfterTitle(isGPGConnect);
		while (ienum.MoveNext())
		{
			yield return null;
		}
		ienum = null;
		RestartResolver rr = new RestartResolver();
		while (!rr.ResolveAction())
		{
			yield return null;
		}
		if (rr.result.nextScene != SceneManager.SceneName.None)
		{
			Singleton<SceneManager>.Instance.SetNextScene(rr.result.nextScene, rr.result.nextSceneArgs);
			yield break;
		}
		rr = null;
		if (DataManager.DmUserInfo.tutorialSequence == TutorialUtil.Sequence.INVALID || DataManager.DmUserInfo.tutorialSequence == TutorialUtil.Sequence.END)
		{
			Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneHome, null);
		}
		else if (DataManager.DmUserInfo.tutorialSequence == TutorialUtil.Sequence.FIRST || DataManager.DmUserInfo.tutorialSequence == TutorialUtil.Sequence.DATA_RESET)
		{
			TutorialUtil.RequestNextSequence(DataManager.DmUserInfo.tutorialSequence);
		}
		else
		{
			TutorialUtil.SetHelper(-1);
			TutorialUtil.RequestNextSequence(DataManager.DmUserInfo.tutorialSequence - 1);
		}
		yield break;
	}

	private void GotoNextScene(bool isReboot)
	{
		if (this.appEnd != 0 || this.appEndObj.activeSelf)
		{
			return;
		}
		Singleton<AssetManager>.Instance.SetAbChkType(AssetManager.abCheckType.LOW);
		if (isReboot)
		{
			Singleton<SceneManager>.Instance.SetSceneReboot();
			return;
		}
		if (this.gotoNextSceneInternal == null)
		{
			this.gotoNextSceneInternal = this.GotoNextSceneInternal();
		}
	}

	public override void OnDisableScene()
	{
		this.basePanel.gameObject.SetActive(false);
		AssetManager.UnloadAssetData(DataInitializeResolver.titleLogo, AssetManager.OWNER.DataInitialize);
	}

	public override void OnDestroyScene()
	{
		Object.Destroy(this.basePanel);
		this.basePanel = null;
	}

	private IEnumerator CheckCooperation(Action<bool> action, GPGLoginResponse response)
	{
		int sel = 0;
		bool isActiveCheck = false;
		string text = "Google Play ゲームサービスと\nゲームデータを連携しますか？\nGoogle Play ゲームサービスとの連携設定を行うことで\n様々なデバイスでのゲームデータの引き継ぎが可能です。\n<size=25>※iOS版、DMM GAMES版への引き継ぎには利用できません。</size>";
		CanvasManager.HdlOpenWindowCheckBox.SetupCheckBox("データ連携", text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, delegate(int index)
		{
			sel = index;
			return true;
		}, delegate(bool isActive)
		{
			isActiveCheck = isActive;
		}, "ウィンドウを今後表示しない");
		CanvasManager.HdlOpenWindowCheckBox.Open();
		do
		{
			yield return null;
		}
		while (!CanvasManager.HdlOpenWindowCheckBox.FinishedClose());
		if (response.result == 2 || response.result == 4)
		{
			List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>> list = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>();
			list.Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.NEGATIVE, "いいえ"));
			list.Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, "連携上書き"));
			text = string.Format("フレンドID:{0}\nユーザー名:{1}\n{2}\nログインしたGoogle Play ゲームアカウントは\n既に上記のアカウントと連携されています\n連携を上書きしますか？", response.after_friend_code, response.after_user_name, this.GetHyphen(response.after_friend_code.ToString().IsNullOrEmpty() ? "" : response.after_friend_code.ToString(), response.after_user_name.IsNullOrEmpty() ? "" : response.after_user_name));
			CanvasManager.HdlOpenWindowBasic.Setup("データ連携", text, list, true, delegate(int index)
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
		}
		action(sel == 1);
		if (isActiveCheck)
		{
			PlayerPrefs.SetInt("sgnfw_login_cooperation_checked", 1);
		}
		yield break;
	}

	private IEnumerator CheckTransfer(GPGLoginResponse response)
	{
		string text = "Google Play ゲームサービスと\n連携済みのゲームデータがあります\nゲームデータ連携を行いますか？";
		int sel = 0;
		CanvasManager.HdlOpenWindowBasic.Setup("データ連携", text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, delegate(int index)
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
			yield break;
		}
		bool isFailed = false;
		AccountGPGTransferResponse transferResponse = new AccountGPGTransferResponse();
		Singleton<DataManager>.Instance.ServerRequest(AccountGPGTransferCmd.Create(response.after_transfer_id, SystemInfo.deviceModel), delegate(Command cmd)
		{
			transferResponse = cmd.response as AccountGPGTransferResponse;
			isFailed = transferResponse.result != 1;
		});
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		if (isFailed)
		{
			yield break;
		}
		Singleton<LoginManager>.Instance.AccountGPGTransfer(transferResponse.account_id, transferResponse.uuid, response.after_transfer_id);
		text = string.Format("フレンドID:{0}\nユーザー名:{1}\n{2}\n上記のアカウントを引き継ぎました\nゲームを再起動します", response.after_friend_code, response.after_user_name, this.GetHyphen(response.after_friend_code.ToString().IsNullOrEmpty() ? "" : response.after_friend_code.ToString(), response.after_user_name.IsNullOrEmpty() ? "" : response.after_user_name.ToString()));
		CanvasManager.HdlOpenWindowBasic.Setup("データ連携", text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int index) => true, null, false);
		CanvasManager.HdlOpenWindowBasic.Open();
		do
		{
			yield return null;
		}
		while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
		Singleton<SceneManager>.Instance.SetSceneReboot();
		yield break;
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

	private GameObject basePanel;

	private SelTitleCtrl selTitleCtrl;

	private GameObject appEndObj;

	private int appEnd;

	private bool pvp;

	public static readonly string bgm = "prd_bgm0022";

	private IEnumerator gotoNextSceneInternal;

	private bool isServiceCloseByMst;

	private int terms;
}
