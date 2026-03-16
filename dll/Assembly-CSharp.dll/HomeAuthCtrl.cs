using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HomeAuthCtrl
{
	public GameObject BaseObj
	{
		get
		{
			return this._baseObj;
		}
		private set
		{
		}
	}

	private void Init(List<int> charaIds)
	{
		this._baseObj = new GameObject();
		this._baseObj.name = "MainFieldHomeAuth";
		this._homeAuthParts = new HomeAuthPartsCtrl(this._baseObj.transform, charaIds);
		this._homeAuthUICtrl = new HomeAuthUICtrl();
		this._homeAuthUICtrl.SetOnClickButtonSkipListner(new PguiButtonCtrl.OnClick(this.OnClickButtonSkip));
		this._skipFlg = false;
	}

	public IEnumerator IntroductionFriends(bool isCharacterMovie, List<int> charaIds, UnityAction onInitialized, UnityAction onComplete)
	{
		int answer = 0;
		IEnumerator ask = this.AskPermission(delegate(int action)
		{
			answer = action;
		});
		while (ask.MoveNext())
		{
			yield return null;
		}
		if (answer != 1)
		{
			onComplete();
			yield break;
		}
		IEnumerator setup = this.SetupIntroduction(charaIds);
		while (setup.MoveNext())
		{
			yield return null;
		}
		CanvasManager.HdlCmnMenu.SetActiveMenu(false);
		if (onInitialized != null)
		{
			onInitialized();
		}
		this._homeAuthUICtrl.ActivateButtonSKip();
		this._homeAuthUICtrl.ActivateOutFlame();
		CharaStaticData charaStaticData = DataManager.DmChara.GetCharaStaticData(charaIds[0]);
		if (charaStaticData.baseData.OriginalId != 0)
		{
			SoundManager.PlayBGM("prd_bgm0010_v2");
		}
		else if (charaStaticData.baseData.OriginalId == 0)
		{
			SoundManager.PlayBGM("prd_bgm0010");
		}
		IEnumerator intro = this._homeAuthUICtrl.PlayMovie(isCharacterMovie, DataManager.DmChara.GetCharaStaticData(charaIds[0]));
		while (intro.MoveNext())
		{
			if (this._skipFlg)
			{
				this._homeAuthUICtrl.HideMovieImage();
				break;
			}
			yield return null;
		}
		if (!this._skipFlg)
		{
			IEnumerator gacha = this.PlayGacha(charaIds);
			while (gacha.MoveNext())
			{
				yield return null;
			}
			gacha = null;
		}
		if (!this._skipFlg)
		{
			this._homeAuthUICtrl.ShowBannerImages();
			IEnumerator gacha = this._homeAuthUICtrl.PlayMovie(false, null);
			while (gacha.MoveNext() && !this._skipFlg)
			{
				yield return null;
			}
			gacha = null;
		}
		this._homeAuthParts.DeactivateArtsStage();
		this.DestroyAll();
		CanvasManager.HdlCmnMenu.SetActiveMenu(true);
		if (onComplete != null)
		{
			onComplete();
		}
		yield break;
	}

	private IEnumerator PlayGacha(List<int> charaIds)
	{
		int num;
		for (int i = 0; i < charaIds.Count; i = num + 1)
		{
			this._homeAuthParts.SetStageActive(false);
			AuthPlayer artsPlayer = this._homeAuthParts.GetArtsPlayer(i);
			AuthPlayer rocketPlayer = this._homeAuthParts.GetRocketPlayer(i);
			Component boxPlayer = this._homeAuthParts.GetBoxPlayer(i);
			rocketPlayer.gameObject.SetActive(false);
			boxPlayer.gameObject.SetActive(false);
			CharaStaticData chara = DataManager.DmChara.GetCharaStaticData(charaIds[i]);
			artsPlayer.PlayAuth(false);
			this._homeAuthUICtrl.SetupArtsUI(chara);
			while (artsPlayer.IsPlaying())
			{
				if (this._skipFlg)
				{
					artsPlayer.StopSound();
					SoundManager.Stop(artsPlayer.charaList[0].charaModelHandle.loadVoiceCueSheetName);
					artsPlayer.ForceFinish();
					break;
				}
				yield return null;
			}
			if (this._skipFlg)
			{
				break;
			}
			this._homeAuthUICtrl.HideArtsUI();
			IEnumerator greeting = this.PlayGreeting(chara.GetId());
			while (greeting.MoveNext())
			{
				yield return null;
			}
			this._homeAuthParts.SetAuthPlayersActive(i, false);
			if (i == charaIds.Count - 1 || this._skipFlg)
			{
				break;
			}
			this._homeAuthParts.SetStageActive(true);
			this._homeAuthParts.SetAuthPlayersActive(i + 1, true);
			CharaStaticData charaStaticData = DataManager.DmChara.GetCharaStaticData(charaIds[i + 1]);
			this._homeAuthParts.SwapGachaStage(charaStaticData.baseData.OriginalId != 0);
			AuthPlayer nextRocketPlayer = this._homeAuthParts.GetRocketPlayer(i + 1);
			nextRocketPlayer.PlayAuth(false);
			if (charaStaticData.baseData.OriginalId != 0)
			{
				SoundManager.PlayBGM("prd_bgm0010_v2");
			}
			else if (charaStaticData.baseData.OriginalId == 0)
			{
				SoundManager.PlayBGM("prd_bgm0010");
			}
			while (nextRocketPlayer.IsPlaying())
			{
				if (this._skipFlg)
				{
					nextRocketPlayer.StopSound();
					nextRocketPlayer.ForceFinish();
					break;
				}
				yield return null;
			}
			if (this._skipFlg)
			{
				break;
			}
			AuthPlayer nextBoxPlayer = this._homeAuthParts.GetBoxPlayer(i + 1);
			nextBoxPlayer.PlayAuth(false);
			while (nextBoxPlayer.IsPlaying())
			{
				if (this._skipFlg)
				{
					nextBoxPlayer.StopSound();
					nextBoxPlayer.ForceFinish();
					break;
				}
				yield return null;
			}
			artsPlayer = null;
			chara = null;
			greeting = null;
			nextRocketPlayer = null;
			nextBoxPlayer = null;
			num = i;
		}
		yield break;
	}

	private IEnumerator SetupIntroduction(List<int> charaIds)
	{
		CanvasManager.HdlLoadAndTipsCtrl.SetDispLoading(true);
		this.Init(charaIds);
		while (!AssetManager.IsLoadFinishAssetData(HomeAuthPartsCtrl.ARTS_STAGE_PATH))
		{
			yield return null;
		}
		while (!AssetManager.IsLoadFinishAssetData(HomeAuthPartsCtrl.GACHA_STAGE_PATH))
		{
			yield return null;
		}
		while (!AssetManager.IsLoadFinishAssetData(HomeAuthPartsCtrl.GACHA_STAGE_PATH_NIGHT))
		{
			yield return null;
		}
		this._homeAuthParts.SetupStage(this._baseObj);
		for (int i = 0; i < charaIds.Count; i++)
		{
			CharaStaticData charaStaticData = DataManager.DmChara.GetCharaStaticData(charaIds[i]);
			this._homeAuthParts.InitializeAuthPlayers(charaStaticData, i);
		}
		while (this._homeAuthParts.IsArtsPlayersLoading())
		{
			yield return null;
		}
		while (this._homeAuthParts.IsRocketPlayersLoading())
		{
			yield return null;
		}
		while (this._homeAuthParts.IsBoxPlayersLoading())
		{
			yield return null;
		}
		this._homeAuthUICtrl.SetupBannerImages();
		IEnumerator ienum = AssetDownloadResolver.ResolveActionIntroductionMovie();
		while (ienum.MoveNext())
		{
			yield return null;
		}
		for (int j = 0; j < charaIds.Count; j++)
		{
			this._homeAuthParts.SetAuthPlayersActive(j, false);
		}
		CanvasManager.HdlLoadAndTipsCtrl.SetDispLoading(false);
		this._homeAuthParts.SetAuthPlayersActive(0, true);
		yield break;
	}

	private void OnClickButtonSkip(PguiButtonCtrl button)
	{
		if (this._homeAuthUICtrl.ButtonSkip == null)
		{
			return;
		}
		if (button == this._homeAuthUICtrl.ButtonSkip)
		{
			this._skipFlg = true;
		}
	}

	private void DestroyAll()
	{
		this._homeAuthParts.Destroy();
		this._homeAuthParts = null;
		this._homeAuthUICtrl.Destroy();
		this._homeAuthUICtrl = null;
		Object.Destroy(this._baseObj);
		this._baseObj = null;
		AssetManager.UnloadAssetData(HomeAuthPartsCtrl.ARTS_STAGE_PATH, AssetManager.OWNER.IntroductionFriends);
		AssetManager.UnloadAssetData(HomeAuthPartsCtrl.GACHA_STAGE_PATH, AssetManager.OWNER.IntroductionFriends);
		AssetManager.UnloadAssetData(HomeAuthPartsCtrl.GACHA_STAGE_PATH_NIGHT, AssetManager.OWNER.IntroductionFriends);
		PrjUtil.ReleaseMemory(PrjUtil.Garbagecollection);
	}

	private IEnumerator AskPermission(Action<int> answerAction)
	{
		bool isWindowFinish = false;
		int answer = 0;
		CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("新フレンズ紹介"), PrjUtil.MakeMessage("新フレンズ紹介の動画を再生します。\n\n <color=red> ※データのダウンロードが発生する場合がある為、\nWi-Fi環境など、通信の安定した状態を推奨します。</color>\n\n再生しますか？"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, delegate(int index)
		{
			answer = index;
			isWindowFinish = true;
			return true;
		}, null, false);
		CanvasManager.HdlOpenWindowBasic.Open();
		while (!isWindowFinish)
		{
			yield return null;
		}
		answerAction(answer);
		yield break;
	}

	public IEnumerator PlayGreeting(int charaId)
	{
		CanvasManager.SetBgTexture("selbg_Gacha");
		CanvasManager.SetEnableCmnTouchMask(true);
		GachaAuthCtrl playGachaAuth = new GachaAuthCtrl();
		playGachaAuth.Initialize();
		yield return null;
		SoundManager.SetTempVolume(0.2f);
		IEnumerator greeting = playGachaAuth.PlayGreetingForOtherScene(new List<GachaAuthCtrl.AuthItem>
		{
			new GachaAuthCtrl.AuthItem
			{
				isNew = true,
				itemId = charaId
			}
		}, true);
		while (greeting.MoveNext())
		{
			if (this._skipFlg)
			{
				DataManager.DmGacha.LatestGreetingVoice.Stop();
				break;
			}
			yield return null;
		}
		playGachaAuth.DestroyAllObject();
		playGachaAuth = null;
		SoundManager.ReturnOrgVolume();
		CanvasManager.SetEnableCmnTouchMask(false);
		yield break;
	}

	private HomeAuthPartsCtrl _homeAuthParts;

	private HomeAuthUICtrl _homeAuthUICtrl;

	private GameObject _baseObj;

	private bool _skipFlg;
}
