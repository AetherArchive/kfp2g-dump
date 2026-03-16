using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.Mst;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;

public class SelAssistantCtrl : MonoBehaviour
{
	private CharaPackData DefaultAssistant
	{
		get
		{
			if (this.defaultAssistant == null)
			{
				int num = ((this.scene == SelAssistantCtrl.Scene.SHOP) ? 21 : 1);
				this.defaultAssistant = CharaPackData.MakeDummy(num);
			}
			return this.defaultAssistant;
		}
	}

	public GameObject mainObj { get; private set; }

	public int CurrentCharaId
	{
		get
		{
			if (this.scene != SelAssistantCtrl.Scene.SHOP)
			{
				return DataManager.DmAssistant.UserData.questAssistantCharaId;
			}
			return DataManager.DmAssistant.UserData.shopAssistantCharaId;
		}
	}

	private List<int> PurchaseList
	{
		get
		{
			DataManagerAssistant.UserAssistantData userData = DataManager.DmAssistant.UserData;
			if (this.scene != SelAssistantCtrl.Scene.SHOP)
			{
				return userData.purchaseListQuest;
			}
			return userData.purchaseListShop;
		}
	}

	public SelAssistantCtrl.Mode currentMode
	{
		get
		{
			return this._currentMode;
		}
		set
		{
			this._preMode = this._currentMode;
			this._currentMode = value;
		}
	}

	public SelAssistantCtrl.Mode preMode
	{
		get
		{
			return this._preMode;
		}
	}

	public bool IsEnableScene
	{
		get
		{
			return this._preMode > SelAssistantCtrl.Mode.INVALID;
		}
		set
		{
		}
	}

