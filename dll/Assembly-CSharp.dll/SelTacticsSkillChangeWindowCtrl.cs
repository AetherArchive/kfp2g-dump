using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelTacticsSkillChangeWindowCtrl : MonoBehaviour
{
	public void Initialize(GameObject baseWindow)
	{
		this.guiData = new SelTacticsSkillChangeWindowCtrl.GUI(baseWindow.transform);
		foreach (SelTacticsSkillChangeWindowCtrl.GUI.SkillButton skillButton in this.guiData.skillButtonList)
		{
			skillButton.btnCtrl.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickSkillButton), PguiButtonCtrl.SoundType.DEFAULT);
			skillButton.CurrentObj.SetActive(false);
		}
	}

	public void Open(SelTacticsSkillChangeWindowCtrl.SetupParam setupParam)
	{
		this.windowOpenEndCb = setupParam.openEndCb;
		this.windowCloseEndCb = setupParam.closeEndCb;
		this.currentPvpTacticsTypeId = setupParam.pvpTacticsTypeId;
		List<TacticsStaticSkill> tacticsSkillStaticData = DataManager.DmChara.GetTacticsSkillStaticData();
		this.UpdateSkillButton(tacticsSkillStaticData);
		this.guiData.owCtrl.Setup("作戦変更", null, null, true, new PguiOpenWindowCtrl.Callback(this.OnClickCloseButton), null, false);
		this.guiData.owCtrl.Open();
		UnityAction unityAction = this.windowOpenEndCb;
		if (unityAction != null)
		{
			unityAction();
		}
		this.windowOpenEndCb = null;
	}

	private bool OnClickCloseButton(int index)
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

	private void OnClickSkillButton(PguiButtonCtrl button)
	{
		for (int i = 0; i < this.guiData.skillButtonList.Count; i++)
		{
			if (button == this.guiData.skillButtonList[i].btnCtrl)
			{
				this.guiData.skillButtonList[i].CurrentObj.SetActive(true);
				this.currentPvpTacticsTypeId = i + 1;
				TacticsStaticSkill tacticsSkillStaticData = DataManager.DmChara.GetTacticsSkillStaticData(this.currentPvpTacticsTypeId);
				this.guiData.Txt_SkillInfo.text = tacticsSkillStaticData.skillInfo;
			}
			else
			{
				this.guiData.skillButtonList[i].CurrentObj.SetActive(false);
			}
		}
	}

	private void UpdateSkillButton(List<TacticsStaticSkill> skillList)
	{
		if (0 < skillList.Count)
		{
			for (int i = 0; i < this.guiData.skillButtonList.Count; i++)
			{
				if (i < skillList.Count)
				{
					this.guiData.skillButtonList[i].baseObj.SetActive(true);
					this.guiData.skillButtonList[i].Txt_SkillName.text = skillList[i].skillName;
					if (this.currentPvpTacticsTypeId == i + 1)
					{
						this.guiData.skillButtonList[i].CurrentObj.SetActive(true);
						this.guiData.Txt_SkillInfo.text = skillList[i].skillInfo;
					}
					else
					{
						this.guiData.skillButtonList[i].CurrentObj.SetActive(false);
					}
				}
				else
				{
					this.guiData.skillButtonList[i].baseObj.SetActive(false);
				}
			}
		}
	}

	public SelTacticsSkillChangeWindowCtrl.EditResultData GetEditResultData()
	{
		return new SelTacticsSkillChangeWindowCtrl.EditResultData
		{
			pvpTacticsTypeId = this.currentPvpTacticsTypeId
		};
	}

	public SelTacticsSkillChangeWindowCtrl.GUI guiData;

	private IEnumerator IEWindowMove;

	private UnityAction windowOpenEndCb;

	private UnityAction windowCloseEndCb;

	private int currentPvpTacticsTypeId;

	public class GUI
	{
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.owCtrl = baseTr.Find("Window_TacticsSkill_Change").GetComponent<PguiOpenWindowCtrl>();
			this.skillButtonList = new List<SelTacticsSkillChangeWindowCtrl.GUI.SkillButton>
			{
				new SelTacticsSkillChangeWindowCtrl.GUI.SkillButton(baseTr.Find("Window_TacticsSkill_Change/Base/Window/Contents/Contents01/Btn_SkillSelect01")),
				new SelTacticsSkillChangeWindowCtrl.GUI.SkillButton(baseTr.Find("Window_TacticsSkill_Change/Base/Window/Contents/Contents01/Btn_SkillSelect02")),
				new SelTacticsSkillChangeWindowCtrl.GUI.SkillButton(baseTr.Find("Window_TacticsSkill_Change/Base/Window/Contents/Contents01/Btn_SkillSelect03")),
				new SelTacticsSkillChangeWindowCtrl.GUI.SkillButton(baseTr.Find("Window_TacticsSkill_Change/Base/Window/Contents/Contents01/Btn_SkillSelect04")),
				new SelTacticsSkillChangeWindowCtrl.GUI.SkillButton(baseTr.Find("Window_TacticsSkill_Change/Base/Window/Contents/Contents01/Btn_SkillSelect05")),
				new SelTacticsSkillChangeWindowCtrl.GUI.SkillButton(baseTr.Find("Window_TacticsSkill_Change/Base/Window/Contents/Contents01/Btn_SkillSelect06")),
				new SelTacticsSkillChangeWindowCtrl.GUI.SkillButton(baseTr.Find("Window_TacticsSkill_Change/Base/Window/Contents/Contents01/Btn_SkillSelect07")),
				new SelTacticsSkillChangeWindowCtrl.GUI.SkillButton(baseTr.Find("Window_TacticsSkill_Change/Base/Window/Contents/Contents01/Btn_SkillSelect08")),
				new SelTacticsSkillChangeWindowCtrl.GUI.SkillButton(baseTr.Find("Window_TacticsSkill_Change/Base/Window/Contents/Contents01/Btn_SkillSelect09")),
				new SelTacticsSkillChangeWindowCtrl.GUI.SkillButton(baseTr.Find("Window_TacticsSkill_Change/Base/Window/Contents/Contents01/Btn_SkillSelect10"))
			};
			this.Txt_SkillInfo = baseTr.Find("Window_TacticsSkill_Change/Base/Window/Contents/Contents01/Txt").GetComponent<PguiTextCtrl>();
		}

		public GameObject baseObj;

		public PguiOpenWindowCtrl owCtrl;

		public List<SelTacticsSkillChangeWindowCtrl.GUI.SkillButton> skillButtonList;

		public PguiTextCtrl Txt_SkillInfo;

		public class SkillButton
		{
			public SkillButton(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.btnCtrl = baseTr.GetComponent<PguiButtonCtrl>();
				this.icon = baseTr.Find("BaseImage/Icon_Skill").GetComponent<PguiImageCtrl>();
				this.Txt_SkillName = baseTr.Find("BaseImage/Txt_Skill").GetComponent<PguiTextCtrl>();
				this.CurrentObj = baseTr.Find("BaseImage/Current").gameObject;
			}

			public GameObject baseObj;

			public PguiButtonCtrl btnCtrl;

			public PguiImageCtrl icon;

			public PguiTextCtrl Txt_SkillName;

			public GameObject CurrentObj;
		}
	}

	public class SetupParam
	{
		public SetupParam()
		{
			this.openEndCb = null;
			this.closeEndCb = null;
			this.pvpTacticsTypeId = 1;
		}

		public UnityAction openEndCb;

		public UnityAction closeEndCb;

		public int pvpTacticsTypeId;
	}

	public class EditResultData
	{
		public EditResultData()
		{
			this.pvpTacticsTypeId = 1;
		}

		public int pvpTacticsTypeId;
	}
}
