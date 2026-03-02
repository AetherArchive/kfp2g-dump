using System;
using SGNFW.Common;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

// Token: 0x0200018F RID: 399
public class AchievementDetailCtrl : MonoBehaviour
{
	// Token: 0x06001AA5 RID: 6821 RVA: 0x001577CA File Offset: 0x001559CA
	public void Init()
	{
		this.guiData = new AchievementDetailCtrl.GUI(base.transform);
		this.guiData.baseObj.SetActive(false);
	}

	// Token: 0x06001AA6 RID: 6822 RVA: 0x001577F0 File Offset: 0x001559F0
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

	// Token: 0x06001AA7 RID: 6823 RVA: 0x00157970 File Offset: 0x00155B70
	public void Close()
	{
		Singleton<SceneManager>.Instance.GetCanvasCamera(SceneManager.CanvasType.SYSTEM).GetComponent<Blur>().enabled = false;
		this.guiData.baseObj.SetActive(false);
	}

	// Token: 0x06001AA8 RID: 6824 RVA: 0x00157999 File Offset: 0x00155B99
	private void Update()
	{
	}

	// Token: 0x04001449 RID: 5193
	private AchievementDetailCtrl.GUI guiData;

	// Token: 0x02000E81 RID: 3713
	public class GUI
	{
		// Token: 0x06004CE5 RID: 19685 RVA: 0x0022F9E0 File Offset: 0x0022DBE0
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

		// Token: 0x04005358 RID: 21336
		public GameObject baseObj;

		// Token: 0x04005359 RID: 21337
		public PguiImageCtrl Window;

		// Token: 0x0400535A RID: 21338
		public Transform Null_Left;

		// Token: 0x0400535B RID: 21339
		public Transform Null_Right;

		// Token: 0x0400535C RID: 21340
		public PguiTextCtrl Txt_Name;

		// Token: 0x0400535D RID: 21341
		public PguiTextCtrl Txt_Info;

		// Token: 0x0400535E RID: 21342
		public PguiRawImageCtrl Texture;
	}
}
