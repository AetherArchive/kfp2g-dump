using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.Http;
using SGNFW.Login;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000164 RID: 356
public class SelTransferCtrl : MonoBehaviour
{
	// Token: 0x1700038B RID: 907
	// (get) Token: 0x060014D1 RID: 5329 RVA: 0x000FD5CE File Offset: 0x000FB7CE
	// (set) Token: 0x060014D0 RID: 5328 RVA: 0x000FD5BA File Offset: 0x000FB7BA
	private IEnumerator currentEnumerator
	{
		get
		{
			return this._currentEnumerator;
		}
		set
		{
			if (value == null || this._currentEnumerator == null)
			{
				this._currentEnumerator = value;
			}
		}
	}

	// Token: 0x1700038C RID: 908
	// (get) Token: 0x060014D2 RID: 5330 RVA: 0x000FD5D6 File Offset: 0x000FB7D6
	public PguiTextCtrl InfoMassageTextCtrl
	{
		get
		{
			return this.guiData.Massage01;
		}
	}

	// Token: 0x060014D3 RID: 5331 RVA: 0x000FD5E3 File Offset: 0x000FB7E3
	private void Update()
	{
		if (this.currentEnumerator != null && !this.currentEnumerator.MoveNext())
		{
			this.currentEnumerator = null;
		}
	}

	// Token: 0x060014D4 RID: 5332 RVA: 0x000FD601 File Offset: 0x000FB801
	private void OnDestroy()
	{
		if (this.guiPassWindow != null)
		{
			Object.Destroy(this.guiPassWindow.baseObj);
		}
	}

	// Token: 0x060014D5 RID: 5333 RVA: 0x000FD61C File Offset: 0x000FB81C
	public void Init()
	{
		this.guiData = new SelTransferCtrl.GUI(Object.Instantiate<GameObject>(Resources.Load("SceneMenu/GUI/Prefab/GUI_DataMigration") as GameObject, base.transform).transform);
		this.guiPassWindow = new SelTransferCtrl.GuiPassWindow(Object.Instantiate<GameObject>(Resources.Load("SceneMenu/GUI/Prefab/GUI_DataMigration_Pass_Window") as GameObject, Singleton<CanvasManager>.Instance.SystemMiddleArea).transform);
		this.guiData.Btn_Copy.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Btn_Pass.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
	}

	// Token: 0x060014D6 RID: 5334 RVA: 0x000FD6BC File Offset: 0x000FB8BC
	public void Setup(bool isAnime)
	{
		this.guiData.Txt_UserName.text = LoginManager.TransferId;
		this.guiData.Txt_UserNameRuby.text = SelTransferCtrl.MakeTransferIdRuby(LoginManager.TransferId);
		this.guiData.Txt_UserPass.text = (LoginManager.IsSettingTransferPassword ? "設定済み" : "未設定");
		if (isAnime)
		{
			this.guiData.BaseAnime.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
		}
		this.isDisableAction = false;
	}

