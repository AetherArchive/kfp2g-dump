using System;
using System.Collections.Generic;
using System.Text;
using SegaUnityCEFBrowser;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001FA RID: 506
public class WebViewPC : MonoBehaviour, IWebView
{
	// Token: 0x06002156 RID: 8534 RVA: 0x0018EBEA File Offset: 0x0018CDEA
	public int SetPreLoadCallback(PreLoadCallback func)
	{
		return this.preLoadCBs.Entry(func);
	}

	// Token: 0x06002157 RID: 8535 RVA: 0x0018EBF8 File Offset: 0x0018CDF8
	public void UnsetPreLoadCallback(int hndl)
	{
		this.preLoadCBs.Remove(hndl);
	}

	// Token: 0x06002158 RID: 8536 RVA: 0x0018EC06 File Offset: 0x0018CE06
	public int SetPostLoadCallback(PostLoadCallback func)
	{
		return this.postLoadCBs.Entry(func);
	}

	// Token: 0x06002159 RID: 8537 RVA: 0x0018EC14 File Offset: 0x0018CE14
	public void UnsetPostLoadCallback(int hndl)
	{
		this.postLoadCBs.Remove(hndl);
	}

	// Token: 0x0600215A RID: 8538 RVA: 0x0018EC22 File Offset: 0x0018CE22
	public void SetReferenceResolution(Vector2Int reso)
	{
		this.CONF_RESO_X = reso.x;
		this.CONF_RESO_Y = reso.y;
	}

	// Token: 0x0600215B RID: 8539 RVA: 0x0018EC3E File Offset: 0x0018CE3E
	public void SetMargins(int left, int top, int right, int bottom)
	{
		this.CONF_MARGIN_LEFT = left;
		this.CONF_MARGIN_TOP = top;
		this.CONF_MARGIN_RIGHT = right;
		this.CONF_MARGIN_BOTTOM = bottom;
	}

	// Token: 0x0600215C RID: 8540 RVA: 0x0018EC5D File Offset: 0x0018CE5D
	public void SetURL(string url)
	{
		this.CONF_URL = url;
		this.LoadURL();
	}

	// Token: 0x0600215D RID: 8541 RVA: 0x0018EC6C File Offset: 0x0018CE6C
	public void LoadURL()
	{
		this.Init();
		int num = this.CONF_RESO_X - (this.CONF_MARGIN_LEFT + this.CONF_MARGIN_RIGHT);
		int num2 = this.CONF_RESO_Y - (this.CONF_MARGIN_TOP + this.CONF_MARGIN_BOTTOM);
		this.DEBUG_RESO_X = num;
		this.DEBUG_RESO_Y = num2;
		Vector3 vector;
		vector.x = (float)((this.CONF_MARGIN_LEFT + num / 2 - this.CONF_RESO_X / 2) * 2);
		vector.y = (float)((this.CONF_MARGIN_TOP + num2 / 2 - this.CONF_RESO_Y / 2) * -2);
		vector.z = 0f;
		this.browserInst.Resize(num, num2);
		if (this.bDependCanvasScaler)
		{
			this.browserInst._WWWCamera.orthographicSize = 200f;
		}
		else
		{
			this.browserInst._WWWCamera.orthographicSize = (float)this.CONF_RESO_Y;
		}
		this.browserInst._MeshCollider.transform.localPosition = vector;
		this.browserInst.SetRequestHeader("FrameWidth", num.ToString());
		this.browserInst.SetRequestHeader("DeviceWidth", num.ToString());
		string text = "Mozilla / 5.0(Windows NT 6.3; WOW64) AppleWebKit / 537.36(KHTML, like Gecko) Chrome / 45.0.2454.99 Safari / 537.36";
		this.browserInst.SetRequestHeader("User-Agent", text);
		this.browserInst.setOnBeforeBrowse(delegate(int id, string url)
		{
			int num3 = 0;
			foreach (KeyValuePair<int, PreLoadCallback> keyValuePair in this.preLoadCBs.funcs)
			{
				num3 = keyValuePair.Value(url);
			}
			return num3;
		});
		this.browserInst.setOnLoadEnd(delegate(int id, string url, int status)
		{
			int num4 = 0;
			foreach (KeyValuePair<int, PostLoadCallback> keyValuePair2 in this.postLoadCBs.funcs)
			{
				num4 = keyValuePair2.Value(url, status);
			}
			return num4;
		});
		this.browserInst.LoadURL(this.CONF_URL);
	}

