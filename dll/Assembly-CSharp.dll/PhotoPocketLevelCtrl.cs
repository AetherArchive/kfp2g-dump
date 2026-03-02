using System;
using UnityEngine;

// Token: 0x020001B0 RID: 432
public class PhotoPocketLevelCtrl : MonoBehaviour
{
	// Token: 0x17000406 RID: 1030
	// (get) Token: 0x06001D38 RID: 7480 RVA: 0x0016BF98 File Offset: 0x0016A198
	// (set) Token: 0x06001D39 RID: 7481 RVA: 0x0016BFA0 File Offset: 0x0016A1A0
	private PhotoPocketLevelCtrl.GUI GuiData { get; set; }

	// Token: 0x17000407 RID: 1031
	// (get) Token: 0x06001D3A RID: 7482 RVA: 0x0016BFA9 File Offset: 0x0016A1A9
	// (set) Token: 0x06001D3B RID: 7483 RVA: 0x0016BFB1 File Offset: 0x0016A1B1
	private bool IsInit { get; set; }

	// Token: 0x06001D3C RID: 7484 RVA: 0x0016BFBC File Offset: 0x0016A1BC
	public void Setup(PhotoPocketLevelCtrl.SetupParam param)
	{
		if (!this.IsInit)
		{
			this.InitForce();
		}
		this.setupParam = param;
		this.GuiData.Num.text = string.Format("{0}", this.setupParam.charaPackData.dynamicData.PhotoFrameTotalStep);
		this.GuiData.Num.m_Text.color = this.GuiData.Num.GetComponent<PguiColorCtrl>().GetGameObjectById((param.charaPackData.dynamicData.PhotoFrameTotalStep >= DataManager.DmServerMst.StaticCharaPpDataMap[param.charaPackData.staticData.baseData.photoFrameTableId].PpStepMax) ? "Max" : "Normal");
	}

	// Token: 0x06001D3D RID: 7485 RVA: 0x0016C084 File Offset: 0x0016A284
	public void SetupMask(bool isEnable, Color maskColor)
	{
		PguiImageCtrl[] components = this.GuiData.baseObj.GetComponents<PguiImageCtrl>();
		if (components != null)
		{
			PguiImageCtrl[] array = components;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].m_Image.color = (isEnable ? maskColor : Color.white);
			}
		}
	}

	// Token: 0x06001D3E RID: 7486 RVA: 0x0016C0CD File Offset: 0x0016A2CD
	public void SetActive(bool sw)
	{
		this.GuiData.baseObj.SetActive(sw);
	}

	// Token: 0x06001D3F RID: 7487 RVA: 0x0016C0E0 File Offset: 0x0016A2E0
	private void InitForce()
	{
		this.GuiData = new PhotoPocketLevelCtrl.GUI(base.transform);
		this.IsInit = true;
	}

	// Token: 0x06001D40 RID: 7488 RVA: 0x0016C0FA File Offset: 0x0016A2FA
	private void Awake()
	{
		this.InitForce();
	}

	// Token: 0x06001D41 RID: 7489 RVA: 0x0016C102 File Offset: 0x0016A302
	private void Start()
	{
	}

	// Token: 0x06001D42 RID: 7490 RVA: 0x0016C104 File Offset: 0x0016A304
	private void Update()
	{
	}

	// Token: 0x04001582 RID: 5506
	private PhotoPocketLevelCtrl.SetupParam setupParam = new PhotoPocketLevelCtrl.SetupParam();

	// Token: 0x02000F44 RID: 3908
	public class SetupParam
	{
		// Token: 0x0400568A RID: 22154
		public CharaPackData charaPackData;
	}

	// Token: 0x02000F45 RID: 3909
	protected class GUI
	{
		// Token: 0x06004F15 RID: 20245 RVA: 0x00238ED2 File Offset: 0x002370D2
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Num = baseTr.Find("Num").GetComponent<PguiTextCtrl>();
		}

		// Token: 0x0400568B RID: 22155
		public GameObject baseObj;

		// Token: 0x0400568C RID: 22156
		public PguiTextCtrl Num;
	}
}
