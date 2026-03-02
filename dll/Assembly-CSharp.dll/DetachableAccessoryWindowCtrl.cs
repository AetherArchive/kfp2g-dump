using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200019B RID: 411
public class DetachableAccessoryWindowCtrl : MonoBehaviour
{
	// Token: 0x170003D8 RID: 984
	// (get) Token: 0x06001B31 RID: 6961 RVA: 0x0015DB84 File Offset: 0x0015BD84
	// (set) Token: 0x06001B30 RID: 6960 RVA: 0x0015DB7B File Offset: 0x0015BD7B
	private DetachableAccessoryWindowCtrl.GUI GuiData { get; set; }

	// Token: 0x170003D9 RID: 985
	// (get) Token: 0x06001B32 RID: 6962 RVA: 0x0015DB8C File Offset: 0x0015BD8C
	// (set) Token: 0x06001B33 RID: 6963 RVA: 0x0015DB94 File Offset: 0x0015BD94
	private SortFilterBtnsAllCtrl SortFilterBar { get; set; }

	// Token: 0x170003DA RID: 986
	// (get) Token: 0x06001B34 RID: 6964 RVA: 0x0015DB9D File Offset: 0x0015BD9D
	// (set) Token: 0x06001B35 RID: 6965 RVA: 0x0015DBA5 File Offset: 0x0015BDA5
	private int ClickCounter { get; set; }

	// Token: 0x170003DB RID: 987
	// (get) Token: 0x06001B36 RID: 6966 RVA: 0x0015DBAE File Offset: 0x0015BDAE
	// (set) Token: 0x06001B37 RID: 6967 RVA: 0x0015DBB6 File Offset: 0x0015BDB6
	private List<DataManagerCharaAccessory.Accessory> DispAccessories { get; set; }

	// Token: 0x170003DC RID: 988
	// (get) Token: 0x06001B39 RID: 6969 RVA: 0x0015DBC8 File Offset: 0x0015BDC8
	// (set) Token: 0x06001B38 RID: 6968 RVA: 0x0015DBBF File Offset: 0x0015BDBF
	private DataManagerCharaAccessory.Accessory TempEquipAccessory { get; set; }

	// Token: 0x170003DD RID: 989
	// (get) Token: 0x06001B3B RID: 6971 RVA: 0x0015DBD9 File Offset: 0x0015BDD9
	// (set) Token: 0x06001B3A RID: 6970 RVA: 0x0015DBD0 File Offset: 0x0015BDD0
	private AccessoryUtil.IconAccessorySet TouchIcon { get; set; }

	// Token: 0x170003DE RID: 990
	// (get) Token: 0x06001B3D RID: 6973 RVA: 0x0015DBEA File Offset: 0x0015BDEA
	// (set) Token: 0x06001B3C RID: 6972 RVA: 0x0015DBE1 File Offset: 0x0015BDE1
	private bool IsCloseForceWindow { get; set; }

	// Token: 0x06001B3E RID: 6974 RVA: 0x0015DBF2 File Offset: 0x0015BDF2
	private IEnumerator IERelease()
	{
		DataManager.DmChara.RequestActoinCharaAccessoryEquip(this.charaPackData.id, this.TempEquipAccessory);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		this.SortFilterBar.RequestUpdateSortData();
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		UnityAction unityAction = this.onFixedCb;
		if (unityAction != null)
		{
			unityAction();
		}
		UnityAction unityAction2 = this.onLocalFixedCb;
		if (unityAction2 != null)
		{
			unityAction2();
		}
		if (this.IsCloseForceWindow)
		{
			this.ForceClose();
		}
		this.GuiData.ScrollView.Refresh();
		this.UpdateAccessoryInfo();
		yield break;
	}

	// Token: 0x06001B3F RID: 6975 RVA: 0x0015DC01 File Offset: 0x0015BE01
	private IEnumerator IEOpened()
	{
		while (!this.GuiData.ow.FinishedOpen())
		{
			yield return null;
		}
		AccessoryUtil.OpenTutorialWindow();
		this.GuiData.Btn_SizeChange.ResetScrollView();
		yield break;
	}

	// Token: 0x06001B40 RID: 6976 RVA: 0x0015DC10 File Offset: 0x0015BE10
	private IEnumerator IEUploadBeforeClosing()
	{
		this.SortFilterBar.RequestUpdateSortData();
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		this.ForceClose();
		yield break;
	}

