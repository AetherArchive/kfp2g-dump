using System;
using System.Collections.Generic;
using UnityEngine;

public class FollowWindowCtrl : PguiOpenWindowCtrl
{
	private static void InitializeAnimation(FollowWindowCtrl.Mode mode, SimpleAnimation anime, SelCharaDeckCtrl.GUI.CharaDeck charaDeck)
	{
		if (anime != null && charaDeck != null)
		{
			anime.ExInit();
			switch (mode)
			{
			case FollowWindowCtrl.Mode.DECK_TOP:
				charaDeck.Btn_Chara.SetToggleIndex(1);
				charaDeck.Btn_Photo.SetToggleIndex(0);
				charaDeck.Btn_Accessory.SetToggleIndex(0);
				anime.ExPauseAnimationLastFrame(SimpleAnimation.ExPguiStatus.END);
				return;
			case FollowWindowCtrl.Mode.PHOTO_TOP:
				charaDeck.Btn_Chara.SetToggleIndex(0);
				charaDeck.Btn_Photo.SetToggleIndex(1);
				charaDeck.Btn_Accessory.SetToggleIndex(0);
				anime.ExPauseAnimationLastFrame(SimpleAnimation.ExPguiStatus.START);
				return;
			case FollowWindowCtrl.Mode.ACCESSORY_TOP:
				charaDeck.Btn_Chara.SetToggleIndex(0);
				charaDeck.Btn_Photo.SetToggleIndex(0);
				charaDeck.Btn_Accessory.SetToggleIndex(1);
				anime.ExPauseAnimationLastFrame("START2");
				break;
			default:
				return;
			}
		}
	}

	private FollowWindowCtrl.Mode CurrentMode { get; set; }

