using System;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace SegaUnityCEFBrowser
{
	public class Browser : MonoBehaviour
	{
		public bool IsReady
		{
			get
			{
				return this.mBrowserId != 0;
			}
		}

		public int Width
		{
			get
			{
				return this.mWidth;
			}
		}

		public int Height
		{
			get
			{
				return this.mHeight;
			}
		}

		public Resolution RealResolution { get; set; }

		public void reuseBrowser(int id)
		{
			this.mBrowserId = id;
			this.needClose = false;
		}

		public void LoadURL(string url)
		{
			this.mUrl = url;
			if (!this.IsReady)
			{
				return;
			}
			BrowserNative.ucef_loadURL(this.mBrowserId, url);
		}

		public void Reload()
		{
			if (!this.IsReady)
			{
				return;
			}
			if (string.IsNullOrEmpty(this.mUrl))
			{
				return;
			}
			BrowserNative.ucef_loadURL(this.mBrowserId, this.mUrl);
		}

		public bool CanGoBack()
		{
			if (!this.IsReady)
			{
				return false;
			}
			int num = 0;
			BrowserNative.ucef_nav_can(this.mBrowserId, -1, ref num);
			return num != 0;
		}

		public void GoBack()
		{
			if (!this.IsReady)
			{
				return;
			}
			BrowserNative.ucef_nav_go(this.mBrowserId, -1);
		}

		public bool CanGoForward()
		{
			if (!this.IsReady)
			{
				return false;
			}
			int num = 0;
			BrowserNative.ucef_nav_can(this.mBrowserId, 1, ref num);
			return num != 0;
		}

		public void GoForward()
		{
			if (!this.IsReady)
			{
				return;
			}
			BrowserNative.ucef_nav_go(this.mBrowserId, 1);
		}

		public void Resize(int width, int height)
		{
			if (!this.IsReady)
			{
				if (BrowserPump.Instance.IsCrashed)
				{
					return;
				}
				if (this.needClose)
				{
					this.mBrowserId = BrowserManager.Instance.Pop();
				}
				else
				{
					this.mBrowserId = BrowserManager.Instance.Front();
				}
				int num = this.mBrowserId;
			}
			if (null != this.mTexture && this.mTexture.width != width && this.mTexture.height == height)
			{
				return;
			}
			BrowserNative.ucef_resize(this.mBrowserId, width, height);
			Texture2D texture2D = new Texture2D(width, height, TextureFormat.BGRA32, false);
			texture2D.wrapMode = TextureWrapMode.Clamp;
			Object.Destroy(this.mTexture);
			this.mTexture = texture2D;
			this.mImage = new byte[width * height * 4];
			if (IntPtr.Zero != this.mImageUnmanage)
			{
				Marshal.FreeHGlobal(this.mImageUnmanage);
				this.mImageUnmanage = IntPtr.Zero;
			}
			this.mImageUnmanage = Marshal.AllocHGlobal(this.mImage.Length);
			ulong num2 = (ulong)((long)(width * height * 4));
			BrowserNative.ZeroMemory(this.mImageUnmanage, num2);
			this.mTexture.LoadRawTextureData(this.mImage);
			this.mTexture.Apply();
			this._MeshCollider.GetComponent<Renderer>().material.mainTexture = this.mTexture;
			Mesh mesh = base.GetComponentInChildren<MeshFilter>().mesh;
			Vector3[] vertices = mesh.vertices;
			vertices[0] = new Vector3((float)(-(float)width), (float)(-(float)height), 0f);
			vertices[1] = new Vector3((float)width, (float)height, 0f);
			vertices[2] = new Vector3((float)width, (float)(-(float)height), 0f);
			vertices[3] = new Vector3((float)(-(float)width), (float)height, 0f);
			mesh.vertices = vertices;
			int[] array = new int[] { 0, 1, 2, 1, 0, 3 };
			mesh.triangles = array;
			this._WWWCamera.orthographicSize = (float)Screen.height;
			this._WWWCamera.aspect = (float)Screen.width / (float)Screen.height;
			this.mWidth = width;
			this.mHeight = height;
		}

		public void EvaluateJS(string jsString, string funcIdentifier)
		{
			if (jsString == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(funcIdentifier))
			{
				return;
			}
			BrowserNative.ucef_evaluateJS(this.mBrowserId, jsString, funcIdentifier);
		}

		public static void ClearCookies()
		{
			BrowserNative.ucef_cookie_delete_all();
		}

		public static void SetCookie(string url, string name, string value, string domain, string path)
		{
			if (url == null)
			{
				return;
			}
			if (url.Length == 0)
			{
				return;
			}
			BrowserNative.ucef_cookie_set(url, name, value, domain, path);
		}

		public string GetCookie(string url, string name)
		{
			if (url == null)
			{
				return string.Empty;
			}
			if (url.Length == 0)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder(8192);
			BrowserNative.ucef_cookie_get(url, name, stringBuilder, stringBuilder.Capacity);
			return stringBuilder.ToString();
		}

		public void SetRequestHeader(string name, string value)
		{
			if (!this.IsReady)
			{
				return;
			}
			BrowserNative.ucef_req_header_set(this.mBrowserId, name, value);
		}

		public void InputMouseClick(int x, int y, bool isButtonUp)
		{
			if (!this.IsReady)
			{
				return;
			}
			BrowserNative.ucef_mouse_click(this.mBrowserId, x, y, 1, isButtonUp ? 1 : 0);
		}

		public void InputMouseMove(int x, int y)
		{
			if (!this.IsReady)
			{
				return;
			}
			BrowserNative.ucef_mouse_move(this.mBrowserId, x, y);
		}

		public void InputMouseWheel(int x, int y, int deltaX, int deltaY)
		{
			if (!this.IsReady)
			{
				return;
			}
			BrowserNative.ucef_mouse_wheel(this.mBrowserId, x, y, deltaX, deltaY);
		}

		public void ShowDevTools()
		{
			if (!this.IsReady)
			{
				return;
			}
			BrowserNative.ucef_devtools(this.mBrowserId, 1);
		}

		public void InputKeyEvent(uint keyCode, bool isKeyUp)
		{
			if (!this.IsReady)
			{
				return;
			}
			BrowserNative.ucef_keyevent(this.mBrowserId, keyCode, isKeyUp ? 1 : 0);
		}

		public string GetUrl()
		{
			if (!this.IsReady)
			{
				return string.Empty;
			}
			int num = 0;
			string text = null;
			BrowserNative.ucef_url_get(this.mBrowserId, ref num, ref text);
			return text ?? string.Empty;
		}

		private void Start()
		{
			Mesh mesh = base.GetComponentInChildren<MeshFilter>().mesh;
			Vector3[] vertices = mesh.vertices;
			int[] triangles = mesh.triangles;
			Vector3[] normals = mesh.normals;
			Vector2[] uv = mesh.uv;
			uv[0] = new Vector2(0f, 1f);
			uv[1] = new Vector2(1f, 0f);
			uv[2] = new Vector2(1f, 1f);
			uv[3] = new Vector2(0f, 0f);
			mesh.uv = uv;
		}

		private void Update()
		{
			if (!this.IsReady)
			{
				return;
			}
			BrowserNative.ucef_getImage(this.mBrowserId, this.mImageUnmanage);
			Marshal.Copy(this.mImageUnmanage, this.mImage, 0, this.mImage.Length);
			this.mTexture.LoadRawTextureData(this.mImage);
			this.mTexture.Apply();
		}

		private void OnDestroy()
		{
			if (this.IsReady && BrowserPump.Instance.IsInitialized)
			{
				BrowserManager.Instance.IsAlive(this.mBrowserId);
				if (this.needClose)
				{
					BrowserNative.ucef_close_browser(this.mBrowserId);
					BrowserManager.Instance.RequestCreateBrowser();
				}
			}
			if (IntPtr.Zero != this.mImageUnmanage)
			{
				Marshal.FreeHGlobal(this.mImageUnmanage);
				this.mImageUnmanage = IntPtr.Zero;
			}
		}

		private int onLoadingStateChange(int id, int isLoading)
		{
			return 0;
		}

		private int onLoadStart(int id, int transitionType)
		{
			return 0;
		}

		private int onLoadEnd(int id, int httpStatusCode)
		{
			return 0;
		}

		private int onLoadError(int id, int errorCode)
		{
			return 0;
		}

		public void setOnCrashed(BrowserNative.OnCrashedDelegate callback)
		{
			BrowserEvent.Instance.setOnCrashedCallback(callback);
		}

		public void setOnLoadingStateChange(BrowserNative.OnLoadingStateChangeDelegate callback)
		{
			if (!this.IsReady)
			{
				return;
			}
			BrowserEvent.Instance.setOnLoadingStateChangeCallback(this.mBrowserId, callback);
		}

		public void setOnLoadStart(BrowserNative.OnLoadStartDelegate callback)
		{
			if (!this.IsReady)
			{
				return;
			}
			BrowserEvent.Instance.setOnLoadStartCallback(this.mBrowserId, callback);
		}

		public void setOnLoadEnd(BrowserNative.OnLoadEndDelegate callback)
		{
			if (!this.IsReady)
			{
				return;
			}
			BrowserEvent.Instance.setOnLoadEndCallback(this.mBrowserId, callback);
		}

		public void setOnLoadError(BrowserNative.OnLoadErrorDelegate callback)
		{
			if (!this.IsReady)
			{
				return;
			}
			BrowserEvent.Instance.setOnLoadErrorCallback(this.mBrowserId, callback);
		}

		private int onBeforeBrowse(int id, string url)
		{
			if (!false)
			{
				return 0;
			}
			return 1;
		}

		public void setOnBeforeBrowse(BrowserNative.OnBeforeBrowseDelegate callback)
		{
			if (!this.IsReady)
			{
				return;
			}
			BrowserEvent.Instance.setOnBeforeBrowseCallback(this.mBrowserId, callback);
		}

		private int onCertificateError(int id, int errorCode, uint certStatus)
		{
			return 0;
		}

		public void setOnCertificateError(BrowserNative.OnCertificateErrorDelegate callback)
		{
			if (!this.IsReady)
			{
				return;
			}
			BrowserEvent.Instance.setOnCertificateErrorCallback(this.mBrowserId, callback);
		}

		private int onRenderProcessTerminated(int id, int status)
		{
			return 0;
		}

		public void setOnRenderProcessTerminated(BrowserNative.OnRenderProcessTerminatedDelegate callback)
		{
			if (!this.IsReady)
			{
				return;
			}
			BrowserEvent.Instance.setOnRenderProcessTerminatedCallback(this.mBrowserId, callback);
		}

		public void setOnJavaScriptEvaluated(BrowserNative.OnJavaScriptEvaluatedDelegate callback)
		{
			if (!this.IsReady)
			{
				return;
			}
			BrowserEvent.Instance.setOnJavaScriptEvaluatedCallback(this.mBrowserId, callback);
		}

		public Camera _WWWCamera;

		public MeshCollider _MeshCollider;

		protected internal int mBrowserId;

		protected Texture2D mTexture;

		internal byte[] mImage;

		internal IntPtr mImageUnmanage = IntPtr.Zero;

		internal int mWidth = -1;

		internal int mHeight = -1;

		internal bool needClose = true;

		private string mUrl = string.Empty;
	}
}
