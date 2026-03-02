using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SegaUnityCEFBrowser
{
	// Token: 0x02000209 RID: 521
	public static class BrowserNative
	{
		// Token: 0x060021E9 RID: 8681
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_init();

		// Token: 0x060021EA RID: 8682
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_term();

		// Token: 0x060021EB RID: 8683
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_isReady(ref int isReady);

		// Token: 0x060021EC RID: 8684
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_cookie_set([MarshalAs(UnmanagedType.LPWStr)] string url, [MarshalAs(UnmanagedType.LPWStr)] string name, [MarshalAs(UnmanagedType.LPWStr)] string value, [MarshalAs(UnmanagedType.LPWStr)] string domain, [MarshalAs(UnmanagedType.LPWStr)] string path);

		// Token: 0x060021ED RID: 8685
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_cookie_delete([MarshalAs(UnmanagedType.LPWStr)] string url, [MarshalAs(UnmanagedType.LPWStr)] string name);

		// Token: 0x060021EE RID: 8686
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_cookie_delete_all();

		// Token: 0x060021EF RID: 8687
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_cookie_get([MarshalAs(UnmanagedType.LPWStr)] string url, [MarshalAs(UnmanagedType.LPWStr)] string key, [MarshalAs(UnmanagedType.LPWStr)] [Out] StringBuilder valueBuffer, int bufferSizeCch);

		// Token: 0x060021F0 RID: 8688
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_create_browser(ref int id, int width, int height, int isTransparent, uint bgcolor, int enableJavaScript);

		// Token: 0x060021F1 RID: 8689
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_close_browser(int id);

		// Token: 0x060021F2 RID: 8690
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_nav_can(int id, int direction, ref int result);

		// Token: 0x060021F3 RID: 8691
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_nav_go(int id, int direction);

		// Token: 0x060021F4 RID: 8692
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_mouse_move(int id, int x, int y);

		// Token: 0x060021F5 RID: 8693
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_mouse_click(int id, int x, int y, int mouseButton, int isButtonUp);

		// Token: 0x060021F6 RID: 8694
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_mouse_wheel(int id, int x, int y, int deltaX, int deltaY);

		// Token: 0x060021F7 RID: 8695
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_loadURL(int id, [MarshalAs(UnmanagedType.LPWStr)] string url);

		// Token: 0x060021F8 RID: 8696
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_reload(int id);

		// Token: 0x060021F9 RID: 8697
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_resize(int id, int width, int height);

		// Token: 0x060021FA RID: 8698
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_keyevent(int id, uint keycode, int isKeyUp);

		// Token: 0x060021FB RID: 8699
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_getImage(int id, IntPtr image);

		// Token: 0x060021FC RID: 8700
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_evaluateJS(int id, [MarshalAs(UnmanagedType.LPWStr)] string jsCode, [MarshalAs(UnmanagedType.LPWStr)] string funcId);

		// Token: 0x060021FD RID: 8701
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_devtools(int id, int show);

		// Token: 0x060021FE RID: 8702
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_req_header_set(int id, [MarshalAs(UnmanagedType.LPWStr)] string name, [MarshalAs(UnmanagedType.LPWStr)] string value);

		// Token: 0x060021FF RID: 8703
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_req_header_del(int id, [MarshalAs(UnmanagedType.LPWStr)] string name, [MarshalAs(UnmanagedType.LPWStr)] string value);

		// Token: 0x06002200 RID: 8704
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_url_get(int id, ref int result, [MarshalAs(UnmanagedType.LPStr)] ref string url);

		// Token: 0x06002201 RID: 8705
		[DllImport("Kernel32.dll", EntryPoint = "RtlZeroMemory")]
		public static extern void ZeroMemory(IntPtr dest, ulong size);

		// Token: 0x06002202 RID: 8706
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_pump();

		// Token: 0x06002203 RID: 8707
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_setOnContextInitialized(BrowserNative.OnContextInitializedDelegate callback);

		// Token: 0x06002204 RID: 8708
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_setOnCrashed(BrowserNative.OnCrashedDelegate callback);

		// Token: 0x06002205 RID: 8709
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_setOnBeforeClose(int id, BrowserNative.OnBeforeCloseDelegate callback);

		// Token: 0x06002206 RID: 8710
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_setOnLoadingStateChange(int id, BrowserNative.OnLoadingStateChangeDelegate callback);

		// Token: 0x06002207 RID: 8711
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_setOnLoadStart(int id, BrowserNative.OnLoadStartDelegate callback);

		// Token: 0x06002208 RID: 8712
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_setOnLoadEnd(int id, BrowserNative.OnLoadEndDelegate callback);

		// Token: 0x06002209 RID: 8713
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_setOnLoadError(int id, BrowserNative.OnLoadErrorDelegate callback);

		// Token: 0x0600220A RID: 8714
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_setOnBeforeBrowse(int id, BrowserNative.OnBeforeBrowseDelegate callback);

		// Token: 0x0600220B RID: 8715
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_setOnCertificateError(int id, BrowserNative.OnCertificateErrorDelegate callback);

		// Token: 0x0600220C RID: 8716
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_setOnRenderProcessTerminated(int id, BrowserNative.OnRenderProcessTerminatedDelegate callback);

		// Token: 0x0600220D RID: 8717
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_setOnJavaScriptEvaluated(int id, BrowserNative.OnJavaScriptEvaluatedDelegate callback);

		// Token: 0x0600220E RID: 8718
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_check(ref uint flag, ref int size);

		// Token: 0x0600220F RID: 8719
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_readOnContextInitialized();

		// Token: 0x06002210 RID: 8720
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_writeOnContextInitialized(int result);

		// Token: 0x06002211 RID: 8721
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_readOnCrashed();

		// Token: 0x06002212 RID: 8722
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_writeOnCrashed(int result);

		// Token: 0x06002213 RID: 8723
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_readOnAfterCreated(ref int id);

		// Token: 0x06002214 RID: 8724
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_writeOnAfterCreated(int result);

		// Token: 0x06002215 RID: 8725
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_readOnBeforeClose(ref int id);

		// Token: 0x06002216 RID: 8726
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_writeOnBeforeClose(int result);

		// Token: 0x06002217 RID: 8727
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_readOnLoadingStateChange(ref int id, ref int isLoading);

		// Token: 0x06002218 RID: 8728
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_writeOnLoadingStateChange(int result);

		// Token: 0x06002219 RID: 8729
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_readOnLoadStart(ref int id, ref int transitionType);

		// Token: 0x0600221A RID: 8730
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_writeOnLoadStart(int result);

		// Token: 0x0600221B RID: 8731
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_readOnLoadEnd(ref int id, IntPtr url, ref int httpStatusCode, int size);

		// Token: 0x0600221C RID: 8732
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_writeOnLoadEnd(int result);

		// Token: 0x0600221D RID: 8733
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_readOnLoadError(ref int id, ref int errorCode);

		// Token: 0x0600221E RID: 8734
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_writeOnLoadError(int result);

		// Token: 0x0600221F RID: 8735
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_readOnBeforeBrowse(ref int id, IntPtr url, int size);

		// Token: 0x06002220 RID: 8736
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_writeOnBeforeBrowse(int result);

		// Token: 0x06002221 RID: 8737
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_readOnCertificateError(ref int id, ref int errorCode, ref uint certStatus);

		// Token: 0x06002222 RID: 8738
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_writeOnCertificateError(int result);

		// Token: 0x06002223 RID: 8739
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_readOnRenderProcessTerminated(ref int id, ref int status);

		// Token: 0x06002224 RID: 8740
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_writeOnRenderProcessTerminated(int result);

		// Token: 0x06002225 RID: 8741
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_readOnJavaScriptEvaluated(ref int id, ref int funcResult, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder evalResultBuf, int capacityCch, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder funcIdentifier, int size);

		// Token: 0x06002226 RID: 8742
		[DllImport("unity_cef_bridge", CallingConvention = CallingConvention.Cdecl)]
		public static extern int ucef_callback_writeOnJavaScriptEvaluated(int result);

		// Token: 0x0200104E RID: 4174
		// (Invoke) Token: 0x0600529C RID: 21148
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int OnBeforeBrowseDelegate(int browserId, [MarshalAs(UnmanagedType.LPWStr)] string url);

		// Token: 0x0200104F RID: 4175
		// (Invoke) Token: 0x060052A0 RID: 21152
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int OnBeforeCloseDelegate(int id);

		// Token: 0x02001050 RID: 4176
		// (Invoke) Token: 0x060052A4 RID: 21156
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int OnCertificateErrorDelegate(int id, int errorCode, uint certStatus);

		// Token: 0x02001051 RID: 4177
		// (Invoke) Token: 0x060052A8 RID: 21160
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int OnContextInitializedDelegate();

		// Token: 0x02001052 RID: 4178
		// (Invoke) Token: 0x060052AC RID: 21164
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int OnCrashedDelegate();

		// Token: 0x02001053 RID: 4179
		// (Invoke) Token: 0x060052B0 RID: 21168
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int OnLoadEndDelegate(int id, [MarshalAs(UnmanagedType.LPWStr)] string url, int httpStatusCode);

		// Token: 0x02001054 RID: 4180
		// (Invoke) Token: 0x060052B4 RID: 21172
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int OnLoadErrorDelegate(int id, int errorCode);

		// Token: 0x02001055 RID: 4181
		// (Invoke) Token: 0x060052B8 RID: 21176
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int OnLoadingStateChangeDelegate(int id, int isLoading);

		// Token: 0x02001056 RID: 4182
		// (Invoke) Token: 0x060052BC RID: 21180
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int OnLoadStartDelegate(int id, int transitionType);

		// Token: 0x02001057 RID: 4183
		// (Invoke) Token: 0x060052C0 RID: 21184
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int OnRenderProcessTerminatedDelegate(int id, int status);

		// Token: 0x02001058 RID: 4184
		// (Invoke) Token: 0x060052C4 RID: 21188
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate int OnJavaScriptEvaluatedDelegate(int id, int funcResult, [MarshalAs(UnmanagedType.LPWStr)] string evalResult, [MarshalAs(UnmanagedType.LPWStr)] string funcIdentifier);

		// Token: 0x02001059 RID: 4185
		public enum CallbackFlag : uint
		{
			// Token: 0x04005B71 RID: 23409
			UCEF_CALLBACK_FLAG_OnContextInitialized = 1U,
			// Token: 0x04005B72 RID: 23410
			UCEF_CALLBACK_FLAG_OnCrashed = 8U,
			// Token: 0x04005B73 RID: 23411
			UCEF_CALLBACK_FLAG_OnBeforePopup = 16U,
			// Token: 0x04005B74 RID: 23412
			UCEF_CALLBACK_FLAG_OnAfterCreated = 32U,
			// Token: 0x04005B75 RID: 23413
			UCEF_CALLBACK_FLAG_OnBeforeClose = 64U,
			// Token: 0x04005B76 RID: 23414
			UCEF_CALLBACK_FLAG_OnLoadingStateChange = 256U,
			// Token: 0x04005B77 RID: 23415
			UCEF_CALLBACK_FLAG_OnLoadStart = 512U,
			// Token: 0x04005B78 RID: 23416
			UCEF_CALLBACK_FLAG_OnLoadEnd = 1024U,
			// Token: 0x04005B79 RID: 23417
			UCEF_CALLBACK_FLAG_OnLoadError = 2048U,
			// Token: 0x04005B7A RID: 23418
			UCEF_CALLBACK_FLAG_OnBeforeBrowse = 4096U,
			// Token: 0x04005B7B RID: 23419
			UCEF_CALLBACK_FLAG_OnCertificateError = 8192U,
			// Token: 0x04005B7C RID: 23420
			UCEF_CALLBACK_FLAG_OnRenderProcessTerminated = 16384U,
			// Token: 0x04005B7D RID: 23421
			UCEF_CALLBACK_FLAG_OnJavaScriptEvaluated = 65536U
		}

		// Token: 0x0200105A RID: 4186
		public enum MouseButton
		{
			// Token: 0x04005B7F RID: 23423
			MBT_LEFT = 1,
			// Token: 0x04005B80 RID: 23424
			MBT_RIGHT,
			// Token: 0x04005B81 RID: 23425
			MBT_MIDDLE
		}
	}
}
