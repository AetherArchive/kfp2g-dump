using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SGNFW.Common;
using SGNFW.Http;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000148 RID: 328
public class SelGachaCtrl : MonoBehaviour
{
	// Token: 0x17000371 RID: 881
	// (get) Token: 0x060011F4 RID: 4596 RVA: 0x000D9324 File Offset: 0x000D7524
	// (set) Token: 0x060011F5 RID: 4597 RVA: 0x000D932C File Offset: 0x000D752C
	private DateTime GachaPackDataLastUpdateTime { get; set; }

	// Token: 0x17000372 RID: 882
	// (get) Token: 0x060011F6 RID: 4598 RVA: 0x000D9335 File Offset: 0x000D7535
	private bool IsTutorial
	{
		get
		{
			return this.sceneOpenParam != null && this.sceneOpenParam.tutorialSequence > TutorialUtil.Sequence.INVALID;
		}
	}

	// Token: 0x17000373 RID: 883
	// (get) Token: 0x060011F7 RID: 4599 RVA: 0x000D934F File Offset: 0x000D754F
	public bool RenderCharaLBFinishedSetup
	{
		get
		{
			return this.RenderCharaLB.FinishedSetup;
		}
	}

	// Token: 0x17000374 RID: 884
	// (get) Token: 0x060011F8 RID: 4600 RVA: 0x000D935C File Offset: 0x000D755C
	private bool IsDispGuiResult
	{
		get
		{
			return this.guiDataResult != null && this.guiDataResult.baseObj.activeSelf;
		}
	}

	// Token: 0x060011F9 RID: 4601 RVA: 0x000D9378 File Offset: 0x000D7578
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

	// Token: 0x060011FA RID: 4602 RVA: 0x000D96B4 File Offset: 0x000D78B4
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

	// Token: 0x060011FB RID: 4603 RVA: 0x000D98C8 File Offset: 0x000D7AC8
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

	// Token: 0x060011FC RID: 4604 RVA: 0x000D9924 File Offset: 0x000D7B24
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

	// Token: 0x060011FD RID: 4605 RVA: 0x000D9B08 File Offset: 0x000D7D08
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

	// Token: 0x060011FE RID: 4606 RVA: 0x000D9BF4 File Offset: 0x000D7DF4
	public void Destroy()
	{
		if (null != this.gachaWindowInfo)
		{
			Object.Destroy(this.gachaWindowInfo.gameObject);
			this.gachaWindowInfo = null;
		}
	}

	// Token: 0x060011FF RID: 4607 RVA: 0x000D9C1B File Offset: 0x000D7E1B
	public void OnDisable()
	{
		this.Disable();
	}

	// Token: 0x06001200 RID: 4608 RVA: 0x000D9C24 File Offset: 0x000D7E24
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

	// Token: 0x06001201 RID: 4609 RVA: 0x000D9DA4 File Offset: 0x000D7FA4
	private void UpdateGachaPackDataList(List<DataManagerGacha.GachaPackData> list)
	{
		this.GachaPackDataLastUpdateTime = TimeManager.Now;
		this.gachaPackDataList = ((this.currentGachaTab == 0) ? list : list.FindAll((DataManagerGacha.GachaPackData gacha) => gacha.staticData.tabCategory == (DataManagerGacha.TabCategory)this.currentGachaTab));
		DataManager.DmGacha.SelectedGachaIdHashSet = new HashSet<int>(DataManager.DmGameStatus.MakeUserFlagData().GachaNewInfoData.DisplayedIDList);
	}

	// Token: 0x06001202 RID: 4610 RVA: 0x000D9E04 File Offset: 0x000D8004
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

	// Token: 0x06001203 RID: 4611 RVA: 0x000D9EC0 File Offset: 0x000D80C0
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

	// Token: 0x06001204 RID: 4612 RVA: 0x000DA024 File Offset: 0x000D8224
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

	// Token: 0x06001205 RID: 4613 RVA: 0x000DA9A8 File Offset: 0x000D8BA8
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

	// Token: 0x06001206 RID: 4614 RVA: 0x000DAA2C File Offset: 0x000D8C2C
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

