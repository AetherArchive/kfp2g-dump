using System;
using System.Collections;
using System.Collections.Generic;
using CriWare;
using SGNFW.Ab;
using SGNFW.Common;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000140 RID: 320
public class GachaAuthCtrl
{
	// Token: 0x06001190 RID: 4496 RVA: 0x000D4B28 File Offset: 0x000D2D28
	public void Initialize()
	{
		this.IsDestroy = false;
		if (null == this.MainFieldGachaAuth)
		{
			this.MainFieldGachaAuth = new GameObject();
			this.MainFieldGachaAuth.name = "MainFieldGachaAuth";
		}
		this.gachaAuth_common = AuthPlayer.InstantiateAuthPlayer(null, false);
		this.gachaAuth_common.name = "CommonAuth";
		this.gachaAuth_common.gameObject.SetActive(false);
		this.gachaAuth_common.transform.SetParent(this.MainFieldGachaAuth.transform);
		if (this.gachaAuthFirstList == null)
		{
			this.gachaAuthFirstList = new List<AuthPlayer>();
			for (int i = 0; i < 10; i++)
			{
				AuthPlayer authPlayer = AuthPlayer.InstantiateAuthPlayer(null, false);
				authPlayer.name = "FirstAuth" + i.ToString();
				authPlayer.gameObject.SetActive(false);
				authPlayer.transform.SetParent(this.MainFieldGachaAuth.transform);
				this.gachaAuthFirstList.Add(authPlayer);
			}
		}
		if (this.gachaAuthSecondList == null)
		{
			this.gachaAuthSecondList = new List<AuthPlayer>();
			for (int j = 0; j < 10; j++)
			{
				AuthPlayer authPlayer2 = AuthPlayer.InstantiateAuthPlayer(null, false);
				authPlayer2.name = "SecondAuth" + j.ToString();
				authPlayer2.gameObject.SetActive(false);
				authPlayer2.transform.SetParent(this.MainFieldGachaAuth.transform);
				this.gachaAuthSecondList.Add(authPlayer2);
			}
		}
		if (this.gachaAuth_miracle == null)
		{
			this.gachaAuth_miracle = new List<AuthPlayer>();
			for (int k = 0; k < 10; k++)
			{
				AuthPlayer authPlayer3 = AuthPlayer.InstantiateAuthPlayer(null, false);
				authPlayer3.name = "MiracleAuth" + k.ToString();
				authPlayer3.gameObject.SetActive(false);
				authPlayer3.transform.SetParent(this.MainFieldGachaAuth.transform);
				this.gachaAuth_miracle.Add(authPlayer3);
			}
		}
		GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneGacha/GUI/Prefab/GUI_GachaAuth"));
		this.guiDataGachaAuth = new GachaAuthCtrl.GUIGachaAuth(gameObject.transform);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this.guiDataGachaAuth.baseObj.transform, true);
		this.guiDataGachaAuth.baseObj.transform.SetParent(Singleton<CanvasManager>.Instance.SystemPanel.transform, true);
		this.guiDataGachaAuth.baseObj.transform.SetSiblingIndex(CanvasManager.HdlLoadAndTipsCtrl.transform.GetSiblingIndex());
		this.gachaAeGreeting = new GachaAuthCtrl.GachaAeGreeting(this.guiDataGachaAuth.baseObj.transform.Find("NewComer"));
		this.gachaAeGreeting.Txt_WName.gameObject.SetActive(false);
		this.gachaAeGreeting.baseObj.SetActive(false);
		this.gachaWipe_Top = this.guiDataGachaAuth.baseObj.transform.Find("NewComer_Wipe_Top").gameObject;
		this.AEImage_Wipe_Top = this.gachaWipe_Top.transform.Find("AEImage").GetComponent<PguiAECtrl>();
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this.gachaWipe_Top.transform, true);
		this.gachaWipe_Top.transform.SetParent(Singleton<CanvasManager>.Instance.SystemPanel.transform, true);
		this.gachaWipe_Top.transform.SetSiblingIndex(CanvasManager.HdlLoadAndTipsCtrl.transform.GetSiblingIndex());
		this.AEImage_Wipe_Top.PauseAnimation(PguiAECtrl.AmimeType.START, null);
		this.gachaWipe_Top.gameObject.SetActive(false);
		this.gachaWipe_Change = this.guiDataGachaAuth.baseObj.transform.Find("NewComer_Wipe_Change").gameObject;
		this.AEImage_Wipe_Change = this.gachaWipe_Change.transform.Find("AEImage").GetComponent<PguiAECtrl>();
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this.gachaWipe_Change.transform, true);
		this.gachaWipe_Change.transform.SetParent(Singleton<CanvasManager>.Instance.SystemPanel.transform, true);
		this.gachaWipe_Change.transform.SetSiblingIndex(CanvasManager.HdlLoadAndTipsCtrl.transform.GetSiblingIndex());
		this.gachaWipe_Change.gameObject.SetActive(false);
		this.gachaWipe_End = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneGacha/GUI/Prefab/GUI_GachaEnd_Wipe"));
		this.AEImage_Wipe_End = this.gachaWipe_End.transform.Find("AEImage").GetComponent<PguiAECtrl>();
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this.gachaWipe_End.transform, true);
		this.gachaWipe_End.transform.SetParent(Singleton<CanvasManager>.Instance.SystemPanel.transform, true);
		this.gachaWipe_End.transform.SetSiblingIndex(CanvasManager.HdlLoadAndTipsCtrl.transform.GetSiblingIndex());
		this.gachaWipe_End.gameObject.SetActive(false);
		this.guiDataGachaAuth.skipButton.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiDataGachaAuth.skipButton.gameObject.SetActive(false);
		PrjUtil.AddTouchEventTrigger(this.guiDataGachaAuth.baseObj, delegate(Transform x)
		{
			this.touchByGachaAuth = true;
		});
		this.guiDataGachaAuth.baseObj.SetActive(false);
	}

	// Token: 0x06001191 RID: 4497 RVA: 0x000D502C File Offset: 0x000D322C
	public IEnumerator SetupPlayAuth(DataManagerGacha.PlayResult playResult, bool tutorialFlg = false)
	{
		List<GachaAuthCtrl.AuthItem> list = new List<GachaAuthCtrl.AuthItem>();
		foreach (DataManagerGacha.PlayResult.OneData oneData in playResult.gachaResult)
		{
			ItemDef.Kind kind = ItemDef.Id2Kind(oneData.itemId);
			if (kind == ItemDef.Kind.CHARA || kind == ItemDef.Kind.PHOTO || kind == ItemDef.Kind.TREEHOUSE_FURNITURE)
			{
				GachaAuthCtrl.AuthItem authItem = new GachaAuthCtrl.AuthItem
				{
					itemId = oneData.itemId,
					isNew = oneData.isNew,
					replaced = oneData.replaced,
					replaceItem = oneData.replaceItem,
					replaceItemEx = oneData.replaceItemEx,
					replaceItemIsNew = oneData.replaceItemIsNew,
					replaceItemExIsNew = oneData.replaceItemExIsNew
				};
				list.Add(authItem);
			}
		}
		IEnumerator ienum = this.PlayAuth(list, playResult.gachaId, tutorialFlg);
		while (ienum.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001192 RID: 4498 RVA: 0x000D5049 File Offset: 0x000D3249
	public IEnumerator PlayCharaPresentAuth(DataManagerPresent.UserPresentData presentData, List<int> haveCharaId, List<ItemData> replaceItemList)
	{
		List<GachaAuthCtrl.AuthItem> list = new List<GachaAuthCtrl.AuthItem>();
		DataManager.DmChara.GetCharaStaticData(presentData.itemId);
		GachaAuthCtrl.AuthItem authItem = new GachaAuthCtrl.AuthItem();
		authItem.itemId = presentData.itemId;
		authItem.isNew = !haveCharaId.Contains(presentData.itemId);
		authItem.replaced = replaceItemList.Count > 0;
		if (authItem.replaced)
		{
			authItem.replaceItem = replaceItemList.Find((ItemData data) => data.id != 100003);
			authItem.replaceItemEx = replaceItemList.Find((ItemData data) => data.id == 100003);
		}
		list.Add(authItem);
		IEnumerator ienum = this.PlayAuth(list, 0, false);
		while (ienum.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001193 RID: 4499 RVA: 0x000D506D File Offset: 0x000D326D
	public IEnumerator PlayGreetingForOtherScene(List<GachaAuthCtrl.AuthItem> authItemList, bool isHomeAuth = false)
	{
		GachaAuthCtrl.<>c__DisplayClass29_0 CS$<>8__locals1 = new GachaAuthCtrl.<>c__DisplayClass29_0();
		CS$<>8__locals1.<>4__this = this;
		this.guiDataGachaAuth.baseObj.SetActive(true);
		bool oldFlag = this.guiDataGachaAuth.skipButton.gameObject.activeSelf;
		this.guiDataGachaAuth.skipButton.gameObject.SetActive(false);
		CS$<>8__locals1.loadFinish = false;
		PguiRawImageCtrl bgImage = this.guiDataGachaAuth.baseObj.transform.Find("BG").GetComponent<PguiRawImageCtrl>();
		bgImage.SetRawImage("Texture2D/Bg_Scene/selbg_Gacha", true, false, delegate
		{
			CS$<>8__locals1.loadFinish = true;
		});
		while (!CS$<>8__locals1.loadFinish)
		{
			yield return null;
		}
		IEnumerator ienum = this.PlayGreeting(authItemList, false, false, !isHomeAuth, bgImage.gameObject, isHomeAuth);
		while (ienum.MoveNext())
		{
			yield return null;
		}
		ienum = null;
		this.gachaAeGreeting.baseObj.SetActive(false);
		bgImage.gameObject.SetActive(false);
		if (!isHomeAuth)
		{
			GachaAuthCtrl.<>c__DisplayClass29_1 CS$<>8__locals2 = new GachaAuthCtrl.<>c__DisplayClass29_1();
			CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
			CS$<>8__locals2.isPlay = true;
			this.AEImage_Wipe_End.PlayAnimation(PguiAECtrl.AmimeType.END, delegate
			{
				CS$<>8__locals2.isPlay = false;
				CS$<>8__locals2.CS$<>8__locals1.<>4__this.gachaWipe_End.gameObject.SetActive(false);
			});
			while (CS$<>8__locals2.isPlay)
			{
				yield return null;
			}
			CS$<>8__locals2 = null;
		}
		this.guiDataGachaAuth.skipButton.gameObject.SetActive(oldFlag);
		this.guiDataGachaAuth.baseObj.SetActive(false);
		PrjUtil.ReleaseMemory(PrjUtil.Garbagecollection);
		yield break;
	}

	// Token: 0x06001194 RID: 4500 RVA: 0x000D508A File Offset: 0x000D328A
	private IEnumerator PlayAuth(List<GachaAuthCtrl.AuthItem> authItemList, int gachaId, bool tutorialFlg)
	{
		this.loadData = null;
		this.playAuthIdx = 0;
		IEnumerator ienum = this.PlayAuthInternal(authItemList, gachaId, tutorialFlg);
		while (!this.IsDestroy && ienum.MoveNext())
		{
			if (this.loadData != null && !this.loadData.MoveNext())
			{
				this.loadData = null;
			}
			yield return null;
		}
		PrjUtil.ReleaseMemory(PrjUtil.Garbagecollection);
		yield break;
	}

	// Token: 0x06001195 RID: 4501 RVA: 0x000D50AE File Offset: 0x000D32AE
	private IEnumerator DispLoading(GachaAuthCtrl.cbDispLoading cbEnd)
	{
		DateTime dt = TimeManager.SystemNow;
		while (!this.IsDestroy)
		{
			if (cbEnd())
			{
				yield break;
			}
			if ((TimeManager.SystemNow - dt).TotalSeconds > 1.0)
			{
				break;
			}
			yield return null;
		}
		CanvasManager.HdlLoadAndTipsCtrl.SetDispLoading(true);
		while (!this.IsDestroy)
		{
			if (cbEnd())
			{
				CanvasManager.HdlLoadAndTipsCtrl.SetDispLoading(false);
				yield break;
			}
			if ((TimeManager.SystemNow - dt).TotalSeconds > 2.0)
			{
				break;
			}
			yield return null;
		}
		float loader = Manager.LoadProgress;
		float progress = 0f;
		CanvasManager.HdlLoadAndTipsCtrl.Setup(new LoadAndTipsCtrl.SetupParam
		{
			isDispProgress = true,
			cbGetProgress = () => progress
		});
		while (!this.IsDestroy)
		{
			float num = ((float)(TimeManager.SystemNow - dt).TotalSeconds - 2f) / 60f;
			if (cbEnd())
			{
				break;
			}
			float loadProgress = Manager.LoadProgress;
			if (loader != loadProgress)
			{
				loader = loadProgress;
				num = Mathf.Clamp01(1f - num);
				if ((progress = 1f - num * num * num) > 0.99f)
				{
					progress = 0.99f;
				}
			}
			yield return null;
		}
		CanvasManager.HdlLoadAndTipsCtrl.SetDispLoading(false);
		CanvasManager.HdlLoadAndTipsCtrl.Close(true);
		yield break;
	}

	// Token: 0x06001196 RID: 4502 RVA: 0x000D50C4 File Offset: 0x000D32C4
	private IEnumerator PlayAuthInternal(List<GachaAuthCtrl.AuthItem> authItemList, int gachaId, bool tutorialFlg)
	{
		GachaAuthCtrl.<>c__DisplayClass33_0 CS$<>8__locals1 = new GachaAuthCtrl.<>c__DisplayClass33_0();
		CS$<>8__locals1.<>4__this = this;
		bool isGacha = gachaId != 0;
		this.gachaAllSkipFlag = false;
		this.guiDataGachaAuth.baseObj.SetActive(true);
		IEnumerator dispLoading = null;
		bool furnitureOnly = false;
		if (isGacha)
		{
			DataManagerGacha.GachaStaticData gachaStaticData = DataManager.DmGacha.GetGachaStaticData(gachaId);
			furnitureOnly = !gachaStaticData.gachaItemData.Exists((DataManagerGacha.GachaItemdata x) => ItemDef.Kind.TREEHOUSE_FURNITURE != ItemDef.Id2Kind(x.itemId));
		}
		AuthPlayer.GachaParam.Before beforeGachaParam;
		List<AuthPlayer.GachaParam.After> afterParamList;
		if (isGacha)
		{
			if (furnitureOnly)
			{
				beforeGachaParam = this.GetBeforeGachaParam(authItemList, new FurnitureOnlyResultRarity());
				afterParamList = this.GetAfterGachaParam(authItemList, beforeGachaParam, new FurnitureOnlyResultRarity());
			}
			else
			{
				beforeGachaParam = this.GetBeforeGachaParam(authItemList, new DefaultResultRarity());
				afterParamList = this.GetAfterGachaParam(authItemList, beforeGachaParam, new DefaultResultRarity());
			}
		}
		else
		{
			beforeGachaParam = this.GetBeforeGachaParamByPresentBox(authItemList);
			afterParamList = this.GetAfterGachaParam(authItemList, beforeGachaParam, new DefaultResultRarity());
		}
		string text = ((beforeGachaParam.skyType == AuthPlayer.GachaParam.SkyType.NORMAL) ? "SD_gacha_noon_b" : "SD_gacha_night_b");
		CS$<>8__locals1.stageGacha = StagePresetCtrl.PackDataPath + text;
		AssetManager.LoadAssetData(CS$<>8__locals1.stageGacha, AssetManager.OWNER.AuthGacha, 0, null);
		CS$<>8__locals1.stageSavnnahrNoon = StagePresetCtrl.PackDataPath + "SD_savannahr_noon_a";
		AssetManager.LoadAssetData(CS$<>8__locals1.stageSavnnahrNoon, AssetManager.OWNER.AuthGacha, 0, null);
		dispLoading = this.DispLoading(() => AssetManager.IsLoadFinishAssetData(CS$<>8__locals1.stageGacha) && AssetManager.IsLoadFinishAssetData(CS$<>8__locals1.stageSavnnahrNoon));
		while (dispLoading.MoveNext())
		{
			yield return null;
		}
		this.gachaAuthStage = AssetManager.InstantiateAssetData(CS$<>8__locals1.stageGacha, null).GetComponent<StagePresetCtrl>();
		this.gachaAuthStage.Setting(Camera.main);
		this.gachaAuthStage.transform.localEulerAngles = new Vector3(0f, 10f, 0f);
		this.gachaAuthStage.transform.SetParent(this.MainFieldGachaAuth.transform);
		this.miracleStage = AssetManager.InstantiateAssetData(CS$<>8__locals1.stageSavnnahrNoon, null).GetComponent<StagePresetCtrl>();
		this.miracleStage.Setting(Camera.main);
		this.miracleStage.transform.localEulerAngles = new Vector3(0f, 10f, 0f);
		this.miracleStage.gameObject.SetActive(false);
		this.miracleStage.transform.SetParent(this.MainFieldGachaAuth.transform);
		this.loadData = this.LoadAuth(authItemList, afterParamList);
		this.loadData.MoveNext();
		if (!tutorialFlg)
		{
			this.guiDataGachaAuth.skipButton.gameObject.SetActive(true);
		}
		if (isGacha)
		{
			this.gachaAuthStage.gameObject.SetActive(true);
			beforeGachaParam.stageData = this.gachaAuthStage;
			this.gachaAuth_common.gameObject.SetActive(true);
			this.gachaAuth_common.InitializeByGacha(beforeGachaParam);
			dispLoading = this.DispLoading(() => CS$<>8__locals1.<>4__this.gachaAuth_common.IsFinishInitialize());
			while (dispLoading.MoveNext())
			{
				yield return null;
			}
			this.gachaAuth_common.PlayAuth(false);
			int gachaResultRank4PuRandomItemId = DataManagerGacha.GetGachaResultRank4PuRandomItemId();
			int num = new Random().Next(100);
			string text2;
			if (furnitureOnly)
			{
				text2 = this.GetPremiumSEName(authItemList, new FurnitureOnlyResultRarity());
			}
			else
			{
				text2 = this.GetPremiumSEName(authItemList, new DefaultResultRarity());
				CharaStaticData charaStaticData = DataManager.DmChara.GetCharaStaticData(gachaResultRank4PuRandomItemId);
				if ((charaStaticData != null) & (num < 50))
				{
					SoundManager.LoadCueSheet(charaStaticData.cueSheetName);
					DataManager.DmGacha.LatestGreetingVoice = SoundManager.PlayVoice(charaStaticData.cueSheetName, VOICE_TYPE.SPE01);
				}
			}
			if (!string.IsNullOrEmpty(text2))
			{
				SoundManager.Play(text2, false, false);
			}
			this.touchByGachaAuth = false;
			while (!this.gachaAuth_common.IsFinished() && !this.gachaAllSkipFlag && !this.touchByGachaAuth)
			{
				yield return null;
			}
		}
		CS$<>8__locals1.playAe = null;
		GachaAuthCtrl.<>c__DisplayClass33_1 CS$<>8__locals2 = new GachaAuthCtrl.<>c__DisplayClass33_1();
		CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
		CS$<>8__locals2.i = 0;
		while (CS$<>8__locals2.i < authItemList.Count)
		{
			GachaAuthCtrl.<>c__DisplayClass33_2 CS$<>8__locals3 = new GachaAuthCtrl.<>c__DisplayClass33_2();
			CS$<>8__locals3.CS$<>8__locals2 = CS$<>8__locals2;
			this.playAuthIdx = CS$<>8__locals3.CS$<>8__locals2.i;
			CS$<>8__locals3.newAe = null;
			bool bonusChara = false;
			bool bonusPhoto = false;
			bool dropBonusChara = false;
			bool haveNickName = false;
			ItemData getItem = null;
			ItemData getItemEx = null;
			bool getItemIsNew = true;
			bool getItemExIsNew = true;
			string playSe = "";
			ItemDef.Kind kind = ItemDef.Id2Kind(authItemList[CS$<>8__locals3.CS$<>8__locals2.i].itemId);
			if (kind != ItemDef.Kind.CHARA)
			{
				goto IL_0981;
			}
			CharaStaticData charaStaticData2 = DataManager.DmChara.GetCharaStaticData(authItemList[CS$<>8__locals3.CS$<>8__locals2.i].itemId);
			int rankLow = charaStaticData2.baseData.rankLow;
			if (rankLow >= 4)
			{
				CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.playAe = this.guiDataGachaAuth.ResultFriendsList[3];
				playSe = "prd_se_gacha_friends_rank2d_3";
			}
			else if (rankLow == 3)
			{
				CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.playAe = this.guiDataGachaAuth.ResultFriendsList[2];
				playSe = "prd_se_gacha_friends_rank2d_2";
			}
			else if (rankLow == 2)
			{
				CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.playAe = this.guiDataGachaAuth.ResultFriendsList[1];
				playSe = "prd_se_gacha_friends_rank2d_1";
			}
			else
			{
				CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.playAe = this.guiDataGachaAuth.ResultFriendsList[0];
				playSe = "prd_se_gacha_friends_rank2d_1";
			}
			CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.playAe.transform.parent.Find("NameAll/Txt_FriendsName").GetComponent<PguiTextCtrl>().text = charaStaticData2.GetName();
			if (!string.IsNullOrEmpty(charaStaticData2.baseData.NickName))
			{
				this.guiDataGachaAuth.ResultFriends_WName.Txt_WName.text = charaStaticData2.baseData.NickName;
				this.guiDataGachaAuth.ResultFriends_WName.Txt_WName_White.text = charaStaticData2.baseData.NickName;
				int attribute = (int)charaStaticData2.baseData.attribute;
				this.guiDataGachaAuth.ResultFriends_WName.repCtrl.Replace(attribute.ToString());
				playSe = "prd_se_gacha_friends_ver2";
				haveNickName = true;
			}
			if (authItemList[CS$<>8__locals3.CS$<>8__locals2.i].isNew)
			{
				CS$<>8__locals3.newAe = this.guiDataGachaAuth.AEImage_New_Chara;
			}
			if (authItemList[CS$<>8__locals3.CS$<>8__locals2.i].replaced)
			{
				getItem = authItemList[CS$<>8__locals3.CS$<>8__locals2.i].replaceItem;
				getItemIsNew = authItemList[CS$<>8__locals3.CS$<>8__locals2.i].replaceItemIsNew;
				if (authItemList[CS$<>8__locals3.CS$<>8__locals2.i].replaceItemEx != null)
				{
					getItemEx = authItemList[CS$<>8__locals3.CS$<>8__locals2.i].replaceItemEx;
					getItemExIsNew = authItemList[CS$<>8__locals3.CS$<>8__locals2.i].replaceItemExIsNew;
				}
			}
			if (!tutorialFlg)
			{
				using (List<DataManagerChara.BonusCharaData>.Enumerator enumerator = DataManager.DmChara.GetBonusCharaDataList().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						DataManagerChara.BonusCharaData bonusCharaData = enumerator.Current;
						if (charaStaticData2.GetId() == bonusCharaData.charaId)
						{
							bool flag = bonusCharaData.hpBonusRatio != 0 || bonusCharaData.strBonusRatio != 0 || bonusCharaData.defBonusRatio != 0 || bonusCharaData.kizunaBonusRatio != 0;
							bonusChara = flag;
							dropBonusChara = bonusCharaData.increaseItemId01 != 0 || bonusCharaData.increaseItemId02 != 0;
							break;
						}
					}
					goto IL_0CB6;
				}
				goto IL_0981;
			}
			goto IL_0CB6;
			IL_1B9C:
			int i = CS$<>8__locals2.i;
			CS$<>8__locals2.i = i + 1;
			continue;
			IL_0981:
			if (kind == ItemDef.Kind.PHOTO)
			{
				PhotoStaticData photoStaticData = DataManager.DmPhoto.GetPhotoStaticData(authItemList[CS$<>8__locals3.CS$<>8__locals2.i].itemId);
				switch (photoStaticData.baseData.rarity)
				{
				case ItemDef.Rarity.STAR2:
					CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.playAe = this.guiDataGachaAuth.ResultPhotoList[1];
					playSe = "prd_se_gacha_photo_rank2d_1";
					break;
				case ItemDef.Rarity.STAR3:
					CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.playAe = this.guiDataGachaAuth.ResultPhotoList[2];
					playSe = "prd_se_gacha_photo_rank2d_2";
					break;
				case ItemDef.Rarity.STAR4:
					CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.playAe = this.guiDataGachaAuth.ResultPhotoList[3];
					playSe = "prd_se_gacha_photo_rank2d_3";
					break;
				default:
					CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.playAe = this.guiDataGachaAuth.ResultPhotoList[0];
					playSe = "prd_se_gacha_photo_rank2d_1";
					break;
				}
				CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.playAe.transform.parent.Find("Card_Photo/Icon").GetComponent<IconPhotoCtrl>().Setup(photoStaticData, SortFilterDefine.SortType.LEVEL, true, false, -1, 0, false);
				if (authItemList[CS$<>8__locals3.CS$<>8__locals2.i].isNew)
				{
					CS$<>8__locals3.newAe = this.guiDataGachaAuth.AEImage_New_Photo;
				}
				List<int> photoBonusTargetItemIdByTime = DataManager.DmPhoto.GetPhotoBonusTargetItemIdByTime(photoStaticData.GetId(), TimeManager.Now);
				bonusPhoto = photoBonusTargetItemIdByTime.Count > 0;
			}
			else
			{
				if (kind == ItemDef.Kind.TREEHOUSE_FURNITURE)
				{
					TreeHouseFurnitureStatic treeHouseFurnitureStaticData = DataManager.DmTreeHouse.GetTreeHouseFurnitureStaticData(authItemList[CS$<>8__locals3.CS$<>8__locals2.i].itemId);
					switch (treeHouseFurnitureStaticData.GetRarity())
					{
					case ItemDef.Rarity.STAR3:
						goto IL_0C0D;
					case ItemDef.Rarity.STAR4:
						CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.playAe = this.guiDataGachaAuth.ResultFurnitureList[1];
						playSe = "prd_se_gacha_interior_rank2d_2";
						break;
					case ItemDef.Rarity.STAR5:
						CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.playAe = this.guiDataGachaAuth.ResultFurnitureList[2];
						playSe = "prd_se_gacha_interior_rank2d_3";
						break;
					default:
						goto IL_0C0D;
					}
					IL_0C3E:
					CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.playAe.transform.parent.Find("Card_Interior/Gacha_InteriorGacha_AuthParts/Card_Interior").GetComponent<IconTreeHouseFurnitureCtrl>().Setup(new IconTreeHouseFurnitureCtrl.SetupParam
					{
						thfs = treeHouseFurnitureStaticData
					});
					if (authItemList[CS$<>8__locals3.CS$<>8__locals2.i].isNew)
					{
						CS$<>8__locals3.newAe = this.guiDataGachaAuth.AEImage_New_Interior;
						goto IL_0CB6;
					}
					goto IL_0CB6;
					IL_0C0D:
					CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.playAe = this.guiDataGachaAuth.ResultFurnitureList[0];
					playSe = "prd_se_gacha_interior_rank2d_1";
					goto IL_0C3E;
				}
				goto IL_1B9C;
			}
			IL_0CB6:
			CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.playAe.transform.parent.gameObject.SetActive(false);
			if (!this.gachaAllSkipFlag)
			{
				PguiAECtrl playAe = CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.playAe;
				PguiAECtrl.AmimeType amimeType = PguiAECtrl.AmimeType.START;
				PguiAECtrl.FinishCallback finishCallback;
				if ((finishCallback = CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>9__5) == null)
				{
					finishCallback = (CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>9__5 = delegate
					{
						CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.playAe.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
					});
				}
				playAe.PauseAnimation(amimeType, finishCallback);
				GachaAuthCtrl.cbDispLoading cbDispLoading;
				if ((cbDispLoading = CS$<>8__locals3.CS$<>8__locals2.<>9__6) == null)
				{
					cbDispLoading = (CS$<>8__locals3.CS$<>8__locals2.<>9__6 = () => CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.gachaAuthFirstList[CS$<>8__locals3.CS$<>8__locals2.i].gameObject.activeSelf && CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.gachaAuthFirstList[CS$<>8__locals3.CS$<>8__locals2.i].IsFinishInitialize());
				}
				dispLoading = this.DispLoading(cbDispLoading);
				while (dispLoading.MoveNext())
				{
					yield return null;
				}
				this.gachaAuthFirstList[CS$<>8__locals3.CS$<>8__locals2.i].PlayAuth(false);
				DateTime dt = TimeManager.SystemNow;
				while ((TimeManager.SystemNow - dt).TotalSeconds < 0.10000000149011612)
				{
					yield return null;
				}
				this.gachaAuth_common.gameObject.SetActive(false);
				if (CS$<>8__locals3.CS$<>8__locals2.i > 0)
				{
					int num2 = CS$<>8__locals3.CS$<>8__locals2.i - 1;
					this.gachaAuthFirstList[num2].PlayFinishProcess();
					this.gachaAuthFirstList[num2].DestroyProcessing();
					this.gachaAuthFirstList[num2].gameObject.SetActive(false);
					if (this.gachaAuthSecondList[num2] != null)
					{
						this.gachaAuthSecondList[num2].PlayFinishProcess();
						this.gachaAuthSecondList[num2].DestroyProcessing();
						this.gachaAuthSecondList[num2].gameObject.SetActive(false);
					}
				}
				this.touchByGachaAuth = false;
				while (!this.gachaAuthFirstList[CS$<>8__locals3.CS$<>8__locals2.i].IsFinished() && !this.gachaAllSkipFlag && !this.touchByGachaAuth)
				{
					yield return null;
				}
				if (!this.gachaAllSkipFlag)
				{
					if (kind == ItemDef.Kind.CHARA)
					{
						if (null != CS$<>8__locals3.newAe)
						{
							GachaAuthCtrl.cbDispLoading cbDispLoading2;
							if ((cbDispLoading2 = CS$<>8__locals3.CS$<>8__locals2.<>9__7) == null)
							{
								cbDispLoading2 = (CS$<>8__locals3.CS$<>8__locals2.<>9__7 = () => CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.gachaAuthSecondList[CS$<>8__locals3.CS$<>8__locals2.i].gameObject.activeSelf && CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.gachaAuthSecondList[CS$<>8__locals3.CS$<>8__locals2.i].IsFinishInitialize() && CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.gachaAuth_miracle[CS$<>8__locals3.CS$<>8__locals2.i].gameObject.activeSelf && CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.gachaAuth_miracle[CS$<>8__locals3.CS$<>8__locals2.i].IsFinishInitialize());
							}
							dispLoading = this.DispLoading(cbDispLoading2);
							while (dispLoading.MoveNext())
							{
								yield return null;
							}
							SoundManager.SetTempVolume(0.2f);
							SoundManager.Play("prd_se_gacha_new_arrival_miracle", false, false);
							this.gachaAuth_miracle[CS$<>8__locals3.CS$<>8__locals2.i].PlayAuth(false);
							this.gachaAuthFirstList[CS$<>8__locals3.CS$<>8__locals2.i].gameObject.SetActive(false);
							this.gachaAuthStage.gameObject.SetActive(false);
							this.miracleStage.gameObject.SetActive(true);
							while (!this.gachaAuth_miracle[CS$<>8__locals3.CS$<>8__locals2.i].IsFinished() && !this.gachaAllSkipFlag)
							{
								yield return null;
							}
							this.gachaAuth_miracle[CS$<>8__locals3.CS$<>8__locals2.i].StopSound();
							this.gachaAuth_miracle[CS$<>8__locals3.CS$<>8__locals2.i].PlayFinishProcess();
							this.gachaAuth_miracle[CS$<>8__locals3.CS$<>8__locals2.i].DestroyProcessing();
							SoundManager.ReturnOrgVolume();
						}
						else
						{
							GachaAuthCtrl.cbDispLoading cbDispLoading3;
							if ((cbDispLoading3 = CS$<>8__locals3.CS$<>8__locals2.<>9__8) == null)
							{
								cbDispLoading3 = (CS$<>8__locals3.CS$<>8__locals2.<>9__8 = () => CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.gachaAuthSecondList[CS$<>8__locals3.CS$<>8__locals2.i].gameObject.activeSelf && CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.gachaAuthSecondList[CS$<>8__locals3.CS$<>8__locals2.i].IsFinishInitialize());
							}
							dispLoading = this.DispLoading(cbDispLoading3);
							while (dispLoading.MoveNext())
							{
								yield return null;
							}
						}
						this.miracleStage.gameObject.SetActive(false);
						this.gachaAuthStage.gameObject.SetActive(true);
						this.gachaAuthSecondList[CS$<>8__locals3.CS$<>8__locals2.i].PlayAuth(false);
						this.gachaAuthFirstList[CS$<>8__locals3.CS$<>8__locals2.i].gameObject.SetActive(false);
						this.gachaAuth_miracle[CS$<>8__locals3.CS$<>8__locals2.i].gameObject.SetActive(false);
						this.touchByGachaAuth = false;
						dt = TimeManager.SystemNow;
						while ((TimeManager.SystemNow - dt).TotalSeconds < 1.0 && !this.gachaAllSkipFlag && !this.touchByGachaAuth)
						{
							yield return null;
						}
						if (this.gachaAllSkipFlag)
						{
							goto IL_17AC;
						}
					}
					CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.playAe.transform.parent.gameObject.SetActive(true);
					CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.playAe.ResumeAnimation();
					SoundManager.Play(playSe, false, false);
					if (haveNickName)
					{
						this.guiDataGachaAuth.ResultFriends_WName.baseObj.SetActive(true);
						PguiAECtrl aeCtrl = this.guiDataGachaAuth.ResultFriends_WName.aeCtrl;
						PguiAECtrl.AmimeType amimeType2 = PguiAECtrl.AmimeType.START;
						PguiAECtrl.FinishCallback finishCallback2;
						if ((finishCallback2 = CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>9__9) == null)
						{
							finishCallback2 = (CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>9__9 = delegate
							{
								CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.guiDataGachaAuth.ResultFriends_WName.aeCtrl.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
							});
						}
						aeCtrl.PlayAnimation(amimeType2, finishCallback2);
					}
					if (null != CS$<>8__locals3.newAe)
					{
						CS$<>8__locals3.newAe.gameObject.SetActive(true);
						CS$<>8__locals3.newAe.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
						{
							CS$<>8__locals3.newAe.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
						});
					}
					if (bonusChara)
					{
						this.guiDataGachaAuth.AEImage_EventEffect.gameObject.SetActive(true);
						PguiAECtrl aeimage_EventEffect = this.guiDataGachaAuth.AEImage_EventEffect;
						PguiAECtrl.AmimeType amimeType3 = PguiAECtrl.AmimeType.START;
						PguiAECtrl.FinishCallback finishCallback3;
						if ((finishCallback3 = CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>9__11) == null)
						{
							finishCallback3 = (CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>9__11 = delegate
							{
								CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.guiDataGachaAuth.AEImage_EventEffect.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
							});
						}
						aeimage_EventEffect.PlayAnimation(amimeType3, finishCallback3);
					}
					if (dropBonusChara)
					{
						this.guiDataGachaAuth.AEImage_BonusCharaEffect.gameObject.SetActive(true);
						PguiAECtrl aeimage_BonusCharaEffect = this.guiDataGachaAuth.AEImage_BonusCharaEffect;
						PguiAECtrl.AmimeType amimeType4 = PguiAECtrl.AmimeType.START;
						PguiAECtrl.FinishCallback finishCallback4;
						if ((finishCallback4 = CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>9__12) == null)
						{
							finishCallback4 = (CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>9__12 = delegate
							{
								CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.guiDataGachaAuth.AEImage_BonusCharaEffect.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
							});
						}
						aeimage_BonusCharaEffect.PlayAnimation(amimeType4, finishCallback4);
					}
					if (bonusPhoto)
					{
						this.guiDataGachaAuth.AEImage_PhotoEffect.gameObject.SetActive(true);
						PguiAECtrl aeimage_PhotoEffect = this.guiDataGachaAuth.AEImage_PhotoEffect;
						PguiAECtrl.AmimeType amimeType5 = PguiAECtrl.AmimeType.START;
						PguiAECtrl.FinishCallback finishCallback5;
						if ((finishCallback5 = CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>9__13) == null)
						{
							finishCallback5 = (CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>9__13 = delegate
							{
								CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.guiDataGachaAuth.AEImage_PhotoEffect.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
							});
						}
						aeimage_PhotoEffect.PlayAnimation(amimeType5, finishCallback5);
					}
					if (getItem != null)
					{
						GachaAuthCtrl.<>c__DisplayClass33_3 CS$<>8__locals4 = new GachaAuthCtrl.<>c__DisplayClass33_3();
						CS$<>8__locals4.CS$<>8__locals3 = CS$<>8__locals3;
						this.guiDataGachaAuth.gachaAeItemGet.baseObj.SetActive(true);
						GachaAuthCtrl.<>c__DisplayClass33_3 CS$<>8__locals5 = CS$<>8__locals4;
						Transform transform = this.guiDataGachaAuth.gachaAeItemGet.Icon_Item.transform.Find("Icon_Item(Clone)");
						CS$<>8__locals5.itemInstance = ((transform != null) ? transform.gameObject : null);
						if (null == CS$<>8__locals4.itemInstance)
						{
							CS$<>8__locals4.itemInstance = Object.Instantiate<GameObject>((GameObject)Resources.Load("CmnIconFrame/GUI/Prefab/Icon_Item"), this.guiDataGachaAuth.gachaAeItemGet.Icon_Item.transform);
						}
						IconItemCtrl component = CS$<>8__locals4.itemInstance.GetComponent<IconItemCtrl>();
						component.Setup(getItem);
						component.DispNew(getItemIsNew);
						CS$<>8__locals4.itemInstance.SetActive(false);
						this.guiDataGachaAuth.gachaAeItemGet.aeCtrl.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
						{
							CS$<>8__locals4.itemInstance.SetActive(true);
							CS$<>8__locals4.CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.guiDataGachaAuth.gachaAeItemGet.aeCtrl.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
						});
					}
					if (getItemEx != null)
					{
						GachaAuthCtrl.<>c__DisplayClass33_4 CS$<>8__locals6 = new GachaAuthCtrl.<>c__DisplayClass33_4();
						CS$<>8__locals6.CS$<>8__locals4 = CS$<>8__locals3;
						this.guiDataGachaAuth.gachaAeItemBonus.baseObj.SetActive(true);
						GachaAuthCtrl.<>c__DisplayClass33_4 CS$<>8__locals7 = CS$<>8__locals6;
						Transform transform2 = this.guiDataGachaAuth.gachaAeItemBonus.Icon_Item.transform.Find("Icon_Item(Clone)");
						CS$<>8__locals7.itemInstance = ((transform2 != null) ? transform2.gameObject : null);
						if (null == CS$<>8__locals6.itemInstance)
						{
							CS$<>8__locals6.itemInstance = Object.Instantiate<GameObject>((GameObject)Resources.Load("CmnIconFrame/GUI/Prefab/Icon_Item"), this.guiDataGachaAuth.gachaAeItemBonus.Icon_Item.transform);
						}
						IconItemCtrl component2 = CS$<>8__locals6.itemInstance.GetComponent<IconItemCtrl>();
						component2.Setup(getItemEx);
						component2.DispNew(getItemExIsNew);
						CS$<>8__locals6.itemInstance.SetActive(false);
						this.guiDataGachaAuth.gachaAeItemBonus.aeCtrl.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
						{
							CS$<>8__locals6.itemInstance.SetActive(true);
							CS$<>8__locals6.CS$<>8__locals4.CS$<>8__locals2.CS$<>8__locals1.<>4__this.guiDataGachaAuth.gachaAeItemBonus.aeCtrl.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
						});
					}
					this.touchByGachaAuth = false;
					while (!this.touchByGachaAuth)
					{
						this.AuthEndFlg = true;
						if (this.gachaAllSkipFlag)
						{
							break;
						}
						yield return null;
					}
					this.AuthEndFlg = false;
					PguiAECtrl playAe2 = CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.playAe;
					PguiAECtrl.AmimeType amimeType6 = PguiAECtrl.AmimeType.END;
					PguiAECtrl.FinishCallback finishCallback6;
					if ((finishCallback6 = CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>9__14) == null)
					{
						finishCallback6 = (CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>9__14 = delegate
						{
							CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.playAe.transform.parent.gameObject.SetActive(false);
						});
					}
					playAe2.PlayAnimation(amimeType6, finishCallback6);
				}
			}
			IL_17AC:
			if (haveNickName && this.guiDataGachaAuth.ResultFriends_WName.baseObj.activeSelf)
			{
				PguiAECtrl aeCtrl2 = this.guiDataGachaAuth.ResultFriends_WName.aeCtrl;
				PguiAECtrl.AmimeType amimeType7 = PguiAECtrl.AmimeType.END;
				PguiAECtrl.FinishCallback finishCallback7;
				if ((finishCallback7 = CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>9__15) == null)
				{
					finishCallback7 = (CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>9__15 = delegate
					{
						CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.guiDataGachaAuth.ResultFriends_WName.baseObj.SetActive(false);
					});
				}
				aeCtrl2.PlayAnimation(amimeType7, finishCallback7);
			}
			if (null != CS$<>8__locals3.newAe && CS$<>8__locals3.newAe.gameObject.activeSelf)
			{
				CS$<>8__locals3.newAe.PlayAnimation(PguiAECtrl.AmimeType.END, delegate
				{
					CS$<>8__locals3.newAe.gameObject.SetActive(false);
				});
			}
			if (bonusChara && this.guiDataGachaAuth.AEImage_EventEffect.gameObject.activeSelf)
			{
				PguiAECtrl aeimage_EventEffect2 = this.guiDataGachaAuth.AEImage_EventEffect;
				PguiAECtrl.AmimeType amimeType8 = PguiAECtrl.AmimeType.END;
				PguiAECtrl.FinishCallback finishCallback8;
				if ((finishCallback8 = CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>9__17) == null)
				{
					finishCallback8 = (CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>9__17 = delegate
					{
						CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.guiDataGachaAuth.AEImage_EventEffect.gameObject.SetActive(false);
					});
				}
				aeimage_EventEffect2.PlayAnimation(amimeType8, finishCallback8);
			}
			if (dropBonusChara && this.guiDataGachaAuth.AEImage_BonusCharaEffect.gameObject.activeSelf)
			{
				PguiAECtrl aeimage_BonusCharaEffect2 = this.guiDataGachaAuth.AEImage_BonusCharaEffect;
				PguiAECtrl.AmimeType amimeType9 = PguiAECtrl.AmimeType.END;
				PguiAECtrl.FinishCallback finishCallback9;
				if ((finishCallback9 = CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>9__18) == null)
				{
					finishCallback9 = (CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>9__18 = delegate
					{
						CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.guiDataGachaAuth.AEImage_BonusCharaEffect.gameObject.SetActive(false);
					});
				}
				aeimage_BonusCharaEffect2.PlayAnimation(amimeType9, finishCallback9);
			}
			if (bonusPhoto && this.guiDataGachaAuth.AEImage_PhotoEffect.gameObject.activeSelf)
			{
				PguiAECtrl aeimage_PhotoEffect2 = this.guiDataGachaAuth.AEImage_PhotoEffect;
				PguiAECtrl.AmimeType amimeType10 = PguiAECtrl.AmimeType.END;
				PguiAECtrl.FinishCallback finishCallback10;
				if ((finishCallback10 = CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>9__19) == null)
				{
					finishCallback10 = (CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>9__19 = delegate
					{
						CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.guiDataGachaAuth.AEImage_PhotoEffect.gameObject.SetActive(false);
					});
				}
				aeimage_PhotoEffect2.PlayAnimation(amimeType10, finishCallback10);
			}
			if (getItem != null && this.guiDataGachaAuth.gachaAeItemGet.baseObj.activeSelf)
			{
				PguiAECtrl aeCtrl3 = this.guiDataGachaAuth.gachaAeItemGet.aeCtrl;
				PguiAECtrl.AmimeType amimeType11 = PguiAECtrl.AmimeType.END;
				PguiAECtrl.FinishCallback finishCallback11;
				if ((finishCallback11 = CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>9__20) == null)
				{
					finishCallback11 = (CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>9__20 = delegate
					{
						CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.guiDataGachaAuth.gachaAeItemGet.baseObj.SetActive(false);
					});
				}
				aeCtrl3.PlayAnimation(amimeType11, finishCallback11);
			}
			if (getItemEx != null && this.guiDataGachaAuth.gachaAeItemBonus.baseObj.activeSelf)
			{
				PguiAECtrl aeCtrl4 = this.guiDataGachaAuth.gachaAeItemBonus.aeCtrl;
				PguiAECtrl.AmimeType amimeType12 = PguiAECtrl.AmimeType.END;
				PguiAECtrl.FinishCallback finishCallback12;
				if ((finishCallback12 = CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>9__21) == null)
				{
					finishCallback12 = (CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>9__21 = delegate
					{
						CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.guiDataGachaAuth.gachaAeItemBonus.baseObj.SetActive(false);
					});
				}
				aeCtrl4.PlayAnimation(amimeType12, finishCallback12);
			}
			while (CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.playAe.transform.parent.gameObject.activeSelf && !this.gachaAllSkipFlag)
			{
				yield return null;
			}
			CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.playAe.transform.parent.gameObject.SetActive(false);
			if (!this.gachaAllSkipFlag)
			{
				CS$<>8__locals3 = null;
				getItem = null;
				getItemEx = null;
				playSe = null;
				goto IL_1B9C;
			}
			break;
		}
		CS$<>8__locals2 = null;
		this.playAuthIdx = -1 - this.playAuthIdx;
		IEnumerator ienum = this.PlayGreeting(authItemList, tutorialFlg, true, false, null, false);
		while (ienum.MoveNext())
		{
			yield return null;
		}
		ienum = null;
		dispLoading = this.DispLoading(() => CS$<>8__locals1.<>4__this.loadData == null);
		while (dispLoading.MoveNext())
		{
			yield return null;
		}
		if (this.gachaAeGreeting != null)
		{
			this.gachaAeGreeting.baseObj.SetActive(false);
		}
		for (int j = 0; j < authItemList.Count; j++)
		{
			if (this.gachaAuthFirstList[j] != null)
			{
				this.gachaAuthFirstList[j].PlayFinishProcess();
				this.gachaAuthFirstList[j].DestroyProcessing();
				this.gachaAuthFirstList[j].gameObject.SetActive(false);
			}
			if (this.gachaAuthSecondList[j] != null)
			{
				this.gachaAuthSecondList[j].PlayFinishProcess();
				this.gachaAuthSecondList[j].DestroyProcessing();
				this.gachaAuthSecondList[j].gameObject.SetActive(false);
			}
			if (this.gachaAuth_miracle[j] != null)
			{
				this.gachaAuth_miracle[j].PlayFinishProcess();
				this.gachaAuth_miracle[j].DestroyProcessing();
				this.gachaAuth_miracle[j].gameObject.SetActive(false);
			}
		}
		if (this.gachaAuth_common != null)
		{
			this.gachaAuth_common.PlayFinishProcess();
			this.gachaAuth_common.DestroyProcessing();
			this.gachaAuth_common.gameObject.SetActive(false);
		}
		this.guiDataGachaAuth.skipButton.gameObject.SetActive(false);
		Object.Destroy(this.gachaAuthStage.gameObject);
		AssetManager.UnloadAssetData(CS$<>8__locals1.stageGacha, AssetManager.OWNER.AuthGacha);
		this.gachaAuthStage = null;
		Object.Destroy(this.miracleStage.gameObject);
		AssetManager.UnloadAssetData(CS$<>8__locals1.stageSavnnahrNoon, AssetManager.OWNER.AuthGacha);
		this.miracleStage = null;
		this.AEImage_Wipe_End.PlayAnimation(PguiAECtrl.AmimeType.END, delegate
		{
			CS$<>8__locals1.<>4__this.gachaWipe_End.gameObject.SetActive(false);
		});
		this.guiDataGachaAuth.baseObj.SetActive(false);
		yield break;
	}

	// Token: 0x06001197 RID: 4503 RVA: 0x000D50E8 File Offset: 0x000D32E8
	private IEnumerator PlayGreeting(List<GachaAuthCtrl.AuthItem> authItemList, bool tutorialFlg, bool isDispLoading, bool clothChange, GameObject bgObj = null, bool isHomeAuth = false)
	{
		GachaAuthCtrl.<>c__DisplayClass34_0 CS$<>8__locals1 = new GachaAuthCtrl.<>c__DisplayClass34_0();
		CS$<>8__locals1.<>4__this = this;
		List<int> greetingList = new List<int>();
		foreach (GachaAuthCtrl.AuthItem authItem in authItemList)
		{
			if (ItemDef.Id2Kind(authItem.itemId) == ItemDef.Kind.CHARA && (authItem.isNew || tutorialFlg))
			{
				greetingList.Add(authItem.itemId);
			}
		}
		CS$<>8__locals1.isPlay = false;
		if (greetingList.Count > 0)
		{
			PguiAECtrl.AmimeType amimeType = (this.gachaAllSkipFlag ? PguiAECtrl.AmimeType.START_SUB : PguiAECtrl.AmimeType.START);
			this.gachaAllSkipFlag = false;
			CS$<>8__locals1.isPlay = true;
			this.gachaWipe_Top.gameObject.SetActive(true);
			this.AEImage_Wipe_Top.PlayAnimation(amimeType, delegate
			{
				CS$<>8__locals1.isPlay = false;
			});
			while (CS$<>8__locals1.isPlay)
			{
				yield return null;
			}
			if (isDispLoading)
			{
				IEnumerator dispLoading = this.DispLoading(() => CS$<>8__locals1.<>4__this.loadData == null);
				while (dispLoading.MoveNext())
				{
					yield return null;
				}
				dispLoading = null;
			}
			this.gachaAuth_common.gameObject.SetActive(false);
			for (int i = 0; i < authItemList.Count; i++)
			{
				this.gachaAuthFirstList[i].gameObject.SetActive(false);
				this.gachaAuthSecondList[i].gameObject.SetActive(false);
			}
			CanvasManager.SetBgEnable(true);
			if (bgObj != null)
			{
				bgObj.SetActive(true);
			}
			this.AEImage_Wipe_Top.PlayAnimation(PguiAECtrl.AmimeType.END, delegate
			{
				CS$<>8__locals1.<>4__this.gachaWipe_Top.gameObject.SetActive(false);
			});
			this.gachaWipe_Change.gameObject.SetActive(false);
		}
		this.guiDataGachaAuth.ResultFriends_WName.baseObj.SetActive(false);
		this.guiDataGachaAuth.AEImage_New_Chara.gameObject.SetActive(false);
		this.guiDataGachaAuth.AEImage_New_Photo.gameObject.SetActive(false);
		this.guiDataGachaAuth.AEImage_New_Interior.gameObject.SetActive(false);
		this.guiDataGachaAuth.AEImage_EventEffect.gameObject.SetActive(false);
		this.guiDataGachaAuth.AEImage_PhotoEffect.gameObject.SetActive(false);
		this.guiDataGachaAuth.AEImage_BonusCharaEffect.gameObject.SetActive(false);
		this.guiDataGachaAuth.gachaAeItemGet.baseObj.SetActive(false);
		this.guiDataGachaAuth.gachaAeItemBonus.baseObj.SetActive(false);
		int cnt = 0;
		foreach (int num in greetingList)
		{
			int num2 = cnt;
			cnt = num2 + 1;
			this.gachaAeGreeting.baseObj.SetActive(true);
			CharaStaticData csd = DataManager.DmChara.GetCharaStaticData(num);
			this.gachaAeGreeting.Txt_Serif.text = csd.baseData.greetingText;
			this.gachaAeGreeting.Txt_CharaName.text = csd.GetName();
			bool flag = false;
			if (!string.IsNullOrEmpty(csd.baseData.NickName))
			{
				this.gachaAeGreeting.Txt_WName.text = csd.baseData.NickName;
				flag = true;
			}
			this.gachaAeGreeting.Txt_WName.gameObject.SetActive(flag);
			this.gachaAeGreeting.Txt_CharaName_Eng.text = csd.baseData.charaNameEng;
			this.gachaAeGreeting.Txt_CharaKind.text = csd.baseData.eponymName;
			for (int j = 0; j < this.gachaAeGreeting.Icon_Star.Count; j++)
			{
				PguiAECtrl star = this.gachaAeGreeting.Icon_Star[j];
				star.gameObject.SetActive(j < csd.baseData.rankLow);
				star.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
				{
					star.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
				});
			}
			int attribute = (int)csd.baseData.attribute;
			this.gachaAeGreeting.Bg_Pattern.color = this.gachaAeGreeting.Bg_Color.GetGameObjectById(attribute.ToString());
			this.gachaAeGreeting.Top_Mat.color = this.gachaAeGreeting.Top_Mat_Color.GetGameObjectById(attribute.ToString());
			this.gachaAeGreeting.Bot_Mat.color = this.gachaAeGreeting.Bot_Mat_Color.GetGameObjectById(attribute.ToString());
			this.gachaAeGreeting.Top_Pt.Replace(attribute);
			this.gachaAeGreeting.Bot_Pt.Replace(attribute);
			this.gachaAeGreeting.Img_No.color = this.gachaAeGreeting.Img_No_Color.GetGameObjectById(attribute.ToString());
			this.gachaAeGreeting.AEImage_NewComer.Replace(attribute.ToString());
			SoundManager.Play("prd_se_gacha_new_arrival", false, false);
			PguiAECtrl aeCtrl = this.gachaAeGreeting.aeCtrl;
			PguiAECtrl.AmimeType amimeType2 = PguiAECtrl.AmimeType.START;
			PguiAECtrl.FinishCallback finishCallback;
			if ((finishCallback = CS$<>8__locals1.<>9__4) == null)
			{
				finishCallback = (CS$<>8__locals1.<>9__4 = delegate
				{
					CS$<>8__locals1.<>4__this.gachaAeGreeting.aeCtrl.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
				});
			}
			aeCtrl.PlayAnimation(amimeType2, finishCallback);
			int num3 = 0;
			if (clothChange)
			{
				CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(num);
				if (userCharaData != null)
				{
					num3 = userCharaData.equipClothImageId;
				}
			}
			this.gachaAeGreeting.RenderChara.Setup(csd.GetId(), 2, CharaMotionDefine.ActKey.GACHA_LP, num3, false, true, null, false, null, 0f, null, false, false, false);
			SoundManager.LoadCueSheet(csd.cueSheetName);
			if (isDispLoading)
			{
				GachaAuthCtrl.cbDispLoading cbDispLoading;
				if ((cbDispLoading = CS$<>8__locals1.<>9__6) == null)
				{
					cbDispLoading = (CS$<>8__locals1.<>9__6 = () => CS$<>8__locals1.<>4__this.gachaAeGreeting.RenderChara.FinishedSetup);
				}
				IEnumerator dispLoading = this.DispLoading(cbDispLoading);
				while (dispLoading.MoveNext())
				{
					yield return null;
				}
				dispLoading = null;
			}
			while (this.gachaWipe_Top.gameObject.activeSelf || this.gachaWipe_Change.gameObject.activeSelf)
			{
				yield return null;
			}
			DataManager.DmGacha.LatestGreetingVoice.Stop();
			DataManager.DmGacha.LatestGreetingVoice = SoundManager.PlayVoice(csd.cueSheetName, VOICE_TYPE.GCH01);
			this.touchByGachaAuth = false;
			while (!this.touchByGachaAuth)
			{
				if (Input.GetKeyDown(KeyCode.Escape))
				{
					this.touchByGachaAuth = true;
				}
				if ((isHomeAuth && DataManager.DmGacha.LatestGreetingVoice.GetStatus() == CriAtomExPlayback.Status.Removed) || this.gachaAllSkipFlag)
				{
					break;
				}
				yield return null;
			}
			if (isHomeAuth)
			{
				DataManager.DmGacha.LatestGreetingVoice.Stop();
			}
			foreach (PguiAECtrl pguiAECtrl in this.gachaAeGreeting.Icon_Star)
			{
				pguiAECtrl.PlayAnimation(PguiAECtrl.AmimeType.END, null);
			}
			if (cnt < greetingList.Count && !this.gachaAllSkipFlag)
			{
				GachaAuthCtrl.<>c__DisplayClass34_2 CS$<>8__locals3 = new GachaAuthCtrl.<>c__DisplayClass34_2();
				CS$<>8__locals3.CS$<>8__locals1 = CS$<>8__locals1;
				CS$<>8__locals3.isPlaying = true;
				this.gachaAeGreeting.aeCtrl.PlayAnimation(PguiAECtrl.AmimeType.END, delegate
				{
					CS$<>8__locals3.CS$<>8__locals1.<>4__this.gachaWipe_Change.gameObject.SetActive(true);
					PguiAECtrl aeimage_Wipe_Change = CS$<>8__locals3.CS$<>8__locals1.<>4__this.AEImage_Wipe_Change;
					PguiAECtrl.AmimeType amimeType3 = PguiAECtrl.AmimeType.START;
					PguiAECtrl.FinishCallback finishCallback2;
					if ((finishCallback2 = CS$<>8__locals3.<>9__8) == null)
					{
						finishCallback2 = (CS$<>8__locals3.<>9__8 = delegate
						{
							CS$<>8__locals3.isPlaying = false;
							PguiAECtrl aeimage_Wipe_Change2 = CS$<>8__locals3.CS$<>8__locals1.<>4__this.AEImage_Wipe_Change;
							PguiAECtrl.AmimeType amimeType4 = PguiAECtrl.AmimeType.END;
							PguiAECtrl.FinishCallback finishCallback3;
							if ((finishCallback3 = CS$<>8__locals3.CS$<>8__locals1.<>9__9) == null)
							{
								finishCallback3 = (CS$<>8__locals3.CS$<>8__locals1.<>9__9 = delegate
								{
									CS$<>8__locals3.CS$<>8__locals1.<>4__this.gachaWipe_Change.gameObject.SetActive(false);
								});
							}
							aeimage_Wipe_Change2.PlayAnimation(amimeType4, finishCallback3);
						});
					}
					aeimage_Wipe_Change.PlayAnimation(amimeType3, finishCallback2);
				});
				while (CS$<>8__locals3.isPlaying && !this.gachaAllSkipFlag)
				{
					yield return null;
				}
				CS$<>8__locals3 = null;
			}
			if (this.gachaAllSkipFlag)
			{
				break;
			}
			csd = null;
		}
		List<int>.Enumerator enumerator2 = default(List<int>.Enumerator);
		if (!isHomeAuth)
		{
			CS$<>8__locals1.isPlay = true;
			this.gachaWipe_End.gameObject.SetActive(true);
			SoundManager.Play("prd_se_gacha_to_result", false, false);
			this.AEImage_Wipe_End.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
			{
				CS$<>8__locals1.isPlay = false;
			});
			while (CS$<>8__locals1.isPlay)
			{
				yield return null;
			}
		}
		using (List<int>.Enumerator enumerator4 = greetingList.GetEnumerator())
		{
			while (enumerator4.MoveNext())
			{
				int num4 = enumerator4.Current;
				SoundManager.UnloadCueSheet(DataManager.DmChara.GetCharaStaticData(num4).cueSheetName);
			}
			yield break;
		}
		yield break;
		yield break;
	}

	// Token: 0x06001198 RID: 4504 RVA: 0x000D5124 File Offset: 0x000D3324
	private IEnumerator LoadAuth(List<GachaAuthCtrl.AuthItem> authItem, List<AuthPlayer.GachaParam.After> afterParam)
	{
		foreach (AuthPlayer.GachaParam.After after in afterParam)
		{
			after.stageData = this.gachaAuthStage;
		}
		foreach (AuthPlayer authPlayer in this.gachaAuthFirstList)
		{
			authPlayer.gameObject.SetActive(false);
		}
		foreach (AuthPlayer authPlayer2 in this.gachaAuthSecondList)
		{
			authPlayer2.gameObject.SetActive(false);
		}
		foreach (AuthPlayer authPlayer3 in this.gachaAuth_miracle)
		{
			authPlayer3.gameObject.SetActive(false);
		}
		yield return null;
		int i = 0;
		IL_03E2:
		while (i < authItem.Count)
		{
			while (this.playAuthIdx >= 0)
			{
				if (i < this.playAuthIdx + 3)
				{
					ItemDef.Kind kind = ItemDef.Id2Kind(authItem[i].itemId);
					if (kind == ItemDef.Kind.CHARA || kind == ItemDef.Kind.PHOTO || kind == ItemDef.Kind.TREEHOUSE_FURNITURE)
					{
						this.gachaAuthFirstList[i].gameObject.SetActive(true);
						this.gachaAuthFirstList[i].InitializeByGacha(afterParam[i]);
						while (this.gachaAuthFirstList[i].gameObject.activeSelf && !this.gachaAuthFirstList[i].IsFinishInitialize())
						{
							yield return null;
						}
						if (this.playAuthIdx < 0)
						{
							yield break;
						}
						if (kind == ItemDef.Kind.CHARA)
						{
							this.gachaAuthSecondList[i].gameObject.SetActive(true);
							this.gachaAuthSecondList[i].InitializeByGacha2(afterParam[i]);
							while (this.gachaAuthSecondList[i].gameObject.activeSelf && !this.gachaAuthSecondList[i].IsFinishInitialize())
							{
								yield return null;
							}
							if (this.playAuthIdx < 0)
							{
								yield break;
							}
							if (authItem[i].isNew)
							{
								this.gachaAuth_miracle[i].gameObject.SetActive(true);
								this.gachaAuth_miracle[i].Initialize(DataManager.DmChara.GetCharaStaticData(authItem[i].itemId).artsData.authParam.authName, null, this.miracleStage);
								while (this.gachaAuth_miracle[i].gameObject.activeSelf && !this.gachaAuth_miracle[i].IsFinishInitialize())
								{
									yield return null;
								}
								if (this.playAuthIdx < 0)
								{
									yield break;
								}
							}
						}
					}
					int num = i;
					i = num + 1;
					goto IL_03E2;
				}
				yield return null;
			}
			yield break;
		}
		yield break;
	}

	// Token: 0x06001199 RID: 4505 RVA: 0x000D5144 File Offset: 0x000D3344
	private AuthPlayer.GachaParam.Before GetBeforeGachaParamByPresentBox(List<GachaAuthCtrl.AuthItem> authItemList)
	{
		ItemDef.Rarity rarity = DataManager.DmItem.GetItemStaticBase(authItemList[0].itemId).GetRarity();
		AuthPlayer.GachaParam.Before before = new AuthPlayer.GachaParam.Before();
		before.postActType = AuthPlayer.GachaParam.PostActType.NORMAL;
		before.putType = AuthPlayer.GachaParam.PutType.NORMAL;
		before.skyType = AuthPlayer.GachaParam.SkyType.NORMAL;
		before.effectType = AuthPlayer.GachaParam.EffectType.BLUE;
		switch (rarity)
		{
		case ItemDef.Rarity.STAR1:
		case ItemDef.Rarity.STAR2:
			before.effectType = AuthPlayer.GachaParam.EffectType.BLUE;
			break;
		case ItemDef.Rarity.STAR3:
			before.effectType = AuthPlayer.GachaParam.EffectType.GOLD;
			break;
		case ItemDef.Rarity.STAR4:
		case ItemDef.Rarity.STAR5:
			before.effectType = AuthPlayer.GachaParam.EffectType.RAINBOW;
			break;
		}
		return before;
	}

	// Token: 0x0600119A RID: 4506 RVA: 0x000D51C8 File Offset: 0x000D33C8
	private AuthPlayer.GachaParam.Before GetBeforeGachaParam(List<GachaAuthCtrl.AuthItem> authItemList, ResultRarity resultRarity)
	{
		int highRarity = resultRarity.GetHighRarity(authItemList);
		return this.LotteryBeforeGachaParam(highRarity, resultRarity.SKY_PER, resultRarity.POST_PER, resultRarity.PUT_PER, resultRarity.PUT_PER_V, resultRarity.EFF_PER);
	}

	// Token: 0x0600119B RID: 4507 RVA: 0x000D5204 File Offset: 0x000D3404
	private AuthPlayer.GachaParam.Before LotteryBeforeGachaParam(int highRarity, int[,] skyPer, int[,] postPer, int[,] putPer, int[,] putPer_v, int[,] effPer)
	{
		AuthPlayer.GachaParam.Before before = new AuthPlayer.GachaParam.Before();
		Random random = new Random();
		int num = random.Next(100);
		int i = 0;
		while (i < skyPer.GetLength(1))
		{
			int num2 = skyPer[highRarity, i];
			if (num < num2)
			{
				switch (i)
				{
				case 0:
					before.skyType = AuthPlayer.GachaParam.SkyType.NORMAL;
					goto IL_006D;
				case 1:
					before.skyType = AuthPlayer.GachaParam.SkyType.NIGHT;
					goto IL_006D;
				case 2:
					before.skyType = AuthPlayer.GachaParam.SkyType.NIGHT_STAR;
					goto IL_006D;
				default:
					goto IL_006D;
				}
			}
			else
			{
				i++;
			}
		}
		IL_006D:
		num = random.Next(100);
		int j = 0;
		while (j < postPer.GetLength(1))
		{
			int num3 = postPer[highRarity, j];
			if (num < num3)
			{
				if (j == 0)
				{
					before.postActType = AuthPlayer.GachaParam.PostActType.NORMAL;
					break;
				}
				if (j != 1)
				{
					break;
				}
				before.postActType = AuthPlayer.GachaParam.PostActType.JUMP;
				break;
			}
			else
			{
				j++;
			}
		}
		num = random.Next(100);
		int k = 0;
		while (k < putPer.GetLength(1))
		{
			int num4 = putPer[highRarity, k];
			if (num < num4)
			{
				switch (k)
				{
				case 0:
					before.putType = AuthPlayer.GachaParam.PutType.NORMAL;
					goto IL_0204;
				case 1:
					before.putType = AuthPlayer.GachaParam.PutType.LUCKY_BEAST;
					goto IL_0204;
				case 2:
				{
					before.putType = AuthPlayer.GachaParam.PutType.MIRAI;
					if (!DataManagerGacha.IsExistResultPuChara())
					{
						goto IL_0204;
					}
					int num5 = random.Next(65);
					if (num5 < putPer_v[0, 0])
					{
						switch (num5 % 3)
						{
						case 0:
							before.putType = AuthPlayer.GachaParam.PutType.MIRAI_KAKO;
							goto IL_0204;
						case 1:
							before.putType = AuthPlayer.GachaParam.PutType.MIRAI_NANA;
							goto IL_0204;
						case 2:
							before.putType = AuthPlayer.GachaParam.PutType.MIRAI_CARRENDER;
							goto IL_0204;
						default:
							goto IL_0204;
						}
					}
					else if (num5 < putPer_v[0, 1])
					{
						switch ((num5 - putPer_v[0, 0]) % 3)
						{
						case 0:
							before.putType = AuthPlayer.GachaParam.PutType.MIRAI_KAKO_NANA;
							goto IL_0204;
						case 1:
							before.putType = AuthPlayer.GachaParam.PutType.MIRAI_KAKO_CARRENDER;
							goto IL_0204;
						case 2:
							before.putType = AuthPlayer.GachaParam.PutType.MIRAI_NANA_CARRENDER;
							goto IL_0204;
						default:
							goto IL_0204;
						}
					}
					else
					{
						if (num5 < putPer_v[0, 2])
						{
							before.putType = AuthPlayer.GachaParam.PutType.FULL_MEMBERS;
							goto IL_0204;
						}
						if (num5 < putPer_v[0, 3])
						{
							before.putType = AuthPlayer.GachaParam.PutType.MIRAI;
							goto IL_0204;
						}
						goto IL_0204;
					}
					break;
				}
				default:
					goto IL_0204;
				}
			}
			else
			{
				k++;
			}
		}
		IL_0204:
		num = random.Next(100);
		int l = 0;
		while (l < effPer.GetLength(1))
		{
			int num6 = effPer[highRarity, l];
			if (num < num6)
			{
				switch (l)
				{
				case 0:
					before.effectType = AuthPlayer.GachaParam.EffectType.BLUE;
					return before;
				case 1:
					before.effectType = AuthPlayer.GachaParam.EffectType.GOLD;
					return before;
				case 2:
					before.effectType = AuthPlayer.GachaParam.EffectType.RAINBOW;
					return before;
				default:
					return before;
				}
			}
			else
			{
				l++;
			}
		}
		return before;
	}

	// Token: 0x0600119C RID: 4508 RVA: 0x000D5478 File Offset: 0x000D3678
	private string GetPremiumSEName(List<GachaAuthCtrl.AuthItem> authItemList, ResultRarity resultRarity)
	{
		List<string> list = new List<string> { "prd_se_gacha_premium_1", "prd_se_gacha_premium_2", "prd_se_gacha_premium_3" };
		return resultRarity.LotteryPremiumSEName(authItemList, list);
	}

	// Token: 0x0600119D RID: 4509 RVA: 0x000D54B4 File Offset: 0x000D36B4
	private List<AuthPlayer.GachaParam.After> GetAfterGachaParam(List<GachaAuthCtrl.AuthItem> authItemList, AuthPlayer.GachaParam.Before beforeGachaParam, ResultRarity resultRarity)
	{
		List<AuthPlayer.GachaParam.After> list = new List<AuthPlayer.GachaParam.After>();
		bool flag = AuthPlayer.GachaParam.SkyType.NIGHT == beforeGachaParam.skyType || AuthPlayer.GachaParam.SkyType.NIGHT_STAR == beforeGachaParam.skyType || AuthPlayer.GachaParam.PostActType.JUMP == beforeGachaParam.postActType || AuthPlayer.GachaParam.PutType.MIRAI == beforeGachaParam.putType || AuthPlayer.GachaParam.EffectType.RAINBOW == beforeGachaParam.effectType;
		foreach (GachaAuthCtrl.AuthItem authItem in authItemList)
		{
			DataManager.DmItem.GetItemStaticBase(authItem.itemId);
			AuthPlayer.GachaParam.After after = new AuthPlayer.GachaParam.After();
			after.itemId = authItem.itemId;
			after.putType = beforeGachaParam.putType;
			after.effectType = resultRarity.GetEffectTypeForGachaParamAfter(authItem);
			after.stageData = this.gachaAuthStage;
			if (!flag)
			{
				after.isPromotion = resultRarity.LotteryPromotion(authItem);
				if (after.isPromotion)
				{
					after.effectType = beforeGachaParam.effectType;
				}
			}
			list.Add(after);
		}
		return list;
	}

	// Token: 0x0600119E RID: 4510 RVA: 0x000D55B8 File Offset: 0x000D37B8
	private void OnClickButton(PguiButtonCtrl button)
	{
		if (this.guiDataGachaAuth.skipButton != null && button == this.guiDataGachaAuth.skipButton)
		{
			this.gachaAllSkipFlag = true;
		}
	}

	// Token: 0x0600119F RID: 4511 RVA: 0x000D55E8 File Offset: 0x000D37E8
	public void DestroyAllObject()
	{
		this.IsDestroy = true;
		if (null != this.gachaAuthStage)
		{
			Object.Destroy(this.gachaAuthStage.gameObject);
			this.gachaAuthStage = null;
		}
		if (null != this.gachaAuth_common)
		{
			Object.Destroy(this.gachaAuth_common.gameObject);
			this.gachaAuth_common = null;
		}
		if (this.gachaAuth_miracle != null)
		{
			for (int i = 0; i < this.gachaAuth_miracle.Count; i++)
			{
				if (null != this.gachaAuth_miracle[i])
				{
					Object.Destroy(this.gachaAuth_miracle[i].gameObject);
					this.gachaAuth_miracle[i] = null;
				}
			}
			this.gachaAuth_miracle = null;
		}
		if (this.gachaAuthFirstList != null)
		{
			for (int j = 0; j < this.gachaAuthFirstList.Count; j++)
			{
				if (null != this.gachaAuthFirstList[j])
				{
					Object.Destroy(this.gachaAuthFirstList[j].gameObject);
					this.gachaAuthFirstList[j] = null;
				}
			}
			this.gachaAuthFirstList = null;
		}
		if (this.gachaAuthSecondList != null)
		{
			for (int k = 0; k < this.gachaAuthSecondList.Count; k++)
			{
				if (null != this.gachaAuthSecondList[k])
				{
					Object.Destroy(this.gachaAuthSecondList[k].gameObject);
					this.gachaAuthSecondList[k] = null;
				}
			}
			this.gachaAuthSecondList = null;
		}
		if (null != this.gachaAuthStage)
		{
			Object.Destroy(this.gachaAuthStage.gameObject);
			this.gachaAuthStage.Destory();
		}
		if (null != this.miracleStage)
		{
			Object.Destroy(this.miracleStage.gameObject);
			this.miracleStage.Destory();
		}
		if (null != this.MainFieldGachaAuth)
		{
			Object.Destroy(this.MainFieldGachaAuth);
		}
		if (null != this.guiDataGachaAuth.baseObj)
		{
			Object.Destroy(this.guiDataGachaAuth.baseObj);
			this.guiDataGachaAuth.baseObj = null;
		}
		if (this.gachaAeGreeting != null)
		{
			if (this.gachaAeGreeting.RenderChara != null)
			{
				Object.Destroy(this.gachaAeGreeting.RenderChara.gameObject);
			}
			this.gachaAeGreeting.RenderChara = null;
			this.gachaAeGreeting = null;
		}
		if (this.gachaWipe_Top != null)
		{
			Object.Destroy(this.gachaWipe_Top);
			this.gachaWipe_Top = null;
		}
		if (this.gachaWipe_Change != null)
		{
			Object.Destroy(this.gachaWipe_Change);
			this.gachaWipe_Change = null;
		}
		if (this.gachaWipe_End != null)
		{
			Object.Destroy(this.gachaWipe_End);
			this.gachaWipe_End = null;
		}
		DataManagerGacha.ReleasePuCharaList();
	}

	// Token: 0x04000ED0 RID: 3792
	private GachaAuthCtrl.GUIGachaAuth guiDataGachaAuth;

	// Token: 0x04000ED1 RID: 3793
	private AuthPlayer gachaAuth_common;

	// Token: 0x04000ED2 RID: 3794
	private List<AuthPlayer> gachaAuthFirstList;

	// Token: 0x04000ED3 RID: 3795
	private List<AuthPlayer> gachaAuthSecondList;

	// Token: 0x04000ED4 RID: 3796
	private List<AuthPlayer> gachaAuth_miracle;

	// Token: 0x04000ED5 RID: 3797
	private GachaAuthCtrl.GachaAeGreeting gachaAeGreeting;

	// Token: 0x04000ED6 RID: 3798
	private GameObject gachaWipe_Top;

	// Token: 0x04000ED7 RID: 3799
	private GameObject gachaWipe_Change;

	// Token: 0x04000ED8 RID: 3800
	private GameObject gachaWipe_End;

	// Token: 0x04000ED9 RID: 3801
	private PguiAECtrl AEImage_Wipe_Top;

	// Token: 0x04000EDA RID: 3802
	private PguiAECtrl AEImage_Wipe_Change;

	// Token: 0x04000EDB RID: 3803
	private PguiAECtrl AEImage_Wipe_End;

	// Token: 0x04000EDC RID: 3804
	private StagePresetCtrl gachaAuthStage;

	// Token: 0x04000EDD RID: 3805
	private StagePresetCtrl miracleStage;

	// Token: 0x04000EDE RID: 3806
	private IEnumerator loadData;

	// Token: 0x04000EDF RID: 3807
	private int playAuthIdx;

	// Token: 0x04000EE0 RID: 3808
	private bool touchByGachaAuth;

	// Token: 0x04000EE1 RID: 3809
	private bool gachaAllSkipFlag;

	// Token: 0x04000EE2 RID: 3810
	private bool AuthEndFlg;

	// Token: 0x04000EE3 RID: 3811
	private GameObject MainFieldGachaAuth;

	// Token: 0x04000EE4 RID: 3812
	private bool IsDestroy;

	// Token: 0x02000A86 RID: 2694
	public class GachaAeGreeting
	{
		// Token: 0x06003F8F RID: 16271 RVA: 0x001EF9CC File Offset: 0x001EDBCC
		public GachaAeGreeting(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.aeCtrl = this.baseObj.transform.Find("AEImage_NewComer").GetComponent<PguiAECtrl>();
			this.Txt_CharaKind = this.baseObj.transform.Find("CharaInfo/Txt_CharaKind").GetComponent<PguiTextCtrl>();
			this.Txt_CharaName = this.baseObj.transform.Find("CharaInfo/Txt_CharaName").GetComponent<PguiTextCtrl>();
			this.Txt_WName = this.baseObj.transform.Find("CharaInfo/Txt_WName").GetComponent<PguiTextCtrl>();
			this.Txt_CharaName_Eng = this.baseObj.transform.Find("CharaInfo/Txt_CharaName_Eng").GetComponent<PguiTextCtrl>();
			this.Txt_Serif = this.baseObj.transform.Find("SerifWindow/Txt_Serif").GetComponent<PguiTextCtrl>();
			this.RenderChara = Object.Instantiate<GameObject>((GameObject)Resources.Load("RenderTextureChara/Prefab/RenderTextureCharaCtrl"), this.baseObj.transform.Find("RenderChara").transform).GetComponent<RenderTextureChara>();
			this.RenderChara.SetupRenderTexture(1440, 1024);
			this.RenderChara.postion = new Vector2(-324f, -45f);
			this.RenderChara.fieldOfView = 23f;
			this.Icon_Star = new List<PguiAECtrl>();
			for (int i = 0; i < 5; i++)
			{
				this.Icon_Star.Add(this.baseObj.transform.Find("Icon_Star/AEImage_Star" + (i + 1).ToString("D2")).GetComponent<PguiAECtrl>());
			}
			this.Bg_Pattern = this.baseObj.transform.Find("Bg/Bg_Pattern").GetComponent<Image>();
			this.Bg_Color = this.Bg_Pattern.GetComponent<PguiColorCtrl>();
			this.Bg_Color.InitForce();
			this.Top_Mat = this.baseObj.transform.Find("Img_Obi_Top_Mat").GetComponent<Image>();
			this.Bot_Mat = this.baseObj.transform.Find("Img_Obi_Bottom_Mat").GetComponent<Image>();
			this.Top_Mat_Color = this.Top_Mat.GetComponent<PguiColorCtrl>();
			this.Bot_Mat_Color = this.Bot_Mat.GetComponent<PguiColorCtrl>();
			this.Top_Mat_Color.InitForce();
			this.Bot_Mat_Color.InitForce();
			this.Top_Pt = this.baseObj.transform.Find("Img_Obi_Top_Pt").GetComponent<PguiReplaceSpriteCtrl>();
			this.Bot_Pt = this.baseObj.transform.Find("Img_Obi_Bottom_Pt").GetComponent<PguiReplaceSpriteCtrl>();
			this.Top_Pt.InitForce();
			this.Bot_Pt.InitForce();
			this.Img_No = this.baseObj.transform.Find("CharaInfo/Img_No").GetComponent<Image>();
			this.Img_No_Color = this.Img_No.GetComponent<PguiColorCtrl>();
			this.Img_No_Color.InitForce();
			this.AEImage_NewComer = this.baseObj.transform.Find("AEImage_NewComer").GetComponent<PguiReplaceAECtrl>();
			this.AEImage_NewComer.InitForce();
		}

		// Token: 0x040042FE RID: 17150
		public GameObject baseObj;

		// Token: 0x040042FF RID: 17151
		public PguiAECtrl aeCtrl;

		// Token: 0x04004300 RID: 17152
		public PguiTextCtrl Txt_CharaKind;

		// Token: 0x04004301 RID: 17153
		public PguiTextCtrl Txt_CharaName;

		// Token: 0x04004302 RID: 17154
		public PguiTextCtrl Txt_WName;

		// Token: 0x04004303 RID: 17155
		public PguiTextCtrl Txt_CharaName_Eng;

		// Token: 0x04004304 RID: 17156
		public PguiTextCtrl Txt_Serif;

		// Token: 0x04004305 RID: 17157
		public RenderTextureChara RenderChara;

		// Token: 0x04004306 RID: 17158
		public List<PguiAECtrl> Icon_Star;

		// Token: 0x04004307 RID: 17159
		public Image Bg_Pattern;

		// Token: 0x04004308 RID: 17160
		public PguiColorCtrl Bg_Color;

		// Token: 0x04004309 RID: 17161
		public Image Top_Mat;

		// Token: 0x0400430A RID: 17162
		public Image Bot_Mat;

		// Token: 0x0400430B RID: 17163
		public PguiColorCtrl Top_Mat_Color;

		// Token: 0x0400430C RID: 17164
		public PguiColorCtrl Bot_Mat_Color;

		// Token: 0x0400430D RID: 17165
		public PguiReplaceSpriteCtrl Top_Pt;

		// Token: 0x0400430E RID: 17166
		public PguiReplaceSpriteCtrl Bot_Pt;

		// Token: 0x0400430F RID: 17167
		public Image Img_No;

		// Token: 0x04004310 RID: 17168
		public PguiColorCtrl Img_No_Color;

		// Token: 0x04004311 RID: 17169
		public PguiReplaceAECtrl AEImage_NewComer;
	}

	// Token: 0x02000A87 RID: 2695
	public class GachaAeItemGet
	{
		// Token: 0x06003F90 RID: 16272 RVA: 0x001EFCE8 File Offset: 0x001EDEE8
		public GachaAeItemGet(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.aeCtrl = this.baseObj.transform.Find("AEImage_ItemGet").GetComponent<PguiAECtrl>();
			this.Icon_Item = this.baseObj.transform.Find("Icon_Item").gameObject;
		}

		// Token: 0x04004312 RID: 17170
		public GameObject baseObj;

		// Token: 0x04004313 RID: 17171
		public PguiAECtrl aeCtrl;

		// Token: 0x04004314 RID: 17172
		public GameObject Icon_Item;
	}

	// Token: 0x02000A88 RID: 2696
	public class AuthItem
	{
		// Token: 0x04004315 RID: 17173
		public int itemId;

		// Token: 0x04004316 RID: 17174
		public bool isNew;

		// Token: 0x04004317 RID: 17175
		public bool replaced;

		// Token: 0x04004318 RID: 17176
		public ItemData replaceItem;

		// Token: 0x04004319 RID: 17177
		public ItemData replaceItemEx;

		// Token: 0x0400431A RID: 17178
		public bool replaceItemIsNew;

		// Token: 0x0400431B RID: 17179
		public bool replaceItemExIsNew;
	}

	// Token: 0x02000A89 RID: 2697
	public class GachaAeNickName
	{
		// Token: 0x06003F92 RID: 16274 RVA: 0x001EFD50 File Offset: 0x001EDF50
		public GachaAeNickName(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.aeCtrl = this.baseObj.transform.Find("AEImage_WName").GetComponent<PguiAECtrl>();
			this.repCtrl = this.baseObj.transform.Find("AEImage_WName").GetComponent<PguiReplaceAECtrl>();
			this.repCtrl.InitForce();
			this.Txt_WName = this.baseObj.transform.Find("WName/Txt_WName").GetComponent<PguiTextCtrl>();
			this.Txt_WName_White = this.baseObj.transform.Find("WName_White/Txt_WName_White").GetComponent<PguiTextCtrl>();
		}

		// Token: 0x0400431C RID: 17180
		public GameObject baseObj;

		// Token: 0x0400431D RID: 17181
		public PguiAECtrl aeCtrl;

		// Token: 0x0400431E RID: 17182
		public PguiReplaceAECtrl repCtrl;

		// Token: 0x0400431F RID: 17183
		public PguiTextCtrl Txt_WName;

		// Token: 0x04004320 RID: 17184
		public PguiTextCtrl Txt_WName_White;
	}

	// Token: 0x02000A8A RID: 2698
	private class GUIGachaAuth
	{
		// Token: 0x06003F93 RID: 16275 RVA: 0x001EFDFC File Offset: 0x001EDFFC
		public GUIGachaAuth(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.AEImage_New_Chara = this.baseObj.transform.Find("AEImage_New").GetComponent<PguiAECtrl>();
			this.AEImage_New_Chara.gameObject.SetActive(false);
			this.AEImage_New_Photo = this.baseObj.transform.Find("AEImage_New_Photo").GetComponent<PguiAECtrl>();
			this.AEImage_New_Photo.gameObject.SetActive(false);
			this.AEImage_New_Interior = this.baseObj.transform.Find("AEImage_New_Interior").GetComponent<PguiAECtrl>();
			this.AEImage_New_Interior.gameObject.SetActive(false);
			this.AEImage_PhotoEffect = this.baseObj.transform.Find("AEImage_PhotoEffect").GetComponent<PguiAECtrl>();
			this.AEImage_PhotoEffect.gameObject.SetActive(false);
			this.AEImage_EventEffect = this.baseObj.transform.Find("AEImage_EventEffect").GetComponent<PguiAECtrl>();
			this.AEImage_EventEffect.gameObject.SetActive(false);
			this.AEImage_BonusCharaEffect = this.baseObj.transform.Find("AEImage_BonusCharaEffect").GetComponent<PguiAECtrl>();
			this.AEImage_BonusCharaEffect.gameObject.SetActive(false);
			this.ResultFriends_WName = new GachaAuthCtrl.GachaAeNickName(this.baseObj.transform.Find("ResultFriends_WName"));
			this.ResultFriends_WName.baseObj.SetActive(false);
			List<string> list = new List<string>();
			list.Add("ResultFriends_01");
			list.Add("ResultFriends_02");
			list.Add("ResultFriends_03");
			list.Add("ResultFriends_04");
			this.ResultFriendsList = new List<PguiAECtrl>();
			foreach (string text in list)
			{
				PguiAECtrl component = this.baseObj.transform.Find(text + "/AEImage").GetComponent<PguiAECtrl>();
				component.transform.parent.gameObject.SetActive(false);
				this.ResultFriendsList.Add(component);
			}
			List<string> list2 = new List<string>();
			list2.Add("ResultPhoto_01");
			list2.Add("ResultPhoto_02");
			list2.Add("ResultPhoto_03");
			list2.Add("ResultPhoto_04");
			this.ResultPhotoList = new List<PguiAECtrl>();
			foreach (string text2 in list2)
			{
				PguiAECtrl component2 = this.baseObj.transform.Find(text2 + "/AEImage").GetComponent<PguiAECtrl>();
				component2.transform.parent.gameObject.SetActive(false);
				this.ResultPhotoList.Add(component2);
				Object.Instantiate<GameObject>(CanvasManager.RefResource.Card_Photo, this.baseObj.transform.Find(text2 + "/Card_Photo")).name = "Icon";
			}
			List<string> list3 = new List<string>();
			list3.Add("ResultInterior_R");
			list3.Add("ResultInterior_SR");
			list3.Add("ResultInterior_SSR");
			this.ResultFurnitureList = new List<PguiAECtrl>();
			foreach (string text3 in list3)
			{
				PguiAECtrl component3 = this.baseObj.transform.Find(text3 + "/AEImage").GetComponent<PguiAECtrl>();
				component3.transform.parent.gameObject.SetActive(false);
				this.ResultFurnitureList.Add(component3);
			}
			this.gachaAeItemGet = new GachaAuthCtrl.GachaAeItemGet(this.baseObj.transform.Find("ItemGet"));
			this.gachaAeItemGet.baseObj.SetActive(false);
			this.gachaAeItemBonus = new GachaAuthCtrl.GachaAeItemGet(this.baseObj.transform.Find("ItemGet_Bonus"));
			this.gachaAeItemBonus.baseObj.SetActive(false);
			this.skipButton = this.baseObj.transform.Find("Btn_Skip").GetComponent<PguiButtonCtrl>();
		}

		// Token: 0x04004321 RID: 17185
		public GameObject baseObj;

		// Token: 0x04004322 RID: 17186
		public PguiAECtrl AEImage_New_Chara;

		// Token: 0x04004323 RID: 17187
		public PguiAECtrl AEImage_New_Photo;

		// Token: 0x04004324 RID: 17188
		public PguiAECtrl AEImage_New_Interior;

		// Token: 0x04004325 RID: 17189
		public PguiAECtrl AEImage_PhotoEffect;

		// Token: 0x04004326 RID: 17190
		public PguiAECtrl AEImage_EventEffect;

		// Token: 0x04004327 RID: 17191
		public PguiAECtrl AEImage_BonusCharaEffect;

		// Token: 0x04004328 RID: 17192
		public GachaAuthCtrl.GachaAeNickName ResultFriends_WName;

		// Token: 0x04004329 RID: 17193
		public List<PguiAECtrl> ResultFriendsList;

		// Token: 0x0400432A RID: 17194
		public List<PguiAECtrl> ResultPhotoList;

		// Token: 0x0400432B RID: 17195
		public List<PguiAECtrl> ResultFurnitureList;

		// Token: 0x0400432C RID: 17196
		public GachaAuthCtrl.GachaAeItemGet gachaAeItemGet;

		// Token: 0x0400432D RID: 17197
		public GachaAuthCtrl.GachaAeItemGet gachaAeItemBonus;

		// Token: 0x0400432E RID: 17198
		public PguiButtonCtrl skipButton;
	}

	// Token: 0x02000A8B RID: 2699
	// (Invoke) Token: 0x06003F95 RID: 16277
	public delegate bool cbDispLoading();
}
