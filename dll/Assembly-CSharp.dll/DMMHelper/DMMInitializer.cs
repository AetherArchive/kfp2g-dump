using System;
using UnityEngine;

namespace DMMHelper
{
	public class DMMInitializer : MonoBehaviour
	{
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

		private void Update()
		{
			if (this.userFunc != null && this.isValidLoginResult())
			{
				this.userFunc(this.loginResult);
				this.userFunc = null;
				this.failedFunc = null;
			}
		}

		private bool isValidLoginResult()
		{
			return this.loginResult.viewer_id.Length > 0 && this.loginResult.onetime_token.Length > 0 && this.loginResult.access_token.Length > 0;
		}

		public void GetLoginResult(LoginResultRecvFunc func, LoginResultRecvFailedFunc failedFunc)
		{
			this.userFunc = func;
			this.failedFunc = failedFunc;
		}

		private LoginResult loginResult;

		private LoginResultRecvFunc userFunc;

		private LoginResultRecvFailedFunc failedFunc;
	}
}
