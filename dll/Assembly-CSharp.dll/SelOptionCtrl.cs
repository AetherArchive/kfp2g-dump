using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using UnityEngine;

public class SelOptionCtrl : MonoBehaviour
{
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

	public UserOptionData GetCustomData()
	{
		return this.cloneUserOptionData;
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (this.currentEnumerator != null && !this.currentEnumerator.MoveNext())
		{
			this.currentEnumerator = null;
		}
	}

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

	private bool OnClickOwButton(int index)
	{
		return true;
	}

	private bool OnSelectTab(int index)
	{
		this.guiData.baseScreen.SetActive(index == 0);
		this.guiData.baseInfo.SetActive(index == 1);
		this.guiData.baseSound.SetActive(index == 2);
		this.guiData.baseScenario.SetActive(index == 3);
		this.guiData.baseEtc.SetActive(index == 4);
		return true;
	}

	public bool OnClickReturnButton()
	{
		this.currentEnumerator = this.RequestUpdateOption();
		return true;
	}

	public bool OnClickMoveSequenceButton(SceneManager.SceneName sceneName, object sceneArgs)
	{
		this.currentEnumerator = this.RequestUpdateOption();
		this.OnClickMoveSequenceName = sceneName;
		this.OnClickMoveSequenceArgs = sceneArgs;
		return true;
	}

	public bool isDebug;

	private SelOptionCtrl.GUI guiData;

	private UserOptionData cloneUserOptionData;

	private IEnumerator currentEnumerator;

	private SceneManager.SceneName OnClickMoveSequenceName;

	private object OnClickMoveSequenceArgs;

	public enum Mode
	{
		INVALID
	}

	public class GUI
	{
		public PguiTabGroupCtrl TabGroup { get; set; }

		public List<PguiToggleButtonCtrl> Display_Direction { get; set; }

		public List<PguiToggleButtonCtrl> GraphicsQuality { get; set; }

		public List<PguiToggleButtonCtrl> FrameRate { get; set; }

		public List<PguiToggleButtonCtrl> Stamina_Notify { get; set; }

		public List<PguiToggleButtonCtrl> PvpStamina_Notify { get; set; }

		public List<PguiToggleButtonCtrl> Genki_Notify { get; set; }

		public List<PguiToggleButtonCtrl> Info_Notify { get; set; }

		public List<PguiButtonCtrl> Sound_Btn_Yaji { get; set; }

		public List<PguiToggleButtonCtrl> Scenario_Speed { get; set; }

		public List<PguiToggleButtonCtrl> AutoSpeed { get; set; }

		public List<PguiToggleButtonCtrl> PhotoAffect { get; set; }

		public List<PguiToggleButtonCtrl> ClothesAffect { get; set; }

		public List<PguiToggleButtonCtrl> MissionProgressNotify { get; set; }

		public List<PguiToggleButtonCtrl> ALL_Toggle { get; set; }

		public List<PguiButtonCtrl> ALL_Button { get; set; }

		public List<GameObject> Sound_Icon_01 { get; set; }

		public List<GameObject> Sound_Icon_02 { get; set; }

		public List<GameObject> Sound_Icon_03 { get; set; }

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

		public GameObject baseObj;

		public GameObject baseSound;

		public GameObject baseScreen;

		public GameObject baseInfo;

		public GameObject baseScenario;

		public GameObject baseEtc;

		public SimpleAnimation animation;
	}
}
