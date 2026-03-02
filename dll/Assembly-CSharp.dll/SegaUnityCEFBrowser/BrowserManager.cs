using System;
using System.Collections.Generic;
using System.Threading;

namespace SegaUnityCEFBrowser
{
	// Token: 0x02000206 RID: 518
	public class BrowserManager
	{
		// Token: 0x060021C0 RID: 8640 RVA: 0x00190F4C File Offset: 0x0018F14C
		private BrowserManager()
		{
		}

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x060021C1 RID: 8641 RVA: 0x00190F5F File Offset: 0x0018F15F
		public static BrowserManager Instance { get; } = new BrowserManager();

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x060021C2 RID: 8642 RVA: 0x00190F68 File Offset: 0x0018F168
		public int Count
		{
			get
			{
				int num = 0;
				Queue<int> queue = this.mQueueBrowser;
				lock (queue)
				{
					num = this.mQueueBrowser.Count;
				}
				return num;
			}
		}

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x060021C3 RID: 8643 RVA: 0x00190FB4 File Offset: 0x0018F1B4
		// (set) Token: 0x060021C4 RID: 8644 RVA: 0x00190FBC File Offset: 0x0018F1BC
		public bool IsReady { get; private set; }

		// Token: 0x060021C5 RID: 8645 RVA: 0x00190FC8 File Offset: 0x0018F1C8
		public void Add(int id)
		{
			Queue<int> queue = this.mQueueBrowser;
			lock (queue)
			{
				if (!this.mQueueBrowser.Contains(id))
				{
					this.mQueueBrowser.Enqueue(id);
				}
			}
		}

		// Token: 0x060021C6 RID: 8646 RVA: 0x00191020 File Offset: 0x0018F220
		public int Pop()
		{
			int num = -1;
			if (this.mQueueBrowser.Count <= 0)
			{
				this.RequestCreateBrowser();
			}
			if (BrowserPump.Instance.IsCrashed)
			{
				return num;
			}
			int num2 = 0;
			while (num2 < 30 && 0 >= this.mQueueBrowser.Count)
			{
				Thread.Sleep(500);
				num2++;
			}
			if (this.mQueueBrowser.Count <= 0)
			{
				return -1;
			}
			Queue<int> queue = this.mQueueBrowser;
			lock (queue)
			{
				num = this.mQueueBrowser.Dequeue();
			}
			return num;
		}

		// Token: 0x060021C7 RID: 8647 RVA: 0x001910C0 File Offset: 0x0018F2C0
		public int Front()
		{
			int num = -1;
			Queue<int> queue = this.mQueueBrowser;
			lock (queue)
			{
				num = this.mQueueBrowser.Peek();
			}
			return num;
		}

		// Token: 0x060021C8 RID: 8648 RVA: 0x0019110C File Offset: 0x0018F30C
		public bool IsAlive(int id)
		{
			Queue<int> queue = this.mQueueBrowser;
			lock (queue)
			{
				if (this.mQueueBrowser.Contains(id))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060021C9 RID: 8649 RVA: 0x0019115C File Offset: 0x0018F35C
		public void RequestCreateBrowser()
		{
			if (this.mRequestCreate != null)
			{
				this.mRequestCreate.Set();
			}
		}

		// Token: 0x060021CA RID: 8650 RVA: 0x00191174 File Offset: 0x0018F374
		public void Start(int numBrowser)
		{
			if (this.mThreadCreate != null)
			{
				return;
			}
			BrowserManager.mIsPlaying = true;
			this.mRequestCreate = new ManualResetEvent(false);
			this.mThreadCreate = new Thread(delegate
			{
				int i = 0;
				int num = 256;
				int num2 = 256;
				while (i < numBrowser)
				{
					if (!BrowserManager.mIsPlaying)
					{
						return;
					}
					if (BrowserPump.Instance.IsCrashed)
					{
						break;
					}
					int num3 = -1;
					BrowserNative.ucef_create_browser(ref num3, num, num2, 1, 0U, 1);
					if (0 < num3)
					{
						BrowserManager.Instance.Add(num3);
						BrowserEvent.Instance.setOnBeforeBrowseCallback(num3, null);
						i++;
					}
				}
				this.IsReady = true;
				while (BrowserManager.mIsPlaying)
				{
					try
					{
						if (this.mRequestCreate.WaitOne())
						{
							if (BrowserPump.Instance.IsCrashed)
							{
								this.mRequestCreate.Reset();
							}
							else
							{
								int num4 = 256;
								int num5 = 256;
								int num6 = -1;
								BrowserNative.ucef_create_browser(ref num6, num4, num5, 1, 8421504U, 1);
								if (0 < num6)
								{
									BrowserManager.Instance.Add(num6);
								}
								if (numBrowser <= this.Count)
								{
									this.mRequestCreate.Reset();
								}
								Thread.Sleep(1000);
							}
						}
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
			});
			this.mThreadCreate.Start();
		}

		// Token: 0x060021CB RID: 8651 RVA: 0x001911D4 File Offset: 0x0018F3D4
		public void Stop()
		{
			if (this.mQueueBrowser != null)
			{
				int count = this.Count;
				for (int i = 0; i < count; i++)
				{
					int num = this.Pop();
					if (0 < num)
					{
						BrowserNative.ucef_close_browser(num);
					}
				}
				this.mQueueBrowser = null;
			}
			BrowserManager.mIsPlaying = false;
			if (this.mThreadCreate != null)
			{
				this.mThreadCreate.Abort();
				this.mThreadCreate.Join();
				this.mThreadCreate = null;
			}
			if (this.mRequestCreate != null)
			{
				this.mRequestCreate = null;
			}
			this.IsReady = false;
		}

		// Token: 0x04001872 RID: 6258
		private static bool mIsPlaying;

		// Token: 0x04001873 RID: 6259
		private Queue<int> mQueueBrowser = new Queue<int>();

		// Token: 0x04001874 RID: 6260
		private Thread mThreadCreate;

		// Token: 0x04001875 RID: 6261
		private ManualResetEvent mRequestCreate;
	}
}
