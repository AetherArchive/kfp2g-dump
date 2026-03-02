using System;
using System.Collections.Generic;
using System.Linq;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000138 RID: 312
public class SelCharaPhotoAllCtrl : MonoBehaviour
{
	// Token: 0x1700034A RID: 842
	// (get) Token: 0x06001087 RID: 4231 RVA: 0x000C9BDB File Offset: 0x000C7DDB
	// (set) Token: 0x06001088 RID: 4232 RVA: 0x000C9BE3 File Offset: 0x000C7DE3
	private List<CharaPackData> DispCharaPackList { get; set; }

	// Token: 0x1700034B RID: 843
	// (get) Token: 0x06001089 RID: 4233 RVA: 0x000C9BEC File Offset: 0x000C7DEC
	// (set) Token: 0x0600108A RID: 4234 RVA: 0x000C9BF4 File Offset: 0x000C7DF4
	public List<CharaPackData> OriginalDispCharaPackList { get; private set; }

	// Token: 0x1700034C RID: 844
	// (get) Token: 0x0600108B RID: 4235 RVA: 0x000C9BFD File Offset: 0x000C7DFD
	// (set) Token: 0x0600108C RID: 4236 RVA: 0x000C9C05 File Offset: 0x000C7E05
	public UnityAction PhotoAlbumButtonCallBack { get; set; }

	// Token: 0x0600108D RID: 4237 RVA: 0x000C9C10 File Offset: 0x000C7E10
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

	// Token: 0x0600108E RID: 4238 RVA: 0x000CA17D File Offset: 0x000C837D
	private int GetIconSizeIndex(SortFilterDefine.IconPlace iconPlace)
	{
		return DataManager.DmGameStatus.MakeUserFlagData().GetIconSizeData(iconPlace).SizeIndex;
	}

	// Token: 0x0600108F RID: 4239 RVA: 0x000CA194 File Offset: 0x000C8394
	private void SetupPhotoList()
	{
		this.dispPhotoPackList = new List<PhotoPackData>(DataManager.DmPhoto.GetUserPhotoMap().Values);
	}

	// Token: 0x06001090 RID: 4240 RVA: 0x000CA1B0 File Offset: 0x000C83B0
	private void ReloadAccessoryList()
	{
		this.dispAccessoryList = new List<DataManagerCharaAccessory.Accessory>(this.guiData.accessoryFilterBtnsGUI.SortFilterBar.GetSortFilteredAccessoryList());
	}

	// Token: 0x06001091 RID: 4241 RVA: 0x000CA1D4 File Offset: 0x000C83D4
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

	// Token: 0x06001092 RID: 4242 RVA: 0x000CA2BA File Offset: 0x000C84BA
	public void Dest()
	{
	}

	// Token: 0x06001093 RID: 4243 RVA: 0x000CA2BC File Offset: 0x000C84BC
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

	// Token: 0x06001094 RID: 4244 RVA: 0x000CA728 File Offset: 0x000C8928
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

	// Token: 0x06001095 RID: 4245 RVA: 0x000CA858 File Offset: 0x000C8A58
	private void Start()
	{
	}

	// Token: 0x06001096 RID: 4246 RVA: 0x000CA85A File Offset: 0x000C8A5A
	private void OnDestroy()
	{
		if (this.guiData != null)
		{
			Object.Destroy(this.guiData.baseObj);
			this.guiData = null;
		}
	}

	// Token: 0x06001097 RID: 4247 RVA: 0x000CA87C File Offset: 0x000C8A7C
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

	// Token: 0x06001098 RID: 4248 RVA: 0x000CA8F0 File Offset: 0x000C8AF0
	private void OnUpdateChara(int index, GameObject go)
	{
		CharaUtil.OnUpdateChara(index, go, this.DispCharaPackList, this.notHaveCharaStaticDataList, this.sortType, this.guiData.charaAllGUI.Txt_CharaSelect.gameObject, false);
	}

	// Token: 0x06001099 RID: 4249 RVA: 0x000CA924 File Offset: 0x000C8B24
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

	// Token: 0x0600109A RID: 4250 RVA: 0x000CA95C File Offset: 0x000C8B5C
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

