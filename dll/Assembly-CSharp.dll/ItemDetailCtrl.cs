using System;
using SGNFW.Common;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class ItemDetailCtrl : MonoBehaviour
{
	private void Start()
	{
	}

	public void Init()
	{
		this.guiData = new ItemDetailCtrl.GUI(base.transform);
		this.guiData.baseObj.SetActive(false);
	}

	public void Open(IconItemCtrl ctrl, bool viewItem = true)
	{
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
		this.guiData.Window.transform.Find("Icon_Item/Icon_Item").GetComponent<IconItemCtrl>().Setup(ctrl.itemStaticBase, -1);
		this.guiData.Txt_Name.text = ctrl.itemStaticBase.GetName();
		this.guiData.Txt_Info.text = ctrl.itemStaticBase.GetInfo();
		ItemData userItemData = DataManager.DmItem.GetUserItemData(ctrl.itemStaticBase.GetId());
		this.guiData.Num_Own.text = userItemData.num.ToString();
		this.guiData.OwnInfo.gameObject.SetActive(viewItem);
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

	private ItemDetailCtrl.GUI guiData;

	public class GUI
	{
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Window = baseTr.Find("Window").GetComponent<PguiImageCtrl>();
			this.Null_Left = baseTr.Find("Null_Left");
			this.Null_Right = baseTr.Find("Null_Right");
			this.Icon_Item = baseTr.Find("Window/Icon_Item/Icon_Item/Icon_Item").GetComponent<IconItemCtrl>();
			this.Txt_Name = baseTr.Find("Window/Txt_Name").GetComponent<PguiTextCtrl>();
			this.Txt_Info = baseTr.Find("Window/Txt_Name/Txt_Info").GetComponent<PguiTextCtrl>();
			this.Num_Own = baseTr.Find("Window/OwnInfo/Num").GetComponent<PguiTextCtrl>();
			this.OwnInfo = baseTr.Find("Window/OwnInfo").GetComponent<PguiImageCtrl>();
		}

		public GameObject baseObj;

		public PguiImageCtrl Window;

		public Transform Null_Left;

		public Transform Null_Right;

		public IconItemCtrl Icon_Item;

		public PguiTextCtrl Txt_Name;

		public PguiTextCtrl Txt_Info;

		public PguiTextCtrl Num_Own;

		public PguiImageCtrl OwnInfo;
	}
}
