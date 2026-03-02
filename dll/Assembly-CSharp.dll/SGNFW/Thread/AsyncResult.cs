using System;

namespace SGNFW.Thread
{
	// Token: 0x0200023F RID: 575
	public class AsyncResult<T>
	{
		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x06002459 RID: 9305 RVA: 0x0019C27C File Offset: 0x0019A47C
		public T result
		{
			get
			{
				T t;
				lock (this)
				{
					t = this.result_;
				}
				return t;
			}
		}

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x0600245A RID: 9306 RVA: 0x0019C2BC File Offset: 0x0019A4BC
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

		// Token: 0x0600245B RID: 9307 RVA: 0x0019C2FC File Offset: 0x0019A4FC
		public void __setResult(T result)
		{
			lock (this)
			{
				this.result_ = result;
				this.isCompleted_ = true;
			}
		}

		// Token: 0x04001B2A RID: 6954
		private T result_;

		// Token: 0x04001B2B RID: 6955
		private bool isCompleted_;
	}
}