	// Token: 0x0600109B RID: 4251 RVA: 0x000CA9EC File Offset: 0x000C8BEC
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

	// Token: 0x0600109C RID: 4252 RVA: 0x000CAA60 File Offset: 0x000C8C60
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

	// Token: 0x0600109D RID: 4253 RVA: 0x000CAB4C File Offset: 0x000C8D4C
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

	// Token: 0x0600109E RID: 4254 RVA: 0x000CAC44 File Offset: 0x000C8E44
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

	// Token: 0x0600109F RID: 4255 RVA: 0x000CACA0 File Offset: 0x000C8EA0
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

	// Token: 0x060010A0 RID: 4256 RVA: 0x000CAD74 File Offset: 0x000C8F74
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

	// Token: 0x060010A1 RID: 4257 RVA: 0x000CAE94 File Offset: 0x000C9094
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

	// Token: 0x060010A2 RID: 4258 RVA: 0x000CAEE7 File Offset: 0x000C90E7
	private void OnClickPhotoAlbumButton(PguiButtonCtrl button)
	{
		UnityAction photoAlbumButtonCallBack = this.PhotoAlbumButtonCallBack;
		if (photoAlbumButtonCallBack == null)
		{
			return;
		}
		photoAlbumButtonCallBack();
	}

	// Token: 0x060010A3 RID: 4259 RVA: 0x000CAEF9 File Offset: 0x000C90F9
	private void OnChangedAccessorySortFilter()
	{
		this.ReloadAccessoryList();
		this.ResizeScrollView();
	}

	// Token: 0x060010A4 RID: 4260 RVA: 0x000CAF08 File Offset: 0x000C9108
	private void ResizeScrollView()
	{
		this.guiData.accessoryFilterBtnsGUI.sizeChangeBtnGUI.ResetScrollView();
		List<DataManagerCharaAccessory.Accessory> userAccessoryList = DataManager.DmChAccessory.GetUserAccessoryList();
		this.guiData.accessoryAllGUI.Txt_None_Noitem.SetActive(userAccessoryList.Count <= 0);
		this.guiData.accessoryAllGUI.ResizeScrollView(this.dispAccessoryList.Count, this.dispAccessoryList.Count / this.guiData.accessoryFilterBtnsGUI.sizeChangeBtnGUI.IconAccessoryParamList[this.guiData.accessoryFilterBtnsGUI.sizeChangeBtnGUI.SizeIndex].num + 1);
	}

	// Token: 0x04000E59 RID: 3673
	public bool isDebug;

	// Token: 0x04000E5A RID: 3674
	private SelCharaPhotoAllCtrl.GUI guiData;

	// Token: 0x04000E5B RID: 3675
	private SelCharaPhotoAllCtrl.Mode currentMode;

	// Token: 0x04000E5C RID: 3676
	private List<PhotoPackData> dispPhotoPackList;

	// Token: 0x04000E5D RID: 3677
	private List<CharaPackData> haveCharaPackList;

	// Token: 0x04000E60 RID: 3680
	private List<DataManagerCharaAccessory.Accessory> dispAccessoryList;

	// Token: 0x04000E61 RID: 3681
	private SortFilterDefine.SortType sortType = SortFilterDefine.SortType.LEVEL;

	// Token: 0x04000E62 RID: 3682
	private float scrollItemSize;

	// Token: 0x04000E63 RID: 3683
	private List<CharaStaticData> notHaveCharaStaticDataList;

	// Token: 0x04000E64 RID: 3684
	private UserOptionData cloneUserOptionData;

	// Token: 0x02000A30 RID: 2608
	public enum Mode
	{
		// Token: 0x04004140 RID: 16704
		ModeChara,
		// Token: 0x04004141 RID: 16705
		ModePhoto,
		// Token: 0x04004142 RID: 16706
		ModeAccessory
	}

	// Token: 0x02000A31 RID: 2609
	public class GUI
	{
		// Token: 0x06003E88 RID: 16008 RVA: 0x001EA8F0 File Offset: 0x001E8AF0
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

