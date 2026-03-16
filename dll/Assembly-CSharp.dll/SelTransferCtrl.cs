using System;
using System.Collections;
using System.Collections.Generic;
using SGNFW.Common;
using SGNFW.Http;
using SGNFW.Login;
using UnityEngine;
using UnityEngine.UI;

public class SelTransferCtrl : MonoBehaviour
{
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

	public PguiTextCtrl InfoMassageTextCtrl
	{
		get
		{
			return this.guiData.Massage01;
		}
	}

	private void Update()
	{
		if (this.currentEnumerator != null && !this.currentEnumerator.MoveNext())
		{
			this.currentEnumerator = null;
		}
	}

	private void OnDestroy()
	{
		if (this.guiPassWindow != null)
		{
			Object.Destroy(this.guiPassWindow.baseObj);
		}
	}

	public void Init()
	{
		this.guiData = new SelTransferCtrl.GUI(Object.Instantiate<GameObject>(Resources.Load("SceneMenu/GUI/Prefab/GUI_DataMigration") as GameObject, base.transform).transform);
		this.guiPassWindow = new SelTransferCtrl.GuiPassWindow(Object.Instantiate<GameObject>(Resources.Load("SceneMenu/GUI/Prefab/GUI_DataMigration_Pass_Window") as GameObject, Singleton<CanvasManager>.Instance.SystemMiddleArea).transform);
		this.guiData.Btn_Copy.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
		this.guiData.Btn_Pass.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
	}

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

	public void OnClickReturnButton()
	{
		if (!this.isDisableAction)
		{
			this.isDisableAction = true;
			this.currentEnumerator = this.DisableAction();
		}
	}

	private IEnumerator DisableAction()
	{
		yield return null;
		this.guiData.BaseAnime.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, delegate
		{
			CanvasManager.HdlCmnMenu.MoveSceneByMenu(SceneManager.SceneName.SceneOtherMenuTop, null);
		});
		yield break;
	}

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

	private IEnumerator _currentEnumerator;

	private SelTransferCtrl.GUI guiData;

	private SelTransferCtrl.GuiPassWindow guiPassWindow;

	private bool isDisableAction;

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

	public class GUI
	{
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

		public GameObject baseObj;

		public PguiButtonCtrl Btn_Copy;

		public PguiButtonCtrl Btn_Pass;

		public PguiTextCtrl Txt_UserName;

		public PguiTextCtrl Txt_UserNameRuby;

		public PguiTextCtrl Txt_UserPass;

		public SimpleAnimation BaseAnime;

		public PguiTextCtrl Massage01;
	}

	public class GuiPassWindow
	{
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

		public GameObject baseObj;

		public InputField InputFieldPass;

		public InputField InputFieldPass2;

		public PguiOpenWindowCtrl NameWindow;

		public PguiTextCtrl errorText;
	}
}
