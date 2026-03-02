using System;
using System.ComponentModel;
using System.Threading;
using UnityEngine;

namespace SegaUnityCEFBrowser
{
	// Token: 0x02000208 RID: 520
	public class BrowserControl : MonoBehaviour
	{
		// Token: 0x060021E6 RID: 8678 RVA: 0x00192028 File Offset: 0x00190228
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

		// Token: 0x060021E7 RID: 8679 RVA: 0x00192084 File Offset: 0x00190284
		private void OnDestroy()
		{
			BrowserPump.Instance.Stop();
			BrowserManager.Instance.Stop();
			BrowserNative.ucef_term();
		}

		// Token: 0x0400187B RID: 6267
		private const int MaxCount = 100;

		// Token: 0x0400187C RID: 6268
		private const int WaitInterval = 100;
	}
}
