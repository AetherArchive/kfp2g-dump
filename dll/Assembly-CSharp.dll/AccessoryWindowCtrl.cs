using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200018D RID: 397
public class AccessoryWindowCtrl : MonoBehaviour
{
	// Token: 0x06001A7B RID: 6779 RVA: 0x00156B90 File Offset: 0x00154D90
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

	// Token: 0x06001A7C RID: 6780 RVA: 0x00156C51 File Offset: 0x00154E51
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

	// Token: 0x06001A7D RID: 6781 RVA: 0x00156C80 File Offset: 0x00154E80
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

	// Token: 0x06001A7E RID: 6782 RVA: 0x00156D1C File Offset: 0x00154F1C
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

	// Token: 0x06001A7F RID: 6783 RVA: 0x00156D64 File Offset: 0x00154F64
	public void ResetPrevData()
	{
		this.prevOpenParam = null;
	}

	// Token: 0x06001A80 RID: 6784 RVA: 0x00156D70 File Offset: 0x00154F70
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

	// Token: 0x06001A81 RID: 6785 RVA: 0x00156E2C File Offset: 0x0015502C
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

	// Token: 0x06001A82 RID: 6786 RVA: 0x00156F30 File Offset: 0x00155130
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

	// Token: 0x06001A83 RID: 6787 RVA: 0x00156FA4 File Offset: 0x001551A4
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

	// Token: 0x06001A84 RID: 6788 RVA: 0x00157044 File Offset: 0x00155244
	private void UpdateStatusUpParam(List<AccessoryUtil.ParamPackData.BaseParam> dispStatusList)
	{
		for (int i = 0; i < this.guiData.guiParamUpList.Count; i++)
		{
			AccessoryUtil.ParamPackData.BaseParam baseParam = ((i < dispStatusList.Count) ? dispStatusList[i] : new AccessoryUtil.ParamPackData.BaseParam());
			this.guiData.guiParamUpList[i].Setup(baseParam);
		}
	}

	// Token: 0x06001A85 RID: 6789 RVA: 0x0015709C File Offset: 0x0015529C
	private void SetupKeyButton()
	{
		int num = ((!AccessoryUtil.IsInvalid(this.dispAccessory) && this.dispAccessory.IsLock) ? 1 : 0);
		this.guiData.Btn_Key.SetToggleIndex(num);
		this.guiData.Btn_Key.gameObject.SetActive(this.haveAccessory);
	}

	// Token: 0x06001A86 RID: 6790 RVA: 0x001570F4 File Offset: 0x001552F4
	private void SetupArrowButton()
	{
		bool flag = this.IsDispArrowButton();
		this.guiData.Btn_Yaji_Left.gameObject.SetActive(flag);
		this.guiData.Btn_Yaji_Right.gameObject.SetActive(flag);
	}

	// Token: 0x06001A87 RID: 6791 RVA: 0x00157134 File Offset: 0x00155334
	private bool IsDispArrowButton()
	{
		return !AccessoryUtil.IsInvalid(this.dispAccessory) && this.dispAccessoryList != null && 1 < this.dispAccessoryList.Count && 0 <= this.dispAccessoryList.IndexOf(this.dispAccessory);
	}

	// Token: 0x06001A88 RID: 6792 RVA: 0x00157174 File Offset: 0x00155374
	private void SetupAccessoryGrowButton()
	{
		bool flag = this.prevOpenParam != null;
		bool flag2 = this.haveAccessory && !AccessoryUtil.IsInvalid(this.dispAccessory) && AccessoryUtil.CanEquipped(this.dispAccessory) && this.dispAccessory.Level < this.dispAccessory.AccessoryData.Rarity.LevelLimit && !flag && !CanvasManager.HdlDressUpWindowCtrl.IsActive();
		this.guiData.Btn_AccessoryGrow.gameObject.SetActive(flag2);
	}

	// Token: 0x06001A89 RID: 6793 RVA: 0x001571F9 File Offset: 0x001553F9
	public void SetPvpSeasonId(int seasonId)
	{
		this.pvpSeasonId = seasonId;
	}

	// Token: 0x06001A8A RID: 6794 RVA: 0x00157202 File Offset: 0x00155402
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

	// Token: 0x06001A8B RID: 6795 RVA: 0x00157211 File Offset: 0x00155411
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

	// Token: 0x06001A8C RID: 6796 RVA: 0x00157232 File Offset: 0x00155432
	private void OnClickLeftArrowButton(PguiButtonCtrl button)
	{
		this.RequestClickArrowButton(button, -1);
	}

	// Token: 0x06001A8D RID: 6797 RVA: 0x0015723C File Offset: 0x0015543C
	private void OnClickRightArrowButton(PguiButtonCtrl button)
	{
		this.RequestClickArrowButton(button, 1);
	}

	// Token: 0x06001A8E RID: 6798 RVA: 0x00157248 File Offset: 0x00155448
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

	// Token: 0x06001A8F RID: 6799 RVA: 0x00157298 File Offset: 0x00155498
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

	// Token: 0x06001A90 RID: 6800 RVA: 0x001572E0 File Offset: 0x001554E0
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

	// Token: 0x04001436 RID: 5174
	public AccessoryWindowCtrl.GUI guiData;

	// Token: 0x04001437 RID: 5175
	private UnityAction windowCloseEndCb;

	// Token: 0x04001438 RID: 5176
	private UnityAction updateStasusEndCb;

	// Token: 0x04001439 RID: 5177
	private DataManagerCharaAccessory.Accessory dispAccessory;

	// Token: 0x0400143A RID: 5178
	private List<DataManagerCharaAccessory.Accessory> dispAccessoryList;

