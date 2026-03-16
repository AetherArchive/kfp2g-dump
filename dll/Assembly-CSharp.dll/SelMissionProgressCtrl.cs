using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using UnityEngine;

public class SelMissionProgressCtrl : MonoBehaviour
{
	public bool IsPlayingGachaAuth { get; set; }

	public bool IsPresentReceiveAuth { get; set; }

	public bool IsPhotoGrow { get; set; }

	public bool IsWildRelease { get; set; }

	public bool IsMiracleUp { get; set; }

	public bool IsAfterQuestSkip { get; set; }

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

	public void PushProgress(UserMissionOne mission)
	{
		this.requestMissionQueue.RemoveAll((UserMissionOne itm) => itm.missionId == mission.missionId);
		this.requestMissionQueue.Add(mission);
	}

	public void ClaerProgress()
	{
		this.requestMissionQueue.Clear();
	}

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

	private void OnTouchRect()
	{
		this.forceAnimEnd = true;
	}

	private SelMissionProgressCtrl.GUI guiData;

	private bool forceAnimEnd;

	private IEnumerator currentPopupMission;

	private List<UserMissionOne> requestMissionQueue = new List<UserMissionOne>();

	public class GUI
	{
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

		public GameObject baseObj;

		public PguiImageCtrl Gage;

		public PguiTextCtrl Txt_Mission;

		public PguiTextCtrl Num_Mission;

		public SimpleAnimation MissionInfo;

		public PguiAECtrl AEImage_Clear;

		public PguiButtonCtrl Cmn_Btn_Close;
	}
}
