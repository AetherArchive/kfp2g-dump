using System;

namespace DMM.Games.Client.Sdk.Connection
{
	// Token: 0x0200057B RID: 1403
	public class Endpoint
	{
		// Token: 0x06002E6F RID: 11887 RVA: 0x001B1CE0 File Offset: 0x001AFEE0
		protected static string GetHost(bool isSandbox)
		{
			if (!isSandbox)
			{
				return Endpoint.PRODUCTION;
			}
			return Endpoint.SANDBOX;
		}

		// Token: 0x06002E70 RID: 11888 RVA: 0x001B1CF0 File Offset: 0x001AFEF0
		public static string GetUpdateToken(bool isSandbox)
		{
			return Endpoint.GetHost(isSandbox) + "/updateToken";
		}

		// Token: 0x06002E71 RID: 11889 RVA: 0x001B1D02 File Offset: 0x001AFF02
		public static string GetRequest(bool isSandbox)
		{
			return Endpoint.GetHost(isSandbox) + "/request";
		}

		// Token: 0x040028CE RID: 10446
		private static readonly string PRODUCTION = "https://sdk-gameplayer.dmm.com/api/sdk";

		// Token: 0x040028CF RID: 10447
		private static readonly string SANDBOX = "https://sbx-sdk-gameplayer.dmm.com/api/sdk";
	}
}
