using System;
using System.Collections.Generic;
using SGNFW.uGUI;
using UnityEngine;

public class RewardListWindowCtrl : MonoBehaviour
{
	private RewardListWindowCtrl.GUI GuiData { get; set; }

	private RewardListWindowCtrl.SuccessGUI SuccessGuiData { get; set; }

	private RewardListWindowCtrl.OtherTeamSuccessGUI OtherTeamSuccessGuiData { get; set; }

	private RewardListWindowCtrl.TimeOutGUI TimeOutGuiData { get; set; }

	private int MapId { get; set; }

	public void Init()
	{
		Transform transform = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneEvent/GUI/Prefab/GUI_Event_Multi_Window"), base.transform).transform;
		this.GuiData = new RewardListWindowCtrl.GUI(transform);
		this.SuccessGuiData = new RewardListWindowCtrl.SuccessGUI(transform.Find("Window_HardQuest_Success"));
		this.SuccessGuiData.baseObj.SetActive(false);
		this.SuccessGuiData.BtnClose.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.SuccessGuiData.ButtonC.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.OtherTeamSuccessGuiData = new RewardListWindowCtrl.OtherTeamSuccessGUI(transform.Find("Window_HardQuest_OtherTeamSuccess"));
		this.TimeOutGuiData = new RewardListWindowCtrl.TimeOutGUI(transform.Find("Window_HardQuest_TimeOut"));
		this.GuiData.ScrollView.InitForce();
		ReuseScroll scrollView = this.GuiData.ScrollView;
		scrollView.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollView.onStartItem, new Action<int, GameObject>(delegate(int index, GameObject go)
		{
			new RewardListWindowCtrl.GUI.ListBar(go.transform);
		}));
		ReuseScroll scrollView2 = this.GuiData.ScrollView;
		scrollView2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollView2.onUpdateItem, new Action<int, GameObject>(delegate(int index, GameObject go)
		{
			DataManagerEvent.CoopData.MapInfo mapInfo = DataManager.DmEvent.LastCoopInfo.MapInfoMap[this.MapId];
			List<DataManagerEvent.CoopConditionData> mapRewardConditionalDataList = mapInfo.MapRewardConditionalDataList;
			mapRewardConditionalDataList.Sort((DataManagerEvent.CoopConditionData a, DataManagerEvent.CoopConditionData b) => b.AchievementCondition.CompareTo(a.AchievementCondition));
			for (int i = 0; i < 1; i++)
			{
				int num = index + i;
				go.SetActive(num < mapRewardConditionalDataList.Count);
				if (num < mapRewardConditionalDataList.Count)
				{
					DataManagerEvent.CoopConditionData coopConditionData = mapRewardConditionalDataList[num];
					RewardListWindowCtrl.GUI.ListBar listBar = new RewardListWindowCtrl.GUI.ListBar(go.transform);
					listBar.Icon_Item.Setup(DataManager.DmItem.GetItemStaticBase(coopConditionData.AchievementItem.itemId), coopConditionData.AchievementItem.num, new IconItemCtrl.SetupParam
					{
						useInfo = true
					});
					listBar.Mark_Clear.gameObject.SetActive(mapInfo.TotalPoint >= coopConditionData.AchievementCondition);
					listBar.Txt_Num.text = string.Format("{0}", coopConditionData.AchievementCondition);
				}
			}
		}));
		this.GuiData.ScrollView.Setup(10, 0);
	}

	public void Open(int mapId)
	{
		this.MapId = mapId;
		DataManagerEvent.CoopData.MapInfo mapInfo = DataManager.DmEvent.LastCoopInfo.MapInfoMap[this.MapId];
		this.GuiData.ScrollView.Resize(mapInfo.MapRewardConditionalDataList.Count, 0);
		this.GuiData.owCtrl.Setup("＜" + DataManager.DmQuest.QuestStaticData.mapDataMap[this.MapId].mapName + "＞ 報酬一覧", null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int index) => true, null, false);
		this.GuiData.owCtrl.Open();
	}

	public void OpenAchievement()
	{
		this.OtherTeamSuccessGuiData.owCtrl.Setup(null, null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int index) => true, null, false);
		this.OtherTeamSuccessGuiData.owCtrl.Open();
	}

	public void OpenAchievementAndClear()
	{
		if (this.SuccessGuiData.baseObj.activeSelf)
		{
			return;
		}
		SoundManager.Play("prd_se_selector_blockquest_cracker", false, false);
		this.SuccessGuiData.baseObj.SetActive(true);
		this.SuccessGuiData.Bg.SetActive(true);
		this.SuccessGuiData.AEImage.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
		{
			this.SuccessGuiData.AEImage.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
		});
	}

	public void OpenTimeout()
	{
		this.TimeOutGuiData.owCtrl.Setup(null, null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int index) => true, null, false);
		this.TimeOutGuiData.owCtrl.Open();
	}

	private void OnClickButton(PguiButtonCtrl button)
	{
		if (button == this.SuccessGuiData.BtnClose || button == this.SuccessGuiData.ButtonC)
		{
			this.SuccessGuiData.AEImage.PlayAnimation(PguiAECtrl.AmimeType.END, delegate
			{
				this.SuccessGuiData.baseObj.SetActive(false);
			});
		}
	}

	public class GUI
	{
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.ScrollView = baseTr.Find("Window_GetItem/Base/Window/List/ScrollView").GetComponent<ReuseScroll>();
			this.owCtrl = baseTr.Find("Window_GetItem").GetComponent<PguiOpenWindowCtrl>();
		}

		public GameObject baseObj;

		public ReuseScroll ScrollView;

		public PguiOpenWindowCtrl owCtrl;

		public class ListBar
		{
			public ListBar(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Txt_Num = baseTr.Find("BaseImage/Txt_Num").GetComponent<PguiTextCtrl>();
				this.Icon_Item = baseTr.Find("BaseImage/Icon_Item").GetComponent<IconItemCtrl>();
				this.Mark_Clear = baseTr.Find("BaseImage/Mark_Clear").GetComponent<PguiImageCtrl>();
			}

			public GameObject baseObj;

			public PguiTextCtrl Txt_Num;

			public IconItemCtrl Icon_Item;

			public PguiImageCtrl Mark_Clear;
		}
	}

	public class SuccessGUI
	{
		public SuccessGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Bg = baseTr.Find("MoveBg_Stripe").gameObject;
			this.AEImage = baseTr.Find("AEImage").GetComponent<PguiAECtrl>();
			this.BtnClose = baseTr.Find("Window/BtnClose").GetComponent<PguiButtonCtrl>();
			this.ButtonC = baseTr.Find("Window/ButtonC").GetComponent<PguiButtonCtrl>();
		}

		public GameObject baseObj;

		public GameObject Bg;

		public PguiAECtrl AEImage;

		public PguiButtonCtrl BtnClose;

		public PguiButtonCtrl ButtonC;
	}

	public class OtherTeamSuccessGUI
	{
		public OtherTeamSuccessGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
		}

		public GameObject baseObj;

		public PguiOpenWindowCtrl owCtrl;
	}

	public class TimeOutGUI
	{
		public TimeOutGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
		}

		public GameObject baseObj;

		public PguiOpenWindowCtrl owCtrl;
	}
}
