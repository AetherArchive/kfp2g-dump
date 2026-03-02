using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.uGUI;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000122 RID: 290
public class StaminaRecoveryWindowCtrl : MonoBehaviour
{
	// Token: 0x17000335 RID: 821
	// (get) Token: 0x06000ED1 RID: 3793 RVA: 0x000B42BF File Offset: 0x000B24BF
	private int ConstRecoveryStaminaMax
	{
		get
		{
			return DataManager.DmServerMst.MstAppConfig.staminaLimit;
		}
	}

	// Token: 0x17000336 RID: 822
	// (get) Token: 0x06000ED2 RID: 3794 RVA: 0x000B42D0 File Offset: 0x000B24D0
	private int kirakiraUnitNum
	{
		get
		{
			return DataManager.DmServerMst.MstAppConfig.staminaStone;
		}
	}

	// Token: 0x17000337 RID: 823
	// (get) Token: 0x06000ED3 RID: 3795 RVA: 0x000B42E1 File Offset: 0x000B24E1
	private int kirakiraUnitNumSkip
	{
		get
		{
			return DataManager.DmServerMst.MstAppConfig.skipStone;
		}
	}

	// Token: 0x17000338 RID: 824
	// (get) Token: 0x06000ED4 RID: 3796 RVA: 0x000B42F4 File Offset: 0x000B24F4
	// (set) Token: 0x06000ED5 RID: 3797 RVA: 0x000B4324 File Offset: 0x000B2524
	private int SelectItemRecoveryValue
	{
		get
		{
			StaminaRecoveryWindowCtrl.RecoveryItemData recoveryItemData = this.recoveryItemDataList.Find((StaminaRecoveryWindowCtrl.RecoveryItemData x) => x.itemId == this.selectedItemId);
			if (recoveryItemData == null)
			{
				return 0;
			}
			return recoveryItemData.point;
		}
		set
		{
		}
	}

	// Token: 0x06000ED6 RID: 3798 RVA: 0x000B4328 File Offset: 0x000B2528
	public void Initialize()
	{
		this.guiStaminaKindWindow = new StaminaRecoveryWindowCtrl.GUI_StaminaItemSelectWindow(base.transform.Find("GUI_Cmn_StaminaKind(Clone)"));
		this.guiStaminaUseWindow = new StaminaRecoveryWindowCtrl.GUI_StaminaItemUseNumWindow(base.transform.Find("GUI_Cmn_StaminaUse(Clone)"));
		this.guiStaminaRecoveryWindow = new StaminaRecoveryWindowCtrl.GUI_StaminaRecoveryExecuteWindow(base.transform.Find("GUI_Cmn_StaminaUseKirakira(Clone)"));
		this.guiQuestSkipRecoveryWindow = new StaminaRecoveryWindowCtrl.GUI_QuestSkipRecoveryExecuteWindow(base.transform.Find("GUI_Cmn_QuestSkipUseKirakira(Clone)"));
		this.guiQuestSkipRecoveryFinWindow = new StaminaRecoveryWindowCtrl.GUI_QuestSkipRecoveryFinishWindow(base.transform.Find("GUI_Cmn_QuestSkipRecoveryFinish(Clone)"));
	}

	// Token: 0x06000ED7 RID: 3799 RVA: 0x000B43BC File Offset: 0x000B25BC
	private void Setup()
	{
		this.recoveryItemDataList = new List<StaminaRecoveryWindowCtrl.RecoveryItemData>
		{
			new StaminaRecoveryWindowCtrl.RecoveryItemData
			{
				itemId = 30100,
				reqNum = this.kirakiraUnitNum,
				point = DataManager.DmUserInfo.staminaInfo.GetInfoByNow(TimeManager.Now).stackMaxNum
			}
		};
		foreach (DataManagerItem.StaminaRecoveryItemData staminaRecoveryItemData in DataManager.DmItem.StaminaRecoveryItemDataList)
		{
			StaminaRecoveryWindowCtrl.RecoveryItemData recoveryItemData = new StaminaRecoveryWindowCtrl.RecoveryItemData
			{
				itemId = staminaRecoveryItemData.id,
				reqNum = 1,
				point = staminaRecoveryItemData.recoveryValue
			};
			this.recoveryItemDataList.Add(recoveryItemData);
		}
		this.nowWindow = StaminaRecoveryWindowCtrl.WindowType.Invalid;
	}

