using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SegaUnityCEFBrowser
{
	public static class BrowserNative
	{
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_init();

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_term();

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_isReady(ref int isReady);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_cookie_set([MarshalAs(UnmanagedType.LPWStr)] string url, [MarshalAs(UnmanagedType.LPWStr)] string name, [MarshalAs(UnmanagedType.LPWStr)] string value, [MarshalAs(UnmanagedType.LPWStr)] string domain, [MarshalAs(UnmanagedType.LPWStr)] string path);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_cookie_delete([MarshalAs(UnmanagedType.LPWStr)] string url, [MarshalAs(UnmanagedType.LPWStr)] string name);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_cookie_delete_all();

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_cookie_get([MarshalAs(UnmanagedType.LPWStr)] string url, [MarshalAs(UnmanagedType.LPWStr)] string key, [MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder valueBuffer, int bufferSizeCch);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_create_browser(ref int id, int width, int height, int isTransparent, uint bgcolor, int enableJavaScript);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_close_browser(int id);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_nav_can(int id, int direction, ref int result);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_nav_go(int id, int direction);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_mouse_move(int id, int x, int y);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_mouse_click(int id, int x, int y, int mouseButton, int isButtonUp);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_mouse_wheel(int id, int x, int y, int deltaX, int deltaY);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_loadURL(int id, [MarshalAs(UnmanagedType.LPWStr)] string url);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_reload(int id);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_resize(int id, int width, int height);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_keyevent(int id, uint keycode, int isKeyUp);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_getImage(int id, IntPtr image);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_evaluateJS(int id, [MarshalAs(UnmanagedType.LPWStr)] string jsCode, [MarshalAs(UnmanagedType.LPWStr)] string funcId);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_devtools(int id, int show);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_req_header_set(int id, [MarshalAs(UnmanagedType.LPWStr)] string name, [MarshalAs(UnmanagedType.LPWStr)] string value);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_req_header_del(int id, [MarshalAs(UnmanagedType.LPWStr)] string name, [MarshalAs(UnmanagedType.LPWStr)] string value);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_url_get(int id, ref int result, [MarshalAs(UnmanagedType.LPStr)] ref string url);

		[DllImport("Kernel32.dll", EntryPoint = "RtlZeroMemory")]
		public static extern void ZeroMemory(IntPtr dest, ulong size);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_pump();

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_setOnContextInitialized(BrowserNative.OnContextInitializedDelegate callback);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_setOnCrashed(BrowserNative.OnCrashedDelegate callback);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_setOnBeforeClose(int id, BrowserNative.OnBeforeCloseDelegate callback);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_setOnLoadingStateChange(int id, BrowserNative.OnLoadingStateChangeDelegate callback);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_setOnLoadStart(int id, BrowserNative.OnLoadStartDelegate callback);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_setOnLoadEnd(int id, BrowserNative.OnLoadEndDelegate callback);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_setOnLoadError(int id, BrowserNative.OnLoadErrorDelegate callback);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_setOnBeforeBrowse(int id, BrowserNative.OnBeforeBrowseDelegate callback);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_setOnCertificateError(int id, BrowserNative.OnCertificateErrorDelegate callback);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_setOnRenderProcessTerminated(int id, BrowserNative.OnRenderProcessTerminatedDelegate callback);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_setOnJavaScriptEvaluated(int id, BrowserNative.OnJavaScriptEvaluatedDelegate callback);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_check(ref uint flag, ref int size);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_readOnContextInitialized();

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_writeOnContextInitialized(int result);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_readOnCrashed();

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_writeOnCrashed(int result);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_readOnAfterCreated(ref int id);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_writeOnAfterCreated(int result);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_readOnBeforeClose(ref int id);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_writeOnBeforeClose(int result);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_readOnLoadingStateChange(ref int id, ref int isLoading);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_writeOnLoadingStateChange(int result);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_readOnLoadStart(ref int id, ref int transitionType);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_writeOnLoadStart(int result);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_readOnLoadEnd(ref int id, IntPtr url, ref int httpStatusCode, int size);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_writeOnLoadEnd(int result);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_readOnLoadError(ref int id, ref int errorCode);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_writeOnLoadError(int result);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_readOnBeforeBrowse(ref int id, IntPtr url, int size);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_writeOnBeforeBrowse(int result);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_readOnCertificateError(ref int id, ref int errorCode, ref uint certStatus);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_writeOnCertificateError(int result);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_readOnRenderProcessTerminated(ref int id, ref int status);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_writeOnRenderProcessTerminated(int result);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_readOnJavaScriptEvaluated(ref int id, ref int funcResult, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder evalResultBuf, int capacityCch, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder funcIdentifier, int size);

		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_writeOnJavaScriptEvaluated(int result);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int OnBeforeBrowseDelegate(int browserId, [MarshalAs(UnmanagedType.LPWStr)] string url);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int OnBeforeCloseDelegate(int id);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int OnCertificateErrorDelegate(int id, int errorCode, uint certStatus);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int OnContextInitializedDelegate();

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int OnCrashedDelegate();

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int OnLoadEndDelegate(int id, [MarshalAs(UnmanagedType.LPWStr)] string url, int httpStatusCode);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int OnLoadErrorDelegate(int id, int errorCode);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int OnLoadingStateChangeDelegate(int id, int isLoading);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int OnLoadStartDelegate(int id, int transitionType);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int OnRenderProcessTerminatedDelegate(int id, int status);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int OnJavaScriptEvaluatedDelegate(int id, int funcResult, [MarshalAs(UnmanagedType.LPWStr)] string evalResult, [MarshalAs(UnmanagedType.LPWStr)] string funcIdentifier);

		public enum CallbackFlag : uint
		{
			UCEF_CALLBACK_FLAG_OnContextInitialized = 1U,
			UCEF_CALLBACK_FLAG_OnCrashed = 8U,
			UCEF_CALLBACK_FLAG_OnBeforePopup = 16U,
			UCEF_CALLBACK_FLAG_OnAfterCreated = 32U,
			UCEF_CALLBACK_FLAG_OnBeforeClose = 64U,
			UCEF_CALLBACK_FLAG_OnLoadingStateChange = 256U,
			UCEF_CALLBACK_FLAG_OnLoadStart = 512U,
			UCEF_CALLBACK_FLAG_OnLoadEnd = 1024U,
			UCEF_CALLBACK_FLAG_OnLoadError = 2048U,
			UCEF_CALLBACK_FLAG_OnBeforeBrowse = 4096U,
			UCEF_CALLBACK_FLAG_OnCertificateError = 8192U,
			UCEF_CALLBACK_FLAG_OnRenderProcessTerminated = 16384U,
			UCEF_CALLBACK_FLAG_OnJavaScriptEvaluated = 65536U
		}

		public enum MouseButton
		{
			MBT_LEFT = 1,
			MBT_RIGHT,
			MBT_MIDDLE
		}
	}
}
