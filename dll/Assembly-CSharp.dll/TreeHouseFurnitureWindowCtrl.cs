using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TreeHouseFurnitureWindowCtrl : MonoBehaviour
{
	public void Initialize()
	{
		this.guiData = new TreeHouseFurnitureWindowCtrl.GUI(base.transform);
		this.guiData.Btn_Yaji_Left.gameObject.SetActive(false);
		this.guiData.Btn_Yaji_Right.gameObject.SetActive(false);
	}

	public void Open(TreeHouseFurnitureWindowCtrl.SetupParam setupParam)
	{
		this.windowCloseEndCb = setupParam.closeEndCb;
		this.DispTreeHouseFurnitureStatic = setupParam.thfs;
		this.ChangeFurniture();
		this.guiData.owCtrl.Open();
		UnityAction openEndCb = setupParam.openEndCb;
		if (openEndCb == null)
		{
			return;
		}
		openEndCb();
	}

	private bool OnClickOwButton(int index)
	{
		if (-1 == index)
		{
			UnityAction unityAction = this.windowCloseEndCb;
			if (unityAction != null)
			{
				unityAction();
			}
			this.windowCloseEndCb = null;
			return true;
		}
		return false;
	}

	private void ChangeFurniture()
	{
		TreeHouseFurnitureStatic.Category category = this.DispTreeHouseFurnitureStatic.category;
		if (category != TreeHouseFurnitureStatic.Category.STAND)
		{
			if (category != TreeHouseFurnitureStatic.Category.WALL_HANGINGS)
			{
				this.guiData.Obj_Set01.SetActive(true);
				this.guiData.Obj_Set02.SetActive(false);
				this.guiData.Txt_Set01HeightOrDepthTitle.text = "高さ";
			}
			else
			{
				this.guiData.Obj_Set01.SetActive(true);
				this.guiData.Obj_Set02.SetActive(false);
				this.guiData.Txt_Set01HeightOrDepthTitle.text = "奥行";
			}
		}
		else
		{
			this.guiData.Obj_Set01.SetActive(false);
			this.guiData.Obj_Set02.SetActive(true);
		}
		string text = "-";
		if (0 < this.DispTreeHouseFurnitureStatic.sizeX && 0 < this.DispTreeHouseFurnitureStatic.sizeY)
		{
			text = this.DispTreeHouseFurnitureStatic.sizeX.ToString() + "×" + this.DispTreeHouseFurnitureStatic.sizeY.ToString();
		}
		string text2 = ((0 < this.DispTreeHouseFurnitureStatic.sizeHeightOrDepth) ? this.DispTreeHouseFurnitureStatic.sizeHeightOrDepth.ToString() : "-");
		if (this.guiData.Obj_Set01.activeSelf)
		{
			this.guiData.Txt_Set01Mass.text = text;
			this.guiData.Txt_Set01HeightOrDepth.text = text2;
		}
		if (this.guiData.Obj_Set02.activeSelf)
		{
			this.guiData.Txt_Set02Mass.text = text;
			this.guiData.Txt_Set02Height.text = text2;
			this.guiData.Txt_Set02GoodsNum.text = ((0 < this.DispTreeHouseFurnitureStatic.locatorGoods) ? this.DispTreeHouseFurnitureStatic.locatorGoods.ToString() : "-");
		}
		this.guiData.Txt_Type.text = this.DispTreeHouseFurnitureStatic.GetCategoryName();
		string text3 = "-";
		if (2 < this.DispTreeHouseFurnitureStatic.charaCountReaction)
		{
			text3 = "2～" + this.DispTreeHouseFurnitureStatic.charaCountReaction.ToString() + "人";
		}
		else if (0 < this.DispTreeHouseFurnitureStatic.charaCountReaction)
		{
			text3 = this.DispTreeHouseFurnitureStatic.charaCountReaction.ToString() + "人";
		}
		this.guiData.Txt_PlayTitle.text = "遊べる人数";
		this.guiData.Txt_PlayNum.text = text3;
		if (!string.IsNullOrEmpty(this.DispTreeHouseFurnitureStatic.bgmFilepath))
		{
			this.guiData.Txt_PlayTitle.text = "追加される楽曲";
			if (this.DispTreeHouseFurnitureStatic.bgmName.Length > 20)
			{
				this.guiData.Txt_PlayNum.m_Text.fontSize = 28 - (this.DispTreeHouseFurnitureStatic.bgmName.Length - 20);
			}
			this.guiData.Txt_PlayNum.text = this.DispTreeHouseFurnitureStatic.bgmName;
		}
		this.guiData.Txt_BonusNum.text = this.DispTreeHouseFurnitureStatic.kizunaBonusExp.ToString();
		this.guiData.iconTreeHouseFurnitureCtrl.Setup(new IconTreeHouseFurnitureCtrl.SetupParam
		{
			thfs = this.DispTreeHouseFurnitureStatic
		});
		this.guiData.owCtrl.Setup(this.DispTreeHouseFurnitureStatic.GetName(), null, null, true, new PguiOpenWindowCtrl.Callback(this.OnClickOwButton), null, false);
	}

	public TreeHouseFurnitureWindowCtrl.GUI guiData;

	private IEnumerator IEWindowMove;

	private UnityAction windowCloseEndCb;

	private TreeHouseFurnitureStatic DispTreeHouseFurnitureStatic;

	public class GUI
	{
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.Txt_Title = baseTr.Find("Base/Window/Title").GetComponent<PguiTextCtrl>();
			this.Obj_Set01 = baseTr.Find("Base/Window/InfoBase/Set01").gameObject;
			this.Txt_Set01Mass = baseTr.Find("Base/Window/InfoBase/Set01/Mass/Num").GetComponent<PguiTextCtrl>();
			this.Txt_Set01HeightOrDepth = baseTr.Find("Base/Window/InfoBase/Set01/Hgight_Depth/Num").GetComponent<PguiTextCtrl>();
			this.Txt_Set01HeightOrDepthTitle = baseTr.Find("Base/Window/InfoBase/Set01/Hgight_Depth/Title").GetComponent<PguiTextCtrl>();
			this.Obj_Set02 = baseTr.Find("Base/Window/InfoBase/Set02").gameObject;
			this.Txt_Set02Mass = baseTr.Find("Base/Window/InfoBase/Set02/Mass/Num").GetComponent<PguiTextCtrl>();
			this.Txt_Set02Height = baseTr.Find("Base/Window/InfoBase/Set02/Hgight_Depth/Num").GetComponent<PguiTextCtrl>();
			this.Txt_Set02GoodsNum = baseTr.Find("Base/Window/InfoBase/Set02/GoodsNum/Num").GetComponent<PguiTextCtrl>();
			this.Txt_Type = baseTr.Find("Base/Window/InfoBase/Type/Num").GetComponent<PguiTextCtrl>();
			this.Txt_PlayTitle = baseTr.Find("Base/Window/InfoBase/PlayNum/Title").GetComponent<PguiTextCtrl>();
			this.Txt_PlayNum = baseTr.Find("Base/Window/InfoBase/PlayNum/Num").GetComponent<PguiTextCtrl>();
			this.Txt_BonusNum = baseTr.Find("Base/Window/InfoBase/Bonus/Num").GetComponent<PguiTextCtrl>();
			this.Btn_Yaji_Left = baseTr.Find("Base/Window/Btn_Yaji_Left").GetComponent<PguiButtonCtrl>();
			this.Btn_Yaji_Right = baseTr.Find("Base/Window/Btn_Yaji_Right").GetComponent<PguiButtonCtrl>();
			this.iconTreeHouseFurnitureCtrl = baseTr.Find("Base/Window/CaedInterior/Card_Interior").GetComponent<IconTreeHouseFurnitureCtrl>();
		}

		public GameObject baseObj;

		public PguiOpenWindowCtrl owCtrl;

		public PguiTextCtrl Txt_Title;

		public GameObject Obj_Set01;

		public PguiTextCtrl Txt_Set01Mass;

		public PguiTextCtrl Txt_Set01HeightOrDepth;

		public PguiTextCtrl Txt_Set01HeightOrDepthTitle;

		public GameObject Obj_Set02;

		public PguiTextCtrl Txt_Set02Mass;

		public PguiTextCtrl Txt_Set02Height;

		public PguiTextCtrl Txt_Set02GoodsNum;

		public PguiTextCtrl Txt_Type;

		public PguiTextCtrl Txt_PlayTitle;

		public PguiTextCtrl Txt_PlayNum;

		public PguiTextCtrl Txt_BonusNum;

		public PguiButtonCtrl Btn_Yaji_Left;

		public PguiButtonCtrl Btn_Yaji_Right;

		public IconTreeHouseFurnitureCtrl iconTreeHouseFurnitureCtrl;
	}

	public class SetupParam
	{
		public SetupParam()
		{
			this.openEndCb = null;
			this.closeEndCb = null;
			this.thfs = null;
		}

		public UnityAction openEndCb;

		public UnityAction closeEndCb;

		public TreeHouseFurnitureStatic thfs;
	}
}
