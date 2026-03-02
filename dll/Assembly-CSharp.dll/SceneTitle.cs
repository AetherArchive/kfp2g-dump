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

// Token: 0x0200017F RID: 383
public class SceneTitle : BaseScene
{
	// Token: 0x060018D1 RID: 6353 RVA: 0x00131484 File Offset: 0x0012F684
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

	// Token: 0x060018D2 RID: 6354 RVA: 0x0013154C File Offset: 0x0012F74C
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

	// Token: 0x060018D3 RID: 6355 RVA: 0x0013167C File Offset: 0x0012F87C
	public override void OnStartControl()
	{
	}

	// Token: 0x060018D4 RID: 6356 RVA: 0x0013167E File Offset: 0x0012F87E
	private bool AppEndWindow(int index)
	{
		if (this.appEnd == 1)
		{
			this.appEnd = ((index == 1) ? 2 : 3);
		}
		return true;
	}

	// Token: 0x060018D5 RID: 6357 RVA: 0x00131698 File Offset: 0x0012F898
	public override void Update()
	{
		if (this.gotoNextSceneInternal != null && !this.gotoNextSceneInternal.MoveNext())
		{
			this.gotoNextSceneInternal = null;
		}
	}

	// Token: 0x060018D6 RID: 6358 RVA: 0x001316B6 File Offset: 0x0012F8B6
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

	// Token: 0x060018D7 RID: 6359 RVA: 0x001316F2 File Offset: 0x0012F8F2
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

	// Token: 0x060018D8 RID: 6360 RVA: 0x00131704 File Offset: 0x0012F904
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

	// Token: 0x060018D9 RID: 6361 RVA: 0x00131754 File Offset: 0x0012F954
	public override void OnDisableScene()
	{
		this.basePanel.gameObject.SetActive(false);
		AssetManager.UnloadAssetData(DataInitializeResolver.titleLogo, AssetManager.OWNER.DataInitialize);
	}

	// Token: 0x060018DA RID: 6362 RVA: 0x00131773 File Offset: 0x0012F973
	public override void OnDestroyScene()
	{
		Object.Destroy(this.basePanel);
		this.basePanel = null;
	}

	// Token: 0x060018DB RID: 6363 RVA: 0x00131787 File Offset: 0x0012F987
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

	// Token: 0x060018DC RID: 6364 RVA: 0x001317A4 File Offset: 0x0012F9A4
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

	// Token: 0x060018DD RID: 6365 RVA: 0x001317BC File Offset: 0x0012F9BC
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

	// Token: 0x04001300 RID: 4864
	private GameObject basePanel;

	// Token: 0x04001301 RID: 4865
	private SelTitleCtrl selTitleCtrl;

	// Token: 0x04001302 RID: 4866
	private GameObject appEndObj;

	// Token: 0x04001303 RID: 4867
	private int appEnd;

	// Token: 0x04001304 RID: 4868
	private bool pvp;

	// Token: 0x04001305 RID: 4869
	public static readonly string bgm = "prd_bgm0022";

	// Token: 0x04001306 RID: 4870
	private IEnumerator gotoNextSceneInternal;

	// Token: 0x04001307 RID: 4871
	private bool isServiceCloseByMst;

	// Token: 0x04001308 RID: 4872
	private int terms;
}
