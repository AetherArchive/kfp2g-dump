using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.Mst;
using UnityEngine;

// Token: 0x020001AC RID: 428
public class LoadAndTipsCtrl : MonoBehaviour
{
	// Token: 0x06001CDA RID: 7386 RVA: 0x0016A204 File Offset: 0x00168404
	public void Init()
	{
		this.guiBgFade = new LoadAndTipsCtrl.GUIBgFade(Object.Instantiate<GameObject>(Resources.Load("SceneLoading/GUI/Prefab/Loading_Fade") as GameObject, base.transform).transform);
		this.guiBgFade.baseObj.SetActive(false);
		this.guiLoad = new LoadAndTipsCtrl.GUILoad(Object.Instantiate<GameObject>(Resources.Load("SceneLoading/GUI/Prefab/Server_Loading") as GameObject, base.transform).transform);
		this.guiLoad.baseObj.SetActive(false);
		this.guiLoad.GageAll.gameObject.SetActive(false);
		this.guiTips = new LoadAndTipsCtrl.GUITips(Object.Instantiate<GameObject>(Resources.Load("SceneLoading/GUI/Prefab/GUI_Loading") as GameObject, base.transform).transform);
		this.guiTips.baseObj.SetActive(false);
		this.guiTips.Btn_Stop.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiTips.Btn_Stop.gameObject.SetActive(false);
	}

	// Token: 0x06001CDB RID: 7387 RVA: 0x0016A30B File Offset: 0x0016850B
	public void Setup(LoadAndTipsCtrl.SetupParam param)
	{
		if (this.setupInternal == null)
		{
			this.reqClose = false;
			this.reqFinish = false;
			this.setupParam = param;
			this.setupInternal = this.SetupAndAction(this.setupParam);
		}
	}

	// Token: 0x06001CDC RID: 7388 RVA: 0x0016A33C File Offset: 0x0016853C
	public bool isActive()
	{
		return this.setupInternal != null && !this.reqClose;
	}

	// Token: 0x06001CDD RID: 7389 RVA: 0x0016A351 File Offset: 0x00168551
	public void Close(bool isFinish)
	{
		this.reqClose = true;
		this.reqFinish = isFinish;
	}

	// Token: 0x06001CDE RID: 7390 RVA: 0x0016A361 File Offset: 0x00168561
	public void SetDispLoading(bool isDisp)
	{
		this.dispLoadingFlagExternal = isDisp;
	}

	// Token: 0x06001CDF RID: 7391 RVA: 0x0016A36C File Offset: 0x0016856C
	private void Update()
	{
		if (this.setupInternal != null && !this.setupInternal.MoveNext())
		{
			this.setupInternal = null;
		}
		if (this.guiLoad != null)
		{
			this.guiLoad.baseObj.SetActive(this.dispLoadingFlagInternal || this.dispLoadingFlagExternal);
			this.guiLoad.Cmn_Loading.gameObject.SetActive(this.dispLoadingFlagInternal || this.dispLoadingFlagExternal);
		}
	}