	// Token: 0x06001207 RID: 4615 RVA: 0x000DAB54 File Offset: 0x000D8D54
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

	// Token: 0x06001208 RID: 4616 RVA: 0x000DACD0 File Offset: 0x000D8ED0
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

	// Token: 0x06001209 RID: 4617 RVA: 0x000DAF70 File Offset: 0x000D9170
	private bool IsDiscountEnable(DataManagerGacha.GachaStaticTypeData staticTypeData, DataManagerGacha.DynamicGachaTypeData dynamicTypeData)
	{
		return staticTypeData.discountData != null && staticTypeData.discountData.startDatetime <= TimeManager.Now && TimeManager.Now < staticTypeData.discountData.endDatetime && (staticTypeData.discountData.availableCount == 0 || staticTypeData.discountData.availableCount > dynamicTypeData.discountPlayNum);
	}

	// Token: 0x0600120A RID: 4618 RVA: 0x000DAFD7 File Offset: 0x000D91D7
	private bool IsSubstituteEnable(DataManagerGacha.GachaStaticTypeData staticTypeData)
	{
		return this.IsMatchingSubItemUseCondition(staticTypeData, true) && staticTypeData.substituteItemId != 0 && DataManager.DmItem.GetUserItemData(staticTypeData.substituteItemId).num >= staticTypeData.substituteItemNumber;
	}

	// Token: 0x0600120B RID: 4619 RVA: 0x000DB010 File Offset: 0x000D9210
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

	// Token: 0x0600120C RID: 4620 RVA: 0x000DB0A7 File Offset: 0x000D92A7
	private void CreateDetailWindow(DataManagerGacha.GachaPackData gachaPackData)
	{
		this.pauseCenterInfoDispLoop = true;
		Singleton<SceneManager>.Instance.StartCoroutine(this.gachaWindowInfo.Open(gachaPackData, delegate
		{
			this.pauseCenterInfoDispLoop = false;
		}));
	}

	// Token: 0x0600120D RID: 4621 RVA: 0x000DB0D3 File Offset: 0x000D92D3
	private void OpenUseItemInfoWindow(int itemId)
	{
		CanvasManager.HdlOpenWindowItemInfo.SetupItemInfo(itemId);
		CanvasManager.HdlOpenWindowItemInfo.Open();
	}

	// Token: 0x0600120E RID: 4622 RVA: 0x000DB0EC File Offset: 0x000D92EC
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

	// Token: 0x0600120F RID: 4623 RVA: 0x000DB1A4 File Offset: 0x000D93A4
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

	// Token: 0x06001210 RID: 4624 RVA: 0x000DB27C File Offset: 0x000D947C
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

	// Token: 0x06001211 RID: 4625 RVA: 0x000DB368 File Offset: 0x000D9568
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

	// Token: 0x06001212 RID: 4626 RVA: 0x000DB674 File Offset: 0x000D9874
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

	// Token: 0x06001213 RID: 4627 RVA: 0x000DBDB8 File Offset: 0x000D9FB8
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

	// Token: 0x06001214 RID: 4628 RVA: 0x000DC4E8 File Offset: 0x000DA6E8
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

	// Token: 0x06001215 RID: 4629 RVA: 0x000DD270 File Offset: 0x000DB470
	private void ClearResultIconList()
	{
		foreach (GameObject gameObject in this.resultIconObjList)
		{
			Object.Destroy(gameObject);
		}
		this.resultIconObjList.Clear();
	}

	// Token: 0x06001216 RID: 4630 RVA: 0x000DD2CC File Offset: 0x000DB4CC
	public void SetIconAttr(int charaId, ref PguiImageCtrl Icon_Atr)
	{
		CharaStaticBase baseData = DataManager.DmChara.GetCharaStaticData(charaId).baseData;
		Icon_Atr.SetImageByName(IconCharaCtrl.Attribute2IconName(baseData.attribute));
	}

	// Token: 0x06001217 RID: 4631 RVA: 0x000DD2FC File Offset: 0x000DB4FC
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

	// Token: 0x06001218 RID: 4632 RVA: 0x000DD378 File Offset: 0x000DB578
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

