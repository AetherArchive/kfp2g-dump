using System;

namespace AppsFlyerSDK
{
	// Token: 0x02000582 RID: 1410
	public class AppsFlyerRequestEventArgs : EventArgs
	{
		// Token: 0x06002EBF RID: 11967 RVA: 0x001B2B61 File Offset: 0x001B0D61
		public AppsFlyerRequestEventArgs(int code, string description)
		{
			this.statusCode = code;
			this.errorDescription = description;
		}

		// Token: 0x1700063E RID: 1598
		// (get) Token: 0x06002EC0 RID: 11968 RVA: 0x001B2B77 File Offset: 0x001B0D77
		public int statusCode { get; }

		// Token: 0x1700063F RID: 1599
		// (get) Token: 0x06002EC1 RID: 11969 RVA: 0x001B2B7F File Offset: 0x001B0D7F
		public string errorDescription { get; }
	}
}
