using System;

namespace DMM.Games.Client.Sdk.Sample
{
	// Token: 0x0200057D RID: 1405
	public class NetGameApiSdkArgsModel
	{
		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x06002E7B RID: 11899 RVA: 0x001B1DE7 File Offset: 0x001AFFE7
		// (set) Token: 0x06002E7C RID: 11900 RVA: 0x001B1DEF File Offset: 0x001AFFEF
		public string ViewerId { get; protected set; }

		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x06002E7D RID: 11901 RVA: 0x001B1DF8 File Offset: 0x001AFFF8
		// (set) Token: 0x06002E7E RID: 11902 RVA: 0x001B1E00 File Offset: 0x001B0000
		public string AccessToken { get; protected set; }

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x06002E7F RID: 11903 RVA: 0x001B1E09 File Offset: 0x001B0009
		// (set) Token: 0x06002E80 RID: 11904 RVA: 0x001B1E11 File Offset: 0x001B0011
		public string OnetimeToken { get; protected set; }

		// Token: 0x06002E81 RID: 11905 RVA: 0x001B1E1A File Offset: 0x001B001A
		public NetGameApiSdkArgsModel()
		{
			this.ViewerId = string.Empty;
			this.OnetimeToken = string.Empty;
			this.AccessToken = string.Empty;
		}

		// Token: 0x06002E82 RID: 11906 RVA: 0x001B1E43 File Offset: 0x001B0043
		public static NetGameApiSdkArgsModel Load()
		{
			return NetGameApiSdkArgsModel.Load(NetGameApiSdkArgsModel.DEFAULT_PREFIX);
		}

		// Token: 0x06002E83 RID: 11907 RVA: 0x001B1E4F File Offset: 0x001B004F
		public static NetGameApiSdkArgsModel Load(string prefix)
		{
			return NetGameApiSdkArgsModel.Parse(Environment.GetCommandLineArgs(), prefix);
		}

		// Token: 0x06002E84 RID: 11908 RVA: 0x001B1E5C File Offset: 0x001B005C
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

		// Token: 0x040028D3 RID: 10451
		protected static readonly string DEFAULT_PREFIX = "/";

		// Token: 0x040028D4 RID: 10452
		protected static readonly string KEY_VIEWER_ID = "viewer_id=";

		// Token: 0x040028D5 RID: 10453
		protected static readonly string KEY_ONETIME_TOKEN = "onetime_token=";

		// Token: 0x040028D6 RID: 10454
		protected static readonly string KEY_ACCESS_TOKEN = "access_token=";
	}
}
