using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001A1 RID: 417
public class GetItemWindowCtrl : MonoBehaviour
{
	// Token: 0x06001BD2 RID: 7122 RVA: 0x00162114 File Offset: 0x00160314
	private IEnumerator OpenWindow()
	{
		while (this.dispItemListCount < this.dispItemList.Count)
		{
			if (this.dispItemListCount == this.openItemIndexCount)
			{
				ItemData itemData = this.dispItemList[this.dispItemListCount];
				ItemStaticBase itemStaticBase = DataManager.DmItem.GetItemStaticBase(itemData.id);
				this.Setup(itemStaticBase, itemData.num, (int index) => true, delegate
				{
					this.dispItemListCount++;
					if (this.dispItemListCount >= this.dispItemList.Count && this.setupParam.windowFinishedCallback != null)
					{
						this.setupParam.windowFinishedCallback(0);
					}
				});
				this.guiData.owCtrl.Open();
				this.openItemIndexCount++;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001BD3 RID: 7123 RVA: 0x00162123 File Offset: 0x00160323
	public void Init(Transform baseTr)
	{
		this.guiData = new GetItemWindowCtrl.GUI(baseTr);
		this.guiData.Img_item.gameObject.SetActive(false);
	}

	// Token: 0x06001BD4 RID: 7124 RVA: 0x00162148 File Offset: 0x00160348
	public void Setup(List<ItemData> list, GetItemWindowCtrl.SetupParam _setupParam)
	{
		this.setupParam = _setupParam;
		this.dispItemList = new List<ItemData>(list);
		this.dispItemListCount = (this.openItemIndexCount = 0);
		if (this.dispItemList.Count <= 0 && this.setupParam.windowFinishedCallback != null)
		{
			this.setupParam.windowFinishedCallback(0);
		}
	}

	// Token: 0x06001BD5 RID: 7125 RVA: 0x001621A5 File Offset: 0x001603A5
	public void Open()
	{
		this.openWindow = this.OpenWindow();
	}

	// Token: 0x06001BD6 RID: 7126 RVA: 0x001621B4 File Offset: 0x001603B4
	private void Setup(ItemStaticBase isb, int num, PguiOpenWindowCtrl.Callback cb, UnityAction finishedCloseCB)
	{
		this.DestroyInstItem();
		ItemDef.Kind kind = isb.GetKind();
		string text;
		if (kind != ItemDef.Kind.CHARA)
		{
			if (kind != ItemDef.Kind.PHOTO)
			{
				text = this.setupParam.strItemCb(new GetItemWindowCtrl.WordingCallbackParam
				{
					itemNum = num,
					itemStaticBase = isb
				});
				this.guiData.Icon_item01.SetActive(true);
				this.guiData.Icon_item02.SetActive(false);
				this.guiData.instItem = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, this.guiData.Icon_item01.transform);
			}
			else
			{
				text = this.setupParam.strPhotoCb(new GetItemWindowCtrl.WordingCallbackParam
				{
					itemStaticBase = isb,
					itemNum = num
				});
				this.guiData.Icon_item01.SetActive(false);
				this.guiData.Icon_item02.SetActive(true);
				this.guiData.instItem = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, this.guiData.Icon_item02.transform);
			}
		}
		else
		{
			text = this.setupParam.strCharaCb(new GetItemWindowCtrl.WordingCallbackParam
			{
				itemStaticBase = isb
			});
			this.guiData.Icon_item01.SetActive(false);
			this.guiData.Icon_item02.SetActive(true);
			this.guiData.instItem = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, this.guiData.Icon_item02.transform);
		}
		if (this.setupParam.isDispItemNum)
		{
			this.guiData.Txt_ItemNum.transform.parent.gameObject.SetActive(true);
			this.guiData.Txt_ItemNum.text = "×" + num.ToString();
		}
		else
		{
			this.guiData.Txt_ItemNum.transform.parent.gameObject.SetActive(false);
		}
		this.guiData.instItem.GetComponent<IconItemCtrl>().Setup(isb, new IconItemCtrl.SetupParam
		{
			enableIconScale = false
		});
		this.guiData.owCtrl.Setup(this.setupParam.strTitle, text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, cb, finishedCloseCB, false);
	}

