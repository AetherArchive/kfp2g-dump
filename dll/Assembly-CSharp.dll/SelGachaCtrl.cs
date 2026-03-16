using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SGNFW.Common;
using SGNFW.Http;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelGachaCtrl : MonoBehaviour
{
	private DateTime GachaPackDataLastUpdateTime { get; set; }

	private bool IsTutorial
	{
		get
		{
			return this.sceneOpenParam != null && this.sceneOpenParam.tutorialSequence > TutorialUtil.Sequence.INVALID;
		}
	}

	public bool RenderCharaLBFinishedSetup
	{
		get
		{
			return this.RenderCharaLB.FinishedSetup;
		}
	}

	private bool IsDispGuiResult
	{
		get
		{
			return this.guiDataResult != null && this.guiDataResult.baseObj.activeSelf;
		}
	}

	public void Init()
	{
		GameObject gameObject = Object.Instantiate<GameObject>((GameObject)AssetManager.GetAssetData("SceneGacha/GUI/Prefab/GUI_GachaTop"), base.transform);
		this.guiDataTop = new SelGachaCtrl.GachaTopGUI(gameObject.transform);
		this.guiDataTop.InfoDispWName.gameObject.SetActive(false);
		this.CenterInfoDisplayedBanner = true;
		this.guiDataTop.GachaBtnSet_Left.BaseButton.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickGachaTopBtnLeft), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiDataTop.GachaBtnSet_Right.BaseButton.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickGachaTopBtnRight), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiDataTop.Btn_Reload.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickGachaTopReloadBtn), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiDataTop.TouchPanel.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickGachaTopTouchPanel), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiDataTop.Btn_GachaDetailInfo.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickGachaTopGachaDetailInfoBtn), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiDataTop.Btn_CharaInfo.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickGachaTopCharaInfoBtn), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiDataTop.Btn_PhotoInfo.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickGachaTopPhotoInfoBtn), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiDataTop.Btn_BoxReset.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickBoxResetButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiDataTop.BoxCheckBtn.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickBoxCheckButtonTop), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiDataTop.GachaUseItemBtnList[0].button.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickGachaTopUseItemBtnTop), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiDataTop.GachaUseItemBtnList[1].button.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickGachaTopUseItemBtnMiddle), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiDataTop.GachaUseItemBtnList[2].button.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickGachaTopUseItemBtnBottom), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiDataTop.Btn_FurnitureInfo.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickGachaTopFurnitureInfoBtn), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiDataTop.Chara_MovieImage.gameObject.AddComponent<MoviePlayer>();
		this.guiDataTop.Furniture_MovieImage.gameObject.AddComponent<MoviePlayer>();
		CustomScrollRect gachaBannerScroll = this.guiDataTop.GachaBannerScroll;
		gachaBannerScroll.onStartItem = (Action<int, GameObject>)Delegate.Combine(gachaBannerScroll.onStartItem, new Action<int, GameObject>(this.OnStartItem));
		CustomScrollRect gachaBannerScroll2 = this.guiDataTop.GachaBannerScroll;
		gachaBannerScroll2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(gachaBannerScroll2.onUpdateItem, new Action<int, GameObject>(this.OnUpdateItem));
		CustomScrollRect gachaBannerScroll3 = this.guiDataTop.GachaBannerScroll;
		gachaBannerScroll3.onChangeFocusItem = (Action<int, GameObject>)Delegate.Combine(gachaBannerScroll3.onChangeFocusItem, new Action<int, GameObject>(this.OnChangeFocusItem));
		this.gachaPackDataList = new List<DataManagerGacha.GachaPackData>();
		this.resultIconObjList = new List<GameObject>();
		this.centerInfoDispIdList = new List<int>();
		this.centerInfoDispIndex = 0;
		this.lastSelectGacha = new SelGachaCtrl.LastSelectGacha();
		this.guiDataTop.PhotoCard.Setup(PhotoPackData.MakeDummy(1L, 2001), SortFilterDefine.SortType.LEVEL, true, false, -1, false);
		this.gachaWindowInfo = CanvasManager.HdlGachaWindowInfoCtrl;
		this.gachaBannerImgMap = new Dictionary<int, PguiRawImageCtrl>();
		this.guiDataTop.TabGroup.Setup(0, new PguiTabGroupCtrl.OnSelectTab(this.OnSelectGachaTab));
		this.OnSelectGachaTab(0);
	}

	public void Setup(SceneGacha.OpenParam args = null)
	{
		this.sceneOpenParam = args;
		this.guiDataTop.baseObj.SetActive(true);
		this.SetDisplayCmnMenu(true);
		GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("RenderTextureChara/Prefab/RenderTextureCharaCtrl"), this.guiDataTop.baseObj.transform);
		this.RenderCharaCenter = gameObject.GetComponent<RenderTextureChara>();
		this.RenderCharaCenter.postion = new Vector2(100f, -100f);
		this.RenderCharaCenter.fieldOfView = 26f;
		this.RenderCharaCenter.transform.SetSiblingIndex(this.RENDER_TEXTURE_CHARA_TRANSFORM_INDEX);
		this.RenderCharaCenter.Setup(1, 0, CharaMotionDefine.ActKey.GACHA_LP, 0, false, true, null, false, null, 0f, null, false, false, false);
		this.RenderCharaCenter.gameObject.SetActive(true);
		GameObject gameObject2 = Object.Instantiate<GameObject>((GameObject)Resources.Load("RenderTextureChara/Prefab/RenderTextureCharaCtrl"), this.guiDataTop.baseObj.transform);
		this.RenderCharaLB = gameObject2.GetComponent<RenderTextureChara>();
		this.RenderCharaLB.postion = new Vector2(0f, 250f);
		this.RenderCharaLB.rotation = new Vector3(0f, -60f, 0f);
		this.RenderCharaLB.fieldOfView = 30f;
		this.RenderCharaLB.Setup(1004, 1, CharaMotionDefine.ActKey.GACHA_LP, 1, false, true, null, false, null, 0f, null, false, false, false);
		this.RenderCharaLB.gameObject.SetActive(true);
		this.RenderCharaLB.DispLuckyEyeEffect(true, 4);
		this.RenderCharaLB.transform.SetParent(this.guiDataTop.RenderTexture_LB.transform, false);
		this.RenderCharaLB.transform.localPosition = new Vector3(0f, 0f, 0f);
		this.currentBannerBtnIndex = 0;
		this.currentGachaPackData = null;
		this.guiDataResult = null;
		this.pauseCenterInfoDispLoop = false;
		CanvasManager.HdlSelPurchaseStoneWindowCtrl.AddOnSuccessPurchaseListener(new UnityAction(this.OnSuccessPurcheseStone));
		CanvasManager.HdlSelMonthlyPackWindowCtrl.AddOnSuccessPurchaseListener(new UnityAction(this.OnSuccessPurcheseMonthlyPack));
	}

	public void SetupGachaData()
	{
		int num = ((this.sceneOpenParam != null) ? this.sceneOpenParam.gachaId : 0);
		this.guiDataTop.TabGroup.SelectTab(0);
		this.RefreshGachaPackDataList(num, true, true, 0);
		if (this.IsTutorial)
		{
			Singleton<SceneManager>.Instance.StartCoroutine(this.Tutorial());
		}
	}

	public void UpdateSel()
	{
		SelGachaCtrl.GachaTopGUI gachaTopGUI = this.guiDataTop;
		if (((gachaTopGUI != null) ? gachaTopGUI.StartPopupPlay : null) != null && !this.guiDataTop.StartPopupPlay.MoveNext())
		{
			this.guiDataTop.StartPopupPlay = null;
		}
		SelGachaCtrl.GachaTopGUI gachaTopGUI2 = this.guiDataTop;
		if (((gachaTopGUI2 != null) ? gachaTopGUI2.RefreshGachaTop : null) != null && !this.guiDataTop.RefreshGachaTop.MoveNext())
		{
			this.guiDataTop.RefreshGachaTop = null;
		}
		if (!this.pauseCenterInfoDispLoop && !this.CenterInfoDisplayedBanner)
		{
			this.centerInfoDispTimeElapsed += Time.deltaTime;
			this.movieTime += Time.deltaTime;
			ItemDef.Kind kind = ItemDef.Id2Kind(this.GetCenterInfoDispItemId());
			if (kind != ItemDef.Kind.CHARA)
			{
				if (kind != ItemDef.Kind.PHOTO)
				{
					if (kind != ItemDef.Kind.TREEHOUSE_FURNITURE)
					{
						return;
					}
					if (this.guiDataTop.Furniture_MovieImage.gameObject.activeSelf)
					{
						if (this.guiDataTop.Furniture_MovieImage.m_RawImage.raycastTarget)
						{
							this.guiDataTop.Furniture_MovieImage.SetRaycastTarget(false);
						}
						if (!MoviePlayer.Playing(this.guiDataTop.Furniture_MovieImage.gameObject))
						{
							this.ChangeCenterInfo(false);
						}
					}
				}
				else if (SelGachaCtrl.INFO_DISP_UPDATE_TIME <= this.centerInfoDispTimeElapsed)
				{
					this.ChangeCenterInfo(false);
					return;
				}
			}
			else
			{
				GameObject gameObject = ((this.cloneCharaMovieObject == null) ? this.guiDataTop.Chara_MovieImage.gameObject : this.cloneCharaMovieObject);
				if (gameObject.activeSelf)
				{
					if (this.guiDataTop.Chara_MovieImage.m_RawImage.raycastTarget)
					{
						this.guiDataTop.Chara_MovieImage.SetRaycastTarget(false);
					}
					if (this.cloneCharaMovieObject == null && !this.RenderCharaCenter.IsBloom())
					{
						if (!MoviePlayer.Playing(gameObject))
						{
							this.ChangeCenterInfo(false);
							return;
						}
					}
					else if (!MoviePlayer.Playing(gameObject) && this.CLONE_CHARA_MOVIE_TIME <= this.movieTime)
					{
						this.ChangeCenterInfo(false);
						return;
					}
				}
			}
		}
	}

	public void Disable()
	{
		this.guiDataTop.baseObj.SetActive(false);
		if (null != this.RenderCharaCenter)
		{
			Object.Destroy(this.RenderCharaCenter.gameObject);
			this.RenderCharaCenter = null;
		}
		if (null != this.RenderCharaLB)
		{
			Object.Destroy(this.RenderCharaLB.gameObject);
			this.RenderCharaLB = null;
		}
		if (this.guiDataResult != null)
		{
			Object.Destroy(this.guiDataResult.baseObj);
			this.guiDataResult = null;
		}
		if (this.GachaPlayActionCoroutine != null)
		{
			Singleton<SceneManager>.Instance.StopCoroutine(this.GachaPlayActionCoroutine);
			this.GachaPlayActionCoroutine = null;
		}
		if (this.gachaAuth != null)
		{
			this.gachaAuth.DestroyAllObject();
			this.gachaAuth = null;
		}
		CanvasManager.HdlSelPurchaseStoneWindowCtrl.RemoveOnSuccessPurchaseListener(new UnityAction(this.OnSuccessPurcheseStone));
		CanvasManager.HdlSelMonthlyPackWindowCtrl.RemoveOnSuccessPurchaseListener(new UnityAction(this.OnSuccessPurcheseMonthlyPack));
	}

	public void Destroy()
	{
		if (null != this.gachaWindowInfo)
		{
			Object.Destroy(this.gachaWindowInfo.gameObject);
			this.gachaWindowInfo = null;
		}
	}

	public void OnDisable()
	{
		this.Disable();
	}

	private void RefreshGachaPackDataList(int focusGachaId, bool refreshMonthlyPackData = true, bool isSetup = false, int gachaTab = 0)
	{
		if (refreshMonthlyPackData)
		{
			this.RefreshEnableMonthlyPackData();
		}
		int count = this.gachaPackDataList.Count;
		this.currentGachaTab = gachaTab;
		List<DataManagerGacha.GachaPackData> list = DataManager.DmGacha.CopyGachaPackDataList();
		int i;
		int j;
		for (i = 1; i < this.guiDataTop.TabGroup.m_PguiTabList.Count; i = j + 1)
		{
			bool flag = list.Any<DataManagerGacha.GachaPackData>((DataManagerGacha.GachaPackData gacha) => gacha.staticData.tabCategory == (DataManagerGacha.TabCategory)i);
			this.guiDataTop.TabGroup.m_PguiTabList[i].SetActEnable(flag);
			j = i;
		}
		this.UpdateGachaPackDataList(list);
		int num = this.GetFocusGachaIndex(focusGachaId);
		if (count != this.gachaPackDataList.Count)
		{
			this.gachaBannerImgMap.Clear();
			this.guiDataTop.GachaBannerScroll.Initialize(this.gachaPackDataList.Count);
			if (isSetup)
			{
				this.guiDataTop.GachaBannerScroll.ChangeFocusItem(num, true);
			}
			else
			{
				num = 0;
			}
		}
		else
		{
			this.guiDataTop.GachaBannerScroll.ChangeFocusItem(num, true);
		}
		this.focusForceChanged = true;
		this.gachaIdChanged = this.gachaPackDataList[num].gachaId != focusGachaId;
		this.guiDataTop.GachaBannerScroll.Refresh();
		if (!this.guiDataTop.baseObj.activeSelf)
		{
			DataManagerGacha.GachaPackData gachaPackData = this.gachaPackDataList[num];
			this.UpdateStoneNum(gachaPackData);
			this.UpdateGachaTopDispData(gachaPackData);
		}
	}

	private void UpdateGachaPackDataList(List<DataManagerGacha.GachaPackData> list)
	{
		this.GachaPackDataLastUpdateTime = TimeManager.Now;
		this.gachaPackDataList = ((this.currentGachaTab == 0) ? list : list.FindAll((DataManagerGacha.GachaPackData gacha) => gacha.staticData.tabCategory == (DataManagerGacha.TabCategory)this.currentGachaTab));
		DataManager.DmGacha.SelectedGachaIdHashSet = new HashSet<int>(DataManager.DmGameStatus.MakeUserFlagData().GachaNewInfoData.DisplayedIDList);
	}

	private int GetFocusGachaIndex(int gachaId)
	{
		if (gachaId == 0)
		{
			return 0;
		}
		int num = this.gachaPackDataList.FindIndex((DataManagerGacha.GachaPackData x) => x.gachaId == gachaId);
		if (0 <= num)
		{
			return num;
		}
		DataManagerGacha.GachaStaticData gsd = DataManager.DmGacha.GetGachaStaticData(gachaId);
		if (gsd == null)
		{
			return 0;
		}
		if (gsd.ReplaceGroupId != 0)
		{
			num = this.gachaPackDataList.FindIndex((DataManagerGacha.GachaPackData x) => x.staticData.ReplaceGroupId == gsd.ReplaceGroupId);
			if (0 <= num)
			{
				return num;
			}
		}
		if (gsd.gachaGroupId != 0)
		{
			num = this.gachaPackDataList.FindIndex((DataManagerGacha.GachaPackData x) => x.staticData.gachaGroupId == gsd.gachaGroupId);
			if (0 <= num)
			{
				return num;
			}
		}
		return 0;
	}

	private void RefreshEnableMonthlyPackData()
	{
		this.nowPackData = null;
		this.nowPackDataEndDateTime = new DateTime(1970, 1, 1, 9, 0, 0);
		this.nextPackData = null;
		this.nextPackDataEndDateTime = new DateTime(1970, 1, 1, 9, 0, 0);
		DataManagerMonthlyPack.UserPackData userPackData = DataManager.DmMonthlyPack.nowPackData;
		if (userPackData.MonthlypackData == null)
		{
			return;
		}
		DateTime dateTime = new DateTime(TimeManager.Now.Year, TimeManager.Now.Month, TimeManager.Now.Day);
		DateTime dateTime2 = new DateTime(userPackData.EndDatetime.Year, userPackData.EndDatetime.Month, userPackData.EndDatetime.Day);
		DataManagerMonthlyPack.UserPackData userPackData2 = DataManager.DmMonthlyPack.nextPackData;
		if (dateTime <= dateTime2)
		{
			this.nowPackData = userPackData.MonthlypackData;
			this.nowPackDataEndDateTime = userPackData.EndDatetime;
			this.nextPackData = userPackData2.MonthlypackData;
			this.nextPackDataEndDateTime = userPackData2.EndDatetime;
			return;
		}
		if (userPackData2.MonthlypackData == null)
		{
			return;
		}
		DateTime dateTime3 = new DateTime(userPackData2.EndDatetime.Year, userPackData2.EndDatetime.Month, userPackData2.EndDatetime.Day);
		if (dateTime <= dateTime3)
		{
			this.nowPackData = userPackData2.MonthlypackData;
			this.nowPackDataEndDateTime = userPackData2.EndDatetime;
		}
	}

	private void OpenExecuteGachaConfirmWindow(bool isLeftBtn, DataManagerGacha.GachaPackData gpd)
	{
		if (gpd == null)
		{
			return;
		}
		this.lastSelectGacha = new SelGachaCtrl.LastSelectGacha();
		this.lastSelectGacha.id = gpd.gachaId;
		this.lastSelectGacha.isLeftBtn = isLeftBtn;
		if (2000 == gpd.staticData.StepResetTime.Year && this.ChkStepResetTimeIsOver(gpd.staticData.StepResetTime))
		{
			CanvasManager.HdlOpenWindowBasic.Setup("確認", "更新データが見つかりました\nデータ更新を行います", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int index)
			{
				if (this.IsDispGuiResult)
				{
					this.ChangeModeResultToTop();
				}
				this.guiDataTop.RefreshGachaTop = this.RefreshGachaTop(true);
				return true;
			}, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			return;
		}
		bool flag = 1 < gpd.staticData.typeDataList.Count;
		int num = ((!isLeftBtn && flag) ? 1 : 0);
		DataManagerGacha.GachaStaticTypeData staticTypeData = gpd.staticData.typeDataList[num];
		DataManagerGacha.DynamicGachaTypeData dynamicGachaTypeData = gpd.dynamicData.gachaTypeData[num];
		int useItemId = staticTypeData.useItemId;
		int num2 = DataManager.DmItem.GetUserItemData(useItemId).num;
		int requiredNum = staticTypeData.useItemNumber;
		if (isLeftBtn && !flag)
		{
			bool flag2 = this.IsSubstituteEnable(staticTypeData);
			if (staticTypeData.subItemUseCondition != 0 && this.ChkSwitchEnableSubstitute(staticTypeData, flag2))
			{
				CanvasManager.HdlOpenWindowBasic.Setup("確認", "月間パスポートの期限が終了しました。\nデータ更新を行います。", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int index)
				{
					if (this.IsDispGuiResult)
					{
						this.ChangeModeResultToTop();
					}
					this.guiDataTop.RefreshGachaTop = this.RefreshGachaTop(true);
					return true;
				}, null, false);
				CanvasManager.HdlOpenWindowBasic.Open();
				return;
			}
			bool flag3 = this.IsMatchingSubItemUseCondition(staticTypeData, true);
			if (staticTypeData.substituteItemId == 0 || !flag3)
			{
				return;
			}
			useItemId = staticTypeData.substituteItemId;
			num2 = DataManager.DmItem.GetUserItemData(staticTypeData.substituteItemId).num;
			requiredNum = staticTypeData.substituteItemNumber;
		}
		else if (this.IsDiscountEnable(staticTypeData, dynamicGachaTypeData))
		{
			DataManagerGacha.DiscountType discountType = staticTypeData.discountData.discountType;
			if (discountType != DataManagerGacha.DiscountType.NoneReset)
			{
				if (discountType == DataManagerGacha.DiscountType.OnceADay)
				{
					DateTime now = TimeManager.Now;
					DateTime dateTime = new DateTime(now.Year, now.Month, now.Day);
					if (dynamicGachaTypeData.lastPlayDateTime < dateTime)
					{
						requiredNum -= staticTypeData.discountData.discountNum;
					}
				}
			}
			else
			{
				requiredNum -= staticTypeData.discountData.discountNum;
			}
		}
		if (DataManagerGacha.Category.StepUp == gpd.staticData.gachaCategory)
		{
			this.lastSelectGacha.isStepUp = true;
			DataManagerGacha.GachaStaticData gachaStaticData = gpd.staticData;
			bool flag4 = true;
			int num3 = 1;
			while (flag4)
			{
				if (gachaStaticData.stepPreviousGachaId != 0)
				{
					num3++;
					gachaStaticData = DataManager.DmGacha.GetGachaStaticData(gachaStaticData.stepPreviousGachaId);
				}
				else
				{
					flag4 = false;
				}
			}
			if (gpd.staticData.stepNextGachaId != 0)
			{
				this.lastSelectGacha.stepNextGachaId = gpd.staticData.stepNextGachaId;
				this.lastSelectGacha.stepNextNum = num3 + 1;
			}
			else
			{
				int num4 = dynamicGachaTypeData.totalSubPlayNum + 1;
				bool flag5 = 0 < gachaStaticData.availableCount && gachaStaticData.availableCount <= num4;
				this.lastSelectGacha.stepNextGachaId = ((!flag5) ? gachaStaticData.gachaId : 0);
				this.lastSelectGacha.stepNextNum = 1;
			}
		}
		HomeCheckResult homeCheckResult = DataManager.DmHome.GetHomeCheckResult();
		if (DataManagerGacha.Category.Roulette == gpd.staticData.gachaCategory && (homeCheckResult.rouletteData == null || (homeCheckResult.rouletteData.targetGachaId == gpd.staticData.gachaId && homeCheckResult.rouletteData.remainingDrawCount <= 0)))
		{
			string text = "残り回数がありません。";
			CanvasManager.HdlOpenWindowBasic.Setup("確認", text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, null, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			return;
		}
		if (DataManagerGacha.Category.Box == gpd.staticData.gachaCategory && dynamicGachaTypeData.boxRemainNum < staticTypeData.lotTime)
		{
			string text2 = string.Format("残り <color=red>{0}</color> 個  \nぼっくすの中身が足りません。", dynamicGachaTypeData.boxRemainNum);
			CanvasManager.HdlOpenWindowBasic.Setup("確認", text2, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, null, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			return;
		}
		if (requiredNum <= num2)
		{
			string name = DataManager.DmItem.GetUserItemData(useItemId).staticData.GetName();
			string text3 = ((requiredNum != 0) ? (name + "を\n" + requiredNum.ToString() + "消費して") : "無料で");
			text3 += string.Format("{0}回しょうたいします\nよろしいですか？", staticTypeData.lotTime);
			if (gpd.staticData.bonusIdList.Exists((int x) => ItemDef.Id2Kind(x) == ItemDef.Kind.ACHIEVEMENT))
			{
				text3 += "\n<color=red>おまけの称号を重複して獲得した場合は、アイテム変換が行われます</color>";
			}
			CanvasManager.HdlOpenWindowUseItem.SetupByUseItem("しょうたい確認", text3, new PguiOpenWindowCtrl.Callback(this.OnChoiceOpenWindow), requiredNum, num2, true);
			Transform transform = CanvasManager.HdlOpenWindowUseItem.transform.Find("Base/Window/PurchaseConfirmButton");
			if (transform != null)
			{
				string providePeriod = "";
				DateTime startDatetime = gpd.staticData.startDatetime;
				DateTime endDatetime = gpd.staticData.endDatetime;
				string text4 = "";
				if (gpd.staticData.dayOfWeekFlg)
				{
					for (int i = 0; i <= 6; i++)
					{
						switch (i)
						{
						case 0:
							if (gpd.staticData.dayOfWeek[i])
							{
								text4 += ((text4 == "") ? "日" : ",日");
							}
							break;
						case 1:
							if (gpd.staticData.dayOfWeek[i])
							{
								text4 += ((text4 == "") ? "月" : ",月");
							}
							break;
						case 2:
							if (gpd.staticData.dayOfWeek[i])
							{
								text4 += ((text4 == "") ? "火" : ",火");
							}
							break;
						case 3:
							if (gpd.staticData.dayOfWeek[i])
							{
								text4 += ((text4 == "") ? "水" : ",水");
							}
							break;
						case 4:
							if (gpd.staticData.dayOfWeek[i])
							{
								text4 += ((text4 == "") ? "木" : ",木");
							}
							break;
						case 5:
							if (gpd.staticData.dayOfWeek[i])
							{
								text4 += ((text4 == "") ? "金" : ",金");
							}
							break;
						case 6:
							if (gpd.staticData.dayOfWeek[i])
							{
								text4 += ((text4 == "") ? "土" : ",土");
							}
							break;
						}
					}
				}
				if ((gpd.staticData.endDatetime - TimeManager.Now).Days < DataManagerPurchase.LimitItemJudgeDays)
				{
					providePeriod = gpd.staticData.startDatetime.ToString("yyyy/MM/dd H:mm:ss") + " 〜 " + gpd.staticData.endDatetime.ToString("yyyy/MM/dd H:mm:ss");
					if (gpd.staticData.dayOfWeekFlg)
					{
						providePeriod = providePeriod + " の" + text4 + "曜日";
					}
				}
				else if (gpd.staticData.dayOfWeekFlg)
				{
					providePeriod = "毎週" + text4 + "曜日";
				}
				PguiButtonCtrl component = transform.GetComponent<PguiButtonCtrl>();
				if (component != null)
				{
					component.AddOnClickListener(delegate(PguiButtonCtrl btn)
					{
						CanvasManager.HdlPurchaseConfirmWindow.Initialize(string.Format("{0} {1}回", gpd.staticData.gachaName, staticTypeData.lotTime), DataManager.DmItem.GetItemStaticBase(useItemId).GetName(), requiredNum, providePeriod, PurchaseConfirmWindow.TEMP_IMMEDIATE_DELIVERY, false);
					}, PguiButtonCtrl.SoundType.DEFAULT);
				}
			}
			CanvasManager.HdlOpenWindowUseItem.Open();
			return;
		}
		CanvasManager.HdlOpenWindowNoStone.SetupByNoStone(requiredNum, useItemId, delegate(int index)
		{
			if (1 == index)
			{
				CanvasManager.HdlSelPurchaseStoneWindowCtrl.Setup(PurchaseProductOne.TabType.Invalid);
			}
			return true;
		});
		CanvasManager.HdlOpenWindowNoStone.Open();
	}

	private bool ChkStepResetTimeIsOver(DateTime stepResetTime)
	{
		bool flag = false;
		DateTime dateTime = new DateTime(this.GachaPackDataLastUpdateTime.Year, this.GachaPackDataLastUpdateTime.Month, this.GachaPackDataLastUpdateTime.Day, stepResetTime.Hour, stepResetTime.Minute, stepResetTime.Second);
		if (dateTime < this.GachaPackDataLastUpdateTime)
		{
			dateTime = dateTime.AddDays(1.0);
		}
		if (dateTime < TimeManager.Now)
		{
			flag = true;
		}
		return flag;
	}

	private bool ChkSwitchEnableSubstitute(DataManagerGacha.GachaStaticTypeData staticTypeData, bool nowPackEnableSubstitute)
	{
		bool flag = false;
		if (this.nowPackData != null)
		{
			DateTime dateTime = new DateTime(TimeManager.Now.Year, TimeManager.Now.Month, TimeManager.Now.Day);
			if (new DateTime(this.nowPackDataEndDateTime.Year, this.nowPackDataEndDateTime.Month, this.nowPackDataEndDateTime.Day) < dateTime)
			{
				if (this.nextPackData != null)
				{
					if (new DateTime(this.nextPackDataEndDateTime.Year, this.nextPackDataEndDateTime.Month, this.nextPackDataEndDateTime.Day) < dateTime)
					{
						flag = true;
					}
					else
					{
						bool flag2 = this.IsMatchingSubItemUseCondition(staticTypeData, false) && DataManager.DmItem.GetUserItemData(staticTypeData.substituteItemId).num >= staticTypeData.substituteItemNumber;
						if (nowPackEnableSubstitute != flag2)
						{
							flag = true;
						}
						else
						{
							this.nowPackData = this.nextPackData;
							this.nowPackDataEndDateTime = this.nextPackDataEndDateTime;
							this.nextPackData = null;
							this.nextPackDataEndDateTime = new DateTime(1970, 1, 1, 9, 0, 0);
						}
					}
				}
				else
				{
					flag = true;
				}
			}
		}
		return flag;
	}

	private void UpdateStoneNum(DataManagerGacha.GachaPackData gachaPackData)
	{
		if (gachaPackData == null)
		{
			return;
		}
		List<int> list = new List<int>();
		List<DataManagerGacha.GachaStaticTypeData> typeDataList = gachaPackData.staticData.typeDataList;
		foreach (DataManagerGacha.GachaStaticTypeData gachaStaticTypeData in typeDataList)
		{
			list.Add(gachaStaticTypeData.useItemId);
		}
		if (1 == typeDataList.Count)
		{
			DataManagerGacha.GachaStaticTypeData staticTypeData = typeDataList[0];
			bool flag = this.IsMatchingSubItemUseCondition(staticTypeData, true);
			if (staticTypeData.substituteItemId != 0 && flag && !list.Exists((int x) => x == staticTypeData.substituteItemId))
			{
				list.Add(staticTypeData.substituteItemId);
			}
		}
		list = new List<int>(new HashSet<int>(list));
		int num = 0;
		foreach (SelGachaCtrl.ItemBoardBase itemBoardBase in this.guiDataTop.GachaUseItemBtnList)
		{
			if (list.Count <= num)
			{
				itemBoardBase.button.SetActEnable(false, false, false);
				itemBoardBase.Base.SetActive(false);
			}
			else
			{
				itemBoardBase.button.SetActEnable(true, false, false);
				itemBoardBase.Base.SetActive(true);
				itemBoardBase.Setup(list[num]);
				num++;
			}
		}
	}

	private void UpdateGachaResultOneMoreBtn(DataManagerGacha.GachaStaticData execGachaStaticData)
	{
		bool flag = DataManagerGacha.Category.StepUp == execGachaStaticData.gachaCategory;
		if (flag)
		{
			bool flag2 = false;
			if (this.lastSelectGacha.stepNextGachaId != 0)
			{
				DataManagerGacha.GachaPackData gachaPackData = this.gachaPackDataList.Find((DataManagerGacha.GachaPackData x) => x.gachaId == this.lastSelectGacha.stepNextGachaId);
				flag2 = gachaPackData != null;
			}
			this.guiDataResult.Btn_Onemore.SetActEnable(flag2, false, false);
			this.guiDataResult.Btn_OnemoreText.text = string.Format("ステップ{0}を回す", this.lastSelectGacha.stepNextNum);
			return;
		}
		this.guiDataResult.Btn_OnemoreText.text = "もう一度回す";
		DataManagerGacha.GachaPackData gachaPackData2 = this.gachaPackDataList.Find((DataManagerGacha.GachaPackData x) => x.gachaId == execGachaStaticData.gachaId);
		if (gachaPackData2 == null)
		{
			this.guiDataResult.Btn_Onemore.SetActEnable(false, false, false);
			return;
		}
		bool flag3 = 1 < gachaPackData2.staticData.typeDataList.Count;
		int num = ((!this.lastSelectGacha.isLeftBtn && flag3) ? 1 : 0);
		DataManagerGacha.GachaStaticTypeData gachaStaticTypeData = gachaPackData2.staticData.typeDataList[num];
		DataManagerGacha.DynamicGachaTypeData dynamicGachaTypeData = gachaPackData2.dynamicData.gachaTypeData[num];
		int num2 = gachaStaticTypeData.useItemId;
		int num3 = gachaStaticTypeData.useItemNumber;
		bool flag4 = false;
		if (this.lastSelectGacha.isLeftBtn && !flag3)
		{
			bool flag5 = this.IsMatchingSubItemUseCondition(gachaStaticTypeData, true);
			if (gachaStaticTypeData.substituteItemId != 0 && flag5)
			{
				num2 = gachaStaticTypeData.substituteItemId;
				num3 = gachaStaticTypeData.substituteItemNumber;
			}
			else
			{
				flag4 = true;
			}
		}
		bool flag6 = this.IsDiscountEnable(gachaStaticTypeData, dynamicGachaTypeData);
		bool flag7 = false;
		if (num2 - 30001 <= 1 || num2 == 30100)
		{
			flag7 = true;
		}
		DateTime now = TimeManager.Now;
		DateTime dateTime = new DateTime(now.Year, now.Month, now.Day);
		bool flag8 = 0 < gachaPackData2.staticData.availableCount && gachaPackData2.staticData.availableCount <= dynamicGachaTypeData.totalSubPlayNum;
		bool flag9 = flag6 && gachaStaticTypeData.discountData.discountType == DataManagerGacha.DiscountType.OnceADay && dynamicGachaTypeData.lastPlayDateTime < dateTime;
		bool flag10 = DataManager.DmItem.GetUserItemData(num2).num >= num3;
		bool flag11 = !flag4 && !flag && (flag10 || (!flag8 && (flag7 || flag9)));
		if (DataManagerGacha.Category.Box == gachaPackData2.staticData.gachaCategory && dynamicGachaTypeData.boxRemainNum < gachaStaticTypeData.lotTime)
		{
			flag11 = false;
		}
		this.guiDataResult.Btn_Onemore.SetActEnable(flag11, false, false);
	}

	private bool IsDiscountEnable(DataManagerGacha.GachaStaticTypeData staticTypeData, DataManagerGacha.DynamicGachaTypeData dynamicTypeData)
	{
		return staticTypeData.discountData != null && staticTypeData.discountData.startDatetime <= TimeManager.Now && TimeManager.Now < staticTypeData.discountData.endDatetime && (staticTypeData.discountData.availableCount == 0 || staticTypeData.discountData.availableCount > dynamicTypeData.discountPlayNum);
	}

	private bool IsSubstituteEnable(DataManagerGacha.GachaStaticTypeData staticTypeData)
	{
		return this.IsMatchingSubItemUseCondition(staticTypeData, true) && staticTypeData.substituteItemId != 0 && DataManager.DmItem.GetUserItemData(staticTypeData.substituteItemId).num >= staticTypeData.substituteItemNumber;
	}

	private bool IsMatchingSubItemUseCondition(DataManagerGacha.GachaStaticTypeData typeData, bool useNowPackData)
	{
		bool flag = false;
		DataManagerMonthlyPack.PurchaseMonthlypackData purchaseMonthlypackData = (useNowPackData ? this.nowPackData : this.nextPackData);
		switch (typeData.subItemUseCondition)
		{
		case 0:
			flag = true;
			break;
		case 1:
			if (purchaseMonthlypackData != null && purchaseMonthlypackData.PackType == 1)
			{
				flag = true;
			}
			break;
		case 2:
			if (purchaseMonthlypackData != null)
			{
				int num = purchaseMonthlypackData.PackType;
				if (num - 1 <= 1)
				{
					flag = true;
				}
			}
			break;
		case 3:
			if (purchaseMonthlypackData != null)
			{
				int num = purchaseMonthlypackData.PackType;
				if (num - 1 <= 2)
				{
					flag = true;
				}
			}
			break;
		case 4:
			if (purchaseMonthlypackData != null)
			{
				int num = purchaseMonthlypackData.PackType;
				if (num - 1 <= 3)
				{
					flag = true;
				}
			}
			break;
		default:
			flag = true;
			break;
		}
		return flag;
	}

	private void CreateDetailWindow(DataManagerGacha.GachaPackData gachaPackData)
	{
		this.pauseCenterInfoDispLoop = true;
		Singleton<SceneManager>.Instance.StartCoroutine(this.gachaWindowInfo.Open(gachaPackData, delegate
		{
			this.pauseCenterInfoDispLoop = false;
		}));
	}

	private void OpenUseItemInfoWindow(int itemId)
	{
		CanvasManager.HdlOpenWindowItemInfo.SetupItemInfo(itemId);
		CanvasManager.HdlOpenWindowItemInfo.Open();
	}

	private void ChangeCenterInfo(bool viewBanner)
	{
		bool flag = this.CenterInfoDisplayedBanner != viewBanner;
		this.CenterInfoDisplayedBanner = viewBanner;
		if (!this.CenterInfoDisplayedBanner)
		{
			this.pauseCenterInfoDispLoop = false;
			this.guiDataTop.BannerAll.SetActive(false);
			this.UpdateCenterInfoDispIndex(flag);
			int centerInfoDispItemId = this.GetCenterInfoDispItemId();
			this.UpdateCenterInfoDisp(centerInfoDispItemId);
			return;
		}
		this.pauseCenterInfoDispLoop = true;
		this.guiDataTop.BannerAll.SetActive(true);
		this.guiDataTop.CharaInfoAll.SetActive(false);
		this.guiDataTop.PhotoInfoAll.SetActive(false);
		this.guiDataTop.FurnitureInfoAll.SetActive(false);
		RenderTextureChara renderCharaCenter = this.RenderCharaCenter;
		if (renderCharaCenter == null)
		{
			return;
		}
		renderCharaCenter.gameObject.SetActive(false);
	}

	private void SetCurrentBanner(int key)
	{
		this.ResetBannerSize();
		if (this.gachaBannerImgMap.ContainsKey(key))
		{
			RawImage rawImage = this.gachaBannerImgMap[key].m_RawImage;
			rawImage.color = Color.white;
			rawImage.transform.localScale = Vector3.one;
			RectTransform component = rawImage.transform.gameObject.GetComponent<RectTransform>();
			component.anchoredPosition = new Vector2(-30f, component.anchoredPosition.y);
			Component component2 = rawImage.transform.Find("Img_Yaji").GetComponent<PguiImageCtrl>();
			SimpleAnimation yaji_anime = rawImage.transform.Find("Img_Yaji").GetComponent<SimpleAnimation>();
			component2.gameObject.SetActive(true);
			yaji_anime.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, delegate
			{
				yaji_anime.ExPlayAnimation(SimpleAnimation.ExPguiStatus.LOOP, null);
			});
		}
	}

	private void ResetBannerSize()
	{
		foreach (PguiRawImageCtrl pguiRawImageCtrl in this.gachaBannerImgMap.Values)
		{
			RawImage rawImage = pguiRawImageCtrl.m_RawImage;
			rawImage.color = new Color32(184, 184, 184, byte.MaxValue);
			rawImage.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
			RectTransform component = rawImage.transform.gameObject.GetComponent<RectTransform>();
			component.anchoredPosition = new Vector2(0f, component.anchoredPosition.y);
			pguiRawImageCtrl.transform.Find("Img_Yaji").GetComponent<PguiImageCtrl>().gameObject.SetActive(false);
		}
	}

	private void UpdateGachaTopDispData(DataManagerGacha.GachaPackData gachaPackData)
	{
		if (gachaPackData == null)
		{
			this.guiDataTop.GachaBtnSet_Left.BaseButton.SetActEnable(false, false, false);
			this.guiDataTop.GachaBtnSet_Right.BaseButton.SetActEnable(false, false, false);
			return;
		}
		DataManager.DmGacha.SelectedGachaIdHashSet.Add(gachaPackData.gachaId);
		this.centerInfoDispIdList = gachaPackData.staticData.InfoDispIdList;
		bool flag = 0 < this.centerInfoDispIdList.Count;
		this.guiDataTop.Btn_Reload.SetActEnable(flag, false, false);
		this.guiDataTop.Btn_GachaDetailInfo.SetActEnable(!gachaPackData.staticData.RateHideFlg, false, false);
		this.guiDataTop.CeilingCountImageCtrl.gameObject.SetActive(0 < gachaPackData.staticData.highLimit);
		if (this.guiDataTop.CeilingCountImageCtrl.gameObject.activeSelf)
		{
			int highLimit = gachaPackData.staticData.highLimit;
			int ceilingCountNow = DataManager.DmGacha.GetCeilingCountNow(gachaPackData.staticData.gachaId);
			this.guiDataTop.CeilingCountText.text = string.Format("★4確定まで\n<size=24>{0}/{1}</size>", ceilingCountNow, highLimit);
		}
		this.guiDataTop.BoxBtns.SetActive(DataManagerGacha.Category.Box == gachaPackData.staticData.gachaCategory);
		if (this.guiDataTop.BoxBtns.activeSelf)
		{
			this.guiDataTop.ResetCountInfo.SetActive(0 < gachaPackData.staticData.availableCount);
			int num = gachaPackData.staticData.availableCount - gachaPackData.dynamicData.gachaTypeData[0].resetNum;
			bool flag2 = gachaPackData.staticData.availableCount == 0 || 0 < num;
			this.guiDataTop.Btn_BoxReset.SetActEnable(flag2, false, false);
			this.guiDataTop.ResetCountText.text = string.Format("あと {0} 回", num);
		}
		this.guiDataTop.BannerTexture.banner = "Texture2D/GachaTop/" + gachaPackData.staticData.banner;
		this.UpdateGachaTopBtnLeft(gachaPackData);
		this.UpdateGachaTopBtnRight(gachaPackData);
		this.guiDataTop.StartPopupPlay = this.PopupInitialWaitSync();
		this.guiDataTop.Txt_GachaName.text = gachaPackData.staticData.gachaName;
		DateTime dateTime = TimeManager.Now.AddDays((double)SelGachaCtrl.GACHA_VIEW_LIMIT_DAY);
		this.guiDataTop.Txt_Term.gameObject.SetActive(dateTime > gachaPackData.staticData.endDatetime || gachaPackData.staticData.dayOfWeekFlg);
		if (gachaPackData.staticData.dayOfWeekFlg)
		{
			this.guiDataTop.Txt_Term.text = "開催期間\n" + TimeManager.FormattedTime(gachaPackData.staticData.EndTimeOfDayOfWeek(TimeManager.Now), TimeManager.Format.yyyyMMdd_hhmm) + "まで";
			return;
		}
		this.guiDataTop.Txt_Term.text = "開催期間\n" + TimeManager.FormattedTime(gachaPackData.staticData.endDatetime, TimeManager.Format.yyyyMMdd_hhmm) + "まで";
	}

	private void UpdateGachaTopBtnLeft(DataManagerGacha.GachaPackData gachaPackData)
	{
		this.guiDataTop.GachaBtnSet_Left.BaseButton.gameObject.SetActive(false);
		this.guiDataTop.GachaBtnSet_Left.StopPopup();
		this.guiDataTop.GachaBtnSet_Left.HidePopup();
		DataManagerGacha.GachaStaticTypeData staticTypeData = gachaPackData.staticData.typeDataList[0];
		DataManagerGacha.DynamicGachaTypeData dynamicGachaTypeData = gachaPackData.dynamicData.gachaTypeData.Find((DataManagerGacha.DynamicGachaTypeData x) => staticTypeData.gachaType == x.gachaType);
		if (dynamicGachaTypeData == null)
		{
			return;
		}
		bool flag = false;
		bool flag2 = false;
		int num = staticTypeData.useItemId;
		int num2 = staticTypeData.useItemNumber;
		if (1 == gachaPackData.staticData.typeDataList.Count)
		{
			bool flag3 = this.IsMatchingSubItemUseCondition(staticTypeData, true);
			if (staticTypeData.substituteItemId == 0 || !flag3)
			{
				return;
			}
			this.guiDataTop.GachaBtnSet_Left.BaseButton.gameObject.SetActive(true);
			num = staticTypeData.substituteItemId;
			num2 = staticTypeData.substituteItemNumber;
		}
		else
		{
			if (2 != gachaPackData.staticData.typeDataList.Count)
			{
				return;
			}
			this.guiDataTop.GachaBtnSet_Left.BaseButton.gameObject.SetActive(true);
			if (this.IsDiscountEnable(staticTypeData, dynamicGachaTypeData))
			{
				if (DataManagerGacha.DiscountType.NoneReset == staticTypeData.discountData.discountType)
				{
					flag = true;
					this.guiDataTop.GachaBtnSet_Left.PopupSale.MainText.text = ((staticTypeData.discountData.discountNum == num2) ? "無料！" : string.Format("{0}%オフ！", staticTypeData.discountData.discountNum * 100 / num2));
					num2 -= staticTypeData.discountData.discountNum;
					int num3 = ((0 < staticTypeData.discountData.availableCount) ? (staticTypeData.discountData.availableCount - dynamicGachaTypeData.discountPlayNum) : 0);
					if (0 < num3)
					{
						this.guiDataTop.GachaBtnSet_Left.PopupSale.CountText.gameObject.SetActive(true);
						this.guiDataTop.GachaBtnSet_Left.PopupSale.CountText.text = string.Format("あと<size=20>{0}</size>回", num3);
					}
					else
					{
						this.guiDataTop.GachaBtnSet_Left.PopupSale.CountText.gameObject.SetActive(false);
					}
				}
				DateTime now = TimeManager.Now;
				DateTime dateTime = new DateTime(now.Year, now.Month, now.Day);
				if (DataManagerGacha.DiscountType.OnceADay == staticTypeData.discountData.discountType && dynamicGachaTypeData.lastPlayDateTime < dateTime)
				{
					this.guiDataTop.GachaBtnSet_Left.FreeText.gameObject.SetActive(true);
					this.guiDataTop.GachaBtnSet_Left.FreeCampaignText.gameObject.SetActive(false);
					this.guiDataTop.GachaBtnSet_Left.UseNumText.gameObject.SetActive(false);
					this.guiDataTop.GachaBtnSet_Left.StoneIcon.gameObject.SetActive(false);
					flag2 = true;
				}
			}
			if (gachaPackData.staticData.gachaCategory == DataManagerGacha.Category.Roulette)
			{
				HomeCheckResult homeCheckResult = DataManager.DmHome.GetHomeCheckResult();
				if (homeCheckResult != null && homeCheckResult.rouletteData != null)
				{
					this.guiDataTop.GachaBtnSet_Left.FreeText.gameObject.SetActive(false);
					this.guiDataTop.GachaBtnSet_Left.FreeCampaignText.gameObject.SetActive(true);
					string text;
					if (homeCheckResult == null || homeCheckResult.rouletteData == null || homeCheckResult.rouletteData.targetGachaId != gachaPackData.staticData.gachaId)
					{
						text = "残り回数なし";
					}
					else
					{
						text = ((homeCheckResult.rouletteData.remainingDrawCount > 0) ? ("本日あと" + homeCheckResult.rouletteData.remainingDrawCount.ToString() + "回無料!") : "本日分実行済み");
					}
					this.guiDataTop.GachaBtnSet_Left.FreeCampaignText.text = text;
					this.guiDataTop.GachaBtnSet_Left.UseNumText.gameObject.SetActive(false);
					this.guiDataTop.GachaBtnSet_Left.StoneIcon.gameObject.SetActive(false);
					flag2 = true;
				}
			}
		}
		if (!flag2)
		{
			this.guiDataTop.GachaBtnSet_Left.FreeText.gameObject.SetActive(false);
			this.guiDataTop.GachaBtnSet_Left.FreeCampaignText.gameObject.SetActive(false);
			this.guiDataTop.GachaBtnSet_Left.UseNumText.gameObject.SetActive(true);
			this.guiDataTop.GachaBtnSet_Left.UseNumText.text = num2.ToString();
			this.guiDataTop.GachaBtnSet_Left.StoneIcon.gameObject.SetActive(true);
			if (num != 0)
			{
				this.guiDataTop.GachaBtnSet_Left.StoneIcon.SetRawImage(DataManager.DmItem.GetItemStaticBase(num).GetIconName(), true, false, null);
				this.guiDataTop.GachaBtnSet_Left.StoneIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(SelGachaCtrl.ICON_SIZE, SelGachaCtrl.ICON_SIZE);
			}
		}
		this.guiDataTop.GachaBtnSet_Left.TimesText.text = string.Format("{0}回", staticTypeData.lotTime);
		int num4 = staticTypeData.bonusItemLimit - dynamicGachaTypeData.totalSubPlayNum;
		bool flag4 = (staticTypeData.bonusItemOneId != 0 || staticTypeData.bonusItemSetId != 0) && (staticTypeData.bonusItemLimit == 0 || 0 < num4);
		if (flag4 && staticTypeData.bonusItemLimit != 0)
		{
			this.guiDataTop.GachaBtnSet_Left.PopupAnother.CountText.gameObject.SetActive(true);
			this.guiDataTop.GachaBtnSet_Left.PopupAnother.CountText.text = string.Format("あと<size=20>{0}</size>回", num4);
		}
		else
		{
			this.guiDataTop.GachaBtnSet_Left.PopupAnother.CountText.gameObject.SetActive(false);
		}
		if (0 < gachaPackData.staticData.availableCount)
		{
			int num5 = gachaPackData.staticData.availableCount - dynamicGachaTypeData.totalSubPlayNum;
			if (0 < num5)
			{
				this.guiDataTop.GachaBtnSet_Left.CountInfo.gameObject.SetActive(true);
				this.guiDataTop.GachaBtnSet_Left.CountInfo.transform.Find("Num_Count").GetComponent<Text>().text = string.Format("あと{0}回", num5);
				this.guiDataTop.GachaBtnSet_Left.BaseButton.SetActEnable(true, false, false);
			}
			else
			{
				this.guiDataTop.GachaBtnSet_Left.CountInfo.gameObject.SetActive(false);
				this.guiDataTop.GachaBtnSet_Left.BaseButton.SetActEnable(false, false, false);
			}
		}
		else
		{
			this.guiDataTop.GachaBtnSet_Left.CountInfo.gameObject.SetActive(false);
			this.guiDataTop.GachaBtnSet_Left.BaseButton.SetActEnable(true, false, false);
		}
		this.guiDataTop.GachaBtnSet_Left.PopupInitialize(flag, flag4);
	}

	private void UpdateGachaTopBtnRight(DataManagerGacha.GachaPackData gachaPackData)
	{
		this.guiDataTop.GachaBtnSet_Right.BaseButton.gameObject.SetActive(false);
		this.guiDataTop.GachaBtnSet_Right.StopPopup();
		this.guiDataTop.GachaBtnSet_Right.HidePopup();
		DataManagerGacha.GachaStaticTypeData staticTypeData;
		if (1 == gachaPackData.staticData.typeDataList.Count)
		{
			staticTypeData = gachaPackData.staticData.typeDataList[0];
		}
		else
		{
			if (2 != gachaPackData.staticData.typeDataList.Count)
			{
				return;
			}
			staticTypeData = gachaPackData.staticData.typeDataList[1];
		}
		DataManagerGacha.DynamicGachaTypeData dynamicGachaTypeData = gachaPackData.dynamicData.gachaTypeData.Find((DataManagerGacha.DynamicGachaTypeData x) => staticTypeData.gachaType == x.gachaType);
		if (dynamicGachaTypeData == null)
		{
			return;
		}
		this.guiDataTop.GachaBtnSet_Right.BaseButton.gameObject.SetActive(true);
		int useItemId = staticTypeData.useItemId;
		int num = staticTypeData.useItemNumber;
		bool flag = this.IsDiscountEnable(staticTypeData, dynamicGachaTypeData);
		bool flag2 = false;
		bool flag3 = false;
		if (flag)
		{
			if (DataManagerGacha.DiscountType.NoneReset == staticTypeData.discountData.discountType)
			{
				flag2 = true;
				this.guiDataTop.GachaBtnSet_Right.PopupSale.MainText.text = ((staticTypeData.discountData.discountNum == num) ? "無料！" : string.Format("{0}%オフ！", staticTypeData.discountData.discountNum * 100 / num));
				num -= staticTypeData.discountData.discountNum;
				int num2 = ((0 < staticTypeData.discountData.availableCount) ? (staticTypeData.discountData.availableCount - dynamicGachaTypeData.discountPlayNum) : 0);
				if (0 < num2)
				{
					this.guiDataTop.GachaBtnSet_Right.PopupSale.CountText.gameObject.SetActive(true);
					this.guiDataTop.GachaBtnSet_Right.PopupSale.CountText.text = string.Format("あと<size=20>{0}</size>回", num2);
				}
				else
				{
					this.guiDataTop.GachaBtnSet_Right.PopupSale.CountText.gameObject.SetActive(false);
				}
			}
			DateTime now = TimeManager.Now;
			DateTime dateTime = new DateTime(now.Year, now.Month, now.Day);
			if (DataManagerGacha.DiscountType.OnceADay == staticTypeData.discountData.discountType && dynamicGachaTypeData.lastPlayDateTime < dateTime)
			{
				this.guiDataTop.GachaBtnSet_Right.FreeText.gameObject.SetActive(true);
				this.guiDataTop.GachaBtnSet_Right.FreeCampaignText.gameObject.SetActive(false);
				this.guiDataTop.GachaBtnSet_Right.UseNumText.gameObject.SetActive(false);
				this.guiDataTop.GachaBtnSet_Right.StoneIcon.gameObject.SetActive(false);
				flag3 = true;
			}
		}
		if (gachaPackData.staticData.gachaCategory == DataManagerGacha.Category.Roulette)
		{
			HomeCheckResult homeCheckResult = DataManager.DmHome.GetHomeCheckResult();
			this.guiDataTop.GachaBtnSet_Right.FreeText.gameObject.SetActive(false);
			this.guiDataTop.GachaBtnSet_Right.FreeCampaignText.gameObject.SetActive(true);
			string text;
			if (homeCheckResult == null || homeCheckResult.rouletteData == null || homeCheckResult.rouletteData.targetGachaId != gachaPackData.staticData.gachaId)
			{
				text = "残り回数なし";
			}
			else
			{
				text = ((homeCheckResult.rouletteData.remainingDrawCount > 0) ? ("本日あと" + homeCheckResult.rouletteData.remainingDrawCount.ToString() + "回無料!") : "本日分実行済み");
			}
			this.guiDataTop.GachaBtnSet_Right.FreeCampaignText.text = text;
			this.guiDataTop.GachaBtnSet_Right.UseNumText.gameObject.SetActive(false);
			this.guiDataTop.GachaBtnSet_Right.StoneIcon.gameObject.SetActive(false);
			flag3 = true;
		}
		if (!flag3)
		{
			this.guiDataTop.GachaBtnSet_Right.FreeText.gameObject.SetActive(false);
			this.guiDataTop.GachaBtnSet_Right.FreeCampaignText.gameObject.SetActive(false);
			this.guiDataTop.GachaBtnSet_Right.UseNumText.gameObject.SetActive(true);
			this.guiDataTop.GachaBtnSet_Right.UseNumText.text = num.ToString();
			this.guiDataTop.GachaBtnSet_Right.StoneIcon.gameObject.SetActive(true);
			if (useItemId != 0)
			{
				this.guiDataTop.GachaBtnSet_Right.StoneIcon.SetRawImage(DataManager.DmItem.GetItemStaticBase(useItemId).GetIconName(), true, false, null);
				this.guiDataTop.GachaBtnSet_Right.StoneIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(SelGachaCtrl.ICON_SIZE, SelGachaCtrl.ICON_SIZE);
			}
		}
		this.guiDataTop.GachaBtnSet_Right.TimesText.text = string.Format("{0}回", staticTypeData.lotTime);
		int num3 = staticTypeData.bonusItemLimit - dynamicGachaTypeData.totalSubPlayNum;
		bool flag4 = (staticTypeData.bonusItemOneId != 0 || staticTypeData.bonusItemSetId != 0) && (staticTypeData.bonusItemLimit == 0 || 0 < num3);
		if (flag4 && staticTypeData.bonusItemLimit != 0)
		{
			this.guiDataTop.GachaBtnSet_Right.PopupAnother.CountText.gameObject.SetActive(true);
			this.guiDataTop.GachaBtnSet_Right.PopupAnother.CountText.text = string.Format("あと<size=20>{0}</size>回", num3);
		}
		else
		{
			this.guiDataTop.GachaBtnSet_Right.PopupAnother.CountText.gameObject.SetActive(false);
		}
		if (0 < gachaPackData.staticData.availableCount)
		{
			int num4 = gachaPackData.staticData.availableCount - dynamicGachaTypeData.totalSubPlayNum;
			if (0 < num4)
			{
				this.guiDataTop.GachaBtnSet_Right.CountInfo.gameObject.SetActive(true);
				this.guiDataTop.GachaBtnSet_Right.CountInfo.transform.Find("Num_Count").GetComponent<Text>().text = string.Format("あと{0}回", num4);
				this.guiDataTop.GachaBtnSet_Right.BaseButton.SetActEnable(true, false, false);
			}
			else
			{
				this.guiDataTop.GachaBtnSet_Right.CountInfo.gameObject.SetActive(false);
				this.guiDataTop.GachaBtnSet_Right.BaseButton.SetActEnable(false, false, false);
			}
		}
		else
		{
			this.guiDataTop.GachaBtnSet_Right.CountInfo.gameObject.SetActive(false);
			this.guiDataTop.GachaBtnSet_Right.BaseButton.SetActEnable(true, false, false);
		}
		this.guiDataTop.GachaBtnSet_Right.StepUpInfoText.text = gachaPackData.staticData.stepupBtnText;
		this.guiDataTop.GachaBtnSet_Right.StepUpInfo.SetActive(DataManagerGacha.Category.StepUp == gachaPackData.staticData.gachaCategory);
		this.guiDataTop.GachaBtnSet_Right.PopupInitialize(flag2, flag4);
	}

	private void SetupGachaResult(DataManagerGacha.PlayResult result)
	{
		DataManagerGacha.GachaStaticData gachaStaticData = DataManager.DmGacha.GetGachaStaticData(result.gachaId);
		if (this.guiDataResult != null)
		{
			this.guiDataResult.baseObj.SetActive(true);
		}
		else
		{
			GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneGacha/GUI/Prefab/GUI_GachaResult"), base.transform);
			this.guiDataResult = new SelGachaCtrl.GUIResult(gameObject.transform);
			this.guiDataResult.Btn_Friends.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickGachaResultFriendsBtn), PguiButtonCtrl.SoundType.DEFAULT);
			this.guiDataResult.Btn_Next.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickGachaResultNextBtn), PguiButtonCtrl.SoundType.DEFAULT);
			this.guiDataResult.Btn_Party.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickGachaResultPartyBtn), PguiButtonCtrl.SoundType.DEFAULT);
			this.guiDataResult.Btn_Onemore.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickGachaResultOnemoreBtn), PguiButtonCtrl.SoundType.DEFAULT);
			this.guiDataResult.Btn_BoxInfo.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickBoxCheckButtonResult), PguiButtonCtrl.SoundType.DEFAULT);
			this.guiDataResult.Btn_StepInfo.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickStepCheckButton), PguiButtonCtrl.SoundType.DEFAULT);
			this.guiDataResult.Btn_TreeHouse.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickTreeHouseButton), PguiButtonCtrl.SoundType.DEFAULT);
		}
		foreach (SelGachaCtrl.GUIResult.ResultIconBase resultIconBase in this.guiDataResult.ResultIconBaseList)
		{
			resultIconBase.MarkNew.gameObject.SetActive(false);
			resultIconBase.Base.gameObject.SetActive(false);
			resultIconBase.BaseExchange.gameObject.SetActive(false);
			resultIconBase.IconReplaceTr.GetComponent<GridLayoutGroup>().cellSize = new Vector2(100f, 100f);
		}
		int num = 0;
		using (List<DataManagerGacha.PlayResult.OneData>.Enumerator enumerator2 = result.gachaResult.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				DataManagerGacha.PlayResult.OneData gachaResult = enumerator2.Current;
				SelGachaCtrl.GUIResult.ResultIconBase resultIconBase2 = this.guiDataResult.ResultIconBaseList[num];
				num++;
				SimpleAnimation component = resultIconBase2.baseObj.GetComponent<SimpleAnimation>();
				component.ExPauseAnimation(SimpleAnimation.ExPguiStatus.LOOP, null);
				ItemDef.Kind kind = ItemDef.Id2Kind(gachaResult.itemId);
				if (kind != ItemDef.Kind.CHARA)
				{
					if (kind != ItemDef.Kind.PHOTO)
					{
						if (kind != ItemDef.Kind.TREEHOUSE_FURNITURE)
						{
							GameObject gameObject2 = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, resultIconBase2.IconBaseTr);
							resultIconBase2.IconBaseTr.GetComponent<CanvasGroup>().blocksRaycasts = !gachaResult.replaced;
							this.resultIconObjList.Add(gameObject2);
							gameObject2.GetComponent<IconItemCtrl>().Setup(DataManager.DmItem.GetItemStaticBase(gachaResult.itemId), gachaResult.replaced ? 0 : gachaResult.itemNum, new IconItemCtrl.SetupParam
							{
								useInfo = true,
								viewItemCount = false
							});
							resultIconBase2.Base.gameObject.SetActive(true);
							resultIconBase2.Base.SetSiblingIndex(0);
							if (gachaResult.replaced)
							{
								GameObject gameObject3 = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, resultIconBase2.IconReplaceTr);
								this.resultIconObjList.Add(gameObject3);
								IconItemCtrl component2 = gameObject3.GetComponent<IconItemCtrl>();
								component2.Setup(DataManager.DmItem.GetItemStaticBase(gachaResult.replaceItem.id), gachaResult.replaceItem.num, new IconItemCtrl.SetupParam
								{
									useInfo = true,
									viewItemCount = false
								});
								component2.DispNew(gachaResult.replaceItemIsNew);
								resultIconBase2.BaseExchange.gameObject.SetActive(true);
								resultIconBase2.BaseExchange.SetSiblingIndex(0);
								component.ExPlayAnimation(SimpleAnimation.ExPguiStatus.LOOP, null);
							}
						}
						else
						{
							GameObject gameObject4 = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, resultIconBase2.IconBaseTr);
							resultIconBase2.IconBaseTr.GetComponent<CanvasGroup>().blocksRaycasts = !gachaResult.replaced;
							this.resultIconObjList.Add(gameObject4);
							IconItemCtrl component3 = gameObject4.GetComponent<IconItemCtrl>();
							component3.Setup(DataManager.DmItem.GetItemStaticBase(gachaResult.itemId), gachaResult.replaced ? 0 : gachaResult.itemNum, new IconItemCtrl.SetupParam());
							component3.AddOnLongClickListener(delegate(IconItemCtrl x)
							{
								TreeHouseFurnitureStatic treeHouseFurnitureStaticData = DataManager.DmTreeHouse.GetTreeHouseFurnitureStaticData(gachaResult.itemId);
								CanvasManager.HdlTreeHouseFurnitureWindowCtrl.Open(new TreeHouseFurnitureWindowCtrl.SetupParam
								{
									thfs = treeHouseFurnitureStaticData
								});
							});
							resultIconBase2.Base.gameObject.SetActive(true);
							resultIconBase2.Base.SetSiblingIndex(0);
							if (gachaResult.replaced)
							{
								GameObject gameObject5 = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, resultIconBase2.IconReplaceTr);
								this.resultIconObjList.Add(gameObject5);
								IconItemCtrl component4 = gameObject5.GetComponent<IconItemCtrl>();
								component4.Setup(DataManager.DmItem.GetItemStaticBase(gachaResult.replaceItem.id), gachaResult.replaceItem.num, new IconItemCtrl.SetupParam
								{
									useInfo = true,
									viewItemCount = false
								});
								component4.DispNew(gachaResult.replaceItemIsNew);
								resultIconBase2.BaseExchange.gameObject.SetActive(true);
								resultIconBase2.BaseExchange.SetSiblingIndex(0);
								component.ExPlayAnimation(SimpleAnimation.ExPguiStatus.LOOP, null);
							}
						}
					}
					else
					{
						GameObject gameObject6 = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Photo, resultIconBase2.IconBaseTr);
						resultIconBase2.IconBaseTr.GetComponent<CanvasGroup>().blocksRaycasts = !gachaResult.replaced;
						this.resultIconObjList.Add(gameObject6);
						gameObject6.GetComponent<IconPhotoCtrl>().Setup(new IconPhotoCtrl.SetupParam
						{
							ppd = PhotoPackData.MakeMaximum(gachaResult.itemId, false, false)
						});
						gameObject6.transform.Find("All/Top/Txt").gameObject.SetActive(false);
					}
				}
				else
				{
					GameObject gameObject7 = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Chara, resultIconBase2.IconBaseTr);
					resultIconBase2.IconBaseTr.GetComponent<CanvasGroup>().blocksRaycasts = !gachaResult.replaced;
					this.resultIconObjList.Add(gameObject7);
					IconCharaCtrl component5 = gameObject7.GetComponent<IconCharaCtrl>();
					component5.transform.SetSiblingIndex(0);
					component5.Setup(DataManager.DmChara.GetUserCharaData(gachaResult.itemId), SortFilterDefine.SortType.LEVEL, false, new CharaWindowCtrl.DetailParamSetting(CharaWindowCtrl.DetailParamSetting.Preset.GACHA_RESULT, null), 0, -1, 0);
					component5.DispMarkAccessory(false);
					gameObject7.transform.Find("Txt_Lv").gameObject.SetActive(false);
					if (gachaResult.replaced)
					{
						GameObject gameObject8 = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, resultIconBase2.IconReplaceTr);
						if (gachaResult.replaceItemEx != null)
						{
							gameObject8.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
						}
						this.resultIconObjList.Add(gameObject8);
						IconItemCtrl component6 = gameObject8.GetComponent<IconItemCtrl>();
						component6.Setup(DataManager.DmItem.GetItemStaticBase(gachaResult.replaceItem.id), gachaResult.replaceItem.num, new IconItemCtrl.SetupParam
						{
							useInfo = true,
							viewItemCount = false
						});
						component6.DispNew(gachaResult.replaceItemIsNew);
						if (gachaResult.replaceItemEx != null)
						{
							resultIconBase2.IconReplaceTr.GetComponent<GridLayoutGroup>().cellSize = new Vector2(80f, 80f);
							GameObject gameObject9 = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, resultIconBase2.IconReplaceTr);
							gameObject9.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
							this.resultIconObjList.Add(gameObject9);
							IconItemCtrl component7 = gameObject9.GetComponent<IconItemCtrl>();
							component7.Setup(DataManager.DmItem.GetItemStaticBase(gachaResult.replaceItemEx.id), gachaResult.replaceItemEx.num, new IconItemCtrl.SetupParam
							{
								useInfo = true,
								viewItemCount = false
							});
							component7.DispNew(gachaResult.replaceItemExIsNew);
						}
						resultIconBase2.BaseExchange.gameObject.SetActive(true);
						resultIconBase2.BaseExchange.SetSiblingIndex(0);
						component.ExPlayAnimation(SimpleAnimation.ExPguiStatus.LOOP, null);
					}
				}
				if (gachaResult.isNew)
				{
					resultIconBase2.MarkNew.gameObject.SetActive(true);
				}
			}
		}
		HashSet<int> hashSet = new HashSet<int>();
		bool flag = false;
		bool flag2 = false;
		List<ItemData> list = new List<ItemData>();
		if (result.GetBonusOne)
		{
			flag |= result.bonusOne.replaced;
			flag2 |= result.bonusOne.isPresentBox;
			if (result.bonusOne.isNew)
			{
				hashSet.Add(result.bonusOne.itemData.id);
			}
			list.Add(result.bonusOne.itemData);
		}
		if (result.GetBonusSet)
		{
			bool flag3 = result.bonusSet.Exists((DataManagerGacha.PlayResult.BonusOneData x) => x.replaced);
			flag = flag || flag3;
			bool flag4 = result.bonusSet.Exists((DataManagerGacha.PlayResult.BonusOneData x) => x.isPresentBox);
			flag2 = flag2 || flag4;
			hashSet.UnionWith(result.bonusSet.FindAll((DataManagerGacha.PlayResult.BonusOneData x) => x.isNew).ConvertAll<int>((DataManagerGacha.PlayResult.BonusOneData y) => y.itemData.id));
			list.AddRange(result.bonusSet.ConvertAll<ItemData>((DataManagerGacha.PlayResult.BonusOneData x) => x.itemData));
		}
		if (result.GetPrizeBonus)
		{
			bool flag5 = result.prizeBonusList.Exists((DataManagerGacha.PlayResult.BonusOneData x) => x.replaced);
			flag = flag || flag5;
			bool flag6 = result.prizeBonusList.Exists((DataManagerGacha.PlayResult.BonusOneData x) => x.isPresentBox);
			flag2 = flag2 || flag6;
			hashSet.UnionWith(result.prizeBonusList.FindAll((DataManagerGacha.PlayResult.BonusOneData x) => x.isNew).ConvertAll<int>((DataManagerGacha.PlayResult.BonusOneData y) => y.itemData.id));
			list.AddRange(result.prizeBonusList.ConvertAll<ItemData>((DataManagerGacha.PlayResult.BonusOneData x) => x.itemData));
		}
		if (0 < list.Count)
		{
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			foreach (ItemData itemData in list)
			{
				if (dictionary.ContainsKey(itemData.id))
				{
					Dictionary<int, int> dictionary2 = dictionary;
					int id = itemData.id;
					dictionary2[id] += itemData.num;
				}
				else
				{
					dictionary.Add(itemData.id, itemData.num);
				}
			}
			List<ItemData> list2 = new List<ItemData>();
			foreach (KeyValuePair<int, int> keyValuePair in dictionary)
			{
				list2.Add(new ItemData(keyValuePair.Key, keyValuePair.Value));
			}
			PrjUtil.InsertionSort<ItemData>(ref list2, (ItemData a, ItemData b) => a.id.CompareTo(b.id));
			string text = "以下のおまけを入手しました。";
			if (flag)
			{
				text += "\n<size=20><color=red>※一部のアイテムは獲得済みの為、代替アイテムに自動変換されました。</color></size>";
			}
			if (flag2)
			{
				text += "\n<size=20><color=red>※所持数上限を超えたアイテムはプレゼントボックスに移動しました。</color></size>";
			}
			GetMultiItemWindowCtrl.SetupParam setupParam = new GetMultiItemWindowCtrl.SetupParam
			{
				titleText = "おまけゲット！",
				messageText = text,
				innerTitleText = "入手したおまけ",
				dispNewItemIdSet = hashSet
			};
			setupParam.callBack = delegate(int x)
			{
				this.guiDataResult.IsOpenBonusItemWindow = false;
				return true;
			};
			CanvasManager.HdlGetItemSetWindowCtrl.Setup(list2, setupParam, false, 0);
			this.guiDataResult.IsOpenBonusItemWindow = true;
			CanvasManager.HdlGetItemSetWindowCtrl.Open();
		}
		this.guiDataResult.Btn_Party.gameObject.SetActive(!gachaStaticData.IsResultTreeHouseFurnitureInfo);
		this.guiDataResult.Btn_Friends.gameObject.SetActive(!gachaStaticData.IsResultTreeHouseFurnitureInfo);
		this.guiDataResult.Btn_TreeHouse.gameObject.SetActive(gachaStaticData.IsResultTreeHouseFurnitureInfo);
		this.guiDataResult.Btn_BoxInfo.gameObject.SetActive(DataManagerGacha.Category.Box == gachaStaticData.gachaCategory);
		this.guiDataResult.Btn_StepInfo.gameObject.SetActive(DataManagerGacha.Category.StepUp == gachaStaticData.gachaCategory);
		this.guiDataResult.Btn_StepInfo.SetActEnable(this.lastSelectGacha.stepNextGachaId != 0, false, false);
		this.UpdateGachaResultOneMoreBtn(gachaStaticData);
		this.guiDataResult.ResultEffect.SetActive(4 <= result.highestRarity);
		CanvasManager.HdlMissionProgressCtrl.IsPlayingGachaAuth = false;
	}

	private void ClearResultIconList()
	{
		foreach (GameObject gameObject in this.resultIconObjList)
		{
			Object.Destroy(gameObject);
		}
		this.resultIconObjList.Clear();
	}

	public void SetIconAttr(int charaId, ref PguiImageCtrl Icon_Atr)
	{
		CharaStaticBase baseData = DataManager.DmChara.GetCharaStaticData(charaId).baseData;
		Icon_Atr.SetImageByName(IconCharaCtrl.Attribute2IconName(baseData.attribute));
	}

	public void SetIconSubAttr(int charaId, ref PguiImageCtrl iconAtr, ref RectTransform rankStar, Vector2 initialRankStarPos)
	{
		CharaStaticBase baseData = DataManager.DmChara.GetCharaStaticData(charaId).baseData;
		if (baseData.subAttribute <= CharaDef.AttributeType.ALL)
		{
			rankStar.anchoredPosition = initialRankStarPos - new Vector2(30f, 0f);
			iconAtr.gameObject.SetActive(false);
			return;
		}
		rankStar.anchoredPosition = initialRankStarPos;
		iconAtr.SetImageByName(IconCharaCtrl.SubAttribute2IconName(baseData.subAttribute));
		iconAtr.gameObject.SetActive(true);
	}

	private void UpdateCenterInfoDispIndex(bool isReset)
	{
		if (isReset)
		{
			this.centerInfoDispIndex = 0;
		}
		else
		{
			this.centerInfoDispIndex++;
			if (this.centerInfoDispIdList.Count == this.centerInfoDispIndex)
			{
				this.centerInfoDispIndex = 0;
			}
			else if (this.centerInfoDispIdList.Count < this.centerInfoDispIndex)
			{
				this.centerInfoDispIndex = 0;
			}
		}
		this.centerInfoDispTimeElapsed = 0f;
	}

	private int GetCenterInfoDispItemId()
	{
		return this.centerInfoDispIdList[this.centerInfoDispIndex];
	}

	private void UpdateCenterInfoDisp(int dispItemId)
	{
		ItemDef.Kind kind = ItemDef.Id2Kind(dispItemId);
		if (kind == ItemDef.Kind.CHARA)
		{
			this.guiDataTop.DispPhotoInfo.SetActive(false);
			this.guiDataTop.PhotoInfoAll.SetActive(false);
			this.guiDataTop.FurnitureInfoAll.SetActive(false);
			CharaStaticData charaStaticData = DataManager.DmChara.GetCharaStaticData(dispItemId);
			this.guiDataTop.InfoDispCharaName.text = charaStaticData.baseData.charaName;
			bool flag = false;
			if (!string.IsNullOrEmpty(charaStaticData.baseData.NickName))
			{
				this.guiDataTop.InfoDispWName.text = charaStaticData.baseData.NickName;
				flag = true;
			}
			this.guiDataTop.InfoDispWName.gameObject.SetActive(flag);
			this.SetIconAttr(dispItemId, ref this.guiDataTop.InfoDispCharaAttrIcon);
			this.SetIconSubAttr(dispItemId, ref this.guiDataTop.InfoDispCharaSubAttrIcon, ref this.guiDataTop.CharaRankStarRectTransform, this.guiDataTop.CharaRankStarInitialPosition);
			this.guiDataTop.InfoDispCharaMiracleName.text = charaStaticData.artsData.actionName;
			for (int i = 0; i < this.guiDataTop.Chara_IconStar.Count; i++)
			{
				this.guiDataTop.Chara_IconStar[i].gameObject.SetActive(i < charaStaticData.baseData.rankLow);
			}
			PguiReplaceAECtrl component = this.guiDataTop.Chara_RankStar_AE.GetComponent<PguiReplaceAECtrl>();
			component.InitForce();
			component.Replace(charaStaticData.baseData.rankLow.ToString("D2"));
			this.guiDataTop.CharaInfoAll.SetActive(true);
			CharaMotionDefine.ActKey actKey = CharaMotionDefine.ActKey.GACHA_ST;
			CharaMotionDefine.ActKey loopMotion = CharaMotionDefine.ActKey.GACHA_LP;
			this.RenderCharaCenter.Setup(dispItemId, 0, actKey, 0, false, false, delegate
			{
				this.RenderCharaCenter.SetAnimation(loopMotion, true);
			}, false, null, 2.4f, null, true, false, false);
			this.RenderCharaCenter.gameObject.SetActive(true);
			this.guiDataTop.Chara_TextAEImage.PlayAnimation(PguiAECtrl.AmimeType.START, null);
			this.guiDataTop.Chara_RankStar_AE.PlayAnimation(PguiAECtrl.AmimeType.START, null);
			this.cloneCharaMovieObject = this.RenderCharaCenter.GetCloneCharaMovieObject();
			this.movieTime = 0f;
			if (this.cloneCharaMovieObject == null)
			{
				GameObject gameObject = this.guiDataTop.Chara_MovieImage.gameObject;
				string text = "Gacha/Gacha_movie_" + dispItemId.ToString("0000");
				if (AssetManager.IsExsistAssetData(AssetManager.PREFIX_PATH_MOVIE + text))
				{
					gameObject.SetActive(true);
					MoviePlayer.Play(gameObject, text, false);
					this.guiDataTop.Chara_MovieImage.SetRaycastTarget(false);
				}
				else
				{
					gameObject.SetActive(false);
				}
			}
			this.guiDataTop.AEImage_Loading.gameObject.SetActive(true);
			return;
		}
		if (kind == ItemDef.Kind.PHOTO)
		{
			this.RenderCharaCenter.gameObject.SetActive(false);
			this.guiDataTop.CharaInfoAll.SetActive(false);
			this.guiDataTop.FurnitureInfoAll.SetActive(false);
			this.guiDataTop.AEImage_Loading.gameObject.SetActive(false);
			PhotoStaticData photoStaticData = DataManager.DmPhoto.GetPhotoStaticData(dispItemId);
			this.guiDataTop.PhotoName_Text.text = photoStaticData.baseData.photoName;
			this.guiDataTop.Icon_PhotoKind.InitForce();
			Image component2 = this.guiDataTop.Icon_PhotoKind.GetComponent<Image>();
			if (photoStaticData.baseData.type == PhotoDef.Type.OTHER)
			{
				component2.enabled = false;
			}
			else
			{
				component2.enabled = true;
				this.guiDataTop.Icon_PhotoKind.Replace((int)photoStaticData.baseData.type);
			}
			this.guiDataTop.PhotoInfo_Text.text = ((null != photoStaticData.abilityData) ? photoStaticData.abilityData.abilityEffect : "");
			for (int j = 0; j < this.guiDataTop.Photo_IconStar.Count; j++)
			{
				this.guiDataTop.Photo_IconStar[j].gameObject.SetActive(j < (int)photoStaticData.baseData.rarity);
			}
			PhotoPackData photoPackData = PhotoPackData.MakeMaximum(dispItemId, false, true);
			photoPackData.dynamicData.level = photoPackData.limitLevel;
			PrjUtil.ParamPreset paramPreset = PrjUtil.CalcParamByPhoto(photoPackData);
			this.guiDataTop.PhotoHpTextCtrl.text = paramPreset.hp.ToString();
			this.guiDataTop.PhotoAtkTextCtrl.text = paramPreset.atk.ToString();
			this.guiDataTop.PhotoDefTextCtrl.text = paramPreset.def.ToString();
			this.guiDataTop.PhotoEventInfo.gameObject.SetActive(false);
			List<int> photoBonusTargetItemIdByTime = DataManager.DmPhoto.GetPhotoBonusTargetItemIdByTime(photoStaticData.GetId(), TimeManager.Now);
			this.guiDataTop.PhotoDropPopUp.gameObject.SetActive(0 < photoBonusTargetItemIdByTime.Count);
			this.guiDataTop.PhotoInfoAll.SetActive(true);
			this.guiDataTop.DispPhotoInfo.SetActive(true);
			this.guiDataTop.PhotoCard.Setup(photoStaticData, SortFilterDefine.SortType.LEVEL, false, false, -1, 0, false);
			this.guiDataTop.PhotoInfo_AEImage.PlayAnimation(PguiAECtrl.AmimeType.START, null);
			this.guiDataTop.DispPhotoInfo.transform.Find("Card").gameObject.GetComponent<SimpleAnimation>().Play("LOOP");
			return;
		}
		if (kind != ItemDef.Kind.TREEHOUSE_FURNITURE)
		{
			this.guiDataTop.DispPhotoInfo.SetActive(false);
			this.guiDataTop.PhotoInfoAll.SetActive(false);
			this.RenderCharaCenter.gameObject.SetActive(false);
			this.guiDataTop.CharaInfoAll.SetActive(false);
			this.guiDataTop.FurnitureInfoAll.SetActive(false);
			this.guiDataTop.AEImage_Loading.gameObject.SetActive(false);
			return;
		}
		this.RenderCharaCenter.gameObject.SetActive(false);
		this.guiDataTop.CharaInfoAll.SetActive(false);
		this.guiDataTop.DispPhotoInfo.SetActive(false);
		this.guiDataTop.PhotoInfoAll.SetActive(false);
		TreeHouseFurnitureStatic treeHouseFurnitureStaticData = DataManager.DmTreeHouse.GetTreeHouseFurnitureStaticData(dispItemId);
		this.guiDataTop.FurnitureRarity_Text.text = PrjUtil.Rarity2String((int)treeHouseFurnitureStaticData.GetRarity());
		this.guiDataTop.FurnitureName_Text.text = treeHouseFurnitureStaticData.GetName();
		this.guiDataTop.FurnitureInfo_AEImage.PlayAnimation(PguiAECtrl.AmimeType.START, null);
		this.guiDataTop.FurnitureInfoAll.SetActive(true);
		GameObject gameObject2 = this.guiDataTop.Furniture_MovieImage.gameObject;
		string text2 = "Gacha/InteriorGacha_movie_" + dispItemId.ToString("000000");
		if (AssetManager.IsExsistAssetData(AssetManager.PREFIX_PATH_MOVIE + text2))
		{
			gameObject2.SetActive(true);
			MoviePlayer.Play(gameObject2, text2, false);
			this.guiDataTop.Furniture_MovieImage.SetRaycastTarget(false);
		}
		else
		{
			gameObject2.SetActive(false);
		}
		this.guiDataTop.AEImage_Loading.gameObject.SetActive(true);
	}

	private void SetDisplayCmnMenu(bool disped)
	{
		CanvasManager.HdlCmnMenu.SetupMenu(disped, "しょうたい", delegate
		{
			SceneManager.SceneName sceneName = SceneManager.SceneName.SceneHome;
			object obj = null;
			if (this.sceneOpenParam != null && this.sceneOpenParam.resultNextSceneName != SceneManager.SceneName.None)
			{
				sceneName = this.sceneOpenParam.resultNextSceneName;
				obj = this.sceneOpenParam.resultNextSceneArgs;
			}
			this.requestNextSceneCb(sceneName, obj);
		}, "", null, null);
	}

	private IEnumerator RefreshGachaTop(bool getServerdata)
	{
		if (getServerdata)
		{
			DataManager.DmGacha.RequestGetGachaList();
			while (DataManager.IsServerRequesting())
			{
				yield return null;
			}
		}
		this.guiDataTop.TabGroup.SelectTab(this.currentGachaTab);
		this.RefreshGachaPackDataList(this.currentGachaPackData.gachaId, true, false, this.currentGachaTab);
		yield break;
	}

	private IEnumerator BoxReset(int id)
	{
		DataManager.DmGacha.RequestActionGachaReset(id);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		this.guiDataTop.RefreshGachaTop = this.RefreshGachaTop(true);
		CanvasManager.HdlOpenWindowBasic.Setup("確認", "ぼっくすを切り替えました", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int index) => true, null, false);
		CanvasManager.HdlOpenWindowBasic.Open();
		yield return null;
		yield break;
	}

	private IEnumerator PopupInitialWaitSync()
	{
		while (this.guiDataTop.GachaBtnSet_Left.IsPlaying() || this.guiDataTop.GachaBtnSet_Right.IsPlaying())
		{
			yield return null;
		}
		this.guiDataTop.GachaBtnSet_Left.StartPlayPopup();
		this.guiDataTop.GachaBtnSet_Right.StartPlayPopup();
		yield break;
	}

	private IEnumerator GachaPlayAction(int gachaId, int gachaType, int useItemId)
	{
		this.guiDataTop.baseObj.SetActive(false);
		SelGachaCtrl.GUIResult guiresult = this.guiDataResult;
		if (guiresult != null)
		{
			guiresult.baseObj.SetActive(false);
		}
		if (this.gachaAuth == null)
		{
			this.gachaAuth = new GachaAuthCtrl();
			this.gachaAuth.Initialize();
		}
		CanvasManager.HdlTutorialMaskCtrl.SetEnable(false);
		CanvasManager.HdlTutorialMaskCtrl.SetBlackMask(false);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetStickCursor(false, null, null, null);
		bool isTutorial = this.IsTutorial;
		DataManagerGacha.PlayResult playResult;
		if (!isTutorial)
		{
			DataManagerGameStatus.UserFlagData userFlagData = DataManager.DmGameStatus.MakeUserFlagData();
			if (userFlagData.GachaNewInfoData.RegisterIDs(DataManager.DmGacha.SelectedGachaIdHashSet))
			{
				DataManager.DmGameStatus.RequestActionUpdateUserFlag(userFlagData);
				while (DataManager.IsServerRequesting())
				{
					yield return null;
				}
			}
			CanvasManager.HdlMissionProgressCtrl.IsPlayingGachaAuth = true;
			DataManager.DmGacha.RequestActionPlayGacha(gachaId, gachaType, useItemId);
			while (DataManager.IsServerRequesting())
			{
				if (DataManager.GetServerErrorType() == ActionTypeMask.REFRESH)
				{
					CanvasManager.HdlMissionProgressCtrl.IsPlayingGachaAuth = false;
					yield break;
				}
				yield return null;
			}
			playResult = DataManager.DmGacha.GetLastPlayGachaResult();
		}
		else
		{
			playResult = new DataManagerGacha.PlayResult();
			playResult.gachaResult.Add(new DataManagerGacha.PlayResult.OneData
			{
				itemId = 29,
				isNew = true
			});
			DataManager.DmGacha.SetLastPlayGachaResultByTutorial(playResult);
		}
		if (playResult == null || playResult.gachaResult.Count == 0)
		{
			yield break;
		}
		this.RenderCharaCenter.gameObject.SetActive(false);
		this.ClearResultIconList();
		this.SetDisplayCmnMenu(false);
		IEnumerator enumerator = this.gachaAuth.SetupPlayAuth(playResult, isTutorial);
		yield return Singleton<SceneManager>.Instance.StartCoroutine(enumerator);
		if (!isTutorial)
		{
			DataManager.DmGacha.RequestGetGachaList();
		}
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		if (this.gachaAuth == null)
		{
			yield break;
		}
		this.RefreshGachaPackDataList(gachaId, false, false, this.currentGachaTab);
		this.SetupGachaResult(playResult);
		if (isTutorial)
		{
			this.gotoNextStepByTutorial = true;
		}
		yield break;
	}

	private IEnumerator Tutorial()
	{
		CanvasManager.HdlTutorialMaskCtrl.SetEnable(true);
		CanvasManager.HdlTutorialMaskCtrl.SetBlackMask(true);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
		this.gotoNextStepByTutorial = false;
		CanvasManager.HdlTutorialMaskCtrl.SetCharaInfo(new TutorialMaskCtrl.CharaInfoParameter
		{
			dispType = TutorialMaskCtrl.CharaDispType.IN,
			postion = new Vector2?(new Vector2(200f, 500f)),
			charaImagePath = "Texture2D/Icon_Chara/Chara/icon_chara_0001",
			messageList = new List<string> { "招待状の送り方、隊長さんにも教えてあげますね！" },
			finishCallBack = delegate
			{
				this.gotoNextStepByTutorial = true;
			}
		});
		while (!this.gotoNextStepByTutorial)
		{
			yield return null;
		}
		this.gotoNextStepByTutorial = false;
		CanvasManager.HdlTutorialMaskCtrl.SetCharaInfo(new TutorialMaskCtrl.CharaInfoParameter
		{
			dispType = TutorialMaskCtrl.CharaDispType.IN_QUICK,
			postion = new Vector2?(new Vector2(200f, 500f)),
			messageList = new List<string> { "ここをおして、お手紙を送ります！\n覚えててくださいね！" },
			finishCallBack = delegate
			{
				this.gotoNextStepByTutorial = true;
			}
		});
		while (!this.gotoNextStepByTutorial)
		{
			yield return null;
		}
		this.gotoNextStepByTutorial = false;
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, this.guiDataTop.GachaBtnSet_Right.BaseButton.transform as RectTransform, true, 1f, 1f);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetStickCursor(this.guiDataTop.GachaBtnSet_Right.BaseButton.transform as RectTransform, 1f);
		while (!this.gotoNextStepByTutorial)
		{
			yield return null;
		}
		CanvasManager.HdlTutorialMaskCtrl.SetCharaInfo(new TutorialMaskCtrl.CharaInfoParameter
		{
			dispType = TutorialMaskCtrl.CharaDispType.OUT_QUICK,
			finishCallBack = delegate
			{
				this.gotoNextStepByTutorial = true;
			}
		});
		while (!this.gotoNextStepByTutorial)
		{
			yield return null;
		}
		CanvasManager.HdlTutorialMaskCtrl.SetStickCursor(false, null, null, null);
		this.OpenExecuteGachaConfirmWindow(false, this.currentGachaPackData);
		while (!CanvasManager.HdlOpenWindowUseItem.FinishedOpen())
		{
			yield return null;
		}
		this.gotoNextStepByTutorial = false;
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, CanvasManager.HdlOpenWindowUseItem.GetButtonRectTransform(2), true, 1f, 1f);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetStickCursor(CanvasManager.HdlOpenWindowUseItem.GetButtonRectTransform(2), 1f);
		while (!this.gotoNextStepByTutorial)
		{
			yield return null;
		}
		CanvasManager.HdlTutorialMaskCtrl.SetEnable(true);
		CanvasManager.HdlTutorialMaskCtrl.SetBlackMask(true);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
		this.gotoNextStepByTutorial = false;
		CanvasManager.HdlTutorialMaskCtrl.SetCharaInfo(new TutorialMaskCtrl.CharaInfoParameter
		{
			dispType = TutorialMaskCtrl.CharaDispType.IN,
			postion = new Vector2?(new Vector2(200f, 500f)),
			charaImagePath = "Texture2D/Icon_Chara/Chara/icon_chara_0001",
			messageList = new List<string> { "うわーい！！\n新しいフレンズさんが来てくれました！！" },
			finishCallBack = delegate
			{
				this.gotoNextStepByTutorial = true;
			}
		});
		while (!this.gotoNextStepByTutorial)
		{
			yield return null;
		}
		this.gotoNextStepByTutorial = false;
		CanvasManager.HdlTutorialMaskCtrl.SetCharaInfo(new TutorialMaskCtrl.CharaInfoParameter
		{
			dispType = TutorialMaskCtrl.CharaDispType.IN_QUICK,
			postion = new Vector2?(new Vector2(200f, 500f)),
			messageList = new List<string> { "はじめまして！\nこれからよろしくお願いします！！" },
			finishCallBack = delegate
			{
				this.gotoNextStepByTutorial = true;
			}
		});
		while (!this.gotoNextStepByTutorial)
		{
			yield return null;
		}
		TutorialUtil.RequestNextSequence(this.sceneOpenParam.tutorialSequence);
		CanvasManager.HdlTutorialMaskCtrl.SetEnable(false);
		CanvasManager.HdlTutorialMaskCtrl.SetBlackMask(false);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(0, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetFrame(1, false, null, true, null);
		CanvasManager.HdlTutorialMaskCtrl.SetStickCursor(false, null, null, null);
		yield return null;
		yield break;
	}

	private void OnClickGachaBannerImg(int index)
	{
		if (this.isRequestingNextSceneCb())
		{
			return;
		}
		if (this.currentBannerBtnIndex == index)
		{
			return;
		}
		this.ResetBannerSize();
		this.guiDataTop.GachaBannerScroll.ChangeFocusItem(index, false);
	}

	private void OnClickGachaTopBtnLeft(PguiButtonCtrl button)
	{
		if (this.isRequestingNextSceneCb())
		{
			return;
		}
		if (this.IsTutorial)
		{
			this.gotoNextStepByTutorial = true;
			return;
		}
		if (this.guiDataTop.GachaBannerScroll.IsMoving)
		{
			return;
		}
		this.OpenExecuteGachaConfirmWindow(true, this.currentGachaPackData);
	}

	private void OnClickGachaTopBtnRight(PguiButtonCtrl button)
	{
		if (this.isRequestingNextSceneCb())
		{
			return;
		}
		if (this.IsTutorial)
		{
			this.gotoNextStepByTutorial = true;
			return;
		}
		if (this.guiDataTop.GachaBannerScroll.IsMoving)
		{
			return;
		}
		this.OpenExecuteGachaConfirmWindow(false, this.currentGachaPackData);
	}

	private void OnClickGachaTopTouchPanel(PguiButtonCtrl button)
	{
		if (this.isRequestingNextSceneCb())
		{
			return;
		}
		if (this.guiDataTop.GachaBannerScroll.IsMoving)
		{
			return;
		}
		if (!this.CenterInfoDisplayedBanner)
		{
			this.ChangeCenterInfo(false);
		}
	}

	private void OnClickGachaTopReloadBtn(PguiButtonCtrl button)
	{
		if (this.isRequestingNextSceneCb())
		{
			return;
		}
		if (this.IsTutorial)
		{
			this.gotoNextStepByTutorial = true;
			return;
		}
		if (this.guiDataTop.GachaBannerScroll.IsMoving)
		{
			return;
		}
		this.ChangeCenterInfo(!this.CenterInfoDisplayedBanner);
	}

	private void OnClickGachaTopGachaDetailInfoBtn(PguiButtonCtrl button)
	{
		if (this.isRequestingNextSceneCb())
		{
			return;
		}
		if (this.IsTutorial)
		{
			this.gotoNextStepByTutorial = true;
			return;
		}
		if (this.guiDataTop.GachaBannerScroll.IsMoving)
		{
			return;
		}
		DataManager.DmGacha.RequestActionRateView(this.currentGachaPackData.gachaId);
		this.CreateDetailWindow(this.currentGachaPackData);
	}

	private void OnClickGachaTopCharaInfoBtn(PguiButtonCtrl button)
	{
		if (this.isRequestingNextSceneCb())
		{
			return;
		}
		if (this.guiDataTop.GachaBannerScroll.IsMoving)
		{
			return;
		}
		CharaPackData charaPackData = CharaPackData.MakeInitial(this.GetCenterInfoDispItemId());
		DataManager.DmChara.GetCharaStaticData(this.GetCenterInfoDispItemId());
		CanvasManager.HdlCharaWindowCtrl.Open(charaPackData, new CharaWindowCtrl.DetailParamSetting(CharaWindowCtrl.DetailParamSetting.Preset.DISPLAY, null), delegate
		{
			this.pauseCenterInfoDispLoop = false;
		});
		this.pauseCenterInfoDispLoop = true;
	}

	private void OnClickGachaTopPhotoInfoBtn(PguiButtonCtrl button)
	{
		if (this.isRequestingNextSceneCb())
		{
			return;
		}
		if (this.guiDataTop.GachaBannerScroll.IsMoving)
		{
			return;
		}
		PhotoPackData photoPackData = PhotoPackData.MakeMaximum(this.GetCenterInfoDispItemId(), false, true);
		photoPackData.dynamicData.level = photoPackData.limitLevel;
		this.pauseCenterInfoDispLoop = true;
		CanvasManager.HdlPhotoWindowCtrl.Open(new PhotoWindowCtrl.SetupParam
		{
			ppd = photoPackData,
			closeWindowCB = delegate
			{
				this.pauseCenterInfoDispLoop = false;
			},
			dispMax = true
		});
	}

	private void OnClickGachaTopFurnitureInfoBtn(PguiButtonCtrl button)
	{
		if (this.isRequestingNextSceneCb())
		{
			return;
		}
		if (this.guiDataTop.GachaBannerScroll.IsMoving)
		{
			return;
		}
		this.pauseCenterInfoDispLoop = true;
		TreeHouseFurnitureStatic treeHouseFurnitureStaticData = DataManager.DmTreeHouse.GetTreeHouseFurnitureStaticData(this.GetCenterInfoDispItemId());
		CanvasManager.HdlTreeHouseFurnitureWindowCtrl.Open(new TreeHouseFurnitureWindowCtrl.SetupParam
		{
			thfs = treeHouseFurnitureStaticData,
			closeEndCb = delegate
			{
				this.pauseCenterInfoDispLoop = false;
			}
		});
	}

	private void OnClickGachaTopUseItemBtnTop(PguiButtonCtrl button)
	{
		if (this.isRequestingNextSceneCb())
		{
			return;
		}
		if (this.IsTutorial)
		{
			this.gotoNextStepByTutorial = true;
			return;
		}
		if (this.guiDataTop.GachaBannerScroll.IsMoving)
		{
			return;
		}
		this.OpenUseItemInfoWindow(this.guiDataTop.GachaUseItemBtnList[0].itemId);
	}

	private void OnClickGachaTopUseItemBtnMiddle(PguiButtonCtrl button)
	{
		if (this.isRequestingNextSceneCb())
		{
			return;
		}
		if (this.IsTutorial)
		{
			this.gotoNextStepByTutorial = true;
			return;
		}
		if (this.guiDataTop.GachaBannerScroll.IsMoving)
		{
			return;
		}
		this.OpenUseItemInfoWindow(this.guiDataTop.GachaUseItemBtnList[1].itemId);
	}

	private void OnClickGachaTopUseItemBtnBottom(PguiButtonCtrl button)
	{
		if (this.isRequestingNextSceneCb())
		{
			return;
		}
		if (this.IsTutorial)
		{
			this.gotoNextStepByTutorial = true;
			return;
		}
		if (this.guiDataTop.GachaBannerScroll.IsMoving)
		{
			return;
		}
		this.OpenUseItemInfoWindow(this.guiDataTop.GachaUseItemBtnList[2].itemId);
	}

	private void OnClickGachaResultOnemoreBtn(PguiButtonCtrl button)
	{
		if (this.isRequestingNextSceneCb())
		{
			return;
		}
		if (this.IsTutorial)
		{
			this.gotoNextStepByTutorial = true;
			return;
		}
		if (this.guiDataResult.IsOpenBonusItemWindow)
		{
			return;
		}
		DataManagerGacha.GachaPackData gachaPackData = this.gachaPackDataList.Find((DataManagerGacha.GachaPackData x) => x.gachaId == this.lastSelectGacha.id);
		if (gachaPackData == null && this.lastSelectGacha.isStepUp)
		{
			gachaPackData = this.gachaPackDataList.Find((DataManagerGacha.GachaPackData x) => x.gachaId == this.lastSelectGacha.stepNextGachaId);
		}
		this.OpenExecuteGachaConfirmWindow(this.lastSelectGacha.isLeftBtn, gachaPackData);
	}

	private void OnClickGachaResultFriendsBtn(PguiButtonCtrl button)
	{
		if (this.isRequestingNextSceneCb())
		{
			return;
		}
		if (this.IsTutorial)
		{
			this.gotoNextStepByTutorial = true;
			return;
		}
		if (this.guiDataResult.IsOpenBonusItemWindow)
		{
			return;
		}
		this.requestNextSceneCb(SceneManager.SceneName.SceneCharaEdit, null);
	}

	private void OnClickGachaResultPartyBtn(PguiButtonCtrl button)
	{
		if (this.isRequestingNextSceneCb())
		{
			return;
		}
		if (this.IsTutorial)
		{
			this.gotoNextStepByTutorial = true;
			return;
		}
		if (this.guiDataResult.IsOpenBonusItemWindow)
		{
			return;
		}
		SceneCharaEdit.Args args = new SceneCharaEdit.Args
		{
			requestMode = SceneCharaEdit.Mode.DECK
		};
		this.requestNextSceneCb(SceneManager.SceneName.SceneCharaEdit, args);
	}

	private void OnClickGachaResultNextBtn(PguiButtonCtrl button)
	{
		if (this.isRequestingNextSceneCb())
		{
			return;
		}
		if (this.IsTutorial)
		{
			this.gotoNextStepByTutorial = true;
			return;
		}
		if (this.guiDataResult.IsOpenBonusItemWindow)
		{
			return;
		}
		this.ChangeModeResultToTop();
	}

	private void ChangeModeResultToTop()
	{
		this.SetDisplayCmnMenu(true);
		this.ChangeCenterInfo(this.CenterInfoDisplayedBanner);
		this.guiDataTop.baseObj.SetActive(true);
		this.guiDataResult.baseObj.SetActive(false);
		DataManager.DmGacha.LatestGreetingVoice.Stop();
	}

	private void OnClickBoxResetButton(PguiButtonCtrl button)
	{
		if (this.isRequestingNextSceneCb())
		{
			return;
		}
		if (this.guiDataTop.GachaBannerScroll.IsMoving)
		{
			return;
		}
		if (DataManagerGacha.Category.Box != this.currentGachaPackData.staticData.gachaCategory)
		{
			return;
		}
		string gachaName = this.currentGachaPackData.staticData.gachaName;
		bool isReset = false;
		CanvasManager.HdlOpenWindowBasic.Setup("確認", "『ぼっくす』を切り替えます。\n\n新しい「ぼっくす」に切り替わります。\n新しい「ぼっくす」\nは最初からになりますがよろしいですか？", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, delegate(int index)
		{
			if (1 == index)
			{
				isReset = true;
			}
			return true;
		}, delegate
		{
			if (isReset)
			{
				this.guiDataTop.RefreshGachaTop = this.BoxReset(this.currentGachaPackData.gachaId);
			}
		}, false);
		CanvasManager.HdlOpenWindowBasic.Open();
	}

	private void OnClickBoxCheckButtonTop(PguiButtonCtrl button)
	{
		if (this.isRequestingNextSceneCb())
		{
			return;
		}
		if (this.guiDataTop.GachaBannerScroll.IsMoving)
		{
			return;
		}
		if (DataManagerGacha.Category.Box != this.currentGachaPackData.staticData.gachaCategory)
		{
			return;
		}
		DataManager.DmGacha.RequestActionRateView(this.currentGachaPackData.gachaId);
		this.pauseCenterInfoDispLoop = true;
		CanvasManager.HdlGachaWindowBoxInfoCtrl.Open(this.currentGachaPackData.staticData, null, delegate
		{
			this.pauseCenterInfoDispLoop = false;
		});
	}

	private void OnClickBoxCheckButtonResult(PguiButtonCtrl button)
	{
		if (this.isRequestingNextSceneCb())
		{
			return;
		}
		if (this.guiDataResult.IsOpenBonusItemWindow)
		{
			return;
		}
		DataManagerGacha.GachaPackData gachaPackData = this.gachaPackDataList.Find((DataManagerGacha.GachaPackData x) => x.gachaId == this.lastSelectGacha.id);
		if (gachaPackData == null || DataManagerGacha.Category.Box != gachaPackData.staticData.gachaCategory)
		{
			return;
		}
		DataManager.DmGacha.RequestActionRateView(gachaPackData.gachaId);
		this.pauseCenterInfoDispLoop = true;
		CanvasManager.HdlGachaWindowBoxInfoCtrl.Open(gachaPackData.staticData, null, delegate
		{
			this.pauseCenterInfoDispLoop = false;
		});
	}

	private void OnClickStepCheckButton(PguiButtonCtrl button)
	{
		if (this.isRequestingNextSceneCb())
		{
			return;
		}
		if (this.guiDataResult.IsOpenBonusItemWindow)
		{
			return;
		}
		DataManagerGacha.GachaPackData gachaPackData = this.gachaPackDataList.Find((DataManagerGacha.GachaPackData x) => x.gachaId == this.lastSelectGacha.id);
		if (gachaPackData == null)
		{
			gachaPackData = this.gachaPackDataList.Find((DataManagerGacha.GachaPackData x) => x.gachaId == this.lastSelectGacha.stepNextGachaId);
		}
		if (gachaPackData == null || DataManagerGacha.Category.StepUp != gachaPackData.staticData.gachaCategory)
		{
			return;
		}
		this.pauseCenterInfoDispLoop = true;
		CanvasManager.HdlGachaWindowStepInfoCtrl.Open(gachaPackData.staticData, null, delegate
		{
			this.pauseCenterInfoDispLoop = false;
		});
	}

	private void OnClickTreeHouseButton(PguiButtonCtrl button)
	{
		if (this.isRequestingNextSceneCb())
		{
			return;
		}
		if (this.guiDataResult.IsOpenBonusItemWindow)
		{
			return;
		}
		this.requestNextSceneCb(SceneManager.SceneName.SceneTreeHouse, null);
	}

	private bool OnChoiceOpenWindow(int index)
	{
		if (index != 0 && index == 1)
		{
			DataManagerGacha.GachaPackData gachaPackData = this.gachaPackDataList.Find((DataManagerGacha.GachaPackData x) => x.gachaId == this.lastSelectGacha.id);
			bool flag = 1 < gachaPackData.staticData.typeDataList.Count;
			int num = ((!this.lastSelectGacha.isLeftBtn && flag) ? 1 : 0);
			DataManagerGacha.GachaStaticTypeData gachaStaticTypeData = gachaPackData.staticData.typeDataList[num];
			int num2 = gachaStaticTypeData.useItemId;
			if (2000 == gachaPackData.staticData.StepResetTime.Year && this.ChkStepResetTimeIsOver(gachaPackData.staticData.StepResetTime))
			{
				CanvasManager.HdlOpenWindowBasic.Setup("確認", "更新データが見つかりました\nデータ更新を行います", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int basicWindowIndex)
				{
					if (this.IsDispGuiResult)
					{
						this.ChangeModeResultToTop();
					}
					this.guiDataTop.RefreshGachaTop = this.RefreshGachaTop(true);
					return true;
				}, null, false);
				CanvasManager.HdlOpenWindowBasic.Open();
			}
			else
			{
				if (this.lastSelectGacha.isLeftBtn && !flag)
				{
					bool flag2 = this.IsSubstituteEnable(gachaStaticTypeData);
					if (gachaStaticTypeData.subItemUseCondition != 0 && this.ChkSwitchEnableSubstitute(gachaStaticTypeData, flag2))
					{
						CanvasManager.HdlOpenWindowBasic.Setup("確認", "月間パスポートの期限が終了しました。\nデータ更新を行います。", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int basicWindowIndex)
						{
							if (this.IsDispGuiResult)
							{
								this.ChangeModeResultToTop();
							}
							this.guiDataTop.RefreshGachaTop = this.RefreshGachaTop(true);
							return true;
						}, null, false);
						CanvasManager.HdlOpenWindowBasic.Open();
						return true;
					}
					if (!flag2)
					{
						return true;
					}
					num2 = gachaStaticTypeData.substituteItemId;
				}
				if (this.IsDispGuiResult)
				{
					DataManager.DmGacha.LatestGreetingVoice.Stop();
				}
				int gachaType = gachaStaticTypeData.gachaType;
				this.GachaPlayActionCoroutine = Singleton<SceneManager>.Instance.StartCoroutine(this.GachaPlayAction(gachaPackData.gachaId, gachaType, num2));
			}
		}
		return true;
	}

	private void OnSuccessPurcheseStone()
	{
		this.UpdateStoneNum(this.currentGachaPackData);
	}

	private void OnSuccessPurcheseMonthlyPack()
	{
		this.RefreshGachaPackDataList(this.currentGachaPackData.gachaId, true, false, this.currentGachaTab);
	}

	private void OnStartItem(int index, GameObject go)
	{
		PguiRawImageCtrl component = go.transform.GetComponent<PguiRawImageCtrl>();
		if (this.gachaBannerImgMap.ContainsKey(index))
		{
			this.gachaBannerImgMap[index] = component;
			return;
		}
		this.gachaBannerImgMap.Add(index, component);
	}

	private void OnUpdateItem(int index, GameObject go)
	{
		if (index < 0 || this.gachaPackDataList.Count <= index)
		{
			go.SetActive(false);
			return;
		}
		go.SetActive(true);
		DataManagerGacha.GachaPackData gachaPackData = this.gachaPackDataList[index];
		SelGachaCtrl.GachaBanner gachaBanner = new SelGachaCtrl.GachaBanner(go);
		gachaBanner.BannerImg.banner = "Texture2D/GachaLabel/" + gachaPackData.staticData.labelTextureName;
		PrjUtil.AddTouchEventTrigger(gachaBanner.BannerImg.gameObject, delegate(Transform x)
		{
			this.OnClickGachaBannerImg(index);
		});
		bool flag = false;
		bool flag2 = false;
		using (List<DataManagerGacha.GachaStaticTypeData>.Enumerator enumerator = gachaPackData.staticData.typeDataList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				DataManagerGacha.GachaStaticTypeData typeData2 = enumerator.Current;
				if (typeData2.discountData != null)
				{
					DateTime now = TimeManager.Now;
					if (typeData2.discountData.startDatetime < now && now < typeData2.discountData.endDatetime)
					{
						DataManagerGacha.DynamicGachaTypeData dynamicGachaTypeData = gachaPackData.dynamicData.gachaTypeData.Find((DataManagerGacha.DynamicGachaTypeData x) => x.gachaType == typeData2.gachaType);
						if (typeData2.discountData.availableCount == 0 || typeData2.discountData.availableCount > dynamicGachaTypeData.discountPlayNum)
						{
							DataManagerGacha.DiscountType discountType = typeData2.discountData.discountType;
							if (discountType != DataManagerGacha.DiscountType.NoneReset)
							{
								if (discountType == DataManagerGacha.DiscountType.OnceADay)
								{
									DateTime dateTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
									if (dynamicGachaTypeData.lastPlayDateTime < dateTime)
									{
										if (typeData2.useItemNumber <= typeData2.discountData.discountNum)
										{
											flag = true;
										}
										else
										{
											flag2 = true;
										}
									}
								}
							}
							else if (typeData2.useItemNumber <= typeData2.discountData.discountNum)
							{
								flag = true;
							}
							else
							{
								flag2 = true;
							}
						}
					}
				}
			}
		}
		gachaBanner.MarkFree.gameObject.SetActive(flag);
		gachaBanner.Sale.gameObject.SetActive(flag2);
		gachaBanner.New.gameObject.SetActive(!DataManager.DmGacha.SelectedGachaIdHashSet.Contains(gachaPackData.gachaId));
		gachaBanner.PickUp.gameObject.SetActive(gachaPackData.staticData.recommendFlg);
		bool flag3 = false;
		using (List<DataManagerGacha.GachaStaticTypeData>.Enumerator enumerator = gachaPackData.staticData.typeDataList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				DataManagerGacha.GachaStaticTypeData typeData = enumerator.Current;
				if (typeData.bonusItemOneId != 0 || typeData.bonusItemSetId != 0)
				{
					if (typeData.bonusItemLimit == 0)
					{
						flag3 = true;
					}
					else
					{
						DataManagerGacha.DynamicGachaTypeData dynamicGachaTypeData2 = gachaPackData.dynamicData.gachaTypeData.Find((DataManagerGacha.DynamicGachaTypeData x) => x.gachaType == typeData.gachaType);
						int num = typeData.bonusItemLimit - dynamicGachaTypeData2.totalSubPlayNum;
						if (0 < num)
						{
							flag3 = true;
						}
					}
				}
			}
		}
		gachaBanner.Omake.gameObject.SetActive(flag3);
		DateTime dateTime2 = this.GachaPackDataLastUpdateTime.AddDays((double)SelGachaCtrl.GACHA_VIEW_LIMIT_DAY);
		gachaBanner.LimitTimeText.gameObject.SetActive(dateTime2 > gachaPackData.staticData.endDatetime || gachaPackData.staticData.dayOfWeekFlg);
		if (gachaPackData.staticData.dayOfWeekFlg)
		{
			gachaBanner.LimitTimeText.text = TimeManager.MakeTimeResidueText(this.GachaPackDataLastUpdateTime, gachaPackData.staticData.EndTimeOfDayOfWeek(TimeManager.Now), false, false);
			return;
		}
		gachaBanner.LimitTimeText.text = TimeManager.MakeTimeResidueText(this.GachaPackDataLastUpdateTime, gachaPackData.staticData.endDatetime, false, false);
	}

	private void OnChangeFocusItem(int index, GameObject go)
	{
		if (index < 0)
		{
			this.guiDataTop.GachaBannerScroll.ChangeFocusItem(0, false);
			return;
		}
		if (this.gachaPackDataList.Count <= index)
		{
			this.guiDataTop.GachaBannerScroll.ChangeFocusItem(this.gachaPackDataList.Count - 1, false);
			return;
		}
		bool flag = true;
		if (!this.focusForceChanged)
		{
			SoundManager.Play("prd_se_click", false, false);
		}
		else
		{
			flag = this.gachaIdChanged || this.CenterInfoDisplayedBanner;
			this.focusForceChanged = false;
		}
		int num = index % this.gachaBannerImgMap.Count;
		this.SetCurrentBanner(num);
		this.currentBannerBtnIndex = index;
		this.currentGachaPackData = this.gachaPackDataList[index];
		this.UpdateStoneNum(this.currentGachaPackData);
		this.UpdateGachaTopDispData(this.currentGachaPackData);
		this.ChangeCenterInfo(flag);
	}

	private bool OnSelectGachaTab(int index)
	{
		this.currentGachaTab = index;
		this.RefreshGachaPackDataList(0, true, false, this.currentGachaTab);
		return true;
	}

	private static readonly int GACHA_VIEW_LIMIT_DAY = 100;

	private static readonly float INFO_DISP_UPDATE_TIME = 7f;

	private static readonly float ICON_SIZE = 38f;

	private readonly float CLONE_CHARA_MOVIE_TIME = 13f;

	private readonly int RENDER_TEXTURE_CHARA_TRANSFORM_INDEX = 3;

	private SelGachaCtrl.GachaTopGUI guiDataTop;

	private SelGachaCtrl.GUIResult guiDataResult;

	private GachaWindowInfoCtrl gachaWindowInfo;

	private GachaAuthCtrl gachaAuth;

	private Coroutine GachaPlayActionCoroutine;

	private RenderTextureChara RenderCharaCenter;

	private RenderTextureChara RenderCharaLB;

	private List<DataManagerGacha.GachaPackData> gachaPackDataList;

	private Dictionary<int, PguiRawImageCtrl> gachaBannerImgMap;

	private int currentBannerBtnIndex;

	private DataManagerGacha.GachaPackData currentGachaPackData;

	private bool CenterInfoDisplayedBanner;

	private bool pauseCenterInfoDispLoop;

	private List<int> centerInfoDispIdList;

	private int centerInfoDispIndex;

	private float centerInfoDispTimeElapsed;

	private SelGachaCtrl.LastSelectGacha lastSelectGacha;

	private bool focusForceChanged;

	private bool gachaIdChanged;

	private List<GameObject> resultIconObjList;

	private bool gotoNextStepByTutorial;

	private SceneGacha.OpenParam sceneOpenParam;

	public Action<SceneManager.SceneName, object> requestNextSceneCb;

	public Func<bool> isRequestingNextSceneCb;

	private DataManagerMonthlyPack.PurchaseMonthlypackData nowPackData;

	private DateTime nowPackDataEndDateTime;

	private DataManagerMonthlyPack.PurchaseMonthlypackData nextPackData;

	private DateTime nextPackDataEndDateTime;

	private int currentGachaTab;

	private GameObject cloneCharaMovieObject;

	private float movieTime;

	public class GachaTopGUI
	{
		public GachaTopGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Txt_GachaName = baseTr.Find("CenterInfo/TitleBase/Txt_GachaName").GetComponent<PguiTextCtrl>();
			this.Txt_Term = baseTr.Find("CenterInfo/TitleBase/Txt_Term").GetComponent<PguiTextCtrl>();
			this.Btn_GachaDetailInfo = baseTr.Find("CenterInfo/TitleBase/Btn_MoreInfo").GetComponent<PguiButtonCtrl>();
			this.Btn_Reload = baseTr.Find("Btn_Reload").GetComponent<PguiButtonCtrl>();
			this.TouchPanel = baseTr.Find("CenterInfo/TouchPanel").GetComponent<PguiButtonCtrl>();
			this.RenderTexture_LB = baseTr.Find("RenderTexture_LB").gameObject;
			this.AEImage_Loading = baseTr.Find("CenterInfo/AEImage_Loading").GetComponent<PguiAECtrl>();
			this.TabGroup = baseTr.Find("ScrollBtns/TabGroup").GetComponent<PguiTabGroupCtrl>();
			this.GachaUseItemBtnList = new List<SelGachaCtrl.ItemBoardBase>
			{
				new SelGachaCtrl.ItemBoardBase(baseTr.Find("ItemInfo/Base/ItemList/ItemOwnBase01").GetComponent<PguiButtonCtrl>()),
				new SelGachaCtrl.ItemBoardBase(baseTr.Find("ItemInfo/Base/ItemList/ItemOwnBase02").GetComponent<PguiButtonCtrl>()),
				new SelGachaCtrl.ItemBoardBase(baseTr.Find("ItemInfo/Base/ItemList/ItemOwnBase03").GetComponent<PguiButtonCtrl>())
			};
			this.GachaBtnSet_Left = new SelGachaCtrl.GachaButtonSet(baseTr.Find("Btn_Main/Btn_Gacha_Left").GetComponent<PguiButtonCtrl>());
			this.GachaBtnSet_Left.StepUpInfo.SetActive(false);
			this.GachaBtnSet_Right = new SelGachaCtrl.GachaButtonSet(baseTr.Find("Btn_Main/Btn_Gacha_Right").GetComponent<PguiButtonCtrl>());
			this.GachaBannerScroll = baseTr.Find("ScrollBtns/BannerScroll/ScrollView").GetComponent<CustomScrollRect>();
			this.CeilingCountImageCtrl = baseTr.Find("CountInfo/ConfirmCount").GetComponent<PguiImageCtrl>();
			this.CeilingCountText = baseTr.Find("CountInfo/ConfirmCount/Txt_Item").GetComponent<PguiTextCtrl>();
			this.BannerAll = baseTr.Find("CenterInfo/BannerAll").gameObject;
			this.BannerTexture = baseTr.Find("CenterInfo/BannerAll/Texture").GetComponent<PguiRawImageCtrl>();
			this.BoxBtns = baseTr.Find("BoxBtns").gameObject;
			this.Btn_BoxReset = baseTr.Find("BoxBtns/Btn_Reset").GetComponent<PguiButtonCtrl>();
			this.ResetCountInfo = baseTr.Find("BoxBtns/Btn_Reset/BaseImage/CountInfo").gameObject;
			this.ResetCountText = baseTr.Find("BoxBtns/Btn_Reset/BaseImage/CountInfo/Num_Count").GetComponent<PguiTextCtrl>();
			this.BoxCheckBtn = baseTr.Find("BoxBtns/Btn_BoxCheck").GetComponent<PguiButtonCtrl>();
			this.CharaInfoAll = baseTr.Find("CenterInfo/CharaInfoAll").gameObject;
			this.Btn_CharaInfo = baseTr.Find("CenterInfo/CharaInfoAll/Btn_MoreInfo").GetComponent<PguiButtonCtrl>();
			this.Chara_IconStar = new List<PguiImageCtrl>();
			this.Chara_IconStar.Add(baseTr.Find("CenterInfo/CharaInfoAll/TxtAll/CharaName/Rank_Star/Icon_Star01").GetComponent<PguiImageCtrl>());
			this.Chara_IconStar.Add(baseTr.Find("CenterInfo/CharaInfoAll/TxtAll/CharaName/Rank_Star/Icon_Star02").GetComponent<PguiImageCtrl>());
			this.Chara_IconStar.Add(baseTr.Find("CenterInfo/CharaInfoAll/TxtAll/CharaName/Rank_Star/Icon_Star03").GetComponent<PguiImageCtrl>());
			this.Chara_IconStar.Add(baseTr.Find("CenterInfo/CharaInfoAll/TxtAll/CharaName/Rank_Star/Icon_Star04").GetComponent<PguiImageCtrl>());
			this.Chara_MovieImage = baseTr.Find("CenterInfo/CharaInfoAll/MovieMask/Chara_MovieImage").GetComponent<PguiRawImageCtrl>();
			this.Chara_TextAEImage = baseTr.Find("CenterInfo/CharaInfoAll/TxtAll/AEImage_Txt").GetComponent<PguiAECtrl>();
			this.Chara_RankStar_AE = baseTr.Find("CenterInfo/CharaInfoAll/TxtAll/CharaName/Rank_Star").GetComponent<PguiAECtrl>();
			this.CharaRankStarRectTransform = baseTr.Find("CenterInfo/CharaInfoAll/TxtAll/CharaName/Rank_Star").GetComponent<RectTransform>();
			this.CharaRankStarInitialPosition = this.CharaRankStarRectTransform.anchoredPosition;
			this.InfoDispCharaName = baseTr.Find("CenterInfo/CharaInfoAll/TxtAll/CharaName/Txt_CharaName").GetComponent<Text>();
			this.InfoDispWName = baseTr.Find("CenterInfo/CharaInfoAll/TxtAll/CharaName/Txt_CharaName/Txt_WName").GetComponent<Text>();
			this.InfoDispCharaAttrIcon = baseTr.Find("CenterInfo/CharaInfoAll/TxtAll/CharaName/Icon_CharAtr").GetComponent<PguiImageCtrl>();
			this.InfoDispCharaSubAttrIcon = baseTr.Find("CenterInfo/CharaInfoAll/TxtAll/CharaName/Icon_CharSubAtr").GetComponent<PguiImageCtrl>();
			this.InfoDispCharaMiracleName = baseTr.Find("CenterInfo/CharaInfoAll/TxtAll/MiracleName/Txt_CharaName").GetComponent<Text>();
			this.PhotoInfoAll = baseTr.Find("CenterInfo/PhotoInfoAll").gameObject;
			this.PhotoInfo_AEImage = baseTr.Find("CenterInfo/PhotoInfoAll/AEImage_PhotoInfo").GetComponent<PguiAECtrl>();
			this.Btn_PhotoInfo = baseTr.Find("CenterInfo/PhotoInfoAll/Btn_MoreInfo").GetComponent<PguiButtonCtrl>();
			this.Photo_IconStar = new List<PguiImageCtrl>();
			this.Photo_IconStar.Add(baseTr.Find("CenterInfo/PhotoInfoAll/All/Icon_Star/Icon_Star01").GetComponent<PguiImageCtrl>());
			this.Photo_IconStar.Add(baseTr.Find("CenterInfo/PhotoInfoAll/All/Icon_Star/Icon_Star02").GetComponent<PguiImageCtrl>());
			this.Photo_IconStar.Add(baseTr.Find("CenterInfo/PhotoInfoAll/All/Icon_Star/Icon_Star03").GetComponent<PguiImageCtrl>());
			this.Photo_IconStar.Add(baseTr.Find("CenterInfo/PhotoInfoAll/All/Icon_Star/Icon_Star04").GetComponent<PguiImageCtrl>());
			this.PhotoEventInfo = baseTr.Find("CenterInfo/PhotoInfoAll/All/EventInfo").GetComponent<PguiImageCtrl>();
			this.DispPhotoInfo = baseTr.Find("CenterInfo/PhotoInfoAll/PickUp_Card").gameObject;
			this.PhotoDropPopUp = baseTr.Find("CenterInfo/PhotoInfoAll/PickUp_Card/PopUp").GetComponent<PguiImageCtrl>();
			GameObject gameObject = baseTr.Find("CenterInfo/PhotoInfoAll/PickUp_Card/Card/Card_Photo").gameObject;
			gameObject.GetComponent<PguiPanel>().raycastTarget = false;
			this.PhotoCard = gameObject.GetComponent<IconPhotoCtrl>();
			this.Icon_PhotoKind = baseTr.Find("CenterInfo/PhotoInfoAll/All/PhotoName").GetComponent<PguiReplaceSpriteCtrl>();
			this.PhotoName_Text = baseTr.Find("CenterInfo/PhotoInfoAll/All/PhotoName/Txt_PhotoName").GetComponent<PguiTextCtrl>();
			this.PhotoInfo_Text = baseTr.Find("CenterInfo/PhotoInfoAll/All/TxtInfo/Txt_PhotoInfo").GetComponent<PguiTextCtrl>();
			this.PhotoHpTextCtrl = baseTr.Find("CenterInfo/PhotoInfoAll/All/StatusInfo_01/Num_Status").GetComponent<PguiTextCtrl>();
			this.PhotoAtkTextCtrl = baseTr.Find("CenterInfo/PhotoInfoAll/All/StatusInfo_02/Num_Status").GetComponent<PguiTextCtrl>();
			this.PhotoDefTextCtrl = baseTr.Find("CenterInfo/PhotoInfoAll/All/StatusInfo_03/Num_Status").GetComponent<PguiTextCtrl>();
			this.FurnitureInfoAll = baseTr.Find("CenterInfo/InteriorInfoAll").gameObject;
			this.FurnitureInfo_AEImage = baseTr.Find("CenterInfo/InteriorInfoAll/AEImage_InteriorInfo").GetComponent<PguiAECtrl>();
			this.Btn_FurnitureInfo = baseTr.Find("CenterInfo/InteriorInfoAll/Btn_MoreInfo").GetComponent<PguiButtonCtrl>();
			this.Furniture_MovieImage = baseTr.Find("CenterInfo/InteriorInfoAll/MovieMask/Interior_MovieImage").GetComponent<PguiRawImageCtrl>();
			this.FurnitureRarity_Text = baseTr.Find("CenterInfo/InteriorInfoAll/All/InteriorRarity/Txt_Rarity").GetComponent<PguiTextCtrl>();
			this.FurnitureName_Text = baseTr.Find("CenterInfo/InteriorInfoAll/All/InteriorName/Txt_InteriorName").GetComponent<PguiTextCtrl>();
		}

		public GameObject baseObj;

		public PguiTextCtrl Txt_GachaName;

		public PguiTextCtrl Txt_Term;

		public PguiButtonCtrl Btn_Reload;

		public PguiButtonCtrl TouchPanel;

		public GameObject RenderTexture_LB;

		public PguiAECtrl AEImage_Loading;

		public PguiButtonCtrl Btn_GachaDetailInfo;

		public List<SelGachaCtrl.ItemBoardBase> GachaUseItemBtnList;

		public SelGachaCtrl.GachaButtonSet GachaBtnSet_Left;

		public SelGachaCtrl.GachaButtonSet GachaBtnSet_Right;

		public CustomScrollRect GachaBannerScroll;

		public PguiImageCtrl CeilingCountImageCtrl;

		public PguiTextCtrl CeilingCountText;

		public GameObject BannerAll;

		public PguiRawImageCtrl BannerTexture;

		public GameObject CharaInfoAll;

		public PguiButtonCtrl Btn_CharaInfo;

		public List<PguiImageCtrl> Chara_IconStar;

		public PguiRawImageCtrl Chara_MovieImage;

		public PguiAECtrl Chara_TextAEImage;

		public PguiAECtrl Chara_RankStar_AE;

		public RectTransform CharaRankStarRectTransform;

		public Vector2 CharaRankStarInitialPosition;

		public Text InfoDispCharaName;

		public Text InfoDispWName;

		public PguiImageCtrl InfoDispCharaAttrIcon;

		public PguiImageCtrl InfoDispCharaSubAttrIcon;

		public Text InfoDispCharaMiracleName;

		public GameObject BoxBtns;

		public PguiButtonCtrl Btn_BoxReset;

		public GameObject ResetCountInfo;

		public PguiTextCtrl ResetCountText;

		public PguiButtonCtrl BoxCheckBtn;

		public GameObject PhotoInfoAll;

		public PguiAECtrl PhotoInfo_AEImage;

		public PguiButtonCtrl Btn_PhotoInfo;

		public List<PguiImageCtrl> Photo_IconStar;

		public PguiImageCtrl PhotoEventInfo;

		public GameObject DispPhotoInfo;

		public PguiImageCtrl PhotoDropPopUp;

		public IconPhotoCtrl PhotoCard;

		public PguiReplaceSpriteCtrl Icon_PhotoKind;

		public PguiTextCtrl PhotoName_Text;

		public PguiTextCtrl PhotoInfo_Text;

		public PguiTextCtrl PhotoHpTextCtrl;

		public PguiTextCtrl PhotoAtkTextCtrl;

		public PguiTextCtrl PhotoDefTextCtrl;

		public GameObject FurnitureInfoAll;

		public PguiAECtrl FurnitureInfo_AEImage;

		public PguiButtonCtrl Btn_FurnitureInfo;

		public PguiRawImageCtrl Furniture_MovieImage;

		public PguiTextCtrl FurnitureRarity_Text;

		public PguiTextCtrl FurnitureName_Text;

		public PguiTabGroupCtrl TabGroup;

		public IEnumerator StartPopupPlay;

		public IEnumerator RefreshGachaTop;
	}

	public class GachaBanner
	{
		public GachaBanner(GameObject go)
		{
			this.BannerImg = go.transform.GetComponent<PguiRawImageCtrl>();
			this.MarkFree = go.transform.Find("Mark_Free").GetComponent<PguiImageCtrl>();
			this.Sale = go.transform.Find("Mark_Sale").GetComponent<PguiImageCtrl>();
			this.PickUp = go.transform.Find("Mark_PickUp").GetComponent<PguiImageCtrl>();
			this.Omake = go.transform.Find("Mark_Omake").GetComponent<PguiImageCtrl>();
			this.New = go.transform.Find("Cmn_Mark_New").gameObject;
			this.LimitTimeText = go.transform.Find("Txt_Time").GetComponent<PguiTextCtrl>();
		}

		public PguiRawImageCtrl BannerImg;

		public PguiImageCtrl MarkFree;

		public PguiImageCtrl Sale;

		public PguiImageCtrl PickUp;

		public PguiImageCtrl Omake;

		public GameObject New;

		public PguiTextCtrl LimitTimeText;
	}

	public class GachaButtonSet
	{
		public GachaButtonSet(PguiButtonCtrl btn)
		{
			this.BaseButton = btn;
			this.CountInfo = btn.transform.Find("Gacha_CountInfo").GetComponent<PguiImageCtrl>();
			this.BaseImgObj = btn.transform.Find("BaseImage").gameObject;
			this.UseNumText = this.BaseImgObj.transform.Find("Num_use").GetComponent<PguiTextCtrl>();
			this.TimesText = this.BaseImgObj.transform.Find("Txt").GetComponent<PguiTextCtrl>();
			this.FreeText = this.BaseImgObj.transform.Find("Txt_Free").GetComponent<PguiTextCtrl>();
			this.FreeCampaignText = this.BaseImgObj.transform.Find("Txt_FreeCampaign").GetComponent<PguiTextCtrl>();
			this.StoneIcon = this.BaseImgObj.transform.Find("Icon_Stone").GetComponent<PguiRawImageCtrl>();
			this.StoneKind = this.StoneIcon.transform.Find("Txt_kind").GetComponent<PguiTextCtrl>();
			this.StoneKind.gameObject.SetActive(false);
			this.PopupSale = new PopUpCtrl();
			this.PopupSale.baseObj = btn.transform.Find("PopUpInfo/Gacha_PopUp_Sale").gameObject;
			this.PopupSale.MainText = this.PopupSale.baseObj.transform.Find("Txt_Off").GetComponent<PguiTextCtrl>();
			this.PopupSale.CountText = this.PopupSale.baseObj.transform.Find("Txt_Off_Count").GetComponent<PguiTextCtrl>();
			this.PopupSale.Animation = this.PopupSale.baseObj.GetComponent<SimpleAnimation>();
			this.PopupAnother = new PopUpCtrl();
			this.PopupAnother.baseObj = btn.transform.Find("PopUpInfo/Gacha_PopUp_Another").gameObject;
			this.PopupAnother.MainText = this.PopupAnother.baseObj.transform.Find("Txt").GetComponent<PguiTextCtrl>();
			this.PopupAnother.CountText = this.PopupAnother.baseObj.transform.Find("Txt_Count").GetComponent<PguiTextCtrl>();
			this.PopupAnother.Animation = this.PopupAnother.baseObj.GetComponent<SimpleAnimation>();
			this.StepUpInfo = btn.transform.Find("Stepup_Info").gameObject;
			this.StepUpInfoText = btn.transform.Find("Stepup_Info/Txt").GetComponent<PguiTextCtrl>();
		}

		public void PopupInitialize(bool PlaySale, bool PlayAnother)
		{
			this.PlayPopupQueue = new Queue<PopUpCtrl>();
			if (PlaySale)
			{
				this.PlayPopupQueue.Enqueue(this.PopupSale);
			}
			if (PlayAnother)
			{
				this.PlayPopupQueue.Enqueue(this.PopupAnother);
			}
		}

		public void StartPlayPopup()
		{
			if (this.PlayPopupQueue != null)
			{
				this.PlayPopup(this.PlayPopupQueue);
			}
		}

		private void PlayPopup(Queue<PopUpCtrl> popupQueue)
		{
			this.PopupSale.Animation.ExStop(true);
			this.PopupAnother.Animation.ExStop(true);
			this.PopupSale.baseObj.SetActive(false);
			this.PopupAnother.baseObj.SetActive(false);
			if (0 < popupQueue.Count)
			{
				PopUpCtrl popup = popupQueue.Dequeue();
				popup.baseObj.SetActive(true);
				popup.Animation.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, delegate
				{
					popupQueue.Enqueue(popup);
					this.PlayPopup(popupQueue);
				});
			}
		}

		public void StopPopup()
		{
			this.PopupSale.Animation.ExStop(true);
			this.PopupAnother.Animation.ExStop(true);
		}

		public void HidePopup()
		{
			this.PopupSale.baseObj.SetActive(false);
			this.PopupAnother.baseObj.SetActive(false);
		}

		public bool IsPlaying()
		{
			return this.PopupSale.Animation.ExIsPlaying() || this.PopupAnother.Animation.ExIsPlaying();
		}

		public PguiButtonCtrl BaseButton;

		public PguiImageCtrl CountInfo;

		public GameObject BaseImgObj;

		public PguiTextCtrl UseNumText;

		public PguiRawImageCtrl StoneIcon;

		public PguiTextCtrl TimesText;

		public PguiTextCtrl FreeText;

		public PguiTextCtrl FreeCampaignText;

		private PguiTextCtrl StoneKind;

		public PopUpCtrl PopupSale;

		public PopUpCtrl PopupAnother;

		private Queue<PopUpCtrl> PlayPopupQueue;

		public GameObject StepUpInfo;

		public PguiTextCtrl StepUpInfoText;
	}

	public class ItemBoardBase
	{
		public ItemBoardBase(PguiButtonCtrl button)
		{
			this.button = button;
			this.Base = button.gameObject;
			this.ItemIcon = button.transform.Find("BaseImage/Icon_Stone").GetComponent<PguiRawImageCtrl>();
			this.ItemNumText = button.transform.Find("BaseImage/Num_Own").GetComponent<PguiTextCtrl>();
			this.itemId = 0;
		}

		public void Setup(int id)
		{
			ItemData userItemData = DataManager.DmItem.GetUserItemData(id);
			this.itemId = id;
			this.ItemNumText.text = userItemData.num.ToString();
			this.ItemIcon.SetRawImage(userItemData.staticData.GetIconName(), true, false, null);
			this.ItemIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(SelGachaCtrl.ICON_SIZE, SelGachaCtrl.ICON_SIZE);
		}

		public GameObject Base;

		public PguiButtonCtrl button;

		public PguiRawImageCtrl ItemIcon;

		public PguiTextCtrl ItemNumText;

		public int itemId;
	}

	public class GUIResult
	{
		public bool IsOpenBonusItemWindow { get; set; }

		public GUIResult(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_Next = baseTr.Find("Btn_Next").GetComponent<PguiButtonCtrl>();
			this.Btn_Next.androidBackKeyTarget = true;
			this.Btn_Party = baseTr.Find("Btn_Party").GetComponent<PguiButtonCtrl>();
			this.Btn_Friends = baseTr.Find("Btn_Friends").GetComponent<PguiButtonCtrl>();
			this.Btn_Onemore = baseTr.Find("Btn_Onemore").GetComponent<PguiButtonCtrl>();
			this.Btn_OnemoreText = baseTr.Find("Btn_Onemore/BaseImage/Txt").GetComponent<PguiTextCtrl>();
			this.Btn_BoxInfo = baseTr.Find("Btn_BoxInfo").GetComponent<PguiButtonCtrl>();
			this.Btn_StepInfo = baseTr.Find("Btn_StepInfo").GetComponent<PguiButtonCtrl>();
			this.Btn_TreeHouse = baseTr.Find("Btn_TreeHouse").GetComponent<PguiButtonCtrl>();
			this.ResultEffect = baseTr.Find("AEImage_ResultEffect").gameObject;
			this.ResultAllObj = baseTr.Find("ResultAll").gameObject;
			this.ResultIconBaseList = new List<SelGachaCtrl.GUIResult.ResultIconBase>();
			for (int i = 0; i < 10; i++)
			{
				string text = "Icon" + (i + 1).ToString("D2");
				SelGachaCtrl.GUIResult.ResultIconBase resultIconBase = new SelGachaCtrl.GUIResult.ResultIconBase(this.ResultAllObj.transform.Find(text).gameObject);
				this.ResultIconBaseList.Add(resultIconBase);
			}
		}

		public GameObject baseObj;

		public PguiButtonCtrl Btn_Next;

		public PguiButtonCtrl Btn_Party;

		public PguiButtonCtrl Btn_Friends;

		public PguiButtonCtrl Btn_Onemore;

		public PguiTextCtrl Btn_OnemoreText;

		public PguiButtonCtrl Btn_BoxInfo;

		public PguiButtonCtrl Btn_StepInfo;

		public PguiButtonCtrl Btn_TreeHouse;

		public GameObject ResultEffect;

		public GameObject ResultAllObj;

		public List<SelGachaCtrl.GUIResult.ResultIconBase> ResultIconBaseList;

		public class ResultIconBase
		{
			public ResultIconBase(GameObject obj)
			{
				this.baseObj = obj;
				this.MarkNew = this.baseObj.transform.Find("Cmn_Mark_New");
				this.Base = this.baseObj.transform.Find("Base");
				this.BaseExchange = this.baseObj.transform.Find("Base_Exchange");
				this.IconBaseTr = this.baseObj.transform.Find("Icon_Base");
				this.IconReplaceTr = this.baseObj.transform.Find("Icon_Replace");
			}

			public GameObject baseObj;

			public Transform MarkNew;

			public Transform Base;

			public Transform BaseExchange;

			public Transform IconBaseTr;

			public Transform IconReplaceTr;
		}
	}

	private class LastSelectGacha
	{
		public LastSelectGacha()
		{
			this.id = 0;
			this.isLeftBtn = false;
			this.isStepUp = false;
			this.stepNextGachaId = 0;
			this.stepNextNum = 0;
		}

		public int id;

		public bool isLeftBtn;

		public bool isStepUp;

		public int stepNextGachaId;

		public int stepNextNum;
	}
}
