using System;
using System.Collections;
using SGNFW.Common;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001BF RID: 447
public class SelQuestCountRecoveryWindowCtrl : MonoBehaviour
{
	// Token: 0x17000429 RID: 1065
	// (get) Token: 0x06001ED9 RID: 7897 RVA: 0x0017FCFA File Offset: 0x0017DEFA
	// (set) Token: 0x06001ED8 RID: 7896 RVA: 0x0017FCF1 File Offset: 0x0017DEF1
	private int QuestOneId { get; set; }

	// Token: 0x1700042A RID: 1066
	// (get) Token: 0x06001EDA RID: 7898 RVA: 0x0017FD02 File Offset: 0x0017DF02
	// (set) Token: 0x06001EDB RID: 7899 RVA: 0x0017FD0A File Offset: 0x0017DF0A
	private DateTime OpenWindowTime { get; set; }

	// Token: 0x06001EDC RID: 7900 RVA: 0x0017FD14 File Offset: 0x0017DF14
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

	// Token: 0x06001EDD RID: 7901 RVA: 0x0017FD74 File Offset: 0x0017DF74
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

	// Token: 0x06001EDE RID: 7902 RVA: 0x0017FF4B File Offset: 0x0017E14B
	private void Update()
	{
	}

	// Token: 0x06001EDF RID: 7903 RVA: 0x0017FF4D File Offset: 0x0017E14D
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

	// Token: 0x06001EE0 RID: 7904 RVA: 0x0017FF5C File Offset: 0x0017E15C
	private void OnDestroy()
	{
		if (this.requestSequence != null)
		{
			Singleton<SceneManager>.Instance.StopCoroutine(this.requestSequence);
		}
		this.requestSequence = null;
	}

	// Token: 0x04001680 RID: 5760
	private SelQuestCountRecoveryWindowCtrl.Window window;

	// Token: 0x04001682 RID: 5762
	private Coroutine requestSequence;

	// Token: 0x04001684 RID: 5764
	private UnityAction refreshCallback;

	// Token: 0x02000FE6 RID: 4070
	public class SetupGuiParts
	{
		// Token: 0x04005953 RID: 22867
		public int itemId;

		// Token: 0x04005954 RID: 22868
		public int needItemNum;

		// Token: 0x04005955 RID: 22869
		public int todayRecoveryNum;

		// Token: 0x04005956 RID: 22870
		public int recoveryMaxNum;

		// Token: 0x04005957 RID: 22871
		public int limitClearNum;
	}

	// Token: 0x02000FE7 RID: 4071
	public class Window
	{
		// Token: 0x0600515F RID: 20831 RVA: 0x002456B0 File Offset: 0x002438B0
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

		// Token: 0x06005160 RID: 20832 RVA: 0x002457C4 File Offset: 0x002439C4
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

		// Token: 0x06005161 RID: 20833 RVA: 0x002459BF File Offset: 0x00243BBF
		public void SetupTxtCaution(string str)
		{
			this.Txt_Caution.text = str;
		}

		// Token: 0x06005162 RID: 20834 RVA: 0x002459CD File Offset: 0x00243BCD
		public void ReplaceTxtCaution(string str)
		{
			this.Txt_Caution.ReplaceTextByDefault("Param01", str);
		}

		// Token: 0x04005958 RID: 22872
		public GameObject baseObj;

		// Token: 0x04005959 RID: 22873
		public PguiRawImageCtrl Icon_Tex;

		// Token: 0x0400595A RID: 22874
		public PguiTextCtrl Txt_Caution;

		// Token: 0x0400595B RID: 22875
		public PguiTextCtrl Txt_UseCount;

		// Token: 0x0400595C RID: 22876
		public PguiOpenWindowCtrl owCtrl;

		// Token: 0x0400595D RID: 22877
		public PguiTextCtrl ItemName;

		// Token: 0x0400595E RID: 22878
		public PguiTextCtrl Item_Num_BeforeTxt;

		// Token: 0x0400595F RID: 22879
		public PguiTextCtrl Item_Num_AfterTxt;

		// Token: 0x04005960 RID: 22880
		public PguiTextCtrl Times_Num_BeforeTxt;

		// Token: 0x04005961 RID: 22881
		public PguiTextCtrl Times_Num_AfterTxt;

		// Token: 0x04005962 RID: 22882
		public PguiTextCtrl Text_None;
	}
}
