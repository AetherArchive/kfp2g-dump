using System;

namespace SGNFW.Thread
{
	// Token: 0x02000240 RID: 576
	public class AsyncResult
	{
		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x0600245D RID: 9309 RVA: 0x0019C348 File Offset: 0x0019A548
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

		// Token: 0x0600245E RID: 9310 RVA: 0x0019C388 File Offset: 0x0019A588
		public void __completed()
		{
			lock (this)
			{
				this.isCompleted_ = true;
			}
		}

		// Token: 0x04001B2C RID: 6956
		private bool isCompleted_;
	}
}
