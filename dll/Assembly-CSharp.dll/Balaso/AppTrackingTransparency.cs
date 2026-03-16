using System;
using System.Threading.Tasks;

namespace Balaso
{
	public static class AppTrackingTransparency
	{
		public static AppTrackingTransparency.AuthorizationStatus TrackingAuthorizationStatus
		{
			get
			{
				return AppTrackingTransparency.AuthorizationStatus.NOT_DETERMINED;
			}
		}

		public static void UpdateConversionValue(int value)
		{
		}

		public static void RegisterAppForAdNetworkAttribution()
		{
		}

		public static void RequestTrackingAuthorization()
		{
		}

		public static string IdentifierForAdvertising()
		{
			return null;
		}

		private static TaskScheduler currentSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext();

		public static Action<AppTrackingTransparency.AuthorizationStatus> OnAuthorizationRequestDone;

		public enum AuthorizationStatus
		{
			NOT_DETERMINED,
			RESTRICTED,
			DENIED,
			AUTHORIZED
		}
	}
}
