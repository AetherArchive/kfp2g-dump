using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000135 RID: 309
public class SelCharaGrowPhotoPocket
{
	// Token: 0x06001075 RID: 4213 RVA: 0x000C84D4 File Offset: 0x000C66D4
	public SelCharaGrowPhotoPocket(Transform baseTr)
	{
		this.GrowPhotoPocketGUI = new SelCharaGrowPhotoPocket.CharaGrowPhotoPocketGUI();
		GameObject gameObject = (GameObject)Resources.Load("SceneCharaEdit/GUI/Prefab/GUI_CharaEdit_Window_Photo");
		this.GrowPhotoPocketGUI.photoPocketWindow = new SelCharaGrowPhotoPocket.WindowPhotoPocket(Object.Instantiate<Transform>(gameObject.transform.Find("Window_PhotoPocketOpen"), baseTr).transform);
		this.GrowPhotoPocketGUI.photoPocketResult = new SelCharaGrowPhotoPocket.PhotoPocketResult(Object.Instantiate<Transform>(gameObject.transform.Find("Window_PhotoPocketOpen_After"), baseTr).transform);
		this.GrowPhotoPocketGUI.photoPocketResult.baseObj.SetActive(false);
		this.GrowPhotoPocketGUI.photoPocketInfoWindow = new SelCharaGrowPhotoPocket.WindowPhotoPocketInfo(Object.Instantiate<Transform>(gameObject.transform.Find("Window_PhotoPocketInfo"), baseTr).transform);
		this.GrowPhotoPocketGUI.photoPocketTab = new SelCharaGrowPhotoPocket.PhotoPocketTab(baseTr.Find("CharaGrow_Main").Find("Main/Right/Photo"));
		this.ConversionCheckConfirmed = false;
	}

	// Token: 0x06001076 RID: 4214 RVA: 0x000C85C8 File Offset: 0x000C67C8
	public void SetupItemPhotoPocket(int charaId)
	{
		SelCharaGrowPhotoPocket.PhotoPocketTab photoPocketTab = this.GrowPhotoPocketGUI.photoPocketTab;
		CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(charaId);
		GrowItemData nextItemByReleasePhotoFrame = userCharaData.GetNextItemByReleasePhotoFrame();
		if (nextItemByReleasePhotoFrame != null)
		{
			ItemData userItemData = DataManager.DmItem.GetUserItemData(nextItemByReleasePhotoFrame.item.id);
			int num = userItemData.num;
			int num2 = nextItemByReleasePhotoFrame.item.num;
			photoPocketTab.GageAll.m_Image.fillAmount = (float)num / (float)num2;
			photoPocketTab.Num_Item.text = ((num >= num2) ? string.Format("{0}/{1}", num, num2) : string.Format("{0}{1}{2}/{3}", new object[]
			{
				PrjUtil.ColorRedStartTag,
				num,
				PrjUtil.ColorEndTag,
				num2
			}));
			photoPocketTab.ItemIconCtrl.Setup(userItemData.staticData);
			photoPocketTab.Txt_ItemName.text = userItemData.staticData.GetName();
			return;
		}
		photoPocketTab.GageAll.m_Image.fillAmount = 1f;
		photoPocketTab.Num_Item.text = "-";
		ItemStaticBase itemStaticBase = DataManager.DmItem.GetItemStaticBase(userCharaData.staticData.baseData.ppItemId);
		photoPocketTab.ItemIconCtrl.Setup(itemStaticBase);
		photoPocketTab.Txt_ItemName.text = itemStaticBase.GetName();
	}

	// Token: 0x06001077 RID: 4215 RVA: 0x000C8723 File Offset: 0x000C6923
	public void UpdateItemPhotoPocket(int charaId)
	{
		this.SetupItemPhotoPocket(charaId);
	}

	// Token: 0x04000E54 RID: 3668
	public SelCharaGrowPhotoPocket.CharaGrowPhotoPocketGUI GrowPhotoPocketGUI;

