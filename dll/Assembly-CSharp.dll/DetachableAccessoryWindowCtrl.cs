using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DetachableAccessoryWindowCtrl : MonoBehaviour
{
	private DetachableAccessoryWindowCtrl.GUI GuiData { get; set; }

	private SortFilterBtnsAllCtrl SortFilterBar { get; set; }

	private int ClickCounter { get; set; }

	private List<DataManagerCharaAccessory.Accessory> DispAccessories { get; set; }

	private DataManagerCharaAccessory.Accessory TempEquipAccessory { get; set; }

	private AccessoryUtil.IconAccessorySet TouchIcon { get; set; }

	private bool IsCloseForceWindow { get; set; }

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

	private void RefreshAccessoryList()
	{
		this.DispAccessories = this.SortFilterBar.GetSortFilteredAccessoryList();
		this.DispAccessories.Insert(0, this.removeButtonAccessory);
		this.GuiData.Btn_SizeChange.ResetScrollView();
		List<DataManagerCharaAccessory.Accessory> userAccessoryList = DataManager.DmChAccessory.GetUserAccessoryList();
		this.GuiData.Txt_None_Noitem.SetActive(userAccessoryList.Count <= 0);
		this.GuiData.ResizeScrollView(this.DispAccessories.Count - 1, this.DispAccessories.Count / this.GuiData.Btn_SizeChange.IconAccessoryParamList[this.GuiData.Btn_SizeChange.SizeIndex].num + 1);
	}

	private void Refresh()
	{
		this.RefreshAccessoryList();
	}

	private void UpdateDecideButon()
	{
		this.GuiData.ow.choiceR.GetComponent<PguiButtonCtrl>().SetActEnable(this.selectAccessoryData != null, false, false);
	}

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

	private int GetButtonSizeIndex()
	{
		return DataManager.DmGameStatus.MakeUserFlagData().GetIconSizeData(SortFilterDefine.IconPlace.AccessoryCharaEdit).SizeIndex;
	}

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

	private void ResetToggleButton()
	{
		this.GuiData.Btn_Radio01.SetToggleIndex(0);
		this.GuiData.Btn_Radio02.SetToggleIndex(0);
		this.GuiData.Btn_Radio03.SetToggleIndex(0);
	}

	private void SetActiveToggleButton(bool sw)
	{
		this.GuiData.Btn_Radio01.gameObject.SetActive(sw);
		this.GuiData.Btn_Radio02.gameObject.SetActive(sw);
		this.GuiData.Btn_Radio03.gameObject.SetActive(sw);
	}

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

	public void ReOpen()
	{
		this.InternalOpen();
	}

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

	public bool IsActiveWIndow()
	{
		return this.isActiveWindow;
	}

	public void ForceClose()
	{
		this.GuiData.ow.ForceClose();
		this.isActiveWindow = false;
	}

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

	private UnityAction onFixedCb;

	private UnityAction onLocalFixedCb;

	private readonly DataManagerCharaAccessory.Accessory removeButtonAccessory = AccessoryUtil.MakeDummy();

	private CharaPackData charaPackData;

	private DetachableAccessoryWindowCtrl.SelectAccessoryData selectAccessoryData;

	private IEnumerator ieRelease;

	private IEnumerator ieOpened;

	private IEnumerator ieUploadBeforeClosing;

	private bool isActiveWindow;

	public class SelectAccessoryData
	{
		public SelectAccessoryData(DataManagerCharaAccessory.Accessory c)
		{
			this.accessory = c;
		}

		public DataManagerCharaAccessory.Accessory accessory;
	}

	public class GUI
	{
		public AccessoryUtil.IconAccessorySet SearchIconAccessory(DetachableAccessoryWindowCtrl.SelectAccessoryData sad)
		{
			if (sad != null)
			{
				return this.reserveAccessoryIcon.Find((AccessoryUtil.IconAccessorySet item) => item.iconAccessoryCtrl.accessory == sad.accessory);
			}
			return null;
		}

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

		public void DispBox(int count)
		{
			bool flag = count % 2 == 0;
			this.Box01.SetActive(flag);
			this.Box02.SetActive(!flag);
		}

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

		public GameObject baseObj;

		public PguiButtonCtrl BtnChange;

		public PguiRawImageCtrl Texture_Render;

		public PguiTextCtrl Txt_Lv;

		public PguiTextCtrl Num;

		public PguiTextCtrl Txt_CharaName;

		public PguiTextCtrl AccessoryName;

		public PguiTextCtrl Txt02;

		public PguiTextCtrl Box02_Txt02;

		public PguiTextCtrl Num_Lv;

		public ReuseScroll ScrollView;

		public PguiToggleButtonCtrl Btn_Radio01;

		public PguiToggleButtonCtrl Btn_Radio02;

		public PguiToggleButtonCtrl Btn_Radio03;

		public PguiOpenWindowCtrl ow;

		public GameObject Box01;

		public GameObject Box02;

		public AccessoryUtil.SizeChangeBtnGUI Btn_SizeChange;

		public List<AccessoryUtil.IconAccessorySet> reserveAccessoryIcon = new List<AccessoryUtil.IconAccessorySet>();

		public IconCharaCtrl Icon_Chara;

		public IconAccessoryCtrl Icon_Accessory;

		public List<DetachableAccessoryWindowCtrl.GUI.Param> Params;

		public PguiRenderTextureCharaCtrl renderTextureCharaCtrl;

		public GameObject Txt_None_Nofilter;

		public GameObject Txt_None_Noitem;

		public class Param
		{
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

			public GameObject baseObj;

			public PguiImageCtrl Base_Nomal;

			public PguiImageCtrl Base_Dmage;

			public PguiTextCtrl Base_Nomal_Txt_01;

			public PguiTextCtrl Base_Dmage_Txt_01;

			public PguiTextCtrl Txt_Damage;

			public PguiTextCtrl Num_01;
		}
	}
}