	// Token: 0x06001219 RID: 4633 RVA: 0x000DD3E0 File Offset: 0x000DB5E0
	private int GetCenterInfoDispItemId()
	{
		return this.centerInfoDispIdList[this.centerInfoDispIndex];
	}

	// Token: 0x0600121A RID: 4634 RVA: 0x000DD3F4 File Offset: 0x000DB5F4
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

	// Token: 0x0600121B RID: 4635 RVA: 0x000DDAE9 File Offset: 0x000DBCE9
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

	// Token: 0x0600121C RID: 4636 RVA: 0x000DDB0E File Offset: 0x000DBD0E
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

	// Token: 0x0600121D RID: 4637 RVA: 0x000DDB24 File Offset: 0x000DBD24
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

	// Token: 0x0600121E RID: 4638 RVA: 0x000DDB3A File Offset: 0x000DBD3A
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

	// Token: 0x0600121F RID: 4639 RVA: 0x000DDB49 File Offset: 0x000DBD49
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

	// Token: 0x06001220 RID: 4640 RVA: 0x000DDB6D File Offset: 0x000DBD6D
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

	// Token: 0x06001221 RID: 4641 RVA: 0x000DDB7C File Offset: 0x000DBD7C
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

	// Token: 0x06001222 RID: 4642 RVA: 0x000DDBAE File Offset: 0x000DBDAE
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

	// Token: 0x06001223 RID: 4643 RVA: 0x000DDBEE File Offset: 0x000DBDEE
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

	// Token: 0x06001224 RID: 4644 RVA: 0x000DDC2E File Offset: 0x000DBE2E
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

	// Token: 0x06001225 RID: 4645 RVA: 0x000DDC60 File Offset: 0x000DBE60
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

	// Token: 0x06001226 RID: 4646 RVA: 0x000DDCB0 File Offset: 0x000DBEB0
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

	// Token: 0x06001227 RID: 4647 RVA: 0x000DDD10 File Offset: 0x000DBF10
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

	// Token: 0x06001228 RID: 4648 RVA: 0x000DDD80 File Offset: 0x000DBF80
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

	// Token: 0x06001229 RID: 4649 RVA: 0x000DDE04 File Offset: 0x000DC004
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

	// Token: 0x0600122A RID: 4650 RVA: 0x000DDE74 File Offset: 0x000DC074
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

	// Token: 0x0600122B RID: 4651 RVA: 0x000DDED0 File Offset: 0x000DC0D0
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

	// Token: 0x0600122C RID: 4652 RVA: 0x000DDF2C File Offset: 0x000DC12C
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

	// Token: 0x0600122D RID: 4653 RVA: 0x000DDF88 File Offset: 0x000DC188
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

	// Token: 0x0600122E RID: 4654 RVA: 0x000DE013 File Offset: 0x000DC213
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

	// Token: 0x0600122F RID: 4655 RVA: 0x000DE050 File Offset: 0x000DC250
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

	// Token: 0x06001230 RID: 4656 RVA: 0x000DE0A4 File Offset: 0x000DC2A4
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

	// Token: 0x06001231 RID: 4657 RVA: 0x000DE0D8 File Offset: 0x000DC2D8
	private void ChangeModeResultToTop()
	{
		this.SetDisplayCmnMenu(true);
		this.ChangeCenterInfo(this.CenterInfoDisplayedBanner);
		this.guiDataTop.baseObj.SetActive(true);
		this.guiDataResult.baseObj.SetActive(false);
		DataManager.DmGacha.LatestGreetingVoice.Stop();
	}

	// Token: 0x06001232 RID: 4658 RVA: 0x000DE12C File Offset: 0x000DC32C
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

	// Token: 0x06001233 RID: 4659 RVA: 0x000DE1D4 File Offset: 0x000DC3D4
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

	// Token: 0x06001234 RID: 4660 RVA: 0x000DE254 File Offset: 0x000DC454
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

	// Token: 0x06001235 RID: 4661 RVA: 0x000DE2DC File Offset: 0x000DC4DC
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

