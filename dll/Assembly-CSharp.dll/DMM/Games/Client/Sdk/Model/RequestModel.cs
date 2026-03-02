using System;
using UnityEngine;

namespace DMM.Games.Client.Sdk.Model
{
	// Token: 0x0200057A RID: 1402
	[Serializable]
	public class RequestModel
	{
		// Token: 0x06002E6C RID: 11884 RVA: 0x001B1C98 File Offset: 0x001AFE98
		public static RequestModel Create(NetGameApiSdk.Kind kind)
		{
			RequestModel requestModel;
			if (kind == NetGameApiSdk.Kind.Payment)
			{
				requestModel = new PaymentRequestModel();
			}
			else
			{
				requestModel = new RequestModel();
			}
			requestModel.command = kind.ToString().ToLower();
			return requestModel;
		}

		// Token: 0x06002E6D RID: 11885 RVA: 0x001B1CD0 File Offset: 0x001AFED0
		public string ToJson()
		{
			return JsonUtility.ToJson(this);
		}

		// Token: 0x040028C9 RID: 10441
		public string access_token;

		// Token: 0x040028CA RID: 10442
		public string sdk_version;

		// Token: 0x040028CB RID: 10443
		public string viewer_id;

		// Token: 0x040028CC RID: 10444
		public string onetime_token;

		// Token: 0x040028CD RID: 10445
		public string command;
	}
}
