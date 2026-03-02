using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using UnityEngine;

// Token: 0x020001BA RID: 442
public class SelMissionProgressCtrl : MonoBehaviour
{
	// Token: 0x17000421 RID: 1057
	// (get) Token: 0x06001E59 RID: 7769 RVA: 0x0017AABF File Offset: 0x00178CBF
	// (set) Token: 0x06001E5A RID: 7770 RVA: 0x0017AAC7 File Offset: 0x00178CC7
	public bool IsPlayingGachaAuth { get; set; }

	// Token: 0x17000422 RID: 1058
	// (get) Token: 0x06001E5B RID: 7771 RVA: 0x0017AAD0 File Offset: 0x00178CD0
	// (set) Token: 0x06001E5C RID: 7772 RVA: 0x0017AAD8 File Offset: 0x00178CD8
	public bool IsPresentReceiveAuth { get; set; }

	// Token: 0x17000423 RID: 1059
	// (get) Token: 0x06001E5D RID: 7773 RVA: 0x0017AAE1 File Offset: 0x00178CE1
	// (set) Token: 0x06001E5E RID: 7774 RVA: 0x0017AAE9 File Offset: 0x00178CE9
	public bool IsPhotoGrow { get; set; }

	// Token: 0x17000424 RID: 1060
	// (get) Token: 0x06001E5F RID: 7775 RVA: 0x0017AAF2 File Offset: 0x00178CF2
	// (set) Token: 0x06001E60 RID: 7776 RVA: 0x0017AAFA File Offset: 0x00178CFA
	public bool IsWildRelease { get; set; }

	// Token: 0x17000425 RID: 1061
	// (get) Token: 0x06001E61 RID: 7777 RVA: 0x0017AB03 File Offset: 0x00178D03
	// (set) Token: 0x06001E62 RID: 7778 RVA: 0x0017AB0B File Offset: 0x00178D0B
	public bool IsMiracleUp { get; set; }

	// Token: 0x17000426 RID: 1062
	// (get) Token: 0x06001E63 RID: 7779 RVA: 0x0017AB14 File Offset: 0x00178D14
	// (set) Token: 0x06001E64 RID: 7780 RVA: 0x0017AB1C File Offset: 0x00178D1C
	public bool IsAfterQuestSkip { get; set; }

	// Token: 0x06001E65 RID: 7781 RVA: 0x0017AB28 File Offset: 0x00178D28
	public void Init()
	{
		this.guiData = new SelMissionProgressCtrl.GUI(AssetManager.InstantiateAssetData("Cmn/GUI/Prefab/Cmn_MissionInfo", base.transform).transform);
		this.guiData.MissionInfo.ExPauseAnimation(SimpleAnimation.ExPguiStatus.START, null);
		this.guiData.baseObj.SetActive(false);
		this.guiData.Cmn_Btn_Close.AddOnClickListener(delegate(PguiButtonCtrl x)
		{
			this.OnTouchRect();
		}, PguiButtonCtrl.SoundType.INVALID);
	}

	// Token: 0x06001E66 RID: 7782 RVA: 0x0017AB98 File Offset: 0x00178D98
	public void PushProgress(UserMissionOne mission)
	{
		this.requestMissionQueue.RemoveAll((UserMissionOne itm) => itm.missionId == mission.missionId);
		this.requestMissionQueue.Add(mission);
	}

	// Token: 0x06001E67 RID: 7783 RVA: 0x0017ABDB File Offset: 0x00178DDB
	public void ClaerProgress()
	{
		this.requestMissionQueue.Clear();
	}

	// Token: 0x06001E68 RID: 7784 RVA: 0x0017ABE8 File Offset: 0x00178DE8
	private void Update()
	{
		SceneManager.SceneName sceneName = ((Singleton<SceneManager>.Instance != null) ? Singleton<SceneManager>.Instance.CurrentSceneName : SceneManager.SceneName.None);
		if (this.currentPopupMission != null)
		{
			if (!this.currentPopupMission.MoveNext())
			{
				this.currentPopupMission = null;
				if ((sceneName == SceneManager.SceneName.SceneScenario || sceneName == SceneManager.SceneName.SceneBattle || (sceneName == SceneManager.SceneName.SceneBattleResult && !this.IsAfterQuestSkip) || this.IsPlayingGachaAuth || this.IsPresentReceiveAuth || this.IsPhotoGrow || this.IsWildRelease || this.IsMiracleUp) && this.requestMissionQueue.Count > 0)
				{
					this.requestMissionQueue = new List<UserMissionOne>();
					return;
				}
			}
		}
		else if (sceneName != SceneManager.SceneName.SceneScenario && sceneName != SceneManager.SceneName.SceneBattle && (sceneName != SceneManager.SceneName.SceneBattleResult || this.IsAfterQuestSkip) && !this.IsPlayingGachaAuth && !this.IsPresentReceiveAuth && !this.IsPhotoGrow && !this.IsWildRelease && !this.IsMiracleUp && this.requestMissionQueue.Count > 0)
		{
			UserMissionOne userMissionOne = this.requestMissionQueue[0];
			this.requestMissionQueue.RemoveAt(0);
			this.currentPopupMission = this.PopupMission(userMissionOne);
		}
	}