	// Token: 0x06001CE0 RID: 7392 RVA: 0x0016A3E4 File Offset: 0x001685E4
	private IEnumerator SetupAndAction(LoadAndTipsCtrl.SetupParam param)
	{
		this.guiBgFade.Loading_Fade.ExPauseAnimation(SimpleAnimation.ExPguiStatus.START, null);
		this.guiBgFade.baseObj.gameObject.SetActive(true);
		yield return null;
		if (param.isDispFade)
		{
			this.guiBgFade.Loading_Fade.ExResumeAnimation(null);
			while (this.guiBgFade.Loading_Fade.ExIsPlaying())
			{
				yield return null;
			}
		}
		this.guiLoad.baseObj.SetActive(true);
		List<MstTipsData> list;
		if (Singleton<DataManager>.Instance != null)
		{
			DataManagerServerMst dmServerMst = DataManager.DmServerMst;
			if (((dmServerMst != null) ? dmServerMst.mstTipsDataList : null) != null)
			{
				list = DataManager.DmServerMst.mstTipsDataList;
				goto IL_0146;
			}
		}
		if (Singleton<MstManager>.Instance != null)
		{
			list = Singleton<MstManager>.Instance.GetMst<List<MstTipsData>>(MstType.TIPS_DATA);
		}
		else
		{
			list = new List<MstTipsData>();
		}
		IL_0146:
		List<MstTipsData> enableMstTipsList = null;
		if (param.isDispTips && list.Count > 0)
		{
			enableMstTipsList = new List<MstTipsData>(list);
			long nowTimeByServer = PrjUtil.ConvertTicksToTime(TimeManager.Now.Ticks);
			List<int> enableQuestOneIdList = (Singleton<DataManager>.Instance.IsSetupData ? DataManager.DmQuest.GetPlayableQuestIdList(true) : new List<int>());
			enableMstTipsList.RemoveAll((MstTipsData item) => item.startDatetime > nowTimeByServer || item.endDatetime < nowTimeByServer || (item.questId != 0 && !enableQuestOneIdList.Contains(item.questId)));
			bool flag = false;
			if (param.nextSceneName == SceneManager.SceneName.ScenePvp)
			{
				ScenePvp.Args args = param.nextSceneArgs as ScenePvp.Args;
				if (args != null)
				{
					PvpStaticData pvpStaticDataBySeasonID = DataManager.DmPvp.GetPvpStaticDataBySeasonID(args.fastPvpSeasonId);
					flag = pvpStaticDataBySeasonID != null && pvpStaticDataBySeasonID.type == PvpStaticData.Type.SPECIAL;
				}
			}
			else if (param.nextSceneName == SceneManager.SceneName.ScenePvpDeck)
			{
				ScenePvpDeck.Args args2 = param.nextSceneArgs as ScenePvpDeck.Args;
				if (args2 != null)
				{
					PvpStaticData pvpStaticDataBySeasonID2 = DataManager.DmPvp.GetPvpStaticDataBySeasonID(args2.pvpSeasonId);
					flag = pvpStaticDataBySeasonID2 != null && pvpStaticDataBySeasonID2.type == PvpStaticData.Type.SPECIAL;
				}
			}
			else if (param.nextSceneName == SceneManager.SceneName.SceneBattle)
			{
				SceneBattleArgs sceneBattleArgs = param.nextSceneArgs as SceneBattleArgs;
				if (sceneBattleArgs != null)
				{
					PvpStaticData pvpStaticDataBySeasonID3 = DataManager.DmPvp.GetPvpStaticDataBySeasonID(sceneBattleArgs.pvpSeasonId);
					flag = pvpStaticDataBySeasonID3 != null && pvpStaticDataBySeasonID3.type == PvpStaticData.Type.SPECIAL;
				}
			}
			if (flag)
			{
				enableMstTipsList.RemoveAll((MstTipsData item) => item.dispType != 1);
			}
			else if (param.prevSceneName == SceneManager.SceneName.SceneTreeHouse || param.nextSceneName == SceneManager.SceneName.SceneTreeHouse)
			{
				enableMstTipsList.RemoveAll((MstTipsData item) => item.dispType != 2);
			}
			else
			{
				enableMstTipsList.RemoveAll((MstTipsData item) => item.dispType != 0);
			}
			if (0 < enableMstTipsList.Count)
			{
				MstTipsData mstTipsData;
				if (param.dispTipsId == 0)
				{
					int num = Random.Range(0, enableMstTipsList.Count);
					mstTipsData = enableMstTipsList[num];
				}
				else
				{
					mstTipsData = enableMstTipsList.Find((MstTipsData item) => item.id == param.dispTipsId);
				}
				this.DispTipsInternal(mstTipsData);
			}
		}
		this.guiTips.Btn_Stop.gameObject.SetActive(param.isDispStopButton);
		this.guiLoad.GageAll.gameObject.SetActive(param.isDispProgress);
		if (param.isDispProgress)
		{
			this.guiLoad.Num_Percent_Txt.text = "0%";
			this.guiLoad.Num_Score_Txt.text = "0/1";
			this.guiLoad.Gage.m_Image.fillAmount = 0f;
		}
		this.dispLoadingFlagInternal = true;
		yield return null;
		if (param.cbTipsDispFinish != null)
		{
			if (this.guiTips.baseObj.activeSelf)
			{
				float t = 0f;
				for (;;)
				{
					bool flag2 = (t += TimeManager.DeltaTime) > 1f;
					foreach (LoadAndTipsCtrl.GUITips.BaseData baseData in this.guiTips.baseDataList)
					{
						flag2 |= baseData.baseObj.activeSelf;
					}
					if (flag2)
					{
						break;
					}
					yield return null;
				}
			}
			param.cbTipsDispFinish();
		}
		this.guiTips.touchTemporary = false;
		while (!this.reqClose)
		{
			if (param.isDispProgress && param.cbGetProgress != null)
			{
				float num2 = param.cbGetProgress();
				this.guiLoad.Gage.m_Image.fillAmount = num2;
				this.guiLoad.Num_Percent_Txt.text = ((int)(num2 * 100f)).ToString() + "%";
				this.guiLoad.Num_Score_Txt.text = ((num2 >= 1f) ? "1/1" : "0/1");
			}
			if (this.guiTips.touchTemporary && enableMstTipsList != null && 0 < enableMstTipsList.Count)
			{
				int num3 = Random.Range(0, enableMstTipsList.Count);
				MstTipsData mstTipsData2 = enableMstTipsList[num3];
				this.DispTipsInternal(mstTipsData2);
				this.guiTips.touchTemporary = false;
			}
			yield return null;
		}
		if (param.isDispProgress && this.reqFinish)
		{
			this.guiLoad.Num_Score_Txt.text = "100%";
			this.guiLoad.Num_Score_Txt.text = "1/1";
		}
		this.dispLoadingFlagInternal = false;
		this.guiLoad.GageAll.gameObject.SetActive(false);
		this.guiTips.Btn_Stop.gameObject.SetActive(false);
		this.guiLoad.baseObj.SetActive(false);
		this.guiTips.baseObj.SetActive(false);
		if (param.isDispFade)
		{
			this.guiBgFade.Loading_Fade.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
			while (this.guiBgFade.Loading_Fade.ExIsPlaying())
			{
				yield return null;
			}
		}
		this.guiBgFade.baseObj.SetActive(false);
		yield return null;
		yield break;
	}

