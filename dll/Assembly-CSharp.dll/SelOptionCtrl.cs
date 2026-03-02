using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using UnityEngine;

// Token: 0x02000161 RID: 353
public class SelOptionCtrl : MonoBehaviour
{
	// Token: 0x06001462 RID: 5218 RVA: 0x000F8F94 File Offset: 0x000F7194
	public void Init()
	{
		this.guiData = new SelOptionCtrl.GUI(AssetManager.InstantiateAssetData("SceneOption/GUI/Prefab/GUI_Option", base.transform).transform);
		foreach (PguiButtonCtrl pguiButtonCtrl in this.guiData.ALL_Button)
		{
			pguiButtonCtrl.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.INVALID);
		}
		foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl in this.guiData.ALL_Toggle)
		{
			pguiToggleButtonCtrl.AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickToggle));
		}
		this.guiData.TabGroup.Setup(0, new PguiTabGroupCtrl.OnSelectTab(this.OnSelectTab));
		this.guiData.Display_Direction[0].transform.parent.gameObject.SetActive(false);
		this.guiData.TabGroup.m_PguiTabList[1].gameObject.SetActive(false);
		for (int i = 2; i < this.guiData.TabGroup.m_PguiTabList.Count; i++)
		{
			Vector3 localPosition = this.guiData.TabGroup.m_PguiTabList[i].transform.localPosition;
			localPosition.x -= 230f;
			this.guiData.TabGroup.m_PguiTabList[i].transform.localPosition = localPosition;
		}
		this.guiData.GraphicsQuality[1].gameObject.SetActive(false);
		GameObject gameObject = this.guiData.FrameRate[0].transform.parent.gameObject;
		gameObject.SetActive(true);
		gameObject.transform.position = this.guiData.Display_Direction[0].transform.parent.position;
	}

	// Token: 0x06001463 RID: 5219 RVA: 0x000F91AC File Offset: 0x000F73AC
	public void Setup()
	{
		this.OnClickMoveSequenceName = SceneManager.SceneName.None;
		this.OnClickMoveSequenceArgs = null;
		this.guiData.TabGroup.Setup(0, new PguiTabGroupCtrl.OnSelectTab(this.OnSelectTab));
		this.OnSelectTab(0);
		this.cloneUserOptionData = DataManager.DmUserInfo.optionData.Clone();
		this.RefreshUI();
		this.guiData.animation.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
	}

	// Token: 0x06001464 RID: 5220 RVA: 0x000F9219 File Offset: 0x000F7419
	public UserOptionData GetCustomData()
	{
		return this.cloneUserOptionData;
	}

	// Token: 0x06001465 RID: 5221 RVA: 0x000F9221 File Offset: 0x000F7421
	private void Start()
	{
	}

	// Token: 0x06001466 RID: 5222 RVA: 0x000F9223 File Offset: 0x000F7423
	private void Update()
	{
		if (this.currentEnumerator != null && !this.currentEnumerator.MoveNext())
		{
			this.currentEnumerator = null;
		}
	}

	// Token: 0x06001467 RID: 5223 RVA: 0x000F9244 File Offset: 0x000F7444
	private void RefreshUI()
	{
		this.guiData.Display_Direction[0].SetToggleIndex((this.cloneUserOptionData.DisplayDirection == 0) ? 1 : 0);
		this.guiData.Display_Direction[1].SetToggleIndex((1 == this.cloneUserOptionData.DisplayDirection) ? 1 : 0);
		this.guiData.Display_Direction[2].SetToggleIndex((2 == this.cloneUserOptionData.DisplayDirection) ? 1 : 0);
		this.guiData.GraphicsQuality[0].SetToggleIndex((2 == this.cloneUserOptionData.Quality) ? 1 : 0);
		this.guiData.GraphicsQuality[1].SetToggleIndex((this.cloneUserOptionData.Quality == 0) ? 1 : 0);
		this.guiData.GraphicsQuality[2].SetToggleIndex((1 == this.cloneUserOptionData.Quality) ? 1 : 0);
		this.guiData.FrameRate[0].SetToggleIndex((this.cloneUserOptionData.FrameRate == 0) ? 1 : 0);
		this.guiData.FrameRate[1].SetToggleIndex((1 == this.cloneUserOptionData.FrameRate) ? 1 : 0);
		this.guiData.Stamina_Notify[0].SetToggleIndex(this.cloneUserOptionData.PushNotifyStaminaMax ? 1 : 0);
		this.guiData.Stamina_Notify[1].SetToggleIndex((!this.cloneUserOptionData.PushNotifyStaminaMax) ? 1 : 0);
		this.guiData.PvpStamina_Notify[0].SetToggleIndex(this.cloneUserOptionData.PushNotifyPvpStaminaMax ? 1 : 0);
		this.guiData.PvpStamina_Notify[1].SetToggleIndex((!this.cloneUserOptionData.PushNotifyPvpStaminaMax) ? 1 : 0);
		this.guiData.Genki_Notify[0].SetToggleIndex(this.cloneUserOptionData.PushNotifyGenkiZero ? 1 : 0);
		this.guiData.Genki_Notify[1].SetToggleIndex((!this.cloneUserOptionData.PushNotifyGenkiZero) ? 1 : 0);
		this.guiData.Info_Notify[0].SetToggleIndex(this.cloneUserOptionData.PushNotifyInfomation ? 1 : 0);
		this.guiData.Info_Notify[1].SetToggleIndex((!this.cloneUserOptionData.PushNotifyInfomation) ? 1 : 0);
		for (int i = 0; i < this.guiData.Sound_Icon_01.Count; i++)
		{
			this.guiData.Sound_Icon_01[i].SetActive(this.cloneUserOptionData.VolumeBGM > i);
		}
		for (int j = 0; j < this.guiData.Sound_Icon_02.Count; j++)
		{
			this.guiData.Sound_Icon_02[j].SetActive(this.cloneUserOptionData.VolumeSE > j);
		}
		for (int k = 0; k < this.guiData.Sound_Icon_03.Count; k++)
		{
			this.guiData.Sound_Icon_03[k].SetActive(this.cloneUserOptionData.VolumeVOICE > k);
		}
		for (int l = 0; l < this.guiData.Scenario_Speed.Count; l++)
		{
			this.guiData.Scenario_Speed[l].SetToggleIndex((this.cloneUserOptionData.ScenarioSpeed == l) ? 1 : 0);
		}
		for (int m = 0; m < this.guiData.AutoSpeed.Count; m++)
		{
			this.guiData.AutoSpeed[m].SetToggleIndex((this.cloneUserOptionData.autoSpeed == m) ? 1 : 0);
		}
		this.guiData.PhotoAffect[0].SetToggleIndex(this.cloneUserOptionData.ViewPhotoAffect ? 1 : 0);
		this.guiData.PhotoAffect[1].SetToggleIndex((!this.cloneUserOptionData.ViewPhotoAffect) ? 1 : 0);
		this.guiData.ClothesAffect[0].SetToggleIndex(this.cloneUserOptionData.ViewClothesAffect ? 1 : 0);
		this.guiData.ClothesAffect[1].SetToggleIndex((!this.cloneUserOptionData.ViewClothesAffect) ? 1 : 0);
		this.guiData.MissionProgressNotify[0].SetToggleIndex((this.cloneUserOptionData.MissionProgressNotify == 0) ? 1 : 0);
		this.guiData.MissionProgressNotify[1].SetToggleIndex((1 == this.cloneUserOptionData.MissionProgressNotify) ? 1 : 0);
		this.guiData.MissionProgressNotify[2].SetToggleIndex((2 == this.cloneUserOptionData.MissionProgressNotify) ? 1 : 0);
	}

	// Token: 0x06001468 RID: 5224 RVA: 0x000F9722 File Offset: 0x000F7922
	private IEnumerator RequestUpdateOption()
	{
		DataManager.DmUserInfo.RequestActionUpdateUserOption(this.cloneUserOptionData);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		if (this.OnClickMoveSequenceName == SceneManager.SceneName.None)
		{
			this.guiData.animation.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
			{
				CanvasManager.HdlCmnMenu.MoveSceneByMenu(SceneManager.SceneName.SceneOtherMenuTop, null);
			});
		}
		else
		{
			CanvasManager.HdlCmnMenu.MoveSceneByMenu(this.OnClickMoveSequenceName, this.OnClickMoveSequenceArgs);
		}
		yield break;
	}

	// Token: 0x06001469 RID: 5225 RVA: 0x000F9734 File Offset: 0x000F7934
	private void OnClickButton(PguiButtonCtrl button)
	{
		bool flag = false;
		for (int i = 0; i < this.guiData.Sound_Btn_Yaji.Count; i++)
		{
			if (button == this.guiData.Sound_Btn_Yaji[i])
			{
				switch (i / 2)
				{
				case 0:
					this.cloneUserOptionData.VolumeBGM += ((i % 2 == 0) ? (-1) : 1);
					break;
				case 1:
					this.cloneUserOptionData.VolumeSE += ((i % 2 == 0) ? (-1) : 1);
					break;
				case 2:
					this.cloneUserOptionData.VolumeVOICE += ((i % 2 == 0) ? (-1) : 1);
					flag = true;
					break;
				}
				SoundManager.SetCategoryVolume(this.cloneUserOptionData.VolumeList);
				break;
			}
		}
		if (!flag)
		{
			SoundManager.Play("prd_se_click", false, false);
		}
		else
		{
			SoundManager.PlayVoice(DataManager.DmChara.GetCharaStaticData(DataManager.DmUserInfo.favoriteCharaId).cueSheetName, VOICE_TYPE.TCL01);
		}
		this.RefreshUI();
	}

	// Token: 0x0600146A RID: 5226 RVA: 0x000F983C File Offset: 0x000F7A3C
	private bool OnClickToggle(PguiToggleButtonCtrl toggle, int index)
	{
		SoundManager.Play("prd_se_click", false, false);
		int[] option = SceneManager.GetOption();
		if (this.guiData.Display_Direction.Contains(toggle))
		{
			this.cloneUserOptionData.DisplayDirection = this.guiData.Display_Direction.IndexOf(toggle);
			Singleton<CanvasManager>.Instance.SetDisplayDirection(this.cloneUserOptionData.DisplayDirection);
			option[0] = this.cloneUserOptionData.DisplayDirection;
		}
		else if (this.guiData.GraphicsQuality.Contains(toggle))
		{
			switch (this.guiData.GraphicsQuality.IndexOf(toggle))
			{
			case 0:
				this.cloneUserOptionData.Quality = 2;
				break;
			case 1:
				this.cloneUserOptionData.Quality = 0;
				break;
			case 2:
				this.cloneUserOptionData.Quality = 1;
				break;
			}
			this.cloneUserOptionData.SetDisplayQuality();
			option[1] = this.cloneUserOptionData.Quality;
		}
		else if (this.guiData.FrameRate.Contains(toggle))
		{
			int num = this.guiData.FrameRate.IndexOf(toggle);
			if (num != 0)
			{
				if (num == 1)
				{
					this.cloneUserOptionData.FrameRate = 1;
				}
			}
			else
			{
				this.cloneUserOptionData.FrameRate = 0;
			}
			this.cloneUserOptionData.SetFrameRate();
		}
		else if (this.guiData.Stamina_Notify.Contains(toggle))
		{
			this.cloneUserOptionData.PushNotifyStaminaMax = this.guiData.Stamina_Notify[0] == toggle;
		}
		else if (this.guiData.PvpStamina_Notify.Contains(toggle))
		{
			this.cloneUserOptionData.PushNotifyPvpStaminaMax = this.guiData.PvpStamina_Notify[0] == toggle;
		}
		else if (this.guiData.Genki_Notify.Contains(toggle))
		{
			this.cloneUserOptionData.PushNotifyGenkiZero = this.guiData.Genki_Notify[0] == toggle;
		}
		else if (this.guiData.Info_Notify.Contains(toggle))
		{
			this.cloneUserOptionData.PushNotifyInfomation = this.guiData.Info_Notify[0] == toggle;
		}
		else if (this.guiData.Scenario_Speed.Contains(toggle))
		{
			this.cloneUserOptionData.ScenarioSpeed = this.guiData.Scenario_Speed.IndexOf(toggle);
		}
		else if (this.guiData.AutoSpeed.Contains(toggle))
		{
			this.cloneUserOptionData.autoSpeed = this.guiData.AutoSpeed.IndexOf(toggle);
		}
		else if (this.guiData.PhotoAffect.Contains(toggle))
		{
			this.cloneUserOptionData.ViewPhotoAffect = this.guiData.PhotoAffect[0] == toggle;
		}
		else if (this.guiData.ClothesAffect.Contains(toggle))
		{
			this.cloneUserOptionData.ViewClothesAffect = this.guiData.ClothesAffect[0] == toggle;
		}
		else if (this.guiData.MissionProgressNotify.Contains(toggle))
		{
			this.cloneUserOptionData.MissionProgressNotify = this.guiData.MissionProgressNotify.IndexOf(toggle);
		}
		this.RefreshUI();
		SceneManager.SetOption(option);
		return false;
	}

	// Token: 0x0600146B RID: 5227 RVA: 0x000F9B84 File Offset: 0x000F7D84
	private bool OnClickOwButton(int index)
	{
		return true;
	}

	// Token: 0x0600146C RID: 5228 RVA: 0x000F9B88 File Offset: 0x000F7D88
	private bool OnSelectTab(int index)
	{
		this.guiData.baseScreen.SetActive(index == 0);
		this.guiData.baseInfo.SetActive(index == 1);
		this.guiData.baseSound.SetActive(index == 2);
		this.guiData.baseScenario.SetActive(index == 3);
		this.guiData.baseEtc.SetActive(index == 4);
		return true;
	}

	// Token: 0x0600146D RID: 5229 RVA: 0x000F9BFA File Offset: 0x000F7DFA
	public bool OnClickReturnButton()
	{
		this.currentEnumerator = this.RequestUpdateOption();
		return true;
	}

	// Token: 0x0600146E RID: 5230 RVA: 0x000F9C09 File Offset: 0x000F7E09
	public bool OnClickMoveSequenceButton(SceneManager.SceneName sceneName, object sceneArgs)
	{
		this.currentEnumerator = this.RequestUpdateOption();
		this.OnClickMoveSequenceName = sceneName;
		this.OnClickMoveSequenceArgs = sceneArgs;
		return true;
	}

	// Token: 0x040010C4 RID: 4292
	public bool isDebug;

	// Token: 0x040010C5 RID: 4293
	private SelOptionCtrl.GUI guiData;

	// Token: 0x040010C6 RID: 4294
	private UserOptionData cloneUserOptionData;

	// Token: 0x040010C7 RID: 4295
	private IEnumerator currentEnumerator;

	// Token: 0x040010C8 RID: 4296
	private SceneManager.SceneName OnClickMoveSequenceName;

	// Token: 0x040010C9 RID: 4297
	private object OnClickMoveSequenceArgs;

	// Token: 0x02000B77 RID: 2935
	public enum Mode
	{
		// Token: 0x04004799 RID: 18329
		INVALID
	}

	// Token: 0x02000B78 RID: 2936
	public class GUI
	{
		// Token: 0x170009DC RID: 2524
		// (get) Token: 0x060042E1 RID: 17121 RVA: 0x0020167F File Offset: 0x001FF87F
		// (set) Token: 0x060042E2 RID: 17122 RVA: 0x00201687 File Offset: 0x001FF887
		public PguiTabGroupCtrl TabGroup { get; set; }

		// Token: 0x170009DD RID: 2525
		// (get) Token: 0x060042E3 RID: 17123 RVA: 0x00201690 File Offset: 0x001FF890
		// (set) Token: 0x060042E4 RID: 17124 RVA: 0x00201698 File Offset: 0x001FF898
		public List<PguiToggleButtonCtrl> Display_Direction { get; set; }

		// Token: 0x170009DE RID: 2526
		// (get) Token: 0x060042E5 RID: 17125 RVA: 0x002016A1 File Offset: 0x001FF8A1
		// (set) Token: 0x060042E6 RID: 17126 RVA: 0x002016A9 File Offset: 0x001FF8A9
		public List<PguiToggleButtonCtrl> GraphicsQuality { get; set; }

		// Token: 0x170009DF RID: 2527
		// (get) Token: 0x060042E7 RID: 17127 RVA: 0x002016B2 File Offset: 0x001FF8B2
		// (set) Token: 0x060042E8 RID: 17128 RVA: 0x002016BA File Offset: 0x001FF8BA
		public List<PguiToggleButtonCtrl> FrameRate { get; set; }

		// Token: 0x170009E0 RID: 2528
		// (get) Token: 0x060042E9 RID: 17129 RVA: 0x002016C3 File Offset: 0x001FF8C3
		// (set) Token: 0x060042EA RID: 17130 RVA: 0x002016CB File Offset: 0x001FF8CB
		public List<PguiToggleButtonCtrl> Stamina_Notify { get; set; }

		// Token: 0x170009E1 RID: 2529
		// (get) Token: 0x060042EB RID: 17131 RVA: 0x002016D4 File Offset: 0x001FF8D4
		// (set) Token: 0x060042EC RID: 17132 RVA: 0x002016DC File Offset: 0x001FF8DC
		public List<PguiToggleButtonCtrl> PvpStamina_Notify { get; set; }

		// Token: 0x170009E2 RID: 2530
		// (get) Token: 0x060042ED RID: 17133 RVA: 0x002016E5 File Offset: 0x001FF8E5
		// (set) Token: 0x060042EE RID: 17134 RVA: 0x002016ED File Offset: 0x001FF8ED
		public List<PguiToggleButtonCtrl> Genki_Notify { get; set; }

		// Token: 0x170009E3 RID: 2531
		// (get) Token: 0x060042EF RID: 17135 RVA: 0x002016F6 File Offset: 0x001FF8F6
		// (set) Token: 0x060042F0 RID: 17136 RVA: 0x002016FE File Offset: 0x001FF8FE
		public List<PguiToggleButtonCtrl> Info_Notify { get; set; }

		// Token: 0x170009E4 RID: 2532
		// (get) Token: 0x060042F1 RID: 17137 RVA: 0x00201707 File Offset: 0x001FF907
		// (set) Token: 0x060042F2 RID: 17138 RVA: 0x0020170F File Offset: 0x001FF90F
		public List<PguiButtonCtrl> Sound_Btn_Yaji { get; set; }

		// Token: 0x170009E5 RID: 2533
		// (get) Token: 0x060042F3 RID: 17139 RVA: 0x00201718 File Offset: 0x001FF918
		// (set) Token: 0x060042F4 RID: 17140 RVA: 0x00201720 File Offset: 0x001FF920
		public List<PguiToggleButtonCtrl> Scenario_Speed { get; set; }

		// Token: 0x170009E6 RID: 2534
		// (get) Token: 0x060042F5 RID: 17141 RVA: 0x00201729 File Offset: 0x001FF929
		// (set) Token: 0x060042F6 RID: 17142 RVA: 0x00201731 File Offset: 0x001FF931
		public List<PguiToggleButtonCtrl> AutoSpeed { get; set; }

		// Token: 0x170009E7 RID: 2535
		// (get) Token: 0x060042F7 RID: 17143 RVA: 0x0020173A File Offset: 0x001FF93A
		// (set) Token: 0x060042F8 RID: 17144 RVA: 0x00201742 File Offset: 0x001FF942
		public List<PguiToggleButtonCtrl> PhotoAffect { get; set; }

		// Token: 0x170009E8 RID: 2536
		// (get) Token: 0x060042F9 RID: 17145 RVA: 0x0020174B File Offset: 0x001FF94B
		// (set) Token: 0x060042FA RID: 17146 RVA: 0x00201753 File Offset: 0x001FF953
		public List<PguiToggleButtonCtrl> ClothesAffect { get; set; }

		// Token: 0x170009E9 RID: 2537
		// (get) Token: 0x060042FB RID: 17147 RVA: 0x0020175C File Offset: 0x001FF95C
		// (set) Token: 0x060042FC RID: 17148 RVA: 0x00201764 File Offset: 0x001FF964
		public List<PguiToggleButtonCtrl> MissionProgressNotify { get; set; }

		// Token: 0x170009EA RID: 2538
		// (get) Token: 0x060042FD RID: 17149 RVA: 0x0020176D File Offset: 0x001FF96D
		// (set) Token: 0x060042FE RID: 17150 RVA: 0x00201775 File Offset: 0x001FF975
		public List<PguiToggleButtonCtrl> ALL_Toggle { get; set; }

		// Token: 0x170009EB RID: 2539
		// (get) Token: 0x060042FF RID: 17151 RVA: 0x0020177E File Offset: 0x001FF97E
		// (set) Token: 0x06004300 RID: 17152 RVA: 0x00201786 File Offset: 0x001FF986
		public List<PguiButtonCtrl> ALL_Button { get; set; }

		// Token: 0x170009EC RID: 2540
		// (get) Token: 0x06004301 RID: 17153 RVA: 0x0020178F File Offset: 0x001FF98F
		// (set) Token: 0x06004302 RID: 17154 RVA: 0x00201797 File Offset: 0x001FF997
		public List<GameObject> Sound_Icon_01 { get; set; }

		// Token: 0x170009ED RID: 2541
		// (get) Token: 0x06004303 RID: 17155 RVA: 0x002017A0 File Offset: 0x001FF9A0
		// (set) Token: 0x06004304 RID: 17156 RVA: 0x002017A8 File Offset: 0x001FF9A8
		public List<GameObject> Sound_Icon_02 { get; set; }

		// Token: 0x170009EE RID: 2542
		// (get) Token: 0x06004305 RID: 17157 RVA: 0x002017B1 File Offset: 0x001FF9B1
		// (set) Token: 0x06004306 RID: 17158 RVA: 0x002017B9 File Offset: 0x001FF9B9
		public List<GameObject> Sound_Icon_03 { get; set; }

		// Token: 0x06004307 RID: 17159 RVA: 0x002017C4 File Offset: 0x001FF9C4
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.baseSound = baseTr.Find("All/Base/Main_Sound").gameObject;
			this.baseScreen = baseTr.Find("All/Base/Main_Screen").gameObject;
			this.baseInfo = baseTr.Find("All/Base/Main_Info").gameObject;
			this.baseScenario = baseTr.Find("All/Base/Main_Scenario").gameObject;
			this.baseEtc = baseTr.Find("All/Base/Main_Etc").gameObject;
			this.TabGroup = baseTr.Find("All/Base/TabGroup").GetComponent<PguiTabGroupCtrl>();
			this.Display_Direction = new List<PguiToggleButtonCtrl>
			{
				baseTr.Find("All/Base/Main_Screen/Box01/Btn_Radio01").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("All/Base/Main_Screen/Box01/Btn_Radio02").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("All/Base/Main_Screen/Box01/Btn_Radio03").GetComponent<PguiToggleButtonCtrl>()
			};
			this.GraphicsQuality = new List<PguiToggleButtonCtrl>
			{
				baseTr.Find("All/Base/Main_Screen/Box02/Btn_Radio01").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("All/Base/Main_Screen/Box02/Btn_Radio02").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("All/Base/Main_Screen/Box02/Btn_Radio03").GetComponent<PguiToggleButtonCtrl>()
			};
			this.FrameRate = new List<PguiToggleButtonCtrl>
			{
				baseTr.Find("All/Base/Main_Screen/Box03/Btn_Radio03").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("All/Base/Main_Screen/Box03/Btn_Radio01").GetComponent<PguiToggleButtonCtrl>()
			};
			this.Stamina_Notify = new List<PguiToggleButtonCtrl>
			{
				baseTr.Find("All/Base/Main_Info/Box01/Btn_Radio02").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("All/Base/Main_Info/Box01/Btn_Radio03").GetComponent<PguiToggleButtonCtrl>()
			};
			this.PvpStamina_Notify = new List<PguiToggleButtonCtrl>
			{
				baseTr.Find("All/Base/Main_Info/Box02/Btn_Radio02").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("All/Base/Main_Info/Box02/Btn_Radio03").GetComponent<PguiToggleButtonCtrl>()
			};
			this.Genki_Notify = new List<PguiToggleButtonCtrl>
			{
				baseTr.Find("All/Base/Main_Info/Box03/Btn_Radio02").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("All/Base/Main_Info/Box03/Btn_Radio03").GetComponent<PguiToggleButtonCtrl>()
			};
			this.Info_Notify = new List<PguiToggleButtonCtrl>
			{
				baseTr.Find("All/Base/Main_Info/Box04/Btn_Radio02").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("All/Base/Main_Info/Box04/Btn_Radio03").GetComponent<PguiToggleButtonCtrl>()
			};
			this.Sound_Btn_Yaji = new List<PguiButtonCtrl>
			{
				baseTr.Find("All/Base/Main_Sound/Box01/Btn_Yaji_Left").GetComponent<PguiButtonCtrl>(),
				baseTr.Find("All/Base/Main_Sound/Box01/Btn_Yaji_Right").GetComponent<PguiButtonCtrl>(),
				baseTr.Find("All/Base/Main_Sound/Box02/Btn_Yaji_Left").GetComponent<PguiButtonCtrl>(),
				baseTr.Find("All/Base/Main_Sound/Box02/Btn_Yaji_Right").GetComponent<PguiButtonCtrl>(),
				baseTr.Find("All/Base/Main_Sound/Box03/Btn_Yaji_Left").GetComponent<PguiButtonCtrl>(),
				baseTr.Find("All/Base/Main_Sound/Box03/Btn_Yaji_Right").GetComponent<PguiButtonCtrl>()
			};
			this.Sound_Icon_01 = new List<GameObject>();
			this.Sound_Icon_02 = new List<GameObject>();
			this.Sound_Icon_03 = new List<GameObject>();
			for (int i = 1; i < 10; i++)
			{
				this.Sound_Icon_01.Add(baseTr.Find("All/Base/Main_Sound/Box01/Img_Sound/Img_Sound0" + i.ToString()).gameObject);
				this.Sound_Icon_02.Add(baseTr.Find("All/Base/Main_Sound/Box02/Img_Sound/Img_Sound0" + i.ToString()).gameObject);
				this.Sound_Icon_03.Add(baseTr.Find("All/Base/Main_Sound/Box03/Img_Sound/Img_Sound0" + i.ToString()).gameObject);
			}
			this.Scenario_Speed = new List<PguiToggleButtonCtrl>
			{
				baseTr.Find("All/Base/Main_Scenario/Box01/Btn_Radio01").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("All/Base/Main_Scenario/Box01/Btn_Radio02").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("All/Base/Main_Scenario/Box01/Btn_Radio03").GetComponent<PguiToggleButtonCtrl>()
			};
			this.AutoSpeed = new List<PguiToggleButtonCtrl>
			{
				baseTr.Find("All/Base/Main_Scenario/Box02/Btn_Radio01").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("All/Base/Main_Scenario/Box02/Btn_Radio02").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("All/Base/Main_Scenario/Box02/Btn_Radio03").GetComponent<PguiToggleButtonCtrl>()
			};
			this.PhotoAffect = new List<PguiToggleButtonCtrl>
			{
				baseTr.Find("All/Base/Main_Etc/Box01/Btn_Radio02").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("All/Base/Main_Etc/Box01/Btn_Radio03").GetComponent<PguiToggleButtonCtrl>()
			};
			this.ClothesAffect = new List<PguiToggleButtonCtrl>
			{
				baseTr.Find("All/Base/Main_Etc/Box02/Btn_Radio02").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("All/Base/Main_Etc/Box02/Btn_Radio03").GetComponent<PguiToggleButtonCtrl>()
			};
			this.MissionProgressNotify = new List<PguiToggleButtonCtrl>
			{
				baseTr.Find("All/Base/Main_Etc/Box03/Btn_Radio01").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("All/Base/Main_Etc/Box03/Btn_Radio02").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("All/Base/Main_Etc/Box03/Btn_Radio03").GetComponent<PguiToggleButtonCtrl>()
			};
			this.animation = baseTr.GetComponent<SimpleAnimation>();
			this.ALL_Toggle = new List<PguiToggleButtonCtrl>();
			this.ALL_Toggle.AddRange(baseTr.GetComponentsInChildren<PguiToggleButtonCtrl>());
			this.ALL_Button = new List<PguiButtonCtrl>();
			this.ALL_Button.AddRange(baseTr.GetComponentsInChildren<PguiButtonCtrl>());
		}

		// Token: 0x0400479A RID: 18330
		public GameObject baseObj;

		// Token: 0x040047AE RID: 18350
		public GameObject baseSound;

		// Token: 0x040047AF RID: 18351
		public GameObject baseScreen;

		// Token: 0x040047B0 RID: 18352
		public GameObject baseInfo;

		// Token: 0x040047B1 RID: 18353
		public GameObject baseScenario;

		// Token: 0x040047B2 RID: 18354
		public GameObject baseEtc;

		// Token: 0x040047B3 RID: 18355
		public SimpleAnimation animation;
	}
}
