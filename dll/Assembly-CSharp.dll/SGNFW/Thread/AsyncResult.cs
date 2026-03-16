using System;

namespace SGNFW.Thread
{
	public class AsyncResult<T>
	{
		public T result
		{
			get
			{
				T t;
				lock (this)
				{
					t = this.result_;
				}
				return t;
			}
		}

		public bool isCompleted
		{
			get
			{
				bool flag2;
				lock (this)
				{
					flag2 = this.isCompleted_;
				}
				return flag2;
			}
		}

		public void __setResult(T result)
		{
			lock (this)
			{
				this.result_ = result;
				this.isCompleted_ = true;
			}
		}

		private T result_;

		private bool isCompleted_;
	}
}
