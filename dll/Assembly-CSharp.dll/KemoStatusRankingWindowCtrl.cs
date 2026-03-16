using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;

public class KemoStatusRankingWindowCtrl : MonoBehaviour
{
	private void Update()
	{
		if (this.IEWindowMove != null && !this.IEWindowMove.MoveNext())
		{
			this.IEWindowMove = null;
		}
	}

	public void Initialize()
	{
		this.guiData = new KemoStatusRankingWindowCtrl.GUI(base.transform.Find("Window_TotalStatusRank").transform);
		this.guiData.ScrollView.InitForce();
		ReuseScroll scrollView = this.guiData.ScrollView;
		scrollView.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollView.onStartItem, new Action<int, GameObject>(this.OnStartItem));
		ReuseScroll scrollView2 = this.guiData.ScrollView;
		scrollView2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollView2.onUpdateItem, new Action<int, GameObject>(this.OnUpdateItem));
		this.guiData.ScrollView.Setup(0, 0);
	}

	public void Open(KemoStatusRankingWindowCtrl.SetupParam setupParam)
	{
		this.windowCloseEndCb = setupParam.closeEndCb;
		this.IEWindowMove = this.OpenWindow(setupParam.openEndCb);
	}

	private IEnumerator OpenWindow(UnityAction openEndCb)
	{
		IEnumerator enumerator = this.GetRankingData();
		do
		{
			yield return null;
		}
		while (enumerator.MoveNext());
		this.RefreshInfo();
		this.guiData.owCtrl.Setup("けもステランキング", null, null, true, new PguiOpenWindowCtrl.Callback(this.OnClickOwButton), null, false);
		this.guiData.owCtrl.Open();
		if (openEndCb != null)
		{
			openEndCb();
		}
		yield break;
	}

	private IEnumerator GetRankingData()
	{
		DataManager.DmGameStatus.RequestGetKemoStatusRanking();
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		this.rankingData = DataManager.DmGameStatus.GetKemoStatusRankingData();
		yield break;
	}

	private void RefreshInfo()
	{
		this.guiData.Text_LastUpdateDateTime.ReplaceTextByDefault("Param01", TimeManager.FormattedTime(this.rankingData.lastUpdateTime, TimeManager.Format.yyyyMMdd_hhmm));
		this.guiData.Text_MyRank.transform.parent.gameObject.SetActive(0 < this.rankingData.myRank && this.rankingData.myRank <= 100);
		if (this.guiData.Text_MyRank.transform.parent.gameObject.activeSelf)
		{
			this.guiData.Text_MyRank.ReplaceTextByDefault("Param01", this.rankingData.myRank.ToString());
		}
		this.guiData.Text_TotalKemoStatusNum.text = DataManager.DmChara.UserAllCharaKemoStatus.ToString();
		this.guiData.ScrollView.Clear();
		this.guiData.ScrollView.Setup(this.rankingData.rankingList.Count, 0);
		this.guiData.ScrollView.Refresh();
		this.guiData.Text_NoRank.SetActive(this.rankingData.rankingList.Count <= 0);
	}

	private bool OnClickOwButton(int index)
	{
		if (-1 == index)
		{
			UnityAction unityAction = this.windowCloseEndCb;
			if (unityAction != null)
			{
				unityAction();
			}
			this.windowCloseEndCb = null;
			return true;
		}
		return false;
	}

	private void OnStartItem(int index, GameObject go)
	{
		Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Chara, go.transform.Find("BaseImage/Icon_Chara")).name = "Icon_Chara";
	}

	private void OnUpdateItem(int index, GameObject go)
	{
		if (index < this.rankingData.rankingList.Count)
		{
			go.SetActive(true);
			KemoStatusRankingWindowCtrl.RankingBarGUI rankingBarGUI = new KemoStatusRankingWindowCtrl.RankingBarGUI(go.transform);
			KemoStatusRankingData.RankingOne rankingOne = this.rankingData.rankingList[index];
			rankingBarGUI.DispRankIcon(rankingOne.number);
			rankingBarGUI.Text_Lv.ReplaceTextByDefault("Param01", rankingOne.userLevel.ToString());
			rankingBarGUI.Text_Name.text = rankingOne.userName;
			rankingBarGUI.Text_KemoStatusNum.text = rankingOne.kemoStatus.ToString();
			rankingBarGUI.IconChara.Setup(CharaPackData.MakeInitial(rankingOne.favoriteCharaId), SortFilterDefine.SortType.INVALID, false, null, 0, rankingOne.favoriteCharaFaceId, 0);
			rankingBarGUI.IconChara.DispRanking();
			rankingBarGUI.Achievement.Setup(rankingOne.achievementId, true, false);
			return;
		}
		go.SetActive(false);
	}

	public KemoStatusRankingWindowCtrl.GUI guiData;

	private IEnumerator IEWindowMove;

	private UnityAction windowCloseEndCb;

	private KemoStatusRankingData rankingData;

	public class GUI
	{
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.Find("Base").gameObject;
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.ScrollView = baseTr.Find("Base/Window/Tab_All/Rank/ScrollView").GetComponent<ReuseScroll>();
			this.Text_NoRank = baseTr.Find("Base/Window/Tab_All/Txt_NoRank").gameObject;
			this.Text_LastUpdateDateTime = baseTr.Find("Base/Window/Text_Date").GetComponent<PguiTextCtrl>();
			this.Text_MyRank = baseTr.Find("Base/Window/MyRank/Txt").GetComponent<PguiTextCtrl>();
			this.Text_TotalKemoStatusNum = baseTr.Find("Base/Window/Total/Num").GetComponent<PguiTextCtrl>();
		}

		public GameObject baseObj;

		public PguiOpenWindowCtrl owCtrl;

		public ReuseScroll ScrollView;

		public GameObject Text_NoRank;

		public PguiTextCtrl Text_LastUpdateDateTime;

		public PguiTextCtrl Text_MyRank;

		public PguiTextCtrl Text_TotalKemoStatusNum;
	}

	public class RankingBarGUI
	{
		public RankingBarGUI(Transform baseTr)
		{
			this.RankIconList = new List<KemoStatusRankingWindowCtrl.RankingBarGUI.RankIcon>();
			this.baseObj = baseTr.gameObject;
			foreach (string text in new List<string> { "Rank_1", "Rank_2", "Rank_3", "Rank_4_10", "Rank_11_100", "Rank_101_200" })
			{
				KemoStatusRankingWindowCtrl.RankingBarGUI.RankIcon rankIcon = new KemoStatusRankingWindowCtrl.RankingBarGUI.RankIcon(baseTr.Find("Num_Rank/" + text));
				rankIcon.baseObj.SetActive(false);
				this.RankIconList.Add(rankIcon);
			}
			this.Text_Lv = baseTr.Find("BaseImage/Num_Rank").GetComponent<PguiTextCtrl>();
			this.Text_Name = baseTr.Find("BaseImage/UserName").GetComponent<PguiTextCtrl>();
			this.Text_KemoStatusNum = baseTr.Find("BaseImage/Total/Num").GetComponent<PguiTextCtrl>();
			this.IconChara = baseTr.Find("BaseImage/Icon_Chara/Icon_Chara").GetComponent<IconCharaCtrl>();
			this.Achievement = baseTr.Find("BaseImage/Achievement").GetComponent<AchievementCtrl>();
		}

		public void DispRankIcon(int rank)
		{
			KemoStatusRankingWindowCtrl.RankingBarGUI.<>c__DisplayClass9_0 CS$<>8__locals1;
			CS$<>8__locals1.rank = rank;
			int num = KemoStatusRankingWindowCtrl.RankingBarGUI.<DispRankIcon>g__GetRankIconListIndex|9_0(ref CS$<>8__locals1);
			for (int i = 0; i < this.RankIconList.Count; i++)
			{
				this.RankIconList[i].baseObj.SetActive(i == num);
				if (this.RankIconList[i].baseObj.activeSelf)
				{
					this.RankIconList[i].Text_Num.text = CS$<>8__locals1.rank.ToString();
				}
			}
		}

		[CompilerGenerated]
		internal static int <DispRankIcon>g__GetRankIconListIndex|9_0(ref KemoStatusRankingWindowCtrl.RankingBarGUI.<>c__DisplayClass9_0 A_0)
		{
			if (100 < A_0.rank)
			{
				return 5;
			}
			if (10 < A_0.rank)
			{
				return 4;
			}
			if (3 < A_0.rank)
			{
				return 3;
			}
			if (3 == A_0.rank)
			{
				return 2;
			}
			if (2 == A_0.rank)
			{
				return 1;
			}
			if (1 == A_0.rank)
			{
				return 0;
			}
			return 5;
		}

		public GameObject baseObj;

		private List<KemoStatusRankingWindowCtrl.RankingBarGUI.RankIcon> RankIconList;

		public PguiTextCtrl Text_Lv;

		public PguiTextCtrl Text_Name;

		public PguiTextCtrl Text_KemoStatusNum;

		public IconCharaCtrl IconChara;

		public AchievementCtrl Achievement;

		public class RankIcon
		{
			public RankIcon(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Text_Num = baseTr.Find("Num").GetComponent<PguiTextCtrl>();
			}

			public GameObject baseObj;

			public PguiTextCtrl Text_Num;
		}
	}

	public class SetupParam
	{
		public SetupParam()
		{
			this.openEndCb = null;
			this.closeEndCb = null;
		}

		public UnityAction openEndCb;

		public UnityAction closeEndCb;
	}
}
