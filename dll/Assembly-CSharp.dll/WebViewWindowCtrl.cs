using System;
using System.Collections.Generic;
using System.IO;
using SGNFW.Login;
using UnityEngine;
using UnityEngine.Events;

public class WebViewWindowCtrl : MonoBehaviour
{
	private void Start()
	{
	}

	public void Init()
	{
		this.guiData = new WebViewWindowCtrl.GUI(base.transform);
		this.guiData.BaseAnim.ExPauseAnimation(SimpleAnimation.ExPguiStatus.START, null);
		this.guiData.InBase.SetActive(false);
		this.guiData.WebTarget.SetActive(false);
		this.guiData.WebTarget.SetReferenceResolution(new Vector2Int(SceneManager.screenSize.width, SceneManager.screenSize.height));
		this.guiData.BtnClose.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
	}

	public void Open(string url)
	{
		string text = WebViewWindowCtrl.ReplaceDmmURL.TryGetValueEx(url, url);
		if (!text.StartsWith("http"))
		{
			text = Path.Combine(LoginManager.WebViewBaseURL, text);
		}
		this.Open(text, null);
	}

	public void Open(string url, UnityAction<string> cb)
	{
		if (this.m_Sequence == WebViewWindowCtrl.Sequence.INACTIVE)
		{
			this.currentURL = url;
			this.m_Sequence = WebViewWindowCtrl.Sequence.OPEN_START;
			this.guiData.WebTarget.SetURL(url);
			this.m_CallBack = cb;
		}
	}

	public void Close()
	{
		if (this.m_Sequence == WebViewWindowCtrl.Sequence.ACTIVE)
		{
			this.m_ReqSequence = WebViewWindowCtrl.Sequence.CLOSE_START;
		}
	}

	private void OnClickButton(PguiButtonCtrl button)
	{
		if (button == this.guiData.BtnClose)
		{
			this.Close();
		}
	}

	private void Update()
	{
		switch (this.m_Sequence)
		{
		case WebViewWindowCtrl.Sequence.INACTIVE:
			if (this.m_ReqSequence == WebViewWindowCtrl.Sequence.OPEN_START)
			{
				this.m_Sequence = WebViewWindowCtrl.Sequence.OPEN_START;
			}
			break;
		case WebViewWindowCtrl.Sequence.OPEN_START:
			this.guiData.InBase.SetActive(true);
			this.guiData.BaseAnim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.START, null);
			this.m_Sequence = WebViewWindowCtrl.Sequence.OPEN_WAIT;
			SoundManager.Play("prd_se_dialog_disp", false, false);
			break;
		case WebViewWindowCtrl.Sequence.OPEN_WAIT:
			if (!this.guiData.BaseAnim.ExIsPlaying())
			{
				this.guiData.WebTarget.SetURL(this.currentURL);
				this.guiData.WebTarget.SetActive(true);
				this.m_Sequence = WebViewWindowCtrl.Sequence.ACTIVE;
			}
			break;
		case WebViewWindowCtrl.Sequence.ACTIVE:
			if (this.m_ReqSequence == WebViewWindowCtrl.Sequence.CALLBACK_ACTION)
			{
				SoundManager.Play("prd_se_dialog_close", false, false);
				this.m_Sequence = WebViewWindowCtrl.Sequence.CALLBACK_ACTION;
			}
			else if (this.m_ReqSequence == WebViewWindowCtrl.Sequence.CLOSE_START)
			{
				SoundManager.Play("prd_se_dialog_close", false, false);
				this.m_Sequence = WebViewWindowCtrl.Sequence.CLOSE_START;
			}
			else if (CanvasManager.winClose != 0)
			{
				SoundManager.Play("prd_se_dialog_close", false, false);
				this.m_Sequence = WebViewWindowCtrl.Sequence.CLOSE_START;
			}
			break;
		case WebViewWindowCtrl.Sequence.CALLBACK_ACTION:
			if (this.m_CallBack != null)
			{
				this.m_CallBack("");
			}
			this.m_Sequence = WebViewWindowCtrl.Sequence.CLOSE_START;
			break;
		case WebViewWindowCtrl.Sequence.CLOSE_START:
			this.guiData.BaseAnim.ExPlayAnimation(SimpleAnimation.ExPguiStatus.END, null);
			this.m_Sequence = WebViewWindowCtrl.Sequence.CLOSE_WAIT;
			this.guiData.WebTarget.SetActive(false);
			break;
		case WebViewWindowCtrl.Sequence.CLOSE_WAIT:
			if (!this.guiData.BaseAnim.ExIsPlaying())
			{
				this.guiData.InBase.SetActive(false);
				this.m_Sequence = WebViewWindowCtrl.Sequence.INACTIVE;
			}
			break;
		}
		this.m_ReqSequence = WebViewWindowCtrl.Sequence.NONE;
	}

	public bool FinishedClose()
	{
		return this.m_Sequence == WebViewWindowCtrl.Sequence.INACTIVE;
	}

	public bool isDebug;

	private WebViewWindowCtrl.GUI guiData;

	private WebViewWindowCtrl.Sequence m_ReqSequence;

	private WebViewWindowCtrl.Sequence m_Sequence = WebViewWindowCtrl.Sequence.INACTIVE;

	private UnityAction<string> m_CallBack;

	private string currentURL = "";

	private static readonly Dictionary<string, string> ReplaceDmmURL = new Dictionary<string, string>
	{
		{ "kiyaku/index.html", "kiyaku_dmm/index.html" },
		{ "torihiki/index.html", "torihiki_dmm/index.html" },
		{ "kessai/index.html", "kessai_dmm/index.html" }
	};

	public class GUI
	{
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.BtnClose = baseTr.Find("Base/Window/BtnClose").GetComponent<PguiButtonCtrl>();
			this.BtnClose.androidBackKeyTarget = true;
			this.InBase = baseTr.Find("Base").gameObject;
			this.WebTarget = baseTr.Find("Base/Window/WebViewPC").GetComponent<WebViewPC>();
			this.BaseAnim = baseTr.Find("Base").GetComponent<SimpleAnimation>();
		}

		public GameObject baseObj;

		public PguiButtonCtrl BtnClose;

		public GameObject InBase;

		public IWebView WebTarget;

		public SimpleAnimation BaseAnim;
	}

	private enum Sequence
	{
		NONE,
		INACTIVE,
		OPEN_START,
		OPEN_WAIT,
		ACTIVE,
		CALLBACK_ACTION,
		CLOSE_START,
		CLOSE_WAIT
	}
}
