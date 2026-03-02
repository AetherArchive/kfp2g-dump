using System;
using System.Collections;
using System.Collections.Generic;
using Battle;
using SGNFW.Common;
using SGNFW.HttpRequest.Protocol;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000120 RID: 288
public class QuestSkipWindowsCtrl : MonoBehaviour
{
	// Token: 0x06000EAD RID: 3757 RVA: 0x000B2CB9 File Offset: 0x000B0EB9
	public void Initialize()
	{
		this.guiQuestSkipWindow = new QuestSkipWindowsCtrl.QuestSkipWindow(base.transform);
	}

	// Token: 0x06000EAE RID: 3758 RVA: 0x000B2CCC File Offset: 0x000B0ECC
	private void Update()
	{
		if (this.skipCoroutine != null && !this.skipCoroutine.MoveNext())
		{
			this.skipCoroutine = null;
		}
		if (this.guiQuestSkipWindow == null || this.guiQuestSkipWindow.window.FinishedClose())
		{
			this.isOpenWindow = false;
		}
	}

	// Token: 0x06000EAF RID: 3759 RVA: 0x000B2D0B File Offset: 0x000B0F0B
	public bool IsOpenWindow()
	{
		return this.isOpenWindow;
	}

	// Token: 0x06000EB0 RID: 3760 RVA: 0x000B2D14 File Offset: 0x000B0F14
	public void InitializeWindow(int questOneId, HelperPackData helperPackData, int attrIndex, int deckId, int beforeSelect = 0)
	{
		if (this.guiQuestSkipWindow == null)
		{
			this.Initialize();
		}
		QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(questOneId);
		QuestUtil.UsrQuestSkipInfo skipInfo = QuestUtil.GetSkipInfo(DataManager.DmMonthlyPack.GetValidMonthlyPackData(), questOnePackData);
		if (!skipInfo.isSkippable)
		{
			CanvasManager.HdlOpenWindowBasic.Setup("エラー", "現在スキップができません。\n月間パスポートの購入状況を確認してください。", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, (int idx) => true, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			return;
		}
		this.currentDeckId = deckId;
		int num = DataManager.DmItem.GetUserItemData(39000).num;
		this.selectedQuestOneId = questOneId;
		this.helperPackData = helperPackData;
		CharaPackData charaPackData = ((helperPackData != null && helperPackData.HelperCharaSetList[attrIndex].helpChara != null) ? helperPackData.HelperCharaSetList[attrIndex].helpChara : CharaPackData.MakeInvalid());
		this.helperCharaId = charaPackData.id;
		this.helperAttrIndex = attrIndex;
		this.helperPhotoIdList = null;
		this.isEventQuest = questOnePackData.questChapter.category == QuestStaticChapter.Category.EVENT;
		if (this.isEventQuest)
		{
			List<PhotoPackData> list = ((helperPackData != null) ? helperPackData.HelperCharaSetList[attrIndex].helpPhotoList : new List<PhotoPackData>());
			if (list != null && list.Count > 0)
			{
				this.helperPhotoIdList = new List<long>();
				foreach (PhotoPackData photoPackData in list)
				{
					this.helperPhotoIdList.Add((photoPackData != null) ? photoPackData.dataId : 0L);
				}
			}
		}
		this.selectedItemNum = ((num <= 0) ? 0 : ((beforeSelect == 0) ? 1 : beforeSelect));
		Transform transform = this.guiQuestSkipWindow.baseObj.transform.Find("Base/Window/Base_UseInfo/Icon_Item/Icon_Item");
		if (null != transform)
		{
			IconItemCtrl component = transform.GetComponent<IconItemCtrl>();
			if (null != component)
			{
				component.Setup(DataManager.DmItem.GetItemStaticBase(39000));
			}
		}
		int restSkipCount = skipInfo.restSkipCount;
		int restSkipRecoveryCount = skipInfo.restSkipRecoveryCount;
		int num2 = DataManager.DmQuest.CalcRestPlayNumByQuestOneId(questOnePackData.questOne.questId);
		int num3 = DataManager.DmQuest.CalcRestPlayNumByQuestGroupId(questOnePackData.questOne.questGroupId);
		int maxNum = ((50 <= DataManager.DmItem.GetUserItemData(39000).num) ? 50 : DataManager.DmItem.GetUserItemData(39000).num);
		if (skipInfo.hasSkipLimit)
		{
			maxNum = ((maxNum > restSkipCount) ? restSkipCount : maxNum);
		}
		if (num2 >= 0)
		{
			maxNum = ((maxNum > num2) ? num2 : maxNum);
		}
		if (num3 >= 0)
		{
			maxNum = ((maxNum > num3) ? num3 : maxNum);
		}
		this.guiQuestSkipWindow.maxNum = maxNum;
		this.guiQuestSkipWindow.messageText.gameObject.SetActive(skipInfo.hasSkipLimit);
		bool flag = skipInfo.hasSkipLimit && skipInfo.restSkipCount == 0;
		this.guiQuestSkipWindow.buttonOK.transform.Find("BaseImage/Text").GetComponent<PguiTextCtrl>().text = (flag ? "回数を回復する" : "スキップする");
		this.guiQuestSkipWindow.sliderObjImage.GetComponent<Image>().color = (flag ? (QuestSkipWindowsCtrl.DisableColor * this.guiQuestSkipWindow.sliderObjImage.GetComponent<PguiDataHolder>().color) : this.guiQuestSkipWindow.sliderObjImage.GetComponent<PguiDataHolder>().color);
		if (flag)
		{
			this.guiQuestSkipWindow.messageText.text = (skipInfo.isSkipByGroup ? "同一クエストタブ\u3000内で" : "このクエストは") + "、" + skipInfo.prefixStr + "の\n残りスキップ可能回数が0のためスキップできません";
			this.selectedItemNum = 0;
		}
		else
		{
			this.guiQuestSkipWindow.messageText.text = string.Format("{0}、{1}残り{2}回スキップ可能です", skipInfo.isSkipByGroup ? "同一クエストタブ\u3000内で" : "このクエストは", skipInfo.prefixStr, skipInfo.restSkipCount);
		}
		if (this.guiQuestSkipWindow.messageText.gameObject.activeSelf && this.guiQuestSkipWindow.messageText.text.IndexOf("同一クエストタブ") >= 0)
		{
			base.StartCoroutine(this.DispQuestTabObj());
		}
		else
		{
			this.guiQuestSkipWindow.questTab.SetActive(false);
		}
		int nowStaminaNum = DataManager.DmUserInfo.staminaInfo.GetInfoByNow(TimeManager.Now).stackNum;
		int needStaminaNum = questOnePackData.questOne.stamina;
		this.guiQuestSkipWindow.numBeforeStaminaText.text = nowStaminaNum.ToString();
		this.guiQuestSkipWindow.numUseStaminaText.text = (this.selectedItemNum * needStaminaNum).ToString();
		this.guiQuestSkipWindow.numUseStaminaText.m_Text.color = ((flag || this.selectedItemNum * needStaminaNum > nowStaminaNum) ? Color.red : Color.white);
		this.guiQuestSkipWindow.haveNumText.text = num.ToString();
		this.guiQuestSkipWindow.slider.onValueChanged.RemoveAllListeners();
		this.guiQuestSkipWindow.slider.minValue = (float)((maxNum > 0) ? 1 : 0);
		this.guiQuestSkipWindow.slider.maxValue = (float)maxNum;
		this.guiQuestSkipWindow.SetButton(this.selectedItemNum);
		this.guiQuestSkipWindow.excText.text = this.selectedItemNum.ToString() + " / " + maxNum.ToString();
		this.guiQuestSkipWindow.excText.m_Text.color = (flag ? Color.red : Color.white);
		PguiOutline[] components = this.guiQuestSkipWindow.excText.GetComponents<PguiOutline>();
		for (int i = 0; i < components.Length; i++)
		{
			components[i].enabled = !flag;
		}
		this.guiQuestSkipWindow.slider.value = (float)this.selectedItemNum;
		this.guiQuestSkipWindow.slider.onValueChanged.AddListener(delegate(float slider)
		{
			this.selectedItemNum = int.Parse(slider.ToString());
			this.guiQuestSkipWindow.SetButton(this.selectedItemNum);
			this.guiQuestSkipWindow.excText.text = this.selectedItemNum.ToString() + " / " + maxNum.ToString();
			int num4 = needStaminaNum * int.Parse(slider.ToString());
			this.guiQuestSkipWindow.numUseStaminaText.m_Text.color = ((num4 > nowStaminaNum) ? Color.red : Color.white);
			this.guiQuestSkipWindow.numUseStaminaText.text = num4.ToString();
		});
		this.guiQuestSkipWindow.buttonInc.AddOnClickListener(delegate(PguiButtonCtrl button)
		{
			int num5 = ((50 <= DataManager.DmItem.GetUserItemData(39000).num) ? 50 : DataManager.DmItem.GetUserItemData(39000).num);
			if (this.selectedItemNum < num5)
			{
				this.selectedItemNum++;
			}
			this.guiQuestSkipWindow.SetButton(this.selectedItemNum);
			this.guiQuestSkipWindow.excText.text = this.selectedItemNum.ToString() + " / " + maxNum.ToString();
			int num6 = needStaminaNum * this.selectedItemNum;
			this.guiQuestSkipWindow.numUseStaminaText.text = num6.ToString();
			this.guiQuestSkipWindow.numUseStaminaText.m_Text.color = ((num6 > nowStaminaNum) ? Color.red : Color.white);
			this.guiQuestSkipWindow.slider.value = (float)this.selectedItemNum;
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.guiQuestSkipWindow.buttonDec.AddOnClickListener(delegate(PguiButtonCtrl button)
		{
			if (1 < this.selectedItemNum)
			{
				this.selectedItemNum--;
			}
			this.guiQuestSkipWindow.SetButton(this.selectedItemNum);
			this.guiQuestSkipWindow.excText.text = this.selectedItemNum.ToString() + " / " + maxNum.ToString();
			int num7 = needStaminaNum * this.selectedItemNum;
			this.guiQuestSkipWindow.numUseStaminaText.text = num7.ToString();
			this.guiQuestSkipWindow.numUseStaminaText.m_Text.color = ((num7 > nowStaminaNum) ? Color.red : Color.white);
			this.guiQuestSkipWindow.slider.value = (float)this.selectedItemNum;
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.guiQuestSkipWindow.window.SetupByQuestSkip(new PguiOpenWindowCtrl.Callback(this.okCallBack));
		this.guiQuestSkipWindow.window.Open();
		this.isOpenWindow = true;
	}

	// Token: 0x06000EB1 RID: 3761 RVA: 0x000B33E8 File Offset: 0x000B15E8
	private IEnumerator DispQuestTabObj()
	{
		this.guiQuestSkipWindow.messageText.m_Text.color = Color.clear;
		this.guiQuestSkipWindow.questTab.SetActive(false);
		while (!this.guiQuestSkipWindow.window.FinishedOpen())
		{
			yield return null;
		}
		Canvas.ForceUpdateCanvases();
		Text text = this.guiQuestSkipWindow.messageText.m_Text;
		IList<UIVertex> verts = text.cachedTextGenerator.verts;
		int num = (text.text.IndexOf("同一クエストタブ") + 3) * 4;
		UIVertex uivertex = verts[num];
		UIVertex uivertex2 = verts[num + 2];
		uivertex.position /= text.pixelsPerUnit;
		uivertex2.position /= text.pixelsPerUnit;
		Vector3 vector = (uivertex.position + uivertex2.position) / 2f;
		this.guiQuestSkipWindow.questTab.GetComponent<RectTransform>().anchoredPosition = vector;
		this.guiQuestSkipWindow.messageText.m_Text.color = Color.red;
		this.guiQuestSkipWindow.questTab.SetActive(true);
		yield break;
	}

	// Token: 0x06000EB2 RID: 3762 RVA: 0x000B33F7 File Offset: 0x000B15F7
	public bool okCallBack(int index)
	{
		if (index == 2)
		{
			this.skipCoroutine = this.SkipStart();
		}
		return this.skipCoroutine == null;
	}

	// Token: 0x06000EB3 RID: 3763 RVA: 0x000B3414 File Offset: 0x000B1614
	public void SetReturnSkipCountAction(UnityAction<int> action)
	{
		this.returnSkipCountAction = action;
	}

	// Token: 0x06000EB4 RID: 3764 RVA: 0x000B341D File Offset: 0x000B161D
	public IEnumerator SkipStart()
	{
		QuestOnePackData qopd = DataManager.DmQuest.GetQuestOnePackData(this.selectedQuestOneId);
		DataManagerMonthlyPack.PurchaseMonthlypackData validMonthlyPackData = DataManager.DmMonthlyPack.GetValidMonthlyPackData();
		QuestUtil.UsrQuestSkipInfo usrQuestSkipInfo = QuestUtil.GetSkipInfo(validMonthlyPackData, qopd);
		StaminaRecoveryWindowCtrl staminawindow = CanvasManager.HdlStaminaRecoveryWindowCtrl;
		if (usrQuestSkipInfo.hasSkipLimit && usrQuestSkipInfo.restSkipCount == 0)
		{
			IEnumerator checkSkip = staminawindow.SkipRecoveryAction(this.selectedQuestOneId, validMonthlyPackData, qopd);
			while (checkSkip.MoveNext())
			{
				yield return null;
			}
			usrQuestSkipInfo = QuestUtil.GetSkipInfo(DataManager.DmMonthlyPack.GetValidMonthlyPackData(), qopd);
			if (usrQuestSkipInfo.restSkipCount == 0 || this.selectedItemNum <= 0)
			{
				this.InitializeWindow(this.selectedQuestOneId, this.helperPackData, this.helperAttrIndex, this.currentDeckId, 0);
				yield break;
			}
			checkSkip = null;
		}
		int beforeGold = DataManager.DmItem.GetUserItemData(30101).num;
		int nowStamina = DataManager.DmUserInfo.staminaInfo.GetInfoByNow(TimeManager.Now).stackNum;
		IEnumerator checkStamina = staminawindow.StaminaCheckAction(this.selectedQuestOneId, this.selectedItemNum);
		while (checkStamina.MoveNext())
		{
			yield return null;
		}
		int stackNum = DataManager.DmUserInfo.staminaInfo.GetInfoByNow(TimeManager.Now).stackNum;
		if (nowStamina == stackNum && nowStamina < this.selectedItemNum * qopd.questOne.stamina)
		{
			this.guiQuestSkipWindow.window.ForceClose();
			yield break;
		}
		if (nowStamina != stackNum)
		{
			this.guiQuestSkipWindow.window.ForceClose();
			this.InitializeWindow(this.selectedQuestOneId, this.helperPackData, this.helperAttrIndex, this.currentDeckId, this.selectedItemNum);
			yield break;
		}
		if (stackNum >= this.selectedItemNum * qopd.questOne.stamina)
		{
			IEnumerator checkSkip = this.InitializeResult(beforeGold);
			CanvasManager.HdlLoadAndTipsCtrl.SetDispLoading(true);
			while (checkSkip.MoveNext())
			{
				yield return null;
			}
			CanvasManager.HdlLoadAndTipsCtrl.SetDispLoading(false);
			this.guiQuestSkipWindow.window.ForceClose();
			UnityAction<int> unityAction = this.returnSkipCountAction;
			if (unityAction != null)
			{
				unityAction(this.selectedItemNum);
			}
			this.returnSkipCountAction = null;
			checkSkip = null;
		}
		yield break;
	}

	// Token: 0x06000EB5 RID: 3765 RVA: 0x000B342C File Offset: 0x000B162C
	public void RequestExecQuestSkip()
	{
		this.ExecQuestSkip();
	}

	// Token: 0x06000EB6 RID: 3766 RVA: 0x000B3434 File Offset: 0x000B1634
	private void ExecQuestSkip()
	{
		this.resultArgs.resultField = new GameObject("FieldSceneBattleResult");
		SceneManager.Add3DObjectByBaseField(this.resultArgs.resultField.transform);
		SceneBattleArgs sceneBattleArgs = new SceneBattleArgs();
		sceneBattleArgs.oppUser = null;
		sceneBattleArgs.difficulty = PvpDynamicData.EnemyInfo.Difficulty.INVALID;
		sceneBattleArgs.questOneId = this.selectedQuestOneId;
		sceneBattleArgs.waveEnemiesIdList = DataManager.DmQuest.LastQuestStartResponse.waveEnemiesIdList;
		sceneBattleArgs.startTime = DataManager.DmQuest.LastQuestStartResponse.startTime;
		QuestOnePackData qopd = DataManager.DmQuest.GetQuestOnePackData(this.selectedQuestOneId);
		DataManagerEvent.EventData eventData = ((qopd == null) ? null : DataManager.DmEvent.GetEventDataList().Find((DataManagerEvent.EventData itm) => itm.eventChapterId == qopd.questChapter.chapterId));
		sceneBattleArgs.eventId = ((eventData == null) ? 0 : eventData.eventId);
		sceneBattleArgs.selectDeckId = this.currentDeckId;
		sceneBattleArgs.helper = null;
		sceneBattleArgs.attrIndex = this.helperAttrIndex;
		this.resultArgs.battleArgs = sceneBattleArgs;
		int num = DataManager.DmDeck.GetUserDeckById(this.currentDeckId).CalcDeckKemoStatusWithPhoto(false, 0);
		UserOptionData userOptionData = DataManager.DmUserInfo.optionData.Clone();
		userOptionData.CurrentQuestParty = this.currentDeckId;
		DataManager.DmUserInfo.RequestActionUpdateUserOption(userOptionData);
		DataManager.DmQuest.RequestActionQuestSkip(this.selectedQuestOneId, this.currentDeckId, (this.isEventQuest && this.helperPackData != null) ? this.helperPackData.friendId : 0, this.helperCharaId, this.selectedItemNum, num, this.helperPhotoIdList);
	}

	// Token: 0x06000EB7 RID: 3767 RVA: 0x000B35C0 File Offset: 0x000B17C0
	public void GoNextScene(List<DrewItem> drew_items)
	{
		SceneBattle_QuestInfo sceneBattle_QuestInfo = new SceneBattle_QuestInfo();
		sceneBattle_QuestInfo.MakeQuestSkipInfo(this.selectedQuestOneId, drew_items);
		this.resultArgs.quest = sceneBattle_QuestInfo;
		this.resultArgs.battleArgs.dropItemList = drew_items;
		this.resultFriends.SetModelActive(true);
		this.resultFriends.SetAlpha(1f);
		this.resultFriends.shadowSize = 0.3f;
		this.resultFriends.transform.position = new Vector3(0f, -10f, 0f);
		this.resultFriends.transform.SetParent(this.resultArgs.resultField.transform, false);
		int num = LayerMask.NameToLayer("AuthMain");
		this.resultFriends.SetLayer(num);
		Light light = new GameObject("resultLight").AddComponent<Light>();
		light.transform.SetParent(this.resultArgs.resultField.transform, false);
		light.cullingMask = (1 << num) | (1 << LayerMask.NameToLayer("Bloom"));
		light.type = LightType.Directional;
		this.resultCamera = new GameObject("Result Camera", new Type[] { typeof(Camera) }).AddComponent<FieldCameraScaler>();
		this.resultCamera.transform.SetParent(this.resultArgs.resultField.transform, false);
		this.resultCamera.fieldCamera.clearFlags = CameraClearFlags.Depth;
		this.resultCamera.fieldCamera.backgroundColor = Color.clear;
		this.resultCamera.fieldCamera.depth = (float)((SceneManager.CameraDepth[SceneManager.CanvasType.BACK] + SceneManager.CameraDepth[SceneManager.CanvasType.FRONT]) / 2);
		this.resultCamera.fieldCamera.allowHDR = false;
		this.resultCamera.gameObject.layer = num;
		this.resultCamera.fieldCamera.cullingMask = (1 << num) | (1 << LayerMask.NameToLayer("Bloom"));
		this.resultCamera.gameObject.AddComponent<MultipleGaussianBloom>().gaussianFilter = MultipleGaussianBloom.FilterTaps._5Taps;
		CanvasManager.HdlMissionProgressCtrl.IsAfterQuestSkip = true;
		Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneBattleResult, this.resultArgs);
	}

	// Token: 0x06000EB8 RID: 3768 RVA: 0x000B37E5 File Offset: 0x000B19E5
	private IEnumerator InitializeResult(int beforeGold)
	{
		this.resultArgs = new SceneBattleResultArgs();
		this.resultArgs.tryCount = this.selectedItemNum;
		this.resultArgs.isSkip = true;
		this.resultArgs.helper = this.helperPackData;
		this.resultArgs.pvpDeck = null;
		this.resultArgs.deck = new SceneBattle_DeckInfo(this.currentDeckId, this.isEventQuest ? this.helperPackData : null, this.isEventQuest ? this.helperAttrIndex : 0);
		this.resultArgs.userLevel = DataManager.DmUserInfo.level;
		this.resultArgs.userExp = DataManager.DmUserInfo.exp;
		this.resultArgs.charaLevel = new List<int>();
		this.resultArgs.charaExp = new List<long>();
		this.resultArgs.kizunaLevel = new List<int>();
		this.resultArgs.kizunaExp = new List<long>();
		this.resultArgs.haveGoldNum = beforeGold;
		foreach (CharaPackData charaPackData2 in this.resultArgs.deck.deckData)
		{
			this.resultArgs.charaLevel.Add((charaPackData2 == null) ? 0 : charaPackData2.dynamicData.level);
			this.resultArgs.charaExp.Add((charaPackData2 == null) ? 0L : charaPackData2.dynamicData.exp);
			this.resultArgs.kizunaLevel.Add((charaPackData2 == null) ? 0 : charaPackData2.dynamicData.kizunaLevel);
			this.resultArgs.kizunaExp.Add((charaPackData2 == null) ? 0L : charaPackData2.dynamicData.kizunaExp);
		}
		this.resultArgs.battleEndStatus = DataManagerQuest.BattleEndStatus.INVALID;
		this.resultArgs.battleMissionStatus = new List<bool>();
		this.resultArgs.resultField = null;
		this.resultArgs.resultVoiceFirst = "";
		this.resultArgs.resultVoiceFirstSheet = "";
		this.resultArgs.resultVoiceFirstLength = 0f;
		this.resultArgs.resultVoiceSecond = "";
		this.resultArgs.resultVoiceSecondSheet = "";
		this.resultArgs.resultVoiceSecondTime = TimeManager.SystemNow;
		this.resultArgs.clearTurn = 0;
		this.resultArgs.trainingRevive = 0;
		this.resultArgs.trainingScore = 0L;
		this.resultFriends = new GameObject("resultFriends", new Type[] { typeof(CharaModelHandle) }).GetComponent<CharaModelHandle>();
		Random random = new Random();
		UserDeckData userDeckById = DataManager.DmDeck.GetUserDeckById(this.currentDeckId);
		int num = random.Next(0, userDeckById.charaIdList.Count);
		CharaPackData charaPackData = this.resultArgs.deck.deckData[num];
		if (charaPackData == null)
		{
			num = this.resultArgs.deck.deckData.FindIndex((CharaPackData chara) => chara != null);
			charaPackData = this.resultArgs.deck.deckData[num];
		}
		int num2 = charaPackData.equipClothImageId;
		if (num2 > 0 && num == this.resultArgs.deck.deckHelperIndex && !DataManager.DmUserInfo.optionData.ViewClothesAffect)
		{
			num2 = 0;
		}
		this.resultFriends.Initialize(charaPackData.id, true, false, num2, num2 > 0 && charaPackData.equipLongSkirt, true, false, null);
		while (!this.resultFriends.IsFinishInitialize())
		{
			yield return null;
		}
		if (DataManager.DmChara.GetCharaStaticData(charaPackData.id).baseData.isFloating)
		{
			this.resultFriends.PlayAnimation(CharaMotionDefine.ActKey.WIN_ST, false, 1f, 0f, 0f, false);
		}
		this.resultFriends.SetModelActive(false);
		yield break;
	}

	// Token: 0x04000D58 RID: 3416
	private QuestSkipWindowsCtrl.QuestSkipWindow guiQuestSkipWindow;

	// Token: 0x04000D59 RID: 3417
	private int selectedItemNum;

	// Token: 0x04000D5A RID: 3418
	private int selectedQuestOneId;

	// Token: 0x04000D5B RID: 3419
	private int selectedDeckId = 1;

	// Token: 0x04000D5C RID: 3420
	private HelperPackData helperPackData;

	// Token: 0x04000D5D RID: 3421
	private int helperAttrIndex;

	// Token: 0x04000D5E RID: 3422
	private int helperCharaId;

	// Token: 0x04000D5F RID: 3423
	private List<long> helperPhotoIdList;

	// Token: 0x04000D60 RID: 3424
	private bool isEventQuest;

	// Token: 0x04000D61 RID: 3425
	private int currentDeckId;

	// Token: 0x04000D62 RID: 3426
	private int maxNum = 1;

	// Token: 0x04000D63 RID: 3427
	private const int ConstUseItemNumMin = 1;

	// Token: 0x04000D64 RID: 3428
	private const int ConstUseItemNumMax = 50;

	// Token: 0x04000D65 RID: 3429
	private const int skipTicketItemID = 39000;

	// Token: 0x04000D66 RID: 3430
	private SceneBattleResultArgs resultArgs;

	// Token: 0x04000D67 RID: 3431
	private CharaModelHandle resultFriends;

	// Token: 0x04000D68 RID: 3432
	private FieldCameraScaler resultCamera;

	// Token: 0x04000D69 RID: 3433
	private IEnumerator skipCoroutine;

	// Token: 0x04000D6A RID: 3434
	private UnityAction<int> returnSkipCountAction;

	// Token: 0x04000D6B RID: 3435
	private static readonly Color DisableColor = new Color(0.7f, 0.7f, 0.7f, 1f);

	// Token: 0x04000D6C RID: 3436
	private bool isOpenWindow;

	// Token: 0x02000912 RID: 2322
	public class QuestSkipWindow
	{
		// Token: 0x06003AA2 RID: 15010 RVA: 0x001D011C File Offset: 0x001CE31C
		public QuestSkipWindow(Transform baseTr)
		{
			this.window = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.baseObj = baseTr.gameObject;
			this.messageText = baseTr.Find("Base/Window/Base_UseInfo/Txt01").gameObject.GetComponent<PguiTextCtrl>();
			this.questTab = baseTr.Find("Base/Window/Base_UseInfo/Txt01/QuestTab").gameObject;
			this.iconItemNestPrefab = baseTr.Find("Base/Window/Base_UseInfo/Icon_Item").GetComponent<PguiNestPrefab>();
			this.iconItemNestPrefab.InitForce();
			this.buttonClose = baseTr.Find("Base/Window/Window_BtnClose").GetComponent<PguiButtonCtrl>();
			this.buttonInc = baseTr.Find("Base/Window/Base_UseInfo/Exchange/Btn_Plus").gameObject.GetComponent<PguiButtonCtrl>();
			this.buttonDec = baseTr.Find("Base/Window/Base_UseInfo/Exchange/Btn_Minus").gameObject.GetComponent<PguiButtonCtrl>();
			this.buttonOK = baseTr.Find("Base/Window/BtnOk").GetComponent<PguiButtonCtrl>();
			this.numBeforeStaminaText = baseTr.Find("Base/Window/Base_UseInfo/Parts_ItemUseCoin/Num_BeforeTxt").gameObject.GetComponent<PguiTextCtrl>();
			this.numUseStaminaText = baseTr.Find("Base/Window/Base_UseInfo/Parts_ItemUseCoin/Num_AfterTxt").gameObject.GetComponent<PguiTextCtrl>();
			this.excText = baseTr.Find("Base/Window/Base_UseInfo/Exchange/Tex/Num_Txt").gameObject.GetComponent<PguiTextCtrl>();
			this.haveNumText = baseTr.Find("Base/Window/Base_UseInfo/HaveNum_Txt").gameObject.GetComponent<PguiTextCtrl>();
			this.slider = baseTr.Find("Base/Window/Base_UseInfo/SliderBar").GetComponent<Slider>();
			this.sliderObj = baseTr.Find("Base/Window/Base_UseInfo/SliderBar").gameObject;
			this.sliderObjImage = this.sliderObj.transform.Find("Handle Slide Area/Handle/Image").gameObject;
		}

		// Token: 0x06003AA3 RID: 15011 RVA: 0x001D02B4 File Offset: 0x001CE4B4
		public void SetButton(int num)
		{
			this.buttonInc.SetActEnable(this.maxNum > num, false, false);
			this.buttonDec.SetActEnable(1 < num, false, false);
			this.buttonOK.SetActEnable(DataManager.DmItem.GetUserItemData(39000).num != 0, false, false);
		}

		// Token: 0x04003B57 RID: 15191
		public GameObject baseObj;

		// Token: 0x04003B58 RID: 15192
		public PguiOpenWindowCtrl window;

		// Token: 0x04003B59 RID: 15193
		public PguiTextCtrl messageText;

		// Token: 0x04003B5A RID: 15194
		public GameObject questTab;

		// Token: 0x04003B5B RID: 15195
		public PguiNestPrefab iconItemNestPrefab;

		// Token: 0x04003B5C RID: 15196
		public PguiButtonCtrl buttonClose;

		// Token: 0x04003B5D RID: 15197
		public PguiButtonCtrl buttonInc;

		// Token: 0x04003B5E RID: 15198
		public PguiButtonCtrl buttonDec;

		// Token: 0x04003B5F RID: 15199
		public PguiButtonCtrl buttonOK;

		// Token: 0x04003B60 RID: 15200
		public PguiTextCtrl numBeforeStaminaText;

		// Token: 0x04003B61 RID: 15201
		public PguiTextCtrl numUseStaminaText;

		// Token: 0x04003B62 RID: 15202
		public PguiTextCtrl excText;

		// Token: 0x04003B63 RID: 15203
		public PguiTextCtrl haveNumText;

		// Token: 0x04003B64 RID: 15204
		public int maxNum;

		// Token: 0x04003B65 RID: 15205
		public Slider slider;

		// Token: 0x04003B66 RID: 15206
		public GameObject sliderObj;

		// Token: 0x04003B67 RID: 15207
		public GameObject sliderObjImage;
	}
}