		// Token: 0x04004143 RID: 16707
		public GameObject baseObj;

		// Token: 0x04004144 RID: 16708
		public SelCharaPhotoAllCtrl.CharaAllGUI charaAllGUI;

		// Token: 0x04004145 RID: 16709
		public SelCharaPhotoAllCtrl.PhotoAllGUI photoAllGUI;

		// Token: 0x04004146 RID: 16710
		public SelCharaPhotoAllCtrl.AccessoryAllGUI accessoryAllGUI;

		// Token: 0x04004147 RID: 16711
		public SelCharaPhotoAllCtrl.RadioBtnGUI radioBtnGUI;

		// Token: 0x04004148 RID: 16712
		public SimpleAnimation SelCmn_AllInOut;

		// Token: 0x04004149 RID: 16713
		public SelCharaPhotoAllCtrl.CharaPhotoSortFilterBtnsAllGUI charaPhotoFilterBtnsGUI;

		// Token: 0x0400414A RID: 16714
		public SelCharaPhotoAllCtrl.AccessorySortFilterBtnsAllGUI accessoryFilterBtnsGUI;

		// Token: 0x0400414B RID: 16715
		public PguiButtonCtrl Btn_PhotoAlbum;
	}

	// Token: 0x02000A32 RID: 2610
	public class CharaAllGUI
	{
		// Token: 0x06003E89 RID: 16009 RVA: 0x001EA9B8 File Offset: 0x001E8BB8
		public CharaAllGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.ScrollView = baseTr.Find("ScrollView").GetComponent<ReuseScroll>();
			this.Txt_CharaSelect = baseTr.Find("Txt_CharaSelect").GetComponent<PguiTextCtrl>();
			this.Txt_None = baseTr.Find("Txt_None").gameObject;
			this.Txt_None.SetActive(false);
		}

		// Token: 0x06003E8A RID: 16010 RVA: 0x001EAA25 File Offset: 0x001E8C25
		public void ResizeScrollView(int count, int resize)
		{
			this.Txt_None.SetActive(count <= 0);
			this.ScrollView.Resize(resize, 0);
		}

		// Token: 0x0400414C RID: 16716
		public GameObject baseObj;

		// Token: 0x0400414D RID: 16717
		public ReuseScroll ScrollView;

		// Token: 0x0400414E RID: 16718
		public PguiTextCtrl Txt_CharaSelect;

