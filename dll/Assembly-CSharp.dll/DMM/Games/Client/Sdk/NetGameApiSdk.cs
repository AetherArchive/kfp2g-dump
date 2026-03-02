using System;
using DMM.Games.Client.Sdk.Connection;
using DMM.Games.Client.Sdk.Model;
using UnityEngine;

namespace DMM.Games.Client.Sdk
{
	// Token: 0x02000577 RID: 1399
	public class NetGameApiSdk : MonoBehaviour
	{
		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x06002E40 RID: 11840 RVA: 0x001B189F File Offset: 0x001AFA9F
		// (set) Token: 0x06002E41 RID: 11841 RVA: 0x001B18A7 File Offset: 0x001AFAA7
		public bool Sandbox { get; protected set; }

		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x06002E42 RID: 11842 RVA: 0x001B18B0 File Offset: 0x001AFAB0
		// (set) Token: 0x06002E43 RID: 11843 RVA: 0x001B18B8 File Offset: 0x001AFAB8
		public string AccessToken { get; protected set; }

		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x06002E44 RID: 11844 RVA: 0x001B18C1 File Offset: 0x001AFAC1
		// (set) Token: 0x06002E45 RID: 11845 RVA: 0x001B18C9 File Offset: 0x001AFAC9
		public string ViewerId { get; protected set; }

		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x06002E46 RID: 11846 RVA: 0x001B18D2 File Offset: 0x001AFAD2
		// (set) Token: 0x06002E47 RID: 11847 RVA: 0x001B18DA File Offset: 0x001AFADA
		public string OnetimeToken { get; protected set; }

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x06002E48 RID: 11848 RVA: 0x001B18E3 File Offset: 0x001AFAE3
		public bool IsInitialized
		{
			get
			{
				return this.isInitialized;
			}
		}

		// Token: 0x06002E49 RID: 11849 RVA: 0x001B18EC File Offset: 0x001AFAEC
		public void Initialize(string accessToken, string viewerId, string onetimeToken, bool sandbox)
		{
			if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(viewerId) || string.IsNullOrEmpty(onetimeToken))
			{
				throw new ArgumentNullException();
			}
			this.AccessToken = accessToken;
			this.ViewerId = viewerId;
			this.OnetimeToken = onetimeToken;
			this.Sandbox = sandbox;
			this.isInitialized = true;
		}

		// Token: 0x06002E4A RID: 11850 RVA: 0x001B193C File Offset: 0x001AFB3C
		public void UpdateToken(NetGameApiSdk.SuccessCallback success, NetGameApiSdk.FailureCallback failure)
		{
			if (success == null || failure == null)
			{
				throw new ArgumentNullException();
			}
			string accessToken = this.AccessToken;
			lock (accessToken)
			{
				RequestModel requestModel = this.getRequestModel(NetGameApiSdk.Kind.UpdateToken);
				NetgameApiConnector.CreateUpdateToken(this, requestModel).Send(delegate(string json)
				{
					NetGameApiResult netGameApiResult = NetGameApiResult.Parse(json);
					if (netGameApiResult.IsSuccess())
					{
						this.updateAccessToken(netGameApiResult.AccessToken);
						this.updateOneTimeToken(netGameApiResult.OnetimeToken);
						success(NetGameApiSdk.Kind.UpdateToken, this, netGameApiResult);
						return;
					}
					failure(NetGameApiSdk.Kind.UpdateToken, this, netGameApiResult.GetErrorCode());
				}, delegate(string error)
				{
					failure(NetGameApiSdk.Kind.UpdateToken, this, NetGameApiSdk.ErrorCode.NetworkErrorAccessNetGameApi);
				});
			}
		}

		// Token: 0x06002E4B RID: 11851 RVA: 0x001B19D4 File Offset: 0x001AFBD4
		public void CheckLogin(NetGameApiSdk.SuccessCallback success, NetGameApiSdk.FailureCallback failure)
		{
			this.Request(success, failure, NetGameApiSdk.Kind.CheckLogin, this.getRequestModel(NetGameApiSdk.Kind.CheckLogin));
		}

