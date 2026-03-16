using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using UnityEngine;

public class SelCharaGrowRank
{
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

	public void UpdateItemRankUp(int charaId)
	{
		this.SetupItemRank(charaId);
	}

	public SelCharaGrowRank.CharaGrowRankGUI GrowRankGUI;

	public class RankUpItem
	{
		public RankUpItem(GrowItemData data)
		{
			this.itemStatic = DataManager.DmItem.GetItemStaticBase(data.item.id);
			this.itemOwnNum = DataManager.DmItem.GetUserItemData(data.item.id).num;
			this.itemNeedNum = data.item.num;
		}

		public ItemStaticBase itemStatic;

		public int itemOwnNum;

		public int itemNeedNum;
	}

	public class RankUpTab
	{
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

		public GameObject baseObj;

		public PguiTextCtrl Txt_ItemName;

		public PguiTextCtrl Num_Item;

		public GameObject Icon_Item;

		public IconItemCtrl ItemIconCtrl;

		public PguiImageCtrl GageAll;
	}

	public class WindowRankUp
	{
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

		public PguiOpenWindowCtrl owCtrl;

		public IconCharaCtrl iconChara;

		public GameObject iconCharaObject;

		public PguiTextCtrl Txt_CharaName;

		public PguiTextCtrl UseItem_Num_Before;

		public PguiTextCtrl UseItem_Num_After;

		public PguiTextCtrl UseCoin_Num_Before;

		public PguiTextCtrl UseCoin_Num_After;

		public List<PguiImageCtrl> StarAll;

		public List<PguiImageCtrl> StarAddAll;

		public List<PguiTextCtrl> ParamAll;
	}

	public class WindowRankUpResult
	{
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

		public PguiOpenWindowCtrl owCtrl;

		public IconCharaCtrl iconChara;

		public PguiTextCtrl Txt_CharaName;

		public List<PguiImageCtrl> StarAll;

		public List<PguiTextCtrl> ParamAll;

		public PguiTextCtrl Txt_GetInfo;
	}

	public class RankUpAuth
	{
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

		private IEnumerator SetFacePack()
		{
			while (!this.rtc.FinishedSetup)
			{
				yield return null;
			}
			this.rtc.SetFacePack(FacePackData.Id2PackData("FACE_SMILE_2_B"));
			yield break;
		}

		public void Teardown()
		{
			if (this.rtc != null)
			{
				Object.Destroy(this.rtc.gameObject);
				this.rtc = null;
			}
		}

		public GameObject baseObj;

		public PguiImageCtrl Bg;

		public GameObject RenderTexture;

		public PguiTextCtrl Txt_Touch;

		public PguiAECtrl AEImage_Bg;

		public PguiAECtrl AEImage_Back;

		public PguiAECtrl AEImage_RankUp;

		public List<PguiAECtrl> AEImage_StarAll;

		public RenderTextureChara rtc;

		public enum SIZE
		{
			SIZE_S,
			SIZE_M,
			SIZE_L
		}
	}

	public class CharaGrowRankGUI
	{
		public SelCharaGrowRank.RankUpAuth rankUpAuth;

		public SelCharaGrowRank.WindowRankUp rankUpWindow;

		public SelCharaGrowRank.WindowRankUpResult rankUpResultWindow;

		public SelCharaGrowRank.RankUpTab rankUpTab;
	}
}
