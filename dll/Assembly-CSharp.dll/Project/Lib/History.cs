using System;
using System.Collections.Generic;

namespace Project.Lib
{
	public class History<T>
	{
		public T Next(T currnet)
		{
			if (this.temp_.Count <= 0)
			{
				return currnet;
			}
			this.history_.Push(currnet);
			return this.temp_.Pop();
		}

		public T Prev(T currnet)
		{
			if (this.history_.Count <= 0)
			{
				return currnet;
			}
			this.temp_.Push(currnet);
			return this.history_.Pop();
		}

		public void RecordHistory(T currnet)
		{
			this.history_.Push(currnet);
			this.temp_.Clear();
		}

		private Stack<T> history_ = new Stack<T>();

		private Stack<T> temp_ = new Stack<T>();
	}
}
