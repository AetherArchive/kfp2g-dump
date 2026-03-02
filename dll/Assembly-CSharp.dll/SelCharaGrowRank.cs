using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using UnityEngine;

// Token: 0x02000136 RID: 310
public class SelCharaGrowRank
{
	// Token: 0x06001078 RID: 4216 RVA: 0x000C872C File Offset: 0x000C692C
	public SelCharaGrowRank(Transform baseTr)
	{
		this.GrowRankGUI = new SelCharaGrowRank.CharaGrowRankGUI();
		GameObject gameObject = (GameObject)Resources.Load("SceneCharaEdit/GUI/Prefab/GUI_CharaEdit_Window_Rank");
		this.GrowRankGUI.rankUpAuth = new SelCharaGrowRank.RankUpAuth(Object.Instantiate<Transform>(gameObject.transform.Find("CharaRankUpAuth"), baseTr).transform);
		this.GrowRankGUI.rankUpAuth.baseObj.SetActive(false);
		this.GrowRankGUI.rankUpWindow = new SelCharaGrowRank.WindowRankUp(Object.Instantiate<Transform>(gameObject.transform.Find("Window_RankUp"), baseTr).transform);
		this.GrowRankGUI.rankUpResultWindow = new SelCharaGrowRank.WindowRankUpResult(Object.Instantiate<Transform>(gameObject.transform.Find("Window_RankUp_After"), baseTr).transform);
		this.GrowRankGUI.rankUpTab = new SelCharaGrowRank.RankUpTab(baseTr.Find("CharaGrow_Main").Find("Main/Right/RankUp"));
	}

	// Token: 0x06001079 RID: 4217 RVA: 0x000C8818 File Offset: 0x000C6A18
	public void SetupItemRank(int charaId)
	{
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(charaId);
		GrowItemData nextItemByRankup = userCharaData.GetNextItemByRankup(0);
		if (nextItemByRankup != null)
		{
			SelCharaGrowRank.RankUpItem rankUpItem = new SelCharaGrowRank.RankUpItem(nextItemByRankup);
			this.GrowRankGUI.rankUpTab.GageAll.m_Image.fillAmount = (float)rankUpItem.itemOwnNum / (float)rankUpItem.itemNeedNum;
			this.GrowRankGUI.rankUpTab.Num_Item.text = ((rankUpItem.itemOwnNum >= rankUpItem.itemNeedNum) ? string.Format("{0}/{1}", rankUpItem.itemOwnNum, rankUpItem.itemNeedNum) : string.Format("{0}{1}{2}/{3}", new object[]
			{
				PrjUtil.ColorRedStartTag,
				rankUpItem.itemOwnNum,
				PrjUtil.ColorEndTag,
				rankUpItem.itemNeedNum
			}));
			this.GrowRankGUI.rankUpTab.ItemIconCtrl.Setup(rankUpItem.itemStatic);
			this.GrowRankGUI.rankUpTab.Txt_ItemName.text = rankUpItem.itemStatic.GetName();
			return;
		}
		ItemStaticBase itemStaticBase = DataManager.DmItem.GetItemStaticBase(userCharaData.staticData.baseData.rankItemId);
		this.GrowRankGUI.rankUpTab.GageAll.m_Image.fillAmount = 1f;
		this.GrowRankGUI.rankUpTab.Num_Item.text = "-";
		this.GrowRankGUI.rankUpTab.ItemIconCtrl.Setup(itemStaticBase);
		this.GrowRankGUI.rankUpTab.Txt_ItemName.text = itemStaticBase.GetName();
	}

	// Token: 0x0600107A RID: 4218 RVA: 0x000C89B1 File Offset: 0x000C6BB1
	public void UpdateItemRankUp(int charaId)
	{
		this.SetupItemRank(charaId);
	}

	// Token: 0x04000E56 RID: 3670
	public SelCharaGrowRank.CharaGrowRankGUI GrowRankGUI;

	// Token: 0x02000A20 RID: 2592
	public class RankUpItem
	{
		// Token: 0x06003E6E RID: 15982 RVA: 0x001E9878 File Offset: 0x001E7A78
		public RankUpItem(GrowItemData data)
		{
			this.itemStatic = DataManager.DmItem.GetItemStaticBase(data.item.id);
			this.itemOwnNum = DataManager.DmItem.GetUserItemData(data.item.id).num;
			this.itemNeedNum = data.item.num;
		}

		// Token: 0x040040D1 RID: 16593
		public ItemStaticBase itemStatic;

		// Token: 0x040040D2 RID: 16594
		public int itemOwnNum;

		// Token: 0x040040D3 RID: 16595
		public int itemNeedNum;
	}

