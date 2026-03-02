using System;
using System.Collections;
using System.Collections.Generic;
using AEAuth3;
using SGNFW.Common;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200012C RID: 300
public class SelAccessoryGrowCtrl : MonoBehaviour
{
	// Token: 0x1700033E RID: 830
	// (get) Token: 0x06000F51 RID: 3921 RVA: 0x000B7F52 File Offset: 0x000B6152
	// (set) Token: 0x06000F52 RID: 3922 RVA: 0x000B7F5A File Offset: 0x000B615A
	public SelAccessoryGrowCtrl.Mode CurrentMode { get; private set; }

	// Token: 0x1700033F RID: 831
	// (get) Token: 0x06000F53 RID: 3923 RVA: 0x000B7F63 File Offset: 0x000B6163
	// (set) Token: 0x06000F54 RID: 3924 RVA: 0x000B7F6B File Offset: 0x000B616B
	private SortFilterBtnsAllCtrl SelectSortFilterBar { get; set; }

	// Token: 0x17000340 RID: 832
	// (get) Token: 0x06000F55 RID: 3925 RVA: 0x000B7F74 File Offset: 0x000B6174
	// (set) Token: 0x06000F56 RID: 3926 RVA: 0x000B7F7C File Offset: 0x000B617C
	private SortFilterBtnsAllCtrl GrowSortFilterBar { get; set; }

	// Token: 0x17000341 RID: 833
	// (get) Token: 0x06000F57 RID: 3927 RVA: 0x000B7F85 File Offset: 0x000B6185
	// (set) Token: 0x06000F58 RID: 3928 RVA: 0x000B7F8D File Offset: 0x000B618D
	private List<DataManagerCharaAccessory.Accessory> DispAccessories { get; set; }

	// Token: 0x17000342 RID: 834
	// (get) Token: 0x06000F5A RID: 3930 RVA: 0x000B7F9F File Offset: 0x000B619F
	// (set) Token: 0x06000F59 RID: 3929 RVA: 0x000B7F96 File Offset: 0x000B6196
	private DataManagerCharaAccessory.Accessory BaseAccessory { get; set; }

	// Token: 0x06000F5B RID: 3931 RVA: 0x000B7FA7 File Offset: 0x000B61A7
	private int GetButtonSizeIndex(SortFilterDefine.IconPlace iconPlace)
	{
		return DataManager.DmGameStatus.MakeUserFlagData().GetIconSizeData(iconPlace).SizeIndex;
	}

