using System;
using System.Collections;
using DMM.Games.Client.Sdk;
using SGNFW.Common;
using UnityEngine;
using UnityEngine.Events;

namespace DMMHelper
{
	// Token: 0x02000575 RID: 1397
	public class DMMHelpManager : Singleton<DMMHelpManager>
	{
		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x06002E28 RID: 11816 RVA: 0x001B150E File Offset: 0x001AF70E
		// (set) Token: 0x06002E29 RID: 11817 RVA: 0x001B1515 File Offset: 0x001AF715
		public static bool IsSandBox { get; set; } = false;

		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x06002E2A RID: 11818 RVA: 0x001B151D File Offset: 0x001AF71D
		// (set) Token: 0x06002E2B RID: 11819 RVA: 0x001B1525 File Offset: 0x001AF725
		public int VewerID { get; private set; }

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x06002E2C RID: 11820 RVA: 0x001B152E File Offset: 0x001AF72E
		// (set) Token: 0x06002E2D RID: 11821 RVA: 0x001B1536 File Offset: 0x001AF736
		public string AccessToken { get; private set; } = string.Empty;

		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x06002E2E RID: 11822 RVA: 0x001B153F File Offset: 0x001AF73F
		// (set) Token: 0x06002E2F RID: 11823 RVA: 0x001B1547 File Offset: 0x001AF747
		public string OnetimeToken { get; private set; } = string.Empty;

		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x06002E30 RID: 11824 RVA: 0x001B1550 File Offset: 0x001AF750
		// (set) Token: 0x06002E31 RID: 11825 RVA: 0x001B1558 File Offset: 0x001AF758
		public bool IsItializeSuccess { get; private set; }

		// Token: 0x06002E32 RID: 11826 RVA: 0x001B1561 File Offset: 0x001AF761
		public IEnumerator ResolveinItialize()
		{
			if (this.IsItializeSuccess)
			{
				yield break;
			}
			if (this.netgameApiSdk == null)
			{
				this.netgameApiSdk = base.gameObject.AddComponent<NetGameApiSdk>();
			}
			DMMInitializer dmmHelper = base.gameObject.AddComponent<DMMInitializer>();
			bool isOk = false;
			bool isSetup = false;
			dmmHelper.GetLoginResult(delegate(LoginResult result)
			{
				this.AccessToken = result.access_token;
				this.VewerID = int.Parse(result.viewer_id);
				this.OnetimeToken = result.onetime_token;
				isSetup = true;
				isOk = true;
			}, delegate
			{
				isSetup = true;
				isOk = false;
			});
			while (!isSetup)
			{
				yield return null;
			}
			Object.Destroy(dmmHelper);
			if (!isOk)
			{
				CanvasManager.HdlOpenWindowBasic.Setup("エラー", "不正な起動です", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), false, delegate(int index)
				{
					PrjUtil.ForceShutdown();
					return true;
				}, null, false);
				CanvasManager.HdlOpenWindowBasic.Open();
				for (;;)
				{
					yield return null;
				}
			}
			else
			{
				dmmHelper = null;
				this.netgameApiSdk.Initialize(this.AccessToken, this.VewerID.ToString(), this.OnetimeToken, DMMHelpManager.IsSandBox);
				this.apiResultStatus = DMMHelpManager.ApiResultStatus.None;
				this.netgameApiSdk.CheckLogin(new NetGameApiSdk.SuccessCallback(this.OnSuccessCallback), new NetGameApiSdk.FailureCallback(this.OnFailureCallback));
				while (this.apiResultStatus == DMMHelpManager.ApiResultStatus.None)
				{
					yield return null;
				}
				if (this.apiResultStatus == DMMHelpManager.ApiResultStatus.Failure)
				{
					yield break;
				}
				this.nextUpdateTokenTime = TimeManager.SystemNow.Ticks + DMMHelpManager.UpdateTokenInterval;
				this.IsItializeSuccess = true;
				yield break;
			}
		}

		// Token: 0x06002E33 RID: 11827 RVA: 0x001B1570 File Offset: 0x001AF770
		public void GetDmmPoint(UnityAction<int> callback)
		{
			if (!this.IsItializeSuccess)
			{
				return;
			}
			NetGameApiSdk.SuccessCallback successCallback = delegate(NetGameApiSdk.Kind kind, NetGameApiSdk sender, NetGameApiResult result)
			{
				callback(result.CanUsePoint);
			};
			NetGameApiSdk.FailureCallback failureCallback = delegate(NetGameApiSdk.Kind kind, NetGameApiSdk sender, NetGameApiSdk.ErrorCode result)
			{
				callback(-1);
			};
			this.netgameApiSdk.GetPoint(successCallback, failureCallback);
		}

