using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;

public class PhotoUtil : MonoBehaviour
{
	public static void OpenWindowConfirmReleasePhotoDeck(IconPhotoCtrl iconPhotoCtrl, IEnumerator requestServer)
	{
		string colorRedStartTag = PrjUtil.ColorRedStartTag;
		string colorEndTag = PrjUtil.ColorEndTag;
		string text = "";
		PhotoPackData photoPackData = iconPhotoCtrl.photoPackData;
		bool flag = false;
		Predicate<long> <>9__1;
		foreach (UserDeckData userDeckData in DataManager.DmDeck.GetUserDeckList(UserDeckData.Category.NORMAL))
		{
			foreach (List<long> list in userDeckData.equipPhotoList)
			{
				Predicate<long> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = (long item) => item == photoPackData.dataId);
				}
				if (list.Exists(predicate))
				{
					text = string.Concat(new string[]
					{
						text,
						colorRedStartTag,
						PrjUtil.PARTY_FORMATION,
						" ",
						colorEndTag
					});
					flag = true;
					break;
				}
			}
			if (flag)
			{
				break;
			}
		}
		bool flag2 = false;
		Predicate<long> <>9__2;
		foreach (UserDeckData userDeckData2 in DataManager.DmDeck.GetUserDeckList(UserDeckData.Category.PVP))
		{
			foreach (List<long> list2 in userDeckData2.equipPhotoList)
			{
				Predicate<long> predicate2;
				if ((predicate2 = <>9__2) == null)
				{
					predicate2 = (<>9__2 = (long item) => item == photoPackData.dataId);
				}
				if (list2.Exists(predicate2))
				{
					text = string.Concat(new string[]
					{
						text,
						colorRedStartTag,
						PrjUtil.PvP_FORMATION,
						" ",
						colorEndTag
					});
					flag2 = true;
					break;
				}
			}
			if (flag2)
			{
				break;
			}
		}
		bool flag3 = false;
		Predicate<long> <>9__3;
		foreach (UserDeckData userDeckData3 in DataManager.DmDeck.GetUserDeckList(UserDeckData.Category.TRAINING))
		{
			foreach (List<long> list3 in userDeckData3.equipPhotoList)
			{
				Predicate<long> predicate3;
				if ((predicate3 = <>9__3) == null)
				{
					predicate3 = (<>9__3 = (long item) => item == photoPackData.dataId);
				}
				if (list3.Exists(predicate3))
				{
					text = string.Concat(new string[]
					{
						text,
						colorRedStartTag,
						PrjUtil.TRAINING_FORMATION,
						" ",
						colorEndTag
					});
					flag3 = true;
					break;
				}
			}
			if (flag3)
			{
				break;
			}
		}
		Predicate<long> <>9__4;
		foreach (LoanPackData loanPackData in DataManager.DmUserInfo.loanPackList)
		{
			List<long> photoDataIdList = loanPackData.photoDataIdList;
			Predicate<long> predicate4;
			if ((predicate4 = <>9__4) == null)
			{
				predicate4 = (<>9__4 = (long item) => item == photoPackData.dataId);
			}
			if (photoDataIdList.Exists(predicate4))
			{
				text = string.Concat(new string[]
				{
					text,
					colorRedStartTag,
					PrjUtil.HELPER_FRIENDS,
					" ",
					colorEndTag
				});
				break;
			}
		}
		text += "で編成中です。\nこのフォトがすべての編成から外れますが\nよろしいですか？";
		CanvasManager.HdlOpenWindowItemInfo.SetupItemInfoNeedIconPhotoCtrl(iconPhotoCtrl, true, "編成解除確認", text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, delegate(int index)
		{
			if (index == 1)
			{
				Singleton<SceneManager>.Instance.StartCoroutine(requestServer);
			}
			return true;
		}, null);
		CanvasManager.HdlOpenWindowItemInfo.Open();
	}

	public static List<PhotoPackData> GetDropItemEffectPhotoDeck(QuestOnePackData qopd, List<PhotoPackData> dropItemEffectPhotoList)
	{
		if (qopd != null)
		{
			List<DataManagerPhoto.PhotoDropItemData> dropItemList = DataManager.DmPhoto.GetPhotoQuestDropItemList(TimeManager.Now);
			if (dropItemList != null)
			{
				dropItemList.RemoveAll((DataManagerPhoto.PhotoDropItemData item) => qopd.questOne.questId != item.TargetId);
				dropItemEffectPhotoList.RemoveAll((PhotoPackData item) => !dropItemList.Exists((DataManagerPhoto.PhotoDropItemData drop) => drop.PhotoId == item.staticData.GetId()));
			}
			return dropItemEffectPhotoList;
		}
		return new List<PhotoPackData>();
	}

	public static void RefDropItemEffectPhotoList(ref List<PhotoPackData> dropItemEffectPhotoList, PhotoPackData epd, bool isHelper)
	{
		if (epd == null)
		{
			return;
		}
		DataManagerPhoto.PhotoDropItemData photoDropItemData = DataManager.DmPhoto.GetPhotoQuestDropItemList(TimeManager.Now).Find((DataManagerPhoto.PhotoDropItemData item) => item.PhotoId == epd.staticData.GetId());
		if (photoDropItemData != null)
		{
			if (isHelper)
			{
				if (photoDropItemData.HelperEnabled)
				{
					dropItemEffectPhotoList.Add(epd);
					return;
				}
			}
			else
			{
				dropItemEffectPhotoList.Add(epd);
			}
		}
	}

	public static bool IsLevelLimitOverPhoto(PhotoPackData ppd)
	{
		return ppd != null && ppd.staticData.baseData.expPhotoType == PhotoDef.ExpPhotoType.LevelLimitOver;
	}

	public static DataManagerPhoto.PhotoLevelupResult CalcPhotoGrow(PhotoPackData basePhoto, List<PhotoPackData> feedPhotoList)
	{
		DataManagerPhoto.PhotoLevelupResult photoLevelupResult = new DataManagerPhoto.PhotoLevelupResult();
		photoLevelupResult.SetBasePhoto(basePhoto);
		photoLevelupResult.level = photoLevelupResult.befLevel;
		photoLevelupResult.exp = photoLevelupResult.befExp;
		photoLevelupResult.BonusRrityList = new List<ItemDef.Rarity>();
		feedPhotoList = feedPhotoList ?? new List<PhotoPackData>();
		Dictionary<ItemDef.Rarity, int> dictionary = new Dictionary<ItemDef.Rarity, int>();
		foreach (PhotoPackData photoPackData in feedPhotoList)
		{
			int num = ((PhotoDef.ExpPhotoType.Experience == photoPackData.staticData.baseData.expPhotoType) ? photoPackData.staticData.rarityData.expPhotoStrengBase : photoPackData.staticData.rarityData.photoStrengBase);
			int num2 = 0;
			if (photoPackData.dynamicData.level == 1)
			{
				ItemDef.Rarity rarity = photoPackData.staticData.GetRarity();
				if (rarity - ItemDef.Rarity.STAR1 <= 3)
				{
					if (!dictionary.ContainsKey(photoPackData.staticData.GetRarity()))
					{
						dictionary.Add(photoPackData.staticData.GetRarity(), 0);
					}
					Dictionary<ItemDef.Rarity, int> dictionary2 = dictionary;
					ItemDef.Rarity rarity2 = photoPackData.staticData.GetRarity();
					int num3 = dictionary2[rarity2] + 1;
					dictionary2[rarity2] = num3;
					if (dictionary[photoPackData.staticData.GetRarity()] % 10 == 0)
					{
						num2 = DataManager.DmPhoto.GetPhotoBonuExpData(photoPackData.staticData.GetRarity());
						if (!photoLevelupResult.BonusRrityList.Contains(photoPackData.staticData.GetRarity()))
						{
							photoLevelupResult.BonusRrityList.Add(photoPackData.staticData.GetRarity());
						}
					}
				}
			}
			num *= photoPackData.dynamicData.level;
			photoLevelupResult.exp += (long)num;
			photoLevelupResult.exp += (long)num2;
		}
		photoLevelupResult.BonusRrityList.Sort();
		List<PhotoPackData> list = feedPhotoList.FindAll((PhotoPackData feed) => feed.staticData.GetId() == basePhoto.staticData.GetId() || PhotoUtil.IsLevelLimitOverPhoto(feed));
		photoLevelupResult.levelRank = basePhoto.dynamicData.levelRank + list.Count;
		foreach (PhotoPackData photoPackData2 in list)
		{
			photoLevelupResult.levelRank += photoPackData2.dynamicData.levelRank;
		}
		photoLevelupResult.levelRank = Math.Min(photoLevelupResult.levelRank, 4);
		for (;;)
		{
			long expByNextLevel = DataManager.DmPhoto.GetExpByNextLevel(photoLevelupResult.itemId, photoLevelupResult.level, photoLevelupResult.levelRank);
			if (expByNextLevel <= 0L || photoLevelupResult.exp < expByNextLevel)
			{
				break;
			}
			photoLevelupResult.exp -= expByNextLevel;
			DataManagerPhoto.PhotoLevelupResult photoLevelupResult2 = photoLevelupResult;
			int num3 = photoLevelupResult2.level;
			photoLevelupResult2.level = num3 + 1;
		}
		photoLevelupResult.limitLevel = basePhoto.calcLimitLevel(photoLevelupResult.levelRank);
		if (photoLevelupResult.level >= photoLevelupResult.limitLevel)
		{
			photoLevelupResult.level = photoLevelupResult.limitLevel;
			photoLevelupResult.exp = 0L;
		}
		return photoLevelupResult;
	}

	public static readonly string PhotoPocketNotReleasedText = "未解放";

	public static readonly string NoSelectedText = "選択不可";

	public static readonly string PhotoTypeText = "フォトタイプ";

	public static readonly string FriendsText = "フレンズ";

	public static readonly string SamePhotoText = "同一フォト";

	public static readonly string StoryPhotoText = "ストーリーフォト";

	public static readonly string NoFormationText = "編成不可";

	public static readonly string NoStrengthenText = "強化対象外";

	public class SizeChangeBtnGUI
	{
		public SizeChangeBtnGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_SizeChange = baseTr.GetComponent<PguiButtonCtrl>();
			this.Txt = baseTr.Find("BaseImage/On/Txt").GetComponent<PguiTextCtrl>();
		}

		public void Setup(PhotoUtil.SizeChangeBtnGUI.SetupParam param)
		{
			this.setupParam = param;
			this.scrollSize = this.setupParam.refScrollView.Size;
			this.Btn_SizeChange.AddOnClickListener(delegate(PguiButtonCtrl btn)
			{
				if (btn == this.Btn_SizeChange)
				{
					this.setupParam.sizeIndex--;
					this.setupParam.sizeIndex = (this.setupParam.sizeIndex + this.setupParam.iconPhotoParamList.Count) % this.setupParam.iconPhotoParamList.Count;
					this.ResetScrollView();
					this.setupParam.funcResult(new PhotoUtil.SizeChangeBtnGUI.ResultParam
					{
						sizeIndex = this.setupParam.sizeIndex
					});
				}
			}, PguiButtonCtrl.SoundType.DEFAULT);
		}

		private void UpdateText(int index)
		{
			switch (index)
			{
			case 0:
				this.Txt.text = PrjUtil.MakeMessage("小");
				return;
			case 1:
				this.Txt.text = PrjUtil.MakeMessage("中");
				return;
			case 2:
				this.Txt.text = PrjUtil.MakeMessage("大");
				return;
			case 3:
				this.Txt.text = PrjUtil.MakeMessage("特大");
				return;
			default:
				this.Txt.text = PrjUtil.MakeMessage("エラー");
				return;
			}
		}

		public void ResetScrollView()
		{
			this.UpdateText(this.setupParam.sizeIndex);
			this.setupParam.refScrollView.Clear();
			this.setupParam.refScrollView.SetPrefab(this.setupParam.iconPhotoParamList[this.setupParam.sizeIndex].prefab);
			UnityAction resetCallback = this.setupParam.resetCallback;
			if (resetCallback != null)
			{
				resetCallback();
			}
			if (this.setupParam.refScrollView.onStartItem != null)
			{
				ReuseScroll refScrollView = this.setupParam.refScrollView;
				refScrollView.onStartItem = (Action<int, GameObject>)Delegate.Remove(refScrollView.onStartItem, this.setupParam.onStartItem);
			}
			if (this.setupParam.refScrollView.onUpdateItem != null)
			{
				ReuseScroll refScrollView2 = this.setupParam.refScrollView;
				refScrollView2.onUpdateItem = (Action<int, GameObject>)Delegate.Remove(refScrollView2.onUpdateItem, this.setupParam.onUpdateItem);
			}
			ReuseScroll refScrollView3 = this.setupParam.refScrollView;
			refScrollView3.onStartItem = (Action<int, GameObject>)Delegate.Combine(refScrollView3.onStartItem, this.setupParam.onStartItem);
			ReuseScroll refScrollView4 = this.setupParam.refScrollView;
			refScrollView4.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(refScrollView4.onUpdateItem, this.setupParam.onUpdateItem);
			this.setupParam.refScrollView.Size = this.scrollSize * this.setupParam.iconPhotoParamList[this.setupParam.sizeIndex].scale.y;
			int num = ((this.setupParam.dispIconPhotoCountCallback == null) ? 10 : this.setupParam.dispIconPhotoCountCallback());
			this.setupParam.refScrollView.Setup(num / this.setupParam.iconPhotoParamList[this.setupParam.sizeIndex].num + 1, 0);
			this.setupParam.refScrollView.Refresh();
		}

		public List<PhotoUtil.SizeChangeBtnGUI.IconPhotoParam> IconPhotoParamList
		{
			get
			{
				return this.setupParam.iconPhotoParamList;
			}
		}

		public int SizeIndex
		{
			get
			{
				return this.setupParam.sizeIndex;
			}
		}

		public GameObject baseObj;

		public PguiButtonCtrl Btn_SizeChange;

		public PguiTextCtrl Txt;

		private float scrollSize;

		private PhotoUtil.SizeChangeBtnGUI.SetupParam setupParam = new PhotoUtil.SizeChangeBtnGUI.SetupParam();

		public delegate void FuncResult(PhotoUtil.SizeChangeBtnGUI.ResultParam result);

		public class IconPhotoParam
		{
			public Vector3 scale;

			public Vector3 scaleCurrent;

			public Vector3 scaleCount;

			public int num;

			public GameObject prefab;
		}

		public class SetupParam
		{
			public int sizeIndex;

			public List<PhotoUtil.SizeChangeBtnGUI.IconPhotoParam> iconPhotoParamList;

			public ReuseScroll refScrollView;

			public PhotoUtil.SizeChangeBtnGUI.FuncResult funcResult;

			public Action<int, GameObject> onStartItem;

			public Action<int, GameObject> onUpdateItem;

			public UnityAction resetCallback;

			public Func<int> dispIconPhotoCountCallback;
		}

		public class ResultParam
		{
			public int sizeIndex;
		}
	}
}
