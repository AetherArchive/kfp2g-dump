using System;
using SGNFW.Common;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

// Token: 0x020001AA RID: 426
public class ItemDetailCtrl : MonoBehaviour
{
	// Token: 0x06001CCB RID: 7371 RVA: 0x00169593 File Offset: 0x00167793
	private void Start()
	{
	}

	// Token: 0x06001CCC RID: 7372 RVA: 0x00169595 File Offset: 0x00167795
	public void Init()
	{
		this.guiData = new ItemDetailCtrl.GUI(base.transform);
		this.guiData.baseObj.SetActive(false);
	}

	// Token: 0x06001CCD RID: 7373 RVA: 0x001695BC File Offset: 0x001677BC
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

	// Token: 0x06001CCE RID: 7374 RVA: 0x0016972B File Offset: 0x0016792B
	public void Close()
	{
		Singleton<SceneManager>.Instance.GetCanvasCamera(SceneManager.CanvasType.SYSTEM).GetComponent<Blur>().enabled = false;
		this.guiData.baseObj.SetActive(false);
	}

	// Token: 0x06001CCF RID: 7375 RVA: 0x00169754 File Offset: 0x00167954
	private void Update()
	{
	}

	// Token: 0x0400156B RID: 5483
	private ItemDetailCtrl.GUI guiData;

	// Token: 0x02000F1F RID: 3871
	public class GUI
	{
		// Token: 0x06004EB3 RID: 20147 RVA: 0x00237004 File Offset: 0x00235204
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

		// Token: 0x040055DA RID: 21978
		public GameObject baseObj;

		// Token: 0x040055DB RID: 21979
		public PguiImageCtrl Window;

		// Token: 0x040055DC RID: 21980
		public Transform Null_Left;

		// Token: 0x040055DD RID: 21981
		public Transform Null_Right;

		// Token: 0x040055DE RID: 21982
		public IconItemCtrl Icon_Item;

		// Token: 0x040055DF RID: 21983
		public PguiTextCtrl Txt_Name;

		// Token: 0x040055E0 RID: 21984
		public PguiTextCtrl Txt_Info;

		// Token: 0x040055E1 RID: 21985
		public PguiTextCtrl Num_Own;

		// Token: 0x040055E2 RID: 21986
		public PguiImageCtrl OwnInfo;
	}
}