	// Token: 0x04000E55 RID: 3669
	public bool ConversionCheckConfirmed;

	// Token: 0x02000A19 RID: 2585
	public class PhotoPocketTab
	{
		// Token: 0x06003E5E RID: 15966 RVA: 0x001E8C24 File Offset: 0x001E6E24
		public PhotoPocketTab(Transform baseTr)
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

		// Token: 0x04004099 RID: 16537
		public GameObject baseObj;

		// Token: 0x0400409A RID: 16538
		public PguiTextCtrl Txt_ItemName;

		// Token: 0x0400409B RID: 16539
		public PguiTextCtrl Num_Item;

		// Token: 0x0400409C RID: 16540
		public GameObject Icon_Item;

		// Token: 0x0400409D RID: 16541
		public IconItemCtrl ItemIconCtrl;

		// Token: 0x0400409E RID: 16542
		public PguiImageCtrl GageAll;
	}

	// Token: 0x02000A1A RID: 2586
	public class WindowPhotoPocket
	{
		// Token: 0x06003E5F RID: 15967 RVA: 0x001E8D00 File Offset: 0x001E6F00
		public WindowPhotoPocket(Transform baseTr)
		{
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.photoPocketIconBefores = new List<SelCharaGrowCtrl.PhotoPocketIcon>
			{
				new SelCharaGrowCtrl.PhotoPocketIcon(baseTr.Find("Base/Window/Icon_Photo_Before/Icon_Photo01")),
				new SelCharaGrowCtrl.PhotoPocketIcon(baseTr.Find("Base/Window/Icon_Photo_Before/Icon_Photo02")),
				new SelCharaGrowCtrl.PhotoPocketIcon(baseTr.Find("Base/Window/Icon_Photo_Before/Icon_Photo03")),
				new SelCharaGrowCtrl.PhotoPocketIcon(baseTr.Find("Base/Window/Icon_Photo_Before/Icon_Photo04"))
			};
			this.photoPocketIconAfters = new List<SelCharaGrowCtrl.PhotoPocketIcon>
			{
				new SelCharaGrowCtrl.PhotoPocketIcon(baseTr.Find("Base/Window/Icon_Photo_After/Icon_Photo01")),
				new SelCharaGrowCtrl.PhotoPocketIcon(baseTr.Find("Base/Window/Icon_Photo_After/Icon_Photo02")),
				new SelCharaGrowCtrl.PhotoPocketIcon(baseTr.Find("Base/Window/Icon_Photo_After/Icon_Photo03")),
				new SelCharaGrowCtrl.PhotoPocketIcon(baseTr.Find("Base/Window/Icon_Photo_After/Icon_Photo04"))
			};
			this.ItemBef_Num = baseTr.Find("Base/Window/ItemInfo/Img_Yaji/Num_BeforeTxt").GetComponent<PguiTextCtrl>();
			this.ItemAft_Num = baseTr.Find("Base/Window/ItemInfo/Img_Yaji/Num_AfterTxt").GetComponent<PguiTextCtrl>();
			this.Item_Txt = baseTr.Find("Base/Window/ItemInfo/Txt01").GetComponent<PguiTextCtrl>();
			this.Item_kiseki = baseTr.Find("Base/Window/Txt_Kiseki").gameObject;
			this.Btn_MoreInfo = baseTr.Find("Base/Window/Btn_MoreInfo").GetComponent<PguiButtonCtrl>();
		}

		// Token: 0x0400409F RID: 16543
		public static readonly int COUNT = 4;

		// Token: 0x040040A0 RID: 16544
		public PguiOpenWindowCtrl owCtrl;

		// Token: 0x040040A1 RID: 16545
		public List<SelCharaGrowCtrl.PhotoPocketIcon> photoPocketIconBefores;

		// Token: 0x040040A2 RID: 16546
		public List<SelCharaGrowCtrl.PhotoPocketIcon> photoPocketIconAfters;

