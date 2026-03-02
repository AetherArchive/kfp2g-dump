using System;
using System.Collections;
using System.Collections.Generic;
using AEAuth3;
using CriWare;
using SGNFW.Common;
using SGNFW.HttpRequest.Protocol;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000139 RID: 313
public class SelMasterSkillCtrl : MonoBehaviour
{
	// Token: 0x1700034D RID: 845
	// (get) Token: 0x060010B2 RID: 4274 RVA: 0x000CB126 File Offset: 0x000C9326
	// (set) Token: 0x060010B3 RID: 4275 RVA: 0x000CB12E File Offset: 0x000C932E
	private int FocusIndex { get; set; }

	// Token: 0x060010B4 RID: 4276 RVA: 0x000CB138 File Offset: 0x000C9338
	public void Initialize()
	{
		GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneCharaEdit/GUI/Prefab/GUI_UserSkillGrow"), base.transform);
		this.ScrollButtonList = new List<GameObject>();
		this.guiData = new SelMasterSkillCtrl.MasterSkillGui(gameObject);
		ReuseScroll scrollView = this.guiData.Left.ScrollView;
		scrollView.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollView.onStartItem, new Action<int, GameObject>(this.OnStartScroll));
		ReuseScroll scrollView2 = this.guiData.Left.ScrollView;
		scrollView2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollView2.onUpdateItem, new Action<int, GameObject>(this.OnUpdateScroll));
		this.guiData.Left.ScrollView.Setup(0, 0);
		this.guiData.Right.MinusButton.AddOnClickListener(delegate(PguiButtonCtrl x)
		{
			this.OnClickMinusButton();
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Right.PlusButton.AddOnClickListener(delegate(PguiButtonCtrl x)
		{
			this.OnClickPlusButton();
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Right.SliderBar.onValueChanged.RemoveAllListeners();
		this.guiData.Right.SliderBar.minValue = 0f;
		this.guiData.Right.SliderBar.maxValue = 100f;
		this.guiData.Right.SliderBar.value = 1f;
		this.guiData.Right.SliderBar.onValueChanged.AddListener(new UnityAction<float>(this.OnSliderValueChanged));
		this.guiData.Right.ExecuteButton.AddOnClickListener(delegate(PguiButtonCtrl x)
		{
			this.OnClickExecuteButton();
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.selectGrowSkillData = new SelMasterSkillCtrl.GrowSkillData();
		GameObject gameObject2 = (GameObject)Resources.Load("SceneCharaEdit/GUI/Prefab/GUI_UserEdit_Window_UserSkillGrow");
		this.masterSkillGrowWindow = new SelMasterSkillCtrl.WindowMasterSkill(Object.Instantiate<Transform>(gameObject2.transform.Find("Window_UserSkillGrow"), gameObject.transform).transform);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this.masterSkillGrowWindow.baseObj.transform, true);
		this.masterSkillGrowWindow.owCtrl.transform.SetParent(Singleton<CanvasManager>.Instance.SystemPanel.transform);
	}

	// Token: 0x060010B5 RID: 4277 RVA: 0x000CB368 File Offset: 0x000C9568
	public void Setup()
	{
		this.UpdateMasterSkill();
		this.FocusIndex = 0;
		this.isGrowing = false;
		this.SetupSelectedInfo();
		this.guiData.Left.ScrollView.Resize(this.MasterSkillPackDataList.Count, 0);
		this.guiData.SelCmn_AllInOut.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
		this.IERequestMasterSkillGrow = null;
	}

	// Token: 0x060010B6 RID: 4278 RVA: 0x000CB3CC File Offset: 0x000C95CC
	private void UpdateMasterSkill()
	{
		Dictionary<int, MasterSkillPackData>.ValueCollection values = DataManager.DmChara.GetUserMasterSkillMap().Values;
		this.MasterSkillPackDataList = new List<MasterSkillPackData>();
		foreach (MasterSkillPackData masterSkillPackData in values)
		{
			this.MasterSkillPackDataList.Add(masterSkillPackData);
		}
		this.MasterSkillPackDataList.Sort((MasterSkillPackData a, MasterSkillPackData b) => a.id - b.id);
		this.LevelItemDataList = DataManager.DmMasterSkill.GetLevelItemDataList();
	}

	// Token: 0x060010B7 RID: 4279 RVA: 0x000CB474 File Offset: 0x000C9674
	private void SetupSelectedInfo()
	{
		MasterSkillPackData masterSkillPackData = this.MasterSkillPackDataList[this.FocusIndex];
		DataManagerMasterSkill.SkillData skillData = DataManager.DmMasterSkill.GetSkillData(masterSkillPackData.id);
		this.guiData.Right.SkillIcon.SetImageByName(masterSkillPackData.staticData.iconName);
		this.guiData.Right.SkillName.text = masterSkillPackData.staticData.skillName;
		this.guiData.Right.LevelTextBefore.gameObject.SetActive(false);
		this.guiData.Right.LevelNumBefore.gameObject.SetActive(false);
		this.guiData.Right.LevelArrow.gameObject.SetActive(false);
		this.guiData.Right.LevelNumAfter.ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[]
		{
			masterSkillPackData.dynamicData.level.ToString(),
			skillData.LevelMax.ToString()
		});
		this.selectGrowSkillData.skillPackData = masterSkillPackData;
		DataManagerMasterSkill.LevelItemData levelItemData = ((0 < this.LevelItemDataList.Count) ? this.LevelItemDataList[0] : null);
		this.selectGrowSkillData.useLvItem = levelItemData;
		int num = ((levelItemData != null) ? this.LevelItemDataList[0].ItemId : 0);
		ItemData userItemData = DataManager.DmItem.GetUserItemData(num);
		this.guiData.Right.ItemIconSetIcon.Setup(userItemData.staticData);
		this.guiData.Right.ItemIconSetHaveCountText.text = userItemData.num.ToString();
		this.guiData.Right.UseItemNameText.text = userItemData.staticData.GetName();
		this.selectGrowSkillData.useItem = new ItemInput(num, 0);
		int num2 = DataManager.DmItem.GetUserItemData(30101).num;
		this.guiData.Right.OwnCoinText.text = num2.ToString();
		DataManagerMasterSkill.NextCalcSkillLevelExp nextCalcSkillLevelExp = DataManager.DmMasterSkill.CalcMasterSkillLevelExp(masterSkillPackData.id, new List<ItemInput>
		{
			new ItemInput(this.selectGrowSkillData.useItem.itemId, 0)
		});
		float num3 = ((0L < nextCalcSkillLevelExp.Denominator) ? ((float)nextCalcSkillLevelExp.Numerator / (float)nextCalcSkillLevelExp.Denominator) : 0f);
		if (nextCalcSkillLevelExp.AfterLevel == skillData.LevelMax)
		{
			num3 = 1f;
		}
		this.guiData.Right.ExpGageNow.fillAmount = num3;
		int num4 = ((0 < num2) ? (num2 / this.selectGrowSkillData.useLvItem.CoinNum) : 0);
		List<DataManagerMasterSkill.LevelData> levenDataList = DataManager.DmMasterSkill.GetLevenDataList(skillData.LevelId);
		long num5 = 0L;
		DataManagerMasterSkill.MasterSkillData masterSkillData = DataManager.DmMasterSkill.UserMasterSkillDataList.Find((DataManagerMasterSkill.MasterSkillData x) => x.SkillId == this.selectGrowSkillData.skillPackData.id);
		foreach (DataManagerMasterSkill.LevelData levelData in levenDataList)
		{
			if (num5 < levelData.Exp)
			{
				num5 = levelData.Exp;
			}
		}
		long num6 = num5 - masterSkillData.Exp;
		long num7 = num6 / this.selectGrowSkillData.useLvItem.Exp + ((0L < num6 % this.selectGrowSkillData.useLvItem.Exp) ? 1L : 0L);
		int num8 = DataManager.DmItem.GetUserItemData(this.selectGrowSkillData.useLvItem.ItemId).num;
		int num9 = Mathf.Min(num4, (int)num7);
		num9 = Mathf.Min(num9, num8);
		this.guiData.Right.SliderBar.maxValue = (float)num9;
		this.guiData.Right.SliderBar.value = 0f;
		bool flag = masterSkillPackData.dynamicData.level == skillData.LevelMax;
		this.guiData.Right.InfoMax.SetActive(flag);
		this.guiData.Right.ExpInfo.SetActive(!flag);
		this.guiData.Right.ItemIconSet.SetActive(!flag);
		this.guiData.Right.UseItemNameText.gameObject.SetActive(!flag);
		this.guiData.Right.Exchange.SetActive(!flag);
		this.UpdateNextLevelInfo();
	}

	// Token: 0x060010B8 RID: 4280 RVA: 0x000CB8F8 File Offset: 0x000C9AF8
	private void UpdateNextLevelInfo()
	{
		MasterSkillPackData masterSkillPackData = this.MasterSkillPackDataList[this.FocusIndex];
		DataManagerMasterSkill.SkillData skillData = DataManager.DmMasterSkill.GetSkillData(masterSkillPackData.id);
		DataManagerMasterSkill.NextCalcSkillLevelExp nextCalcSkillLevelExp = DataManager.DmMasterSkill.CalcMasterSkillLevelExp(masterSkillPackData.id, new List<ItemInput>
		{
			new ItemInput(this.selectGrowSkillData.useItem.itemId, this.selectGrowSkillData.useItem.num)
		});
		if (0L < nextCalcSkillLevelExp.AddExp)
		{
			this.guiData.Right.SkillInfo.text = masterSkillPackData.staticData.MakeSkillText(nextCalcSkillLevelExp.AfterLevel, masterSkillPackData.dynamicData.level != nextCalcSkillLevelExp.AfterLevel);
			this.guiData.Right.LevelTextBefore.gameObject.SetActive(true);
			this.guiData.Right.LevelNumBefore.gameObject.SetActive(true);
			this.guiData.Right.LevelNumBefore.ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[]
			{
				masterSkillPackData.dynamicData.level.ToString(),
				skillData.LevelMax.ToString()
			});
			this.guiData.Right.LevelArrow.gameObject.SetActive(true);
			string text = nextCalcSkillLevelExp.AfterLevel.ToString();
			if (masterSkillPackData.dynamicData.level != nextCalcSkillLevelExp.AfterLevel)
			{
				text = "<color=#FF7B16FF>" + text + "</color>";
			}
			this.guiData.Right.LevelNumAfter.ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[]
			{
				text,
				skillData.LevelMax.ToString()
			});
		}
		else
		{
			this.guiData.Right.SkillInfo.text = masterSkillPackData.staticData.MakeSkillText(masterSkillPackData.dynamicData.level, false);
			this.guiData.Right.LevelTextBefore.gameObject.SetActive(false);
			this.guiData.Right.LevelNumBefore.gameObject.SetActive(false);
			this.guiData.Right.LevelArrow.gameObject.SetActive(false);
			this.guiData.Right.LevelNumAfter.ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[]
			{
				masterSkillPackData.dynamicData.level.ToString(),
				skillData.LevelMax.ToString()
			});
		}
		float num = ((0L < nextCalcSkillLevelExp.Denominator) ? ((float)nextCalcSkillLevelExp.Numerator / (float)nextCalcSkillLevelExp.Denominator) : 0f);
		if (nextCalcSkillLevelExp.AfterLevel == skillData.LevelMax)
		{
			num = 1f;
		}
		bool flag = 0 < this.selectGrowSkillData.useItem.num;
		this.guiData.Right.ExpGageNow.gameObject.SetActive(!flag);
		this.guiData.Right.ExpGageUp.gameObject.SetActive(flag);
		this.guiData.Right.ExpGageUp.fillAmount = num;
		this.guiData.Right.ExpNextNum.text = "次のLvまで" + nextCalcSkillLevelExp.RequiredExp.ToString();
		this.guiData.Right.ItemIconSetUseCountObj.SetActive(flag);
		this.guiData.Right.ItemIconSetUseCountText.text = this.selectGrowSkillData.useItem.num.ToString();
		this.guiData.Right.SliderBar.value = (float)this.selectGrowSkillData.useItem.num;
		bool flag2 = this.guiData.Right.SliderBar.minValue == (float)this.selectGrowSkillData.useItem.num;
		this.guiData.Right.MinusButton.SetActEnable(!flag2, false, false);
		bool flag3 = this.guiData.Right.SliderBar.maxValue == (float)this.selectGrowSkillData.useItem.num;
		this.guiData.Right.PlusButton.SetActEnable(!flag3, false, false);
		int num2 = this.selectGrowSkillData.useLvItem.CoinNum * this.selectGrowSkillData.useItem.num;
		this.guiData.Right.UseCoinText.text = ((num2 == 0) ? "-" : num2.ToString());
		this.guiData.Right.ExecuteButton.SetActEnable(flag, false, false);
	}

	// Token: 0x060010B9 RID: 4281 RVA: 0x000CBDD0 File Offset: 0x000C9FD0
	private void OnClickMinusButton()
	{
		this.selectGrowSkillData.useItem.num--;
		if (this.selectGrowSkillData.useItem.num < 0)
		{
			this.selectGrowSkillData.useItem.num = 0;
		}
		this.UpdateNextLevelInfo();
	}

	// Token: 0x060010BA RID: 4282 RVA: 0x000CBE1F File Offset: 0x000CA01F
	private void OnClickPlusButton()
	{
		this.selectGrowSkillData.useItem.num++;
		this.UpdateNextLevelInfo();
	}

	// Token: 0x060010BB RID: 4283 RVA: 0x000CBE3F File Offset: 0x000CA03F
	private void OnSliderValueChanged(float value)
	{
		this.selectGrowSkillData.useItem.num = int.Parse(value.ToString());
		this.UpdateNextLevelInfo();
	}

	// Token: 0x060010BC RID: 4284 RVA: 0x000CBE63 File Offset: 0x000CA063
	private void OnStartScroll(int index, GameObject go)
	{
		this.ScrollButtonList.Add(go);
	}

	// Token: 0x060010BD RID: 4285 RVA: 0x000CBE74 File Offset: 0x000CA074
	private void OnUpdateScroll(int index, GameObject go)
	{
		MasterSkillPackData masterSkillPackData = this.MasterSkillPackDataList[index];
		DataManagerMasterSkill.SkillData skillData = DataManager.DmMasterSkill.GetSkillData(masterSkillPackData.id);
		SelMasterSkillCtrl.SkillGrowBarGui skillGrowBarGui = new SelMasterSkillCtrl.SkillGrowBarGui(go);
		skillGrowBarGui.BaseButton.AddOnClickListener(delegate(PguiButtonCtrl x)
		{
			this.OnClickSkillSelectButton(index, go);
		}, PguiButtonCtrl.SoundType.DEFAULT);
		skillGrowBarGui.SkillName.text = masterSkillPackData.staticData.skillName;
		skillGrowBarGui.LevelText.ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[]
		{
			masterSkillPackData.dynamicData.level.ToString(),
			skillData.LevelMax.ToString()
		});
		skillGrowBarGui.CurrentFrame.SetActive(this.FocusIndex == index);
		skillGrowBarGui.SkillMiniIcon.SetImageByName(DataManager.DmChara.GetMasterSkillStaticData(masterSkillPackData.id).iconMiniName);
	}

	// Token: 0x060010BE RID: 4286 RVA: 0x000CBF80 File Offset: 0x000CA180
	private void OnClickSkillSelectButton(int index, GameObject go)
	{
		foreach (GameObject gameObject in this.ScrollButtonList)
		{
			new SelMasterSkillCtrl.SkillGrowBarGui(gameObject).CurrentFrame.SetActive(false);
		}
		new SelMasterSkillCtrl.SkillGrowBarGui(go).CurrentFrame.SetActive(true);
		this.FocusIndex = index;
		this.SetupSelectedInfo();
	}

	// Token: 0x060010BF RID: 4287 RVA: 0x000CBFFC File Offset: 0x000CA1FC
	private void OnClickExecuteButton()
	{
		if (this.isGrowing)
		{
			return;
		}
		this.masterSkillGrowWindow.Setup(this.selectGrowSkillData);
		this.masterSkillGrowWindow.owCtrl.RegistCallback(delegate(int index)
		{
			if (index != 1)
			{
				return true;
			}
			if (!this.isGrowing)
			{
				this.isGrowing = true;
				this.IERequestMasterSkillGrow = this.RequestMasterSkilGrow();
				return false;
			}
			return true;
		});
		this.masterSkillGrowWindow.owCtrl.Open();
	}

	// Token: 0x060010C0 RID: 4288 RVA: 0x000CC04F File Offset: 0x000CA24F
	private IEnumerator RequestMasterSkilGrow()
	{
		CanvasManager.SetEnableCmnTouchMask(true);
		UseItem useItem = new UseItem
		{
			use_item_id = this.selectGrowSkillData.useItem.itemId,
			use_item_num = this.selectGrowSkillData.useItem.num
		};
		DataManagerMasterSkill.SkillData skillData = DataManager.DmMasterSkill.GetSkillData(this.selectGrowSkillData.skillPackData.id);
		int beforeSkillLevel = this.selectGrowSkillData.skillPackData.dynamicData.level;
		float beforeGageNow = this.guiData.Right.ExpGageNow.fillAmount;
		DataManager.DmMasterSkill.RequestActionMasterSkillGrow(this.selectGrowSkillData.skillPackData.id, new List<UseItem> { useItem });
		this.masterSkillGrowWindow.owCtrl.ForceClose();
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		yield return null;
		DataManagerMasterSkill.NextCalcSkillLevelExp nextCalcSkillLevelExp = DataManager.DmMasterSkill.CalcMasterSkillLevelExp(this.selectGrowSkillData.skillPackData.id, new List<ItemInput>
		{
			new ItemInput(this.selectGrowSkillData.useItem.itemId, 0)
		});
		int afterSkillLevel = this.selectGrowSkillData.skillPackData.dynamicData.level;
		float afterGageNow = ((0L < nextCalcSkillLevelExp.Denominator) ? ((float)nextCalcSkillLevelExp.Numerator / (float)nextCalcSkillLevelExp.Denominator) : 1f);
		int dispSkillLevel = beforeSkillLevel;
		float dispGageNow = beforeGageNow;
		this.guiData.Right.ExpGageUp.gameObject.SetActive(false);
		this.guiData.Right.ExpGageNow.gameObject.SetActive(true);
		this.guiData.Right.ExpGageUp.fillAmount = 0f;
		this.guiData.Right.ExpGageNow.fillAmount = beforeGageNow;
		this.guiData.Right.LevelTextBefore.gameObject.SetActive(false);
		this.guiData.Right.LevelNumBefore.gameObject.SetActive(false);
		this.guiData.Right.LevelArrow.gameObject.SetActive(false);
		this.guiData.Right.LevelNumAfter.ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[]
		{
			dispSkillLevel.ToString(),
			skillData.LevelMax.ToString()
		});
		CriAtomExPlayback gaugeSE = SoundManager.Play("prd_se_friends_levelup_gauge", true, false);
		bool isLoop = true;
		while (isLoop)
		{
			if (afterSkillLevel <= dispSkillLevel && afterGageNow == dispGageNow)
			{
				isLoop = false;
				break;
			}
			if (1f == dispGageNow)
			{
				int num = dispSkillLevel + 1;
				dispSkillLevel = num;
				dispGageNow = 0f;
				this.guiData.Right.LevelNumAfter.ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[]
				{
					dispSkillLevel.ToString(),
					skillData.LevelMax.ToString()
				});
				this.guiData.Right.ExpGageNow.fillAmount = 0f;
				this.guiData.Right.AEImage_LvUp.gameObject.SetActive(true);
				this.guiData.Right.AEImage_LvUp.playTime = 0f;
				this.guiData.Right.AEImage_LvUp.autoPlay = true;
				this.guiData.Right.AEImage_LvUp.playLoop = false;
				SoundManager.Play("prd_se_friends_levelup_font", false, false);
			}
			dispGageNow += 0.1f;
			if (1f < dispGageNow)
			{
				dispGageNow = 1f;
			}
			if (afterSkillLevel <= dispSkillLevel && afterGageNow < dispGageNow)
			{
				dispGageNow = afterGageNow;
			}
			this.guiData.Right.ExpGageNow.fillAmount = dispGageNow;
			yield return null;
		}
		gaugeSE.Stop();
		this.UpdateMasterSkill();
		this.guiData.Left.ScrollView.Refresh();
		this.SetupSelectedInfo();
		CanvasManager.SetEnableCmnTouchMask(false);
		this.isGrowing = false;
		yield break;
	}

	// Token: 0x060010C1 RID: 4289 RVA: 0x000CC060 File Offset: 0x000CA260
	public void OnClickMenuReturn(UnityAction callback = null)
	{
		this.guiData.SelCmn_AllInOut.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
		{
			UnityAction callback2 = callback;
			if (callback2 == null)
			{
				return;
			}
			callback2();
		});
	}

	// Token: 0x060010C2 RID: 4290 RVA: 0x000CC097 File Offset: 0x000CA297
	private void Update()
	{
		if (this.IERequestMasterSkillGrow != null && !this.IERequestMasterSkillGrow.MoveNext())
		{
			this.IERequestMasterSkillGrow = null;
		}
	}

	// Token: 0x04000E66 RID: 3686
	public SelMasterSkillCtrl.MasterSkillGui guiData;

	// Token: 0x04000E67 RID: 3687
	public SelMasterSkillCtrl.WindowMasterSkill masterSkillGrowWindow;

	// Token: 0x04000E68 RID: 3688
	private List<MasterSkillPackData> MasterSkillPackDataList;

	// Token: 0x04000E69 RID: 3689
	private List<DataManagerMasterSkill.LevelItemData> LevelItemDataList;

	// Token: 0x04000E6A RID: 3690
	private List<GameObject> ScrollButtonList;

	// Token: 0x04000E6B RID: 3691
	private SelMasterSkillCtrl.GrowSkillData selectGrowSkillData;

	// Token: 0x04000E6D RID: 3693
	private IEnumerator IERequestMasterSkillGrow;

	// Token: 0x04000E6E RID: 3694
	private bool isGrowing;

	// Token: 0x02000A3E RID: 2622
	public class MasterSkillGui
	{
		// Token: 0x06003E9F RID: 16031 RVA: 0x001EAD88 File Offset: 0x001E8F88
		public MasterSkillGui(GameObject mainObj)
		{
			this.SelCmn_AllInOut = mainObj.transform.Find("UserSkill_Main").GetComponent<SimpleAnimation>();
			Transform transform = mainObj.transform.Find("UserSkill_Main/All/Left");
			this.Left = new SelMasterSkillCtrl.MasterSkillGui.LeftObj(transform);
			Transform transform2 = mainObj.transform.Find("UserSkill_Main/All/Right");
			this.Right = new SelMasterSkillCtrl.MasterSkillGui.RightObj(transform2);
		}

		// Token: 0x04004170 RID: 16752
		public SimpleAnimation SelCmn_AllInOut;

		// Token: 0x04004171 RID: 16753
		public SelMasterSkillCtrl.MasterSkillGui.LeftObj Left;

		// Token: 0x04004172 RID: 16754
		public SelMasterSkillCtrl.MasterSkillGui.RightObj Right;

		// Token: 0x0200116C RID: 4460
		public class LeftObj
		{
			// Token: 0x17000CDB RID: 3291
			// (get) Token: 0x06005620 RID: 22048 RVA: 0x00250C06 File Offset: 0x0024EE06
			// (set) Token: 0x06005621 RID: 22049 RVA: 0x00250C0E File Offset: 0x0024EE0E
			public ReuseScroll ScrollView { get; set; }

			// Token: 0x06005622 RID: 22050 RVA: 0x00250C17 File Offset: 0x0024EE17
			public LeftObj(Transform lTr)
			{
				this.ScrollView = lTr.Find("ScrollView").gameObject.GetComponent<ReuseScroll>();
			}
		}

		// Token: 0x0200116D RID: 4461
		public class RightObj
		{
			// Token: 0x06005623 RID: 22051 RVA: 0x00250C3C File Offset: 0x0024EE3C
			public RightObj(Transform rTr)
			{
				this.SkillIcon = rTr.Find("SkillInfo/Icon_PlayerSkill").GetComponent<PguiImageCtrl>();
				this.SkillName = rTr.Find("SkillInfo/Txt_UserSkill").GetComponent<Text>();
				this.SkillInfo = rTr.Find("SkillInfo/Txt_SkillInfo").GetComponent<Text>();
				this.LevelTextBefore = rTr.Find("ExpInfo/Txt_Lv_Before").GetComponent<PguiTextCtrl>();
				this.LevelNumBefore = rTr.Find("ExpInfo/Num_Lv_L").GetComponent<PguiTextCtrl>();
				this.LevelArrow = rTr.Find("ExpInfo/Img_Yaji").GetComponent<PguiImageCtrl>();
				this.LevelNumBefore.ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[] { "99", "99" });
				this.LevelTextAfter = rTr.Find("ExpInfo/Txt_Lv_After").GetComponent<PguiTextCtrl>();
				this.LevelNumAfter = rTr.Find("ExpInfo/Num_Lv_R").GetComponent<PguiTextCtrl>();
				this.LevelNumAfter.ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[] { "99", "99" });
				this.ExpGageUp = rTr.Find("ExpInfo/ExpGage/Gage_Up").GetComponent<Image>();
				this.ExpGageNow = rTr.Find("ExpInfo/ExpGage/Gage").GetComponent<Image>();
				this.ExpNextNum = rTr.Find("ExpInfo/Num_Exp_Next").GetComponent<Text>();
				this.AEImage_ExpUp = rTr.Find("ExpInfo/AEImage_ExpUp").GetComponent<AEImage>();
				this.AEImage_ExpUp.gameObject.SetActive(false);
				this.AEImage_LvUp = rTr.Find("ExpInfo/AEImage_LevelUP").GetComponent<AEImage>();
				this.AEImage_LvUp.gameObject.SetActive(false);
				this.InfoMax = rTr.Find("Info_Max").gameObject;
				this.ExpInfo = rTr.Find("ExpInfo").gameObject;
				this.ItemIconSet = rTr.Find("CharaGrow_ItemIconSet").gameObject;
				this.Exchange = rTr.Find("Exchange").gameObject;
				this.UseItemNameText = rTr.Find("Txt01").GetComponent<Text>();
				GameObject gameObject = this.ItemIconSet.transform.Find("Icon_Item").gameObject;
				this.ItemIconSetIcon = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, gameObject.transform).GetComponent<IconItemCtrl>();
				this.ItemIconSetUseCountObj = this.ItemIconSet.transform.Find("Count").gameObject;
				this.ItemIconSetUseCountText = this.ItemIconSet.transform.Find("Count/Num_Count").GetComponent<Text>();
				this.ItemIconSetHaveCountText = this.ItemIconSet.transform.Find("Num_Own").GetComponent<Text>();
				this.MinusButton = rTr.Find("Exchange/Btn_Minus").GetComponent<PguiButtonCtrl>();
				this.PlusButton = rTr.Find("Exchange/Btn_Plus").GetComponent<PguiButtonCtrl>();
				this.SliderBar = rTr.Find("Exchange/SliderBar").GetComponent<Slider>();
				this.UseCoinText = rTr.Find("UseCoin/Num").GetComponent<Text>();
				this.OwnCoinText = rTr.Find("OwnCoin/Num").GetComponent<Text>();
				this.ExecuteButton = rTr.Find("ButtonC").GetComponent<PguiButtonCtrl>();
			}

			// Token: 0x04005F97 RID: 24471
			public PguiImageCtrl SkillIcon;

			// Token: 0x04005F98 RID: 24472
			public Text SkillName;

			// Token: 0x04005F99 RID: 24473
			public Text SkillInfo;

			// Token: 0x04005F9A RID: 24474
			public PguiTextCtrl LevelTextBefore;

			// Token: 0x04005F9B RID: 24475
			public PguiTextCtrl LevelNumBefore;

			// Token: 0x04005F9C RID: 24476
			public PguiImageCtrl LevelArrow;

			// Token: 0x04005F9D RID: 24477
			public PguiTextCtrl LevelTextAfter;

			// Token: 0x04005F9E RID: 24478
			public PguiTextCtrl LevelNumAfter;

			// Token: 0x04005F9F RID: 24479
			public Image ExpGageUp;

			// Token: 0x04005FA0 RID: 24480
			public Image ExpGageNow;

			// Token: 0x04005FA1 RID: 24481
			public Text ExpNextNum;

			// Token: 0x04005FA2 RID: 24482
			public AEImage AEImage_ExpUp;

			// Token: 0x04005FA3 RID: 24483
			public AEImage AEImage_LvUp;

			// Token: 0x04005FA4 RID: 24484
			public GameObject InfoMax;

			// Token: 0x04005FA5 RID: 24485
			public GameObject ExpInfo;

			// Token: 0x04005FA6 RID: 24486
			public GameObject Exchange;

			// Token: 0x04005FA7 RID: 24487
			public GameObject ItemIconSet;

			// Token: 0x04005FA8 RID: 24488
			public IconItemCtrl ItemIconSetIcon;

			// Token: 0x04005FA9 RID: 24489
			public GameObject ItemIconSetUseCountObj;

			// Token: 0x04005FAA RID: 24490
			public Text ItemIconSetUseCountText;

			// Token: 0x04005FAB RID: 24491
			public Text ItemIconSetHaveCountText;

			// Token: 0x04005FAC RID: 24492
			public Text UseItemNameText;

			// Token: 0x04005FAD RID: 24493
			public PguiButtonCtrl MinusButton;

			// Token: 0x04005FAE RID: 24494
			public PguiButtonCtrl PlusButton;

			// Token: 0x04005FAF RID: 24495
			public Slider SliderBar;

			// Token: 0x04005FB0 RID: 24496
			public Text UseCoinText;

			// Token: 0x04005FB1 RID: 24497
			public Text OwnCoinText;

			// Token: 0x04005FB2 RID: 24498
			public PguiButtonCtrl ExecuteButton;
		}
	}

	// Token: 0x02000A3F RID: 2623
	public class SkillGrowBarGui
	{
		// Token: 0x06003EA0 RID: 16032 RVA: 0x001EADF0 File Offset: 0x001E8FF0
		public SkillGrowBarGui(GameObject barObj)
		{
			this.BaseButton = barObj.GetComponent<PguiButtonCtrl>();
			this.SkillMiniIcon = barObj.transform.Find("BaseImage/Icon_Skill").GetComponent<PguiImageCtrl>();
			this.SkillName = barObj.transform.Find("BaseImage/Txt_Skill").GetComponent<Text>();
			this.LevelText = barObj.transform.Find("BaseImage/Num_Lv").GetComponent<PguiTextCtrl>();
			this.CurrentFrame = barObj.transform.Find("BaseImage/Current").gameObject;
		}

		// Token: 0x04004173 RID: 16755
		public PguiButtonCtrl BaseButton;

		// Token: 0x04004174 RID: 16756
		public PguiImageCtrl SkillMiniIcon;

		// Token: 0x04004175 RID: 16757
		public Text SkillName;

		// Token: 0x04004176 RID: 16758
		public PguiTextCtrl LevelText;

		// Token: 0x04004177 RID: 16759
		public GameObject CurrentFrame;
	}

	// Token: 0x02000A40 RID: 2624
	public class WindowMasterSkill
	{
		// Token: 0x06003EA1 RID: 16033 RVA: 0x001EAE7C File Offset: 0x001E907C
		public WindowMasterSkill(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.owCtrl.Setup(string.Empty, string.Empty, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, null, null, false);
			this.closeButton = baseTr.Find("Base/Window/BtnClose").GetComponent<PguiButtonCtrl>();
			this.SkillIcon = baseTr.Find("Base/Window/Icon_UserSkill").GetComponent<PguiImageCtrl>();
			this.SkillName = baseTr.Find("Base/Window/Txt_SkillName").GetComponent<PguiTextCtrl>();
			this.SkillInfoBefore = baseTr.Find("Base/Window/UserSkillInfo_Before/Txt_SkillInfo").GetComponent<PguiTextCtrl>();
			this.SkillInfoAfter = baseTr.Find("Base/Window/UserSkillInfo_After/Txt_SkillInfo").GetComponent<PguiTextCtrl>();
			this.LevelNumBefore = baseTr.Find("Base/Window/ExpInfo/Num_Lv_Before").GetComponent<PguiTextCtrl>();
			this.LevelNumAfter = baseTr.Find("Base/Window/ExpInfo/Num_Lv_After").GetComponent<PguiTextCtrl>();
			this.ExpGageUp = baseTr.Find("Base/Window/ExpInfo/ExpGage/Gage_Up").GetComponent<Image>();
			this.ExpGageNow = baseTr.Find("Base/Window/ExpInfo/ExpGage/Gage").GetComponent<Image>();
			this.ExpNextNum = baseTr.Find("Base/Window/ExpInfo/Num_Exp_Next").GetComponent<PguiTextCtrl>();
			this.UseItemIcon = baseTr.Find("Base/Window/ItemInfo/Icon_Tex").GetComponent<PguiRawImageCtrl>();
			this.UseItemName = baseTr.Find("Base/Window/ItemInfo/Txt01").GetComponent<PguiTextCtrl>();
			this.UseItemBeforeNum = baseTr.Find("Base/Window/ItemInfo/Num_BeforeTxt").GetComponent<PguiTextCtrl>();
			this.UseItemAfterNum = baseTr.Find("Base/Window/ItemInfo/Num_AfterTxt").GetComponent<PguiTextCtrl>();
			this.UseGoldBeforeNum = baseTr.Find("Base/Window/UseCoin/Num_BeforeTxt").GetComponent<PguiTextCtrl>();
			this.UseGoldAfterNum = baseTr.Find("Base/Window/UseCoin/Num_AfterTxt").GetComponent<PguiTextCtrl>();
		}

		// Token: 0x06003EA2 RID: 16034 RVA: 0x001EB028 File Offset: 0x001E9228
		public void Setup(SelMasterSkillCtrl.GrowSkillData settingsIn)
		{
			this.openSettings = settingsIn;
			MasterSkillPackData skillPackData = this.openSettings.skillPackData;
			DataManagerMasterSkill.SkillData skillData = DataManager.DmMasterSkill.GetSkillData(this.openSettings.skillPackData.id);
			DataManagerMasterSkill.NextCalcSkillLevelExp nextCalcSkillLevelExp = DataManager.DmMasterSkill.CalcMasterSkillLevelExp(skillPackData.id, new List<ItemInput> { this.openSettings.useItem });
			this.SkillIcon.SetImageByName(DataManager.DmChara.GetMasterSkillStaticData(skillPackData.id).iconName);
			this.SkillName.text = skillPackData.staticData.skillName;
			this.SkillInfoBefore.text = skillPackData.staticData.MakeSkillText(skillPackData.dynamicData.level, false);
			this.SkillInfoAfter.text = skillPackData.staticData.MakeSkillText(nextCalcSkillLevelExp.AfterLevel, skillPackData.dynamicData.level != nextCalcSkillLevelExp.AfterLevel);
			this.ExpNextNum.text = "次のLvまで" + nextCalcSkillLevelExp.RequiredExp.ToString();
			float num = ((0L < nextCalcSkillLevelExp.Denominator) ? ((float)nextCalcSkillLevelExp.Numerator / (float)nextCalcSkillLevelExp.Denominator) : 0f);
			if (nextCalcSkillLevelExp.AfterLevel == skillData.LevelMax)
			{
				num = 1f;
			}
			if (0 < this.openSettings.useItem.num)
			{
				this.ExpGageNow.gameObject.SetActive(false);
				this.ExpGageUp.gameObject.SetActive(true);
				this.ExpGageUp.fillAmount = num;
			}
			else
			{
				this.ExpGageNow.gameObject.SetActive(true);
				this.ExpGageUp.gameObject.SetActive(false);
				this.ExpGageNow.fillAmount = num;
			}
			this.LevelNumBefore.ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[]
			{
				skillPackData.dynamicData.level.ToString(),
				skillData.LevelMax.ToString()
			});
			string text = nextCalcSkillLevelExp.AfterLevel.ToString();
			if (skillPackData.dynamicData.level != nextCalcSkillLevelExp.AfterLevel)
			{
				text = "<color=#FF7B16FF>" + text + "</color>";
			}
			this.LevelNumAfter.ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[]
			{
				text,
				skillData.LevelMax.ToString()
			});
			ItemData userItemData = DataManager.DmItem.GetUserItemData(this.openSettings.useItem.itemId);
			this.UseItemIcon.SetRawImage(userItemData.staticData.GetIconName(), true, false, null);
			this.UseItemName.text = userItemData.staticData.GetName();
			this.UseItemBeforeNum.text = userItemData.num.ToString();
			this.UseItemAfterNum.text = (userItemData.num - this.openSettings.useItem.num).ToString();
			ItemData userItemData2 = DataManager.DmItem.GetUserItemData(30101);
			this.UseGoldBeforeNum.text = userItemData2.num.ToString();
			this.UseGoldAfterNum.text = (userItemData2.num - this.openSettings.useLvItem.CoinNum * this.openSettings.useItem.num).ToString();
		}

		// Token: 0x04004178 RID: 16760
		public GameObject baseObj;

		// Token: 0x04004179 RID: 16761
		public PguiOpenWindowCtrl owCtrl;

		// Token: 0x0400417A RID: 16762
		public PguiButtonCtrl closeButton;

		// Token: 0x0400417B RID: 16763
		private SelMasterSkillCtrl.GrowSkillData openSettings;

		// Token: 0x0400417C RID: 16764
		public PguiImageCtrl SkillIcon;

		// Token: 0x0400417D RID: 16765
		public PguiTextCtrl SkillName;

		// Token: 0x0400417E RID: 16766
		public PguiTextCtrl SkillInfoBefore;

		// Token: 0x0400417F RID: 16767
		public PguiTextCtrl SkillInfoAfter;

		// Token: 0x04004180 RID: 16768
		public PguiTextCtrl LevelNumBefore;

		// Token: 0x04004181 RID: 16769
		public PguiTextCtrl LevelNumAfter;

		// Token: 0x04004182 RID: 16770
		public Image ExpGageUp;

		// Token: 0x04004183 RID: 16771
		public Image ExpGageNow;

		// Token: 0x04004184 RID: 16772
		public PguiTextCtrl ExpNextNum;

		// Token: 0x04004185 RID: 16773
		public PguiRawImageCtrl UseItemIcon;

		// Token: 0x04004186 RID: 16774
		public PguiTextCtrl UseItemName;

		// Token: 0x04004187 RID: 16775
		public PguiTextCtrl UseItemBeforeNum;

		// Token: 0x04004188 RID: 16776
		public PguiTextCtrl UseItemAfterNum;

		// Token: 0x04004189 RID: 16777
		public PguiTextCtrl UseGoldBeforeNum;

		// Token: 0x0400418A RID: 16778
		public PguiTextCtrl UseGoldAfterNum;
	}

	// Token: 0x02000A41 RID: 2625
	public class GrowSkillData
	{
		// Token: 0x0400418B RID: 16779
		public MasterSkillPackData skillPackData;

		// Token: 0x0400418C RID: 16780
		public ItemInput useItem;

		// Token: 0x0400418D RID: 16781
		public DataManagerMasterSkill.LevelItemData useLvItem;
	}
}
