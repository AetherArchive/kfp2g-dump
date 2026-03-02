using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.Mst;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x0200017D RID: 381
public class SelAssistantCtrl : MonoBehaviour
{
	// Token: 0x170003C0 RID: 960
	// (get) Token: 0x0600184E RID: 6222 RVA: 0x0012A778 File Offset: 0x00128978
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

	// Token: 0x170003C1 RID: 961
	// (get) Token: 0x0600184F RID: 6223 RVA: 0x0012A7AD File Offset: 0x001289AD
	// (set) Token: 0x06001850 RID: 6224 RVA: 0x0012A7B5 File Offset: 0x001289B5
	public GameObject mainObj { get; private set; }

	// Token: 0x170003C2 RID: 962
	// (get) Token: 0x06001851 RID: 6225 RVA: 0x0012A7BE File Offset: 0x001289BE
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

	// Token: 0x170003C3 RID: 963
	// (get) Token: 0x06001852 RID: 6226 RVA: 0x0012A7EC File Offset: 0x001289EC
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

	// Token: 0x170003C4 RID: 964
	// (get) Token: 0x06001853 RID: 6227 RVA: 0x0012A819 File Offset: 0x00128A19
	// (set) Token: 0x06001854 RID: 6228 RVA: 0x0012A821 File Offset: 0x00128A21
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

	// Token: 0x170003C5 RID: 965
	// (get) Token: 0x06001855 RID: 6229 RVA: 0x0012A836 File Offset: 0x00128A36
	public SelAssistantCtrl.Mode preMode
	{
		get
		{
			return this._preMode;
		}
	}

	// Token: 0x170003C6 RID: 966
	// (get) Token: 0x06001856 RID: 6230 RVA: 0x0012A83E File Offset: 0x00128A3E
	// (set) Token: 0x06001857 RID: 6231 RVA: 0x0012A849 File Offset: 0x00128A49
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

	// Token: 0x06001858 RID: 6232 RVA: 0x0012A84C File Offset: 0x00128A4C
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

	// Token: 0x06001859 RID: 6233 RVA: 0x0012AA28 File Offset: 0x00128C28
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

	// Token: 0x0600185A RID: 6234 RVA: 0x0012AA7B File Offset: 0x00128C7B
	private void Update()
	{
		if (this.currentEnumerator != null && !this.currentEnumerator.MoveNext())
		{
			this.currentEnumerator = null;
		}
	}

	// Token: 0x0600185B RID: 6235 RVA: 0x0012AA9C File Offset: 0x00128C9C
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

	// Token: 0x0600185C RID: 6236 RVA: 0x0012ABF0 File Offset: 0x00128DF0
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

	// Token: 0x0600185D RID: 6237 RVA: 0x0012AD3C File Offset: 0x00128F3C
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

	// Token: 0x0600185E RID: 6238 RVA: 0x0012ADA8 File Offset: 0x00128FA8
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

	// Token: 0x0600185F RID: 6239 RVA: 0x0012AE54 File Offset: 0x00129054
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

	// Token: 0x06001860 RID: 6240 RVA: 0x0012AF04 File Offset: 0x00129104
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

	// Token: 0x06001861 RID: 6241 RVA: 0x0012AF58 File Offset: 0x00129158
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

	// Token: 0x06001862 RID: 6242 RVA: 0x0012AF9C File Offset: 0x0012919C
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

	// Token: 0x06001863 RID: 6243 RVA: 0x0012B0AC File Offset: 0x001292AC
	private void SetupSelectDisable()
	{
		foreach (SelAssistantCtrl.CharaAllGUI.IconChara iconChara in this.guiData.reserveCharaIcon)
		{
			this.SetupSelectDisable(iconChara);
		}
	}

	// Token: 0x06001864 RID: 6244 RVA: 0x0012B104 File Offset: 0x00129304
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

	// Token: 0x06001865 RID: 6245 RVA: 0x0012B240 File Offset: 0x00129440
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

	// Token: 0x06001866 RID: 6246 RVA: 0x0012B34C File Offset: 0x0012954C
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

	// Token: 0x06001867 RID: 6247 RVA: 0x0012B560 File Offset: 0x00129760
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

	// Token: 0x06001868 RID: 6248 RVA: 0x0012B618 File Offset: 0x00129818
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

