using System;
using UnityEngine;

public class PhotoPocketLevelCtrl : MonoBehaviour
{
	private PhotoPocketLevelCtrl.GUI GuiData { get; set; }

	private bool IsInit { get; set; }

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

	public void SetActive(bool sw)
	{
		this.GuiData.baseObj.SetActive(sw);
	}

	private void InitForce()
	{
		this.GuiData = new PhotoPocketLevelCtrl.GUI(base.transform);
		this.IsInit = true;
	}

	private void Awake()
	{
		this.InitForce();
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	private PhotoPocketLevelCtrl.SetupParam setupParam = new PhotoPocketLevelCtrl.SetupParam();

	public class SetupParam
	{
		public CharaPackData charaPackData;
	}

	protected class GUI
	{
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Num = baseTr.Find("Num").GetComponent<PguiTextCtrl>();
		}

		public GameObject baseObj;

		public PguiTextCtrl Num;
	}
}