		// Token: 0x06002E34 RID: 11828 RVA: 0x001B15BA File Offset: 0x001AF7BA
		private void OnSuccessCallback(NetGameApiSdk.Kind kind, NetGameApiSdk sender, NetGameApiResult result)
		{
			this.apiResultStatus = DMMHelpManager.ApiResultStatus.Success;
			if (kind == NetGameApiSdk.Kind.UpdateToken)
			{
				this.OnetimeToken = result.OnetimeToken;
				this.AccessToken = result.AccessToken;
				return;
			}
			if (kind != NetGameApiSdk.Kind.CheckLogin)
			{
			}
		}

		// Token: 0x06002E35 RID: 11829 RVA: 0x001B15E8 File Offset: 0x001AF7E8
		private void OnFailureCallback(NetGameApiSdk.Kind kind, NetGameApiSdk sender, NetGameApiSdk.ErrorCode error)
		{
			this.apiResultStatus = DMMHelpManager.ApiResultStatus.Failure;
		}

		// Token: 0x06002E36 RID: 11830 RVA: 0x001B15F4 File Offset: 0x001AF7F4
		private void Update()
		{
			if (!this.IsItializeSuccess)
			{
				return;
			}
			if (TimeManager.SystemNow.Ticks > this.nextUpdateTokenTime)
			{
				this.netgameApiSdk.UpdateToken(new NetGameApiSdk.SuccessCallback(this.OnSuccessCallback), new NetGameApiSdk.FailureCallback(this.OnFailureCallback));
				this.nextUpdateTokenTime = TimeManager.SystemNow.Ticks + DMMHelpManager.UpdateTokenInterval;
			}
			if (this.connectError != null)
			{
				if (!this.connectError.MoveNext())
				{
					this.connectError = null;
					DMMHelpManager.abErrorCnt = 0;
					return;
				}
			}
			else
			{
				if (!CanvasManager.HdlOpenWindowServerError.FinishedClose())
				{
					DMMHelpManager.abErrorCnt = 0;
					return;
				}
				if (DMMHelpManager.abErrorCnt > 3)
				{
					this.connectError = DMMHelpManager.connectErrorAction();
					DMMHelpManager.abErrorCnt = 0;
				}
			}
		}

		// Token: 0x06002E37 RID: 11831 RVA: 0x001B16AA File Offset: 0x001AF8AA
		private static IEnumerator connectErrorAction()
		{
			GameObject owp = Object.Instantiate(Resources.Load("prefab/CmnOpenWindow")) as GameObject;
			SceneManager.AddPanelByBaseCanvas(SceneManager.CanvasType.OVERLAY, owp.transform, true);
			PguiOpenWindowCtrl powc = owp.GetComponent<PguiOpenWindowCtrl>();
			powc.Setup("通信エラー", "通信エラーが発⽣しました。\n電波状況をご確認いただき、再度お試しください。", PguiOpenWindowCtrl.GetButtonPreset(PguiOpenWindowCtrl.PresetType.CLOSE), false, (int idx) => true, null, false);
			powc.ForceOpen();
			while (!powc.FinishedClose())
			{
				yield return null;
			}
			Object.Destroy(owp);
			yield break;
		}

		// Token: 0x040028A6 RID: 10406
		private NetGameApiSdk netgameApiSdk;

		// Token: 0x040028A7 RID: 10407
		private long nextUpdateTokenTime;

		// Token: 0x040028A8 RID: 10408
		private static readonly long UpdateTokenInterval = TimeManager.Second2Tick(3600L);

		// Token: 0x040028AA RID: 10410
		private DMMHelpManager.ApiResultStatus apiResultStatus;

		// Token: 0x040028AF RID: 10415
		public static int abErrorCnt = 0;

		// Token: 0x040028B0 RID: 10416
		private IEnumerator connectError;

		// Token: 0x020010E6 RID: 4326
		private enum ApiResultStatus
		{
			// Token: 0x04005D65 RID: 23909
			None,
			// Token: 0x04005D66 RID: 23910
			Success,
			// Token: 0x04005D67 RID: 23911
			Failure
		}
	}
}
