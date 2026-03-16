using System;
using DMMHelper;
using UnityEngine;

namespace DMM.Games.Client.Sdk.Sample
{
	[ExecuteInEditMode]
	public class NetGameApiSdkSample : MonoBehaviour
	{
		public void OnSuccessCallback(NetGameApiSdk.Kind kind, NetGameApiSdk sender, NetGameApiResult result)
		{
			this.clearLog();
			this.log("OnSuccessCallback");
			this.log("Kind: " + kind.ToString());
			if (kind == NetGameApiSdk.Kind.UpdateToken)
			{
				this.log("OnetimeToken: " + result.OnetimeToken);
				this.log("AccessToken: " + result.AccessToken);
				this.onetimeToken = result.OnetimeToken;
				this.accessToken = result.AccessToken;
				return;
			}
			if (kind == NetGameApiSdk.Kind.CheckLogin)
			{
				this.log("OnetimeToken: " + result.OnetimeToken);
				this.log("InstallStatus: " + result.InstallStatus.ToString());
				return;
			}
			if (kind == NetGameApiSdk.Kind.GetProfile)
			{
				this.log("Nickname: " + result.Nickname);
				return;
			}
			if (kind == NetGameApiSdk.Kind.GetChip)
			{
				this.log("CanUseChip: " + result.CanUseChip.ToString());
				return;
			}
			if (kind == NetGameApiSdk.Kind.GetPoint)
			{
				this.log("CanUsePoint: " + result.CanUsePoint.ToString());
				return;
			}
			if (kind == NetGameApiSdk.Kind.Payment)
			{
				this.log("PaymentId: " + result.PaymentId);
			}
		}

		public void OnFailureCallback(NetGameApiSdk.Kind kind, NetGameApiSdk sender, NetGameApiSdk.ErrorCode error)
		{
			this.clearLog();
			this.log("OnFailureCallback");
			this.log("Kind: " + kind.ToString());
			this.log("Result: " + error.ToString());
		}

