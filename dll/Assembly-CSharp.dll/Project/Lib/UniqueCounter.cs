using System;

namespace Project.Lib
{
	// Token: 0x0200056B RID: 1387
	public class UniqueCounter
	{
		// Token: 0x06002DF9 RID: 11769 RVA: 0x001B0F05 File Offset: 0x001AF105
		public UniqueCounter()
		{
			this.counter_ = 0;
		}

		// Token: 0x06002DFA RID: 11770 RVA: 0x001B0F14 File Offset: 0x001AF114
		public int GetUniqueId()
		{
			this.counter_++;
			return this.counter_;
		}

		// Token: 0x04002886 RID: 10374
		private int counter_;
	}
}