	// Token: 0x06001B41 RID: 6977 RVA: 0x0015DC20 File Offset: 0x0015BE20
	private void RefreshAccessoryList()
	{
		this.DispAccessories = this.SortFilterBar.GetSortFilteredAccessoryList();
		this.DispAccessories.Insert(0, this.removeButtonAccessory);
		this.GuiData.Btn_SizeChange.ResetScrollView();
		List<DataManagerCharaAccessory.Accessory> userAccessoryList = DataManager.DmChAccessory.GetUserAccessoryList();
		this.GuiData.Txt_None_Noitem.SetActive(userAccessoryList.Count <= 0);
		this.GuiData.ResizeScrollView(this.DispAccessories.Count - 1, this.DispAccessories.Count / this.GuiData.Btn_SizeChange.IconAccessoryParamList[this.GuiData.Btn_SizeChange.SizeIndex].num + 1);
	}

	// Token: 0x06001B42 RID: 6978 RVA: 0x0015DCD7 File Offset: 0x0015BED7
	private void Refresh()
	{
		this.RefreshAccessoryList();
	}

	// Token: 0x06001B43 RID: 6979 RVA: 0x0015DCDF File Offset: 0x0015BEDF
	private void UpdateDecideButon()
	{
		this.GuiData.ow.choiceR.GetComponent<PguiButtonCtrl>().SetActEnable(this.selectAccessoryData != null, false, false);
	}

	// Token: 0x06001B44 RID: 6980 RVA: 0x0015DD08 File Offset: 0x0015BF08
	private void UpdateAccessoryInfo()
	{
		this.GuiData.Icon_Accessory.Setup(new IconAccessoryCtrl.SetupParam
		{
			acce = this.TempEquipAccessory
		});
		this.GuiData.Num_Lv.text = ((!AccessoryUtil.IsInvalid(this.TempEquipAccessory)) ? AccessoryUtil.MakeLevelString(this.TempEquipAccessory, true) : "");
		this.GuiData.Txt02.text = ((!AccessoryUtil.IsInvalid(this.TempEquipAccessory)) ? AccessoryUtil.MakeDispTypeString(this.TempEquipAccessory) : "-");
		this.GuiData.AccessoryName.text = ((!AccessoryUtil.IsInvalid(this.TempEquipAccessory)) ? this.TempEquipAccessory.AccessoryData.Name : "");
		this.GuiData.SetupParams(this.TempEquipAccessory);
		CharaStaticData charaStaticData = null;
		if (AccessoryUtil.IsDecidedOwner(this.TempEquipAccessory))
		{
			charaStaticData = DataManager.DmChara.GetCharaStaticData(this.TempEquipAccessory.CharaId);
		}
		this.GuiData.Icon_Accessory.DispIconCharaMini(charaStaticData != null, charaStaticData);
	}

	// Token: 0x06001B45 RID: 6981 RVA: 0x0015DE14 File Offset: 0x0015C014
	private void Internal()
	{
		this.onFixedCb = null;
		this.onLocalFixedCb = null;
		this.IsCloseForceWindow = true;
		this.selectAccessoryData = null;
		if (this.TouchIcon != null)
		{
			this.TouchIcon.currentFrame.SetActive(false);
		}
		this.TouchIcon = null;
		this.ieRelease = null;
		this.ieOpened = null;
		this.ieUploadBeforeClosing = null;
	}

	// Token: 0x06001B46 RID: 6982 RVA: 0x0015DE72 File Offset: 0x0015C072
	private int GetButtonSizeIndex()
	{
		return DataManager.DmGameStatus.MakeUserFlagData().GetIconSizeData(SortFilterDefine.IconPlace.AccessoryCharaEdit).SizeIndex;
	}

