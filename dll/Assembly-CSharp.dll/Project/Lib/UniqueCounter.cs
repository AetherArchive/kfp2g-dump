using System;

namespace Project.Lib
{
	public class UniqueCounter
	{
		public UniqueCounter()
		{
			this.counter_ = 0;
		}

		public int GetUniqueId()
		{
			this.counter_++;
			return this.counter_;
		}

		private int counter_;
	}
}