	// Token: 0x06001236 RID: 4662 RVA: 0x000DE36E File Offset: 0x000DC56E
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

	// Token: 0x06001237 RID: 4663 RVA: 0x000DE39C File Offset: 0x000DC59C
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

	// Token: 0x06001238 RID: 4664 RVA: 0x000DE52A File Offset: 0x000DC72A
	private void OnSuccessPurcheseStone()
	{
		this.UpdateStoneNum(this.currentGachaPackData);
	}

	// Token: 0x06001239 RID: 4665 RVA: 0x000DE538 File Offset: 0x000DC738
	private void OnSuccessPurcheseMonthlyPack()
	{
		this.RefreshGachaPackDataList(this.currentGachaPackData.gachaId, true, false, this.currentGachaTab);
	}

	// Token: 0x0600123A RID: 4666 RVA: 0x000DE554 File Offset: 0x000DC754
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

	// Token: 0x0600123B RID: 4667 RVA: 0x000DE598 File Offset: 0x000DC798
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

	// Token: 0x0600123C RID: 4668 RVA: 0x000DE9C8 File Offset: 0x000DCBC8
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

	// Token: 0x0600123D RID: 4669 RVA: 0x000DEA98 File Offset: 0x000DCC98
	private bool OnSelectGachaTab(int index)
	{
		this.currentGachaTab = index;
		this.RefreshGachaPackDataList(0, true, false, this.currentGachaTab);
		return true;
	}

	// Token: 0x04000EF9 RID: 3833
	private static readonly int GACHA_VIEW_LIMIT_DAY = 100;

	// Token: 0x04000EFA RID: 3834
	private static readonly float INFO_DISP_UPDATE_TIME = 7f;

	// Token: 0x04000EFB RID: 3835
	private static readonly float ICON_SIZE = 38f;

	// Token: 0x04000EFC RID: 3836
	private readonly float CLONE_CHARA_MOVIE_TIME = 13f;

	// Token: 0x04000EFD RID: 3837
	private readonly int RENDER_TEXTURE_CHARA_TRANSFORM_INDEX = 3;

	// Token: 0x04000EFE RID: 3838
	private SelGachaCtrl.GachaTopGUI guiDataTop;

	// Token: 0x04000EFF RID: 3839
	private SelGachaCtrl.GUIResult guiDataResult;

	// Token: 0x04000F00 RID: 3840
	private GachaWindowInfoCtrl gachaWindowInfo;

	// Token: 0x04000F01 RID: 3841
	private GachaAuthCtrl gachaAuth;

	// Token: 0x04000F02 RID: 3842
	private Coroutine GachaPlayActionCoroutine;

	// Token: 0x04000F03 RID: 3843
	private RenderTextureChara RenderCharaCenter;

	// Token: 0x04000F04 RID: 3844
	private RenderTextureChara RenderCharaLB;

	// Token: 0x04000F05 RID: 3845
	private List<DataManagerGacha.GachaPackData> gachaPackDataList;

	// Token: 0x04000F06 RID: 3846
	private Dictionary<int, PguiRawImageCtrl> gachaBannerImgMap;

	// Token: 0x04000F07 RID: 3847
	private int currentBannerBtnIndex;

	// Token: 0x04000F08 RID: 3848
	private DataManagerGacha.GachaPackData currentGachaPackData;

	// Token: 0x04000F09 RID: 3849
	private bool CenterInfoDisplayedBanner;

	// Token: 0x04000F0A RID: 3850
	private bool pauseCenterInfoDispLoop;

	// Token: 0x04000F0B RID: 3851
	private List<int> centerInfoDispIdList;

	// Token: 0x04000F0C RID: 3852
	private int centerInfoDispIndex;

	// Token: 0x04000F0D RID: 3853
	private float centerInfoDispTimeElapsed;

	// Token: 0x04000F0F RID: 3855
	private SelGachaCtrl.LastSelectGacha lastSelectGacha;

	// Token: 0x04000F10 RID: 3856
	private bool focusForceChanged;

	// Token: 0x04000F11 RID: 3857
	private bool gachaIdChanged;

	// Token: 0x04000F12 RID: 3858
	private List<GameObject> resultIconObjList;

