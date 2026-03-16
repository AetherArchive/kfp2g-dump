using System;

namespace DMM.Games.Client.Sdk.Connection
{
	public class Endpoint
	{
		protected static string GetHost(bool isSandbox)
		{
			if (!isSandbox)
			{
				return Endpoint.PRODUCTION;
			}
			return Endpoint.SANDBOX;
		}

		public static string GetUpdateToken(bool isSandbox)
		{
			return Endpoint.GetHost(isSandbox) + "/updateToken";
		}

		public static string GetRequest(bool isSandbox)
		{
			return Endpoint.GetHost(isSandbox) + "/request";
		}

		private static readonly string PRODUCTION = "https://sdk-gameplayer.dmm.com/api/sdk";

		private static readonly string SANDBOX = "https://sbx-sdk-gameplayer.dmm.com/api/sdk";
	}
}
