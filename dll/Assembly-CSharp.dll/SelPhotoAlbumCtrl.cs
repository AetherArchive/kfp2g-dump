using System;
using System.Collections.Generic;
using SGNFW.uGUI;
using UnityEngine;

// Token: 0x02000166 RID: 358
public class SelPhotoAlbumCtrl : MonoBehaviour
{
	// Token: 0x060014E8 RID: 5352 RVA: 0x000FDD50 File Offset: 0x000FBF50
	public void Init()
	{
		GameObject gameObject = Object.Instantiate<GameObject>((GameObject)AssetManager.GetAssetData("ScenePhotoAlbum/GUI/Prefab/GUI_PhotoAlbum"), base.transform);
		this.cloneUserOptionData = DataManager.DmUserInfo.optionData.Clone();
		this.guiData = new SelPhotoAlbumCtrl.GUI(gameObject.transform);
		this.guiData.sizeChangeBtnGUI.Setup(new PhotoUtil.SizeChangeBtnGUI.SetupParam
		{
			funcResult = delegate(PhotoUtil.SizeChangeBtnGUI.ResultParam result)
			{
				this.cloneUserOptionData.photoIconSizePhotoAlbum = result.sizeIndex;
				DataManager.DmUserInfo.RequestActionUpdateUserOption(this.cloneUserOptionData);
			},
			iconPhotoParamList = new List<PhotoUtil.SizeChangeBtnGUI.IconPhotoParam>
			{
				new PhotoUtil.SizeChangeBtnGUI.IconPhotoParam
				{
					scale = new Vector3(0.6f, 0.6f, 1f),
					num = 10,
					prefab = Object.Instantiate<GameObject>((GameObject)Resources.Load("SelCmn/GUI/Prefab/CharaPhotoAll_Icon_Photo_List_S"), base.transform)
				},
				new PhotoUtil.SizeChangeBtnGUI.IconPhotoParam
				{
					scale = new Vector3(0.75f, 0.75f, 1f),
					num = 8,
					prefab = Object.Instantiate<GameObject>((GameObject)Resources.Load("SelCmn/GUI/Prefab/CharaPhotoAll_Icon_Photo_List_M"), base.transform)
				},
				new PhotoUtil.SizeChangeBtnGUI.IconPhotoParam
				{
					scale = new Vector3(0.85f, 0.85f, 1f),
					num = 7,
					prefab = Object.Instantiate<GameObject>((GameObject)Resources.Load("SelCmn/GUI/Prefab/CharaPhotoAll_Icon_Photo_List_L"), base.transform)
				},
				new PhotoUtil.SizeChangeBtnGUI.IconPhotoParam
				{
					scale = new Vector3(1f, 1f, 1f),
					num = 6,
					prefab = Object.Instantiate<GameObject>((GameObject)Resources.Load("SelCmn/GUI/Prefab/CharaPhotoAll_Icon_Photo_List_XL"), base.transform)
				}
			},
			onStartItem = new Action<int, GameObject>(this.OnStartItemPhotoSelect),
			onUpdateItem = new Action<int, GameObject>(this.OnUpdateItemPhotoSelect),
			refScrollView = this.guiData.ScrollView,
			sizeIndex = this.cloneUserOptionData.photoIconSizePhotoAlbum,
			dispIconPhotoCountCallback = () => this.dispPhotoPackList.Count
		});
	}

	// Token: 0x060014E9 RID: 5353 RVA: 0x000FDF5C File Offset: 0x000FC15C
	public void Setup()
	{
		DataManager.DmPhoto.RequestActionPhotoAlbum();
		this.cloneUserOptionData = DataManager.DmUserInfo.optionData.Clone();
		this.guiData.sizeChangeBtnGUI.baseObj.SetActive(true);
		this.guiData.sizeChangeBtnGUI.ResetScrollView();
		this.guiData.SelCmn_AllInOut.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
	}