	// Token: 0x04000F13 RID: 3859
	private bool gotoNextStepByTutorial;

	// Token: 0x04000F14 RID: 3860
	private SceneGacha.OpenParam sceneOpenParam;

	// Token: 0x04000F15 RID: 3861
	public Action<SceneManager.SceneName, object> requestNextSceneCb;

	// Token: 0x04000F16 RID: 3862
	public Func<bool> isRequestingNextSceneCb;

	// Token: 0x04000F17 RID: 3863
	private DataManagerMonthlyPack.PurchaseMonthlypackData nowPackData;

	// Token: 0x04000F18 RID: 3864
	private DateTime nowPackDataEndDateTime;

	// Token: 0x04000F19 RID: 3865
	private DataManagerMonthlyPack.PurchaseMonthlypackData nextPackData;

	// Token: 0x04000F1A RID: 3866
	private DateTime nextPackDataEndDateTime;

	// Token: 0x04000F1B RID: 3867
	private int currentGachaTab;

	// Token: 0x04000F1C RID: 3868
	private GameObject cloneCharaMovieObject;

	// Token: 0x04000F1D RID: 3869
	private float movieTime;

	// Token: 0x02000AC5 RID: 2757
	public class GachaTopGUI
	{
		// Token: 0x06004061 RID: 16481 RVA: 0x001F53C4 File Offset: 0x001F35C4
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

		// Token: 0x04004465 RID: 17509
		public GameObject baseObj;

		// Token: 0x04004466 RID: 17510
		public PguiTextCtrl Txt_GachaName;

		// Token: 0x04004467 RID: 17511
		public PguiTextCtrl Txt_Term;

		// Token: 0x04004468 RID: 17512
		public PguiButtonCtrl Btn_Reload;

		// Token: 0x04004469 RID: 17513
		public PguiButtonCtrl TouchPanel;

		// Token: 0x0400446A RID: 17514
		public GameObject RenderTexture_LB;

		// Token: 0x0400446B RID: 17515
		public PguiAECtrl AEImage_Loading;

		// Token: 0x0400446C RID: 17516
		public PguiButtonCtrl Btn_GachaDetailInfo;

		// Token: 0x0400446D RID: 17517
		public List<SelGachaCtrl.ItemBoardBase> GachaUseItemBtnList;

		// Token: 0x0400446E RID: 17518
		public SelGachaCtrl.GachaButtonSet GachaBtnSet_Left;

		// Token: 0x0400446F RID: 17519
		public SelGachaCtrl.GachaButtonSet GachaBtnSet_Right;

		// Token: 0x04004470 RID: 17520
		public CustomScrollRect GachaBannerScroll;

		// Token: 0x04004471 RID: 17521
		public PguiImageCtrl CeilingCountImageCtrl;

		// Token: 0x04004472 RID: 17522
		public PguiTextCtrl CeilingCountText;

		// Token: 0x04004473 RID: 17523
		public GameObject BannerAll;

		// Token: 0x04004474 RID: 17524
		public PguiRawImageCtrl BannerTexture;

		// Token: 0x04004475 RID: 17525
		public GameObject CharaInfoAll;

		// Token: 0x04004476 RID: 17526
		public PguiButtonCtrl Btn_CharaInfo;

		// Token: 0x04004477 RID: 17527
		public List<PguiImageCtrl> Chara_IconStar;

		// Token: 0x04004478 RID: 17528
		public PguiRawImageCtrl Chara_MovieImage;

		// Token: 0x04004479 RID: 17529
		public PguiAECtrl Chara_TextAEImage;

		// Token: 0x0400447A RID: 17530
		public PguiAECtrl Chara_RankStar_AE;

		// Token: 0x0400447B RID: 17531
		public RectTransform CharaRankStarRectTransform;

		// Token: 0x0400447C RID: 17532
		public Vector2 CharaRankStarInitialPosition;

		// Token: 0x0400447D RID: 17533
		public Text InfoDispCharaName;

		// Token: 0x0400447E RID: 17534
		public Text InfoDispWName;