	public void Init()
	{
		if (this.guiData != null)
		{
			return;
		}
		this.guiData = new FollowWindowCtrl.GUI(base.transform);
		this.CurrentMode = FollowWindowCtrl.Mode.DECK_TOP;
		this.guiData.charaDeck.Btn_Chara.AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickCharaButton));
		this.guiData.charaDeck.Btn_Photo.AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickPhotoButton));
		this.guiData.charaDeck.Btn_Accessory.AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickAccessoryButton));
		this.guiData.charaDeck.Btn_Chara.SetToggleIndex(1);
		this.guiData.charaDeck.Btn_Photo.SetToggleIndex(0);
		this.guiData.charaDeck.Btn_Accessory.SetToggleIndex(0);
		for (int i = 0; i < this.guiData.charaDeck.iconCharaPacks.Count; i++)
		{
			this.guiData.charaDeck.iconCharaPacks[i].iconChara.Setup(new SelCharaDeckCtrl.GUI.IconChara.SetupParam
			{
				cbClickIconPhotoKind = delegate
				{
					this.OnClickPhotoButton(this.guiData.charaDeck.Btn_Photo, 0);
					this.guiData.charaDeck.Btn_Photo.SetToggleIndex(1);
					return true;
				},
				isDeck = false,
				index = i
			});
		}
	}

	private bool OnClickCharaButton(PguiToggleButtonCtrl buttonCtrl, int toggleIndex)
	{
		if (this.CurrentMode != FollowWindowCtrl.Mode.DECK_TOP)
		{
			string text = ((this.CurrentMode == FollowWindowCtrl.Mode.PHOTO_TOP) ? "END" : "END2");
			this.CurrentMode = FollowWindowCtrl.Mode.DECK_TOP;
			foreach (SelCharaDeckCtrl.GUI.IconCharaPack iconCharaPack in this.guiData.charaDeck.iconCharaPacks)
			{
				if (!iconCharaPack.iconChara.PhotoIconKind.gameObject.activeSelf)
				{
					iconCharaPack.iconChara.PhotoIconKind.gameObject.SetActive(true);
				}
				iconCharaPack.iconChara.anime.ExPlayAnimation(text, null);
			}
			this.guiData.charaDeck.Btn_Photo.SetToggleIndex(0);
			this.guiData.charaDeck.Btn_Accessory.SetToggleIndex(0);
			return true;
		}
		return false;
	}

	private bool OnClickPhotoButton(PguiToggleButtonCtrl buttonCtrl, int toggleIndex)
	{
		if (this.CurrentMode != FollowWindowCtrl.Mode.PHOTO_TOP)
		{
			string text = ((this.CurrentMode == FollowWindowCtrl.Mode.DECK_TOP) ? "START" : "END3");
			this.CurrentMode = FollowWindowCtrl.Mode.PHOTO_TOP;
			foreach (SelCharaDeckCtrl.GUI.IconCharaPack iconCharaPack in this.guiData.charaDeck.iconCharaPacks)
			{
				iconCharaPack.iconChara.anime.ExPlayAnimation(text, null);
			}
			this.guiData.charaDeck.Btn_Chara.SetToggleIndex(0);
			this.guiData.charaDeck.Btn_Accessory.SetToggleIndex(0);
			return true;
		}
		return false;
	}

	private bool OnClickAccessoryButton(PguiToggleButtonCtrl buttonCtrl, int toggleIndex)
	{
		if (this.CurrentMode != FollowWindowCtrl.Mode.ACCESSORY_TOP)
		{
			string text = ((this.CurrentMode == FollowWindowCtrl.Mode.DECK_TOP) ? "START2" : "START3");
			this.CurrentMode = FollowWindowCtrl.Mode.ACCESSORY_TOP;
			using (List<SelCharaDeckCtrl.GUI.IconCharaPack>.Enumerator enumerator = this.guiData.charaDeck.iconCharaPacks.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SelCharaDeckCtrl.GUI.IconCharaPack ic = enumerator.Current;
					ic.iconChara.anime.ExPlayAnimation(text, delegate
					{
						ic.iconChara.PhotoIconKind.gameObject.SetActive(false);
					});
				}
			}
			this.guiData.charaDeck.Btn_Chara.SetToggleIndex(0);
			this.guiData.charaDeck.Btn_Photo.SetToggleIndex(0);
			return true;
		}
		return false;
	}

	public void SetUserProfile(HelperPackData hpd)
	{
		foreach (SelCharaDeckCtrl.GUI.IconCharaPack iconCharaPack in this.guiData.charaDeck.iconCharaPacks)
		{
			FollowWindowCtrl.InitializeAnimation(this.CurrentMode, iconCharaPack.iconChara.anime, this.guiData.charaDeck);
		}
		this.guiData.profile.IconCharaCtrl.Setup(hpd.FavoriteChara, SortFilterDefine.SortType.LEVEL, false, new CharaWindowCtrl.DetailParamSetting(CharaWindowCtrl.DetailParamSetting.Preset.OTHER, null), 0, -1, 0);
		this.guiData.profile.Txt_Rank.text = "探検隊Lv." + hpd.level.ToString();
		this.guiData.profile.Txt_Name.text = hpd.userName;
		this.guiData.profile.Txt_Comment.text = hpd.comment;
		this.guiData.profile.Achievement.Setup(hpd.achievementId, true, false);
		for (int i = 0; i < this.guiData.charaDeck.iconCharaPacks.Count; i++)
		{
			SelCharaDeckCtrl.GUI.IconChara iconChara = this.guiData.charaDeck.iconCharaPacks[i].iconChara;
			CharaPackData helpChara = hpd.HelperCharaSetList[i].helpChara;
			iconChara.iconCharaSet.iconCharaCtrl.Setup(helpChara, SortFilterDefine.SortType.LEVEL, iconChara.iconCharaSet.selected.activeSelf || iconChara.iconCharaSet.disable.activeSelf, new CharaWindowCtrl.DetailParamSetting(CharaWindowCtrl.DetailParamSetting.Preset.OTHER, null), 0, -1, 0);
			iconChara.iconCharaSet.InvalidAE();
			List<PhotoPackData> list = new List<PhotoPackData>();
			for (int j = 0; j < hpd.HelperCharaSetList[i].helpPhotoList.Count; j++)
			{
				PhotoPackData photoPackData = ((hpd != null) ? hpd.HelperCharaSetList[i].helpPhotoList[j] : null);
				iconChara.iconPhotoCtrl[j].Setup(photoPackData, SortFilterDefine.SortType.LEVEL, true, false, -1, true);
				iconChara.iconPhotoCtrl[j].InvalidAE();
				if (photoPackData != null && !photoPackData.IsInvalid())
				{
					iconChara.iconPhotoKind[j].Replace(photoPackData.staticData.baseData.type);
				}
				else
				{
					iconChara.iconPhotoKind[j].Replace(-1);
				}
				if (iconChara.iconBlankFrame != null)
				{
					bool flag = helpChara != null && !helpChara.IsInvalid() && helpChara.dynamicData.PhotoPocket[j].Flag;
					bool flag2 = photoPackData != null && !photoPackData.IsInvalid();
					bool flag3 = photoPackData != null && photoPackData.staticData.baseData.kizunaPhotoFlg;
					bool flag4 = flag3 && (helpChara == null || helpChara.IsInvalid() || photoPackData.staticData.GetId() != helpChara.staticData.baseData.kizunaPhotoId);
					int num = ((helpChara == null || helpChara.IsInvalid()) ? 0 : helpChara.dynamicData.PhotoPocket[j].Step);
					if (iconChara != null)
					{
						SelCharaDeckCtrl.DecorationPhotoFrame(iconChara, j, flag2, flag, flag3, flag4, num, false);
					}
				}
				if (photoPackData != null)
				{
					list.Add(photoPackData);
				}
			}
			DataManagerCharaAccessory.Accessory accessory = ((hpd != null) ? hpd.HelperCharaSetList[i].helpAccessory : null);
			iconChara.iconAccessoryCtrl.Setup(new IconAccessoryCtrl.SetupParam
			{
				acce = accessory
			});
			SelCharaDeckCtrl.DecorationAccessoryFrame(iconChara, helpChara != null, false);
		}
		this.guiData.charaDeck.SwitchHelperIcon(-1, false);
	}

	private FollowWindowCtrl.GUI guiData;

	public enum Mode
	{
		INVALID,
		DECK_TOP,
		PHOTO_TOP,
		ACCESSORY_TOP
	}

	public delegate void OnClick();

	public class GUI
	{
		public GUI(Transform baseTr)
		{
			this.profile = new FollowWindowCtrl.Profile(baseTr.Find("Base/Window/Profile"));
			this.charaDeck = new SelCharaDeckCtrl.GUI.CharaDeck(baseTr.Find("Base/Window/DeckSelect"), 7);
		}

		public FollowWindowCtrl.Profile profile;

		public SelCharaDeckCtrl.GUI.CharaDeck charaDeck;
	}

	public class Profile
	{
		public Profile(Transform baseTr)
		{
			this.IconCharaCtrl = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Chara, baseTr.Find("Icon_Chara")).GetComponent<IconCharaCtrl>();
			this.Txt_Rank = baseTr.Find("Txt_Rank").GetComponent<PguiTextCtrl>();
			this.Txt_Name = baseTr.Find("Txt_Name").GetComponent<PguiTextCtrl>();
			this.Txt_Comment = baseTr.Find("Comment/Txt_Comment").GetComponent<PguiTextCtrl>();
			this.Achievement = baseTr.Find("Achievement").GetComponent<AchievementCtrl>();
		}

		public IconCharaCtrl IconCharaCtrl;

		public PguiTextCtrl Txt_Rank;

		public PguiTextCtrl Txt_Name;

		public PguiTextCtrl Txt_Comment;

		public AchievementCtrl Achievement;
	}
}