		// Token: 0x06002E4C RID: 11852 RVA: 0x001B19E6 File Offset: 0x001AFBE6
		public void GetPoint(NetGameApiSdk.SuccessCallback success, NetGameApiSdk.FailureCallback failure)
		{
			this.Request(success, failure, NetGameApiSdk.Kind.GetPoint, this.getRequestModel(NetGameApiSdk.Kind.GetPoint));
		}

		// Token: 0x06002E4D RID: 11853 RVA: 0x001B19F8 File Offset: 0x001AFBF8
		public void GetChip(NetGameApiSdk.SuccessCallback success, NetGameApiSdk.FailureCallback failure)
		{
			this.Request(success, failure, NetGameApiSdk.Kind.GetChip, this.getRequestModel(NetGameApiSdk.Kind.GetChip));
		}

		// Token: 0x06002E4E RID: 11854 RVA: 0x001B1A0A File Offset: 0x001AFC0A
		public void GetProfile(NetGameApiSdk.SuccessCallback success, NetGameApiSdk.FailureCallback failure)
		{
			this.Request(success, failure, NetGameApiSdk.Kind.GetProfile, this.getRequestModel(NetGameApiSdk.Kind.GetProfile));
		}

		// Token: 0x06002E4F RID: 11855 RVA: 0x001B1A1C File Offset: 0x001AFC1C
		public void Payment(string itemId, string itemName, int unitPrice, int quantity, string callbackurl, string finishurl, NetGameApiSdk.SuccessCallback success, NetGameApiSdk.FailureCallback failure)
		{
			if (string.IsNullOrEmpty(itemId) || string.IsNullOrEmpty(itemName) || string.IsNullOrEmpty(callbackurl) || string.IsNullOrEmpty(finishurl))
			{
				throw new ArgumentNullException();
			}
			PaymentRequestModel paymentRequestModel = this.getRequestModel(NetGameApiSdk.Kind.Payment) as PaymentRequestModel;
			paymentRequestModel.itemId = itemId;
			paymentRequestModel.itemName = itemName;
			paymentRequestModel.unitPrice = unitPrice;
			paymentRequestModel.quantity = quantity;
			paymentRequestModel.callbackurl = callbackurl;
			paymentRequestModel.finishurl = finishurl;
			this.Request(success, failure, NetGameApiSdk.Kind.Payment, paymentRequestModel);
		}

		// Token: 0x06002E50 RID: 11856 RVA: 0x001B1A98 File Offset: 0x001AFC98
		protected void Request(NetGameApiSdk.SuccessCallback success, NetGameApiSdk.FailureCallback failure, NetGameApiSdk.Kind kind, RequestModel request)
		{
			if (success == null || failure == null)
			{
				throw new ArgumentNullException();
			}
			string accessToken = this.AccessToken;
			lock (accessToken)
			{
				NetgameApiConnector.CreateRequest(this, request).Send(delegate(string json)
				{
					NetGameApiResult netGameApiResult = NetGameApiResult.Parse(json);
					if (netGameApiResult.IsSuccess())
					{
						success(kind, this, netGameApiResult);
						return;
					}
					failure(kind, this, netGameApiResult.GetErrorCode());
				}, delegate(string error)
				{
					failure(kind, this, NetGameApiSdk.ErrorCode.NetworkErrorAccessNetGameApi);
				});
			}
		}

		// Token: 0x06002E51 RID: 11857 RVA: 0x001B1B30 File Offset: 0x001AFD30
		protected bool updateAccessToken(string newAccessToken)
		{
			if (string.IsNullOrEmpty(newAccessToken))
			{
				return false;
			}
			this.AccessToken = newAccessToken;
			return true;
		}

		// Token: 0x06002E52 RID: 11858 RVA: 0x001B1B44 File Offset: 0x001AFD44
		protected bool updateOneTimeToken(string newOneTimeToken)
		{
			if (string.IsNullOrEmpty(newOneTimeToken))
			{
				return false;
			}
			this.OnetimeToken = newOneTimeToken;
			return true;
		}

