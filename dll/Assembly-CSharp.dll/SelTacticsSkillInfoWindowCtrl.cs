using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001C3 RID: 451
public class SelTacticsSkillInfoWindowCtrl : MonoBehaviour
{
	// Token: 0x06001EF3 RID: 7923 RVA: 0x00180498 File Offset: 0x0017E698
	public void Initialize(GameObject baseWindow)
	{
		this.guiData = new SelTacticsSkillInfoWindowCtrl.GUI(baseWindow.transform);
		this.guiData.Btn_SkillChange.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnclickSkillChangeButton), PguiButtonCtrl.SoundType.DECIDE);
		foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl in this.guiData.tacticsTermsTypeButtonList)
		{
			pguiToggleButtonCtrl.AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickTacticsTermsTypeButton));
		}
		foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl2 in this.guiData.tacticsTermsValueIdButtonList)
		{
			pguiToggleButtonCtrl2.AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickTacticsTermsValueButton));
		}
	}

	// Token: 0x06001EF4 RID: 7924 RVA: 0x00180578 File Offset: 0x0017E778
	public void Open(SelTacticsSkillInfoWindowCtrl.SetupParam setupParam)
	{
		this.windowOpenEndCb = setupParam.openEndCb;
		this.windowCloseEndCb = setupParam.closeEndCb;
		this.currentPvpTacticsTypeId = setupParam.pvpTacticsTypeId;
		this.currentPvpTacticsTermsTypeId = setupParam.pvpTacticsTermsTypeId;
		this.currentPvpTacticsTermsValueId = setupParam.pvpTacticsTermsValueId;
		TacticsStaticSkill tacticsSkillStaticData = DataManager.DmChara.GetTacticsSkillStaticData(this.currentPvpTacticsTypeId);
		if (null != tacticsSkillStaticData)
		{
			this.guiData.Icon_TacticsSkill.SetSprite(this.currentPvpTacticsTypeId);
			this.guiData.Txt_SkillName.text = tacticsSkillStaticData.skillName;
			this.guiData.Txt_SkillInfo.text = tacticsSkillStaticData.skillInfo;
		}
		List<TacticsParam.Tactics> tacticsParamData = DataManager.DmChara.GetTacticsParamData();
		this.UpdateTacticsTermsTypeButton(tacticsParamData);
		if (TacticsStaticSkill.Type.ADD_ACTION_DAMAGE == tacticsSkillStaticData.type || TacticsStaticSkill.Type.ADD_BEAT_DAMAGE == tacticsSkillStaticData.type || TacticsStaticSkill.Type.ADD_TRY_DAMAGE == tacticsSkillStaticData.type)
		{
			for (int i = 0; i < tacticsParamData.Count; i++)
			{
				if (TacticsParam.Tactics.Type.TURN_NUM != tacticsParamData[i].type)
				{
					this.guiData.tacticsTermsTypeButtonList[i].SetActEnable(false);
				}
			}
		}
		TacticsParam.Tactics tacticsParamData2 = DataManager.DmChara.GetTacticsParamData((TacticsParam.Tactics.Type)this.currentPvpTacticsTermsTypeId);
		this.UpdateTacticsTermsValueButton(tacticsParamData2);
		this.guiData.owCtrl.Setup("作戦設定", null, null, true, new PguiOpenWindowCtrl.Callback(this.OnClickOwButton), null, false);
		this.guiData.owCtrl.Open();
		UnityAction unityAction = this.windowOpenEndCb;
		if (unityAction != null)
		{
			unityAction();
		}
		this.windowOpenEndCb = null;
	}

	// Token: 0x06001EF5 RID: 7925 RVA: 0x001806E9 File Offset: 0x0017E8E9
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

	// Token: 0x06001EF6 RID: 7926 RVA: 0x0018070A File Offset: 0x0017E90A
	private void OnclickSkillChangeButton(PguiButtonCtrl button)
	{
		CanvasManager.HdlOpenWindowTacticsSkillChange.Open(new SelTacticsSkillChangeWindowCtrl.SetupParam
		{
			openEndCb = null,
			closeEndCb = new UnityAction(this.OnCloseSkillChangeWindow),
			pvpTacticsTypeId = this.currentPvpTacticsTypeId
		});
	}

	// Token: 0x06001EF7 RID: 7927 RVA: 0x00180740 File Offset: 0x0017E940
	private bool OnClickTacticsTermsTypeButton(PguiToggleButtonCtrl button, int index)
	{
		if (index == 0)
		{
			for (int i = 0; i < this.guiData.tacticsTermsTypeButtonList.Count; i++)
			{
				if (this.guiData.tacticsTermsTypeButtonList[i].gameObject.activeSelf)
				{
					if (button != this.guiData.tacticsTermsTypeButtonList[i])
					{
						this.guiData.tacticsTermsTypeButtonList[i].SetToggleIndex(0);
					}
					else
					{
						this.currentPvpTacticsTermsTypeId = i + 1;
						TacticsParam.Tactics tacticsParamData = DataManager.DmChara.GetTacticsParamData((TacticsParam.Tactics.Type)this.currentPvpTacticsTermsTypeId);
						this.currentPvpTacticsTermsValueId = 1;
						this.UpdateTacticsTermsValueButton(tacticsParamData);
					}
				}
			}
			return true;
		}
		return false;
	}

	// Token: 0x06001EF8 RID: 7928 RVA: 0x001807EC File Offset: 0x0017E9EC
	private bool OnClickTacticsTermsValueButton(PguiToggleButtonCtrl button, int index)
	{
		if (index == 0)
		{
			for (int i = 0; i < this.guiData.tacticsTermsValueIdButtonList.Count; i++)
			{
				if (this.guiData.tacticsTermsValueIdButtonList[i].gameObject.activeSelf)
				{
					if (button != this.guiData.tacticsTermsValueIdButtonList[i])
					{
						this.guiData.tacticsTermsValueIdButtonList[i].SetToggleIndex(0);
					}
					else
					{
						this.currentPvpTacticsTermsValueId = i + 1;
					}
				}
			}
			return true;
		}
		return false;
	}

	// Token: 0x06001EF9 RID: 7929 RVA: 0x00180874 File Offset: 0x0017EA74
	private void OnCloseSkillChangeWindow()
	{
		SelTacticsSkillChangeWindowCtrl.EditResultData editResultData = CanvasManager.HdlOpenWindowTacticsSkillChange.GetEditResultData();
		this.currentPvpTacticsTypeId = editResultData.pvpTacticsTypeId;
		TacticsStaticSkill tacticsSkillStaticData = DataManager.DmChara.GetTacticsSkillStaticData(this.currentPvpTacticsTypeId);
		this.guiData.Icon_TacticsSkill.SetSprite(this.currentPvpTacticsTypeId);
		this.guiData.Txt_SkillName.text = tacticsSkillStaticData.skillName;
		this.guiData.Txt_SkillInfo.text = tacticsSkillStaticData.skillInfo;
		List<TacticsParam.Tactics> tacticsParamData = DataManager.DmChara.GetTacticsParamData();
		this.UpdateTacticsTermsTypeButton(tacticsParamData);
		if (TacticsStaticSkill.Type.ADD_ACTION_DAMAGE == tacticsSkillStaticData.type || TacticsStaticSkill.Type.ADD_BEAT_DAMAGE == tacticsSkillStaticData.type || TacticsStaticSkill.Type.ADD_TRY_DAMAGE == tacticsSkillStaticData.type)
		{
			for (int i = 0; i < tacticsParamData.Count; i++)
			{
				if (TacticsParam.Tactics.Type.TURN_NUM != tacticsParamData[i].type)
				{
					this.guiData.tacticsTermsTypeButtonList[i].SetActEnable(false);
					this.guiData.tacticsTermsTypeButtonList[i].SetToggleIndex(0);
				}
				else if (this.currentPvpTacticsTermsTypeId != i + 1)
				{
					this.currentPvpTacticsTermsTypeId = i + 1;
					this.guiData.tacticsTermsTypeButtonList[i].SetToggleIndex(1);
					this.currentPvpTacticsTermsValueId = 1;
					this.UpdateTacticsTermsValueButton(tacticsParamData[i]);
				}
			}
		}
	}

	// Token: 0x06001EFA RID: 7930 RVA: 0x001809B4 File Offset: 0x0017EBB4
	private void UpdateTacticsTermsTypeButton(List<TacticsParam.Tactics> paramDataList)
	{
		if (0 < paramDataList.Count)
		{
			for (int i = 0; i < this.guiData.tacticsTermsTypeButtonList.Count; i++)
			{
				if (i < paramDataList.Count)
				{
					this.guiData.tacticsTermsTypeButtonList[i].gameObject.SetActive(true);
					if (!this.guiData.tacticsTermsTypeButtonList[i].ActEnable)
					{
						this.guiData.tacticsTermsTypeButtonList[i].SetActEnable(true);
					}
					this.guiData.tacticsTermsTypeButtonList[i].transform.Find("Num_Txt").GetComponent<PguiTextCtrl>().text = paramDataList[i].tacticsName;
					if (this.currentPvpTacticsTermsTypeId == i + 1)
					{
						this.guiData.tacticsTermsTypeButtonList[i].SetToggleIndex(1);
					}
					else
					{
						this.guiData.tacticsTermsTypeButtonList[i].SetToggleIndex(0);
					}
				}
				else
				{
					this.guiData.tacticsTermsTypeButtonList[i].gameObject.SetActive(false);
					this.guiData.tacticsTermsTypeButtonList[i].SetToggleIndex(0);
				}
			}
		}
	}

	// Token: 0x06001EFB RID: 7931 RVA: 0x00180AEC File Offset: 0x0017ECEC
	private void UpdateTacticsTermsValueButton(TacticsParam.Tactics param)
	{
		if (param != null)
		{
			for (int i = 0; i < this.guiData.tacticsTermsValueIdButtonList.Count; i++)
			{
				if (i < param.param.Count)
				{
					this.guiData.tacticsTermsValueIdButtonList[i].gameObject.SetActive(true);
					this.guiData.tacticsTermsValueIdButtonList[i].transform.Find("Num_Txt").GetComponent<PguiTextCtrl>().text = param.paramInfo.Replace("[Param]", param.param[i].ToString());
					if (this.currentPvpTacticsTermsValueId == i + 1)
					{
						this.guiData.tacticsTermsValueIdButtonList[i].SetToggleIndex(1);
					}
					else
					{
						this.guiData.tacticsTermsValueIdButtonList[i].SetToggleIndex(0);
					}
				}
				else
				{
					this.guiData.tacticsTermsValueIdButtonList[i].gameObject.SetActive(false);
					this.guiData.tacticsTermsValueIdButtonList[i].SetToggleIndex(0);
				}
			}
		}
	}

	// Token: 0x06001EFC RID: 7932 RVA: 0x00180C0B File Offset: 0x0017EE0B
	public SelTacticsSkillInfoWindowCtrl.EditResultData GetEditResultData()
	{
		return new SelTacticsSkillInfoWindowCtrl.EditResultData
		{
			pvpTacticsTypeId = this.currentPvpTacticsTypeId,
			pvpTacticsTermsTypeId = this.currentPvpTacticsTermsTypeId,
			pvpTacticsTermsValueId = this.currentPvpTacticsTermsValueId
		};
	}

	// Token: 0x04001694 RID: 5780
	public SelTacticsSkillInfoWindowCtrl.GUI guiData;

	// Token: 0x04001695 RID: 5781
	private IEnumerator IEWindowMove;

	// Token: 0x04001696 RID: 5782
	private UnityAction windowOpenEndCb;

	// Token: 0x04001697 RID: 5783
	private UnityAction windowCloseEndCb;

	// Token: 0x04001698 RID: 5784
	public int currentPvpTacticsTypeId;

	// Token: 0x04001699 RID: 5785
	public int currentPvpTacticsTermsTypeId;

	// Token: 0x0400169A RID: 5786
	public int currentPvpTacticsTermsValueId;

	// Token: 0x02000FF6 RID: 4086
	public class GUI
	{
		// Token: 0x06005191 RID: 20881 RVA: 0x00246AC8 File Offset: 0x00244CC8
		public GUI(Transform baseTr)
		{
			this.owCtrl = baseTr.Find("Window_TacticsSkill_Info").GetComponent<PguiOpenWindowCtrl>();
			this.Icon_TacticsSkill = new SelTacticsSkillInfoWindowCtrl.GUI.IconSkil(baseTr.Find("Window_TacticsSkill_Info/Base/Window/Contents/Contents01/Icon"));
			this.Txt_SkillName = baseTr.Find("Window_TacticsSkill_Info/Base/Window/Contents/Contents01/TacticsName").GetComponent<PguiTextCtrl>();
			this.Btn_SkillChange = baseTr.Find("Window_TacticsSkill_Info/Base/Window/Contents/Contents01/Btn_change").GetComponent<PguiButtonCtrl>();
			this.Txt_SkillInfo = baseTr.Find("Window_TacticsSkill_Info/Base/Window/Contents/Contents01/Txt").GetComponent<PguiTextCtrl>();
			this.tacticsTermsTypeButtonList = new List<PguiToggleButtonCtrl>
			{
				baseTr.Find("Window_TacticsSkill_Info/Base/Window/Contents/Contents02/Btn01").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Window_TacticsSkill_Info/Base/Window/Contents/Contents02/Btn02").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Window_TacticsSkill_Info/Base/Window/Contents/Contents02/Btn03").GetComponent<PguiToggleButtonCtrl>()
			};
			this.tacticsTermsValueIdButtonList = new List<PguiToggleButtonCtrl>
			{
				baseTr.Find("Window_TacticsSkill_Info/Base/Window/Contents/Contents03/Btn01").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Window_TacticsSkill_Info/Base/Window/Contents/Contents03/Btn02").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Window_TacticsSkill_Info/Base/Window/Contents/Contents03/Btn03").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Window_TacticsSkill_Info/Base/Window/Contents/Contents03/Btn04").GetComponent<PguiToggleButtonCtrl>(),
				baseTr.Find("Window_TacticsSkill_Info/Base/Window/Contents/Contents03/Btn05").GetComponent<PguiToggleButtonCtrl>()
			};
		}

		// Token: 0x0400599B RID: 22939
		public GameObject baseObj;

		// Token: 0x0400599C RID: 22940
		public PguiOpenWindowCtrl owCtrl;

		// Token: 0x0400599D RID: 22941
		public SelTacticsSkillInfoWindowCtrl.GUI.IconSkil Icon_TacticsSkill;

		// Token: 0x0400599E RID: 22942
		public PguiTextCtrl Txt_SkillName;

		// Token: 0x0400599F RID: 22943
		public PguiButtonCtrl Btn_SkillChange;

		// Token: 0x040059A0 RID: 22944
		public PguiTextCtrl Txt_SkillInfo;

		// Token: 0x040059A1 RID: 22945
		public List<PguiToggleButtonCtrl> tacticsTermsTypeButtonList;

		// Token: 0x040059A2 RID: 22946
		public List<PguiToggleButtonCtrl> tacticsTermsValueIdButtonList;

		// Token: 0x02001227 RID: 4647
		public class IconSkil
		{
			// Token: 0x060057EF RID: 22511 RVA: 0x00259F19 File Offset: 0x00258119
			public IconSkil(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.icon = baseTr.GetComponent<PguiReplaceSpriteCtrl>();
			}

			// Token: 0x060057F0 RID: 22512 RVA: 0x00259F39 File Offset: 0x00258139
			public void SetSprite(int id)
			{
				this.icon.Replace(id);
			}

			// Token: 0x04006371 RID: 25457
			private GameObject baseObj;

			// Token: 0x04006372 RID: 25458
			private PguiReplaceSpriteCtrl icon;
		}
	}

	// Token: 0x02000FF7 RID: 4087
	public class SetupParam
	{
		// Token: 0x06005192 RID: 20882 RVA: 0x00246C0F File Offset: 0x00244E0F
		public SetupParam()
		{
			this.openEndCb = null;
			this.closeEndCb = null;
			this.pvpTacticsTypeId = 1;
			this.pvpTacticsTermsTypeId = 1;
			this.pvpTacticsTermsValueId = 1;
		}

		// Token: 0x040059A3 RID: 22947
		public UnityAction openEndCb;

		// Token: 0x040059A4 RID: 22948
		public UnityAction closeEndCb;

		// Token: 0x040059A5 RID: 22949
		public int pvpTacticsTypeId;

		// Token: 0x040059A6 RID: 22950
		public int pvpTacticsTermsTypeId;

		// Token: 0x040059A7 RID: 22951
		public int pvpTacticsTermsValueId;
	}

	// Token: 0x02000FF8 RID: 4088
	public class EditResultData
	{
		// Token: 0x06005193 RID: 20883 RVA: 0x00246C3A File Offset: 0x00244E3A
		public EditResultData()
		{
			this.pvpTacticsTypeId = 1;
			this.pvpTacticsTermsTypeId = 1;
			this.pvpTacticsTermsValueId = 1;
		}

		// Token: 0x040059A8 RID: 22952
		public int pvpTacticsTypeId;

		// Token: 0x040059A9 RID: 22953
		public int pvpTacticsTermsTypeId;

		// Token: 0x040059AA RID: 22954
		public int pvpTacticsTermsValueId;
	}
}
