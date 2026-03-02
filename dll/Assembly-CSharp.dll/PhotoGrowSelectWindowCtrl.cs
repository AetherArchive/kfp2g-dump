using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200012A RID: 298
public class PhotoGrowSelectWindowCtrl
{
	// Token: 0x06000F1B RID: 3867 RVA: 0x000B5EC0 File Offset: 0x000B40C0
	public PhotoGrowSelectWindowCtrl(Transform baseTr)
	{
		this.windowGUI = new PhotoGrowSelectWindowCtrl.WindowGUI(baseTr);
	}

	// Token: 0x06000F1C RID: 3868 RVA: 0x000B5ED4 File Offset: 0x000B40D4
	public void SetCloseStatusCallback(UnityAction<List<ItemDef.Rarity>, List<SortFilterDefine.PhotoLevelType>, List<PhotoDef.Type>> statusCallback)
	{
		this.windowGUI.SetLatestStatusCallBack(statusCallback);
	}

	// Token: 0x04000DAB RID: 3499
	public PhotoGrowSelectWindowCtrl.WindowGUI windowGUI;

	// Token: 0x02000971 RID: 2417
	public class WindowGUI
	{
		// Token: 0x1700088E RID: 2190
		// (get) Token: 0x06003BDC RID: 15324 RVA: 0x001D76BE File Offset: 0x001D58BE
		// (set) Token: 0x06003BDD RID: 15325 RVA: 0x001D76C6 File Offset: 0x001D58C6
		private List<PhotoGrowSelectWindowCtrl.WindowGUI.RarityButton> RarityBtnList { get; set; }

		// Token: 0x1700088F RID: 2191
		// (get) Token: 0x06003BDE RID: 15326 RVA: 0x001D76CF File Offset: 0x001D58CF
		// (set) Token: 0x06003BDF RID: 15327 RVA: 0x001D76D7 File Offset: 0x001D58D7
		private List<PhotoGrowSelectWindowCtrl.WindowGUI.LevelButton> LevelBtnList { get; set; }

		// Token: 0x17000890 RID: 2192
		// (get) Token: 0x06003BE0 RID: 15328 RVA: 0x001D76E0 File Offset: 0x001D58E0
		// (set) Token: 0x06003BE1 RID: 15329 RVA: 0x001D76E8 File Offset: 0x001D58E8
		private List<PhotoGrowSelectWindowCtrl.WindowGUI.PhotoTypeButton> PhotoTypeBtnList { get; set; }

		// Token: 0x06003BE2 RID: 15330 RVA: 0x001D76F4 File Offset: 0x001D58F4
		public WindowGUI(Transform baseTr)
		{
			this.baseWindow = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.RarityBtnList = new List<PhotoGrowSelectWindowCtrl.WindowGUI.RarityButton>
			{
				new PhotoGrowSelectWindowCtrl.WindowGUI.RarityButton(baseTr.Find("Base/Window/Sort/Btn01").gameObject, ItemDef.Rarity.STAR1),
				new PhotoGrowSelectWindowCtrl.WindowGUI.RarityButton(baseTr.Find("Base/Window/Sort/Btn02").gameObject, ItemDef.Rarity.STAR2),
				new PhotoGrowSelectWindowCtrl.WindowGUI.RarityButton(baseTr.Find("Base/Window/Sort/Btn03").gameObject, ItemDef.Rarity.STAR3),
				new PhotoGrowSelectWindowCtrl.WindowGUI.RarityButton(baseTr.Find("Base/Window/Sort/Btn04").gameObject, ItemDef.Rarity.STAR4)
			};
			foreach (PhotoGrowSelectWindowCtrl.WindowGUI.RarityButton rarityButton in this.RarityBtnList)
			{
				rarityButton.Button.AddOnClickListener((PguiToggleButtonCtrl pbc, int toggleIdx) => true);
			}
			this.LevelBtnList = new List<PhotoGrowSelectWindowCtrl.WindowGUI.LevelButton>
			{
				new PhotoGrowSelectWindowCtrl.WindowGUI.LevelButton(baseTr.Find("Base/Window/Sort/Btn05").gameObject, SortFilterDefine.PhotoLevelType.One)
			};
			foreach (PhotoGrowSelectWindowCtrl.WindowGUI.LevelButton levelButton in this.LevelBtnList)
			{
				levelButton.Button.AddOnClickListener((PguiToggleButtonCtrl pbc, int toggleIdx) => true);
			}
			this.PhotoTypeBtnList = new List<PhotoGrowSelectWindowCtrl.WindowGUI.PhotoTypeButton>
			{
				new PhotoGrowSelectWindowCtrl.WindowGUI.PhotoTypeButton(baseTr.Find("Base/Window/Sort/Btn07").gameObject, PhotoDef.Type.OTHER)
			};
			foreach (PhotoGrowSelectWindowCtrl.WindowGUI.PhotoTypeButton photoTypeButton in this.PhotoTypeBtnList)
			{
				photoTypeButton.Button.AddOnClickListener((PguiToggleButtonCtrl pbc, int toggleIdx) => true);
			}
			this.baseWindow.Setup("まとめて選択", string.Empty, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, new PguiOpenWindowCtrl.Callback(this.OnClickWindowButton), null, false);
		}

