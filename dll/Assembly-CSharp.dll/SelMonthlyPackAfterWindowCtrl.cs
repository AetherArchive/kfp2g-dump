using System;
using System.Collections;
using SGNFW.Common;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001BB RID: 443
public class SelMonthlyPackAfterWindowCtrl : MonoBehaviour
{
	// Token: 0x06001E6D RID: 7789 RVA: 0x0017AD34 File Offset: 0x00178F34
	public void Init()
	{
		GameObject gameObject = Object.Instantiate<GameObject>((GameObject)Resources.Load("SelCmn/GUI/Prefab/GUI_CmnShop_PassAfter_Window"), base.transform);
		this.window = new SelMonthlyPackAfterWindowCtrl.Window(gameObject.transform);
		this.window.owCtrl.SetupByMonthlyPack(new PguiOpenWindowCtrl.Callback(this.OnClickOwButton));
		this.window.Btn_Info.AddOnClickListener(delegate(PguiButtonCtrl btn)
		{
			if (this.isActiveWindow && this.nowPack != null)
			{
				CanvasManager.HdlWebViewWindowCtrl.Open(this.nowPack.WebviewLink);
			}
		}, PguiButtonCtrl.SoundType.DEFAULT);
		this.window.Btn_Mission.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickMissionButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.window.Btn_Mission3.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickMissionButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.window.Btn_Mission4.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickMissionButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.requestStatus = SelMonthlyPackAfterWindowCtrl.Status.NONE;
		this.currentStatus = SelMonthlyPackAfterWindowCtrl.Status.NONE;
		this.isActiveWindow = false;
		this.nowPack = null;
	}

	// Token: 0x06001E6E RID: 7790 RVA: 0x0017AE19 File Offset: 0x00179019
	public void Setup()
	{
		this.isActiveWindow = true;
		this.requestStatus = SelMonthlyPackAfterWindowCtrl.Status.SETUP;
		this.nowPack = null;
	}

	// Token: 0x06001E6F RID: 7791 RVA: 0x0017AE30 File Offset: 0x00179030
	public bool IsActiveWindow()
	{
		return this.isActiveWindow;
	}

	// Token: 0x06001E70 RID: 7792 RVA: 0x0017AE38 File Offset: 0x00179038
	private void Update()
	{
		if (this.requestStatus != this.currentStatus)
		{
			this.currentStatus = this.requestStatus;
			if (this.currentStatus == SelMonthlyPackAfterWindowCtrl.Status.SETUP)
			{
				this.setupSequence = this.SetupSequence();
			}
			else if (this.currentStatus == SelMonthlyPackAfterWindowCtrl.Status.ERROR)
			{
				this.window.owCtrl.ForceClose();
				this.isActiveWindow = false;
			}
		}
		if (this.setupSequence != null && !this.setupSequence.MoveNext())
		{
			this.setupSequence = null;
		}
	}