		// Token: 0x0400447F RID: 17535
		public PguiImageCtrl InfoDispCharaAttrIcon;

		// Token: 0x04004480 RID: 17536
		public PguiImageCtrl InfoDispCharaSubAttrIcon;

		// Token: 0x04004481 RID: 17537
		public Text InfoDispCharaMiracleName;

		// Token: 0x04004482 RID: 17538
		public GameObject BoxBtns;

		// Token: 0x04004483 RID: 17539
		public PguiButtonCtrl Btn_BoxReset;

		// Token: 0x04004484 RID: 17540
		public GameObject ResetCountInfo;

		// Token: 0x04004485 RID: 17541
		public PguiTextCtrl ResetCountText;

		// Token: 0x04004486 RID: 17542
		public PguiButtonCtrl BoxCheckBtn;

		// Token: 0x04004487 RID: 17543
		public GameObject PhotoInfoAll;

		// Token: 0x04004488 RID: 17544
		public PguiAECtrl PhotoInfo_AEImage;

		// Token: 0x04004489 RID: 17545
		public PguiButtonCtrl Btn_PhotoInfo;

		// Token: 0x0400448A RID: 17546
		public List<PguiImageCtrl> Photo_IconStar;

		// Token: 0x0400448B RID: 17547
		public PguiImageCtrl PhotoEventInfo;

		// Token: 0x0400448C RID: 17548
		public GameObject DispPhotoInfo;

		// Token: 0x0400448D RID: 17549
		public PguiImageCtrl PhotoDropPopUp;

		// Token: 0x0400448E RID: 17550
		public IconPhotoCtrl PhotoCard;

		// Token: 0x0400448F RID: 17551
		public PguiReplaceSpriteCtrl Icon_PhotoKind;

		// Token: 0x04004490 RID: 17552
		public PguiTextCtrl PhotoName_Text;

		// Token: 0x04004491 RID: 17553
		public PguiTextCtrl PhotoInfo_Text;

		// Token: 0x04004492 RID: 17554
		public PguiTextCtrl PhotoHpTextCtrl;

		// Token: 0x04004493 RID: 17555
		public PguiTextCtrl PhotoAtkTextCtrl;

		// Token: 0x04004494 RID: 17556
		public PguiTextCtrl PhotoDefTextCtrl;

		// Token: 0x04004495 RID: 17557
		public GameObject FurnitureInfoAll;

		// Token: 0x04004496 RID: 17558
		public PguiAECtrl FurnitureInfo_AEImage;

		// Token: 0x04004497 RID: 17559
		public PguiButtonCtrl Btn_FurnitureInfo;

		// Token: 0x04004498 RID: 17560
		public PguiRawImageCtrl Furniture_MovieImage;

		// Token: 0x04004499 RID: 17561
		public PguiTextCtrl FurnitureRarity_Text;

		// Token: 0x0400449A RID: 17562
		public PguiTextCtrl FurnitureName_Text;

		// Token: 0x0400449B RID: 17563
		public PguiTabGroupCtrl TabGroup;

		// Token: 0x0400449C RID: 17564
		public IEnumerator StartPopupPlay;

		// Token: 0x0400449D RID: 17565
		public IEnumerator RefreshGachaTop;
	}

	// Token: 0x02000AC6 RID: 2758
	public class GachaBanner
	{
		// Token: 0x06004062 RID: 16482 RVA: 0x001F59B8 File Offset: 0x001F3BB8
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

		// Token: 0x0400449E RID: 17566
		public PguiRawImageCtrl BannerImg;

		// Token: 0x0400449F RID: 17567
		public PguiImageCtrl MarkFree;

		// Token: 0x040044A0 RID: 17568
		public PguiImageCtrl Sale;

		// Token: 0x040044A1 RID: 17569
		public PguiImageCtrl PickUp;

		// Token: 0x040044A2 RID: 17570
		public PguiImageCtrl Omake;

		// Token: 0x040044A3 RID: 17571
		public GameObject New;

		// Token: 0x040044A4 RID: 17572
		public PguiTextCtrl LimitTimeText;
	}

