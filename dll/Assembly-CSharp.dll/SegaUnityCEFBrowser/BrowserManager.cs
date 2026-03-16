using System;
using System.Collections.Generic;
using System.Threading;

namespace SegaUnityCEFBrowser
{
	public class BrowserManager
	{
		private BrowserManager()
		{
		}

		public static BrowserManager Instance { get; } = new BrowserManager();

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

		public bool IsReady { get; private set; }

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

		public void RequestCreateBrowser()
		{
			if (this.mRequestCreate != null)
			{
				this.mRequestCreate.Set();
			}
		}

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

		private static bool mIsPlaying;

		private Queue<int> mQueueBrowser = new Queue<int>();

		private Thread mThreadCreate;

		private ManualResetEvent mRequestCreate;
	}
}