	// Token: 0x060014D7 RID: 5335 RVA: 0x000FD738 File Offset: 0x000FB938
	public static string MakeTransferIdRuby(string transferId)
	{
		string text = string.Empty;
		foreach (char c in transferId)
		{
			if (SelTransferCtrl.passRuby.ContainsKey(c.ToString()))
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += "・";
				}
				text += SelTransferCtrl.passRuby[c.ToString()];
			}
		}
		return text;
	}

	// Token: 0x060014D8 RID: 5336 RVA: 0x000FD7A8 File Offset: 0x000FB9A8
	private void OnClickButton(PguiButtonCtrl button)
	{
		if (button == this.guiData.Btn_Pass)
		{
			this.currentEnumerator = this.SetupTransferPassword();
			return;
		}
		if (button == this.guiData.Btn_Copy)
		{
			GUIUtility.systemCopyBuffer = LoginManager.TransferId;
			CanvasManager.HdlOpenWindowBasic.Setup("", PrjUtil.MakeMessage("コピーしました"), PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, null, null, false);
			CanvasManager.HdlOpenWindowBasic.Open();
		}
	}

	// Token: 0x060014D9 RID: 5337 RVA: 0x000FD81F File Offset: 0x000FBA1F
	public void OnClickReturnButton()
	{
		if (!this.isDisableAction)
		{
			this.isDisableAction = true;
			this.currentEnumerator = this.DisableAction();
		}
	}

	// Token: 0x060014DA RID: 5338 RVA: 0x000FD83C File Offset: 0x000FBA3C
	private IEnumerator DisableAction()
	{
		yield return null;
		this.guiData.BaseAnime.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
		{
			CanvasManager.HdlCmnMenu.MoveSceneByMenu(SceneManager.SceneName.SceneOtherMenuTop, null);
		});
		yield break;
	}

	// Token: 0x060014DB RID: 5339 RVA: 0x000FD84B File Offset: 0x000FBA4B
	private IEnumerator SetupTransferPassword()
	{
		bool isFinish = false;
		bool isRequest = false;
		PguiOpenWindowCtrl.Callback callback = delegate(int index)
		{
			if (index == 0 || index == PguiOpenWindowCtrl.CLOSE_BUTTON_INDEX)
			{
				isFinish = true;
				return true;
			}
			bool flag = true;
			if (string.IsNullOrEmpty(this.guiPassWindow.InputFieldPass.text) || string.IsNullOrEmpty(this.guiPassWindow.InputFieldPass2.text))
			{
				this.guiPassWindow.errorText.text = PrjUtil.MakeMessage("未入力の項目があります");
				return false;
			}
			if (this.guiPassWindow.InputFieldPass.text.Length < 8 || 15 < this.guiPassWindow.InputFieldPass.text.Length)
			{
				this.guiPassWindow.errorText.text = PrjUtil.MakeMessage("パスワードの文字数が8～15文字ではありません");
				return false;
			}
			if (this.guiPassWindow.InputFieldPass.text != this.guiPassWindow.InputFieldPass2.text)
			{
				this.guiPassWindow.errorText.text = PrjUtil.MakeMessage("確認用パスワードが違います");
				return false;
			}
			if (flag)
			{
				isRequest = true;
				isFinish = true;
			}
			return flag;
		};
		this.guiPassWindow.InputFieldPass.text = "";
		this.guiPassWindow.InputFieldPass2.text = "";
		this.guiPassWindow.errorText.text = "";
		this.guiPassWindow.NameWindow.Setup(null, null, PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CANCEL_SET), true, callback, null, false);
		this.guiPassWindow.NameWindow.Open();
		while (!isFinish)
		{
			yield return null;
		}
		if (!isRequest)
		{
			yield break;
		}
		SelTransferCtrl.<>c__DisplayClass19_1 CS$<>8__locals2 = new SelTransferCtrl.<>c__DisplayClass19_1();
		CS$<>8__locals2.isFinish = false;
		Singleton<LoginManager>.Instance.AccountTransferPassword(delegate(Command cmd)
		{
			CS$<>8__locals2.isFinish = true;
		}, this.guiPassWindow.InputFieldPass.text);
		while (!CS$<>8__locals2.isFinish)
		{
			yield return null;
		}
		CS$<>8__locals2 = null;
		SelTransferCtrl.<>c__DisplayClass19_2 CS$<>8__locals3 = new SelTransferCtrl.<>c__DisplayClass19_2();
		CS$<>8__locals3.isFinish = false;
		CanvasManager.HdlOpenWindowBasic.Setup("確認", "パスワードを設定しました", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), true, delegate(int index)
		{
			CS$<>8__locals3.isFinish = true;
			return true;
		}, null, false);
		CanvasManager.HdlOpenWindowBasic.Open();
		this.guiData.Txt_UserPass.text = PrjUtil.MakeMessage("設定済み");
		while (!CS$<>8__locals3.isFinish)
		{
			yield return null;
		}
		CS$<>8__locals3 = null;
		yield return null;
		yield break;
	}

	// Token: 0x040010F5 RID: 4341
	private IEnumerator _currentEnumerator;

	// Token: 0x040010F6 RID: 4342
	private SelTransferCtrl.GUI guiData;

	// Token: 0x040010F7 RID: 4343
	private SelTransferCtrl.GuiPassWindow guiPassWindow;

	// Token: 0x040010F8 RID: 4344
	private bool isDisableAction;

	// Token: 0x040010F9 RID: 4345
	private static readonly Dictionary<string, string> passRuby = new Dictionary<string, string>
	{
		{ "a", "エイ" },
		{ "b", "ビー" },
		{ "c", "シー" },
		{ "d", "ディー" },
		{ "e", "イー" },
		{ "f", "エフ" },
		{ "g", "ジー" },
		{ "h", "エイチ" },
		{ "i", "アイ" },
		{ "j", "ジェイ" },
		{ "k", "ケイ" },
		{ "l", "エル" },
		{ "m", "エム" },
		{ "n", "エヌ" },
		{ "o", "オー" },
		{ "p", "ピー" },
		{ "q", "キュー" },
		{ "r", "アール" },
		{ "s", "エス" },
		{ "t", "ティー" },
		{ "u", "ユー" },
		{ "v", "ブイ" },
		{ "w", "ダブリュー" },
		{ "x", "エックス" },
		{ "y", "ワイ" },
		{ "z", "ゼット" },
		{ "A", "エイ" },
		{ "B", "ビー" },
		{ "C", "シー" },
		{ "D", "ディー" },
		{ "E", "イー" },
		{ "F", "エフ" },
		{ "G", "ジー" },
		{ "H", "エイチ" },
		{ "I", "アイ" },
		{ "J", "ジェイ" },
		{ "K", "ケイ" },
		{ "L", "エル" },
		{ "M", "エム" },
		{ "N", "エヌ" },
		{ "O", "オー" },
		{ "P", "ピー" },
		{ "Q", "キュー" },
		{ "R", "アール" },
		{ "S", "エス" },
		{ "T", "ティー" },
		{ "U", "ユー" },
		{ "V", "ブイ" },
		{ "W", "ダブリュー" },
		{ "X", "エックス" },
		{ "Y", "ワイ" },
		{ "Z", "ゼット" }
	};

	// Token: 0x02000BA4 RID: 2980
	public class GUI
	{
		// Token: 0x0600438D RID: 17293 RVA: 0x002033FC File Offset: 0x002015FC
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.Btn_Copy = baseTr.Find("All/All/WindowAll/base/Btn_Copy").GetComponent<PguiButtonCtrl>();
			this.Btn_Pass = baseTr.Find("All/All/WindowAll/base/Btn_Pass").GetComponent<PguiButtonCtrl>();
			this.Txt_UserName = baseTr.Find("All/All/WindowAll/base/Img_line_1/Txt_UserName").GetComponent<PguiTextCtrl>();
			this.Txt_UserNameRuby = baseTr.Find("All/All/WindowAll/base/Img_line_1/Txt_Ruby").GetComponent<PguiTextCtrl>();
			this.Txt_UserPass = baseTr.Find("All/All/WindowAll/base/Img_line_2/Txt_UserName").GetComponent<PguiTextCtrl>();
			this.BaseAnime = baseTr.GetComponent<SimpleAnimation>();
			this.Massage01 = baseTr.Find("All/All/WindowAll/Txt1").GetComponent<PguiTextCtrl>();
		}

		// Token: 0x0400484D RID: 18509
		public GameObject baseObj;

		// Token: 0x0400484E RID: 18510
		public PguiButtonCtrl Btn_Copy;

		// Token: 0x0400484F RID: 18511
		public PguiButtonCtrl Btn_Pass;

		// Token: 0x04004850 RID: 18512
		public PguiTextCtrl Txt_UserName;

		// Token: 0x04004851 RID: 18513
		public PguiTextCtrl Txt_UserNameRuby;

		// Token: 0x04004852 RID: 18514
		public PguiTextCtrl Txt_UserPass;

		// Token: 0x04004853 RID: 18515
		public SimpleAnimation BaseAnime;

		// Token: 0x04004854 RID: 18516
		public PguiTextCtrl Massage01;
	}

	// Token: 0x02000BA5 RID: 2981
	public class GuiPassWindow
	{
		// Token: 0x0600438E RID: 17294 RVA: 0x002034AC File Offset: 0x002016AC
		public GuiPassWindow(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.InputFieldPass = baseTr.Find("NameWindow/Base/Window/Input_01/InputField").GetComponent<InputField>();
			this.InputFieldPass2 = baseTr.Find("NameWindow/Base/Window/Input_02/InputField").GetComponent<InputField>();
			this.NameWindow = baseTr.Find("NameWindow").GetComponent<PguiOpenWindowCtrl>();
			PguiTextCtrl pguiTextCtrl;
			if (baseTr == null)
			{
				pguiTextCtrl = null;
			}
			else
			{
				Transform transform = baseTr.Find("NameWindow/Base/Window/Caution_02");
				pguiTextCtrl = ((transform != null) ? transform.GetComponent<PguiTextCtrl>() : null);
			}
			this.errorText = pguiTextCtrl;
		}

		// Token: 0x04004855 RID: 18517
		public GameObject baseObj;

		// Token: 0x04004856 RID: 18518
		public InputField InputFieldPass;

		// Token: 0x04004857 RID: 18519
		public InputField InputFieldPass2;

		// Token: 0x04004858 RID: 18520
		public PguiOpenWindowCtrl NameWindow;

		// Token: 0x04004859 RID: 18521
		public PguiTextCtrl errorText;
	}
}
