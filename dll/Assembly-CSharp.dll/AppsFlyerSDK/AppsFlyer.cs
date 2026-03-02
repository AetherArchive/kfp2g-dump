using System;
using System.Collections.Generic;
using AFMiniJSON;
using UnityEngine;

namespace AppsFlyerSDK
{
	// Token: 0x02000580 RID: 1408
	public class AppsFlyer : MonoBehaviour
	{
		// Token: 0x06002E94 RID: 11924 RVA: 0x001B2968 File Offset: 0x001B0B68
		public static void initSDK(string devKey, string appID)
		{
			AppsFlyer.initSDK(devKey, appID, null);
		}

		// Token: 0x06002E95 RID: 11925 RVA: 0x001B2972 File Offset: 0x001B0B72
		public static void initSDK(string devKey, string appID, MonoBehaviour gameObject)
		{
			if (gameObject != null)
			{
				AppsFlyer.CallBackObjectName = gameObject.name;
			}
		}

		// Token: 0x06002E96 RID: 11926 RVA: 0x001B2988 File Offset: 0x001B0B88
		public static void startSDK()
		{
		}

		// Token: 0x06002E97 RID: 11927 RVA: 0x001B298A File Offset: 0x001B0B8A
		public static void sendEvent(string eventName, Dictionary<string, string> eventValues)
		{
		}

		// Token: 0x06002E98 RID: 11928 RVA: 0x001B298C File Offset: 0x001B0B8C
		public static void stopSDK(bool isSDKStopped)
		{
		}

		// Token: 0x06002E99 RID: 11929 RVA: 0x001B298E File Offset: 0x001B0B8E
		public static bool isSDKStopped()
		{
			return false;
		}

		// Token: 0x06002E9A RID: 11930 RVA: 0x001B2991 File Offset: 0x001B0B91
		public static string getSdkVersion()
		{
			return "";
		}

		// Token: 0x06002E9B RID: 11931 RVA: 0x001B2998 File Offset: 0x001B0B98
		public static void setIsDebug(bool shouldEnable)
		{
		}

		// Token: 0x06002E9C RID: 11932 RVA: 0x001B299A File Offset: 0x001B0B9A
		public static void setCustomerUserId(string id)
		{
		}

		// Token: 0x06002E9D RID: 11933 RVA: 0x001B299C File Offset: 0x001B0B9C
		public static void setAppInviteOneLinkID(string oneLinkId)
		{
		}

		// Token: 0x06002E9E RID: 11934 RVA: 0x001B299E File Offset: 0x001B0B9E
		public static void setAdditionalData(Dictionary<string, string> customData)
		{
		}

		// Token: 0x06002E9F RID: 11935 RVA: 0x001B29A0 File Offset: 0x001B0BA0
		public static void setResolveDeepLinkURLs(params string[] urls)
		{
		}

		// Token: 0x06002EA0 RID: 11936 RVA: 0x001B29A2 File Offset: 0x001B0BA2
		public static void setOneLinkCustomDomain(params string[] domains)
		{
		}

		// Token: 0x06002EA1 RID: 11937 RVA: 0x001B29A4 File Offset: 0x001B0BA4
		public static void setCurrencyCode(string currencyCode)
		{
		}

		// Token: 0x06002EA2 RID: 11938 RVA: 0x001B29A6 File Offset: 0x001B0BA6
		public static void recordLocation(double latitude, double longitude)
		{
		}

		// Token: 0x06002EA3 RID: 11939 RVA: 0x001B29A8 File Offset: 0x001B0BA8
		public static void anonymizeUser(bool shouldAnonymizeUser)
		{
		}

		// Token: 0x06002EA4 RID: 11940 RVA: 0x001B29AA File Offset: 0x001B0BAA
		public static string getAppsFlyerId()
		{
			return "";
		}

		// Token: 0x06002EA5 RID: 11941 RVA: 0x001B29B1 File Offset: 0x001B0BB1
		public static void setMinTimeBetweenSessions(int seconds)
		{
		}

		// Token: 0x06002EA6 RID: 11942 RVA: 0x001B29B3 File Offset: 0x001B0BB3
		public static void setHost(string hostPrefixName, string hostName)
		{
		}

		// Token: 0x06002EA7 RID: 11943 RVA: 0x001B29B5 File Offset: 0x001B0BB5
		public static void setUserEmails(EmailCryptType cryptMethod, params string[] emails)
		{
		}

		// Token: 0x06002EA8 RID: 11944 RVA: 0x001B29B7 File Offset: 0x001B0BB7
		public static void setPhoneNumber(string phoneNumber)
		{
		}

