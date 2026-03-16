using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GetItemWindowCtrl : MonoBehaviour
{
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

	public void Init(Transform baseTr)
	{
		this.guiData = new GetItemWindowCtrl.GUI(baseTr);
		this.guiData.Img_item.gameObject.SetActive(false);
	}

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

	public void Open()
	{
		this.openWindow = this.OpenWindow();
	}

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

	private void DestroyInstItem()
	{
		if (this.guiData.instItem != null)
		{
			Object.Destroy(this.guiData.instItem);
			this.guiData.instItem = null;
		}
	}

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

	public GetItemWindowCtrl.GUI guiData;

	private GetItemWindowCtrl.SetupParam setupParam = new GetItemWindowCtrl.SetupParam();

	private List<ItemData> dispItemList = new List<ItemData>();

	private int dispItemListCount;

	private int openItemIndexCount;

	private IEnumerator openWindow;

	public delegate string WordingCallback(GetItemWindowCtrl.WordingCallbackParam param);

	public class SetupParam
	{
		public SetupParam()
		{
			this.strTitle = PrjUtil.MakeMessage("確認");
		}

		public bool isBox;

		public string strTitle;

		public bool isDispItemNum;

		public GetItemWindowCtrl.WordingCallback strCharaCb;

		public GetItemWindowCtrl.WordingCallback strPhotoCb;

		public GetItemWindowCtrl.WordingCallback strItemCb;

		public PguiOpenWindowCtrl.Callback windowFinishedCallback;
	}

	public class WordingCallbackParam
	{
		public ItemStaticBase itemStaticBase;

		public int itemNum;
	}

	public class GUI
	{
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Icon_item01 = baseTr.Find("Base/Window/Icon_Item01").gameObject;
			this.Icon_item02 = baseTr.Find("Base/Window/Icon_Item02").gameObject;
			this.Txt_ItemNum = baseTr.Find("Base/Window/Txt_Window/Num_Txt").GetComponent<PguiTextCtrl>();
			this.Img_item = baseTr.Find("Base/Window/Icon_Item01/Icon_Item").GetComponent<PguiRawImageCtrl>();
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
		}

		public PguiOpenWindowCtrl owCtrl;

		public GameObject baseObj;

		public GameObject Icon_item01;

		public GameObject Icon_item02;

		public GameObject instItem;

		public PguiTextCtrl Txt_ItemNum;

		public PguiRawImageCtrl Img_item;
	}
}
