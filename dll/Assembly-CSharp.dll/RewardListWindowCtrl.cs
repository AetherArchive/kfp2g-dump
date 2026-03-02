using System;
using System.Collections.Generic;
using SGNFW.uGUI;
using UnityEngine;

// Token: 0x020001B6 RID: 438
public class RewardListWindowCtrl : MonoBehaviour
{
	// Token: 0x17000418 RID: 1048
	// (get) Token: 0x06001DCB RID: 7627 RVA: 0x0017297D File Offset: 0x00170B7D
	// (set) Token: 0x06001DCC RID: 7628 RVA: 0x00172985 File Offset: 0x00170B85
	private RewardListWindowCtrl.GUI GuiData { get; set; }

	// Token: 0x17000419 RID: 1049
	// (get) Token: 0x06001DCD RID: 7629 RVA: 0x0017298E File Offset: 0x00170B8E
	// (set) Token: 0x06001DCE RID: 7630 RVA: 0x00172996 File Offset: 0x00170B96
	private RewardListWindowCtrl.SuccessGUI SuccessGuiData { get; set; }

	// Token: 0x1700041A RID: 1050
	// (get) Token: 0x06001DCF RID: 7631 RVA: 0x0017299F File Offset: 0x00170B9F
	// (set) Token: 0x06001DD0 RID: 7632 RVA: 0x001729A7 File Offset: 0x00170BA7
	private RewardListWindowCtrl.OtherTeamSuccessGUI OtherTeamSuccessGuiData { get; set; }

	// Token: 0x1700041B RID: 1051
	// (get) Token: 0x06001DD1 RID: 7633 RVA: 0x001729B0 File Offset: 0x00170BB0
	// (set) Token: 0x06001DD2 RID: 7634 RVA: 0x001729B8 File Offset: 0x00170BB8
	private RewardListWindowCtrl.TimeOutGUI TimeOutGuiData { get; set; }

	// Token: 0x1700041C RID: 1052
	// (get) Token: 0x06001DD3 RID: 7635 RVA: 0x001729C1 File Offset: 0x00170BC1
	// (set) Token: 0x06001DD4 RID: 7636 RVA: 0x001729C9 File Offset: 0x00170BC9
	private int MapId { get; set; }

	// Token: 0x06001DD5 RID: 7637 RVA: 0x001729D4 File Offset: 0x00170BD4
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

	// Token: 0x06001DD6 RID: 7638 RVA: 0x00172B28 File Offset: 0x00170D28
	public void Open(int mapId)
	{
		this.MapId = mapId;
		DataManagerEvent.CoopData.MapInfo mapInfo = DataManager.DmEvent.LastCoopInfo.MapInfoMap[this.MapId];
		this.GuiData.ScrollView.Resize(mapInfo.MapRewardConditionalDataList.Count, 0);
		this.GuiData.owCtrl.Setup("＜" + DataManager.DmQuest.QuestStaticData.mapDataMap[this.MapId].mapName + "＞ 報酬一覧", null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int index) => true, null, false);
		this.GuiData.owCtrl.Open();
	}

	// Token: 0x06001DD7 RID: 7639 RVA: 0x00172BEC File Offset: 0x00170DEC
	public void OpenAchievement()
	{
		this.OtherTeamSuccessGuiData.owCtrl.Setup(null, null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int index) => true, null, false);
		this.OtherTeamSuccessGuiData.owCtrl.Open();
	}

	// Token: 0x06001DD8 RID: 7640 RVA: 0x00172C44 File Offset: 0x00170E44
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

	// Token: 0x06001DD9 RID: 7641 RVA: 0x00172CB0 File Offset: 0x00170EB0
	public void OpenTimeout()
	{
		this.TimeOutGuiData.owCtrl.Setup(null, null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int index) => true, null, false);
		this.TimeOutGuiData.owCtrl.Open();
	}

	// Token: 0x06001DDA RID: 7642 RVA: 0x00172D08 File Offset: 0x00170F08
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

	// Token: 0x02000F63 RID: 3939
	public class GUI
	{
		// Token: 0x06004F71 RID: 20337 RVA: 0x0023B216 File Offset: 0x00239416
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.ScrollView = baseTr.Find("Window_GetItem/Base/Window/List/ScrollView").GetComponent<ReuseScroll>();
			this.owCtrl = baseTr.Find("Window_GetItem").GetComponent<PguiOpenWindowCtrl>();
		}

		// Token: 0x0400571C RID: 22300
		public GameObject baseObj;

		// Token: 0x0400571D RID: 22301
		public ReuseScroll ScrollView;

		// Token: 0x0400571E RID: 22302
		public PguiOpenWindowCtrl owCtrl;

		// Token: 0x02001209 RID: 4617
		public class ListBar
		{
			// Token: 0x0600579D RID: 22429 RVA: 0x00257AA8 File Offset: 0x00255CA8
			public ListBar(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Txt_Num = baseTr.Find("BaseImage/Txt_Num").GetComponent<PguiTextCtrl>();
				this.Icon_Item = baseTr.Find("BaseImage/Icon_Item").GetComponent<IconItemCtrl>();
				this.Mark_Clear = baseTr.Find("BaseImage/Mark_Clear").GetComponent<PguiImageCtrl>();
			}

			// Token: 0x040062AC RID: 25260
			public GameObject baseObj;

			// Token: 0x040062AD RID: 25261
			public PguiTextCtrl Txt_Num;

			// Token: 0x040062AE RID: 25262
			public IconItemCtrl Icon_Item;

			// Token: 0x040062AF RID: 25263
			public PguiImageCtrl Mark_Clear;
		}
	}

	// Token: 0x02000F64 RID: 3940
	public class SuccessGUI
	{
		// Token: 0x06004F72 RID: 20338 RVA: 0x0023B258 File Offset: 0x00239458
		public SuccessGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Bg = baseTr.Find("MoveBg_Stripe").gameObject;
			this.AEImage = baseTr.Find("AEImage").GetComponent<PguiAECtrl>();
			this.BtnClose = baseTr.Find("Window/BtnClose").GetComponent<PguiButtonCtrl>();
			this.ButtonC = baseTr.Find("Window/ButtonC").GetComponent<PguiButtonCtrl>();
		}

		// Token: 0x0400571F RID: 22303
		public GameObject baseObj;

		// Token: 0x04005720 RID: 22304
		public GameObject Bg;

		// Token: 0x04005721 RID: 22305
		public PguiAECtrl AEImage;

		// Token: 0x04005722 RID: 22306
		public PguiButtonCtrl BtnClose;

		// Token: 0x04005723 RID: 22307
		public PguiButtonCtrl ButtonC;
	}

	// Token: 0x02000F65 RID: 3941
	public class OtherTeamSuccessGUI
	{
		// Token: 0x06004F73 RID: 20339 RVA: 0x0023B2CF File Offset: 0x002394CF
		public OtherTeamSuccessGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
		}

		// Token: 0x04005724 RID: 22308
		public GameObject baseObj;

		// Token: 0x04005725 RID: 22309
		public PguiOpenWindowCtrl owCtrl;
	}

	// Token: 0x02000F66 RID: 3942
	public class TimeOutGUI
	{
		// Token: 0x06004F74 RID: 20340 RVA: 0x0023B2EF File Offset: 0x002394EF
		public TimeOutGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
		}

		// Token: 0x04005726 RID: 22310
		public GameObject baseObj;

		// Token: 0x04005727 RID: 22311
		public PguiOpenWindowCtrl owCtrl;
	}
}
