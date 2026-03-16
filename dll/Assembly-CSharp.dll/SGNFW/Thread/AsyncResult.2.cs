using System;

namespace SGNFW.Thread
{
	public class AsyncResult
	{
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

		public void __completed()
		{
			lock (this)
			{
				this.isCompleted_ = true;
			}
		}

		private bool isCompleted_;
	}
}
