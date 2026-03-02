using System;
using System.Threading;

namespace SegaUnityCEFBrowser
{
	// Token: 0x02000205 RID: 517
	public class BrowserPump
	{
		// Token: 0x060021B6 RID: 8630 RVA: 0x00190E6E File Offset: 0x0018F06E
		private BrowserPump()
		{
		}

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x060021B7 RID: 8631 RVA: 0x00190E76 File Offset: 0x0018F076
		public static BrowserPump Instance { get; } = new BrowserPump();

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x060021B8 RID: 8632 RVA: 0x00190E7D File Offset: 0x0018F07D
		// (set) Token: 0x060021B9 RID: 8633 RVA: 0x00190E85 File Offset: 0x0018F085
		public bool IsInitialized { get; private set; }

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x060021BA RID: 8634 RVA: 0x00190E8E File Offset: 0x0018F08E
		// (set) Token: 0x060021BB RID: 8635 RVA: 0x00190E96 File Offset: 0x0018F096
		public bool IsCrashed { get; private set; }

		// Token: 0x060021BC RID: 8636 RVA: 0x00190EA0 File Offset: 0x0018F0A0
		public void Start(int numBrowser)
		{
			if (this.mThreadPump != null)
			{
				return;
			}
			BrowserPump.mIsPlaying = true;
			this.mThreadPump = new Thread(delegate
			{
				BrowserEvent.Instance.setOnCrashedCallback(new BrowserNative.OnCrashedDelegate(BrowserPump.onCrashed));
				BrowserNative.ucef_init();
				while (BrowserPump.mIsPlaying)
				{
					int num = 0;
					int num2 = BrowserNative.ucef_isReady(ref num);
					if (num != 0)
					{
						this.IsInitialized = true;
						BrowserManager.Instance.Start(numBrowser);
						while (BrowserPump.mIsPlaying)
						{
							try
							{
								if (BrowserPump.Instance.IsCrashed)
								{
									return;
								}
								BrowserEvent.Instance.PollEvent();
							}
							catch (Exception)
							{
							}
							if (!BrowserManager.Instance.IsReady)
							{
								Thread.Sleep(500);
								continue;
							}
							while (BrowserPump.mIsPlaying)
							{
								try
								{
									if (BrowserPump.Instance.IsCrashed)
									{
										Thread.Sleep(10000);
										continue;
									}
									BrowserEvent.Instance.PollEvent();
									Thread.Sleep(10);
									BrowserEvent.Instance.PollEvent();
									BrowserEvent.Instance.PollEvent();
									continue;
								}
								catch (ThreadAbortException)
								{
								}
								catch (Exception)
								{
									continue;
								}
								return;
							}
							return;
						}
						return;
					}
					if (num2 < 0)
					{
						return;
					}
					Thread.Sleep(5000);
				}
			});
			this.mThreadPump.Start();
		}

		// Token: 0x060021BD RID: 8637 RVA: 0x00190EF2 File Offset: 0x0018F0F2
		public void Stop()
		{
			BrowserPump.mIsPlaying = false;
			if (this.mThreadPump != null)
			{
				this.mThreadPump.Abort();
				this.mThreadPump.Join();
				this.mThreadPump = null;
			}
			BrowserNative.ucef_term();
			this.IsInitialized = false;
		}

		// Token: 0x060021BE RID: 8638 RVA: 0x00190F2C File Offset: 0x0018F12C
		private static int onCrashed()
		{
			BrowserPump.Instance.IsCrashed = true;
			BrowserNative.ucef_term();
			return 0;
		}

		// Token: 0x0400186D RID: 6253
		private static bool mIsPlaying;

		// Token: 0x0400186E RID: 6254
		private Thread mThreadPump;
	}
}
