using System;
using System.Collections;
using System.Text;
using DMM.Games.Client.Sdk.Model;
using UnityEngine;
using UnityEngine.Networking;

namespace DMM.Games.Client.Sdk.Connection
{
	// Token: 0x0200057C RID: 1404
	public class NetgameApiConnector
	{
		// Token: 0x06002E74 RID: 11892 RVA: 0x001B1D32 File Offset: 0x001AFF32
		public static NetgameApiConnector CreateUpdateToken(NetGameApiSdk sdk, RequestModel model)
		{
			NetgameApiConnector netgameApiConnector = NetgameApiConnector.Create(sdk, Endpoint.GetUpdateToken(sdk.Sandbox));
			netgameApiConnector.SetRequestModel(model);
			return netgameApiConnector;
		}

		// Token: 0x06002E75 RID: 11893 RVA: 0x001B1D4C File Offset: 0x001AFF4C
		public static NetgameApiConnector CreateRequest(NetGameApiSdk sdk, RequestModel model)
		{
			NetgameApiConnector netgameApiConnector = NetgameApiConnector.Create(sdk, Endpoint.GetRequest(sdk.Sandbox));
			netgameApiConnector.SetRequestModel(model);
			return netgameApiConnector;
		}

		// Token: 0x06002E76 RID: 11894 RVA: 0x001B1D66 File Offset: 0x001AFF66
		public static NetgameApiConnector Create(MonoBehaviour sender, string url)
		{
			return new NetgameApiConnector(sender, url);
		}

		// Token: 0x06002E77 RID: 11895 RVA: 0x001B1D6F File Offset: 0x001AFF6F
		protected NetgameApiConnector(MonoBehaviour sender, string url)
		{
			this.sender = sender;
			this.url = url;
		}

		// Token: 0x06002E78 RID: 11896 RVA: 0x001B1D85 File Offset: 0x001AFF85
		public void SetRequestModel(RequestModel model)
		{
			this.requestModel = model;
		}

		// Token: 0x06002E79 RID: 11897 RVA: 0x001B1D8E File Offset: 0x001AFF8E
		public void Send(Action<string> onSuccesCallback, Action<string> onFailedCallback)
		{
			if (this.requestModel == null || onSuccesCallback == null || onFailedCallback == null)
			{
				throw new ArgumentNullException();
			}
			this.sender.StartCoroutine(this.SendCoroutine(this.requestModel.ToJson(), onSuccesCallback, onFailedCallback));
		}

		// Token: 0x06002E7A RID: 11898 RVA: 0x001B1DC3 File Offset: 0x001AFFC3
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

		// Token: 0x040028D0 RID: 10448
		protected MonoBehaviour sender;

		// Token: 0x040028D1 RID: 10449
		protected string url;

		// Token: 0x040028D2 RID: 10450
		protected RequestModel requestModel;
	}
}
