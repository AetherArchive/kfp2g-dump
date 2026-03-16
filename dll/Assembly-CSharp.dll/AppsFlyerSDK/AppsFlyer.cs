using System;
using System.Collections.Generic;
using AFMiniJSON;
using UnityEngine;

namespace AppsFlyerSDK
{
	public class AppsFlyer : MonoBehaviour
	{
		public static void initSDK(string devKey, string appID)
		{
			AppsFlyer.initSDK(devKey, appID, null);
		}

		public static void initSDK(string devKey, string appID, MonoBehaviour gameObject)
		{
			if (gameObject != null)
			{
				AppsFlyer.CallBackObjectName = gameObject.name;
			}
		}

		public static void startSDK()
		{
		}

		public static void sendEvent(string eventName, Dictionary<string, string> eventValues)
		{
		}

		public static void stopSDK(bool isSDKStopped)
		{
		}

		public static bool isSDKStopped()
		{
			return false;
		}

		public static string getSdkVersion()
		{
			return "";
		}

		public static void setIsDebug(bool shouldEnable)
		{
		}

		public static void setCustomerUserId(string id)
		{
		}

		public static void setAppInviteOneLinkID(string oneLinkId)
		{
		}

		public static void setAdditionalData(Dictionary<string, string> customData)
		{
		}

		public static void setResolveDeepLinkURLs(params string[] urls)
		{
		}

		public static void setOneLinkCustomDomain(params string[] domains)
		{
		}

		public static void setCurrencyCode(string currencyCode)
		{
		}

		public static void recordLocation(double latitude, double longitude)
		{
		}

		public static void anonymizeUser(bool shouldAnonymizeUser)
		{
		}

		public static string getAppsFlyerId()
		{
			return "";
		}

		public static void setMinTimeBetweenSessions(int seconds)
		{
		}

		public static void setHost(string hostPrefixName, string hostName)
		{
		}

		public static void setUserEmails(EmailCryptType cryptMethod, params string[] emails)
		{
		}

		public static void setPhoneNumber(string phoneNumber)
		{
		}

		public static void setSharingFilterForAllPartners()
		{
		}

		public static void setSharingFilter(params string[] partners)
		{
		}

		public static void getConversionData(string objectName)
		{
		}

		public static void attributeAndOpenStore(string appID, string campaign, Dictionary<string, string> userParams, MonoBehaviour gameObject)
		{
		}

		public static void recordCrossPromoteImpression(string appID, string campaign, Dictionary<string, string> parameters)
		{
		}

		public static void generateUserInviteLink(Dictionary<string, string> parameters, MonoBehaviour gameObject)
		{
		}

		public static void addPushNotificationDeepLinkPath(params string[] paths)
		{
		}

		public static void subscribeForDeepLink()
		{
		}

		public static event EventHandler OnRequestResponse
		{
			add
			{
				AppsFlyer.onRequestResponse = (EventHandler)Delegate.Combine(AppsFlyer.onRequestResponse, value);
			}
			remove
			{
				AppsFlyer.onRequestResponse = (EventHandler)Delegate.Remove(AppsFlyer.onRequestResponse, value);
			}
		}

		public static event EventHandler OnInAppResponse
		{
			add
			{
				AppsFlyer.onInAppResponse = (EventHandler)Delegate.Combine(AppsFlyer.onInAppResponse, value);
			}
			remove
			{
				AppsFlyer.onInAppResponse = (EventHandler)Delegate.Remove(AppsFlyer.onInAppResponse, value);
			}
		}

		public static event EventHandler OnDeepLinkReceived
		{
			add
			{
				AppsFlyer.onDeepLinkReceived = (EventHandler)Delegate.Combine(AppsFlyer.onDeepLinkReceived, value);
				AppsFlyer.subscribeForDeepLink();
			}
			remove
			{
				AppsFlyer.onDeepLinkReceived = (EventHandler)Delegate.Remove(AppsFlyer.onDeepLinkReceived, value);
			}
		}

		public void inAppResponseReceived(string response)
		{
			if (AppsFlyer.onInAppResponse != null)
			{
				AppsFlyer.onInAppResponse(null, AppsFlyer.parseRequestCallback(response));
			}
		}

		public void requestResponseReceived(string response)
		{
			if (AppsFlyer.onRequestResponse != null)
			{
				AppsFlyer.onRequestResponse(null, AppsFlyer.parseRequestCallback(response));
			}
		}

		public void onDeepLinking(string response)
		{
			DeepLinkEventsArgs deepLinkEventsArgs = new DeepLinkEventsArgs(response);
			if (AppsFlyer.onDeepLinkReceived != null)
			{
				AppsFlyer.onDeepLinkReceived(null, deepLinkEventsArgs);
			}
		}

		private static AppsFlyerRequestEventArgs parseRequestCallback(string response)
		{
			int num = 0;
			string text = "";
			try
			{
				Dictionary<string, object> dictionary = AppsFlyer.CallbackStringToDictionary(response);
				text = (string)(dictionary.ContainsKey("errorDescription") ? dictionary["errorDescription"] : "");
				num = (int)((long)dictionary["statusCode"]);
			}
			catch (Exception ex)
			{
				AppsFlyer.AFLog("parseRequestCallback", string.Format("{0} Exception caught.", ex));
			}
			return new AppsFlyerRequestEventArgs(num, text);
		}

		public static Dictionary<string, object> CallbackStringToDictionary(string str)
		{
			return Json.Deserialize(str) as Dictionary<string, object>;
		}

		public static void AFLog(string methodName, string str)
		{
		}

		public static readonly string kAppsFlyerPluginVersion = "6.2.41";

		public static string CallBackObjectName = null;

		private static EventHandler onRequestResponse;

		private static EventHandler onInAppResponse;

		private static EventHandler onDeepLinkReceived;
	}
}