	// Token: 0x0600215E RID: 8542 RVA: 0x0018EDDC File Offset: 0x0018CFDC
	private int OnClickWebView(string msg)
	{
		int num = 0;
		foreach (KeyValuePair<string, string> keyValuePair in WebViewComponent.GetCommandMap(msg))
		{
			if (keyValuePair.Key == "openurl")
			{
				this.requestOpenExternalBrowserUrl = keyValuePair.Value;
				num = 1;
			}
		}
		return num;
	}

	// Token: 0x0600215F RID: 8543 RVA: 0x0018EE50 File Offset: 0x0018D050
	private void Start()
	{
		this.Init();
	}

	// Token: 0x06002160 RID: 8544 RVA: 0x0018EE58 File Offset: 0x0018D058
	private void Init()
	{
		if (this.browserInst != null)
		{
			return;
		}
		Transform transform = base.transform;
		while (transform != null)
		{
			CanvasScaler component = transform.GetComponent<CanvasScaler>();
			if (component != null)
			{
				this.CONF_RESO_X = (int)component.referenceResolution.x;
				this.CONF_RESO_Y = (int)component.referenceResolution.y;
				this.bDependCanvasScaler = true;
				break;
			}
			transform = transform.parent;
		}
		transform = base.transform;
		while (transform != null)
		{
			CanvasScaler component2 = transform.GetComponent<CanvasScaler>();
			RectTransform component3 = transform.GetComponent<RectTransform>();
			if (component3 != null && component2 == null)
			{
				this.CONF_MARGIN_LEFT = (int)component3.offsetMin.x;
				this.CONF_MARGIN_TOP = -(int)component3.offsetMax.y;
				this.CONF_MARGIN_RIGHT = -(int)component3.offsetMax.x;
				this.CONF_MARGIN_BOTTOM = (int)component3.offsetMin.y;
				break;
			}
			transform = transform.parent;
		}
		this.browserInst = base.gameObject.GetComponentInChildren<Browser>();
		this.browserInst.gameObject.SetActive(false);
		this.browserInst._WWWCamera.depth = 100f;
		this.isSetupCb = false;
	}

	// Token: 0x06002161 RID: 8545 RVA: 0x0018EF90 File Offset: 0x0018D190
	private void Update()
	{
		Vector3 mousePosition = Input.mousePosition;
		float num = (float)this.CONF_RESO_X / (float)Screen.width;
		float num2 = (float)this.CONF_RESO_Y / (float)Screen.height;
		mousePosition.y = (float)Screen.height - mousePosition.y;
		mousePosition.x *= num;
		mousePosition.y *= num2;
		mousePosition.x -= (float)this.CONF_MARGIN_LEFT;
		mousePosition.y -= (float)this.CONF_MARGIN_TOP;
		if (this.DEBUG_TEXT != null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("DISPLAY SIZE    :").Append(Screen.width).Append("x")
				.Append(Screen.height)
				.Append("\n");
			stringBuilder.Append("BROWSER RESO    :").Append(this.DEBUG_RESO_X).Append("x")
				.Append(this.DEBUG_RESO_Y)
				.Append("\n");
			stringBuilder.Append("POS(vs BROWSER) :").Append(mousePosition.x).Append(":")
				.Append(mousePosition.y)
				.Append("\n");
			this.DEBUG_TEXT.text = stringBuilder.ToString();
		}
		if (Input.GetMouseButtonDown(0))
		{
			this.browserInst.InputMouseClick((int)mousePosition.x, (int)mousePosition.y, false);
		}
		else if (Input.GetMouseButtonUp(0))
		{
			this.browserInst.InputMouseClick((int)mousePosition.x, (int)mousePosition.y, true);
		}
		else
		{
			this.browserInst.InputMouseMove((int)mousePosition.x, (int)mousePosition.y);
		}
		float axis = Input.GetAxis("Mouse ScrollWheel");
		if (axis > 0f)
		{
			this.browserInst.InputKeyEvent(38U, false);
			this.browserInst.InputKeyEvent(38U, true);
			this.browserInst.InputKeyEvent(38U, false);
			this.browserInst.InputKeyEvent(38U, true);
		}
		else if (axis < 0f)
		{
			this.browserInst.InputKeyEvent(40U, false);
			this.browserInst.InputKeyEvent(40U, true);
			this.browserInst.InputKeyEvent(40U, false);
			this.browserInst.InputKeyEvent(40U, true);
		}
		if (Input.GetKey(KeyCode.PageDown))
		{
			this.browserInst.InputKeyEvent(40U, false);
		}
		if (Input.GetKeyUp(KeyCode.PageDown))
		{
			this.browserInst.InputKeyEvent(40U, true);
		}
		if (Input.GetKey(KeyCode.PageUp))
		{
			this.browserInst.InputKeyEvent(38U, false);
		}
		if (Input.GetKeyUp(KeyCode.PageUp))
		{
			this.browserInst.InputKeyEvent(38U, true);
		}
		if (!this.isSetupCb && this.browserInst.IsReady)
		{
			this.SetPreLoadCallback(new PreLoadCallback(this.OnClickWebView));
			this.isSetupCb = true;
		}
		if (!string.IsNullOrEmpty(this.requestOpenExternalBrowserUrl))
		{
			Application.OpenURL(this.requestOpenExternalBrowserUrl);
			this.requestOpenExternalBrowserUrl = "";
		}
	}

