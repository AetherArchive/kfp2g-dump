using System;
using UnityEngine;

namespace DMMHelper
{
	// Token: 0x02000576 RID: 1398
	public class DMMInitializer : MonoBehaviour
	{
		// Token: 0x06002E3A RID: 11834 RVA: 0x001B16F0 File Offset: 0x001AF8F0
		private void Start()
		{
			this.loginResult.viewer_id = "";
			this.loginResult.onetime_token = "";
			this.loginResult.access_token = "";
			if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.LinuxEditor)
			{
				base.gameObject.AddComponent<DMMHelperClient>().GetLoginResult(delegate(LoginResult result)
				{
					this.loginResult = result;
				});
				return;
			}
			foreach (string text in Environment.GetCommandLineArgs())
			{
				if (text.Contains("viewer_id"))
				{
					this.loginResult.viewer_id = text.Split('=', StringSplitOptions.None)[1];
				}
				else if (text.Contains("onetime_token"))
				{
					this.loginResult.onetime_token = text.Split('=', StringSplitOptions.None)[1];
				}
				else if (text.Contains("access_token"))
				{
					this.loginResult.access_token = text.Split('=', StringSplitOptions.None)[1];
				}
			}
			if (this.failedFunc != null && !this.isValidLoginResult())
			{
				this.failedFunc();
				this.userFunc = null;
				this.failedFunc = null;
			}
		}

		// Token: 0x06002E3B RID: 11835 RVA: 0x001B1810 File Offset: 0x001AFA10
		private void Update()
		{
			if (this.userFunc != null && this.isValidLoginResult())
			{
				this.userFunc(this.loginResult);
				this.userFunc = null;
				this.failedFunc = null;
			}
		}

		// Token: 0x06002E3C RID: 11836 RVA: 0x001B1841 File Offset: 0x001AFA41
		private bool isValidLoginResult()
		{
			return this.loginResult.viewer_id.Length > 0 && this.loginResult.onetime_token.Length > 0 && this.loginResult.access_token.Length > 0;
		}

		// Token: 0x06002E3D RID: 11837 RVA: 0x001B187E File Offset: 0x001AFA7E
		public void GetLoginResult(LoginResultRecvFunc func, LoginResultRecvFailedFunc failedFunc)
		{
			this.userFunc = func;
			this.failedFunc = failedFunc;
		}

		// Token: 0x040028B1 RID: 10417
		private LoginResult loginResult;

		// Token: 0x040028B2 RID: 10418
		private LoginResultRecvFunc userFunc;

		// Token: 0x040028B3 RID: 10419
		private LoginResultRecvFailedFunc failedFunc;
	}
}