		// Token: 0x06002EA9 RID: 11945 RVA: 0x001B29B9 File Offset: 0x001B0BB9
		public static void setSharingFilterForAllPartners()
		{
		}

		// Token: 0x06002EAA RID: 11946 RVA: 0x001B29BB File Offset: 0x001B0BBB
		public static void setSharingFilter(params string[] partners)
		{
		}

		// Token: 0x06002EAB RID: 11947 RVA: 0x001B29BD File Offset: 0x001B0BBD
		public static void getConversionData(string objectName)
		{
		}

		// Token: 0x06002EAC RID: 11948 RVA: 0x001B29BF File Offset: 0x001B0BBF
		public static void attributeAndOpenStore(string appID, string campaign, Dictionary<string, string> userParams, MonoBehaviour gameObject)
		{
		}

		// Token: 0x06002EAD RID: 11949 RVA: 0x001B29C1 File Offset: 0x001B0BC1
		public static void recordCrossPromoteImpression(string appID, string campaign, Dictionary<string, string> parameters)
		{
		}

		// Token: 0x06002EAE RID: 11950 RVA: 0x001B29C3 File Offset: 0x001B0BC3
		public static void generateUserInviteLink(Dictionary<string, string> parameters, MonoBehaviour gameObject)
		{
		}

		// Token: 0x06002EAF RID: 11951 RVA: 0x001B29C5 File Offset: 0x001B0BC5
		public static void addPushNotificationDeepLinkPath(params string[] paths)
		{
		}

		// Token: 0x06002EB0 RID: 11952 RVA: 0x001B29C7 File Offset: 0x001B0BC7
		public static void subscribeForDeepLink()
		{
		}

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06002EB1 RID: 11953 RVA: 0x001B29C9 File Offset: 0x001B0BC9
		// (remove) Token: 0x06002EB2 RID: 11954 RVA: 0x001B29E0 File Offset: 0x001B0BE0
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

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06002EB3 RID: 11955 RVA: 0x001B29F7 File Offset: 0x001B0BF7
		// (remove) Token: 0x06002EB4 RID: 11956 RVA: 0x001B2A0E File Offset: 0x001B0C0E
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

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06002EB5 RID: 11957 RVA: 0x001B2A25 File Offset: 0x001B0C25
		// (remove) Token: 0x06002EB6 RID: 11958 RVA: 0x001B2A41 File Offset: 0x001B0C41
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

		// Token: 0x06002EB7 RID: 11959 RVA: 0x001B2A58 File Offset: 0x001B0C58
		public void inAppResponseReceived(string response)
		{
			if (AppsFlyer.onInAppResponse != null)
			{
				AppsFlyer.onInAppResponse(null, AppsFlyer.parseRequestCallback(response));
			}
		}

		// Token: 0x06002EB8 RID: 11960 RVA: 0x001B2A72 File Offset: 0x001B0C72
		public void requestResponseReceived(string response)
		{
			if (AppsFlyer.onRequestResponse != null)
			{
				AppsFlyer.onRequestResponse(null, AppsFlyer.parseRequestCallback(response));
			}
		}

		// Token: 0x06002EB9 RID: 11961 RVA: 0x001B2A8C File Offset: 0x001B0C8C
		public void onDeepLinking(string response)
		{
			DeepLinkEventsArgs deepLinkEventsArgs = new DeepLinkEventsArgs(response);
			if (AppsFlyer.onDeepLinkReceived != null)
			{
				AppsFlyer.onDeepLinkReceived(null, deepLinkEventsArgs);
			}
		}

		// Token: 0x06002EBA RID: 11962 RVA: 0x001B2AB4 File Offset: 0x001B0CB4
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

		// Token: 0x06002EBB RID: 11963 RVA: 0x001B2B38 File Offset: 0x001B0D38
		public static Dictionary<string, object> CallbackStringToDictionary(string str)
		{
			return Json.Deserialize(str) as Dictionary<string, object>;
		}

		// Token: 0x06002EBC RID: 11964 RVA: 0x001B2B45 File Offset: 0x001B0D45
		public static void AFLog(string methodName, string str)
		{
		}

		// Token: 0x040028E8 RID: 10472
		public static readonly string kAppsFlyerPluginVersion = "6.2.41";

		// Token: 0x040028E9 RID: 10473
		public static string CallBackObjectName = null;

		// Token: 0x040028EA RID: 10474
		private static EventHandler onRequestResponse;

		// Token: 0x040028EB RID: 10475
		private static EventHandler onInAppResponse;

		// Token: 0x040028EC RID: 10476
		private static EventHandler onDeepLinkReceived;
	}
}
