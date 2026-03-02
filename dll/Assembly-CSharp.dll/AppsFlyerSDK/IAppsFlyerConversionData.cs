using System;

namespace AppsFlyerSDK
{
	// Token: 0x02000586 RID: 1414
	public interface IAppsFlyerConversionData
	{
		// Token: 0x06002ED3 RID: 11987
		void onConversionDataSuccess(string conversionData);

		// Token: 0x06002ED4 RID: 11988
		void onConversionDataFail(string error);

		// Token: 0x06002ED5 RID: 11989
		void onAppOpenAttribution(string attributionData);

		// Token: 0x06002ED6 RID: 11990
		void onAppOpenAttributionFailure(string error);
	}
}