		private void OnGUI()
		{
			GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
			GUILayout.Label("<Initialize Arguments>", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Label("ViewerId", Array.Empty<GUILayoutOption>());
			this.viewerId = GUILayout.TextField(this.viewerId, new GUILayoutOption[] { GUILayout.Width(400f) });
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Label("OnetimeToken", Array.Empty<GUILayoutOption>());
			this.onetimeToken = GUILayout.TextField(this.onetimeToken, new GUILayoutOption[] { GUILayout.Width(400f) });
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Label("AccessToken", Array.Empty<GUILayoutOption>());
			this.accessToken = GUILayout.TextField(this.accessToken, new GUILayoutOption[] { GUILayout.Width(400f) });
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Label("Sandbox", Array.Empty<GUILayoutOption>());
			this.isSandbox = GUILayout.Toggle(this.isSandbox, string.Empty, new GUILayoutOption[] { GUILayout.Width(400f) });
			GUILayout.EndHorizontal();
			GUILayout.Label("<Payment Arguments>", Array.Empty<GUILayoutOption>());
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Label("ItemId", Array.Empty<GUILayoutOption>());
			this.paymentItemId = GUILayout.TextField(this.paymentItemId, new GUILayoutOption[] { GUILayout.Width(400f) });
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Label("ItemName", Array.Empty<GUILayoutOption>());
			this.paymentItemName = GUILayout.TextField(this.paymentItemName, new GUILayoutOption[] { GUILayout.Width(400f) });
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Label("UnitPrice", Array.Empty<GUILayoutOption>());
			string text = GUILayout.TextField(this.paymentUnitPrice.ToString(), new GUILayoutOption[] { GUILayout.Width(400f) });
			try
			{
				this.paymentUnitPrice = int.Parse(text);
			}
			catch
			{
				this.paymentUnitPrice = 0;
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Label("Quantity", Array.Empty<GUILayoutOption>());
			string text2 = GUILayout.TextField(this.paymentQuantity.ToString(), new GUILayoutOption[] { GUILayout.Width(400f) });
			try
			{
				this.paymentQuantity = int.Parse(text2);
			}
			catch
			{
				this.paymentQuantity = 0;
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Label("Callbackurl", Array.Empty<GUILayoutOption>());
			this.paymentCallbackurl = GUILayout.TextField(this.paymentCallbackurl, new GUILayoutOption[] { GUILayout.Width(400f) });
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.Label("Finishurl", Array.Empty<GUILayoutOption>());
			this.paymentFinishurl = GUILayout.TextField(this.paymentFinishurl, new GUILayoutOption[] { GUILayout.Width(400f) });
			GUILayout.EndHorizontal();
			GUILayout.Space(20f);
			GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
			GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
			GUILayout.Label("MENU", Array.Empty<GUILayoutOption>());
			if (GUILayout.Button("Load Args", new GUILayoutOption[] { GUILayout.Width(150f) }))
			{
				base.GetComponent<DMMInitializer>().GetLoginResult(delegate(LoginResult result)
				{
					this.viewerId = result.viewer_id;
					this.onetimeToken = result.onetime_token;
					this.accessToken = result.access_token;
					this.clearLog();
					this.log("Success Load Args");
					this.log("ViewerId:" + this.viewerId);
					this.log("OnetimeToken:" + this.onetimeToken);
					this.log("AccessToken:" + this.accessToken);
				}, null);
			}
			if (GUILayout.Button("Initialize", new GUILayoutOption[] { GUILayout.Width(150f) }))
			{
				try
				{
					this.netgameApiSdk.Initialize(this.accessToken, this.viewerId, this.onetimeToken, this.isSandbox);
					this.clearLog();
					this.log("Initialized");
					this.log("IsSandbox:" + this.isSandbox.ToString());
					this.log("ViewerId:" + this.viewerId);
					this.log("OnetimeToken:" + this.onetimeToken);
					this.log("AccessToken:" + this.accessToken);
				}
				catch (Exception ex)
				{
					this.logException(ex.ToString());
				}
			}
			if (this.netgameApiSdk.IsInitialized)
			{
				if (GUILayout.Button("API CheckLogin", new GUILayoutOption[] { GUILayout.Width(150f) }))
				{
					try
					{
						this.netgameApiSdk.CheckLogin(new NetGameApiSdk.SuccessCallback(this.OnSuccessCallback), new NetGameApiSdk.FailureCallback(this.OnFailureCallback));
					}
					catch (Exception ex2)
					{
						this.logException(ex2.ToString());
					}
				}
				if (GUILayout.Button("API UpdateToken", new GUILayoutOption[] { GUILayout.Width(150f) }))
				{
					try
					{
						this.netgameApiSdk.UpdateToken(new NetGameApiSdk.SuccessCallback(this.OnSuccessCallback), new NetGameApiSdk.FailureCallback(this.OnFailureCallback));
					}
					catch (Exception ex3)
					{
						this.logException(ex3.ToString());
					}
				}
				if (GUILayout.Button("API GetProfile", new GUILayoutOption[] { GUILayout.Width(150f) }))
				{
					try
					{
						this.netgameApiSdk.GetProfile(new NetGameApiSdk.SuccessCallback(this.OnSuccessCallback), new NetGameApiSdk.FailureCallback(this.OnFailureCallback));
					}
					catch (Exception ex4)
					{
						this.logException(ex4.ToString());
					}
				}
				if (GUILayout.Button("API GetPoint", new GUILayoutOption[] { GUILayout.Width(150f) }))
				{
					try
					{
						this.netgameApiSdk.GetPoint(new NetGameApiSdk.SuccessCallback(this.OnSuccessCallback), new NetGameApiSdk.FailureCallback(this.OnFailureCallback));
					}
					catch (Exception ex5)
					{
						this.logException(ex5.ToString());
					}
				}
				if (GUILayout.Button("API GetChip", new GUILayoutOption[] { GUILayout.Width(150f) }))
				{
					try
					{
						this.netgameApiSdk.GetChip(new NetGameApiSdk.SuccessCallback(this.OnSuccessCallback), new NetGameApiSdk.FailureCallback(this.OnFailureCallback));
					}
					catch (Exception ex6)
					{
						this.logException(ex6.ToString());
					}
				}
				if (GUILayout.Button("API Payment", new GUILayoutOption[] { GUILayout.Width(150f) }))
				{
					try
					{
						this.netgameApiSdk.Payment(this.paymentItemId, this.paymentItemName, this.paymentUnitPrice, this.paymentQuantity, this.paymentCallbackurl, this.paymentFinishurl, new NetGameApiSdk.SuccessCallback(this.OnSuccessCallback), new NetGameApiSdk.FailureCallback(this.OnFailureCallback));
					}
					catch (Exception ex7)
					{
						this.logException(ex7.ToString());
					}
				}
			}
			GUILayout.EndVertical();
			GUILayout.Space(20f);
			GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
			GUILayout.Label("RESULT", Array.Empty<GUILayoutOption>());
			this.logger = GUILayout.TextArea(this.logger, 1000, new GUILayoutOption[]
			{
				GUILayout.MinWidth(500f),
				GUILayout.ExpandWidth(true),
				GUILayout.MinHeight(200f),
				GUILayout.ExpandHeight(true)
			});
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
		}

		protected void clearLog()
		{
			this.logger = string.Empty;
		}

		protected void log(string message)
		{
			this.logger = this.logger + message + "\n";
		}

		protected void logException(string message)
		{
			this.clearLog();
			this.log("Exception");
			this.log(message);
		}

		[SerializeField]
		protected NetGameApiSdk netgameApiSdk;

		[SerializeField]
		[TextArea(1, 2)]
		protected string viewerId = string.Empty;

		[SerializeField]
		[TextArea(2, 4)]
		protected string onetimeToken = string.Empty;

		[SerializeField]
		[TextArea(5, 10)]
		protected string accessToken = string.Empty;

		[SerializeField]
		protected bool isSandbox;

		[SerializeField]
		protected string paymentItemId = "test1";

		[SerializeField]
		protected string paymentItemName = "テスト1";

		[SerializeField]
		protected int paymentUnitPrice = 100;

		[SerializeField]
		protected int paymentQuantity = 2;

		[SerializeField]
		protected string paymentCallbackurl = "http://www.dmm.com";

		[SerializeField]
		protected string paymentFinishurl = "http://www.dmm.com";

		protected string logger = string.Empty;
	}
}
