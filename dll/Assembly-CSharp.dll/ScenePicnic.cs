using System;
using System.Collections;
using System.Collections.Generic;
using CriWare;
using SGNFW.Common;
using SGNFW.Mst;
using SGNFW.Touch;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScenePicnic : BaseScene
{
	public override void OnCreateScene()
	{
		this.basePanel = AssetManager.InstantiateAssetData("ScenePicnic/GUI/Prefab/GUI_Picnic", null).GetComponent<SimpleAnimation>();
		PguiPanel pguiPanel = this.basePanel.GetComponent<PguiPanel>();
		if (pguiPanel != null)
		{
			pguiPanel.raycastTarget = false;
		}
		this.windowPanel = AssetManager.InstantiateAssetData("ScenePicnic/GUI/Prefab/GUI_Picnic_Window", Singleton<CanvasManager>.Instance.SystemMiddleArea);
		pguiPanel = this.windowPanel.GetComponent<PguiPanel>();
		if (pguiPanel != null)
		{
			pguiPanel.raycastTarget = false;
		}
		this.buyPanel = AssetManager.InstantiateAssetData("Cmn/GUI/Prefab/GUI_Picnic_Buy_Food", Singleton<CanvasManager>.Instance.SystemMiddleArea);
		this.playPanel = AssetManager.InstantiateAssetData("ScenePicnic/GUI/Prefab/GUI_Picnic_PlayItemWindow", Singleton<CanvasManager>.Instance.SystemMiddleArea);
		pguiPanel = this.playPanel.GetComponent<PguiPanel>();
		if (pguiPanel != null)
		{
			pguiPanel.raycastTarget = false;
		}
		this.field = Object.Instantiate<GameObject>((GameObject)Resources.Load("ScenePicnic/FieldScenePicnic"));
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.FRONT, this.basePanel.transform, true);
		SceneManager.Add3DObjectByBaseField(this.field.transform);
		this.hidePanel = new GameObject("hide").transform;
		this.hidePanel.SetParent(this.basePanel.transform, false);
		this.hidePanel.gameObject.SetActive(false);
		this.charaIconAll = this.basePanel.transform.Find("CharaIconAll");
		int num = 1;
		for (;;)
		{
			Transform tmp2 = this.charaIconAll.Find("Picnic_CharaIcon_" + num.ToString("D2"));
			if (tmp2 == null)
			{
				break;
			}
			tmp2.Find("Img_Blank").gameObject.AddComponent<PguiTouchTrigger>().AddListener(delegate
			{
				this.OnClickPicnicChara(tmp2);
			}, null, null, null, null);
			num++;
		}
		this.charaSelect = this.basePanel.transform.Find("CharaSelect");
		this.haveCharaPackList = new List<CharaPackData>();
		this.dispCharaPackList = new List<CharaPackData>();
		this.sortType = SortFilterDefine.SortType.LEVEL;
		this.picnicChara = new List<Transform>();
		this.charaScroll = this.charaSelect.Find("ScrollView").GetComponent<ReuseScroll>();
		this.charaScroll.InitForce();
		ReuseScroll reuseScroll = this.charaScroll;
		reuseScroll.onStartItem = (Action<int, GameObject>)Delegate.Combine(reuseScroll.onStartItem, new Action<int, GameObject>(this.SetupPicnicChara));
		ReuseScroll reuseScroll2 = this.charaScroll;
		reuseScroll2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(reuseScroll2.onUpdateItem, new Action<int, GameObject>(this.SetupPicnicChara));
		this.charaScroll.Setup(0, 0);
		this.charaSelect.Find("Btn_OK").GetComponent<PguiButtonCtrl>().AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickPicnicCharaOk), PguiButtonCtrl.SoundType.DECIDE);
		this.playIconAll = this.basePanel.transform.Find("PlayIconAll");
		int num2 = 1;
		for (;;)
		{
			Transform tmp = this.playIconAll.Find("Picnic_PlayIcon_" + num2.ToString("D2"));
			if (tmp == null)
			{
				break;
			}
			tmp.Find("Bace").gameObject.AddComponent<PguiTouchTrigger>().AddListener(delegate
			{
				this.OnClickPicnicPlay(tmp);
			}, null, null, null, null);
			num2++;
		}
		this.playSelect = this.basePanel.transform.Find("PlaySelect");
		this.havePlayPackList = new List<ScenePicnic.PlayPackData>();
		this.dispPlayPackList = new List<ScenePicnic.PlayPackData>();
		this.playCategory = 0;
		this.playBtn = this.playSelect.Find("BtnAll").GetComponentsInChildren<PguiToggleButtonCtrl>(true);
		PguiToggleButtonCtrl[] array = this.playBtn;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickPicnicPlaySel));
		}
		this.picnicPlay = new List<Transform>();
		this.playScroll = this.playSelect.Find("PlayAll/ScrollView").GetComponent<ReuseScroll>();
		this.playScroll.InitForce();
		ReuseScroll reuseScroll3 = this.playScroll;
		reuseScroll3.onStartItem = (Action<int, GameObject>)Delegate.Combine(reuseScroll3.onStartItem, new Action<int, GameObject>(this.SetupPicnicPlay));
		ReuseScroll reuseScroll4 = this.playScroll;
		reuseScroll4.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(reuseScroll4.onUpdateItem, new Action<int, GameObject>(this.SetupPicnicPlay));
		this.playScroll.Setup(0, 0);
		this.playSelect.Find("Btn_PlayOK").GetComponent<PguiButtonCtrl>().AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickPicnicPlayOk), PguiButtonCtrl.SoundType.DECIDE);
		this.leftTop = this.basePanel.transform.Find("LeftTop");
		this.leftTop.Find("Stamina/Btn_Add_Food").GetComponent<PguiButtonCtrl>().AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickStamina), PguiButtonCtrl.SoundType.DEFAULT);
		this.rightBtn = this.basePanel.transform.Find("RightBtn");
		this.rightBtn.Find("Btn_View").GetComponent<PguiButtonCtrl>().AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickPicnicView), PguiButtonCtrl.SoundType.DECIDE);
		this.rightBtn.Find("Btn_Hide").GetComponent<PguiButtonCtrl>().AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickPicnicHide), PguiButtonCtrl.SoundType.DECIDE);
		this.Campaign_TimeInfo = this.basePanel.transform.Find("SelCmn_CampaignInfo").GetComponent<PguiImageCtrl>();
		this.winItemGet = this.windowPanel.transform.Find("Window_ItemGet").GetComponent<PguiOpenWindowCtrl>();
		this.winStaminaCharge = this.windowPanel.transform.Find("Window_StaminaCharge").GetComponent<PguiOpenWindowCtrl>();
		int num3 = 1;
		for (;;)
		{
			Transform transform = this.winStaminaCharge.transform.Find("Base/Window/Food" + num3.ToString("D2"));
			if (transform == null)
			{
				break;
			}
			transform.Find("BuyFood/Btn_Buy").GetComponent<PguiButtonCtrl>().AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickFoodSel), PguiButtonCtrl.SoundType.DEFAULT);
			transform.Find("Btn_Charge").GetComponent<PguiButtonCtrl>().AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickStaminaCharge), PguiButtonCtrl.SoundType.DEFAULT);
			num3++;
		}
		this.winStaminaBuyCharge = this.winStaminaCharge.transform.Find("Base/Window/Btn_CheckBox/BaseImage/Img_Check").gameObject;
		this.winStaminaCharge.transform.Find("Base/Window/Btn_CheckBox").GetComponent<PguiButtonCtrl>().AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickStaminaBuyCharge), PguiButtonCtrl.SoundType.DEFAULT);
		this.winBuy = this.buyPanel.GetComponent<PguiOpenWindowCtrl>();
		Object.Destroy(this.buyPanel.transform.Find("Base/Window/Base_BuyInfo/Parts_NotPhotoBuy").gameObject);
		Object.Destroy(this.buyPanel.transform.Find("Base/Window/Base_SetInfo").gameObject);
		this.winBuyInfo = this.winBuy.transform.Find("Base/Window/Base_BuyInfo");
		this.winBuyIcon = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, this.winBuyInfo.Find("Buy_Img")).GetComponent<IconItemCtrl>();
		this.winBuyIcon.transform.SetAsFirstSibling();
		this.winBuyBtnPlus = this.winBuyInfo.Find("Parts_Exchange/Exchange/Btn_Plus").GetComponent<PguiButtonCtrl>();
		this.winBuyBtnMinus = this.winBuyInfo.Find("Parts_Exchange/Exchange/Btn_Minus").GetComponent<PguiButtonCtrl>();
		this.winBuyBtnMax = this.winBuyInfo.Find("Parts_Exchange/Exchange/Btn_Max").GetComponent<PguiButtonCtrl>();
		this.winBuyBtnMax.transform.Find("BaseImage/Txt").GetComponent<PguiTextCtrl>().text = "+10";
		this.winBuyBtnMin = this.winBuyInfo.Find("Parts_Exchange/Exchange/Btn_Min").GetComponent<PguiButtonCtrl>();
		this.winBuyBtnMin.transform.Find("BaseImage/Txt").GetComponent<PguiTextCtrl>().text = "-10";
		this.winBuyBtnPlus.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickFoodBuyPlus), PguiButtonCtrl.SoundType.DEFAULT);
		this.winBuyBtnMinus.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickFoodBuyMinus), PguiButtonCtrl.SoundType.DEFAULT);
		this.winBuyBtnMax.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickFoodBuyMax), PguiButtonCtrl.SoundType.DEFAULT);
		this.winBuyBtnMin.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickFoodBuyMin), PguiButtonCtrl.SoundType.DEFAULT);
		this.winBuyReport = this.winBuy.transform.Find("Base/Window/Base_BuyReport");
		this.winPlay = this.playPanel.GetComponent<PguiOpenWindowCtrl>();
		this.winPlayLeft = this.winPlay.transform.Find("Base/Window/Btn_Yaji_Left").GetComponent<PguiButtonCtrl>();
		this.winPlayLeft.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickPlayItemYaji), PguiButtonCtrl.SoundType.DEFAULT);
		this.winPlayRight = this.winPlay.transform.Find("Base/Window/Btn_Yaji_Right").GetComponent<PguiButtonCtrl>();
		this.winPlayRight.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickPlayItemYaji), PguiButtonCtrl.SoundType.DEFAULT);
		this.winPlayItem = new List<IconItemCtrl>();
		int num4 = 1;
		for (;;)
		{
			Transform transform2 = this.winPlay.transform.Find("Base/Window/InBase_fix/Icon_Item" + num4.ToString("D2"));
			if (transform2 == null)
			{
				break;
			}
			IconItemCtrl component = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item).GetComponent<IconItemCtrl>();
			component.transform.SetParent(transform2, false);
			this.winPlayItem.Add(component);
			num4++;
		}
		this.winPlayData = null;
		this.shopData = new List<KeyValuePair<DataManagerPicnic.FoodData, ShopData.ItemOne>>();
		this.chargeList = new List<int>();
		this.chargeTime = 0f;
		this.chargeIdx = 0;
		this.wipe = this.windowPanel.transform.Find("Wipe_All/AEimage_Wipe").GetComponent<PguiAECtrl>();
		this.camera = null;
		this.camPos = null;
		this.camNo = 0;
		this.chrPos = null;
		this.stageCtrl = null;
		this.basePanel.gameObject.SetActive(false);
		this.windowPanel.SetActive(false);
		this.buyPanel.SetActive(false);
		this.playPanel.SetActive(false);
		this.field.SetActive(false);
		this.effLoadList = new List<string>();
		this.loadEffect(ScenePicnic.ballEffName);
		foreach (string text in ScenePicnic.attEffName)
		{
			this.loadEffect(ScenePicnic.balonEffName + text);
			this.loadEffect(ScenePicnic.racketEffName + text);
			this.loadEffect(ScenePicnic.shutleEffName + text);
			this.loadEffect(ScenePicnic.ringEffName + text);
			this.loadEffect(ScenePicnic.ruberEffName + text);
		}
		this.loadEffect(ScenePicnic.hagoEffName);
		this.loadEffect(ScenePicnic.haneEffName);
		this.loadEffect(ScenePicnic.train1EffName);
		this.loadEffect(ScenePicnic.train2EffName);
		this.loadEffect(ScenePicnic.trWhistleEffName);
		this.loadEffect(ScenePicnic.trNoteEffName);
		foreach (string text2 in ScenePicnic.fwSmlEffName)
		{
			this.loadEffect(text2);
		}
		foreach (string text3 in ScenePicnic.emoEffName)
		{
			this.loadEffect(text3);
		}
		this.loadEffect(ScenePicnic.spinEffName);
		this.loadEffect(ScenePicnic.sheetEffName);
		this.tutorial = 0;
		this.requestNextScene = SceneManager.SceneName.None;
		this.requestNextArgs = null;
	}

	private void loadEffect(string eff)
	{
		this.effLoadList.Add(eff);
		EffectManager.ReqLoadEffect(eff, AssetManager.OWNER.PicnicStage, 0, null);
	}

	public override bool OnCreateSceneWait()
	{
		bool flag = true;
		foreach (string text in this.effLoadList)
		{
			flag &= EffectManager.IsLoadFinishEffect(text);
		}
		return flag;
	}

	private IEnumerator LoadStage(bool fade, bool load)
	{
		this.wipe.transform.parent.gameObject.SetActive(fade);
		if (this.wipe.transform.parent.gameObject.activeSelf)
		{
			SoundManager.Play("prd_se_picnic_screen_update", false, false);
			this.wipe.GetComponent<PguiReplaceAECtrl>().Replace((Random.Range(0, 6) + 1).ToString("D2"));
			this.wipe.PlayAnimation(PguiAECtrl.AmimeType.START, null);
			while (this.wipe.IsPlaying())
			{
				yield return null;
			}
			this.wipe.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
		}
		this.reqStageLoad = false;
		EffectManager.BillboardCamera = null;
		FieldCameraScaler fcs = null;
		if (load)
		{
			this.fwPos = null;
			this.sheetPos = new List<Vector3>();
			this.snPos = null;
			this.snowPos = new List<Transform>();
			this.camPos = null;
			this.camNo = 0;
			this.chrPos = null;
			if (this.stageCtrl != null)
			{
				Object.Destroy(this.stageCtrl.gameObject);
			}
			this.stageCtrl = null;
			if (this.camera != null)
			{
				Object.Destroy(this.camera);
			}
			this.camera = null;
			foreach (ScenePicnic.FirWrk firWrk in this.fwEff)
			{
				EffectManager.DestroyEffect(firWrk.eff);
			}
			this.fwEff = new List<ScenePicnic.FirWrk>();
			if (this.snowEff != null)
			{
				EffectManager.DestroyEffect(this.snowEff);
			}
			this.snowEff = null;
			if (this.snowShadowEff != null)
			{
				EffectManager.DestroyEffect(this.snowShadowEff);
			}
			this.snowShadowEff = null;
			PrjUtil.ReleaseMemory(PrjUtil.UnloadUnused / 10);
			yield return null;
		}
		else
		{
			fcs = this.camera;
		}
		this.camera = null;
		string path = StagePresetCtrl.PackDataPath + this.stageName;
		if (load)
		{
			AssetManager.LoadAssetData(path, AssetManager.OWNER.PicnicStage, 0, null);
			foreach (string text in this.fwEffName)
			{
				EffectManager.UnloadEffect(text, AssetManager.OWNER.PicnicStage);
			}
			this.fwEffName = new List<string>();
			if (!string.IsNullOrEmpty(this.snowEffName))
			{
				EffectManager.UnloadEffect(this.snowEffName, AssetManager.OWNER.PicnicStage);
				EffectManager.UnloadEffect(ScenePicnic.snowShadowPath, AssetManager.OWNER.PicnicStage);
			}
			this.snowEffName = null;
			ScenePicnic.PlayPackData playPackData = this.havePlayPackList.Find((ScenePicnic.PlayPackData itm) => itm.item.StagePackName == this.stageName);
			if (playPackData != null)
			{
				if (playPackData.item.CharaReactionId == 404)
				{
					EffectManager.ReqLoadEffect(this.snowEffName = "Ef_info_picnic_snowball", AssetManager.OWNER.PicnicStage, 0, null);
					EffectManager.ReqLoadEffect(ScenePicnic.snowShadowPath, AssetManager.OWNER.PicnicStage, 0, null);
				}
				else
				{
					foreach (string text2 in playPackData.item.FireworksEffectNameList)
					{
						this.fwEffName.Add(text2);
						EffectManager.ReqLoadEffect(text2, AssetManager.OWNER.PicnicStage, 0, null);
					}
				}
			}
		}
		this.SetGameType();
		foreach (ScenePicnic.CharaCtrl charaCtrl in this.charaList)
		{
			charaCtrl.pos = 0;
		}
		bool flag;
		do
		{
			yield return null;
			if (load)
			{
				if (!AssetManager.IsLoadFinishAssetData(path))
				{
					continue;
				}
				if (this.fwEffName.Find((string itm) => !EffectManager.IsLoadFinishEffect(itm)) != null || (!string.IsNullOrEmpty(this.snowEffName) && (!EffectManager.IsLoadFinishEffect(this.snowEffName) || !EffectManager.IsLoadFinishEffect(ScenePicnic.snowShadowPath))))
				{
					continue;
				}
			}
			flag = true;
			foreach (ScenePicnic.CharaCtrl charaCtrl2 in this.charaList)
			{
				if ((charaCtrl2.bf > 0 || charaCtrl2.af > 0 || charaCtrl2.chara != null) && (charaCtrl2.af != charaCtrl2.bf || charaCtrl2.bf != charaCtrl2.hid || charaCtrl2.hdl == null || !charaCtrl2.hdl.IsFinishInitialize()))
				{
					flag = false;
					break;
				}
			}
		}
		while (!flag);
		yield return null;
		if (load)
		{
			this.stageCtrl = AssetManager.InstantiateAssetData(path, null).GetComponent<StagePresetCtrl>();
			this.stageCtrl.transform.SetParent(this.field.transform, false);
			this.camPos = this.stageCtrl.GetComponentsInChildren<Camera>(true);
			Camera componentInChildren = this.field.GetComponentInChildren<Camera>(true);
			componentInChildren.CopyFrom(this.camPos[this.camNo = 0]);
			this.camera = componentInChildren.gameObject.AddComponent<FieldCameraScaler>();
			Camera[] array = this.camPos;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].gameObject.SetActive(false);
			}
			this.stageCtrl.Setting(this.camera.fieldCamera);
			this.camera.fieldCamera.cullingMask = (1 << this.stageCtrl.stageModelObj.layer) | (1 << LayerMask.NameToLayer(ScenePicnic.charaLayer)) | (1 << LayerMask.NameToLayer(ScenePicnic.charaShadowLayer));
			this.camera.fieldCamera.depth = (float)((SceneManager.CameraDepth[SceneManager.CanvasType.BACK] + SceneManager.CameraDepth[SceneManager.CanvasType.FRONT]) / 2);
			this.chrPos = this.stageCtrl.transform.Find("CharaPosition");
			this.chrPos.gameObject.SetActive(false);
			float num = DataManager.DmUserInfo.optionData.VolumeList[0];
			if (this.fwEffName.Count > 0)
			{
				num *= 0.5f;
			}
			SoundManager.SetCategoryVolume(SoundCategory.BGM, num);
			if (this.gameType == 11)
			{
				this.fwPos = this.stageCtrl.transform.Find("Effect_fireworks");
			}
			if (this.gameType == 12)
			{
				this.snPos = this.stageCtrl.stageModelObj.transform;
				this.snowPos.Add(this.snPos.Find("CharaPosition_c"));
				this.snowPos.Add(this.snPos.Find("CharaPosition_d"));
				this.snowPos.Add(this.snPos.Find("CharaPosition_a"));
				this.snowPos.Add(this.snPos.Find("CharaPosition_b"));
				this.snowPos.Add(this.chrPos);
			}
		}
		else
		{
			this.camera = fcs;
		}
		EffectManager.BillboardCamera = this.camera.fieldCamera;
		yield return null;
		if (this.wipe.transform.parent.gameObject.activeSelf)
		{
			this.wipe.PlayAnimation(PguiAECtrl.AmimeType.END, null);
			while (this.wipe.IsPlaying())
			{
				yield return null;
			}
			this.wipe.transform.parent.gameObject.SetActive(false);
		}
		yield break;
	}

	public override void OnEnableScene(object args)
	{
		CanvasManager.SetBgTexture(null);
		CanvasManager.HdlCmnMenu.SetupMenu(true, "ピクニック", new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickButtonMenuRetrun), "", new PguiCmnMenuCtrl.OnClickMoveSequenceButton(this.OnClickMoveSequenceButton), null);
		SoundManager.PlayBGM("prd_bgm0031");
		this.basePanel.gameObject.SetActive(true);
		this.basePanel.ExPauseAnimationLastFrame(SimpleAnimation.ExPguiStatus.END);
		this.windowPanel.SetActive(true);
		this.winItemGet.ForceClose();
		this.winStaminaCharge.ForceClose();
		this.buyPanel.SetActive(true);
		this.winBuy.ForceClose();
		this.playPanel.SetActive(true);
		this.winPlay.ForceClose();
		this.field.SetActive(true);
		this.charaIconAll.gameObject.SetActive(true);
		this.charaSelect.gameObject.SetActive(false);
		this.playIconAll.gameObject.SetActive(true);
		this.playSelect.gameObject.SetActive(false);
		this.leftTop.gameObject.SetActive(true);
		this.rightBtn.gameObject.SetActive(true);
		this.wipe.transform.parent.gameObject.SetActive(false);
		this.haveCharaPackList = new List<CharaPackData>(DataManager.DmChara.GetUserCharaMap().Values);
		this.dispCharaPackList = new List<CharaPackData>(this.haveCharaPackList);
		this.charaList = null;
		this.havePlayPackList = new List<ScenePicnic.PlayPackData>();
		using (List<DataManagerPicnic.PlayTypeData>.Enumerator enumerator = DataManager.DmPicnic.PicnicStaticData.PlayTypeDataList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				DataManagerPicnic.PlayTypeData ptd = enumerator.Current;
				DataManagerPicnic.PlayItemData playItemData = DataManager.DmPicnic.PicnicStaticData.PlayItemDataList.Find((DataManagerPicnic.PlayItemData itm) => itm.GetId() == ptd.PlayId);
				if (playItemData != null && DataManager.DmItem.GetUserItemData(playItemData.GetId()).num > 0)
				{
					this.havePlayPackList.Add(new ScenePicnic.PlayPackData
					{
						type = ptd,
						item = playItemData
					});
				}
			}
		}
		this.havePlayPackList.Sort(new Comparison<ScenePicnic.PlayPackData>(this.playSort));
		this.winPlayLeft.gameObject.SetActive(this.havePlayPackList.Count > 1);
		this.winPlayRight.gameObject.SetActive(this.havePlayPackList.Count > 1);
		this.dispPlayPackList = new List<ScenePicnic.PlayPackData>(this.havePlayPackList);
		this.playCategory = 0;
		this.playList = null;
		this.camera = null;
		this.camPos = null;
		this.camNo = 0;
		this.chrPos = null;
		this.stageCtrl = null;
		this.ballEff = null;
		this.catchEff = null;
		this.catchEffBone = null;
		this.racketEff = new Dictionary<int, EffectData>();
		this.shutleEff = null;
		this.kenpaEff = null;
		this.balon = new List<ScenePicnic.Balon>();
		this.hagoEff = new Dictionary<int, EffectData>();
		this.haneEff = null;
		this.trainEff = new List<EffectData>();
		this.trainChr = new List<ScenePicnic.CharaCtrl>();
		this.trWhistleEff = null;
		this.trNote = new List<ScenePicnic.TrNote>();
		this.trainRot = (this.trainSpd = 0f);
		this.trainStep = 0;
		this.trainLpCnt = 0;
		this.trainDist = 0f;
		this.trainSE = false;
		this.fwEff = new List<ScenePicnic.FirWrk>();
		this.fwEffName = new List<string>();
		this.fwPos = null;
		this.emoEff = new List<EffectData>();
		this.sheetEff = new List<EffectData>();
		this.sheetPos = new List<Vector3>();
		this.snowEffName = null;
		this.snPos = null;
		this.snowPos = new List<Transform>();
		this.snowEff = null;
		this.snowShadowEff = null;
		this.snowBallLpCnt = 0;
		this.snowBallRot = (this.snowBallSpd = 0f);
		this.snowBallAng = 0f;
		this.snowBallSiz = ScenePicnic.snowBallSizMin;
		this.itemGet = 1;
		this.itemGetList = new List<DataManagerPicnic.DropItemData>();
		this.kisekiUpdate = false;
		DataManager.DmPicnic.RequestPicnicGetUserData();
		DataManager.DmShop.RequestGetShopList();
		this.staminaCharge = false;
		this.monthlyType = 0;
		this.tutorial = 0;
		this.requestNextScene = SceneManager.SceneName.None;
		this.requestNextArgs = null;
	}

	private int playSort(ScenePicnic.PlayPackData a, ScenePicnic.PlayPackData b)
	{
		int num = a.item.GetCategory() - b.item.GetCategory();
		if (num == 0)
		{
			num = a.item.CharaReactionId - b.item.CharaReactionId;
		}
		if (num == 0)
		{
			num = a.item.GetId() - b.item.GetId();
		}
		return num;
	}

	private string chkStage()
	{
		double totalHours = (TimeManager.Now - DataManager.DmPicnic.StartDateTime).TotalHours;
		int length = ScenePicnic.stageList.GetLength(0);
		int num = (int)totalHours / 5 % length;
		int num2 = (int)totalHours % 5;
		List<string> list = new List<string>();
		if (this.energy > 0)
		{
			List<DataManagerPicnic.PlayData> list2 = DataManager.DmPicnic.PicnicDynamicData.PlayDataList;
			if (list2 == null)
			{
				list2 = new List<DataManagerPicnic.PlayData>();
			}
			using (List<DataManagerPicnic.PlayData>.Enumerator enumerator = list2.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DataManagerPicnic.PlayData pd = enumerator.Current;
					ScenePicnic.PlayPackData playPackData = this.havePlayPackList.Find((ScenePicnic.PlayPackData itm) => itm.type.PlayId == pd.PlayId);
					if (playPackData != null && playPackData.item.GetCategory() == 3 && !string.IsNullOrEmpty(playPackData.item.StagePackName))
					{
						list.Add(playPackData.item.StagePackName);
					}
				}
			}
		}
		if (list.Count <= 0 || num2 >= 2)
		{
			return ScenePicnic.stageList[num, 0];
		}
		return list[num % list.Count];
	}

	public override bool OnEnableSceneWait()
	{
		if (DataManager.IsServerRequesting())
		{
			return false;
		}
		if (this.charaList == null)
		{
			DataManager.DmPicnic.RequestPicnicStartTime();
			this.CalcEnegy();
			this.requestGame = ScenePicnic.GameType.Invalid;
			this.stageName = this.chkStage();
			this.stageCtrl = null;
			this.stageLoad = this.LoadStage(false, true);
			List<DataManagerPicnic.CharaData> list = DataManager.DmPicnic.PicnicDynamicData.CharaDataList;
			if (list == null)
			{
				list = new List<DataManagerPicnic.CharaData>();
			}
			this.charaList = new List<ScenePicnic.CharaCtrl>();
			DataManagerGameStatus.UserFlagData userFlagData = DataManager.DmGameStatus.MakeUserFlagData();
			int num = 1;
			for (;;)
			{
				Transform transform = this.charaIconAll.Find("Picnic_CharaIcon_" + num.ToString("D2"));
				if (transform == null)
				{
					break;
				}
				int id2 = ((num <= list.Count) ? list[num - 1].CharaId : (-1));
				CharaPackData charaPackData = this.haveCharaPackList.Find((CharaPackData itm) => itm.id == id2);
				if (charaPackData == null && id2 > 0)
				{
					id2 = 0;
				}
				transform.Find("Icon_Chara_Picnic/Icon_Chara").GetComponent<IconCharaCtrl>().Setup(charaPackData, SortFilterDefine.SortType.LEVEL, false, null, 0, -1, 0);
				transform.Find("Icon_Chara_Picnic/Icon_Chara").GetComponent<IconCharaCtrl>().DispPhotoPocketLevel(true);
				this.SetKiseki(transform.Find("Icon_Chara_Picnic/Kiseki_Num").gameObject, charaPackData);
				transform.Find("Icon_Chara_Picnic/Icon_CharaSet/Remove").gameObject.SetActive(false);
				transform.Find("Icon_Chara_Picnic/Icon_CharaSet/Fnt_Selected").gameObject.SetActive(false);
				transform.Find("Icon_Chara_Picnic/Icon_CharaSet/Txt_Disable").gameObject.SetActive(false);
				transform.Find("Icon_Chara_Picnic/Icon_CharaSet/Current").gameObject.SetActive(false);
				transform.Find("Icon_Chara_Picnic/Icon_CharaSet/AEImage_Eff_Change").GetComponent<PguiAECtrl>().PauseAnimation(PguiAECtrl.AmimeType.START, null);
				Transform transform2 = transform.Find("Mark_Lock");
				if (transform2 != null)
				{
					transform2.gameObject.SetActive(false);
				}
				transform.Find("Img_Blank/Txt_Touch").GetComponent<SimpleAnimation>().ExPlayAnimation(SimpleAnimation.ExPguiStatus.LOOP, null);
				PguiTextCtrl component = transform.Find("Img_Blank/Txt_Touch").GetComponent<PguiTextCtrl>();
				Color color = component.m_Text.color;
				color.a = 0f;
				component.m_Text.color = color;
				CharaModelHandle charaModelHandle = ((charaPackData == null) ? null : this.MakeChara(charaPackData));
				int num2 = 0;
				if (id2 < 0)
				{
					num2 = 1;
				}
				else if (num == 3 && userFlagData.ReleaseModeFlag.Picnic2 != DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Released)
				{
					num2 = 1;
				}
				else if (num == 4 && userFlagData.ReleaseModeFlag.Picnic3 != DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Released)
				{
					num2 = 1;
				}
				else if (num == 5 && userFlagData.ReleaseModeFlag.Picnic4 != DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Released)
				{
					num2 = 1;
				}
				bool flag = charaPackData != null && charaPackData.staticData.baseData.isFlyingTypeByHome;
				bool flag2 = charaPackData != null && charaPackData.equipPlayMotion == CharaClothStatic.PlayMotionType.HandsNotUse;
				this.charaList.Add(new ScenePicnic.CharaCtrl
				{
					no = num,
					bf = id2,
					af = id2,
					icon = transform,
					chara = charaPackData,
					ctrl = null,
					hdl = charaModelHandle,
					hid = id2,
					pos = 0,
					rest = -1L,
					lck = num2,
					mov = 0,
					fly = flag,
					hand = flag2,
					fw = 0f
				});
				num++;
			}
			this.staminaBuyCharge = (userFlagData.InformationsFlag.PicnicBuyCharge ? 1 : 0);
			return false;
		}
		if (this.stageLoad != null && this.stageLoad.MoveNext())
		{
			return false;
		}
		this.stageLoad = null;
		this.reqStageLoad = false;
		this.picnicChara = new List<Transform>();
		GameObject obj4 = Object.Instantiate<GameObject>((GameObject)Resources.Load("ScenePicnic/GUI/Prefab/Icon_Chara_Picnic"));
		obj4.transform.SetParent(this.hidePanel, false);
		obj4.name = "0";
		Object.Destroy(obj4.transform.Find("Icon_Chara").gameObject);
		Object.Destroy(obj4.transform.Find("Kiseki_Num").gameObject);
		obj4.transform.Find("Icon_CharaSet/Fnt_Selected").gameObject.SetActive(false);
		obj4.transform.Find("Icon_CharaSet/Txt_Disable").gameObject.SetActive(false);
		obj4.transform.Find("Icon_CharaSet/Current").gameObject.SetActive(false);
		obj4.transform.Find("Icon_CharaSet/AEImage_Eff_Change").GetComponent<PguiAECtrl>().PauseAnimation(PguiAECtrl.AmimeType.START, null);
		obj4.AddComponent<PguiCollider>();
		obj4.AddComponent<PguiTouchTrigger>().AddListener(delegate
		{
			this.OnClickPicnicCharaList(obj4.transform);
		}, null, null, null, null);
		this.picnicChara.Add(obj4.transform);
		using (List<CharaPackData>.Enumerator enumerator = this.dispCharaPackList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ScenePicnic.<>c__DisplayClass191_2 CS$<>8__locals3 = new ScenePicnic.<>c__DisplayClass191_2();
				CS$<>8__locals3.<>4__this = this;
				CS$<>8__locals3.cpd = enumerator.Current;
				GameObject obj2 = Object.Instantiate<GameObject>((GameObject)Resources.Load("ScenePicnic/GUI/Prefab/Icon_Chara_Picnic"));
				obj2.transform.SetParent(this.hidePanel, false);
				obj2.name = CS$<>8__locals3.cpd.id.ToString();
				HashSet<int> cid = DataManager.DmChara.GetSameCharaList(CS$<>8__locals3.cpd.id, false);
				bool flag3 = this.charaList.Find((ScenePicnic.CharaCtrl itm) => itm.af == CS$<>8__locals3.cpd.id) != null;
				bool flag4 = this.charaList.Find((ScenePicnic.CharaCtrl itm) => itm.af > 0 && cid.Contains(itm.af)) != null;
				obj2.transform.Find("Icon_Chara").GetComponent<IconCharaCtrl>().Setup(CS$<>8__locals3.cpd, this.sortType, flag3 || flag4, null, 0, -1, 0);
				obj2.transform.Find("Icon_Chara").GetComponent<IconCharaCtrl>().DispPhotoPocketLevel(true);
				this.SetKiseki(obj2.transform.Find("Kiseki_Num").gameObject, CS$<>8__locals3.cpd);
				obj2.transform.Find("Icon_CharaSet/Remove").gameObject.SetActive(false);
				obj2.transform.Find("Icon_CharaSet/Fnt_Selected").gameObject.SetActive(flag3);
				obj2.transform.Find("Icon_CharaSet/Txt_Disable").gameObject.SetActive(!flag3 && flag4);
				obj2.transform.Find("Icon_CharaSet/Txt_Disable").GetComponent<PguiTextCtrl>().text = "選択不可";
				obj2.transform.Find("Icon_CharaSet/Current").gameObject.SetActive(false);
				obj2.transform.Find("Icon_CharaSet/AEImage_Eff_Change").GetComponent<PguiAECtrl>().PauseAnimation(PguiAECtrl.AmimeType.START, null);
				obj2.AddComponent<PguiCollider>();
				obj2.AddComponent<PguiTouchTrigger>().AddListener(delegate
				{
					CS$<>8__locals3.<>4__this.OnClickPicnicCharaList(obj2.transform);
				}, null, null, null, null);
				this.picnicChara.Add(obj2.transform);
			}
		}
		SortWindowCtrl.RegisterData registerData = new SortWindowCtrl.RegisterData
		{
			register = SortFilterDefine.RegisterType.PICNIC_CHANGE,
			filterButton = this.charaSelect.Find("TopBtns/Btn_FilterOnOff").GetComponent<PguiButtonCtrl>(),
			sortButton = this.charaSelect.Find("TopBtns/Btn_Sort").GetComponent<PguiButtonCtrl>(),
			sortUdButton = this.charaSelect.Find("TopBtns/Btn_SortUpDown").GetComponent<PguiButtonCtrl>(),
			funcGetTargetBaseList = () => new SortWindowCtrl.SortTarget
			{
				charaList = this.haveCharaPackList
			},
			funcDisideTarget = delegate(SortWindowCtrl.SortTarget item)
			{
				this.dispCharaPackList = item.charaList;
				this.sortType = item.sortType;
				this.charaScroll.Resize((this.dispCharaPackList.Count + 1 + ScenePicnic.ScrollDeckNum - 1) / ScenePicnic.ScrollDeckNum, 0);
			}
		};
		CanvasManager.HdlOpenWindowSortFilter.Register(registerData, true, null);
		this.chgChr = null;
		if (this.playList == null)
		{
			List<DataManagerPicnic.PlayData> list2 = DataManager.DmPicnic.PicnicDynamicData.PlayDataList;
			if (list2 == null)
			{
				list2 = new List<DataManagerPicnic.PlayData>();
			}
			this.playList = new List<ScenePicnic.PlayCtrl>();
			int num3 = 1;
			for (;;)
			{
				Transform transform3 = this.playIconAll.Find("Picnic_PlayIcon_" + num3.ToString("D2"));
				if (transform3 == null)
				{
					break;
				}
				int id = ((num3 <= list2.Count) ? list2[num3 - 1].PlayId : (-1));
				ScenePicnic.PlayPackData playPackData = this.havePlayPackList.Find((ScenePicnic.PlayPackData itm) => itm.type.PlayId == id);
				if ((playPackData == null) & (id > 0))
				{
					id = 0;
				}
				PguiRawImageCtrl component2 = transform3.Find("Bace/Icon").GetComponent<PguiRawImageCtrl>();
				component2.gameObject.SetActive(playPackData != null);
				if (playPackData != null)
				{
					component2.SetRawImage(playPackData.item.GetIconName(), true, false, null);
				}
				transform3.Find("Bace/Current").gameObject.SetActive(false);
				this.playList.Add(new ScenePicnic.PlayCtrl
				{
					no = num3,
					af = id,
					bf = id,
					icon = transform3,
					play = playPackData,
					rest = -1L
				});
				num3++;
			}
		}
		this.playCategory = 0;
		for (int i = 0; i < this.playBtn.Length; i++)
		{
			this.playBtn[i].SetToggleIndex((i == this.playCategory) ? 0 : 1);
		}
		this.picnicPlay = new List<Transform>();
		GameObject obj = Object.Instantiate<GameObject>((GameObject)Resources.Load("ScenePicnic/GUI/Prefab/Icon_Play_Picnic"));
		obj.transform.SetParent(this.hidePanel, false);
		obj.name = "0";
		Object.Destroy(obj.transform.Find("Bace/Name").gameObject);
		Object.Destroy(obj.transform.Find("Bace/Line").gameObject);
		Object.Destroy(obj.transform.Find("Bace/Icon").gameObject);
		Object.Destroy(obj.transform.Find("Bace/Txt").gameObject);
		Object.Destroy(obj.transform.Find("Bace/GetItem").gameObject);
		obj.transform.Find("Bace/Disable").gameObject.SetActive(false);
		obj.transform.Find("AEImage_Eff_Change").GetComponent<PguiAECtrl>().PauseAnimation(PguiAECtrl.AmimeType.START, null);
		obj.transform.Find("Bace").gameObject.AddComponent<PguiTouchTrigger>().AddListener(delegate
		{
			this.OnClickPicnicPlayList(obj.transform);
		}, null, null, null, null);
		this.picnicPlay.Add(obj.transform);
		using (List<ScenePicnic.PlayPackData>.Enumerator enumerator2 = this.dispPlayPackList.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				ScenePicnic.<>c__DisplayClass191_6 CS$<>8__locals7 = new ScenePicnic.<>c__DisplayClass191_6();
				CS$<>8__locals7.<>4__this = this;
				CS$<>8__locals7.ppd = enumerator2.Current;
				GameObject obj3 = Object.Instantiate<GameObject>((GameObject)Resources.Load("ScenePicnic/GUI/Prefab/Icon_Play_Picnic"));
				obj3.transform.SetParent(this.hidePanel, false);
				obj3.name = CS$<>8__locals7.ppd.type.PlayId.ToString();
				Object.Destroy(obj3.transform.Find("Bace/Remove").gameObject);
				obj3.transform.Find("Bace/Icon").GetComponent<PguiRawImageCtrl>().SetRawImage(CS$<>8__locals7.ppd.item.GetIconName(), true, false, null);
				obj3.transform.Find("Bace/Name").GetComponent<PguiTextCtrl>().text = CS$<>8__locals7.ppd.item.GetName();
				ItemStaticBase itemStaticBase = DataManager.DmItem.GetItemStaticBase(CS$<>8__locals7.ppd.type.GetItemList[0].Item.itemId);
				obj3.transform.Find("Bace/Txt").GetComponent<PguiTextCtrl>().text = CS$<>8__locals7.ppd.item.GetInfo() + ((itemStaticBase == null) ? "" : itemStaticBase.GetInfo());
				obj3.transform.Find("Bace/Disable").gameObject.SetActive(this.playList.Find((ScenePicnic.PlayCtrl itm) => itm.af == CS$<>8__locals7.ppd.type.PlayId) != null);
				obj3.transform.Find("AEImage_Eff_Change").GetComponent<PguiAECtrl>().PauseAnimation(PguiAECtrl.AmimeType.START, null);
				obj3.transform.Find("Bace").gameObject.AddComponent<PguiTouchTrigger>().AddListener(delegate
				{
					CS$<>8__locals7.<>4__this.OnClickPicnicPlayList(obj3.transform);
				}, null, null, null, null);
				obj3.transform.Find("Bace/GetItem").gameObject.AddComponent<PguiTouchTrigger>().AddListener(null, delegate
				{
					CS$<>8__locals7.<>4__this.OnClickPicnicPlayItem(obj3.transform);
				}, null, null, null);
				this.picnicPlay.Add(obj3.transform);
			}
		}
		this.chgPly = null;
		this.hideMode = false;
		this.shopData = new List<KeyValuePair<DataManagerPicnic.FoodData, ShopData.ItemOne>>();
		ShopData shopData = DataManager.DmShop.GetShopData(DataManager.DmPicnic.food_shop_id);
		List<DataManagerPicnic.FoodData> list3 = new List<DataManagerPicnic.FoodData>(DataManager.DmPicnic.PicnicStaticData.FoodDataList);
		list3.Sort((DataManagerPicnic.FoodData a, DataManagerPicnic.FoodData b) => a.AddEnergyNum.CompareTo(b.AddEnergyNum));
		int num4 = 0;
		using (List<DataManagerPicnic.FoodData>.Enumerator enumerator3 = list3.GetEnumerator())
		{
			while (enumerator3.MoveNext())
			{
				DataManagerPicnic.FoodData fd = enumerator3.Current;
				ShopData.ItemOne itemOne = ((shopData == null || shopData.oneDataList == null) ? null : shopData.oneDataList.Find((ShopData.ItemOne d) => d.itemId == fd.Id));
				if (itemOne != null)
				{
					Transform transform4 = this.winStaminaCharge.transform;
					string text = "Base/Window/Food";
					int num5;
					num4 = (num5 = num4 + 1);
					Transform transform5 = transform4.Find(text + num5.ToString("D2"));
					if (transform5 == null)
					{
						break;
					}
					transform5.gameObject.SetActive(true);
					transform5.Find("Titlebase/Title").GetComponent<PguiTextCtrl>().text = "+" + (fd.AddEnergyNum / 60).ToString() + "分";
					transform5.Find("Icon_Item/Icon").GetComponent<PguiRawImageCtrl>().SetRawImage(DataManager.DmItem.GetUserItemData(fd.Id).staticData.GetIconName(), true, false, null);
					transform5.Find("Icon_Item/AEimage_Frame").GetComponent<PguiAECtrl>().PauseAnimation(PguiAECtrl.AmimeType.START, null);
					transform5.Find("BuyFood/Price").GetComponent<PguiTextCtrl>().text = "G" + itemOne.priceItemNum.ToString();
					this.shopData.Add(new KeyValuePair<DataManagerPicnic.FoodData, ShopData.ItemOne>(fd, itemOne));
				}
			}
		}
		for (;;)
		{
			Transform transform6 = this.winStaminaCharge.transform;
			string text2 = "Base/Window/Food";
			int num5;
			num4 = (num5 = num4 + 1);
			Transform transform7 = transform6.Find(text2 + num5.ToString("D2"));
			if (transform7 == null)
			{
				break;
			}
			transform7.gameObject.SetActive(false);
		}
		this.winStaminaCharge.transform.Find("Base/Window/StaminaGage/AEimage_CountUp").GetComponent<PguiAECtrl>().PauseAnimation(PguiAECtrl.AmimeType.START, null);
		this.winStaminaBuyCharge.SetActive(this.staminaBuyCharge > 0);
		this.foodBuy = 0;
		this.DispFood();
		this.energy = (this.energyBase = 0);
		this.leftTop.Find("Stamina/Chargelamp").gameObject.SetActive(false);
		this.chargeList = new List<int>();
		this.chargeTime = 0f;
		this.chargeIdx = 0;
		this.DispEnergy();
		this.stageCtrl.gameObject.SetActive(false);
		this.stageCtrl.gameObject.SetActive(true);
		if (this.energy > 0)
		{
			foreach (ScenePicnic.CharaCtrl charaCtrl in this.charaList)
			{
				if (charaCtrl.hdl != null)
				{
					charaCtrl.pos = this.ChkPos(0, charaCtrl.no);
					charaCtrl.hdl.SetModelActive(true);
					this.SetCharaStay(charaCtrl);
					charaCtrl.ctrl = this.charaStay(charaCtrl);
				}
			}
		}
		SGNFW.Touch.Manager.RegisterTap(new SGNFW.Touch.Manager.SingleAction(this.OnTap));
		return true;
	}

	private void SetGameType()
	{
		List<int> list = new List<int>();
		List<DataManagerPicnic.CharaData> list2 = DataManager.DmPicnic.PicnicDynamicData.CharaDataList;
		if (list2 == null)
		{
			list2 = new List<DataManagerPicnic.CharaData>();
		}
		using (List<DataManagerPicnic.CharaData>.Enumerator enumerator = list2.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				DataManagerPicnic.CharaData cd = enumerator.Current;
				if (this.haveCharaPackList.Find((CharaPackData itm) => itm.id == cd.CharaId) != null)
				{
					list.Add(cd.CharaId);
				}
			}
		}
		List<ScenePicnic.GameType> list3 = new List<ScenePicnic.GameType>
		{
			ScenePicnic.GameType.Sit,
			ScenePicnic.GameType.Walk
		};
		List<int> list4 = new List<int>();
		List<int> list5 = new List<int>();
		List<int> list6 = new List<int>();
		List<int> list7 = new List<int>();
		List<int> list8 = new List<int>();
		List<int> list9 = new List<int>();
		List<DataManagerPicnic.PlayData> list10 = DataManager.DmPicnic.PicnicDynamicData.PlayDataList;
		if (list10 == null)
		{
			list10 = new List<DataManagerPicnic.PlayData>();
		}
		using (List<DataManagerPicnic.PlayData>.Enumerator enumerator2 = list10.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				DataManagerPicnic.PlayData pd = enumerator2.Current;
				ScenePicnic.PlayPackData playPackData = this.havePlayPackList.Find((ScenePicnic.PlayPackData itm) => itm.type.PlayId == pd.PlayId);
				if (playPackData != null)
				{
					if (playPackData.item.GetCategory() == 1)
					{
						if (playPackData.item.CharaReactionId == 101)
						{
							list4.Add(playPackData.item.AttributeId);
						}
						else if (playPackData.item.CharaReactionId == 102)
						{
							list5.Add(playPackData.item.AttributeId);
						}
					}
					else if (playPackData.item.GetCategory() == 2)
					{
						if (playPackData.item.CharaReactionId == 201)
						{
							list6.Add(playPackData.item.AttributeId);
						}
						else if (playPackData.item.CharaReactionId == 202)
						{
							list7.Add(playPackData.item.AttributeId);
						}
						else if (playPackData.item.CharaReactionId == 203)
						{
							list8.Add(playPackData.item.AttributeId);
						}
						else if (playPackData.item.CharaReactionId == 204)
						{
							list9.Add(playPackData.item.AttributeId);
						}
					}
				}
			}
		}
		if (list.Count > 1)
		{
			list3.Add(ScenePicnic.GameType.Kick);
			list3.Add(ScenePicnic.GameType.Hana);
			if (list4.Count > 0)
			{
				list3.Add(ScenePicnic.GameType.Catch);
			}
			if (list5.Count > 0)
			{
				list3.Add(ScenePicnic.GameType.Badmint);
			}
			if (list8.Count > 0)
			{
				list3.Add(ScenePicnic.GameType.Hane);
			}
			if (list9.Count > 0)
			{
				list3.Add(ScenePicnic.GameType.Train);
			}
		}
		if (list6.Count > 0)
		{
			list3.Add(ScenePicnic.GameType.Kenpa);
		}
		if (list7.Count > 0)
		{
			list3.Add(ScenePicnic.GameType.Balon);
		}
		this.gameType = (int)((!string.IsNullOrEmpty(this.snowEffName)) ? ScenePicnic.GameType.Snow : ((this.fwEffName.Count > 0) ? ScenePicnic.GameType.FireWork : ((this.requestGame == ScenePicnic.GameType.Invalid) ? list3[Random.Range(0, list3.Count)] : this.requestGame)));
		this.requestGame = ScenePicnic.GameType.Invalid;
		this.catchName = ScenePicnic.ruberEffName + ScenePicnic.attEffName[(list4.Count > 0) ? list4[Random.Range(0, list4.Count)] : 0];
		int num = ((list5.Count > 0) ? list5[Random.Range(0, list5.Count)] : 0);
		this.racketName = ScenePicnic.racketEffName + ScenePicnic.attEffName[num];
		this.shutleName = ScenePicnic.shutleEffName + ScenePicnic.attEffName[num];
		this.kenpaName = ScenePicnic.ringEffName + ScenePicnic.attEffName[(list6.Count > 0) ? list6[Random.Range(0, list6.Count)] : 0];
		if (list7.Count <= 0)
		{
			list7.Add(0);
		}
		int i;
		for (i = 0; i < list7.Count; i++)
		{
			while (i >= this.balon.Count)
			{
				this.balon.Add(new ScenePicnic.Balon
				{
					name = "",
					eff = null,
					time = 0f,
					no = -1,
					tag = null
				});
			}
			this.balon[i].name = ScenePicnic.balonEffName + ScenePicnic.attEffName[list7[i]];
		}
		while (i < this.balon.Count)
		{
			this.balon[i++].name = "";
		}
	}

	private void SetKiseki(GameObject obj, CharaPackData cpd)
	{
		obj.SetActive(cpd != null);
		if (cpd != null)
		{
			GrowItemData nextItemByRankup = cpd.GetNextItemByRankup(0);
			string text = "";
			int num = 1;
			int num2;
			if (nextItemByRankup == null)
			{
				num = 3;
				num2 = DataManager.DmItem.GetUserItemData(cpd.staticData.baseData.rankItemId).num;
			}
			else
			{
				text = "/" + nextItemByRankup.item.num.ToString();
				num2 = DataManager.DmItem.GetUserItemData(nextItemByRankup.item.id).num;
				if (num2 >= nextItemByRankup.item.num)
				{
					num = 2;
				}
			}
			PguiTextCtrl component = obj.transform.Find("Num_Txt").GetComponent<PguiTextCtrl>();
			component.text = ((num2 < 1000) ? num2.ToString() : "999+") + text;
			component.m_Text.color = component.GetComponent<PguiColorCtrl>().GetGameObjectById(num.ToString());
		}
	}

	public override void OnStartSceneFade()
	{
		foreach (ScenePicnic.CharaCtrl charaCtrl in this.charaList)
		{
			if (charaCtrl.ctrl != null)
			{
				charaCtrl.ctrl.MoveNext();
			}
		}
		this.CtrlGame();
	}

	public override void OnStartSceneFadeWait()
	{
		foreach (ScenePicnic.CharaCtrl charaCtrl in this.charaList)
		{
			if (charaCtrl.ctrl != null)
			{
				charaCtrl.ctrl.MoveNext();
			}
		}
		this.CtrlGame();
	}

	public override void OnStartControl()
	{
		foreach (ScenePicnic.CharaCtrl charaCtrl in this.charaList)
		{
			if (charaCtrl.ctrl != null)
			{
				charaCtrl.ctrl.MoveNext();
			}
		}
		this.CtrlGame();
	}

	private void OnClickButtonMenuRetrun()
	{
		if (this.chgChr != null)
		{
			this.OnClickPicnicCharaOk(null);
			return;
		}
		if (this.chgPly != null)
		{
			this.OnClickPicnicPlayOk(null);
			return;
		}
		this.requestNextScene = SceneManager.SceneName.SceneHome;
		this.requestNextArgs = null;
	}

	private bool OnClickMoveSequenceButton(SceneManager.SceneName sceneName, object sceneArgs)
	{
		this.OnClickPicnicCharaOk(null);
		this.OnClickPicnicPlayOk(null);
		this.requestNextScene = sceneName;
		this.requestNextArgs = sceneArgs;
		return true;
	}

	public override void Update()
	{
		if (this.itemGet != 0 && !DataManager.IsServerRequesting() && this.stageLoad == null)
		{
			if (this.itemGet < 0 && this.energy > 0)
			{
				this.reqStageLoad = true;
			}
			if (DataManager.DmPicnic.PicnicDynamicData.UpdateItemList != null)
			{
				using (List<DataManagerPicnic.DropItemData>.Enumerator enumerator = DataManager.DmPicnic.PicnicDynamicData.UpdateItemList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						DataManagerPicnic.DropItemData did = enumerator.Current;
						if (did.ItemNum > 0)
						{
							DataManagerPicnic.DropItemData dropItemData = this.itemGetList.Find((DataManagerPicnic.DropItemData itm) => itm.CharaId == did.CharaId && itm.ItemId == did.ItemId);
							if (dropItemData == null)
							{
								this.itemGetList.Add(dropItemData = new DataManagerPicnic.DropItemData(did.CharaId, did.ItemId, 0));
							}
							dropItemData.ItemNum += did.ItemNum;
							dropItemData.MonthlyBonusNum += did.MonthlyBonusNum;
							dropItemData.isCampaign = did.isCampaign;
						}
					}
				}
			}
			this.DispFood();
			this.itemGet = 0;
			foreach (ScenePicnic.CharaCtrl charaCtrl in this.charaList)
			{
				charaCtrl.rest = -1L;
			}
			foreach (ScenePicnic.PlayCtrl playCtrl in this.playList)
			{
				playCtrl.rest = -1L;
			}
		}
		if (this.itemGet == 0)
		{
			this.DispEnergy();
		}
		if (this.foodBuy < 0 && !DataManager.IsServerRequesting() && this.winBuy.FinishedClose())
		{
			if (this.foodBuy > -10)
			{
				this.winBuy.Setup("購入完了", null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, new PguiOpenWindowCtrl.Callback(this.OnClickFoodBuyReport), null, false);
				this.winBuy.ForceOpen();
				this.winBuyInfo.gameObject.SetActive(false);
				this.winBuyReport.gameObject.SetActive(true);
				SoundManager.Play("prd_se_shop_payment", false, false);
			}
			this.DispFood();
			this.foodBuy = 0;
		}
		if (this.tutorial == 0 && this.itemGet == 0 && this.foodBuy == 0 && this.chargeIdx <= 0 && this.staminaBuyCharge >= 0 && this.stageLoad == null)
		{
			if (DataManager.DmGameStatus.MakeUserFlagData().TutorialFinishFlag.PicnicFirst)
			{
				this.tutorial = -1;
			}
			else
			{
				this.tutorial = 1;
				List<string> list = new List<string> { "Texture2D/Tutorial_Window/Picnic/tutorial_picnic_01", "Texture2D/Tutorial_Window/Picnic/tutorial_picnic_02", "Texture2D/Tutorial_Window/Picnic/tutorial_picnic_03", "Texture2D/Tutorial_Window/Picnic/tutorial_picnic_04" };
				CanvasManager.HdlCmnFeedPageWindowCtrl.Open(CmnFeedPageWindowCtrl.Type.PAGE_FEED, "", list, delegate(bool b)
				{
					this.tutorial = 2;
				});
			}
		}
		else if (this.tutorial > 2)
		{
			if (!DataManager.IsServerRequesting())
			{
				this.tutorial = -1;
			}
		}
		else if (this.tutorial > 1)
		{
			DataManagerGameStatus.UserFlagData userFlagData = DataManager.DmGameStatus.MakeUserFlagData();
			userFlagData.TutorialFinishFlag.PicnicFirst = true;
			DataManager.DmGameStatus.RequestActionUpdateUserFlag(userFlagData);
			this.tutorial++;
		}
		if ((this.energyBase <= 0 || (this.chargeTime += TimeManager.DeltaTime) > 2f) && this.tutorial < 0 && this.itemGet == 0 && this.foodBuy == 0 && this.chargeIdx <= 0 && this.staminaBuyCharge >= 0 && this.chargeList.Count > 0 && this.stageLoad == null)
		{
			DataManager.DmPicnic.RequestPicnicUseFood(this.chargeList);
			this.chargeList = new List<int>();
			this.itemGet = ((this.energyBase > 0) ? 1 : (-1));
		}
		if (this.tutorial < 0 && this.itemGet == 0 && this.foodBuy == 0 && this.chargeIdx <= 0 && this.staminaBuyCharge >= 0 && this.chargeList.Count <= 0 && this.stageLoad == null && this.winItemGet.FinishedClose() && this.winBuy.FinishedClose() && this.winPlay.FinishedClose() && this.winStaminaCharge.FinishedClose() && this.itemGetList.Count > 0)
		{
			int b = 0;
			this.itemGetList.RemoveAll((DataManagerPicnic.DropItemData itm) => itm.ItemNum <= 0);
			int num = 1;
			for (;;)
			{
				Transform transform = this.winItemGet.transform.Find("Base/Window/Grid/ItemIcon" + num.ToString("D2"));
				if (transform == null)
				{
					break;
				}
				DataManagerPicnic.DropItemData dropItemData2 = this.itemGetList.Find((DataManagerPicnic.DropItemData itm) => itm.CharaId > 0);
				if (dropItemData2 == null)
				{
					transform.gameObject.SetActive(false);
				}
				else
				{
					transform.gameObject.SetActive(true);
					transform.Find("Icon_Item").GetComponent<IconItemCtrl>().Setup(new ItemData(dropItemData2.ItemId, 0));
					transform.Find("Num_Get").GetComponent<PguiTextCtrl>().text = "×" + (dropItemData2.ItemNum - dropItemData2.MonthlyBonusNum).ToString();
					transform.Find("Num_Get").GetComponent<PguiTextCtrl>().m_Text.color = transform.Find("Num_Get").GetComponent<PguiColorCtrl>().GetGameObjectById(dropItemData2.isCampaign ? "CAMPAIGN" : "NORMAL");
					PguiOutline[] array = transform.Find("Num_Get").GetComponents<PguiOutline>();
					for (int i = 0; i < array.Length; i++)
					{
						array[i].enabled = dropItemData2.isCampaign;
					}
					transform.Find("Num_Get").GetComponent<Animator>().enabled = dropItemData2.isCampaign;
					transform.Find("Num_GetPlus").GetComponent<PguiTextCtrl>().text = ((dropItemData2.MonthlyBonusNum > 0) ? ("(+" + dropItemData2.MonthlyBonusNum.ToString() + ")") : "");
					b = 1;
					this.itemGetList.Remove(dropItemData2);
				}
				num++;
			}
			string text = ((b > 0) ? "小さな輝石獲得" : "ピクニックアイテム獲得");
			string text2 = ((b > 0) ? "以下のアイテムを獲得しました" : "あそびでアイテムを獲得しました");
			if (b == 0)
			{
				int num2 = 1;
				for (;;)
				{
					Transform transform2 = this.winItemGet.transform.Find("Base/Window/Grid/ItemIcon" + num2.ToString("D2"));
					if (transform2 == null)
					{
						break;
					}
					DataManagerPicnic.DropItemData dropItemData3 = this.itemGetList.Find((DataManagerPicnic.DropItemData itm) => itm.CharaId <= 0);
					if (dropItemData3 == null)
					{
						break;
					}
					transform2.gameObject.SetActive(true);
					transform2.Find("Icon_Item").GetComponent<IconItemCtrl>().Setup(new ItemData(dropItemData3.ItemId, 0));
					transform2.Find("Num_Get").GetComponent<PguiTextCtrl>().text = "×" + dropItemData3.ItemNum.ToString();
					transform2.Find("Num_Get").GetComponent<PguiTextCtrl>().m_Text.color = transform2.Find("Num_Get").GetComponent<PguiColorCtrl>().GetGameObjectById("NORMAL");
					PguiOutline[] array = transform2.Find("Num_Get").GetComponents<PguiOutline>();
					for (int i = 0; i < array.Length; i++)
					{
						array[i].enabled = false;
					}
					transform2.Find("Num_Get").GetComponent<Animator>().enabled = false;
					transform2.Find("Num_GetPlus").GetComponent<PguiTextCtrl>().text = "";
					b = -1;
					this.itemGetList.Remove(dropItemData3);
					num2++;
				}
			}
			if (b != 0)
			{
				this.winItemGet.Setup(text, text2, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, null, delegate
				{
					this.kisekiUpdate = b > 0;
				}, false);
				this.winItemGet.ForceOpen();
			}
		}
		if (this.kisekiUpdate)
		{
			foreach (ScenePicnic.CharaCtrl charaCtrl2 in this.charaList)
			{
				this.SetKiseki(charaCtrl2.icon.Find("Icon_Chara_Picnic/Kiseki_Num").gameObject, charaCtrl2.chara);
			}
			using (List<Transform>.Enumerator enumerator4 = this.picnicChara.GetEnumerator())
			{
				while (enumerator4.MoveNext())
				{
					Transform tmp = enumerator4.Current;
					if (tmp.name != "0")
					{
						this.SetKiseki(tmp.Find("Kiseki_Num").gameObject, this.haveCharaPackList.Find((CharaPackData itm) => itm.id == int.Parse(tmp.name)));
					}
				}
			}
			this.kisekiUpdate = false;
		}
		CanvasManager.HdlCmnMenu.UpdateMenu(true, true);
		if (this.basePanel.ExIsCurrent(SimpleAnimation.ExPguiStatus.START))
		{
			if (!this.charaSelect.gameObject.activeSelf)
			{
				this.charaSelect.gameObject.SetActive(true);
			}
		}
		else if (this.basePanel.ExIsCurrent(SimpleAnimation.ExPguiStatus.END))
		{
			if (this.charaSelect.gameObject.activeSelf && !this.basePanel.ExIsPlaying())
			{
				this.charaSelect.gameObject.SetActive(false);
			}
		}
		else if (this.basePanel.ExIsCurrent(SimpleAnimation.ExPguiStatus.START_SUB))
		{
			if (!this.playSelect.gameObject.activeSelf)
			{
				this.playSelect.gameObject.SetActive(true);
			}
		}
		else if (this.basePanel.ExIsCurrent(SimpleAnimation.ExPguiStatus.END_SUB))
		{
			if (this.playSelect.gameObject.activeSelf && !this.basePanel.ExIsPlaying())
			{
				this.playSelect.gameObject.SetActive(false);
			}
		}
		else if (!this.basePanel.ExIsCurrent(SimpleAnimation.ExPguiStatus.LOOP))
		{
			this.basePanel.ExIsCurrent(SimpleAnimation.ExPguiStatus.LOOP_SUB);
		}
		if (this.stageLoad != null && !this.stageLoad.MoveNext())
		{
			this.stageLoad = null;
		}
		if (this.stageLoad == null && this.tutorial < 0 && this.itemGet == 0 && this.chgChr == null && this.chgPly == null && this.winItemGet.FinishedClose() && this.winStaminaCharge.FinishedClose() && this.winBuy.FinishedClose() && this.winPlay.FinishedClose() && CanvasManager.HdlOpenWindowBasic.FinishedClose() && this.requestNextScene == SceneManager.SceneName.None)
		{
			string text3 = this.chkStage();
			bool flag = this.stageName != text3;
			if (flag || (this.reqStageLoad && this.energy > 0))
			{
				this.stageName = text3;
				this.stageLoad = this.LoadStage(true, flag);
			}
			else
			{
				this.reqStageLoad = false;
			}
		}
		using (List<ScenePicnic.CharaCtrl>.Enumerator enumerator2 = this.charaList.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				ScenePicnic.CharaCtrl cc = enumerator2.Current;
				if (cc.ctrl == null)
				{
					if (cc.bf == cc.af && cc.af > 0 && cc.chara != null)
					{
						if (cc.hdl == null)
						{
							cc.hdl = this.MakeChara(cc.chara);
							cc.hid = cc.chara.id;
							cc.fly = cc.chara.staticData.baseData.isFlyingTypeByHome;
							cc.hand = cc.chara.equipPlayMotion == CharaClothStatic.PlayMotionType.HandsNotUse;
						}
						else if (cc.hdl.IsFinishInitialize())
						{
							if (cc.bf != cc.hid)
							{
								Object.Destroy(cc.hdl.gameObject);
								cc.hdl = null;
								cc.hid = -1;
								cc.pos = 0;
							}
							else if (this.energy > 0 && this.itemGet == 0 && !this.reqStageLoad)
							{
								cc.pos = this.ChkPos(0, cc.no);
								cc.hdl.SetModelActive(true);
								if (this.camera == null)
								{
									cc.ctrl = this.charaStay(cc);
								}
								else
								{
									cc.hdl.transform.position = this.InOutPos(cc.pos);
									Vector3 vector;
									Vector3 vector2;
									this.TargetPos(cc.pos, cc.hdl.transform.position, out vector, out vector2);
									cc.hdl.transform.LookAt(vector);
									cc.ctrl = this.charaIn(cc);
								}
								cc.ctrl.MoveNext();
							}
						}
					}
				}
				else
				{
					cc.ctrl.MoveNext();
					if (cc.ctrl == null && cc.hdl != null)
					{
						Object.Destroy(cc.hdl.gameObject);
						cc.hdl = null;
						cc.hid = -1;
						cc.pos = 0;
					}
				}
				Transform transform3 = cc.icon.Find("Mark_Lock");
				MarkLockCtrl markLockCtrl = ((transform3 == null) ? null : transform3.GetComponent<MarkLockCtrl>());
				if (markLockCtrl != null && cc.lck == 1)
				{
					markLockCtrl.gameObject.SetActive(true);
					if (cc.bf < 0)
					{
						cc.lck = -1;
					}
					else if (this.tutorial < 0)
					{
						cc.lck = 2;
					}
					DataManagerServerMst.ModeReleaseData.ModeCategory mc = DataManagerServerMst.ModeReleaseData.ModeCategory.Picnic;
					if (cc.no == 3)
					{
						mc = DataManagerServerMst.ModeReleaseData.ModeCategory.Picnic2;
					}
					else if (cc.no == 4)
					{
						mc = DataManagerServerMst.ModeReleaseData.ModeCategory.Picnic3;
					}
					else if (cc.no == 5)
					{
						mc = DataManagerServerMst.ModeReleaseData.ModeCategory.Picnic4;
					}
					DataManagerServerMst.ModeReleaseData modeReleaseData = DataManager.DmServerMst.ModeReleaseDataList.Find((DataManagerServerMst.ModeReleaseData item) => item.Category == mc);
					QuestOnePackData questOnePackData = ((modeReleaseData == null) ? null : DataManager.DmQuest.GetQuestOnePackData(modeReleaseData.QuestId));
					string text4 = "クエスト\n情報が\nありません";
					if (questOnePackData != null)
					{
						if (questOnePackData.questChapter.category == QuestStaticChapter.Category.STORY)
						{
							text4 = "メイン\nストーリー";
						}
						else if (questOnePackData.questChapter.category == QuestStaticChapter.Category.CHARA)
						{
							text4 = "キャラ\nストーリー";
						}
						else if (questOnePackData.questChapter.category == QuestStaticChapter.Category.GROW)
						{
							text4 = "成長\n";
						}
						else if (questOnePackData.questChapter.category == QuestStaticChapter.Category.EVENT)
						{
							text4 = "イベント\n";
						}
						else if (questOnePackData.questChapter.category == QuestStaticChapter.Category.SIDE_STORY)
						{
							text4 = "アライさん\n隊長日誌";
						}
						else if (questOnePackData.questChapter.category == QuestStaticChapter.Category.PVP)
						{
							text4 = "ちからくらべ\n";
						}
						else
						{
							text4 = "\n";
						}
						text4 = text4 + "\n\n" + questOnePackData.questChapter.chapterName + questOnePackData.questGroup.titleName;
					}
					MarkLockCtrl markLockCtrl2 = markLockCtrl;
					MarkLockCtrl.SetupParam setupParam = new MarkLockCtrl.SetupParam();
					setupParam.updateConditionCallback = () => false;
					setupParam.releaseFlag = false;
					setupParam.tagetObject = null;
					setupParam.text = text4;
					setupParam.updateUserFlagDataCallback = delegate
					{
						cc.lck = 3;
					};
					markLockCtrl2.Setup(setupParam, false);
					if (cc.lck == 2 && questOnePackData != null && (questOnePackData.questDynamicOne.status == QuestOneStatus.CLEAR || questOnePackData.questDynamicOne.status == QuestOneStatus.COMPLETE))
					{
						markLockCtrl.StartAEForce();
					}
				}
				else if (cc.lck == 3 && !DataManager.IsServerRequesting())
				{
					DataManagerGameStatus.UserFlagData userFlagData2 = DataManager.DmGameStatus.MakeUserFlagData();
					if (cc.no == 3)
					{
						userFlagData2.ReleaseModeFlag.Picnic2 = DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Released;
					}
					if (cc.no == 4)
					{
						userFlagData2.ReleaseModeFlag.Picnic3 = DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Released;
					}
					if (cc.no == 5)
					{
						userFlagData2.ReleaseModeFlag.Picnic4 = DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Released;
					}
					DataManager.DmGameStatus.RequestActionUpdateUserFlag(userFlagData2);
					cc.lck = 0;
				}
				PguiTextCtrl component = cc.icon.Find("Img_Blank/Txt_Touch").GetComponent<PguiTextCtrl>();
				Color color = component.m_Text.color;
				color.a = ((cc.lck == 0) ? 1f : 0f);
				component.m_Text.color = color;
			}
		}
		this.CtrlGame();
		if (this.stageLoad == null && this.itemGet == 0 && this.tutorial < 0 && this.foodBuy == 0 && this.chargeIdx <= 0 && this.staminaBuyCharge >= 0 && this.winItemGet.FinishedClose() && this.winStaminaCharge.FinishedClose() && CanvasManager.HdlOpenWindowBasic.FinishedClose() && this.chgChr == null && this.chgPly == null)
		{
			if (this.requestNextScene != SceneManager.SceneName.None)
			{
				CanvasManager.HdlCmnMenu.MoveSceneByMenu(this.requestNextScene, this.requestNextArgs);
				return;
			}
			if (this.staminaCharge)
			{
				this.winStaminaCharge.Setup(null, null, null, true, new PguiOpenWindowCtrl.Callback(this.OnClickStaminaOk), null, false);
				this.winStaminaCharge.ForceOpen();
				this.staminaCharge = false;
				return;
			}
		}
		else if (this.foodBuy != 0 || this.chargeIdx > 0 || this.staminaBuyCharge < 0 || !this.winItemGet.FinishedClose() || !this.winStaminaCharge.FinishedClose())
		{
			this.requestNextScene = SceneManager.SceneName.None;
			this.requestNextArgs = null;
		}
	}

	private void CtrlGame()
	{
		this.BallKick();
		this.CatchBall();
		this.Badminton();
		this.Kenkenpa();
		this.Balloon();
		this.Hanetsuki();
		this.Train();
		this.FireWork();
		this.Snow();
	}

	public override void LateUpdate()
	{
		if (this.catchEff != null && this.catchEffBone != null)
		{
			this.catchEff.effectObject.transform.localPosition = this.catchEffBone.TransformPoint(-0.06f, 0f, 0.08f);
		}
		using (Dictionary<int, EffectData>.KeyCollection.Enumerator enumerator = this.racketEff.Keys.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				int key2 = enumerator.Current;
				ScenePicnic.CharaCtrl charaCtrl = this.charaList.Find((ScenePicnic.CharaCtrl itm) => itm.pos == key2);
				Vector3 zero = new Vector3(-0.05f, 0f, 0.02f);
				Vector3 zero2 = new Vector3(115f, 90f, 0f);
				Transform transform = ((charaCtrl == null || charaCtrl.hand) ? null : charaCtrl.hdl.GetNodeTransform("j_wrist_r"));
				if (charaCtrl != null && transform == null)
				{
					transform = charaCtrl.hdl.GetNodeTransform("j_weapon_a");
					zero = Vector3.zero;
					zero2 = Vector3.zero;
				}
				this.racketEff[key2].effectObject.transform.position = ((transform == null) ? new Vector3(0f, -10f, 0f) : transform.position);
				this.racketEff[key2].effectObject.transform.rotation = ((transform == null) ? Quaternion.identity : transform.rotation);
				this.racketEff[key2].effectObject.transform.Translate(zero);
				this.racketEff[key2].effectObject.transform.Rotate(zero2);
			}
		}
		using (Dictionary<int, EffectData>.KeyCollection.Enumerator enumerator = this.hagoEff.Keys.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				int key = enumerator.Current;
				ScenePicnic.CharaCtrl charaCtrl2 = this.charaList.Find((ScenePicnic.CharaCtrl itm) => itm.pos == key);
				Vector3 zero3 = new Vector3(-0.08f, -0.03f, 0.03f);
				Vector3 zero4 = new Vector3(-101f, 2f, 60f);
				Transform transform2 = ((charaCtrl2 == null || charaCtrl2.hand) ? null : charaCtrl2.hdl.GetNodeTransform("j_wrist_r"));
				if (charaCtrl2 != null && transform2 == null)
				{
					transform2 = charaCtrl2.hdl.GetNodeTransform("j_weapon_a");
					zero3 = Vector3.zero;
					zero4 = Vector3.zero;
				}
				this.hagoEff[key].effectObject.transform.position = ((transform2 == null) ? new Vector3(0f, -10f, 0f) : transform2.position);
				this.hagoEff[key].effectObject.transform.rotation = ((transform2 == null) ? Quaternion.identity : transform2.rotation);
				this.hagoEff[key].effectObject.transform.Translate(zero3);
				this.hagoEff[key].effectObject.transform.Rotate(zero4);
			}
		}
		for (int i = 0; i < this.trainEff.Count; i++)
		{
			Transform transform3 = this.trainEff[i].effectObject.transform;
			float num = 0.184f;
			float num2 = 1f;
			if (i < 2)
			{
				if (this.trainChr.Count > 0)
				{
					int num3 = ((i == 0) ? 0 : (this.trainChr.Count - 1));
					CharaModelHandle hdl = this.trainChr[num3].hdl;
					Vector3 vector = hdl.transform.TransformPoint(num, num2, 0f);
					Transform transform4 = (hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.PIC_TRAIN_CALL) ? null : hdl.GetNodeTransform("j_wrist_r"));
					if (transform4 == null)
					{
						transform4 = hdl.GetNodeTransform("pelvis");
						if (transform4 != null)
						{
							vector = transform4.TransformPoint(num, 0f, 0f);
						}
					}
					else
					{
						vector = transform4.position;
					}
					Vector3 vector2 = hdl.transform.TransformPoint(-num, num2, 0f);
					transform4 = (hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.PIC_TRAIN_CALL) ? null : hdl.GetNodeTransform("j_wrist_l"));
					if (transform4 == null)
					{
						transform4 = hdl.GetNodeTransform("pelvis");
						if (transform4 != null)
						{
							vector2 = transform4.TransformPoint(-num, 0f, 0f);
						}
					}
					else
					{
						vector2 = transform4.position;
					}
					transform3.position = (vector + vector2) * 0.5f;
					float num4 = hdl.transform.eulerAngles.y;
					if (i > 0)
					{
						num4 += 180f;
					}
					transform3.eulerAngles = new Vector3(0f, num4, 0f);
				}
				else
				{
					transform3.position = new Vector3(0f, -10f, 0f);
				}
			}
			else
			{
				int num5 = i / 2;
				if (this.trainChr.Count > num5)
				{
					CharaModelHandle charaModelHandle = this.trainChr[num5].hdl;
					string text = ((i % 2 == 0) ? "j_wrist_r" : "j_wrist_l");
					float num6 = ((i % 2 == 0) ? num : (-num));
					Vector3 vector3 = charaModelHandle.transform.TransformPoint(num6, num2, 0f);
					Transform transform5 = charaModelHandle.GetNodeTransform(text);
					if (transform5 == null)
					{
						transform5 = charaModelHandle.GetNodeTransform("pelvis");
						if (transform5 != null)
						{
							vector3 = transform5.TransformPoint(num6, 0f, 0f);
						}
					}
					else
					{
						vector3 = transform5.position;
					}
					if (num5 == this.trainChr.Count - 1)
					{
						vector3.y = this.trainEff[1].effectObject.transform.position.y;
					}
					Vector3 vector4 = this.trainEff[i - 2].effectObject.transform.position;
					if (i < 4)
					{
						charaModelHandle = this.trainChr[0].hdl;
						vector4 = charaModelHandle.transform.TransformPoint(num6, num2, 0f);
						transform5 = ((i == 2 && charaModelHandle.IsCurrentAnimation(CharaMotionDefine.ActKey.PIC_TRAIN_CALL)) ? null : charaModelHandle.GetNodeTransform(text));
						if (transform5 == null)
						{
							transform5 = charaModelHandle.GetNodeTransform("pelvis");
							if (transform5 != null)
							{
								vector4 = transform5.TransformPoint(num6, 0f, 0f);
							}
						}
						else
						{
							vector4 = transform5.position;
						}
						vector4.y = this.trainEff[0].effectObject.transform.position.y;
					}
					transform3.position = vector3;
					transform3.LookAt(vector4);
					transform3.localScale = new Vector3(1f, 1f, Vector3.Distance(vector3, vector4) / 1.79f);
				}
				else
				{
					transform3.position = new Vector3(0f, -10f, 0f);
				}
			}
		}
	}

	public override void OnStopControl()
	{
	}

	public override void OnDisableScene()
	{
		if (this.ballEff != null)
		{
			EffectManager.DestroyEffect(this.ballEff);
			this.ballEff = null;
		}
		this.catchEffBone = null;
		if (this.catchEff != null)
		{
			EffectManager.DestroyEffect(this.catchEff);
			this.catchEff = null;
		}
		foreach (EffectData effectData in this.racketEff.Values)
		{
			EffectManager.DestroyEffect(effectData);
		}
		this.racketEff = new Dictionary<int, EffectData>();
		if (this.shutleEff != null)
		{
			EffectManager.DestroyEffect(this.shutleEff);
			this.shutleEff = null;
		}
		if (this.kenpaEff != null)
		{
			EffectManager.DestroyEffect(this.kenpaEff);
			this.kenpaEff = null;
		}
		foreach (ScenePicnic.Balon balon in this.balon)
		{
			balon.tag = null;
			if (balon.eff != null)
			{
				EffectManager.DestroyEffect(balon.eff);
				balon.eff = null;
			}
		}
		this.balon = new List<ScenePicnic.Balon>();
		foreach (EffectData effectData2 in this.hagoEff.Values)
		{
			EffectManager.DestroyEffect(effectData2);
		}
		this.hagoEff = new Dictionary<int, EffectData>();
		if (this.haneEff != null)
		{
			EffectManager.DestroyEffect(this.haneEff);
			this.haneEff = null;
		}
		foreach (EffectData effectData3 in this.trainEff)
		{
			EffectManager.DestroyEffect(effectData3);
		}
		this.trainEff = new List<EffectData>();
		this.trainChr = new List<ScenePicnic.CharaCtrl>();
		if (this.trWhistleEff != null)
		{
			EffectManager.DestroyEffect(this.trWhistleEff);
		}
		this.trWhistleEff = null;
		foreach (ScenePicnic.TrNote trNote in this.trNote)
		{
			if (trNote.eff != null)
			{
				EffectManager.DestroyEffect(trNote.eff);
			}
		}
		this.trNote = new List<ScenePicnic.TrNote>();
		if (this.trainSE)
		{
			this.trainSEhdl.Stop();
		}
		this.trainSE = false;
		foreach (ScenePicnic.FirWrk firWrk in this.fwEff)
		{
			EffectManager.DestroyEffect(firWrk.eff);
		}
		this.fwEff = new List<ScenePicnic.FirWrk>();
		if (this.snowEff != null)
		{
			EffectManager.DestroyEffect(this.snowEff);
		}
		this.snowEff = null;
		if (this.snowShadowEff != null)
		{
			EffectManager.DestroyEffect(this.snowShadowEff);
		}
		this.snowShadowEff = null;
		foreach (EffectData effectData4 in this.emoEff)
		{
			EffectManager.DestroyEffect(effectData4);
		}
		this.emoEff = new List<EffectData>();
		foreach (EffectData effectData5 in this.sheetEff)
		{
			EffectManager.DestroyEffect(effectData5);
		}
		this.sheetEff = new List<EffectData>();
		CanvasManager.HdlCmnMenu.SetupMenu(false, "", null, "", null, null);
		if (this.chargeList.Count > 0)
		{
			DataManager.DmPicnic.RequestPicnicUseFood(this.chargeList);
			this.chargeList = new List<int>();
		}
		SGNFW.Touch.Manager.UnRegisterTap(new SGNFW.Touch.Manager.SingleAction(this.OnTap));
		AssetManager.UnloadAssetData(StagePresetCtrl.PackDataPath + this.stageName, AssetManager.OWNER.PicnicStage);
	}

	public override bool OnDisableSceneWait()
	{
		bool flag = !DataManager.IsServerRequesting();
		if (this.stageLoad != null)
		{
			if (this.stageLoad.MoveNext())
			{
				flag = false;
			}
			else
			{
				this.stageLoad = null;
			}
		}
		foreach (ScenePicnic.CharaCtrl charaCtrl in this.charaList)
		{
			charaCtrl.ctrl = null;
			charaCtrl.chara = null;
			charaCtrl.af = (charaCtrl.bf = -1);
			if (charaCtrl.hdl != null)
			{
				if (charaCtrl.hdl.IsFinishInitialize())
				{
					Object.Destroy(charaCtrl.hdl.gameObject);
					charaCtrl.hdl = null;
					charaCtrl.hid = -1;
					charaCtrl.pos = 0;
				}
				else
				{
					flag = false;
				}
			}
		}
		if (!flag)
		{
			return false;
		}
		EffectManager.BillboardCamera = null;
		foreach (string text in this.fwEffName)
		{
			EffectManager.UnloadEffect(text, AssetManager.OWNER.PicnicStage);
		}
		this.fwEffName = new List<string>();
		if (!string.IsNullOrEmpty(this.snowEffName))
		{
			EffectManager.UnloadEffect(this.snowEffName, AssetManager.OWNER.PicnicStage);
			EffectManager.UnloadEffect(ScenePicnic.snowShadowPath, AssetManager.OWNER.PicnicStage);
		}
		this.snowEffName = null;
		this.basePanel.gameObject.SetActive(false);
		this.windowPanel.SetActive(false);
		this.buyPanel.SetActive(false);
		this.playPanel.SetActive(false);
		this.field.SetActive(false);
		this.camPos = null;
		this.chrPos = null;
		if (this.stageCtrl != null)
		{
			Object.Destroy(this.stageCtrl.gameObject);
		}
		this.stageCtrl = null;
		if (this.camera != null)
		{
			Object.Destroy(this.camera);
		}
		this.camera = null;
		foreach (ScenePicnic.CharaCtrl charaCtrl2 in this.charaList)
		{
			charaCtrl2.icon.Find("Icon_Chara_Picnic/Icon_Chara").GetComponent<IconCharaCtrl>().Setup(null, SortFilterDefine.SortType.LEVEL, false, null, 0, -1, 0);
			charaCtrl2.chara = null;
		}
		this.charaList = new List<ScenePicnic.CharaCtrl>();
		this.haveCharaPackList = new List<CharaPackData>();
		this.dispCharaPackList = new List<CharaPackData>();
		this.charaScroll.Resize(0, 0);
		foreach (Transform transform in this.picnicChara)
		{
			Object.Destroy(transform.gameObject);
		}
		this.picnicChara = new List<Transform>();
		SortWindowCtrl.RegisterData registerData = new SortWindowCtrl.RegisterData();
		registerData.register = SortFilterDefine.RegisterType.PICNIC_CHANGE;
		registerData.filterButton = null;
		registerData.sortButton = null;
		registerData.sortUdButton = null;
		registerData.funcGetTargetBaseList = () => new SortWindowCtrl.SortTarget();
		registerData.funcDisideTarget = delegate(SortWindowCtrl.SortTarget item)
		{
		};
		SortWindowCtrl.RegisterData registerData2 = registerData;
		CanvasManager.HdlOpenWindowSortFilter.Register(registerData2, false, null);
		this.playList = new List<ScenePicnic.PlayCtrl>();
		this.havePlayPackList = new List<ScenePicnic.PlayPackData>();
		this.dispPlayPackList = new List<ScenePicnic.PlayPackData>();
		this.playScroll.Resize(0, 0);
		foreach (Transform transform2 in this.picnicPlay)
		{
			Object.Destroy(transform2.gameObject);
		}
		this.picnicPlay = new List<Transform>();
		SoundManager.SetCategoryVolume(SoundCategory.BGM, DataManager.DmUserInfo.optionData.VolumeList[0]);
		return true;
	}

	public override void OnDestroyScene()
	{
		this.charaScroll = null;
		this.charaIconAll = null;
		this.charaSelect = null;
		this.playScroll = null;
		this.playIconAll = null;
		this.playSelect = null;
		this.hidePanel = null;
		this.wipe = null;
		this.leftTop = null;
		this.rightBtn = null;
		Object.Destroy(this.basePanel.gameObject);
		this.basePanel = null;
		this.winItemGet = null;
		this.winStaminaCharge = null;
		Object.Destroy(this.windowPanel);
		this.windowPanel = null;
		this.winBuy = null;
		this.winBuyInfo = null;
		this.winBuyIcon = null;
		this.winBuyBtnPlus = null;
		this.winBuyBtnMinus = null;
		this.winBuyReport = null;
		Object.Destroy(this.buyPanel);
		this.buyPanel = null;
		this.winPlay = null;
		this.winPlayLeft = null;
		this.winPlayRight = null;
		Object.Destroy(this.playPanel);
		this.playPanel = null;
		this.camera = null;
		Object.Destroy(this.field);
		this.field = null;
		foreach (string text in this.effLoadList)
		{
			EffectManager.UnloadEffect(text, AssetManager.OWNER.PicnicStage);
		}
		this.effLoadList = new List<string>();
	}

	private int CalcEnegy()
	{
		int num = (int)(TimeManager.Now - DataManager.DmPicnic.PicnicDynamicData.LastUpdateTime).TotalSeconds;
		int num2 = 0;
		using (List<DataManagerPicnic.CharaData>.Enumerator enumerator = DataManager.DmPicnic.PicnicDynamicData.CharaDataList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.CharaId > 0)
				{
					num2++;
				}
			}
		}
		if (num2 <= 0)
		{
			num = 0;
		}
		this.energyBase = DataManager.DmPicnic.PicnicDynamicData.Energy;
		if ((this.energy = this.energyBase - num) < 0)
		{
			this.energy = 0;
			num = this.energyBase;
		}
		this.energyBase = this.energy;
		return num;
	}

	private void DispFood()
	{
		int num = DataManager.DmItem.GetUserItemData(30101).num;
		this.winStaminaCharge.transform.Find("Base/Window/Parts_ItemUseCoin/NumTxt").GetComponent<PguiTextCtrl>().text = num.ToString();
		int i;
		Predicate<int> <>9__0;
		int j;
		for (i = 0; i < this.shopData.Count; i = j + 1)
		{
			Transform transform = this.winStaminaCharge.transform.Find("Base/Window/Food" + (i + 1).ToString("D2"));
			if (transform == null)
			{
				break;
			}
			int num2 = DataManager.DmItem.GetUserItemData(this.shopData[i].Key.Id).num;
			int num3 = num2;
			List<int> list = this.chargeList;
			Predicate<int> predicate;
			if ((predicate = <>9__0) == null)
			{
				predicate = (<>9__0 = (int itm) => itm == this.shopData[i].Key.Id);
			}
			if ((num2 = num3 - list.FindAll(predicate).Count) < 0)
			{
				num2 = 0;
			}
			transform.Find("Icon_Item/Num_Own").GetComponent<PguiTextCtrl>().text = num2.ToString();
			transform.Find("BuyFood/Btn_Buy").GetComponent<PguiButtonCtrl>().SetActEnable(num >= this.shopData[i].Value.priceItemNum, false, false);
			transform.Find("Btn_Charge/BaseImage/Txt").GetComponent<PguiTextCtrl>().text = ((this.staminaBuyCharge > 0 && num2 <= 0) ? "購入と" : "") + "補充";
			j = i;
		}
	}

	private void DispEnergy()
	{
		int num = this.CalcEnegy();
		using (List<int>.Enumerator enumerator = this.chargeList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				int i = enumerator.Current;
				KeyValuePair<DataManagerPicnic.FoodData, ShopData.ItemOne> keyValuePair = this.shopData.Find((KeyValuePair<DataManagerPicnic.FoodData, ShopData.ItemOne> itm) => itm.Key.Id == i);
				if (!keyValuePair.Equals(default(KeyValuePair<DataManagerPicnic.FoodData, ShopData.ItemOne>)))
				{
					this.energy += keyValuePair.Key.AddEnergyNum;
				}
			}
		}
		string text = string.Concat(new string[]
		{
			(this.energy / 3600).ToString(),
			":",
			(this.energy / 60 % 60).ToString("D2"),
			":",
			(this.energy % 60).ToString("D2")
		});
		PguiTextCtrl pguiTextCtrl = this.leftTop.Find("Stamina/Num_Time").GetComponent<PguiTextCtrl>();
		pguiTextCtrl.text = text;
		pguiTextCtrl.m_Text.color = pguiTextCtrl.GetComponent<PguiColorCtrl>().GetGameObjectById((this.energy > DataManager.DmPicnic.active_time_max) ? "02" : "01");
		pguiTextCtrl = this.winStaminaCharge.transform.Find("Base/Window/StaminaGage/Num_Time").GetComponent<PguiTextCtrl>();
		pguiTextCtrl.text = text;
		pguiTextCtrl.m_Text.color = pguiTextCtrl.GetComponent<PguiColorCtrl>().GetGameObjectById((this.energy > DataManager.DmPicnic.active_time_max) ? "1" : "0");
		float num2 = (float)this.energy / (float)DataManager.DmPicnic.active_time_max;
		num2 = Mathf.Clamp01(num2);
		this.leftTop.Find("Stamina/StaminaGage/Gage").GetComponent<PguiImageCtrl>().m_Image.fillAmount = num2;
		this.winStaminaCharge.transform.Find("Base/Window/StaminaGage/Gage").GetComponent<PguiImageCtrl>().m_Image.fillAmount = num2;
		SimpleAnimation component = this.leftTop.Find("Stamina/Chargelamp").GetComponent<SimpleAnimation>();
		if (this.energy > 0)
		{
			if (component.gameObject.activeSelf)
			{
				component.gameObject.SetActive(false);
			}
		}
		else if (!component.gameObject.activeSelf)
		{
			component.gameObject.SetActive(true);
			component.ExPlayAnimation(SimpleAnimation.ExPguiStatus.LOOP, null);
		}
		for (int l = 0; l < this.shopData.Count; l++)
		{
			Transform transform = this.winStaminaCharge.transform.Find("Base/Window/Food" + (l + 1).ToString("D2"));
			if (transform == null)
			{
				break;
			}
			int num3 = int.Parse(transform.Find("Icon_Item/Num_Own").GetComponent<PguiTextCtrl>().text);
			if (num3 <= 0 && this.staminaBuyCharge > 0 && transform.Find("BuyFood/Btn_Buy").GetComponent<PguiButtonCtrl>().ActEnable)
			{
				num3 = 1;
			}
			transform.Find("Btn_Charge").GetComponent<PguiButtonCtrl>().SetActEnable(num3 > 0 && this.energy < DataManager.DmPicnic.active_time_max, false, false);
		}
		DataManagerCampaign.CampaignPicnicData presentCampaignPicnicData = DataManager.DmCampaign.PresentCampaignPicnicData;
		this.Campaign_TimeInfo.gameObject.SetActive(presentCampaignPicnicData != null);
		if (presentCampaignPicnicData != null)
		{
			this.Campaign_TimeInfo.transform.Find("Txt_Campaign").GetComponent<PguiTextCtrl>().text = "獲得できる小さな輝石" + (presentCampaignPicnicData.picnicBuffAddratio / 100).ToString() + "倍!";
			this.Campaign_TimeInfo.transform.Find("TimeInfo/Num_Time").GetComponent<PguiTextCtrl>().text = TimeManager.MakeTimeResidueText(TimeManager.Now, presentCampaignPicnicData.endTime, true, true);
		}
		DataManagerMonthlyPack.PurchaseMonthlypackData validMonthlyPackData = DataManager.DmMonthlyPack.GetValidMonthlyPackData();
		string text2 = ((validMonthlyPackData == null) ? "" : ("獲得量" + PguiCmnMenuCtrl.Ratio2String((validMonthlyPackData.PicnicBuffAddRatio - 100) * 100) + "倍"));
		this.monthlyType = ((validMonthlyPackData == null) ? 0 : validMonthlyPackData.PackType);
		int num4 = ((validMonthlyPackData == null) ? 0 : validMonthlyPackData.PicnicBuffFrameCount);
		for (int j = 0; j < this.charaList.Count; j++)
		{
			ScenePicnic.CharaCtrl cc = this.charaList[j];
			Transform transform2 = cc.icon.Find("TimeGage");
			Transform transform3 = cc.icon.Find("Num_Time");
			Transform transform4 = cc.icon.Find("Txt");
			Transform transform5 = cc.icon.Find("Txt_Buff");
			if (cc.chara == null)
			{
				transform2.gameObject.SetActive(false);
				transform3.gameObject.SetActive(false);
				transform4.gameObject.SetActive(false);
			}
			else
			{
				transform2.gameObject.SetActive(true);
				transform3.gameObject.SetActive(true);
				transform4.gameObject.SetActive(true);
				DataManagerPicnic.GettimeData gettimeData = DataManager.DmPicnic.PicnicStaticData.GettimeDataList.Find((DataManagerPicnic.GettimeData d) => d.GettimeId == cc.chara.staticData.baseData.picnicGettimeId);
				int num5 = ((gettimeData == null) ? 1 : (cc.chara.dynamicData.CheckCharaRankMaxConversion(false) ? gettimeData.getItemRankMaxTime : gettimeData.getItemTime));
				if ((cc.rest < 0L || num > 0) && (cc.rest = DataManager.DmPicnic.PicnicDynamicData.CharaDataList[cc.no - 1].RestTime - (long)num) < 0L)
				{
					cc.rest = 0L;
				}
				long num6 = ((cc.af == cc.bf) ? cc.rest : ((long)num5));
				transform3.GetComponent<PguiTextCtrl>().text = string.Concat(new string[]
				{
					(num6 / 3600L).ToString(),
					":",
					(num6 / 60L % 60L).ToString("D2"),
					":",
					(num6 % 60L).ToString("D2")
				});
				transform2.Find("Gage").GetComponent<PguiImageCtrl>().m_Image.fillAmount = (float)num6 / (float)num5;
			}
			transform5.gameObject.SetActive(j < num4);
			transform5.GetComponent<PguiTextCtrl>().text = text2;
		}
		string text3 = "";
		for (int k = 0; k < this.playList.Count; k++)
		{
			ScenePicnic.PlayCtrl playCtrl = this.playList[k];
			Transform transform6 = playCtrl.icon.Find("Bace/TimeGage");
			Transform transform7 = playCtrl.icon.Find("Bace/Num_Time");
			Transform transform8 = playCtrl.icon.Find("Bace/Txt");
			Transform transform9 = playCtrl.icon.Find("Bace/Txt_Buff");
			if (playCtrl.play == null)
			{
				transform6.gameObject.SetActive(false);
				transform7.gameObject.SetActive(false);
				transform8.gameObject.SetActive(false);
			}
			else
			{
				transform6.gameObject.SetActive(true);
				transform7.gameObject.SetActive(true);
				transform8.gameObject.SetActive(true);
				int getTime = playCtrl.play.type.GetTime;
				if ((playCtrl.rest < 0L || num > 0) && (playCtrl.rest = DataManager.DmPicnic.PicnicDynamicData.PlayDataList[playCtrl.no - 1].RestTime - (long)num) < 0L)
				{
					playCtrl.rest = 0L;
				}
				long num7 = ((playCtrl.af == playCtrl.bf) ? playCtrl.rest : ((long)getTime));
				string text4 = string.Concat(new string[]
				{
					(num7 / 3600L).ToString(),
					":",
					(num7 / 60L % 60L).ToString("D2"),
					":",
					(num7 % 60L).ToString("D2")
				});
				transform7.GetComponent<PguiTextCtrl>().text = text4;
				transform6.Find("Gage").GetComponent<PguiImageCtrl>().m_Image.fillAmount = (float)num7 / (float)getTime;
				if (this.winPlayData != null && this.winPlayData.type.PlayId == playCtrl.af)
				{
					text3 = text4;
				}
			}
			transform9.gameObject.SetActive(this.monthlyType > 0);
		}
		if (this.winPlayData != null)
		{
			int getTime2 = this.winPlayData.type.GetTime;
			text3 = string.Concat(new string[]
			{
				(getTime2 / 3600).ToString(),
				":",
				(getTime2 / 60 % 60).ToString("D2"),
				":",
				(getTime2 % 60).ToString("D2")
			});
		}
		if (string.IsNullOrEmpty(text3))
		{
			this.winPlay.transform.Find("Base/Window/Txt_Buff").gameObject.SetActive(false);
			this.winPlay.transform.Find("Base/Window/Time_Bse").gameObject.SetActive(false);
			return;
		}
		this.winPlay.transform.Find("Base/Window/Txt_Buff").gameObject.SetActive(this.monthlyType > 0);
		this.winPlay.transform.Find("Base/Window/Time_Bse").gameObject.SetActive(true);
		this.winPlay.transform.Find("Base/Window/Time_Bse/Num_Time").GetComponent<PguiTextCtrl>().text = text3;
	}

	private void OnClickFoodSel(PguiButtonCtrl pbc)
	{
		if (this.stageLoad != null || this.itemGet != 0 || this.tutorial >= 0)
		{
			return;
		}
		if (!this.winItemGet.FinishedClose() || !this.winBuy.FinishedClose() || !this.winPlay.FinishedClose() || !CanvasManager.HdlOpenWindowBasic.FinishedClose())
		{
			return;
		}
		if (this.foodBuy != 0 || this.chargeIdx > 0 || this.staminaBuyCharge < 0)
		{
			return;
		}
		string name = pbc.transform.parent.parent.name;
		int num = int.Parse(name.Substring(name.Length - 2)) - 1;
		if (num < 0 || num >= this.shopData.Count)
		{
			return;
		}
		ShopData.ItemOne value = this.shopData[num].Value;
		this.foodBuy = this.shopData[num].Value.goodsId;
		this.foodBuyItem = DataManager.DmItem.GetUserItemData(value.itemId);
		this.winStaminaCharge.ForceClose();
		this.winBuy.Setup("購入確認", null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, new PguiOpenWindowCtrl.Callback(this.OnClickFoodBuy), null, false);
		this.winBuy.ForceOpen();
		if ((this.foodBuyAdd = value.itemNum) < 1)
		{
			this.foodBuyAdd = 1;
		}
		this.foodBuyCoin = DataManager.DmItem.GetUserItemData(value.priceItemId);
		if ((this.foodBuyPrc = value.priceItemNum) < 1)
		{
			this.foodBuyPrc = 1;
		}
		this.foodBuyMax = this.foodBuyCoin.num / this.foodBuyPrc;
		this.foodBuyNum = ((this.foodBuyMax > 0) ? 1 : 0);
		ItemData itemData = new ItemData(this.shopData[num].Key.Id, 0);
		this.winBuyIcon.Setup(itemData);
		this.winBuyInfo.Find("Buy_Img/Txt_Window/Num_Txt").GetComponent<PguiTextCtrl>().text = "×1";
		this.winBuyInfo.Find("tex_Fukidashi").gameObject.SetActive(false);
		this.winBuyInfo.Find("Txt01").GetComponent<PguiTextCtrl>().text = itemData.staticData.GetName();
		this.winBuyInfo.Find("Txt02").GetComponent<PguiTextCtrl>().text = itemData.staticData.GetInfo();
		this.winBuyInfo.Find("Parts_Exchange/Parts_ItemNeedInfo/Icon_Tex").GetComponent<PguiRawImageCtrl>().SetRawImage(this.foodBuyCoin.staticData.GetIconName(), true, false, null);
		this.winBuyInfo.Find("Parts_Exchange/Parts_ItemNeedInfo/Num_Txt").GetComponent<PguiTextCtrl>().text = this.foodBuyPrc.ToString();
		this.winBuyReport.Find("Txt01").GetComponent<PguiTextCtrl>().text = itemData.staticData.GetName();
		this.winBuyInfo.gameObject.SetActive(true);
		this.winBuyReport.gameObject.SetActive(false);
		this.winBuyInfo.Find("Parts_Exchange/Parts_ItemUseInfo/Icon_Tex").GetComponent<PguiRawImageCtrl>().SetRawImage(this.foodBuyItem.staticData.GetIconName(), true, false, null);
		this.winBuyInfo.Find("Parts_Exchange/Parts_ItemUseInfo/Txt01").GetComponent<PguiTextCtrl>().text = "所持数";
		this.winBuyInfo.Find("Parts_Exchange/Parts_ItemUseCoin/Icon_Tex").GetComponent<PguiRawImageCtrl>().SetRawImage(this.foodBuyCoin.staticData.GetIconName(), true, false, null);
		this.winBuyInfo.Find("Parts_Exchange/Parts_ItemUseCoin/Txt01").GetComponent<PguiTextCtrl>().text = "所持" + this.foodBuyCoin.staticData.GetName();
		this.winBuyReport.Find("Parts_ItemUseInfo/Icon_Tex").GetComponent<PguiRawImageCtrl>().SetRawImage(this.foodBuyItem.staticData.GetIconName(), true, false, null);
		this.winBuyReport.Find("Parts_ItemUseInfo/Txt01").GetComponent<PguiTextCtrl>().text = "所持数";
		this.winBuyReport.Find("Parts_ItemUseCoin/Icon_Tex").GetComponent<PguiRawImageCtrl>().SetRawImage(this.foodBuyCoin.staticData.GetIconName(), true, false, null);
		this.winBuyReport.Find("Parts_ItemUseCoin/Txt01").GetComponent<PguiTextCtrl>().text = "所持" + this.foodBuyCoin.staticData.GetName();
		this.DispBuy();
		this.winBuy.choiceR.GetComponent<PguiButtonCtrl>().SetActEnable(this.foodBuyMax > 0, false, false);
		this.winBuy.choiceR.transform.Find("DisableText").GetComponent<PguiTextCtrl>().text = this.foodBuyCoin.staticData.GetName() + "が\n足りません";
	}

	private void DispBuy()
	{
		this.winBuyInfo.Find("Parts_Exchange/Exchange/Tex/Num_Txt").GetComponent<PguiTextCtrl>().text = this.foodBuyNum.ToString();
		int num = this.foodBuyItem.num - this.chargeList.FindAll((int itm) => itm == this.foodBuyItem.id).Count;
		this.winBuyInfo.Find("Parts_Exchange/Parts_ItemUseInfo/Num_BeforeTxt").GetComponent<PguiTextCtrl>().text = num.ToString();
		this.winBuyInfo.Find("Parts_Exchange/Parts_ItemUseInfo/Num_AfterTxt").GetComponent<PguiTextCtrl>().text = (num + this.foodBuyNum * this.foodBuyAdd).ToString();
		this.winBuyInfo.Find("Parts_Exchange/Parts_ItemUseCoin/Num_BeforeTxt").GetComponent<PguiTextCtrl>().text = this.foodBuyCoin.num.ToString();
		this.winBuyInfo.Find("Parts_Exchange/Parts_ItemUseCoin/Num_AfterTxt").GetComponent<PguiTextCtrl>().text = (this.foodBuyCoin.num - this.foodBuyNum * this.foodBuyPrc).ToString();
		this.winBuyBtnPlus.SetActEnable(this.foodBuyNum < this.foodBuyMax, false, false);
		this.winBuyBtnMinus.SetActEnable(this.foodBuyNum > 1, false, false);
		this.winBuyBtnMax.SetActEnable(this.foodBuyNum < this.foodBuyMax, false, false);
		this.winBuyBtnMin.SetActEnable(this.foodBuyNum > 1, false, false);
		this.winBuyReport.Find("Txt02").GetComponent<PguiTextCtrl>().text = "を" + this.foodBuyNum.ToString() + "個購入しました";
		this.winBuyReport.Find("Parts_ItemUseInfo/Num_BeforeTxt").GetComponent<PguiTextCtrl>().text = num.ToString();
		this.winBuyReport.Find("Parts_ItemUseInfo/Num_AfterTxt").GetComponent<PguiTextCtrl>().text = (num + this.foodBuyNum * this.foodBuyAdd).ToString();
		this.winBuyReport.Find("Parts_ItemUseCoin/Num_BeforeTxt").GetComponent<PguiTextCtrl>().text = this.foodBuyCoin.num.ToString();
		this.winBuyReport.Find("Parts_ItemUseCoin/Num_AfterTxt").GetComponent<PguiTextCtrl>().text = (this.foodBuyCoin.num - this.foodBuyNum * this.foodBuyPrc).ToString();
	}

	private bool OnClickFoodBuy(int idx)
	{
		if (this.foodBuy <= 0)
		{
			return true;
		}
		if (idx == 1 && this.foodBuyNum > 0)
		{
			DataManager.DmShop.RequestActionBuyShopItem(this.foodBuy, this.foodBuyNum);
			this.foodBuy = -1;
		}
		else
		{
			this.winStaminaCharge.ForceOpen();
			this.foodBuy = 0;
		}
		return true;
	}

	private void OnClickFoodBuyPlus(PguiButtonCtrl pbc)
	{
		if (this.foodBuy <= 0)
		{
			return;
		}
		if (this.foodBuyNum < this.foodBuyMax)
		{
			this.foodBuyNum++;
		}
		this.DispBuy();
	}

	private void OnClickFoodBuyMinus(PguiButtonCtrl pbc)
	{
		if (this.foodBuy <= 0)
		{
			return;
		}
		if (this.foodBuyNum > 1)
		{
			this.foodBuyNum--;
		}
		this.DispBuy();
	}

	private void OnClickFoodBuyMax(PguiButtonCtrl pbc)
	{
		if (this.foodBuy <= 0)
		{
			return;
		}
		if ((this.foodBuyNum += 10) > this.foodBuyMax)
		{
			this.foodBuyNum = this.foodBuyMax;
		}
		this.DispBuy();
	}

	private void OnClickFoodBuyMin(PguiButtonCtrl pbc)
	{
		if (this.foodBuy <= 0)
		{
			return;
		}
		if ((this.foodBuyNum -= 10) < 1)
		{
			this.foodBuyNum = ((this.foodBuyMax > 0) ? 1 : 0);
		}
		this.DispBuy();
	}

	private bool OnClickFoodBuyReport(int idx)
	{
		this.winStaminaCharge.ForceOpen();
		return true;
	}

	private void OnClickStamina(PguiButtonCtrl pbc)
	{
		if (this.stageLoad != null || this.itemGet != 0 || this.tutorial >= 0 || this.foodBuy != 0 || this.chargeIdx > 0 || this.staminaBuyCharge < 0)
		{
			return;
		}
		if (!this.winItemGet.FinishedClose() || !this.winStaminaCharge.FinishedClose() || !CanvasManager.HdlOpenWindowBasic.FinishedClose())
		{
			return;
		}
		this.OnClickPicnicCharaOk(null);
		this.OnClickPicnicPlayOk(null);
		this.staminaCharge = true;
	}

	private bool OnClickStaminaOk(int idx)
	{
		return true;
	}

	private void OnClickStaminaCharge(PguiButtonCtrl pbc)
	{
		if (this.stageLoad != null || this.itemGet != 0 || this.tutorial >= 0)
		{
			return;
		}
		if (!this.winItemGet.FinishedClose() || !this.winBuy.FinishedClose() || !this.winPlay.FinishedClose() || !CanvasManager.HdlOpenWindowBasic.FinishedClose())
		{
			return;
		}
		if (this.foodBuy != 0 || this.chargeIdx > 0 || this.staminaBuyCharge < 0)
		{
			return;
		}
		if (this.energy >= DataManager.DmPicnic.active_time_max)
		{
			return;
		}
		string name = pbc.transform.parent.name;
		if ((this.chargeIdx = int.Parse(name.Substring(name.Length - 2))) > this.shopData.Count)
		{
			this.chargeIdx = 0;
		}
		if (this.chargeIdx <= 0)
		{
			return;
		}
		int id = this.shopData[this.chargeIdx - 1].Key.Id;
		if (DataManager.DmItem.GetUserItemData(id).num - this.chargeList.FindAll((int itm) => itm == id).Count <= 0 && this.staminaBuyCharge <= 0)
		{
			this.chargeIdx = 0;
			return;
		}
		if (this.energy + this.shopData[this.chargeIdx - 1].Key.AddEnergyNum > DataManager.DmPicnic.active_time_max)
		{
			CanvasManager.HdlOpenWindowBasic.Setup("確認", "げんきが最大値を超えて回復します\nこのままじゃぱりフードを使用してもよろしいですか？", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, new PguiOpenWindowCtrl.Callback(this.OnClickStaminaChargeOk), null, false);
			CanvasManager.HdlOpenWindowBasic.ForceOpen();
			this.winStaminaCharge.ForceClose();
			return;
		}
		this.OnClickStaminaChargeOk(1);
	}

	private bool OnClickStaminaChargeOk(int idx)
	{
		if (idx == 1 && this.chargeIdx > 0)
		{
			int id = this.shopData[this.chargeIdx - 1].Key.Id;
			this.chargeList.Add(id);
			if (DataManager.DmItem.GetUserItemData(id).num < this.chargeList.FindAll((int itm) => itm == id).Count)
			{
				DataManager.DmShop.RequestActionBuyShopItem(this.shopData[this.chargeIdx - 1].Value.goodsId, 1);
				this.chargeTime = 999f;
				this.foodBuy = -11;
			}
			else
			{
				this.chargeTime = 0f;
				this.DispFood();
			}
			this.winStaminaCharge.transform.Find("Base/Window/Food" + this.chargeIdx.ToString("D2")).Find("Icon_Item/AEimage_Frame").GetComponent<PguiAECtrl>()
				.PlayAnimation(PguiAECtrl.AmimeType.START, null);
			this.winStaminaCharge.transform.Find("Base/Window/StaminaGage/AEimage_CountUp").GetComponent<PguiAECtrl>().PlayAnimation(PguiAECtrl.AmimeType.START, null);
		}
		if (!this.winStaminaCharge.FinishedOpen())
		{
			this.winStaminaCharge.ForceOpen();
		}
		this.chargeIdx = 0;
		return true;
	}

	private void OnClickStaminaBuyCharge(PguiButtonCtrl pbc)
	{
		if (this.stageLoad != null || this.itemGet != 0 || this.tutorial >= 0)
		{
			return;
		}
		if (!this.winItemGet.FinishedClose() || !this.winBuy.FinishedClose() || !this.winPlay.FinishedClose() || !CanvasManager.HdlOpenWindowBasic.FinishedClose())
		{
			return;
		}
		if (this.foodBuy != 0 || this.chargeIdx > 0 || this.staminaBuyCharge < 0)
		{
			return;
		}
		if (!this.winStaminaCharge.FinishedOpen())
		{
			return;
		}
		if ((this.staminaBuyCharge -= 2) < -1)
		{
			CanvasManager.HdlOpenWindowBasic.Setup("購入と補充", "特定のフードの所持数が0の時、\n「購入と補充」が選択できます。\n「購入と補充」をタップすると、\n自動的に1個分のフードの購入、補充を行います。\n\n「購入と補充」を有効にしますか？", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, new PguiOpenWindowCtrl.Callback(this.OnClickStaminaBuyChargeOk), null, false);
			CanvasManager.HdlOpenWindowBasic.ForceOpen();
			this.winStaminaCharge.ForceClose();
			return;
		}
		this.OnClickStaminaBuyChargeOk(1);
	}

	private bool OnClickStaminaBuyChargeOk(int idx)
	{
		if (this.staminaBuyCharge >= 0)
		{
			return true;
		}
		if (idx == 1)
		{
			this.winStaminaBuyCharge.SetActive((this.staminaBuyCharge = -1 - this.staminaBuyCharge) > 0);
			DataManagerGameStatus.UserFlagData userFlagData = DataManager.DmGameStatus.MakeUserFlagData();
			userFlagData.InformationsFlag.PicnicBuyCharge = this.winStaminaBuyCharge.activeSelf;
			DataManager.DmGameStatus.RequestActionUpdateUserFlag(userFlagData);
			this.DispFood();
		}
		else
		{
			this.staminaBuyCharge += 2;
		}
		if (!this.winStaminaCharge.FinishedOpen())
		{
			this.winStaminaCharge.ForceOpen();
		}
		return true;
	}

	private void OnClickPicnicChara(Transform tmp)
	{
		if (this.stageLoad != null || this.itemGet != 0 || this.tutorial >= 0 || this.chgPly != null)
		{
			return;
		}
		if (this.foodBuy != 0 || this.chargeIdx > 0 || this.staminaBuyCharge < 0)
		{
			return;
		}
		if (!this.winItemGet.FinishedClose() || !this.winStaminaCharge.FinishedClose())
		{
			return;
		}
		if (DataManager.IsServerRequesting() || !CanvasManager.HdlOpenWindowBasic.FinishedClose())
		{
			return;
		}
		ScenePicnic.CharaCtrl charaCtrl = this.charaList.Find((ScenePicnic.CharaCtrl itm) => itm.icon == tmp);
		if (charaCtrl == null)
		{
			return;
		}
		if (charaCtrl.bf < 0)
		{
			return;
		}
		if (charaCtrl.lck != 0)
		{
			return;
		}
		if (this.chgChr == null)
		{
			this.basePanel.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
		}
		else
		{
			this.chgChr.icon.Find("Icon_Chara_Picnic/Icon_CharaSet/Current").gameObject.SetActive(false);
			if (this.chgChr.af > 0)
			{
				using (List<CharaPackData>.Enumerator enumerator = this.haveCharaPackList.FindAll((CharaPackData itm) => DataManager.DmChara.GetSameCharaList(itm.id, false).Contains(this.chgChr.af)).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CharaPackData d2 = enumerator.Current;
						Transform transform = this.picnicChara.Find((Transform itm) => itm.name == d2.id.ToString());
						if (transform != null)
						{
							transform.Find("Icon_CharaSet/Txt_Disable").gameObject.SetActive(!transform.Find("Icon_CharaSet/Fnt_Selected").gameObject.activeSelf);
							transform.Find("Icon_Chara").GetComponentInChildren<IconCharaCtrl>().IsEnableMask(true);
						}
					}
				}
			}
		}
		this.chgChr = charaCtrl;
		this.chgChr.icon.Find("Icon_Chara_Picnic/Icon_CharaSet/Current").gameObject.SetActive(true);
		if (this.chgChr.af > 0)
		{
			using (List<CharaPackData>.Enumerator enumerator = this.haveCharaPackList.FindAll((CharaPackData itm) => DataManager.DmChara.GetSameCharaList(itm.id, false).Contains(this.chgChr.af)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CharaPackData d = enumerator.Current;
					Transform transform2 = this.picnicChara.Find((Transform itm) => itm.name == d.id.ToString());
					if (transform2 != null)
					{
						transform2.Find("Icon_CharaSet/Txt_Disable").gameObject.SetActive(false);
						transform2.Find("Icon_Chara").GetComponentInChildren<IconCharaCtrl>().IsEnableMask(transform2.Find("Icon_CharaSet/Fnt_Selected").gameObject.activeSelf);
					}
				}
			}
		}
		SoundManager.Play("prd_se_click", false, false);
	}

	private void OnClickPicnicCharaList(Transform tmp)
	{
		if (this.chgChr == null)
		{
			return;
		}
		if (this.chgChr.bf < 0)
		{
			return;
		}
		if (tmp.Find("Icon_CharaSet/Fnt_Selected").gameObject.activeSelf || tmp.Find("Icon_CharaSet/Txt_Disable").gameObject.activeSelf)
		{
			return;
		}
		int id = int.Parse(tmp.name);
		CharaPackData charaPackData = this.haveCharaPackList.Find((CharaPackData itm) => itm.id == id);
		if (charaPackData == null && id > 0)
		{
			id = 0;
		}
		if (this.chgChr.af == id)
		{
			return;
		}
		if (this.chgChr.af > 0)
		{
			Transform transform = this.picnicChara.Find((Transform itm) => itm.name == this.chgChr.af.ToString());
			if (transform != null)
			{
				transform.Find("Icon_CharaSet/Fnt_Selected").gameObject.SetActive(false);
				transform.Find("Icon_CharaSet/Txt_Disable").gameObject.SetActive(false);
				transform.Find("Icon_Chara").GetComponentInChildren<IconCharaCtrl>().IsEnableMask(false);
			}
			using (List<CharaPackData>.Enumerator enumerator = this.haveCharaPackList.FindAll((CharaPackData itm) => DataManager.DmChara.GetSameCharaList(itm.id, false).Contains(this.chgChr.af)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CharaPackData d2 = enumerator.Current;
					transform = this.picnicChara.Find((Transform itm) => itm.name == d2.id.ToString());
					if (transform != null)
					{
						transform.Find("Icon_CharaSet/Txt_Disable").gameObject.SetActive(false);
						transform.Find("Icon_Chara").GetComponentInChildren<IconCharaCtrl>().IsEnableMask(transform.Find("Icon_CharaSet/Fnt_Selected").gameObject.activeSelf);
					}
				}
			}
		}
		if (id > 0)
		{
			tmp.Find("Icon_CharaSet/Fnt_Selected").gameObject.SetActive(true);
			tmp.Find("Icon_CharaSet/Txt_Disable").gameObject.SetActive(false);
			tmp.Find("Icon_Chara").GetComponentInChildren<IconCharaCtrl>().IsEnableMask(true);
			using (List<CharaPackData>.Enumerator enumerator = this.haveCharaPackList.FindAll((CharaPackData itm) => DataManager.DmChara.GetSameCharaList(itm.id, false).Contains(id)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CharaPackData d = enumerator.Current;
					Transform transform2 = this.picnicChara.Find((Transform itm) => itm.name == d.id.ToString());
					if (transform2 != null)
					{
						transform2.Find("Icon_CharaSet/Txt_Disable").gameObject.SetActive(false);
						transform2.Find("Icon_Chara").GetComponentInChildren<IconCharaCtrl>().IsEnableMask(false);
					}
				}
			}
		}
		this.chgChr.af = id;
		this.chgChr.chara = charaPackData;
		this.chgChr.icon.Find("Icon_Chara_Picnic/Icon_Chara").GetComponent<IconCharaCtrl>().Setup(this.chgChr.chara, SortFilterDefine.SortType.LEVEL, false, null, 0, -1, 0);
		this.chgChr.icon.Find("Icon_Chara_Picnic/Icon_Chara").GetComponent<IconCharaCtrl>().DispPhotoPocketLevel(true);
		this.SetKiseki(this.chgChr.icon.Find("Icon_Chara_Picnic/Kiseki_Num").gameObject, this.chgChr.chara);
		tmp.Find("Icon_CharaSet/AEImage_Eff_Change").GetComponent<PguiAECtrl>().PlayAnimation(PguiAECtrl.AmimeType.START, null);
		this.chgChr.icon.Find("Icon_Chara_Picnic/Icon_CharaSet/AEImage_Eff_Change").GetComponent<PguiAECtrl>().PlayAnimation(PguiAECtrl.AmimeType.START, null);
		SoundManager.Play((id > 0) ? "prd_se_click" : "prd_se_cancel", false, false);
	}

	private void OnClickPicnicCharaOk(PguiButtonCtrl pbc)
	{
		if (this.chgChr != null && CanvasManager.HdlOpenWindowBasic.FinishedClose())
		{
			this.basePanel.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
			bool flag = false;
			foreach (ScenePicnic.CharaCtrl charaCtrl in this.charaList)
			{
				if (charaCtrl.bf != charaCtrl.af)
				{
					flag = true;
				}
			}
			this.chgChr.icon.Find("Icon_Chara_Picnic/Icon_CharaSet/Current").gameObject.SetActive(false);
			if (this.chgChr.af > 0)
			{
				using (List<CharaPackData>.Enumerator enumerator2 = this.haveCharaPackList.FindAll((CharaPackData itm) => DataManager.DmChara.GetSameCharaList(itm.id, false).Contains(this.chgChr.af)).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						CharaPackData d = enumerator2.Current;
						Transform transform = this.picnicChara.Find((Transform itm) => itm.name == d.id.ToString());
						if (transform != null)
						{
							transform.Find("Icon_CharaSet/Txt_Disable").gameObject.SetActive(!transform.Find("Icon_CharaSet/Fnt_Selected").gameObject.activeSelf);
							transform.Find("Icon_Chara").GetComponentInChildren<IconCharaCtrl>().IsEnableMask(true);
						}
					}
				}
			}
			if (flag)
			{
				CanvasManager.HdlOpenWindowBasic.Setup("確認", "ピクニックの編成を変更します\nよろしいですか？\n\n入れ替えた枠は獲得までの時間がリセットされます", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, new PguiOpenWindowCtrl.Callback(this.OnClickPicnicCharaDecide), null, false);
				CanvasManager.HdlOpenWindowBasic.ForceOpen();
				return;
			}
			this.chgChr = null;
		}
	}

	private bool OnClickPicnicCharaDecide(int idx)
	{
		if (this.chgChr != null)
		{
			if (idx == 1)
			{
				List<int> list = new List<int>();
				foreach (ScenePicnic.CharaCtrl charaCtrl in this.charaList)
				{
					if (charaCtrl.bf != charaCtrl.af)
					{
						charaCtrl.bf = charaCtrl.af;
						this.itemGet = -1;
					}
					list.Add(charaCtrl.bf);
				}
				if (this.itemGet < 0)
				{
					DataManager.DmPicnic.RequestPicnicSetCharaList(list);
				}
				using (List<ScenePicnic.CharaCtrl>.Enumerator enumerator = this.charaList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ScenePicnic.CharaCtrl cc2 = enumerator.Current;
						if (cc2.hid > 0)
						{
							ScenePicnic.CharaCtrl charaCtrl2 = this.charaList.Find((ScenePicnic.CharaCtrl itm) => itm.bf == cc2.hid);
							if (charaCtrl2 != null)
							{
								int no = cc2.no;
								int bf = cc2.bf;
								CharaPackData chara = cc2.chara;
								Transform icon = cc2.icon;
								cc2.no = charaCtrl2.no;
								cc2.af = (cc2.bf = charaCtrl2.bf);
								cc2.chara = charaCtrl2.chara;
								cc2.icon = charaCtrl2.icon;
								charaCtrl2.no = no;
								charaCtrl2.af = (charaCtrl2.bf = bf);
								charaCtrl2.chara = chara;
								charaCtrl2.icon = icon;
							}
						}
					}
				}
				this.charaList.Sort((ScenePicnic.CharaCtrl a, ScenePicnic.CharaCtrl b) => a.no.CompareTo(b.no));
			}
			else
			{
				using (List<ScenePicnic.CharaCtrl>.Enumerator enumerator = this.charaList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ScenePicnic.CharaCtrl cc = enumerator.Current;
						cc.af = cc.bf;
						cc.chara = this.haveCharaPackList.Find((CharaPackData itm) => itm.id == cc.af);
						cc.icon.Find("Icon_Chara_Picnic/Icon_Chara").GetComponent<IconCharaCtrl>().Setup(cc.chara, SortFilterDefine.SortType.LEVEL, false, null, 0, -1, 0);
						cc.icon.Find("Icon_Chara_Picnic/Icon_Chara").GetComponent<IconCharaCtrl>().DispPhotoPocketLevel(true);
						this.SetKiseki(cc.icon.Find("Icon_Chara_Picnic/Kiseki_Num").gameObject, cc.chara);
					}
				}
			}
			this.chgChr = null;
			foreach (Transform transform in this.picnicChara)
			{
				int id = int.Parse(transform.name);
				bool flag = false;
				bool flag2 = false;
				if (id > 0)
				{
					HashSet<int> cid = DataManager.DmChara.GetSameCharaList(id, false);
					flag = this.charaList.Find((ScenePicnic.CharaCtrl itm) => itm.af == id) != null;
					flag2 = this.charaList.Find((ScenePicnic.CharaCtrl itm) => itm.af > 0 && cid.Contains(itm.af)) != null;
				}
				transform.Find("Icon_CharaSet/Fnt_Selected").gameObject.SetActive(flag);
				transform.Find("Icon_CharaSet/Txt_Disable").gameObject.SetActive(!flag && flag2);
				IconCharaCtrl componentInChildren = transform.GetComponentInChildren<IconCharaCtrl>();
				if (componentInChildren != null)
				{
					componentInChildren.IsEnableMask(flag || flag2);
				}
			}
		}
		return true;
	}

	private void SetupPicnicChara(int index, GameObject go)
	{
		foreach (object obj in go.transform)
		{
			foreach (object obj2 in ((Transform)obj))
			{
				((Transform)obj2).SetParent(this.hidePanel, false);
			}
		}
		int num = index * ScenePicnic.ScrollDeckNum;
		for (int i = 0; i < ScenePicnic.ScrollDeckNum; i++)
		{
			int num2 = i + num;
			if (num2 > this.dispCharaPackList.Count)
			{
				break;
			}
			CharaPackData charaPackData = ((num2 > 0) ? this.dispCharaPackList[num2 - 1] : null);
			string id = ((charaPackData == null) ? "0" : charaPackData.id.ToString());
			Transform transform = this.picnicChara.Find((Transform itm) => itm.name == id);
			if (transform != null)
			{
				transform.SetParent(go.transform.Find("Icon_Chara" + (i + 1).ToString("D2")), false);
				IconCharaCtrl componentInChildren = transform.GetComponentInChildren<IconCharaCtrl>();
				if (componentInChildren != null)
				{
					componentInChildren.Setup(charaPackData, this.sortType, transform.Find("Icon_CharaSet/Fnt_Selected").gameObject.activeSelf || transform.Find("Icon_CharaSet/Txt_Disable").gameObject.activeSelf, null, 0, -1, 0);
					componentInChildren.DispPhotoPocketLevel(true);
					this.SetKiseki(transform.Find("Kiseki_Num").gameObject, charaPackData);
				}
			}
		}
	}

	private void OnClickPicnicPlay(Transform tmp)
	{
		if (this.stageLoad != null || this.itemGet != 0 || this.tutorial >= 0 || this.chgChr != null)
		{
			return;
		}
		if (this.foodBuy != 0 || this.chargeIdx > 0 || this.staminaBuyCharge < 0)
		{
			return;
		}
		if (!this.winItemGet.FinishedClose() || !this.winStaminaCharge.FinishedClose() || !this.winPlay.FinishedClose())
		{
			return;
		}
		if (DataManager.IsServerRequesting() || !CanvasManager.HdlOpenWindowBasic.FinishedClose())
		{
			return;
		}
		ScenePicnic.PlayCtrl playCtrl = this.playList.Find((ScenePicnic.PlayCtrl itm) => itm.icon == tmp);
		if (playCtrl == null)
		{
			return;
		}
		if (this.chgPly == null)
		{
			this.basePanel.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START_SUB, null);
			this.OnClickPicnicPlaySel(this.playBtn[this.playCategory], -1);
		}
		else
		{
			this.chgPly.icon.Find("Bace/Current").gameObject.SetActive(false);
		}
		this.chgPly = playCtrl;
		this.chgPly.icon.Find("Bace/Current").gameObject.SetActive(true);
		SoundManager.Play("prd_se_click", false, false);
	}

	private bool OnClickPicnicPlaySel(PguiToggleButtonCtrl pbc, int toggleIndex)
	{
		if (!this.winPlay.FinishedClose())
		{
			return false;
		}
		if (this.playBtn[this.playCategory] == pbc && toggleIndex >= 0)
		{
			return false;
		}
		if (toggleIndex >= 0)
		{
			SoundManager.Play("prd_se_click", false, false);
		}
		for (int i = 0; i < this.playBtn.Length; i++)
		{
			if (this.playBtn[i] == pbc)
			{
				this.playCategory = i;
			}
		}
		for (int j = 0; j < this.playBtn.Length; j++)
		{
			this.playBtn[j].SetToggleIndex((j == this.playCategory) ? 0 : 1);
		}
		this.dispPlayPackList = this.havePlayPackList.FindAll((ScenePicnic.PlayPackData itm) => itm.item.GetCategory() == this.playCategory + 1);
		this.playScroll.Resize((this.dispPlayPackList.Count + 1 + ScenePicnic.ScrollPlayNum - 1) / ScenePicnic.ScrollPlayNum, 0);
		return false;
	}

	private void OnClickPicnicPlayList(Transform tmp)
	{
		if (!this.winPlay.FinishedClose())
		{
			return;
		}
		if (this.chgPly == null)
		{
			return;
		}
		if (this.chgPly.bf < 0)
		{
			return;
		}
		if (tmp.Find("Bace/Disable").gameObject.activeSelf)
		{
			return;
		}
		int id = int.Parse(tmp.name);
		ScenePicnic.PlayPackData playPackData = this.havePlayPackList.Find((ScenePicnic.PlayPackData itm) => itm.type.PlayId == id);
		if (playPackData == null && id > 0)
		{
			id = 0;
		}
		if (this.chgPly.af == id)
		{
			return;
		}
		if (this.chgPly.af > 0)
		{
			Transform transform = this.picnicPlay.Find((Transform itm) => itm.name == this.chgPly.af.ToString());
			if (transform != null)
			{
				transform.Find("Bace/Disable").gameObject.SetActive(false);
			}
		}
		if (id > 0)
		{
			tmp.Find("Bace/Disable").gameObject.SetActive(true);
		}
		this.chgPly.af = id;
		this.chgPly.play = playPackData;
		PguiRawImageCtrl component = this.chgPly.icon.Find("Bace/Icon").GetComponent<PguiRawImageCtrl>();
		component.gameObject.SetActive(this.chgPly.play != null);
		if (this.chgPly.play != null)
		{
			component.SetRawImage(playPackData.item.GetIconName(), true, false, null);
		}
		tmp.Find("AEImage_Eff_Change").GetComponent<PguiAECtrl>().PlayAnimation(PguiAECtrl.AmimeType.START, null);
		this.chgPly.icon.Find("AEImage_Eff_Change").GetComponent<PguiAECtrl>().PlayAnimation(PguiAECtrl.AmimeType.START, null);
		SoundManager.Play((id > 0) ? "prd_se_click" : "prd_se_cancel", false, false);
	}

	private void OnClickPicnicPlayOk(PguiButtonCtrl pbc)
	{
		if (!this.winPlay.FinishedClose())
		{
			return;
		}
		if (this.chgPly != null && CanvasManager.HdlOpenWindowBasic.FinishedClose())
		{
			this.basePanel.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END_SUB, null);
			bool flag = false;
			foreach (ScenePicnic.PlayCtrl playCtrl in this.playList)
			{
				if (playCtrl.bf != playCtrl.af)
				{
					flag = true;
				}
			}
			this.chgPly.icon.Find("Bace/Current").gameObject.SetActive(false);
			if (flag)
			{
				CanvasManager.HdlOpenWindowBasic.Setup("確認", "あそびを変更します\nよろしいですか？\n\n入れ替えた枠は獲得までの時間がリセットされます", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, new PguiOpenWindowCtrl.Callback(this.OnClickPicnicPlayDecide), null, false);
				CanvasManager.HdlOpenWindowBasic.ForceOpen();
				return;
			}
			this.chgPly = null;
		}
	}

	private bool OnClickPicnicPlayDecide(int idx)
	{
		if (this.chgPly != null)
		{
			if (idx == 1)
			{
				List<int> list = new List<int>();
				List<bool> list2 = new List<bool>();
				List<ScenePicnic.GameType> list3 = new List<ScenePicnic.GameType>();
				foreach (ScenePicnic.PlayCtrl playCtrl in this.playList)
				{
					if (playCtrl.bf != playCtrl.af)
					{
						playCtrl.bf = playCtrl.af;
						this.itemGet = -1;
						list2.Add(playCtrl.play != null && playCtrl.play.item.GetCategory() == 3);
						if (playCtrl.play != null)
						{
							if (playCtrl.play.item.GetCategory() == 1)
							{
								if (this.charaList.FindAll((ScenePicnic.CharaCtrl itm) => itm.bf > 0).Count > 1)
								{
									if (playCtrl.play.item.CharaReactionId == 101)
									{
										list3.Add(ScenePicnic.GameType.Catch);
									}
									else if (playCtrl.play.item.CharaReactionId == 102)
									{
										list3.Add(ScenePicnic.GameType.Badmint);
									}
								}
							}
							else if (playCtrl.play.item.GetCategory() == 2)
							{
								if (playCtrl.play.item.CharaReactionId == 201)
								{
									list3.Add(ScenePicnic.GameType.Kenpa);
								}
								else if (playCtrl.play.item.CharaReactionId == 202)
								{
									list3.Add(ScenePicnic.GameType.Balon);
								}
								else if (playCtrl.play.item.CharaReactionId == 203)
								{
									list3.Add(ScenePicnic.GameType.Hane);
								}
								else if (playCtrl.play.item.CharaReactionId == 204)
								{
									list3.Add(ScenePicnic.GameType.Train);
								}
							}
						}
					}
					else
					{
						list2.Add(false);
					}
					list.Add(playCtrl.bf);
				}
				if (this.itemGet < 0)
				{
					DataManager.DmPicnic.RequestPicnicSetPlayList(list);
					int num = list2.IndexOf(true);
					if (num >= 0)
					{
						DataManager.DmPicnic.RequestPicnicStartTime(TimeManager.Now.AddHours((double)(-(double)num * 5)));
					}
					this.requestGame = ((list3.Count > 0) ? list3[Random.Range(0, list3.Count)] : ScenePicnic.GameType.Invalid);
				}
			}
			else
			{
				using (List<ScenePicnic.PlayCtrl>.Enumerator enumerator = this.playList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ScenePicnic.PlayCtrl pc = enumerator.Current;
						pc.af = pc.bf;
						pc.play = this.havePlayPackList.Find((ScenePicnic.PlayPackData itm) => itm.type.PlayId == pc.af);
						PguiRawImageCtrl component = pc.icon.Find("Bace/Icon").GetComponent<PguiRawImageCtrl>();
						component.gameObject.SetActive(pc.play != null);
						if (pc.play != null)
						{
							component.SetRawImage(pc.play.item.GetIconName(), true, false, null);
						}
					}
				}
			}
			this.chgPly = null;
			foreach (Transform transform in this.picnicPlay)
			{
				int id = int.Parse(transform.name);
				transform.Find("Bace/Disable").gameObject.SetActive(id > 0 && this.playList.Find((ScenePicnic.PlayCtrl itm) => itm.af == id) != null);
			}
		}
		return true;
	}

	private void OnClickPicnicPlayItem(Transform tmp)
	{
		if (!this.winPlay.FinishedClose())
		{
			return;
		}
		if (this.chgPly == null)
		{
			return;
		}
		if (this.chgPly.bf < 0)
		{
			return;
		}
		int id = int.Parse(tmp.name);
		if (id <= 0)
		{
			return;
		}
		this.winPlayData = this.havePlayPackList.Find((ScenePicnic.PlayPackData itm) => itm.type.PlayId == id);
		if (this.winPlayData == null)
		{
			return;
		}
		this.winPlay.Setup("獲得情報", null, null, true, (int idx) => true, null, false);
		this.winPlay.ForceOpen();
		this.SetupPlayItem();
		SoundManager.Play("prd_se_click", false, false);
	}

	private void OnClickPlayItemYaji(PguiButtonCtrl pbc)
	{
		if (!this.winPlay.FinishedOpen())
		{
			return;
		}
		if (this.winPlayData == null)
		{
			return;
		}
		int num = this.havePlayPackList.IndexOf(this.winPlayData);
		if (pbc == this.winPlayLeft)
		{
			if (--num < 0)
			{
				num = this.havePlayPackList.Count - 1;
			}
		}
		else if ((pbc = this.winPlayRight) && ++num >= this.havePlayPackList.Count)
		{
			num = 0;
		}
		this.winPlayData = this.havePlayPackList[num];
		this.SetupPlayItem();
	}

	private void SetupPlayItem()
	{
		this.winPlay.transform.Find("Base/Window/Icon_Item").GetComponent<PguiRawImageCtrl>().SetRawImage(this.winPlayData.item.GetIconName(), true, false, null);
		this.winPlay.transform.Find("Base/Window/Txt01").GetComponent<PguiTextCtrl>().text = this.winPlayData.item.GetName();
		ItemLotteryData itemLotteryData = DataManager.DmItem.GetItemStaticBase(this.winPlayData.type.GetItemList[this.monthlyType].Item.itemId) as ItemLotteryData;
		List<ItemData> list = new List<ItemData>();
		if (itemLotteryData != null)
		{
			foreach (MstItemLotteryLineup mstItemLotteryLineup in itemLotteryData.lineupList)
			{
				if (mstItemLotteryLineup.itemNum > 0)
				{
					list.Add(new ItemData(mstItemLotteryLineup.itemId, mstItemLotteryLineup.itemNum));
				}
			}
		}
		List<ItemData> dspLst = new List<ItemData>();
		dspLst.AddRange(DataManager.DmItem.GetUserDispItemList(list, DataManagerItem.DispType.Common));
		dspLst.AddRange(DataManager.DmItem.GetUserDispItemList(list, DataManagerItem.DispType.Decoration));
		dspLst.AddRange(DataManager.DmItem.GetUserDispItemList(list, DataManagerItem.DispType.Photo));
		dspLst.AddRange(DataManager.DmItem.GetUserDispItemList(list, DataManagerItem.DispType.Growth));
		dspLst.AddRange(DataManager.DmItem.GetUserDispItemList(list, DataManagerItem.DispType.PlayItem));
		list.RemoveAll((ItemData itm) => dspLst.Contains(itm));
		dspLst.AddRange(list);
		for (int i = 0; i < this.winPlayItem.Count; i++)
		{
			this.winPlayItem[i].transform.parent.gameObject.SetActive(i < dspLst.Count);
			if (i < dspLst.Count)
			{
				ItemData itemData = new ItemData(dspLst[i].id, dspLst[i].num);
				this.winPlayItem[i].Setup(itemData.staticData, itemData.num, new IconItemCtrl.SetupParam
				{
					useInfo = true,
					useMaxDetail = true
				});
			}
		}
	}

	private void SetupPicnicPlay(int index, GameObject go)
	{
		foreach (object obj in go.transform)
		{
			foreach (object obj2 in ((Transform)obj))
			{
				((Transform)obj2).SetParent(this.hidePanel, false);
			}
		}
		int num = index * ScenePicnic.ScrollPlayNum;
		for (int i = 0; i < ScenePicnic.ScrollPlayNum; i++)
		{
			int num2 = i + num;
			if (num2 > this.dispPlayPackList.Count)
			{
				break;
			}
			ScenePicnic.PlayPackData playPackData = ((num2 > 0) ? this.dispPlayPackList[num2 - 1] : null);
			string id = ((playPackData == null) ? "0" : playPackData.type.PlayId.ToString());
			Transform transform = this.picnicPlay.Find((Transform itm) => itm.name == id);
			if (transform != null)
			{
				transform.SetParent(go.transform.Find("Icon_Play" + (i + 1).ToString("D2")), false);
				transform = transform.Find("Bace/GetItem/Icon_GetItem");
				if (transform != null)
				{
					ItemStaticBase itemStaticBase = ((playPackData == null) ? null : DataManager.DmItem.GetItemStaticBase(playPackData.type.GetItemList[this.monthlyType].Item.itemId));
					transform.gameObject.SetActive(itemStaticBase != null);
					if (itemStaticBase != null)
					{
						transform.GetComponent<PguiRawImageCtrl>().SetRawImage(itemStaticBase.GetIconName(), true, false, null);
					}
				}
			}
		}
	}

	private void OnClickPicnicView(PguiButtonCtrl pbc)
	{
		if (this.stageLoad != null || this.itemGet != 0 || this.tutorial >= 0 || this.chgChr != null || this.chgPly != null)
		{
			return;
		}
		if (this.foodBuy != 0 || this.chargeIdx > 0 || this.staminaBuyCharge < 0)
		{
			return;
		}
		if (!this.winItemGet.FinishedClose() || !this.winStaminaCharge.FinishedClose())
		{
			return;
		}
		if (DataManager.IsServerRequesting() || !CanvasManager.HdlOpenWindowBasic.FinishedClose())
		{
			return;
		}
		if (this.camera == null || this.camPos == null)
		{
			return;
		}
		int num = this.camNo + 1;
		this.camNo = num;
		if (num >= this.camPos.Length)
		{
			this.camNo = 0;
		}
		this.camera.transform.position = this.camPos[this.camNo].transform.position;
		this.camera.transform.rotation = this.camPos[this.camNo].transform.rotation;
		this.camera.fieldOfView = this.camPos[this.camNo].fieldOfView;
	}

	private void OnClickPicnicHide(PguiButtonCtrl pbc)
	{
		if (this.stageLoad != null || this.itemGet != 0 || this.tutorial >= 0 || this.chgChr != null || this.chgPly != null)
		{
			return;
		}
		if (this.foodBuy != 0 || this.chargeIdx > 0 || this.staminaBuyCharge < 0)
		{
			return;
		}
		if (!this.winItemGet.FinishedClose() || !this.winStaminaCharge.FinishedClose())
		{
			return;
		}
		if (DataManager.IsServerRequesting() || !CanvasManager.HdlOpenWindowBasic.FinishedClose())
		{
			return;
		}
		if (this.hideMode)
		{
			return;
		}
		this.hideMode = true;
		this.basePanel.ExPlayAnimation(SimpleAnimation.ExPguiStatus.LOOP, null);
		CanvasManager.HdlCmnMenu.SetActiveMenu(false);
	}

	private void OnTap(Info info)
	{
		if (!this.hideMode)
		{
			return;
		}
		if (this.OnUiTap(info.CurrentPosition))
		{
			return;
		}
		this.hideMode = false;
		this.basePanel.ExPlayAnimation(SimpleAnimation.ExPguiStatus.LOOP_SUB, null);
		CanvasManager.HdlCmnMenu.SetActiveMenu(true);
	}

	private bool OnUiTap(Vector2 pos)
	{
		PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
		pointerEventData.position = pos;
		List<RaycastResult> list = new List<RaycastResult>();
		EventSystem.current.RaycastAll(pointerEventData, list);
		return list.Count > 0;
	}

	private bool IsMoveMotion(ScenePicnic.CharaCtrl c)
	{
		return c.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.H_HOM_MOV_ST) || c.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.H_HOM_MOV_LP) || c.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.H_HOM_MOV_EN) || (c.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.H_HOM_FLY_MOV_ST) || c.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.H_HOM_FLY_MOV_LP) || c.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.H_HOM_FLY_MOV_EN));
	}

	private void BallKick()
	{
		Vector3 zero = Vector3.zero;
		if (this.chrPos != null)
		{
			zero.x = this.chrPos.localPosition.x;
			zero.z = this.chrPos.localPosition.z;
		}
		if (this.ballEff == null)
		{
			this.ballEff = EffectManager.InstantiateEffect(ScenePicnic.ballEffName, this.field.transform, LayerMask.NameToLayer(ScenePicnic.charaLayer), 1f);
			this.ballEff.PlayEffect(false);
			this.ballEff.effectObject.transform.localPosition = zero + new Vector3(0f, -0.3f, 0f);
			this.ballEffNo = -1;
			this.ballEffSpd = Vector3.zero;
			this.ballEffRot = Vector3.zero;
			this.ballTime = 0f;
		}
		this.ballTime -= TimeManager.DeltaTime;
		this.ballEffSpd.y = this.ballEffSpd.y - TimeManager.DeltaTime * 9.8f;
		Vector3 vector = this.ballEff.effectObject.transform.localPosition;
		List<ScenePicnic.CharaCtrl> list = new List<ScenePicnic.CharaCtrl>();
		foreach (ScenePicnic.CharaCtrl charaCtrl in this.charaList)
		{
			if (charaCtrl.bf > 0 && charaCtrl.bf == charaCtrl.hid && !(charaCtrl.hdl == null) && charaCtrl.ctrl != null && charaCtrl.pos / 10 == 1 && !this.IsMoveMotion(charaCtrl))
			{
				list.Add(charaCtrl);
			}
		}
		ScenePicnic.CharaCtrl charaCtrl2 = list.Find((ScenePicnic.CharaCtrl itm) => itm.pos == this.ballEffNo);
		float num = ((charaCtrl2 == null) ? (-0.3f) : (charaCtrl2.fly ? 2f : 0.3f));
		vector += this.ballEffSpd * TimeManager.DeltaTime;
		if ((vector.y <= num && this.ballEffSpd.y < 0f) || this.stageCtrl == null || this.camera == null)
		{
			vector.y = num;
			if (list.Count < 2 || this.stageCtrl == null || this.camera == null)
			{
				charaCtrl2 = null;
			}
			else
			{
				if (charaCtrl2 != null)
				{
					list.Remove(charaCtrl2);
				}
				charaCtrl2 = ((list.Count > 0) ? list[Random.Range(0, list.Count)] : null);
			}
			if (charaCtrl2 == null)
			{
				this.ballEffNo = -1;
				this.ballEffSpd = zero + new Vector3(0f, -0.3f, 0f);
			}
			else
			{
				this.ballEffNo = charaCtrl2.pos;
				this.ballEffSpd = charaCtrl2.hdl.transform.TransformPoint(charaCtrl2.fly ? new Vector3(0f, 2f, 0.5f) : new Vector3(0f, 0.3f, 0.85f));
			}
			float num2 = Mathf.Sqrt((4.5f - num) / 4.9f);
			float num3 = Mathf.Sqrt((4.5f - this.ballEffSpd.y) / 4.9f);
			this.ballEffSpd = (this.ballEffSpd - vector) / (num2 + num3);
			this.ballEffSpd.y = ((this.ballEffNo < 0) ? 0f : (9.8f * num2));
			this.ballEffRot = ((this.ballEffSpd.magnitude < 0.1f) ? Vector3.zero : new Vector3(Random.Range(90f, 720f), Random.Range(90f, 720f), Random.Range(90f, 720f)));
			this.ballTime = ((charaCtrl2 == null) ? (-1f) : (num2 + num3 - (charaCtrl2.fly ? 1f : 1.1f)));
		}
		else if (charaCtrl2 != null && charaCtrl2.hdl.IsLoopAnimation() && this.ballTime <= 0f && this.ballTime > -0.2f)
		{
			charaCtrl2.hdl.PlayAnimation(CharaMotionDefine.ActKey.PIC_BALL_KICK, false, 1f, 0.2f, 0.1f, false);
			charaCtrl2.hdl.enabledFaceMotion = true;
		}
		this.ballEff.effectObject.transform.localPosition = vector;
		this.ballEff.effectObject.transform.localEulerAngles += this.ballEffRot * TimeManager.DeltaTime;
	}

	private void CatchBall()
	{
		Vector3 zero = Vector3.zero;
		if (this.chrPos != null)
		{
			zero.x = this.chrPos.localPosition.x;
			zero.z = this.chrPos.localPosition.z;
		}
		if (this.catchEff != null && this.catchEff.EffectName != this.catchName)
		{
			EffectManager.DestroyEffect(this.catchEff);
			this.catchEff = null;
		}
		if (this.catchEff == null)
		{
			this.catchEff = EffectManager.InstantiateEffect(this.catchName, this.field.transform, LayerMask.NameToLayer(ScenePicnic.charaLayer), 1f);
			this.catchEff.PlayEffect(false);
			this.catchEff.effectObject.transform.localPosition = zero + new Vector3(0f, -0.3f, 0f);
			this.catchEffNo = -1;
			this.catchEffReq = -1;
			this.catchEffSpd = Vector3.zero;
			this.catchEffRot = Vector3.zero;
			this.catchTime = 0f;
			this.catchCall = 0;
		}
		this.catchTime -= TimeManager.DeltaTime;
		this.catchEffSpd.y = this.catchEffSpd.y - TimeManager.DeltaTime * 9.8f;
		this.catchEffBone = null;
		Vector3 vector = this.catchEff.effectObject.transform.localPosition;
		if (this.stageCtrl == null || this.camera == null)
		{
			this.catchEffNo = -1;
			this.catchEffReq = -1;
			this.catchEffSpd = Vector3.zero;
			vector = zero + new Vector3(0f, -0.3f, 0f);
		}
		else
		{
			vector += this.catchEffSpd * TimeManager.DeltaTime;
			List<ScenePicnic.CharaCtrl> list = new List<ScenePicnic.CharaCtrl>();
			foreach (ScenePicnic.CharaCtrl charaCtrl in this.charaList)
			{
				if (charaCtrl.bf > 0 && charaCtrl.bf == charaCtrl.hid && !(charaCtrl.hdl == null) && charaCtrl.ctrl != null && charaCtrl.pos / 10 == 5 && !this.IsMoveMotion(charaCtrl))
				{
					list.Add(charaCtrl);
				}
			}
			ScenePicnic.CharaCtrl cc = list.Find((ScenePicnic.CharaCtrl itm) => itm.pos == this.catchEffNo);
			float num = ((cc == null) ? (-0.3f) : (cc.hand ? 0.8f : 1.2f));
			bool flag = false;
			if (cc != null && cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.PIC_CATCHBALL_THROW) && this.catchTime < -100f)
			{
				float motionTime = cc.hdl.GetMotionTime(true);
				this.catchEffBone = cc.hdl.GetNodeTransform("j_wrist_r");
				if (motionTime > 109f)
				{
					flag = true;
					vector = this.catchEff.effectObject.transform.localPosition;
				}
				else if (motionTime > 20f)
				{
					vector.y = num;
					this.catchEffSpd = new Vector3(0f, -100f, 0f);
					this.catchEffRot = Vector3.zero;
				}
				else if (vector.y <= num && this.catchEffSpd.y < 0f)
				{
					vector.y = num;
					this.catchEffSpd = new Vector3(0f, -100f, 0f);
					this.catchEffRot = Vector3.zero;
				}
				else
				{
					this.catchEffBone = null;
				}
			}
			else if (vector.y <= num && this.catchEffSpd.y < 0f)
			{
				flag = true;
				vector.y = num;
			}
			else if (cc != null && this.catchTime <= 0f && this.catchTime > -50f)
			{
				cc.hdl.PlayAnimation(cc.hand ? CharaMotionDefine.ActKey.PIC_CATCHBALL_THROW_NOHAND : CharaMotionDefine.ActKey.PIC_CATCHBALL_THROW, false, 1f, 0.2f, 0.1f, false);
				cc.hdl.enabledFaceMotion = true;
				if (cc.hand)
				{
					EffectData effectData = EffectManager.InstantiateEffect(ScenePicnic.spinEffName, cc.hdl.transform, cc.hdl.GetLayer(), 1f);
					effectData.effectObject.transform.localPosition = new Vector3(0f, -999f, 0f);
					effectData.PlayEffect(false);
					cc.hdl.SetEffect(effectData);
				}
				this.catchTime = -200f;
				list.Remove(cc);
				this.catchEffReq = ((list.Count > 0) ? list[Random.Range(0, list.Count)].pos : (-1));
				int num2 = this.catchCall + 1;
				this.catchCall = num2;
				if (num2 >= 1)
				{
					List<ScenePicnic.CharaCtrl> list2 = list.FindAll((ScenePicnic.CharaCtrl itm) => itm != cc && itm.hdl.IsLoopAnimation());
					if (list2.Count > 0)
					{
						this.catchCall = 0;
						int num3 = Random.Range(0, list2.Count);
						while (num3-- >= 0)
						{
							ScenePicnic.CharaCtrl charaCtrl2 = list2[Random.Range(0, list2.Count)];
							charaCtrl2.hdl.PlayAnimation(charaCtrl2.hand ? CharaMotionDefine.ActKey.PIC_CATCHBALL_CALL_NOHAND : CharaMotionDefine.ActKey.PIC_CATCHBALL_CALL, false, 1f, 0.2f, 0.1f, false);
							charaCtrl2.hdl.enabledFaceMotion = true;
						}
					}
				}
			}
			if (flag)
			{
				if (this.catchEffReq >= 0)
				{
					cc = list.Find((ScenePicnic.CharaCtrl itm) => itm.pos == this.catchEffReq);
				}
				else if (list.Count < 2)
				{
					cc = null;
				}
				else
				{
					if (cc != null)
					{
						list.Remove(cc);
					}
					cc = ((list.Count > 0) ? list[Random.Range(0, list.Count)] : null);
				}
				this.catchEffReq = -1;
				if (cc == null)
				{
					this.catchEffNo = -1;
					this.catchEffSpd = zero + new Vector3(0f, -0.3f, 0f);
				}
				else
				{
					this.catchEffNo = cc.pos;
					this.catchEffSpd = cc.hdl.transform.TransformPoint(cc.hand ? new Vector3(0f, 0.8f, 0.35f) : new Vector3(0f, 1.2f, 0.5f));
				}
				float num4 = Mathf.Sqrt((4.5f - vector.y) / 4.9f);
				float num5 = Mathf.Sqrt((4.5f - this.catchEffSpd.y) / 4.9f);
				this.catchEffSpd = (this.catchEffSpd - vector) / (num4 + num5);
				this.catchEffSpd.y = ((this.catchEffNo < 0) ? 0f : (9.8f * num4));
				this.catchEffRot = ((this.catchEffSpd.magnitude < 0.1f) ? Vector3.zero : new Vector3(Random.Range(90f, 720f), Random.Range(90f, 720f), Random.Range(90f, 720f)));
				this.catchTime = ((cc == null) ? (-1f) : (num4 + num5 - (cc.hand ? 0.9f : 0.66f)));
			}
		}
		this.catchEff.effectObject.transform.localPosition = vector;
		this.catchEff.effectObject.transform.localEulerAngles += this.catchEffRot * TimeManager.DeltaTime;
	}

	private void Badminton()
	{
		Vector3 zero = Vector3.zero;
		if (this.chrPos != null)
		{
			zero.x = this.chrPos.localPosition.x;
			zero.z = this.chrPos.localPosition.z;
		}
		if (this.shutleEff != null && this.shutleEff.EffectName != this.shutleName)
		{
			EffectManager.DestroyEffect(this.shutleEff);
			this.shutleEff = null;
		}
		foreach (int num in new List<int>(this.racketEff.Keys))
		{
			if (this.racketEff[num].EffectName != this.racketName)
			{
				EffectManager.DestroyEffect(this.racketEff[num]);
				this.racketEff[num] = null;
				this.racketEff.Remove(num);
			}
		}
		if (this.shutleEff == null)
		{
			this.shutleEff = EffectManager.InstantiateEffect(this.shutleName, this.field.transform, LayerMask.NameToLayer(ScenePicnic.charaLayer), 1f);
			this.shutleEff.PlayEffect(false);
			this.shutleEff.effectObject.transform.localPosition = zero + new Vector3(0f, -0.3f, 0f);
			this.badmintEffNo = -1;
			this.badmintEffSpd = Vector3.zero;
			this.badmintTime = 0f;
			this.badmintCall = 0;
		}
		List<int> list = new List<int>();
		this.badmintTime -= TimeManager.DeltaTime;
		this.badmintEffSpd.y = this.badmintEffSpd.y - TimeManager.DeltaTime * 9.8f;
		Vector3 vector = this.shutleEff.effectObject.transform.localPosition;
		if (this.stageCtrl == null || this.camera == null)
		{
			this.badmintEffNo = -1;
			this.badmintEffSpd = Vector3.zero;
			vector = zero + new Vector3(0f, -0.3f, 0f);
			list.AddRange(this.racketEff.Keys);
		}
		else
		{
			vector += this.badmintEffSpd * TimeManager.DeltaTime;
			List<ScenePicnic.CharaCtrl> list2 = new List<ScenePicnic.CharaCtrl>();
			foreach (ScenePicnic.CharaCtrl charaCtrl in this.charaList)
			{
				if (charaCtrl.bf > 0 && charaCtrl.bf == charaCtrl.hid && !(charaCtrl.hdl == null) && charaCtrl.ctrl != null && charaCtrl.pos / 10 == 6 && !this.IsMoveMotion(charaCtrl))
				{
					list2.Add(charaCtrl);
					if (!this.racketEff.ContainsKey(charaCtrl.pos))
					{
						EffectData effectData = EffectManager.InstantiateEffect(this.racketName, this.field.transform, LayerMask.NameToLayer(ScenePicnic.charaLayer), 1f);
						effectData.PlayEffect(false);
						this.racketEff.Add(charaCtrl.pos, effectData);
					}
				}
			}
			using (Dictionary<int, EffectData>.KeyCollection.Enumerator enumerator3 = this.racketEff.Keys.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					int key = enumerator3.Current;
					ScenePicnic.CharaCtrl charaCtrl2 = list2.Find((ScenePicnic.CharaCtrl itm) => itm.pos == key);
					if (charaCtrl2 == null || (charaCtrl2.hdl.GetNodeTransform("j_wrist_r") == null && charaCtrl2.hdl.GetNodeTransform("j_weapon_a") == null))
					{
						list.Add(key);
					}
				}
			}
			ScenePicnic.CharaCtrl cc = list2.Find((ScenePicnic.CharaCtrl itm) => itm.pos == this.badmintEffNo);
			float num2 = ((cc == null) ? (-0.3f) : (cc.hand ? 1.3f : 1f));
			if (vector.y <= num2 && this.badmintEffSpd.y < 0f)
			{
				vector.y = num2;
				if (list2.Count < 2)
				{
					cc = null;
				}
				else
				{
					if (cc != null)
					{
						list2.RemoveAll((ScenePicnic.CharaCtrl itm) => (itm.pos & 1) == (cc.pos & 1));
					}
					cc = ((list2.Count > 0) ? list2[Random.Range(0, list2.Count)] : null);
				}
				if (cc == null)
				{
					this.badmintEffNo = -1;
					this.badmintEffSpd = zero + new Vector3(0f, -0.3f, 0f);
				}
				else
				{
					this.badmintEffNo = cc.pos;
					this.badmintEffSpd = cc.hdl.transform.TransformPoint(cc.hand ? new Vector3(0.5f, 1.3f, 0.25f) : new Vector3(0.6f, 1f, 0.8f));
				}
				float num3 = Mathf.Sqrt((4.5f - num2) / 4.9f);
				float num4 = Mathf.Sqrt((4.5f - this.badmintEffSpd.y) / 4.9f);
				this.badmintEffSpd = (this.badmintEffSpd - vector) / (num3 + num4);
				this.badmintEffSpd.y = ((this.badmintEffNo < 0) ? 0f : (9.8f * num3));
				this.badmintTime = ((cc == null) ? (-1f) : (num3 + num4 - (cc.hand ? 0.5f : 1f)));
			}
			else if (cc != null && this.badmintTime <= 0f && this.badmintTime > -50f)
			{
				cc.hdl.PlayAnimation(cc.hand ? CharaMotionDefine.ActKey.PIC_BADMINTON_HIT_NOHAND : CharaMotionDefine.ActKey.PIC_BADMINTON_HIT, false, 1f, 0.2f, 0.1f, false);
				cc.hdl.enabledFaceMotion = true;
				if (cc.hand)
				{
					EffectData effectData2 = EffectManager.InstantiateEffect(ScenePicnic.spinEffName, cc.hdl.transform, cc.hdl.GetLayer(), 1f);
					effectData2.effectObject.transform.localPosition = new Vector3(0f, -999f, 0f);
					effectData2.PlayEffect(false);
					cc.hdl.SetEffect(effectData2);
				}
				this.badmintTime = -200f;
				int num5 = this.badmintCall + 1;
				this.badmintCall = num5;
				if (num5 >= 5)
				{
					List<ScenePicnic.CharaCtrl> list3 = list2.FindAll((ScenePicnic.CharaCtrl itm) => itm != cc && itm.hdl.IsLoopAnimation());
					if (list3.Count > 0)
					{
						this.badmintCall = 0;
						ScenePicnic.CharaCtrl charaCtrl3 = list3[Random.Range(0, list3.Count)];
						charaCtrl3.hdl.PlayAnimation(charaCtrl3.hand ? CharaMotionDefine.ActKey.PIC_BADMINTON_CALL_NOHAND : CharaMotionDefine.ActKey.PIC_BADMINTON_CALL, false, 1f, 0.2f, 0.1f, false);
						charaCtrl3.hdl.enabledFaceMotion = true;
					}
				}
			}
		}
		this.shutleEff.effectObject.transform.LookAt(vector);
		this.shutleEff.effectObject.transform.localPosition = vector;
		foreach (int num6 in list)
		{
			EffectManager.DestroyEffect(this.racketEff[num6]);
			this.racketEff[num6] = null;
			this.racketEff.Remove(num6);
		}
	}

	private void Kenkenpa()
	{
		Vector3 zero = Vector3.zero;
		float num = 0f;
		if (this.chrPos != null)
		{
			zero.x = this.chrPos.localPosition.x;
			zero.z = this.chrPos.localPosition.z;
			num = this.chrPos.eulerAngles.y;
		}
		if (this.kenpaEff != null && this.kenpaEff.EffectName != this.kenpaName)
		{
			EffectManager.DestroyEffect(this.kenpaEff);
			this.kenpaEff = null;
		}
		if (this.kenpaEff == null)
		{
			this.kenpaEff = EffectManager.InstantiateEffect(this.kenpaName, this.field.transform, LayerMask.NameToLayer(ScenePicnic.charaLayer), 1f);
			this.kenpaEff.PlayEffect(false);
			this.kenpaEff.effectObject.transform.localPosition = zero + new Vector3(0f, -10f, 0f);
			this.kenpaEff.effectObject.transform.localEulerAngles = new Vector3(0f, num, 0f);
		}
		Vector3 vector = zero;
		if (this.stageCtrl == null || this.camera == null)
		{
			vector.y = -10f;
		}
		else
		{
			List<ScenePicnic.CharaCtrl> list = new List<ScenePicnic.CharaCtrl>();
			foreach (ScenePicnic.CharaCtrl charaCtrl in this.charaList)
			{
				if (charaCtrl.bf > 0 && charaCtrl.bf == charaCtrl.hid && !(charaCtrl.hdl == null) && charaCtrl.ctrl != null && charaCtrl.pos / 10 == 7)
				{
					list.Add(charaCtrl);
				}
			}
			if (list.Count > 0)
			{
				vector.y = 0.01f;
			}
			else
			{
				vector.y = -10f;
			}
		}
		this.kenpaEff.effectObject.transform.localPosition = vector;
		this.kenpaEff.effectObject.transform.localEulerAngles = new Vector3(0f, num, 0f);
	}

	private void Balloon()
	{
		Vector3 zero = Vector3.zero;
		if (this.chrPos != null)
		{
			zero.x = this.chrPos.localPosition.x;
			zero.z = this.chrPos.localPosition.z;
		}
		Dictionary<ScenePicnic.CharaCtrl, Vector3> dictionary = new Dictionary<ScenePicnic.CharaCtrl, Vector3>();
		foreach (ScenePicnic.CharaCtrl charaCtrl in this.charaList)
		{
			if (charaCtrl.bf > 0 && charaCtrl.bf == charaCtrl.hid && !(charaCtrl.hdl == null) && charaCtrl.ctrl != null && charaCtrl.pos / 10 == 8)
			{
				Transform transform = (charaCtrl.hand ? null : charaCtrl.hdl.GetNodeTransform("j_wrist_r"));
				Vector3 vector = new Vector3(-0.06f, 0f, 0.02f);
				if (transform == null)
				{
					transform = charaCtrl.hdl.GetNodeTransform("pelvis");
					if (transform == null)
					{
						continue;
					}
					vector = new Vector3(0.3f, 0f, 0.1f);
				}
				dictionary.Add(charaCtrl, transform.TransformPoint(vector));
			}
		}
		List<ScenePicnic.CharaCtrl> list = new List<ScenePicnic.CharaCtrl>(dictionary.Keys);
		using (List<ScenePicnic.Balon>.Enumerator enumerator2 = this.balon.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				ScenePicnic.Balon bl = enumerator2.Current;
				if (bl.eff != null && (string.IsNullOrEmpty(bl.name) || bl.eff.EffectName != bl.name))
				{
					bl.tag = null;
					EffectManager.DestroyEffect(bl.eff);
					bl.eff = null;
				}
				if (bl.eff == null)
				{
					bl.no = -1;
					bl.time = 0f;
					if (string.IsNullOrEmpty(bl.name))
					{
						continue;
					}
					bl.eff = EffectManager.InstantiateEffect(bl.name, this.field.transform, LayerMask.NameToLayer(ScenePicnic.charaLayer), 1f);
					bl.eff.PlayEffect(false);
					bl.eff.effectObject.transform.localPosition = zero + new Vector3(0f, -10f, 0f);
					bl.tag = new GameObject("tag").transform;
					bl.tag.SetParent(bl.eff.effectObject.transform, false);
					bl.tag.localPosition = new Vector3(0f, 1f, 0f);
				}
				bl.time += TimeManager.DeltaTime;
				Vector3 vector2 = bl.eff.effectObject.transform.localPosition;
				if (this.stageCtrl == null || this.camera == null)
				{
					bl.no = -1;
					vector2 = zero + new Vector3(0f, -10f, 0f);
				}
				else
				{
					ScenePicnic.CharaCtrl charaCtrl2 = list.Find((ScenePicnic.CharaCtrl itm) => itm.no == bl.no);
					if (charaCtrl2 == null && list.Count > 0)
					{
						charaCtrl2 = list[Random.Range(0, list.Count)];
					}
					if (charaCtrl2 == null)
					{
						bl.no = -1;
						vector2 = zero + new Vector3(0f, -10f, 0f);
						bl.time = 0f;
					}
					else
					{
						bl.no = charaCtrl2.no;
						vector2 = dictionary[charaCtrl2];
						list.Remove(charaCtrl2);
					}
				}
				bl.eff.effectObject.transform.localPosition = vector2;
			}
		}
	}

	private void Hanetsuki()
	{
		Vector3 zero = Vector3.zero;
		if (this.chrPos != null)
		{
			zero.x = this.chrPos.localPosition.x;
			zero.z = this.chrPos.localPosition.z;
		}
		new List<int>(this.hagoEff.Keys);
		if (this.haneEff == null)
		{
			this.haneEff = EffectManager.InstantiateEffect(ScenePicnic.haneEffName, this.field.transform, LayerMask.NameToLayer(ScenePicnic.charaLayer), 1f);
			this.haneEff.PlayEffect(false);
			this.haneEff.effectObject.transform.localPosition = zero + new Vector3(0f, -0.3f, 0f);
			this.haneEffNo = -1;
			this.haneEffTyp = 0;
			this.haneEffSpd = Vector3.zero;
			this.haneTime = 0f;
			this.haneCall = 0;
		}
		List<int> list = new List<int>();
		this.haneTime -= TimeManager.DeltaTime;
		this.haneEffSpd.y = this.haneEffSpd.y - TimeManager.DeltaTime * 9.8f;
		Vector3 vector = this.haneEff.effectObject.transform.localPosition;
		if (this.stageCtrl == null || this.camera == null)
		{
			this.haneEffNo = -1;
			this.haneEffSpd = Vector3.zero;
			vector = zero + new Vector3(0f, -0.3f, 0f);
			list.AddRange(this.hagoEff.Keys);
		}
		else
		{
			vector += this.haneEffSpd * TimeManager.DeltaTime;
			List<ScenePicnic.CharaCtrl> list2 = new List<ScenePicnic.CharaCtrl>();
			foreach (ScenePicnic.CharaCtrl charaCtrl in this.charaList)
			{
				if (charaCtrl.bf > 0 && charaCtrl.bf == charaCtrl.hid && !(charaCtrl.hdl == null) && charaCtrl.ctrl != null && charaCtrl.pos / 10 == 9 && !this.IsMoveMotion(charaCtrl))
				{
					list2.Add(charaCtrl);
					if (!this.hagoEff.ContainsKey(charaCtrl.pos))
					{
						EffectData effectData = EffectManager.InstantiateEffect(ScenePicnic.hagoEffName, this.field.transform, LayerMask.NameToLayer(ScenePicnic.charaLayer), 1f);
						effectData.PlayEffect(false);
						this.hagoEff.Add(charaCtrl.pos, effectData);
					}
				}
			}
			using (Dictionary<int, EffectData>.KeyCollection.Enumerator enumerator2 = this.hagoEff.Keys.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					int key = enumerator2.Current;
					ScenePicnic.CharaCtrl charaCtrl2 = list2.Find((ScenePicnic.CharaCtrl itm) => itm.pos == key);
					if (charaCtrl2 == null || (charaCtrl2.hdl.GetNodeTransform("j_wrist_r") == null && charaCtrl2.hdl.GetNodeTransform("j_weapon_a") == null))
					{
						list.Add(key);
					}
				}
			}
			ScenePicnic.CharaCtrl cc = list2.Find((ScenePicnic.CharaCtrl itm) => itm.pos == this.haneEffNo);
			float num = 0.9f;
			if (cc == null)
			{
				num = -0.3f;
			}
			else if (cc.hand)
			{
				num = ((this.haneEffTyp == 2) ? 0.9f : 1.1f);
			}
			else if (this.haneEffTyp == 1)
			{
				num = 1.6f;
			}
			else if (this.haneEffTyp == 2)
			{
				num = 0.8f;
			}
			else if (this.haneEffTyp == 3)
			{
				num = 1.5f;
			}
			if (vector.y <= num && this.haneEffSpd.y < 0f)
			{
				vector.y = num;
				if (cc != null && this.haneEffTyp == 2)
				{
					this.haneEffTyp = 3;
					this.haneEffSpd = cc.hdl.transform.TransformPoint(cc.hand ? new Vector3(0.3f, 1.1f, 0.15f) : new Vector3(0.1f, 1.5f, 0.3f));
					float num2 = (cc.hand ? 2f : 1.2f);
					this.haneEffSpd = (this.haneEffSpd - vector) / num2;
					this.haneEffSpd.y = this.haneEffSpd.y + 5f * num2;
					if (cc.hand)
					{
						this.haneTime = num2 - 0.5f;
					}
					SoundManager.Play("prd_se_picnic_battledore", false, false);
				}
				else
				{
					if (list2.Count < 2)
					{
						cc = null;
					}
					else
					{
						if (cc != null)
						{
							list2.RemoveAll((ScenePicnic.CharaCtrl itm) => (itm.pos & 1) == (cc.pos & 1));
						}
						cc = ((list2.Count > 0) ? list2[Random.Range(0, list2.Count)] : null);
					}
					if (cc == null)
					{
						this.haneEffNo = -1;
						this.haneTime = -1f;
					}
					else
					{
						this.haneEffNo = cc.pos;
						this.haneEffTyp = Random.Range(0, 3);
						Vector3 vector2 = new Vector3(0.4f, 0.9f, 0.6f);
						if (cc.hand)
						{
							vector2 = ((this.haneEffTyp == 2) ? new Vector3(0f, 0.9f, 0.25f) : new Vector3(0.3f, 1.1f, 0.15f));
						}
						else if (this.haneEffTyp == 1)
						{
							vector2 = new Vector3(0f, 1.6f, 0.5f);
						}
						else if (this.haneEffTyp == 2)
						{
							vector2 = new Vector3(0.3f, 0.8f, 0.5f);
						}
						this.haneEffSpd = cc.hdl.transform.TransformPoint(vector2);
						float num3 = Mathf.Sqrt((4.5f - num) / 4.9f);
						float num4 = Mathf.Sqrt((4.5f - this.haneEffSpd.y) / 4.9f);
						this.haneEffSpd = (this.haneEffSpd - vector) / (num3 + num4);
						this.haneEffSpd.y = ((this.haneEffNo < 0) ? 0f : (9.8f * num3));
						float num5 = 1f;
						if (cc.hand)
						{
							num5 = 0.5f;
						}
						else if (this.haneEffTyp == 1)
						{
							num5 = 0.9f;
						}
						else if (this.haneEffTyp == 2)
						{
							num5 = 1.1f;
						}
						this.haneTime = num3 + num4 - num5;
						SoundManager.Play("prd_se_picnic_battledore", false, false);
					}
				}
			}
			else if (cc != null && this.haneTime <= 0f && this.haneTime > -50f)
			{
				CharaMotionDefine.ActKey actKey = CharaMotionDefine.ActKey.PIC_HANETSUKI_HIT_1;
				if (cc.hand)
				{
					actKey = CharaMotionDefine.ActKey.PIC_HANETSUKI_HIT_NOHAND;
				}
				else if (this.haneEffTyp == 1)
				{
					actKey = CharaMotionDefine.ActKey.PIC_HANETSUKI_HIT_2;
				}
				else if (this.haneEffTyp == 2)
				{
					actKey = CharaMotionDefine.ActKey.PIC_HANETSUKI_HIT_3;
				}
				cc.hdl.PlayAnimation(actKey, false, 1f, 0.2f, 0.1f, false);
				cc.hdl.enabledFaceMotion = true;
				if (cc.hand)
				{
					EffectData effectData2 = EffectManager.InstantiateEffect(ScenePicnic.spinEffName, cc.hdl.transform, cc.hdl.GetLayer(), 1f);
					effectData2.effectObject.transform.localPosition = new Vector3(0f, -999f, 0f);
					effectData2.PlayEffect(false);
					cc.hdl.SetEffect(effectData2);
				}
				this.haneTime = -200f;
				int num6 = this.haneCall + 1;
				this.haneCall = num6;
				if (num6 >= 5)
				{
					List<ScenePicnic.CharaCtrl> list3 = list2.FindAll((ScenePicnic.CharaCtrl itm) => itm != cc && itm.hdl.IsLoopAnimation());
					if (list3.Count > 0)
					{
						this.haneCall = 0;
						ScenePicnic.CharaCtrl charaCtrl3 = list3[Random.Range(0, list3.Count)];
						charaCtrl3.hdl.PlayAnimation(charaCtrl3.hand ? CharaMotionDefine.ActKey.PIC_HANETSUKI_CALL_NOHAND : CharaMotionDefine.ActKey.PIC_HANETSUKI_CALL, false, 1f, 0.2f, 0.1f, false);
						charaCtrl3.hdl.enabledFaceMotion = true;
					}
				}
			}
		}
		this.haneEff.effectObject.transform.LookAt(vector);
		this.haneEff.effectObject.transform.localPosition = vector;
		foreach (int num7 in list)
		{
			EffectManager.DestroyEffect(this.hagoEff[num7]);
			this.hagoEff[num7] = null;
			this.hagoEff.Remove(num7);
		}
	}

	private void Train()
	{
		while (this.trainEff.Count < 10)
		{
			EffectData effectData = EffectManager.InstantiateEffect((this.trainEff.Count < 2) ? ScenePicnic.train1EffName : ScenePicnic.train2EffName, this.field.transform, LayerMask.NameToLayer(ScenePicnic.charaLayer), 1f);
			effectData.PlayEffect(false);
			this.trainEff.Add(effectData);
		}
		if (this.trWhistleEff == null)
		{
			this.trWhistleEff = EffectManager.InstantiateEffect(ScenePicnic.trWhistleEffName, null, LayerMask.NameToLayer(ScenePicnic.charaLayer), 1f);
			this.trWhistleEff.PlayEffect(false);
		}
		int num = 0;
		this.trainChr = new List<ScenePicnic.CharaCtrl>();
		foreach (ScenePicnic.CharaCtrl charaCtrl in this.charaList)
		{
			if (charaCtrl.bf > 0)
			{
				num++;
				if (!(charaCtrl.hdl == null) && charaCtrl.ctrl != null && charaCtrl.pos / 10 == 10 && !this.IsMoveMotion(charaCtrl))
				{
					this.trainChr.Add(charaCtrl);
				}
			}
		}
		if (num != this.trainChr.Count)
		{
			this.trainChr = new List<ScenePicnic.CharaCtrl>();
		}
		this.trainChr.Sort((ScenePicnic.CharaCtrl a, ScenePicnic.CharaCtrl b) => a.pos - b.pos);
		Transform transform = ((this.trainChr.Count > 0) ? this.trainChr[0].hdl.GetNodeTransform("j_mouth") : this.field.transform);
		if (this.trWhistleEff.effectObject.transform.parent != transform)
		{
			this.trWhistleEff.effectObject.transform.SetParent(transform, false);
			this.trWhistleEff.effectObject.transform.localPosition = ((transform == this.field.transform) ? new Vector3(0f, -10f, 0f) : new Vector3(0f, 0f, 0.12f));
			this.trWhistleEff.effectObject.transform.localEulerAngles = new Vector3(0f, 180f, -90f);
		}
		foreach (ScenePicnic.TrNote trNote in this.trNote)
		{
			if (this.trainChr.Count <= 0)
			{
				trNote.chr = null;
			}
			if (trNote.eff == null)
			{
				if (trNote.chr != null && (trNote.tim -= TimeManager.DeltaTime) <= 0f)
				{
					trNote.eff = EffectManager.InstantiateEffect(ScenePicnic.trNoteEffName, this.field.transform, LayerMask.NameToLayer(ScenePicnic.charaLayer), 1f);
					trNote.eff.PlayEffect(false);
					trNote.eff.effectObject.transform.position = trNote.chr.hdl.GetNodeTransform("j_head").position;
				}
			}
			else if (trNote.eff.IsFinishByAnimation() || trNote.chr == null)
			{
				EffectManager.DestroyEffect(trNote.eff);
				trNote.eff = null;
				trNote.chr = null;
			}
			else
			{
				trNote.eff.effectObject.transform.position = trNote.chr.hdl.GetNodeTransform("j_head").position;
			}
		}
		this.trNote.RemoveAll((ScenePicnic.TrNote itm) => itm.chr == null && itm.eff == null);
		bool flag = false;
		if (this.trainChr.Count > 0)
		{
			CharaModelHandle hdl = this.trainChr[0].hdl;
			CharaMotionDefine.ActKey currentAnimation = hdl.GetCurrentAnimation();
			float animationTime = hdl.GetAnimationTime(currentAnimation.ToString());
			if (currentAnimation == CharaMotionDefine.ActKey.PIC_TRAIN_WALK_ST)
			{
				if ((this.trainSpd = (animationTime - 0.05f) / 0.95f) < 0f)
				{
					this.trainSpd = 0f;
				}
				flag = true;
			}
			else if (currentAnimation == CharaMotionDefine.ActKey.PIC_TRAIN_WALK_LP_1)
			{
				this.trainSpd = 1f + animationTime * 0.3f;
				flag = true;
			}
			else if (currentAnimation == CharaMotionDefine.ActKey.PIC_TRAIN_WALK_LP_2)
			{
				if ((this.trainSpd += TimeManager.DeltaTime * 0.8f) > 2f)
				{
					this.trainSpd = 2f;
				}
				flag = true;
			}
			else if (currentAnimation == CharaMotionDefine.ActKey.PIC_TRAIN_WALK_EN)
			{
				if ((this.trainSpd = (1f - animationTime * 1.1f) * 2f) < 0f)
				{
					this.trainSpd = 0f;
				}
				flag = true;
			}
			else
			{
				this.trainSpd = 0f;
				if (currentAnimation == CharaMotionDefine.ActKey.PIC_TRAIN_CALL)
				{
					flag = true;
				}
			}
		}
		else
		{
			this.trainSpd = 0f;
		}
		if ((this.trainRot -= ScenePicnic.trainSpeed * this.trainSpd * TimeManager.DeltaTime) < 0f)
		{
			this.trainRot += 360f;
		}
		this.trainDist = (ScenePicnic.trainDistMax - ScenePicnic.trainDistMin) * this.trainSpd * 0.5f;
		this.trainDist += ScenePicnic.trainDistMin;
		if (flag)
		{
			if (!this.trainSE)
			{
				this.trainSEhdl = SoundManager.Play("prd_se_picnic_whistle", false, false);
				this.trainSE = true;
				return;
			}
		}
		else if (this.trainSE)
		{
			this.trainSEhdl.Stop();
			this.trainSE = false;
		}
	}

	private void FireWork()
	{
		if (this.fwPos == null)
		{
			if (this.sheetEff.Count > 0)
			{
				foreach (EffectData effectData in this.sheetEff)
				{
					EffectManager.DestroyEffect(effectData);
				}
				this.sheetEff = new List<EffectData>();
			}
		}
		else
		{
			if (this.sheetEff.Count <= 0)
			{
				this.fwCnt = 0;
				this.fwTim = 2f;
			}
			int count = this.charaList.FindAll((ScenePicnic.CharaCtrl itm) => itm.hid > 0 && itm.hdl != null && itm.hdl.IsModelActive()).Count;
			this.sheetPos = new List<Vector3>();
			if (count > 3)
			{
				while (this.sheetEff.Count < 2)
				{
					EffectData effectData2 = EffectManager.InstantiateEffect(ScenePicnic.sheetEffName, this.field.transform, LayerMask.NameToLayer(ScenePicnic.charaLayer), 1f);
					effectData2.PlayEffect(false);
					this.sheetEff.Add(effectData2);
				}
				Vector3 vector = ((this.chrPos == null) ? Vector3.zero : this.chrPos.localPosition);
				this.sheetEff[0].effectObject.transform.localPosition = vector;
				this.sheetEff[0].effectObject.transform.LookAt(this.fwPos);
				this.sheetEff[0].effectObject.transform.Translate(1.8f, 0f, 0f);
				this.sheetEff[0].effectObject.transform.LookAt(this.fwPos);
				Vector3 vector2 = this.sheetEff[0].effectObject.transform.localScale;
				if (count < 5)
				{
					this.sheetPos.Add(this.sheetEff[0].effectObject.transform.TransformPoint(0.6f / vector2.x, 0f, 0f));
					this.sheetPos.Add(this.sheetEff[0].effectObject.transform.TransformPoint(-0.6f / vector2.x, 0f, 0f));
				}
				else
				{
					this.sheetPos.Add(this.sheetEff[0].effectObject.transform.TransformPoint(0.9f / vector2.x, 0f, 0.5f / vector2.z));
					this.sheetPos.Add(this.sheetEff[0].effectObject.transform.TransformPoint(0f, 0f, -0.8f / vector2.z));
					this.sheetPos.Add(this.sheetEff[0].effectObject.transform.TransformPoint(-0.9f / vector2.x, 0f, 0.5f / vector2.z));
				}
				this.sheetEff[1].effectObject.transform.localPosition = vector;
				this.sheetEff[1].effectObject.transform.LookAt(this.fwPos);
				this.sheetEff[1].effectObject.transform.Translate(-1.8f, 0f, 0f);
				this.sheetEff[1].effectObject.transform.LookAt(this.fwPos);
				vector2 = this.sheetEff[0].effectObject.transform.localScale;
				this.sheetPos.Add(this.sheetEff[1].effectObject.transform.TransformPoint(0.6f / vector2.x, 0f, 0f));
				this.sheetPos.Add(this.sheetEff[1].effectObject.transform.TransformPoint(-0.6f / vector2.x, 0f, 0f));
			}
			else if (count > 0)
			{
				while (this.sheetEff.Count > 1)
				{
					EffectManager.DestroyEffect(this.sheetEff[0]);
					this.sheetEff.RemoveAt(0);
				}
				if (this.sheetEff.Count < 1)
				{
					EffectData effectData3 = EffectManager.InstantiateEffect(ScenePicnic.sheetEffName, this.field.transform, LayerMask.NameToLayer(ScenePicnic.charaLayer), 1f);
					effectData3.PlayEffect(false);
					this.sheetEff.Add(effectData3);
				}
				this.sheetEff[0].effectObject.transform.localPosition = ((this.chrPos == null) ? Vector3.zero : this.chrPos.localPosition);
				this.sheetEff[0].effectObject.transform.LookAt(this.fwPos);
				Vector3 localScale = this.sheetEff[0].effectObject.transform.localScale;
				if (count < 2)
				{
					this.sheetPos.Add(this.sheetEff[0].effectObject.transform.TransformPoint(0f, 0f, 0f));
				}
				else if (count < 3)
				{
					this.sheetPos.Add(this.sheetEff[0].effectObject.transform.TransformPoint(0.6f / localScale.x, 0f, 0f));
					this.sheetPos.Add(this.sheetEff[0].effectObject.transform.TransformPoint(-0.6f / localScale.x, 0f, 0f));
				}
				else
				{
					this.sheetPos.Add(this.sheetEff[0].effectObject.transform.TransformPoint(0.9f / localScale.x, 0f, 0.5f / localScale.z));
					this.sheetPos.Add(this.sheetEff[0].effectObject.transform.TransformPoint(0f, 0f, -0.8f / localScale.z));
					this.sheetPos.Add(this.sheetEff[0].effectObject.transform.TransformPoint(-0.9f / localScale.x, 0f, 0.5f / localScale.z));
				}
			}
			else if (this.sheetEff.Count > 0)
			{
				foreach (EffectData effectData4 in this.sheetEff)
				{
					EffectManager.DestroyEffect(effectData4);
				}
				this.sheetEff = new List<EffectData>();
			}
			if ((this.fwTim -= TimeManager.DeltaTime) <= 0f)
			{
				this.fwTim = 1f + Random.Range(0f, 1.5f);
				ScenePicnic.FirWrk firWrk = new ScenePicnic.FirWrk();
				firWrk.time = 1.5f;
				string text = ScenePicnic.fwSmlEffName[Random.Range(0, ScenePicnic.fwSmlEffName.Count)];
				string text2 = "prd_se_picnic_fireworks_normal";
				ScenePicnic.FirWrk firWrk2 = firWrk;
				int num = this.fwCnt + 1;
				this.fwCnt = num;
				if ((firWrk2.no = num) > 6)
				{
					this.fwTim = 2.5f + Random.Range(0f, 0.5f);
					this.fwCnt = 0;
					text = this.fwEffName[Random.Range(0, this.fwEffName.Count)];
					text2 = "prd_se_picnic_fireworks_pattern";
					firWrk.time = 2f;
				}
				firWrk.eff = EffectManager.InstantiateEffect(text, this.field.transform, LayerMask.NameToLayer(ScenePicnic.charaLayer), 1f);
				firWrk.eff.PlayEffect(false);
				firWrk.eff.effectObject.transform.position = this.fwPos.position;
				firWrk.eff.effectObject.transform.rotation = this.fwPos.rotation;
				firWrk.eff.effectObject.transform.localScale = this.fwPos.localScale;
				this.fwEff.Add(firWrk);
				SoundManager.Play(text2, false, false);
			}
		}
		int num2 = 0;
		List<ScenePicnic.FirWrk> rmv2 = new List<ScenePicnic.FirWrk>();
		foreach (ScenePicnic.FirWrk firWrk3 in this.fwEff)
		{
			if (this.fwPos == null || firWrk3.eff.IsFinishByAnimation())
			{
				EffectManager.DestroyEffect(firWrk3.eff);
				rmv2.Add(firWrk3);
			}
			else if (firWrk3.time > 0f && (firWrk3.time -= TimeManager.DeltaTime) <= 0f)
			{
				if (firWrk3.no > 6)
				{
					num2 = 2;
				}
				else if (num2 < 2 && (firWrk3.no & 1) != 0)
				{
					num2 = 1;
				}
			}
		}
		this.fwEff.RemoveAll((ScenePicnic.FirWrk itm) => rmv2.Contains(itm));
		List<ScenePicnic.CharaCtrl> list = new List<ScenePicnic.CharaCtrl>();
		foreach (ScenePicnic.CharaCtrl charaCtrl in this.charaList)
		{
			if (charaCtrl.bf > 0 && charaCtrl.bf == charaCtrl.hid && !(charaCtrl.hdl == null) && charaCtrl.ctrl != null && charaCtrl.pos / 10 == 11 && charaCtrl.hdl.IsLoopAnimation() && charaCtrl.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.PIC_SITTING_DOWN))
			{
				list.Add(charaCtrl);
				if (charaCtrl.fw > 0f && (charaCtrl.fw -= TimeManager.DeltaTime) <= 0f)
				{
					charaCtrl.hdl.SetFacePackData(FacePackData.Id2PackData("FACE_SMILE_1_A"), null, null);
				}
			}
		}
		if (num2 > 0)
		{
			int num3 = 2 + Random.Range(0, 2);
			if (num2 > 1)
			{
				num3 = list.Count;
			}
			else
			{
				if (num3 > list.Count)
				{
					num3 = list.Count;
				}
				if (num3 < 2)
				{
					num3 = 0;
				}
			}
			while (num3-- > 0)
			{
				ScenePicnic.CharaCtrl charaCtrl2 = list[Random.Range(0, list.Count)];
				int num4 = ((num2 < 2) ? 0 : Random.Range(1, ScenePicnic.emoEffName.Count));
				EffectData effectData5 = EffectManager.InstantiateEffect(ScenePicnic.emoEffName[num4], this.field.transform, LayerMask.NameToLayer(ScenePicnic.charaLayer), 1f);
				effectData5.PlayEffect(false);
				effectData5.effectObject.transform.position = charaCtrl2.hdl.GetNodePos("j_head");
				effectData5.effectObject.transform.eulerAngles = Vector3.zero;
				effectData5.effectObject.transform.localScale = Vector3.one;
				this.emoEff.Add(effectData5);
				List<string> list2 = ScenePicnic.emoFaceName[num4];
				charaCtrl2.hdl.enabledFaceMotion = false;
				charaCtrl2.hdl.SetFacePackData(FacePackData.Id2PackData(list2[Random.Range(0, list2.Count)]), null, null);
				charaCtrl2.fw = 1f + Random.Range(0f, 0.5f);
				list.Remove(charaCtrl2);
			}
		}
		List<EffectData> rmv = new List<EffectData>();
		foreach (EffectData effectData6 in this.emoEff)
		{
			if (this.fwPos == null || effectData6.IsFinishByAnimation())
			{
				EffectManager.DestroyEffect(effectData6);
				rmv.Add(effectData6);
			}
		}
		this.emoEff.RemoveAll((EffectData itm) => rmv.Contains(itm));
	}

	private void Snow()
	{
		if (this.snPos == null)
		{
			if (this.snowEff != null)
			{
				EffectManager.DestroyEffect(this.snowEff);
			}
			this.snowEff = null;
			if (this.snowShadowEff != null)
			{
				EffectManager.DestroyEffect(this.snowShadowEff);
			}
			this.snowShadowEff = null;
			return;
		}
		List<ScenePicnic.CharaCtrl> list = new List<ScenePicnic.CharaCtrl>();
		foreach (ScenePicnic.CharaCtrl charaCtrl in this.charaList)
		{
			if (charaCtrl.bf > 0 && charaCtrl.bf == charaCtrl.hid && !(charaCtrl.hdl == null) && charaCtrl.ctrl != null && charaCtrl.pos / 10 == 12 && charaCtrl.pos % 10 >= 4 && !charaCtrl.hdl.IsLoopAnimation() && (ScenePicnic.snowBall1.Contains(charaCtrl.hdl.GetCurrentAnimation()) || ScenePicnic.snowBall2.Contains(charaCtrl.hdl.GetCurrentAnimation())))
			{
				list.Add(charaCtrl);
			}
		}
		if (this.snowEff == null)
		{
			this.snowEff = EffectManager.InstantiateEffect(this.snowEffName, this.field.transform, LayerMask.NameToLayer(ScenePicnic.charaLayer), 1f);
			this.snowEff.PlayEffect(false);
			this.snowEff.effectObject.transform.position = this.snPos.position;
			this.snowEff.effectObject.transform.eulerAngles = Vector3.zero;
			this.snowEff.effectObject.transform.localScale = Vector3.one;
		}
		if (this.snowShadowEff == null)
		{
			this.snowShadowEff = EffectManager.InstantiateEffect(ScenePicnic.snowShadowPath, this.field.transform, LayerMask.NameToLayer(ScenePicnic.charaLayer), 1f);
			this.snowShadowEff.PlayEffect(false);
			this.snowShadowEff.effectObject.transform.position = this.snPos.position;
			this.snowShadowEff.effectObject.transform.eulerAngles = Vector3.zero;
			this.snowShadowEff.effectObject.transform.localScale = Vector3.one;
		}
		float num = this.snowBallSiz * 0.5f;
		this.snowEff.effectObject.transform.position = ((list.Count > 0) ? list[0].hdl.transform.TransformPoint(0f, num, 0.3f + num) : new Vector3(0f, -10f, 0f));
		this.snowEff.effectObject.transform.eulerAngles = new Vector3(this.snowBallAng, (list.Count > 0) ? list[0].hdl.transform.eulerAngles.y : 0f, 0f);
		this.snowEff.effectObject.transform.localScale = Vector3.one * num / 0.7f;
		this.snowShadowEff.effectObject.transform.position = ((list.Count > 0) ? list[0].hdl.transform.TransformPoint(0f, 0.01f, 1f) : new Vector3(0f, -10f, 0f));
		num /= 0.4f;
		this.snowShadowEff.effectObject.transform.localScale = new Vector3(num, 1f, num);
	}

	private IEnumerator charaStay(ScenePicnic.CharaCtrl cc)
	{
		cc.mov = 0;
		cc.hdl.eyeFollowObj = (cc.hdl.headFollowObj = null);
		ScenePicnic.<>c__DisplayClass251_0 CS$<>8__locals1;
		int num13;
		for (;;)
		{
			CS$<>8__locals1 = new ScenePicnic.<>c__DisplayClass251_0();
			if (this.stageCtrl == null || this.camera == null)
			{
				cc.hdl.PlayAnimation(cc.fly ? CharaMotionDefine.ActKey.H_HOM_FLY_WAIT0 : CharaMotionDefine.ActKey.H_HOM_WAIT0, true, 1f, 0f, 0f, false);
				cc.hdl.enabledFaceMotion = true;
				while (this.stageCtrl == null || this.camera == null)
				{
					yield return null;
				}
				cc.pos = this.ChkPos(cc.pos, cc.no);
				this.SetCharaStay(cc);
			}
			CS$<>8__locals1.ps = cc.pos / 10;
			bool flag = this.charaList.Find((ScenePicnic.CharaCtrl chk) => chk.bf > 0 && chk.bf == chk.hid && chk.hdl != null && (chk.mov != 0 || !chk.hdl.IsLoopAnimation())) != null;
			if (CS$<>8__locals1.ps == 2 && !flag && !cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.PIC_HANAICHIMONME))
			{
				if ((cc.pos & 1) != 0)
				{
					if (this.charaList.Find((ScenePicnic.CharaCtrl itm) => itm.hid > 0 && itm.hdl != null && itm.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.PIC_HANAICHIMONME) && itm.hdl.GetAnimationTime(CharaMotionDefine.ActKey.PIC_HANAICHIMONME.ToString()) >= 0.5f) == null)
					{
						goto IL_1504;
					}
				}
				cc.hdl.PlayAnimation(CharaMotionDefine.ActKey.PIC_HANAICHIMONME, true, 1f, 0.5f, 0.25f, false);
				cc.hdl.enabledFaceMotion = true;
			}
			else if (CS$<>8__locals1.ps == 3 && cc.hdl.IsLoopAnimation() && !cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.PIC_SITTING_DOWN))
			{
				cc.hdl.PlayAnimation(CharaMotionDefine.ActKey.PIC_SITTING_DOWN, true, 1f, 0.5f, 0.25f, false);
				cc.hdl.enabledFaceMotion = true;
			}
			else if (CS$<>8__locals1.ps == 1 && ((!cc.hdl.IsLoopAnimation() && !cc.hdl.IsPlaying()) || (cc.fly && cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.H_HOM_WAIT0))))
			{
				cc.hdl.PlayAnimation(cc.fly ? CharaMotionDefine.ActKey.H_HOM_FLY_WAIT0 : CharaMotionDefine.ActKey.H_HOM_WAIT0, true, 1f, 0.5f, 0.25f, false);
				cc.hdl.enabledFaceMotion = true;
			}
			else if (CS$<>8__locals1.ps == 5 && !cc.hdl.IsLoopAnimation() && !cc.hdl.IsPlaying())
			{
				cc.hdl.PlayAnimation(cc.hand ? CharaMotionDefine.ActKey.PIC_CATCHBALL_WAIT_NOHAND : CharaMotionDefine.ActKey.PIC_CATCHBALL_WAIT, true, 1f, 0.5f, 0.25f, false);
				cc.hdl.enabledFaceMotion = true;
			}
			else if (CS$<>8__locals1.ps == 6 && !cc.hdl.IsLoopAnimation() && !cc.hdl.IsPlaying())
			{
				cc.hdl.PlayAnimation(cc.hand ? CharaMotionDefine.ActKey.PIC_BADMINTON_WAIT_NOHAND : CharaMotionDefine.ActKey.PIC_BADMINTON_WAIT, true, 1f, 0.5f, 0.25f, false);
				cc.hdl.enabledFaceMotion = true;
			}
			else if (CS$<>8__locals1.ps == 7)
			{
				if (cc.pos % 10 == 0)
				{
					if (cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.PIC_KENKENPA_LP))
					{
						if (cc.hdl.GetAnimationTime(null) * cc.hdl.GetAnimationLength(null) * 30f >= 260f)
						{
							cc.hdl.PlayAnimation(CharaMotionDefine.ActKey.PIC_KENKENPA_ED, false, 1f, 0.5f, 0.25f, false);
							Transform nodeTransform = cc.hdl.GetNodeTransform("pelvis");
							if (nodeTransform != null)
							{
								cc.hdl.SetPosition(new Vector3(nodeTransform.position.x, 0f, nodeTransform.position.z));
								cc.hdl.SetRotation(nodeTransform.eulerAngles.y);
							}
						}
						else
						{
							Vector3 vector;
							Vector3 vector2;
							this.TargetPos(cc.pos, cc.hdl.transform.position, out vector, out vector2);
							cc.hdl.transform.position = vector;
							cc.hdl.transform.LookAt(vector2);
							Transform nodeTransform2 = cc.hdl.GetNodeTransform("root");
							Transform nodeTransform3 = cc.hdl.GetNodeTransform("pelvis");
							if (nodeTransform2 != null && nodeTransform3 != null)
							{
								Vector3 vector3 = nodeTransform3.position - nodeTransform2.position;
								vector3 = vector3 / nodeTransform2.localScale.z - vector3;
								vector3.y = 0f;
								cc.hdl.transform.position += vector3;
							}
						}
					}
					else if (cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.PIC_KENKENPA_ED))
					{
						if (!cc.hdl.IsPlaying())
						{
							cc.hdl.PlayAnimation(CharaMotionDefine.ActKey.PIC_KENKENPA_JOY, false, 1f, 0.5f, 0.25f, false);
						}
					}
					else if (cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.PIC_KENKENPA_JOY))
					{
						if (!cc.hdl.IsPlaying())
						{
							break;
						}
					}
					else
					{
						cc.hdl.PlayAnimation(CharaMotionDefine.ActKey.PIC_KENKENPA_LP, false, 1f, 0.5f, 0.25f, false);
						cc.hdl.enabledFaceMotion = true;
					}
					if (this.ChgChara(cc))
					{
						goto Block_37;
					}
					yield return null;
					continue;
				}
				else if (cc.pos % 10 == 1)
				{
					if (cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.PIC_KENKENPA_JOY))
					{
						if (!cc.hdl.IsPlaying())
						{
							goto Block_40;
						}
					}
					else
					{
						ScenePicnic.CharaCtrl charaCtrl = this.charaList.Find((ScenePicnic.CharaCtrl itm) => itm.pos == CS$<>8__locals1.ps * 10);
						if (charaCtrl != null)
						{
							Vector3 haraPos = charaCtrl.hdl.GetHaraPos();
							haraPos.y = 0f;
							cc.hdl.transform.LookAt(haraPos);
							if (charaCtrl.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.PIC_KENKENPA_JOY))
							{
								cc.hdl.PlayAnimation(CharaMotionDefine.ActKey.PIC_KENKENPA_JOY, false, 1f, 0.5f, 0.25f, false);
								cc.hdl.enabledFaceMotion = true;
							}
						}
					}
					if (this.ChgChara(cc))
					{
						goto Block_43;
					}
					yield return null;
					continue;
				}
				else
				{
					cc.pos = 0;
				}
			}
			else if (CS$<>8__locals1.ps == 9 && !cc.hdl.IsLoopAnimation() && !cc.hdl.IsPlaying())
			{
				cc.hdl.PlayAnimation(cc.hand ? CharaMotionDefine.ActKey.PIC_HANETSUKI_WAIT_NOHAND : CharaMotionDefine.ActKey.PIC_HANETSUKI_WAIT, true, 1f, 0.5f, 0.25f, false);
				cc.hdl.enabledFaceMotion = true;
			}
			else if (CS$<>8__locals1.ps == 10)
			{
				int num = cc.pos % 10;
				bool flag2 = true;
				new List<ScenePicnic.CharaCtrl>();
				foreach (ScenePicnic.CharaCtrl charaCtrl2 in this.charaList)
				{
					if (charaCtrl2.bf > 0 && (charaCtrl2.hdl == null || charaCtrl2.ctrl == null || charaCtrl2.pos / 10 != 10 || this.IsMoveMotion(charaCtrl2)))
					{
						flag2 = false;
					}
				}
				if (flag2)
				{
					if (cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.PIC_TRAIN_WAIT))
					{
						if (num > 0)
						{
							if (this.trainStep > 1)
							{
								cc.hdl.PlayAnimation(CharaMotionDefine.ActKey.PIC_TRAIN_WALK_ST, false, 1f, 0.1f, 0.05f, false);
								cc.hdl.enabledFaceMotion = true;
							}
						}
						else if (!cc.hdl.IsPlaying())
						{
							this.trainStep = 1;
							cc.hdl.PlayAnimation(CharaMotionDefine.ActKey.PIC_TRAIN_CALL, false, 1f, -1f, -1f, false);
							cc.hdl.enabledFaceMotion = true;
							this.trNote.Add(new ScenePicnic.TrNote
							{
								chr = cc,
								eff = null,
								tim = 1f
							});
						}
					}
					else if (cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.PIC_TRAIN_CALL))
					{
						if (!cc.hdl.IsPlaying())
						{
							this.trainStep = 2;
							cc.hdl.PlayAnimation(CharaMotionDefine.ActKey.PIC_TRAIN_WALK_ST, false, 1f, -1f, -1f, false);
							cc.hdl.enabledFaceMotion = true;
							this.trNote.Add(new ScenePicnic.TrNote
							{
								chr = cc,
								eff = null,
								tim = 0f
							});
						}
					}
					else if (cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.PIC_TRAIN_WALK_ST))
					{
						if (!cc.hdl.IsPlaying())
						{
							cc.hdl.PlayAnimation(CharaMotionDefine.ActKey.PIC_TRAIN_WALK_LP_1, num > 0, 1f, -1f, -1f, false);
							cc.hdl.enabledFaceMotion = true;
							float num2 = cc.hdl.GetEnableAnimationList()[0].GetState(CharaMotionDefine.ActKey.PIC_TRAIN_WALK_LP_1.ToString()).clip.length / 1.15f * 0.5f;
							this.trNote.Add(new ScenePicnic.TrNote
							{
								chr = cc,
								eff = null,
								tim = Random.Range(0f, num2)
							});
							this.trNote.Add(new ScenePicnic.TrNote
							{
								chr = cc,
								eff = null,
								tim = Random.Range(0f, num2) + num2
							});
						}
					}
					else if (cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.PIC_TRAIN_WALK_LP_1))
					{
						cc.hdl.SetAnimationSpeed(this.trainSpd);
						if (!cc.hdl.IsPlaying() || this.trainStep > 2)
						{
							if (num == 0)
							{
								this.trainStep = 3;
								this.trainLpCnt = 0;
							}
							cc.hdl.PlayAnimation(CharaMotionDefine.ActKey.PIC_TRAIN_WALK_LP_2, num > 0, 1f, 0.1f, 0.05f, false);
							cc.hdl.enabledFaceMotion = true;
							float length = cc.hdl.GetEnableAnimationList()[0].GetState(CharaMotionDefine.ActKey.PIC_TRAIN_WALK_LP_2.ToString()).clip.length;
							for (int i = 0; i < ScenePicnic.trainLpNum; i++)
							{
								this.trNote.Add(new ScenePicnic.TrNote
								{
									chr = cc,
									eff = null,
									tim = Random.Range(0f, length) + (float)i * length
								});
							}
						}
					}
					else if (cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.PIC_TRAIN_WALK_LP_2))
					{
						if (!cc.hdl.IsPlaying() || this.trainStep < 0)
						{
							if (num == 0)
							{
								int num3 = this.trainLpCnt + 1;
								this.trainLpCnt = num3;
								if (num3 >= ScenePicnic.trainLpNum)
								{
									this.trainStep = -1;
								}
							}
							cc.hdl.PlayAnimation((this.trainStep < 0) ? CharaMotionDefine.ActKey.PIC_TRAIN_WALK_EN : CharaMotionDefine.ActKey.PIC_TRAIN_WALK_LP_2, false, 1f, 0.1f, 0.05f, false);
							cc.hdl.enabledFaceMotion = true;
						}
					}
					else if (cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.PIC_TRAIN_WALK_EN))
					{
						if (!cc.hdl.IsPlaying())
						{
							if (num == 0)
							{
								this.trainStep = 0;
							}
							cc.hdl.PlayAnimation(CharaMotionDefine.ActKey.PIC_TRAIN_WAIT, num > 0, 1f, -1f, -1f, false);
							cc.hdl.enabledFaceMotion = true;
						}
					}
					else if (num == 0)
					{
						this.trainStep = 1;
						cc.hdl.PlayAnimation(CharaMotionDefine.ActKey.PIC_TRAIN_CALL, false, 1f, 0.3f, 0.15f, false);
						cc.hdl.enabledFaceMotion = true;
						this.trNote.Add(new ScenePicnic.TrNote
						{
							chr = cc,
							eff = null,
							tim = 1f
						});
					}
					else
					{
						cc.hdl.PlayAnimation(CharaMotionDefine.ActKey.PIC_TRAIN_WAIT, true, 1f, 0.3f, 0.15f, false);
						cc.hdl.enabledFaceMotion = true;
					}
				}
				else
				{
					if (num == 0)
					{
						this.trainStep = 0;
					}
					if (!cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.H_HOM_WAIT0))
					{
						cc.hdl.PlayAnimation(CharaMotionDefine.ActKey.H_HOM_WAIT0, true, 1f, 0.3f, 0.15f, false);
					}
				}
			}
			else if (CS$<>8__locals1.ps == 11 && cc.hdl.IsLoopAnimation() && !cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.PIC_SITTING_DOWN))
			{
				cc.hdl.PlayAnimation(CharaMotionDefine.ActKey.PIC_SITTING_DOWN, true, 1f, -1f, -1f, false);
				cc.hdl.enabledFaceMotion = true;
			}
			else if (CS$<>8__locals1.ps == 12)
			{
				int num4 = cc.pos % 10;
				if (num4 < 2)
				{
					List<CharaMotionDefine.ActKey> list = ((num4 == 0) ? ScenePicnic.snowMan1 : ScenePicnic.snowMan2);
					CharaMotionDefine.ActKey currentAnimation = cc.hdl.GetCurrentAnimation();
					int num5 = list.IndexOf(currentAnimation);
					if (num5 < 0)
					{
						num5 = 0;
					}
					else if (cc.hdl.IsPlaying())
					{
						num5 = -1;
					}
					else if (++num5 >= list.Count)
					{
						num5 = 0;
					}
					if (num5 >= 0)
					{
						cc.hdl.PlayAnimation(list[num5], false, 1f, -1f, -1f, false);
						cc.hdl.enabledFaceMotion = true;
					}
				}
				else if (num4 < 4)
				{
					CharaMotionDefine.ActKey actKey = CharaMotionDefine.ActKey.PIC_SITTING_DOWN;
					if (!cc.hdl.IsLoopAnimation() || !cc.hdl.IsCurrentAnimation(actKey))
					{
						cc.hdl.PlayAnimation(actKey, true, 1f, -1f, -1f, false);
						cc.hdl.enabledFaceMotion = true;
					}
				}
				else
				{
					CharaMotionDefine.ActKey currentAnimation2 = cc.hdl.GetCurrentAnimation();
					if (ScenePicnic.snowBall1.Contains(currentAnimation2))
					{
						float num6 = 0.75f;
						float num7 = num6 * TimeManager.DeltaTime;
						if (currentAnimation2 == CharaMotionDefine.ActKey.PIC_SNOWROLING_EN)
						{
							if ((this.snowBallSpd -= num7) < 0f)
							{
								this.snowBallSpd = 0f;
							}
							this.snowBallSiz = ScenePicnic.snowBallSizMax;
						}
						else
						{
							if ((this.snowBallSpd += num7) > num6)
							{
								this.snowBallSpd = num6;
							}
							if (currentAnimation2 == CharaMotionDefine.ActKey.PIC_SNOWROLING_LP)
							{
								float num8 = ScenePicnic.snowBallSizMax - ScenePicnic.snowBallSizMin;
								float num9 = cc.hdl.GetAnimationLength(null) * (float)ScenePicnic.snowBallLpNum;
								if ((this.snowBallSiz += TimeManager.DeltaTime * num8 / num9) > ScenePicnic.snowBallSizMax)
								{
									this.snowBallSiz = ScenePicnic.snowBallSizMax;
								}
							}
						}
						num7 = this.snowBallSpd * TimeManager.DeltaTime * 360f;
						this.snowBallRot += num7 / (ScenePicnic.snowBallCircle * 2f * 3.1415927f);
						this.snowBallAng += num7 / (this.snowBallSiz * 3.1415927f);
					}
					else if (currentAnimation2 == CharaMotionDefine.ActKey.PIC_HARDING_LP)
					{
						float[,] array = new float[,]
						{
							{ 1.3f, 3.5f },
							{ 5.2f, 7.8f },
							{ 9.4f, 11.5f },
							{ 17.9f, 19.8f },
							{ 9999f, 0f }
						};
						float num10 = cc.hdl.GetAnimationTime(null) * cc.hdl.GetAnimationLength(null);
						float num11 = 0f;
						for (int j = 0; j < array.GetLength(0); j++)
						{
							num11 = (float)j;
							if (num10 < array[j, 0])
							{
								break;
							}
							if (num10 <= array[j, 1])
							{
								num11 += (num10 - array[j, 0]) / (array[j, 1] - array[j, 0]);
								break;
							}
						}
						num11 = 1f - num11 / (float)(array.GetLength(0) - 1);
						this.snowBallSiz = ScenePicnic.snowBallSizMin + (ScenePicnic.snowBallSizMax - ScenePicnic.snowBallSizMin) * num11;
					}
					else
					{
						this.snowBallSiz = ((currentAnimation2 == CharaMotionDefine.ActKey.PIC_HARDING_ST) ? ScenePicnic.snowBallSizMax : ScenePicnic.snowBallSizMin);
					}
					List<CharaMotionDefine.ActKey> list2 = new List<CharaMotionDefine.ActKey>(ScenePicnic.snowBall1);
					list2.AddRange(ScenePicnic.snowBall2);
					int num12 = list2.IndexOf(currentAnimation2);
					if (num12 < 0)
					{
						num12 = 0;
					}
					else if (cc.hdl.IsPlaying())
					{
						num12 = -1;
					}
					else
					{
						if (list2[num12] == CharaMotionDefine.ActKey.PIC_SNOWROLING_LP)
						{
							int num3 = this.snowBallLpCnt + 1;
							this.snowBallLpCnt = num3;
							if (num3 < ScenePicnic.snowBallLpNum)
							{
								goto IL_1457;
							}
						}
						if (++num12 >= list2.Count)
						{
							num12 = 0;
						}
					}
					IL_1457:
					if (num12 >= 0)
					{
						cc.hdl.PlayAnimation(list2[num12], false, 1f, -1f, -1f, false);
						cc.hdl.enabledFaceMotion = true;
						if (currentAnimation2 == CharaMotionDefine.ActKey.PIC_SNOWROLING_ST)
						{
							this.snowBallLpCnt = 0;
						}
					}
				}
			}
			else if (!cc.hdl.IsLoopAnimation() && !cc.hdl.IsPlaying())
			{
				cc.hdl.PlayAnimation(CharaMotionDefine.ActKey.H_HOM_WAIT0, true, 1f, 0.5f, 0.25f, false);
				cc.hdl.enabledFaceMotion = true;
			}
			IL_1504:
			Transform transform = null;
			if (CS$<>8__locals1.ps == 1 && this.ballEff != null && this.ballEff.effectObject.transform.localPosition.y > 0.01f)
			{
				transform = this.ballEff.effectObject.transform;
			}
			else if (CS$<>8__locals1.ps == 5 && this.catchEff != null && this.catchEff.effectObject.transform.localPosition.y > 0.01f)
			{
				transform = this.catchEff.effectObject.transform;
			}
			else if (CS$<>8__locals1.ps == 6 && this.shutleEff != null && this.shutleEff.effectObject.transform.localPosition.y > 0.01f)
			{
				transform = this.shutleEff.effectObject.transform;
			}
			else if (CS$<>8__locals1.ps == 9 && this.haneEff != null && this.haneEff.effectObject.transform.localPosition.y > 0.01f)
			{
				transform = this.haneEff.effectObject.transform;
			}
			cc.hdl.eyeFollowObj = (cc.hdl.headFollowObj = transform);
			if (this.ChgChara(cc))
			{
				goto Block_116;
			}
			num13 = this.ChkPos((CS$<>8__locals1.ps == 4 || CS$<>8__locals1.ps == 8) ? 0 : cc.pos, cc.no);
			if (num13 != cc.pos)
			{
				goto IL_1874;
			}
			Vector3 vector4;
			Vector3 vector5;
			this.TargetPos(cc.pos, cc.hdl.transform.position, out vector4, out vector5);
			cc.hdl.transform.position = vector4;
			cc.hdl.transform.LookAt(vector5);
			if (cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.PIC_CATCHBALL_THROW))
			{
				ScenePicnic.CharaCtrl charaCtrl3 = this.charaList.Find((ScenePicnic.CharaCtrl itm) => itm.pos == this.catchEffNo);
				if (charaCtrl3 == cc)
				{
					charaCtrl3 = this.charaList.Find((ScenePicnic.CharaCtrl itm) => itm.pos == this.catchEffReq);
				}
				if (charaCtrl3 != null)
				{
					float num14 = cc.hdl.GetMotionTime(true);
					if (num14 < 85f)
					{
						num14 = 0f;
					}
					else if (num14 < 103f)
					{
						num14 = (num14 - 85f) / 18f;
					}
					else if (num14 < 162f)
					{
						num14 = 1f;
					}
					else if (num14 < 178f)
					{
						num14 = (178f - num14) / 16f;
					}
					else
					{
						num14 = 0f;
					}
					float y = cc.hdl.transform.eulerAngles.y;
					cc.hdl.transform.LookAt(charaCtrl3.hdl.transform.position);
					cc.hdl.transform.eulerAngles = new Vector3(0f, y + Mathf.DeltaAngle(y, cc.hdl.transform.eulerAngles.y) * num14, 0f);
				}
			}
			yield return null;
			CS$<>8__locals1 = null;
		}
		cc.pos = CS$<>8__locals1.ps * 10 + 9;
		cc.ctrl = this.charaIn(cc);
		cc.ctrl.MoveNext();
		yield break;
		Block_37:
		yield break;
		Block_40:
		cc.pos = CS$<>8__locals1.ps * 10 + 9;
		cc.ctrl = this.charaIn(cc);
		cc.ctrl.MoveNext();
		yield break;
		Block_43:
		yield break;
		Block_116:
		yield break;
		IL_1874:
		cc.pos = num13;
		cc.ctrl = this.charaIn(cc);
		cc.ctrl.MoveNext();
		yield break;
		yield break;
	}

	private IEnumerator charaIn(ScenePicnic.CharaCtrl cc)
	{
		if (!cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.H_HOM_FLY_MOV_LP) && !cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.H_HOM_MOV_LP) && !cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.PIC_BALLOON_WALK) && !cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.PIC_BALLOON_LOOK))
		{
			bool flag = cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.PIC_KENKENPA) || cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.PIC_KENKENPA_LP) || cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.PIC_HANAICHIMONME);
			if (this.balon.Find((ScenePicnic.Balon itm) => itm.no == cc.no) != null)
			{
				cc.hdl.PlayAnimation(CharaMotionDefine.ActKey.PIC_BALLOON_WALK, true, 1f, 0.2f, 0.1f, false);
			}
			else
			{
				cc.hdl.PlayAnimation(cc.fly ? CharaMotionDefine.ActKey.H_HOM_FLY_MOV_ST : CharaMotionDefine.ActKey.H_HOM_MOV_ST, false, 1f, 0.2f, 0.1f, false);
			}
			cc.hdl.enabledFaceMotion = true;
			if (flag)
			{
				Vector3 haraPos = cc.hdl.GetHaraPos();
				cc.hdl.SetPosition(new Vector3(haraPos.x, 0f, haraPos.z));
			}
		}
		cc.mov = 1;
		cc.hdl.eyeFollowObj = (cc.hdl.headFollowObj = null);
		Predicate<ScenePicnic.Balon> <>9__1;
		for (;;)
		{
			if (this.stageCtrl == null || this.camera == null)
			{
				goto IL_1160;
			}
			if (this.ChgChara(cc))
			{
				break;
			}
			this.SetCharaMove(cc);
			cc.pos = this.ChkPos(cc.pos, cc.no);
			int ps = cc.pos / 10;
			if (ps >= 11)
			{
				goto Block_13;
			}
			Vector3 cp;
			Vector3 vector;
			this.TargetPos(cc.pos, cc.hdl.transform.position, out cp, out vector);
			float num = Vector3.Distance(cc.hdl.transform.position, cp);
			float num2 = TimeManager.DeltaTime;
			List<ScenePicnic.Balon> list = this.balon;
			Predicate<ScenePicnic.Balon> predicate;
			if ((predicate = <>9__1) == null)
			{
				predicate = (<>9__1 = (ScenePicnic.Balon itm) => itm.no == cc.no);
			}
			ScenePicnic.Balon balon = list.Find(predicate);
			if (ps == 8 && balon != null)
			{
				cc.hdl.eyeFollowObj = (cc.hdl.headFollowObj = null);
				num2 *= 0.7f;
				if (balon.time > 8f && cc.hdl.GetMotionTime(true) < 5f && num > 1f)
				{
					balon.time = -8.75f;
					cc.hdl.PlayAnimation(CharaMotionDefine.ActKey.PIC_BALLOON_LOOK, false, 0.8f, 0f, 0f, false);
					cc.hdl.enabledFaceMotion = true;
				}
			}
			if (num > num2)
			{
				bool flag2 = false;
				if (cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.PIC_BALLOON_LOOK))
				{
					num2 = 0f;
				}
				else if (!cc.hdl.IsLoopAnimation())
				{
					CharaMotionDefine.ActKey actKey = (cc.fly ? CharaMotionDefine.ActKey.H_HOM_FLY_MOV_ST : CharaMotionDefine.ActKey.H_HOM_MOV_ST);
					if (cc.hdl.IsCurrentAnimation(actKey))
					{
						num2 *= cc.hdl.GetAnimationTime(actKey.ToString());
					}
					else
					{
						actKey = (cc.fly ? CharaMotionDefine.ActKey.H_HOM_FLY_MOV_EN : CharaMotionDefine.ActKey.H_HOM_MOV_EN);
						if (cc.hdl.IsCurrentAnimation(actKey))
						{
							num2 *= 1f - cc.hdl.GetAnimationTime(actKey.ToString());
						}
					}
				}
				else if (cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.H_HOM_FLY_MOV_LP) || cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.H_HOM_MOV_LP))
				{
					flag2 = true;
				}
				float num3 = 1f;
				float y = cc.hdl.transform.eulerAngles.y;
				foreach (ScenePicnic.CharaCtrl charaCtrl in this.charaList)
				{
					if (charaCtrl != cc && charaCtrl.bf > 0 && !(charaCtrl.hdl == null) && charaCtrl.hdl.IsFinishInitialize() && charaCtrl.pos / 10 != 10)
					{
						Vector3 haraPos2 = charaCtrl.hdl.GetHaraPos();
						Vector3 vector2 = haraPos2 - cc.hdl.transform.position;
						vector2.y = 0f;
						float num4 = vector2.magnitude / 2f;
						if (num4 > 0.15f && num3 > num4)
						{
							cc.hdl.transform.LookAt(haraPos2);
							if (Mathf.Abs(Mathf.DeltaAngle(y, cc.hdl.transform.eulerAngles.y)) < (1f - num4) * 45f)
							{
								num3 = num4;
							}
						}
					}
				}
				num2 *= num3;
				if (ps == 8 && balon == null)
				{
					ScenePicnic.Balon tag = null;
					float num5 = 99999f;
					foreach (ScenePicnic.Balon balon2 in this.balon)
					{
						if (!(balon2.tag == null))
						{
							Vector3 position = balon2.tag.position;
							position.y = 0f;
							float num6 = Vector3.Distance(position, cc.hdl.transform.position);
							if (num5 >= num6 && num6 >= 0.3f && num6 <= 10f)
							{
								cc.hdl.transform.LookAt(position);
								if (Mathf.Abs(Mathf.DeltaAngle(y, cc.hdl.transform.eulerAngles.y)) <= 45f - num6)
								{
									tag = balon2;
									num5 = num6;
								}
							}
						}
					}
					cc.hdl.eyeFollowObj = (cc.hdl.headFollowObj = ((tag == null) ? null : tag.tag));
					bool flag3 = false;
					if (tag != null)
					{
						ScenePicnic.CharaCtrl charaCtrl2 = this.charaList.Find((ScenePicnic.CharaCtrl itm) => itm.no == tag.no);
						if (num5 < (flag2 ? 3f : 3.5f) && charaCtrl2 != null && !charaCtrl2.hdl.IsLoopAnimation())
						{
							flag3 = true;
						}
						num2 *= 0.6f + 0.03f * num5;
					}
					if (flag3)
					{
						if (cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.H_HOM_WAIT0))
						{
							num2 = 0f;
						}
						else
						{
							CharaMotionDefine.ActKey actKey2 = (cc.fly ? CharaMotionDefine.ActKey.H_HOM_FLY_MOV_EN : CharaMotionDefine.ActKey.H_HOM_MOV_EN);
							if (!cc.hdl.IsCurrentAnimation(actKey2) && cc.hdl.GetMotionTime(true) < 5f)
							{
								cc.hdl.PlayAnimation(actKey2, false, 1f, 0.2f, 0.1f, false);
								cc.hdl.enabledFaceMotion = true;
							}
						}
					}
					else if (!flag2)
					{
						CharaMotionDefine.ActKey actKey3 = (cc.fly ? CharaMotionDefine.ActKey.H_HOM_FLY_MOV_ST : CharaMotionDefine.ActKey.H_HOM_MOV_ST);
						if (!cc.hdl.IsCurrentAnimation(actKey3))
						{
							cc.hdl.PlayAnimation(actKey3, false, 1f, 0.2f, 0.1f, false);
							cc.hdl.enabledFaceMotion = true;
						}
					}
				}
				else if (ps == 7 && cc.pos % 10 >= 2)
				{
					ScenePicnic.CharaCtrl charaCtrl3 = this.charaList.Find((ScenePicnic.CharaCtrl itm) => itm.pos == ps * 10);
					if (charaCtrl3 != null && charaCtrl3.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.PIC_KENKENPA_LP))
					{
						Vector3 zero = Vector3.zero;
						float num7 = 0f;
						if (this.chrPos != null)
						{
							zero.x = this.chrPos.position.x;
							zero.z = this.chrPos.position.z;
							num7 = this.chrPos.eulerAngles.y;
						}
						Matrix4x4 identity = Matrix4x4.identity;
						identity.SetTRS(zero, Quaternion.Euler(0f, num7, 0f), Vector3.one);
						Vector3 vector3 = identity.MultiplyPoint3x4(new Vector3(0f, 0f, 5f)) - zero;
						float num8 = Vector3.Dot(cc.hdl.transform.position - zero, vector3);
						if (num8 >= 0f)
						{
							num8 = (zero - cc.hdl.transform.position + vector3 * num8 / vector3.sqrMagnitude).magnitude;
						}
						if (num8 > 0.75f && num8 < 1.25f && num3 > 0.35f)
						{
							cc.pos = ps * 10 + 1;
						}
					}
				}
				cc.hdl.transform.position = Vector3.Lerp(cc.hdl.transform.position, cp, num2 / num);
				cc.hdl.transform.LookAt(cp);
				yield return null;
			}
			else
			{
				if (ps != 4 && ps != 8 && (ps != 7 || cc.pos % 10 < 2))
				{
					goto IL_0F41;
				}
				if (ps == 7 && cc.pos % 10 == 9 && !cc.hdl.IsLoopAnimation() && cc.hdl.IsPlaying())
				{
					cc.hdl.transform.Translate(0f, 0f, num2 * cc.hdl.GetAnimationTime(null));
					yield return null;
					continue;
				}
				cc.hdl.eyeFollowObj = (cc.hdl.headFollowObj = null);
				cc.pos = this.ChkPos(0, cc.no);
				this.TargetPos(cc.pos, cc.hdl.transform.position, out cp, out vector);
				bool b;
				do
				{
					b = false;
					if (cc.hdl.IsLoopAnimation())
					{
						float num9 = cc.hdl.transform.eulerAngles.y;
						cc.hdl.transform.LookAt(cp);
						float num10 = Mathf.DeltaAngle(num9, cc.hdl.transform.eulerAngles.y);
						float num11 = num2 * 180f;
						num9 += Mathf.Clamp(num10, -num11, num11);
						cc.hdl.transform.eulerAngles = new Vector3(0f, num9, 0f);
						cc.hdl.transform.Translate(0f, 0f, num2);
						if (Mathf.Abs(num10) <= num11)
						{
							b = true;
						}
					}
					else
					{
						this.SetCharaMove(cc);
					}
					yield return null;
					num2 = TimeManager.DeltaTime;
					if (this.stageCtrl == null || this.camera == null)
					{
						break;
					}
				}
				while (!b);
			}
			cp = default(Vector3);
		}
		yield break;
		Block_13:
		this.SetCharaStay(cc);
		goto IL_1160;
		IL_0F41:
		cc.hdl.PlayAnimation(cc.fly ? CharaMotionDefine.ActKey.H_HOM_FLY_MOV_EN : CharaMotionDefine.ActKey.H_HOM_MOV_EN, false, 1f, 0.2f, 0.1f, false);
		cc.hdl.enabledFaceMotion = true;
		Vector3 pp = cc.hdl.transform.position;
		float aa = cc.hdl.transform.eulerAngles.y;
		for (;;)
		{
			yield return null;
			if (this.stageCtrl == null || this.camera == null)
			{
				goto IL_1160;
			}
			if (!cc.hdl.IsPlaying())
			{
				break;
			}
			Vector3 cp;
			Vector3 vector;
			this.TargetPos(cc.pos, cc.hdl.transform.position, out cp, out vector);
			cc.hdl.transform.position = Vector3.Lerp(pp, cp, cc.hdl.GetAnimationTime(null));
			cc.hdl.transform.LookAt(vector);
			float num12 = Mathf.DeltaAngle(aa, cc.hdl.transform.eulerAngles.y) * cc.hdl.GetAnimationTime(null);
			cc.hdl.transform.eulerAngles = new Vector3(0f, aa + num12, 0f);
		}
		this.SetCharaStay(cc);
		IL_1160:
		cc.ctrl = this.charaStay(cc);
		cc.ctrl.MoveNext();
		yield break;
	}

	private IEnumerator charaOut(ScenePicnic.CharaCtrl cc)
	{
		if (cc.pos / 10 >= 11)
		{
			float tim = 5f;
			while (this.gameType >= 11 && this.stageCtrl != null && this.camera != null && (tim -= TimeManager.DeltaTime) > 0f)
			{
				yield return null;
			}
			cc.ctrl = null;
			yield break;
		}
		if (cc.bf != cc.hid)
		{
			float tim = 5f;
			while (cc.bf != cc.hid && this.stageCtrl != null && this.camera != null && (tim -= TimeManager.DeltaTime) > 0f)
			{
				yield return null;
			}
			cc.ctrl = null;
			yield break;
		}
		cc.mov = -1;
		if (!cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.H_HOM_FLY_MOV_LP) && !cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.H_HOM_MOV_LP) && !cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.PIC_BALLOON_WALK) && !cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.PIC_BALLOON_LOOK))
		{
			bool flag = cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.PIC_KENKENPA) || cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.PIC_KENKENPA_LP) || cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.PIC_HANAICHIMONME);
			if (this.balon.Find((ScenePicnic.Balon itm) => itm.no == cc.no) != null)
			{
				cc.hdl.PlayAnimation(CharaMotionDefine.ActKey.PIC_BALLOON_WALK, true, 1f, 0.2f, 0.1f, false);
			}
			else
			{
				cc.hdl.PlayAnimation(cc.fly ? CharaMotionDefine.ActKey.H_HOM_FLY_MOV_ST : CharaMotionDefine.ActKey.H_HOM_MOV_ST, false, 1f, 0.2f, 0.1f, false);
			}
			cc.hdl.enabledFaceMotion = true;
			if (flag)
			{
				Vector3 haraPos = cc.hdl.GetHaraPos();
				cc.hdl.SetPosition(new Vector3(haraPos.x, 0f, haraPos.z));
			}
		}
		int oldCam = this.camNo;
		Vector3 iop = this.InOutPos(cc.pos);
		Vector3 vector = cc.hdl.transform.position;
		vector.y = 0f;
		cc.hdl.transform.position = vector;
		Vector3 cp = ((this.camera == null) ? vector : this.camera.transform.InverseTransformPoint(cc.hdl.transform.position));
		while (!(this.stageCtrl == null) && !(this.camera == null))
		{
			if (cc.bf == cc.af)
			{
				if (cc.bf > 0 && cc.bf == cc.hid && this.energy > 0)
				{
					if (cc.pos / 10 == 0)
					{
						cc.pos = this.ChkPos(0, cc.no);
					}
					cc.ctrl = this.charaIn(cc);
					cc.ctrl.MoveNext();
					yield break;
				}
				if (cc.bf <= 0)
				{
					cc.pos = 0;
				}
			}
			if (oldCam != this.camNo)
			{
				vector = this.camera.transform.TransformPoint(cp);
				vector.y = 0f;
				cc.hdl.transform.position = vector;
				oldCam = this.camNo;
			}
			vector = this.InOutPos(cc.pos);
			if (Vector3.Distance(iop, vector) > 1f)
			{
				iop = vector;
			}
			float num = Vector3.Distance(cc.hdl.transform.position, iop);
			cc.hdl.eyeFollowObj = (cc.hdl.headFollowObj = ((num < 5f) ? this.camera.transform : null));
			float num2 = TimeManager.DeltaTime;
			if (num <= num2)
			{
				break;
			}
			if (!cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.PIC_BALLOON_LOOK))
			{
				if (!cc.hdl.IsLoopAnimation())
				{
					CharaMotionDefine.ActKey actKey = (cc.fly ? CharaMotionDefine.ActKey.H_HOM_FLY_MOV_ST : CharaMotionDefine.ActKey.H_HOM_MOV_ST);
					num2 *= cc.hdl.GetAnimationTime(actKey.ToString());
				}
				cc.hdl.transform.position = Vector3.Lerp(cc.hdl.transform.position, iop, num2 / num);
				cc.hdl.transform.LookAt(iop);
			}
			this.SetCharaMove(cc);
			cp = this.camera.transform.InverseTransformPoint(cc.hdl.transform.position);
			yield return null;
		}
		cc.hdl.eyeFollowObj = (cc.hdl.headFollowObj = null);
		cc.ctrl = null;
		yield break;
	}

	private void SetCharaStay(ScenePicnic.CharaCtrl cc)
	{
		Vector3 vector;
		Vector3 vector2;
		this.TargetPos(cc.pos, cc.hdl.transform.position, out vector, out vector2);
		cc.hdl.transform.position = vector;
		int num = cc.pos / 10;
		CharaMotionDefine.ActKey actKey = CharaMotionDefine.ActKey.H_HOM_WAIT0;
		if (num == 5)
		{
			actKey = (cc.hand ? CharaMotionDefine.ActKey.PIC_CATCHBALL_WAIT_NOHAND : CharaMotionDefine.ActKey.PIC_CATCHBALL_WAIT);
		}
		else if (num == 6)
		{
			actKey = (cc.hand ? CharaMotionDefine.ActKey.PIC_BADMINTON_WAIT_NOHAND : CharaMotionDefine.ActKey.PIC_BADMINTON_WAIT);
		}
		else if (num == 9)
		{
			actKey = (cc.hand ? CharaMotionDefine.ActKey.PIC_HANETSUKI_WAIT_NOHAND : CharaMotionDefine.ActKey.PIC_HANETSUKI_WAIT);
		}
		cc.hdl.PlayAnimation(actKey, true, 1f, 0.5f, 0.25f, false);
		cc.hdl.enabledFaceMotion = true;
		cc.hdl.transform.LookAt(vector2);
	}

	private void SetCharaMove(ScenePicnic.CharaCtrl cc)
	{
		bool flag = !cc.hdl.IsPlaying();
		bool flag2 = this.balon.Find((ScenePicnic.Balon itm) => itm.no == cc.no) != null;
		if (flag2 ^ (cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.PIC_BALLOON_WALK) || cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.PIC_BALLOON_LOOK)))
		{
			flag = true;
		}
		if (flag)
		{
			bool flag3 = cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.PIC_BALLOON_LOOK) || cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.PIC_HANAICHIMONME) || cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.PIC_KENKENPA) || cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.PIC_KENKENPA_LP);
			CharaMotionDefine.ActKey actKey = (flag2 ? CharaMotionDefine.ActKey.PIC_BALLOON_WALK : (cc.fly ? CharaMotionDefine.ActKey.H_HOM_FLY_MOV_LP : CharaMotionDefine.ActKey.H_HOM_MOV_LP));
			if (cc.pos / 10 == 8 && !flag2 && (cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.H_HOM_FLY_MOV_EN) || cc.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.H_HOM_MOV_EN)))
			{
				actKey = CharaMotionDefine.ActKey.H_HOM_WAIT0;
			}
			cc.hdl.PlayAnimation(actKey, true, 1f, 0.2f, 0.1f, false);
			cc.hdl.enabledFaceMotion = true;
			if (flag3)
			{
				Vector3 haraPos = cc.hdl.GetHaraPos();
				cc.hdl.SetPosition(new Vector3(haraPos.x, 0f, haraPos.z));
			}
		}
	}

	private int ChkPos(int p, int no)
	{
		int num = this.gameType * 10;
		if (this.gameType == 4 || this.gameType == 8)
		{
			if (p >= num && p < num + 10)
			{
				num = p;
			}
			else
			{
				List<int> list = new List<int>();
				for (int i = 0; i < 10; i++)
				{
					list.Add(num + i);
				}
				foreach (ScenePicnic.CharaCtrl charaCtrl in this.charaList)
				{
					list.Remove(charaCtrl.pos);
				}
				if (list.Count > 0)
				{
					num = list[Random.Range(0, list.Count)];
				}
			}
		}
		else if (this.gameType == 7)
		{
			if (p >= num && p < num + 10)
			{
				num = p;
			}
			else
			{
				List<int> list2 = new List<int>();
				for (int j = 2; j < 9; j++)
				{
					list2.Add(num + j);
				}
				bool flag = true;
				foreach (ScenePicnic.CharaCtrl charaCtrl2 in this.charaList)
				{
					if (charaCtrl2.pos == num && !charaCtrl2.hdl.IsCurrentAnimation(CharaMotionDefine.ActKey.PIC_KENKENPA_JOY))
					{
						flag = false;
					}
					list2.Remove(charaCtrl2.pos);
				}
				if (!flag)
				{
					num = ((list2.Count > 0) ? list2[Random.Range(0, list2.Count)] : (num + 9));
				}
			}
		}
		else if (this.gameType == 10)
		{
			List<ScenePicnic.CharaCtrl> list3 = this.charaList.FindAll((ScenePicnic.CharaCtrl itm) => itm.bf > 0 && itm.bf == itm.hid);
			if (p / 10 == this.gameType)
			{
				num = ((list3.Find((ScenePicnic.CharaCtrl itm) => itm.no != no && itm.pos == p) == null) ? p : 0);
			}
			else
			{
				if ((p = list3.FindIndex((ScenePicnic.CharaCtrl itm) => itm.no == no)) < 0)
				{
					p = 0;
				}
				num += p;
			}
		}
		else if (this.gameType == 12)
		{
			List<ScenePicnic.CharaCtrl> list4 = this.charaList.FindAll((ScenePicnic.CharaCtrl itm) => itm.bf > 0 && itm.bf == itm.hid);
			if (p / 10 == this.gameType)
			{
				num = ((list4.Find((ScenePicnic.CharaCtrl itm) => itm.no != no && itm.pos == p) == null) ? p : 0);
			}
			else
			{
				List<int> list5 = new List<int> { 0, 1, 2, 3, 4 };
				if (list4.Count % 2 == 1)
				{
					int num2 = list4.FindLastIndex((ScenePicnic.CharaCtrl itm) => !itm.hand);
					if (num2 >= 0)
					{
						list5[list4.Count - 1] = list5[num2];
						list5[num2] = 4;
					}
				}
				int num3 = list4.FindIndex((ScenePicnic.CharaCtrl itm) => itm.no == no);
				num = ((num3 < 0) ? 0 : (num + list5[num3]));
			}
		}
		else if (p != num)
		{
			int k = 0;
			while (k < this.charaList.Count)
			{
				if (num == this.charaList[k++].pos)
				{
					if (++num == p)
					{
						break;
					}
					k = 0;
				}
			}
		}
		return num;
	}

	private void TargetPos(int no, Vector3 np, out Vector3 cp, out Vector3 tp)
	{
		int num = 0;
		foreach (ScenePicnic.CharaCtrl charaCtrl in this.charaList)
		{
			if (charaCtrl.bf > 0 && charaCtrl.bf == charaCtrl.hid)
			{
				num++;
			}
		}
		Vector3 zero = Vector3.zero;
		float num2 = 0f;
		if (this.chrPos != null)
		{
			zero.x = this.chrPos.position.x;
			zero.z = this.chrPos.position.z;
			num2 = this.chrPos.eulerAngles.y;
		}
		cp = zero;
		tp = zero;
		int typ = no / 10;
		no %= 10;
		if (typ == 1 || typ == 3 || typ == 5)
		{
			int num3 = 0;
			if (num > 2)
			{
				num3 = 180 / num;
			}
			else if (num > 1)
			{
				num3 = 45;
			}
			int num4 = no / 2 * 2 + 1;
			num3 *= num4;
			if ((no & 1) != 0)
			{
				num3 = -num3;
			}
			float num5 = 0.017453292f * ((float)num3 + num2);
			float num6 = Mathf.Sin(num5);
			float num7 = Mathf.Cos(num5);
			float num8 = ((typ == 1 || typ == 5) ? 5f : 1f);
			if (num < 2)
			{
				tp += new Vector3(num6, 0f, -num7) * num8;
				return;
			}
			cp += new Vector3(num6, 0f, -num7) * num8;
			if (num < 3)
			{
				float num9 = 0.017453292f * (num2 - (float)num3);
				num6 = Mathf.Sin(num9);
				num7 = Mathf.Cos(num9);
				tp += new Vector3(num6, 0f, -num7) * num8;
				return;
			}
		}
		else
		{
			if (typ == 2 || typ == 6 || typ == 9)
			{
				float num10 = ((typ == 2) ? 1.7f : 4f);
				float num11 = (float)(no / 2);
				float num12 = ((typ == 2) ? 0.8f : 2f);
				float num13 = num11 * num12 - (float)((num - 1) / 2) * num12 * 0.5f;
				if ((no & 1) != 0)
				{
					num10 = -num10;
					if ((num & 1) != 0)
					{
						num13 += num12 * 0.5f;
					}
				}
				float num14 = 0.017453292f * num2;
				float num15 = Mathf.Sin(num14);
				float num16 = Mathf.Cos(num14);
				tp += new Vector3(num15, 0f, -num16) * num13;
				float num17 = 0.017453292f * (90f + num2);
				num15 = Mathf.Sin(num17);
				num16 = Mathf.Cos(num17);
				cp = tp + new Vector3(num15, 0f, -num16) * num10;
				return;
			}
			if (typ == 4 || typ == 8)
			{
				float num18 = 0.017453292f * ((float)(36 * no) + num2);
				float num19 = Mathf.Sin(num18);
				float num20 = Mathf.Cos(num18);
				cp += new Vector3(num19, 0f, -num20) * 5f;
				tp = cp + (cp - np);
				return;
			}
			if (typ == 7)
			{
				if (no == 0)
				{
					Matrix4x4 identity = Matrix4x4.identity;
					identity.SetTRS(tp, Quaternion.Euler(0f, num2, 0f), Vector3.one);
					tp = identity.MultiplyPoint3x4(new Vector3(0f, 0f, 5f));
					return;
				}
				if (no == 1)
				{
					cp = np;
					ScenePicnic.CharaCtrl charaCtrl2 = this.charaList.Find((ScenePicnic.CharaCtrl itm) => itm.pos == typ * 10);
					if (charaCtrl2 != null)
					{
						tp = charaCtrl2.hdl.GetHaraPos();
						tp.y = 0f;
						return;
					}
				}
				else
				{
					if (no < 9)
					{
						float num21 = 0.017453292f * ((float)(50 * (no - 2)) + num2);
						float num22 = Mathf.Sin(num21);
						float num23 = Mathf.Cos(num21);
						cp += new Vector3(num22, 0f, -num23) * 5f;
						tp = cp + (cp - np);
						return;
					}
					cp = np;
					return;
				}
			}
			else
			{
				if (typ == 10)
				{
					float num24 = Mathf.Asin(this.trainDist / 2f / ScenePicnic.trainCircle) * 57.29578f;
					Matrix4x4 identity2 = Matrix4x4.identity;
					identity2.SetTRS(cp, Quaternion.Euler(0f, num2 + num24 * (float)no * 2f + this.trainRot, 0f), Vector3.one);
					cp = identity2.MultiplyPoint3x4(new Vector3(0f, 0f, ScenePicnic.trainCircle));
					tp = identity2.MultiplyPoint3x4(new Vector3(-ScenePicnic.trainCircle, 0f, ScenePicnic.trainCircle));
					return;
				}
				if (typ == 11)
				{
					if (no < this.sheetPos.Count)
					{
						cp = this.sheetPos[no];
					}
					if (this.fwPos != null)
					{
						tp = this.fwPos.position;
						return;
					}
				}
				else if (typ == 12)
				{
					if (no < this.snowPos.Count - 1)
					{
						cp = this.snowPos[no].position;
						tp = this.snowPos[no].TransformPoint(0f, 0f, 5f);
						return;
					}
					if (this.snowPos.Count > 0)
					{
						cp = (this.snowPos[0].position + this.snowPos[2].position) * 0.5f;
						cp = (cp + this.snowPos[4].position) * 0.5f;
						Matrix4x4 identity3 = Matrix4x4.identity;
						identity3.SetTRS(cp, Quaternion.Euler(0f, num2 + this.snowBallRot, 0f), Vector3.one);
						cp = identity3.MultiplyPoint3x4(new Vector3(0f, 0f, ScenePicnic.snowBallCircle));
						tp = identity3.MultiplyPoint3x4(new Vector3(ScenePicnic.snowBallCircle, 0f, ScenePicnic.snowBallCircle));
						return;
					}
				}
				else
				{
					cp = this.InOutPos(no);
					tp = cp + (cp - np);
				}
			}
		}
	}

	private Vector3 InOutPos(int p)
	{
		float num = 2.5f + (float)(p % 10 / 2) * 0.75f;
		if ((p & 1) == 0)
		{
			num = -num;
		}
		Vector3 vector = ((this.camPos == null) ? Vector3.zero : this.camPos[this.camNo].transform.TransformPoint(num, 0f, 0f));
		vector.y = 0f;
		return vector;
	}

	private bool ChgChara(ScenePicnic.CharaCtrl cc)
	{
		bool flag = false;
		if (this.energy <= 0)
		{
			cc.ctrl = this.charaOut(cc);
			cc.ctrl.MoveNext();
			flag = true;
		}
		else if (cc.bf == cc.af && (cc.bf <= 0 || cc.bf != cc.hid))
		{
			cc.ctrl = this.charaOut(cc);
			cc.ctrl.MoveNext();
			flag = true;
		}
		return flag;
	}

	private CharaModelHandle MakeChara(CharaPackData cpd)
	{
		CharaModelHandle component = new GameObject("Chara" + cpd.id.ToString(), new Type[] { typeof(CharaModelHandle) }).GetComponent<CharaModelHandle>();
		component.transform.SetParent(this.field.transform, false);
		component.Initialize(cpd, true, false, false);
		component.SetLayer(ScenePicnic.charaLayer);
		component.SetModelActive(false);
		component.SetWeaponActive(false);
		component.PlayAnimation(CharaMotionDefine.ActKey.H_HOM_WAIT0, true, 1f, 0f, 0f, false);
		component.shadowSize = 0.6f;
		component.DispAccessory(0, true, false);
		return component;
	}

	// Note: this type is marked as 'beforefieldinit'.
	static ScenePicnic()
	{
		string[,] array = new string[4, 2];
		array[0, 0] = "SD_picnicjungle_noon_a";
		array[0, 1] = "SD_picnicjungle_night_a";
		array[1, 0] = "SD_picnicfishpond_noon_a";
		array[1, 1] = "SD_picnicfishpond_night_a";
		array[2, 0] = "SD_picnicpond_noon_a";
		array[2, 1] = "SD_picnicpond_night_a";
		array[3, 0] = "SD_picnicgacha_noon_a";
		array[3, 1] = "SD_picnicgacha_night_a";
		ScenePicnic.stageList = array;
		ScenePicnic.ballEffName = "Ef_info_picnicball";
		ScenePicnic.balonEffName = "Ef_info_picnicbaloon";
		ScenePicnic.racketEffName = "Ef_info_picnicracket";
		ScenePicnic.shutleEffName = "Ef_info_picnicshuttlecock";
		ScenePicnic.ringEffName = "Ef_info_picnicring";
		ScenePicnic.ruberEffName = "Ef_info_picnicrubberball";
		ScenePicnic.spinEffName = "Ef_info_kanzasi_spin";
		ScenePicnic.hagoEffName = "Ef_info_picnic_hagoita";
		ScenePicnic.haneEffName = "Ef_info_picnic_hanetuiki_hane";
		ScenePicnic.train1EffName = "Ef_info_picnic_train_rope_a";
		ScenePicnic.train2EffName = "Ef_info_picnic_train_rope_b";
		ScenePicnic.trWhistleEffName = "Ef_info_picnic_whistle_a";
		ScenePicnic.trNoteEffName = "Ef_info_picnic_train_note";
		ScenePicnic.attEffName = new List<string> { "_g", "_a", "_b", "_c", "_d", "_e", "_f" };
		ScenePicnic.fwSmlEffName = new List<string> { "Ef_stage_surface_fws_blue", "Ef_stage_surface_fws_green", "Ef_stage_surface_fws_lightblue", "Ef_stage_surface_fws_pink", "Ef_stage_surface_fws_red", "Ef_stage_surface_fws_yellowgreen" };
		ScenePicnic.emoEffName = new List<string> { "Ef_info_picnic_speak", "Ef_info_picnic_touching", "Ef_info_picnic_surprise" };
		ScenePicnic.emoFaceName = new List<List<string>>
		{
			new List<string> { "FACE_SMILE_1_B" },
			new List<string> { "FACE_SMILE_1_B", "FACE_SMILE_2_A", "FACE_SMILE_4_B" },
			new List<string> { "FACE_NOWAY_4_C", "FACE_NOWAY_1_B", "FACE_NOWAY_1_D" }
		};
		ScenePicnic.sheetEffName = "Ef_info_picnic_sheet";
		ScenePicnic.trainCircle = 4.5f;
		ScenePicnic.trainSpeed = 360f / (ScenePicnic.trainCircle * 2f * 3.1415927f);
		ScenePicnic.trainDistMin = 1.6f;
		ScenePicnic.trainDistMax = 1.9f;
		ScenePicnic.trainLpNum = 4;
		ScenePicnic.snowShadowPath = "Ef_info_shadow";
		ScenePicnic.snowMan1 = new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.PIC_SNOWMAN_1_ST,
			CharaMotionDefine.ActKey.PIC_SNOWMAN_1_LP,
			CharaMotionDefine.ActKey.PIC_SNOWMAN_1_ED
		};
		ScenePicnic.snowMan2 = new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.PIC_SNOWMAN_2_ST,
			CharaMotionDefine.ActKey.PIC_SNOWMAN_2_LP,
			CharaMotionDefine.ActKey.PIC_SNOWMAN_2_ED
		};
		ScenePicnic.snowBall1 = new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.PIC_SNOWROLING_ST,
			CharaMotionDefine.ActKey.PIC_SNOWROLING_LP,
			CharaMotionDefine.ActKey.PIC_SNOWROLING_EN
		};
		ScenePicnic.snowBall2 = new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.PIC_HARDING_ST,
			CharaMotionDefine.ActKey.PIC_HARDING_LP,
			CharaMotionDefine.ActKey.PIC_HARDING_EN
		};
		ScenePicnic.snowBallCircle = 5f;
		ScenePicnic.snowBallLpNum = 4;
		ScenePicnic.snowBallSizMax = 1.7f;
		ScenePicnic.snowBallSizMin = 1.1f;
	}

	private SimpleAnimation basePanel;

	private GameObject windowPanel;

	private GameObject buyPanel;

	private GameObject playPanel;

	private GameObject field;

	private Transform hidePanel;

	private FieldCameraScaler camera;

	private Camera[] camPos;

	private int camNo;

	private Transform chrPos;

	private StagePresetCtrl stageCtrl;

	private string stageName;

	private IEnumerator stageLoad;

	private bool reqStageLoad;

	private Transform charaIconAll;

	private Transform charaSelect;

	private List<CharaPackData> haveCharaPackList;

	private List<CharaPackData> dispCharaPackList;

	private SortFilterDefine.SortType sortType = SortFilterDefine.SortType.LEVEL;

	private ReuseScroll charaScroll;

	private List<Transform> picnicChara;

	private static readonly int ScrollDeckNum = 2;

	private ScenePicnic.CharaCtrl chgChr;

	private Transform playIconAll;

	private Transform playSelect;

	private List<ScenePicnic.PlayPackData> havePlayPackList;

	private List<ScenePicnic.PlayPackData> dispPlayPackList;

	private int playCategory;

	private PguiToggleButtonCtrl[] playBtn;

	private ReuseScroll playScroll;

	private List<Transform> picnicPlay;

	private static readonly int ScrollPlayNum = 2;

	private ScenePicnic.PlayCtrl chgPly;

	private bool hideMode;

	private Transform leftTop;

	private Transform rightBtn;

	private PguiImageCtrl Campaign_TimeInfo;

	private PguiOpenWindowCtrl winItemGet;

	private PguiOpenWindowCtrl winStaminaCharge;

	private GameObject winStaminaBuyCharge;

	private PguiOpenWindowCtrl winBuy;

	private Transform winBuyInfo;

	private IconItemCtrl winBuyIcon;

	private PguiButtonCtrl winBuyBtnMax;

	private PguiButtonCtrl winBuyBtnMin;

	private PguiButtonCtrl winBuyBtnPlus;

	private PguiButtonCtrl winBuyBtnMinus;

	private Transform winBuyReport;

	private PguiOpenWindowCtrl winPlay;

	private PguiButtonCtrl winPlayLeft;

	private PguiButtonCtrl winPlayRight;

	private List<IconItemCtrl> winPlayItem;

	private ScenePicnic.PlayPackData winPlayData;

	private List<KeyValuePair<DataManagerPicnic.FoodData, ShopData.ItemOne>> shopData;

	private List<int> chargeList;

	private float chargeTime;

	private int chargeIdx;

	private List<ScenePicnic.CharaCtrl> charaList;

	private int gameType;

	private ScenePicnic.GameType requestGame;

	private List<ScenePicnic.PlayCtrl> playList;

	private int itemGet;

	private List<DataManagerPicnic.DropItemData> itemGetList;

	private bool kisekiUpdate;

	private bool staminaCharge;

	private int staminaBuyCharge;

	private int foodBuy;

	private ItemData foodBuyItem;

	private ItemData foodBuyCoin;

	private int foodBuyAdd;

	private int foodBuyPrc;

	private int foodBuyNum;

	private int foodBuyMax;

	private int energy;

	private int energyBase;

	private int monthlyType;

	private static readonly string charaLayer = "FieldPlayer";

	private static readonly string charaShadowLayer = "FieldPlayerShadow";

	private static readonly string[,] stageList;

	private static readonly string ballEffName;

	private static readonly string balonEffName;

	private static readonly string racketEffName;

	private static readonly string shutleEffName;

	private static readonly string ringEffName;

	private static readonly string ruberEffName;

	private static readonly string spinEffName;

	private static readonly string hagoEffName;

	private static readonly string haneEffName;

	private static readonly string train1EffName;

	private static readonly string train2EffName;

	private static readonly string trWhistleEffName;

	private static readonly string trNoteEffName;

	private static readonly List<string> attEffName;

	private static readonly List<string> fwSmlEffName;

	private static readonly List<string> emoEffName;

	private static readonly List<List<string>> emoFaceName;

	private static readonly string sheetEffName;

	private List<string> effLoadList;

	private EffectData ballEff;

	private int ballEffNo;

	private Vector3 ballEffSpd;

	private Vector3 ballEffRot;

	private float ballTime;

	private string catchName;

	private EffectData catchEff;

	private Transform catchEffBone;

	private int catchEffNo;

	private int catchEffReq;

	private Vector3 catchEffSpd;

	private Vector3 catchEffRot;

	private float catchTime;

	private int catchCall;

	private string racketName;

	private string shutleName;

	private Dictionary<int, EffectData> racketEff;

	private EffectData shutleEff;

	private int badmintEffNo;

	private Vector3 badmintEffSpd;

	private float badmintTime;

	private int badmintCall;

	private string kenpaName;

	private EffectData kenpaEff;

	private List<ScenePicnic.Balon> balon;

	private Dictionary<int, EffectData> hagoEff;

	private EffectData haneEff;

	private int haneEffNo;

	private int haneEffTyp;

	private Vector3 haneEffSpd;

	private float haneTime;

	private int haneCall;

	private static readonly float trainCircle;

	private static readonly float trainSpeed;

	private static readonly float trainDistMin;

	private static readonly float trainDistMax;

	private static readonly int trainLpNum;

	private int trainLpCnt;

	private List<EffectData> trainEff;

	private List<ScenePicnic.CharaCtrl> trainChr;

	private EffectData trWhistleEff;

	private List<ScenePicnic.TrNote> trNote;

	private float trainSpd;

	private float trainRot;

	private int trainStep;

	private float trainDist;

	private bool trainSE;

	private CriAtomExPlayback trainSEhdl;

	private List<ScenePicnic.FirWrk> fwEff;

	private List<string> fwEffName;

	private Transform fwPos;

	private int fwCnt;

	private float fwTim;

	private List<EffectData> emoEff;

	private List<EffectData> sheetEff;

	private List<Vector3> sheetPos;

	private string snowEffName;

	private static readonly string snowShadowPath;

	private Transform snPos;

	private List<Transform> snowPos;

	private EffectData snowEff;

	private EffectData snowShadowEff;

	private static readonly List<CharaMotionDefine.ActKey> snowMan1;

	private static readonly List<CharaMotionDefine.ActKey> snowMan2;

	private static readonly List<CharaMotionDefine.ActKey> snowBall1;

	private static readonly List<CharaMotionDefine.ActKey> snowBall2;

	private static readonly float snowBallCircle;

	private static readonly int snowBallLpNum;

	private int snowBallLpCnt;

	private float snowBallSpd;

	private float snowBallRot;

	private float snowBallAng;

	private static readonly float snowBallSizMax;

	private static readonly float snowBallSizMin;

	private float snowBallSiz;

	private PguiAECtrl wipe;

	private int tutorial;

	private SceneManager.SceneName requestNextScene;

	private object requestNextArgs;

	private class PlayPackData
	{
		public DataManagerPicnic.PlayTypeData type;

		public DataManagerPicnic.PlayItemData item;
	}

	private class CharaCtrl
	{
		public int no;

		public int bf;

		public int af;

		public Transform icon;

		public CharaPackData chara;

		public IEnumerator ctrl;

		public CharaModelHandle hdl;

		public int hid;

		public int pos;

		public long rest;

		public int lck;

		public int mov;

		public bool fly;

		public bool hand;

		public float fw;
	}

	private enum GameType
	{
		Invalid,
		Kick,
		Hana,
		Sit,
		Walk,
		Catch,
		Badmint,
		Kenpa,
		Balon,
		Hane,
		Train,
		FireWork,
		Snow
	}

	private class PlayCtrl
	{
		public int no;

		public int bf;

		public int af;

		public Transform icon;

		public ScenePicnic.PlayPackData play;

		public long rest;
	}

	private class Balon
	{
		public string name;

		public EffectData eff;

		public float time;

		public int no;

		public Transform tag;
	}

	private class TrNote
	{
		public ScenePicnic.CharaCtrl chr;

		public float tim;

		public EffectData eff;
	}

	private class FirWrk
	{
		public EffectData eff;

		public int no;

		public float time;
	}
}