	// Token: 0x06001869 RID: 6249 RVA: 0x0012B77E File Offset: 0x0012997E
	private void OnTouchRandomButton()
	{
		SoundManager.Play("prd_se_click", false, false);
		this.isRandom = true;
		this.SelectCharaIcon(this.randomButtonCharaData, this.currentChara, true);
		this.CharaSetupRandom();
	}

	// Token: 0x0600186A RID: 6250 RVA: 0x0012B7B0 File Offset: 0x001299B0
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

	// Token: 0x0600186B RID: 6251 RVA: 0x0012B810 File Offset: 0x00129A10
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

	// Token: 0x0600186C RID: 6252 RVA: 0x0012B864 File Offset: 0x00129A64
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

	// Token: 0x0600186D RID: 6253 RVA: 0x0012BA0F File Offset: 0x00129C0F
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

	// Token: 0x0600186E RID: 6254 RVA: 0x0012BA20 File Offset: 0x00129C20
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

	// Token: 0x0600186F RID: 6255 RVA: 0x0012BD07 File Offset: 0x00129F07
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

	// Token: 0x06001870 RID: 6256 RVA: 0x0012BD44 File Offset: 0x00129F44
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

	// Token: 0x06001871 RID: 6257 RVA: 0x0012BD53 File Offset: 0x00129F53
	private void UpdateDispCharaPackList(bool isRefresh = false)
	{
		this.dispCharaPackList = new List<CharaPackData>(this.charaPackList);
		this.dispCharaPackList.Insert(0, this.randomButtonCharaData);
		if (isRefresh)
		{
			this.guiData.ScrollView.Refresh();
		}
	}

	// Token: 0x06001872 RID: 6258 RVA: 0x0012BD8C File Offset: 0x00129F8C
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

	// Token: 0x06001873 RID: 6259 RVA: 0x0012BE6C File Offset: 0x0012A06C
	private void UpdateTextureSetting()
	{
		this.renderTextureChara.postion = new Vector2(-440f, -124f);
		this.renderTextureChara.rotation = new Vector3(0f, 345f, 0f);
		this.renderTextureChara.fieldOfView = 17f;
		this.renderTextureChara.transform.SetSiblingIndex(0);
	}

	// Token: 0x06001874 RID: 6260 RVA: 0x0012BED4 File Offset: 0x0012A0D4
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

	// Token: 0x040012BA RID: 4794
	private const int DEFAULT_SHOP_ASSISTANT_CHARA_ID = 21;

	// Token: 0x040012BB RID: 4795
	private const int DEFAULT_QUEST_ASSISTANT_CHARA_ID = 1;

	// Token: 0x040012BC RID: 4796
	private const int DEFAULT_CHARA_EQUIP_CLOTH_IMAGID = 0;

	// Token: 0x040012BD RID: 4797
	private CharaPackData defaultAssistant;

	// Token: 0x040012BF RID: 4799
	public GameObject grandObj;

	// Token: 0x040012C0 RID: 4800
	public PguiButtonCtrl btnAssistantEdit;

	// Token: 0x040012C1 RID: 4801
	public SelAssistantCtrl.CharaAllGUI guiData;

	// Token: 0x040012C2 RID: 4802
	public RenderTextureChara renderTextureChara;

	// Token: 0x040012C3 RID: 4803
	private List<CharaPackData> charaPackList = new List<CharaPackData>();

	// Token: 0x040012C4 RID: 4804
	private List<CharaPackData> dispCharaPackList;

	// Token: 0x040012C5 RID: 4805
	private CharaPackData randomButtonCharaData = CharaPackData.MakeInvalid();

	// Token: 0x040012C6 RID: 4806
	private SortFilterDefine.SortType sortType;

	// Token: 0x040012C7 RID: 4807
	private bool isChangeClone;

	// Token: 0x040012C8 RID: 4808
	private bool isRandom;

	// Token: 0x040012C9 RID: 4809
	private bool isHave;

	// Token: 0x040012CA RID: 4810
	private int currentCharaCloth;

	// Token: 0x040012CB RID: 4811
	private bool currentCharaSkirt;

	// Token: 0x040012CC RID: 4812
	private CharaPackData currentChara;

	// Token: 0x040012CD RID: 4813
	private int resistCharaId;

	// Token: 0x040012CE RID: 4814
	private SelAssistantCtrl.Scene scene;

	// Token: 0x040012CF RID: 4815
	private SelAssistantCtrl.Mode _currentMode;