	// Token: 0x06001E71 RID: 7793 RVA: 0x0017AEB2 File Offset: 0x001790B2
	private IEnumerator SetupSequence()
	{
		this.nowPack = DataManager.DmMonthlyPack.nowPackData.MonthlypackData;
		if (this.nowPack == null)
		{
			CanvasManager.HdlOpenWindowBasic.Setup(PrjUtil.MakeMessage("月間パスポート"), PrjUtil.MakeMessage("月間パスポートを購入していません"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, null, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
			do
			{
				yield return null;
			}
			while (!CanvasManager.HdlOpenWindowBasic.FinishedClose());
			this.requestStatus = SelMonthlyPackAfterWindowCtrl.Status.NONE;
			this.isActiveWindow = false;
			yield break;
		}
		Transform tmp = this.window.owCtrl.transform.Find("Base/Window");
		tmp.Find("Passport_1").gameObject.SetActive(this.nowPack.PackType == 1);
		tmp.Find("Passport_2").gameObject.SetActive(this.nowPack.PackType == 2);
		tmp.Find("Passport_3").gameObject.SetActive(this.nowPack.PackType == 3);
		tmp.Find("Passport_4").gameObject.SetActive(this.nowPack.PackType == 4);
		tmp.Find("Bonus_Cmn").gameObject.SetActive(this.nowPack.PackType == 1 || this.nowPack.PackType == 2);
		tmp.Find("Bonus_Cmn/Bonus05").gameObject.SetActive(this.nowPack.PackType == 1);
		tmp.Find("Bonus_03").gameObject.SetActive(this.nowPack.PackType == 3);
		tmp.Find("Bonus_04").gameObject.SetActive(this.nowPack.PackType == 4);
		tmp.Find("Passport_" + this.nowPack.PackType.ToString() + "/PassTitle/Fnt_sugoi").gameObject.SetActive(this.nowPack.PackId > 4);
		tmp.Find("Passport_" + this.nowPack.PackType.ToString() + "/PassTitle/Fnt").gameObject.SetActive(this.nowPack.PackId <= 4);
		DateTime dateTime = DataManager.DmMonthlyPack.nowPackData.EndDatetime;
		DataManagerMonthlyPack.PurchaseMonthlypackData purchaseMonthlypackData = DataManager.DmMonthlyPack.nextPackData.MonthlypackData;
		if (purchaseMonthlypackData != null)
		{
			if (DataManager.DmMonthlyPack.nextPackData.EndDatetime > dateTime)
			{
				if (this.nowPack.PackId == purchaseMonthlypackData.PackId)
				{
					dateTime = DataManager.DmMonthlyPack.nextPackData.EndDatetime;
					purchaseMonthlypackData = null;
				}
			}
			else
			{
				purchaseMonthlypackData = null;
			}
		}
		tmp.Find("Txt_Term").GetComponent<PguiTextCtrl>().text = string.Concat(new string[]
		{
			"有効期限\u3000",
			dateTime.Year.ToString(),
			"年",
			dateTime.Month.ToString(),
			"月",
			dateTime.Day.ToString(),
			"日\u300023:59まで"
		});
		Transform transform = tmp.Find("Txt_Term/Txt_Caution");
		transform.gameObject.SetActive(purchaseMonthlypackData != null);
		if (purchaseMonthlypackData != null)
		{
			dateTime = dateTime.AddDays(1.0);
			transform.GetComponent<PguiTextCtrl>().text = string.Concat(new string[]
			{
				dateTime.Year.ToString(),
				"/",
				dateTime.Month.ToString(),
				"/",
				dateTime.Day.ToString(),
				"  00:00\u3000から、",
				purchaseMonthlypackData.PackName,
				"に切り替わります。"
			});
		}
		string[] array = new string[] { "Param01", "Param02" };
		string[] array2 = new string[]
		{
			"獲得アイテム",
			PguiCmnMenuCtrl.Ratio2String((DataManager.DmMonthlyPack.nowPackData.MonthlypackData.PicnicBuffAddRatio - 100) * 100)
		};
		tmp.Find("Bonus_Cmn/Bonus02/Txt_Info").GetComponent<PguiTextCtrl>().ReplaceTextByDefault(array, array2);
		tmp.Find("Bonus_03/Bonus02/Txt_Info").GetComponent<PguiTextCtrl>().ReplaceTextByDefault(array, array2);
		tmp.Find("Bonus_04/Bonus02/Txt_Info").GetComponent<PguiTextCtrl>().ReplaceTextByDefault(array, array2);
		tmp.Find("Bonus_Cmn/Bonus02/Txt_Info").gameObject.SetActive(false);
		tmp.Find("Bonus_03/Bonus02/Txt_Info").gameObject.SetActive(false);
		tmp.Find("Bonus_04/Bonus02/Txt_Info").gameObject.SetActive(false);
		tmp.Find("Bonus_Cmn/Bonus02/Txt_Info02").gameObject.SetActive(true);
		tmp.Find("Bonus_03/Bonus02/Txt_Info02").gameObject.SetActive(true);
		tmp.Find("Bonus_04/Bonus02/Txt_Info02").gameObject.SetActive(true);
		tmp.Find("Bonus_Cmn/ContinueInfo/Txt_Continue").GetComponent<PguiTextCtrl>().ReplaceTextByDefault("Param01", "ボーナス");
		tmp.Find("Bonus_03/ContinueInfo/Txt_Continue").GetComponent<PguiTextCtrl>().ReplaceTextByDefault("Param01", "ボーナス");
		string text = "";
		if (this.nowPack.BattleRetryFlag)
		{
			text += "バトル再戦";
		}
		if (this.nowPack.SkippableFlag)
		{
			if (!string.IsNullOrEmpty(text))
			{
				text += "&";
			}
			text += "スキップ";
		}
		if (!string.IsNullOrEmpty(text))
		{
			text += "機能解放中！";
		}
		else
		{
			tmp.Find("Bonus_Cmn/Bonus_battle").gameObject.SetActive(false);
			tmp.Find("Bonus_04/Bonus_battle").gameObject.SetActive(false);
		}
		tmp.Find("Bonus_Cmn/Bonus_battle/Txt_Bonus").GetComponent<PguiTextCtrl>().text = text;
		tmp.Find("Bonus_04/Bonus_battle/Txt_Bonus").GetComponent<PguiTextCtrl>().text = text;
		int num = 0;
		DataManagerMonthlyPack.MonthlypackContinueData monthlypackContinueData = DataManager.DmMonthlyPack.monthlypackContinueDataList.Find((DataManagerMonthlyPack.MonthlypackContinueData mst) => mst.PrevMonthlyPackId == this.nowPack.PackId);
		if (monthlypackContinueData != null)
		{
			num = monthlypackContinueData.AddItemId;
		}
		ItemStaticBase itemStaticBase = DataManager.DmItem.GetItemStaticBase(num);
		tmp.Find("Bonus_Cmn/ContinueInfo/Txt_ItemName").GetComponent<PguiTextCtrl>().text = ((itemStaticBase == null) ? "" : itemStaticBase.GetName());
		tmp.Find("Bonus_03/ContinueInfo/Txt_ItemName").GetComponent<PguiTextCtrl>().text = ((itemStaticBase == null) ? "" : itemStaticBase.GetName());
		tmp.Find("Bonus_04/ContinueInfo/Grid/Txt_ItemName").GetComponent<PguiTextCtrl>().text = ((itemStaticBase == null) ? "" : itemStaticBase.GetName());
		int userClearAllSpecialMissionNum = DataManager.DmMission.GetUserClearAllSpecialMissionNum();
		this.window.Btn_Mission.transform.Find("BaseImage/Mark_Pop").gameObject.SetActive(userClearAllSpecialMissionNum > 0);
		this.window.Btn_Mission3.transform.Find("BaseImage/Mark_Pop").gameObject.SetActive(userClearAllSpecialMissionNum > 0);
		this.window.Btn_Mission4.transform.Find("BaseImage/Mark_Pop").gameObject.SetActive(userClearAllSpecialMissionNum > 0);
		this.window.owCtrl.Open();
		yield return null;
		yield return null;
		tmp.Find("Bonus_04/ContinueInfo/Grid").GetComponent<HorizontalLayoutGroup>().enabled = false;
		tmp.Find("Bonus_04/ContinueInfo/Grid").GetComponent<HorizontalLayoutGroup>().enabled = true;
		yield break;
	}

	// Token: 0x06001E72 RID: 7794 RVA: 0x0017AEC1 File Offset: 0x001790C1
	private void OnClickMissionButton(PguiButtonCtrl button)
	{
		if (this.isActiveWindow)
		{
			Singleton<SceneManager>.Instance.SetNextScene(SceneManager.SceneName.SceneMission, null);
		}
		this.requestStatus = SelMonthlyPackAfterWindowCtrl.Status.ERROR;
	}

	// Token: 0x06001E73 RID: 7795 RVA: 0x0017AEDF File Offset: 0x001790DF
	private bool OnClickOwButton(int index)
	{
		this.requestStatus = SelMonthlyPackAfterWindowCtrl.Status.NONE;
		this.isActiveWindow = false;
		return true;
	}

	// Token: 0x0400163B RID: 5691
	private SelMonthlyPackAfterWindowCtrl.Window window;

	// Token: 0x0400163C RID: 5692
	private SelMonthlyPackAfterWindowCtrl.Status requestStatus;

	// Token: 0x0400163D RID: 5693
	private SelMonthlyPackAfterWindowCtrl.Status currentStatus;

	// Token: 0x0400163E RID: 5694
	private bool isActiveWindow;

	// Token: 0x0400163F RID: 5695
	private DataManagerMonthlyPack.PurchaseMonthlypackData nowPack;

	// Token: 0x04001640 RID: 5696
	private IEnumerator setupSequence;

	// Token: 0x02000FAE RID: 4014
	public class Window
	{
		// Token: 0x06005095 RID: 20629 RVA: 0x00241D48 File Offset: 0x0023FF48
		public Window(Transform baseTr)
		{
			this.owCtrl = baseTr.GetComponent<PguiOpenWindowCtrl>();
			this.Btn_Info = baseTr.Find("Base/Window/Cmn_Btn_Info_Circle").GetComponent<PguiButtonCtrl>();
			this.Btn_Mission = baseTr.Find("Base/Window/Bonus_Cmn/Bonus01/Btn_Mission").GetComponent<PguiButtonCtrl>();
			this.Btn_Mission3 = baseTr.Find("Base/Window/Bonus_03/Bonus01/Btn_Mission").GetComponent<PguiButtonCtrl>();
			this.Btn_Mission4 = baseTr.Find("Base/Window/Bonus_04/Bonus01/Btn_Mission").GetComponent<PguiButtonCtrl>();
		}

		// Token: 0x04005876 RID: 22646
		public PguiOpenWindowCtrl owCtrl;

		// Token: 0x04005877 RID: 22647
		public PguiButtonCtrl Btn_Info;

		// Token: 0x04005878 RID: 22648
		public PguiButtonCtrl Btn_Mission;

		// Token: 0x04005879 RID: 22649
		public PguiButtonCtrl Btn_Mission3;

		// Token: 0x0400587A RID: 22650
		public PguiButtonCtrl Btn_Mission4;
	}

	// Token: 0x02000FAF RID: 4015
	public enum Status
	{
		// Token: 0x0400587C RID: 22652
		NONE,
		// Token: 0x0400587D RID: 22653
		ERROR,
		// Token: 0x0400587E RID: 22654
		SETUP
	}
}
