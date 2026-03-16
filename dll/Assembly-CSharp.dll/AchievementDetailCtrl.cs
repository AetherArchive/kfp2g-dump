using System;
using SGNFW.Common;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class AchievementDetailCtrl : MonoBehaviour
{
	public void Init()
	{
		this.guiData = new AchievementDetailCtrl.GUI(base.transform);
		this.guiData.baseObj.SetActive(false);
	}

	public void Open(AchievementCtrl ctrl)
	{
		DataManagerAchievement.AchievementStaticData achievementData = DataManager.DmAchievement.GetAchievementData(ctrl.GetAchievementId());
		if (achievementData == null)
		{
			return;
		}
		base.gameObject.SetActive(true);
		Singleton<SceneManager>.Instance.GetCanvasCamera(SceneManager.CanvasType.SYSTEM).GetComponent<Blur>().enabled = true;
		Transform transform;
		if (ctrl.transform.parent.position.x < 0f)
		{
			transform = this.guiData.Null_Right;
		}
		else
		{
			transform = this.guiData.Null_Left;
		}
		this.guiData.Window.transform.SetParent(transform);
		Vector3 localPosition = this.guiData.Window.transform.localPosition;
		localPosition.x = 0f;
		this.guiData.Window.transform.localPosition = localPosition;
		this.guiData.Txt_Name.text = (achievementData.name.IsNullOrEmpty() ? "" : achievementData.name);
		this.guiData.Txt_Info.text = (achievementData.flavorText.IsNullOrEmpty() ? "" : achievementData.flavorText);
		if (!achievementData.iconName.IsNullOrEmpty())
		{
			this.guiData.Texture.SetRawImage("Texture2D/" + achievementData.iconName, true, false, null);
		}
		this.guiData.Texture.gameObject.SetActive(!achievementData.iconName.IsNullOrEmpty());
		this.guiData.baseObj.SetActive(true);
	}

	public void Close()
	{
		Singleton<SceneManager>.Instance.GetCanvasCamera(SceneManager.CanvasType.SYSTEM).GetComponent<Blur>().enabled = false;
		this.guiData.baseObj.SetActive(false);
	}

	private void Update()
	{
	}

	private AchievementDetailCtrl.GUI guiData;

	public class GUI
	{
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Window = baseTr.Find("Window").GetComponent<PguiImageCtrl>();
			this.Null_Left = baseTr.Find("Null_Left");
			this.Null_Right = baseTr.Find("Null_Right");
			this.Txt_Name = baseTr.Find("Window/Txt_Name").GetComponent<PguiTextCtrl>();
			this.Txt_Info = baseTr.Find("Window/Txt_Name/Txt_Info").GetComponent<PguiTextCtrl>();
			this.Texture = baseTr.Find("Window/Texture").GetComponent<PguiRawImageCtrl>();
		}

		public GameObject baseObj;

		public PguiImageCtrl Window;

		public Transform Null_Left;

		public Transform Null_Right;

		public PguiTextCtrl Txt_Name;

		public PguiTextCtrl Txt_Info;

		public PguiRawImageCtrl Texture;
	}
}