	// Token: 0x02000AC7 RID: 2759
	public class GachaButtonSet
	{
		// Token: 0x06004063 RID: 16483 RVA: 0x001F5A80 File Offset: 0x001F3C80
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

		// Token: 0x06004064 RID: 16484 RVA: 0x001F5D0B File Offset: 0x001F3F0B
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

		// Token: 0x06004065 RID: 16485 RVA: 0x001F5D40 File Offset: 0x001F3F40
		public void StartPlayPopup()
		{
			if (this.PlayPopupQueue != null)
			{
				this.PlayPopup(this.PlayPopupQueue);
			}
		}

		// Token: 0x06004066 RID: 16486 RVA: 0x001F5D58 File Offset: 0x001F3F58
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

		// Token: 0x06004067 RID: 16487 RVA: 0x001F5E0A File Offset: 0x001F400A
		public void StopPopup()
		{
			this.PopupSale.Animation.ExStop(true);
			this.PopupAnother.Animation.ExStop(true);
		}

		// Token: 0x06004068 RID: 16488 RVA: 0x001F5E2E File Offset: 0x001F402E
		public void HidePopup()
		{
			this.PopupSale.baseObj.SetActive(false);
			this.PopupAnother.baseObj.SetActive(false);
		}

		// Token: 0x06004069 RID: 16489 RVA: 0x001F5E52 File Offset: 0x001F4052
		public bool IsPlaying()
		{
			return this.PopupSale.Animation.ExIsPlaying() || this.PopupAnother.Animation.ExIsPlaying();
		}

		// Token: 0x040044A5 RID: 17573
		public PguiButtonCtrl BaseButton;

		// Token: 0x040044A6 RID: 17574
		public PguiImageCtrl CountInfo;

		// Token: 0x040044A7 RID: 17575
		public GameObject BaseImgObj;

		// Token: 0x040044A8 RID: 17576
		public PguiTextCtrl UseNumText;

		// Token: 0x040044A9 RID: 17577
		public PguiRawImageCtrl StoneIcon;

		// Token: 0x040044AA RID: 17578
		public PguiTextCtrl TimesText;

		// Token: 0x040044AB RID: 17579
		public PguiTextCtrl FreeText;

		// Token: 0x040044AC RID: 17580
		public PguiTextCtrl FreeCampaignText;

		// Token: 0x040044AD RID: 17581
		private PguiTextCtrl StoneKind;

		// Token: 0x040044AE RID: 17582
		public PopUpCtrl PopupSale;

		// Token: 0x040044AF RID: 17583
		public PopUpCtrl PopupAnother;

		// Token: 0x040044B0 RID: 17584
		private Queue<PopUpCtrl> PlayPopupQueue;

		// Token: 0x040044B1 RID: 17585
		public GameObject StepUpInfo;

		// Token: 0x040044B2 RID: 17586
		public PguiTextCtrl StepUpInfoText;
	}

	// Token: 0x02000AC8 RID: 2760
	public class ItemBoardBase
	{
		// Token: 0x0600406A RID: 16490 RVA: 0x001F5E78 File Offset: 0x001F4078
		public ItemBoardBase(PguiButtonCtrl button)
		{
			this.button = button;
			this.Base = button.gameObject;
			this.ItemIcon = button.transform.Find("BaseImage/Icon_Stone").GetComponent<PguiRawImageCtrl>();
			this.ItemNumText = button.transform.Find("BaseImage/Num_Own").GetComponent<PguiTextCtrl>();
			this.itemId = 0;
		}

		// Token: 0x0600406B RID: 16491 RVA: 0x001F5EDC File Offset: 0x001F40DC
		public void Setup(int id)
		{
			ItemData userItemData = DataManager.DmItem.GetUserItemData(id);
			this.itemId = id;
			this.ItemNumText.text = userItemData.num.ToString();
			this.ItemIcon.SetRawImage(userItemData.staticData.GetIconName(), true, false, null);
			this.ItemIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(SelGachaCtrl.ICON_SIZE, SelGachaCtrl.ICON_SIZE);
		}

		// Token: 0x040044B3 RID: 17587
		public GameObject Base;

		// Token: 0x040044B4 RID: 17588
		public PguiButtonCtrl button;

