using System;
using System.Threading;

namespace SegaUnityCEFBrowser
{
	public class BrowserPump
	{
		private BrowserPump()
		{
		}

		public static BrowserPump Instance { get; } = new BrowserPump();

		public bool IsInitialized { get; private set; }

		public bool IsCrashed { get; private set; }

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

		private static int onCrashed()
		{
			BrowserPump.Instance.IsCrashed = true;
			BrowserNative.ucef_term();
			return 0;
		}

		private static bool mIsPlaying;

		private Thread mThreadPump;
	}
}
