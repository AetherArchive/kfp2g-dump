using System;

namespace DMM.Games.Client.Sdk.Sample
{
	public class NetGameApiSdkArgsModel
	{
		public string ViewerId { get; protected set; }

		public string AccessToken { get; protected set; }

		public string OnetimeToken { get; protected set; }

		public NetGameApiSdkArgsModel()
		{
			this.ViewerId = string.Empty;
			this.OnetimeToken = string.Empty;
			this.AccessToken = string.Empty;
		}

		public static NetGameApiSdkArgsModel Load()
		{
			return NetGameApiSdkArgsModel.Load(NetGameApiSdkArgsModel.DEFAULT_PREFIX);
		}

		public static NetGameApiSdkArgsModel Load(string prefix)
		{
			return NetGameApiSdkArgsModel.Parse(Environment.GetCommandLineArgs(), prefix);
		}

		public static NetGameApiSdkArgsModel Parse(string[] args, string prefix)
		{
			if (args == null || args.Length <= 1)
			{
				return null;
			}
			NetGameApiSdkArgsModel netGameApiSdkArgsModel = new NetGameApiSdkArgsModel();
			for (int i = 1; i < args.Length; i++)
			{
				string text = args[i];
				if (text.StartsWith(prefix))
				{
					text = text.Substring(prefix.Length);
					if (text.StartsWith(NetGameApiSdkArgsModel.KEY_VIEWER_ID))
					{
						netGameApiSdkArgsModel.ViewerId = text.Substring(NetGameApiSdkArgsModel.KEY_VIEWER_ID.Length);
					}
					else if (text.StartsWith(NetGameApiSdkArgsModel.KEY_ONETIME_TOKEN))
					{
						netGameApiSdkArgsModel.OnetimeToken = text.Substring(NetGameApiSdkArgsModel.KEY_ONETIME_TOKEN.Length);
					}
					else if (text.StartsWith(NetGameApiSdkArgsModel.KEY_ACCESS_TOKEN))
					{
						netGameApiSdkArgsModel.AccessToken = text.Substring(NetGameApiSdkArgsModel.KEY_ACCESS_TOKEN.Length);
					}
				}
			}
			return netGameApiSdkArgsModel;
		}

		protected static readonly string DEFAULT_PREFIX = "/";

		protected static readonly string KEY_VIEWER_ID = "viewer_id=";

		protected static readonly string KEY_ONETIME_TOKEN = "onetime_token=";

		protected static readonly string KEY_ACCESS_TOKEN = "access_token=";
	}
}
