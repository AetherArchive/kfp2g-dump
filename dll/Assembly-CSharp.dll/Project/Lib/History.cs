using System;
using System.Collections.Generic;

namespace Project.Lib
{
	// Token: 0x0200056A RID: 1386
	public class History<T>
	{
		// Token: 0x06002DF5 RID: 11765 RVA: 0x001B0E7C File Offset: 0x001AF07C
		public T Next(T currnet)
		{
			if (this.temp_.Count <= 0)
			{
				return currnet;
			}
			this.history_.Push(currnet);
			return this.temp_.Pop();
		}

		// Token: 0x06002DF6 RID: 11766 RVA: 0x001B0EA5 File Offset: 0x001AF0A5
		public T Prev(T currnet)
		{
			if (this.history_.Count <= 0)
			{
				return currnet;
			}
			this.temp_.Push(currnet);
			return this.history_.Pop();
		}

		// Token: 0x06002DF7 RID: 11767 RVA: 0x001B0ECE File Offset: 0x001AF0CE
		public void RecordHistory(T currnet)
		{
			this.history_.Push(currnet);
			this.temp_.Clear();
		}

		// Token: 0x04002884 RID: 10372
		private Stack<T> history_ = new Stack<T>();

		// Token: 0x04002885 RID: 10373
		private Stack<T> temp_ = new Stack<T>();
	}
}
