using System;
using System.Collections;
using System.Collections.Generic;

namespace Project.Lib
{
	// Token: 0x0200056D RID: 1389
	public class TaskList
	{
		// Token: 0x06002E06 RID: 11782 RVA: 0x001B11A0 File Offset: 0x001AF3A0
		public void Add(IEnumerator task)
		{
			this.taskList_.Add(task);
		}

		// Token: 0x06002E07 RID: 11783 RVA: 0x001B11AE File Offset: 0x001AF3AE
		public void AddRange(List<IEnumerator> task)
		{
			this.taskList_.AddRange(task);
		}

		// Token: 0x06002E08 RID: 11784 RVA: 0x001B11BC File Offset: 0x001AF3BC
		public void Clear()
		{
			this.taskList_.Clear();
		}

		// Token: 0x06002E09 RID: 11785 RVA: 0x001B11C9 File Offset: 0x001AF3C9
		public bool IsEnd()
		{
			return this.taskList_.Count == 0;
		}

		// Token: 0x06002E0A RID: 11786 RVA: 0x001B11DB File Offset: 0x001AF3DB
		public int Count()
		{
			return this.taskList_.Count;
		}

		// Token: 0x06002E0B RID: 11787 RVA: 0x001B11E8 File Offset: 0x001AF3E8
		public void Execute()
		{
			while (this.taskList_.Count > 0 && !this.taskList_[0].MoveNext())
			{
				this.taskList_.RemoveAt(0);
			}
		}

		// Token: 0x0400288E RID: 10382
		private List<IEnumerator> taskList_ = new List<IEnumerator>();
	}
}
