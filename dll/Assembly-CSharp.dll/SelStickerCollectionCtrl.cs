using System;
using System.Collections.Generic;
using SGNFW.Mst;
using SGNFW.uGUI;
using UnityEngine;

public class SelStickerCollectionCtrl : MonoBehaviour
{
	public void Init()
	{
		GameObject gameObject = Object.Instantiate<GameObject>((GameObject)AssetManager.GetAssetData("SceneStickerCollection/GUI/Prefab/GUI_StickerCollection"), base.transform);
		this.guiData = new SelStickerCollectionCtrl.GUI(gameObject.transform);
		this.cloneUserOptionData = DataManager.DmUserInfo.optionData.Clone();
		this.winPanel = AssetManager.InstantiateAssetData("SceneStickerCollection/GUI/Prefab/GUI_StickerBonus_Window", null);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this.winPanel.transform, true);
		int allStickerWeight = DataManager.DmSticker.GetAllStickerWeight();
		DataManager.DmSticker.GetPlayerExpBonusData(allStickerWeight);
		DataManager.DmSticker.GetPlayerExpNextBonusData(allStickerWeight);
		this.guiData.Btn_Info.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickBonusButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.sizeChangeBtnGUI.Setup(new StickerUtil.SizeChangeBtnGUI.SetupParam
		{
			funcResult = delegate(StickerUtil.SizeChangeBtnGUI.ResultParam result)
			{
				this.cloneUserOptionData.stickerIconSizeCollection = result.sizeIndex;
				DataManager.DmUserInfo.RequestActionUpdateUserOption(this.cloneUserOptionData);
			},
			iconStickerParamList = new List<StickerUtil.SizeChangeBtnGUI.IconStickerParam>
			{
				new StickerUtil.SizeChangeBtnGUI.IconStickerParam
				{
					scale = new Vector3(0.6f, 0.6f, 1f),
					num = 10,
					prefab = Object.Instantiate<GameObject>((GameObject)Resources.Load("SelCmn/GUI/Prefab/CharaPhotoAll_Icon_Photo_List_S"), base.transform)
				},
				new StickerUtil.SizeChangeBtnGUI.IconStickerParam
				{
					scale = new Vector3(0.75f, 0.75f, 1f),
					num = 8,
					prefab = Object.Instantiate<GameObject>((GameObject)Resources.Load("SelCmn/GUI/Prefab/CharaPhotoAll_Icon_Photo_List_M"), base.transform)
				},
				new StickerUtil.SizeChangeBtnGUI.IconStickerParam
				{
					scale = new Vector3(0.85f, 0.85f, 1f),
					num = 7,
					prefab = Object.Instantiate<GameObject>((GameObject)Resources.Load("SelCmn/GUI/Prefab/CharaPhotoAll_Icon_Photo_List_L"), base.transform)
				},
				new StickerUtil.SizeChangeBtnGUI.IconStickerParam
				{
					scale = new Vector3(1f, 1f, 1f),
					num = 6,
					prefab = Object.Instantiate<GameObject>((GameObject)Resources.Load("SelCmn/GUI/Prefab/CharaPhotoAll_Icon_Photo_List_XL"), base.transform)
				}
			},
			onStartItem = new Action<int, GameObject>(this.OnStartItemStickerSelect),
			onUpdateItem = new Action<int, GameObject>(this.OnUpdateItemStickerSelect),
			refScrollView = this.guiData.ScrollView,
			sizeIndex = this.cloneUserOptionData.stickerIconSizeCollection,
			dispIconStickerCountCallback = () => this.dispStickerPackList.Count
		});
	}

	public void Setup()
	{
		this.cloneUserOptionData = DataManager.DmUserInfo.optionData.Clone();
		this.guiData.sizeChangeBtnGUI.baseObj.SetActive(true);
		this.guiData.sizeChangeBtnGUI.ResetScrollView();
		this.guiData.SelCmn_AllInOut.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
		this.UpdateTextTotal();
		this.UpdateBonusText();
	}

	public void Update()
	{
		if (!this.winPanel.activeSelf)
		{
			return;
		}
		if (!this.winBonus.win.FinishedClose())
		{
			return;
		}
		this.winPanel.SetActive(false);
	}

	public void UpdateTextTotal()
	{
		List<DataManagerSticker.StickerDynamicData> allStickerDynamicData = DataManager.DmSticker.GetAllStickerDynamicData();
		Dictionary<int, DataManagerSticker.StickerStaticData> allStaticData = DataManager.DmSticker.GetAllStaticData();
		this.guiData.Txt_Kind.text = allStickerDynamicData.Count.ToString() + "/" + allStaticData.Count.ToString();
	}

	public void UpdateBonusText()
	{
		int allStickerWeight = DataManager.DmSticker.GetAllStickerWeight();
		List<DataManagerSticker.StickerDynamicData> allStickerDynamicData = DataManager.DmSticker.GetAllStickerDynamicData();
		MstStickerPlayerExpBonus playerExpBonusData = DataManager.DmSticker.GetPlayerExpBonusData(allStickerWeight);
		MstStickerPlayerExpBonus playerExpNextBonusData = DataManager.DmSticker.GetPlayerExpNextBonusData(allStickerWeight);
		if (playerExpBonusData != null)
		{
			string text = string.Format(SelStickerCollectionCtrl.STICKER_BONUS_FORMAT, (double)playerExpBonusData.playerExpIncrease / 10.0);
			this.winBonus = new SelStickerCollectionCtrl.GUI_BonusWindow(this.winPanel.transform.Find("Window_Bonus"));
			this.winBonus.ratio.text = text;
			if (playerExpNextBonusData != null)
			{
				this.winBonus.next.text = SelStickerCollectionCtrl.STICKER_NEXT_BONUS_FORMAT;
				this.winBonus.next.text = this.winBonus.next.text.Replace("Param01", (playerExpNextBonusData.weight - allStickerWeight).ToString());
			}
			else
			{
				this.winBonus.next.text = SelStickerCollectionCtrl.STICKER_BONUS_MAX_FORMAT;
			}
			int num = 0;
			foreach (DataManagerSticker.StickerDynamicData stickerDynamicData in allStickerDynamicData)
			{
				num += stickerDynamicData.num;
			}
			PguiTextCtrl next = this.winBonus.next;
			next.text += string.Format(SelStickerCollectionCtrl.STICKER_TOTAL_FORMAT, num.ToString());
			this.winBonus.win.SetupButtonOnly(null);
			this.winPanel.SetActive(false);
		}
	}

	public void SetupCollectionData()
	{
		List<DataManagerSticker.StickerStaticData> list = new List<DataManagerSticker.StickerStaticData>(DataManager.DmSticker.GetAllStaticData().Values);
		list.RemoveAll((DataManagerSticker.StickerStaticData ssd) => TimeManager.Now < ssd.startTime);
		Dictionary<SortFilterDefine.RegistrationStatus, HashSet<int>> statusMap = new Dictionary<SortFilterDefine.RegistrationStatus, HashSet<int>>();
		HashSet<int> hashSet = new HashSet<int>();
		HashSet<int> hashSet2 = new HashSet<int>();
		List<DataManagerSticker.StickerPackData> dummyDataList = new List<DataManagerSticker.StickerPackData>();
		foreach (DataManagerSticker.StickerStaticData stickerStaticData in list)
		{
			DataManagerSticker.StickerDynamicData stickerDynamicData = new DataManagerSticker.StickerDynamicData();
			stickerDynamicData.id = stickerStaticData.GetId();
			if (DataManager.DmSticker.IsExistDynamicData(stickerStaticData.GetId()))
			{
				hashSet2.Add(stickerStaticData.GetId());
			}
			else
			{
				hashSet.Add(stickerStaticData.GetId());
			}
			dummyDataList.Add(new DataManagerSticker.StickerPackData(stickerDynamicData));
		}
		statusMap.Add(SortFilterDefine.RegistrationStatus.Unregistered, hashSet);
		statusMap.Add(SortFilterDefine.RegistrationStatus.Registered, hashSet2);
		this.collectionRegistStatusMap = new Dictionary<SortFilterDefine.RegistrationStatus, HashSet<int>>(statusMap, new SortFilterDefine.RagistrationStatusEqualityComparer());
		SortWindowCtrl.RegisterData registerData = new SortWindowCtrl.RegisterData
		{
			register = SortFilterDefine.RegisterType.STICKER_COLLECTION,
			filterButton = this.guiData.sortFilterBtnGUI.Btn_FilterOnOff,
			sortButton = this.guiData.sortFilterBtnGUI.Btn_Sort,
			sortUdButton = this.guiData.sortFilterBtnGUI.Btn_SortUpDown,
			funcGetTargetBaseList = () => new SortWindowCtrl.SortTarget
			{
				stickerList = new List<DataManagerSticker.StickerPackData>(dummyDataList),
				registrationStatusMap = new Dictionary<SortFilterDefine.RegistrationStatus, HashSet<int>>(statusMap)
			},
			funcDisideTarget = delegate(SortWindowCtrl.SortTarget item)
			{
				this.dispStickerPackList = item.stickerList;
				this.sortType = item.sortType;
				this.guiData.ResizeScrollView(this.dispStickerPackList.Count, 1 + this.dispStickerPackList.Count / this.guiData.sizeChangeBtnGUI.IconStickerParamList[this.guiData.sizeChangeBtnGUI.SizeIndex].num);
			}
		};
		CanvasManager.HdlOpenWindowSortFilter.Register(registerData, true, null);
	}

	private void OnStartItemStickerSelect(int index, GameObject go)
	{
		for (int i = 0; i < this.guiData.sizeChangeBtnGUI.IconStickerParamList[this.guiData.sizeChangeBtnGUI.SizeIndex].num; i++)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Sticker, go.transform);
			gameObject.name = i.ToString();
			IconStickerCtrl component = gameObject.GetComponent<IconStickerCtrl>();
			component.AddOnClickListener(new IconStickerCtrl.OnClick(this.OnTouchStickerIcon));
			component.AddOnLongClickListener(new IconStickerCtrl.OnLongClick(this.OnTouchStickerIcon));
			component.transform.localScale = this.guiData.sizeChangeBtnGUI.IconStickerParamList[this.guiData.sizeChangeBtnGUI.SizeIndex].scale;
		}
	}

	private void OnUpdateItemStickerSelect(int index, GameObject go)
	{
		for (int i = 0; i < this.guiData.sizeChangeBtnGUI.IconStickerParamList[this.guiData.sizeChangeBtnGUI.SizeIndex].num; i++)
		{
			int num = index * this.guiData.sizeChangeBtnGUI.IconStickerParamList[this.guiData.sizeChangeBtnGUI.SizeIndex].num + i;
			DataManagerSticker.StickerPackData stickerPackData = ((num < this.dispStickerPackList.Count) ? this.dispStickerPackList[num] : null);
			IconStickerCtrl component = go.transform.Find(i.ToString()).GetComponent<IconStickerCtrl>();
			component.Setup(new IconStickerCtrl.SetupParam
			{
				spd = stickerPackData
			});
			if (stickerPackData != null)
			{
				bool flag = this.collectionRegistStatusMap[SortFilterDefine.RegistrationStatus.Unregistered].Contains(stickerPackData.staticData.GetId());
				component.DispImgDisable(flag);
			}
		}
	}

	private void OnClickBonusButton(PguiButtonCtrl button)
	{
		this.winPanel.SetActive(true);
		this.winBonus.win.ForceOpen();
	}

	private void OnTouchStickerIcon(IconStickerCtrl iconSticker)
	{
		if (this.collectionRegistStatusMap[SortFilterDefine.RegistrationStatus.Unregistered].Contains(iconSticker.stickerPackData.staticData.GetId()))
		{
			return;
		}
		SoundManager.Play("prd_se_click", false, false);
		DataManagerSticker.StickerPackData stickerPackData = null;
		List<DataManagerSticker.StickerPackData> list = new List<DataManagerSticker.StickerPackData>();
		foreach (DataManagerSticker.StickerPackData stickerPackData2 in this.dispStickerPackList)
		{
			if (!this.collectionRegistStatusMap[SortFilterDefine.RegistrationStatus.Unregistered].Contains(stickerPackData2.staticData.GetId()))
			{
				DataManagerSticker.StickerPackData stickerPackData3 = new DataManagerSticker.StickerPackData(new DataManagerSticker.StickerDynamicData
				{
					id = stickerPackData2.staticData.GetId()
				});
				if (stickerPackData2.staticData.GetId() == iconSticker.stickerPackData.staticData.GetId())
				{
					stickerPackData = stickerPackData3;
				}
				list.Add(stickerPackData3);
			}
		}
		CanvasManager.HdlStickerWindowCtrl.Open(new StickerWindowCtrl.SetupParam
		{
			spd = stickerPackData,
			isc = iconSticker,
			dispStickerPacks = list
		});
	}

	public void OnDestroyScene()
	{
		Object.Destroy(this.guiData.baseObj);
		this.guiData.baseObj = null;
		this.guiData = null;
		Object.Destroy(this.winPanel);
		this.guiData = null;
	}

	private static readonly string STICKER_BONUS_FORMAT = "探検隊EXP +{0}%";

	private static readonly string STICKER_NEXT_BONUS_FORMAT = "あと<size=30><color=#FF6E00>Param01</color></size>枚入手で効果量アップ！";

	private static readonly string STICKER_BONUS_MAX_FORMAT = "<color=#FF6E00>ボーナスが最大値に達しました</color>";

	private static readonly string STICKER_TOTAL_FORMAT = "\nシール総所持枚数：{0}枚";

	private SelStickerCollectionCtrl.GUI guiData;

	private SelStickerCollectionCtrl.GUI_BonusWindow winBonus;

	private UserOptionData cloneUserOptionData;

	private Dictionary<SortFilterDefine.RegistrationStatus, HashSet<int>> collectionRegistStatusMap = new Dictionary<SortFilterDefine.RegistrationStatus, HashSet<int>>();

	private List<DataManagerSticker.StickerPackData> dispStickerPackList = new List<DataManagerSticker.StickerPackData>();

	private SortFilterDefine.SortType sortType = SortFilterDefine.DefaultSortTypeMap[SortFilterDefine.RegisterType.STICKER_COLLECTION];

	private GameObject winPanel;

	public class GUI
	{
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.ScrollView = baseTr.Find("Collection_Top/All/WindowAll/CollectionWindow/ScrollView").GetComponent<ReuseScroll>();
			this.SelCmn_AllInOut = baseTr.Find("Collection_Top").GetComponent<SimpleAnimation>();
			this.sortFilterBtnGUI = new SelCharaPhotoAllCtrl.SortFilterBtnGUI(baseTr.Find("Collection_Top/All/WindowAll/SortFilterBtnsAll"));
			this.sizeChangeBtnGUI = new StickerUtil.SizeChangeBtnGUI(baseTr.Find("Collection_Top/All/WindowAll/SortFilterBtnsAll/Btn_SizeChange"));
			this.Txt_None = baseTr.Find("Collection_Top/All/WindowAll/CollectionWindow/Txt_None").gameObject;
			this.Txt_None.SetActive(false);
			this.Txt_Kind = baseTr.Find("Collection_Top/All/WindowAll/TotalBase/Txt_Num").GetComponent<PguiTextCtrl>();
			this.Btn_Info = baseTr.Find("Collection_Top/All/WindowAll/Btn_Info").GetComponent<PguiButtonCtrl>();
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

		public StickerUtil.SizeChangeBtnGUI sizeChangeBtnGUI;

		public GameObject Txt_None;

		public PguiTextCtrl Txt_Kind;

		public PguiButtonCtrl Btn_Info;
	}

	public class GUI_BonusWindow
	{
		public GUI_BonusWindow(Transform baseTr)
		{
			this.win = baseTr.GetComponent<PguiOpenWindowCtrl>();
			baseTr = this.win.WindowRectTransform;
			this.ratio = baseTr.Find("Info01/Txt_Effect").GetComponent<PguiTextCtrl>();
			this.next = baseTr.Find("Info01/Txt_NextEffect").GetComponent<PguiTextCtrl>();
		}

		public PguiOpenWindowCtrl win;

		public PguiTextCtrl ratio;

		public PguiTextCtrl next;
	}
}
