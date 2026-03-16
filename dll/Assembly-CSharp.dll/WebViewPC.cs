using System;
using System.Collections.Generic;
using System.Text;
using SegaUnityCEFBrowser;
using UnityEngine;
using UnityEngine.UI;

public class WebViewPC : MonoBehaviour, IWebView
{
	public int SetPreLoadCallback(PreLoadCallback func)
	{
		return this.preLoadCBs.Entry(func);
	}

	public void UnsetPreLoadCallback(int hndl)
	{
		this.preLoadCBs.Remove(hndl);
	}

	public int SetPostLoadCallback(PostLoadCallback func)
	{
		return this.postLoadCBs.Entry(func);
	}

	public void UnsetPostLoadCallback(int hndl)
	{
		this.postLoadCBs.Remove(hndl);
	}

	public void SetReferenceResolution(Vector2Int reso)
	{
		this.CONF_RESO_X = reso.x;
		this.CONF_RESO_Y = reso.y;
	}

	public void SetMargins(int left, int top, int right, int bottom)
	{
		this.CONF_MARGIN_LEFT = left;
		this.CONF_MARGIN_TOP = top;
		this.CONF_MARGIN_RIGHT = right;
		this.CONF_MARGIN_BOTTOM = bottom;
	}

	public void SetURL(string url)
	{
		this.CONF_URL = url;
		this.LoadURL();
	}

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

	private void Start()
	{
		this.Init();
	}

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

	private void OnDestroy()
	{
	}

	private void OnEnable()
	{
		if (this.browserInst != null)
		{
			this.browserInst.gameObject.SetActive(true);
		}
	}

	private void OnDisable()
	{
		if (this.browserInst != null)
		{
			this.browserInst.gameObject.SetActive(false);
		}
	}

	public void SetActive(bool active)
	{
		base.gameObject.SetActive(active);
	}

	public string CONF_URL = "";

	public int CONF_RESO_X = 1280;

	public int CONF_RESO_Y = 720;

	public int CONF_MARGIN_LEFT;

	public int CONF_MARGIN_TOP;

	public int CONF_MARGIN_RIGHT;

	public int CONF_MARGIN_BOTTOM;

	private bool bDependCanvasScaler;

	private Browser browserInst;

	public Text DEBUG_TEXT;

	private int DEBUG_RESO_X;

	private int DEBUG_RESO_Y;

	private bool isSetupCb;

	private string requestOpenExternalBrowserUrl = "";

	private WebViewPC.CallbackHolder<PreLoadCallback> preLoadCBs = new WebViewPC.CallbackHolder<PreLoadCallback>();

	private WebViewPC.CallbackHolder<PostLoadCallback> postLoadCBs = new WebViewPC.CallbackHolder<PostLoadCallback>();

	private class CallbackHolder<T>
	{
		public int Entry(T func)
		{
			Dictionary<int, T> dictionary = this.funcs;
			int num = this.sequencer + 1;
			this.sequencer = num;
			dictionary.Add(num, func);
			return this.sequencer;
		}

		public void Remove(int hndl)
		{
			this.funcs.Remove(hndl);
		}

		public Dictionary<int, T> funcs = new Dictionary<int, T>();

		private int sequencer;
	}
}
