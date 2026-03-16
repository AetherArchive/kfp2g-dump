using System;
using System.Collections;
using System.Text;
using DMM.Games.Client.Sdk.Model;
using UnityEngine;
using UnityEngine.Networking;

namespace DMM.Games.Client.Sdk.Connection
{
	public class NetgameApiConnector
	{
		public static NetgameApiConnector CreateUpdateToken(NetGameApiSdk sdk, RequestModel model)
		{
			NetgameApiConnector netgameApiConnector = NetgameApiConnector.Create(sdk, Endpoint.GetUpdateToken(sdk.Sandbox));
			netgameApiConnector.SetRequestModel(model);
			return netgameApiConnector;
		}

		public static NetgameApiConnector CreateRequest(NetGameApiSdk sdk, RequestModel model)
		{
			NetgameApiConnector netgameApiConnector = NetgameApiConnector.Create(sdk, Endpoint.GetRequest(sdk.Sandbox));
			netgameApiConnector.SetRequestModel(model);
			return netgameApiConnector;
		}

		public static NetgameApiConnector Create(MonoBehaviour sender, string url)
		{
			return new NetgameApiConnector(sender, url);
		}

		protected NetgameApiConnector(MonoBehaviour sender, string url)
		{
			this.sender = sender;
			this.url = url;
		}

		public void SetRequestModel(RequestModel model)
		{
			this.requestModel = model;
		}

		public void Send(Action<string> onSuccesCallback, Action<string> onFailedCallback)
		{
			if (this.requestModel == null || onSuccesCallback == null || onFailedCallback == null)
			{
				throw new ArgumentNullException();
			}
			this.sender.StartCoroutine(this.SendCoroutine(this.requestModel.ToJson(), onSuccesCallback, onFailedCallback));
		}

		protected IEnumerator SendCoroutine(string requestBody, Action<string> onSuccesCallback, Action<string> onFailedCallback)
		{
			UnityWebRequest request = new UnityWebRequest(this.url, "POST");
			byte[] bytes = Encoding.UTF8.GetBytes(requestBody);
			request.uploadHandler = new UploadHandlerRaw(bytes);
			request.downloadHandler = new DownloadHandlerBuffer();
			request.SetRequestHeader("Content-Type", "application/json");
			yield return request.Send();
			if (!request.isNetworkError)
			{
				onSuccesCallback(request.downloadHandler.text);
			}
			else
			{
				onFailedCallback(request.error);
			}
			yield break;
		}

		protected MonoBehaviour sender;

		protected string url;

		protected RequestModel requestModel;
	}
}