		// Token: 0x040040A3 RID: 16547
		public PguiTextCtrl ItemBef_Num;

		// Token: 0x040040A4 RID: 16548
		public PguiTextCtrl ItemAft_Num;

		// Token: 0x040040A5 RID: 16549
		public PguiTextCtrl Item_Txt;

		// Token: 0x040040A6 RID: 16550
		public GameObject Item_kiseki;

		// Token: 0x040040A7 RID: 16551
		public PguiButtonCtrl Btn_MoreInfo;
	}

	// Token: 0x02000A1B RID: 2587
	public class PhotoPocketResult
	{
		// Token: 0x06003E61 RID: 15969 RVA: 0x001E8E5C File Offset: 0x001E705C
		public PhotoPocketResult(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.AEImage_Bg = baseTr.Find("AEImage_Bg").GetComponent<PguiAECtrl>();
			this.AEImage_Window = baseTr.Find("AEImage_Window").GetComponent<PguiAECtrl>();
			this.aeImages = new List<SelCharaGrowPhotoPocket.PhotoPocketResult.AEImage>
			{
				new SelCharaGrowPhotoPocket.PhotoPocketResult.AEImage(baseTr.Find("Window/AEImage_Photo01")),
				new SelCharaGrowPhotoPocket.PhotoPocketResult.AEImage(baseTr.Find("Window/AEImage_Photo02")),
				new SelCharaGrowPhotoPocket.PhotoPocketResult.AEImage(baseTr.Find("Window/AEImage_Photo03")),
				new SelCharaGrowPhotoPocket.PhotoPocketResult.AEImage(baseTr.Find("Window/AEImage_Photo04"))
			};
			this.Txt_Title = baseTr.Find("Window/Txt_Title").GetComponent<PguiTextCtrl>();
		}

