using System;
using System.Collections;
using System.Collections.Generic;

namespace Project.Lib
{
	public class TaskList
	{
		public void Add(IEnumerator task)
		{
			this.taskList_.Add(task);
		}

		public void AddRange(List<IEnumerator> task)
		{
			this.taskList_.AddRange(task);
		}

		public void Clear()
		{
			this.taskList_.Clear();
		}

		public bool IsEnd()
		{
			return this.taskList_.Count == 0;
		}

		public int Count()
		{
			return this.taskList_.Count;
		}

		public void Execute()
		{
			while (this.taskList_.Count > 0 && !this.taskList_[0].MoveNext())
			{
				this.taskList_.RemoveAt(0);
			}
		}

		private List<IEnumerator> taskList_ = new List<IEnumerator>();
	}
}