	// Token: 0x06001CE1 RID: 7393 RVA: 0x0016A3FC File Offset: 0x001685FC
	private void DispTipsInternal(MstTipsData tips)
	{
		if (tips != null)
		{
			LoadAndTipsCtrl.GUITips.BaseData baseData = null;
			foreach (LoadAndTipsCtrl.GUITips.BaseData baseData2 in this.guiTips.baseDataList)
			{
				baseData2.baseObj.SetActive(false);
			}
			if (tips.message != string.Empty && tips.fileName != string.Empty)
			{
				baseData = this.guiTips.baseDataList[2];
			}
			else if (tips.message != string.Empty)
			{
				baseData = this.guiTips.baseDataList[0];
			}
			else if (tips.fileName != string.Empty)
			{
				baseData = this.guiTips.baseDataList[1];
			}
			if (baseData != null)
			{
				this.guiTips.baseObj.SetActive(true);
				baseData.Num_Title_Txt.text = tips.title;
				if (baseData.Num_Txt != null)
				{
					baseData.Num_Txt.text = tips.message;
				}
				if (baseData.Texture_Tips != null)
				{
					baseData.Texture_Tips.SetRawImage(tips.fileName, true, false, delegate
					{
						baseData.baseObj.SetActive(true);
					});
					return;
				}
				baseData.baseObj.SetActive(true);
			}
		}
	}

