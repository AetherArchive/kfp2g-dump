using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace SegaUnityCEFBrowser
{
	public class BrowserEvent
	{
		public static BrowserEvent Instance { get; } = new BrowserEvent();

		private BrowserEvent()
		{
		}

		public void setOnCrashedCallback(BrowserNative.OnCrashedDelegate callback)
		{
			Dictionary<int, BrowserEvent.BrowserCallback> dictionary = this.mDictCallback;
			lock (dictionary)
			{
				this.mOnCrashedCallback = (BrowserNative.OnCrashedDelegate)Delegate.Combine(this.mOnCrashedCallback, callback);
				BrowserNative.ucef_callback_setOnCrashed(new BrowserNative.OnCrashedDelegate(BrowserEvent.onCrashed));
			}
		}

		public void delOnCrashedCallback(BrowserNative.OnCrashedDelegate callback)
		{
			Dictionary<int, BrowserEvent.BrowserCallback> dictionary = this.mDictCallback;
			lock (dictionary)
			{
				if (this.mOnCrashedCallback != null)
				{
					this.mOnCrashedCallback = (BrowserNative.OnCrashedDelegate)Delegate.Remove(this.mOnCrashedCallback, callback);
				}
			}
		}

		private static int onCrashed()
		{
			int num = 0;
			BrowserEvent instance = BrowserEvent.Instance;
			Dictionary<int, BrowserEvent.BrowserCallback> dictionary = instance.mDictCallback;
			lock (dictionary)
			{
				try
				{
					if (instance.mOnCrashedCallback != null)
					{
						Delegate[] invocationList = instance.mOnCrashedCallback.GetInvocationList();
						if (invocationList != null)
						{
							int num2 = invocationList.Length;
							for (int i = 0; i < num2; i++)
							{
								num += instance.mOnCrashedCallback();
							}
						}
					}
				}
				catch (Exception)
				{
				}
			}
			return num;
		}

		public void setOnBeforeCloseCallback(int id, BrowserNative.OnBeforeCloseDelegate callback)
		{
			Dictionary<int, BrowserEvent.BrowserCallback> dictionary = this.mDictCallback;
			lock (dictionary)
			{
				BrowserEvent.BrowserCallback browserCallback;
				if (!this.mDictCallback.TryGetValue(id, out browserCallback))
				{
					browserCallback = new BrowserEvent.BrowserCallback();
					this.mDictCallback.Add(id, browserCallback);
				}
				browserCallback.mOnBeforeCloseCallback = callback;
				BrowserNative.ucef_callback_setOnBeforeClose(id, new BrowserNative.OnBeforeCloseDelegate(BrowserEvent.onBeforeClose));
			}
		}

		private static int onBeforeClose(int id)
		{
			int num = -1;
			BrowserEvent instance = BrowserEvent.Instance;
			Dictionary<int, BrowserEvent.BrowserCallback> dictionary = instance.mDictCallback;
			lock (dictionary)
			{
				BrowserEvent.BrowserCallback browserCallback;
				if (instance.mDictCallback.TryGetValue(id, out browserCallback))
				{
					try
					{
						if (browserCallback.mOnBeforeCloseCallback != null)
						{
							num = browserCallback.mOnBeforeCloseCallback(id);
						}
					}
					catch (Exception)
					{
					}
				}
				instance.mDictCallback.Remove(id);
			}
			return num;
		}

		public void setOnLoadingStateChangeCallback(int id, BrowserNative.OnLoadingStateChangeDelegate callback)
		{
			Dictionary<int, BrowserEvent.BrowserCallback> dictionary = this.mDictCallback;
			lock (dictionary)
			{
				BrowserEvent.BrowserCallback browserCallback;
				if (!this.mDictCallback.TryGetValue(id, out browserCallback))
				{
					browserCallback = new BrowserEvent.BrowserCallback();
					this.mDictCallback.Add(id, browserCallback);
				}
				browserCallback.mOnLoadingStateChangeCallback = callback;
				BrowserNative.ucef_callback_setOnLoadingStateChange(id, new BrowserNative.OnLoadingStateChangeDelegate(BrowserEvent.onLoadingStateChange));
			}
		}

		public void setOnLoadStartCallback(int id, BrowserNative.OnLoadStartDelegate callback)
		{
			Dictionary<int, BrowserEvent.BrowserCallback> dictionary = this.mDictCallback;
			lock (dictionary)
			{
				BrowserEvent.BrowserCallback browserCallback;
				if (!this.mDictCallback.TryGetValue(id, out browserCallback))
				{
					browserCallback = new BrowserEvent.BrowserCallback();
					this.mDictCallback.Add(id, browserCallback);
				}
				browserCallback.mOnLoadStartCallback = callback;
				BrowserNative.ucef_callback_setOnLoadStart(id, new BrowserNative.OnLoadStartDelegate(BrowserEvent.onLoadStart));
			}
		}

		public void setOnLoadEndCallback(int id, BrowserNative.OnLoadEndDelegate callback)
		{
			Dictionary<int, BrowserEvent.BrowserCallback> dictionary = this.mDictCallback;
			lock (dictionary)
			{
				BrowserEvent.BrowserCallback browserCallback;
				if (!this.mDictCallback.TryGetValue(id, out browserCallback))
				{
					browserCallback = new BrowserEvent.BrowserCallback();
					this.mDictCallback.Add(id, browserCallback);
				}
				browserCallback.mOnLoadEndCallback = callback;
				BrowserNative.ucef_callback_setOnLoadEnd(id, new BrowserNative.OnLoadEndDelegate(BrowserEvent.onLoadEnd));
			}
		}

		public void setOnLoadErrorCallback(int id, BrowserNative.OnLoadErrorDelegate callback)
		{
			Dictionary<int, BrowserEvent.BrowserCallback> dictionary = this.mDictCallback;
			lock (dictionary)
			{
				BrowserEvent.BrowserCallback browserCallback;
				if (!this.mDictCallback.TryGetValue(id, out browserCallback))
				{
					browserCallback = new BrowserEvent.BrowserCallback();
					this.mDictCallback.Add(id, browserCallback);
				}
				browserCallback.mOnLoadErrorCallback = callback;
				BrowserNative.ucef_callback_setOnLoadError(id, new BrowserNative.OnLoadErrorDelegate(BrowserEvent.onLoadError));
			}
		}

		private static int onLoadingStateChange(int id, int isLoading)
		{
			int num = -1;
			BrowserEvent instance = BrowserEvent.Instance;
			Dictionary<int, BrowserEvent.BrowserCallback> dictionary = instance.mDictCallback;
			lock (dictionary)
			{
				BrowserEvent.BrowserCallback browserCallback;
				if (instance.mDictCallback.TryGetValue(id, out browserCallback))
				{
					try
					{
						if (browserCallback.mOnLoadingStateChangeCallback != null)
						{
							num = browserCallback.mOnLoadingStateChangeCallback(id, isLoading);
						}
					}
					catch (Exception)
					{
					}
				}
			}
			return num;
		}

		private static int onLoadStart(int id, int transitionType)
		{
			int num = -1;
			BrowserEvent instance = BrowserEvent.Instance;
			Dictionary<int, BrowserEvent.BrowserCallback> dictionary = instance.mDictCallback;
			lock (dictionary)
			{
				BrowserEvent.BrowserCallback browserCallback;
				if (instance.mDictCallback.TryGetValue(id, out browserCallback))
				{
					try
					{
						if (browserCallback.mOnLoadStartCallback != null)
						{
							num = browserCallback.mOnLoadStartCallback(id, transitionType);
						}
					}
					catch (Exception)
					{
					}
				}
			}
			return num;
		}

		private static int onLoadEnd(int id, string url, int httpStatusCode)
		{
			int num = -1;
			BrowserEvent instance = BrowserEvent.Instance;
			Dictionary<int, BrowserEvent.BrowserCallback> dictionary = instance.mDictCallback;
			lock (dictionary)
			{
				BrowserEvent.BrowserCallback browserCallback;
				if (instance.mDictCallback.TryGetValue(id, out browserCallback))
				{
					try
					{
						if (browserCallback.mOnLoadEndCallback != null)
						{
							num = browserCallback.mOnLoadEndCallback(id, url, httpStatusCode);
						}
					}
					catch (Exception)
					{
					}
				}
			}
			return num;
		}

		private static int onLoadError(int id, int errorCode)
		{
			int num = -1;
			BrowserEvent instance = BrowserEvent.Instance;
			Dictionary<int, BrowserEvent.BrowserCallback> dictionary = instance.mDictCallback;
			lock (dictionary)
			{
				BrowserEvent.BrowserCallback browserCallback;
				if (instance.mDictCallback.TryGetValue(id, out browserCallback))
				{
					try
					{
						if (browserCallback.mOnLoadErrorCallback != null)
						{
							num = browserCallback.mOnLoadErrorCallback(id, errorCode);
						}
					}
					catch (Exception)
					{
					}
				}
			}
			return num;
		}

		public void setOnBeforeBrowseCallback(int id, BrowserNative.OnBeforeBrowseDelegate callback)
		{
			Dictionary<int, BrowserEvent.BrowserCallback> dictionary = this.mDictCallback;
			lock (dictionary)
			{
				BrowserEvent.BrowserCallback browserCallback;
				if (!this.mDictCallback.TryGetValue(id, out browserCallback))
				{
					browserCallback = new BrowserEvent.BrowserCallback();
					this.mDictCallback.Add(id, browserCallback);
				}
				browserCallback.mOnBeforeBrowseCallback = callback;
				BrowserNative.ucef_callback_setOnBeforeBrowse(id, new BrowserNative.OnBeforeBrowseDelegate(BrowserEvent.onBeforeBrowse));
			}
		}

		public void setOnCertificateErrorCallback(int id, BrowserNative.OnCertificateErrorDelegate callback)
		{
			Dictionary<int, BrowserEvent.BrowserCallback> dictionary = this.mDictCallback;
			lock (dictionary)
			{
				BrowserEvent.BrowserCallback browserCallback;
				if (!this.mDictCallback.TryGetValue(id, out browserCallback))
				{
					browserCallback = new BrowserEvent.BrowserCallback();
					this.mDictCallback.Add(id, browserCallback);
				}
				browserCallback.mOnCertificateErrorCallback = callback;
				BrowserNative.ucef_callback_setOnCertificateError(id, new BrowserNative.OnCertificateErrorDelegate(BrowserEvent.onCertificateError));
			}
		}

		public void setOnRenderProcessTerminatedCallback(int id, BrowserNative.OnRenderProcessTerminatedDelegate callback)
		{
			Dictionary<int, BrowserEvent.BrowserCallback> dictionary = this.mDictCallback;
			lock (dictionary)
			{
				BrowserEvent.BrowserCallback browserCallback;
				if (!this.mDictCallback.TryGetValue(id, out browserCallback))
				{
					browserCallback = new BrowserEvent.BrowserCallback();
					this.mDictCallback.Add(id, browserCallback);
				}
				browserCallback.mOnRenderProcessTerminatedCallback = callback;
				BrowserNative.ucef_callback_setOnRenderProcessTerminated(id, new BrowserNative.OnRenderProcessTerminatedDelegate(BrowserEvent.onRenderProcessTerminated));
			}
		}

		public void setOnJavaScriptEvaluatedCallback(int id, BrowserNative.OnJavaScriptEvaluatedDelegate callback)
		{
			Dictionary<int, BrowserEvent.BrowserCallback> dictionary = this.mDictCallback;
			lock (dictionary)
			{
				BrowserEvent.BrowserCallback browserCallback;
				if (!this.mDictCallback.TryGetValue(id, out browserCallback))
				{
					browserCallback = new BrowserEvent.BrowserCallback();
					this.mDictCallback.Add(id, browserCallback);
				}
				browserCallback.mOnJavaScriptEvaluatedCallback = callback;
				BrowserNative.ucef_callback_setOnJavaScriptEvaluated(id, new BrowserNative.OnJavaScriptEvaluatedDelegate(BrowserEvent.onJavaScriptEvaluated));
			}
		}

		private static int onBeforeBrowse(int id, string url)
		{
			int num = 0;
			BrowserEvent instance = BrowserEvent.Instance;
			Dictionary<int, BrowserEvent.BrowserCallback> dictionary = instance.mDictCallback;
			lock (dictionary)
			{
				BrowserEvent.BrowserCallback browserCallback;
				if (instance.mDictCallback.TryGetValue(id, out browserCallback))
				{
					try
					{
						if (browserCallback.mOnBeforeBrowseCallback != null)
						{
							num = browserCallback.mOnBeforeBrowseCallback(id, url);
						}
					}
					catch (Exception)
					{
					}
				}
			}
			return num;
		}

		private static int onCertificateError(int id, int errorCode, uint certStatus)
		{
			int num = 0;
			BrowserEvent instance = BrowserEvent.Instance;
			Dictionary<int, BrowserEvent.BrowserCallback> dictionary = instance.mDictCallback;
			lock (dictionary)
			{
				BrowserEvent.BrowserCallback browserCallback;
				if (instance.mDictCallback.TryGetValue(id, out browserCallback))
				{
					try
					{
						if (browserCallback.mOnCertificateErrorCallback != null)
						{
							num = browserCallback.mOnCertificateErrorCallback(id, errorCode, certStatus);
						}
					}
					catch (Exception)
					{
					}
				}
			}
			return num;
		}

		private static int onRenderProcessTerminated(int id, int status)
		{
			int num = -1;
			BrowserEvent instance = BrowserEvent.Instance;
			Dictionary<int, BrowserEvent.BrowserCallback> dictionary = instance.mDictCallback;
			lock (dictionary)
			{
				BrowserEvent.BrowserCallback browserCallback;
				if (instance.mDictCallback.TryGetValue(id, out browserCallback))
				{
					try
					{
						if (browserCallback.mOnRenderProcessTerminatedCallback != null)
						{
							num = browserCallback.mOnRenderProcessTerminatedCallback(id, status);
						}
					}
					catch (Exception)
					{
					}
				}
			}
			return num;
		}

		private static int onJavaScriptEvaluated(int id, int funcResult, string evalResult, string funcIdentifier)
		{
			int num = 0;
			BrowserEvent instance = BrowserEvent.Instance;
			Dictionary<int, BrowserEvent.BrowserCallback> dictionary = instance.mDictCallback;
			lock (dictionary)
			{
				BrowserEvent.BrowserCallback browserCallback;
				if (instance.mDictCallback.TryGetValue(id, out browserCallback))
				{
					try
					{
						if (browserCallback.mOnJavaScriptEvaluatedCallback != null)
						{
							num = browserCallback.mOnJavaScriptEvaluatedCallback(id, funcResult, evalResult, funcIdentifier);
						}
					}
					catch (Exception)
					{
					}
				}
			}
			return num;
		}

		public void PollEvent()
		{
			uint num = 0U;
			int num2 = 0;
			BrowserNative.ucef_callback_check(ref num, ref num2);
			if (num != 0U)
			{
				uint num3 = num;
				if ((num3 & 1U) != 0U)
				{
					num3 &= 4294967294U;
					BrowserNative.ucef_callback_readOnContextInitialized();
					BrowserNative.ucef_callback_writeOnContextInitialized(0);
					return;
				}
				if ((num3 & 8U) != 0U)
				{
					num3 &= 4294967287U;
					BrowserNative.ucef_callback_readOnCrashed();
					int num4 = 0;
					try
					{
						num4 = BrowserEvent.onCrashed();
					}
					catch (Exception)
					{
					}
					BrowserNative.ucef_callback_writeOnCrashed(num4);
					return;
				}
				if ((num3 & 32U) != 0U)
				{
					num3 &= 4294967263U;
					int num5 = -1;
					BrowserNative.ucef_callback_readOnAfterCreated(ref num5);
					BrowserNative.ucef_callback_writeOnAfterCreated(0);
					return;
				}
				if ((num3 & 64U) != 0U)
				{
					num3 &= 4294967231U;
					int num6 = -1;
					BrowserNative.ucef_callback_readOnBeforeClose(ref num6);
					int num7 = 0;
					try
					{
						num7 = BrowserEvent.onBeforeClose(num6);
					}
					catch (Exception)
					{
					}
					BrowserNative.ucef_callback_writeOnBeforeClose(num7);
					return;
				}
				if ((num3 & 256U) != 0U)
				{
					num3 &= 4294967039U;
					int num8 = -1;
					int num9 = -1;
					BrowserNative.ucef_callback_readOnLoadingStateChange(ref num8, ref num9);
					int num10 = 0;
					try
					{
						num10 = BrowserEvent.onLoadingStateChange(num8, num9);
					}
					catch (Exception)
					{
					}
					BrowserNative.ucef_callback_writeOnLoadingStateChange(num10);
					return;
				}
				if ((num3 & 512U) != 0U)
				{
					num3 &= 4294966783U;
					int num11 = -1;
					int num12 = -1;
					BrowserNative.ucef_callback_readOnLoadStart(ref num11, ref num12);
					int num13 = 0;
					try
					{
						num13 = BrowserEvent.onLoadStart(num11, num12);
					}
					catch (Exception)
					{
					}
					BrowserNative.ucef_callback_writeOnLoadStart(num13);
					return;
				}
				if ((num3 & 1024U) != 0U)
				{
					num3 &= 4294966271U;
					int num14 = -1;
					IntPtr intPtr = IntPtr.Zero;
					int num15 = -1;
					string text = string.Empty;
					intPtr = Marshal.AllocHGlobal(4096);
					BrowserNative.ucef_callback_readOnLoadEnd(ref num14, intPtr, ref num15, num2);
					text = Marshal.PtrToStringUni(intPtr);
					Marshal.FreeHGlobal(intPtr);
					int num16 = 0;
					try
					{
						num16 = BrowserEvent.onLoadEnd(num14, text, num15);
					}
					catch (Exception)
					{
					}
					BrowserNative.ucef_callback_writeOnLoadEnd(num16);
					return;
				}
				if ((num3 & 2048U) != 0U)
				{
					num3 &= 4294965247U;
					int num17 = -1;
					int num18 = -1;
					BrowserNative.ucef_callback_readOnLoadError(ref num17, ref num18);
					int num19 = 0;
					try
					{
						num19 = BrowserEvent.onLoadError(num17, num18);
					}
					catch (Exception)
					{
					}
					BrowserNative.ucef_callback_writeOnLoadError(num19);
					return;
				}
				if ((num3 & 4096U) != 0U)
				{
					num3 &= 4294963199U;
					int num20 = -1;
					IntPtr intPtr2 = IntPtr.Zero;
					intPtr2 = Marshal.AllocHGlobal(1024);
					string text2 = string.Empty;
					BrowserNative.ucef_callback_readOnBeforeBrowse(ref num20, intPtr2, num2);
					text2 = Marshal.PtrToStringUni(intPtr2);
					Marshal.FreeHGlobal(intPtr2);
					int num21 = 0;
					try
					{
						num21 = BrowserEvent.onBeforeBrowse(num20, text2);
					}
					catch (Exception)
					{
					}
					BrowserNative.ucef_callback_writeOnBeforeBrowse(num21);
					return;
				}
				if ((num3 & 16384U) != 0U)
				{
					num3 &= 4294950911U;
					int num22 = -1;
					int num23 = -1;
					BrowserNative.ucef_callback_readOnRenderProcessTerminated(ref num22, ref num23);
					int num24 = 0;
					try
					{
						num24 = BrowserEvent.onRenderProcessTerminated(num22, num23);
					}
					catch (Exception)
					{
					}
					BrowserNative.ucef_callback_writeOnRenderProcessTerminated(num24);
					return;
				}
				if ((num3 & 8192U) != 0U)
				{
					num3 &= 4294959103U;
					int num25 = -1;
					int num26 = -1;
					uint num27 = 0U;
					BrowserNative.ucef_callback_readOnCertificateError(ref num25, ref num26, ref num27);
					int num28 = 0;
					try
					{
						num28 = BrowserEvent.onCertificateError(num25, num26, num27);
					}
					catch (Exception)
					{
					}
					BrowserNative.ucef_callback_writeOnCertificateError(num28);
					return;
				}
				if ((num3 & 65536U) != 0U)
				{
					num3 &= 4294901759U;
					int num29 = -1;
					int num30 = -1;
					StringBuilder stringBuilder = new StringBuilder(2048);
					StringBuilder stringBuilder2 = new StringBuilder(128);
					BrowserNative.ucef_callback_readOnJavaScriptEvaluated(ref num29, ref num30, stringBuilder, stringBuilder.Capacity, stringBuilder2, num2);
					int num31 = 0;
					try
					{
						num31 = BrowserEvent.onJavaScriptEvaluated(num29, num30, stringBuilder.ToString(), stringBuilder2.ToString());
					}
					catch (Exception)
					{
					}
					BrowserNative.ucef_callback_writeOnJavaScriptEvaluated(num31);
				}
			}
		}

		private BrowserNative.OnCrashedDelegate mOnCrashedCallback;

		private readonly Dictionary<int, BrowserEvent.BrowserCallback> mDictCallback = new Dictionary<int, BrowserEvent.BrowserCallback>();

		private class BrowserCallback
		{
			public BrowserNative.OnBeforeCloseDelegate mOnBeforeCloseCallback;

			public BrowserNative.OnLoadingStateChangeDelegate mOnLoadingStateChangeCallback;

			public BrowserNative.OnLoadStartDelegate mOnLoadStartCallback;

			public BrowserNative.OnLoadEndDelegate mOnLoadEndCallback;

			public BrowserNative.OnLoadErrorDelegate mOnLoadErrorCallback;

			public BrowserNative.OnBeforeBrowseDelegate mOnBeforeBrowseCallback;

			public BrowserNative.OnCertificateErrorDelegate mOnCertificateErrorCallback;

			public BrowserNative.OnRenderProcessTerminatedDelegate mOnRenderProcessTerminatedCallback;

			public BrowserNative.OnJavaScriptEvaluatedDelegate mOnJavaScriptEvaluatedCallback;
		}
	}
}
