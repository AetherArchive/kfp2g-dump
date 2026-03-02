using System;

namespace AppsFlyerSDK
{
	// Token: 0x02000588 RID: 1416
	public interface IAppsFlyerValidateReceipt
	{
		// Token: 0x06002EDA RID: 11994
		void didFinishValidateReceipt(string result);

		// Token: 0x06002EDB RID: 11995
		void didFinishValidateReceiptWithError(string error);
	}
}
