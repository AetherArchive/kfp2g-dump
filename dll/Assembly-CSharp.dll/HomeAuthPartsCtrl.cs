using System;
using System.Collections.Generic;
using SGNFW.Common;
using UnityEngine;

// Token: 0x0200014A RID: 330
public class HomeAuthPartsCtrl
{
	// Token: 0x06001262 RID: 4706 RVA: 0x000DEE28 File Offset: 0x000DD028
	public HomeAuthPartsCtrl(Transform baseTr, List<int> charaIds)
	{
		this._artsPlayers = new List<AuthPlayer>();
		this._rocketPlayers = new List<AuthPlayer>();
		this._boxPlayers = new List<AuthPlayer>();
		string text = "arts";
		string text2 = "rocket";
		string text3 = "box";
		GameObject gameObject = new GameObject();
		GameObject gameObject2 = new GameObject();
		GameObject gameObject3 = new GameObject();
		gameObject.SetParent(baseTr);
		gameObject2.SetParent(baseTr);
		gameObject3.SetParent(baseTr);
		gameObject.name = text;
		gameObject2.name = text2;
		gameObject3.name = text3;
		for (int i = 0; i < charaIds.Count; i++)
		{
			AuthPlayer authPlayer = AuthPlayer.InstantiateAuthPlayer(null, false);
			AuthPlayer authPlayer2 = AuthPlayer.InstantiateAuthPlayer(null, false);
			AuthPlayer authPlayer3 = AuthPlayer.InstantiateAuthPlayer(null, false);
			authPlayer.name = text + "_" + charaIds[i].ToString();
			authPlayer2.name = text2 + "_" + charaIds[i].ToString();
			authPlayer3.name = text3 + "_" + charaIds[i].ToString();
			authPlayer.gameObject.SetParent(gameObject);
			authPlayer2.gameObject.SetParent(gameObject2);
			authPlayer3.gameObject.SetParent(gameObject3);
			this._artsPlayers.Add(authPlayer);
			this._rocketPlayers.Add(authPlayer2);
			this._boxPlayers.Add(authPlayer3);
			this.SetAuthPlayersActive(i, true);
		}
		AssetManager.LoadAssetData(HomeAuthPartsCtrl.ARTS_STAGE_PATH, AssetManager.OWNER.IntroductionFriends, 0, null);
		AssetManager.LoadAssetData(HomeAuthPartsCtrl.GACHA_STAGE_PATH, AssetManager.OWNER.IntroductionFriends, 0, null);
		AssetManager.LoadAssetData(HomeAuthPartsCtrl.GACHA_STAGE_PATH_NIGHT, AssetManager.OWNER.IntroductionFriends, 0, null);
	}

	// Token: 0x06001263 RID: 4707 RVA: 0x000DEFD4 File Offset: 0x000DD1D4
	public void SetupStage(GameObject mainField)
	{
		this._artsStage = AssetManager.InstantiateAssetData(HomeAuthPartsCtrl.ARTS_STAGE_PATH, null).GetComponent<StagePresetCtrl>();
		this._artsStage.SettingForHomeAuth(Camera.main, mainField.transform);
		this._gachaStage = AssetManager.InstantiateAssetData(HomeAuthPartsCtrl.GACHA_STAGE_PATH, null).GetComponent<StagePresetCtrl>();
		this._gachaStage.SettingForHomeAuth(Camera.main, mainField.transform);
		this._gachaStageNight = AssetManager.InstantiateAssetData(HomeAuthPartsCtrl.GACHA_STAGE_PATH_NIGHT, null).GetComponent<StagePresetCtrl>();
		this._gachaStageNight.SettingForHomeAuth(Camera.main, mainField.transform);
		this._gachaStage.gameObject.SetActive(false);
		this._gachaStageNight.gameObject.SetActive(false);
		this._artsStage.gameObject.SetActive(false);
	}

	// Token: 0x06001264 RID: 4708 RVA: 0x000DF098 File Offset: 0x000DD298
	public void InitializeAuthPlayers(CharaStaticData chara, int idx)
	{
		this._rocketPlayers[idx].InitializeByGacha(new AuthPlayer.GachaParam.Before
		{
			effectType = AuthPlayer.GachaParam.EffectType.BLUE,
			putType = AuthPlayer.GachaParam.PutType.NORMAL,
			postActType = AuthPlayer.GachaParam.PostActType.NORMAL,
			skyType = ((chara.baseData.OriginalId == 0) ? AuthPlayer.GachaParam.SkyType.NORMAL : AuthPlayer.GachaParam.SkyType.NIGHT),
			stageData = ((chara.baseData.OriginalId == 0) ? this._gachaStage : this._gachaStageNight)
		});
		AuthPlayer.GachaParam.After after = new AuthPlayer.GachaParam.After
		{
			effectType = AuthPlayer.GachaParam.EffectType.BLUE,
			putType = AuthPlayer.GachaParam.PutType.NORMAL,
			isPromotion = false,
			itemId = chara.GetId(),
			stageData = ((chara.baseData.OriginalId == 0) ? this._gachaStage : this._gachaStageNight)
		};
		this._boxPlayers[idx].InitializeByGacha(after);
		this._artsPlayers[idx].Initialize(chara.artsData.authParam.authName, null, this._artsStage, null, null, false, false, false);
	}

