using System;
using System.Collections;
using SGNFW.Common;
using UnityEngine;
using UnityEngine.Events;

public class SelQuestCountRecoveryWindowCtrl : MonoBehaviour
{
	private int QuestOneId { get; set; }

	private DateTime OpenWindowTime { get; set; }

	public void Init()
	{
		GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("SelCmn/GUI/Prefab/GUI_QuestRevival_Window"), base.transform);
		this.window = new SelQuestCountRecoveryWindowCtrl.Window(gameObject.transform);
		this.window.owCtrl.Setup(null, null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_OK), true, delegate(int index)
		{
			if (index == 1)
			{
				this.requestSequence = Singleton<SceneManager>.Instance.StartCoroutine(this.RequestSequence());
			}
			return true;
		}, null, false);
	}

	public void Setup(int questOneId, UnityAction refreshCb)
	{
		this.OpenWindowTime = TimeManager.Now;
		DateTime terminalTimeByDay = new DateTime(this.OpenWindowTime.Year, this.OpenWindowTime.Month, this.OpenWindowTime.Day);
		terminalTimeByDay = TimeManager.GetTerminalTimeByDay(terminalTimeByDay);
		this.QuestOneId = 0;
		this.refreshCallback = refreshCb;
		if (this.requestSequence != null)
		{
			Singleton<SceneManager>.Instance.StopCoroutine(this.requestSequence);
		}
		this.requestSequence = null;
		QuestStaticQuestOne questStaticQuestOne = DataManager.DmQuest.QuestStaticData.oneDataMap[questOneId];
		QuestOnePackData questOnePackData = DataManager.DmQuest.GetQuestOnePackData(questOneId);
		ItemInput keyItem = questStaticQuestOne.RecoveryKeyItem;
		this.window.SetupWindowGui(new SelQuestCountRecoveryWindowCtrl.SetupGuiParts
		{
			itemId = keyItem.itemId,
			needItemNum = keyItem.num,
			todayRecoveryNum = questOnePackData.questDynamicOne.todayRecoveryNum,
			recoveryMaxNum = questStaticQuestOne.RecoveryMaxNum,
			limitClearNum = questStaticQuestOne.limitClearNum
		});
		if (keyItem.itemId == 30100)
		{
			Transform transform = this.window.baseObj.transform.Find("Window_QuestRevival/Base/Window/RevivalOK/LayoutGroup/PurchaseConfirmButton");
			if (transform != null)
			{
				PguiButtonCtrl component = transform.GetComponent<PguiButtonCtrl>();
				if (component != null)
				{
					transform.gameObject.SetActive(true);
					component.AddOnClickListener(delegate(PguiButtonCtrl btn)
					{
						CanvasManager.HdlPurchaseConfirmWindow.Initialize("クエストの挑戦回数の回復", "キラキラ", keyItem.num, null, PurchaseConfirmWindow.TEMP_IMMEDIATE_DELIVERY, false);
					}, PguiButtonCtrl.SoundType.DEFAULT);
				}
			}
		}
		ItemData userItemData = DataManager.DmItem.GetUserItemData(keyItem.itemId);
		this.window.ReplaceTxtCaution(TimeManager.MakeTimeResidueText(this.OpenWindowTime, terminalTimeByDay, false, true));
		if (userItemData.num >= keyItem.num)
		{
			this.QuestOneId = questOneId;
		}
		this.window.owCtrl.Open();
	}

	private void Update()
	{
	}

	private IEnumerator RequestSequence()
	{
		if (this.QuestOneId == 0)
		{
			yield break;
		}
		DataManager.DmQuest.RequestActionQuestLimitRecoveryByQuestOne(this.QuestOneId);
		while (DataManager.IsServerRequesting())
		{
			yield return null;
		}
		UnityAction unityAction = this.refreshCallback;
		if (unityAction != null)
		{
			unityAction();
		}
		this.requestSequence = null;
		yield break;
	}

	private void OnDestroy()
	{
		if (this.requestSequence != null)
		{
			Singleton<SceneManager>.Instance.StopCoroutine(this.requestSequence);
		}
		this.requestSequence = null;
	}

	private SelQuestCountRecoveryWindowCtrl.Window window;

	private Coroutine requestSequence;

	private UnityAction refreshCallback;

	public class SetupGuiParts
	{
		public int itemId;

		public int needItemNum;

		public int todayRecoveryNum;

		public int recoveryMaxNum;

		public int limitClearNum;
	}

	public class Window
	{
		public Window(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Icon_Tex = baseTr.Find("Window_QuestRevival/Base/Window/RevivalOK/LayoutGroup/UseInfos/UseItem/Icon_Tex").GetComponent<PguiRawImageCtrl>();
			this.Txt_Caution = baseTr.Find("Window_QuestRevival/Base/Window/RevivalOK/LayoutGroup/Txt_Caution").GetComponent<PguiTextCtrl>();
			this.Txt_UseCount = baseTr.Find("Window_QuestRevival/Base/Window/RevivalOK/LayoutGroup/Txt_UseCount").GetComponent<PguiTextCtrl>();
			this.owCtrl = baseTr.Find("Window_QuestRevival").GetComponent<PguiOpenWindowCtrl>();
			Transform transform = baseTr.Find("Window_QuestRevival/Base/Window/RevivalOK/LayoutGroup/UseInfos/UseItem");
			this.ItemName = transform.Find("Txt").GetComponent<PguiTextCtrl>();
			this.Item_Num_BeforeTxt = transform.Find("Img_Yaji/Num_BeforeTxt").GetComponent<PguiTextCtrl>();
			this.Item_Num_AfterTxt = transform.Find("Img_Yaji/Num_AfterTxt").GetComponent<PguiTextCtrl>();
			Transform transform2 = baseTr.Find("Window_QuestRevival/Base/Window/RevivalOK/LayoutGroup/UseInfos/UseCoin");
			this.Times_Num_BeforeTxt = transform2.Find("Img_Yaji/Num_BeforeTxt").GetComponent<PguiTextCtrl>();
			this.Times_Num_AfterTxt = transform2.Find("Img_Yaji/Num_AfterTxt").GetComponent<PguiTextCtrl>();
			this.Text_None = baseTr.Find("Window_QuestRevival/Base/Window/RevivalOK/ButtonR/Text_None").GetComponent<PguiTextCtrl>();
		}

		public void SetupWindowGui(SelQuestCountRecoveryWindowCtrl.SetupGuiParts setupGuiParts)
		{
			ItemStaticBase itemStaticBase = DataManager.DmItem.GetItemStaticBase(setupGuiParts.itemId);
			ItemData userItemData = DataManager.DmItem.GetUserItemData(setupGuiParts.itemId);
			this.Icon_Tex.SetRawImage(itemStaticBase.GetIconName(), true, false, null);
			this.ItemName.text = itemStaticBase.GetName();
			this.owCtrl.MassageText.ReplaceTextByDefault(new string[] { "Param01", "Param02" }, new string[]
			{
				itemStaticBase.GetName(),
				string.Format("×{0}", setupGuiParts.needItemNum)
			});
			this.Txt_UseCount.ReplaceTextByDefault("Param01", string.Format("{0}/{1}", setupGuiParts.recoveryMaxNum - setupGuiParts.todayRecoveryNum, setupGuiParts.recoveryMaxNum));
			int num = userItemData.num - setupGuiParts.needItemNum;
			this.Text_None.gameObject.SetActive(num < 0);
			this.owCtrl.choiceR.GetComponent<PguiButtonCtrl>().SetActEnable(num >= 0, false, false);
			string text = "<color=" + PrjUtil.WARNING_COLOR_CODE + ">";
			string text2 = "</color>";
			this.Item_Num_BeforeTxt.text = ((num >= 0) ? string.Format("{0} / {1}", userItemData.num, setupGuiParts.needItemNum) : string.Format("{0}{1}{2} / {3}", new object[] { text, userItemData.num, text2, setupGuiParts.needItemNum }));
			this.Item_Num_AfterTxt.text = ((num >= 0) ? string.Format("{0}", num) : "-");
			this.Times_Num_BeforeTxt.text = "0";
			this.Times_Num_AfterTxt.text = ((num >= 0) ? string.Format("{0}", setupGuiParts.limitClearNum) : "0");
		}

		public void SetupTxtCaution(string str)
		{
			this.Txt_Caution.text = str;
		}

		public void ReplaceTxtCaution(string str)
		{
			this.Txt_Caution.ReplaceTextByDefault("Param01", str);
		}

		public GameObject baseObj;

		public PguiRawImageCtrl Icon_Tex;

		public PguiTextCtrl Txt_Caution;

		public PguiTextCtrl Txt_UseCount;

		public PguiOpenWindowCtrl owCtrl;

		public PguiTextCtrl ItemName;

		public PguiTextCtrl Item_Num_BeforeTxt;

		public PguiTextCtrl Item_Num_AfterTxt;

		public PguiTextCtrl Times_Num_BeforeTxt;

		public PguiTextCtrl Times_Num_AfterTxt;

		public PguiTextCtrl Text_None;
	}
}
