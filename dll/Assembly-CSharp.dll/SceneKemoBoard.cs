using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.Touch;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000151 RID: 337
public class SceneKemoBoard : BaseScene
{
	// Token: 0x0600133C RID: 4924 RVA: 0x000ECE58 File Offset: 0x000EB058
	public override void OnCreateScene()
	{
		this.guiData = new SceneKemoBoard.GUI(AssetManager.InstantiateAssetData("SceneCharaEdit/GUI/Prefab/GUI_KemoBoard", null).transform);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.FRONT, this.guiData.baseObj.transform, true);
		this.winPanel = AssetManager.InstantiateAssetData("SceneCharaEdit/GUI/Prefab/GUI_KemoBoard_Window", null);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, this.winPanel.transform, true);
		this.winReset = new SceneKemoBoard.WIN_RESET(this.winPanel.transform.Find("Window_ResetWindow"));
		this.winCheck = new SceneKemoBoard.WIN_CHECK(this.winPanel.transform.Find("Window_ResetCheck"));
		Transform transform = new GameObject("line").transform;
		transform.SetParent(this.guiData.Board, false);
		Transform transform2 = new GameObject("point").transform;
		transform2.SetParent(this.guiData.Board, false);
		this.iconStart = AssetManager.InstantiateAssetData("SceneCharaEdit/GUI/Prefab/KemoBoard_Icon_Start", this.guiData.Board);
		this.iconStart.name = "start";
		this.iconResult = AssetManager.InstantiateAssetData("SceneCharaEdit/GUI/Prefab/KemoBoard_Result_Up", this.guiData.baseObj.transform);
		this.iconResult.name = "result";
		this.iconResult.SetActive(false);
		this.iconYaji = AssetManager.InstantiateAssetData("SceneCharaEdit/GUI/Prefab/KemoBoard_CurrentYaji", this.guiData.Board);
		this.iconYaji.name = "yaji";
		this.iconYaji.SetActive(false);
		this.guiData.BtnReset.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.BtnShop.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.BtnOpen.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickOpen), PguiButtonCtrl.SoundType.DECIDE);
		this.guiData.BtnCancel.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickCancel), PguiButtonCtrl.SoundType.CANCEL);
		foreach (PguiButtonCtrl pguiButtonCtrl in this.winReset.BtnReset)
		{
			pguiButtonCtrl.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickReset), PguiButtonCtrl.SoundType.DEFAULT);
		}
		this.winReset.win.Setup(null, null, null, true, null, null, false);
		this.winCheck.win.SetupKemoBoardResetCheck(new PguiOpenWindowCtrl.Callback(this.OnClickResetCheck));
		this.scrMin = Vector2.zero;
		this.scrMin = Vector2.zero;
		float num = 130f;
		this.kemoData = new Dictionary<CharaDef.AttributeType, Dictionary<int, KeyValuePair<DataManagerKemoBoard.KemoBoardPanelData, KeyValuePair<Transform, RectTransform>>>>();
		using (List<DataManagerKemoBoard.KemoBoardPanelData>.Enumerator enumerator2 = DataManager.DmKemoBoard.KemoBoardPanelDataList.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				DataManagerKemoBoard.KemoBoardPanelData kd = enumerator2.Current;
				if (kd.Id <= 1 || kd.ParentId > 0)
				{
					string text = ((kd.UseItemList.Find((ItemInput itm) => itm.itemId >= 15001 && itm.itemId <= 15006) == null) ? "KemoBoard_Icon_Point" : "KemoBoard_IconStar_Point");
					Transform transform3 = AssetManager.InstantiateAssetData("SceneCharaEdit/GUI/Prefab/" + text, transform2).transform;
					transform3.name = kd.Attr.ToString() + "-" + kd.Id.ToString();
					RectTransform rectTransform = ((kd.ParentId > 0) ? AssetManager.InstantiateAssetData("SceneCharaEdit/GUI/Prefab/KemoBoard_Line", transform).GetComponent<RectTransform>() : null);
					if (rectTransform != null)
					{
						rectTransform.name = string.Concat(new string[]
						{
							kd.Attr.ToString(),
							"-",
							kd.ParentId.ToString(),
							"-",
							kd.Id.ToString()
						});
					}
					int num2 = kd.Id;
					int i;
					for (i = 1; i < num2; i++)
					{
						num2 -= i;
					}
					float num3 = 0f;
					float num4 = 0f;
					if (--i > 0)
					{
						num3 = num * (float)i;
						num4 = -num3 * 0.5f;
						num4 -= num4 * 2f / (float)i * (float)(num2 - 1);
					}
					num3 *= 0.866f;
					num3 += num;
					float num5 = 0f;
					if (kd.AreaId == 1)
					{
						num5 = 60f;
					}
					else if (kd.AreaId == 2)
					{
						num5 = -60f;
					}
					else if (kd.AreaId == 4)
					{
						num5 = -120f;
					}
					else if (kd.AreaId == 5)
					{
						num5 = 120f;
					}
					else if (kd.AreaId == 6)
					{
						num5 = 180f;
					}
					num5 *= 0.017453292f;
					float num6 = Mathf.Sin(num5);
					float num7 = Mathf.Cos(num5);
					Vector3 vector = new Vector3(num3 * num7 + num4 * num6, num3 * num6 - num4 * num7, 0f);
					if (this.scrMin.x > vector.x)
					{
						this.scrMin.x = vector.x;
					}
					if (this.scrMin.y > vector.y)
					{
						this.scrMin.y = vector.y;
					}
					if (this.scrMax.x < vector.x)
					{
						this.scrMax.x = vector.x;
					}
					if (this.scrMax.y < vector.y)
					{
						this.scrMax.y = vector.y;
					}
					transform3.localPosition = vector;
					transform3.Find("None").GetComponent<PguiReplaceSpriteCtrl>().Replace(kd.AreaId);
					transform3.Find("Act/Ball").GetComponent<PguiReplaceSpriteCtrl>().Replace(kd.AreaId);
					int bonusIcon = this.GetBonusIcon(kd);
					Transform transform4 = transform3.Find("None/Icon_Kind");
					transform4.gameObject.SetActive(bonusIcon > 0);
					transform4.GetComponent<PguiReplaceSpriteCtrl>().Replace(bonusIcon);
					Transform transform5 = transform3.Find("Act/Ball/Icon_Kind");
					transform5.gameObject.SetActive(bonusIcon > 0);
					transform5.GetComponent<PguiReplaceSpriteCtrl>().Replace(bonusIcon);
					if (!this.kemoData.ContainsKey(kd.Attr))
					{
						this.kemoData.Add(kd.Attr, new Dictionary<int, KeyValuePair<DataManagerKemoBoard.KemoBoardPanelData, KeyValuePair<Transform, RectTransform>>>());
					}
					this.kemoData[kd.Attr].Add(kd.Id, new KeyValuePair<DataManagerKemoBoard.KemoBoardPanelData, KeyValuePair<Transform, RectTransform>>(kd, new KeyValuePair<Transform, RectTransform>(transform3, rectTransform)));
					transform3.Find("None").gameObject.AddComponent<PguiTouchTrigger>().AddListener(delegate
					{
						this.OnClickPoint(kd);
					}, null, null, delegate
					{
						this.OnClickPoint(true);
					}, delegate
					{
						this.OnClickPoint(false);
					});
				}
			}
		}
		this.scrMin -= Vector2.one * 38f;
		this.scrMax += Vector2.one * 38f;
		foreach (CharaDef.AttributeType attributeType in this.kemoData.Keys)
		{
			foreach (int num8 in this.kemoData[attributeType].Keys)
			{
				int parentId = this.kemoData[attributeType][num8].Key.ParentId;
				Vector3 localPosition = this.kemoData[attributeType][num8].Value.Key.localPosition;
				Vector3 vector2 = ((parentId > 0) ? ((num8 != parentId && this.kemoData[attributeType].ContainsKey(parentId)) ? this.kemoData[attributeType][parentId].Value.Key.localPosition : (localPosition + new Vector3(1f, 1f, 0f))) : this.iconStart.transform.localPosition);
				RectTransform value = this.kemoData[attributeType][num8].Value.Value;
				if (value != null)
				{
					value.localPosition = (localPosition + vector2) * 0.5f;
					Vector3 vector3 = vector2 - localPosition;
					value.localEulerAngles = new Vector3(0f, 0f, Mathf.Atan2(vector3.y, vector3.x) * 57.29578f + 90f);
					value.sizeDelta = new Vector2(value.sizeDelta.x, vector3.magnitude);
				}
			}
		}
		this.guiData.BtnShop.transform.Find("BaseImage/Icon_Stone").GetComponent<PguiRawImageCtrl>().SetRawImage(DataManager.DmItem.GetItemStaticBase(30133).GetIconName(), true, false, null);
		this.guiData.baseObj.SetActive(false);
		this.winPanel.SetActive(false);
	}

	// Token: 0x0600133D RID: 4925 RVA: 0x000ED8B8 File Offset: 0x000EBAB8
	private int GetBonusIcon(DataManagerKemoBoard.KemoBoardPanelData kd)
	{
		int num = kd.bonusTypeNum;
		if (num == 10)
		{
			ItemStaticBase itemStaticBase = DataManager.DmItem.GetItemStaticBase(kd.BonusItemId);
			if (itemStaticBase == null)
			{
				num = 0;
			}
			else if (itemStaticBase.GetKind() == ItemDef.Kind.CHARA)
			{
				num = 10;
			}
			else if (itemStaticBase.GetKind() == ItemDef.Kind.PHOTO)
			{
				num = 11;
			}
			else
			{
				num = 12;
			}
		}
		else if (num < 1 || num > 7)
		{
			num = 0;
		}
		return num;
	}

	// Token: 0x0600133E RID: 4926 RVA: 0x000ED918 File Offset: 0x000EBB18
	public override void OnEnableScene(object args)
	{
		this.guiData.baseObj.SetActive(true);
		this.winPanel.SetActive(false);
		this.winReset.win.ForceClose();
		this.winCheck.win.ForceClose();
		CanvasManager.HdlCmnMenu.SetupMenu(true, "探検！迷宮ゾーン", new PguiCmnMenuCtrl.OnClickReturnButton(this.OnClickButtonRetrun), "", new PguiCmnMenuCtrl.OnClickMoveSequenceButton(this.OnClickButtonMenu), null);
		CanvasManager.SetBgTexture(null);
		SoundManager.PlayBGM("prd_bgm0003");
		this.ienum = (DataManager.DmGameStatus.MakeUserFlagData().TutorialFinishFlag.KemoBoardFirst ? null : this.FirstTutorial());
		this.guiData.WinOpen.ExPauseAnimationLastFrame(SimpleAnimation.ExPguiStatus.END);
		this.guiData.WinOpen.gameObject.SetActive(false);
		Manager.RegisterStart(new Manager.SingleAction(this.OnTouchStart));
		Manager.RegisterRelease(new Manager.SingleAction(this.OnTouchEnd));
		Manager.RegisterMove(new Manager.SingleAction(this.OnTouchMove));
		Manager.RegisterPinch(new Manager.DoubleAction(this.OnPinch));
		Manager.RegisterMouseWheel(new Manager.WheelAction(this.OnWheel));
		this.guiData.Board.localPosition = Vector3.zero;
		this.guiData.Board.localScale = Vector3.one;
		this.boardScroll = false;
		this.moveBoard = Vector2.zero;
		this.pinchBoard = 0f;
		this.pinchScl = this.guiData.Board.localScale.x;
		this.wheelBoard = 0f;
		this.iconResult.SetActive(false);
		this.iconYaji.SetActive(false);
		this.guiData.BtnReset.gameObject.SetActive(true);
		this.guiData.BtnShop.gameObject.SetActive(true);
		this.selPoint = null;
		this.reset = null;
		DataManager.DmKemoBoard.RequestGetKemoBoard();
	}

	// Token: 0x0600133F RID: 4927 RVA: 0x000EDB0C File Offset: 0x000EBD0C
	public override bool OnEnableSceneWait()
	{
		if (DataManager.IsServerRequesting())
		{
			return false;
		}
		foreach (CharaDef.AttributeType attributeType in this.kemoData.Keys)
		{
			this.DispKemoData(attributeType);
		}
		GameObject gameObject = AssetManager.InstantiateAssetData("Cmn/GUI/Prefab/GUI_Cmn_StaminaUseKirakira", null);
		SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.SYSTEM, gameObject.transform, true);
		this.useItem = new StaminaRecoveryWindowCtrl.GUI_StaminaRecoveryExecuteWindow(gameObject.transform);
		this.useItem.window.WindowRectTransform.Find("Icon_Item").GetComponent<PguiNestPrefab>().InitForce();
		this.useItem.numBeforeStaminaText.transform.parent.gameObject.SetActive(false);
		this.useItem.window.SetupByStaminaUse(new PguiOpenWindowCtrl.Callback(this.OnClickResetOk));
		this.useItem.buttonClose.androidBackKeyTarget = true;
		this.useItem.baseObj.SetActive(false);
		return true;
	}

	// Token: 0x06001340 RID: 4928 RVA: 0x000EDC1C File Offset: 0x000EBE1C
	private void DispKemoData(CharaDef.AttributeType attr)
	{
		HashSet<int> hashSet = (DataManager.DmKemoBoard.ReleaseKamoPanelMap.ContainsKey(attr) ? DataManager.DmKemoBoard.ReleaseKamoPanelMap[attr] : new HashSet<int>());
		bool flag = false;
		foreach (int num in this.kemoData[attr].Keys)
		{
			Transform transform = this.kemoData[attr][num].Value.Key.Find("Act");
			transform.gameObject.SetActive(!transform.Find("Ball/Icon_Kind").gameObject.activeSelf || hashSet.Contains(num));
			transform.Find("AEImage_Act_Back").GetComponent<PguiAECtrl>().PauseAnimationLastFrame(PguiAECtrl.AmimeType.START);
			transform.Find("AEImage_Act_Front").GetComponent<PguiAECtrl>().PauseAnimationLastFrame(PguiAECtrl.AmimeType.START);
			if (hashSet.Contains(num))
			{
				DataManagerKemoBoard.KemoBoardPanelData key = this.kemoData[attr][num].Key;
				if (key.bonusTypeNum >= 1 && key.bonusTypeNum <= 7)
				{
					flag = true;
				}
			}
		}
		this.winReset.BtnReset[attr - CharaDef.AttributeType.RED].SetActEnable(flag, false, false);
		foreach (int num2 in this.kemoData[attr].Keys)
		{
			bool activeSelf = this.kemoData[attr][num2].Value.Key.Find("Act").gameObject.activeSelf;
			bool flag2 = true;
			DataManagerKemoBoard.KemoBoardPanelData key2 = this.kemoData[attr][num2].Key;
			int num3 = key2.ParentId;
			while (num3 > 0 && flag2)
			{
				if (this.kemoData[attr].ContainsKey(num3))
				{
					if (this.kemoData[attr][num3].Value.Key.Find("Act").gameObject.activeSelf)
					{
						num3 = this.kemoData[attr][num3].Key.ParentId;
					}
					else
					{
						flag2 = false;
					}
				}
				else
				{
					flag2 = false;
				}
			}
			bool flag3 = flag2;
			int num4 = 0;
			while (flag3 && num4 < key2.UseItemList.Count)
			{
				int itemId = key2.UseItemList[num4].itemId;
				if (itemId != 0 && (!DataManager.DmItem.GetUserItemMap().ContainsKey(itemId) || DataManager.DmItem.GetUserItemMap()[itemId].num < key2.UseItemList[num4].num))
				{
					flag3 = false;
				}
				num4++;
			}
			Transform transform2 = this.kemoData[attr][num2].Value.Key;
			transform2.Find("Eff_Able").gameObject.SetActive(!activeSelf && flag2);
			transform2.Find("Eff_Able").GetComponent<PguiImageCtrl>().m_Image.enabled = flag3;
			Transform transform3 = transform2.Find("None/Icon_Kind");
			transform3.GetComponent<PguiImageCtrl>().m_Image.color = transform3.GetComponent<PguiColorCtrl>().GetGameObjectById((!activeSelf && flag2 && flag3) ? "Able" : "Normal");
			transform2 = this.kemoData[attr][num2].Value.Value;
			if (transform2 != null)
			{
				transform2.Find("Act").gameObject.SetActive(activeSelf && flag2);
			}
		}
	}

	// Token: 0x06001341 RID: 4929 RVA: 0x000EE050 File Offset: 0x000EC250
	private IEnumerator FirstTutorial()
	{
		yield return null;
		bool isFinishWindow = false;
		CanvasManager.HdlCmnFeedPageWindowCtrl.Open(CmnFeedPageWindowCtrl.Type.PAGE_FEED, "", new List<string> { "Texture2D/Tutorial_Window/KemoBoard/tutorial_kemoboard_01" }, delegate(bool b)
		{
			isFinishWindow = true;
		});
		while (!isFinishWindow)
		{
			yield return null;
		}
		DataManagerGameStatus.UserFlagData userFlagData = DataManager.DmGameStatus.MakeUserFlagData();
		userFlagData.TutorialFinishFlag.KemoBoardFirst = true;
		DataManager.DmGameStatus.RequestActionUpdateUserFlag(userFlagData);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001342 RID: 4930 RVA: 0x000EE058 File Offset: 0x000EC258
	public override void Update()
	{
		if (this.winPanel.activeSelf && this.winReset.win.FinishedClose() && this.winCheck.win.FinishedClose())
		{
			this.winPanel.SetActive(false);
		}
		if (this.useItem.baseObj.activeSelf && this.useItem.window.FinishedClose())
		{
			this.useItem.baseObj.SetActive(false);
		}
		if (this.ienum != null && !this.ienum.MoveNext())
		{
			this.ienum = null;
		}
		if (this.pinchBoard == 0f)
		{
			this.pinchScl = this.guiData.Board.localScale.x;
		}
		float num = Mathf.Clamp(this.pinchScl + this.pinchBoard + this.wheelBoard, 1280f / (this.scrMax.x - this.scrMin.x), 2f);
		this.guiData.Board.localScale = Vector3.one * num;
		Vector3 vector = this.guiData.Board.localPosition + new Vector3(this.moveBoard.x, this.moveBoard.y, 0f);
		vector.x = Mathf.Clamp(vector.x, this.scrMin.x * this.guiData.Board.localScale.x + 640f, this.scrMax.x * this.guiData.Board.localScale.x - 640f);
		vector.y = Mathf.Clamp(vector.y, this.scrMin.y * this.guiData.Board.localScale.y - 72f + 360f, this.scrMax.y * this.guiData.Board.localScale.y - 360f);
		this.guiData.Board.localPosition = vector;
		this.pinchBoard = 0f;
		this.wheelBoard = 0f;
		this.moveBoard = Vector2.zero;
		CanvasManager.HdlCmnMenu.UpdateMenu(true, true);
	}

	// Token: 0x06001343 RID: 4931 RVA: 0x000EE2AC File Offset: 0x000EC4AC
	private void SetWinOpen(DataManagerKemoBoard.KemoBoardPanelData kd)
	{
		this.selPoint = kd;
		Transform key = this.kemoData[this.selPoint.Attr][this.selPoint.Id].Value.Key;
		this.iconYaji.SetActive(true);
		this.iconYaji.transform.SetParent(key, false);
		this.guiData.BtnReset.gameObject.SetActive(false);
		this.guiData.BtnShop.gameObject.SetActive(false);
		bool activeSelf = key.Find("Act").gameObject.activeSelf;
		bool activeSelf2 = key.Find("Eff_Able").gameObject.activeSelf;
		bool enabled = key.Find("Eff_Able").GetComponent<PguiImageCtrl>().m_Image.enabled;
		this.guiData.WinOpen.gameObject.SetActive(true);
		Transform transform = this.guiData.WinOpen.transform.Find("Window/All/KemoBoard_Icon_Point");
		Transform transform2 = this.guiData.WinOpen.transform.Find("Window/All/KemoBoard_IconStar_Point");
		Transform transform3 = ((this.selPoint.UseItemList.Find((ItemInput itm) => itm.itemId >= 15001 && itm.itemId <= 15006) == null) ? transform : transform2);
		transform.gameObject.SetActive(transform == transform3);
		transform2.gameObject.SetActive(transform2 == transform3);
		Transform transform4 = transform3.Find("None");
		transform4.GetComponent<PguiReplaceSpriteCtrl>().Replace(this.selPoint.AreaId);
		int bonusIcon = this.GetBonusIcon(this.selPoint);
		transform4 = transform4.Find("Icon_Kind");
		transform4.gameObject.SetActive(bonusIcon > 0);
		transform4.GetComponent<PguiReplaceSpriteCtrl>().Replace(bonusIcon);
		transform4.GetComponent<PguiImageCtrl>().m_Image.color = transform4.GetComponent<PguiColorCtrl>().GetGameObjectById((activeSelf2 && enabled) ? "Able" : "Normal");
		transform4 = transform3.Find("Act");
		transform4.Find("Ball").GetComponent<PguiReplaceSpriteCtrl>().Replace(this.selPoint.AreaId);
		transform4.gameObject.SetActive(activeSelf);
		transform4.Find("AEImage_Act_Back").GetComponent<PguiAECtrl>().PauseAnimationLastFrame(PguiAECtrl.AmimeType.START);
		transform4.Find("AEImage_Act_Front").GetComponent<PguiAECtrl>().PauseAnimationLastFrame(PguiAECtrl.AmimeType.START);
		transform4 = transform4.Find("Ball/Icon_Kind");
		transform4.gameObject.SetActive(bonusIcon > 0);
		transform4.GetComponent<PguiReplaceSpriteCtrl>().Replace(bonusIcon);
		transform4 = transform3.Find("Eff_Able");
		transform4.gameObject.SetActive(activeSelf2);
		transform4.GetComponent<PguiImageCtrl>().m_Image.enabled = enabled;
		Transform transform5 = this.guiData.WinOpen.transform.Find("Window/All/Info_Bonus/Icon_Item");
		transform5.gameObject.SetActive(bonusIcon >= 10);
		if (transform5.gameObject.activeSelf)
		{
			IconItemCtrl component = transform5.GetComponent<IconItemCtrl>();
			component.initialize();
			ItemStaticBase itemStaticBase = DataManager.DmItem.GetItemStaticBase(this.selPoint.BonusItemId);
			component.Setup(itemStaticBase, this.selPoint.BonusValue, new IconItemCtrl.SetupParam
			{
				useInfo = (bonusIcon >= 12),
				useMaxDetail = (bonusIcon < 12)
			});
			this.iconResult.transform.Find("Num_Result").GetComponent<PguiTextCtrl>().text = ((itemStaticBase == null) ? "" : (itemStaticBase.GetName() + "を" + ((this.selPoint.BonusValue > 1) ? (this.selPoint.BonusValue.ToString() + "個") : "") + "獲得"));
			this.iconResult.transform.Find("Num_Result/Icon_Atr").gameObject.SetActive(false);
		}
		transform5 = this.guiData.WinOpen.transform.Find("Window/All/Info_Bonus/Txt_Bonus");
		string text = "";
		if (bonusIcon == 1)
		{
			text = "たいりょく";
		}
		else if (bonusIcon == 2)
		{
			text = "こうげき";
		}
		else if (bonusIcon == 3)
		{
			text = "まもり";
		}
		else if (bonusIcon == 4)
		{
			text = "かいひ";
		}
		else if (bonusIcon == 5)
		{
			text = "Beat!!!";
		}
		else if (bonusIcon == 6)
		{
			text = "Action!";
		}
		else if (bonusIcon == 7)
		{
			text = "Try!!";
		}
		transform5.gameObject.SetActive(!string.IsNullOrEmpty(text));
		if (transform5.gameObject.activeSelf)
		{
			string text2;
			if (bonusIcon < 4)
			{
				text2 = this.selPoint.BonusValue.ToString();
			}
			else
			{
				text2 = (this.selPoint.BonusValue / 10).ToString();
				int num = this.selPoint.BonusValue % 10;
				if (num > 0)
				{
					text2 = text2 + "." + num.ToString();
				}
				text2 += "%";
			}
			transform5.GetComponent<PguiTextCtrl>().ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[] { text, text2 });
			transform5.Find("Icon_Atr").GetComponent<PguiReplaceSpriteCtrl>().Replace((int)this.selPoint.Attr);
			this.iconResult.transform.Find("Num_Result").GetComponent<PguiTextCtrl>().text = text + "+" + text2;
			this.iconResult.transform.Find("Num_Result/Icon_Atr").GetComponent<PguiReplaceSpriteCtrl>().Replace((int)this.selPoint.Attr);
			this.iconResult.transform.Find("Num_Result/Icon_Atr").gameObject.SetActive(true);
		}
		int num2 = 0;
		for (;;)
		{
			Transform transform6 = this.guiData.WinOpen.transform.Find("Window/All/Info_UseItem/Grid/ItemList_" + (num2 + 1).ToString("D2"));
			if (transform6 == null)
			{
				break;
			}
			transform6.gameObject.SetActive(this.selPoint.UseItemList.Count > num2 && this.selPoint.UseItemList[num2].itemId != 0);
			if (transform6.gameObject.activeSelf)
			{
				ItemData userItemData = DataManager.DmItem.GetUserItemData(this.selPoint.UseItemList[num2].itemId);
				transform6.Find("Icon_Item").GetComponent<IconItemCtrl>().Setup(userItemData.staticData, this.selPoint.UseItemList[num2].num, new IconItemCtrl.SetupParam
				{
					useInfo = true
				});
				transform6.Find("Txt_ItemName").GetComponent<PguiTextCtrl>().text = ((userItemData.staticData == null) ? "" : userItemData.staticData.GetName());
				transform6.Find("Txt_Own").GetComponent<PguiTextCtrl>().ReplaceTextByDefault("Param01", userItemData.num.ToString());
			}
			num2++;
		}
		text = "";
		if (activeSelf)
		{
			text = "解放済みです";
		}
		else if (!activeSelf2)
		{
			text = "まだ解放できません";
		}
		else if (!enabled)
		{
			text = "アイテムが足りません";
		}
		this.guiData.BtnOpen.SetActEnable(string.IsNullOrEmpty(text), false, false);
		this.guiData.BtnOpen.transform.Find("Txt_Caution").GetComponent<PguiTextCtrl>().text = text;
		if (!this.guiData.WinOpen.ExIsCurrent(SimpleAnimation.ExPguiStatus.START))
		{
			this.guiData.WinOpen.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
		}
	}

	// Token: 0x06001344 RID: 4932 RVA: 0x000EEA83 File Offset: 0x000ECC83
	private IEnumerator PointOpen()
	{
		while (this.guiData.WinOpen.gameObject.activeSelf)
		{
			yield return null;
		}
		if (this.selPoint == null)
		{
			yield break;
		}
		DataManager.DmKemoBoard.RequestOpenKemoBoard(this.selPoint.Attr, this.selPoint.Id);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		SoundManager.Play("prd_se_kemoboard_release", false, false);
		if (DataManager.DmKemoBoard.ReleaseKamoPanelMap.ContainsKey(this.selPoint.Attr) && DataManager.DmKemoBoard.ReleaseKamoPanelMap[this.selPoint.Attr].Contains(this.selPoint.Id))
		{
			Transform pnt = this.kemoData[this.selPoint.Attr][this.selPoint.Id].Value.Key;
			pnt.SetAsLastSibling();
			pnt.Find("Act").gameObject.SetActive(true);
			pnt.Find("Act/AEImage_Act_Back").GetComponent<PguiAECtrl>().PlayAnimation(PguiAECtrl.AmimeType.START, null);
			pnt.Find("Act/AEImage_Act_Front").GetComponent<PguiAECtrl>().PlayAnimation(PguiAECtrl.AmimeType.START, null);
			pnt.Find("Eff_Able").gameObject.SetActive(false);
			this.iconResult.SetActive(true);
			this.iconResult.GetComponent<SimpleAnimation>().ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
			while (!pnt.Find("Act/AEImage_Act_Front").GetComponent<PguiAECtrl>().m_AEImage.end)
			{
				yield return null;
			}
			pnt = null;
		}
		using (Dictionary<CharaDef.AttributeType, Dictionary<int, KeyValuePair<DataManagerKemoBoard.KemoBoardPanelData, KeyValuePair<Transform, RectTransform>>>>.KeyCollection.Enumerator enumerator = this.kemoData.Keys.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				CharaDef.AttributeType attributeType = enumerator.Current;
				this.DispKemoData(attributeType);
			}
			yield break;
		}
		yield break;
	}

	// Token: 0x06001345 RID: 4933 RVA: 0x000EEA92 File Offset: 0x000ECC92
	private IEnumerator PointReset(DataManagerKemoBoard.KemoBoardAreaData area, bool stone)
	{
		while (this.winPanel.activeSelf || this.useItem.baseObj.activeSelf)
		{
			yield return null;
		}
		DataManager.DmKemoBoard.RequestResetKemoBoard(area.Attr, stone);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		foreach (CharaDef.AttributeType attributeType in this.kemoData.Keys)
		{
			this.DispKemoData(attributeType);
		}
		CanvasManager.HdlOpenWindowBasic.Setup("確認", this.resetMsg, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, null, null, false);
		CanvasManager.HdlOpenWindowBasic.ForceOpen();
		do
		{
			yield return null;
		}
		while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
		yield break;
	}

	// Token: 0x06001346 RID: 4934 RVA: 0x000EEAB0 File Offset: 0x000ECCB0
	private void OnClickPoint(DataManagerKemoBoard.KemoBoardPanelData kd)
	{
		if (this.winPanel.activeSelf || this.useItem.baseObj.activeSelf || this.ienum != null)
		{
			return;
		}
		if (!this.kemoData[kd.Attr][kd.Id].Value.Key.Find("Act/Ball/Icon_Kind").gameObject.activeSelf)
		{
			return;
		}
		this.SetWinOpen(kd);
		SoundManager.Play("prd_se_click", false, false);
	}

	// Token: 0x06001347 RID: 4935 RVA: 0x000EEB3C File Offset: 0x000ECD3C
	private void OnClickPoint(bool sw)
	{
		if (this.winPanel.activeSelf || this.useItem.baseObj.activeSelf || this.ienum != null)
		{
			return;
		}
		this.boardScroll = sw;
	}

	// Token: 0x06001348 RID: 4936 RVA: 0x000EEB70 File Offset: 0x000ECD70
	private void OnClickOpen(PguiButtonCtrl button)
	{
		if (this.winPanel.activeSelf || this.useItem.baseObj.activeSelf || this.ienum != null)
		{
			return;
		}
		if (!this.guiData.WinOpen.gameObject.activeSelf || !this.iconYaji.activeSelf)
		{
			return;
		}
		if (!this.guiData.BtnOpen.ActEnable)
		{
			return;
		}
		this.guiData.WinOpen.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
		{
			this.guiData.WinOpen.gameObject.SetActive(false);
		});
		this.iconYaji.SetActive(false);
		this.guiData.BtnReset.gameObject.SetActive(true);
		this.guiData.BtnShop.gameObject.SetActive(true);
		this.ienum = this.PointOpen();
	}

	// Token: 0x06001349 RID: 4937 RVA: 0x000EEC40 File Offset: 0x000ECE40
	private void OnClickCancel(PguiButtonCtrl button)
	{
		if (this.winPanel.activeSelf || this.useItem.baseObj.activeSelf || this.ienum != null)
		{
			return;
		}
		if (!this.guiData.WinOpen.gameObject.activeSelf || !this.iconYaji.activeSelf)
		{
			return;
		}
		this.guiData.WinOpen.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
		{
			this.guiData.WinOpen.gameObject.SetActive(false);
		});
		this.iconYaji.SetActive(false);
		this.guiData.BtnReset.gameObject.SetActive(true);
		this.guiData.BtnShop.gameObject.SetActive(true);
	}

	// Token: 0x0600134A RID: 4938 RVA: 0x000EECF0 File Offset: 0x000ECEF0
	private void OnClickReset(PguiButtonCtrl button)
	{
		if (!this.winPanel.activeSelf || !this.winReset.win.FinishedOpen() || !this.winCheck.win.FinishedClose() || this.useItem.baseObj.activeSelf || !button.ActEnable)
		{
			return;
		}
		if (this.ienum != null)
		{
			return;
		}
		int idx = this.winReset.BtnReset.IndexOf(button) + 1;
		if (idx < 1 || idx > 6)
		{
			return;
		}
		this.reset = DataManager.DmKemoBoard.KemoBoardAreaDataList.Find((DataManagerKemoBoard.KemoBoardAreaData itm) => itm.Id == idx);
		if (this.reset == null)
		{
			return;
		}
		this.winPanel.SetActive(true);
		this.winCheck.attr.Replace((int)this.reset.Attr);
		this.resetMsg = CharaDef.GetAttributeName(this.reset.Attr) + "属性のボーナスをリセット";
		string text = "アイテムを消費して" + this.resetMsg + "します。";
		this.winCheck.win.MassageText.text = text + "\nよろしいですか？";
		this.useItem.window.WindowRectTransform.Find("LayoutGroup/TxtGroup").GetComponent<PguiTextCtrl>().text = text;
		this.resetMsg += "しました";
		this.resetStone = DataManager.DmItem.GetUserItemData(30100);
		this.winCheck.stone.transform.Find("BaseImage/NumInfo/Icon_Stone").GetComponent<PguiRawImageCtrl>().SetRawImage(this.resetStone.staticData.GetIconName(), true, false, null);
		this.winCheck.stone.transform.Find("NumInfo_Stone/Icon_Stone").GetComponent<PguiRawImageCtrl>().SetRawImage(this.resetStone.staticData.GetIconName(), true, false, null);
		this.winCheck.stone.transform.Find("BaseImage/NumInfo/Num").GetComponent<PguiTextCtrl>().text = this.reset.ResetStoneNum.ToString();
		this.winCheck.stone.transform.Find("NumInfo_Stone/Num").GetComponent<PguiTextCtrl>().text = this.resetStone.num.ToString();
		this.resetItem = DataManager.DmItem.GetUserItemData(this.reset.ResetItemId);
		this.winCheck.item.transform.Find("BaseImage/NumInfo/Icon_Item").GetComponent<PguiRawImageCtrl>().SetRawImage(this.resetItem.staticData.GetIconName(), true, false, null);
		this.winCheck.item.transform.Find("NumInfo_Stone/Icon_Stone").GetComponent<PguiRawImageCtrl>().SetRawImage(this.resetItem.staticData.GetIconName(), true, false, null);
		this.winCheck.item.transform.Find("BaseImage/NumInfo/Num").GetComponent<PguiTextCtrl>().text = this.reset.ResetItemNum.ToString();
		this.winCheck.item.transform.Find("NumInfo_Stone/Num").GetComponent<PguiTextCtrl>().text = this.resetItem.num.ToString();
		this.winCheck.stone.SetActEnable(this.reset.ResetStoneNum <= this.resetStone.num, false, false);
		this.winCheck.item.SetActEnable(this.reset.ResetItemNum <= this.resetItem.num, false, false);
		this.winCheck.win.ForceOpen();
		this.winReset.win.ForceClose();
		this.resetType = 0;
	}

	// Token: 0x0600134B RID: 4939 RVA: 0x000EF0D0 File Offset: 0x000ED2D0
	private bool OnClickResetCheck(int idx)
	{
		if (!this.winReset.win.FinishedClose() || this.useItem.baseObj.activeSelf)
		{
			return false;
		}
		if (this.ienum != null)
		{
			return true;
		}
		if (this.reset == null)
		{
			return true;
		}
		if (this.resetType > 0)
		{
			return true;
		}
		ItemData selItm;
		int selNum;
		if (idx > 1)
		{
			if (!this.winCheck.stone.ActEnable)
			{
				return false;
			}
			selItm = this.resetStone;
			selNum = this.reset.ResetStoneNum;
		}
		else
		{
			if (idx <= 0)
			{
				this.winPanel.SetActive(true);
				this.winReset.win.ForceOpen();
				return true;
			}
			if (!this.winCheck.item.ActEnable)
			{
				return false;
			}
			selItm = this.resetItem;
			selNum = this.reset.ResetItemNum;
		}
		this.resetMsg = string.Concat(new string[]
		{
			selItm.staticData.GetName(),
			"を",
			selNum.ToString(),
			"個消費して\n",
			this.resetMsg
		});
		this.resetType = idx;
		this.useItem.baseObj.SetActive(true);
		this.useItem.itemNametext.text = selItm.staticData.GetName() + "×" + selNum.ToString();
		this.useItem.numBeforeItemText.text = selItm.num.ToString();
		this.useItem.numAfterItemText.text = (selItm.num - selNum).ToString();
		Transform transform = this.useItem.baseObj.transform.Find("Base/Window/LayoutGroup/PurchaseConfirmButton");
		if (transform != null)
		{
			transform.gameObject.SetActive(selItm.id == 30100);
			PguiButtonCtrl component = transform.GetComponent<PguiButtonCtrl>();
			if (component != null)
			{
				component.AddOnClickListener(delegate(PguiButtonCtrl btn)
				{
					CanvasManager.HdlPurchaseConfirmWindow.Initialize("探検！迷宮ゾーンの" + CharaDef.GetAttributeName(this.reset.Attr) + "属性のボーナスのリセット", DataManager.DmItem.GetItemStaticBase(selItm.id).GetName(), selNum, null, PurchaseConfirmWindow.TEMP_IMMEDIATE_DELIVERY, false);
				}, PguiButtonCtrl.SoundType.DEFAULT);
			}
		}
		Transform transform2 = this.useItem.window.WindowRectTransform.Find("Icon_Item/Icon_Item");
		if (null != transform2)
		{
			IconItemCtrl component2 = transform2.GetComponent<IconItemCtrl>();
			if (null != component2)
			{
				component2.Setup(selItm.staticData);
			}
		}
		this.useItem.window.ForceOpen();
		return true;
	}

	// Token: 0x0600134C RID: 4940 RVA: 0x000EF360 File Offset: 0x000ED560
	private bool OnClickResetOk(int idx)
	{
		if (this.winPanel.activeSelf && !this.winCheck.win.FinishedClose())
		{
			return false;
		}
		if (this.ienum != null)
		{
			return true;
		}
		if (this.reset == null)
		{
			return true;
		}
		if (this.resetType <= 0)
		{
			return true;
		}
		if (idx == 2)
		{
			this.ienum = this.PointReset(this.reset, this.resetType > 1);
		}
		else
		{
			this.winPanel.SetActive(true);
			this.winReset.win.ForceOpen();
		}
		this.reset = null;
		this.resetType = 0;
		return true;
	}

	// Token: 0x0600134D RID: 4941 RVA: 0x000EF3FC File Offset: 0x000ED5FC
	private void OnClickButton(PguiButtonCtrl button)
	{
		if (this.winPanel.activeSelf || this.useItem.baseObj.activeSelf || this.ienum != null)
		{
			return;
		}
		if (this.guiData.WinOpen.gameObject.activeSelf)
		{
			return;
		}
		if (button == this.guiData.BtnReset)
		{
			this.ienum = this.InitReset();
			return;
		}
		if (button == this.guiData.BtnShop)
		{
			this.ienum = this.GoShop();
		}
	}

	// Token: 0x0600134E RID: 4942 RVA: 0x000EF488 File Offset: 0x000ED688
	private IEnumerator InitReset()
	{
		this.winPanel.SetActive(true);
		this.SetKemoBoardParam(CharaDef.AttributeType.RED, this.winReset.win.WindowRectTransform.Find("Base/Info_R/Grid"));
		this.SetKemoBoardParam(CharaDef.AttributeType.GREEN, this.winReset.win.WindowRectTransform.Find("Base/Info_G/Grid"));
		this.SetKemoBoardParam(CharaDef.AttributeType.BLUE, this.winReset.win.WindowRectTransform.Find("Base/Info_B/Grid"));
		this.SetKemoBoardParam(CharaDef.AttributeType.PINK, this.winReset.win.WindowRectTransform.Find("Base/Info_R2/Grid"));
		this.SetKemoBoardParam(CharaDef.AttributeType.LIME, this.winReset.win.WindowRectTransform.Find("Base/Info_G2/Grid"));
		this.SetKemoBoardParam(CharaDef.AttributeType.AQUA, this.winReset.win.WindowRectTransform.Find("Base/Info_B2/Grid"));
		this.winReset.win.ForceOpen();
		yield return null;
		yield break;
	}

	// Token: 0x0600134F RID: 4943 RVA: 0x000EF498 File Offset: 0x000ED698
	private void SetKemoBoardParam(CharaDef.AttributeType attr, Transform tmp)
	{
		DataManagerKemoBoard.KemoBoardBonusParam kemoBoardBonusParam;
		if (!DataManager.DmKemoBoard.KemoBoardBonusParamMap.TryGetValue(attr, out kemoBoardBonusParam))
		{
			kemoBoardBonusParam = new DataManagerKemoBoard.KemoBoardBonusParam(attr);
		}
		tmp.Find("Num_HP").GetComponent<PguiTextCtrl>().text = "+" + kemoBoardBonusParam.Hp.ToString();
		tmp.Find("Num_Atack").GetComponent<PguiTextCtrl>().text = "+" + kemoBoardBonusParam.Attack.ToString();
		tmp.Find("Num_Guard").GetComponent<PguiTextCtrl>().text = "+" + kemoBoardBonusParam.Difence.ToString();
		tmp.Find("Num_Avoid").GetComponent<PguiTextCtrl>().text = "+" + ((float)kemoBoardBonusParam.Avoid / 10f).ToString("F1") + "%";
		tmp.Find("Num_Beat").GetComponent<PguiTextCtrl>().text = "+" + ((float)kemoBoardBonusParam.BeatDamage / 10f).ToString("F1") + "%";
		tmp.Find("Num_Action").GetComponent<PguiTextCtrl>().text = "+" + ((float)kemoBoardBonusParam.ActionDamage / 10f).ToString("F1") + "%";
		tmp.Find("Num_Try").GetComponent<PguiTextCtrl>().text = "+" + ((float)kemoBoardBonusParam.TryDamage / 10f).ToString("F1") + "%";
	}

	// Token: 0x06001350 RID: 4944 RVA: 0x000EF63F File Offset: 0x000ED83F
	private IEnumerator GoShop()
	{
		SceneShopArgs sceneShopArgs = new SceneShopArgs();
		sceneShopArgs.resultNextSceneName = SceneManager.SceneName.SceneKemoBoard;
		sceneShopArgs.resultNextSceneArgs = null;
		sceneShopArgs.shopId = 2100;
		Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneShop, sceneShopArgs);
		yield return null;
		yield break;
	}

	// Token: 0x06001351 RID: 4945 RVA: 0x000EF647 File Offset: 0x000ED847
	private bool OnClickButtonMenu(SceneManager.SceneName sceneName, object sceneArgs)
	{
		return this.winPanel.activeSelf || this.useItem.baseObj.activeSelf || this.ienum != null;
	}

	// Token: 0x06001352 RID: 4946 RVA: 0x000EF673 File Offset: 0x000ED873
	private void OnClickButtonRetrun()
	{
		if (!this.winPanel.activeSelf && !this.useItem.baseObj.activeSelf && this.ienum == null)
		{
			Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneCharaEdit, null);
		}
	}

	// Token: 0x06001353 RID: 4947 RVA: 0x000EF6A9 File Offset: 0x000ED8A9
	private void OnTouchStart(Info info)
	{
		if (this.OnUiTap(info.CurrentPosition))
		{
			return;
		}
		this.boardScroll = true;
	}

	// Token: 0x06001354 RID: 4948 RVA: 0x000EF6C1 File Offset: 0x000ED8C1
	private void OnTouchEnd(Info info)
	{
		this.boardScroll = false;
	}

	// Token: 0x06001355 RID: 4949 RVA: 0x000EF6CA File Offset: 0x000ED8CA
	private void OnTouchMove(Info info)
	{
		if (!this.boardScroll)
		{
			return;
		}
		if (this.winPanel.activeSelf || this.useItem.baseObj.activeSelf || this.ienum != null)
		{
			return;
		}
		this.moveBoard = info.DeltaPosition;
	}

	// Token: 0x06001356 RID: 4950 RVA: 0x000EF70C File Offset: 0x000ED90C
	private void OnPinch(Info fingerA, Info fingerB, float distance, float rotation)
	{
		if (!this.boardScroll)
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
		this.pinchBoard = distance * Mathf.Sqrt(1280f / (float)Screen.width * (720f / (float)Screen.height)) / 300f;
	}

	// Token: 0x06001357 RID: 4951 RVA: 0x000EF76C File Offset: 0x000ED96C
	private void OnWheel(Info info, float distance)
	{
		if (this.boardScroll)
		{
			return;
		}
		if (this.OnUiTap(info.CurrentPosition))
		{
			return;
		}
		if (!CanvasManager.CheckInWindow(info.CurrentPosition))
		{
			return;
		}
		this.wheelBoard = distance;
	}

	// Token: 0x06001358 RID: 4952 RVA: 0x000EF79C File Offset: 0x000ED99C
	private bool OnUiTap(Vector2 pos)
	{
		if (this.winPanel.activeSelf || this.useItem.baseObj.activeSelf || this.ienum != null)
		{
			return true;
		}
		PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
		pointerEventData.position = pos;
		List<RaycastResult> list = new List<RaycastResult>();
		EventSystem.current.RaycastAll(pointerEventData, list);
		return list.FindAll((RaycastResult itm) => itm.gameObject.name != "None").Count > 0;
	}

	// Token: 0x06001359 RID: 4953 RVA: 0x000EF824 File Offset: 0x000EDA24
	public override void OnDisableScene()
	{
		Manager.UnRegisterStart(new Manager.SingleAction(this.OnTouchStart));
		Manager.UnRegisterRelease(new Manager.SingleAction(this.OnTouchEnd));
		Manager.UnRegisterMove(new Manager.SingleAction(this.OnTouchMove));
		Manager.UnRegisterPinch(new Manager.DoubleAction(this.OnPinch));
		Manager.UnRegisterMouseWheel(new Manager.WheelAction(this.OnWheel));
		CanvasManager.HdlCmnMenu.SetupMenu(false, "", null, "", null, null);
		this.winPanel.SetActive(false);
		this.guiData.baseObj.SetActive(false);
		Object.Destroy(this.useItem.baseObj);
		this.useItem.baseObj = null;
		this.useItem = null;
	}

	// Token: 0x0600135A RID: 4954 RVA: 0x000EF8E0 File Offset: 0x000EDAE0
	public override void OnDestroyScene()
	{
		Object.Destroy(this.iconStart);
		this.iconStart = null;
		Object.Destroy(this.iconResult);
		this.iconResult = null;
		foreach (Dictionary<int, KeyValuePair<DataManagerKemoBoard.KemoBoardPanelData, KeyValuePair<Transform, RectTransform>>> dictionary in this.kemoData.Values)
		{
			foreach (KeyValuePair<DataManagerKemoBoard.KemoBoardPanelData, KeyValuePair<Transform, RectTransform>> keyValuePair in dictionary.Values)
			{
				Object.Destroy(keyValuePair.Value.Key.gameObject);
				if (keyValuePair.Value.Value != null)
				{
					Object.Destroy(keyValuePair.Value.Value.gameObject);
				}
			}
		}
		this.kemoData = null;
		Object.Destroy(this.guiData.baseObj);
		this.guiData.baseObj = null;
		this.winReset = null;
		this.winCheck = null;
		Object.Destroy(this.winPanel);
		this.winPanel = null;
	}

	// Token: 0x04001017 RID: 4119
	private SceneKemoBoard.GUI guiData;

	// Token: 0x04001018 RID: 4120
	private GameObject winPanel;

	// Token: 0x04001019 RID: 4121
	private SceneKemoBoard.WIN_RESET winReset;

	// Token: 0x0400101A RID: 4122
	private SceneKemoBoard.WIN_CHECK winCheck;

	// Token: 0x0400101B RID: 4123
	private StaminaRecoveryWindowCtrl.GUI_StaminaRecoveryExecuteWindow useItem;

	// Token: 0x0400101C RID: 4124
	private GameObject iconStart;

	// Token: 0x0400101D RID: 4125
	private GameObject iconResult;

	// Token: 0x0400101E RID: 4126
	private GameObject iconYaji;

	// Token: 0x0400101F RID: 4127
	private Vector2 scrMin;

	// Token: 0x04001020 RID: 4128
	private Vector2 scrMax;

	// Token: 0x04001021 RID: 4129
	private bool boardScroll;

	// Token: 0x04001022 RID: 4130
	private Vector2 moveBoard;

	// Token: 0x04001023 RID: 4131
	private float pinchBoard;

	// Token: 0x04001024 RID: 4132
	private float pinchScl;

	// Token: 0x04001025 RID: 4133
	private float wheelBoard;

	// Token: 0x04001026 RID: 4134
	private DataManagerKemoBoard.KemoBoardPanelData selPoint;

	// Token: 0x04001027 RID: 4135
	private DataManagerKemoBoard.KemoBoardAreaData reset;

	// Token: 0x04001028 RID: 4136
	private ItemData resetStone;

	// Token: 0x04001029 RID: 4137
	private ItemData resetItem;

	// Token: 0x0400102A RID: 4138
	private int resetType;

	// Token: 0x0400102B RID: 4139
	private string resetMsg;

	// Token: 0x0400102C RID: 4140
	private IEnumerator ienum;

	// Token: 0x0400102D RID: 4141
	private Dictionary<CharaDef.AttributeType, Dictionary<int, KeyValuePair<DataManagerKemoBoard.KemoBoardPanelData, KeyValuePair<Transform, RectTransform>>>> kemoData;

	// Token: 0x02000B1F RID: 2847
	public class GUI
	{
		// Token: 0x060041B3 RID: 16819 RVA: 0x001FD288 File Offset: 0x001FB488
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.BtnReset = baseTr.Find("Btn_Reset").GetComponent<PguiButtonCtrl>();
			this.BtnShop = baseTr.Find("Btn_Shop").GetComponent<PguiButtonCtrl>();
			this.WinOpen = baseTr.Find("Window_Open").GetComponent<SimpleAnimation>();
			this.BtnOpen = this.WinOpen.transform.Find("Window/All/Btn_Open").GetComponent<PguiButtonCtrl>();
			this.BtnCancel = this.WinOpen.transform.Find("Window/All/Btn_Cancel").GetComponent<PguiButtonCtrl>();
			this.Board = baseTr.Find("BG");
		}

		// Token: 0x04004615 RID: 17941
		public GameObject baseObj;

		// Token: 0x04004616 RID: 17942
		public PguiButtonCtrl BtnReset;

		// Token: 0x04004617 RID: 17943
		public PguiButtonCtrl BtnShop;

		// Token: 0x04004618 RID: 17944
		public SimpleAnimation WinOpen;

		// Token: 0x04004619 RID: 17945
		public PguiButtonCtrl BtnOpen;

		// Token: 0x0400461A RID: 17946
		public PguiButtonCtrl BtnCancel;

		// Token: 0x0400461B RID: 17947
		public Transform Board;
	}

	// Token: 0x02000B20 RID: 2848
	public class WIN_RESET
	{
		// Token: 0x060041B4 RID: 16820 RVA: 0x001FD33C File Offset: 0x001FB53C
		public WIN_RESET(Transform baseTr)
		{
			this.win = baseTr.GetComponent<PguiOpenWindowCtrl>();
			baseTr = this.win.WindowRectTransform;
			this.BtnReset = new List<PguiButtonCtrl>
			{
				baseTr.Find("Base/Info_R/Cmn_Btn_Strong").GetComponent<PguiButtonCtrl>(),
				baseTr.Find("Base/Info_G/Cmn_Btn_Strong").GetComponent<PguiButtonCtrl>(),
				baseTr.Find("Base/Info_B/Cmn_Btn_Strong").GetComponent<PguiButtonCtrl>(),
				baseTr.Find("Base/Info_R2/Cmn_Btn_Strong").GetComponent<PguiButtonCtrl>(),
				baseTr.Find("Base/Info_G2/Cmn_Btn_Strong").GetComponent<PguiButtonCtrl>(),
				baseTr.Find("Base/Info_B2/Cmn_Btn_Strong").GetComponent<PguiButtonCtrl>()
			};
			this.param = new List<Transform>
			{
				baseTr.Find("Base/Info_R/Grid"),
				baseTr.Find("Base/Info_G/Grid"),
				baseTr.Find("Base/Info_B/Grid"),
				baseTr.Find("Base/Info_R2/Grid"),
				baseTr.Find("Base/Info_G2/Grid"),
				baseTr.Find("Base/Info_B2/Grid")
			};
		}

		// Token: 0x0400461C RID: 17948
		public PguiOpenWindowCtrl win;

		// Token: 0x0400461D RID: 17949
		public List<PguiButtonCtrl> BtnReset;

		// Token: 0x0400461E RID: 17950
		public List<Transform> param;
	}

	// Token: 0x02000B21 RID: 2849
	public class WIN_CHECK
	{
		// Token: 0x060041B5 RID: 16821 RVA: 0x001FD468 File Offset: 0x001FB668
		public WIN_CHECK(Transform baseTr)
		{
			this.win = baseTr.GetComponent<PguiOpenWindowCtrl>();
			baseTr = this.win.WindowRectTransform;
			this.attr = baseTr.Find("Icon_Atr").GetComponent<PguiReplaceSpriteCtrl>();
			this.item = baseTr.Find("ButtonUseItem").GetComponent<PguiButtonCtrl>();
			this.stone = baseTr.Find("ButtonUseStone").GetComponent<PguiButtonCtrl>();
		}

		// Token: 0x0400461F RID: 17951
		public PguiOpenWindowCtrl win;

		// Token: 0x04004620 RID: 17952
		public PguiReplaceSpriteCtrl attr;

		// Token: 0x04004621 RID: 17953
		public PguiButtonCtrl item;

		// Token: 0x04004622 RID: 17954
		public PguiButtonCtrl stone;
	}
}
