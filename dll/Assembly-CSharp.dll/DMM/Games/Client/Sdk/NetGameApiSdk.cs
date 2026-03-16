using System;
using DMM.Games.Client.Sdk.Connection;
using DMM.Games.Client.Sdk.Model;
using UnityEngine;

namespace DMM.Games.Client.Sdk
{
	public class NetGameApiSdk : MonoBehaviour
	{
		public bool Sandbox { get; protected set; }

		public string AccessToken { get; protected set; }

		public string ViewerId { get; protected set; }

		public string OnetimeToken { get; protected set; }

		public bool IsInitialized
		{
			get
			{
				return this.isInitialized;
			}
		}

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

		public void CheckLogin(NetGameApiSdk.SuccessCallback success, NetGameApiSdk.FailureCallback failure)
		{
			this.Request(success, failure, NetGameApiSdk.Kind.CheckLogin, this.getRequestModel(NetGameApiSdk.Kind.CheckLogin));
		}

		public void GetPoint(NetGameApiSdk.SuccessCallback success, NetGameApiSdk.FailureCallback failure)
		{
			this.Request(success, failure, NetGameApiSdk.Kind.GetPoint, this.getRequestModel(NetGameApiSdk.Kind.GetPoint));
		}

		public void GetChip(NetGameApiSdk.SuccessCallback success, NetGameApiSdk.FailureCallback failure)
		{
			this.Request(success, failure, NetGameApiSdk.Kind.GetChip, this.getRequestModel(NetGameApiSdk.Kind.GetChip));
		}

		public void GetProfile(NetGameApiSdk.SuccessCallback success, NetGameApiSdk.FailureCallback failure)
		{
			this.Request(success, failure, NetGameApiSdk.Kind.GetProfile, this.getRequestModel(NetGameApiSdk.Kind.GetProfile));
		}

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

		protected bool updateAccessToken(string newAccessToken)
		{
			if (string.IsNullOrEmpty(newAccessToken))
			{
				return false;
			}
			this.AccessToken = newAccessToken;
			return true;
		}

		protected bool updateOneTimeToken(string newOneTimeToken)
		{
			if (string.IsNullOrEmpty(newOneTimeToken))
			{
				return false;
			}
			this.OnetimeToken = newOneTimeToken;
			return true;
		}

		protected RequestModel getRequestModel(NetGameApiSdk.Kind kind)
		{
			RequestModel requestModel = RequestModel.Create(kind);
			requestModel.sdk_version = NetGameApiSdk.SdkVersion;
			requestModel.access_token = this.AccessToken;
			requestModel.viewer_id = this.ViewerId;
			requestModel.onetime_token = this.OnetimeToken;
			return requestModel;
		}

		public static readonly string SdkVersion = "1";

		private bool isInitialized;

		public enum ErrorCode
		{
			Success,
			Error = 100,
			OneTimeTokenExpired,
			RequiredLogin,
			NetworkErrorAccessCallbackUrl,
			UserInfoGetError,
			AccessTokenParameterError = 110,
			AccessTokenUpdateError,
			AccessTokenFatalError,
			NetworkErrorAccessNetGameApi,
			ParameterError = 200,
			ApplicationIdentifierValidationError = 203,
			OneTimeTokenValidationError,
			CallbackUrlValidationError,
			FinishUrlValidationError,
			OrderIdValidationError,
			ItemIdValidationError,
			ItemNameValidationError,
			ItemPriceValidationError,
			ItemQuantityValidationError,
			ItemValidationError,
			PointBalanceShortageError = 400,
			OrderIdentifierIssueError,
			ItemInfoConfirmError,
			CanNotCompletePointCharge,
			CanNotGetPointBalance,
			CanNotLaunchGamePlayer = 500,
			DatabaseObstacleError = 900,
			Maintenance,
			UnknownError = 999
		}

		public enum Kind
		{
			Initialize = 1,
			UpdateToken,
			CheckLogin,
			GetPoint,
			GetChip,
			GetProfile,
			Payment
		}

		public delegate void SuccessCallback(NetGameApiSdk.Kind kind, NetGameApiSdk sender, NetGameApiResult result);

		public delegate void FailureCallback(NetGameApiSdk.Kind kind, NetGameApiSdk sender, NetGameApiSdk.ErrorCode error);
	}
}
