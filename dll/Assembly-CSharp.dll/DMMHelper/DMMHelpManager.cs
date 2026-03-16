using System;
using System.Collections;
using DMM.Games.Client.Sdk;
using SGNFW.Common;
using UnityEngine;
using UnityEngine.Events;

namespace DMMHelper
{
	public class DMMHelpManager : Singleton<DMMHelpManager>
	{
		public static bool IsSandBox { get; set; } = false;

		public int VewerID { get; private set; }

		public string AccessToken { get; private set; } = string.Empty;

		public string OnetimeToken { get; private set; } = string.Empty;

		public bool IsItializeSuccess { get; private set; }

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

		private void OnFailureCallback(NetGameApiSdk.Kind kind, NetGameApiSdk sender, NetGameApiSdk.ErrorCode error)
		{
			this.apiResultStatus = DMMHelpManager.ApiResultStatus.Failure;
		}

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

		private NetGameApiSdk netgameApiSdk;

		private long nextUpdateTokenTime;

		private static readonly long UpdateTokenInterval = TimeManager.Second2Tick(3600L);

		private DMMHelpManager.ApiResultStatus apiResultStatus;

		public static int abErrorCnt = 0;

		private IEnumerator connectError;

		private enum ApiResultStatus
		{
			None,
			Success,
			Failure
		}
	}
}