		// Token: 0x06003E62 RID: 15970 RVA: 0x001E8F20 File Offset: 0x001E7120
		public void Setup(int currentCharaId, int nextOpenPhotoIndex)
		{
			SoundManager.Play("prd_se_friends_photo_pocket_add", false, false);
			SoundManager.PlayVoice(DataManager.DmChara.GetCharaStaticData(currentCharaId).cueSheetName, VOICE_TYPE.PHT01);
			CharaPackData userCharaData = DataManager.DmChara.GetUserCharaData(currentCharaId);
			this.AEImage_Bg.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
			{
				this.AEImage_Bg.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
			});
			this.AEImage_Window.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
			{
				this.AEImage_Window.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
			});
			for (int i = 0; i < this.aeImages.Count; i++)
			{
				this.aeImages[i].AEImage_Photo.GetComponent<PguiReplaceAECtrl>().Replace((userCharaData.staticData.baseData.spAbilityRelPp == i + 1) ? "1" : "0");
				SelCharaGrowPhotoPocket.PhotoPocketResult.AEImage aeimage = this.aeImages[i];
				if (i == nextOpenPhotoIndex)
				{
					aeimage.AEImage_Photo.PlayAnimation(PguiAECtrl.AmimeType.START, null);
					aeimage.AEImage_LvUpEffect.PlayAnimation(PguiAECtrl.AmimeType.MAX, null);
				}
				else if (userCharaData.dynamicData.PhotoPocket[i].Flag)
				{
					aeimage.AEImage_Photo.PauseAnimation(PguiAECtrl.AmimeType.END, null);
				}
				else
				{
					aeimage.AEImage_Photo.PauseAnimation(PguiAECtrl.AmimeType.LOOP, null);
				}
				aeimage.Num_Lv.gameObject.SetActive(userCharaData.dynamicData.PhotoPocket[i].Step > 0);
				uGUITweenColor component = aeimage.Num_Lv.GetComponent<uGUITweenColor>();
				if (component != null)
				{
					component.Reset();
					component.enabled = nextOpenPhotoIndex == i;
				}
				aeimage.Num_Lv.text = string.Format("{0}", userCharaData.dynamicData.PhotoPocket[i].Step);
			}
			this.Txt_Title.text = string.Format("フォトポケット[{0}]が強化されました！", nextOpenPhotoIndex + 1);
		}

		// Token: 0x040040A8 RID: 16552
		public static readonly int COUNT = 4;

		// Token: 0x040040A9 RID: 16553
		public GameObject baseObj;

		// Token: 0x040040AA RID: 16554
		public PguiAECtrl AEImage_Bg;

		// Token: 0x040040AB RID: 16555
		public PguiAECtrl AEImage_Window;

		// Token: 0x040040AC RID: 16556
		public List<SelCharaGrowPhotoPocket.PhotoPocketResult.AEImage> aeImages;

		// Token: 0x040040AD RID: 16557
		public PguiTextCtrl Txt_Title;

		// Token: 0x02001167 RID: 4455
		public class AEImage
		{
			// Token: 0x06005614 RID: 22036 RVA: 0x00250AD8 File Offset: 0x0024ECD8
			public AEImage(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.AEImage_Photo = baseTr.GetComponent<PguiAECtrl>();
				this.AEImage_LvUpEffect = baseTr.Find("AEImage_LvUpEffect").GetComponent<PguiAECtrl>();
				this.Num_Lv = baseTr.Find("Num_Lv").GetComponent<PguiTextCtrl>();
			}

			// Token: 0x04005F88 RID: 24456
			public GameObject baseObj;

			// Token: 0x04005F89 RID: 24457
			public PguiAECtrl AEImage_Photo;

			// Token: 0x04005F8A RID: 24458
			public PguiAECtrl AEImage_LvUpEffect;

			// Token: 0x04005F8B RID: 24459
			public PguiTextCtrl Num_Lv;
		}
	}

	// Token: 0x02000A1C RID: 2588
	public class WindowItemExchange
	{
		// Token: 0x06003E66 RID: 15974 RVA: 0x001E9108 File Offset: 0x001E7308
		public WindowItemExchange(Transform baseTr)
		{
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.Txt_Title = baseTr.Find("Base/Window/Title/Text").GetComponent<PguiTextCtrl>();
			this.Left_Name = baseTr.Find("Base/Window/Box/Left/Txt_ItemBox").GetComponent<PguiTextCtrl>();
			this.Left_Num = baseTr.Find("Base/Window/Box/Left/NumInfo/Num").GetComponent<PguiTextCtrl>();
			this.Left_Txt = baseTr.Find("Base/Window/Box/Left/NumInfo/Txt").GetComponent<PguiTextCtrl>();
			this.Right_Name = baseTr.Find("Base/Window/Box/Right/Txt_ItemBox").GetComponent<PguiTextCtrl>();
			this.Right_Num = baseTr.Find("Base/Window/Box/Right/NumInfo/Num").GetComponent<PguiTextCtrl>();
			this.Txt_RateInfo = baseTr.Find("Base/Window/Box/Txt_RateInfo").GetComponent<PguiTextCtrl>();
			this.Left_IconTex = baseTr.Find("Base/Window/ItemNumInfo01/Icon_Tex").GetComponent<PguiRawImageCtrl>();
			this.Left_Category = baseTr.Find("Base/Window/ItemNumInfo01/Txt").GetComponent<PguiTextCtrl>();
			this.Left_BeforeNum = baseTr.Find("Base/Window/ItemNumInfo01/Img_Yaji/Num_BeforeTxt").GetComponent<PguiTextCtrl>();
			this.Left_AfterNum = baseTr.Find("Base/Window/ItemNumInfo01/Img_Yaji/Num_AfterTxt").GetComponent<PguiTextCtrl>();
			this.Right_IconTex = baseTr.Find("Base/Window/ItemNumInfo02/Icon_Tex").GetComponent<PguiRawImageCtrl>();
			this.Right_Category = baseTr.Find("Base/Window/ItemNumInfo02/Txt01").GetComponent<PguiTextCtrl>();
			this.Right_BeforeNum = baseTr.Find("Base/Window/ItemNumInfo02/Img_Yaji/Num_BeforeTxt").GetComponent<PguiTextCtrl>();
			this.Right_AfterNum = baseTr.Find("Base/Window/ItemNumInfo02/Img_Yaji/Num_AfterTxt").GetComponent<PguiTextCtrl>();
			this.ButtonL = baseTr.Find("Base/Window/ButtonL").GetComponent<PguiButtonCtrl>();
			this.ButtonR = baseTr.Find("Base/Window/ButtonR").GetComponent<PguiButtonCtrl>();
			this.Left_Icon = baseTr.Find("Base/Window/Box/Left/ItemIcon/Icon_Item").gameObject;
			this.Left_ItemIconCtrl = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, this.Left_Icon.transform).GetComponent<IconItemCtrl>();
			if (this.Left_Icon.transform.Find("Icon_Item") != null)
			{
				this.Left_Icon.transform.Find("Icon_Item").gameObject.SetActive(false);
			}
			this.Right_Icon = baseTr.Find("Base/Window/Box/Right/ItemIcon/Icon_Item").gameObject;
			this.Right_ItemIconCtrl = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Item, this.Right_Icon.transform).GetComponent<IconItemCtrl>();
			if (this.Right_Icon.transform.Find("Icon_Item") != null)
			{
				this.Right_Icon.transform.Find("Icon_Item").gameObject.SetActive(false);
			}
		}

		// Token: 0x040040AE RID: 16558
		public PguiOpenWindowCtrl owCtrl;

		// Token: 0x040040AF RID: 16559
		public PguiTextCtrl Txt_Title;

		// Token: 0x040040B0 RID: 16560
		public PguiTextCtrl Left_Name;

		// Token: 0x040040B1 RID: 16561
		public GameObject Left_Icon;

		// Token: 0x040040B2 RID: 16562
		public IconItemCtrl Left_ItemIconCtrl;

		// Token: 0x040040B3 RID: 16563
		public PguiTextCtrl Left_Num;

		// Token: 0x040040B4 RID: 16564
		public PguiTextCtrl Left_Txt;

		// Token: 0x040040B5 RID: 16565
		public PguiTextCtrl Right_Name;

		// Token: 0x040040B6 RID: 16566
		public GameObject Right_Icon;

		// Token: 0x040040B7 RID: 16567
		public IconItemCtrl Right_ItemIconCtrl;

		// Token: 0x040040B8 RID: 16568
		public PguiTextCtrl Right_Num;

		// Token: 0x040040B9 RID: 16569
		public PguiTextCtrl Txt_RateInfo;

		// Token: 0x040040BA RID: 16570
		public PguiRawImageCtrl Left_IconTex;

		// Token: 0x040040BB RID: 16571
		public PguiTextCtrl Left_Category;

		// Token: 0x040040BC RID: 16572
		public PguiTextCtrl Left_BeforeNum;

		// Token: 0x040040BD RID: 16573
		public PguiTextCtrl Left_AfterNum;

		// Token: 0x040040BE RID: 16574
		public PguiRawImageCtrl Right_IconTex;

		// Token: 0x040040BF RID: 16575
		public PguiTextCtrl Right_Category;

		// Token: 0x040040C0 RID: 16576
		public PguiTextCtrl Right_BeforeNum;

		// Token: 0x040040C1 RID: 16577
		public PguiTextCtrl Right_AfterNum;

		// Token: 0x040040C2 RID: 16578
		public PguiButtonCtrl ButtonL;

		// Token: 0x040040C3 RID: 16579
		public PguiButtonCtrl ButtonR;
	}

	// Token: 0x02000A1D RID: 2589
	public class ExchangeWarningWindow : MonoBehaviour
	{
		// Token: 0x06003E67 RID: 15975 RVA: 0x001E938D File Offset: 0x001E758D
		public void Initialize()
		{
			this.owCtrl = base.gameObject.GetComponent<PguiOpenWindowCtrl>();
			this.CheckBoxButton = base.gameObject.transform.Find("Base/Window/Btn_ConfirmCheckBox").GetComponent<PguiButtonCtrl>();
		}

		// Token: 0x040040C4 RID: 16580
		public PguiOpenWindowCtrl owCtrl;

		// Token: 0x040040C5 RID: 16581
		public PguiButtonCtrl CheckBoxButton;
	}

	// Token: 0x02000A1E RID: 2590
	public class WindowPhotoPocketInfo
	{
		// Token: 0x06003E69 RID: 15977 RVA: 0x001E93C8 File Offset: 0x001E75C8
		public WindowPhotoPocketInfo(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Txt_PocketName = baseTr.Find("Base/Window/CurrentInfo/Txt_PocketName").GetComponent<PguiTextCtrl>();
			this.Txt_Info = baseTr.Find("Base/Window/CurrentInfo/Txt_Info").GetComponent<PguiTextCtrl>();
			this.Num_PhotoPocketLevel = baseTr.Find("Base/Window/Num_PhotoPocketLevel").GetComponent<PguiTextCtrl>();
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.photoPocketIcons = new List<SelCharaGrowCtrl.PhotoPocketIcon>
			{
				new SelCharaGrowCtrl.PhotoPocketIcon(baseTr.Find("Base/Window/Icon_Photo01")),
				new SelCharaGrowCtrl.PhotoPocketIcon(baseTr.Find("Base/Window/Icon_Photo02")),
				new SelCharaGrowCtrl.PhotoPocketIcon(baseTr.Find("Base/Window/Icon_Photo03")),
				new SelCharaGrowCtrl.PhotoPocketIcon(baseTr.Find("Base/Window/Icon_Photo04"))
			};
		}

		// Token: 0x1700092E RID: 2350
		// (get) Token: 0x06003E6A RID: 15978 RVA: 0x001E9498 File Offset: 0x001E7698
		public int GetPhotoPocketIconIndex
		{
			get
			{
				return this.photoPocketIcons.FindIndex((SelCharaGrowCtrl.PhotoPocketIcon item) => item.Current != null && item.Current.activeSelf);
			}
		}

		// Token: 0x06003E6B RID: 15979 RVA: 0x001E94C4 File Offset: 0x001E76C4
		public void Setup(CharaPackData charaData, bool nextInfo)
		{
			this.cdd = new CharaDynamicData();
			this.cdd.id = charaData.id;
			this.cdd.PhotoFrameTotalStep = charaData.dynamicData.PhotoFrameTotalStep;
			if (nextInfo)
			{
				CharaDynamicData charaDynamicData = this.cdd;
				int num = charaDynamicData.PhotoFrameTotalStep + 1;
				charaDynamicData.PhotoFrameTotalStep = num;
				if (this.cdd.PhotoFrameTotalStep > DataManager.DmServerMst.StaticCharaPpDataMap[charaData.staticData.baseData.photoFrameTableId].PpStepMax)
				{
					this.cdd.PhotoFrameTotalStep = DataManager.DmServerMst.StaticCharaPpDataMap[charaData.staticData.baseData.photoFrameTableId].PpStepMax;
				}
			}
			for (int i = 0; i < this.photoPocketIcons.Count; i++)
			{
				bool flag = this.cdd.PhotoPocket[i].Flag;
				SelCharaGrowCtrl.PhotoPocketIcon photoPocketIcon = this.photoPocketIcons[i];
				photoPocketIcon.IconPhoto.SetImageByName(photoPocketIcon.ReplaceIconPhoto.GetSpriteById(flag ? 1 : 0).name);
				photoPocketIcon.MarkKiseki.gameObject.SetActive(charaData.staticData.baseData.spAbilityRelPp == i + 1);
				photoPocketIcon.MarkKiseki.Replace(flag ? 1 : 0);
				photoPocketIcon.NumLv.gameObject.SetActive(this.cdd.PhotoPocket[i].Step > 0);
				photoPocketIcon.NumLv.text = string.Format("{0}", this.cdd.PhotoPocket[i].Step);
			}
			this.UpdateInfo(charaData.staticData);
		}

		// Token: 0x06003E6C RID: 15980 RVA: 0x001E967C File Offset: 0x001E787C
		public void UpdateInfo(CharaStaticData csd)
		{
			int getPhotoPocketIconIndex = this.GetPhotoPocketIconIndex;
			bool flag = getPhotoPocketIconIndex >= 0;
			if (flag)
			{
				CharaDynamicData.PPParam ppparam = this.cdd.PhotoPocket[getPhotoPocketIconIndex];
				string text = ((ppparam.Hp > 0) ? ("たいりょく x" + ppparam.HpRatio2String + "\n") : "");
				text += ((ppparam.Atk > 0) ? ("こうげき x" + ppparam.AtkRatio2String + "\n") : "");
				text += ((ppparam.Def > 0) ? ("まもり x" + ppparam.DefRatio2String + "\n") : "");
				this.Txt_Info.text = text ?? "";
				string text2 = "[?]";
				switch (getPhotoPocketIconIndex)
				{
				case 0:
					text2 = "[1]";
					break;
				case 1:
					text2 = "[2]";
					break;
				case 2:
					text2 = "[3]";
					break;
				case 3:
					text2 = "[4]";
					break;
				}
				string text3 = ((this.cdd.PhotoPocket[getPhotoPocketIconIndex].Step <= 0) ? PhotoUtil.PhotoPocketNotReleasedText : string.Format("Lv.{0}", this.cdd.PhotoPocket[getPhotoPocketIconIndex].Step));
				this.Txt_PocketName.text = "フォトポケット" + text2 + " " + text3;
			}
			this.Txt_Info.gameObject.SetActive(flag);
			this.Txt_PocketName.gameObject.SetActive(flag);
			this.Num_PhotoPocketLevel.text = string.Format("{0}Rank {1}{2}/{3}", new object[]
			{
				"<size=24>",
				"</size>",
				this.cdd.PhotoFrameTotalStep,
				DataManager.DmServerMst.StaticCharaPpDataMap[csd.baseData.photoFrameTableId].PpStepMax
			});
		}

		// Token: 0x040040C6 RID: 16582
		public PguiOpenWindowCtrl owCtrl;

		// Token: 0x040040C7 RID: 16583
		public GameObject baseObj;

		// Token: 0x040040C8 RID: 16584
		public List<SelCharaGrowCtrl.PhotoPocketIcon> photoPocketIcons;

		// Token: 0x040040C9 RID: 16585
		public PguiTextCtrl Txt_PocketName;

		// Token: 0x040040CA RID: 16586
		public PguiTextCtrl Txt_Info;

		// Token: 0x040040CB RID: 16587
		public PguiTextCtrl Num_PhotoPocketLevel;

		// Token: 0x040040CC RID: 16588
		private CharaDynamicData cdd;
	}

	// Token: 0x02000A1F RID: 2591
	public class CharaGrowPhotoPocketGUI
	{
		// Token: 0x040040CD RID: 16589
		public SelCharaGrowPhotoPocket.WindowPhotoPocket photoPocketWindow;

		// Token: 0x040040CE RID: 16590
		public SelCharaGrowPhotoPocket.PhotoPocketResult photoPocketResult;

		// Token: 0x040040CF RID: 16591
		public SelCharaGrowPhotoPocket.WindowPhotoPocketInfo photoPocketInfoWindow;

		// Token: 0x040040D0 RID: 16592
		public SelCharaGrowPhotoPocket.PhotoPocketTab photoPocketTab;
	}
}
