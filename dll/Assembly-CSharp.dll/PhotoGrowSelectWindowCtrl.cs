using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhotoGrowSelectWindowCtrl
{
	public PhotoGrowSelectWindowCtrl(Transform baseTr)
	{
		this.windowGUI = new PhotoGrowSelectWindowCtrl.WindowGUI(baseTr);
	}

	public void SetCloseStatusCallback(UnityAction<List<ItemDef.Rarity>, List<SortFilterDefine.PhotoLevelType>, List<PhotoDef.Type>> statusCallback)
	{
		this.windowGUI.SetLatestStatusCallBack(statusCallback);
	}

	public PhotoGrowSelectWindowCtrl.WindowGUI windowGUI;

	public class WindowGUI
	{
		private List<PhotoGrowSelectWindowCtrl.WindowGUI.RarityButton> RarityBtnList { get; set; }

		private List<PhotoGrowSelectWindowCtrl.WindowGUI.LevelButton> LevelBtnList { get; set; }

		private List<PhotoGrowSelectWindowCtrl.WindowGUI.PhotoTypeButton> PhotoTypeBtnList { get; set; }

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

		public void SetLatestStatusCallBack(UnityAction<List<ItemDef.Rarity>, List<SortFilterDefine.PhotoLevelType>, List<PhotoDef.Type>> statusCallback)
		{
			this.LatestStatusCallBack = statusCallback;
		}

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

		public PguiOpenWindowCtrl baseWindow;

		private UnityAction<List<ItemDef.Rarity>, List<SortFilterDefine.PhotoLevelType>, List<PhotoDef.Type>> LatestStatusCallBack;

		private class RarityButton
		{
			public PguiToggleButtonCtrl Button { get; set; }

			public ItemDef.Rarity Rarity { get; set; }

			public RarityButton(GameObject go, ItemDef.Rarity rarity)
			{
				this.Button = go.GetComponent<PguiToggleButtonCtrl>();
				this.Rarity = rarity;
			}
		}

		private class LevelButton
		{
			public PguiToggleButtonCtrl Button { get; set; }

			public SortFilterDefine.PhotoLevelType LevelType { get; set; }

			public LevelButton(GameObject go, SortFilterDefine.PhotoLevelType lvType)
			{
				this.Button = go.GetComponent<PguiToggleButtonCtrl>();
				this.LevelType = lvType;
			}
		}

		private class PhotoTypeButton
		{
			public PguiToggleButtonCtrl Button { get; set; }

			public PhotoDef.Type PhotoType { get; set; }

			public PhotoTypeButton(GameObject go, PhotoDef.Type photoType)
			{
				this.Button = go.GetComponent<PguiToggleButtonCtrl>();
				this.PhotoType = photoType;
			}
		}
	}
}