	// Token: 0x06001CE2 RID: 7394 RVA: 0x0016A5A0 File Offset: 0x001687A0
	private void OnClickButton(PguiButtonCtrl button)
	{
		if (this.setupParam != null && this.setupParam.cbDownloadStop != null)
		{
			this.setupParam.cbDownloadStop();
		}
	}

	// Token: 0x06001CE3 RID: 7395 RVA: 0x0016A5C8 File Offset: 0x001687C8
	public void DebugSetup()
	{
		this.Init();
		LoadAndTipsCtrl.SetupParam setupParam = new LoadAndTipsCtrl.SetupParam();
		setupParam.dispTipsId = 0;
		setupParam.isDispFade = false;
		setupParam.isDispProgress = true;
		setupParam.isDispStopButton = true;
		setupParam.isDispTips = true;
		setupParam.cbGetProgress = () => Random.Range(0f, 1f);
		this.Setup(setupParam);
	}

	// Token: 0x0400156F RID: 5487
	private LoadAndTipsCtrl.GUIBgFade guiBgFade;

	// Token: 0x04001570 RID: 5488
	private LoadAndTipsCtrl.GUILoad guiLoad;

	// Token: 0x04001571 RID: 5489
	private LoadAndTipsCtrl.GUITips guiTips;

	// Token: 0x04001572 RID: 5490
	private IEnumerator setupInternal;

	// Token: 0x04001573 RID: 5491
	private bool reqClose;

	// Token: 0x04001574 RID: 5492
	private bool reqFinish;

	// Token: 0x04001575 RID: 5493
	private bool dispLoadingFlagInternal;

	// Token: 0x04001576 RID: 5494
	private bool dispLoadingFlagExternal;

	// Token: 0x04001577 RID: 5495
	private LoadAndTipsCtrl.SetupParam setupParam;

	// Token: 0x02000F26 RID: 3878
	public class GUIBgFade
	{
		// Token: 0x06004EC5 RID: 20165 RVA: 0x002375B2 File Offset: 0x002357B2
		public GUIBgFade(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Loading_Fade = baseTr.GetComponent<SimpleAnimation>();
		}

		// Token: 0x0400560F RID: 22031
		public GameObject baseObj;

		// Token: 0x04005610 RID: 22032
		public SimpleAnimation Loading_Fade;
	}

	// Token: 0x02000F27 RID: 3879
	public class GUILoad
	{
		// Token: 0x06004EC6 RID: 20166 RVA: 0x002375D4 File Offset: 0x002357D4
		public GUILoad(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.GageAll = baseTr.Find("Mask/GageAll").GetComponent<PguiImageCtrl>();
			this.Gage = baseTr.Find("Mask/GageAll/Gage").GetComponent<PguiImageCtrl>();
			this.Num_Score_Txt = baseTr.Find("Mask/GageAll/Num_Score_Txt").GetComponent<PguiTextCtrl>();
			this.Num_Percent_Txt = baseTr.Find("Mask/GageAll/Num_Percent_Txt").GetComponent<PguiTextCtrl>();
			this.Cmn_Loading = baseTr.Find("Mask/Cmn_Loading").GetComponent<PguiAECtrl>();
		}

		// Token: 0x04005611 RID: 22033
		public GameObject baseObj;

		// Token: 0x04005612 RID: 22034
		public PguiImageCtrl GageAll;

		// Token: 0x04005613 RID: 22035
		public PguiImageCtrl Gage;

		// Token: 0x04005614 RID: 22036
		public PguiTextCtrl Num_Score_Txt;

		// Token: 0x04005615 RID: 22037
		public PguiTextCtrl Num_Percent_Txt;

		// Token: 0x04005616 RID: 22038
		public PguiAECtrl Cmn_Loading;
	}

