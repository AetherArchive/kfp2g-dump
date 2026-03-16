using System;
using System.Collections;

namespace Project.Lib
{
	public class Task
	{
		public void Play(IEnumerator task)
		{
			this.task_ = task;
		}

		public void Clear()
		{
			this.task_ = null;
		}

		public bool IsEnd()
		{
			return this.task_ == null;
		}

		public void Execute()
		{
			if (this.task_ == null)
			{
				return;
			}
			if (!this.task_.MoveNext())
			{
				this.task_ = null;
			}
		}

		private IEnumerator task_;
	}
}