	// Token: 0x02000A21 RID: 2593
	public class RankUpTab
	{
		// Token: 0x06003E6F RID: 15983 RVA: 0x001E98D8 File Offset: 0x001E7AD8
		public RankUpTab(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Txt_ItemName = baseTr.Find("Txt_ItemName").GetComponent<PguiTextCtrl>();
			this.Num_Item = baseTr.Find("Num_Item").GetComponent<PguiTextCtrl>();
			this.Icon_Item = baseTr.Find("Icon_Item").gameObject;
			this.GageAll = baseTr.Find("GageAll/Gage").GetComponent<PguiImageCtrl>();
			this.ItemIconCtrl = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, this.Icon_Item.transform).GetComponent<IconItemCtrl>();
			if (this.Icon_Item.transform.Find("Icon_Item") != null)
			{
				this.Icon_Item.transform.Find("Icon_Item").gameObject.SetActive(false);
			}
		}

		// Token: 0x040040D4 RID: 16596
		public GameObject baseObj;

		// Token: 0x040040D5 RID: 16597
		public PguiTextCtrl Txt_ItemName;

		// Token: 0x040040D6 RID: 16598
		public PguiTextCtrl Num_Item;

		// Token: 0x040040D7 RID: 16599
		public GameObject Icon_Item;

		// Token: 0x040040D8 RID: 16600
		public IconItemCtrl ItemIconCtrl;