	// Token: 0x060014EA RID: 5354 RVA: 0x000FDFC0 File Offset: 0x000FC1C0
	public void SetupAlbumData()
	{
		List<DataManagerPhoto.AlbumData> list = new List<DataManagerPhoto.AlbumData>(DataManager.DmPhoto.AlbumDataList);
		Dictionary<int, DataManagerPhoto.AlbumData> albumDataMap = new Dictionary<int, DataManagerPhoto.AlbumData>();
		foreach (DataManagerPhoto.AlbumData albumData in list)
		{
			if (!albumDataMap.ContainsKey(albumData.PhotoId))
			{
				albumDataMap.Add(albumData.PhotoId, albumData);
			}
		}
		List<PhotoStaticData> list2 = new List<PhotoStaticData>(DataManager.DmPhoto.GetPhotoStaticMap().Values);
		list2.RemoveAll((PhotoStaticData psd) => TimeManager.Now < psd.baseData.StartDateTime && !albumDataMap.ContainsKey(psd.GetId()));
		Dictionary<SortFilterDefine.PhotoAlbumRegistrationStatus, HashSet<int>> statusMap = new Dictionary<SortFilterDefine.PhotoAlbumRegistrationStatus, HashSet<int>>();
		HashSet<int> hashSet = new HashSet<int>();
		HashSet<int> hashSet2 = new HashSet<int>();
		HashSet<int> hashSet3 = new HashSet<int>();
		HashSet<int> hashSet4 = new HashSet<int>();
		List<PhotoPackData> dummyDataList = new List<PhotoPackData>();
		foreach (PhotoStaticData photoStaticData in list2)
		{
			PhotoDynamicData photoDynamicData = new PhotoDynamicData();
			photoDynamicData.dataId = -1L;
			photoDynamicData.photoId = photoStaticData.GetId();
			photoDynamicData.level = photoStaticData.getLimitLevel(0);
			photoDynamicData.levelRank = 0;
			if (albumDataMap.ContainsKey(photoStaticData.GetId()))
			{
				if (albumDataMap[photoStaticData.GetId()].IsLevelMax)
				{
					hashSet4.Add(photoStaticData.GetId());
				}
				else if (4 <= albumDataMap[photoStaticData.GetId()].LimitOverNum)
				{
					hashSet3.Add(photoStaticData.GetId());
				}
				else
				{
					hashSet2.Add(photoStaticData.GetId());
				}
				photoDynamicData.OwnerType = PhotoDynamicData.PhotoOwnerType.User;
			}
			else
			{
				hashSet.Add(photoStaticData.GetId());
				photoDynamicData.OwnerType = PhotoDynamicData.PhotoOwnerType.Undefined;
			}
			dummyDataList.Add(new PhotoPackData(photoDynamicData));
		}
		statusMap.Add(SortFilterDefine.PhotoAlbumRegistrationStatus.Unregistered, hashSet);
		statusMap.Add(SortFilterDefine.PhotoAlbumRegistrationStatus.Registered, hashSet2);
		statusMap.Add(SortFilterDefine.PhotoAlbumRegistrationStatus.BreakthroughLimitMax, hashSet3);
		statusMap.Add(SortFilterDefine.PhotoAlbumRegistrationStatus.GrowthMax, hashSet4);
		this.photoAlbumRegistStatusMap = new Dictionary<SortFilterDefine.PhotoAlbumRegistrationStatus, HashSet<int>>(statusMap, new SelPhotoAlbumCtrl.PhotoAlbumRagistrationStatusEqualityComparer());
		SortWindowCtrl.RegisterData registerData = new SortWindowCtrl.RegisterData
		{
			register = SortFilterDefine.RegisterType.PHOTO_ALBUM,
			filterButton = this.guiData.sortFilterBtnGUI.Btn_FilterOnOff,
			sortButton = this.guiData.sortFilterBtnGUI.Btn_Sort,
			sortUdButton = this.guiData.sortFilterBtnGUI.Btn_SortUpDown,
			funcGetTargetBaseList = () => new SortWindowCtrl.SortTarget
			{
				photoList = new List<PhotoPackData>(dummyDataList),
				photoAlbumRegistrationStatusMap = new Dictionary<SortFilterDefine.PhotoAlbumRegistrationStatus, HashSet<int>>(statusMap)
			},
			funcDisideTarget = delegate(SortWindowCtrl.SortTarget item)
			{
				this.dispPhotoPackList = item.photoList;
				this.sortType = item.sortType;
				this.guiData.ResizeScrollView(this.dispPhotoPackList.Count, 1 + this.dispPhotoPackList.Count / this.guiData.sizeChangeBtnGUI.IconPhotoParamList[this.guiData.sizeChangeBtnGUI.SizeIndex].num);
			}
		};
		CanvasManager.HdlOpenWindowSortFilter.Register(registerData, true, null);
	}