	// Token: 0x06000ED8 RID: 3800 RVA: 0x000B4490 File Offset: 0x000B2690
	public IEnumerator StaminaCheckAction(int questOneId, int tryCount = 1)
	{
		this.Setup();
		QuestOnePackData qopd = DataManager.DmQuest.GetQuestOnePackData(questOneId);
		int num = qopd.questOne.stamina;
		Predicate<DataManagerCampaign.CampaignTarget> <>9__0;
		foreach (DataManagerCampaign.CampaignQuestStaminaData campaignQuestStaminaData in DataManager.DmCampaign.PresentCampaignQuestStaminaDataList)
		{
			if (campaignQuestStaminaData.value >= 0)
			{
				List<DataManagerCampaign.CampaignTarget> campaignTargetList = campaignQuestStaminaData.campaignTargetList;
				Predicate<DataManagerCampaign.CampaignTarget> predicate;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = (DataManagerCampaign.CampaignTarget itm) => itm.TargetType == DataManagerCampaign.TARGET_TYPE.Chapter && itm.TargetId == qopd.questChapter.chapterId);
				}
				if (campaignTargetList.Find(predicate) != null)
				{
					if ((num -= campaignQuestStaminaData.value) < 0)
					{
						num = 0;
						break;
					}
					break;
				}
			}
		}
		num *= tryCount;
		if (num <= DataManager.DmUserInfo.staminaInfo.GetInfoByNow(TimeManager.Now).stackNum)
		{
			yield return true;
			yield break;
		}
		this.nowWindow = StaminaRecoveryWindowCtrl.WindowType.UseItemSelectWindow;
		bool isLoop = true;
		PguiButtonCtrl.OnClick <>9__5;
		while (isLoop)
		{
			switch (this.nowWindow)
			{
			case StaminaRecoveryWindowCtrl.WindowType.Invalid:
				yield break;
			case StaminaRecoveryWindowCtrl.WindowType.UseItemSelectWindow:
			{
				bool isWindowFinish2 = false;
				StaminaRecoveryWindowCtrl.PushButton btnIndex2 = StaminaRecoveryWindowCtrl.PushButton.Invalid;
				bool itemSelected = false;
				this.selectedItemId = 0;
				this.selectedItemNum = 0;
				this.guiStaminaKindWindow.window.SetupByStaminaSelect(delegate(int index)
				{
					btnIndex2 = (StaminaRecoveryWindowCtrl.PushButton)index;
					isWindowFinish2 = true;
					return true;
				});
				List<StaminaRecoveryWindowCtrl.RecoveryItemData> list = this.recoveryItemDataList.FindAll((StaminaRecoveryWindowCtrl.RecoveryItemData x) => 30100 == x.itemId || 0 < DataManager.DmItem.GetUserItemData(x.itemId).num);
				List<int> reqIdList = new List<int>();
				foreach (StaminaRecoveryWindowCtrl.RecoveryItemData recoveryItemData in list)
				{
					reqIdList.Add(recoveryItemData.itemId);
				}
				this.guiStaminaKindWindow.titleText.text = "スタミナ回復";
				this.guiStaminaKindWindow.kindInfoText.text = "スタミナがたりません\nアイテムを使ってスタミナを回復します";
				this.guiStaminaKindWindow.scrollView.gameObject.SetActive(false);
				this.guiStaminaKindWindow.window.Open();
				while (!this.guiStaminaKindWindow.window.FinishedOpen())
				{
					yield return null;
				}
				this.InitializeItemSelectWindow(reqIdList);
				while (!isWindowFinish2 || !this.guiStaminaKindWindow.window.FinishedClose())
				{
					if (this.selectedItemId != 0)
					{
						this.guiStaminaKindWindow.window.ForceClose();
						itemSelected = true;
						break;
					}
					yield return null;
				}
				if (!itemSelected)
				{
					this.nowWindow = StaminaRecoveryWindowCtrl.WindowType.Invalid;
				}
				else if (30100 == this.selectedItemId)
				{
					if (this.kirakiraUnitNum > DataManager.DmItem.GetUserItemData(30100).num)
					{
						this.nowWindow = StaminaRecoveryWindowCtrl.WindowType.NoStoneWindow;
					}
					else
					{
						this.selectedItemNum = this.kirakiraUnitNum;
						this.nowWindow = StaminaRecoveryWindowCtrl.WindowType.ConfirmWindow;
					}
				}
				else
				{
					this.nowWindow = StaminaRecoveryWindowCtrl.WindowType.ItemSettingWindow;
				}
				break;
			}
			case StaminaRecoveryWindowCtrl.WindowType.ItemSettingWindow:
			{
				bool isWindowFinish3 = false;
				StaminaRecoveryWindowCtrl.PushButton btnIndex3 = StaminaRecoveryWindowCtrl.PushButton.Invalid;
				this.guiStaminaUseWindow.window.SetupByStaminaSetting(delegate(int index)
				{
					btnIndex3 = (StaminaRecoveryWindowCtrl.PushButton)index;
					isWindowFinish3 = true;
					return true;
				});
				this.guiStaminaUseWindow.window.Open();
				while (this.guiStaminaUseWindow.window.FinishedOpen())
				{
					yield return null;
				}
				this.guiStaminaUseWindow.iconItemNestPrefab.InitForce();
				while (null == this.guiStaminaUseWindow.iconItemNestPrefab.transform.Find("Icon_Item"))
				{
					yield return null;
				}
				this.guiStaminaUseWindow.buttonClose.androidBackKeyTarget = true;
				this.InitializeStaminaUseWindow();
				while ((!isWindowFinish3 || !this.guiStaminaUseWindow.window.FinishedClose()) && btnIndex3 == StaminaRecoveryWindowCtrl.PushButton.Invalid)
				{
					yield return null;
				}
				switch (btnIndex3)
				{
				case StaminaRecoveryWindowCtrl.PushButton.Cancel:
				case StaminaRecoveryWindowCtrl.PushButton.Close:
					this.nowWindow = StaminaRecoveryWindowCtrl.WindowType.UseItemSelectWindow;
					break;
				case StaminaRecoveryWindowCtrl.PushButton.OK:
					this.nowWindow = StaminaRecoveryWindowCtrl.WindowType.ConfirmWindow;
					break;
				default:
					this.nowWindow = StaminaRecoveryWindowCtrl.WindowType.Invalid;
					break;
				}
				break;
			}
			case StaminaRecoveryWindowCtrl.WindowType.ConfirmWindow:
			{
				bool isWindowFinish = false;
				StaminaRecoveryWindowCtrl.PushButton btnIndex = StaminaRecoveryWindowCtrl.PushButton.Invalid;
				this.guiStaminaRecoveryWindow.window.SetupByStaminaUse(delegate(int index)
				{
					btnIndex = (StaminaRecoveryWindowCtrl.PushButton)index;
					isWindowFinish = true;
					return true;
				});
				Transform transform = this.guiStaminaRecoveryWindow.baseObj.transform.Find("Base/Window/LayoutGroup/PurchaseConfirmButton");
				if (transform != null)
				{
					transform.gameObject.SetActive(this.selectedItemId == 30100);
					PguiButtonCtrl component = transform.GetComponent<PguiButtonCtrl>();
					if (component != null)
					{
						PguiButtonCtrl pguiButtonCtrl = component;
						PguiButtonCtrl.OnClick onClick;
						if ((onClick = <>9__5) == null)
						{
							onClick = (<>9__5 = delegate(PguiButtonCtrl btn)
							{
								CanvasManager.HdlPurchaseConfirmWindow.Initialize("スタミナの回復", DataManager.DmItem.GetItemStaticBase(this.selectedItemId).GetName(), this.selectedItemNum, null, PurchaseConfirmWindow.TEMP_IMMEDIATE_DELIVERY, false);
							});
						}
						pguiButtonCtrl.AddOnClickListener(onClick, PguiButtonCtrl.SoundType.DEFAULT);
					}
				}
				this.guiStaminaRecoveryWindow.window.Open();
				while (this.guiStaminaRecoveryWindow.window.FinishedOpen())
				{
					yield return null;
				}
				this.guiStaminaRecoveryWindow.baseObj.transform.Find("Base/Window/Icon_Item").GetComponent<PguiNestPrefab>().InitForce();
				while (null == this.guiStaminaRecoveryWindow.baseObj.transform.Find("Base/Window/Icon_Item/Icon_Item"))
				{
					yield return null;
				}
				this.guiStaminaRecoveryWindow.buttonClose.androidBackKeyTarget = true;
				this.InitializeItemUseWindow(this.guiStaminaRecoveryWindow.window);
				while ((!isWindowFinish || !this.guiStaminaRecoveryWindow.window.FinishedClose()) && btnIndex == StaminaRecoveryWindowCtrl.PushButton.Invalid)
				{
					yield return null;
				}
				switch (btnIndex)
				{
				case StaminaRecoveryWindowCtrl.PushButton.Cancel:
				case StaminaRecoveryWindowCtrl.PushButton.Close:
					if (30100 == this.selectedItemId)
					{
						this.nowWindow = StaminaRecoveryWindowCtrl.WindowType.UseItemSelectWindow;
					}
					else
					{
						this.nowWindow = StaminaRecoveryWindowCtrl.WindowType.ItemSettingWindow;
					}
					break;
				case StaminaRecoveryWindowCtrl.PushButton.OK:
					this.nowWindow = StaminaRecoveryWindowCtrl.WindowType.RecoveryWindow;
					break;
				default:
					this.nowWindow = StaminaRecoveryWindowCtrl.WindowType.Invalid;
					break;
				}
				break;
			}
			case StaminaRecoveryWindowCtrl.WindowType.RecoveryWindow:
			{
				int num2 = ((30100 == this.selectedItemId) ? this.kirakiraUnitNum : this.selectedItemNum);
				DataManager.DmUserInfo.RequestActionRecoveryStamina(this.selectedItemId, num2);
				while (DataManager.IsServerRequesting())
				{
					yield return null;
				}
				StaminaRecoveryWindowCtrl.<>c__DisplayClass35_4 CS$<>8__locals5 = new StaminaRecoveryWindowCtrl.<>c__DisplayClass35_4();
				CS$<>8__locals5.isWindowFinish = false;
				CanvasManager.HdlOpenWindowBasic.Setup("確認", "スタミナを回復しました", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int index)
				{
					CS$<>8__locals5.isWindowFinish = true;
					return true;
				}, null, false);
				CanvasManager.HdlOpenWindowBasic.Open();
				while (!CS$<>8__locals5.isWindowFinish)
				{
					yield return null;
				}
				CS$<>8__locals5 = null;
				this.nowWindow = StaminaRecoveryWindowCtrl.WindowType.Invalid;
				break;
			}
			case StaminaRecoveryWindowCtrl.WindowType.NoStoneWindow:
			{
				IEnumerator enumerator = this.NoStoneWindowAction(true, StaminaRecoveryWindowCtrl.WindowType.UseItemSelectWindow);
				while (enumerator.MoveNext())
				{
					yield return null;
				}
				break;
			}
			case StaminaRecoveryWindowCtrl.WindowType.PurchaseWindow:
			{
				IEnumerator enumerator = this.PurchaseWindowAction(StaminaRecoveryWindowCtrl.WindowType.UseItemSelectWindow);
				while (enumerator.MoveNext())
				{
					yield return null;
				}
				break;
			}
			default:
				yield return null;
				break;
			}
		}
		yield break;
	}

	// Token: 0x06000ED9 RID: 3801 RVA: 0x000B44AD File Offset: 0x000B26AD
	public IEnumerator SkipRecoveryAction(int questOneId, DataManagerMonthlyPack.PurchaseMonthlypackData mpd, QuestOnePackData qopd)
	{
		this.recoveryItemDataList = new List<StaminaRecoveryWindowCtrl.RecoveryItemData>
		{
			new StaminaRecoveryWindowCtrl.RecoveryItemData
			{
				itemId = 30100,
				reqNum = this.kirakiraUnitNumSkip,
				point = 1
			}
		};
		this.nowWindow = StaminaRecoveryWindowCtrl.WindowType.UseItemSelectWindow;
		StaminaRecoveryWindowCtrl.WindowType fromWindow = StaminaRecoveryWindowCtrl.WindowType.Invalid;
		for (;;)
		{
			switch (this.nowWindow)
			{
			case StaminaRecoveryWindowCtrl.WindowType.Invalid:
				goto IL_085D;
			case StaminaRecoveryWindowCtrl.WindowType.UseItemSelectWindow:
			{
				bool isWindowFinish2 = false;
				StaminaRecoveryWindowCtrl.PushButton btnIndex2 = StaminaRecoveryWindowCtrl.PushButton.Invalid;
				bool itemSelected = false;
				this.selectedItemId = 0;
				this.selectedItemNum = 0;
				this.guiStaminaKindWindow.window.SetupByStaminaSelect(delegate(int index)
				{
					btnIndex2 = (StaminaRecoveryWindowCtrl.PushButton)index;
					isWindowFinish2 = true;
					return true;
				});
				List<StaminaRecoveryWindowCtrl.RecoveryItemData> list = this.recoveryItemDataList.FindAll((StaminaRecoveryWindowCtrl.RecoveryItemData x) => 30100 == x.itemId || 0 < DataManager.DmItem.GetUserItemData(x.itemId).num);
				List<int> reqIdList = new List<int>();
				foreach (StaminaRecoveryWindowCtrl.RecoveryItemData recoveryItemData in list)
				{
					reqIdList.Add(recoveryItemData.itemId);
				}
				this.guiStaminaKindWindow.titleText.text = "スキップ可能回数回復";
				this.guiStaminaKindWindow.kindInfoText.text = "アイテムを使って<color=red>スキップ可能回数</color>を回復します";
				this.guiStaminaKindWindow.scrollView.gameObject.SetActive(false);
				this.guiStaminaKindWindow.window.Open();
				while (!this.guiStaminaKindWindow.window.FinishedOpen())
				{
					yield return null;
				}
				this.InitializeItemSelectWindow(reqIdList);
				while (!isWindowFinish2 || !this.guiStaminaKindWindow.window.FinishedClose())
				{
					if (this.selectedItemId != 0)
					{
						this.guiStaminaKindWindow.window.ForceClose();
						itemSelected = true;
						break;
					}
					yield return null;
				}
				if (!itemSelected)
				{
					this.nowWindow = StaminaRecoveryWindowCtrl.WindowType.Invalid;
				}
				else
				{
					this.nowWindow = StaminaRecoveryWindowCtrl.WindowType.ItemSettingWindow;
					if (this.kirakiraUnitNumSkip > DataManager.DmItem.GetUserItemData(30100).num)
					{
						fromWindow = StaminaRecoveryWindowCtrl.WindowType.UseItemSelectWindow;
						this.nowWindow = StaminaRecoveryWindowCtrl.WindowType.NoStoneWindow;
					}
				}
				break;
			}
			case StaminaRecoveryWindowCtrl.WindowType.ItemSettingWindow:
			{
				bool isWindowFinish3 = false;
				StaminaRecoveryWindowCtrl.PushButton btnIndex3 = StaminaRecoveryWindowCtrl.PushButton.Invalid;
				this.guiStaminaUseWindow.window.SetupByStaminaSetting(delegate(int index)
				{
					btnIndex3 = (StaminaRecoveryWindowCtrl.PushButton)index;
					isWindowFinish3 = true;
					return true;
				});
				this.guiStaminaUseWindow.window.Open();
				while (this.guiStaminaUseWindow.window.FinishedOpen())
				{
					yield return null;
				}
				this.guiStaminaUseWindow.iconItemNestPrefab.InitForce();
				while (null == this.guiStaminaUseWindow.iconItemNestPrefab.transform.Find("Icon_Item"))
				{
					yield return null;
				}
				this.guiStaminaUseWindow.buttonClose.androidBackKeyTarget = true;
				this.InitializeSkipUseWindow(QuestUtil.GetSkipInfo(mpd, qopd));
				while ((!isWindowFinish3 || !this.guiStaminaUseWindow.window.FinishedClose()) && btnIndex3 == StaminaRecoveryWindowCtrl.PushButton.Invalid)
				{
					yield return null;
				}
				switch (btnIndex3)
				{
				case StaminaRecoveryWindowCtrl.PushButton.Cancel:
				case StaminaRecoveryWindowCtrl.PushButton.Close:
					this.nowWindow = StaminaRecoveryWindowCtrl.WindowType.UseItemSelectWindow;
					break;
				case StaminaRecoveryWindowCtrl.PushButton.OK:
					if (this.selectedItemNum * this.kirakiraUnitNumSkip > DataManager.DmItem.GetUserItemData(30100).num)
					{
						fromWindow = StaminaRecoveryWindowCtrl.WindowType.ItemSettingWindow;
						this.nowWindow = StaminaRecoveryWindowCtrl.WindowType.NoStoneWindow;
					}
					else
					{
						this.nowWindow = StaminaRecoveryWindowCtrl.WindowType.ConfirmWindow;
					}
					break;
				default:
					this.nowWindow = StaminaRecoveryWindowCtrl.WindowType.Invalid;
					break;
				}
				break;
			}
			case StaminaRecoveryWindowCtrl.WindowType.ConfirmWindow:
			{
				bool isWindowFinish4 = false;
				StaminaRecoveryWindowCtrl.PushButton btnIndex = StaminaRecoveryWindowCtrl.PushButton.Invalid;
				this.guiQuestSkipRecoveryWindow.window.SetupByStaminaUse(delegate(int index)
				{
					btnIndex = (StaminaRecoveryWindowCtrl.PushButton)index;
					isWindowFinish4 = true;
					return true;
				});
				Transform transform = this.guiQuestSkipRecoveryWindow.baseObj.transform.Find("Base/Window/LayoutGroup/PurchaseConfirmButton");
				if (transform != null)
				{
					transform.gameObject.SetActive(this.selectedItemId == 30100);
					PguiButtonCtrl component = transform.GetComponent<PguiButtonCtrl>();
					if (component != null)
					{
						component.AddOnClickListener(delegate(PguiButtonCtrl btn)
						{
							CanvasManager.HdlPurchaseConfirmWindow.Initialize("スキップ可能回数の回復", DataManager.DmItem.GetItemStaticBase(this.selectedItemId).GetName(), this.selectedItemNum * this.kirakiraUnitNumSkip, null, PurchaseConfirmWindow.TEMP_IMMEDIATE_DELIVERY, false);
						}, PguiButtonCtrl.SoundType.DEFAULT);
					}
				}
				this.guiQuestSkipRecoveryWindow.window.Open();
				while (this.guiQuestSkipRecoveryWindow.window.FinishedOpen())
				{
					yield return null;
				}
				this.guiQuestSkipRecoveryWindow.baseObj.transform.Find("Base/Window/Icon_Item").GetComponent<PguiNestPrefab>().InitForce();
				while (null == this.guiQuestSkipRecoveryWindow.baseObj.transform.Find("Base/Window/Icon_Item/Icon_Item"))
				{
					yield return null;
				}
				this.guiQuestSkipRecoveryWindow.buttonClose.androidBackKeyTarget = true;
				this.InitializeQuestSkipItemUseWindow(this.guiQuestSkipRecoveryWindow.window, QuestUtil.GetSkipInfo(mpd, qopd));
				while ((!isWindowFinish4 || !this.guiQuestSkipRecoveryWindow.window.FinishedClose()) && btnIndex == StaminaRecoveryWindowCtrl.PushButton.Invalid)
				{
					yield return null;
				}
				switch (btnIndex)
				{
				case StaminaRecoveryWindowCtrl.PushButton.Cancel:
				case StaminaRecoveryWindowCtrl.PushButton.Close:
					this.nowWindow = StaminaRecoveryWindowCtrl.WindowType.ItemSettingWindow;
					break;
				case StaminaRecoveryWindowCtrl.PushButton.OK:
					this.nowWindow = StaminaRecoveryWindowCtrl.WindowType.RecoveryWindow;
					break;
				default:
					this.nowWindow = StaminaRecoveryWindowCtrl.WindowType.Invalid;
					break;
				}
				break;
			}
			case StaminaRecoveryWindowCtrl.WindowType.RecoveryWindow:
			{
				int num = this.selectedItemNum * this.SelectItemRecoveryValue;
				DataManager.DmQuest.RequestActionRecoverySkip(questOneId, this.selectedItemId, num);
				while (DataManager.IsServerRequesting())
				{
					yield return null;
				}
				QuestUtil.UsrQuestSkipInfo skipInfo = QuestUtil.GetSkipInfo(mpd, qopd);
				if (CanvasManager.HdlSelCharaDeck.gameObject.activeSelf)
				{
					CanvasManager.HdlSelCharaDeck.SetQuestSkipPopup(skipInfo);
				}
				else if (Singleton<SceneManager>.Instance.CurrentSceneName == SceneManager.SceneName.SceneBattleResult)
				{
					((SceneBattleResult)Singleton<SceneManager>.Instance.CurrentScene).SetQuestSkipPopup(skipInfo);
				}
				bool isWindowFinish = false;
				this.guiQuestSkipRecoveryFinWindow.window.Setup(null, null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int index)
				{
					isWindowFinish = true;
					return true;
				}, null, false);
				this.guiQuestSkipRecoveryFinWindow.window.Open();
				while (this.guiQuestSkipRecoveryFinWindow.window.FinishedOpen())
				{
					yield return null;
				}
				this.InitializeQuestSkipRecoveryFinWindow(skipInfo);
				while (!isWindowFinish)
				{
					yield return null;
				}
				this.nowWindow = StaminaRecoveryWindowCtrl.WindowType.Invalid;
				break;
			}
			case StaminaRecoveryWindowCtrl.WindowType.NoStoneWindow:
			{
				IEnumerator enumerator = this.NoStoneWindowAction(false, fromWindow);
				while (enumerator.MoveNext())
				{
					yield return null;
				}
				break;
			}
			case StaminaRecoveryWindowCtrl.WindowType.PurchaseWindow:
			{
				IEnumerator enumerator = this.PurchaseWindowAction(fromWindow);
				while (enumerator.MoveNext())
				{
					yield return null;
				}
				break;
			}
			default:
				yield return null;
				break;
			}
		}
		IL_085D:
		yield break;
		yield break;
	}

	// Token: 0x06000EDA RID: 3802 RVA: 0x000B44D1 File Offset: 0x000B26D1
	private IEnumerator NoStoneWindowAction(bool isStamina, StaminaRecoveryWindowCtrl.WindowType fromWindow)
	{
		bool isWindowFinish = false;
		StaminaRecoveryWindowCtrl.PushButton btnIndex = StaminaRecoveryWindowCtrl.PushButton.Invalid;
		PguiOpenWindowCtrl noStoneWindowCtrl = CanvasManager.HdlOpenWindowNoStone;
		int num = (isStamina ? this.kirakiraUnitNum : (this.kirakiraUnitNumSkip * Math.Max(this.selectedItemNum, 1)));
		noStoneWindowCtrl.SetupByNoStone(num, 30100, delegate(int index)
		{
			isWindowFinish = true;
			switch (index)
			{
			case -1:
				btnIndex = StaminaRecoveryWindowCtrl.PushButton.Close;
				break;
			case 0:
				btnIndex = StaminaRecoveryWindowCtrl.PushButton.Cancel;
				break;
			case 1:
				btnIndex = StaminaRecoveryWindowCtrl.PushButton.OK;
				break;
			default:
				btnIndex = StaminaRecoveryWindowCtrl.PushButton.Invalid;
				break;
			}
			return true;
		});
		noStoneWindowCtrl.Open();
		while ((!isWindowFinish || !noStoneWindowCtrl.FinishedClose()) && btnIndex == StaminaRecoveryWindowCtrl.PushButton.Invalid)
		{
			yield return null;
		}
		switch (btnIndex)
		{
		case StaminaRecoveryWindowCtrl.PushButton.Cancel:
		case StaminaRecoveryWindowCtrl.PushButton.Close:
			this.nowWindow = fromWindow;
			break;
		case StaminaRecoveryWindowCtrl.PushButton.OK:
			this.nowWindow = StaminaRecoveryWindowCtrl.WindowType.PurchaseWindow;
			break;
		default:
			this.nowWindow = StaminaRecoveryWindowCtrl.WindowType.Invalid;
			break;
		}
		yield break;
	}

	// Token: 0x06000EDB RID: 3803 RVA: 0x000B44EE File Offset: 0x000B26EE
	private IEnumerator PurchaseWindowAction(StaminaRecoveryWindowCtrl.WindowType fromWindow)
	{
		SelPurchaseStoneWindowCtrl purchaseStoneWindow = CanvasManager.HdlSelPurchaseStoneWindowCtrl;
		purchaseStoneWindow.Setup(PurchaseProductOne.TabType.Invalid);
		while (purchaseStoneWindow.IsActiveWindow())
		{
			yield return null;
		}
		this.nowWindow = fromWindow;
		yield break;
	}

	// Token: 0x06000EDC RID: 3804 RVA: 0x000B4504 File Offset: 0x000B2704
	private void InitializeItemSelectWindow(List<int> reqIdList)
	{
		this.requiredItemIdList = reqIdList;
		ReuseScroll scrollView = this.guiStaminaKindWindow.scrollView;
		scrollView.onStartItem = (Action<int, GameObject>)Delegate.Combine(scrollView.onStartItem, new Action<int, GameObject>(this.OnStartStaminaView));
		ReuseScroll scrollView2 = this.guiStaminaKindWindow.scrollView;
		scrollView2.onUpdateItem = (Action<int, GameObject>)Delegate.Combine(scrollView2.onUpdateItem, new Action<int, GameObject>(this.OnUpdateStaminaView));
		int num = 0;
		List<GameObject> list = new List<GameObject>();
		foreach (int num2 in this.requiredItemIdList)
		{
			Transform transform = this.guiStaminaKindWindow.staminaContent.transform.Find(num.ToString());
			if (null != transform)
			{
				list.Add(transform.gameObject);
			}
			num++;
		}
		this.guiStaminaKindWindow.scrollView.gameObject.SetActive(true);
		if (list.Count == 0)
		{
			this.guiStaminaKindWindow.scrollView.Setup(this.requiredItemIdList.Count, 0);
			return;
		}
		this.guiStaminaKindWindow.scrollView.Resize(this.requiredItemIdList.Count, this.guiStaminaKindWindow.scrollView.CalcCurrentFocusIndex());
	}

	// Token: 0x06000EDD RID: 3805 RVA: 0x000B4654 File Offset: 0x000B2854
	private void InitializeStaminaUseWindow()
	{
		this.guiStaminaUseWindow.titleText.text = "スタミナ回復";
		this.guiStaminaUseWindow.recoveryTitleText.text = "スタミナ回復量";
		this.guiStaminaUseWindow.descriptionText.text = "使う数を選んでください";
		this.selectedItemNum = 1;
		this.guiStaminaUseWindow.itemNameText.text = DataManager.DmItem.GetItemStaticBase(this.selectedItemId).GetName();
		int nowItemNum = DataManager.DmItem.GetUserItemData(this.selectedItemId).num;
		this.guiStaminaUseWindow.numBeforeItemText.text = nowItemNum.ToString();
		this.guiStaminaUseWindow.numAfterItemText.text = (nowItemNum - this.selectedItemNum).ToString();
		Transform transform = this.guiStaminaUseWindow.baseObj.transform.Find("Base/Window/Base_UseInfo/Icon_Item/Icon_Item");
		if (null != transform)
		{
			IconItemCtrl component = transform.GetComponent<IconItemCtrl>();
			if (null != component)
			{
				component.Setup(DataManager.DmItem.GetItemStaticBase(this.selectedItemId));
			}
		}
		int nowStaminaNum = DataManager.DmUserInfo.staminaInfo.GetInfoByNow(TimeManager.Now).stackNum;
		this.guiStaminaUseWindow.numBeforeStaminaText.text = nowStaminaNum.ToString();
		int num = ((30100 == this.selectedItemId) ? this.SelectItemRecoveryValue : (this.selectedItemNum * this.SelectItemRecoveryValue));
		int num2 = nowStaminaNum + num;
		this.guiStaminaUseWindow.numAfterStaminaText.text = ((this.ConstRecoveryStaminaMax < num2) ? this.ConstRecoveryStaminaMax : num2).ToString();
		int num3 = this.ConstRecoveryStaminaMax - nowStaminaNum;
		int num4 = num3 / this.SelectItemRecoveryValue;
		num4 = ((nowItemNum < num4) ? (nowItemNum - 1) : num4);
		int num5 = num3 % this.SelectItemRecoveryValue;
		if (0 < num5)
		{
			num4++;
		}
		this.guiStaminaUseWindow.slider.onValueChanged.RemoveAllListeners();
		this.guiStaminaUseWindow.slider.minValue = 1f;
		this.guiStaminaUseWindow.slider.maxValue = (float)num4;
		this.guiStaminaUseWindow.slider.value = 1f;
		this.guiStaminaUseWindow.slider.onValueChanged.AddListener(delegate(float slider)
		{
			StaminaRecoveryWindowCtrl.Limit limit = this.SetSelectedItemNum(int.Parse(slider.ToString()), null);
			this.guiStaminaUseWindow.SetButton(limit);
			this.guiStaminaUseWindow.excText.text = this.selectedItemNum.ToString();
			this.guiStaminaUseWindow.numAfterItemText.text = (nowItemNum - this.selectedItemNum).ToString();
			int num6 = nowStaminaNum + this.selectedItemNum * this.SelectItemRecoveryValue;
			this.guiStaminaUseWindow.numAfterStaminaText.text = ((this.ConstRecoveryStaminaMax < num6) ? this.ConstRecoveryStaminaMax : num6).ToString();
		});
		this.guiStaminaUseWindow.SetButton(this.SetSelectedItemNum(this.selectedItemNum, null));
		this.guiStaminaUseWindow.excText.text = this.selectedItemNum.ToString();
		this.guiStaminaUseWindow.buttonInc.AddOnClickListener(delegate(PguiButtonCtrl button)
		{
			StaminaRecoveryWindowCtrl.Limit limit2 = this.SetSelectedItemNum(this.selectedItemNum + 1, null);
			this.guiStaminaUseWindow.SetButton(limit2);
			this.guiStaminaUseWindow.excText.text = this.selectedItemNum.ToString();
			this.guiStaminaUseWindow.numAfterItemText.text = (nowItemNum - this.selectedItemNum).ToString();
			int num7 = nowStaminaNum + this.selectedItemNum * this.SelectItemRecoveryValue;
			this.guiStaminaUseWindow.numAfterStaminaText.text = ((this.ConstRecoveryStaminaMax < num7) ? this.ConstRecoveryStaminaMax : num7).ToString();
			this.guiStaminaUseWindow.slider.value = (float)this.selectedItemNum;
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.guiStaminaUseWindow.buttonDec.AddOnClickListener(delegate(PguiButtonCtrl button)
		{
			StaminaRecoveryWindowCtrl.Limit limit3 = this.SetSelectedItemNum(this.selectedItemNum - 1, null);
			this.guiStaminaUseWindow.SetButton(limit3);
			this.guiStaminaUseWindow.excText.text = this.selectedItemNum.ToString();
			this.guiStaminaUseWindow.numAfterItemText.text = (nowItemNum - this.selectedItemNum).ToString();
			int num8 = nowStaminaNum + this.selectedItemNum * this.SelectItemRecoveryValue;
			this.guiStaminaUseWindow.numAfterStaminaText.text = ((this.ConstRecoveryStaminaMax < num8) ? this.ConstRecoveryStaminaMax : num8).ToString();
			this.guiStaminaUseWindow.slider.value = (float)this.selectedItemNum;
		}, PguiButtonCtrl.SoundType.DEFAULT);
	}

	// Token: 0x06000EDE RID: 3806 RVA: 0x000B4934 File Offset: 0x000B2B34
	private void InitializeSkipUseWindow(QuestUtil.UsrQuestSkipInfo skipInfo)
	{
		this.guiStaminaUseWindow.titleText.text = "スキップ可能回数回復";
		this.guiStaminaUseWindow.recoveryTitleText.text = "スキップ可能回数";
		string text = "<color=red>スキップ可能回数1回につき、キラキラ×{0}を使って回復します</color>\n回復する<color=red>スキップ可能回数</color>を選んでください\n";
		this.guiStaminaUseWindow.descriptionText.text = string.Format(text, this.kirakiraUnitNumSkip);
		this.selectedItemNum = 1;
		this.guiStaminaUseWindow.itemNameText.text = "";
		int nowItemNum = DataManager.DmItem.GetUserItemData(this.selectedItemId).num;
		this.guiStaminaUseWindow.numBeforeItemText.text = nowItemNum.ToString();
		this.guiStaminaUseWindow.numAfterItemText.text = (nowItemNum - this.selectedItemNum * this.kirakiraUnitNumSkip).ToString();
		this.guiStaminaUseWindow.numAfterItemText.m_Text.color = ((0 <= nowItemNum - this.selectedItemNum * this.kirakiraUnitNumSkip) ? Color.white : Color.red);
		Transform transform = this.guiStaminaUseWindow.baseObj.transform.Find("Base/Window/Base_UseInfo/Icon_Item/Icon_Item");
		if (null != transform)
		{
			IconItemCtrl component = transform.GetComponent<IconItemCtrl>();
			if (null != component)
			{
				component.Setup(DataManager.DmItem.GetItemStaticBase(this.selectedItemId), this.kirakiraUnitNumSkip);
			}
		}
		int nowSkipNum = skipInfo.restSkipCount;
		this.guiStaminaUseWindow.numBeforeStaminaText.text = nowSkipNum.ToString();
		int num = nowSkipNum + this.selectedItemNum * this.SelectItemRecoveryValue;
		this.guiStaminaUseWindow.numAfterStaminaText.text = num.ToString();
		int restSkipRecoveryCount = skipInfo.restSkipRecoveryCount;
		this.guiStaminaUseWindow.slider.onValueChanged.RemoveAllListeners();
		this.guiStaminaUseWindow.slider.minValue = 1f;
		this.guiStaminaUseWindow.slider.maxValue = (float)restSkipRecoveryCount;
		this.guiStaminaUseWindow.slider.value = 1f;
		this.guiStaminaUseWindow.slider.onValueChanged.AddListener(delegate(float slider)
		{
			StaminaRecoveryWindowCtrl.Limit limit = this.SetSelectedItemNum(int.Parse(slider.ToString()), skipInfo);
			this.guiStaminaUseWindow.SetButton(limit);
			this.guiStaminaUseWindow.excText.text = this.selectedItemNum.ToString();
			this.guiStaminaUseWindow.numAfterItemText.text = (nowItemNum - this.selectedItemNum * this.kirakiraUnitNumSkip).ToString();
			this.guiStaminaUseWindow.numAfterItemText.m_Text.color = ((0 <= nowItemNum - this.selectedItemNum * this.kirakiraUnitNumSkip) ? Color.white : Color.red);
			int num2 = nowSkipNum + this.selectedItemNum * this.SelectItemRecoveryValue;
			this.guiStaminaUseWindow.numAfterStaminaText.text = num2.ToString();
		});
		this.guiStaminaUseWindow.SetButton(this.SetSelectedItemNum(this.selectedItemNum, skipInfo));
		this.guiStaminaUseWindow.excText.text = this.selectedItemNum.ToString();
		this.guiStaminaUseWindow.buttonInc.AddOnClickListener(delegate(PguiButtonCtrl button)
		{
			StaminaRecoveryWindowCtrl.Limit limit2 = this.SetSelectedItemNum(this.selectedItemNum + 1, skipInfo);
			this.guiStaminaUseWindow.SetButton(limit2);
			this.guiStaminaUseWindow.excText.text = this.selectedItemNum.ToString();
			this.guiStaminaUseWindow.numAfterItemText.text = (nowItemNum - this.selectedItemNum * this.kirakiraUnitNumSkip).ToString();
			this.guiStaminaUseWindow.numAfterItemText.m_Text.color = ((0 <= nowItemNum - this.selectedItemNum * this.kirakiraUnitNumSkip) ? Color.white : Color.red);
			int num3 = nowSkipNum + this.selectedItemNum * this.SelectItemRecoveryValue;
			this.guiStaminaUseWindow.numAfterStaminaText.text = num3.ToString();
			this.guiStaminaUseWindow.slider.value = (float)this.selectedItemNum;
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.guiStaminaUseWindow.buttonDec.AddOnClickListener(delegate(PguiButtonCtrl button)
		{
			StaminaRecoveryWindowCtrl.Limit limit3 = this.SetSelectedItemNum(this.selectedItemNum - 1, skipInfo);
			this.guiStaminaUseWindow.SetButton(limit3);
			this.guiStaminaUseWindow.excText.text = this.selectedItemNum.ToString();
			this.guiStaminaUseWindow.numAfterItemText.text = (nowItemNum - this.selectedItemNum * this.kirakiraUnitNumSkip).ToString();
			this.guiStaminaUseWindow.numAfterItemText.m_Text.color = ((0 <= nowItemNum - this.selectedItemNum * this.kirakiraUnitNumSkip) ? Color.white : Color.red);
			int num4 = nowSkipNum + this.selectedItemNum * this.SelectItemRecoveryValue;
			this.guiStaminaUseWindow.numAfterStaminaText.text = num4.ToString();
			this.guiStaminaUseWindow.slider.value = (float)this.selectedItemNum;
		}, PguiButtonCtrl.SoundType.DEFAULT);
	}

	// Token: 0x06000EDF RID: 3807 RVA: 0x000B4BFC File Offset: 0x000B2DFC
	private StaminaRecoveryWindowCtrl.Limit SetSelectedItemNum(int useItemNum, QuestUtil.UsrQuestSkipInfo skipInfo = null)
	{
		int num = ((skipInfo == null) ? 99 : skipInfo.restSkipRecoveryCount);
		if (useItemNum >= num)
		{
			useItemNum = num;
		}
		if (useItemNum <= 1)
		{
			useItemNum = 1;
		}
		StaminaRecoveryWindowCtrl.Limit limit = StaminaRecoveryWindowCtrl.Limit.Invalid;
		if (skipInfo != null)
		{
			if (skipInfo.restSkipRecoveryCount < useItemNum + 1)
			{
				limit = StaminaRecoveryWindowCtrl.Limit.Over;
			}
		}
		else
		{
			int stackNum = DataManager.DmUserInfo.staminaInfo.GetInfoByNow(TimeManager.Now).stackNum;
			int num2 = ((skipInfo == null) ? stackNum : skipInfo.restSkipCount);
			while (this.ConstRecoveryStaminaMax + this.SelectItemRecoveryValue < useItemNum * this.SelectItemRecoveryValue + num2)
			{
				useItemNum--;
			}
			if (DataManager.DmItem.GetUserItemData(this.selectedItemId).num < useItemNum)
			{
				useItemNum = DataManager.DmItem.GetUserItemData(this.selectedItemId).num;
			}
			if (this.ConstRecoveryStaminaMax + this.SelectItemRecoveryValue < (useItemNum + 1) * this.SelectItemRecoveryValue || DataManager.DmItem.GetUserItemData(this.selectedItemId).num < useItemNum + 1)
			{
				limit = StaminaRecoveryWindowCtrl.Limit.Over;
			}
		}
		if (1 == useItemNum)
		{
			limit = ((StaminaRecoveryWindowCtrl.Limit.Over == limit) ? StaminaRecoveryWindowCtrl.Limit.OverAndUnder : StaminaRecoveryWindowCtrl.Limit.Under);
		}
		this.selectedItemNum = useItemNum;
		return limit;
	}

	// Token: 0x06000EE0 RID: 3808 RVA: 0x000B4D00 File Offset: 0x000B2F00
	private void InitializeItemUseWindow(PguiOpenWindowCtrl powc)
	{
		this.guiStaminaRecoveryWindow.itemNametext.text = DataManager.DmItem.GetItemStaticBase(this.selectedItemId).GetName() + "×" + this.selectedItemNum.ToString();
		int num = DataManager.DmItem.GetUserItemData(this.selectedItemId).num;
		this.guiStaminaRecoveryWindow.numBeforeItemText.text = num.ToString();
		this.guiStaminaRecoveryWindow.numAfterItemText.text = (num - this.selectedItemNum).ToString();
		Transform transform = powc.gameObject.transform.Find("Base/Window/Icon_Item/Icon_Item");
		if (null != transform)
		{
			IconItemCtrl component = transform.GetComponent<IconItemCtrl>();
			if (null != component)
			{
				component.Setup(DataManager.DmItem.GetItemStaticBase(this.selectedItemId));
			}
		}
		int stackNum = DataManager.DmUserInfo.staminaInfo.GetInfoByNow(TimeManager.Now).stackNum;
		this.guiStaminaRecoveryWindow.numBeforeStaminaText.text = stackNum.ToString();
		int num2 = ((30100 == this.selectedItemId) ? this.SelectItemRecoveryValue : (this.selectedItemNum * this.SelectItemRecoveryValue));
		int num3 = stackNum + num2;
		this.guiStaminaRecoveryWindow.numAfterStaminaText.text = ((this.ConstRecoveryStaminaMax < num3) ? this.ConstRecoveryStaminaMax : num3).ToString();
	}

	// Token: 0x06000EE1 RID: 3809 RVA: 0x000B4E64 File Offset: 0x000B3064
	private void InitializeQuestSkipItemUseWindow(PguiOpenWindowCtrl powc, QuestUtil.UsrQuestSkipInfo skipInfo)
	{
		this.guiQuestSkipRecoveryWindow.itemNametext.text = DataManager.DmItem.GetItemStaticBase(this.selectedItemId).GetName() + "×" + (this.selectedItemNum * this.kirakiraUnitNumSkip).ToString() + "<size=28>を使って</size>";
		int num = DataManager.DmItem.GetUserItemData(this.selectedItemId).num;
		this.guiQuestSkipRecoveryWindow.numBeforeItemText.text = num.ToString();
		this.guiQuestSkipRecoveryWindow.numAfterItemText.text = (num - this.selectedItemNum * this.kirakiraUnitNumSkip).ToString();
		Transform transform = powc.gameObject.transform.Find("Base/Window/Icon_Item/Icon_Item");
		IconItemCtrl iconItemCtrl;
		if (null != transform && transform.TryGetComponent<IconItemCtrl>(out iconItemCtrl))
		{
			iconItemCtrl.Setup(DataManager.DmItem.GetItemStaticBase(this.selectedItemId));
		}
		this.guiQuestSkipRecoveryWindow.numBeforeSkipCntText.text = skipInfo.restSkipCount.ToString();
		this.guiQuestSkipRecoveryWindow.numAfterSkipCntText.text = (skipInfo.restSkipCount + this.selectedItemNum).ToString();
		this.guiQuestSkipRecoveryWindow.numBeforeSkipRcvrCntText.text = skipInfo.restSkipRecoveryCount.ToString();
		this.guiQuestSkipRecoveryWindow.numAfterSkipRcvrCntText.text = (skipInfo.restSkipRecoveryCount - this.selectedItemNum).ToString();
		this.guiQuestSkipRecoveryWindow.messageText.text = "このクエストの<color=red>スキップ可能回数</color>を回復します";
	}

	// Token: 0x06000EE2 RID: 3810 RVA: 0x000B4FE4 File Offset: 0x000B31E4
	private void InitializeQuestSkipRecoveryFinWindow(QuestUtil.UsrQuestSkipInfo skipInfo)
	{
		this.guiQuestSkipRecoveryFinWindow.numBeforeSkipRcvrCntText.text = (skipInfo.restSkipRecoveryCount + this.selectedItemNum).ToString();
		int restSkipRecoveryCount = skipInfo.restSkipRecoveryCount;
		this.guiQuestSkipRecoveryFinWindow.numAfterSkipRcvrCntText.text = restSkipRecoveryCount.ToString();
		if (restSkipRecoveryCount == 0)
		{
			this.guiQuestSkipRecoveryFinWindow.alertText.text = "※" + skipInfo.prefixStr + "はもう回復できません";
			return;
		}
		this.guiQuestSkipRecoveryFinWindow.alertText.text = string.Format("※{0}{1}残り{2}回回復可能です", skipInfo.prefixStr, (skipInfo.prefixStr == "期間中") ? "" : "は", restSkipRecoveryCount);
	}

	// Token: 0x06000EE3 RID: 3811 RVA: 0x000B50A1 File Offset: 0x000B32A1
	private void OnStartStaminaView(int rowIdx, GameObject go)
	{
		this.SetStaminaView(rowIdx, go);
	}

	// Token: 0x06000EE4 RID: 3812 RVA: 0x000B50AB File Offset: 0x000B32AB
	private void OnUpdateStaminaView(int rowIdx, GameObject go)
	{
		this.SetStaminaView(rowIdx, go);
	}

	// Token: 0x06000EE5 RID: 3813 RVA: 0x000B50B8 File Offset: 0x000B32B8
	private void SetStaminaView(int rowIdx, GameObject go)
	{
		if (this.requiredItemIdList.Count <= rowIdx)
		{
			go.SetActive(false);
			return;
		}
		go.SetActive(true);
		StaminaRecoveryWindowCtrl.GUI_StaminaItemLabel gui_StaminaItemLabel = new StaminaRecoveryWindowCtrl.GUI_StaminaItemLabel(go.transform);
		gui_StaminaItemLabel.iconText.text = DataManager.DmItem.GetItemStaticBase(this.requiredItemIdList[rowIdx]).GetName();
		gui_StaminaItemLabel.numText.text = DataManager.DmItem.GetUserItemData(this.requiredItemIdList[rowIdx]).num.ToString();
		gui_StaminaItemLabel.iconItem.Setup(DataManager.DmItem.GetItemStaticBase(this.requiredItemIdList[rowIdx]));
		gui_StaminaItemLabel.button.AddOnClickListener(delegate(PguiButtonCtrl button)
		{
			this.OnClickStaminaButton(rowIdx);
		}, PguiButtonCtrl.SoundType.DEFAULT);
	}

	// Token: 0x06000EE6 RID: 3814 RVA: 0x000B51A4 File Offset: 0x000B33A4
	private void OnClickStaminaButton(int rowIdx)
	{
		int num = ((this.requiredItemIdList.Count <= rowIdx) ? 0 : rowIdx);
		this.selectedItemId = this.requiredItemIdList[num];
	}

	// Token: 0x04000D7D RID: 3453
	private StaminaRecoveryWindowCtrl.GUI_StaminaItemSelectWindow guiStaminaKindWindow;

	// Token: 0x04000D7E RID: 3454
	private StaminaRecoveryWindowCtrl.GUI_StaminaItemUseNumWindow guiStaminaUseWindow;

	// Token: 0x04000D7F RID: 3455
	private StaminaRecoveryWindowCtrl.GUI_StaminaRecoveryExecuteWindow guiStaminaRecoveryWindow;

	// Token: 0x04000D80 RID: 3456
	private StaminaRecoveryWindowCtrl.GUI_QuestSkipRecoveryExecuteWindow guiQuestSkipRecoveryWindow;

	// Token: 0x04000D81 RID: 3457
	private StaminaRecoveryWindowCtrl.GUI_QuestSkipRecoveryFinishWindow guiQuestSkipRecoveryFinWindow;

	// Token: 0x04000D82 RID: 3458
	private StaminaRecoveryWindowCtrl.WindowType nowWindow;

	// Token: 0x04000D83 RID: 3459
	private int selectedItemId;

	// Token: 0x04000D84 RID: 3460
	private int selectedItemNum;

	// Token: 0x04000D85 RID: 3461
	private const int ConstUseItemNumMin = 1;

	// Token: 0x04000D86 RID: 3462
	private const int ConstUseItemNumMax = 99;

	// Token: 0x04000D87 RID: 3463
	private const int kirakira_common = 30100;

	// Token: 0x04000D88 RID: 3464
	private const int ConstSkipRecoveryNum = 1;

	// Token: 0x04000D89 RID: 3465
	private List<StaminaRecoveryWindowCtrl.RecoveryItemData> recoveryItemDataList;

	// Token: 0x04000D8A RID: 3466
	private List<int> requiredItemIdList;

	// Token: 0x02000923 RID: 2339
	public class GUI_StaminaItemSelectWindow
	{
		// Token: 0x06003AF4 RID: 15092 RVA: 0x001D20F0 File Offset: 0x001D02F0
		public GUI_StaminaItemSelectWindow(Transform baseTr)
		{
			this.window = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.titleText = baseTr.Find("Base/Window/Title/Txt").gameObject.GetComponent<PguiTextCtrl>();
			this.kindInfoText = baseTr.Find("Base/Window/Base_KindInfo/Txt").gameObject.GetComponent<PguiTextCtrl>();
			this.scrollView = baseTr.Find("Base/Window/Base_KindInfo/InBase/ScrollView").GetComponent<ReuseScroll>();
			this.staminaContent = baseTr.Find("Base/Window/Base_KindInfo/InBase/ScrollView/Viewport/Content").gameObject;
		}

		// Token: 0x04003BB3 RID: 15283
		public PguiOpenWindowCtrl window;

		// Token: 0x04003BB4 RID: 15284
		public PguiTextCtrl titleText;

		// Token: 0x04003BB5 RID: 15285
		public PguiTextCtrl kindInfoText;

		// Token: 0x04003BB6 RID: 15286
		public ReuseScroll scrollView;

		// Token: 0x04003BB7 RID: 15287
		public GameObject staminaContent;
	}

	// Token: 0x02000924 RID: 2340
	public class GUI_StaminaItemLabel
	{
		// Token: 0x06003AF5 RID: 15093 RVA: 0x001D2174 File Offset: 0x001D0374
		public GUI_StaminaItemLabel(Transform baseTr)
		{
			this.iconItem = baseTr.Find("BaseImage/Icon_Item/Icon_Item").gameObject.GetComponent<IconItemCtrl>();
			this.iconText = baseTr.Find("BaseImage/Txt").gameObject.GetComponent<PguiTextCtrl>();
			this.numText = baseTr.Find("BaseImage/Cmn_Parts_Info/Num_01").gameObject.GetComponent<PguiTextCtrl>();
			this.button = baseTr.gameObject.GetComponent<PguiButtonCtrl>();
		}

		// Token: 0x04003BB8 RID: 15288
		public IconItemCtrl iconItem;

		// Token: 0x04003BB9 RID: 15289
		public PguiTextCtrl iconText;

		// Token: 0x04003BBA RID: 15290
		public PguiTextCtrl numText;

		// Token: 0x04003BBB RID: 15291
		public PguiButtonCtrl button;
	}

	// Token: 0x02000925 RID: 2341
	public class GUI_StaminaItemUseNumWindow
	{
		// Token: 0x06003AF6 RID: 15094 RVA: 0x001D21EC File Offset: 0x001D03EC
		public GUI_StaminaItemUseNumWindow(Transform baseTr)
		{
			this.window = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.baseObj = baseTr.gameObject;
			this.itemNameText = baseTr.Find("Base/Window/Base_UseInfo/Txt01").gameObject.GetComponent<PguiTextCtrl>();
			this.iconItemNestPrefab = baseTr.Find("Base/Window/Base_UseInfo/Icon_Item").GetComponent<PguiNestPrefab>();
			this.buttonClose = baseTr.Find("Base/Window/Window_BtnClose").GetComponent<PguiButtonCtrl>();
			this.buttonInc = baseTr.Find("Base/Window/Base_UseInfo/Exchange/Btn_Plus").gameObject.GetComponent<PguiButtonCtrl>();
			this.buttonDec = baseTr.Find("Base/Window/Base_UseInfo/Exchange/Btn_Minus").gameObject.GetComponent<PguiButtonCtrl>();
			this.numBeforeItemText = baseTr.Find("Base/Window/Base_UseInfo/Parts_ItemUseInfo/Num_BeforeTxt").gameObject.GetComponent<PguiTextCtrl>();
			this.numAfterItemText = baseTr.Find("Base/Window/Base_UseInfo/Parts_ItemUseInfo/Num_AfterTxt").gameObject.GetComponent<PguiTextCtrl>();
			this.numBeforeStaminaText = baseTr.Find("Base/Window/Base_UseInfo/Parts_ItemUseCoin/Num_BeforeTxt").gameObject.GetComponent<PguiTextCtrl>();
			this.numAfterStaminaText = baseTr.Find("Base/Window/Base_UseInfo/Parts_ItemUseCoin/Num_AfterTxt").gameObject.GetComponent<PguiTextCtrl>();
			this.titleText = baseTr.Find("Base/Window/Title/Txt").GetComponent<PguiTextCtrl>();
			this.recoveryTitleText = baseTr.Find("Base/Window/Base_UseInfo/Parts_ItemUseCoin/Txt01").gameObject.GetComponent<PguiTextCtrl>();
			this.descriptionText = baseTr.Find("Base/Window/Base_UseInfo/Txt02").GetComponent<PguiTextCtrl>();
			this.excText = baseTr.Find("Base/Window/Base_UseInfo/Exchange/Tex/Num_Txt").gameObject.GetComponent<PguiTextCtrl>();
			this.slider = baseTr.Find("Base/Window/Base_UseInfo/SliderBar").GetComponent<Slider>();
			this.sliderObj = baseTr.Find("Base/Window/Base_UseInfo/SliderBar").gameObject;
		}

		// Token: 0x06003AF7 RID: 15095 RVA: 0x001D2390 File Offset: 0x001D0590
		public void SetButton(StaminaRecoveryWindowCtrl.Limit limit)
		{
			this.buttonInc.SetActEnable(true, false, false);
			this.buttonDec.SetActEnable(true, false, false);
			switch (limit)
			{
			case StaminaRecoveryWindowCtrl.Limit.Over:
				this.buttonInc.SetActEnable(false, false, false);
				return;
			case StaminaRecoveryWindowCtrl.Limit.Under:
				this.buttonDec.SetActEnable(false, false, false);
				return;
			case StaminaRecoveryWindowCtrl.Limit.OverAndUnder:
				this.buttonInc.SetActEnable(false, false, false);
				this.buttonDec.SetActEnable(false, false, false);
				return;
			default:
				return;
			}
		}

		// Token: 0x04003BBC RID: 15292
		public GameObject baseObj;

		// Token: 0x04003BBD RID: 15293
		public PguiOpenWindowCtrl window;

		// Token: 0x04003BBE RID: 15294
		public PguiTextCtrl itemNameText;

		// Token: 0x04003BBF RID: 15295
		public PguiNestPrefab iconItemNestPrefab;

		// Token: 0x04003BC0 RID: 15296
		public PguiButtonCtrl buttonClose;

		// Token: 0x04003BC1 RID: 15297
		public PguiButtonCtrl buttonInc;

		// Token: 0x04003BC2 RID: 15298
		public PguiButtonCtrl buttonDec;

		// Token: 0x04003BC3 RID: 15299
		public PguiTextCtrl numBeforeItemText;

		// Token: 0x04003BC4 RID: 15300
		public PguiTextCtrl numAfterItemText;

		// Token: 0x04003BC5 RID: 15301
		public PguiTextCtrl numBeforeStaminaText;

		// Token: 0x04003BC6 RID: 15302
		public PguiTextCtrl numAfterStaminaText;

		// Token: 0x04003BC7 RID: 15303
		public PguiTextCtrl titleText;

		// Token: 0x04003BC8 RID: 15304
		public PguiTextCtrl recoveryTitleText;

		// Token: 0x04003BC9 RID: 15305
		public PguiTextCtrl descriptionText;

		// Token: 0x04003BCA RID: 15306
		public PguiTextCtrl excText;

		// Token: 0x04003BCB RID: 15307
		public Slider slider;

		// Token: 0x04003BCC RID: 15308
		public GameObject sliderObj;
	}

	// Token: 0x02000926 RID: 2342
	public class GUI_StaminaRecoveryExecuteWindow
	{
		// Token: 0x06003AF8 RID: 15096 RVA: 0x001D2408 File Offset: 0x001D0608
		public GUI_StaminaRecoveryExecuteWindow(Transform baseTr)
		{
			this.window = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.baseObj = baseTr.gameObject;
			this.buttonClose = baseTr.Find("Base/Window/Window_BtnClose").GetComponent<PguiButtonCtrl>();
			this.itemNametext = baseTr.Find("Base/Window/Txt01").gameObject.GetComponent<PguiTextCtrl>();
			this.numBeforeItemText = baseTr.Find("Base/Window/LayoutGroup/TxtGroup/Parts_ItemUseInfo/Num_BeforeTxt").gameObject.GetComponent<PguiTextCtrl>();
			this.numAfterItemText = baseTr.Find("Base/Window/LayoutGroup/TxtGroup/Parts_ItemUseInfo/Num_AfterTxt").gameObject.GetComponent<PguiTextCtrl>();
			this.numBeforeStaminaText = baseTr.Find("Base/Window/LayoutGroup/TxtGroup/Parts_ItemUseCoin/Num_BeforeTxt").gameObject.GetComponent<PguiTextCtrl>();
			this.numAfterStaminaText = baseTr.Find("Base/Window/LayoutGroup/TxtGroup/Parts_ItemUseCoin/Num_AfterTxt").gameObject.GetComponent<PguiTextCtrl>();
		}

		// Token: 0x04003BCD RID: 15309
		public GameObject baseObj;

		// Token: 0x04003BCE RID: 15310
		public PguiOpenWindowCtrl window;

		// Token: 0x04003BCF RID: 15311
		public PguiButtonCtrl buttonClose;

		// Token: 0x04003BD0 RID: 15312
		public PguiTextCtrl itemNametext;

		// Token: 0x04003BD1 RID: 15313
		public PguiTextCtrl numBeforeItemText;

		// Token: 0x04003BD2 RID: 15314
		public PguiTextCtrl numAfterItemText;

		// Token: 0x04003BD3 RID: 15315
		public PguiTextCtrl numBeforeStaminaText;

		// Token: 0x04003BD4 RID: 15316
		public PguiTextCtrl numAfterStaminaText;
	}

	// Token: 0x02000927 RID: 2343
	public class GUI_QuestSkipRecoveryExecuteWindow
	{
		// Token: 0x06003AF9 RID: 15097 RVA: 0x001D24D0 File Offset: 0x001D06D0
		public GUI_QuestSkipRecoveryExecuteWindow(Transform baseTr)
		{
			this.window = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.baseObj = baseTr.gameObject;
			this.buttonClose = baseTr.Find("Base/Window/Window_BtnClose").GetComponent<PguiButtonCtrl>();
			this.itemNametext = baseTr.Find("Base/Window/Txt01").gameObject.GetComponent<PguiTextCtrl>();
			this.messageText = baseTr.Find("Base/Window/LayoutGroup/TxtGroup").gameObject.GetComponent<PguiTextCtrl>();
			this.numBeforeItemText = baseTr.Find("Base/Window/LayoutGroup/TxtGroup/Parts_ItemUseInfo/Num_BeforeTxt").gameObject.GetComponent<PguiTextCtrl>();
			this.numAfterItemText = baseTr.Find("Base/Window/LayoutGroup/TxtGroup/Parts_ItemUseInfo/Num_AfterTxt").gameObject.GetComponent<PguiTextCtrl>();
			this.numBeforeSkipCntText = baseTr.Find("Base/Window/LayoutGroup/TxtGroup/Parts_SkipCount/Num_BeforeTxt").gameObject.GetComponent<PguiTextCtrl>();
			this.numAfterSkipCntText = baseTr.Find("Base/Window/LayoutGroup/TxtGroup/Parts_SkipCount/Num_AfterTxt").gameObject.GetComponent<PguiTextCtrl>();
			this.numBeforeSkipRcvrCntText = baseTr.Find("Base/Window/LayoutGroup/TxtGroup/Parts_SkipRecoveryCount/Num_BeforeTxt").gameObject.GetComponent<PguiTextCtrl>();
			this.numAfterSkipRcvrCntText = baseTr.Find("Base/Window/LayoutGroup/TxtGroup/Parts_SkipRecoveryCount/Num_AfterTxt").gameObject.GetComponent<PguiTextCtrl>();
		}

		// Token: 0x04003BD5 RID: 15317
		public GameObject baseObj;

		// Token: 0x04003BD6 RID: 15318
		public PguiOpenWindowCtrl window;

		// Token: 0x04003BD7 RID: 15319
		public PguiButtonCtrl buttonClose;

		// Token: 0x04003BD8 RID: 15320
		public PguiTextCtrl itemNametext;

		// Token: 0x04003BD9 RID: 15321
		public PguiTextCtrl messageText;

		// Token: 0x04003BDA RID: 15322
		public PguiTextCtrl numBeforeItemText;

		// Token: 0x04003BDB RID: 15323
		public PguiTextCtrl numAfterItemText;

		// Token: 0x04003BDC RID: 15324
		public PguiTextCtrl numBeforeSkipCntText;

		// Token: 0x04003BDD RID: 15325
		public PguiTextCtrl numAfterSkipCntText;

		// Token: 0x04003BDE RID: 15326
		public PguiTextCtrl numBeforeSkipRcvrCntText;

		// Token: 0x04003BDF RID: 15327
		public PguiTextCtrl numAfterSkipRcvrCntText;
	}

	// Token: 0x02000928 RID: 2344
	public class GUI_QuestSkipRecoveryFinishWindow
	{
		// Token: 0x06003AFA RID: 15098 RVA: 0x001D25EC File Offset: 0x001D07EC
		public GUI_QuestSkipRecoveryFinishWindow(Transform baseTr)
		{
			this.window = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.baseObj = baseTr.gameObject;
			this.buttonClose = baseTr.Find("Base/Window/BtnClose").GetComponent<PguiButtonCtrl>();
			this.numBeforeSkipRcvrCntText = baseTr.Find("Base/Window/Parts_SkipRecoveryCount/Num_BeforeTxt").gameObject.GetComponent<PguiTextCtrl>();
			this.numAfterSkipRcvrCntText = baseTr.Find("Base/Window/Parts_SkipRecoveryCount/Num_AfterTxt").gameObject.GetComponent<PguiTextCtrl>();
			this.alertText = baseTr.Find("Base/Window/AlertMessage").gameObject.GetComponent<PguiTextCtrl>();
		}

		// Token: 0x04003BE0 RID: 15328
		public GameObject baseObj;

		// Token: 0x04003BE1 RID: 15329
		public PguiOpenWindowCtrl window;

		// Token: 0x04003BE2 RID: 15330
		public PguiButtonCtrl buttonClose;

		// Token: 0x04003BE3 RID: 15331
		public PguiTextCtrl numBeforeSkipRcvrCntText;

		// Token: 0x04003BE4 RID: 15332
		public PguiTextCtrl numAfterSkipRcvrCntText;

		// Token: 0x04003BE5 RID: 15333
		public PguiTextCtrl alertText;
	}

	// Token: 0x02000929 RID: 2345
	private class RecoveryItemData
	{
		// Token: 0x04003BE6 RID: 15334
		public int itemId;

		// Token: 0x04003BE7 RID: 15335
		public int reqNum;

		// Token: 0x04003BE8 RID: 15336
		public int point;
	}

	// Token: 0x0200092A RID: 2346
	public enum WindowType
	{
		// Token: 0x04003BEA RID: 15338
		Invalid,
		// Token: 0x04003BEB RID: 15339
		UseItemSelectWindow,
		// Token: 0x04003BEC RID: 15340
		ItemSettingWindow,
		// Token: 0x04003BED RID: 15341
		ConfirmWindow,
		// Token: 0x04003BEE RID: 15342
		RecoveryWindow,
		// Token: 0x04003BEF RID: 15343
		NoStoneWindow,
		// Token: 0x04003BF0 RID: 15344
		PurchaseWindow
	}

	// Token: 0x0200092B RID: 2347
	public enum PushButton
	{
		// Token: 0x04003BF2 RID: 15346
		DefClose = -1,
		// Token: 0x04003BF3 RID: 15347
		Invalid,
		// Token: 0x04003BF4 RID: 15348
		Cancel,
		// Token: 0x04003BF5 RID: 15349
		OK,
		// Token: 0x04003BF6 RID: 15350
		Close
	}

	// Token: 0x0200092C RID: 2348
	public enum Limit
	{
		// Token: 0x04003BF8 RID: 15352
		Invalid,
		// Token: 0x04003BF9 RID: 15353
		Over,
		// Token: 0x04003BFA RID: 15354
		Under,
		// Token: 0x04003BFB RID: 15355
		OverAndUnder
	}
}
