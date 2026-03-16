using System;
using System.Collections.Generic;
using System.Threading;
using SGNFW.Common;

namespace SGNFW.Thread
{
	public class ThreadPool : Singleton<ThreadPool>
	{
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

		public void Restart()
		{
			this.SetThreadsMax(ThreadPool.threadsMax_);
		}

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

		public bool IsStop()
		{
			return this.thread_ == null;
		}

		public bool IsProcessDone()
		{
			long executeProcessNum = this.GetExecuteProcessNum();
			long waitProcessNum = this.GetWaitProcessNum();
			return executeProcessNum <= 0L && waitProcessNum <= 0L;
		}

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

		public long GetExecuteProcessNum()
		{
			return Interlocked.Read(ref ThreadPool.execProcessNum_);
		}

		public static AsyncResult<T> Invoke<T>(Func<T> method)
		{
			AsyncResult<T> ar = new AsyncResult<T>();
			Singleton<ThreadPool>.Instance.addMethod_(delegate
			{
				ar.__setResult(method());
			});
			return ar;
		}

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

		private void Start()
		{
			this.SetThreadsMax(4);
		}

		protected override void OnSingletonDestroy()
		{
			this.Stop();
		}

		public const int THREADPOOL_DEFAULT_THREADS_MAX = 4;

		private static int threadsMax_;

		private static long execProcessNum_;

		private Thread[] thread_;

		private static volatile bool threadStopFlag_;

		private Queue<Action> methods_ = new Queue<Action>();

		private object lockObject_ = new object();
	}
}
