using System;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001AE RID: 430
public class MissionBarCtrl : MonoBehaviour
{
	// Token: 0x06001CFD RID: 7421 RVA: 0x0016A92E File Offset: 0x00168B2E
	public void Init()
	{
		this.GuiData = new MissionBarCtrl.GuiMissionListBar(base.transform);
	}

	// Token: 0x06001CFE RID: 7422 RVA: 0x0016A941 File Offset: 0x00168B41
	public void SetupMission(MissionBarCtrl.SetupParam param)
	{
		if (this.GuiData == null)
		{
			return;
		}
		this.GuiData.Setup(param, base.transform.gameObject);
	}

	// Token: 0x06001CFF RID: 7423 RVA: 0x0016A963 File Offset: 0x00168B63
	public void SetupCharaMission(MissionBarCtrl.SetupParam param)
	{
		if (this.GuiData == null)
		{
			return;
		}
		param.gageDisp = true;
		this.GuiData.Setup(param, base.transform.gameObject);
	}

	// Token: 0x0400157D RID: 5501
	private MissionBarCtrl.GuiMissionListBar GuiData;

	// Token: 0x02000F34 RID: 3892
	public class SetupParam
	{
		// Token: 0x04005645 RID: 22085
		public string missionContents;

		// Token: 0x04005646 RID: 22086
		public bool isClear;

		// Token: 0x04005647 RID: 22087
		public int numerator;

		// Token: 0x04005648 RID: 22088
		public int denominator;

		// Token: 0x04005649 RID: 22089
		public bool received;

		// Token: 0x0400564A RID: 22090
		public bool isSpecial;

		// Token: 0x0400564B RID: 22091
		public ItemData itemData;

		// Token: 0x0400564C RID: 22092
		public UnityAction onClick;

		// Token: 0x0400564D RID: 22093
		public bool gageDisp;

		// Token: 0x0400564E RID: 22094
		public bool isContainTransition;
	}

	// Token: 0x02000F35 RID: 3893
	private class GuiMissionListBar
	{
		// Token: 0x06004EEF RID: 20207 RVA: 0x0023827C File Offset: 0x0023647C
		public GuiMissionListBar(Transform BaseTr)
		{
			this.ItemNameText = BaseTr.Find("BaseImage/Txt_ItemName").GetComponent<PguiTextCtrl>();
			this.NumMissionText = BaseTr.Find("BaseImage/Num_Mission").GetComponent<PguiTextCtrl>();
			this.BaseClearImage = BaseTr.Find("BaseImage/Base_Clear").GetComponent<PguiImageCtrl>();
			this.MarkClearImage = BaseTr.Find("BaseImage/Mark_Clear").GetComponent<PguiImageCtrl>();
			this.GageOuterImage = BaseTr.Find("BaseImage/GageAll").GetComponent<PguiImageCtrl>();
			this.GageInnerImage = BaseTr.Find("BaseImage/GageAll/Gage").GetComponent<PguiImageCtrl>();
		}

		// Token: 0x06004EF0 RID: 20208 RVA: 0x00238314 File Offset: 0x00236514
		public void Setup(MissionBarCtrl.SetupParam param, GameObject go)
		{
			this.ItemNameText.text = param.missionContents;
			this.BaseClearImage.gameObject.SetActive(param.isClear);
			this.NumMissionText.text = param.numerator.ToString() + "/" + param.denominator.ToString();
			IconItemCtrl component = go.transform.Find("BaseImage/Icon_Item/Icon_Item").GetComponent<IconItemCtrl>();
			if (component != null)
			{
				component.Setup(param.itemData, new IconItemCtrl.SetupParam
				{
					useInfo = true
				});
			}
			this.GageInnerImage.m_Image.fillAmount = (float)param.numerator / (float)param.denominator;
			this.MarkClearImage.gameObject.SetActive(param.isClear);
			PguiButtonCtrl component2 = go.transform.Find("BaseImage/Btn_Get").GetComponent<PguiButtonCtrl>();
			component2.SetActEnable((param.isClear || param.isContainTransition) && !param.received, false, false);
			component2.AddOnClickListener(delegate(PguiButtonCtrl button)
			{
				param.onClick();
			}, PguiButtonCtrl.SoundType.DEFAULT);
			go.transform.Find("BaseImage/Btn_Get/BaseImage/Txt").GetComponent<PguiTextCtrl>().text = (param.received ? "受け取り済" : ((!param.isClear && param.isContainTransition) ? "挑戦する" : "受け取る"));
			go.transform.Find("BaseImage/Mark_Special").gameObject.SetActive(param.isSpecial);
			this.NumMissionText.gameObject.SetActive(param.gageDisp);
			this.GageOuterImage.gameObject.SetActive(param.gageDisp);
			this.GageInnerImage.gameObject.SetActive(param.gageDisp);
			go.transform.Find("BaseImage/Mark_New").gameObject.SetActive(false);
		}

		// Token: 0x0400564F RID: 22095
		public PguiTextCtrl ItemNameText;

		// Token: 0x04005650 RID: 22096
		public PguiTextCtrl NumMissionText;

		// Token: 0x04005651 RID: 22097
		public PguiImageCtrl BaseClearImage;

		// Token: 0x04005652 RID: 22098
		public PguiImageCtrl MarkClearImage;

		// Token: 0x04005653 RID: 22099
		public PguiImageCtrl GageOuterImage;

		// Token: 0x04005654 RID: 22100
		public PguiImageCtrl GageInnerImage;
	}
}