	// Token: 0x02000F28 RID: 3880
	public class GUITips
	{
		// Token: 0x06004EC7 RID: 20167 RVA: 0x00237664 File Offset: 0x00235864
		public GUITips(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_Stop = baseTr.Find("Btn_Stop").GetComponent<PguiButtonCtrl>();
			this.baseDataList = new List<LoadAndTipsCtrl.GUITips.BaseData>
			{
				new LoadAndTipsCtrl.GUITips.BaseData(baseTr.Find("Txt_Base")),
				new LoadAndTipsCtrl.GUITips.BaseData(baseTr.Find("Tips_Base")),
				new LoadAndTipsCtrl.GUITips.BaseData(baseTr.Find("Screenshot_Base"))
			};
			PrjUtil.AddTouchEventTrigger(baseTr.Find("TouchMask").gameObject, delegate(Transform tr)
			{
				this.touchTemporary = true;
			});
		}

		// Token: 0x04005617 RID: 22039
		public GameObject baseObj;

		// Token: 0x04005618 RID: 22040
		public List<LoadAndTipsCtrl.GUITips.BaseData> baseDataList;

		// Token: 0x04005619 RID: 22041
		public PguiButtonCtrl Btn_Stop;

		// Token: 0x0400561A RID: 22042
		public bool touchTemporary;

		// Token: 0x020011F9 RID: 4601
		public enum DataIndex
		{
			// Token: 0x04006287 RID: 25223
			TEXT,
			// Token: 0x04006288 RID: 25224
			IMAGE,
			// Token: 0x04006289 RID: 25225
			MIX
		}

		// Token: 0x020011FA RID: 4602
		public class BaseData
		{
			// Token: 0x06005770 RID: 22384 RVA: 0x002577CC File Offset: 0x002559CC
			public BaseData(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				Transform transform = baseTr.Find("Texture_Tips");
				this.Texture_Tips = ((transform != null) ? transform.GetComponent<PguiRawImageCtrl>() : null);
				Transform transform2 = baseTr.Find("Num_Title_Txt");
				this.Num_Title_Txt = ((transform2 != null) ? transform2.GetComponent<PguiTextCtrl>() : null);
				Transform transform3 = baseTr.Find("Num_Txt");
				this.Num_Txt = ((transform3 != null) ? transform3.GetComponent<PguiTextCtrl>() : null);
			}

			// Token: 0x0400628A RID: 25226
			public GameObject baseObj;

			// Token: 0x0400628B RID: 25227
			public PguiRawImageCtrl Texture_Tips;

			// Token: 0x0400628C RID: 25228
			public PguiTextCtrl Num_Title_Txt;

			// Token: 0x0400628D RID: 25229
			public PguiTextCtrl Num_Txt;
		}
	}

	// Token: 0x02000F29 RID: 3881
	public class SetupParam
	{
		// Token: 0x0400561B RID: 22043
		public bool isDispStopButton;

		// Token: 0x0400561C RID: 22044
		public bool isDispFade;

		// Token: 0x0400561D RID: 22045
		public bool isDispProgress;

		// Token: 0x0400561E RID: 22046
		public bool isDispTips;

		// Token: 0x0400561F RID: 22047
		public int dispTipsId;

		// Token: 0x04005620 RID: 22048
		public SceneManager.SceneName prevSceneName;

		// Token: 0x04005621 RID: 22049
		public SceneManager.SceneName nextSceneName;

		// Token: 0x04005622 RID: 22050
		public object nextSceneArgs;

		// Token: 0x04005623 RID: 22051
		public LoadAndTipsCtrl.SetupParam.GetProgress cbGetProgress;

		// Token: 0x04005624 RID: 22052
		public LoadAndTipsCtrl.SetupParam.TipsDispFinish cbTipsDispFinish;

		// Token: 0x04005625 RID: 22053
		public LoadAndTipsCtrl.SetupParam.DownloadStop cbDownloadStop;

		// Token: 0x020011FB RID: 4603
		// (Invoke) Token: 0x06005772 RID: 22386
		public delegate float GetProgress();

		// Token: 0x020011FC RID: 4604
		// (Invoke) Token: 0x06005776 RID: 22390
		public delegate void DownloadStop();

		// Token: 0x020011FD RID: 4605
		// (Invoke) Token: 0x0600577A RID: 22394
		public delegate void TipsDispFinish();
	}
}
