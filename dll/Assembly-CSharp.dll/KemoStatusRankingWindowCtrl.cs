using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000156 RID: 342
public class KemoStatusRankingWindowCtrl : MonoBehaviour
{
	// Token: 0x060013B2 RID: 5042 RVA: 0x000F23BA File Offset: 0x000F05BA
	private void Update()
	{
		if (this.IEWindowMove != null && !this.IEWindowMove.MoveNext())
		{
			this.IEWindowMove = null;
		}
	}

	// Token: 0x060013B3 RID: 5043 RVA: 0x000F23D8 File Offset: 0x000F05D8
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

	// Token: 0x060013B4 RID: 5044 RVA: 0x000F247F File Offset: 0x000F067F
	public void Open(KemoStatusRankingWindowCtrl.SetupParam setupParam)
	{
		this.windowCloseEndCb = setupParam.closeEndCb;
		this.IEWindowMove = this.OpenWindow(setupParam.openEndCb);
	}

	// Token: 0x060013B5 RID: 5045 RVA: 0x000F249F File Offset: 0x000F069F
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

	// Token: 0x060013B6 RID: 5046 RVA: 0x000F24B5 File Offset: 0x000F06B5
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

	// Token: 0x060013B7 RID: 5047 RVA: 0x000F24C4 File Offset: 0x000F06C4
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

	// Token: 0x060013B8 RID: 5048 RVA: 0x000F2608 File Offset: 0x000F0808
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

	// Token: 0x060013B9 RID: 5049 RVA: 0x000F2629 File Offset: 0x000F0829
	private void OnStartItem(int index, GameObject go)
	{
		Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Chara, go.transform.Find("BaseImage/Icon_Chara")).name = "Icon_Chara";
	}

	// Token: 0x060013BA RID: 5050 RVA: 0x000F2654 File Offset: 0x000F0854
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

	// Token: 0x04001050 RID: 4176
	public KemoStatusRankingWindowCtrl.GUI guiData;

	// Token: 0x04001051 RID: 4177
	private IEnumerator IEWindowMove;

	// Token: 0x04001052 RID: 4178
	private UnityAction windowCloseEndCb;

	// Token: 0x04001053 RID: 4179
	private KemoStatusRankingData rankingData;

	// Token: 0x02000B42 RID: 2882
	public class GUI
	{
		// Token: 0x06004248 RID: 16968 RVA: 0x001FF218 File Offset: 0x001FD418
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

		// Token: 0x040046AC RID: 18092
		public GameObject baseObj;

		// Token: 0x040046AD RID: 18093
		public PguiOpenWindowCtrl owCtrl;

		// Token: 0x040046AE RID: 18094
		public ReuseScroll ScrollView;

		// Token: 0x040046AF RID: 18095
		public GameObject Text_NoRank;

		// Token: 0x040046B0 RID: 18096
		public PguiTextCtrl Text_LastUpdateDateTime;

		// Token: 0x040046B1 RID: 18097
		public PguiTextCtrl Text_MyRank;

		// Token: 0x040046B2 RID: 18098
		public PguiTextCtrl Text_TotalKemoStatusNum;
	}

	// Token: 0x02000B43 RID: 2883
	public class RankingBarGUI
	{
		// Token: 0x06004249 RID: 16969 RVA: 0x001FF2BC File Offset: 0x001FD4BC
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

		// Token: 0x0600424A RID: 16970 RVA: 0x001FF404 File Offset: 0x001FD604
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

		// Token: 0x0600424B RID: 16971 RVA: 0x001FF48C File Offset: 0x001FD68C
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

		// Token: 0x040046B3 RID: 18099
		public GameObject baseObj;

		// Token: 0x040046B4 RID: 18100
		private List<KemoStatusRankingWindowCtrl.RankingBarGUI.RankIcon> RankIconList;

		// Token: 0x040046B5 RID: 18101
		public PguiTextCtrl Text_Lv;

		// Token: 0x040046B6 RID: 18102
		public PguiTextCtrl Text_Name;

		// Token: 0x040046B7 RID: 18103
		public PguiTextCtrl Text_KemoStatusNum;

		// Token: 0x040046B8 RID: 18104
		public IconCharaCtrl IconChara;

		// Token: 0x040046B9 RID: 18105
		public AchievementCtrl Achievement;

		// Token: 0x0200118E RID: 4494
		public class RankIcon
		{
			// Token: 0x06005699 RID: 22169 RVA: 0x00252CE2 File Offset: 0x00250EE2
			public RankIcon(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Text_Num = baseTr.Find("Num").GetComponent<PguiTextCtrl>();
			}

			// Token: 0x04006031 RID: 24625
			public GameObject baseObj;

			// Token: 0x04006032 RID: 24626
			public PguiTextCtrl Text_Num;
		}
	}

	// Token: 0x02000B44 RID: 2884
	public class SetupParam
	{
		// Token: 0x0600424C RID: 16972 RVA: 0x001FF4DE File Offset: 0x001FD6DE
		public SetupParam()
		{
			this.openEndCb = null;
			this.closeEndCb = null;
		}

		// Token: 0x040046BA RID: 18106
		public UnityAction openEndCb;

		// Token: 0x040046BB RID: 18107
		public UnityAction closeEndCb;
	}
}
