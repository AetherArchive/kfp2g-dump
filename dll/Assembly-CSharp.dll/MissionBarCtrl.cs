using System;
using UnityEngine;
using UnityEngine.Events;

public class MissionBarCtrl : MonoBehaviour
{
	public void Init()
	{
		this.GuiData = new MissionBarCtrl.GuiMissionListBar(base.transform);
	}

	public void SetupMission(MissionBarCtrl.SetupParam param)
	{
		if (this.GuiData == null)
		{
			return;
		}
		this.GuiData.Setup(param, base.transform.gameObject);
	}

	public void SetupCharaMission(MissionBarCtrl.SetupParam param)
	{
		if (this.GuiData == null)
		{
			return;
		}
		param.gageDisp = true;
		this.GuiData.Setup(param, base.transform.gameObject);
	}

	private MissionBarCtrl.GuiMissionListBar GuiData;

	public class SetupParam
	{
		public string missionContents;

		public bool isClear;

		public int numerator;

		public int denominator;

		public bool received;

		public bool isSpecial;

		public ItemData itemData;

		public UnityAction onClick;

		public bool gageDisp;

		public bool isContainTransition;
	}

	private class GuiMissionListBar
	{
		public GuiMissionListBar(Transform BaseTr)
		{
			this.ItemNameText = BaseTr.Find("BaseImage/Txt_ItemName").GetComponent<PguiTextCtrl>();
			this.NumMissionText = BaseTr.Find("BaseImage/Num_Mission").GetComponent<PguiTextCtrl>();
			this.BaseClearImage = BaseTr.Find("BaseImage/Base_Clear").GetComponent<PguiImageCtrl>();
			this.MarkClearImage = BaseTr.Find("BaseImage/Mark_Clear").GetComponent<PguiImageCtrl>();
			this.GageOuterImage = BaseTr.Find("BaseImage/GageAll").GetComponent<PguiImageCtrl>();
			this.GageInnerImage = BaseTr.Find("BaseImage/GageAll/Gage").GetComponent<PguiImageCtrl>();
		}

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

		public PguiTextCtrl ItemNameText;

		public PguiTextCtrl NumMissionText;

		public PguiImageCtrl BaseClearImage;

		public PguiImageCtrl MarkClearImage;

		public PguiImageCtrl GageOuterImage;

		public PguiImageCtrl GageInnerImage;
	}
}
