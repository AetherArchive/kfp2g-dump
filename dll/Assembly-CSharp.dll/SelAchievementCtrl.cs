using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.uGUI;
using UnityEngine;

public class SelAchievementCtrl : MonoBehaviour
{
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

	private void Update()
	{
		if (this.currentEnumerator != null && !this.currentEnumerator.MoveNext())
		{
			this.currentEnumerator = null;
		}
	}

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

	private bool OnClickButtonWindow(int index)
	{
		return true;
	}

	private void OnClickButtonChange()
	{
		base.StartCoroutine(this.ChangeSelectAchievement());
	}

	private void OnClickButtonInfo()
	{
		DataManagerAchievement.AchievementStaticData achievementData = DataManager.DmAchievement.GetAchievementData(this.currentAchievementId);
		CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("入手方法"), PrjUtil.MakeMessage(achievementData.infoGettext), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, new PguiOpenWindowCtrl.Callback(this.OnClickButtonWindow), null, false);
		CanvasManager.HdlOpenWindowBasic.Open();
	}

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

	public bool OnClickReturnButton()
	{
		if (this.BackAnimPlaying)
		{
			return true;
		}
		this.BackAnimPlaying = true;
		return true;
	}

	public bool OnClickMoveSequenceButton(SceneManager.SceneName sceneName, object sceneArgs)
	{
		this.currentEnumerator = this.RequestUpdateOption();
		this.OnClickMoveSequenceName = sceneName;
		this.OnClickMoveSequenceArgs = sceneArgs;
		return true;
	}

	public bool OnClickFilterButton(int filterIdx)
	{
		DataManager.DmAchievement.currentFilter = (DataManagerAchievement.FILTER)filterIdx;
		this.UpdateShowAchievementList();
		this.gui.achievementAll.noneText.gameObject.SetActive(this.currentShowAchievementList.Count <= 0);
		return true;
	}

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

	private IEnumerator currentEnumerator;

	private SceneManager.SceneName OnClickMoveSequenceName;

	private SelAchievementCtrl.GUI gui;

	private List<DataManagerAchievement.AchievementStaticData> currentShowAchievementList;

	private object OnClickMoveSequenceArgs;

	private int currentAchievementId = -1;

	public bool BackAnimPlaying;

	private int COLUMN_MAX = 2;

	public class AchievementChange
	{
		public AchievementChange(Transform baseTr)
		{
			this.beforeObj = new SelAchievementCtrl.AchievementChange.Before(baseTr.Find("All/Left/All/Before"));
			this.afterObj = new SelAchievementCtrl.AchievementChange.After(baseTr.Find("All/Left/All/After"));
			this.Btn_Change = baseTr.Find("All/Left/All/Btn_Change").GetComponent<PguiButtonCtrl>();
		}

		public SelAchievementCtrl.AchievementChange.Before beforeObj;

		public SelAchievementCtrl.AchievementChange.After afterObj;

		public PguiButtonCtrl Btn_Change;

		public class Before
		{
			public Before(Transform baseTr)
			{
				this.Achievement = baseTr.Find("Achievement").GetComponent<AchievementCtrl>();
				this.Txt_Achievement = baseTr.Find("Txt_Achievement").GetComponent<PguiTextCtrl>();
			}

			public AchievementCtrl Achievement;

			public PguiTextCtrl Txt_Achievement;
		}

		public class After
		{
			public After(Transform baseTr)
			{
				this.Achievement = baseTr.Find("Achievement").GetComponent<AchievementCtrl>();
				this.Txt_Achievement = baseTr.Find("Txt_Achievement").GetComponent<PguiTextCtrl>();
				this.Btn_Info = baseTr.Find("Btn_Info").GetComponent<PguiButtonCtrl>();
			}

			public AchievementCtrl Achievement;

			public PguiTextCtrl Txt_Achievement;

			public PguiButtonCtrl Btn_Info;
		}
	}

	public class AchievementAll
	{
		public AchievementAll(Transform baseObj)
		{
			this.scrollView = baseObj.Find("All/Right/AchievementAll/ScrollView").GetComponent<ReuseScroll>();
			this.noneText = baseObj.Find("All/Right/AchievementAll/Txt_None").GetComponent<PguiTextCtrl>();
			this.acquistitionText = baseObj.Find("All/Right/AchievementAll/Txt_Acquisition").GetComponent<PguiTextCtrl>();
			this.filterTab = baseObj.Find("All/Right/AchievementAll/Filter/TabGroup").GetComponent<PguiTabGroupCtrl>();
		}

		public ReuseScroll scrollView;

		public PguiTextCtrl noneText;

		public PguiTextCtrl acquistitionText;

		public PguiTabGroupCtrl filterTab;

		public Action<int, GameObject> onStartItem;

		public Action<int, GameObject> onUpdateItem;
	}

	public class GUI
	{
		public GUI(Transform baseObj)
		{
			this.baseObj = baseObj.gameObject;
			this.achievementChange = new SelAchievementCtrl.AchievementChange(baseObj);
			this.achievementAll = new SelAchievementCtrl.AchievementAll(baseObj);
		}

		public GameObject baseObj;

		public SelAchievementCtrl.AchievementAll achievementAll;

		public SelAchievementCtrl.AchievementChange achievementChange;
	}
}
