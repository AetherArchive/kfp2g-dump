using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001C2 RID: 450
public class SelTacticsSkillChangeWindowCtrl : MonoBehaviour
{
	// Token: 0x06001EEC RID: 7916 RVA: 0x00180184 File Offset: 0x0017E384
	public void Initialize(GameObject baseWindow)
	{
		this.guiData = new SelTacticsSkillChangeWindowCtrl.GUI(baseWindow.transform);
		foreach (SelTacticsSkillChangeWindowCtrl.GUI.SkillButton skillButton in this.guiData.skillButtonList)
		{
			skillButton.btnCtrl.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickSkillButton), PguiButtonCtrl.SoundType.DEFAULT);
			skillButton.CurrentObj.SetActive(false);
		}
	}

	// Token: 0x06001EED RID: 7917 RVA: 0x00180208 File Offset: 0x0017E408
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

	// Token: 0x06001EEE RID: 7918 RVA: 0x00180299 File Offset: 0x0017E499
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

	// Token: 0x06001EEF RID: 7919 RVA: 0x001802BC File Offset: 0x0017E4BC
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

	// Token: 0x06001EF0 RID: 7920 RVA: 0x00180374 File Offset: 0x0017E574
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

	// Token: 0x06001EF1 RID: 7921 RVA: 0x0018047C File Offset: 0x0017E67C
	public SelTacticsSkillChangeWindowCtrl.EditResultData GetEditResultData()
	{
		return new SelTacticsSkillChangeWindowCtrl.EditResultData
		{
			pvpTacticsTypeId = this.currentPvpTacticsTypeId
		};
	}

	// Token: 0x0400168F RID: 5775
	public SelTacticsSkillChangeWindowCtrl.GUI guiData;

	// Token: 0x04001690 RID: 5776
	private IEnumerator IEWindowMove;

	// Token: 0x04001691 RID: 5777
	private UnityAction windowOpenEndCb;

	// Token: 0x04001692 RID: 5778
	private UnityAction windowCloseEndCb;

	// Token: 0x04001693 RID: 5779
	private int currentPvpTacticsTypeId;

	// Token: 0x02000FF3 RID: 4083
	public class GUI
	{
		// Token: 0x0600518E RID: 20878 RVA: 0x00246968 File Offset: 0x00244B68
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

		// Token: 0x04005993 RID: 22931
		public GameObject baseObj;

		// Token: 0x04005994 RID: 22932
		public PguiOpenWindowCtrl owCtrl;

		// Token: 0x04005995 RID: 22933
		public List<SelTacticsSkillChangeWindowCtrl.GUI.SkillButton> skillButtonList;

		// Token: 0x04005996 RID: 22934
		public PguiTextCtrl Txt_SkillInfo;

		// Token: 0x02001226 RID: 4646
		public class SkillButton
		{
			// Token: 0x060057EE RID: 22510 RVA: 0x00259EAC File Offset: 0x002580AC
			public SkillButton(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.btnCtrl = baseTr.GetComponent<PguiButtonCtrl>();
				this.icon = baseTr.Find("BaseImage/Icon_Skill").GetComponent<PguiImageCtrl>();
				this.Txt_SkillName = baseTr.Find("BaseImage/Txt_Skill").GetComponent<PguiTextCtrl>();
				this.CurrentObj = baseTr.Find("BaseImage/Current").gameObject;
			}

			// Token: 0x0400636C RID: 25452
			public GameObject baseObj;

			// Token: 0x0400636D RID: 25453
			public PguiButtonCtrl btnCtrl;

			// Token: 0x0400636E RID: 25454
			public PguiImageCtrl icon;

			// Token: 0x0400636F RID: 25455
			public PguiTextCtrl Txt_SkillName;

			// Token: 0x04006370 RID: 25456
			public GameObject CurrentObj;
		}
	}

	// Token: 0x02000FF4 RID: 4084
	public class SetupParam
	{
		// Token: 0x0600518F RID: 20879 RVA: 0x00246A9A File Offset: 0x00244C9A
		public SetupParam()
		{
			this.openEndCb = null;
			this.closeEndCb = null;
			this.pvpTacticsTypeId = 1;
		}

		// Token: 0x04005997 RID: 22935
		public UnityAction openEndCb;

		// Token: 0x04005998 RID: 22936
		public UnityAction closeEndCb;

		// Token: 0x04005999 RID: 22937
		public int pvpTacticsTypeId;
	}

	// Token: 0x02000FF5 RID: 4085
	public class EditResultData
	{
		// Token: 0x06005190 RID: 20880 RVA: 0x00246AB7 File Offset: 0x00244CB7
		public EditResultData()
		{
			this.pvpTacticsTypeId = 1;
		}

		// Token: 0x0400599A RID: 22938
		public int pvpTacticsTypeId;
	}
}