		// Token: 0x06003BE3 RID: 15331 RVA: 0x001D7934 File Offset: 0x001D5B34
		public void SelectSetup(List<ItemDef.Rarity> rarityList, List<SortFilterDefine.PhotoLevelType> levelTypeList, List<PhotoDef.Type> photoTypeList)
		{
			foreach (PhotoGrowSelectWindowCtrl.WindowGUI.RarityButton rarityButton in this.RarityBtnList)
			{
				if (rarityList.Contains(rarityButton.Rarity))
				{
					rarityButton.Button.SetToggleIndex(1);
				}
			}
			foreach (PhotoGrowSelectWindowCtrl.WindowGUI.LevelButton levelButton in this.LevelBtnList)
			{
				if (levelTypeList.Contains(levelButton.LevelType))
				{
					levelButton.Button.SetToggleIndex(1);
				}
			}
			foreach (PhotoGrowSelectWindowCtrl.WindowGUI.PhotoTypeButton photoTypeButton in this.PhotoTypeBtnList)
			{
				if (photoTypeList.Contains(photoTypeButton.PhotoType))
				{
					photoTypeButton.Button.SetToggleIndex(1);
				}
			}
		}

		// Token: 0x17000891 RID: 2193
		// (get) Token: 0x06003BE4 RID: 15332 RVA: 0x001D7A48 File Offset: 0x001D5C48
		private List<ItemDef.Rarity> SelectRarityList
		{
			get
			{
				List<ItemDef.Rarity> list = new List<ItemDef.Rarity>();
				foreach (PhotoGrowSelectWindowCtrl.WindowGUI.RarityButton rarityButton in this.RarityBtnList)
				{
					if (1 == rarityButton.Button.GetToggleIndex())
					{
						list.Add(rarityButton.Rarity);
					}
				}
				return list;
			}
		}

		// Token: 0x17000892 RID: 2194
		// (get) Token: 0x06003BE5 RID: 15333 RVA: 0x001D7AB8 File Offset: 0x001D5CB8
		private List<SortFilterDefine.PhotoLevelType> SelectLevelTypeList
		{
			get
			{
				List<SortFilterDefine.PhotoLevelType> list = new List<SortFilterDefine.PhotoLevelType>();
				foreach (PhotoGrowSelectWindowCtrl.WindowGUI.LevelButton levelButton in this.LevelBtnList)
				{
					if (1 == levelButton.Button.GetToggleIndex())
					{
						list.Add(levelButton.LevelType);
					}
				}
				return list;
			}
		}

		// Token: 0x17000893 RID: 2195
		// (get) Token: 0x06003BE6 RID: 15334 RVA: 0x001D7B28 File Offset: 0x001D5D28
		private List<PhotoDef.Type> SelectPhotoTypeList
		{
			get
			{
				List<PhotoDef.Type> list = new List<PhotoDef.Type>();
				foreach (PhotoGrowSelectWindowCtrl.WindowGUI.PhotoTypeButton photoTypeButton in this.PhotoTypeBtnList)
				{
					if (1 == photoTypeButton.Button.GetToggleIndex())
					{
						list.Add(photoTypeButton.PhotoType);
					}
				}
				return list;
			}
		}

		// Token: 0x06003BE7 RID: 15335 RVA: 0x001D7B98 File Offset: 0x001D5D98
		public void SetLatestStatusCallBack(UnityAction<List<ItemDef.Rarity>, List<SortFilterDefine.PhotoLevelType>, List<PhotoDef.Type>> statusCallback)
		{
			this.LatestStatusCallBack = statusCallback;
		}