		// Token: 0x06002E53 RID: 11859 RVA: 0x001B1B58 File Offset: 0x001AFD58
		protected RequestModel getRequestModel(NetGameApiSdk.Kind kind)
		{
			RequestModel requestModel = RequestModel.Create(kind);
			requestModel.sdk_version = NetGameApiSdk.SdkVersion;
			requestModel.access_token = this.AccessToken;
			requestModel.viewer_id = this.ViewerId;
			requestModel.onetime_token = this.OnetimeToken;
			return requestModel;
		}

		// Token: 0x040028B4 RID: 10420
		public static readonly string SdkVersion = "1";

		// Token: 0x040028B9 RID: 10425
		private bool isInitialized;

		// Token: 0x020010EC RID: 4332
		public enum ErrorCode
		{
			// Token: 0x04005D79 RID: 23929
			Success,
			// Token: 0x04005D7A RID: 23930
			Error = 100,
			// Token: 0x04005D7B RID: 23931
			OneTimeTokenExpired,
			// Token: 0x04005D7C RID: 23932
			RequiredLogin,
			// Token: 0x04005D7D RID: 23933
			NetworkErrorAccessCallbackUrl,
			// Token: 0x04005D7E RID: 23934
			UserInfoGetError,
			// Token: 0x04005D7F RID: 23935
			AccessTokenParameterError = 110,
			// Token: 0x04005D80 RID: 23936
			AccessTokenUpdateError,
			// Token: 0x04005D81 RID: 23937
			AccessTokenFatalError,
			// Token: 0x04005D82 RID: 23938
			NetworkErrorAccessNetGameApi,
			// Token: 0x04005D83 RID: 23939
			ParameterError = 200,
			// Token: 0x04005D84 RID: 23940
			ApplicationIdentifierValidationError = 203,
			// Token: 0x04005D85 RID: 23941
			OneTimeTokenValidationError,
			// Token: 0x04005D86 RID: 23942
			CallbackUrlValidationError,
			// Token: 0x04005D87 RID: 23943
			FinishUrlValidationError,
			// Token: 0x04005D88 RID: 23944
			OrderIdValidationError,
			// Token: 0x04005D89 RID: 23945
			ItemIdValidationError,
			// Token: 0x04005D8A RID: 23946
			ItemNameValidationError,
			// Token: 0x04005D8B RID: 23947
			ItemPriceValidationError,
			// Token: 0x04005D8C RID: 23948
			ItemQuantityValidationError,
			// Token: 0x04005D8D RID: 23949
			ItemValidationError,
			// Token: 0x04005D8E RID: 23950
			PointBalanceShortageError = 400,
			// Token: 0x04005D8F RID: 23951
			OrderIdentifierIssueError,
			// Token: 0x04005D90 RID: 23952
			ItemInfoConfirmError,
			// Token: 0x04005D91 RID: 23953
			CanNotCompletePointCharge,
			// Token: 0x04005D92 RID: 23954
			CanNotGetPointBalance,
			// Token: 0x04005D93 RID: 23955
			CanNotLaunchGamePlayer = 500,
			// Token: 0x04005D94 RID: 23956
			DatabaseObstacleError = 900,
			// Token: 0x04005D95 RID: 23957
			Maintenance,
			// Token: 0x04005D96 RID: 23958
			UnknownError = 999
		}

		// Token: 0x020010ED RID: 4333
		public enum Kind
		{
			// Token: 0x04005D98 RID: 23960
			Initialize = 1,
			// Token: 0x04005D99 RID: 23961
			UpdateToken,
			// Token: 0x04005D9A RID: 23962
			CheckLogin,
			// Token: 0x04005D9B RID: 23963
			GetPoint,
			// Token: 0x04005D9C RID: 23964
			GetChip,
			// Token: 0x04005D9D RID: 23965
			GetProfile,
			// Token: 0x04005D9E RID: 23966
			Payment
		}

		// Token: 0x020010EE RID: 4334
		// (Invoke) Token: 0x0600542F RID: 21551
		public delegate void SuccessCallback(NetGameApiSdk.Kind kind, NetGameApiSdk sender, NetGameApiResult result);

		// Token: 0x020010EF RID: 4335
		// (Invoke) Token: 0x06005433 RID: 21555
		public delegate void FailureCallback(NetGameApiSdk.Kind kind, NetGameApiSdk sender, NetGameApiSdk.ErrorCode error);
	}
}