	public void Init(SelAssistantCtrl.Scene scene)
	{
		this.scene = scene;
		this._preMode = SelAssistantCtrl.Mode.INVALID;
		this.mainObj = Object.Instantiate<GameObject>((GameObject)AssetManager.GetAssetData("SceneShop/GUI/Prefab/GUI_CharaAll"), base.transform);
		this.grandObj = this.mainObj.transform.parent.parent.gameObject;
		this.btnAssistantEdit = this.grandObj.transform.Find("BtnAssistant_Edit").GetComponent<PguiButtonCtrl>();
		this.guiData = new SelAssistantCtrl.CharaAllGUI(this.mainObj.transform);
		GameObject gameObject = Object.Instantiate<GameObject>((GameObject)AssetManager.GetAssetData("Cmn/GUI/Prefab/GUI_Cmn_Assistant_Resist_Confirm"), Singleton<CanvasManager>.Instance.SystemMiddleArea);
		Transform transform = gameObject.transform.Find("Base/Window");
		this.windowBuyConfirm = new SelAssistantCtrl.WindowBuyConfirm(gameObject.transform, transform);
		gameObject = AssetManager.GetAssetData("Cmn/GUI/Prefab/GUI_Cmn_ShopWindow_Result") as GameObject;
		this.windowBuyEnd = new SelShopCtrl.WindowBuyEnd(Object.Instantiate<Transform>(gameObject.transform, Singleton<CanvasManager>.Instance.SystemMiddleArea).transform);
		this.isRandom = this.CurrentCharaId == 0;
		if (!QuestUtil.IsDispDhole())
		{
			this.currentMode = SelAssistantCtrl.Mode.DISP_DHOLE;
		}
		else
		{
			this.currentMode = SelAssistantCtrl.Mode.TOP;
		}
		this.guiData.ScrollView.InitForce();
		ReuseScroll scrollView = this.guiData.ScrollView;
		scrollView.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollView.onStartItem, new Action<int, GameObject>(this.OnStartItemChara));
		ReuseScroll scrollView2 = this.guiData.ScrollView;
		scrollView2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollView2.onUpdateItem, new Action<int, GameObject>(this.OnUpdateItemChara));
		this.guiData.ScrollView.Setup(10, 0);
		this.guiData.ScrollView.gameObject.SetActive(false);
		this.guiData.ScrollView.gameObject.SetActive(true);
		this.UpdateCharaPackList();
	}

	public void Setup()
	{
		this._preMode = SelAssistantCtrl.Mode.INVALID;
		SortWindowCtrl.RegisterData registerData = new SortWindowCtrl.RegisterData
		{
			register = SortFilterDefine.RegisterType.CHARA_SHOP_ASSISTANT,
			funcGetTargetBaseList = delegate
			{
				List<CharaPackData> list = new List<CharaPackData>();
				if (this.currentChara != null)
				{
					list.Add(this.currentChara);
				}
				return new SortWindowCtrl.SortTarget
				{
					charaList = this.charaPackList,
					disableFilterCharaList = list
				};
			},
			funcDisideTarget = delegate(SortWindowCtrl.SortTarget item)
			{
				this.dispCharaPackList = new List<CharaPackData>(item.charaList);
				this.dispCharaPackList.Insert(0, this.randomButtonCharaData);
				this.sortType = item.sortType;
				this.guiData.ResizeScrollView(this.dispCharaPackList.Count - 1, this.dispCharaPackList.Count / 2 + 1);
			}
		};
		CanvasManager.HdlOpenWindowSortFilter.Register(registerData, false, null);
	}

	private void Update()
	{
		if (this.currentEnumerator != null && !this.currentEnumerator.MoveNext())
		{
			this.currentEnumerator = null;
		}
	}

	public void CharaSetup()
	{
		if (this.renderTextureChara == null)
		{
			this.renderTextureChara = Object.Instantiate<GameObject>((GameObject)Resources.Load("RenderTextureChara/Prefab/RenderTextureCharaCtrl"), this.grandObj.transform).GetComponent<RenderTextureChara>();
		}
		if (QuestUtil.IsDispDhole())
		{
			this.currentMode = SelAssistantCtrl.Mode.TOP;
		}
		if (this.currentMode == SelAssistantCtrl.Mode.DISP_DHOLE)
		{
			this.currentChara = this.DefaultAssistant;
			this.renderTextureChara.SetupEnableTouch(this.currentChara.id, 0, this.scene == SelAssistantCtrl.Scene.SHOP, 0, false);
		}
		else
		{
			int id = ((this.currentChara == null) ? this.CurrentCharaId : this.currentChara.id);
			CharaPackData charaPackData = this.charaPackList.Find((CharaPackData itm) => itm.id == id);
			if (charaPackData == null)
			{
				this.currentChara = this.DefaultAssistant;
			}
			else if (charaPackData != null)
			{
				this.currentChara = charaPackData;
			}
			this.currentCharaCloth = this.currentChara.equipClothImageId;
			this.currentCharaSkirt = this.currentCharaCloth > 0 && this.currentChara.equipLongSkirt;
			this.renderTextureChara.SetupEnableTouch(this.currentChara.id, 0, this.scene == SelAssistantCtrl.Scene.SHOP, this.currentCharaCloth, this.currentCharaSkirt);
		}
		if (this.scene == SelAssistantCtrl.Scene.SHOP)
		{
			this.UpdateTextureSetting();
		}
	}

	public void CharaSetupRandom()
	{
		if (this.renderTextureChara == null)
		{
			this.renderTextureChara = Object.Instantiate<GameObject>((GameObject)Resources.Load("RenderTextureChara/Prefab/RenderTextureCharaCtrl"), this.grandObj.transform).GetComponent<RenderTextureChara>();
		}
		if (this.currentMode == SelAssistantCtrl.Mode.DISP_DHOLE)
		{
			this.currentChara = this.DefaultAssistant;
			this.renderTextureChara.SetupEnableTouch(this.currentChara.id, 0, this.scene == SelAssistantCtrl.Scene.SHOP, 0, false);
		}
		else
		{
			if (this.currentChara != null)
			{
				int id = this.currentChara.id;
			}
			else
			{
				int currentCharaId = this.CurrentCharaId;
			}
			int charaId = this.PickRandomChara();
			CharaPackData charaPackData = this.charaPackList.Find((CharaPackData itm) => itm.id == charaId);
			if (charaPackData == null)
			{
				this.currentChara = this.DefaultAssistant;
			}
			else if (charaPackData != null)
			{
				this.currentChara = charaPackData;
			}
			this.currentCharaCloth = this.currentChara.equipClothImageId;
			this.currentCharaSkirt = this.currentCharaCloth > 0 && this.currentChara.equipLongSkirt;
			this.renderTextureChara.SetupEnableTouch(this.currentChara.id, 0, this.scene == SelAssistantCtrl.Scene.SHOP, this.currentCharaCloth, this.currentCharaSkirt);
		}
		if (this.scene == SelAssistantCtrl.Scene.SHOP)
		{
			this.UpdateTextureSetting();
		}
	}

	public void SetupAssistant()
	{
		this.isChangeClone = false;
		this.UpdateDispCharaPackList(false);
		this.guiData.ScrollView.Refresh();
		if (this.CurrentCharaId == 0)
		{
			this.CharaSetupRandom();
		}
		else
		{
			this.CharaSetup();
		}
		if (this.currentMode != SelAssistantCtrl.Mode.DISP_DHOLE)
		{
			this._preMode = SelAssistantCtrl.Mode.TOP;
		}
		else
		{
			this._preMode = SelAssistantCtrl.Mode.DISP_DHOLE;
		}
		this.guiData.ScrollView.Refresh();
	}

	public bool OnClickMenuReturn(UnityAction onFinish, UnityAction onCancel = null)
	{
		if (this.currentMode != SelAssistantCtrl.Mode.ASSISTANT_EDIT)
		{
			if (onFinish != null)
			{
				onFinish();
			}
			return true;
		}
		if (this.currentChara.id != this.CurrentCharaId && (!this.isRandom || this.CurrentCharaId != 0))
		{
			this.currentMode = SelAssistantCtrl.Mode.OW_DISCARD_CHARA_RETRUEN;
			CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("確認"), PrjUtil.MakeMessage("フレンズが変更されています"), SelAssistantCtrl.questionButtonSet, true, new PguiOpenWindowCtrl.Callback(this.OnSelectOpenWindowButtonCallback), null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			this._onCancel = onCancel;
			this._onFinish = onFinish;
			return true;
		}
		this.ChangeMode(SelAssistantCtrl.Mode.TOP);
		if (onFinish != null)
		{
			onFinish();
		}
		return true;
	}

	public bool OnClickMoveSequenceButton(SceneManager.SceneName sceneName, object sceneArgs)
	{
		this.guiData.tapGuard.SetActive(false);
		this.guiData.CharaAll.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
		if (this.currentMode == SelAssistantCtrl.Mode.ASSISTANT_EDIT && this.currentChara.id != this.CurrentCharaId && (!this.isRandom || this.CurrentCharaId != 0))
		{
			this.OnClickMoveSequenceName = sceneName;
			this.OnClickMoveSequenceArgs = sceneArgs;
			this.currentMode = SelAssistantCtrl.Mode.OW_DISCARD_CHARA_MOVE_SCENE;
			CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("確認"), PrjUtil.MakeMessage("フレンズが変更されています"), SelAssistantCtrl.questionButtonSet, true, new PguiOpenWindowCtrl.Callback(this.OnSelectOpenWindowButtonCallback), null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			return true;
		}
		return false;
	}

	public void ChangeMode(SelAssistantCtrl.Mode nextMode)
	{
		if (this.currentMode != nextMode && this.currentMode == SelAssistantCtrl.Mode.ASSISTANT_EDIT && nextMode == SelAssistantCtrl.Mode.TOP)
		{
			this.ResetCurrentIcon();
			this.currentMode = SelAssistantCtrl.Mode.TOP;
		}
		this.guiData.tapGuard.SetActive(false);
		this.guiData.CharaAll.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
	}

	private void ResetCurrentIcon()
	{
		if (this.currentChara != null)
		{
			SelAssistantCtrl.CharaAllGUI.IconChara iconChara = this.guiData.SearchIconChara(this.currentChara);
			if (iconChara != null)
			{
				iconChara.iconCharaSet.currentFrame.SetActive(false);
			}
			this.SetupSelectDisable();
		}
	}

	private void SelectCharaIcon(CharaPackData newSelectChara, CharaPackData oldSelectChara, bool isRandom = false)
	{
		SelAssistantCtrl.CharaAllGUI.IconChara iconChara = this.guiData.SearchIconChara(newSelectChara);
		CharaPackData charaPackData = (oldSelectChara.IsInvalid() ? this.randomButtonCharaData : oldSelectChara);
		this.isRandom = isRandom;
		SelAssistantCtrl.CharaAllGUI.IconChara iconChara2 = this.guiData.SearchIconChara(charaPackData);
		List<PguiAECtrl> list = new List<PguiAECtrl>();
		if (iconChara.iconCharaSet.disable.activeSelf)
		{
			if (iconChara2 != null)
			{
				iconChara2.iconCharaSet.currentFrame.SetActive(false);
			}
			iconChara.iconCharaSet.currentFrame.SetActive(false);
			this.isChangeClone = false;
			this.ResetCurrentIcon();
		}
		else if (newSelectChara != oldSelectChara || newSelectChara.id == 0)
		{
			if (iconChara2 != null)
			{
				iconChara2.iconCharaSet.currentFrame.SetActive(false);
			}
			iconChara.iconCharaSet.currentFrame.SetActive(true);
			if (!isRandom)
			{
				this.currentChara = newSelectChara;
			}
			if (newSelectChara.id != charaPackData.id)
			{
				this.isChangeClone = true;
			}
		}
		this.SetupSelectDisable();
		if (this.isChangeClone)
		{
			for (int i = 0; i < list.Count; i++)
			{
				list[i].PlayAnimation(PguiAECtrl.AmimeType.START, null);
			}
		}
	}

	private void SetupSelectDisable()
	{
		foreach (SelAssistantCtrl.CharaAllGUI.IconChara iconChara in this.guiData.reserveCharaIcon)
		{
			this.SetupSelectDisable(iconChara);
		}
	}

	private void SetupSelectDisable(SelAssistantCtrl.CharaAllGUI.IconChara ic)
	{
		CharaPackData iconCpd = ic.iconCharaSet.iconCharaCtrl.charaPackData;
		bool flag = iconCpd != null && this.PurchaseList.Exists((int item) => item == iconCpd.id);
		if (iconCpd == null)
		{
			ic.iconCharaSet.selected.gameObject.SetActive(false);
			ic.iconCharaSet.disable.SetActive(false);
			ic.iconCharaSet.iconCharaCtrl.IsEnableMask(!flag);
			return;
		}
		int num = ((iconCpd == null) ? 0 : iconCpd.id);
		ic.iconCharaSet.selected.gameObject.SetActive(num >= 0 && this.CurrentCharaId == num);
		bool flag2 = ic.iconCharaSet.selected.gameObject.activeSelf && false;
		ic.iconCharaSet.disable.SetActive(flag2);
		ic.iconCharaSet.iconCharaCtrl.IsEnableMask(ic.iconCharaSet.selected.gameObject.activeSelf || ic.iconCharaSet.disable.activeSelf || !flag);
	}

	private void OnStartItemChara(int index, GameObject go)
	{
		if (this.guiData.reserveCharaIcon == null)
		{
			this.guiData.reserveCharaIcon = new List<SelAssistantCtrl.CharaAllGUI.IconChara>();
		}
		for (int i = 0; i < 2; i++)
		{
			SelAssistantCtrl.CharaAllGUI.IconChara et = new SelAssistantCtrl.CharaAllGUI.IconChara(go.transform.Find("Icon_Chara0" + (i + 1).ToString()), go.transform.Find("AEImage_Mark_Ban0" + (i + 1).ToString()));
			et.iconCharaSet.iconCharaCtrl.AddOnClickListener(delegate(IconCharaCtrl x)
			{
				this.OnTouchCharaIcon(et);
			});
			Transform transform = et.iconCharaSet.baseObj.transform.Find("Random");
			if (transform != null)
			{
				PrjUtil.AddTouchEventTrigger(transform.gameObject, delegate(Transform x)
				{
					this.OnTouchRandomButton();
				});
			}
			this.guiData.reserveCharaIcon.Add(et);
		}
	}

	private void OnUpdateItemChara(int index, GameObject go)
	{
		if (this.dispCharaPackList == null || this.currentChara == null)
		{
			return;
		}
		for (int i = 0; i < 2; i++)
		{
			GameObject iconObj = go.transform.Find("Icon_Chara0" + (i + 1).ToString()).gameObject;
			SelAssistantCtrl.CharaAllGUI.IconChara iconChara = this.guiData.reserveCharaIcon.Find((SelAssistantCtrl.CharaAllGUI.IconChara item) => item.baseObj == iconObj);
			int num = index * 2 + i;
			if (this.dispCharaPackList.Count > num)
			{
				iconChara.baseObj.SetActive(true);
				CharaPackData cpd = this.dispCharaPackList[num];
				CharaWindowCtrl.DetailParamSetting.Preset preset = CharaWindowCtrl.DetailParamSetting.Preset.MINE_EASY;
				iconChara.iconCharaSet.iconCharaCtrl.Setup(cpd, this.sortType, false, new CharaWindowCtrl.DetailParamSetting(preset, this.dispCharaPackList)
				{
					openPrevCB = delegate
					{
						this.CharaSetup();
					}
				}, 0, -1, 0);
				this.SetupSelectDisable(iconChara);
				iconChara.iconCharaSet.randomFrame.SetActive(this.randomButtonCharaData == cpd);
				if (iconChara.iconCharaSet.randomFrame.activeSelf)
				{
					iconChara.iconCharaSet.selected.SetImageByName("shop_fnt_assistant_random");
					iconChara.iconCharaSet.selected.GetComponent<RectTransform>().sizeDelta = new Vector2(88f, 40f);
				}
				int num2 = (this.isRandom ? 0 : this.currentChara.id);
				iconChara.iconCharaSet.currentFrame.SetActive(this.currentChara != null && num2 == cpd.id);
				bool flag = cpd != null && this.PurchaseList.Exists((int item) => item == cpd.id);
				iconChara.iconCharaSet.iconCharaCtrl.IsEnableMask(!flag || cpd.id == this.CurrentCharaId);
			}
			else
			{
				iconChara.baseObj.SetActive(false);
			}
		}
	}

	public void OnClickAssistantButton()
	{
		if (this.currentMode == SelAssistantCtrl.Mode.TOP)
		{
			this.UpdateCharaPackList();
			this.UpdateDispCharaPackList(false);
			this.guiData.CharaAll.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
			this.guiData.ScrollView.Refresh();
			this.currentMode = SelAssistantCtrl.Mode.ASSISTANT_EDIT;
			return;
		}
		if (this.currentMode == SelAssistantCtrl.Mode.ASSISTANT_EDIT)
		{
			this.guiData.CharaAll.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
			this.currentMode = SelAssistantCtrl.Mode.TOP;
			return;
		}
		if (this.currentMode == SelAssistantCtrl.Mode.DISP_DHOLE && this.scene == SelAssistantCtrl.Scene.SHOP)
		{
			CanvasManager.HdlCmnReleaseConditionWindowCtrl.Open(PrjUtil.MakeMessage("解放条件"), new List<CmnReleaseConditionWindowCtrl.SetupParam>
			{
				new CmnReleaseConditionWindowCtrl.SetupParam
				{
					text = "メインストーリー 9章11話クリアで再解放",
					enableClear = false
				}
			});
		}
	}

	private void OnTouchCharaIcon(SelAssistantCtrl.CharaAllGUI.IconChara iconChara)
	{
		this.resistCharaId = 0;
		bool flag = this.isRandom;
		CharaPackData cpd = iconChara.iconCharaSet.iconCharaCtrl.charaPackData;
		bool flag2 = cpd != null && this.PurchaseList.Exists((int item) => item == cpd.id);
		if (flag2)
		{
			SoundManager.Play("prd_se_click", false, false);
			this.isRandom = false;
			this.SelectCharaIcon(cpd, flag ? this.randomButtonCharaData : this.currentChara, false);
			this.CharaSetup();
			iconChara.iconCharaSet.iconCharaCtrl.IsEnableMask(iconChara.iconCharaSet.selected.gameObject.activeSelf || iconChara.iconCharaSet.disable.activeSelf || !flag2);
			return;
		}
		if (cpd.id == 0)
		{
			return;
		}
		this.resistCharaId = cpd.id;
		this.SetupWindowConfirm(cpd);
		this.isHave = DataManager.DmChara.GetUserCharaData(cpd.id) != null;
		if (!this.isHave)
		{
			CanvasManager.HdlOpenWindowBasic.Setup("確認", "<color=red>おてつだいの対象フレンズが未加入です</color>", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, null, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			return;
		}
		this.windowBuyConfirm.owCtrl.Open();
	}

	private void OnTouchRandomButton()
	{
		SoundManager.Play("prd_se_click", false, false);
		this.isRandom = true;
		this.SelectCharaIcon(this.randomButtonCharaData, this.currentChara, true);
		this.CharaSetupRandom();
	}

	private int PickRandomChara()
	{
		int num = ((this.currentChara == null) ? this.CurrentCharaId : this.currentChara.id);
		int num3;
		do
		{
			int num2 = Random.Range(0, this.PurchaseList.Count);
			num3 = this.PurchaseList[num2];
		}
		while (this.PurchaseList.Count != 1 && num3 == num);
		return num3;
	}

	public void OnClickEditOk(PguiButtonCtrl button)
	{
		SelAssistantCtrl.Mode currentMode = this.currentMode;
		this.ChangeMode(SelAssistantCtrl.Mode.TOP);
		if (this.currentChara.id == this.CurrentCharaId)
		{
			return;
		}
		if (currentMode == SelAssistantCtrl.Mode.ASSISTANT_EDIT && button == this.guiData.Btn_EditOk)
		{
			this.currentEnumerator = this.RequestUpdateID();
		}
	}

	private bool OnSelectOpenWindowButtonCallback(int index)
	{
		if (index == PguiOpenWindowCtrl.CLOSE_BUTTON_INDEX)
		{
			UnityAction onCancel = this._onCancel;
			if (onCancel != null)
			{
				onCancel();
			}
			this.currentMode = this.preMode;
		}
		else if (this.currentMode == SelAssistantCtrl.Mode.OW_DISCARD_CHARA_RETRUEN || this.currentMode == SelAssistantCtrl.Mode.OW_DISCARD_CHARA_MOVE_SCENE)
		{
			UnityAction onFinish = this._onFinish;
			if (onFinish != null)
			{
				onFinish();
			}
			if (index == 1)
			{
				this.currentMode = this.preMode;
				this.currentEnumerator = this.RequestUpdateID();
				if (this.preMode == SelAssistantCtrl.Mode.OW_DISCARD_CHARA_RETRUEN)
				{
					if (this.currentMode != SelAssistantCtrl.Mode.TOP)
					{
						this.ChangeMode(SelAssistantCtrl.Mode.TOP);
					}
				}
				else
				{
					CanvasManager.HdlCmnMenu.MoveSceneByMenu(this.OnClickMoveSequenceName, this.OnClickMoveSequenceArgs);
				}
				this.btnAssistantEdit.gameObject.SetActive(true);
				this.guiData.CharaAll.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
			}
			else
			{
				DataManagerAssistant.UserAssistantData userData = DataManager.DmAssistant.UserData;
				int target = ((this.scene == SelAssistantCtrl.Scene.SHOP) ? userData.shopAssistantCharaId : userData.questAssistantCharaId);
				CharaPackData charaPackData;
				if (target != 0)
				{
					charaPackData = this.charaPackList.Find((CharaPackData item) => item.id == target);
				}
				else
				{
					charaPackData = this.randomButtonCharaData;
				}
				this.SelectCharaIcon(charaPackData, this.currentChara, charaPackData.id == 0);
				this.SetupAssistant();
				this.ResetCurrentIcon();
				this.currentMode = this.preMode;
				if (this.preMode == SelAssistantCtrl.Mode.OW_DISCARD_CHARA_RETRUEN)
				{
					if (this.currentMode != SelAssistantCtrl.Mode.TOP)
					{
						this.ChangeMode(SelAssistantCtrl.Mode.TOP);
					}
				}
				else
				{
					CanvasManager.HdlCmnMenu.MoveSceneByMenu(this.OnClickMoveSequenceName, this.OnClickMoveSequenceArgs);
				}
				this.btnAssistantEdit.gameObject.SetActive(true);
				this.guiData.CharaAll.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
			}
		}
		return true;
	}

	private IEnumerator RequestUpdateID()
	{
		if (this.scene == SelAssistantCtrl.Scene.SHOP)
		{
			DataManager.DmAssistant.RequestUpdateShopAssistant(this.isRandom ? 0 : this.currentChara.id);
		}
		else
		{
			DataManager.DmAssistant.RequestUpdateQuestAssistant(this.isRandom ? 0 : this.currentChara.id);
		}
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		yield break;
	}

	private void SetupWindowConfirm(CharaPackData cpd)
	{
		MstAssistantData mstAssistantData = DataManager.DmAssistant.GetShowDataList().Find((MstAssistantData item) => item.charaId == cpd.id);
		int userHaveNum = DataManagerItem.GetUserHaveNum(mstAssistantData.priceItemId);
		this.windowBuyConfirm.itemData = new ShopData.ItemOne();
		int priceItemId = mstAssistantData.priceItemId;
		if (priceItemId == 0)
		{
			this.windowBuyConfirm.NeedInfoImage.gameObject.SetActive(false);
		}
		string iconName = DataManager.DmItem.GetItemStaticBase(priceItemId).GetIconName();
		this.windowBuyConfirm.NeedInfoImage.SetRawImage(iconName, true, false, null);
		this.windowBuyConfirm.UseMoneyImage.SetRawImage(iconName, true, false, null);
		this.windowBuyConfirm.BuyObject.SetActive(true);
		ItemStaticBase itemStaticBase = DataManager.DmItem.GetItemStaticBase(cpd.id);
		this.windowBuyConfirm.IconItem.Setup(itemStaticBase, new IconItemCtrl.SetupParam
		{
			useMaxDetail = false
		});
		string text = ((this.scene == SelAssistantCtrl.Scene.SHOP) ? "ショップ" : "クエスト");
		this.windowBuyConfirm.Txt_BuyItemName.text = cpd.staticData.GetName() + "のおてつだい(" + text + ")";
		this.windowBuyConfirm.Txt_BuyItemInfo.text = cpd.staticData.GetName() + "に" + text + "でおてつだいしてもらえるようになります";
		this.windowBuyConfirm.itemData.priceItemId = mstAssistantData.priceItemId;
		this.windowBuyConfirm.itemData.priceItemNum = mstAssistantData.priceItemNum;
		this.windowBuyConfirm.Txt_Price.text = this.windowBuyConfirm.itemData.priceItemNum.ToString();
		this.windowBuyConfirm.Txt_BuyBeforeMoney.text = userHaveNum.ToString();
		int num = userHaveNum - this.windowBuyConfirm.itemData.priceItemNum * this.windowBuyConfirm.buyCount;
		this.windowBuyConfirm.Txt_BuyAfterMoney.text = Mathf.Max(0, num).ToString();
		this.windowBuyConfirm.Txt_BuyBeforeCount.text = "0";
		this.windowBuyConfirm.Txt_BuyAfterCount.text = "1";
		this.windowBuyConfirm.Txt_BuyItemCount.text = "1";
		bool flag = DataManagerItem.GetUserHaveNum(mstAssistantData.priceItemId) >= mstAssistantData.priceItemNum;
		string text2 = (flag ? PrjUtil.MakeMessage("") : PrjUtil.MakeMessage("アイテムが足りません"));
		this.windowBuyConfirm.Txt_owErrorText.text = text2;
		this.windowBuyConfirm.owCtrl.choiceR.GetComponent<PguiButtonCtrl>().SetActEnable(flag, false, false);
		this.windowBuyConfirm.owCtrl.Setup("解放確認", "", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, new PguiOpenWindowCtrl.Callback(this.OnClickConfirmButton), null, false);
	}

	private bool OnClickConfirmButton(int index)
	{
		if (index == 1)
		{
			if (this.scene == SelAssistantCtrl.Scene.SHOP)
			{
				DataManager.DmAssistant.RequestResistShopAssistant(this.resistCharaId);
			}
			else
			{
				DataManager.DmAssistant.RequestResistQuestAssistant(this.resistCharaId);
			}
			this.currentEnumerator = this.RequestResistShop();
		}
		return true;
	}

	private IEnumerator RequestResistShop()
	{
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		this.windowBuyEnd.owCtrl.Setup("解放完了", "を交換しました。", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, null, null, false);
		this.windowBuyEnd.Txt_ItemName.text = this.windowBuyConfirm.Txt_BuyItemName.text;
		this.windowBuyEnd.Txt_ItemCount.text = "を解放しました";
		this.windowBuyEnd.Txt_BuyBeforeMoney.text = this.windowBuyConfirm.Txt_BuyBeforeMoney.text;
		this.windowBuyEnd.Txt_BuyAfterMoney.text = this.windowBuyConfirm.Txt_BuyAfterMoney.text;
		this.windowBuyEnd.Txt_BuyBeforeCount.text = this.windowBuyConfirm.Txt_BuyBeforeCount.text;
		this.windowBuyEnd.Txt_BuyAfterCount.text = this.windowBuyConfirm.Txt_BuyAfterCount.text;
		this.windowBuyEnd.Txt_ItemReq.gameObject.SetActive(false);
		string iconName = DataManager.DmItem.GetItemStaticBase(this.windowBuyConfirm.itemData.priceItemId).GetIconName();
		this.windowBuyEnd.UseMoneyImage.SetRawImage(iconName, true, false, null);
		this.windowBuyEnd.owCtrl.Open();
		while (!this.windowBuyEnd.owCtrl.FinishedClose())
		{
			yield return null;
		}
		this.UpdateCharaPackList();
		this.UpdateDispCharaPackList(true);
		this.guiData.ScrollView.Refresh();
		yield break;
	}

	private void UpdateDispCharaPackList(bool isRefresh = false)
	{
		this.dispCharaPackList = new List<CharaPackData>(this.charaPackList);
		this.dispCharaPackList.Insert(0, this.randomButtonCharaData);
		if (isRefresh)
		{
			this.guiData.ScrollView.Refresh();
		}
	}

	private void UpdateCharaPackList()
	{
		List<MstAssistantData> showDataList = DataManager.DmAssistant.GetShowDataList();
		this.charaPackList = new List<CharaPackData>();
		using (List<MstAssistantData>.Enumerator enumerator = showDataList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				MstAssistantData data = enumerator.Current;
				CharaPackData charaPackData = DataManager.DmChara.GetUserCharaData(data.charaId);
				if (charaPackData == null)
				{
					charaPackData = CharaPackData.MakeDummy(data.charaId);
				}
				if (charaPackData != null && !this.charaPackList.Exists((CharaPackData item) => item.id == data.charaId))
				{
					this.charaPackList.Add(charaPackData);
				}
			}
		}
		this.guiData.ResizeScrollView(this.charaPackList.Count, (this.charaPackList.Count + 1) / 2 + 1);
	}

	private void UpdateTextureSetting()
	{
		this.renderTextureChara.postion = new Vector2(-440f, -124f);
		this.renderTextureChara.rotation = new Vector3(0f, 345f, 0f);
		this.renderTextureChara.fieldOfView = 17f;
		this.renderTextureChara.transform.SetSiblingIndex(0);
	}

	public void Destroy()
	{
		if (this.guiData != null)
		{
			Object.Destroy(this.guiData.baseObj);
			this.guiData = null;
		}
		if (this.windowBuyConfirm != null)
		{
			Object.Destroy(this.windowBuyConfirm.owCtrl.gameObject);
			this.windowBuyConfirm = null;
		}
		if (this.windowBuyEnd != null)
		{
			Object.Destroy(this.windowBuyEnd.owCtrl.gameObject);
			this.windowBuyEnd = null;
		}
	}

	private const int DEFAULT_SHOP_ASSISTANT_CHARA_ID = 21;

	private const int DEFAULT_QUEST_ASSISTANT_CHARA_ID = 1;

	private const int DEFAULT_CHARA_EQUIP_CLOTH_IMAGID = 0;

	private CharaPackData defaultAssistant;

	public GameObject grandObj;

	public PguiButtonCtrl btnAssistantEdit;

	public SelAssistantCtrl.CharaAllGUI guiData;

	public RenderTextureChara renderTextureChara;

	private List<CharaPackData> charaPackList = new List<CharaPackData>();

	private List<CharaPackData> dispCharaPackList;

	private CharaPackData randomButtonCharaData = CharaPackData.MakeInvalid();

	private SortFilterDefine.SortType sortType;

	private bool isChangeClone;

	private bool isRandom;

	private bool isHave;

	private int currentCharaCloth;

	private bool currentCharaSkirt;

	private CharaPackData currentChara;

	private int resistCharaId;

	private SelAssistantCtrl.Scene scene;

	private SelAssistantCtrl.Mode _currentMode;

	private SelAssistantCtrl.Mode _preMode;

	private IEnumerator currentEnumerator;

	private SelAssistantCtrl.WindowBuyConfirm windowBuyConfirm;

	private SelShopCtrl.WindowBuyEnd windowBuyEnd;

	public static readonly List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>> questionButtonSet = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>
	{
		new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.NEGATIVE, PrjUtil.MakeMessage("破棄して移動")),
		new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, PrjUtil.MakeMessage("保存して移動"))
	};

	private UnityAction _onFinish;

	private UnityAction _onCancel;

	private SceneManager.SceneName OnClickMoveSequenceName;

	private object OnClickMoveSequenceArgs;

	public class WindowBuyConfirm
	{
		public WindowBuyConfirm(Transform baseTr, Transform windowBase)
		{
			this.buyCount = 1;
			this.buyMax = 0;
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.Base_BuyInfo = windowBase.Find("Base_BuyInfo").gameObject;
			this.Txt_owErrorText = windowBase.Find("BtnOk/DisableText").GetComponent<PguiTextCtrl>();
			this.Txt_BuyItemName = windowBase.Find("Base_BuyInfo/Txt01").GetComponent<PguiTextCtrl>();
			this.Txt_BuyItemInfo = windowBase.Find("Base_BuyInfo/Txt02").GetComponent<PguiTextCtrl>();
			this.Txt_BuyItemCount = windowBase.Find("Base_BuyInfo/Buy_Img/Txt_Window/Num_Txt").GetComponent<PguiTextCtrl>();
			this.Txt_BuyItemType = windowBase.Find("Base_BuyInfo/Parts_Exchange/Parts_ItemUseCoin/Txt01").GetComponent<PguiTextCtrl>();
			this.Txt_BuyBeforeMoney = windowBase.Find("Base_BuyInfo/Parts_Exchange/Parts_ItemUseCoin/Num_BeforeTxt").GetComponent<PguiTextCtrl>();
			this.Txt_BuyAfterMoney = windowBase.Find("Base_BuyInfo/Parts_Exchange/Parts_ItemUseCoin/Num_AfterTxt").GetComponent<PguiTextCtrl>();
			this.Txt_BuyBeforeCount = windowBase.Find("Base_BuyInfo/Parts_Exchange/Parts_ItemUseInfo/Num_BeforeTxt").GetComponent<PguiTextCtrl>();
			this.Txt_BuyAfterCount = windowBase.Find("Base_BuyInfo/Parts_Exchange/Parts_ItemUseInfo/Num_AfterTxt").GetComponent<PguiTextCtrl>();
			this.Parts_ItemUseInfo = windowBase.Find("Base_BuyInfo/Parts_Exchange/Parts_ItemUseInfo").gameObject;
			this.Parts_Exchange = windowBase.Find("Base_BuyInfo/Parts_Exchange").gameObject;
			this.Txt_Price = windowBase.Find("Base_BuyInfo/Parts_Exchange/Parts_ItemNeedInfo/Num_Txt").GetComponent<PguiTextCtrl>();
			this.BuyObject = windowBase.Find("Base_BuyInfo/Buy_Img").gameObject;
			this.NeedInfoImage = windowBase.Find("Base_BuyInfo/Parts_Exchange/Parts_ItemNeedInfo/Icon_Tex").GetComponent<PguiRawImageCtrl>();
			this.UseInfoImage = windowBase.Find("Base_BuyInfo/Parts_Exchange/Parts_ItemUseInfo/Icon_Tex").GetComponent<PguiRawImageCtrl>();
			this.UseMoneyImage = windowBase.Find("Base_BuyInfo/Parts_Exchange/Parts_ItemUseCoin/Icon_Tex").GetComponent<PguiRawImageCtrl>();
			this.UseInfoImage.gameObject.SetActive(false);
			this.IconItem = windowBase.Find("Base_BuyInfo/Buy_Img/Icon_Item").GetComponent<IconItemCtrl>();
		}

		public PguiOpenWindowCtrl owCtrl;

		public PguiTextCtrl Txt_owErrorText;

		public PguiTextCtrl Txt_BuyItemName;

		public PguiTextCtrl Txt_BuyItemInfo;

		public PguiTextCtrl Txt_BuyItemCount;

		public PguiTextCtrl Txt_BuyItemType;

		public PguiTextCtrl Txt_BuyBeforeMoney;

		public PguiTextCtrl Txt_BuyAfterMoney;

		public PguiTextCtrl Txt_BuyBeforeCount;

		public PguiTextCtrl Txt_BuyAfterCount;

		public PguiTextCtrl Txt_Price;

		public IconItemCtrl IconItem;

		public GameObject BuyObject;

		public PguiRawImageCtrl NeedInfoImage;

		public PguiRawImageCtrl UseInfoImage;

		public PguiRawImageCtrl UseMoneyImage;

		public GameObject Base_BuyInfo;

		public GameObject Parts_ItemUseInfo;

		public GameObject Parts_Exchange;

		public ShopData.ItemOne itemData;

		public int buyCount;

		public int buyMax;
	}

	public enum Mode
	{
		INVALID,
		TOP,
		ASSISTANT_EDIT,
		DISP_DHOLE,
		OW_DISCARD_CHARA_RETRUEN,
		OW_DISCARD_CHARA_TAB,
		OW_DISCARD_CHARA_MOVE_SCENE
	}

	public enum Scene
	{
		SHOP,
		QUEST
	}

	public class CharaAllGUI
	{
		public CharaAllGUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.tapGuard = baseTr.Find("tapGuard").gameObject;
			Transform transform = baseTr.Find("CharaAll/Btn_EditOk");
			if (transform)
			{
				this.Btn_EditOk = transform.GetComponent<PguiButtonCtrl>();
			}
			baseTr.Find("CharaAll/TopBtns/Btn_FilterOnOff").gameObject.SetActive(false);
			baseTr.Find("CharaAll/TopBtns/Btn_Sort").gameObject.SetActive(false);
			baseTr.Find("CharaAll/TopBtns/Btn_SortUpDown").gameObject.SetActive(false);
			Transform transform2 = baseTr.Find("CharaAll/ScrollView");
			if (transform2)
			{
				this.ScrollView = transform2.GetComponent<ReuseScroll>();
			}
			Transform transform3 = baseTr.Find("CharaAll/Txt_None");
			if (transform3)
			{
				this.Txt_None = transform3.gameObject;
				this.Txt_None.SetActive(false);
			}
			this.tapGuard.SetActive(false);
			this.CharaAll = baseTr.GetComponent<SimpleAnimation>();
		}

		public void ResizeScrollView(int count, int resize)
		{
			if (this.Txt_None == null)
			{
				return;
			}
			this.Txt_None.SetActive(count <= 0);
			this.ScrollView.ResizeFocesNoMove(resize);
		}

		public SelAssistantCtrl.CharaAllGUI.IconChara SearchIconChara(CharaPackData scd)
		{
			if (scd != null)
			{
				return this.reserveCharaIcon.Find((SelAssistantCtrl.CharaAllGUI.IconChara item) => item.iconCharaSet.iconCharaCtrl.charaPackData == scd);
			}
			return null;
		}

		public GameObject baseObj;

		public GameObject tapGuard;

		public PguiButtonCtrl Btn_EditOk;

		public ReuseScroll ScrollView;

		public GameObject Txt_None;

		public List<SelAssistantCtrl.CharaAllGUI.IconChara> reserveCharaIcon;

		public PguiTabGroupCtrl DeckTab;

		public SimpleAnimation CharaAll;

		public class IconCharaSet
		{
			public IconCharaSet(Transform baseTr)
			{
				this.baseObj = baseTr.gameObject;
				this.iconBase = baseTr.Find("Icon_Chara").GetComponent<RectTransform>();
				GameObject gameObject = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_Chara, this.iconBase);
				this.iconCharaCtrl = gameObject.GetComponent<IconCharaCtrl>();
				this.currentFrame = baseTr.Find("Current").gameObject;
				this.currentFrame.SetActive(false);
				Transform transform = baseTr.Find("Remove");
				if (transform != null)
				{
					this.removeFrame = transform.gameObject;
					this.removeFrame.SetActive(false);
				}
				Transform transform2 = baseTr.Find("Random");
				if (transform2 != null)
				{
					this.randomFrame = transform2.gameObject;
					this.randomFrame.SetActive(false);
				}
				this.selected = baseTr.Find("Fnt_Selected").GetComponent<PguiImageCtrl>();
				this.selected.SetImageByName("shop_fnt_assistant");
				this.selected.gameObject.SetActive(false);
				this.selected.GetComponent<RectTransform>().sizeDelta = new Vector2(170f, 40f);
				this.disable = baseTr.Find("Txt_Disable").gameObject;
				this.disable.SetActive(false);
			}

			public GameObject baseObj;

			public RectTransform iconBase;

			public IconCharaCtrl iconCharaCtrl;

			public GameObject currentFrame;

			public GameObject removeFrame;

			public GameObject randomFrame;

			public PguiImageCtrl selected;

			public GameObject disable;
		}

		public class IconChara
		{
			public IconChara(Transform baseTr, Transform banTr)
			{
				this.baseObj = baseTr.gameObject;
				this.anime = baseTr.GetComponent<SimpleAnimation>();
				if (this.anime != null)
				{
					this.anime.ExInit();
					this.anime.ExPauseAnimation(SimpleAnimation.ExPguiStatus.START, null);
				}
				if (banTr == null)
				{
					banTr = baseTr.Find("AEImage_Mark_Ban");
				}
				this.banObj = banTr.gameObject;
				this.banObj.SetActive(false);
				GameObject gameObject = Object.Instantiate<GameObject>(CanvasManager.RefResource.Icon_CharaSet, baseTr);
				this.iconCharaSet = new SelAssistantCtrl.CharaAllGUI.IconCharaSet(gameObject.transform);
				Transform transform = baseTr.Find("Icon_Chara");
				if (transform != null)
				{
					this.iconCharaSet.baseObj.transform.SetParent(transform, false);
				}
			}

			public const int SCROLL_ITEM_NUN_H = 2;

			public GameObject baseObj;

			public SimpleAnimation anime;

			public SelAssistantCtrl.CharaAllGUI.IconCharaSet iconCharaSet;

			public List<PguiReplaceSpriteCtrl> iconBlankFrame;

			public List<PguiReplaceSpriteCtrl> iconStatusKind;

			public GameObject Mark_Friend;

			public PguiReplaceSpriteCtrl Base_CharaBlank;

			public GameObject Base_CharaBlank_Friend;

			public GameObject banObj;
		}
	}
}
