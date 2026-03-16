using System;

namespace AppsFlyerSDK
{
	public class AppsFlyerRequestEventArgs : EventArgs
	{
		public AppsFlyerRequestEventArgs(int code, string description)
		{
			this.statusCode = code;
			this.errorDescription = description;
		}

		public int statusCode { get; }

		public string errorDescription { get; }
	}
}
