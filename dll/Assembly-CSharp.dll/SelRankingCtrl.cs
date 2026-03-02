using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.HttpRequest.Protocol;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000163 RID: 355
public class SelRankingCtrl : MonoBehaviour
{
	// Token: 0x060014C1 RID: 5313 RVA: 0x000FCBE0 File Offset: 0x000FADE0
	public void Initialize()
	{
		this.RankingMenuList = new List<SelRankingCtrl.RankingMenu>
		{
			new SelRankingCtrl.RankingMenu("シーサーバル道場ランキング", new UnityAction(this.OnClickTrainingButton), false),
			new SelRankingCtrl.RankingMenu("けもステランキング", new UnityAction(this.OnClickRankingButton), false)
		};
		GameObject gameObject = Object.Instantiate<GameObject>((GameObject)AssetManager.GetAssetData("SceneMenu/GUI/Prefab/GUI_Menu_Ranking"), base.transform);
		this.guiData = new SelRankingCtrl.GUI(gameObject.transform);
		this.guiData.ScrollView.InitForce();
		ReuseScroll scrollView = this.guiData.ScrollView;
		scrollView.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollView.onStartItem, new Action<int, GameObject>(this.OnStartItem));
		ReuseScroll scrollView2 = this.guiData.ScrollView;
		scrollView2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollView2.onUpdateItem, new Action<int, GameObject>(this.OnUpdateItem));
		this.guiData.ScrollView.Setup(this.RankingMenuList.Count, 0);
		GameObject gameObject2 = Object.Instantiate<GameObject>((GameObject)AssetManager.GetAssetData("SceneMenu/GUI/Prefab/GUI_Menu_Ranking_Window"), Singleton<CanvasManager>.Instance.SystemMiddleArea);
		gameObject2.GetComponent<PguiPanel>().raycastTarget = false;
		this.kemoStatusRankingWindowCtrl = gameObject2.AddComponent<KemoStatusRankingWindowCtrl>();
		this.kemoStatusRankingWindowCtrl.Initialize();
		this.InitializeTrainingRankingWindow();
		this.trainingSeasonList = new List<SeasonTrainingRankingData.RankingOne>();
		this.trainingSeasonId = 0;
	}

	// Token: 0x060014C2 RID: 5314 RVA: 0x000FCD3C File Offset: 0x000FAF3C
	public void Setup()
	{
		HomeCheckResult homeCheckResult = DataManager.DmHome.GetHomeCheckResult();
		Sealed @sealed = ((homeCheckResult != null) ? homeCheckResult.sealedData : null);
		bool flag = false;
		bool flag2 = false;
		if (@sealed != null)
		{
			flag = 1 == @sealed.quest_training;
			flag2 = 1 == @sealed.kemostatus_ranking;
		}
		this.RankingMenuList[0].IsSealed = flag;
		this.RankingMenuList[1].IsSealed = flag2;
		this.guiData.ScrollView.Refresh();
		this.trainingRankingWindow.tab.Setup(1, new PguiTabGroupCtrl.OnSelectTab(this.onClickTabSeason));
	}

	// Token: 0x060014C3 RID: 5315 RVA: 0x000FCDCC File Offset: 0x000FAFCC
	public void UpdateSel()
	{
		if (this.trainingRankingwindowOpen != null && !this.trainingRankingwindowOpen.MoveNext())
		{
			this.trainingRankingwindowOpen = null;
		}
	}

	// Token: 0x060014C4 RID: 5316 RVA: 0x000FCDEC File Offset: 0x000FAFEC
	public void Destroy()
	{
		if (null != this.kemoStatusRankingWindowCtrl)
		{
			Object.Destroy(this.kemoStatusRankingWindowCtrl.gameObject);
			this.kemoStatusRankingWindowCtrl = null;
		}
		if (null != this.trainingRankingWindowBaseObj)
		{
			Object.Destroy(this.trainingRankingWindowBaseObj);
			this.trainingRankingWindow = null;
			this.trainingRankingWindowBaseObj = null;
		}
	}

	// Token: 0x060014C5 RID: 5317 RVA: 0x000FCE45 File Offset: 0x000FB045
	private void OnClickTrainingButton()
	{
		this.trainingRankingwindowOpen = this.SeasonDisp();
	}

	// Token: 0x060014C6 RID: 5318 RVA: 0x000FCE53 File Offset: 0x000FB053
	private void OnClickRankingButton()
	{
		this.kemoStatusRankingWindowCtrl.Open(new KemoStatusRankingWindowCtrl.SetupParam());
	}

	// Token: 0x060014C7 RID: 5319 RVA: 0x000FCE68 File Offset: 0x000FB068
	private void OnStartItem(int index, GameObject go)
	{
		if (index < this.RankingMenuList.Count)
		{
			new SelRankingCtrl.GUIMenuBar(go.transform).BarButton.AddOnClickListener(delegate(PguiButtonCtrl button)
			{
				UnityAction action = this.RankingMenuList[index].Action;
				if (action == null)
				{
					return;
				}
				action();
			}, PguiButtonCtrl.SoundType.DEFAULT);
		}
	}

	// Token: 0x060014C8 RID: 5320 RVA: 0x000FCEC0 File Offset: 0x000FB0C0
	private void OnUpdateItem(int index, GameObject go)
	{
		if (index < this.RankingMenuList.Count)
		{
			go.SetActive(true);
			new SelRankingCtrl.GUIMenuBar(go.transform)
			{
				Txt_Title = 
				{
					text = this.RankingMenuList[index].Title
				}
			}.BarButton.SetActEnable(!this.RankingMenuList[index].IsSealed, false, false);
			return;
		}
		go.SetActive(false);
	}

	// Token: 0x060014C9 RID: 5321 RVA: 0x000FCF34 File Offset: 0x000FB134
	private void InitializeTrainingRankingWindow()
	{
		this.trainingRankingWindowBaseObj = Object.Instantiate<GameObject>((GameObject)AssetManager.GetAssetData("SceneTraining/GUI/Prefab/GUI_Training_Window"), Singleton<CanvasManager>.Instance.SystemMiddleArea);
		this.trainingRankingWindowBaseObj.GetComponent<PguiPanel>().raycastTarget = false;
		this.trainingRankingWindow = new SceneTraining.WIN_SEASON(this.trainingRankingWindowBaseObj.transform.Find("Window_SeasonRank"));
		this.trainingRankingWindow.scroll.InitForce();
		ReuseScroll scroll = this.trainingRankingWindow.scroll;
		scroll.onStartItem = (Action<int, GameObject>)Delegate.Combine(scroll.onStartItem, new Action<int, GameObject>(this.SetupSeason));
		ReuseScroll scroll2 = this.trainingRankingWindow.scroll;
		scroll2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scroll2.onUpdateItem, new Action<int, GameObject>(this.UpdateSeason));
		this.trainingRankingWindow.scroll.Setup(0, 0);
		this.trainingRankingWindow.win.Setup(null, null, null, true, null, null, false);
		Object.Destroy(this.trainingRankingWindowBaseObj.transform.Find("Window_GetItem").gameObject);
		Object.Destroy(this.trainingRankingWindowBaseObj.transform.Find("Window_ScoreRank").gameObject);
		Object.Destroy(this.trainingRankingWindowBaseObj.transform.Find("Window_MyScore").gameObject);
		Object.Destroy(this.trainingRankingWindowBaseObj.transform.Find("Window_UserParty").gameObject);
	}

	// Token: 0x060014CA RID: 5322 RVA: 0x000FD0A3 File Offset: 0x000FB2A3
	private IEnumerator SeasonDisp()
	{
		if (this.trainingSeasonId == 0)
		{
			DataManager.DmTraining.RequestGetTrainingInfo();
			do
			{
				yield return null;
			}
			while (DataManager.IsServerRequesting());
			TrainingStaticData staticData = DataManager.DmTraining.GetTrainingPackData().staticData;
			this.trainingSeasonId = staticData.SeasonId;
		}
		DataManager.DmTraining.RequestGetSeasonTrainingRanking(this.trainingSeasonId);
		do
		{
			yield return null;
		}
		while (DataManager.IsServerRequesting());
		this.trainingRankingWindow.win.ForceOpen();
		this.SeasonListDisp(this.trainingRankingWindow.tab.SelectIndex);
		yield break;
	}

	// Token: 0x060014CB RID: 5323 RVA: 0x000FD0B4 File Offset: 0x000FB2B4
	private void SeasonListDisp(int idx)
	{
		idx = 1 - idx;
		SeasonTrainingRankingData seasonTrainingRankingData = ((idx >= 0 && idx < DataManager.DmTraining.GetSeasonTrainingRankingData().Count) ? DataManager.DmTraining.GetSeasonTrainingRankingData()[idx] : null);
		bool flag = idx <= 0;
		this.trainingRankingWindow.resultConfirm.SetActive(seasonTrainingRankingData != null && !flag && seasonTrainingRankingData.isTallyFinish);
		this.trainingRankingWindow.resultCounting.SetActive(seasonTrainingRankingData != null && !flag && !seasonTrainingRankingData.isTallyFinish);
		DateTime dateTime = ((seasonTrainingRankingData == null) ? new DateTime(1900, 1, 1) : seasonTrainingRankingData.lastUpdateTime);
		this.trainingRankingWindow.lastDate.text = ((dateTime.Year >= 2000 && flag) ? dateTime.ToString("最終更新\u3000yyyy/MM/dd\u3000HH:mm") : "");
		dateTime = ((seasonTrainingRankingData == null) ? new DateTime(1900, 1, 1) : DataManager.DmTraining.GetTrainingEndTime(seasonTrainingRankingData.seasonId));
		this.trainingRankingWindow.seasonDate.text = ((dateTime.Year >= 2000) ? dateTime.ToString("yyyy/MM/dd\u3000HH:mm\u3000まで") : "");
		this.trainingRankingWindow.myRank.SetActive(seasonTrainingRankingData != null && seasonTrainingRankingData.myRankingNo > 0);
		if (seasonTrainingRankingData != null)
		{
			this.trainingRankingWindow.myRankNo.text = "自分の順位<size=28>" + seasonTrainingRankingData.myRankingNo.ToString() + "</size>";
		}
		this.trainingSeasonList = ((seasonTrainingRankingData == null) ? null : seasonTrainingRankingData.rankingList);
		this.trainingRankingWindow.scroll.Resize((this.trainingSeasonList == null) ? 0 : this.trainingSeasonList.Count, 0);
		this.trainingRankingWindow.noRank.SetActive(this.trainingSeasonList == null || this.trainingSeasonList.Count <= 0);
	}

	// Token: 0x060014CC RID: 5324 RVA: 0x000FD28D File Offset: 0x000FB48D
	private void SetupSeason(int index, GameObject go)
	{
		this.UpdateSeason(index, go);
	}

	// Token: 0x060014CD RID: 5325 RVA: 0x000FD298 File Offset: 0x000FB498
	private void UpdateSeason(int index, GameObject go)
	{
		SeasonTrainingRankingData.RankingOne rankingOne = ((this.trainingSeasonList != null && index >= 0 && index < this.trainingSeasonList.Count) ? this.trainingSeasonList[index] : null);
		Transform transform = go.transform.Find("Num_Rank");
		transform.Find("Rank_1").gameObject.SetActive(rankingOne != null && rankingOne.number <= 1);
		transform.Find("Rank_2").gameObject.SetActive(rankingOne != null && rankingOne.number == 2);
		transform.Find("Rank_3").gameObject.SetActive(rankingOne != null && rankingOne.number == 3);
		transform.Find("Rank_4_10").gameObject.SetActive(rankingOne != null && rankingOne.number >= 4 && rankingOne.number <= 10);
		transform.Find("Rank_11_100").gameObject.SetActive(rankingOne != null && rankingOne.number >= 11 && rankingOne.number <= 100);
		transform.Find("Rank_101_200").gameObject.SetActive(rankingOne != null && rankingOne.number >= 101);
		transform.Find("Rank_4_10/Num").GetComponent<PguiTextCtrl>().text = ((rankingOne == null) ? "" : rankingOne.number.ToString());
		transform.Find("Rank_11_100/Num").GetComponent<PguiTextCtrl>().text = ((rankingOne == null) ? "" : rankingOne.number.ToString());
		transform.Find("Rank_101_200/Num").GetComponent<PguiTextCtrl>().text = ((rankingOne == null) ? "" : rankingOne.number.ToString());
		transform = go.transform.Find("BaseImage");
		transform.Find("Num_Rank").GetComponent<PguiTextCtrl>().ReplaceTextByDefault("Param01", (rankingOne == null) ? "-" : rankingOne.userLevel.ToString());
		transform.Find("UserName").GetComponent<PguiTextCtrl>().text = ((rankingOne == null) ? "" : rankingOne.userName);
		transform.Find("Ranking/Num").GetComponent<PguiTextCtrl>().text = ((rankingOne == null) ? "0" : rankingOne.rankingPoint.ToString());
		transform.Find("Total/Num").GetComponent<PguiTextCtrl>().text = ((rankingOne == null) ? "0" : rankingOne.totalGoodScore.ToString());
		transform.Find("Achievement").GetComponent<AchievementCtrl>().Setup((rankingOne == null) ? 0 : rankingOne.achievementId, true, false);
		IconCharaCtrl iconCharaCtrl = transform.Find("Icon_Chara").GetComponentInChildren<IconCharaCtrl>();
		if (iconCharaCtrl == null)
		{
			iconCharaCtrl = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Chara).GetComponent<IconCharaCtrl>();
			iconCharaCtrl.transform.SetParent(transform.Find("Icon_Chara"), false);
		}
		iconCharaCtrl.Setup((rankingOne == null) ? null : CharaPackData.MakeInitial(rankingOne.favoriteCharaId), SortFilterDefine.SortType.INVALID, false, null, 0, (rankingOne == null) ? (-1) : rankingOne.favoriteCharaFaceId, 0);
		iconCharaCtrl.DispRanking();
	}

	// Token: 0x060014CE RID: 5326 RVA: 0x000FD5A8 File Offset: 0x000FB7A8
	private bool onClickTabSeason(int idx)
	{
		this.SeasonListDisp(idx);
		return true;
	}

	// Token: 0x040010ED RID: 4333
	private SelRankingCtrl.GUI guiData;

	// Token: 0x040010EE RID: 4334
	private List<SelRankingCtrl.RankingMenu> RankingMenuList;

	// Token: 0x040010EF RID: 4335
	private KemoStatusRankingWindowCtrl kemoStatusRankingWindowCtrl;

	// Token: 0x040010F0 RID: 4336
	private SceneTraining.WIN_SEASON trainingRankingWindow;

	// Token: 0x040010F1 RID: 4337
	private GameObject trainingRankingWindowBaseObj;

	// Token: 0x040010F2 RID: 4338
	private List<SeasonTrainingRankingData.RankingOne> trainingSeasonList;

	// Token: 0x040010F3 RID: 4339
	private int trainingSeasonId;

	// Token: 0x040010F4 RID: 4340
	private IEnumerator trainingRankingwindowOpen;

	// Token: 0x02000B9F RID: 2975
	public class GUI
	{
		// Token: 0x06004382 RID: 17282 RVA: 0x0020324C File Offset: 0x0020144C
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.ScrollView = baseTr.Find("Scenario_Chapter_Ranking/All/WindowAll/ScrollView").GetComponent<ReuseScroll>();
		}

		// Token: 0x04004840 RID: 18496
		public GameObject baseObj;

		// Token: 0x04004841 RID: 18497
		public ReuseScroll ScrollView;
	}

	// Token: 0x02000BA0 RID: 2976
	public class GUIMenuBar
	{
		// Token: 0x06004383 RID: 17283 RVA: 0x00203276 File Offset: 0x00201476
		public GUIMenuBar(Transform baseTr)
		{
			this.BaseObj = baseTr.gameObject;
			this.BarButton = baseTr.GetComponent<PguiButtonCtrl>();
			this.Txt_Title = baseTr.Find("BaseImage/Txt").GetComponent<PguiTextCtrl>();
		}

		// Token: 0x04004842 RID: 18498
		public GameObject BaseObj;

		// Token: 0x04004843 RID: 18499
		public PguiButtonCtrl BarButton;

		// Token: 0x04004844 RID: 18500
		public PguiTextCtrl Txt_Title;
	}

	// Token: 0x02000BA1 RID: 2977
	public class RankingMenu
	{
		// Token: 0x06004384 RID: 17284 RVA: 0x002032AC File Offset: 0x002014AC
		public RankingMenu(string title, UnityAction action, bool isSealed)
		{
			this.Title = title;
			this.Action = action;
			this.IsSealed = isSealed;
		}

		// Token: 0x04004845 RID: 18501
		public string Title;

		// Token: 0x04004846 RID: 18502
		public UnityAction Action;

		// Token: 0x04004847 RID: 18503
		public bool IsSealed;
	}
}
