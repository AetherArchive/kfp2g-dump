using System;
using System.Collections.Generic;
using System.IO;
using SGNFW.Login;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x020001C7 RID: 455
public class WebViewWindowCtrl : MonoBehaviour
{
	// Token: 0x06001F43 RID: 8003 RVA: 0x001834A5 File Offset: 0x001816A5
	private void Start()
	{
	}

	// Token: 0x06001F44 RID: 8004 RVA: 0x001834A8 File Offset: 0x001816A8
	public void Init()
	{
		this.guiData = new WebViewWindowCtrl.GUI(base.transform);
		this.guiData.BaseAnim.ExPauseAnimation(SimpleAnimation.ExPguiStatus.START, null);
		this.guiData.InBase.SetActive(false);
		this.guiData.WebTarget.SetActive(false);
		this.guiData.WebTarget.SetReferenceResolution(new Vector2Int(SceneManager.screenSize.width, SceneManager.screenSize.height));
		this.guiData.BtnClose.AddOnClickListener(new PguiButtonCtrl.OnClick(this.OnClickButton), PguiButtonCtrl.SoundType.DEFAULT);
	}

	// Token: 0x06001F45 RID: 8005 RVA: 0x00183540 File Offset: 0x00181740
	public void Open(string url)
	{
		string text = WebViewWindowCtrl.ReplaceDmmURL.TryGetValueEx(url, url);
		if (!text.StartsWith("http"))
		{
			text = Path.Combine(LoginManager.WebViewBaseURL, text);
		}
		this.Open(text, null);
	}

	// Token: 0x06001F46 RID: 8006 RVA: 0x0018357D File Offset: 0x0018177D
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

	// Token: 0x06001F47 RID: 8007 RVA: 0x001835AE File Offset: 0x001817AE
	public void Close()
	{
		if (this.m_Sequence == WebViewWindowCtrl.Sequence.ACTIVE)
		{
			this.m_ReqSequence = WebViewWindowCtrl.Sequence.CLOSE_START;
		}
	}

	// Token: 0x06001F48 RID: 8008 RVA: 0x001835C0 File Offset: 0x001817C0
	private void OnClickButton(PguiButtonCtrl button)
	{
		if (button == this.guiData.BtnClose)
		{
			this.Close();
		}
	}

	// Token: 0x06001F49 RID: 8009 RVA: 0x001835DC File Offset: 0x001817DC
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

	// Token: 0x06001F4A RID: 8010 RVA: 0x00183797 File Offset: 0x00181997
	public bool FinishedClose()
	{
		return this.m_Sequence == WebViewWindowCtrl.Sequence.INACTIVE;
	}

	// Token: 0x040016B9 RID: 5817
	public bool isDebug;

	// Token: 0x040016BA RID: 5818
	private WebViewWindowCtrl.GUI guiData;

	// Token: 0x040016BB RID: 5819
	private WebViewWindowCtrl.Sequence m_ReqSequence;

	// Token: 0x040016BC RID: 5820
	private WebViewWindowCtrl.Sequence m_Sequence = WebViewWindowCtrl.Sequence.INACTIVE;

	// Token: 0x040016BD RID: 5821
	private UnityAction<string> m_CallBack;

	// Token: 0x040016BE RID: 5822
	private string currentURL = "";

	// Token: 0x040016BF RID: 5823
	private static readonly Dictionary<string, string> ReplaceDmmURL = new Dictionary<string, string>
	{
		{ "kiyaku/index.html", "kiyaku_dmm/index.html" },
		{ "torihiki/index.html", "torihiki_dmm/index.html" },
		{ "kessai/index.html", "kessai_dmm/index.html" }
	};

	// Token: 0x02001004 RID: 4100
	public class GUI
	{
		// Token: 0x060051AF RID: 20911 RVA: 0x002474E4 File Offset: 0x002456E4
		public GUI(Transform baseTr)
		{
			this.baseObj = baseTr.gameObject;
			this.BtnClose = baseTr.Find("Base/Window/BtnClose").GetComponent<PguiButtonCtrl>();
			this.BtnClose.androidBackKeyTarget = true;
			this.InBase = baseTr.Find("Base").gameObject;
			this.WebTarget = baseTr.Find("Base/Window/WebViewPC").GetComponent<WebViewPC>();
			this.BaseAnim = baseTr.Find("Base").GetComponent<SimpleAnimation>();
		}

		// Token: 0x040059E9 RID: 23017
		public GameObject baseObj;

		// Token: 0x040059EA RID: 23018
		public PguiButtonCtrl BtnClose;

		// Token: 0x040059EB RID: 23019
		public GameObject InBase;

		// Token: 0x040059EC RID: 23020
		public IWebView WebTarget;

		// Token: 0x040059ED RID: 23021
		public SimpleAnimation BaseAnim;
	}

	// Token: 0x02001005 RID: 4101
	private enum Sequence
	{
		// Token: 0x040059EF RID: 23023
		NONE,
		// Token: 0x040059F0 RID: 23024
		INACTIVE,
		// Token: 0x040059F1 RID: 23025
		OPEN_START,
		// Token: 0x040059F2 RID: 23026
		OPEN_WAIT,
		// Token: 0x040059F3 RID: 23027
		ACTIVE,
		// Token: 0x040059F4 RID: 23028
		CALLBACK_ACTION,
		// Token: 0x040059F5 RID: 23029
		CLOSE_START,
		// Token: 0x040059F6 RID: 23030
		CLOSE_WAIT
	}
}