	// Token: 0x0400143B RID: 5179
	private bool haveAccessory;

	// Token: 0x0400143C RID: 5180
	private AccessoryWindowCtrl.SetupParam prevOpenParam;

	// Token: 0x0400143D RID: 5181
	private int pvpSeasonId;

	// Token: 0x0400143E RID: 5182
	private IEnumerator RequestStatusFunc;

	// Token: 0x02000E76 RID: 3702
	public class GUI
	{
		// Token: 0x06004CC5 RID: 19653 RVA: 0x0022F298 File Offset: 0x0022D498
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

		// Token: 0x04005334 RID: 21300
		public GameObject baseObj;

		// Token: 0x04005335 RID: 21301
		public PguiOpenWindowCtrl owCtrl;

		// Token: 0x04005336 RID: 21302
		public PguiTextCtrl Txt_GachaInfo;

		// Token: 0x04005337 RID: 21303
		public PguiToggleButtonCtrl Btn_Key;

		// Token: 0x04005338 RID: 21304
		public IconAccessoryCtrl iconAccessoryCtrl;

		// Token: 0x04005339 RID: 21305
		public PguiButtonCtrl Btn_Yaji_Left;

		// Token: 0x0400533A RID: 21306
		public PguiButtonCtrl Btn_Yaji_Right;

		// Token: 0x0400533B RID: 21307
		public PguiTextCtrl Txt_DispType;

		// Token: 0x0400533C RID: 21308
		public PguiTextCtrl Txt_Lv;

		// Token: 0x0400533D RID: 21309
		public List<AccessoryWindowCtrl.GUIParamUp> guiParamUpList;

		// Token: 0x0400533E RID: 21310
		public PguiTextCtrl Txt_Info;

		// Token: 0x0400533F RID: 21311
		public PguiButtonCtrl Btn_AccessoryGrow;
	}

	// Token: 0x02000E77 RID: 3703
	public class GUIParamUp
	{
		// Token: 0x06004CC6 RID: 19654 RVA: 0x0022F3CC File Offset: 0x0022D5CC
		public GUIParamUp(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Txt_Num = baseTr.Find("Num_01").GetComponent<PguiTextCtrl>();
			this.nomal = new AccessoryWindowCtrl.GUIParamUp.Normal(baseTr.Find("Base_Nomal"));
			this.damage = new AccessoryWindowCtrl.GUIParamUp.Damage(baseTr.Find("Base_Dmage"));
		}

		// Token: 0x06004CC7 RID: 19655 RVA: 0x0022F430 File Offset: 0x0022D630
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

		// Token: 0x04005340 RID: 21312
		public GameObject baseObj;

		// Token: 0x04005341 RID: 21313
		public PguiTextCtrl Txt_Num;

		// Token: 0x04005342 RID: 21314
		public AccessoryWindowCtrl.GUIParamUp.Normal nomal;

		// Token: 0x04005343 RID: 21315
		public AccessoryWindowCtrl.GUIParamUp.Damage damage;

		// Token: 0x020011E4 RID: 4580
		public class Normal
		{
			// Token: 0x06005751 RID: 22353 RVA: 0x00256806 File Offset: 0x00254A06
			public Normal(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Txt_ParamName = baseTr.Find("Txt_01").GetComponent<PguiTextCtrl>();
			}

			// Token: 0x04006206 RID: 25094
			public GameObject baseObj;

			// Token: 0x04006207 RID: 25095
			public PguiTextCtrl Txt_ParamName;
		}

		// Token: 0x020011E5 RID: 4581
		public class Damage
		{
			// Token: 0x06005752 RID: 22354 RVA: 0x00256830 File Offset: 0x00254A30
			public Damage(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Txt_ParamName = baseTr.Find("Txt_01").GetComponent<PguiTextCtrl>();
				this.gradientCtrl = baseTr.Find("Txt_01").GetComponent<PguiGradientCtrl>();
				this.Img_Bg = baseTr.GetComponent<Image>();
				this.colorCtrl = baseTr.GetComponent<PguiColorCtrl>();
			}

			// Token: 0x04006208 RID: 25096
			public GameObject baseObj;

			// Token: 0x04006209 RID: 25097
			public PguiTextCtrl Txt_ParamName;

			// Token: 0x0400620A RID: 25098
			public PguiGradientCtrl gradientCtrl;

			// Token: 0x0400620B RID: 25099
			public Image Img_Bg;

			// Token: 0x0400620C RID: 25100
			public PguiColorCtrl colorCtrl;
		}
	}

	// Token: 0x02000E78 RID: 3704
	public class SetupParam
	{
		// Token: 0x06004CC8 RID: 19656 RVA: 0x0022F6E1 File Offset: 0x0022D8E1
		public SetupParam()
		{
			this.windowOpenEndCb = null;
			this.windowCloseEndCb = null;
			this.updateStasusEndCb = null;
			this.acce = null;
			this.acceList = null;
			this.dispMaxAccessoryId = 0;
		}

		// Token: 0x04005344 RID: 21316
		public UnityAction windowOpenEndCb;

		// Token: 0x04005345 RID: 21317
		public UnityAction windowCloseEndCb;

		// Token: 0x04005346 RID: 21318
		public UnityAction updateStasusEndCb;

		// Token: 0x04005347 RID: 21319
		public DataManagerCharaAccessory.Accessory acce;

		// Token: 0x04005348 RID: 21320
		public List<DataManagerCharaAccessory.Accessory> acceList;

		// Token: 0x04005349 RID: 21321
		public int dispMaxAccessoryId;
	}
}