	// Token: 0x060014EB RID: 5355 RVA: 0x000FE298 File Offset: 0x000FC498
	private void OnStartItemPhotoSelect(int index, GameObject go)
	{
		for (int i = 0; i < this.guiData.sizeChangeBtnGUI.IconPhotoParamList[this.guiData.sizeChangeBtnGUI.SizeIndex].num; i++)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Photo, go.transform);
			gameObject.name = i.ToString();
			IconPhotoCtrl component = gameObject.GetComponent<IconPhotoCtrl>();
			component.AddOnClickListener(new IconPhotoCtrl.OnClick(this.OnTouchPhotoIcon));
			component.transform.localScale = this.guiData.sizeChangeBtnGUI.IconPhotoParamList[this.guiData.sizeChangeBtnGUI.SizeIndex].scale;
		}
	}

	// Token: 0x060014EC RID: 5356 RVA: 0x000FE34C File Offset: 0x000FC54C
	private void OnUpdateItemPhotoSelect(int index, GameObject go)
	{
		for (int i = 0; i < this.guiData.sizeChangeBtnGUI.IconPhotoParamList[this.guiData.sizeChangeBtnGUI.SizeIndex].num; i++)
		{
			int num = index * this.guiData.sizeChangeBtnGUI.IconPhotoParamList[this.guiData.sizeChangeBtnGUI.SizeIndex].num + i;
			PhotoPackData photoPackData = ((num < this.dispPhotoPackList.Count) ? this.dispPhotoPackList[num] : null);
			IconPhotoCtrl component = go.transform.Find(i.ToString()).GetComponent<IconPhotoCtrl>();
			component.Setup(new IconPhotoCtrl.SetupParam
			{
				ppd = photoPackData,
				sortType = this.sortType,
				isEnableLongPress = false
			});
			if (photoPackData != null)
			{
				component.DispDrop(false, 0);
				component.DispTextParam(false);
				if (this.photoAlbumRegistStatusMap[SortFilterDefine.PhotoAlbumRegistrationStatus.Unregistered].Contains(photoPackData.staticData.GetId()))
				{
					component.DispImgDisable(true);
					if (PhotoDef.AlbumCategory.OverOnceHave == photoPackData.staticData.baseData.albumCategory)
					{
						component.DispLockNotOwn(true);
					}
				}
				else if (this.photoAlbumRegistStatusMap[SortFilterDefine.PhotoAlbumRegistrationStatus.GrowthMax].Contains(photoPackData.staticData.GetId()))
				{
					component.DispUpto(true);
					component.DispUptoLv(true);
					if (photoPackData.staticData.baseData.imageChangeFlg)
					{
						component.DispPhotoChange(true);
					}
				}
				else if (this.photoAlbumRegistStatusMap[SortFilterDefine.PhotoAlbumRegistrationStatus.BreakthroughLimitMax].Contains(photoPackData.staticData.GetId()))
				{
					component.DispUpto(true);
					if (photoPackData.staticData.baseData.imageChangeFlg)
					{
						component.DispPhotoChange(true);
					}
				}
			}
		}
	}

	// Token: 0x060014ED RID: 5357 RVA: 0x000FE500 File Offset: 0x000FC700
	private void OnTouchPhotoIcon(IconPhotoCtrl iconPhoto)
	{
		SoundManager.Play("prd_se_click", false, false);
		if (PhotoDef.AlbumCategory.OverOnceHave == iconPhoto.photoStaticData.baseData.albumCategory && this.photoAlbumRegistStatusMap[SortFilterDefine.PhotoAlbumRegistrationStatus.Unregistered].Contains(iconPhoto.photoStaticData.GetId()))
		{
			CanvasManager.HdlOpenWindowBasic.Setup("入手方法", iconPhoto.photoStaticData.baseData.InfoGetText, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, null, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			return;
		}
		PhotoPackData photoPackData = null;
		List<PhotoPackData> list = new List<PhotoPackData>();
		foreach (PhotoPackData photoPackData2 in this.dispPhotoPackList)
		{
			if (PhotoDef.AlbumCategory.OverOnceHave != photoPackData2.staticData.baseData.albumCategory || !this.photoAlbumRegistStatusMap[SortFilterDefine.PhotoAlbumRegistrationStatus.Unregistered].Contains(photoPackData2.staticData.GetId()))
			{
				PhotoDynamicData photoDynamicData = new PhotoDynamicData();
				photoDynamicData.dataId = -1L;
				photoDynamicData.photoId = photoPackData2.staticData.GetId();
				photoDynamicData.OwnerType = photoPackData2.dynamicData.OwnerType;
				if (this.photoAlbumRegistStatusMap[SortFilterDefine.PhotoAlbumRegistrationStatus.BreakthroughLimitMax].Contains(photoPackData2.staticData.GetId()) || this.photoAlbumRegistStatusMap[SortFilterDefine.PhotoAlbumRegistrationStatus.GrowthMax].Contains(photoPackData2.staticData.GetId()))
				{
					photoDynamicData.level = photoPackData2.staticData.getLimitLevel(4);
					photoDynamicData.levelRank = 4;
				}
				else
				{
					photoDynamicData.level = photoPackData2.dynamicData.level;
					photoDynamicData.levelRank = photoPackData2.dynamicData.levelRank;
				}
				PhotoPackData photoPackData3 = new PhotoPackData(photoDynamicData);
				if (photoPackData2.staticData.GetId() == iconPhoto.photoPackData.staticData.GetId())
				{
					photoPackData = photoPackData3;
				}
				list.Add(photoPackData3);
			}
		}
		CanvasManager.HdlPhotoWindowCtrl.Open(new PhotoWindowCtrl.SetupParam
		{
			ppd = photoPackData,
			dispPhotoPacks = list,
			isPhotoAlbum = true
		});
	}

	// Token: 0x040010FF RID: 4351
	private SelPhotoAlbumCtrl.GUI guiData;

	// Token: 0x04001100 RID: 4352
	private List<PhotoPackData> dispPhotoPackList = new List<PhotoPackData>();

	// Token: 0x04001101 RID: 4353
	private Dictionary<SortFilterDefine.PhotoAlbumRegistrationStatus, HashSet<int>> photoAlbumRegistStatusMap = new Dictionary<SortFilterDefine.PhotoAlbumRegistrationStatus, HashSet<int>>();

	// Token: 0x04001102 RID: 4354
	private UserOptionData cloneUserOptionData;

	// Token: 0x04001103 RID: 4355
	private SortFilterDefine.SortType sortType = SortFilterDefine.DefaultSortTypeMap[SortFilterDefine.RegisterType.PHOTO_ALBUM];

	// Token: 0x02000BAD RID: 2989
	public class PhotoAlbumRagistrationStatusEqualityComparer : IEqualityComparer<SortFilterDefine.PhotoAlbumRegistrationStatus>
	{
		// Token: 0x060043A5 RID: 17317 RVA: 0x002039D0 File Offset: 0x00201BD0
		public bool Equals(SortFilterDefine.PhotoAlbumRegistrationStatus x, SortFilterDefine.PhotoAlbumRegistrationStatus y)
		{
			return x == y;
		}

		// Token: 0x060043A6 RID: 17318 RVA: 0x002039D6 File Offset: 0x00201BD6
		public int GetHashCode(SortFilterDefine.PhotoAlbumRegistrationStatus obj)
		{
			return (int)obj;
		}
	}

	// Token: 0x02000BAE RID: 2990
	public class GUI
	{
		// Token: 0x060043A8 RID: 17320 RVA: 0x002039E4 File Offset: 0x00201BE4
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.ScrollView = baseTr.Find("PhotoAlbum_Top/All/WindowAll/PhotoWindow/ScrollView").GetComponent<ReuseScroll>();
			this.SelCmn_AllInOut = baseTr.Find("PhotoAlbum_Top").GetComponent<SimpleAnimation>();
			this.sortFilterBtnGUI = new SelCharaPhotoAllCtrl.SortFilterBtnGUI(baseTr.Find("PhotoAlbum_Top/All/WindowAll/SortFilterBtnsAll"));
			this.sizeChangeBtnGUI = new PhotoUtil.SizeChangeBtnGUI(baseTr.Find("PhotoAlbum_Top/All/WindowAll/SortFilterBtnsAll/Btn_SizeChange"));
			this.Txt_None = baseTr.Find("PhotoAlbum_Top/All/WindowAll/PhotoWindow/Txt_None").gameObject;
			this.Txt_None.SetActive(false);
		}

		// Token: 0x060043A9 RID: 17321 RVA: 0x00203A7D File Offset: 0x00201C7D
		public void ResizeScrollView(int count, int resize)
		{
			this.Txt_None.SetActive(count <= 0);
			this.ScrollView.Resize(resize, 0);
		}

		// Token: 0x0400486C RID: 18540
		public GameObject baseObj;

		// Token: 0x0400486D RID: 18541
		public ReuseScroll ScrollView;

		// Token: 0x0400486E RID: 18542
		public SelCharaPhotoAllCtrl.SortFilterBtnGUI sortFilterBtnGUI;

		// Token: 0x0400486F RID: 18543
		public SimpleAnimation SelCmn_AllInOut;

		// Token: 0x04004870 RID: 18544
		public PhotoUtil.SizeChangeBtnGUI sizeChangeBtnGUI;

		// Token: 0x04004871 RID: 18545
		public GameObject Txt_None;
	}
}