	// Token: 0x06001B47 RID: 6983 RVA: 0x0015DE8C File Offset: 0x0015C08C
	public void Init()
	{
		this.ClickCounter = 0;
		GameObject gameObject = Resources.Load("SceneAccessory/GUI/Prefab/GUI_Accessory_SetWindow") as GameObject;
		this.GuiData = new DetachableAccessoryWindowCtrl.GUI(Object.Instantiate<GameObject>(gameObject, base.transform).transform);
		this.SortFilterBar = new SortFilterBtnsAllCtrl(SortFilterDefine.RegisterType.ACCESSORY_EQUIP, this.GuiData.baseObj.transform.Find("Base/Window/SortFilterBtnsAll").gameObject, new UnityAction(this.RefreshAccessoryList));
		this.GuiData.ScrollView.InitForce();
		this.GuiData.DispBox(this.ClickCounter);
		this.GuiData.BtnChange.AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			int num = this.ClickCounter + 1;
			this.ClickCounter = num;
			this.GuiData.DispBox(this.ClickCounter);
			if (this.GuiData.Box02.activeSelf)
			{
				this.SetActiveToggleButton(false);
				this.GuiData.Box02_Txt02.text = ((this.TempEquipAccessory != null) ? AccessoryUtil.MakeDispTypeString(this.TempEquipAccessory) : "-");
				if (this.TempEquipAccessory != null && this.TempEquipAccessory.AccessoryData.DispType == DataManagerCharaAccessory.DispType.Battle)
				{
					this.SetActiveToggleButton(true);
				}
			}
		}, PguiButtonCtrl.SoundType.DEFAULT);
		AccessoryUtil.SizeChangeBtnGUI.DataPack[] dataPacks = AccessoryUtil.SizeChangeBtnGUI.GetDataPacks(AccessoryUtil.SizeChangeBtnGUI.DataPackType.Set);
		AccessoryUtil.SizeChangeBtnGUI btn_SizeChange = this.GuiData.Btn_SizeChange;
		AccessoryUtil.SizeChangeBtnGUI.SetupParam setupParam = new AccessoryUtil.SizeChangeBtnGUI.SetupParam();
		setupParam.funcResult = delegate(AccessoryUtil.SizeChangeBtnGUI.ResultParam result)
		{
			DataManager.DmGameStatus.RequestActionUpdateIconsideIndex(SortFilterDefine.IconPlace.AccessoryCharaEdit, result.sizeIndex);
		};
		setupParam.iconAccessoryParamList = new List<AccessoryUtil.SizeChangeBtnGUI.IconAccessoryParam>
		{
			new AccessoryUtil.SizeChangeBtnGUI.IconAccessoryParam
			{
				scale = new Vector3(0.52f, 0.52f, 1f),
				scaleCurrent = new Vector3(0.85f, 0.85f, 1f),
				scaleCount = new Vector3(0.85f, 0.85f, 1f),
				num = dataPacks[0].num,
				prefab = Object.Instantiate<GameObject>((GameObject)Resources.Load(dataPacks[0].prefabName), base.transform)
			},
			new AccessoryUtil.SizeChangeBtnGUI.IconAccessoryParam
			{
				scale = new Vector3(0.6f, 0.6f, 1f),
				scaleCurrent = new Vector3(1f, 1f, 1f),
				scaleCount = new Vector3(1f, 1f, 1f),
				num = dataPacks[1].num,
				prefab = Object.Instantiate<GameObject>((GameObject)Resources.Load(dataPacks[1].prefabName), base.transform)
			},
			new AccessoryUtil.SizeChangeBtnGUI.IconAccessoryParam
			{
				scale = new Vector3(0.7f, 0.7f, 1f),
				scaleCurrent = new Vector3(1f, 1f, 1f),
				scaleCount = new Vector3(1f, 1f, 1f),
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
		setupParam.onStartItem = new Action<int, GameObject>(this.OnStartItem);
		setupParam.onUpdateItem = new Action<int, GameObject>(this.OnUpdateItem);
		setupParam.refScrollView = this.GuiData.ScrollView;
		setupParam.sizeIndex = this.GetButtonSizeIndex();
		setupParam.resetCallback = delegate
		{
			this.GuiData.reserveAccessoryIcon.Clear();
		};
		setupParam.dispIconAccessoryCountCallback = () => this.DispAccessories.Count;
		btn_SizeChange.Setup(setupParam);
		this.GuiData.Btn_Radio01.AddOnClickListener(delegate(PguiToggleButtonCtrl pbc, int toggleIndex)
		{
			if (toggleIndex == 1)
			{
				SoundManager.Play("prd_se_click", false, false);
				return false;
			}
			this.GuiData.Btn_Radio03.SetToggleIndex(0);
			this.GuiData.Btn_Radio02.SetToggleIndex(0);
			return true;
		});
		this.GuiData.Btn_Radio02.AddOnClickListener(delegate(PguiToggleButtonCtrl pbc, int toggleIndex)
		{
			if (toggleIndex == 1)
			{
				SoundManager.Play("prd_se_click", false, false);
				return false;
			}
			this.GuiData.Btn_Radio01.SetToggleIndex(0);
			this.GuiData.Btn_Radio03.SetToggleIndex(0);
			return true;
		});
		this.GuiData.Btn_Radio03.AddOnClickListener(delegate(PguiToggleButtonCtrl pbc, int toggleIndex)
		{
			if (toggleIndex == 1)
			{
				SoundManager.Play("prd_se_click", false, false);
				return false;
			}
			this.GuiData.Btn_Radio01.SetToggleIndex(0);
			this.GuiData.Btn_Radio02.SetToggleIndex(0);
			return true;
		});
		this.GuiData.Icon_Accessory.AddOnUpdateStatus(delegate(IconAccessoryCtrl x)
		{
			this.GuiData.Icon_Accessory.DispLockIcon(!AccessoryUtil.IsInvalid(this.TempEquipAccessory) && this.TempEquipAccessory.IsLock);
		});
	}

	// Token: 0x06001B48 RID: 6984 RVA: 0x0015E28B File Offset: 0x0015C48B
	private void ResetToggleButton()
	{
		this.GuiData.Btn_Radio01.SetToggleIndex(0);
		this.GuiData.Btn_Radio02.SetToggleIndex(0);
		this.GuiData.Btn_Radio03.SetToggleIndex(0);
	}

	// Token: 0x06001B49 RID: 6985 RVA: 0x0015E2C0 File Offset: 0x0015C4C0
	private void SetActiveToggleButton(bool sw)
	{
		this.GuiData.Btn_Radio01.gameObject.SetActive(sw);
		this.GuiData.Btn_Radio02.gameObject.SetActive(sw);
		this.GuiData.Btn_Radio03.gameObject.SetActive(sw);
	}

	// Token: 0x06001B4A RID: 6986 RVA: 0x0015E310 File Offset: 0x0015C510
	private void InternalOpen()
	{
		this.GuiData.ow.Setup(null, null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, delegate(int index)
		{
			if (index == 1)
			{
				if (!AccessoryUtil.IsInvalid(this.TempEquipAccessory))
				{
					if (!AccessoryUtil.IsDecidedOwner(this.TempEquipAccessory))
					{
						CanvasManager.HdlAccessoryOwnerSettingWindowCtrl.OpenOwnerSetting(this.charaPackData, this.TempEquipAccessory, delegate
						{
							this.onLocalFixedCb = delegate
							{
								if (AccessoryUtil.IsDecidedOwner(this.TempEquipAccessory))
								{
									CanvasManager.HdlAccessoryOwnerSettingWindowCtrl.OpenOwnerSettingAfter(this.charaPackData, this.TempEquipAccessory, delegate
									{
										this.ForceClose();
									});
								}
							};
							this.ieRelease = this.IERelease();
						}, delegate
						{
						});
						this.IsCloseForceWindow = false;
					}
					else
					{
						this.ieRelease = this.IERelease();
					}
				}
				else
				{
					this.ieRelease = this.IERelease();
				}
				return false;
			}
			this.ieUploadBeforeClosing = this.IEUploadBeforeClosing();
			return false;
		}, delegate
		{
			this.GuiData.renderTextureCharaCtrl.Destroy();
		}, false);
		this.GuiData.ow.Open();
		this.isActiveWindow = true;
		this.ieOpened = this.IEOpened();
		if (this.charaPackData != null)
		{
			RenderTextureChara renderTextureChara = this.GuiData.renderTextureCharaCtrl.Create();
			renderTextureChara.fieldOfView = 30f;
			renderTextureChara.Setup(this.charaPackData, 1, CharaMotionDefine.ActKey.SCENARIO_STAND_BY, true, null, false, null, 0f, null, false);
		}
	}

	// Token: 0x06001B4B RID: 6987 RVA: 0x0015E3AE File Offset: 0x0015C5AE
	public void ReOpen()
	{
		this.InternalOpen();
	}

	// Token: 0x06001B4C RID: 6988 RVA: 0x0015E3B8 File Offset: 0x0015C5B8
	public void Open(CharaPackData cpd, UnityAction fixedCb = null)
	{
		this.charaPackData = cpd;
		this.SortFilterBar.SelectCharaId = this.charaPackData.id;
		this.Internal();
		base.gameObject.SetActive(true);
		this.Refresh();
		this.GuiData.Btn_SizeChange.ResetScrollView();
		this.GuiData.Icon_Chara.SetupPrm(new IconCharaCtrl.SetupParam
		{
			cpd = this.charaPackData
		});
		this.GuiData.Txt_CharaName.ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[]
		{
			this.charaPackData.staticData.baseData.NickName,
			this.charaPackData.staticData.GetName()
		});
		this.TempEquipAccessory = this.charaPackData.dynamicData.accessory;
		this.UpdateDecideButon();
		this.UpdateAccessoryInfo();
		this.ResetToggleButton();
		this.GuiData.Btn_Radio01.SetToggleIndex(1);
		this.InternalOpen();
		this.onFixedCb = fixedCb;
	}

	// Token: 0x06001B4D RID: 6989 RVA: 0x0015E4C8 File Offset: 0x0015C6C8
	public bool IsActiveWIndow()
	{
		return this.isActiveWindow;
	}

	// Token: 0x06001B4E RID: 6990 RVA: 0x0015E4D0 File Offset: 0x0015C6D0
	public void ForceClose()
	{
		this.GuiData.ow.ForceClose();
		this.isActiveWindow = false;
	}

	// Token: 0x06001B4F RID: 6991 RVA: 0x0015E4EC File Offset: 0x0015C6EC
	private void Update()
	{
		if (this.ieRelease != null && !this.ieRelease.MoveNext())
		{
			this.ieRelease = null;
		}
		if (this.ieOpened != null && !this.ieOpened.MoveNext())
		{
			this.ieOpened = null;
		}
		if (this.ieUploadBeforeClosing != null && !this.ieUploadBeforeClosing.MoveNext())
		{
			this.ieUploadBeforeClosing = null;
		}
	}

	// Token: 0x06001B50 RID: 6992 RVA: 0x0015E550 File Offset: 0x0015C750
	private void OnStartItem(int index, GameObject go)
	{
		for (int i = 0; i < this.GuiData.Btn_SizeChange.IconAccessoryParamList[this.GuiData.Btn_SizeChange.SizeIndex].num; i++)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_AccessorySet, go.transform);
			gameObject.name = i.ToString();
			AccessoryUtil.IconAccessorySet et = new AccessoryUtil.IconAccessorySet(gameObject.transform);
			et.iconAccessoryCtrl.AddOnClickListener(delegate(IconAccessoryCtrl x)
			{
				this.OnTouchIcon(et);
			});
			et.iconAccessoryCtrl.AddOnUpdateStatus(delegate(IconAccessoryCtrl x)
			{
				this.GuiData.ScrollView.Refresh();
			});
			et.baseObj.transform.Find("Icon_Accessory").localScale = this.GuiData.Btn_SizeChange.IconAccessoryParamList[this.GuiData.Btn_SizeChange.SizeIndex].scale;
			et.SetScale(this.GuiData.Btn_SizeChange.IconAccessoryParamList[this.GuiData.Btn_SizeChange.SizeIndex].scaleCurrent, this.GuiData.Btn_SizeChange.IconAccessoryParamList[this.GuiData.Btn_SizeChange.SizeIndex].scaleCount);
			PrjUtil.AddTouchEventTrigger(et.iconBase, delegate(Transform x)
			{
				this.OnTouchIcon(et);
			});
			this.GuiData.reserveAccessoryIcon.Add(et);
		}
	}

	// Token: 0x06001B51 RID: 6993 RVA: 0x0015E6EC File Offset: 0x0015C8EC
	private void OnUpdateItem(int index, GameObject go)
	{
		CharaPackData charaPackData = this.charaPackData;
		if (charaPackData == null)
		{
			charaPackData = CharaPackData.MakeInvalid();
		}
		CharaStaticData staticData = charaPackData.staticData;
		for (int i = 0; i < this.GuiData.Btn_SizeChange.IconAccessoryParamList[this.GuiData.Btn_SizeChange.SizeIndex].num; i++)
		{
			GameObject iconObj = go.transform.Find(i.ToString()).gameObject;
			AccessoryUtil.IconAccessorySet iconAccessorySet = this.GuiData.reserveAccessoryIcon.Find((AccessoryUtil.IconAccessorySet item) => item.baseObj == iconObj);
			int num = index * this.GuiData.Btn_SizeChange.IconAccessoryParamList[this.GuiData.Btn_SizeChange.SizeIndex].num + i;
			if (this.DispAccessories.Count > num)
			{
				iconAccessorySet.baseObj.SetActive(true);
				DataManagerCharaAccessory.Accessory accessory = this.DispAccessories[num];
				iconAccessorySet.iconAccessoryCtrl.Setup(new IconAccessoryCtrl.SetupParam
				{
					acce = accessory,
					sortType = this.SortFilterBar.SortType
				});
				iconAccessorySet.iconAccessoryCtrl.onReturnAccessoryList = () => this.DispAccessories;
				iconAccessorySet.iconAccessoryCtrl.DispRemove(AccessoryUtil.IsInvalid(accessory));
				iconAccessorySet.currentFrame.SetActive(this.selectAccessoryData != null && this.selectAccessoryData.accessory == accessory);
				iconAccessorySet.iconAccessoryCtrl.DispImgDisable(false);
				CharaStaticData charaStaticData = null;
				if (AccessoryUtil.IsDecidedOwner(accessory))
				{
					charaStaticData = DataManager.DmChara.GetCharaStaticData(accessory.CharaId);
				}
				iconAccessorySet.iconAccessoryCtrl.DispIconCharaMini(charaStaticData != null, charaStaticData);
				if (AccessoryUtil.IsInvalid(accessory))
				{
					iconAccessorySet.iconAccessoryCtrl.DispParty(false, true);
				}
				else
				{
					iconAccessorySet.iconAccessoryCtrl.DispParty(!AccessoryUtil.IsInvalid(charaPackData.dynamicData.accessory) && charaPackData.dynamicData.accessory.UniqId == accessory.UniqId, true);
				}
				if (!AccessoryUtil.IsInvalid(accessory) && this.selectAccessoryData != null)
				{
				}
			}
			else
			{
				iconAccessorySet.baseObj.SetActive(false);
			}
		}
		go.GetComponent<GridLayoutGroup>().enabled = false;
		go.GetComponent<GridLayoutGroup>().enabled = true;
	}

	// Token: 0x06001B52 RID: 6994 RVA: 0x0015E928 File Offset: 0x0015CB28
	private void OnTouchIcon(AccessoryUtil.IconAccessorySet iconAccessorySet)
	{
		SoundManager.Play("prd_se_click", false, false);
		AccessoryUtil.IconAccessorySet iconAccessorySet2 = this.GuiData.SearchIconAccessory(new DetachableAccessoryWindowCtrl.SelectAccessoryData(iconAccessorySet.iconAccessoryCtrl.accessory));
		if (this.selectAccessoryData == null)
		{
			if (iconAccessorySet2 != null)
			{
				iconAccessorySet2.currentFrame.SetActive(true);
			}
		}
		else
		{
			AccessoryUtil.IconAccessorySet iconAccessorySet3 = this.GuiData.SearchIconAccessory(new DetachableAccessoryWindowCtrl.SelectAccessoryData(this.selectAccessoryData.accessory));
			if (iconAccessorySet3 != null)
			{
				iconAccessorySet3.currentFrame.SetActive(false);
				if (iconAccessorySet3 != iconAccessorySet2)
				{
					if (iconAccessorySet2 != null)
					{
						iconAccessorySet2.currentFrame.SetActive(true);
					}
				}
				else
				{
					iconAccessorySet2 = null;
				}
			}
		}
		if (iconAccessorySet2 != null)
		{
			this.selectAccessoryData = new DetachableAccessoryWindowCtrl.SelectAccessoryData(iconAccessorySet2.iconAccessoryCtrl.accessory);
			this.TempEquipAccessory = this.selectAccessoryData.accessory;
		}
		else
		{
			this.selectAccessoryData = null;
			this.TempEquipAccessory = null;
		}
		this.UpdateDecideButon();
		this.UpdateAccessoryInfo();
		this.GuiData.ScrollView.Refresh();
	}

	// Token: 0x04001489 RID: 5257
	private UnityAction onFixedCb;

	// Token: 0x0400148A RID: 5258
	private UnityAction onLocalFixedCb;

	// Token: 0x0400148D RID: 5261
	private readonly DataManagerCharaAccessory.Accessory removeButtonAccessory = AccessoryUtil.MakeDummy();

	// Token: 0x0400148E RID: 5262
	private CharaPackData charaPackData;

	// Token: 0x0400148F RID: 5263
	private DetachableAccessoryWindowCtrl.SelectAccessoryData selectAccessoryData;

	// Token: 0x04001493 RID: 5267
	private IEnumerator ieRelease;

	// Token: 0x04001494 RID: 5268
	private IEnumerator ieOpened;

	// Token: 0x04001495 RID: 5269
	private IEnumerator ieUploadBeforeClosing;

	// Token: 0x04001496 RID: 5270
	private bool isActiveWindow;

	// Token: 0x02000EB1 RID: 3761
	public class SelectAccessoryData
	{
		// Token: 0x06004D66 RID: 19814 RVA: 0x0023297A File Offset: 0x00230B7A
		public SelectAccessoryData(DataManagerCharaAccessory.Accessory c)
		{
			this.accessory = c;
		}

		// Token: 0x04005444 RID: 21572
		public DataManagerCharaAccessory.Accessory accessory;
	}

	// Token: 0x02000EB2 RID: 3762
	public class GUI
	{
		// Token: 0x06004D67 RID: 19815 RVA: 0x0023298C File Offset: 0x00230B8C
		public AccessoryUtil.IconAccessorySet SearchIconAccessory(DetachableAccessoryWindowCtrl.SelectAccessoryData sad)
		{
			if (sad != null)
			{
				return this.reserveAccessoryIcon.Find((AccessoryUtil.IconAccessorySet item) => item.iconAccessoryCtrl.accessory == sad.accessory);
			}
			return null;
		}

		// Token: 0x06004D68 RID: 19816 RVA: 0x002329C8 File Offset: 0x00230BC8
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.BtnChange = baseTr.Find("Base/Window/BtnChange").GetComponent<PguiButtonCtrl>();
			this.BtnChange.gameObject.SetActive(false);
			this.Box01 = baseTr.Find("Base/Window/Left/Box01").gameObject;
			this.Box02 = baseTr.Find("Base/Window/Left/Box02").gameObject;
			this.Texture_Render = baseTr.Find("Base/Window/Left/Box02/Mask/Texture_Render").GetComponent<PguiRawImageCtrl>();
			this.Txt_CharaName = baseTr.Find("Base/Window/Left/Box01/Img_Line/Txt_CharaName").GetComponent<PguiTextCtrl>();
			this.AccessoryName = baseTr.Find("Base/Window/Left/Box01/AccessoryName").GetComponent<PguiTextCtrl>();
			this.Txt02 = baseTr.Find("Base/Window/Left/Box01/VeiwTipe/Txt02").GetComponent<PguiTextCtrl>();
			this.Num_Lv = baseTr.Find("Base/Window/Left/Box01/Num_Lv").GetComponent<PguiTextCtrl>();
			this.Box02_Txt02 = baseTr.Find("Base/Window/Left/Box02/VeiwTipe/Txt02").GetComponent<PguiTextCtrl>();
			this.ScrollView = baseTr.Find("Base/Window/Right/ScrollView").GetComponent<ReuseScroll>();
			this.Btn_Radio01 = baseTr.Find("Base/Window/Left/Box02/Btn_Radio01").GetComponent<PguiToggleButtonCtrl>();
			this.Btn_Radio02 = baseTr.Find("Base/Window/Left/Box02/Btn_Radio02").GetComponent<PguiToggleButtonCtrl>();
			this.Btn_Radio03 = baseTr.Find("Base/Window/Left/Box02/Btn_Radio03").GetComponent<PguiToggleButtonCtrl>();
			this.ow = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.Btn_SizeChange = new AccessoryUtil.SizeChangeBtnGUI(baseTr.Find("Base/Window/SortFilterBtnsAll/Btn_SizeChange"));
			this.Icon_Chara = baseTr.Find("Base/Window/Left/Box01/Img_Line/Icon_Chara").GetComponent<IconCharaCtrl>();
			this.Icon_Accessory = baseTr.Find("Base/Window/Left/Box01/Icon_Accessory").GetComponent<IconAccessoryCtrl>();
			this.Params = new List<DetachableAccessoryWindowCtrl.GUI.Param>
			{
				new DetachableAccessoryWindowCtrl.GUI.Param(this.Box01.transform.Find("Param01")),
				new DetachableAccessoryWindowCtrl.GUI.Param(this.Box01.transform.Find("Param02")),
				new DetachableAccessoryWindowCtrl.GUI.Param(this.Box01.transform.Find("Param03"))
			};
			this.renderTextureCharaCtrl = this.Box02.transform.Find("Mask/Texture_Render").GetComponent<PguiRenderTextureCharaCtrl>();
			this.Txt_None_Nofilter = baseTr.Find("Base/Window/Right/Txt_None_Nofilter").gameObject;
			this.Txt_None_Nofilter.SetActive(false);
			this.Txt_None_Noitem = baseTr.Find("Base/Window/Right/Txt_None_Noitem").gameObject;
			this.Txt_None_Noitem.SetActive(false);
		}

		// Token: 0x06004D69 RID: 19817 RVA: 0x00232C3E File Offset: 0x00230E3E
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

		// Token: 0x06004D6A RID: 19818 RVA: 0x00232C7C File Offset: 0x00230E7C
		public void DispBox(int count)
		{
			bool flag = count % 2 == 0;
			this.Box01.SetActive(flag);
			this.Box02.SetActive(!flag);
		}

		// Token: 0x06004D6B RID: 19819 RVA: 0x00232CAC File Offset: 0x00230EAC
		public void SetupParams(DataManagerCharaAccessory.Accessory accessory)
		{
			foreach (DetachableAccessoryWindowCtrl.GUI.Param param in this.Params)
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

		// Token: 0x04005445 RID: 21573
		public GameObject baseObj;

		// Token: 0x04005446 RID: 21574
		public PguiButtonCtrl BtnChange;

		// Token: 0x04005447 RID: 21575
		public PguiRawImageCtrl Texture_Render;

		// Token: 0x04005448 RID: 21576
		public PguiTextCtrl Txt_Lv;

		// Token: 0x04005449 RID: 21577
		public PguiTextCtrl Num;

		// Token: 0x0400544A RID: 21578
		public PguiTextCtrl Txt_CharaName;

		// Token: 0x0400544B RID: 21579
		public PguiTextCtrl AccessoryName;

		// Token: 0x0400544C RID: 21580
		public PguiTextCtrl Txt02;

		// Token: 0x0400544D RID: 21581
		public PguiTextCtrl Box02_Txt02;

		// Token: 0x0400544E RID: 21582
		public PguiTextCtrl Num_Lv;

		// Token: 0x0400544F RID: 21583
		public ReuseScroll ScrollView;

		// Token: 0x04005450 RID: 21584
		public PguiToggleButtonCtrl Btn_Radio01;

		// Token: 0x04005451 RID: 21585
		public PguiToggleButtonCtrl Btn_Radio02;

		// Token: 0x04005452 RID: 21586
		public PguiToggleButtonCtrl Btn_Radio03;

		// Token: 0x04005453 RID: 21587
		public PguiOpenWindowCtrl ow;

		// Token: 0x04005454 RID: 21588
		public GameObject Box01;

		// Token: 0x04005455 RID: 21589
		public GameObject Box02;

		// Token: 0x04005456 RID: 21590
		public AccessoryUtil.SizeChangeBtnGUI Btn_SizeChange;

		// Token: 0x04005457 RID: 21591
		public List<AccessoryUtil.IconAccessorySet> reserveAccessoryIcon = new List<AccessoryUtil.IconAccessorySet>();

		// Token: 0x04005458 RID: 21592
		public IconCharaCtrl Icon_Chara;

		// Token: 0x04005459 RID: 21593
		public IconAccessoryCtrl Icon_Accessory;

		// Token: 0x0400545A RID: 21594
		public List<DetachableAccessoryWindowCtrl.GUI.Param> Params;

		// Token: 0x0400545B RID: 21595
		public PguiRenderTextureCharaCtrl renderTextureCharaCtrl;

		// Token: 0x0400545C RID: 21596
		public GameObject Txt_None_Nofilter;

		// Token: 0x0400545D RID: 21597
		public GameObject Txt_None_Noitem;

		// Token: 0x020011F1 RID: 4593
		public class Param
		{
			// Token: 0x06005762 RID: 22370 RVA: 0x0025721C File Offset: 0x0025541C
			public Param(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.Base_Nomal = baseTr.Find("Base_Nomal").GetComponent<PguiImageCtrl>();
				this.Base_Dmage = baseTr.Find("Base_Dmage").GetComponent<PguiImageCtrl>();
				this.Base_Nomal_Txt_01 = baseTr.Find("Base_Nomal/Txt_01").GetComponent<PguiTextCtrl>();
				this.Base_Dmage_Txt_01 = baseTr.Find("Base_Dmage/Txt_01").GetComponent<PguiTextCtrl>();
				this.Txt_Damage = baseTr.Find("Base_Dmage/Txt_Damage").GetComponent<PguiTextCtrl>();
				this.Num_01 = baseTr.Find("Num_01").GetComponent<PguiTextCtrl>();
			}

			// Token: 0x06005763 RID: 22371 RVA: 0x002572C0 File Offset: 0x002554C0
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
				this.Num_01.text = (AccessoryUtil.IsNeedPermillage(data.type) ? ("+" + AccessoryUtil.GetPermillageText(data.value)) : string.Format("+{0}", data.value));
			}

			// Token: 0x04006265 RID: 25189
			public GameObject baseObj;

			// Token: 0x04006266 RID: 25190
			public PguiImageCtrl Base_Nomal;

			// Token: 0x04006267 RID: 25191
			public PguiImageCtrl Base_Dmage;

			// Token: 0x04006268 RID: 25192
			public PguiTextCtrl Base_Nomal_Txt_01;

			// Token: 0x04006269 RID: 25193
			public PguiTextCtrl Base_Dmage_Txt_01;

			// Token: 0x0400626A RID: 25194
			public PguiTextCtrl Txt_Damage;

			// Token: 0x0400626B RID: 25195
			public PguiTextCtrl Num_01;
		}
	}
}