		// Token: 0x0400414F RID: 16719
		public GameObject Txt_None;
	}

	// Token: 0x02000A33 RID: 2611
	public class PhotoAllGUI
	{
		// Token: 0x06003E8B RID: 16011 RVA: 0x001EAA48 File Offset: 0x001E8C48
		public PhotoAllGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.ScrollView = baseTr.Find("ScrollView").GetComponent<ReuseScroll>();
			this.Num_Own = baseTr.Find("Num_Own").GetComponent<PguiTextCtrl>();
			this.Txt_None = baseTr.Find("Txt_None").gameObject;
			this.Txt_None.SetActive(false);
		}

		// Token: 0x06003E8C RID: 16012 RVA: 0x001EAAB5 File Offset: 0x001E8CB5
		public void ResizeScrollView(int count, int resize)
		{
			this.Txt_None.SetActive(count <= 0);
			this.ScrollView.Resize(resize, 0);
		}

		// Token: 0x04004150 RID: 16720
		public GameObject baseObj;

		// Token: 0x04004151 RID: 16721
		public ReuseScroll ScrollView;

		// Token: 0x04004152 RID: 16722
		public PguiTextCtrl Num_Own;

		// Token: 0x04004153 RID: 16723
		public GameObject Txt_None;
	}

	// Token: 0x02000A34 RID: 2612
	public class AccessoryAllGUI
	{
		// Token: 0x06003E8D RID: 16013 RVA: 0x001EAAD8 File Offset: 0x001E8CD8
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

		// Token: 0x06003E8E RID: 16014 RVA: 0x001EAB67 File Offset: 0x001E8D67
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

		// Token: 0x04004154 RID: 16724
		public GameObject baseObj;

		// Token: 0x04004155 RID: 16725
		public ReuseScroll ScrollView;

		// Token: 0x04004156 RID: 16726
		public PguiTextCtrl Num_Own;

		// Token: 0x04004157 RID: 16727
		public GameObject Txt_None_Nofilter;

		// Token: 0x04004158 RID: 16728
		public GameObject Txt_None_Noitem;
	}

	// Token: 0x02000A35 RID: 2613
	public class RadioBtnGUI
	{
		// Token: 0x06003E8F RID: 16015 RVA: 0x001EABA4 File Offset: 0x001E8DA4
		public RadioBtnGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_Chara = baseTr.Find("Btn_Left").GetComponent<PguiToggleButtonCtrl>();
			this.Btn_Photo = baseTr.Find("Btn_Right").GetComponent<PguiToggleButtonCtrl>();
			this.Btn_Accessory = baseTr.Find("Btn_Right02").GetComponent<PguiToggleButtonCtrl>();
		}

		// Token: 0x04004159 RID: 16729
		public GameObject baseObj;

		// Token: 0x0400415A RID: 16730
		public PguiToggleButtonCtrl Btn_Chara;

		// Token: 0x0400415B RID: 16731
		public PguiToggleButtonCtrl Btn_Photo;

		// Token: 0x0400415C RID: 16732
		public PguiToggleButtonCtrl Btn_Accessory;
	}

	// Token: 0x02000A36 RID: 2614
	public class CharaPhotoSortFilterBtnsAllGUI
	{
		// Token: 0x06003E90 RID: 16016 RVA: 0x001EAC05 File Offset: 0x001E8E05
		public CharaPhotoSortFilterBtnsAllGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.sortFilterBtnGUI = new SelCharaPhotoAllCtrl.SortFilterBtnGUI(baseTr);
			this.sizeChangeBtnGUI = new PhotoUtil.SizeChangeBtnGUI(baseTr.Find("Btn_SizeChange"));
		}

		// Token: 0x0400415D RID: 16733
		public GameObject baseObj;

		// Token: 0x0400415E RID: 16734
		public SelCharaPhotoAllCtrl.SortFilterBtnGUI sortFilterBtnGUI;

		// Token: 0x0400415F RID: 16735
		public PhotoUtil.SizeChangeBtnGUI sizeChangeBtnGUI;
	}

	// Token: 0x02000A37 RID: 2615
	public class AccessorySortFilterBtnsAllGUI
	{
		// Token: 0x06003E91 RID: 16017 RVA: 0x001EAC3B File Offset: 0x001E8E3B
		public AccessorySortFilterBtnsAllGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.sizeChangeBtnGUI = new AccessoryUtil.SizeChangeBtnGUI(baseTr.Find("Btn_SizeChange"));
		}

		// Token: 0x04004160 RID: 16736
		public GameObject baseObj;

		// Token: 0x04004161 RID: 16737
		public SortFilterBtnsAllCtrl SortFilterBar;

		// Token: 0x04004162 RID: 16738
		public AccessoryUtil.SizeChangeBtnGUI sizeChangeBtnGUI;
	}

	// Token: 0x02000A38 RID: 2616
	public class SortFilterBtnGUI
	{
		// Token: 0x06003E92 RID: 16018 RVA: 0x001EAC68 File Offset: 0x001E8E68
		public SortFilterBtnGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_FilterOnOff = baseTr.Find("Btn_FilterOnOff").GetComponent<PguiButtonCtrl>();
			this.Btn_Sort = baseTr.Find("Btn_Sort").GetComponent<PguiButtonCtrl>();
			this.Btn_SortUpDown = baseTr.Find("Btn_SortUpDown").GetComponent<PguiButtonCtrl>();
		}

		// Token: 0x04004163 RID: 16739
		public GameObject baseObj;

		// Token: 0x04004164 RID: 16740
		public PguiButtonCtrl Btn_FilterOnOff;

		// Token: 0x04004165 RID: 16741
		public PguiButtonCtrl Btn_Sort;

		// Token: 0x04004166 RID: 16742
		public PguiButtonCtrl Btn_SortUpDown;
	}
}