	// Token: 0x06001265 RID: 4709 RVA: 0x000DF18F File Offset: 0x000DD38F
	public void SetStageActive(bool isInit)
	{
		this._gachaStage.gameObject.SetActive(isInit);
		this._gachaStageNight.gameObject.SetActive(isInit);
		this._artsStage.gameObject.SetActive(!isInit);
	}

	// Token: 0x06001266 RID: 4710 RVA: 0x000DF1C7 File Offset: 0x000DD3C7
	public void SwapGachaStage(bool isNight)
	{
		this._gachaStage.gameObject.SetActive(!isNight);
		this._gachaStageNight.gameObject.SetActive(isNight);
	}

	// Token: 0x06001267 RID: 4711 RVA: 0x000DF1EE File Offset: 0x000DD3EE
	public void DeactivateArtsStage()
	{
		this._artsStage.gameObject.SetActive(false);
	}

	// Token: 0x06001268 RID: 4712 RVA: 0x000DF204 File Offset: 0x000DD404
	public void SetAuthPlayersActive(int idx, bool isActive)
	{
		this._artsPlayers[idx].gameObject.SetActive(isActive);
		this._rocketPlayers[idx].gameObject.SetActive(isActive);
		this._boxPlayers[idx].gameObject.SetActive(isActive);
	}

	// Token: 0x06001269 RID: 4713 RVA: 0x000DF256 File Offset: 0x000DD456
	public AuthPlayer GetArtsPlayer(int index)
	{
		return this._artsPlayers[index];
	}

	// Token: 0x0600126A RID: 4714 RVA: 0x000DF264 File Offset: 0x000DD464
	public AuthPlayer GetRocketPlayer(int index)
	{
		return this._rocketPlayers[index];
	}

	// Token: 0x0600126B RID: 4715 RVA: 0x000DF272 File Offset: 0x000DD472
	public AuthPlayer GetBoxPlayer(int index)
	{
		return this._boxPlayers[index];
	}

	// Token: 0x0600126C RID: 4716 RVA: 0x000DF280 File Offset: 0x000DD480
	public bool IsArtsPlayersLoading()
	{
		return this._artsPlayers.Exists(delegate(AuthPlayer player)
		{
			if (player.IsFinishInitialize())
			{
				return player.charaList.Exists((AuthCharaData chara) => !chara.charaModelHandle.IsFinishInitialize());
			}
			return true;
		});
	}

	// Token: 0x0600126D RID: 4717 RVA: 0x000DF2AC File Offset: 0x000DD4AC
	public bool IsRocketPlayersLoading()
	{
		return this._rocketPlayers.Exists((AuthPlayer player) => !player.IsFinishInitialize());
	}

	// Token: 0x0600126E RID: 4718 RVA: 0x000DF2D8 File Offset: 0x000DD4D8
	public bool IsBoxPlayersLoading()
	{
		return this._boxPlayers.Exists((AuthPlayer player) => !player.IsFinishInitialize());
	}

	// Token: 0x0600126F RID: 4719 RVA: 0x000DF304 File Offset: 0x000DD504
	public void Destroy()
	{
		for (int i = 0; i < this._artsPlayers.Count; i++)
		{
			this._artsPlayers[i].DestroyProcessing();
			this._rocketPlayers[i].DestroyProcessing();
			this._boxPlayers[i].DestroyProcessing();
			Object.Destroy(this._artsPlayers[i].gameObject);
			Object.Destroy(this._rocketPlayers[i].gameObject);
			Object.Destroy(this._boxPlayers[i].gameObject);
			this._artsPlayers[i] = null;
			this._rocketPlayers[i] = null;
			this._boxPlayers[i] = null;
		}
		Object.Destroy(this._artsStage.gameObject);
		Object.Destroy(this._gachaStage.gameObject);
		Object.Destroy(this._gachaStageNight.gameObject);
		this._artsStage.Destory();
		this._gachaStage.Destory();
		this._gachaStageNight.Destory();
		this._artsPlayers = null;
		this._rocketPlayers = null;
		this._boxPlayers = null;
	}

	// Token: 0x04000F22 RID: 3874
	public static readonly string ARTS_STAGE_PATH = StagePresetCtrl.PackDataPath + "SD_savannahr_noon_a";

	// Token: 0x04000F23 RID: 3875
	public static readonly string GACHA_STAGE_PATH = StagePresetCtrl.PackDataPath + "SD_gacha_noon_a";

	// Token: 0x04000F24 RID: 3876
	public static readonly string GACHA_STAGE_PATH_NIGHT = StagePresetCtrl.PackDataPath + "SD_gacha_night_a";

	// Token: 0x04000F25 RID: 3877
	private List<AuthPlayer> _artsPlayers;

	// Token: 0x04000F26 RID: 3878
	private List<AuthPlayer> _rocketPlayers;

	// Token: 0x04000F27 RID: 3879
	private List<AuthPlayer> _boxPlayers;

	// Token: 0x04000F28 RID: 3880
	private StagePresetCtrl _artsStage;

	// Token: 0x04000F29 RID: 3881
	private StagePresetCtrl _gachaStage;

	// Token: 0x04000F2A RID: 3882
	private StagePresetCtrl _gachaStageNight;
}
