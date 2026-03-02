using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001C6 RID: 454
public class TreeHouseFurnitureWindowCtrl : MonoBehaviour
{
	// Token: 0x06001F3E RID: 7998 RVA: 0x0018308A File Offset: 0x0018128A
	public void Initialize()
	{
		this.guiData = new TreeHouseFurnitureWindowCtrl.GUI(base.transform);
		this.guiData.Btn_Yaji_Left.gameObject.SetActive(false);
		this.guiData.Btn_Yaji_Right.gameObject.SetActive(false);
	}

	// Token: 0x06001F3F RID: 7999 RVA: 0x001830C9 File Offset: 0x001812C9
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

	// Token: 0x06001F40 RID: 8000 RVA: 0x00183109 File Offset: 0x00181309
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

	// Token: 0x06001F41 RID: 8001 RVA: 0x0018312C File Offset: 0x0018132C
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

	// Token: 0x040016B5 RID: 5813
	public TreeHouseFurnitureWindowCtrl.GUI guiData;

	// Token: 0x040016B6 RID: 5814
	private IEnumerator IEWindowMove;

	// Token: 0x040016B7 RID: 5815
	private UnityAction windowCloseEndCb;

	// Token: 0x040016B8 RID: 5816
	private TreeHouseFurnitureStatic DispTreeHouseFurnitureStatic;

	// Token: 0x02001002 RID: 4098
	public class GUI
	{
		// Token: 0x060051AD RID: 20909 RVA: 0x0024733C File Offset: 0x0024553C
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

		// Token: 0x040059D4 RID: 22996
		public GameObject baseObj;

		// Token: 0x040059D5 RID: 22997
		public PguiOpenWindowCtrl owCtrl;

		// Token: 0x040059D6 RID: 22998
		public PguiTextCtrl Txt_Title;

		// Token: 0x040059D7 RID: 22999
		public GameObject Obj_Set01;

		// Token: 0x040059D8 RID: 23000
		public PguiTextCtrl Txt_Set01Mass;

		// Token: 0x040059D9 RID: 23001
		public PguiTextCtrl Txt_Set01HeightOrDepth;

		// Token: 0x040059DA RID: 23002
		public PguiTextCtrl Txt_Set01HeightOrDepthTitle;

		// Token: 0x040059DB RID: 23003
		public GameObject Obj_Set02;

		// Token: 0x040059DC RID: 23004
		public PguiTextCtrl Txt_Set02Mass;

		// Token: 0x040059DD RID: 23005
		public PguiTextCtrl Txt_Set02Height;

		// Token: 0x040059DE RID: 23006
		public PguiTextCtrl Txt_Set02GoodsNum;

		// Token: 0x040059DF RID: 23007
		public PguiTextCtrl Txt_Type;

		// Token: 0x040059E0 RID: 23008
		public PguiTextCtrl Txt_PlayTitle;

		// Token: 0x040059E1 RID: 23009
		public PguiTextCtrl Txt_PlayNum;

		// Token: 0x040059E2 RID: 23010
		public PguiTextCtrl Txt_BonusNum;

		// Token: 0x040059E3 RID: 23011
		public PguiButtonCtrl Btn_Yaji_Left;

		// Token: 0x040059E4 RID: 23012
		public PguiButtonCtrl Btn_Yaji_Right;

		// Token: 0x040059E5 RID: 23013
		public IconTreeHouseFurnitureCtrl iconTreeHouseFurnitureCtrl;
	}

	// Token: 0x02001003 RID: 4099
	public class SetupParam
	{
		// Token: 0x060051AE RID: 20910 RVA: 0x002474C7 File Offset: 0x002456C7
		public SetupParam()
		{
			this.openEndCb = null;
			this.closeEndCb = null;
			this.thfs = null;
		}

		// Token: 0x040059E6 RID: 23014
		public UnityAction openEndCb;

		// Token: 0x040059E7 RID: 23015
		public UnityAction closeEndCb;

		// Token: 0x040059E8 RID: 23016
		public TreeHouseFurnitureStatic thfs;
	}
}