	// Token: 0x06000F5C RID: 3932 RVA: 0x000B7FC0 File Offset: 0x000B61C0
	public void Init()
	{
		this.guiData = new SelAccessoryGrowCtrl.GUI(base.transform);
		this.SelectSortFilterBar = new SortFilterBtnsAllCtrl(SortFilterDefine.RegisterType.ACCESSORY_GROW_BASE, this.guiData.select.baseObj.transform.Find("All/WindowAll/SortFilterBtnsAll").gameObject, new UnityAction(this.RefreshSelectAccessoryList));
		this.GrowSortFilterBar = new SortFilterBtnsAllCtrl(SortFilterDefine.RegisterType.ACCESSORY_GROW_MATERIAL, this.guiData.grow.baseObj.transform.Find("All/AccessoryAll/SortFilterBtnsAll").gameObject, new UnityAction(this.RefreshGrowAccessoryList));
		this.FeedAccessories = new List<DataManagerCharaAccessory.Accessory>();
		AccessoryUtil.SizeChangeBtnGUI.DataPack[] dataPacks = AccessoryUtil.SizeChangeBtnGUI.GetDataPacks(AccessoryUtil.SizeChangeBtnGUI.DataPackType.All);
		AccessoryUtil.SizeChangeBtnGUI btn_SizeChange = this.guiData.select.Btn_SizeChange;
		AccessoryUtil.SizeChangeBtnGUI.SetupParam setupParam = new AccessoryUtil.SizeChangeBtnGUI.SetupParam();
		setupParam.funcResult = delegate(AccessoryUtil.SizeChangeBtnGUI.ResultParam result)
		{
			DataManager.DmGameStatus.RequestActionUpdateIconsideIndex(SortFilterDefine.IconPlace.AccessoryGrow, result.sizeIndex);
		};
		setupParam.iconAccessoryParamList = new List<AccessoryUtil.SizeChangeBtnGUI.IconAccessoryParam>
		{
			new AccessoryUtil.SizeChangeBtnGUI.IconAccessoryParam
			{
				scale = new Vector3(0.6f, 0.6f, 1f),
				scaleCurrent = new Vector3(0.6f, 0.6f, 1f),
				scaleCount = new Vector3(0.75f, 0.75f, 1f),
				num = dataPacks[0].num,
				prefab = Object.Instantiate<GameObject>((GameObject)Resources.Load(dataPacks[0].prefabName), base.transform)
			},
			new AccessoryUtil.SizeChangeBtnGUI.IconAccessoryParam
			{
				scale = new Vector3(0.75f, 0.75f, 1f),
				scaleCurrent = new Vector3(0.75f, 0.75f, 1f),
				scaleCount = new Vector3(0.85f, 0.85f, 1f),
				num = dataPacks[1].num,
				prefab = Object.Instantiate<GameObject>((GameObject)Resources.Load(dataPacks[1].prefabName), base.transform)
			},
			new AccessoryUtil.SizeChangeBtnGUI.IconAccessoryParam
			{
				scale = new Vector3(0.85f, 0.85f, 1f),
				scaleCurrent = new Vector3(0.85f, 0.85f, 1f),
				scaleCount = new Vector3(0.85f, 0.85f, 1f),
				num = dataPacks[2].num,
				prefab = Object.Instantiate<GameObject>((GameObject)Resources.Load(dataPacks[2].prefabName), base.transform)
			},
			new AccessoryUtil.SizeChangeBtnGUI.IconAccessoryParam
			{
				scale = new Vector3(1f, 1f, 1f),
				scaleCurrent = new Vector3(1f, 1f, 1f),
				scaleCount = new Vector3(1f, 1f, 1f),
				num = dataPacks[3].num,
				prefab = Object.Instantiate<GameObject>((GameObject)Resources.Load(dataPacks[3].prefabName), base.transform)
			}
		};
		setupParam.onStartItem = new Action<int, GameObject>(this.OnStartItemAccessorySelect);
		setupParam.onUpdateItem = new Action<int, GameObject>(this.OnUpdateItemAccessorySelect);
		setupParam.refScrollView = this.guiData.select.ScrollView;
		setupParam.sizeIndex = this.GetButtonSizeIndex(SortFilterDefine.IconPlace.AccessoryGrow);
		setupParam.dispIconAccessoryCountCallback = () => this.DispAccessories.Count;
		setupParam.resetCallback = delegate
		{
			this.guiData.selectAccessoryIcon.Clear();
		};
		btn_SizeChange.Setup(setupParam);
		AccessoryUtil.SizeChangeBtnGUI.DataPack[] dataPacks2 = AccessoryUtil.SizeChangeBtnGUI.GetDataPacks(AccessoryUtil.SizeChangeBtnGUI.DataPackType.Set);
		AccessoryUtil.SizeChangeBtnGUI btn_SizeChange2 = this.guiData.grow.Btn_SizeChange;
		AccessoryUtil.SizeChangeBtnGUI.SetupParam setupParam2 = new AccessoryUtil.SizeChangeBtnGUI.SetupParam();
		setupParam2.funcResult = delegate(AccessoryUtil.SizeChangeBtnGUI.ResultParam result)
		{
			DataManager.DmGameStatus.RequestActionUpdateIconsideIndex(SortFilterDefine.IconPlace.AccessoryGrowMaterial, result.sizeIndex);
		};
		setupParam2.iconAccessoryParamList = new List<AccessoryUtil.SizeChangeBtnGUI.IconAccessoryParam>
		{
			new AccessoryUtil.SizeChangeBtnGUI.IconAccessoryParam
			{
				scale = new Vector3(0.52f, 0.52f, 1f),
				scaleCurrent = new Vector3(0.85f, 0.85f, 1f),
				scaleCount = new Vector3(0.85f, 0.85f, 1f),
				num = dataPacks2[0].num,
				prefab = Object.Instantiate<GameObject>((GameObject)Resources.Load(dataPacks2[0].prefabName), base.transform)
			},
			new AccessoryUtil.SizeChangeBtnGUI.IconAccessoryParam
			{
				scale = new Vector3(0.6f, 0.6f, 1f),
				scaleCurrent = new Vector3(1f, 1f, 1f),
				scaleCount = new Vector3(1f, 1f, 1f),
				num = dataPacks2[1].num,
				prefab = Object.Instantiate<GameObject>((GameObject)Resources.Load(dataPacks2[1].prefabName), base.transform)
			},
			new AccessoryUtil.SizeChangeBtnGUI.IconAccessoryParam
			{
				scale = new Vector3(0.7f, 0.7f, 1f),
				scaleCurrent = new Vector3(1f, 1f, 1f),
				scaleCount = new Vector3(1f, 1f, 1f),
				num = dataPacks2[2].num,
				prefab = Object.Instantiate<GameObject>((GameObject)Resources.Load(dataPacks2[2].prefabName), base.transform)
			},
			new AccessoryUtil.SizeChangeBtnGUI.IconAccessoryParam
			{
				scale = new Vector3(1f, 1f, 1f),
				scaleCurrent = new Vector3(1f, 1f, 1f),
				scaleCount = new Vector3(1f, 1f, 1f),
				num = dataPacks2[3].num,
				prefab = Object.Instantiate<GameObject>((GameObject)Resources.Load(dataPacks2[3].prefabName), base.transform)
			}
		};
		setupParam2.onStartItem = new Action<int, GameObject>(this.OnStartItemMainFeed);
		setupParam2.onUpdateItem = new Action<int, GameObject>(this.OnUpdateItemMainFeed);
		setupParam2.refScrollView = this.guiData.grow.ScrollView;
		setupParam2.sizeIndex = this.GetButtonSizeIndex(SortFilterDefine.IconPlace.AccessoryGrowMaterial);
		setupParam2.resetCallback = delegate
		{
			this.guiData.reserveAccessoryIcon.Clear();
		};
		setupParam2.dispIconAccessoryCountCallback = () => this.DispAccessories.Count;
		btn_SizeChange2.Setup(setupParam2);
		this.guiData.grow.Icon_Accessory.AddOnClickListener(delegate(IconAccessoryCtrl x)
		{
			SoundManager.Play("prd_se_click", false, false);
			this.ReturnAccessorySelect();
		});
		this.guiData.grow.Icon_Accessory.AddOnUpdateStatus(delegate(IconAccessoryCtrl x)
		{
			this.guiData.grow.Icon_Accessory.DispLockIcon(!AccessoryUtil.IsInvalid(this.BaseAccessory) && this.BaseAccessory.IsLock);
		});
		this.guiData.select.Btn_Sale.AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			CanvasManager.HdlAccessoryWindowCtrl.ResetPrevData();
			SceneCharaEdit.Args args = new SceneCharaEdit.Args
			{
				requestMode = SceneCharaEdit.Mode.ACCESSORY_SELL
			};
			Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneCharaEdit, args);
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.grow.ButtonL.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.grow.ButtonR.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
	}

	// Token: 0x06000F5D RID: 3933 RVA: 0x000B86E8 File Offset: 0x000B68E8
	public void Setup(SelAccessoryGrowCtrl.SetupParam param)
	{
		this.setupParam = param;
		this.requestMode = SelAccessoryGrowCtrl.Mode.SELECT;
		this.CurrentMode = SelAccessoryGrowCtrl.Mode.INVALID;
		this.serverRequestGrow = null;
		this.guiData.select.baseObj.SetActive(false);
		this.guiData.grow.baseObj.SetActive(false);
		this.RefreshSelect();
		this.guiData.select.Btn_SizeChange.ResetScrollView();
		this.guiData.grow.Btn_SizeChange.ResetScrollView();
	}

	// Token: 0x06000F5E RID: 3934 RVA: 0x000B8770 File Offset: 0x000B6970
	public void SetupBySceneForce(long AccessoryId)
	{
		DataManager.DmChAccessory.GetUserAccessoryList();
		DataManagerCharaAccessory.Accessory accessory = DataManager.DmChAccessory.GetUserAccessoryList().Find((DataManagerCharaAccessory.Accessory item) => item.UniqId == AccessoryId);
		if (AccessoryUtil.IsInvalid(accessory))
		{
			return;
		}
		this.CurrentMode = this.requestMode;
		this.SetupBaseAccessory(accessory);
		this.guiData.select.baseObj.SetActive(true);
	}

	// Token: 0x06000F5F RID: 3935 RVA: 0x000B87E3 File Offset: 0x000B69E3
	public void Dest()
	{
		this.Reset();
		this.guiData.select.baseObj.SetActive(false);
		this.guiData.grow.baseObj.SetActive(false);
	}

	// Token: 0x06000F60 RID: 3936 RVA: 0x000B8818 File Offset: 0x000B6A18
	private void RefreshSelectAccessoryList()
	{
		this.DispAccessories = this.SelectSortFilterBar.GetSortFilteredAccessoryList();
		this.guiData.select.Btn_SizeChange.ResetScrollView();
		List<DataManagerCharaAccessory.Accessory> userAccessoryList = DataManager.DmChAccessory.GetUserAccessoryList();
		this.guiData.select.Txt_None_Noitem.SetActive(userAccessoryList.Count <= 0);
		this.guiData.select.ResizeScrollView(this.DispAccessories.Count, this.DispAccessories.Count / this.guiData.select.Btn_SizeChange.IconAccessoryParamList[this.guiData.select.Btn_SizeChange.SizeIndex].num + 1);
	}

	// Token: 0x06000F61 RID: 3937 RVA: 0x000B88D4 File Offset: 0x000B6AD4
	private void RefreshGrowAccessoryList()
	{
		this.GrowSortFilterBar.GrowTargetAccessory = this.BaseAccessory;
		this.DispAccessories = this.GrowSortFilterBar.GetSortFilteredAccessoryList();
		this.guiData.grow.Btn_SizeChange.ResetScrollView();
		this.guiData.grow.ResizeScrollView(this.DispAccessories.Count, this.DispAccessories.Count / this.guiData.grow.Btn_SizeChange.IconAccessoryParamList[this.guiData.grow.Btn_SizeChange.SizeIndex].num + 1);
	}

	// Token: 0x06000F62 RID: 3938 RVA: 0x000B8975 File Offset: 0x000B6B75
	private void RefreshSelect()
	{
		this.RefreshSelectAccessoryList();
	}

	// Token: 0x06000F63 RID: 3939 RVA: 0x000B8980 File Offset: 0x000B6B80
	private void RefreshGrow()
	{
		this.ClearFeedAccessories();
		this.guiData.grow.Icon_Accessory.Setup(new IconAccessoryCtrl.SetupParam
		{
			acce = this.BaseAccessory
		});
		CharaStaticData charaStaticData = null;
		if (AccessoryUtil.IsDecidedOwner(this.BaseAccessory))
		{
			charaStaticData = DataManager.DmChara.GetCharaStaticData(this.BaseAccessory.CharaId);
		}
		this.guiData.grow.Icon_Accessory.DispIconCharaMini(charaStaticData != null, charaStaticData);
		this.guiData.grow.AccessoryName.text = this.BaseAccessory.AccessoryData.Name;
		this.guiData.grow.Txt02.text = AccessoryUtil.MakeDispTypeString(this.BaseAccessory);
		this.guiData.grow.Num_Lv_Before.ReplaceTextByDefault("Param01", AccessoryUtil.MakeLevelString(this.BaseAccessory, false));
		this.guiData.grow.Num_Lv_After.ReplaceTextByDefault(new string[] { "Param01", "Param02", "Param03" }, new string[]
		{
			AccessoryUtil.GetLevelString(this.BaseAccessory, false),
			AccessoryUtil.GetLevelString(this.BaseAccessory, true),
			AccessoryUtil.GetColorString(true)
		});
		this.guiData.grow.SetupParams(this.BaseAccessory);
		this.SettingGrowBySelectInfo();
		this.RefreshGrowAccessoryList();
	}

	// Token: 0x06000F64 RID: 3940 RVA: 0x000B8AE5 File Offset: 0x000B6CE5
	public void SetActive(bool a)
	{
		this.guiData.baseObj.SetActive(a);
	}

	// Token: 0x06000F65 RID: 3941 RVA: 0x000B8AF8 File Offset: 0x000B6CF8
	private void OnClickButton(PguiButtonCtrl button)
	{
		if (this.CurrentMode != SelAccessoryGrowCtrl.Mode.GROW)
		{
			return;
		}
		if (button == this.guiData.grow.ButtonL)
		{
			this.ClearFeedAccessories();
			this.SettingGrowBySelectInfo();
			this.guiData.grow.ScrollView.Refresh();
			return;
		}
		if (button == this.guiData.grow.ButtonR)
		{
			CanvasManager.HdlAccessoryCheckWindowCtrl.OpenGrowth(this.FeedAccessories, delegate
			{
				this.serverRequestGrow = this.ServerRequestGrow();
			});
		}
	}

	// Token: 0x06000F66 RID: 3942 RVA: 0x000B8B7D File Offset: 0x000B6D7D
	private void OnUpdateAccessoryLock(IconAccessoryCtrl icon)
	{
		if (icon.accessory.IsLock && this.FeedAccessories.Contains(icon.accessory))
		{
			this.OnTouchAccessoryIcon(SelAccessoryGrowCtrl.Type.GROW, icon);
		}
		this.guiData.grow.ScrollView.Refresh();
	}

	// Token: 0x06000F67 RID: 3943 RVA: 0x000B8BBC File Offset: 0x000B6DBC
	private void SetupBaseAccessory(DataManagerCharaAccessory.Accessory accessory)
	{
		this.BaseAccessory = accessory;
		this.requestMode = SelAccessoryGrowCtrl.Mode.GROW;
	}

	// Token: 0x06000F68 RID: 3944 RVA: 0x000B8BCC File Offset: 0x000B6DCC
	private void OnTouchAccessoryIcon(SelAccessoryGrowCtrl.Type type, IconAccessoryCtrl iconAccessory)
	{
		SoundManager.Play("prd_se_click", false, false);
		if (type != SelAccessoryGrowCtrl.Type.SELECT || this.CurrentMode != SelAccessoryGrowCtrl.Mode.SELECT || AccessoryUtil.IsInvalid(iconAccessory.accessory))
		{
			if (type == SelAccessoryGrowCtrl.Type.GROW && this.CurrentMode == SelAccessoryGrowCtrl.Mode.GROW && !AccessoryUtil.IsInvalid(iconAccessory.accessory))
			{
				bool flag = false;
				if (this.BaseAccessory != iconAccessory.accessory)
				{
					if (iconAccessory.accessory.IsLock && this.FeedAccessories.Contains(iconAccessory.accessory))
					{
						this.FeedAccessories.Remove(iconAccessory.accessory);
						flag = true;
					}
					else if (!iconAccessory.accessory.IsLock && !iconAccessory.CheckImgDisable())
					{
						if (this.FeedAccessories.Contains(iconAccessory.accessory))
						{
							this.FeedAccessories.Remove(iconAccessory.accessory);
							flag = true;
						}
						else
						{
							this.FeedAccessories.Add(iconAccessory.accessory);
							flag = true;
						}
					}
				}
				if (flag)
				{
					foreach (KeyValuePair<GameObject, AccessoryUtil.IconAccessorySet> keyValuePair in this.guiData.reserveAccessoryIcon)
					{
						int num = this.FeedAccessories.IndexOf(keyValuePair.Value.iconAccessoryCtrl.accessory);
						if (num >= 0)
						{
							keyValuePair.Value.currentFrame.SetActive(true);
							keyValuePair.Value.DispCount(true, (num + 1).ToString());
						}
						else
						{
							keyValuePair.Value.currentFrame.SetActive(false);
							keyValuePair.Value.DispCount(false, null);
						}
					}
					bool flag2 = this.simulateGrowLvMax;
					this.SettingGrowBySelectInfo();
					if (flag2 != this.simulateGrowLvMax)
					{
						this.guiData.grow.ScrollView.Refresh();
					}
				}
			}
			return;
		}
		if (iconAccessory.CheckImgDisable())
		{
			return;
		}
		this.SetupBaseAccessory(iconAccessory.accessory);
	}

	// Token: 0x06000F69 RID: 3945 RVA: 0x000B8DB4 File Offset: 0x000B6FB4
	private void SettingGrowBySelectInfo()
	{
		if (AccessoryUtil.IsInvalid(this.BaseAccessory))
		{
			return;
		}
		DataManagerCharaAccessory.LevelParam levelParam = this.BaseAccessory.GrowSimulate(this.FeedAccessories);
		this.guiData.grow.SetupSimulateParam(this.BaseAccessory, levelParam);
		this.simulateGrowLvMax = levelParam.Level >= this.BaseAccessory.AccessoryData.Rarity.LevelLimit;
		bool flag = this.FeedAccessories.Count > 0;
		this.guiData.grow.ButtonL.SetActEnable(flag, false, false);
		this.guiData.grow.ButtonR.SetActEnable(flag, false, false);
		this.guiData.grow.Num_SelPhoto.text = string.Format("{0}", this.FeedAccessories.Count);
	}

	// Token: 0x06000F6A RID: 3946 RVA: 0x000B8E8C File Offset: 0x000B708C
	private void OnStartItemAccessorySelect(int index, GameObject go)
	{
		for (int i = 0; i < this.guiData.select.Btn_SizeChange.IconAccessoryParamList[this.guiData.select.Btn_SizeChange.SizeIndex].num; i++)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_AccessorySet, go.transform);
			gameObject.name = i.ToString();
			AccessoryUtil.IconAccessorySet ias = new AccessoryUtil.IconAccessorySet(gameObject.transform);
			ias.iconAccessoryCtrl.AddOnClickListener(delegate(IconAccessoryCtrl x)
			{
				this.OnTouchAccessoryIcon(SelAccessoryGrowCtrl.Type.SELECT, ias.iconAccessoryCtrl);
			});
			ias.iconAccessoryCtrl.AddOnUpdateStatus(delegate(IconAccessoryCtrl x)
			{
				this.guiData.select.ScrollView.Refresh();
			});
			ias.baseObj.transform.Find("Icon_Accessory").localScale = this.guiData.select.Btn_SizeChange.IconAccessoryParamList[this.guiData.select.Btn_SizeChange.SizeIndex].scale;
			ias.SetScale(this.guiData.select.Btn_SizeChange.IconAccessoryParamList[this.guiData.select.Btn_SizeChange.SizeIndex].scaleCurrent, this.guiData.select.Btn_SizeChange.IconAccessoryParamList[this.guiData.select.Btn_SizeChange.SizeIndex].scaleCount);
			this.guiData.selectAccessoryIcon.Add(ias.baseObj, ias);
		}
		go.GetComponent<GridLayoutGroup>().SetLayoutHorizontal();
	}

	// Token: 0x06000F6B RID: 3947 RVA: 0x000B904C File Offset: 0x000B724C
	private void OnUpdateItemAccessorySelect(int index, GameObject go)
	{
		for (int i = 0; i < this.guiData.select.Btn_SizeChange.IconAccessoryParamList[this.guiData.select.Btn_SizeChange.SizeIndex].num; i++)
		{
			int num = index * this.guiData.select.Btn_SizeChange.IconAccessoryParamList[this.guiData.select.Btn_SizeChange.SizeIndex].num + i;
			DataManagerCharaAccessory.Accessory accessory = null;
			if (num < this.DispAccessories.Count)
			{
				accessory = this.DispAccessories[num];
			}
			GameObject gameObject = go.transform.Find(i.ToString()).gameObject;
			AccessoryUtil.IconAccessorySet iconAccessorySet = this.guiData.selectAccessoryIcon[gameObject];
			if (!AccessoryUtil.IsInvalid(accessory))
			{
				iconAccessorySet.iconAccessoryCtrl.Setup(new IconAccessoryCtrl.SetupParam
				{
					acce = accessory,
					sortType = this.SelectSortFilterBar.SortType
				});
				iconAccessorySet.iconAccessoryCtrl.onReturnAccessoryList = () => this.DispAccessories;
				CharaStaticData charaStaticData = null;
				if (AccessoryUtil.IsDecidedOwner(accessory))
				{
					charaStaticData = DataManager.DmChara.GetCharaStaticData(accessory.CharaId);
				}
				iconAccessorySet.iconAccessoryCtrl.DispIconCharaMini(charaStaticData != null, charaStaticData);
				if (AccessoryUtil.CanEquipped(accessory) && accessory.Level >= accessory.AccessoryData.Rarity.LevelLimit)
				{
					iconAccessorySet.iconAccessoryCtrl.DispImgDisable(true);
					iconAccessorySet.iconAccessoryCtrl.DispTextDisable(true, PrjUtil.MakeMessage("LvMAX"), null);
				}
				else if (!AccessoryUtil.CanEquipped(accessory))
				{
					iconAccessorySet.iconAccessoryCtrl.DispImgDisable(true);
					iconAccessorySet.iconAccessoryCtrl.DispTextDisable(true, AccessoryUtil.NoSelectedText, null);
				}
				if (AccessoryUtil.IsEquipped(accessory) && !accessory.IsLock)
				{
					iconAccessorySet.iconAccessoryCtrl.DispParty(true, true);
				}
			}
			else
			{
				iconAccessorySet.iconAccessoryCtrl.Setup(new IconAccessoryCtrl.SetupParam());
			}
		}
	}

	// Token: 0x06000F6C RID: 3948 RVA: 0x000B923C File Offset: 0x000B743C
	private void OnStartItemMainFeed(int index, GameObject go)
	{
		for (int i = 0; i < this.guiData.grow.Btn_SizeChange.IconAccessoryParamList[this.guiData.grow.Btn_SizeChange.SizeIndex].num; i++)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_AccessorySet, go.transform);
			gameObject.name = i.ToString();
			AccessoryUtil.IconAccessorySet ias = new AccessoryUtil.IconAccessorySet(gameObject.transform);
			ias.iconAccessoryCtrl.AddOnClickListener(delegate(IconAccessoryCtrl x)
			{
				this.OnTouchAccessoryIcon(SelAccessoryGrowCtrl.Type.GROW, ias.iconAccessoryCtrl);
			});
			ias.iconAccessoryCtrl.AddOnUpdateStatus(delegate(IconAccessoryCtrl x)
			{
				this.OnUpdateAccessoryLock(x);
			});
			ias.baseObj.transform.Find("Icon_Accessory").localScale = this.guiData.grow.Btn_SizeChange.IconAccessoryParamList[this.guiData.grow.Btn_SizeChange.SizeIndex].scale;
			ias.SetScale(this.guiData.grow.Btn_SizeChange.IconAccessoryParamList[this.guiData.grow.Btn_SizeChange.SizeIndex].scaleCurrent, this.guiData.grow.Btn_SizeChange.IconAccessoryParamList[this.guiData.grow.Btn_SizeChange.SizeIndex].scaleCount);
			this.guiData.reserveAccessoryIcon.Add(ias.baseObj, ias);
		}
		go.GetComponent<GridLayoutGroup>().SetLayoutHorizontal();
	}

	// Token: 0x06000F6D RID: 3949 RVA: 0x000B93FC File Offset: 0x000B75FC
	private void OnUpdateItemMainFeed(int index, GameObject go)
	{
		for (int i = 0; i < this.guiData.grow.Btn_SizeChange.IconAccessoryParamList[this.guiData.grow.Btn_SizeChange.SizeIndex].num; i++)
		{
			int num = index * this.guiData.grow.Btn_SizeChange.IconAccessoryParamList[this.guiData.grow.Btn_SizeChange.SizeIndex].num + i;
			DataManagerCharaAccessory.Accessory accessory = null;
			if (num < this.DispAccessories.Count)
			{
				accessory = this.DispAccessories[num];
			}
			GameObject gameObject = go.transform.Find(i.ToString()).gameObject;
			AccessoryUtil.IconAccessorySet iconAccessorySet = this.guiData.reserveAccessoryIcon[gameObject];
			iconAccessorySet.iconAccessoryCtrl.Setup(new IconAccessoryCtrl.SetupParam
			{
				acce = accessory,
				sortType = this.GrowSortFilterBar.SortType
			});
			CharaStaticData charaStaticData = null;
			if (AccessoryUtil.IsDecidedOwner(accessory))
			{
				charaStaticData = DataManager.DmChara.GetCharaStaticData(accessory.CharaId);
			}
			iconAccessorySet.iconAccessoryCtrl.DispIconCharaMini(charaStaticData != null, charaStaticData);
			bool flag = false;
			if (!AccessoryUtil.IsInvalid(accessory) && accessory == this.BaseAccessory)
			{
				iconAccessorySet.iconAccessoryCtrl.DispImgDisable(true);
			}
			else if (AccessoryUtil.IsEquipped(accessory))
			{
				if (accessory == this.BaseAccessory || accessory.IsLock || flag || !AccessoryUtil.CanStrengthened(this.BaseAccessory, accessory))
				{
					iconAccessorySet.iconAccessoryCtrl.DispImgDisable(true);
				}
				if (!flag && !accessory.IsLock)
				{
					iconAccessorySet.iconAccessoryCtrl.DispParty(true, true);
				}
			}
			else if (!AccessoryUtil.IsInvalid(accessory) && accessory.IsLock)
			{
				iconAccessorySet.iconAccessoryCtrl.DispImgDisable(true);
			}
			else if (!flag)
			{
				if (!AccessoryUtil.CanStrengthened(this.BaseAccessory, accessory))
				{
					iconAccessorySet.iconAccessoryCtrl.DispImgDisable(true);
					iconAccessorySet.iconAccessoryCtrl.DispTextDisable(true, AccessoryUtil.NoSelectedText, null);
				}
				else
				{
					iconAccessorySet.iconAccessoryCtrl.DispImgDisable(false);
					iconAccessorySet.iconAccessoryCtrl.DispTextDisable(false, null, null);
					iconAccessorySet.iconAccessoryCtrl.DispParty(false, true);
				}
			}
			iconAccessorySet.iconAccessoryCtrl.CheckTextDisable(false, null);
			int num2 = this.FeedAccessories.IndexOf(accessory);
			if (num2 >= 0)
			{
				iconAccessorySet.currentFrame.SetActive(true);
				iconAccessorySet.DispCount(true, (num2 + 1).ToString());
			}
			else
			{
				iconAccessorySet.currentFrame.SetActive(false);
				iconAccessorySet.DispCount(false, null);
			}
			bool flag2 = false;
			if (this.simulateGrowLvMax && this.FeedAccessories.Find((DataManagerCharaAccessory.Accessory item) => item == accessory) == null)
			{
				flag2 = true;
			}
			if (flag2)
			{
				iconAccessorySet.iconAccessoryCtrl.DispImgDisable(true);
			}
		}
	}

	// Token: 0x06000F6E RID: 3950 RVA: 0x000B9714 File Offset: 0x000B7914
	private void OnDestroy()
	{
		if (this.guiData != null)
		{
			Object.Destroy(this.guiData.baseObj);
			this.guiData = null;
		}
	}

	// Token: 0x06000F6F RID: 3951 RVA: 0x000B9735 File Offset: 0x000B7935
	private void Reset()
	{
		this.ClearFeedAccessories();
	}

	// Token: 0x06000F70 RID: 3952 RVA: 0x000B9740 File Offset: 0x000B7940
	private void Update()
	{
		if (this.requestMode != this.CurrentMode)
		{
			if (this.requestMode == SelAccessoryGrowCtrl.Mode.SELECT)
			{
				if (this.CurrentMode == SelAccessoryGrowCtrl.Mode.GROW)
				{
					this.guiData.grow.AccessoryGrow_Main.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
					{
						this.guiData.grow.baseObj.SetActive(false);
						this.guiData.select.baseObj.SetActive(true);
						this.guiData.select.Num_Own.ReplaceTextByDefault("Param01", DataManager.DmChAccessory.GetUserAccessoryList().Count.ToString() + "/" + DataManager.DmChAccessory.AccessoryStockLimit.ToString());
						this.guiData.select.AccessoryGrow_Top.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
					});
				}
				else
				{
					this.guiData.select.baseObj.SetActive(true);
					this.guiData.select.Num_Own.ReplaceTextByDefault("Param01", DataManager.DmChAccessory.GetUserAccessoryList().Count.ToString() + "/" + DataManager.DmChAccessory.AccessoryStockLimit.ToString());
					this.guiData.select.AccessoryGrow_Top.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
				}
				this.RefreshSelect();
			}
			else if (this.requestMode == SelAccessoryGrowCtrl.Mode.GROW)
			{
				if (this.CurrentMode == SelAccessoryGrowCtrl.Mode.SELECT)
				{
					this.guiData.select.AccessoryGrow_Top.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
					{
						this.guiData.select.baseObj.SetActive(false);
						this.guiData.grow.baseObj.SetActive(true);
						this.guiData.grow.AccessoryGrow_Main.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
					});
				}
				else
				{
					this.guiData.grow.baseObj.SetActive(true);
					this.guiData.grow.AccessoryGrow_Main.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
				}
				this.RefreshGrow();
			}
			this.CurrentMode = this.requestMode;
		}
		if (this.serverRequestGrow != null && !this.serverRequestGrow.MoveNext())
		{
			this.serverRequestGrow = null;
		}
	}

	// Token: 0x06000F71 RID: 3953 RVA: 0x000B98AA File Offset: 0x000B7AAA
	private IEnumerator ServerRequestGrow()
	{
		if (this.BaseAccessory.GrowSimulate(this.FeedAccessories).Level > this.BaseAccessory.Level)
		{
			CanvasManager.SetEnableCmnTouchMask(true);
			AEImage ae = this.guiData.grow.AEImage_LvUp;
			ae.gameObject.SetActive(true);
			ae.playTime = 0f;
			ae.autoPlay = true;
			SoundManager.Play("prd_se_accessory_levelup", false, false);
			while (!ae.end)
			{
				yield return null;
			}
			DataManager.DmChAccessory.RequestActionGrow(this.BaseAccessory, this.FeedAccessories);
			while (DataManager.IsServerRequesting())
			{
				yield return null;
			}
			this.RefreshGrow();
			if (this.BaseAccessory.Level >= this.BaseAccessory.AccessoryData.Rarity.LevelLimit)
			{
				this.requestMode = SelAccessoryGrowCtrl.Mode.SELECT;
			}
			CanvasManager.SetEnableCmnTouchMask(false);
			ae = null;
		}
		yield break;
	}

	// Token: 0x06000F72 RID: 3954 RVA: 0x000B98B9 File Offset: 0x000B7AB9
	private void ClearFeedAccessories()
	{
		this.FeedAccessories.Clear();
	}

	// Token: 0x06000F73 RID: 3955 RVA: 0x000B98C6 File Offset: 0x000B7AC6
	private bool ReturnAccessorySelect()
	{
		if (this.setupParam.onReturnSceneNameCB() == SceneManager.SceneName.None && this.CurrentMode == SelAccessoryGrowCtrl.Mode.GROW)
		{
			this.requestMode = SelAccessoryGrowCtrl.Mode.SELECT;
			this.Reset();
			return true;
		}
		return false;
	}

	// Token: 0x06000F74 RID: 3956 RVA: 0x000B98F4 File Offset: 0x000B7AF4
	public void OnClickMenuReturn(UnityAction callback)
	{
		if (this.ReturnAccessorySelect())
		{
			return;
		}
		this.guiData.select.AccessoryGrow_Top.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
		{
			this.Reset();
			UnityAction callback2 = callback;
			if (callback2 == null)
			{
				return;
			}
			callback2();
		});
	}

	// Token: 0x04000DC0 RID: 3520
	private SelAccessoryGrowCtrl.Mode requestMode;

	// Token: 0x04000DC2 RID: 3522
	private SelAccessoryGrowCtrl.GUI guiData;

	// Token: 0x04000DC7 RID: 3527
	private List<DataManagerCharaAccessory.Accessory> FeedAccessories;

	// Token: 0x04000DC8 RID: 3528
	private bool simulateGrowLvMax;

	// Token: 0x04000DC9 RID: 3529
	private SelAccessoryGrowCtrl.SetupParam setupParam = new SelAccessoryGrowCtrl.SetupParam();

	// Token: 0x04000DCA RID: 3530
	private IEnumerator serverRequestGrow;

	// Token: 0x0200097B RID: 2427
	public enum Type
	{
		// Token: 0x04003D3E RID: 15678
		INVALID,
		// Token: 0x04003D3F RID: 15679
		SELECT,
		// Token: 0x04003D40 RID: 15680
		GROW
	}

	// Token: 0x0200097C RID: 2428
	public enum Mode
	{
		// Token: 0x04003D42 RID: 15682
		INVALID,
		// Token: 0x04003D43 RID: 15683
		SELECT,
		// Token: 0x04003D44 RID: 15684
		GROW
	}

	// Token: 0x0200097D RID: 2429
	public class Select
	{
		// Token: 0x06003C06 RID: 15366 RVA: 0x001D95A0 File Offset: 0x001D77A0
		public Select(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_Sale = baseTr.Find("All/WindowAll/SortFilterBtnsAll/Btn_Sale").GetComponent<PguiButtonCtrl>();
			this.Btn_FilterOnOff = baseTr.Find("All/WindowAll/SortFilterBtnsAll/Btn_FilterOnOff").GetComponent<PguiButtonCtrl>();
			this.Btn_SizeChange = new AccessoryUtil.SizeChangeBtnGUI(baseTr.Find("All/WindowAll/SortFilterBtnsAll/Btn_SizeChange"));
			this.Btn_Sort = baseTr.Find("All/WindowAll/SortFilterBtnsAll/Btn_Sort").GetComponent<PguiButtonCtrl>();
			this.Btn_SortUpDown = baseTr.Find("All/WindowAll/SortFilterBtnsAll/Btn_SortUpDown").GetComponent<PguiButtonCtrl>();
			this.SortFilterBtnsAll = baseTr.Find("All/WindowAll/SortFilterBtnsAll").GetComponent<PguiImageCtrl>();
			this.Num_Own = baseTr.Find("All/WindowAll/SortFilterBtnsAll/Num_Own").GetComponent<PguiTextCtrl>();
			this.AccessoryGrow_Top = baseTr.GetComponent<SimpleAnimation>();
			this.ScrollView = baseTr.Find("All/WindowAll/ScrollView").GetComponent<ReuseScroll>();
			this.Txt_None_Nofilter = baseTr.Find("All/WindowAll/Txt_None_Nofilter").gameObject;
			this.Txt_None_Nofilter.SetActive(false);
			this.Txt_None_Noitem = baseTr.Find("All/WindowAll/Txt_None_Noitem").gameObject;
			this.Txt_None_Noitem.SetActive(false);
		}

		// Token: 0x06003C07 RID: 15367 RVA: 0x001D96BF File Offset: 0x001D78BF
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

		// Token: 0x04003D45 RID: 15685
		public static readonly int SCROLL_ITEM_NUN_H = 6;

		// Token: 0x04003D46 RID: 15686
		public GameObject baseObj;

		// Token: 0x04003D47 RID: 15687
		public PguiButtonCtrl Btn_Sale;

		// Token: 0x04003D48 RID: 15688
		public PguiButtonCtrl Btn_FilterOnOff;

		// Token: 0x04003D49 RID: 15689
		public AccessoryUtil.SizeChangeBtnGUI Btn_SizeChange;

		// Token: 0x04003D4A RID: 15690
		public PguiButtonCtrl Btn_Sort;

		// Token: 0x04003D4B RID: 15691
		public PguiButtonCtrl Btn_SortUpDown;

		// Token: 0x04003D4C RID: 15692
		public PguiImageCtrl SortFilterBtnsAll;

		// Token: 0x04003D4D RID: 15693
		public PguiTextCtrl Num_Own;

		// Token: 0x04003D4E RID: 15694
		public SimpleAnimation AccessoryGrow_Top;

		// Token: 0x04003D4F RID: 15695
		public ReuseScroll ScrollView;

		// Token: 0x04003D50 RID: 15696
		public GameObject Txt_None_Nofilter;

		// Token: 0x04003D51 RID: 15697
		public GameObject Txt_None_Noitem;
	}

	// Token: 0x0200097E RID: 2430
	public class Grow
	{
		// Token: 0x06003C09 RID: 15369 RVA: 0x001D9704 File Offset: 0x001D7904
		public Grow(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_Sort = baseTr.Find("All/AccessoryAll/SortFilterBtnsAll/Btn_Sort").GetComponent<PguiButtonCtrl>();
			this.Btn_SizeChange = new AccessoryUtil.SizeChangeBtnGUI(baseTr.Find("All/AccessoryAll/SortFilterBtnsAll/Btn_SizeChange"));
			this.Btn_SortUpDown = baseTr.Find("All/AccessoryAll/SortFilterBtnsAll/Btn_SortUpDown").GetComponent<PguiButtonCtrl>();
			this.ButtonL = baseTr.Find("All/UseInfo/ButtonL").GetComponent<PguiButtonCtrl>();
			this.ButtonR = baseTr.Find("All/UseInfo/ButtonR").GetComponent<PguiButtonCtrl>();
			this.AccessoryName = baseTr.Find("All/BaseAccessory/StatusInfo/AccessoryName").GetComponent<PguiTextCtrl>();
			this.Txt02 = baseTr.Find("All/BaseAccessory/StatusInfo/VeiwTipe/Txt02").GetComponent<PguiTextCtrl>();
			this.Num_Lv_Before = baseTr.Find("All/BaseAccessory/StatusInfo/Num_Lv_Before").GetComponent<PguiTextCtrl>();
			this.Num_Lv_After = baseTr.Find("All/BaseAccessory/StatusInfo/Num_Lv_After").GetComponent<PguiTextCtrl>();
			this.AEImage_LvUp = baseTr.Find("All/BaseAccessory/StatusInfo/AEImage_LvUp").GetComponent<AEImage>();
			this.AEImage_LvUp.gameObject.SetActive(false);
			this.Num_SelPhoto = baseTr.Find("All/AccessoryAll/Num_SelPhoto").GetComponent<PguiTextCtrl>();
			this.AccessoryGrow_Main = baseTr.GetComponent<SimpleAnimation>();
			this.ScrollView = baseTr.Find("All/AccessoryAll/ScrollView").GetComponent<ReuseScroll>();
			this.Icon_Accessory = baseTr.Find("All/BaseAccessory/StatusInfo/Icon_Accessory").GetComponent<IconAccessoryCtrl>();
			this.Params = new List<SelAccessoryGrowCtrl.Grow.Param>
			{
				new SelAccessoryGrowCtrl.Grow.Param(baseTr.Find("All/BaseAccessory/StatusInfo/Param01")),
				new SelAccessoryGrowCtrl.Grow.Param(baseTr.Find("All/BaseAccessory/StatusInfo/Param02")),
				new SelAccessoryGrowCtrl.Grow.Param(baseTr.Find("All/BaseAccessory/StatusInfo/Param03"))
			};
			this.Txt_None_Nofilter = baseTr.Find("All/AccessoryAll/Txt_None_Nofilter").gameObject;
			this.Txt_None_Nofilter.SetActive(false);
		}

		// Token: 0x06003C0A RID: 15370 RVA: 0x001D98D0 File Offset: 0x001D7AD0
		public void SetupParams(DataManagerCharaAccessory.Accessory accessory)
		{
			foreach (SelAccessoryGrowCtrl.Grow.Param param in this.Params)
			{
				param.baseObj.SetActive(false);
			}
			if (AccessoryUtil.IsInvalid(accessory))
			{
				return;
			}
			List<AccessoryUtil.ParamPackData.BaseParam> list = new List<AccessoryUtil.ParamPackData.BaseParam>();
			AccessoryUtil.ParamPackData.CreateDispList<AccessoryUtil.ParamPackData.BaseParam>(ref list, new AccessoryUtil.ParamPackData.AccessoryPackData
			{
				accessory = accessory
			});
			for (int i = 0; i < this.Params.Count; i++)
			{
				if (list.Count > i)
				{
					this.Params[i].baseObj.SetActive(true);
					this.Params[i].Setup(list[i]);
				}
			}
		}

		// Token: 0x06003C0B RID: 15371 RVA: 0x001D9998 File Offset: 0x001D7B98
		public void SetupSimulateParam(DataManagerCharaAccessory.Accessory accessory, DataManagerCharaAccessory.LevelParam levelParam)
		{
			if (AccessoryUtil.IsInvalid(accessory))
			{
				return;
			}
			List<AccessoryUtil.ParamPackData.GrowthParam> list = new List<AccessoryUtil.ParamPackData.GrowthParam>();
			AccessoryUtil.ParamPackData.CreateDispList<AccessoryUtil.ParamPackData.GrowthParam>(ref list, new AccessoryUtil.ParamPackData.AccessoryPackData
			{
				accessory = accessory,
				levelParam = levelParam
			});
			for (int i = 0; i < this.Params.Count; i++)
			{
				if (list.Count > i)
				{
					this.Params[i].baseObj.SetActive(true);
					this.Params[i].SetupSimulateParam(list[i]);
				}
			}
			this.Num_Lv_After.ReplaceTextByDefault(new string[] { "Param01", "Param02", "Param03" }, new string[]
			{
				levelParam.Level.ToString(),
				AccessoryUtil.GetLevelString(accessory, true),
				AccessoryUtil.GetColorString(levelParam.Level <= accessory.Param.Level)
			});
		}

		// Token: 0x06003C0C RID: 15372 RVA: 0x001D9A84 File Offset: 0x001D7C84
		public void ResizeScrollView(int count, int resize)
		{
			this.Txt_None_Nofilter.SetActive(count <= 0);
			this.ScrollView.Resize(resize, 0);
		}

		// Token: 0x04003D52 RID: 15698
		public GameObject baseObj;

		// Token: 0x04003D53 RID: 15699
		public PguiButtonCtrl Btn_Sort;

		// Token: 0x04003D54 RID: 15700
		public AccessoryUtil.SizeChangeBtnGUI Btn_SizeChange;

		// Token: 0x04003D55 RID: 15701
		public PguiButtonCtrl Btn_SortUpDown;

		// Token: 0x04003D56 RID: 15702
		public PguiButtonCtrl ButtonL;

		// Token: 0x04003D57 RID: 15703
		public PguiButtonCtrl ButtonR;

		// Token: 0x04003D58 RID: 15704
		public PguiTextCtrl AccessoryName;

		// Token: 0x04003D59 RID: 15705
		public PguiTextCtrl Txt02;

		// Token: 0x04003D5A RID: 15706
		public PguiTextCtrl Num_Lv_Before;

		// Token: 0x04003D5B RID: 15707
		public PguiTextCtrl Num_Lv_After;

		// Token: 0x04003D5C RID: 15708
		public AEImage AEImage_LvUp;

		// Token: 0x04003D5D RID: 15709
		public PguiTextCtrl Num_SelPhoto;

		// Token: 0x04003D5E RID: 15710
		public SimpleAnimation AccessoryGrow_Main;

		// Token: 0x04003D5F RID: 15711
		public ReuseScroll ScrollView;

		// Token: 0x04003D60 RID: 15712
		public IconAccessoryCtrl Icon_Accessory;

		// Token: 0x04003D61 RID: 15713
		public List<SelAccessoryGrowCtrl.Grow.Param> Params;

		// Token: 0x04003D62 RID: 15714
		public GameObject Txt_None_Nofilter;

		// Token: 0x02001153 RID: 4435
		public class Param
		{
			// Token: 0x060055A8 RID: 21928 RVA: 0x0024F620 File Offset: 0x0024D820
			public Param(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Base_Nomal = baseTr.Find("Base_Nomal").GetComponent<PguiImageCtrl>();
				this.Base_Dmage = baseTr.Find("Base_Dmage").GetComponent<PguiImageCtrl>();
				this.Base_Nomal_Txt_01 = baseTr.Find("Base_Nomal/Txt_01").GetComponent<PguiTextCtrl>();
				this.Base_Dmage_Txt_01 = baseTr.Find("Base_Dmage/Txt_01").GetComponent<PguiTextCtrl>();
				this.Txt_Damage = baseTr.Find("Base_Dmage/Txt_Damage").GetComponent<PguiTextCtrl>();
				this.Num_before = baseTr.Find("Num_before").GetComponent<PguiTextCtrl>();
				this.Num_After = baseTr.Find("Num_After").GetComponent<PguiTextCtrl>();
			}

			// Token: 0x060055A9 RID: 21929 RVA: 0x0024F6DC File Offset: 0x0024D8DC
			public void Setup(AccessoryUtil.ParamPackData.BaseParam data)
			{
				this.Base_Nomal.gameObject.SetActive(false);
				this.Base_Dmage.gameObject.SetActive(false);
				switch (data.type)
				{
				case AccessoryUtil.ParamType.Normal:
					this.Base_Nomal.gameObject.SetActive(true);
					this.Base_Nomal_Txt_01.text = data.name;
					break;
				case AccessoryUtil.ParamType.Beat:
					this.Base_Dmage.gameObject.SetActive(true);
					this.Base_Dmage_Txt_01.text = data.name;
					this.Base_Dmage.m_Image.color = this.Base_Dmage.GetComponent<PguiColorCtrl>().GetGameObjectById("Beat");
					this.Base_Dmage_Txt_01.GetComponent<PguiGradientCtrl>().SetGameObjectById("Beat");
					break;
				case AccessoryUtil.ParamType.Action:
					this.Base_Dmage.gameObject.SetActive(true);
					this.Base_Dmage_Txt_01.text = data.name;
					this.Base_Dmage.m_Image.color = this.Base_Dmage.GetComponent<PguiColorCtrl>().GetGameObjectById("Action");
					this.Base_Dmage_Txt_01.GetComponent<PguiGradientCtrl>().SetGameObjectById("Action");
					break;
				case AccessoryUtil.ParamType.Try:
					this.Base_Dmage.gameObject.SetActive(true);
					this.Base_Dmage_Txt_01.text = data.name;
					this.Base_Dmage.m_Image.color = this.Base_Dmage.GetComponent<PguiColorCtrl>().GetGameObjectById("Try");
					this.Base_Dmage_Txt_01.GetComponent<PguiGradientCtrl>().SetGameObjectById("Try");
					break;
				case AccessoryUtil.ParamType.Avoid:
					this.Base_Nomal.gameObject.SetActive(true);
					this.Base_Nomal_Txt_01.text = data.name;
					break;
				}
				this.Num_After.text = (this.Num_before.text = (AccessoryUtil.IsNeedPermillage(data.type) ? (AccessoryUtil.GetPermillageText(data.value) ?? "") : string.Format("{0}", data.value)));
			}

			// Token: 0x060055AA RID: 21930 RVA: 0x0024F8F0 File Offset: 0x0024DAF0
			public void SetupSimulateParam(AccessoryUtil.ParamPackData.GrowthParam data)
			{
				this.Num_After.text = (AccessoryUtil.IsNeedPermillage(data.type) ? (AccessoryUtil.GetPermillageText(data.value) ?? "") : string.Format("{0}", data.value));
				this.Num_After.m_Text.color = data.color;
			}

			// Token: 0x04005F22 RID: 24354
			public GameObject baseObj;

			// Token: 0x04005F23 RID: 24355
			public PguiImageCtrl Base_Nomal;

			// Token: 0x04005F24 RID: 24356
			public PguiImageCtrl Base_Dmage;

			// Token: 0x04005F25 RID: 24357
			public PguiTextCtrl Base_Nomal_Txt_01;

			// Token: 0x04005F26 RID: 24358
			public PguiTextCtrl Base_Dmage_Txt_01;

			// Token: 0x04005F27 RID: 24359
			public PguiTextCtrl Txt_Damage;

			// Token: 0x04005F28 RID: 24360
			public PguiTextCtrl Num_before;

			// Token: 0x04005F29 RID: 24361
			public PguiTextCtrl Num_After;
		}
	}

	// Token: 0x0200097F RID: 2431
	private class GUI
	{
		// Token: 0x06003C0D RID: 15373 RVA: 0x001D9AA8 File Offset: 0x001D7CA8
		public GUI(Transform baseTr)
		{
			GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneAccessory/GUI/Prefab/GUI_AccessoryGrow"), baseTr);
			this.baseObj = baseTr.gameObject;
			this.select = new SelAccessoryGrowCtrl.Select(gameObject.transform.Find("AccessoryGrow_Top"));
			this.grow = new SelAccessoryGrowCtrl.Grow(gameObject.transform.Find("AccessoryGrow_Main"));
			this.select.baseObj.SetActive(false);
			this.grow.baseObj.SetActive(false);
		}

		// Token: 0x04003D63 RID: 15715
		public SelAccessoryGrowCtrl.Select select;

		// Token: 0x04003D64 RID: 15716
		public SelAccessoryGrowCtrl.Grow grow;

		// Token: 0x04003D65 RID: 15717
		public GameObject baseObj;

		// Token: 0x04003D66 RID: 15718
		public Dictionary<GameObject, AccessoryUtil.IconAccessorySet> selectAccessoryIcon = new Dictionary<GameObject, AccessoryUtil.IconAccessorySet>();

		// Token: 0x04003D67 RID: 15719
		public Dictionary<GameObject, AccessoryUtil.IconAccessorySet> reserveAccessoryIcon = new Dictionary<GameObject, AccessoryUtil.IconAccessorySet>();
	}

	// Token: 0x02000980 RID: 2432
	public class SetupParam
	{
		// Token: 0x04003D68 RID: 15720
		public SceneCharaEdit.OnReturnSceneName onReturnSceneNameCB;
	}
}
