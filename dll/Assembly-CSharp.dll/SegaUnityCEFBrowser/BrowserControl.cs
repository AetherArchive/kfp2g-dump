using System;
using System.ComponentModel;
using System.Threading;
using UnityEngine;

namespace SegaUnityCEFBrowser
{
	public class BrowserControl : MonoBehaviour
	{
		public static void Create(int numBrowser, bool waitForStartup)
		{
			GameObject gameObject = new GameObject("SUCEF Shutdown");
			gameObject.AddComponent<BrowserControl>();
			Object.DontDestroyOnLoad(gameObject);
			BrowserPump.Instance.Start(numBrowser);
			if (waitForStartup)
			{
				int num = 0;
				while (!BrowserPump.Instance.IsInitialized)
				{
					Thread.Sleep(100);
					num++;
					if (num > 100)
					{
						throw new Win32Exception("Cannot start Browser process.");
					}
				}
			}
		}

		private void OnDestroy()
		{
			BrowserPump.Instance.Stop();
			BrowserManager.Instance.Stop();
			BrowserNative.ucef_term();
		}

		private const int MaxCount = 100;

		private const int WaitInterval = 100;
	}
}
