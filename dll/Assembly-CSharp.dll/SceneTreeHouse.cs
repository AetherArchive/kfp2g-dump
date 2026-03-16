using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using AEAuth3;
using CriWare;
using SGNFW.Common;
using SGNFW.HttpRequest.Protocol;
using SGNFW.Mst;
using SGNFW.Touch;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SceneTreeHouse : BaseScene
{
	private bool IsNight
	{
		get
		{
			return this.stageType > SceneHome.StageType.EVENING;
		}
	}

	private string stampIcon(int id)
	{
		return "Texture2D/Icon_Stamp/icon_stamp_" + id.ToString("D2");
	}

	private float ClampAngle(float f)
	{
		while (f < 0f)
		{
			f += 360f;
		}
		while (f >= 360f)
		{
			f -= 360f;
		}
		return f;
	}

	private float Round30(float f)
	{
		return Mathf.Round(this.ClampAngle(f) / 30f) * 30f;
	}

	private float Round90(float f)
	{
		return Mathf.Round(this.ClampAngle(f) / 90f) * 90f;
	}

	public override void OnCreateScene()
	{
		this.guiData = new SceneTreeHouse.GUI(AssetManager.InstantiateAssetData("SceneTreeHouse/GUI/Prefab/GUI_TreeHouse", null).transform);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.FRONT, this.guiData.baseObj.transform, true);
		this.guiOther = new SceneTreeHouse.GUI_OTHER(AssetManager.InstantiateAssetData("SceneTreeHouse/GUI/Prefab/GUI_TreeHouse_OtherUser", null).transform);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.FRONT, this.guiOther.baseObj.transform, true);
		this.winPanel = AssetManager.InstantiateAssetData("SceneTreeHouse/GUI/Prefab/GUI_TreeHouse_Window", null);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this.winPanel.transform, true);
		this.winMyset = new SceneTreeHouse.WIN_MYSET(this.winPanel.transform.Find("Window_MySet"));
		this.winGetstamp = new SceneTreeHouse.WIN_GETSTAMP(this.winPanel.transform.Find("Window_GetStamp"));
		this.winSocial = new SceneTreeHouse.WIN_SOCIAL(this.winPanel.transform.Find("Window_Social"));
		this.winOpenconf = new SceneTreeHouse.WIN_OPENCONF(this.winPanel.transform.Find("Window_OpenConf"));
		this.winFurniture = new SceneTreeHouse.WIN_FURNITURE(this.winPanel.transform.Find("Window_FurnitureEffect"));
		this.winFilter = new SceneTreeHouse.WIN_FILTER(this.winPanel.transform.Find("Window_Filter"));
		this.winSort = new SceneTreeHouse.WIN_SORT(this.winPanel.transform.Find("SortWindow"));
		this.winName = new SceneTreeHouse.WIN_NAME(this.winPanel.transform.Find("RoomeNameWindow"));
		this.winComment = new SceneTreeHouse.WIN_COMMENT(this.winPanel.transform.Find("AppealCommentWindow"));
		this.winMusic = new SceneTreeHouse.WIN_MUSIC(this.winPanel.transform.Find("Window_Music"));
		this.winCharge = new SceneTreeHouse.WIN_CHARGE(this.winPanel.transform.Find("Window_Charge"));
		this.winChargeUse = new SceneTreeHouse.WIN_CHARGE_USE(this.winPanel.transform.Find("Window_Chage_ItemUse"));
		this.winChargeGet = new SceneTreeHouse.WIN_CHARGE_GET(this.winPanel.transform.Find("Window_Chage_ItemGet"));
		this.winMachine = new SceneTreeHouse.WIN_MACHINE(this.winPanel.transform.Find("Window_Machine"));
		this.winMachineGet = new SceneTreeHouse.WIN_MACHINE_GET(this.winPanel.transform.Find("Window_Machine_ItemGet"));
		this.winMachineAll = new SceneTreeHouse.WIN_MACHINE_ALL(this.winPanel.transform.Find("Window_Machine_All"));
		this.winVR = new SceneTreeHouse.WIN_VR(this.winPanel.transform.Find("Window_VR"));
		this.hidePanel = new GameObject("hide").transform;
		this.hidePanel.SetParent(this.guiData.baseObj.transform, false);
		this.hidePanel.gameObject.SetActive(false);
		this.allFurnitureDataList = new List<SceneTreeHouse.FurnitureData>();
		foreach (TreeHouseFurnitureStatic treeHouseFurnitureStatic in DataManager.DmTreeHouse.GetTreeHouseFurnitureStaticData())
		{
			this.allFurnitureDataList.Add(new SceneTreeHouse.FurnitureData
			{
				dat = treeHouseFurnitureStatic,
				siz = Vector3Int.zero,
				objSiz = Vector3.zero,
				posY = 0f,
				num = 0,
				sortSiz = 0
			});
		}
		this.furnitureDataDisp = new List<SceneTreeHouse.FurnitureData>();
		this.furnitureNew = new List<int>();
		this.batteryData = DataManager.DmTreeHouse.GetChargeBatteryData();
		this.batteryTim = 0;
		this.fuelData = DataManager.DmTreeHouse.GetChargeFuelData();
		this.logList = new List<TreeHouseReceiveStampLog.Log>();
		this.guiData.btnEdit.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.btnMyset.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.btnShop.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.btnGacha.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.btnSocial.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.btnCharge.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.btnMachineAll.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.btnPlacemennt.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.btnMove.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.btnReset.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.btnShopEdit.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.btnGachaEdit.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.btnCancelEdit.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.btnHide.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.btnView.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.btnMusic.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.btnCamera.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.btnOkEdit.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.btnViewEdit.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.btnViewPlace.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.btnRotR.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.btnRotL.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.btnRotG.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.btnOk.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.btnCancel.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.btnTid.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.btnClose.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.btnInfo.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.btnFilter.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.btnSort.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.btnDisp.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.categoryScroll.InitForce();
		ReuseScroll categoryScroll = this.guiData.categoryScroll;
		categoryScroll.onStartItem = (Action<int, GameObject>)Delegate.Combine(categoryScroll.onStartItem, new Action<int, GameObject>(this.SetupCategory));
		ReuseScroll categoryScroll2 = this.guiData.categoryScroll;
		categoryScroll2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(categoryScroll2.onUpdateItem, new Action<int, GameObject>(this.UpdateCategory));
		this.guiData.categoryScroll.Setup(0, 0);
		this.guiData.furnitureScroll.InitForce();
		ReuseScroll furnitureScroll = this.guiData.furnitureScroll;
		furnitureScroll.onStartItem = (Action<int, GameObject>)Delegate.Combine(furnitureScroll.onStartItem, new Action<int, GameObject>(this.SetupFurniture));
		ReuseScroll furnitureScroll2 = this.guiData.furnitureScroll;
		furnitureScroll2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(furnitureScroll2.onUpdateItem, new Action<int, GameObject>(this.UpdateFurniture));
		this.guiData.furnitureScroll.Setup(0, 0);
		for (int i = 0; i < this.guiData.charaIcon.Count; i++)
		{
			Transform icn = this.guiData.charaIcon[i];
			int no = i + 1;
			icn.Find("Img_Blank").gameObject.AddComponent<PguiTouchTrigger>().AddListener(delegate
			{
				this.OnClickChara(icn, no);
			}, null, null, null, null);
		}
		this.haveCharaPackList = new List<CharaPackData>();
		this.dispCharaPackList = new List<CharaPackData>();
		this.charaSortType = SortFilterDefine.SortType.LEVEL;
		this.treehouseChara = new List<Transform>();
		this.guiData.charaScroll.InitForce();
		ReuseScroll charaScroll = this.guiData.charaScroll;
		charaScroll.onStartItem = (Action<int, GameObject>)Delegate.Combine(charaScroll.onStartItem, new Action<int, GameObject>(this.SetupTreeHouseChara));
		ReuseScroll charaScroll2 = this.guiData.charaScroll;
		charaScroll2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(charaScroll2.onUpdateItem, new Action<int, GameObject>(this.SetupTreeHouseChara));
		this.guiData.charaScroll.Setup(0, 0);
		this.guiData.btnOkChara.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.btnTipsHand.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.btnTipsMouse.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.btnResetCam.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.btnGyro.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.btnVR.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.btnCancelCam.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.btnReturn.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.btnHideCam.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiOther.btnBack.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiOther.btnHide.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiOther.btnView.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiOther.btnFollow.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiOther.btnCamera.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiOther.btnSocial.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiOther.btnNext.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiOther.btnInfo.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		for (int j = 0; j < this.guiOther.stamp.Count; j++)
		{
			Transform tmp2 = this.guiOther.stamp[j];
			int id = j + 1;
			tmp2.Find("Icon_Stamp").gameObject.AddComponent<PguiTouchTrigger>().AddListener(delegate
			{
				this.OnClickStamp(tmp2, id);
			}, null, null, null, null);
			tmp2.Find("AEImage_Eff_Send").gameObject.SetActive(false);
			tmp2.Find("Icon_Stamp/Texture_Item").GetComponent<PguiRawImageCtrl>().SetRawImage(this.stampIcon(id), true, false, null);
		}
		this.guiOther.btnResetCam.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiOther.btnGyro.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiOther.btnVR.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiOther.btnCancelCam.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiOther.btnReturn.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiOther.btnHideCam.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		for (int k = 0; k < this.winMyset.list.Count; k++)
		{
			SceneTreeHouse.BAR_MYSET bar_MYSET = this.winMyset.list[k];
			bar_MYSET.baseObj.GetComponent<PguiButtonCtrl>().SetActEnable(false, false, true);
			bar_MYSET.save.gameObject.SetActive(k > 0);
			bar_MYSET.load.gameObject.SetActive(k > 0);
			if (bar_MYSET.save.gameObject.activeSelf)
			{
				bar_MYSET.save.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickMyset), PguiButtonCtrl.SoundType.DEFAULT);
			}
			if (bar_MYSET.load.gameObject.activeSelf)
			{
				bar_MYSET.load.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickMyset), PguiButtonCtrl.SoundType.DEFAULT);
			}
			bar_MYSET.disable.transform.Find("Mark_Lock/Txt_LockInfo").GetComponent<PguiTextCtrl>().text = "";
			int ii = k - 2;
			List<DataManagerMonthlyPack.PurchaseMonthlypackData> list = DataManager.DmMonthlyPack.purchaseMonthlypackDataList.FindAll((DataManagerMonthlyPack.PurchaseMonthlypackData itm) => itm.TreeHouseAddMysetNum > ii);
			string text = "";
			DateTime nowDateTime = TimeManager.Now;
			using (List<DataManagerMonthlyPack.PurchaseMonthlypackData>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					DataManagerMonthlyPack.PurchaseMonthlypackData p = enumerator2.Current;
					if (DataManager.DmPurchase.PurchaseProductStaticList.Find((PurchaseProductStatic itm) => new DateTime(PrjUtil.ConvertTimeToTicks(itm.endTime)) > nowDateTime && itm.monthlyPackId == p.PackId) != null)
					{
						if (!string.IsNullOrEmpty(text))
						{
							text += "・";
						}
						text += p.PackName;
					}
				}
			}
			bar_MYSET.disable.transform.Find("Txt01").GetComponent<PguiTextCtrl>().text = ((list.Count >= 4) ? "月間パスポート" : text);
			bar_MYSET.disable.transform.Find("Txt02").GetComponent<PguiTextCtrl>().text = ((list.Count <= 0) ? "" : (((list.Count > 1) ? "いずれか" : "") + "所持時に有効"));
			bar_MYSET.disable.SetActive(ii >= 0);
		}
		this.winSocial.btnName.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickSocial), PguiButtonCtrl.SoundType.DEFAULT);
		this.winSocial.btnComment.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickSocial), PguiButtonCtrl.SoundType.DEFAULT);
		this.winSocial.btnOpenconf.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickSocial), PguiButtonCtrl.SoundType.DEFAULT);
		this.winSocial.btnConfCopy.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickSocial), PguiButtonCtrl.SoundType.DEFAULT);
		this.winSocial.btnVisit.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickSocial), PguiButtonCtrl.SoundType.DEFAULT);
		this.winSocial.btnCopy.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickSocial), PguiButtonCtrl.SoundType.DEFAULT);
		for (int l = 0; l < this.winSocial.stamp.Count; l++)
		{
			this.winSocial.stamp[l].Find("Icon_Stamp/Texture_Item").GetComponent<PguiRawImageCtrl>().SetRawImage(this.stampIcon(l + 1), true, false, null);
		}
		this.winOpenconf.btnOpen.AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickOpenConf));
		this.winOpenconf.btnFollower.AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickOpenConf));
		this.winOpenconf.btnPrivate.AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickOpenConf));
		foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl in this.winSort.btn)
		{
			pguiToggleButtonCtrl.AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickSort));
		}
		this.winName.inputField.onEndEdit.AddListener(delegate(string str)
		{
			this.winName.inputField.text = PrjUtil.ModifiedRoomName(str);
			this.winName.errorMessage.SetActive(false);
		});
		this.winComment.inputField.onEndEdit.AddListener(delegate(string str)
		{
			this.winComment.inputField.text = PrjUtil.ModifiedComment(str);
			this.winComment.errorMessage.SetActive(false);
		});
		this.winMusic.btnChk.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickMusic), PguiButtonCtrl.SoundType.DEFAULT);
		this.chargeFuel = new List<UseItem>();
		for (int m = 0; m < this.winCharge.fuel.Count; m++)
		{
			Transform tmp = this.winCharge.fuel[m];
			if (m >= this.fuelData.Count)
			{
				tmp.gameObject.SetActive(false);
			}
			else
			{
				tmp.gameObject.SetActive(true);
				tmp.name = this.fuelData[m].itemId.ToString();
				tmp.Find("Titlebase/Title").GetComponent<PguiTextCtrl>().text = "-" + this.fuelData[m].addTime.ToString() + "分";
				IconItemCtrl component = tmp.Find("ItemIconSet/Icon_Item").GetComponent<IconItemCtrl>();
				component.Setup(DataManager.DmItem.GetItemStaticBase(this.fuelData[m].itemId));
				component.AddOnClickListener(delegate(IconItemCtrl xxx)
				{
					this.OnClickFuel(tmp);
				});
				component.AddOnLongClickListener(delegate(IconItemCtrl xxx)
				{
					this.OnLongClickFuel(tmp);
				});
				tmp.gameObject.AddComponent<PguiTouchTrigger>().AddListener(delegate
				{
					this.OnClickFuel(tmp);
				}, delegate
				{
					this.OnLongClickFuel(tmp);
				}, null, null, null);
				this.chargeFuel.Add(new UseItem
				{
					use_item_id = this.fuelData[m].itemId,
					use_item_num = 0
				});
			}
		}
		this.winChargeUse.btnPlus.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickCharge), PguiButtonCtrl.SoundType.DEFAULT);
		this.winChargeUse.btnMinus.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickCharge), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.baseObj.SetActive(false);
		this.guiOther.baseObj.SetActive(false);
		this.winPanel.SetActive(false);
		this.winGetstamp.scroll.InitForce();
		ReuseScroll scroll = this.winGetstamp.scroll;
		scroll.onStartItem = (Action<int, GameObject>)Delegate.Combine(scroll.onStartItem, new Action<int, GameObject>(this.SetupGetStamp));
		ReuseScroll scroll2 = this.winGetstamp.scroll;
		scroll2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scroll2.onUpdateItem, new Action<int, GameObject>(this.UpdateGetStamp));
		this.winGetstamp.scroll.Setup(0, 0);
		this.winSocial.scrList.InitForce();
		ReuseScroll scrList = this.winSocial.scrList;
		scrList.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrList.onStartItem, new Action<int, GameObject>(this.SetupSocialList));
		ReuseScroll scrList2 = this.winSocial.scrList;
		scrList2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrList2.onUpdateItem, new Action<int, GameObject>(this.UpdateSocialList));
		this.winSocial.scrList.Setup(0, 0);
		this.winSocial.scrRank.InitForce();
		ReuseScroll scrRank = this.winSocial.scrRank;
		scrRank.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrRank.onStartItem, new Action<int, GameObject>(this.SetupSocialRank));
		ReuseScroll scrRank2 = this.winSocial.scrRank;
		scrRank2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrRank2.onUpdateItem, new Action<int, GameObject>(this.UpdateSocialRank));
		this.winSocial.scrRank.Setup(0, 0);
		this.winMachineGet.scroll.InitForce();
		ReuseScroll scroll3 = this.winMachineGet.scroll;
		scroll3.onStartItem = (Action<int, GameObject>)Delegate.Combine(scroll3.onStartItem, new Action<int, GameObject>(this.SetupGetMachine));
		ReuseScroll scroll4 = this.winMachineGet.scroll;
		scroll4.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scroll4.onUpdateItem, new Action<int, GameObject>(this.UpdateGetMachine));
		this.winMachineGet.scroll.Setup(0, 0);
		this.winMachineAll.scroll.InitForce();
		ReuseScroll scroll5 = this.winMachineAll.scroll;
		scroll5.onStartItem = (Action<int, GameObject>)Delegate.Combine(scroll5.onStartItem, new Action<int, GameObject>(this.SetupMachineAll));
		ReuseScroll scroll6 = this.winMachineAll.scroll;
		scroll6.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scroll6.onUpdateItem, new Action<int, GameObject>(this.UpdateMachineAll));
		this.winMachineAll.scroll.Setup(0, 0);
		this.bgmList = new List<int>();
		this.bgmPanel = AssetManager.InstantiateAssetData("SceneTreeHouse/GUI/Prefab/TreeHouse_BGM_Btn_BGMSelect", null);
		this.bgmPanel.transform.SetParent(this.hidePanel, false);
		this.bgmLineup = new List<Transform>();
		this.playBgm = 0;
		this.testBgm = -1f;
		this.winMusic.scroll.InitForce();
		ReuseScroll scroll7 = this.winMusic.scroll;
		scroll7.onStartItem = (Action<int, GameObject>)Delegate.Combine(scroll7.onStartItem, new Action<int, GameObject>(this.SetupBgmLineup));
		ReuseScroll scroll8 = this.winMusic.scroll;
		scroll8.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scroll8.onUpdateItem, new Action<int, GameObject>(this.UpdateBgmLineup));
		this.winMusic.scroll.Setup(0, 0);
		this.winMyset.win.SetupButtonOnly(null);
		this.winGetstamp.win.SetupButtonOnly(null);
		this.winSocial.win.SetupButtonOnly(delegate(int index)
		{
			this.SaveSocialIndex();
			return true;
		});
		this.winOpenconf.win.SetupButtonOnly(null);
		this.winFurniture.win.SetupButtonOnly(null);
		this.winFilter.win.SetupButtonOnly(null);
		this.winSort.win.SetupButtonOnly(new PguiOpenWindowCtrl.Callback(this.ExecSort));
		this.winName.win.SetupButtonOnly(null);
		this.winComment.win.SetupButtonOnly(null);
		this.winMusic.win.SetupButtonOnly(null);
		this.winCharge.win.SetupButtonOnly(null);
		this.winChargeUse.win.SetupKemoBoardResetCheck(null);
		this.winChargeGet.win.SetupButtonOnly(null);
		this.winMachine.win.SetupButtonOnly(null);
		this.winMachineGet.win.SetupButtonOnly(null);
		this.winMachineAll.win.SetupButtonOnly(null);
		this.winVR.win.SetupButtonOnly(null);
		this.field = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneTreeHouse/FieldSceneTreeHouse"));
		SceneManager.Add3DObjectByBaseField(this.field.transform);
		this.field.SetActive(false);
		this.furnitureBase = new GameObject("Furniture");
		this.furnitureBase.transform.SetParent(this.field.transform, false);
		this.furnitureBase.SetActive(true);
		this.doorBase = null;
		this.doorGridMap = new Dictionary<GameObject, Vector3>();
		this.charaBase = new GameObject("Chara");
		this.charaBase.transform.SetParent(this.field.transform, false);
		this.charaBase.SetActive(true);
		AssetManager.LoadAssetData(SceneTreeHouse.LOCATOR_PATH, AssetManager.OWNER.TreeHouseStage, 0, null);
		this.stageLocator = null;
		this.floorGrid = null;
		this.wallGrid = null;
		this.curtainBoard = null;
		this.stageCtrl = null;
		this.stageLight = null;
		this.stageLoad = null;
		this.stageType = SceneHome.StageType.INVALID;
		this.smallFurnitureDataList = DataManager.DmTreeHouse.GetSmallFurnitureData();
	}

	public override bool OnCreateSceneWait()
	{
		if (!AssetManager.IsLoadFinishAssetData(SceneTreeHouse.LOCATOR_PATH))
		{
			return false;
		}
		this.stageLocator = AssetManager.InstantiateAssetData(SceneTreeHouse.LOCATOR_PATH, null);
		this.stageLocator.transform.SetParent(this.field.transform, false);
		this.stageLocator.layer = SceneTreeHouse.stageLayer;
		this.stageLocator.SetActive(false);
		AssetManager.AddLoadList(SceneTreeHouse.LOCATOR_PATH, AssetManager.OWNER.TreeHouseStage);
		foreach (SceneTreeHouse.FurnitureData furnitureData in this.allFurnitureDataList)
		{
			int length = furnitureData.dat.modelFileName.Length;
			Transform transform = this.stageLocator.transform.Find("siz_obj_" + furnitureData.dat.modelFileName.Substring(length - 10, 6) + "_a");
			furnitureData.siz = Vector3Int.one;
			furnitureData.objSiz = Vector3.zero;
			furnitureData.posY = 0f;
			if (furnitureData.dat.category == TreeHouseFurnitureStatic.Category.CEIL_LIGHT || furnitureData.dat.category == TreeHouseFurnitureStatic.Category.CEIL_DECO || furnitureData.dat.category == TreeHouseFurnitureStatic.Category.WALL_PAPER || furnitureData.dat.category == TreeHouseFurnitureStatic.Category.FLOOR_PAPER)
			{
				furnitureData.siz = Vector3Int.zero;
				furnitureData.sortSiz = 1;
				furnitureData.objSiz.y = 0.5f;
			}
			else if (transform != null)
			{
				if (furnitureData.dat.category == TreeHouseFurnitureStatic.Category.WALL_HANGINGS || furnitureData.dat.category == TreeHouseFurnitureStatic.Category.CURTAIN)
				{
					furnitureData.siz.x = (int)((transform.localScale.x + 12.49f) / 12.5f);
					furnitureData.siz.y = (int)((transform.localScale.y + 12.49f) / 12.5f);
					furnitureData.siz.z = ((transform.localScale.z < 2f) ? (-1) : ((transform.localScale.z < 10f) ? 0 : ((int)((transform.localScale.z + 24.9f) / 25f))));
				}
				else
				{
					furnitureData.siz.x = (int)((transform.localScale.x + 24.9f) / 25f);
					furnitureData.siz.y = (int)((transform.localScale.y + 24.9f) / 25f);
					furnitureData.siz.z = (int)((transform.localScale.z + 24.9f) / 25f);
				}
				if (furnitureData.dat.category == TreeHouseFurnitureStatic.Category.WALL_HANGINGS || furnitureData.dat.category == TreeHouseFurnitureStatic.Category.CURTAIN)
				{
					furnitureData.sortSiz = furnitureData.siz.x * furnitureData.siz.y;
				}
				else
				{
					furnitureData.sortSiz = furnitureData.siz.x * furnitureData.siz.z;
				}
				furnitureData.objSiz = transform.localScale / 100f;
				if (furnitureData.dat.category == TreeHouseFurnitureStatic.Category.CURTAIN)
				{
					furnitureData.posY = transform.position.y / 2f;
				}
			}
		}
		this.camera = this.field.GetComponentInChildren<Camera>(true).gameObject.AddComponent<FieldCameraScaler>();
		this.camera.fieldCamera.depth = (float)((SceneManager.CameraDepth[SceneManager.CanvasType.BACK] + SceneManager.CameraDepth[SceneManager.CanvasType.FRONT]) / 2);
		this.camera.fieldCamera.cullingMask = (1 << this.stageLocator.layer) | (1 << SceneTreeHouse.charaLayer) | (1 << SceneTreeHouse.charaShadowLayer) | (1 << SceneTreeHouse.bloomLayer);
		this.camera.gameObject.AddComponent<CriAtomListener>();
		this.camBCG = this.camera.GetComponent<CC_BrightnessContrastGamma>();
		this.camBCG.enabled = false;
		this.camL = new GameObject("CamL", new Type[] { typeof(Camera) }).gameObject.AddComponent<FieldCameraScaler>();
		this.camL.transform.SetParent(this.field.transform, false);
		this.camL.transform.SetSiblingIndex(this.camera.transform.GetSiblingIndex() + 1);
		this.camL.fieldCamera.CopyFrom(this.camera.fieldCamera);
		this.camL.fieldCamera.depth = (float)((SceneManager.CameraDepth[SceneManager.CanvasType.BACK] + SceneManager.CameraDepth[SceneManager.CanvasType.FRONT]) / 2);
		this.camL.fieldCamera.cullingMask = (1 << this.stageLocator.layer) | (1 << SceneTreeHouse.charaLayer) | (1 << SceneTreeHouse.charaShadowLayer) | (1 << SceneTreeHouse.bloomLayer);
		this.camL.rect = new Rect(-0.2f, -0.1f, 0.7f, 1.1f);
		this.camL.gameObject.AddComponent<CriAtomListener>();
		this.camL.gameObject.SetActive(false);
		this.camL.gameObject.AddComponent<MultipleGaussianBloom>().gaussianFilter = MultipleGaussianBloom.FilterTaps._5Taps;
		this.camR = new GameObject("CamR", new Type[] { typeof(Camera) }).gameObject.AddComponent<FieldCameraScaler>();
		this.camR.transform.SetParent(this.field.transform, false);
		this.camR.transform.SetSiblingIndex(this.camL.transform.GetSiblingIndex() + 1);
		this.camR.fieldCamera.depth = (float)((SceneManager.CameraDepth[SceneManager.CanvasType.BACK] + SceneManager.CameraDepth[SceneManager.CanvasType.FRONT]) / 2);
		this.camR.fieldCamera.cullingMask = (1 << this.stageLocator.layer) | (1 << SceneTreeHouse.charaLayer) | (1 << SceneTreeHouse.charaShadowLayer) | (1 << SceneTreeHouse.bloomLayer);
		this.camR.rect = new Rect(0.5f, -0.1f, 0.7f, 1.1f);
		this.camR.gameObject.SetActive(false);
		this.camR.gameObject.AddComponent<MultipleGaussianBloom>().gaussianFilter = MultipleGaussianBloom.FilterTaps._5Taps;
		this.camViewNo = (this.camOtherNo = 0);
		this.camView = new List<SceneTreeHouse.CamDat>();
		this.camOther = new List<SceneTreeHouse.CamDat>();
		char c = 'a';
		for (;;)
		{
			Transform transform2 = this.stageLocator.transform.Find("cam_pos_view_" + c.ToString());
			if (transform2 == null)
			{
				break;
			}
			transform2.localScale = new Vector3(Mathf.DeltaAngle(0f, transform2.localEulerAngles.x), Mathf.DeltaAngle(0f, transform2.localEulerAngles.y), 50f);
			this.camView.Add(new SceneTreeHouse.CamDat
			{
				bas = transform2,
				ang = Vector2.zero,
				fov = 0f
			});
			this.camOther.Add(new SceneTreeHouse.CamDat
			{
				bas = transform2,
				ang = Vector2.zero,
				fov = 0f
			});
			c += '\u0001';
		}
		while (this.camView.Count < 4)
		{
			this.camView.Add(new SceneTreeHouse.CamDat
			{
				bas = this.camView[0].bas,
				ang = Vector2.zero,
				fov = 0f
			});
		}
		while (this.camOther.Count < 4)
		{
			this.camOther.Add(new SceneTreeHouse.CamDat
			{
				bas = this.camOther[0].bas,
				ang = Vector2.zero,
				fov = 0f
			});
		}
		this.camEditNo = 0;
		this.camEdit = new List<SceneTreeHouse.CamDat>();
		char c2 = 'a';
		for (;;)
		{
			Transform transform3 = this.stageLocator.transform.Find("cam_pos_edit_" + c2.ToString());
			if (transform3 == null)
			{
				break;
			}
			transform3.localScale = new Vector3(Mathf.DeltaAngle(0f, transform3.localEulerAngles.x), Mathf.DeltaAngle(0f, transform3.localEulerAngles.y), 50f);
			this.camEdit.Add(new SceneTreeHouse.CamDat
			{
				bas = transform3,
				ang = Vector2.zero,
				fov = 0f
			});
			c2 += '\u0001';
		}
		while (this.camEdit.Count < 6)
		{
			this.camEdit.Add(new SceneTreeHouse.CamDat
			{
				bas = this.camEdit[0].bas,
				ang = Vector2.zero,
				fov = 0f
			});
		}
		this.wallLoc = new List<Transform>();
		float num = 0f;
		char c3 = 'a';
		for (;;)
		{
			Transform transform4 = this.stageLocator.transform.Find("pos_wall_grid_" + c3.ToString());
			if (transform4 == null)
			{
				break;
			}
			this.wallLoc.Add(transform4);
			num += transform4.position.x * transform4.position.x + transform4.position.z * transform4.position.z;
			c3 += '\u0001';
		}
		num = ((this.wallLoc.Count > 0) ? (num / (float)this.wallLoc.Count) : 25f);
		float num2 = (num = Mathf.Sqrt(num)) / Mathf.Sqrt(3f);
		this.wallPos = new List<Vector3>
		{
			new Vector3(num2, 0f, num),
			new Vector3(-num2, 0f, num),
			new Vector3(-num2 - num2, 0f, 0f),
			new Vector3(-num2, 0f, -num),
			new Vector3(num2, 0f, -num),
			new Vector3(num2 + num2, 0f, 0f)
		};
		this.curtainLoc = new List<Transform>();
		char c4 = 'a';
		for (;;)
		{
			Transform transform5 = this.stageLocator.transform.Find("pos_curtain_" + c4.ToString());
			if (transform5 == null)
			{
				break;
			}
			this.curtainLoc.Add(transform5);
			c4 += '\u0001';
		}
		this.lightLoc = this.stageLocator.transform.Find("pos_ceiling_light_a");
		return true;
	}

	private IEnumerator StageLoad(bool ini)
	{
		this.camBCG.enabled = true;
		string nm = StagePresetCtrl.PackDataPath + SceneTreeHouse.STAGE_PATH[this.stageType - SceneHome.StageType.MORNING];
		AssetManager.LoadAssetData(nm, AssetManager.OWNER.TreeHouseStage, 0, null);
		if (ini)
		{
			this.camBCG.brightness = -100f;
			goto IL_0213;
		}
		do
		{
			yield return null;
		}
		while ((this.camBCG.brightness -= TimeManager.DeltaTime * 200f) > -100f);
		this.camBCG.brightness = -100f;
		foreach (SceneTreeHouse.FurnitureCtrl furnitureCtrl in this.furnitureList)
		{
			if (furnitureCtrl.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_PAPER || furnitureCtrl.data.dat.category == TreeHouseFurnitureStatic.Category.FLOOR_PAPER || furnitureCtrl.data.dat.category == TreeHouseFurnitureStatic.Category.CEIL_DECO)
			{
				furnitureCtrl.chgmdl = true;
			}
		}
		foreach (SceneTreeHouse.FurnitureCtrl furnitureCtrl2 in this.otherFurniture)
		{
			if (furnitureCtrl2.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_PAPER || furnitureCtrl2.data.dat.category == TreeHouseFurnitureStatic.Category.FLOOR_PAPER || furnitureCtrl2.data.dat.category == TreeHouseFurnitureStatic.Category.CEIL_DECO)
			{
				furnitureCtrl2.chgmdl = true;
			}
		}
		using (List<SceneTreeHouse.PaperCtrl>.Enumerator enumerator2 = this.paperList.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				SceneTreeHouse.PaperCtrl paperCtrl = enumerator2.Current;
				paperCtrl.chgmdl = true;
			}
			goto IL_0213;
		}
		IL_01FC:
		yield return null;
		IL_0213:
		if (AssetManager.IsLoadFinishAssetData(nm))
		{
			this.stageLight = null;
			if (this.stageCtrl != null)
			{
				Object.Destroy(this.stageCtrl.gameObject);
			}
			PrjUtil.ReleaseMemory(PrjUtil.UnloadUnused / 10);
			this.stageCtrl = AssetManager.InstantiateAssetData(nm, null).GetComponent<StagePresetCtrl>();
			this.stageCtrl.transform.SetParent(this.field.transform, false);
			this.stageCtrl.Setting(this.camera.fieldCamera);
			this.stageCtrl.RenderSettingParam.Param2Scene();
			AssetManager.AddLoadList(nm, AssetManager.OWNER.TreeHouseStage);
			yield return null;
			this.stageLight = new Dictionary<string, List<Light>>();
			foreach (object obj in this.stageCtrl.transform)
			{
				Transform transform = (Transform)obj;
				if (transform.name.StartsWith("Stage_Interiorlight_"))
				{
					string text = transform.name.Substring(5);
					if (!this.stageLight.ContainsKey(text))
					{
						this.stageLight.Add(text, new List<Light> { null, null });
					}
					this.stageLight[text][0] = transform.GetComponent<Light>();
				}
				if (transform.name.StartsWith("Chara_Interiorlight_"))
				{
					string text2 = transform.name.Substring(5);
					if (!this.stageLight.ContainsKey(text2))
					{
						this.stageLight.Add(text2, new List<Light> { null, null });
					}
					this.stageLight[text2][1] = transform.GetComponent<Light>();
				}
			}
			foreach (string text3 in this.stageLight.Keys)
			{
				List<Light> list = this.stageLight[text3];
				if (list[1] == null && list[0] != null)
				{
					list[1] = list[0];
					list[1].cullingMask |= 1 << LayerMask.NameToLayer("FieldPlayer");
					list[1].cullingMask |= 1 << LayerMask.NameToLayer("FieldPlayerAlpha");
				}
			}
			for (;;)
			{
				bool flag = false;
				foreach (SceneTreeHouse.FurnitureCtrl furnitureCtrl3 in this.furnitureList)
				{
					if (furnitureCtrl3.mdl == null)
					{
						flag = true;
					}
					else if (furnitureCtrl3.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_PAPER || furnitureCtrl3.data.dat.category == TreeHouseFurnitureStatic.Category.FLOOR_PAPER || furnitureCtrl3.data.dat.category == TreeHouseFurnitureStatic.Category.CEIL_DECO)
					{
						flag |= furnitureCtrl3.chgmdl;
					}
				}
				foreach (SceneTreeHouse.FurnitureCtrl furnitureCtrl4 in this.otherFurniture)
				{
					if (furnitureCtrl4.mdl == null)
					{
						flag = true;
					}
					else if (furnitureCtrl4.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_PAPER || furnitureCtrl4.data.dat.category == TreeHouseFurnitureStatic.Category.FLOOR_PAPER || furnitureCtrl4.data.dat.category == TreeHouseFurnitureStatic.Category.CEIL_DECO)
					{
						flag |= furnitureCtrl4.chgmdl;
					}
				}
				foreach (SceneTreeHouse.PaperCtrl paperCtrl2 in this.paperList)
				{
					if (paperCtrl2.enable && (paperCtrl2.mdl == null || paperCtrl2.chgmdl))
					{
						flag = true;
					}
				}
				if (!flag)
				{
					break;
				}
				yield return null;
			}
			foreach (SceneTreeHouse.FurnitureCtrl furnitureCtrl5 in this.furnitureList)
			{
				if (furnitureCtrl5.no >= 0 && furnitureCtrl5.data.dat.category == TreeHouseFurnitureStatic.Category.CEIL_LIGHT)
				{
					furnitureCtrl5.onoff = this.IsNight;
				}
			}
			this.lightName = "xyz";
			this.ChangeStageLight();
			if (ini)
			{
				this.camBCG.brightness = -100f;
			}
			else
			{
				do
				{
					yield return null;
				}
				while ((this.camBCG.brightness += TimeManager.DeltaTime * 200f) < 0f);
				this.camBCG.brightness = 0f;
			}
			this.camBCG.enabled = false;
			yield break;
		}
		goto IL_01FC;
	}

	private void ChangeStageLight()
	{
		if (this.stageCtrl == null || this.stageLight == null)
		{
			return;
		}
		SceneTreeHouse.FurnitureCtrl furnitureCtrl = this.furnitureList.Find((SceneTreeHouse.FurnitureCtrl itm) => itm.no == 0 && itm.data.dat.category == TreeHouseFurnitureStatic.Category.CEIL_LIGHT && itm.mdl != null);
		if (furnitureCtrl == null)
		{
			furnitureCtrl = this.furnitureList.Find((SceneTreeHouse.FurnitureCtrl itm) => itm.no > 0 && itm.data.dat.category == TreeHouseFurnitureStatic.Category.CEIL_LIGHT && itm.mdl != null);
		}
		if (furnitureCtrl == null)
		{
			furnitureCtrl = this.otherFurniture.Find((SceneTreeHouse.FurnitureCtrl itm) => itm.no > 0 && itm.data.dat.category == TreeHouseFurnitureStatic.Category.CEIL_LIGHT && itm.mdl != null);
		}
		string text = ((furnitureCtrl != null && furnitureCtrl.onoff && this.stageLight.ContainsKey(furnitureCtrl.light)) ? furnitureCtrl.light : "");
		if (this.lightName == text)
		{
			return;
		}
		this.lightName = text;
		Light light = (this.stageLight.ContainsKey(this.lightName) ? this.stageLight[this.lightName][0] : null);
		Light light2 = (this.stageLight.ContainsKey(this.lightName) ? this.stageLight[this.lightName][1] : null);
		this.stageCtrl.lightByStage.gameObject.SetActive(light == null);
		this.stageCtrl.lightByPlayer.gameObject.SetActive(light2 == null);
		foreach (List<Light> list in this.stageLight.Values)
		{
			list[0].gameObject.SetActive(list[0] == light);
			list[1].gameObject.SetActive(list[1] == light2);
		}
	}

	public override void OnEnableScene(object args)
	{
		CanvasManager.HdlCmnMenu.SetupMenu(true, "わいわいツリーハウス", new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickButtonMenuRetrun), "", new PguiCmnMenuCtrl.OnClickMoveSequenceButton(this.OnClickButtonMenu), null);
		CanvasManager.SetBgTexture(null);
		EffectManager.BillboardCamera = this.camera.fieldCamera;
		this.guiData.baseObj.SetActive(true);
		this.guiData.anime.ExPauseAnimation("START");
		this.guiOther.baseObj.SetActive(false);
		this.guiOther.stampAll.SetActive(true);
		this.guiOther.rightBtn.SetActive(true);
		this.winPanel.SetActive(false);
		this.winMyset.win.ForceClose();
		this.winGetstamp.win.ForceClose();
		this.winSocial.win.ForceClose();
		this.winOpenconf.win.ForceClose();
		this.winFurniture.win.ForceClose();
		this.winFilter.win.ForceClose();
		this.winSort.win.ForceClose();
		this.winName.win.ForceClose();
		this.winComment.win.ForceClose();
		this.winMusic.win.ForceClose();
		this.winCharge.win.ForceClose();
		this.winChargeUse.win.ForceClose();
		this.winChargeGet.win.ForceClose();
		this.winMachine.win.ForceClose();
		this.winMachineGet.win.ForceClose();
		this.winMachineAll.win.ForceClose();
		this.winVR.win.ForceClose();
		this.field.SetActive(true);
		this.camera.gameObject.SetActive(false);
		this.camL.gameObject.SetActive(false);
		this.camR.gameObject.SetActive(false);
		this.camera.gameObject.SetActive(true);
		foreach (SceneTreeHouse.CamDat camDat in this.camView)
		{
			camDat.ang = Vector2.zero;
			camDat.fov = 0f;
		}
		foreach (SceneTreeHouse.CamDat camDat2 in this.camOther)
		{
			camDat2.ang = Vector2.zero;
			camDat2.fov = 0f;
		}
		foreach (SceneTreeHouse.CamDat camDat3 in this.camEdit)
		{
			camDat3.ang = Vector2.zero;
			camDat3.fov = 0f;
		}
		this.camViewNo = (this.camOtherNo = 0);
		this.camera.transform.position = this.camView[this.camViewNo].bas.position;
		this.camera.transform.eulerAngles = new Vector3(this.camView[this.camViewNo].bas.localScale.x, this.camView[this.camViewNo].bas.localScale.y, 0f);
		this.camera.fieldOfView = this.camView[this.camViewNo].bas.localScale.z;
		this.camEditNo = 0;
		this.gyroLst = new List<SceneTreeHouse.FurnitureCtrl>();
		this.gyroCam = null;
		this.gyroBase = Quaternion.identity;
		this.gyroAng = Vector2.zero;
		this.gyroFov = 0f;
		this.gyroSpace = 0.05f;
		this.guiData.btnCamera.SetActEnable(false, false, false);
		this.guiOther.btnCamera.SetActEnable(false, false, false);
		AssetManager.LoadAssetData(SceneTreeHouse.BOARD_PATH, AssetManager.OWNER.TreeHouseStage, 0, null);
		AssetManager.LoadAssetData(SceneTreeHouse.CURTAIN_BOARD_PATH, AssetManager.OWNER.TreeHouseStage, 0, null);
		this.curtainBoard = null;
		AssetManager.LoadAssetData(SceneTreeHouse.GRID_FLOOR_PATH, AssetManager.OWNER.TreeHouseStage, 0, null);
		AssetManager.LoadAssetData(SceneTreeHouse.GRID_WALL_PATH, AssetManager.OWNER.TreeHouseStage, 0, null);
		AssetManager.LoadAssetData(SceneTreeHouse.GRID_DEPEND_PATH, AssetManager.OWNER.TreeHouseStage, 0, null);
		this.floorGrid = null;
		this.wallGrid = null;
		this.ienum = new List<IEnumerator>();
		this.stageCtrl = null;
		this.stageLight = null;
		this.stageType = SceneHome.GetStageType();
		this.stageLoad = this.StageLoad(true);
		this.isMove = 0;
		this.furnitureList = null;
		this.furnitureMove = null;
		this.furniturePlace = 0;
		this.furnitureSel = -1;
		this.categorySel = TreeHouseFurnitureStatic.Category.INVALID;
		this.furnitureNew = new List<int>();
		this.furnitureDataDisp = new List<SceneTreeHouse.FurnitureData>();
		this.favoriteFuniture = null;
		this.paperList = null;
		this.doorBase = null;
		this.doorGridMap = new Dictionary<GameObject, Vector3>();
		this.haveCharaPackList = new List<CharaPackData>(DataManager.DmChara.GetUserCharaMap().Values);
		this.dispCharaPackList = new List<CharaPackData>(this.haveCharaPackList);
		this.charaList = null;
		this.actionList = new List<SceneTreeHouse.ActionCtrl>();
		this.batteryInfo = DataManager.DmItem.GetUserItemData(this.batteryData.getItemId);
		MasterRoomMachineDataModel masterRoomMachineDataModel = DataManager.DmHome.GetHomeCheckResult().OriginTreeHouseChargeTimeList.Find((MasterRoomMachineDataModel item) => item.machineId == DataManager.DmTreeHouse.GetChargeBatteryData().id);
		if (DataManager.DmHome.GetHomeCheckResult() != null && masterRoomMachineDataModel.nextsecond < 0)
		{
			this.batteryInfo = null;
		}
		DataManager.DmTreeHouse.RequestGetTreeHouseBase(true);
		this.otherFurniture = new List<SceneTreeHouse.FurnitureCtrl>();
		this.otherChara = new List<SceneTreeHouse.CharaCtrl>();
		this.otherAction = new List<SceneTreeHouse.ActionCtrl>();
		this.kizunaRatio = "";
		this.monthlyDay = -1;
		if (SceneTreeHouse.furnitureDispType != SceneTreeHouse.FurnitureDispType.ALL && SceneTreeHouse.furnitureDispType != SceneTreeHouse.FurnitureDispType.FAVORITE)
		{
			SceneTreeHouse.furnitureDispType = SceneTreeHouse.FurnitureDispType.ALL;
		}
		this.guiData.btnFilter.gameObject.SetActive(false);
		this.guiData.btnDisp.transform.Find("BaseImage/On/Txt").GetComponent<PguiTextCtrl>().text = ((SceneTreeHouse.furnitureDispType == SceneTreeHouse.FurnitureDispType.FAVORITE) ? "お気に入り" : "一覧");
		this.guiData.loading.gameObject.SetActive(false);
		bool disableTreeHouseTips = DataManager.DmGameStatus.MakeUserFlagData().InformationsFlag.DisableTreeHouseTips;
		this.guiData.tipsHand.SetActive(false);
		this.guiData.tipsMouse.SetActive(!disableTreeHouseTips);
		this.guiData.chkTipsHand.SetActive(false);
		this.guiData.chkTipsMouse.SetActive(false);
		this.winSocial.txtSearch.text = string.Empty;
		this.winSocial.txtConfMyid.text = (this.winSocial.txtMyid.text = DataManager.DmUserInfo.friendId.ToString());
		this.winMusic.imgChk.SetActive(!DataManager.DmGameStatus.MakeUserFlagData().InformationsFlag.DisableTreeHouseBgmChange);
		this.furnitureMapEdit = (this.furnitureMapSave = null);
		foreach (string text in SceneTreeHouse.emoEffName.Values)
		{
			EffectManager.ReqLoadEffect(text, AssetManager.OWNER.TreeHouseStage, 0, null);
		}
		EffectManager.ReqLoadEffect(SceneTreeHouse.cupEffName, AssetManager.OWNER.TreeHouseStage, 0, null);
		EffectManager.ReqLoadEffect(SceneTreeHouse.penEffName, AssetManager.OWNER.TreeHouseStage, 0, null);
		EffectManager.ReqLoadEffect(SceneTreeHouse.penEasterEffName, AssetManager.OWNER.TreeHouseStage, 0, null);
		EffectManager.ReqLoadEffect(SceneTreeHouse.potEffName, AssetManager.OWNER.TreeHouseStage, 0, null);
		EffectManager.ReqLoadEffect(SceneTreeHouse.ringEffName, AssetManager.OWNER.TreeHouseStage, 0, null);
		EffectManager.ReqLoadEffect(SceneTreeHouse.ringNewYearEffName, AssetManager.OWNER.TreeHouseStage, 0, null);
		EffectManager.ReqLoadEffect(SceneTreeHouse.ballEffName, AssetManager.OWNER.TreeHouseStage, 0, null);
		EffectManager.ReqLoadEffect(SceneTreeHouse.ballBasketEffName, AssetManager.OWNER.TreeHouseStage, 0, null);
		EffectManager.ReqLoadEffect(SceneTreeHouse.gdnEffName, AssetManager.OWNER.TreeHouseStage, 0, null);
		EffectManager.ReqLoadEffect(SceneTreeHouse.gdnChristmasEffName, AssetManager.OWNER.TreeHouseStage, 0, null);
		EffectManager.ReqLoadEffect(SceneTreeHouse.gdnTinplateEffName, AssetManager.OWNER.TreeHouseStage, 0, null);
		EffectManager.ReqLoadEffect(SceneTreeHouse.stkEffName, AssetManager.OWNER.TreeHouseStage, 0, null);
		EffectManager.ReqLoadEffect(SceneTreeHouse.spnEffName, AssetManager.OWNER.TreeHouseStage, 0, null);
		EffectManager.ReqLoadEffect(SceneTreeHouse.grnEffName, AssetManager.OWNER.TreeHouseStage, 0, null);
		EffectManager.ReqLoadEffect(SceneTreeHouse.mkr1EffName, AssetManager.OWNER.TreeHouseStage, 0, null);
		EffectManager.ReqLoadEffect(SceneTreeHouse.mkr2EffName, AssetManager.OWNER.TreeHouseStage, 0, null);
		EffectManager.ReqLoadEffect(SceneTreeHouse.mkr1YewYearEffName, AssetManager.OWNER.TreeHouseStage, 0, null);
		EffectManager.ReqLoadEffect(SceneTreeHouse.mkr2YewYearEffName, AssetManager.OWNER.TreeHouseStage, 0, null);
		EffectManager.ReqLoadEffect(SceneTreeHouse.onpEffName, AssetManager.OWNER.TreeHouseStage, 0, null);
		EffectManager.ReqLoadEffect(SceneTreeHouse.ladleEffName, AssetManager.OWNER.TreeHouseStage, 0, null);
		EffectManager.ReqLoadEffect(SceneTreeHouse.jpncupEffName, AssetManager.OWNER.TreeHouseStage, 0, null);
		foreach (TreeHouseSmallFurnitureData treeHouseSmallFurnitureData in this.smallFurnitureDataList)
		{
			foreach (TreeHouseSmallFurnitureData.MotionData motionData in treeHouseSmallFurnitureData.motionDataList)
			{
				EffectManager.ReqLoadEffect(motionData.modelName, AssetManager.OWNER.TreeHouseStage, 0, null);
			}
			foreach (TreeHouseSmallFurnitureData.OptData optData in treeHouseSmallFurnitureData.optDataList)
			{
				EffectManager.ReqLoadEffect(optData.modelName, AssetManager.OWNER.TreeHouseStage, 0, null);
			}
		}
		this.guiData.machineAllBadge.SetActive(DataManager.DmHome.GetHomeCheckResult().GetTreeHouseMachineFntrTimeList().FindAll((MasterRoomMachineDataModel x) => x.nextsecond == 0)
			.Count > 0);
	}

	public override bool OnEnableSceneWait()
	{
		if (DataManager.IsServerRequesting())
		{
			return false;
		}
		if (!AssetManager.IsLoadFinishAssetData(SceneTreeHouse.BOARD_PATH) || !AssetManager.IsLoadFinishAssetData(SceneTreeHouse.CURTAIN_BOARD_PATH) || !AssetManager.IsLoadFinishAssetData(SceneTreeHouse.GRID_DEPEND_PATH))
		{
			return false;
		}
		if (this.curtainBoard == null)
		{
			this.curtainBoard = new List<GameObject>();
			foreach (Transform transform in this.curtainLoc)
			{
				GameObject gameObject = AssetManager.InstantiateAssetData(SceneTreeHouse.CURTAIN_BOARD_PATH, null);
				gameObject.transform.SetParent(this.field.transform, false);
				gameObject.SetLayerRecursively(this.stageLocator.layer);
				gameObject.AddComponent<MeshCollider>();
				gameObject.transform.position = transform.position;
				gameObject.transform.eulerAngles = new Vector3(0f, this.Round30(transform.eulerAngles.y), 0f);
				gameObject.SetActive(false);
				this.curtainBoard.Add(gameObject);
			}
			AssetManager.AddLoadList(SceneTreeHouse.CURTAIN_BOARD_PATH, AssetManager.OWNER.TreeHouseStage);
			this.doorBase = new GameObject("Door");
			this.doorBase.transform.SetParent(this.furnitureBase.transform, false);
			this.doorBase.transform.position = SceneTreeHouse.DOOR_BATH_POS;
			this.doorBase.transform.eulerAngles = new Vector3(0f, 180f, 0f);
			this.doorBase.SetActive(true);
			for (int j = 0; j < SceneTreeHouse.DOOR_GRID_POS_AND_SIZE.GetLength(0); j++)
			{
				Transform transform2 = new GameObject("poster_" + j.ToString()).transform;
				transform2.transform.SetParent(this.doorBase.transform, false);
				transform2.transform.localPosition = SceneTreeHouse.DOOR_GRID_POS_AND_SIZE[j, 0];
				transform2.gameObject.SetActive(true);
				GameObject gameObject2 = AssetManager.InstantiateAssetData(SceneTreeHouse.GRID_DEPEND_PATH, null);
				gameObject2.name = transform2.name;
				gameObject2.transform.SetParent(transform2, false);
				gameObject2.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
				gameObject2.SetLayerRecursively(this.stageLocator.layer);
				gameObject2.SetActive(false);
				this.doorGridMap.Add(gameObject2, SceneTreeHouse.DOOR_GRID_POS_AND_SIZE[j, 1]);
			}
		}
		bool flag = false;
		if (this.furnitureList == null)
		{
			SceneTreeHouse.<>c__DisplayClass263_0 CS$<>8__locals1 = new SceneTreeHouse.<>c__DisplayClass263_0();
			CS$<>8__locals1.<>4__this = this;
			this.bgmList = new List<int>();
			this.bgmList.Add(0);
			CS$<>8__locals1.lst = new List<string> { SceneTreeHouse.TREE_HOUSE_BGM };
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			foreach (SceneTreeHouse.FurnitureData furnitureData in this.allFurnitureDataList)
			{
				int id = furnitureData.dat.GetId();
				TreeHouseFurniturePackData treeHouseFurniturePackData = DataManager.DmTreeHouse.UserFurniturePackList.Find((TreeHouseFurniturePackData itm) => itm.id == id);
				furnitureData.num = ((treeHouseFurniturePackData == null) ? 0 : treeHouseFurniturePackData.num);
				if (!string.IsNullOrEmpty(furnitureData.dat.bgmFilepath))
				{
					string fileName = Path.GetFileName(furnitureData.dat.bgmFilepath);
					if (furnitureData.num > 0)
					{
						if (!CS$<>8__locals1.lst.Contains(fileName))
						{
							this.bgmList.Add(id);
							CS$<>8__locals1.lst.Add(fileName);
						}
					}
					else if (!dictionary.ContainsKey(fileName))
					{
						dictionary.Add(fileName, -id);
					}
				}
			}
			CS$<>8__locals1.lst = new List<string>(dictionary.Keys).FindAll((string itm) => CS$<>8__locals1.lst.Contains(itm));
			foreach (string text in CS$<>8__locals1.lst)
			{
				dictionary.Remove(text);
			}
			this.bgmList.AddRange(dictionary.Values);
			this.bgmLineup = new List<Transform>();
			using (List<int>.Enumerator enumerator4 = this.bgmList.GetEnumerator())
			{
				while (enumerator4.MoveNext())
				{
					SceneTreeHouse.<>c__DisplayClass263_2 CS$<>8__locals3 = new SceneTreeHouse.<>c__DisplayClass263_2();
					CS$<>8__locals3.CS$<>8__locals1 = CS$<>8__locals1;
					CS$<>8__locals3.bgm = enumerator4.Current;
					SceneTreeHouse.FurnitureData furnitureData2 = this.allFurnitureDataList.Find((SceneTreeHouse.FurnitureData itm) => itm.dat.GetId() == Mathf.Abs(CS$<>8__locals3.bgm));
					GameObject obj2 = Object.Instantiate<GameObject>(this.bgmPanel);
					obj2.transform.SetParent(this.hidePanel, false);
					obj2.name = CS$<>8__locals3.bgm.ToString();
					obj2.transform.Find("Base/Txt_BGMTitle").GetComponent<PguiTextCtrl>().text = ((furnitureData2 == null) ? "わいわいツリーハウス" : furnitureData2.dat.bgmName);
					obj2.transform.Find("Base/Disable").gameObject.SetActive(CS$<>8__locals3.bgm < 0);
					obj2.transform.Find("Base/Disable/Txt_Clear").GetComponent<PguiTextCtrl>().text = ((furnitureData2 == null) ? "わいわいツリーハウス" : furnitureData2.dat.GetName()) + "\n獲得で解放";
					obj2.transform.Find("Base/Playing").gameObject.SetActive(false);
					obj2.transform.Find("Base/TestPlaying").gameObject.SetActive(false);
					if (obj2.GetComponent<PguiTouchTrigger>() == null)
					{
						obj2.AddComponent<PguiTouchTrigger>().AddListener(delegate
						{
							CS$<>8__locals3.CS$<>8__locals1.<>4__this.OnClickBgmLineup(obj2);
						}, null, null, null, null);
					}
					this.bgmLineup.Add(obj2.transform);
				}
			}
			this.winMusic.scroll.Resize((this.bgmList.Count + 3 - 1) / 3, 0);
			this.furnitureList = new List<SceneTreeHouse.FurnitureCtrl>();
			this.playBgm = this.ChangeBgm(this.SetFurniture(false));
			if ((PlayerPrefs.HasKey(SceneTreeHouse.TREE_HOUSE_KEY) ? PlayerPrefs.GetInt(SceneTreeHouse.TREE_HOUSE_KEY) : 0) == DataManager.DmUserInfo.friendId)
			{
				foreach (SceneTreeHouse.FurnitureCtrl furnitureCtrl in this.furnitureList)
				{
					if (furnitureCtrl.machinePopup == null)
					{
						furnitureCtrl.onoff = false;
					}
				}
			}
			PlayerPrefs.SetInt(SceneTreeHouse.TREE_HOUSE_KEY, DataManager.DmUserInfo.friendId);
			PlayerPrefs.Save();
		}
		else
		{
			this.CtrlFurniture(false);
			foreach (SceneTreeHouse.FurnitureCtrl furnitureCtrl2 in this.furnitureList)
			{
				if (furnitureCtrl2.mdl == null || furnitureCtrl2.chgmdl)
				{
					flag = true;
				}
			}
		}
		if (this.paperList == null)
		{
			this.paperList = new List<SceneTreeHouse.PaperCtrl>();
			this.paperList.Add(new SceneTreeHouse.PaperCtrl
			{
				cat = TreeHouseFurnitureStatic.Category.WALL_PAPER,
				mdlnam = "captainroom_obj_ceiling_wall_000000_a_mdl",
				enable = false,
				path = null,
				mdl = null,
				mesh = null,
				chgmdl = false
			});
			this.paperList.Add(new SceneTreeHouse.PaperCtrl
			{
				cat = TreeHouseFurnitureStatic.Category.FLOOR_PAPER,
				mdlnam = "captainroom_obj_ceiling_floor_000000_a_mdl",
				enable = false,
				path = null,
				mdl = null,
				mesh = null,
				chgmdl = false
			});
			this.paperList.Add(new SceneTreeHouse.PaperCtrl
			{
				cat = TreeHouseFurnitureStatic.Category.CEIL_DECO,
				mdlnam = "captainroom_obj_ceiling_attic_000000_a_mdl",
				enable = false,
				path = null,
				mdl = null,
				mesh = null,
				chgmdl = false
			});
		}
		else
		{
			this.CtrlPaper();
			foreach (SceneTreeHouse.PaperCtrl paperCtrl in this.paperList)
			{
				if (paperCtrl.enable && (paperCtrl.mdl == null || paperCtrl.chgmdl))
				{
					flag = true;
				}
			}
		}
		if (this.charaList == null)
		{
			this.charaList = new List<SceneTreeHouse.CharaCtrl>();
			this.SetChara(false);
		}
		else
		{
			this.CtrlChara(false);
			foreach (SceneTreeHouse.CharaCtrl charaCtrl in this.charaList)
			{
				if (charaCtrl.hdl != null && !charaCtrl.hdl.IsFinishInitialize())
				{
					flag = true;
				}
			}
		}
		if (!AssetManager.IsLoadFinishAssetData(SceneTreeHouse.GRID_FLOOR_PATH) || !AssetManager.IsLoadFinishAssetData(SceneTreeHouse.GRID_WALL_PATH))
		{
			flag = true;
		}
		if (this.stageLoad != null)
		{
			if (this.stageLoad.MoveNext())
			{
				flag = true;
			}
			else
			{
				this.stageLoad = null;
			}
		}
		if (flag)
		{
			return false;
		}
		this.favoriteFuniture = new HashSet<int>(DataManager.DmTreeHouse.FavoriteFurnitureItemIdList);
		this.floorGrid = AssetManager.InstantiateAssetData(SceneTreeHouse.GRID_FLOOR_PATH, null);
		this.floorGrid.transform.SetParent(this.field.transform, false);
		this.floorGrid.SetLayerRecursively(this.stageLocator.layer);
		this.floorGrid.AddComponent<MeshCollider>();
		this.wallGrid = AssetManager.InstantiateAssetData(SceneTreeHouse.GRID_WALL_PATH, null);
		this.wallGrid.transform.SetParent(this.field.transform, false);
		this.wallGrid.SetLayerRecursively(this.stageLocator.layer);
		this.wallGrid.AddComponent<MeshCollider>();
		AssetManager.AddLoadList(SceneTreeHouse.GRID_FLOOR_PATH, AssetManager.OWNER.TreeHouseStage);
		AssetManager.AddLoadList(SceneTreeHouse.GRID_WALL_PATH, AssetManager.OWNER.TreeHouseStage);
		this.floorGrid.SetActive(false);
		this.wallGrid.SetActive(false);
		this.touchView = false;
		this.moveView = Vector2.zero;
		this.pinchView = 0f;
		this.wheelView = 0f;
		SGNFW.Touch.Manager.RegisterStart(new SGNFW.Touch.Manager.SingleAction(this.OnTouchStart));
		SGNFW.Touch.Manager.RegisterStartTwo(new SGNFW.Touch.Manager.DoubleAction(this.OnTouchStart2));
		SGNFW.Touch.Manager.RegisterRelease(new SGNFW.Touch.Manager.SingleAction(this.OnTouchEnd));
		SGNFW.Touch.Manager.RegisterMove(new SGNFW.Touch.Manager.SingleAction(this.OnTouchMove));
		SGNFW.Touch.Manager.RegisterMoveTwo(new SGNFW.Touch.Manager.DoubleAction(this.OnTouchMove2));
		SGNFW.Touch.Manager.RegisterPinch(new SGNFW.Touch.Manager.DoubleAction(this.OnPinch));
		SGNFW.Touch.Manager.RegisterMouseWheel(new SGNFW.Touch.Manager.WheelAction(this.OnWheel));
		SGNFW.Touch.Manager.RegisterTap(new SGNFW.Touch.Manager.SingleAction(this.OnTap));
		SGNFW.Touch.Manager.RegisterDoubleTap(new SGNFW.Touch.Manager.SingleAction(this.OnDoubleTap));
		SGNFW.Touch.Manager.RegisterLongPress(new SGNFW.Touch.Manager.SingleAction(this.OnLongTap));
		this.singleTap = (this.doubleTap = false);
		this.gridRot = 0;
		this.floorGrid.transform.eulerAngles = Vector3.zero;
		this.movePos = new Vector3(0f, -999f, 0f);
		this.moveNrm = new Vector3(0f, 1f, 0f);
		this.moveRot = 0f;
		this.moveCurtain = null;
		int i;
		int i2;
		for (i = 0; i < this.guiData.charaIcon.Count; i = i2 + 1)
		{
			Transform transform3 = this.guiData.charaIcon[i];
			SceneTreeHouse.CharaCtrl charaCtrl2 = this.charaList.Find((SceneTreeHouse.CharaCtrl itm) => itm.no == i + 1);
			IconCharaCtrl component = transform3.Find("Icon_Chara_TreeHouse/Icon_Chara").GetComponent<IconCharaCtrl>();
			component.Setup((charaCtrl2 == null) ? null : charaCtrl2.chara, SortFilterDefine.SortType.LEVEL, false, null, 0, -1, 0);
			component.DispPhotoPocketLevel(true);
			transform3.Find("Icon_Chara_TreeHouse/Icon_CharaSet/Remove").gameObject.SetActive(false);
			transform3.Find("Icon_Chara_TreeHouse/Icon_CharaSet/Fnt_Selected").gameObject.SetActive(false);
			transform3.Find("Icon_Chara_TreeHouse/Icon_CharaSet/Txt_Disable").gameObject.SetActive(false);
			transform3.Find("Icon_Chara_TreeHouse/Icon_CharaSet/Current").gameObject.SetActive(false);
			transform3.Find("Icon_Chara_TreeHouse/Icon_CharaSet/AEImage_Eff_Change").GetComponent<PguiAECtrl>().PauseAnimation(PguiAECtrl.AmimeType.START, null);
			transform3.Find("Img_Blank/Txt_Touch").GetComponent<SimpleAnimation>().ExPlayAnimation(SimpleAnimation.ExPguiStatus.LOOP, null);
			i2 = i;
		}
		this.treehouseChara = new List<Transform>();
		GameObject obj = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneTreeHouse/GUI/Prefab/Icon_Chara_TreeHouse"));
		obj.transform.SetParent(this.hidePanel, false);
		obj.name = "0";
		Object.Destroy(obj.transform.Find("Icon_Chara").gameObject);
		obj.transform.Find("Icon_CharaSet/Fnt_Selected").gameObject.SetActive(false);
		obj.transform.Find("Icon_CharaSet/Txt_Disable").gameObject.SetActive(false);
		obj.transform.Find("Icon_CharaSet/Current").gameObject.SetActive(false);
		obj.transform.Find("Icon_CharaSet/AEImage_Eff_Change").GetComponent<PguiAECtrl>().PauseAnimation(PguiAECtrl.AmimeType.START, null);
		obj.AddComponent<PguiCollider>();
		obj.AddComponent<PguiTouchTrigger>().AddListener(delegate
		{
			this.OnClickCharaList(obj.transform);
		}, null, null, null, null);
		this.treehouseChara.Add(obj.transform);
		using (List<CharaPackData>.Enumerator enumerator8 = this.dispCharaPackList.GetEnumerator())
		{
			while (enumerator8.MoveNext())
			{
				SceneTreeHouse.<>c__DisplayClass263_6 CS$<>8__locals7 = new SceneTreeHouse.<>c__DisplayClass263_6();
				CS$<>8__locals7.<>4__this = this;
				CS$<>8__locals7.cpd = enumerator8.Current;
				GameObject obj3 = Object.Instantiate<GameObject>((GameObject)Resources.Load("SceneTreeHouse/GUI/Prefab/Icon_Chara_TreeHouse"));
				obj3.transform.SetParent(this.hidePanel, false);
				obj3.name = CS$<>8__locals7.cpd.id.ToString();
				HashSet<int> cid = DataManager.DmChara.GetSameCharaList(CS$<>8__locals7.cpd.id, false);
				bool flag2 = this.charaList.Find((SceneTreeHouse.CharaCtrl itm) => itm.chara != null && itm.chara.id == CS$<>8__locals7.cpd.id) != null;
				bool flag3 = this.charaList.Find((SceneTreeHouse.CharaCtrl itm) => itm.chara != null && cid.Contains(itm.chara.id)) != null;
				obj3.transform.Find("Icon_Chara").GetComponent<IconCharaCtrl>().Setup(CS$<>8__locals7.cpd, this.charaSortType, flag2 || flag3, null, 0, -1, 0);
				obj3.transform.Find("Icon_Chara").GetComponent<IconCharaCtrl>().DispPhotoPocketLevel(true);
				obj3.transform.Find("Icon_CharaSet/Remove").gameObject.SetActive(false);
				obj3.transform.Find("Icon_CharaSet/Fnt_Selected").gameObject.SetActive(flag2);
				obj3.transform.Find("Icon_CharaSet/Txt_Disable").gameObject.SetActive(!flag2 && flag3);
				obj3.transform.Find("Icon_CharaSet/Txt_Disable").GetComponent<PguiTextCtrl>().text = "選択不可";
				obj3.transform.Find("Icon_CharaSet/Current").gameObject.SetActive(false);
				obj3.transform.Find("Icon_CharaSet/AEImage_Eff_Change").GetComponent<PguiAECtrl>().PauseAnimation(PguiAECtrl.AmimeType.START, null);
				obj3.AddComponent<PguiCollider>();
				obj3.AddComponent<PguiTouchTrigger>().AddListener(delegate
				{
					CS$<>8__locals7.<>4__this.OnClickCharaList(obj3.transform);
				}, null, null, null, null);
				this.treehouseChara.Add(obj3.transform);
			}
		}
		SortWindowCtrl.RegisterData registerData = new SortWindowCtrl.RegisterData
		{
			register = SortFilterDefine.RegisterType.TREEHOUSE_CHANGE,
			filterButton = this.guiData.charaSelect.Find("TopBtns/Btn_FilterOnOff").GetComponent<PguiButtonCtrl>(),
			sortButton = this.guiData.charaSelect.Find("TopBtns/Btn_Sort").GetComponent<PguiButtonCtrl>(),
			sortUdButton = this.guiData.charaSelect.Find("TopBtns/Btn_SortUpDown").GetComponent<PguiButtonCtrl>(),
			funcGetTargetBaseList = () => new SortWindowCtrl.SortTarget
			{
				charaList = this.haveCharaPackList
			},
			funcDisideTarget = delegate(SortWindowCtrl.SortTarget item)
			{
				this.dispCharaPackList = item.charaList;
				this.charaSortType = item.sortType;
				this.guiData.charaScroll.Resize((this.dispCharaPackList.Count + 1 + SceneTreeHouse.ScrollDeckNum - 1) / SceneTreeHouse.ScrollDeckNum, 0);
			}
		};
		CanvasManager.HdlOpenWindowSortFilter.Register(registerData, true, null);
		this.changeList = new Dictionary<int, int>();
		this.chgChr = 0;
		this.socialInfo = false;
		this.visitTabTyp = (this.socialTabTyp = TreeHouseSocialTabType.INVALID);
		foreach (TreeHouseSocialTabType treeHouseSocialTabType in new List<TreeHouseSocialTabType>(this.socialIndex.Keys))
		{
			this.socialIndex[treeHouseSocialTabType] = 0;
		}
		this.socialUserList = null;
		this.socialUserIdx = 0;
		this.socialVisitList = new List<int>();
		this.SetMonthlyPackInfo();
		DataManagerGameStatus.UserFlagData userFlagData = DataManager.DmGameStatus.MakeUserFlagData();
		this.ienum = new List<IEnumerator> { (userFlagData.TutorialFinishFlag.TreeHouseFirst == DataManagerGameStatus.UserFlagData.TREE_HOUSE_TUTORIAL.LATEST && this.batteryInfo != null) ? this.GetStamp(true) : this.PlayTutorial() };
		return true;
	}

	private int SetFurniture(bool other)
	{
		List<SceneTreeHouse.FurnitureCtrl> list = (other ? this.otherFurniture : this.furnitureList);
		List<TreeHouseFurnitureMapping> list2 = (other ? DataManager.DmTreeHouse.SocialVisitUserData.FurnitureMappingList : DataManager.DmTreeHouse.FurnitureMappingList);
		if (list2 == null)
		{
			list2 = new List<TreeHouseFurnitureMapping>();
		}
		int num = 0;
		int num2 = 0;
		using (List<TreeHouseFurnitureMapping>.Enumerator enumerator = list2.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				TreeHouseFurnitureMapping thfm = enumerator.Current;
				SceneTreeHouse.FurnitureData furnitureData = this.allFurnitureDataList.Find((SceneTreeHouse.FurnitureData itm) => itm.dat.GetId() == thfm.furnitureId);
				if (furnitureData != null)
				{
					if (thfm.placementId <= 0)
					{
						num = furnitureData.dat.GetId();
					}
					else
					{
						Vector3 vector = new Vector3((float)thfm.postion.x / 1000f, (float)thfm.postion.y / 1000f, (float)thfm.postion.z / 1000f);
						float num3 = this.Round30((float)thfm.angle);
						int num4 = 0;
						bool flag = thfm.effectFlag;
						if (furnitureData.dat.category == TreeHouseFurnitureStatic.Category.RUG)
						{
							vector.y = 0f;
							num4 = thfm.postion.y;
						}
						else if (furnitureData.dat.category == TreeHouseFurnitureStatic.Category.CEIL_LIGHT)
						{
							vector = this.lightLoc.position;
							num3 = 0f;
						}
						else if (furnitureData.dat.category == TreeHouseFurnitureStatic.Category.WALL_PAPER || furnitureData.dat.category == TreeHouseFurnitureStatic.Category.FLOOR_PAPER || furnitureData.dat.category == TreeHouseFurnitureStatic.Category.CEIL_DECO)
						{
							vector = Vector3.zero;
							num3 = 0f;
							flag = false;
						}
						GameObject gameObject = this.MakeFurniture(thfm.placementId, furnitureData.dat.GetId(), vector, num3, other);
						list.Add(new SceneTreeHouse.FurnitureCtrl(thfm.placementId, furnitureData, vector, num3, gameObject, flag)
						{
							rug = num4
						});
						if (num2 < thfm.placementId)
						{
							num2 = thfm.placementId;
						}
					}
				}
			}
		}
		if (!other)
		{
			this.furniturePlace = num2;
		}
		foreach (SceneTreeHouse.FurnitureCtrl furnitureCtrl in list)
		{
			if (furnitureCtrl.data.dat.category == TreeHouseFurnitureStatic.Category.GENERAL_MERCHANDISE)
			{
				float num5 = -1f;
				using (List<SceneTreeHouse.FurnitureCtrl>.Enumerator enumerator3 = list.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						SceneTreeHouse.FurnitureCtrl furnitureCtrl2 = enumerator3.Current;
						if (furnitureCtrl2 != furnitureCtrl && num5 <= furnitureCtrl2.obj.transform.position.y)
						{
							if (furnitureCtrl2.data.dat.category == TreeHouseFurnitureStatic.Category.STAND)
							{
								if (this.ChkFloor(furnitureCtrl.obj.transform.position, furnitureCtrl2.corner))
								{
									num5 = furnitureCtrl2.obj.transform.position.y;
									furnitureCtrl.depend = furnitureCtrl2.obj;
								}
							}
							else if (furnitureCtrl2.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_HANGINGS && furnitureCtrl2.data.siz.z > 0)
							{
								List<Vector3> list3 = new List<Vector3>
								{
									furnitureCtrl2.corner[3],
									furnitureCtrl2.corner[2],
									furnitureCtrl2.corner[2] + furnitureCtrl2.walloff,
									furnitureCtrl2.corner[3] + furnitureCtrl2.walloff
								};
								if (furnitureCtrl.pos.y >= list3[0].y && this.ChkFloor(furnitureCtrl.obj.transform.position, list3))
								{
									num5 = furnitureCtrl2.obj.transform.position.y;
									furnitureCtrl.depend = furnitureCtrl2.obj;
								}
							}
						}
					}
					goto IL_066A;
				}
			}
			if (furnitureCtrl.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_HANGINGS && furnitureCtrl.data.siz.z < 0)
			{
				foreach (GameObject gameObject2 in this.doorGridMap.Keys)
				{
					if (Vector3.Distance(gameObject2.transform.parent.position, furnitureCtrl.obj.transform.position) < 0.1f)
					{
						furnitureCtrl.depend = gameObject2;
						break;
					}
				}
				if (furnitureCtrl.depend != null)
				{
					continue;
				}
				foreach (SceneTreeHouse.FurnitureCtrl furnitureCtrl3 in list)
				{
					if (furnitureCtrl3 != furnitureCtrl)
					{
						if (furnitureCtrl3.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_HANGINGS)
						{
							Vector3 vector2 = furnitureCtrl.obj.transform.TransformPoint(new Vector3(0f, 0f, -0.05f));
							List<Vector3> list4 = new List<Vector3>
							{
								furnitureCtrl3.corner[3],
								furnitureCtrl3.corner[2],
								furnitureCtrl3.corner[2] + furnitureCtrl3.walloff,
								furnitureCtrl3.corner[3] + furnitureCtrl3.walloff
							};
							if (this.ChkWall(furnitureCtrl.obj.transform.position, furnitureCtrl3.corner) && this.ChkFloor(vector2, list4))
							{
								furnitureCtrl.depend = furnitureCtrl3.obj;
								break;
							}
						}
						else if (furnitureCtrl3.data.dat.category == TreeHouseFurnitureStatic.Category.LARGE_FURNITURE || furnitureCtrl3.data.dat.category == TreeHouseFurnitureStatic.Category.STAND)
						{
							Vector3 vector3 = furnitureCtrl.obj.transform.TransformPoint(new Vector3(0f, 0f, 0.05f));
							if (this.ChkFloor(vector3, furnitureCtrl3.corner))
							{
								furnitureCtrl.depend = furnitureCtrl3.obj;
								break;
							}
						}
					}
				}
			}
			IL_066A:
			if (!other && furnitureCtrl.data.dat.machineId > 0)
			{
				this.AddMachineUI(furnitureCtrl);
			}
		}
		this.CalcMachineUI();
		list.Sort((SceneTreeHouse.FurnitureCtrl a, SceneTreeHouse.FurnitureCtrl b) => b.rug - a.rug);
		this.ChkRug(other);
		if (!other)
		{
			this.furnitureMapSave = this.MakeFurnitureMap();
		}
		return num;
	}

	private List<Vector3> CalcFloorCorner(Transform trs, SceneTreeHouse.FurnitureData fd)
	{
		float num = (float)fd.siz.x * 0.25f - 0.05f;
		float num2 = (float)fd.siz.z * 0.25f - 0.05f;
		return new List<Vector3>
		{
			trs.TransformPoint(new Vector3(num, 0f, num2)),
			trs.TransformPoint(new Vector3(-num, 0f, num2)),
			trs.TransformPoint(new Vector3(-num, 0f, -num2)),
			trs.TransformPoint(new Vector3(num, 0f, -num2))
		};
	}

	private void ChkRug(bool other)
	{
		List<SceneTreeHouse.FurnitureCtrl> list = new List<SceneTreeHouse.FurnitureCtrl>((other ? this.otherFurniture : this.furnitureList).FindAll((SceneTreeHouse.FurnitureCtrl itm) => itm.data.dat.category == TreeHouseFurnitureStatic.Category.RUG));
		list.Remove(this.furnitureMove);
		list.RemoveAll((SceneTreeHouse.FurnitureCtrl itm) => itm.no <= 0);
		for (int i = list.Count - 1; i >= 0; i--)
		{
			list[i].rug = 0;
			Transform transform = list[i].obj.transform;
			List<Vector3> list2 = this.CalcFloorCorner(transform, list[i].data);
			for (int j = list.Count - 1; j > i; j--)
			{
				List<Vector3> corner = list[j].corner;
				bool flag = false;
				if (this.ChkFloor(transform.position, corner) || this.ChkFloor(list2[0], corner) || this.ChkFloor(list2[1], corner) || this.ChkFloor(list2[2], corner) || this.ChkFloor(list2[3], corner))
				{
					flag = true;
				}
				else if (this.ChkFloor(list[j].obj.transform.position, list2) || this.ChkFloor(corner[0], list2) || this.ChkFloor(corner[1], list2) || this.ChkFloor(corner[2], list2) || this.ChkFloor(corner[3], list2))
				{
					flag = true;
				}
				if (flag)
				{
					int num = list[j].rug + 1;
					if (list[i].rug < num)
					{
						list[i].rug = num;
					}
				}
			}
		}
	}

	private void SetChara(bool other)
	{
		List<SceneTreeHouse.CharaCtrl> list = (other ? this.otherChara : this.charaList);
		List<TreeHousePutCharaData> list2 = (other ? DataManager.DmTreeHouse.SocialVisitUserData.PutCharaDataList : DataManager.DmTreeHouse.PutCharaDataList);
		if (list2 == null)
		{
			list2 = new List<TreeHousePutCharaData>();
		}
		using (List<TreeHousePutCharaData>.Enumerator enumerator = list2.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				TreeHousePutCharaData tpcd = enumerator.Current;
				if (tpcd != null && tpcd.charaId > 0)
				{
					CharaPackData charaPackData;
					if (other)
					{
						charaPackData = CharaPackData.MakeInitial(tpcd.charaId);
						charaPackData.dynamicData.equipClothesId = tpcd.clothId;
						charaPackData.dynamicData.accessory = ((tpcd.accessoryId == 0) ? null : new DataManagerCharaAccessory.Accessory(new Accessory
						{
							item_id = tpcd.accessoryId
						}));
						charaPackData.dynamicData.dispAccessoryEffect = charaPackData.dynamicData.accessory != null && charaPackData.dynamicData.accessory.AccessoryData != null;
					}
					else if ((charaPackData = this.haveCharaPackList.Find((CharaPackData itm) => itm.id == tpcd.charaId)) == null)
					{
						continue;
					}
					list.Add(new SceneTreeHouse.CharaCtrl(tpcd.indexId, charaPackData));
				}
			}
		}
	}

	private void SetMonthlyPackInfo()
	{
		this.monthlyDay = TimeManager.Now.Day;
		DataManagerMonthlyPack.PurchaseMonthlypackData mpd = DataManager.DmMonthlyPack.GetValidMonthlyPackData();
		int num = DataManager.DmTreeHouse.KizunaBonusData.GetKizunaBonusRatio(mpd != null);
		this.kizunaRatio = (num / 100).ToString();
		num %= 100;
		if (num > 0)
		{
			this.kizunaRatio = this.kizunaRatio + "." + (num / 10).ToString();
			num %= 10;
			if (num > 0)
			{
				this.kizunaRatio += num.ToString();
			}
		}
		foreach (Transform transform in this.guiData.charaIcon)
		{
			transform.Find("Campaign_Pass").gameObject.SetActive(mpd != null);
			transform.Find("Num_Pt").GetComponent<PguiTextCtrl>().ReplaceTextByDefault("Param01", this.kizunaRatio);
		}
		int num2 = ((mpd == null) ? 0 : DataManager.DmMonthlyPack.purchaseMonthlypackDataList.Find((DataManagerMonthlyPack.PurchaseMonthlypackData itm) => itm.PackId == mpd.PackId).TreeHouseAddMysetNum);
		for (int i = 1; i < this.winMyset.list.Count; i++)
		{
			SceneTreeHouse.BAR_MYSET bar_MYSET = this.winMyset.list[i];
			int num3 = i - 1;
			bar_MYSET.disable.SetActive(num3 > num2);
			bar_MYSET.save.SetActEnable(!bar_MYSET.disable.activeSelf, false, false);
			bar_MYSET.load.SetActEnable(!bar_MYSET.disable.activeSelf && DataManager.DmTreeHouse.MysetList.Count >= i && DataManager.DmTreeHouse.MysetList[i - 1].isDataEnable, false, false);
		}
	}

	private void ResetGyro()
	{
		this.guiData.btnGyro.gameObject.SetActive(SceneTreeHouse.gyroMode >= 0);
		this.guiData.btnGyro.transform.Find("BaseImage/On").gameObject.SetActive(SceneTreeHouse.gyroMode > 0);
		this.guiData.btnGyro.transform.Find("BaseImage/Off").gameObject.SetActive(SceneTreeHouse.gyroMode <= 0);
		this.guiData.btnVR.gameObject.SetActive(SceneTreeHouse.gyroMode >= 0);
		this.guiOther.btnGyro.gameObject.SetActive(SceneTreeHouse.gyroMode >= 0);
		this.guiOther.btnGyro.transform.Find("BaseImage/On").gameObject.SetActive(SceneTreeHouse.gyroMode > 0);
		this.guiOther.btnGyro.transform.Find("BaseImage/Off").gameObject.SetActive(SceneTreeHouse.gyroMode <= 0);
		this.guiOther.btnVR.gameObject.SetActive(SceneTreeHouse.gyroMode >= 0);
		this.gyroBase = ((SceneTreeHouse.gyroMode > 0) ? Input.gyro.attitude : Quaternion.identity);
		this.gyroAng = Vector2.zero;
	}

	private void NextGyrocam()
	{
		SceneTreeHouse.FurnitureCtrl furnitureCtrl = this.gyroLst.Find((SceneTreeHouse.FurnitureCtrl itm) => itm.cam.Contains(this.gyroCam));
		if (furnitureCtrl != null)
		{
			int num = furnitureCtrl.cam.IndexOf(this.gyroCam);
			if (++num < furnitureCtrl.cam.Count)
			{
				this.gyroCam = furnitureCtrl.cam[num];
				return;
			}
			num = this.gyroLst.IndexOf(furnitureCtrl);
			if (++num >= this.gyroLst.Count)
			{
				num = 0;
			}
			this.gyroCam = this.gyroLst[num].cam[0];
		}
	}

	public override void OnStartSceneFadeWait()
	{
		this.CtrlPaper();
		this.CtrlFurniture(false);
		this.CtrlAction(false);
		this.CtrlChara(false);
	}

	public override void OnStartControl()
	{
		this.guiData.anime.ExResumeAnimation(null);
		this.categorySel = TreeHouseFurnitureStatic.Category.LARGE_FURNITURE;
		this.guiData.categoryScroll.Resize(DataManagerTreeHouse.categoryList.Keys.Count, 0);
		this.MakefurnitureDataDisp();
	}

	public override void Update()
	{
		if (this.ienum.Count <= 0 && !this.winPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
		{
			if (!this.camera.gameObject.activeSelf)
			{
				this.camL.gameObject.SetActive(false);
				this.camR.gameObject.SetActive(false);
				this.camera.gameObject.SetActive(true);
				if (this.guiOther.baseObj.activeSelf)
				{
					this.Hide2OtherCamera();
				}
				else
				{
					this.Hide2Camera();
				}
			}
			else if (this.IsOtherHide())
			{
				this.Hide2Other();
			}
			else if (this.IsOtherCameraHide())
			{
				this.Hide2OtherCamera();
			}
			else if (this.IsOtherCamera())
			{
				this.Camera2Other();
			}
			else if (this.IsOther() || this.IsOtherInfo())
			{
				this.OnClickEndVisit();
			}
			else if (this.IsHide())
			{
				this.Hide2Top();
			}
			else if (this.IsCameraHide())
			{
				this.Hide2Camera();
			}
			else if (this.IsCamera())
			{
				this.Camera2Top();
			}
			else if (this.IsEdit())
			{
				this.Edit2Top();
			}
			else if (this.IsFurniture())
			{
				this.Furniture2Edit();
			}
			else if (this.IsPlacement())
			{
				this.CancelFurniture();
			}
		}
		if (this.winPanel.activeSelf && this.winMyset.win.FinishedClose() && this.winGetstamp.win.FinishedClose() && this.winFurniture.win.FinishedClose() && this.winFilter.win.FinishedClose() && this.winSort.win.FinishedClose() && this.winSocial.win.FinishedClose() && this.winOpenconf.win.FinishedClose() && this.winName.win.FinishedClose() && this.winComment.win.FinishedClose() && this.winMusic.win.FinishedClose() && this.winCharge.win.FinishedClose() && this.winChargeUse.win.FinishedClose() && this.winChargeGet.win.FinishedClose() && this.winMachine.win.FinishedClose() && this.winMachineGet.win.FinishedClose() && this.winMachineAll.win.FinishedClose() && this.winVR.win.FinishedClose())
		{
			this.winPanel.SetActive(false);
		}
		this.ienum.RemoveAll((IEnumerator itm) => !itm.MoveNext());
		if (this.stageLoad == null)
		{
			if (this.ienum.Count <= 0)
			{
				SceneHome.StageType stageType = SceneHome.GetStageType();
				if (this.stageType != stageType)
				{
					this.stageType = stageType;
					this.stageLoad = this.StageLoad(false);
				}
				else
				{
					this.ChangeStageLight();
				}
			}
		}
		else if (!this.stageLoad.MoveNext())
		{
			this.stageLoad = null;
		}
		CanvasManager.HdlCmnMenu.UpdateMenu(true, true);
		this.moveView *= this.camera.fieldOfView / 750f;
		if (this.gyroCam == null)
		{
			SceneTreeHouse.CamDat camDat = null;
			if (this.IsTop() || this.IsHide())
			{
				camDat = this.camView[this.camViewNo];
			}
			else if (this.IsEdit() || this.IsPlacement())
			{
				camDat = this.camEdit[this.camEditNo];
			}
			else if ((this.IsOther() || this.IsOtherInfo() || this.IsOtherHide()) && this.camOtherNo >= 0)
			{
				camDat = this.camOther[this.camOtherNo];
			}
			if (camDat != null)
			{
				camDat.ang = new Vector2(Mathf.Clamp(camDat.ang.x + this.moveView.y, -60f, 60f), Mathf.Clamp(camDat.ang.y - this.moveView.x, -60f, 60f));
				camDat.fov = Mathf.Clamp(camDat.fov - this.pinchView - this.wheelView, -25f, 0f);
				this.camera.transform.position = camDat.bas.position;
				this.camera.transform.eulerAngles = new Vector3(camDat.bas.localScale.x + camDat.ang.x, camDat.bas.localScale.y + camDat.ang.y, 0f);
				this.camera.fieldOfView = camDat.bas.localScale.z + camDat.fov;
			}
		}
		else
		{
			this.gyroAng = new Vector2(Mathf.Clamp(this.gyroAng.x + this.moveView.y, -60f, 60f), Mathf.Clamp(this.gyroAng.y - this.moveView.x, -60f, 60f));
			this.gyroFov = Mathf.Clamp(this.gyroFov - this.pinchView - this.wheelView, -20f, 20f);
			Quaternion quaternion = ((SceneTreeHouse.gyroMode > 0) ? Input.gyro.attitude : Quaternion.Euler(-this.gyroAng.x, -this.gyroAng.y, 0f));
			this.camera.transform.position = this.gyroCam.position;
			this.camera.transform.rotation = this.gyroCam.rotation * Quaternion.Euler(0f, 0f, -180f) * (Quaternion.Inverse(this.gyroBase) * quaternion) * Quaternion.Euler(0f, 0f, 180f);
			this.camera.fieldOfView = 40f + this.gyroFov;
			this.camL.transform.position = this.camera.transform.TransformPoint(-this.gyroSpace, 0f, 0f);
			this.camL.transform.rotation = this.camera.transform.rotation;
			this.camL.fieldOfView = this.camera.fieldOfView;
			this.camR.transform.position = this.camera.transform.TransformPoint(this.gyroSpace, 0f, 0f);
			this.camR.transform.rotation = this.camera.transform.rotation;
			this.camR.fieldOfView = this.camera.fieldOfView;
		}
		this.moveView = Vector2.zero;
		this.pinchView = 0f;
		this.wheelView = 0f;
		if (this.monthlyDay != TimeManager.Now.Day)
		{
			this.SetMonthlyPackInfo();
		}
		this.PlacementFurniture();
		this.CtrlPaper();
		this.CtrlFurniture(false);
		this.CtrlAction(false);
		this.CtrlChara(false);
		this.CtrlFurniture(true);
		this.CtrlAction(true);
		this.CtrlChara(true);
		if (!this.IsPlacement() && ((this.guiData.tipsHand.activeSelf && this.guiData.chkTipsHand.activeSelf) || (this.guiData.tipsMouse.activeSelf && this.guiData.chkTipsMouse.activeSelf)))
		{
			this.guiData.tipsHand.SetActive(false);
			this.guiData.tipsMouse.SetActive(false);
			DataManagerGameStatus.UserFlagData userFlagData = DataManager.DmGameStatus.MakeUserFlagData();
			userFlagData.InformationsFlag.DisableTreeHouseTips = true;
			DataManager.DmGameStatus.RequestActionUpdateUserFlag(userFlagData);
		}
		if (!this.winPanel.activeSelf)
		{
			DataManagerGameStatus.UserFlagData userFlagData2 = DataManager.DmGameStatus.MakeUserFlagData();
			if (this.winMusic.imgChk.activeSelf == userFlagData2.InformationsFlag.DisableTreeHouseBgmChange)
			{
				userFlagData2.InformationsFlag.DisableTreeHouseBgmChange = !this.winMusic.imgChk.activeSelf;
				DataManager.DmGameStatus.RequestActionUpdateUserFlag(userFlagData2);
			}
		}
		if (this.testBgm > 0f && (this.testBgm -= TimeManager.DeltaTime) <= 0f)
		{
			this.ChangeBgm(this.playBgm);
		}
		if (this.removeMachineUI > 0f && (this.removeMachineUI -= TimeManager.DeltaTime) <= 0f)
		{
			this.CalcMachineUI();
		}
		if (this.winPanel.activeSelf)
		{
			if (!this.winCharge.win.FinishedClose() || !this.winChargeUse.win.FinishedClose() || !this.winChargeGet.win.FinishedClose())
			{
				this.batteryTim = ((DataManager.DmHome.GetHomeCheckResult() == null) ? 1 : DataManager.DmHome.GetHomeCheckResult().GetTreeHouseBatteryChargeTime());
				string text = string.Concat(new string[]
				{
					(this.batteryTim / 3600).ToString(),
					":",
					(this.batteryTim / 60 % 60).ToString("D2"),
					":",
					(this.batteryTim % 60).ToString("D2")
				});
				this.chargeTim = 0;
				using (List<UseItem>.Enumerator enumerator = this.chargeFuel.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						UseItem f = enumerator.Current;
						MstMasterRoomFuelData mstMasterRoomFuelData = this.fuelData.Find((MstMasterRoomFuelData itm) => itm.itemId == f.use_item_id);
						this.chargeTim += f.use_item_num * mstMasterRoomFuelData.addTime * 60;
					}
				}
				if (this.chargeTim > 0)
				{
					text = string.Concat(new string[]
					{
						text,
						"<size=18> (-",
						(this.chargeTim / 3600).ToString(),
						":",
						(this.chargeTim / 60 % 60).ToString("D2"),
						":",
						(this.chargeTim % 60).ToString("D2"),
						")</size>"
					});
				}
				this.winCharge.batteryGet.Find("Num_Time").GetComponent<PguiTextCtrl>().text = text;
				this.winChargeUse.batteryGet.Find("Num_Time").GetComponent<PguiTextCtrl>().text = text;
				float num = (float)((this.batteryData.getItemTime > 0) ? this.batteryData.getItemTime : 525600) * 60f;
				float num2 = 1f - Mathf.Clamp01((float)(this.batteryTim - this.chargeTim) / num);
				num = 1f - (float)this.batteryTim / num;
				this.winCharge.batteryGet.Find("StaminaGage/Gage").GetComponent<PguiImageCtrl>().m_Image.fillAmount = num;
				this.winCharge.batteryGet.Find("StaminaGage/Gage_Up").GetComponent<PguiImageCtrl>().m_Image.fillAmount = num2;
				this.winChargeUse.batteryGet.Find("StaminaGage/Gage").GetComponent<PguiImageCtrl>().m_Image.fillAmount = num;
				this.winChargeUse.batteryGet.Find("StaminaGage/Gage_Up").GetComponent<PguiImageCtrl>().m_Image.fillAmount = num2;
				this.winCharge.win.choiceR.GetComponent<PguiButtonCtrl>().SetActEnable(this.chargeTim > 0, false, false);
			}
			if (!this.winMachine.win.FinishedClose())
			{
				MasterRoomMachineDataModel machineTimeData = this.winMachine.machineTimeData;
				MstMasterRoomMachineData mstMachineData = this.winMachine.mstMachineData;
				if (this.winMachine.machineTimeData != null && this.winMachine.mstMachineData != null)
				{
					int numMachine = this.GetNumMachine(machineTimeData, mstMachineData);
					this.winMachine.itemGetNumText.text = numMachine.ToString();
					if (this.IsMachineStackMax(machineTimeData, mstMachineData))
					{
						this.winMachine.itemGetTimeInfo.Find("Num_Time").GetComponent<PguiTextCtrl>().text = "00:00:00";
						this.winMachine.itemGetTimeInfo.Find("StaminaGage/Gage").GetComponent<PguiImageCtrl>().m_Image.fillAmount = 1f;
						this.winMachine.getMax.playOutTime = this.winMachine.getMax.duration;
					}
					else
					{
						int num3 = mstMachineData.getItemTime * 60 - (int)(this.GetMachinePassTime(machineTimeData) / 10000000L) % (mstMachineData.getItemTime * 60);
						this.winMachine.itemGetTimeInfo.Find("Num_Time").GetComponent<PguiTextCtrl>().text = string.Concat(new string[]
						{
							(num3 / 3600).ToString(),
							":",
							(num3 / 60 % 60).ToString("D2"),
							":",
							(num3 % 60).ToString("D2")
						});
						float num4 = 1f - (float)num3 / ((float)mstMachineData.getItemTime * 60f);
						this.winMachine.itemGetTimeInfo.Find("StaminaGage/Gage").GetComponent<PguiImageCtrl>().m_Image.fillAmount = num4;
						this.winMachine.getMax.playOutTime = 0f;
					}
					int num5;
					int num6;
					if (mstMachineData.getItemId > 0)
					{
						num5 = DataManager.DmItem.GetUserItemData(mstMachineData.getItemId).num;
						num6 = num5 + numMachine;
					}
					else
					{
						num5 = DataManager.DmUserInfo.staminaInfo.GetInfoByNow(TimeManager.Now).stackNum;
						num6 = num5 + numMachine;
						if (num6 > DataManager.DmServerMst.MstAppConfig.staminaLimit)
						{
							num6 = DataManager.DmServerMst.MstAppConfig.staminaLimit;
						}
					}
					this.winMachine.numBeforeItemText.text = num5.ToString();
					this.winMachine.numAfterItemText.text = num6.ToString();
					bool flag = mstMachineData.getItemId == 0 && num5 >= DataManager.DmServerMst.MstAppConfig.staminaLimit;
					this.winMachine.win.choiceR.GetComponent<PguiButtonCtrl>().SetActEnable(numMachine > 0 && !flag, false, false);
					this.winMachine.isPrepared = true;
				}
			}
			else if (!this.winMachineAll.win.FinishedClose())
			{
				if (this.winMachineAll.scroll.transform.parent.gameObject.activeSelf)
				{
					this.winMachineAll.scroll.Refresh();
				}
				List<MasterRoomMachineDataModel> list = DataManager.DmHome.GetHomeCheckResult().GetTreeHouseMachineFntrTimeList().FindAll((MasterRoomMachineDataModel x) => x.nextsecond == 0);
				bool flag2 = list.Count > 0;
				bool flag3;
				if (flag2)
				{
					flag3 = list.Find((MasterRoomMachineDataModel x) => DataManager.DmTreeHouse.GetMstMasterRoomMachineList().Find((MstMasterRoomMachineData y) => y.getItemId > 0 && x.machineId == y.id) != null) != null;
				}
				else
				{
					flag3 = false;
				}
				bool flag4 = flag3;
				bool flag5 = DataManager.DmUserInfo.staminaInfo.GetInfoByNow(TimeManager.Now).stackNum >= DataManager.DmServerMst.MstAppConfig.staminaLimit;
				this.winMachineAll.win.choiceR.GetComponent<PguiButtonCtrl>().SetActEnable(flag4 || (flag2 && !flag5), false, false);
			}
		}
		if (this.IsTop())
		{
			this.guiData.machineAllBadge.SetActive(DataManager.DmHome.GetHomeCheckResult().GetTreeHouseMachineFntrTimeList().FindAll((MasterRoomMachineDataModel x) => x.nextsecond == 0)
				.Count > 0);
		}
	}

	private void SetBtnOk(bool b, bool c)
	{
		this.guiData.btnOk.SetActEnable(b, true, false);
		this.guiData.btnOk.m_Button.enabled = this.guiData.btnOk.ActEnable;
		this.guiData.btnViewPlace.gameObject.SetActive(c);
	}

	private void PlacementFurniture()
	{
		if (this.IsPlacement() && this.furnitureMove != null)
		{
			List<SceneTreeHouse.FurnitureCtrl> list = new List<SceneTreeHouse.FurnitureCtrl>();
			bool flag = true;
			if (this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.CURTAIN)
			{
				this.floorGrid.SetActive(false);
				this.wallGrid.SetActive(false);
				if (this.moveCurtain != null)
				{
					this.furnitureMove.board = this.moveCurtain;
					this.furnitureMove.obj.transform.position = this.moveCurtain.transform.position;
					this.furnitureMove.obj.transform.eulerAngles = new Vector3(0f, this.moveRot = this.Round30(this.moveCurtain.transform.eulerAngles.y), 0f);
				}
				if (this.furnitureMove.board != null)
				{
					list.Add(this.furnitureMove);
				}
			}
			else if (this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_HANGINGS)
			{
				this.floorGrid.SetActive(false);
				this.wallGrid.SetActive(true);
				if (this.movePos.y > -100f)
				{
					if (this.furnitureMove.movdep != null || Vector3.Dot(this.furnitureMove.obj.transform.TransformVector(new Vector3(0f, 0f, -1f)), this.moveNrm) < 0.9f)
					{
						this.furnitureMove.obj.transform.position = this.movePos;
						this.furnitureMove.obj.transform.eulerAngles = new Vector3(0f, this.moveRot = this.Round30(Quaternion.LookRotation(-this.moveNrm).eulerAngles.y), 0f);
					}
					this.wallGrid.transform.eulerAngles = new Vector3(0f, -this.furnitureMove.obj.transform.eulerAngles.y, 0f);
					Vector3 vector = this.wallGrid.transform.TransformPoint(this.furnitureMove.obj.transform.position) * 8f;
					Vector3 vector2 = this.wallGrid.transform.TransformPoint(this.movePos) * 8f;
					float num = (float)this.furnitureMove.data.siz.x;
					float num2 = (float)this.furnitureMove.data.siz.y;
					vector2 = this.ChkWallColli(vector, vector2, num, -num2);
					vector2 = this.ChkWallColli(vector, vector2, -num, -num2);
					vector2 = this.ChkWallColli(vector, vector2, -num, num2);
					vector2 = this.ChkWallColli(vector, vector2, num, num2);
					vector2 /= 8f;
					this.furnitureMove.obj.transform.position = this.wallGrid.transform.InverseTransformPoint(vector2);
					this.wallGrid.transform.eulerAngles = Vector3.zero;
				}
				else if (!this.touchView)
				{
					if (this.furnitureMove.movdep == null)
					{
						this.wallGrid.transform.eulerAngles = new Vector3(0f, -this.furnitureMove.obj.transform.eulerAngles.y, 0f);
						this.movePos = this.wallGrid.transform.TransformPoint(this.furnitureMove.obj.transform.position) * 8f;
						this.movePos.x = Mathf.Round(this.movePos.x);
						this.movePos.y = Mathf.Round(this.movePos.y);
						float num3 = this.ChkWallGrid(this.movePos);
						if (num3 > -999f)
						{
							this.movePos.z = num3;
						}
						this.furnitureMove.obj.transform.position = this.wallGrid.transform.InverseTransformPoint(this.movePos / 8f);
						this.wallGrid.transform.eulerAngles = Vector3.zero;
					}
					else
					{
						this.furnitureMove.obj.transform.position = this.furnitureMove.movdep.transform.parent.position;
						this.furnitureMove.obj.transform.rotation = this.furnitureMove.movdep.transform.parent.rotation;
					}
				}
				if (this.furnitureMove.board != null)
				{
					list.AddRange(this.ChkPutWall());
				}
			}
			else if (this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_PAPER || this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.FLOOR_PAPER || this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.CEIL_DECO)
			{
				flag = false;
				this.floorGrid.SetActive(false);
				this.wallGrid.SetActive(false);
				this.furnitureMove.obj.transform.position = Vector3.zero;
				this.furnitureMove.obj.transform.eulerAngles = Vector3.zero;
				if (this.furnitureMove.no == 0)
				{
					list.Add(this.furnitureMove);
				}
			}
			else if (this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.CEIL_LIGHT)
			{
				flag = false;
				this.floorGrid.SetActive(false);
				this.wallGrid.SetActive(false);
				this.furnitureMove.obj.transform.position = this.lightLoc.position;
				this.furnitureMove.obj.transform.eulerAngles = Vector3.zero;
				if (this.furnitureMove.no == 0)
				{
					list.Add(this.furnitureMove);
				}
			}
			else
			{
				this.floorGrid.SetActive(true);
				this.wallGrid.SetActive(false);
				this.furnitureMove.obj.transform.eulerAngles = new Vector3(0f, this.Round30(this.floorGrid.transform.eulerAngles.y + this.moveRot), 0f);
				if (this.movePos.y > -100f)
				{
					Vector3 vector3 = this.floorGrid.transform.InverseTransformPoint(this.furnitureMove.obj.transform.position) * 4f;
					Vector3 vector4 = this.floorGrid.transform.InverseTransformPoint(this.movePos) * 4f;
					Vector3 vector5 = new Vector3((float)this.furnitureMove.data.siz.x, 0f, (float)this.furnitureMove.data.siz.z);
					Matrix4x4 identity = Matrix4x4.identity;
					identity.SetTRS(Vector3.zero, Quaternion.Euler(0f, this.moveRot, 0f), Vector3.one);
					vector5 = identity.MultiplyPoint3x4(vector5);
					vector4 = this.ChkFloorColli(vector3, vector4, vector5.x, vector5.z);
					vector4 = this.ChkFloorColli(vector3, vector4, -vector5.x, vector5.z);
					vector4 = this.ChkFloorColli(vector3, vector4, vector5.x, -vector5.z);
					vector4 = this.ChkFloorColli(vector3, vector4, -vector5.x, -vector5.z);
					vector4 /= 4f;
					this.furnitureMove.obj.transform.position = this.floorGrid.transform.TransformPoint(vector4);
				}
				else if (!this.touchView)
				{
					if (this.furnitureMove.movdep == null)
					{
						this.movePos = this.floorGrid.transform.InverseTransformPoint(this.furnitureMove.obj.transform.position);
						this.movePos.x = Mathf.Round(this.movePos.x * 4f) / 4f;
						this.movePos.z = Mathf.Round(this.movePos.z * 4f) / 4f;
						this.furnitureMove.obj.transform.position = this.floorGrid.transform.TransformPoint(this.movePos);
					}
					else
					{
						this.furnitureMove.obj.transform.position = this.furnitureMove.movdep.transform.parent.position;
						float num4 = this.Round90(Mathf.DeltaAngle(this.furnitureMove.movdep.transform.parent.eulerAngles.y, this.furnitureMove.obj.transform.eulerAngles.y));
						this.furnitureMove.obj.transform.eulerAngles = new Vector3(0f, this.furnitureMove.movdep.transform.parent.eulerAngles.y + num4, 0f);
					}
				}
				if (this.furnitureMove.board != null)
				{
					list.AddRange(this.ChkPutFloor());
				}
			}
			this.SetBtnOk(list.Contains(this.furnitureMove), flag);
		}
		else
		{
			this.floorGrid.SetActive(false);
			this.wallGrid.SetActive(false);
		}
		this.movePos.y = -999f;
		this.moveCurtain = null;
		using (List<GameObject>.Enumerator enumerator = this.curtainBoard.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameObject obj = enumerator.Current;
				if (this.furnitureList.Find((SceneTreeHouse.FurnitureCtrl itm) => itm.no >= 0 && itm.board == obj) == null)
				{
					obj.SetActive(this.furnitureMove != null && this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.CURTAIN);
					if (obj.activeSelf)
					{
						obj.GetComponent<MeshRenderer>().material.SetColor("_TintColor", new Color(0f, 0f, 0.5f, 0.25f));
					}
				}
			}
		}
	}

	private void CtrlAction(bool other)
	{
		List<SceneTreeHouse.FurnitureCtrl> list = new List<SceneTreeHouse.FurnitureCtrl>((other ? this.otherFurniture : this.furnitureList).FindAll((SceneTreeHouse.FurnitureCtrl itm) => itm.no > 0 && itm.mdl != null));
		list.RemoveAll((SceneTreeHouse.FurnitureCtrl itm) => itm.data.dat.charaActionId <= 0 || itm.action == null || itm.action.Count < 1);
		List<SceneTreeHouse.CharaCtrl> list2 = new List<SceneTreeHouse.CharaCtrl>((other ? this.otherChara : this.charaList).FindAll((SceneTreeHouse.CharaCtrl itm) => itm.no > 0 && itm.chara != null && itm.hdl != null && itm.hdl.IsFinishInitialize()));
		List<SceneTreeHouse.ActionCtrl> act = (other ? this.otherAction : this.actionList);
		List<SceneTreeHouse.ActionCtrl> rmv = new List<SceneTreeHouse.ActionCtrl>();
		using (List<SceneTreeHouse.ActionCtrl>.Enumerator enumerator = act.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				SceneTreeHouse.<>c__DisplayClass276_1 CS$<>8__locals2 = new SceneTreeHouse.<>c__DisplayClass276_1();
				CS$<>8__locals2.ac = enumerator.Current;
				SceneTreeHouse.FurnitureCtrl furnitureCtrl = list.Find((SceneTreeHouse.FurnitureCtrl itm) => itm.no == CS$<>8__locals2.ac.furniture);
				if (furnitureCtrl == null)
				{
					rmv.Add(CS$<>8__locals2.ac);
				}
				else
				{
					int j = 0;
					while (j < CS$<>8__locals2.ac.chara.Count)
					{
						if (j >= furnitureCtrl.action.Count)
						{
							CS$<>8__locals2.ac.chara.RemoveAt(j);
						}
						else if (list2.Find((SceneTreeHouse.CharaCtrl itm) => itm.no == CS$<>8__locals2.ac.chara[j]) == null)
						{
							CS$<>8__locals2.ac.chara.RemoveAt(j);
							furnitureCtrl.action.Add(furnitureCtrl.action[j]);
							furnitureCtrl.action.RemoveAt(j);
						}
						else
						{
							int num = j;
							j = num + 1;
						}
					}
				}
			}
		}
		act.RemoveAll((SceneTreeHouse.ActionCtrl itm) => rmv.Contains(itm));
		if (this.furnitureMove == null)
		{
			list.RemoveAll((SceneTreeHouse.FurnitureCtrl itm) => act.Find((SceneTreeHouse.ActionCtrl itm2) => itm2.furniture == itm.no && itm2.chara.Count >= itm.action.Count) != null);
			list2.RemoveAll((SceneTreeHouse.CharaCtrl itm) => act.Find((SceneTreeHouse.ActionCtrl itm2) => itm2.chara.Contains(itm.no)) != null);
			while (list.Count > 0)
			{
				if (list2.Count <= 0)
				{
					break;
				}
				SceneTreeHouse.FurnitureCtrl fc = list[Random.Range(0, list.Count)];
				list.Remove(fc);
				int num2 = ((fc.action.Count > 1) ? 2 : 1);
				SceneTreeHouse.ActionCtrl ac2 = act.Find((SceneTreeHouse.ActionCtrl itm) => itm.furniture == fc.no);
				if (ac2 == null)
				{
					ac2 = new SceneTreeHouse.ActionCtrl
					{
						furniture = fc.no,
						chara = new List<int>(),
						step = 0,
						flag = 0,
						motSpd = -1f
					};
				}
				else
				{
					act.Remove(ac2);
				}
				List<SceneTreeHouse.CharaCtrl> list3 = new List<SceneTreeHouse.CharaCtrl>();
				List<SceneTreeHouse.CharaCtrl> r = new List<SceneTreeHouse.CharaCtrl>();
				Predicate<SceneTreeHouse.CharaCtrl> <>9__13;
				Predicate<SceneTreeHouse.CharaCtrl> <>9__14;
				while (ac2.chara.Count < fc.action.Count && list2.Count > 0)
				{
					SceneTreeHouse.CharaCtrl charaCtrl = list2[Random.Range(0, list2.Count)];
					list3.Add(charaCtrl);
					list2.Remove(charaCtrl);
					bool flag = charaCtrl.noHand || DataManager.DmTreeHouse.IsSpecialActionByChara(fc.data.dat.charaActionId, charaCtrl.chara.id, charaCtrl.chara.dynamicData.equipClothesId);
					if (fc.data.dat.charaActionId == 1006 && ac2.chara.Count > 0)
					{
						List<SceneTreeHouse.CharaCtrl> list4 = (other ? this.otherChara : this.charaList);
						Predicate<SceneTreeHouse.CharaCtrl> predicate;
						if ((predicate = <>9__13) == null)
						{
							predicate = (<>9__13 = (SceneTreeHouse.CharaCtrl itm) => itm.no == ac2.chara[0]);
						}
						SceneTreeHouse.CharaCtrl charaCtrl2 = list4.Find(predicate);
						if (flag == (charaCtrl2.noHand || DataManager.DmTreeHouse.IsSpecialActionByChara(fc.data.dat.charaActionId, charaCtrl2.chara.id, charaCtrl2.chara.dynamicData.equipClothesId)))
						{
							ac2.chara.Add(charaCtrl.no);
						}
						else
						{
							r.Add(charaCtrl);
						}
					}
					else if ((fc.data.dat.charaActionId == 1004 || fc.data.dat.charaActionId == 1010 || fc.data.dat.charaActionId == 1013 || fc.data.dat.charaActionId == 1016 || fc.data.dat.charaActionId == 1017 || fc.data.dat.charaActionId == 1023 || fc.data.dat.charaActionId == 1026 || fc.data.dat.charaActionId == 1027 || fc.data.dat.charaActionId == 2028) && ac2.chara.Count > 0)
					{
						List<SceneTreeHouse.CharaCtrl> list5 = (other ? this.otherChara : this.charaList);
						Predicate<SceneTreeHouse.CharaCtrl> predicate2;
						if ((predicate2 = <>9__14) == null)
						{
							predicate2 = (<>9__14 = (SceneTreeHouse.CharaCtrl itm) => itm.no == ac2.chara[0]);
						}
						SceneTreeHouse.CharaCtrl charaCtrl3 = list5.Find(predicate2);
						if (!flag || (!charaCtrl3.noHand && !DataManager.DmTreeHouse.IsSpecialActionByChara(fc.data.dat.charaActionId, charaCtrl3.chara.id, charaCtrl3.chara.dynamicData.equipClothesId)))
						{
							ac2.chara.Add(charaCtrl.no);
						}
						else
						{
							r.Add(charaCtrl);
						}
					}
					else if (fc.data.dat.charaActionId == 1012 || fc.data.dat.charaActionId == 1015 || fc.data.dat.charaActionId == 2015 || fc.data.dat.charaActionId == 1022 || fc.data.dat.charaActionId == 2019)
					{
						if (flag)
						{
							r.Add(charaCtrl);
						}
						else
						{
							ac2.chara.Add(charaCtrl.no);
						}
					}
					else if (fc.data.dat.charaActionId == 2004 || fc.data.dat.charaActionId == 2006 || fc.data.dat.charaActionId == 2012 || fc.data.dat.charaActionId == 2016 || fc.data.dat.charaActionId == 2017 || fc.data.dat.charaActionId == 2018 || fc.data.dat.charaActionId == 2020 || fc.data.dat.charaActionId == 1020)
					{
						if (flag)
						{
							r.Add(charaCtrl);
						}
						else
						{
							ac2.chara.Add(charaCtrl.no);
						}
					}
					else if (fc.data.dat.charaActionId == 1019)
					{
						if (DataManager.DmTreeHouse.IsSpecialActionByChara(fc.data.dat.charaActionId, charaCtrl.chara.id, charaCtrl.chara.dynamicData.equipClothesId))
						{
							r.Add(charaCtrl);
						}
						else
						{
							ac2.chara.Add(charaCtrl.no);
						}
					}
					else
					{
						ac2.chara.Add(charaCtrl.no);
					}
					if (ac2.chara.Count >= num2)
					{
						break;
					}
				}
				list3.RemoveAll((SceneTreeHouse.CharaCtrl itm) => r.Contains(itm));
				list2.AddRange(r);
				if (ac2.chara.Count < num2)
				{
					list2.AddRange(list3);
				}
				else
				{
					ac2.step = 0;
					ac2.flag = 0;
					this.StickInit(fc);
					if (fc.handle != null)
					{
						fc.handle.localPosition = Vector3.zero;
					}
					act.Add(ac2);
				}
			}
		}
		else
		{
			SceneTreeHouse.ActionCtrl ac3 = act.Find((SceneTreeHouse.ActionCtrl itm) => itm.furniture == this.furnitureMove.no);
			if (ac3 != null)
			{
				bool flag2 = true;
				int i = 0;
				Predicate<SceneTreeHouse.CharaCtrl> <>9__16;
				while (flag2 && i < ac3.chara.Count)
				{
					List<SceneTreeHouse.CharaCtrl> list6 = list2;
					Predicate<SceneTreeHouse.CharaCtrl> predicate3;
					if ((predicate3 = <>9__16) == null)
					{
						predicate3 = (<>9__16 = (SceneTreeHouse.CharaCtrl itm) => itm.no == ac3.chara[i]);
					}
					SceneTreeHouse.CharaCtrl charaCtrl4 = list6.Find(predicate3);
					if (charaCtrl4 != null && charaCtrl4.hdl.GetAlpha() > 0.01f)
					{
						flag2 = false;
					}
					int num = i;
					i = num + 1;
				}
				if (flag2)
				{
					act.Remove(ac3);
				}
			}
		}
		rmv = new List<SceneTreeHouse.ActionCtrl>();
		using (List<SceneTreeHouse.ActionCtrl>.Enumerator enumerator = act.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				SceneTreeHouse.ActionCtrl ac = enumerator.Current;
				SceneTreeHouse.FurnitureCtrl furnitureCtrl2 = (other ? this.otherFurniture : this.furnitureList).Find((SceneTreeHouse.FurnitureCtrl itm) => itm.no == ac.furniture);
				int num3 = ((furnitureCtrl2.action.Count > 1) ? 2 : 1);
				if (ac.chara.Count < num3)
				{
					rmv.Add(ac);
				}
				else if (furnitureCtrl2.data.dat.charaActionId == 1004 || furnitureCtrl2.data.dat.charaActionId == 1010 || furnitureCtrl2.data.dat.charaActionId == 1013 || furnitureCtrl2.data.dat.charaActionId == 1016 || furnitureCtrl2.data.dat.charaActionId == 1017 || furnitureCtrl2.data.dat.charaActionId == 1023 || furnitureCtrl2.data.dat.charaActionId == 1026 || furnitureCtrl2.data.dat.charaActionId == 1027 || furnitureCtrl2.data.dat.charaActionId == 2028)
				{
					int idx = (furnitureCtrl2.action[0].name.EndsWith("_a") ? 0 : 1);
					SceneTreeHouse.CharaCtrl charaCtrl5 = (other ? this.otherChara : this.charaList).Find((SceneTreeHouse.CharaCtrl itm) => itm.no == ac.chara[idx]);
					if (charaCtrl5.noHand || DataManager.DmTreeHouse.IsSpecialActionByChara(furnitureCtrl2.data.dat.charaActionId, charaCtrl5.chara.id, charaCtrl5.chara.dynamicData.equipClothesId))
					{
						ac.chara.Add(ac.chara[0]);
						ac.chara.RemoveAt(0);
					}
				}
			}
		}
		act.RemoveAll((SceneTreeHouse.ActionCtrl itm) => rmv.Contains(itm));
	}

	private void CtrlFurniture(bool other)
	{
		bool flag = false;
		float num = 1f;
		float num2 = (this.camBCG.enabled ? 1f : (TimeManager.DeltaTime * 4f));
		if (!other && (this.furnitureMove != null || this.isMove > 0))
		{
			flag = true;
			num = 0.5f;
		}
		bool flag2 = this.furnitureMove != null && this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.GENERAL_MERCHANDISE;
		bool flag3 = this.furnitureMove != null && this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_HANGINGS && this.furnitureMove.data.siz.z < 0;
		foreach (GameObject gameObject in this.doorGridMap.Keys)
		{
			gameObject.SetActive(flag3);
		}
		List<SceneTreeHouse.FurnitureCtrl> list = (other ? this.otherFurniture : this.furnitureList);
		List<SceneTreeHouse.ActionCtrl> list2 = (other ? this.otherAction : this.actionList);
		using (List<SceneTreeHouse.FurnitureCtrl>.Enumerator enumerator2 = list.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				SceneTreeHouse.FurnitureCtrl fc = enumerator2.Current;
				if (fc.mdl == null)
				{
					if (string.IsNullOrEmpty(fc.path))
					{
						fc.path = fc.data.dat.modelFileName;
						if ((fc.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_PAPER || fc.data.dat.category == TreeHouseFurnitureStatic.Category.FLOOR_PAPER || fc.data.dat.category == TreeHouseFurnitureStatic.Category.CEIL_DECO) && this.IsNight)
						{
							fc.path = fc.path.Replace("_a_mdl", "_b_mdl");
						}
						AssetManager.LoadAssetData(fc.path = SceneTreeHouse.FURNITURE_PATH + fc.path, AssetManager.OWNER.TreeHouseStage, 0, null);
						fc.mainTex = fc.data.dat.embedTexturePathSub;
						fc.addTex = fc.data.dat.embedTexturePath;
						if (string.IsNullOrEmpty(fc.mainTex))
						{
							fc.mainTex = fc.addTex;
							fc.addTex = null;
						}
						if (!string.IsNullOrEmpty(fc.mainTex))
						{
							AssetManager.LoadAssetData(fc.mainTex, AssetManager.OWNER.TreeHouseStage, 0, null);
						}
						if (!string.IsNullOrEmpty(fc.addTex))
						{
							AssetManager.LoadAssetData(fc.addTex, AssetManager.OWNER.TreeHouseStage, 0, null);
						}
						this.FurnitureEffect1006Init(fc);
						this.FurnitureEffect2006Init(fc);
					}
					else if (AssetManager.IsLoadFinishAssetData(fc.path) && (string.IsNullOrEmpty(fc.mainTex) || AssetManager.IsLoadFinishAssetData(fc.mainTex)) && (string.IsNullOrEmpty(fc.addTex) || AssetManager.IsLoadFinishAssetData(fc.addTex)) && this.FurnitureEffect1006Check(fc) && this.FurnitureEffect2006Check(fc))
					{
						bool flag4 = false;
						TreeHouseFurnitureStatic.Category category = fc.data.dat.category;
						if (category == TreeHouseFurnitureStatic.Category.RUG || category - TreeHouseFurnitureStatic.Category.CURTAIN <= 4)
						{
							flag4 = true;
						}
						fc.alpha = num2;
						fc.mdl = AssetManager.InstantiateAssetData(fc.path, null);
						fc.mdl.transform.SetParent(fc.obj.transform, false);
						fc.mdl.SetLayerRecursively(this.stageLocator.layer);
						fc.mdl.SetActive(true);
						fc.on = new List<GameObject>();
						fc.off = new List<GameObject>();
						fc.chr = new List<Transform>();
						fc.light = "";
						fc.sw = null;
						fc.grid = new Dictionary<GameObject, Vector3>();
						fc.pstr = new Dictionary<GameObject, Vector3>();
						fc.stick = null;
						fc.stickMin = null;
						fc.stickMax = null;
						fc.clockH = new List<Transform>();
						fc.clockM = new List<Transform>();
						fc.clockS = new List<Transform>();
						fc.cam = new List<Transform>();
						fc.handle = null;
						foreach (object obj in fc.mdl.transform)
						{
							Transform transform = (Transform)obj;
							if (transform.name.StartsWith("pos_cha_direction_"))
							{
								fc.chr.Add(transform);
								transform.gameObject.SetActive(transform.childCount > 0);
							}
							else if (transform.name.StartsWith("switch_obj_"))
							{
								fc.sw = transform;
								int num3 = transform.name.IndexOf("_Interiorlight_");
								if (num3 > 0)
								{
									fc.light = transform.name.Substring(num3);
								}
								transform.gameObject.SetActive(false);
							}
							else if (transform.name.StartsWith("pos_obj_interior_"))
							{
								if (fc.data.dat.category == TreeHouseFurnitureStatic.Category.STAND || (fc.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_HANGINGS && fc.data.siz.z > 0))
								{
									GameObject gameObject2 = AssetManager.InstantiateAssetData(SceneTreeHouse.GRID_DEPEND_PATH, null);
									gameObject2.name = "depend_" + transform.name.Substring(transform.name.Length - 1);
									gameObject2.transform.SetParent(transform, false);
									gameObject2.SetLayerRecursively(this.stageLocator.layer);
									transform.gameObject.SetActive(true);
									gameObject2.SetActive(false);
									Vector3 vector = Vector3.one * 10f;
									foreach (object obj2 in transform)
									{
										Transform transform2 = (Transform)obj2;
										if (transform2.name.StartsWith("pos_siz"))
										{
											vector = transform2.localScale / 100f;
											break;
										}
									}
									fc.grid.Add(gameObject2, vector);
								}
							}
							else if (transform.name.StartsWith("pos_pstr_interior_"))
							{
								if (!flag4 && (fc.data.dat.category != TreeHouseFurnitureStatic.Category.WALL_HANGINGS || fc.data.siz.z >= 0))
								{
									GameObject gameObject3 = AssetManager.InstantiateAssetData(SceneTreeHouse.GRID_DEPEND_PATH, null);
									gameObject3.name = "poster_" + transform.name.Substring(transform.name.Length - 1);
									gameObject3.transform.SetParent(transform, false);
									gameObject3.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
									gameObject3.SetLayerRecursively(this.stageLocator.layer);
									transform.gameObject.SetActive(true);
									gameObject3.SetActive(false);
									Vector3 vector2 = Vector3.one * 10f;
									foreach (object obj3 in transform)
									{
										Transform transform3 = (Transform)obj3;
										if (transform3.name.StartsWith("pos_siz"))
										{
											vector2 = transform3.localScale / 100f;
											break;
										}
									}
									fc.pstr.Add(gameObject3, vector2);
								}
							}
							else if (transform.name.EndsWith("__on"))
							{
								fc.on.Add(transform.gameObject);
							}
							else if (transform.name.EndsWith("__off"))
							{
								fc.off.Add(transform.gameObject);
							}
							else if (transform.name.StartsWith("cam_pos_direction_"))
							{
								fc.cam.Add(transform);
								transform.gameObject.SetActive(false);
							}
							else
							{
								transform.gameObject.SetActive(true);
								if (transform.name.StartsWith("clock_hourhand_"))
								{
									fc.clockH.Add(transform);
								}
								else if (transform.name.StartsWith("clock_minutehand_"))
								{
									fc.clockM.Add(transform);
								}
								else if (transform.name.StartsWith("clock_secondhand_"))
								{
									fc.clockS.Add(transform);
								}
							}
						}
						fc.chr.Sort((Transform a, Transform b) => a.name.CompareTo(b.name));
						if ((fc.data.dat.charaActionId == 2004 || fc.data.dat.charaActionId == 2005 || fc.data.dat.charaActionId == 2006 || fc.data.dat.charaActionId == 2009 || fc.data.dat.charaActionId == 2012 || fc.data.dat.charaActionId == 2016 || fc.data.dat.charaActionId == 2017 || fc.data.dat.charaActionId == 1020 || fc.data.dat.charaActionId == 2018 || fc.data.dat.charaActionId == 2020) && fc.chr.Count > 1)
						{
							fc.chr = new List<Transform> { fc.chr[0] };
						}
						fc.mesh = fc.mdl.GetComponentsInChildren<MeshRenderer>(true);
						AssetManager.AddLoadList(fc.path, AssetManager.OWNER.TreeHouseStage);
						if (!string.IsNullOrEmpty(fc.mainTex))
						{
							fc.mesh[0].material.SetTexture("_MainTex", AssetManager.GetAssetData(fc.mainTex) as Texture2D);
						}
						if (!string.IsNullOrEmpty(fc.addTex))
						{
							fc.mesh[0].material.SetTexture("_AddTex", AssetManager.GetAssetData(fc.addTex) as Texture2D);
						}
						if (fc.data.dat.charaActionId == 1006)
						{
							fc.stick = new List<Transform>
							{
								fc.mdl.transform.Find("stick_left"),
								fc.mdl.transform.Find("stick_right")
							};
							this.StickInit(fc);
						}
						else if (fc.data.dat.charaActionId == 2006)
						{
							fc.stick = new List<Transform> { fc.mdl.transform.Find("stick_right") };
							this.StickInit(fc);
						}
						else if (fc.data.dat.charaActionId == 1023)
						{
							fc.handle = fc.mdl.transform.Find("handle_a");
							if (fc.handle != null)
							{
								fc.handle.localPosition = Vector3.zero;
							}
						}
						if (!other)
						{
							if (fc.data.dat.category == TreeHouseFurnitureStatic.Category.CURTAIN)
							{
								float num4 = 9999f;
								using (List<GameObject>.Enumerator enumerator5 = this.curtainBoard.GetEnumerator())
								{
									while (enumerator5.MoveNext())
									{
										GameObject gameObject4 = enumerator5.Current;
										float num5 = Vector3.Distance(fc.pos, gameObject4.transform.position);
										if (num5 < num4)
										{
											num4 = num5;
											fc.board = gameObject4;
										}
									}
									goto IL_1093;
								}
							}
							if (fc.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_PAPER || fc.data.dat.category == TreeHouseFurnitureStatic.Category.FLOOR_PAPER || fc.data.dat.category == TreeHouseFurnitureStatic.Category.CEIL_DECO)
							{
								fc.alpha = 1f;
							}
							else if (fc.data.dat.category == TreeHouseFurnitureStatic.Category.CEIL_LIGHT)
							{
								fc.alpha = 1f;
							}
							else
							{
								fc.board = AssetManager.InstantiateAssetData(SceneTreeHouse.BOARD_PATH, null);
								fc.board.transform.SetParent(fc.obj.transform, false);
								fc.board.SetLayerRecursively(this.stageLocator.layer);
								fc.board.AddComponent<MeshCollider>();
								fc.board.SetActive(false);
								if (fc.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_HANGINGS)
								{
									fc.board.transform.localPosition = new Vector3(0f, 0f, -0.01f);
									fc.board.transform.localScale = new Vector3((float)fc.data.siz.x, 1f, (float)fc.data.siz.y);
									fc.board.transform.localEulerAngles = new Vector3(-90f, 0f, 0f);
								}
								else
								{
									fc.board.transform.localPosition = new Vector3(0f, 0.01f, 0f);
									fc.board.transform.localScale = new Vector3((float)(fc.data.siz.x * 2), 1f, (float)(fc.data.siz.z * 2));
								}
							}
						}
						IL_1093:
						foreach (SceneTreeHouse.FurnitureCtrl furnitureCtrl in list)
						{
							if (furnitureCtrl.no > 0 && !(furnitureCtrl.depend != fc.obj))
							{
								if (furnitureCtrl.data.dat.category == TreeHouseFurnitureStatic.Category.GENERAL_MERCHANDISE)
								{
									float num6 = 99999f;
									GameObject gameObject5 = null;
									using (Dictionary<GameObject, Vector3>.KeyCollection.Enumerator enumerator = fc.grid.Keys.GetEnumerator())
									{
										while (enumerator.MoveNext())
										{
											GameObject g2 = enumerator.Current;
											if (list.Find((SceneTreeHouse.FurnitureCtrl itm) => itm.depend == g2) == null)
											{
												float magnitude = (furnitureCtrl.obj.transform.position - g2.transform.parent.position).magnitude;
												if (num6 > magnitude)
												{
													num6 = magnitude;
													gameObject5 = g2;
												}
											}
										}
									}
									if ((furnitureCtrl.depend = gameObject5) != null)
									{
										furnitureCtrl.dir = this.Round90(Mathf.DeltaAngle(furnitureCtrl.depend.transform.parent.eulerAngles.y, furnitureCtrl.obj.transform.eulerAngles.y));
										furnitureCtrl.pos = (furnitureCtrl.obj.transform.position = furnitureCtrl.depend.transform.parent.position);
										furnitureCtrl.obj.transform.eulerAngles = new Vector3(0f, furnitureCtrl.depend.transform.parent.eulerAngles.y + furnitureCtrl.dir, 0f);
										furnitureCtrl.CalcCorner();
									}
								}
								else if (furnitureCtrl.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_HANGINGS)
								{
									float num7 = 99999f;
									GameObject gameObject6 = null;
									using (Dictionary<GameObject, Vector3>.KeyCollection.Enumerator enumerator = fc.pstr.Keys.GetEnumerator())
									{
										while (enumerator.MoveNext())
										{
											GameObject g = enumerator.Current;
											if (list.Find((SceneTreeHouse.FurnitureCtrl itm) => itm.depend == g) == null)
											{
												float magnitude2 = (furnitureCtrl.obj.transform.position - g.transform.parent.position).magnitude;
												if (num7 > magnitude2)
												{
													num7 = magnitude2;
													gameObject6 = g;
												}
											}
										}
									}
									if ((furnitureCtrl.depend = gameObject6) != null)
									{
										furnitureCtrl.dir = 0f;
										furnitureCtrl.pos = (furnitureCtrl.obj.transform.position = furnitureCtrl.depend.transform.parent.position);
										furnitureCtrl.obj.transform.rotation = furnitureCtrl.depend.transform.parent.rotation;
										furnitureCtrl.CalcCorner();
									}
								}
							}
						}
						this.SetAlpha(fc);
						if (fc.cam.Count > 0)
						{
							this.gyroLst.Add(fc);
							this.gyroLst.Sort(delegate(SceneTreeHouse.FurnitureCtrl a, SceneTreeHouse.FurnitureCtrl b)
							{
								if (a.no <= 0)
								{
									return -1;
								}
								if (b.no <= 0)
								{
									return 1;
								}
								return b.no - a.no;
							});
							this.guiData.btnCamera.SetActEnable(this.gyroLst.Count > 0, false, false);
							this.guiData.btnReturn.SetActEnable(this.gyroLst.Count > 1, false, false);
							this.guiOther.btnCamera.SetActEnable(this.gyroLst.Count > 0, false, false);
							this.guiOther.btnReturn.SetActEnable(this.gyroLst.Count > 1, false, false);
						}
					}
				}
				else if (fc.no < 0)
				{
					if (fc.obj != null)
					{
						bool flag5 = false;
						if (fc.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_PAPER || fc.data.dat.category == TreeHouseFurnitureStatic.Category.FLOOR_PAPER || fc.data.dat.category == TreeHouseFurnitureStatic.Category.CEIL_DECO || fc.data.dat.category == TreeHouseFurnitureStatic.Category.CEIL_LIGHT)
						{
							SceneTreeHouse.FurnitureCtrl furnitureCtrl2 = list.Find((SceneTreeHouse.FurnitureCtrl itm) => itm.no >= 0 && itm.data.dat.category == fc.data.dat.category);
							if (furnitureCtrl2 == null)
							{
								SceneTreeHouse.PaperCtrl paperCtrl = this.paperList.Find((SceneTreeHouse.PaperCtrl itm) => itm.cat == fc.data.dat.category);
								if (paperCtrl == null)
								{
									flag5 = true;
								}
								else if (paperCtrl.enable && paperCtrl.mdl != null && !paperCtrl.chgmdl)
								{
									flag5 = true;
								}
							}
							else if (furnitureCtrl2.mdl != null && !furnitureCtrl2.chgmdl)
							{
								flag5 = true;
							}
							fc.alpha = 1f;
						}
						else if ((fc.alpha -= num2) <= 0f)
						{
							flag5 = true;
						}
						if (flag5)
						{
							if (fc.eff != null)
							{
								EffectManager.DestroyEffect(fc.eff);
							}
							fc.eff = null;
							this.FurnitureEffect1006Term(fc);
							this.FurnitureEffect2006Term(fc);
							fc.depend = null;
							fc.movdep = null;
							if (fc.data.dat.category != TreeHouseFurnitureStatic.Category.CURTAIN && fc.board != null)
							{
								Object.Destroy(fc.board);
							}
							fc.board = null;
							foreach (GameObject gameObject7 in fc.grid.Keys)
							{
								Object.Destroy(gameObject7);
							}
							fc.grid = new Dictionary<GameObject, Vector3>();
							foreach (GameObject gameObject8 in fc.pstr.Keys)
							{
								Object.Destroy(gameObject8);
							}
							fc.pstr = new Dictionary<GameObject, Vector3>();
							fc.action = null;
							fc.stick = null;
							fc.stickMin = null;
							fc.stickMax = null;
							fc.on = null;
							fc.off = null;
							fc.sw = null;
							fc.chr = null;
							fc.mesh = null;
							fc.clockH = new List<Transform>();
							fc.clockM = new List<Transform>();
							fc.clockS = new List<Transform>();
							fc.cam = new List<Transform>();
							fc.handle = null;
							if (fc.mdl != null)
							{
								Object.Destroy(fc.mdl);
							}
							fc.mdl = null;
							if (fc.obj != null)
							{
								Object.Destroy(fc.obj);
							}
							fc.obj = null;
							fc.data = null;
							if (!string.IsNullOrEmpty(fc.mainTex))
							{
								AssetManager.AddLoadList(fc.mainTex, AssetManager.OWNER.TreeHouseStage);
							}
							if (!string.IsNullOrEmpty(fc.addTex))
							{
								AssetManager.AddLoadList(fc.addTex, AssetManager.OWNER.TreeHouseStage);
							}
							fc.path = null;
							fc.mainTex = null;
							fc.addTex = null;
							fc.chgmdl = false;
							PrjUtil.ReleaseMemory(PrjUtil.UnloadUnused / 50);
							if (this.gyroLst.Contains(fc))
							{
								this.gyroLst.Remove(fc);
								this.guiData.btnCamera.SetActEnable(this.gyroLst.Count > 0, false, false);
								this.guiData.btnReturn.SetActEnable(this.gyroLst.Count > 1, false, false);
								this.guiOther.btnCamera.SetActEnable(this.gyroLst.Count > 0, false, false);
								this.guiOther.btnReturn.SetActEnable(this.gyroLst.Count > 1, false, false);
							}
						}
						else
						{
							this.SetAlpha(fc);
						}
					}
				}
				else
				{
					float num8 = ((fc.no == 0) ? 1f : num);
					if (fc.data.dat.category == TreeHouseFurnitureStatic.Category.LARGE_FURNITURE || fc.data.dat.category == TreeHouseFurnitureStatic.Category.STAND)
					{
						if (!flag && fc != this.furnitureMove && this.ChkDistance(this.camera.transform.position, fc.corner) < SceneTreeHouse.neighborDistance * SceneTreeHouse.neighborDistance)
						{
							num8 = 0f;
						}
						SceneTreeHouse.ActionCtrl actionCtrl = ((fc.action == null) ? null : list2.Find((SceneTreeHouse.ActionCtrl itm) => itm.furniture == fc.no));
						if (actionCtrl != null)
						{
							for (int i = 0; i < actionCtrl.chara.Count; i++)
							{
								if (i >= fc.action.Count)
								{
									break;
								}
								Vector2 vector3 = new Vector2(this.camera.transform.position.x - fc.action[i].position.x, this.camera.transform.position.z - fc.action[i].position.z);
								if (vector3.magnitude < SceneTreeHouse.friendsDistance)
								{
									num8 = -1f;
									break;
								}
							}
						}
					}
					else if (fc.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_HANGINGS)
					{
						if (fc.data.siz.z > 0)
						{
							List<Vector3> list3 = new List<Vector3>
							{
								fc.corner[0],
								fc.corner[1],
								fc.corner[1] + fc.walloff,
								fc.corner[0] + fc.walloff
							};
							if (!flag && fc != this.furnitureMove && this.ChkDistance(this.camera.transform.position, list3) < SceneTreeHouse.neighborDistance * SceneTreeHouse.neighborDistance)
							{
								num8 = 0f;
							}
						}
						else if (fc != this.furnitureMove && fc.depend != null)
						{
							SceneTreeHouse.FurnitureCtrl furnitureCtrl3 = list.Find((SceneTreeHouse.FurnitureCtrl itm) => itm.pstr.ContainsKey(fc.depend));
							if (furnitureCtrl3 != null)
							{
								num8 = furnitureCtrl3.alpha;
							}
						}
					}
					else if (fc.data.dat.category == TreeHouseFurnitureStatic.Category.GENERAL_MERCHANDISE)
					{
						if (fc != this.furnitureMove && fc.depend != null)
						{
							SceneTreeHouse.FurnitureCtrl furnitureCtrl4 = list.Find((SceneTreeHouse.FurnitureCtrl itm) => itm.grid.ContainsKey(fc.depend));
							if (furnitureCtrl4 != null)
							{
								num8 = furnitureCtrl4.alpha;
							}
						}
					}
					else if (fc.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_PAPER || fc.data.dat.category == TreeHouseFurnitureStatic.Category.FLOOR_PAPER || fc.data.dat.category == TreeHouseFurnitureStatic.Category.CEIL_DECO || fc.data.dat.category == TreeHouseFurnitureStatic.Category.CEIL_LIGHT)
					{
						if (fc != this.furnitureMove && this.furnitureMove != null && fc.data.dat.category == this.furnitureMove.data.dat.category && this.furnitureMove.mdl != null)
						{
							num8 = 0f;
						}
						else
						{
							num8 = 1f;
						}
					}
					if (fc.alpha < num8)
					{
						if ((fc.alpha += num2) > num8)
						{
							fc.alpha = num8;
						}
					}
					else if (fc.alpha > num8 && (fc.alpha -= num2) < num8)
					{
						fc.alpha = num8;
					}
					if (fc.board != null)
					{
						bool flag6 = flag;
						if (flag6 && this.furnitureMove != null)
						{
							if (this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.CURTAIN)
							{
								if (fc.data.dat.category != TreeHouseFurnitureStatic.Category.CURTAIN)
								{
									flag6 = false;
								}
							}
							else if (this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_HANGINGS)
							{
								if (fc.data.dat.category != TreeHouseFurnitureStatic.Category.WALL_HANGINGS)
								{
									flag6 = false;
								}
							}
							else if (this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_PAPER || this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.FLOOR_PAPER || this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.CEIL_DECO || fc.data.dat.category == TreeHouseFurnitureStatic.Category.CEIL_LIGHT)
							{
								flag6 = false;
							}
							else if (fc.data.dat.category != TreeHouseFurnitureStatic.Category.CURTAIN && fc.data.dat.category != TreeHouseFurnitureStatic.Category.WALL_HANGINGS)
							{
								if (this.furnitureMove.data.dat.category != TreeHouseFurnitureStatic.Category.GENERAL_MERCHANDISE && fc.depend != null)
								{
									flag6 = false;
								}
							}
							else
							{
								flag6 = false;
							}
						}
						fc.board.SetActive(flag6);
					}
					this.SetAlpha(fc);
					if (this.furnitureMove != null && fc.data.dat.category == TreeHouseFurnitureStatic.Category.GENERAL_MERCHANDISE)
					{
						if (fc == this.furnitureMove)
						{
							float num9 = 0f;
							if (fc.movdep == null || list.Find((SceneTreeHouse.FurnitureCtrl itm) => itm.grid.ContainsKey(fc.movdep)) == null)
							{
								SceneTreeHouse.FurnitureCtrl furnitureCtrl5 = list.Find((SceneTreeHouse.FurnitureCtrl itm) => itm.obj == fc.movdep);
								if (furnitureCtrl5 != null)
								{
									num9 = furnitureCtrl5.obj.transform.position.y + furnitureCtrl5.data.objSiz.y;
								}
							}
							else
							{
								num9 = fc.movdep.transform.parent.position.y;
							}
							fc.obj.transform.position = new Vector3(fc.obj.transform.position.x, num9, fc.obj.transform.position.z);
						}
						else if (fc.depend == null || list.Find((SceneTreeHouse.FurnitureCtrl itm) => itm.grid.ContainsKey(fc.depend)) == null)
						{
							if (fc.movdep == null || list.Find((SceneTreeHouse.FurnitureCtrl itm) => itm.grid.ContainsKey(fc.movdep)) == null)
							{
								float num10 = 0f;
								SceneTreeHouse.FurnitureCtrl furnitureCtrl6 = list.Find((SceneTreeHouse.FurnitureCtrl itm) => itm.obj == fc.movdep);
								if (furnitureCtrl6 != null)
								{
									num10 = furnitureCtrl6.obj.transform.position.y + furnitureCtrl6.data.objSiz.y;
								}
								fc.obj.transform.position = new Vector3(fc.pos.x, num10, fc.pos.z);
								fc.obj.transform.eulerAngles = new Vector3(0f, fc.dir, 0f);
							}
							else
							{
								fc.obj.transform.position = fc.movdep.transform.parent.position;
								float num11 = this.Round90(Mathf.DeltaAngle(fc.movdep.transform.parent.eulerAngles.y, fc.dir));
								fc.obj.transform.eulerAngles = new Vector3(0f, fc.movdep.transform.parent.eulerAngles.y + num11, 0f);
							}
						}
						else
						{
							fc.obj.transform.position = fc.depend.transform.parent.position;
							fc.obj.transform.eulerAngles = new Vector3(0f, fc.depend.transform.parent.eulerAngles.y + fc.dir, 0f);
						}
					}
					else if (this.furnitureMove != null && fc.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_HANGINGS && fc.data.siz.z < 0)
					{
						if (fc == this.furnitureMove)
						{
							if (fc.movdep != null && list.Find((SceneTreeHouse.FurnitureCtrl itm) => itm.pstr.ContainsKey(fc.movdep)) == null && this.doorGridMap.ContainsKey(fc.movdep))
							{
							}
						}
						else if (fc.depend == null || (list.Find((SceneTreeHouse.FurnitureCtrl itm) => itm.pstr.ContainsKey(fc.depend)) == null && !this.doorGridMap.ContainsKey(fc.depend)))
						{
							if (!(fc.movdep == null) && (list.Find((SceneTreeHouse.FurnitureCtrl itm) => itm.pstr.ContainsKey(fc.movdep)) != null || this.doorGridMap.ContainsKey(fc.movdep)))
							{
								fc.obj.transform.position = fc.movdep.transform.parent.position;
								fc.obj.transform.rotation = fc.movdep.transform.parent.rotation;
							}
						}
						else
						{
							fc.obj.transform.position = fc.depend.transform.parent.position;
							fc.obj.transform.rotation = fc.depend.transform.parent.rotation;
						}
					}
					float num12 = 0f;
					float num13 = 0f;
					if (fc == this.furnitureMove)
					{
						if (fc.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_HANGINGS || fc.data.dat.category == TreeHouseFurnitureStatic.Category.CURTAIN)
						{
							num13 -= 0.005f;
						}
						else if (fc.data.dat.category != TreeHouseFurnitureStatic.Category.WALL_PAPER && fc.data.dat.category != TreeHouseFurnitureStatic.Category.FLOOR_PAPER && fc.data.dat.category != TreeHouseFurnitureStatic.Category.CEIL_DECO && fc.data.dat.category != TreeHouseFurnitureStatic.Category.CEIL_LIGHT)
						{
							num12 += 0.005f;
						}
					}
					else if (fc.data.dat.category == TreeHouseFurnitureStatic.Category.RUG)
					{
						num12 = (float)fc.rug * 0.001f;
					}
					fc.mdl.transform.localPosition = new Vector3(0f, num12, num13);
					foreach (GameObject gameObject9 in fc.grid.Keys)
					{
						gameObject9.SetActive(flag2 || fc == this.furnitureMove);
					}
					foreach (GameObject gameObject10 in fc.pstr.Keys)
					{
						gameObject10.SetActive(flag3 || fc == this.furnitureMove);
					}
					if (fc.action == null && fc.data.dat.charaActionId > 0)
					{
						fc.action = new List<Transform>();
						float num14 = 0f;
						float num15 = -SceneTreeHouse.friendsDistance;
						float num16 = 1.7f;
						if (fc.data.dat.charaActionId == 1009 || fc.data.dat.charaActionId == 1014)
						{
							num14 = SceneTreeHouse.friendsDistance;
							num15 = 0f;
							num16 = 0.7f;
						}
						else if (fc.data.dat.charaActionId == 1001 || fc.data.dat.charaActionId == 1002 || fc.data.dat.charaActionId == 1003 || fc.data.dat.charaActionId == 1011 || fc.data.dat.charaActionId == 1024)
						{
							num16 = 1.2f;
						}
						float num17 = SceneTreeHouse.friendsDistance * SceneTreeHouse.friendsDistance;
						foreach (Transform transform4 in fc.chr)
						{
							Vector3 position = transform4.position;
							if (this.ChkDistance(position, this.wallPos) <= -num17)
							{
								Vector3 vector4 = transform4.TransformPoint(num14, 0f, num15);
								if (this.ChkDistance(vector4, this.wallPos) <= -num17)
								{
									Vector3 vector5 = new Vector3(0f, -999f, 0f);
									if ((fc.data.dat.charaActionId == 1010 || fc.data.dat.charaActionId == 1013 || fc.data.dat.charaActionId == 1026 || fc.data.dat.charaActionId == 1027) && transform4.name[transform4.name.Length - 1] == 'a')
									{
										vector5 = transform4.TransformPoint(0f, 0f, 1.1f);
									}
									bool flag7 = true;
									foreach (SceneTreeHouse.FurnitureCtrl furnitureCtrl7 in list)
									{
										if (furnitureCtrl7 != fc && furnitureCtrl7.no > 0 && furnitureCtrl7.data.dat.category != TreeHouseFurnitureStatic.Category.CURTAIN && furnitureCtrl7.data.dat.category != TreeHouseFurnitureStatic.Category.RUG && furnitureCtrl7.data.dat.category != TreeHouseFurnitureStatic.Category.WALL_PAPER && furnitureCtrl7.data.dat.category != TreeHouseFurnitureStatic.Category.FLOOR_PAPER && furnitureCtrl7.data.dat.category != TreeHouseFurnitureStatic.Category.CEIL_DECO && furnitureCtrl7.data.dat.category != TreeHouseFurnitureStatic.Category.CEIL_LIGHT && !(furnitureCtrl7.depend != null))
										{
											List<Vector3> list4 = new List<Vector3>(furnitureCtrl7.corner);
											if (furnitureCtrl7.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_HANGINGS)
											{
												if (furnitureCtrl7.data.siz.z <= 0 || num16 < list4[2].y)
												{
													continue;
												}
												list4[0] = list4[3] + furnitureCtrl7.walloff;
												list4[1] = list4[2] + furnitureCtrl7.walloff;
											}
											if (this.ChkDistance(position, list4) < num17)
											{
												flag7 = false;
											}
											else if (this.ChkDistance(vector4, list4) < num17)
											{
												flag7 = false;
											}
											else if (vector5.y > -1f && (furnitureCtrl7.data.dat.category != TreeHouseFurnitureStatic.Category.GENERAL_MERCHANDISE || furnitureCtrl7.data.siz.y > 3) && this.ChkDistance(vector5, list4) < num17)
											{
												flag7 = false;
											}
											else if (furnitureCtrl7.action != null)
											{
												float num18 = SceneTreeHouse.friendsDistance;
												if (furnitureCtrl7.data.dat.charaActionId == 1009 || fc.data.dat.charaActionId == 1014)
												{
													float num19 = SceneTreeHouse.friendsDistance;
												}
												foreach (Transform transform5 in furnitureCtrl7.action)
												{
													Vector3 vector6 = transform5.position;
													float num20 = (position.x - vector6.x) * 0.5f;
													float num21 = (position.z - vector6.z) * 0.5f;
													if (num20 * num20 + num21 * num21 < num17)
													{
														flag7 = false;
														break;
													}
													float num22 = (vector4.x - vector6.x) * 0.5f;
													num21 = (vector4.z - vector6.z) * 0.5f;
													if (num22 * num22 + num21 * num21 < num17)
													{
														flag7 = false;
														break;
													}
													vector6 = transform5.TransformPoint(num14, 0f, num15);
													float num23 = (position.x - vector6.x) * 0.5f;
													num21 = (position.z - vector6.z) * 0.5f;
													if (num23 * num23 + num21 * num21 < num17)
													{
														flag7 = false;
														break;
													}
													float num24 = (vector4.x - vector6.x) * 0.5f;
													num21 = (vector4.z - vector6.z) * 0.5f;
													if (num24 * num24 + num21 * num21 < num17)
													{
														flag7 = false;
														break;
													}
												}
											}
											if (!flag7)
											{
												break;
											}
										}
									}
									if (flag7)
									{
										fc.action.Add(transform4);
									}
								}
							}
						}
						if (fc.chr.Count > 1 && fc.action.Count < 2)
						{
							fc.action = new List<Transform>();
						}
						this.StickInit(fc);
						if (fc.handle != null)
						{
							fc.handle.localPosition = Vector3.zero;
						}
					}
					this.FurnitureEffect1006(fc, list2);
					this.FurnitureEffect2006(fc, list2);
					if ((fc.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_PAPER || fc.data.dat.category == TreeHouseFurnitureStatic.Category.FLOOR_PAPER || fc.data.dat.category == TreeHouseFurnitureStatic.Category.CEIL_DECO || fc.data.dat.category == TreeHouseFurnitureStatic.Category.CEIL_LIGHT) && fc.chgmdl)
					{
						fc.mesh = null;
						if (fc.mdl != null)
						{
							Object.Destroy(fc.mdl);
						}
						fc.mdl = null;
						fc.path = null;
						fc.chgmdl = false;
						PrjUtil.ReleaseMemory(PrjUtil.UnloadUnused / 50);
					}
				}
				if (this.IsTop() && fc.data != null && fc.data.dat.machineId > 0)
				{
					this.UpdateMachineDisp(fc, false);
				}
				if (fc.on != null)
				{
					foreach (GameObject gameObject11 in fc.on)
					{
						gameObject11.SetActive(fc.onoff && fc.alpha > 0.1f);
						if (gameObject11.activeSelf && gameObject11.transform.childCount > 0)
						{
							foreach (object obj4 in gameObject11.transform)
							{
								((Transform)obj4).gameObject.SetActive(this.camera.gameObject.activeSelf);
							}
						}
					}
				}
				if (fc.off != null)
				{
					foreach (GameObject gameObject12 in fc.off)
					{
						gameObject12.SetActive(!fc.onoff && fc.alpha > 0.1f);
					}
				}
				if (fc.chr != null)
				{
					Predicate<SceneTreeHouse.ActionCtrl> <>9__18;
					foreach (Transform transform6 in fc.chr)
					{
						if (transform6.childCount > 0)
						{
							bool flag8 = false;
							int num25 = ((fc.action == null) ? (-1) : fc.action.IndexOf(transform6));
							if (num25 >= 0)
							{
								List<SceneTreeHouse.ActionCtrl> list5 = list2;
								Predicate<SceneTreeHouse.ActionCtrl> predicate;
								if ((predicate = <>9__18) == null)
								{
									predicate = (<>9__18 = (SceneTreeHouse.ActionCtrl itm) => itm.furniture == fc.no);
								}
								SceneTreeHouse.ActionCtrl actionCtrl2 = list5.Find(predicate);
								if (actionCtrl2 != null && actionCtrl2.chara.Count > num25 && actionCtrl2.chara[num25] > 0)
								{
									flag8 = true;
								}
							}
							foreach (object obj5 in transform6)
							{
								Transform transform7 = (Transform)obj5;
								if (transform7.name.IndexOf("__on") > 0)
								{
									transform7.gameObject.SetActive(flag8);
								}
								else if (transform7.name.IndexOf("__off") > 0)
								{
									transform7.gameObject.SetActive(!flag8);
								}
							}
						}
					}
				}
				float num26 = (float)((TimeManager.Now.Hour * 60 + TimeManager.Now.Minute) * 60 + TimeManager.Now.Second);
				foreach (Transform transform8 in fc.clockH)
				{
					transform8.localEulerAngles = new Vector3(0f, 0f, -num26 / 120f);
				}
				foreach (Transform transform9 in fc.clockM)
				{
					transform9.localEulerAngles = new Vector3(0f, 0f, -num26 / 10f);
				}
				foreach (Transform transform10 in fc.clockS)
				{
					transform10.localEulerAngles = new Vector3(0f, 0f, -num26 * 6f);
				}
			}
		}
		list.RemoveAll((SceneTreeHouse.FurnitureCtrl itm) => itm.no < 0 && itm.obj == null);
	}

	private void FurnitureEffect1006Init(SceneTreeHouse.FurnitureCtrl fc)
	{
		if (fc.data.dat.charaActionId != 1006)
		{
			return;
		}
		EffectManager.ReqLoadEffect(SceneTreeHouse.eff611051StName, AssetManager.OWNER.TreeHouseStage, 0, null);
		EffectManager.ReqLoadEffect(SceneTreeHouse.eff611051LpName, AssetManager.OWNER.TreeHouseStage, 0, null);
		EffectManager.ReqLoadEffect(SceneTreeHouse.eff611051EnName, AssetManager.OWNER.TreeHouseStage, 0, null);
		EffectManager.ReqLoadEffect(SceneTreeHouse.eff611051EbName, AssetManager.OWNER.TreeHouseStage, 0, null);
		EffectManager.ReqLoadEffect(SceneTreeHouse.eff611051AdName, AssetManager.OWNER.TreeHouseStage, 0, null);
	}

	private bool FurnitureEffect1006Check(SceneTreeHouse.FurnitureCtrl fc)
	{
		return fc.data.dat.charaActionId != 1006 || (EffectManager.IsLoadFinishEffect(SceneTreeHouse.eff611051StName) && EffectManager.IsLoadFinishEffect(SceneTreeHouse.eff611051LpName) && EffectManager.IsLoadFinishEffect(SceneTreeHouse.eff611051EnName) && EffectManager.IsLoadFinishEffect(SceneTreeHouse.eff611051EbName) && EffectManager.IsLoadFinishEffect(SceneTreeHouse.eff611051AdName));
	}

	private void FurnitureEffect1006Term(SceneTreeHouse.FurnitureCtrl fc)
	{
		if (fc.data.dat.charaActionId != 1006)
		{
			return;
		}
		EffectManager.UnloadEffect(SceneTreeHouse.eff611051StName, AssetManager.OWNER.TreeHouseStage);
		EffectManager.UnloadEffect(SceneTreeHouse.eff611051LpName, AssetManager.OWNER.TreeHouseStage);
		EffectManager.UnloadEffect(SceneTreeHouse.eff611051EnName, AssetManager.OWNER.TreeHouseStage);
		EffectManager.UnloadEffect(SceneTreeHouse.eff611051EbName, AssetManager.OWNER.TreeHouseStage);
		EffectManager.UnloadEffect(SceneTreeHouse.eff611051AdName, AssetManager.OWNER.TreeHouseStage);
	}

	private void FurnitureEffect1006(SceneTreeHouse.FurnitureCtrl fc, List<SceneTreeHouse.ActionCtrl> act)
	{
		if (fc.data.dat.charaActionId != 1006)
		{
			return;
		}
		SceneTreeHouse.ActionCtrl actionCtrl = act.Find((SceneTreeHouse.ActionCtrl itm) => itm.furniture == fc.no);
		if (actionCtrl == null || actionCtrl.flag == 0)
		{
			if (fc.eff != null && (fc.eff.IsFinishByAnimation() || fc.eff.EffectName != SceneTreeHouse.eff611051AdName))
			{
				EffectManager.DestroyEffect(fc.eff);
				fc.eff = null;
			}
			if (fc.eff == null)
			{
				fc.eff = EffectManager.InstantiateEffect(SceneTreeHouse.eff611051AdName, fc.mdl.transform, this.stageLocator.layer, 1f);
				if (fc.eff != null)
				{
					fc.eff.PlayEffect(false);
				}
			}
		}
		else if ((actionCtrl.step & 12288) == 0)
		{
			string text = SceneTreeHouse.eff611051StName;
			if (fc.eff != null && (fc.eff.EffectName == text || fc.eff.EffectName == SceneTreeHouse.eff611051LpName))
			{
				if (fc.eff.IsFinishByAnimation())
				{
					EffectManager.DestroyEffect(fc.eff);
					fc.eff = null;
					text = SceneTreeHouse.eff611051LpName;
				}
			}
			else if (fc.eff != null)
			{
				EffectManager.DestroyEffect(fc.eff);
				fc.eff = null;
			}
			if (fc.eff == null)
			{
				fc.eff = EffectManager.InstantiateEffect(text, fc.mdl.transform, this.stageLocator.layer, 1f);
				if (fc.eff != null)
				{
					fc.eff.PlayEffect(false);
				}
			}
		}
		else
		{
			string text2 = ((actionCtrl.flag == 1) ? SceneTreeHouse.eff611051EnName : SceneTreeHouse.eff611051EbName);
			if (fc.eff != null && (fc.eff.EffectName == text2 || fc.eff.EffectName == SceneTreeHouse.eff611051AdName))
			{
				if (fc.eff.IsFinishByAnimation())
				{
					EffectManager.DestroyEffect(fc.eff);
					fc.eff = null;
					text2 = SceneTreeHouse.eff611051AdName;
				}
			}
			else if (fc.eff != null)
			{
				EffectManager.DestroyEffect(fc.eff);
				fc.eff = null;
			}
			if (fc.eff == null)
			{
				fc.eff = EffectManager.InstantiateEffect(text2, fc.mdl.transform, this.stageLocator.layer, 1f);
				if (fc.eff != null)
				{
					fc.eff.PlayEffect(false);
				}
			}
		}
		if (fc.eff != null)
		{
			int num = ((fc.alpha < 1f) ? LayerMask.NameToLayer("FieldEffect") : this.stageLocator.layer);
			if (fc.eff.effectObject.layer != num)
			{
				fc.eff.effectObject.SetLayerRecursively(num);
			}
		}
	}

	private void FurnitureEffect2006Init(SceneTreeHouse.FurnitureCtrl fc)
	{
		if (fc.data.dat.charaActionId != 2006)
		{
			return;
		}
		EffectManager.ReqLoadEffect(SceneTreeHouse.eff611052StName, AssetManager.OWNER.TreeHouseStage, 0, null);
		EffectManager.ReqLoadEffect(SceneTreeHouse.eff611052LpName, AssetManager.OWNER.TreeHouseStage, 0, null);
		EffectManager.ReqLoadEffect(SceneTreeHouse.eff611052EnName, AssetManager.OWNER.TreeHouseStage, 0, null);
		EffectManager.ReqLoadEffect(SceneTreeHouse.eff611052EbName, AssetManager.OWNER.TreeHouseStage, 0, null);
		EffectManager.ReqLoadEffect(SceneTreeHouse.eff611052AdName, AssetManager.OWNER.TreeHouseStage, 0, null);
	}

	private bool FurnitureEffect2006Check(SceneTreeHouse.FurnitureCtrl fc)
	{
		return fc.data.dat.charaActionId != 2006 || (EffectManager.IsLoadFinishEffect(SceneTreeHouse.eff611052StName) && EffectManager.IsLoadFinishEffect(SceneTreeHouse.eff611052LpName) && EffectManager.IsLoadFinishEffect(SceneTreeHouse.eff611052EnName) && EffectManager.IsLoadFinishEffect(SceneTreeHouse.eff611052EbName) && EffectManager.IsLoadFinishEffect(SceneTreeHouse.eff611052AdName));
	}

	private void FurnitureEffect2006Term(SceneTreeHouse.FurnitureCtrl fc)
	{
		if (fc.data.dat.charaActionId != 2006)
		{
			return;
		}
		EffectManager.UnloadEffect(SceneTreeHouse.eff611052StName, AssetManager.OWNER.TreeHouseStage);
		EffectManager.UnloadEffect(SceneTreeHouse.eff611052LpName, AssetManager.OWNER.TreeHouseStage);
		EffectManager.UnloadEffect(SceneTreeHouse.eff611052EnName, AssetManager.OWNER.TreeHouseStage);
		EffectManager.UnloadEffect(SceneTreeHouse.eff611052EbName, AssetManager.OWNER.TreeHouseStage);
		EffectManager.UnloadEffect(SceneTreeHouse.eff611052AdName, AssetManager.OWNER.TreeHouseStage);
	}

	private void FurnitureEffect2006(SceneTreeHouse.FurnitureCtrl fc, List<SceneTreeHouse.ActionCtrl> act)
	{
		if (fc.data.dat.charaActionId != 2006)
		{
			return;
		}
		SceneTreeHouse.ActionCtrl actionCtrl = act.Find((SceneTreeHouse.ActionCtrl itm) => itm.furniture == fc.no);
		if (actionCtrl == null || actionCtrl.step == 0)
		{
			if (fc.eff != null && (fc.eff.IsFinishByAnimation() || fc.eff.EffectName != SceneTreeHouse.eff611052AdName))
			{
				EffectManager.DestroyEffect(fc.eff);
				fc.eff = null;
			}
			if (fc.eff == null)
			{
				fc.eff = EffectManager.InstantiateEffect(SceneTreeHouse.eff611052AdName, fc.mdl.transform, this.stageLocator.layer, 1f);
				if (fc.eff != null)
				{
					fc.eff.PlayEffect(false);
				}
			}
		}
		else if (actionCtrl.step == 1)
		{
			string text = SceneTreeHouse.eff611052StName;
			if (fc.eff != null && (fc.eff.EffectName == text || fc.eff.EffectName == SceneTreeHouse.eff611052LpName))
			{
				if (fc.eff.IsFinishByAnimation())
				{
					EffectManager.DestroyEffect(fc.eff);
					fc.eff = null;
					text = SceneTreeHouse.eff611052LpName;
				}
			}
			else if (fc.eff != null)
			{
				EffectManager.DestroyEffect(fc.eff);
				fc.eff = null;
			}
			if (fc.eff == null)
			{
				fc.eff = EffectManager.InstantiateEffect(text, fc.mdl.transform, this.stageLocator.layer, 1f);
				if (fc.eff != null)
				{
					fc.eff.PlayEffect(false);
				}
			}
		}
		else
		{
			string text2 = ((actionCtrl.flag == 1) ? SceneTreeHouse.eff611052EnName : SceneTreeHouse.eff611052EbName);
			if (fc.eff != null && (fc.eff.EffectName == text2 || fc.eff.EffectName == SceneTreeHouse.eff611052AdName))
			{
				if (fc.eff.IsFinishByAnimation())
				{
					EffectManager.DestroyEffect(fc.eff);
					fc.eff = null;
					text2 = SceneTreeHouse.eff611052AdName;
				}
			}
			else if (fc.eff != null)
			{
				EffectManager.DestroyEffect(fc.eff);
				fc.eff = null;
			}
			if (fc.eff == null)
			{
				fc.eff = EffectManager.InstantiateEffect(text2, fc.mdl.transform, this.stageLocator.layer, 1f);
				if (fc.eff != null)
				{
					fc.eff.PlayEffect(false);
				}
			}
		}
		if (fc.eff != null)
		{
			int num = ((fc.alpha < 1f) ? LayerMask.NameToLayer("FieldEffect") : this.stageLocator.layer);
			if (fc.eff.effectObject.layer != num)
			{
				fc.eff.effectObject.SetLayerRecursively(num);
			}
		}
	}

	private void SetAlpha(SceneTreeHouse.FurnitureCtrl fc)
	{
		float num = fc.alpha;
		if (num < 0f)
		{
			num = 0f;
		}
		this.SetAlpha(fc.mesh, num, fc.no > 0);
		if (fc.board != null)
		{
			float num2 = ((fc.no < 0 || num < 0.49f) ? (num * 0.5f) : ((fc.no == 0) ? 0.25f : ((1f - num) * 0.5f)));
			Color color = ((fc == this.furnitureMove || fc.no < 0) ? (this.guiData.btnOk.ActEnable ? new Color(0f, 0f, 0.5f, num2) : new Color(0.5f, 0.5f, 0f, num2)) : new Color(0.5f, 0f, 0f, num2));
			fc.board.GetComponent<MeshRenderer>().material.SetColor("_TintColor", color);
		}
	}

	private void SetAlpha(MeshRenderer[] mesh, float alpha, bool sdw)
	{
		int num = ((alpha <= 0f) ? SceneTreeHouse.ignoreLayer : ((alpha < 1f) ? SceneTreeHouse.stageAlphaLayer : this.stageLocator.layer));
		foreach (MeshRenderer meshRenderer in mesh)
		{
			meshRenderer.gameObject.layer = num;
			foreach (Material material in meshRenderer.materials)
			{
				if (material.name.Contains("_sdw_"))
				{
					Color color = material.GetColor("_TintColor");
					color.a = ((sdw && meshRenderer.gameObject.layer == this.stageLocator.layer) ? 1f : 0f);
					material.SetColor("_TintColor", color);
				}
				else
				{
					material.SetColor("_Color", new Color(1f, 1f, 1f, alpha));
				}
			}
		}
	}

	private void StickInit(SceneTreeHouse.FurnitureCtrl fc)
	{
		if (fc.stick == null)
		{
			return;
		}
		fc.stickMin = new List<Vector3>();
		fc.stickMax = new List<Vector3>();
		foreach (Transform transform in fc.stick)
		{
			fc.stickMin.Add(new Vector3(999f, 999f, 999f));
			fc.stickMax.Add(new Vector3(-999f, -999f, -999f));
			foreach (object obj in transform)
			{
				Transform transform2 = (Transform)obj;
				transform2.gameObject.SetActive(transform2.name.StartsWith("stick_m"));
			}
		}
	}

	private void StickCtrl(SceneTreeHouse.FurnitureCtrl fc, int typ, Vector3 pos)
	{
		if (fc.stick == null || typ >= fc.stick.Count || typ < -fc.stick.Count)
		{
			return;
		}
		float num = 0f;
		float num2 = 0f;
		if (typ < 0)
		{
			typ = -1 - typ;
		}
		else
		{
			fc.stickMin[typ] = Vector3.Min(fc.stickMin[typ], pos);
			fc.stickMax[typ] = Vector3.Max(fc.stickMax[typ], pos);
			float num3 = (fc.stickMin[typ].y + fc.stickMax[typ].y) * 0.5f;
			string text = "stick_s";
			float num4 = 0.83753f;
			float num5 = 0.86171f;
			float num6 = 0.88575f;
			if (num3 > (num5 + num6) * 0.5f)
			{
				text = "stick_l";
			}
			else if (num3 > (num4 + num5) * 0.5f)
			{
				text = "stick_m";
			}
			Vector3 vector = pos - fc.stickMin[typ];
			Vector3 vector2 = fc.stickMax[typ] - fc.stickMin[typ];
			Vector3 vector3 = vector2 * 0.7f;
			vector2 *= 0.3f;
			if (vector.z > vector3.z)
			{
				num = 15f;
			}
			else if (vector.z < vector2.z)
			{
				num = -15f;
			}
			if (vector.x > vector3.x)
			{
				num2 = -15f;
			}
			else if (vector.x < vector2.x)
			{
				num2 = 15f;
			}
			foreach (object obj in fc.stick[typ])
			{
				Transform transform = (Transform)obj;
				transform.gameObject.SetActive(transform.name.StartsWith(text));
			}
		}
		fc.stick[typ].localEulerAngles = new Vector3(num, 0f, num2);
	}

	private void CtrlPaper()
	{
		using (List<SceneTreeHouse.PaperCtrl>.Enumerator enumerator = this.paperList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				SceneTreeHouse.PaperCtrl pc = enumerator.Current;
				bool flag = false;
				if (this.stageCtrl == null && this.stageLoad == null)
				{
					pc.enable = false;
					flag = true;
				}
				else
				{
					SceneTreeHouse.FurnitureCtrl furnitureCtrl = this.furnitureList.Find((SceneTreeHouse.FurnitureCtrl itm) => itm.no > 0 && itm.data.dat.category == pc.cat);
					if (furnitureCtrl == null)
					{
						furnitureCtrl = this.otherFurniture.Find((SceneTreeHouse.FurnitureCtrl itm) => itm.no > 0 && itm.data.dat.category == pc.cat);
					}
					pc.enable = furnitureCtrl == null;
					if (!pc.enable && furnitureCtrl.mdl != null && !furnitureCtrl.chgmdl)
					{
						flag = true;
					}
				}
				if (pc.mdl == null)
				{
					if (string.IsNullOrEmpty(pc.path))
					{
						if (pc.enable)
						{
							pc.path = pc.mdlnam;
							if (this.IsNight)
							{
								pc.path = pc.path.Replace("_a_mdl", "_b_mdl");
							}
							AssetManager.LoadAssetData(pc.path = SceneTreeHouse.FURNITURE_PATH + pc.path, AssetManager.OWNER.TreeHouseStage, 0, null);
						}
					}
					else if (AssetManager.IsLoadFinishAssetData(pc.path))
					{
						pc.mdl = AssetManager.InstantiateAssetData(pc.path, null);
						pc.mdl.transform.SetParent(this.field.transform, false);
						pc.mdl.SetLayerRecursively(this.stageLocator.layer);
						pc.mdl.SetActive(true);
						pc.mesh = pc.mdl.GetComponentsInChildren<MeshRenderer>(true);
						AssetManager.AddLoadList(pc.path, AssetManager.OWNER.TreeHouseStage);
						this.SetAlpha(pc.mesh, 1f, true);
					}
				}
				else if (pc.enable)
				{
					this.SetAlpha(pc.mesh, (this.furnitureMove != null && pc.cat == this.furnitureMove.data.dat.category && this.furnitureMove.mdl != null) ? 0f : 1f, true);
					if (pc.chgmdl)
					{
						pc.mesh = null;
						if (pc.mdl != null)
						{
							Object.Destroy(pc.mdl);
						}
						pc.mdl = null;
						pc.path = null;
						pc.chgmdl = false;
						PrjUtil.ReleaseMemory(PrjUtil.UnloadUnused / 50);
					}
				}
				else if (flag)
				{
					pc.mesh = null;
					if (pc.mdl != null)
					{
						Object.Destroy(pc.mdl);
					}
					pc.mdl = null;
					pc.path = null;
					pc.chgmdl = false;
					PrjUtil.ReleaseMemory(PrjUtil.UnloadUnused / 50);
				}
				else
				{
					this.SetAlpha(pc.mesh, 1f, true);
				}
			}
		}
		if (this.stageCtrl == null && this.stageLoad == null)
		{
			this.paperList.RemoveAll((SceneTreeHouse.PaperCtrl itm) => !itm.enable && itm.mdl == null);
		}
	}

	private void CtrlChara(bool other)
	{
		float num = 1f;
		float num2 = (this.camBCG.enabled ? 1f : (TimeManager.DeltaTime * 4f));
		if (!other && (this.furnitureMove != null || this.isMove > 0))
		{
			num = 0f;
		}
		List<SceneTreeHouse.FurnitureCtrl> list = (other ? this.otherFurniture : this.furnitureList);
		List<SceneTreeHouse.CharaCtrl> list2 = (other ? this.otherChara : this.charaList);
		List<SceneTreeHouse.ActionCtrl> list3 = (other ? this.otherAction : this.actionList);
		using (List<SceneTreeHouse.CharaCtrl>.Enumerator enumerator = list2.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				SceneTreeHouse.CharaCtrl cc = enumerator.Current;
				if (cc.no < 0)
				{
					if (cc.eff != null)
					{
						EffectManager.DestroyEffect(cc.eff);
					}
					cc.eff = null;
					if (cc.cup != null)
					{
						EffectManager.DestroyEffect(cc.cup);
					}
					cc.cup = null;
					if (cc.stk != null)
					{
						EffectManager.DestroyEffect(cc.stk);
					}
					cc.stk = null;
					cc.stkL = (cc.stkR = 99999f);
					foreach (EffectData effectData in cc.onp)
					{
						EffectManager.DestroyEffect(effectData);
					}
					cc.onp = new List<EffectData>();
					foreach (SceneTreeHouse.CharaCtrl.Effect effect in cc.ring)
					{
						if (effect.eff != null)
						{
							EffectManager.DestroyEffect(effect.eff);
						}
					}
					cc.ring = new List<SceneTreeHouse.CharaCtrl.Effect>();
					foreach (SceneTreeHouse.CharaCtrl.Effect effect2 in cc.ball)
					{
						if (effect2.eff != null)
						{
							EffectManager.DestroyEffect(effect2.eff);
						}
					}
					cc.ball = new List<SceneTreeHouse.CharaCtrl.Effect>();
					if (cc.hdl != null)
					{
						float num3 = cc.hdl.GetAlpha() - num2;
						if (num3 > 0f)
						{
							cc.hdl.SetAlpha(num3);
						}
						else
						{
							if (!string.IsNullOrEmpty(cc.hdl.loadVoiceCueSheetName))
							{
								SoundManager.SetPosition(cc.hdl.loadVoiceCueSheetName, null, 0f);
							}
							Object.Destroy(cc.hdl.gameObject);
							cc.hdl = null;
							cc.chara = null;
						}
					}
				}
				else if (cc.hdl == null)
				{
					int num4 = ((other && !DataManager.DmUserInfo.optionData.ViewClothesAffect) ? 0 : cc.chara.equipClothImageId);
					DataManagerCharaAccessory.Accessory accessory = (cc.chara.dynamicData.dispAccessoryEffect ? cc.chara.dynamicData.accessory : null);
					if (accessory != null && other && !DataManager.DmUserInfo.optionData.ViewClothesAffect)
					{
						accessory = null;
					}
					cc.hdl = new GameObject((other ? "O" : "M") + cc.no.ToString() + "-" + cc.chara.id.ToString(), new Type[] { typeof(CharaModelHandle) }).GetComponent<CharaModelHandle>();
					cc.hdl.transform.SetParent(this.charaBase.transform, false);
					cc.hdl.Initialize(cc.chara.id, true, false, num4, num4 > 0 && cc.chara.equipLongSkirt, true, false, accessory);
					cc.hdl.SetLayer(SceneTreeHouse.charaLayer);
					cc.hdl.SetModelActive(false);
					cc.hdl.SetWeaponActive(false);
					cc.hdl.shadowSize = 0.6f;
					cc.hdl.SetAlpha(0f);
					cc.hdl.PlayAnimation(CharaMotionDefine.ActKey.MYR_STAND_BY, false, 1f, 0f, 0f, false);
				}
				else if (cc.hdl.IsFinishInitialize() && !cc.hdl.IsModelActive())
				{
					cc.hdl.SetModelActive(true);
					cc.hdl.SetEnableUpdateOffscreen();
				}
				else if (cc.hdl.IsFinishInitialize())
				{
					float num5 = cc.hdl.GetAlpha();
					float num6 = num;
					CharaMotionDefine.ActKey currentAnimation = cc.hdl.GetCurrentAnimation();
					bool flag = cc.hdl.IsLoopAnimation();
					cc.hdl.IsPlaying();
					float num7 = Random.Range(0.975f, 1.025f);
					SceneTreeHouse.ActionCtrl ac = list3.Find((SceneTreeHouse.ActionCtrl itm) => itm.chara.Contains(cc.no));
					SceneTreeHouse.FurnitureCtrl furnitureCtrl = ((ac == null) ? null : list.Find((SceneTreeHouse.FurnitureCtrl itm) => itm.no == ac.furniture));
					int num8 = ((ac == null) ? (-1) : ac.chara.IndexOf(cc.no));
					List<VOICE_TYPE> list4 = null;
					Transform transform = ((furnitureCtrl == null || num8 < 0 || num8 >= furnitureCtrl.action.Count) ? null : furnitureCtrl.action[num8]);
					if (transform == null)
					{
						num6 = 0f;
						cc.act = 0;
						cc.idx = -1;
						cc.spe = false;
						cc.tlk = false;
						foreach (SceneTreeHouse.CharaCtrl.Effect effect3 in cc.ring)
						{
							if (effect3.eff != null)
							{
								EffectManager.DestroyEffect(effect3.eff);
							}
						}
						cc.ring = new List<SceneTreeHouse.CharaCtrl.Effect>();
						foreach (SceneTreeHouse.CharaCtrl.Effect effect4 in cc.ball)
						{
							if (effect4.eff != null)
							{
								EffectManager.DestroyEffect(effect4.eff);
							}
						}
						cc.ball = new List<SceneTreeHouse.CharaCtrl.Effect>();
					}
					else
					{
						if (furnitureCtrl.alpha < 0f)
						{
							num6 = 0f;
						}
						else if (furnitureCtrl.data.dat.charaActionId != 1004 && furnitureCtrl.data.dat.charaActionId != 1005 && furnitureCtrl.data.dat.charaActionId != 2004 && furnitureCtrl.data.dat.charaActionId != 2005 && furnitureCtrl.data.dat.charaActionId != 2028 && num6 > furnitureCtrl.alpha)
						{
							num6 = furnitureCtrl.alpha;
						}
						num8 = (int)(transform.name[transform.name.Length - 1] - 'a');
						if (cc.act != furnitureCtrl.data.dat.charaActionId || cc.idx != num8)
						{
							cc.itvl = Random.Range(0.5f, 2f);
							cc.act = furnitureCtrl.data.dat.charaActionId;
							cc.idx = num8;
							cc.spe = DataManager.DmTreeHouse.IsSpecialActionByChara(cc.act, cc.chara.id, cc.chara.dynamicData.equipClothesId);
							cc.tlk = Random.Range(0, 2) == 0;
							foreach (SceneTreeHouse.CharaCtrl.Effect effect5 in cc.ring)
							{
								if (effect5.eff != null)
								{
									EffectManager.DestroyEffect(effect5.eff);
								}
							}
							cc.ring = new List<SceneTreeHouse.CharaCtrl.Effect>();
							foreach (SceneTreeHouse.CharaCtrl.Effect effect6 in cc.ball)
							{
								if (effect6.eff != null)
								{
									EffectManager.DestroyEffect(effect6.eff);
								}
							}
							cc.ball = new List<SceneTreeHouse.CharaCtrl.Effect>();
							if (cc.eff != null)
							{
								EffectManager.DestroyEffect(cc.eff);
							}
							cc.eff = null;
							if (cc.cup != null)
							{
								EffectManager.DestroyEffect(cc.cup);
							}
							cc.cup = null;
							if (cc.stk != null)
							{
								EffectManager.DestroyEffect(cc.stk);
							}
							cc.stk = null;
							cc.stkL = (cc.stkR = 99999f);
							foreach (EffectData effectData2 in cc.onp)
							{
								EffectManager.DestroyEffect(effectData2);
							}
							cc.onp = new List<EffectData>();
						}
						cc.hdl.transform.position = transform.position;
						cc.hdl.transform.rotation = transform.rotation;
						this.CtrlCharaActionLoop(cc, ref currentAnimation, ref flag);
						this.CtrlCharaActionSynchro(cc, ref currentAnimation, ref flag, ac);
						this.CtrlCharaActionAlone(cc, ref currentAnimation, ref flag, ac);
						this.CtrlCharaActionSequence(cc, ref currentAnimation, ref flag, ac);
						this.CtrlCharaAction1006(cc, ref currentAnimation, ref flag, ac, furnitureCtrl, num6);
						this.CtrlCharaAction2006(cc, ref currentAnimation, ref flag, ac, furnitureCtrl, num6);
						this.CtrlCharaActionWanage(cc, ref currentAnimation, ref flag, ac, furnitureCtrl, num6, ref list4);
						this.CtrlCharaActionBall(cc, ref currentAnimation, ref flag, ac, furnitureCtrl, num6, ref list4);
						this.CtrlCharaAction1023(cc, currentAnimation, furnitureCtrl);
						this.CtrlCharaEffect(cc, currentAnimation, ac, transform, num6);
					}
					if (!cc.hdl.IsCurrentAnimation(currentAnimation))
					{
						if (ac != null)
						{
							if (ac.motSpd < 0f)
							{
								ac.motSpd = num7;
							}
							else
							{
								num7 = ac.motSpd;
							}
						}
						cc.hdl.PlayAnimation(currentAnimation, flag, flag ? num7 : 1f, (num5 > 0.1f) ? 0.2f : (-1f), (num5 > 0.1f) ? 0.1f : (-1f), false);
						using (List<KeyValuePair<List<CharaMotionDefine.ActKey>, List<VOICE_TYPE>>>.Enumerator enumerator4 = SceneTreeHouse.voiceMot.GetEnumerator())
						{
							while (enumerator4.MoveNext())
							{
								KeyValuePair<List<CharaMotionDefine.ActKey>, List<VOICE_TYPE>> keyValuePair = enumerator4.Current;
								if (keyValuePair.Key == null || keyValuePair.Key.Contains(currentAnimation))
								{
									list4 = keyValuePair.Value;
									break;
								}
							}
							goto IL_0E56;
						}
						goto IL_0CDA;
					}
					goto IL_0CDA;
					IL_0E56:
					if (num5 < num6)
					{
						if ((num5 += num2) > num6)
						{
							num5 = num6;
						}
					}
					else if (num5 > num6 && (num5 -= num2) < num6)
					{
						num5 = num6;
					}
					cc.hdl.SetAlpha(num5);
					if (num5 < 1f)
					{
						if (cc.eff != null)
						{
							EffectManager.DestroyEffect(cc.eff);
						}
						cc.eff = null;
						if (cc.cup != null)
						{
							EffectManager.DestroyEffect(cc.cup);
						}
						cc.cup = null;
						if (cc.stk != null)
						{
							EffectManager.DestroyEffect(cc.stk);
						}
						cc.stk = null;
						cc.stkL = (cc.stkR = 99999f);
						foreach (EffectData effectData3 in cc.onp)
						{
							EffectManager.DestroyEffect(effectData3);
						}
						cc.onp = new List<EffectData>();
					}
					else if (list4 != null && list4.Count > 0 && this.gyroCam != null)
					{
						cc.voice = list4[Random.Range(0, list4.Count)];
						SoundManager.SetPosition(cc.hdl.loadVoiceCueSheetName, cc.hdl.GetNodeTransform("j_mouth"), 30f);
						SoundManager.PlayVoice(cc.hdl.loadVoiceCueSheetName, cc.voice);
						cc.voiceTime = -0.01f - SoundManager.GetVoiceLength(cc.hdl.loadVoiceCueSheetName, cc.voice);
					}
					cc.hdl.DispAccessory(0, this.camera.gameObject.activeSelf, false);
					List<EffectData> charaEffect = cc.hdl.charaEffect;
					if (charaEffect != null && charaEffect.Count > 0 && charaEffect[0] != null && charaEffect[0].effectObject.layer != SceneTreeHouse.bloomLayer)
					{
						charaEffect[0].effectObject.SetLayerRecursively(SceneTreeHouse.bloomLayer);
						continue;
					}
					continue;
					IL_0CDA:
					if (cc.voiceTime < 0f)
					{
						cc.voiceTime = ((SceneTreeHouse.gam1_mot.Contains(currentAnimation) || SceneTreeHouse.gam2_mot.Contains(currentAnimation) || SceneTreeHouse.game_mot.Contains(currentAnimation)) ? (0.5f + Random.Range(0f, 1.5f)) : (2f + Random.Range(0f, 8f))) - cc.voiceTime;
						goto IL_0E56;
					}
					if ((cc.voiceTime -= TimeManager.DeltaTime) < 0f)
					{
						cc.voiceTime = 1f + Random.Range(0f, 1f);
						Predicate<VOICE_TYPE> <>9__3;
						foreach (KeyValuePair<List<CharaMotionDefine.ActKey>, List<VOICE_TYPE>> keyValuePair2 in SceneTreeHouse.voiceList)
						{
							if (keyValuePair2.Key == null || keyValuePair2.Key.Contains(currentAnimation))
							{
								if (keyValuePair2.Value == null || keyValuePair2.Value.Count <= 0)
								{
									break;
								}
								list4 = new List<VOICE_TYPE>(keyValuePair2.Value);
								if (list4.Count > 1)
								{
									List<VOICE_TYPE> list5 = list4;
									Predicate<VOICE_TYPE> predicate;
									if ((predicate = <>9__3) == null)
									{
										predicate = (<>9__3 = (VOICE_TYPE itm) => itm == cc.voice);
									}
									list5.RemoveAll(predicate);
									break;
								}
								break;
							}
						}
						goto IL_0E56;
					}
					goto IL_0E56;
				}
			}
		}
		list2.RemoveAll((SceneTreeHouse.CharaCtrl itm) => itm.no < 0 && itm.hdl == null);
	}

	private void CtrlCharaActionLoop(SceneTreeHouse.CharaCtrl cc, ref CharaMotionDefine.ActKey mot, ref bool lp)
	{
		if (cc.act == 1001)
		{
			mot = SceneTreeHouse.chat_mot[cc.idx];
		}
		else if (cc.act == 1002)
		{
			mot = ((cc.noHand || cc.spe || cc.tlk) ? SceneTreeHouse.talk_mot[cc.idx] : SceneTreeHouse.tea_mot[cc.idx]);
		}
		else if (cc.act == 1003)
		{
			mot = ((cc.noHand || cc.spe) ? SceneTreeHouse.talk_mot[cc.idx] : SceneTreeHouse.play_mot[cc.idx]);
		}
		else if (cc.act == 1009)
		{
			mot = SceneTreeHouse.sleep_mot[cc.idx];
		}
		else if (cc.act == 1011)
		{
			mot = SceneTreeHouse.book_mot[cc.idx];
		}
		else if (cc.act == 1012)
		{
			mot = SceneTreeHouse.kitchen_mot[cc.idx];
		}
		else if (cc.act == 1014)
		{
			mot = SceneTreeHouse.sleep_mot[cc.idx];
		}
		else if (cc.act == 1015)
		{
			mot = SceneTreeHouse.diary_mot[cc.idx];
		}
		else if (cc.act == 1016)
		{
			mot = ((cc.noHand || cc.spe) ? SceneTreeHouse.gardenn_mot : SceneTreeHouse.garden_mot[cc.idx]);
		}
		else if (cc.act == 1017)
		{
			mot = SceneTreeHouse.xylo_mot[cc.idx];
		}
		else if (cc.act == 1018)
		{
			mot = ((cc.noHand || cc.spe || cc.tlk) ? SceneTreeHouse.talk_mot[cc.idx] : SceneTreeHouse.curry_mot[cc.idx]);
		}
		else if (cc.act == 1019)
		{
			mot = SceneTreeHouse.tent_mot[cc.idx];
		}
		else if (cc.act == 1020)
		{
			mot = SceneTreeHouse.cook_mot;
		}
		else if (cc.act == 1021)
		{
			mot = ((cc.noHand || cc.spe) ? SceneTreeHouse.talk_mot[cc.idx] : SceneTreeHouse.jpntea_mot[cc.idx]);
		}
		else if (cc.act == 1022)
		{
			mot = SceneTreeHouse.sweets_mot[cc.idx];
		}
		else if (cc.act == 1023)
		{
			mot = ((cc.noHand || cc.spe) ? SceneTreeHouse.carn_mot : SceneTreeHouse.car_mot[cc.idx]);
		}
		else if (cc.act == 1024)
		{
			mot = ((cc.noHand || cc.spe) ? SceneTreeHouse.talk_mot[cc.idx] : SceneTreeHouse.sand_mot[cc.idx]);
		}
		else if (cc.act == 2009)
		{
			mot = (SceneTreeHouse.bed_mot.Contains(mot) ? mot : SceneTreeHouse.bed_mot[Random.Range(0, SceneTreeHouse.bed_mot.Count)]);
		}
		else if (cc.act == 2012)
		{
			mot = SceneTreeHouse.greentea_mot;
		}
		else if (cc.act == 2015)
		{
			mot = SceneTreeHouse.banner_mot[cc.idx];
		}
		else if (cc.act == 2019)
		{
			mot = SceneTreeHouse.kakizome_mot[cc.idx];
		}
		else if (cc.act == 2016 || cc.act == 2018 || cc.act == 2020)
		{
			mot = SceneTreeHouse.garden_mot[0];
		}
		else if (cc.act == 2017)
		{
			mot = SceneTreeHouse.xylo1_mot;
		}
		else
		{
			if (cc.act != 2029)
			{
				return;
			}
			mot = SceneTreeHouse.doll_mot;
		}
		lp = true;
	}

	private void CtrlCharaActionSynchro(SceneTreeHouse.CharaCtrl cc, ref CharaMotionDefine.ActKey mot, ref bool lp, SceneTreeHouse.ActionCtrl ac)
	{
		if (cc.act != 1004 && cc.act != 2028)
		{
			return;
		}
		List<CharaMotionDefine.ActKey> list = ((cc.idx == 0) ? SceneTreeHouse.drw1_mot : ((cc.noHand || cc.spe) ? SceneTreeHouse.drw2n_mot : SceneTreeHouse.drw2_mot));
		CharaMotionDefine.ActKey actKey = SceneTreeHouse.drwe_mot;
		bool flag = !cc.hdl.IsPlaying();
		lp = false;
		List<int> list3;
		if (cc.idx != 0)
		{
			List<int> list2 = new List<int>();
			list2.Add(2);
			list2.Add(32);
			list2.Add(512);
			list2.Add(1);
			list2.Add(16);
			list3 = list2;
			list2.Add(256);
		}
		else
		{
			List<int> list4 = new List<int>();
			list4.Add(1);
			list4.Add(16);
			list4.Add(256);
			list4.Add(2);
			list4.Add(32);
			list3 = list4;
			list4.Add(512);
		}
		List<int> list5 = list3;
		if ((ac.step & list5[1]) != 0)
		{
			if ((ac.step & list5[4]) != 0)
			{
				ac.step |= list5[2];
			}
			int num = list.IndexOf(mot);
			if (num < 0 && mot != actKey)
			{
				ac.step &= ~(list5[0] | list5[1] | list5[2]);
			}
			else if (flag)
			{
				if (mot == actKey)
				{
					ac.step &= ~(list5[0] | list5[1] | list5[2]);
				}
				else if (++num < list.Count)
				{
					mot = list[num];
				}
				else
				{
					mot = actKey;
				}
			}
			if ((ac.step & list5[1]) == 0)
			{
				cc.itvl = Random.Range(0.5f, 2f);
			}
			return;
		}
		if ((cc.itvl -= TimeManager.DeltaTime) > 0f || (ac.step & (list5[3] | list5[4])) == 0 || (ac.step & list5[5]) != 0)
		{
			if (cc.itvl <= 0f)
			{
				ac.step |= list5[0];
			}
			mot = CharaMotionDefine.ActKey.MYR_STAND_BY;
			lp = true;
			return;
		}
		ac.step |= list5[1];
		ac.step &= ~list5[0];
		mot = list[0];
	}

	private void CtrlCharaActionAlone(SceneTreeHouse.CharaCtrl cc, ref CharaMotionDefine.ActKey mot, ref bool lp, SceneTreeHouse.ActionCtrl ac)
	{
		if (cc.act != 2004)
		{
			return;
		}
		List<CharaMotionDefine.ActKey> list = SceneTreeHouse.drw1_mot;
		CharaMotionDefine.ActKey actKey = SceneTreeHouse.drwe_mot;
		bool flag = !cc.hdl.IsPlaying();
		lp = false;
		if (ac.step != 0)
		{
			int num = list.IndexOf(mot);
			if (num < 0 && mot != actKey)
			{
				ac.step = 0;
			}
			else if (flag)
			{
				if (mot == actKey)
				{
					ac.step = 0;
				}
				else if (++num < list.Count)
				{
					mot = list[num];
				}
				else
				{
					mot = actKey;
				}
			}
			if (ac.step == 0)
			{
				cc.itvl = Random.Range(0.5f, 2f);
			}
			return;
		}
		if ((cc.itvl -= TimeManager.DeltaTime) > 0f)
		{
			mot = CharaMotionDefine.ActKey.MYR_STAND_BY;
			lp = true;
			return;
		}
		ac.step = 1;
		mot = list[0];
	}

	private void CtrlCharaActionSequence(SceneTreeHouse.CharaCtrl cc, ref CharaMotionDefine.ActKey mot, ref bool lp, SceneTreeHouse.ActionCtrl ac)
	{
		List<CharaMotionDefine.ActKey> list;
		if (cc.act == 1005)
		{
			list = ((cc.idx == 0) ? ((cc.noHand || cc.spe) ? SceneTreeHouse.cos1n_mot : SceneTreeHouse.cos1_mot) : ((cc.noHand || cc.spe) ? SceneTreeHouse.cos2n_mot : SceneTreeHouse.cos2_mot));
		}
		else
		{
			if (cc.act != 2005)
			{
				return;
			}
			list = ((cc.noHand || cc.spe) ? SceneTreeHouse.cos1n_mot : SceneTreeHouse.cos1_mot);
		}
		bool flag = !cc.hdl.IsPlaying();
		if ((ac.step & (1 << cc.idx)) == 0)
		{
			if ((cc.itvl -= TimeManager.DeltaTime) > 0f)
			{
				mot = CharaMotionDefine.ActKey.MYR_STAND_BY;
				lp = true;
				return;
			}
			mot = list[0];
			lp = false;
			ac.step |= 1 << cc.idx;
			return;
		}
		else
		{
			int num = list.IndexOf(mot);
			if (num < 0 || (flag && ++num >= list.Count))
			{
				ac.step &= ~(1 << cc.idx);
				cc.itvl = Random.Range(0.5f, 2f);
				return;
			}
			mot = list[num];
			lp = false;
			return;
		}
	}

	private void CtrlCharaAction1006(SceneTreeHouse.CharaCtrl cc, ref CharaMotionDefine.ActKey mot, ref bool lp, SceneTreeHouse.ActionCtrl ac, SceneTreeHouse.FurnitureCtrl fc, float a)
	{
		if (cc.act != 1006)
		{
			return;
		}
		bool flag = !cc.hdl.IsPlaying();
		lp = false;
		if (cc.idx == 0)
		{
			List<CharaMotionDefine.ActKey> list = ((cc.noHand || cc.spe) ? SceneTreeHouse.gamn_mot : SceneTreeHouse.gam1_mot);
			if ((ac.step & 272) == 0)
			{
				if ((cc.itvl -= TimeManager.DeltaTime) > 0f || (ac.step & 34) == 0 || (ac.step & 512) != 0)
				{
					if (cc.itvl <= 0f)
					{
						ac.step |= 1;
					}
					mot = CharaMotionDefine.ActKey.MYR_STAND_BY;
					lp = true;
				}
				else
				{
					ac.step |= 16;
					ac.step &= -2;
					mot = list[0];
					ac.flag = ((cc.noHand || cc.spe) ? 0 : Random.Range(1, 3));
				}
			}
			else if ((ac.step & 256) == 0)
			{
				int num = list.IndexOf(mot);
				if (num < 0)
				{
					ac.step &= -4370;
				}
				else if (flag)
				{
					if (++num < list.Count)
					{
						mot = list[num];
						if (!cc.noHand && !cc.spe && num + 1 >= list.Count)
						{
							ac.step |= 4096;
						}
					}
					else if (cc.noHand || cc.spe)
					{
						ac.step &= -4370;
					}
					else
					{
						mot = SceneTreeHouse.game_mot[ac.flag - 1];
						ac.step |= 256;
					}
				}
			}
			else if (!SceneTreeHouse.game_mot.Contains(mot) || flag)
			{
				ac.step &= -4370;
				cc.itvl = Random.Range(0.5f, 2f);
			}
		}
		else
		{
			List<CharaMotionDefine.ActKey> list2 = ((cc.noHand || cc.spe) ? SceneTreeHouse.gamn_mot : SceneTreeHouse.gam2_mot);
			if ((ac.step & 32) == 0)
			{
				if ((cc.itvl -= TimeManager.DeltaTime) > 0f || (ac.step & 17) == 0 || (ac.step & 256) != 0)
				{
					if (cc.itvl <= 0f)
					{
						ac.step |= 2;
					}
					mot = CharaMotionDefine.ActKey.MYR_STAND_BY;
					lp = true;
				}
				else
				{
					ac.step |= 32;
					ac.step &= -3;
					mot = list2[0];
				}
			}
			else if ((ac.step & 512) == 0)
			{
				int num2 = list2.IndexOf(mot);
				if (num2 < 0)
				{
					ac.step &= -8739;
				}
				else if (flag)
				{
					if (++num2 < list2.Count)
					{
						mot = list2[num2];
						if (!cc.noHand && !cc.spe && num2 + 1 >= list2.Count)
						{
							ac.step |= 8192;
						}
					}
					else if (cc.noHand || cc.spe)
					{
						ac.step &= -8739;
					}
					else
					{
						mot = SceneTreeHouse.game_mot[2 - ac.flag];
						ac.step |= 512;
					}
				}
			}
			else if (!SceneTreeHouse.game_mot.Contains(mot) || flag)
			{
				ac.step &= -8739;
				cc.itvl = Random.Range(0.5f, 2f);
			}
		}
		bool flag2 = mot == CharaMotionDefine.ActKey.MYR_GAME_1MOT_LP_A || mot == CharaMotionDefine.ActKey.MYR_GAME_2MOT_LP_A || mot == CharaMotionDefine.ActKey.MYR_GAME_1MOT_LP_B || mot == CharaMotionDefine.ActKey.MYR_GAME_2MOT_LP_B;
		if (a < 1f || !cc.hdl.IsCurrentAnimation(mot))
		{
			flag2 = false;
		}
		this.StickCtrl(fc, flag2 ? cc.idx : (-1 - cc.idx), flag2 ? cc.hdl.transform.InverseTransformPoint(cc.hdl.GetNodePos("j_wrist_l")) : Vector3.zero);
	}

	private void CtrlCharaAction2006(SceneTreeHouse.CharaCtrl cc, ref CharaMotionDefine.ActKey mot, ref bool lp, SceneTreeHouse.ActionCtrl ac, SceneTreeHouse.FurnitureCtrl fc, float a)
	{
		if (cc.act != 2006)
		{
			return;
		}
		bool flag = !cc.hdl.IsPlaying();
		lp = false;
		List<CharaMotionDefine.ActKey> list = SceneTreeHouse.gam1_mot;
		if (ac.step == 0)
		{
			if ((cc.itvl -= TimeManager.DeltaTime) > 0f)
			{
				mot = CharaMotionDefine.ActKey.MYR_STAND_BY;
				lp = true;
			}
			else
			{
				ac.step = 1;
				mot = list[0];
				ac.flag = Random.Range(1, 3);
			}
		}
		else if (ac.step == 1)
		{
			int num = list.IndexOf(mot);
			if (num < 0)
			{
				ac.step = 0;
			}
			else if (flag)
			{
				if (++num < list.Count)
				{
					mot = list[num];
				}
				else
				{
					mot = SceneTreeHouse.game_mot[ac.flag - 1];
					ac.step = 2;
				}
			}
		}
		else if (!SceneTreeHouse.game_mot.Contains(mot) || flag)
		{
			ac.step = 0;
			cc.itvl = Random.Range(0.5f, 2f);
		}
		bool flag2 = mot == CharaMotionDefine.ActKey.MYR_GAME_1MOT_LP_A || mot == CharaMotionDefine.ActKey.MYR_GAME_1MOT_LP_B;
		if (a < 1f || !cc.hdl.IsCurrentAnimation(mot))
		{
			flag2 = false;
		}
		this.StickCtrl(fc, flag2 ? 0 : (-1), flag2 ? cc.hdl.transform.InverseTransformPoint(cc.hdl.GetNodePos("j_wrist_l")) : Vector3.zero);
	}

	private void CtrlCharaActionWanage(SceneTreeHouse.CharaCtrl cc, ref CharaMotionDefine.ActKey mot, ref bool lp, SceneTreeHouse.ActionCtrl ac, SceneTreeHouse.FurnitureCtrl fc, float a, ref List<VOICE_TYPE> voc)
	{
		if (cc.act != 1010 && cc.act != 1026)
		{
			return;
		}
		string text = "";
		if (cc.act == 1010)
		{
			text = SceneTreeHouse.ringEffName;
		}
		else if (cc.act == 1026)
		{
			text = SceneTreeHouse.ringNewYearEffName;
		}
		bool flag = !cc.hdl.IsPlaying();
		lp = false;
		List<CharaMotionDefine.ActKey> list = ((cc.idx == 0) ? SceneTreeHouse.ring_mot : SceneTreeHouse.see_mot);
		List<CharaMotionDefine.ActKey> list2 = SceneTreeHouse.ringe_mot;
		List<SceneTreeHouse.CharaCtrl.Effect> ring = cc.ring;
		List<int> list4;
		if (cc.idx != 0)
		{
			List<int> list3 = new List<int>();
			list3.Add(2);
			list3.Add(32);
			list3.Add(512);
			list3.Add(1);
			list3.Add(16);
			list4 = list3;
			list3.Add(256);
		}
		else
		{
			List<int> list5 = new List<int>();
			list5.Add(1);
			list5.Add(16);
			list5.Add(256);
			list5.Add(2);
			list5.Add(32);
			list4 = list5;
			list5.Add(512);
		}
		List<int> list6 = list4;
		if ((ac.step & list6[1]) == 0)
		{
			if ((cc.itvl -= TimeManager.DeltaTime) > 0f || (ac.step & (list6[3] | list6[4])) == 0 || (ac.step & list6[5]) != 0)
			{
				if (cc.itvl <= 0f)
				{
					ac.step |= list6[0];
				}
				mot = CharaMotionDefine.ActKey.MYR_STAND_BY;
				lp = true;
			}
			else
			{
				ac.step |= list6[1];
				ac.step &= ~list6[0];
				mot = list[0];
			}
		}
		else
		{
			if ((ac.step & list6[4]) != 0)
			{
				ac.step |= list6[2];
			}
			int num = list.IndexOf(mot);
			if (num < 0 && !list2.Contains(mot))
			{
				ac.step &= ~(list6[0] | list6[1] | list6[2]);
			}
			else if (flag)
			{
				if (list2.Contains(mot))
				{
					ac.step &= ~(list6[0] | list6[1] | list6[2]);
				}
				else if (++num < list.Count)
				{
					mot = list[num];
				}
				else
				{
					mot = list2[((ac.step & 4096) == 0) ? 0 : 1];
				}
			}
			else if (cc.idx > 0 && (ac.step & (list6[3] | list6[4] | list6[5])) == 0 && !list2.Contains(mot))
			{
				mot = list2[((ac.step & 4096) == 0) ? 0 : 1];
			}
			if ((ac.step & list6[1]) == 0)
			{
				cc.itvl = Random.Range(0.5f, 2f);
			}
		}
		if (cc.idx != 0)
		{
			return;
		}
		while (ring.Count < list.Count)
		{
			ring.Add(new SceneTreeHouse.CharaCtrl.Effect
			{
				idx = ring.Count,
				bone = this.field.transform,
				eff = null,
				mesh = null,
				alpha = 0f,
				bas = Vector3.zero,
				tag = Vector3.zero,
				typ = -1,
				tim = 0f
			});
		}
		int num2 = list.IndexOf(mot);
		float num3 = cc.hdl.GetAnimationTime(mot.ToString()) * cc.hdl.GetAnimationLength(mot.ToString()) * 30f;
		List<float> list7 = new List<float> { 20f, 33f, 45f, 130f, 230f, 218f };
		Vector3 vector = new Vector3(-0.05f, -0.01f, 0.11f);
		bool flag2 = false;
		List<bool> list8 = new List<bool> { false, false, false, false, false, false, false, false, false, false };
		ac.step &= -61441;
		foreach (SceneTreeHouse.CharaCtrl.Effect effect in ring)
		{
			int num4 = -1;
			if (effect.idx == num2)
			{
				if (num3 >= list7[num2 + 3])
				{
					num4 = 1;
				}
				else if (num3 >= list7[num2])
				{
					num4 = 0;
				}
			}
			else if (num2 != 0)
			{
				num4 = effect.typ;
			}
			if (num4 > 0)
			{
				if (effect.typ <= 0)
				{
					effect.typ = Random.Range(0, 9) + 1;
					Transform transform = fc.mdl.transform.Find("pos_throwing_target_" + effect.typ.ToString());
					effect.tag = ((transform == null) ? fc.mdl.transform : transform).position;
					effect.bas = effect.bone.TransformPoint(vector);
					effect.tim = 0f;
					effect.bone = this.field.transform;
					voc = SceneTreeHouse.voiceTrw;
				}
				if ((effect.tim += TimeManager.DeltaTime * 2f) > 1f)
				{
					effect.tim = 1f;
				}
			}
			else if (effect.typ != num4)
			{
				effect.bone = (((effect.typ = num4) < 0) ? this.field.transform : cc.hdl.GetNodeTransform("j_wrist_r"));
			}
			if (a < 1f)
			{
				effect.mesh = null;
				if (effect.eff != null)
				{
					EffectManager.DestroyEffect(effect.eff);
				}
				effect.eff = null;
			}
			else
			{
				if (effect.eff == null)
				{
					effect.eff = EffectManager.InstantiateEffect(text, this.field.transform, SceneTreeHouse.charaLayer, 1f);
					effect.eff.PlayEffect(false);
					effect.mesh = effect.eff.effectObject.GetComponentsInChildren<MeshRenderer>(true);
					effect.alpha = 0f;
				}
				if (effect.bone != effect.eff.effectObject.transform.parent)
				{
					effect.eff.effectObject.transform.SetParent(effect.bone, false);
					if (effect.bone != this.field.transform)
					{
						effect.eff.effectObject.transform.localPosition = vector;
					}
					effect.eff.effectObject.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
					effect.alpha = 1f;
				}
				if (effect.bone == this.field.transform)
				{
					if (effect.typ > 0)
					{
						effect.alpha = 1f;
						if (effect.tim < 1f)
						{
							Vector3 vector2 = Vector3.Lerp(effect.bas, effect.tag, effect.tim);
							vector2.y = (0.5f + (effect.tag.y - effect.bas.y - 0.5f) * effect.tim) * effect.tim + effect.bas.y;
							effect.eff.effectObject.transform.position = vector2;
							effect.eff.effectObject.transform.localEulerAngles = new Vector3(0f, effect.tim * 700f, 0f);
						}
						else
						{
							Transform transform2 = fc.mdl.transform.Find("pos_throwing_target_" + effect.typ.ToString());
							if (transform2 == null)
							{
								effect.eff.effectObject.transform.position = new Vector3(0f, -10f, 0f);
							}
							else
							{
								float num5 = 0.01f;
								float num6 = -0.045f;
								int i;
								int ii;
								for (ii = 0; ii < effect.idx; ii = i + 1)
								{
									if (ring.Find((SceneTreeHouse.CharaCtrl.Effect itm) => itm.idx == ii).typ == effect.typ)
									{
										num5 += 0.02f;
										num6 += 0.005f;
									}
									i = ii;
								}
								num5 /= transform2.localScale.y;
								effect.eff.effectObject.transform.position = transform2.TransformPoint(0f, num5, num6);
								effect.eff.effectObject.transform.rotation = transform2.rotation;
								effect.eff.effectObject.transform.Rotate(0f, (float)(effect.idx * 9 + effect.typ) * 10f, 0f);
							}
						}
					}
					else if (effect.alpha > 0f && (effect.alpha -= TimeManager.DeltaTime * 2f) <= 0f)
					{
						effect.alpha = 0f;
						effect.eff.effectObject.transform.position = new Vector3(0f, -10f, 0f);
					}
				}
				int num7 = ((effect.alpha < 1f) ? ((effect.alpha > 0f) ? SceneTreeHouse.charaAlphaLayer : SceneTreeHouse.ignoreLayer) : SceneTreeHouse.charaLayer);
				if (effect.eff.effectObject.layer != num7)
				{
					effect.eff.effectObject.SetLayerRecursively(num7);
				}
				MeshRenderer[] mesh = effect.mesh;
				for (int i = 0; i < mesh.Length; i++)
				{
					Material[] materials = mesh[i].materials;
					for (int j = 0; j < materials.Length; j++)
					{
						materials[j].SetFloat("_Alpha", effect.alpha);
					}
				}
			}
			num4 = effect.typ;
			if (num4 < 0 || num4 >= list8.Count)
			{
				num4 = 0;
			}
			if (num4 > 0 && list8[num4])
			{
				flag2 = true;
			}
			list8[num4] = true;
		}
		if (flag2)
		{
			ac.step |= 4096;
			return;
		}
		if ((list8[1] & list8[2] & list8[3]) || (list8[4] & list8[5] & list8[6]) || (list8[7] & list8[8] & list8[9]))
		{
			ac.step |= 8192;
			return;
		}
		if ((list8[1] & list8[4] & list8[7]) || (list8[2] & list8[5] & list8[8]) || (list8[3] & list8[6] & list8[9]))
		{
			ac.step |= 16384;
			return;
		}
		if ((list8[1] & list8[5] & list8[9]) || (list8[3] & list8[5] & list8[7]))
		{
			ac.step |= 32768;
		}
	}

	private void CtrlCharaActionBall(SceneTreeHouse.CharaCtrl cc, ref CharaMotionDefine.ActKey mot, ref bool lp, SceneTreeHouse.ActionCtrl ac, SceneTreeHouse.FurnitureCtrl fc, float a, ref List<VOICE_TYPE> voc)
	{
		if (cc.act != 1013 && cc.act != 1027)
		{
			return;
		}
		bool flag = !cc.hdl.IsPlaying();
		lp = false;
		List<CharaMotionDefine.ActKey> list = ((cc.idx == 0) ? SceneTreeHouse.ball_mot : SceneTreeHouse.see_mot);
		List<CharaMotionDefine.ActKey> list2 = SceneTreeHouse.balle_mot;
		List<SceneTreeHouse.CharaCtrl.Effect> ball = cc.ball;
		List<int> list4;
		if (cc.idx != 0)
		{
			List<int> list3 = new List<int>();
			list3.Add(2);
			list3.Add(32);
			list3.Add(512);
			list3.Add(1);
			list3.Add(16);
			list4 = list3;
			list3.Add(256);
		}
		else
		{
			List<int> list5 = new List<int>();
			list5.Add(1);
			list5.Add(16);
			list5.Add(256);
			list5.Add(2);
			list5.Add(32);
			list4 = list5;
			list5.Add(512);
		}
		List<int> list6 = list4;
		if ((ac.step & list6[1]) == 0)
		{
			if ((cc.itvl -= TimeManager.DeltaTime) > 0f || (ac.step & (list6[3] | list6[4])) == 0 || (ac.step & list6[5]) != 0)
			{
				if (cc.itvl <= 0f)
				{
					ac.step |= list6[0];
				}
				mot = CharaMotionDefine.ActKey.MYR_STAND_BY;
				lp = true;
			}
			else
			{
				ac.step |= list6[1];
				ac.step &= ~list6[0];
				mot = list[0];
			}
		}
		else
		{
			if ((ac.step & list6[4]) != 0)
			{
				ac.step |= list6[2];
			}
			int num = list.IndexOf(mot);
			if (num < 0 && !list2.Contains(mot))
			{
				ac.step &= ~(list6[0] | list6[1] | list6[2]);
			}
			else if (flag)
			{
				if (list2.Contains(mot))
				{
					ac.step &= ~(list6[0] | list6[1] | list6[2]);
				}
				else if (++num < list.Count)
				{
					mot = list[num];
				}
				else
				{
					mot = list2[((ac.step & 4096) == 0) ? 0 : 1];
				}
			}
			else if (cc.idx > 0 && (ac.step & (list6[3] | list6[4] | list6[5])) == 0 && !list2.Contains(mot))
			{
				mot = list2[((ac.step & 4096) == 0) ? 0 : 1];
			}
			if ((ac.step & list6[1]) == 0)
			{
				cc.itvl = Random.Range(0.5f, 2f);
			}
		}
		if (cc.idx != 0)
		{
			return;
		}
		while (ball.Count < list.Count)
		{
			ball.Add(new SceneTreeHouse.CharaCtrl.Effect
			{
				idx = ball.Count,
				bone = this.field.transform,
				eff = null,
				mesh = null,
				alpha = 0f,
				bas = Vector3.zero,
				tag = Vector3.zero,
				typ = -1,
				tim = 0f
			});
		}
		int num2 = list.IndexOf(mot);
		float num3 = cc.hdl.GetAnimationTime(mot.ToString()) * cc.hdl.GetAnimationLength(mot.ToString()) * 30f;
		List<float> list7 = new List<float> { 20f, 45f, 43f, 127f, 221f, 216f };
		Vector3 vector = new Vector3(-0.08f, 0.01f, 0.05f);
		bool flag2 = false;
		List<bool> list8 = new List<bool> { false, false, false, false, false, false, false, false, false, false };
		ac.step &= -61441;
		foreach (SceneTreeHouse.CharaCtrl.Effect effect in ball)
		{
			int num4 = -1;
			if (effect.idx == num2)
			{
				if (num3 >= list7[num2 + 3])
				{
					num4 = 1;
				}
				else if (num3 >= list7[num2])
				{
					num4 = 0;
				}
			}
			else if (num2 != 0)
			{
				num4 = effect.typ;
			}
			if (num4 > 0)
			{
				if (effect.typ <= 0)
				{
					effect.typ = Random.Range(0, 9) + 1;
					Transform transform = fc.mdl.transform.Find("pos_throwing_target_" + effect.typ.ToString());
					Matrix4x4 identity = Matrix4x4.identity;
					identity.SetTRS(((transform == null) ? fc.mdl.transform : transform).position, Quaternion.Euler(0f, fc.obj.transform.eulerAngles.y, 0f), Vector3.one);
					float num5 = ((transform == null) ? 0.6f : (transform.localScale.x * 0.004f));
					effect.tag = identity.MultiplyPoint3x4(new Vector3(Random.Range(-num5, num5), Random.Range(-num5, num5), 0f));
					effect.bas = effect.bone.TransformPoint(vector);
					effect.tim = 0f;
					effect.bone = this.field.transform;
					voc = SceneTreeHouse.voiceTrw;
				}
				if ((effect.tim += TimeManager.DeltaTime * 2f) > 1f)
				{
					effect.tim = 1f;
				}
			}
			else if (effect.typ != num4)
			{
				effect.bone = (((effect.typ = num4) < 0) ? this.field.transform : cc.hdl.GetNodeTransform("j_wrist_r"));
			}
			if (a < 1f)
			{
				effect.mesh = null;
				if (effect.eff != null)
				{
					EffectManager.DestroyEffect(effect.eff);
				}
				effect.eff = null;
			}
			else
			{
				if (effect.eff == null)
				{
					if (cc.act == 1013)
					{
						effect.eff = EffectManager.InstantiateEffect(SceneTreeHouse.ballEffName, this.field.transform, SceneTreeHouse.charaLayer, 1f);
					}
					else
					{
						effect.eff = EffectManager.InstantiateEffect(SceneTreeHouse.ballBasketEffName, this.field.transform, SceneTreeHouse.charaLayer, 1f);
					}
					effect.eff.effectObject.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
					effect.eff.PlayEffect(false);
					effect.mesh = effect.eff.effectObject.GetComponentsInChildren<MeshRenderer>(true);
					effect.alpha = 0f;
				}
				if (effect.bone != effect.eff.effectObject.transform.parent)
				{
					effect.eff.effectObject.transform.SetParent(effect.bone, false);
					if (effect.bone != this.field.transform)
					{
						effect.eff.effectObject.transform.localPosition = vector;
					}
					effect.alpha = 1f;
				}
				if (effect.bone == this.field.transform)
				{
					if (effect.typ > 0)
					{
						effect.alpha = 1f;
						Vector3 vector2 = Vector3.Lerp(effect.bas, effect.tag, effect.tim);
						vector2.y = (1.5f + (effect.tag.y - effect.bas.y - 1.5f) * effect.tim) * effect.tim + effect.bas.y;
						effect.eff.effectObject.transform.position = vector2;
					}
					else if (effect.alpha > 0f && (effect.alpha -= TimeManager.DeltaTime * 2f) < 0f)
					{
						effect.alpha = 0f;
						effect.eff.effectObject.transform.position = new Vector3(0f, -10f, 0f);
					}
				}
				int num6 = ((effect.alpha < 1f) ? ((effect.alpha > 0f) ? SceneTreeHouse.charaAlphaLayer : SceneTreeHouse.ignoreLayer) : SceneTreeHouse.charaLayer);
				if (effect.eff.effectObject.layer != num6)
				{
					effect.eff.effectObject.SetLayerRecursively(num6);
				}
				MeshRenderer[] mesh = effect.mesh;
				for (int i = 0; i < mesh.Length; i++)
				{
					Material[] materials = mesh[i].materials;
					for (int j = 0; j < materials.Length; j++)
					{
						materials[j].SetFloat("_Alpha", effect.alpha);
					}
				}
			}
			num4 = effect.typ;
			if (num4 < 0 || num4 >= list8.Count)
			{
				num4 = 0;
			}
			if (num4 > 0 && list8[num4])
			{
				flag2 = true;
			}
			list8[num4] = true;
		}
		if (flag2)
		{
			ac.step |= 4096;
			return;
		}
		if ((list8[1] & list8[2] & list8[3]) || (list8[4] & list8[5] & list8[6]) || (list8[7] & list8[8] & list8[9]))
		{
			ac.step |= 8192;
			return;
		}
		if ((list8[1] & list8[4] & list8[7]) || (list8[2] & list8[5] & list8[8]) || (list8[3] & list8[6] & list8[9]))
		{
			ac.step |= 16384;
			return;
		}
		if ((list8[1] & list8[5] & list8[9]) || (list8[3] & list8[5] & list8[7]))
		{
			ac.step |= 32768;
		}
	}

	private void CtrlCharaAction1023(SceneTreeHouse.CharaCtrl cc, CharaMotionDefine.ActKey mot, SceneTreeHouse.FurnitureCtrl fc)
	{
		if (cc.act != 1023)
		{
			return;
		}
		if (mot != CharaMotionDefine.ActKey.MYR_CAR_1)
		{
			return;
		}
		if (fc.handle == null)
		{
			return;
		}
		if (fc.handle.childCount != 1)
		{
			return;
		}
		Transform nodeTransform = cc.hdl.GetNodeTransform("j_index_b_l");
		Transform nodeTransform2 = cc.hdl.GetNodeTransform("j_index_b_r");
		if (nodeTransform == null || nodeTransform2 == null)
		{
			return;
		}
		if (Mathf.Abs(nodeTransform.position.y - nodeTransform2.position.y) > 0.005f)
		{
			return;
		}
		Vector3 vector = (nodeTransform.position + nodeTransform2.position) * 0.5f;
		vector.y -= 0.03f;
		Vector3 vector2 = fc.handle.localPosition;
		fc.handle.localPosition = Vector3.zero;
		vector -= fc.handle.GetChild(0).position;
		float y = vector.y;
		vector.y = 0f;
		float num = vector.magnitude;
		float y2 = vector2.y;
		vector2.y = 0f;
		float num2 = vector2.magnitude;
		if (num < 0.2f && (num2 == 0f || num2 > num))
		{
			vector2 = vector;
		}
		num = Mathf.Abs(y);
		num2 = Mathf.Abs(y2);
		vector2.y = ((num < 0.2f && (num2 == 0f || num2 > num)) ? y : y2);
		fc.handle.localPosition = vector2;
	}

	private void CtrlCharaEffect(SceneTreeHouse.CharaCtrl cc, CharaMotionDefine.ActKey mot, SceneTreeHouse.ActionCtrl ac, Transform tmp, float a)
	{
		if (cc.eff == null)
		{
			if (a >= 1f && cc.hdl.IsCurrentAnimation(mot) && this.camera.gameObject.activeSelf)
			{
				if (mot == CharaMotionDefine.ActKey.MYR_GAME_1MOT_LP_B || mot == CharaMotionDefine.ActKey.MYR_GAME_2MOT_LP_B)
				{
					cc.eff = EffectManager.InstantiateEffect(SceneTreeHouse.emoEffName[SceneTreeHouse.EmoTyp.GAME], this.field.transform, SceneTreeHouse.charaLayer, 1f);
					cc.eff.PlayEffect(false);
					cc.eff.effectObject.transform.position = cc.hdl.GetNodePos("j_head");
					cc.eff.effectObject.transform.eulerAngles = Vector3.zero;
					cc.eff.effectObject.transform.localScale = Vector3.one;
				}
				else if ((SceneTreeHouse.chat_mot.Contains(mot) || SceneTreeHouse.talk_mot.Contains(mot) || SceneTreeHouse.tea_mot.Contains(mot) || SceneTreeHouse.play_mot.Contains(mot) || SceneTreeHouse.curry_mot.Contains(mot) || SceneTreeHouse.tent_mot.Contains(mot) || SceneTreeHouse.jpntea_mot.Contains(mot) || SceneTreeHouse.sweets_mot.Contains(mot) || SceneTreeHouse.sand_mot.Contains(mot)) && cc.tim > 0f)
				{
					if ((cc.tim -= TimeManager.DeltaTime) <= 0f)
					{
						cc.eff = EffectManager.InstantiateEffect(SceneTreeHouse.emoEffName[SceneTreeHouse.EmoTyp.SPEAK], this.field.transform, SceneTreeHouse.charaLayer, 1f);
						cc.eff.PlayEffect(false);
						cc.eff.effectObject.transform.position = cc.hdl.GetNodePos("j_head");
						cc.eff.effectObject.transform.eulerAngles = Vector3.zero;
						cc.eff.effectObject.transform.localScale = Vector3.one;
					}
				}
				else if ((SceneTreeHouse.sleep_mot.Contains(mot) || SceneTreeHouse.bed_mot.Contains(mot)) && cc.tim > 0f)
				{
					if ((cc.tim -= TimeManager.DeltaTime) <= 0f)
					{
						cc.eff = EffectManager.InstantiateEffect(SceneTreeHouse.emoEffName[SceneTreeHouse.EmoTyp.SLEEP], this.field.transform, SceneTreeHouse.charaLayer, 1f);
						cc.eff.PlayEffect(false);
						cc.eff.effectObject.transform.position = cc.hdl.GetNodePos("j_head");
						cc.eff.effectObject.transform.eulerAngles = Vector3.zero;
						cc.eff.effectObject.transform.localScale = Vector3.one;
					}
				}
				else if (((cc.act == 1010 || cc.act == 1026) && SceneTreeHouse.ringe_mot.Contains(mot)) || ((cc.act == 1013 || cc.act == 1027) && SceneTreeHouse.balle_mot.Contains(mot)))
				{
					if (cc.tim > 0f)
					{
						SceneTreeHouse.EmoTyp emoTyp = SceneTreeHouse.EmoTyp.INVALID;
						if ((ac.step & 4096) != 0)
						{
							emoTyp = SceneTreeHouse.EmoTyp.SUPPRISE;
						}
						else if ((ac.step & 57344) != 0)
						{
							emoTyp = ((cc.idx > 0) ? SceneTreeHouse.EmoTyp.STROKE : SceneTreeHouse.EmoTyp.HAPPY);
						}
						if (emoTyp != SceneTreeHouse.EmoTyp.INVALID)
						{
							cc.eff = EffectManager.InstantiateEffect(SceneTreeHouse.emoEffName[emoTyp], this.field.transform, SceneTreeHouse.charaLayer, 1f);
							cc.eff.PlayEffect(false);
							cc.eff.effectObject.transform.position = cc.hdl.GetNodePos("j_head");
							cc.eff.effectObject.transform.eulerAngles = Vector3.zero;
							cc.eff.effectObject.transform.localScale = Vector3.one;
						}
						cc.tim = -1f;
					}
				}
				else if (SceneTreeHouse.xylo_mot[0] != mot && SceneTreeHouse.xylo1_mot != mot)
				{
					cc.tim = Random.Range(0f, 2f) + 2f;
				}
			}
		}
		else if (cc.eff.IsFinishByAnimation() || a < 1f || !cc.hdl.IsCurrentAnimation(mot) || !this.camera.gameObject.activeSelf)
		{
			if (cc.eff != null)
			{
				EffectManager.DestroyEffect(cc.eff);
			}
			cc.eff = null;
		}
		Transform nodeTransform = cc.hdl.GetNodeTransform("j_wrist_r");
		Transform nodeTransform2 = cc.hdl.GetNodeTransform("j_wrist_l");
		Transform transform = null;
		foreach (object obj in tmp)
		{
			Transform transform2 = (Transform)obj;
			if (transform2.name.StartsWith("pos_tea_cup"))
			{
				transform = transform2;
			}
			else if (transform2.name.StartsWith("action_obj_") && transform2.name.EndsWith("__on"))
			{
				transform = transform2;
			}
		}
		string text = null;
		Vector3 vector = Vector3.zero;
		Vector3 vector2 = Vector3.zero;
		if (nodeTransform != null)
		{
			if (SceneTreeHouse.tea_mot.Contains(mot))
			{
				text = SceneTreeHouse.cupEffName;
				vector = new Vector3(-0.06f, -0.04f, 0.05f);
				vector2 = new Vector3(-130f, 90f, 0f);
			}
			else if (SceneTreeHouse.drw1_mot.Contains(mot))
			{
				text = ((cc.act == 2028) ? SceneTreeHouse.penEasterEffName : SceneTreeHouse.penEffName);
				vector = new Vector3(-0.057f, -0.033f, 0.029f);
				vector2 = new Vector3(0f, 6.3f, 0f);
			}
			else if (SceneTreeHouse.kitchen_mot[0] == mot)
			{
				text = SceneTreeHouse.potEffName;
				vector = new Vector3(-0.08f, 0.02f, 0.02f);
				vector2 = new Vector3(180f, 30f, 0f);
			}
			else if (SceneTreeHouse.diary_mot.Contains(mot))
			{
				text = SceneTreeHouse.penEffName;
				vector = new Vector3(-0.07f, -0.045f, 0f);
				vector2 = new Vector3(-25f, 0f, 0f);
			}
			else if (SceneTreeHouse.banner_mot.Contains(mot))
			{
				text = ((cc.idx == 0) ? SceneTreeHouse.mkr1EffName : SceneTreeHouse.mkr2EffName);
				vector = new Vector3(-0.07f, -0.045f, 0f);
				vector2 = new Vector3(-25f, 0f, 0f);
			}
			else if (SceneTreeHouse.kakizome_mot.Contains(mot))
			{
				text = ((cc.idx == 0) ? SceneTreeHouse.mkr1YewYearEffName : SceneTreeHouse.mkr2YewYearEffName);
				vector = new Vector3(-0.07f, -0.045f, 0f);
				vector2 = new Vector3(-25f, 0f, 0f);
			}
			else if (SceneTreeHouse.garden_mot[0] == mot)
			{
				if (cc.act == 2018)
				{
					text = SceneTreeHouse.gdnChristmasEffName;
				}
				else if (cc.act == 2020)
				{
					text = SceneTreeHouse.gdnTinplateEffName;
				}
				else
				{
					text = SceneTreeHouse.gdnEffName;
				}
				vector = new Vector3(-0.05f, 0f, 0.025f);
				vector2 = new Vector3(0f, -90f, 180f);
			}
			else if (SceneTreeHouse.xylo_mot[0] == mot || SceneTreeHouse.xylo1_mot == mot)
			{
				text = SceneTreeHouse.stkEffName;
				vector = new Vector3(-0.08f, -0.04f, 0.03f);
				vector2 = new Vector3(90f, 0f, 60f);
			}
			else if (SceneTreeHouse.curry_mot.Contains(mot) || SceneTreeHouse.sweets_mot.Contains(mot))
			{
				text = SceneTreeHouse.spnEffName;
				vector = new Vector3(-0.06f, -0.0275f, 0.04f);
				vector2 = new Vector3(5f, 180f, 180f);
			}
			else if (SceneTreeHouse.greentea_mot == mot)
			{
				text = SceneTreeHouse.grnEffName;
				vector = new Vector3(-0.06f, -0.025f, 0.025f);
				vector2 = new Vector3(15f, -190f, 145f);
			}
			else if (SceneTreeHouse.cook_mot == mot)
			{
				text = SceneTreeHouse.ladleEffName;
				vector = new Vector3(-0.06f, 0f, 0.03f);
				vector2 = new Vector3(0f, 0f, 155f);
			}
			else if (SceneTreeHouse.jpntea_mot.Contains(mot))
			{
				text = SceneTreeHouse.jpncupEffName;
				vector = new Vector3(-0.09f, -0.015f, 0.055f);
				vector2 = new Vector3(170f, -100f, 10f);
			}
		}
		string text2 = null;
		Vector3 vector3 = Vector3.zero;
		Vector3 vector4 = Vector3.zero;
		if (transform != null)
		{
			if (cc.act == 1002)
			{
				text2 = SceneTreeHouse.cupEffName;
				vector3 = new Vector3(0.08f, 0.06f, 0f);
			}
			else if (cc.act == 1018)
			{
				text2 = SceneTreeHouse.spnEffName;
				vector3 = new Vector3(0.1f, 0.025f, 0f);
				vector4 = new Vector3(0f, 60f, 0f);
			}
			else if (cc.act == 1021)
			{
				text2 = SceneTreeHouse.jpncupEffName;
			}
		}
		foreach (TreeHouseSmallFurnitureData treeHouseSmallFurnitureData in this.smallFurnitureDataList.FindAll((TreeHouseSmallFurnitureData item) => item.reactionId == cc.act))
		{
			TreeHouseSmallFurnitureData.MotionData motionData = treeHouseSmallFurnitureData.motionDataList[cc.idx];
			if (motionData != null && cc.hdl.GetNodeTransform(motionData.nodeName) != null && mot == motionData.actKey)
			{
				text = motionData.modelName;
				vector = motionData.havePos;
				vector2 = motionData.haveRot;
				break;
			}
			foreach (TreeHouseSmallFurnitureData.OptData optData in treeHouseSmallFurnitureData.optDataList)
			{
				if (transform != null)
				{
					text2 = optData.modelName;
					vector3 = optData.putPos;
					vector4 = optData.putRot;
				}
			}
		}
		if (!string.IsNullOrEmpty(text))
		{
			if (a >= 1f)
			{
				if (cc.cup == null)
				{
					cc.cup = EffectManager.InstantiateEffect(text, this.field.transform, SceneTreeHouse.charaLayer, 1f);
					cc.cup.PlayEffect(false);
				}
				if (cc.cup.effectObject.transform.parent != nodeTransform)
				{
					cc.cup.effectObject.transform.SetParent(nodeTransform, false);
					cc.cup.effectObject.transform.localPosition = vector;
					cc.cup.effectObject.transform.localEulerAngles = vector2;
				}
				if (text == SceneTreeHouse.gdnEffName || text == SceneTreeHouse.gdnChristmasEffName || text == SceneTreeHouse.gdnTinplateEffName)
				{
					ParticleSystem componentInChildren = cc.cup.effectObject.GetComponentInChildren<ParticleSystem>(true);
					if (componentInChildren != null)
					{
						componentInChildren.gameObject.SetActive(nodeTransform.position.y > componentInChildren.transform.position.y);
					}
				}
			}
		}
		else if (!string.IsNullOrEmpty(text2))
		{
			if (a >= 1f)
			{
				if (cc.cup == null)
				{
					cc.cup = EffectManager.InstantiateEffect(text2, this.field.transform, SceneTreeHouse.charaLayer, 1f);
					cc.cup.PlayEffect(false);
				}
				if (cc.cup.effectObject.transform.parent != transform)
				{
					cc.cup.effectObject.transform.SetParent(transform, false);
					cc.cup.effectObject.transform.localPosition = vector3;
					cc.cup.effectObject.transform.localEulerAngles = vector4;
				}
			}
		}
		else
		{
			if (cc.cup != null)
			{
				EffectManager.DestroyEffect(cc.cup);
			}
			cc.cup = null;
		}
		if ((SceneTreeHouse.xylo_mot[0] == mot || SceneTreeHouse.xylo1_mot == mot) && nodeTransform2 != null)
		{
			if (a >= 1f)
			{
				if (cc.stk == null)
				{
					cc.stk = EffectManager.InstantiateEffect(SceneTreeHouse.stkEffName, this.field.transform, SceneTreeHouse.charaLayer, 1f);
					cc.stk.PlayEffect(false);
				}
				if (cc.stk.effectObject.transform.parent != nodeTransform2)
				{
					cc.stk.effectObject.transform.SetParent(nodeTransform2, false);
					cc.stk.effectObject.transform.localPosition = new Vector3(-0.08f, -0.04f, -0.03f);
					cc.stk.effectObject.transform.localEulerAngles = new Vector3(90f, 0f, 60f);
				}
			}
		}
		else
		{
			if (cc.stk != null)
			{
				EffectManager.DestroyEffect(cc.stk);
			}
			cc.stk = null;
		}
		if (SceneTreeHouse.xylo_mot[0] == mot || SceneTreeHouse.xylo1_mot == mot)
		{
			if (a >= 1f)
			{
				List<EffectData> rmv = cc.onp.FindAll((EffectData itm) => itm.IsFinishByAnimation());
				foreach (EffectData effectData in rmv)
				{
					EffectManager.DestroyEffect(effectData);
				}
				cc.onp.RemoveAll((EffectData itm) => rmv.Contains(itm));
				bool flag = false;
				if (nodeTransform != null)
				{
					if (cc.stkR > 0f)
					{
						if (cc.stkR > nodeTransform.position.y)
						{
							cc.stkR = nodeTransform.position.y;
						}
						else if (cc.stkR + 0.1f < nodeTransform.position.y)
						{
							cc.stkR = -cc.stkR;
						}
					}
					else if (0.05f - cc.stkR > nodeTransform.position.y)
					{
						cc.stkR = -cc.stkR;
						flag = true;
					}
				}
				if (nodeTransform2 != null)
				{
					if (cc.stkL > 0f)
					{
						if (cc.stkL > nodeTransform2.position.y)
						{
							cc.stkL = nodeTransform2.position.y;
						}
						else if (cc.stkL + 0.1f < nodeTransform2.position.y)
						{
							cc.stkL = -cc.stkL;
						}
					}
					else if (0.05f - cc.stkL > nodeTransform2.position.y)
					{
						cc.stkL = -cc.stkL;
						flag = true;
					}
				}
				if (flag)
				{
					EffectData effectData2 = EffectManager.InstantiateEffect(SceneTreeHouse.onpEffName, this.field.transform, SceneTreeHouse.charaLayer, 1f);
					effectData2.PlayEffect(false);
					effectData2.effectObject.transform.position = cc.hdl.GetNodeTransform("pelvis").TransformPoint(0f, -0.25f, 0.45f);
					effectData2.effectObject.transform.eulerAngles = Vector3.zero;
					effectData2.effectObject.transform.localScale = Vector3.one;
					cc.onp.Add(effectData2);
					return;
				}
			}
		}
		else
		{
			cc.stkL = (cc.stkR = 99999f);
			foreach (EffectData effectData3 in cc.onp)
			{
				EffectManager.DestroyEffect(effectData3);
			}
			cc.onp = new List<EffectData>();
		}
	}

	private Vector3 ChkFloorColli(Vector3 prv, Vector3 nxt, float sx, float sz)
	{
		bool flag = sx > 0f;
		bool flag2 = !flag;
		bool flag3 = sz > 0f;
		bool flag4 = !flag3;
		float num = nxt.x + sx;
		float num2 = nxt.z + sz;
		num = (flag ? Mathf.Ceil(num) : Mathf.Floor(num));
		num2 = (flag3 ? Mathf.Ceil(num2) : Mathf.Floor(num2));
		float num3 = (flag ? (-0.1f) : 0.1f);
		float num4 = (flag3 ? (-0.1f) : 0.1f);
		while ((flag || flag2 || flag3 || flag4) && !this.ChkFloorGrid(new Vector3(num + num3, 0f, num2 + num4)))
		{
			if (flag)
			{
				float num5 = num - 1f;
				if (this.ChkFloorGrid(new Vector3(num5 + num3, 0f, num2 + num4)))
				{
					nxt.x = num5 - sx;
					break;
				}
			}
			else if (flag2)
			{
				float num6 = num + 1f;
				if (this.ChkFloorGrid(new Vector3(num6 + num3, 0f, num2 + num4)))
				{
					nxt.x = num6 - sx;
					break;
				}
			}
			if (flag3)
			{
				float num7 = num2 - 1f;
				if (this.ChkFloorGrid(new Vector3(num + num3, 0f, num7 + num4)))
				{
					nxt.z = num7 - sz;
					break;
				}
			}
			else if (flag4)
			{
				float num8 = num2 + 1f;
				if (this.ChkFloorGrid(new Vector3(num + num3, 0f, num8 + num4)))
				{
					nxt.z = num8 - sz;
					break;
				}
			}
			if (flag)
			{
				flag = (num -= 1f) > prv.x;
			}
			else if (flag2)
			{
				flag2 = (num += 1f) < prv.x;
			}
			if (flag3)
			{
				flag3 = (num2 -= 1f) > prv.z;
			}
			else if (flag4)
			{
				flag4 = (num2 += 1f) < prv.z;
			}
			nxt.x = num - sx;
			nxt.z = num2 - sz;
		}
		return nxt;
	}

	private bool ChkFloorGrid(Vector3 p)
	{
		bool flag = false;
		p = this.floorGrid.transform.TransformPoint(p / 4f);
		RaycastHit[] array = Physics.RaycastAll(new Ray(new Vector3(p.x, 1f, p.z), new Vector3(0f, -1f, 0f)), 2f);
		int num = 0;
		while (!flag && num < array.Length)
		{
			if (array[num].transform == this.floorGrid.transform)
			{
				flag = true;
			}
			num++;
		}
		return flag;
	}

	private Vector3 ChkWallColli(Vector3 prv, Vector3 nxt, float sx, float sy)
	{
		bool flag = sx > 0f;
		bool flag2 = sy > 0f;
		float num = nxt.x + sx;
		float num2 = nxt.y + sy;
		num = (flag ? Mathf.Ceil(num) : Mathf.Floor(num));
		num2 = (flag2 ? Mathf.Ceil(num2) : Mathf.Floor(num2));
		float num3 = (flag ? (-0.1f) : 0.1f);
		float num4 = (flag2 ? (-0.1f) : 0.1f);
		while ((nxt.z = this.ChkWallGrid(new Vector3(num + num3, num2 + num4, prv.z))) < -999f)
		{
			if ((nxt.z = this.ChkWallGrid(new Vector3(prv.x, num2 + num4, prv.z))) > -999f)
			{
				while (flag ? ((num -= 1f) > prv.x) : ((num += 1f) < prv.x))
				{
					nxt.x = num - sx;
					if ((nxt.z = this.ChkWallGrid(new Vector3(num + num3, num2 + num4, prv.z))) > -999f)
					{
						break;
					}
				}
				break;
			}
			if ((nxt.z = this.ChkWallGrid(new Vector3(num + num3, prv.y, prv.z))) > -999f)
			{
				while (flag2 ? ((num2 -= 1f) > prv.y) : ((num2 += 1f) < prv.y))
				{
					nxt.y = num2 - sy;
					if ((nxt.z = this.ChkWallGrid(new Vector3(num + num3, num2 + num4, prv.z))) > -999f)
					{
						break;
					}
				}
				break;
			}
			bool flag3 = true;
			nxt.z = prv.z;
			if (flag ? ((num -= 1f) > prv.x) : ((num += 1f) < prv.x))
			{
				flag3 = false;
				nxt.x = num - sx;
			}
			if (flag2 ? ((num2 -= 1f) > prv.y) : ((num2 += 1f) < prv.y))
			{
				flag3 = false;
				nxt.y = num2 - sy;
			}
			if (flag3)
			{
				break;
			}
		}
		return nxt;
	}

	private float ChkWallGrid(Vector3 p)
	{
		float num = -9999f;
		p /= 8f;
		Vector3 vector = new Vector3(0f, 0f, 1f);
		RaycastHit[] array = Physics.RaycastAll(new Ray(new Vector3(p.x, p.y, p.z - 1f), vector), 2f);
		int num2 = 0;
		while (num < -999f && num2 < array.Length)
		{
			if (array[num2].transform == this.wallGrid.transform && Vector3.Dot(array[num2].normal, vector) < -0.9f)
			{
				num = array[num2].point.z * 8f;
			}
			num2++;
		}
		return num;
	}

	private List<SceneTreeHouse.FurnitureCtrl> ChkPutFloor()
	{
		using (List<SceneTreeHouse.FurnitureCtrl>.Enumerator enumerator = this.furnitureList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				SceneTreeHouse.FurnitureCtrl fc = enumerator.Current;
				if (fc.data.dat.category == TreeHouseFurnitureStatic.Category.GENERAL_MERCHANDISE && fc != this.furnitureMove)
				{
					fc.movdep = null;
					if (fc.depend == null || this.furnitureList.Find((SceneTreeHouse.FurnitureCtrl itm) => itm.no >= 0 && itm.grid.ContainsKey(fc.depend)) == null)
					{
						fc.obj.transform.position = fc.pos;
						fc.obj.transform.eulerAngles = new Vector3(0f, fc.dir, 0f);
					}
					else
					{
						fc.obj.transform.position = fc.depend.transform.parent.position;
						fc.obj.transform.eulerAngles = new Vector3(0f, fc.depend.transform.parent.eulerAngles.y + fc.dir, 0f);
					}
				}
			}
		}
		bool flag = true;
		List<SceneTreeHouse.FurnitureCtrl> lst = new List<SceneTreeHouse.FurnitureCtrl>();
		List<Vector3> list = this.CalcFloorCorner(this.furnitureMove.obj.transform, this.furnitureMove.data);
		if (this.furnitureMove.movdep == null)
		{
			foreach (Vector3 vector in list)
			{
				RaycastHit[] array = Physics.RaycastAll(new Ray(new Vector3(vector.x, 1f, vector.z), new Vector3(0f, -1f, 0f)), 2f);
				bool flag2 = false;
				int num = 0;
				while (!flag2 && num < array.Length)
				{
					if (array[num].transform == this.floorGrid.transform)
					{
						flag2 = true;
					}
					num++;
				}
				flag = flag && flag2;
			}
		}
		List<List<SceneTreeHouse.FurnitureCtrl>> list2 = new List<List<SceneTreeHouse.FurnitureCtrl>>
		{
			new List<SceneTreeHouse.FurnitureCtrl>(),
			new List<SceneTreeHouse.FurnitureCtrl>(),
			new List<SceneTreeHouse.FurnitureCtrl>(),
			new List<SceneTreeHouse.FurnitureCtrl>(),
			new List<SceneTreeHouse.FurnitureCtrl>()
		};
		List<List<SceneTreeHouse.FurnitureCtrl>> list3 = new List<List<SceneTreeHouse.FurnitureCtrl>>
		{
			new List<SceneTreeHouse.FurnitureCtrl>(),
			new List<SceneTreeHouse.FurnitureCtrl>(),
			new List<SceneTreeHouse.FurnitureCtrl>(),
			new List<SceneTreeHouse.FurnitureCtrl>(),
			new List<SceneTreeHouse.FurnitureCtrl>()
		};
		foreach (SceneTreeHouse.FurnitureCtrl furnitureCtrl in this.furnitureList)
		{
			if (furnitureCtrl != this.furnitureMove && furnitureCtrl.no >= 0 && furnitureCtrl.data.dat.category != TreeHouseFurnitureStatic.Category.WALL_PAPER && furnitureCtrl.data.dat.category != TreeHouseFurnitureStatic.Category.FLOOR_PAPER && furnitureCtrl.data.dat.category != TreeHouseFurnitureStatic.Category.CEIL_DECO && furnitureCtrl.data.dat.category != TreeHouseFurnitureStatic.Category.CEIL_LIGHT && (furnitureCtrl.data.dat.category != TreeHouseFurnitureStatic.Category.GENERAL_MERCHANDISE || !(furnitureCtrl.depend != null)) && furnitureCtrl.data.dat.category != TreeHouseFurnitureStatic.Category.CURTAIN)
			{
				List<Vector3> list4 = new List<Vector3>(furnitureCtrl.corner);
				if (furnitureCtrl.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_HANGINGS)
				{
					if (furnitureCtrl.data.siz.z <= 0)
					{
						continue;
					}
					list4 = new List<Vector3>
					{
						furnitureCtrl.corner[3],
						furnitureCtrl.corner[2],
						furnitureCtrl.corner[2] + furnitureCtrl.walloff,
						furnitureCtrl.corner[3] + furnitureCtrl.walloff
					};
				}
				if (this.ChkFloor(this.furnitureMove.obj.transform.position, list4))
				{
					list2[0].Add(furnitureCtrl);
				}
				if (this.ChkFloor(list[0], list4))
				{
					list2[1].Add(furnitureCtrl);
				}
				if (this.ChkFloor(list[1], list4))
				{
					list2[2].Add(furnitureCtrl);
				}
				if (this.ChkFloor(list[2], list4))
				{
					list2[3].Add(furnitureCtrl);
				}
				if (this.ChkFloor(list[3], list4))
				{
					list2[4].Add(furnitureCtrl);
				}
				if (this.ChkFloor(furnitureCtrl.obj.transform.position, list))
				{
					list3[0].Add(furnitureCtrl);
				}
				if (this.ChkFloor(list4[0], list))
				{
					list3[1].Add(furnitureCtrl);
				}
				if (this.ChkFloor(list4[1], list))
				{
					list3[2].Add(furnitureCtrl);
				}
				if (this.ChkFloor(list4[2], list))
				{
					list3[3].Add(furnitureCtrl);
				}
				if (this.ChkFloor(list4[3], list))
				{
					list3[4].Add(furnitureCtrl);
				}
			}
		}
		if (this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.STAND)
		{
			int num2 = 0;
			while (flag && num2 < 5)
			{
				if (list2[num2].Find((SceneTreeHouse.FurnitureCtrl itm) => itm.data.dat.category == TreeHouseFurnitureStatic.Category.STAND) != null)
				{
					flag = false;
				}
				else if (list2[num2].Find((SceneTreeHouse.FurnitureCtrl itm) => itm.data.dat.category == TreeHouseFurnitureStatic.Category.LARGE_FURNITURE) != null)
				{
					flag = false;
				}
				else if (list3[num2].Find((SceneTreeHouse.FurnitureCtrl itm) => itm.data.dat.category == TreeHouseFurnitureStatic.Category.STAND) != null)
				{
					flag = false;
				}
				else if (list3[num2].Find((SceneTreeHouse.FurnitureCtrl itm) => itm.data.dat.category == TreeHouseFurnitureStatic.Category.LARGE_FURNITURE) != null)
				{
					flag = false;
				}
				num2++;
			}
			lst = new List<SceneTreeHouse.FurnitureCtrl>(list3[0].FindAll((SceneTreeHouse.FurnitureCtrl itm) => itm.data.dat.category == TreeHouseFurnitureStatic.Category.GENERAL_MERCHANDISE));
			int num3 = 1;
			Predicate<SceneTreeHouse.FurnitureCtrl> <>9__6;
			Predicate<SceneTreeHouse.FurnitureCtrl> <>9__7;
			while (flag && num3 < 5)
			{
				List<SceneTreeHouse.FurnitureCtrl> list5 = list2[num3];
				Predicate<SceneTreeHouse.FurnitureCtrl> predicate;
				if ((predicate = <>9__6) == null)
				{
					predicate = (<>9__6 = (SceneTreeHouse.FurnitureCtrl itm) => itm.data.dat.category == TreeHouseFurnitureStatic.Category.GENERAL_MERCHANDISE && !lst.Contains(itm));
				}
				if (list5.Find(predicate) != null)
				{
					flag = false;
				}
				else
				{
					List<SceneTreeHouse.FurnitureCtrl> list6 = list3[num3];
					Predicate<SceneTreeHouse.FurnitureCtrl> predicate2;
					if ((predicate2 = <>9__7) == null)
					{
						predicate2 = (<>9__7 = (SceneTreeHouse.FurnitureCtrl itm) => itm.data.dat.category == TreeHouseFurnitureStatic.Category.GENERAL_MERCHANDISE && !lst.Contains(itm));
					}
					if (list6.Find(predicate2) != null)
					{
						flag = false;
					}
				}
				num3++;
			}
			using (List<SceneTreeHouse.FurnitureCtrl>.Enumerator enumerator = lst.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SceneTreeHouse.FurnitureCtrl fc1 = enumerator.Current;
					float num4 = 99999f;
					GameObject o2 = null;
					foreach (GameObject gameObject in this.furnitureMove.grid.Keys)
					{
						Vector3 vector2 = this.furnitureMove.grid[gameObject];
						if (fc1.data.objSiz.x <= vector2.x && fc1.data.objSiz.y <= vector2.y && fc1.data.objSiz.z <= vector2.z)
						{
							float magnitude = (fc1.obj.transform.position - gameObject.transform.parent.position).magnitude;
							if (num4 > magnitude)
							{
								num4 = magnitude;
								o2 = gameObject;
							}
						}
					}
					if (o2 != null && this.furnitureList.Find((SceneTreeHouse.FurnitureCtrl itm) => itm != fc1 && itm.no >= 0 && (itm.depend == o2 || itm.movdep == o2)) == null)
					{
						fc1.movdep = o2;
					}
					else
					{
						flag = false;
						fc1.movdep = this.furnitureMove.obj;
					}
				}
			}
			List<SceneTreeHouse.FurnitureCtrl> list7 = new List<SceneTreeHouse.FurnitureCtrl>();
			float y2 = this.furnitureMove.obj.transform.position.y + this.furnitureMove.data.objSiz.y;
			int num5 = 0;
			Predicate<SceneTreeHouse.FurnitureCtrl> <>9__11;
			Predicate<SceneTreeHouse.FurnitureCtrl> <>9__12;
			while (flag)
			{
				if (num5 >= 5)
				{
					break;
				}
				list2[num5].RemoveAll((SceneTreeHouse.FurnitureCtrl itm) => itm.data.dat.category != TreeHouseFurnitureStatic.Category.WALL_HANGINGS);
				list3[num5].RemoveAll((SceneTreeHouse.FurnitureCtrl itm) => itm.data.dat.category != TreeHouseFurnitureStatic.Category.WALL_HANGINGS);
				List<SceneTreeHouse.FurnitureCtrl> list8 = list2[num5];
				Predicate<SceneTreeHouse.FurnitureCtrl> predicate3;
				if ((predicate3 = <>9__11) == null)
				{
					predicate3 = (<>9__11 = (SceneTreeHouse.FurnitureCtrl itm) => y2 > itm.corner[2].y + 0.005f);
				}
				if (list8.Find(predicate3) != null)
				{
					flag = false;
				}
				else
				{
					List<SceneTreeHouse.FurnitureCtrl> list9 = list3[num5];
					Predicate<SceneTreeHouse.FurnitureCtrl> predicate4;
					if ((predicate4 = <>9__12) == null)
					{
						predicate4 = (<>9__12 = (SceneTreeHouse.FurnitureCtrl itm) => y2 > itm.corner[2].y + 0.005f);
					}
					if (list9.Find(predicate4) != null)
					{
						flag = false;
					}
					else
					{
						foreach (SceneTreeHouse.FurnitureCtrl furnitureCtrl2 in list2[num5])
						{
							if (!list7.Contains(furnitureCtrl2))
							{
								list7.Add(furnitureCtrl2);
							}
						}
						foreach (SceneTreeHouse.FurnitureCtrl furnitureCtrl3 in list3[num5])
						{
							if (!list7.Contains(furnitureCtrl3))
							{
								list7.Add(furnitureCtrl3);
							}
						}
					}
				}
				num5++;
			}
			while (flag && list7.Count > 0)
			{
				Vector3 position = list7[0].obj.transform.position;
				List<Vector3> list10 = new List<Vector3>
				{
					list7[0].corner[3],
					list7[0].corner[2],
					list7[0].corner[2] + list7[0].walloff,
					list7[0].corner[3] + list7[0].walloff
				};
				y2 = list10[0].y + 0.005f;
				list7.RemoveAt(0);
				using (Dictionary<GameObject, Vector3>.KeyCollection.Enumerator enumerator3 = this.furnitureMove.grid.Keys.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						GameObject o3 = enumerator3.Current;
						SceneTreeHouse.FurnitureCtrl furnitureCtrl4 = this.furnitureList.Find((SceneTreeHouse.FurnitureCtrl itm) => itm.no >= 0 && itm.depend == o3);
						if (furnitureCtrl4 != null && furnitureCtrl4.obj.transform.position.y + furnitureCtrl4.data.objSiz.y - 0.005f >= y2)
						{
							list = this.CalcFloorCorner(furnitureCtrl4.obj.transform, furnitureCtrl4.data);
							if (this.ChkFloor(furnitureCtrl4.obj.transform.position, list10) || this.ChkFloor(list[0], list10) || this.ChkFloor(list[1], list10) || this.ChkFloor(list[2], list10) || this.ChkFloor(list[3], list10))
							{
								flag = false;
								break;
							}
							if (this.ChkFloor(position, list) || this.ChkFloor(list10[0], list) || this.ChkFloor(list10[1], list) || this.ChkFloor(list10[2], list) || this.ChkFloor(list10[3], list))
							{
								flag = false;
								break;
							}
						}
					}
				}
			}
			if (flag)
			{
				lst.Add(this.furnitureMove);
			}
		}
		else if (this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.RUG)
		{
			int num6 = -1;
			int num7 = 0;
			while (flag && num7 < 5)
			{
				foreach (SceneTreeHouse.FurnitureCtrl furnitureCtrl5 in list2[num7])
				{
					if (furnitureCtrl5.data.dat.category == TreeHouseFurnitureStatic.Category.RUG && furnitureCtrl5.rug > num6)
					{
						num6 = furnitureCtrl5.rug;
					}
				}
				foreach (SceneTreeHouse.FurnitureCtrl furnitureCtrl6 in list3[num7])
				{
					if (furnitureCtrl6.data.dat.category == TreeHouseFurnitureStatic.Category.RUG && furnitureCtrl6.rug > num6)
					{
						num6 = furnitureCtrl6.rug;
					}
				}
				num7++;
			}
			num6 += 2;
			string text = num6.ToString();
			if (num6 > SceneTreeHouse.rugStack)
			{
				flag = false;
				text = "<color=red>" + text + "</color>";
			}
			this.guiData.carpetInfo.transform.Find("Num").GetComponent<PguiTextCtrl>().text = text + "/" + SceneTreeHouse.rugStack.ToString();
			if (flag)
			{
				lst.Add(this.furnitureMove);
			}
		}
		else if (this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.GENERAL_MERCHANDISE)
		{
			lst = new List<SceneTreeHouse.FurnitureCtrl>(list2[0].FindAll((SceneTreeHouse.FurnitureCtrl itm) => itm.data.dat.category == TreeHouseFurnitureStatic.Category.STAND));
			lst.AddRange(list2[0].FindAll((SceneTreeHouse.FurnitureCtrl itm) => itm.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_HANGINGS && this.furnitureMove.obj.transform.position.y > itm.corner[2].y));
			float num8 = 99999f;
			GameObject gameObject2 = null;
			float num9 = 99999f;
			GameObject o = null;
			float yy = -1f;
			float y3 = this.furnitureMove.obj.transform.position.y;
			Vector3 vector3 = Vector3.zero;
			foreach (SceneTreeHouse.FurnitureCtrl furnitureCtrl7 in lst)
			{
				if ((furnitureCtrl7.data.dat.category != TreeHouseFurnitureStatic.Category.WALL_HANGINGS || ((!(this.furnitureMove.movdep != furnitureCtrl7.obj) || (!(this.furnitureMove.movdep == null) && furnitureCtrl7.grid.ContainsKey(this.furnitureMove.movdep))) && this.furnitureMove.obj.transform.position.y >= furnitureCtrl7.corner[2].y + 0.005f)) && yy <= furnitureCtrl7.obj.transform.position.y)
				{
					Vector3 vector4 = this.furnitureMove.obj.transform.position - furnitureCtrl7.obj.transform.position;
					vector4.y = 0f;
					float num10 = vector4.magnitude;
					if (num8 > num10)
					{
						num8 = num10;
						gameObject2 = furnitureCtrl7.obj;
						yy = furnitureCtrl7.obj.transform.position.y;
					}
					foreach (GameObject gameObject3 in furnitureCtrl7.grid.Keys)
					{
						num10 = (this.furnitureMove.obj.transform.position - gameObject3.transform.parent.position).magnitude;
						if (num9 > num10)
						{
							num9 = num10;
							o = gameObject3;
							y3 = o.transform.parent.position.y;
							yy = furnitureCtrl7.obj.transform.position.y;
							vector3 = furnitureCtrl7.grid[gameObject3];
						}
					}
				}
			}
			if (gameObject2 == null)
			{
				this.furnitureMove.movdep = null;
			}
			else if (o != null && (this.furnitureMove.depend == o || this.furnitureList.Find((SceneTreeHouse.FurnitureCtrl itm) => itm.no >= 0 && itm.depend == o) == null))
			{
				this.furnitureMove.movdep = o;
				if (this.furnitureMove.data.objSiz.x > vector3.x || this.furnitureMove.data.objSiz.y > vector3.y || this.furnitureMove.data.objSiz.z > vector3.z)
				{
					flag = false;
				}
			}
			else
			{
				flag = false;
				this.furnitureMove.movdep = gameObject2;
			}
			if (this.furnitureMove.movdep == null)
			{
				int num11 = 0;
				while (flag && num11 < 5)
				{
					if (list2[num11].Find((SceneTreeHouse.FurnitureCtrl itm) => itm.data.dat.category == TreeHouseFurnitureStatic.Category.GENERAL_MERCHANDISE) != null)
					{
						flag = false;
					}
					else if (list2[num11].Find((SceneTreeHouse.FurnitureCtrl itm) => itm.data.dat.category == TreeHouseFurnitureStatic.Category.STAND) != null)
					{
						flag = false;
					}
					else if (list2[num11].Find((SceneTreeHouse.FurnitureCtrl itm) => itm.data.dat.category == TreeHouseFurnitureStatic.Category.LARGE_FURNITURE) != null)
					{
						flag = false;
					}
					else if (list3[num11].Find((SceneTreeHouse.FurnitureCtrl itm) => itm.data.dat.category == TreeHouseFurnitureStatic.Category.GENERAL_MERCHANDISE) != null)
					{
						flag = false;
					}
					else if (list3[num11].Find((SceneTreeHouse.FurnitureCtrl itm) => itm.data.dat.category == TreeHouseFurnitureStatic.Category.STAND) != null)
					{
						flag = false;
					}
					else if (list3[num11].Find((SceneTreeHouse.FurnitureCtrl itm) => itm.data.dat.category == TreeHouseFurnitureStatic.Category.LARGE_FURNITURE) != null)
					{
						flag = false;
					}
					num11++;
				}
			}
			yy = y3 + this.furnitureMove.data.objSiz.y;
			int num12 = 0;
			Predicate<SceneTreeHouse.FurnitureCtrl> <>9__24;
			Predicate<SceneTreeHouse.FurnitureCtrl> <>9__27;
			Predicate<SceneTreeHouse.FurnitureCtrl> <>9__25;
			Predicate<SceneTreeHouse.FurnitureCtrl> <>9__28;
			while (flag && num12 < 5)
			{
				list2[num12].RemoveAll((SceneTreeHouse.FurnitureCtrl itm) => itm.data.dat.category != TreeHouseFurnitureStatic.Category.WALL_HANGINGS);
				List<SceneTreeHouse.FurnitureCtrl> list11 = list2[num12];
				Predicate<SceneTreeHouse.FurnitureCtrl> predicate5;
				if ((predicate5 = <>9__24) == null)
				{
					predicate5 = (<>9__24 = (SceneTreeHouse.FurnitureCtrl itm) => this.furnitureMove.movdep == itm.obj || (this.furnitureMove.movdep != null && itm.grid.ContainsKey(this.furnitureMove.movdep)));
				}
				list11.RemoveAll(predicate5);
				List<SceneTreeHouse.FurnitureCtrl> list12 = list2[num12];
				Predicate<SceneTreeHouse.FurnitureCtrl> predicate6;
				if ((predicate6 = <>9__25) == null)
				{
					predicate6 = (<>9__25 = (SceneTreeHouse.FurnitureCtrl itm) => y3 > itm.corner[0].y - 0.005f || yy < itm.corner[2].y + 0.005f);
				}
				list12.RemoveAll(predicate6);
				list3[num12].RemoveAll((SceneTreeHouse.FurnitureCtrl itm) => itm.data.dat.category != TreeHouseFurnitureStatic.Category.WALL_HANGINGS);
				List<SceneTreeHouse.FurnitureCtrl> list13 = list3[num12];
				Predicate<SceneTreeHouse.FurnitureCtrl> predicate7;
				if ((predicate7 = <>9__27) == null)
				{
					predicate7 = (<>9__27 = (SceneTreeHouse.FurnitureCtrl itm) => this.furnitureMove.movdep == itm.obj || (this.furnitureMove.movdep != null && itm.grid.ContainsKey(this.furnitureMove.movdep)));
				}
				list13.RemoveAll(predicate7);
				List<SceneTreeHouse.FurnitureCtrl> list14 = list3[num12];
				Predicate<SceneTreeHouse.FurnitureCtrl> predicate8;
				if ((predicate8 = <>9__28) == null)
				{
					predicate8 = (<>9__28 = (SceneTreeHouse.FurnitureCtrl itm) => y3 > itm.corner[0].y - 0.005f || yy < itm.corner[2].y + 0.005f);
				}
				list14.RemoveAll(predicate8);
				if (list2[num12].Count > 0 || list3[num12].Count > 0)
				{
					flag = false;
				}
				num12++;
			}
			if (flag)
			{
				lst.Add(this.furnitureMove);
			}
		}
		else
		{
			float y = this.furnitureMove.obj.transform.position.y + this.furnitureMove.data.objSiz.y;
			int num13 = 0;
			Predicate<SceneTreeHouse.FurnitureCtrl> <>9__30;
			Predicate<SceneTreeHouse.FurnitureCtrl> <>9__32;
			while (flag && num13 < 5)
			{
				list2[num13].RemoveAll((SceneTreeHouse.FurnitureCtrl itm) => itm.data.dat.category == TreeHouseFurnitureStatic.Category.RUG);
				List<SceneTreeHouse.FurnitureCtrl> list15 = list2[num13];
				Predicate<SceneTreeHouse.FurnitureCtrl> predicate9;
				if ((predicate9 = <>9__30) == null)
				{
					predicate9 = (<>9__30 = (SceneTreeHouse.FurnitureCtrl itm) => itm.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_HANGINGS && y < itm.corner[2].y + 0.005f);
				}
				list15.RemoveAll(predicate9);
				list3[num13].RemoveAll((SceneTreeHouse.FurnitureCtrl itm) => itm.data.dat.category == TreeHouseFurnitureStatic.Category.RUG);
				List<SceneTreeHouse.FurnitureCtrl> list16 = list3[num13];
				Predicate<SceneTreeHouse.FurnitureCtrl> predicate10;
				if ((predicate10 = <>9__32) == null)
				{
					predicate10 = (<>9__32 = (SceneTreeHouse.FurnitureCtrl itm) => itm.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_HANGINGS && y < itm.corner[2].y + 0.005f);
				}
				list16.RemoveAll(predicate10);
				if (list2[num13].Count > 0 || list3[num13].Count > 0)
				{
					flag = false;
				}
				num13++;
			}
			if (flag)
			{
				lst.Add(this.furnitureMove);
			}
		}
		return lst;
	}

	private bool ChkFloor(Vector3 p, List<Vector3> b)
	{
		for (int i = 0; i < b.Count; i++)
		{
			Vector3 vector = b[i];
			Vector3 vector2 = b[(i + 1) % b.Count];
			if (!this.ChkLine(p.x, p.z, vector.x, vector.z, vector2.x, vector2.z))
			{
				return false;
			}
		}
		return true;
	}

	private List<SceneTreeHouse.FurnitureCtrl> ChkPutWall()
	{
		List<SceneTreeHouse.FurnitureCtrl> lst = new List<SceneTreeHouse.FurnitureCtrl>();
		if (this.furnitureMove.data.dat.category != TreeHouseFurnitureStatic.Category.WALL_HANGINGS)
		{
			return lst;
		}
		float num = (float)this.furnitureMove.data.siz.x * 0.125f - 0.005f;
		float num2 = (float)this.furnitureMove.data.siz.y * 0.125f - 0.005f;
		List<Vector3> list = new List<Vector3>
		{
			this.furnitureMove.obj.transform.TransformPoint(new Vector3(num, num2, 0f)),
			this.furnitureMove.obj.transform.TransformPoint(new Vector3(-num, num2, 0f)),
			this.furnitureMove.obj.transform.TransformPoint(new Vector3(-num, -num2, 0f)),
			this.furnitureMove.obj.transform.TransformPoint(new Vector3(num, -num2, 0f))
		};
		using (List<SceneTreeHouse.FurnitureCtrl>.Enumerator enumerator = this.furnitureList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				SceneTreeHouse.FurnitureCtrl fc = enumerator.Current;
				if (fc.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_HANGINGS && fc != this.furnitureMove)
				{
					fc.movdep = null;
					if (fc.depend == null || (this.furnitureList.Find((SceneTreeHouse.FurnitureCtrl itm) => itm.no >= 0 && itm.pstr.ContainsKey(fc.depend)) == null && !this.doorGridMap.ContainsKey(fc.depend)))
					{
						fc.obj.transform.position = fc.pos;
						fc.obj.transform.eulerAngles = new Vector3(0f, fc.dir, 0f);
					}
					else
					{
						fc.obj.transform.position = fc.depend.transform.parent.position;
						fc.obj.transform.rotation = fc.depend.transform.parent.rotation;
					}
				}
			}
		}
		if (this.furnitureMove.data.siz.z < 0)
		{
			GameObject md2 = null;
			Vector3 vector = Vector3.zero;
			float num3 = 0.1f;
			foreach (GameObject gameObject in this.doorGridMap.Keys)
			{
				float num4 = Vector3.Distance(this.furnitureMove.obj.transform.position, gameObject.transform.parent.position);
				if (num4 < num3)
				{
					num3 = num4;
					vector = this.doorGridMap[md2 = gameObject];
				}
			}
			if (md2 != null)
			{
				if (this.furnitureMove.depend == md2 || this.furnitureList.Find((SceneTreeHouse.FurnitureCtrl itm) => itm != this.furnitureMove && itm.no >= 0 && (itm.depend == md2 || itm.movdep == md2)) == null)
				{
					this.furnitureMove.movdep = md2;
					if (this.furnitureMove.data.objSiz.x <= vector.x && this.furnitureMove.data.objSiz.y <= vector.y)
					{
						bool flag = true;
						using (Dictionary<GameObject, Vector3>.KeyCollection.Enumerator enumerator2 = this.doorGridMap.Keys.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								GameObject o2 = enumerator2.Current;
								if (!(o2 == md2))
								{
									SceneTreeHouse.FurnitureCtrl furnitureCtrl = this.furnitureList.Find((SceneTreeHouse.FurnitureCtrl itm) => itm != this.furnitureMove && itm.no >= 0 && (itm.depend == o2 || itm.movdep == o2));
									if (furnitureCtrl != null)
									{
										float num5 = (float)furnitureCtrl.data.siz.x * 0.125f - 0.05f;
										float num6 = (float)furnitureCtrl.data.siz.y * 0.125f - 0.05f;
										List<Vector3> list2 = new List<Vector3>
										{
											furnitureCtrl.obj.transform.TransformPoint(new Vector3(num5, num6, 0f)),
											furnitureCtrl.obj.transform.TransformPoint(new Vector3(-num5, num6, 0f)),
											furnitureCtrl.obj.transform.TransformPoint(new Vector3(-num5, -num6, 0f)),
											furnitureCtrl.obj.transform.TransformPoint(new Vector3(num5, -num6, 0f))
										};
										if (this.ChkWall(this.furnitureMove.obj.transform.position, list2))
										{
											flag = false;
											break;
										}
										if (this.ChkWall(list[0], list2) || this.ChkWall(list[1], list2) || this.ChkWall(list[2], list2) || this.ChkWall(list[3], list2))
										{
											flag = false;
											break;
										}
										if (this.ChkWall(furnitureCtrl.obj.transform.position, list))
										{
											flag = false;
											break;
										}
										if (this.ChkWall(list2[0], list) || this.ChkWall(list2[1], list) || this.ChkWall(list2[2], list) || this.ChkWall(list2[3], list))
										{
											flag = false;
											break;
										}
									}
								}
							}
						}
						if (flag)
						{
							lst.Add(this.furnitureMove);
						}
					}
				}
				return lst;
			}
			foreach (SceneTreeHouse.FurnitureCtrl furnitureCtrl2 in this.furnitureList)
			{
				foreach (GameObject gameObject2 in furnitureCtrl2.pstr.Keys)
				{
					float num7 = Vector3.Distance(this.furnitureMove.obj.transform.position, gameObject2.transform.parent.position);
					if (num7 < num3)
					{
						num3 = num7;
						vector = furnitureCtrl2.pstr[md2 = gameObject2];
					}
				}
			}
			if (md2 != null)
			{
				if (this.furnitureMove.depend == md2 || this.furnitureList.Find((SceneTreeHouse.FurnitureCtrl itm) => itm != this.furnitureMove && itm.no >= 0 && (itm.depend == md2 || itm.movdep == md2)) == null)
				{
					this.furnitureMove.movdep = md2;
					if (this.furnitureMove.data.objSiz.x <= vector.x && this.furnitureMove.data.objSiz.y <= vector.y)
					{
						lst.Add(this.furnitureMove);
					}
				}
				return lst;
			}
		}
		for (float num8 = -num + 1.25f; num8 < num; num8 += 1.25f)
		{
			list.Add(this.furnitureMove.obj.transform.TransformPoint(new Vector3(num8, -num2, 0f)));
		}
		Vector3 vector2 = this.furnitureMove.obj.transform.TransformVector(new Vector3(0f, 0f, 1f));
		foreach (Vector3 vector3 in list)
		{
			RaycastHit[] array = Physics.RaycastAll(new Ray(vector3 - vector2, vector2), 2f);
			bool flag2 = false;
			int num9 = 0;
			while (!flag2 && num9 < array.Length)
			{
				if (array[num9].transform == this.wallGrid.transform && Vector3.Dot(array[num9].normal, vector2) < -0.9f)
				{
					flag2 = true;
				}
				num9++;
			}
			if (!flag2)
			{
				return lst;
			}
		}
		List<List<SceneTreeHouse.FurnitureCtrl>> list3 = new List<List<SceneTreeHouse.FurnitureCtrl>>
		{
			new List<SceneTreeHouse.FurnitureCtrl>(),
			new List<SceneTreeHouse.FurnitureCtrl>(),
			new List<SceneTreeHouse.FurnitureCtrl>(),
			new List<SceneTreeHouse.FurnitureCtrl>(),
			new List<SceneTreeHouse.FurnitureCtrl>()
		};
		List<List<SceneTreeHouse.FurnitureCtrl>> list4 = new List<List<SceneTreeHouse.FurnitureCtrl>>
		{
			new List<SceneTreeHouse.FurnitureCtrl>(),
			new List<SceneTreeHouse.FurnitureCtrl>(),
			new List<SceneTreeHouse.FurnitureCtrl>(),
			new List<SceneTreeHouse.FurnitureCtrl>(),
			new List<SceneTreeHouse.FurnitureCtrl>()
		};
		foreach (SceneTreeHouse.FurnitureCtrl furnitureCtrl3 in this.furnitureList)
		{
			if (furnitureCtrl3 != this.furnitureMove && furnitureCtrl3.no >= 0 && furnitureCtrl3.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_HANGINGS && !(furnitureCtrl3.depend != null) && Mathf.Abs(Mathf.DeltaAngle(this.furnitureMove.obj.transform.eulerAngles.y, furnitureCtrl3.obj.transform.eulerAngles.y)) <= 10f)
			{
				if (this.ChkWall(this.furnitureMove.obj.transform.position, furnitureCtrl3.corner))
				{
					list3[0].Add(furnitureCtrl3);
				}
				if (this.ChkWall(list[0], furnitureCtrl3.corner))
				{
					list3[1].Add(furnitureCtrl3);
				}
				if (this.ChkWall(list[1], furnitureCtrl3.corner))
				{
					list3[2].Add(furnitureCtrl3);
				}
				if (this.ChkWall(list[2], furnitureCtrl3.corner))
				{
					list3[3].Add(furnitureCtrl3);
				}
				if (this.ChkWall(list[3], furnitureCtrl3.corner))
				{
					list3[4].Add(furnitureCtrl3);
				}
				if (this.ChkWall(furnitureCtrl3.obj.transform.position, list))
				{
					list4[0].Add(furnitureCtrl3);
				}
				if (this.ChkWall(furnitureCtrl3.corner[0], list))
				{
					list4[1].Add(furnitureCtrl3);
				}
				if (this.ChkWall(furnitureCtrl3.corner[1], list))
				{
					list4[2].Add(furnitureCtrl3);
				}
				if (this.ChkWall(furnitureCtrl3.corner[2], list))
				{
					list4[3].Add(furnitureCtrl3);
				}
				if (this.ChkWall(furnitureCtrl3.corner[3], list))
				{
					list4[4].Add(furnitureCtrl3);
				}
			}
		}
		if (this.furnitureMove.data.siz.z > 0)
		{
			if (list3.FindAll((List<SceneTreeHouse.FurnitureCtrl> itm) => itm.Count > 0).Count <= 0)
			{
				if (list4.FindAll((List<SceneTreeHouse.FurnitureCtrl> itm) => itm.Count > 0).Count <= 0)
				{
					float num10 = list[0].y;
					vector2 = this.furnitureMove.obj.transform.TransformVector(new Vector3(0f, 0f, -((float)this.furnitureMove.data.siz.z * 0.25f - 0.05f)));
					list = new List<Vector3>
					{
						list[3],
						list[2],
						list[2] + vector2,
						list[3] + vector2
					};
					List<SceneTreeHouse.FurnitureCtrl> list5 = new List<SceneTreeHouse.FurnitureCtrl>();
					using (List<SceneTreeHouse.FurnitureCtrl>.Enumerator enumerator = this.furnitureList.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							SceneTreeHouse.FurnitureCtrl furnitureCtrl4 = enumerator.Current;
							if (furnitureCtrl4 != this.furnitureMove && furnitureCtrl4.no >= 0 && furnitureCtrl4.data.dat.category != TreeHouseFurnitureStatic.Category.WALL_PAPER && furnitureCtrl4.data.dat.category != TreeHouseFurnitureStatic.Category.FLOOR_PAPER && furnitureCtrl4.data.dat.category != TreeHouseFurnitureStatic.Category.CEIL_DECO && furnitureCtrl4.data.dat.category != TreeHouseFurnitureStatic.Category.CEIL_LIGHT && furnitureCtrl4.data.dat.category != TreeHouseFurnitureStatic.Category.CURTAIN)
							{
								if (furnitureCtrl4.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_HANGINGS)
								{
									if (furnitureCtrl4.depend == null && furnitureCtrl4.data.siz.z > 0 && num10 < furnitureCtrl4.corner[2].y + 0.005f)
									{
										list5.Add(furnitureCtrl4);
									}
								}
								else if ((!(furnitureCtrl4.depend != null) || !this.furnitureMove.grid.ContainsKey(furnitureCtrl4.depend)) && furnitureCtrl4.obj.transform.position.y + furnitureCtrl4.data.objSiz.y >= list[2].y && furnitureCtrl4.obj.transform.position.y <= num10)
								{
									if (this.ChkFloor(this.furnitureMove.obj.transform.position, furnitureCtrl4.corner) || this.ChkFloor(list[0], furnitureCtrl4.corner) || this.ChkFloor(list[1], furnitureCtrl4.corner) || this.ChkFloor(list[2], furnitureCtrl4.corner) || this.ChkFloor(list[3], furnitureCtrl4.corner))
									{
										return lst;
									}
									if (this.ChkFloor(furnitureCtrl4.obj.transform.position, list) || this.ChkFloor(furnitureCtrl4.corner[0], list) || this.ChkFloor(furnitureCtrl4.corner[1], list) || this.ChkFloor(furnitureCtrl4.corner[2], list) || this.ChkFloor(furnitureCtrl4.corner[3], list))
									{
										return lst;
									}
								}
							}
						}
						goto IL_11F8;
					}
					IL_0FB4:
					Vector3 position = list5[0].obj.transform.position;
					List<Vector3> list6 = new List<Vector3>
					{
						list5[0].corner[3],
						list5[0].corner[2],
						list5[0].corner[2] + list5[0].walloff,
						list5[0].corner[3] + list5[0].walloff
					};
					num10 = list6[0].y + 0.005f;
					list5.RemoveAt(0);
					using (Dictionary<GameObject, Vector3>.KeyCollection.Enumerator enumerator2 = this.furnitureMove.grid.Keys.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							GameObject o3 = enumerator2.Current;
							SceneTreeHouse.FurnitureCtrl furnitureCtrl5 = this.furnitureList.Find((SceneTreeHouse.FurnitureCtrl itm) => itm.no >= 0 && itm.depend == o3);
							if (furnitureCtrl5 != null && furnitureCtrl5.obj.transform.position.y + furnitureCtrl5.data.objSiz.y >= num10)
							{
								list = this.CalcFloorCorner(furnitureCtrl5.obj.transform, furnitureCtrl5.data);
								if (this.ChkFloor(furnitureCtrl5.obj.transform.position, list6) || this.ChkFloor(list[0], list6) || this.ChkFloor(list[1], list6) || this.ChkFloor(list[2], list6) || this.ChkFloor(list[3], list6))
								{
									return lst;
								}
								if (this.ChkFloor(position, list) || this.ChkFloor(list6[0], list) || this.ChkFloor(list6[1], list) || this.ChkFloor(list6[2], list) || this.ChkFloor(list6[3], list))
								{
									return lst;
								}
							}
						}
					}
					IL_11F8:
					if (list5.Count <= 0)
					{
						goto IL_177A;
					}
					goto IL_0FB4;
				}
			}
			return lst;
		}
		if (this.furnitureMove.data.siz.z < 0)
		{
			GameObject md = null;
			Vector3 vector4 = Vector3.zero;
			float num11 = 99999f;
			foreach (SceneTreeHouse.FurnitureCtrl furnitureCtrl6 in list3[0])
			{
				if (furnitureCtrl6.data.siz.z == 0)
				{
					foreach (GameObject gameObject3 in furnitureCtrl6.pstr.Keys)
					{
						float num12 = Vector3.Distance(this.furnitureMove.obj.transform.position, gameObject3.transform.parent.position);
						if (num12 < num11)
						{
							num11 = num12;
							vector4 = furnitureCtrl6.pstr[md = gameObject3];
						}
					}
				}
			}
			if (md != null && this.furnitureMove.depend != md && this.furnitureList.Find((SceneTreeHouse.FurnitureCtrl itm) => itm != this.furnitureMove && itm.no >= 0 && (itm.depend == md || itm.movdep == md)) != null)
			{
				return lst;
			}
			if ((this.furnitureMove.movdep = md) == null)
			{
				if (list3.FindAll((List<SceneTreeHouse.FurnitureCtrl> itm) => itm.Count > 0).Count <= 0)
				{
					if (list4.FindAll((List<SceneTreeHouse.FurnitureCtrl> itm) => itm.Count > 0).Count <= 0)
					{
						goto IL_177A;
					}
				}
				return lst;
			}
			if (this.furnitureMove.data.objSiz.x > vector4.x || this.furnitureMove.data.objSiz.y > vector4.y)
			{
				return lst;
			}
		}
		else
		{
			if (this.furnitureMove.pstr.Count <= 0)
			{
				if (list3.FindAll((List<SceneTreeHouse.FurnitureCtrl> itm) => itm.Count > 0).Count <= 0)
				{
					if (list4.FindAll((List<SceneTreeHouse.FurnitureCtrl> itm) => itm.Count > 0).Count <= 0)
					{
						goto IL_177A;
					}
				}
				return lst;
			}
			for (int i = 0; i < 5; i++)
			{
				if (list3[i].Find((SceneTreeHouse.FurnitureCtrl itm) => itm.data.siz.z >= 0) != null)
				{
					return lst;
				}
				if (list4[i].Find((SceneTreeHouse.FurnitureCtrl itm) => itm.data.siz.z >= 0) != null)
				{
					return lst;
				}
			}
			lst = new List<SceneTreeHouse.FurnitureCtrl>(list4[0].FindAll((SceneTreeHouse.FurnitureCtrl itm) => itm.data.siz.z < 0));
			Predicate<SceneTreeHouse.FurnitureCtrl> <>9__15;
			Predicate<SceneTreeHouse.FurnitureCtrl> <>9__16;
			for (int j = 1; j < 5; j++)
			{
				List<SceneTreeHouse.FurnitureCtrl> list7 = list3[j];
				Predicate<SceneTreeHouse.FurnitureCtrl> predicate;
				if ((predicate = <>9__15) == null)
				{
					predicate = (<>9__15 = (SceneTreeHouse.FurnitureCtrl itm) => !lst.Contains(itm));
				}
				if (list7.Find(predicate) != null)
				{
					return lst;
				}
				List<SceneTreeHouse.FurnitureCtrl> list8 = list4[j];
				Predicate<SceneTreeHouse.FurnitureCtrl> predicate2;
				if ((predicate2 = <>9__16) == null)
				{
					predicate2 = (<>9__16 = (SceneTreeHouse.FurnitureCtrl itm) => !lst.Contains(itm));
				}
				if (list8.Find(predicate2) != null)
				{
					return lst;
				}
			}
			bool flag3 = true;
			using (List<SceneTreeHouse.FurnitureCtrl>.Enumerator enumerator = lst.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SceneTreeHouse.FurnitureCtrl fc1 = enumerator.Current;
					float num13 = 99999f;
					GameObject o = null;
					foreach (GameObject gameObject4 in this.furnitureMove.pstr.Keys)
					{
						Vector3 vector5 = this.furnitureMove.pstr[gameObject4];
						if (fc1.data.objSiz.x <= vector5.x && fc1.data.objSiz.y <= vector5.y)
						{
							float num14 = Vector3.Distance(fc1.obj.transform.position, gameObject4.transform.parent.position);
							if (num13 > num14)
							{
								num13 = num14;
								o = gameObject4;
							}
						}
					}
					if (o != null && this.furnitureList.Find((SceneTreeHouse.FurnitureCtrl itm) => itm != fc1 && itm.no >= 0 && (itm.depend == o || itm.movdep == o)) != null)
					{
						flag3 = false;
					}
					else if ((fc1.movdep = o) == null)
					{
						flag3 = false;
					}
				}
			}
			if (!flag3)
			{
				return lst;
			}
		}
		IL_177A:
		lst.Add(this.furnitureMove);
		return lst;
	}

	private bool ChkWall(Vector3 p, List<Vector3> b)
	{
		float num = b[0].x;
		float num2 = b[0].x;
		float y = b[2].y;
		float y2 = b[0].y;
		if (b[2].x < num)
		{
			num = b[2].x;
		}
		else
		{
			num2 = b[2].x;
		}
		return p.x >= num && p.x <= num2 && p.y >= y && p.y <= y2;
	}

	private bool ChkLine(float p1, float p2, float b11, float b12, float b21, float b22)
	{
		return (b21 - b11) * (p2 - b12) - (p1 - b11) * (b22 - b12) >= 0f;
	}

	private float ChkDistance(Vector3 p, List<Vector3> b)
	{
		float num = 999999f;
		bool flag = true;
		for (int i = 0; i < b.Count; i++)
		{
			Vector3 vector = b[i];
			Vector3 vector2 = b[(i + 1) % b.Count];
			flag &= this.ChkLine(p.x, p.z, vector.x, vector.z, vector2.x, vector2.z);
			float num2 = this.ChkDistance(p.x, p.z, vector.x, vector.z, vector2.x, vector2.z);
			if (num > num2)
			{
				num = num2;
			}
		}
		if (flag)
		{
			num = -num;
		}
		return num;
	}

	private float ChkDistance(float p1, float p2, float b11, float b12, float b21, float b22)
	{
		float num = b21 - b11;
		float num2 = b22 - b12;
		float num3 = Mathf.Clamp01(((p1 - b11) * num + (p2 - b12) * num2) / (num * num + num2 * num2));
		float num4 = num * num3 + b11 - p1;
		float num5 = num2 * num3 + b12 - p2;
		return num4 * num4 + num5 * num5;
	}

	private bool IsTop()
	{
		return !this.guiOther.baseObj.activeSelf && (this.guiData.anime.ExIsCurrent("START") || this.guiData.anime.ExIsCurrent("END_SUB") || this.guiData.anime.ExIsCurrent("END03") || this.guiData.anime.ExIsCurrent("END04") || this.guiData.anime.ExIsCurrent("CAMERA_END"));
	}

	private bool IsHide()
	{
		return !this.guiOther.baseObj.activeSelf && this.guiData.anime.ExIsCurrent("END02");
	}

	private bool IsEdit()
	{
		return !this.guiOther.baseObj.activeSelf && (this.guiData.anime.ExIsCurrent("LOOP") || this.guiData.anime.ExIsCurrent("LOOP_END") || this.guiData.anime.ExIsCurrent("END"));
	}

	private bool IsChara()
	{
		return !this.guiOther.baseObj.activeSelf && this.guiData.anime.ExIsCurrent("START_SUB");
	}

	private bool IsFurniture()
	{
		return !this.guiOther.baseObj.activeSelf && (this.guiData.anime.ExIsCurrent("LOOP_SUB") || this.guiData.anime.ExIsCurrent("LOOP_END02"));
	}

	private bool IsPlacement()
	{
		return !this.guiOther.baseObj.activeSelf && this.guiData.anime.ExIsCurrent("LOOP_SUB02");
	}

	private bool IsCamera()
	{
		return !this.guiOther.baseObj.activeSelf && (this.guiData.anime.ExIsCurrent("CAMERA_START") || this.guiData.anime.ExIsCurrent("CAMERA_HIDE_END"));
	}

	private bool IsCameraHide()
	{
		return !this.guiOther.baseObj.activeSelf && (this.guiData.anime.ExIsCurrent("CAMERA_HIDE") || this.guiData.anime.ExIsCurrent("VR_POPUP"));
	}

	private bool IsOther()
	{
		return this.guiOther.baseObj.activeSelf && (this.guiOther.anime.ExIsCurrent("START") || this.guiOther.anime.ExIsCurrent("END02") || this.guiOther.anime.ExIsCurrent("END_PROF") || this.guiOther.anime.ExIsCurrent("CAMERA_END"));
	}

	private bool IsOtherHide()
	{
		return this.guiOther.baseObj.activeSelf && (this.guiOther.anime.ExIsCurrent("START02") || this.guiOther.anime.ExIsCurrent("ESC_PROF"));
	}

	private bool IsOtherInfo()
	{
		return this.guiOther.baseObj.activeSelf && this.guiOther.anime.ExIsCurrent("START_PROF");
	}

	private bool IsOtherCamera()
	{
		return this.guiOther.baseObj.activeSelf && (this.guiOther.anime.ExIsCurrent("CAMERA_START") || this.guiOther.anime.ExIsCurrent("CAMERA_HIDE_END"));
	}

	private bool IsOtherCameraHide()
	{
		return this.guiOther.baseObj.activeSelf && this.guiOther.anime.ExIsCurrent("CAMERA_HIDE");
	}

	private void Top2Chara()
	{
		this.guiData.anime.ExPlayAnimation("START_SUB", null);
	}

	private void Chara2Top()
	{
		this.guiData.anime.ExPlayAnimation("END_SUB", null);
	}

	private void Top2Edit()
	{
		this.guiData.popupMachineAll.SetActive(false);
		this.furnitureMapEdit = this.MakeFurnitureMap();
		bool flag = this.furnitureList.Find((SceneTreeHouse.FurnitureCtrl itm) => itm.no > 0) != null;
		this.guiData.anime.ExPlayAnimation("LOOP", null);
		CanvasManager.HdlCmnMenu.SetActiveMenu(false);
		this.guiData.btnCancelEdit.SetActEnable(false, false, false);
		this.guiData.btnShopEdit.SetActEnable(true, false, false);
		this.guiData.btnGachaEdit.SetActEnable(true, false, false);
		this.guiData.btnReset.SetActEnable(flag, false, false);
		this.guiData.btnMove.SetActEnable(flag, false, false);
		this.guiData.btnPlacemennt.SetActEnable(true, false, false);
		this.isMove = 0;
		if (this.camEditNo > 4)
		{
			this.camEditNo = 0;
		}
	}

	private void Edit2Top()
	{
		this.guiData.anime.ExPlayAnimation("END03", null);
		CanvasManager.HdlCmnMenu.SetActiveMenu(true);
		this.isMove = 0;
		this.RemoveMachineUITemporarily();
	}

	private void Edit2Furniture()
	{
		this.guiData.anime.ExPlayAnimation("LOOP_SUB", null);
		this.guiData.rightPlaceBtn.SetActive(false);
		this.guiData.btnTid.gameObject.SetActive(false);
		this.guiData.furnitureList.SetActive(true);
		this.guiData.categoryScroll.Refresh();
		this.guiData.furnitureScroll.Refresh();
		this.guiData.btnShopEdit.SetActEnable(false, false, false);
		this.guiData.btnGachaEdit.SetActEnable(false, false, false);
		this.guiData.btnReset.SetActEnable(false, false, false);
		this.guiData.btnMove.SetActEnable(true, false, false);
		this.guiData.btnPlacemennt.SetActEnable(true, false, false);
		this.isMove = 0;
	}

	private void Edit2Placement()
	{
		this.guiData.anime.ExPlayAnimation("LOOP_SUB02", null);
		this.guiData.rightPlaceBtn.SetActive(true);
		this.guiData.btnTid.gameObject.SetActive(true);
		this.guiData.furnitureList.SetActive(false);
	}

	private void Furniture2Edit()
	{
		bool flag = this.furnitureList.Find((SceneTreeHouse.FurnitureCtrl itm) => itm.no > 0) != null;
		if (!flag)
		{
			this.isMove = 0;
		}
		this.guiData.anime.ExPlayAnimation("END", null);
		this.guiData.btnCancelEdit.SetActEnable(this.CheckFurnitureMap(this.furnitureMapEdit) != null, false, false);
		this.guiData.btnShopEdit.SetActEnable(this.isMove <= 0, false, false);
		this.guiData.btnGachaEdit.SetActEnable(this.isMove <= 0, false, false);
		this.guiData.btnReset.SetActEnable(this.isMove <= 0 && flag, false, false);
		this.guiData.btnMove.SetActEnable(this.isMove > 0 || flag, false, false);
		this.guiData.btnPlacemennt.SetActEnable(true, false, false);
		if (this.camEditNo > 4)
		{
			this.camEditNo = 0;
		}
	}

	private void Furniture2Placement()
	{
		this.guiData.anime.ExPlayAnimation("LOOP_SUB02", null);
		this.guiData.rightPlaceBtn.SetActive(true);
		this.guiData.btnTid.gameObject.SetActive(false);
	}

	private void Placement2Furniture()
	{
		this.guiData.anime.ExPlayAnimation("LOOP_END02", null);
		this.guiData.rightPlaceBtn.SetActive(false);
		this.guiData.btnTid.gameObject.SetActive(false);
		this.guiData.furnitureList.SetActive(true);
		this.guiData.categoryScroll.Refresh();
		this.guiData.furnitureScroll.Refresh();
		this.guiData.btnShopEdit.SetActEnable(false, false, false);
		this.guiData.btnGachaEdit.SetActEnable(false, false, false);
		this.guiData.btnReset.SetActEnable(false, false, false);
		this.guiData.btnMove.SetActEnable(true, false, false);
		this.guiData.btnPlacemennt.SetActEnable(true, false, false);
	}

	private void Placement2Edit()
	{
		bool flag = this.furnitureList.Find((SceneTreeHouse.FurnitureCtrl itm) => itm.no > 0) != null;
		if (!flag)
		{
			this.isMove = 0;
		}
		this.guiData.anime.ExPlayAnimation("LOOP_END", null);
		this.guiData.btnCancelEdit.SetActEnable(this.CheckFurnitureMap(this.furnitureMapEdit) != null, false, false);
		this.guiData.btnShopEdit.SetActEnable(this.isMove <= 0, false, false);
		this.guiData.btnGachaEdit.SetActEnable(this.isMove <= 0, false, false);
		this.guiData.btnReset.SetActEnable(this.isMove <= 0 && flag, false, false);
		this.guiData.btnMove.SetActEnable(this.isMove > 0 || flag, false, false);
		this.guiData.btnPlacemennt.SetActEnable(true, false, false);
		if (this.camEditNo > 4)
		{
			this.camEditNo = 0;
		}
	}

	private void Top2Hide()
	{
		this.guiData.popupMachineAll.SetActive(false);
		this.guiData.anime.ExPlayAnimation("END02", null);
		CanvasManager.HdlCmnMenu.SetActiveMenu(false);
	}

	private void Hide2Top()
	{
		this.guiData.popupMachineAll.SetActive(true);
		this.guiData.anime.ExPlayAnimation("END04", null);
		CanvasManager.HdlCmnMenu.SetActiveMenu(true);
	}

	private void Top2Camera()
	{
		this.guiData.popupMachineAll.SetActive(false);
		this.guiData.anime.ExPlayAnimation("CAMERA_START", null);
		CanvasManager.HdlCmnMenu.SetActiveMenu(false);
	}

	private void Camera2Top()
	{
		this.guiData.anime.ExPlayAnimation("CAMERA_END", null);
		CanvasManager.HdlCmnMenu.SetActiveMenu(true);
		this.gyroCam = null;
		this.RemoveMachineUITemporarily();
	}

	private void Camera2Hide()
	{
		this.guiData.anime.ExPlayAnimation("CAMERA_HIDE", null);
	}

	private void Hide2Camera()
	{
		this.guiData.anime.ExPlayAnimation("CAMERA_HIDE_END", null);
	}

	private void InfoVR()
	{
		this.guiData.anime.ExPlayAnimation("VR_POPUP", null);
	}

	private void Other2Hide()
	{
		this.guiOther.anime.ExPlayAnimation("START02", null);
	}

	private void Hide2Other()
	{
		this.guiOther.anime.ExPlayAnimation("END02", null);
		this.guiOther.btnInfo.transform.Find("BaseImage/Mark_PullDown").GetComponent<PguiReplaceSpriteCtrl>().Replace(1);
	}

	private void Other2Info()
	{
		this.guiOther.anime.ExPlayAnimation("START_PROF", null);
		this.guiOther.btnInfo.transform.Find("BaseImage/Mark_PullDown").GetComponent<PguiReplaceSpriteCtrl>().Replace(2);
	}

	private void Info2Other()
	{
		this.guiOther.anime.ExPlayAnimation("END_PROF", null);
		this.guiOther.btnInfo.transform.Find("BaseImage/Mark_PullDown").GetComponent<PguiReplaceSpriteCtrl>().Replace(1);
	}

	private void Info2Hide()
	{
		this.guiOther.anime.ExPlayAnimation("ESC_PROF", null);
	}

	private void Other2Camera()
	{
		this.guiOther.anime.ExPlayAnimation("CAMERA_START", null);
	}

	private void Camera2Other()
	{
		this.guiOther.anime.ExPlayAnimation("CAMERA_END", null);
		this.gyroCam = null;
	}

	private void OtherCamera2Hide()
	{
		this.guiOther.anime.ExPlayAnimation("CAMERA_HIDE", null);
	}

	private void Hide2OtherCamera()
	{
		this.guiOther.anime.ExPlayAnimation("CAMERA_HIDE_END", null);
	}

	private void OnClickButton(PguiButtonCtrl button)
	{
		if (this.winPanel.activeSelf)
		{
			return;
		}
		if (this.ienum.Count > 0)
		{
			return;
		}
		if (!CanvasManager.HdlOpenWindowBasic.FinishedClose())
		{
			return;
		}
		if (button == this.guiData.btnEdit)
		{
			if (this.IsTop())
			{
				this.Top2Edit();
				return;
			}
		}
		else if (button == this.guiData.btnMyset)
		{
			if (this.IsTop())
			{
				this.ienum.Add(this.MysetOpen());
				return;
			}
		}
		else if (button == this.guiData.btnShop || button == this.guiData.btnShopEdit)
		{
			bool flag = false;
			if (button == this.guiData.btnShop)
			{
				if (this.IsTop())
				{
					flag = true;
				}
			}
			else if (button == this.guiData.btnShopEdit && this.IsEdit() && this.isMove <= 0)
			{
				flag = true;
			}
			if (flag)
			{
				SceneShopArgs sceneShopArgs = new SceneShopArgs();
				sceneShopArgs.resultNextSceneName = SceneManager.SceneName.SceneTreeHouse;
				sceneShopArgs.resultNextSceneArgs = null;
				sceneShopArgs.shopId = 90;
				Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneShop, sceneShopArgs);
				return;
			}
		}
		else if (button == this.guiData.btnGacha || button == this.guiData.btnGachaEdit)
		{
			bool flag2 = false;
			if (button == this.guiData.btnGacha)
			{
				if (this.IsTop())
				{
					flag2 = true;
				}
			}
			else if (button == this.guiData.btnGachaEdit && this.IsEdit() && this.isMove <= 0)
			{
				flag2 = true;
			}
			if (flag2)
			{
				SceneGacha.OpenParam openParam = new SceneGacha.OpenParam();
				openParam.resultNextSceneName = SceneManager.SceneName.SceneTreeHouse;
				openParam.resultNextSceneArgs = null;
				openParam.gachaId = 50012;
				Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneGacha, openParam);
				return;
			}
		}
		else if (button == this.guiData.btnSocial)
		{
			if (this.IsTop())
			{
				this.ienum.Add(this.SocialOpen());
				return;
			}
		}
		else if (button == this.guiData.btnCharge)
		{
			if (this.IsTop())
			{
				this.ienum.Add(this.ChargeOpen());
				return;
			}
		}
		else if (button == this.guiData.btnMachineAll)
		{
			if (this.IsTop())
			{
				this.ienum.Add(this.MachineAllOpen());
				return;
			}
		}
		else if (button == this.guiData.btnPlacemennt)
		{
			if (this.IsFurniture())
			{
				this.Furniture2Edit();
				return;
			}
			if (this.IsEdit())
			{
				this.Edit2Furniture();
				return;
			}
		}
		else if (button == this.guiData.btnMove)
		{
			if (this.IsEdit() || this.IsFurniture())
			{
				if (this.IsFurniture())
				{
					this.Furniture2Edit();
				}
				this.isMove = ((this.isMove > 0) ? 0 : 1);
				this.guiData.btnShopEdit.SetActEnable(this.isMove <= 0, false, false);
				this.guiData.btnGachaEdit.SetActEnable(this.isMove <= 0, false, false);
				this.guiData.btnReset.SetActEnable(this.isMove <= 0, false, false);
				this.guiData.btnMove.SetActEnable(true, false, false);
				this.guiData.btnPlacemennt.SetActEnable(true, false, false);
				return;
			}
		}
		else if (button == this.guiData.btnReset)
		{
			if (this.IsEdit() && this.isMove <= 0)
			{
				this.ienum.Add(this.ResetFurniture());
				return;
			}
		}
		else if (button == this.guiData.btnHide)
		{
			if (this.IsTop())
			{
				this.Top2Hide();
				return;
			}
		}
		else if (button == this.guiData.btnView)
		{
			if (this.IsTop())
			{
				this.RemoveMachineUITemporarily();
				int num = this.camViewNo + 1;
				this.camViewNo = num;
				if (num >= this.camView.Count)
				{
					this.camViewNo = 0;
					return;
				}
			}
		}
		else if (button == this.guiData.btnMusic)
		{
			if (this.IsTop())
			{
				this.ienum.Add(this.MusicOpen());
				return;
			}
		}
		else if (button == this.guiData.btnCamera)
		{
			if (this.IsTop() && this.gyroLst.Count > 0)
			{
				this.Top2Camera();
				this.gyroCam = this.gyroLst[0].cam[0];
				this.ResetGyro();
				return;
			}
		}
		else if (button == this.guiData.btnCancelEdit)
		{
			if (this.IsEdit())
			{
				this.ienum.Add(this.CancelFurnitureEdit());
				return;
			}
		}
		else if (button == this.guiData.btnOkEdit)
		{
			if (this.IsEdit())
			{
				this.Edit2Top();
				return;
			}
		}
		else if (button == this.guiData.btnViewEdit)
		{
			if (this.IsEdit())
			{
				int num = this.camEditNo + 1;
				this.camEditNo = num;
				if (num > 4)
				{
					this.camEditNo = 0;
					return;
				}
			}
		}
		else if (button == this.guiData.btnViewPlace)
		{
			if (this.IsPlacement() && this.furnitureMove != null)
			{
				if (this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.CURTAIN)
				{
					int num = this.camEditNo + 1;
					this.camEditNo = num;
					if (num > 4 || this.camEditNo < 2)
					{
						this.camEditNo = 2;
						return;
					}
					if (this.camEditNo == 3)
					{
						this.camEditNo = 4;
						return;
					}
				}
				else
				{
					if (this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_PAPER)
					{
						this.camEditNo = 2;
						return;
					}
					if (this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.FLOOR_PAPER)
					{
						this.camEditNo = 0;
						return;
					}
					if (this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.CEIL_DECO)
					{
						this.camEditNo = 5;
						return;
					}
					if (this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.CEIL_LIGHT)
					{
						this.camEditNo = 5;
						return;
					}
					int num = this.camEditNo + 1;
					this.camEditNo = num;
					if (num > 4)
					{
						this.camEditNo = 0;
						return;
					}
				}
			}
		}
		else if (button == this.guiData.btnRotR)
		{
			if (this.IsPlacement() && this.furnitureMove != null && this.furnitureMove.data.dat.category != TreeHouseFurnitureStatic.Category.WALL_HANGINGS && this.furnitureMove.data.dat.category != TreeHouseFurnitureStatic.Category.CURTAIN && this.furnitureMove.data.dat.category != TreeHouseFurnitureStatic.Category.WALL_PAPER && this.furnitureMove.data.dat.category != TreeHouseFurnitureStatic.Category.FLOOR_PAPER && this.furnitureMove.data.dat.category != TreeHouseFurnitureStatic.Category.CEIL_DECO && this.furnitureMove.data.dat.category != TreeHouseFurnitureStatic.Category.CEIL_LIGHT)
			{
				this.moveRot += 90f;
				if (this.movePos.y < -100f)
				{
					this.movePos = this.furnitureMove.obj.transform.position;
					this.moveNrm = new Vector3(0f, 1f, 0f);
					return;
				}
			}
		}
		else if (button == this.guiData.btnRotL)
		{
			if (this.IsPlacement() && this.furnitureMove != null && this.furnitureMove.data.dat.category != TreeHouseFurnitureStatic.Category.WALL_HANGINGS && this.furnitureMove.data.dat.category != TreeHouseFurnitureStatic.Category.CURTAIN && this.furnitureMove.data.dat.category != TreeHouseFurnitureStatic.Category.WALL_PAPER && this.furnitureMove.data.dat.category != TreeHouseFurnitureStatic.Category.FLOOR_PAPER && this.furnitureMove.data.dat.category != TreeHouseFurnitureStatic.Category.CEIL_DECO && this.furnitureMove.data.dat.category != TreeHouseFurnitureStatic.Category.CEIL_LIGHT)
			{
				this.moveRot -= 90f;
				if (this.movePos.y < -100f)
				{
					this.movePos = this.furnitureMove.obj.transform.position;
					this.moveNrm = new Vector3(0f, 1f, 0f);
					return;
				}
			}
		}
		else if (button == this.guiData.btnRotG)
		{
			if (this.IsPlacement() && this.floorGrid.activeSelf && this.furnitureMove != null && this.furnitureMove.data.dat.category != TreeHouseFurnitureStatic.Category.WALL_HANGINGS && this.furnitureMove.data.dat.category != TreeHouseFurnitureStatic.Category.CURTAIN && this.furnitureMove.data.dat.category != TreeHouseFurnitureStatic.Category.WALL_PAPER && this.furnitureMove.data.dat.category != TreeHouseFurnitureStatic.Category.FLOOR_PAPER && this.furnitureMove.data.dat.category != TreeHouseFurnitureStatic.Category.CEIL_DECO && this.furnitureMove.data.dat.category != TreeHouseFurnitureStatic.Category.CEIL_LIGHT)
			{
				int num = this.gridRot + 1;
				this.gridRot = num;
				if (num > 5)
				{
					this.gridRot = 0;
				}
				this.floorGrid.transform.eulerAngles = new Vector3(0f, 60f * (float)this.gridRot, 0f);
				if (this.movePos.y < -100f)
				{
					this.movePos = this.furnitureMove.obj.transform.position;
					this.moveNrm = new Vector3(0f, 1f, 0f);
					return;
				}
			}
		}
		else if (button == this.guiData.btnOk)
		{
			if (this.IsFurniture())
			{
				this.NewFurniture();
				return;
			}
			if (this.IsPlacement())
			{
				this.DecideFurniture();
				return;
			}
		}
		else if (button == this.guiData.btnCancel)
		{
			if (this.IsFurniture())
			{
				this.Furniture2Edit();
				return;
			}
			if (this.IsPlacement())
			{
				this.CancelFurniture();
				return;
			}
		}
		else if (button == this.guiData.btnTid)
		{
			if (this.IsPlacement())
			{
				this.TidFurniture();
				return;
			}
		}
		else if (button == this.guiData.btnClose)
		{
			if (this.IsFurniture())
			{
				this.Furniture2Edit();
				return;
			}
		}
		else if (button == this.guiData.btnInfo)
		{
			if (this.IsFurniture())
			{
				this.winPanel.SetActive(true);
				this.winFurniture.win.ForceOpen();
				this.winFurniture.ratio.text = "なかよしポイント＋" + this.kizunaRatio + "％";
				this.winFurniture.bonus.ReplaceTextByDefault("Param01", DataManager.DmTreeHouse.KizunaBonusData.totalFurniturePoint.ToString());
				this.winFurniture.next.ReplaceTextByDefault("Param01", DataManager.DmTreeHouse.KizunaBonusData.nextLevelFurniturePoint.ToString());
				TreeHouseStaminaBonusData staminaBonusData = DataManager.DmTreeHouse.StaminaBonusData;
				this.winFurniture.ratioSSR.text = "スタミナ最大値＋" + staminaBonusData.staminaBonus.ToString();
				this.winFurniture.bonusSSR.ReplaceTextByDefault("Param01", staminaBonusData.staminaFurnitureCount.ToString());
				this.winFurniture.nextSSR.ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[]
				{
					staminaBonusData.nextStaminaFurnitureCount.ToString(),
					staminaBonusData.nextUpStaminaBonus.ToString()
				});
				this.winFurniture.nextSSR.gameObject.SetActive(staminaBonusData.nextStaminaFurnitureCount > 0 && staminaBonusData.nextUpStaminaBonus > 0);
				return;
			}
		}
		else if (!(button == this.guiData.btnFilter))
		{
			if (button == this.guiData.btnSort)
			{
				if (this.IsFurniture())
				{
					this.winPanel.SetActive(true);
					this.winSort.win.ForceOpen();
					int num2 = (int)(SceneTreeHouse.furnitureSortType.ContainsKey(this.categorySel) ? SceneTreeHouse.furnitureSortType[this.categorySel] : SceneTreeHouse.FurnitureSortType.NUMBER);
					for (int i = 0; i < this.winSort.btn.Count; i++)
					{
						this.winSort.btn[i].SetToggleIndex((num2 == i) ? 1 : 0);
					}
					return;
				}
			}
			else if (button == this.guiData.btnDisp)
			{
				if (this.IsFurniture())
				{
					string text;
					if (SceneTreeHouse.furnitureDispType == SceneTreeHouse.FurnitureDispType.ALL)
					{
						SceneTreeHouse.furnitureDispType = SceneTreeHouse.FurnitureDispType.FAVORITE;
						text = "お気に入り";
					}
					else if (SceneTreeHouse.furnitureDispType == SceneTreeHouse.FurnitureDispType.FAVORITE)
					{
						if (this.categorySel == TreeHouseFurnitureStatic.Category.WALL_HANGINGS)
						{
							SceneTreeHouse.furnitureDispType = SceneTreeHouse.FurnitureDispType.BANNER;
							text = "のぼり";
						}
						else
						{
							SceneTreeHouse.furnitureDispType = SceneTreeHouse.FurnitureDispType.ALL;
							text = "一覧";
						}
					}
					else if (SceneTreeHouse.furnitureDispType == SceneTreeHouse.FurnitureDispType.BANNER)
					{
						SceneTreeHouse.furnitureDispType = SceneTreeHouse.FurnitureDispType.POSTER;
						text = "ポスター";
					}
					else
					{
						SceneTreeHouse.furnitureDispType = SceneTreeHouse.FurnitureDispType.ALL;
						text = "一覧";
					}
					this.guiData.btnDisp.transform.Find("BaseImage/On/Txt").GetComponent<PguiTextCtrl>().text = text;
					this.MakefurnitureDataDisp();
					return;
				}
			}
			else if (button == this.guiData.btnOkChara)
			{
				if (this.IsChara() || this.chgChr > 0)
				{
					this.OnClickCharaOk();
					return;
				}
			}
			else if (button == this.guiData.btnTipsHand)
			{
				if (this.guiData.tipsHand.activeSelf)
				{
					this.guiData.chkTipsHand.SetActive(!this.guiData.chkTipsHand.activeSelf);
					return;
				}
			}
			else if (button == this.guiData.btnTipsMouse)
			{
				if (this.guiData.tipsMouse.activeSelf)
				{
					this.guiData.chkTipsMouse.SetActive(!this.guiData.chkTipsMouse.activeSelf);
					return;
				}
			}
			else if (button == this.guiData.btnReturn)
			{
				if (this.IsCamera() && this.gyroLst.Count > 1)
				{
					this.NextGyrocam();
					return;
				}
			}
			else if (button == this.guiData.btnHideCam)
			{
				if (this.IsCamera())
				{
					this.Camera2Hide();
					return;
				}
			}
			else if (button == this.guiData.btnCancelCam)
			{
				if (this.IsCamera())
				{
					this.Camera2Top();
					return;
				}
			}
			else if (button == this.guiData.btnResetCam)
			{
				if (this.IsCamera())
				{
					this.ResetGyro();
					return;
				}
			}
			else if (button == this.guiData.btnGyro)
			{
				if (SceneTreeHouse.gyroMode >= 0 && this.IsCamera())
				{
					SceneTreeHouse.gyroMode = 1 - SceneTreeHouse.gyroMode;
					this.ResetGyro();
					return;
				}
			}
			else if (button == this.guiData.btnVR)
			{
				if (SceneTreeHouse.gyroMode >= 0 && this.IsCamera() && this.camera.gameObject.activeSelf)
				{
					this.ienum.Add(this.StartVR());
					return;
				}
			}
			else if (button == this.guiOther.btnBack)
			{
				if (this.IsOther() || this.IsOtherInfo())
				{
					this.OnClickEndVisit();
					return;
				}
			}
			else if (button == this.guiOther.btnHide)
			{
				if (this.IsOtherInfo())
				{
					this.Info2Hide();
					return;
				}
				if (this.IsOther())
				{
					this.Other2Hide();
					return;
				}
			}
			else if (button == this.guiOther.btnView)
			{
				if (this.IsOther() || this.IsOtherInfo())
				{
					int num = this.camOtherNo + 1;
					this.camOtherNo = num;
					if (num >= this.camOther.Count)
					{
						this.camOtherNo = 0;
						return;
					}
				}
			}
			else if (button == this.guiOther.btnFollow)
			{
				if (this.IsOther() || this.IsOtherInfo())
				{
					this.ienum.Add(this.FollowOther());
					return;
				}
			}
			else if (button == this.guiOther.btnCamera)
			{
				if ((this.IsOther() || this.IsOtherInfo()) && this.gyroLst.Count > 0)
				{
					this.Other2Camera();
					this.gyroCam = this.gyroLst[0].cam[0];
					this.ResetGyro();
					return;
				}
			}
			else if (button == this.guiOther.btnReturn)
			{
				if (this.IsOtherCamera() && this.gyroLst.Count > 1)
				{
					this.NextGyrocam();
					return;
				}
			}
			else if (button == this.guiOther.btnHideCam)
			{
				if (this.IsOtherCamera())
				{
					this.OtherCamera2Hide();
					return;
				}
			}
			else if (button == this.guiOther.btnCancelCam)
			{
				if (this.IsOtherCamera())
				{
					this.Camera2Other();
					return;
				}
			}
			else if (button == this.guiOther.btnResetCam)
			{
				if (this.IsOtherCamera())
				{
					this.ResetGyro();
					return;
				}
			}
			else if (button == this.guiOther.btnGyro)
			{
				if (SceneTreeHouse.gyroMode >= 0 && this.IsOtherCamera())
				{
					SceneTreeHouse.gyroMode = 1 - SceneTreeHouse.gyroMode;
					this.ResetGyro();
					return;
				}
			}
			else if (button == this.guiOther.btnVR)
			{
				if (SceneTreeHouse.gyroMode >= 0 && this.IsOtherCamera() && this.camera.gameObject.activeSelf)
				{
					this.ienum.Add(this.StartVR());
					return;
				}
			}
			else if (button == this.guiOther.btnInfo)
			{
				if (this.IsOtherInfo())
				{
					this.Info2Other();
					return;
				}
				if (this.IsOther())
				{
					this.Other2Info();
					return;
				}
			}
			else if (button == this.guiOther.btnSocial)
			{
				if (this.IsOther() && !this.IsOtherInfo())
				{
					this.ienum.Add(this.SocialOpen());
					return;
				}
			}
			else if (button == this.guiOther.btnNext && this.IsOther() && !this.IsOtherInfo())
			{
				this.OnClickNextVisit();
			}
		}
	}

	private void NewFurniture()
	{
		if (this.furnitureSel >= 0 && this.furnitureSel < this.allFurnitureDataList.Count)
		{
			SceneTreeHouse.FurnitureData fd = this.allFurnitureDataList[this.furnitureSel];
			if (this.furnitureList.FindAll((SceneTreeHouse.FurnitureCtrl itm) => itm.no > 0 && itm.data == fd).Count < fd.num)
			{
				this.movePos = Vector3.zero;
				this.moveRot = 0f;
				this.moveCurtain = null;
				bool flag = true;
				GameObject gameObject = this.MakeFurniture(this.furniturePlace + 1, fd.dat.GetId(), this.movePos, this.moveRot, false);
				if (fd.dat.category == TreeHouseFurnitureStatic.Category.CURTAIN)
				{
					if (this.camEditNo < 2 || this.camEditNo == 3 || this.camEditNo > 4)
					{
						this.camEditNo = 2;
					}
					float y = this.camEdit[this.camEditNo].bas.localScale.y;
					float num = 200f;
					using (List<GameObject>.Enumerator enumerator = this.curtainBoard.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							GameObject t = enumerator.Current;
							if (this.furnitureList.Find((SceneTreeHouse.FurnitureCtrl itm) => itm.no > 0 && itm.board == t) == null)
							{
								float num2 = Mathf.Abs(Mathf.DeltaAngle(t.transform.eulerAngles.y, y));
								if (num > num2)
								{
									num = num2;
									this.moveCurtain = t;
								}
							}
						}
					}
					if (this.moveCurtain == null)
					{
						Object.Destroy(gameObject);
						gameObject = null;
						PrjUtil.ReleaseMemory(PrjUtil.UnloadUnused / 50);
					}
					else
					{
						this.movePos = (gameObject.transform.position = this.moveCurtain.transform.position);
						gameObject.transform.eulerAngles = new Vector3(0f, this.moveRot = this.Round30(this.moveCurtain.transform.eulerAngles.y), 0f);
						if (num > 80f)
						{
							this.camEditNo = 6 - this.camEditNo;
						}
					}
				}
				else if (fd.dat.category == TreeHouseFurnitureStatic.Category.WALL_HANGINGS)
				{
					if (this.camEditNo < 2 || this.camEditNo > 4)
					{
						this.camEditNo = 2;
					}
					float y2 = this.camEdit[this.camEditNo].bas.localScale.y;
					float num3 = 200f;
					Transform transform = null;
					foreach (Transform transform2 in this.wallLoc)
					{
						float num4 = Mathf.Abs(Mathf.DeltaAngle(transform2.eulerAngles.y, y2));
						if (num3 > num4)
						{
							num3 = num4;
							transform = transform2;
						}
					}
					if (transform == null)
					{
						Object.Destroy(gameObject);
						gameObject = null;
						PrjUtil.ReleaseMemory(PrjUtil.UnloadUnused / 50);
					}
					else
					{
						this.movePos = (gameObject.transform.position = transform.position);
						gameObject.transform.eulerAngles = new Vector3(0f, this.moveRot = this.Round30(transform.eulerAngles.y), 0f);
						this.moveNrm = gameObject.transform.TransformVector(0f, 0f, -1f);
					}
				}
				else if (fd.dat.category == TreeHouseFurnitureStatic.Category.WALL_PAPER || fd.dat.category == TreeHouseFurnitureStatic.Category.FLOOR_PAPER || fd.dat.category == TreeHouseFurnitureStatic.Category.CEIL_DECO)
				{
					flag = false;
					this.movePos = (gameObject.transform.position = Vector3.zero);
					gameObject.transform.eulerAngles = Vector3.zero;
					this.moveNrm = gameObject.transform.TransformVector(0f, -1f, 0f);
				}
				else if (fd.dat.category == TreeHouseFurnitureStatic.Category.CEIL_LIGHT)
				{
					flag = false;
					this.movePos = (gameObject.transform.position = this.lightLoc.position);
					gameObject.transform.eulerAngles = Vector3.zero;
					this.moveNrm = gameObject.transform.TransformVector(0f, -1f, 0f);
				}
				else if (this.camEditNo > 4)
				{
					this.camEditNo = 0;
				}
				if (gameObject != null)
				{
					bool flag2 = fd.dat.machineId == 0 && (fd.dat.category != TreeHouseFurnitureStatic.Category.CEIL_LIGHT || this.IsNight);
					if (fd.dat.machineId > 0)
					{
						flag2 = this.IsNewMachineOn(fd);
					}
					this.furnitureMove = new SceneTreeHouse.FurnitureCtrl(0, fd, this.movePos, this.moveRot, gameObject, flag2);
					this.furnitureList.Add(this.furnitureMove);
					this.SetBtnOk(false, flag);
					this.Furniture2Placement();
					this.InitMoveFurniture();
				}
			}
		}
	}

	private void DecideFurniture()
	{
		if (this.furnitureMove != null && this.furnitureMove.obj != null)
		{
			List<SceneTreeHouse.FurnitureCtrl> list = new List<SceneTreeHouse.FurnitureCtrl>();
			if (this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.CEIL_LIGHT || this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_PAPER || this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.FLOOR_PAPER || this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.CEIL_DECO)
			{
				list.Add(this.furnitureMove);
			}
			else if (!(this.furnitureMove.board == null))
			{
				if (this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.CURTAIN)
				{
					list.Add(this.furnitureMove);
				}
				else if (this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_HANGINGS)
				{
					list.AddRange(this.ChkPutWall());
				}
				else
				{
					list.AddRange(this.ChkPutFloor());
				}
			}
			if (list.Contains(this.furnitureMove))
			{
				if (this.furnitureMove.no == 0)
				{
					SceneTreeHouse.FurnitureCtrl furnitureCtrl = this.furnitureMove;
					int num = this.furniturePlace + 1;
					this.furniturePlace = num;
					furnitureCtrl.no = num;
				}
				this.furnitureMove.pos = this.furnitureMove.obj.transform.position;
				this.furnitureMove.dir = this.Round30(Mathf.DeltaAngle(0f, this.furnitureMove.obj.transform.eulerAngles.y));
				this.furnitureMove.CalcCorner();
				this.furnitureMove.mdl.transform.localPosition = Vector3.zero;
				using (List<SceneTreeHouse.FurnitureCtrl>.Enumerator enumerator = this.furnitureList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						SceneTreeHouse.FurnitureCtrl fc = enumerator.Current;
						if (fc != this.furnitureMove && fc.data.dat.category == this.furnitureMove.data.dat.category)
						{
							if (fc.data.dat.category == TreeHouseFurnitureStatic.Category.CEIL_LIGHT)
							{
								fc.no = -1;
							}
							else if (fc.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_PAPER || fc.data.dat.category == TreeHouseFurnitureStatic.Category.FLOOR_PAPER || fc.data.dat.category == TreeHouseFurnitureStatic.Category.CEIL_DECO)
							{
								fc.no = -1;
							}
						}
						if (fc.no >= 0)
						{
							fc.action = null;
							if (fc.data.dat.category == TreeHouseFurnitureStatic.Category.GENERAL_MERCHANDISE)
							{
								if (fc.movdep != null && this.furnitureList.Find((SceneTreeHouse.FurnitureCtrl itm) => itm.no >= 0 && itm.grid.ContainsKey(fc.movdep)) != null)
								{
									fc.depend = fc.movdep;
									fc.pos = fc.depend.transform.parent.position;
									fc.dir = this.Round90(Mathf.DeltaAngle(fc.depend.transform.parent.eulerAngles.y, fc.obj.transform.eulerAngles.y));
									fc.obj.transform.eulerAngles = new Vector3(0f, fc.depend.transform.parent.eulerAngles.y + fc.dir, 0f);
								}
								else if (fc == this.furnitureMove && fc.depend != fc.movdep)
								{
									fc.depend = null;
									fc.pos.y = 0f;
									int num2 = Mathf.RoundToInt(this.ClampAngle(Mathf.DeltaAngle(0f, fc.obj.transform.eulerAngles.y)) / 30f);
									int num3;
									if (num2 % 3 == 1)
									{
										num2 = (num2 - (num3 = 4)) / 3;
									}
									else
									{
										num3 = num2 % 3;
										num2 /= 3;
									}
									num3 /= 2;
									fc.obj.transform.eulerAngles = new Vector3(0f, fc.dir = (float)(60 * num3) + (float)(num2 * 90), 0f);
								}
								else if (fc.depend != null)
								{
									fc.pos = fc.depend.transform.parent.position;
									fc.obj.transform.eulerAngles = new Vector3(0f, fc.depend.transform.parent.eulerAngles.y + fc.dir, 0f);
								}
								fc.obj.transform.position = fc.pos;
								fc.CalcCorner();
							}
							else if (fc.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_HANGINGS && fc.data.siz.z < 0)
							{
								if (fc.movdep != null && (this.furnitureList.Find((SceneTreeHouse.FurnitureCtrl itm) => itm.no >= 0 && itm.pstr.ContainsKey(fc.movdep)) != null || this.doorGridMap.ContainsKey(fc.movdep)))
								{
									fc.depend = fc.movdep;
									fc.pos = fc.depend.transform.parent.position;
									fc.obj.transform.rotation = fc.depend.transform.parent.rotation;
								}
								else if (fc == this.furnitureMove && fc.depend != fc.movdep)
								{
									fc.depend = null;
									fc.obj.transform.eulerAngles = new Vector3(0f, fc.dir, 0f);
								}
								else if (fc.depend != null)
								{
									fc.pos = fc.depend.transform.parent.position;
									fc.obj.transform.rotation = fc.depend.transform.parent.rotation;
								}
								fc.obj.transform.position = fc.pos;
								fc.CalcCorner();
							}
							if (fc.data.dat.machineId > 0)
							{
								this.AddMachineUI(fc);
							}
						}
					}
				}
				this.furnitureList.Remove(this.furnitureMove);
				this.furnitureList.Insert(0, this.furnitureMove);
				this.furnitureMove = null;
				this.ChkRug(false);
				this.SetBtnOk(true, true);
			}
		}
		if (this.furnitureMove == null)
		{
			if (this.isMove == 0)
			{
				this.Placement2Furniture();
			}
			else
			{
				this.Placement2Edit();
			}
			this.ienum.Add(this.SaveFurniture(null));
		}
		this.CalcMachineUI();
	}

	private void CancelFurniture()
	{
		if (this.furnitureMove != null)
		{
			if (this.furnitureMove.no == 0)
			{
				this.furnitureMove.no = -1;
			}
			else if (this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.CURTAIN)
			{
				float num = 9999f;
				foreach (GameObject gameObject in this.curtainBoard)
				{
					float num2 = Vector3.Distance(this.furnitureMove.pos, gameObject.transform.position);
					if (num2 < num)
					{
						num = num2;
						this.furnitureMove.board = gameObject;
					}
				}
			}
			this.furnitureMove.obj.transform.position = this.furnitureMove.pos;
			if (this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_HANGINGS && this.furnitureMove.depend != null)
			{
				this.furnitureMove.obj.transform.rotation = this.furnitureMove.depend.transform.parent.rotation;
			}
			else
			{
				this.furnitureMove.obj.transform.eulerAngles = new Vector3(0f, ((this.furnitureMove.depend == null) ? 0f : this.furnitureMove.depend.transform.parent.eulerAngles.y) + this.furnitureMove.dir, 0f);
			}
			this.furnitureMove.mdl.transform.localPosition = Vector3.zero;
			this.furnitureMove.CalcCorner();
			foreach (SceneTreeHouse.FurnitureCtrl furnitureCtrl in this.furnitureList)
			{
				if (furnitureCtrl.no >= 0)
				{
					if (furnitureCtrl != this.furnitureMove && furnitureCtrl.movdep != null)
					{
						furnitureCtrl.obj.transform.position = furnitureCtrl.pos;
						if (furnitureCtrl.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_HANGINGS && furnitureCtrl.depend != null)
						{
							furnitureCtrl.obj.transform.rotation = furnitureCtrl.depend.transform.parent.rotation;
						}
						else
						{
							furnitureCtrl.obj.transform.eulerAngles = new Vector3(0f, ((furnitureCtrl.depend == null) ? 0f : furnitureCtrl.depend.transform.parent.eulerAngles.y) + furnitureCtrl.dir, 0f);
						}
						furnitureCtrl.CalcCorner();
					}
					else if (furnitureCtrl.depend != null && this.furnitureMove.grid.ContainsKey(furnitureCtrl.depend))
					{
						furnitureCtrl.obj.transform.position = (furnitureCtrl.pos = furnitureCtrl.depend.transform.parent.position);
						furnitureCtrl.obj.transform.eulerAngles = new Vector3(0f, furnitureCtrl.depend.transform.parent.eulerAngles.y + furnitureCtrl.dir, 0f);
						furnitureCtrl.CalcCorner();
					}
					else if (furnitureCtrl.depend != null && this.furnitureMove.pstr.ContainsKey(furnitureCtrl.depend))
					{
						furnitureCtrl.obj.transform.position = (furnitureCtrl.pos = furnitureCtrl.depend.transform.parent.position);
						furnitureCtrl.obj.transform.rotation = furnitureCtrl.depend.transform.parent.rotation;
						furnitureCtrl.CalcCorner();
					}
					furnitureCtrl.movdep = null;
				}
			}
			this.furnitureMove = null;
			this.ChkRug(false);
			this.SetBtnOk(true, true);
		}
		if (this.isMove == 0)
		{
			this.Placement2Furniture();
			return;
		}
		this.Placement2Edit();
	}

	private void TidFurniture()
	{
		if (this.isMove != 0 && this.furnitureMove != null)
		{
			this.furnitureMove.no = -1;
			this.furniturePlace = 0;
			foreach (SceneTreeHouse.FurnitureCtrl furnitureCtrl in this.furnitureList)
			{
				if (furnitureCtrl.no >= 0)
				{
					furnitureCtrl.action = null;
					if (furnitureCtrl.depend != null && this.furnitureMove.grid.ContainsKey(furnitureCtrl.depend))
					{
						furnitureCtrl.no = -1;
					}
					else if (furnitureCtrl.depend != null && this.furnitureMove.pstr.ContainsKey(furnitureCtrl.depend))
					{
						furnitureCtrl.no = -1;
					}
					else
					{
						if (this.furniturePlace < furnitureCtrl.no)
						{
							this.furniturePlace = furnitureCtrl.no;
						}
						if (furnitureCtrl.movdep != null)
						{
							furnitureCtrl.obj.transform.position = furnitureCtrl.pos;
							if (furnitureCtrl.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_HANGINGS && furnitureCtrl.depend != null)
							{
								furnitureCtrl.obj.transform.rotation = furnitureCtrl.depend.transform.parent.rotation;
							}
							else
							{
								furnitureCtrl.obj.transform.eulerAngles = new Vector3(0f, ((furnitureCtrl.depend == null) ? 0f : furnitureCtrl.depend.transform.parent.eulerAngles.y) + furnitureCtrl.dir, 0f);
							}
							furnitureCtrl.CalcCorner();
						}
					}
					furnitureCtrl.movdep = null;
				}
			}
			this.furnitureMove = null;
			this.ChkRug(false);
			this.SetBtnOk(true, true);
			this.Placement2Edit();
			this.ienum.Add(this.SaveFurniture(null));
		}
	}

	private bool OnClickButtonMenu(SceneManager.SceneName sceneName, object sceneArgs)
	{
		return false;
	}

	private void OnClickButtonMenuRetrun()
	{
		if (this.winPanel.activeSelf)
		{
			return;
		}
		if (!this.camera.gameObject.activeSelf)
		{
			this.camL.gameObject.SetActive(false);
			this.camR.gameObject.SetActive(false);
			this.camera.gameObject.SetActive(true);
			if (this.guiOther.baseObj.activeSelf)
			{
				this.Hide2OtherCamera();
				return;
			}
			this.Hide2Camera();
			return;
		}
		else
		{
			if (this.IsOtherHide())
			{
				this.Hide2Other();
				return;
			}
			if (this.IsOtherCameraHide())
			{
				this.Hide2OtherCamera();
				return;
			}
			if (this.IsOtherCamera())
			{
				this.Camera2Other();
				return;
			}
			if (this.IsOther() || this.IsOtherInfo())
			{
				this.OnClickEndVisit();
				return;
			}
			if (this.IsHide())
			{
				this.Hide2Top();
				return;
			}
			if (this.IsCameraHide())
			{
				this.Hide2Camera();
				return;
			}
			if (this.IsCamera())
			{
				this.Camera2Top();
				return;
			}
			if (this.IsChara() || this.chgChr > 0)
			{
				this.OnClickCharaOk();
				return;
			}
			if (this.IsEdit())
			{
				this.Edit2Top();
				return;
			}
			if (this.IsFurniture())
			{
				this.Furniture2Edit();
				return;
			}
			if (this.IsPlacement())
			{
				this.CancelFurniture();
				return;
			}
			Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneHome, null);
			return;
		}
	}

	private void OnTouchStart(Info info)
	{
		this.singleTap = false;
		if (this.ienum.Count > 0)
		{
			return;
		}
		if (this.OnUiTap(info.CurrentPosition))
		{
			return;
		}
		this.singleTap = true;
		this.touchView = true;
	}

	private void OnTouchStart2(Info infoA, Info infoB, float distance, float rotation)
	{
		this.singleTap = false;
		if (this.ienum.Count > 0)
		{
			return;
		}
		if (this.OnUiTap(infoA.CurrentPosition))
		{
			return;
		}
		if (this.OnUiTap(infoB.CurrentPosition))
		{
			return;
		}
		this.singleTap = true;
		this.touchView = true;
	}

	private void OnTouchEnd(Info info)
	{
		this.touchView = false;
	}

	private void OnTouchMove(Info info)
	{
		if (!this.touchView)
		{
			return;
		}
		if (this.IsPlacement() && this.furnitureMove != null)
		{
			Vector2 vector = info.DeltaPosition * (this.camera.fieldOfView / 5000f);
			if (this.furnitureMove.data.dat.category != TreeHouseFurnitureStatic.Category.CURTAIN)
			{
				if (this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_HANGINGS)
				{
					bool flag = this.furnitureMove.movdep == null;
					if (!flag && !this.doorGridMap.ContainsKey(this.furnitureMove.movdep))
					{
						SceneTreeHouse.FurnitureCtrl furnitureCtrl = this.furnitureList.Find((SceneTreeHouse.FurnitureCtrl itm) => itm.pstr.ContainsKey(this.furnitureMove.movdep));
						if (furnitureCtrl == null || (furnitureCtrl.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_HANGINGS && furnitureCtrl.depend == null && furnitureCtrl.data.siz.z <= 0))
						{
							flag = true;
						}
					}
					if (flag)
					{
						this.movePos = this.furnitureMove.obj.transform.TransformPoint(new Vector3(vector.x, vector.y, 0f));
						this.moveNrm = this.furnitureMove.obj.transform.TransformVector(0f, 0f, -1f);
						return;
					}
				}
				else if (this.furnitureMove.data.dat.category != TreeHouseFurnitureStatic.Category.WALL_PAPER && this.furnitureMove.data.dat.category != TreeHouseFurnitureStatic.Category.FLOOR_PAPER && this.furnitureMove.data.dat.category != TreeHouseFurnitureStatic.Category.CEIL_DECO && this.furnitureMove.data.dat.category != TreeHouseFurnitureStatic.Category.CEIL_LIGHT)
				{
					Vector3 vector2 = this.camera.transform.TransformVector(vector.x, 0f, vector.y);
					vector2.y = 0f;
					this.movePos = this.furnitureMove.obj.transform.position + vector2;
					this.moveNrm = new Vector3(0f, 1f, 0f);
					return;
				}
			}
		}
		else if (this.IsTop() || this.IsHide() || this.IsCamera() || this.IsCameraHide() || this.IsOther() || this.IsOtherInfo() || this.IsOtherHide() || this.IsOtherCamera() || this.IsOtherCameraHide() || this.IsEdit())
		{
			this.RemoveMachineUITemporarily();
			this.moveView = info.DeltaPosition;
		}
	}

	private void OnTouchMove2(Info infoA, Info infoB, float distance, float rotation)
	{
		if (!this.touchView)
		{
			return;
		}
		if (this.OnUiTap(infoA.CurrentPosition))
		{
			return;
		}
		if (this.OnUiTap(infoB.CurrentPosition))
		{
			return;
		}
		if (!this.IsTop() && !this.IsHide() && !this.IsCamera() && !this.IsCameraHide() && !this.IsOther() && !this.IsOtherInfo() && !this.IsOtherHide() && !this.IsOtherCamera() && !this.IsOtherCameraHide() && !this.IsEdit() && !this.IsPlacement())
		{
			return;
		}
		float magnitude = infoA.DeltaPosition.magnitude;
		float magnitude2 = infoB.DeltaPosition.magnitude;
		if (magnitude < 0.1f || magnitude2 < 0.1f)
		{
			return;
		}
		if (Mathf.Abs(magnitude - magnitude2) > 20f)
		{
			return;
		}
		if (Vector2.Dot(infoA.DeltaPosition, infoB.DeltaPosition) / magnitude / magnitude2 < 0.9f)
		{
			return;
		}
		this.RemoveMachineUITemporarily();
		this.moveView = infoA.DeltaPosition;
	}

	private void OnPinch(Info fingerA, Info fingerB, float distance, float rotation)
	{
		if (!this.touchView)
		{
			return;
		}
		if (this.OnUiTap(fingerA.CurrentPosition))
		{
			return;
		}
		if (this.OnUiTap(fingerB.CurrentPosition))
		{
			return;
		}
		if (!this.IsTop() && !this.IsHide() && !this.IsCamera() && !this.IsCameraHide() && !this.IsOther() && !this.IsOtherInfo() && !this.IsOtherHide() && !this.IsOtherCamera() && !this.IsOtherCameraHide() && !this.IsEdit() && !this.IsPlacement())
		{
			return;
		}
		this.RemoveMachineUITemporarily();
		this.pinchView = distance * Mathf.Sqrt(1280f / (float)Screen.width * (720f / (float)Screen.height)) * 0.1f;
	}

	private void OnWheel(Info info, float distance)
	{
		if (this.touchView)
		{
			return;
		}
		if (this.ienum.Count > 0)
		{
			return;
		}
		if (this.OnUiTap(info.CurrentPosition))
		{
			return;
		}
		if (!this.IsTop() && !this.IsHide() && !this.IsCamera() && !this.IsCameraHide() && !this.IsOther() && !this.IsOtherInfo() && !this.IsOtherHide() && !this.IsOtherCamera() && !this.IsOtherCameraHide() && !this.IsEdit() && !this.IsPlacement())
		{
			return;
		}
		if (!CanvasManager.CheckInWindow(info.CurrentPosition))
		{
			return;
		}
		this.RemoveMachineUITemporarily();
		this.wheelView = distance * 10f;
	}

	private void OnTap(Info info)
	{
		if (!this.singleTap || this.doubleTap)
		{
			this.doubleTap = false;
			return;
		}
		this.singleTap = false;
		if (this.ienum.Count > 0)
		{
			return;
		}
		if (this.OnUiTap(info.CurrentPosition))
		{
			return;
		}
		if (!this.camera.gameObject.activeSelf)
		{
			this.InfoVR();
			return;
		}
		GameObject gameObject = null;
		float num = 999999f;
		if (this.furnitureMove != null && this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.GENERAL_MERCHANDISE)
		{
			using (List<SceneTreeHouse.FurnitureCtrl>.Enumerator enumerator = this.furnitureList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SceneTreeHouse.FurnitureCtrl furnitureCtrl = enumerator.Current;
					foreach (GameObject gameObject2 in furnitureCtrl.grid.Keys)
					{
						float num2 = this.ChkScrDst(gameObject2.transform.parent.position, info.CurrentPosition);
						if (num > num2)
						{
							num = num2;
							gameObject = gameObject2;
						}
					}
				}
				goto IL_02F6;
			}
		}
		if (this.furnitureMove != null && this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_HANGINGS && this.furnitureMove.data.siz.z < 0)
		{
			using (Dictionary<GameObject, Vector3>.KeyCollection.Enumerator enumerator2 = this.doorGridMap.Keys.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					GameObject o2 = enumerator2.Current;
					if (!(this.furnitureMove.depend != o2) || this.furnitureList.Find((SceneTreeHouse.FurnitureCtrl itm) => itm != this.furnitureMove && itm.no >= 0 && (itm.depend == o2 || itm.movdep == o2)) == null)
					{
						float num3 = this.ChkScrDst(o2.transform.parent.position, info.CurrentPosition);
						if (num > num3)
						{
							num = num3;
							gameObject = o2;
						}
					}
				}
			}
			foreach (SceneTreeHouse.FurnitureCtrl furnitureCtrl2 in this.furnitureList)
			{
				using (Dictionary<GameObject, Vector3>.KeyCollection.Enumerator enumerator2 = furnitureCtrl2.pstr.Keys.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						GameObject o = enumerator2.Current;
						if (!(this.furnitureMove.depend != o) || this.furnitureList.Find((SceneTreeHouse.FurnitureCtrl itm) => itm != this.furnitureMove && itm.no >= 0 && (itm.depend == o || itm.movdep == o)) == null)
						{
							float num4 = this.ChkScrDst(o.transform.parent.position, info.CurrentPosition);
							if (num > num4)
							{
								num = num4;
								gameObject = o;
							}
						}
					}
				}
			}
		}
		IL_02F6:
		SceneTreeHouse.FurnitureCtrl furnitureCtrl3 = null;
		if (this.isMove <= 0 && this.furnitureMove == null)
		{
			foreach (SceneTreeHouse.FurnitureCtrl furnitureCtrl4 in this.furnitureList)
			{
				if (!(furnitureCtrl4.sw == null))
				{
					float num5 = this.ChkScrDst(furnitureCtrl4.sw.position, info.CurrentPosition);
					if (num > num5)
					{
						num = num5;
						furnitureCtrl3 = furnitureCtrl4;
					}
				}
			}
			foreach (SceneTreeHouse.FurnitureCtrl furnitureCtrl5 in this.otherFurniture)
			{
				if (!(furnitureCtrl5.sw == null) && furnitureCtrl5.data.dat.specialValue == TreeHouseFurnitureStatic.SpecialValue.BOX)
				{
					float num6 = this.ChkScrDst(furnitureCtrl5.sw.position, info.CurrentPosition);
					if (num > num6)
					{
						num = num6;
						furnitureCtrl3 = furnitureCtrl5;
					}
				}
			}
		}
		if (furnitureCtrl3 != null)
		{
			SoundManager.Play("prd_se_room_effect_switch", false, false);
			furnitureCtrl3.onoff = !furnitureCtrl3.onoff;
			return;
		}
		if (this.IsOtherHide())
		{
			this.Hide2Other();
			return;
		}
		if (this.IsHide())
		{
			this.Hide2Top();
			return;
		}
		if (this.IsCameraHide())
		{
			this.Hide2Camera();
			return;
		}
		if (this.IsOtherCameraHide())
		{
			this.Hide2OtherCamera();
			return;
		}
		if (gameObject != null)
		{
			this.furnitureMove.movdep = gameObject;
			return;
		}
		if (this.furnitureMove != null && this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.CURTAIN)
		{
			Ray ray = this.camera.fieldCamera.ScreenPointToRay(info.CurrentPosition);
			RaycastHit[] h2 = Physics.RaycastAll(ray, 30f);
			int num7 = -1;
			int num8;
			int j;
			Predicate<GameObject> <>9__2;
			for (j = 0; j < h2.Length; j = num8 + 1)
			{
				SceneTreeHouse.<>c__DisplayClass366_3 CS$<>8__locals4 = new SceneTreeHouse.<>c__DisplayClass366_3();
				SceneTreeHouse.<>c__DisplayClass366_3 CS$<>8__locals5 = CS$<>8__locals4;
				List<GameObject> list = this.curtainBoard;
				Predicate<GameObject> predicate;
				if ((predicate = <>9__2) == null)
				{
					predicate = (<>9__2 = (GameObject itm) => itm.transform == h2[j].transform);
				}
				CS$<>8__locals5.obj = list.Find(predicate);
				if (CS$<>8__locals4.obj != null && this.furnitureList.Find((SceneTreeHouse.FurnitureCtrl itm) => itm.no >= 0 && itm.board == CS$<>8__locals4.obj) == null)
				{
					num7 = j;
				}
				num8 = j;
			}
			if (num7 >= 0)
			{
				this.moveCurtain = h2[num7].transform.gameObject;
			}
			SoundManager.Play("prd_se_room_furniture_set", false, false);
			return;
		}
		if ((this.floorGrid.activeSelf || this.wallGrid.activeSelf) && this.furnitureMove != null)
		{
			Ray ray2 = this.camera.fieldCamera.ScreenPointToRay(info.CurrentPosition);
			RaycastHit[] h = Physics.RaycastAll(ray2, 30f);
			int num9 = -1;
			int num10 = -1;
			bool flag = false;
			int num8;
			int i;
			Predicate<SceneTreeHouse.FurnitureCtrl> <>9__4;
			for (i = 0; i < h.Length; i = num8 + 1)
			{
				if (h[i].transform == this.floorGrid.transform)
				{
					num9 = i;
				}
				else if (h[i].transform == this.wallGrid.transform)
				{
					num10 = i;
				}
				else
				{
					List<SceneTreeHouse.FurnitureCtrl> list2 = this.furnitureList;
					Predicate<SceneTreeHouse.FurnitureCtrl> predicate2;
					if ((predicate2 = <>9__4) == null)
					{
						predicate2 = (<>9__4 = (SceneTreeHouse.FurnitureCtrl itm) => itm.no >= 0 && itm.board != null && itm.board.transform == h[i].transform && itm.data.dat.category != TreeHouseFurnitureStatic.Category.RUG);
					}
					if (list2.Find(predicate2) != null)
					{
						flag = true;
					}
				}
				num8 = i;
			}
			if (this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.GENERAL_MERCHANDISE)
			{
				flag = false;
			}
			if (num9 >= 0 && this.floorGrid.activeSelf && !flag)
			{
				this.movePos = h[num9].point;
				this.moveNrm = h[num9].normal;
			}
			else if (num10 >= 0 && this.wallGrid.activeSelf && !flag)
			{
				this.movePos = h[num10].point;
				this.moveNrm = h[num10].normal;
			}
			SoundManager.Play("prd_se_room_furniture_set", false, false);
			return;
		}
		if (this.isMove > 0 && this.furnitureMove == null && this.IsEdit())
		{
			this.ChkFurniturePos(info.CurrentPosition, true);
			if (this.furnitureMove != null)
			{
				this.SetMoveFurniture();
				return;
			}
		}
		else if (this.IsTop())
		{
			SceneTreeHouse.FurnitureCtrl furnitureCtrl6 = this.ChkFurniturePos(info.CurrentPosition, false);
			if (furnitureCtrl6 != null && furnitureCtrl6.machinePopup != null && furnitureCtrl6.machinePopup.activeSelf)
			{
				this.OnClickMachine(furnitureCtrl6.machinePopup.GetComponent<PguiButtonCtrl>());
			}
		}
	}

	private float ChkScrDst(Vector3 cp, Vector2 tp)
	{
		if (this.camera.fieldCamera.WorldToViewportPoint(cp).z > 0.3f)
		{
			cp.y -= 0.25f;
			Vector2 vector = RectTransformUtility.WorldToScreenPoint(this.camera.fieldCamera, cp);
			if (vector.y < tp.y)
			{
				cp.y += 0.5f;
				Vector2 vector2 = RectTransformUtility.WorldToScreenPoint(this.camera.fieldCamera, cp);
				if (vector2.y > tp.y)
				{
					float num = vector2.y - vector.y;
					float num2 = tp.x - vector.x;
					if ((num2 *= num2) < num * num)
					{
						num = (vector.y + vector2.y) * 0.5f - tp.y;
						return num * num + num2;
					}
				}
			}
		}
		return 999999f;
	}

	private void OnDoubleTap(Info info)
	{
		this.singleTap = false;
		this.doubleTap = true;
		if (this.ienum.Count > 0)
		{
			return;
		}
		if (this.OnUiTap(info.CurrentPosition))
		{
			return;
		}
		if (this.camera.gameObject.activeSelf)
		{
			if (this.IsPlacement() && this.furnitureMove != null && this.guiData.btnRotR.gameObject.activeSelf)
			{
				this.OnClickButton(this.guiData.btnRotR);
			}
			return;
		}
		this.camL.gameObject.SetActive(false);
		this.camR.gameObject.SetActive(false);
		this.camera.gameObject.SetActive(true);
		if (this.guiOther.baseObj.activeSelf)
		{
			this.Hide2OtherCamera();
			return;
		}
		this.Hide2Camera();
	}

	private void OnLongTap(Info info)
	{
		this.singleTap = false;
		if (this.ienum.Count > 0)
		{
			return;
		}
		if (this.OnUiTap(info.CurrentPosition))
		{
			return;
		}
		if (!this.camera.gameObject.activeSelf)
		{
			this.InfoVR();
			if (SceneTreeHouse.gyroMode > 0)
			{
				this.ResetGyro();
			}
			return;
		}
		if (this.furnitureMove == null && this.IsEdit())
		{
			this.ChkFurniturePos(info.CurrentPosition, true);
			if (this.furnitureMove != null)
			{
				this.isMove = -1;
				this.SetMoveFurniture();
			}
		}
	}

	private void RemoveMachineUITemporarily()
	{
		this.guiData.popupMachineAll.SetActive(false);
		this.removeMachineUI = SceneTreeHouse.REMOVE_MACHINE_UI_SEC;
	}

	private void CalcMachineUI()
	{
		if (!this.IsTop())
		{
			return;
		}
		using (IEnumerator enumerator = this.guiData.popupMachineAll.transform.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Transform popup = (Transform)enumerator.Current;
				SceneTreeHouse.FurnitureCtrl furnitureCtrl = this.furnitureList.Find((SceneTreeHouse.FurnitureCtrl x) => popup.gameObject.name.EndsWith(x.obj.name));
				if (!(popup.gameObject == this.guiData.popupMachineCmn))
				{
					if (furnitureCtrl == null)
					{
						Object.Destroy(popup.gameObject);
					}
					else
					{
						Vector2 vector = RectTransformUtility.WorldToScreenPoint(this.camera.fieldCamera, furnitureCtrl.obj.transform.position + new Vector3(0f, furnitureCtrl.data.objSiz.y, 0f));
						Vector2 vector2;
						RectTransformUtility.ScreenPointToLocalPointInRectangle(this.guiData.popupMachineAll.GetComponent<RectTransform>(), vector, this.guiData.popupMachineAll.GetComponentInParent<Canvas>().worldCamera, out vector2);
						popup.transform.localPosition = vector2;
					}
				}
			}
		}
		this.guiData.popupMachineAll.SetActive(true);
		this.removeMachineUI = 0f;
	}

	private void AddMachineUI(SceneTreeHouse.FurnitureCtrl fc)
	{
		if (fc.machinePopup == null)
		{
			Transform transform = this.guiData.popupMachineAll.transform.Find("Popup_Machine_" + fc.obj.name);
			GameObject gameObject;
			if (transform != null)
			{
				gameObject = transform.gameObject;
			}
			else
			{
				gameObject = Object.Instantiate<GameObject>(this.guiData.popupMachineCmn);
				gameObject.name = "Popup_Machine_" + fc.obj.name;
				gameObject.transform.SetParent(this.guiData.popupMachineAll.transform, false);
				gameObject.GetComponent<PguiButtonCtrl>().AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickMachine), PguiButtonCtrl.SoundType.DEFAULT);
			}
			fc.machinePopup = gameObject;
			if (this.IsTop())
			{
				this.UpdateMachineDisp(fc, true);
				return;
			}
		}
		else if (this.IsTop())
		{
			this.UpdateMachineDisp(fc, false);
		}
	}

	private bool IsNewMachineOn(SceneTreeHouse.FurnitureData fd)
	{
		MstMasterRoomMachineData mstMachineData = DataManager.DmTreeHouse.GetMstMasterRoomMachineList().Find((MstMasterRoomMachineData x) => x.id == fd.dat.machineId);
		MasterRoomMachineDataModel masterRoomMachineDataModel = DataManager.DmHome.GetHomeCheckResult().GetTreeHouseMachineFntrTimeList().Find((MasterRoomMachineDataModel x) => x.machineId == mstMachineData.id && x.indexId == 0 && x.furnitureId == fd.dat.GetId());
		if (masterRoomMachineDataModel == null)
		{
			DataManager.DmHome.GetHomeCheckResult().GetTreeHouseMachineFntrTimeList().Find((MasterRoomMachineDataModel x) => x.machineId == mstMachineData.id && x.indexId == 0);
		}
		return (mstMachineData.getItemTime * 60 - masterRoomMachineDataModel.nextsecond) * 100 / (mstMachineData.getItemTime * 60) >= mstMachineData.mdlOnoffPercent;
	}

	private void UpdateMachineDisp(SceneTreeHouse.FurnitureCtrl fc, bool execOnOff)
	{
		if (fc.machinePopup == null)
		{
			return;
		}
		MasterRoomMachineDataModel masterRoomMachineDataModel = DataManager.DmHome.GetHomeCheckResult().GetTreeHouseMachineFntrTimeList().Find((MasterRoomMachineDataModel x) => x.indexId == fc.no);
		if (masterRoomMachineDataModel == null)
		{
			fc.machinePopup.SetActive(false);
			fc.onoff = false;
			return;
		}
		MstMasterRoomMachineData mstMasterRoomMachineData = DataManager.DmTreeHouse.GetMstMasterRoomMachineList().Find((MstMasterRoomMachineData x) => x.id == fc.data.dat.machineId);
		if (masterRoomMachineDataModel.nextsecond == 0)
		{
			string text = ((mstMasterRoomMachineData.getItemId > 0) ? "アイテム" : "スタミナ");
			int numMachine = this.GetNumMachine(masterRoomMachineDataModel, mstMasterRoomMachineData);
			fc.machinePopup.transform.Find("BaseImage/Txt").GetComponent<Text>().text = text + "×<size=24><color=#FFEE00>" + numMachine.ToString() + "</color></size>";
			fc.machinePopup.SetActive(numMachine > 0);
		}
		else
		{
			fc.machinePopup.SetActive(false);
		}
		if (execOnOff)
		{
			fc.onoff = (mstMasterRoomMachineData.getItemTime * 60 - masterRoomMachineDataModel.nextsecond) * 100 / (mstMasterRoomMachineData.getItemTime * 60) >= mstMasterRoomMachineData.mdlOnoffPercent;
		}
	}

	private int GetNumMachine(MasterRoomMachineDataModel machineTimeData, MstMasterRoomMachineData mstMachineData)
	{
		long num = PrjUtil.ConvertTimeToTicks(machineTimeData.lastGettime);
		int num2 = (int)((TimeManager.Now.Ticks - num) / TimeManager.Second2Tick((long)(mstMachineData.getItemTime * 60)));
		num2 = ((num2 > mstMachineData.stackMax) ? mstMachineData.stackMax : num2);
		return ((mstMachineData.getItemId > 0) ? mstMachineData.getItemNum : mstMachineData.getStaminaNum) * num2;
	}

	private bool IsMachineStackMax(MasterRoomMachineDataModel machineTimeData, MstMasterRoomMachineData mstMachineData)
	{
		long num = PrjUtil.ConvertTimeToTicks(machineTimeData.lastGettime);
		return (int)((TimeManager.Now.Ticks - num) / TimeManager.Second2Tick((long)(mstMachineData.getItemTime * 60))) >= mstMachineData.stackMax;
	}

	private long GetMachinePassTime(MasterRoomMachineDataModel machineTimeData)
	{
		long num = PrjUtil.ConvertTimeToTicks(machineTimeData.lastGettime);
		return TimeManager.Now.Ticks - num;
	}

	private void OnClickMachine(PguiButtonCtrl button)
	{
		if (this.IsTop())
		{
			this.ienum.Add(this.MachineOpen(button));
		}
	}

	private IEnumerator MachineOpen(PguiButtonCtrl button)
	{
		this.winMachine.isPrepared = false;
		this.winPanel.SetActive(true);
		this.winMachine.win.SetupButtonOnly(new PguiOpenWindowCtrl.Callback(this.DecideMachine));
		GameObject popup = button.gameObject;
		SceneTreeHouse.FurnitureCtrl fc = this.furnitureList.Find((SceneTreeHouse.FurnitureCtrl x) => popup.name.EndsWith(x.obj.name));
		this.winMachine.furnitureText.text = fc.data.dat.GetName();
		this.winMachine.fc = fc;
		Transform furnitureIcon = this.winMachine.furnitureIcon;
		furnitureIcon.Find("Texture_Photo").gameObject.SetActive(false);
		PguiRawImageCtrl component = furnitureIcon.Find("Texture_Item").GetComponent<PguiRawImageCtrl>();
		component.gameObject.SetActive(true);
		component.SetRawImage(fc.data.dat.GetIconName(), true, false, null);
		furnitureIcon.Find("Badge").gameObject.SetActive(false);
		furnitureIcon.Find("Current").gameObject.SetActive(false);
		furnitureIcon.Find("Remove").gameObject.SetActive(false);
		furnitureIcon.Find("Mark_FriendsAction").gameObject.SetActive(false);
		furnitureIcon.Find("Mark_FriendsAction_2").gameObject.SetActive(false);
		furnitureIcon.Find("Icon_Chara_Interior").gameObject.SetActive(false);
		MasterRoomMachineDataModel masterRoomMachineDataModel = DataManager.DmHome.GetHomeCheckResult().GetTreeHouseMachineFntrTimeList().Find((MasterRoomMachineDataModel x) => x.indexId == fc.no);
		MstMasterRoomMachineData mstMasterRoomMachineData = DataManager.DmTreeHouse.GetMstMasterRoomMachineList().Find((MstMasterRoomMachineData x) => x.id == fc.data.dat.machineId);
		this.winMachine.itemNameText.text = ((mstMasterRoomMachineData.getItemId > 0) ? DataManager.DmItem.GetItemStaticBase(mstMasterRoomMachineData.getItemId).GetName() : "スタミナ");
		this.winMachine.itemGetTxt01.text = ((mstMasterRoomMachineData.getItemId > 0) ? "所持数" : "スタミナ回復量");
		this.winMachine.getMax.playTime = 0f;
		this.winMachine.getMax.autoPlay = true;
		this.winMachine.machineTimeData = masterRoomMachineDataModel;
		this.winMachine.mstMachineData = mstMasterRoomMachineData;
		this.winMachine.win.ForceOpen();
		do
		{
			yield return null;
		}
		while (!this.winMachine.win.FinishedOpen());
		yield break;
	}

	private bool DecideMachine(int idx)
	{
		if (idx > 0)
		{
			this.ienum.Add(this.ReceiveMcnItem());
		}
		return true;
	}

	private IEnumerator ReceiveMcnItem()
	{
		DataManager.DmTreeHouse.RequestActionReceiveMcnItem(false, this.winMachine.fc.no);
		do
		{
			yield return null;
		}
		while (DataManager.IsServerRequesting());
		this.UpdateMachineDisp(this.winMachine.fc, this.winMachine.fc.onoff);
		this.ienum.Add(this.MachineReceiveOpen(false));
		yield break;
	}

	private bool DecideMachineAll(int idx)
	{
		if (idx > 0)
		{
			this.ienum.Add(this.ReceiveMcnItemAll());
		}
		return true;
	}

	private IEnumerator ReceiveMcnItemAll()
	{
		DataManager.DmTreeHouse.RequestActionReceiveMcnItem(true, 0);
		do
		{
			yield return null;
		}
		while (DataManager.IsServerRequesting());
		this.ienum.Add(this.MachineReceiveOpen(true));
		yield break;
	}

	private IEnumerator MachineReceiveOpen(bool is_all)
	{
		this.winPanel.SetActive(true);
		this.winMachineGet.oneArea.gameObject.SetActive(!is_all);
		this.winMachineGet.allArea.gameObject.SetActive(is_all);
		if (is_all)
		{
			this.winMachineGet.scroll.Resize(DataManager.DmTreeHouse.GetMachineReceiveList().Count, 0);
			using (List<SceneTreeHouse.FurnitureCtrl>.Enumerator enumerator = this.furnitureList.FindAll((SceneTreeHouse.FurnitureCtrl x) => x.machinePopup != null).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SceneTreeHouse.FurnitureCtrl furnitureCtrl = enumerator.Current;
					this.UpdateMachineDisp(furnitureCtrl, furnitureCtrl.onoff);
				}
				goto IL_02B4;
			}
		}
		SceneTreeHouse.FurnitureCtrl fc = this.winMachine.fc;
		this.winMachineGet.oneArea.Find("Info_Furniture/Txt").gameObject.GetComponent<PguiTextCtrl>().text = fc.data.dat.GetName();
		Transform transform = this.winMachineGet.oneArea.Find("Info_Furniture/Icon_Furniture_mini");
		transform.Find("Texture_Photo").gameObject.SetActive(false);
		PguiRawImageCtrl component = transform.Find("Texture_Item").GetComponent<PguiRawImageCtrl>();
		component.gameObject.SetActive(true);
		component.SetRawImage(fc.data.dat.GetIconName(), true, false, null);
		transform.Find("Badge").gameObject.SetActive(false);
		transform.Find("Current").gameObject.SetActive(false);
		transform.Find("Remove").gameObject.SetActive(false);
		transform.Find("Mark_FriendsAction").gameObject.SetActive(false);
		transform.Find("Mark_FriendsAction_2").gameObject.SetActive(false);
		transform.Find("Icon_Chara_Interior").gameObject.SetActive(false);
		Transform transform2 = this.winMachineGet.oneArea.Find("TreeHouse_GetMachine_ListBar");
		MasterRoomMachineReceiveModel masterRoomMachineReceiveModel = DataManager.DmTreeHouse.GetMachineReceiveList()[0];
		transform2.Find("BaseImage/Txt_Item").GetComponent<PguiTextCtrl>().text = ((masterRoomMachineReceiveModel.getItemId > 0) ? DataManager.DmItem.GetItemStaticBase(masterRoomMachineReceiveModel.getItemId).GetName() : "スタミナ");
		transform2.Find("BaseImage/Txt_Num").GetComponent<PguiTextCtrl>().text = ((masterRoomMachineReceiveModel.getItemId > 0) ? masterRoomMachineReceiveModel.getItemNum.ToString() : masterRoomMachineReceiveModel.getStamina.ToString());
		IL_02B4:
		this.winMachineGet.win.ForceOpen();
		do
		{
			yield return null;
		}
		while (!this.winMachineGet.win.FinishedOpen());
		yield break;
	}

	private IEnumerator MachineAllOpen()
	{
		this.winPanel.SetActive(true);
		this.winMachineAll.win.SetupButtonOnly(new PguiOpenWindowCtrl.Callback(this.DecideMachineAll));
		this.winMachineAll.win.ForceOpen();
		List<MasterRoomMachineDataModel> mcnTimeList = DataManager.DmHome.GetHomeCheckResult().GetTreeHouseMachineFntrTimeList();
		this.winMachineAll.noMachine.gameObject.SetActive(mcnTimeList.Count == 0);
		this.winMachineAll.scroll.transform.parent.gameObject.SetActive(mcnTimeList.Count > 0);
		if (mcnTimeList.Count > 0)
		{
			this.winMachineAll.scroll.Resize(mcnTimeList.Count, 0);
			for (int i = 0; i < this.winMachineAll.scroll.ReuseItemNum; i++)
			{
				this.SetupMachineAll(i, this.winMachineAll.scroll.transform.Find("Viewport/Content/" + i.ToString()).gameObject);
			}
		}
		do
		{
			if (mcnTimeList.Count > 0)
			{
				this.winMachineAll.scroll.ForceFocus(0);
			}
			yield return null;
		}
		while (!this.winMachineAll.win.FinishedOpen());
		yield break;
	}

	private SceneTreeHouse.FurnitureCtrl ChkFurniturePos(Vector2 infoPos, bool isForMove)
	{
		int num = 970299;
		float num2 = 9.9998E+09f;
		SceneTreeHouse.FurnitureCtrl furnitureCtrl = null;
		foreach (SceneTreeHouse.FurnitureCtrl furnitureCtrl2 in this.furnitureList)
		{
			if (furnitureCtrl2.no > 0 && furnitureCtrl2.alpha >= 0.1f && furnitureCtrl2.data.dat.category != TreeHouseFurnitureStatic.Category.WALL_PAPER && furnitureCtrl2.data.dat.category != TreeHouseFurnitureStatic.Category.FLOOR_PAPER && furnitureCtrl2.data.dat.category != TreeHouseFurnitureStatic.Category.CEIL_DECO && furnitureCtrl2.data.dat.category != TreeHouseFurnitureStatic.Category.CEIL_LIGHT && this.camera.fieldCamera.WorldToViewportPoint(furnitureCtrl2.pos).z >= 0.3f)
			{
				int num3 = furnitureCtrl2.data.siz.x;
				if (furnitureCtrl2.data.siz.y > 0)
				{
					num3 *= furnitureCtrl2.data.siz.y;
				}
				if (furnitureCtrl2.data.siz.z > 0)
				{
					num3 *= furnitureCtrl2.data.siz.z;
				}
				if (num >= num3 || furnitureCtrl2.depend != null)
				{
					float num4 = 99999f;
					float num5 = -99999f;
					float num6 = 99999f;
					float num7 = -99999f;
					foreach (Vector3 vector in furnitureCtrl2.corner)
					{
						Vector2 vector2 = RectTransformUtility.WorldToScreenPoint(this.camera.fieldCamera, vector);
						if (num4 > vector2.x)
						{
							num4 = vector2.x;
						}
						if (num5 < vector2.x)
						{
							num5 = vector2.x;
						}
						if (num6 > vector2.y)
						{
							num6 = vector2.y;
						}
						if (num7 < vector2.y)
						{
							num7 = vector2.y;
						}
					}
					if (furnitureCtrl2.data.objSiz.y > 0f)
					{
						foreach (Vector3 vector3 in furnitureCtrl2.corner)
						{
							Vector2 vector4 = RectTransformUtility.WorldToScreenPoint(this.camera.fieldCamera, vector3 + new Vector3(0f, furnitureCtrl2.data.objSiz.y, 0f));
							if (num4 > vector4.x)
							{
								num4 = vector4.x;
							}
							if (num5 < vector4.x)
							{
								num5 = vector4.x;
							}
							if (num6 > vector4.y)
							{
								num6 = vector4.y;
							}
							if (num7 < vector4.y)
							{
								num7 = vector4.y;
							}
						}
					}
					if (num4 <= (float)Screen.width && num5 >= 0f && num6 <= (float)Screen.height && num7 >= 0f && (num4 >= 0f || num5 <= (float)Screen.width) && (num6 >= 0f || num7 <= (float)Screen.height))
					{
						if (furnitureCtrl2.depend != null)
						{
							float num8 = (num5 - num4) * 0.1f;
							float num9 = (num7 - num6) * 0.1f;
							num4 += num8;
							num5 -= num8;
							num6 += num9;
							num7 -= num9;
						}
						if (num4 <= infoPos.x && num5 >= infoPos.x && num6 <= infoPos.y && num7 >= infoPos.y)
						{
							float num10 = (num4 + num5) * 0.5f - infoPos.x;
							float num11 = (num6 + num7) * 0.5f - infoPos.y;
							float num12 = num10 * num10 + num11 * num11;
							if (num > num3 || num2 > num12)
							{
								num2 = num12;
								num = num3;
								if (isForMove)
								{
									this.furnitureMove = furnitureCtrl2;
								}
								furnitureCtrl = furnitureCtrl2;
							}
						}
					}
				}
			}
		}
		return furnitureCtrl;
	}

	private void SetMoveFurniture()
	{
		float num = Mathf.DeltaAngle(0f, this.furnitureMove.obj.transform.eulerAngles.y);
		if (this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_HANGINGS || this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.CURTAIN)
		{
			this.moveRot = this.Round30(num);
		}
		else if (this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_PAPER || this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.FLOOR_PAPER || this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.CEIL_DECO)
		{
			this.moveRot = 0f;
		}
		else if (this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.CEIL_LIGHT)
		{
			this.moveRot = 0f;
		}
		else
		{
			int num2 = Mathf.RoundToInt(this.ClampAngle(num) / 30f);
			if (num2 % 3 == 1)
			{
				num2 = (num2 - (this.gridRot = 4)) / 3;
			}
			else
			{
				this.gridRot = num2 % 3;
				num2 /= 3;
			}
			this.gridRot /= 2;
			this.floorGrid.transform.eulerAngles = new Vector3(0f, (float)(60 * this.gridRot), 0f);
			this.moveRot = (float)(num2 * 90);
		}
		this.InitMoveFurniture();
		this.ChkRug(false);
		this.Edit2Placement();
		SoundManager.Play("prd_se_room_furniture_set", false, false);
	}

	private void InitMoveFurniture()
	{
		if (this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.CURTAIN || this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_HANGINGS)
		{
			this.guiData.btnRotG.gameObject.SetActive(false);
			this.guiData.btnRotL.gameObject.SetActive(false);
			this.guiData.btnRotR.gameObject.SetActive(false);
		}
		else if (this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_PAPER || this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.FLOOR_PAPER)
		{
			this.camEditNo = ((this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_PAPER) ? 2 : 0);
			this.guiData.btnRotG.gameObject.SetActive(false);
			this.guiData.btnRotL.gameObject.SetActive(false);
			this.guiData.btnRotR.gameObject.SetActive(false);
		}
		else if (this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.CEIL_DECO || this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.CEIL_LIGHT)
		{
			this.camEditNo = 5;
			this.guiData.btnRotG.gameObject.SetActive(false);
			this.guiData.btnRotL.gameObject.SetActive(false);
			this.guiData.btnRotR.gameObject.SetActive(false);
		}
		else
		{
			this.guiData.btnRotG.gameObject.SetActive(true);
			this.guiData.btnRotL.gameObject.SetActive(true);
			this.guiData.btnRotR.gameObject.SetActive(true);
		}
		this.guiData.carpetInfo.SetActive(this.furnitureMove.data.dat.category == TreeHouseFurnitureStatic.Category.RUG);
		this.furnitureMove.movdep = this.furnitureMove.depend;
	}

	private bool OnUiTap(Vector2 pos)
	{
		PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
		pointerEventData.position = pos;
		List<RaycastResult> list = new List<RaycastResult>();
		EventSystem.current.RaycastAll(pointerEventData, list);
		return list.Count > 0;
	}

	public override void OnDisableScene()
	{
		if (this.furnitureMapSave != null)
		{
			List<TreeHouseFurnitureMapping> list = this.CheckFurnitureMap(this.furnitureMapSave);
			if (list != null)
			{
				DataManager.DmTreeHouse.RequestActionPutFurniture(list);
			}
			this.furnitureMapSave = null;
		}
		CanvasManager.HdlCmnMenu.SetActiveMenu(true);
		CanvasManager.HdlCmnMenu.SetupMenu(false, "", null, "", null, null);
		this.winPanel.SetActive(false);
		this.guiOther.baseObj.SetActive(false);
		this.guiData.baseObj.SetActive(false);
		foreach (Transform transform in this.guiData.charaIcon)
		{
			transform.Find("Icon_Chara_TreeHouse/Icon_Chara").GetComponent<IconCharaCtrl>().Setup(null, SortFilterDefine.SortType.LEVEL, false, null, 0, -1, 0);
		}
		this.furnitureMove = null;
		this.gyroCam = null;
		this.actionList = new List<SceneTreeHouse.ActionCtrl>();
		foreach (SceneTreeHouse.FurnitureCtrl furnitureCtrl in this.furnitureList)
		{
			furnitureCtrl.no = -1;
		}
		foreach (SceneTreeHouse.CharaCtrl charaCtrl in this.charaList)
		{
			charaCtrl.no = -1;
		}
		this.otherAction = new List<SceneTreeHouse.ActionCtrl>();
		foreach (SceneTreeHouse.FurnitureCtrl furnitureCtrl2 in this.otherFurniture)
		{
			furnitureCtrl2.no = -1;
		}
		foreach (SceneTreeHouse.CharaCtrl charaCtrl2 in this.otherChara)
		{
			charaCtrl2.no = -1;
		}
		if (this.floorGrid != null)
		{
			Object.Destroy(this.floorGrid);
		}
		this.floorGrid = null;
		if (this.wallGrid != null)
		{
			Object.Destroy(this.wallGrid);
		}
		this.wallGrid = null;
		SGNFW.Touch.Manager.UnRegisterStart(new SGNFW.Touch.Manager.SingleAction(this.OnTouchStart));
		SGNFW.Touch.Manager.UnRegisterStartTwo(new SGNFW.Touch.Manager.DoubleAction(this.OnTouchStart2));
		SGNFW.Touch.Manager.UnRegisterRelease(new SGNFW.Touch.Manager.SingleAction(this.OnTouchEnd));
		SGNFW.Touch.Manager.UnRegisterMove(new SGNFW.Touch.Manager.SingleAction(this.OnTouchMove));
		SGNFW.Touch.Manager.UnRegisterMoveTwo(new SGNFW.Touch.Manager.DoubleAction(this.OnTouchMove2));
		SGNFW.Touch.Manager.UnRegisterPinch(new SGNFW.Touch.Manager.DoubleAction(this.OnPinch));
		SGNFW.Touch.Manager.UnRegisterMouseWheel(new SGNFW.Touch.Manager.WheelAction(this.OnWheel));
		SGNFW.Touch.Manager.UnRegisterTap(new SGNFW.Touch.Manager.SingleAction(this.OnTap));
		SGNFW.Touch.Manager.UnRegisterDoubleTap(new SGNFW.Touch.Manager.SingleAction(this.OnDoubleTap));
		SGNFW.Touch.Manager.UnRegisterLongPress(new SGNFW.Touch.Manager.SingleAction(this.OnLongTap));
		this.singleTap = (this.doubleTap = false);
		if (this.furnitureNew.Count > 0)
		{
			DataManager.DmItem.RequestActionUpdateNewFlag(this.furnitureNew);
			this.furnitureNew = new List<int>();
		}
	}

	public override bool OnDisableSceneWait()
	{
		this.CtrlPaper();
		this.CtrlFurniture(false);
		this.CtrlChara(false);
		this.CtrlFurniture(true);
		this.CtrlChara(true);
		if (this.stageCtrl != null)
		{
			if (this.stageLoad == null)
			{
				this.stageLight = null;
				Object.Destroy(this.stageCtrl.gameObject);
				this.stageCtrl = null;
			}
			else
			{
				if (this.stageLoad.MoveNext())
				{
					return false;
				}
				this.stageLoad = null;
			}
		}
		if (DataManager.IsServerRequesting())
		{
			return false;
		}
		if (this.furnitureList.Count > 0 || this.charaList.Count > 0)
		{
			return false;
		}
		if (this.otherFurniture.Count > 0 || this.otherChara.Count > 0)
		{
			return false;
		}
		if (this.paperList.Count > 0)
		{
			return false;
		}
		this.gyroLst = new List<SceneTreeHouse.FurnitureCtrl>();
		this.field.SetActive(false);
		this.charaList = new List<SceneTreeHouse.CharaCtrl>();
		this.haveCharaPackList = new List<CharaPackData>();
		this.dispCharaPackList = new List<CharaPackData>();
		this.guiData.charaScroll.Resize(0, 0);
		foreach (Transform transform in this.treehouseChara)
		{
			Object.Destroy(transform.gameObject);
		}
		this.treehouseChara = new List<Transform>();
		SortWindowCtrl.RegisterData registerData = new SortWindowCtrl.RegisterData();
		registerData.register = SortFilterDefine.RegisterType.TREEHOUSE_CHANGE;
		registerData.filterButton = null;
		registerData.sortButton = null;
		registerData.sortUdButton = null;
		registerData.funcGetTargetBaseList = () => new SortWindowCtrl.SortTarget();
		registerData.funcDisideTarget = delegate(SortWindowCtrl.SortTarget item)
		{
		};
		SortWindowCtrl.RegisterData registerData2 = registerData;
		CanvasManager.HdlOpenWindowSortFilter.Register(registerData2, false, null);
		this.guiData.categoryScroll.Resize(0, 0);
		this.guiData.furnitureScroll.Resize(0, 0);
		this.winMusic.scroll.Resize(0, 0);
		this.winGetstamp.scroll.Resize(0, 0);
		this.winMachineGet.scroll.Resize(0, 0);
		this.winMachineAll.scroll.Resize(0, 0);
		foreach (Transform transform2 in this.bgmLineup)
		{
			Object.Destroy(transform2.gameObject);
		}
		this.bgmLineup = new List<Transform>();
		this.furnitureList = new List<SceneTreeHouse.FurnitureCtrl>();
		if (this.curtainBoard != null)
		{
			foreach (GameObject gameObject in this.curtainBoard)
			{
				Object.Destroy(gameObject);
			}
		}
		this.curtainBoard = null;
		this.paperList = new List<SceneTreeHouse.PaperCtrl>();
		foreach (GameObject gameObject2 in this.doorGridMap.Keys)
		{
			Object.Destroy(gameObject2);
		}
		this.doorGridMap = new Dictionary<GameObject, Vector3>();
		Object.Destroy(this.doorBase);
		this.doorBase = null;
		EffectManager.BillboardCamera = null;
		AssetManager.UnloadAssetData(SceneTreeHouse.BOARD_PATH, AssetManager.OWNER.TreeHouseStage);
		AssetManager.UnloadAssetData(SceneTreeHouse.GRID_DEPEND_PATH, AssetManager.OWNER.TreeHouseStage);
		AssetManager.UnLoadByList(AssetManager.OWNER.TreeHouseStage, "", true);
		foreach (string text in SceneTreeHouse.emoEffName.Values)
		{
			EffectManager.UnloadEffect(text, AssetManager.OWNER.TreeHouseStage);
		}
		EffectManager.UnloadEffect(SceneTreeHouse.cupEffName, AssetManager.OWNER.TreeHouseStage);
		EffectManager.UnloadEffect(SceneTreeHouse.penEffName, AssetManager.OWNER.TreeHouseStage);
		EffectManager.UnloadEffect(SceneTreeHouse.penEasterEffName, AssetManager.OWNER.TreeHouseStage);
		EffectManager.UnloadEffect(SceneTreeHouse.potEffName, AssetManager.OWNER.TreeHouseStage);
		EffectManager.UnloadEffect(SceneTreeHouse.ringEffName, AssetManager.OWNER.TreeHouseStage);
		EffectManager.UnloadEffect(SceneTreeHouse.ringNewYearEffName, AssetManager.OWNER.TreeHouseStage);
		EffectManager.UnloadEffect(SceneTreeHouse.ballEffName, AssetManager.OWNER.TreeHouseStage);
		EffectManager.UnloadEffect(SceneTreeHouse.ballBasketEffName, AssetManager.OWNER.TreeHouseStage);
		EffectManager.UnloadEffect(SceneTreeHouse.gdnEffName, AssetManager.OWNER.TreeHouseStage);
		EffectManager.UnloadEffect(SceneTreeHouse.gdnChristmasEffName, AssetManager.OWNER.TreeHouseStage);
		EffectManager.UnloadEffect(SceneTreeHouse.gdnTinplateEffName, AssetManager.OWNER.TreeHouseStage);
		EffectManager.UnloadEffect(SceneTreeHouse.stkEffName, AssetManager.OWNER.TreeHouseStage);
		EffectManager.UnloadEffect(SceneTreeHouse.spnEffName, AssetManager.OWNER.TreeHouseStage);
		EffectManager.UnloadEffect(SceneTreeHouse.grnEffName, AssetManager.OWNER.TreeHouseStage);
		EffectManager.UnloadEffect(SceneTreeHouse.mkr1EffName, AssetManager.OWNER.TreeHouseStage);
		EffectManager.UnloadEffect(SceneTreeHouse.mkr2EffName, AssetManager.OWNER.TreeHouseStage);
		EffectManager.UnloadEffect(SceneTreeHouse.mkr1YewYearEffName, AssetManager.OWNER.TreeHouseStage);
		EffectManager.UnloadEffect(SceneTreeHouse.mkr2YewYearEffName, AssetManager.OWNER.TreeHouseStage);
		EffectManager.UnloadEffect(SceneTreeHouse.onpEffName, AssetManager.OWNER.TreeHouseStage);
		EffectManager.UnloadEffect(SceneTreeHouse.ladleEffName, AssetManager.OWNER.TreeHouseStage);
		EffectManager.UnloadEffect(SceneTreeHouse.jpncupEffName, AssetManager.OWNER.TreeHouseStage);
		PlayerPrefs.DeleteKey(SceneTreeHouse.TREE_HOUSE_KEY);
		foreach (TreeHouseSmallFurnitureData treeHouseSmallFurnitureData in this.smallFurnitureDataList)
		{
			foreach (TreeHouseSmallFurnitureData.MotionData motionData in treeHouseSmallFurnitureData.motionDataList)
			{
				EffectManager.UnloadEffect(motionData.modelName, AssetManager.OWNER.TreeHouseStage);
			}
			foreach (TreeHouseSmallFurnitureData.OptData optData in treeHouseSmallFurnitureData.optDataList)
			{
				EffectManager.UnloadEffect(optData.modelName, AssetManager.OWNER.TreeHouseStage);
			}
		}
		return true;
	}

	public override void OnDestroyScene()
	{
		Object.Destroy(this.guiData.baseObj);
		this.guiData.baseObj = null;
		this.guiData = null;
		Object.Destroy(this.guiOther.baseObj);
		this.guiOther.baseObj = null;
		this.guiOther = null;
		this.winMyset = null;
		this.winGetstamp = null;
		this.winSocial = null;
		this.winOpenconf = null;
		this.winFurniture = null;
		this.winFilter = null;
		this.winSort = null;
		this.winName = null;
		this.winComment = null;
		this.winMusic = null;
		this.winCharge = null;
		this.winChargeUse = null;
		this.winChargeGet = null;
		this.winMachine = null;
		this.winMachineGet = null;
		this.winVR = null;
		Object.Destroy(this.winPanel);
		this.winPanel = null;
		Object.Destroy(this.stageLocator);
		this.stageLocator = null;
		Object.Destroy(this.camL.gameObject);
		Object.Destroy(this.camR.gameObject);
		this.camL = (this.camR = null);
		this.camera = null;
		Object.Destroy(this.field);
		this.field = null;
	}

	private IEnumerator PlayTutorial()
	{
		yield return null;
		bool isFinishWindow = false;
		DataManagerGameStatus.UserFlagData ufd = DataManager.DmGameStatus.MakeUserFlagData();
		List<string> list = new List<string> { "Texture2D/Tutorial_Window/TreeHouse/tutorial_treehouse_01", "Texture2D/Tutorial_Window/TreeHouse/tutorial_treehouse_02", "Texture2D/Tutorial_Window/TreeHouse/tutorial_treehouse_03", "Texture2D/Tutorial_Window/TreeHouse/tutorial_treehouse_04", "Texture2D/Tutorial_Window/TreeHouse/tutorial_treehouse_05" };
		List<string> list2 = new List<string> { "Texture2D/Tutorial_Window/TreeHouse/tutorial_treehouse_06" };
		List<string> list3 = new List<string> { "Texture2D/Tutorial_Window/TreeHouse/tutorial_treehouse_07" };
		if (ufd.TutorialFinishFlag.TreeHouseFirst <= DataManagerGameStatus.UserFlagData.TREE_HOUSE_TUTORIAL.FIRST)
		{
			list3.InsertRange(0, list2);
			list3.InsertRange(0, list);
		}
		else if (this.batteryInfo == null)
		{
			list3.InsertRange(0, list2);
		}
		CanvasManager.HdlCmnFeedPageWindowCtrl.Open(CmnFeedPageWindowCtrl.Type.PAGE_FEED, "", list3, delegate(bool b)
		{
			isFinishWindow = true;
		});
		while (!isFinishWindow)
		{
			yield return null;
		}
		ufd.TutorialFinishFlag.TreeHouseFirst = DataManagerGameStatus.UserFlagData.TREE_HOUSE_TUTORIAL.LATEST;
		DataManager.DmGameStatus.RequestActionUpdateUserFlag(ufd);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		this.batteryInfo = DataManager.DmItem.GetUserItemData(this.batteryData.getItemId);
		this.ienum.Add(this.GetStamp(true));
		yield break;
	}

	private IEnumerator GetStamp(bool stamp = false)
	{
		SceneTreeHouse.<>c__DisplayClass394_0 CS$<>8__locals1 = new SceneTreeHouse.<>c__DisplayClass394_0();
		if (stamp)
		{
			do
			{
				yield return null;
			}
			while (DataManager.IsServerRequesting());
		}
		string text = "";
		string text2 = "";
		this.logList = new List<TreeHouseReceiveStampLog.Log>();
		ItemData userItemData = DataManager.DmItem.GetUserItemData(this.batteryData.getItemId);
		if (this.batteryInfo.num < userItemData.num)
		{
			this.logList.Add(new TreeHouseReceiveStampLog.Log(new MasterRoomReceiveStamplog
			{
				stamp_point = (long)userItemData.num,
				user_rank = userItemData.num - this.batteryInfo.num
			}));
			this.batteryInfo = userItemData;
			text = "電池";
		}
		int num = ((stamp && DataManager.DmTreeHouse.ReceiveStampLog != null) ? DataManager.DmTreeHouse.ReceiveStampLog.logList.Count : 0);
		if (num > 0)
		{
			if (!string.IsNullOrEmpty(text))
			{
				text = "と" + text;
			}
			text = "スタンプ" + DataManager.DmTreeHouse.ReceiveStampLog.receiveStampNum.ToString() + "個" + text;
			if (DataManager.DmTreeHouse.ReceiveStampLog.receiveStampNum > (long)num)
			{
				text2 = "※最新の" + num.ToString() + "件のみ表示されます";
			}
			this.logList.AddRange(DataManager.DmTreeHouse.ReceiveStampLog.logList);
		}
		CS$<>8__locals1.jmp = false;
		if (!string.IsNullOrEmpty(text))
		{
			this.winPanel.SetActive(true);
			List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>> list = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>();
			bool flag = QuestUtil.ClearConditionGrayOutButton(QuestStaticChapter.Category.ETCETERA);
			if (this.logList.Count > num && flag)
			{
				list.Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, ""));
				list.Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, ""));
			}
			else
			{
				list.Add(new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, ""));
			}
			this.winGetstamp.win.Setup(null, null, list, true, new PguiOpenWindowCtrl.Callback(CS$<>8__locals1.<GetStamp>g__decide|0), null, false);
			this.winGetstamp.win.ForceOpen();
			this.winGetstamp.msg.text = text + "を受け取りました";
			this.winGetstamp.msg2.text = text2;
			this.winGetstamp.scroll.Resize(this.logList.Count, 0);
			do
			{
				yield return null;
			}
			while (!this.winGetstamp.win.FinishedOpen());
			do
			{
				yield return null;
			}
			while (!this.winGetstamp.win.FinishedClose());
		}
		if (stamp && DataManager.DmHome.GetHomeCheckResult() != null)
		{
			DataManager.DmHome.GetHomeCheckResult().treeHouseBadgeFlag = false;
		}
		if (CS$<>8__locals1.jmp)
		{
			SceneQuest.Args args = new SceneQuest.Args();
			args.initialMap = true;
			args.category = QuestStaticChapter.Category.ETCETERA;
			args.menuBackSceneName = SceneManager.SceneName.SceneTreeHouse;
			args.menuBackSceneArgs = null;
			Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneQuest, args);
		}
		yield break;
	}

	private void SetupGetStamp(int index, GameObject go)
	{
		this.UpdateGetStamp(index, go);
	}

	private void UpdateGetStamp(int index, GameObject go)
	{
		TreeHouseReceiveStampLog.Log log = ((index >= 0 && index < this.logList.Count) ? this.logList[index] : null);
		Transform transform = go.transform.Find("BaseImage");
		if (log == null || log.stampId > 0)
		{
			transform.Find("TitleBase").gameObject.SetActive(true);
			transform.Find("Icon_Stamp").gameObject.SetActive(true);
			transform.Find("Txt_PresentUser").gameObject.SetActive(true);
			transform.Find("Txt_Date").gameObject.SetActive(true);
			transform.Find("Icon_Stamp/Texture_Item").GetComponent<PguiRawImageCtrl>().SetRawImage(this.stampIcon((log == null) ? 1 : log.stampId), true, false, null);
			transform.Find("Txt_PresentUser").GetComponent<PguiTextCtrl>().text = ((log == null) ? "" : ("＜" + log.userName + "＞\n<size=16>から受け取りました</size>"));
			transform.Find("Txt_Date").GetComponent<PguiTextCtrl>().text = ((log == null) ? "" : log.receiveTime.ToString("受け取り日時：yyyy/MM/dd\u3000HH:mm"));
			transform.Find("TitleBase_02").gameObject.SetActive(false);
			transform.Find("Icon_Item").gameObject.SetActive(false);
			transform.Find("Txt_ItemGet").gameObject.SetActive(false);
			transform.Find("Num_Own").gameObject.SetActive(false);
			transform = go.transform.Find("StampPt");
			transform.gameObject.SetActive(true);
			int num = ((log == null) ? 0 : (log.isReceiveFollow ? (log.isSendFollow ? 1 : 2) : (log.isSendFollow ? 3 : 0)));
			PguiReplaceSpriteCtrl component = transform.Find("Mark_Friend").GetComponent<PguiReplaceSpriteCtrl>();
			component.gameObject.SetActive(num > 0);
			component.Replace(num);
			transform.Find("Pt").GetComponent<PguiTextCtrl>().text = ((log == null) ? "0" : log.getStampPoint.ToString());
			return;
		}
		transform.Find("TitleBase").gameObject.SetActive(false);
		transform.Find("Icon_Stamp").gameObject.SetActive(false);
		transform.Find("Txt_PresentUser").gameObject.SetActive(false);
		transform.Find("Txt_Date").gameObject.SetActive(false);
		transform.Find("TitleBase_02").gameObject.SetActive(true);
		transform.Find("Icon_Item").gameObject.SetActive(true);
		transform.Find("Txt_ItemGet").gameObject.SetActive(true);
		transform.Find("Num_Own").gameObject.SetActive(true);
		transform.Find("Icon_Item").GetComponent<IconItemCtrl>().Setup(this.batteryInfo.staticData, log.userRank);
		transform.Find("Num_Own").GetComponent<PguiTextCtrl>().text = "（所持数\u3000" + log.getStampPoint.ToString() + "）";
		transform = go.transform.Find("StampPt");
		transform.gameObject.SetActive(false);
	}

	private void SetupGetMachine(int index, GameObject go)
	{
	}

	private void UpdateGetMachine(int index, GameObject go)
	{
		MasterRoomMachineReceiveModel masterRoomMachineReceiveModel = ((index >= 0 && index < DataManager.DmTreeHouse.GetMachineReceiveList().Count) ? DataManager.DmTreeHouse.GetMachineReceiveList()[index] : null);
		if (masterRoomMachineReceiveModel == null)
		{
			return;
		}
		Transform transform = go.transform.Find("BaseImage");
		transform.Find("Txt_Item").GetComponent<PguiTextCtrl>().text = ((masterRoomMachineReceiveModel.getItemId > 0) ? DataManager.DmItem.GetItemStaticBase(masterRoomMachineReceiveModel.getItemId).GetName() : "スタミナ");
		transform.Find("Txt_Num").GetComponent<PguiTextCtrl>().text = ((masterRoomMachineReceiveModel.getItemId > 0) ? masterRoomMachineReceiveModel.getItemNum.ToString() : masterRoomMachineReceiveModel.getStamina.ToString());
	}

	private void SetupMachineAll(int index, GameObject go)
	{
		AEImage component = go.transform.Find("BaseImage").Find("AEImage_Max").GetComponent<AEImage>();
		component.playTime = 0f;
		component.autoPlay = true;
	}

	private void UpdateMachineAll(int index, GameObject go)
	{
		List<MasterRoomMachineDataModel> treeHouseMachineFntrTimeList = DataManager.DmHome.GetHomeCheckResult().GetTreeHouseMachineFntrTimeList();
		MasterRoomMachineDataModel machineTimeData = ((index >= 0 && index < treeHouseMachineFntrTimeList.Count) ? treeHouseMachineFntrTimeList[index] : null);
		if (machineTimeData == null || this.furnitureList == null)
		{
			return;
		}
		MstMasterRoomMachineData mstMasterRoomMachineData = DataManager.DmTreeHouse.GetMstMasterRoomMachineList().Find((MstMasterRoomMachineData x) => x.id == machineTimeData.machineId);
		SceneTreeHouse.FurnitureData furnitureData = this.allFurnitureDataList.Find((SceneTreeHouse.FurnitureData x) => x.dat.GetId() == machineTimeData.furnitureId);
		Transform transform = go.transform.Find("BaseImage");
		Transform transform2 = transform.Find("Icon_Furniture_mini");
		transform2.Find("Texture_Photo").gameObject.SetActive(false);
		transform2.Find("Texture_Item").GetComponent<PguiRawImageCtrl>().SetRawImage(furnitureData.dat.GetIconName(), true, false, null);
		transform2.Find("Badge").gameObject.SetActive(false);
		transform2.Find("Current").gameObject.SetActive(false);
		transform2.Find("Remove").gameObject.SetActive(false);
		transform2.Find("Mark_FriendsAction").gameObject.SetActive(false);
		transform2.Find("Mark_FriendsAction_2").gameObject.SetActive(false);
		transform2.Find("Icon_Chara_Interior").gameObject.SetActive(false);
		transform.Find("Txt_Item").GetComponent<PguiTextCtrl>().text = ((mstMasterRoomMachineData.getItemId > 0) ? DataManager.DmItem.GetItemStaticBase(mstMasterRoomMachineData.getItemId).GetName() : "スタミナ");
		int numMachine = this.GetNumMachine(machineTimeData, mstMasterRoomMachineData);
		transform.Find("Txt_Num").GetComponent<PguiTextCtrl>().text = numMachine.ToString();
		AEImage component = transform.Find("AEImage_Max").GetComponent<AEImage>();
		if (this.IsMachineStackMax(machineTimeData, mstMasterRoomMachineData))
		{
			transform.Find("Time_Info/Num_Time").GetComponent<PguiTextCtrl>().text = "00:00:00";
			transform.Find("Time_Info/StaminaGage/Gage").GetComponent<PguiImageCtrl>().m_Image.fillAmount = 1f;
			component.playOutTime = component.duration;
			return;
		}
		int num = mstMasterRoomMachineData.getItemTime * 60 - (int)(this.GetMachinePassTime(machineTimeData) / 10000000L) % (mstMasterRoomMachineData.getItemTime * 60);
		transform.Find("Time_Info/Num_Time").GetComponent<PguiTextCtrl>().text = string.Concat(new string[]
		{
			(num / 3600).ToString(),
			":",
			(num / 60 % 60).ToString("D2"),
			":",
			(num % 60).ToString("D2")
		});
		float num2 = 1f - (float)num / ((float)mstMasterRoomMachineData.getItemTime * 60f);
		transform.Find("Time_Info/StaminaGage/Gage").GetComponent<PguiImageCtrl>().m_Image.fillAmount = num2;
		component.playOutTime = 0f;
	}

	private void SetupTreeHouseChara(int index, GameObject go)
	{
		foreach (object obj in go.transform)
		{
			foreach (object obj2 in ((Transform)obj))
			{
				((Transform)obj2).SetParent(this.hidePanel, false);
			}
		}
		int num = index * SceneTreeHouse.ScrollDeckNum;
		for (int i = 0; i < SceneTreeHouse.ScrollDeckNum; i++)
		{
			int num2 = i + num;
			if (num2 > this.dispCharaPackList.Count)
			{
				break;
			}
			CharaPackData charaPackData = ((num2 > 0) ? this.dispCharaPackList[num2 - 1] : null);
			string id = ((charaPackData == null) ? "0" : charaPackData.id.ToString());
			Transform transform = this.treehouseChara.Find((Transform itm) => itm.name == id);
			if (transform != null)
			{
				transform.SetParent(go.transform.Find("Icon_Chara" + (i + 1).ToString("D2")), false);
				IconCharaCtrl componentInChildren = transform.GetComponentInChildren<IconCharaCtrl>();
				if (componentInChildren != null)
				{
					componentInChildren.Setup(charaPackData, this.charaSortType, transform.Find("Icon_CharaSet/Fnt_Selected").gameObject.activeSelf || transform.Find("Icon_CharaSet/Txt_Disable").gameObject.activeSelf, null, 0, -1, 0);
					componentInChildren.DispPhotoPocketLevel(true);
				}
			}
		}
	}

	private void OnClickChara(Transform icn, int no)
	{
		if (this.IsTop())
		{
			this.Top2Chara();
		}
		if (this.chgChr > 0)
		{
			this.guiData.charaIcon[this.chgChr - 1].Find("Icon_Chara_TreeHouse/Icon_CharaSet/Current").gameObject.SetActive(false);
			if (this.changeList[this.chgChr] > 0)
			{
				using (List<CharaPackData>.Enumerator enumerator = this.haveCharaPackList.FindAll((CharaPackData itm) => DataManager.DmChara.GetSameCharaList(itm.id, false).Contains(this.changeList[this.chgChr])).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CharaPackData d2 = enumerator.Current;
						Transform transform = this.treehouseChara.Find((Transform itm) => itm.name == d2.id.ToString());
						if (transform != null)
						{
							transform.Find("Icon_CharaSet/Txt_Disable").gameObject.SetActive(!transform.Find("Icon_CharaSet/Fnt_Selected").gameObject.activeSelf);
							transform.Find("Icon_Chara").GetComponentInChildren<IconCharaCtrl>().IsEnableMask(true);
						}
					}
				}
			}
		}
		this.chgChr = no;
		if (!this.changeList.ContainsKey(no))
		{
			SceneTreeHouse.CharaCtrl charaCtrl = this.charaList.Find((SceneTreeHouse.CharaCtrl itm) => itm.no == this.chgChr);
			this.changeList.Add(this.chgChr, (charaCtrl == null || charaCtrl.chara == null) ? 0 : charaCtrl.chara.id);
		}
		icn.Find("Icon_Chara_TreeHouse/Icon_CharaSet/Current").gameObject.SetActive(true);
		if (this.changeList[this.chgChr] > 0)
		{
			using (List<CharaPackData>.Enumerator enumerator = this.haveCharaPackList.FindAll((CharaPackData itm) => DataManager.DmChara.GetSameCharaList(itm.id, false).Contains(this.changeList[this.chgChr])).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CharaPackData d = enumerator.Current;
					Transform transform2 = this.treehouseChara.Find((Transform itm) => itm.name == d.id.ToString());
					if (transform2 != null)
					{
						transform2.Find("Icon_CharaSet/Txt_Disable").gameObject.SetActive(false);
						transform2.Find("Icon_Chara").GetComponentInChildren<IconCharaCtrl>().IsEnableMask(transform2.Find("Icon_CharaSet/Fnt_Selected").gameObject.activeSelf);
					}
				}
			}
		}
		SoundManager.Play("prd_se_click", false, false);
	}

	private void OnClickCharaList(Transform tmp)
	{
		if (this.chgChr <= 0)
		{
			return;
		}
		if (tmp.Find("Icon_CharaSet/Fnt_Selected").gameObject.activeSelf || tmp.Find("Icon_CharaSet/Txt_Disable").gameObject.activeSelf)
		{
			return;
		}
		int id = int.Parse(tmp.name);
		CharaPackData charaPackData = this.haveCharaPackList.Find((CharaPackData itm) => itm.id == id);
		if (charaPackData == null && id > 0)
		{
			id = 0;
		}
		if (this.changeList[this.chgChr] == id)
		{
			return;
		}
		if (this.changeList[this.chgChr] > 0)
		{
			Transform transform = this.treehouseChara.Find((Transform itm) => itm.name == this.changeList[this.chgChr].ToString());
			if (transform != null)
			{
				transform.Find("Icon_CharaSet/Fnt_Selected").gameObject.SetActive(false);
				transform.Find("Icon_CharaSet/Txt_Disable").gameObject.SetActive(false);
				transform.Find("Icon_Chara").GetComponentInChildren<IconCharaCtrl>().IsEnableMask(false);
			}
			using (List<CharaPackData>.Enumerator enumerator = this.haveCharaPackList.FindAll((CharaPackData itm) => DataManager.DmChara.GetSameCharaList(itm.id, false).Contains(this.changeList[this.chgChr])).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CharaPackData d2 = enumerator.Current;
					transform = this.treehouseChara.Find((Transform itm) => itm.name == d2.id.ToString());
					if (transform != null)
					{
						transform.Find("Icon_CharaSet/Txt_Disable").gameObject.SetActive(false);
						transform.Find("Icon_Chara").GetComponentInChildren<IconCharaCtrl>().IsEnableMask(transform.Find("Icon_CharaSet/Fnt_Selected").gameObject.activeSelf);
					}
				}
			}
		}
		if (id > 0)
		{
			tmp.Find("Icon_CharaSet/Fnt_Selected").gameObject.SetActive(true);
			tmp.Find("Icon_CharaSet/Txt_Disable").gameObject.SetActive(false);
			tmp.Find("Icon_Chara").GetComponentInChildren<IconCharaCtrl>().IsEnableMask(true);
			using (List<CharaPackData>.Enumerator enumerator = this.haveCharaPackList.FindAll((CharaPackData itm) => DataManager.DmChara.GetSameCharaList(itm.id, false).Contains(id)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CharaPackData d = enumerator.Current;
					Transform transform2 = this.treehouseChara.Find((Transform itm) => itm.name == d.id.ToString());
					if (transform2 != null)
					{
						transform2.Find("Icon_CharaSet/Txt_Disable").gameObject.SetActive(false);
						transform2.Find("Icon_Chara").GetComponentInChildren<IconCharaCtrl>().IsEnableMask(false);
					}
				}
			}
		}
		this.changeList[this.chgChr] = id;
		Transform transform3 = this.guiData.charaIcon[this.chgChr - 1];
		IconCharaCtrl component = transform3.Find("Icon_Chara_TreeHouse/Icon_Chara").GetComponent<IconCharaCtrl>();
		component.Setup(charaPackData, SortFilterDefine.SortType.LEVEL, false, null, 0, -1, 0);
		component.DispPhotoPocketLevel(true);
		tmp.Find("Icon_CharaSet/AEImage_Eff_Change").GetComponent<PguiAECtrl>().PlayAnimation(PguiAECtrl.AmimeType.START, null);
		transform3.Find("Icon_Chara_TreeHouse/Icon_CharaSet/AEImage_Eff_Change").GetComponent<PguiAECtrl>().PlayAnimation(PguiAECtrl.AmimeType.START, null);
		SoundManager.Play((id > 0) ? "prd_se_click" : "prd_se_cancel", false, false);
	}

	private void OnClickCharaOk()
	{
		this.ienum.Add(this.ChangeChara());
	}

	private IEnumerator ChangeChara()
	{
		if (this.chgChr > 0)
		{
			this.guiData.charaIcon[this.chgChr - 1].Find("Icon_Chara_TreeHouse/Icon_CharaSet/Current").gameObject.SetActive(false);
			if (this.changeList[this.chgChr] > 0)
			{
				using (List<CharaPackData>.Enumerator enumerator = this.haveCharaPackList.FindAll((CharaPackData itm) => DataManager.DmChara.GetSameCharaList(itm.id, false).Contains(this.changeList[this.chgChr])).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CharaPackData d = enumerator.Current;
						Transform transform = this.treehouseChara.Find((Transform itm) => itm.name == d.id.ToString());
						if (transform != null)
						{
							transform.Find("Icon_CharaSet/Txt_Disable").gameObject.SetActive(!transform.Find("Icon_CharaSet/Fnt_Selected").gameObject.activeSelf);
							transform.Find("Icon_Chara").GetComponentInChildren<IconCharaCtrl>().IsEnableMask(true);
						}
					}
				}
			}
			this.chgChr = 0;
		}
		List<int> list = new List<int>();
		using (Dictionary<int, int>.KeyCollection.Enumerator enumerator2 = this.changeList.Keys.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				int no2 = enumerator2.Current;
				SceneTreeHouse.CharaCtrl charaCtrl = this.charaList.Find((SceneTreeHouse.CharaCtrl itm) => itm.no == no2);
				if (((charaCtrl == null || charaCtrl.chara == null) ? 0 : charaCtrl.chara.id) == this.changeList[no2])
				{
					list.Add(no2);
				}
			}
		}
		foreach (int num in list)
		{
			this.changeList.Remove(num);
		}
		if (this.changeList.Count > 0)
		{
			CanvasManager.HdlOpenWindowBasic.Setup("確認", "ツリーハウスを訪れるフレンズを変更します\n\nよろしいですか？", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, delegate(int idx)
			{
				if (idx != 1)
				{
					this.changeList = new Dictionary<int, int>();
				}
				return true;
			}, null, false);
			CanvasManager.HdlOpenWindowBasic.ForceOpen();
			do
			{
				yield return null;
			}
			while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
			if (this.changeList.Count > 0)
			{
				using (Dictionary<int, int>.KeyCollection.Enumerator enumerator2 = this.changeList.Keys.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						int no3 = enumerator2.Current;
						SceneTreeHouse.CharaCtrl charaCtrl2 = this.charaList.Find((SceneTreeHouse.CharaCtrl itm) => itm.no == no3);
						if (charaCtrl2 != null)
						{
							charaCtrl2.no = -1;
						}
					}
				}
				do
				{
					yield return null;
				}
				while (this.charaList.Find((SceneTreeHouse.CharaCtrl itm) => itm.no < 0) != null);
				using (Dictionary<int, int>.KeyCollection.Enumerator enumerator2 = this.changeList.Keys.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						int no4 = enumerator2.Current;
						CharaPackData charaPackData = this.haveCharaPackList.Find((CharaPackData itm) => itm.id == this.changeList[no4]);
						if (charaPackData != null)
						{
							this.charaList.Add(new SceneTreeHouse.CharaCtrl(no4, charaPackData));
						}
					}
				}
				List<TreeHousePutCharaData> list2 = new List<TreeHousePutCharaData>();
				for (int j = 0; j < this.guiData.charaIcon.Count; j++)
				{
					int no = j + 1;
					SceneTreeHouse.CharaCtrl charaCtrl3 = this.charaList.Find((SceneTreeHouse.CharaCtrl itm) => itm.no == no);
					int num2 = ((charaCtrl3 == null || charaCtrl3.chara == null) ? 0 : charaCtrl3.chara.id);
					list2.Add(new TreeHousePutCharaData
					{
						indexId = no,
						charaId = num2
					});
				}
				DataManager.DmTreeHouse.RequestActionSetChara(list2);
				do
				{
					yield return null;
				}
				while (DataManager.IsServerRequesting());
			}
			else
			{
				int i2;
				int i;
				for (i = 0; i < this.guiData.charaIcon.Count; i = i2 + 1)
				{
					IconCharaCtrl component = this.guiData.charaIcon[i].Find("Icon_Chara_TreeHouse/Icon_Chara").GetComponent<IconCharaCtrl>();
					SceneTreeHouse.CharaCtrl charaCtrl4 = this.charaList.Find((SceneTreeHouse.CharaCtrl itm) => itm.no == i + 1);
					component.Setup((charaCtrl4 == null) ? null : charaCtrl4.chara, SortFilterDefine.SortType.LEVEL, false, null, 0, -1, 0);
					component.DispPhotoPocketLevel(true);
					i2 = i;
				}
			}
			foreach (Transform transform2 in this.treehouseChara)
			{
				int id = int.Parse(transform2.name);
				bool flag = false;
				bool flag2 = false;
				if (id > 0)
				{
					HashSet<int> cid = DataManager.DmChara.GetSameCharaList(id, false);
					flag = this.charaList.Find((SceneTreeHouse.CharaCtrl itm) => itm.no > 0 && itm.chara != null && itm.chara.id == id) != null;
					flag2 = this.charaList.Find((SceneTreeHouse.CharaCtrl itm) => itm.no > 0 && itm.chara != null && cid.Contains(itm.chara.id)) != null;
				}
				transform2.Find("Icon_CharaSet/Fnt_Selected").gameObject.SetActive(flag);
				transform2.Find("Icon_CharaSet/Txt_Disable").gameObject.SetActive(!flag && flag2);
				IconCharaCtrl componentInChildren = transform2.GetComponentInChildren<IconCharaCtrl>();
				if (componentInChildren != null)
				{
					componentInChildren.IsEnableMask(flag || flag2);
				}
			}
		}
		if (this.IsChara())
		{
			this.Chara2Top();
		}
		yield break;
	}

	private GameObject MakeFurniture(int no, int id, Vector3 pos, float dir, bool other)
	{
		GameObject gameObject = new GameObject((other ? "O" : "M") + no.ToString() + "-" + id.ToString());
		gameObject.transform.SetParent(this.furnitureBase.transform, false);
		gameObject.transform.position = pos;
		gameObject.transform.eulerAngles = new Vector3(0f, dir, 0f);
		return gameObject;
	}

	private void SetupCategory(int index, GameObject go)
	{
		PguiToggleButtonCtrl component = go.GetComponent<PguiToggleButtonCtrl>();
		component.AddOnClickListener(new PguiToggleButtonCtrl.OnClick(this.OnClickCategory));
		if (component.GetComponent<PguiDataHolder>() == null)
		{
			component.gameObject.AddComponent<PguiDataHolder>();
		}
		this.UpdateCategory(index, go);
	}

	private void UpdateCategory(int index, GameObject go)
	{
		List<TreeHouseFurnitureStatic.Category> list = new List<TreeHouseFurnitureStatic.Category>(DataManagerTreeHouse.categoryList.Keys);
		TreeHouseFurnitureStatic.Category cat = ((index >= 0 && index < list.Count) ? list[index] : TreeHouseFurnitureStatic.Category.INVALID);
		string text = ((cat == TreeHouseFurnitureStatic.Category.INVALID) ? "" : DataManagerTreeHouse.categoryList[cat]);
		go.transform.Find("BaseImage/Txt").GetComponent<PguiTextCtrl>().text = text;
		go.GetComponent<PguiDataHolder>().id = (int)cat;
		bool flag = false;
		foreach (SceneTreeHouse.FurnitureData furnitureData in this.allFurnitureDataList.FindAll((SceneTreeHouse.FurnitureData itm) => itm.dat.category == cat && itm.num > 0))
		{
			if (!DataManager.DmItem.GetOldItemIdList().Contains(furnitureData.dat.GetId()) && !this.furnitureNew.Contains(furnitureData.dat.GetId()))
			{
				flag = true;
				break;
			}
		}
		go.transform.Find("BaseImage/Badge").gameObject.SetActive(flag);
		go.GetComponent<PguiToggleButtonCtrl>().SetToggleIndex((cat == this.categorySel) ? 1 : 0);
	}

	private bool OnClickCategory(PguiToggleButtonCtrl button, int index)
	{
		if (button.GetToggleIndex() == 1)
		{
			return false;
		}
		foreach (SceneTreeHouse.FurnitureData furnitureData in this.allFurnitureDataList.FindAll((SceneTreeHouse.FurnitureData itm) => itm.dat.category == this.categorySel && itm.num > 0))
		{
			if (!DataManager.DmItem.GetOldItemIdList().Contains(furnitureData.dat.GetId()) && !this.furnitureNew.Contains(furnitureData.dat.GetId()))
			{
				this.furnitureNew.Add(furnitureData.dat.GetId());
			}
		}
		this.categorySel = (TreeHouseFurnitureStatic.Category)button.transform.GetComponent<PguiDataHolder>().id;
		this.guiData.categoryScroll.Refresh();
		button.SetToggleIndex(0);
		this.MakefurnitureDataDisp();
		this.ienum.Add(this.UpdateNewBadge());
		return true;
	}

	private void MakefurnitureDataDisp()
	{
		if (this.categorySel != TreeHouseFurnitureStatic.Category.WALL_HANGINGS && SceneTreeHouse.furnitureDispType != SceneTreeHouse.FurnitureDispType.ALL && SceneTreeHouse.furnitureDispType != SceneTreeHouse.FurnitureDispType.FAVORITE)
		{
			SceneTreeHouse.furnitureDispType = SceneTreeHouse.FurnitureDispType.ALL;
			this.guiData.btnDisp.transform.Find("BaseImage/On/Txt").GetComponent<PguiTextCtrl>().text = "一覧";
		}
		this.furnitureDataDisp = this.allFurnitureDataList.FindAll((SceneTreeHouse.FurnitureData itm) => this.DispFunitureType(itm));
		SceneTreeHouse.FurnitureSortType furnitureSortType = (SceneTreeHouse.furnitureSortType.ContainsKey(this.categorySel) ? SceneTreeHouse.furnitureSortType[this.categorySel] : SceneTreeHouse.FurnitureSortType.NUMBER);
		if (furnitureSortType == SceneTreeHouse.FurnitureSortType.DESCEND)
		{
			this.furnitureDataDisp.Sort(new Comparison<SceneTreeHouse.FurnitureData>(this.SortFurnitureDescend));
		}
		else if (furnitureSortType == SceneTreeHouse.FurnitureSortType.ASCEND)
		{
			this.furnitureDataDisp.Sort(new Comparison<SceneTreeHouse.FurnitureData>(this.SortFurnitureAscend));
		}
		else if (furnitureSortType == SceneTreeHouse.FurnitureSortType.NAME)
		{
			this.furnitureDataDisp.Sort(new Comparison<SceneTreeHouse.FurnitureData>(this.SortFurnitureName));
		}
		else
		{
			this.furnitureDataDisp.Sort(new Comparison<SceneTreeHouse.FurnitureData>(this.SortFurnitureNumber));
		}
		this.guiData.btnSort.SetActEnable(SceneTreeHouse.furnitureSortType.ContainsKey(this.categorySel), false, true);
		this.guiData.btnSort.transform.Find("BaseImage/Txt_btn").GetComponent<PguiTextCtrl>().text = this.winSort.btn[(int)furnitureSortType].transform.Find("Num_Txt").GetComponent<PguiTextCtrl>().text;
		this.furnitureSel = ((this.furnitureDataDisp.Count > 0) ? this.allFurnitureDataList.IndexOf(this.furnitureDataDisp[0]) : (-1));
		this.guiData.furnitureScroll.Resize(this.furnitureDataDisp.Count, 0);
		this.guiData.furnitureNone.gameObject.SetActive(this.furnitureDataDisp.Count <= 0);
		if (SceneTreeHouse.furnitureDispType == SceneTreeHouse.FurnitureDispType.FAVORITE)
		{
			this.guiData.furnitureNone.text = "お気に入りに登録されているインテリアはありません";
			return;
		}
		if (SceneTreeHouse.furnitureDispType == SceneTreeHouse.FurnitureDispType.BANNER)
		{
			this.guiData.furnitureNone.text = "所持しているのぼりはありません";
			return;
		}
		if (SceneTreeHouse.furnitureDispType == SceneTreeHouse.FurnitureDispType.POSTER)
		{
			this.guiData.furnitureNone.text = "所持しているポスターはありません";
			return;
		}
		this.guiData.furnitureNone.text = "所持しているインテリアはありません";
	}

	private bool DispFunitureType(SceneTreeHouse.FurnitureData fd)
	{
		if (fd.dat.category == this.categorySel && fd.num > 0)
		{
			if (SceneTreeHouse.furnitureDispType == SceneTreeHouse.FurnitureDispType.FAVORITE)
			{
				if (this.favoriteFuniture.Contains(fd.dat.GetId()))
				{
					return true;
				}
			}
			else if (SceneTreeHouse.furnitureDispType == SceneTreeHouse.FurnitureDispType.BANNER)
			{
				if (fd.dat.modelFileName.StartsWith("captainroom_obj_wallhangings_banner_65200"))
				{
					return true;
				}
			}
			else
			{
				if (SceneTreeHouse.furnitureDispType != SceneTreeHouse.FurnitureDispType.POSTER)
				{
					return true;
				}
				if (fd.dat.modelFileName.StartsWith("captainroom_obj_wallhangings_poster_65100"))
				{
					return true;
				}
			}
		}
		return false;
	}

	private int SortFurnitureNumber(SceneTreeHouse.FurnitureData a, SceneTreeHouse.FurnitureData b)
	{
		int num = a.dat.subCategory - b.dat.subCategory;
		if (num == 0)
		{
			num = a.dat.GetId() - b.dat.GetId();
		}
		return num;
	}

	private int SortFurnitureDescend(SceneTreeHouse.FurnitureData a, SceneTreeHouse.FurnitureData b)
	{
		int num = b.sortSiz - a.sortSiz;
		if (num == 0)
		{
			num = this.SortFurnitureNumber(a, b);
		}
		return num;
	}

	private int SortFurnitureAscend(SceneTreeHouse.FurnitureData a, SceneTreeHouse.FurnitureData b)
	{
		int num = a.sortSiz - b.sortSiz;
		if (num == 0)
		{
			num = this.SortFurnitureNumber(a, b);
		}
		return num;
	}

	private int SortFurnitureName(SceneTreeHouse.FurnitureData a, SceneTreeHouse.FurnitureData b)
	{
		int num = DataManager.DmPhoto.ComparePhotoPackDataNyName(new string[]
		{
			b.dat.sortName,
			b.dat.GetName()
		}, new string[]
		{
			a.dat.sortName,
			a.dat.GetName()
		});
		if (num == 0)
		{
			num = this.SortFurnitureNumber(a, b);
		}
		return num;
	}

	private bool ExecSort(int index)
	{
		if (index == 2)
		{
			if (SceneTreeHouse.furnitureSortType.ContainsKey(this.categorySel))
			{
				SceneTreeHouse.furnitureSortType[this.categorySel] = (SceneTreeHouse.FurnitureSortType)this.winSort.btn.IndexOf(this.winSort.btn.Find((PguiToggleButtonCtrl itm) => itm.GetToggleIndex() == 1));
			}
			this.MakefurnitureDataDisp();
		}
		return true;
	}

	private bool OnClickSort(PguiToggleButtonCtrl button, int index)
	{
		if (button.GetToggleIndex() == 1)
		{
			return false;
		}
		foreach (PguiToggleButtonCtrl pguiToggleButtonCtrl in this.winSort.btn)
		{
			pguiToggleButtonCtrl.SetToggleIndex(0);
		}
		return true;
	}

	private void SetupFurniture(int index, GameObject go)
	{
		PguiButtonCtrl pguiButtonCtrl = go.GetComponent<PguiButtonCtrl>();
		pguiButtonCtrl.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickFurniture), PguiButtonCtrl.SoundType.DEFAULT);
		pguiButtonCtrl.SetActEnable(false, false, true);
		PguiDataHolder pdh = pguiButtonCtrl.GetComponent<PguiDataHolder>();
		if (pdh == null)
		{
			pdh = pguiButtonCtrl.gameObject.AddComponent<PguiDataHolder>();
		}
		pguiButtonCtrl = go.transform.Find("BaseImage/Btn_ Tid").GetComponent<PguiButtonCtrl>();
		pguiButtonCtrl.AddOnClickListener(delegate(PguiButtonCtrl button)
		{
			this.ienum.Add(this.OnClickTid(pdh));
		}, PguiButtonCtrl.SoundType.DEFAULT);
		pguiButtonCtrl = go.transform.Find("Btn_OK").GetComponent<PguiButtonCtrl>();
		pguiButtonCtrl.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickFurnitureSet), PguiButtonCtrl.SoundType.DEFAULT);
		go.transform.Find("BaseImage/BtnFavorite").GetComponent<PguiToggleButtonCtrl>().AddOnClickListener((PguiToggleButtonCtrl pbc, int toggleIndex) => this.OnClickFurnitureFavorite(pdh, toggleIndex));
		Transform transform = go.transform.Find("BaseImage/Icon_Furniture_mini");
		if (transform.GetComponent<PguiTouchTrigger>() == null)
		{
			transform.gameObject.AddComponent<PguiTouchTrigger>().AddListener(null, delegate
			{
				if (this.ienum.Count <= 0)
				{
					this.ienum.Add(this.OnClickFurnitureIconLong(pdh));
				}
			}, null, null, null);
		}
		this.UpdateFurniture(index, go);
	}

	private void UpdateFurniture(int index, GameObject go)
	{
		SceneTreeHouse.FurnitureData fd = ((index >= 0 && index < this.furnitureDataDisp.Count) ? this.furnitureDataDisp[index] : null);
		go.GetComponent<PguiDataHolder>().id = this.allFurnitureDataList.IndexOf(fd);
		go.transform.Find("BaseImage/ItemName").GetComponent<PguiTextCtrl>().text = ((fd == null) ? "" : fd.dat.GetName());
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		if (fd != null)
		{
			if (fd.dat.category == TreeHouseFurnitureStatic.Category.WALL_HANGINGS)
			{
				num = fd.siz.x;
				num3 = fd.siz.y;
				num2 = fd.siz.z;
			}
			else if (fd.dat.category == TreeHouseFurnitureStatic.Category.CURTAIN)
			{
				num = fd.siz.x * 2;
				num3 = fd.siz.y * 2;
			}
			else
			{
				num = fd.siz.x * 2;
				num3 = fd.siz.z * 2;
				if (fd.dat.category == TreeHouseFurnitureStatic.Category.LARGE_FURNITURE || fd.dat.category == TreeHouseFurnitureStatic.Category.STAND || fd.dat.category == TreeHouseFurnitureStatic.Category.GENERAL_MERCHANDISE)
				{
					num2 = fd.siz.y;
				}
			}
		}
		go.transform.Find("BaseImage/Num_Grid").GetComponent<PguiTextCtrl>().text = ((num > 0) ? ("マス：" + num.ToString() + "X" + num3.ToString()) : "");
		go.transform.Find("BaseImage/Num_Hgight_Depth").GetComponent<PguiTextCtrl>().text = ((num2 > 0) ? (((fd.dat.category == TreeHouseFurnitureStatic.Category.WALL_HANGINGS) ? "奥行" : "高さ") + "：" + num2.ToString()) : "");
		int num4 = ((this.furnitureList == null) ? 0 : this.furnitureList.FindAll((SceneTreeHouse.FurnitureCtrl itm) => itm.no > 0 && itm.data == fd).Count);
		int num5 = ((fd == null) ? 0 : fd.num);
		go.transform.Find("BaseImage/PlacementNow/Num_Placement").GetComponent<PguiTextCtrl>().text = "配置中\u3000" + num4.ToString() + "/" + num5.ToString();
		bool flag = false;
		if (fd != null && fd.dat.category == TreeHouseFurnitureStatic.Category.CURTAIN && this.curtainBoard.Find((GameObject itm) => this.furnitureList.Find((SceneTreeHouse.FurnitureCtrl itm2) => itm2.no >= 0 && itm2.board == itm) == null) == null)
		{
			flag = true;
		}
		go.transform.Find("BaseImage/Txt_Space").gameObject.SetActive(flag);
		go.transform.Find("BaseImage/Disable").gameObject.SetActive(num4 >= num5 || flag);
		go.transform.Find("BaseImage/Current").gameObject.SetActive(false);
		go.transform.Find("BaseImage/Btn_ Tid").gameObject.SetActive(num4 > 0);
		Transform transform = go.transform.Find("BaseImage/Icon_Furniture_mini");
		transform.Find("Texture_Photo").gameObject.SetActive(false);
		PguiRawImageCtrl component = transform.Find("Texture_Item").GetComponent<PguiRawImageCtrl>();
		component.gameObject.SetActive(true);
		if (fd == null)
		{
			component.SetTexture(null, true);
		}
		else
		{
			component.SetRawImage(fd.dat.GetIconName(), true, false, null);
		}
		transform.Find("Badge").gameObject.SetActive(fd != null && !DataManager.DmItem.GetOldItemIdList().Contains(fd.dat.GetId()) && !this.furnitureNew.Contains(fd.dat.GetId()));
		transform.Find("Current").gameObject.SetActive(false);
		transform.Find("Remove").gameObject.SetActive(false);
		SceneTreeHouse.MarkFriendsAction markFriendsAction = SceneTreeHouse.MarkFriendsAction.INVALID;
		SceneTreeHouse.MarkFriendsAction markFriendsAction2 = SceneTreeHouse.MarkFriendsAction.INVALID;
		if (fd != null)
		{
			if (fd.dat.charaActionId > 0)
			{
				markFriendsAction = SceneTreeHouse.MarkFriendsAction.CHARA_ACTION;
			}
			if (fd.dat.specialValue == TreeHouseFurnitureStatic.SpecialValue.POSTER_BOARD)
			{
				if (markFriendsAction == SceneTreeHouse.MarkFriendsAction.INVALID)
				{
					markFriendsAction = SceneTreeHouse.MarkFriendsAction.POSTER_BOARD;
				}
				else
				{
					markFriendsAction2 = SceneTreeHouse.MarkFriendsAction.POSTER_BOARD;
				}
			}
			else if (fd.dat.specialValue == TreeHouseFurnitureStatic.SpecialValue.CLOCK)
			{
				if (markFriendsAction == SceneTreeHouse.MarkFriendsAction.INVALID)
				{
					markFriendsAction = SceneTreeHouse.MarkFriendsAction.CLOCK;
				}
				else
				{
					markFriendsAction2 = SceneTreeHouse.MarkFriendsAction.CLOCK;
				}
			}
			else if (fd.dat.specialValue == TreeHouseFurnitureStatic.SpecialValue.BOX)
			{
				if (markFriendsAction == SceneTreeHouse.MarkFriendsAction.INVALID)
				{
					markFriendsAction = SceneTreeHouse.MarkFriendsAction.BOX;
				}
				else
				{
					markFriendsAction2 = SceneTreeHouse.MarkFriendsAction.BOX;
				}
			}
			else if (fd.dat.specialValue == TreeHouseFurnitureStatic.SpecialValue.CAMERA)
			{
				if (markFriendsAction == SceneTreeHouse.MarkFriendsAction.INVALID)
				{
					markFriendsAction = SceneTreeHouse.MarkFriendsAction.CAMERA;
				}
				else
				{
					markFriendsAction2 = SceneTreeHouse.MarkFriendsAction.CAMERA;
				}
			}
			else if (fd.dat.machineId > 0)
			{
				if (markFriendsAction == SceneTreeHouse.MarkFriendsAction.INVALID)
				{
					markFriendsAction = SceneTreeHouse.MarkFriendsAction.MACHINE;
				}
				else
				{
					markFriendsAction2 = SceneTreeHouse.MarkFriendsAction.MACHINE;
				}
			}
		}
		GameObject gameObject = transform.Find("Mark_FriendsAction").gameObject;
		GameObject gameObject2 = transform.Find("Mark_FriendsAction_2").gameObject;
		if (markFriendsAction > SceneTreeHouse.MarkFriendsAction.INVALID)
		{
			gameObject.GetComponent<PguiReplaceSpriteCtrl>().Replace((int)markFriendsAction);
		}
		if (markFriendsAction2 > SceneTreeHouse.MarkFriendsAction.INVALID)
		{
			gameObject2.GetComponent<PguiReplaceSpriteCtrl>().Replace((int)markFriendsAction2);
		}
		gameObject.SetActive(markFriendsAction > SceneTreeHouse.MarkFriendsAction.INVALID);
		gameObject2.SetActive(markFriendsAction2 > SceneTreeHouse.MarkFriendsAction.INVALID);
		PguiRawImageCtrl component2 = transform.Find("Icon_Chara_Interior").GetComponent<PguiRawImageCtrl>();
		CharaStaticData charaStaticData = ((fd == null || fd.dat.iconCharaId <= 0) ? null : DataManager.DmChara.GetCharaStaticData(fd.dat.iconCharaId));
		component2.gameObject.SetActive(charaStaticData != null);
		if (component2.gameObject.activeSelf)
		{
			component2.SetRawImage(charaStaticData.GetMiniIconName(), true, false, null);
		}
		else
		{
			component2.SetTexture(null, false);
		}
		bool flag2 = false;
		if (fd != null && this.furnitureList.FindAll((SceneTreeHouse.FurnitureCtrl itm) => itm.no > 0 && itm.data == fd).Count < fd.num && (fd.dat.category != TreeHouseFurnitureStatic.Category.CURTAIN || this.curtainBoard.Find((GameObject itm) => this.furnitureList.Find((SceneTreeHouse.FurnitureCtrl itm2) => itm2.no >= 0 && itm2.board == itm) == null) != null))
		{
			flag2 = true;
		}
		go.transform.Find("Btn_OK").GetComponent<PguiButtonCtrl>().SetActEnable(flag2, false, false);
		go.transform.Find("BaseImage/BtnFavorite").GetComponent<PguiToggleButtonCtrl>().SetToggleIndex((fd != null && this.favoriteFuniture.Contains(fd.dat.GetId())) ? 1 : 0);
	}

	private void OnClickFurniture(PguiButtonCtrl button)
	{
		SceneTreeHouse.FurnitureData furnitureData = ((this.furnitureSel >= 0 && this.furnitureSel < this.allFurnitureDataList.Count) ? this.allFurnitureDataList[this.furnitureSel] : null);
		if (furnitureData != null && !DataManager.DmItem.GetOldItemIdList().Contains(furnitureData.dat.GetId()) && !this.furnitureNew.Contains(furnitureData.dat.GetId()))
		{
			this.furnitureNew.Add(furnitureData.dat.GetId());
		}
		this.furnitureSel = button.transform.GetComponent<PguiDataHolder>().id;
		this.guiData.furnitureScroll.Refresh();
		this.guiData.categoryScroll.Refresh();
		this.ienum.Add(this.UpdateNewBadge());
	}

	private void OnClickFurnitureSet(PguiButtonCtrl button)
	{
		this.furnitureSel = button.transform.parent.GetComponent<PguiDataHolder>().id;
		SceneTreeHouse.FurnitureData furnitureData = ((this.furnitureSel >= 0 && this.furnitureSel < this.allFurnitureDataList.Count) ? this.allFurnitureDataList[this.furnitureSel] : null);
		if (furnitureData != null && !DataManager.DmItem.GetOldItemIdList().Contains(furnitureData.dat.GetId()) && !this.furnitureNew.Contains(furnitureData.dat.GetId()))
		{
			this.furnitureNew.Add(furnitureData.dat.GetId());
		}
		this.guiData.furnitureScroll.Refresh();
		this.guiData.categoryScroll.Refresh();
		if (this.IsFurniture())
		{
			this.NewFurniture();
		}
		this.ienum.Add(this.UpdateNewBadge());
	}

	private IEnumerator OnClickTid(PguiDataHolder pdh)
	{
		SceneTreeHouse.FurnitureData fd = ((pdh.id >= 0 && pdh.id < this.allFurnitureDataList.Count) ? this.allFurnitureDataList[pdh.id] : null);
		if (fd != null)
		{
			string text = "配置されているすべての\n＜" + fd.dat.GetName() + "＞を片づけます\n\nよろしいですか？";
			CanvasManager.HdlOpenWindowBasic.Setup("片づけ", text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, delegate(int idx)
			{
				if (idx != 1)
				{
					fd = null;
				}
				return true;
			}, null, false);
			CanvasManager.HdlOpenWindowBasic.ForceOpen();
			do
			{
				yield return null;
			}
			while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
		}
		if (fd != null)
		{
			this.furniturePlace = 0;
			foreach (SceneTreeHouse.FurnitureCtrl furnitureCtrl in this.furnitureList)
			{
				if (furnitureCtrl.no >= 0)
				{
					furnitureCtrl.action = null;
					if (furnitureCtrl.data == fd)
					{
						furnitureCtrl.no = -1;
						using (List<SceneTreeHouse.FurnitureCtrl>.Enumerator enumerator2 = this.furnitureList.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								SceneTreeHouse.FurnitureCtrl furnitureCtrl2 = enumerator2.Current;
								if (furnitureCtrl2.no >= 0 && !(furnitureCtrl2.depend == null))
								{
									if (furnitureCtrl.grid.ContainsKey(furnitureCtrl2.depend))
									{
										furnitureCtrl2.no = -1;
									}
									else if (furnitureCtrl.pstr.ContainsKey(furnitureCtrl2.depend))
									{
										furnitureCtrl2.no = -1;
									}
								}
							}
							continue;
						}
					}
					if (this.furniturePlace < furnitureCtrl.no)
					{
						this.furniturePlace = furnitureCtrl.no;
					}
				}
			}
			this.ChkRug(false);
			this.guiData.furnitureScroll.Refresh();
			this.ienum.Add(this.SaveFurniture(null));
		}
		yield break;
	}

	private bool OnClickFurnitureFavorite(PguiDataHolder pdh, int toggleIndex)
	{
		SceneTreeHouse.FurnitureData furnitureData = ((pdh.id >= 0 && pdh.id < this.allFurnitureDataList.Count) ? this.allFurnitureDataList[pdh.id] : null);
		if (furnitureData == null)
		{
			return false;
		}
		if (toggleIndex == 0)
		{
			this.favoriteFuniture.Add(furnitureData.dat.GetId());
		}
		else
		{
			this.favoriteFuniture.Remove(furnitureData.dat.GetId());
		}
		this.ienum.Add(this.UpdateFavorite());
		return true;
	}

	private IEnumerator UpdateFavorite()
	{
		HashSet<int> hashSet = new HashSet<int>(new List<int>(DataManager.DmTreeHouse.FavoriteFurnitureItemIdList).FindAll((int itm) => !this.favoriteFuniture.Contains(itm)));
		HashSet<int> hashSet2 = new HashSet<int>(new List<int>(this.favoriteFuniture).FindAll((int itm) => !DataManager.DmTreeHouse.FavoriteFurnitureItemIdList.Contains(itm)));
		if (hashSet.Count > 0 || hashSet2.Count > 0)
		{
			DataManager.DmTreeHouse.RequestActionUpdateFavorite(hashSet, hashSet2);
			do
			{
				yield return null;
			}
			while (DataManager.IsServerRequesting());
		}
		yield break;
	}

	private IEnumerator UpdateNewBadge()
	{
		if (this.furnitureNew.Count > 0)
		{
			DataManager.DmItem.RequestActionUpdateNewFlag(this.furnitureNew);
			do
			{
				yield return null;
			}
			while (DataManager.IsServerRequesting());
			this.furnitureNew = new List<int>();
		}
		yield break;
	}

	private IEnumerator OnClickFurnitureIconLong(PguiDataHolder pdh)
	{
		SceneTreeHouse.FurnitureData furnitureData = ((pdh.id >= 0 && pdh.id < this.allFurnitureDataList.Count) ? this.allFurnitureDataList[pdh.id] : null);
		if (furnitureData != null)
		{
			SceneTreeHouse.<>c__DisplayClass426_0 CS$<>8__locals1 = new SceneTreeHouse.<>c__DisplayClass426_0();
			CS$<>8__locals1.end = false;
			CanvasManager.HdlTreeHouseFurnitureWindowCtrl.Open(new TreeHouseFurnitureWindowCtrl.SetupParam
			{
				thfs = furnitureData.dat,
				closeEndCb = delegate
				{
					CS$<>8__locals1.end = true;
				}
			});
			do
			{
				yield return null;
			}
			while (!CS$<>8__locals1.end);
			CS$<>8__locals1 = null;
		}
		yield break;
	}

	private IEnumerator SaveFurniture(List<TreeHouseFurnitureMapping> lst = null)
	{
		this.furnitureMapSave = ((lst == null) ? this.MakeFurnitureMap() : lst);
		DataManager.DmTreeHouse.RequestActionPutFurniture(this.furnitureMapSave);
		do
		{
			yield return null;
		}
		while (DataManager.IsServerRequesting());
		yield break;
	}

	private List<TreeHouseFurnitureMapping> MakeFurnitureMap()
	{
		List<TreeHouseFurnitureMapping> list = new List<TreeHouseFurnitureMapping>();
		foreach (SceneTreeHouse.FurnitureCtrl furnitureCtrl in this.furnitureList)
		{
			if (furnitureCtrl.no > 0)
			{
				Vector3 vector = furnitureCtrl.obj.transform.position * 1000f;
				int num = (int)vector.x;
				int num2 = (int)vector.y;
				int num3 = (int)vector.z;
				int num4 = (int)this.Round30(furnitureCtrl.obj.transform.eulerAngles.y);
				if (furnitureCtrl.data.dat.category == TreeHouseFurnitureStatic.Category.RUG)
				{
					num2 = furnitureCtrl.rug;
				}
				list.Add(new TreeHouseFurnitureMapping
				{
					placementId = furnitureCtrl.no,
					furnitureId = furnitureCtrl.data.dat.GetId(),
					postion = new Vector3Int(num, num2, num3),
					angle = num4,
					effectFlag = furnitureCtrl.onoff
				});
			}
		}
		if (this.playBgm > 0 && !DataManager.DmGameStatus.MakeUserFlagData().InformationsFlag.DisableTreeHouseBgmChange)
		{
			list.Add(new TreeHouseFurnitureMapping
			{
				placementId = 0,
				furnitureId = this.playBgm,
				postion = Vector3Int.zero,
				angle = 0,
				effectFlag = false
			});
		}
		return list;
	}

	private List<TreeHouseFurnitureMapping> CheckFurnitureMap(List<TreeHouseFurnitureMapping> map)
	{
		if (map == null)
		{
			return null;
		}
		List<TreeHouseFurnitureMapping> list = this.MakeFurnitureMap();
		int i;
		for (i = 0; i < map.Count; i++)
		{
			if (i >= list.Count)
			{
				return list;
			}
			TreeHouseFurnitureMapping treeHouseFurnitureMapping = map[i];
			TreeHouseFurnitureMapping treeHouseFurnitureMapping2 = list[i];
			if (treeHouseFurnitureMapping.placementId != treeHouseFurnitureMapping2.placementId || treeHouseFurnitureMapping.furnitureId != treeHouseFurnitureMapping2.furnitureId || treeHouseFurnitureMapping.postion != treeHouseFurnitureMapping2.postion || treeHouseFurnitureMapping.angle != treeHouseFurnitureMapping2.angle || treeHouseFurnitureMapping.effectFlag != treeHouseFurnitureMapping2.effectFlag)
			{
				return list;
			}
		}
		if (i >= list.Count)
		{
			return null;
		}
		return list;
	}

	private IEnumerator ResetFurniture()
	{
		bool reset = false;
		CanvasManager.HdlOpenWindowBasic.Setup("配置リセット", "配置されているインテリアをすべて片づけます\n\nよろしいですか？", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, delegate(int idx)
		{
			if (idx == 1)
			{
				reset = true;
			}
			return true;
		}, null, false);
		CanvasManager.HdlOpenWindowBasic.ForceOpen();
		do
		{
			yield return null;
		}
		while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
		if (reset)
		{
			foreach (SceneTreeHouse.FurnitureCtrl furnitureCtrl in this.furnitureList)
			{
				furnitureCtrl.no = -1;
			}
			this.guiData.btnCancelEdit.SetActEnable(this.CheckFurnitureMap(this.furnitureMapEdit) != null, false, false);
			this.guiData.btnReset.SetActEnable(false, false, false);
			this.guiData.btnMove.SetActEnable(false, false, false);
			this.ienum.Add(this.SaveFurniture(null));
			do
			{
				yield return null;
			}
			while (this.furnitureList.Count > 0);
			this.furniturePlace = 0;
		}
		yield break;
	}

	private IEnumerator CancelFurnitureEdit()
	{
		bool cancel = false;
		CanvasManager.HdlOpenWindowBasic.Setup("編集キャンセル", "すべての配置・移動を編集前の状態に戻します\n\nよろしいですか？", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, delegate(int idx)
		{
			if (idx == 1)
			{
				cancel = true;
			}
			return true;
		}, null, false);
		CanvasManager.HdlOpenWindowBasic.ForceOpen();
		do
		{
			yield return null;
		}
		while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
		if (cancel)
		{
			foreach (SceneTreeHouse.FurnitureCtrl furnitureCtrl in this.furnitureList)
			{
				furnitureCtrl.no = -1;
			}
			this.ienum.Add(this.SaveFurniture(this.furnitureMapEdit));
			do
			{
				yield return null;
			}
			while (DataManager.IsServerRequesting() || this.furnitureList.Count > 0);
			this.SetFurniture(false);
			this.guiData.btnCancelEdit.SetActEnable(false, false, false);
			this.guiData.btnReset.SetActEnable(this.furnitureList.Count > 0 && this.isMove <= 0, false, false);
			this.guiData.btnMove.SetActEnable(this.furnitureList.Count > 0, false, false);
		}
		yield break;
	}

	private IEnumerator MysetOpen()
	{
		this.winPanel.SetActive(true);
		this.winMyset.win.ForceOpen();
		this.winMyset.list[0].name.text = DataManager.DmTreeHouse.PublicInfo.houseName;
		this.winMyset.list[0].date.text = "";
		for (int i = 1; i < this.winMyset.list.Count; i++)
		{
			TreeHouseMyset treeHouseMyset = ((DataManager.DmTreeHouse.MysetList.Count >= i) ? DataManager.DmTreeHouse.MysetList[i - 1] : new TreeHouseMyset(i, null));
			SceneTreeHouse.BAR_MYSET bar_MYSET = this.winMyset.list[i];
			bar_MYSET.num.text = treeHouseMyset.mysetId.ToString();
			bar_MYSET.name.text = treeHouseMyset.name;
			bar_MYSET.date.text = (treeHouseMyset.isDataEnable ? treeHouseMyset.saveTime.ToString("保存日時：yyyy/MM/dd\u3000HH:mm") : "");
			bar_MYSET.save.transform.Find("BaseImage/Txt").GetComponent<PguiTextCtrl>().text = (treeHouseMyset.isDataEnable ? "上書き保存" : "保存");
			bar_MYSET.save.SetActEnable(!bar_MYSET.disable.activeSelf, false, false);
			bar_MYSET.load.SetActEnable(!bar_MYSET.disable.activeSelf && treeHouseMyset.isDataEnable, false, false);
		}
		do
		{
			yield return null;
		}
		while (!this.winMyset.win.FinishedOpen());
		yield break;
	}

	private void OnClickMyset(PguiButtonCtrl button)
	{
		if (!this.winPanel.activeSelf)
		{
			return;
		}
		if (this.ienum.Count > 0)
		{
			return;
		}
		if (!this.winMyset.win.FinishedOpen())
		{
			return;
		}
		if (!CanvasManager.HdlOpenWindowBasic.FinishedClose())
		{
			return;
		}
		for (int i = 1; i < this.winMyset.list.Count; i++)
		{
			SceneTreeHouse.BAR_MYSET bar_MYSET = this.winMyset.list[i];
			if (!bar_MYSET.disable.activeSelf)
			{
				if (bar_MYSET.save == button)
				{
					this.ienum.Add(this.MysetSave(bar_MYSET));
					return;
				}
				if (bar_MYSET.load.ActEnable && bar_MYSET.load == button)
				{
					this.ienum.Add(this.MysetLoad(bar_MYSET));
					return;
				}
			}
		}
	}

	private IEnumerator MysetSave(SceneTreeHouse.BAR_MYSET bar)
	{
		SceneTreeHouse.<>c__DisplayClass434_0 CS$<>8__locals1 = new SceneTreeHouse.<>c__DisplayClass434_0();
		CS$<>8__locals1.<>4__this = this;
		int id = int.Parse(bar.num.text);
		string text = "保存";
		string text2 = "【" + DataManager.DmTreeHouse.PublicInfo.houseName + "】で\n";
		if (bar.load.ActEnable)
		{
			text = "上書き" + text;
			text2 = string.Concat(new string[]
			{
				"【",
				bar.name.text,
				"】に\n",
				text2,
				"上書き保存します"
			});
		}
		else
		{
			text2 += "保存します";
		}
		text2 += "\n\nよろしいですか？";
		CS$<>8__locals1.save = false;
		CanvasManager.HdlOpenWindowBasic.Setup(text, text2, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, new PguiOpenWindowCtrl.Callback(CS$<>8__locals1.<MysetSave>g__decide|0), null, false);
		CanvasManager.HdlOpenWindowBasic.ForceOpen();
		this.winMyset.win.ForceClose();
		do
		{
			yield return null;
		}
		while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
		if (CS$<>8__locals1.save)
		{
			List<TreeHouseFurnitureMapping> list = ((this.furnitureMapSave == null) ? null : this.CheckFurnitureMap(this.furnitureMapSave));
			if (list != null)
			{
				DataManager.DmTreeHouse.RequestActionPutFurniture(this.furnitureMapSave = list);
				do
				{
					yield return null;
				}
				while (DataManager.IsServerRequesting());
			}
			DataManager.DmTreeHouse.RequestActionSaveByMyset(id);
			do
			{
				yield return null;
			}
			while (DataManager.IsServerRequesting());
		}
		else
		{
			do
			{
				yield return null;
			}
			while (!this.winMyset.win.FinishedOpen());
		}
		yield break;
	}

	private IEnumerator MysetLoad(SceneTreeHouse.BAR_MYSET bar)
	{
		SceneTreeHouse.<>c__DisplayClass435_0 CS$<>8__locals1 = new SceneTreeHouse.<>c__DisplayClass435_0();
		CS$<>8__locals1.<>4__this = this;
		int id = int.Parse(bar.num.text);
		string text = "【" + bar.name.text + "】を\n読み込みます\n\nよろしいですか？";
		text += "\n\n<color=#ff0000ff>現在編集中の配置は上書きされます</color>";
		CS$<>8__locals1.load = false;
		CanvasManager.HdlOpenWindowBasic.Setup("読み込み", text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, new PguiOpenWindowCtrl.Callback(CS$<>8__locals1.<MysetLoad>g__decide|0), null, false);
		CanvasManager.HdlOpenWindowBasic.ForceOpen();
		this.winMyset.win.ForceClose();
		do
		{
			yield return null;
		}
		while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
		if (CS$<>8__locals1.load)
		{
			foreach (SceneTreeHouse.FurnitureCtrl furnitureCtrl in this.furnitureList)
			{
				furnitureCtrl.no = -1;
			}
			DataManager.DmTreeHouse.RequestActionLoadByMyset(id);
			do
			{
				yield return null;
			}
			while (DataManager.IsServerRequesting() || this.furnitureList.Count > 0);
			this.SetFurniture(false);
		}
		else
		{
			do
			{
				yield return null;
			}
			while (!this.winMyset.win.FinishedOpen());
		}
		yield break;
	}

	private IEnumerator SocialOpen()
	{
		if (!this.socialInfo)
		{
			DataManager.DmTreeHouse.RequestGetSocialTabData(TreeHouseSocialTabType.INVALID);
			do
			{
				yield return null;
			}
			while (DataManager.IsServerRequesting());
			this.socialInfo = true;
			this.winSocial.date.text = "対象期間\u3000" + TimeManager.Now.AddDays(-7.0).ToString("yyyy/MM/dd\u300000:00:00～") + TimeManager.Now.AddDays(-1.0).ToString("yyyy/MM/dd\u300023:59:59");
			this.winSocial.last.text = "最終更新\u3000" + TimeManager.Now.ToString("yyyy/MM/dd\u3000HH:mm");
			this.winSocial.scrList.Resize(0, 0);
			this.winSocial.scrRank.Resize(0, 0);
			this.winSocial.tab.Setup(0, new PguiTabGroupCtrl.OnSelectTab(this.onClickTabSocial));
		}
		this.winPanel.SetActive(true);
		this.winSocial.win.ForceOpen();
		this.SetupSocial(this.winSocial.tab.SelectIndex);
		int lstidx = (this.socialIndex.ContainsKey(this.socialTabTyp) ? this.socialIndex[this.socialTabTyp] : 0);
		do
		{
			this.winSocial.scrList.ForceFocus(lstidx);
			this.winSocial.scrRank.ForceFocus(lstidx);
			yield return null;
		}
		while (!this.winSocial.win.FinishedOpen());
		this.winSocial.scrList.ForceFocus(lstidx);
		this.winSocial.scrRank.ForceFocus(lstidx);
		yield break;
	}

	private bool onClickTabSocial(int idx)
	{
		if (!this.IsTop() && !this.IsOther())
		{
			return false;
		}
		if (!this.winPanel.activeSelf)
		{
			return false;
		}
		if (this.ienum.Count > 0)
		{
			return false;
		}
		if (!this.winSocial.win.FinishedOpen())
		{
			return false;
		}
		this.SaveSocialIndex();
		this.SetupSocial(idx);
		return true;
	}

	private void SetupSocial(int idx)
	{
		this.socialTabTyp = idx + TreeHouseSocialTabType.FOLLOW;
		this.winSocial.header.Replace((this.socialTabTyp == TreeHouseSocialTabType.PUBLIC) ? 2 : 1);
		this.socialUserList = null;
		if (this.socialTabTyp == TreeHouseSocialTabType.FOLLOW)
		{
			this.socialUserList = DataManager.DmTreeHouse.SocialFollowDataList;
		}
		else if (this.socialTabTyp == TreeHouseSocialTabType.PASSING)
		{
			this.socialUserList = DataManager.DmTreeHouse.SocialPassingDataList;
		}
		else if (this.socialTabTyp == TreeHouseSocialTabType.STAMP_HISTORY)
		{
			this.socialUserList = DataManager.DmTreeHouse.SocialStampHistoryDataList;
		}
		this.socialUserIdx = (this.socialIndex.ContainsKey(this.socialTabTyp) ? this.socialIndex[this.socialTabTyp] : 0);
		this.socialVisitList = new List<int>();
		this.winSocial.scrList.transform.parent.gameObject.SetActive(this.socialUserList != null);
		if (this.socialUserList != null)
		{
			this.winSocial.scrList.Resize(this.socialUserList.Count, this.socialUserIdx);
			this.winSocial.noData.SetActive(this.socialUserList.Count <= 0);
		}
		List<TreeHouseSocialUser> list = ((this.socialTabTyp == TreeHouseSocialTabType.RANKING) ? DataManager.DmTreeHouse.SocialRankingDataList : null);
		this.winSocial.scrRank.transform.parent.gameObject.SetActive(list != null);
		if (list != null)
		{
			this.winSocial.scrRank.Resize(list.Count, this.socialUserIdx);
			this.winSocial.noData.SetActive(list.Count <= 0);
		}
		this.winSocial.openConf.SetActive(this.socialTabTyp == TreeHouseSocialTabType.PUBLIC);
		if (this.winSocial.openConf.activeSelf)
		{
			this.SetOpenConf();
			string text = "１週間の対象期間\u3000" + TimeManager.Now.AddDays(-7.0).ToString("yyyy/MM/dd\u300000:00:00～") + TimeManager.Now.AddDays(-1.0).ToString("yyyy/MM/dd\u300023:59:59");
			this.winSocial.openConf.transform.Find("Txt_Date").GetComponent<PguiTextCtrl>().text = text;
			int j;
			int i;
			for (i = 0; i < this.winSocial.stamp.Count; i = j + 1)
			{
				Transform transform = this.winSocial.stamp[i];
				TreeHousePublicInfo.ReceiveStamp receiveStamp = DataManager.DmTreeHouse.PublicInfo.receiveStampList.Find((TreeHousePublicInfo.ReceiveStamp itm) => itm.stampId == i + 1);
				transform.Find("Txt_Toal/Num").GetComponent<PguiTextCtrl>().text = ((receiveStamp == null) ? "0" : receiveStamp.totalPoint.ToString());
				transform.Find("Txt_Week/Num").GetComponent<PguiTextCtrl>().text = ((receiveStamp == null) ? "0" : receiveStamp.monthlyPoint.ToString());
				transform.Find("Txt_Today/Num").GetComponent<PguiTextCtrl>().text = ((receiveStamp == null) ? "0" : receiveStamp.dailyPoint.ToString());
				j = i;
			}
			this.winSocial.noData.SetActive(false);
		}
		else if (!this.winSocial.scrList.transform.parent.gameObject.activeSelf && !this.winSocial.scrRank.transform.parent.gameObject.activeSelf)
		{
			this.winSocial.noData.SetActive(true);
		}
		this.winSocial.date.gameObject.SetActive(this.socialTabTyp == TreeHouseSocialTabType.PASSING || this.socialTabTyp == TreeHouseSocialTabType.RANKING);
		this.winSocial.search.SetActive(this.socialTabTyp == TreeHouseSocialTabType.SEARCH);
		if (this.winSocial.search.activeSelf)
		{
			this.winSocial.noData.SetActive(false);
		}
	}

	private void SaveSocialIndex()
	{
		if (this.socialIndex.ContainsKey(this.socialTabTyp))
		{
			this.socialIndex[this.socialTabTyp] = ((this.socialTabTyp == TreeHouseSocialTabType.RANKING) ? this.winSocial.scrRank.CalcCurrentFocusIndex() : this.winSocial.scrList.CalcCurrentFocusIndex());
		}
	}

	private void SetOpenConf()
	{
		this.winSocial.openConf.transform.Find("Name/Txt").GetComponent<PguiTextCtrl>().text = DataManager.DmTreeHouse.PublicInfo.houseName;
		this.winSocial.openConf.transform.Find("Comment/Txt").GetComponent<PguiTextCtrl>().text = DataManager.DmTreeHouse.PublicInfo.houseComment;
		string text = "";
		if (DataManager.DmTreeHouse.PublicInfo.publicType == TreeHousePublicInfo.PublicType.PUBLIC)
		{
			text = "公開中";
		}
		else if (DataManager.DmTreeHouse.PublicInfo.publicType == TreeHousePublicInfo.PublicType.FOLLOW_ONLY)
		{
			text = "フォロワーのみ公開中";
		}
		else if (DataManager.DmTreeHouse.PublicInfo.publicType == TreeHousePublicInfo.PublicType.PRIVATE)
		{
			text = "非公開";
		}
		this.winSocial.openConf.transform.Find("OpenConf/Txt").GetComponent<PguiTextCtrl>().text = text;
	}

	private void OnClickSocial(PguiButtonCtrl button)
	{
		if (!this.IsTop() && !this.IsOther())
		{
			return;
		}
		if (!this.winPanel.activeSelf)
		{
			return;
		}
		if (this.ienum.Count > 0)
		{
			return;
		}
		if (!this.winSocial.win.FinishedOpen())
		{
			return;
		}
		if (!this.winName.win.FinishedClose() || !this.winComment.win.FinishedClose() || !this.winOpenconf.win.FinishedClose())
		{
			return;
		}
		if (!CanvasManager.HdlOpenWindowBasic.FinishedClose())
		{
			return;
		}
		if (this.socialTabTyp == TreeHouseSocialTabType.PUBLIC)
		{
			if (button == this.winSocial.btnName)
			{
				this.ienum.Add(this.NameOpen());
				return;
			}
			if (button == this.winSocial.btnComment)
			{
				this.ienum.Add(this.CommentOpen());
				return;
			}
			if (button == this.winSocial.btnOpenconf)
			{
				this.ienum.Add(this.ConfOpen());
				return;
			}
			if (button == this.winSocial.btnConfCopy)
			{
				this.ienum.Add(this.CopyMyid());
				return;
			}
		}
		else if (this.socialTabTyp == TreeHouseSocialTabType.SEARCH)
		{
			if (button == this.winSocial.btnVisit)
			{
				int num;
				if (!int.TryParse(this.winSocial.inputField.text, out num))
				{
					num = 0;
				}
				this.ienum.Add(this.SearchFriend(num));
				return;
			}
			if (button == this.winSocial.btnCopy)
			{
				this.ienum.Add(this.CopyMyid());
			}
		}
	}

	private void SetupSocialList(int index, GameObject go)
	{
		go.GetComponent<PguiButtonCtrl>().SetActEnable(false, false, true);
		PguiButtonCtrl component = go.transform.Find("BaseImage/Img_Line/Btn_Mission").GetComponent<PguiButtonCtrl>();
		component.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickVisit), PguiButtonCtrl.SoundType.DEFAULT);
		PguiDataHolder pguiDataHolder = component.GetComponent<PguiDataHolder>();
		if (pguiDataHolder == null)
		{
			pguiDataHolder = component.gameObject.AddComponent<PguiDataHolder>();
		}
		pguiDataHolder.id = index;
		this.UpdateSocialList(index, go);
	}

	private void UpdateSocialList(int index, GameObject go)
	{
		TreeHouseSocialUser treeHouseSocialUser = ((this.socialUserList != null && index >= 0 && index < this.socialUserList.Count) ? this.socialUserList[index] : null);
		this.SetUserData(go.transform, treeHouseSocialUser, index);
	}

	private void SetupSocialRank(int index, GameObject go)
	{
		go.GetComponent<PguiButtonCtrl>().SetActEnable(false, false, true);
		go.transform.Find("TreeHouse_Social_ListBar").GetComponent<PguiButtonCtrl>().SetActEnable(false, false, true);
		PguiButtonCtrl component = go.transform.Find("TreeHouse_Social_ListBar/BaseImage/Img_Line/Btn_Mission").GetComponent<PguiButtonCtrl>();
		component.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickVisit), PguiButtonCtrl.SoundType.DEFAULT);
		PguiDataHolder pguiDataHolder = component.GetComponent<PguiDataHolder>();
		if (pguiDataHolder == null)
		{
			pguiDataHolder = component.gameObject.AddComponent<PguiDataHolder>();
		}
		pguiDataHolder.id = index;
		this.UpdateSocialRank(index, go);
	}

	private void UpdateSocialRank(int index, GameObject go)
	{
		List<TreeHouseSocialUser> list = ((this.socialTabTyp == TreeHouseSocialTabType.RANKING) ? DataManager.DmTreeHouse.SocialRankingDataList : null);
		TreeHouseSocialUser treeHouseSocialUser = ((list != null && index >= 0 && index < list.Count) ? list[index] : null);
		Transform transform = go.transform.Find("Num_Rank");
		transform.Find("Rank_1").gameObject.SetActive(treeHouseSocialUser != null && treeHouseSocialUser.rankingNo <= 1);
		transform.Find("Rank_2").gameObject.SetActive(treeHouseSocialUser != null && treeHouseSocialUser.rankingNo == 2);
		transform.Find("Rank_3").gameObject.SetActive(treeHouseSocialUser != null && treeHouseSocialUser.rankingNo == 3);
		transform.Find("Rank_4_10").gameObject.SetActive(treeHouseSocialUser != null && treeHouseSocialUser.rankingNo >= 4 && treeHouseSocialUser.rankingNo <= 10);
		transform.Find("Rank_11_100").gameObject.SetActive(treeHouseSocialUser != null && treeHouseSocialUser.rankingNo >= 11 && treeHouseSocialUser.rankingNo <= 100);
		transform.Find("Rank_101_200").gameObject.SetActive(treeHouseSocialUser != null && treeHouseSocialUser.rankingNo >= 101);
		transform.Find("Rank_4_10/Num").GetComponent<PguiTextCtrl>().text = ((treeHouseSocialUser == null) ? "" : treeHouseSocialUser.rankingNo.ToString());
		transform.Find("Rank_11_100/Num").GetComponent<PguiTextCtrl>().text = ((treeHouseSocialUser == null) ? "" : treeHouseSocialUser.rankingNo.ToString());
		transform.Find("Rank_101_200/Num").GetComponent<PguiTextCtrl>().text = ((treeHouseSocialUser == null) ? "" : treeHouseSocialUser.rankingNo.ToString());
		this.SetUserData(go.transform.Find("TreeHouse_Social_ListBar"), treeHouseSocialUser, index);
	}

	private void SetUserData(Transform tmp, TreeHouseSocialUser thsu, int index)
	{
		PguiButtonCtrl component = tmp.Find("BaseImage/Img_Line/Btn_Mission").GetComponent<PguiButtonCtrl>();
		component.SetActEnable(thsu != null && thsu.isVisit && thsu.friendId != DataManager.DmUserInfo.friendId && (!this.IsOther() || thsu.friendId != DataManager.DmTreeHouse.SocialVisitUserData.friendId), false, false);
		component.GetComponent<PguiDataHolder>().id = ((thsu == null) ? (-1) : index);
		tmp.Find("Disable").gameObject.SetActive(this.IsOther() && thsu != null && thsu.friendId == DataManager.DmTreeHouse.SocialVisitUserData.friendId);
		tmp = tmp.Find("BaseImage");
		tmp.Find("StampSent").gameObject.SetActive(thsu != null && thsu.isFinishSendStamp);
		tmp.Find("Num_Rank").GetComponent<PguiTextCtrl>().ReplaceTextByDefault("Param01", (thsu == null) ? "-" : thsu.userRank.ToString());
		tmp.Find("Num_Rank/UserName").GetComponent<PguiTextCtrl>().text = ((thsu == null) ? "" : thsu.userName);
		tmp.Find("Img_Line/Txt_Date").GetComponent<PguiTextCtrl>().text = ((thsu == null) ? "" : thsu.houseName);
		tmp.Find("Img_Line/MarkNew").gameObject.SetActive(thsu != null && thsu.isDispNew);
		int num = ((thsu == null) ? 0 : (thsu.isReceiveFollow ? (thsu.isSendFollow ? 1 : 2) : (thsu.isSendFollow ? 3 : 0)));
		PguiReplaceSpriteCtrl component2 = tmp.Find("Mark_Friend").GetComponent<PguiReplaceSpriteCtrl>();
		component2.gameObject.SetActive(num > 0);
		component2.Replace(num);
		IconCharaCtrl iconCharaCtrl = tmp.Find("Icon_Chara").GetComponentInChildren<IconCharaCtrl>();
		if (iconCharaCtrl == null)
		{
			iconCharaCtrl = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Chara).GetComponent<IconCharaCtrl>();
			iconCharaCtrl.transform.SetParent(tmp.Find("Icon_Chara"), false);
		}
		iconCharaCtrl.Setup((thsu == null) ? null : CharaPackData.MakeInitial(thsu.favoriteCharaId), SortFilterDefine.SortType.INVALID, false, null, 0, (thsu == null) ? (-1) : thsu.favoriteCharaFaceId, 0);
		iconCharaCtrl.DispRanking();
		if (this.socialTabTyp == TreeHouseSocialTabType.STAMP_HISTORY)
		{
			tmp.Find("Contents01").gameObject.SetActive(false);
			tmp = tmp.Find("Contents02");
			tmp.gameObject.SetActive(thsu != null);
			tmp.Find("Icon_Stamp/Texture_Item").GetComponent<PguiRawImageCtrl>().SetRawImage(this.stampIcon((thsu == null) ? 1 : thsu.actionStampId), true, false, null);
			string text = "";
			if (thsu != null)
			{
				text = text + "を" + TimeManager.MakeTimeSpanText(thsu.actionTime, TimeManager.Now) + "前に";
				text += ((thsu.stampActionType == TreeHouseSocialUser.StampActionType.SEND_BY_MINE) ? "送りました" : "受け取りました");
				tmp.Find("MarkSend_Receive").GetComponent<PguiReplaceSpriteCtrl>().Replace((thsu.stampActionType == TreeHouseSocialUser.StampActionType.SEND_BY_MINE) ? 1 : 2);
			}
			tmp.Find("Txt_PresentUser").GetComponent<PguiTextCtrl>().text = text;
			return;
		}
		tmp.Find("Contents02").gameObject.SetActive(false);
		tmp = tmp.Find("Contents01");
		tmp.gameObject.SetActive(thsu != null);
		tmp.Find("Comment/Txt_Comment").GetComponent<PguiTextCtrl>().text = ((thsu == null) ? "" : thsu.houseComment);
		string text2 = "";
		string text3 = "";
		if (thsu != null)
		{
			if (this.socialTabTyp == TreeHouseSocialTabType.FOLLOW)
			{
				text2 = "最終更新";
				text3 = TimeManager.MakeTimeSpanText(thsu.updateTime, TimeManager.Now) + "前";
			}
			else
			{
				text2 = "獲得スタンプポイント";
				text3 = thsu.getStampPoint.ToString();
			}
		}
		tmp.Find("Txt_PresentUser").GetComponent<PguiTextCtrl>().ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[] { text2, text3 });
	}

	private void OnClickVisit(PguiButtonCtrl button)
	{
		if (!this.winPanel.activeSelf || !this.winSocial.win.FinishedOpen() || (!this.IsTop() && !this.IsOther()) || this.ienum.Count > 0)
		{
			return;
		}
		List<TreeHouseSocialUser> list = ((this.socialTabTyp == TreeHouseSocialTabType.RANKING) ? DataManager.DmTreeHouse.SocialRankingDataList : this.socialUserList);
		this.socialUserIdx = button.GetComponent<PguiDataHolder>().id;
		TreeHouseSocialUser treeHouseSocialUser = ((list != null && this.socialUserIdx >= 0 && this.socialUserIdx < list.Count) ? list[this.socialUserIdx] : null);
		if (treeHouseSocialUser == null || !treeHouseSocialUser.isVisit)
		{
			return;
		}
		if (treeHouseSocialUser.friendId == DataManager.DmUserInfo.friendId)
		{
			return;
		}
		if (this.IsOther() && treeHouseSocialUser.friendId == DataManager.DmTreeHouse.SocialVisitUserData.friendId)
		{
			return;
		}
		this.socialVisitList = new List<int> { treeHouseSocialUser.friendId };
		this.SaveSocialIndex();
		this.ienum.Add(this.StartOther(treeHouseSocialUser.friendId));
	}

	private void OnClickNextVisit()
	{
		if (this.socialVisitUserList != null && (this.socialUserList == null || this.socialUserList.Count == 0))
		{
			this.socialUserList = this.socialVisitUserList;
			this.socialUserIdx = this.socialVisitUserIdx;
		}
		if (this.socialUserList == null)
		{
			return;
		}
		if (this.winPanel.activeSelf || !this.winSocial.win.FinishedClose() || !this.IsOther() || this.ienum.Count > 0)
		{
			return;
		}
		if (this.socialUserIdx >= this.socialUserList.Count)
		{
			return;
		}
		TreeHouseSocialUser treeHouseSocialUser = this.socialUserList[this.socialUserIdx];
		this.socialVisitList.Add(treeHouseSocialUser.friendId);
		if (this.socialIndex.ContainsKey(this.socialTabTyp))
		{
			this.socialIndex[this.socialTabTyp] = this.socialUserIdx;
		}
		this.ienum.Add(this.StartOther(treeHouseSocialUser.friendId));
	}

	private void OnClickStamp(Transform tmp, int id)
	{
		if (!this.IsOther() || this.IsOtherInfo() || this.IsOtherHide() || this.IsOtherCamera() || this.IsOtherCameraHide())
		{
			return;
		}
		if (DataManager.DmTreeHouse.SocialVisitUserData.isFinishSendStamp)
		{
			return;
		}
		using (IEnumerator enumerator = tmp.parent.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (((Transform)enumerator.Current).Find("Txt_Send").gameObject.activeSelf)
				{
					return;
				}
			}
		}
		if (this.ienum.Count > 0)
		{
			return;
		}
		this.ienum.Add(this.SendStamp(tmp, id));
		SoundManager.Play("prd_se_click", false, false);
	}

	private IEnumerator SendStamp(Transform tmp, int id)
	{
		DataManager.DmTreeHouse.RequestActionSendStamp(DataManager.DmTreeHouse.SocialVisitUserData.friendId, id, this.visitTabTyp);
		do
		{
			yield return null;
		}
		while (DataManager.IsServerRequesting());
		PguiAECtrl ae = tmp.Find("AEImage_Eff_Send").GetComponent<PguiAECtrl>();
		ae.gameObject.SetActive(true);
		ae.PlayAnimation(PguiAECtrl.AmimeType.START, delegate
		{
			ae.gameObject.SetActive(false);
		});
		using (IEnumerator enumerator = tmp.parent.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				((Transform)obj).Find("Txt_Send").gameObject.SetActive(true);
			}
			yield break;
		}
		yield break;
	}

	private IEnumerator NameOpen()
	{
		SceneTreeHouse.<>c__DisplayClass451_0 CS$<>8__locals1 = new SceneTreeHouse.<>c__DisplayClass451_0();
		this.winSocial.win.ForceClose();
		CS$<>8__locals1.cb = false;
		CS$<>8__locals1.chg = false;
		this.winName.win.SetupButtonOnly(new PguiOpenWindowCtrl.Callback(CS$<>8__locals1.<NameOpen>g__decide|0));
		this.winName.win.ForceOpen();
		this.winName.inputField.text = DataManager.DmTreeHouse.PublicInfo.houseName;
		this.winName.inputField.textComponent.text = DataManager.DmTreeHouse.PublicInfo.houseName;
		this.winName.errorMessage.SetActive(false);
		do
		{
			yield return null;
		}
		while (!this.winName.win.FinishedOpen());
		for (;;)
		{
			CS$<>8__locals1.cb = false;
			do
			{
				yield return null;
			}
			while (!CS$<>8__locals1.cb);
			if (!CS$<>8__locals1.chg)
			{
				goto IL_0243;
			}
			if (string.IsNullOrEmpty(this.winName.inputField.text))
			{
				this.winName.errorMessage.SetActive(true);
				this.winName.errorMessage.GetComponent<PguiTextCtrl>().text = "名前を入力してください";
			}
			else
			{
				DataManager.DmTreeHouse.RequestActionUpdatePublicInfo(this.winName.inputField.text, DataManager.DmTreeHouse.PublicInfo.houseComment, DataManager.DmTreeHouse.PublicInfo.publicType);
				do
				{
					yield return null;
				}
				while (DataManager.IsServerRequesting());
				if (DataManager.DmTreeHouse.PublicNameUpdateSuccess)
				{
					break;
				}
				this.winName.errorMessage.SetActive(true);
				this.winName.errorMessage.GetComponent<PguiTextCtrl>().text = "この名前は利用できません";
			}
		}
		this.SetOpenConf();
		this.winName.win.ForceClose();
		IL_0243:
		this.winSocial.win.ForceOpen();
		do
		{
			yield return null;
		}
		while (!this.winSocial.win.FinishedOpen());
		yield break;
	}

	private IEnumerator CommentOpen()
	{
		SceneTreeHouse.<>c__DisplayClass452_0 CS$<>8__locals1 = new SceneTreeHouse.<>c__DisplayClass452_0();
		this.winSocial.win.ForceClose();
		CS$<>8__locals1.cb = false;
		CS$<>8__locals1.chg = false;
		this.winComment.win.SetupButtonOnly(new PguiOpenWindowCtrl.Callback(CS$<>8__locals1.<CommentOpen>g__decide|0));
		this.winComment.win.ForceOpen();
		this.winComment.inputField.text = DataManager.DmTreeHouse.PublicInfo.houseComment;
		this.winComment.inputField.textComponent.text = DataManager.DmTreeHouse.PublicInfo.houseComment;
		this.winComment.errorMessage.SetActive(false);
		do
		{
			yield return null;
		}
		while (!this.winComment.win.FinishedOpen());
		for (;;)
		{
			CS$<>8__locals1.cb = false;
			do
			{
				yield return null;
			}
			while (!CS$<>8__locals1.cb);
			if (!CS$<>8__locals1.chg)
			{
				goto IL_01E2;
			}
			DataManager.DmTreeHouse.RequestActionUpdatePublicInfo(DataManager.DmTreeHouse.PublicInfo.houseName, this.winComment.inputField.text, DataManager.DmTreeHouse.PublicInfo.publicType);
			do
			{
				yield return null;
			}
			while (DataManager.IsServerRequesting());
			if (DataManager.DmTreeHouse.PublicCommentUpdateSuccess)
			{
				break;
			}
			this.winComment.errorMessage.SetActive(true);
		}
		this.SetOpenConf();
		this.winComment.win.ForceClose();
		IL_01E2:
		this.winSocial.win.ForceOpen();
		do
		{
			yield return null;
		}
		while (!this.winSocial.win.FinishedOpen());
		yield break;
	}

	private IEnumerator ConfOpen()
	{
		SceneTreeHouse.<>c__DisplayClass453_0 CS$<>8__locals1 = new SceneTreeHouse.<>c__DisplayClass453_0();
		this.winSocial.win.ForceClose();
		CS$<>8__locals1.cb = false;
		CS$<>8__locals1.chg = false;
		this.winOpenconf.win.SetupButtonOnly(new PguiOpenWindowCtrl.Callback(CS$<>8__locals1.<ConfOpen>g__decide|0));
		this.winOpenconf.win.ForceOpen();
		this.winOpenconf.btnOpen.SetToggleIndex((DataManager.DmTreeHouse.PublicInfo.publicType == TreeHousePublicInfo.PublicType.PUBLIC) ? 1 : 0);
		this.winOpenconf.btnFollower.SetToggleIndex((DataManager.DmTreeHouse.PublicInfo.publicType == TreeHousePublicInfo.PublicType.FOLLOW_ONLY) ? 1 : 0);
		this.winOpenconf.btnPrivate.SetToggleIndex((DataManager.DmTreeHouse.PublicInfo.publicType == TreeHousePublicInfo.PublicType.PRIVATE) ? 1 : 0);
		do
		{
			yield return null;
		}
		while (!this.winOpenconf.win.FinishedOpen());
		CS$<>8__locals1.cb = false;
		do
		{
			yield return null;
		}
		while (!CS$<>8__locals1.cb);
		TreeHousePublicInfo.PublicType publicType = ((this.winOpenconf.btnOpen.GetToggleIndex() == 1) ? TreeHousePublicInfo.PublicType.PUBLIC : ((this.winOpenconf.btnFollower.GetToggleIndex() == 1) ? TreeHousePublicInfo.PublicType.FOLLOW_ONLY : TreeHousePublicInfo.PublicType.PRIVATE));
		if (CS$<>8__locals1.chg && publicType != DataManager.DmTreeHouse.PublicInfo.publicType)
		{
			DataManager.DmTreeHouse.RequestActionUpdatePublicInfo(DataManager.DmTreeHouse.PublicInfo.houseName, DataManager.DmTreeHouse.PublicInfo.houseComment, publicType);
			do
			{
				yield return null;
			}
			while (DataManager.IsServerRequesting());
			this.SetOpenConf();
		}
		this.winSocial.win.ForceOpen();
		do
		{
			yield return null;
		}
		while (!this.winSocial.win.FinishedOpen());
		yield break;
	}

	private bool OnClickOpenConf(PguiToggleButtonCtrl button, int index)
	{
		if (button.GetToggleIndex() == 1)
		{
			return false;
		}
		if (button != this.winOpenconf.btnOpen)
		{
			this.winOpenconf.btnOpen.SetToggleIndex(0);
		}
		if (button != this.winOpenconf.btnFollower)
		{
			this.winOpenconf.btnFollower.SetToggleIndex(0);
		}
		if (button != this.winOpenconf.btnPrivate)
		{
			this.winOpenconf.btnPrivate.SetToggleIndex(0);
		}
		return true;
	}

	private IEnumerator SearchFriend(int friendId)
	{
		string text;
		if (friendId == DataManager.DmUserInfo.friendId)
		{
			text = "自分を訪問することはできません";
		}
		else if (this.IsOther() && friendId == DataManager.DmTreeHouse.SocialVisitUserData.friendId)
		{
			text = "訪問中です";
		}
		else
		{
			DataManager.DmHelper.RequestSearchHelper(friendId);
			do
			{
				yield return null;
			}
			while (DataManager.IsServerRequesting());
			if (DataManager.DmHelper.GetSearchHelperResult() != null)
			{
				this.ienum.Add(this.StartOther(friendId));
				yield break;
			}
			text = "該当するプレイヤーが見つかりませんでした";
		}
		this.winSocial.win.ForceClose();
		CanvasManager.HdlOpenWindowBasic.Setup("検索訪問", text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, new PguiOpenWindowCtrl.Callback(this.<SearchFriend>g__decide|455_0), null, false);
		CanvasManager.HdlOpenWindowBasic.ForceOpen();
		do
		{
			yield return null;
		}
		while (!CanvasManager.HdlOpenWindowBasic.FinishedClose() || !this.winSocial.win.FinishedOpen());
		yield break;
	}

	private IEnumerator CopyMyid()
	{
		GUIUtility.systemCopyBuffer = DataManager.DmUserInfo.friendId.ToString();
		this.winSocial.win.ForceClose();
		CanvasManager.HdlOpenWindowBasic.Setup("コピーしました", "そのままペーストしてください", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, new PguiOpenWindowCtrl.Callback(this.<CopyMyid>g__decide|456_0), null, false);
		CanvasManager.HdlOpenWindowBasic.ForceOpen();
		do
		{
			yield return null;
		}
		while (!CanvasManager.HdlOpenWindowBasic.FinishedClose() || !this.winSocial.win.FinishedOpen());
		yield break;
	}

	private IEnumerator MusicOpen()
	{
		this.winPanel.SetActive(true);
		this.winMusic.win.ForceOpen();
		do
		{
			yield return null;
		}
		while (!this.winMusic.win.FinishedOpen());
		yield break;
	}

	private void OnClickMusic(PguiButtonCtrl button)
	{
		if (!this.IsTop())
		{
			return;
		}
		if (!this.winPanel.activeSelf)
		{
			return;
		}
		if (this.ienum.Count > 0)
		{
			return;
		}
		if (!this.winMusic.win.FinishedOpen())
		{
			return;
		}
		if (button == this.winMusic.btnChk)
		{
			this.winMusic.imgChk.SetActive(!this.winMusic.imgChk.activeSelf);
		}
	}

	private void SetupBgmLineup(int index, GameObject go)
	{
		this.UpdateBgmLineup(index, go);
	}

	private void UpdateBgmLineup(int index, GameObject go)
	{
		List<Transform> list = new List<Transform>();
		foreach (object obj in go.transform)
		{
			Transform transform = (Transform)obj;
			list.Add(transform);
		}
		foreach (Transform transform2 in list)
		{
			transform2.SetParent(this.hidePanel, false);
		}
		int num = index * 3;
		int num2 = num + 3;
		int i = num;
		Predicate<Transform> <>9__0;
		while (i < num2 && i < this.bgmList.Count)
		{
			List<Transform> list2 = this.bgmLineup;
			Predicate<Transform> predicate;
			if ((predicate = <>9__0) == null)
			{
				predicate = (<>9__0 = (Transform itm) => itm.name == this.bgmList[i].ToString());
			}
			Transform transform3 = list2.Find(predicate);
			if (transform3 != null)
			{
				transform3.SetParent(go.transform, false);
			}
			int j = i;
			i = j + 1;
		}
	}

	private void OnClickBgmLineup(GameObject go)
	{
		int id = int.Parse(go.name);
		if (id < 0)
		{
			SceneTreeHouse.FurnitureData furnitureData = this.allFurnitureDataList.Find((SceneTreeHouse.FurnitureData itm) => itm.dat.GetId() == -id);
			if (furnitureData == null)
			{
				return;
			}
			SoundManager.PlayBGM(Path.GetFileName(furnitureData.dat.bgmFilepath), 500, 500, -1);
			this.testBgm = 10f;
			using (List<Transform>.Enumerator enumerator = this.bgmLineup.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Transform transform = enumerator.Current;
					transform.Find("Base/TestPlaying").gameObject.SetActive(transform.name == id.ToString());
				}
				return;
			}
		}
		this.playBgm = this.ChangeBgm(id);
	}

	private int ChangeBgm(int id)
	{
		this.testBgm = -1f;
		SceneTreeHouse.FurnitureData furnitureData = this.allFurnitureDataList.Find((SceneTreeHouse.FurnitureData itm) => itm.dat.GetId() == id);
		int num = ((furnitureData == null) ? 0 : furnitureData.dat.GetId());
		string text = ((furnitureData == null) ? "" : furnitureData.dat.bgmFilepath);
		text = (string.IsNullOrEmpty(text) ? SceneTreeHouse.TREE_HOUSE_BGM : Path.GetFileName(text));
		SoundManager.PlayBGM(text, 500, 500, 0);
		foreach (Transform transform in this.bgmLineup)
		{
			transform.Find("Base/Playing").gameObject.SetActive(transform.name == num.ToString());
			transform.Find("Base/TestPlaying").gameObject.SetActive(false);
		}
		return num;
	}

	private IEnumerator ChargeOpen()
	{
		if (DataManager.DmHome.GetHomeCheckResult() == null || DataManager.DmHome.GetHomeCheckResult().IsTreeHouseBatteryCharge())
		{
			DataManager.DmTreeHouse.RequestActionUpdateMachineData();
			do
			{
				yield return null;
			}
			while (DataManager.IsServerRequesting());
			IEnumerator ie = this.GetStamp(false);
			while (ie.MoveNext())
			{
				yield return null;
			}
			ie = null;
		}
		this.winPanel.SetActive(true);
		this.winCharge.win.SetupButtonOnly(new PguiOpenWindowCtrl.Callback(this.DecideCharge));
		this.winCharge.win.ForceOpen();
		this.winCharge.batteryGet.Find("GetItem/Icon_Item").GetComponent<IconItemCtrl>().Setup(this.batteryInfo.staticData);
		this.winCharge.batteryGet.Find("GetItem/Txt_Window/Num_Txt").GetComponent<PguiTextCtrl>().text = "×" + this.batteryData.getItemNum.ToString();
		this.batteryTim = 0;
		this.chargeTim = 0;
		foreach (Transform transform in this.winCharge.fuel)
		{
			if (transform.gameObject.activeSelf)
			{
				ItemData itm = DataManager.DmItem.GetUserItemData(int.Parse(transform.name));
				transform.Find("ItemIconSet/Num_Own").GetComponent<PguiTextCtrl>().text = itm.num.ToString();
				this.DispChargeNum(transform.Find("ItemIconSet"), this.chargeFuel.Find((UseItem cf) => cf.use_item_id == itm.id).use_item_num = 0);
				transform.Find("ItemIconSet/Icon_Item").GetComponent<IconItemCtrl>().SetActEnable(itm.num > 0);
			}
		}
		do
		{
			yield return null;
		}
		while (!this.winCharge.win.FinishedOpen());
		yield break;
	}

	private bool DecideCharge(int idx)
	{
		if (idx > 0)
		{
			this.ienum.Add(this.UseFuel());
		}
		return true;
	}

	private IEnumerator UseFuel()
	{
		if (this.chargeTim > this.batteryTim)
		{
			SceneTreeHouse.<>c__DisplayClass465_0 CS$<>8__locals1 = new SceneTreeHouse.<>c__DisplayClass465_0();
			CS$<>8__locals1.d = 0;
			string text = "獲得までの時間が最大値を超えて回復します\nよろしいですか？\n（超過した回復分は次回に持ち越されます）";
			CanvasManager.HdlOpenWindowBasic.Setup("確認", text, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.NO_YES), true, new PguiOpenWindowCtrl.Callback(CS$<>8__locals1.<UseFuel>g__decide|1), null, false);
			CanvasManager.HdlOpenWindowBasic.ForceOpen();
			do
			{
				yield return null;
			}
			while (!this.winCharge.win.FinishedClose() || !CanvasManager.HdlOpenWindowBasic.FinishedOpen());
			while (CS$<>8__locals1.d == 0)
			{
				yield return null;
			}
			if (CS$<>8__locals1.d < 0)
			{
				this.winPanel.SetActive(true);
				this.winCharge.win.ForceOpen();
				do
				{
					yield return null;
				}
				while (!this.winCharge.win.FinishedOpen() || !CanvasManager.HdlOpenWindowBasic.FinishedClose());
				yield break;
			}
			CS$<>8__locals1 = null;
		}
		List<UseItem> list = new List<UseItem>(this.chargeFuel.FindAll((UseItem itm) => itm.use_item_num > 0));
		if (list.Count <= 0)
		{
			yield break;
		}
		DataManager.DmTreeHouse.RequestActionUseFuel(list);
		do
		{
			yield return null;
		}
		while (DataManager.IsServerRequesting());
		this.ienum.Add(this.GetStamp(false));
		yield break;
	}

	private void OnClickFuel(Transform fuel)
	{
		if (!this.IsTop())
		{
			return;
		}
		if (!this.winPanel.activeSelf)
		{
			return;
		}
		if (this.ienum.Count > 0)
		{
			return;
		}
		if (!this.winCharge.win.FinishedOpen())
		{
			return;
		}
		ItemData itm = DataManager.DmItem.GetUserItemData(int.Parse(fuel.name));
		this.chargeItm = this.chargeFuel.Find((UseItem cf) => cf.use_item_id == itm.id);
		if (this.chargeItm == null)
		{
			return;
		}
		UseItem useItem = this.chargeItm;
		int num = useItem.use_item_num + 1;
		useItem.use_item_num = num;
		if (num > itm.num)
		{
			this.chargeItm.use_item_num = itm.num;
		}
		this.DispChargeNum(fuel.Find("ItemIconSet"), this.chargeItm.use_item_num);
	}

	private void OnLongClickFuel(Transform fuel)
	{
		if (!this.IsTop())
		{
			return;
		}
		if (!this.winPanel.activeSelf)
		{
			return;
		}
		if (this.ienum.Count > 0)
		{
			return;
		}
		if (!this.winCharge.win.FinishedOpen())
		{
			return;
		}
		this.ienum.Add(this.FuelOpen(fuel));
	}

	private IEnumerator FuelOpen(Transform fuel)
	{
		SceneTreeHouse.<>c__DisplayClass468_0 CS$<>8__locals1 = new SceneTreeHouse.<>c__DisplayClass468_0();
		CS$<>8__locals1.itm = DataManager.DmItem.GetUserItemData(int.Parse(fuel.name));
		if (CS$<>8__locals1.itm.num <= 0)
		{
			yield break;
		}
		this.winChargeUse.slider.maxValue = (float)CS$<>8__locals1.itm.num;
		this.chargeItm = this.chargeFuel.Find((UseItem cf) => cf.use_item_id == CS$<>8__locals1.itm.id);
		if (this.chargeItm == null)
		{
			yield break;
		}
		int num = this.chargeItm.use_item_num;
		this.winChargeUse.slider.value = (float)num;
		this.winCharge.win.ForceClose();
		CS$<>8__locals1.d = 0;
		this.winPanel.SetActive(true);
		this.winChargeUse.win.SetupKemoBoardResetCheck(new PguiOpenWindowCtrl.Callback(CS$<>8__locals1.<FuelOpen>g__decide|1));
		this.winChargeUse.win.ForceOpen();
		this.winChargeUse.fuelInfo.Find("Icon_Item").GetComponent<IconItemCtrl>().Setup(CS$<>8__locals1.itm.staticData);
		this.winChargeUse.fuelInfo.Find("Num_Own").GetComponent<PguiTextCtrl>().text = CS$<>8__locals1.itm.num.ToString();
		this.DispChargeNum(this.winChargeUse.fuelInfo, this.chargeItm.use_item_num);
		this.winChargeUse.batteryGet.Find("GetItem/Icon_Item").GetComponent<IconItemCtrl>().Setup(this.batteryInfo.staticData);
		this.winChargeUse.batteryGet.Find("GetItem/Txt_Window/Num_Txt").GetComponent<PguiTextCtrl>().text = "×" + this.batteryData.getItemNum.ToString();
		do
		{
			yield return null;
		}
		while (!this.winChargeUse.win.FinishedOpen() || !this.winCharge.win.FinishedClose());
		while (CS$<>8__locals1.d == 0)
		{
			this.chargeItm.use_item_num = (int)this.winChargeUse.slider.value;
			this.DispChargeNum(this.winChargeUse.fuelInfo, this.chargeItm.use_item_num);
			yield return null;
		}
		this.winPanel.SetActive(true);
		this.winCharge.win.ForceOpen();
		if (CS$<>8__locals1.d <= 0)
		{
			this.chargeItm.use_item_num = num;
		}
		this.DispChargeNum(fuel.Find("ItemIconSet"), this.chargeItm.use_item_num);
		do
		{
			yield return null;
		}
		while (!this.winChargeUse.win.FinishedClose() || !this.winCharge.win.FinishedOpen() || !CanvasManager.HdlOpenWindowBasic.FinishedClose());
		yield break;
	}

	private void OnClickCharge(PguiButtonCtrl button)
	{
		if (!this.IsTop())
		{
			return;
		}
		if (!this.winPanel.activeSelf)
		{
			return;
		}
		if (!this.winChargeUse.win.FinishedOpen())
		{
			return;
		}
		if (this.chargeItm == null)
		{
			return;
		}
		if (button == this.winChargeUse.btnPlus)
		{
			int num = (int)this.winChargeUse.slider.maxValue;
			UseItem useItem = this.chargeItm;
			int num2 = useItem.use_item_num + 1;
			useItem.use_item_num = num2;
			if (num2 > num)
			{
				this.chargeItm.use_item_num = num;
			}
		}
		else if (button == this.winChargeUse.btnMinus)
		{
			UseItem useItem2 = this.chargeItm;
			int num2 = useItem2.use_item_num - 1;
			useItem2.use_item_num = num2;
			if (num2 < 0)
			{
				this.chargeItm.use_item_num = 0;
			}
		}
		this.DispChargeNum(this.winChargeUse.fuelInfo, this.chargeItm.use_item_num);
		this.winChargeUse.slider.value = (float)this.chargeItm.use_item_num;
	}

	private void DispChargeNum(Transform tmp, int num)
	{
		tmp.Find("Count").gameObject.SetActive(num > 0);
		tmp.Find("Count/Num_Count").GetComponent<PguiTextCtrl>().text = num.ToString();
	}

	private IEnumerator ChargeGet()
	{
		ItemData userItemData = DataManager.DmItem.GetUserItemData(this.batteryData.getItemId);
		if (this.batteryInfo.num < userItemData.num)
		{
			int num = userItemData.num - this.batteryInfo.num;
			this.batteryInfo = userItemData;
			this.winPanel.SetActive(true);
			List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>> list = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>
			{
				new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, ""),
				new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, "")
			};
			this.winChargeGet.win.Setup(null, null, list, true, new PguiOpenWindowCtrl.Callback(SceneTreeHouse.<ChargeGet>g__decide|471_0), null, false);
			this.winChargeGet.batteryGet.Find("Icon_Item").GetComponent<IconItemCtrl>().Setup(this.batteryInfo.staticData);
			this.winChargeGet.batteryGet.Find("Txt_Window/Num_Txt").GetComponent<PguiTextCtrl>().text = "×" + num.ToString();
			this.winChargeGet.win.ForceOpen();
			do
			{
				yield return null;
			}
			while (!this.winChargeGet.win.FinishedOpen());
			do
			{
				yield return null;
			}
			while (!this.winChargeGet.win.FinishedClose());
		}
		yield break;
	}

	private IEnumerator StartVR()
	{
		SceneTreeHouse.<>c__DisplayClass472_0 CS$<>8__locals1 = new SceneTreeHouse.<>c__DisplayClass472_0();
		this.winPanel.SetActive(true);
		CS$<>8__locals1.vr = false;
		this.winVR.win.SetupButtonOnly(new PguiOpenWindowCtrl.Callback(CS$<>8__locals1.<StartVR>g__decide|0));
		this.winVR.win.ForceOpen();
		do
		{
			yield return null;
		}
		while (!this.winVR.win.FinishedOpen());
		do
		{
			yield return null;
		}
		while (!this.winVR.win.FinishedClose());
		if (CS$<>8__locals1.vr)
		{
			this.camera.gameObject.SetActive(false);
			this.camR.gameObject.SetActive(true);
			this.camL.gameObject.SetActive(true);
			if (this.guiOther.baseObj.activeSelf)
			{
				this.OtherCamera2Hide();
			}
			else
			{
				this.Camera2Hide();
			}
		}
		yield break;
	}

	private IEnumerator StartOther(int id)
	{
		this.winSocial.win.ForceClose();
		DataManager.DmTreeHouse.RequestActionOtherVisit(id, this.socialTabTyp);
		do
		{
			yield return null;
		}
		while (DataManager.IsServerRequesting());
		if (DataManager.DmTreeHouse.SocialVisitUserData.friendId != id)
		{
			string text = "";
			if (this.socialTabTyp == TreeHouseSocialTabType.FOLLOW)
			{
				text = "フォロー訪問";
			}
			else if (this.socialTabTyp == TreeHouseSocialTabType.PASSING)
			{
				text = "きまぐれ訪問";
			}
			else if (this.socialTabTyp == TreeHouseSocialTabType.SEARCH)
			{
				text = "検索訪問";
			}
			else if (this.socialTabTyp == TreeHouseSocialTabType.RANKING)
			{
				text = "スタンプ順";
			}
			else if (this.socialTabTyp == TreeHouseSocialTabType.STAMP_HISTORY)
			{
				text = "スタンプ履歴";
			}
			CanvasManager.HdlOpenWindowBasic.Setup(text, "該当するプレイヤーが見つかりませんでした", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, new PguiOpenWindowCtrl.Callback(this.<StartOther>g__decide|473_2), null, false);
			CanvasManager.HdlOpenWindowBasic.ForceOpen();
			do
			{
				yield return null;
			}
			while (!CanvasManager.HdlOpenWindowBasic.FinishedClose() || !this.winSocial.win.FinishedOpen());
			yield break;
		}
		this.visitTabTyp = this.socialTabTyp;
		this.camOtherNo = -1;
		if (this.IsTop())
		{
			this.Top2Hide();
		}
		if (this.IsOther())
		{
			this.Other2Hide();
		}
		else if (this.IsOtherInfo())
		{
			this.Info2Hide();
		}
		else
		{
			this.guiOther.baseObj.SetActive(true);
			this.guiOther.anime.ExPauseAnimationLastFrame("START02");
		}
		while (this.stageLoad != null)
		{
			yield return null;
		}
		this.camBCG.enabled = true;
		do
		{
			yield return null;
		}
		while ((this.camBCG.brightness -= TimeManager.DeltaTime * 200f) > -100f);
		this.camBCG.brightness = -100f;
		if (this.furnitureMapSave != null)
		{
			List<TreeHouseFurnitureMapping> list = this.CheckFurnitureMap(this.furnitureMapSave);
			if (list != null)
			{
				DataManager.DmTreeHouse.RequestActionPutFurniture(list);
				do
				{
					yield return null;
				}
				while (DataManager.IsServerRequesting());
			}
			this.furnitureMapSave = null;
		}
		foreach (SceneTreeHouse.FurnitureCtrl furnitureCtrl in this.furnitureList)
		{
			furnitureCtrl.no = -1;
		}
		foreach (SceneTreeHouse.CharaCtrl charaCtrl in this.charaList)
		{
			charaCtrl.no = -1;
		}
		foreach (SceneTreeHouse.FurnitureCtrl furnitureCtrl2 in this.otherFurniture)
		{
			furnitureCtrl2.no = -1;
		}
		foreach (SceneTreeHouse.CharaCtrl charaCtrl2 in this.otherChara)
		{
			charaCtrl2.no = -1;
		}
		do
		{
			yield return null;
		}
		while (this.furnitureList.Count > 0 || this.charaList.Count > 0 || this.otherFurniture.Count > 0 || this.otherChara.Count > 0 || !this.winSocial.win.FinishedClose());
		this.camOtherNo = 0;
		this.guiOther.btnFollow.SetActEnable(!DataManager.DmTreeHouse.SocialVisitUserData.isFinishFollow, false, false);
		this.guiOther.houseName.text = DataManager.DmTreeHouse.SocialVisitUserData.roomName;
		this.guiOther.name.text = DataManager.DmTreeHouse.SocialVisitUserData.userName;
		this.guiOther.comment.text = DataManager.DmTreeHouse.SocialVisitUserData.roomComment;
		this.guiOther.achievement.Setup(DataManager.DmTreeHouse.SocialVisitUserData.achievementId, true, false);
		if (this.socialUserList != null)
		{
			TreeHouseSocialUser treeHouseSocialUser;
			do
			{
				int num = this.socialUserIdx + 1;
				this.socialUserIdx = num;
				if (num >= this.socialUserList.Count)
				{
					break;
				}
				treeHouseSocialUser = this.socialUserList[this.socialUserIdx];
			}
			while (this.socialVisitList.Contains(treeHouseSocialUser.friendId) || !treeHouseSocialUser.isVisit || treeHouseSocialUser.isFinishSendStamp);
		}
		this.socialVisitUserIdx = this.socialUserIdx;
		if (this.socialUserList != null && this.socialUserList.Count != 0)
		{
			this.socialVisitUserList = this.socialUserList;
		}
		this.guiOther.btnNext.gameObject.SetActive(this.socialUserList != null);
		this.guiOther.btnNext.SetActEnable(this.socialUserList != null && this.socialUserIdx < this.socialUserList.Count, false, false);
		foreach (Transform transform in this.guiOther.stamp)
		{
			transform.Find("Txt_Send").gameObject.SetActive(DataManager.DmTreeHouse.SocialVisitUserData.isFinishSendStamp);
		}
		this.ChangeBgm(this.SetFurniture(true));
		this.SetChara(true);
		float tim = 0f;
		for (;;)
		{
			if (!this.guiData.loading.gameObject.activeSelf && (tim += TimeManager.DeltaTime) > 2f)
			{
				this.guiData.loading.gameObject.SetActive(true);
				this.guiData.loading.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
			}
			yield return null;
			if (this.otherFurniture.Find((SceneTreeHouse.FurnitureCtrl itm) => itm.mdl == null) == null)
			{
				if (this.otherChara.Find((SceneTreeHouse.CharaCtrl itm) => itm.hdl != null && !itm.hdl.IsFinishInitialize()) == null)
				{
					break;
				}
			}
		}
		this.guiData.loading.gameObject.SetActive(false);
		this.ChangeStageLight();
		this.Hide2Other();
		do
		{
			yield return null;
		}
		while ((this.camBCG.brightness += TimeManager.DeltaTime * 200f) < 0f);
		this.camBCG.brightness = 0f;
		this.camBCG.enabled = false;
		yield break;
	}

	private void OnClickEndVisit()
	{
		this.ienum.Add(this.EndOther());
	}

	private IEnumerator EndOther()
	{
		if (this.IsOther())
		{
			this.Other2Hide();
		}
		else if (this.IsOtherInfo())
		{
			this.Info2Hide();
		}
		while (this.stageLoad != null)
		{
			yield return null;
		}
		this.camBCG.enabled = true;
		do
		{
			yield return null;
		}
		while ((this.camBCG.brightness -= TimeManager.DeltaTime * 200f) > -100f);
		this.camBCG.brightness = -100f;
		this.guiOther.baseObj.SetActive(false);
		foreach (SceneTreeHouse.FurnitureCtrl furnitureCtrl in this.otherFurniture)
		{
			furnitureCtrl.no = -1;
		}
		foreach (SceneTreeHouse.CharaCtrl charaCtrl in this.otherChara)
		{
			charaCtrl.no = -1;
		}
		do
		{
			yield return null;
		}
		while (this.otherFurniture.Count > 0 || this.otherChara.Count > 0);
		this.SetFurniture(false);
		foreach (SceneTreeHouse.FurnitureCtrl furnitureCtrl2 in this.furnitureList.FindAll((SceneTreeHouse.FurnitureCtrl x) => x.machinePopup != null))
		{
			this.UpdateMachineDisp(furnitureCtrl2, true);
		}
		this.ChangeBgm(this.playBgm);
		this.SetChara(false);
		float tim = 0f;
		for (;;)
		{
			if (!this.guiData.loading.gameObject.activeSelf && (tim += TimeManager.DeltaTime) > 2f)
			{
				this.guiData.loading.gameObject.SetActive(true);
				this.guiData.loading.PlayAnimation(PguiAECtrl.AmimeType.LOOP, null);
			}
			yield return null;
			if (this.furnitureList.Find((SceneTreeHouse.FurnitureCtrl itm) => itm.mdl == null) == null)
			{
				if (this.charaList.Find((SceneTreeHouse.CharaCtrl itm) => itm.hdl != null && !itm.hdl.IsFinishInitialize()) == null)
				{
					break;
				}
			}
		}
		this.guiData.loading.gameObject.SetActive(false);
		this.ChangeStageLight();
		this.Hide2Top();
		do
		{
			yield return null;
		}
		while ((this.camBCG.brightness += TimeManager.DeltaTime * 200f) < 0f);
		this.camBCG.brightness = 0f;
		this.camBCG.enabled = false;
		yield break;
	}

	private IEnumerator FollowOther()
	{
		IEnumerator ie = DataManagerHelper.RequestFollowApply(DataManager.DmTreeHouse.SocialVisitUserData.friendId);
		while (ie.MoveNext())
		{
			yield return null;
		}
		DataManager.DmTreeHouse.RequestActionOtherVisit(DataManager.DmTreeHouse.SocialVisitUserData.friendId, TreeHouseSocialTabType.INVALID);
		do
		{
			yield return null;
		}
		while (DataManager.IsServerRequesting());
		this.guiOther.btnFollow.SetActEnable(!DataManager.DmTreeHouse.SocialVisitUserData.isFinishFollow, false, false);
		DataManager.DmTreeHouse.RequestGetSocialTabData(TreeHouseSocialTabType.FOLLOW);
		do
		{
			yield return null;
		}
		while (DataManager.IsServerRequesting());
		yield break;
	}

	// Note: this type is marked as 'beforefieldinit'.
	static SceneTreeHouse()
	{
		Vector3[,] array = new Vector3[13, 2];
		array[0, 0] = new Vector3(0f, 0f, 0f);
		array[0, 1] = new Vector3(1f, 1f, 0f);
		array[1, 0] = new Vector3(0f, 0.35f, 0f);
		array[1, 1] = new Vector3(1f, 1.4f, 0f);
		array[2, 0] = new Vector3(0f, -0.35f, 0f);
		array[2, 1] = new Vector3(1f, 1.4f, 0f);
		array[3, 0] = new Vector3(0f, 0.64f, 0f);
		array[3, 1] = new Vector3(0.5f, 0.5f, 0f);
		array[4, 0] = new Vector3(0f, -0.64f, 0f);
		array[4, 1] = new Vector3(0.5f, 0.5f, 0f);
		array[5, 0] = new Vector3(-0.23f, 0.74f, 0f);
		array[5, 1] = new Vector3(0.5f, 0.5f, 0f);
		array[6, 0] = new Vector3(0.23f, 0.74f, 0f);
		array[6, 1] = new Vector3(0.5f, 0.5f, 0f);
		array[7, 0] = new Vector3(-0.23f, 0.28f, 0f);
		array[7, 1] = new Vector3(0.5f, 0.5f, 0f);
		array[8, 0] = new Vector3(0.23f, 0.28f, 0f);
		array[8, 1] = new Vector3(0.5f, 0.5f, 0f);
		array[9, 0] = new Vector3(-0.23f, -0.22f, 0f);
		array[9, 1] = new Vector3(0.5f, 0.5f, 0f);
		array[10, 0] = new Vector3(0.23f, -0.22f, 0f);
		array[10, 1] = new Vector3(0.5f, 0.5f, 0f);
		array[11, 0] = new Vector3(-0.23f, -0.7f, 0f);
		array[11, 1] = new Vector3(0.5f, 0.5f, 0f);
		array[12, 0] = new Vector3(0.23f, -0.7f, 0f);
		array[12, 1] = new Vector3(0.5f, 0.5f, 0f);
		SceneTreeHouse.DOOR_GRID_POS_AND_SIZE = array;
		SceneTreeHouse.TREE_HOUSE_BGM = "prd_bgm0083";
		SceneTreeHouse.ScrollDeckNum = 2;
		SceneTreeHouse.stageLayer = LayerMask.NameToLayer("FieldStage");
		SceneTreeHouse.stageAlphaLayer = LayerMask.NameToLayer("FieldStageAlpha");
		SceneTreeHouse.charaLayer = LayerMask.NameToLayer("FieldPlayer");
		SceneTreeHouse.charaAlphaLayer = LayerMask.NameToLayer("FieldPlayerAlpha");
		SceneTreeHouse.charaShadowLayer = LayerMask.NameToLayer("FieldPlayerShadow");
		SceneTreeHouse.ignoreLayer = LayerMask.NameToLayer("Ignore Raycast");
		SceneTreeHouse.bloomLayer = LayerMask.NameToLayer("Bloom");
		SceneTreeHouse.chat_mot = new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.MYR_CHAT_1MOT,
			CharaMotionDefine.ActKey.MYR_CHAT_2MOT,
			CharaMotionDefine.ActKey.MYR_CHAT_3MOT,
			CharaMotionDefine.ActKey.MYR_CHAT_4MOT
		};
		SceneTreeHouse.tea_mot = new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.MYR_TEA_1MOT,
			CharaMotionDefine.ActKey.MYR_TEA_2MOT,
			CharaMotionDefine.ActKey.MYR_TEA_3MOT,
			CharaMotionDefine.ActKey.MYR_TEA_4MOT
		};
		SceneTreeHouse.play_mot = new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.MYR_PLAY_1MOT,
			CharaMotionDefine.ActKey.MYR_PLAY_2MOT,
			CharaMotionDefine.ActKey.MYR_PLAY_3MOT,
			CharaMotionDefine.ActKey.MYR_PLAY_4MOT
		};
		SceneTreeHouse.talk_mot = new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.MYR_TALK_1MOT_NOHAND,
			CharaMotionDefine.ActKey.MYR_TALK_2MOT_NOHAND,
			CharaMotionDefine.ActKey.MYR_TALK_3MOT_NOHAND,
			CharaMotionDefine.ActKey.MYR_TALK_4MOT_NOHAND
		};
		SceneTreeHouse.drw1_mot = new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.MYR_DRW_ST,
			CharaMotionDefine.ActKey.MYR_DRW_LP_A,
			CharaMotionDefine.ActKey.MYR_DRW_LP_B,
			CharaMotionDefine.ActKey.MYR_DRW_EN
		};
		SceneTreeHouse.drw2_mot = new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.MYR_DRW_SEE_ST,
			CharaMotionDefine.ActKey.MYR_DRW_SEE_LP,
			CharaMotionDefine.ActKey.MYR_DRW_SEE_EN
		};
		SceneTreeHouse.drw2n_mot = new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.MYR_DRW_SEE_NOHAND_ST,
			CharaMotionDefine.ActKey.MYR_DRW_SEE_NOHAND_LP,
			CharaMotionDefine.ActKey.MYR_DRW_SEE_NOHAND_EN
		};
		SceneTreeHouse.drwe_mot = CharaMotionDefine.ActKey.MYR_DRW_JOY;
		SceneTreeHouse.cos1_mot = new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.MYR_COS_ST,
			CharaMotionDefine.ActKey.MYR_COS_EN
		};
		SceneTreeHouse.cos2_mot = new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.MYR_COS_SEE_ST,
			CharaMotionDefine.ActKey.MYR_COS_SEE_LP,
			CharaMotionDefine.ActKey.MYR_COS_SEE_EN
		};
		SceneTreeHouse.cos1n_mot = new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.MYR_COS_NOHAND_ST,
			CharaMotionDefine.ActKey.MYR_COS_NOHAND_EN
		};
		SceneTreeHouse.cos2n_mot = new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.MYR_COS_SEE_NOHAND_ST,
			CharaMotionDefine.ActKey.MYR_COS_SEE_NOHAND_LP,
			CharaMotionDefine.ActKey.MYR_COS_SEE_NOHAND_EN
		};
		SceneTreeHouse.gam1_mot = new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.MYR_GAME_1MOT_ST,
			CharaMotionDefine.ActKey.MYR_GAME_1MOT_LP_A,
			CharaMotionDefine.ActKey.MYR_GAME_1MOT_LP_B,
			CharaMotionDefine.ActKey.MYR_GAME_1MOT_END
		};
		SceneTreeHouse.gam2_mot = new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.MYR_GAME_2MOT_ST,
			CharaMotionDefine.ActKey.MYR_GAME_2MOT_LP_A,
			CharaMotionDefine.ActKey.MYR_GAME_2MOT_LP_B,
			CharaMotionDefine.ActKey.MYR_GAME_2MOT_END
		};
		SceneTreeHouse.gamn_mot = new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.MYR_GAME_NOHAND_ST,
			CharaMotionDefine.ActKey.MYR_GAME_NOHAND_LP,
			CharaMotionDefine.ActKey.MYR_GAME_NOHAND_EN
		};
		SceneTreeHouse.game_mot = new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.MYR_GAME_EMO_JOY,
			CharaMotionDefine.ActKey.MYR_GAME_EMO_SAD
		};
		SceneTreeHouse.sleep_mot = new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.MYR_SLEEP_1,
			CharaMotionDefine.ActKey.MYR_SLEEP_2,
			CharaMotionDefine.ActKey.MYR_SLEEP_3,
			CharaMotionDefine.ActKey.MYR_SLEEP_4
		};
		SceneTreeHouse.book_mot = new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.MYR_BOOK_READ_1,
			CharaMotionDefine.ActKey.MYR_BOOK_READ_2
		};
		SceneTreeHouse.kitchen_mot = new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.MYR_KITCHEN_TEA,
			CharaMotionDefine.ActKey.MYR_KITCHEN_FOOD
		};
		SceneTreeHouse.greentea_mot = CharaMotionDefine.ActKey.MYR_KITCHEN_GREENTEA;
		SceneTreeHouse.see_mot = new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.MYR_PLAYSEE_1,
			CharaMotionDefine.ActKey.MYR_PLAYSEE_2,
			CharaMotionDefine.ActKey.MYR_PLAYSEE_3
		};
		SceneTreeHouse.ring_mot = new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.MYR_WANAGE_THROW_1,
			CharaMotionDefine.ActKey.MYR_WANAGE_THROW_2,
			CharaMotionDefine.ActKey.MYR_WANAGE_THROW_3
		};
		SceneTreeHouse.ringe_mot = new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.MYR_GAME_EMO_JOY,
			CharaMotionDefine.ActKey.MYR_GAME_EMO_SAD
		};
		SceneTreeHouse.ball_mot = new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.MYR_BALL_THROW_1,
			CharaMotionDefine.ActKey.MYR_BALL_THROW_2,
			CharaMotionDefine.ActKey.MYR_BALL_THROW_3
		};
		SceneTreeHouse.balle_mot = new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.MYR_GAME_EMO_JOY,
			CharaMotionDefine.ActKey.MYR_GAME_EMO_SAD
		};
		SceneTreeHouse.diary_mot = new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.MYR_NOTE_1,
			CharaMotionDefine.ActKey.MYR_NOTE_2
		};
		SceneTreeHouse.banner_mot = new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.MYR_BANNER_1,
			CharaMotionDefine.ActKey.MYR_BANNER_2
		};
		SceneTreeHouse.kakizome_mot = new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.MYR_KAKIZOME_1,
			CharaMotionDefine.ActKey.MYR_KAKIZOME_2
		};
		SceneTreeHouse.garden_mot = new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.MYR_GARDEN_WATER,
			CharaMotionDefine.ActKey.MYR_GARDEN_SEE
		};
		SceneTreeHouse.gardenn_mot = CharaMotionDefine.ActKey.MYR_GARDEN_SEE_NOHAND;
		SceneTreeHouse.xylo_mot = new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.MYR_XYLOPHONE_PLAY_DUO,
			CharaMotionDefine.ActKey.MYR_XYLOPHONE_SEE_DUO
		};
		SceneTreeHouse.xylo1_mot = CharaMotionDefine.ActKey.MYR_XYLOPHONE_PLAY_SOLO;
		SceneTreeHouse.curry_mot = new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.MYR_CURRY_1,
			CharaMotionDefine.ActKey.MYR_CURRY_2,
			CharaMotionDefine.ActKey.MYR_CURRY_3,
			CharaMotionDefine.ActKey.MYR_CURRY_4
		};
		SceneTreeHouse.tent_mot = new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.MYR_TENT_TALK,
			CharaMotionDefine.ActKey.MYR_TENT_TALK
		};
		SceneTreeHouse.bed_mot = new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.MYR_SLEEP_2,
			CharaMotionDefine.ActKey.MYR_SLEEP_4
		};
		SceneTreeHouse.cook_mot = CharaMotionDefine.ActKey.MYR_COOK;
		SceneTreeHouse.jpntea_mot = new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.MYR_JPNTEA_1,
			CharaMotionDefine.ActKey.MYR_JPNTEA_2
		};
		SceneTreeHouse.sweets_mot = new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.MYR_SWEETS_1,
			CharaMotionDefine.ActKey.MYR_SWEETS_2
		};
		SceneTreeHouse.car_mot = new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.MYR_CAR_1,
			CharaMotionDefine.ActKey.MYR_CAR_SEE
		};
		SceneTreeHouse.sand_mot = new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.MYR_SAND_PLAY_1,
			CharaMotionDefine.ActKey.MYR_SAND_PLAY_2,
			CharaMotionDefine.ActKey.MYR_SAND_PLAY_3,
			CharaMotionDefine.ActKey.MYR_SAND_PLAY_4
		};
		SceneTreeHouse.carn_mot = CharaMotionDefine.ActKey.MYR_CAR_SEE_NOHAND;
		SceneTreeHouse.doll_mot = CharaMotionDefine.ActKey.MYR_PLUSH_SEE;
		SceneTreeHouse.friendsDistance = 0.45f;
		SceneTreeHouse.neighborDistance = 0.2f;
		SceneTreeHouse.emoEffName = new Dictionary<SceneTreeHouse.EmoTyp, string>
		{
			{
				SceneTreeHouse.EmoTyp.SPEAK,
				"Ef_info_picnic_speak"
			},
			{
				SceneTreeHouse.EmoTyp.GAME,
				"Ef_stage_surface_game_emotion"
			},
			{
				SceneTreeHouse.EmoTyp.SLEEP,
				"Ef_info_treehouse_sleep"
			},
			{
				SceneTreeHouse.EmoTyp.HAPPY,
				"Ef_info_home_happy"
			},
			{
				SceneTreeHouse.EmoTyp.STROKE,
				"Ef_info_home_stroke"
			},
			{
				SceneTreeHouse.EmoTyp.SUPPRISE,
				"Ef_info_home_supprise"
			}
		};
		SceneTreeHouse.cupEffName = "Ef_info_cup_a";
		SceneTreeHouse.penEffName = "Ef_info_colored_pencils_a";
		SceneTreeHouse.penEasterEffName = "Ef_info_treehouse_easteregg";
		SceneTreeHouse.potEffName = "Ef_info_treehoue_teapot";
		SceneTreeHouse.ringEffName = "Ef_info_treehouse_throwing";
		SceneTreeHouse.ringNewYearEffName = "Ef_info_treehouse_throwing_newyear";
		SceneTreeHouse.ballEffName = "Ef_info_treehouse_ball";
		SceneTreeHouse.ballBasketEffName = "Ef_info_treehouse_basketball";
		SceneTreeHouse.gdnEffName = "Ef_info_treehoue_wateringcan";
		SceneTreeHouse.gdnChristmasEffName = "Ef_info_treehoue_wateringcan_christmas";
		SceneTreeHouse.gdnTinplateEffName = "Ef_info_treehoue_wateringcan_tinplate";
		SceneTreeHouse.stkEffName = "Ef_info_treehoue_drumstick";
		SceneTreeHouse.spnEffName = "Ef_info_treehoue_spoon";
		SceneTreeHouse.grnEffName = "Ef_info_treehoue_jpteapot";
		SceneTreeHouse.mkr1EffName = "Ef_info_treehoue_marker_a";
		SceneTreeHouse.mkr2EffName = "Ef_info_treehoue_marker_b";
		SceneTreeHouse.mkr1YewYearEffName = "Ef_info_treehoue_marker_newyear_a";
		SceneTreeHouse.mkr2YewYearEffName = "Ef_info_treehoue_marker_newyear_b";
		SceneTreeHouse.onpEffName = "Ef_info_treehoue_onpu";
		SceneTreeHouse.ladleEffName = "Ef_info_treehoue_kitchenware_a";
		SceneTreeHouse.jpncupEffName = "Ef_info_cup_b";
		SceneTreeHouse.TREE_HOUSE_KEY = "treeHouse";
	}

	[CompilerGenerated]
	private bool <SearchFriend>g__decide|455_0(int idx)
	{
		this.winPanel.SetActive(true);
		this.winSocial.win.ForceOpen();
		return true;
	}

	[CompilerGenerated]
	private bool <CopyMyid>g__decide|456_0(int idx)
	{
		this.winPanel.SetActive(true);
		this.winSocial.win.ForceOpen();
		return true;
	}

	[CompilerGenerated]
	internal static bool <ChargeGet>g__decide|471_0(int idx)
	{
		if (idx == 0)
		{
			SceneQuest.Args args = new SceneQuest.Args();
			args.initialMap = true;
			args.category = QuestStaticChapter.Category.ETCETERA;
			args.menuBackSceneName = SceneManager.SceneName.SceneTreeHouse;
			args.menuBackSceneArgs = null;
			Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneQuest, args);
		}
		return true;
	}

	[CompilerGenerated]
	private bool <StartOther>g__decide|473_2(int idx)
	{
		this.winPanel.SetActive(true);
		this.winSocial.win.ForceOpen();
		return true;
	}

	private SceneTreeHouse.GUI guiData;

	private SceneTreeHouse.GUI_OTHER guiOther;

	private GameObject winPanel;

	private SceneTreeHouse.WIN_MYSET winMyset;

	private SceneTreeHouse.WIN_GETSTAMP winGetstamp;

	private SceneTreeHouse.WIN_SOCIAL winSocial;

	private SceneTreeHouse.WIN_OPENCONF winOpenconf;

	private SceneTreeHouse.WIN_FURNITURE winFurniture;

	private SceneTreeHouse.WIN_FILTER winFilter;

	private SceneTreeHouse.WIN_SORT winSort;

	private SceneTreeHouse.WIN_NAME winName;

	private SceneTreeHouse.WIN_COMMENT winComment;

	private SceneTreeHouse.WIN_MUSIC winMusic;

	private SceneTreeHouse.WIN_CHARGE winCharge;

	private SceneTreeHouse.WIN_CHARGE_USE winChargeUse;

	private SceneTreeHouse.WIN_CHARGE_GET winChargeGet;

	private SceneTreeHouse.WIN_MACHINE winMachine;

	private SceneTreeHouse.WIN_MACHINE_GET winMachineGet;

	private SceneTreeHouse.WIN_MACHINE_ALL winMachineAll;

	private SceneTreeHouse.WIN_VR winVR;

	private List<SceneTreeHouse.FurnitureData> allFurnitureDataList;

	private List<SceneTreeHouse.FurnitureData> furnitureDataDisp;

	private static readonly string eff611051StName = "Ef_stage_surface_game_st";

	private static readonly string eff611051LpName = "Ef_stage_surface_game_lp";

	private static readonly string eff611051EnName = "Ef_stage_surface_game_en";

	private static readonly string eff611051EbName = "Ef_stage_surface_game_en_b";

	private static readonly string eff611051AdName = "Ef_stage_surface_game_wait";

	private static readonly string eff611052StName = "Ef_stage_surface_game_solo_st";

	private static readonly string eff611052LpName = "Ef_stage_surface_game_solo_lp";

	private static readonly string eff611052EnName = "Ef_stage_surface_game_solo_en";

	private static readonly string eff611052EbName = "Ef_stage_surface_game_solo_en_b";

	private static readonly string eff611052AdName = "Ef_stage_surface_game_solo_wait";

	private List<int> furnitureNew;

	private GameObject field;

	private Transform hidePanel;

	private FieldCameraScaler camera;

	private CC_BrightnessContrastGamma camBCG;

	private FieldCameraScaler camL;

	private FieldCameraScaler camR;

	private static readonly string LOCATOR_PATH = "Stage/Stage/st_room_captain_locator_a";

	private static readonly string GRID_FLOOR_PATH = "Stage/Stage/st_room_captain_grid_a";

	private static readonly string GRID_WALL_PATH = "Stage/Stage/st_room_captain_grid_wall_a";

	private static readonly string GRID_DEPEND_PATH = "Stage/Stage/st_room_captain_grid_one_a";

	private static readonly string BOARD_PATH = "Stage/Stage/st_room_captain_gridboard_a";

	private static readonly string CURTAIN_BOARD_PATH = "Stage/Stage/st_room_captain_gridboard_curtain_a";

	private static readonly List<string> STAGE_PATH = new List<string> { "SD_room_captain_timezone_a_morning", "SD_room_captain_timezone_b_noon", "SD_room_captain_timezone_c_evening", "SD_room_captain_timezone_d_night" };

	private static readonly string FURNITURE_PATH = "Stage/Captain_furniture/";

	private GameObject stageLocator;

	private GameObject floorGrid;

	private GameObject wallGrid;

	private StagePresetCtrl stageCtrl;

	private Dictionary<string, List<Light>> stageLight;

	private string lightName;

	private IEnumerator stageLoad;

	private SceneHome.StageType stageType;

	private List<SceneTreeHouse.CamDat> camView;

	private List<SceneTreeHouse.CamDat> camOther;

	private List<SceneTreeHouse.CamDat> camEdit;

	private int camViewNo;

	private int camOtherNo;

	private int camEditNo;

	private List<Transform> wallLoc;

	private List<Transform> curtainLoc;

	private List<GameObject> curtainBoard;

	private List<Vector3> wallPos;

	private Transform lightLoc;

	private float removeMachineUI;

	private static readonly float REMOVE_MACHINE_UI_SEC = 0.5f;

	private bool touchView;

	private Vector2 moveView;

	private float pinchView;

	private float wheelView;

	private bool singleTap;

	private bool doubleTap;

	private List<SceneTreeHouse.FurnitureCtrl> gyroLst;

	private Transform gyroCam;

	private static int gyroMode = -1;

	private Quaternion gyroBase;

	private Vector2 gyroAng;

	private float gyroFov;

	private float gyroSpace;

	private static readonly List<KeyValuePair<List<CharaMotionDefine.ActKey>, List<VOICE_TYPE>>> voiceList = new List<KeyValuePair<List<CharaMotionDefine.ActKey>, List<VOICE_TYPE>>>
	{
		new KeyValuePair<List<CharaMotionDefine.ActKey>, List<VOICE_TYPE>>(new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.MYR_SLEEP_1,
			CharaMotionDefine.ActKey.MYR_SLEEP_2,
			CharaMotionDefine.ActKey.MYR_SLEEP_3,
			CharaMotionDefine.ActKey.MYR_SLEEP_4
		}, new List<VOICE_TYPE>
		{
			VOICE_TYPE.SLP01,
			VOICE_TYPE.SLP02,
			VOICE_TYPE.SLP03,
			VOICE_TYPE.SWT03
		}),
		new KeyValuePair<List<CharaMotionDefine.ActKey>, List<VOICE_TYPE>>(new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.MYR_COS_ST,
			CharaMotionDefine.ActKey.MYR_COS_EN,
			CharaMotionDefine.ActKey.MYR_COS_NOHAND_ST,
			CharaMotionDefine.ActKey.MYR_COS_NOHAND_EN
		}, new List<VOICE_TYPE> { VOICE_TYPE.COS01 }),
		new KeyValuePair<List<CharaMotionDefine.ActKey>, List<VOICE_TYPE>>(new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.MYR_GAME_1MOT_LP_A,
			CharaMotionDefine.ActKey.MYR_GAME_1MOT_LP_B,
			CharaMotionDefine.ActKey.MYR_GAME_2MOT_LP_A,
			CharaMotionDefine.ActKey.MYR_GAME_2MOT_LP_B
		}, new List<VOICE_TYPE>
		{
			VOICE_TYPE.ATK01,
			VOICE_TYPE.DMP01
		}),
		new KeyValuePair<List<CharaMotionDefine.ActKey>, List<VOICE_TYPE>>(new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.MYR_DRW_ST,
			CharaMotionDefine.ActKey.MYR_DRW_LP_A,
			CharaMotionDefine.ActKey.MYR_DRW_LP_B,
			CharaMotionDefine.ActKey.MYR_DRW_EN,
			CharaMotionDefine.ActKey.MYR_DRW_SEE_ST,
			CharaMotionDefine.ActKey.MYR_DRW_SEE_LP,
			CharaMotionDefine.ActKey.MYR_DRW_SEE_EN,
			CharaMotionDefine.ActKey.MYR_DRW_SEE_NOHAND_ST,
			CharaMotionDefine.ActKey.MYR_DRW_SEE_NOHAND_LP,
			CharaMotionDefine.ActKey.MYR_DRW_SEE_NOHAND_EN,
			CharaMotionDefine.ActKey.MYR_COS_SEE_ST,
			CharaMotionDefine.ActKey.MYR_COS_SEE_LP,
			CharaMotionDefine.ActKey.MYR_COS_SEE_EN,
			CharaMotionDefine.ActKey.MYR_COS_SEE_NOHAND_ST,
			CharaMotionDefine.ActKey.MYR_COS_SEE_NOHAND_LP,
			CharaMotionDefine.ActKey.MYR_COS_SEE_NOHAND_EN,
			CharaMotionDefine.ActKey.MYR_PLAYSEE_1,
			CharaMotionDefine.ActKey.MYR_PLAYSEE_2,
			CharaMotionDefine.ActKey.MYR_PLAYSEE_3
		}, new List<VOICE_TYPE>
		{
			VOICE_TYPE.CST01,
			VOICE_TYPE.QUE01,
			VOICE_TYPE.JOY01,
			VOICE_TYPE.ACT01
		}),
		new KeyValuePair<List<CharaMotionDefine.ActKey>, List<VOICE_TYPE>>(new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.MYR_STAND_BY,
			CharaMotionDefine.ActKey.MYR_GAME_1MOT_ST,
			CharaMotionDefine.ActKey.MYR_GAME_1MOT_END,
			CharaMotionDefine.ActKey.MYR_GAME_2MOT_ST,
			CharaMotionDefine.ActKey.MYR_GAME_2MOT_END,
			CharaMotionDefine.ActKey.MYR_GAME_EMO_JOY,
			CharaMotionDefine.ActKey.MYR_GAME_EMO_SAD,
			CharaMotionDefine.ActKey.MYR_DRW_JOY,
			CharaMotionDefine.ActKey.MYR_WANAGE_THROW_1,
			CharaMotionDefine.ActKey.MYR_WANAGE_THROW_2,
			CharaMotionDefine.ActKey.MYR_WANAGE_THROW_3,
			CharaMotionDefine.ActKey.MYR_BALL_THROW_1,
			CharaMotionDefine.ActKey.MYR_BALL_THROW_2,
			CharaMotionDefine.ActKey.MYR_BALL_THROW_3
		}, null),
		new KeyValuePair<List<CharaMotionDefine.ActKey>, List<VOICE_TYPE>>(null, new List<VOICE_TYPE>
		{
			VOICE_TYPE.CST01,
			VOICE_TYPE.QUE01,
			VOICE_TYPE.JOY01,
			VOICE_TYPE.ACT01,
			VOICE_TYPE.MOV02,
			VOICE_TYPE.MOV03,
			VOICE_TYPE.MOV04,
			VOICE_TYPE.MOV05,
			VOICE_TYPE.HOM02,
			VOICE_TYPE.HOM03,
			VOICE_TYPE.HOM04,
			VOICE_TYPE.SWT01,
			VOICE_TYPE.SWT02,
			VOICE_TYPE.SWT04
		})
	};

	private static readonly List<KeyValuePair<List<CharaMotionDefine.ActKey>, List<VOICE_TYPE>>> voiceMot = new List<KeyValuePair<List<CharaMotionDefine.ActKey>, List<VOICE_TYPE>>>
	{
		new KeyValuePair<List<CharaMotionDefine.ActKey>, List<VOICE_TYPE>>(new List<CharaMotionDefine.ActKey>
		{
			CharaMotionDefine.ActKey.MYR_GAME_EMO_JOY,
			CharaMotionDefine.ActKey.MYR_DRW_JOY
		}, new List<VOICE_TYPE> { VOICE_TYPE.JOY01 }),
		new KeyValuePair<List<CharaMotionDefine.ActKey>, List<VOICE_TYPE>>(new List<CharaMotionDefine.ActKey> { CharaMotionDefine.ActKey.MYR_GAME_EMO_SAD }, new List<VOICE_TYPE> { VOICE_TYPE.DMP01 })
	};

	private static readonly List<VOICE_TYPE> voiceTrw = new List<VOICE_TYPE> { VOICE_TYPE.ATK01 };

	private int gridRot;

	private Vector3 movePos;

	private Vector3 moveNrm;

	private float moveRot;

	private GameObject moveCurtain;

	private int isMove;

	private string kizunaRatio;

	private int monthlyDay;

	private GameObject furnitureBase;

	private List<SceneTreeHouse.FurnitureCtrl> furnitureList;

	private SceneTreeHouse.FurnitureCtrl furnitureMove;

	private int furniturePlace;

	private int furnitureSel;

	private TreeHouseFurnitureStatic.Category categorySel;

	private HashSet<int> favoriteFuniture;

	private static SceneTreeHouse.FurnitureDispType furnitureDispType = SceneTreeHouse.FurnitureDispType.ALL;

	private static readonly int rugStack = 3;

	private static Dictionary<TreeHouseFurnitureStatic.Category, SceneTreeHouse.FurnitureSortType> furnitureSortType = new Dictionary<TreeHouseFurnitureStatic.Category, SceneTreeHouse.FurnitureSortType>
	{
		{
			TreeHouseFurnitureStatic.Category.LARGE_FURNITURE,
			SceneTreeHouse.FurnitureSortType.NUMBER
		},
		{
			TreeHouseFurnitureStatic.Category.STAND,
			SceneTreeHouse.FurnitureSortType.NUMBER
		},
		{
			TreeHouseFurnitureStatic.Category.GENERAL_MERCHANDISE,
			SceneTreeHouse.FurnitureSortType.NUMBER
		},
		{
			TreeHouseFurnitureStatic.Category.RUG,
			SceneTreeHouse.FurnitureSortType.NUMBER
		},
		{
			TreeHouseFurnitureStatic.Category.WALL_HANGINGS,
			SceneTreeHouse.FurnitureSortType.NUMBER
		},
		{
			TreeHouseFurnitureStatic.Category.CURTAIN,
			SceneTreeHouse.FurnitureSortType.NUMBER
		},
		{
			TreeHouseFurnitureStatic.Category.CEIL_LIGHT,
			SceneTreeHouse.FurnitureSortType.NUMBER
		},
		{
			TreeHouseFurnitureStatic.Category.WALL_PAPER,
			SceneTreeHouse.FurnitureSortType.NUMBER
		},
		{
			TreeHouseFurnitureStatic.Category.FLOOR_PAPER,
			SceneTreeHouse.FurnitureSortType.NUMBER
		}
	};

	private List<SceneTreeHouse.PaperCtrl> paperList;

	private GameObject doorBase;

	private Dictionary<GameObject, Vector3> doorGridMap;

	private static readonly Vector3 DOOR_BATH_POS = new Vector3(0f, 1.07f, -5.27f);

	private static readonly Vector3[,] DOOR_GRID_POS_AND_SIZE;

	private List<int> bgmList;

	private GameObject bgmPanel;

	private List<Transform> bgmLineup;

	private int playBgm;

	private float testBgm;

	private static readonly string TREE_HOUSE_BGM;

	private GameObject charaBase;

	private List<SceneTreeHouse.CharaCtrl> charaList;

	private List<CharaPackData> haveCharaPackList;

	private List<CharaPackData> dispCharaPackList;

	private SortFilterDefine.SortType charaSortType = SortFilterDefine.SortType.LEVEL;

	private List<Transform> treehouseChara;

	private static readonly int ScrollDeckNum;

	private Dictionary<int, int> changeList;

	private int chgChr;

	private List<SceneTreeHouse.ActionCtrl> actionList;

	private bool socialInfo;

	private TreeHouseSocialTabType socialTabTyp;

	private TreeHouseSocialTabType visitTabTyp;

	private List<SceneTreeHouse.FurnitureCtrl> otherFurniture;

	private List<SceneTreeHouse.CharaCtrl> otherChara;

	private List<SceneTreeHouse.ActionCtrl> otherAction;

	private Dictionary<TreeHouseSocialTabType, int> socialIndex = new Dictionary<TreeHouseSocialTabType, int>
	{
		{
			TreeHouseSocialTabType.FOLLOW,
			0
		},
		{
			TreeHouseSocialTabType.PASSING,
			0
		},
		{
			TreeHouseSocialTabType.RANKING,
			0
		},
		{
			TreeHouseSocialTabType.STAMP_HISTORY,
			0
		}
	};

	private List<TreeHouseSocialUser> socialUserList;

	private List<TreeHouseSocialUser> socialVisitUserList;

	private int socialUserIdx;

	private int socialVisitUserIdx;

	private List<int> socialVisitList;

	private List<TreeHouseFurnitureMapping> furnitureMapEdit;

	private List<TreeHouseFurnitureMapping> furnitureMapSave;

	private MstMasterRoomMachineData batteryData;

	private int batteryTim;

	private List<MstMasterRoomFuelData> fuelData;

	private ItemData batteryInfo;

	private List<UseItem> chargeFuel;

	private UseItem chargeItm;

	private int chargeTim;

	private List<TreeHouseReceiveStampLog.Log> logList;

	private List<IEnumerator> ienum;

	private static readonly int stageLayer;

	private static readonly int stageAlphaLayer;

	private static readonly int charaLayer;

	private static readonly int charaAlphaLayer;

	private static readonly int charaShadowLayer;

	private static readonly int ignoreLayer;

	private static readonly int bloomLayer;

	private static readonly List<CharaMotionDefine.ActKey> chat_mot;

	private static readonly List<CharaMotionDefine.ActKey> tea_mot;

	private static readonly List<CharaMotionDefine.ActKey> play_mot;

	private static readonly List<CharaMotionDefine.ActKey> talk_mot;

	private static readonly List<CharaMotionDefine.ActKey> drw1_mot;

	private static readonly List<CharaMotionDefine.ActKey> drw2_mot;

	private static readonly List<CharaMotionDefine.ActKey> drw2n_mot;

	private static readonly CharaMotionDefine.ActKey drwe_mot;

	private static readonly List<CharaMotionDefine.ActKey> cos1_mot;

	private static readonly List<CharaMotionDefine.ActKey> cos2_mot;

	private static readonly List<CharaMotionDefine.ActKey> cos1n_mot;

	private static readonly List<CharaMotionDefine.ActKey> cos2n_mot;

	private static readonly List<CharaMotionDefine.ActKey> gam1_mot;

	private static readonly List<CharaMotionDefine.ActKey> gam2_mot;

	private static readonly List<CharaMotionDefine.ActKey> gamn_mot;

	private static readonly List<CharaMotionDefine.ActKey> game_mot;

	private static readonly List<CharaMotionDefine.ActKey> sleep_mot;

	private static readonly List<CharaMotionDefine.ActKey> book_mot;

	private static readonly List<CharaMotionDefine.ActKey> kitchen_mot;

	private static readonly CharaMotionDefine.ActKey greentea_mot;

	private static readonly List<CharaMotionDefine.ActKey> see_mot;

	private static readonly List<CharaMotionDefine.ActKey> ring_mot;

	private static readonly List<CharaMotionDefine.ActKey> ringe_mot;

	private static readonly List<CharaMotionDefine.ActKey> ball_mot;

	private static readonly List<CharaMotionDefine.ActKey> balle_mot;

	private static readonly List<CharaMotionDefine.ActKey> diary_mot;

	private static readonly List<CharaMotionDefine.ActKey> banner_mot;

	private static readonly List<CharaMotionDefine.ActKey> kakizome_mot;

	private static readonly List<CharaMotionDefine.ActKey> garden_mot;

	private static readonly CharaMotionDefine.ActKey gardenn_mot;

	private static readonly List<CharaMotionDefine.ActKey> xylo_mot;

	private static readonly CharaMotionDefine.ActKey xylo1_mot;

	private static readonly List<CharaMotionDefine.ActKey> curry_mot;

	private static readonly List<CharaMotionDefine.ActKey> tent_mot;

	private static readonly List<CharaMotionDefine.ActKey> bed_mot;

	private static readonly CharaMotionDefine.ActKey cook_mot;

	private static readonly List<CharaMotionDefine.ActKey> jpntea_mot;

	private static readonly List<CharaMotionDefine.ActKey> sweets_mot;

	private static readonly List<CharaMotionDefine.ActKey> car_mot;

	private static readonly List<CharaMotionDefine.ActKey> sand_mot;

	private static readonly CharaMotionDefine.ActKey carn_mot;

	private static readonly CharaMotionDefine.ActKey doll_mot;

	private static readonly float friendsDistance;

	private static readonly float neighborDistance;

	private static readonly Dictionary<SceneTreeHouse.EmoTyp, string> emoEffName;

	private List<TreeHouseSmallFurnitureData> smallFurnitureDataList = new List<TreeHouseSmallFurnitureData>();

	private static readonly string cupEffName;

	private static readonly string penEffName;

	private static readonly string penEasterEffName;

	private static readonly string potEffName;

	private static readonly string ringEffName;

	private static readonly string ringNewYearEffName;

	private static readonly string ballEffName;

	private static readonly string ballBasketEffName;

	private static readonly string gdnEffName;

	private static readonly string gdnChristmasEffName;

	private static readonly string gdnTinplateEffName;

	private static readonly string stkEffName;

	private static readonly string spnEffName;

	private static readonly string grnEffName;

	private static readonly string mkr1EffName;

	private static readonly string mkr2EffName;

	private static readonly string mkr1YewYearEffName;

	private static readonly string mkr2YewYearEffName;

	private static readonly string onpEffName;

	private static readonly string ladleEffName;

	private static readonly string jpncupEffName;

	private static readonly string TREE_HOUSE_KEY;

	public class GUI
	{
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.anime = baseTr.GetComponent<SimpleAnimation>();
			this.topBtn = baseTr.Find("TopBtn").gameObject;
			this.btnEdit = this.topBtn.transform.Find("Btn_Edit").GetComponent<PguiButtonCtrl>();
			this.btnMyset = this.topBtn.transform.Find("Btn_Myset").GetComponent<PguiButtonCtrl>();
			this.btnShop = this.topBtn.transform.Find("Btn_Shop").GetComponent<PguiButtonCtrl>();
			this.btnGacha = this.topBtn.transform.Find("Btn_Gacha").GetComponent<PguiButtonCtrl>();
			this.btnSocial = this.topBtn.transform.Find("Btn_Social").GetComponent<PguiButtonCtrl>();
			this.btnCharge = this.topBtn.transform.Find("Btn_Charge").GetComponent<PguiButtonCtrl>();
			this.btnMachineAll = this.topBtn.transform.Find("Btn_MachineAll").GetComponent<PguiButtonCtrl>();
			this.editBtn = baseTr.Find("TopBtn_Edit").gameObject;
			this.btnPlacemennt = this.editBtn.transform.Find("Btn_Placement").GetComponent<PguiButtonCtrl>();
			this.btnMove = this.editBtn.transform.Find("Btn_Move").GetComponent<PguiButtonCtrl>();
			this.btnReset = this.editBtn.transform.Find("Btn_Reset").GetComponent<PguiButtonCtrl>();
			this.btnShopEdit = this.editBtn.transform.Find("Btn_Shop").GetComponent<PguiButtonCtrl>();
			this.btnGachaEdit = this.editBtn.transform.Find("Btn_Gacha").GetComponent<PguiButtonCtrl>();
			this.btnCancelEdit = baseTr.Find("TopBtn_Cancel/Btn_Cancel").GetComponent<PguiButtonCtrl>();
			this.rightBtn = baseTr.Find("RightBtn").gameObject;
			this.btnHide = this.rightBtn.transform.Find("Btn_Hide").GetComponent<PguiButtonCtrl>();
			this.btnView = this.rightBtn.transform.Find("Btn_View").GetComponent<PguiButtonCtrl>();
			this.btnMusic = this.rightBtn.transform.Find("Btn_Music").GetComponent<PguiButtonCtrl>();
			this.btnCamera = this.rightBtn.transform.Find("Btn_Camera").GetComponent<PguiButtonCtrl>();
			this.rightEditBtn = baseTr.Find("RightBtn_Edit").gameObject;
			this.btnOkEdit = this.rightEditBtn.transform.Find("Btn_OK").GetComponent<PguiButtonCtrl>();
			this.btnViewEdit = this.rightEditBtn.transform.Find("Btn_View").GetComponent<PguiButtonCtrl>();
			this.placeBtn = baseTr.Find("TopBtn_Placement").gameObject;
			this.btnViewPlace = this.placeBtn.transform.Find("Btn_View").GetComponent<PguiButtonCtrl>();
			this.btnRotR = this.placeBtn.transform.Find("Btn_RightRotation").GetComponent<PguiButtonCtrl>();
			this.btnRotL = this.placeBtn.transform.Find("Btn_LeftRotation").GetComponent<PguiButtonCtrl>();
			this.btnRotG = this.placeBtn.transform.Find("Btn_GridRotation").GetComponent<PguiButtonCtrl>();
			this.carpetInfo = this.placeBtn.transform.Find("Carpet_Num").gameObject;
			this.rightPlaceBtn = baseTr.Find("RightBtn_Placement").gameObject;
			this.btnOk = this.rightPlaceBtn.transform.Find("Btn_OK").GetComponent<PguiButtonCtrl>();
			this.btnCancel = this.rightPlaceBtn.transform.Find("Btn_Cancel").GetComponent<PguiButtonCtrl>();
			this.btnTid = this.rightPlaceBtn.transform.Find("Btn_Tid").GetComponent<PguiButtonCtrl>();
			this.furnitureList = baseTr.Find("FurnitureList").gameObject;
			this.furnitureNone = this.furnitureList.transform.Find("Txt_None").GetComponent<PguiTextCtrl>();
			this.btnClose = this.furnitureList.transform.Find("BtnClose").GetComponent<PguiButtonCtrl>();
			this.btnInfo = this.furnitureList.transform.Find("Furniture_BtnList/Img_Line/Btn_Question").GetComponent<PguiButtonCtrl>();
			this.btnFilter = this.furnitureList.transform.Find("Furniture_IconList/Btn_FilterOnOff").GetComponent<PguiButtonCtrl>();
			this.btnSort = this.furnitureList.transform.Find("Furniture_IconList/Btn_Sort").GetComponent<PguiButtonCtrl>();
			this.btnDisp = this.furnitureList.transform.Find("Furniture_IconList/Btn_DisplayChange").GetComponent<PguiButtonCtrl>();
			this.categoryScroll = this.furnitureList.transform.Find("Furniture_BtnList/List/ScrollView").GetComponent<ReuseScroll>();
			this.furnitureScroll = this.furnitureList.transform.Find("Furniture_IconList/List/ScrollView").GetComponent<ReuseScroll>();
			this.charaIconAll = baseTr.Find("CharaIconAll");
			this.charaIcon = new List<Transform>();
			int num = 1;
			for (;;)
			{
				Transform transform = this.charaIconAll.Find("TreeHouse_CharaIcon" + num.ToString("D2"));
				if (transform == null)
				{
					break;
				}
				this.charaIcon.Add(transform);
				num++;
			}
			this.charaSelect = baseTr.Find("CharaSelect");
			this.charaScroll = this.charaSelect.Find("ScrollView").GetComponent<ReuseScroll>();
			this.btnOkChara = this.charaSelect.Find("Btn_OK").GetComponent<PguiButtonCtrl>();
			this.loading = baseTr.Find("AEImage_Loading").GetComponent<PguiAECtrl>();
			this.tipsHand = baseTr.Find("Tips_All/Tips_Hand").gameObject;
			this.tipsMouse = baseTr.Find("Tips_All/Tips_Mouse").gameObject;
			this.btnTipsHand = this.tipsHand.transform.Find("Btn_CheckBox").GetComponent<PguiButtonCtrl>();
			this.btnTipsMouse = this.tipsMouse.transform.Find("Btn_CheckBox").GetComponent<PguiButtonCtrl>();
			this.chkTipsHand = this.btnTipsHand.transform.Find("BaseImage/Img_Check").gameObject;
			this.chkTipsMouse = this.btnTipsMouse.transform.Find("BaseImage/Img_Check").gameObject;
			this.cameraBtn = baseTr.Find("TopBtn_Camera").gameObject;
			this.btnResetCam = this.cameraBtn.transform.Find("Btn_Reset").GetComponent<PguiButtonCtrl>();
			this.btnGyro = this.cameraBtn.transform.Find("Btn_Gyro").GetComponent<PguiButtonCtrl>();
			this.btnVR = this.cameraBtn.transform.Find("Btn_VR").GetComponent<PguiButtonCtrl>();
			this.camendBtn = baseTr.Find("TopBtn_CameraEnd").gameObject;
			this.btnCancelCam = this.camendBtn.transform.Find("Btn_Cancel").GetComponent<PguiButtonCtrl>();
			this.rightCameraBtn = baseTr.Find("RightBtn_Camera").gameObject;
			this.btnReturn = this.rightCameraBtn.transform.Find("Btn_Return").GetComponent<PguiButtonCtrl>();
			this.btnHideCam = this.rightCameraBtn.transform.Find("Btn_Hide").GetComponent<PguiButtonCtrl>();
			this.VRPop = baseTr.Find("VR_Popup").gameObject;
			this.popupMachineAll = baseTr.Find("Popup_Machine_All").gameObject;
			this.popupMachineCmn = this.popupMachineAll.transform.Find("Popup_Machine_Cmn").gameObject;
			this.machineAllBadge = this.btnMachineAll.transform.Find("BaseImage/Cmn_Badge").gameObject;
		}

		public GameObject baseObj;

		public SimpleAnimation anime;

		public GameObject topBtn;

		public PguiButtonCtrl btnEdit;

		public PguiButtonCtrl btnMyset;

		public PguiButtonCtrl btnShop;

		public PguiButtonCtrl btnGacha;

		public PguiButtonCtrl btnSocial;

		public PguiButtonCtrl btnCharge;

		public PguiButtonCtrl btnMachineAll;

		public GameObject editBtn;

		public PguiButtonCtrl btnPlacemennt;

		public PguiButtonCtrl btnMove;

		public PguiButtonCtrl btnReset;

		public PguiButtonCtrl btnShopEdit;

		public PguiButtonCtrl btnGachaEdit;

		public PguiButtonCtrl btnCancelEdit;

		public GameObject rightBtn;

		public PguiButtonCtrl btnHide;

		public PguiButtonCtrl btnView;

		public PguiButtonCtrl btnMusic;

		public PguiButtonCtrl btnCamera;

		public GameObject rightEditBtn;

		public PguiButtonCtrl btnOkEdit;

		public PguiButtonCtrl btnViewEdit;

		public GameObject placeBtn;

		public PguiButtonCtrl btnViewPlace;

		public PguiButtonCtrl btnRotR;

		public PguiButtonCtrl btnRotL;

		public PguiButtonCtrl btnRotG;

		public GameObject carpetInfo;

		public GameObject rightPlaceBtn;

		public PguiButtonCtrl btnOk;

		public PguiButtonCtrl btnCancel;

		public PguiButtonCtrl btnTid;

		public GameObject furnitureList;

		public PguiTextCtrl furnitureNone;

		public PguiButtonCtrl btnClose;

		public PguiButtonCtrl btnInfo;

		public PguiButtonCtrl btnFilter;

		public PguiButtonCtrl btnSort;

		public PguiButtonCtrl btnDisp;

		public ReuseScroll categoryScroll;

		public ReuseScroll furnitureScroll;

		public Transform charaIconAll;

		public List<Transform> charaIcon;

		public Transform charaSelect;

		public ReuseScroll charaScroll;

		public PguiButtonCtrl btnOkChara;

		public PguiAECtrl loading;

		public GameObject tipsHand;

		public GameObject tipsMouse;

		public PguiButtonCtrl btnTipsHand;

		public PguiButtonCtrl btnTipsMouse;

		public GameObject chkTipsHand;

		public GameObject chkTipsMouse;

		public GameObject cameraBtn;

		public PguiButtonCtrl btnResetCam;

		public PguiButtonCtrl btnGyro;

		public PguiButtonCtrl btnVR;

		public GameObject camendBtn;

		public PguiButtonCtrl btnCancelCam;

		public GameObject rightCameraBtn;

		public PguiButtonCtrl btnReturn;

		public PguiButtonCtrl btnHideCam;

		public GameObject VRPop;

		public GameObject popupMachineAll;

		public GameObject popupMachineCmn;

		public GameObject machineAllBadge;
	}

	public class GUI_OTHER
	{
		public GUI_OTHER(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.anime = baseTr.GetComponent<SimpleAnimation>();
			this.stampAll = baseTr.Find("StampAll").gameObject;
			this.stamp = new List<Transform>();
			int num = 1;
			for (;;)
			{
				Transform transform = this.stampAll.transform.Find(num.ToString("D2"));
				if (transform == null)
				{
					break;
				}
				this.stamp.Add(transform);
				num++;
			}
			this.rightBtn = baseTr.Find("RightBtn").gameObject;
			this.btnBack = this.rightBtn.transform.Find("Btn_Back").GetComponent<PguiButtonCtrl>();
			this.btnHide = this.rightBtn.transform.Find("Btn_Hide").GetComponent<PguiButtonCtrl>();
			this.btnView = this.rightBtn.transform.Find("Btn_View").GetComponent<PguiButtonCtrl>();
			this.btnFollow = this.rightBtn.transform.Find("Btn_Follow").GetComponent<PguiButtonCtrl>();
			this.btnCamera = this.rightBtn.transform.Find("Btn_Camera").GetComponent<PguiButtonCtrl>();
			this.houseName = baseTr.Find("OtherUserName/Name").GetComponent<PguiTextCtrl>();
			this.btnSocial = baseTr.Find("LeftBtn/Btn_Social").GetComponent<PguiButtonCtrl>();
			this.btnNext = baseTr.Find("LeftBtn/Btn_Next").GetComponent<PguiButtonCtrl>();
			this.btnInfo = baseTr.Find("OtherUserName").GetComponent<PguiButtonCtrl>();
			this.name = baseTr.Find("otherUser_HouseInfo/BaseImage/Name/Txt").GetComponent<PguiTextCtrl>();
			this.comment = baseTr.Find("otherUser_HouseInfo/BaseImage/Comment/Txt").GetComponent<PguiTextCtrl>();
			this.achievement = baseTr.Find("otherUser_HouseInfo/BaseImage/Achievement/Achievement").GetComponent<AchievementCtrl>();
			this.cameraBtn = baseTr.Find("TopBtn_Camera").gameObject;
			this.btnResetCam = this.cameraBtn.transform.Find("Btn_Reset").GetComponent<PguiButtonCtrl>();
			this.btnGyro = this.cameraBtn.transform.Find("Btn_Gyro").GetComponent<PguiButtonCtrl>();
			this.btnVR = this.cameraBtn.transform.Find("Btn_VR").GetComponent<PguiButtonCtrl>();
			this.camendBtn = baseTr.Find("TopBtn_CameraEnd").gameObject;
			this.btnCancelCam = this.camendBtn.transform.Find("Btn_Cancel").GetComponent<PguiButtonCtrl>();
			this.rightCameraBtn = baseTr.Find("RightBtn_Camera").gameObject;
			this.btnReturn = this.rightCameraBtn.transform.Find("Btn_Return").GetComponent<PguiButtonCtrl>();
			this.btnHideCam = this.rightCameraBtn.transform.Find("Btn_Hide").GetComponent<PguiButtonCtrl>();
		}

		public GameObject baseObj;

		public SimpleAnimation anime;

		public GameObject stampAll;

		public List<Transform> stamp;

		public GameObject rightBtn;

		public PguiButtonCtrl btnBack;

		public PguiButtonCtrl btnHide;

		public PguiButtonCtrl btnView;

		public PguiButtonCtrl btnFollow;

		public PguiButtonCtrl btnCamera;

		public PguiTextCtrl houseName;

		public PguiButtonCtrl btnSocial;

		public PguiButtonCtrl btnNext;

		public PguiButtonCtrl btnInfo;

		public PguiTextCtrl name;

		public PguiTextCtrl comment;

		public GameObject cameraBtn;

		public PguiButtonCtrl btnResetCam;

		public PguiButtonCtrl btnGyro;

		public PguiButtonCtrl btnVR;

		public GameObject camendBtn;

		public PguiButtonCtrl btnCancelCam;

		public GameObject rightCameraBtn;

		public PguiButtonCtrl btnReturn;

		public PguiButtonCtrl btnHideCam;

		public AchievementCtrl achievement;
	}

	public class WIN_MYSET
	{
		public WIN_MYSET(Transform baseTr)
		{
			this.win = baseTr.GetComponent<PguiOpenWindowCtrl>();
			baseTr = this.win.WindowRectTransform;
			this.list = new List<SceneTreeHouse.BAR_MYSET>
			{
				new SceneTreeHouse.BAR_MYSET(baseTr.Find("TreeHouse_MySet_ListBar_Aut"))
			};
			int num = 1;
			for (;;)
			{
				Transform transform = baseTr.Find("TreeHouse_MySet_ListBar_" + num.ToString("D2"));
				if (transform == null)
				{
					break;
				}
				this.list.Add(new SceneTreeHouse.BAR_MYSET(transform));
				num++;
			}
		}

		public PguiOpenWindowCtrl win;

		public List<SceneTreeHouse.BAR_MYSET> list;
	}

	public class BAR_MYSET
	{
		public BAR_MYSET(Transform tmp)
		{
			this.baseObj = tmp.gameObject;
			this.disable = tmp.Find("Disable").gameObject;
			tmp = tmp.Find("BaseImage");
			this.num = tmp.Find("MySet_Num/Num").GetComponent<PguiTextCtrl>();
			this.name = tmp.Find("MySet_Name").GetComponent<PguiTextCtrl>();
			this.date = tmp.Find("Txt_Date").GetComponent<PguiTextCtrl>();
			this.save = tmp.Find("Btn_Save").GetComponent<PguiButtonCtrl>();
			this.load = tmp.Find("Btn_Load").GetComponent<PguiButtonCtrl>();
		}

		public GameObject baseObj;

		public PguiTextCtrl num;

		public PguiTextCtrl name;

		public PguiTextCtrl date;

		public PguiButtonCtrl save;

		public PguiButtonCtrl load;

		public GameObject disable;
	}

	public class WIN_GETSTAMP
	{
		public WIN_GETSTAMP(Transform baseTr)
		{
			this.win = baseTr.GetComponent<PguiOpenWindowCtrl>();
			baseTr = this.win.WindowRectTransform;
			this.msg = baseTr.Find("Txt").GetComponent<PguiTextCtrl>();
			this.msg2 = baseTr.Find("Txt02").GetComponent<PguiTextCtrl>();
			this.scroll = baseTr.Find("List/ScrollView").GetComponent<ReuseScroll>();
		}

		public PguiOpenWindowCtrl win;

		public PguiTextCtrl msg;

		public PguiTextCtrl msg2;

		public ReuseScroll scroll;
	}

	public class WIN_SOCIAL
	{
		public WIN_SOCIAL(Transform baseTr)
		{
			this.win = baseTr.GetComponent<PguiOpenWindowCtrl>();
			baseTr = this.win.WindowRectTransform;
			this.tab = baseTr.Find("Tab_All/TabGroup").GetComponent<PguiTabGroupCtrl>();
			this.header = baseTr.Find("Tab_All/Header").GetComponent<PguiReplaceSpriteCtrl>();
			this.date = baseTr.Find("Tab_All/Txt_Date").GetComponent<PguiTextCtrl>();
			this.last = baseTr.Find("Tab_All/Txt_LastDate/Num_Placement").GetComponent<PguiTextCtrl>();
			this.scrList = baseTr.Find("Tab_All/List/ScrollView").GetComponent<ReuseScroll>();
			this.scrRank = baseTr.Find("Tab_All/Rank/ScrollView").GetComponent<ReuseScroll>();
			this.openConf = baseTr.Find("Tab_All/OpenConf").gameObject;
			this.btnName = this.openConf.transform.Find("Name/Btn_Save").GetComponent<PguiButtonCtrl>();
			this.btnComment = this.openConf.transform.Find("Comment/Btn_Save").GetComponent<PguiButtonCtrl>();
			this.btnOpenconf = this.openConf.transform.Find("OpenConf/Btn_Save").GetComponent<PguiButtonCtrl>();
			this.txtConfMyid = this.openConf.transform.Find("MyID/Base/Txt_ID").GetComponent<PguiTextCtrl>();
			this.btnConfCopy = this.openConf.transform.Find("MyID/Btn_Copy").GetComponent<PguiButtonCtrl>();
			this.stamp = new List<Transform>();
			int num = 1;
			for (;;)
			{
				Transform transform = this.openConf.transform.Find(num.ToString("D2"));
				if (transform == null)
				{
					break;
				}
				this.stamp.Add(transform);
				num++;
			}
			this.noData = baseTr.Find("Txt_NoData").gameObject;
			this.search = baseTr.Find("Tab_All/Search").gameObject;
			this.btnVisit = this.search.transform.Find("Search/Btn_Mission").GetComponent<PguiButtonCtrl>();
			this.btnCopy = this.search.transform.Find("MyID/Btn_Copy").GetComponent<PguiButtonCtrl>();
			this.txtSearch = this.search.transform.Find("Search/InputField/Placeholder").GetComponent<Text>();
			this.txtMyid = this.search.transform.Find("MyID/Base/Txt_ID").GetComponent<PguiTextCtrl>();
			this.inputField = this.search.transform.Find("Search/InputField").GetComponent<InputField>();
		}

		public PguiOpenWindowCtrl win;

		public PguiTabGroupCtrl tab;

		public PguiReplaceSpriteCtrl header;

		public PguiTextCtrl date;

		public PguiTextCtrl last;

		public ReuseScroll scrList;

		public ReuseScroll scrRank;

		public GameObject openConf;

		public PguiButtonCtrl btnName;

		public PguiButtonCtrl btnComment;

		public PguiButtonCtrl btnOpenconf;

		public PguiTextCtrl txtConfMyid;

		public PguiButtonCtrl btnConfCopy;

		public List<Transform> stamp;

		public GameObject noData;

		public GameObject search;

		public PguiButtonCtrl btnVisit;

		public PguiButtonCtrl btnCopy;

		public Text txtSearch;

		public PguiTextCtrl txtMyid;

		public InputField inputField;
	}

	public class WIN_OPENCONF
	{
		public WIN_OPENCONF(Transform baseTr)
		{
			this.win = baseTr.GetComponent<PguiOpenWindowCtrl>();
			baseTr = this.win.WindowRectTransform;
			this.btnOpen = baseTr.Find("Btn_FullOpen").GetComponent<PguiToggleButtonCtrl>();
			this.btnFollower = baseTr.Find("Btn_ForFollower").GetComponent<PguiToggleButtonCtrl>();
			this.btnPrivate = baseTr.Find("Btn_Private").GetComponent<PguiToggleButtonCtrl>();
		}

		public PguiOpenWindowCtrl win;

		public PguiToggleButtonCtrl btnOpen;

		public PguiToggleButtonCtrl btnFollower;

		public PguiToggleButtonCtrl btnPrivate;
	}

	public class WIN_FURNITURE
	{
		public WIN_FURNITURE(Transform baseTr)
		{
			this.win = baseTr.GetComponent<PguiOpenWindowCtrl>();
			baseTr = this.win.WindowRectTransform;
			this.ratio = baseTr.Find("Info01/Txt_Effect").GetComponent<PguiTextCtrl>();
			this.bonus = baseTr.Find("Info01/Txt_NowEffect").GetComponent<PguiTextCtrl>();
			this.next = baseTr.Find("Info01/Txt_NextEffect").GetComponent<PguiTextCtrl>();
			this.ratioSSR = baseTr.Find("Info02/Txt_Effect").GetComponent<PguiTextCtrl>();
			this.bonusSSR = baseTr.Find("Info02/Txt_NowEffect").GetComponent<PguiTextCtrl>();
			this.nextSSR = baseTr.Find("Info02/Txt_NextEffect").GetComponent<PguiTextCtrl>();
		}

		public PguiOpenWindowCtrl win;

		public PguiTextCtrl ratio;

		public PguiTextCtrl bonus;

		public PguiTextCtrl next;

		public PguiTextCtrl ratioSSR;

		public PguiTextCtrl bonusSSR;

		public PguiTextCtrl nextSSR;
	}

	public class WIN_FILTER
	{
		public WIN_FILTER(Transform baseTr)
		{
			this.win = baseTr.GetComponent<PguiOpenWindowCtrl>();
			baseTr = this.win.WindowRectTransform;
		}

		public PguiOpenWindowCtrl win;
	}

	public class WIN_SORT
	{
		public WIN_SORT(Transform baseTr)
		{
			this.win = baseTr.GetComponent<PguiOpenWindowCtrl>();
			baseTr = this.win.WindowRectTransform;
			Transform transform = baseTr.Find("Sort");
			this.btn = new List<PguiToggleButtonCtrl>();
			for (int i = 0; i < Enum.GetValues(typeof(SceneTreeHouse.FurnitureSortType)).Length; i++)
			{
				this.btn.Add(transform.Find("Btn" + (i + 1).ToString("D2")).GetComponent<PguiToggleButtonCtrl>());
			}
		}

		public PguiOpenWindowCtrl win;

		public List<PguiToggleButtonCtrl> btn;
	}

	private enum FurnitureSortType
	{
		NUMBER,
		DESCEND,
		ASCEND,
		NAME
	}

	private enum FurnitureDispType
	{
		ALL,
		FAVORITE,
		BANNER,
		POSTER
	}

	private enum MarkFriendsAction
	{
		INVALID,
		CHARA_ACTION,
		POSTER_BOARD,
		CLOCK,
		BOX,
		CAMERA,
		MACHINE
	}

	public enum CharaReactionId
	{
		INVALID,
		GATHERING_TABLE = 1001,
		TEA_TABLE,
		TSUMIKI,
		EASEL,
		STAND_MIRROR,
		ARCADE,
		KOTATSU = 1009,
		RING,
		READING,
		COUNTER,
		BALL_HIT,
		SLEEP_BAG,
		DIARY,
		PLANTER,
		XYLOPHONE,
		STEW,
		TENT,
		LARGE_POT,
		TEA_CEREMONY,
		PARFAIT,
		BABY_CAR,
		SAND_BOX,
		RING_NEW_YEAR = 1026,
		BASCKETBALL,
		EASEL_SINGLE = 2004,
		MIC,
		ARCADE_SINGLE,
		BED = 2009,
		JAPANESE_TEA = 2012,
		BANNER_PAINT = 2015,
		PLANTER_SINGLE,
		XYLOPHONE_SINGLE,
		PLANTER_XSMAS,
		KAKIZOME,
		PLANTER_TINPLATE,
		EGG_PAINTING = 2028,
		STUFFED_ANIMAL
	}

	public class WIN_NAME
	{
		public WIN_NAME(Transform baseTr)
		{
			this.win = baseTr.GetComponent<PguiOpenWindowCtrl>();
			baseTr = this.win.WindowRectTransform;
			this.inputField = baseTr.Find("InputField").GetComponent<InputField>();
			this.errorMessage = baseTr.Find("Massage_03").gameObject;
		}

		public PguiOpenWindowCtrl win;

		public InputField inputField;

		public GameObject errorMessage;
	}

	public class WIN_COMMENT
	{
		public WIN_COMMENT(Transform baseTr)
		{
			this.win = baseTr.GetComponent<PguiOpenWindowCtrl>();
			baseTr = this.win.WindowRectTransform;
			this.inputField = baseTr.Find("InputField").GetComponent<InputField>();
			this.errorMessage = baseTr.Find("Massage_03").gameObject;
		}

		public PguiOpenWindowCtrl win;

		public InputField inputField;

		public GameObject errorMessage;
	}

	public class WIN_MUSIC
	{
		public WIN_MUSIC(Transform baseTr)
		{
			this.win = baseTr.GetComponent<PguiOpenWindowCtrl>();
			baseTr = this.win.WindowRectTransform;
			this.scroll = baseTr.Find("List/ScrollView").GetComponent<ReuseScroll>();
			this.btnChk = baseTr.Find("Btn_CheckBox").GetComponent<PguiButtonCtrl>();
			this.imgChk = this.btnChk.transform.Find("BaseImage/Img_Check").gameObject;
		}

		public PguiOpenWindowCtrl win;

		public ReuseScroll scroll;

		public PguiButtonCtrl btnChk;

		public GameObject imgChk;
	}

	public class WIN_CHARGE
	{
		public WIN_CHARGE(Transform baseTr)
		{
			this.win = baseTr.GetComponent<PguiOpenWindowCtrl>();
			baseTr = this.win.WindowRectTransform;
			this.batteryGet = baseTr.Find("Info_GetItem");
			this.fuel = new List<Transform>
			{
				baseTr.Find("Fuel01"),
				baseTr.Find("Fuel02"),
				baseTr.Find("Fuel03")
			};
		}

		public PguiOpenWindowCtrl win;

		public Transform batteryGet;

		public List<Transform> fuel;
	}

	public class WIN_CHARGE_USE
	{
		public WIN_CHARGE_USE(Transform baseTr)
		{
			this.win = baseTr.GetComponent<PguiOpenWindowCtrl>();
			baseTr = this.win.WindowRectTransform;
			this.fuelInfo = baseTr.Find("ItemIconSet");
			this.batteryGet = baseTr.Find("Info_GetItem");
			this.slider = baseTr.Find("SliderBar").GetComponent<Slider>();
			this.btnPlus = baseTr.Find("Btn_Plus").GetComponent<PguiButtonCtrl>();
			this.btnMinus = baseTr.Find("Btn_Minus").GetComponent<PguiButtonCtrl>();
		}

		public PguiOpenWindowCtrl win;

		public Transform fuelInfo;

		public Transform batteryGet;

		public Slider slider;

		public PguiButtonCtrl btnPlus;

		public PguiButtonCtrl btnMinus;
	}

	public class WIN_CHARGE_GET
	{
		public WIN_CHARGE_GET(Transform baseTr)
		{
			this.win = baseTr.GetComponent<PguiOpenWindowCtrl>();
			baseTr = this.win.WindowRectTransform;
			this.batteryGet = baseTr.Find("GetItem");
		}

		public PguiOpenWindowCtrl win;

		public Transform batteryGet;

		public PguiTextCtrl getNum;
	}

	private class WIN_MACHINE
	{
		public WIN_MACHINE(Transform baseTr)
		{
			this.win = baseTr.GetComponent<PguiOpenWindowCtrl>();
			baseTr = this.win.WindowRectTransform;
			this.furnitureText = baseTr.Find("Info_Furniture/Txt").gameObject.GetComponent<PguiTextCtrl>();
			this.furnitureIcon = baseTr.Find("Info_Furniture/Icon_Furniture_mini");
			this.itemNameText = baseTr.Find("Info_GetItem/Info_Group/Item_Txt/Txt_Item").gameObject.GetComponent<PguiTextCtrl>();
			this.itemGetNumText = baseTr.Find("Info_GetItem/Info_Group/Item_Txt/Txt_Num").gameObject.GetComponent<PguiTextCtrl>();
			this.getMax = baseTr.Find("Info_GetItem/Info_Group/Item_Txt/AEImage_Max").GetComponent<AEImage>();
			this.itemGetTimeInfo = baseTr.Find("Info_GetItem/Info_Group/Time_Info");
			this.itemGetTxt01 = baseTr.Find("Parts_ItemGet/Txt01").gameObject.GetComponent<PguiTextCtrl>();
			this.numBeforeItemText = baseTr.Find("Parts_ItemGet/Num_BeforeTxt").gameObject.GetComponent<PguiTextCtrl>();
			this.numAfterItemText = baseTr.Find("Parts_ItemGet/Num_AfterTxt").gameObject.GetComponent<PguiTextCtrl>();
		}

		public PguiOpenWindowCtrl win;

		public PguiTextCtrl furnitureText;

		public Transform furnitureIcon;

		public SceneTreeHouse.FurnitureCtrl fc;

		public PguiTextCtrl itemNameText;

		public PguiTextCtrl itemGetNumText;

		public AEImage getMax;

		public Transform itemGetTimeInfo;

		public PguiTextCtrl itemGetTxt01;

		public PguiTextCtrl numBeforeItemText;

		public PguiTextCtrl numAfterItemText;

		public MasterRoomMachineDataModel machineTimeData;

		public MstMasterRoomMachineData mstMachineData;

		public bool isPrepared;
	}

	public class WIN_MACHINE_GET
	{
		public WIN_MACHINE_GET(Transform baseTr)
		{
			this.win = baseTr.GetComponent<PguiOpenWindowCtrl>();
			baseTr = this.win.WindowRectTransform;
			this.oneArea = baseTr.Find("OneArea");
			this.allArea = baseTr.Find("AllArea");
			this.scroll = this.allArea.Find("List/ScrollView").GetComponent<ReuseScroll>();
		}

		public PguiOpenWindowCtrl win;

		public Transform oneArea;

		public Transform allArea;

		public ReuseScroll scroll;
	}

	public class WIN_MACHINE_ALL
	{
		public WIN_MACHINE_ALL(Transform baseTr)
		{
			this.win = baseTr.GetComponent<PguiOpenWindowCtrl>();
			baseTr = this.win.WindowRectTransform;
			this.noMachine = baseTr.Find("NoMachine");
			this.scroll = baseTr.Find("List/ScrollView").GetComponent<ReuseScroll>();
		}

		public PguiOpenWindowCtrl win;

		public Transform noMachine;

		public ReuseScroll scroll;
	}

	public class WIN_VR
	{
		public WIN_VR(Transform baseTr)
		{
			this.win = baseTr.GetComponent<PguiOpenWindowCtrl>();
		}

		public PguiOpenWindowCtrl win;
	}

	private class FurnitureData
	{
		public TreeHouseFurnitureStatic dat;

		public Vector3Int siz;

		public int sortSiz;

		public Vector3 objSiz;

		public float posY;

		public int num;
	}

	private class CamDat
	{
		public Transform bas;

		public Vector2 ang;

		public float fov;
	}

	private class FurnitureCtrl
	{
		public FurnitureCtrl(int n, SceneTreeHouse.FurnitureData fd, Vector3 p, float d, GameObject o, bool isOn)
		{
			this.no = n;
			this.data = fd;
			this.pos = p;
			this.dir = d;
			this.path = null;
			this.obj = o;
			this.mdl = null;
			this.on = null;
			this.off = null;
			this.chr = null;
			this.mesh = null;
			this.board = null;
			this.CalcCorner();
			this.alpha = 0f;
			this.eff = null;
			this.depend = null;
			this.movdep = null;
			this.grid = new Dictionary<GameObject, Vector3>();
			this.pstr = new Dictionary<GameObject, Vector3>();
			this.mainTex = null;
			this.addTex = null;
			this.action = null;
			this.stick = null;
			this.stickMin = null;
			this.stickMax = null;
			this.rug = 0;
			this.light = "";
			this.sw = null;
			this.onoff = isOn;
			this.chgmdl = false;
			this.clockH = new List<Transform>();
			this.clockM = new List<Transform>();
			this.clockS = new List<Transform>();
			this.cam = new List<Transform>();
			this.handle = null;
			this.machinePopup = null;
		}

		public void CalcCorner()
		{
			Transform transform = this.obj.transform;
			if (this.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_PAPER || this.data.dat.category == TreeHouseFurnitureStatic.Category.FLOOR_PAPER || this.data.dat.category == TreeHouseFurnitureStatic.Category.CEIL_DECO)
			{
				this.corner = new List<Vector3>
				{
					Vector3.zero,
					Vector3.zero,
					Vector3.zero,
					Vector3.zero
				};
				this.walloff = Vector3.zero;
				return;
			}
			float num = (float)this.data.siz.x * ((this.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_HANGINGS || this.data.dat.category == TreeHouseFurnitureStatic.Category.CURTAIN) ? 0.125f : 0.25f);
			float num2 = (float)this.data.siz.y * ((this.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_HANGINGS || this.data.dat.category == TreeHouseFurnitureStatic.Category.CURTAIN) ? 0.125f : 0.25f);
			float num3 = (float)this.data.siz.z * 0.25f;
			if (num3 < 0.2f)
			{
				num3 = 0.2f;
			}
			float num4 = this.data.posY;
			this.walloff = transform.TransformVector(0f, 0f, -num3);
			if (this.data.dat.category == TreeHouseFurnitureStatic.Category.WALL_HANGINGS || this.data.dat.category == TreeHouseFurnitureStatic.Category.CURTAIN)
			{
				num3 = 0f;
			}
			else if (this.data.dat.category == TreeHouseFurnitureStatic.Category.CEIL_LIGHT)
			{
				num4 = -this.data.objSiz.y;
			}
			else
			{
				num2 = 0f;
				num4 = 0f;
			}
			this.corner = new List<Vector3>
			{
				transform.TransformPoint(new Vector3(num, num4 + num2, num3)),
				transform.TransformPoint(new Vector3(-num, num4 + num2, num3)),
				transform.TransformPoint(new Vector3(-num, num4 - num2, -num3)),
				transform.TransformPoint(new Vector3(num, num4 - num2, -num3))
			};
		}

		public int no;

		public SceneTreeHouse.FurnitureData data;

		public Vector3 pos;

		public float dir;

		public string path;

		public GameObject obj;

		public GameObject mdl;

		public List<GameObject> on;

		public List<GameObject> off;

		public List<Transform> chr;

		public MeshRenderer[] mesh;

		public float alpha;

		public GameObject board;

		public List<Vector3> corner;

		public Vector3 walloff;

		public EffectData eff;

		public GameObject depend;

		public GameObject movdep;

		public Dictionary<GameObject, Vector3> grid;

		public Dictionary<GameObject, Vector3> pstr;

		public string mainTex;

		public string addTex;

		public List<Transform> action;

		public List<Transform> stick;

		public List<Vector3> stickMin;

		public List<Vector3> stickMax;

		public int rug;

		public string light;

		public Transform sw;

		public bool onoff;

		public bool chgmdl;

		public List<Transform> clockH;

		public List<Transform> clockM;

		public List<Transform> clockS;

		public List<Transform> cam;

		public Transform handle;

		public GameObject machinePopup;
	}

	private class PaperCtrl
	{
		public TreeHouseFurnitureStatic.Category cat;

		public string mdlnam;

		public bool enable;

		public string path;

		public GameObject mdl;

		public MeshRenderer[] mesh;

		public bool chgmdl;
	}

	private class CharaCtrl
	{
		public CharaCtrl(int n, CharaPackData cpd)
		{
			this.no = n;
			this.chara = cpd;
			this.noHand = cpd.equipPlayMotion == CharaClothStatic.PlayMotionType.HandsNotUse;
			this.hdl = null;
			this.cup = null;
			this.stk = null;
			this.stkL = 99999f;
			this.stkR = 99999f;
			this.onp = new List<EffectData>();
			this.eff = null;
			this.ring = new List<SceneTreeHouse.CharaCtrl.Effect>();
			this.ball = new List<SceneTreeHouse.CharaCtrl.Effect>();
			this.tim = 0f;
			this.itvl = 0f;
			this.act = 0;
			this.idx = -1;
			this.spe = false;
			this.tlk = false;
			this.voice = VOICE_TYPE.NONE;
			this.voiceTime = -0.1f;
		}

		public int no;

		public CharaPackData chara;

		public bool noHand;

		public CharaModelHandle hdl;

		public EffectData cup;

		public EffectData stk;

		public float stkL;

		public float stkR;

		public List<EffectData> onp;

		public EffectData eff;

		public List<SceneTreeHouse.CharaCtrl.Effect> ring;

		public List<SceneTreeHouse.CharaCtrl.Effect> ball;

		public float tim;

		public float itvl;

		public int act;

		public int idx;

		public bool spe;

		public bool tlk;

		public VOICE_TYPE voice;

		public float voiceTime;

		public class Effect
		{
			public int idx;

			public Transform bone;

			public EffectData eff;

			public MeshRenderer[] mesh;

			public float alpha;

			public int typ;

			public Vector3 bas;

			public Vector3 tag;

			public float tim;
		}
	}

	private class ActionCtrl
	{
		public int furniture;

		public List<int> chara;

		public int step;

		public int flag;

		public float motSpd;
	}

	private enum EmoTyp
	{
		INVALID,
		SPEAK,
		GAME,
		SLEEP,
		HAPPY,
		STROKE,
		SUPPRISE
	}
}
