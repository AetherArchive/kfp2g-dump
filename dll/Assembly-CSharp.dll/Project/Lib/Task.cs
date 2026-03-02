using System;
using System.Collections;

namespace Project.Lib
{
	// Token: 0x0200056E RID: 1390
	public class Task
	{
		// Token: 0x06002E0D RID: 11789 RVA: 0x001B122C File Offset: 0x001AF42C
		public void Play(IEnumerator task)
		{
			this.task_ = task;
		}

		// Token: 0x06002E0E RID: 11790 RVA: 0x001B1235 File Offset: 0x001AF435
		public void Clear()
		{
			this.task_ = null;
		}

		// Token: 0x06002E0F RID: 11791 RVA: 0x001B123E File Offset: 0x001AF43E
		public bool IsEnd()
		{
			return this.task_ == null;
		}

		// Token: 0x06002E10 RID: 11792 RVA: 0x001B124B File Offset: 0x001AF44B
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

		// Token: 0x0400288F RID: 10383
		private IEnumerator task_;
	}
}
