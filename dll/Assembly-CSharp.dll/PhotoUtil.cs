using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001B1 RID: 433
public class PhotoUtil : MonoBehaviour
{
	// Token: 0x06001D44 RID: 7492 RVA: 0x0016C11C File Offset: 0x0016A31C
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

	// Token: 0x06001D45 RID: 7493 RVA: 0x0016C4C4 File Offset: 0x0016A6C4
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

	// Token: 0x06001D46 RID: 7494 RVA: 0x0016C53C File Offset: 0x0016A73C
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

	// Token: 0x06001D47 RID: 7495 RVA: 0x0016C5A9 File Offset: 0x0016A7A9
	public static bool IsLevelLimitOverPhoto(PhotoPackData ppd)
	{
		return ppd != null && ppd.staticData.baseData.expPhotoType == PhotoDef.ExpPhotoType.LevelLimitOver;
	}

	// Token: 0x06001D48 RID: 7496 RVA: 0x0016C5C4 File Offset: 0x0016A7C4
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

	// Token: 0x04001585 RID: 5509
	public static readonly string PhotoPocketNotReleasedText = "未解放";

	// Token: 0x04001586 RID: 5510
	public static readonly string NoSelectedText = "選択不可";

	// Token: 0x04001587 RID: 5511
	public static readonly string PhotoTypeText = "フォトタイプ";

	// Token: 0x04001588 RID: 5512
	public static readonly string FriendsText = "フレンズ";

	// Token: 0x04001589 RID: 5513
	public static readonly string SamePhotoText = "同一フォト";

	// Token: 0x0400158A RID: 5514
	public static readonly string StoryPhotoText = "ストーリーフォト";

	// Token: 0x0400158B RID: 5515
	public static readonly string NoFormationText = "編成不可";

	// Token: 0x0400158C RID: 5516
	public static readonly string NoStrengthenText = "強化対象外";

	// Token: 0x02000F46 RID: 3910
	public class SizeChangeBtnGUI
	{
		// Token: 0x06004F16 RID: 20246 RVA: 0x00238EFC File Offset: 0x002370FC
		public SizeChangeBtnGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_SizeChange = baseTr.GetComponent<PguiButtonCtrl>();
			this.Txt = baseTr.Find("BaseImage/On/Txt").GetComponent<PguiTextCtrl>();
		}

		// Token: 0x06004F17 RID: 20247 RVA: 0x00238F48 File Offset: 0x00237148
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

		// Token: 0x06004F18 RID: 20248 RVA: 0x00238F80 File Offset: 0x00237180
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

		// Token: 0x06004F19 RID: 20249 RVA: 0x00239014 File Offset: 0x00237214
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

		// Token: 0x17000B6A RID: 2922
		// (get) Token: 0x06004F1A RID: 20250 RVA: 0x002391FB File Offset: 0x002373FB
		public List<PhotoUtil.SizeChangeBtnGUI.IconPhotoParam> IconPhotoParamList
		{
			get
			{
				return this.setupParam.iconPhotoParamList;
			}
		}

		// Token: 0x17000B6B RID: 2923
		// (get) Token: 0x06004F1B RID: 20251 RVA: 0x00239208 File Offset: 0x00237408
		public int SizeIndex
		{
			get
			{
				return this.setupParam.sizeIndex;
			}
		}

		// Token: 0x0400568D RID: 22157
		public GameObject baseObj;

		// Token: 0x0400568E RID: 22158
		public PguiButtonCtrl Btn_SizeChange;

		// Token: 0x0400568F RID: 22159
		public PguiTextCtrl Txt;

		// Token: 0x04005690 RID: 22160
		private float scrollSize;

		// Token: 0x04005691 RID: 22161
		private PhotoUtil.SizeChangeBtnGUI.SetupParam setupParam = new PhotoUtil.SizeChangeBtnGUI.SetupParam();

		// Token: 0x020011FF RID: 4607
		// (Invoke) Token: 0x06005780 RID: 22400
		public delegate void FuncResult(PhotoUtil.SizeChangeBtnGUI.ResultParam result);

		// Token: 0x02001200 RID: 4608
		public class IconPhotoParam
		{
			// Token: 0x0400628F RID: 25231
			public Vector3 scale;

			// Token: 0x04006290 RID: 25232
			public Vector3 scaleCurrent;

			// Token: 0x04006291 RID: 25233
			public Vector3 scaleCount;

			// Token: 0x04006292 RID: 25234
			public int num;

			// Token: 0x04006293 RID: 25235
			public GameObject prefab;
		}

		// Token: 0x02001201 RID: 4609
		public class SetupParam
		{
			// Token: 0x04006294 RID: 25236
			public int sizeIndex;

			// Token: 0x04006295 RID: 25237
			public List<PhotoUtil.SizeChangeBtnGUI.IconPhotoParam> iconPhotoParamList;

			// Token: 0x04006296 RID: 25238
			public ReuseScroll refScrollView;

			// Token: 0x04006297 RID: 25239
			public PhotoUtil.SizeChangeBtnGUI.FuncResult funcResult;

			// Token: 0x04006298 RID: 25240
			public Action<int, GameObject> onStartItem;

			// Token: 0x04006299 RID: 25241
			public Action<int, GameObject> onUpdateItem;

			// Token: 0x0400629A RID: 25242
			public UnityAction resetCallback;

			// Token: 0x0400629B RID: 25243
			public Func<int> dispIconPhotoCountCallback;
		}

		// Token: 0x02001202 RID: 4610
		public class ResultParam
		{
			// Token: 0x0400629C RID: 25244
			public int sizeIndex;
		}
	}
}
