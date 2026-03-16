using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AccessoryWindowCtrl : MonoBehaviour
{
	public void Initialize()
	{
		this.guiData = new AccessoryWindowCtrl.GUI(base.transform);
		this.guiData.Btn_Key.AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickLockButton));
		this.guiData.Btn_Yaji_Left.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickLeftArrowButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Btn_Yaji_Right.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickRightArrowButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Btn_AccessoryGrow.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickAccessoryGrowButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.windowCloseEndCb = null;
		this.updateStasusEndCb = null;
		this.dispAccessory = null;
		this.dispAccessoryList = null;
		this.haveAccessory = false;
		this.pvpSeasonId = 0;
		this.ResetPrevData();
	}

	public void Update()
	{
		if (this.RequestStatusFunc != null && !this.RequestStatusFunc.MoveNext())
		{
			this.RequestStatusFunc = null;
			UnityAction unityAction = this.updateStasusEndCb;
			if (unityAction == null)
			{
				return;
			}
			unityAction();
		}
	}

	public void Open(AccessoryWindowCtrl.SetupParam setupParam)
	{
		this.windowCloseEndCb = setupParam.windowCloseEndCb;
		this.updateStasusEndCb = setupParam.updateStasusEndCb;
		this.dispAccessoryList = setupParam.acceList;
		this.ChangeAccessory(setupParam.acce);
		bool flag = AccessoryUtil.IsInvalid(setupParam.acce) && 0 < setupParam.dispMaxAccessoryId;
		this.guiData.Txt_GachaInfo.gameObject.SetActive(flag);
		if (flag)
		{
			this.DispMaxLevelAccesory(setupParam.dispMaxAccessoryId);
		}
		this.guiData.owCtrl.Open();
		UnityAction windowOpenEndCb = setupParam.windowOpenEndCb;
		if (windowOpenEndCb == null)
		{
			return;
		}
		windowOpenEndCb();
	}

	public void OpenPrev()
	{
		if (this.prevOpenParam == null)
		{
			return;
		}
		if (!DataManager.DmChAccessory.GetUserAccessoryList().Exists((DataManagerCharaAccessory.Accessory x) => x.UniqId == this.prevOpenParam.acce.UniqId))
		{
			return;
		}
		AccessoryWindowCtrl.SetupParam setupParam = this.prevOpenParam;
		this.ResetPrevData();
		this.Open(setupParam);
	}

	public void ResetPrevData()
	{
		this.prevOpenParam = null;
	}

	private void ChangeAccessory(DataManagerCharaAccessory.Accessory acce)
	{
		this.dispAccessory = acce;
		this.haveAccessory = !AccessoryUtil.IsInvalid(acce) && DataManager.DmChAccessory.GetUserAccessoryList().Exists((DataManagerCharaAccessory.Accessory item) => item.UniqId == acce.UniqId);
		this.RefleshIcon();
		this.UpdateInfo();
		this.guiData.owCtrl.Setup((!AccessoryUtil.IsInvalid(acce)) ? acce.AccessoryData.Name : string.Empty, null, null, true, new PguiOpenWindowCtrl.Callback(this.OnClickOwButton), null, false);
		this.SetupKeyButton();
		this.SetupArrowButton();
		this.SetupAccessoryGrowButton();
	}

	private void DispMaxLevelAccesory(int accessoryId)
	{
		DataManagerCharaAccessory.AccessoryData accessoryData = DataManager.DmChAccessory.GetAccessoryData(accessoryId);
		this.guiData.iconAccessoryCtrl.SetupByAccessoryData(accessoryData, true, false);
		this.guiData.Txt_DispType.text = AccessoryUtil.MakeDispTypeStringByAccessoryData(accessoryData);
		this.guiData.Txt_Lv.text = AccessoryUtil.MakeLevelMaxString(accessoryData);
		this.guiData.Txt_Info.text = ((accessoryData != null) ? accessoryData.FlavorText : string.Empty);
		this.guiData.owCtrl.Setup((accessoryData != null) ? accessoryData.Name : string.Empty, null, null, true, new PguiOpenWindowCtrl.Callback(this.OnClickOwButton), null, false);
		List<AccessoryUtil.ParamPackData.BaseParam> list = AccessoryUtil.ParamPackData.CreateDispListForLvMax(accessoryData);
		this.UpdateStatusUpParam(list);
		if (accessoryData != null && accessoryData.LevelupNum > 0)
		{
			this.guiData.Txt_GachaInfo.gameObject.SetActive(false);
			this.guiData.Txt_Lv.text = string.Format("Lv.1/{0}", accessoryData.Rarity.LevelLimit);
		}
	}

	private void RefleshIcon()
	{
		this.guiData.iconAccessoryCtrl.Setup(new IconAccessoryCtrl.SetupParam
		{
			acce = this.dispAccessory,
			sortType = SortFilterDefine.SortType.INVALID,
			isEnableRaycast = false
		});
		if (AccessoryUtil.IsDecidedOwner(this.dispAccessory))
		{
			CharaStaticData charaStaticData = DataManager.DmChara.GetCharaStaticData(this.dispAccessory.CharaId);
			this.guiData.iconAccessoryCtrl.DispIconCharaMini(charaStaticData != null, charaStaticData);
		}
	}

	private void UpdateInfo()
	{
		this.guiData.Txt_DispType.text = AccessoryUtil.MakeDispTypeString(this.dispAccessory);
		this.guiData.Txt_Lv.text = AccessoryUtil.MakeLevelString(this.dispAccessory, true);
		this.guiData.Txt_Info.text = ((!AccessoryUtil.IsInvalid(this.dispAccessory)) ? this.dispAccessory.AccessoryData.FlavorText : string.Empty);
		List<AccessoryUtil.ParamPackData.BaseParam> list = new List<AccessoryUtil.ParamPackData.BaseParam>();
		AccessoryUtil.ParamPackData.CreateDispList<AccessoryUtil.ParamPackData.BaseParam>(ref list, new AccessoryUtil.ParamPackData.AccessoryPackData
		{
			accessory = this.dispAccessory
		});
		this.UpdateStatusUpParam(list);
	}

	private void UpdateStatusUpParam(List<AccessoryUtil.ParamPackData.BaseParam> dispStatusList)
	{
		for (int i = 0; i < this.guiData.guiParamUpList.Count; i++)
		{
			AccessoryUtil.ParamPackData.BaseParam baseParam = ((i < dispStatusList.Count) ? dispStatusList[i] : new AccessoryUtil.ParamPackData.BaseParam());
			this.guiData.guiParamUpList[i].Setup(baseParam);
		}
	}

	private void SetupKeyButton()
	{
		int num = ((!AccessoryUtil.IsInvalid(this.dispAccessory) && this.dispAccessory.IsLock) ? 1 : 0);
		this.guiData.Btn_Key.SetToggleIndex(num);
		this.guiData.Btn_Key.gameObject.SetActive(this.haveAccessory);
	}

	private void SetupArrowButton()
	{
		bool flag = this.IsDispArrowButton();
		this.guiData.Btn_Yaji_Left.gameObject.SetActive(flag);
		this.guiData.Btn_Yaji_Right.gameObject.SetActive(flag);
	}

	private bool IsDispArrowButton()
	{
		return !AccessoryUtil.IsInvalid(this.dispAccessory) && this.dispAccessoryList != null && 1 < this.dispAccessoryList.Count && 0 <= this.dispAccessoryList.IndexOf(this.dispAccessory);
	}

	private void SetupAccessoryGrowButton()
	{
		bool flag = this.prevOpenParam != null;
		bool flag2 = this.haveAccessory && !AccessoryUtil.IsInvalid(this.dispAccessory) && AccessoryUtil.CanEquipped(this.dispAccessory) && this.dispAccessory.Level < this.dispAccessory.AccessoryData.Rarity.LevelLimit && !flag && !CanvasManager.HdlDressUpWindowCtrl.IsActive();
		this.guiData.Btn_AccessoryGrow.gameObject.SetActive(flag2);
	}

	public void SetPvpSeasonId(int seasonId)
	{
		this.pvpSeasonId = seasonId;
	}

	private IEnumerator RequestUpdateStatus()
	{
		if (AccessoryUtil.IsInvalid(this.dispAccessory))
		{
			yield break;
		}
		bool flag = !this.dispAccessory.IsLock;
		DataManager.DmChAccessory.RequestActionUpdateStatus(this.dispAccessory, flag);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		this.RefleshIcon();
		yield break;
	}

	private bool OnClickOwButton(int index)
	{
		if (-1 == index)
		{
			UnityAction unityAction = this.windowCloseEndCb;
			if (unityAction != null)
			{
				unityAction();
			}
			this.windowCloseEndCb = null;
			return true;
		}
		return false;
	}

	private void OnClickLeftArrowButton(PguiButtonCtrl button)
	{
		this.RequestClickArrowButton(button, -1);
	}

	private void OnClickRightArrowButton(PguiButtonCtrl button)
	{
		this.RequestClickArrowButton(button, 1);
	}

	private void RequestClickArrowButton(PguiButtonCtrl button, int slideIndexNum)
	{
		int num = this.dispAccessoryList.IndexOf(this.dispAccessory);
		num += slideIndexNum;
		int num2 = this.dispAccessoryList.Count - 1;
		if (num < 0)
		{
			num = num2;
		}
		if (num2 < num)
		{
			num = 0;
		}
		this.ChangeAccessory(this.dispAccessoryList[num]);
	}

	private bool OnClickLockButton(PguiToggleButtonCtrl pbc, int toggleIndex)
	{
		SoundManager.Play("prd_se_photo_lock_unlock", false, false);
		bool flag = toggleIndex == 0;
		if (this.dispAccessory.IsLock != flag && this.RequestStatusFunc == null)
		{
			this.RequestStatusFunc = this.RequestUpdateStatus();
		}
		return true;
	}

	private void OnClickAccessoryGrowButton(PguiButtonCtrl button)
	{
		object obj = null;
		SceneManager.SceneName currentSceneName = Singleton<SceneManager>.Instance.CurrentSceneName;
		if (currentSceneName <= SceneManager.SceneName.SceneCharaEdit)
		{
			if (currentSceneName != SceneManager.SceneName.SceneBattleSelector)
			{
				if (currentSceneName == SceneManager.SceneName.SceneCharaEdit)
				{
					obj = new SceneCharaEdit.Args
					{
						detailAccessoryId = this.dispAccessory.UniqId,
						openDetailWindow = true
					};
				}
			}
			else
			{
				obj = new SceneBattleSelector.Args
				{
					detailAccesssoryId = this.dispAccessory.UniqId
				};
			}
		}
		else if (currentSceneName != SceneManager.SceneName.SceneProfile)
		{
			if (currentSceneName != SceneManager.SceneName.ScenePvpDeck)
			{
				if (currentSceneName == SceneManager.SceneName.SceneTraining)
				{
					obj = new SceneTraining.Args
					{
						deck = true,
						openAccessoryWindow = true
					};
				}
			}
			else
			{
				obj = new ScenePvpDeck.Args
				{
					pvpSeasonId = this.pvpSeasonId,
					openAccessoryWindow = true
				};
			}
		}
		else
		{
			obj = new SceneProfile.Args
			{
				isHelperSettingStartFromCharaEdit = true,
				openAccessoryWindow = true
			};
		}
		this.prevOpenParam = new AccessoryWindowCtrl.SetupParam
		{
			acce = this.dispAccessory,
			acceList = this.dispAccessoryList,
			updateStasusEndCb = this.updateStasusEndCb,
			windowCloseEndCb = this.windowCloseEndCb
		};
		this.guiData.owCtrl.ForceClose();
		if (CanvasManager.HdlDetachableAccessoryWindowCtrl.IsActiveWIndow())
		{
			CanvasManager.HdlDetachableAccessoryWindowCtrl.ForceClose();
		}
		SceneCharaEdit.Args args = new SceneCharaEdit.Args
		{
			growAccessoryId = this.dispAccessory.UniqId,
			requestAccessoryGrowSubMode = SelAccessoryGrowCtrl.Mode.GROW,
			menuBackSceneArgs = obj,
			menuBackSceneName = Singleton<SceneManager>.Instance.CurrentSceneName
		};
		Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneCharaEdit, args);
	}

	public AccessoryWindowCtrl.GUI guiData;

	private UnityAction windowCloseEndCb;

	private UnityAction updateStasusEndCb;

	private DataManagerCharaAccessory.Accessory dispAccessory;

	private List<DataManagerCharaAccessory.Accessory> dispAccessoryList;

	private bool haveAccessory;

	private AccessoryWindowCtrl.SetupParam prevOpenParam;

	private int pvpSeasonId;

	private IEnumerator RequestStatusFunc;

	public class GUI
	{
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.Txt_GachaInfo = baseTr.Find("Base/Window/Txt_GachaInfo").GetComponent<PguiTextCtrl>();
			this.Btn_Key = baseTr.Find("Base/Window/BtnKey").GetComponent<PguiToggleButtonCtrl>();
			this.iconAccessoryCtrl = baseTr.Find("Base/Window/ParamInfo/Left/Icon_Accessory").GetComponent<IconAccessoryCtrl>();
			this.Btn_Yaji_Left = baseTr.Find("Base/Window/Btn_Yaji_Left").GetComponent<PguiButtonCtrl>();
			this.Btn_Yaji_Right = baseTr.Find("Base/Window/Btn_Yaji_Right").GetComponent<PguiButtonCtrl>();
			this.Txt_DispType = baseTr.Find("Base/Window/ParamInfo/Left/VeiwTipe/Txt02").GetComponent<PguiTextCtrl>();
			this.Txt_Lv = baseTr.Find("Base/Window/ParamInfo/Box01/Num_Lv").GetComponent<PguiTextCtrl>();
			this.guiParamUpList = new List<AccessoryWindowCtrl.GUIParamUp>();
			for (int i = 0; i < 3; i++)
			{
				AccessoryWindowCtrl.GUIParamUp guiparamUp = new AccessoryWindowCtrl.GUIParamUp(baseTr.Find(string.Format("Base/Window/ParamInfo/Box01/Param0{0}", i + 1)));
				this.guiParamUpList.Add(guiparamUp);
			}
			this.Txt_Info = baseTr.Find("Base/Window/ParamInfo/Box02/Txt").GetComponent<PguiTextCtrl>();
			this.Btn_AccessoryGrow = baseTr.Find("Base/Window/Btn_AccessoryGrow").GetComponent<PguiButtonCtrl>();
		}

		public GameObject baseObj;

		public PguiOpenWindowCtrl owCtrl;

		public PguiTextCtrl Txt_GachaInfo;

		public PguiToggleButtonCtrl Btn_Key;

		public IconAccessoryCtrl iconAccessoryCtrl;

		public PguiButtonCtrl Btn_Yaji_Left;

		public PguiButtonCtrl Btn_Yaji_Right;

		public PguiTextCtrl Txt_DispType;

		public PguiTextCtrl Txt_Lv;

		public List<AccessoryWindowCtrl.GUIParamUp> guiParamUpList;

		public PguiTextCtrl Txt_Info;

		public PguiButtonCtrl Btn_AccessoryGrow;
	}

	public class GUIParamUp
	{
		public GUIParamUp(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Txt_Num = baseTr.Find("Num_01").GetComponent<PguiTextCtrl>();
			this.nomal = new AccessoryWindowCtrl.GUIParamUp.Normal(baseTr.Find("Base_Nomal"));
			this.damage = new AccessoryWindowCtrl.GUIParamUp.Damage(baseTr.Find("Base_Dmage"));
		}

		public void Setup(AccessoryUtil.ParamPackData.BaseParam param)
		{
			this.baseObj.SetActive(false);
			switch (param.type)
			{
			case AccessoryUtil.ParamType.Normal:
				this.baseObj.SetActive(true);
				this.Txt_Num.text = param.value.ToString();
				this.nomal.baseObj.SetActive(true);
				this.nomal.Txt_ParamName.text = param.name;
				this.damage.baseObj.SetActive(false);
				return;
			case AccessoryUtil.ParamType.Beat:
				this.baseObj.SetActive(true);
				this.Txt_Num.text = AccessoryUtil.GetPermillageText(param.value);
				this.nomal.baseObj.SetActive(false);
				this.damage.baseObj.SetActive(true);
				this.damage.Txt_ParamName.text = param.name;
				this.damage.gradientCtrl.SetGameObjectById("Beat");
				this.damage.Img_Bg.color = this.damage.colorCtrl.GetGameObjectById("Beat");
				return;
			case AccessoryUtil.ParamType.Action:
				this.baseObj.SetActive(true);
				this.Txt_Num.text = AccessoryUtil.GetPermillageText(param.value);
				this.nomal.baseObj.SetActive(false);
				this.damage.baseObj.SetActive(true);
				this.damage.Txt_ParamName.text = param.name;
				this.damage.gradientCtrl.SetGameObjectById("Action");
				this.damage.Img_Bg.color = this.damage.colorCtrl.GetGameObjectById("Action");
				return;
			case AccessoryUtil.ParamType.Try:
				this.baseObj.SetActive(true);
				this.Txt_Num.text = AccessoryUtil.GetPermillageText(param.value);
				this.nomal.baseObj.SetActive(false);
				this.damage.baseObj.SetActive(true);
				this.damage.Txt_ParamName.text = param.name;
				this.damage.gradientCtrl.SetGameObjectById("Try");
				this.damage.Img_Bg.color = this.damage.colorCtrl.GetGameObjectById("Try");
				return;
			case AccessoryUtil.ParamType.Avoid:
				this.baseObj.SetActive(true);
				this.Txt_Num.text = AccessoryUtil.GetPermillageText(param.value);
				this.nomal.baseObj.SetActive(true);
				this.nomal.Txt_ParamName.text = param.name;
				this.damage.baseObj.SetActive(false);
				return;
			default:
				return;
			}
		}

		public GameObject baseObj;

		public PguiTextCtrl Txt_Num;

		public AccessoryWindowCtrl.GUIParamUp.Normal nomal;

		public AccessoryWindowCtrl.GUIParamUp.Damage damage;

		public class Normal
		{
			public Normal(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Txt_ParamName = baseTr.Find("Txt_01").GetComponent<PguiTextCtrl>();
			}

			public GameObject baseObj;

			public PguiTextCtrl Txt_ParamName;
		}

		public class Damage
		{
			public Damage(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Txt_ParamName = baseTr.Find("Txt_01").GetComponent<PguiTextCtrl>();
				this.gradientCtrl = baseTr.Find("Txt_01").GetComponent<PguiGradientCtrl>();
				this.Img_Bg = baseTr.GetComponent<Image>();
				this.colorCtrl = baseTr.GetComponent<PguiColorCtrl>();
			}

			public GameObject baseObj;

			public PguiTextCtrl Txt_ParamName;

			public PguiGradientCtrl gradientCtrl;

			public Image Img_Bg;

			public PguiColorCtrl colorCtrl;
		}
	}

	public class SetupParam
	{
		public SetupParam()
		{
			this.windowOpenEndCb = null;
			this.windowCloseEndCb = null;
			this.updateStasusEndCb = null;
			this.acce = null;
			this.acceList = null;
			this.dispMaxAccessoryId = 0;
		}

		public UnityAction windowOpenEndCb;

		public UnityAction windowCloseEndCb;

		public UnityAction updateStasusEndCb;

		public DataManagerCharaAccessory.Accessory acce;

		public List<DataManagerCharaAccessory.Accessory> acceList;

		public int dispMaxAccessoryId;
	}
}
