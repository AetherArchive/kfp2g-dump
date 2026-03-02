using System;
using System.Collections.Generic;
using System.Threading;
using SGNFW.Common;

namespace SGNFW.Thread
{
	// Token: 0x02000241 RID: 577
	public class ThreadPool : Singleton<ThreadPool>
	{
		// Token: 0x06002460 RID: 9312 RVA: 0x0019C3CC File Offset: 0x0019A5CC
		public void SetThreadsMax(int threadsMax)
		{
			this.IsProcessDone();
			this.Stop();
			ThreadPool.threadsMax_ = threadsMax;
			this.thread_ = new Thread[threadsMax];
			for (int i = 0; i < this.thread_.Length; i++)
			{
				this.thread_[i] = new Thread(new ThreadStart(this.threadWork_));
				this.thread_[i].Start();
			}
		}

		// Token: 0x06002461 RID: 9313 RVA: 0x0019C431 File Offset: 0x0019A631
		public void Restart()
		{
			this.SetThreadsMax(ThreadPool.threadsMax_);
		}

		// Token: 0x06002462 RID: 9314 RVA: 0x0019C440 File Offset: 0x0019A640
		public void Stop()
		{
			lock (this.lockObject_)
			{
				try
				{
					ThreadPool.threadStopFlag_ = true;
					this.methods_.Clear();
					Monitor.PulseAll(this.lockObject_);
				}
				catch (Exception)
				{
				}
			}
			if (this.thread_ != null)
			{
				for (int i = 0; i < this.thread_.Length; i++)
				{
					if (this.thread_[i] != null)
					{
						this.thread_[i].Join();
						this.thread_[i] = null;
					}
				}
			}
			this.thread_ = null;
			ThreadPool.threadsMax_ = 0;
			ThreadPool.execProcessNum_ = 0L;
			ThreadPool.threadStopFlag_ = false;
		}

		// Token: 0x06002463 RID: 9315 RVA: 0x0019C4FC File Offset: 0x0019A6FC
		public bool IsStop()
		{
			return this.thread_ == null;
		}

		// Token: 0x06002464 RID: 9316 RVA: 0x0019C508 File Offset: 0x0019A708
		public bool IsProcessDone()
		{
			long executeProcessNum = this.GetExecuteProcessNum();
			long waitProcessNum = this.GetWaitProcessNum();
			return executeProcessNum <= 0L && waitProcessNum <= 0L;
		}

		// Token: 0x06002465 RID: 9317 RVA: 0x0019C530 File Offset: 0x0019A730
		public long GetWaitProcessNum()
		{
			long num = 0L;
			lock (this.lockObject_)
			{
				try
				{
					num = (long)this.methods_.Count;
				}
				catch (Exception)
				{
				}
			}
			return num;
		}

		// Token: 0x06002466 RID: 9318 RVA: 0x0019C588 File Offset: 0x0019A788
		public long GetExecuteProcessNum()
		{
			return Interlocked.Read(ref ThreadPool.execProcessNum_);
		}

		// Token: 0x06002467 RID: 9319 RVA: 0x0019C594 File Offset: 0x0019A794
		public static AsyncResult<T> Invoke<T>(Func<T> method)
		{
			AsyncResult<T> ar = new AsyncResult<T>();
			Singleton<ThreadPool>.Instance.addMethod_(delegate
			{
				ar.__setResult(method());
			});
			return ar;
		}

		// Token: 0x06002468 RID: 9320 RVA: 0x0019C5D8 File Offset: 0x0019A7D8
		public static AsyncResult Invoke(Action method)
		{
			AsyncResult ar = new AsyncResult();
			Singleton<ThreadPool>.Instance.addMethod_(delegate
			{
				method();
				ar.__completed();
			});
			return ar;
		}

		// Token: 0x06002469 RID: 9321 RVA: 0x0019C61C File Offset: 0x0019A81C
		private void addMethod_(Action method)
		{
			lock (this.lockObject_)
			{
				try
				{
					this.methods_.Enqueue(method);
					Monitor.PulseAll(this.lockObject_);
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x0600246A RID: 9322 RVA: 0x0019C678 File Offset: 0x0019A878
		private void threadWork_()
		{
			while (!ThreadPool.threadStopFlag_)
			{
				Action action = null;
				lock (this.lockObject_)
				{
					try
					{
						while (this.methods_.Count == 0 && !ThreadPool.threadStopFlag_)
						{
							Monitor.Wait(this.lockObject_);
						}
						if (!ThreadPool.threadStopFlag_)
						{
							Interlocked.Increment(ref ThreadPool.execProcessNum_);
							action = this.methods_.Dequeue();
							Monitor.PulseAll(this.lockObject_);
						}
					}
					catch (Exception)
					{
					}
				}
				if (action != null)
				{
					try
					{
						action();
					}
					catch (Exception)
					{
					}
					Interlocked.Decrement(ref ThreadPool.execProcessNum_);
				}
				else if (!ThreadPool.threadStopFlag_)
				{
					Interlocked.Decrement(ref ThreadPool.execProcessNum_);
				}
			}
		}

		// Token: 0x0600246B RID: 9323 RVA: 0x0019C75C File Offset: 0x0019A95C
		private void Start()
		{
			this.SetThreadsMax(4);
		}

		// Token: 0x0600246C RID: 9324 RVA: 0x0019C765 File Offset: 0x0019A965
		protected override void OnSingletonDestroy()
		{
			this.Stop();
		}

		// Token: 0x04001B2D RID: 6957
		public const int THREADPOOL_DEFAULT_THREADS_MAX = 4;

		// Token: 0x04001B2E RID: 6958
		private static int threadsMax_;

		// Token: 0x04001B2F RID: 6959
		private static long execProcessNum_;

		// Token: 0x04001B30 RID: 6960
		private Thread[] thread_;

		// Token: 0x04001B31 RID: 6961
		private static volatile bool threadStopFlag_;

		// Token: 0x04001B32 RID: 6962
		private Queue<Action> methods_ = new Queue<Action>();

		// Token: 0x04001B33 RID: 6963
		private object lockObject_ = new object();
	}
}