	// Token: 0x06002162 RID: 8546 RVA: 0x0018F286 File Offset: 0x0018D486
	private void OnDestroy()
	{
	}

	// Token: 0x06002163 RID: 8547 RVA: 0x0018F288 File Offset: 0x0018D488
	private void OnEnable()
	{
		if (this.browserInst != null)
		{
			this.browserInst.gameObject.SetActive(true);
		}
	}

	// Token: 0x06002164 RID: 8548 RVA: 0x0018F2A9 File Offset: 0x0018D4A9
	private void OnDisable()
	{
		if (this.browserInst != null)
		{
			this.browserInst.gameObject.SetActive(false);
		}
	}

	// Token: 0x06002165 RID: 8549 RVA: 0x0018F2CA File Offset: 0x0018D4CA
	public void SetActive(bool active)
	{
		base.gameObject.SetActive(active);
	}

	// Token: 0x040017FD RID: 6141
	public string CONF_URL = "";

	// Token: 0x040017FE RID: 6142
	public int CONF_RESO_X = 1280;

	// Token: 0x040017FF RID: 6143
	public int CONF_RESO_Y = 720;

	// Token: 0x04001800 RID: 6144
	public int CONF_MARGIN_LEFT;

	// Token: 0x04001801 RID: 6145
	public int CONF_MARGIN_TOP;

	// Token: 0x04001802 RID: 6146
	public int CONF_MARGIN_RIGHT;

	// Token: 0x04001803 RID: 6147
	public int CONF_MARGIN_BOTTOM;

	// Token: 0x04001804 RID: 6148
	private bool bDependCanvasScaler;

	// Token: 0x04001805 RID: 6149
	private Browser browserInst;

	// Token: 0x04001806 RID: 6150
	public Text DEBUG_TEXT;

	// Token: 0x04001807 RID: 6151
	private int DEBUG_RESO_X;

	// Token: 0x04001808 RID: 6152
	private int DEBUG_RESO_Y;

	// Token: 0x04001809 RID: 6153
	private bool isSetupCb;

	// Token: 0x0400180A RID: 6154
	private string requestOpenExternalBrowserUrl = "";

	// Token: 0x0400180B RID: 6155
	private WebViewPC.CallbackHolder<PreLoadCallback> preLoadCBs = new WebViewPC.CallbackHolder<PreLoadCallback>();

	// Token: 0x0400180C RID: 6156
	private WebViewPC.CallbackHolder<PostLoadCallback> postLoadCBs = new WebViewPC.CallbackHolder<PostLoadCallback>();

	// Token: 0x02001044 RID: 4164
	private class CallbackHolder<T>
	{
		// Token: 0x06005293 RID: 21139 RVA: 0x00249118 File Offset: 0x00247318
		public int Entry(T func)
		{
			Dictionary<int, T> dictionary = this.funcs;
			int num = this.sequencer + 1;
			this.sequencer = num;
			dictionary.Add(num, func);
			return this.sequencer;
		}

		// Token: 0x06005294 RID: 21140 RVA: 0x00249148 File Offset: 0x00247348
		public void Remove(int hndl)
		{
			this.funcs.Remove(hndl);
		}

		// Token: 0x04005B48 RID: 23368
		public Dictionary<int, T> funcs = new Dictionary<int, T>();

		// Token: 0x04005B49 RID: 23369
		private int sequencer;
	}
}
