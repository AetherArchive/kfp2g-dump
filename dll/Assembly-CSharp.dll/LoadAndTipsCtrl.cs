using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.Mst;
using UnityEngine;

public class LoadAndTipsCtrl : MonoBehaviour
{
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

	public bool isActive()
	{
		return this.setupInternal != null && !this.reqClose;
	}

	public void Close(bool isFinish)
	{
		this.reqClose = true;
		this.reqFinish = isFinish;
	}

	public void SetDispLoading(bool isDisp)
	{
		this.dispLoadingFlagExternal = isDisp;
	}

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

	private void OnClickButton(PguiButtonCtrl button)
	{
		if (this.setupParam != null && this.setupParam.cbDownloadStop != null)
		{
			this.setupParam.cbDownloadStop();
		}
	}

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

	private LoadAndTipsCtrl.GUIBgFade guiBgFade;

	private LoadAndTipsCtrl.GUILoad guiLoad;

	private LoadAndTipsCtrl.GUITips guiTips;

	private IEnumerator setupInternal;

	private bool reqClose;

	private bool reqFinish;

	private bool dispLoadingFlagInternal;

	private bool dispLoadingFlagExternal;

	private LoadAndTipsCtrl.SetupParam setupParam;

	public class GUIBgFade
	{
		public GUIBgFade(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Loading_Fade = baseTr.GetComponent<SimpleAnimation>();
		}

		public GameObject baseObj;

		public SimpleAnimation Loading_Fade;
	}

	public class GUILoad
	{
		public GUILoad(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.GageAll = baseTr.Find("Mask/GageAll").GetComponent<PguiImageCtrl>();
			this.Gage = baseTr.Find("Mask/GageAll/Gage").GetComponent<PguiImageCtrl>();
			this.Num_Score_Txt = baseTr.Find("Mask/GageAll/Num_Score_Txt").GetComponent<PguiTextCtrl>();
			this.Num_Percent_Txt = baseTr.Find("Mask/GageAll/Num_Percent_Txt").GetComponent<PguiTextCtrl>();
			this.Cmn_Loading = baseTr.Find("Mask/Cmn_Loading").GetComponent<PguiAECtrl>();
		}

		public GameObject baseObj;

		public PguiImageCtrl GageAll;

		public PguiImageCtrl Gage;

		public PguiTextCtrl Num_Score_Txt;

		public PguiTextCtrl Num_Percent_Txt;

		public PguiAECtrl Cmn_Loading;
	}

	public class GUITips
	{
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

		public GameObject baseObj;

		public List<LoadAndTipsCtrl.GUITips.BaseData> baseDataList;

		public PguiButtonCtrl Btn_Stop;

		public bool touchTemporary;

		public enum DataIndex
		{
			TEXT,
			IMAGE,
			MIX
		}

		public class BaseData
		{
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

			public GameObject baseObj;

			public PguiRawImageCtrl Texture_Tips;

			public PguiTextCtrl Num_Title_Txt;

			public PguiTextCtrl Num_Txt;
		}
	}

	public class SetupParam
	{
		public bool isDispStopButton;

		public bool isDispFade;

		public bool isDispProgress;

		public bool isDispTips;

		public int dispTipsId;

		public SceneManager.SceneName prevSceneName;

		public SceneManager.SceneName nextSceneName;

		public object nextSceneArgs;

		public LoadAndTipsCtrl.SetupParam.GetProgress cbGetProgress;

		public LoadAndTipsCtrl.SetupParam.TipsDispFinish cbTipsDispFinish;

		public LoadAndTipsCtrl.SetupParam.DownloadStop cbDownloadStop;

		public delegate float GetProgress();

		public delegate void DownloadStop();

		public delegate void TipsDispFinish();
	}
}