	// Token: 0x06001BD7 RID: 7127 RVA: 0x001623E4 File Offset: 0x001605E4
	private void DestroyInstItem()
	{
		if (this.guiData.instItem != null)
		{
			Object.Destroy(this.guiData.instItem);
			this.guiData.instItem = null;
		}
	}

	// Token: 0x06001BD8 RID: 7128 RVA: 0x00162418 File Offset: 0x00160618
	private void Update()
	{
		if (this.openWindow != null && !this.openWindow.MoveNext())
		{
			if (this.setupParam.isBox && this.guiData.owCtrl.FinishedClose())
			{
				CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("確認"), PrjUtil.MakeMessage("持ちきれなかったアイテムは\nプレゼントに移動しました"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int index) => true, null, false);
				CanvasManager.HdlOpenWindowBasic.Open();
				this.setupParam.isBox = false;
			}
			this.openWindow = null;
		}
	}

	// Token: 0x040014B8 RID: 5304
	public GetItemWindowCtrl.GUI guiData;

	// Token: 0x040014B9 RID: 5305
	private GetItemWindowCtrl.SetupParam setupParam = new GetItemWindowCtrl.SetupParam();

	// Token: 0x040014BA RID: 5306
	private List<ItemData> dispItemList = new List<ItemData>();

	// Token: 0x040014BB RID: 5307
	private int dispItemListCount;

	// Token: 0x040014BC RID: 5308
	private int openItemIndexCount;

	// Token: 0x040014BD RID: 5309
	private IEnumerator openWindow;

	// Token: 0x02000EE0 RID: 3808
	// (Invoke) Token: 0x06004DFF RID: 19967
	public delegate string WordingCallback(GetItemWindowCtrl.WordingCallbackParam param);

	// Token: 0x02000EE1 RID: 3809
	public class SetupParam
	{
		// Token: 0x06004E02 RID: 19970 RVA: 0x00234EF8 File Offset: 0x002330F8
		public SetupParam()
		{
			this.strTitle = PrjUtil.MakeMessage("確認");
		}

		// Token: 0x0400550A RID: 21770
		public bool isBox;

		// Token: 0x0400550B RID: 21771
		public string strTitle;

		// Token: 0x0400550C RID: 21772
		public bool isDispItemNum;

		// Token: 0x0400550D RID: 21773
		public GetItemWindowCtrl.WordingCallback strCharaCb;

		// Token: 0x0400550E RID: 21774
		public GetItemWindowCtrl.WordingCallback strPhotoCb;

		// Token: 0x0400550F RID: 21775
		public GetItemWindowCtrl.WordingCallback strItemCb;

		// Token: 0x04005510 RID: 21776
		public PguiOpenWindowCtrl.Callback windowFinishedCallback;
	}

	// Token: 0x02000EE2 RID: 3810
	public class WordingCallbackParam
	{
		// Token: 0x04005511 RID: 21777
		public ItemStaticBase itemStaticBase;

		// Token: 0x04005512 RID: 21778
		public int itemNum;
	}

	// Token: 0x02000EE3 RID: 3811
	public class GUI
	{
		// Token: 0x06004E04 RID: 19972 RVA: 0x00234F18 File Offset: 0x00233118
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Icon_item01 = baseTr.Find("Base/Window/Icon_Item01").gameObject;
			this.Icon_item02 = baseTr.Find("Base/Window/Icon_Item02").gameObject;
			this.Txt_ItemNum = baseTr.Find("Base/Window/Txt_Window/Num_Txt").GetComponent<PguiTextCtrl>();
			this.Img_item = baseTr.Find("Base/Window/Icon_Item01/Icon_Item").GetComponent<PguiRawImageCtrl>();
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
		}

		// Token: 0x04005513 RID: 21779
		public PguiOpenWindowCtrl owCtrl;

		// Token: 0x04005514 RID: 21780
		public GameObject baseObj;

		// Token: 0x04005515 RID: 21781
		public GameObject Icon_item01;

		// Token: 0x04005516 RID: 21782
		public GameObject Icon_item02;

		// Token: 0x04005517 RID: 21783
		public GameObject instItem;

		// Token: 0x04005518 RID: 21784
		public PguiTextCtrl Txt_ItemNum;

		// Token: 0x04005519 RID: 21785
		public PguiRawImageCtrl Img_item;
	}
}
