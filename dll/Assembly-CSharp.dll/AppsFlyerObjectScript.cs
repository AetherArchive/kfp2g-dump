using System;
using AppsFlyerSDK;
using UnityEngine;

public class AppsFlyerObjectScript : MonoBehaviour, IAppsFlyerConversionData
{
	private void Start()
	{
		AppsFlyer.setIsDebug(this.isDebug);
		AppsFlyer.initSDK(this.devKey, this.appID, this.getConversionData ? this : null);
		AppsFlyer.startSDK();
	}

	private void Update()
	{
	}

	public void onConversionDataSuccess(string conversionData)
	{
		AppsFlyer.AFLog("didReceiveConversionData", conversionData);
		AppsFlyer.CallbackStringToDictionary(conversionData);
	}

	public void onConversionDataFail(string error)
	{
		AppsFlyer.AFLog("didReceiveConversionDataWithError", error);
	}

	public void onAppOpenAttribution(string attributionData)
	{
		AppsFlyer.AFLog("onAppOpenAttribution", attributionData);
		AppsFlyer.CallbackStringToDictionary(attributionData);
	}

	public void onAppOpenAttributionFailure(string error)
	{
		AppsFlyer.AFLog("onAppOpenAttributionFailure", error);
	}

	public string devKey;

	public string appID;

	public bool isDebug;

	public bool getConversionData;
}