		// Token: 0x040040D9 RID: 16601
		public PguiImageCtrl GageAll;
	}

	// Token: 0x02000A22 RID: 2594
	public class WindowRankUp
	{
		// Token: 0x06003E70 RID: 15984 RVA: 0x001E99B4 File Offset: 0x001E7BB4
		public WindowRankUp(Transform baseTr)
		{
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("CmnIconFrame/GUI/Prefab/Icon_Chara"), baseTr.Find("Base/Window/Icon_Chara"));
			this.iconChara = gameObject.GetComponent<IconCharaCtrl>();
			this.iconCharaObject = baseTr.Find("Base/Window/Icon_Chara").gameObject;
			this.UseItem_Num_Before = baseTr.Find("Base/Window/UseItem/Img_Yaji/Num_BeforeTxt").GetComponent<PguiTextCtrl>();
			this.UseItem_Num_After = baseTr.Find("Base/Window/UseItem/Img_Yaji/Num_AfterTxt").GetComponent<PguiTextCtrl>();
			this.UseCoin_Num_Before = baseTr.Find("Base/Window/UseCoin/Img_Yaji/Num_BeforeTxt").GetComponent<PguiTextCtrl>();
			this.UseCoin_Num_After = baseTr.Find("Base/Window/UseCoin/Img_Yaji/Num_AfterTxt").GetComponent<PguiTextCtrl>();
			this.Txt_CharaName = baseTr.Find("Base/Window/Txt_CharaName").GetComponent<PguiTextCtrl>();
			this.StarAll = new List<PguiImageCtrl>();
			this.StarAddAll = new List<PguiImageCtrl>();
			for (int i = 0; i < 6; i++)
			{
				this.StarAll.Add(baseTr.Find("Base/Window/Icon_StarAll/Icon_Star" + (i + 1).ToString("D2")).GetComponent<PguiImageCtrl>());
				this.StarAddAll.Add(baseTr.Find("Base/Window/Icon_StarAll/Icon_Star" + (i + 1).ToString("D2") + "/Icon_Add").GetComponent<PguiImageCtrl>());
			}
			this.ParamAll = new List<PguiTextCtrl>();
			for (int j = 0; j < 4; j++)
			{
				this.ParamAll.Add(baseTr.Find("Base/Window/ParamAll/Info" + (j + 1).ToString("D2") + "/Num_After").GetComponent<PguiTextCtrl>());
			}
			baseTr.Find("Base/Window/ParamAll/Info05").gameObject.SetActive(false);
		}

		// Token: 0x040040DA RID: 16602
		public PguiOpenWindowCtrl owCtrl;

		// Token: 0x040040DB RID: 16603
		public IconCharaCtrl iconChara;

		// Token: 0x040040DC RID: 16604
		public GameObject iconCharaObject;

		// Token: 0x040040DD RID: 16605
		public PguiTextCtrl Txt_CharaName;

		// Token: 0x040040DE RID: 16606
		public PguiTextCtrl UseItem_Num_Before;

		// Token: 0x040040DF RID: 16607
		public PguiTextCtrl UseItem_Num_After;

		// Token: 0x040040E0 RID: 16608
		public PguiTextCtrl UseCoin_Num_Before;

		// Token: 0x040040E1 RID: 16609
		public PguiTextCtrl UseCoin_Num_After;

		// Token: 0x040040E2 RID: 16610
		public List<PguiImageCtrl> StarAll;

		// Token: 0x040040E3 RID: 16611
		public List<PguiImageCtrl> StarAddAll;

		// Token: 0x040040E4 RID: 16612
		public List<PguiTextCtrl> ParamAll;
	}

	// Token: 0x02000A23 RID: 2595
	public class WindowRankUpResult
	{
		// Token: 0x06003E71 RID: 15985 RVA: 0x001E9B6C File Offset: 0x001E7D6C
		public WindowRankUpResult(Transform baseTr)
		{
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("CmnIconFrame/GUI/Prefab/Icon_Chara"), baseTr.Find("Base/Window/Icon_Chara"));
			this.iconChara = gameObject.GetComponent<IconCharaCtrl>();
			this.Txt_CharaName = baseTr.Find("Base/Window/Txt_CharaName").GetComponent<PguiTextCtrl>();
			this.StarAll = new List<PguiImageCtrl>();
			for (int i = 0; i < 6; i++)
			{
				this.StarAll.Add(baseTr.Find("Base/Window/Icon_StarAll/Icon_Star" + (i + 1).ToString("D2")).GetComponent<PguiImageCtrl>());
			}
			this.ParamAll = new List<PguiTextCtrl>();
			for (int j = 0; j < 4; j++)
			{
				this.ParamAll.Add(baseTr.Find("Base/Window/ParamAll/Info" + (j + 1).ToString("D2") + "/Num_After").GetComponent<PguiTextCtrl>());
			}
			baseTr.Find("Base/Window/ParamAll/Info05").gameObject.SetActive(false);
			this.Txt_GetInfo = baseTr.Find("Base/Window/Txt_GetInfo").GetComponent<PguiTextCtrl>();
		}

		// Token: 0x040040E5 RID: 16613
		public PguiOpenWindowCtrl owCtrl;

		// Token: 0x040040E6 RID: 16614
		public IconCharaCtrl iconChara;

		// Token: 0x040040E7 RID: 16615
		public PguiTextCtrl Txt_CharaName;

		// Token: 0x040040E8 RID: 16616
		public List<PguiImageCtrl> StarAll;

		// Token: 0x040040E9 RID: 16617
		public List<PguiTextCtrl> ParamAll;

		// Token: 0x040040EA RID: 16618
		public PguiTextCtrl Txt_GetInfo;
	}

	// Token: 0x02000A24 RID: 2596
	public class RankUpAuth
	{
		// Token: 0x06003E72 RID: 15986 RVA: 0x001E9C8C File Offset: 0x001E7E8C
		public RankUpAuth(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Bg = baseTr.Find("AEImage_Bg/Bg_Pattern/Bg").GetComponent<PguiImageCtrl>();
			this.RenderTexture = baseTr.Find("RenderTexture").gameObject;
			this.Txt_Touch = baseTr.Find("Txt_Touch").GetComponent<PguiTextCtrl>();
			this.AEImage_Bg = baseTr.Find("AEImage_Bg").GetComponent<PguiAECtrl>();
			this.AEImage_Back = baseTr.Find("AEImage_Back").GetComponent<PguiAECtrl>();
			this.AEImage_RankUp = baseTr.Find("AEImage_RankUp").GetComponent<PguiAECtrl>();
			this.AEImage_StarAll = new List<PguiAECtrl>();
			for (int i = 0; i < 6; i++)
			{
				this.AEImage_StarAll.Add(baseTr.Find("Icon_StarAll/Icon_Star" + (i + 1).ToString("D2") + "/AEImage").GetComponent<PguiAECtrl>());
			}
		}

		// Token: 0x06003E73 RID: 15987 RVA: 0x001E9D7C File Offset: 0x001E7F7C
		public void Setup(int currentCharaId, SelCharaGrowRank.RankUpAuth.SIZE size, int rtcLayer, int beforeRank = 0)
		{
			SoundManager.Play("prd_se_friends_rankup", false, false);
			CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(currentCharaId);
			this.AEImage_Bg.gameObject.SetActive(true);
			this.AEImage_Bg.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
			{
				this.AEImage_Bg.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
			});
			this.AEImage_Back.gameObject.SetActive(true);
			this.AEImage_Back.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
			{
				this.AEImage_Back.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
			});
			this.AEImage_RankUp.gameObject.SetActive(true);
			PguiReplaceAECtrl component = this.AEImage_RankUp.GetComponent<PguiReplaceAECtrl>();
			string text;
			switch (size)
			{
			case SelCharaGrowRank.RankUpAuth.SIZE.SIZE_S:
				text = "S";
				break;
			case SelCharaGrowRank.RankUpAuth.SIZE.SIZE_M:
				text = "M";
				break;
			case SelCharaGrowRank.RankUpAuth.SIZE.SIZE_L:
				text = "L";
				break;
			default:
				text = "S";
				break;
			}
			component.Replace(text);
			this.AEImage_RankUp.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
			{
				this.AEImage_RankUp.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
			});
			if (beforeRank == 0)
			{
				beforeRank = userCharaData.dynamicData.rank - 1;
			}
			for (int i = 0; i < this.AEImage_StarAll.Count; i++)
			{
				PguiAECtrl AEimgStar = this.AEImage_StarAll[i];
				AEimgStar.transform.parent.gameObject.SetActive(i < userCharaData.staticData.baseData.rankHigh);
				PguiReplaceAECtrl component2 = AEimgStar.m_AEImage.GetComponent<PguiReplaceAECtrl>();
				if (i < beforeRank)
				{
					component2.Replace("NORMAL");
				}
				else if (beforeRank <= i && i < userCharaData.dynamicData.rank)
				{
					component2.Replace("ADD");
				}
				else
				{
					component2.Replace("BLANK");
				}
				AEimgStar.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
				{
					AEimgStar.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
				});
			}
			if (this.rtc != null)
			{
				Object.Destroy(this.rtc.gameObject);
				this.rtc = null;
			}
			this.rtc = Object.Instantiate<GameObject>((GameObject)Resources.Load("RenderTextureChara/Prefab/RenderTextureCharaCtrl"), this.RenderTexture.transform).GetComponent<RenderTextureChara>();
			this.rtc.postion = new Vector2(0f, 0f);
			this.rtc.fieldOfView = 18f;
			this.rtc.transform.SetSiblingIndex(1);
			this.rtc.Setup(userCharaData, rtcLayer, CharaMotionDefine.ActKey.JOY, false, delegate
			{
				this.rtc.SetAnimation(CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true);
			}, false, null, 0f, null, false);
			Singleton<SceneManager>.Instance.StartCoroutine(this.SetFacePack());
		}

		// Token: 0x06003E74 RID: 15988 RVA: 0x001EA006 File Offset: 0x001E8206
		private IEnumerator SetFacePack()
		{
			while (!this.rtc.FinishedSetup)
			{
				yield return null;
			}
			this.rtc.SetFacePack(FacePackData.Id2PackData("FACE_SMILE_2_B"));
			yield break;
		}

		// Token: 0x06003E75 RID: 15989 RVA: 0x001EA015 File Offset: 0x001E8215
		public void Teardown()
		{
			if (this.rtc != null)
			{
				Object.Destroy(this.rtc.gameObject);
				this.rtc = null;
			}
		}

		// Token: 0x040040EB RID: 16619
		public GameObject baseObj;

		// Token: 0x040040EC RID: 16620
		public PguiImageCtrl Bg;

		// Token: 0x040040ED RID: 16621
		public GameObject RenderTexture;

		// Token: 0x040040EE RID: 16622
		public PguiTextCtrl Txt_Touch;

		// Token: 0x040040EF RID: 16623
		public PguiAECtrl AEImage_Bg;

		// Token: 0x040040F0 RID: 16624
		public PguiAECtrl AEImage_Back;

		// Token: 0x040040F1 RID: 16625
		public PguiAECtrl AEImage_RankUp;

		// Token: 0x040040F2 RID: 16626
		public List<PguiAECtrl> AEImage_StarAll;

		// Token: 0x040040F3 RID: 16627
		public RenderTextureChara rtc;

		// Token: 0x02001169 RID: 4457
		public enum SIZE
		{
			// Token: 0x04005F8F RID: 24463
			SIZE_S,
			// Token: 0x04005F90 RID: 24464
			SIZE_M,
			// Token: 0x04005F91 RID: 24465
			SIZE_L
		}
	}

	// Token: 0x02000A25 RID: 2597
	public class CharaGrowRankGUI
	{
		// Token: 0x040040F4 RID: 16628
		public SelCharaGrowRank.RankUpAuth rankUpAuth;

		// Token: 0x040040F5 RID: 16629
		public SelCharaGrowRank.WindowRankUp rankUpWindow;

		// Token: 0x040040F6 RID: 16630
		public SelCharaGrowRank.WindowRankUpResult rankUpResultWindow;

		// Token: 0x040040F7 RID: 16631
		public SelCharaGrowRank.RankUpTab rankUpTab;
	}
}