		// Token: 0x040044B5 RID: 17589
		public PguiRawImageCtrl ItemIcon;

		// Token: 0x040044B6 RID: 17590
		public PguiTextCtrl ItemNumText;

		// Token: 0x040044B7 RID: 17591
		public int itemId;
	}

	// Token: 0x02000AC9 RID: 2761
	public class GUIResult
	{
		// Token: 0x1700096E RID: 2414
		// (get) Token: 0x0600406C RID: 16492 RVA: 0x001F5F4D File Offset: 0x001F414D
		// (set) Token: 0x0600406D RID: 16493 RVA: 0x001F5F55 File Offset: 0x001F4155
		public bool IsOpenBonusItemWindow { get; set; }

		// Token: 0x0600406E RID: 16494 RVA: 0x001F5F60 File Offset: 0x001F4160
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

		// Token: 0x040044B8 RID: 17592
		public GameObject baseObj;

		// Token: 0x040044B9 RID: 17593
		public PguiButtonCtrl Btn_Next;

		// Token: 0x040044BA RID: 17594
		public PguiButtonCtrl Btn_Party;

		// Token: 0x040044BB RID: 17595
		public PguiButtonCtrl Btn_Friends;

		// Token: 0x040044BC RID: 17596
		public PguiButtonCtrl Btn_Onemore;

		// Token: 0x040044BD RID: 17597
		public PguiTextCtrl Btn_OnemoreText;

		// Token: 0x040044BE RID: 17598
		public PguiButtonCtrl Btn_BoxInfo;

		// Token: 0x040044BF RID: 17599
		public PguiButtonCtrl Btn_StepInfo;

		// Token: 0x040044C0 RID: 17600
		public PguiButtonCtrl Btn_TreeHouse;

		// Token: 0x040044C1 RID: 17601
		public GameObject ResultEffect;

		// Token: 0x040044C2 RID: 17602
		public GameObject ResultAllObj;

		// Token: 0x040044C3 RID: 17603
		public List<SelGachaCtrl.GUIResult.ResultIconBase> ResultIconBaseList;

		// Token: 0x02001174 RID: 4468
		public class ResultIconBase
		{
			// Token: 0x06005628 RID: 22056 RVA: 0x0025101C File Offset: 0x0024F21C
			public ResultIconBase(GameObject obj)
			{
				this.baseObj = obj;
				this.MarkNew = this.baseObj.transform.Find("Cmn_Mark_New");
				this.Base = this.baseObj.transform.Find("Base");
				this.BaseExchange = this.baseObj.transform.Find("Base_Exchange");
				this.IconBaseTr = this.baseObj.transform.Find("Icon_Base");
				this.IconReplaceTr = this.baseObj.transform.Find("Icon_Replace");
			}

			// Token: 0x04005FD1 RID: 24529
			public GameObject baseObj;

			// Token: 0x04005FD2 RID: 24530
			public Transform MarkNew;

			// Token: 0x04005FD3 RID: 24531
			public Transform Base;

			// Token: 0x04005FD4 RID: 24532
			public Transform BaseExchange;

			// Token: 0x04005FD5 RID: 24533
			public Transform IconBaseTr;

			// Token: 0x04005FD6 RID: 24534
			public Transform IconReplaceTr;
		}
	}

	// Token: 0x02000ACA RID: 2762
	private class LastSelectGacha
	{
		// Token: 0x0600406F RID: 16495 RVA: 0x001F60C2 File Offset: 0x001F42C2
		public LastSelectGacha()
		{
			this.id = 0;
			this.isLeftBtn = false;
			this.isStepUp = false;
			this.stepNextGachaId = 0;
			this.stepNextNum = 0;
		}

		// Token: 0x040044C5 RID: 17605
		public int id;

		// Token: 0x040044C6 RID: 17606
		public bool isLeftBtn;

		// Token: 0x040044C7 RID: 17607
		public bool isStepUp;

		// Token: 0x040044C8 RID: 17608
		public int stepNextGachaId;

		// Token: 0x040044C9 RID: 17609
		public int stepNextNum;
	}
}
