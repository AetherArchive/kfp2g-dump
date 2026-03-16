using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelTacticsSkillInfoWindowCtrl : MonoBehaviour
{
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

	private void OnclickSkillChangeButton(PguiButtonCtrl button)
	{
		CanvasManager.HdlOpenWindowTacticsSkillChange.Open(new SelTacticsSkillChangeWindowCtrl.SetupParam
		{
			openEndCb = null,
			closeEndCb = new UnityAction(this.OnCloseSkillChangeWindow),
			pvpTacticsTypeId = this.currentPvpTacticsTypeId
		});
	}

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

	public SelTacticsSkillInfoWindowCtrl.EditResultData GetEditResultData()
	{
		return new SelTacticsSkillInfoWindowCtrl.EditResultData
		{
			pvpTacticsTypeId = this.currentPvpTacticsTypeId,
			pvpTacticsTermsTypeId = this.currentPvpTacticsTermsTypeId,
			pvpTacticsTermsValueId = this.currentPvpTacticsTermsValueId
		};
	}

	public SelTacticsSkillInfoWindowCtrl.GUI guiData;

	private IEnumerator IEWindowMove;

	private UnityAction windowOpenEndCb;

	private UnityAction windowCloseEndCb;

	public int currentPvpTacticsTypeId;

	public int currentPvpTacticsTermsTypeId;

	public int currentPvpTacticsTermsValueId;

	public class GUI
	{
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

		public GameObject baseObj;

		public PguiOpenWindowCtrl owCtrl;

		public SelTacticsSkillInfoWindowCtrl.GUI.IconSkil Icon_TacticsSkill;

		public PguiTextCtrl Txt_SkillName;

		public PguiButtonCtrl Btn_SkillChange;

		public PguiTextCtrl Txt_SkillInfo;

		public List<PguiToggleButtonCtrl> tacticsTermsTypeButtonList;

		public List<PguiToggleButtonCtrl> tacticsTermsValueIdButtonList;

		public class IconSkil
		{
			public IconSkil(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.icon = baseTr.GetComponent<PguiReplaceSpriteCtrl>();
			}

			public void SetSprite(int id)
			{
				this.icon.Replace(id);
			}

			private GameObject baseObj;

			private PguiReplaceSpriteCtrl icon;
		}
	}

	public class SetupParam
	{
		public SetupParam()
		{
			this.openEndCb = null;
			this.closeEndCb = null;
			this.pvpTacticsTypeId = 1;
			this.pvpTacticsTermsTypeId = 1;
			this.pvpTacticsTermsValueId = 1;
		}

		public UnityAction openEndCb;

		public UnityAction closeEndCb;

		public int pvpTacticsTypeId;

		public int pvpTacticsTermsTypeId;

		public int pvpTacticsTermsValueId;
	}

	public class EditResultData
	{
		public EditResultData()
		{
			this.pvpTacticsTypeId = 1;
			this.pvpTacticsTermsTypeId = 1;
			this.pvpTacticsTermsValueId = 1;
		}

		public int pvpTacticsTypeId;

		public int pvpTacticsTermsTypeId;

		public int pvpTacticsTermsValueId;
	}
}