	// Token: 0x040012D0 RID: 4816
	private SelAssistantCtrl.Mode _preMode;

	// Token: 0x040012D1 RID: 4817
	private IEnumerator currentEnumerator;

	// Token: 0x040012D2 RID: 4818
	private SelAssistantCtrl.WindowBuyConfirm windowBuyConfirm;

	// Token: 0x040012D3 RID: 4819
	private SelShopCtrl.WindowBuyEnd windowBuyEnd;

	// Token: 0x040012D4 RID: 4820
	public static readonly List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>> questionButtonSet = new List<KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>>
	{
		new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.NEGATIVE, PrjUtil.MakeMessage("破棄して移動")),
		new KeyValuePair<PguiOpenWindowCtrl.BTN_TYPE, string>(PguiOpenWindowCtrl.BTN_TYPE.POSITIVE, PrjUtil.MakeMessage("保存して移動"))
	};

	// Token: 0x040012D5 RID: 4821
	private UnityAction _onFinish;

	// Token: 0x040012D6 RID: 4822
	private UnityAction _onCancel;

	// Token: 0x040012D7 RID: 4823
	private SceneManager.SceneName OnClickMoveSequenceName;

	// Token: 0x040012D8 RID: 4824
	private object OnClickMoveSequenceArgs;

	// Token: 0x02000D3D RID: 3389
	public class WindowBuyConfirm
	{
		// Token: 0x06004893 RID: 18579 RVA: 0x0021C614 File Offset: 0x0021A814
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

		// Token: 0x04004DCF RID: 19919
		public PguiOpenWindowCtrl owCtrl;

		// Token: 0x04004DD0 RID: 19920
		public PguiTextCtrl Txt_owErrorText;

		// Token: 0x04004DD1 RID: 19921
		public PguiTextCtrl Txt_BuyItemName;

		// Token: 0x04004DD2 RID: 19922
		public PguiTextCtrl Txt_BuyItemInfo;

		// Token: 0x04004DD3 RID: 19923
		public PguiTextCtrl Txt_BuyItemCount;

		// Token: 0x04004DD4 RID: 19924
		public PguiTextCtrl Txt_BuyItemType;

		// Token: 0x04004DD5 RID: 19925
		public PguiTextCtrl Txt_BuyBeforeMoney;

		// Token: 0x04004DD6 RID: 19926
		public PguiTextCtrl Txt_BuyAfterMoney;

		// Token: 0x04004DD7 RID: 19927
		public PguiTextCtrl Txt_BuyBeforeCount;

		// Token: 0x04004DD8 RID: 19928
		public PguiTextCtrl Txt_BuyAfterCount;

		// Token: 0x04004DD9 RID: 19929
		public PguiTextCtrl Txt_Price;

		// Token: 0x04004DDA RID: 19930
		public IconItemCtrl IconItem;

		// Token: 0x04004DDB RID: 19931
		public GameObject BuyObject;

		// Token: 0x04004DDC RID: 19932
		public PguiRawImageCtrl NeedInfoImage;

		// Token: 0x04004DDD RID: 19933
		public PguiRawImageCtrl UseInfoImage;

		// Token: 0x04004DDE RID: 19934
		public PguiRawImageCtrl UseMoneyImage;

		// Token: 0x04004DDF RID: 19935
		public GameObject Base_BuyInfo;

		// Token: 0x04004DE0 RID: 19936
		public GameObject Parts_ItemUseInfo;

		// Token: 0x04004DE1 RID: 19937
		public GameObject Parts_Exchange;

		// Token: 0x04004DE2 RID: 19938
		public ShopData.ItemOne itemData;

		// Token: 0x04004DE3 RID: 19939
		public int buyCount;

		// Token: 0x04004DE4 RID: 19940
		public int buyMax;
	}

	// Token: 0x02000D3E RID: 3390
	public enum Mode
	{
		// Token: 0x04004DE6 RID: 19942
		INVALID,
		// Token: 0x04004DE7 RID: 19943
		TOP,
		// Token: 0x04004DE8 RID: 19944
		ASSISTANT_EDIT,
		// Token: 0x04004DE9 RID: 19945
		DISP_DHOLE,
		// Token: 0x04004DEA RID: 19946
		OW_DISCARD_CHARA_RETRUEN,
		// Token: 0x04004DEB RID: 19947
		OW_DISCARD_CHARA_TAB,
		// Token: 0x04004DEC RID: 19948
		OW_DISCARD_CHARA_MOVE_SCENE
	}

	// Token: 0x02000D3F RID: 3391
	public enum Scene
	{
		// Token: 0x04004DEE RID: 19950
		SHOP,
		// Token: 0x04004DEF RID: 19951
		QUEST
	}

	// Token: 0x02000D40 RID: 3392
	public class CharaAllGUI
	{
		// Token: 0x06004894 RID: 18580 RVA: 0x0021C7E0 File Offset: 0x0021A9E0
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

		// Token: 0x06004895 RID: 18581 RVA: 0x0021C8DB File Offset: 0x0021AADB
		public void ResizeScrollView(int count, int resize)
		{
			if (this.Txt_None == null)
			{
				return;
			}
			this.Txt_None.SetActive(count <= 0);
			this.ScrollView.ResizeFocesNoMove(resize);
		}

		// Token: 0x06004896 RID: 18582 RVA: 0x0021C90C File Offset: 0x0021AB0C
		public SelAssistantCtrl.CharaAllGUI.IconChara SearchIconChara(CharaPackData scd)
		{
			if (scd != null)
			{
				return this.reserveCharaIcon.Find((SelAssistantCtrl.CharaAllGUI.IconChara item) => item.iconCharaSet.iconCharaCtrl.charaPackData == scd);
			}
			return null;
		}

		// Token: 0x04004DF0 RID: 19952
		public GameObject baseObj;

		// Token: 0x04004DF1 RID: 19953
		public GameObject tapGuard;

		// Token: 0x04004DF2 RID: 19954
		public PguiButtonCtrl Btn_EditOk;

		// Token: 0x04004DF3 RID: 19955
		public ReuseScroll ScrollView;

		// Token: 0x04004DF4 RID: 19956
		public GameObject Txt_None;

		// Token: 0x04004DF5 RID: 19957
		public List<SelAssistantCtrl.CharaAllGUI.IconChara> reserveCharaIcon;

		// Token: 0x04004DF6 RID: 19958
		public PguiTabGroupCtrl DeckTab;

		// Token: 0x04004DF7 RID: 19959
		public SimpleAnimation CharaAll;

		// Token: 0x020011CB RID: 4555
		public class IconCharaSet
		{
			// Token: 0x06005720 RID: 22304 RVA: 0x00256150 File Offset: 0x00254350
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

			// Token: 0x040061AC RID: 25004
			public GameObject baseObj;

			// Token: 0x040061AD RID: 25005
			public RectTransform iconBase;

			// Token: 0x040061AE RID: 25006
			public IconCharaCtrl iconCharaCtrl;

			// Token: 0x040061AF RID: 25007
			public GameObject currentFrame;

			// Token: 0x040061B0 RID: 25008
			public GameObject removeFrame;

			// Token: 0x040061B1 RID: 25009
			public GameObject randomFrame;

			// Token: 0x040061B2 RID: 25010
			public PguiImageCtrl selected;

			// Token: 0x040061B3 RID: 25011
			public GameObject disable;
		}

		// Token: 0x020011CC RID: 4556
		public class IconChara
		{
			// Token: 0x06005721 RID: 22305 RVA: 0x0025629C File Offset: 0x0025449C
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

			// Token: 0x040061B4 RID: 25012
			public const int SCROLL_ITEM_NUN_H = 2;

			// Token: 0x040061B5 RID: 25013
			public GameObject baseObj;

			// Token: 0x040061B6 RID: 25014
			public SimpleAnimation anime;

			// Token: 0x040061B7 RID: 25015
			public SelAssistantCtrl.CharaAllGUI.IconCharaSet iconCharaSet;

			// Token: 0x040061B8 RID: 25016
			public List<PguiReplaceSpriteCtrl> iconBlankFrame;

			// Token: 0x040061B9 RID: 25017
			public List<PguiReplaceSpriteCtrl> iconStatusKind;

			// Token: 0x040061BA RID: 25018
			public GameObject Mark_Friend;

			// Token: 0x040061BB RID: 25019
			public PguiReplaceSpriteCtrl Base_CharaBlank;

			// Token: 0x040061BC RID: 25020
			public GameObject Base_CharaBlank_Friend;

			// Token: 0x040061BD RID: 25021
			public GameObject banObj;
		}
	}
}
