using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using AEAuth3;
using CriWare;
using SGNFW.Common;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelCharaGrowCtrl : MonoBehaviour
{
	public bool TouchRect
	{
		get
		{
			bool touchRect = this._touchRect;
			this._touchRect = false;
			return touchRect;
		}
		private set
		{
			this._touchRect = value;
		}
	}

	public bool IsTutorial
	{
		get
		{
			return this._isTutorial;
		}
	}

	public List<CharaPackData> DispCharaPackList
	{
		get
		{
			return this._dispCharaPackList;
		}
	}

	public void SetIsTutorial(bool isTutorial)
	{
		this._isTutorial = isTutorial;
	}

	public RectTransform GetCharaTopRectTransform(int id)
	{
		return new List<IconCharaCtrl>(this._guiData.CharacterGrowTop.ScrollView.GetComponentsInChildren<IconCharaCtrl>()).Find((IconCharaCtrl item) => item.charaPackData.id == id).transform.Find("BaseImage") as RectTransform;
	}

	public bool IsPlayingAnimCharaSelect()
	{
		return this._guiData.CharacterGrowTop.SelCmnAllInOut.ExIsPlaying();
	}

	public RectTransform GetTabRectTransform(SelCharaGrowCtrl.TabType tab)
	{
		this._tutorialClickTabIndex = (int)tab;
		return this._guiData.CharacterGrowMain.Cmn.TabGuiList[(int)tab].TabCtrl.transform as RectTransform;
	}

	public RectTransform GetWildReleaseBtnRectTransform()
	{
		return this._charaGrowWild.GrowWildGUI.wildReleaseTab.Btn_OpenComp.transform as RectTransform;
	}

	public bool FinishedOpenConfirmationWindow()
	{
		return this._charaGrowWild.GrowWildGUI.wildGrowWindow.owCtrl.FinishedOpen();
	}

	public RectTransform GetWindowButtonRectTransform(int btnIndex)
	{
		if (btnIndex < 0)
		{
			return this._charaGrowWild.GrowWildGUI.wildResultWindow.owCtrl.GetButtonRectTransform(1);
		}
		return this._charaGrowWild.GrowWildGUI.wildGrowWindow.owCtrl.GetButtonRectTransform(btnIndex);
	}

	public void Init()
	{
		this._mainCharaGrowObj = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneCharaEdit/GUI/Prefab/GUI_CharaGrow"), base.transform);
		this._guiData = new SelCharaGrowCtrl.CommonGUI(this._mainCharaGrowObj.transform);
		this._charaGrowLevelUp = new SelCharaGrowLevel(this._mainCharaGrowObj.transform);
		this._charaGrowWild = new SelCharaGrowWild(this._mainCharaGrowObj.transform);
		this._charaGrowRank = new SelCharaGrowRank(this._mainCharaGrowObj.transform);
		this._charaGrowMiracle = new SelCharaGrowMiracle(this._mainCharaGrowObj.transform);
		this._charaGrowPhotoPocket = new SelCharaGrowPhotoPocket(this._mainCharaGrowObj.transform);
		this._charaGrowKizuna = new SelCharaGrowKizuna(this._mainCharaGrowObj.transform);
		this._charaGrowNanairo = new SelCharaGrowNanairo(this._mainCharaGrowObj.transform);
		this._charaGrowMulti = new SelCharaGrowMulti(this._mainCharaGrowObj.transform);
		this._guiData.CharacterGrowMain.TabObjectMap.Add(SelCharaGrowCtrl.TabType.LevelUp, this._charaGrowLevelUp.LevelUpGUI.lvUpTab.baseObj);
		this._guiData.CharacterGrowMain.TabObjectMap.Add(SelCharaGrowCtrl.TabType.WildRelease, this._charaGrowWild.GrowWildGUI.wildReleaseTab.baseObj);
		this._guiData.CharacterGrowMain.TabObjectMap.Add(SelCharaGrowCtrl.TabType.RankUp, this._charaGrowRank.GrowRankGUI.rankUpTab.baseObj);
		this._guiData.CharacterGrowMain.TabObjectMap.Add(SelCharaGrowCtrl.TabType.MiracleUp, this._charaGrowMiracle.GrowMiracleGUI.miracleUpTab.baseObj);
		this._guiData.CharacterGrowMain.TabObjectMap.Add(SelCharaGrowCtrl.TabType.PhotoPocket, this._charaGrowPhotoPocket.GrowPhotoPocketGUI.photoPocketTab.baseObj);
		this._guiData.CharacterGrowMain.TabObjectMap.Add(SelCharaGrowCtrl.TabType.Kizuna, this._charaGrowKizuna.GrowKizunaGUI.KizunaTab.ParentObject);
		this._guiData.CharacterGrowMain.TabObjectMap.Add(SelCharaGrowCtrl.TabType.Nanairo, this._charaGrowNanairo.GrowNanairoGUI.nanairoUpTab.baseObj);
		this._charaGrowWindow = new GameObject();
		this._charaGrowWindow.name = "CharaGrowWindow";
		this._charaGrowWindow.transform.localScale = Vector3.one;
		RectTransform rectTransform = this._charaGrowWindow.AddComponent<RectTransform>();
		rectTransform.anchorMin = Vector2.zero;
		rectTransform.anchorMax = Vector2.one;
		rectTransform.offsetMax = Vector2.zero;
		rectTransform.offsetMin = Vector2.zero;
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.FRONT, base.transform, true);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this._charaGrowWindow.transform, false);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this._charaGrowLevelUp.LevelUpGUI.lvUpWindow.owCtrl.transform, true);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this._charaGrowWild.GrowWildGUI.wildGrowWindow.owCtrl.transform, true);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this._charaGrowWild.GrowWildGUI.wildResultWindow.owCtrl.transform, true);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this._charaGrowWild.GrowWildGUI.tokuseiInfoWindow.owCtrl.transform, true);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this._charaGrowWild.GrowWildGUI.iconOpenWindow.owCtrl.transform, true);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this._charaGrowRank.GrowRankGUI.rankUpWindow.owCtrl.transform, true);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this._charaGrowRank.GrowRankGUI.rankUpResultWindow.owCtrl.transform, true);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this._charaGrowMiracle.GrowMiracleGUI.miracleWindow.owCtrl.transform, true);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this._charaGrowMiracle.GrowMiracleGUI.miracleResultWindow.owCtrl.transform, true);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this._charaGrowLevelUp.LevelUpGUI.itemUseWindow.owCtrl.transform, true);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this._charaGrowPhotoPocket.GrowPhotoPocketGUI.photoPocketWindow.owCtrl.transform, true);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this._guiData.ItemExchangeWindow.owCtrl.transform, true);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this._charaGrowRank.GrowRankGUI.rankUpAuth.baseObj.transform, true);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this._charaGrowPhotoPocket.GrowPhotoPocketGUI.photoPocketResult.baseObj.transform, true);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this._charaGrowPhotoPocket.GrowPhotoPocketGUI.photoPocketInfoWindow.baseObj.transform, true);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this._guiData.GrowthAchievementRewardWindow.BaseObj.transform, true);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this._charaGrowLevelUp.LevelUpGUI.levelLimitOverWindow.baseObj.transform, true);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this._charaGrowKizuna.GrowKizunaGUI.KizunaWindow.OpenWindowCtrl.transform, true);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this._charaGrowKizuna.GrowKizunaGUI.KizunaLvUpWindow.OpenWindowCtrl.transform, true);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this._charaGrowKizuna.LevelUpGUI.ItemUseWindow.OpenWindowCtrl.transform, true);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this._charaGrowNanairo.GrowNanairoGUI.nanairoWindow.owCtrl.transform, true);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this._charaGrowNanairo.GrowNanairoGUI.nanairoResultWindow.owCtrl.transform, true);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this._charaGrowMulti.GrowMultiGUI.growMultiSelectWindow.owCtrl.transform, true);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this._charaGrowMulti.GrowMultiGUI.growMultiCheckWindow.owCtrl.transform, true);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this._guiData.SingleItemInfoWindow.owCtrl.transform, true);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this._guiData.MultiItemInfoWindow.owCtrl.transform, true);
		this._charaGrowWindow.transform.SetParent(Singleton<CanvasManager>.Instance.SystemPanel.transform, true);
		this._charaGrowWindow.transform.SetSiblingIndex(Singleton<CanvasManager>.Instance.cmnTouchMask.transform.GetSiblingIndex());
		this._charaGrowLevelUp.LevelUpGUI.lvUpWindow.owCtrl.transform.SetParent(this._charaGrowWindow.transform, true);
		this._charaGrowLevelUp.LevelUpGUI.lvUpWindow.owCtrl.transform.SetAsFirstSibling();
		this._charaGrowWild.GrowWildGUI.wildGrowWindow.owCtrl.transform.SetParent(this._charaGrowWindow.transform, true);
		this._charaGrowWild.GrowWildGUI.wildGrowWindow.owCtrl.transform.SetAsFirstSibling();
		this._charaGrowWild.GrowWildGUI.wildResultWindow.owCtrl.transform.SetParent(this._charaGrowWindow.transform, true);
		this._charaGrowWild.GrowWildGUI.wildResultWindow.owCtrl.transform.SetAsFirstSibling();
		this._charaGrowWild.GrowWildGUI.tokuseiInfoWindow.owCtrl.transform.SetParent(this._charaGrowWindow.transform, true);
		this._charaGrowWild.GrowWildGUI.tokuseiInfoWindow.owCtrl.transform.SetAsFirstSibling();
		this._charaGrowWild.GrowWildGUI.iconOpenWindow.owCtrl.transform.SetParent(this._charaGrowWindow.transform, true);
		this._charaGrowWild.GrowWildGUI.iconOpenWindow.owCtrl.transform.SetAsFirstSibling();
		this._charaGrowRank.GrowRankGUI.rankUpWindow.owCtrl.transform.SetParent(this._charaGrowWindow.transform, true);
		this._charaGrowRank.GrowRankGUI.rankUpWindow.owCtrl.transform.SetAsFirstSibling();
		this._charaGrowRank.GrowRankGUI.rankUpResultWindow.owCtrl.transform.SetParent(this._charaGrowWindow.transform, true);
		this._charaGrowRank.GrowRankGUI.rankUpResultWindow.owCtrl.transform.SetAsFirstSibling();
		this._charaGrowMiracle.GrowMiracleGUI.miracleWindow.owCtrl.transform.SetParent(this._charaGrowWindow.transform, true);
		this._charaGrowMiracle.GrowMiracleGUI.miracleWindow.owCtrl.transform.SetAsFirstSibling();
		this._charaGrowMiracle.GrowMiracleGUI.miracleResultWindow.owCtrl.transform.SetParent(this._charaGrowWindow.transform, true);
		this._charaGrowMiracle.GrowMiracleGUI.miracleResultWindow.owCtrl.transform.SetAsFirstSibling();
		this._charaGrowLevelUp.LevelUpGUI.itemUseWindow.owCtrl.transform.SetParent(this._charaGrowWindow.transform, true);
		this._charaGrowLevelUp.LevelUpGUI.itemUseWindow.owCtrl.transform.SetAsFirstSibling();
		this._charaGrowPhotoPocket.GrowPhotoPocketGUI.photoPocketInfoWindow.owCtrl.transform.SetParent(this._charaGrowWindow.transform, true);
		this._charaGrowPhotoPocket.GrowPhotoPocketGUI.photoPocketInfoWindow.owCtrl.transform.SetAsFirstSibling();
		this._charaGrowPhotoPocket.GrowPhotoPocketGUI.photoPocketWindow.owCtrl.transform.SetParent(this._charaGrowWindow.transform, true);
		this._charaGrowPhotoPocket.GrowPhotoPocketGUI.photoPocketWindow.owCtrl.transform.SetAsFirstSibling();
		this._guiData.ItemExchangeWindow.owCtrl.transform.SetParent(this._charaGrowWindow.transform, true);
		this._guiData.ItemExchangeWindow.owCtrl.transform.SetAsFirstSibling();
		this._charaGrowRank.GrowRankGUI.rankUpAuth.baseObj.transform.SetParent(this._charaGrowWindow.transform, true);
		this._charaGrowRank.GrowRankGUI.rankUpAuth.baseObj.transform.SetAsFirstSibling();
		this._charaGrowPhotoPocket.GrowPhotoPocketGUI.photoPocketResult.baseObj.transform.SetParent(this._charaGrowWindow.transform, true);
		this._charaGrowPhotoPocket.GrowPhotoPocketGUI.photoPocketResult.baseObj.transform.SetAsFirstSibling();
		this._guiData.GrowthAchievementRewardWindow.OpenWindowCtrl.transform.SetParent(this._charaGrowWindow.transform, true);
		this._guiData.GrowthAchievementRewardWindow.OpenWindowCtrl.transform.SetAsFirstSibling();
		this._charaGrowLevelUp.LevelUpGUI.levelLimitOverWindow.owCtrl.transform.SetParent(this._charaGrowWindow.transform, true);
		this._charaGrowLevelUp.LevelUpGUI.levelLimitOverWindow.owCtrl.transform.SetAsFirstSibling();
		this._charaGrowKizuna.GrowKizunaGUI.KizunaWindow.OpenWindowCtrl.transform.SetParent(this._charaGrowWindow.transform, true);
		this._charaGrowKizuna.GrowKizunaGUI.KizunaWindow.OpenWindowCtrl.transform.SetAsFirstSibling();
		this._charaGrowNanairo.GrowNanairoGUI.nanairoResultWindow.owCtrl.transform.SetParent(this._charaGrowWindow.transform, true);
		this._charaGrowNanairo.GrowNanairoGUI.nanairoResultWindow.owCtrl.transform.SetAsFirstSibling();
		this._charaGrowMulti.GrowMultiGUI.growMultiSelectWindow.owCtrl.transform.SetParent(this._charaGrowWindow.transform, true);
		this._charaGrowMulti.GrowMultiGUI.growMultiSelectWindow.owCtrl.transform.SetAsFirstSibling();
		this._charaGrowMulti.GrowMultiGUI.growMultiCheckWindow.owCtrl.transform.SetParent(this._charaGrowWindow.transform, true);
		this._charaGrowMulti.GrowMultiGUI.growMultiCheckWindow.owCtrl.transform.SetAsFirstSibling();
		this._charaGrowKizuna.GrowKizunaGUI.KizunaLvUpWindow.OpenWindowCtrl.transform.SetParent(this._charaGrowWindow.transform, true);
		this._charaGrowKizuna.GrowKizunaGUI.KizunaLvUpWindow.OpenWindowCtrl.transform.SetAsFirstSibling();
		this._charaGrowKizuna.LevelUpGUI.LvUpWindow.OpenWindowCtrl.transform.SetParent(this._charaGrowWindow.transform, true);
		this._charaGrowKizuna.LevelUpGUI.LvUpWindow.OpenWindowCtrl.transform.SetAsFirstSibling();
		this._charaGrowKizuna.LevelUpGUI.ItemUseWindow.OpenWindowCtrl.transform.SetParent(this._charaGrowWindow.transform, true);
		this._charaGrowKizuna.LevelUpGUI.ItemUseWindow.OpenWindowCtrl.transform.SetAsFirstSibling();
		this._guiData.SingleItemInfoWindow.owCtrl.transform.SetParent(this._charaGrowWindow.transform, true);
		this._guiData.SingleItemInfoWindow.owCtrl.transform.SetAsFirstSibling();
		this._guiData.MultiItemInfoWindow.owCtrl.transform.SetParent(this._charaGrowWindow.transform, true);
		this._guiData.MultiItemInfoWindow.owCtrl.transform.SetAsFirstSibling();
		this.InitializeItemWindow();
		this._currentCharaId = new List<CharaPackData>(DataManager.DmChara.GetUserCharaMap().Values)[0].id;
		this._currentMode = SelCharaGrowCtrl.Mode.Top;
		this._lvUpCostCoin = 0;
		this._effectStatus = SelCharaGrowCtrl.EffectStatus.None;
		this._selectLvUpItemIdList = new List<int>();
		this._selectKizunaLvUpItemIdList = new List<int>();
		this._nextOpenPhotoIndex = -1;
		this._guiData.CharacterGrowMain.Cmn.TabGroup.Setup(0, new PguiTabGroupCtrl.OnSelectTab(this.OnSelectTab));
		this._lastTabIndex = 0;
		this._guiData.CharacterGrowMain.Cmn.ButtonL.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this._guiData.CharacterGrowMain.Cmn.ButtonR.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this._guiData.CharacterGrowMain.Cmn.ButtonRExchange.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this._guiData.CharacterGrowMain.BtnYajiLeft.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickArrowButton), PguiButtonCtrl.SoundType.DEFAULT);
		this._guiData.CharacterGrowMain.BtnYajiRight.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickArrowButton), PguiButtonCtrl.SoundType.DEFAULT);
		this._guiData.CharacterGrowMain.BtnMoreInfo.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickCharaDetailButton), PguiButtonCtrl.SoundType.DEFAULT);
		this._guiData.CharacterGrowMain.BtnPresent.AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			this._guiData.GrowthAchievementRewardWindow.Refresh(this._currentCharaId);
			this._guiData.GrowthAchievementRewardWindow.OpenWindowCtrl.Setup(null, null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.LR_CURSOR), true, (int index) => true, null, false);
			this._guiData.GrowthAchievementRewardWindow.OpenWindowCtrl.Open();
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this._guiData.CharacterGrowMain.BtnFavorite.AddOnClickListener(delegate(PguiToggleButtonCtrl ptb, int btn)
		{
			DataManager.DmChara.RequestActtionCharaFavoriteFlag(this._currentCharaId);
			return true;
		});
		this._guiData.CharacterGrowMain.BtnGrowMulti.AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			this._charaGrowMulti.GrowMultiGUI.growMultiSelectWindow.owCtrl.Setup(null, null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES_EMPTY), true, new PguiOpenWindowCtrl.Callback(this.OnSelectGrowMultiSelectWindowButtonCallback), null, false);
			this._charaGrowMulti.GrowMultiGUI.growMultiSelectWindow.cpd = DataManager.DmChara.GetUserCharaData(this._currentCharaId);
			this._charaGrowMulti.GrowMultiGUI.growMultiSelectWindow.InitContent(this._enhanceList);
			base.StartCoroutine(this._charaGrowMulti.GrowMultiGUI.growMultiSelectWindow.DispMessageMark());
			this._charaGrowMulti.GrowMultiGUI.growMultiSelectWindow.owCtrl.Open();
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this._guiData.CharacterGrowMain.BtnAccessory.AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this._guiData.CharacterGrowMain.StatusInfomation.BtnInfo.AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			this._charaGrowPhotoPocket.GrowPhotoPocketGUI.photoPocketInfoWindow.owCtrl.Setup(null, null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.LR_CURSOR), true, (int index) => true, null, false);
			this._charaGrowPhotoPocket.GrowPhotoPocketGUI.photoPocketInfoWindow.owCtrl.Open();
			this._charaGrowPhotoPocket.GrowPhotoPocketGUI.photoPocketInfoWindow.Setup(DataManager.DmChara.GetUserCharaData(this._currentCharaId), false);
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this._haveCharaPackList = new List<CharaPackData>(DataManager.DmChara.GetUserCharaMap().Values);
		this._dispCharaPackList = new List<CharaPackData>(this._haveCharaPackList);
		ReuseScroll scrollView = this._guiData.CharacterGrowTop.ScrollView;
		scrollView.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollView.onStartItem, new Action<int, GameObject>(this.OnStartCharaTop));
		ReuseScroll scrollView2 = this._guiData.CharacterGrowTop.ScrollView;
		scrollView2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollView2.onUpdateItem, new Action<int, GameObject>(this.OnUpdateCharaTop));
		this._guiData.CharacterGrowTop.ScrollView.Setup(10, 0);
		ReuseScroll scrollView3 = this._charaGrowLevelUp.LevelUpGUI.lvUpTab.ScrollView;
		scrollView3.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollView3.onStartItem, new Action<int, GameObject>(this.OnStartItemLvUp));
		ReuseScroll scrollView4 = this._charaGrowLevelUp.LevelUpGUI.lvUpTab.ScrollView;
		scrollView4.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollView4.onUpdateItem, new Action<int, GameObject>(this.OnUpdateItemLvUp));
		this._charaGrowLevelUp.LevelUpGUI.lvUpTab.ScrollView.Setup(7, 0);
		this._charaGrowLevelUp.LevelUpGUI.lvUpWindow.ScrollView.InitForce();
		ReuseScroll scrollView5 = this._charaGrowLevelUp.LevelUpGUI.lvUpWindow.ScrollView;
		scrollView5.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollView5.onStartItem, new Action<int, GameObject>(this.OnStartWindowLvUpItem));
		ReuseScroll scrollView6 = this._charaGrowLevelUp.LevelUpGUI.lvUpWindow.ScrollView;
		scrollView6.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollView6.onUpdateItem, new Action<int, GameObject>(this.OnUpdateWindowLvUpItem));
		this._charaGrowLevelUp.LevelUpGUI.lvUpWindow.ScrollView.Setup(1, 0);
		this._charaGrowLevelUp.LevelUpGUI.itemUseWindow.Btn_Minus.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonForItemUse), PguiButtonCtrl.SoundType.DEFAULT);
		this._charaGrowLevelUp.LevelUpGUI.itemUseWindow.Btn_Plus.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonForItemUse), PguiButtonCtrl.SoundType.DEFAULT);
		this._charaGrowWild.currentIndexWild = 0;
		this.StartItemWild(this._currentCharaId);
		GameObject gameObject = (GameObject)Resources.Load("CmnIconFrame/GUI/Prefab/Icon_Item");
		this._charaGrowWild.GrowWildGUI.wildReleaseTab.ItemIcon = Object.Instantiate<GameObject>(gameObject, this._charaGrowWild.GrowWildGUI.wildReleaseTab.baseObj.transform.Find("ItemIcon"));
		this._charaGrowWild.GrowWildGUI.wildReleaseTab.ItemIconCtrl = this._charaGrowWild.GrowWildGUI.wildReleaseTab.ItemIcon.GetComponent<IconItemCtrl>();
		this._charaGrowWild.SetupItemInfoWild(this._currentCharaId);
		this._charaGrowWild.GrowWildGUI.wildReleaseTab.Btn_OpenAll.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickSetReleaseButton), PguiButtonCtrl.SoundType.DEFAULT);
		this._charaGrowWild.GrowWildGUI.wildReleaseTab.Btn_OpenComp.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickWildReleaseButton), PguiButtonCtrl.SoundType.DEFAULT);
		this._charaGrowRank.SetupItemRank(this._currentCharaId);
		this._charaGrowMiracle.SetupItemMiracle(this._currentCharaId);
		this._charaGrowPhotoPocket.SetupItemPhotoPocket(this._currentCharaId);
		this._guiData.CharacterGrowMain.PhotoMax.ItemConversion.ButtonC.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this._guiData.CharacterGrowMain.PhotoPocketConversionStrengthen.ItemConversion.ButtonC.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this._guiData.CharacterGrowMain.LvLimitOpen.ButtonC.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this._charaGrowNanairo.SetupItemNanairo(this._currentCharaId);
		this._charaGrowPhotoPocket.GrowPhotoPocketGUI.photoPocketWindow.Btn_MoreInfo.AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			this._charaGrowPhotoPocket.GrowPhotoPocketGUI.photoPocketInfoWindow.owCtrl.Setup(null, null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.LR_CURSOR), true, (int index) => true, null, false);
			this._charaGrowPhotoPocket.GrowPhotoPocketGUI.photoPocketInfoWindow.owCtrl.Open();
			this._charaGrowPhotoPocket.GrowPhotoPocketGUI.photoPocketInfoWindow.Setup(DataManager.DmChara.GetUserCharaData(this._currentCharaId), true);
		}, PguiButtonCtrl.SoundType.DEFAULT);
		using (List<SelCharaGrowCtrl.PhotoPocketIcon>.Enumerator enumerator = this._charaGrowPhotoPocket.GrowPhotoPocketGUI.photoPocketInfoWindow.photoPocketIcons.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				SelCharaGrowCtrl.PhotoPocketIcon photoPocketIcon = enumerator.Current;
				photoPocketIcon.SetActiveCurrent(false);
				photoPocketIcon.AddTouchEventTrigger(delegate(Transform tr)
				{
					SoundManager.Play("prd_se_click", false, false);
					foreach (SelCharaGrowCtrl.PhotoPocketIcon photoPocketIcon2 in this._charaGrowPhotoPocket.GrowPhotoPocketGUI.photoPocketInfoWindow.photoPocketIcons)
					{
						photoPocketIcon2.SetActiveCurrent(false);
					}
					photoPocketIcon.SetActiveCurrent(true);
					this._charaGrowPhotoPocket.GrowPhotoPocketGUI.photoPocketInfoWindow.UpdateInfo(DataManager.DmChara.GetUserCharaData(this._currentCharaId).staticData);
				});
				this._charaGrowPhotoPocket.GrowPhotoPocketGUI.photoPocketInfoWindow.photoPocketIcons[0].SetActiveCurrent(true);
			}
		}
		this._charaGrowPhotoPocket.GrowPhotoPocketGUI.photoPocketInfoWindow.Txt_Info.gameObject.SetActive(this._charaGrowPhotoPocket.GrowPhotoPocketGUI.photoPocketInfoWindow.GetPhotoPocketIconIndex >= 0);
		this._charaGrowPhotoPocket.GrowPhotoPocketGUI.photoPocketInfoWindow.Txt_PocketName.gameObject.SetActive(this._charaGrowPhotoPocket.GrowPhotoPocketGUI.photoPocketInfoWindow.GetPhotoPocketIconIndex >= 0);
		this._charaGrowKizuna.Initialize(delegate(int x)
		{
			this.ExecuteKizunaLimitOver(x);
			return true;
		});
		SelCharaGrowKizuna.WindowItemUse itemUseWindow = this._charaGrowKizuna.LevelUpGUI.ItemUseWindow;
		this._charaGrowKizuna.GrowKizunaGUI.KizunaLvUpTab.SetAction(delegate(int index, GameObject go)
		{
			this.OnStartItemKizunaLvUp(index, go);
		}, delegate(int index, GameObject go)
		{
			this.OnUpdateItemKizunaLvUp(index, go);
		});
		this._charaGrowKizuna.GrowKizunaGUI.KizunaLvUpWindow.SetAction(delegate(int index, GameObject go)
		{
			this.OnStartWindowKizunaLvUpItem(index, go);
		}, delegate(int index, GameObject go)
		{
			this.OnUpdateWindowKizunaLvUpItem(index, go);
		});
		itemUseWindow.BtnMinus.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonForKizunaItemUse), PguiButtonCtrl.SoundType.DEFAULT);
		itemUseWindow.BtnPlus.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButtonForKizunaItemUse), PguiButtonCtrl.SoundType.DEFAULT);
		this._guiData.GrowthAchievementRewardWindow.ScrollView.InitForce();
		ReuseScroll scrollView7 = this._guiData.GrowthAchievementRewardWindow.ScrollView;
		scrollView7.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollView7.onStartItem, new Action<int, GameObject>(this.OnAchievementRewardStart));
		ReuseScroll scrollView8 = this._guiData.GrowthAchievementRewardWindow.ScrollView;
		scrollView8.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollView8.onUpdateItem, new Action<int, GameObject>(this.OnAchievementRewardUpdate));
		this._guiData.GrowthAchievementRewardWindow.ScrollView.Setup(10, 0);
		SortWindowCtrl.RegisterData registerData = new SortWindowCtrl.RegisterData
		{
			register = SortFilterDefine.RegisterType.CHARA_GROW_TOP,
			filterButton = this._guiData.CharacterGrowTop.BtnFilterOnOff,
			sortButton = this._guiData.CharacterGrowTop.BtnSort,
			sortUdButton = this._guiData.CharacterGrowTop.BtnSortUpDown,
			funcGetTargetBaseList = () => new SortWindowCtrl.SortTarget
			{
				charaList = this._haveCharaPackList
			},
			funcDisideTarget = delegate(SortWindowCtrl.SortTarget item)
			{
				this._dispCharaPackList = item.charaList;
				this._sortType = item.sortType;
				this._guiData.CharacterGrowTop.TxtNone.SetActive(this._dispCharaPackList.Count <= 0);
				this._guiData.CharacterGrowTop.ScrollView.ResizeFocesNoMove(1 + this._dispCharaPackList.Count / 3);
			}
		};
		CanvasManager.HdlOpenWindowSortFilter.Register(registerData, true, null);
		this.Setup();
	}

	public void Setup()
	{
		this._haveCharaPackList = new List<CharaPackData>(DataManager.DmChara.GetUserCharaMap().Values);
		this._dispCharaPackList = new List<CharaPackData>(this._haveCharaPackList);
		this._currentCampaign = DataManager.DmCampaign.PresentCampaignGrowCharaData;
		this._expGageEffect = null;
		this._wildReleaseEffect = null;
		this._rankUpGageEffect = null;
		this._miracleEffect = null;
		this._photoPocketEffect = null;
		this._itemExchangeEffect = null;
		this._connectFlowBase = null;
		this._growthAchievementRewardEffect = null;
		this.levelLimitOverEffect = null;
		this._ieKizunaLimitOver = null;
		this._ieReleaseAccessory = null;
		this._nanairoReleaseEffect = null;
		this._growMultiCoroutine = null;
		if (this._renderTextureChara == null)
		{
			this._renderTextureChara = AssetManager.InstantiateAssetData("RenderTextureChara/Prefab/RenderTextureCharaCtrl", this._guiData.CharacterGrowMain.BaseObj.transform.Find("Main/Left")).GetComponent<RenderTextureChara>();
			this._renderTextureChara.postion = new Vector2(0f, -78f);
			this._renderTextureChara.fieldOfView = 31.5f;
			this._renderTextureChara.transform.SetSiblingIndex(0);
		}
		this._guiData.CharacterGrowMain.BaseObj.SetActive(false);
		this._guiData.CharacterGrowTop.BaseObj.SetActive(true);
		this._guiData.CharacterGrowTop.SelCmnAllInOut.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
		this._guiData.CharacterGrowTop.ScrollView.Refresh();
		CanvasManager.HdlOpenWindowSortFilter.SolutionList(SortFilterDefine.RegisterType.CHARA_GROW_TOP, null);
		this._tutorialClickTabIndex = Enum.GetValues(typeof(SelCharaGrowCtrl.TabType)).Length;
	}

	private void CharaGrowSetup(int charaId, bool resetTab)
	{
		this._guiData.CharacterGrowMain.BtnYajiRight.gameObject.SetActive(this._enableLeftRightButton);
		this._guiData.CharacterGrowMain.BtnYajiLeft.gameObject.SetActive(this._enableLeftRightButton);
		this._guiData.CharacterGrowMain.BtnMoreInfo.gameObject.SetActive(this._enableMoreButton);
		this._currentCharaId = charaId;
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(this._currentCharaId);
		int num = this.<CharaGrowSetup>g__GetCanReceiveGrowthAchievementRewardCount|117_0();
		this._guiData.CharacterGrowMain.TxtPresentNum.text = string.Format("{0}", num);
		this._guiData.CharacterGrowMain.TxtPresentNum.transform.parent.gameObject.SetActive(0 < num);
		this.ClearSelectLvUpItem(false);
		this._guiData.CharacterGrowMain.BtnAccessoryImageOpen.gameObject.SetActive(false);
		if (this._renderTextureChara.DispCharaId != this._currentCharaId || (this._renderTextureChara.DispCharaId == this._currentCharaId && this._renderTextureChara.DispClothImageId != userCharaData.equipClothImageId))
		{
			this._renderTextureChara.Setup(userCharaData, 0, CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true, null, false, null, 0f, null, true);
		}
		this._guiData.CharacterGrowMain.SetCharaInfo(userCharaData);
		this._guiData.CharacterGrowMain.UpdateCharaParameter(userCharaData);
		this._enhanceList = new List<bool> { userCharaData.EnhanceInfoLv, userCharaData.EnhanceInfoPromote, userCharaData.EnhanceInfoRank, userCharaData.EnhanceInfoMiracle, userCharaData.EnhanceInfoNanairo, userCharaData.EnhanceInfoPhotoPocket, userCharaData.EnhanceInfoKizunaLimit };
		foreach (object obj in Enum.GetValues(typeof(SelCharaGrowCtrl.TabType)))
		{
			int num2 = (int)obj;
			if (num2 >= Enum.GetValues(typeof(SelCharaGrowCtrl.TabType)).Length)
			{
				break;
			}
			this._guiData.CharacterGrowMain.Cmn.TabGuiList[num2].YellowBadge.SetActive(this._enhanceList[num2]);
			if (this._enhanceList[num2])
			{
				this._guiData.CharacterGrowMain.Cmn.TabGuiList[num2].YellowBadgeAnim.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
			}
		}
		if (resetTab)
		{
			this._guiData.CharacterGrowMain.Cmn.TabGroup.Setup(0, new PguiTabGroupCtrl.OnSelectTab(this.OnSelectTab));
			this._lastTabIndex = 0;
		}
		this.OnSelectTab(this._guiData.CharacterGrowMain.Cmn.TabGroup.SelectIndex);
		if (DataManager.DmChara.GetUserCharaData(this._currentCharaId).dynamicData.favoriteFlag)
		{
			this._guiData.CharacterGrowMain.BtnFavorite.SetToggleIndex(1);
			return;
		}
		this._guiData.CharacterGrowMain.BtnFavorite.SetToggleIndex(0);
	}

	public void SetupParam(bool enableLeftRightButton, bool enableMoreButton)
	{
		this._enableLeftRightButton = enableLeftRightButton;
		this._enableMoreButton = enableMoreButton;
	}

	public void SetupBySceneForce(int charaId, int selectTab, bool itemWindow)
	{
		if (this.lastOpenItemWindowParam == null || this.lastOpenItemWindowParam.itemList.Count <= this.lastOpenItemWindowParam.index)
		{
			itemWindow = false;
		}
		if (itemWindow && selectTab == 1)
		{
			this._wildReleaseEffectPlaying = true;
		}
		this.OnClickCharaTopButton(charaId);
		this._guiData.CharacterGrowMain.Cmn.TabGroup.Setup(selectTab, new PguiTabGroupCtrl.OnSelectTab(this.OnSelectTab));
		this._lastTabIndex = selectTab;
		this.OnSelectTab(selectTab);
		if (itemWindow)
		{
			this._wildReleaseEffectPlaying = false;
			this.OpenItemWindow(this.lastOpenItemWindowParam.itemList, this.lastOpenItemWindowParam.isMulti, this.lastOpenItemWindowParam.index);
		}
	}

	public void SetDisable()
	{
		base.gameObject.SetActive(false);
		if (this._renderTextureChara != null)
		{
			Object.Destroy(this._renderTextureChara.gameObject);
		}
		this._renderTextureChara = null;
		this._guiData.MultiItemInfoWindow.owCtrl.gameObject.SetActive(false);
		this._guiData.SingleItemInfoWindow.owCtrl.gameObject.SetActive(false);
	}

	private void OnDestroy()
	{
		if (this._guiData != null)
		{
			Object.Destroy(this._guiData.BaseObj);
			this._guiData = null;
		}
		if (this._charaGrowWindow != null)
		{
			Object.Destroy(this._charaGrowWindow);
			this._charaGrowWindow = null;
		}
	}

	private void Update()
	{
		if (this._growMultiCoroutine != null && !this._growMultiCoroutine.MoveNext())
		{
			this._effectStatus = SelCharaGrowCtrl.EffectStatus.Finished;
			this._growMultiCoroutine = null;
			this.SetMaxInfo(this._guiData.CharacterGrowMain.Cmn.TabGroup.SelectIndex, false, false);
		}
		if (this._expGageEffect != null && !this._expGageEffect.MoveNext())
		{
			this._effectStatus = SelCharaGrowCtrl.EffectStatus.Finished;
			this._expGageEffect = null;
		}
		if (this._wildReleaseEffect != null && !this._wildReleaseEffect.MoveNext())
		{
			this._effectStatus = SelCharaGrowCtrl.EffectStatus.Finished;
			this._wildReleaseEffect = null;
		}
		if (this._rankUpGageEffect != null && !this._rankUpGageEffect.MoveNext())
		{
			this._effectStatus = SelCharaGrowCtrl.EffectStatus.Finished;
			this._rankUpGageEffect = null;
		}
		if (this._miracleEffect != null && !this._miracleEffect.MoveNext())
		{
			this._effectStatus = SelCharaGrowCtrl.EffectStatus.Finished;
			this._miracleEffect = null;
		}
		if (this._photoPocketEffect != null && !this._photoPocketEffect.MoveNext())
		{
			this._effectStatus = SelCharaGrowCtrl.EffectStatus.Finished;
			this._photoPocketEffect = null;
		}
		if (this._itemExchangeEffect != null && !this._itemExchangeEffect.MoveNext())
		{
			this._effectStatus = SelCharaGrowCtrl.EffectStatus.Finished;
			this._itemExchangeEffect = null;
		}
		if (this.levelLimitOverEffect != null && !this.levelLimitOverEffect.MoveNext())
		{
			this._effectStatus = SelCharaGrowCtrl.EffectStatus.Finished;
			this.levelLimitOverEffect = null;
		}
		if (this._ieKizunaLimitOver != null && !this._ieKizunaLimitOver.MoveNext())
		{
			this._effectStatus = SelCharaGrowCtrl.EffectStatus.Finished;
			this._ieKizunaLimitOver = null;
		}
		if (this._growthAchievementRewardEffect != null && !this._growthAchievementRewardEffect.MoveNext())
		{
			this._effectStatus = SelCharaGrowCtrl.EffectStatus.Finished;
			this._growthAchievementRewardEffect = null;
		}
		if (this._connectFlowBase != null && !this._connectFlowBase.MoveNext())
		{
			this._connectFlowBase = null;
		}
		if (this._ieReleaseAccessory != null && !this._ieReleaseAccessory.MoveNext())
		{
			this._effectStatus = SelCharaGrowCtrl.EffectStatus.Finished;
			this._ieReleaseAccessory = null;
		}
		if (this._nanairoReleaseEffect != null && !this._nanairoReleaseEffect.MoveNext())
		{
			this._effectStatus = SelCharaGrowCtrl.EffectStatus.Finished;
			this._nanairoReleaseEffect = null;
		}
		if (this._itemExchange != null && !this._itemExchange.MoveNext())
		{
			this._itemExchange = null;
		}
		SelCharaGrowKizuna.KizunaLevelUpEffectWindow kizunaLevelUpEffectWindow = this._charaGrowKizuna.GrowKizunaGUI.KizunaLevelUpEffectWindow;
		bool flag = !kizunaLevelUpEffectWindow.CheckKizunaWinCharaIsActive();
		if (this._renderTextureChara != null)
		{
			this._renderTextureChara.gameObject.SetActive(flag);
			if (kizunaLevelUpEffectWindow.CheckTouchIsActive())
			{
				kizunaLevelUpEffectWindow.UpdateKizunaLevelUpEffect(this._renderTextureChara);
			}
		}
		if (this._effectStatus == SelCharaGrowCtrl.EffectStatus.Finished)
		{
			this._effectStatus = SelCharaGrowCtrl.EffectStatus.None;
			CanvasManager.SetEnableCmnTouchMask(false);
		}
		this._guiData.CharacterGrowMain.Cmn.HaveGoldText.text = DataManager.DmItem.GetUserItemData(30101).num.ToString();
		if (this._currentCampaign != null)
		{
			bool flag2 = this._currentCampaign.IsDisplayCampaign && TimeManager.Now <= this._currentCampaign.endDatetime;
			this._guiData.CharacterGrowTop.CampaignInfo.gameObject.SetActive(flag2);
			this._guiData.CharacterGrowMain.CampaignInfo.gameObject.SetActive(flag2);
			if (flag2)
			{
				this._guiData.CharacterGrowTop.CampaignTimeInfo.text = TimeManager.MakeTimeResidueText(TimeManager.Now, this._currentCampaign.endDatetime, true, true);
				this._guiData.CharacterGrowMain.CampaignTimeInfo.text = TimeManager.MakeTimeResidueText(TimeManager.Now, this._currentCampaign.endDatetime, true, true);
			}
		}
		this._charaGrowMulti.UpdateGrowMulti(this);
	}

	private void SwitchCurrentTab(int index, bool isGrowMulti = false)
	{
		foreach (GameObject gameObject in this._guiData.CharacterGrowMain.TabObjectMap.Values)
		{
			gameObject.SetActive(false);
		}
		if (this._guiData.CharacterGrowMain.TabObjectMap.ContainsKey((SelCharaGrowCtrl.TabType)index))
		{
			this._guiData.CharacterGrowMain.TabObjectMap[(SelCharaGrowCtrl.TabType)index].SetActive(true);
		}
		DataManagerGameStatus.UserFlagData userFlagData = DataManager.DmGameStatus.MakeUserFlagData();
		DataManagerGameStatus.UserFlagData.CharaGrowTutorial charaGrowTutorialFlag = userFlagData.CharaGrowTutorialFlag;
		string text = null;
		int tipsId = 0;
		switch (index)
		{
		case 0:
			this.SelectLvUp();
			this._guiData.CharacterGrowMain.Cmn.TxtTabInfo.text = this._charaGrowLevelUp.TabInfoText(this._currentCharaId);
			this._guiData.CharacterGrowMain.Cmn.ButtonLText.text = this.ALL_CANCEL_TEXT;
			this._guiData.CharacterGrowMain.Cmn.ButtonRText.text = this.ENHANCEMENT_TEXT;
			if (!charaGrowTutorialFlag.LevelUp && !isGrowMulti)
			{
				charaGrowTutorialFlag.LevelUp = true;
				text = "Texture2D/Loading/Screenshot/Screenshots_13";
				tipsId = 13;
			}
			this._charaGrowLevelUp.LevelUpGUI.lvUpTab.ScrollView.Refresh();
			this._charaGrowLevelUp.LevelUpGUI.lvUpTab.ScrollView.ForceFocus(0);
			break;
		case 1:
		{
			this._guiData.CharacterGrowMain.Cmn.TxtTabInfo.text = this.TAB_INFO_WILD_RELEASE_TEXT;
			this._guiData.CharacterGrowMain.Cmn.ButtonLText.text = this.HOW_TO_GET_TEXT;
			this._guiData.CharacterGrowMain.Cmn.ButtonRText.text = this.ENHANCEMENT_TEXT;
			int num = (this._wildReleaseEffectPlaying ? this._charaGrowWild.currentIndexWild : 0);
			this.ChangeCurrentWildIcon(num);
			this._charaGrowWild.GrowWildGUI.wildReleaseTab.Result_Up.gameObject.SetActive(false);
			if (!charaGrowTutorialFlag.Yasei && !isGrowMulti)
			{
				charaGrowTutorialFlag.Yasei = true;
				text = "Texture2D/Loading/Screenshot/Screenshots_10";
				tipsId = 10;
			}
			break;
		}
		case 2:
			this._guiData.CharacterGrowMain.Cmn.TxtTabInfo.text = this.TAB_INFO_RANK_UP_TEXT;
			this._guiData.CharacterGrowMain.Cmn.ButtonLText.text = this.HOW_TO_GET_TEXT;
			this._guiData.CharacterGrowMain.Cmn.ButtonRText.text = this.ENHANCEMENT_TEXT;
			if (!charaGrowTutorialFlag.RankUp && !isGrowMulti)
			{
				charaGrowTutorialFlag.RankUp = true;
				text = "Texture2D/Loading/Screenshot/Screenshots_09";
				tipsId = 9;
			}
			break;
		case 3:
			this._guiData.CharacterGrowMain.Cmn.TxtTabInfo.text = this.TAB_INFO_MIRACLE_UP_TEXT;
			this._guiData.CharacterGrowMain.Cmn.ButtonLText.text = this.HOW_TO_GET_TEXT;
			this._guiData.CharacterGrowMain.Cmn.ButtonRText.text = this.ENHANCEMENT_TEXT;
			if (!charaGrowTutorialFlag.Miracle && !isGrowMulti)
			{
				charaGrowTutorialFlag.Miracle = true;
				text = "Texture2D/Loading/Screenshot/Screenshots_14";
				tipsId = 14;
			}
			this._guiData.CharacterGrowMain.Cmn.TabGuiList[3].TabCtrl.SetActEnable(true);
			break;
		case 4:
			this._guiData.CharacterGrowMain.Cmn.TxtTabInfo.text = this.TAB_INFO_NANAIRO_TEXT;
			this._guiData.CharacterGrowMain.Cmn.ButtonLText.text = this.HOW_TO_GET_TEXT;
			this._guiData.CharacterGrowMain.Cmn.ButtonRText.text = this.RELEASE_TEXT;
			if (!charaGrowTutorialFlag.Nanairo && !isGrowMulti)
			{
				charaGrowTutorialFlag.Nanairo = true;
				text = "Texture2D/Loading/Screenshot/Screenshots_31";
				tipsId = 31;
			}
			this._guiData.CharacterGrowMain.Cmn.TabGuiList[4].TabCtrl.SetActEnable(true);
			break;
		case 5:
			this._guiData.CharacterGrowMain.Cmn.TxtTabInfo.text = this.TAB_INFO_PHOTO_POCKET_TEXT;
			this._guiData.CharacterGrowMain.Cmn.ButtonLText.text = this.HOW_TO_GET_TEXT;
			this._guiData.CharacterGrowMain.Cmn.ButtonRText.text = this.ENHANCEMENT_TEXT;
			if (!charaGrowTutorialFlag.Photo && !isGrowMulti)
			{
				charaGrowTutorialFlag.Photo = true;
				text = "Texture2D/Loading/Screenshot/Screenshots_12";
				tipsId = 12;
			}
			break;
		case 6:
			this.SelectKizunaLvUp();
			this._guiData.CharacterGrowMain.Cmn.TxtTabInfo.text = this._charaGrowKizuna.TabInfoText(this._currentCharaId);
			this._guiData.CharacterGrowMain.Cmn.ButtonLText.text = this.ALL_CANCEL_TEXT;
			this._guiData.CharacterGrowMain.Cmn.ButtonRText.text = this.ENHANCEMENT_TEXT;
			if (!charaGrowTutorialFlag.Kizuna && !isGrowMulti)
			{
				charaGrowTutorialFlag.Kizuna = true;
			}
			this._charaGrowKizuna.GrowKizunaGUI.KizunaLvUpTab.ScrollView.Refresh();
			this._charaGrowKizuna.GrowKizunaGUI.KizunaLvUpTab.ScrollView.ForceFocus(0);
			break;
		}
		if (text != null)
		{
			DataManagerServerMst dmServerMst = DataManager.DmServerMst;
			MstTipsData mstTipsData = ((dmServerMst != null) ? dmServerMst.mstTipsDataList.Find((MstTipsData item) => item.id == tipsId) : null);
			string text2 = ((mstTipsData != null) ? mstTipsData.title : "");
			CanvasManager.HdlCmnFeedPageWindowCtrl.Open(CmnFeedPageWindowCtrl.Type.PAGE_FEED, text2, new List<string> { text }, null);
			DataManager.DmGameStatus.RequestActionUpdateUserFlag(userFlagData);
		}
		this.SetActStrengthButton(index, isGrowMulti);
		this.SetOwnCoin(index);
		this.SetMaxInfo(index, isGrowMulti, false);
	}

	private void SelectLvUp()
	{
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(this._currentCharaId);
		this._guiData.CharacterGrowMain.UpdateCharaParameter(userCharaData);
		SelCharaGrowLevel.LvUpTab lvUpTab = this._charaGrowLevelUp.LevelUpGUI.lvUpTab;
		bool flag = this._selectLvUpItemIdList.Count > 0;
		bool flag2 = this._effectStatus == SelCharaGrowCtrl.EffectStatus.Execute || this._effectStatus == SelCharaGrowCtrl.EffectStatus.ReqServer;
		lvUpTab.Num_Lv_L.gameObject.SetActive(flag || flag2);
		lvUpTab.Num_Lv_R.gameObject.SetActive(true);
		lvUpTab.Img_Yaji.gameObject.SetActive(flag || flag2);
		lvUpTab.Gage_Up.gameObject.SetActive(flag);
		SelCharaGrowLevel.WindowLvUp lvUpWindow = this._charaGrowLevelUp.LevelUpGUI.lvUpWindow;
		lvUpWindow.itemList = new List<ItemInput>();
		List<ItemData> expAddItemList = DataManager.DmItem.GetUserItemListByKind(ItemDef.Kind.EXP_ADD);
		int i;
		Predicate<int> <>9__0;
		int num;
		for (i = 0; i < expAddItemList.Count; i = num)
		{
			List<int> selectLvUpItemIdList = this._selectLvUpItemIdList;
			Predicate<int> predicate;
			if ((predicate = <>9__0) == null)
			{
				predicate = (<>9__0 = (int n) => n == expAddItemList[i].id);
			}
			List<int> list = selectLvUpItemIdList.FindAll(predicate);
			if (list.Count != 0)
			{
				ItemInput itemInput = new ItemInput(expAddItemList[i].id, list.Count);
				lvUpWindow.itemList.Add(itemInput);
			}
			num = i + 1;
		}
		lvUpWindow.ScrollView.ResizeFocesNoMove((lvUpWindow.itemList.Count - 1) / 5 + 1);
		DataManagerChara.SimulateAddExpResult simulateAddExpResult = new DataManagerChara.SimulateAddExpResult();
		simulateAddExpResult = DataManager.DmChara.SimulateAddExp(DataManager.DmChara.GetUserCharaData(this._currentCharaId), lvUpWindow.itemList);
		CharaDynamicData dynamicData = userCharaData.dynamicData;
		lvUpWindow.Num_Lv_Before.text = (lvUpTab.Num_Lv_L.text = this.DEFAULT_LEVEL_TEXT + dynamicData.level.ToString() + "/" + dynamicData.limitLevel.ToString());
		string text = this.DEFAULT_LEVEL_TEXT + ((simulateAddExpResult.level > dynamicData.level) ? ("<color=#FF7B16FF>" + simulateAddExpResult.level.ToString() + "</color>") : simulateAddExpResult.level.ToString());
		lvUpWindow.Num_Lv_After.text = (lvUpTab.Num_Lv_R.text = text + "/" + dynamicData.limitLevel.ToString());
		lvUpWindow.Gage_Up.m_Image.fillAmount = (lvUpTab.Gage_Up.m_Image.fillAmount = (float)simulateAddExpResult.exp / (float)DataManager.DmChara.GetExpByNextLevel(dynamicData.id, simulateAddExpResult.level));
		lvUpTab.SetActiveGage(dynamicData.level >= simulateAddExpResult.level);
		lvUpWindow.SetActiveGage(dynamicData.level >= simulateAddExpResult.level);
		long num2 = ((simulateAddExpResult.level >= dynamicData.limitLevel) ? 0L : (DataManager.DmChara.GetExpByNextLevel(dynamicData.id, simulateAddExpResult.level) - simulateAddExpResult.exp));
		lvUpWindow.Num_Exp_Next.text = (lvUpTab.Num_Exp_Next.text = this.NUM_EXP_NEXT_TEXT + num2.ToString());
		this._lvUpCostCoin = simulateAddExpResult.costGold;
		this._saExpResult = simulateAddExpResult;
		if (simulateAddExpResult.level >= dynamicData.limitLevel)
		{
			lvUpWindow.Gage_Up.m_Image.fillAmount = (lvUpTab.Gage_Up.m_Image.fillAmount = 1f);
		}
		lvUpWindow.Gage.m_Image.fillAmount = (lvUpTab.Gage.m_Image.fillAmount = (float)dynamicData.exp / (float)DataManager.DmChara.GetExpByNextLevel(dynamicData.id, dynamicData.level));
		if (simulateAddExpResult.level >= dynamicData.limitLevel)
		{
			lvUpWindow.Gage.m_Image.fillAmount = (lvUpTab.Gage.m_Image.fillAmount = 1f);
		}
		using (List<SelCharaGrowLevel.LvUpItem>.Enumerator enumerator = lvUpTab.iconItemList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				SelCharaGrowLevel.LvUpItem item = enumerator.Current;
				if (item.itemId != 0)
				{
					List<int> list2 = (flag ? this._selectLvUpItemIdList.FindAll((int n) => n == item.itemId) : this._selectLvUpItemIdList);
					item.imgCount.gameObject.SetActive(0 < list2.Count);
					item.itemCount.text = list2.Count.ToString();
					int num3 = DataManager.DmItem.GetUserItemData(item.itemId).num;
					int needGold = ItemDef.GetAddCharaLevelExpBase(item.itemId, userCharaData.staticData.baseData.attribute).needGold;
					int num4 = DataManager.DmItem.GetUserItemData(30101).num;
					bool flag3 = true;
					if (num3 == 0 || list2.Count == num3 || simulateAddExpResult.level >= dynamicData.limitLevel || this._lvUpCostCoin + needGold > num4)
					{
						flag3 = false;
					}
					item.iconItemCtrl.SetActEnable(flag3);
				}
			}
		}
	}

	private bool CheckLvUp(int addItemId)
	{
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(this._currentCharaId);
		new DataManagerChara.SimulateAddExpResult();
		List<ItemInput> list = new List<ItemInput>();
		foreach (int num in this._selectLvUpItemIdList)
		{
			ItemInput itemInput = new ItemInput(num, 1);
			list.Add(itemInput);
		}
		if (DataManager.DmChara.SimulateAddExp(DataManager.DmChara.GetUserCharaData(this._currentCharaId), list).level >= userCharaData.dynamicData.limitLevel)
		{
			return false;
		}
		ItemInput itemInput2 = new ItemInput(addItemId, 1);
		list.Add(itemInput2);
		return DataManager.DmChara.SimulateAddExp(DataManager.DmChara.GetUserCharaData(this._currentCharaId), list).level <= userCharaData.dynamicData.limitLevel;
	}

	private bool CheckKizunaLvUp(int addItemId)
	{
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(this._currentCharaId);
		new DataManagerChara.SimulateAddExpResult();
		List<ItemInput> list = new List<ItemInput>();
		foreach (int num in this._selectKizunaLvUpItemIdList)
		{
			ItemInput itemInput = new ItemInput(num, 1);
			list.Add(itemInput);
		}
		if (DataManager.DmChara.SimulateAddKizunaExp(DataManager.DmChara.GetUserCharaData(this._currentCharaId), list).level >= userCharaData.dynamicData.KizunaLimitLevel)
		{
			return false;
		}
		ItemInput itemInput2 = new ItemInput(addItemId, 1);
		list.Add(itemInput2);
		return DataManager.DmChara.SimulateAddKizunaExp(DataManager.DmChara.GetUserCharaData(this._currentCharaId), list).level <= userCharaData.dynamicData.KizunaLimitLevel;
	}

	private void SelectKizunaLvUp()
	{
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(this._currentCharaId);
		SelCharaGrowKizuna.KizunaLvUpTab kizunaLvUpTab = this._charaGrowKizuna.GrowKizunaGUI.KizunaLvUpTab;
		CharaDynamicData dynamicData = userCharaData.dynamicData;
		SelCharaGrowKizuna.WindowKizunaLvUp kizunaLvUpWindow = this._charaGrowKizuna.GrowKizunaGUI.KizunaLvUpWindow;
		DataManagerChara.SimulateAddExpResult simulateAddExpResult = new DataManagerChara.SimulateAddExpResult();
		bool flag = this._selectKizunaLvUpItemIdList.Count > 0;
		bool flag2 = this._effectStatus == SelCharaGrowCtrl.EffectStatus.Execute || this._effectStatus == SelCharaGrowCtrl.EffectStatus.ReqServer;
		this._charaGrowKizuna.GrowKizunaGUI.SetActiveCtrl(flag, flag2);
		List<ItemInput> list = new List<ItemInput>();
		List<ItemData> expAddItemList = DataManager.DmItem.GetUserItemListByKind(ItemDef.Kind.EXP_ADD);
		int i;
		Predicate<int> <>9__0;
		int num;
		for (i = 0; i < expAddItemList.Count; i = num)
		{
			List<int> selectKizunaLvUpItemIdList = this._selectKizunaLvUpItemIdList;
			Predicate<int> predicate;
			if ((predicate = <>9__0) == null)
			{
				predicate = (<>9__0 = (int n) => n == expAddItemList[i].id);
			}
			List<int> list2 = selectKizunaLvUpItemIdList.FindAll(predicate);
			if (list2.Count != 0)
			{
				ItemInput itemInput = new ItemInput(expAddItemList[i].id, list2.Count);
				list.Add(itemInput);
			}
			num = i + 1;
		}
		kizunaLvUpWindow.SetItemImputList(list);
		kizunaLvUpWindow.ScrollView.ResizeFocesNoMove((list.Count - 1) / 5 + 1);
		simulateAddExpResult = DataManager.DmChara.SimulateAddKizunaExp(userCharaData, list);
		int level = simulateAddExpResult.level;
		int kizunaLimitLevel = dynamicData.KizunaLimitLevel;
		int kizunaLevel = dynamicData.kizunaLevel;
		bool flag3 = kizunaLevel >= level;
		bool flag4 = level >= kizunaLimitLevel;
		int id = dynamicData.id;
		string text = this.KIZUNA_LEVEL_TEXT + kizunaLevel.ToString() + "/" + kizunaLimitLevel.ToString();
		string text2 = this.KIZUNA_LEVEL_TEXT + ((level > kizunaLevel) ? ("<color=#FF7B16FF>" + level.ToString() + "</color>") : level.ToString()) + "/" + kizunaLimitLevel.ToString();
		kizunaLvUpTab.SetActiveGage(flag3);
		kizunaLvUpWindow.SetActiveGage(flag3);
		long num2 = (flag4 ? 0L : (DataManager.DmChara.GetKizunaExpForNextLevel(id, level) - simulateAddExpResult.exp));
		string text3 = this.NEXT_KIZUNA_LEVEL_DESCRIPTION_TEXT + num2.ToString();
		kizunaLvUpWindow.SetText(text, text2, text3);
		kizunaLvUpTab.SetText(text, text2, text3);
		float num3 = (float)simulateAddExpResult.exp / (float)DataManager.DmChara.GetExpByNextKizunaLevel(id, simulateAddExpResult.level);
		kizunaLvUpWindow.SetGageUpImageFillAmount(num3);
		kizunaLvUpTab.SetGageUpImageFillAmount(num3);
		this._lvUpCostCoin = simulateAddExpResult.costGold;
		this._saExpResult = simulateAddExpResult;
		if (flag4)
		{
			kizunaLvUpWindow.SetGageUpImageFillAmount(1f);
			kizunaLvUpTab.SetGageUpImageFillAmount(1f);
		}
		num3 = (float)dynamicData.kizunaExp / (float)DataManager.DmChara.GetExpByNextKizunaLevel(id, kizunaLevel);
		kizunaLvUpWindow.SetGageImageFillAmount(num3);
		kizunaLvUpTab.SetGageImageFillAmount(num3);
		if (flag4)
		{
			kizunaLvUpWindow.SetGageImageFillAmount(1f);
			kizunaLvUpTab.SetGageImageFillAmount(1f);
		}
		using (List<SelCharaGrowKizuna.KizunaLvUpItem>.Enumerator enumerator = kizunaLvUpTab.IconItemList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				SelCharaGrowKizuna.KizunaLvUpItem item = enumerator.Current;
				if (item.ItemId != 0)
				{
					List<int> list3 = (flag ? this._selectKizunaLvUpItemIdList.FindAll((int n) => n == item.ItemId) : this._selectKizunaLvUpItemIdList);
					item.SetActiveImageCountGameObject(0 < list3.Count);
					item.SetTextItemCount(list3.Count.ToString());
					int num4 = DataManager.DmItem.GetUserItemData(item.ItemId).num;
					int needGold = ItemDef.GetAddCharaLevelExpBase(item.ItemId, userCharaData.staticData.baseData.attribute).needGold;
					int num5 = DataManager.DmItem.GetUserItemData(30101).num;
					bool flag5 = true;
					if (num4 == 0 || list3.Count == num4 || simulateAddExpResult.level >= dynamicData.KizunaLimitLevel || this._lvUpCostCoin + needGold > num5)
					{
						flag5 = false;
					}
					item.IconItemCtrl.SetActEnable(flag5);
				}
			}
		}
	}

	private void SetOwnCoin(int tabIndex)
	{
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(this._currentCharaId);
		int num = 0;
		switch (tabIndex)
		{
		case 0:
		case 6:
			num = this._lvUpCostCoin;
			break;
		case 1:
		{
			CharaPromoteOne charaPromoteOne = userCharaData.staticData.promoteList[this._charaGrowWild.GetPromoteNum(this._currentCharaId)].promoteOneList[this._charaGrowWild.currentIndexWild];
			num = ((!userCharaData.dynamicData.promoteFlag[this._charaGrowWild.currentIndexWild]) ? charaPromoteOne.costGoldNum : 0);
			break;
		}
		case 2:
		{
			GrowItemData nextItemByRankup = userCharaData.GetNextItemByRankup(0);
			num = ((nextItemByRankup != null) ? nextItemByRankup.needGold : 0);
			break;
		}
		case 3:
		{
			GrowItemList nextItemByArtsUp = userCharaData.GetNextItemByArtsUp(0);
			num = ((nextItemByArtsUp != null) ? nextItemByArtsUp.needGold : 0);
			break;
		}
		}
		this._guiData.CharacterGrowMain.Cmn.NeedGoldText.text = ((num != 0) ? num.ToString() : "-");
		ItemData userItemData = DataManager.DmItem.GetUserItemData(30101);
		this._guiData.CharacterGrowMain.Cmn.HaveGoldText.m_Text.color = ((userItemData.num < num) ? Color.red : Color.white);
	}

	private void RefParam(out string title, out string rateInfo, out int rate, out ItemData leftItem, out ItemData rightItem, out ItemStaticBase leftItemStaticBase, out ItemStaticBase rightItemStaticData, int index, int holdStone)
	{
		CharaPackData charaPackData = DataManager.DmChara.GetUserCharaData(this._currentCharaId);
		if (index == 5)
		{
			title = this.PHOTO_POCKET_EXCHANGE_TITLE_TEXT;
			MstCharaRankData mstCharaRankData = DataManager.DmServerMst.mstCharaRankDataList.Find((MstCharaRankData x) => x.rankId == charaPackData.staticData.baseData.rankId && x.rank == charaPackData.staticData.baseData.rankLow);
			rate = mstCharaRankData.pxExRate;
			rightItem = DataManager.DmItem.GetUserItemData(30104);
			rateInfo = PrjUtil.MakeMessage(string.Concat(new string[]
			{
				"輝石 1 個 → ",
				rightItem.staticData.GetName(),
				" ",
				rate.ToString(),
				" 個"
			}));
			leftItem = DataManager.DmItem.GetUserItemData(charaPackData.staticData.baseData.ppItemId);
		}
		else
		{
			title = this.OTHER_PHOTO_POCKET_EXCHANGE_TITLE_TEXT;
			rightItem = DataManager.DmItem.GetUserItemData(30119);
			rate = 1;
			rateInfo = PrjUtil.MakeMessage(string.Concat(new string[]
			{
				"小さな輝石 1 個 → ",
				rightItem.staticData.GetName(),
				" ",
				rate.ToString(),
				" 個"
			}));
			ItemData userItemData = DataManager.DmItem.GetUserItemData(charaPackData.staticData.baseData.rankItemId);
			leftItem = new ItemData(userItemData.id, userItemData.num - holdStone);
		}
		leftItemStaticBase = leftItem.staticData;
		rightItemStaticData = rightItem.staticData;
	}

	private void SetMaxInfo(int tabIdx, bool notDispMax = false, bool forceLevelLimitOver = false)
	{
		Action<ItemData, ItemData, int, int, string> action = delegate(ItemData leftItem, ItemData rightItem, int rate, int holdStone, string rateInfo)
		{
			this._guiData.ItemExchangeWindow.Txt_Title.text = this.EXCHANGE_CONFIRM_TEXT;
			this._guiData.ItemExchangeWindow.Left_ItemIconCtrl.Setup(leftItem.staticData);
			this._guiData.ItemExchangeWindow.Left_Name.text = leftItem.staticData.GetName();
			this._guiData.ItemExchangeWindow.Left_Num.text = leftItem.num.ToString();
			this._guiData.ItemExchangeWindow.Left_Txt.text = ((tabIdx == 5) ? this.POSSESSION_NUMBER_TEXT : this.POSSIBLE_EXCHANGE_NUMBER_TEXT);
			this._guiData.ItemExchangeWindow.Right_ItemIconCtrl.Setup(rightItem.staticData);
			this._guiData.ItemExchangeWindow.Right_Name.text = rightItem.staticData.GetName();
			this._guiData.ItemExchangeWindow.Right_Num.text = (leftItem.num * rate).ToString();
			this._guiData.ItemExchangeWindow.Left_IconTex.SetRawImage((tabIdx == 5) ? "Texture2D/Icon_Item/icon_item_gembase01" : "Texture2D/Icon_Item/icon_item_gembase02", true, false, null);
			this._guiData.ItemExchangeWindow.Left_Category.text = PrjUtil.MakeMessage((tabIdx == 5) ? this.GEMSTONE_TEXT : this.SMALL_GEMSTONE_TEXT);
			this._guiData.ItemExchangeWindow.Left_BeforeNum.text = (holdStone + leftItem.num).ToString();
			this._guiData.ItemExchangeWindow.Left_AfterNum.text = holdStone.ToString();
			this._guiData.ItemExchangeWindow.Right_IconTex.SetRawImage(rightItem.staticData.GetIconName(), true, false, null);
			this._guiData.ItemExchangeWindow.Right_Category.text = PrjUtil.MakeMessage(rightItem.staticData.GetName());
			this._guiData.ItemExchangeWindow.Right_BeforeNum.text = rightItem.num.ToString();
			this._guiData.ItemExchangeWindow.Right_AfterNum.text = (rightItem.num + leftItem.num * rate).ToString();
			this._guiData.ItemExchangeWindow.Txt_RateInfo.text = rateInfo;
		};
		CharaPackData charaPackData = DataManager.DmChara.GetUserCharaData(this._currentCharaId);
		string text = "";
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		SelCharaGrowCtrl.TabType tabIdx2 = (SelCharaGrowCtrl.TabType)tabIdx;
		int kizunaLevel = charaPackData.dynamicData.kizunaLevel;
		switch (tabIdx2)
		{
		case SelCharaGrowCtrl.TabType.LevelUp:
			if (this._charaGrowLevelUp.CanLevelLimitOver(this._currentCharaId) || forceLevelLimitOver)
			{
				flag3 = true;
			}
			else if (charaPackData.dynamicData.level == charaPackData.dynamicData.limitLevel)
			{
				text = this.LEVEL_UP_MAX_INFO_MESSAGE_TEXT;
				flag = true;
			}
			this._guiData.CharacterGrowMain.Cmn.TxtTabInfo.text = this._charaGrowLevelUp.TabInfoText(this._currentCharaId);
			break;
		case SelCharaGrowCtrl.TabType.WildRelease:
			if (charaPackData.dynamicData.promoteNum == charaPackData.staticData.maxPromoteNum)
			{
				text = this.WILD_RELEASE_MAX_INFO_MESSAGE_TEXT;
				flag = true;
			}
			break;
		case SelCharaGrowCtrl.TabType.RankUp:
			if (charaPackData.dynamicData.rank == charaPackData.staticData.baseData.rankHigh)
			{
				text = this.RANK_UP_MAX_INFO_MESSAGE_TEXT;
				flag = true;
			}
			break;
		case SelCharaGrowCtrl.TabType.MiracleUp:
			if (charaPackData.dynamicData.artsLevel == 5)
			{
				text = this.MIRACLEUP_MAX_INFO_MESSAGE_TEXT;
				flag = true;
			}
			break;
		case SelCharaGrowCtrl.TabType.Nanairo:
			if (charaPackData.dynamicData.nanairoAbilityReleaseFlag)
			{
				text = this.NANAIRO_UP_MAX_INFO_MESSAGE_TEXT;
				flag = true;
			}
			break;
		case SelCharaGrowCtrl.TabType.PhotoPocket:
			if (charaPackData.dynamicData.PhotoFrameTotalStep >= DataManager.DmServerMst.StaticCharaPpDataMap[charaPackData.staticData.baseData.photoFrameTableId].PpStepMax)
			{
				text = this.PHOTO_POCKET_MAX_INFO_MESSAGE_TEXT;
				flag = true;
			}
			else if (charaPackData.dynamicData.PhotoPocket[SelCharaGrowPhotoPocket.WindowPhotoPocket.COUNT - 1].Flag)
			{
				flag2 = true;
			}
			break;
		case SelCharaGrowCtrl.TabType.Kizuna:
		{
			int nowLimitLevel = charaPackData.dynamicData.KizunaLimitLevel;
			bool flag5 = nowLimitLevel != kizunaLevel;
			if (DataManager.DmServerMst.gameLevelInfoList.Find((GameLevelInfo x) => x.level == nowLimitLevel + 1).kizunaLevelExp.ContainsKey(charaPackData.staticData.baseData.kizunaLevelId))
			{
				flag5 = true;
			}
			flag4 = nowLimitLevel == kizunaLevel || forceLevelLimitOver;
			if (!flag5)
			{
				text = this.KIZUNA_UP_MAX_INFO_MESSAGE_TEXT;
				flag = true;
			}
			break;
		}
		}
		this._guiData.CharacterGrowMain.PhotoMax.BaseObj.SetActive(false);
		this._guiData.CharacterGrowMain.PhotoPocketConversionStrengthen.BaseObj.SetActive(false);
		this._guiData.CharacterGrowMain.LvLimitOpen.baseObj.SetActive(false);
		this._charaGrowKizuna.GrowKizunaGUI.KizunaLvUpTab.BaseObj.SetActive(false);
		if (flag && !notDispMax)
		{
			if (tabIdx2 != SelCharaGrowCtrl.TabType.Kizuna)
			{
				this._guiData.CharacterGrowMain.TabObjectMap[(SelCharaGrowCtrl.TabType)tabIdx].SetActive(false);
			}
			this._guiData.CharacterGrowMain.TxtMaxInfo.gameObject.SetActive(true);
			this._guiData.CharacterGrowMain.TxtMaxInfo.text = text;
			this._guiData.CharacterGrowMain.MarkMax.gameObject.SetActive(true);
			this._guiData.CharacterGrowMain.Cmn.NeedGoldText.text = "-";
		}
		else if (flag2 && !notDispMax)
		{
			this._guiData.CharacterGrowMain.TabObjectMap[(SelCharaGrowCtrl.TabType)tabIdx].SetActive(false);
			this._guiData.CharacterGrowMain.TxtMaxInfo.gameObject.SetActive(false);
			this._guiData.CharacterGrowMain.MarkMax.gameObject.SetActive(false);
		}
		else if (flag3 && !notDispMax)
		{
			this._guiData.CharacterGrowMain.TabObjectMap[(SelCharaGrowCtrl.TabType)tabIdx].SetActive(false);
			this._guiData.CharacterGrowMain.TxtMaxInfo.gameObject.SetActive(false);
			this._guiData.CharacterGrowMain.MarkMax.gameObject.SetActive(false);
			DataManagerChara.LevelLimitData levelLimitData = DataManager.DmChara.GetLevelLimitData(charaPackData.dynamicData.levelLimitId + 1);
			int num = DataManager.DmItem.GetUserItemData(30101).num;
			this._guiData.CharacterGrowMain.Cmn.NeedGoldText.text = ((levelLimitData == null || (levelLimitData != null && levelLimitData.needGoldNum == 0)) ? "-" : string.Format("{0}", levelLimitData.needGoldNum));
			this._guiData.CharacterGrowMain.Cmn.HaveGoldText.m_Text.color = ((levelLimitData != null && num < levelLimitData.needGoldNum) ? Color.red : Color.white);
		}
		else
		{
			if (this._guiData.CharacterGrowMain.TabObjectMap.ContainsKey((SelCharaGrowCtrl.TabType)tabIdx))
			{
				this._guiData.CharacterGrowMain.TabObjectMap[(SelCharaGrowCtrl.TabType)tabIdx].SetActive(true);
			}
			this._guiData.CharacterGrowMain.TxtMaxInfo.gameObject.SetActive(false);
			this._guiData.CharacterGrowMain.MarkMax.gameObject.SetActive(false);
		}
		bool flag6 = flag3 || (6 == tabIdx && flag4);
		this._guiData.CharacterGrowMain.Cmn.ButtonL.gameObject.SetActive(!flag6);
		this._guiData.CharacterGrowMain.Cmn.ButtonR.gameObject.SetActive(!flag6);
		if (flag && (tabIdx == 5 || tabIdx == 2) && !notDispMax)
		{
			int holdStone = 0;
			if (tabIdx == 2)
			{
				this._guiData.CharacterGrowMain.PhotoMax.BaseObj.SetActive(false);
				DataManager.DmServerMst.mstCharaRankDataList.FindAll((MstCharaRankData item) => item.rankId == charaPackData.staticData.baseData.gradeTableId && item.rank > charaPackData.dynamicData.rank).ForEach(delegate(MstCharaRankData item)
				{
					holdStone += item.useCostFragmentNum;
				});
				if (DataManager.DmItem.GetUserItemData(charaPackData.staticData.baseData.rankItemId).num - holdStone <= 0)
				{
					return;
				}
			}
			this._guiData.CharacterGrowMain.TxtMaxInfo.gameObject.SetActive(false);
			this._guiData.CharacterGrowMain.PhotoMax.TxtMaxInfo.text = text;
			this._guiData.CharacterGrowMain.PhotoMax.BaseObj.SetActive(true);
			string text2;
			string text3;
			int num2;
			ItemData itemData;
			ItemData itemData2;
			ItemStaticBase itemStaticBase;
			ItemStaticBase itemStaticBase2;
			this.RefParam(out text2, out text3, out num2, out itemData, out itemData2, out itemStaticBase, out itemStaticBase2, tabIdx, holdStone);
			this._guiData.CharacterGrowMain.PhotoMax.ItemConversion.Title.text = text2;
			string text4 = ((tabIdx == 5) ? this.POSSESSION_NUMBER_TEXT : this.POSSIBLE_EXCHANGE_NUMBER_TEXT);
			this._guiData.CharacterGrowMain.PhotoMax.ItemConversion.ItemBoxeMap[SelCharaGrowCtrl.CommonGUI.ItemConversion.ItemPosition.Left].Setup(new SelCharaGrowCtrl.CommonGUI.ItemConversion.ItemBox.SetupParam(itemData, text4, itemData.num, itemStaticBase));
			text4 = this.EXCHANGE_NUMBER_TEXT;
			this._guiData.CharacterGrowMain.PhotoMax.ItemConversion.ItemBoxeMap[SelCharaGrowCtrl.CommonGUI.ItemConversion.ItemPosition.Right].Setup(new SelCharaGrowCtrl.CommonGUI.ItemConversion.ItemBox.SetupParam(itemData2, text4, itemData.num * num2, itemStaticBase2));
			this._guiData.CharacterGrowMain.PhotoMax.ItemConversion.ButtonC.SetActEnable(itemData.num > 0, false, false);
			this._guiData.CharacterGrowMain.PhotoMax.ItemConversion.TxtRateInfo.text = text3;
			action(itemData, itemData2, num2, holdStone, text3);
			return;
		}
		if (flag2 && tabIdx == 5 && !notDispMax)
		{
			int num3 = 0;
			this._guiData.CharacterGrowMain.TxtMaxInfo.gameObject.SetActive(false);
			this._guiData.CharacterGrowMain.PhotoPocketConversionStrengthen.BaseObj.SetActive(true);
			string text5;
			string text6;
			int num4;
			ItemData itemData3;
			ItemData itemData4;
			ItemStaticBase itemStaticBase3;
			ItemStaticBase itemStaticBase4;
			this.RefParam(out text5, out text6, out num4, out itemData3, out itemData4, out itemStaticBase3, out itemStaticBase4, tabIdx, num3);
			this._guiData.CharacterGrowMain.PhotoPocketConversionStrengthen.ItemConversion.Title.text = text5;
			string text7 = ((tabIdx == 5) ? this.POSSESSION_NUMBER_TEXT : this.POSSIBLE_EXCHANGE_NUMBER_TEXT);
			this._guiData.CharacterGrowMain.PhotoPocketConversionStrengthen.ItemConversion.ItemBoxeMap[SelCharaGrowCtrl.CommonGUI.ItemConversion.ItemPosition.Left].Setup(new SelCharaGrowCtrl.CommonGUI.ItemConversion.ItemBox.SetupParam(itemData3, text7, itemData3.num, itemStaticBase3));
			text7 = this.EXCHANGE_NUMBER_TEXT;
			this._guiData.CharacterGrowMain.PhotoPocketConversionStrengthen.ItemConversion.ItemBoxeMap[SelCharaGrowCtrl.CommonGUI.ItemConversion.ItemPosition.Right].Setup(new SelCharaGrowCtrl.CommonGUI.ItemConversion.ItemBox.SetupParam(itemData4, text7, itemData3.num * num4, itemStaticBase4));
			this._guiData.CharacterGrowMain.PhotoPocketConversionStrengthen.ItemConversion.ButtonC.SetActEnable(itemData3.num > 0, false, false);
			this._guiData.CharacterGrowMain.PhotoPocketConversionStrengthen.ItemConversion.TxtRateInfo.text = text6;
			action(itemData3, itemData4, num4, num3, text6);
			return;
		}
		if (flag3 && tabIdx == 0 && !notDispMax)
		{
			this._guiData.CharacterGrowMain.TxtMaxInfo.gameObject.SetActive(false);
			int num5 = ((this._growMultiCoroutine != null) ? this._charaGrowMulti.GrowMultiGUI.growMultiCheckWindow.afterLevelLimitId : (charaPackData.dynamicData.levelLimitId + 1));
			DataManagerChara.LevelLimitData levelLimitData2 = DataManager.DmChara.GetLevelLimitData(num5);
			if (levelLimitData2 != null)
			{
				this._guiData.CharacterGrowMain.LvLimitOpen.baseObj.SetActive(true);
				this._guiData.CharacterGrowMain.LvLimitOpen.Setup(new SelCharaGrowLevel.LvLimitOpen.SetupParam
				{
					levelLimitData = levelLimitData2,
					diffLevel = levelLimitData2.maxLevel - charaPackData.dynamicData.level
				});
				return;
			}
		}
		else if (tabIdx == 6 && !notDispMax)
		{
			this._charaGrowKizuna.SetActiveTab(flag4);
		}
	}

	private void SetActStrengthButton(int index, bool isGrowMulti = false)
	{
		this._guiData.CharacterGrowMain.Cmn.ButtonL.gameObject.SetActive(true);
		this._guiData.CharacterGrowMain.Cmn.ButtonR.gameObject.SetActive(true);
		this._guiData.CharacterGrowMain.Cmn.ButtonRExchange.gameObject.SetActive(false);
		switch (index)
		{
		case 0:
		{
			this._charaGrowLevelUp.UpdateItemLvUp();
			bool flag = this._lvUpCostCoin <= DataManager.DmItem.GetUserItemData(30101).num && DataManager.DmItem.GetUserItemData(30101).num > 0 && this._selectLvUpItemIdList.Count > 0;
			this._guiData.CharacterGrowMain.Cmn.ButtonR.SetActEnable(flag, false, false);
			this._guiData.CharacterGrowMain.Cmn.ButtonL.SetActEnable(flag, false, false);
			return;
		}
		case 1:
		{
			SelCharaGrowWild.WildReleaseTab wildReleaseTab = this._charaGrowWild.GrowWildGUI.wildReleaseTab;
			CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(this._currentCharaId);
			CharaDynamicData dynamicData = userCharaData.dynamicData;
			CharaStaticData staticData = userCharaData.staticData;
			CharaPromoteOne charaPromoteOne = staticData.promoteList[this._charaGrowWild.GetPromoteNum(this._currentCharaId)].promoteOneList[this._charaGrowWild.currentIndexWild];
			ItemData userItemData = DataManager.DmItem.GetUserItemData(charaPromoteOne.promoteUseItemId);
			bool flag2 = charaPromoteOne.promoteUseItemNum <= userItemData.num && !dynamicData.promoteFlag[this._charaGrowWild.currentIndexWild] && charaPromoteOne.costGoldNum <= DataManager.DmItem.GetUserItemData(30101).num;
			this._guiData.CharacterGrowMain.Cmn.ButtonR.SetActEnable(flag2 && dynamicData.promoteNum < staticData.maxPromoteNum, false, false);
			this._guiData.CharacterGrowMain.Cmn.ButtonL.SetActEnable(dynamicData.promoteNum != staticData.maxPromoteNum, false, false);
			this._charaGrowWild.SetupItemInfoWild(this._currentCharaId);
			this._charaGrowWild.UpdateItemWild(this._currentCharaId);
			this._charaGrowWild.SetActWildButton(this._currentCharaId);
			wildReleaseTab.YaseiInfo.SetActive(dynamicData.promoteNum < staticData.maxPromoteNum);
			return;
		}
		case 2:
		{
			this._charaGrowRank.UpdateItemRankUp(this._currentCharaId);
			CharaPackData userCharaData2 = DataManager.DmChara.GetUserCharaData(this._currentCharaId);
			GrowItemData nextItemByRankup = userCharaData2.GetNextItemByRankup(0);
			bool flag3 = true;
			if (nextItemByRankup != null)
			{
				int num = DataManager.DmItem.GetUserItemData(nextItemByRankup.item.id).num;
				int num2 = nextItemByRankup.item.num;
				if (num < num2)
				{
					flag3 = false;
				}
				else if (DataManager.DmItem.GetUserItemData(30101).num < nextItemByRankup.needGold)
				{
					flag3 = false;
				}
			}
			this._guiData.CharacterGrowMain.Cmn.ButtonR.SetActEnable(flag3 && userCharaData2.dynamicData.rank != userCharaData2.staticData.baseData.rankHigh, false, false);
			this._guiData.CharacterGrowMain.Cmn.ButtonL.SetActEnable(userCharaData2.dynamicData.rank != userCharaData2.staticData.baseData.rankHigh, false, false);
			return;
		}
		case 3:
		{
			this._charaGrowMiracle.UpdateMiracle(this._currentCharaId);
			CharaPackData userCharaData3 = DataManager.DmChara.GetUserCharaData(this._currentCharaId);
			this._guiData.CharacterGrowMain.Cmn.ButtonR.SetActEnable(userCharaData3.EnhanceInfoMiracle, false, false);
			this._guiData.CharacterGrowMain.Cmn.ButtonL.SetActEnable(userCharaData3.dynamicData.artsLevel != 5, false, false);
			return;
		}
		case 4:
		{
			this._charaGrowNanairo.UpdateNanairo(this._currentCharaId);
			CharaPackData userCharaData4 = DataManager.DmChara.GetUserCharaData(this._currentCharaId);
			CharaDynamicData dynamicData2 = userCharaData4.dynamicData;
			bool flag4 = userCharaData4.IsNanairoAbilityReleaseAvailable && !dynamicData2.nanairoAbilityReleaseFlag;
			this._guiData.CharacterGrowMain.Cmn.ButtonR.SetActEnable(userCharaData4.EnhanceInfoNanairo, false, false);
			this._guiData.CharacterGrowMain.Cmn.ButtonL.SetActEnable(!userCharaData4.dynamicData.nanairoAbilityReleaseFlag && flag4, false, false);
			return;
		}
		case 5:
		{
			this._charaGrowPhotoPocket.UpdateItemPhotoPocket(this._currentCharaId);
			this._guiData.CharacterGrowMain.Cmn.ButtonR.gameObject.SetActive(false);
			this._guiData.CharacterGrowMain.Cmn.ButtonRExchange.gameObject.SetActive(true);
			CharaPackData userCharaData5 = DataManager.DmChara.GetUserCharaData(this._currentCharaId);
			GrowItemData nextItemByReleasePhotoFrame = userCharaData5.GetNextItemByReleasePhotoFrame();
			if (nextItemByReleasePhotoFrame != null)
			{
				ItemData userItemData2 = DataManager.DmItem.GetUserItemData(nextItemByReleasePhotoFrame.item.id);
				this._guiData.CharacterGrowMain.Cmn.ButtonRExchangeNumUse.text = string.Format("{0}", nextItemByReleasePhotoFrame.item.num);
				this._guiData.CharacterGrowMain.Cmn.ButtonRExchangeNumUse.SetTextDefaultColor(this._guiData.CharacterGrowMain.Cmn.ButtonRExchangeNumUse.GetComponent<PguiColorCtrl>().GetGameObjectById((nextItemByReleasePhotoFrame.item.num > userItemData2.num) ? "DISABLE" : "NORMAL"));
				this._guiData.CharacterGrowMain.Cmn.ButtonRExchangeIconItem.SetRawImage("Texture2D/Icon_Item/icon_item_gembase01", true, false, null);
			}
			this._guiData.CharacterGrowMain.Cmn.ButtonRExchange.SetActEnable(userCharaData5.CanBeEnhancedPhotoPocket, false, false);
			this._guiData.CharacterGrowMain.Cmn.ButtonL.SetActEnable(true, false, false);
			return;
		}
		case 6:
		{
			bool flag5 = this._charaGrowKizuna.CheckIsPossibleLevelUp(this._currentCharaId);
			bool flag6 = this._lvUpCostCoin <= DataManager.DmItem.GetUserItemData(30101).num && DataManager.DmItem.GetUserItemData(30101).num > 0 && this._selectKizunaLvUpItemIdList.Count > 0 && flag5;
			this._guiData.CharacterGrowMain.Cmn.ButtonL.SetActEnable(flag6, false, false);
			this._guiData.CharacterGrowMain.Cmn.ButtonR.SetActEnable(flag6, false, false);
			if (isGrowMulti)
			{
				this._charaGrowKizuna.SetupLimitLvItemActivation(this._currentCharaId, isGrowMulti);
				return;
			}
			if (flag5)
			{
				this._charaGrowKizuna.UpdateItemLvUp();
				this._charaGrowKizuna.AdjustExpInfoActivation();
				return;
			}
			this._charaGrowKizuna.SetupLimitLvItemActivation(this._currentCharaId, false);
			return;
		}
		default:
			return;
		}
	}

	private bool OnSelectTab(int index)
	{
		if (this._isTutorial && this._tutorialClickTabIndex != index)
		{
			return false;
		}
		this.SwitchCurrentTab(index, this._growMultiCoroutine != null);
		this.TouchRect = true;
		this.SetupNextTab(index);
		return true;
	}

	private void OnClickSetReleaseButton(PguiButtonCtrl pguibtn)
	{
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(this._currentCharaId);
		List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>> list = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>
		{
			new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.NEGATIVE, this.CANCEL_TEXT),
			new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.NONE, this.ENHANCE_TEXT)
		};
		this._charaGrowWild.GrowWildGUI.wildGrowWindow.owCtrl.Setup(this.ALL_RELEASE_TITLE_TEXT, this.ALL_RELEASE_MESSAGE_TEXT, list, true, new PguiOpenWindowCtrl.Callback(this.OnSelectOpenAllWindowButtonCallback), null, false);
		this._charaGrowWild.GrowWildGUI.wildGrowWindow.iconCharaSetRelease.Setup(userCharaData, SortFilterDefine.SortType.LEVEL, false, null, 0, -1, 0);
		this._charaGrowWild.GrowWildGUI.wildGrowWindow.setReleaseIconSet.SetActive(true);
		this._charaGrowWild.GrowWildGUI.wildGrowWindow.wildReleaseIconSet.SetActive(false);
		this._charaGrowWild.GrowWildGUI.wildGrowWindow.charaNameText.text = userCharaData.staticData.GetName();
		int num = DataManager.DmItem.GetUserItemData(30101).num;
		int costGoldTogetherEquip = this._charaGrowWild.GetCostGoldTogetherEquip(this._currentCharaId);
		this._charaGrowWild.GrowWildGUI.wildGrowWindow.haveGoldText.text = num.ToString();
		this._charaGrowWild.GrowWildGUI.wildGrowWindow.haveGoldText.m_Text.color = ((num < costGoldTogetherEquip) ? Color.red : Color.white);
		this._charaGrowWild.GrowWildGUI.wildGrowWindow.needGoldText.text = costGoldTogetherEquip.ToString();
		if (num < costGoldTogetherEquip)
		{
			this._charaGrowWild.GrowWildGUI.wildGrowWindow.ButtonRight.SetActEnable(false, false, false);
			this._charaGrowWild.GrowWildGUI.wildGrowWindow.DisableMessageText.text = this.RELEASE_DISABLE_MESSAGE_TEXT;
		}
		else
		{
			this._charaGrowWild.GrowWildGUI.wildGrowWindow.ButtonRight.SetActEnable(true, false, false);
			this._charaGrowWild.GrowWildGUI.wildGrowWindow.DisableMessageText.text = "";
		}
		this._charaGrowWild.GrowWildGUI.wildGrowWindow.setReleasePhaseNum.text = userCharaData.dynamicData.promoteNum.ToString() + "/" + userCharaData.staticData.maxPromoteNum.ToString();
		this.TouchRect = true;
		this._charaGrowWild.GrowWildGUI.wildGrowWindow.owCtrl.Open();
	}

	private void OnClickWildReleaseButton(PguiButtonCtrl pguibtn)
	{
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(this._currentCharaId);
		bool flag = true;
		using (List<bool>.Enumerator enumerator = userCharaData.dynamicData.promoteFlag.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (!enumerator.Current)
				{
					flag = false;
					break;
				}
			}
		}
		if (flag)
		{
			this.OnSelectWildWindowButtonCallback(1);
			return;
		}
		List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>> list = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>
		{
			new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.NEGATIVE, this.CANCEL_TEXT),
			new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, this.ENHANCE_TEXT)
		};
		this._charaGrowWild.GrowWildGUI.wildGrowWindow.owCtrl.Setup(this.WILD_RELEASE_TITLE_TEXT, this.WILD_RELEASE_MESSAGE_TEXT, list, true, new PguiOpenWindowCtrl.Callback(this.OnSelectWildWindowButtonCallback), null, false);
		this._charaGrowWild.GrowWildGUI.wildGrowWindow.iconCharaWildRelease_Before.Setup(userCharaData, SortFilterDefine.SortType.LEVEL, false, null, 0, -1, 0);
		this._charaGrowWild.GrowWildGUI.wildGrowWindow.iconCharaWildRelease_Before.gameObject.SetActive(true);
		this._charaGrowWild.GrowWildGUI.wildGrowWindow.iconCharaWildRelease_After.Setup(userCharaData, SortFilterDefine.SortType.LEVEL, false, null, 0, -1, 0);
		this._charaGrowWild.GrowWildGUI.wildGrowWindow.iconCharaWildRelease_After.SetWakeUp(userCharaData.dynamicData.promoteNum + 1);
		this._charaGrowWild.GrowWildGUI.wildGrowWindow.iconCharaWildRelease_After.gameObject.SetActive(true);
		this._charaGrowWild.GrowWildGUI.wildGrowWindow.setReleaseIconSet.SetActive(false);
		this._charaGrowWild.GrowWildGUI.wildGrowWindow.wildReleaseIconSet.SetActive(true);
		this._charaGrowWild.GrowWildGUI.wildGrowWindow.charaNameText.text = userCharaData.staticData.GetName();
		int num = DataManager.DmItem.GetUserItemData(30101).num;
		int costGoldWildRelease = this._charaGrowWild.GetCostGoldWildRelease(this._currentCharaId);
		this._charaGrowWild.GrowWildGUI.wildGrowWindow.haveGoldText.text = num.ToString();
		this._charaGrowWild.GrowWildGUI.wildGrowWindow.needGoldText.text = costGoldWildRelease.ToString();
		if (num < costGoldWildRelease)
		{
			this._charaGrowWild.GrowWildGUI.wildGrowWindow.ButtonRight.SetActEnable(false, false, false);
			this._charaGrowWild.GrowWildGUI.wildGrowWindow.DisableMessageText.text = this.RELEASE_DISABLE_MESSAGE_TEXT;
		}
		else
		{
			this._charaGrowWild.GrowWildGUI.wildGrowWindow.ButtonRight.SetActEnable(true, false, false);
			this._charaGrowWild.GrowWildGUI.wildGrowWindow.DisableMessageText.text = "";
		}
		this._charaGrowWild.GrowWildGUI.wildGrowWindow.wildReleasePhaseNumBefore.text = userCharaData.dynamicData.promoteNum.ToString() + "/" + userCharaData.staticData.maxPromoteNum.ToString();
		this._charaGrowWild.GrowWildGUI.wildGrowWindow.wildReleasePhaseNumAfter.text = "<color=#FF7B16FF>" + (userCharaData.dynamicData.promoteNum + 1).ToString() + "</color>/" + userCharaData.staticData.maxPromoteNum.ToString();
		this.TouchRect = true;
		this._charaGrowWild.GrowWildGUI.wildGrowWindow.owCtrl.Open();
	}

	private IEnumerator ExpGageEffectRequest(List<ItemInput> itemList, bool isKizuna = false)
	{
		List<ItemData> expAddItemList = DataManager.DmItem.GetUserItemListByKind(ItemDef.Kind.EXP_ADD);
		int i;
		Predicate<int> <>9__1;
		Predicate<int> <>9__2;
		int num;
		for (i = 0; i < expAddItemList.Count; i = num)
		{
			List<int> list;
			if (!isKizuna)
			{
				List<int> selectLvUpItemIdList = this._selectLvUpItemIdList;
				Predicate<int> predicate;
				if ((predicate = <>9__2) == null)
				{
					predicate = (<>9__2 = (int n) => n == expAddItemList[i].id);
				}
				list = selectLvUpItemIdList.FindAll(predicate);
			}
			else
			{
				List<int> selectKizunaLvUpItemIdList = this._selectKizunaLvUpItemIdList;
				Predicate<int> predicate2;
				if ((predicate2 = <>9__1) == null)
				{
					predicate2 = (<>9__1 = (int n) => n == expAddItemList[i].id);
				}
				list = selectKizunaLvUpItemIdList.FindAll(predicate2);
			}
			List<int> list2 = list;
			if (list2.Count != 0)
			{
				ItemInput itemInput = new ItemInput
				{
					itemId = expAddItemList[i].id,
					num = list2.Count
				};
				itemList.Add(itemInput);
			}
			num = i + 1;
		}
		itemList.Sort((ItemInput a, ItemInput b) => b.num - a.num);
		if (isKizuna)
		{
			DataManager.DmChara.RequestActionCharaKizunaLevelup(this._currentCharaId, itemList);
		}
		else
		{
			DataManager.DmChara.RequestActionCharaLevelup(this._currentCharaId, itemList);
		}
		this._effectStatus = SelCharaGrowCtrl.EffectStatus.ReqServer;
		CanvasManager.SetEnableCmnTouchMask(true);
		this._selectLvUpItemIdList.Clear();
		this._selectKizunaLvUpItemIdList.Clear();
		if (this._growMultiCoroutine == null || (this._guiData.CharacterGrowMain.Cmn.ButtonL.gameObject.activeInHierarchy && this._guiData.CharacterGrowMain.Cmn.ButtonR.gameObject.activeInHierarchy))
		{
			this.SetActStrengthButton(this._guiData.CharacterGrowMain.Cmn.TabGroup.SelectIndex, false);
		}
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator ExpGageEffectAfterRequest(List<ItemInput> itemList)
	{
		CanvasManager.SetEnableCmnTouchMask(true);
		DataManagerChara.CharaLevelupResult result = DataManager.DmChara.GetCharaLevelupResult();
		this._effectStatus = SelCharaGrowCtrl.EffectStatus.Execute;
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(this._currentCharaId);
		CharaDynamicData dynamicData = userCharaData.dynamicData;
		int effectLevel = result.befLevel;
		SelCharaGrowLevel.LvUpTab guiLvUp = this._charaGrowLevelUp.LevelUpGUI.lvUpTab;
		guiLvUp.Num_Lv_L.gameObject.SetActive(false);
		guiLvUp.Img_Yaji.gameObject.SetActive(false);
		guiLvUp.Num_Lv_R.text = string.Concat(new string[]
		{
			this.DEFAULT_LEVEL_TEXT,
			"<color=#FF7B16FF>",
			effectLevel.ToString(),
			"</color>/",
			dynamicData.limitLevel.ToString()
		});
		yield return null;
		guiLvUp.AEImage_result.gameObject.SetActive(true);
		this.SetPguiReplaceAECtrl(result, false);
		guiLvUp.AEImage_result.playTime = 0f;
		guiLvUp.AEImage_result.autoPlay = true;
		guiLvUp.Gage.m_Image.fillAmount = (float)result.befExp / (float)DataManager.DmChara.GetExpByNextLevel(dynamicData.id, result.befLevel);
		guiLvUp.Gage.gameObject.SetActive(true);
		while (!guiLvUp.AEImage_result.end)
		{
			yield return null;
		}
		guiLvUp.AEImage_result.gameObject.SetActive(false);
		bool playVoice = false;
		CriAtomExPlayback gaugeSE = SoundManager.Play("prd_se_friends_levelup_gauge", true, false);
		float afterFill = ((DataManager.DmChara.GetExpByNextLevel(dynamicData.id, result.level) == 0L) ? 1f : ((float)result.exp / (float)DataManager.DmChara.GetExpByNextLevel(dynamicData.id, result.level)));
		CanvasManager.AddCallbackCmnTouchMask(new UnityAction<Transform>(this.OnTouchMask));
		this._touchScreenAuth = false;
		List<int> calcParamList = new List<int> { 0, 0, 0 };
		List<string> textList = new List<string> { this.HP_UP_TEXT, this.ATTACK_UP_TEXT, this.DEFENSE_UP_TEXT };
		this._charaGrowLevelUp.LevelUpGUI.lvupAuth.baseObj.SetActive(true);
		if (itemList.Count == 1)
		{
			ItemData userItemData = DataManager.DmItem.GetUserItemData(itemList[0].itemId);
			foreach (PguiReplaceAECtrl pguiReplaceAECtrl in this._charaGrowLevelUp.LevelUpGUI.lvupAuth.AEImage_AList)
			{
				pguiReplaceAECtrl.Replace(this._charaGrowLevelUp.GetItemId2AttributeId(userItemData.id));
			}
			foreach (PguiReplaceAECtrl pguiReplaceAECtrl2 in this._charaGrowLevelUp.LevelUpGUI.lvupAuth.AEImage_BList)
			{
				pguiReplaceAECtrl2.Replace(this._charaGrowLevelUp.GetItemId2AttributeId(userItemData.id));
			}
			using (List<PguiReplaceAECtrl>.Enumerator enumerator = this._charaGrowLevelUp.LevelUpGUI.lvupAuth.AEImage_CList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PguiReplaceAECtrl pguiReplaceAECtrl3 = enumerator.Current;
					pguiReplaceAECtrl3.Replace(this._charaGrowLevelUp.GetItemId2AttributeId(userItemData.id));
				}
				goto IL_074C;
			}
		}
		if (itemList.Count == 2)
		{
			ItemData userItemData2 = DataManager.DmItem.GetUserItemData(itemList[0].itemId);
			ItemData userItemData3 = DataManager.DmItem.GetUserItemData(itemList[1].itemId);
			foreach (PguiReplaceAECtrl pguiReplaceAECtrl4 in this._charaGrowLevelUp.LevelUpGUI.lvupAuth.AEImage_AList)
			{
				pguiReplaceAECtrl4.Replace(this._charaGrowLevelUp.GetItemId2AttributeId(userItemData2.id));
			}
			foreach (PguiReplaceAECtrl pguiReplaceAECtrl5 in this._charaGrowLevelUp.LevelUpGUI.lvupAuth.AEImage_BList)
			{
				pguiReplaceAECtrl5.Replace(this._charaGrowLevelUp.GetItemId2AttributeId(userItemData3.id));
			}
			this._charaGrowLevelUp.LevelUpGUI.lvupAuth.AEImage_CList[0].Replace(this._charaGrowLevelUp.GetItemId2AttributeId(userItemData2.id));
			this._charaGrowLevelUp.LevelUpGUI.lvupAuth.AEImage_CList[1].Replace(this._charaGrowLevelUp.GetItemId2AttributeId(userItemData3.id));
			this._charaGrowLevelUp.LevelUpGUI.lvupAuth.AEImage_CList[2].Replace(this._charaGrowLevelUp.GetItemId2AttributeId(userItemData3.id));
		}
		else
		{
			ItemData userItemData4 = DataManager.DmItem.GetUserItemData(itemList[0].itemId);
			ItemData userItemData5 = DataManager.DmItem.GetUserItemData(itemList[1].itemId);
			ItemData userItemData6 = DataManager.DmItem.GetUserItemData(itemList[2].itemId);
			foreach (PguiReplaceAECtrl pguiReplaceAECtrl6 in this._charaGrowLevelUp.LevelUpGUI.lvupAuth.AEImage_AList)
			{
				pguiReplaceAECtrl6.Replace(this._charaGrowLevelUp.GetItemId2AttributeId(userItemData4.id));
			}
			foreach (PguiReplaceAECtrl pguiReplaceAECtrl7 in this._charaGrowLevelUp.LevelUpGUI.lvupAuth.AEImage_BList)
			{
				pguiReplaceAECtrl7.Replace(this._charaGrowLevelUp.GetItemId2AttributeId(userItemData5.id));
			}
			foreach (PguiReplaceAECtrl pguiReplaceAECtrl8 in this._charaGrowLevelUp.LevelUpGUI.lvupAuth.AEImage_CList)
			{
				pguiReplaceAECtrl8.Replace(this._charaGrowLevelUp.GetItemId2AttributeId(userItemData6.id));
			}
		}
		IL_074C:
		CriAtomExPlayback japamanSE = SoundManager.Play("prd_se_friends_japaman_hit", true, false);
		bool oneLoop = false;
		RenderTextureChara.FinishCallback <>9__4;
		this._renderTextureChara.SetAnimation(CharaMotionDefine.ActKey.EXP_GROWTH_ST, false, delegate
		{
			RenderTextureChara renderTextureChara = this._renderTextureChara;
			CharaMotionDefine.ActKey actKey = CharaMotionDefine.ActKey.EXP_GROWTH_LP;
			bool flag = false;
			RenderTextureChara.FinishCallback finishCallback;
			if ((finishCallback = <>9__4) == null)
			{
				finishCallback = (<>9__4 = delegate
				{
					oneLoop = true;
					this._renderTextureChara.SetAnimation(CharaMotionDefine.ActKey.EXP_GROWTH_LP, true);
				});
			}
			renderTextureChara.SetAnimation(actKey, flag, finishCallback);
		});
		bool isLvup = false;
		while (effectLevel <= result.level)
		{
			if (!this._charaGrowLevelUp.LevelUpGUI.lvupAuth.AEImage_JapamanFeed.IsPlaying())
			{
				this._charaGrowLevelUp.LevelUpGUI.lvupAuth.AEImage_JapamanFeed.PlayAnimation(PguiAECtrl.AmimeType.START, null);
			}
			if (this._touchScreenAuth)
			{
				if (effectLevel < result.level)
				{
					guiLvUp.SetAEImageLevelUP();
					isLvup = true;
				}
				effectLevel = result.level;
			}
			float num = 0.1f * Time.deltaTime / 0.033333335f;
			guiLvUp.Gage.m_Image.fillAmount += num;
			guiLvUp.Num_Lv_R.text = this.DEFAULT_LEVEL_TEXT + PrjUtil.MakeMessage("<color=#FF7B16FF>" + effectLevel.ToString() + "</color>/" + dynamicData.limitLevel.ToString());
			if (effectLevel < result.level)
			{
				if (guiLvUp.Gage.m_Image.fillAmount >= 1f)
				{
					guiLvUp.Gage.m_Image.fillAmount = 1f;
					yield return null;
					int num2 = effectLevel;
					effectLevel = num2 + 1;
					guiLvUp.Gage.m_Image.fillAmount = 0f;
					playVoice = true;
					guiLvUp.SetAEImageLevelUP();
				}
				isLvup = true;
			}
			else if (guiLvUp.Gage.m_Image.fillAmount >= afterFill)
			{
				guiLvUp.Gage.m_Image.fillAmount = afterFill;
				IL_09B0:
				while (this._charaGrowLevelUp.LevelUpGUI.lvupAuth.AEImage_JapamanFeed.IsPlaying())
				{
					yield return null;
				}
				japamanSE.Stop();
				gaugeSE.Stop();
				guiLvUp.Result_Lvup.gameObject.SetActive(true);
				PrjUtil.ParamPreset paramPreset = PrjUtil.CalcParamByChara(DataManager.DmChara.GetUserCharaData(this._currentCharaId).dynamicData, result.befLevel, dynamicData.rank);
				PrjUtil.ParamPreset paramPreset2 = PrjUtil.CalcParamByChara(DataManager.DmChara.GetUserCharaData(this._currentCharaId).dynamicData, effectLevel, dynamicData.rank);
				List<int> list = new List<int> { paramPreset.hp, paramPreset.atk, paramPreset.def };
				List<int> list2 = new List<int> { paramPreset2.hp, paramPreset2.atk, paramPreset2.def };
				int num2;
				for (int j = 0; j < list2.Count; j++)
				{
					List<int> list3 = calcParamList;
					num2 = j;
					list3[num2] += list2[j] - list[j];
				}
				for (int i = 0; i < calcParamList.Count; i = num2)
				{
					if (calcParamList[i] > 0)
					{
						SoundManager.Play("prd_se_friends_levelup_font", false, false);
						guiLvUp.Num_Result.text = textList[i] + calcParamList[i].ToString();
						guiLvUp.Result_Lvup.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
						yield return null;
						while (guiLvUp.Result_Lvup.ExIsPlaying())
						{
							yield return null;
						}
					}
					num2 = i + 1;
				}
				guiLvUp.Result_Lvup.gameObject.SetActive(false);
				while (!oneLoop)
				{
					yield return null;
				}
				if (isLvup)
				{
					this._renderTextureChara.SetAnimation(CharaMotionDefine.ActKey.POSITIVE, false, delegate
					{
						this._renderTextureChara.SetAnimation(CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true);
					});
				}
				else
				{
					this._renderTextureChara.SetAnimation(CharaMotionDefine.ActKey.EXP_GROWTH_EN, false, delegate
					{
						this._renderTextureChara.SetAnimation(CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true);
					});
				}
				this._charaGrowLevelUp.LevelUpGUI.lvupAuth.baseObj.SetActive(false);
				guiLvUp.AEImage_LevelUP.gameObject.SetActive(false);
				if (this._growMultiCoroutine == null)
				{
					this._effectStatus = SelCharaGrowCtrl.EffectStatus.Finished;
				}
				else
				{
					this._effectStatus = SelCharaGrowCtrl.EffectStatus.ResultEnd;
				}
				if (playVoice)
				{
					SoundManager.PlayVoice(DataManager.DmChara.GetCharaStaticData(this._currentCharaId).cueSheetName, VOICE_TYPE.LUP01);
					this._voiceLength = SoundManager.GetVoiceLength(DataManager.DmChara.GetCharaStaticData(this._currentCharaId).cueSheetName, VOICE_TYPE.LUP01);
					this._voiceStartTime = Time.time;
				}
				this.CharaGrowSetup(this._currentCharaId, false);
				if (result.returnItem)
				{
					this._effectStatus = SelCharaGrowCtrl.EffectStatus.Result;
					CanvasManager.HdlOpenWindowBasic.Setup(this.RETURN_ITEM_TITLE_TEXT, this.RETURN_ITEM_MESSAGE_TEXT, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int index)
					{
						this._effectStatus = SelCharaGrowCtrl.EffectStatus.ResultEnd;
						return true;
					}, null, false);
					CanvasManager.HdlOpenWindowBasic.Open();
					while (this._effectStatus != SelCharaGrowCtrl.EffectStatus.ResultEnd)
					{
						yield return null;
					}
				}
				yield break;
			}
			yield return null;
		}
		goto IL_09B0;
	}

	private IEnumerator KizunaExpGageEffectAfterRequest(List<ItemInput> itemList)
	{
		CanvasManager.SetEnableCmnTouchMask(true);
		DataManagerChara.CharaLevelupResult result = DataManager.DmChara.GetCharaKizunaLevelupResult();
		this._effectStatus = SelCharaGrowCtrl.EffectStatus.Execute;
		CharaPackData charaPackData = DataManager.DmChara.GetUserCharaData(this._currentCharaId);
		CharaDynamicData dynamicData = charaPackData.dynamicData;
		int level = result.level;
		int beforeLevel = result.befLevel;
		int effectLevel = beforeLevel;
		SelCharaGrowKizuna.KizunaLvUpTab guiLvUp = this._charaGrowKizuna.GrowKizunaGUI.KizunaLvUpTab;
		guiLvUp.NumLvLeft.gameObject.SetActive(false);
		guiLvUp.ImgYaji.gameObject.SetActive(false);
		guiLvUp.NumLvRight.text = string.Concat(new string[]
		{
			this.DEFAULT_LEVEL_TEXT,
			"<color=#FF7B16FF>",
			effectLevel.ToString(),
			"</color>/",
			dynamicData.KizunaLimitLevel.ToString()
		});
		yield return null;
		AEImage resultImage = guiLvUp.ImageResult;
		resultImage.gameObject.SetActive(true);
		this.SetPguiReplaceAECtrl(result, true);
		resultImage.playTime = 0f;
		resultImage.autoPlay = true;
		float num = (float)result.befExp / (float)DataManager.DmChara.GetExpByNextKizunaLevel(dynamicData.id, beforeLevel);
		guiLvUp.SetGageImageFillAmount(num);
		guiLvUp.Gage.gameObject.SetActive(true);
		while (!resultImage.end)
		{
			yield return null;
		}
		resultImage.gameObject.SetActive(false);
		bool playVoice = false;
		CriAtomExPlayback gaugeSE = SoundManager.Play("prd_se_friends_levelup_gauge", true, false);
		float afterFill = ((DataManager.DmChara.GetExpByNextKizunaLevel(dynamicData.id, level) == 0L) ? 1f : ((float)result.exp / (float)DataManager.DmChara.GetExpByNextKizunaLevel(dynamicData.id, level)));
		CanvasManager.AddCallbackCmnTouchMask(new UnityAction<Transform>(this.OnTouchMask));
		this._touchScreenAuth = false;
		List<int> list = new List<int>();
		list.Add(0);
		list.Add(0);
		list.Add(0);
		List<string> list2 = new List<string>();
		list2.Add(this.HP_UP_TEXT);
		list2.Add(this.ATTACK_UP_TEXT);
		list2.Add(this.DEFENSE_UP_TEXT);
		SelCharaGrowKizuna.LvupAuth lvupAuth = this._charaGrowKizuna.LevelUpGUI.LvupAuth;
		bool oneLoop = false;
		RenderTextureChara.FinishCallback <>9__4;
		this._renderTextureChara.SetAnimation(CharaMotionDefine.ActKey.EXP_GROWTH_ST, false, delegate
		{
			RenderTextureChara renderTextureChara = this._renderTextureChara;
			CharaMotionDefine.ActKey actKey = CharaMotionDefine.ActKey.EXP_GROWTH_LP;
			bool flag = false;
			RenderTextureChara.FinishCallback finishCallback;
			if ((finishCallback = <>9__4) == null)
			{
				finishCallback = (<>9__4 = delegate
				{
					oneLoop = true;
					this._renderTextureChara.SetAnimation(CharaMotionDefine.ActKey.EXP_GROWTH_LP, true);
				});
			}
			renderTextureChara.SetAnimation(actKey, flag, finishCallback);
		});
		bool isLvup = false;
		while (effectLevel <= level)
		{
			if (!lvupAuth.ImageJapamanFeed.IsPlaying())
			{
				lvupAuth.ImageJapamanFeed.PlayAnimation(PguiAECtrl.AmimeType.START, null);
			}
			if (this._touchScreenAuth)
			{
				if (effectLevel < level)
				{
					guiLvUp.SetAEImageLevelUP();
					isLvup = true;
				}
				effectLevel = level;
			}
			float num2 = 0.1f * Time.deltaTime / 0.033333335f;
			guiLvUp.AddGageImageFillAmount(num2);
			num = guiLvUp.Gage.m_Image.fillAmount;
			guiLvUp.NumLvRight.text = this.DEFAULT_LEVEL_TEXT + PrjUtil.MakeMessage("<color=#FF7B16FF>" + effectLevel.ToString() + "</color>/" + dynamicData.KizunaLimitLevel.ToString());
			if (effectLevel < level)
			{
				if (num >= 1f)
				{
					guiLvUp.SetGageImageFillAmount(1f);
					yield return null;
					int num3 = effectLevel;
					effectLevel = num3 + 1;
					guiLvUp.SetGageImageFillAmount(0f);
					playVoice = true;
					guiLvUp.SetAEImageLevelUP();
				}
				isLvup = true;
			}
			else if (num >= afterFill)
			{
				guiLvUp.SetGageImageFillAmount(afterFill);
				break;
			}
			yield return null;
		}
		gaugeSE.Stop();
		guiLvUp.ResultLvup.gameObject.SetActive(false);
		while (!oneLoop)
		{
			yield return null;
		}
		if (isLvup)
		{
			this._renderTextureChara.SetAnimation(CharaMotionDefine.ActKey.POSITIVE, false, delegate
			{
				this._renderTextureChara.SetAnimation(CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true);
			});
		}
		else
		{
			this._renderTextureChara.SetAnimation(CharaMotionDefine.ActKey.EXP_GROWTH_EN, false, delegate
			{
				this._renderTextureChara.SetAnimation(CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true);
			});
		}
		lvupAuth.BaseObj.SetActive(false);
		guiLvUp.ImageLevelUP.gameObject.SetActive(false);
		if (this._growMultiCoroutine == null)
		{
			this._effectStatus = SelCharaGrowCtrl.EffectStatus.Finished;
		}
		else
		{
			this._effectStatus = SelCharaGrowCtrl.EffectStatus.ResultEnd;
		}
		if (playVoice)
		{
			SoundManager.PlayVoice(DataManager.DmChara.GetCharaStaticData(this._currentCharaId).cueSheetName, VOICE_TYPE.LUP01);
			this._voiceLength = SoundManager.GetVoiceLength(DataManager.DmChara.GetCharaStaticData(this._currentCharaId).cueSheetName, VOICE_TYPE.LUP01);
			this._voiceStartTime = Time.time;
		}
		this.CharaGrowSetup(this._currentCharaId, false);
		if (result.returnItem)
		{
			this._effectStatus = SelCharaGrowCtrl.EffectStatus.Result;
			CanvasManager.HdlOpenWindowBasic.Setup(this.RETURN_ITEM_TITLE_TEXT, this.RETURN_KIZUNA_ITEM_MESSAGE_TEXT, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int index)
			{
				this._effectStatus = SelCharaGrowCtrl.EffectStatus.ResultEnd;
				return true;
			}, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			while (this._effectStatus != SelCharaGrowCtrl.EffectStatus.ResultEnd)
			{
				yield return null;
			}
		}
		if (level > beforeLevel)
		{
			SelCharaGrowKizuna.KizunaLevelUpEffectWindow kizunaLevelUpEffectWindow = this._charaGrowKizuna.GrowKizunaGUI.KizunaLevelUpEffectWindow;
			kizunaLevelUpEffectWindow.SetCurrentCharaPackData(charaPackData);
			kizunaLevelUpEffectWindow.StartKizunaUp(beforeLevel, level);
		}
		yield break;
	}

	private IEnumerator ExpGageEffect(bool isKizuna = false)
	{
		List<ItemInput> itemList = new List<ItemInput>();
		IEnumerator ienum = this.ExpGageEffectRequest(itemList, isKizuna);
		while (ienum.MoveNext())
		{
			yield return null;
		}
		ienum = (isKizuna ? this.KizunaExpGageEffectAfterRequest(itemList) : this.ExpGageEffectAfterRequest(itemList));
		while (ienum.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator LevelLimitOverEffectRequest(int afterLevelLimitId)
	{
		DataManager.DmChara.RequestActoinCharaLimitLevelUp(this._currentCharaId, afterLevelLimitId);
		this._effectStatus = SelCharaGrowCtrl.EffectStatus.ReqServer;
		CanvasManager.SetEnableCmnTouchMask(true);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator LevelLimitOverEffectAfterRequest()
	{
		CanvasManager.SetEnableCmnTouchMask(true);
		this._effectStatus = SelCharaGrowCtrl.EffectStatus.Execute;
		AEImage resultImage = this._guiData.CharacterGrowMain.LvLimitOpen.AEImage_result;
		resultImage.gameObject.SetActive(true);
		resultImage.playTime = 0f;
		resultImage.autoPlay = true;
		SoundManager.Play("prd_se_friends_level_expansion", false, false);
		this._renderTextureChara.SetAnimation(CharaMotionDefine.ActKey.JOY, false, delegate
		{
			this._renderTextureChara.SetAnimation(CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true);
		});
		this._renderTextureChara.PlayVoice(VOICE_TYPE.JOY01);
		this._voiceLength = SoundManager.GetVoiceLength(DataManager.DmChara.GetCharaStaticData(this._currentCharaId).cueSheetName, VOICE_TYPE.JOY01);
		this._voiceStartTime = Time.time;
		while (!resultImage.end)
		{
			yield return null;
		}
		resultImage.gameObject.SetActive(false);
		if (this._growMultiCoroutine == null)
		{
			this._effectStatus = SelCharaGrowCtrl.EffectStatus.Finished;
		}
		else
		{
			this._effectStatus = SelCharaGrowCtrl.EffectStatus.ResultEnd;
		}
		this.CharaGrowSetup(this._currentCharaId, false);
		yield break;
	}

	private IEnumerator LevelLimitOverEffect()
	{
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(this._currentCharaId);
		IEnumerator ienum = this.LevelLimitOverEffectRequest(userCharaData.dynamicData.levelLimitId + 1);
		while (ienum.MoveNext())
		{
			yield return null;
		}
		ienum = this.LevelLimitOverEffectAfterRequest();
		while (ienum.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator WildReleaseEffectRequest(bool isPromoteStepUp, bool isSingle, List<WildResult> promoteRequest = null)
	{
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(this._currentCharaId);
		CharaDynamicData dynamicData = userCharaData.dynamicData;
		if (!isPromoteStepUp)
		{
			this._guiData.CharacterGrowMain.Cmn.ButtonR.SetActEnable(false, false, false);
			this._guiData.CharacterGrowMain.Cmn.ButtonRExchange.SetActEnable(false, false, false);
			this._pressedButtonRWildRelease = false;
		}
		CanvasManager.HdlMissionProgressCtrl.IsWildRelease = true;
		List<int> list = new List<int>();
		if (isSingle)
		{
			list.Add(this._charaGrowWild.currentIndexWild);
		}
		else
		{
			HashSet<int> hashSet = new HashSet<int>();
			foreach (IconItemCtrl iconItemCtrl in this._charaGrowWild.GrowWildGUI.wildGrowWindow.iconItemList)
			{
				if (iconItemCtrl.isActiveAndEnabled)
				{
					hashSet.Add(iconItemCtrl.itemStaticBase.GetId());
				}
			}
			List<CharaPromoteOne> promoteOneList = userCharaData.staticData.promoteList[this._charaGrowWild.GetPromoteNum(this._currentCharaId)].promoteOneList;
			for (int i = 0; i < promoteOneList.Count; i++)
			{
				if (hashSet.Contains(promoteOneList[i].promoteUseItemId) && !dynamicData.promoteFlag[i])
				{
					list.Add(i);
				}
			}
		}
		if (promoteRequest == null)
		{
			promoteRequest = new List<WildResult>();
			WildResult wildResult = new WildResult
			{
				chara_id = this._currentCharaId,
				promote_num = this._charaGrowWild.GetPromoteNum(this._currentCharaId)
			};
			foreach (int num in list)
			{
				if (num == 0)
				{
					wildResult.promote_flag00 = 1;
				}
				else if (num == 1)
				{
					wildResult.promote_flag01 = 1;
				}
				else if (num == 2)
				{
					wildResult.promote_flag02 = 1;
				}
				else if (num == 3)
				{
					wildResult.promote_flag03 = 1;
				}
				else if (num == 4)
				{
					wildResult.promote_flag04 = 1;
				}
				else if (num == 5)
				{
					wildResult.promote_flag05 = 1;
				}
			}
			promoteRequest.Add(wildResult);
		}
		DataManager.DmChara.RequestActionCharaPromote(this._currentCharaId, promoteRequest, isPromoteStepUp);
		this._effectStatus = SelCharaGrowCtrl.EffectStatus.ReqServer;
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator WildReleaseEffectAfterRequest(bool isPromoteStepUp, PrjUtil.ParamPreset beforeParam, int beforePromoteNum)
	{
		CanvasManager.SetEnableCmnTouchMask(true);
		CharaPackData charaData = DataManager.DmChara.GetUserCharaData(this._currentCharaId);
		CharaStaticData charaStaticData = charaData.staticData;
		CharaDynamicData charaDynamicData = charaData.dynamicData;
		this._charaGrowWild.UpdateItemWild(this._currentCharaId);
		this._charaGrowWild.SetupItemInfoWild(this._currentCharaId);
		this._effectStatus = SelCharaGrowCtrl.EffectStatus.Execute;
		SelCharaGrowWild.WildReleaseTab wild = this._charaGrowWild.GrowWildGUI.wildReleaseTab;
		if (isPromoteStepUp)
		{
			AEImage resultImage = wild.AEImage_result;
			resultImage.gameObject.SetActive(true);
			resultImage.playTime = 0f;
			resultImage.autoPlay = true;
			resultImage.playLoop = false;
			this._renderTextureChara.SetAnimation(CharaMotionDefine.ActKey.POSITIVE, false, delegate
			{
				this._renderTextureChara.SetAnimation(CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true);
			});
			SoundManager.Play("prd_se_friends_liberation_auth", false, false);
			SoundManager.PlayVoice(charaData.staticData.cueSheetName, VOICE_TYPE.PMT01);
			this._voiceLength = SoundManager.GetVoiceLength(charaData.staticData.cueSheetName, VOICE_TYPE.PMT01);
			this._voiceStartTime = Time.time;
			wild.YaseiInfo.SetActive(false);
			while (!resultImage.end)
			{
				yield return null;
			}
			resultImage.gameObject.SetActive(false);
			wild.YaseiInfo.SetActive(charaDynamicData.promoteNum < charaStaticData.maxPromoteNum);
			this._effectStatus = SelCharaGrowCtrl.EffectStatus.Result;
			this._charaGrowWild.GrowWildGUI.wildResultWindow.owCtrl.Setup(this.WILD_RELEASE_RESULT_TITLE_TEXT, this.WILD_RELEASE_RESULT_MESSAGE_TEXT, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, new PguiOpenWindowCtrl.Callback(this.OnResultWildWindowButtonCallback), null, false);
			this._charaGrowWild.GrowWildGUI.wildResultWindow.iconChara.Setup(charaData, SortFilterDefine.SortType.LEVEL, false, null, 0, -1, 0);
			this._charaGrowWild.GrowWildGUI.wildResultWindow.Txt_CharaName.text = charaStaticData.GetName();
			this._charaGrowWild.GrowWildGUI.wildResultWindow.Txt_Num.text = charaDynamicData.promoteNum.ToString() + "/" + charaStaticData.maxPromoteNum.ToString();
			PrjUtil.ParamPreset paramPreset = (this._isTutorial ? PrjUtil.CalcParamByChara(charaDynamicData, charaDynamicData.level, charaDynamicData.rank, 1, new List<bool> { false, false, false, false, false, false }) : PrjUtil.CalcParamByChara(charaDynamicData, charaDynamicData.level, charaDynamicData.rank));
			List<int> list = new List<int>();
			list.Add(beforeParam.totalParam);
			list.Add(beforeParam.hp);
			list.Add(beforeParam.atk);
			list.Add(beforeParam.def);
			list.Add(beforeParam.avoid);
			list.Add(beforeParam.beatDamageRatio);
			list.Add(beforeParam.actionDamageRatio);
			list.Add(beforeParam.tryDamageRatio);
			List<int> list2 = new List<int> { paramPreset.totalParam, paramPreset.hp, paramPreset.atk, paramPreset.def, paramPreset.avoid, paramPreset.beatDamageRatio, paramPreset.actionDamageRatio, paramPreset.tryDamageRatio };
			SelCharaGrowWild.WindowWildResult.SetWindowParam(list, list2, this._charaGrowWild.GrowWildGUI.wildResultWindow.ParamAll);
			this._charaGrowWild.GrowWildGUI.wildResultWindow.owCtrl.Open();
			while (!this._charaGrowWild.GrowWildGUI.wildResultWindow.owCtrl.FinishedOpen())
			{
				yield return null;
			}
			CanvasManager.SetEnableCmnTouchMask(false);
			CharaPromoteOne charaPromoteOne = charaStaticData.promoteList[this._charaGrowWild.GetPromoteNum(this._currentCharaId)].promoteOneList[this._charaGrowWild.currentIndexWild];
			while (this._effectStatus != SelCharaGrowCtrl.EffectStatus.ResultEnd)
			{
				yield return null;
			}
			CanvasManager.SetEnableCmnTouchMask(true);
			if (beforePromoteNum < 1 && 1 <= charaDynamicData.promoteNum)
			{
				this._effectStatus = SelCharaGrowCtrl.EffectStatus.Result;
				this._charaGrowWild.GrowWildGUI.tokuseiInfoWindow.owCtrl.Setup(PrjUtil.MakeMessage(this.WILD_RELEASE_ABILITY_OPEN_TITLE_TEXT), PrjUtil.MakeMessage(this.WILD_RELEASE_ABILITY_OPEN_MESSAGE_TEXT), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, new PguiOpenWindowCtrl.Callback(this.OnResultWildWindowButtonCallback), null, false);
				this._charaGrowWild.GrowWildGUI.tokuseiInfoWindow.skill.SetActive(true);
				this._charaGrowWild.GrowWildGUI.tokuseiInfoWindow.skillKiseki.SetActive(false);
				this._charaGrowWild.GrowWildGUI.tokuseiInfoWindow.Txt_Name.text = charaData.staticData.abilityData[0].abilityName;
				this._charaGrowWild.GrowWildGUI.tokuseiInfoWindow.Txt_Info.text = charaData.staticData.abilityData[0].abilityEffect;
				this._charaGrowWild.GrowWildGUI.tokuseiInfoWindow.owCtrl.Open();
				while (!this._charaGrowWild.GrowWildGUI.tokuseiInfoWindow.owCtrl.FinishedOpen())
				{
					yield return null;
				}
				CanvasManager.SetEnableCmnTouchMask(false);
				while (this._effectStatus != SelCharaGrowCtrl.EffectStatus.ResultEnd)
				{
					yield return null;
				}
				CanvasManager.SetEnableCmnTouchMask(true);
			}
			if (beforePromoteNum < 4 && 2 <= charaDynamicData.promoteNum)
			{
				this._charaGrowWild.GrowWildGUI.iconOpenWindow.owCtrl.Setup(this.WILD_RELEASE_ICON_OPEN_TITLE_TEXT, this.WILD_RELEASE_ICON_OPEN_MESSAGE_TEXT, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, new PguiOpenWindowCtrl.Callback(this.OnResultWildWindowButtonCallback), null, false);
				int num2;
				for (int num = Math.Max(2, beforePromoteNum + 1); num <= Math.Min(4, charaDynamicData.promoteNum); num = num2 + 1)
				{
					this._effectStatus = SelCharaGrowCtrl.EffectStatus.Result;
					this._charaGrowWild.GrowWildGUI.iconOpenWindow.Icon_Chara.gameObject.SetActive(false);
					this._charaGrowWild.GrowWildGUI.iconOpenWindow.owCtrl.ForceOpen();
					while (!this._charaGrowWild.GrowWildGUI.iconOpenWindow.owCtrl.StartOpenAnim())
					{
						if (this._charaGrowWild.GrowWildGUI.iconOpenWindow.owCtrl.FinishedOpen())
						{
							break;
						}
						yield return null;
					}
					while (!this._charaGrowWild.GrowWildGUI.iconOpenWindow.owCtrl.FinishedOpen())
					{
						yield return null;
					}
					CanvasManager.SetEnableCmnTouchMask(false);
					this._charaGrowWild.GrowWildGUI.iconOpenWindow.Icon_Chara.SetupPrm(new IconCharaCtrl.SetupParam
					{
						cpd = charaData,
						iconId = num,
						sortType = SortFilterDefine.SortType.INVALID
					});
					this._charaGrowWild.GrowWildGUI.iconOpenWindow.Icon_Chara.DispRarity(false);
					this._charaGrowWild.GrowWildGUI.iconOpenWindow.Icon_Chara.DispAttribute(false);
					this._charaGrowWild.GrowWildGUI.iconOpenWindow.Icon_Chara.DispAttributeMark((CharaDef.AttributeMask)0);
					this._charaGrowWild.GrowWildGUI.iconOpenWindow.Icon_Chara.DispWakeUp(false);
					while (this._effectStatus != SelCharaGrowCtrl.EffectStatus.ResultEnd)
					{
						yield return null;
					}
					CanvasManager.SetEnableCmnTouchMask(true);
					num2 = num;
				}
			}
			this._wildReleaseEffectPlaying = true;
			this.CharaGrowSetup(this._currentCharaId, false);
			this._wildReleaseEffectPlaying = false;
			resultImage = null;
		}
		else
		{
			List<int> calcParamList = new List<int> { 0, 0, 0, 0, 0, 0, 0 };
			List<string> textList = new List<string> { this.HP_UP_TEXT, this.ATTACK_UP_TEXT, this.DEFENSE_UP_TEXT, this.AVOID_UP_TEXT, this.WILD_RELEASE_BEAT_DAMAGE_UP_TEXT, this.WILD_RELEASE_TRY_DAMAGE_UP_TEXT, this.WILD_RELEASE_ACTION_DAMAGE_UP_TEXT };
			wild.Result_Up.gameObject.SetActive(true);
			PrjUtil.ParamPreset paramPreset2 = (this._isTutorial ? PrjUtil.CalcParamByChara(charaDynamicData, charaDynamicData.level, charaDynamicData.rank, 1, new List<bool> { false, false, false, false, false, false }) : PrjUtil.CalcParamByChara(charaDynamicData, charaDynamicData.level, charaDynamicData.rank));
			List<int> list3 = new List<int> { beforeParam.hp, beforeParam.atk, beforeParam.def, beforeParam.avoid, beforeParam.beatDamageRatio, beforeParam.tryDamageRatio, beforeParam.actionDamageRatio };
			List<int> list4 = new List<int> { paramPreset2.hp, paramPreset2.atk, paramPreset2.def, paramPreset2.avoid, paramPreset2.beatDamageRatio, paramPreset2.tryDamageRatio, paramPreset2.actionDamageRatio };
			int num2;
			for (int i = 0; i < list4.Count; i++)
			{
				List<int> list5 = calcParamList;
				num2 = i;
				list5[num2] += list4[i] - list3[i];
			}
			for (int num = 0; num < calcParamList.Count; num = num2)
			{
				if (calcParamList[num] > 0)
				{
					SoundManager.Play("prd_se_friends_levelup_font", false, false);
					wild.Num_Result.text = ((num < 3) ? (textList[num] + calcParamList[num].ToString()) : (textList[num] + ((float)calcParamList[num] / 10f).ToString("F1") + "%"));
					wild.Result_Up.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
					yield return null;
					while (wild.Result_Up.ExIsPlaying())
					{
						yield return null;
					}
				}
				num2 = num + 1;
			}
			wild.Result_Up.gameObject.SetActive(false);
			this._wildReleaseEffectPlaying = true;
			this.CharaGrowSetup(this._currentCharaId, false);
			this._wildReleaseEffectPlaying = false;
			calcParamList = null;
			textList = null;
		}
		CanvasManager.HdlMissionProgressCtrl.IsWildRelease = false;
		yield return null;
		yield break;
	}

	private IEnumerator WildReleaseEffect(bool isPromoteStepUp, bool isSingle)
	{
		CharaDynamicData dynamicData = DataManager.DmChara.GetUserCharaData(this._currentCharaId).dynamicData;
		int level = dynamicData.level;
		int rank = dynamicData.rank;
		PrjUtil.ParamPreset beforeParam = (this._isTutorial ? PrjUtil.CalcParamByChara(dynamicData, level, rank, 0, new List<bool> { false, false, false, false, false, false }) : PrjUtil.CalcParamByChara(dynamicData, level, rank));
		int beforePromoteNum = dynamicData.promoteNum;
		IEnumerator ienum = this.WildReleaseEffectRequest(isPromoteStepUp, isSingle, null);
		while (ienum.MoveNext())
		{
			yield return null;
		}
		ienum = this.WildReleaseEffectAfterRequest(isPromoteStepUp, beforeParam, beforePromoteNum);
		while (ienum.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator RankUpGageEffectRequest(int afterRank)
	{
		DataManager.DmChara.RequestActionCharaRankup(this._currentCharaId, afterRank);
		this._effectStatus = SelCharaGrowCtrl.EffectStatus.ReqServer;
		CanvasManager.SetEnableCmnTouchMask(true);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator RankUpGageEffectAfterRequest(int beforedRank)
	{
		CanvasManager.SetEnableCmnTouchMask(true);
		this.CharaGrowSetup(this._currentCharaId, false);
		List<ItemInput> kemoBoardItem = DataManager.DmChara.RankUpResultKemoBoardItem;
		this._effectStatus = SelCharaGrowCtrl.EffectStatus.Execute;
		Vector3 charaScale = this._renderTextureChara.GetCharaScale();
		SelCharaGrowRank.RankUpAuth.SIZE size = SelCharaGrowRank.RankUpAuth.SIZE.SIZE_S;
		if (Math.Abs(charaScale.y - 1f) <= 1E-45f)
		{
			size = SelCharaGrowRank.RankUpAuth.SIZE.SIZE_M;
		}
		else if (charaScale.y < 1f)
		{
			size = SelCharaGrowRank.RankUpAuth.SIZE.SIZE_S;
		}
		else if (charaScale.y > 1f)
		{
			size = SelCharaGrowRank.RankUpAuth.SIZE.SIZE_L;
		}
		this._charaGrowRank.GrowRankGUI.rankUpAuth.baseObj.SetActive(true);
		this._charaGrowRank.GrowRankGUI.rankUpAuth.Setup(this._currentCharaId, size, 1, beforedRank);
		while (!this._charaGrowRank.GrowRankGUI.rankUpAuth.rtc.FinishedSetup)
		{
			yield return null;
		}
		SoundManager.PlayVoice(DataManager.DmChara.GetCharaStaticData(this._currentCharaId).cueSheetName, VOICE_TYPE.RUP01);
		this._voiceLength = SoundManager.GetVoiceLength(DataManager.DmChara.GetCharaStaticData(this._currentCharaId).cueSheetName, VOICE_TYPE.RUP01);
		this._voiceStartTime = Time.time;
		CanvasManager.AddCallbackCmnTouchMask(new UnityAction<Transform>(this.OnTouchMask));
		this._touchScreenAuth = false;
		while (!this._touchScreenAuth)
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				this._touchScreenAuth = true;
			}
			yield return null;
		}
		SelCharaGrowRank.RankUpAuth rankUpAuth = this._charaGrowRank.GrowRankGUI.rankUpAuth;
		rankUpAuth.AEImage_Back.PlayAnimation(PguiAECtrl.AmimeType.END, delegate
		{
			rankUpAuth.AEImage_Back.gameObject.SetActive(false);
		});
		rankUpAuth.AEImage_RankUp.PlayAnimation(PguiAECtrl.AmimeType.END, delegate
		{
			rankUpAuth.AEImage_RankUp.gameObject.SetActive(false);
		});
		rankUpAuth.AEImage_Bg.PlayAnimation(PguiAECtrl.AmimeType.END, delegate
		{
			rankUpAuth.AEImage_Bg.gameObject.SetActive(false);
		});
		using (List<PguiAECtrl>.Enumerator enumerator = rankUpAuth.AEImage_StarAll.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				PguiAECtrl pguiAECtrl = enumerator.Current;
				pguiAECtrl.PlayAnimation(PguiAECtrl.AmimeType.END, null);
			}
			goto IL_02A5;
		}
		IL_028E:
		yield return null;
		IL_02A5:
		if (!rankUpAuth.AEImage_Back.gameObject.activeSelf)
		{
			rankUpAuth.baseObj.SetActive(false);
			rankUpAuth.Teardown();
			CanvasManager.RemoveCallbackCmnTouchMask();
			this._effectStatus = SelCharaGrowCtrl.EffectStatus.Result;
			this._charaGrowRank.GrowRankGUI.rankUpResultWindow.owCtrl.Setup(null, null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, new PguiOpenWindowCtrl.Callback(this.OnResultRankupWindowButtonCallback), null, false);
			this._charaGrowRank.GrowRankGUI.rankUpResultWindow.owCtrl.Open();
			IconCharaCtrl component = this._charaGrowRank.GrowRankGUI.rankUpResultWindow.iconChara.GetComponent<IconCharaCtrl>();
			CharaPackData charaData = DataManager.DmChara.GetUserCharaData(this._currentCharaId);
			component.Setup(charaData, SortFilterDefine.SortType.LEVEL, false, null, 0, -1, 0);
			this._charaGrowRank.GrowRankGUI.rankUpResultWindow.Txt_CharaName.text = charaData.staticData.GetName();
			PrjUtil.ParamPreset paramPreset = PrjUtil.CalcParamByChara(DataManager.DmChara.GetUserCharaData(this._currentCharaId).dynamicData, charaData.dynamicData.level, beforedRank);
			PrjUtil.ParamPreset paramPreset2 = PrjUtil.CalcParamByChara(DataManager.DmChara.GetUserCharaData(this._currentCharaId).dynamicData, charaData.dynamicData.level, charaData.dynamicData.rank);
			List<int> list = new List<int> { paramPreset.totalParam, paramPreset.hp, paramPreset.atk, paramPreset.def };
			List<int> list2 = new List<int> { paramPreset2.totalParam, paramPreset2.hp, paramPreset2.atk, paramPreset2.def };
			this._charaGrowWild.SetWindowParam(list, list2, this._charaGrowRank.GrowRankGUI.rankUpResultWindow.ParamAll);
			for (int i = 0; i < this._charaGrowRank.GrowRankGUI.rankUpResultWindow.StarAll.Count; i++)
			{
				PguiImageCtrl pguiImageCtrl = this._charaGrowRank.GrowRankGUI.rankUpResultWindow.StarAll[i];
				PguiReplaceSpriteCtrl component2 = pguiImageCtrl.GetComponent<PguiReplaceSpriteCtrl>();
				component2.InitForce();
				component2.Replace((i < charaData.dynamicData.rank) ? 1 : 2);
				pguiImageCtrl.gameObject.SetActive(i < charaData.staticData.baseData.rankHigh);
			}
			string text = "";
			foreach (ItemInput itemInput in kemoBoardItem)
			{
				text = string.Format("{0}を{1}個獲得しました\n", DataManager.DmItem.GetItemStaticBase(itemInput.itemId).GetName(), itemInput.num);
			}
			this._charaGrowRank.GrowRankGUI.rankUpResultWindow.Txt_GetInfo.gameObject.SetActive(!string.IsNullOrEmpty(text));
			this._charaGrowRank.GrowRankGUI.rankUpResultWindow.Txt_GetInfo.text = text;
			while (!this._charaGrowRank.GrowRankGUI.rankUpResultWindow.owCtrl.FinishedOpen())
			{
				yield return null;
			}
			CanvasManager.SetEnableCmnTouchMask(false);
			while (this._effectStatus != SelCharaGrowCtrl.EffectStatus.ResultEnd)
			{
				yield return null;
			}
			CanvasManager.SetEnableCmnTouchMask(true);
			int num;
			for (int rank = beforedRank + 1; rank <= charaData.dynamicData.rank; rank = num + 1)
			{
				this.OpenRankUpClothesWindow(rank);
				while (this._effectStatus != SelCharaGrowCtrl.EffectStatus.ResultEnd)
				{
					yield return null;
				}
				num = rank;
			}
			yield break;
		}
		goto IL_028E;
	}

	private IEnumerator RankUpGageEffect()
	{
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(this._currentCharaId);
		int beforedRank = userCharaData.dynamicData.rank;
		IEnumerator ienum = this.RankUpGageEffectRequest(beforedRank + 1);
		while (ienum.MoveNext())
		{
			yield return null;
		}
		ienum = this.RankUpGageEffectAfterRequest(beforedRank);
		while (ienum.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	private void OnTouchMask(Transform tr)
	{
		this._touchScreenAuth = true;
	}

	private IEnumerator MiracleEffectRequest(int afterArtsLv)
	{
		CanvasManager.HdlMissionProgressCtrl.IsMiracleUp = true;
		DataManager.DmChara.RequestActionCharaArtsUp(this._currentCharaId, afterArtsLv);
		this._effectStatus = SelCharaGrowCtrl.EffectStatus.ReqServer;
		CanvasManager.SetEnableCmnTouchMask(true);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator MiracleEffectAfterRequest(int beforeArtsLv)
	{
		CanvasManager.SetEnableCmnTouchMask(true);
		CharaPackData charaData = DataManager.DmChara.GetUserCharaData(this._currentCharaId);
		this.CharaGrowSetup(this._currentCharaId, false);
		this._effectStatus = SelCharaGrowCtrl.EffectStatus.Execute;
		PguiAECtrl charaEffect = this._guiData.CharacterGrowMain.ImageCharaEffect;
		charaEffect.gameObject.SetActive(true);
		SoundManager.Play("prd_se_friends_arts_levelup", false, false);
		SoundManager.PlayVoice(DataManager.DmChara.GetCharaStaticData(this._currentCharaId).cueSheetName, VOICE_TYPE.AUP01);
		this._voiceLength = SoundManager.GetVoiceLength(DataManager.DmChara.GetCharaStaticData(this._currentCharaId).cueSheetName, VOICE_TYPE.AUP01);
		this._voiceStartTime = Time.time;
		this._renderTextureChara.SetAnimation(CharaMotionDefine.ActKey.POSITIVE, false, delegate
		{
			this._renderTextureChara.SetAnimation(CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true);
		});
		charaEffect.m_AEImage.playTime = 0f;
		charaEffect.m_AEImage.autoPlay = true;
		while (!charaEffect.m_AEImage.end)
		{
			yield return null;
		}
		charaEffect.gameObject.SetActive(false);
		this._effectStatus = SelCharaGrowCtrl.EffectStatus.Result;
		this._charaGrowMiracle.GrowMiracleGUI.miracleResultWindow.owCtrl.Setup(null, null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, new PguiOpenWindowCtrl.Callback(this.OnResultArtsWindowButtonCallback), null, false);
		this._charaGrowMiracle.GrowMiracleGUI.miracleResultWindow.owCtrl.Open();
		this._charaGrowMiracle.GrowMiracleGUI.miracleResultWindow.Txt_ArtsName.text = charaData.staticData.artsData.actionName;
		this._charaGrowMiracle.GrowMiracleGUI.miracleResultWindow.Num_Lv_Before.ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[]
		{
			SelCharaGrowCtrl.ARTS_TEXT_SIZE.ToString(),
			beforeArtsLv.ToString()
		});
		this._charaGrowMiracle.GrowMiracleGUI.miracleResultWindow.Num_Lv_After.ReplaceTextByDefault(new string[] { "Param01", "Param02", "Param03" }, new string[]
		{
			SelCharaGrowCtrl.ARTS_TEXT_SIZE.ToString(),
			charaData.dynamicData.artsLevel.ToString(),
			SelCharaGrowCtrl.ARTS_HIGHLIGHT_COLOR_CODE
		});
		while (!this._charaGrowMiracle.GrowMiracleGUI.miracleResultWindow.owCtrl.FinishedOpen())
		{
			yield return null;
		}
		CanvasManager.SetEnableCmnTouchMask(false);
		while (this._effectStatus != SelCharaGrowCtrl.EffectStatus.ResultEnd)
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator MiracleEffect()
	{
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(this._currentCharaId);
		int beforeArtsLv = userCharaData.dynamicData.artsLevel;
		IEnumerator ienum = this.MiracleEffectRequest(beforeArtsLv + 1);
		while (ienum.MoveNext())
		{
			yield return null;
		}
		ienum = this.MiracleEffectAfterRequest(beforeArtsLv);
		while (ienum.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator NanairoReleaseEffectRequest()
	{
		DataManager.DmChara.RequestActionCharaNanairo(this._currentCharaId);
		this._effectStatus = SelCharaGrowCtrl.EffectStatus.ReqServer;
		CanvasManager.SetEnableCmnTouchMask(true);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator NanairoReleaseEffectAfterRequest()
	{
		CanvasManager.SetEnableCmnTouchMask(true);
		CharaPackData charaData = DataManager.DmChara.GetUserCharaData(this._currentCharaId);
		this.CharaGrowSetup(this._currentCharaId, false);
		this._effectStatus = SelCharaGrowCtrl.EffectStatus.Execute;
		SelCharaGrowNanairo.CharaGrowNanairoGUI growNanairoGUI = this._charaGrowNanairo.GrowNanairoGUI;
		SelCharaGrowNanairo.NanairoReleaseTab nanairoUpTab = growNanairoGUI.nanairoUpTab;
		AEImage imageResult = nanairoUpTab.AEImage_result;
		imageResult.gameObject.SetActive(true);
		SoundManager.Play("prd_se_friends_arts_levelup", false, false);
		SoundManager.PlayVoice(DataManager.DmChara.GetCharaStaticData(this._currentCharaId).cueSheetName, VOICE_TYPE.AUP01);
		this._voiceLength = SoundManager.GetVoiceLength(DataManager.DmChara.GetCharaStaticData(this._currentCharaId).cueSheetName, VOICE_TYPE.AUP01);
		this._voiceStartTime = Time.time;
		this._renderTextureChara.SetAnimation(CharaMotionDefine.ActKey.POSITIVE, false, delegate
		{
			this._renderTextureChara.SetAnimation(CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true);
		});
		foreach (SelCharaGrowNanairo.NanairoItem nanairoItem in nanairoUpTab.iconItemList)
		{
			nanairoItem.iconItemCtrl.gameObject.SetActive(false);
			nanairoItem.iconItemCtrl.Clear();
			nanairoItem.itemNum.gameObject.SetActive(false);
		}
		nanairoUpTab.baseObj.SetActive(true);
		imageResult.playTime = 0f;
		imageResult.autoPlay = true;
		while (!imageResult.end)
		{
			yield return null;
		}
		imageResult.gameObject.SetActive(false);
		this._effectStatus = SelCharaGrowCtrl.EffectStatus.Result;
		SelCharaGrowNanairo.WindowNanairoResult nanairoResultWindow = growNanairoGUI.nanairoResultWindow;
		nanairoResultWindow.owCtrl.Setup("なないろとくせい解放！", null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int index)
		{
			this._effectStatus = SelCharaGrowCtrl.EffectStatus.ResultEnd;
			return true;
		}, null, false);
		nanairoResultWindow.Txt_NameNanairo.text = (charaData.IsHaveNanairoAbility ? charaData.staticData.nanairoAbilityData.abilityName : "");
		nanairoResultWindow.Txt_InfoNanairo.text = (charaData.IsHaveNanairoAbility ? charaData.staticData.nanairoAbilityData.abilityEffect : "");
		nanairoResultWindow.skillNanairo.SetActive(true);
		nanairoResultWindow.owCtrl.Open();
		growNanairoGUI.nanairoUpTab.baseObj.SetActive(false);
		while (!nanairoResultWindow.owCtrl.FinishedOpen())
		{
			yield return null;
		}
		CanvasManager.SetEnableCmnTouchMask(false);
		while (this._effectStatus != SelCharaGrowCtrl.EffectStatus.ResultEnd)
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator NanairoReleaseEffect()
	{
		IEnumerator ienum = this.NanairoReleaseEffectRequest();
		while (ienum.MoveNext())
		{
			yield return null;
		}
		ienum = this.NanairoReleaseEffectAfterRequest();
		while (ienum.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator PhotoPocketEffect()
	{
		CharaPackData charaData = DataManager.DmChara.GetUserCharaData(this._currentCharaId);
		DataManager.DmChara.RequestActionCharaReleasePhotoFrame(this._currentCharaId, charaData.dynamicData.PhotoFrameTotalStep);
		this._effectStatus = SelCharaGrowCtrl.EffectStatus.ReqServer;
		CanvasManager.SetEnableCmnTouchMask(true);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		this.CharaGrowSetup(this._currentCharaId, false);
		this._effectStatus = SelCharaGrowCtrl.EffectStatus.Execute;
		SelCharaGrowPhotoPocket.PhotoPocketResult photoPocketResult = this._charaGrowPhotoPocket.GrowPhotoPocketGUI.photoPocketResult;
		photoPocketResult.baseObj.SetActive(true);
		photoPocketResult.Setup(this._currentCharaId, this._nextOpenPhotoIndex);
		this._renderTextureChara.SetAnimation(CharaMotionDefine.ActKey.POSITIVE, false, delegate
		{
			this._renderTextureChara.SetAnimation(CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true);
		});
		CanvasManager.AddCallbackCmnTouchMask(new UnityAction<Transform>(this.OnTouchMask));
		this._touchScreenAuth = false;
		while (!this._touchScreenAuth)
		{
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				this._touchScreenAuth = true;
			}
			yield return null;
		}
		photoPocketResult.AEImage_Window.PlayAnimation(PguiAECtrl.AmimeType.END, delegate
		{
		});
		photoPocketResult.AEImage_Bg.PlayAnimation(PguiAECtrl.AmimeType.END, delegate
		{
		});
		while (!photoPocketResult.AEImage_Window.m_AEImage.end)
		{
			yield return null;
		}
		photoPocketResult.baseObj.SetActive(false);
		CanvasManager.RemoveCallbackCmnTouchMask();
		CanvasManager.SetEnableCmnTouchMask(false);
		if (charaData.IsHaveSpAbility && charaData.staticData.baseData.spAbilityRelPp == charaData.dynamicData.PhotoFrameTotalStep)
		{
			this._effectStatus = SelCharaGrowCtrl.EffectStatus.Result;
			SelCharaGrowWild.WindowTokuseiInfo tokuseiInfoWindow = this._charaGrowWild.GrowWildGUI.tokuseiInfoWindow;
			tokuseiInfoWindow.owCtrl.Setup(PrjUtil.MakeMessage("キセキとくせい解放！"), PrjUtil.MakeMessage("キセキとくせい解放！"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, new PguiOpenWindowCtrl.Callback(this.OnResultWildWindowButtonCallback), null, false);
			tokuseiInfoWindow.skill.SetActive(false);
			tokuseiInfoWindow.skillKiseki.SetActive(true);
			CharaStaticAbility spAbilityData = charaData.staticData.spAbilityData;
			tokuseiInfoWindow.Txt_NameKiseki.text = spAbilityData.abilityName;
			tokuseiInfoWindow.Txt_InfoKiseki.text = spAbilityData.abilityEffect;
			tokuseiInfoWindow.owCtrl.Open();
			while (this._effectStatus != SelCharaGrowCtrl.EffectStatus.ResultEnd)
			{
				yield return null;
			}
		}
		yield break;
	}

	private IEnumerator IEKizunaLimitOverRequest()
	{
		DataManager.DmChara.RequestActoinCharaKizunaLimitLevelUp(this._currentCharaId);
		this._effectStatus = SelCharaGrowCtrl.EffectStatus.ReqServer;
		CanvasManager.SetEnableCmnTouchMask(true);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator IEKizunaLimitOverAfterRequest()
	{
		CanvasManager.SetEnableCmnTouchMask(true);
		this._effectStatus = SelCharaGrowCtrl.EffectStatus.Execute;
		AEImage resultImage = this._charaGrowKizuna.GrowKizunaGUI.KizunaTab.ImageResult;
		resultImage.gameObject.SetActive(true);
		resultImage.playTime = 0f;
		resultImage.autoPlay = true;
		SoundManager.Play("prd_se_friends_bond_level_expansion", false, false);
		this._renderTextureChara.SetAnimation(CharaMotionDefine.ActKey.JOY, false, delegate
		{
			this._renderTextureChara.SetAnimation(CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true);
		});
		this._renderTextureChara.PlayVoice(VOICE_TYPE.JOY01);
		this._voiceLength = SoundManager.GetVoiceLength(DataManager.DmChara.GetCharaStaticData(this._currentCharaId).cueSheetName, VOICE_TYPE.JOY01);
		this._voiceStartTime = Time.time;
		while (!resultImage.end)
		{
			yield return null;
		}
		resultImage.gameObject.SetActive(false);
		this._charaGrowKizuna.UpdateLimitLvItemActivation(this._currentCharaId);
		if (this._growMultiCoroutine == null)
		{
			this._effectStatus = SelCharaGrowCtrl.EffectStatus.Finished;
		}
		else
		{
			this._effectStatus = SelCharaGrowCtrl.EffectStatus.ResultEnd;
		}
		this.CharaGrowSetup(this._currentCharaId, false);
		yield break;
	}

	private IEnumerator IEKizunaLimitOver()
	{
		IEnumerator ienum = this.IEKizunaLimitOverRequest();
		while (ienum.MoveNext())
		{
			yield return null;
		}
		ienum = this.IEKizunaLimitOverAfterRequest();
		while (ienum.MoveNext())
		{
			yield return null;
		}
		yield break;
	}

	private IEnumerator ItemExchangeEffect()
	{
		CharaPackData charaData = DataManager.DmChara.GetUserCharaData(this._currentCharaId);
		int beforeNum = 0;
		int afterNum = 0;
		string beforeName = "";
		string afterName = "";
		if (this._guiData.CharacterGrowMain.Cmn.TabGroup.SelectIndex == 5)
		{
			ItemData userItemData = DataManager.DmItem.GetUserItemData(charaData.staticData.baseData.ppItemId);
			beforeNum = userItemData.num;
			beforeName = userItemData.staticData.GetName();
			MstCharaRankData mstCharaRankData = DataManager.DmServerMst.mstCharaRankDataList.Find((MstCharaRankData x) => x.rankId == charaData.staticData.baseData.rankId && x.rank == charaData.staticData.baseData.rankLow);
			afterNum = userItemData.num * mstCharaRankData.pxExRate;
			afterName = DataManager.DmItem.GetUserItemData(30104).staticData.GetName();
			DataManager.DmItem.RequestCharaPxChange(this._currentCharaId, userItemData.num);
		}
		else
		{
			List<MstCharaRankData> list = DataManager.DmServerMst.mstCharaRankDataList.FindAll((MstCharaRankData item) => item.rankId == charaData.staticData.baseData.gradeTableId && item.rank > charaData.dynamicData.rank);
			int holdStone = 0;
			list.ForEach(delegate(MstCharaRankData item)
			{
				holdStone += item.useCostFragmentNum;
			});
			ItemData userItemData2 = DataManager.DmItem.GetUserItemData(charaData.staticData.baseData.rankItemId);
			beforeNum = userItemData2.num - holdStone;
			beforeName = userItemData2.staticData.GetName();
			afterNum = userItemData2.num - holdStone;
			afterName = DataManager.DmItem.GetUserItemData(30119).staticData.GetName();
			DataManager.DmItem.RequestCharaSpxChange(this._currentCharaId, userItemData2.num - holdStone);
		}
		this._effectStatus = SelCharaGrowCtrl.EffectStatus.ReqServer;
		CanvasManager.SetEnableCmnTouchMask(true);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		this.CharaGrowSetup(this._currentCharaId, false);
		CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("変換完了"), PrjUtil.MakeMessage(string.Format("{0}\n{1}個を\n{2}\n{3}個に変換しました", new object[] { beforeName, beforeNum, afterName, afterNum })), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int index) => true, null, false);
		CanvasManager.HdlOpenWindowBasic.Open();
		yield break;
	}

	private IEnumerator GrowthAchievementRewardEffect(DataManagerCharaMission.DynamicCharaMission.MissionOne dmo, DataManagerCharaMission.StaticMission sbd)
	{
		this._GettingAchievement = true;
		this._effectStatus = SelCharaGrowCtrl.EffectStatus.ReqServer;
		DataManager.DmChMission.RequestActionMissionRewardOne(dmo);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		List<ItemData> list = new List<ItemData>
		{
			new ItemData(sbd.RewardItemId, sbd.RewardItemNum)
		};
		GetItemWindowCtrl hdlGetItemWindowCtrl = CanvasManager.HdlGetItemWindowCtrl;
		List<ItemData> list2 = list;
		GetItemWindowCtrl.SetupParam setupParam = new GetItemWindowCtrl.SetupParam();
		setupParam.strCharaCb = (GetItemWindowCtrl.WordingCallbackParam param) => PrjUtil.MakeMessage(param.itemStaticBase.GetName() + "\nが探検隊に加わりました\nプレゼントボックスをご確認ください");
		setupParam.strPhotoCb = (GetItemWindowCtrl.WordingCallbackParam param) => PrjUtil.MakeMessage(string.Format("{0}\nを {1}枚 受け取りました", param.itemStaticBase.GetName(), param.itemNum));
		setupParam.strItemCb = (GetItemWindowCtrl.WordingCallbackParam param) => PrjUtil.MakeMessage(string.Format("{0}\n× {1} を 受け取りました", param.itemStaticBase.GetName(), param.itemNum));
		setupParam.windowFinishedCallback = delegate(int index)
		{
			this._GettingAchievement = false;
			return true;
		};
		hdlGetItemWindowCtrl.Setup(list2, setupParam);
		CanvasManager.HdlGetItemWindowCtrl.Open();
		this._guiData.GrowthAchievementRewardWindow.Refresh(this._currentCharaId);
		this.CharaGrowSetup(this._currentCharaId, false);
		yield break;
	}

	private IEnumerator IEReleaseAccessory()
	{
		DataManager.DmChara.RequestActoinCharaAccessoryOpen(this._currentCharaId);
		this._effectStatus = SelCharaGrowCtrl.EffectStatus.ReqServer;
		CanvasManager.SetEnableCmnTouchMask(true);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		this._effectStatus = SelCharaGrowCtrl.EffectStatus.Execute;
		this._guiData.CharacterGrowMain.BtnAccessoryImageOpen.gameObject.SetActive(true);
		this._guiData.CharacterGrowMain.BtnAccessoryImageOpen.playTime = 0f;
		this._guiData.CharacterGrowMain.BtnAccessoryImageOpen.autoPlay = true;
		this._guiData.CharacterGrowMain.BtnAccessoryImageOpenOK.gameObject.SetActive(false);
		SoundManager.Play("prd_se_accessory_slot_release", false, false);
		while (!this._guiData.CharacterGrowMain.BtnAccessoryImageOpen.end)
		{
			yield return null;
		}
		this.CharaGrowSetup(this._currentCharaId, false);
		CanvasManager.SetEnableCmnTouchMask(false);
		this._effectStatus = SelCharaGrowCtrl.EffectStatus.Finished;
		yield break;
	}

	private IEnumerator ConnectFlowBase(UnityAction connectBef, UnityAction connectAft)
	{
		connectBef();
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		connectAft();
		yield break;
	}

	private void SetupLvUpItem(ItemStaticBase expAddItem, int attr, int index)
	{
		SelCharaGrowLevel.LvUpTab lvUpTab = this._charaGrowLevelUp.LevelUpGUI.lvUpTab;
		int itemId = expAddItem.GetId();
		SelCharaGrowLevel.LvUpItem lvUpItem = lvUpTab.itemListBar[attr].IconItemList[index];
		lvUpItem.iconItemCtrl.gameObject.SetActive(true);
		lvUpItem.itemId = itemId;
		lvUpItem.iconItemCtrl.Setup(expAddItem, -1);
		lvUpItem.itemNum.gameObject.SetActive(true);
		lvUpItem.itemCount.gameObject.SetActive(true);
		lvUpItem.expBonus.gameObject.SetActive(false);
		lvUpItem.ColorBase.gameObject.SetActive(false);
		Dictionary<int, ItemData> userItemMap = DataManager.DmItem.GetUserItemMap();
		int num = (userItemMap.ContainsKey(itemId) ? userItemMap[itemId].num : 0);
		lvUpItem.itemNum.text = num.ToString();
		lvUpItem.iconItemCtrl.SetActEnable(0 < num);
		lvUpItem.iconItemCtrl.GetComponent<RectTransform>();
		lvUpItem.iconItemCtrl.AddOnClickListener(delegate(IconItemCtrl x)
		{
			this.OnTouchLvUpIcon(lvUpItem.itemId, false);
		});
		lvUpItem.iconItemCtrl.AddOnLongClickListener(delegate(IconItemCtrl x)
		{
			this.OnLongTouchlvUpIcon(itemId);
		});
	}

	private void SetupKizunaLvUpItem(ItemStaticBase expAddItem, int attr, int index)
	{
		SelCharaGrowKizuna.KizunaLvUpTab kizunaLvUpTab = this._charaGrowKizuna.GrowKizunaGUI.KizunaLvUpTab;
		int itemId = expAddItem.GetId();
		SelCharaGrowKizuna.KizunaLvUpItem lvUpItem = kizunaLvUpTab.ItemListBar[attr].IconItemListKizuna[index];
		int num = DataManager.DmItem.GetUserItemData(itemId).num;
		lvUpItem.SetUp(itemId, expAddItem, num, delegate(IconItemCtrl x)
		{
			this.OnTouchLvUpIcon(lvUpItem.ItemId, true);
		}, delegate(IconItemCtrl x)
		{
			this.OnLongTouchKizunalvUpIcon(itemId);
		});
	}

	private int GetAttributeFromIndex(int index)
	{
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(this._currentCharaId);
		int num = 0;
		if (index == 0)
		{
			num = (int)userCharaData.staticData.baseData.attribute;
		}
		else if (index == 1)
		{
			num = 0;
		}
		else if (index <= 6)
		{
			List<int> list = new List<int>();
			list.Add(-1);
			list.Add(-1);
			list.Add(1);
			list.Add(2);
			list.Add(3);
			list.Add(4);
			list.Add(5);
			list.Add(6);
			list.Remove((int)userCharaData.staticData.baseData.attribute);
			num = list[index];
		}
		return num;
	}

	private SelCharaGrowCtrl.CommonGUI.ItemListBar SetupAttrBar(GameObject go, int attr)
	{
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(this._currentCharaId);
		SelCharaGrowCtrl.CommonGUI.ItemListBar itemListBar = new SelCharaGrowCtrl.CommonGUI.ItemListBar(go.transform);
		PguiReplaceSpriteCtrl component = itemListBar.ImgAtr.GetComponent<PguiReplaceSpriteCtrl>();
		component.InitForce();
		Transform transform = itemListBar.BaseObj.transform.Find("ColorBase");
		PguiColorCtrl component2 = transform.GetComponent<PguiColorCtrl>();
		component2.InitForce();
		PguiImageCtrl component3 = transform.GetComponent<PguiImageCtrl>();
		itemListBar.TxtExpBonus.gameObject.SetActive(attr == (int)userCharaData.staticData.baseData.attribute || attr == 0);
		itemListBar.TxtAll.gameObject.SetActive(attr == 0);
		PguiReplaceSpriteCtrl component4 = transform.GetComponent<PguiReplaceSpriteCtrl>();
		component4.InitForce();
		int num = attr;
		int num2 = 0;
		Color color = component2.GetGameObjectById(attr.ToString());
		switch (attr)
		{
		case 1:
		case 2:
			break;
		case 3:
			num = 0;
			break;
		case 4:
			num = 3;
			break;
		case 5:
			num = 4;
			break;
		case 6:
			num = 5;
			break;
		default:
			num = 6;
			num2 = 1;
			color = Color.white;
			break;
		}
		component.Replace(num);
		component3.m_Image.color = color;
		component4.Replace(num2);
		return itemListBar;
	}

	private void OnStartItemLvUp(int index, GameObject go)
	{
		SelCharaGrowLevel.LvUpTab lvUpTab = this._charaGrowLevelUp.LevelUpGUI.lvUpTab;
		int attributeFromIndex = this.GetAttributeFromIndex(index);
		lvUpTab.itemListBar.Add(this.SetupAttrBar(go, attributeFromIndex));
		List<ItemStaticBase> expAddItemList = this._charaGrowLevelUp.GetExpAddItemList(attributeFromIndex);
		for (int i = 0; i < 3; i++)
		{
			int num = ((i < expAddItemList.Count) ? expAddItemList[i].GetId() : 0);
			this._charaGrowLevelUp.CreateLvUpItem(lvUpTab.itemListBar[index].Grid.gameObject, i, num, index);
			if (num != 0)
			{
				this.SetupLvUpItem(expAddItemList[i], index, i);
			}
		}
	}

	private void OnStartItemKizunaLvUp(int index, GameObject go)
	{
		SelCharaGrowKizuna.KizunaLvUpTab kizunaLvUpTab = this._charaGrowKizuna.GrowKizunaGUI.KizunaLvUpTab;
		int num = 0;
		if (index != 0)
		{
			return;
		}
		kizunaLvUpTab.ItemListBar.Add(this.SetupAttrBar(go, num));
		List<ItemStaticBase> expAddItemList = this._charaGrowKizuna.GetExpAddItemList(num);
		for (int i = 0; i < 3; i++)
		{
			int num2 = ((i < expAddItemList.Count) ? expAddItemList[i].GetId() : 0);
			this._charaGrowKizuna.CreateLvUpItem(kizunaLvUpTab.ItemListBar[index].Grid.gameObject, i, num2, index);
			if (num2 != 0)
			{
				this.SetupKizunaLvUpItem(expAddItemList[i], 0, i);
			}
		}
	}

	private void OnUpdateItemLvUp(int index, GameObject go)
	{
		int attributeFromIndex = this.GetAttributeFromIndex(index);
		this.SetupAttrBar(go, attributeFromIndex);
		List<ItemStaticBase> expAddItemList = this._charaGrowLevelUp.GetExpAddItemList(attributeFromIndex);
		for (int i = 0; i < 3; i++)
		{
			if (i < expAddItemList.Count && expAddItemList[i].GetId() != 0)
			{
				this.SetupLvUpItem(expAddItemList[i], index, i);
			}
		}
	}

	private void OnUpdateItemKizunaLvUp(int index, GameObject go)
	{
		int num = 0;
		this.SetupAttrBar(go, num);
		List<ItemStaticBase> expAddItemList = this._charaGrowKizuna.GetExpAddItemList(num);
		for (int i = 0; i < 3; i++)
		{
			if (i < expAddItemList.Count && expAddItemList[i].GetId() != 0)
			{
				this.SetupKizunaLvUpItem(expAddItemList[i], 0, i);
			}
		}
	}

	private void OnStartCharaTop(int index, GameObject go)
	{
		GameObject gameObject = (GameObject)Resources.Load("SceneCharaEdit/GUI/Prefab/CharaGrow_Btn_CharaSelect");
		for (int i = 0; i < 3; i++)
		{
			GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject, go.transform);
			IconCharaCtrl icon = gameObject2.GetComponent<IconCharaCtrl>();
			gameObject2.name = i.ToString();
			gameObject2.GetComponent<PguiButtonCtrl>().AddOnClickListener(delegate(PguiButtonCtrl button)
			{
				this.OnClickCharaTopButton(icon.charaPackData.id);
			}, PguiButtonCtrl.SoundType.DEFAULT);
		}
	}

	private void OnUpdateCharaTop(int index, GameObject go)
	{
		for (int i = 0; i < 3; i++)
		{
			int num = index * 3 + i;
			Transform transform = go.transform.Find(i.ToString());
			if (transform)
			{
				IconCharaCtrl component = transform.GetComponent<IconCharaCtrl>();
				if (num < this._dispCharaPackList.Count)
				{
					component.Setup(this._dispCharaPackList[num], this._sortType, false, null, 0, -1, 0);
					component.DispPhotoPocketLevel(true);
				}
				else
				{
					transform.GetComponent<IconCharaCtrl>().Setup(null, SortFilterDefine.SortType.LEVEL, false, null, 0, -1, 0);
				}
			}
		}
	}

	private void OnTouchLvUpIcon(int itemId, bool isKizuna = false)
	{
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(this._currentCharaId);
		int num = (isKizuna ? userCharaData.dynamicData.kizunaLevel : userCharaData.dynamicData.level);
		int num2 = (isKizuna ? userCharaData.dynamicData.KizunaLimitLevel : userCharaData.dynamicData.limitLevel);
		if (num >= num2)
		{
			return;
		}
		int needGold = ItemDef.GetAddCharaLevelExpBase(itemId, userCharaData.staticData.baseData.attribute).needGold;
		if (this._lvUpCostCoin + needGold > DataManager.DmItem.GetUserItemData(30101).num)
		{
			return;
		}
		Dictionary<int, ItemData> userItemMap = DataManager.DmItem.GetUserItemMap();
		DataManager.DmItem.GetUserItemListByKind(ItemDef.Kind.EXP_ADD);
		if (!userItemMap.ContainsKey(itemId))
		{
			return;
		}
		if (userItemMap[itemId].num <= 0)
		{
			return;
		}
		if (isKizuna)
		{
			if (userItemMap[itemId].num <= this._selectKizunaLvUpItemIdList.Count<int>((int item) => item == itemId))
			{
				return;
			}
			if (!this.CheckKizunaLvUp(itemId))
			{
				return;
			}
			this._selectKizunaLvUpItemIdList.Add(itemId);
			this.SelectKizunaLvUp();
		}
		else
		{
			if (userItemMap[itemId].num <= this._selectLvUpItemIdList.Count<int>((int item) => item == itemId))
			{
				return;
			}
			if (!this.CheckLvUp(itemId))
			{
				return;
			}
			this._selectLvUpItemIdList.Add(itemId);
			this.SelectLvUp();
		}
		int selectIndex = this._guiData.CharacterGrowMain.Cmn.TabGroup.SelectIndex;
		this.SetOwnCoin(selectIndex);
		this.SetMaxInfo(selectIndex, false, false);
		this.SetActStrengthButton(selectIndex, false);
		SoundManager.Play("prd_se_click", false, false);
	}

	private void OnLongTouchlvUpIcon(int itemId)
	{
		ItemData userItemData = DataManager.DmItem.GetUserItemData(itemId);
		SelCharaGrowLevel.WindowItemUse itemUseWindow = this._charaGrowLevelUp.LevelUpGUI.itemUseWindow;
		itemUseWindow.owCtrl.Setup(this.USING_ITEM_NUM_SELECTION_TITLE_TEXT, null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.OK), true, new PguiOpenWindowCtrl.Callback(this.OnItemUseWindowCallback), null, false);
		itemUseWindow.owCtrl.Open();
		itemUseWindow.lvUpItem.iconItemCtrl.gameObject.SetActive(true);
		itemUseWindow.lvUpItem.iconItemCtrl.Setup(userItemData.staticData, -1);
		itemUseWindow.lvUpItem.itemId = itemId;
		int num = this.CalcItemMaxValue(itemId, false);
		int count = this._selectLvUpItemIdList.FindAll((int item) => item == itemId).Count;
		num = ((count > num) ? count : num);
		itemUseWindow.SliderBar.minValue = 0f;
		itemUseWindow.SliderBar.maxValue = (float)num;
		itemUseWindow.SliderBar.value = (float)count;
		itemUseWindow.SliderBar.onValueChanged.AddListener(new UnityAction<float>(this.OnValueChanged));
		this.SelectLvUpForWindow(count);
	}

	private void OnLongTouchKizunalvUpIcon(int itemId)
	{
		ItemData userItemData = DataManager.DmItem.GetUserItemData(itemId);
		SelCharaGrowKizuna.WindowItemUse itemUseWindow = this._charaGrowKizuna.LevelUpGUI.ItemUseWindow;
		itemUseWindow.OpenWindowCtrl.Setup(this.USING_ITEM_NUM_SELECTION_TITLE_TEXT, null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.OK), true, new PguiOpenWindowCtrl.Callback(this.OnKizunaItemUseWindowCallback), null, false);
		itemUseWindow.OpenWindowCtrl.Open();
		itemUseWindow.LvUpItem.iconItemCtrl.gameObject.SetActive(true);
		itemUseWindow.LvUpItem.iconItemCtrl.Setup(userItemData.staticData, -1);
		itemUseWindow.LvUpItem.itemId = itemId;
		int num = this.CalcItemMaxValue(itemId, true);
		int count = this._selectKizunaLvUpItemIdList.FindAll((int item) => item == itemId).Count;
		num = ((count > num) ? count : num);
		itemUseWindow.SliderBar.minValue = 0f;
		itemUseWindow.SliderBar.maxValue = (float)num;
		itemUseWindow.SliderBar.value = (float)count;
		itemUseWindow.SliderBar.onValueChanged.AddListener(new UnityAction<float>(this.OnValueChangedKizuna));
		this.SelectKizunaLvUpForWindow(count);
	}

	private bool OnItemUseWindowCallback(int index)
	{
		SelCharaGrowLevel.WindowItemUse itemUseWindow = this._charaGrowLevelUp.LevelUpGUI.itemUseWindow;
		if (index == 0)
		{
			int num = Mathf.FloorToInt(itemUseWindow.SliderBar.value);
			int itemId = itemUseWindow.lvUpItem.itemId;
			int count = this._selectLvUpItemIdList.FindAll((int n) => n == itemId).Count;
			if (num > count)
			{
				for (int i = num - count; i > 0; i--)
				{
					this._selectLvUpItemIdList.Add(itemId);
				}
			}
			else if (num < count)
			{
				for (int j = count - num; j > 0; j--)
				{
					this._selectLvUpItemIdList.Remove(itemId);
				}
			}
			this.SelectLvUp();
			int selectIndex = this._guiData.CharacterGrowMain.Cmn.TabGroup.SelectIndex;
			this.SetOwnCoin(selectIndex);
			this.SetActStrengthButton(selectIndex, false);
		}
		return true;
	}

	private bool OnKizunaItemUseWindowCallback(int index)
	{
		SelCharaGrowKizuna.WindowItemUse itemUseWindow = this._charaGrowKizuna.LevelUpGUI.ItemUseWindow;
		if (index == 0)
		{
			int num = Mathf.FloorToInt(itemUseWindow.SliderBar.value);
			int itemId = itemUseWindow.LvUpItem.itemId;
			int count = this._selectKizunaLvUpItemIdList.FindAll((int n) => n == itemId).Count;
			if (num > count)
			{
				for (int i = num - count; i > 0; i--)
				{
					this._selectKizunaLvUpItemIdList.Add(itemId);
				}
			}
			else if (num < count)
			{
				for (int j = count - num; j > 0; j--)
				{
					this._selectKizunaLvUpItemIdList.Remove(itemId);
				}
			}
			this.SelectKizunaLvUp();
			int selectIndex = this._guiData.CharacterGrowMain.Cmn.TabGroup.SelectIndex;
			this.SetOwnCoin(selectIndex);
			this.SetActStrengthButton(selectIndex, false);
		}
		return true;
	}

	private void OnValueChanged(float val)
	{
		int num = Mathf.FloorToInt(val);
		this.SelectLvUpForWindow(num);
	}

	private void OnValueChangedKizuna(float val)
	{
		int num = Mathf.FloorToInt(val);
		this.SelectKizunaLvUpForWindow(num);
	}

	private void SelectLvUpForWindow(int value)
	{
		CharaDynamicData dynamicData = DataManager.DmChara.GetUserCharaData(this._currentCharaId).dynamicData;
		SelCharaGrowLevel.WindowItemUse itemUseWindow = this._charaGrowLevelUp.LevelUpGUI.itemUseWindow;
		SelCharaGrowLevel.LvUpItem lvUpItem = itemUseWindow.lvUpItem;
		ItemData userItemData = DataManager.DmItem.GetUserItemData(lvUpItem.itemId);
		List<ItemInput> list = new List<ItemInput>();
		int num = 0;
		foreach (int num2 in this._selectLvUpItemIdList)
		{
			if (userItemData.id == num2)
			{
				num++;
				if (value < num)
				{
					continue;
				}
			}
			ItemInput itemInput = new ItemInput(num2, 1);
			list.Add(itemInput);
		}
		if (value > num)
		{
			for (int i = value - num; i > 0; i--)
			{
				ItemInput itemInput2 = new ItemInput(userItemData.id, 1);
				list.Add(itemInput2);
			}
		}
		DataManagerChara.SimulateAddExpResult simulateAddExpResult = new DataManagerChara.SimulateAddExpResult();
		simulateAddExpResult = DataManager.DmChara.SimulateAddExp(DataManager.DmChara.GetUserCharaData(this._currentCharaId), list);
		List<ItemInput> list2 = list.FindAll((ItemInput n) => n.itemId == lvUpItem.itemId);
		bool flag = list.Count > 0;
		itemUseWindow.Num_Lv_Before.gameObject.SetActive(flag);
		itemUseWindow.Num_Lv_After.gameObject.SetActive(true);
		itemUseWindow.Img_Yaji.gameObject.SetActive(flag);
		itemUseWindow.Gage_Up.gameObject.SetActive(flag);
		bool flag2 = list2.Count == 0;
		bool flag3 = Mathf.FloorToInt(itemUseWindow.SliderBar.maxValue) == value;
		itemUseWindow.Btn_Minus.SetActEnable(!flag2, false, false);
		itemUseWindow.Btn_Plus.SetActEnable(!flag3, false, false);
		lvUpItem.iconItemCtrl.SetActEnable(!flag3);
		lvUpItem.itemNum.gameObject.SetActive(true);
		lvUpItem.itemNum.text = userItemData.num.ToString();
		lvUpItem.imgCount.gameObject.SetActive(0 < list2.Count);
		lvUpItem.itemCount.gameObject.SetActive(0 < list2.Count);
		lvUpItem.itemCount.text = list2.Count.ToString();
		itemUseWindow.Num_Lv_Before.text = this.DEFAULT_LEVEL_TEXT + PrjUtil.MakeMessage(dynamicData.level.ToString() + "/" + dynamicData.limitLevel.ToString());
		string text = this.DEFAULT_LEVEL_TEXT + ((simulateAddExpResult.level > dynamicData.level) ? ("<color=#FF7B16FF>" + simulateAddExpResult.level.ToString() + "</color>") : simulateAddExpResult.level.ToString());
		itemUseWindow.Num_Lv_After.text = PrjUtil.MakeMessage(text + "/" + dynamicData.limitLevel.ToString());
		itemUseWindow.Gage_Up.m_Image.fillAmount = (float)simulateAddExpResult.exp / (float)DataManager.DmChara.GetExpByNextLevel(dynamicData.id, simulateAddExpResult.level);
		itemUseWindow.Gage.gameObject.SetActive(dynamicData.level >= simulateAddExpResult.level);
		long num3 = ((simulateAddExpResult.level >= dynamicData.limitLevel) ? 0L : (DataManager.DmChara.GetExpByNextLevel(dynamicData.id, simulateAddExpResult.level) - simulateAddExpResult.exp));
		itemUseWindow.Num_Exp_Next.text = PrjUtil.MakeMessage(this.NUM_EXP_NEXT_TEXT + num3.ToString());
		itemUseWindow.Num_BeforeCoin.text = DataManager.DmItem.GetUserItemData(30101).num.ToString();
		itemUseWindow.Num_AfterCoin.text = (DataManager.DmItem.GetUserItemData(30101).num - simulateAddExpResult.costGold).ToString();
		if (simulateAddExpResult.level >= dynamicData.limitLevel)
		{
			itemUseWindow.Gage_Up.m_Image.fillAmount = 1f;
		}
		itemUseWindow.Gage.m_Image.fillAmount = (float)dynamicData.exp / (float)DataManager.DmChara.GetExpByNextLevel(dynamicData.id, dynamicData.level);
		if (simulateAddExpResult.level >= dynamicData.limitLevel)
		{
			itemUseWindow.Gage.m_Image.fillAmount = 1f;
		}
	}

	private void SelectKizunaLvUpForWindow(int value)
	{
		CharaDynamicData dynamicData = DataManager.DmChara.GetUserCharaData(this._currentCharaId).dynamicData;
		SelCharaGrowKizuna.WindowItemUse itemUseWindow = this._charaGrowKizuna.LevelUpGUI.ItemUseWindow;
		SelCharaGrowLevel.LvUpItem lvUpItem = itemUseWindow.LvUpItem;
		ItemData userItemData = DataManager.DmItem.GetUserItemData(lvUpItem.itemId);
		List<ItemInput> list = new List<ItemInput>();
		int num = 0;
		foreach (int num2 in this._selectKizunaLvUpItemIdList)
		{
			if (userItemData.id == num2)
			{
				num++;
				if (value < num)
				{
					continue;
				}
			}
			ItemInput itemInput = new ItemInput(num2, 1);
			list.Add(itemInput);
		}
		if (value > num)
		{
			for (int i = value - num; i > 0; i--)
			{
				ItemInput itemInput2 = new ItemInput(userItemData.id, 1);
				list.Add(itemInput2);
			}
		}
		DataManagerChara.SimulateAddExpResult simulateAddExpResult = new DataManagerChara.SimulateAddExpResult();
		simulateAddExpResult = DataManager.DmChara.SimulateAddKizunaExp(DataManager.DmChara.GetUserCharaData(this._currentCharaId), list);
		List<ItemInput> list2 = list.FindAll((ItemInput n) => n.itemId == lvUpItem.itemId);
		bool flag = list.Count > 0;
		itemUseWindow.SetActiveCtrl(flag);
		bool flag2 = list2.Count == 0;
		bool flag3 = Mathf.FloorToInt(itemUseWindow.SliderBar.maxValue) == value;
		itemUseWindow.BtnMinus.SetActEnable(!flag2, false, false);
		itemUseWindow.BtnPlus.SetActEnable(!flag3, false, false);
		itemUseWindow.SetUpLvUpItem(flag3, userItemData.num, list2.Count);
		int kizunaLimitLevel = dynamicData.KizunaLimitLevel;
		int kizunaLevel = dynamicData.kizunaLevel;
		itemUseWindow.NumLvBefore.text = this.DEFAULT_LEVEL_TEXT + PrjUtil.MakeMessage(kizunaLevel.ToString() + "/" + kizunaLimitLevel.ToString());
		string text = this.DEFAULT_LEVEL_TEXT + ((simulateAddExpResult.level > kizunaLevel) ? ("<color=#FF7B16FF>" + simulateAddExpResult.level.ToString() + "</color>") : simulateAddExpResult.level.ToString());
		itemUseWindow.NumLvAfter.text = PrjUtil.MakeMessage(text + "/" + kizunaLimitLevel.ToString());
		itemUseWindow.GageUp.m_Image.fillAmount = (float)simulateAddExpResult.exp / (float)DataManager.DmChara.GetExpByNextKizunaLevel(dynamicData.id, simulateAddExpResult.level);
		itemUseWindow.Gage.gameObject.SetActive(kizunaLevel >= simulateAddExpResult.level);
		long num3 = ((simulateAddExpResult.level >= kizunaLimitLevel) ? 0L : (DataManager.DmChara.GetExpByNextKizunaLevel(dynamicData.id, simulateAddExpResult.level) - simulateAddExpResult.exp));
		itemUseWindow.NumExpNext.text = PrjUtil.MakeMessage(this.NEXT_KIZUNA_LEVEL_DESCRIPTION_TEXT + num3.ToString());
		itemUseWindow.NumBeforeCoin.text = DataManager.DmItem.GetUserItemData(30101).num.ToString();
		itemUseWindow.NumAfterCoin.text = (DataManager.DmItem.GetUserItemData(30101).num - simulateAddExpResult.costGold).ToString();
		if (simulateAddExpResult.level >= kizunaLimitLevel)
		{
			itemUseWindow.GageUp.m_Image.fillAmount = 1f;
		}
		itemUseWindow.Gage.m_Image.fillAmount = (float)dynamicData.kizunaExp / (float)DataManager.DmChara.GetExpByNextKizunaLevel(dynamicData.id, kizunaLevel);
		if (simulateAddExpResult.level >= kizunaLimitLevel)
		{
			itemUseWindow.Gage.m_Image.fillAmount = 1f;
		}
	}

	private void OnClickButtonForItemUse(PguiButtonCtrl button)
	{
		SelCharaGrowLevel.WindowItemUse itemUseWindow = this._charaGrowLevelUp.LevelUpGUI.itemUseWindow;
		int num = Mathf.FloorToInt(itemUseWindow.SliderBar.value);
		int num2 = Mathf.FloorToInt(itemUseWindow.SliderBar.maxValue);
		if (itemUseWindow.Btn_Minus == button)
		{
			if (num > 0)
			{
				num--;
			}
		}
		else if (itemUseWindow.Btn_Plus == button && num < num2)
		{
			num++;
		}
		itemUseWindow.SliderBar.value = (float)num;
		this.SelectLvUpForWindow(num);
	}

	private void OnClickButtonForKizunaItemUse(PguiButtonCtrl button)
	{
		SelCharaGrowKizuna.WindowItemUse itemUseWindow = this._charaGrowKizuna.LevelUpGUI.ItemUseWindow;
		int num = Mathf.FloorToInt(itemUseWindow.SliderBar.value);
		int num2 = Mathf.FloorToInt(itemUseWindow.SliderBar.maxValue);
		if (itemUseWindow.BtnMinus == button)
		{
			if (num > 0)
			{
				num--;
			}
		}
		else if (itemUseWindow.BtnPlus == button && num < num2)
		{
			num++;
		}
		itemUseWindow.SliderBar.value = (float)num;
		this.SelectKizunaLvUpForWindow(num);
	}

	private void OnTouchWildIcon(int index)
	{
		this.ChangeCurrentWildIcon(index);
		this._charaGrowWild.SetupItemInfoWild(this._currentCharaId);
		this.SetOwnCoin(this._guiData.CharacterGrowMain.Cmn.TabGroup.SelectIndex);
		this.SetMaxInfo(this._guiData.CharacterGrowMain.Cmn.TabGroup.SelectIndex, false, false);
		this.SetActStrengthButton(this._guiData.CharacterGrowMain.Cmn.TabGroup.SelectIndex, false);
		SoundManager.Play("prd_se_click", false, false);
	}

	private void ChangeCurrentWildIcon(int index)
	{
		SelCharaGrowWild.WildReleaseTab wildReleaseTab = this._charaGrowWild.GrowWildGUI.wildReleaseTab;
		wildReleaseTab.iconItemList[this._charaGrowWild.currentIndexWild].current.SetActive(false);
		this._charaGrowWild.currentIndexWild = index;
		wildReleaseTab.iconItemList[this._charaGrowWild.currentIndexWild].current.SetActive(true);
	}

	private void ClearSelectLvUpItem(bool isKizuna = false)
	{
		this._selectLvUpItemIdList.Clear();
		this._selectKizunaLvUpItemIdList.Clear();
		foreach (SelCharaGrowCtrl.CommonGUI.ItemListBar itemListBar in (isKizuna ? this._charaGrowKizuna.GrowKizunaGUI.KizunaLvUpTab.ItemListBar : this._charaGrowLevelUp.LevelUpGUI.lvUpTab.itemListBar))
		{
			foreach (SelCharaGrowLevel.LvUpItem lvUpItem in itemListBar.IconItemList)
			{
				int num = DataManager.DmItem.GetUserItemData(lvUpItem.itemId).num;
				lvUpItem.iconItemCtrl.SetActEnable(0 < num);
				lvUpItem.imgCount.gameObject.SetActive(false);
			}
		}
	}

	private void OnClickButton(PguiButtonCtrl button)
	{
		CharaPackData charaData = DataManager.DmChara.GetUserCharaData(this._currentCharaId);
		switch (this._guiData.CharacterGrowMain.Cmn.TabGroup.SelectIndex)
		{
		case 0:
			if (button == this._guiData.CharacterGrowMain.Cmn.ButtonL)
			{
				this.ClearSelectLvUpItem(false);
				this.SetActStrengthButton(this._guiData.CharacterGrowMain.Cmn.TabGroup.SelectIndex, false);
				this.SelectLvUp();
				return;
			}
			if (button == this._guiData.CharacterGrowMain.Cmn.ButtonR)
			{
				List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>> list = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>
				{
					new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.NEGATIVE, this.CANCEL_TEXT),
					new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, this.ENHANCE_TEXT)
				};
				this._charaGrowLevelUp.LevelUpGUI.lvUpWindow.owCtrl.Setup(null, null, list, true, new PguiOpenWindowCtrl.Callback(this.OnSelectLvUpWindowButtonCallback), null, false);
				this._charaGrowLevelUp.LevelUpGUI.lvUpWindow.iconChara.GetComponent<IconCharaCtrl>().Setup(DataManager.DmChara.GetUserCharaData(this._currentCharaId), SortFilterDefine.SortType.LEVEL, false, null, 0, -1, 0);
				this._charaGrowLevelUp.LevelUpGUI.lvUpWindow.owCtrl.Open();
				this._charaGrowLevelUp.LevelUpGUI.lvUpWindow.Txt_CharaName.text = charaData.staticData.GetName();
				this._charaGrowLevelUp.LevelUpGUI.lvUpWindow.Num_CoinOwn.text = DataManager.DmItem.GetUserItemData(30101).num.ToString();
				this._charaGrowLevelUp.LevelUpGUI.lvUpWindow.Num_CoinUse.text = this._lvUpCostCoin.ToString();
				return;
			}
			if (button == this._guiData.CharacterGrowMain.LvLimitOpen.ButtonC)
			{
				this._charaGrowLevelUp.LevelUpGUI.levelLimitOverWindow.owCtrl.Setup(null, null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, delegate(int index)
				{
					if (index == 1)
					{
						this.levelLimitOverEffect = this.LevelLimitOverEffect();
					}
					return true;
				}, null, false);
				this._charaGrowLevelUp.LevelUpGUI.levelLimitOverWindow.owCtrl.Open();
				this._charaGrowLevelUp.LevelUpGUI.levelLimitOverWindow.Setup(new SelCharaGrowLevel.WindowLevelLimitOver.SetupParam
				{
					charaData = charaData
				});
				return;
			}
			break;
		case 1:
		{
			SelCharaGrowWild.WildReleaseTab guiWild = this._charaGrowWild.GrowWildGUI.wildReleaseTab;
			CharaPromotePreset wildPreset = charaData.staticData.promoteList[this._charaGrowWild.GetPromoteNum(this._currentCharaId)];
			if (button == this._guiData.CharacterGrowMain.Cmn.ButtonL)
			{
				CharaPromoteOne charaPromoteOne = wildPreset.promoteOneList[this._charaGrowWild.currentIndexWild];
				this.OpenItemWindow(new List<ItemData>
				{
					new ItemData(charaPromoteOne.promoteUseItemId, charaPromoteOne.promoteUseItemNum)
				}, false, 0);
				return;
			}
			if (button == this._guiData.CharacterGrowMain.Cmn.ButtonR && !this._pressedButtonRWildRelease)
			{
				this._pressedButtonRWildRelease = true;
				this._connectFlowBase = this.ConnectFlowBase(delegate
				{
					this._wildReleaseEffect = this.WildReleaseEffect(false, true);
				}, delegate
				{
					SoundManager.Play("prd_se_friends_part_of_liberation", false, false);
					int currentIndexWild = this._charaGrowWild.currentIndexWild;
					CharaPromoteOne charaPromoteOne2 = wildPreset.promoteOneList[currentIndexWild];
					guiWild.iconItemList[currentIndexWild].iconItemCtrl.SetActEnable(charaData.dynamicData.promoteFlag[currentIndexWild]);
					ItemData userItemData2 = DataManager.DmItem.GetUserItemData(charaPromoteOne2.promoteUseItemId);
					bool flag2 = charaData.dynamicData.promoteNum >= charaData.staticData.maxPromoteNum;
					guiWild.iconItemList[currentIndexWild].markPlus.gameObject.SetActive(charaPromoteOne2.promoteUseItemNum <= userItemData2.num && !charaData.dynamicData.promoteFlag[currentIndexWild] && !flag2);
					guiWild.iconItemList[currentIndexWild].AEImage_OpenEff.PlayAnimation(PguiAECtrl.AmimeType.MAX, null);
					guiWild.iconItemList[currentIndexWild].current.SetActive(false);
					this.SetActStrengthButton(this._guiData.CharacterGrowMain.Cmn.TabGroup.SelectIndex, false);
				});
				return;
			}
			break;
		}
		case 2:
			if (button == this._guiData.CharacterGrowMain.Cmn.ButtonL)
			{
				if (charaData.GetNextItemByRankup(0) != null)
				{
					this.OpenItemWindow(new List<ItemData> { charaData.GetNextItemByRankup(0).item }, false, 0);
					return;
				}
			}
			else
			{
				if (button == this._guiData.CharacterGrowMain.Cmn.ButtonR)
				{
					List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>> list2 = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>
					{
						new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.NEGATIVE, this.CANCEL_TEXT),
						new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, "アップする")
					};
					this._charaGrowRank.GrowRankGUI.rankUpWindow.owCtrl.Setup(null, null, list2, true, new PguiOpenWindowCtrl.Callback(this.OnSelectRankupWindowButtonCallback), null, false);
					this._charaGrowRank.GrowRankGUI.rankUpWindow.owCtrl.Open();
					this._charaGrowRank.GrowRankGUI.rankUpWindow.iconChara.GetComponent<IconCharaCtrl>().Setup(DataManager.DmChara.GetUserCharaData(this._currentCharaId), SortFilterDefine.SortType.LEVEL, false, null, 0, -1, 0);
					this._charaGrowRank.GrowRankGUI.rankUpWindow.Txt_CharaName.text = charaData.staticData.GetName();
					GrowItemData nextItemByRankup = charaData.GetNextItemByRankup(0);
					if (nextItemByRankup != null)
					{
						SelCharaGrowRank.RankUpItem rankUpItem = new SelCharaGrowRank.RankUpItem(nextItemByRankup);
						this._charaGrowRank.GrowRankGUI.rankUpWindow.UseItem_Num_Before.text = rankUpItem.itemOwnNum.ToString();
						this._charaGrowRank.GrowRankGUI.rankUpWindow.UseItem_Num_After.text = Math.Abs(rankUpItem.itemNeedNum - rankUpItem.itemOwnNum).ToString();
						this._charaGrowRank.GrowRankGUI.rankUpWindow.UseCoin_Num_Before.text = DataManager.DmItem.GetUserItemData(30101).num.ToString();
						this._charaGrowRank.GrowRankGUI.rankUpWindow.UseCoin_Num_After.text = (DataManager.DmItem.GetUserItemData(30101).num - nextItemByRankup.needGold).ToString();
					}
					for (int i = 0; i < this._charaGrowRank.GrowRankGUI.rankUpWindow.StarAll.Count; i++)
					{
						PguiImageCtrl pguiImageCtrl = this._charaGrowRank.GrowRankGUI.rankUpWindow.StarAll[i];
						PguiReplaceSpriteCtrl component = pguiImageCtrl.GetComponent<PguiReplaceSpriteCtrl>();
						component.InitForce();
						component.Replace((i < charaData.dynamicData.rank) ? 1 : 2);
						pguiImageCtrl.gameObject.SetActive(i < charaData.staticData.baseData.rankHigh);
						PguiImageCtrl pguiImageCtrl2 = this._charaGrowRank.GrowRankGUI.rankUpWindow.StarAddAll[i];
						pguiImageCtrl2.GetComponent<uGUITweenColor>().Reset();
						pguiImageCtrl2.gameObject.SetActive(i == charaData.dynamicData.rank);
					}
					PrjUtil.ParamPreset paramPreset = PrjUtil.CalcParamByChara(DataManager.DmChara.GetUserCharaData(this._currentCharaId).dynamicData, charaData.dynamicData.level, charaData.dynamicData.rank);
					PrjUtil.ParamPreset paramPreset2 = PrjUtil.CalcParamByChara(DataManager.DmChara.GetUserCharaData(this._currentCharaId).dynamicData, charaData.dynamicData.level, charaData.dynamicData.rank + 1);
					List<int> list3 = new List<int> { paramPreset.totalParam, paramPreset.hp, paramPreset.atk, paramPreset.def };
					List<int> list4 = new List<int> { paramPreset2.totalParam, paramPreset2.hp, paramPreset2.atk, paramPreset2.def };
					this._charaGrowWild.SetWindowParam(list3, list4, this._charaGrowRank.GrowRankGUI.rankUpWindow.ParamAll);
					return;
				}
				if (button == this._guiData.CharacterGrowMain.PhotoMax.ItemConversion.ButtonC || button == this._guiData.CharacterGrowMain.PhotoPocketConversionStrengthen.ItemConversion.ButtonC)
				{
					this._guiData.ItemExchangeWindow.owCtrl.Setup(null, null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, new PguiOpenWindowCtrl.Callback(this.OnItemExchangeWindowButtonCallback), null, false);
					this._guiData.ItemExchangeWindow.owCtrl.Open();
					return;
				}
			}
			break;
		case 3:
		{
			GrowItemList nextItemByArtsUp = charaData.GetNextItemByArtsUp(0);
			if (button == this._guiData.CharacterGrowMain.Cmn.ButtonL)
			{
				if (charaData.GetNextItemByArtsUp(0) != null)
				{
					this.OpenItemWindow(nextItemByArtsUp.itemList, true, 0);
					return;
				}
			}
			else if (button == this._guiData.CharacterGrowMain.Cmn.ButtonR)
			{
				List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>> list5 = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>
				{
					new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.NEGATIVE, this.CANCEL_TEXT),
					new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, this.ENHANCE_TEXT)
				};
				this._charaGrowMiracle.GrowMiracleGUI.miracleWindow.owCtrl.Setup(null, null, list5, true, new PguiOpenWindowCtrl.Callback(this.OnSelectArtsWindowButtonCallback), null, false);
				GrowItemList nextItemByArtsUp2 = charaData.GetNextItemByArtsUp(0);
				for (int j = 0; j < this._charaGrowMiracle.GrowMiracleGUI.miracleWindow.iconItemList.Count; j++)
				{
					this._charaGrowMiracle.GrowMiracleGUI.miracleWindow.iconItemList[j].GetComponent<IconItemCtrl>().Setup((j < nextItemByArtsUp2.itemList.Count) ? DataManager.DmItem.GetItemStaticBase(nextItemByArtsUp2.itemList[j].id) : null, (j < nextItemByArtsUp2.itemList.Count) ? nextItemByArtsUp2.itemList[j].num : (-1));
				}
				this._charaGrowMiracle.GrowMiracleGUI.miracleWindow.owCtrl.Open();
				this._charaGrowMiracle.GrowMiracleGUI.miracleWindow.Txt_ArtsName.text = charaData.staticData.artsData.actionName;
				this._charaGrowMiracle.GrowMiracleGUI.miracleWindow.Num_Lv_Before.ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[]
				{
					SelCharaGrowCtrl.ARTS_TEXT_SIZE.ToString(),
					charaData.dynamicData.artsLevel.ToString()
				});
				this._charaGrowMiracle.GrowMiracleGUI.miracleWindow.Num_Lv_After.ReplaceTextByDefault(new string[] { "Param01", "Param02", "Param03" }, new string[]
				{
					SelCharaGrowCtrl.ARTS_TEXT_SIZE.ToString(),
					(charaData.dynamicData.artsLevel + 1).ToString(),
					SelCharaGrowCtrl.ARTS_HIGHLIGHT_COLOR_CODE
				});
				this._charaGrowMiracle.GrowMiracleGUI.miracleWindow.ItemUse_Num.text = nextItemByArtsUp.needGold.ToString();
				this._charaGrowMiracle.GrowMiracleGUI.miracleWindow.ItemOwn_Num.text = DataManager.DmItem.GetUserItemData(30101).num.ToString();
				this._charaGrowMiracle.GrowMiracleGUI.miracleWindow.ItemOwn_Num.m_Text.color = ((nextItemByArtsUp.needGold > DataManager.DmItem.GetUserItemData(30101).num) ? Color.red : Color.white);
				this._charaGrowMiracle.GrowMiracleGUI.miracleWindow.skillInfoBefore.Setup(new CharaUtil.GUISkillInfo.SetupParam
				{
					charaPackData = charaData,
					maxDisp = false,
					type = CharaUtil.GUISkillInfo.Type.KemonoMiracle
				});
				this._charaGrowMiracle.GrowMiracleGUI.miracleWindow.skillInfoAfter.Setup(new CharaUtil.GUISkillInfo.SetupParam
				{
					charaPackData = charaData,
					maxDisp = false,
					type = CharaUtil.GUISkillInfo.Type.KemonoMiracle,
					offsetKemonoMiracleLv = 1
				});
				return;
			}
			break;
		}
		case 4:
		{
			GrowItemList releaseItemByNanairoAbilityRelease = charaData.GetReleaseItemByNanairoAbilityRelease();
			if (button == this._guiData.CharacterGrowMain.Cmn.ButtonL)
			{
				if (releaseItemByNanairoAbilityRelease != null)
				{
					this.OpenItemWindow(releaseItemByNanairoAbilityRelease.itemList, true, 0);
					return;
				}
			}
			else if (button == this._guiData.CharacterGrowMain.Cmn.ButtonR)
			{
				List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>> list6 = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>
				{
					new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.NEGATIVE, this.CANCEL_TEXT),
					new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, this.TO_RELEASE_TEXT)
				};
				this._charaGrowNanairo.GrowNanairoGUI.nanairoWindow.owCtrl.Setup("なないろとくせい" + this.NANAIRO_RELEASE_TITLE_TEXT, null, list6, true, new PguiOpenWindowCtrl.Callback(this.OnSelectNanairoWindowButtonCallback), null, false);
				for (int k = 0; k < this._charaGrowNanairo.GrowNanairoGUI.nanairoWindow.iconItemList.Count; k++)
				{
					this._charaGrowNanairo.GrowNanairoGUI.nanairoWindow.iconItemList[k].GetComponent<IconItemCtrl>().Setup((k < releaseItemByNanairoAbilityRelease.itemList.Count) ? DataManager.DmItem.GetItemStaticBase(releaseItemByNanairoAbilityRelease.itemList[k].id) : null, (k < releaseItemByNanairoAbilityRelease.itemList.Count) ? releaseItemByNanairoAbilityRelease.itemList[k].num : (-1));
				}
				this._charaGrowNanairo.GrowNanairoGUI.nanairoWindow.Txt_AbilityName.text = (charaData.IsHaveNanairoAbility ? charaData.staticData.nanairoAbilityData.abilityName : "");
				this._charaGrowNanairo.GrowNanairoGUI.nanairoWindow.AbilityInfo.Setup(new CharaUtil.GUISkillInfo.SetupParam
				{
					charaPackData = charaData,
					maxDisp = false,
					type = CharaUtil.GUISkillInfo.Type.NanairoAbility
				});
				this._charaGrowNanairo.GrowNanairoGUI.nanairoWindow.owCtrl.Open();
				return;
			}
			break;
		}
		case 5:
			if (button == this._guiData.CharacterGrowMain.Cmn.ButtonL)
			{
				if (charaData.GetNextItemByReleasePhotoFrame() != null)
				{
					this.OpenItemWindow(new List<ItemData> { charaData.GetNextItemByReleasePhotoFrame().item }, false, 0);
					return;
				}
			}
			else if (button == this._guiData.CharacterGrowMain.Cmn.ButtonRExchange)
			{
				List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>> list7 = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>
				{
					new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.NEGATIVE, this.CANCEL_TEXT),
					new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, this.ENHANCE_TEXT)
				};
				this._charaGrowPhotoPocket.GrowPhotoPocketGUI.photoPocketWindow.owCtrl.Setup("フォトポケット強化確認", null, list7, true, new PguiOpenWindowCtrl.Callback(this.OnSelectPhotoPocketWindowButtonCallback), null, false);
				this._nextOpenPhotoIndex = charaData.dynamicData.PhotoFrameTotalStep % this._charaGrowPhotoPocket.GrowPhotoPocketGUI.photoPocketWindow.photoPocketIconBefores.Count;
				for (int l = 0; l < this._charaGrowPhotoPocket.GrowPhotoPocketGUI.photoPocketWindow.photoPocketIconBefores.Count; l++)
				{
					bool flag = charaData.dynamicData.PhotoPocket[l].Flag;
					SelCharaGrowCtrl.PhotoPocketIcon photoPocketIcon = this._charaGrowPhotoPocket.GrowPhotoPocketGUI.photoPocketWindow.photoPocketIconBefores[l];
					photoPocketIcon.IconPhoto.SetImageByName(photoPocketIcon.ReplaceIconPhoto.GetSpriteById(flag ? 1 : 0).name);
					photoPocketIcon.MarkKiseki.gameObject.SetActive(charaData.staticData.baseData.spAbilityRelPp == l + 1);
					photoPocketIcon.MarkKiseki.Replace(charaData.dynamicData.PhotoPocket[l].Flag ? 1 : 0);
					photoPocketIcon.NumLv.gameObject.SetActive(charaData.dynamicData.PhotoPocket[l].Step > 0);
					photoPocketIcon.NumLv.text = string.Format("{0}", charaData.dynamicData.PhotoPocket[l].Step);
					SelCharaGrowCtrl.PhotoPocketIcon photoPocketIcon2 = this._charaGrowPhotoPocket.GrowPhotoPocketGUI.photoPocketWindow.photoPocketIconAfters[l];
					flag = this._nextOpenPhotoIndex == l || flag;
					photoPocketIcon2.IconPhoto.SetImageByName(photoPocketIcon2.ReplaceIconPhoto.GetSpriteById(flag ? 1 : 0).name);
					uGUITweenColor component2 = photoPocketIcon2.IconPhoto.GetComponent<uGUITweenColor>();
					if (component2 != null)
					{
						component2.Reset();
						component2.enabled = this._nextOpenPhotoIndex == l;
					}
					photoPocketIcon2.MarkKiseki.gameObject.SetActive(charaData.staticData.baseData.spAbilityRelPp == l + 1);
					photoPocketIcon2.MarkKiseki.Replace(flag ? 1 : 0);
					photoPocketIcon2.NumLv.gameObject.SetActive((this._nextOpenPhotoIndex == l) ? (charaData.dynamicData.PhotoPocket[l].Step + 1 > 0) : (charaData.dynamicData.PhotoPocket[l].Step > 0));
					photoPocketIcon2.NumLv.text = ((this._nextOpenPhotoIndex == l) ? string.Format("{0}", charaData.dynamicData.PhotoPocket[l].Step + 1) : string.Format("{0}", charaData.dynamicData.PhotoPocket[l].Step));
				}
				GrowItemData nextItemByReleasePhotoFrame = charaData.GetNextItemByReleasePhotoFrame();
				if (nextItemByReleasePhotoFrame != null)
				{
					ItemData userItemData = DataManager.DmItem.GetUserItemData(nextItemByReleasePhotoFrame.item.id);
					this._charaGrowPhotoPocket.GrowPhotoPocketGUI.photoPocketWindow.owCtrl.Open();
					this._charaGrowPhotoPocket.GrowPhotoPocketGUI.photoPocketWindow.ItemBef_Num.text = userItemData.num.ToString();
					this._charaGrowPhotoPocket.GrowPhotoPocketGUI.photoPocketWindow.ItemAft_Num.text = (userItemData.num - nextItemByReleasePhotoFrame.item.num).ToString();
					this._charaGrowPhotoPocket.GrowPhotoPocketGUI.photoPocketWindow.Item_Txt.text = this.GEMSTONE_TEXT;
					this._charaGrowPhotoPocket.GrowPhotoPocketGUI.photoPocketWindow.Item_kiseki.SetActive(charaData.dynamicData.PhotoFrameTotalStep + 1 == charaData.staticData.baseData.spAbilityRelPp);
					return;
				}
			}
			else if (button == this._guiData.CharacterGrowMain.PhotoMax.ItemConversion.ButtonC || button == this._guiData.CharacterGrowMain.PhotoPocketConversionStrengthen.ItemConversion.ButtonC)
			{
				this._guiData.ItemExchangeWindow.owCtrl.Setup(null, null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, new PguiOpenWindowCtrl.Callback(this.OnItemExchangeWindowButtonCallback), null, false);
				this._guiData.ItemExchangeWindow.owCtrl.Open();
				if (!this._charaGrowPhotoPocket.ConversionCheckConfirmed && charaData.dynamicData.PhotoFrameTotalStep < 12)
				{
					CanvasManager.HdlExchangeWarningWindow.owCtrl.Setup(this.PHOTO_POCKET_GEMSTONE_EXCHANGE_WARNING_TITLE_TEXT, this.PHOTO_POCKET_GEMSTONE_EXCHANGE_WARNING_MESSAGE_TEXT, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, null, null, false);
					CanvasManager.HdlExchangeWarningWindow.CheckBoxButton.transform.Find("BaseImage/Img_Check").gameObject.SetActive(this._charaGrowPhotoPocket.ConversionCheckConfirmed);
					CanvasManager.HdlExchangeWarningWindow.CheckBoxButton.AddOnClickListener(delegate(PguiButtonCtrl x)
					{
						this.OnClickExchangeCheckButton(x);
					}, PguiButtonCtrl.SoundType.DEFAULT);
					CanvasManager.HdlExchangeWarningWindow.owCtrl.Open();
					return;
				}
			}
			break;
		case 6:
			if (button == this._guiData.CharacterGrowMain.Cmn.ButtonL)
			{
				this.ClearSelectLvUpItem(true);
				this.SetActStrengthButton(this._guiData.CharacterGrowMain.Cmn.TabGroup.SelectIndex, false);
				this.SelectKizunaLvUp();
				return;
			}
			if (button == this._guiData.CharacterGrowMain.Cmn.ButtonR)
			{
				List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>> list8 = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>
				{
					new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.NEGATIVE, this.CANCEL_TEXT),
					new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, this.ENHANCE_TEXT)
				};
				SelCharaGrowKizuna.WindowKizunaLvUp kizunaLvUpWindow = this._charaGrowKizuna.GrowKizunaGUI.KizunaLvUpWindow;
				kizunaLvUpWindow.OpenWindowCtrl.Setup(null, null, list8, true, new PguiOpenWindowCtrl.Callback(this.OnSelectKizunaLvUpWindowButtonCallback), null, false);
				kizunaLvUpWindow.IconChara.GetComponent<IconCharaCtrl>().Setup(DataManager.DmChara.GetUserCharaData(this._currentCharaId), SortFilterDefine.SortType.LEVEL, false, null, 0, -1, 0);
				kizunaLvUpWindow.OpenWindowCtrl.Open();
				kizunaLvUpWindow.TxtCharaName.text = charaData.staticData.GetName();
				kizunaLvUpWindow.NumCoinOwn.text = DataManager.DmItem.GetUserItemData(30101).num.ToString();
				kizunaLvUpWindow.NumCoinUse.text = this._lvUpCostCoin.ToString();
			}
			break;
		default:
			return;
		}
	}

	private void OnClickExchangeCheckButton(PguiButtonCtrl btn)
	{
		this._charaGrowPhotoPocket.ConversionCheckConfirmed = !this._charaGrowPhotoPocket.ConversionCheckConfirmed;
		CanvasManager.HdlExchangeWarningWindow.CheckBoxButton.transform.Find("BaseImage/Img_Check").gameObject.SetActive(this._charaGrowPhotoPocket.ConversionCheckConfirmed);
	}

	private void OnClickArrowButton(PguiButtonCtrl button)
	{
		int num = this._dispCharaPackList.FindIndex((CharaPackData item) => item.id == this._currentCharaId);
		num += ((button == this._guiData.CharacterGrowMain.BtnYajiLeft) ? (-1) : 1);
		num = (num + this._dispCharaPackList.Count) % this._dispCharaPackList.Count;
		this.CharaGrowSetup(this._dispCharaPackList[num].id, false);
	}

	private int CalcItemMaxValue(int itemId, bool isKizuna = false)
	{
		long num = (isKizuna ? DataManager.DmChara.GetNeedExpByKizunaLimitLevel(this._currentCharaId) : DataManager.DmChara.GetNeedExpByLimitLevel(this._currentCharaId));
		int num2 = 0;
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(this._currentCharaId);
		List<int> list = (isKizuna ? this._selectKizunaLvUpItemIdList : this._selectLvUpItemIdList);
		CharaDef.AttributeType attribute = userCharaData.staticData.baseData.attribute;
		foreach (int num3 in list)
		{
			if (itemId != num3)
			{
				ItemDef.AddCharaLevelExp addCharaLevelExpBase = ItemDef.GetAddCharaLevelExpBase(num3, attribute);
				num -= addCharaLevelExpBase.addExp;
				num2 += addCharaLevelExpBase.needGold;
			}
		}
		ItemDef.AddCharaLevelExp addCharaLevelExpBase2 = ItemDef.GetAddCharaLevelExpBase(itemId, attribute);
		int num4 = 0;
		while (num > 0L)
		{
			num2 += addCharaLevelExpBase2.needGold;
			if (num2 > DataManager.DmItem.GetUserItemData(30101).num || num4 >= DataManager.DmItem.GetUserItemData(itemId).num)
			{
				break;
			}
			num -= addCharaLevelExpBase2.addExp;
			num4++;
		}
		return num4;
	}

	private PguiReplaceAECtrl SetPguiReplaceAECtrl(DataManagerChara.CharaLevelupResult result, bool isKizuna = false)
	{
		PguiReplaceAECtrl pguiReplaceAECtrl = (isKizuna ? this._charaGrowKizuna.GrowKizunaGUI.KizunaLvUpTab.ImageResult.GetComponent<PguiReplaceAECtrl>() : this._charaGrowLevelUp.LevelUpGUI.lvUpTab.AEImage_result.GetComponent<PguiReplaceAECtrl>());
		switch (result.successStatus)
		{
		case DataManagerChara.CharaLevelupResult.Status.NORMAL:
			pguiReplaceAECtrl.Replace("RESULT_01");
			SoundManager.Play("prd_se_friends_levelup_auth", false, false);
			break;
		case DataManagerChara.CharaLevelupResult.Status.SPECIAL_S:
			pguiReplaceAECtrl.Replace("RESULT_02");
			SoundManager.Play("prd_se_friends_levelup_good_auth", false, false);
			break;
		case DataManagerChara.CharaLevelupResult.Status.SPECIAL_L:
			pguiReplaceAECtrl.Replace("RESULT_03");
			SoundManager.Play("prd_se_friends_levelup_great_auth", false, false);
			break;
		default:
			pguiReplaceAECtrl.Replace("RESULT_01");
			break;
		}
		return pguiReplaceAECtrl;
	}

	public void OnClickCharaDetailButton(PguiButtonCtrl btn)
	{
		int num = this._dispCharaPackList.FindIndex((CharaPackData item) => item.id == this._currentCharaId);
		CanvasManager.HdlCharaWindowCtrl.Open(this._dispCharaPackList[num], new CharaWindowCtrl.DetailParamSetting
		{
			UIPreset = CharaWindowCtrl.DetailParamSetting.Preset.MINE_EASY_NO_GROW,
			openPrevCB = delegate
			{
				if (this._renderTextureChara != null)
				{
					this._renderTextureChara.Setup(DataManager.DmChara.GetUserCharaData(this._currentCharaId), 0, CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true, null, false, null, 0f, null, false);
				}
			}
		}, delegate
		{
			if (this._renderTextureChara)
			{
				this._renderTextureChara.SetAnimation(CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true);
			}
		});
	}

	public bool _isFinishedAnimCharaTopButton { get; private set; }

	private void OnClickCharaTopButton(int charaId)
	{
		this._isFinishedAnimCharaTopButton = false;
		this._guiData.CharacterGrowTop.SelCmnAllInOut.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
		{
			this._guiData.CharacterGrowTop.BaseObj.SetActive(false);
			this._guiData.CharacterGrowMain.BaseObj.SetActive(true);
			this._guiData.CharacterGrowMain.CharaEditCharaGrowSE.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, delegate
			{
				this._isFinishedAnimCharaTopButton = true;
			});
		});
		this._currentMode = SelCharaGrowCtrl.Mode.Main;
		this.CharaGrowSetup(charaId, true);
	}

	private bool OnSelectLvUpWindowButtonCallback(int index)
	{
		if (index == 1)
		{
			this._expGageEffect = this.ExpGageEffect(false);
			this._lvUpCostCoin = 0;
		}
		return true;
	}

	private bool OnSelectKizunaLvUpWindowButtonCallback(int index)
	{
		if (index == 1)
		{
			this._expGageEffect = this.ExpGageEffect(true);
			this._lvUpCostCoin = 0;
		}
		return true;
	}

	private bool OnSelectOpenAllWindowButtonCallback(int index)
	{
		if (index == 1)
		{
			if (!this._charaGrowWild.GrowWildGUI.wildGrowWindow.ButtonRight.ActEnable)
			{
				return false;
			}
			CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(this._currentCharaId);
			CharaPromotePreset charaPromotePreset = userCharaData.staticData.promoteList[this._charaGrowWild.GetPromoteNum(this._currentCharaId)];
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			foreach (CharaPromoteOne charaPromoteOne in charaPromotePreset.promoteOneList)
			{
				ItemData userItemData = DataManager.DmItem.GetUserItemData(charaPromoteOne.promoteUseItemId);
				if (!dictionary.ContainsKey(charaPromoteOne.promoteUseItemId))
				{
					dictionary.Add(charaPromoteOne.promoteUseItemId, userItemData.num);
				}
			}
			bool flag = true;
			for (int i = 0; i < charaPromotePreset.promoteOneList.Count; i++)
			{
				CharaPromoteOne charaPromoteOne2 = charaPromotePreset.promoteOneList[i];
				if (!userCharaData.dynamicData.promoteFlag[i])
				{
					if (dictionary[charaPromoteOne2.promoteUseItemId] >= charaPromoteOne2.promoteUseItemNum)
					{
						Dictionary<int, int> dictionary2 = dictionary;
						int promoteUseItemId = charaPromoteOne2.promoteUseItemId;
						dictionary2[promoteUseItemId] -= charaPromoteOne2.promoteUseItemNum;
					}
					else
					{
						flag = false;
					}
				}
			}
			this._wildReleaseEffect = this.WildReleaseEffect(flag, false);
			this.TouchRect = true;
		}
		return true;
	}

	private bool OnSelectWildWindowButtonCallback(int index)
	{
		if (index == 1)
		{
			if (!this._charaGrowWild.GrowWildGUI.wildGrowWindow.ButtonRight.ActEnable)
			{
				return false;
			}
			this._wildReleaseEffect = this.WildReleaseEffect(true, false);
			this.TouchRect = true;
		}
		return true;
	}

	private bool OnResultWildWindowButtonCallback(int index)
	{
		this._effectStatus = SelCharaGrowCtrl.EffectStatus.ResultEnd;
		this.TouchRect = true;
		return true;
	}

	private bool OnSelectRankupWindowButtonCallback(int index)
	{
		if (index == 1)
		{
			this._rankUpGageEffect = this.RankUpGageEffect();
		}
		return true;
	}

	private bool OnSelectItemInfoWindowButtonCallback(int index)
	{
		return true;
	}

	private bool OnSelectArtsWindowButtonCallback(int index)
	{
		if (index == 1)
		{
			this._miracleEffect = this.MiracleEffect();
		}
		return true;
	}

	private bool OnSelectNanairoWindowButtonCallback(int index)
	{
		if (index == 1)
		{
			this._nanairoReleaseEffect = this.NanairoReleaseEffect();
		}
		return true;
	}

	private bool OnSelectPhotoPocketWindowButtonCallback(int index)
	{
		if (index == 1)
		{
			this._photoPocketEffect = this.PhotoPocketEffect();
		}
		return true;
	}

	private bool OnItemExchangeWindowButtonCallback(int index)
	{
		if (index == 1)
		{
			this._itemExchangeEffect = this.ItemExchangeEffect();
		}
		return true;
	}

	private bool OnSelectGrowMultiSelectWindowButtonCallback(int index)
	{
		if (index == 1)
		{
			this._charaGrowMulti.GrowMultiGUI.growMultiCheckWindow.owCtrl.Setup(null, null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES_EMPTY), true, new PguiOpenWindowCtrl.Callback(this.OnSelectGrowMultiCheckWindowButtonCallback), null, false);
			CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(this._currentCharaId);
			this._charaGrowMulti.GrowMultiGUI.growMultiCheckWindow.SetUpGrowMultiContent(userCharaData);
			this.UpdateGrowMultiContent(userCharaData);
			this._charaGrowMulti.GrowMultiGUI.growMultiCheckWindow.owCtrl.Open();
		}
		return true;
	}

	private void UpdateGrowMultiContent(CharaPackData charaPackData)
	{
		SelCharaGrowMulti.WindowGrowMultiCheck growMultiCheckWindow = this._charaGrowMulti.GrowMultiGUI.growMultiCheckWindow;
		List<ItemInput> useItemList = this._charaGrowMulti.GrowMultiGUI.growMultiCheckWindow.useItemList;
		foreach (object obj in Enum.GetValues(typeof(SelCharaGrowMulti.GrowCategory)))
		{
			SelCharaGrowMulti.GrowCategory growCategory = (SelCharaGrowMulti.GrowCategory)obj;
			if (this._charaGrowMulti.IsSelected(growCategory))
			{
				this._charaGrowMulti.GrowMultiGUI.growMultiCheckWindow.UpdateUseItemList();
				switch (growCategory)
				{
				case SelCharaGrowMulti.GrowCategory.Level:
					growMultiCheckWindow.SetUpLevelInfo(charaPackData, ref this._selectLvUpItemIdList, ref this._lvUpCostCoin);
					break;
				case SelCharaGrowMulti.GrowCategory.Wild:
					growMultiCheckWindow.SetUpWildInfo(charaPackData);
					break;
				case SelCharaGrowMulti.GrowCategory.Rank:
					growMultiCheckWindow.SetUpRankInfo(charaPackData, ref this._selectLvUpItemIdList, ref this._lvUpCostCoin);
					break;
				case SelCharaGrowMulti.GrowCategory.Arts:
					growMultiCheckWindow.SetUpArtsInfo(charaPackData);
					break;
				case SelCharaGrowMulti.GrowCategory.Nanairo:
					growMultiCheckWindow.SetUpNanairoInfo(charaPackData);
					break;
				case SelCharaGrowMulti.GrowCategory.LevelLimit:
					growMultiCheckWindow.SetUpLevelLimitInfo(charaPackData, ref this._selectLvUpItemIdList, ref this._lvUpCostCoin);
					break;
				case SelCharaGrowMulti.GrowCategory.KizunaLimit:
					growMultiCheckWindow.SetUpKizunaLevelLimitInfo(charaPackData);
					break;
				}
			}
		}
		growMultiCheckWindow.SetUseItemList(charaPackData);
		growMultiCheckWindow.itemUseInfoLayout.gameObject.SetActive(useItemList.Count > 0);
		bool flag = useItemList.Count > 0 || growMultiCheckWindow.afterPromoteNum > charaPackData.dynamicData.promoteNum;
		growMultiCheckWindow.paramObj.SetActive(growMultiCheckWindow.paramObj.activeSelf && flag);
		growMultiCheckWindow.owCtrl.choiceR.GetComponent<PguiButtonCtrl>().SetActEnable(flag, false, false);
		growMultiCheckWindow.owCtrl.choiceR.transform.Find("DisableText").gameObject.SetActive(!flag);
		if (growMultiCheckWindow.paramObj.activeSelf)
		{
			growMultiCheckWindow.SetParamInfo(charaPackData);
		}
		growMultiCheckWindow.SetNotExecMessage();
	}

	private bool OnSelectGrowMultiCheckWindowButtonCallback(int index)
	{
		if (index == 0 || index == PguiOpenWindowCtrl.CLOSE_BUTTON_INDEX)
		{
			this._charaGrowMulti.GrowMultiGUI.growMultiSelectWindow.owCtrl.Open();
			this._charaGrowMulti.GrowMultiGUI.growMultiCheckWindow.Reset();
			if (this._charaGrowMulti.IsSelected(SelCharaGrowMulti.GrowCategory.Level))
			{
				this.ClearSelectLvUpItem(false);
				if (this._guiData.CharacterGrowMain.Cmn.ButtonL.gameObject.activeInHierarchy && this._guiData.CharacterGrowMain.Cmn.ButtonR.gameObject.activeInHierarchy)
				{
					this.SetActStrengthButton(this._guiData.CharacterGrowMain.Cmn.TabGroup.SelectIndex, false);
				}
				this.SelectLvUp();
			}
		}
		else if (index == 1)
		{
			this._growMultiCoroutine = this.GrowMultiCoroutine();
		}
		return true;
	}

	private IEnumerator GrowMultiCoroutine()
	{
		CanvasManager.SetEnableCmnTouchMask(true);
		CharaPackData charaPackData = DataManager.DmChara.GetUserCharaData(this._currentCharaId);
		SelCharaGrowMulti.WindowGrowMultiCheck growMultiCheckWindow = this._charaGrowMulti.GrowMultiGUI.growMultiCheckWindow;
		DataManager.DmChara.CharaGrowMultiRequest = new CharaGrowMultiRequest();
		List<ItemInput> lvItemList = new List<ItemInput>();
		int beforePromoteNum = charaPackData.dynamicData.promoteNum;
		List<bool> beforePromoteFlag = charaPackData.dynamicData.promoteFlag;
		bool isPromoteStepUp = false;
		int beforeRank = charaPackData.dynamicData.rank;
		int beforeArtsLevel = charaPackData.dynamicData.artsLevel;
		foreach (object obj in Enum.GetValues(typeof(SelCharaGrowMulti.GrowCategory)))
		{
			SelCharaGrowMulti.GrowCategory growCategory = (SelCharaGrowMulti.GrowCategory)obj;
			if (growMultiCheckWindow.execMap[growCategory])
			{
				IEnumerator ienum = null;
				switch (growCategory)
				{
				case SelCharaGrowMulti.GrowCategory.Level:
					ienum = this.ExpGageEffectRequest(lvItemList, false);
					break;
				case SelCharaGrowMulti.GrowCategory.Wild:
				{
					List<WildResult> promoteRequestList = this._charaGrowMulti.GrowMultiGUI.growMultiCheckWindow.promoteRequestList;
					isPromoteStepUp = promoteRequestList.Count > 1 || ((promoteRequestList[0].promote_flag00 == 1 || charaPackData.dynamicData.promoteFlag[0]) && (promoteRequestList[0].promote_flag01 == 1 || charaPackData.dynamicData.promoteFlag[1]) && (promoteRequestList[0].promote_flag02 == 1 || charaPackData.dynamicData.promoteFlag[2]) && (promoteRequestList[0].promote_flag03 == 1 || charaPackData.dynamicData.promoteFlag[3]) && (promoteRequestList[0].promote_flag04 == 1 || charaPackData.dynamicData.promoteFlag[4]) && (promoteRequestList[0].promote_flag05 == 1 || charaPackData.dynamicData.promoteFlag[5]));
					ienum = this.WildReleaseEffectRequest(isPromoteStepUp, false, promoteRequestList);
					break;
				}
				case SelCharaGrowMulti.GrowCategory.Rank:
					ienum = this.RankUpGageEffectRequest(growMultiCheckWindow.afterRank);
					break;
				case SelCharaGrowMulti.GrowCategory.Arts:
					ienum = this.MiracleEffectRequest(growMultiCheckWindow.afterArtsLv);
					break;
				case SelCharaGrowMulti.GrowCategory.Nanairo:
					ienum = this.NanairoReleaseEffectRequest();
					break;
				case SelCharaGrowMulti.GrowCategory.LevelLimit:
					ienum = this.LevelLimitOverEffectRequest(growMultiCheckWindow.afterLevelLimitId);
					break;
				case SelCharaGrowMulti.GrowCategory.KizunaLimit:
					ienum = this.IEKizunaLimitOverRequest();
					break;
				}
				while (ienum.MoveNext())
				{
					yield return null;
				}
				ienum = null;
			}
		}
		IEnumerator enumerator = null;
		CanvasManager.SetEnableCmnTouchMask(true);
		DataManager.DmChara.RequestActionCharaGrowMulti();
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		this._voiceLength = 0f;
		this._voiceStartTime = 0f;
		foreach (object obj2 in Enum.GetValues(typeof(SelCharaGrowMulti.GrowCategory)))
		{
			SelCharaGrowMulti.GrowCategory key = (SelCharaGrowMulti.GrowCategory)obj2;
			if (growMultiCheckWindow.execMap[key])
			{
				if (this._voiceLength > 0f)
				{
					IEnumerator waitVoice = this.WaitVoice();
					while (waitVoice.MoveNext())
					{
						yield return null;
					}
					waitVoice = null;
				}
				this._voiceLength = 0f;
				this._voiceStartTime = 0f;
				IEnumerator ienum = null;
				switch (key)
				{
				case SelCharaGrowMulti.GrowCategory.Level:
					this._guiData.CharacterGrowMain.Cmn.TabGroup.SelectTab(0);
					if (this._guiData.CharacterGrowMain.Cmn.TabGroup.SelectIndex == 0)
					{
						this.OnSelectTab(0);
					}
					ienum = this.ExpGageEffectAfterRequest(lvItemList);
					this._lvUpCostCoin = 0;
					break;
				case SelCharaGrowMulti.GrowCategory.Wild:
				{
					this._guiData.CharacterGrowMain.Cmn.TabGroup.SelectTab(1);
					if (this._guiData.CharacterGrowMain.Cmn.TabGroup.SelectIndex == 1)
					{
						this.OnSelectTab(1);
					}
					PrjUtil.ParamPreset paramPreset = PrjUtil.CalcParamByChara(charaPackData.dynamicData, this._charaGrowMulti.GrowMultiGUI.growMultiCheckWindow.afterLevel, this._charaGrowMulti.GrowMultiGUI.growMultiCheckWindow.afterRank, beforePromoteNum, beforePromoteFlag);
					ienum = this.WildReleaseEffectAfterRequest(isPromoteStepUp, paramPreset, beforePromoteNum);
					break;
				}
				case SelCharaGrowMulti.GrowCategory.Rank:
					this._guiData.CharacterGrowMain.Cmn.TabGroup.SelectTab(2);
					if (this._guiData.CharacterGrowMain.Cmn.TabGroup.SelectIndex == 2)
					{
						this.OnSelectTab(2);
					}
					ienum = this.RankUpGageEffectAfterRequest(beforeRank);
					break;
				case SelCharaGrowMulti.GrowCategory.Arts:
					this._guiData.CharacterGrowMain.Cmn.TabGroup.SelectTab(3);
					if (this._guiData.CharacterGrowMain.Cmn.TabGroup.SelectIndex == 3)
					{
						this.OnSelectTab(3);
					}
					ienum = this.MiracleEffectAfterRequest(beforeArtsLevel);
					break;
				case SelCharaGrowMulti.GrowCategory.Nanairo:
					this._guiData.CharacterGrowMain.Cmn.TabGroup.SelectTab(4);
					if (this._guiData.CharacterGrowMain.Cmn.TabGroup.SelectIndex == 4)
					{
						this.OnSelectTab(4);
					}
					ienum = this.NanairoReleaseEffectAfterRequest();
					break;
				case SelCharaGrowMulti.GrowCategory.LevelLimit:
					this._guiData.CharacterGrowMain.Cmn.TabGroup.SelectTab(0);
					if (this._guiData.CharacterGrowMain.Cmn.TabGroup.SelectIndex == 0)
					{
						this.OnSelectTab(0);
					}
					this.SetMaxInfo(0, false, true);
					ienum = this.LevelLimitOverEffectAfterRequest();
					break;
				case SelCharaGrowMulti.GrowCategory.KizunaLimit:
					this._guiData.CharacterGrowMain.Cmn.TabGroup.SelectTab(6);
					if (this._guiData.CharacterGrowMain.Cmn.TabGroup.SelectIndex == 6)
					{
						this.OnSelectTab(6);
					}
					this.SetMaxInfo(6, false, true);
					ienum = this.IEKizunaLimitOverAfterRequest();
					break;
				}
				while (ienum.MoveNext())
				{
					yield return null;
				}
				if (this._charaGrowMulti.IsSelected(SelCharaGrowMulti.GrowCategory.Level))
				{
					this._lvUpCostCoin = 0;
				}
				ienum = null;
			}
		}
		enumerator = null;
		growMultiCheckWindow.Reset();
		yield break;
		yield break;
	}

	private IEnumerator WaitVoice()
	{
		CanvasManager.SetEnableCmnTouchMask(true);
		CanvasManager.AddCallbackCmnTouchMask(new UnityAction<Transform>(this.OnTouchMask));
		this._touchScreenAuth = false;
		while (Time.time - this._voiceStartTime < this._voiceLength && !this._touchScreenAuth)
		{
			yield return null;
		}
		CanvasManager.RemoveCallbackCmnTouchMask();
		yield break;
	}

	private bool OnResultRankupWindowButtonCallback(int index)
	{
		this._effectStatus = SelCharaGrowCtrl.EffectStatus.ResultEnd;
		return true;
	}

	private void OpenRankUpClothesWindow(int rank)
	{
		CharaClothStatic charaClothStatic = DataManager.DmChara.GetClothListByChara(this._currentCharaId).Find((CharaClothStatic x) => x.GetRank == rank);
		if (charaClothStatic != null)
		{
			this._effectStatus = SelCharaGrowCtrl.EffectStatus.Result;
			List<ItemData> list = new List<ItemData>
			{
				new ItemData(charaClothStatic.GetId(), 1)
			};
			CanvasManager.HdlGetItemWindowCtrl.Setup(list, new GetItemWindowCtrl.SetupParam
			{
				strItemCb = delegate(GetItemWindowCtrl.WordingCallbackParam param)
				{
					string text = "けも級";
					for (int i = 0; i < rank; i++)
					{
						text += "★";
					}
					return text + "に到達したため\n" + param.itemStaticBase.GetName() + "が解放されました";
				},
				windowFinishedCallback = new PguiOpenWindowCtrl.Callback(this.OnResultRankupClothesWindowButtonCallback)
			});
			CanvasManager.HdlGetItemWindowCtrl.Open();
			return;
		}
		this._effectStatus = SelCharaGrowCtrl.EffectStatus.ResultEnd;
	}

	private bool OnResultRankupClothesWindowButtonCallback(int index)
	{
		this._effectStatus = SelCharaGrowCtrl.EffectStatus.ResultEnd;
		return true;
	}

	private bool OnResultArtsWindowButtonCallback(int index)
	{
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(this._currentCharaId);
		int num = DataManager.DmQuest.GetPlayableMapIdList(QuestStaticChapter.Category.CHARA).Find((int item) => DataManager.DmQuest.QuestStaticData.mapDataMap[item].questCharaId == this._currentCharaId);
		QuestStaticMap questStaticMap = DataManager.DmQuest.QuestStaticData.mapDataMap[num];
		int chapterId = questStaticMap.chapterId;
		QuestStaticChapter questStaticChapter = DataManager.DmQuest.QuestStaticData.chapterDataMap[chapterId];
		List<QuestStaticQuestGroup> list = new List<QuestStaticQuestGroup>(questStaticMap.questGroupList);
		list.Sort((QuestStaticQuestGroup a, QuestStaticQuestGroup b) => a.dispPriority - b.dispPriority);
		QuestStaticQuestGroup questStaticQuestGroup = list[1];
		int num2 = 0;
		foreach (QuestStaticQuestOne questStaticQuestOne in questStaticQuestGroup.questOneList)
		{
			if (DataManager.DmQuest.QuestDynamicData.oneDataMap.ContainsKey(questStaticQuestOne.questId))
			{
				QuestDynamicQuestOne questDynamicQuestOne = DataManager.DmQuest.QuestDynamicData.oneDataMap[questStaticQuestOne.questId];
				if (questDynamicQuestOne.status == QuestOneStatus.CLEAR || questDynamicQuestOne.status == QuestOneStatus.COMPLETE)
				{
					num2++;
				}
			}
		}
		if (userCharaData.dynamicData.artsLevel == 5 && num2 >= questStaticQuestGroup.questOneList.Count)
		{
			CanvasManager.HdlOpenWindowBasic.Setup(this.FRENDS_STORY_EX_RELEASE_TITLE_TEXT, this.FRENDS_STORY_EX_RELEASE_MESSAGE_TEXT, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int idx)
			{
				this._effectStatus = SelCharaGrowCtrl.EffectStatus.ResultEnd;
				CanvasManager.HdlMissionProgressCtrl.IsMiracleUp = false;
				return true;
			}, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
		}
		else
		{
			this._effectStatus = SelCharaGrowCtrl.EffectStatus.ResultEnd;
			CanvasManager.HdlMissionProgressCtrl.IsMiracleUp = false;
		}
		return true;
	}

	private bool OnResultPhotoPocketWindowButtonCallback(int index)
	{
		this._charaGrowPhotoPocket.UpdateItemPhotoPocket(this._currentCharaId);
		this.SetActStrengthButton(this._guiData.CharacterGrowMain.Cmn.TabGroup.SelectIndex, false);
		return true;
	}

	public bool OnClickMenuReturn(UnityAction callback = null)
	{
		if (this._currentMode == SelCharaGrowCtrl.Mode.Main)
		{
			this._currentMode = SelCharaGrowCtrl.Mode.Top;
			this._guiData.CharacterGrowMain.CharaEditCharaGrowSE.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
			{
				this._guiData.CharacterGrowMain.BaseObj.SetActive(false);
				this._guiData.CharacterGrowTop.BaseObj.SetActive(true);
				this._guiData.CharacterGrowTop.SelCmnAllInOut.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
				CanvasManager.HdlOpenWindowSortFilter.SolutionList(SortFilterDefine.RegisterType.CHARA_GROW_TOP, null);
			});
			return true;
		}
		this._guiData.CharacterGrowTop.SelCmnAllInOut.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
		{
			UnityAction callback2 = callback;
			if (callback2 == null)
			{
				return;
			}
			callback2();
		});
		return false;
	}

	private void OnAchievementRewardStart(int index, GameObject go)
	{
		go.GetComponent<MissionBarCtrl>().Init();
	}

	private void OnAchievementRewardUpdate(int index, GameObject go)
	{
		if (-1 >= index)
		{
			return;
		}
		List<DataManagerCharaMission.DynamicCharaMission.MissionOne> list = DataManager.DmChMission.GetDynamicCharaMissionData(this._currentCharaId).MissionMap.Values.ToList<DataManagerCharaMission.DynamicCharaMission.MissionOne>().FindAll((DataManagerCharaMission.DynamicCharaMission.MissionOne match) => !match.Received);
		List<DataManagerCharaMission.DynamicCharaMission.MissionOne> list2 = new List<DataManagerCharaMission.DynamicCharaMission.MissionOne>();
		List<DataManagerCharaMission.DynamicCharaMission.MissionOne> list3 = new List<DataManagerCharaMission.DynamicCharaMission.MissionOne>();
		List<DataManagerCharaMission.DynamicCharaMission.MissionOne> list4 = new List<DataManagerCharaMission.DynamicCharaMission.MissionOne>();
		foreach (DataManagerCharaMission.DynamicCharaMission.MissionOne missionOne in list)
		{
			if (missionOne.CanReceive)
			{
				list3.Add(missionOne);
			}
			else
			{
				list4.Add(missionOne);
			}
		}
		list3.Sort((DataManagerCharaMission.DynamicCharaMission.MissionOne a, DataManagerCharaMission.DynamicCharaMission.MissionOne b) => a.SortNum - b.SortNum);
		list4.Sort((DataManagerCharaMission.DynamicCharaMission.MissionOne a, DataManagerCharaMission.DynamicCharaMission.MissionOne b) => a.SortNum - b.SortNum);
		list2.AddRange(list3);
		list2.AddRange(list4);
		List<DataManagerCharaMission.StaticMission> missionList = DataManager.DmChMission.GetStaticCharaMissionData(this._currentCharaId).MissionList;
		if (index < list2.Count)
		{
			go.SetActive(true);
			DataManagerCharaMission.DynamicCharaMission.MissionOne dynamicMissionOne = list2[index];
			DataManagerCharaMission.StaticMission staticMission = missionList.Find((DataManagerCharaMission.StaticMission item) => item.MissionId == dynamicMissionOne.MissionId);
			MissionBarCtrl component = go.GetComponent<MissionBarCtrl>();
			int numerator = dynamicMissionOne.Numerator;
			component.SetupCharaMission(new MissionBarCtrl.SetupParam
			{
				denominator = staticMission.Denominator,
				isClear = dynamicMissionOne.CanReceive,
				isSpecial = false,
				itemData = new ItemData(staticMission.RewardItemId, staticMission.RewardItemNum),
				missionContents = staticMission.MissionContents,
				numerator = dynamicMissionOne.Numerator,
				onClick = delegate
				{
					if (!this._GettingAchievement)
					{
						this._growthAchievementRewardEffect = this.GrowthAchievementRewardEffect(dynamicMissionOne, staticMission);
					}
				},
				received = dynamicMissionOne.Received
			});
			return;
		}
		go.SetActive(false);
	}

	private void OnStartWindowLvUpItem(int index, GameObject go)
	{
		int num = 5;
		this.SetStartWindowLvUpItem(go, num);
	}

	private void OnStartWindowKizunaLvUpItem(int index, GameObject go)
	{
		int num = 5;
		this.SetStartWindowLvUpItem(go, num);
	}

	private void OnUpdateWindowLvUpItem(int index, GameObject go)
	{
		this.SetUpdateWindowLvUpItem(index, go, false);
	}

	private void OnUpdateWindowKizunaLvUpItem(int index, GameObject go)
	{
		this.SetUpdateWindowLvUpItem(index, go, true);
	}

	private void SetStartWindowLvUpItem(GameObject go, int count)
	{
		for (int i = 0; i < count; i++)
		{
			GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneCharaEdit/GUI/Prefab/CharaGrow_ItemIconSet"), go.transform);
			gameObject.name = i.ToString();
			GameObject gameObject2 = Object.Instantiate<GameObject>((GameObject)Resources.Load("CmnIconFrame/GUI/Prefab/Icon_Item"), gameObject.transform);
			gameObject2.gameObject.name = "Icon";
			gameObject2.transform.SetSiblingIndex(1);
		}
	}

	private void SetUpdateWindowLvUpItem(int index, GameObject go, bool isKizuna = false)
	{
		int num = (isKizuna ? 5 : 5);
		for (int i = 0; i < num; i++)
		{
			CharaStaticBase baseData = DataManager.DmChara.GetUserCharaData(this._currentCharaId).staticData.baseData;
			Transform transform = go.transform.Find(i.ToString());
			int num2 = index * num + i;
			SelCharaGrowKizuna.WindowKizunaLvUp kizunaLvUpWindow = this._charaGrowKizuna.GrowKizunaGUI.KizunaLvUpWindow;
			SelCharaGrowLevel.WindowLvUp lvUpWindow = this._charaGrowLevelUp.LevelUpGUI.lvUpWindow;
			int num3 = (isKizuna ? kizunaLvUpWindow.ItemList.Count : lvUpWindow.itemList.Count);
			if (num2 < num3)
			{
				transform.gameObject.SetActive(true);
				int num4 = (isKizuna ? kizunaLvUpWindow.ItemList[num2].itemId : lvUpWindow.itemList[num2].itemId);
				transform.transform.Find("Icon_Item").gameObject.SetActive(false);
				transform.transform.Find("Icon").GetComponent<IconItemCtrl>().Setup(DataManager.DmItem.GetItemStaticBase(num4));
				transform.transform.Find("Num_Own").gameObject.SetActive(false);
				int addCharaLevelExpRatio = ItemDef.GetAddCharaLevelExpRatio(num4, baseData.attribute);
				transform.transform.Find("Txt_ExpBonus").gameObject.SetActive(addCharaLevelExpRatio > 100);
				transform.transform.Find("Count/Num_Count").gameObject.GetComponent<PguiTextCtrl>().text = (isKizuna ? kizunaLvUpWindow.ItemList[num2].num.ToString() : lvUpWindow.itemList[num2].num.ToString());
				transform.transform.Find("ColorBase").gameObject.SetActive(false);
			}
			else
			{
				transform.gameObject.SetActive(false);
			}
		}
	}

	private void ExecuteKizunaLimitOver(int index)
	{
		if (index == 1)
		{
			this._ieKizunaLimitOver = this.IEKizunaLimitOver();
		}
	}

	public void StartItemWild(int charaId)
	{
		SelCharaGrowWild.WildReleaseTab wildReleaseTab = this._charaGrowWild.GrowWildGUI.wildReleaseTab;
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(charaId);
		CharaPromotePreset charaPromotePreset = userCharaData.staticData.promoteList[this._charaGrowWild.GetPromoteNum(charaId)];
		for (int i = 0; i < charaPromotePreset.promoteOneList.Count; i++)
		{
			CharaPromoteOne charaPromoteOne = charaPromotePreset.promoteOneList[i];
			SelCharaGrowWild.WildItem wildItem = new SelCharaGrowWild.WildItem();
			GameObject gameObject = wildReleaseTab.baseObj.transform.Find("YaseiInfo").gameObject;
			GameObject gameObject2 = (GameObject)Resources.Load("CmnIconFrame/GUI/Prefab/Icon_Item");
			string text = "ItemOpen" + (i + 1).ToString("D2") + "/";
			GameObject gameObject3 = Object.Instantiate<GameObject>(gameObject2, gameObject.transform.Find(text + "Icon_Item"));
			wildItem.iconItemCtrl = gameObject3.GetComponent<IconItemCtrl>();
			wildItem.iconItemCtrl.Setup(DataManager.DmItem.GetItemStaticBase(charaPromoteOne.promoteUseItemId));
			int wildIconIdx = i;
			wildItem.iconItemCtrl.AddOnClickListener(delegate(IconItemCtrl x)
			{
				this.OnTouchWildIcon(wildIconIdx);
			});
			wildItem.current = gameObject.transform.Find(text + "Current").gameObject;
			wildItem.current.SetActive(i == this._charaGrowWild.currentIndexWild);
			wildItem.markPlus = gameObject.transform.Find(text + "Mark_Plus").gameObject.GetComponent<PguiImageCtrl>();
			wildItem.AEImage_OpenEff = gameObject.transform.Find(text + "AEImage_OpenEff").gameObject.GetComponent<PguiAECtrl>();
			ItemData userItemData = DataManager.DmItem.GetUserItemData(charaPromoteOne.promoteUseItemId);
			bool flag = userCharaData.dynamicData.promoteNum >= userCharaData.staticData.maxPromoteNum;
			wildItem.markPlus.gameObject.SetActive(charaPromoteOne.promoteUseItemNum <= userItemData.num && !userCharaData.dynamicData.promoteFlag[i] && !flag);
			DataManager.DmItem.GetUserItemMap();
			wildReleaseTab.iconItemList.Add(wildItem);
			RectTransform component = wildItem.iconItemCtrl.GetComponent<RectTransform>();
			wildReleaseTab.iconBaseList.Add(component);
		}
	}

	public void SetupNextTab(int tabIdx)
	{
		Vector2 vector = this._guiData.CharacterGrowMain.Cmn.TabRectTransformList[this._lastTabIndex].sizeDelta;
		vector.x = 102f;
		for (int i = 0; i < this._guiData.CharacterGrowMain.Cmn.TabRectTransformList.Count; i++)
		{
			this._guiData.CharacterGrowMain.Cmn.TabRectTransformList[i].sizeDelta = vector;
			this._guiData.CharacterGrowMain.Cmn.TabGuiList[i].IconItem.gameObject.SetActive(false);
			this._guiData.CharacterGrowMain.Cmn.TabGuiList[i].Text.SetActive(true);
		}
		vector = this._guiData.CharacterGrowMain.Cmn.TabRectTransformList[tabIdx].sizeDelta;
		vector.x = 70f;
		this._guiData.CharacterGrowMain.Cmn.TabRectTransformList[tabIdx].sizeDelta = vector;
		this._guiData.CharacterGrowMain.Cmn.TabGuiList[tabIdx].IconItem.gameObject.SetActive(true);
		this._guiData.CharacterGrowMain.Cmn.TabGuiList[tabIdx].Text.SetActive(false);
		this._lastTabIndex = tabIdx;
	}

	private void InitializeItemWindow()
	{
		this._guiData.SingleItemInfoWindow.ScrollView_QuestAll.InitForce();
		ReuseScroll scrollView_QuestAll = this._guiData.SingleItemInfoWindow.ScrollView_QuestAll;
		scrollView_QuestAll.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollView_QuestAll.onStartItem, new Action<int, GameObject>(this.OnStartWindowItemInfo));
		ReuseScroll scrollView_QuestAll2 = this._guiData.SingleItemInfoWindow.ScrollView_QuestAll;
		scrollView_QuestAll2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollView_QuestAll2.onUpdateItem, new Action<int, GameObject>(this.OnUpdateWindowItemInfo));
		this._guiData.SingleItemInfoWindow.ScrollView_QuestAll.Setup(10, 0);
		for (int i = 0; i < this._guiData.MultiItemInfoWindow.IconItemCtrlList.Count; i++)
		{
			IconItemCtrl iconItemCtrl = this._guiData.MultiItemInfoWindow.IconItemCtrlList[i].iconItemCtrl;
			int tmpIndex = i;
			iconItemCtrl.AddOnClickListener(delegate(IconItemCtrl item)
			{
				this.OnClickWindowItemIcon(tmpIndex);
			});
		}
		this._guiData.SingleItemInfoWindow.exchangeButton.AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			this._itemExchange = this.ItemExchange(false);
		}, PguiButtonCtrl.SoundType.DEFAULT);
		for (int j = 0; j < this._guiData.MultiItemInfoWindow.ScrollViewAllList.Count; j++)
		{
			ReuseScroll reuseScroll = this._guiData.MultiItemInfoWindow.ScrollViewAllList[j];
			reuseScroll.name = j.ToString();
			reuseScroll.InitForce();
			reuseScroll.onStartItem = (Action<int, GameObject>)Delegate.Combine(reuseScroll.onStartItem, new Action<int, GameObject>(this.OnStartWindowItemInfoAll));
			reuseScroll.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(reuseScroll.onUpdateItem, new Action<int, GameObject>(this.OnUpdateWindowItemInfoAll));
			reuseScroll.Setup(10, 0);
		}
		this._guiData.MultiItemInfoWindow.exchangeButton.AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			this._itemExchange = this.ItemExchange(true);
		}, PguiButtonCtrl.SoundType.DEFAULT);
	}

	private void OpenItemWindow(List<ItemData> itemList, bool isMulti, int index)
	{
		SelCharaGrowCtrl.<>c__DisplayClass260_0 CS$<>8__locals1 = new SelCharaGrowCtrl.<>c__DisplayClass260_0();
		CS$<>8__locals1.itemList = itemList;
		this.lastOpenItemWindowParam.itemList = CS$<>8__locals1.itemList;
		this.lastOpenItemWindowParam.isMulti = isMulti;
		this.lastOpenItemWindowParam.index = index;
		Dictionary<int, QuestStaticQuestOne> oneDataMap = DataManager.DmQuest.QuestStaticData.oneDataMap;
		this.itemWindowEnableOneList = new List<List<SelCharaGrowCtrl.ListBarKind>>();
		DataManagerGameStatus.UserFlagData userFlagData = DataManager.DmGameStatus.MakeUserFlagData();
		int i;
		Predicate<ShopData.ItemOne> <>9__0;
		int l;
		for (i = 0; i < CS$<>8__locals1.itemList.Count; i = l + 1)
		{
			List<SelCharaGrowCtrl.ListBarKind> list = new List<SelCharaGrowCtrl.ListBarKind>();
			List<SelCharaGrowCtrl.ListBarKind> list2 = new List<SelCharaGrowCtrl.ListBarKind>();
			foreach (ShopData shopData in DataManager.DmShop.GetShopDataList(true, true, ShopData.TabCategory.ALL))
			{
				List<ShopData.ItemOne> oneDataList = shopData.oneDataList;
				Predicate<ShopData.ItemOne> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = (ShopData.ItemOne x) => x.itemId == CS$<>8__locals1.itemList[i].id);
				}
				ShopData.ItemOne itemOne = oneDataList.Find(predicate);
				if (itemOne != null)
				{
					if (itemOne == null || itemOne.maxChangeNum <= 0 || itemOne.nowChangeNum < itemOne.maxChangeNum)
					{
						list.Add(new SelCharaGrowCtrl.ListBarKind(SelCharaGrowCtrl.ListKind.Shop, shopData.shopId));
					}
					else
					{
						list2.Add(new SelCharaGrowCtrl.ListBarKind(SelCharaGrowCtrl.ListKind.Shop, shopData.shopId));
					}
				}
			}
			if (DataManager.DmQuest.QuestStaticData.dropItemQuestMap.ContainsKey(CS$<>8__locals1.itemList[i].id))
			{
				foreach (int num in DataManager.DmQuest.QuestStaticData.dropItemQuestMap[CS$<>8__locals1.itemList[i].id])
				{
					if (oneDataMap.ContainsKey(num))
					{
						QuestOnePackData questOnePack = DataManager.DmQuest.GetQuestOnePackData(num);
						if ((questOnePack.questChapter.category != QuestStaticChapter.Category.GROW || userFlagData.ReleaseModeFlag.GrowthQuest != DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Locked) && (questOnePack.questChapter.category != QuestStaticChapter.Category.SIDE_STORY || userFlagData.ReleaseModeFlag.AraiDiary != DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Locked) && (!SceneQuest.IsMainStoryPart1_5(questOnePack.questChapter.category) || userFlagData.ReleaseModeFlag.CellvalQuest != DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Locked) && (!SceneQuest.IsMainStoryPart2(questOnePack.questChapter.category) || userFlagData.ReleaseModeFlag.MainStory2 != DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Locked) && (!SceneQuest.IsMainStoryPart3(questOnePack.questChapter.category) || userFlagData.ReleaseModeFlag.MainStory3 != DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Locked))
						{
							if (questOnePack.questChapter.category == QuestStaticChapter.Category.ETCETERA)
							{
								if (userFlagData.ReleaseModeFlag.EtceteraQuest == DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Locked)
								{
									continue;
								}
								DateTime now = TimeManager.Now;
								if (questOnePack.questMap.StartDateTime > now || questOnePack.questGroup.startDatetime > now || questOnePack.questGroup.endDatetime < now)
								{
									continue;
								}
							}
							if (questOnePack.questChapter.category != QuestStaticChapter.Category.CHARA || (userFlagData.ReleaseModeFlag.FriendsStory != DataManagerGameStatus.UserFlagData.ReleaseMode.LockStatus.Locked && DataManager.DmChara.GetUserCharaData(questOnePack.questMap.questCharaId) != null))
							{
								if (questOnePack.questChapter.category == QuestStaticChapter.Category.EVENT)
								{
									DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventDataList().Find((DataManagerEvent.EventData x) => x.eventChapterId == questOnePack.questChapter.chapterId);
									if (eventData == null || (eventData.startDatetime <= TimeManager.Now && TimeManager.Now <= eventData.endDatetime))
									{
										continue;
									}
								}
								if (DataManager.DmQuest.QuestDynamicData.oneDataMap.ContainsKey(questOnePack.questOne.questId))
								{
									list.Add(new SelCharaGrowCtrl.ListBarKind(SelCharaGrowCtrl.ListKind.Quest, num));
								}
								else if (!(questOnePack.questGroup.startDatetime > TimeManager.Now) && !(TimeManager.Now > questOnePack.questGroup.endDatetime))
								{
									list2.Add(new SelCharaGrowCtrl.ListBarKind(SelCharaGrowCtrl.ListKind.Quest, num));
								}
							}
						}
					}
				}
				list.Sort((SelCharaGrowCtrl.ListBarKind x, SelCharaGrowCtrl.ListBarKind y) => x.id - y.id);
				list2.Sort((SelCharaGrowCtrl.ListBarKind x, SelCharaGrowCtrl.ListBarKind y) => x.id - y.id);
			}
			list.AddRange(list2);
			this.itemWindowEnableOneList.Add(list);
			l = i;
		}
		if (isMulti)
		{
			for (int j = 0; j < this._guiData.MultiItemInfoWindow.IconItemCtrlList.Count; j++)
			{
				if (j < CS$<>8__locals1.itemList.Count)
				{
					this._guiData.MultiItemInfoWindow.IconItemCtrlList[j].iconItemCtrl.Setup(CS$<>8__locals1.itemList[j].staticData);
					this._guiData.MultiItemInfoWindow.IconItemCtrlList[j].Txt_Num.text = string.Format("{0}/{1}", DataManager.DmItem.GetUserItemData(CS$<>8__locals1.itemList[j].id).num, CS$<>8__locals1.itemList[j].num);
				}
				else
				{
					this._guiData.MultiItemInfoWindow.IconItemCtrlList[j].Clear();
				}
			}
			for (int k = 0; k < this._guiData.MultiItemInfoWindow.ScrollViewAllList.Count; k++)
			{
				if (k < this.itemWindowEnableOneList.Count && this.itemWindowEnableOneList[k].Count > 0)
				{
					this._guiData.MultiItemInfoWindow.ScrollViewAllList[k].Resize(this.itemWindowEnableOneList[k].Count, 0);
				}
				else
				{
					this._guiData.MultiItemInfoWindow.ScrollViewAllList[k].Resize(0, 0);
				}
				this._guiData.MultiItemInfoWindow.ScrollViewList[k].gameObject.SetActive(k == index);
			}
			this.OnClickWindowItemIcon(index);
			this._guiData.MultiItemInfoWindow.owCtrl.Setup(null, null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, new PguiOpenWindowCtrl.Callback(this.OnSelectItemInfoWindowButtonCallback), null, false);
			this._guiData.MultiItemInfoWindow.owCtrl.gameObject.SetActive(true);
			this._guiData.MultiItemInfoWindow.owCtrl.Open();
		}
		else if (CS$<>8__locals1.itemList.Count > 0)
		{
			this._guiData.SingleItemInfoWindow.owCtrl.Setup(PrjUtil.MakeMessage(DataManager.DmItem.GetUserItemData(CS$<>8__locals1.itemList[0].id).staticData.GetName()) + PrjUtil.MakeMessage("入手場所"), null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, new PguiOpenWindowCtrl.Callback(this.OnSelectItemInfoWindowButtonCallback), null, false);
			this._guiData.SingleItemInfoWindow.iconItemPack.iconItemCtrl.Setup(CS$<>8__locals1.itemList[0].staticData);
			this._guiData.SingleItemInfoWindow.iconItemPack.Txt_Num.text = string.Format("{0}/{1}", DataManager.DmItem.GetUserItemData(CS$<>8__locals1.itemList[0].id).num, CS$<>8__locals1.itemList[0].num);
			List<DataManagerItem.ExchangeRatesData> exchageRatesList = DataManager.DmItem.GetExchageRatesList();
			DataManagerItem.ExchangeRatesData exchangeRate = exchageRatesList.Find((DataManagerItem.ExchangeRatesData item) => item.targetItemId == CS$<>8__locals1.itemList[0].staticData.GetId());
			if (exchangeRate != null)
			{
				ItemStaticBase itemStaticBase = DataManager.DmItem.GetItemStaticBase(exchangeRate.targetItemId);
				ItemStaticBase itemStaticBase2 = DataManager.DmItem.GetItemStaticBase(exchangeRate.sourceItemId);
				this._guiData.SingleItemInfoWindow.targetIconItemPack.iconItemCtrl.Setup(itemStaticBase);
				this._guiData.SingleItemInfoWindow.sourceIconItemPack.iconItemCtrl.Setup(itemStaticBase2);
				ItemData userItemData = DataManager.DmItem.GetUserItemData(exchangeRate.sourceItemId);
				string text = ((userItemData.num < exchangeRate.useNum) ? ("<color=red>" + userItemData.num.ToString() + "</color>") : userItemData.num.ToString());
				List<ExchangeExecuteCountInfo> executeCountInfos = DataManager.DmItem.GetExecuteCountInfos();
				if (executeCountInfos != null)
				{
					ExchangeExecuteCountInfo exchangeExecuteCountInfo = executeCountInfos.Find((ExchangeExecuteCountInfo info) => info.targetItemId == exchangeRate.targetItemId);
					int num2 = ((exchangeExecuteCountInfo == null) ? exchangeRate.monthlyExchangeLimit : (exchangeRate.monthlyExchangeLimit - exchangeExecuteCountInfo.executeCount));
					bool flag = (exchangeExecuteCountInfo == null || exchangeRate.monthlyExchangeLimit > exchangeExecuteCountInfo.executeCount) && userItemData.num >= exchangeRate.useNum;
					this._guiData.SingleItemInfoWindow.exchangeButton.SetActEnable(flag, false, false);
					this._guiData.SingleItemInfoWindow.remainExecuteText.ReplaceTextByDefault("Param01", num2.ToString());
					this._guiData.SingleItemInfoWindow.remainExecuteText.gameObject.SetActive(true);
				}
				this._guiData.SingleItemInfoWindow.sourceIconItemPack.Txt_Num.ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[]
				{
					text,
					exchangeRate.useNum.ToString()
				});
				this._guiData.SingleItemInfoWindow.sourceIconItemPack.Txt_Num.m_Text.supportRichText = true;
				this._guiData.SingleItemInfoWindow.targetIconItemPack.Txt_Num.text = exchangeRate.gainNum.ToString();
			}
			this._guiData.SingleItemInfoWindow.exchangeBase.SetActive(exchangeRate != null);
			float num3 = (this._guiData.SingleItemInfoWindow.exchangeBase.activeSelf ? 400f : 560f);
			this._guiData.SingleItemInfoWindow.questInfo.sizeDelta = new Vector2(this._guiData.SingleItemInfoWindow.questInfo.sizeDelta.x, num3);
			if (this.itemWindowEnableOneList[0].Count > 0)
			{
				this._guiData.SingleItemInfoWindow.ScrollView_QuestAll.gameObject.SetActive(true);
				this._guiData.SingleItemInfoWindow.ScrollView_QuestAll.Resize(this.itemWindowEnableOneList[0].Count, 0);
				this._guiData.SingleItemInfoWindow.Txt_NoneInfo.gameObject.SetActive(false);
			}
			else
			{
				this._guiData.SingleItemInfoWindow.ScrollView_QuestAll.gameObject.SetActive(false);
				this._guiData.SingleItemInfoWindow.Txt_NoneInfo.gameObject.SetActive(true);
			}
			this._guiData.SingleItemInfoWindow.owCtrl.gameObject.SetActive(true);
			this._guiData.SingleItemInfoWindow.owCtrl.Open();
		}
		Singleton<SceneManager>.Instance.StartCoroutine(this.RequestExecuteCountInfo(isMulti));
	}

	private void OnClickWindowItemIcon(int index)
	{
		int i;
		Predicate<DataManagerItem.ExchangeRatesData> <>9__0;
		int j;
		for (i = 0; i < this._guiData.MultiItemInfoWindow.ScrollViewAllList.Count; i = j + 1)
		{
			this._guiData.MultiItemInfoWindow.ScrollViewList[i].gameObject.SetActive(i == index);
			if (i == index)
			{
				SelCharaGrowCtrl.<>c__DisplayClass261_1 CS$<>8__locals2 = new SelCharaGrowCtrl.<>c__DisplayClass261_1();
				this._guiData.MultiItemInfoWindow.Current_Frame.transform.SetParent(this._guiData.MultiItemInfoWindow.IconItemCtrlList[i].iconItemCtrl.transform, false);
				this._guiData.MultiItemInfoWindow.Current_Txt_ItemName.text = this._guiData.MultiItemInfoWindow.IconItemCtrlList[i].iconItemCtrl.itemStaticBase.GetName();
				this._guiData.MultiItemInfoWindow.Txt_NoneInfo.gameObject.SetActive(i < this.itemWindowEnableOneList.Count && this.itemWindowEnableOneList[i].Count == 0);
				this.lastOpenItemWindowParam.index = index;
				List<DataManagerItem.ExchangeRatesData> exchageRatesList = DataManager.DmItem.GetExchageRatesList();
				SelCharaGrowCtrl.<>c__DisplayClass261_1 CS$<>8__locals3 = CS$<>8__locals2;
				List<DataManagerItem.ExchangeRatesData> list = exchageRatesList;
				Predicate<DataManagerItem.ExchangeRatesData> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = (DataManagerItem.ExchangeRatesData item) => item.targetItemId == this._guiData.MultiItemInfoWindow.IconItemCtrlList[i].iconItemCtrl.itemStaticBase.GetId());
				}
				CS$<>8__locals3.exchangeRate = list.Find(predicate);
				if (CS$<>8__locals2.exchangeRate != null)
				{
					ItemStaticBase itemStaticBase = DataManager.DmItem.GetItemStaticBase(CS$<>8__locals2.exchangeRate.targetItemId);
					ItemStaticBase itemStaticBase2 = DataManager.DmItem.GetItemStaticBase(CS$<>8__locals2.exchangeRate.sourceItemId);
					this._guiData.MultiItemInfoWindow.targetIconItemPack.iconItemCtrl.Setup(itemStaticBase);
					this._guiData.MultiItemInfoWindow.sourceIconItemPack.iconItemCtrl.Setup(itemStaticBase2);
					ItemData userItemData = DataManager.DmItem.GetUserItemData(CS$<>8__locals2.exchangeRate.sourceItemId);
					string text = ((userItemData.num < CS$<>8__locals2.exchangeRate.useNum) ? ("<color=red>" + userItemData.num.ToString() + "</color>") : userItemData.num.ToString());
					List<ExchangeExecuteCountInfo> executeCountInfos = DataManager.DmItem.GetExecuteCountInfos();
					if (executeCountInfos != null)
					{
						ExchangeExecuteCountInfo exchangeExecuteCountInfo = executeCountInfos.Find((ExchangeExecuteCountInfo info) => info.targetItemId == CS$<>8__locals2.exchangeRate.targetItemId);
						int num = ((exchangeExecuteCountInfo == null) ? CS$<>8__locals2.exchangeRate.monthlyExchangeLimit : (CS$<>8__locals2.exchangeRate.monthlyExchangeLimit - exchangeExecuteCountInfo.executeCount));
						bool flag = (exchangeExecuteCountInfo == null || CS$<>8__locals2.exchangeRate.monthlyExchangeLimit > exchangeExecuteCountInfo.executeCount) && userItemData.num >= CS$<>8__locals2.exchangeRate.useNum;
						this._guiData.MultiItemInfoWindow.exchangeButton.SetActEnable(flag, false, false);
						this._guiData.MultiItemInfoWindow.remainExecuteText.ReplaceTextByDefault("Param01", num.ToString());
						this._guiData.MultiItemInfoWindow.remainExecuteText.gameObject.SetActive(true);
					}
					this._guiData.MultiItemInfoWindow.sourceIconItemPack.Txt_Num.ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[]
					{
						text,
						CS$<>8__locals2.exchangeRate.useNum.ToString()
					});
					this._guiData.MultiItemInfoWindow.sourceIconItemPack.Txt_Num.m_Text.supportRichText = true;
					this._guiData.MultiItemInfoWindow.targetIconItemPack.Txt_Num.text = CS$<>8__locals2.exchangeRate.gainNum.ToString();
				}
				this._guiData.MultiItemInfoWindow.exchangeBase.SetActive(CS$<>8__locals2.exchangeRate != null);
				float num2 = (this._guiData.MultiItemInfoWindow.exchangeBase.activeSelf ? 350f : 510f);
				this._guiData.MultiItemInfoWindow.questInfo.sizeDelta = new Vector2(this._guiData.MultiItemInfoWindow.questInfo.sizeDelta.x, num2);
			}
			j = i;
		}
		SoundManager.Play("prd_se_click", false, false);
	}

	private void OnClickWindowItemButton(PguiButtonCtrl button)
	{
		string str = button.gameObject.GetComponent<PguiDataHolder>().str;
		if (!str.Contains(SelCharaGrowCtrl.ListKind.Quest.ToString()))
		{
			if (str.Contains(SelCharaGrowCtrl.ListKind.Shop.ToString()))
			{
				int id = button.gameObject.GetComponent<PguiDataHolder>().id;
				ShopData shopData = DataManager.DmShop.GetShopData(id);
				SceneShopArgs sceneShopArgs = new SceneShopArgs();
				sceneShopArgs.resultNextSceneName = SceneManager.SceneName.SceneCharaEdit;
				sceneShopArgs.resultNextSceneArgs = new SceneCharaEdit.Args
				{
					growCharaId = this._currentCharaId,
					growTab = this._guiData.CharacterGrowMain.Cmn.TabGroup.SelectIndex,
					openItemWindow = true
				};
				sceneShopArgs.shopId = ((shopData != null) ? shopData.shopId : 0);
				sceneShopArgs.shopItem = this.lastOpenItemWindowParam.itemList[this.lastOpenItemWindowParam.index].id;
				Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneShop, sceneShopArgs);
			}
			return;
		}
		int id2 = button.gameObject.GetComponent<PguiDataHolder>().id;
		QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(id2);
		int num = DataManager.DmQuest.CalcRestPlayNumByQuestOneId(id2);
		if (!button.ActEnable)
		{
			bool flag = questOnePackData.questDynamicOne.status == QuestOneStatus.COMPLETE || questOnePackData.questDynamicOne.status == QuestOneStatus.CLEAR;
			DataManager.DmQuest.QuestDynamicData.oneDataMap.ContainsKey(questOnePackData.questOne.questId);
			if (num >= 0 && questOnePackData.questOne.RecoveryKeyItem != null)
			{
				bool flag2 = questOnePackData.questDynamicOne.todayRecoveryNum < questOnePackData.questOne.RecoveryMaxNum;
			}
			string text;
			if (flag)
			{
				text = QuestUtil.WindowWord01;
			}
			else
			{
				text = "利用できません";
			}
			CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("確認"), text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int index) => true, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			return;
		}
		if (num == 0 && questOnePackData.questDynamicOne.todayRecoveryNum < questOnePackData.questOne.RecoveryMaxNum)
		{
			CanvasManager.HdlSelQuestCountRecoveryWindowCtrl.Setup(id2, delegate
			{
				if (this._guiData.SingleItemInfoWindow.owCtrl.gameObject.activeSelf)
				{
					this._guiData.SingleItemInfoWindow.ScrollView_QuestAll.Refresh();
					return;
				}
				if (this._guiData.MultiItemInfoWindow.owCtrl.gameObject.activeSelf)
				{
					foreach (ReuseScroll reuseScroll in this._guiData.MultiItemInfoWindow.ScrollViewAllList)
					{
						reuseScroll.Refresh();
					}
				}
			});
			return;
		}
		SceneCharaEdit.Args args = new SceneCharaEdit.Args
		{
			growCharaId = this._currentCharaId,
			growTab = this._guiData.CharacterGrowMain.Cmn.TabGroup.SelectIndex,
			openItemWindow = true
		};
		Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneBattleSelector, new SceneBattleSelector.Args
		{
			selectQuestOneId = id2,
			menuBackSceneName = SceneManager.SceneName.SceneCharaEdit,
			menuBackSceneArgs = args
		});
	}

	private void OnStartWindowItemInfo(int index, GameObject go)
	{
		go.AddComponent<PguiDataHolder>();
		new SelCharaGrowCtrl.GuiCharaGrowListBar(go.transform).AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickWindowItemButton));
	}

	private void OnUpdateWindowItemInfo(int index, GameObject go)
	{
		int num = 0;
		if (num < this.itemWindowEnableOneList.Count && index < this.itemWindowEnableOneList[num].Count)
		{
			go.SetActive(true);
			SelCharaGrowCtrl.GuiCharaGrowListBar guiCharaGrowListBar = new SelCharaGrowCtrl.GuiCharaGrowListBar(go.transform);
			SelCharaGrowCtrl.ListBarKind listBarKind = this.itemWindowEnableOneList[num][index];
			int num2 = listBarKind.id;
			SelCharaGrowCtrl.ListKind kind = listBarKind.kind;
			if (kind != SelCharaGrowCtrl.ListKind.Quest)
			{
				if (kind == SelCharaGrowCtrl.ListKind.Shop)
				{
					int id = this.itemWindowEnableOneList[num][index].id;
					num2 = id;
					ShopData shopData = DataManager.DmShop.GetShopData(id);
					guiCharaGrowListBar.Setup(shopData, this._guiData.SingleItemInfoWindow.iconItemPack.iconItemCtrl.itemStaticBase.GetId());
				}
			}
			else
			{
				int id2 = this.itemWindowEnableOneList[num][index].id;
				num2 = id2;
				QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(id2);
				guiCharaGrowListBar.Setup(questOnePackData);
			}
			guiCharaGrowListBar.AddParam(num2, kind.ToString());
			return;
		}
		go.SetActive(false);
	}

	private void OnStartWindowItemInfoAll(int index, GameObject go)
	{
		go.AddComponent<PguiDataHolder>();
		new SelCharaGrowCtrl.GuiCharaGrowListBar(go.transform).AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickWindowItemButton));
	}

	private void OnUpdateWindowItemInfoAll(int index, GameObject go)
	{
		int num = int.Parse(go.transform.parent.parent.parent.name);
		if (num < this.itemWindowEnableOneList.Count && index < this.itemWindowEnableOneList[num].Count)
		{
			go.SetActive(true);
			SelCharaGrowCtrl.GuiCharaGrowListBar guiCharaGrowListBar = new SelCharaGrowCtrl.GuiCharaGrowListBar(go.transform);
			SelCharaGrowCtrl.ListBarKind listBarKind = this.itemWindowEnableOneList[num][index];
			int num2 = listBarKind.id;
			SelCharaGrowCtrl.ListKind kind = listBarKind.kind;
			if (kind != SelCharaGrowCtrl.ListKind.Quest)
			{
				if (kind == SelCharaGrowCtrl.ListKind.Shop)
				{
					int id = this.itemWindowEnableOneList[num][index].id;
					num2 = id;
					ShopData shopData = DataManager.DmShop.GetShopData(id);
					guiCharaGrowListBar.Setup(shopData, this._guiData.MultiItemInfoWindow.IconItemCtrlList[num].iconItemCtrl.itemStaticBase.GetId());
				}
			}
			else
			{
				int id2 = this.itemWindowEnableOneList[num][index].id;
				num2 = id2;
				QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(id2);
				guiCharaGrowListBar.Setup(questOnePackData);
			}
			guiCharaGrowListBar.AddParam(num2, kind.ToString());
			return;
		}
		go.SetActive(false);
	}

	private IEnumerator ItemExchange(bool isMulti)
	{
		List<DataManagerItem.ExchangeRatesData> exchageRatesList = DataManager.DmItem.GetExchageRatesList();
		List<ItemData> itemList = this.lastOpenItemWindowParam.itemList;
		int index = this.lastOpenItemWindowParam.index;
		DataManagerItem.ExchangeRatesData exchangeRate = ((exchageRatesList.Count < index) ? null : exchageRatesList.Find((DataManagerItem.ExchangeRatesData item) => item.targetItemId == itemList[index].staticData.GetId()));
		if (exchangeRate == null)
		{
			yield break;
		}
		int executeCount = 0;
		CanvasManager.HdlSelCharaGrowItemExchangeWindowCtrl.Setup(exchangeRate, delegate(int action)
		{
			executeCount = action;
		});
		CanvasManager.HdlSelCharaGrowItemExchangeWindowCtrl.guiItemUseWindow.window.Open();
		do
		{
			yield return null;
		}
		while (!CanvasManager.HdlSelCharaGrowItemExchangeWindowCtrl.guiItemUseWindow.window.FinishedClose());
		if (executeCount == 0)
		{
			yield break;
		}
		ItemData userSourceItem = DataManager.DmItem.GetUserItemData(exchangeRate.sourceItemId);
		ItemData userTargetItem = DataManager.DmItem.GetUserItemData(exchangeRate.targetItemId);
		DataManager.DmItem.RequestItemExchange(executeCount, exchangeRate.targetItemId);
		do
		{
			yield return null;
		}
		while (DataManager.IsServerRequesting());
		this._guiData.WindowBuyEnd.owCtrl.Setup("変換完了", "を交換しました。", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, null, null, false);
		ItemStaticBase itemStaticBase = DataManager.DmItem.GetItemStaticBase(exchangeRate.sourceItemId);
		ItemStaticBase itemStaticBase2 = DataManager.DmItem.GetItemStaticBase(exchangeRate.targetItemId);
		string text = "{0}{1}個を{2}{3}個";
		this._guiData.WindowBuyEnd.Txt_ItemName.text = string.Format(text, new object[]
		{
			itemStaticBase.GetName(),
			exchangeRate.useNum * executeCount,
			itemStaticBase2.GetName(),
			exchangeRate.gainNum * executeCount
		});
		this._guiData.WindowBuyEnd.Txt_ItemCount.text = "に変換しました。";
		this._guiData.WindowBuyEnd.Txt_ItemReq.text = "";
		this._guiData.WindowBuyEnd.Txt_BuyBeforeMoney.text = userSourceItem.num.ToString();
		this._guiData.WindowBuyEnd.Txt_BuyBeforeCount.text = userTargetItem.num.ToString();
		userSourceItem = DataManager.DmItem.GetUserItemData(exchangeRate.sourceItemId);
		userTargetItem = DataManager.DmItem.GetUserItemData(exchangeRate.targetItemId);
		this._guiData.WindowBuyEnd.Txt_BuyAfterMoney.text = userSourceItem.num.ToString();
		this._guiData.WindowBuyEnd.Txt_BuyAfterCount.text = userTargetItem.num.ToString();
		this._guiData.WindowBuyEnd.UseInfoImage.SetRawImage(itemStaticBase2.GetIconName(), true, false, null);
		this._guiData.WindowBuyEnd.UseMoneyImage.SetRawImage(itemStaticBase.GetIconName(), true, false, null);
		this._guiData.WindowBuyEnd.UseInfoImage.gameObject.SetActive(true);
		this._guiData.WindowBuyEnd.owCtrl.Open();
		do
		{
			yield return null;
		}
		while (!this._guiData.WindowBuyEnd.owCtrl.FinishedClose());
		ExchangeExecuteCountInfo exchangeExecuteCountInfo = DataManager.DmItem.GetExecuteCountInfos().Find((ExchangeExecuteCountInfo info) => info.targetItemId == exchangeRate.targetItemId);
		int num = ((exchangeExecuteCountInfo == null) ? exchangeRate.monthlyExchangeLimit : (exchangeRate.monthlyExchangeLimit - exchangeExecuteCountInfo.executeCount));
		bool flag = (exchangeExecuteCountInfo == null || exchangeRate.monthlyExchangeLimit > exchangeExecuteCountInfo.executeCount) && userSourceItem.num >= exchangeRate.useNum;
		if (!isMulti)
		{
			this._guiData.SingleItemInfoWindow.iconItemPack.Txt_Num.text = string.Format("{0}/{1}", userTargetItem.num, itemList[0].num);
			this._guiData.SingleItemInfoWindow.sourceIconItemPack.Txt_Num.ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[]
			{
				userSourceItem.num.ToString(),
				exchangeRate.useNum.ToString()
			});
			this._guiData.SingleItemInfoWindow.exchangeButton.SetActEnable(flag, false, false);
			this._guiData.SingleItemInfoWindow.remainExecuteText.ReplaceTextByDefault("Param01", num.ToString());
			this._guiData.SingleItemInfoWindow.remainExecuteText.gameObject.SetActive(true);
		}
		else
		{
			this._guiData.MultiItemInfoWindow.IconItemCtrlList[index].Txt_Num.text = string.Format("{0}/{1}", userTargetItem.num, itemList[index].num);
			ItemData itemData = itemList.Find((ItemData item) => item.id == userSourceItem.id);
			int num2 = itemList.IndexOf(itemData);
			if (num2 != -1)
			{
				this._guiData.MultiItemInfoWindow.IconItemCtrlList[num2].Txt_Num.text = string.Format("{0}/{1}", userSourceItem.num, itemList[num2].num);
			}
			this._guiData.MultiItemInfoWindow.sourceIconItemPack.Txt_Num.ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[]
			{
				userSourceItem.num.ToString(),
				exchangeRate.useNum.ToString()
			});
			this._guiData.MultiItemInfoWindow.exchangeButton.SetActEnable(flag, false, false);
			this._guiData.MultiItemInfoWindow.remainExecuteText.ReplaceTextByDefault("Param01", num.ToString());
			this._guiData.MultiItemInfoWindow.remainExecuteText.gameObject.SetActive(true);
		}
		this.SetActStrengthButton(this._guiData.CharacterGrowMain.Cmn.TabGroup.SelectIndex, false);
		yield break;
	}

	private IEnumerator RequestExecuteCountInfo(bool isMulti)
	{
		bool executeCountInfos = DataManager.DmItem.GetExecuteCountInfos() != null;
		bool flag = DataManager.DmItem.IsNeededUpdateExecuteCountInfos();
		if (executeCountInfos && !flag)
		{
			yield break;
		}
		DataManager.DmItem.RequestExchangeExecuteList();
		do
		{
			yield return null;
		}
		while (DataManager.IsServerRequesting());
		List<DataManagerItem.ExchangeRatesData> exchageRatesList = DataManager.DmItem.GetExchageRatesList();
		int index = this.lastOpenItemWindowParam.index;
		ItemStaticBase itemStatic = (isMulti ? this._guiData.MultiItemInfoWindow.IconItemCtrlList[index].iconItemCtrl.itemStaticBase : this.lastOpenItemWindowParam.itemList[0].staticData);
		DataManagerItem.ExchangeRatesData exchangeRate = exchageRatesList.Find((DataManagerItem.ExchangeRatesData item) => item.targetItemId == itemStatic.GetId());
		if (exchangeRate == null)
		{
			yield break;
		}
		ItemData userItemData = DataManager.DmItem.GetUserItemData(exchangeRate.sourceItemId);
		List<ExchangeExecuteCountInfo> executeCountInfos2 = DataManager.DmItem.GetExecuteCountInfos();
		ExchangeExecuteCountInfo exchangeExecuteCountInfo = ((executeCountInfos2 == null) ? null : executeCountInfos2.Find((ExchangeExecuteCountInfo info) => info.targetItemId == exchangeRate.targetItemId));
		bool flag2 = executeCountInfos2 != null && (exchangeExecuteCountInfo == null || exchangeRate.monthlyExchangeLimit > exchangeExecuteCountInfo.executeCount) && userItemData.num >= exchangeRate.useNum;
		int num = ((exchangeExecuteCountInfo == null) ? exchangeRate.monthlyExchangeLimit : (exchangeRate.monthlyExchangeLimit - exchangeExecuteCountInfo.executeCount));
		if (isMulti)
		{
			this._guiData.MultiItemInfoWindow.exchangeButton.SetActEnable(flag2, false, false);
			this._guiData.MultiItemInfoWindow.remainExecuteText.ReplaceTextByDefault("Param01", num.ToString());
			this._guiData.MultiItemInfoWindow.remainExecuteText.gameObject.SetActive(true);
		}
		else
		{
			this._guiData.SingleItemInfoWindow.exchangeButton.SetActEnable(flag2, false, false);
			this._guiData.SingleItemInfoWindow.remainExecuteText.ReplaceTextByDefault("Param01", num.ToString());
			this._guiData.SingleItemInfoWindow.remainExecuteText.gameObject.SetActive(true);
		}
		yield break;
	}

	[CompilerGenerated]
	private int <CharaGrowSetup>g__GetCanReceiveGrowthAchievementRewardCount|117_0()
	{
		List<DataManagerCharaMission.DynamicCharaMission.MissionOne> list = DataManager.DmChMission.GetDynamicCharaMissionData(this._currentCharaId).MissionMap.Values.ToList<DataManagerCharaMission.DynamicCharaMission.MissionOne>().FindAll((DataManagerCharaMission.DynamicCharaMission.MissionOne match) => !match.Received);
		int num = 0;
		using (List<DataManagerCharaMission.DynamicCharaMission.MissionOne>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.CanReceive)
				{
					num++;
				}
			}
		}
		return num;
	}

	private readonly string ALL_CANCEL_TEXT = "全解除";

	private readonly string ENHANCEMENT_TEXT = "強化";

	private static readonly int LEVEL_TEXT_SIZE = 22;

	private readonly string DEFAULT_LEVEL_TEXT = string.Format("<size={0}>Lv.</size>", SelCharaGrowCtrl.LEVEL_TEXT_SIZE);

	private readonly string KIZUNA_LEVEL_TEXT = string.Format("<size={0}>なかよしLv.</size>", SelCharaGrowCtrl.LEVEL_TEXT_SIZE);

	private readonly string NEXT_KIZUNA_LEVEL_DESCRIPTION_TEXT = "次のなかよしLvまで";

	private readonly string USING_ITEM_NUM_SELECTION_TITLE_TEXT = "使用アイテム個数選択";

	private readonly string TAB_INFO_WILD_RELEASE_TEXT = "野生解放素材を使ってフレンズのステータスを強化します";

	private readonly string TAB_INFO_RANK_UP_TEXT = "小さな輝石を使ってフレンズのけも級(★)を上げます";

	private readonly string TAB_INFO_MIRACLE_UP_TEXT = "ミラクル強化素材を使ってけものミラクルを強化します";

	private readonly string TAB_INFO_NANAIRO_TEXT = "素材を使ってなないろとくせいを解放します";

	private readonly string TAB_INFO_PHOTO_POCKET_TEXT = "輝石を使ってフォトポケランクを強化します";

	private readonly string HOW_TO_GET_TEXT = "入手方法";

	private readonly string RELEASE_TEXT = "解放";

	private readonly string HP_UP_TEXT = "たいりょく +";

	private readonly string ATTACK_UP_TEXT = "こうげき +";

	private readonly string DEFENSE_UP_TEXT = "まもり +";

	private readonly string AVOID_UP_TEXT = "かいひ +";

	private readonly string WILD_RELEASE_BEAT_DAMAGE_UP_TEXT = "Beat!!!ダメージアップ +";

	private readonly string WILD_RELEASE_TRY_DAMAGE_UP_TEXT = "Try!!ダメージアップ +";

	private readonly string WILD_RELEASE_ACTION_DAMAGE_UP_TEXT = "Action!ダメージアップ +";

	private readonly string WILD_RELEASE_RESULT_TITLE_TEXT = "野生解放結果";

	private readonly string WILD_RELEASE_RESULT_MESSAGE_TEXT = "";

	private readonly string WILD_RELEASE_ABILITY_OPEN_TITLE_TEXT = "とくせい解放！";

	private readonly string WILD_RELEASE_ABILITY_OPEN_MESSAGE_TEXT = "とくせい解放！";

	private readonly string WILD_RELEASE_ICON_OPEN_TITLE_TEXT = "アイコン解放！";

	private readonly string WILD_RELEASE_ICON_OPEN_MESSAGE_TEXT = "アイコン解放！";

	private readonly string PHOTO_POCKET_EXCHANGE_TITLE_TEXT = "輝石の変換";

	private readonly string OTHER_PHOTO_POCKET_EXCHANGE_TITLE_TEXT = "小さな輝石の変換";

	private readonly string EXCHANGE_CONFIRM_TEXT = "変換確認";

	private readonly string POSSESSION_NUMBER_TEXT = "所持数";

	private readonly string POSSIBLE_EXCHANGE_NUMBER_TEXT = "変換可能数";

	private readonly string GEMSTONE_TEXT = "輝石";

	private readonly string SMALL_GEMSTONE_TEXT = "小さな輝石";

	private readonly string LEVEL_UP_MAX_INFO_MESSAGE_TEXT = "レベルは最大まで強化されています";

	private readonly string WILD_RELEASE_MAX_INFO_MESSAGE_TEXT = "野生解放は最大まで強化されています";

	private readonly string RANK_UP_MAX_INFO_MESSAGE_TEXT = "けも級は最大まで強化されています";

	private readonly string MIRACLEUP_MAX_INFO_MESSAGE_TEXT = "けものミラクルは最大まで強化されています";

	private readonly string PHOTO_POCKET_MAX_INFO_MESSAGE_TEXT = "フォトポケランクは最大まで強化されています";

	private readonly string KIZUNA_UP_MAX_INFO_MESSAGE_TEXT = "なかよしレベル上限は最大まで強化されています";

	private readonly string NANAIRO_UP_MAX_INFO_MESSAGE_TEXT = "なないろとくせいは解放されています";

	private readonly string EXCHANGE_NUMBER_TEXT = "変換数";

	private readonly string CANCEL_TEXT = "キャンセル";

	private readonly string ENHANCE_TEXT = "強化する";

	private readonly string ALL_RELEASE_TITLE_TEXT = "まとめて解放";

	private readonly string ALL_RELEASE_MESSAGE_TEXT = "まとめて解放しますか？";

	private readonly string RELEASE_DISABLE_MESSAGE_TEXT = "ゴールドが足りません";

	private readonly string WILD_RELEASE_TITLE_TEXT = "野生解放確認";

	private readonly string WILD_RELEASE_MESSAGE_TEXT = "野生解放しますか？";

	private readonly string RETURN_ITEM_TITLE_TEXT = "確認";

	private readonly string RETURN_ITEM_MESSAGE_TEXT = "EXPの上限を超えた分の\nジャパまんとゴールドは使用を取り消しました！";

	private readonly string RETURN_KIZUNA_ITEM_MESSAGE_TEXT = "EXPの上限を超えた分の\nマジカルキャンディとゴールドは使用を取り消しました！";

	private readonly string NUM_EXP_NEXT_TEXT = "次のLvまで";

	private readonly string PHOTO_POCKET_GEMSTONE_EXCHANGE_WARNING_TITLE_TEXT = "輝石変換時の注意";

	private readonly string PHOTO_POCKET_GEMSTONE_EXCHANGE_WARNING_MESSAGE_TEXT = "変換後は元の輝石に戻すことができません";

	private readonly string TO_RELEASE_TEXT = "解放する";

	private readonly string NANAIRO_RELEASE_TITLE_TEXT = "解放確認";

	private readonly string FRENDS_STORY_EX_RELEASE_TITLE_TEXT = "";

	private readonly string FRENDS_STORY_EX_RELEASE_MESSAGE_TEXT = "フレンズストーリーのEXが解放されました";

	public static readonly string ARTS_HIGHLIGHT_COLOR_CODE = "#6B510AFF";

	public static readonly int ARTS_TEXT_SIZE = 24;

	private GameObject _mainCharaGrowObj;

	private SelCharaGrowCtrl.CommonGUI _guiData;

	private SelCharaGrowLevel _charaGrowLevelUp;

	private SelCharaGrowWild _charaGrowWild;

	private SelCharaGrowRank _charaGrowRank;

	private SelCharaGrowMiracle _charaGrowMiracle;

	private SelCharaGrowPhotoPocket _charaGrowPhotoPocket;

	private SelCharaGrowKizuna _charaGrowKizuna;

	private SelCharaGrowNanairo _charaGrowNanairo;

	private SelCharaGrowMulti _charaGrowMulti;

	private GameObject _charaGrowWindow;

	private SelCharaGrowCtrl.Mode _currentMode;

	private int _currentCharaId;

	private SortFilterDefine.SortType _sortType = SortFilterDefine.SortType.LEVEL;

	private List<bool> _enhanceList;

	private int _lastTabIndex;

	private List<int> _selectLvUpItemIdList;

	private List<int> _selectKizunaLvUpItemIdList;

	private int _lvUpCostCoin;

	private SelCharaGrowCtrl.EffectStatus _effectStatus;

	private RenderTextureChara _renderTextureChara;

	private DataManagerChara.SimulateAddExpResult _saExpResult;

	private List<CharaPackData> _haveCharaPackList;

	private int _nextOpenPhotoIndex;

	private DataManagerCampaign.CampaignGrowData _currentCampaign;

	private float _voiceLength;

	private float _voiceStartTime;

	private bool _touchRect;

	private bool _touchScreenAuth;

	private bool _wildReleaseEffectPlaying;

	private IEnumerator _itemExchange;

	private bool _isTutorial;

	private List<CharaPackData> _dispCharaPackList;

	private int _tutorialClickTabIndex;

	private bool _enableLeftRightButton;

	private bool _enableMoreButton;

	private IEnumerator _expGageEffect;

	private IEnumerator levelLimitOverEffect;

	private bool _pressedButtonRWildRelease;

	private IEnumerator _wildReleaseEffect;

	private IEnumerator _rankUpGageEffect;

	private IEnumerator _miracleEffect;

	private IEnumerator _nanairoReleaseEffect;

	private IEnumerator _photoPocketEffect;

	private IEnumerator _ieKizunaLimitOver;

	private IEnumerator _itemExchangeEffect;

	private bool _GettingAchievement;

	private IEnumerator _growthAchievementRewardEffect;

	private IEnumerator _ieReleaseAccessory;

	private IEnumerator _connectFlowBase;

	private IEnumerator _growMultiCoroutine;

	private List<List<SelCharaGrowCtrl.ListBarKind>> itemWindowEnableOneList;

	private SelCharaGrowCtrl.OpenItemWindowParam lastOpenItemWindowParam = new SelCharaGrowCtrl.OpenItemWindowParam();

	public enum Mode
	{
		Top,
		Main
	}

	public enum TabType
	{
		LevelUp,
		WildRelease,
		RankUp,
		MiracleUp,
		Nanairo,
		PhotoPocket,
		Kizuna
	}

	public enum EffectStatus
	{
		None,
		ReqServer,
		Execute,
		Result,
		ResultEnd,
		Finished
	}

	public class PhotoPocketIcon
	{
		public PguiImageCtrl IconPhoto
		{
			get
			{
				return this._iconPhoto;
			}
		}

		public PguiReplaceSpriteCtrl ReplaceIconPhoto
		{
			get
			{
				return this._replaceIconPhoto;
			}
		}

		public PguiReplaceSpriteCtrl MarkKiseki
		{
			get
			{
				return this._markKiseki;
			}
		}

		public GameObject Current
		{
			get
			{
				return this._current;
			}
		}

		public PguiTextCtrl NumLv
		{
			get
			{
				return this._numLv;
			}
		}

		public PhotoPocketIcon(Transform baseTr)
		{
			this._baseObj = baseTr.gameObject;
			this._iconPhoto = baseTr.GetComponent<PguiImageCtrl>();
			this._replaceIconPhoto = baseTr.GetComponent<PguiReplaceSpriteCtrl>();
			this._markKiseki = baseTr.Find("Mark_Kiseki").GetComponent<PguiReplaceSpriteCtrl>();
			if (baseTr.Find("Current") != null)
			{
				this._current = baseTr.Find("Current").gameObject;
			}
			this._numLv = baseTr.Find("Num_Lv").GetComponent<PguiTextCtrl>();
		}

		public void AddTouchEventTrigger(UnityAction<Transform> cb)
		{
			if (this.Current != null)
			{
				PrjUtil.AddTouchEventTrigger(this._baseObj, cb);
			}
		}

		public void SetActiveCurrent(bool sw)
		{
			if (this.Current != null)
			{
				this.Current.SetActive(sw);
			}
		}

		private GameObject _baseObj;

		private PguiImageCtrl _iconPhoto;

		private PguiReplaceSpriteCtrl _replaceIconPhoto;

		private PguiReplaceSpriteCtrl _markKiseki;

		private GameObject _current;

		private PguiTextCtrl _numLv;
	}

	public class WindowGrowthAchievementReward
	{
		public GameObject BaseObj
		{
			get
			{
				return this._baseObj;
			}
		}

		public ReuseScroll ScrollView
		{
			get
			{
				return this._scrollView;
			}
		}

		public PguiOpenWindowCtrl OpenWindowCtrl
		{
			get
			{
				return this._openWindowCtrl;
			}
		}

		public WindowGrowthAchievementReward(Transform baseTr)
		{
			this._baseObj = baseTr.gameObject;
			this._cautionText = baseTr.Find("Base/Window/Txt_Caution").GetComponent<PguiTextCtrl>();
			this._numList = baseTr.Find("Base/Window/Num_List").GetComponent<PguiTextCtrl>();
			this._scrollView = baseTr.Find("Base/Window/ListAll/ScrollView").GetComponent<ReuseScroll>();
			this._openWindowCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
		}

		public void Refresh(int chId)
		{
			List<DataManagerCharaMission.DynamicCharaMission.MissionOne> list = DataManager.DmChMission.GetDynamicCharaMissionData(chId).MissionMap.Values.ToList<DataManagerCharaMission.DynamicCharaMission.MissionOne>().FindAll((DataManagerCharaMission.DynamicCharaMission.MissionOne match) => !match.Received);
			int count = list.Count;
			int count2 = list.FindAll((DataManagerCharaMission.DynamicCharaMission.MissionOne x) => x.CanReceive).Count;
			this._scrollView.Resize(count + 1, 0);
			this._numList.text = string.Format("あと{0}/{1}", count2, list.Count);
			this._cautionText.gameObject.SetActive(count == 0);
		}

		private PguiTextCtrl _numList;

		private PguiTextCtrl _cautionText;

		private GameObject _baseObj;

		private ReuseScroll _scrollView;

		private PguiOpenWindowCtrl _openWindowCtrl;
	}

	public class CommonGUI
	{
		public GameObject BaseObj
		{
			get
			{
				return this._baseObj;
			}
		}

		public SelCharaGrowCtrl.CommonGUI.CharaGrowTop CharacterGrowTop
		{
			get
			{
				return this._charaGrowTop;
			}
		}

		public SelCharaGrowCtrl.CommonGUI.CharaGrowMain CharacterGrowMain
		{
			get
			{
				return this._charaGrowMain;
			}
		}

		public SelCharaGrowPhotoPocket.WindowItemExchange ItemExchangeWindow
		{
			get
			{
				return this._itemExchangeWindow;
			}
		}

		public SelCharaGrowCtrl.SingleItemInfoWindow SingleItemInfoWindow
		{
			get
			{
				return this._singleItemInfoWindow;
			}
		}

		public SelCharaGrowCtrl.MultipleItemInfoWindow MultiItemInfoWindow
		{
			get
			{
				return this._multiItemInfoWindow;
			}
		}

		public SelShopCtrl.WindowBuyEnd WindowBuyEnd
		{
			get
			{
				return this._windowBuyEnd;
			}
		}

		public SelCharaGrowCtrl.WindowGrowthAchievementReward GrowthAchievementRewardWindow
		{
			get
			{
				return this._growthAchievementRewardWindow;
			}
		}

		public CommonGUI(Transform baseTr)
		{
			this._baseObj = baseTr.gameObject;
			this._charaGrowTop = new SelCharaGrowCtrl.CommonGUI.CharaGrowTop(this._baseObj.transform.Find("CharaGrow_Top"));
			this._charaGrowMain = new SelCharaGrowCtrl.CommonGUI.CharaGrowMain(this._baseObj.transform.Find("CharaGrow_Main"));
			this._charaGrowMain.BaseObj.SetActive(false);
			GameObject gameObject = (GameObject)Resources.Load("SceneCharaEdit/GUI/Prefab/GUI_CharaEdit_Window_ItemExchange");
			this._itemExchangeWindow = new SelCharaGrowPhotoPocket.WindowItemExchange(Object.Instantiate<Transform>(gameObject.transform.Find("Window_ItemExchange"), baseTr).transform);
			GameObject gameObject2 = (GameObject)Resources.Load("SceneCharaEdit/GUI/Prefab/GUI_CharaEdit_Window_ItemInfo");
			this._singleItemInfoWindow = new SelCharaGrowCtrl.SingleItemInfoWindow(Object.Instantiate<Transform>(gameObject2.transform.Find("Window_ItemInfo"), baseTr).transform);
			this._multiItemInfoWindow = new SelCharaGrowCtrl.MultipleItemInfoWindow(Object.Instantiate<Transform>(gameObject2.transform.Find("Window_ItemInfoAll"), baseTr).transform);
			GameObject gameObject3 = AssetManager.GetAssetData("Cmn/GUI/Prefab/GUI_Cmn_ShopWindow_Result") as GameObject;
			this._windowBuyEnd = new SelShopCtrl.WindowBuyEnd(Object.Instantiate<Transform>(gameObject3.transform, Singleton<CanvasManager>.Instance.SystemMiddleArea).transform);
			GameObject gameObject4 = (GameObject)Resources.Load("SceneCharaEdit/GUI/Prefab/GUI_CharaEdit_Window_GrowPresent");
			this._growthAchievementRewardWindow = new SelCharaGrowCtrl.WindowGrowthAchievementReward(Object.Instantiate<Transform>(gameObject4.transform.Find("Window_GrowPresent"), baseTr).transform);
		}

		private GameObject _baseObj;

		private SelCharaGrowCtrl.CommonGUI.CharaGrowTop _charaGrowTop;

		private SelCharaGrowCtrl.CommonGUI.CharaGrowMain _charaGrowMain;

		private SelCharaGrowPhotoPocket.WindowItemExchange _itemExchangeWindow;

		private SelCharaGrowCtrl.SingleItemInfoWindow _singleItemInfoWindow;

		private SelCharaGrowCtrl.MultipleItemInfoWindow _multiItemInfoWindow;

		private SelShopCtrl.WindowBuyEnd _windowBuyEnd;

		private SelCharaGrowCtrl.WindowGrowthAchievementReward _growthAchievementRewardWindow;

		public class CharaGrowTop
		{
			public GameObject BaseObj
			{
				get
				{
					return this._baseObj;
				}
			}

			public PguiButtonCtrl BtnFilterOnOff
			{
				get
				{
					return this._btnFilterOnOff;
				}
			}

			public PguiButtonCtrl BtnSort
			{
				get
				{
					return this._btnSort;
				}
			}

			public PguiButtonCtrl BtnSortUpDown
			{
				get
				{
					return this._btnSortUpDown;
				}
			}

			public ReuseScroll ScrollView
			{
				get
				{
					return this._scrollView;
				}
			}

			public PguiImageCtrl CampaignInfo
			{
				get
				{
					return this._campaignInfo;
				}
			}

			public PguiTextCtrl CampaignTimeInfo
			{
				get
				{
					return this._campaignTimeInfo;
				}
			}

			public SimpleAnimation SelCmnAllInOut
			{
				get
				{
					return this._selCmnAllInOut;
				}
			}

			public GameObject TxtNone
			{
				get
				{
					return this._txtNone;
				}
			}

			public CharaGrowTop(Transform baseTr)
			{
				this._baseObj = baseTr.gameObject;
				this._btnFilterOnOff = baseTr.Find("All/WindowAll/SortFilterBtnsAll/Btn_FilterOnOff").GetComponent<PguiButtonCtrl>();
				this._btnSort = baseTr.Find("All/WindowAll/SortFilterBtnsAll/Btn_Sort").GetComponent<PguiButtonCtrl>();
				this._btnSortUpDown = baseTr.Find("All/WindowAll/SortFilterBtnsAll/Btn_SortUpDown").GetComponent<PguiButtonCtrl>();
				this._scrollView = baseTr.Find("All/WindowAll/ScrollView").GetComponent<ReuseScroll>();
				this._campaignInfo = baseTr.Find("All/WindowAll/Campaign/SelCmn_CampaignInfo").GetComponent<PguiImageCtrl>();
				this._campaignTimeInfo = baseTr.Find("All/WindowAll/Campaign/SelCmn_CampaignInfo/TimeInfo/Num_Time").GetComponent<PguiTextCtrl>();
				this._txtNone = baseTr.Find("All/WindowAll/Txt_None").gameObject;
				this._txtNone.SetActive(false);
				this._selCmnAllInOut = baseTr.GetComponent<SimpleAnimation>();
			}

			private GameObject _baseObj;

			private PguiButtonCtrl _btnFilterOnOff;

			private PguiButtonCtrl _btnSort;

			private PguiButtonCtrl _btnSortUpDown;

			private ReuseScroll _scrollView;

			private PguiImageCtrl _campaignInfo;

			private PguiTextCtrl _campaignTimeInfo;

			private SimpleAnimation _selCmnAllInOut;

			private GameObject _txtNone;
		}

		public class CharaGrowMain
		{
			public GameObject BaseObj
			{
				get
				{
					return this._baseObj;
				}
			}

			public SelCharaGrowCtrl.CommonGUI.CmnTabGUI Cmn
			{
				get
				{
					return this._cmn;
				}
			}

			public SelCharaGrowCtrl.CommonGUI.CharaGrowMain.StatusInfo StatusInfomation
			{
				get
				{
					return this._statusInfo;
				}
			}

			public SelCharaGrowCtrl.CommonGUI.PhotoMax PhotoMax
			{
				get
				{
					return this._photoMax;
				}
			}

			public SelCharaGrowCtrl.CommonGUI.PhotoPocketConversionStrengthen PhotoPocketConversionStrengthen
			{
				get
				{
					return this._photoPocketConversionStrengthen;
				}
			}

			public SelCharaGrowLevel.LvLimitOpen LvLimitOpen
			{
				get
				{
					return this._lvLimitOpen;
				}
			}

			public PguiButtonCtrl BtnYajiLeft
			{
				get
				{
					return this._btnYajiLeft;
				}
			}

			public PguiButtonCtrl BtnYajiRight
			{
				get
				{
					return this._btnYajiRight;
				}
			}

			public PguiButtonCtrl BtnMoreInfo
			{
				get
				{
					return this._btnMoreInfo;
				}
			}

			public PguiButtonCtrl BtnPresent
			{
				get
				{
					return this._btnPresent;
				}
			}

			public PguiTextCtrl TxtPresentNum
			{
				get
				{
					return this._txtPresentNum;
				}
			}

			public PguiToggleButtonCtrl BtnFavorite
			{
				get
				{
					return this._btnFavorite;
				}
			}

			public PguiButtonCtrl BtnGrowMulti
			{
				get
				{
					return this._btnGrowMulti;
				}
			}

			public PguiImageCtrl MarkMax
			{
				get
				{
					return this._markMax;
				}
			}

			public PguiTextCtrl TxtMaxInfo
			{
				get
				{
					return this._txtMaxInfo;
				}
			}

			public PguiImageCtrl CampaignInfo
			{
				get
				{
					return this._campaignInfo;
				}
			}

			public PguiTextCtrl CampaignTimeInfo
			{
				get
				{
					return this._campaignTimeInfo;
				}
			}

			public PguiAECtrl ImageCharaEffect
			{
				get
				{
					return this._imageCharaEffect;
				}
			}

			public SimpleAnimation CharaEditCharaGrowSE
			{
				get
				{
					return this._charaEditCharaGrowSE;
				}
			}

			public Dictionary<SelCharaGrowCtrl.TabType, GameObject> TabObjectMap
			{
				get
				{
					return this._tabObjectMap;
				}
			}

			public PguiButtonCtrl BtnAccessory
			{
				get
				{
					return this._btnAccessory;
				}
			}

			public AEImage BtnAccessoryImageOpenOK
			{
				get
				{
					return this._btnAccessoryImageOpenOK;
				}
			}

			public AEImage BtnAccessoryImageOpen
			{
				get
				{
					return this._btnAccessoryImageOpen;
				}
			}

			public CharaGrowMain(Transform baseTr)
			{
				this._baseObj = baseTr.gameObject;
				this._txtCharaName = baseTr.Find("Main/Left/CharaName/Txt_CharaName").GetComponent<PguiTextCtrl>();
				this._txtCharaNameRectTransform = baseTr.Find("Main/Left/CharaName/Txt_CharaName").GetComponent<RectTransform>();
				this._txtCharaNameInitialPos = this._txtCharaNameRectTransform.anchoredPosition;
				this._txtCharaNameInitialSize = this._txtCharaNameRectTransform.sizeDelta;
				this._iconAtr = baseTr.Find("Main/Left/CharaName/Icon_Atr").GetComponent<PguiImageCtrl>();
				this._iconSubAtr = baseTr.Find("Main/Left/CharaName/Icon_SubAtr").GetComponent<PguiImageCtrl>();
				this._cmn = new SelCharaGrowCtrl.CommonGUI.CmnTabGUI(baseTr.Find("Main/Right/WindowBase"));
				this._statusInfo = new SelCharaGrowCtrl.CommonGUI.CharaGrowMain.StatusInfo(baseTr.Find("Main/Left/StatusInfo"));
				this._tabObjectMap = new Dictionary<SelCharaGrowCtrl.TabType, GameObject>();
				this._photoMax = new SelCharaGrowCtrl.CommonGUI.PhotoMax(baseTr.Find("Main/Right/PhotoMax"));
				this._photoPocketConversionStrengthen = new SelCharaGrowCtrl.CommonGUI.PhotoPocketConversionStrengthen(baseTr.Find("Main/Right/Photo_Exchange"));
				this._lvLimitOpen = new SelCharaGrowLevel.LvLimitOpen(baseTr.Find("Main/Right/LvLimitOpen"));
				this._btnYajiLeft = baseTr.Find("LeftBtn/Btn_Yaji_Left").GetComponent<PguiButtonCtrl>();
				this._btnYajiRight = baseTr.Find("RightBtn/Btn_Yaji_Right").GetComponent<PguiButtonCtrl>();
				this._btnMoreInfo = baseTr.Find("Main/Left/Btn_MoreInfo").GetComponent<PguiButtonCtrl>();
				this._btnPresent = baseTr.Find("Main/Left/Btn_Present").GetComponent<PguiButtonCtrl>();
				this._txtPresentNum = this._btnPresent.transform.Find("BaseImage/Badges/Cmn_Badge/Num").GetComponent<PguiTextCtrl>();
				this._btnFavorite = baseTr.Find("Main/Left/Btn_Favorite").GetComponent<PguiToggleButtonCtrl>();
				this._btnGrowMulti = baseTr.Find("Main/Left/Btn_GrowMulti").GetComponent<PguiButtonCtrl>();
				this._markMax = baseTr.Find("Main/Right/Mark_Max").GetComponent<PguiImageCtrl>();
				this._txtMaxInfo = baseTr.Find("Main/Right/Txt_MaxInfo").GetComponent<PguiTextCtrl>();
				this._campaignInfo = baseTr.Find("Main/Right/Campaign/SelCmn_CampaignInfo").GetComponent<PguiImageCtrl>();
				this._campaignTimeInfo = baseTr.Find("Main/Right/Campaign/SelCmn_CampaignInfo/TimeInfo/Num_Time").GetComponent<PguiTextCtrl>();
				this._imageCharaEffect = baseTr.Find("Main/Left/AEImage_CharaEffect").GetComponent<PguiAECtrl>();
				this._imageCharaEffect.gameObject.SetActive(false);
				this._charaEditCharaGrowSE = baseTr.GetComponent<SimpleAnimation>();
				this._btnAccessory = baseTr.Find("Main/Left/Btn_Accessory").GetComponent<PguiButtonCtrl>();
				this._btnAccessory.gameObject.SetActive(false);
				this._btnAccessoryImageOpenOK = this._btnAccessory.transform.Find("AEImage_OpenOK").GetComponent<AEImage>();
				this._btnAccessoryImageOpenOK.gameObject.SetActive(false);
				this._btnAccessoryImageOpen = this._btnAccessory.transform.Find("AEImage_Open").GetComponent<AEImage>();
				this._btnAccessoryImageOpen.gameObject.SetActive(false);
			}

			public void SetCharaInfo(CharaPackData charaPackData)
			{
				this._txtCharaName.ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[]
				{
					charaPackData.staticData.baseData.NickName,
					charaPackData.staticData.GetName()
				});
				this._iconAtr.SetImageByName(IconCharaCtrl.Attribute2IconName(charaPackData.staticData.baseData.attribute));
				if (charaPackData.staticData.baseData.subAttribute <= CharaDef.AttributeType.ALL)
				{
					this._txtCharaNameRectTransform.anchoredPosition = this._txtCharaNameInitialPos - new Vector2(30f, 0f);
					this._txtCharaNameRectTransform.sizeDelta = this._txtCharaNameInitialSize + new Vector2(30f, 0f);
					this._iconSubAtr.gameObject.SetActive(false);
					return;
				}
				this._txtCharaNameRectTransform.anchoredPosition = this._txtCharaNameInitialPos;
				this._iconSubAtr.SetImageByName(IconCharaCtrl.SubAttribute2IconName(charaPackData.staticData.baseData.subAttribute));
				this._iconSubAtr.gameObject.SetActive(true);
			}

			public void UpdateCharaParameter(CharaPackData charaPackData)
			{
				CharaDynamicData dynamicData = charaPackData.dynamicData;
				PrjUtil.ParamPreset paramPreset = PrjUtil.CalcParamByChara(dynamicData, null, null, null);
				DataManagerKemoBoard.KemoBoardBonusParam kemoBoardBonusParam = DataManager.DmKemoBoard.KemoBoardBonusParamMap[charaPackData.staticData.baseData.attribute];
				this._statusInfo.NumLv.ReplaceTextByDefault("Param01", string.Format("{0}/{1}", dynamicData.level, dynamicData.limitLevel));
				this._statusInfo.NumWild.text = string.Format("{0}/{1}", dynamicData.promoteNum, charaPackData.staticData.maxPromoteNum);
				this._statusInfo.NumArts.text = string.Format("{0}/{1}", dynamicData.artsLevel, dynamicData.limitMiracleLevel);
				this._statusInfo.NumStatus.text = string.Format("{0}", paramPreset.totalParam + kemoBoardBonusParam.KemoStatus);
				this._statusInfo.PhotoPocketLevelCtrl.Setup(new PhotoPocketLevelCtrl.SetupParam
				{
					charaPackData = charaPackData
				});
				for (int i = 0; i < this._statusInfo.StarAll.Count; i++)
				{
					PguiImageCtrl pguiImageCtrl = this._statusInfo.StarAll[i];
					PguiImageCtrl component = pguiImageCtrl.GetComponent<PguiImageCtrl>();
					string text = ((i < dynamicData.rank) ? "icon_star" : "icon_star_blank");
					DataManagerChara.LevelLimitData levelLimitData = DataManager.DmChara.GetLevelLimitData(dynamicData.levelLimitId);
					if (levelLimitData != null)
					{
						if (dynamicData.level == levelLimitData.maxLevel)
						{
							text = levelLimitData.compImageName;
						}
						else
						{
							text = levelLimitData.openImageName;
						}
					}
					text += "_m";
					component.SetImageByName(text);
					pguiImageCtrl.gameObject.SetActive(i < charaPackData.staticData.baseData.rankHigh);
				}
				for (int j = 0; j < this._statusInfo.PhotoPocketIcons.Count; j++)
				{
					SelCharaGrowCtrl.PhotoPocketIcon photoPocketIcon = this._statusInfo.PhotoPocketIcons[j];
					photoPocketIcon.IconPhoto.SetImageByName(dynamicData.PhotoPocket[j].Flag ? "friends_icon_photopocket_mini_act" : "friends_icon_photopocket_mini_none");
					photoPocketIcon.MarkKiseki.gameObject.SetActive(charaPackData.staticData.baseData.spAbilityRelPp == j + 1);
					photoPocketIcon.MarkKiseki.Replace(dynamicData.PhotoPocket[j].Flag ? 1 : 0);
					photoPocketIcon.NumLv.gameObject.SetActive(dynamicData.PhotoPocket[j].Step > 0);
					photoPocketIcon.NumLv.text = string.Format("{0}", dynamicData.PhotoPocket[j].Step);
				}
			}

			private GameObject _baseObj;

			private SelCharaGrowCtrl.CommonGUI.CmnTabGUI _cmn;

			private SelCharaGrowCtrl.CommonGUI.CharaGrowMain.StatusInfo _statusInfo;

			private SelCharaGrowCtrl.CommonGUI.PhotoMax _photoMax;

			private SelCharaGrowCtrl.CommonGUI.PhotoPocketConversionStrengthen _photoPocketConversionStrengthen;

			private SelCharaGrowLevel.LvLimitOpen _lvLimitOpen;

			private PguiTextCtrl _txtCharaName;

			private RectTransform _txtCharaNameRectTransform;

			private Vector2 _txtCharaNameInitialPos;

			private Vector2 _txtCharaNameInitialSize;

			private PguiImageCtrl _iconAtr;

			private PguiImageCtrl _iconSubAtr;

			private PguiButtonCtrl _btnYajiLeft;

			private PguiButtonCtrl _btnYajiRight;

			private PguiButtonCtrl _btnMoreInfo;

			private PguiButtonCtrl _btnPresent;

			private PguiTextCtrl _txtPresentNum;

			private PguiToggleButtonCtrl _btnFavorite;

			private PguiButtonCtrl _btnGrowMulti;

			private PguiImageCtrl _markMax;

			private PguiTextCtrl _txtMaxInfo;

			private PguiImageCtrl _campaignInfo;

			private PguiTextCtrl _campaignTimeInfo;

			private PguiAECtrl _imageCharaEffect;

			private SimpleAnimation _charaEditCharaGrowSE;

			private Dictionary<SelCharaGrowCtrl.TabType, GameObject> _tabObjectMap;

			private PguiButtonCtrl _btnAccessory;

			private AEImage _btnAccessoryImageOpenOK;

			private AEImage _btnAccessoryImageOpen;

			public class StatusInfo
			{
				public PguiTextCtrl NumLv
				{
					get
					{
						return this._numLv;
					}
				}

				public List<PguiImageCtrl> StarAll
				{
					get
					{
						return this._starAll;
					}
				}

				public PguiTextCtrl NumWild
				{
					get
					{
						return this._numWild;
					}
				}

				public PguiTextCtrl NumArts
				{
					get
					{
						return this._numArts;
					}
				}

				public PguiTextCtrl NumStatus
				{
					get
					{
						return this._numStatus;
					}
				}

				public List<SelCharaGrowCtrl.PhotoPocketIcon> PhotoPocketIcons
				{
					get
					{
						return this._photoPocketIcons;
					}
				}

				public PguiButtonCtrl BtnInfo
				{
					get
					{
						return this._btnInfo;
					}
				}

				public PhotoPocketLevelCtrl PhotoPocketLevelCtrl
				{
					get
					{
						return this._photoPocketLevelCtrl;
					}
				}

				public StatusInfo(Transform baseTr)
				{
					this._numLv = baseTr.Find("Num_Lv").GetComponent<PguiTextCtrl>();
					this._starAll = new List<PguiImageCtrl>();
					for (int i = 0; i < 6; i++)
					{
						this._starAll.Add(baseTr.Find("StarAll/Icon_Star" + (i + 1).ToString("D2")).GetComponent<PguiImageCtrl>());
					}
					this._numWild = baseTr.Find("Contents01/Num_Yasei").GetComponent<PguiTextCtrl>();
					this._numArts = baseTr.Find("Contents02/Num_Arts").GetComponent<PguiTextCtrl>();
					this._numStatus = baseTr.Find("Contents04/Num").GetComponent<PguiTextCtrl>();
					this._photoPocketIcons = new List<SelCharaGrowCtrl.PhotoPocketIcon>
					{
						new SelCharaGrowCtrl.PhotoPocketIcon(baseTr.Find("Contents03/Icon_PhotoPocket01")),
						new SelCharaGrowCtrl.PhotoPocketIcon(baseTr.Find("Contents03/Icon_PhotoPocket02")),
						new SelCharaGrowCtrl.PhotoPocketIcon(baseTr.Find("Contents03/Icon_PhotoPocket03")),
						new SelCharaGrowCtrl.PhotoPocketIcon(baseTr.Find("Contents03/Icon_PhotoPocket04"))
					};
					this._btnInfo = baseTr.Find("Contents03/Btn_Info").GetComponent<PguiButtonCtrl>();
					this._photoPocketLevelCtrl = baseTr.Find("Contents03/Mark_PhotoPocketLevel").GetComponent<PhotoPocketLevelCtrl>();
				}

				private PguiTextCtrl _numLv;

				private List<PguiImageCtrl> _starAll;

				private PguiTextCtrl _numWild;

				private PguiTextCtrl _numArts;

				private PguiTextCtrl _numStatus;

				private List<SelCharaGrowCtrl.PhotoPocketIcon> _photoPocketIcons;

				private PguiButtonCtrl _btnInfo;

				private PhotoPocketLevelCtrl _photoPocketLevelCtrl;
			}
		}

		public class CmnTabGUI
		{
			public PguiTabGroupCtrl TabGroup
			{
				get
				{
					return this._tabGroup;
				}
			}

			public List<SelCharaGrowCtrl.CommonGUI.CmnTabGUI.TabGui> TabGuiList
			{
				get
				{
					return this._tabGuiList;
				}
			}

			public List<RectTransform> TabRectTransformList
			{
				get
				{
					return this._tabRectTransformList;
				}
			}

			public PguiTextCtrl TxtTabInfo
			{
				get
				{
					return this._txtTabInfo;
				}
			}

			public PguiButtonCtrl ButtonL
			{
				get
				{
					return this._buttonL;
				}
			}

			public PguiTextCtrl ButtonLText
			{
				get
				{
					return this._buttonLText;
				}
			}

			public PguiButtonCtrl ButtonR
			{
				get
				{
					return this._buttonR;
				}
			}

			public PguiTextCtrl ButtonRText
			{
				get
				{
					return this._buttonRText;
				}
			}

			public PguiButtonCtrl ButtonRExchange
			{
				get
				{
					return this._buttonRExchange;
				}
			}

			public PguiRawImageCtrl ButtonRExchangeIconItem
			{
				get
				{
					return this._buttonRExchangeIconItem;
				}
			}

			public PguiTextCtrl ButtonRExchangeNumUse
			{
				get
				{
					return this._buttonRExchangeNumUse;
				}
			}

			public PguiTextCtrl NeedGoldText
			{
				get
				{
					return this._needGoldText;
				}
			}

			public PguiTextCtrl HaveGoldText
			{
				get
				{
					return this._haveGoldText;
				}
			}

			public CmnTabGUI(Transform baseTr)
			{
				this._tabGroup = baseTr.Find("TabGroup").GetComponent<PguiTabGroupCtrl>();
				this._tabGuiList = new List<SelCharaGrowCtrl.CommonGUI.CmnTabGUI.TabGui>();
				this._tabRectTransformList = new List<RectTransform>();
				for (int i = 0; i < Enum.GetValues(typeof(SelCharaGrowCtrl.TabType)).Length; i++)
				{
					SelCharaGrowCtrl.CommonGUI.CmnTabGUI.TabGui tabGui = new SelCharaGrowCtrl.CommonGUI.CmnTabGUI.TabGui(baseTr, i, this.GetItemName(i));
					RectTransform component = baseTr.Find("TabGroup/Tab" + (i + 1).ToString("D2")).GetComponent<RectTransform>();
					this._tabRectTransformList.Add(component);
					this._tabGuiList.Add(tabGui);
				}
				this._txtTabInfo = baseTr.Find("TabbtnBase/Txt_TabInfo").GetComponent<PguiTextCtrl>();
				this._buttonL = baseTr.Find("ButtonL").GetComponent<PguiButtonCtrl>();
				this._buttonLText = this._buttonL.transform.Find("BaseImage/Text").GetComponent<PguiTextCtrl>();
				this._buttonR = baseTr.Find("ButtonR").GetComponent<PguiButtonCtrl>();
				this._buttonRText = this._buttonR.transform.Find("BaseImage/Text").GetComponent<PguiTextCtrl>();
				this._buttonRExchange = baseTr.Find("ButtonR_Exchange").GetComponent<PguiButtonCtrl>();
				this._buttonRExchangeIconItem = this._buttonRExchange.transform.Find("BaseImage/Inbase/Icon_Item").GetComponent<PguiRawImageCtrl>();
				this._buttonRExchangeNumUse = this._buttonRExchange.transform.Find("BaseImage/Inbase/Num_Use").GetComponent<PguiTextCtrl>();
				this._haveGoldText = baseTr.Find("ItemOwn/Num").GetComponent<PguiTextCtrl>();
				this._needGoldText = baseTr.Find("ItemUse/Num").GetComponent<PguiTextCtrl>();
			}

			private string GetItemName(int idx)
			{
				string text = "Texture2D/Icon_Item/icon_item_{0}";
				string text2 = string.Empty;
				switch (idx)
				{
				case 0:
					text2 = string.Format(text, 13003);
					break;
				case 1:
					text2 = string.Format(text, 14020);
					break;
				case 2:
					text2 = string.Format(text, 30119);
					break;
				case 3:
					text2 = string.Format(text, 12020);
					break;
				case 4:
					text2 = string.Format(text, 13120);
					break;
				case 5:
					text2 = string.Format(text, 30104);
					break;
				case 6:
					text2 = string.Format(text, 17101);
					break;
				}
				return text2;
			}

			private PguiTabGroupCtrl _tabGroup;

			private List<SelCharaGrowCtrl.CommonGUI.CmnTabGUI.TabGui> _tabGuiList;

			private List<RectTransform> _tabRectTransformList;

			private PguiTextCtrl _txtTabInfo;

			private PguiButtonCtrl _buttonL;

			private PguiTextCtrl _buttonLText;

			private PguiButtonCtrl _buttonR;

			private PguiTextCtrl _buttonRText;

			private PguiButtonCtrl _buttonRExchange;

			private PguiRawImageCtrl _buttonRExchangeIconItem;

			private PguiTextCtrl _buttonRExchangeNumUse;

			private PguiTextCtrl _needGoldText;

			private PguiTextCtrl _haveGoldText;

			public class TabGui
			{
				public PguiTabCtrl TabCtrl
				{
					get
					{
						return this._tabCtrl;
					}
				}

				public GameObject YellowBadge
				{
					get
					{
						return this._yellowBadge;
					}
				}

				public PguiAECtrl YellowBadgeAnim
				{
					get
					{
						return this._yellowBadgeAnim;
					}
				}

				public PguiRawImageCtrl IconItem
				{
					get
					{
						return this._iconItem;
					}
				}

				public GameObject Text
				{
					get
					{
						return this._text;
					}
				}

				public TabGui(Transform baseTr, int i, string itemName)
				{
					this._tabCtrl = baseTr.Find("TabGroup/Tab" + (i + 1).ToString("D2")).GetComponent<PguiTabCtrl>();
					this._yellowBadge = this._tabCtrl.transform.Find("BaseImage/Mark_YellowBadge").gameObject;
					this._yellowBadgeAnim = this.YellowBadge.GetComponent<PguiAECtrl>();
					this._iconItem = this._tabCtrl.transform.Find("BaseImage/Icon_Item").gameObject.GetComponent<PguiRawImageCtrl>();
					this._iconItem.SetRawImage(itemName, true, false, null);
					this._text = this._tabCtrl.transform.Find("BaseImage/Txt").gameObject;
				}

				private PguiTabCtrl _tabCtrl;

				private GameObject _yellowBadge;

				private PguiAECtrl _yellowBadgeAnim;

				private PguiRawImageCtrl _iconItem;

				private GameObject _text;
			}
		}

		public class ItemListBar
		{
			public GameObject BaseObj
			{
				get
				{
					return this._baseObj;
				}
			}

			public PguiImageCtrl ImgAtr
			{
				get
				{
					return this._imgAtr;
				}
			}

			public PguiTextCtrl TxtExpBonus
			{
				get
				{
					return this._txtExpBonus;
				}
			}

			public PguiTextCtrl TxtAll
			{
				get
				{
					return this._txtAll;
				}
			}

			public GridLayoutGroup Grid
			{
				get
				{
					return this._grid;
				}
			}

			public List<SelCharaGrowLevel.LvUpItem> IconItemList
			{
				get
				{
					return this._iconItemList;
				}
			}

			public List<SelCharaGrowKizuna.KizunaLvUpItem> IconItemListKizuna
			{
				get
				{
					return this._iconItemListKizuna;
				}
			}

			public ItemListBar(Transform baseTr)
			{
				this._baseObj = baseTr.gameObject;
				this._imgAtr = baseTr.Find("Img_Atr").GetComponent<PguiImageCtrl>();
				this._txtExpBonus = baseTr.Find("Txt_ExpBonus").GetComponent<PguiTextCtrl>();
				this._txtAll = baseTr.Find("Txt_All").GetComponent<PguiTextCtrl>();
				this._grid = baseTr.Find("Grid").GetComponent<GridLayoutGroup>();
				this._iconItemList = new List<SelCharaGrowLevel.LvUpItem>();
				this._iconItemListKizuna = new List<SelCharaGrowKizuna.KizunaLvUpItem>();
			}

			private GameObject _baseObj;

			private PguiImageCtrl _imgAtr;

			private PguiTextCtrl _txtExpBonus;

			private PguiTextCtrl _txtAll;

			private GridLayoutGroup _grid;

			private List<SelCharaGrowLevel.LvUpItem> _iconItemList;

			private List<SelCharaGrowKizuna.KizunaLvUpItem> _iconItemListKizuna;
		}

		public class ItemConversion
		{
			public PguiTextCtrl Title
			{
				get
				{
					return this._title;
				}
			}

			public Dictionary<SelCharaGrowCtrl.CommonGUI.ItemConversion.ItemPosition, SelCharaGrowCtrl.CommonGUI.ItemConversion.ItemBox> ItemBoxeMap
			{
				get
				{
					return this._itemBoxeMap;
				}
			}

			public PguiButtonCtrl ButtonC
			{
				get
				{
					return this._buttonC;
				}
			}

			public PguiTextCtrl TxtRateInfo
			{
				get
				{
					return this._txtRateInfo;
				}
			}

			public ItemConversion(Transform baseTr)
			{
				this._title = baseTr.Find("Title").GetComponent<PguiTextCtrl>();
				this._buttonC = baseTr.Find("ButtonC").GetComponent<PguiButtonCtrl>();
				this._txtRateInfo = baseTr.Find("Txt_RateInfo").GetComponent<PguiTextCtrl>();
				this._itemBoxeMap = new Dictionary<SelCharaGrowCtrl.CommonGUI.ItemConversion.ItemPosition, SelCharaGrowCtrl.CommonGUI.ItemConversion.ItemBox>
				{
					{
						SelCharaGrowCtrl.CommonGUI.ItemConversion.ItemPosition.Left,
						new SelCharaGrowCtrl.CommonGUI.ItemConversion.ItemBox(baseTr.Find("Left"))
					},
					{
						SelCharaGrowCtrl.CommonGUI.ItemConversion.ItemPosition.Right,
						new SelCharaGrowCtrl.CommonGUI.ItemConversion.ItemBox(baseTr.Find("Right"))
					}
				};
			}

			private PguiTextCtrl _title;

			private Dictionary<SelCharaGrowCtrl.CommonGUI.ItemConversion.ItemPosition, SelCharaGrowCtrl.CommonGUI.ItemConversion.ItemBox> _itemBoxeMap;

			private PguiButtonCtrl _buttonC;

			private PguiTextCtrl _txtRateInfo;

			public enum ItemPosition
			{
				Left,
				Right
			}

			public class ItemBox
			{
				public ItemBox(Transform baseTr)
				{
					this._name = baseTr.Find("Txt_ItemBox").GetComponent<PguiTextCtrl>();
					this._num = baseTr.Find("NumInfo/Num").GetComponent<PguiTextCtrl>();
					this._txt = baseTr.Find("NumInfo/Txt").GetComponent<PguiTextCtrl>();
					this._icon = baseTr.Find("ItemIcon").gameObject;
					this._itemIconCtrl = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, this._icon.transform).GetComponent<IconItemCtrl>();
					if (this._icon.transform.Find("Icon_Item") != null)
					{
						this._icon.transform.Find("Icon_Item").gameObject.SetActive(false);
					}
				}

				public void Setup(SelCharaGrowCtrl.CommonGUI.ItemConversion.ItemBox.SetupParam param)
				{
					this._itemIconCtrl.Setup(param.ItemStaticBase);
					this._name.text = param.ItemData.staticData.GetName();
					this._num.text = string.Format("{0}", param.Num);
					this._txt.text = param.InfoText;
				}

				private GameObject _icon;

				private IconItemCtrl _itemIconCtrl;

				private PguiTextCtrl _name;

				private PguiTextCtrl _num;

				private PguiTextCtrl _txt;

				public class SetupParam
				{
					public ItemData ItemData
					{
						get
						{
							return this._itemData;
						}
					}

					public string InfoText
					{
						get
						{
							return this._infoText;
						}
					}

					public int Num
					{
						get
						{
							return this._num;
						}
					}

					public ItemStaticBase ItemStaticBase
					{
						get
						{
							return this._itemStaticBase;
						}
					}

					public SetupParam(ItemData itemData, string infoText, int num, ItemStaticBase itemStaticBase)
					{
						this._itemData = itemData;
						this._infoText = infoText;
						this._num = num;
						this._itemStaticBase = itemStaticBase;
					}

					private ItemData _itemData;

					private string _infoText;

					private int _num;

					private ItemStaticBase _itemStaticBase;
				}
			}
		}

		public class PhotoMax
		{
			public GameObject BaseObj
			{
				get
				{
					return this._baseObj;
				}
			}

			public PguiTextCtrl TxtMaxInfo
			{
				get
				{
					return this._txtMaxInfo;
				}
			}

			public SelCharaGrowCtrl.CommonGUI.ItemConversion ItemConversion
			{
				get
				{
					return this._itemConversion;
				}
			}

			public PhotoMax(Transform baseTr)
			{
				this._baseObj = baseTr.gameObject;
				this._itemConversion = new SelCharaGrowCtrl.CommonGUI.ItemConversion(baseTr.Find("Box"));
				this._txtMaxInfo = baseTr.Find("Txt_MaxInfo_Photo").GetComponent<PguiTextCtrl>();
			}

			private GameObject _baseObj;

			private PguiTextCtrl _txtMaxInfo;

			private SelCharaGrowCtrl.CommonGUI.ItemConversion _itemConversion;
		}

		public class PhotoPocketConversionStrengthen
		{
			public GameObject BaseObj
			{
				get
				{
					return this._baseObj;
				}
			}

			public SelCharaGrowCtrl.CommonGUI.ItemConversion ItemConversion
			{
				get
				{
					return this._itemConversion;
				}
			}

			public PhotoPocketConversionStrengthen(Transform baseTr)
			{
				this._baseObj = baseTr.gameObject;
				this._itemConversion = new SelCharaGrowCtrl.CommonGUI.ItemConversion(baseTr.Find("Box"));
			}

			private GameObject _baseObj;

			private SelCharaGrowCtrl.CommonGUI.ItemConversion _itemConversion;
		}
	}

	private enum ListKind
	{
		Quest,
		Shop
	}

	private struct ListBarKind
	{
		public ListBarKind(SelCharaGrowCtrl.ListKind kind, int id)
		{
			this.kind = kind;
			this.id = id;
		}

		public SelCharaGrowCtrl.ListKind kind;

		public int id;
	}

	public class IconItemPack
	{
		public void Clear()
		{
			if (this.iconItemCtrl != null)
			{
				this.iconItemCtrl.Clear();
			}
			if (this.Txt_Num != null)
			{
				this.Txt_Num.text = "";
			}
		}

		public IconItemCtrl iconItemCtrl;

		public PguiTextCtrl Txt_Num;
	}

	public class SingleItemInfoWindow
	{
		public SingleItemInfoWindow(Transform baseTr)
		{
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			Transform transform = baseTr.Find("Base/Window/ItemInfo/Icon_Item");
			GameObject gameObject = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, transform);
			this.iconItemPack.iconItemCtrl = gameObject.GetComponent<IconItemCtrl>();
			this.iconItemPack.Txt_Num = transform.Find("Txt_Num").GetComponent<PguiTextCtrl>();
			this.ScrollView_QuestAll = baseTr.Find("Base/Window/Right/QuestInfo/ScrollView_QuestAll").GetComponent<ReuseScroll>();
			this.Txt_NoneInfo = baseTr.Find("Base/Window/Right/QuestInfo/Txt_NoneInfo").GetComponent<PguiTextCtrl>();
			transform = baseTr.Find("Base/Window/Right/Exchange/Source_Item");
			gameObject = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, transform);
			this.sourceIconItemPack.iconItemCtrl = gameObject.GetComponent<IconItemCtrl>();
			this.sourceIconItemPack.Txt_Num = transform.Find("Txt_Num").GetComponent<PguiTextCtrl>();
			transform = baseTr.Find("Base/Window/Right/Exchange/Target_Item");
			gameObject = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, transform);
			this.targetIconItemPack.iconItemCtrl = gameObject.GetComponent<IconItemCtrl>();
			this.targetIconItemPack.Txt_Num = transform.Find("Txt_Num").GetComponent<PguiTextCtrl>();
			this.exchangeButton = baseTr.Find("Base/Window/Right/Exchange/ButtonR").GetComponent<PguiButtonCtrl>();
			this.exchangeButton.SetActEnable(false, false, false);
			this.remainExecuteText = baseTr.Find("Base/Window/Right/Exchange/Txt_BuyInfo02").GetComponent<PguiTextCtrl>();
			this.exchangeBase = transform.parent.gameObject;
			this.questInfo = baseTr.Find("Base/Window/Right/QuestInfo").GetComponent<RectTransform>();
		}

		public PguiOpenWindowCtrl owCtrl;

		public ReuseScroll ScrollView_QuestAll;

		public PguiTextCtrl Txt_NoneInfo;

		public SelCharaGrowCtrl.IconItemPack iconItemPack = new SelCharaGrowCtrl.IconItemPack();

		public SelCharaGrowCtrl.IconItemPack sourceIconItemPack = new SelCharaGrowCtrl.IconItemPack();

		public SelCharaGrowCtrl.IconItemPack targetIconItemPack = new SelCharaGrowCtrl.IconItemPack();

		public GameObject exchangeBase;

		public RectTransform questInfo;

		public PguiButtonCtrl exchangeButton;

		public PguiTextCtrl remainExecuteText;
	}

	public class MultipleItemInfoWindow
	{
		public MultipleItemInfoWindow(Transform baseTr)
		{
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			for (int i = 0; i < SelCharaGrowCtrl.MultipleItemInfoWindow.COUNT; i++)
			{
				Transform transform = baseTr.Find("Base/Window/ItemInfo/Icon_ItemAll/Icon_Item" + (i + 1).ToString("D2"));
				GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("CmnIconFrame/GUI/Prefab/Icon_Item"), transform);
				SelCharaGrowCtrl.IconItemPack iconItemPack = new SelCharaGrowCtrl.IconItemPack
				{
					iconItemCtrl = gameObject.GetComponent<IconItemCtrl>(),
					Txt_Num = transform.Find("Txt_Num").GetComponent<PguiTextCtrl>()
				};
				this.IconItemCtrlList.Add(iconItemPack);
				this.ScrollViewAllList.Add(baseTr.Find("Base/Window/Right/QuestInfo/ScrollViewAll_" + (i + 1).ToString("D2") + "/ScrollView_QuestAll").GetComponent<ReuseScroll>());
				this.ScrollViewList.Add(baseTr.Find("Base/Window/Right/QuestInfo/ScrollViewAll_" + (i + 1).ToString("D2")));
			}
			this.Current_Txt_ItemName = baseTr.Find("Base/Window/ItemInfo/CurrentItemInfo/Txt_ItemName").GetComponent<PguiTextCtrl>();
			this.Current_Frame = baseTr.Find("Base/Window/ItemInfo/Icon_ItemAll/CurrentItemFrame").gameObject;
			this.Txt_NoneInfo = baseTr.Find("Base/Window/Right/QuestInfo/Txt_NoneInfo").GetComponent<PguiTextCtrl>();
			Transform transform2 = baseTr.Find("Base/Window/Right/Exchange/Source_Item");
			GameObject gameObject2 = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, transform2);
			this.sourceIconItemPack.iconItemCtrl = gameObject2.GetComponent<IconItemCtrl>();
			this.sourceIconItemPack.Txt_Num = transform2.Find("Txt_Num").GetComponent<PguiTextCtrl>();
			transform2 = baseTr.Find("Base/Window/Right/Exchange/Target_Item");
			gameObject2 = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, transform2);
			this.targetIconItemPack.iconItemCtrl = gameObject2.GetComponent<IconItemCtrl>();
			this.targetIconItemPack.Txt_Num = transform2.Find("Txt_Num").GetComponent<PguiTextCtrl>();
			this.exchangeButton = baseTr.Find("Base/Window/Right/Exchange/ButtonR").GetComponent<PguiButtonCtrl>();
			this.exchangeButton.SetActEnable(false, false, false);
			this.remainExecuteText = baseTr.Find("Base/Window/Right/Exchange/Txt_BuyInfo02").GetComponent<PguiTextCtrl>();
			this.exchangeBase = transform2.parent.gameObject;
			this.questInfo = baseTr.Find("Base/Window/Right/QuestInfo").GetComponent<RectTransform>();
		}

		public static readonly int COUNT = 8;

		public PguiOpenWindowCtrl owCtrl;

		public List<SelCharaGrowCtrl.IconItemPack> IconItemCtrlList = new List<SelCharaGrowCtrl.IconItemPack>();

		public List<ReuseScroll> ScrollViewAllList = new List<ReuseScroll>();

		public List<Transform> ScrollViewList = new List<Transform>();

		public PguiTextCtrl Current_Txt_ItemName;

		public PguiTextCtrl Txt_NoneInfo;

		public GameObject Current_Frame;

		public SelCharaGrowCtrl.IconItemPack sourceIconItemPack = new SelCharaGrowCtrl.IconItemPack();

		public SelCharaGrowCtrl.IconItemPack targetIconItemPack = new SelCharaGrowCtrl.IconItemPack();

		public GameObject exchangeBase;

		public RectTransform questInfo;

		public PguiButtonCtrl exchangeButton;

		public PguiTextCtrl remainExecuteText;
	}

	public class GuiCharaGrowListBar
	{
		public GuiCharaGrowListBar(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.listBarQuest = new SelCharaGrowCtrl.GuiCharaGrowListBarQuest(this.baseObj.transform.Find("CharaGrow_ListBar_Quest"));
			this.listBarShop = new SelCharaGrowCtrl.GuiCharaGrowListBarShop(this.baseObj.transform.Find("CharaGrow_ListBar_Shop"));
			this.listBarQuest.CharaGrow_ListBar_Quest.gameObject.AddComponent<PguiDataHolder>();
			this.listBarShop.CharaGrow_ListBar_Shop.gameObject.AddComponent<PguiDataHolder>();
		}

		public void Setup(QuestOnePackData questOnePack)
		{
			this.listBarQuest.CharaGrow_ListBar_Quest.gameObject.SetActive(true);
			this.listBarQuest.Setup(questOnePack);
			this.listBarShop.CharaGrow_ListBar_Shop.gameObject.SetActive(false);
		}

		public void Setup(ShopData shopData, int itemId)
		{
			this.listBarShop.CharaGrow_ListBar_Shop.gameObject.SetActive(true);
			this.listBarShop.Setup(shopData, itemId);
			this.listBarQuest.CharaGrow_ListBar_Quest.gameObject.SetActive(false);
		}

		public void AddParam(int id, string str)
		{
			this.listBarQuest.CharaGrow_ListBar_Quest.GetComponent<PguiDataHolder>().id = id;
			this.listBarQuest.CharaGrow_ListBar_Quest.GetComponent<PguiDataHolder>().str = str;
			this.listBarShop.CharaGrow_ListBar_Shop.GetComponent<PguiDataHolder>().id = id;
			this.listBarShop.CharaGrow_ListBar_Shop.GetComponent<PguiDataHolder>().str = str;
		}

		public void AddOnClickListener(PguiButtonCtrl.OnClick button)
		{
			this.listBarQuest.CharaGrow_ListBar_Quest.AddOnClickListener(button, PguiButtonCtrl.SoundType.DEFAULT);
			this.listBarShop.CharaGrow_ListBar_Shop.AddOnClickListener(button, PguiButtonCtrl.SoundType.DEFAULT);
		}

		public GameObject baseObj;

		private SelCharaGrowCtrl.GuiCharaGrowListBarQuest listBarQuest;

		private SelCharaGrowCtrl.GuiCharaGrowListBarShop listBarShop;
	}

	public class GuiCharaGrowListBarQuest
	{
		public GuiCharaGrowListBarQuest(Transform baseTr)
		{
			this.CharaGrow_ListBar_Quest = baseTr.GetComponent<PguiButtonCtrl>();
			this.QuestKind = baseTr.Find("BaseImage/QuestKindAll/QuestKind").GetComponent<PguiImageCtrl>();
			this.Txt_QuestKind = baseTr.Find("BaseImage/QuestKindAll/QuestKind/Txt_Kind").GetComponent<PguiTextCtrl>();
			this.QuestKind_Event = baseTr.Find("BaseImage/QuestKindAll/QuestKind_Event").GetComponent<PguiImageCtrl>();
			this.Txt_QuestKind_Event = baseTr.Find("BaseImage/QuestKindAll/QuestKind_Event/Txt_Kind").GetComponent<PguiTextCtrl>();
			this.ModeKind = baseTr.Find("BaseImage/QuestKindAll/ModeKind").GetComponent<PguiImageCtrl>();
			this.Txt_ModeKind = baseTr.Find("BaseImage/QuestKindAll/ModeKind/Txt_Kind").GetComponent<PguiTextCtrl>();
			this.Txt_QuestNameSub = baseTr.Find("BaseImage/Txt_ChapterTitle").GetComponent<PguiTextCtrl>();
			this.questListBarCmnInfo = new QuestUtil.QuestListBarCmnInfo(baseTr.Find("BaseImage/QuestListBar_CmnInfo"));
		}

		public void Setup(QuestOnePackData questOnePack)
		{
			bool flag = DataManager.DmQuest.QuestDynamicData.oneDataMap.ContainsKey(questOnePack.questOne.questId);
			DataManager.DmQuest.GetBattleMissionPack(questOnePack.questOne.questId);
			bool flag2 = questOnePack.questDynamicOne.status == QuestOneStatus.COMPLETE || questOnePack.questDynamicOne.status == QuestOneStatus.CLEAR;
			int num = DataManager.DmQuest.CalcRestPlayNumByQuestOneId(questOnePack.questOne.questId);
			ItemInput recoveryKeyItem = questOnePack.questOne.RecoveryKeyItem;
			QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(questOnePack.questOne.questId);
			bool flag3 = num == 0 && (recoveryKeyItem == null || questOnePackData.questDynamicOne.todayRecoveryNum >= questOnePack.questOne.RecoveryMaxNum);
			int num2 = DataManager.DmQuest.CalcRestPlayNumByQuestGroupId(questOnePack.questGroup.questGroupId);
			this.questListBarCmnInfo.Setup(new QuestUtil.QuestListBarCmnInfo.SetupParam
			{
				qsqo = questOnePack.questOne,
				selectData = new QuestUtil.SelectData
				{
					chapterId = questOnePack.questChapter.chapterId,
					questCategory = questOnePack.questChapter.category
				},
				questOneStatus = questOnePack.questDynamicOne.status,
				restGroupNum = num2,
				enableChangeColor = false
			});
			string text;
			if (flag || (!SceneQuest.IsMainStory(questOnePack.questChapter.category) && questOnePack.questChapter.category != QuestStaticChapter.Category.SIDE_STORY))
			{
				string questName = questOnePack.questOne.questName;
				text = string.Concat(new string[]
				{
					questOnePack.questChapter.chapterName,
					questOnePack.questGroup.titleName,
					"「",
					questOnePack.questGroup.storyName,
					"」"
				});
				questOnePack.questOne.difficulty.ToString();
				questOnePack.questOne.stamina.ToString();
				this.questListBarCmnInfo.SetActiveIconPresentBox(!flag);
			}
			else
			{
				PrjUtil.MakeMessage("？？？？？？");
				text = questOnePack.questChapter.chapterName + questOnePack.questGroup.titleName + PrjUtil.MakeMessage("「？？？？？？」");
				PrjUtil.MakeMessage("？？");
				PrjUtil.MakeMessage("？？");
				this.questListBarCmnInfo.SetActiveIconItem(false);
				this.questListBarCmnInfo.SetActiveIconPresentBox(true);
			}
			this.Txt_QuestNameSub.text = text;
			string text2 = "MAIN";
			string text3 = QuestUtil.TitleMain;
			if (questOnePack.questChapter.category == QuestStaticChapter.Category.EVENT)
			{
				this.QuestKind.gameObject.SetActive(false);
				this.QuestKind_Event.gameObject.SetActive(true);
				DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventDataList().Find((DataManagerEvent.EventData x) => x.eventChapterId == questOnePack.questChapter.chapterId);
				if (eventData != null)
				{
					this.Txt_QuestKind_Event.text = eventData.eventName;
				}
			}
			else
			{
				this.QuestKind.gameObject.SetActive(true);
				this.QuestKind_Event.gameObject.SetActive(false);
			}
			switch (questOnePack.questChapter.category)
			{
			case QuestStaticChapter.Category.STORY:
				text3 = QuestUtil.TitleMain;
				text2 = "MAIN";
				break;
			case QuestStaticChapter.Category.GROW:
				text3 = QuestUtil.TitleGrow;
				text2 = "GROW";
				break;
			case QuestStaticChapter.Category.CHARA:
				text3 = QuestUtil.TitleFriends;
				text2 = "CHARA";
				break;
			case QuestStaticChapter.Category.EVENT:
				text3 = QuestUtil.TitleEvent;
				text2 = "MAIN";
				break;
			case QuestStaticChapter.Category.SIDE_STORY:
				text3 = QuestUtil.TitleArai;
				text2 = "ARAI";
				break;
			case QuestStaticChapter.Category.CELLVAL:
				text3 = QuestUtil.TitleCellval;
				text2 = "CELLVALL";
				break;
			case QuestStaticChapter.Category.ETCETERA:
				text3 = QuestUtil.TitleEtcetera;
				text2 = "ETCETERA";
				break;
			case QuestStaticChapter.Category.STORY2:
				text3 = QuestUtil.TitleMain2;
				text2 = "MAIN02";
				break;
			case QuestStaticChapter.Category.STORY3:
				text3 = QuestUtil.TitleMain3;
				text2 = "MAIN03";
				break;
			}
			this.Txt_QuestKind.text = text3;
			this.QuestKind.ReplaceColorByPguiColorCtrl(text2);
			PguiDataHolder component = this.QuestKind.m_Image.gameObject.GetComponent<PguiDataHolder>();
			if (component != null)
			{
				component.color = this.QuestKind.m_Image.color;
			}
			this.ModeKind.gameObject.SetActive(QuestUtil.IsHardMode(questOnePack.questChapter.chapterId));
			this.Txt_ModeKind.text = "ハード";
			this.CharaGrow_ListBar_Quest.ReloadChildObject();
			if (num < 0)
			{
				this.CharaGrow_ListBar_Quest.SetActEnable(flag2, true, false);
				return;
			}
			this.CharaGrow_ListBar_Quest.SetActEnable(flag2 && !flag3, true, false);
		}

		public PguiButtonCtrl CharaGrow_ListBar_Quest;

		public PguiImageCtrl QuestKind;

		public PguiTextCtrl Txt_QuestKind;

		public PguiImageCtrl QuestKind_Event;

		public PguiTextCtrl Txt_QuestKind_Event;

		public PguiImageCtrl ModeKind;

		public PguiTextCtrl Txt_ModeKind;

		public PguiTextCtrl Txt_QuestNameSub;

		private QuestUtil.QuestListBarCmnInfo questListBarCmnInfo;
	}

	public class GuiCharaGrowListBarShop
	{
		public GuiCharaGrowListBarShop(Transform baseTr)
		{
			this.CharaGrow_ListBar_Shop = baseTr.GetComponent<PguiButtonCtrl>();
			this.Txt_QuestName = baseTr.Find("BaseImage/QuestImfo/Txt_QuestName").GetComponent<PguiTextCtrl>();
			this.Txt_EventName = baseTr.Find("BaseImage/QuestImfo/Txt_QuestName/Txt_EventName").GetComponent<PguiTextCtrl>();
			this.Icon_Tex = baseTr.Find("BaseImage/ItemNumInfo/Icon_Tex").GetComponent<PguiRawImageCtrl>();
			this.Item_Num = baseTr.Find("BaseImage/ItemNumInfo/Num").GetComponent<PguiTextCtrl>();
			this.Disable = baseTr.Find("BaseImage/Disable").gameObject;
			this.repSprite = baseTr.Find("BaseImage").GetComponent<PguiReplaceSpriteCtrl>();
		}

		public void Setup(ShopData shopData, int itemId)
		{
			this.Txt_QuestName.text = shopData.shopName;
			DataManagerEvent.EventData eventData = DataManager.DmEvent.GetEventDataList().Find((DataManagerEvent.EventData x) => x.eventShopIdList.Count > 0 && x.eventShopIdList[0] == shopData.shopId);
			this.Txt_EventName.text = ((eventData != null) ? eventData.eventName : "");
			ItemData userItemData = DataManager.DmItem.GetUserItemData(shopData.priceItemId);
			this.Icon_Tex.SetRawImage(userItemData.staticData.GetIconName(), true, false, null);
			this.Item_Num.text = userItemData.num.ToString();
			ShopData.ItemOne itemOne = shopData.oneDataList.Find((ShopData.ItemOne x) => x.itemId == itemId);
			this.Disable.gameObject.SetActive(itemOne.maxChangeNum > 0 && itemOne.nowChangeNum >= itemOne.maxChangeNum);
			this.repSprite.Replace((shopData.category == ShopData.Category.OTHER_NOITEM_HIDE) ? 1 : 0);
		}

		public PguiButtonCtrl CharaGrow_ListBar_Shop;

		public PguiTextCtrl Txt_QuestName;

		public PguiTextCtrl Txt_EventName;

		public PguiRawImageCtrl Icon_Tex;

		public PguiTextCtrl Item_Num;

		public GameObject Disable;

		public PguiReplaceSpriteCtrl repSprite;
	}

	private class OpenItemWindowParam
	{
		public List<ItemData> itemList = new List<ItemData>();

		public bool isMulti;

		public int index;
	}
}