		// Token: 0x06003BE8 RID: 15336 RVA: 0x001D7BA1 File Offset: 0x001D5DA1
		public bool OnClickWindowButton(int index)
		{
			if (index == 1)
			{
				UnityAction<List<ItemDef.Rarity>, List<SortFilterDefine.PhotoLevelType>, List<PhotoDef.Type>> latestStatusCallBack = this.LatestStatusCallBack;
				if (latestStatusCallBack != null)
				{
					latestStatusCallBack(this.SelectRarityList, this.SelectLevelTypeList, this.SelectPhotoTypeList);
				}
			}
			return true;
		}

		// Token: 0x04003CFD RID: 15613
		public PguiOpenWindowCtrl baseWindow;

		// Token: 0x04003D01 RID: 15617
		private UnityAction<List<ItemDef.Rarity>, List<SortFilterDefine.PhotoLevelType>, List<PhotoDef.Type>> LatestStatusCallBack;

		// Token: 0x0200114F RID: 4431
		private class RarityButton
		{
			// Token: 0x17000C94 RID: 3220
			// (get) Token: 0x06005594 RID: 21908 RVA: 0x0024F549 File Offset: 0x0024D749
			// (set) Token: 0x06005595 RID: 21909 RVA: 0x0024F551 File Offset: 0x0024D751
			public PguiToggleButtonCtrl Button { get; set; }

			// Token: 0x17000C95 RID: 3221
			// (get) Token: 0x06005596 RID: 21910 RVA: 0x0024F55A File Offset: 0x0024D75A
			// (set) Token: 0x06005597 RID: 21911 RVA: 0x0024F562 File Offset: 0x0024D762
			public ItemDef.Rarity Rarity { get; set; }

			// Token: 0x06005598 RID: 21912 RVA: 0x0024F56B File Offset: 0x0024D76B
			public RarityButton(GameObject go, ItemDef.Rarity rarity)
			{
				this.Button = go.GetComponent<PguiToggleButtonCtrl>();
				this.Rarity = rarity;
			}
		}

		// Token: 0x02001150 RID: 4432
		private class LevelButton
		{
			// Token: 0x17000C96 RID: 3222
			// (get) Token: 0x06005599 RID: 21913 RVA: 0x0024F586 File Offset: 0x0024D786
			// (set) Token: 0x0600559A RID: 21914 RVA: 0x0024F58E File Offset: 0x0024D78E
			public PguiToggleButtonCtrl Button { get; set; }

			// Token: 0x17000C97 RID: 3223
			// (get) Token: 0x0600559B RID: 21915 RVA: 0x0024F597 File Offset: 0x0024D797
			// (set) Token: 0x0600559C RID: 21916 RVA: 0x0024F59F File Offset: 0x0024D79F
			public SortFilterDefine.PhotoLevelType LevelType { get; set; }

			// Token: 0x0600559D RID: 21917 RVA: 0x0024F5A8 File Offset: 0x0024D7A8
			public LevelButton(GameObject go, SortFilterDefine.PhotoLevelType lvType)
			{
				this.Button = go.GetComponent<PguiToggleButtonCtrl>();
				this.LevelType = lvType;
			}
		}

		// Token: 0x02001151 RID: 4433
		private class PhotoTypeButton
		{
			// Token: 0x17000C98 RID: 3224
			// (get) Token: 0x0600559E RID: 21918 RVA: 0x0024F5C3 File Offset: 0x0024D7C3
			// (set) Token: 0x0600559F RID: 21919 RVA: 0x0024F5CB File Offset: 0x0024D7CB
			public PguiToggleButtonCtrl Button { get; set; }

			// Token: 0x17000C99 RID: 3225
			// (get) Token: 0x060055A0 RID: 21920 RVA: 0x0024F5D4 File Offset: 0x0024D7D4
			// (set) Token: 0x060055A1 RID: 21921 RVA: 0x0024F5DC File Offset: 0x0024D7DC
			public PhotoDef.Type PhotoType { get; set; }

			// Token: 0x060055A2 RID: 21922 RVA: 0x0024F5E5 File Offset: 0x0024D7E5
			public PhotoTypeButton(GameObject go, PhotoDef.Type photoType)
			{
				this.Button = go.GetComponent<PguiToggleButtonCtrl>();
				this.PhotoType = photoType;
			}
		}
	}
}
