using System;
using DMMHelper;
using UnityEngine;

namespace DMM.Games.Client.Sdk.Sample
{
	// Token: 0x0200057E RID: 1406
	[ExecuteInEditMode]
	public class NetGameApiSdkSample : MonoBehaviour
	{
		// Token: 0x06002E86 RID: 11910 RVA: 0x001B1F40 File Offset: 0x001B0140
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

		// Token: 0x06002E87 RID: 11911 RVA: 0x001B2078 File Offset: 0x001B0278
		public void OnFailureCallback(NetGameApiSdk.Kind kind, NetGameApiSdk sender, NetGameApiSdk.ErrorCode error)
		{
			this.clearLog();
			this.log("OnFailureCallback");
			this.log("Kind: " + kind.ToString());
			this.log("Result: " + error.ToString());
		}

		// Token: 0x06002E88 RID: 11912 RVA: 0x001B20D0 File Offset: 0x001B02D0
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

		// Token: 0x06002E89 RID: 11913 RVA: 0x001B2810 File Offset: 0x001B0A10
		protected void clearLog()
		{
			this.logger = string.Empty;
		}

		// Token: 0x06002E8A RID: 11914 RVA: 0x001B281D File Offset: 0x001B0A1D
		protected void log(string message)
		{
			this.logger = this.logger + message + "\n";
		}

		// Token: 0x06002E8B RID: 11915 RVA: 0x001B2836 File Offset: 0x001B0A36
		protected void logException(string message)
		{
			this.clearLog();
			this.log("Exception");
			this.log(message);
		}

		// Token: 0x040028DA RID: 10458
		[SerializeField]
		protected NetGameApiSdk netgameApiSdk;

		// Token: 0x040028DB RID: 10459
		[SerializeField]
		[TextArea(1, 2)]
		protected string viewerId = string.Empty;

		// Token: 0x040028DC RID: 10460
		[SerializeField]
		[TextArea(2, 4)]
		protected string onetimeToken = string.Empty;

		// Token: 0x040028DD RID: 10461
		[SerializeField]
		[TextArea(5, 10)]
		protected string accessToken = string.Empty;

		// Token: 0x040028DE RID: 10462
		[SerializeField]
		protected bool isSandbox;

		// Token: 0x040028DF RID: 10463
		[SerializeField]
		protected string paymentItemId = "test1";

		// Token: 0x040028E0 RID: 10464
		[SerializeField]
		protected string paymentItemName = "テスト1";

		// Token: 0x040028E1 RID: 10465
		[SerializeField]
		protected int paymentUnitPrice = 100;

		// Token: 0x040028E2 RID: 10466
		[SerializeField]
		protected int paymentQuantity = 2;

		// Token: 0x040028E3 RID: 10467
		[SerializeField]
		protected string paymentCallbackurl = "http://www.dmm.com";

		// Token: 0x040028E4 RID: 10468
		[SerializeField]
		protected string paymentFinishurl = "http://www.dmm.com";

		// Token: 0x040028E5 RID: 10469
		protected string logger = string.Empty;
	}
}
