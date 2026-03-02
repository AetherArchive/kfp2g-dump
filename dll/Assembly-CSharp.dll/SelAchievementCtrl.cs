using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.uGUI;
using UnityEngine;

// Token: 0x0200015F RID: 351
public class SelAchievementCtrl : MonoBehaviour
{
	// Token: 0x06001431 RID: 5169 RVA: 0x000F7B08 File Offset: 0x000F5D08
	public void Init()
	{
		AssetManager.GetAssetData("SceneMenu/GUI/Prefab/GUI_Menu_Achievement_Window");
		this.gui = new SelAchievementCtrl.GUI(AssetManager.InstantiateAssetData("SceneMenu/GUI/Prefab/GUI_Menu_Achievement", base.transform).transform);
		if (this.gui == null)
		{
			global::UnityEngine.Debug.LogError("guiData not found error!!");
		}
		this.currentShowAchievementList = DataManager.DmAchievement.GetShowDataList();
		this.gui.achievementAll.noneText.gameObject.SetActive(this.currentShowAchievementList.Count <= 0);
		ReuseScroll scrollView = this.gui.achievementAll.scrollView;
		scrollView.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollView.onStartItem, new Action<int, GameObject>(this.OnStartAchievementView));
		ReuseScroll scrollView2 = this.gui.achievementAll.scrollView;
		scrollView2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollView2.onUpdateItem, new Action<int, GameObject>(this.OnUpdateAchievementView));
		int num = (this.currentShowAchievementList.Count + 1) / this.COLUMN_MAX;
		num += (((this.currentShowAchievementList.Count + 1) % this.COLUMN_MAX == 0) ? 0 : 1);
		this.gui.achievementAll.scrollView.Setup(num, 0);
		DataManagerAchievement.AchievementStaticData achievementData = DataManager.DmAchievement.GetAchievementData(this.currentAchievementId);
		if (this.currentShowAchievementList.Count > 0)
		{
			this.gui.achievementChange.beforeObj.Txt_Achievement.text = "称号は未設定です";
		}
		else if (achievementData != null)
		{
			this.gui.achievementChange.beforeObj.Txt_Achievement.text = achievementData.name;
		}
		this.gui.achievementChange.Btn_Change.AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			this.OnClickButtonChange();
		}, PguiButtonCtrl.SoundType.DECIDE);
		DataManagerAchievement.AchievementData selectData = DataManager.DmAchievement.GetSelectData();
		int num2 = ((selectData == null) ? 0 : selectData.staticData.id);
		this.gui.achievementChange.beforeObj.Achievement.Setup(num2, false, false);
		if (selectData != null)
		{
			this.gui.achievementChange.beforeObj.Txt_Achievement.text = selectData.staticData.name;
		}
		this.gui.achievementChange.afterObj.Achievement.Setup(this.currentAchievementId, false, false);
		this.gui.achievementChange.afterObj.Txt_Achievement.gameObject.SetActive(false);
		this.gui.achievementChange.afterObj.Btn_Info.AddOnClickListener(delegate(PguiButtonCtrl button)
		{
			this.OnClickButtonInfo();
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.gui.achievementChange.afterObj.Btn_Info.SetActEnable(false, false, false);
	}

	// Token: 0x06001432 RID: 5170 RVA: 0x000F7DA8 File Offset: 0x000F5FA8
	public void Setup()
	{
		this.BackAnimPlaying = false;
		this.OnClickMoveSequenceName = SceneManager.SceneName.None;
		this.OnClickMoveSequenceArgs = null;
		this.UpdateCurrentFrame(-1);
		this.UpdateEnableChangeButton(false);
		this.UpdateShowAchievementList();
		this.gui.achievementAll.filterTab.Setup((int)DataManager.DmAchievement.currentFilter, new PguiTabGroupCtrl.OnSelectTab(this.OnClickFilterButton));
		DataManagerAchievement.AchievementData haveAchievementData = DataManager.DmAchievement.GetHaveAchievementData(this.currentAchievementId);
		this.gui.achievementChange.afterObj.Btn_Info.SetActEnable(haveAchievementData != null, false, false);
		this.gui.achievementChange.Btn_Change.SetActEnable(haveAchievementData != null, false, false);
		this.gui.achievementAll.noneText.gameObject.SetActive(this.currentShowAchievementList.Count <= 0);
		int count = DataManager.DmAchievement.GetHaveAchievementDataList().Count;
		int count2 = DataManager.DmAchievement.GetShowDataList().Count;
		this.gui.achievementAll.acquistitionText.text = string.Format("獲得状況 {0}/{1}", count, count2);
		this.gui.achievementAll.scrollView.Refresh();
	}

	// Token: 0x06001433 RID: 5171 RVA: 0x000F7EE0 File Offset: 0x000F60E0
	private void Update()
	{
		if (this.currentEnumerator != null && !this.currentEnumerator.MoveNext())
		{
			this.currentEnumerator = null;
		}
	}

	// Token: 0x06001434 RID: 5172 RVA: 0x000F7EFE File Offset: 0x000F60FE
	private IEnumerator RequestUpdateOption()
	{
		if (this.OnClickMoveSequenceName == SceneManager.SceneName.None)
		{
			CanvasManager.HdlCmnMenu.MoveSceneByMenu(SceneManager.SceneName.SceneOtherMenuTop, null);
		}
		else
		{
			CanvasManager.HdlCmnMenu.MoveSceneByMenu(this.OnClickMoveSequenceName, this.OnClickMoveSequenceArgs);
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001435 RID: 5173 RVA: 0x000F7F0D File Offset: 0x000F610D
	private IEnumerator ChangeNewFlag(PguiButtonCtrl btnCtrl)
	{
		int achievementId = btnCtrl.GetComponent<AchievementCtrl>().GetAchievementId();
		DataManagerAchievement.AchievementData haveAchievementData = DataManager.DmAchievement.GetHaveAchievementData(achievementId);
		if (haveAchievementData != null && haveAchievementData.dynamicData.isNewFlag)
		{
			DataManager.DmAchievement.RequestActionNewFlag(achievementId);
			do
			{
				yield return null;
			}
			while (DataManager.IsServerRequesting());
			btnCtrl.GetComponent<AchievementCtrl>().HideBadge(achievementId);
		}
		this.UpdateCurrentFrame(achievementId);
		DataManagerAchievement.AchievementStaticData achievementData = DataManager.DmAchievement.GetAchievementData(this.currentAchievementId);
		this.gui.achievementChange.afterObj.Btn_Info.SetActEnable(achievementData != null, false, false);
		this.UpdateEnableChangeButton(false);
		this.gui.achievementAll.scrollView.Refresh();
		yield break;
	}

	// Token: 0x06001436 RID: 5174 RVA: 0x000F7F23 File Offset: 0x000F6123
	private IEnumerator ChangeSelectAchievement()
	{
		DataManager.DmAchievement.RequestActionSelectFlag(this.currentAchievementId);
		do
		{
			yield return null;
		}
		while (DataManager.IsServerRequesting());
		DataManagerAchievement.AchievementData selectData = DataManager.DmAchievement.GetSelectData();
		this.gui.achievementChange.beforeObj.Achievement.Setup((selectData == null) ? 0 : selectData.staticData.id, false, false);
		this.gui.achievementChange.beforeObj.Txt_Achievement.text = ((selectData == null) ? "称号は未設定です" : selectData.staticData.name);
		this.UpdateEnableChangeButton(true);
		yield break;
	}

	// Token: 0x06001437 RID: 5175 RVA: 0x000F7F34 File Offset: 0x000F6134
	private void UpdateEnableChangeButton(bool isForceInvalid = false)
	{
		if (isForceInvalid)
		{
			this.gui.achievementChange.Btn_Change.SetActEnable(false, false, false);
			return;
		}
		if (this.currentAchievementId == 0 && DataManager.DmAchievement.GetSelectData() != null)
		{
			this.gui.achievementChange.Btn_Change.SetActEnable(true, false, false);
			return;
		}
		DataManagerAchievement.AchievementData haveAchievementData = DataManager.DmAchievement.GetHaveAchievementData(this.currentAchievementId);
		DataManagerAchievement.AchievementData selectData = DataManager.DmAchievement.GetSelectData();
		int num = ((haveAchievementData == null) ? 0 : haveAchievementData.staticData.id);
		int num2 = ((selectData == null) ? 0 : selectData.staticData.id);
		this.gui.achievementChange.Btn_Change.SetActEnable(num != 0 && num != num2, false, false);
	}

	// Token: 0x06001438 RID: 5176 RVA: 0x000F7FF0 File Offset: 0x000F61F0
	private void UpdateCurrentFrame(int id)
	{
		this.currentAchievementId = id;
		this.gui.achievementChange.afterObj.Achievement.Setup(id, false, false);
		DataManagerAchievement.AchievementData haveAchievementData = DataManager.DmAchievement.GetHaveAchievementData(id);
		if (haveAchievementData != null)
		{
			this.gui.achievementChange.afterObj.Txt_Achievement.gameObject.SetActive(true);
			this.gui.achievementChange.afterObj.Txt_Achievement.text = haveAchievementData.staticData.name;
			return;
		}
		this.gui.achievementChange.afterObj.Txt_Achievement.gameObject.SetActive(true);
		if (id == 0 && DataManager.DmAchievement.GetSelectData() != null)
		{
			this.gui.achievementChange.afterObj.Txt_Achievement.text = "称号を外します";
			return;
		}
		if (id == -1 || id == 0)
		{
			this.gui.achievementChange.afterObj.Txt_Achievement.text = "";
			return;
		}
		this.gui.achievementChange.afterObj.Txt_Achievement.text = "称号を所持していません";
	}

	// Token: 0x06001439 RID: 5177 RVA: 0x000F8110 File Offset: 0x000F6310
	private void OnStartAchievementView(int rowIdx, GameObject go)
	{
		for (int i = 0; i < this.COLUMN_MAX; i++)
		{
			GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("CmnIconFrame/GUI/Prefab/Achievement"), go.transform);
			gameObject.name = i.ToString();
			List<DataManagerAchievement.AchievementStaticData> list = this.currentShowAchievementList;
			int num = rowIdx * this.COLUMN_MAX + i;
			gameObject.gameObject.SetActive(true);
			AchievementCtrl component = gameObject.GetComponent<AchievementCtrl>();
			PguiButtonCtrl button = component.GetComponent<PguiButtonCtrl>();
			if (num == 0)
			{
				component.Setup(0, false, true);
				component.AddOnClickListener(delegate(AchievementCtrl ctrl)
				{
					this.OnClickButton(button, true);
				});
			}
			else if (num >= list.Count + 1)
			{
				gameObject.gameObject.SetActive(false);
			}
			else
			{
				int num2 = num - 1;
				int id = list[num2].id;
				component.Setup(id, false, false);
				component.SetScale(1f);
				component.AddOnClickListener(delegate(AchievementCtrl ctrl)
				{
					this.OnClickButton(button, false);
				});
			}
		}
	}

	// Token: 0x0600143A RID: 5178 RVA: 0x000F821C File Offset: 0x000F641C
	private void OnUpdateAchievementView(int rowIdx, GameObject rowGo)
	{
		for (int i = 0; i < 3; i++)
		{
			Transform transform = rowGo.transform.Find(i.ToString());
			if (transform)
			{
				AchievementCtrl ac = transform.GetComponent<AchievementCtrl>();
				int num = rowIdx * this.COLUMN_MAX + i;
				if (i == 0 && rowIdx == 0)
				{
					ac.Setup(0, false, true);
					ac.ChangeShowSelect(this.currentAchievementId == 0);
					ac.AddOnClickListener(delegate(AchievementCtrl ctrl)
					{
						this.OnClickButton(ac.GetComponent<PguiButtonCtrl>(), true);
					});
				}
				else
				{
					List<DataManagerAchievement.AchievementStaticData> list = this.currentShowAchievementList;
					if (num <= list.Count)
					{
						int num2 = num - 1;
						int id = list[num2].id;
						transform.gameObject.SetActive(true);
						ac.Setup(id, false, false);
						ac.ChangeShowSelect(id == this.currentAchievementId);
						ac.AddOnClickListener(delegate(AchievementCtrl ctrl)
						{
							this.OnClickButton(ac.GetComponent<PguiButtonCtrl>(), false);
						});
					}
					else
					{
						transform.gameObject.SetActive(false);
					}
				}
			}
		}
	}

	// Token: 0x0600143B RID: 5179 RVA: 0x000F833C File Offset: 0x000F653C
	private void OnClickButton(PguiButtonCtrl btnCtrl, bool isRemove = false)
	{
		SoundManager.Play((isRemove && this.currentAchievementId != 0 && this.currentAchievementId != -1) ? "prd_se_cancel" : "prd_se_click", false, false);
		if (btnCtrl.GetComponent<AchievementCtrl>() == null)
		{
			this.UpdateCurrentFrame(0);
			this.UpdateEnableChangeButton(false);
			return;
		}
		btnCtrl.GetComponent<AchievementCtrl>().GetAchievementId();
		base.StartCoroutine(this.ChangeNewFlag(btnCtrl));
	}

	// Token: 0x0600143C RID: 5180 RVA: 0x000F83A8 File Offset: 0x000F65A8
	private bool OnClickButtonWindow(int index)
	{
		return true;
	}

	// Token: 0x0600143D RID: 5181 RVA: 0x000F83AB File Offset: 0x000F65AB
	private void OnClickButtonChange()
	{
		base.StartCoroutine(this.ChangeSelectAchievement());
	}

	// Token: 0x0600143E RID: 5182 RVA: 0x000F83BC File Offset: 0x000F65BC
	private void OnClickButtonInfo()
	{
		DataManagerAchievement.AchievementStaticData achievementData = DataManager.DmAchievement.GetAchievementData(this.currentAchievementId);
		CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("入手方法"), PrjUtil.MakeMessage(achievementData.infoGettext), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, new PguiOpenWindowCtrl.Callback(this.OnClickButtonWindow), null, false);
		CanvasManager.HdlOpenWindowBasic.Open();
	}

	// Token: 0x0600143F RID: 5183 RVA: 0x000F8418 File Offset: 0x000F6618
	private void UpdateScrollViewItemNum()
	{
		int num = 0;
		if (this.currentShowAchievementList.Count > 0)
		{
			num = (this.currentShowAchievementList.Count + 1) / this.COLUMN_MAX;
			num += (((this.currentShowAchievementList.Count + 1) % this.COLUMN_MAX == 0) ? 0 : 1);
		}
		this.gui.achievementAll.scrollView.Resize(num, 0);
	}

	// Token: 0x06001440 RID: 5184 RVA: 0x000F847E File Offset: 0x000F667E
	public bool OnClickReturnButton()
	{
		if (this.BackAnimPlaying)
		{
			return true;
		}
		this.BackAnimPlaying = true;
		return true;
	}

	// Token: 0x06001441 RID: 5185 RVA: 0x000F8492 File Offset: 0x000F6692
	public bool OnClickMoveSequenceButton(SceneManager.SceneName sceneName, object sceneArgs)
	{
		this.currentEnumerator = this.RequestUpdateOption();
		this.OnClickMoveSequenceName = sceneName;
		this.OnClickMoveSequenceArgs = sceneArgs;
		return true;
	}

	// Token: 0x06001442 RID: 5186 RVA: 0x000F84B0 File Offset: 0x000F66B0
	public bool OnClickFilterButton(int filterIdx)
	{
		DataManager.DmAchievement.currentFilter = (DataManagerAchievement.FILTER)filterIdx;
		this.UpdateShowAchievementList();
		this.gui.achievementAll.noneText.gameObject.SetActive(this.currentShowAchievementList.Count <= 0);
		return true;
	}

	// Token: 0x06001443 RID: 5187 RVA: 0x000F84FC File Offset: 0x000F66FC
	private void UpdateShowAchievementList()
	{
		switch (DataManager.DmAchievement.currentFilter)
		{
		case DataManagerAchievement.FILTER.ALL:
			this.currentShowAchievementList = DataManager.DmAchievement.GetShowDataList();
			break;
		case DataManagerAchievement.FILTER.HAVE:
			this.currentShowAchievementList = DataManager.DmAchievement.GetHaveAchievementDataList();
			break;
		case DataManagerAchievement.FILTER.NOT_HAVE:
			this.currentShowAchievementList = DataManager.DmAchievement.GetNotHaveAchievementDataList();
			break;
		}
		this.UpdateScrollViewItemNum();
	}

	// Token: 0x040010B3 RID: 4275
	private IEnumerator currentEnumerator;

	// Token: 0x040010B4 RID: 4276
	private SceneManager.SceneName OnClickMoveSequenceName;

	// Token: 0x040010B5 RID: 4277
	private SelAchievementCtrl.GUI gui;

	// Token: 0x040010B6 RID: 4278
	private List<DataManagerAchievement.AchievementStaticData> currentShowAchievementList;

	// Token: 0x040010B7 RID: 4279
	private object OnClickMoveSequenceArgs;

	// Token: 0x040010B8 RID: 4280
	private int currentAchievementId = -1;

	// Token: 0x040010B9 RID: 4281
	public bool BackAnimPlaying;

	// Token: 0x040010BA RID: 4282
	private int COLUMN_MAX = 2;

	// Token: 0x02000B69 RID: 2921
	public class AchievementChange
	{
		// Token: 0x060042B9 RID: 17081 RVA: 0x00200F6C File Offset: 0x001FF16C
		public AchievementChange(Transform baseTr)
		{
			this.beforeObj = new SelAchievementCtrl.AchievementChange.Before(baseTr.Find("All/Left/All/Before"));
			this.afterObj = new SelAchievementCtrl.AchievementChange.After(baseTr.Find("All/Left/All/After"));
			this.Btn_Change = baseTr.Find("All/Left/All/Btn_Change").GetComponent<PguiButtonCtrl>();
		}

		// Token: 0x04004767 RID: 18279
		public SelAchievementCtrl.AchievementChange.Before beforeObj;

		// Token: 0x04004768 RID: 18280
		public SelAchievementCtrl.AchievementChange.After afterObj;

		// Token: 0x04004769 RID: 18281
		public PguiButtonCtrl Btn_Change;

		// Token: 0x02001191 RID: 4497
		public class Before
		{
			// Token: 0x0600569A RID: 22170 RVA: 0x00252D0C File Offset: 0x00250F0C
			public Before(Transform baseTr)
			{
				this.Achievement = baseTr.Find("Achievement").GetComponent<AchievementCtrl>();
				this.Txt_Achievement = baseTr.Find("Txt_Achievement").GetComponent<PguiTextCtrl>();
			}

			// Token: 0x04006038 RID: 24632
			public AchievementCtrl Achievement;

			// Token: 0x04006039 RID: 24633
			public PguiTextCtrl Txt_Achievement;
		}

		// Token: 0x02001192 RID: 4498
		public class After
		{
			// Token: 0x0600569B RID: 22171 RVA: 0x00252D40 File Offset: 0x00250F40
			public After(Transform baseTr)
			{
				this.Achievement = baseTr.Find("Achievement").GetComponent<AchievementCtrl>();
				this.Txt_Achievement = baseTr.Find("Txt_Achievement").GetComponent<PguiTextCtrl>();
				this.Btn_Info = baseTr.Find("Btn_Info").GetComponent<PguiButtonCtrl>();
			}

			// Token: 0x0400603A RID: 24634
			public AchievementCtrl Achievement;

			// Token: 0x0400603B RID: 24635
			public PguiTextCtrl Txt_Achievement;

			// Token: 0x0400603C RID: 24636
			public PguiButtonCtrl Btn_Info;
		}
	}

	// Token: 0x02000B6A RID: 2922
	public class AchievementAll
	{
		// Token: 0x060042BA RID: 17082 RVA: 0x00200FC4 File Offset: 0x001FF1C4
		public AchievementAll(Transform baseObj)
		{
			this.scrollView = baseObj.Find("All/Right/AchievementAll/ScrollView").GetComponent<ReuseScroll>();
			this.noneText = baseObj.Find("All/Right/AchievementAll/Txt_None").GetComponent<PguiTextCtrl>();
			this.acquistitionText = baseObj.Find("All/Right/AchievementAll/Txt_Acquisition").GetComponent<PguiTextCtrl>();
			this.filterTab = baseObj.Find("All/Right/AchievementAll/Filter/TabGroup").GetComponent<PguiTabGroupCtrl>();
		}

		// Token: 0x0400476A RID: 18282
		public ReuseScroll scrollView;

		// Token: 0x0400476B RID: 18283
		public PguiTextCtrl noneText;

		// Token: 0x0400476C RID: 18284
		public PguiTextCtrl acquistitionText;

		// Token: 0x0400476D RID: 18285
		public PguiTabGroupCtrl filterTab;

		// Token: 0x0400476E RID: 18286
		public Action<int, GameObject> onStartItem;

		// Token: 0x0400476F RID: 18287
		public Action<int, GameObject> onUpdateItem;
	}

	// Token: 0x02000B6B RID: 2923
	public class GUI
	{
		// Token: 0x060042BB RID: 17083 RVA: 0x0020102F File Offset: 0x001FF22F
		public GUI(Transform baseObj)
		{
			this.baseObj = baseObj.gameObject;
			this.achievementChange = new SelAchievementCtrl.AchievementChange(baseObj);
			this.achievementAll = new SelAchievementCtrl.AchievementAll(baseObj);
		}

		// Token: 0x04004770 RID: 18288
		public GameObject baseObj;

		// Token: 0x04004771 RID: 18289
		public SelAchievementCtrl.AchievementAll achievementAll;

		// Token: 0x04004772 RID: 18290
		public SelAchievementCtrl.AchievementChange achievementChange;
	}
}
