using System;
using System.Collections.Generic;
using SGNFW.uGUI;
using UnityEngine;

public class SelPhotoAlbumCtrl : MonoBehaviour
{
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

	public void Setup()
	{
		DataManager.DmPhoto.RequestActionPhotoAlbum();
		this.cloneUserOptionData = DataManager.DmUserInfo.optionData.Clone();
		this.guiData.sizeChangeBtnGUI.baseObj.SetActive(true);
		this.guiData.sizeChangeBtnGUI.ResetScrollView();
		this.guiData.SelCmn_AllInOut.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
	}

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

	private SelPhotoAlbumCtrl.GUI guiData;

	private List<PhotoPackData> dispPhotoPackList = new List<PhotoPackData>();

	private Dictionary<SortFilterDefine.PhotoAlbumRegistrationStatus, HashSet<int>> photoAlbumRegistStatusMap = new Dictionary<SortFilterDefine.PhotoAlbumRegistrationStatus, HashSet<int>>();

	private UserOptionData cloneUserOptionData;

	private SortFilterDefine.SortType sortType = SortFilterDefine.DefaultSortTypeMap[SortFilterDefine.RegisterType.PHOTO_ALBUM];

	public class PhotoAlbumRagistrationStatusEqualityComparer : IEqualityComparer<SortFilterDefine.PhotoAlbumRegistrationStatus>
	{
		public bool Equals(SortFilterDefine.PhotoAlbumRegistrationStatus x, SortFilterDefine.PhotoAlbumRegistrationStatus y)
		{
			return x == y;
		}

		public int GetHashCode(SortFilterDefine.PhotoAlbumRegistrationStatus obj)
		{
			return (int)obj;
		}
	}

	public class GUI
	{
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

		public void ResizeScrollView(int count, int resize)
		{
			this.Txt_None.SetActive(count <= 0);
			this.ScrollView.Resize(resize, 0);
		}

		public GameObject baseObj;

		public ReuseScroll ScrollView;

		public SelCharaPhotoAllCtrl.SortFilterBtnGUI sortFilterBtnGUI;

		public SimpleAnimation SelCmn_AllInOut;

		public PhotoUtil.SizeChangeBtnGUI sizeChangeBtnGUI;

		public GameObject Txt_None;
	}
}
