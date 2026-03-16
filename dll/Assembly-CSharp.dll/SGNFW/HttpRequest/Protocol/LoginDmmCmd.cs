using System;
using SGNFW.Common.Json;
using SGNFW.Http;

namespace SGNFW.HttpRequest.Protocol
{
	public class LoginDmmCmd : Command
	{
		private LoginDmmCmd()
		{
		}

		private LoginDmmCmd(string version, int lang, string device, string signature, int dmm_viewer_id, string onetime_token)
		{
			this.request = new LoginDmmRequest();
			LoginDmmRequest loginDmmRequest = (LoginDmmRequest)this.request;
			loginDmmRequest.version = version;
			loginDmmRequest.lang = lang;
			loginDmmRequest.device = device;
			loginDmmRequest.signature = signature;
			loginDmmRequest.dmm_viewer_id = dmm_viewer_id;
			loginDmmRequest.onetime_token = onetime_token;
			this.Setting();
		}

		private void Setting()
		{
			base.Url = "LoginDmm.do";
			base.Server = Manager.ServerRoot["sim"];
			base.UserAgent = Manager.UserAgent["sim"];
			base.EncryptKey = Manager.DefaultEncryptKey;
			base.TimeoutTime = 10f;
			base.IsDummy = false;
			base.IsPostMethod = false;
			Manager.Add(this);
		}

		public static LoginDmmCmd Create(string version, int lang, string device, string signature, int dmm_viewer_id, string onetime_token)
		{
			return new LoginDmmCmd(version, lang, device, signature, dmm_viewer_id, onetime_token);
		}

		protected override Response Parse(string __text)
		{
			return PrjJson.FromJson<LoginDmmResponse>(__text);
		}

		protected override string DummyTextPath()
		{
			return "Debug/Texts/Json/Protocol/LoginDmm";
		}
	}
}
