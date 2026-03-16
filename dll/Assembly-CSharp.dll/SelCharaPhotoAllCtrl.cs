using System;
using System.Collections.Generic;
using System.Linq;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelCharaPhotoAllCtrl : MonoBehaviour
{
	private List<CharaPackData> DispCharaPackList { get; set; }

	public List<CharaPackData> OriginalDispCharaPackList { get; private set; }

	public UnityAction PhotoAlbumButtonCallBack { get; set; }

	public void Init()
	{
		GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneCharaEdit/GUI/Prefab/GUI_CharaPhotoAll"), base.transform);
		if (!this.isDebug)
		{
			SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.FRONT, base.transform, true);
		}
		this.cloneUserOptionData = DataManager.DmUserInfo.optionData.Clone();
		this.guiData = new SelCharaPhotoAllCtrl.GUI(gameObject.transform);
		this.guiData.accessoryFilterBtnsGUI.SortFilterBar = new SortFilterBtnsAllCtrl(SortFilterDefine.RegisterType.ACCESSORY_ALL, gameObject.transform.Find("All/WindowAll/SortFilterBtnsAll_Accessory").gameObject, new UnityAction(this.OnChangedAccessorySortFilter));
		this.guiData.charaPhotoFilterBtnsGUI.sizeChangeBtnGUI.Setup(new PhotoUtil.SizeChangeBtnGUI.SetupParam
		{
			funcResult = delegate(PhotoUtil.SizeChangeBtnGUI.ResultParam result)
			{
				this.cloneUserOptionData.photoIconSizeAllView = result.sizeIndex;
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
			refScrollView = this.guiData.photoAllGUI.ScrollView,
			sizeIndex = this.cloneUserOptionData.photoIconSizeAllView,
			dispIconPhotoCountCallback = () => this.dispPhotoPackList.Count
		});
		AccessoryUtil.SizeChangeBtnGUI.DataPack[] dataPacks = AccessoryUtil.SizeChangeBtnGUI.GetDataPacks(AccessoryUtil.SizeChangeBtnGUI.DataPackType.All);
		AccessoryUtil.SizeChangeBtnGUI sizeChangeBtnGUI = this.guiData.accessoryFilterBtnsGUI.sizeChangeBtnGUI;
		AccessoryUtil.SizeChangeBtnGUI.SetupParam setupParam = new AccessoryUtil.SizeChangeBtnGUI.SetupParam();
		setupParam.funcResult = delegate(AccessoryUtil.SizeChangeBtnGUI.ResultParam result)
		{
			DataManager.DmGameStatus.RequestActionUpdateIconsideIndex(SortFilterDefine.IconPlace.AccessoryAll, result.sizeIndex);
		};
		setupParam.iconAccessoryParamList = new List<AccessoryUtil.SizeChangeBtnGUI.IconAccessoryParam>
		{
			new AccessoryUtil.SizeChangeBtnGUI.IconAccessoryParam
			{
				scale = new Vector3(0.6f, 0.6f, 1f),
				num = 10,
				prefab = Object.Instantiate<GameObject>((GameObject)Resources.Load(dataPacks[0].prefabName), base.transform)
			},
			new AccessoryUtil.SizeChangeBtnGUI.IconAccessoryParam
			{
				scale = new Vector3(0.75f, 0.75f, 1f),
				num = 8,
				prefab = Object.Instantiate<GameObject>((GameObject)Resources.Load(dataPacks[1].prefabName), base.transform)
			},
			new AccessoryUtil.SizeChangeBtnGUI.IconAccessoryParam
			{
				scale = new Vector3(0.85f, 0.85f, 1f),
				num = 7,
				prefab = Object.Instantiate<GameObject>((GameObject)Resources.Load(dataPacks[2].prefabName), base.transform)
			},
			new AccessoryUtil.SizeChangeBtnGUI.IconAccessoryParam
			{
				scale = new Vector3(1f, 1f, 1f),
				num = 6,
				prefab = Object.Instantiate<GameObject>((GameObject)Resources.Load(dataPacks[3].prefabName), base.transform)
			}
		};
		setupParam.onStartItem = new Action<int, GameObject>(this.OnStartItemAccessorySelect);
		setupParam.onUpdateItem = new Action<int, GameObject>(this.OnUpdateItemAccessorySelect);
		setupParam.refScrollView = this.guiData.accessoryAllGUI.ScrollView;
		setupParam.sizeIndex = this.GetIconSizeIndex(SortFilterDefine.IconPlace.AccessoryAll);
		setupParam.dispIconAccessoryCountCallback = () => this.dispAccessoryList.Count;
		sizeChangeBtnGUI.Setup(setupParam);
		this.guiData.radioBtnGUI.Btn_Chara.AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickToggle));
		this.guiData.radioBtnGUI.Btn_Photo.AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickToggle));
		this.guiData.radioBtnGUI.Btn_Accessory.AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickToggle));
		this.guiData.Btn_PhotoAlbum.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickPhotoAlbumButton), PguiButtonCtrl.SoundType.DEFAULT);
		ReuseScroll scrollView = this.guiData.charaAllGUI.ScrollView;
		scrollView.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollView.onStartItem, new Action<int, GameObject>(this.OnStartChara));
		ReuseScroll scrollView2 = this.guiData.charaAllGUI.ScrollView;
		scrollView2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollView2.onUpdateItem, new Action<int, GameObject>(this.OnUpdateChara));
		this.guiData.charaAllGUI.ScrollView.Setup(DataManager.DmChara.CharaStaticDataList.Count / 3 + 1, 0);
		this.scrollItemSize = this.guiData.charaAllGUI.ScrollView.Size;
	}

	private int GetIconSizeIndex(SortFilterDefine.IconPlace iconPlace)
	{
		return DataManager.DmGameStatus.MakeUserFlagData().GetIconSizeData(iconPlace).SizeIndex;
	}

	private void SetupPhotoList()
	{
		this.dispPhotoPackList = new List<PhotoPackData>(DataManager.DmPhoto.GetUserPhotoMap().Values);
	}

	private void ReloadAccessoryList()
	{
		this.dispAccessoryList = new List<DataManagerCharaAccessory.Accessory>(this.guiData.accessoryFilterBtnsGUI.SortFilterBar.GetSortFilteredAccessoryList());
	}

	public void Setup()
	{
		this.SetupPhotoList();
		this.haveCharaPackList = new List<CharaPackData>(DataManager.DmChara.GetUserCharaMap().Values);
		this.DispCharaPackList = new List<CharaPackData>(this.haveCharaPackList);
		this.OriginalDispCharaPackList = new List<CharaPackData>(this.haveCharaPackList);
		this.ReloadAccessoryList();
		this.cloneUserOptionData = DataManager.DmUserInfo.optionData.Clone();
		this.notHaveCharaStaticDataList = CharaUtil.CreateNotHaveCharaStaticDataList(this.haveCharaPackList);
		this.guiData.charaAllGUI.Txt_CharaSelect.gameObject.SetActive(this.notHaveCharaStaticDataList.Count > 0);
		CharaUtil.SetupRectTransformOutScreenRange(this.guiData.charaAllGUI.Txt_CharaSelect.gameObject, new Vector2(-1000f, 84f));
		this.ChangeMode(SelCharaPhotoAllCtrl.Mode.ModeChara, true);
		this.guiData.SelCmn_AllInOut.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
	}

	public void Dest()
	{
	}

	private void ChangeMode(SelCharaPhotoAllCtrl.Mode mode, bool sw = true)
	{
		switch (mode)
		{
		case SelCharaPhotoAllCtrl.Mode.ModeChara:
		{
			if (sw)
			{
				this.guiData.radioBtnGUI.Btn_Chara.SetToggleIndex(1);
			}
			this.guiData.radioBtnGUI.Btn_Photo.SetToggleIndex(0);
			this.guiData.radioBtnGUI.Btn_Accessory.SetToggleIndex(0);
			this.guiData.charaAllGUI.baseObj.SetActive(true);
			this.guiData.photoAllGUI.baseObj.SetActive(false);
			this.guiData.accessoryAllGUI.baseObj.SetActive(false);
			this.guiData.charaPhotoFilterBtnsGUI.baseObj.SetActive(true);
			this.guiData.charaPhotoFilterBtnsGUI.sizeChangeBtnGUI.baseObj.SetActive(false);
			this.guiData.accessoryFilterBtnsGUI.baseObj.SetActive(false);
			SortWindowCtrl.RegisterData registerData = new SortWindowCtrl.RegisterData
			{
				register = SortFilterDefine.RegisterType.CHARA_ALL,
				filterButton = this.guiData.charaPhotoFilterBtnsGUI.sortFilterBtnGUI.Btn_FilterOnOff,
				sortButton = this.guiData.charaPhotoFilterBtnsGUI.sortFilterBtnGUI.Btn_Sort,
				sortUdButton = this.guiData.charaPhotoFilterBtnsGUI.sortFilterBtnGUI.Btn_SortUpDown,
				funcGetTargetBaseList = () => new SortWindowCtrl.SortTarget
				{
					charaList = this.haveCharaPackList
				},
				funcDisideTarget = delegate(SortWindowCtrl.SortTarget item)
				{
					this.funcDisideTargetOfChara(item, true);
				}
			};
			CanvasManager.HdlOpenWindowSortFilter.Register(registerData, true, null);
			break;
		}
		case SelCharaPhotoAllCtrl.Mode.ModePhoto:
		{
			this.guiData.radioBtnGUI.Btn_Chara.SetToggleIndex(0);
			if (sw)
			{
				this.guiData.radioBtnGUI.Btn_Photo.SetToggleIndex(1);
			}
			this.guiData.radioBtnGUI.Btn_Accessory.SetToggleIndex(0);
			this.guiData.charaAllGUI.baseObj.SetActive(false);
			this.guiData.photoAllGUI.baseObj.SetActive(true);
			this.guiData.photoAllGUI.Num_Own.ReplaceTextByDefault("Param01", DataManager.DmPhoto.GetUserPhotoMap().Count.ToString() + "/" + DataManager.DmPhoto.PhotoStockLimit.ToString());
			this.guiData.accessoryAllGUI.baseObj.SetActive(false);
			this.guiData.charaPhotoFilterBtnsGUI.baseObj.SetActive(true);
			this.guiData.charaPhotoFilterBtnsGUI.sizeChangeBtnGUI.baseObj.SetActive(true);
			this.guiData.charaPhotoFilterBtnsGUI.sizeChangeBtnGUI.ResetScrollView();
			this.guiData.accessoryFilterBtnsGUI.baseObj.SetActive(false);
			SortWindowCtrl.RegisterData registerData2 = new SortWindowCtrl.RegisterData();
			registerData2.register = SortFilterDefine.RegisterType.PHOTO_ALL;
			registerData2.filterButton = this.guiData.charaPhotoFilterBtnsGUI.sortFilterBtnGUI.Btn_FilterOnOff;
			registerData2.sortButton = this.guiData.charaPhotoFilterBtnsGUI.sortFilterBtnGUI.Btn_Sort;
			registerData2.sortUdButton = this.guiData.charaPhotoFilterBtnsGUI.sortFilterBtnGUI.Btn_SortUpDown;
			registerData2.funcGetTargetBaseList = () => new SortWindowCtrl.SortTarget
			{
				photoList = new List<PhotoPackData>(DataManager.DmPhoto.GetUserPhotoMap().Values)
			};
			registerData2.funcDisideTarget = delegate(SortWindowCtrl.SortTarget item)
			{
				this.dispPhotoPackList = item.photoList;
				this.sortType = item.sortType;
				this.guiData.photoAllGUI.ResizeScrollView(this.dispPhotoPackList.Count, 1 + this.dispPhotoPackList.Count / this.guiData.charaPhotoFilterBtnsGUI.sizeChangeBtnGUI.IconPhotoParamList[this.guiData.charaPhotoFilterBtnsGUI.sizeChangeBtnGUI.SizeIndex].num);
			};
			SortWindowCtrl.RegisterData registerData3 = registerData2;
			CanvasManager.HdlOpenWindowSortFilter.Register(registerData3, true, null);
			break;
		}
		case SelCharaPhotoAllCtrl.Mode.ModeAccessory:
			this.guiData.radioBtnGUI.Btn_Chara.SetToggleIndex(0);
			this.guiData.radioBtnGUI.Btn_Photo.SetToggleIndex(0);
			if (sw)
			{
				this.guiData.radioBtnGUI.Btn_Accessory.SetToggleIndex(1);
			}
			this.guiData.charaAllGUI.baseObj.SetActive(false);
			this.guiData.photoAllGUI.baseObj.SetActive(false);
			this.guiData.accessoryAllGUI.baseObj.SetActive(true);
			this.guiData.accessoryAllGUI.Num_Own.ReplaceTextByDefault("Param01", DataManager.DmChAccessory.GetUserAccessoryList().Count.ToString() + "/" + DataManager.DmChAccessory.AccessoryStockLimit.ToString());
			this.guiData.charaPhotoFilterBtnsGUI.baseObj.SetActive(false);
			this.guiData.accessoryFilterBtnsGUI.baseObj.SetActive(true);
			this.ResizeScrollView();
			break;
		}
		this.currentMode = mode;
	}

	private void funcDisideTargetOfChara(SortWindowCtrl.SortTarget item, bool execScroll)
	{
		this.DispCharaPackList = new List<CharaPackData>(item.charaList);
		this.OriginalDispCharaPackList = new List<CharaPackData>(item.charaList);
		while (this.DispCharaPackList.Count % 3 != 0)
		{
			this.DispCharaPackList.Add(CharaPackData.MakeInvalid());
		}
		this.sortType = item.sortType;
		this.notHaveCharaStaticDataList = CharaUtil.CreateNotHaveCharaStaticDataList(this.haveCharaPackList);
		if (item.includeFriendsSearchText != null && this.notHaveCharaStaticDataList != null)
		{
			this.notHaveCharaStaticDataList = this.notHaveCharaStaticDataList.Where<CharaStaticData>((CharaStaticData data) => data.baseData.charaName.Contains(item.includeFriendsSearchText)).ToList<CharaStaticData>();
		}
		CharaUtil.FilterCharaStaticData(ref this.notHaveCharaStaticDataList, SortFilterDefine.RegisterType.CHARA_ALL);
		CharaUtil.SortCharaStaticData(ref this.notHaveCharaStaticDataList);
		if (execScroll)
		{
			int num = ((this.notHaveCharaStaticDataList.Count % 3 == 0) ? 1 : 2);
			this.guiData.charaAllGUI.ResizeScrollView(this.DispCharaPackList.Count + this.notHaveCharaStaticDataList.Count, num + (this.DispCharaPackList.Count + this.notHaveCharaStaticDataList.Count) / 3);
		}
	}

	private void Start()
	{
	}

	private void OnDestroy()
	{
		if (this.guiData != null)
		{
			Object.Destroy(this.guiData.baseObj);
			this.guiData = null;
		}
	}

	private void OnStartChara(int index, GameObject go)
	{
		GameObject gameObject = (GameObject)Resources.Load("SceneCharaEdit/GUI/Prefab/CharaGrow_Btn_CharaSelect");
		for (int i = 0; i < 3; i++)
		{
			GameObject gameObject2 = Object.Instantiate<GameObject>(gameObject, go.transform);
			IconCharaCtrl icon = gameObject2.GetComponent<IconCharaCtrl>();
			gameObject2.name = i.ToString();
			gameObject2.GetComponent<PguiButtonCtrl>().AddOnClickListener(delegate(PguiButtonCtrl button)
			{
				this.OnClickCharaButton(icon);
			}, PguiButtonCtrl.SoundType.DEFAULT);
		}
	}

	private void OnUpdateChara(int index, GameObject go)
	{
		CharaUtil.OnUpdateChara(index, go, this.DispCharaPackList, this.notHaveCharaStaticDataList, this.sortType, this.guiData.charaAllGUI.Txt_CharaSelect.gameObject, false);
	}

	public void OnClickMenuReturn(UnityAction callback = null)
	{
		this.guiData.SelCmn_AllInOut.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
		{
			UnityAction callback2 = callback;
			if (callback2 == null)
			{
				return;
			}
			callback2();
		});
	}

	private bool OnClickToggle(PguiToggleButtonCtrl pbc, int toggleIndex)
	{
		if (pbc == this.guiData.radioBtnGUI.Btn_Chara)
		{
			if (this.currentMode != SelCharaPhotoAllCtrl.Mode.ModeChara)
			{
				this.ChangeMode(SelCharaPhotoAllCtrl.Mode.ModeChara, false);
				return true;
			}
		}
		else if (pbc == this.guiData.radioBtnGUI.Btn_Photo)
		{
			if (this.currentMode != SelCharaPhotoAllCtrl.Mode.ModePhoto)
			{
				this.ChangeMode(SelCharaPhotoAllCtrl.Mode.ModePhoto, false);
				return true;
			}
		}
		else if (pbc == this.guiData.radioBtnGUI.Btn_Accessory && this.currentMode != SelCharaPhotoAllCtrl.Mode.ModeAccessory)
		{
			this.ChangeMode(SelCharaPhotoAllCtrl.Mode.ModeAccessory, false);
			return true;
		}
		return false;
	}

	private void OnClickCharaButton(IconCharaCtrl icc)
	{
		SortWindowCtrl.RegisterData registerData = CanvasManager.HdlOpenWindowSortFilter.GetRegisterData(SortFilterDefine.RegisterType.CHARA_ALL);
		if (registerData != null)
		{
			registerData.funcDisideTarget = delegate(SortWindowCtrl.SortTarget item)
			{
				this.funcDisideTargetOfChara(item, false);
			};
			CanvasManager.HdlOpenWindowSortFilter.SolutionList(SortFilterDefine.RegisterType.CHARA_ALL, null);
			registerData.funcDisideTarget = delegate(SortWindowCtrl.SortTarget item)
			{
				this.funcDisideTargetOfChara(item, true);
			};
		}
		CanvasManager.HdlCharaWindowCtrl.Open(icc.charaPackData, new CharaWindowCtrl.DetailParamSetting(CharaWindowCtrl.DetailParamSetting.Preset.MINE_DETAIL, this.OriginalDispCharaPackList), delegate
		{
			this.guiData.charaAllGUI.ScrollView.Refresh();
		});
	}

	private void OnStartItemPhotoSelect(int index, GameObject go)
	{
		for (int i = 0; i < this.guiData.charaPhotoFilterBtnsGUI.sizeChangeBtnGUI.IconPhotoParamList[this.guiData.charaPhotoFilterBtnsGUI.sizeChangeBtnGUI.SizeIndex].num; i++)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Photo, go.transform);
			gameObject.name = i.ToString();
			IconPhotoCtrl ipc = gameObject.GetComponent<IconPhotoCtrl>();
			ipc.AddOnClickListener(delegate(IconPhotoCtrl x)
			{
				this.OnTouchPhotoIcon(ipc);
			});
			ipc.transform.localScale = this.guiData.charaPhotoFilterBtnsGUI.sizeChangeBtnGUI.IconPhotoParamList[this.guiData.charaPhotoFilterBtnsGUI.sizeChangeBtnGUI.SizeIndex].scale;
		}
	}

	private void OnUpdateItemPhotoSelect(int index, GameObject go)
	{
		for (int i = 0; i < this.guiData.charaPhotoFilterBtnsGUI.sizeChangeBtnGUI.IconPhotoParamList[this.guiData.charaPhotoFilterBtnsGUI.sizeChangeBtnGUI.SizeIndex].num; i++)
		{
			int num = index * this.guiData.charaPhotoFilterBtnsGUI.sizeChangeBtnGUI.IconPhotoParamList[this.guiData.charaPhotoFilterBtnsGUI.sizeChangeBtnGUI.SizeIndex].num + i;
			PhotoPackData photoPackData = null;
			if (num < this.dispPhotoPackList.Count)
			{
				photoPackData = this.dispPhotoPackList[num];
			}
			IconPhotoCtrl component = go.transform.Find(i.ToString()).GetComponent<IconPhotoCtrl>();
			component.Setup(new IconPhotoCtrl.SetupParam
			{
				ppd = photoPackData,
				sortType = this.sortType,
				isEnableLongPress = false
			});
			component.onReturnPhotoPackDataList = () => this.dispPhotoPackList;
		}
	}

	private void OnTouchPhotoIcon(IconPhotoCtrl iconPhoto)
	{
		SoundManager.Play("prd_se_click", false, false);
		CanvasManager.HdlPhotoWindowCtrl.Open(new PhotoWindowCtrl.SetupParam
		{
			ppd = iconPhoto.photoPackData,
			dispPhotoPacks = this.dispPhotoPackList,
			closeWindowCB = delegate
			{
				ScrollRect component = this.guiData.photoAllGUI.ScrollView.GetComponent<ScrollRect>();
				float verticalNormalizedPosition = component.verticalNormalizedPosition;
				CanvasManager.HdlOpenWindowSortFilter.SolutionList(SortFilterDefine.RegisterType.PHOTO_ALL, null);
				component.verticalNormalizedPosition = verticalNormalizedPosition;
			},
			ipc = iconPhoto
		});
	}

	private void OnStartItemAccessorySelect(int index, GameObject go)
	{
		for (int i = 0; i < this.guiData.accessoryFilterBtnsGUI.sizeChangeBtnGUI.IconAccessoryParamList[this.guiData.accessoryFilterBtnsGUI.sizeChangeBtnGUI.SizeIndex].num; i++)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Accessory, go.transform);
			gameObject.name = i.ToString();
			IconAccessoryCtrl component = gameObject.GetComponent<IconAccessoryCtrl>();
			component.AddOnClickListener(new IconAccessoryCtrl.OnClick(this.OnTouchAccessoryIcon));
			component.transform.localScale = this.guiData.accessoryFilterBtnsGUI.sizeChangeBtnGUI.IconAccessoryParamList[this.guiData.accessoryFilterBtnsGUI.sizeChangeBtnGUI.SizeIndex].scale;
		}
		go.GetComponent<GridLayoutGroup>().SetLayoutHorizontal();
	}

	private void OnUpdateItemAccessorySelect(int index, GameObject go)
	{
		for (int i = 0; i < this.guiData.accessoryFilterBtnsGUI.sizeChangeBtnGUI.IconAccessoryParamList[this.guiData.accessoryFilterBtnsGUI.sizeChangeBtnGUI.SizeIndex].num; i++)
		{
			int num = index * this.guiData.accessoryFilterBtnsGUI.sizeChangeBtnGUI.IconAccessoryParamList[this.guiData.accessoryFilterBtnsGUI.sizeChangeBtnGUI.SizeIndex].num + i;
			DataManagerCharaAccessory.Accessory accessory = ((num < this.dispAccessoryList.Count) ? this.dispAccessoryList[num] : null);
			IconAccessoryCtrl component = go.transform.Find(i.ToString()).GetComponent<IconAccessoryCtrl>();
			component.Setup(new IconAccessoryCtrl.SetupParam
			{
				acce = accessory,
				sortType = this.guiData.accessoryFilterBtnsGUI.SortFilterBar.SortType,
				isEnableLongPress = false
			});
			if (accessory != null && accessory.CharaId != 0)
			{
				CharaStaticData charaStaticData = DataManager.DmChara.GetCharaStaticData(accessory.CharaId);
				component.DispIconCharaMini(charaStaticData != null, charaStaticData);
			}
		}
	}

	private void OnTouchAccessoryIcon(IconAccessoryCtrl iconAcce)
	{
		SoundManager.Play("prd_se_click", false, false);
		CanvasManager.HdlAccessoryWindowCtrl.Open(new AccessoryWindowCtrl.SetupParam
		{
			acce = iconAcce.accessory,
			acceList = this.dispAccessoryList,
			windowCloseEndCb = delegate
			{
				this.guiData.accessoryAllGUI.ScrollView.Refresh();
			}
		});
	}

	private void OnClickPhotoAlbumButton(PguiButtonCtrl button)
	{
		UnityAction photoAlbumButtonCallBack = this.PhotoAlbumButtonCallBack;
		if (photoAlbumButtonCallBack == null)
		{
			return;
		}
		photoAlbumButtonCallBack();
	}

	private void OnChangedAccessorySortFilter()
	{
		this.ReloadAccessoryList();
		this.ResizeScrollView();
	}

	private void ResizeScrollView()
	{
		this.guiData.accessoryFilterBtnsGUI.sizeChangeBtnGUI.ResetScrollView();
		List<DataManagerCharaAccessory.Accessory> userAccessoryList = DataManager.DmChAccessory.GetUserAccessoryList();
		this.guiData.accessoryAllGUI.Txt_None_Noitem.SetActive(userAccessoryList.Count <= 0);
		this.guiData.accessoryAllGUI.ResizeScrollView(this.dispAccessoryList.Count, this.dispAccessoryList.Count / this.guiData.accessoryFilterBtnsGUI.sizeChangeBtnGUI.IconAccessoryParamList[this.guiData.accessoryFilterBtnsGUI.sizeChangeBtnGUI.SizeIndex].num + 1);
	}

	public bool isDebug;

	private SelCharaPhotoAllCtrl.GUI guiData;

	private SelCharaPhotoAllCtrl.Mode currentMode;

	private List<PhotoPackData> dispPhotoPackList;

	private List<CharaPackData> haveCharaPackList;

	private List<DataManagerCharaAccessory.Accessory> dispAccessoryList;

	private SortFilterDefine.SortType sortType = SortFilterDefine.SortType.LEVEL;

	private float scrollItemSize;

	private List<CharaStaticData> notHaveCharaStaticDataList;

	private UserOptionData cloneUserOptionData;

	public enum Mode
	{
		ModeChara,
		ModePhoto,
		ModeAccessory
	}

	public class GUI
	{
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.charaAllGUI = new SelCharaPhotoAllCtrl.CharaAllGUI(baseTr.Find("All/WindowAll/CharaAll"));
			this.photoAllGUI = new SelCharaPhotoAllCtrl.PhotoAllGUI(baseTr.Find("All/WindowAll/PhotoAll"));
			this.accessoryAllGUI = new SelCharaPhotoAllCtrl.AccessoryAllGUI(baseTr.Find("All/WindowAll/AccessoryAll"));
			this.radioBtnGUI = new SelCharaPhotoAllCtrl.RadioBtnGUI(baseTr.Find("All/RadioBtns"));
			this.SelCmn_AllInOut = baseTr.GetComponent<SimpleAnimation>();
			this.charaPhotoFilterBtnsGUI = new SelCharaPhotoAllCtrl.CharaPhotoSortFilterBtnsAllGUI(baseTr.Find("All/WindowAll/SortFilterBtnsAll"));
			this.accessoryFilterBtnsGUI = new SelCharaPhotoAllCtrl.AccessorySortFilterBtnsAllGUI(baseTr.Find("All/WindowAll/SortFilterBtnsAll_Accessory"));
			this.Btn_PhotoAlbum = baseTr.Find("All/Btn_PhotoAlbum").GetComponent<PguiButtonCtrl>();
		}

		public GameObject baseObj;

		public SelCharaPhotoAllCtrl.CharaAllGUI charaAllGUI;

		public SelCharaPhotoAllCtrl.PhotoAllGUI photoAllGUI;

		public SelCharaPhotoAllCtrl.AccessoryAllGUI accessoryAllGUI;

		public SelCharaPhotoAllCtrl.RadioBtnGUI radioBtnGUI;

		public SimpleAnimation SelCmn_AllInOut;

		public SelCharaPhotoAllCtrl.CharaPhotoSortFilterBtnsAllGUI charaPhotoFilterBtnsGUI;

		public SelCharaPhotoAllCtrl.AccessorySortFilterBtnsAllGUI accessoryFilterBtnsGUI;

		public PguiButtonCtrl Btn_PhotoAlbum;
	}

	public class CharaAllGUI
	{
		public CharaAllGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.ScrollView = baseTr.Find("ScrollView").GetComponent<ReuseScroll>();
			this.Txt_CharaSelect = baseTr.Find("Txt_CharaSelect").GetComponent<PguiTextCtrl>();
			this.Txt_None = baseTr.Find("Txt_None").gameObject;
			this.Txt_None.SetActive(false);
		}

		public void ResizeScrollView(int count, int resize)
		{
			this.Txt_None.SetActive(count <= 0);
			this.ScrollView.Resize(resize, 0);
		}

		public GameObject baseObj;

		public ReuseScroll ScrollView;

		public PguiTextCtrl Txt_CharaSelect;

		public GameObject Txt_None;
	}

	public class PhotoAllGUI
	{
		public PhotoAllGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.ScrollView = baseTr.Find("ScrollView").GetComponent<ReuseScroll>();
			this.Num_Own = baseTr.Find("Num_Own").GetComponent<PguiTextCtrl>();
			this.Txt_None = baseTr.Find("Txt_None").gameObject;
			this.Txt_None.SetActive(false);
		}

		public void ResizeScrollView(int count, int resize)
		{
			this.Txt_None.SetActive(count <= 0);
			this.ScrollView.Resize(resize, 0);
		}

		public GameObject baseObj;

		public ReuseScroll ScrollView;

		public PguiTextCtrl Num_Own;

		public GameObject Txt_None;
	}

	public class AccessoryAllGUI
	{
		public AccessoryAllGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.ScrollView = baseTr.Find("ScrollView").GetComponent<ReuseScroll>();
			this.Num_Own = baseTr.Find("Num_Own").GetComponent<PguiTextCtrl>();
			this.Txt_None_Nofilter = baseTr.Find("Txt_None_Nofilter").gameObject;
			this.Txt_None_Nofilter.SetActive(false);
			this.Txt_None_Noitem = baseTr.Find("Txt_None_Noitem").gameObject;
			this.Txt_None_Noitem.SetActive(false);
		}

		public void ResizeScrollView(int count, int resize)
		{
			if (this.Txt_None_Noitem.activeSelf)
			{
				this.Txt_None_Nofilter.SetActive(false);
			}
			else
			{
				this.Txt_None_Nofilter.SetActive(count <= 0);
			}
			this.ScrollView.Resize(resize, 0);
		}

		public GameObject baseObj;

		public ReuseScroll ScrollView;

		public PguiTextCtrl Num_Own;

		public GameObject Txt_None_Nofilter;

		public GameObject Txt_None_Noitem;
	}

	public class RadioBtnGUI
	{
		public RadioBtnGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_Chara = baseTr.Find("Btn_Left").GetComponent<PguiToggleButtonCtrl>();
			this.Btn_Photo = baseTr.Find("Btn_Right").GetComponent<PguiToggleButtonCtrl>();
			this.Btn_Accessory = baseTr.Find("Btn_Right02").GetComponent<PguiToggleButtonCtrl>();
		}

		public GameObject baseObj;

		public PguiToggleButtonCtrl Btn_Chara;

		public PguiToggleButtonCtrl Btn_Photo;

		public PguiToggleButtonCtrl Btn_Accessory;
	}

	public class CharaPhotoSortFilterBtnsAllGUI
	{
		public CharaPhotoSortFilterBtnsAllGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.sortFilterBtnGUI = new SelCharaPhotoAllCtrl.SortFilterBtnGUI(baseTr);
			this.sizeChangeBtnGUI = new PhotoUtil.SizeChangeBtnGUI(baseTr.Find("Btn_SizeChange"));
		}

		public GameObject baseObj;

		public SelCharaPhotoAllCtrl.SortFilterBtnGUI sortFilterBtnGUI;

		public PhotoUtil.SizeChangeBtnGUI sizeChangeBtnGUI;
	}

	public class AccessorySortFilterBtnsAllGUI
	{
		public AccessorySortFilterBtnsAllGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.sizeChangeBtnGUI = new AccessoryUtil.SizeChangeBtnGUI(baseTr.Find("Btn_SizeChange"));
		}

		public GameObject baseObj;

		public SortFilterBtnsAllCtrl SortFilterBar;

		public AccessoryUtil.SizeChangeBtnGUI sizeChangeBtnGUI;
	}

	public class SortFilterBtnGUI
	{
		public SortFilterBtnGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_FilterOnOff = baseTr.Find("Btn_FilterOnOff").GetComponent<PguiButtonCtrl>();
			this.Btn_Sort = baseTr.Find("Btn_Sort").GetComponent<PguiButtonCtrl>();
			this.Btn_SortUpDown = baseTr.Find("Btn_SortUpDown").GetComponent<PguiButtonCtrl>();
		}

		public GameObject baseObj;

		public PguiButtonCtrl Btn_FilterOnOff;

		public PguiButtonCtrl Btn_Sort;

		public PguiButtonCtrl Btn_SortUpDown;
	}
}