	// Token: 0x06001E69 RID: 7785 RVA: 0x0017ACFA File Offset: 0x00178EFA
	private IEnumerator PopupMission(UserMissionOne mission)
	{
		if (DataManager.DmUserInfo.optionData.MissionProgressNotify == 0 || (1 == DataManager.DmUserInfo.optionData.MissionProgressNotify && mission.isClear))
		{
			this.guiData.Num_Mission.text = mission.numerator.ToString() + "/" + mission.denominator.ToString();
			this.guiData.Txt_Mission.text = mission.missionContents;
			this.guiData.Gage.m_Image.fillAmount = (float)mission.numerator / (float)mission.denominator;
			this.guiData.baseObj.SetActive(true);
			this.guiData.AEImage_Clear.gameObject.SetActive(false);
			this.guiData.MissionInfo.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
			while (this.guiData.MissionInfo.ExIsPlaying() && !this.forceAnimEnd)
			{
				yield return null;
			}
			if (mission.isClear)
			{
				this.guiData.AEImage_Clear.gameObject.SetActive(true);
				this.guiData.AEImage_Clear.PlayAnimation(PguiAECtrl.AmimeType.START, null);
				SoundManager.Play("prd_se_selector_mission_progress_clear", false, false);
			}
			long waitTime = TimeManager.Now.Ticks + TimeManager.Second2Tick(2L);
			while (waitTime > TimeManager.Now.Ticks && !this.forceAnimEnd)
			{
				yield return null;
			}
			this.guiData.MissionInfo.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
			while (this.guiData.MissionInfo.ExIsPlaying() && !this.forceAnimEnd)
			{
				yield return null;
			}
		}
		this.forceAnimEnd = false;
		this.guiData.baseObj.SetActive(false);
		yield break;
	}

	// Token: 0x06001E6A RID: 7786 RVA: 0x0017AD10 File Offset: 0x00178F10
	private void OnTouchRect()
	{
		this.forceAnimEnd = true;
	}

	// Token: 0x04001631 RID: 5681
	private SelMissionProgressCtrl.GUI guiData;

	// Token: 0x04001632 RID: 5682
	private bool forceAnimEnd;

	// Token: 0x04001633 RID: 5683
	private IEnumerator currentPopupMission;

	// Token: 0x04001634 RID: 5684
	private List<UserMissionOne> requestMissionQueue = new List<UserMissionOne>();

	// Token: 0x02000FAB RID: 4011
	public class GUI
	{
		// Token: 0x0600508C RID: 20620 RVA: 0x00241A0C File Offset: 0x0023FC0C
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Gage = baseTr.Find("MissionInfo/GageAll/Gage").GetComponent<PguiImageCtrl>();
			this.Txt_Mission = baseTr.Find("MissionInfo/Txt_Mission").GetComponent<PguiTextCtrl>();
			this.Num_Mission = baseTr.Find("MissionInfo/Num_Mission").GetComponent<PguiTextCtrl>();
			this.MissionInfo = baseTr.Find("MissionInfo").GetComponent<SimpleAnimation>();
			this.AEImage_Clear = baseTr.Find("MissionInfo/AEImage_Clear").GetComponent<PguiAECtrl>();
			this.Cmn_Btn_Close = baseTr.Find("MissionInfo/Cmn_Btn_Close").GetComponent<PguiButtonCtrl>();
		}

		// Token: 0x04005869 RID: 22633
		public GameObject baseObj;

		// Token: 0x0400586A RID: 22634
		public PguiImageCtrl Gage;

		// Token: 0x0400586B RID: 22635
		public PguiTextCtrl Txt_Mission;

		// Token: 0x0400586C RID: 22636
		public PguiTextCtrl Num_Mission;

		// Token: 0x0400586D RID: 22637
		public SimpleAnimation MissionInfo;

		// Token: 0x0400586E RID: 22638
		public PguiAECtrl AEImage_Clear;

		// Token: 0x0400586F RID: 22639
		public PguiButtonCtrl Cmn_Btn_Close;
	}
}
