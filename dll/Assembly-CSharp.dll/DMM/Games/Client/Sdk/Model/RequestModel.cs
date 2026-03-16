using System;
using UnityEngine;

namespace DMM.Games.Client.Sdk.Model
{
	[Serializable]
	public class RequestModel
	{
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

		public string ToJson()
		{
			return JsonUtility.ToJson(this);
		}

		public string access_token;

		public string sdk_version;

		public string viewer_id;

		public string onetime_token;

		public string command;
	}
}
