using System;
using AppsFlyerSDK;
using UnityEngine;

// Token: 0x02000003 RID: 3
public class AppsFlyerObjectScript : MonoBehaviour, IAppsFlyerConversionData
{
	// Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
	private void Start()
	{
		AppsFlyer.setIsDebug(this.isDebug);
		AppsFlyer.initSDK(this.devKey, this.appID, this.getConversionData ? this : null);
		AppsFlyer.startSDK();
	}

	// Token: 0x06000003 RID: 3 RVA: 0x00002087 File Offset: 0x00000287
	private void Update()
	{
	}

	// Token: 0x06000004 RID: 4 RVA: 0x00002089 File Offset: 0x00000289
	public void onConversionDataSuccess(string conversionData)
	{
		AppsFlyer.AFLog("didReceiveConversionData", conversionData);
		AppsFlyer.CallbackStringToDictionary(conversionData);
	}

	// Token: 0x06000005 RID: 5 RVA: 0x0000209D File Offset: 0x0000029D
	public void onConversionDataFail(string error)
	{
		AppsFlyer.AFLog("didReceiveConversionDataWithError", error);
	}

	// Token: 0x06000006 RID: 6 RVA: 0x000020AA File Offset: 0x000002AA
	public void onAppOpenAttribution(string attributionData)
	{
		AppsFlyer.AFLog("onAppOpenAttribution", attributionData);
		AppsFlyer.CallbackStringToDictionary(attributionData);
	}

	// Token: 0x06000007 RID: 7 RVA: 0x000020BE File Offset: 0x000002BE
	public void onAppOpenAttributionFailure(string error)
	{
		AppsFlyer.AFLog("onAppOpenAttributionFailure", error);
	}

	// Token: 0x0400003E RID: 62
	public string devKey;

	// Token: 0x0400003F RID: 63
	public string appID;

	// Token: 0x04000040 RID: 64
	public bool isDebug;

	// Token: 0x04000041 RID: 65
	public bool getConversionData;
}
