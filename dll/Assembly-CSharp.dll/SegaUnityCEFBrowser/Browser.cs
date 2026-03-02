using System;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace SegaUnityCEFBrowser
{
	// Token: 0x02000204 RID: 516
	public class Browser : MonoBehaviour
	{
		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x0600218A RID: 8586 RVA: 0x0019073E File Offset: 0x0018E93E
		public bool IsReady
		{
			get
			{
				return this.mBrowserId != 0;
			}
		}

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x0600218B RID: 8587 RVA: 0x00190749 File Offset: 0x0018E949
		public int Width
		{
			get
			{
				return this.mWidth;
			}
		}

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x0600218C RID: 8588 RVA: 0x00190751 File Offset: 0x0018E951
		public int Height
		{
			get
			{
				return this.mHeight;
			}
		}

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x0600218D RID: 8589 RVA: 0x00190759 File Offset: 0x0018E959
		// (set) Token: 0x0600218E RID: 8590 RVA: 0x00190761 File Offset: 0x0018E961
		public Resolution RealResolution { get; set; }

		// Token: 0x0600218F RID: 8591 RVA: 0x0019076A File Offset: 0x0018E96A
		public void reuseBrowser(int id)
		{
			this.mBrowserId = id;
			this.needClose = false;
		}

		// Token: 0x06002190 RID: 8592 RVA: 0x0019077A File Offset: 0x0018E97A
		public void LoadURL(string url)
		{
			this.mUrl = url;
			if (!this.IsReady)
			{
				return;
			}
			BrowserNative.ucef_loadURL(this.mBrowserId, url);
		}

		// Token: 0x06002191 RID: 8593 RVA: 0x00190799 File Offset: 0x0018E999
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

		// Token: 0x06002192 RID: 8594 RVA: 0x001907C4 File Offset: 0x0018E9C4
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

		// Token: 0x06002193 RID: 8595 RVA: 0x001907F2 File Offset: 0x0018E9F2
		public void GoBack()
		{
			if (!this.IsReady)
			{
				return;
			}
			BrowserNative.ucef_nav_go(this.mBrowserId, -1);
		}

		// Token: 0x06002194 RID: 8596 RVA: 0x0019080C File Offset: 0x0018EA0C
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

		// Token: 0x06002195 RID: 8597 RVA: 0x0019083A File Offset: 0x0018EA3A
		public void GoForward()
		{
			if (!this.IsReady)
			{
				return;
			}
			BrowserNative.ucef_nav_go(this.mBrowserId, 1);
		}

		// Token: 0x06002196 RID: 8598 RVA: 0x00190854 File Offset: 0x0018EA54
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

		// Token: 0x06002197 RID: 8599 RVA: 0x00190A5F File Offset: 0x0018EC5F
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

		// Token: 0x06002198 RID: 8600 RVA: 0x00190A7C File Offset: 0x0018EC7C
		public static void ClearCookies()
		{
			BrowserNative.ucef_cookie_delete_all();
		}

		// Token: 0x06002199 RID: 8601 RVA: 0x00190A84 File Offset: 0x0018EC84
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

		// Token: 0x0600219A RID: 8602 RVA: 0x00190AA0 File Offset: 0x0018ECA0
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

		// Token: 0x0600219B RID: 8603 RVA: 0x00190AE4 File Offset: 0x0018ECE4
		public void SetRequestHeader(string name, string value)
		{
			if (!this.IsReady)
			{
				return;
			}
			BrowserNative.ucef_req_header_set(this.mBrowserId, name, value);
		}

		// Token: 0x0600219C RID: 8604 RVA: 0x00190AFD File Offset: 0x0018ECFD
		public void InputMouseClick(int x, int y, bool isButtonUp)
		{
			if (!this.IsReady)
			{
				return;
			}
			BrowserNative.ucef_mouse_click(this.mBrowserId, x, y, 1, isButtonUp ? 1 : 0);
		}

		// Token: 0x0600219D RID: 8605 RVA: 0x00190B1E File Offset: 0x0018ED1E
		public void InputMouseMove(int x, int y)
		{
			if (!this.IsReady)
			{
				return;
			}
			BrowserNative.ucef_mouse_move(this.mBrowserId, x, y);
		}

		// Token: 0x0600219E RID: 8606 RVA: 0x00190B37 File Offset: 0x0018ED37
		public void InputMouseWheel(int x, int y, int deltaX, int deltaY)
		{
			if (!this.IsReady)
			{
				return;
			}
			BrowserNative.ucef_mouse_wheel(this.mBrowserId, x, y, deltaX, deltaY);
		}

		// Token: 0x0600219F RID: 8607 RVA: 0x00190B53 File Offset: 0x0018ED53
		public void ShowDevTools()
		{
			if (!this.IsReady)
			{
				return;
			}
			BrowserNative.ucef_devtools(this.mBrowserId, 1);
		}

		// Token: 0x060021A0 RID: 8608 RVA: 0x00190B6B File Offset: 0x0018ED6B
		public void InputKeyEvent(uint keyCode, bool isKeyUp)
		{
			if (!this.IsReady)
			{
				return;
			}
			BrowserNative.ucef_keyevent(this.mBrowserId, keyCode, isKeyUp ? 1 : 0);
		}

		// Token: 0x060021A1 RID: 8609 RVA: 0x00190B8C File Offset: 0x0018ED8C
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

		// Token: 0x060021A2 RID: 8610 RVA: 0x00190BC8 File Offset: 0x0018EDC8
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

		// Token: 0x060021A3 RID: 8611 RVA: 0x00190C5C File Offset: 0x0018EE5C
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

		// Token: 0x060021A4 RID: 8612 RVA: 0x00190CBC File Offset: 0x0018EEBC
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

		// Token: 0x060021A5 RID: 8613 RVA: 0x00190D34 File Offset: 0x0018EF34
		private int onLoadingStateChange(int id, int isLoading)
		{
			return 0;
		}

		// Token: 0x060021A6 RID: 8614 RVA: 0x00190D37 File Offset: 0x0018EF37
		private int onLoadStart(int id, int transitionType)
		{
			return 0;
		}

		// Token: 0x060021A7 RID: 8615 RVA: 0x00190D3A File Offset: 0x0018EF3A
		private int onLoadEnd(int id, int httpStatusCode)
		{
			return 0;
		}

		// Token: 0x060021A8 RID: 8616 RVA: 0x00190D3D File Offset: 0x0018EF3D
		private int onLoadError(int id, int errorCode)
		{
			return 0;
		}

		// Token: 0x060021A9 RID: 8617 RVA: 0x00190D40 File Offset: 0x0018EF40
		public void setOnCrashed(BrowserNative.OnCrashedDelegate callback)
		{
			BrowserEvent.Instance.setOnCrashedCallback(callback);
		}

		// Token: 0x060021AA RID: 8618 RVA: 0x00190D4D File Offset: 0x0018EF4D
		public void setOnLoadingStateChange(BrowserNative.OnLoadingStateChangeDelegate callback)
		{
			if (!this.IsReady)
			{
				return;
			}
			BrowserEvent.Instance.setOnLoadingStateChangeCallback(this.mBrowserId, callback);
		}

		// Token: 0x060021AB RID: 8619 RVA: 0x00190D69 File Offset: 0x0018EF69
		public void setOnLoadStart(BrowserNative.OnLoadStartDelegate callback)
		{
			if (!this.IsReady)
			{
				return;
			}
			BrowserEvent.Instance.setOnLoadStartCallback(this.mBrowserId, callback);
		}

		// Token: 0x060021AC RID: 8620 RVA: 0x00190D85 File Offset: 0x0018EF85
		public void setOnLoadEnd(BrowserNative.OnLoadEndDelegate callback)
		{
			if (!this.IsReady)
			{
				return;
			}
			BrowserEvent.Instance.setOnLoadEndCallback(this.mBrowserId, callback);
		}

		// Token: 0x060021AD RID: 8621 RVA: 0x00190DA1 File Offset: 0x0018EFA1
		public void setOnLoadError(BrowserNative.OnLoadErrorDelegate callback)
		{
			if (!this.IsReady)
			{
				return;
			}
			BrowserEvent.Instance.setOnLoadErrorCallback(this.mBrowserId, callback);
		}

		// Token: 0x060021AE RID: 8622 RVA: 0x00190DBD File Offset: 0x0018EFBD
		private int onBeforeBrowse(int id, string url)
		{
			if (!false)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x060021AF RID: 8623 RVA: 0x00190DC5 File Offset: 0x0018EFC5
		public void setOnBeforeBrowse(BrowserNative.OnBeforeBrowseDelegate callback)
		{
			if (!this.IsReady)
			{
				return;
			}
			BrowserEvent.Instance.setOnBeforeBrowseCallback(this.mBrowserId, callback);
		}

		// Token: 0x060021B0 RID: 8624 RVA: 0x00190DE1 File Offset: 0x0018EFE1
		private int onCertificateError(int id, int errorCode, uint certStatus)
		{
			return 0;
		}

		// Token: 0x060021B1 RID: 8625 RVA: 0x00190DE4 File Offset: 0x0018EFE4
		public void setOnCertificateError(BrowserNative.OnCertificateErrorDelegate callback)
		{
			if (!this.IsReady)
			{
				return;
			}
			BrowserEvent.Instance.setOnCertificateErrorCallback(this.mBrowserId, callback);
		}

		// Token: 0x060021B2 RID: 8626 RVA: 0x00190E00 File Offset: 0x0018F000
		private int onRenderProcessTerminated(int id, int status)
		{
			return 0;
		}

		// Token: 0x060021B3 RID: 8627 RVA: 0x00190E03 File Offset: 0x0018F003
		public void setOnRenderProcessTerminated(BrowserNative.OnRenderProcessTerminatedDelegate callback)
		{
			if (!this.IsReady)
			{
				return;
			}
			BrowserEvent.Instance.setOnRenderProcessTerminatedCallback(this.mBrowserId, callback);
		}

		// Token: 0x060021B4 RID: 8628 RVA: 0x00190E1F File Offset: 0x0018F01F
		public void setOnJavaScriptEvaluated(BrowserNative.OnJavaScriptEvaluatedDelegate callback)
		{
			if (!this.IsReady)
			{
				return;
			}
			BrowserEvent.Instance.setOnJavaScriptEvaluatedCallback(this.mBrowserId, callback);
		}

		// Token: 0x04001862 RID: 6242
		public Camera _WWWCamera;

		// Token: 0x04001863 RID: 6243
		public MeshCollider _MeshCollider;

		// Token: 0x04001864 RID: 6244
		protected internal int mBrowserId;

		// Token: 0x04001865 RID: 6245
		protected Texture2D mTexture;

		// Token: 0x04001866 RID: 6246
		internal byte[] mImage;

		// Token: 0x04001867 RID: 6247
		internal IntPtr mImageUnmanage = IntPtr.Zero;

		// Token: 0x04001868 RID: 6248
		internal int mWidth = -1;

		// Token: 0x04001869 RID: 6249
		internal int mHeight = -1;

		// Token: 0x0400186A RID: 6250
		internal bool needClose = true;

		// Token: 0x0400186C RID: 6252
		private string mUrl = string.Empty;
	}
}
