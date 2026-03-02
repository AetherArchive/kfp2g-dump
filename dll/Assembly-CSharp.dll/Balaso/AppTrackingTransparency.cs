using System;
using System.Threading.Tasks;

namespace Balaso
{
	// Token: 0x0200057F RID: 1407
	public static class AppTrackingTransparency
	{
		// Token: 0x1700063D RID: 1597
		// (get) Token: 0x06002E8F RID: 11919 RVA: 0x001B295C File Offset: 0x001B0B5C
		public static AppTrackingTransparency.AuthorizationStatus TrackingAuthorizationStatus
		{
			get
			{
				return AppTrackingTransparency.AuthorizationStatus.NOT_DETERMINED;
			}
		}

		// Token: 0x06002E90 RID: 11920 RVA: 0x001B295F File Offset: 0x001B0B5F
		public static void UpdateConversionValue(int value)
		{
		}

		// Token: 0x06002E91 RID: 11921 RVA: 0x001B2961 File Offset: 0x001B0B61
		public static void RegisterAppForAdNetworkAttribution()
		{
		}

		// Token: 0x06002E92 RID: 11922 RVA: 0x001B2963 File Offset: 0x001B0B63
		public static void RequestTrackingAuthorization()
		{
		}

		// Token: 0x06002E93 RID: 11923 RVA: 0x001B2965 File Offset: 0x001B0B65
		public static string IdentifierForAdvertising()
		{
			return null;
		}

		// Token: 0x040028E6 RID: 10470
		private static TaskScheduler currentSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext();

		// Token: 0x040028E7 RID: 10471
		public static Action<AppTrackingTransparency.AuthorizationStatus> OnAuthorizationRequestDone;

		// Token: 0x020010F3 RID: 4339
		public enum AuthorizationStatus
		{
			// Token: 0x04005DAE RID: 23982
			NOT_DETERMINED,
			// Token: 0x04005DAF RID: 23983
			RESTRICTED,
			// Token: 0x04005DB0 RID: 23984
			DENIED,
			// Token: 0x04005DB1 RID: 23985
			AUTHORIZED
		}
	}
}
